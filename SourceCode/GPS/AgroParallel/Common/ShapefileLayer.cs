// ============================================================================
// ShapefileLayer.cs - Capa de outlines de shapefile renderizada sobre oglMain
// Ubicación: SourceCode/GPS/AgroParallel/Common/ShapefileLayer.cs
// Target: net48 (C# 7.3)
//
// Paso 3A del pipeline:
// - Recibe un ShapefileReadResult (polygonos en WGS84 lat/lon).
// - Cachea los rings reproyectados a coords locales (Easting/Northing en metros)
//   usando LocalPlane.ConvertWgs84ToGeoCoord.
// - Expone Draw(LocalPlane) que dibuja outlines con GL.LineLoop.
// - Si el origen de LocalPlane cambia (ej. el usuario abre otro campo), la cache
//   se invalida automaticamente en la siguiente llamada a Draw.
//
// Sin relleno, sin estilo por DBF, sin async. Todo eso queda para pasos
// siguientes. Aca solo validamos que el render + la reproyeccion funcionan.
// ============================================================================

using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;

namespace AgroParallel.Common
{
    public class ShapefileLayer
    {
        public bool IsVisible = true;
        public bool ShowOutline = true;
        public bool ShowFill = true;
        public Color LineColor = Color.FromArgb(0, 200, 255);
        // Alpha 80 (~31%) para que el fill se vea sobre el mapa sin tapar detalles.
        public Color FillColor = Color.FromArgb(80, 0, 200, 255);
        public float LineWidth = 2f;
        public string Source { get; private set; }

        // Ruta absoluta del .shp original (usada para persistir el estado
        // por-campo y auto-recargarlo al reabrir el mismo campo).
        public string SourceFullPath { get; set; }

        // Rings originales en WGS84 (copia compacta: array por ring).
        private readonly List<List<ShapeLatLon[]>> _ringsWgs84 = new List<List<ShapeLatLon[]>>();

        // Cache de rings reproyectados a coords locales (Easting/Northing).
        private readonly List<List<PointF[]>> _ringsLocal = new List<List<PointF[]>>();

        // Atributos DBF por poligono (paralelo a _ringsWgs84).
        private readonly List<Dictionary<string, object>> _polyAttrs = new List<Dictionary<string, object>>();

        // Nombres de campos DBF en orden original.
        private readonly List<string> _fieldNames = new List<string>();

        // Color de fill por poligono si hay estilo por campo aplicado; null = fallback FillColor.
        private Color[] _polyFillColors;

        // Campo actualmente usado para colorear (null = sin estilo por DBF).
        public string StyleField { get; private set; }
        public double StyleMin { get; private set; }
        public double StyleMax { get; private set; }

        private double _cachedOriginLat;
        private double _cachedOriginLon;
        private bool _hasCache;

        public int PolygonCount { get { return _ringsWgs84.Count; } }
        public IReadOnlyList<string> FieldNames { get { return _fieldNames; } }

        public ShapefileLayer(ShapefileReadResult src, string sourceName)
        {
            Source = sourceName;
            if (src == null || src.Polygons == null) return;

            if (src.DbfFieldNames != null)
                _fieldNames.AddRange(src.DbfFieldNames);

            foreach (var poly in src.Polygons)
            {
                if (poly == null || poly.Rings == null) continue;
                var ringsCopy = new List<ShapeLatLon[]>(poly.Rings.Count);
                foreach (var ring in poly.Rings)
                {
                    if (ring == null || ring.Count < 3) continue;
                    ringsCopy.Add(ring.ToArray());
                }
                if (ringsCopy.Count > 0)
                {
                    _ringsWgs84.Add(ringsCopy);
                    _polyAttrs.Add(poly.Attributes ?? new Dictionary<string, object>());
                }
            }
        }

        // Devuelve true si el campo es mayoritariamente numerico (>= 50% de poligonos
        // tienen un valor convertible a double) y rellena min/max/count con las metricas.
        public bool TryGetFieldStats(string fieldName, out double min, out double max, out int count)
        {
            min = double.MaxValue;
            max = double.MinValue;
            count = 0;
            if (string.IsNullOrEmpty(fieldName)) return false;

            for (int i = 0; i < _polyAttrs.Count; i++)
            {
                object raw;
                if (!_polyAttrs[i].TryGetValue(fieldName, out raw)) continue;
                double v;
                if (TryToDouble(raw, out v))
                {
                    count++;
                    if (v < min) min = v;
                    if (v > max) max = v;
                }
            }

            if (count == 0 || _polyAttrs.Count == 0) return false;
            return count * 2 >= _polyAttrs.Count;
        }

        // Aplica un gradiente verde→amarillo→rojo basado en los valores del campo DBF.
        // fieldName == null limpia el estilo y vuelve a FillColor uniforme.
        public bool ApplyColorByField(string fieldName)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                StyleField = null;
                _polyFillColors = null;
                return true;
            }

            double min, max;
            int count;
            if (!TryGetFieldStats(fieldName, out min, out max, out count))
            {
                StyleField = null;
                _polyFillColors = null;
                return false;
            }

            StyleField = fieldName;
            StyleMin = min;
            StyleMax = max;

            double range = max - min;
            if (range < 1e-9) range = 1;

            byte alpha = FillColor.A;
            var cLow = Color.FromArgb(alpha, 0, 200, 0);      // verde
            var cMid = Color.FromArgb(alpha, 255, 220, 0);    // amarillo
            var cHigh = Color.FromArgb(alpha, 220, 40, 0);    // rojo

            _polyFillColors = new Color[_polyAttrs.Count];
            for (int i = 0; i < _polyAttrs.Count; i++)
            {
                object raw;
                double v;
                if (_polyAttrs[i].TryGetValue(fieldName, out raw) && TryToDouble(raw, out v))
                {
                    double t = (v - min) / range;
                    _polyFillColors[i] = Gradient(t, cLow, cMid, cHigh);
                }
                else
                {
                    // Poligono sin valor en el campo → gris semitransparente.
                    _polyFillColors[i] = Color.FromArgb(alpha, 150, 150, 150);
                }
            }
            return true;
        }

        private static bool TryToDouble(object raw, out double v)
        {
            v = 0;
            if (raw == null) return false;
            if (raw is double d) { v = d; return true; }
            if (raw is float f) { v = f; return true; }
            if (raw is int i) { v = i; return true; }
            if (raw is long l) { v = l; return true; }
            if (raw is decimal m) { v = (double)m; return true; }
            string s = Convert.ToString(raw, CultureInfo.InvariantCulture);
            return double.TryParse(s, NumberStyles.Any, CultureInfo.InvariantCulture, out v);
        }

        private static Color Gradient(double t, Color cLow, Color cMid, Color cHigh)
        {
            if (t < 0) t = 0;
            else if (t > 1) t = 1;

            if (t <= 0.5) return Lerp(cLow, cMid, t * 2.0);
            return Lerp(cMid, cHigh, (t - 0.5) * 2.0);
        }

        private static Color Lerp(Color a, Color b, double t)
        {
            int r = (int)Math.Round(a.R + (b.R - a.R) * t);
            int g = (int)Math.Round(a.G + (b.G - a.G) * t);
            int bl = (int)Math.Round(a.B + (b.B - a.B) * t);
            int al = (int)Math.Round(a.A + (b.A - a.A) * t);
            return Color.FromArgb(al, r, g, bl);
        }

        public void Draw(LocalPlane plane)
        {
            if (!IsVisible) return;
            if (plane == null) return;
            if (_ringsWgs84.Count == 0) return;

            EnsureProjected(plane);

            // Fill primero para que el outline quede por encima.
            // Nota: GL.Begin(Polygon) maneja bien poligonos convexos. Con
            // poligonos concavos podrian aparecer artefactos de triangulacion.
            // Los agujeros (Rings[1..n]) se ignoran en el fill — quedan
            // visibles solo como outline.
            if (ShowFill)
            {
                bool hasStyle = _polyFillColors != null;

                for (int p = 0; p < _ringsLocal.Count; p++)
                {
                    var poly = _ringsLocal[p];
                    if (poly.Count == 0) continue;

                    var outer = poly[0];
                    if (outer == null || outer.Length < 3) continue;

                    Color c = (hasStyle && p < _polyFillColors.Length)
                        ? _polyFillColors[p]
                        : FillColor;
                    GL.Color4(c.R, c.G, c.B, c.A);

                    GL.Begin(PrimitiveType.Polygon);
                    for (int i = 0; i < outer.Length; i++)
                        GL.Vertex2(outer[i].X, outer[i].Y);
                    GL.End();
                }
            }

            if (ShowOutline)
            {
                GL.LineWidth(LineWidth);
                GL.Color3(LineColor.R, LineColor.G, LineColor.B);

                for (int p = 0; p < _ringsLocal.Count; p++)
                {
                    var poly = _ringsLocal[p];
                    for (int r = 0; r < poly.Count; r++)
                    {
                        var ring = poly[r];
                        if (ring == null || ring.Length < 3) continue;

                        GL.Begin(PrimitiveType.LineLoop);
                        for (int i = 0; i < ring.Length; i++)
                            GL.Vertex2(ring[i].X, ring[i].Y);
                        GL.End();
                    }
                }
            }
        }

        private void EnsureProjected(LocalPlane plane)
        {
            var origin = plane.Origin;
            if (_hasCache
                && origin.Latitude == _cachedOriginLat
                && origin.Longitude == _cachedOriginLon)
            {
                return;
            }

            _ringsLocal.Clear();

            for (int p = 0; p < _ringsWgs84.Count; p++)
            {
                var polySrc = _ringsWgs84[p];
                var polyDst = new List<PointF[]>(polySrc.Count);

                for (int r = 0; r < polySrc.Count; r++)
                {
                    var ringSrc = polySrc[r];
                    var ringDst = new PointF[ringSrc.Length];
                    for (int i = 0; i < ringSrc.Length; i++)
                    {
                        var gc = plane.ConvertWgs84ToGeoCoord(
                            new Wgs84(ringSrc[i].Lat, ringSrc[i].Lon));
                        ringDst[i] = new PointF((float)gc.Easting, (float)gc.Northing);
                    }
                    polyDst.Add(ringDst);
                }
                _ringsLocal.Add(polyDst);
            }

            _cachedOriginLat = origin.Latitude;
            _cachedOriginLon = origin.Longitude;
            _hasCache = true;
        }
    }
}

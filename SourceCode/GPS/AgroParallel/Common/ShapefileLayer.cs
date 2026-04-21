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

        // Triangulos del contorno exterior por poligono (indices al array Rings[0]).
        // Pre-calculados en EnsureProjected para que GL.Polygon no haga artefactos
        // en poligonos concavos.
        private readonly List<int[]> _outerTriangles = new List<int[]>();

        // Bounding boxes del contorno exterior (pre-filtro en PIP).
        // Formato por poligono: [minE, minN, maxE, maxN].
        private readonly List<float[]> _outerBBox = new List<float[]>();

        // Estado del muestreo por posicion (paso 9A). Actualizado desde FormGPS.
        public int CurrentPolygonIndex { get; private set; } = -1;
        public bool CurrentInside { get; private set; }
        public double CurrentDose { get; private set; }   // valor del StyleField (si hay).
        public bool HasCurrentDose { get; private set; }

        // Lineas del shape (polilineas / MultiLineString expandidas a arrays simples).
        private readonly List<ShapeLatLon[]> _linesWgs84 = new List<ShapeLatLon[]>();
        private readonly List<PointF[]> _linesLocal = new List<PointF[]>();

        // Puntos del shape.
        private readonly List<ShapeLatLon> _pointsWgs84 = new List<ShapeLatLon>();
        private readonly List<PointF> _pointsLocal = new List<PointF>();

        // Estilo para puntos y lineas (fijo por ahora; el gradiente por DBF
        // se aplica solo al fill de poligonos).
        public Color PointColor = Color.FromArgb(255, 255, 80, 200);
        public float PointSize = 6f;

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
        public int LineCount { get { return _linesWgs84.Count; } }
        public int PointCount { get { return _pointsWgs84.Count; } }
        public bool IsEmpty
        {
            get { return PolygonCount == 0 && LineCount == 0 && PointCount == 0; }
        }
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

            if (src.Lines != null)
            {
                foreach (var line in src.Lines)
                {
                    if (line == null || line.Points == null || line.Points.Count < 2) continue;
                    _linesWgs84.Add(line.Points.ToArray());
                }
            }

            if (src.Points != null)
            {
                foreach (var pt in src.Points)
                {
                    if (pt == null) continue;
                    _pointsWgs84.Add(pt.Location);
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

        // Busca el primer poligono cuyo contorno exterior contiene (easting, northing).
        // Requiere que EnsureProjected se haya llamado al menos una vez (lo hace Draw
        // automaticamente). Retorna -1 si el punto no esta en ningun poligono.
        public int FindPolygonAt(double easting, double northing)
        {
            if (_ringsLocal.Count == 0) return -1;

            for (int p = 0; p < _ringsLocal.Count; p++)
            {
                var bbox = p < _outerBBox.Count ? _outerBBox[p] : null;
                if (bbox == null) continue;
                if (easting < bbox[0] || easting > bbox[2]) continue;
                if (northing < bbox[1] || northing > bbox[3]) continue;

                var poly = _ringsLocal[p];
                if (poly.Count == 0) continue;
                var outer = poly[0];
                if (outer == null || outer.Length < 3) continue;

                if (PointInRing(outer, (float)easting, (float)northing))
                    return p;
            }
            return -1;
        }

        // Lee el valor numerico del atributo DBF del poligono dado.
        // Retorna false si no existe, es nulo, o no es convertible a double.
        public bool TryGetPolygonNumeric(int polygonIndex, string fieldName, out double value)
        {
            value = 0;
            if (polygonIndex < 0 || polygonIndex >= _polyAttrs.Count) return false;
            if (string.IsNullOrEmpty(fieldName)) return false;
            object raw;
            if (!_polyAttrs[polygonIndex].TryGetValue(fieldName, out raw)) return false;
            return TryToDouble(raw, out value);
        }

        // Actualiza el estado CurrentInside / CurrentDose con la posicion dada.
        // La posicion viene en las mismas coords locales que usa el mapa
        // (pivotAxlePos.easting / northing de FormGPS).
        public void SamplePosition(double easting, double northing)
        {
            int idx = FindPolygonAt(easting, northing);
            CurrentPolygonIndex = idx;
            CurrentInside = idx >= 0;

            if (idx >= 0 && !string.IsNullOrEmpty(StyleField))
            {
                double v;
                if (TryGetPolygonNumeric(idx, StyleField, out v))
                {
                    CurrentDose = v;
                    HasCurrentDose = true;
                    return;
                }
            }
            CurrentDose = 0;
            HasCurrentDose = false;
        }

        private static bool PointInRing(PointF[] ring, float px, float py)
        {
            // Ray casting clasico. El ring puede tener el ultimo punto igual al
            // primero (shapefile cerrado) — no molesta al algoritmo.
            bool inside = false;
            int n = ring.Length;
            for (int i = 0, j = n - 1; i < n; j = i++)
            {
                float xi = ring[i].X, yi = ring[i].Y;
                float xj = ring[j].X, yj = ring[j].Y;
                bool intersect = ((yi > py) != (yj > py))
                    && (px < (xj - xi) * (py - yi) / (yj - yi) + xi);
                if (intersect) inside = !inside;
            }
            return inside;
        }

        public void Draw(LocalPlane plane)
        {
            if (!IsVisible) return;
            if (plane == null) return;
            if (IsEmpty) return;

            EnsureProjected(plane);

            // Fill primero (triangulado con ear-clipping en EnsureProjected)
            // para que el outline quede por encima. Los agujeros (Rings[1..n])
            // se ignoran en el fill — quedan visibles solo como outline.
            if (ShowFill)
            {
                bool hasStyle = _polyFillColors != null;

                for (int p = 0; p < _ringsLocal.Count; p++)
                {
                    var poly = _ringsLocal[p];
                    if (poly.Count == 0) continue;

                    var outer = poly[0];
                    if (outer == null || outer.Length < 3) continue;

                    int[] tris = p < _outerTriangles.Count ? _outerTriangles[p] : null;
                    if (tris == null || tris.Length < 3) continue;

                    Color c = (hasStyle && p < _polyFillColors.Length)
                        ? _polyFillColors[p]
                        : FillColor;
                    GL.Color4(c.R, c.G, c.B, c.A);

                    GL.Begin(PrimitiveType.Triangles);
                    for (int i = 0; i < tris.Length; i++)
                        GL.Vertex2(outer[tris[i]].X, outer[tris[i]].Y);
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

            // Lineas del shape (tipo PolyLine) — sin cierre, LineStrip.
            if (ShowOutline && _linesLocal.Count > 0)
            {
                GL.LineWidth(LineWidth);
                GL.Color3(LineColor.R, LineColor.G, LineColor.B);

                for (int l = 0; l < _linesLocal.Count; l++)
                {
                    var pts = _linesLocal[l];
                    if (pts == null || pts.Length < 2) continue;

                    GL.Begin(PrimitiveType.LineStrip);
                    for (int i = 0; i < pts.Length; i++)
                        GL.Vertex2(pts[i].X, pts[i].Y);
                    GL.End();
                }
            }

            // Puntos — tamano fijo, color fijo.
            if (_pointsLocal.Count > 0)
            {
                GL.PointSize(PointSize);
                GL.Color4(PointColor.R, PointColor.G, PointColor.B, PointColor.A);

                GL.Begin(PrimitiveType.Points);
                for (int q = 0; q < _pointsLocal.Count; q++)
                    GL.Vertex2(_pointsLocal[q].X, _pointsLocal[q].Y);
                GL.End();
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
            _outerTriangles.Clear();
            _outerBBox.Clear();
            _linesLocal.Clear();
            _pointsLocal.Clear();

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

                // Triangular solo el anillo exterior. Si falla (triangulacion
                // atascada), guardamos array vacio y el fill de ese poligono
                // no se dibuja — el outline se mantiene.
                int[] tris;
                if (polyDst.Count > 0 && polyDst[0] != null && polyDst[0].Length >= 3)
                    tris = EarClipper.Triangulate(polyDst[0]).ToArray();
                else
                    tris = new int[0];
                _outerTriangles.Add(tris);

                // Bounding box del contorno exterior (para acelerar PIP).
                if (polyDst.Count > 0 && polyDst[0] != null && polyDst[0].Length > 0)
                {
                    var outer = polyDst[0];
                    float minE = float.MaxValue, minN = float.MaxValue;
                    float maxE = float.MinValue, maxN = float.MinValue;
                    for (int i = 0; i < outer.Length; i++)
                    {
                        if (outer[i].X < minE) minE = outer[i].X;
                        if (outer[i].X > maxE) maxE = outer[i].X;
                        if (outer[i].Y < minN) minN = outer[i].Y;
                        if (outer[i].Y > maxN) maxN = outer[i].Y;
                    }
                    _outerBBox.Add(new[] { minE, minN, maxE, maxN });
                }
                else
                {
                    _outerBBox.Add(null);
                }
            }

            for (int l = 0; l < _linesWgs84.Count; l++)
            {
                var src = _linesWgs84[l];
                var dst = new PointF[src.Length];
                for (int i = 0; i < src.Length; i++)
                {
                    var gc = plane.ConvertWgs84ToGeoCoord(
                        new Wgs84(src[i].Lat, src[i].Lon));
                    dst[i] = new PointF((float)gc.Easting, (float)gc.Northing);
                }
                _linesLocal.Add(dst);
            }

            for (int q = 0; q < _pointsWgs84.Count; q++)
            {
                var src = _pointsWgs84[q];
                var gc = plane.ConvertWgs84ToGeoCoord(new Wgs84(src.Lat, src.Lon));
                _pointsLocal.Add(new PointF((float)gc.Easting, (float)gc.Northing));
            }

            _cachedOriginLat = origin.Latitude;
            _cachedOriginLon = origin.Longitude;
            _hasCache = true;
        }
    }
}

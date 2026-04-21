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
using System.Collections.Generic;
using System.Drawing;

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

        // Rings originales en WGS84 (copia compacta: array por ring).
        private readonly List<List<ShapeLatLon[]>> _ringsWgs84 = new List<List<ShapeLatLon[]>>();

        // Cache de rings reproyectados a coords locales (Easting/Northing).
        private readonly List<List<PointF[]>> _ringsLocal = new List<List<PointF[]>>();

        private double _cachedOriginLat;
        private double _cachedOriginLon;
        private bool _hasCache;

        public int PolygonCount { get { return _ringsWgs84.Count; } }

        public ShapefileLayer(ShapefileReadResult src, string sourceName)
        {
            Source = sourceName;
            if (src == null || src.Polygons == null) return;

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
                    _ringsWgs84.Add(ringsCopy);
            }
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
                GL.Color4(FillColor.R, FillColor.G, FillColor.B, FillColor.A);

                for (int p = 0; p < _ringsLocal.Count; p++)
                {
                    var poly = _ringsLocal[p];
                    if (poly.Count == 0) continue;

                    var outer = poly[0];
                    if (outer == null || outer.Length < 3) continue;

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

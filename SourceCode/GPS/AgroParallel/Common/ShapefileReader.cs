// ============================================================================
// ShapefileReader.cs - Lector minimo de shapefiles (.shp + .dbf + .prj)
// Ubicación: SourceCode/GPS/AgroParallel/Common/ShapefileReader.cs
// Target: net48 (C# 7.3)
//
// Paso 1 del pipeline de integracion de shapefiles en AgOpenGPS:
// - Solo parsea polygonos (Polygon/MultiPolygon). Puntos y lineas se ignoran.
// - Asume coordenadas WGS84 (X = longitud, Y = latitud). Si el .prj indica
//   otra proyeccion, se loguea como Warning y NO se reproyecta todavia.
// - Sin UI, sin render, sin dependencias a FormGPS.
//
// Proximos pasos: boton en menu -> OpenFileDialog -> llamar a ReadPolygons()
// y pasar el resultado a una capa de render sobre oglMain.
// ============================================================================

using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Esri;
using System;
using System.Collections.Generic;
using System.IO;

namespace AgroParallel.Common
{
    public struct ShapeLatLon
    {
        public double Lat;
        public double Lon;
    }

    public class ShapePolygon
    {
        // Rings[0] = contorno exterior, Rings[1..n] = agujeros.
        public List<List<ShapeLatLon>> Rings = new List<List<ShapeLatLon>>();
        public Dictionary<string, object> Attributes = new Dictionary<string, object>();
    }

    public class ShapefileReadResult
    {
        public List<ShapePolygon> Polygons = new List<ShapePolygon>();
        public List<string> DbfFieldNames = new List<string>();
        public string PrjText;
        public double MinLat = double.MaxValue;
        public double MinLon = double.MaxValue;
        public double MaxLat = double.MinValue;
        public double MaxLon = double.MinValue;
        public List<string> Warnings = new List<string>();
    }

    public static class ShapefileReader
    {
        public static ShapefileReadResult ReadPolygons(string shpPath)
        {
            if (string.IsNullOrWhiteSpace(shpPath))
                throw new ArgumentException("shpPath vacio", "shpPath");
            if (!File.Exists(shpPath))
                throw new FileNotFoundException("No existe el .shp", shpPath);

            var result = new ShapefileReadResult();

            string prjPath = Path.ChangeExtension(shpPath, ".prj");
            if (File.Exists(prjPath))
            {
                try
                {
                    result.PrjText = File.ReadAllText(prjPath);
                    if (!LooksLikeWgs84(result.PrjText))
                    {
                        result.Warnings.Add(
                            ".prj no parece WGS84; las coordenadas se leeran tal cual. "
                            + "Reproyectar externamente o agregar soporte en un paso siguiente.");
                    }
                }
                catch (Exception ex)
                {
                    result.Warnings.Add("No se pudo leer .prj: " + ex.Message);
                }
            }
            else
            {
                result.Warnings.Add("No hay .prj; se asume WGS84 (lon = X, lat = Y).");
            }

            bool fieldsCaptured = false;

            foreach (var feature in Shapefile.ReadAllFeatures(shpPath))
            {
                if (!fieldsCaptured && feature.Attributes != null)
                {
                    foreach (var name in feature.Attributes.GetNames())
                        result.DbfFieldNames.Add(name);
                    fieldsCaptured = true;
                }

                var geom = feature.Geometry;
                if (geom == null) continue;

                if (geom is Polygon polygon)
                {
                    AddPolygon(result, polygon, feature.Attributes);
                }
                else if (geom is MultiPolygon multi)
                {
                    for (int i = 0; i < multi.NumGeometries; i++)
                    {
                        if (multi.GetGeometryN(i) is Polygon p)
                            AddPolygon(result, p, feature.Attributes);
                    }
                }
                // Point / LineString / etc: ignorados en Paso 1.
            }

            if (result.Polygons.Count == 0)
                result.Warnings.Add("No se encontraron poligonos en el shapefile.");

            return result;
        }

        private static void AddPolygon(ShapefileReadResult result, Polygon p, IAttributesTable attrs)
        {
            var poly = new ShapePolygon();
            poly.Rings.Add(ConvertRing(p.ExteriorRing, result));
            if (p.InteriorRings != null)
            {
                foreach (var hole in p.InteriorRings)
                    poly.Rings.Add(ConvertRing(hole, result));
            }

            if (attrs != null)
            {
                foreach (var name in attrs.GetNames())
                {
                    try { poly.Attributes[name] = attrs[name]; }
                    catch { /* atributo problematico: ignorar para no romper el archivo */ }
                }
            }

            result.Polygons.Add(poly);
        }

        private static List<ShapeLatLon> ConvertRing(LineString ring, ShapefileReadResult result)
        {
            var coords = ring.Coordinates;
            var list = new List<ShapeLatLon>(coords.Length);
            for (int i = 0; i < coords.Length; i++)
            {
                double lon = coords[i].X;
                double lat = coords[i].Y;
                list.Add(new ShapeLatLon { Lat = lat, Lon = lon });

                if (lat < result.MinLat) result.MinLat = lat;
                if (lat > result.MaxLat) result.MaxLat = lat;
                if (lon < result.MinLon) result.MinLon = lon;
                if (lon > result.MaxLon) result.MaxLon = lon;
            }
            return list;
        }

        private static bool LooksLikeWgs84(string prj)
        {
            if (string.IsNullOrWhiteSpace(prj)) return false;
            string up = prj.ToUpperInvariant();
            return up.Contains("WGS_1984") || up.Contains("WGS 84") || up.Contains("EPSG\",\"4326");
        }
    }
}

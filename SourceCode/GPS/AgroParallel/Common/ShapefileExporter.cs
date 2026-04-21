// ============================================================================
// ShapefileExporter.cs - Export de la capa cargada a .shp o .kml (paso 13)
// Ubicación: SourceCode/GPS/AgroParallel/Common/ShapefileExporter.cs
// Target: net48 (C# 7.3)
//
// Dos formatos:
//   - Shapefile (.shp + .dbf + .shx + .prj WGS84): solo poligonos, por
//     limitacion de la spec (un shape type por archivo).
//   - KML (.kml): poligonos + lineas + puntos en un solo archivo.
//
// Color del fill por poligono se derivasegun el estilo aplicado actualmente
// (StyleField y _polyFillColors del layer) — solo usado en KML. El shapefile
// exporta solo los datos (la simbologia queda a cargo del software consumidor).
// ============================================================================

using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO.Esri;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Text;

namespace AgroParallel.Common
{
    public static class ShapefileExporter
    {
        // Contenidos WGS84 equivalentes accesibles via reflection interna.
        // Publico el acceso via metodo estatico en ShapefileLayer por limpieza;
        // aca asumimos que el caller nos pasa los rings originales (lat/lon) via
        // IShapefileExportSource — interfaz que ShapefileLayer implementa.

        public static void ExportShapefile(IShapefileExportSource src, string outPath)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (string.IsNullOrWhiteSpace(outPath)) throw new ArgumentException("outPath vacio");

            var factory = GeometryFactory.Default;
            var features = new List<IFeature>();

            for (int p = 0; p < src.PolygonCount; p++)
            {
                var rings = src.GetPolygonRingsWgs84(p);
                if (rings == null || rings.Count == 0) continue;
                if (rings[0] == null || rings[0].Length < 3) continue;

                var outer = factory.CreateLinearRing(ToClosedCoords(rings[0]));
                LinearRing[] holes = null;
                if (rings.Count > 1)
                {
                    holes = new LinearRing[rings.Count - 1];
                    for (int r = 1; r < rings.Count; r++)
                        holes[r - 1] = factory.CreateLinearRing(ToClosedCoords(rings[r]));
                }
                var polygon = factory.CreatePolygon(outer, holes);

                var attrs = new AttributesTable();
                var dict = src.GetPolygonAttributes(p);
                if (dict != null)
                {
                    foreach (var kv in dict)
                    {
                        // AttributesTable solo acepta tipos soportados por DBF;
                        // si el tipo es raro lo pasamos como string.
                        object v = CoerceForDbf(kv.Value);
                        attrs.Add(kv.Key, v);
                    }
                }
                features.Add(new Feature(polygon, attrs));
            }

            if (features.Count == 0)
                throw new InvalidOperationException("La capa no tiene poligonos para exportar.");

            Shapefile.WriteAllFeatures(features, outPath);
            WriteWgs84Prj(Path.ChangeExtension(outPath, ".prj"));
        }

        public static void ExportKml(IShapefileExportSource src, string outPath, string layerName)
        {
            if (src == null) throw new ArgumentNullException("src");
            if (string.IsNullOrWhiteSpace(outPath)) throw new ArgumentException("outPath vacio");

            var sb = new StringBuilder(16 * 1024);
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<kml xmlns=\"http://www.opengis.net/kml/2.2\">");
            sb.AppendLine("  <Document>");
            sb.Append("    <name>").Append(XmlEscape(layerName ?? "ShapefileLayer")).AppendLine("</name>");

            // Estilo por default (usado si no hay color custom por poligono).
            sb.AppendLine("    <Style id=\"def\">");
            sb.AppendLine("      <LineStyle><color>ff00c8ff</color><width>2</width></LineStyle>");
            sb.AppendLine("      <PolyStyle><color>5000c8ff</color></PolyStyle>");
            sb.AppendLine("    </Style>");

            for (int p = 0; p < src.PolygonCount; p++)
            {
                var rings = src.GetPolygonRingsWgs84(p);
                if (rings == null || rings.Count == 0) continue;
                if (rings[0] == null || rings[0].Length < 3) continue;

                var c = src.GetPolygonFillColor(p);
                string styleBlock = null;
                string styleId = null;
                if (c.HasValue)
                {
                    styleId = "s" + p;
                    styleBlock = BuildInlineStyle(styleId, c.Value);
                }

                sb.AppendLine("    <Placemark>");
                sb.Append("      <name>Pol ").Append(p).AppendLine("</name>");

                var attrs = src.GetPolygonAttributes(p);
                sb.Append("      <description><![CDATA[").Append(AttrsAsHtml(attrs)).AppendLine("]]></description>");

                if (styleBlock != null) sb.Append("      ").AppendLine(styleBlock);
                sb.Append("      <styleUrl>#").Append(styleId ?? "def").AppendLine("</styleUrl>");

                sb.AppendLine("      <Polygon>");
                sb.AppendLine("        <outerBoundaryIs><LinearRing><coordinates>");
                WriteKmlCoords(sb, rings[0]);
                sb.AppendLine("        </coordinates></LinearRing></outerBoundaryIs>");
                for (int r = 1; r < rings.Count; r++)
                {
                    sb.AppendLine("        <innerBoundaryIs><LinearRing><coordinates>");
                    WriteKmlCoords(sb, rings[r]);
                    sb.AppendLine("        </coordinates></LinearRing></innerBoundaryIs>");
                }
                sb.AppendLine("      </Polygon>");
                sb.AppendLine("    </Placemark>");
            }

            for (int l = 0; l < src.LineCount; l++)
            {
                var pts = src.GetLinePointsWgs84(l);
                if (pts == null || pts.Length < 2) continue;
                sb.AppendLine("    <Placemark>");
                sb.Append("      <name>Line ").Append(l).AppendLine("</name>");
                sb.AppendLine("      <styleUrl>#def</styleUrl>");
                sb.AppendLine("      <LineString><coordinates>");
                WriteKmlCoords(sb, pts);
                sb.AppendLine("      </coordinates></LineString>");
                sb.AppendLine("    </Placemark>");
            }

            for (int q = 0; q < src.PointCount; q++)
            {
                var pt = src.GetPointWgs84(q);
                sb.AppendLine("    <Placemark>");
                sb.Append("      <name>Pt ").Append(q).AppendLine("</name>");
                sb.AppendLine("      <styleUrl>#def</styleUrl>");
                sb.Append("      <Point><coordinates>");
                sb.Append(pt.Lon.ToString("G17", CultureInfo.InvariantCulture)).Append(',');
                sb.Append(pt.Lat.ToString("G17", CultureInfo.InvariantCulture)).Append(",0");
                sb.AppendLine("</coordinates></Point>");
                sb.AppendLine("    </Placemark>");
            }

            sb.AppendLine("  </Document>");
            sb.AppendLine("</kml>");
            File.WriteAllText(outPath, sb.ToString(), Encoding.UTF8);
        }

        private static void WriteKmlCoords(StringBuilder sb, ShapeLatLon[] pts)
        {
            sb.Append("        ");
            for (int i = 0; i < pts.Length; i++)
            {
                sb.Append(pts[i].Lon.ToString("G17", CultureInfo.InvariantCulture)).Append(',');
                sb.Append(pts[i].Lat.ToString("G17", CultureInfo.InvariantCulture)).Append(",0");
                if (i + 1 < pts.Length) sb.Append(' ');
            }
            sb.AppendLine();
        }

        private static string BuildInlineStyle(string id, Color c)
        {
            // KML usa aabbggrr (alfa, B, G, R en hex).
            string fill = string.Format(CultureInfo.InvariantCulture,
                "{0:x2}{1:x2}{2:x2}{3:x2}", c.A, c.B, c.G, c.R);
            string line = string.Format(CultureInfo.InvariantCulture,
                "ff{0:x2}{1:x2}{2:x2}", c.B, c.G, c.R);
            return string.Format(CultureInfo.InvariantCulture,
                "<Style id=\"{0}\"><LineStyle><color>{1}</color><width>2</width></LineStyle>"
                + "<PolyStyle><color>{2}</color></PolyStyle></Style>",
                id, line, fill);
        }

        private static string AttrsAsHtml(IReadOnlyDictionary<string, object> attrs)
        {
            if (attrs == null || attrs.Count == 0) return "";
            var sb = new StringBuilder(256);
            sb.Append("<table border='1' cellpadding='2' style='font-family:sans-serif;font-size:11px'>");
            foreach (var kv in attrs)
            {
                sb.Append("<tr><td><b>").Append(XmlEscape(kv.Key)).Append("</b></td><td>")
                  .Append(XmlEscape(Convert.ToString(kv.Value, CultureInfo.InvariantCulture) ?? ""))
                  .Append("</td></tr>");
            }
            sb.Append("</table>");
            return sb.ToString();
        }

        private static string XmlEscape(string s)
        {
            if (string.IsNullOrEmpty(s)) return "";
            return s.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;");
        }

        private static Coordinate[] ToClosedCoords(ShapeLatLon[] ring)
        {
            int n = ring.Length;
            bool closed = (ring[0].Lat == ring[n - 1].Lat && ring[0].Lon == ring[n - 1].Lon);
            int len = closed ? n : n + 1;
            var cs = new Coordinate[len];
            for (int i = 0; i < n; i++)
                cs[i] = new Coordinate(ring[i].Lon, ring[i].Lat);
            if (!closed) cs[n] = cs[0];
            return cs;
        }

        private static object CoerceForDbf(object v)
        {
            if (v == null) return "";
            if (v is bool b) return b ? "T" : "F";
            if (v is DateTime dt) return dt;
            if (v is double || v is float || v is int || v is long
                || v is short || v is decimal || v is string) return v;
            return Convert.ToString(v, CultureInfo.InvariantCulture) ?? "";
        }

        private static void WriteWgs84Prj(string prjPath)
        {
            const string wkt =
                "GEOGCS[\"GCS_WGS_1984\",DATUM[\"D_WGS_1984\","
                + "SPHEROID[\"WGS_1984\",6378137.0,298.257223563]],"
                + "PRIMEM[\"Greenwich\",0.0],UNIT[\"Degree\",0.0174532925199433]]";
            try { File.WriteAllText(prjPath, wkt, Encoding.ASCII); }
            catch { /* no critico */ }
        }
    }

    // Interfaz que implementa ShapefileLayer para exportar sin exponer
    // internals. ExportShapefile/ExportKml dependen solo de esto.
    public interface IShapefileExportSource
    {
        int PolygonCount { get; }
        int LineCount { get; }
        int PointCount { get; }

        IReadOnlyList<ShapeLatLon[]> GetPolygonRingsWgs84(int polygonIndex);
        ShapeLatLon[] GetLinePointsWgs84(int lineIndex);
        ShapeLatLon GetPointWgs84(int pointIndex);
        IReadOnlyDictionary<string, object> GetPolygonAttributes(int polygonIndex);
        Color? GetPolygonFillColor(int polygonIndex);
    }
}

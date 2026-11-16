using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>
    /// Writes fields as one GeoJSON FeatureCollection (WGS84): per field a Polygon feature with its
    /// boundary (or a feature without geometry when it has none), and per A-B line a LineString
    /// feature. Properties link lines to their field, so <see cref="FieldImport"/> restores
    /// everything; GIS programs simply show the shapes.
    /// </summary>
    public static class FieldExport
    {
        public static string ToGeoJson(IEnumerable<Field> fields, Func<string, IEnumerable<AbLine>> linesOf)
        {
            using (var ms = new MemoryStream())
            {
                using (var w = new Utf8JsonWriter(ms, new JsonWriterOptions { Indented = true }))
                {
                    w.WriteStartObject();
                    w.WriteString("type", "FeatureCollection");
                    w.WriteString("generator", "AgOpenGPS Android");
                    w.WriteStartArray("features");

                    foreach (Field f in fields)
                    {
                        WriteField(w, f);
                        IEnumerable<AbLine> lines = linesOf?.Invoke(f.Id);
                        if (lines != null) foreach (AbLine l in lines) WriteLine(w, l, f);
                    }

                    w.WriteEndArray();
                    w.WriteEndObject();
                }
                return Encoding.UTF8.GetString(ms.ToArray());
            }
        }

        private static void WriteField(Utf8JsonWriter w, Field f)
        {
            w.WriteStartObject();
            w.WriteString("type", "Feature");
            w.WriteStartObject("properties");
            w.WriteString("kind", "field");
            w.WriteString("name", f.Name);
            w.WriteString("fieldKey", f.Id);
            if (f.HasBoundary) w.WriteNumber("area_ha", Math.Round(f.AreaHectares, 4));
            w.WriteString("created", f.CreatedUtc.ToString("o", CultureInfo.InvariantCulture));
            w.WriteEndObject();

            if (!f.HasBoundary)
            {
                w.WriteNull("geometry");
            }
            else
            {
                w.WriteStartObject("geometry");
                w.WriteString("type", "Polygon");
                w.WriteStartArray("coordinates");
                w.WriteStartArray();
                foreach (TrackPoint p in f.Boundary) Position(w, p.Latitude, p.Longitude);
                Position(w, f.Boundary[0].Latitude, f.Boundary[0].Longitude); // GeoJSON rings are closed
                w.WriteEndArray();
                w.WriteEndArray();
                w.WriteEndObject();
            }
            w.WriteEndObject();
        }

        private static void WriteLine(Utf8JsonWriter w, AbLine l, Field f)
        {
            w.WriteStartObject();
            w.WriteString("type", "Feature");
            w.WriteStartObject("properties");
            w.WriteString("kind", "abline");
            w.WriteString("name", l.Name);
            w.WriteString("lineKind", l.Kind == LineKind.Curve ? "curve" : "straight");
            w.WriteString("field", f.Name);
            w.WriteString("fieldKey", f.Id);
            w.WriteNumber("nudge_m", Math.Round(l.NudgeMeters, 4));
            w.WriteNumber("heading_deg", Math.Round(l.HeadingDegrees, 2));
            w.WriteEndObject();

            w.WriteStartObject("geometry");
            w.WriteString("type", "LineString");
            w.WriteStartArray("coordinates");
            if (l.Kind == LineKind.Curve && l.Points != null && l.Points.Count >= 2)
            {
                foreach (TrackPoint p in l.Points) Position(w, p.Latitude, p.Longitude);
            }
            else
            {
                Position(w, l.LatitudeA, l.LongitudeA);
                Position(w, l.LatitudeB, l.LongitudeB);
            }
            w.WriteEndArray();
            w.WriteEndObject();
            w.WriteEndObject();
        }

        // [longitude, latitude], 8 decimals = about 1 mm.
        private static void Position(Utf8JsonWriter w, double lat, double lon)
        {
            w.WriteStartArray();
            w.WriteNumberValue(Math.Round(lon, 8));
            w.WriteNumberValue(Math.Round(lat, 8));
            w.WriteEndArray();
        }

        /// <summary>A file name from a field name: letters, digits, spaces and - _ . kept.</summary>
        public static string FileName(string name, string extension = ".geojson")
        {
            var sb = new StringBuilder();
            foreach (char c in name ?? "")
                sb.Append(char.IsLetterOrDigit(c) || c == ' ' || c == '-' || c == '_' || c == '.' ? c : '_');
            string n = sb.ToString().Trim();
            return (n.Length == 0 ? "fields" : n) + extension;
        }
    }
}

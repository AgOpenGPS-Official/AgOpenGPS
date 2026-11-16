using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>A field outline read from a file.</summary>
    public sealed class ImportedField
    {
        public string Name;
        public List<TrackPoint> Boundary = new List<TrackPoint>();
    }

    /// <summary>
    /// Reads field outlines from GeoJSON (Polygon / MultiPolygon, WGS84 longitude-latitude, as
    /// geojson.io, QGIS and most GIS exports write) or KML (Google Earth, ARKOD/AGRONET exports).
    /// Only outer outlines are used; holes are ignored for now.
    /// </summary>
    public static class FieldImport
    {
        /// <summary>All polygons in the file. Throws FormatException with a readable reason.</summary>
        public static List<ImportedField> Parse(string text, string fallbackName)
        {
            if (string.IsNullOrWhiteSpace(text)) throw new FormatException("The file is empty");
            string t = text.TrimStart('﻿', ' ', '\t', '\r', '\n');
            List<ImportedField> result = t.StartsWith("{", StringComparison.Ordinal) ? ParseGeoJson(t, fallbackName)
                : t.StartsWith("<", StringComparison.Ordinal) ? ParseKml(t, fallbackName)
                : throw new FormatException("Not a GeoJSON or KML file");

            if (result.Count == 0) throw new FormatException("No field outline (polygon) found in the file");
            for (int i = 0; i < result.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(result[i].Name))
                    result[i].Name = result.Count == 1 ? fallbackName : fallbackName + " " + (i + 1);
            }
            return result;
        }

        // ---------------------------------------------------------------- GeoJSON

        private static List<ImportedField> ParseGeoJson(string text, string fallbackName)
        {
            var list = new List<ImportedField>();
            JsonDocument doc;
            try { doc = JsonDocument.Parse(text); }
            catch (JsonException ex) { throw new FormatException("Damaged GeoJSON: " + ex.Message); }

            using (doc)
            {
                ReadGeoJsonObject(doc.RootElement, null, list);
            }
            return list;
        }

        private static void ReadGeoJsonObject(JsonElement e, string name, List<ImportedField> list)
        {
            if (e.ValueKind != JsonValueKind.Object || !e.TryGetProperty("type", out JsonElement typeEl)) return;
            string type = typeEl.GetString();
            switch (type)
            {
                case "FeatureCollection":
                    if (e.TryGetProperty("features", out JsonElement features) && features.ValueKind == JsonValueKind.Array)
                        foreach (JsonElement f in features.EnumerateArray()) ReadGeoJsonObject(f, null, list);
                    break;
                case "Feature":
                    string featureName = name;
                    if (e.TryGetProperty("properties", out JsonElement props) && props.ValueKind == JsonValueKind.Object)
                    {
                        foreach (string key in new[] { "name", "Name", "NAME", "naziv", "label", "title" })
                        {
                            if (props.TryGetProperty(key, out JsonElement n) && n.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(n.GetString()))
                            {
                                featureName = n.GetString().Trim();
                                break;
                            }
                        }
                    }
                    if (e.TryGetProperty("geometry", out JsonElement geom)) ReadGeoJsonObject(geom, featureName, list);
                    break;
                case "GeometryCollection":
                    if (e.TryGetProperty("geometries", out JsonElement geoms) && geoms.ValueKind == JsonValueKind.Array)
                        foreach (JsonElement g in geoms.EnumerateArray()) ReadGeoJsonObject(g, name, list);
                    break;
                case "Polygon":
                    if (e.TryGetProperty("coordinates", out JsonElement rings) && rings.GetArrayLength() > 0)
                        Add(list, name, ReadRing(rings[0]));
                    break;
                case "MultiPolygon":
                    if (e.TryGetProperty("coordinates", out JsonElement polys))
                        foreach (JsonElement poly in polys.EnumerateArray())
                            if (poly.GetArrayLength() > 0) Add(list, name, ReadRing(poly[0]));
                    break;
            }
        }

        private static List<TrackPoint> ReadRing(JsonElement ring)
        {
            var pts = new List<TrackPoint>();
            foreach (JsonElement c in ring.EnumerateArray())
            {
                if (c.GetArrayLength() < 2) continue;
                pts.Add(Point(c[1].GetDouble(), c[0].GetDouble())); // GeoJSON is [longitude, latitude]
            }
            return pts;
        }

        // ---------------------------------------------------------------- KML

        private static readonly Regex Placemark = new Regex(@"<(?:\w+:)?Placemark\b.*?</(?:\w+:)?Placemark>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static readonly Regex KmlName = new Regex(@"<(?:\w+:)?name>\s*(?:<!\[CDATA\[)?(.*?)(?:\]\]>)?\s*</(?:\w+:)?name>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static readonly Regex OuterCoords = new Regex(
            @"<(?:\w+:)?outerBoundaryIs>.*?<(?:\w+:)?coordinates>(.*?)</(?:\w+:)?coordinates>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        private static List<ImportedField> ParseKml(string text, string fallbackName)
        {
            var list = new List<ImportedField>();
            MatchCollection placemarks = Placemark.Matches(text);
            IEnumerable<string> blocks = placemarks.Count > 0 ? ToStrings(placemarks) : new[] { text };
            foreach (string block in blocks)
            {
                Match nm = KmlName.Match(block);
                string name = nm.Success ? System.Net.WebUtility.HtmlDecode(nm.Groups[1].Value.Trim()) : null;
                foreach (Match m in OuterCoords.Matches(block))
                {
                    var pts = new List<TrackPoint>();
                    foreach (string tuple in m.Groups[1].Value.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        string[] parts = tuple.Split(',');
                        if (parts.Length < 2) continue;
                        pts.Add(Point(D(parts[1]), D(parts[0]))); // KML is lon,lat[,alt]
                    }
                    Add(list, name, pts);
                }
            }
            return list;
        }

        private static IEnumerable<string> ToStrings(MatchCollection mc)
        {
            foreach (Match m in mc) yield return m.Value;
        }

        // ---------------------------------------------------------------- shared

        private static double D(string s)
        {
            if (!double.TryParse(s.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out double v))
                throw new FormatException("Bad coordinate '" + s + "'");
            return v;
        }

        private static TrackPoint Point(double lat, double lon)
        {
            if (Math.Abs(lat) > 90 || Math.Abs(lon) > 180)
                throw new FormatException("The coordinates are not latitude/longitude (WGS84). Export with EPSG:4326, not a national grid such as HTRS96/TM.");
            return new TrackPoint(lat, lon);
        }

        private static void Add(List<ImportedField> list, string name, List<TrackPoint> pts)
        {
            // Drop the repeated closing point: boundaries here are closed implicitly.
            if (pts.Count > 1 && Math.Abs(pts[0].Latitude - pts[pts.Count - 1].Latitude) < 1e-10
                && Math.Abs(pts[0].Longitude - pts[pts.Count - 1].Longitude) < 1e-10)
                pts.RemoveAt(pts.Count - 1);
            if (pts.Count >= 3) list.Add(new ImportedField { Name = name, Boundary = pts });
        }
    }
}

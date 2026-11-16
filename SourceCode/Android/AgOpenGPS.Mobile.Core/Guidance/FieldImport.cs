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
        /// <summary>Empty for a field without a recorded boundary (from an AgOpenGPS export).</summary>
        public List<TrackPoint> Boundary = new List<TrackPoint>();
        /// <summary>A-B lines of the field (AgOpenGPS exports only); ids are new, FieldId is not set yet.</summary>
        public List<AbLine> Lines = new List<AbLine>();
        /// <summary>Links lines to their field inside one file.</summary>
        internal string Key;
    }

    /// <summary>What a GeoJSON feature's properties say (AgOpenGPS exports add kind, fieldKey, ...).</summary>
    internal sealed class FeatureInfo
    {
        public string Name, Kind, Key, FieldName, LineKind;
        public double Nudge;
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
                var lines = new List<(FeatureInfo Info, List<TrackPoint> Points)>();
                ReadGeoJsonObject(doc.RootElement, new FeatureInfo(), list, lines);
                AttachLines(list, lines);
            }
            return list;
        }

        /// <summary>Gives each exported line to its field: by key, else by field name, else to the only field.</summary>
        private static void AttachLines(List<ImportedField> fields, List<(FeatureInfo Info, List<TrackPoint> Points)> lines)
        {
            foreach (var (info, pts) in lines)
            {
                if (pts.Count < 2) continue;
                ImportedField owner = null;
                foreach (ImportedField f in fields)
                    if (info.Key != null && f.Key == info.Key) { owner = f; break; }
                if (owner == null)
                    foreach (ImportedField f in fields)
                        if (info.FieldName != null && f.Name == info.FieldName) { owner = f; break; }
                if (owner == null && fields.Count == 1) owner = fields[0];
                if (owner == null) continue;

                bool curve = info.LineKind == "curve" && pts.Count > 2;
                owner.Lines.Add(new AbLine
                {
                    Name = string.IsNullOrWhiteSpace(info.Name) ? "AB " + (owner.Lines.Count + 1) : info.Name,
                    Kind = curve ? LineKind.Curve : LineKind.Straight,
                    LatitudeA = pts[0].Latitude,
                    LongitudeA = pts[0].Longitude,
                    LatitudeB = pts[pts.Count - 1].Latitude,
                    LongitudeB = pts[pts.Count - 1].Longitude,
                    Points = curve ? pts : new List<TrackPoint>(),
                    NudgeMeters = info.Nudge,
                });
            }
        }

        private static string Str(JsonElement props, params string[] keys)
        {
            foreach (string key in keys)
                if (props.TryGetProperty(key, out JsonElement v) && v.ValueKind == JsonValueKind.String && !string.IsNullOrWhiteSpace(v.GetString()))
                    return v.GetString().Trim();
            return null;
        }

        private static void ReadGeoJsonObject(JsonElement e, FeatureInfo info, List<ImportedField> list,
            List<(FeatureInfo, List<TrackPoint>)> lines)
        {
            if (e.ValueKind != JsonValueKind.Object || !e.TryGetProperty("type", out JsonElement typeEl)) return;
            switch (typeEl.GetString())
            {
                case "FeatureCollection":
                    if (e.TryGetProperty("features", out JsonElement features) && features.ValueKind == JsonValueKind.Array)
                        foreach (JsonElement f in features.EnumerateArray()) ReadGeoJsonObject(f, new FeatureInfo(), list, lines);
                    break;
                case "Feature":
                    var fi = new FeatureInfo();
                    if (e.TryGetProperty("properties", out JsonElement props) && props.ValueKind == JsonValueKind.Object)
                    {
                        fi.Name = Str(props, "name", "Name", "NAME", "naziv", "label", "title");
                        fi.Kind = Str(props, "kind");
                        fi.Key = Str(props, "fieldKey");
                        fi.FieldName = Str(props, "field");
                        fi.LineKind = Str(props, "lineKind");
                        if (props.TryGetProperty("nudge_m", out JsonElement nudge) && nudge.ValueKind == JsonValueKind.Number) fi.Nudge = nudge.GetDouble();
                    }
                    bool hasGeometry = e.TryGetProperty("geometry", out JsonElement geom) && geom.ValueKind == JsonValueKind.Object;
                    if (hasGeometry) ReadGeoJsonObject(geom, fi, list, lines);
                    else if (fi.Kind == "field") list.Add(new ImportedField { Name = fi.Name, Key = fi.Key }); // field without boundary
                    break;
                case "GeometryCollection":
                    if (e.TryGetProperty("geometries", out JsonElement geoms) && geoms.ValueKind == JsonValueKind.Array)
                        foreach (JsonElement g in geoms.EnumerateArray()) ReadGeoJsonObject(g, info, list, lines);
                    break;
                case "Polygon":
                    if (e.TryGetProperty("coordinates", out JsonElement rings) && rings.GetArrayLength() > 0)
                        Add(list, info, ReadRing(rings[0]));
                    break;
                case "MultiPolygon":
                    if (e.TryGetProperty("coordinates", out JsonElement polys))
                        foreach (JsonElement poly in polys.EnumerateArray())
                            if (poly.GetArrayLength() > 0) Add(list, info, ReadRing(poly[0]));
                    break;
                case "LineString":
                    // Only lines marked as A-B lines by an AgOpenGPS export; other lines (roads, ditches) are skipped.
                    if (info.Kind == "abline" && e.TryGetProperty("coordinates", out JsonElement coords))
                        lines.Add((info, ReadRing(coords)));
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
                    Add(list, new FeatureInfo { Name = name }, pts);
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

        private static void Add(List<ImportedField> list, FeatureInfo info, List<TrackPoint> pts)
        {
            // Drop the repeated closing point: boundaries here are closed implicitly.
            if (pts.Count > 1 && Math.Abs(pts[0].Latitude - pts[pts.Count - 1].Latitude) < 1e-10
                && Math.Abs(pts[0].Longitude - pts[pts.Count - 1].Longitude) < 1e-10)
                pts.RemoveAt(pts.Count - 1);
            if (pts.Count >= 3) list.Add(new ImportedField { Name = info.Name, Key = info.Key, Boundary = pts });
        }
    }
}

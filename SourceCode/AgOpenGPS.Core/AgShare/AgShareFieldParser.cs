using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;
using AgLibrary.Logging;

namespace AgOpenGPS.Core.AgShare
{
    public static class AgShareFieldParser
    {
        public static List<ParsedLatLon> ParseBoundary(object boundaryJson)
        {
            var result = new List<ParsedLatLon>();

            if (boundaryJson == null)
                return result;

            try
            {
                var jsonString = boundaryJson.ToString();

                using (var doc = JsonDocument.Parse(jsonString))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("coordinates", out JsonElement coordsElement) &&
                        coordsElement.ValueKind == JsonValueKind.Array)
                    {
                        // GeoJSON Polygon → coordinates[0] = outer ring
                        var outerRing = coordsElement[0];
                        foreach (var point in outerRing.EnumerateArray())
                        {
                            if (point.ValueKind == JsonValueKind.Array && point.GetArrayLength() >= 2)
                            {
                                var lon = point[0].GetDouble();
                                var lat = point[1].GetDouble();

                                result.Add(new ParsedLatLon
                                {
                                    Longitude = lon,
                                    Latitude = lat
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
               Log.EventWriter($"[ERROR] Failed to parse boundary: {ex.Message}");
            }

            return result;
        }

        public static List<ParsedTrackLatLon> ParseAbLines(object abLinesJson)
        {
            var tracks = new List<ParsedTrackLatLon>();
            Debug.WriteLine("AbLinesParserActivated");

            if (abLinesJson == null)
                return tracks;

            try
            {
                var jsonString = JsonSerializer.Serialize(abLinesJson);
                var doc = JsonDocument.Parse(jsonString);

                if (doc.RootElement.TryGetProperty("features", out var features))
                {
                    Debug.WriteLine($"[DEBUG] features count: {features.GetArrayLength()}");

                    foreach (var feature in features.EnumerateArray())
                    {
                        if (!feature.TryGetProperty("geometry", out var geometry)) continue;
                        if (!feature.TryGetProperty("properties", out var properties)) continue;

                        string type = properties.TryGetProperty("type", out var typeEl) ? typeEl.GetString() : "Unknown";
                        string name = properties.TryGetProperty("name", out var nameEl) ? nameEl.GetString() : "Unnamed";

                        if (!geometry.TryGetProperty("coordinates", out var coordsElement)) continue;
                        if (geometry.TryGetProperty("type", out var geomType) && geomType.GetString() != "LineString") continue;

                        var coords = coordsElement.EnumerateArray()
                            .Select(coord => new ParsedLatLon
                            {
                                Longitude = coord[0].GetDouble(),
                                Latitude = coord[1].GetDouble()
                            }).ToList();

                        if (type == "AB" && coords.Count >= 2)
                        {
                            tracks.Add(new ParsedTrackLatLon
                            {
                                Name = name,
                                Type = "AB",
                                Coords = new List<ParsedLatLon> { coords[0], coords[1] }
                            });
                        }
                        else if (type == "Curve" && coords.Count > 1)
                        {
                            tracks.Add(new ParsedTrackLatLon
                            {
                                Name = name,
                                Type = "Curve",
                                Coords = coords
                            });
                        }
                    }
                }
                else
                {
                    Debug.WriteLine("[DEBUG] 'features' property not found in GeoJSON.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ERROR] Failed to parse AB Lines: " + ex.Message);
            }

            return tracks;
        }
    }

    public class GeoJsonFeatureCollection
    {
        public string Type { get; set; }
        public List<GeoJsonFeature> Features { get; set; }
    }

    public class GeoJsonFeature
    {
        public GeoJsonGeometry Geometry { get; set; }
        public Dictionary<string, object> Properties { get; set; }
    }

    public class GeoJsonGeometry
    {
        public string Type { get; set; }
        public JsonElement Coordinates { get; set; }
    }


}

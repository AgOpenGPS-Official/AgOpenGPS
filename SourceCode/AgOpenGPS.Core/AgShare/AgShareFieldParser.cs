using System;
using System.Collections.Generic;
using System.Text.Json;

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
                Console.WriteLine("Error parsing boundary: " + ex.Message);
            }

            return result;
        }

        public static List<ParsedTrackLatLon> ParseAbLines(object abLinesJson)
        {
            var tracks = new List<ParsedTrackLatLon>();

            if (abLinesJson == null)
                return tracks;

            try
            {
                var jsonString = JsonSerializer.Serialize(abLinesJson);
                var featureCollection = JsonSerializer.Deserialize<GeoJsonFeatureCollection>(jsonString);

                if (featureCollection?.Features == null)
                    return tracks;

                foreach (var feature in featureCollection.Features)
                {
                    if (feature.Geometry == null || feature.Properties == null)
                        continue;

                    string type = feature.Properties.ContainsKey("type") ? feature.Properties["type"].ToString() : "";
                    string name = feature.Properties.ContainsKey("name") ? feature.Properties["name"].ToString() : "Unnamed";

                    var coords = feature.Geometry.Coordinates;

                    if (type == "AB" && coords != null && coords.Count >= 2)
                    {
                        var pointA = coords[0];
                        var pointB = coords[1];

                        var parsedTrack = new ParsedTrackLatLon
                        {
                            Name = name,
                            Type = "AB",
                            Coords = new List<ParsedLatLon>
                            {
                                new ParsedLatLon { Longitude = pointA[0], Latitude = pointA[1] },
                                new ParsedLatLon { Longitude = pointB[0], Latitude = pointB[1] }
                            }
                        };

                        tracks.Add(parsedTrack);
                    }
                    else if (type == "Curve" && coords != null && coords.Count > 1)
                    {
                        var curvePoints = new List<ParsedLatLon>();

                        foreach (var coordPair in coords)
                        {
                            if (coordPair.Count < 2) continue;

                            curvePoints.Add(new ParsedLatLon
                            {
                                Longitude = coordPair[0],
                                Latitude = coordPair[1]
                            });
                        }

                        var parsedTrack = new ParsedTrackLatLon
                        {
                            Name = name.StartsWith("Cu") ? name : "Cu " + name,
                            Type = "Curve",
                            Coords = curvePoints
                        };

                        tracks.Add(parsedTrack);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error parsing AB Lines: " + ex.Message);
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
        public List<List<double>> Coordinates { get; set; }
    }
}

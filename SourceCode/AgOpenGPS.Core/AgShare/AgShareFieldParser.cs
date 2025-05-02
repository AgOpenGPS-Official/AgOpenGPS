using System;
using System.Collections.Generic;
using System.Text.Json;

namespace AgOpenGPS.Core.AgShare
{
    public static class AgShareFieldParser
    {
        public static List<ParsedLatLon> ParseBoundary(Geometry geometry)
        {
            var boundaryPoints = new List<ParsedLatLon>();

            if (geometry == null || geometry.Coordinates == null)
                return boundaryPoints;

            if (geometry.Type == "Polygon")
            {
                // Deserialize to List<List<List<double>>>
                var polygon = JsonSerializer.Deserialize<List<List<List<double>>>>(JsonSerializer.Serialize(geometry.Coordinates));
                if (polygon == null || polygon.Count == 0)
                    return boundaryPoints;

                var ring = polygon[0]; // First ring

                foreach (var coordPair in ring)
                {
                    if (coordPair.Count < 2) continue;

                    boundaryPoints.Add(new ParsedLatLon
                    {
                        Longitude = coordPair[0],
                        Latitude = coordPair[1]
                    });
                }
            }
            else if (geometry.Type == "MultiPolygon")
            {
                // Deserialize to List<List<List<List<double>>>>
                var multiPolygon = JsonSerializer.Deserialize<List<List<List<List<double>>>>>(JsonSerializer.Serialize(geometry.Coordinates));
                if (multiPolygon == null || multiPolygon.Count == 0)
                    return boundaryPoints;

                var polygon = multiPolygon[0]; // First polygon
                var ring = polygon[0]; // First ring

                foreach (var coordPair in ring)
                {
                    if (coordPair.Count < 2) continue;

                    boundaryPoints.Add(new ParsedLatLon
                    {
                        Longitude = coordPair[0],
                        Latitude = coordPair[1]
                    });
                }
            }

            return boundaryPoints;
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
                            PtA = new ParsedLatLon
                            {
                                Longitude = pointA[0],
                                Latitude = pointA[1]
                            },
                            PtB = new ParsedLatLon
                            {
                                Longitude = pointB[0],
                                Latitude = pointB[1]
                            },
                            CurvePoints = null
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
                            PtA = null,
                            PtB = null,
                            CurvePoints = curvePoints
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

    public class ParsedLatLon
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class ParsedTrackLatLon
    {
        public string Name { get; set; }
        public string Type { get; set; } // "AB" or "Curve"
        public ParsedLatLon PtA { get; set; }
        public ParsedLatLon PtB { get; set; }
        public List<ParsedLatLon> CurvePoints { get; set; }
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

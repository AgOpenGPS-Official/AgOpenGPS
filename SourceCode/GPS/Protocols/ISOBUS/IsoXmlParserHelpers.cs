using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance; // BIG BANG Step 2: For Track

namespace AgOpenGPS.Protocols.ISOBUS
{
    /// <summary>
    /// This class provides helper methods for parsing ISO XML data related to agricultural fields.
    /// It does all the conversion and extraction of coordinates, boundaries, headlands, and guidance lines.
    /// </summary>
    public static class IsoXmlParserHelpers
    {
        // Extract WGS84 origin from PLN or GGP lines
        public static bool TryExtractOrigin(XmlNodeList fieldParts, out Wgs84 origin)
        {
            double latSum = 0, lonSum = 0;
            int count = 0;

            foreach (XmlNode node in fieldParts)
            {
                if (node.Name == "PLN" && node.Attributes["A"]?.Value == "1")
                {
                    AccumulateCoordinates(node, ref latSum, ref lonSum, ref count);
                }
            }

            if (count == 0)
            {
                foreach (XmlNode node in fieldParts)
                {
                    if (node.Name == "GGP")
                    {
                        var lsg = node.SelectSingleNode("GPN/LSG");
                        if (lsg != null) AccumulateCoordinates(lsg.ParentNode, ref latSum, ref lonSum, ref count);
                    }
                }
            }

            if (count == 0)
            {
                origin = new Wgs84();
                return false;
            }

            origin = new Wgs84(latSum / count, lonSum / count);
            return true;
        }

        // Parse PLN boundaries into CBoundaryList objects
        public static List<CBoundaryList> ParseBoundaries(XmlNodeList fieldParts, ApplicationModel appModel)
        {
            List<CBoundaryList> boundaries = new List<CBoundaryList>();
            bool outerBuilt = false;

            foreach (XmlNode node in fieldParts)
            {
                if (node.Name != "PLN") continue;
                string type = node.Attributes["A"]?.Value;

                if ((type == "1" || type == "9") && !outerBuilt)
                {
                    if (node.SelectSingleNode("LSG[@A='1']") is XmlNode lsg)
                    {
                        boundaries.Add(ParseBoundaryFromLSG(lsg, appModel));
                        outerBuilt = true;
                    }
                }
                else if (type == "3" || type == "4" || type == "6")
                {
                    if (node.SelectSingleNode("LSG[@A='1']") is XmlNode lsg)
                    {
                        boundaries.Add(ParseBoundaryFromLSG(lsg, appModel));
                    }
                }
            }

            return boundaries;
        }
        // Parse Headland if available
        public static List<vec3> ParseHeadland(XmlNodeList fieldParts, ApplicationModel appModel)
        {
            foreach (XmlNode node in fieldParts)
            {
                if (node.Name == "PLN" && node.Attributes["A"]?.Value == "10")
                {
                    if (node.SelectSingleNode("LSG[@A='1']") is XmlNode lsg)
                    {
                        var list = new List<vec3>();
                        foreach (XmlNode pnt in lsg.SelectNodes("PNT"))
                        {
                            if (double.TryParse(pnt.Attributes["C"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) &&
                                double.TryParse(pnt.Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon))
                            {
                                GeoCoord geo = appModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(lat, lon));
                                list.Add(new vec3(geo));
                            }
                        }

                        CurveCABTools.CalculateHeadings(list);
                        return list;
                    }
                }
            }

            return new List<vec3>();
        }


        // Parse all valid guidance lines
        public static List<Track> ParseAllGuidanceLines(XmlNodeList fieldParts, ApplicationModel appModel) // BIG BANG Step 2: CTrk → Track
        {
            List<Track> tracks = new List<Track>(); // BIG BANG Step 2: CTrk → Track

            foreach (XmlNode node in fieldParts)
            {
                if (node.Name == "GGP")
                {
                    var trk = ParseGGPNode(node, appModel);
                    if (trk != null) tracks.Add(trk);
                }
                else if (node.Name == "LSG" && node.Attributes["A"]?.Value == "5")
                {
                    var trk = ParseLSGNode(node, appModel);
                    if (trk != null) tracks.Add(trk);
                }
            }

            return tracks;
        }

        public static CBoundaryList ParseBoundaryFromLSG(XmlNode lsg, ApplicationModel appModel)
        {
            var list = new CBoundaryList();

            foreach (XmlNode pnt in lsg.SelectNodes("PNT"))
            {
                if (double.TryParse(pnt.Attributes["C"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) &&
                    double.TryParse(pnt.Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon))
                {
                    GeoCoord geo = appModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(lat, lon));
                    list.fenceLine.Add(new vec3(geo));
                }
            }

            return list;
        }

        // Parse GGP → GPN → LSG line
        private static Track ParseGGPNode(XmlNode node, ApplicationModel appModel) // BIG BANG Step 2: CTrk → Track
        {
            var gpn = node.SelectSingleNode("GPN");
            var lsg = gpn?.SelectSingleNode("LSG[@A='5']");
            if (gpn == null || lsg == null) return null;

            string lineType = gpn.Attributes["C"]?.Value;
            string name = gpn.Attributes["B"]?.Value ?? node.Attributes["B"]?.Value ?? "Unnamed";

            if (lineType == "1") return ParseABLine(lsg, name, appModel);
            else if (lineType == "3") return ParseCurveLine(lsg, name, appModel);

            return null;
        }

        // Parse LSG line directly
        private static Track ParseLSGNode(XmlNode lsg, ApplicationModel appModel) // BIG BANG Step 2: CTrk → Track
        {
            string name = lsg.Attributes["B"]?.Value ?? "Unnamed";
            int count = lsg.SelectNodes("PNT").Count;

            if (count == 2) return ParseABLine(lsg, name, appModel);
            else if (count > 2) return ParseCurveLine(lsg, name, appModel);

            return null;
        }

        // Parse AB line into Track
        private static Track ParseABLine(XmlNode lsg, string name, ApplicationModel appModel) // BIG BANG Step 2: CTrk → Track
        {
            var points = lsg.SelectNodes("PNT");
            if (points.Count < 2) return null;

            var ptA = ParseVec2(points[0], appModel);
            var ptB = ParseVec2(points[1], appModel);

            double heading = Math.Atan2(ptB.easting - ptA.easting, ptB.northing - ptA.northing);
            if (heading < 0) heading += glm.twoPI;

            // BIG BANG Step 2: CTrk → Track with PascalCase properties
            return new Track
            {
                Id = System.Guid.NewGuid(),
                Heading = heading,
                Mode = TrackMode.AB,
                PtA = ptA,
                PtB = ptB,
                Name = name.Trim(),
                IsVisible = true,
                NudgeDistance = 0,
                CurvePts = new List<vec3>(),
                WorkedTracks = new HashSet<int>()
            };
        }

        // Parse Curve line into Track
        private static Track ParseCurveLine(XmlNode lsg, string name, ApplicationModel appModel) // BIG BANG Step 2: CTrk → Track
        {
            var points = lsg.SelectNodes("PNT");
            if (points == null || points.Count <= 2) return null;

            // Build raw list from ISOXML
            var desList = new List<vec3>();
            foreach (XmlNode pnt in points)
            {
                var geo = ParseGeoCoord(pnt, appModel);
                desList.Add(new vec3(geo));
            }

            // Keep originals if you want ptA/ptB to reflect the original span
            var originalFirst = desList[0];
            var originalLast = desList[desList.Count - 1];

            // Extend ends before preprocessing (pure, no ref)
            double extendMeters = 100.0;     // tweak as needed
            bool keepOriginalAB = false;   // true => ptA/ptB = original endpoints
            desList = ExtendEnds(desList, extendMeters);

            // CABCurve-like pipeline (no ref, returns new lists)
            desList = CurveCABTools.Preprocess(desList, 1.6, 0.5);
            if (desList == null || desList.Count < 2) return null;

            double avgHeading = CurveCABTools.ComputeAverageHeading(desList);

            // Decide ptA/ptB (extended vs original)
            vec2 ptA, ptB;
            if (keepOriginalAB)
            {
                ptA = new vec2(originalFirst.easting, originalFirst.northing);
                ptB = new vec2(originalLast.easting, originalLast.northing);
            }
            else
            {
                ptA = new vec2(desList[0].easting, desList[0].northing);
                ptB = new vec2(desList[desList.Count - 1].easting, desList[desList.Count - 1].northing);
            }

            // Build track - BIG BANG Step 2: CTrk → Track with PascalCase properties
            var track = new Track
            {
                Id = System.Guid.NewGuid(),
                Heading = avgHeading,
                Mode = TrackMode.Curve,
                PtA = ptA,
                PtB = ptB,
                Name = string.IsNullOrWhiteSpace(name) ? "Curve_" + DateTime.Now.ToString("HHmmss") : name,
                IsVisible = true,
                NudgeDistance = 0,
                CurvePts = new List<vec3>(desList), // BIG BANG Step 2: Direct initialization instead of loop
                WorkedTracks = new HashSet<int>()
            };

            return track;
        }

        // Now we can use some helpers

        private static void AccumulateCoordinates(XmlNode parent, ref double latSum, ref double lonSum, ref int count)
        {
            foreach (XmlNode pnt in parent.SelectNodes(".//PNT"))
            {
                if (double.TryParse(pnt.Attributes["C"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat) &&
                    double.TryParse(pnt.Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon))
                {
                    latSum += lat;
                    lonSum += lon;
                    count++;
                }
            }
        }

        private static vec2 ParseVec2(XmlNode pnt, ApplicationModel appModel)
        {
            double.TryParse(pnt.Attributes["C"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat);
            double.TryParse(pnt.Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon);
            GeoCoord geo = appModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(lat, lon));
            return new vec2(geo);
        }

        private static GeoCoord ParseGeoCoord(XmlNode pnt, ApplicationModel appModel)
        {
            double.TryParse(pnt.Attributes["C"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lat);
            double.TryParse(pnt.Attributes["D"]?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out double lon);
            return appModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(lat, lon));
        }
        private static List<vec3> ExtendEnds(List<vec3> pts, double extendMeters)
        {
            if (pts == null || pts.Count < 2 || extendMeters <= 0) return pts;

            // Work on a copy to keep this pure
            var list = new List<vec3>(pts);

            // Extend before the first point (backwards along first segment)
            var first = list[0];
            var second = list[1];
            double dxF = first.easting - second.easting;
            double dyF = first.northing - second.northing;
            double lenF = Math.Sqrt(dxF * dxF + dyF * dyF);
            if (lenF > 1e-6)
            {
                dxF /= lenF; dyF /= lenF;
                list.Insert(0, new vec3(
                    first.easting + dxF * extendMeters,
                    first.northing + dyF * extendMeters,
                    0
                ));
            }

            // Extend after the last point (forwards along last segment)
            var last = list[list.Count - 1];
            var beforeLast = list[list.Count - 2];
            double dxL = last.easting - beforeLast.easting;
            double dyL = last.northing - beforeLast.northing;
            double lenL = Math.Sqrt(dxL * dxL + dyL * dyL);
            if (lenL > 1e-6)
            {
                dxL /= lenL; dyL /= lenL;
                list.Add(new vec3(
                    last.easting + dxL * extendMeters,
                    last.northing + dyL * extendMeters,
                    0
                ));
            }

            return list;
        }

    }
}

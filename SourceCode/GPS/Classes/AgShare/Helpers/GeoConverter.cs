using System;
using System.Collections.Generic;

namespace AgOpenGPS.Classes.AgShare.Helpers
{
    // Converts between WGS84 lat/lon and local NE coordinates using CNMEA projection
    public class GeoConverter
    {
        private readonly double lat0Rad;
        private readonly double lat0Deg;
        private readonly double lon0;
        private readonly double metersPerDegLat;
        private readonly double metersPerDegLon;

        public GeoConverter(double originLat, double originLon)
        {
            lat0Deg = originLat;
            lat0Rad = DegToRad(originLat);
            lon0 = originLon;

            // CNMEA meters-per-degree conversion formulas
            metersPerDegLat = 111132.92
                            - 559.82 * Math.Cos(2 * lat0Rad)
                            + 1.175 * Math.Cos(4 * lat0Rad)
                            - 0.0023 * Math.Cos(6 * lat0Rad);

            metersPerDegLon = 111412.84 * Math.Cos(lat0Rad)
                            - 93.5 * Math.Cos(3 * lat0Rad)
                            + 0.118 * Math.Cos(5 * lat0Rad);
        }

        // Convert WGS84 lat/lon to local easting/northing
        public Vec2 ToLocal(double lat, double lon)
        {
            double deltaLat = lat - lat0Deg;
            double deltaLon = lon - lon0;

            double northing = deltaLat * metersPerDegLat;
            double easting = deltaLon * metersPerDegLon;

            return new Vec2(easting, northing);
        }

        // Calculate heading from two local coordinates (in radians, 0-2π)
        public static double HeadingFromPoints(Vec2 a, Vec2 b)
        {
            double angle = Math.Atan2(b.Easting - a.Easting, b.Northing - a.Northing);
            return (angle + 2 * Math.PI) % (2 * Math.PI);
        }

        private static double DegToRad(double deg) => deg * Math.PI / 180.0;
    }

    // Simple struct to store local easting/northing
    public struct Vec2
    {
        public double Easting;
        public double Northing;

        public Vec2(double e, double n)
        {
            Easting = e;
            Northing = n;
        }
    }
    public static class BoundaryHelper
    {
        /// <summary>
        /// Calculate heading for each boundary point based on the direction to the next point (last → first is closed loop)
        /// </summary>
        public static List<LocalPoint> WithHeadings(List<LocalPoint> points)
        {
            var result = new List<LocalPoint>();
            if (points == null || points.Count < 2) return result;

            for (int i = 0; i < points.Count; i++)
            {
                var curr = points[i];
                var next = points[(i + 1) % points.Count]; // closed ring
                double dx = next.Easting - curr.Easting;
                double dy = next.Northing - curr.Northing;
                double heading = Math.Atan2(dx, dy);
                result.Add(new LocalPoint(curr.Easting, curr.Northing, heading));
            }

            return result;
        }
    }

    public static class CurveHelper
    {
        /// <summary>
        /// Calculate Heading for CurvePoints   
        /// </summary>
        public static List<vec3> CalculateHeadings(List<vec3> inputPoints)
        {
            var output = new List<vec3>();

            if (inputPoints == null || inputPoints.Count < 2)
                return output;

            for (int i = 0; i < inputPoints.Count - 1; i++)
            {
                var p1 = inputPoints[i];
                var p2 = inputPoints[i + 1];

                var dx = p2.easting - p1.easting;
                var dy = p2.northing - p1.northing;

                var heading = Math.Atan2(dx, dy);

                output.Add(new vec3(p1.easting, p1.northing, heading));
            }
            var last = inputPoints[inputPoints.Count - 1];
            var lastHeading = output[output.Count - 1].heading;
            output.Add(new vec3(last.easting, last.northing, lastHeading));

            return output;
        }
    }
    public static class GeoUtils
    {
        // Calculates approximate area of a lat/lon polygon in hectares
        public static double CalculateAreaInHa(List<CoordinateDto> coords)
        {
            if (coords == null || coords.Count < 3)
                return 0;

            double area = 0;
            for (int i = 0, j = coords.Count - 1; i < coords.Count; j = i++)
            {
                double xi = coords[i].Longitude;
                double yi = coords[i].Latitude;
                double xj = coords[j].Longitude;
                double yj = coords[j].Latitude;
                area += (xj + xi) * (yj - yi);
            }

            return Math.Abs(area * 111.32 * 111.32 / 2.0) / 10000.0;
        }
    }
}

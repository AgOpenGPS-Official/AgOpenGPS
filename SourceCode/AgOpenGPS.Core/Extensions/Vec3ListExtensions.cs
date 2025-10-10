using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    /// <summary>
    /// Extension methods for List<vec3> geometry operations
    /// Pure business logic - no UI dependencies
    /// Migrated from AOG_Dev for better testability
    /// </summary>
    public static class Vec3ListExtensions
    {
        /// <summary>
        /// Creates an offset line parallel to the input line
        /// Used for creating guidance tracks at a specific distance from reference
        /// </summary>
        /// <param name="points">Input line points with headings</param>
        /// <param name="distance">Offset distance (positive = right, negative = left)</param>
        /// <param name="minDist">Minimum distance between points in result</param>
        /// <param name="loop">True if path forms a closed loop</param>
        /// <returns>New offset line</returns>
        public static List<vec3> OffsetLine(this List<vec3> points, double distance, double minDist, bool loop)
        {
            // First calculate headings for the input points
            points.CalculateHeadings(loop);

            var result = new List<vec3>();
            int count = points.Count;

            double distSq = distance * distance - 0.0001;

            // Create offset points perpendicular to heading
            for (int i = 0; i < count; i++)
            {
                // Calculate the point offset perpendicular to the heading
                // Using heading + 90 degrees (PIBy2) for perpendicular offset
                var easting = points[i].easting + (Math.Cos(points[i].heading) * distance);
                var northing = points[i].northing - (Math.Sin(points[i].heading) * distance);

                bool Add = true;

                // Check if this point is too close to any original point
                for (int j = 0; j < count; j++)
                {
                    double check = GeoMath.DistanceSquared(northing, easting, points[j].northing, points[j].easting);
                    if (check < distSq)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add)
                {
                    if (result.Count > 0)
                    {
                        double dist = GeoMath.DistanceSquared(northing, easting, result[result.Count - 1].northing, result[result.Count - 1].easting);
                        if (dist > minDist)
                            result.Add(new vec3(easting, northing, 0));
                    }
                    else
                        result.Add(new vec3(easting, northing, 0));
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates headings for each point based on neighboring points
        /// Uses average of previous and next point for smooth heading transitions
        /// </summary>
        /// <param name="points">Points to calculate headings for (modified in place)</param>
        /// <param name="loop">True if path forms a closed loop</param>
        public static void CalculateHeadings(this List<vec3> points, bool loop)
        {
            int cnt = points.Count;

            if (cnt > 1)
            {
                vec3[] arr = new vec3[cnt];
                cnt--;
                points.CopyTo(arr);
                points.Clear();

                // First point needs last, first, second points
                vec3 pt3 = arr[0];
                if (loop)
                    pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
                else
                    pt3.heading = Math.Atan2(arr[1].easting - arr[0].easting, arr[1].northing - arr[0].northing);

                if (pt3.heading < 0) pt3.heading += GeoMath.twoPI;
                points.Add(pt3);

                // Middle points - average of previous and next
                for (int i = 1; i < cnt; i++)
                {
                    pt3 = arr[i];
                    pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                    if (pt3.heading < 0) pt3.heading += GeoMath.twoPI;
                    points.Add(pt3);
                }

                // Last point
                pt3 = arr[cnt];
                if (loop)
                    pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 1].easting, arr[0].northing - arr[cnt - 1].northing);
                else
                    pt3.heading = Math.Atan2(arr[cnt].easting - arr[cnt - 1].easting, arr[cnt].northing - arr[cnt - 1].northing);

                if (pt3.heading < 0) pt3.heading += GeoMath.twoPI;
                points.Add(pt3);
            }
        }
    }
}

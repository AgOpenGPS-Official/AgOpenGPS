using System;
using System.Runtime.CompilerServices;

namespace AgOpenGPS.Core.Models
{
    /// <summary>
    /// Pure geometry math helper functions and constants
    /// No UI dependencies - fully testable
    /// Performance-optimized with AggressiveInlining for hot path methods
    /// </summary>
    public static class GeoMath
    {
        public const double twoPI = 6.28318530717958647692;
        public const double PIBy2 = 1.57079632679489661923;

        /// <summary>
        /// Calculate Euclidean distance between two vec3 points
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(vec3 first, vec3 second)
        {
            double dx = first.easting - second.easting;
            double dy = first.northing - second.northing;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculate Euclidean distance between two vec2 points
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Distance(vec2 first, vec2 second)
        {
            double dx = first.easting - second.easting;
            double dy = first.northing - second.northing;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Calculate squared distance (faster, no sqrt - use for comparisons)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(double northing1, double easting1, double northing2, double easting2)
        {
            double dx = easting1 - easting2;
            double dy = northing1 - northing2;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Calculate squared distance between two vec3 points (faster, no sqrt - use for comparisons)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(vec3 first, vec3 second)
        {
            double dx = first.easting - second.easting;
            double dy = first.northing - second.northing;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Calculate squared distance between two vec2 points (faster, no sqrt - use for comparisons)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double DistanceSquared(vec2 first, vec2 second)
        {
            double dx = first.easting - second.easting;
            double dy = first.northing - second.northing;
            return dx * dx + dy * dy;
        }

        // Catmull Rom interpoint spline calculation
        public static vec3 Catmull(double t, vec3 p0, vec3 p1, vec3 p2, vec3 p3)
        {
            double tt = t * t;
            double ttt = tt * t;

            double q1 = -ttt + 2.0f * tt - t;
            double q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
            double q3 = -3.0f * ttt + 4.0f * tt + t;
            double q4 = ttt - tt;

            double tx = 0.5f * (p0.easting * q1 + p1.easting * q2 + p2.easting * q3 + p3.easting * q4);
            double ty = 0.5f * (p0.northing * q1 + p1.northing * q2 + p2.northing * q3 + p3.northing * q4);

            vec3 ret = new vec3(tx, ty, 0);
            return ret;
        }
    }
}

// AgOpenGPS.Core/Math/Distance2D.cs
// Purpose: Stateless 2D distance helpers for local-plane coordinates.
// Notes: Keep double,double variants; delegate GeoCoord variants to GeoCoord's own methods.
using System;
using AgOpenGPS.Core.Models; // GeoCoord

namespace AgOpenGPS.Core.Mathx
{
    /// <summary>
    /// Helpers to compute Euclidean distances in the local plane (meters).
    /// </summary>
    public static class Distance2D
    {
        /// <summary>
        /// Euclidean distance between two (easting, northing) points (meters).
        /// </summary>
        public static double Distance(double easting1, double northing1, double easting2, double northing2)
        {
            double dx = easting1 - easting2;
            double dy = northing1 - northing2;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        /// <summary>
        /// Squared Euclidean distance between two (easting, northing) points (meters^2).
        /// </summary>
        public static double DistanceSquared(double easting1, double northing1, double easting2, double northing2)
        {
            double dx = easting1 - easting2;
            double dy = northing1 - northing2;
            return dx * dx + dy * dy;
        }

        /// <summary>
        /// Euclidean distance between two GeoCoord points (meters).
        /// Delegates to GeoCoord.Distance to avoid duplicated logic.
        /// </summary>
        public static double Distance(GeoCoord a, GeoCoord b) => a.Distance(b);

        /// <summary>
        /// Squared Euclidean distance between two GeoCoord points (meters^2).
        /// Delegates to GeoCoord.DistanceSquared to avoid duplicated logic.
        /// </summary>
        public static double DistanceSquared(GeoCoord a, GeoCoord b) => a.DistanceSquared(b);
    }
}

using AgOpenGPS.Core.Models;
using System;

namespace AgOpenGPS.Core.Extensions
{
    /// <summary>
    /// Extension methods for vec2 struct.
    /// </summary>
    public static class Vec2Extensions
    {
        /// <summary>
        /// Determines if this vec2 is at the default/uninitialized state (0,0).
        /// </summary>
        /// <param name="v">The vec2 to check.</param>
        /// <param name="tolerance">Optional tolerance for comparison (default: 0.0001).</param>
        /// <returns>True if both easting and northing are effectively zero.</returns>
        public static bool IsDefault(this vec2 v, double tolerance = 0.0001)
        {
            return Math.Abs(v.easting) < tolerance && Math.Abs(v.northing) < tolerance;
        }

        /// <summary>
        /// Determines if two vec2 values are approximately equal within a tolerance.
        /// </summary>
        /// <param name="a">First vec2.</param>
        /// <param name="b">Second vec2.</param>
        /// <param name="tolerance">Tolerance for comparison (default: 0.0001).</param>
        /// <returns>True if both components are within tolerance.</returns>
        public static bool ApproximatelyEquals(this vec2 a, vec2 b, double tolerance = 0.0001)
        {
            return Math.Abs(a.easting - b.easting) < tolerance &&
                   Math.Abs(a.northing - b.northing) < tolerance;
        }
    }
}

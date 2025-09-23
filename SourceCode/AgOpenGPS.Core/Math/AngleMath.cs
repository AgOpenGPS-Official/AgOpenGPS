// AgOpenGPS.Core/Math/AngleMath.cs
// Purpose: Stateless angle utilities for radians/degree conversion and normalization.
using System;

namespace AgOpenGPS.Core.Mathx
{
    /// <summary>
    /// Stateless helpers for angle math. Angles are in radians unless stated otherwise.
    /// </summary>
    public static class AngleMath
    {
        /// <summary>2π constant.</summary>
        public const double TwoPi = 6.28318530717958647692;

        /// <summary>π/2 constant.</summary>
        public const double PiBy2 = 1.57079632679489661923;

        /// <summary>Converts degrees to radians.</summary>
        public static double ToRadians(double degrees) => degrees * (Math.PI / 180.0);

        /// <summary>Converts radians to degrees.</summary>
        public static double ToDegrees(double radians) => radians * (180.0 / Math.PI);

        /// <summary>
        /// Normalizes an angle in radians to the range [0, 2π).
        /// </summary>
        public static double NormalizePositive(double radians)
        {
            double x = radians % TwoPi;
            if (x < 0) x += TwoPi;
            return x;
        }

        /// <summary>
        /// Returns the signed smallest delta from 'from' to 'to' within (-π, π].
        /// Positive means rotate counter-clockwise from 'from' to reach 'to'.
        /// </summary>
        public static double ShortestDelta(double from, double to)
        {
            double delta = NormalizePositive(to) - NormalizePositive(from);
            if (delta > Math.PI) delta -= TwoPi;
            return delta;
        }

        /// <summary>
        /// Absolute angular difference in [0, π].
        /// </summary>
        public static double AngleDiff(double a, double b)
        {
            double d = Math.Abs(NormalizePositive(a) - NormalizePositive(b));
            if (d > Math.PI) d = TwoPi - d;
            return d;
        }

        /// <summary>
        /// Clamps a value within [min, max].
        /// </summary>
        public static double Clamp(double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}

using System;

namespace AgOpenGPS.Core.Models
{
    public static class Units
    {
        public static double DegreesToRadians(double degrees)
        {
            const double degreesToRadians = Math.PI / 180.0;
            return degrees * degreesToRadians;
        }

        public static double RadiansToDegrees(double radians)
        {
            const double radiansToDegrees = 180.0 / Math.PI;
            return radians * radiansToDegrees;
        }

    }
}

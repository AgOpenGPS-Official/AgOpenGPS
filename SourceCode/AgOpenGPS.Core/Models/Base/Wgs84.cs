using System;

namespace AgOpenGPS.Core.Models
{
    // Represents a coordinate in the World Geodetic System 1984
    public struct Wgs84
    {
        public Wgs84(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }

        public double DistanceInMeters(Wgs84 b)
        {
            const double EarthRadius = 6371 * 1000.0;
            double degreesToRad = Math.PI / 180.0;

            double aLatRad = Latitude * degreesToRad;
            double aLongRad = Longitude * degreesToRad;
            double bLatRad = b.Latitude * degreesToRad;
            double bLongRad = b.Longitude * degreesToRad;
            double sinHalfLongDelta = Math.Sin(0.5 * (bLongRad - aLongRad));
            double sinHalfLatDelta = Math.Sin(0.5 * (bLatRad - aLatRad));

            double d3 = sinHalfLatDelta * sinHalfLatDelta + Math.Cos(aLatRad) * Math.Cos(bLatRad) * sinHalfLongDelta * sinHalfLongDelta;
            return EarthRadius * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        public double DistanceInKiloMeters(Wgs84 b)
        {
            return 0.001 * DistanceInMeters(b);
        }
    }

}

using System;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>A point in metres on a <see cref="LocalPlane"/>: northing up, easting right.</summary>
    public readonly struct GeoPoint
    {
        public GeoPoint(double northing, double easting)
        {
            Northing = northing;
            Easting = easting;
        }

        public double Northing { get; }
        public double Easting { get; }

        public double DistanceTo(GeoPoint other)
        {
            double dn = other.Northing - Northing, de = other.Easting - Easting;
            return Math.Sqrt(dn * dn + de * de);
        }

        public override string ToString() => $"N {Northing:0.000} E {Easting:0.000}";
    }

    /// <summary>
    /// Flat local coordinates in metres around an origin, the same conversion the Windows
    /// AgOpenGPS uses (AgOpenGPS.Core LocalPlane), so lines and distances match. Accurate to
    /// centimetres over the size of a field.
    /// </summary>
    public sealed class LocalPlane
    {
        private readonly double _metersPerDegreeLat;

        public LocalPlane(double originLatitude, double originLongitude)
        {
            OriginLatitude = originLatitude;
            OriginLongitude = originLongitude;
            double lat = ToRad(originLatitude);
            _metersPerDegreeLat = 111132.92
                - 559.82 * Math.Cos(2.0 * lat)
                + 1.175 * Math.Cos(4.0 * lat)
                - 0.0023 * Math.Cos(6.0 * lat);
        }

        public double OriginLatitude { get; }
        public double OriginLongitude { get; }

        public GeoPoint ToLocal(double latitude, double longitude)
        {
            return new GeoPoint(
                (latitude - OriginLatitude) * _metersPerDegreeLat,
                (longitude - OriginLongitude) * MetersPerDegreeLon(latitude));
        }

        public void ToWgs84(GeoPoint p, out double latitude, out double longitude)
        {
            latitude = OriginLatitude + p.Northing / _metersPerDegreeLat;
            longitude = OriginLongitude + p.Easting / MetersPerDegreeLon(latitude);
        }

        private static double MetersPerDegreeLon(double latitude)
        {
            double lat = ToRad(latitude);
            return 111412.84 * Math.Cos(lat) - 93.5 * Math.Cos(3.0 * lat) + 0.118 * Math.Cos(5.0 * lat);
        }

        internal static double ToRad(double deg) => deg * Math.PI / 180.0;
        internal static double ToDeg(double rad) => rad * 180.0 / Math.PI;

        /// <summary>Normalises an angle to 0..360 degrees.</summary>
        public static double Wrap360(double deg)
        {
            deg %= 360.0;
            return deg < 0 ? deg + 360.0 : deg;
        }

        /// <summary>Signed difference a - b in -180..180 degrees.</summary>
        public static double AngleDiff(double a, double b)
        {
            double d = Wrap360(a - b);
            return d > 180.0 ? d - 360.0 : d;
        }
    }
}

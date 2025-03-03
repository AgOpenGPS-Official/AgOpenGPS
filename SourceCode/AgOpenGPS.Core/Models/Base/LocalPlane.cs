using System;

namespace AgOpenGPS.Core.Models
{
    public class LocalPlane
    {
        const double degreesToRadians = 2.0 * Math.PI / 360.0;
        //used to offset the antenna position to compensate for drift
        static public GeoDelta FixDelta = new GeoDelta(0, 0);

        private double _mPerDegreeLat;
        private double _mPerDegreeLon;

        public LocalPlane(Wgs84 origin)
        {
            Origin = origin;
            SetLocalMetersPerDegree();
        }

        public Wgs84 Origin { get; }

        public GeoCoord ConvertWgs84ToGeoCoord(Wgs84 latLon)
        {
            return new GeoCoord(
                (latLon.Latitude - Origin.Latitude) * _mPerDegreeLat,
                (latLon.Longitude - Origin.Longitude) * _mPerDegreeLon);
        }

        public Wgs84 ConvertGeoCoordToWgs84(GeoCoord geoCoord)
        {
            double lat = (geoCoord.Northing / _mPerDegreeLat) + Origin.Latitude;
            double lon = (geoCoord.Easting / _mPerDegreeLon) + Origin.Longitude;
            return new Wgs84(lat, lon);
        }

        private void SetLocalMetersPerDegree()
        {
            _mPerDegreeLat = 111132.92
                - 559.82 * Math.Cos(2.0 * Origin.Latitude * degreesToRadians)
                + 1.175 * Math.Cos(4.0 * Origin.Latitude * degreesToRadians)
                - 0.0023 * Math.Cos(6.0 * Origin.Latitude * degreesToRadians);

            _mPerDegreeLon =
                111412.84 * Math.Cos(Origin.Latitude * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * Origin.Latitude * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * Origin.Latitude * degreesToRadians);
        }

    }
}

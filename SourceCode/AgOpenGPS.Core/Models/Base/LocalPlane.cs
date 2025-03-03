using System;

namespace AgOpenGPS.Core.Models
{
    // An instance of LocalPlane defines the origin and the meaning of a local coordinate
    // system that uses Northing and Easting coordinates.
    public class LocalPlane
    {
        // Used to offset the antenna position to compensate for drift
        static public GeoDelta FixDelta = new GeoDelta(0, 0);

        private double _metersPerDegreeLat;
        private double _metersPerDegreeLon;

        public LocalPlane(Wgs84 origin)
        {
            Origin = origin;
            SetLocalMetersPerDegree();
        }

        public Wgs84 Origin { get; }

        public GeoCoord ConvertWgs84ToGeoCoord(Wgs84 latLon)
        {
            return new GeoCoord(
                (latLon.Latitude - Origin.Latitude) * _metersPerDegreeLat,
                (latLon.Longitude - Origin.Longitude) * _metersPerDegreeLon);
        }

        public Wgs84 ConvertGeoCoordToWgs84(GeoCoord geoCoord)
        {
            double lat = (geoCoord.Northing / _metersPerDegreeLat) + Origin.Latitude;
            double lon = (geoCoord.Easting / _metersPerDegreeLon) + Origin.Longitude;
            return new Wgs84(lat, lon);
        }

        // see https://en.wikipedia.org/wiki/Geographic_coordinate_system#Latitude_and_longitude
        private void SetLocalMetersPerDegree()
        {
            double originLatInRad = Units.DegreesToRadians(Origin.Latitude);
            _metersPerDegreeLat = 111132.92
                - 559.82 * Math.Cos(2.0 * originLatInRad)
                + 1.175 * Math.Cos(4.0 * originLatInRad)
                - 0.0023 * Math.Cos(6.0 * originLatInRad);

            _metersPerDegreeLon =
                111412.84 * Math.Cos(originLatInRad)
                - 93.5 * Math.Cos(3.0 * originLatInRad)
                + 0.118 * Math.Cos(5.0 * originLatInRad);
        }

    }
}

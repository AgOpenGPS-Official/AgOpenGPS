using System;

namespace AgOpenGPS.Core.Models
{
    public struct vec3
    {
        public double easting;
        public double northing;
        public double heading;

        public vec3(double easting, double northing, double heading)
        {
            this.easting = easting;
            this.northing = northing;
            this.heading = heading;
        }

        public vec3(vec3 v)
        {
            easting = v.easting;
            northing = v.northing;
            heading = v.heading;
        }

        public vec3(vec2 v, double heading = 0)
        {
            easting = v.easting;
            northing = v.northing;
            this.heading = heading;
        }

        public vec3(GeoCoord geoCoord, double heading = 0)
        {
            easting = geoCoord.Easting;
            northing = geoCoord.Northing;
            this.heading = heading;
        }

        public GeoCoord ToGeoCoord()
        {
            return new GeoCoord(northing, easting);
        }

        public vec2 ToVec2()
        {
            return new vec2(easting, northing);
        }
    }
}

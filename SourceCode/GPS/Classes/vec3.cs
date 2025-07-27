//Please, if you use this, share the improvements

using AgOpenGPS.Core.Models;
using System;
using System.Globalization;

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

        public vec3(GeoCoord geoCoord)
        {
            easting = geoCoord.Easting;
            northing = geoCoord.Northing;
            heading = 0.0;
        }

        public GeoCoord ToGeoCoord()
        {
            return new GeoCoord(northing, easting);
        }
    }

    public struct vecFix2Fix
    {
        public double easting; //easting
        public double distance; //distance since last point
        public double northing; //norting
        public int isSet;    //altitude

        public vecFix2Fix(double _easting, double _northing, double _distance, int _isSet)
        {
            this.easting = _easting;
            this.distance = _distance;
            this.northing = _northing;
            this.isSet = _isSet;
        }
    }

    public struct vec2
    {
        public double easting;
        public double northing;

        public vec2(double easting, double northing)
        {
            this.easting = easting;
            this.northing = northing;
        }

        public vec2(vec2 v)
        {
            easting = v.easting;
            northing = v.northing;
        }

        public vec2(GeoCoord geoCoord)
        {
            northing = geoCoord.Northing;
            easting = geoCoord.Easting;
        }

        public GeoCoord ToGeoCoord()
        {
            return new GeoCoord(northing, easting);
        }

        public static vec2 operator -(vec2 lhs, vec2 rhs)
        {
            return new vec2(lhs.easting - rhs.easting, lhs.northing - rhs.northing);
        }

        public double HeadingXZ()
        {
            return Math.Atan2(easting, northing);
        }

        public vec2 Normalize()
        {
            double length = GetLength();
            if (Math.Abs(length) < 0.000000000001)
            {
                throw new DivideByZeroException("Trying to normalize a vector with length of zero.");
            }

            return new vec2(easting / length, northing / length);
        }

        public double GetLength()
        {
            return Math.Sqrt((easting * easting) + (northing * northing));
        }

        public double GetLengthSquared()
        {
            return (easting * easting) + (northing * northing);
        }

        public static vec2 operator *(vec2 self, double s)
        {
            return new vec2(self.easting * s, self.northing * s);
        }

        public static vec2 operator +(vec2 lhs, vec2 rhs)
        {
            return new vec2(lhs.easting + rhs.easting, lhs.northing + rhs.northing);
        }

        public static vec2 Lerp(vec2 a, vec2 b, double t)
        {
            return new vec2(
                a.easting + (b.easting - a.easting) * t,
                a.northing + (b.northing - a.northing) * t
            );
        }

        public static float Cross(vec2 a, vec2 b)
        {
            return (float)(a.easting * b.northing - a.northing * b.easting);
        }

        public static double Dot(vec2 a, vec2 b)
        {
            return a.easting * b.easting + a.northing * b.northing;
        }

        public static bool IsPointOnSegment(vec2 a, vec2 b, vec2 p)
        {
            double lenSq = (b - a).GetLengthSquared();
            double proj = Dot(p - a, b - a) / lenSq;
            return proj >= 0 && proj <= 1;
        }

        public static vec2 ProjectOnSegment(vec2 a, vec2 b, vec2 p, out double t)
        {
            vec2 ab = b - a;
            double abLenSq = ab.GetLengthSquared();
            if (abLenSq < 1e-6)
            {
                t = 0;
                return a;
            }

            vec2 ap = p - a;
            t = Math.Max(0, Math.Min(1, Dot(ap, ab) / abLenSq));
            return a + ab * t;
        }
}



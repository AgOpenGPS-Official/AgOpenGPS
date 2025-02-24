using System;

namespace AgOpenGPS.Core.Models
{
    public struct GeoCoord
    {
        public GeoCoord(double northing, double easting)
        {
            Northing = northing;
            Easting = easting;
        }

        public double Northing { get; }
        public double Easting { get; }

        public double DistanceSquared(GeoCoord coord2)
        {
            return new GeoDelta(this, coord2).LengthSquared;
        }

        public double Distance(GeoCoord coord2)
        {
            return new GeoDelta(this, coord2).Length;
        }

        public GeoCoord Min(GeoCoord bCoord)
        {
            return new GeoCoord(Math.Min(this.Northing, bCoord.Northing), Math.Min(this.Easting, bCoord.Easting));
        }

        public GeoCoord Max(GeoCoord bCoord)
        {
            return new GeoCoord(Math.Max(this.Northing, bCoord.Northing), Math.Max(this.Easting, bCoord.Easting));
        }

        public GeoArea TriangleArea(GeoCoord b, GeoCoord c)
        {
            // AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)

            double area2 =
                Easting * (b.Northing - c.Northing) +
                b.Easting * (c.Northing - Northing) +
                c.Easting * (Northing - b.Northing);
            return new GeoArea(Math.Abs(0.5 * area2));
        }

        public GeoArea QuadArea(GeoCoord q2, GeoCoord q3, GeoCoord q4)
        {
            return TriangleArea(q2, q3) + TriangleArea(q3, q4);
        }

        public static GeoDelta operator -(GeoCoord aCoord, GeoCoord bCoord)
        {
            return new GeoDelta(aCoord.Northing - bCoord.Northing, aCoord.Easting - bCoord.Easting);
        }

        public static GeoCoord operator +(GeoCoord coord, GeoDelta delta)
        {
            return new GeoCoord(coord.Northing + delta.NorthingDelta, coord.Easting + delta.EastingDelta);
        }

        public static GeoCoord operator -(GeoCoord coord, GeoDelta delta)
        {
            return new GeoCoord(coord.Northing - delta.NorthingDelta, coord.Easting - delta.EastingDelta);
        }

    }
}

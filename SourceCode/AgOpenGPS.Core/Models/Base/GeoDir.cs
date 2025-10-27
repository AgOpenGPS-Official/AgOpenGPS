using System;

namespace AgOpenGPS.Core.Models
{
    public struct GeoDir
    {
        public GeoDir(double angleInRadians)
        {
            AngleInRadians = angleInRadians;
        }

        public GeoDir(GeoDelta delta)
        {
            AngleInRadians = Math.Atan2(delta.EastingDelta, delta.NorthingDelta);
        }
        public GeoDir(GeoCoord fromCoord, GeoCoord toCoord) : this(new GeoDelta(fromCoord, toCoord))
        {
        }

        public double AngleInRadians { get; }
        public double AngleInDegrees => Units.RadiansToDegrees(AngleInRadians);

        public GeoDir PerpendicularLeft => new GeoDir(AngleInRadians - 0.5 * Math.PI);

        public GeoDir PerpendicularRight => new GeoDir(AngleInRadians + 0.5 * Math.PI);
        public GeoDir Inverted => new GeoDir(AngleInRadians + Math.PI);

        public static GeoDelta operator *(double distance, GeoDir dir)
        {
            return new GeoDelta(distance * Math.Cos(dir.AngleInRadians), distance * Math.Sin(dir.AngleInRadians));
        }

        public static GeoDir operator +(GeoDir dir, double angleInRadians)
        {
            return new GeoDir(dir.AngleInRadians + angleInRadians);
        }

        public static GeoDir operator -(GeoDir dir, double angleInRadians)
        {
            return new GeoDir(dir.AngleInRadians - angleInRadians);
        }

    }
}
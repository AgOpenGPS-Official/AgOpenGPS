using System;

namespace AgOpenGPS.Core.Models
{
    public struct GeoDir
    {
        public GeoDir(double angle)
        {
            Angle = angle;
        }

        public GeoDir(GeoDelta delta)
        {
            Angle = Math.Atan2(delta.EastingDelta, delta.NorthingDelta);
        }

        public double Angle { get; private set; }

        public GeoDir PerpendicularLeft => new GeoDir(Angle - 0.5 * Math.PI);

        public GeoDir PerpendicularRight => new GeoDir(Angle + 0.5 * Math.PI);
        public GeoDir Inverted => new GeoDir(Angle + Math.PI);


        public void Invert()
        {
            Angle += Math.PI;
        }

        public static GeoDelta operator *(double distance, GeoDir dir)
        {
            return new GeoDelta(distance * Math.Cos(dir.Angle), distance * Math.Sin(dir.Angle));
        }

        public static GeoDir operator +(GeoDir dir, double angle)
        {
            return new GeoDir(dir.Angle + angle);
        }

        public static GeoDir operator -(GeoDir dir, double angle)
        {
            return new GeoDir(dir.Angle - angle);
        }

    }
}
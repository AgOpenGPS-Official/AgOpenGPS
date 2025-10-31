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
            if (AngleInRadians < 0.0) AngleInRadians += 2 * Math.PI;
        }

        // The returned angle will be in the range [0.0 ... 2 * PI >
        // Also internally, GeoDir will always keep the angle in this range, so it does not need to be checked
        // for each frame that wants to display the HeadingString.
        public double AngleInRadians { get; }

        // The returned angle will be in the range [0.0 ... 360 >
        public double AngleInDegrees => Units.RadiansToDegrees(AngleInRadians);

        public GeoDir PerpendicularLeft => new GeoDir(
            0.5 * Math.PI < AngleInRadians
            ? AngleInRadians - 0.5 * Math.PI
            : AngleInRadians + 1.5 * Math.PI);

        public GeoDir PerpendicularRight => new GeoDir(
            1.5 * Math.PI < AngleInRadians
            ? AngleInRadians - 1.5 * Math.PI
            : AngleInRadians + 0.5 * Math.PI);

        public GeoDir Inverted => new GeoDir(
            Math.PI < AngleInRadians
            ? AngleInRadians - Math.PI
            : AngleInRadians + Math.PI);

        public string HeadingString(string formatString = "N1")
        {
            return AngleInDegrees.ToString(formatString) + "\u00B0";
        }

        public static GeoDelta operator *(double distance, GeoDir dir)
        {
            return new GeoDelta(distance * Math.Cos(dir.AngleInRadians), distance * Math.Sin(dir.AngleInRadians));
        }

        public static GeoDir operator +(GeoDir dir, double angleInRadians)
        {
            return new GeoDir(NormalizeAngle(dir.AngleInRadians + angleInRadians));
        }

        public static GeoDir operator -(GeoDir dir, double angleInRadians)
        {
            return new GeoDir(NormalizeAngle(dir.AngleInRadians - angleInRadians));
        }

        private static double NormalizeAngle(double angleInRadians)
        {
            while (angleInRadians < 0.0) angleInRadians += 2 * Math.PI;
            while (2 * Math.PI < angleInRadians) angleInRadians -= 2 * Math.PI;
            return angleInRadians;
        }

    }
}
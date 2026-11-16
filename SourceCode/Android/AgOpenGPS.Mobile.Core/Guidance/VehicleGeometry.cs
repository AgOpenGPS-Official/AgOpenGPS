using System;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>
    /// Where the antenna sits on the tractor and where the implement works, in metres, as in the
    /// Windows AgOpenGPS (antenna height, antenna pivot, antenna offset, hitch length, tool offset).
    /// The pivot is the middle of the rear axle.
    /// </summary>
    public sealed class VehicleGeometry
    {
        /// <summary>Antenna height above the ground, for tilt (roll) correction. 0 = no correction.</summary>
        public double AntennaHeight { get; set; }
        /// <summary>Antenna ahead of the rear axle (negative = behind it).</summary>
        public double AntennaForward { get; set; }
        /// <summary>Antenna right of the tractor's centre line (negative = left).</summary>
        public double AntennaRight { get; set; }
        /// <summary>Implement's working line behind the rear axle (negative = front mounted).</summary>
        public double ToolBehind { get; set; }
        /// <summary>Implement's centre right of the tractor's centre line (negative = left).</summary>
        public double ToolRight { get; set; }

        public bool IsZero => AntennaHeight == 0 && AntennaForward == 0 && AntennaRight == 0 && ToolBehind == 0 && ToolRight == 0;

        /// <summary>
        /// From the antenna fix and heading, the point guidance follows (the implement's centre line,
        /// level with the rear axle) and the point where the implement works. Without a heading the
        /// antenna position is used for both. Roll in degrees, positive = right side down.
        /// </summary>
        public void Apply(GnssFix antenna, double headingDeg, out GnssFix guidePoint, out GnssFix toolPoint)
        {
            guidePoint = antenna;
            toolPoint = antenna;
            if (antenna == null || !antenna.HasPosition || double.IsNaN(headingDeg) || IsZero) return;

            var plane = new LocalPlane(antenna.Latitude, antenna.Longitude);
            double h = LocalPlane.ToRad(headingDeg);
            double fe = Math.Sin(h), fn = Math.Cos(h);   // forward (east, north)
            double re = Math.Cos(h), rn = -Math.Sin(h);  // right (east, north)

            // Leaning right moves the antenna right of the point on the ground below the tractor's centre.
            double roll = double.IsNaN(antenna.Roll) ? 0 : antenna.Roll;
            double side = AntennaRight + AntennaHeight * Math.Sin(LocalPlane.ToRad(roll));

            // Pivot = antenna moved back by its forward offset and sideways by its side offset.
            double pe = -re * side - fe * AntennaForward;
            double pn = -rn * side - fn * AntennaForward;

            guidePoint = Moved(antenna, plane, pn + rn * ToolRight, pe + re * ToolRight);
            toolPoint = Moved(antenna, plane, pn + rn * ToolRight - fn * ToolBehind, pe + re * ToolRight - fe * ToolBehind);
        }

        private static GnssFix Moved(GnssFix fix, LocalPlane plane, double north, double east)
        {
            plane.ToWgs84(new GeoPoint(north, east), out double lat, out double lon);
            GnssFix f = fix.Clone();
            f.Latitude = lat;
            f.Longitude = lon;
            return f;
        }
    }
}

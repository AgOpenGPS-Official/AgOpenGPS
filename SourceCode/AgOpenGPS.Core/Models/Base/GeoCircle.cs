using System;
using System.Diagnostics;

namespace AgOpenGPS.Core.Models
{
    public enum TurnType { Straight, Left, Right }

    public struct GeoCircle
    {

        public GeoCircle(GeoCoord center, double radius)
        {
            Center = center;
            Radius = radius;
        }

        public GeoCoord Center { get; }
        public double Radius { get; }

        public GeoCoord PointOnCircle(GeoDir dir)
        {
            return Center + Radius * dir;
        }

        public double GetArcLength(
            GeoCoord startPos,
            GeoCoord goalPos,
            TurnType turnType)
        {
            Debug.Assert(turnType != TurnType.Straight);
            GeoDir startDir = new GeoDir(startPos - Center);
            GeoDir goalDir = new GeoDir(goalPos - Center);

            double theta = goalDir.Angle - startDir.Angle;

            if (TurnType.Right == turnType)
            {
                if (theta < 0.0) theta += 2.0 * Math.PI;
            }
            else if (TurnType.Left == turnType)
            {
                if (theta > 0.0) theta -= 2.0 * Math.PI;
            }
            return Math.Abs(theta * Radius);
        }

    }
}

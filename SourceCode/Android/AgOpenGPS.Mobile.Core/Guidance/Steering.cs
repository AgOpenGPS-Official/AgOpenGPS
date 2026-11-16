using System;
using System.Collections.Generic;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>Where to steer: the goal point ahead on the pass and the angle to it.</summary>
    public sealed class SteerResult
    {
        /// <summary>Goal point on the current pass, on the line's plane.</summary>
        public GeoPoint GoalPoint;
        /// <summary>Distance ahead along the pass used to find the goal point, metres.</summary>
        public double LookaheadMeters;
        /// <summary>
        /// Angle from the vehicle heading to the goal point, degrees: positive = steer right,
        /// negative = steer left, 0 = on course. This is what the lightbar shows.
        /// </summary>
        public double SteerDegrees;
    }

    /// <summary>
    /// Pure pursuit, as in AgOpenGPS: from the point of the pass level with the vehicle, go
    /// <see cref="LookaheadMeters"/> further along it in the direction of travel. That goal point is
    /// always ahead, so the indication eases off as soon as the vehicle is turning back to the line
    /// instead of showing the full offset until it gets there.
    /// </summary>
    public static class Steering
    {
        public const double MinLookaheadMeters = 3.0;
        public const double MaxLookaheadMeters = 15.0;
        /// <summary>Seconds of driving to look ahead: 1.8 s is 6 m at 12 km/h.</summary>
        public const double LookaheadSeconds = 1.8;

        /// <summary>
        /// Pure pursuit front wheel angle for a goal point <paramref name="steerDegrees"/> off the
        /// heading at <paramref name="lookahead"/> metres: atan(2 * wheelbase * sin(alpha) / L).
        /// Positive = right; limited to +-35 degrees.
        /// </summary>
        public static double PurePursuitWheelAngle(double steerDegrees, double lookahead, double wheelbase)
        {
            if (double.IsNaN(steerDegrees) || lookahead <= 0) return 0;
            double a = LocalPlane.ToDeg(Math.Atan(2 * wheelbase * Math.Sin(LocalPlane.ToRad(steerDegrees)) / lookahead));
            return Math.Max(-35, Math.Min(35, a));
        }

        /// <summary>Look-ahead distance for a speed; slower means a closer goal point (but never under 3 m).</summary>
        public static double LookaheadMeters(double speedKmh)
        {
            double v = double.IsNaN(speedKmh) ? 0 : Math.Abs(speedKmh) / 3.6;
            return Math.Max(MinLookaheadMeters, Math.Min(MaxLookaheadMeters, v * LookaheadSeconds));
        }

        /// <summary>
        /// The goal point and steer angle on <paramref name="pass"/> (a polyline in the A to B
        /// direction, e.g. from <see cref="AbLine.PassPolyline"/>). Null without a vehicle heading.
        /// </summary>
        public static SteerResult Compute(IReadOnlyList<GeoPoint> pass, GeoPoint vehicle, double vehicleHeading,
            bool sameDirectionAsAb, double lookahead)
        {
            if (pass == null || pass.Count < 2 || double.IsNaN(vehicleHeading)) return null;

            // Walk the pass in the direction of travel.
            var pts = new List<GeoPoint>(pass);
            if (!sameDirectionAsAb) pts.Reverse();

            // Nearest point on the pass.
            int bestSeg = 0;
            double bestT = 0, bestD2 = double.MaxValue;
            for (int i = 0; i < pts.Count - 1; i++)
            {
                GeoPoint a = pts[i], b = pts[i + 1];
                double de = b.Easting - a.Easting, dn = b.Northing - a.Northing;
                double l2 = de * de + dn * dn;
                if (l2 < 1e-12) continue;
                double t = Math.Max(0, Math.Min(1, ((vehicle.Easting - a.Easting) * de + (vehicle.Northing - a.Northing) * dn) / l2));
                double pe = a.Easting + de * t - vehicle.Easting, pn = a.Northing + dn * t - vehicle.Northing;
                double d2 = pe * pe + pn * pn;
                if (d2 < bestD2) { bestD2 = d2; bestSeg = i; bestT = t; }
            }

            // From there, go forward along the pass by the look-ahead distance.
            GeoPoint goal = pts[pts.Count - 1];
            double remaining = lookahead;
            for (int i = bestSeg; i < pts.Count - 1; i++)
            {
                GeoPoint a = pts[i], b = pts[i + 1];
                double segLen = a.DistanceTo(b);
                double startT = i == bestSeg ? bestT : 0;
                double left = segLen * (1 - startT);
                if (left >= remaining && segLen > 1e-9)
                {
                    double t = startT + remaining / segLen;
                    goal = new GeoPoint(a.Northing + (b.Northing - a.Northing) * t, a.Easting + (b.Easting - a.Easting) * t);
                    remaining = 0;
                    break;
                }
                remaining -= left;
            }

            double bearing = LocalPlane.Wrap360(LocalPlane.ToDeg(Math.Atan2(goal.Easting - vehicle.Easting, goal.Northing - vehicle.Northing)));
            return new SteerResult
            {
                GoalPoint = goal,
                LookaheadMeters = lookahead,
                SteerDegrees = LocalPlane.AngleDiff(bearing, vehicleHeading),
            };
        }
    }
}

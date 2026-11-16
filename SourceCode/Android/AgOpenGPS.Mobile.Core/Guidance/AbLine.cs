using System;
using System.Collections.Generic;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    public enum LineKind
    {
        /// <summary>Straight line through A and B (AgOpenGPS "AB Line").</summary>
        Straight,
        /// <summary>The path driven from A to B (AgOpenGPS "AB Curve"), for fields without a straight edge.</summary>
        Curve,
    }

    public sealed class TrackPoint
    {
        public TrackPoint() { }
        public TrackPoint(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    /// <summary>
    /// A guidance line: straight through A and B, or a curve along the driven path. Stored as
    /// latitude/longitude so it stays valid however the local plane is chosen. The line itself is
    /// pass 0; passes 1, 2, ... lie one working width to its right (looking from A to B),
    /// -1, -2, ... to its left. Both ends continue straight beyond A and B.
    /// </summary>
    public sealed class AbLine
    {
        public string Id { get; set; } = Guid.NewGuid().ToString("N");
        public string Name { get; set; } = "";
        /// <summary>The field this line belongs to, or null for a line without a field.</summary>
        public string FieldId { get; set; }
        public LineKind Kind { get; set; } = LineKind.Straight;
        /// <summary>Sideways shift of the line and all its passes, metres, positive = right of A to B.</summary>
        public double NudgeMeters { get; set; }
        public double LatitudeA { get; set; }
        public double LongitudeA { get; set; }
        public double LatitudeB { get; set; }
        public double LongitudeB { get; set; }
        /// <summary>Curve only: the points from A to B (A and B included).</summary>
        public List<TrackPoint> Points { get; set; } = new List<TrackPoint>();
        public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;

        /// <summary>A plane with point A as origin: the most accurate one for this line.</summary>
        public LocalPlane CreatePlane() => new LocalPlane(LatitudeA, LongitudeA);

        /// <summary>The line on a plane: two points for a straight line, the path for a curve.</summary>
        public List<GeoPoint> ToLocal(LocalPlane plane)
        {
            var list = new List<GeoPoint>();
            if (Kind == LineKind.Curve && Points != null && Points.Count >= 2)
            {
                foreach (TrackPoint p in Points) list.Add(plane.ToLocal(p.Latitude, p.Longitude));
            }
            else
            {
                list.Add(plane.ToLocal(LatitudeA, LongitudeA));
                list.Add(plane.ToLocal(LatitudeB, LongitudeB));
            }
            return list;
        }

        /// <summary>Direction from A to B, degrees true (0..360).</summary>
        public double HeadingDegrees
        {
            get
            {
                LocalPlane plane = CreatePlane();
                GeoPoint b = plane.ToLocal(LatitudeB, LongitudeB);
                return LocalPlane.Wrap360(LocalPlane.ToDeg(Math.Atan2(b.Easting, b.Northing)));
            }
        }

        /// <summary>Length along the line from A to B in metres.</summary>
        public double LengthMeters
        {
            get
            {
                List<GeoPoint> pts = ToLocal(CreatePlane());
                double len = 0;
                for (int i = 1; i < pts.Count; i++) len += pts[i - 1].DistanceTo(pts[i]);
                return len;
            }
        }

        /// <summary>
        /// Pass <paramref name="pass"/> as a polyline on <paramref name="plane"/>: the line moved
        /// sideways by pass x width. Straight lines are extended by <paramref name="extend"/> metres
        /// at both ends so they cross the whole view; curves get straight extensions of that length.
        /// </summary>
        public List<GeoPoint> PassPolyline(LocalPlane plane, int pass, double width, double extend = 500)
        {
            List<GeoPoint> pts = ToLocal(plane);
            double offset = pass * width + NudgeMeters;
            var result = new List<GeoPoint>(pts.Count + 2);

            for (int i = 0; i < pts.Count; i++)
            {
                GeoPoint? prev = i > 0 ? pts[i - 1] : (GeoPoint?)null;
                GeoPoint? next = i < pts.Count - 1 ? pts[i + 1] : (GeoPoint?)null;
                result.Add(Geometry.OffsetPoint(pts[i], prev, next, offset));
            }

            if (result.Count >= 2 && extend > 0)
            {
                result.Insert(0, Extend(result[1], result[0], extend));
                result.Add(Extend(result[result.Count - 2], result[result.Count - 1], extend));
            }
            return result;
        }

        private static GeoPoint Extend(GeoPoint from, GeoPoint to, double by)
        {
            double de = to.Easting - from.Easting, dn = to.Northing - from.Northing;
            double l = Math.Sqrt(de * de + dn * dn);
            if (l < 1e-9) return to;
            return new GeoPoint(to.Northing + dn / l * by, to.Easting + de / l * by);
        }
    }

    /// <summary>Where the vehicle is relative to a line and its parallel passes.</summary>
    public sealed class AbGuidanceResult
    {
        /// <summary>Nearest pass: 0 is the recorded line, positive to its right (A to B), negative to its left.</summary>
        public int Pass;

        /// <summary>
        /// Distance from the nearest pass in metres, relative to the direction of travel:
        /// positive = the vehicle is right of the line, so steer left.
        /// </summary>
        public double CrossTrackMeters;

        /// <summary>True when driving from A towards B, false when driving back.</summary>
        public bool SameDirectionAsAb;

        /// <summary>Heading of the line at the vehicle in the direction of travel, degrees true.</summary>
        public double TravelLineHeading;

        /// <summary>Vehicle heading minus line heading, -180..180 (positive = turned right of the line). NaN without heading.</summary>
        public double HeadingErrorDegrees = double.NaN;

        /// <summary>Distance from the (nudged) line itself, metres, positive right of A to B.</summary>
        public double DistanceFromRefLine;

        /// <summary>Position along the line from A, metres (negative before A).</summary>
        public double AlongTrack;
    }

    public static class AbGuidance
    {
        /// <param name="line">The active line.</param>
        /// <param name="plane">A plane for the line (use <see cref="AbLine.CreatePlane"/>).</param>
        /// <param name="vehicle">Vehicle (antenna) position on the same plane.</param>
        /// <param name="vehicleHeading">Degrees true, or NaN when unknown (assumes A to B).</param>
        /// <param name="passWidth">Working width in metres (implement width minus overlap).</param>
        public static AbGuidanceResult Compute(AbLine line, LocalPlane plane, GeoPoint vehicle, double vehicleHeading, double passWidth)
        {
            return Compute(line.ToLocal(plane), vehicle, vehicleHeading, passWidth, line.NudgeMeters);
        }

        /// <summary>
        /// Guidance against a polyline (two points = straight line). The first and last segments
        /// continue straight beyond the ends. Distance is measured to the nearest point of the
        /// line, so passes on a curve are its true parallels as long as the curve radius is
        /// larger than their distance from it.
        /// </summary>
        public static AbGuidanceResult Compute(IReadOnlyList<GeoPoint> pts, GeoPoint vehicle, double vehicleHeading, double passWidth,
            double nudge = 0)
        {
            if (pts == null || pts.Count < 2) throw new ArgumentException("A line needs two points", nameof(pts));

            double bestDist2 = double.MaxValue, bestRight = 0, bestAlong = 0, segHeading = 0;
            double cumulative = 0;
            bool found = false;
            int last = pts.Count - 2;

            for (int i = 0; i <= last; i++)
            {
                GeoPoint a = pts[i], b = pts[i + 1];
                double de = b.Easting - a.Easting, dn = b.Northing - a.Northing;
                double len = Math.Sqrt(de * de + dn * dn);
                if (len < 1e-6) continue;
                double ue = de / len, un = dn / len;
                double re = vehicle.Easting - a.Easting, rn = vehicle.Northing - a.Northing;

                double t = re * ue + rn * un;
                double tc = t;
                if (i > 0 && tc < 0) tc = 0;             // only the first segment runs on before A
                if (i < last && tc > len) tc = len;      // only the last one runs on after B
                double pe = a.Easting + ue * tc, pn = a.Northing + un * tc;
                double d2 = (vehicle.Easting - pe) * (vehicle.Easting - pe) + (vehicle.Northing - pn) * (vehicle.Northing - pn);

                if (d2 < bestDist2 - 1e-12)
                {
                    bestDist2 = d2;
                    bestRight = re * un - rn * ue; // positive = right of the direction A->B
                    // At a corner the perpendicular distance and the corner distance differ: use the real distance.
                    if (tc != t) bestRight = Math.Sign(bestRight == 0 ? 1 : bestRight) * Math.Sqrt(d2);
                    bestAlong = cumulative + tc;
                    segHeading = LocalPlane.Wrap360(LocalPlane.ToDeg(Math.Atan2(ue, un)));
                    found = true;
                }
                cumulative += len;
            }
            if (!found) throw new ArgumentException("All points of the line are the same", nameof(pts));

            bestRight -= nudge; // measure from the nudged line
            int pass = 0;
            double offset = bestRight;
            if (passWidth > 0.01)
            {
                pass = (int)Math.Round(bestRight / passWidth, MidpointRounding.AwayFromZero);
                offset = bestRight - pass * passWidth;
            }

            bool same = double.IsNaN(vehicleHeading) || Math.Abs(LocalPlane.AngleDiff(vehicleHeading, segHeading)) <= 90.0;
            double travelHeading = same ? segHeading : LocalPlane.Wrap360(segHeading + 180.0);

            return new AbGuidanceResult
            {
                Pass = pass,
                CrossTrackMeters = same ? offset : -offset,
                SameDirectionAsAb = same,
                TravelLineHeading = travelHeading,
                HeadingErrorDegrees = double.IsNaN(vehicleHeading) ? double.NaN : LocalPlane.AngleDiff(vehicleHeading, travelHeading),
                DistanceFromRefLine = bestRight,
                AlongTrack = bestAlong,
            };
        }

        /// <summary>
        /// Douglas-Peucker: drops points that lie within <paramref name="tolerance"/> metres of the
        /// line through their neighbours. Keeps the first and last point.
        /// </summary>
        public static List<int> Simplify(IReadOnlyList<GeoPoint> pts, double tolerance)
        {
            var keep = new bool[pts.Count];
            if (pts.Count == 0) return new List<int>();
            keep[0] = keep[pts.Count - 1] = true;
            var stack = new Stack<(int, int)>();
            stack.Push((0, pts.Count - 1));
            while (stack.Count > 0)
            {
                (int s, int e) = stack.Pop();
                if (e <= s + 1) continue;
                double maxD = -1;
                int idx = -1;
                for (int i = s + 1; i < e; i++)
                {
                    double d = DistanceToSegment(pts[i], pts[s], pts[e]);
                    if (d > maxD) { maxD = d; idx = i; }
                }
                if (maxD > tolerance)
                {
                    keep[idx] = true;
                    stack.Push((s, idx));
                    stack.Push((idx, e));
                }
            }
            var result = new List<int>();
            for (int i = 0; i < keep.Length; i++) if (keep[i]) result.Add(i);
            return result;
        }

        private static double DistanceToSegment(GeoPoint p, GeoPoint a, GeoPoint b)
        {
            double de = b.Easting - a.Easting, dn = b.Northing - a.Northing;
            double l2 = de * de + dn * dn;
            double t = l2 < 1e-12 ? 0 : Math.Max(0, Math.Min(1, ((p.Easting - a.Easting) * de + (p.Northing - a.Northing) * dn) / l2));
            double qe = a.Easting + de * t - p.Easting, qn = a.Northing + dn * t - p.Northing;
            return Math.Sqrt(qe * qe + qn * qn);
        }
    }
}

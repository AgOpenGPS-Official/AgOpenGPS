using System;
using System.Collections.Generic;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>Polygon and polyline helpers on a <see cref="LocalPlane"/>.</summary>
    public static class Geometry
    {
        /// <summary>Area of a closed polygon in square metres (shoelace formula; the closing edge is implied).</summary>
        public static double Area(IReadOnlyList<GeoPoint> poly)
        {
            if (poly == null || poly.Count < 3) return 0;
            double sum = 0;
            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                sum += (poly[j].Easting * poly[i].Northing) - (poly[i].Easting * poly[j].Northing);
            return Math.Abs(sum) / 2.0;
        }

        /// <summary>True when the point lies inside the closed polygon (ray casting).</summary>
        public static bool Contains(IReadOnlyList<GeoPoint> poly, GeoPoint p)
        {
            if (poly == null || poly.Count < 3) return false;
            bool inside = false;
            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                GeoPoint a = poly[i], b = poly[j];
                if ((a.Northing > p.Northing) != (b.Northing > p.Northing))
                {
                    double e = (b.Easting - a.Easting) * (p.Northing - a.Northing) / (b.Northing - a.Northing) + a.Easting;
                    if (p.Easting < e) inside = !inside;
                }
            }
            return inside;
        }

        /// <summary>Shortest distance from the point to the edges of the closed polygon, metres.</summary>
        public static double DistanceToEdge(IReadOnlyList<GeoPoint> poly, GeoPoint p)
        {
            if (poly == null || poly.Count < 2) return double.NaN;
            double best = double.MaxValue;
            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
                best = Math.Min(best, DistanceToSegment(p, poly[j], poly[i]));
            return best;
        }

        public static double DistanceToSegment(GeoPoint p, GeoPoint a, GeoPoint b)
        {
            double de = b.Easting - a.Easting, dn = b.Northing - a.Northing;
            double l2 = de * de + dn * dn;
            double t = l2 < 1e-12 ? 0 : Math.Max(0, Math.Min(1, ((p.Easting - a.Easting) * de + (p.Northing - a.Northing) * dn) / l2));
            double qe = a.Easting + de * t - p.Easting, qn = a.Northing + dn * t - p.Northing;
            return Math.Sqrt(qe * qe + qn * qn);
        }

        /// <summary>
        /// Moves every point of a closed path sideways by <paramref name="offset"/> metres, to the
        /// right of the driving direction when positive, to the left when negative.
        /// </summary>
        public static List<GeoPoint> OffsetClosed(IReadOnlyList<GeoPoint> pts, double offset)
        {
            int n = pts.Count;
            var result = new List<GeoPoint>(n);
            if (n < 3 || offset == 0)
            {
                result.AddRange(pts);
                return result;
            }
            for (int i = 0; i < n; i++)
            {
                GeoPoint prev = pts[(i - 1 + n) % n], cur = pts[i], next = pts[(i + 1) % n];
                result.Add(OffsetPoint(cur, prev, next, offset));
            }
            return result;
        }

        /// <summary>
        /// Moves <paramref name="cur"/> sideways by <paramref name="offset"/> so that both adjacent
        /// segments end up exactly that far from the original (a mitred corner). Pass null for a
        /// missing neighbour at the end of an open line. Very sharp corners are limited to 3x.
        /// </summary>
        internal static GeoPoint OffsetPoint(GeoPoint cur, GeoPoint? prev, GeoPoint? next, double offset)
        {
            double e1 = 0, n1 = 0, e2 = 0, n2 = 0;
            bool has1 = prev.HasValue && RightNormal(prev.Value, cur, out e1, out n1);
            bool has2 = next.HasValue && RightNormal(cur, next.Value, out e2, out n2);
            if (!has1 && !has2) return cur;
            if (!has1) { e1 = e2; n1 = n2; }
            if (!has2) { e2 = e1; n2 = n1; }

            double be = e1 + e2, bn = n1 + n2;
            double l = Math.Sqrt(be * be + bn * bn);
            if (l < 1e-9) return cur; // the path turns back on itself
            be /= l; bn /= l;
            double cosHalf = be * e1 + bn * n1;
            double miter = offset / Math.Max(1.0 / 3.0, cosHalf);
            return new GeoPoint(cur.Northing + bn * miter, cur.Easting + be * miter);
        }

        private static bool RightNormal(GeoPoint a, GeoPoint b, out double ne, out double nn)
        {
            double de = b.Easting - a.Easting, dn = b.Northing - a.Northing;
            double l = Math.Sqrt(de * de + dn * dn);
            if (l < 1e-9) { ne = nn = 0; return false; }
            ne = dn / l;   // right of (de, dn) is (dn, -de) as (east, north)
            nn = -de / l;
            return true;
        }

        /// <summary>Moves points from one plane to another (through latitude/longitude).</summary>
        public static List<GeoPoint> Reproject(IReadOnlyList<GeoPoint> pts, LocalPlane from, LocalPlane to)
        {
            var result = new List<GeoPoint>(pts.Count);
            if (ReferenceEquals(from, to))
            {
                result.AddRange(pts);
                return result;
            }
            foreach (GeoPoint p in pts)
            {
                from.ToWgs84(p, out double lat, out double lon);
                result.Add(to.ToLocal(lat, lon));
            }
            return result;
        }
    }
}

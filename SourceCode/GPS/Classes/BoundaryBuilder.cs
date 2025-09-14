using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.IO;
using AgLibrary.Logging;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes
{
    public class BoundaryBuilder
    {
        #region Constants
        private const double INTERSECTION_TOLERANCE = 0.01;
        private const double INTERSECTION_TOLERANCE_SQ = INTERSECTION_TOLERANCE * INTERSECTION_TOLERANCE;
        private const double MAX_SEGMENT_STEP = 1.5;
        private const double INTERSECTION_SEARCH_RADIUS = 5.0;
        private const double TRIM_SEGMENT_LENGTH = 0.5;
        private const double MIN_POLYGON_POINTS = 3;
        #endregion

        #region Properties
        public List<CTrk> InputTracks { get; private set; } = new List<CTrk>();
        public List<Segment> Segments { get; private set; } = new List<Segment>();
        public List<vec2> IntersectionPoints { get; private set; } = new List<vec2>();
        public List<Segment> TrimmedSegments { get; private set; } = new List<Segment>();
        public List<vec3> FinalBoundary { get; private set; } = new List<vec3>();
        public CBoundaryList FinalizedBoundary { get; private set; }
        #endregion

        #region Public API
        public void SetTracks(List<CTrk> tracks)
        {
            InputTracks = tracks?.ToList() ?? new List<CTrk>();
            Log.EventWriter("Tracks set successfully");
        }

        public void ExtendAllTracks(double extendMeters)
        {
            var extendedTracks = new List<CTrk>();

            foreach (var trk in InputTracks)
            {
                var pts = trk.mode == TrackMode.AB
                    ? new List<GeoCoord>
                      {
                  new GeoCoord(trk.ptA.northing, trk.ptA.easting),
                  new GeoCoord(trk.ptB.northing, trk.ptB.easting)
                      }
                    : trk.curvePts
                        .Select(p => new GeoCoord(p.northing, p.easting))
                        .ToList();

                var extended = ExtendTrackEndpoints(pts, extendMeters);

                if (trk.mode == TrackMode.AB && extended.Count >= 2)
                {
                    trk.ptA = new vec2(extended[0].Easting, extended[0].Northing);
                    trk.ptB = new vec2(extended[extended.Count - 1].Easting,
                                       extended[extended.Count - 1].Northing);
                }
                else if (trk.mode == TrackMode.Curve)
                {
                    trk.curvePts = extended
                        .Select(p => new vec3(p.Easting, p.Northing, 0))
                        .ToList();
                }

                extendedTracks.Add(trk);
            }

            InputTracks = extendedTracks;
        }


        public List<vec3> BuildTrimmedBoundary()
        {
            try
            {
                if (InputTracks.Count == 0)
                {
                    Log.EventWriter("No input tracks to process");
                    return new List<vec3>();
                }

                BuildSegments();
                FindIntersections();

                var trimmed = TrimSegmentsToIntersections();
                var polygon = ConvertSegmentsToPolygon(trimmed);

                if (polygon.Count < MIN_POLYGON_POINTS)
                {
                    Log.EventWriter("Insufficient points for valid polygon");
                    return new List<vec3>();
                }

                FinalizedBoundary = FinalizeBoundaryPolygon(polygon);
                FinalBoundary = FinalizedBoundary?.fenceLine ?? new List<vec3>();

                Log.EventWriter($"Boundary created with {FinalBoundary.Count} points");
                return FinalBoundary;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error building boundary: {ex.Message}");
                return new List<vec3>();
            }
        }

        public bool SaveToBoundaryFile(string fieldDirectory)
        {
            try
            {
                if (FinalBoundary.Count < MIN_POLYGON_POINTS)
                {
                    Log.EventWriter("No valid boundary to save");
                    return false;
                }

                string path = Path.Combine(RegistrySettings.fieldsDirectory, fieldDirectory, "Boundary.txt");
                File.WriteAllLines(path, GetBoundaryFileLines(FinalizedBoundary));
                Log.EventWriter($"Boundary successfully saved to {path}");
                return true;
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Failed to save boundary: {ex.Message}");
                return false;
            }
        }
        #endregion

        #region Core Processing

        // Adds extra length to both ends of a track polyline (AB or Curve).
        // This helps ensure intersections are found just beyond original endpoints.
        private List<GeoCoord> ExtendTrackEndpoints(List<GeoCoord> pts, double extendMeters)
        {
            if (pts == null || pts.Count < 2) return pts;

            var result = new List<GeoCoord>(pts.Count + 2);

            // First segment direction
            var a0 = pts[0];
            var a1 = pts[1];
            var dirstart = new GeoDelta(a1, a0);
            var lengthStart = dirstart.Length;
            if (lengthStart > 1e-6)
            {
                var extend = extendMeters / lengthStart;
                var extStart = new GeoCoord(
                    a0.Northing + dirstart.NorthingDelta * extend,
                    a0.Easting + dirstart.EastingDelta * extend
                );
                result.Add(extStart);
            }

            for (int i = 0; i < pts.Count; i++) result.Add(pts[i]);

            var b0 = pts[pts.Count - 2];
            var b1 = pts[pts.Count - 1];
            var dirEnd = new GeoDelta(b0, b1);
            var lengthEnd = dirEnd.Length;
            if (lengthEnd > 1e-6)
            {
                var extend = extendMeters / lengthEnd;
                var extEnd = new GeoCoord(
                    b1.Northing + dirEnd.NorthingDelta * extend,
                    b1.Easting + dirEnd.EastingDelta * extend
                );
                result.Add(extEnd);
            }

            return result;
        }


        public void BuildSegments()
        {
            try
            {
                Segments = new List<Segment>();
                int totalSegments = 0;

                foreach (var trk in InputTracks)
                {
                    var points = GetTrackPoints(trk);
                    if (points.Count < 2) continue;

                    for (int i = 0; i < points.Count - 1; i++)
                    {
                        Segments.Add(new Segment(points[i], points[i + 1], trk));
                        totalSegments++;
                    }
                }

                Log.EventWriter($"Built {totalSegments} segments from {InputTracks.Count} tracks");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error building segments: {ex.Message}");
                Segments = new List<Segment>();
            }
        }

        public void FindIntersections()
        {
            try
            {
                var uniqueIntersections = new HashSet<vec2>(new Vec2EqualityComparer());
                int totalChecks = 0;
                int potentialIntersections = 0;

                for (int i = 0; i < Segments.Count; i++)
                {
                    var seg1 = Segments[i];

                    for (int j = i + 1; j < Segments.Count; j++)
                    {
                        var seg2 = Segments[j];
                        totalChecks++;

                        // Early exit conditions
                        if (seg1.ParentTrack == seg2.ParentTrack)
                            continue;

                        if (!BoundingBoxesIntersect(seg1, seg2, INTERSECTION_SEARCH_RADIUS))
                            continue;

                        potentialIntersections++;
                        var (intersects, intersection) = LineSegmentsIntersect(seg1.Start, seg1.End, seg2.Start, seg2.End);

                        if (intersects)
                        {
                            seg1.AddIntersection(intersection);
                            seg2.AddIntersection(intersection);
                            uniqueIntersections.Add(intersection);
                        }
                    }
                }

                IntersectionPoints = uniqueIntersections.ToList();
            }
            catch (Exception ex)
            {
                Log.EventWriter($"FindIntersections error: {ex.Message}");
            }
        }

        public List<Segment> TrimSegmentsToIntersections()
        {
            var trimmed = new List<Segment>();

            try
            {
                foreach (var trk in InputTracks)
                {
                    var trackSegments = Segments.Where(s => s.ParentTrack == trk).ToList();
                    if (trackSegments.Count == 0) continue;

                    var intersections = trackSegments
                        .SelectMany(s => s.Intersections)
                        .Distinct(new Vec2EqualityComparer())
                        .ToList();

                    if (intersections.Count < 2) continue;

                    var originalPoints = GetTrackPoints(trk);
                    if (originalPoints.Count < 2) continue;

                    var (startDist, endDist) = GetTrimDistances(originalPoints, intersections);
                    if (startDist >= endDist) continue;

                    var trimmedPoints = ExtractTrimmedPoints(originalPoints, startDist, endDist);
                    trimmed.AddRange(CreateUniformSegments(trimmedPoints, trk, TRIM_SEGMENT_LENGTH));
                }

                TrimmedSegments = trimmed;
                Log.EventWriter($"Trimmed {trimmed.Count} segments");
            }
            catch (Exception ex)
            {
                Log.EventWriter($"Error trimming segments: {ex.Message}");
                trimmed = new List<Segment>();
            }

            return trimmed;
        }
        #endregion



        #region Geometry Helpers
        private bool BoundingBoxesIntersect(Segment s1, Segment s2, double tolerance)
        {
            double minX1 = Math.Min(s1.Start.easting, s1.End.easting) - tolerance;
            double maxX1 = Math.Max(s1.Start.easting, s1.End.easting) + tolerance;
            double minY1 = Math.Min(s1.Start.northing, s1.End.northing) - tolerance;
            double maxY1 = Math.Max(s1.Start.northing, s1.End.northing) + tolerance;

            double minX2 = Math.Min(s2.Start.easting, s2.End.easting);
            double maxX2 = Math.Max(s2.Start.easting, s2.End.easting);
            double minY2 = Math.Min(s2.Start.northing, s2.End.northing);
            double maxY2 = Math.Max(s2.Start.northing, s2.End.northing);

            return !(maxX1 < minX2 || maxX2 < minX1 || maxY1 < minY2 || maxY2 < minY1);
        }
        private (bool intersects, vec2 intersection) LineSegmentsIntersect(vec2 p1, vec2 p2, vec2 p3, vec2 p4)
        {
            vec2 r = p2 - p1;
            vec2 s = p4 - p3;
            vec2 pq = p3 - p1;

            float rxs = vec2.Cross(r, s);
            float pqxr = vec2.Cross(pq, r);

            // Handle parallel/collinear cases first
            if (Math.Abs(rxs) < float.Epsilon)
            {
                // Collinear when pqxr is also near zero
                if (Math.Abs(pqxr) < float.Epsilon)
                {
                    // Collinear - check segment overlap
                    float t0 = (float)(vec2.Dot(pq, r) / vec2.Dot(r, r));
                    float t1 = t0 + (float)(vec2.Dot(s, r) / vec2.Dot(r, r));
                    if (t0 > t1) (t0, t1) = (t1, t0);

                    if (t0 <= 1 && t1 >= 0)
                    {
                        float intersectionT = Math.Max(0, Math.Min(t0, 1));
                        return (true, p1 + r * intersectionT);
                    }
                }
                return (false, default); // Parallel but not collinear
            }

            float t = vec2.Cross(pq, s) / rxs;
            float u = pqxr / rxs;

            if (t >= 0 && t <= 1 && u >= 0 && u <= 1)
            {
                return (true, p1 + r * t);
            }

            return (false, default);
        }
        #endregion

        #region Boundary Construction
        private List<vec3> ConvertSegmentsToPolygon(List<Segment> segments)
        {
            if (segments.Count == 0)
                return new List<vec3>();

            var polygon = new List<vec3>();
            var visited = new HashSet<Segment>();
            var current = segments[0];

            polygon.Add(current.Start.ToVec3());
            polygon.Add(current.End.ToVec3());
            visited.Add(current);

            while (visited.Count < segments.Count)
            {
                var next = FindConnectedSegment(segments, visited, current.End);
                if (next == null) break;

                polygon.Add(next.End.ToVec3());
                visited.Add(next);
                current = next;
            }

            return polygon;
        }

        private CBoundaryList FinalizeBoundaryPolygon(List<vec3> polygon)
        {
            if (polygon?.Count < MIN_POLYGON_POINTS)
                return null;

            var boundary = new CBoundaryList();
            boundary.fenceLine.AddRange(polygon);

            boundary.CalculateFenceArea(0);
            boundary.FixFenceLine(0);

            return boundary;
        }
        #endregion

        #region Helper Methods
        private List<vec2> GetTrackPoints(CTrk track)
        {
            List<vec2> points = track.mode == TrackMode.AB
                ? new List<vec2> { track.ptA, track.ptB }
                : track.curvePts.Select(p => new vec2(p.easting, p.northing)).ToList();

            return EnforceMaxStep(points, MAX_SEGMENT_STEP);
        }

        private List<vec2> EnforceMaxStep(List<vec2> points, double maxStep)
        {
            var result = new List<vec2>();
            if (points.Count == 0) return result;

            result.Add(points[0]);

            for (int i = 1; i < points.Count; i++)
            {
                vec2 prev = points[i - 1];
                vec2 current = points[i];
                double distance = (current - prev).GetLength();

                if (distance <= maxStep)
                {
                    result.Add(current);
                    continue;
                }

                // Insert interpolated points
                int steps = (int)Math.Ceiling(distance / maxStep);
                for (int s = 1; s < steps; s++)
                {
                    double t = (double)s / steps;
                    result.Add(vec2.Lerp(prev, current, t));
                }

                result.Add(current);
            }

            return result;
        }

        private (double startDist, double endDist) GetTrimDistances(List<vec2> points, List<vec2> intersections)
        {
            // Calculate cumulative distances along track
            var distances = new List<double> { 0 };
            for (int i = 1; i < points.Count; i++)
            {
                distances.Add(distances[i - 1] + glm.Distance(points[i - 1], points[i]));
            }

            // Project intersections to get their distances along track
            var intersectionDistances = new List<double>();
            foreach (var pt in intersections)
            {
                for (int i = 0; i < points.Count - 1; i++)
                {
                    if (vec2.IsPointOnSegment(points[i], points[i + 1], pt))
                    {
                        vec2.ProjectOnSegment(points[i], points[i + 1], pt, out double t);
                        intersectionDistances.Add(distances[i] + glm.Distance(points[i], pt));
                        break;
                    }
                }
            }

            return (intersectionDistances.Min(), intersectionDistances.Max());
        }

        private List<vec2> ExtractTrimmedPoints(List<vec2> points, double startDist, double endDist)
        {
            var trimmed = new List<vec2>();
            double accumulatedDist = 0;

            for (int i = 0; i < points.Count - 1; i++)
            {
                vec2 a = points[i];
                vec2 b = points[i + 1];
                double segmentLength = glm.Distance(a, b);
                double segmentStart = accumulatedDist;
                double segmentEnd = accumulatedDist + segmentLength;

                // Skip segments completely outside trim range
                if (segmentEnd < startDist || segmentStart > endDist)
                {
                    accumulatedDist = segmentEnd;
                    continue;
                }

                // Calculate trim points on this segment
                double t1 = Math.Max(0, (startDist - segmentStart) / segmentLength);
                double t2 = Math.Min(1, (endDist - segmentStart) / segmentLength);

                vec2 p1 = vec2.Lerp(a, b, t1);
                vec2 p2 = vec2.Lerp(a, b, t2);

                if (!trimmed.Any() || glm.Distance(trimmed.Last(), p1) > INTERSECTION_TOLERANCE)
                {
                    trimmed.Add(p1);
                }

                if (glm.Distance(p1, p2) > INTERSECTION_TOLERANCE)
                {
                    trimmed.Add(p2);
                }

                accumulatedDist = segmentEnd;
            }

            return trimmed;
        }

        private List<Segment> CreateUniformSegments(List<vec2> points, CTrk parentTrack, double segmentLength)
        {
            var segments = new List<Segment>();

            for (int i = 0; i < points.Count - 1; i++)
            {
                vec2 start = points[i];
                vec2 end = points[i + 1];
                double distance = glm.Distance(start, end);
                int steps = Math.Max(1, (int)(distance / segmentLength));

                for (int j = 0; j < steps; j++)
                {
                    double t1 = (double)j / steps;
                    double t2 = (double)(j + 1) / steps;

                    segments.Add(new Segment(
                        vec2.Lerp(start, end, t1),
                        vec2.Lerp(start, end, t2),
                        parentTrack));
                }
            }

            return segments;
        }

        private Segment FindConnectedSegment(List<Segment> segments, HashSet<Segment> visited, vec2 connectionPoint)
        {
            foreach (var seg in segments)
            {
                if (visited.Contains(seg)) continue;

                if (glm.Distance(seg.Start, connectionPoint) < INTERSECTION_TOLERANCE)
                {
                    return seg;
                }

                if (glm.Distance(seg.End, connectionPoint) < INTERSECTION_TOLERANCE)
                {
                    seg.Reverse();
                    return seg;
                }
            }
            return null;
        }

        private IEnumerable<string> GetBoundaryFileLines(CBoundaryList bnd)
        {
            yield return "$Boundary";
            yield return bnd.isDriveThru.ToString();
            yield return bnd.fenceLine.Count.ToString();

            foreach (vec3 pt in bnd.fenceLine)
            {
                yield return FormattableString.Invariant(
                    $"{pt.easting:F6},{pt.northing:F6},{pt.heading:F6}");
            }
        }

        #endregion

        #region Static Helpers

        #endregion

        #region Helper Classes
        public class Segment
        {
            public vec2 Start { get; private set; }
            public vec2 End { get; private set; }
            public CTrk ParentTrack { get; }
            public List<vec2> Intersections { get; } = new List<vec2>();

            public Segment(vec2 start, vec2 end, CTrk parentTrack)
            {
                Start = start;
                End = end;
                ParentTrack = parentTrack;
            }

            public void Reverse() => (Start, End) = (End, Start);

            public void AddIntersection(vec2 point)
            {
                if (!Intersections.Any(p => (p - point).GetLengthSquared() < INTERSECTION_TOLERANCE_SQ))
                {
                    Intersections.Add(point);
                }
            }
        }

        private class Vec2EqualityComparer : IEqualityComparer<vec2>
        {
            public bool Equals(vec2 a, vec2 b) =>
                (a - b).GetLengthSquared() < INTERSECTION_TOLERANCE_SQ;

            public int GetHashCode(vec2 p) =>
                HashCode.Combine(
                    Math.Round(p.easting / INTERSECTION_TOLERANCE),
                    Math.Round(p.northing / INTERSECTION_TOLERANCE)
                );
        }
        #endregion
    }

    public static class Vec2Extensions
    {
        public static vec3 ToVec3(this vec2 vector, double heading = 0) =>
            new vec3(vector.easting, vector.northing, heading);
    }
}
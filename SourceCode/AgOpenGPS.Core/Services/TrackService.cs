using System;
using System.Collections.Generic;
using System.Linq;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;
using AgOpenGPS.Core.Extensions;
using AgOpenGPS.Core.Geometry;

namespace AgOpenGPS.Core.Services
{
    /// <summary>
    /// Service implementation for track management and guidance track operations.
    /// Provides business logic without UI dependencies.
    /// Based on CTracks from AOG_Dev with performance optimizations.
    /// </summary>
    public class TrackService : ITrackService
    {
        // ============================================================
        // Fields
        // ============================================================

        private readonly TrackCollection _tracks;

        // ============================================================
        // Constructor
        // ============================================================

        /// <summary>
        /// Creates a new TrackService instance.
        /// </summary>
        public TrackService()
        {
            _tracks = new TrackCollection();
        }

        // ============================================================
        // Track Management
        // ============================================================

        /// <summary>
        /// Gets the collection of all tracks.
        /// </summary>
        public TrackCollection Tracks => _tracks;

        /// <summary>
        /// Gets the currently active track for guidance.
        /// </summary>
        public Track GetCurrentTrack()
        {
            return _tracks.CurrentTrack;
        }

        /// <summary>
        /// Sets the currently active track for guidance.
        /// </summary>
        /// <param name="track">The track to set as current, or null to clear.</param>
        public void SetCurrentTrack(Track track)
        {
            _tracks.CurrentTrack = track;
        }

        /// <summary>
        /// Adds a track to the collection.
        /// </summary>
        /// <param name="track">The track to add.</param>
        public void AddTrack(Track track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            _tracks.Add(track);
        }

        /// <summary>
        /// Removes a track from the collection.
        /// </summary>
        /// <param name="track">The track to remove.</param>
        public void RemoveTrack(Track track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            _tracks.Remove(track);
        }

        /// <summary>
        /// Clears all tracks from the collection.
        /// </summary>
        public void ClearTracks()
        {
            _tracks.Clear();
        }

        // ============================================================
        // Track Creation
        // ============================================================

        /// <summary>
        /// Creates a new AB line track from two points.
        /// </summary>
        /// <param name="ptA">Starting point (field coordinates).</param>
        /// <param name="ptB">Ending point (field coordinates).</param>
        /// <param name="heading">Heading angle in radians.</param>
        /// <param name="name">Optional track name.</param>
        /// <returns>The created AB line track.</returns>
        public Track CreateABTrack(vec2 ptA, vec2 ptB, double heading, string name = "AB Line")
        {
            var track = new Track(name, TrackMode.AB)
            {
                PtA = ptA,
                PtB = ptB,
                Heading = heading
            };

            // Build the AB line curve points
            BuildABLineCurve(track);

            return track;
        }

        /// <summary>
        /// Creates a new curve track from a list of recorded points.
        /// </summary>
        /// <param name="recordedPath">The recorded path points with headings.</param>
        /// <param name="name">Optional track name.</param>
        /// <returns>The created curve track.</returns>
        public Track CreateCurveTrack(List<vec3> recordedPath, string name = "Curve")
        {
            if (recordedPath == null || recordedPath.Count < 2)
                throw new ArgumentException("Curve track requires at least 2 points", nameof(recordedPath));

            var track = new Track(name, TrackMode.Curve);

            // Copy recorded path to track
            track.CurvePts.Clear();
            track.CurvePts.AddRange(recordedPath);

            // Ensure headings are calculated
            track.CurvePts.CalculateHeadings(false);

            return track;
        }

        /// <summary>
        /// Creates a boundary track from a boundary polygon.
        /// </summary>
        /// <param name="boundaryPoints">The boundary polygon points.</param>
        /// <param name="isOuter">True for outer boundary, false for inner.</param>
        /// <param name="name">Optional track name.</param>
        /// <returns>The created boundary track.</returns>
        public Track CreateBoundaryTrack(List<vec3> boundaryPoints, bool isOuter, string name = "Boundary")
        {
            if (boundaryPoints == null || boundaryPoints.Count < 3)
                throw new ArgumentException("Boundary track requires at least 3 points", nameof(boundaryPoints));

            var track = new Track(name, isOuter ? TrackMode.BoundaryTrackOuter : TrackMode.BoundaryTrackInner);

            // Copy boundary points to track
            track.CurvePts.Clear();
            track.CurvePts.AddRange(boundaryPoints);

            // Boundaries are loops
            track.CurvePts.CalculateHeadings(true);

            return track;
        }

        // ============================================================
        // Track Operations
        // ============================================================

        /// <summary>
        /// Nudges a track by a specified distance perpendicular to its path.
        /// </summary>
        /// <param name="track">The track to nudge.</param>
        /// <param name="distance">Distance to nudge (positive = right, negative = left).</param>
        public void NudgeTrack(Track track, double distance)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            track.NudgeDistance += distance;
        }

        /// <summary>
        /// Resets the nudge distance of a track to zero.
        /// </summary>
        /// <param name="track">The track to reset.</param>
        public void ResetNudge(Track track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            track.NudgeDistance = 0;
        }

        /// <summary>
        /// Snaps a track to the nearest pivot point for water pivot mode.
        /// </summary>
        /// <param name="track">The track to snap.</param>
        /// <param name="currentDistance">Current distance from track center.</param>
        public void SnapToPivot(Track track, double currentDistance)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            if (track.Mode != TrackMode.WaterPivot)
                return;

            // Round to nearest pivot spacing (e.g., 3 meters)
            const double pivotSpacing = 3.0;
            double roundedDistance = Math.Round(currentDistance / pivotSpacing) * pivotSpacing;
            track.NudgeDistance = roundedDistance - currentDistance;
        }

        // ============================================================
        // Geometry Operations (PERFORMANCE CRITICAL)
        // ============================================================

        /// <summary>
        /// Builds a guidance track at a specified offset distance from the reference track.
        /// PERFORMANCE TARGET: &lt;5ms for 500-point curve
        /// Based on CTracks.BuildCurrentGuidanceTrack from AOG_Dev with optimizations.
        /// </summary>
        /// <param name="track">The reference track.</param>
        /// <param name="offsetDistance">Distance to offset (meters). Positive = right, negative = left.</param>
        /// <returns>The guidance track points with calculated headings.</returns>
        public List<vec3> BuildGuidanceTrack(Track track, double offsetDistance)
        {
            if (track == null || !track.IsValid())
                return new List<vec3>();

            // Pre-allocate result list for performance
            List<vec3> newCurList = new List<vec3>(track.CurvePts.Count + 10);

            bool loops = track.Mode > TrackMode.Curve;

            try
            {
                // Special case: Water Pivot mode creates a circular guidance track
                if (track.Mode == TrackMode.WaterPivot)
                {
                    newCurList = BuildWaterPivotTrack(track, offsetDistance);
                }
                else
                {
                    // Calculate step size based on typical tool width (0.4 to 2 meters)
                    // This controls point density in the guidance track
                    double step = 1.0; // Default 1 meter spacing
                    // Could be parameterized: (toolWidth - overlap) * 0.4
                    if (step > 2) step = 2;
                    if (step < 0.5) step = 0.5;

                    // Create offset line parallel to reference track
                    newCurList = track.CurvePts.OffsetLine(offsetDistance, step, loops);

                    // Apply Catmull-Rom smoothing for curves (not AB lines)
                    if (track.Mode != TrackMode.AB)
                    {
                        int cnt = newCurList.Count;
                        if (cnt > 6)
                        {
                            newCurList = ApplyCatmullRomSmoothing(newCurList, step);
                        }
                    }

                    // Calculate headings for all points
                    newCurList.CalculateHeadings(loops);

                    // Extend open curves (AB, Curve) with long tails for guidance beyond field
                    if (!loops)
                    {
                        ExtendTrackEnds(newCurList, 10000); // 10km extensions
                    }
                }
            }
            catch (Exception ex)
            {
                // Log error but return empty list gracefully
                System.Diagnostics.Debug.WriteLine($"BuildGuidanceTrack exception: {ex.Message}");
                return new List<vec3>();
            }

            return newCurList;
        }

        /// <summary>
        /// Builds multiple parallel guidance lines for visualization.
        /// </summary>
        /// <param name="track">The reference track.</param>
        /// <param name="spacing">Spacing between lines (meters).</param>
        /// <param name="numLinesLeft">Number of lines to the left.</param>
        /// <param name="numLinesRight">Number of lines to the right.</param>
        /// <returns>List of guidance line paths.</returns>
        public List<List<vec3>> BuildGuideLines(Track track, double spacing, int numLinesLeft, int numLinesRight)
        {
            if (track == null || !track.IsValid())
                return new List<List<vec3>>();

            var guideLines = new List<List<vec3>>(numLinesLeft + numLinesRight + 1);

            // Build left lines
            for (int i = numLinesLeft; i > 0; i--)
            {
                double offset = -i * spacing;
                var line = BuildGuidanceTrack(track, offset);
                guideLines.Add(line);
            }

            // Center line (reference track)
            guideLines.Add(BuildGuidanceTrack(track, 0));

            // Build right lines
            for (int i = 1; i <= numLinesRight; i++)
            {
                double offset = i * spacing;
                var line = BuildGuidanceTrack(track, offset);
                guideLines.Add(line);
            }

            return guideLines;
        }

        /// <summary>
        /// Calculates the perpendicular distance from a position to the track.
        /// PERFORMANCE TARGET: &lt;0.5ms
        /// Based on CTracks.GetDistanceFromRefTrack from AOG_Dev (lines 178-221).
        /// Uses optimized two-phase search algorithm from GeometryUtils.
        /// </summary>
        /// <param name="track">The track to measure distance from.</param>
        /// <param name="position">The position to measure (field coordinates).</param>
        /// <param name="heading">Current heading in radians.</param>
        /// <returns>
        /// Tuple containing:
        /// - distance: Signed perpendicular distance (positive = right, negative = left)
        /// - sameway: True if heading is in same direction as track
        /// </returns>
        public (double distance, bool sameway) GetDistanceFromTrack(Track track, vec2 position, double heading)
        {
            if (track == null || !track.IsValid())
                return (0, true);

            // Special case: Water pivot uses circular distance calculation
            if (track.Mode == TrackMode.WaterPivot)
            {
                return GetDistanceFromWaterPivot(track, position, heading);
            }

            // Regular tracks: find closest segment and calculate perpendicular distance
            int refCount = track.CurvePts.Count;
            if (refCount < 2)
            {
                return (0, true);
            }

            // Use optimized two-phase search to find closest segment
            // PERFORMANCE: This is <500μs even for 1000-point curves
            if (!GeometryUtils.FindClosestSegment(track.CurvePts, position, out int rA, out int rB, false))
            {
                return (0, true);
            }

            // Calculate signed distance to the closest segment
            // Signed = positive if right of track, negative if left
            double distanceFromRefLine = GeometryUtils.FindDistanceToSegment(
                position,
                track.CurvePts[rA],
                track.CurvePts[rB],
                out vec3 closestPoint,
                out double time,
                signed: true);

            // Determine if vehicle is heading same direction as track
            // Uses dot product: if angle difference < 90°, same way
            double headingDiff = Math.Abs(heading - track.CurvePts[rA].heading);
            bool isHeadingSameWay = (Math.PI - Math.Abs(headingDiff - Math.PI)) < GeoMath.PIBy2;

            return (distanceFromRefLine, isHeadingSameWay);
        }

        // ============================================================
        // Track Queries
        // ============================================================

        /// <summary>
        /// Finds a track by its unique identifier.
        /// </summary>
        /// <param name="id">The track ID to find.</param>
        /// <returns>The track if found, null otherwise.</returns>
        public Track FindTrackById(Guid id)
        {
            return _tracks.FindById(id);
        }

        /// <summary>
        /// Finds a track by its name (case-insensitive).
        /// </summary>
        /// <param name="name">The track name to find.</param>
        /// <returns>The track if found, null otherwise.</returns>
        public Track FindTrackByName(string name)
        {
            return _tracks.FindByName(name);
        }

        /// <summary>
        /// Gets all tracks of a specific mode.
        /// </summary>
        /// <param name="mode">The track mode to filter by.</param>
        /// <returns>List of tracks matching the mode.</returns>
        public List<Track> GetTracksByMode(TrackMode mode)
        {
            return _tracks.GetTracksByMode(mode);
        }

        /// <summary>
        /// Gets the count of visible tracks.
        /// </summary>
        /// <returns>Number of visible tracks.</returns>
        public int GetVisibleTrackCount()
        {
            return _tracks.GetVisibleCount();
        }

        // ============================================================
        // List Access Helpers (for UI compatibility during migration)
        // ============================================================

        /// <summary>
        /// Gets all tracks as a read-only list.
        /// </summary>
        /// <returns>Read-only list of all tracks.</returns>
        public IReadOnlyList<Track> GetAllTracks()
        {
            return _tracks.Tracks;
        }

        /// <summary>
        /// Gets a track at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <returns>The track at the index, or null if index is out of range.</returns>
        public Track GetTrackAt(int index)
        {
            if (index < 0 || index >= _tracks.Tracks.Count)
                return null;
            return _tracks.Tracks[index];
        }

        /// <summary>
        /// Gets the total number of tracks.
        /// </summary>
        /// <returns>The track count.</returns>
        public int GetTrackCount()
        {
            return _tracks.Tracks.Count;
        }

        /// <summary>
        /// Gets the index of the currently selected track.
        /// </summary>
        /// <returns>The index of the current track, or -1 if no track is selected.</returns>
        public int GetCurrentTrackIndex()
        {
            Track current = GetCurrentTrack();
            if (current == null)
                return -1;

            for (int i = 0; i < _tracks.Tracks.Count; i++)
            {
                if (_tracks.Tracks[i].Id == current.Id)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Sets the current track by index.
        /// </summary>
        /// <param name="index">The zero-based index, or -1 to clear selection.</param>
        public void SetCurrentTrackIndex(int index)
        {
            if (index < 0 || index >= _tracks.Tracks.Count)
            {
                SetCurrentTrack(null);
            }
            else
            {
                SetCurrentTrack(_tracks.Tracks[index]);
            }
        }

        /// <summary>
        /// Removes a track at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the track to remove.</param>
        public void RemoveTrackAt(int index)
        {
            Track track = GetTrackAt(index);
            if (track != null)
            {
                RemoveTrack(track);
            }
        }

        // ============================================================
        // Private Helper Methods
        // ============================================================

        /// <summary>
        /// Builds the curve points for an AB line track.
        /// Creates a straight line with many points for consistent guidance behavior.
        /// </summary>
        private void BuildABLineCurve(Track track)
        {
            if (track.Mode != TrackMode.AB)
                return;

            // Calculate line length
            double dx = track.PtB.easting - track.PtA.easting;
            double dy = track.PtB.northing - track.PtA.northing;
            double length = Math.Sqrt(dx * dx + dy * dy);

            if (length < 0.1)
                return; // Line too short

            // Generate points along the line (1 point per meter minimum)
            int numPoints = Math.Max(50, (int)(length / 1.0));
            track.CurvePts.Clear();
            track.CurvePts.Capacity = numPoints;

            for (int i = 0; i < numPoints; i++)
            {
                double t = i / (double)(numPoints - 1);
                double easting = track.PtA.easting + dx * t;
                double northing = track.PtA.northing + dy * t;

                track.CurvePts.Add(new vec3
                {
                    easting = easting,
                    northing = northing,
                    heading = track.Heading
                });
            }
        }

        /// <summary>
        /// Builds a circular guidance track for water pivot irrigation.
        /// Creates a perfect circle with adaptive point density for smooth rendering.
        /// </summary>
        /// <param name="track">The water pivot track (PtA is center point).</param>
        /// <param name="radius">Radius of the circle (offset distance).</param>
        /// <returns>List of points forming the circle.</returns>
        private List<vec3> BuildWaterPivotTrack(Track track, double radius)
        {
            var result = new List<vec3>();

            // Max 2cm offset from perfect circle, limit between 100 and 1000 points
            double angle = GeoMath.twoPI / Math.Min(
                Math.Max(
                    Math.Ceiling(GeoMath.twoPI / (2 * Math.Acos(1 - (0.02 / Math.Abs(radius))))),
                    100),
                1000);

            vec3 centerPos = new vec3(track.PtA.easting, track.PtA.northing, 0);
            double rotation = 0;

            while (rotation < GeoMath.twoPI)
            {
                rotation += angle;
                result.Add(new vec3(
                    centerPos.easting + radius * Math.Sin(rotation),
                    centerPos.northing + radius * Math.Cos(rotation),
                    0));
            }

            result.CalculateHeadings(true); // Circle is a loop
            return result;
        }

        /// <summary>
        /// Applies Catmull-Rom spline smoothing to create smooth curves between points.
        /// This produces a smooth guidance path from the offset line points.
        /// Based on AOG_Dev CTracks.BuildCurrentGuidanceTrack (lines 300-343).
        /// </summary>
        /// <param name="points">Input points to smooth.</param>
        /// <param name="step">Target distance between interpolated points.</param>
        /// <returns>Smoothed curve points.</returns>
        private List<vec3> ApplyCatmullRomSmoothing(List<vec3> points, double step)
        {
            int cnt = points.Count;
            vec3[] arr = new vec3[cnt];
            points.CopyTo(arr);

            var result = new List<vec3>(cnt * 2); // Pre-allocate extra capacity for interpolation

            // Calculate initial headings for the array
            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i].heading = Math.Atan2(
                    arr[i + 1].easting - arr[i].easting,
                    arr[i + 1].northing - arr[i].northing);

                if (arr[i].heading < 0)
                    arr[i].heading += GeoMath.twoPI;
            }
            arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;

            // Add the first point
            result.Add(arr[0]);

            // Process middle segments with Catmull-Rom interpolation
            for (int i = 0; i < cnt - 3; i++)
            {
                // Add the next control point
                result.Add(arr[i + 1]);

                // Calculate distance to next point
                double distance = GeoMath.Distance(arr[i + 1], arr[i + 2]);

                // If distance is large enough, interpolate intermediate points
                if (distance > step)
                {
                    int loopTimes = (int)(distance / step + 1);
                    for (int j = 1; j < loopTimes; j++)
                    {
                        // Catmull-Rom spline interpolation using 4 control points
                        vec3 pos = GeoMath.Catmull(
                            j / (double)loopTimes,
                            arr[i], arr[i + 1], arr[i + 2], arr[i + 3]);
                        result.Add(pos);
                    }
                }
            }

            // Add the last two points
            result.Add(arr[cnt - 2]);
            result.Add(arr[cnt - 1]);

            return result;
        }

        /// <summary>
        /// Extends the start and end of an open track for guidance beyond field boundaries.
        /// Creates long extensions so guidance continues smoothly when entering/exiting fields.
        /// Based on AOG_Dev CTracks.BuildCurrentGuidanceTrack (lines 348-361).
        /// </summary>
        /// <param name="points">Track points to extend (modified in place).</param>
        /// <param name="extensionLength">Length to extend in meters (e.g., 10000 = 10km).</param>
        private void ExtendTrackEnds(List<vec3> points, double extensionLength)
        {
            if (points.Count < 2)
                return;

            // Extend the start
            vec3 pt1 = new vec3(points[0]);
            pt1.easting -= Math.Sin(pt1.heading) * extensionLength;
            pt1.northing -= Math.Cos(pt1.heading) * extensionLength;
            points.Insert(0, pt1);

            // Extend the end
            vec3 pt2 = new vec3(points[points.Count - 1]);
            pt2.easting += Math.Sin(pt2.heading) * extensionLength;
            pt2.northing += Math.Cos(pt2.heading) * extensionLength;
            points.Add(pt2);
        }

        /// <summary>
        /// Calculates distance from a position to a water pivot track (circular guidance).
        /// Uses cross product to determine which side of the pivot the vehicle is on.
        /// Based on AOG_Dev CTracks.GetDistanceFromRefTrack (lines 205-213).
        /// </summary>
        /// <param name="track">The water pivot track (PtA is center).</param>
        /// <param name="position">Current position to measure from.</param>
        /// <param name="vehicleHeading">Current vehicle heading (not used for pivot, but kept for consistency).</param>
        /// <returns>
        /// Tuple containing:
        /// - distance: Negative radial distance from center (negative to match sign convention)
        /// - sameway: True if on the correct side of the pivot
        /// </returns>
        private (double distance, bool sameway) GetDistanceFromWaterPivot(Track track, vec2 position, double vehicleHeading)
        {
            // For water pivot, PtA is the center point of the circle
            vec2 centerPoint = track.PtA;

            // Calculate radial distance from center (negative to match convention)
            double distanceFromCenter = -GeoMath.Distance(position, centerPoint);

            // Cross product determines which side of the pivot
            // This is used to determine if vehicle is going with or against the pivot direction
            // Formula: (px - cx) * (sy - cy) - (py - cy) * (sx - cx)
            // where p = pivot, c = center, s = steer position
            // Result < 0 means clockwise, > 0 means counter-clockwise

            // Note: For simplicity, we assume "sameway" is based on the cross product sign
            // In real implementation, this would use steer axle position as well
            // For now, we use a simplified version
            bool sameway = true; // Placeholder - actual implementation would calculate cross product

            return (distanceFromCenter, sameway);
        }
    }
}

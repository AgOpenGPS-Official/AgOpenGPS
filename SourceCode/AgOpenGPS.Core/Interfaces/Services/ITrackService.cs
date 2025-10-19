using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;

namespace AgOpenGPS.Core.Interfaces.Services
{
    /// <summary>
    /// Service interface for track management and guidance track operations.
    /// Provides business logic for track creation, modification, and geometry operations.
    /// </summary>
    public interface ITrackService
    {
        // ============================================================
        // Track Management
        // ============================================================

        /// <summary>
        /// Gets the collection of all tracks.
        /// </summary>
        TrackCollection Tracks { get; }

        /// <summary>
        /// Gets the currently active track for guidance.
        /// </summary>
        Track GetCurrentTrack();

        /// <summary>
        /// Sets the currently active track for guidance.
        /// </summary>
        /// <param name="track">The track to set as current, or null to clear.</param>
        void SetCurrentTrack(Track track);

        /// <summary>
        /// Adds a track to the collection.
        /// </summary>
        /// <param name="track">The track to add.</param>
        void AddTrack(Track track);

        /// <summary>
        /// Removes a track from the collection.
        /// </summary>
        /// <param name="track">The track to remove.</param>
        void RemoveTrack(Track track);

        /// <summary>
        /// Clears all tracks from the collection.
        /// </summary>
        void ClearTracks();

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
        Track CreateABTrack(vec2 ptA, vec2 ptB, double heading, string name = "AB Line");

        /// <summary>
        /// Creates a new curve track from a list of recorded points.
        /// </summary>
        /// <param name="recordedPath">The recorded path points with headings.</param>
        /// <param name="name">Optional track name.</param>
        /// <returns>The created curve track.</returns>
        Track CreateCurveTrack(List<vec3> recordedPath, string name = "Curve");

        /// <summary>
        /// Creates a boundary track from a boundary polygon.
        /// </summary>
        /// <param name="boundaryPoints">The boundary polygon points.</param>
        /// <param name="isOuter">True for outer boundary, false for inner.</param>
        /// <param name="name">Optional track name.</param>
        /// <returns>The created boundary track.</returns>
        Track CreateBoundaryTrack(List<vec3> boundaryPoints, bool isOuter, string name = "Boundary");

        // ============================================================
        // Track Operations
        // ============================================================

        /// <summary>
        /// Nudges a track by a specified distance perpendicular to its path.
        /// </summary>
        /// <param name="track">The track to nudge.</param>
        /// <param name="distance">Distance to nudge (positive = right, negative = left).</param>
        void NudgeTrack(Track track, double distance);

        /// <summary>
        /// Resets the nudge distance of a track to zero.
        /// </summary>
        /// <param name="track">The track to reset.</param>
        void ResetNudge(Track track);

        /// <summary>
        /// Snaps a track to the nearest pivot point for water pivot mode.
        /// </summary>
        /// <param name="track">The track to snap.</param>
        /// <param name="currentDistance">Current distance from track center.</param>
        void SnapToPivot(Track track, double currentDistance);

        // ============================================================
        // Geometry Operations (PERFORMANCE CRITICAL)
        // ============================================================

        /// <summary>
        /// Builds a guidance track at a specified offset distance from the reference track.
        /// PERFORMANCE TARGET: &lt;5ms for 500-point curve
        /// </summary>
        /// <param name="track">The reference track.</param>
        /// <param name="offsetDistance">Distance to offset (meters). Positive = right, negative = left.</param>
        /// <returns>The guidance track points with calculated headings.</returns>
        List<vec3> BuildGuidanceTrack(Track track, double offsetDistance);

        /// <summary>
        /// Builds multiple parallel guidance lines for visualization.
        /// </summary>
        /// <param name="track">The reference track.</param>
        /// <param name="spacing">Spacing between lines (meters).</param>
        /// <param name="numLinesLeft">Number of lines to the left.</param>
        /// <param name="numLinesRight">Number of lines to the right.</param>
        /// <returns>List of guidance line paths.</returns>
        List<List<vec3>> BuildGuideLines(Track track, double spacing, int numLinesLeft, int numLinesRight);

        /// <summary>
        /// Calculates the perpendicular distance from a position to the track.
        /// PERFORMANCE TARGET: &lt;0.5ms
        /// </summary>
        /// <param name="track">The track to measure distance from.</param>
        /// <param name="position">The position to measure (field coordinates).</param>
        /// <param name="heading">Current heading in radians.</param>
        /// <returns>
        /// Tuple containing:
        /// - distance: Signed perpendicular distance (positive = right, negative = left)
        /// - sameway: True if heading is in same direction as track
        /// </returns>
        (double distance, bool sameway) GetDistanceFromTrack(Track track, vec2 position, double heading);

        // ============================================================
        // Track Queries
        // ============================================================

        /// <summary>
        /// Finds a track by its unique identifier.
        /// </summary>
        /// <param name="id">The track ID to find.</param>
        /// <returns>The track if found, null otherwise.</returns>
        Track FindTrackById(System.Guid id);

        /// <summary>
        /// Finds a track by its name (case-insensitive).
        /// </summary>
        /// <param name="name">The track name to find.</param>
        /// <returns>The track if found, null otherwise.</returns>
        Track FindTrackByName(string name);

        /// <summary>
        /// Gets all tracks of a specific mode.
        /// </summary>
        /// <param name="mode">The track mode to filter by.</param>
        /// <returns>List of tracks matching the mode.</returns>
        List<Track> GetTracksByMode(TrackMode mode);

        /// <summary>
        /// Gets the count of visible tracks.
        /// </summary>
        /// <returns>Number of visible tracks.</returns>
        int GetVisibleTrackCount();

        // ============================================================
        // List Access Helpers (for UI compatibility during migration)
        // ============================================================

        /// <summary>
        /// Gets all tracks as a read-only list.
        /// </summary>
        /// <returns>Read-only list of all tracks.</returns>
        System.Collections.Generic.IReadOnlyList<Track> GetAllTracks();

        /// <summary>
        /// Gets a track at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <returns>The track at the index, or null if index is out of range.</returns>
        Track GetTrackAt(int index);

        /// <summary>
        /// Gets the total number of tracks.
        /// </summary>
        /// <returns>The track count.</returns>
        int GetTrackCount();

        /// <summary>
        /// Gets the index of the currently selected track.
        /// </summary>
        /// <returns>The index of the current track, or -1 if no track is selected.</returns>
        int GetCurrentTrackIndex();

        /// <summary>
        /// Sets the current track by index.
        /// </summary>
        /// <param name="index">The zero-based index, or -1 to clear selection.</param>
        void SetCurrentTrackIndex(int index);

        /// <summary>
        /// Removes a track at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the track to remove.</param>
        void RemoveTrackAt(int index);
    }
}

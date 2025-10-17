using System.Collections.Generic;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Interfaces.Services
{
    /// <summary>
    /// Service for creating and managing YouTurn (end-of-row turn) paths.
    /// Based on CYouTurn from AOG_Dev with clean architecture improvements.
    ///
    /// PERFORMANCE: CreateYouTurn() target <50ms (acceptable - not per-frame operation)
    /// YouTurn creation happens rarely (end of row), so heavy calculations are acceptable.
    /// </summary>
    public interface IYouTurnService
    {
        // ============================================================
        // YouTurn State
        // ============================================================

        /// <summary>
        /// Current YouTurn state.
        /// </summary>
        YouTurnState State { get; }

        /// <summary>
        /// Whether a YouTurn is currently active/triggered.
        /// </summary>
        bool IsActive { get; }

        // ============================================================
        // YouTurn Creation
        // ============================================================

        /// <summary>
        /// Creates a YouTurn path at the end of the current row.
        /// This is the main YouTurn creation method.
        ///
        /// PERFORMANCE: Target <50ms (not per-frame, triggered rarely)
        /// </summary>
        /// <param name="currentPosition">Current vehicle position</param>
        /// <param name="currentHeading">Current vehicle heading (radians)</param>
        /// <param name="currentTrackPoints">Current guidance track curve points</param>
        /// <param name="turnDiameter">YouTurn diameter (meters)</param>
        /// <param name="isTurnRight">True for right turn, false for left</param>
        /// <returns>True if YouTurn was successfully created</returns>
        bool CreateYouTurn(
            vec2 currentPosition,
            double currentHeading,
            List<vec3> currentTrackPoints,
            double turnDiameter,
            bool isTurnRight);

        /// <summary>
        /// Builds a manual YouTurn (triggered by user, not automatic).
        /// Simpler than automatic YouTurn - just creates semicircle.
        /// </summary>
        /// <param name="position">Starting position</param>
        /// <param name="heading">Starting heading</param>
        /// <param name="turnDiameter">Turn diameter</param>
        /// <param name="isTurnRight">True for right turn</param>
        void BuildManualYouTurn(
            vec2 position,
            double heading,
            double turnDiameter,
            bool isTurnRight);

        // ============================================================
        // YouTurn State Management
        // ============================================================

        /// <summary>
        /// Triggers/activates the YouTurn (starts following the turn path).
        /// </summary>
        void TriggerYouTurn();

        /// <summary>
        /// Completes the YouTurn (finished following the path).
        /// </summary>
        void CompleteYouTurn();

        /// <summary>
        /// Resets/cancels the YouTurn state.
        /// </summary>
        void ResetYouTurn();

        /// <summary>
        /// Checks if the vehicle has completed the YouTurn path.
        /// </summary>
        /// <param name="currentPosition">Current vehicle position</param>
        /// <returns>True if YouTurn is complete</returns>
        bool IsYouTurnComplete(vec2 currentPosition);

        // ============================================================
        // Utility Methods
        // ============================================================

        /// <summary>
        /// Calculates the distance remaining in the YouTurn path.
        /// </summary>
        /// <param name="currentPosition">Current position</param>
        /// <returns>Distance in meters</returns>
        double GetDistanceRemaining(vec2 currentPosition);
    }

    /// <summary>
    /// State of the YouTurn system.
    /// Contains the turn path and metadata.
    /// </summary>
    public class YouTurnState
    {
        /// <summary>
        /// Whether YouTurn is triggered/active.
        /// </summary>
        public bool IsTriggered { get; set; }

        /// <summary>
        /// Whether the YouTurn went out of field bounds.
        /// </summary>
        public bool IsOutOfBounds { get; set; }

        /// <summary>
        /// The YouTurn path to follow (semicircle or Dubins path).
        /// Pre-allocated for performance.
        /// </summary>
        public List<vec3> TurnPath { get; set; }

        /// <summary>
        /// The next guidance track after completing the turn.
        /// </summary>
        public List<vec3> NextTrackPath { get; set; }

        /// <summary>
        /// Total length of the turn path (meters).
        /// </summary>
        public double TotalLength { get; set; }

        /// <summary>
        /// Turn diameter used (meters).
        /// </summary>
        public double TurnDiameter { get; set; }

        /// <summary>
        /// Whether this was a right turn (vs left turn).
        /// </summary>
        public bool WasRightTurn { get; set; }

        /// <summary>
        /// Index of current segment being followed in TurnPath.
        /// </summary>
        public int CurrentSegmentIndex { get; set; }

        /// <summary>
        /// Creates a new YouTurnState with pre-allocated capacity.
        /// </summary>
        public YouTurnState()
        {
            TurnPath = new List<vec3>(capacity: 200);      // Pre-allocate for typical turn
            NextTrackPath = new List<vec3>(capacity: 200);
            IsTriggered = false;
            IsOutOfBounds = false;
            TotalLength = 0;
            TurnDiameter = 0;
            CurrentSegmentIndex = 0;
        }

        /// <summary>
        /// Resets the state for a new turn.
        /// </summary>
        public void Reset()
        {
            IsTriggered = false;
            IsOutOfBounds = false;
            TurnPath.Clear();
            NextTrackPath.Clear();
            TotalLength = 0;
            TurnDiameter = 0;
            CurrentSegmentIndex = 0;
        }

        /// <summary>
        /// Calculates the total path length.
        /// </summary>
        public void CalculateTotalLength()
        {
            TotalLength = 0;
            for (int i = 0; i < TurnPath.Count - 1; i++)
            {
                double dx = TurnPath[i + 1].easting - TurnPath[i].easting;
                double dy = TurnPath[i + 1].northing - TurnPath[i].northing;
                TotalLength += System.Math.Sqrt(dx * dx + dy * dy);
            }
        }
    }
}

using System;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Interfaces.Services
{
    /// <summary>
    /// Service for calculating guidance steering based on vehicle position and active track.
    /// Based on CGuidance from AOG_Dev with zero-allocation optimizations.
    ///
    /// PERFORMANCE CRITICAL: This runs 10-100 times per second!
    /// Target: <1ms per calculation with ZERO allocations.
    /// </summary>
    public interface IGuidanceService
    {
        // ============================================================
        // Configuration Properties
        // ============================================================

        /// <summary>
        /// Current guidance algorithm mode.
        /// </summary>
        GuidanceAlgorithm Algorithm { get; set; }

        /// <summary>
        /// Lookahead distance for Pure Pursuit algorithm (meters).
        /// Default: 3.0m, Range: 1.0-10.0m
        /// </summary>
        double LookaheadDistance { get; set; }

        /// <summary>
        /// Vehicle wheelbase (distance between front and rear axle) in meters.
        /// Used in Pure Pursuit formula for steering calculation.
        /// Default: 2.5m (typical tractor)
        /// </summary>
        double Wheelbase { get; set; }

        /// <summary>
        /// Stanley gain parameter for heading control.
        /// Default: 1.0, Range: 0.1-5.0
        /// Higher = more aggressive heading correction.
        /// </summary>
        double StanleyGain { get; set; }

        /// <summary>
        /// Stanley heading error weight (0-1).
        /// Default: 0.5, Range: 0.0-1.0
        /// 1.0 = pure heading control, 0.0 = pure cross-track control
        /// </summary>
        double StanleyHeadingErrorGain { get; set; }

        /// <summary>
        /// Maximum allowed steering angle (radians).
        /// Default: Math.PI/4 (45 degrees)
        /// </summary>
        double MaxSteerAngle { get; set; }

        // ============================================================
        // Main Guidance Calculation (PERFORMANCE CRITICAL!)
        // ============================================================

        /// <summary>
        /// Calculates guidance steering for the current vehicle state.
        ///
        /// PERFORMANCE: Must complete in <1ms with ZERO allocations!
        /// This method is called 10-100 times per second in the guidance loop.
        /// </summary>
        /// <param name="currentPosition">Current vehicle position (easting, northing)</param>
        /// <param name="currentHeading">Current vehicle heading (radians, 0 = north, clockwise)</param>
        /// <param name="currentSpeed">Current vehicle speed (m/s)</param>
        /// <param name="trackCurvePoints">The active track's curve points (from TrackService.BuildGuidanceTrack)</param>
        /// <param name="isReverse">True if vehicle is driving in reverse</param>
        /// <param name="isHeadingSameWay">True if vehicle heading is same direction as track (not 180° reversed)</param>
        /// <returns>GuidanceResult struct with steering angle and diagnostic info</returns>
        GuidanceResult CalculateGuidance(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            System.Collections.Generic.List<vec3> trackCurvePoints,
            bool isReverse = false,
            bool isHeadingSameWay = true);

        // ============================================================
        // Algorithm-Specific Methods (for testing/debugging)
        // ============================================================

        /// <summary>
        /// Calculates Stanley algorithm steering.
        /// Stanley = heading error control + cross-track error control
        ///
        /// Formula: steer = -headingError - atan(k * crossTrackError / (velocity + epsilon))
        /// Where k = Stanley gain parameter
        /// </summary>
        GuidanceResult CalculateStanley(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            System.Collections.Generic.List<vec3> trackCurvePoints,
            bool isReverse,
            bool isHeadingSameWay = true);

        /// <summary>
        /// Calculates Pure Pursuit algorithm steering.
        /// Pure Pursuit = steer toward a goal point ahead on the path
        ///
        /// Formula: steer = atan(2 * wheelbase * sin(alpha) / lookahead)
        /// Where alpha = angle between heading and goal point
        /// </summary>
        GuidanceResult CalculatePurePursuit(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            System.Collections.Generic.List<vec3> trackCurvePoints,
            bool isReverse,
            bool isHeadingSameWay = true);

        // ============================================================
        // Utility Methods
        // ============================================================

        /// <summary>
        /// Finds the goal point on the track at specified lookahead distance.
        /// Used by Pure Pursuit algorithm.
        ///
        /// OPTIMIZATION: Uses DistanceSquared in loop to avoid sqrt calls.
        /// </summary>
        /// <param name="currentPosition">Current vehicle position</param>
        /// <param name="trackCurvePoints">Track curve points to search</param>
        /// <param name="lookaheadDistance">Distance ahead to find goal point</param>
        /// <param name="goalPoint">Output: The goal point on track</param>
        /// <param name="isHeadingSameWay">True if vehicle heading is same direction as track (not 180° reversed)</param>
        /// <param name="isReverse">True if vehicle is driving in reverse (lookahead behind vehicle)</param>
        /// <returns>True if goal point found, false otherwise</returns>
        bool FindGoalPoint(
            vec2 currentPosition,
            System.Collections.Generic.List<vec3> trackCurvePoints,
            double lookaheadDistance,
            out vec3 goalPoint,
            bool isHeadingSameWay = true,
            bool isReverse = false);

        /// <summary>
        /// Clamps steering angle to maximum allowed range.
        /// </summary>
        double ClampSteerAngle(double steerAngle);

        /// <summary>
        /// Normalizes an angle to [-PI, PI] range.
        /// </summary>
        double NormalizeAngle(double angle);
    }

    /// <summary>
    /// Result of a guidance calculation.
    /// STRUCT (not class) to avoid heap allocations in hot path!
    /// </summary>
    public struct GuidanceResult
    {
        /// <summary>
        /// Calculated steering angle (radians).
        /// Positive = turn right, Negative = turn left
        /// Range: [-MaxSteerAngle, +MaxSteerAngle]
        /// </summary>
        public double SteerAngleRad { get; set; }

        /// <summary>
        /// Cross-track error distance (meters).
        /// Positive = right of line, Negative = left of line
        /// </summary>
        public double CrossTrackError { get; set; }

        /// <summary>
        /// Heading error (radians).
        /// Difference between vehicle heading and desired track heading.
        /// </summary>
        public double HeadingError { get; set; }

        /// <summary>
        /// Distance to goal point (meters).
        /// Only used in Pure Pursuit mode.
        /// </summary>
        public double DistanceToGoal { get; set; }

        /// <summary>
        /// The goal point on the track (Pure Pursuit mode).
        /// </summary>
        public vec3 GoalPoint { get; set; }

        /// <summary>
        /// Whether the calculation succeeded.
        /// False if track is invalid or other error occurred.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// The algorithm used for this calculation.
        /// </summary>
        public GuidanceAlgorithm AlgorithmUsed { get; set; }

        /// <summary>
        /// Index of closest curve point on track.
        /// Used for diagnostics and visualization.
        /// </summary>
        public int ClosestPointIndex { get; set; }

        /// <summary>
        /// Creates a default invalid result.
        /// </summary>
        public static GuidanceResult Invalid()
        {
            return new GuidanceResult
            {
                IsValid = false,
                SteerAngleRad = 0,
                CrossTrackError = 0,
                HeadingError = 0,
                DistanceToGoal = 0,
                GoalPoint = new vec3(),
                AlgorithmUsed = GuidanceAlgorithm.Stanley,
                ClosestPointIndex = -1
            };
        }

        /// <summary>
        /// Creates a valid result with calculated values.
        /// </summary>
        public static GuidanceResult Create(
            double steerAngle,
            double crossTrackError,
            double headingError,
            GuidanceAlgorithm algorithm,
            int closestPointIndex)
        {
            return new GuidanceResult
            {
                IsValid = true,
                SteerAngleRad = steerAngle,
                CrossTrackError = crossTrackError,
                HeadingError = headingError,
                AlgorithmUsed = algorithm,
                ClosestPointIndex = closestPointIndex,
                DistanceToGoal = 0,
                GoalPoint = new vec3()
            };
        }

        /// <summary>
        /// Creates a valid Pure Pursuit result with goal point info.
        /// </summary>
        public static GuidanceResult CreatePurePursuit(
            double steerAngle,
            double crossTrackError,
            double headingError,
            vec3 goalPoint,
            double distanceToGoal,
            int closestPointIndex)
        {
            return new GuidanceResult
            {
                IsValid = true,
                SteerAngleRad = steerAngle,
                CrossTrackError = crossTrackError,
                HeadingError = headingError,
                GoalPoint = goalPoint,
                DistanceToGoal = distanceToGoal,
                AlgorithmUsed = GuidanceAlgorithm.PurePursuit,
                ClosestPointIndex = closestPointIndex
            };
        }

        public override string ToString()
        {
            if (!IsValid) return "GuidanceResult: Invalid";

            double steerDeg = SteerAngleRad * 180.0 / Math.PI;
            return $"GuidanceResult: {AlgorithmUsed}, Steer={steerDeg:F1}°, XTE={CrossTrackError:F2}m, HeadingErr={HeadingError * 180.0 / Math.PI:F1}°";
        }
    }

    /// <summary>
    /// Available guidance algorithms.
    /// </summary>
    public enum GuidanceAlgorithm
    {
        /// <summary>
        /// Stanley algorithm: heading error + cross-track error control.
        /// Best for: Straight lines, high-speed operation, quick corrections.
        /// </summary>
        Stanley = 0,

        /// <summary>
        /// Pure Pursuit algorithm: goal point based steering.
        /// Best for: Curves, smoother path following, lower speed.
        /// </summary>
        PurePursuit = 1
    }
}

using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Geometry;

namespace AgOpenGPS.Core.Services
{
    /// <summary>
    /// Implementation of IGuidanceService for calculating steering guidance.
    /// Based on CGuidance from AOG_Dev with performance optimizations.
    ///
    /// PERFORMANCE CRITICAL: Zero allocations in CalculateGuidance!
    /// Target: <1ms per calculation (called 10-100x per second)
    /// </summary>
    public class GuidanceService : IGuidanceService
    {
        // ============================================================
        // Configuration Properties
        // ============================================================

        public GuidanceAlgorithm Algorithm { get; set; }
        public double LookaheadDistance { get; set; }
        public double StanleyGain { get; set; }
        public double StanleyHeadingErrorGain { get; set; }
        public double MaxSteerAngle { get; set; }

        // ============================================================
        // Constants
        // ============================================================

        private const double DefaultLookahead = 3.0;        // 3 meters
        private const double DefaultStanleyGain = 1.0;
        private const double DefaultHeadingGain = 0.5;      // 50% heading, 50% cross-track
        private const double DefaultMaxSteerAngle = Math.PI / 4; // 45 degrees
        private const double MinSpeed = 0.1;                // Minimum speed to avoid division by zero
        private const double EpsilonSpeed = 0.01;           // Small epsilon for Stanley formula

        // ============================================================
        // Constructor
        // ============================================================

        public GuidanceService()
        {
            Algorithm = GuidanceAlgorithm.Stanley;
            LookaheadDistance = DefaultLookahead;
            StanleyGain = DefaultStanleyGain;
            StanleyHeadingErrorGain = DefaultHeadingGain;
            MaxSteerAngle = DefaultMaxSteerAngle;
        }

        // ============================================================
        // Main Guidance Calculation (PERFORMANCE CRITICAL!)
        // ============================================================

        public GuidanceResult CalculateGuidance(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            List<vec3> trackCurvePoints,
            bool isReverse = false)
        {
            // Dispatch to appropriate algorithm
            if (Algorithm == GuidanceAlgorithm.PurePursuit)
            {
                return CalculatePurePursuit(currentPosition, currentHeading, currentSpeed, trackCurvePoints, isReverse);
            }
            else
            {
                return CalculateStanley(currentPosition, currentHeading, currentSpeed, trackCurvePoints, isReverse);
            }
        }

        // ============================================================
        // Stanley Algorithm Implementation
        // ============================================================

        public GuidanceResult CalculateStanley(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            List<vec3> trackCurvePoints,
            bool isReverse)
        {
            // Validate inputs
            if (trackCurvePoints == null || trackCurvePoints.Count < 2)
            {
                return GuidanceResult.Invalid();
            }

            // Find closest segment on track (optimized two-phase search)
            if (!GeometryUtils.FindClosestSegment(trackCurvePoints, currentPosition, out int rA, out int rB, false))
            {
                return GuidanceResult.Invalid();
            }

            // Calculate signed cross-track error
            double crossTrackError = GeometryUtils.FindDistanceToSegment(
                currentPosition,
                trackCurvePoints[rA],
                trackCurvePoints[rB],
                out vec3 closestPoint,
                out double time,
                signed: true);

            // Get desired heading from track
            double desiredHeading = closestPoint.heading;

            // Adjust for reverse driving
            if (isReverse)
            {
                currentHeading = NormalizeAngle(currentHeading + Math.PI);
            }

            // Calculate heading error (normalized to [-PI, PI])
            double headingError = NormalizeAngle(desiredHeading - currentHeading);

            // Stanley formula:
            // steer = -headingError - atan(k * crossTrackError / (velocity + epsilon))
            //
            // Component 1: Heading error (align vehicle with track heading)
            // Component 2: Cross-track error (steer toward track)

            double speed = Math.Max(Math.Abs(currentSpeed), MinSpeed);

            // Cross-track error component
            double crossTrackComponent = Math.Atan(StanleyGain * crossTrackError / (speed + EpsilonSpeed));

            // Combined steering (weighted)
            double steerAngle = -(StanleyHeadingErrorGain * headingError + (1.0 - StanleyHeadingErrorGain) * crossTrackComponent);

            // Clamp to max steer angle
            steerAngle = ClampSteerAngle(steerAngle);

            // Adjust sign for reverse
            if (isReverse)
            {
                steerAngle = -steerAngle;
            }

            return GuidanceResult.Create(
                steerAngle,
                crossTrackError,
                headingError,
                GuidanceAlgorithm.Stanley,
                rA);
        }

        // ============================================================
        // Pure Pursuit Algorithm Implementation
        // ============================================================

        public GuidanceResult CalculatePurePursuit(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            List<vec3> trackCurvePoints,
            bool isReverse)
        {
            // Validate inputs
            if (trackCurvePoints == null || trackCurvePoints.Count < 2)
            {
                return GuidanceResult.Invalid();
            }

            // Find closest point for cross-track error
            if (!GeometryUtils.FindClosestSegment(trackCurvePoints, currentPosition, out int rA, out int rB, false))
            {
                return GuidanceResult.Invalid();
            }

            double crossTrackError = GeometryUtils.FindDistanceToSegment(
                currentPosition,
                trackCurvePoints[rA],
                trackCurvePoints[rB],
                out vec3 closestPoint,
                out double time,
                signed: true);

            // Find goal point at lookahead distance
            if (!FindGoalPoint(currentPosition, trackCurvePoints, LookaheadDistance, out vec3 goalPoint))
            {
                // Fallback to closest point if goal not found
                goalPoint = closestPoint;
            }

            // Calculate distance to goal point
            double dx = goalPoint.easting - currentPosition.easting;
            double dy = goalPoint.northing - currentPosition.northing;
            double distanceToGoal = Math.Sqrt(dx * dx + dy * dy);

            // Adjust heading for reverse
            double vehicleHeading = isReverse ? NormalizeAngle(currentHeading + Math.PI) : currentHeading;

            // Calculate angle to goal point (alpha)
            // AgOpenGPS heading convention: Atan2(dx, dy) where 0 = North, clockwise
            double angleToGoal = Math.Atan2(dx, dy);
            double alpha = NormalizeAngle(angleToGoal - vehicleHeading);

            // Pure Pursuit formula:
            // steer = atan(2 * L * sin(alpha) / lookahead)
            // where L = wheelbase (we use lookahead as proxy)
            //
            // Simplified (for our case, assume L ≈ lookahead/2):
            // steer = atan(sin(alpha) / lookahead) * lookahead
            // steer ≈ alpha for small angles

            double steerAngle = Math.Atan(2.0 * LookaheadDistance * Math.Sin(alpha) / (distanceToGoal + EpsilonSpeed));

            // Clamp to max steer angle
            steerAngle = ClampSteerAngle(steerAngle);

            // Adjust sign for reverse
            if (isReverse)
            {
                steerAngle = -steerAngle;
            }

            // Calculate heading error for diagnostics
            double headingError = NormalizeAngle(goalPoint.heading - currentHeading);

            return GuidanceResult.CreatePurePursuit(
                steerAngle,
                crossTrackError,
                headingError,
                goalPoint,
                distanceToGoal,
                rA);
        }

        // ============================================================
        // Utility Methods
        // ============================================================

        public bool FindGoalPoint(
            vec2 currentPosition,
            List<vec3> trackCurvePoints,
            double lookaheadDistance,
            out vec3 goalPoint)
        {
            goalPoint = new vec3();

            if (trackCurvePoints == null || trackCurvePoints.Count < 2)
                return false;

            // OPTIMIZATION: Use DistanceSquared to avoid sqrt in loop
            double lookaheadSq = lookaheadDistance * lookaheadDistance;

            // First, find closest point as starting reference
            if (!GeometryUtils.FindClosestSegment(trackCurvePoints, currentPosition, out int closestIdx, out int _, false))
            {
                return false;
            }

            // Search forward from closest point for goal point at lookahead distance
            // Strategy: Find first point >= lookahead distance ahead

            int count = trackCurvePoints.Count;
            int searchRange = Math.Min(50, count); // Search up to 50 points ahead

            for (int i = 0; i < searchRange; i++)
            {
                int idx = (closestIdx + i) % count; // Wrap around for closed loops
                vec3 pt = trackCurvePoints[idx];

                double dx = pt.easting - currentPosition.easting;
                double dy = pt.northing - currentPosition.northing;
                double distSq = dx * dx + dy * dy;

                // Found point at or beyond lookahead distance
                if (distSq >= lookaheadSq)
                {
                    // Interpolate between previous and current point for exact lookahead distance
                    if (i > 0)
                    {
                        int prevIdx = (closestIdx + i - 1) % count;
                        vec3 prevPt = trackCurvePoints[prevIdx];

                        // Distance to previous point
                        double dx0 = prevPt.easting - currentPosition.easting;
                        double dy0 = prevPt.northing - currentPosition.northing;
                        double dist0Sq = dx0 * dx0 + dy0 * dy0;

                        // Interpolate between prevPt and pt
                        double dist0 = Math.Sqrt(dist0Sq);
                        double dist1 = Math.Sqrt(distSq);

                        if (dist1 - dist0 > 0.01) // Avoid division by zero
                        {
                            double t = (lookaheadDistance - dist0) / (dist1 - dist0);
                            t = Math.Max(0, Math.Min(1, t)); // Clamp to [0,1]

                            goalPoint = new vec3(
                                prevPt.easting + t * (pt.easting - prevPt.easting),
                                prevPt.northing + t * (pt.northing - prevPt.northing),
                                prevPt.heading + t * (pt.heading - prevPt.heading)
                            );
                            return true;
                        }
                    }

                    // Use current point directly
                    goalPoint = pt;
                    return true;
                }
            }

            // Fallback: use furthest point searched
            int fallbackIdx = (closestIdx + searchRange - 1) % count;
            goalPoint = trackCurvePoints[fallbackIdx];
            return true;
        }

        public double ClampSteerAngle(double steerAngle)
        {
            if (steerAngle > MaxSteerAngle)
                return MaxSteerAngle;
            if (steerAngle < -MaxSteerAngle)
                return -MaxSteerAngle;
            return steerAngle;
        }

        public double NormalizeAngle(double angle)
        {
            // Normalize to [-PI, PI]
            while (angle > Math.PI)
                angle -= 2.0 * Math.PI;
            while (angle < -Math.PI)
                angle += 2.0 * Math.PI;
            return angle;
        }

        // ============================================================
        // Configuration Helpers
        // ============================================================

        /// <summary>
        /// Sets Stanley parameters with validation.
        /// </summary>
        public void ConfigureStanley(double gain, double headingErrorGain)
        {
            StanleyGain = Math.Max(0.1, Math.Min(5.0, gain));
            StanleyHeadingErrorGain = Math.Max(0.0, Math.Min(1.0, headingErrorGain));
        }

        /// <summary>
        /// Sets Pure Pursuit parameters with validation.
        /// </summary>
        public void ConfigurePurePursuit(double lookahead)
        {
            LookaheadDistance = Math.Max(1.0, Math.Min(10.0, lookahead));
        }

        /// <summary>
        /// Resets all parameters to defaults.
        /// </summary>
        public void ResetToDefaults()
        {
            Algorithm = GuidanceAlgorithm.Stanley;
            LookaheadDistance = DefaultLookahead;
            StanleyGain = DefaultStanleyGain;
            StanleyHeadingErrorGain = DefaultHeadingGain;
            MaxSteerAngle = DefaultMaxSteerAngle;
        }
    }
}

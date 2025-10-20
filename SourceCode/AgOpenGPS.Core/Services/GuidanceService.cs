using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Geometry;

namespace AgOpenGPS.Core.Services
{
    /// <summary>
    /// Service for calculating guidance steering based on vehicle position and active track.
    /// Implements both Stanley and Pure Pursuit algorithms.
    ///
    /// PERFORMANCE CRITICAL: Runs 10-100 times per second!
    /// Target: <1ms per calculation with ZERO allocations.
    /// </summary>
    public class GuidanceService : IGuidanceService
    {
        // ============================================================
        // Configuration Properties
        // ============================================================

        public GuidanceAlgorithm Algorithm { get; set; } = GuidanceAlgorithm.PurePursuit;

        // Stanley parameters
        public double StanleyGain { get; set; } = 2.5; // XTE gain - higher = more aggressive XTE correction
        public double StanleyHeadingErrorGain { get; set; } = 0.3; // Heading weight (0-1) - lower = more XTE focus
        public double StanleyIntegralGain { get; set; } = 0.0;

        // Pure Pursuit parameters
        public double LookaheadDistance { get; set; } = 2.0;
        public double Wheelbase { get; set; } = 2.5; // Vehicle wheelbase in meters (default tractor)
        public double PurePursuitDampingGain { get; set; } = 0.3; // XTE damping to prevent overshoot (0-1, default 0.3)

        // Common parameters
        public double MaxSteerAngle { get; set; } = 0.785398; // 45 degrees in radians
        public double EpsilonSpeed { get; set; } = 0.1; // Minimum speed for division
        public double MinSpeed { get; set; } = 0.25; // Minimum speed threshold

        // ============================================================
        // Main Calculation Method
        // ============================================================

        /// <summary>
        /// Calculate guidance steering based on current position and track.
        /// Dispatches to Stanley or Pure Pursuit based on Algorithm setting.
        /// </summary>
        public GuidanceResult CalculateGuidance(
            vec2 currentPosition,
            double currentHeading,
            double currentSpeed,
            List<vec3> trackCurvePoints,
            bool isReverse,
            bool isHeadingSameWay = true)
        {
            if (trackCurvePoints == null || trackCurvePoints.Count < 2)
            {
                return GuidanceResult.Invalid();
            }

            // Dispatch to appropriate algorithm
            if (Algorithm == GuidanceAlgorithm.PurePursuit)
            {
                return CalculatePurePursuit(currentPosition, currentHeading, currentSpeed, trackCurvePoints, isReverse, isHeadingSameWay);
            }
            else
            {
                return CalculateStanley(currentPosition, currentHeading, currentSpeed, trackCurvePoints, isReverse, isHeadingSameWay);
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
            bool isReverse,
            bool isHeadingSameWay = true)
        {
            // Validate inputs
            if (trackCurvePoints == null || trackCurvePoints.Count < 2)
            {
                return GuidanceResult.Invalid();
            }

            // Find closest segment
            if (!GeometryUtils.FindClosestSegment(trackCurvePoints, currentPosition, out int rA, out int rB, false))
            {
                return GuidanceResult.Invalid();
            }

            // Calculate cross-track error (signed distance to track)
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
            bool isReverse,
            bool isHeadingSameWay = true)
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

            // Phase 6.9 FIX: Find goal point at lookahead distance, respecting heading direction AND reverse
            // isHeadingSameWay: if false, walk backwards along track (180° reversed heading)
            // isReverse: if true, lookahead is BEHIND vehicle instead of in front
            if (!FindGoalPoint(currentPosition, trackCurvePoints, LookaheadDistance, out vec3 goalPoint, isHeadingSameWay, isReverse))
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
            // where L = wheelbase (distance between front and rear axle)
            //
            // Phase 6.9 FIX: Use actual Wheelbase instead of LookaheadDistance
            // The wheelbase determines how aggressively the vehicle can turn
            // Longer wheelbase = less aggressive steering response

            // Phase 6.9 FIX: Use AOG_Dev Pure Pursuit formula (CABLine.cs:270-275)
            // This is NOT the classical Pure Pursuit formula!
            //
            // AOG_Dev uses a simple local coordinate system transformation:
            // localHeading = 2π - vehicleHeading
            // (plus a small integral term 'inty' which we ignore for now)
            //
            // This works for BOTH curves and AB lines - it's unified!

            // Calculate local heading - simple transformation
            double localHeading = (2.0 * Math.PI) - vehicleHeading;

            // Vector from current position to goal point
            double deltaEast = goalPoint.easting - currentPosition.easting;
            double deltaNorth = goalPoint.northing - currentPosition.northing;

            // AOG_Dev dot product: NOTE cos/sin usage!
            // (CABLine.cs:273-274)
            double dotProduct = (deltaEast * Math.Cos(localHeading)) + (deltaNorth * Math.Sin(localHeading));

            // Distance squared to goal point
            double distanceSquared = (deltaEast * deltaEast) + (deltaNorth * deltaNorth);

            // Prevent division by zero
            if (distanceSquared < 0.01) distanceSquared = 0.01; // Min 10cm distance

            // AOG_Dev Pure Pursuit formula (CABLine.cs:273-275)
            double steerAngle = Math.Atan(2.0 * dotProduct * Wheelbase / distanceSquared);

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
            out vec3 goalPoint,
            bool isHeadingSameWay = true,
            bool isReverse = false)
        {
            goalPoint = new vec3();

            if (trackCurvePoints == null || trackCurvePoints.Count < 2)
                return false;

            // Phase 6.9 FIX: Follow AOG_Dev logic - find closest point FIRST, then walk along track
            // This ensures smooth lookahead movement along the track (CABCurve.cs lines 792-824)

            // Find closest segment
            if (!GeometryUtils.FindClosestSegment(trackCurvePoints, currentPosition, out int rA, out int rB, false))
            {
                return false;
            }

            int count = trackCurvePoints.Count;

            // Calculate closest point on the segment (like AOG_Dev rEastCu/rNorthCu)
            vec3 ptA = trackCurvePoints[rA];
            vec3 ptB = trackCurvePoints[rB];

            double dx = ptB.easting - ptA.easting;
            double dy = ptB.northing - ptA.northing;
            double segmentLengthSq = dx * dx + dy * dy;

            vec3 closestPoint;
            if (segmentLengthSq < 0.0001)
            {
                closestPoint = ptA;
            }
            else
            {
                // Project currentPosition onto segment
                double U = ((currentPosition.easting - ptA.easting) * dx +
                           (currentPosition.northing - ptA.northing) * dy) / segmentLengthSq;
                U = Math.Max(0, Math.Min(1, U)); // Clamp to [0,1]

                closestPoint = new vec3(
                    ptA.easting + U * dx,
                    ptA.northing + U * dy,
                    ptA.heading + U * (ptB.heading - ptA.heading)
                );
            }

            // Phase 6.9: Detect if track is a loop
            vec3 firstPt = trackCurvePoints[0];
            vec3 lastPt = trackCurvePoints[count - 1];
            double dx_loop = lastPt.easting - firstPt.easting;
            double dy_loop = lastPt.northing - firstPt.northing;
            double distSq_loop = dx_loop * dx_loop + dy_loop * dy_loop;
            bool isLoop = distSq_loop < 4.0; // 2 meters

            // Phase 6.9 FIX: Walk along track from closest point, accumulating distance
            // UNIFIED APPROACH: same logic for curves and AB lines
            //
            // Direction combines TWO independent factors:
            // 1. isReverse: Physical reverse driving (lookahead BEHIND vehicle)
            // 2. isHeadingSameWay: Track direction (forward or 180° reversed on track)
            vec3 start = closestPoint;
            double distSoFar = 0;

            // Use lookahead distance as-is
            // The XOR logic below handles the direction (forward/backward on track)
            double effectiveLookahead = lookaheadDistance;

            // SIMPLE UNIFIED LOGIC (matches AOG_Dev XOR logic in CABLine.cs:249)
            // AOG_Dev: if (isReverse XOR isHeadingSameWay) → + direction, else → - direction
            //
            // Truth table:
            // isReverse=false, isHeadingSameWay=true  → XOR=false → walk backward (-1)
            // isReverse=false, isHeadingSameWay=false → XOR=true  → walk forward (+1)
            // isReverse=true,  isHeadingSameWay=true  → XOR=true  → walk forward (+1)
            // isReverse=true,  isHeadingSameWay=false → XOR=false → walk backward (-1)
            bool xorResult = isReverse ^ isHeadingSameWay;
            int direction = xorResult ? 1 : -1;

            // Start index based on direction
            int startIdx = (direction > 0) ? rB : rA;

            int searchRange = Math.Min(200, count); // Search up to 200 points

            for (int i = 0; i < searchRange; i++)
            {
                int idx;
                if (isLoop)
                {
                    idx = (startIdx + (direction * i) + count) % count;
                }
                else
                {
                    idx = startIdx + (direction * i);
                    if (idx >= count || idx < 0)
                    {
                        // Reached end - use boundary point
                        goalPoint = idx >= count ? trackCurvePoints[count - 1] : trackCurvePoints[0];
                        return true;
                    }
                }

                vec3 pt = trackCurvePoints[idx];

                // Distance from start to this point
                double tempDist = GeoMath.Distance(start, pt);

                // Will we go too far?
                if ((distSoFar + tempDist) > effectiveLookahead)
                {
                    // Interpolate for exact lookahead distance (using effective distance)
                    double j = (effectiveLookahead - distSoFar) / tempDist; // Remainder to travel
                    j = Math.Max(0, Math.Min(1, j));

                    goalPoint = new vec3(
                        ((1 - j) * start.easting) + (j * pt.easting),
                        ((1 - j) * start.northing) + (j * pt.northing),
                        ((1 - j) * start.heading) + (j * pt.heading)
                    );
                    return true;
                }

                distSoFar += tempDist;
                start = pt;
            }

            // Fallback: use furthest point reached
            goalPoint = start;
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
        /// Validates and clamps steering angle to safe range.
        /// </summary>
        public void SetMaxSteerAngle(double maxAngle)
        {
            MaxSteerAngle = Math.Max(0.1, Math.Min(Math.PI / 2, maxAngle));
        }
    }
}

using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Services
{
    /// <summary>
    /// Implementation of IYouTurnService for creating YouTurn (end-of-row turn) paths.
    /// Based on CYouTurn from AOG_Dev with clean architecture improvements.
    ///
    /// PERFORMANCE: CreateYouTurn() runs in <50ms (acceptable - not per-frame)
    /// </summary>
    public class YouTurnService : IYouTurnService
    {
        // ============================================================
        // State
        // ============================================================

        private readonly YouTurnState _state;

        public YouTurnState State => _state;
        public bool IsActive => _state.IsTriggered;

        // ============================================================
        // Constructor
        // ============================================================

        public YouTurnService()
        {
            _state = new YouTurnState();
        }

        // ============================================================
        // YouTurn Creation
        // ============================================================

        public bool CreateYouTurn(
            vec2 currentPosition,
            double currentHeading,
            List<vec3> currentTrackPoints,
            double turnDiameter,
            bool isTurnRight)
        {
            // Validate inputs
            if (currentTrackPoints == null || currentTrackPoints.Count < 2)
                return false;

            if (turnDiameter <= 0)
                return false;

            // Reset state for new turn
            _state.Reset();
            _state.TurnDiameter = turnDiameter;
            _state.WasRightTurn = isTurnRight;

            // Build the YouTurn path
            // Strategy: Create semicircle from current position
            BuildSemicircleTurn(currentPosition, currentHeading, turnDiameter, isTurnRight);

            // Calculate total length
            _state.CalculateTotalLength();

            return _state.TurnPath.Count > 0;
        }

        public void BuildManualYouTurn(
            vec2 position,
            double heading,
            double turnDiameter,
            bool isTurnRight)
        {
            // Manual turn is simpler - just a semicircle
            _state.Reset();
            _state.TurnDiameter = turnDiameter;
            _state.WasRightTurn = isTurnRight;

            BuildSemicircleTurn(position, heading, turnDiameter, isTurnRight);
            _state.CalculateTotalLength();
        }

        // ============================================================
        // YouTurn State Management
        // ============================================================

        public void TriggerYouTurn()
        {
            _state.IsTriggered = true;
            _state.CurrentSegmentIndex = 0;
        }

        public void CompleteYouTurn()
        {
            _state.IsTriggered = false;
            _state.CurrentSegmentIndex = 0;
        }

        public void ResetYouTurn()
        {
            _state.Reset();
        }

        public bool IsYouTurnComplete(vec2 currentPosition)
        {
            if (!_state.IsTriggered || _state.TurnPath.Count == 0)
                return false;

            // Check if we're near the end of the turn path
            vec3 endPoint = _state.TurnPath[_state.TurnPath.Count - 1];
            double dx = currentPosition.easting - endPoint.easting;
            double dy = currentPosition.northing - endPoint.northing;
            double distSq = dx * dx + dy * dy;

            // Complete if within 2 meters of end
            return distSq < (2.0 * 2.0);
        }

        // ============================================================
        // Utility Methods
        // ============================================================

        public double GetDistanceRemaining(vec2 currentPosition)
        {
            if (_state.TurnPath.Count == 0)
                return 0;

            // Find closest point on turn path
            double minDistSq = double.MaxValue;
            int closestIndex = 0;

            for (int i = 0; i < _state.TurnPath.Count; i++)
            {
                double dx = currentPosition.easting - _state.TurnPath[i].easting;
                double dy = currentPosition.northing - _state.TurnPath[i].northing;
                double distSq = dx * dx + dy * dy;

                if (distSq < minDistSq)
                {
                    minDistSq = distSq;
                    closestIndex = i;
                }
            }

            // Calculate remaining distance from closest point to end
            double remaining = 0;
            for (int i = closestIndex; i < _state.TurnPath.Count - 1; i++)
            {
                double dx = _state.TurnPath[i + 1].easting - _state.TurnPath[i].easting;
                double dy = _state.TurnPath[i + 1].northing - _state.TurnPath[i].northing;
                remaining += Math.Sqrt(dx * dx + dy * dy);
            }

            return remaining;
        }

        // ============================================================
        // Private Helper Methods
        // ============================================================

        /// <summary>
        /// Builds a semicircle turn path.
        /// This is the core YouTurn geometry.
        /// </summary>
        private void BuildSemicircleTurn(
            vec2 startPosition,
            double startHeading,
            double diameter,
            bool isTurnRight)
        {
            double radius = diameter / 2.0;

            // Calculate center of turn circle
            // Center is perpendicular to heading, at distance = radius
            double perpHeading = isTurnRight ?
                (startHeading - Math.PI / 2.0) :  // Right turn: -90°
                (startHeading + Math.PI / 2.0);   // Left turn: +90°

            vec2 center = new vec2(
                startPosition.easting + radius * Math.Sin(perpHeading),
                startPosition.northing + radius * Math.Cos(perpHeading)
            );

            // Build semicircle points (180 degrees)
            // Use adaptive point spacing: max 2cm deviation from perfect circle
            int numPoints = CalculateCirclePoints(radius);

            double startAngle = isTurnRight ?
                (startHeading + Math.PI / 2.0) :  // Start angle for right turn
                (startHeading - Math.PI / 2.0);   // Start angle for left turn

            double angleStep = Math.PI / (numPoints - 1);  // 180 degrees / (n-1) points

            _state.TurnPath.Clear();

            for (int i = 0; i < numPoints; i++)
            {
                double angle = startAngle + (isTurnRight ? angleStep : -angleStep) * i;

                vec3 point = new vec3(
                    center.easting + radius * Math.Sin(angle),
                    center.northing + radius * Math.Cos(angle),
                    angle + (isTurnRight ? -Math.PI / 2.0 : Math.PI / 2.0)  // Tangent heading
                );

                _state.TurnPath.Add(point);
            }
        }

        /// <summary>
        /// Calculates the number of points needed for a circle to maintain
        /// max 2cm deviation from perfect circle.
        /// </summary>
        /// <param name="radius">Circle radius in meters</param>
        /// <returns>Number of points (clamped to 100-1000)</returns>
        private int CalculateCirclePoints(double radius)
        {
            // For a circle, max deviation d from chord approximation:
            // d = r * (1 - cos(θ/2))
            // For d = 0.02m (2cm):
            // θ = 2 * acos(1 - d/r)

            const double maxDeviation = 0.02;  // 2cm
            double theta = 2.0 * Math.Acos(1.0 - (maxDeviation / radius));

            // For full circle: numPoints = 2π / θ
            // For semicircle: numPoints = π / θ
            int numPoints = (int)Math.Ceiling(Math.PI / theta);

            // Clamp to reasonable range
            return Math.Max(100, Math.Min(1000, numPoints));
        }

        /// <summary>
        /// Builds a more complex Dubins-style YouTurn with entry/exit paths.
        /// This is for more advanced YouTurn creation (future enhancement).
        /// </summary>
        private void BuildDubinsTurn(
            vec2 startPosition,
            double startHeading,
            vec2 targetPosition,
            double targetHeading,
            double turnRadius,
            bool isTurnRight)
        {
            // Dubins path: LSL, RSR, LSR, RSL
            // Implementation would go here for more complex turns
            // For now, we use simple semicircle (BuildSemicircleTurn)

            // TODO: Implement full Dubins path if needed
            // This would calculate optimal path from (start, startHeading)
            // to (target, targetHeading) with given turn radius
        }
    }
}

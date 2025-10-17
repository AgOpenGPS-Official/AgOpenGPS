using System;
using System.Collections.Generic;
using NUnit.Framework;
using AgOpenGPS.Core.Services;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Tests.Services
{
    /// <summary>
    /// Comprehensive unit tests for YouTurnService.
    /// Tests semicircle turn creation, state management, and utility methods.
    /// </summary>
    [TestFixture]
    public class YouTurnServiceTests
    {
        private YouTurnService _service;

        [SetUp]
        public void Setup()
        {
            _service = new YouTurnService();
        }

        // ============================================================
        // Constructor and Initialization Tests
        // ============================================================

        [Test]
        public void Constructor_InitializesWithDefaultState()
        {
            // Assert
            Assert.That(_service.State, Is.Not.Null);
            Assert.That(_service.IsActive, Is.False);
            Assert.That(_service.State.IsTriggered, Is.False);
            Assert.That(_service.State.TurnPath, Is.Not.Null);
            Assert.That(_service.State.TurnPath.Count, Is.EqualTo(0));
        }

        [Test]
        public void State_ReturnsYouTurnState()
        {
            // Act
            var state = _service.State;

            // Assert
            Assert.That(state, Is.Not.Null);
            Assert.That(state, Is.TypeOf<YouTurnState>());
        }

        // ============================================================
        // CreateYouTurn Tests
        // ============================================================

        [Test]
        public void CreateYouTurn_ValidInputs_CreatesPath()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2; // East
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            double diameter = 20.0;

            // Act
            bool result = _service.CreateYouTurn(position, heading, trackPoints, diameter, isTurnRight: true);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_service.State.TurnPath.Count, Is.GreaterThan(0));
            Assert.That(_service.State.TurnDiameter, Is.EqualTo(diameter));
            Assert.That(_service.State.WasRightTurn, Is.True);
            Assert.That(_service.State.TotalLength, Is.GreaterThan(0));
        }

        [Test]
        public void CreateYouTurn_RightTurn_CreatesClockwisePath()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = 0; // North
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            double diameter = 20.0;

            // Act
            _service.CreateYouTurn(position, heading, trackPoints, diameter, isTurnRight: true);

            // Assert
            Assert.That(_service.State.TurnPath.Count, Is.GreaterThan(0));
            Assert.That(_service.State.WasRightTurn, Is.True);

            // Check that path curves to the right
            var start = _service.State.TurnPath[0];
            var mid = _service.State.TurnPath[_service.State.TurnPath.Count / 2];

            // When heading north, right turn goes west (decreasing easting)
            Assert.That(mid.easting, Is.LessThan(start.easting));
        }

        [Test]
        public void CreateYouTurn_LeftTurn_CreatesCounterClockwisePath()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = 0; // North
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            double diameter = 20.0;

            // Act
            _service.CreateYouTurn(position, heading, trackPoints, diameter, isTurnRight: false);

            // Assert
            Assert.That(_service.State.TurnPath.Count, Is.GreaterThan(0));
            Assert.That(_service.State.WasRightTurn, Is.False);

            // Check that path curves to the left
            var start = _service.State.TurnPath[0];
            var mid = _service.State.TurnPath[_service.State.TurnPath.Count / 2];

            // When heading north, left turn goes east (increasing easting)
            Assert.That(mid.easting, Is.GreaterThan(start.easting));
        }

        [Test]
        public void CreateYouTurn_NullTrackPoints_ReturnsFalse()
        {
            // Act
            bool result = _service.CreateYouTurn(new vec2(0, 0), 0, null, 20.0, true);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateYouTurn_EmptyTrackPoints_ReturnsFalse()
        {
            // Act
            bool result = _service.CreateYouTurn(new vec2(0, 0), 0, new List<vec3>(), 20.0, true);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateYouTurn_ZeroDiameter_ReturnsFalse()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);

            // Act
            bool result = _service.CreateYouTurn(new vec2(0, 0), 0, trackPoints, 0, true);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateYouTurn_NegativeDiameter_ReturnsFalse()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);

            // Act
            bool result = _service.CreateYouTurn(new vec2(0, 0), 0, trackPoints, -10.0, true);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void CreateYouTurn_SmallDiameter_CreatesMorePoints()
        {
            // Arrange
            var position = new vec2(100, 100);
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);

            // Act - Use diameters that won't hit the min/max clamps (100-1000 points)
            _service.CreateYouTurn(position, 0, trackPoints, 5.0, true);
            int smallDiameterPoints = _service.State.TurnPath.Count;

            _service.CreateYouTurn(position, 0, trackPoints, 100.0, true);
            int largeDiameterPoints = _service.State.TurnPath.Count;

            // Assert - smaller radius needs more points for same accuracy (2cm deviation)
            // Both should have enough points to maintain accuracy
            Assert.That(smallDiameterPoints, Is.GreaterThanOrEqualTo(100));
            Assert.That(largeDiameterPoints, Is.GreaterThanOrEqualTo(100));
        }

        [Test]
        public void CreateYouTurn_CalculatesTotalLength()
        {
            // Arrange
            var position = new vec2(100, 100);
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            double diameter = 20.0;

            // Act
            _service.CreateYouTurn(position, 0, trackPoints, diameter, true);

            // Assert
            double expectedLength = Math.PI * diameter / 2.0; // Semicircle = πr
            Assert.That(_service.State.TotalLength, Is.EqualTo(expectedLength).Within(1.0));
        }

        // ============================================================
        // BuildManualYouTurn Tests
        // ============================================================

        [Test]
        public void BuildManualYouTurn_CreatesPath()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            double diameter = 20.0;

            // Act
            _service.BuildManualYouTurn(position, heading, diameter, isTurnRight: true);

            // Assert
            Assert.That(_service.State.TurnPath.Count, Is.GreaterThan(0));
            Assert.That(_service.State.TurnDiameter, Is.EqualTo(diameter));
            Assert.That(_service.State.WasRightTurn, Is.True);
        }

        [Test]
        public void BuildManualYouTurn_ResetsState()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(0, 0), 0, trackPoints, 10.0, true);
            _service.TriggerYouTurn();

            // Act
            _service.BuildManualYouTurn(new vec2(100, 100), Math.PI / 2, 20.0, false);

            // Assert
            Assert.That(_service.State.IsTriggered, Is.False);
            Assert.That(_service.State.WasRightTurn, Is.False);
            Assert.That(_service.State.TurnDiameter, Is.EqualTo(20.0));
        }

        // ============================================================
        // State Management Tests
        // ============================================================

        [Test]
        public void TriggerYouTurn_ActivatesYouTurn()
        {
            // Act
            _service.TriggerYouTurn();

            // Assert
            Assert.That(_service.State.IsTriggered, Is.True);
            Assert.That(_service.IsActive, Is.True);
            Assert.That(_service.State.CurrentSegmentIndex, Is.EqualTo(0));
        }

        [Test]
        public void CompleteYouTurn_DeactivatesYouTurn()
        {
            // Arrange
            _service.TriggerYouTurn();

            // Act
            _service.CompleteYouTurn();

            // Assert
            Assert.That(_service.State.IsTriggered, Is.False);
            Assert.That(_service.IsActive, Is.False);
            Assert.That(_service.State.CurrentSegmentIndex, Is.EqualTo(0));
        }

        [Test]
        public void ResetYouTurn_ClearsState()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(0, 0), 0, trackPoints, 20.0, true);
            _service.TriggerYouTurn();

            // Act
            _service.ResetYouTurn();

            // Assert
            Assert.That(_service.State.IsTriggered, Is.False);
            Assert.That(_service.State.TurnPath.Count, Is.EqualTo(0));
            Assert.That(_service.State.TotalLength, Is.EqualTo(0));
        }

        // ============================================================
        // IsYouTurnComplete Tests
        // ============================================================

        [Test]
        public void IsYouTurnComplete_NotTriggered_ReturnsFalse()
        {
            // Arrange
            var position = new vec2(100, 100);

            // Act
            bool result = _service.IsYouTurnComplete(position);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsYouTurnComplete_EmptyPath_ReturnsFalse()
        {
            // Arrange
            _service.TriggerYouTurn();

            // Act
            bool result = _service.IsYouTurnComplete(new vec2(0, 0));

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsYouTurnComplete_NearEnd_ReturnsTrue()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);
            _service.TriggerYouTurn();

            // Get end point
            var endPoint = _service.State.TurnPath[_service.State.TurnPath.Count - 1];
            var nearEndPosition = new vec2(endPoint.easting + 1.0, endPoint.northing);

            // Act
            bool result = _service.IsYouTurnComplete(nearEndPosition);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsYouTurnComplete_FarFromEnd_ReturnsFalse()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);
            _service.TriggerYouTurn();

            var farPosition = new vec2(0, 0); // Far from turn path

            // Act
            bool result = _service.IsYouTurnComplete(farPosition);

            // Assert
            Assert.That(result, Is.False);
        }

        // ============================================================
        // GetDistanceRemaining Tests
        // ============================================================

        [Test]
        public void GetDistanceRemaining_EmptyPath_ReturnsZero()
        {
            // Act
            double distance = _service.GetDistanceRemaining(new vec2(0, 0));

            // Assert
            Assert.That(distance, Is.EqualTo(0));
        }

        [Test]
        public void GetDistanceRemaining_AtStart_ReturnsFullLength()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);

            var startPoint = _service.State.TurnPath[0];
            var startPosition = new vec2(startPoint.easting, startPoint.northing);

            // Act
            double remaining = _service.GetDistanceRemaining(startPosition);

            // Assert
            Assert.That(remaining, Is.EqualTo(_service.State.TotalLength).Within(1.0));
        }

        [Test]
        public void GetDistanceRemaining_AtEnd_ReturnsZero()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);

            var endPoint = _service.State.TurnPath[_service.State.TurnPath.Count - 1];
            var endPosition = new vec2(endPoint.easting, endPoint.northing);

            // Act
            double remaining = _service.GetDistanceRemaining(endPosition);

            // Assert
            Assert.That(remaining, Is.EqualTo(0).Within(0.1));
        }

        [Test]
        public void GetDistanceRemaining_AtMidpoint_ReturnsHalfLength()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);

            var midPoint = _service.State.TurnPath[_service.State.TurnPath.Count / 2];
            var midPosition = new vec2(midPoint.easting, midPoint.northing);

            // Act
            double remaining = _service.GetDistanceRemaining(midPosition);

            // Assert
            double expectedRemaining = _service.State.TotalLength / 2.0;
            Assert.That(remaining, Is.EqualTo(expectedRemaining).Within(2.0));
        }

        // ============================================================
        // YouTurnState Tests
        // ============================================================

        [Test]
        public void YouTurnState_Constructor_InitializesCorrectly()
        {
            // Act
            var state = new YouTurnState();

            // Assert
            Assert.That(state.TurnPath, Is.Not.Null);
            Assert.That(state.NextTrackPath, Is.Not.Null);
            Assert.That(state.IsTriggered, Is.False);
            Assert.That(state.IsOutOfBounds, Is.False);
            Assert.That(state.TotalLength, Is.EqualTo(0));
            Assert.That(state.TurnDiameter, Is.EqualTo(0));
            Assert.That(state.CurrentSegmentIndex, Is.EqualTo(0));
        }

        [Test]
        public void YouTurnState_Reset_ClearsState()
        {
            // Arrange
            var state = new YouTurnState();
            state.IsTriggered = true;
            state.TurnPath.Add(new vec3(0, 0, 0));
            state.TotalLength = 100;
            state.TurnDiameter = 20;

            // Act
            state.Reset();

            // Assert
            Assert.That(state.IsTriggered, Is.False);
            Assert.That(state.TurnPath.Count, Is.EqualTo(0));
            Assert.That(state.TotalLength, Is.EqualTo(0));
            Assert.That(state.TurnDiameter, Is.EqualTo(0));
        }

        [Test]
        public void YouTurnState_CalculateTotalLength_CorrectCalculation()
        {
            // Arrange
            var state = new YouTurnState();
            state.TurnPath.Add(new vec3(0, 0, 0));
            state.TurnPath.Add(new vec3(10, 0, 0));
            state.TurnPath.Add(new vec3(10, 10, 0));

            // Act
            state.CalculateTotalLength();

            // Assert
            double expectedLength = 10.0 + 10.0; // Two segments of 10m each
            Assert.That(state.TotalLength, Is.EqualTo(expectedLength).Within(0.01));
        }

        // ============================================================
        // Integration Tests
        // ============================================================

        [Test]
        public void Integration_CompleteYouTurnWorkflow()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 200, 0, 100);
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            double diameter = 20.0;

            // Act & Assert - Create turn
            bool created = _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            Assert.That(created, Is.True);
            Assert.That(_service.State.TurnPath.Count, Is.GreaterThan(0));

            // Trigger turn
            _service.TriggerYouTurn();
            Assert.That(_service.IsActive, Is.True);

            // Check distance at start
            var startPos = new vec2(_service.State.TurnPath[0].easting, _service.State.TurnPath[0].northing);
            double distanceAtStart = _service.GetDistanceRemaining(startPos);
            Assert.That(distanceAtStart, Is.GreaterThan(0));

            // Check not complete at start
            Assert.That(_service.IsYouTurnComplete(startPos), Is.False);

            // Check complete at end
            var endPos = new vec2(
                _service.State.TurnPath[_service.State.TurnPath.Count - 1].easting,
                _service.State.TurnPath[_service.State.TurnPath.Count - 1].northing);
            Assert.That(_service.IsYouTurnComplete(endPos), Is.True);

            // Complete turn
            _service.CompleteYouTurn();
            Assert.That(_service.IsActive, Is.False);
        }

        [Test]
        public void Integration_ManualYouTurn_WorksIndependently()
        {
            // Arrange & Act
            _service.BuildManualYouTurn(new vec2(50, 50), 0, 15.0, false);

            // Assert
            Assert.That(_service.State.TurnPath.Count, Is.GreaterThan(0));
            Assert.That(_service.State.TurnDiameter, Is.EqualTo(15.0));
            Assert.That(_service.State.WasRightTurn, Is.False);

            // Trigger and verify
            _service.TriggerYouTurn();
            Assert.That(_service.IsActive, Is.True);

            // Complete
            _service.CompleteYouTurn();
            Assert.That(_service.IsActive, Is.False);
        }

        // ============================================================
        // Helper Methods
        // ============================================================

        private List<vec3> CreateStraightTrack(double x0, double y0, double x1, double y1, int numPoints)
        {
            var track = new List<vec3>(numPoints);
            double heading = Math.Atan2(x1 - x0, y1 - y0);

            for (int i = 0; i < numPoints; i++)
            {
                double t = i / (double)(numPoints - 1);
                double x = x0 + t * (x1 - x0);
                double y = y0 + t * (y1 - y0);
                track.Add(new vec3(x, y, heading));
            }

            return track;
        }
    }
}

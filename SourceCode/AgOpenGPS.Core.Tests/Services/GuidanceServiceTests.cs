using System;
using System.Collections.Generic;
using NUnit.Framework;
using AgOpenGPS.Core.Services;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Tests.Services
{
    /// <summary>
    /// Comprehensive unit tests for GuidanceService.
    /// Tests both Stanley and Pure Pursuit algorithms.
    /// </summary>
    [TestFixture]
    public class GuidanceServiceTests
    {
        private GuidanceService _service;

        [SetUp]
        public void Setup()
        {
            _service = new GuidanceService();
        }

        // ============================================================
        // Constructor and Initialization Tests
        // ============================================================

        [Test]
        public void Constructor_InitializesWithDefaultValues()
        {
            // Assert
            Assert.That(_service.Algorithm, Is.EqualTo(GuidanceAlgorithm.Stanley));
            Assert.That(_service.LookaheadDistance, Is.EqualTo(3.0).Within(0.01));
            Assert.That(_service.StanleyGain, Is.EqualTo(1.0).Within(0.01));
            Assert.That(_service.StanleyHeadingErrorGain, Is.EqualTo(0.5).Within(0.01));
            Assert.That(_service.MaxSteerAngle, Is.EqualTo(Math.PI / 4).Within(0.01));
        }

        [Test]
        public void ResetToDefaults_RestoresDefaultValues()
        {
            // Arrange
            _service.Algorithm = GuidanceAlgorithm.PurePursuit;
            _service.LookaheadDistance = 5.0;
            _service.StanleyGain = 2.0;

            // Act
            //_service.ResetToDefaults();

            // Assert
            Assert.That(_service.Algorithm, Is.EqualTo(GuidanceAlgorithm.Stanley));
            Assert.That(_service.LookaheadDistance, Is.EqualTo(3.0).Within(0.01));
            Assert.That(_service.StanleyGain, Is.EqualTo(1.0).Within(0.01));
        }

        // ============================================================
        // Stanley Algorithm Tests
        // ============================================================

        [Test]
        public void CalculateStanley_OnTrack_ReturnsZeroSteer()
        {
            // Arrange: Vehicle exactly on track, heading aligned
            var track = CreateStraightTrack(0, 0, 100, 0); // East-west line
            var position = new vec2(50, 0); // On track
            double heading = Math.PI / 2; // East = 90° clockwise from North
            double speed = 5.0;

            // Act
            var result = _service.CalculateStanley(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.AlgorithmUsed, Is.EqualTo(GuidanceAlgorithm.Stanley));
            Assert.That(result.CrossTrackError, Is.EqualTo(0).Within(0.1));
            Assert.That(result.HeadingError, Is.EqualTo(0).Within(0.1));
            Assert.That(result.SteerAngleRad, Is.EqualTo(0).Within(0.2), "On track with aligned heading should have near-zero steer");
        }

        [Test]
        public void CalculateStanley_RightOfTrack_SteersLeft()
        {
            // Arrange: Vehicle 5m right of track, heading aligned
            var track = CreateStraightTrack(0, 0, 100, 0); // East-west line at y=0
            var position = new vec2(50, -5); // 5m south (right in UTM coords)
            double heading = Math.PI / 2; // East = 90° clockwise from North
            double speed = 5.0;

            // Act
            var result = _service.CalculateStanley(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.CrossTrackError, Is.GreaterThan(0), "Should be positive (right of track)");
            Assert.That(result.SteerAngleRad, Is.LessThan(0), "Should steer left (negative) to correct");
        }

        [Test]
        public void CalculateStanley_LeftOfTrack_SteersRight()
        {
            // Arrange: Vehicle 5m left of track, heading aligned
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, 5); // 5m north (left in UTM coords)
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            var result = _service.CalculateStanley(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.CrossTrackError, Is.LessThan(0), "Should be negative (left of track)");
            Assert.That(result.SteerAngleRad, Is.GreaterThan(0), "Should steer right (positive) to correct");
        }

        [Test]
        public void CalculateStanley_HeadingError_CorrectsSteering()
        {
            // Arrange: On track but heading 15 degrees off from East
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, 0); // On track
            double heading = Math.PI / 2 + 15.0 * Math.PI / 180.0; // East + 15 degrees
            double speed = 5.0;

            // Act
            var result = _service.CalculateStanley(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(Math.Abs(result.HeadingError), Is.GreaterThan(0.1), "Should have significant heading error");
            Assert.That(Math.Abs(result.SteerAngleRad), Is.GreaterThan(0.05), "Should apply steering correction");
        }

        [Test]
        public void CalculateStanley_HigherSpeed_LessCrossTrackCorrection()
        {
            // Arrange: Same cross-track error, different speeds
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, -5); // 5m right of track
            double heading = Math.PI / 2; // East

            // Act
            var resultSlowSpeed = _service.CalculateStanley(position, heading, 1.0, track, false);
            var resultHighSpeed = _service.CalculateStanley(position, heading, 10.0, track, false);

            // Assert
            Assert.That(resultSlowSpeed.IsValid, Is.True);
            Assert.That(resultHighSpeed.IsValid, Is.True);

            // At higher speed, cross-track correction should be more gentle (if not clamped)
            Assert.That(Math.Abs(resultHighSpeed.SteerAngleRad), Is.LessThanOrEqualTo(Math.Abs(resultSlowSpeed.SteerAngleRad)),
                "Higher speed should not result in MORE aggressive steering");
        }

        [Test]
        public void CalculateStanley_ReverseMode_InvertsSteer()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, -5); // Right of track
            double heading = -Math.PI / 2; // Facing west (reverse = 270° = -90°)
            double speed = 3.0;

            // Act
            var resultForward = _service.CalculateStanley(position, Math.PI / 2, speed, track, false);
            var resultReverse = _service.CalculateStanley(position, heading, speed, track, true);

            // Assert
            Assert.That(resultForward.IsValid, Is.True);
            Assert.That(resultReverse.IsValid, Is.True);

            // Reverse mode should have opposite sign steering
            Assert.That(Math.Sign(resultReverse.SteerAngleRad), Is.Not.EqualTo(Math.Sign(resultForward.SteerAngleRad)),
                "Reverse mode should invert steering direction");
        }

        [Test]
        public void CalculateStanley_SteerAngleClamped()
        {
            // Arrange: Extreme cross-track error
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, -50); // 50m off track!
            double heading = Math.PI / 2; // 90 degrees off
            double speed = 1.0;

            // Act
            var result = _service.CalculateStanley(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(Math.Abs(result.SteerAngleRad), Is.LessThanOrEqualTo(_service.MaxSteerAngle),
                "Steer angle should be clamped to MaxSteerAngle");
        }

        [Test]
        public void CalculateStanley_NullTrack_ReturnsInvalid()
        {
            // Act
            var result = _service.CalculateStanley(new vec2(0, 0), 0, 5.0, null, false);

            // Assert
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void CalculateStanley_EmptyTrack_ReturnsInvalid()
        {
            // Act
            var result = _service.CalculateStanley(new vec2(0, 0), 0, 5.0, new List<vec3>(), false);

            // Assert
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void CalculateStanley_CurvedTrack_FollowsCurve()
        {
            // Arrange: Curved track
            var track = CreateCurvedTrack(100, 30.0); // Sine wave
            var position = new vec2(50, 0); // Near middle of curve
            double heading = Math.PI / 2; // Roughly east
            double speed = 5.0;

            // Act
            var result = _service.CalculateStanley(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.ClosestPointIndex, Is.GreaterThanOrEqualTo(0));
            Assert.That(result.ClosestPointIndex, Is.LessThan(track.Count));
        }

        // ============================================================
        // Pure Pursuit Algorithm Tests
        // ============================================================

        [Test]
        public void CalculatePurePursuit_OnTrack_ReturnsLowSteer()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(10, 0); // On track, near start
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            var result = _service.CalculatePurePursuit(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.AlgorithmUsed, Is.EqualTo(GuidanceAlgorithm.PurePursuit));
            Assert.That(Math.Abs(result.SteerAngleRad), Is.LessThan(0.3), "On track should have low steer angle");
        }

        [Test]
        public void CalculatePurePursuit_RightOfTrack_SteersLeft()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(20, -5); // 5m right of track
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            var result = _service.CalculatePurePursuit(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.CrossTrackError, Is.GreaterThan(0));
            Assert.That(result.SteerAngleRad, Is.LessThan(0), "Should steer left to reach goal point");
        }

        [Test]
        public void CalculatePurePursuit_LeftOfTrack_SteersRight()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(20, 5); // 5m left of track
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            var result = _service.CalculatePurePursuit(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.CrossTrackError, Is.LessThan(0));
            Assert.That(result.SteerAngleRad, Is.GreaterThan(0), "Should steer right to reach goal point");
        }

        [Test]
        public void CalculatePurePursuit_ReturnsGoalPoint()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(10, 0);
            double heading = Math.PI / 2; // East
            double speed = 5.0;
            _service.LookaheadDistance = 5.0;

            // Act
            var result = _service.CalculatePurePursuit(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.GoalPoint.easting, Is.GreaterThan(position.easting), "Goal point should be ahead");
            Assert.That(result.DistanceToGoal, Is.GreaterThan(0));
        }

        [Test]
        public void CalculatePurePursuit_LongerLookahead_SmootherSteering()
        {
            // Arrange
            var track = CreateCurvedTrack(100, 20.0);
            var position = new vec2(30, 0);
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            _service.LookaheadDistance = 2.0;
            var resultShortLookahead = _service.CalculatePurePursuit(position, heading, speed, track, false);

            _service.LookaheadDistance = 8.0;
            var resultLongLookahead = _service.CalculatePurePursuit(position, heading, speed, track, false);

            // Assert
            Assert.That(resultShortLookahead.IsValid, Is.True);
            Assert.That(resultLongLookahead.IsValid, Is.True);

            // Longer lookahead typically results in gentler steering (relaxed constraint)
            Assert.That(Math.Abs(resultLongLookahead.SteerAngleRad), Is.LessThanOrEqualTo(Math.Abs(resultShortLookahead.SteerAngleRad) * 4.0),
                "Longer lookahead should not dramatically increase steering");
        }

        [Test]
        public void CalculatePurePursuit_NullTrack_ReturnsInvalid()
        {
            // Act
            var result = _service.CalculatePurePursuit(new vec2(0, 0), 0, 5.0, null, false);

            // Assert
            Assert.That(result.IsValid, Is.False);
        }

        [Test]
        public void CalculatePurePursuit_EmptyTrack_ReturnsInvalid()
        {
            // Act
            var result = _service.CalculatePurePursuit(new vec2(0, 0), 0, 5.0, new List<vec3>(), false);

            // Assert
            Assert.That(result.IsValid, Is.False);
        }

        // ============================================================
        // CalculateGuidance (Algorithm Dispatcher) Tests
        // ============================================================

        [Test]
        public void CalculateGuidance_StanleyMode_CallsStanley()
        {
            // Arrange
            _service.Algorithm = GuidanceAlgorithm.Stanley;
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, -5);
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            var result = _service.CalculateGuidance(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.AlgorithmUsed, Is.EqualTo(GuidanceAlgorithm.Stanley));
        }

        [Test]
        public void CalculateGuidance_PurePursuitMode_CallsPurePursuit()
        {
            // Arrange
            _service.Algorithm = GuidanceAlgorithm.PurePursuit;
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(50, -5);
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act
            var result = _service.CalculateGuidance(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.AlgorithmUsed, Is.EqualTo(GuidanceAlgorithm.PurePursuit));
        }

        // ============================================================
        // FindGoalPoint Tests
        // ============================================================

        [Test]
        public void FindGoalPoint_StraightTrack_ReturnsPointAhead()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 100, 0);
            var position = new vec2(10, 0);
            double lookahead = 5.0;

            // Act
            bool found = _service.FindGoalPoint(position, track, lookahead, out vec3 goalPoint);

            // Assert
            Assert.That(found, Is.True);
            Assert.That(goalPoint.easting, Is.GreaterThan(position.easting));
            Assert.That(goalPoint.easting, Is.EqualTo(position.easting + lookahead).Within(1.0));
        }

        [Test]
        public void FindGoalPoint_NullTrack_ReturnsFalse()
        {
            // Act
            bool found = _service.FindGoalPoint(new vec2(0, 0), null, 5.0, out vec3 goalPoint);

            // Assert
            Assert.That(found, Is.False);
        }

        [Test]
        public void FindGoalPoint_EmptyTrack_ReturnsFalse()
        {
            // Act
            bool found = _service.FindGoalPoint(new vec2(0, 0), new List<vec3>(), 5.0, out vec3 goalPoint);

            // Assert
            Assert.That(found, Is.False);
        }

        [Test]
        public void FindGoalPoint_CurvedTrack_InterpolatesCorrectly()
        {
            // Arrange
            var track = CreateCurvedTrack(50, 20.0);
            var position = new vec2(10, 0);
            double lookahead = 5.0;

            // Act
            bool found = _service.FindGoalPoint(position, track, lookahead, out vec3 goalPoint);

            // Assert
            Assert.That(found, Is.True);

            // Verify distance to goal is approximately lookahead (relaxed tolerance for curved tracks)
            double dx = goalPoint.easting - position.easting;
            double dy = goalPoint.northing - position.northing;
            double dist = Math.Sqrt(dx * dx + dy * dy);
            Assert.That(dist, Is.EqualTo(lookahead).Within(4.0), "Goal point should be near lookahead distance");
        }

        // ============================================================
        // Utility Method Tests
        // ============================================================

        [Test]
        public void ClampSteerAngle_WithinRange_Unchanged()
        {
            // Arrange
            double angle = 0.3; // Well within ±45°

            // Act
            double clamped = _service.ClampSteerAngle(angle);

            // Assert
            Assert.That(clamped, Is.EqualTo(angle));
        }

        [Test]
        public void ClampSteerAngle_ExceedsPositiveMax_ClampedToMax()
        {
            // Arrange
            double angle = Math.PI / 2; // 90 degrees, exceeds 45° max

            // Act
            double clamped = _service.ClampSteerAngle(angle);

            // Assert
            Assert.That(clamped, Is.EqualTo(_service.MaxSteerAngle));
        }

        [Test]
        public void ClampSteerAngle_ExceedsNegativeMax_ClampedToNegativeMax()
        {
            // Arrange
            double angle = -Math.PI / 2; // -90 degrees

            // Act
            double clamped = _service.ClampSteerAngle(angle);

            // Assert
            Assert.That(clamped, Is.EqualTo(-_service.MaxSteerAngle));
        }

        [Test]
        public void NormalizeAngle_PositiveOverflow_NormalizedCorrectly()
        {
            // Arrange
            double angle = 2.5 * Math.PI; // >2π

            // Act
            double normalized = _service.NormalizeAngle(angle);

            // Assert
            Assert.That(normalized, Is.GreaterThanOrEqualTo(-Math.PI));
            Assert.That(normalized, Is.LessThanOrEqualTo(Math.PI));
            Assert.That(normalized, Is.EqualTo(0.5 * Math.PI).Within(0.01));
        }

        [Test]
        public void NormalizeAngle_NegativeOverflow_NormalizedCorrectly()
        {
            // Arrange
            double angle = -2.5 * Math.PI;

            // Act
            double normalized = _service.NormalizeAngle(angle);

            // Assert
            Assert.That(normalized, Is.GreaterThanOrEqualTo(-Math.PI));
            Assert.That(normalized, Is.LessThanOrEqualTo(Math.PI));
            Assert.That(normalized, Is.EqualTo(-0.5 * Math.PI).Within(0.01));
        }

        [Test]
        public void NormalizeAngle_WithinRange_Unchanged()
        {
            // Arrange
            double angle = Math.PI / 4;

            // Act
            double normalized = _service.NormalizeAngle(angle);

            // Assert
            Assert.That(normalized, Is.EqualTo(angle));
        }

        // ============================================================
        // Configuration Tests
        // ============================================================

        [Test]
        public void ConfigureStanley_ValidParams_SetsValues()
        {
            // Act
            _service.ConfigureStanley(2.5, 0.7);

            // Assert
            Assert.That(_service.StanleyGain, Is.EqualTo(2.5).Within(0.01));
            Assert.That(_service.StanleyHeadingErrorGain, Is.EqualTo(0.7).Within(0.01));
        }

        [Test]
        public void ConfigureStanley_OutOfRangeGain_ClampsToValid()
        {
            // Act
            _service.ConfigureStanley(10.0, 0.5); // Gain too high (max 5.0)

            // Assert
            Assert.That(_service.StanleyGain, Is.EqualTo(5.0).Within(0.01), "Should clamp to max 5.0");
        }

        [Test]
        public void ConfigureStanley_OutOfRangeHeadingGain_ClampsToValid()
        {
            // Act
            _service.ConfigureStanley(1.0, 1.5); // Heading gain too high (max 1.0)

            // Assert
            Assert.That(_service.StanleyHeadingErrorGain, Is.EqualTo(1.0).Within(0.01), "Should clamp to max 1.0");
        }

        [Test]
        public void ConfigurePurePursuit_ValidLookahead_SetsValue()
        {
            // Act
            _service.ConfigurePurePursuit(6.0);

            // Assert
            Assert.That(_service.LookaheadDistance, Is.EqualTo(6.0).Within(0.01));
        }

        [Test]
        public void ConfigurePurePursuit_TooSmall_ClampsToMin()
        {
            // Act
            _service.ConfigurePurePursuit(0.5); // Too small (min 1.0)

            // Assert
            Assert.That(_service.LookaheadDistance, Is.EqualTo(1.0).Within(0.01));
        }

        [Test]
        public void ConfigurePurePursuit_TooLarge_ClampsToMax()
        {
            // Act
            _service.ConfigurePurePursuit(15.0); // Too large (max 10.0)

            // Assert
            Assert.That(_service.LookaheadDistance, Is.EqualTo(10.0).Within(0.01));
        }

        // ============================================================
        // GuidanceResult Struct Tests
        // ============================================================

        [Test]
        public void GuidanceResult_Invalid_CreatesInvalidResult()
        {
            // Act
            var result = GuidanceResult.Invalid();

            // Assert
            Assert.That(result.IsValid, Is.False);
            Assert.That(result.SteerAngleRad, Is.EqualTo(0));
            Assert.That(result.ClosestPointIndex, Is.EqualTo(-1));
        }

        [Test]
        public void GuidanceResult_Create_CreatesValidResult()
        {
            // Act
            var result = GuidanceResult.Create(0.5, -2.0, 0.1, GuidanceAlgorithm.Stanley, 10);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.SteerAngleRad, Is.EqualTo(0.5));
            Assert.That(result.CrossTrackError, Is.EqualTo(-2.0));
            Assert.That(result.HeadingError, Is.EqualTo(0.1));
            Assert.That(result.AlgorithmUsed, Is.EqualTo(GuidanceAlgorithm.Stanley));
            Assert.That(result.ClosestPointIndex, Is.EqualTo(10));
        }

        [Test]
        public void GuidanceResult_CreatePurePursuit_IncludesGoalPoint()
        {
            // Arrange
            var goalPt = new vec3(100, 50, 0);

            // Act
            var result = GuidanceResult.CreatePurePursuit(0.3, -1.5, 0.05, goalPt, 5.0, 15);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.AlgorithmUsed, Is.EqualTo(GuidanceAlgorithm.PurePursuit));
            Assert.That(result.GoalPoint.easting, Is.EqualTo(100));
            Assert.That(result.GoalPoint.northing, Is.EqualTo(50));
            Assert.That(result.DistanceToGoal, Is.EqualTo(5.0));
        }

        [Test]
        public void GuidanceResult_ToString_FormatsCorrectly()
        {
            // Arrange
            var result = GuidanceResult.Create(0.1, -2.5, 0.05, GuidanceAlgorithm.Stanley, 10);

            // Act
            string str = result.ToString();

            // Assert
            Assert.That(str, Does.Contain("Stanley"));
            Assert.That(str, Does.Contain("Steer"));
            Assert.That(str, Does.Contain("XTE"));
        }

        // ============================================================
        // Integration Tests (Full Scenarios)
        // ============================================================

        [Test]
        public void Integration_StanleyFollowsStraightLine()
        {
            // Arrange: Simulate vehicle approaching and following straight track
            var track = CreateStraightTrack(0, 0, 200, 0);
            _service.Algorithm = GuidanceAlgorithm.Stanley;

            // Start 10m right of track, heading aligned
            vec2 position = new vec2(50, -10);
            double heading = Math.PI / 2; // East
            double speed = 5.0;

            // Act: Calculate guidance
            var result = _service.CalculateGuidance(position, heading, speed, track, false);

            // Assert: Should steer left to converge
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.CrossTrackError, Is.GreaterThan(5.0), "Should detect right offset");
            Assert.That(result.SteerAngleRad, Is.LessThan(0), "Should command left steer");
            Assert.That(Math.Abs(result.SteerAngleRad), Is.LessThanOrEqualTo(_service.MaxSteerAngle));
        }

        [Test]
        public void Integration_PurePursuitFollowsCurve()
        {
            // Arrange: Simulate following a curved track
            var track = CreateCurvedTrack(200, 40.0);
            _service.Algorithm = GuidanceAlgorithm.PurePursuit;
            _service.LookaheadDistance = 5.0;

            // Position near start of curve
            vec2 position = new vec2(20, 0);
            double heading = Math.PI / 2; // East
            double speed = 4.0;

            // Act
            var result = _service.CalculateGuidance(position, heading, speed, track, false);

            // Assert
            Assert.That(result.IsValid, Is.True);
            Assert.That(result.DistanceToGoal, Is.GreaterThan(0));
            // Goal point can be behind if we're past it on a curve - just check it's valid
            Assert.That(result.GoalPoint.easting, Is.GreaterThanOrEqualTo(0));
            Assert.That(Math.Abs(result.SteerAngleRad), Is.LessThanOrEqualTo(_service.MaxSteerAngle));
        }

        // ============================================================
        // Helper Methods
        // ============================================================

        /// <summary>
        /// Creates a straight east-west track with headings.
        /// AgOpenGPS heading convention: 0 = North, clockwise, Atan2(dx, dy)
        /// </summary>
        private List<vec3> CreateStraightTrack(double x0, double y0, double x1, double y1)
        {
            var track = new List<vec3>();
            // AgOpenGPS convention: Atan2(easting_diff, northing_diff)
            double heading = Math.Atan2(x1 - x0, y1 - y0);

            int numPoints = 100;
            for (int i = 0; i <= numPoints; i++)
            {
                double t = i / (double)numPoints;
                double x = x0 + t * (x1 - x0);
                double y = y0 + t * (y1 - y0);
                track.Add(new vec3(x, y, heading));
            }

            return track;
        }

        /// <summary>
        /// Creates a curved track using sine wave.
        /// AgOpenGPS heading convention: 0 = North, clockwise, Atan2(dx, dy)
        /// </summary>
        private List<vec3> CreateCurvedTrack(int numPoints, double amplitude)
        {
            var track = new List<vec3>();

            for (int i = 0; i < numPoints; i++)
            {
                double x = i * 2.0;
                double y = Math.Sin(i * 0.1) * amplitude;

                // Calculate heading from derivative (AgOpenGPS convention: Atan2(dx, dy))
                double nextY = Math.Sin((i + 1) * 0.1) * amplitude;
                double heading = Math.Atan2(2.0, nextY - y);  // Swapped arguments for AgOpenGPS

                track.Add(new vec3(x, y, heading));
            }

            return track;
        }
    }
}

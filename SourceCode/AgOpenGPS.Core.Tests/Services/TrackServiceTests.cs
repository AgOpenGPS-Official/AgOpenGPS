using System;
using System.Collections.Generic;
using NUnit.Framework;
using AgOpenGPS.Core.Services;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;

namespace AgOpenGPS.Core.Tests.Services
{
    /// <summary>
    /// Unit tests for TrackService
    /// Tests track management, creation, operations, and queries
    /// </summary>
    [TestFixture]
    public class TrackServiceTests
    {
        private TrackService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TrackService();
        }

        // ============================================================
        // Track Management Tests
        // ============================================================

        [Test]
        public void Constructor_CreatesEmptyTrackCollection()
        {
            // Assert
            Assert.That(_service.Tracks, Is.Not.Null);
            Assert.That(_service.Tracks.Count, Is.EqualTo(0));
            Assert.That(_service.GetCurrentTrack(), Is.Null);
        }

        [Test]
        public void AddTrack_AddsTrackToCollection()
        {
            // Arrange
            var track = new Track("Test Track", TrackMode.AB);

            // Act
            _service.AddTrack(track);

            // Assert
            Assert.That(_service.Tracks.Count, Is.EqualTo(1));
            Assert.That(_service.Tracks.GetByIndex(0).Name, Is.EqualTo("Test Track"));
        }

        [Test]
        public void AddTrack_WithNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.AddTrack(null));
        }

        [Test]
        public void RemoveTrack_RemovesTrackFromCollection()
        {
            // Arrange
            var track = new Track("Test Track", TrackMode.AB);
            _service.AddTrack(track);

            // Act
            _service.RemoveTrack(track);

            // Assert
            Assert.That(_service.Tracks.Count, Is.EqualTo(0));
        }

        [Test]
        public void RemoveTrack_WithNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.RemoveTrack(null));
        }

        [Test]
        public void SetCurrentTrack_SetsActiveTrack()
        {
            // Arrange
            var track = new Track("Test Track", TrackMode.AB);
            _service.AddTrack(track);

            // Act
            _service.SetCurrentTrack(track);

            // Assert
            Assert.That(_service.GetCurrentTrack(), Is.EqualTo(track));
        }

        [Test]
        public void SetCurrentTrack_WithNull_ClearsCurrentTrack()
        {
            // Arrange
            var track = new Track("Test Track", TrackMode.AB);
            _service.AddTrack(track);
            _service.SetCurrentTrack(track);

            // Act
            _service.SetCurrentTrack(null);

            // Assert
            Assert.That(_service.GetCurrentTrack(), Is.Null);
        }

        [Test]
        public void ClearTracks_RemovesAllTracks()
        {
            // Arrange
            _service.AddTrack(new Track("Track 1", TrackMode.AB));
            _service.AddTrack(new Track("Track 2", TrackMode.Curve));
            _service.AddTrack(new Track("Track 3", TrackMode.BoundaryTrackOuter));

            // Act
            _service.ClearTracks();

            // Assert
            Assert.That(_service.Tracks.Count, Is.EqualTo(0));
            Assert.That(_service.GetCurrentTrack(), Is.Null);
        }

        // ============================================================
        // Track Creation Tests
        // ============================================================

        [Test]
        public void CreateABTrack_CreatesValidTrack()
        {
            // Arrange
            var ptA = new vec2(100, 200);
            var ptB = new vec2(500, 600);
            double heading = Math.PI / 4; // 45 degrees

            // Act
            var track = _service.CreateABTrack(ptA, ptB, heading, "AB Test");

            // Assert
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Name, Is.EqualTo("AB Test"));
            Assert.That(track.Mode, Is.EqualTo(TrackMode.AB));
            Assert.That(track.PtA.easting, Is.EqualTo(ptA.easting).Within(0.001));
            Assert.That(track.PtA.northing, Is.EqualTo(ptA.northing).Within(0.001));
            Assert.That(track.PtB.easting, Is.EqualTo(ptB.easting).Within(0.001));
            Assert.That(track.PtB.northing, Is.EqualTo(ptB.northing).Within(0.001));
            Assert.That(track.Heading, Is.EqualTo(heading).Within(0.001));
            Assert.That(track.CurvePts.Count, Is.GreaterThan(0), "AB line should have curve points");
        }

        [Test]
        public void CreateABTrack_GeneratesCurvePoints()
        {
            // Arrange
            var ptA = new vec2(0, 0);
            var ptB = new vec2(100, 0); // 100 meter line

            // Act
            var track = _service.CreateABTrack(ptA, ptB, 0, "AB Line");

            // Assert
            Assert.That(track.CurvePts.Count, Is.GreaterThanOrEqualTo(50), "Should have at least 50 points for 100m line");

            // Verify first point is near ptA
            Assert.That(track.CurvePts[0].easting, Is.EqualTo(ptA.easting).Within(1.0));
            Assert.That(track.CurvePts[0].northing, Is.EqualTo(ptA.northing).Within(1.0));

            // Verify last point is near ptB
            int lastIdx = track.CurvePts.Count - 1;
            Assert.That(track.CurvePts[lastIdx].easting, Is.EqualTo(ptB.easting).Within(1.0));
            Assert.That(track.CurvePts[lastIdx].northing, Is.EqualTo(ptB.northing).Within(1.0));
        }

        [Test]
        public void CreateCurveTrack_CreatesValidTrack()
        {
            // Arrange
            var recordedPath = CreateTestCurvePath(10);

            // Act
            var track = _service.CreateCurveTrack(recordedPath, "Curve Test");

            // Assert
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Name, Is.EqualTo("Curve Test"));
            Assert.That(track.Mode, Is.EqualTo(TrackMode.Curve));
            Assert.That(track.CurvePts.Count, Is.EqualTo(10));
        }

        [Test]
        public void CreateCurveTrack_WithLessThan2Points_ThrowsException()
        {
            // Arrange
            var recordedPath = new List<vec3> { new vec3(0, 0, 0) };

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.CreateCurveTrack(recordedPath, "Invalid"));
        }

        [Test]
        public void CreateCurveTrack_WithNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _service.CreateCurveTrack(null, "Invalid"));
        }

        [Test]
        public void CreateBoundaryTrack_CreatesOuterTrack()
        {
            // Arrange
            var boundaryPoints = CreateTestBoundaryPolygon(8);

            // Act
            var track = _service.CreateBoundaryTrack(boundaryPoints, isOuter: true, "Outer Boundary");

            // Assert
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Name, Is.EqualTo("Outer Boundary"));
            Assert.That(track.Mode, Is.EqualTo(TrackMode.BoundaryTrackOuter));
            Assert.That(track.CurvePts.Count, Is.EqualTo(8));
        }

        [Test]
        public void CreateBoundaryTrack_CreatesInnerTrack()
        {
            // Arrange
            var boundaryPoints = CreateTestBoundaryPolygon(6);

            // Act
            var track = _service.CreateBoundaryTrack(boundaryPoints, isOuter: false, "Inner Boundary");

            // Assert
            Assert.That(track, Is.Not.Null);
            Assert.That(track.Name, Is.EqualTo("Inner Boundary"));
            Assert.That(track.Mode, Is.EqualTo(TrackMode.BoundaryTrackInner));
        }

        [Test]
        public void CreateBoundaryTrack_WithLessThan3Points_ThrowsException()
        {
            // Arrange
            var boundaryPoints = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(10, 0, 0)
            };

            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
                _service.CreateBoundaryTrack(boundaryPoints, true, "Invalid"));
        }

        // ============================================================
        // Track Operations Tests
        // ============================================================

        [Test]
        public void NudgeTrack_IncreasesNudgeDistance()
        {
            // Arrange
            var track = new Track("Test", TrackMode.AB);
            double initialNudge = track.NudgeDistance;

            // Act
            _service.NudgeTrack(track, 5.0);

            // Assert
            Assert.That(track.NudgeDistance, Is.EqualTo(initialNudge + 5.0).Within(0.001));
        }

        [Test]
        public void NudgeTrack_AccumulatesMultipleNudges()
        {
            // Arrange
            var track = new Track("Test", TrackMode.AB);

            // Act
            _service.NudgeTrack(track, 2.0);
            _service.NudgeTrack(track, 3.0);
            _service.NudgeTrack(track, -1.0);

            // Assert
            Assert.That(track.NudgeDistance, Is.EqualTo(4.0).Within(0.001));
        }

        [Test]
        public void NudgeTrack_WithNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.NudgeTrack(null, 5.0));
        }

        [Test]
        public void ResetNudge_SetsNudgeToZero()
        {
            // Arrange
            var track = new Track("Test", TrackMode.AB);
            _service.NudgeTrack(track, 10.0);

            // Act
            _service.ResetNudge(track);

            // Assert
            Assert.That(track.NudgeDistance, Is.EqualTo(0.0).Within(0.001));
        }

        [Test]
        public void ResetNudge_WithNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _service.ResetNudge(null));
        }

        [Test]
        public void SnapToPivot_WithWaterPivotTrack_RoundsToNearestSpacing()
        {
            // Arrange
            var track = new Track("Pivot", TrackMode.WaterPivot);
            double currentDistance = 7.8; // Should snap to 9.0 (nearest 3m spacing)

            // Act
            _service.SnapToPivot(track, currentDistance);

            // Assert
            // Nudge distance should be (9.0 - 7.8) = 1.2
            Assert.That(track.NudgeDistance, Is.EqualTo(1.2).Within(0.01));
        }

        [Test]
        public void SnapToPivot_WithNonPivotTrack_DoesNothing()
        {
            // Arrange
            var track = new Track("AB", TrackMode.AB);
            double initialNudge = track.NudgeDistance;

            // Act
            _service.SnapToPivot(track, 7.8);

            // Assert
            Assert.That(track.NudgeDistance, Is.EqualTo(initialNudge));
        }

        // ============================================================
        // Track Query Tests
        // ============================================================

        [Test]
        public void FindTrackById_ReturnsCorrectTrack()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _service.AddTrack(track1);
            _service.AddTrack(track2);

            // Act
            var found = _service.FindTrackById(track2.Id);

            // Assert
            Assert.That(found, Is.EqualTo(track2));
        }

        [Test]
        public void FindTrackById_WithNonExistentId_ReturnsNull()
        {
            // Arrange
            var track = new Track("Track", TrackMode.AB);
            _service.AddTrack(track);

            // Act
            var found = _service.FindTrackById(Guid.NewGuid());

            // Assert
            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindTrackByName_ReturnsCorrectTrack()
        {
            // Arrange
            _service.AddTrack(new Track("Alpha", TrackMode.AB));
            _service.AddTrack(new Track("Beta", TrackMode.Curve));
            _service.AddTrack(new Track("Gamma", TrackMode.BoundaryTrackOuter));

            // Act
            var found = _service.FindTrackByName("Beta");

            // Assert
            Assert.That(found, Is.Not.Null);
            Assert.That(found.Name, Is.EqualTo("Beta"));
        }

        [Test]
        public void FindTrackByName_IsCaseInsensitive()
        {
            // Arrange
            _service.AddTrack(new Track("TestTrack", TrackMode.AB));

            // Act
            var found = _service.FindTrackByName("testtrack");

            // Assert
            Assert.That(found, Is.Not.Null);
            Assert.That(found.Name, Is.EqualTo("TestTrack"));
        }

        [Test]
        public void FindTrackByName_WithNonExistentName_ReturnsNull()
        {
            // Arrange
            _service.AddTrack(new Track("Track", TrackMode.AB));

            // Act
            var found = _service.FindTrackByName("NonExistent");

            // Assert
            Assert.That(found, Is.Null);
        }

        [Test]
        public void GetTracksByMode_ReturnsOnlyMatchingTracks()
        {
            // Arrange
            _service.AddTrack(new Track("AB1", TrackMode.AB));
            _service.AddTrack(new Track("Curve1", TrackMode.Curve));
            _service.AddTrack(new Track("AB2", TrackMode.AB));
            _service.AddTrack(new Track("Boundary", TrackMode.BoundaryTrackOuter));
            _service.AddTrack(new Track("AB3", TrackMode.AB));

            // Act
            var abTracks = _service.GetTracksByMode(TrackMode.AB);

            // Assert
            Assert.That(abTracks.Count, Is.EqualTo(3));
            Assert.That(abTracks.TrueForAll(t => t.Mode == TrackMode.AB), Is.True);
        }

        [Test]
        public void GetTracksByMode_WithNoMatches_ReturnsEmptyList()
        {
            // Arrange
            _service.AddTrack(new Track("AB", TrackMode.AB));

            // Act
            var curveTracks = _service.GetTracksByMode(TrackMode.Curve);

            // Assert
            Assert.That(curveTracks.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetVisibleTrackCount_ReturnsCorrectCount()
        {
            // Arrange
            var track1 = new Track("Track1", TrackMode.AB) { IsVisible = true };
            var track2 = new Track("Track2", TrackMode.AB) { IsVisible = false };
            var track3 = new Track("Track3", TrackMode.AB) { IsVisible = true };
            var track4 = new Track("Track4", TrackMode.AB) { IsVisible = true };

            _service.AddTrack(track1);
            _service.AddTrack(track2);
            _service.AddTrack(track3);
            _service.AddTrack(track4);

            // Act
            int visibleCount = _service.GetVisibleTrackCount();

            // Assert
            Assert.That(visibleCount, Is.EqualTo(3));
        }

        // ============================================================
        // BuildGuidanceTrack Tests
        // ============================================================

        [Test]
        public void BuildGuidanceTrack_WithNullTrack_ReturnsEmptyList()
        {
            // Act
            var result = _service.BuildGuidanceTrack(null, 10.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void BuildGuidanceTrack_WithInvalidTrack_ReturnsEmptyList()
        {
            // Arrange
            var track = new Track("Invalid", TrackMode.AB); // No points

            // Act
            var result = _service.BuildGuidanceTrack(track, 10.0);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void BuildGuidanceTrack_WithABLine_ReturnsExtendedTrack()
        {
            // Arrange
            var track = _service.CreateABTrack(
                new vec2(0, 0),
                new vec2(100, 0),
                0,
                "AB Test");

            int originalCount = track.CurvePts.Count;

            // Act
            var result = _service.BuildGuidanceTrack(track, 0);

            // Assert
            Assert.That(result.Count, Is.GreaterThan(originalCount), "Should have extended points");

            // Verify extensions exist (first and last points should be far from original)
            double firstDist = Math.Sqrt(
                Math.Pow(result[0].easting - track.CurvePts[0].easting, 2) +
                Math.Pow(result[0].northing - track.CurvePts[0].northing, 2));

            Assert.That(firstDist, Is.GreaterThan(1000), "Should have 10km extension at start");
        }

        [Test]
        public void BuildGuidanceTrack_WithCurve_ReturnsSmoothedTrack()
        {
            // Arrange
            var curvePath = CreateTestCurvePath(20);
            var track = _service.CreateCurveTrack(curvePath, "Curve Test");

            // Act
            var result = _service.BuildGuidanceTrack(track, 0);

            // Assert
            Assert.That(result.Count, Is.GreaterThan(0));
            // Smoothing may increase point count due to interpolation
        }

        // ============================================================
        // GetDistanceFromTrack Tests
        // ============================================================

        [Test]
        public void GetDistanceFromTrack_WithNullTrack_ReturnsZero()
        {
            // Act
            var (distance, sameway) = _service.GetDistanceFromTrack(null, new vec2(0, 0), 0);

            // Assert
            Assert.That(distance, Is.EqualTo(0).Within(0.001));
            Assert.That(sameway, Is.True);
        }

        [Test]
        public void GetDistanceFromTrack_WithInvalidTrack_ReturnsZero()
        {
            // Arrange
            var track = new Track("Invalid", TrackMode.AB);

            // Act
            var (distance, sameway) = _service.GetDistanceFromTrack(track, new vec2(0, 0), 0);

            // Assert
            Assert.That(distance, Is.EqualTo(0).Within(0.001));
            Assert.That(sameway, Is.True);
        }

        [Test]
        public void GetDistanceFromTrack_OnLine_ReturnsZeroDistance()
        {
            // Arrange
            var track = _service.CreateABTrack(
                new vec2(0, 0),
                new vec2(100, 0),
                0,
                "AB Line");

            var position = new vec2(50, 0); // On the line
            double heading = 0;

            // Act
            var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);

            // Assert
            Assert.That(distance, Is.EqualTo(0).Within(0.1), "Should be on the line");
            Assert.That(sameway, Is.True);
        }

        [Test]
        public void GetDistanceFromTrack_RightOfLine_ReturnsPositiveDistance()
        {
            // Arrange
            var track = _service.CreateABTrack(
                new vec2(0, 0),
                new vec2(100, 0),
                0,
                "AB Line");

            var position = new vec2(50, -10); // 10m to the right (south in UTM)
            double heading = 0;

            // Act
            var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);

            // Assert
            Assert.That(distance, Is.GreaterThan(0), "Should be positive (right of line)");
            Assert.That(Math.Abs(distance), Is.EqualTo(10).Within(1.0), "Should be ~10m from line");
        }

        [Test]
        public void GetDistanceFromTrack_LeftOfLine_ReturnsNegativeDistance()
        {
            // Arrange
            var track = _service.CreateABTrack(
                new vec2(0, 0),
                new vec2(100, 0),
                0,
                "AB Line");

            var position = new vec2(50, 10); // 10m to the left (north in UTM)
            double heading = 0;

            // Act
            var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);

            // Assert
            Assert.That(distance, Is.LessThan(0), "Should be negative (left of line)");
            Assert.That(Math.Abs(distance), Is.EqualTo(10).Within(1.0), "Should be ~10m from line");
        }

        // ============================================================
        // Helper Methods
        // ============================================================

        private List<vec3> CreateTestCurvePath(int numPoints)
        {
            var path = new List<vec3>(numPoints);
            for (int i = 0; i < numPoints; i++)
            {
                // Create a simple curved path
                double x = i * 10.0;
                double y = Math.Sin(i * 0.3) * 20.0;
                path.Add(new vec3(x, y, 0));
            }
            return path;
        }

        private List<vec3> CreateTestBoundaryPolygon(int numPoints)
        {
            var polygon = new List<vec3>(numPoints);
            double radius = 50.0;
            double angleStep = 2 * Math.PI / numPoints;

            for (int i = 0; i < numPoints; i++)
            {
                double angle = i * angleStep;
                double x = radius * Math.Cos(angle);
                double y = radius * Math.Sin(angle);
                polygon.Add(new vec3(x, y, 0));
            }

            return polygon;
        }
    }
}

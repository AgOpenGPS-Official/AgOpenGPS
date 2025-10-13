using NUnit.Framework;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;
using AgOpenGPS.Core.Extensions;
using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Tests.Models.Guidance
{
    [TestFixture]
    public class TrackTests
    {
        [Test]
        public void Constructor_Default_InitializesWithCorrectDefaults()
        {
            // Act
            var track = new Track();

            // Assert
            Assert.That(track.Id, Is.Not.EqualTo(Guid.Empty));
            Assert.That(track.Name, Is.EqualTo("New Track"));
            Assert.That(track.Mode, Is.EqualTo(TrackMode.None));
            Assert.That(track.IsVisible, Is.True);
            Assert.That(track.CurvePts, Is.Not.Null);
            Assert.That(track.CurvePts.Count, Is.EqualTo(0));
            Assert.That(track.Heading, Is.EqualTo(0));
            Assert.That(track.NudgeDistance, Is.EqualTo(0));
            Assert.That(track.WorkedTracks, Is.Not.Null);
            Assert.That(track.WorkedTracks.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_WithNameAndMode_InitializesCorrectly()
        {
            // Act
            var track = new Track("Test Track", TrackMode.AB);

            // Assert
            Assert.That(track.Name, Is.EqualTo("Test Track"));
            Assert.That(track.Mode, Is.EqualTo(TrackMode.AB));
            Assert.That(track.IsVisible, Is.True);
        }

        [Test]
        public void CurvePts_PreAllocatedCapacity_IsAtLeast500()
        {
            // Arrange & Act
            var track = new Track();

            // Assert - verify capacity is pre-allocated (no reallocations needed for 500 items)
            int initialCapacity = track.CurvePts.Capacity;
            Assert.That(initialCapacity, Is.GreaterThanOrEqualTo(500),
                "CurvePts should be pre-allocated with capacity >= 500 for performance");
        }

        [Test]
        public void Clone_CreatesDeepCopy_WithSameValues()
        {
            // Arrange
            var original = new Track("Original", TrackMode.Curve)
            {
                Heading = 1.57,
                NudgeDistance = 5.0,
                IsVisible = false,
                PtA = new vec2(100, 200),
                PtB = new vec2(300, 400)
            };
            original.CurvePts.Add(new vec3(10, 20, 0.5));
            original.CurvePts.Add(new vec3(30, 40, 1.0));
            original.WorkedTracks.Add(1);
            original.WorkedTracks.Add(2);

            // Act
            var clone = original.Clone();

            // Assert - verify all properties are copied
            Assert.That(clone.Id, Is.EqualTo(original.Id));
            Assert.That(clone.Name, Is.EqualTo(original.Name));
            Assert.That(clone.Mode, Is.EqualTo(original.Mode));
            Assert.That(clone.IsVisible, Is.EqualTo(original.IsVisible));
            Assert.That(clone.Heading, Is.EqualTo(original.Heading));
            Assert.That(clone.NudgeDistance, Is.EqualTo(original.NudgeDistance));
            Assert.That(clone.PtA.easting, Is.EqualTo(original.PtA.easting));
            Assert.That(clone.PtB.northing, Is.EqualTo(original.PtB.northing));
            Assert.That(clone.CurvePts.Count, Is.EqualTo(original.CurvePts.Count));
            Assert.That(clone.WorkedTracks.Count, Is.EqualTo(original.WorkedTracks.Count));
        }

        [Test]
        public void Clone_CreatesDeepCopy_IndependentLists()
        {
            // Arrange
            var original = new Track();
            original.CurvePts.Add(new vec3(10, 20, 0.5));

            // Act
            var clone = original.Clone();
            clone.CurvePts.Add(new vec3(30, 40, 1.0));  // Modify clone

            // Assert - original should not be affected
            Assert.That(original.CurvePts.Count, Is.EqualTo(1));
            Assert.That(clone.CurvePts.Count, Is.EqualTo(2));
        }

        [Test]
        public void Equals_SameTracks_ReturnsTrue()
        {
            // Arrange
            var track1 = new Track("Track1", TrackMode.AB)
            {
                Heading = 1.57,
                NudgeDistance = 5.0,
                PtA = new vec2(100, 200),
                PtB = new vec2(300, 400)
            };
            track1.CurvePts.Add(new vec3(10, 20, 0.5));

            var track2 = track1.Clone();

            // Act
            bool result = track1.Equals(track2);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Equals_DifferentNames_ReturnsFalse()
        {
            // Arrange
            var track1 = new Track("Track1", TrackMode.AB);
            var track2 = track1.Clone();
            track2.Name = "Track2";

            // Act
            bool result = track1.Equals(track2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_DifferentCurvePtsCounts_ReturnsFalse()
        {
            // Arrange
            var track1 = new Track();
            track1.CurvePts.Add(new vec3(10, 20, 0.5));

            var track2 = track1.Clone();
            track2.CurvePts.Add(new vec3(30, 40, 1.0));

            // Act
            bool result = track1.Equals(track2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_NullTrack_ReturnsFalse()
        {
            // Arrange
            var track = new Track();

            // Act
            bool result = track.Equals(null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Equals_SameReference_ReturnsTrue()
        {
            // Arrange
            var track = new Track();

            // Act
            bool result = track.Equals(track);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_ABMode_WithValidPoints_ReturnsTrue()
        {
            // Arrange
            var track = new Track("AB Line", TrackMode.AB)
            {
                PtA = new vec2(100, 200),
                PtB = new vec2(300, 400)
            };

            // Act
            bool result = track.IsValid();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_ABMode_WithDefaultPoints_ReturnsFalse()
        {
            // Arrange
            var track = new Track("AB Line", TrackMode.AB)
            {
                PtA = new vec2(0, 0),  // Default
                PtB = new vec2(0, 0)   // Default
            };

            // Act
            bool result = track.IsValid();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_CurveMode_WithPoints_ReturnsTrue()
        {
            // Arrange
            var track = new Track("Curve", TrackMode.Curve);
            track.CurvePts.Add(new vec3(10, 20, 0.5));
            track.CurvePts.Add(new vec3(30, 40, 1.0));

            // Act
            bool result = track.IsValid();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValid_CurveMode_WithOnePoint_ReturnsFalse()
        {
            // Arrange
            var track = new Track("Curve", TrackMode.Curve);
            track.CurvePts.Add(new vec3(10, 20, 0.5));

            // Act
            bool result = track.IsValid();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValid_NoneMode_ReturnsFalse()
        {
            // Arrange
            var track = new Track("No Mode", TrackMode.None);

            // Act
            bool result = track.IsValid();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void PointCount_EmptyTrack_ReturnsZero()
        {
            // Arrange
            var track = new Track();

            // Act
            int count = track.PointCount;

            // Assert
            Assert.That(count, Is.EqualTo(0));
        }

        [Test]
        public void PointCount_WithPoints_ReturnsCorrectCount()
        {
            // Arrange
            var track = new Track();
            track.CurvePts.Add(new vec3(10, 20, 0.5));
            track.CurvePts.Add(new vec3(30, 40, 1.0));
            track.CurvePts.Add(new vec3(50, 60, 1.5));

            // Act
            int count = track.PointCount;

            // Assert
            Assert.That(count, Is.EqualTo(3));
        }

        [Test]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var track = new Track("Test Track", TrackMode.AB);
            track.CurvePts.Add(new vec3(10, 20, 0.5));

            // Act
            string result = track.ToString();

            // Assert
            Assert.That(result, Does.Contain("Test Track"));
            Assert.That(result, Does.Contain("AB"));
            Assert.That(result, Does.Contain("1"));  // Point count
            Assert.That(result, Does.Contain("True"));  // IsVisible
        }

        [Test]
        public void WorkedTracks_CanAddAndContains()
        {
            // Arrange
            var track = new Track();

            // Act
            track.WorkedTracks.Add(1);
            track.WorkedTracks.Add(2);
            track.WorkedTracks.Add(3);

            // Assert
            Assert.That(track.WorkedTracks.Count, Is.EqualTo(3));
            Assert.That(track.WorkedTracks.Contains(2), Is.True);
            Assert.That(track.WorkedTracks.Contains(5), Is.False);
        }

        [Test]
        public void TrackMode_Enum_HasCorrectValues()
        {
            // Assert - verify enum values match specification
            Assert.That((int)TrackMode.None, Is.EqualTo(0));
            Assert.That((int)TrackMode.AB, Is.EqualTo(2));
            Assert.That((int)TrackMode.Curve, Is.EqualTo(4));
            Assert.That((int)TrackMode.BoundaryTrackOuter, Is.EqualTo(8));
            Assert.That((int)TrackMode.BoundaryTrackInner, Is.EqualTo(16));
            Assert.That((int)TrackMode.BoundaryCurve, Is.EqualTo(32));
            Assert.That((int)TrackMode.WaterPivot, Is.EqualTo(64));
        }

        [Test]
        public void Track_GuidId_IsUnique()
        {
            // Arrange & Act
            var track1 = new Track();
            var track2 = new Track();

            // Assert
            Assert.That(track1.Id, Is.Not.EqualTo(track2.Id));
        }
    }
}

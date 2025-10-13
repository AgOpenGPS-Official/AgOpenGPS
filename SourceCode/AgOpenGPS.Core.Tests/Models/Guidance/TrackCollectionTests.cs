using NUnit.Framework;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgOpenGPS.Core.Tests.Models.Guidance
{
    [TestFixture]
    public class TrackCollectionTests
    {
        private TrackCollection _collection;

        [SetUp]
        public void SetUp()
        {
            _collection = new TrackCollection();
        }

        [Test]
        public void Constructor_InitializesEmpty()
        {
            // Assert
            Assert.That(_collection.Count, Is.EqualTo(0));
            Assert.That(_collection.CurrentTrack, Is.Null);
            Assert.That(_collection.CurrentIndex, Is.EqualTo(-1));
        }

        [Test]
        public void Add_ValidTrack_AddsSuccessfully()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);

            // Act
            _collection.Add(track);

            // Assert
            Assert.That(_collection.Count, Is.EqualTo(1));
            Assert.That(_collection.Tracks[0], Is.EqualTo(track));
        }

        [Test]
        public void Add_NullTrack_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _collection.Add(null));
        }

        [Test]
        public void Add_MultipleTracks_MaintainsOrder()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter);

            // Act
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Assert
            Assert.That(_collection.Count, Is.EqualTo(3));
            Assert.That(_collection.Tracks[0].Name, Is.EqualTo("Track 1"));
            Assert.That(_collection.Tracks[1].Name, Is.EqualTo("Track 2"));
            Assert.That(_collection.Tracks[2].Name, Is.EqualTo("Track 3"));
        }

        [Test]
        public void Remove_ExistingTrack_RemovesSuccessfully()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act
            bool result = _collection.Remove(track);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_collection.Count, Is.EqualTo(0));
        }

        [Test]
        public void Remove_NonExistingTrack_ReturnsFalse()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);

            // Act
            bool result = _collection.Remove(track);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Remove_CurrentTrack_ClearsCurrentTrack()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);
            _collection.CurrentTrack = track;

            // Act
            _collection.Remove(track);

            // Assert
            Assert.That(_collection.CurrentTrack, Is.Null);
        }

        [Test]
        public void RemoveAt_ValidIndex_RemovesTrack()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            _collection.RemoveAt(0);

            // Assert
            Assert.That(_collection.Count, Is.EqualTo(1));
            Assert.That(_collection.Tracks[0].Name, Is.EqualTo("Track 2"));
        }

        [Test]
        public void RemoveAt_InvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.RemoveAt(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.RemoveAt(-1));
        }

        [Test]
        public void Clear_RemovesAllTracks()
        {
            // Arrange
            _collection.Add(new Track("Track 1", TrackMode.AB));
            _collection.Add(new Track("Track 2", TrackMode.Curve));
            _collection.CurrentTrack = _collection.Tracks[0];

            // Act
            _collection.Clear();

            // Assert
            Assert.That(_collection.Count, Is.EqualTo(0));
            Assert.That(_collection.CurrentTrack, Is.Null);
        }

        [Test]
        public void MoveUp_MiddleTrack_MovesUp()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter);
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            bool result = _collection.MoveUp(track2);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_collection.Tracks[0], Is.EqualTo(track2));
            Assert.That(_collection.Tracks[1], Is.EqualTo(track1));
            Assert.That(_collection.Tracks[2], Is.EqualTo(track3));
        }

        [Test]
        public void MoveUp_FirstTrack_ReturnsFalse()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            bool result = _collection.MoveUp(track1);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_collection.Tracks[0], Is.EqualTo(track1));  // Unchanged
        }

        [Test]
        public void MoveDown_MiddleTrack_MovesDown()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter);
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            bool result = _collection.MoveDown(track2);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_collection.Tracks[0], Is.EqualTo(track1));
            Assert.That(_collection.Tracks[1], Is.EqualTo(track3));
            Assert.That(_collection.Tracks[2], Is.EqualTo(track2));
        }

        [Test]
        public void MoveDown_LastTrack_ReturnsFalse()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            bool result = _collection.MoveDown(track2);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_collection.Tracks[1], Is.EqualTo(track2));  // Unchanged
        }

        [Test]
        public void GetNext_Forward_ReturnsNextTrack()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter);
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            var next = _collection.GetNext(track1, forward: true);

            // Assert
            Assert.That(next, Is.EqualTo(track2));
        }

        [Test]
        public void GetNext_Backward_ReturnsPreviousTrack()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter);
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            var previous = _collection.GetNext(track2, forward: false);

            // Assert
            Assert.That(previous, Is.EqualTo(track1));
        }

        [Test]
        public void GetNext_LastTrackForward_WrapsToFirst()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            var next = _collection.GetNext(track2, forward: true);

            // Assert
            Assert.That(next, Is.EqualTo(track1));  // Wraps around
        }

        [Test]
        public void GetNext_FirstTrackBackward_WrapsToLast()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            var previous = _collection.GetNext(track1, forward: false);

            // Assert
            Assert.That(previous, Is.EqualTo(track2));  // Wraps around
        }

        [Test]
        public void GetVisibleCount_ReturnsCorrectCount()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB) { IsVisible = true };
            var track2 = new Track("Track 2", TrackMode.Curve) { IsVisible = false };
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter) { IsVisible = true };
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            int visibleCount = _collection.GetVisibleCount();

            // Assert
            Assert.That(visibleCount, Is.EqualTo(2));
        }

        [Test]
        public void FindById_ExistingId_ReturnsTrack()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act
            var found = _collection.FindById(track.Id);

            // Assert
            Assert.That(found, Is.EqualTo(track));
        }

        [Test]
        public void FindById_NonExistingId_ReturnsNull()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act
            var found = _collection.FindById(Guid.NewGuid());

            // Assert
            Assert.That(found, Is.Null);
        }

        [Test]
        public void FindByName_ExistingName_ReturnsTrack()
        {
            // Arrange
            var track = new Track("My Special Track", TrackMode.AB);
            _collection.Add(track);

            // Act
            var found = _collection.FindByName("My Special Track");

            // Assert
            Assert.That(found, Is.EqualTo(track));
        }

        [Test]
        public void FindByName_CaseInsensitive_ReturnsTrack()
        {
            // Arrange
            var track = new Track("My Special Track", TrackMode.AB);
            _collection.Add(track);

            // Act
            var found = _collection.FindByName("my special track");

            // Assert
            Assert.That(found, Is.EqualTo(track));
        }

        [Test]
        public void GetTracksByMode_ReturnsMatchingTracks()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            var track3 = new Track("Track 3", TrackMode.AB);
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            var abTracks = _collection.GetTracksByMode(TrackMode.AB);

            // Assert
            Assert.That(abTracks.Count, Is.EqualTo(2));
            Assert.That(abTracks[0], Is.EqualTo(track1));
            Assert.That(abTracks[1], Is.EqualTo(track3));
        }

        [Test]
        public void GetVisibleTracks_ReturnsOnlyVisibleTracks()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB) { IsVisible = true };
            var track2 = new Track("Track 2", TrackMode.Curve) { IsVisible = false };
            var track3 = new Track("Track 3", TrackMode.BoundaryTrackOuter) { IsVisible = true };
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.Add(track3);

            // Act
            var visibleTracks = _collection.GetVisibleTracks();

            // Assert
            Assert.That(visibleTracks.Count, Is.EqualTo(2));
            Assert.That(visibleTracks[0], Is.EqualTo(track1));
            Assert.That(visibleTracks[1], Is.EqualTo(track3));
        }

        [Test]
        public void SetCurrentByIndex_ValidIndex_SetsCurrentTrack()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            bool result = _collection.SetCurrentByIndex(1);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(_collection.CurrentTrack, Is.EqualTo(track2));
            Assert.That(_collection.CurrentIndex, Is.EqualTo(1));
        }

        [Test]
        public void SetCurrentByIndex_InvalidIndex_ReturnsFalse()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act
            bool result = _collection.SetCurrentByIndex(5);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(_collection.CurrentTrack, Is.Null);
        }

        [Test]
        public void GetByIndex_ValidIndex_ReturnsTrack()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            var track = _collection.GetByIndex(1);

            // Assert
            Assert.That(track, Is.EqualTo(track2));
        }

        [Test]
        public void GetByIndex_InvalidIndex_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => _collection.GetByIndex(5));
        }

        [Test]
        public void Contains_ExistingTrack_ReturnsTrue()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act
            bool result = _collection.Contains(track);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Contains_NonExistingTrack_ReturnsFalse()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);

            // Act
            bool result = _collection.Contains(track2);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IndexOf_ExistingTrack_ReturnsCorrectIndex()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);
            _collection.Add(track2);

            // Act
            int index = _collection.IndexOf(track2);

            // Assert
            Assert.That(index, Is.EqualTo(1));
        }

        [Test]
        public void IndexOf_NonExistingTrack_ReturnsMinusOne()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB);
            var track2 = new Track("Track 2", TrackMode.Curve);
            _collection.Add(track1);

            // Act
            int index = _collection.IndexOf(track2);

            // Assert
            Assert.That(index, Is.EqualTo(-1));
        }

        [Test]
        public void ToString_ReturnsFormattedString()
        {
            // Arrange
            var track1 = new Track("Track 1", TrackMode.AB) { IsVisible = true };
            var track2 = new Track("Track 2", TrackMode.Curve) { IsVisible = false };
            _collection.Add(track1);
            _collection.Add(track2);
            _collection.CurrentTrack = track1;

            // Act
            string result = _collection.ToString();

            // Assert
            Assert.That(result, Does.Contain("2 tracks"));
            Assert.That(result, Does.Contain("1 visible"));
            Assert.That(result, Does.Contain("Track 1"));
        }

        [Test]
        public void Tracks_ReturnsReadOnlyList()
        {
            // Arrange
            var track = new Track("Track 1", TrackMode.AB);
            _collection.Add(track);

            // Act
            var tracks = _collection.Tracks;

            // Assert
            Assert.That(tracks, Is.InstanceOf<IReadOnlyList<Track>>());
            Assert.That(tracks.Count, Is.EqualTo(1));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AgOpenGPS.Core.Models.Guidance
{
    /// <summary>
    /// Manages a collection of guidance tracks with navigation and organization capabilities.
    /// Based on CTrack.gArr from AgOpenGPS with improvements for clean architecture.
    /// </summary>
    public class TrackCollection
    {
        private readonly List<Track> _tracks;

        /// <summary>
        /// Gets a read-only view of all tracks in the collection.
        /// </summary>
        public IReadOnlyList<Track> Tracks => _tracks.AsReadOnly();

        /// <summary>
        /// Gets or sets the currently active track for guidance.
        /// </summary>
        public Track CurrentTrack { get; set; }

        /// <summary>
        /// Gets the index of the current track, or -1 if no current track.
        /// </summary>
        public int CurrentIndex => CurrentTrack != null ? _tracks.IndexOf(CurrentTrack) : -1;

        /// <summary>
        /// Gets the total number of tracks in the collection.
        /// </summary>
        public int Count => _tracks.Count;

        /// <summary>
        /// Creates a new empty track collection with pre-allocated capacity.
        /// </summary>
        public TrackCollection()
        {
            _tracks = new List<Track>(capacity: 20);  // Pre-allocate for performance
            CurrentTrack = null;
        }

        /// <summary>
        /// Adds a track to the collection.
        /// </summary>
        /// <param name="track">The track to add. Cannot be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when track is null.</exception>
        public void Add(Track track)
        {
            if (track == null)
                throw new ArgumentNullException(nameof(track));

            _tracks.Add(track);
        }

        /// <summary>
        /// Removes a track from the collection.
        /// If the removed track was current, CurrentTrack is set to null.
        /// </summary>
        /// <param name="track">The track to remove.</param>
        /// <returns>True if the track was removed, false if not found.</returns>
        public bool Remove(Track track)
        {
            if (track == null)
                return false;

            bool removed = _tracks.Remove(track);

            if (removed && CurrentTrack == track)
            {
                CurrentTrack = null;
            }

            return removed;
        }

        /// <summary>
        /// Removes a track at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the track to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
        public void RemoveAt(int index)
        {
            if (index < 0 || index >= _tracks.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            Track track = _tracks[index];
            _tracks.RemoveAt(index);

            if (CurrentTrack == track)
            {
                CurrentTrack = null;
            }
        }

        /// <summary>
        /// Removes all tracks from the collection and clears the current track.
        /// </summary>
        public void Clear()
        {
            _tracks.Clear();
            CurrentTrack = null;
        }

        /// <summary>
        /// Moves a track up in the collection (decreases index by 1).
        /// </summary>
        /// <param name="track">The track to move up.</param>
        /// <returns>True if moved successfully, false if already at top or not found.</returns>
        public bool MoveUp(Track track)
        {
            if (track == null)
                return false;

            int index = _tracks.IndexOf(track);
            if (index <= 0)  // Already at top or not found
                return false;

            _tracks.RemoveAt(index);
            _tracks.Insert(index - 1, track);
            return true;
        }

        /// <summary>
        /// Moves a track down in the collection (increases index by 1).
        /// </summary>
        /// <param name="track">The track to move down.</param>
        /// <returns>True if moved successfully, false if already at bottom or not found.</returns>
        public bool MoveDown(Track track)
        {
            if (track == null)
                return false;

            int index = _tracks.IndexOf(track);
            if (index < 0 || index >= _tracks.Count - 1)  // Not found or already at bottom
                return false;

            _tracks.RemoveAt(index);
            _tracks.Insert(index + 1, track);
            return true;
        }

        /// <summary>
        /// Gets the next track in the collection relative to the current track.
        /// Wraps around at the ends.
        /// </summary>
        /// <param name="current">The current track reference.</param>
        /// <param name="forward">True to get next track, false to get previous.</param>
        /// <returns>The next/previous track, or null if collection is empty or current not found.</returns>
        public Track GetNext(Track current, bool forward = true)
        {
            if (_tracks.Count == 0 || current == null)
                return null;

            int index = _tracks.IndexOf(current);
            if (index < 0)
                return null;

            if (forward)
            {
                index = (index + 1) % _tracks.Count;  // Wrap around
            }
            else
            {
                index = index - 1;
                if (index < 0)
                    index = _tracks.Count - 1;  // Wrap around
            }

            return _tracks[index];
        }

        /// <summary>
        /// Gets the number of visible tracks in the collection.
        /// </summary>
        /// <returns>Count of tracks where IsVisible is true.</returns>
        public int GetVisibleCount()
        {
            return _tracks.Count(t => t.IsVisible);
        }

        /// <summary>
        /// Finds a track by its unique identifier.
        /// </summary>
        /// <param name="id">The GUID of the track to find.</param>
        /// <returns>The track with the specified ID, or null if not found.</returns>
        public Track FindById(Guid id)
        {
            return _tracks.FirstOrDefault(t => t.Id == id);
        }

        /// <summary>
        /// Finds a track by its name (case-insensitive).
        /// </summary>
        /// <param name="name">The name of the track to find.</param>
        /// <returns>The first track with the specified name, or null if not found.</returns>
        public Track FindByName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            return _tracks.FirstOrDefault(t =>
                string.Equals(t.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets all tracks of a specific mode.
        /// </summary>
        /// <param name="mode">The track mode to filter by.</param>
        /// <returns>List of tracks matching the specified mode.</returns>
        public List<Track> GetTracksByMode(TrackMode mode)
        {
            return _tracks.Where(t => t.Mode == mode).ToList();
        }

        /// <summary>
        /// Gets all visible tracks.
        /// </summary>
        /// <returns>List of visible tracks.</returns>
        public List<Track> GetVisibleTracks()
        {
            return _tracks.Where(t => t.IsVisible).ToList();
        }

        /// <summary>
        /// Sets the current track by index.
        /// </summary>
        /// <param name="index">The zero-based index of the track.</param>
        /// <returns>True if set successfully, false if index out of range.</returns>
        public bool SetCurrentByIndex(int index)
        {
            if (index < 0 || index >= _tracks.Count)
            {
                CurrentTrack = null;
                return false;
            }

            CurrentTrack = _tracks[index];
            return true;
        }

        /// <summary>
        /// Gets a track by its index.
        /// </summary>
        /// <param name="index">The zero-based index.</param>
        /// <returns>The track at the specified index.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when index is out of range.</exception>
        public Track GetByIndex(int index)
        {
            if (index < 0 || index >= _tracks.Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            return _tracks[index];
        }

        /// <summary>
        /// Checks if a track exists in the collection.
        /// </summary>
        /// <param name="track">The track to check.</param>
        /// <returns>True if the track exists in the collection.</returns>
        public bool Contains(Track track)
        {
            return track != null && _tracks.Contains(track);
        }

        /// <summary>
        /// Gets the index of a track in the collection.
        /// </summary>
        /// <param name="track">The track to find.</param>
        /// <returns>The zero-based index, or -1 if not found.</returns>
        public int IndexOf(Track track)
        {
            return track != null ? _tracks.IndexOf(track) : -1;
        }

        public override string ToString()
        {
            return $"TrackCollection: {Count} tracks, {GetVisibleCount()} visible, Current: {CurrentTrack?.Name ?? "None"}";
        }
    }
}

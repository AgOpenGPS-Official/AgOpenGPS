using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Extensions;

namespace AgOpenGPS.Core.Models.Guidance
{
    /// <summary>
    /// Represents a guidance track (AB line, curve, or boundary track) without UI dependencies.
    /// Based on CTrk from AgOpenGPS with improvements from AOG_Dev.
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Unique identifier for this track.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Display name of the track.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The type/mode of this track (AB, Curve, Boundary, etc.).
        /// </summary>
        public TrackMode Mode { get; set; }

        /// <summary>
        /// Whether this track is currently visible.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Curve points defining the track path (easting, northing, heading).
        /// Pre-allocated for performance.
        /// </summary>
        public List<vec3> CurvePts { get; set; }

        /// <summary>
        /// Point A for AB line mode (field coordinates).
        /// </summary>
        public vec2 PtA { get; set; }

        /// <summary>
        /// Point B for AB line mode (field coordinates).
        /// </summary>
        public vec2 PtB { get; set; }

        /// <summary>
        /// Heading of the AB line (radians).
        /// </summary>
        public double Heading { get; set; }

        /// <summary>
        /// Current nudge distance offset (meters).
        /// </summary>
        public double NudgeDistance { get; set; }

        /// <summary>
        /// Set of track IDs that have been worked (for multi-track operations).
        /// </summary>
        public HashSet<int> WorkedTracks { get; set; }

        /// <summary>
        /// Creates a new Track with default values.
        /// </summary>
        public Track()
        {
            Id = Guid.NewGuid();
            Name = "New Track";
            Mode = TrackMode.None;
            IsVisible = true;
            CurvePts = new List<vec3>(capacity: 500);  // Pre-allocate for performance
            PtA = new vec2();
            PtB = new vec2();
            Heading = 0;
            NudgeDistance = 0;
            WorkedTracks = new HashSet<int>();
        }

        /// <summary>
        /// Creates a new Track with the specified name and mode.
        /// </summary>
        public Track(string name, TrackMode mode) : this()
        {
            Name = name;
            Mode = mode;
        }

        /// <summary>
        /// Creates a deep copy of this track.
        /// </summary>
        public Track Clone()
        {
            var clone = new Track
            {
                Id = this.Id,  // Keep same ID for cloning
                Name = this.Name,
                Mode = this.Mode,
                IsVisible = this.IsVisible,
                CurvePts = new List<vec3>(this.CurvePts),  // Deep copy of curve points
                PtA = this.PtA,  // vec2 is struct, value copy
                PtB = this.PtB,  // vec2 is struct, value copy
                Heading = this.Heading,
                NudgeDistance = this.NudgeDistance,
                WorkedTracks = new HashSet<int>(this.WorkedTracks)  // Deep copy of worked tracks
            };

            return clone;
        }

        /// <summary>
        /// Determines if this track is equal to another track based on all properties.
        /// </summary>
        public bool Equals(Track other)
        {
            if (other == null) return false;
            if (ReferenceEquals(this, other)) return true;

            // Compare basic properties
            if (Id != other.Id) return false;
            if (Name != other.Name) return false;
            if (Mode != other.Mode) return false;
            if (IsVisible != other.IsVisible) return false;
            if (Math.Abs(Heading - other.Heading) > 0.0001) return false;
            if (Math.Abs(NudgeDistance - other.NudgeDistance) > 0.0001) return false;

            // Compare vec2 properties
            if (!PtA.Equals(other.PtA)) return false;
            if (!PtB.Equals(other.PtB)) return false;

            // Compare curve points
            if (CurvePts.Count != other.CurvePts.Count) return false;
            for (int i = 0; i < CurvePts.Count; i++)
            {
                if (!CurvePts[i].Equals(other.CurvePts[i])) return false;
            }

            // Compare worked tracks
            if (WorkedTracks.Count != other.WorkedTracks.Count) return false;
            if (!WorkedTracks.SetEquals(other.WorkedTracks)) return false;

            return true;
        }

        /// <summary>
        /// Determines if this track has valid geometry for guidance.
        /// </summary>
        public bool IsValid()
        {
            if (Mode == TrackMode.None) return false;

            if (Mode == TrackMode.AB)
            {
                // AB line needs two different points AND populated curve points
                double dx = PtB.easting - PtA.easting;
                double dy = PtB.northing - PtA.northing;
                double distSq = dx * dx + dy * dy;
                return distSq > 0.01 && CurvePts != null && CurvePts.Count >= 2;
            }
            else
            {
                // Curves need at least 2 points
                return CurvePts != null && CurvePts.Count >= 2;
            }
        }

        /// <summary>
        /// Gets the number of curve points in this track.
        /// </summary>
        public int PointCount => CurvePts?.Count ?? 0;

        public override string ToString()
        {
            return $"Track: {Name} ({Mode}), Points: {PointCount}, Visible: {IsVisible}";
        }
    }

    /// <summary>
    /// Defines the type/mode of a guidance track.
    /// </summary>
    public enum TrackMode
    {
        /// <summary>No track mode selected.</summary>
        None = 0,

        /// <summary>Straight AB line guidance.</summary>
        AB = 2,

        /// <summary>Recorded curve guidance.</summary>
        Curve = 4,

        /// <summary>Boundary track (outer).</summary>
        BoundaryTrackOuter = 8,

        /// <summary>Boundary track (inner).</summary>
        BoundaryTrackInner = 16,

        /// <summary>Boundary curve.</summary>
        BoundaryCurve = 32,

        /// <summary>Water pivot irrigation.</summary>
        WaterPivot = 64
    }
}

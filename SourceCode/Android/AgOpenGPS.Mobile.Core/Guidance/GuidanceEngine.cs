using System;
using System.Collections.Generic;

namespace AgOpenGPS.Mobile.Core.Guidance
{
    /// <summary>
    /// Works out the vehicle heading from what the receiver gives: dual antenna heading when there
    /// is one, else course over ground while moving, else the direction between positions at least
    /// <see cref="MinMoveMeters"/> apart. Keeps the last heading while standing still.
    /// </summary>
    public sealed class HeadingTracker
    {
        public const double MinSpeedKmh = 1.5;
        public const double MinMoveMeters = 0.5;

        private LocalPlane _plane;
        private GeoPoint? _lastPoint;

        public double Heading { get; private set; } = double.NaN;

        public void Update(GnssFix fix)
        {
            if (fix == null || !fix.HasPosition) return;
            if (_plane == null) _plane = new LocalPlane(fix.Latitude, fix.Longitude);
            GeoPoint p = _plane.ToLocal(fix.Latitude, fix.Longitude);

            if (!double.IsNaN(fix.HeadingDual))
            {
                Heading = LocalPlane.Wrap360(fix.HeadingDual);
                _lastPoint = p;
                return;
            }

            if (!double.IsNaN(fix.HeadingTrue) && !double.IsNaN(fix.SpeedKmh) && fix.SpeedKmh >= MinSpeedKmh)
            {
                Heading = LocalPlane.Wrap360(fix.HeadingTrue);
                _lastPoint = p;
                return;
            }

            if (_lastPoint == null)
            {
                _lastPoint = p;
                return;
            }

            GeoPoint last = _lastPoint.Value;
            if (last.DistanceTo(p) >= MinMoveMeters)
            {
                Heading = LocalPlane.Wrap360(LocalPlane.ToDeg(Math.Atan2(p.Easting - last.Easting, p.Northing - last.Northing)));
                _lastPoint = p;
            }
        }

        public void Reset()
        {
            _plane = null;
            _lastPoint = null;
            Heading = double.NaN;
        }
    }

    /// <summary>A pass to draw: its number and its points on the snapshot's draw plane.</summary>
    public sealed class PassShape
    {
        public int Pass;
        public List<GeoPoint> Points;
    }

    /// <summary>A snapshot for the UI.</summary>
    public sealed class GuidanceSnapshot
    {
        public GnssFix Fix;
        public double VehicleHeading = double.NaN;
        public AbLine ActiveLine;
        /// <summary>Null without an active line or position.</summary>
        public AbGuidanceResult Result;
        public double PassWidth;

        /// <summary>True after A was set and before B.</summary>
        public bool Recording;
        public LineKind RecordingKind;
        /// <summary>Distance driven from A (along the path for a curve), metres.</summary>
        public double RecordedMeters = double.NaN;

        // Everything below is on one plane, for drawing. Null/empty when there is nothing to draw.
        public GeoPoint? Vehicle;
        public List<PassShape> Passes = new List<PassShape>();
        public List<GeoPoint> RecordingPath = new List<GeoPoint>();
    }

    /// <summary>
    /// Ties positions to the active line and records new lines. Fed from the link's reader thread
    /// (<see cref="OnFix"/>), read by the UI (<see cref="GetSnapshot"/>). Thread safe.
    /// </summary>
    public sealed class GuidanceEngine
    {
        /// <summary>A and B must be at least this far apart for a usable heading.</summary>
        public const double MinAbLengthMeters = 5.0;
        /// <summary>A curve point is recorded every this many metres.</summary>
        public const double CurveSpacingMeters = 1.0;
        /// <summary>Recorded curve points closer than this to the simplified line are dropped.</summary>
        public const double CurveToleranceMeters = 0.03;
        /// <summary>Passes drawn on each side of the current one.</summary>
        public const int DrawPassesEachSide = 3;
        private const int MaxCurvePoints = 50000;

        private readonly object _lock = new object();
        private readonly HeadingTracker _heading = new HeadingTracker();
        private GnssFix _fix;
        private AbLine _line;
        private LocalPlane _plane;
        private double _passWidth = 6.0;

        private bool _recording;
        private LineKind _recordKind;
        private readonly List<TrackPoint> _recorded = new List<TrackPoint>();
        private LocalPlane _recordPlane;
        private GeoPoint _lastRecorded;
        private double _recordedLength;

        public void OnFix(GnssFix fix)
        {
            if (fix == null) return;
            lock (_lock)
            {
                _fix = fix;
                _heading.Update(fix);
                if (_recording && _recordKind == LineKind.Curve && fix.HasPosition && _recorded.Count < MaxCurvePoints)
                {
                    GeoPoint p = _recordPlane.ToLocal(fix.Latitude, fix.Longitude);
                    double step = p.DistanceTo(_lastRecorded);
                    if (step >= CurveSpacingMeters)
                    {
                        _recorded.Add(new TrackPoint(fix.Latitude, fix.Longitude));
                        _recordedLength += step;
                        _lastRecorded = p;
                    }
                }
            }
        }

        public double PassWidth
        {
            get { lock (_lock) return _passWidth; }
            set { lock (_lock) _passWidth = Math.Max(0, value); }
        }

        public AbLine ActiveLine
        {
            get { lock (_lock) return _line; }
        }

        public bool IsRecording
        {
            get { lock (_lock) return _recording; }
        }

        public void SetActiveLine(AbLine line)
        {
            lock (_lock)
            {
                _line = line;
                _plane = line?.CreatePlane();
            }
        }

        /// <summary>Marks point A at the current position and starts recording. False without a position.</summary>
        public bool SetA(LineKind kind = LineKind.Straight)
        {
            lock (_lock)
            {
                if (_fix == null || !_fix.HasPosition) return false;
                _recording = true;
                _recordKind = kind;
                _recorded.Clear();
                _recorded.Add(new TrackPoint(_fix.Latitude, _fix.Longitude));
                _recordPlane = new LocalPlane(_fix.Latitude, _fix.Longitude);
                _lastRecorded = new GeoPoint(0, 0);
                _recordedLength = 0;
                return true;
            }
        }

        public void CancelAb()
        {
            lock (_lock)
            {
                _recording = false;
                _recorded.Clear();
            }
        }

        /// <summary>
        /// Marks point B at the current position and returns the new line (not yet stored or
        /// activated), or null with a reason. Recording continues after a failure.
        /// </summary>
        public AbLine SetB(string name, out string error)
        {
            lock (_lock)
            {
                error = null;
                if (!_recording) { error = "Set point A first"; return null; }
                if (_fix == null || !_fix.HasPosition) { error = "No position"; return null; }

                TrackPoint a = _recorded[0];
                var line = new AbLine
                {
                    Name = name,
                    Kind = _recordKind,
                    LatitudeA = a.Latitude,
                    LongitudeA = a.Longitude,
                    LatitudeB = _fix.Latitude,
                    LongitudeB = _fix.Longitude,
                };

                if (_recordKind == LineKind.Curve)
                {
                    var path = new List<TrackPoint>(_recorded);
                    TrackPoint lastPt = path[path.Count - 1];
                    GeoPoint end = _recordPlane.ToLocal(_fix.Latitude, _fix.Longitude);
                    if (_recordPlane.ToLocal(lastPt.Latitude, lastPt.Longitude).DistanceTo(end) > 0.05)
                        path.Add(new TrackPoint(_fix.Latitude, _fix.Longitude));

                    var local = new List<GeoPoint>(path.Count);
                    foreach (TrackPoint t in path) local.Add(_recordPlane.ToLocal(t.Latitude, t.Longitude));
                    var kept = new List<TrackPoint>();
                    foreach (int i in AbGuidance.Simplify(local, CurveToleranceMeters)) kept.Add(path[i]);
                    line.Points = kept;

                    if (kept.Count < 2 || line.LengthMeters < MinAbLengthMeters)
                    {
                        error = "Drive at least " + MinAbLengthMeters + " m from A before setting B";
                        return null;
                    }
                }
                else if (line.LengthMeters < MinAbLengthMeters)
                {
                    error = "Drive at least " + MinAbLengthMeters + " m from A before setting B";
                    return null;
                }

                _recording = false;
                _recorded.Clear();
                return line;
            }
        }

        public GuidanceSnapshot GetSnapshot()
        {
            lock (_lock)
            {
                var s = new GuidanceSnapshot
                {
                    Fix = _fix?.Clone(),
                    VehicleHeading = _heading.Heading,
                    ActiveLine = _line,
                    PassWidth = _passWidth,
                    Recording = _recording,
                    RecordingKind = _recordKind,
                };

                bool hasPos = _fix != null && _fix.HasPosition;
                LocalPlane draw = _plane ?? _recordPlane ?? (hasPos ? new LocalPlane(_fix.Latitude, _fix.Longitude) : null);
                if (draw == null) return s;

                if (hasPos)
                {
                    GeoPoint v = draw.ToLocal(_fix.Latitude, _fix.Longitude);
                    s.Vehicle = v;

                    if (_line != null)
                    {
                        s.Result = AbGuidance.Compute(_line, _plane, v, _heading.Heading, _passWidth);
                        int from = s.Result.Pass - DrawPassesEachSide, to = s.Result.Pass + DrawPassesEachSide;
                        for (int pass = from; pass <= to; pass++)
                            s.Passes.Add(new PassShape { Pass = pass, Points = _line.PassPolyline(_plane, pass, _passWidth) });
                    }
                }
                else if (_line != null)
                {
                    s.Passes.Add(new PassShape { Pass = 0, Points = _line.PassPolyline(_plane, 0, _passWidth) });
                }

                if (_recording)
                {
                    foreach (TrackPoint t in _recorded) s.RecordingPath.Add(draw.ToLocal(t.Latitude, t.Longitude));
                    if (hasPos) s.RecordingPath.Add(draw.ToLocal(_fix.Latitude, _fix.Longitude));
                    if (_recordKind == LineKind.Curve)
                    {
                        s.RecordedMeters = _recordedLength;
                    }
                    else if (hasPos)
                    {
                        TrackPoint a = _recorded[0];
                        s.RecordedMeters = _recordPlane.ToLocal(_fix.Latitude, _fix.Longitude).DistanceTo(_recordPlane.ToLocal(a.Latitude, a.Longitude));
                        // A straight line only needs A and the current point.
                        s.RecordingPath = new List<GeoPoint> { draw.ToLocal(a.Latitude, a.Longitude), draw.ToLocal(_fix.Latitude, _fix.Longitude) };
                    }
                }
                return s;
            }
        }
    }
}

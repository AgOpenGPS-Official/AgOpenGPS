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
        /// <summary>Goal point and steer angle (lightbar); null without a line, position or heading.</summary>
        public SteerResult Steer;
        public double PassWidth;

        /// <summary>True after A was set and before B.</summary>
        public bool Recording;
        public LineKind RecordingKind;
        /// <summary>Distance driven from A (along the path for a curve), metres.</summary>
        public double RecordedMeters = double.NaN;

        /// <summary>The open field, or null.</summary>
        public Field ActiveField;
        /// <summary>Inside the field boundary; null without a boundary or position.</summary>
        public bool? InsideField;
        /// <summary>Distance from the antenna to the nearest boundary edge, metres; NaN without one.</summary>
        public double DistanceToBoundary = double.NaN;

        /// <summary>Implement working (coverage is painted).</summary>
        public bool Working;
        /// <summary>Worked area, counted once, square metres.</summary>
        public double WorkedSquareMeters;
        /// <summary>Area worked more than once, square metres.</summary>
        public double OverlapSquareMeters;
        /// <summary>Worked area inside the field boundary; NaN without a boundary.</summary>
        public double WorkedInsideSquareMeters = double.NaN;
        /// <summary>Area inside the field boundary; NaN without a boundary.</summary>
        public double FieldSquareMeters = double.NaN;
        /// <summary>The coverage map for drawing (it has its own plane), or null.</summary>
        public CoverageMap Coverage;

        /// <summary>True while a boundary is being driven.</summary>
        public bool RecordingBoundary;
        public BoundarySide BoundarySide;
        public BoundaryMode BoundaryMode;
        /// <summary>Corners marked so far (corner mode).</summary>
        public int BoundaryCorners;
        public double BoundaryRecordedMeters = double.NaN;
        /// <summary>Distance back to where the boundary recording started, metres.</summary>
        public double BoundaryGapMeters = double.NaN;

        // Everything below is on one plane, for drawing. Null/empty when there is nothing to draw.
        public LocalPlane DrawPlane;
        public GeoPoint? Vehicle;
        public List<PassShape> Passes = new List<PassShape>();
        public List<GeoPoint> RecordingPath = new List<GeoPoint>();
        public List<GeoPoint> Boundary = new List<GeoPoint>();
        public List<GeoPoint> BoundaryPath = new List<GeoPoint>();
        /// <summary>Points A and B of the active line (draw plane), or null.</summary>
        public GeoPoint? LineA, LineB;
        /// <summary>Inner headland line (draw plane); empty without headland or boundary.</summary>
        public List<GeoPoint> HeadlandLine = new List<GeoPoint>();
        /// <summary>Headland width in use, metres (0 = off).</summary>
        public double HeadlandWidth;
        /// <summary>The implement is in the headland strip; null without boundary or headland.</summary>
        public bool? InHeadland;
        /// <summary>Distance from the implement to the headland line, metres, positive in the main part.</summary>
        public double DistanceToHeadland = double.NaN;
        /// <summary>Work is off because the implement is in the headland (switches on again in the main part).</summary>
        public bool WorkPausedInHeadland;
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
        private GnssFix _fix;           // the guide point: implement centre line, level with the rear axle
        private GnssFix _antennaFix;    // as received
        private VehicleGeometry _geometry = new VehicleGeometry();
        private AbLine _line;
        private LocalPlane _plane;
        private double _passWidth = 6.0;

        private bool _recording;
        private LineKind _recordKind;
        private readonly List<TrackPoint> _recorded = new List<TrackPoint>();
        private LocalPlane _recordPlane;
        private GeoPoint _lastRecorded;
        private double _recordedLength;

        private Field _field;
        private LocalPlane _fieldPlane;
        private List<GeoPoint> _fieldPoly;

        private CoverageMap _coverage;
        private bool _working;
        private bool _coverageEnabled = true;
        private double _toolWidth = 6.0;
        private GeoPoint? _prevLeft, _prevRight, _prevCentre;
        /// <summary>Positions further apart than this are a jump (lost signal), not a strip to paint.</summary>
        public const double MaxPaintStepMeters = 10.0;

        private double _headlandWidth;
        private bool _autoHeadland;
        private bool _pausedInHeadland;
        private bool? _toolInHeadland;
        private double _toolToHeadland = double.NaN;
        /// <summary>Metres the implement must go past the headland line before Work switches (against flicker).</summary>
        public const double HeadlandHysteresisMeters = 0.5;

        private bool _boundaryRecording;
        private BoundaryMode _boundaryMode;
        private BoundarySide _boundarySide;
        private readonly List<TrackPoint> _boundary = new List<TrackPoint>();
        private LocalPlane _boundaryPlane;
        private GeoPoint _boundaryLast;
        private double _boundaryLength;

        /// <summary>A boundary must close within this distance of where it started.</summary>
        public const double MaxBoundaryGapMeters = 30.0;
        /// <summary>Smallest field accepted, square metres.</summary>
        public const double MinFieldAreaSquareMeters = 100.0;

        public void OnFix(GnssFix fix)
        {
            if (fix == null) return;
            lock (_lock)
            {
                _antennaFix = fix;
                _heading.Update(fix);
                _geometry.Apply(fix, _heading.Heading, out GnssFix guide, out GnssFix tool);
                _fix = guide;
                UpdateHeadland(tool);
                PaintCoverage(tool);
                fix = guide;
                if (_boundaryRecording && _boundaryMode == BoundaryMode.Drive && fix.HasPosition && _boundary.Count < MaxCurvePoints)
                {
                    GeoPoint p = _boundaryPlane.ToLocal(fix.Latitude, fix.Longitude);
                    double step = p.DistanceTo(_boundaryLast);
                    if (step >= CurveSpacingMeters)
                    {
                        _boundary.Add(new TrackPoint(fix.Latitude, fix.Longitude));
                        _boundaryLength += step;
                        _boundaryLast = p;
                    }
                }
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

        /// <summary>Antenna and implement position on the tractor. Set a new object to change it.</summary>
        public VehicleGeometry Vehicle
        {
            get { lock (_lock) return _geometry; }
            set { lock (_lock) _geometry = value ?? new VehicleGeometry(); }
        }

        /// <summary>
        /// Shifts the active line sideways, relative to the driving direction (positive = to the
        /// right as seen from the seat). Returns the new nudge, or NaN without an active line.
        /// </summary>
        public double NudgeActiveLine(double metersToTheRight)
        {
            lock (_lock)
            {
                if (_line == null) return double.NaN;
                bool same = true;
                if (_fix != null && _fix.HasPosition && _plane != null)
                    same = AbGuidance.Compute(_line, _plane, _plane.ToLocal(_fix.Latitude, _fix.Longitude), _heading.Heading, _passWidth).SameDirectionAsAb;
                _line.NudgeMeters += same ? metersToTheRight : -metersToTheRight;
                return _line.NudgeMeters;
            }
        }

        /// <summary>Moves the active line so the nearest pass runs exactly through the guide point now.</summary>
        public double SnapActiveLine()
        {
            lock (_lock)
            {
                if (_line == null || _fix == null || !_fix.HasPosition) return double.NaN;
                AbGuidanceResult r = AbGuidance.Compute(_line, _plane, _plane.ToLocal(_fix.Latitude, _fix.Longitude), _heading.Heading, _passWidth);
                // CrossTrack is relative to travel; turn it back into "right of A to B".
                _line.NudgeMeters += r.SameDirectionAsAb ? r.CrossTrackMeters : -r.CrossTrackMeters;
                return _line.NudgeMeters;
            }
        }

        public void ResetNudge()
        {
            lock (_lock)
            {
                if (_line != null) _line.NudgeMeters = 0;
            }
        }

        /// <summary>Full implement width in metres: the width of the painted strip.</summary>
        public double ToolWidth
        {
            get { lock (_lock) return _toolWidth; }
            set { lock (_lock) _toolWidth = Math.Max(0, value); }
        }

        /// <summary>False switches worked-area recording off completely (nothing is painted).</summary>
        public bool CoverageEnabled
        {
            get { lock (_lock) return _coverageEnabled; }
            set
            {
                lock (_lock)
                {
                    _coverageEnabled = value;
                    _prevCentre = _prevLeft = _prevRight = null;
                }
            }
        }

        /// <summary>
        /// Implement working: while true, the ground under it is painted as worked. Setting it is
        /// the driver's choice and ends an automatic headland pause.
        /// </summary>
        public bool Working
        {
            get { lock (_lock) return _working; }
            set
            {
                lock (_lock)
                {
                    _working = value;
                    _pausedInHeadland = false;
                    _prevCentre = _prevLeft = _prevRight = null;
                }
            }
        }

        /// <summary>Headland strip width along the boundary, metres; 0 switches the headland off.</summary>
        public double HeadlandWidth
        {
            get { lock (_lock) return _headlandWidth; }
            set { lock (_lock) { _headlandWidth = Math.Max(0, value); _pausedInHeadland = false; } }
        }

        /// <summary>Switch Work off when the implement enters the headland and on again when it leaves it.</summary>
        public bool AutoHeadland
        {
            get { lock (_lock) return _autoHeadland; }
            set { lock (_lock) { _autoHeadland = value; _pausedInHeadland = false; } }
        }

        // Where the implement is relative to the headland; switches Work when automatic.
        private void UpdateHeadland(GnssFix tool)
        {
            _toolInHeadland = null;
            _toolToHeadland = double.NaN;
            if (_headlandWidth <= 0 || _fieldPoly == null || _fieldPoly.Count < 3 || tool == null || !tool.HasPosition) return;

            GeoPoint p = _fieldPlane.ToLocal(tool.Latitude, tool.Longitude);
            bool inside = Geometry.Contains(_fieldPoly, p);
            double toEdge = Geometry.DistanceToEdge(_fieldPoly, p);
            double toHeadland = inside ? toEdge - _headlandWidth : -(toEdge + _headlandWidth);
            _toolToHeadland = toHeadland;
            _toolInHeadland = toHeadland < 0;

            if (!_autoHeadland) return;
            if (_working && toHeadland < -HeadlandHysteresisMeters)
            {
                _working = false;               // into the headland (or out of the field): pause
                _pausedInHeadland = true;
                _prevCentre = _prevLeft = _prevRight = null;
            }
            else if (_pausedInHeadland && !_working && toHeadland > HeadlandHysteresisMeters)
            {
                _working = true;                // back in the main part: carry on
                _pausedInHeadland = false;
            }
        }

        public CoverageMap Coverage
        {
            get { lock (_lock) return _coverage; }
        }

        /// <summary>Uses a coverage map (e.g. the open field's saved one), or null for a new one on the first strip.</summary>
        public void SetCoverage(CoverageMap map)
        {
            lock (_lock)
            {
                _coverage = map;
                _coverage?.SetBoundary(_field?.Boundary);
                _prevCentre = _prevLeft = _prevRight = null;
            }
        }

        private void PaintCoverage(GnssFix fix)
        {
            if (!_working || !_coverageEnabled || !fix.HasPosition || double.IsNaN(_heading.Heading) || _toolWidth < 0.05) return;
            if (_coverage == null)
            {
                _coverage = new CoverageMap(fix.Latitude, fix.Longitude);
                _coverage.SetBoundary(_field?.Boundary);
            }

            GeoPoint c = _coverage.Plane.ToLocal(fix.Latitude, fix.Longitude);
            double h = LocalPlane.ToRad(_heading.Heading);
            double half = _toolWidth / 2;
            // Right of the heading is (cos h, -sin h) as (east, north).
            var left = new GeoPoint(c.Northing + Math.Sin(h) * half, c.Easting - Math.Cos(h) * half);
            var right = new GeoPoint(c.Northing - Math.Sin(h) * half, c.Easting + Math.Cos(h) * half);

            if (_prevCentre.HasValue)
            {
                double step = _prevCentre.Value.DistanceTo(c);
                if (step < 0.02) return; // standing still: nothing new, keep the old edge
                if (step <= MaxPaintStepMeters) _coverage.PaintQuad(_prevLeft.Value, _prevRight.Value, right, left);
            }
            _prevCentre = c;
            _prevLeft = left;
            _prevRight = right;
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

        public Field ActiveField
        {
            get { lock (_lock) return _field; }
        }

        public bool IsRecordingBoundary
        {
            get { lock (_lock) return _boundaryRecording; }
        }

        /// <summary>Opens a field (null closes it). Its boundary is used for the edge warning.</summary>
        public void SetActiveField(Field field)
        {
            lock (_lock)
            {
                _field = field;
                _fieldPlane = field?.CreatePlane();
                _fieldPoly = _fieldPlane == null ? null : field.BoundaryLocal(_fieldPlane);
                _coverage?.SetBoundary(field?.Boundary);
            }
        }

        /// <summary>A second Corner press closer than this to the last corner is ignored (pressed twice).</summary>
        public const double MinCornerSpacingMeters = 1.0;

        /// <summary>
        /// Starts recording a boundary. Drive mode records the path from here; corner mode starts
        /// empty and takes the corners from <see cref="AddCorner"/>. False without a position.
        /// </summary>
        public bool StartBoundary(BoundarySide side, BoundaryMode mode = BoundaryMode.Drive)
        {
            lock (_lock)
            {
                if (_fix == null || !_fix.HasPosition) return false;
                _boundaryRecording = true;
                _boundaryMode = mode;
                _boundarySide = side;
                _boundary.Clear();
                if (mode == BoundaryMode.Drive) _boundary.Add(new TrackPoint(_fix.Latitude, _fix.Longitude));
                _boundaryPlane = new LocalPlane(_fix.Latitude, _fix.Longitude);
                _boundaryLast = new GeoPoint(0, 0);
                _boundaryLength = 0;
                return true;
            }
        }

        /// <summary>
        /// Corner mode: marks the current position as the next corner. Returns the number of
        /// corners, or -1 when it was ignored (no position, or the same spot as the last corner).
        /// </summary>
        public int AddCorner()
        {
            lock (_lock)
            {
                if (!_boundaryRecording || _boundaryMode != BoundaryMode.Corners || _fix == null || !_fix.HasPosition) return -1;
                GeoPoint here = _boundaryPlane.ToLocal(_fix.Latitude, _fix.Longitude);
                if (_boundary.Count > 0)
                {
                    TrackPoint last = _boundary[_boundary.Count - 1];
                    GeoPoint lastLocal = _boundaryPlane.ToLocal(last.Latitude, last.Longitude);
                    if (lastLocal.DistanceTo(here) < MinCornerSpacingMeters) return -1;
                    _boundaryLength += lastLocal.DistanceTo(here);
                }
                _boundary.Add(new TrackPoint(_fix.Latitude, _fix.Longitude));
                return _boundary.Count;
            }
        }

        /// <summary>Corner mode: removes the last corner (pressed by mistake). Returns the corners left.</summary>
        public int RemoveLastCorner()
        {
            lock (_lock)
            {
                if (!_boundaryRecording || _boundaryMode != BoundaryMode.Corners || _boundary.Count == 0) return 0;
                _boundary.RemoveAt(_boundary.Count - 1);
                _boundaryLength = 0;
                for (int i = 1; i < _boundary.Count; i++)
                    _boundaryLength += _boundaryPlane.ToLocal(_boundary[i - 1].Latitude, _boundary[i - 1].Longitude)
                        .DistanceTo(_boundaryPlane.ToLocal(_boundary[i].Latitude, _boundary[i].Longitude));
                return _boundary.Count;
            }
        }

        public void CancelBoundary()
        {
            lock (_lock)
            {
                _boundaryRecording = false;
                _boundary.Clear();
            }
        }

        /// <summary>
        /// Ends the boundary drive and returns the closed boundary, moved sideways for
        /// <see cref="BoundarySide"/> by <paramref name="halfWidth"/>, and smoothed. Null with a
        /// reason if it is too small or does not come back near the start; recording then goes on.
        /// </summary>
        public List<TrackPoint> FinishBoundary(double halfWidth, out double areaSquareMeters, out string error)
        {
            lock (_lock)
            {
                areaSquareMeters = 0;
                error = null;
                if (!_boundaryRecording) { error = "Start recording the boundary first"; return null; }
                if (_boundaryMode == BoundaryMode.Corners) return FinishCorners(halfWidth, out areaSquareMeters, out error);

                var path = new List<GeoPoint>();
                foreach (TrackPoint t in _boundary) path.Add(_boundaryPlane.ToLocal(t.Latitude, t.Longitude));
                if (_fix != null && _fix.HasPosition)
                {
                    GeoPoint now = _boundaryPlane.ToLocal(_fix.Latitude, _fix.Longitude);
                    if (now.DistanceTo(path[path.Count - 1]) > 0.05) path.Add(now);
                }

                double gap = path[path.Count - 1].DistanceTo(path[0]);
                if (path.Count < 3 || Geometry.Area(path) < MinFieldAreaSquareMeters)
                {
                    error = "Drive all the way round the field first";
                    return null;
                }
                if (gap > MaxBoundaryGapMeters)
                {
                    error = string.Format(System.Globalization.CultureInfo.InvariantCulture,
                        "Drive back to where you started ({0:0} m to go)", gap);
                    return null;
                }

                double offset = _boundarySide == BoundarySide.LeftOfImplement ? -halfWidth
                    : _boundarySide == BoundarySide.RightOfImplement ? halfWidth : 0;
                List<GeoPoint> moved = Geometry.OffsetClosed(path, offset);

                var result = new List<TrackPoint>();
                foreach (int i in AbGuidance.Simplify(moved, CurveToleranceMeters))
                {
                    _boundaryPlane.ToWgs84(moved[i], out double lat, out double lon);
                    result.Add(new TrackPoint(lat, lon));
                }
                // The last point is next to the first: the closing edge is implied.
                if (result.Count > 3)
                {
                    GeoPoint first = _boundaryPlane.ToLocal(result[0].Latitude, result[0].Longitude);
                    GeoPoint last = _boundaryPlane.ToLocal(result[result.Count - 1].Latitude, result[result.Count - 1].Longitude);
                    if (first.DistanceTo(last) < 1.0) result.RemoveAt(result.Count - 1);
                }

                var local = new List<GeoPoint>();
                foreach (TrackPoint t in result) local.Add(_boundaryPlane.ToLocal(t.Latitude, t.Longitude));
                areaSquareMeters = Geometry.Area(local);
                if (result.Count < 3 || areaSquareMeters < MinFieldAreaSquareMeters)
                {
                    error = "Drive all the way round the field first";
                    return null;
                }

                _boundaryRecording = false;
                _boundary.Clear();
                return result;
            }
        }

        // Corner mode: the corners as they are, joined with straight lines and closed to the first.
        private List<TrackPoint> FinishCorners(double halfWidth, out double areaSquareMeters, out string error)
        {
            areaSquareMeters = 0;
            error = null;
            if (_boundary.Count < 3)
            {
                error = "Mark at least 3 corners";
                return null;
            }

            var corners = new List<GeoPoint>();
            foreach (TrackPoint t in _boundary) corners.Add(_boundaryPlane.ToLocal(t.Latitude, t.Longitude));
            double offset = _boundarySide == BoundarySide.LeftOfImplement ? -halfWidth
                : _boundarySide == BoundarySide.RightOfImplement ? halfWidth : 0;
            List<GeoPoint> moved = Geometry.OffsetClosed(corners, offset);

            areaSquareMeters = Geometry.Area(moved);
            if (areaSquareMeters < MinFieldAreaSquareMeters)
            {
                error = "The corners enclose too small an area";
                return null;
            }

            var result = new List<TrackPoint>();
            foreach (GeoPoint p in moved)
            {
                _boundaryPlane.ToWgs84(p, out double lat, out double lon);
                result.Add(new TrackPoint(lat, lon));
            }
            _boundaryRecording = false;
            _boundary.Clear();
            return result;
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
                    ActiveField = _field,
                    RecordingBoundary = _boundaryRecording,
                    BoundarySide = _boundarySide,
                    BoundaryMode = _boundaryMode,
                    BoundaryCorners = _boundaryRecording && _boundaryMode == BoundaryMode.Corners ? _boundary.Count : 0,
                    Working = _working,
                    Coverage = _coverage,
                };
                if (_coverage != null)
                {
                    s.WorkedSquareMeters = _coverage.CoveredSquareMeters;
                    s.OverlapSquareMeters = _coverage.OverlapSquareMeters;
                    s.WorkedInsideSquareMeters = _coverage.CoveredInsideSquareMeters;
                }
                if (_fieldPoly != null && _fieldPoly.Count >= 3)
                {
                    s.FieldSquareMeters = Geometry.Area(_fieldPoly);
                    if (_coverage == null) s.WorkedInsideSquareMeters = 0;
                }

                bool hasPos = _fix != null && _fix.HasPosition;
                // Passes are made on the line's plane, so that one is used for drawing when there is a line.
                LocalPlane draw = _plane ?? _fieldPlane ?? _recordPlane ?? _boundaryPlane
                    ?? (hasPos ? new LocalPlane(_fix.Latitude, _fix.Longitude) : null);
                if (draw == null) return s;
                s.DrawPlane = draw;

                if (hasPos)
                {
                    GeoPoint v = draw.ToLocal(_fix.Latitude, _fix.Longitude);
                    s.Vehicle = v;

                    if (_line != null)
                    {
                        s.Result = AbGuidance.Compute(_line, _plane, v, _heading.Heading, _passWidth);
                        int from = s.Result.Pass - DrawPassesEachSide, to = s.Result.Pass + DrawPassesEachSide;
                        for (int pass = from; pass <= to; pass++)
                        {
                            var shape = new PassShape { Pass = pass, Points = _line.PassPolyline(_plane, pass, _passWidth) };
                            s.Passes.Add(shape);
                            if (pass == s.Result.Pass)
                                s.Steer = Steering.Compute(shape.Points, v, _heading.Heading, s.Result.SameDirectionAsAb,
                                    Steering.LookaheadMeters(_fix.SpeedKmh));
                        }
                    }

                    if (_fieldPoly != null && _fieldPoly.Count >= 3)
                    {
                        GeoPoint fv = _fieldPlane.ToLocal(_fix.Latitude, _fix.Longitude);
                        s.InsideField = Geometry.Contains(_fieldPoly, fv);
                        s.DistanceToBoundary = Geometry.DistanceToEdge(_fieldPoly, fv);
                    }
                }
                else if (_line != null)
                {
                    s.Passes.Add(new PassShape { Pass = 0, Points = _line.PassPolyline(_plane, 0, _passWidth) });
                }

                if (_fieldPoly != null && _fieldPoly.Count >= 3)
                    s.Boundary = Geometry.Reproject(_fieldPoly, _fieldPlane, draw);
                if (_fieldPoly != null && _fieldPoly.Count >= 3 && _headlandWidth > 0)
                {
                    // Inward is left of a counter-clockwise outline, right of a clockwise one.
                    double signed = 0;
                    for (int i = 0, j = _fieldPoly.Count - 1; i < _fieldPoly.Count; j = i++)
                        signed += _fieldPoly[j].Easting * _fieldPoly[i].Northing - _fieldPoly[i].Easting * _fieldPoly[j].Northing;
                    List<GeoPoint> inner = Geometry.OffsetClosed(_fieldPoly, signed > 0 ? -_headlandWidth : _headlandWidth);
                    s.HeadlandLine = Geometry.Reproject(inner, _fieldPlane, draw);
                }
                s.HeadlandWidth = _headlandWidth;
                s.InHeadland = _toolInHeadland;
                s.DistanceToHeadland = _toolToHeadland;
                s.WorkPausedInHeadland = _pausedInHeadland;
                if (_line != null)
                {
                    s.LineA = draw.ToLocal(_line.LatitudeA, _line.LongitudeA);
                    s.LineB = draw.ToLocal(_line.LatitudeB, _line.LongitudeB);
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

                if (_boundaryRecording)
                {
                    foreach (TrackPoint t in _boundary) s.BoundaryPath.Add(draw.ToLocal(t.Latitude, t.Longitude));
                    s.BoundaryRecordedMeters = _boundaryLength;
                    if (hasPos)
                    {
                        s.BoundaryPath.Add(draw.ToLocal(_fix.Latitude, _fix.Longitude));
                        s.BoundaryGapMeters = _boundaryPlane.ToLocal(_fix.Latitude, _fix.Longitude).DistanceTo(new GeoPoint(0, 0));
                    }
                }
                return s;
            }
        }
    }
}

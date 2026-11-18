using System;
using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.Views;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Top-down driving view, heading up: the vehicle sits low in the middle, the current pass
    /// is bold, neighbouring passes thin, and a line being recorded is drawn in orange.
    /// </summary>
    public sealed class GuidanceView : View
    {
        private readonly Paint _current = Stroke(Palette.CurrentPass, 5);
        private readonly Paint _other = Stroke(Palette.OtherPass, 2);
        private readonly Paint _refLine = Stroke(Palette.RefLine, 3);
        private readonly Paint _recording = Stroke(Palette.Recording, 4);
        private readonly Paint _boundary = Stroke(Palette.Boundary, 3);
        private readonly Paint _boundaryRec = Stroke(Palette.BoundaryRecording, 4);
        private readonly Paint _vehicle = Fill(Palette.Vehicle);
        private readonly Paint _vehicleEdge = Stroke(Palette.VehicleEdge, 2);
        private readonly Paint _pointA = Fill("#FF7043");
        private readonly Paint _cornerDot = Fill("#4DD0E1");
        private readonly Paint _headland = Stroke(Palette.Headland, 2.5f);
        private readonly Paint _abDot = Fill(Palette.RefLine);
        private readonly Paint _abText = new Paint(PaintFlags.AntiAlias) { Color = Color.White, TextAlign = Paint.Align.Center, FakeBoldText = true };
        private readonly Paint _goal = Fill(Palette.Goal);
        private readonly Paint _goalLine = Stroke(Palette.Goal, 1.5f);
        private readonly Paint _hint = new Paint(PaintFlags.AntiAlias) { Color = Color.ParseColor(Palette.Hint), TextAlign = Paint.Align.Center };
        private GuidanceSnapshot _snap;
        /// <summary>Satellite map source, or null for no map.</summary>
        public MapTileProvider Map { get; set; }
        public string MapAttribution { get; set; } = "";
        /// <summary>Most detailed zoom level requested from the map source; closer views enlarge it.</summary>
        public int MapMaxZoom { get; set; } = MapTiles.MaxZoom;
        private readonly Paint _mapPaint = new Paint { FilterBitmap = true };
        private readonly Paint _scaleText = new Paint(PaintFlags.AntiAlias) { Color = Color.ParseColor("#CFD8DC") };
        private readonly Paint _attribution = new Paint(PaintFlags.AntiAlias) { Color = Color.ParseColor("#CFD8DC"), TextAlign = Paint.Align.Right };

        /// <summary>Draw the worked area (Settings).</summary>
        public bool ShowCoverage { get; set; } = true;
        private readonly Dictionary<long, (int Version, Bitmap Bitmap)> _tileCache = new Dictionary<long, (int, Bitmap)>();
        private CoverageMap _cachedMap;
        private readonly Paint _tilePaint = new Paint { FilterBitmap = false };
        private static readonly int Worked = Color.Argb(110, 102, 187, 106).ToArgb();
        private static readonly int Twice = Color.Argb(140, 239, 83, 80).ToArgb();
        private float _density;

        public GuidanceView(Context context) : base(context)
        {
            _density = context.Resources.DisplayMetrics.Density;
            _attribution.TextSize = global::Android.Util.TypedValue.ApplyDimension(global::Android.Util.ComplexUnitType.Sp, 10, context.Resources.DisplayMetrics);
            _attribution.SetShadowLayer(2, 0, 0, Color.Black);
            _scaleText.TextSize = _attribution.TextSize * 1.2f;
            _scaleText.SetShadowLayer(2, 0, 0, Color.Black);
            SetUpGestures(context);
            _headland.SetPathEffect(new DashPathEffect(new[] { 14 * _density, 9 * _density }, 0));
            _abText.TextSize = 13 * _density;
            _hint.TextSize = global::Android.Util.TypedValue.ApplyDimension(global::Android.Util.ComplexUnitType.Sp, 18, context.Resources.DisplayMetrics);
            foreach (Paint p in new[] { _current, _other, _refLine, _recording, _boundary, _boundaryRec, _vehicleEdge, _goalLine, _headland }) p.StrokeWidth *= _density;
        }

        public void Show(GuidanceSnapshot snap)
        {
            _snap = snap;
            Invalidate();
        }

        // ---------------------------------------------------------------- camera

        public const double MinViewMeters = 5, MaxViewMeters = 5000;

        private bool _follow = true;
        private double _zoom = 1.0;                       // multiplies the default width of the view
        private double _camLat = double.NaN, _camLon;     // free camera centre
        private double _camRotation;                      // free camera: direction shown as "up"
        private ScaleGestureDetector _pinch;
        private GestureDetector _gestures;

        // The camera of the last frame, for turning finger movements into metres.
        private float _lastScale = 1;
        private double _lastRotation;
        private GeoPoint _lastCentre;
        private LocalPlane _lastPlane;

        /// <summary>True while the view follows the tractor (heading up); false after moving the map by hand.</summary>
        public bool Following => _follow;

        /// <summary>Back to following the tractor, at the default zoom.</summary>
        public void Follow()
        {
            _follow = true;
            _zoom = 1.0;
            Invalidate();
        }

        /// <summary>Zooms in (factor above 1) or out (below 1), in either mode.</summary>
        public void ZoomBy(double factor)
        {
            _zoom /= factor;
            Invalidate();
        }

        /// <summary>Shows the whole field (or the line), north up, and stops following.</summary>
        public bool ShowWholeField()
        {
            GuidanceSnapshot s = _snap;
            if (s?.DrawPlane == null || !FitAll(s, out GeoPoint centre, out double meters)) return false;
            s.DrawPlane.ToWgs84(centre, out _camLat, out _camLon);
            _camRotation = 0;
            _zoom = meters / BaseViewMeters(s);
            _follow = false;
            Invalidate();
            return true;
        }

        private static double BaseViewMeters(GuidanceSnapshot s) => Math.Max(15.0, 3.2 * Math.Max(0.5, s.PassWidth));

        /// <summary>Centre and width (metres) that show the boundary, else the passes, else the path being recorded.</summary>
        private bool FitAll(GuidanceSnapshot s, out GeoPoint centre, out double meters)
        {
            double minN = double.MaxValue, maxN = double.MinValue, minE = double.MaxValue, maxE = double.MinValue;
            void Add(IEnumerable<GeoPoint> pts)
            {
                if (pts == null) return;
                foreach (GeoPoint p in pts)
                {
                    minN = Math.Min(minN, p.Northing); maxN = Math.Max(maxN, p.Northing);
                    minE = Math.Min(minE, p.Easting); maxE = Math.Max(maxE, p.Easting);
                }
            }
            if (s.Boundary.Count >= 3) Add(s.Boundary);
            else if (s.Recording || s.RecordingBoundary) { Add(s.RecordingPath); Add(s.BoundaryPath); }
            else if (s.ActiveLine != null && s.DrawPlane != null)
                Add(s.ActiveLine.ToLocal(s.DrawPlane)); // the line itself, not its long extensions
            // The tractor only counts without a boundary: it may be far away (another field, the simulator).
            if (s.Vehicle.HasValue && s.Boundary.Count < 3) Add(new[] { s.Vehicle.Value });

            centre = new GeoPoint((minN + maxN) / 2, (minE + maxE) / 2);
            if (minN > maxN) { meters = 0; return false; }
            double w = maxE - minE, h = maxN - minN;
            double aspect = Height > 0 ? (double)Width / Height : 1.5;
            meters = Math.Max(30, Math.Max(w, h * aspect) * 1.15);
            return true;
        }

        private void SetUpGestures(Context context)
        {
            _pinch = new ScaleGestureDetector(context, new PinchListener(this));
            _gestures = new GestureDetector(context, new PanListener(this));
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _pinch.OnTouchEvent(e);
            if (!_pinch.IsInProgress) _gestures.OnTouchEvent(e);
            return true;
        }

        /// <summary>Moves the map with the finger. Leaves "follow" and keeps the current rotation, so it holds still.</summary>
        private void Pan(float dx, float dy)
        {
            if (_lastPlane == null) return;
            GeoPoint c = _lastCentre;
            if (_follow)
            {
                _follow = false;
                _camRotation = _lastRotation;
            }
            else if (!double.IsNaN(_camLat))
            {
                c = _lastPlane.ToLocal(_camLat, _camLon);
            }

            double h = _camRotation * Math.PI / 180.0;
            double right = dx / _lastScale, forward = -dy / _lastScale;
            double de = right * Math.Cos(h) + forward * Math.Sin(h);
            double dn = -right * Math.Sin(h) + forward * Math.Cos(h);
            _lastPlane.ToWgs84(new GeoPoint(c.Northing + dn, c.Easting + de), out _camLat, out _camLon);
            Invalidate();
        }

        private sealed class PinchListener : ScaleGestureDetector.SimpleOnScaleGestureListener
        {
            private readonly GuidanceView _v;
            public PinchListener(GuidanceView v) { _v = v; }
            public override bool OnScale(ScaleGestureDetector detector)
            {
                _v.ZoomBy(detector.ScaleFactor);
                return true;
            }
        }

        private sealed class PanListener : GestureDetector.SimpleOnGestureListener
        {
            private readonly GuidanceView _v;
            public PanListener(GuidanceView v) { _v = v; }
            public override bool OnDown(MotionEvent e) => true;
            public override bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
            {
                _v.Pan(distanceX, distanceY);
                return true;
            }
            public override bool OnDoubleTap(MotionEvent e)
            {
                _v.ZoomBy(2);
                return true;
            }
        }

        // ---------------------------------------------------------------- drawing

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.ParseColor(Palette.MapBackground));
            GuidanceSnapshot s = _snap;

            if (s?.DrawPlane == null)
            {
                canvas.DrawText(s == null || s.Fix == null ? Tr.T("Waiting for position") : Tr.T("No position fix"), Width / 2f, Height / 2f, _hint);
                return;
            }

            double vehicleHeading = !double.IsNaN(s.VehicleHeading) ? s.VehicleHeading
                : s.Result != null ? s.Result.TravelLineHeading : 0;
            _zoom = Math.Max(MinViewMeters / BaseViewMeters(s), Math.Min(MaxViewMeters / BaseViewMeters(s), _zoom));
            double viewMeters = BaseViewMeters(s) * _zoom;
            float scale = (float)(Width / viewMeters);

            // Camera: follow the tractor (heading up, tractor low in the view), or free (centre and rotation set by hand).
            GeoPoint v;
            double heading;
            float cx = Width / 2f, cy;
            if (_follow && s.Vehicle.HasValue)
            {
                v = s.Vehicle.Value;
                heading = vehicleHeading;
                cy = Height * 0.68f;
            }
            else if (!_follow && !double.IsNaN(_camLat))
            {
                v = s.DrawPlane.ToLocal(_camLat, _camLon);
                heading = _camRotation;
                cy = Height / 2f;
            }
            else if (FitAll(s, out GeoPoint centre, out double meters))
            {
                // No position yet: show the field (or line) until there is one.
                v = centre;
                heading = 0;
                scale = (float)(Width / Math.Max(meters, viewMeters));
                cy = Height / 2f;
            }
            else
            {
                canvas.DrawText(Tr.T("Waiting for position"), Width / 2f, Height / 2f, _hint);
                return;
            }
            _lastScale = scale;
            _lastRotation = heading;
            _lastCentre = v;
            _lastPlane = s.DrawPlane;

            double h = heading * Math.PI / 180.0;
            double sin = Math.Sin(h), cos = Math.Cos(h);

            void ToScreen(GeoPoint p, out float x, out float y)
            {
                double de = p.Easting - v.Easting, dn = p.Northing - v.Northing;
                double forward = de * sin + dn * cos;
                double right = de * cos - dn * sin;
                x = cx + (float)(right * scale);
                y = cy - (float)(forward * scale);
            }

            void DrawLine(List<GeoPoint> pts, Paint paint, bool closed = false)
            {
                if (pts == null || pts.Count < 2) return;
                using (var path = new Path())
                {
                    ToScreen(pts[0], out float x, out float y);
                    path.MoveTo(x, y);
                    for (int i = 1; i < pts.Count; i++)
                    {
                        ToScreen(pts[i], out x, out y);
                        path.LineTo(x, y);
                    }
                    if (closed) path.Close();
                    canvas.DrawPath(path, paint);
                }
            }

            // Back to front: satellite map, worked area, boundary, lines, tractor.
            DrawMap(canvas, s, v, heading, scale, cx, cy);
            DrawCoverage(canvas, s, v, heading, scale, cx, cy);

            DrawLine(s.Boundary, _boundary, closed: true);
            DrawLine(s.HeadlandLine, _headland, closed: true);
            if (s.RecordingBoundary)
            {
                DrawLine(s.BoundaryPath, _boundaryRec);
                // Corner mode: the first BoundaryCorners points are the marked corners.
                for (int i = 0; i < s.BoundaryCorners && i < s.BoundaryPath.Count; i++)
                {
                    ToScreen(s.BoundaryPath[i], out float kx, out float ky);
                    canvas.DrawCircle(kx, ky, 7 * _density, _cornerDot);
                }
            }

            int current = s.Result?.Pass ?? 0;
            foreach (PassShape pass in s.Passes)
            {
                if (pass.Pass == current) continue;
                DrawLine(pass.Points, pass.Pass == 0 ? _refLine : _other);
            }
            foreach (PassShape pass in s.Passes)
            {
                if (pass.Pass == current) DrawLine(pass.Points, _current);
            }

            if (s.Recording && s.RecordingPath.Count > 0)
            {
                DrawLine(s.RecordingPath, _recording);
                ToScreen(s.RecordingPath[0], out float ax, out float ay);
                canvas.DrawCircle(ax, ay, 8 * _density, _pointA);
            }

            // A and B of the active line: where it was recorded.
            void Marker(GeoPoint? p, string letter)
            {
                if (!p.HasValue) return;
                ToScreen(p.Value, out float mx, out float my);
                canvas.DrawCircle(mx, my, 11 * _density, _abDot);
                canvas.DrawText(letter, mx, my + 4.5f * _density, _abText);
            }
            Marker(s.LineA, "A");
            Marker(s.LineB, "B");

            if (s.Vehicle.HasValue)
            {
                ToScreen(s.Vehicle.Value, out float tx, out float ty);

                // Goal point ahead on the pass: where the lightbar steers to.
                if (s.Steer != null)
                {
                    ToScreen(s.Steer.GoalPoint, out float gx, out float gy);
                    canvas.DrawLine(tx, ty, gx, gy, _goalLine);
                    canvas.DrawCircle(gx, gy, 6 * _density, _goal);
                }

                // Vehicle: an arrow pointing the way it drives.
                float w = 14 * _density, len = 30 * _density;
                canvas.Save();
                canvas.Translate(tx, ty);
                canvas.Rotate((float)(vehicleHeading - heading));
                using (var arrow = new Path())
                {
                    arrow.MoveTo(0, -len * 0.6f);
                    arrow.LineTo(w, len * 0.4f);
                    arrow.LineTo(0, len * 0.15f);
                    arrow.LineTo(-w, len * 0.4f);
                    arrow.Close();
                    canvas.DrawPath(arrow, _vehicle);
                    canvas.DrawPath(arrow, _vehicleEdge);
                }
                canvas.Restore();
            }

            if (s.ActiveLine == null && !s.Recording && !s.RecordingBoundary && s.Boundary.Count < 3)
                canvas.DrawText(Tr.T("No line: press A, drive, press B"), Width / 2f, Height * 0.2f, _hint);

            // Scale: the width of the view in metres.
            canvas.DrawText(viewMeters >= 1000 ? Tr.F("{0:0.0} km wide", viewMeters / 1000) : Tr.F("{0:0} m wide", viewMeters),
                10 * _density, Height - 8 * _density, _scaleText);
        }

        /// <summary>Satellite map tiles under everything else; tiles not loaded yet are simply left out.</summary>
        private void DrawMap(Canvas canvas, GuidanceSnapshot s, GeoPoint v, double heading, float scale, float cx, float cy)
        {
            MapTileProvider map = Map;
            if (map == null || s.DrawPlane == null) return;

            s.DrawPlane.ToWgs84(v, out double lat, out double lon); // the camera centre
            int zoom = MapTiles.ZoomFor(1.0 / scale, lat, MapMaxZoom);
            double radius = Math.Sqrt((double)Width * Width + (double)Height * Height) / scale * 0.75;
            double dLat = radius / 111132.0, dLon = radius / (111320.0 * Math.Cos(lat * Math.PI / 180.0));
            List<TileId> tiles = MapTiles.TilesFor(lat - dLat, lon - dLon, lat + dLat, lon + dLon, zoom);
            if (tiles.Count > 120) return; // very far zoomed out: not worth it

            canvas.Save();
            canvas.Translate(cx, cy);
            canvas.Rotate((float)-heading);
            foreach (TileId t in tiles)
            {
                // The tile itself, or (missing at this zoom, or still loading) the matching part of a
                // coarser one, enlarged: blurry is better than a hole.
                Bitmap bmp = map.Get(t);
                Rect src = null;
                for (int up = 1; bmp == null && up <= 6 && t.Z - up >= MapTiles.MinZoom; up++)
                {
                    TileId parent = MapTiles.Parent(t, up, out int sx, out int sy, out int part);
                    bmp = map.Get(parent);
                    if (bmp == null) continue;
                    // Bitmaps may come in at another size than 256: scale the source rectangle.
                    float k = (float)bmp.Width / MapTiles.TileSize;
                    src = new Rect((int)(sx * k), (int)(sy * k), (int)((sx + part) * k), (int)((sy + part) * k));
                }
                if (bmp == null) continue;
                MapTiles.ToLatLon(t.X, t.Y, t.Z, out double nLat, out double wLon);
                MapTiles.ToLatLon(t.X + 1, t.Y + 1, t.Z, out double sLat, out double eLon);
                GeoPoint nw = s.DrawPlane.ToLocal(nLat, wLon), se = s.DrawPlane.ToLocal(sLat, eLon);
                var dst = new RectF(
                    (float)((nw.Easting - v.Easting) * scale), (float)(-(nw.Northing - v.Northing) * scale),
                    (float)((se.Easting - v.Easting) * scale) + 0.5f, (float)(-(se.Northing - v.Northing) * scale) + 0.5f);
                canvas.DrawBitmap(bmp, src, dst, _mapPaint);
            }
            canvas.Restore();

            if (!string.IsNullOrEmpty(MapAttribution))
                canvas.DrawText(MapAttribution, Width - 6 * _density, Height - 6 * _density, _attribution);
        }

        /// <summary>Worked ground: green, worked twice or more: red. Drawn under the lines.</summary>
        private void DrawCoverage(Canvas canvas, GuidanceSnapshot s, GeoPoint v, double heading, float scale, float cx, float cy)
        {
            CoverageMap map = s.Coverage;
            if (!ShowCoverage || map == null || s.DrawPlane == null)
            {
                if (_tileCache.Count > 0) ClearTileCache(); // give the memory back
                return;
            }
            if (!ReferenceEquals(map, _cachedMap))
            {
                ClearTileCache();
                _cachedMap = map;
            }

            // The map has its own plane; over a field the planes differ only by a shift.
            GeoPoint o = s.DrawPlane.ToLocal(map.Plane.OriginLatitude, map.Plane.OriginLongitude);
            double radius = Math.Sqrt((double)Width * Width + (double)Height * Height) / scale;
            double vn = v.Northing - o.Northing, ve = v.Easting - o.Easting;
            List<CoverageTileInfo> tiles = map.TilesIn(vn - radius, vn + radius, ve - radius, ve + radius);
            if (_tileCache.Count > 600) ClearTileCache();

            canvas.Save();
            canvas.Translate(cx, cy);
            canvas.Rotate((float)-heading);
            float size = (float)(CoverageMap.TileMeters * scale);
            foreach (CoverageTileInfo t in tiles)
            {
                Bitmap bmp = TileBitmap(map, t);
                if (bmp == null) continue;
                float left = (float)((t.TileX * CoverageMap.TileMeters - ve) * scale);
                float bottom = (float)(-(t.TileY * CoverageMap.TileMeters - vn) * scale);
                // Slightly larger so neighbouring tiles leave no hairline gaps.
                canvas.DrawBitmap(bmp, null, new RectF(left, bottom - size, left + size + 0.5f, bottom + 0.5f), _tilePaint);
            }
            canvas.Restore();
        }

        private Bitmap TileBitmap(CoverageMap map, CoverageTileInfo t)
        {
            long key = ((long)t.TileX << 32) | (uint)t.TileY;
            if (_tileCache.TryGetValue(key, out var cached) && cached.Version == t.Version) return cached.Bitmap;

            byte[] counts = map.CopyTile(t.TileX, t.TileY);
            if (counts == null) return null;
            int n = CoverageMap.TileCells;
            var pixels = new int[n * n];
            for (int row = 0; row < n; row++)          // row 0 of the tile is its south edge,
                for (int col = 0; col < n; col++)      // row 0 of a bitmap is its top: flip
                {
                    byte c = counts[row * n + col];
                    pixels[(n - 1 - row) * n + col] = c == 0 ? 0 : c == 1 ? Worked : Twice;
                }
            Bitmap bmp = Bitmap.CreateBitmap(pixels, n, n, Bitmap.Config.Argb8888);
            cached.Bitmap?.Recycle();
            _tileCache[key] = (t.Version, bmp);
            return bmp;
        }

        private void ClearTileCache()
        {
            foreach (var entry in _tileCache.Values) entry.Bitmap?.Recycle();
            _tileCache.Clear();
        }

        private static Paint Stroke(string color, float width)
        {
            return new Paint(PaintFlags.AntiAlias)
            {
                Color = Color.ParseColor(color),
                StrokeWidth = width,
                StrokeJoin = Paint.Join.Round,
                StrokeCap = Paint.Cap.Round,
            }.Also(p => p.SetStyle(Paint.Style.Stroke));
        }

        private static Paint Fill(string color)
        {
            return new Paint(PaintFlags.AntiAlias) { Color = Color.ParseColor(color) }.Also(p => p.SetStyle(Paint.Style.Fill));
        }
    }

    internal static class PaintExtensions
    {
        public static Paint Also(this Paint p, Action<Paint> a)
        {
            a(p);
            return p;
        }
    }
}

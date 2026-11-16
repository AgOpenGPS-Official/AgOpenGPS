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
        private readonly Paint _current = Stroke("#FFD54F", 5);
        private readonly Paint _other = Stroke("#607D8B", 2);
        private readonly Paint _refLine = Stroke("#4FC3F7", 3);
        private readonly Paint _recording = Stroke("#FF7043", 4);
        private readonly Paint _boundary = Stroke("#A5D6A7", 3);
        private readonly Paint _boundaryRec = Stroke("#4DD0E1", 4);
        private readonly Paint _vehicle = Fill("#FFFFFF");
        private readonly Paint _vehicleEdge = Stroke("#212121", 2);
        private readonly Paint _pointA = Fill("#FF7043");
        private readonly Paint _goal = Fill("#FFFFFF");
        private readonly Paint _goalLine = Stroke("#FFFFFF", 1.5f);
        private readonly Paint _hint = new Paint(PaintFlags.AntiAlias) { Color = Color.ParseColor("#B0BEC5"), TextAlign = Paint.Align.Center };
        private GuidanceSnapshot _snap;
        /// <summary>Satellite map source, or null for no map.</summary>
        public MapTileProvider Map { get; set; }
        public string MapAttribution { get; set; } = "";
        private readonly Paint _mapPaint = new Paint { FilterBitmap = true };
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
            _hint.TextSize = global::Android.Util.TypedValue.ApplyDimension(global::Android.Util.ComplexUnitType.Sp, 18, context.Resources.DisplayMetrics);
            foreach (Paint p in new[] { _current, _other, _refLine, _recording, _boundary, _boundaryRec, _vehicleEdge, _goalLine }) p.StrokeWidth *= _density;
        }

        public void Show(GuidanceSnapshot snap)
        {
            _snap = snap;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.ParseColor("#263238"));
            GuidanceSnapshot s = _snap;

            if (s?.Vehicle == null)
            {
                canvas.DrawText(s == null || s.Fix == null ? "Waiting for position" : "No position fix", Width / 2f, Height / 2f, _hint);
                return;
            }

            GeoPoint v = s.Vehicle.Value;
            double heading = !double.IsNaN(s.VehicleHeading) ? s.VehicleHeading
                : s.Result != null ? s.Result.TravelLineHeading : 0;
            double h = heading * Math.PI / 180.0;
            double sin = Math.Sin(h), cos = Math.Cos(h);

            // About three passes across the screen, at least 15 m.
            double viewMeters = Math.Max(15.0, 3.2 * Math.Max(0.5, s.PassWidth));
            float scale = (float)(Width / viewMeters);
            float cx = Width / 2f, cy = Height * 0.68f;

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

            DrawLine(s.Boundary, _boundary, closed: true);
            if (s.RecordingBoundary) DrawLine(s.BoundaryPath, _boundaryRec);

            DrawMap(canvas, s, v, heading, scale, cx, cy);
            DrawCoverage(canvas, s, v, heading, scale, cx, cy);

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

            // Goal point ahead on the pass: where the lightbar steers to.
            if (s.Steer != null)
            {
                ToScreen(s.Steer.GoalPoint, out float gx, out float gy);
                canvas.DrawLine(cx, cy, gx, gy, _goalLine);
                canvas.DrawCircle(gx, gy, 6 * _density, _goal);
            }

            // Vehicle: an arrow pointing up.
            float w = 14 * _density, len = 30 * _density;
            using (var arrow = new Path())
            {
                arrow.MoveTo(cx, cy - len * 0.6f);
                arrow.LineTo(cx + w, cy + len * 0.4f);
                arrow.LineTo(cx, cy + len * 0.15f);
                arrow.LineTo(cx - w, cy + len * 0.4f);
                arrow.Close();
                canvas.DrawPath(arrow, _vehicle);
                canvas.DrawPath(arrow, _vehicleEdge);
            }

            if (s.ActiveLine == null && !s.Recording && !s.RecordingBoundary)
                canvas.DrawText("No line: press A, drive, press B", Width / 2f, Height * 0.2f, _hint);
        }

        /// <summary>Satellite map tiles under everything else; tiles not loaded yet are simply left out.</summary>
        private void DrawMap(Canvas canvas, GuidanceSnapshot s, GeoPoint v, double heading, float scale, float cx, float cy)
        {
            MapTileProvider map = Map;
            if (map == null || s.Fix == null || !s.Fix.HasPosition || s.DrawPlane == null) return;

            double lat = s.Fix.Latitude, lon = s.Fix.Longitude;
            int zoom = MapTiles.ZoomFor(1.0 / scale, lat);
            double radius = Math.Sqrt((double)Width * Width + (double)Height * Height) / scale * 0.75;
            double dLat = radius / 111132.0, dLon = radius / (111320.0 * Math.Cos(lat * Math.PI / 180.0));
            List<TileId> tiles = MapTiles.TilesFor(lat - dLat, lon - dLon, lat + dLat, lon + dLon, zoom);
            if (tiles.Count > 80) return; // far zoomed out: not worth it

            canvas.Save();
            canvas.Translate(cx, cy);
            canvas.Rotate((float)-heading);
            foreach (TileId t in tiles)
            {
                Bitmap bmp = map.Get(t);
                if (bmp == null) continue;
                MapTiles.ToLatLon(t.X, t.Y, t.Z, out double nLat, out double wLon);
                MapTiles.ToLatLon(t.X + 1, t.Y + 1, t.Z, out double sLat, out double eLon);
                GeoPoint nw = s.DrawPlane.ToLocal(nLat, wLon), se = s.DrawPlane.ToLocal(sLat, eLon);
                var dst = new RectF(
                    (float)((nw.Easting - v.Easting) * scale), (float)(-(nw.Northing - v.Northing) * scale),
                    (float)((se.Easting - v.Easting) * scale) + 0.5f, (float)(-(se.Northing - v.Northing) * scale) + 0.5f);
                canvas.DrawBitmap(bmp, null, dst, _mapPaint);
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

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
        private readonly Paint _hint = new Paint(PaintFlags.AntiAlias) { Color = Color.ParseColor("#B0BEC5"), TextAlign = Paint.Align.Center };
        private GuidanceSnapshot _snap;
        private float _density;

        public GuidanceView(Context context) : base(context)
        {
            _density = context.Resources.DisplayMetrics.Density;
            _hint.TextSize = global::Android.Util.TypedValue.ApplyDimension(global::Android.Util.ComplexUnitType.Sp, 18, context.Resources.DisplayMetrics);
            foreach (Paint p in new[] { _current, _other, _refLine, _recording, _boundary, _boundaryRec, _vehicleEdge }) p.StrokeWidth *= _density;
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

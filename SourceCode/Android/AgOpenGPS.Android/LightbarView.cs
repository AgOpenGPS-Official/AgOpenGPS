using System;
using Android.Content;
using Android.Graphics;
using Android.Views;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Lightbar: a row of segments either side of a centre mark. Segments light from the centre
    /// towards the side to steer to, one per <see cref="DegreesPerSegment"/> of steer angle to the
    /// goal point: green near the centre, then yellow, then red.
    /// </summary>
    public sealed class LightbarView : View
    {
        public const int SegmentsPerSide = 10;
        public const double DegreesPerSegment = 1.0;

        private static Color Off => Color.ParseColor(Palette.LightbarOff);
        private static readonly Color Green = Color.ParseColor("#66BB6A");
        private static readonly Color Yellow = Color.ParseColor("#FFCA28");
        private static readonly Color Red = Color.ParseColor("#EF5350");

        private readonly Paint _paint = new Paint(PaintFlags.AntiAlias);
        private double _steer = double.NaN;
        private bool _dim;

        public LightbarView(Context context) : base(context) { }

        /// <summary>Steer angle in degrees (positive = right), NaN for no indication.
        /// <paramref name="dim"/> greys the bar out (no RTK: the indication is not reliable).</summary>
        public void Show(double steerDegrees, bool dim)
        {
            if (steerDegrees.Equals(_steer) && dim == _dim) return;
            _steer = steerDegrees;
            _dim = dim;
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawColor(Color.ParseColor(Palette.LightbarBackground));

            int total = SegmentsPerSide * 2 + 1;
            float gap = Height * 0.12f;
            float w = (Width - gap * (total + 1)) / total;
            float top = gap, bottom = Height - gap;

            int lit = double.IsNaN(_steer) ? -1 : (int)Math.Min(SegmentsPerSide, Math.Round(Math.Abs(_steer) / DegreesPerSegment));
            bool right = _steer > 0;

            for (int i = 0; i < total; i++)
            {
                int fromCentre = i - SegmentsPerSide; // -10..10, negative = left
                Color c = Off;
                if (lit >= 0)
                {
                    if (fromCentre == 0) c = lit == 0 ? Green : Off;
                    else if ((fromCentre > 0) == right && Math.Abs(fromCentre) <= lit) c = SegmentColor(Math.Abs(fromCentre));
                }
                if (_dim && c != Off) c = Color.ParseColor("#78909C");

                _paint.Color = c;
                float x = gap + i * (w + gap);
                float inset = fromCentre == 0 ? 0 : Height * 0.08f; // the centre segment stands out
                canvas.DrawRoundRect(x, top + inset, x + w, bottom - inset, w * 0.2f, w * 0.2f, _paint);
            }

            // Centre mark.
            _paint.Color = Color.White;
            float cx = gap + SegmentsPerSide * (w + gap) + w / 2;
            canvas.DrawRect(cx - 1.5f, 0, cx + 1.5f, gap * 0.8f, _paint);
            canvas.DrawRect(cx - 1.5f, Height - gap * 0.8f, cx + 1.5f, Height, _paint);
        }

        private static Color SegmentColor(int n) => n <= 3 ? Green : n <= 6 ? Yellow : Red;
    }
}

using System;
using Android.Content;
using Android.Graphics;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AgOpenGPS.Droid
{
    /// <summary>Small helpers for building screens in code.</summary>
    internal static class Ui
    {
        public static int Dp(Context c, float v) => (int)(v * c.Resources.DisplayMetrics.Density + 0.5f);

        public static TextView Text(Context c, float sp, bool bold = false, string color = "#212121")
        {
            var t = new TextView(c);
            t.SetTextSize(ComplexUnitType.Sp, sp);
            t.SetTextColor(Color.ParseColor(color));
            if (bold) t.SetTypeface(null, TypefaceStyle.Bold);
            t.SetPadding(0, Dp(c, 4), 0, Dp(c, 4));
            return t;
        }

        public static TextView Heading(Context c, string text)
        {
            TextView t = Text(c, 18, true);
            t.Text = text;
            t.SetPadding(0, Dp(c, 20), 0, Dp(c, 4));
            return t;
        }

        public static TextView Hint(Context c, string text)
        {
            TextView t = Text(c, 13, false, "#757575");
            t.Text = text;
            return t;
        }

        public static Button Button(Context c, string text, Action onClick, float minHeightDp = 56)
        {
            var b = new Button(c) { Text = text };
            b.SetMinHeight(Dp(c, minHeightDp));
            b.SetAllCaps(false);
            b.SetTextSize(ComplexUnitType.Sp, 18);
            b.Click += (s, e) => onClick();
            return b;
        }

        public static EditText Field(Context c, LinearLayout parent, string label, InputTypes type)
        {
            TextView l = Text(c, 13, false, "#616161");
            l.Text = label;
            l.SetPadding(0, Dp(c, 8), 0, 0);
            parent.AddView(l);
            var e = new EditText(c) { InputType = type };
            e.SetSingleLine(true);
            parent.AddView(e);
            return e;
        }

        public static LinearLayout Card(Context c)
        {
            var l = new LinearLayout(c) { Orientation = Orientation.Vertical };
            l.SetBackgroundColor(Color.White);
            l.SetPadding(Dp(c, 16), Dp(c, 4), Dp(c, 16), Dp(c, 16));
            return l;
        }

        public static LinearLayout.LayoutParams Weight(float weight, int height = ViewGroup.LayoutParams.WrapContent)
            => new LinearLayout.LayoutParams(0, height, weight);

        public static string FixColor(int quality)
        {
            switch (quality)
            {
                case 4: return "#2E7D32"; // RTK fix: green
                case 5: return "#EF6C00"; // RTK float: orange
                case 2: return "#1565C0"; // DGPS: blue
                case 0: return "#C62828"; // no fix: red
                default: return "#6D4C41";
            }
        }
    }
}

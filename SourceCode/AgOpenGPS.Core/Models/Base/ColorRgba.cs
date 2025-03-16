using System;

namespace AgOpenGPS.Core.Models
{
    public struct ColorRgba
    {
        public ColorRgba(byte red, byte green, byte blue, byte alpha)
        {
            Red = red;
            Green = green;
            Blue = blue;
            Alpha = alpha;
        }

        public ColorRgba(ColorRgb colorRgb, float alpha)
        {
            if (alpha < 0.0f || 1.0f < alpha) throw new ArgumentOutOfRangeException(nameof(alpha), "Argument out of range");
            Red = colorRgb.Red;
            Green = colorRgb.Green;
            Blue = colorRgb.Blue;
            Alpha = ColorRgba.FloatToByte(alpha);
        }

        public ColorRgba(float red, float green, float blue, float alpha)
        {
            if (red < 0.0f || 1.0f < red) throw new ArgumentOutOfRangeException(nameof(red), "Argument out of range");
            if (green < 0.0f || 1.0f < green) throw new ArgumentOutOfRangeException(nameof(green), "Argument out of range");
            if (blue < 0.0f || 1.0f < blue) throw new ArgumentOutOfRangeException(nameof(blue), "Argument out of range");
            if (alpha < 0.0f || 1.0f < alpha) throw new ArgumentOutOfRangeException(nameof(alpha), "Argument out of range");
            Red = ColorRgba.FloatToByte(red);
            Green = ColorRgba.FloatToByte(green);
            Blue = ColorRgba.FloatToByte(blue);
            Alpha = ColorRgba.FloatToByte(alpha);
        }

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }
        public byte Alpha { get; }

        public static explicit operator System.Drawing.Color(ColorRgba color)
        {
            return System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static explicit operator ColorRgba(System.Drawing.Color color)
        {
            return new ColorRgba(color.R, color.G, color.B, color.A);
        }

        static private byte FloatToByte(float fraction)
        {
            return (byte)(255 * fraction);
        }

    }

}

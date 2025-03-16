using System;

namespace AgOpenGPS.Core.Models
{
    public struct ColorRgb
    {
        public ColorRgb(byte red, byte green, byte blue)
        {
            Red = red;
            Green = green;
            Blue = blue;
        }

        public ColorRgb(float red, float green, float blue)
        {
            if (red < 0.0f || 1.0f < red) throw new ArgumentOutOfRangeException(nameof(red), "Argument out of range");
            if (green < 0.0f || 1.0f < green) throw new ArgumentOutOfRangeException(nameof(green), "Argument out of range");
            if (blue < 0.0f || 1.0f < blue) throw new ArgumentOutOfRangeException(nameof(blue), "Argument out of range");
            Red = ColorRgb.FloatToByte(red);
            Green = ColorRgb.FloatToByte(green);
            Blue = ColorRgb.FloatToByte(blue);
        }

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public static explicit operator System.Drawing.Color(ColorRgb color)
        {
            return System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue);
        }

        public static explicit operator ColorRgb(System.Drawing.Color color)
        {
            return new ColorRgb(color.R, color.G, color.B);
        }

        static private byte FloatToByte(float fraction)
        {
            return (byte)(255 * fraction);
        }
    }

}

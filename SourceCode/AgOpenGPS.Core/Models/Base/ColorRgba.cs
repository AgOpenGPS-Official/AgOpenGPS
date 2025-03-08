using OpenTK.Graphics.OpenGL;
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

        public ColorRgba(float red, float green, float blue, float alpha) :
            this(
                ColorRgba.FloatToByte(red),
                ColorRgba.FloatToByte(green),
                ColorRgba.FloatToByte(blue),
                ColorRgba.FloatToByte(alpha))
        {
            if (red < 0.0f || 255.0 < red) throw new ArgumentOutOfRangeException(nameof(red), "Argument out of range");
            if (green < 0.0f || 255.0 < green) throw new ArgumentOutOfRangeException(nameof(green), "Argument out of range");
            if (blue < 0.0f || 255.0 < blue) throw new ArgumentOutOfRangeException(nameof(blue), "Argument out of range");
            if (alpha < 0.0f || 255.0 < alpha) throw new ArgumentOutOfRangeException(nameof(alpha), "Argument out of range");
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

﻿using OpenTK.Graphics.OpenGL;

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

        public ColorRgb(float red, float green, float blue) :
        this(
            ColorRgb.FloatToByte(red),
            ColorRgb.FloatToByte(green),
            ColorRgb.FloatToByte(blue))
        {
        }

        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }

        public void SetColor()
        {
            GL.Color3(Red, Green, Blue);
        }

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

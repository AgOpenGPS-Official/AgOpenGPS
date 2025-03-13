﻿using OpenTK.Graphics.OpenGL;
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

        public ColorRgba(ColorRgb colorRgb, float alpha) :
            this(
                colorRgb.Red,
                colorRgb.Green,
                colorRgb.Blue,
                ColorRgba.FloatToByte(alpha))
        {
        }

        public ColorRgba(float red, float green, float blue, float alpha) :
            this(
                ColorRgba.FloatToByte(red),
                ColorRgba.FloatToByte(green),
                ColorRgba.FloatToByte(blue),
                ColorRgba.FloatToByte(alpha))
        {
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
            if (fraction < 0.0f || 1.0f < fraction) throw new ArgumentOutOfRangeException(nameof(fraction), "Argument out of range");

            return (byte)(255 * fraction);
        }

    }

}

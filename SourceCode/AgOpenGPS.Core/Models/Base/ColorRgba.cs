using System;

namespace AgOpenGPS.Core.Models
{
    public struct ColorRgba
    {
        public ColorRgba(byte red, byte green, byte blue, byte alpha = 255)
        {
            ByteArray = new byte[4] { red, green, blue, alpha };
        }

        public ColorRgba(float red, float green, float blue, float alpha = 1.0f)
        {
            if (red < 0.0f || 1.0f < red) throw new ArgumentOutOfRangeException(nameof(red), "Argument out of range");
            if (green < 0.0f || 1.0f < green) throw new ArgumentOutOfRangeException(nameof(green), "Argument out of range");
            if (blue < 0.0f || 1.0f < blue) throw new ArgumentOutOfRangeException(nameof(blue), "Argument out of range");
            if (alpha < 0.0f || 1.0f < alpha) throw new ArgumentOutOfRangeException(nameof(alpha), "Argument out of range");
            ByteArray = new byte[4] { FloatToByte(red), FloatToByte(green), FloatToByte(blue), FloatToByte(alpha) };
        }

        // For better performance in GLW.SetColor()
        public byte[] ByteArray { get; private set; }

        public byte Red
        {
            get { return ByteArray[0]; }
            set { ByteArray[0] = value; }
        }

        public byte Green
        {
            get { return ByteArray[1]; }
            set { ByteArray[1] = value; }
        }

        public byte Blue
        {
            get { return ByteArray[2]; }
            set { ByteArray[2] = value; }
        }

        public byte Alpha
        {
            get { return ByteArray[3]; }
            set { ByteArray[3] = value; }
        }

        public static explicit operator System.Drawing.Color(ColorRgba color)
        {
            return System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static explicit operator ColorRgba(System.Drawing.Color color)
        {
            return new ColorRgba(color.R, color.G, color.B, color.A);
        }

        private static byte FloatToByte(float fraction)
        {
            return (byte)(255 * fraction);
        }

    }

}

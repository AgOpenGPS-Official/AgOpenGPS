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

        public byte Red { get; }
        public byte Green { get; }
        public byte Blue { get; }

        public static explicit operator System.Drawing.Color(ColorRgb color)
        {
            return System.Drawing.Color.FromArgb(color.Red, color.Green, color.Blue);
        }

        public static explicit operator ColorRgb(System.Drawing.Color color)
        {
            return new ColorRgb(color.R, color.G, color.B);
        }
    }
}

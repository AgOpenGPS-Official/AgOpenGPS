using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.DrawLib
{
    public class PointStyle
    {
        public PointStyle(float size, ColorRgba colorRgba)
        {
            Size = size;
            Color = colorRgba;
        }

        public float Size { get; set; }
        public ColorRgba Color { get; set; }
    }
}

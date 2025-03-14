using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.DrawLib
{
    public class PointStyle
    {
        public PointStyle(float size, ColorRgb colorRgb)
        {
            Size = size;
            Color = colorRgb;
        }

        public float Size { get; set; }
        public ColorRgb Color { get; set; }
    }
}

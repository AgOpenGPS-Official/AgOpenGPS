using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.DrawLib
{
    public class PointStyle
    {
        public PointStyle(float pointSize, ColorRgba colorRgba)
        {
            PointSize = pointSize;
            Color = colorRgba;
        }

        public float PointSize { get; set; }
        public ColorRgba Color { get; set; }
    }
}

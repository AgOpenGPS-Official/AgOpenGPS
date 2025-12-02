using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.DrawLib
{
    public class LineStyle
    {
        public LineStyle(float width, ColorRgba colorRgba)
        {
            Width = width;
            Color = colorRgba;
        }

        public float Width { get; set; }
        public ColorRgba Color { get; set; }
    }
}

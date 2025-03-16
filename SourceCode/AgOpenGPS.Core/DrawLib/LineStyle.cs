using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.DrawLib
{
    public class LineStyle
    {
        public LineStyle(float width, ColorRgb colorRgb)
        {
            Width = width;
            Color = colorRgb;
        }

        public float Width { get; set; }
        public ColorRgb Color { get; set; }
    }
}

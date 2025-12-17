using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Visuals
{
    public static class FenceLineVisual
    {
        private static ColorRgba normalColor = new ColorRgba(0xff9ea1a1);
        private static ColorRgba selectedColor = Colors.White;

        public static void DrawFenceLine(GeoCoord[] fenceLineEar, bool isSelected)
        {
            GLW.SetLineWidth(3.0f);
            if (isSelected)
            {
                GLW.SetColor(selectedColor);
            }
            else
            {
                GLW.SetColor(normalColor);
            }
            GLW.DrawLineLoopPrimitive(fenceLineEar);
        }
    }
}

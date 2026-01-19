using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Visuals
{
    public static class TouchPointsLineVisual
    {
        private static readonly ColorRgba backgroundColor = Colors.Black;
        private static readonly ColorRgba pointAColor = new ColorRgba(0.950f, 0.75f, 0.50f);
        private static readonly ColorRgba pointBColor = new ColorRgba(0.5f, 0.5f, 0.935f);
        private static readonly ColorRgba pointCColor = new ColorRgba(0.95f, 0.95f, 0.35f);
        private static readonly ColorRgba lineOrange = new ColorRgba(0.90f, 0.5f, 0.25f);

        public static void DrawTouchPoints(GeoCoord? coordA, GeoCoord? coordB, GeoCoord? coordC = null)
        {
            // Draw backgrounds first. Looks better when points overlap.
            List<GeoCoord> backGrounds = new List<GeoCoord>();
            if (coordA.HasValue) { backGrounds.Add(coordA.Value); }
            if (coordB.HasValue) { backGrounds.Add(coordB.Value); }
            if (coordC.HasValue) { backGrounds.Add(coordC.Value); }

            // background layer
            GLW.SetPointSize(24.0f);
            GLW.SetColor(backgroundColor);
            GLW.DrawPointsPrimitive(backGrounds.ToArray());

            // foreground layer
            GLW.SetPointSize(16.0f);
            if (coordA.HasValue)
            {
                GLW.SetColor(pointAColor);
                GLW.DrawPoint(coordA.Value);
            }
            if (coordB.HasValue)
            {
                GLW.SetColor(pointBColor);
                GLW.DrawPoint(coordB.Value);
            }
            if (coordC.HasValue)
            {
                GLW.SetColor(pointCColor);
                GLW.DrawPoint(coordC.Value);
            }
        }

        public static void DrawTouchPointsLine(GeoCoord? coordA, GeoCoord? coordB, GeoCoord? coordC = null)
        {
            DrawTouchPoints(coordA, coordB, coordC);
            if (coordA.HasValue && coordB.HasValue)
            {
                GLW.SetLineWidth(4.0f);
                GLW.SetColor(lineOrange);
                List<GeoCoord> orangeLineStrip = new List<GeoCoord> { coordA.Value, coordB.Value };

                if (coordC.HasValue) orangeLineStrip.Insert(1, coordC.Value);
                GLW.DrawLineStripPrimitive(orangeLineStrip.ToArray());
            }
        }
    }
}

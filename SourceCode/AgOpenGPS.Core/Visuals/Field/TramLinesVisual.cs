using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Visuals
{
    public class TramLinesVisual
    {
        private readonly LineLoopRenderer _outerBoundaryRenderer;
        private readonly LineLoopRenderer _innerBoundaryRenderer;
        private readonly List<LineStripRenderer> _fillTrackRenderers;

        public TramLinesVisual()
        {
            _outerBoundaryRenderer = new LineLoopRenderer();
            _innerBoundaryRenderer = new LineLoopRenderer();
            _fillTrackRenderers = new List<LineStripRenderer>();
        }

        public void UpdateBoundaryTracks(
            GeoCoord[] outerBoundaryVertices,
            GeoCoord[] innerBoundaryVertices)
        {
            _outerBoundaryRenderer.UpdateVertices(outerBoundaryVertices);
            _innerBoundaryRenderer.UpdateVertices(innerBoundaryVertices);
        }


        public void DrawTramLinesLayered(
            bool drawFillLines,
            bool drawBoundary,
            LineStyle backgroundLineStyle,
            LineStyle foregroundLineStyle)
        {
            DrawTramLines(
                drawFillLines,
                drawBoundary,
                backgroundLineStyle);
            DrawTramLines(
                drawFillLines,
                drawBoundary,
                foregroundLineStyle);
        }

        public void DrawTramLines(
            bool drawFillLines,
            bool drawBoundary,
            LineStyle lineStyle)
        {
            GLW.SetLineWidth(lineStyle.Width);
            GLW.SetColor(lineStyle.Color);
            if (drawFillLines)
            {
                foreach (LineStripRenderer fillTrackRenderer in _fillTrackRenderers)
                {
                    fillTrackRenderer.Draw();
                }
            }
            if (drawBoundary)
            {
                _outerBoundaryRenderer.Draw();
                _innerBoundaryRenderer.Draw();
            }
        }

    }

}

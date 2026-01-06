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

        public void UpdateFillTracks(
            List<GeoCoord[]> fillTracks)
        {
            // Remove and dispose excess renderers
            int nTracks = fillTracks != null ? fillTracks.Count : 0;
            if (nTracks < _fillTrackRenderers.Count)
            {
                for (int i = nTracks; i < _fillTrackRenderers.Count; i++)
                {
                    _fillTrackRenderers[i].Dispose();
                }
                _fillTrackRenderers.RemoveRange(nTracks, _fillTrackRenderers.Count - nTracks);
            }
            // Update renderers
            if (fillTracks != null)
            {
                for (int i = 0; i < fillTracks.Count; i++)
                {
                    if (i >= _fillTrackRenderers.Count)
                    {
                        _fillTrackRenderers.Add(new LineStripRenderer());
                    }
                    _fillTrackRenderers[i].UpdateVertices(fillTracks[i]);
                }
            }
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

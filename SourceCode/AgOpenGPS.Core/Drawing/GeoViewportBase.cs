using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using System;

namespace AgOpenGPS.Core.Drawing
{
    // This is a rudimentary version that only support an orthogonal view.
    // Possible future improvements:
    // - replace the perspective field of view with orthogonal projection
    // - also support perspective view
    // - embed this viewport in a reusable ViewportControl that takes care of creation,
    //   initialization, resizing,etc of the viewport
    // - support animations when the pan or zoom changes
    // - support transparent overlays with overlay buttons

    public abstract class GeoViewportBase
    {
        private const double PanStep = 0.15;
        private const double ZoomStep = 2.0;
        private const double MinZoom = 0.015625;
        private const double MaxZoom = 1.0;

        public event EventHandler<GeoCoord> MouseDownEventHandler;

        public GeoViewportBase(GeoBoundingBox boundingBox)
        {
            SetBoundingBox(boundingBox);
            ResetZoomPan();
        }

        public static void Initialize()
        {
            GLW.EnableCullFace();
            GLW.SetCullFaceModeBack();
            GLW.SetClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public void Resize(int width, int height)
        {
            MakeCurrent();
            GLW.Viewport(width, height);
            GLW.CreatePerspectiveFieldOfView();
        }

        public abstract void Refresh();
        protected abstract void MakeCurrent();

        public GeoBoundingBox BoundingBox { get; private set; }
        private double BoundingBoxWidth => BoundingBox.MaxEasting - BoundingBox.MinEasting;
        private double BoundingBoxHeight => BoundingBox.MaxNorthing - BoundingBox.MinNorthing;
        private double BoundingBoxDistance => Math.Max(BoundingBoxWidth, BoundingBoxHeight);

        public double Zoom { get; private set; }
        public GeoCoord ViewportCenter { get; set; }

        public abstract XyDelta ViewportSize { get; }

        private double scaleBoundingBoxToViewport;

        public void SetBoundingBox(GeoBoundingBox boundingBox)
        {
            BoundingBox = boundingBox;
            ViewportCenter = BoundingBox.CenterCoord;
        }

        public void BeginPaint()
        {
            MakeCurrent();
            GLW.ClearColorAndDepthBuffer();
            GLW.LoadIdentity();

            XyDelta viewportSize = ViewportSize;
            GeoDelta boundingBoxSize = BoundingBox.MaxCoord - BoundingBox.MinCoord;
            double scaleEastingToX = viewportSize.DeltaX / boundingBoxSize.EastingDelta;
            double scaleNorthingToY = viewportSize.DeltaY / boundingBoxSize.NorthingDelta;
            scaleBoundingBoxToViewport = Math.Min(scaleEastingToX, scaleNorthingToY);

            GLW.Translate(0, 0, -BoundingBoxDistance * Zoom);
            GLW.Translate(-ViewportCenter.Easting, -ViewportCenter.Northing, 0);
        }

        public virtual void EndPaint()
        {
            GLW.Flush();
        }

        public GeoCoord GetGeoCoord(XyCoord xyClientCoord)
        {
            double scaleViewportToBoundingBox = 1.0 / scaleBoundingBoxToViewport;
            XyCoord xyCenteredCoord = xyClientCoord - 0.5 * ViewportSize;
            // Magic number 0.903
            GeoDelta delta = (scaleViewportToBoundingBox / Zoom / 0.903) * new GeoDelta(-xyCenteredCoord.Y, xyCenteredCoord.X);
            return ViewportCenter + delta;
        }

        public void ResetZoomPan()
        {
            Zoom = 1.0;
            ViewportCenter = BoundingBox.CenterCoord;
            Refresh();
        }

        public void ZoomInStep()
        {
            Zoom = Math.Max(Zoom / ZoomStep, MinZoom);
            Refresh();
        }

        public void ZoomOutStep()
        {
            Zoom = Math.Min(Zoom * ZoomStep, MaxZoom);
            Refresh();
        }

        public void PanRight()
        {
            ViewportCenter += PanStep * Zoom * new GeoDelta(0.0, BoundingBoxWidth);
            Refresh();
        }

        public void PanLeft()
        {
            ViewportCenter -= PanStep * Zoom * new GeoDelta(0.0, BoundingBoxWidth);
            Refresh();
        }

        public void PanDown()
        {
            ViewportCenter -= PanStep * Zoom * new GeoDelta(BoundingBoxHeight, 0.0);
            Refresh();
        }

        public void PanUp()
        {
            ViewportCenter += PanStep * Zoom * new GeoDelta(BoundingBoxHeight, 0.0);
            Refresh();
        }

        public void PointZoom(GeoCoord newViewportCenter, double zoom)
        {
            ViewportCenter = newViewportCenter;
            Zoom = zoom;
            Refresh();
        }

        protected void OnMouseDown(GeoCoord mouseDownCoord)
        {
            MouseDownEventHandler?.Invoke(this, mouseDownCoord);
            Refresh();
        }
    }

}

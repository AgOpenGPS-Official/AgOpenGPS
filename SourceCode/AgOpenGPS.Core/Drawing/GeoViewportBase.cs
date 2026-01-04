using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using System;

namespace AgOpenGPS.Core.Drawing
{
    // This is a rudimentary version that only support an orthogonal view.
    // Possible future improvements:
    // - also support perspective view
    // - embed this viewport in a reusable ViewportControl that takes care of creation,
    //   initialization, resizing,etc of the viewport
    // - support animations when the pan or zoom changes
    // - support transparent overlays with overlay buttons

    public abstract class GeoViewportBase
    {
        private const double PanStep = 0.15;        // Fraction of viewport size
        private const double ZoomStep = 1.4;
        private const double MinZoom = 0.015625;
        private const double MaxZoom = 1.0;

        public event EventHandler<GeoCoord> MouseDownEventHandler;

        public GeoViewportBase()
        {
            ResetZoomPan();
        }

        public static void Initialize()
        {
            GLW.EnableCullFace();
            GLW.SetCullFaceModeBack();
            GLW.SetClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        public void SizeChanged()
        {
            MakeCurrent();
            UpdateProjection();
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

        private double projectedRectToViewportFactor;

        public void SetBoundingBox(GeoBoundingBox boundingBox)
        {
            BoundingBox = boundingBox;
            ViewportCenter = BoundingBox.CenterCoord;
            UpdateProjection();
        }

        public void BeginPaint()
        {
            MakeCurrent();
            GLW.ClearColorAndDepthBuffer();
            GLW.LoadIdentity();

            UpdateProjection();

            GLW.Translate(0, 0, -100.0);
            // Without translation, the center of the bounding box is already at
            // the center of the viewport. See UpdateProjection()
            GeoDelta translation = ViewportCenter - BoundingBox.CenterCoord;
            GLW.Translate(-translation);
        }

        public virtual void EndPaint()
        {
            GLW.Flush();
        }

        public GeoCoord GetGeoCoord(XyCoord xyClientCoord)
        {
            double viewportToProjectedRectFactor = 1.0 / projectedRectToViewportFactor;
            XyCoord xyCenteredCoord = xyClientCoord - 0.5 * ViewportSize;
            GeoDelta delta = viewportToProjectedRectFactor * new GeoDelta(-xyCenteredCoord.Y, xyCenteredCoord.X);
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

        private void UpdateProjection()
        {
            if (BoundingBox.IsEmpty) return;
            XyDelta viewportSize = ViewportSize;
            GeoDelta boundingBoxSize = BoundingBox.MaxCoord - BoundingBox.MinCoord;
            double scaleEastingToX = viewportSize.DeltaX / boundingBoxSize.EastingDelta;
            double scaleNorthingToY = viewportSize.DeltaY / boundingBoxSize.NorthingDelta;
            // Compute a rectangle with an aspect ratio that equals the viewport aspect ratio,
            // otherwise the content will be stretched
            GeoBoundingBox projectedRect;
            if (scaleEastingToX < scaleNorthingToY)
            {
                double resizeNorthingFactor = scaleNorthingToY / scaleEastingToX;
                projectedRect = BoundingBox.Scaled(Zoom * resizeNorthingFactor, Zoom);
            }
            else
            {
                double resizeEastingFactor = scaleEastingToX / scaleNorthingToY;
                projectedRect = BoundingBox.Scaled(Zoom, Zoom * resizeEastingFactor);
            }
            GLW.CreateOrthoProjection((int)ViewportSize.DeltaX, (int)ViewportSize.DeltaY, projectedRect);
            projectedRectToViewportFactor = Math.Min(scaleEastingToX, scaleNorthingToY) / Zoom;
        }
    }

}

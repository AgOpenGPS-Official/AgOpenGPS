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
        }

        public void Initialize(GeoBoundingBox boundingBox)
        {
            MakeCurrent();
            SetBoundingBox(boundingBox);
            ResetZoomPan();
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

        public abstract XyDeltaInt ViewportSize { get; }

        private GeoDelta DefaultDisplayedSize { get; set; }
        private double DefaultBoundingBoxToViewportScale { get; set; }

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
            GLW.Translate(0, 0, -100.0);
        }

        public virtual void EndPaint()
        {
            GLW.Flush();
        }

        public GeoCoord GetGeoCoord(XyCoord xyClientCoord)
        {
            double viewportToZoomedPannedBoundingBox = Zoom / DefaultBoundingBoxToViewportScale;
            XyCoord xyCenteredCoord = xyClientCoord - 0.5 * ViewportSize;
            GeoDelta delta = viewportToZoomedPannedBoundingBox * new GeoDelta(-xyCenteredCoord.Y, xyCenteredCoord.X);
            return ViewportCenter + delta;
        }

        public void ResetZoomPan()
        {
            Zoom = 1.0;
            ViewportCenter = BoundingBox.CenterCoord;
            UpdateProjectionForZoomPan();
        }

        public void ZoomInStep()
        {
            Zoom = Math.Max(Zoom / ZoomStep, MinZoom);
            UpdateProjectionForZoomPan();
        }

        public void ZoomOutStep()
        {
            Zoom = Math.Min(Zoom * ZoomStep, MaxZoom);
            UpdateProjectionForZoomPan();
        }

        public void PanRight()
        {
            ViewportCenter += PanStep * Zoom * new GeoDelta(0.0, BoundingBoxWidth);
            UpdateProjectionForZoomPan();
        }

        public void PanLeft()
        {
            ViewportCenter -= PanStep * Zoom * new GeoDelta(0.0, BoundingBoxWidth);
            UpdateProjectionForZoomPan();
        }

        public void PanDown()
        {
            ViewportCenter -= PanStep * Zoom * new GeoDelta(BoundingBoxHeight, 0.0);
            UpdateProjectionForZoomPan();
        }

        public void PanUp()
        {
            ViewportCenter += PanStep * Zoom * new GeoDelta(BoundingBoxHeight, 0.0);
            UpdateProjectionForZoomPan();
        }

        public void PointZoom(GeoCoord newViewportCenter, double zoom)
        {
            ViewportCenter = newViewportCenter;
            Zoom = zoom;
            UpdateProjectionForZoomPan();
        }

        protected void OnMouseDown(GeoCoord mouseDownCoord)
        {
            MouseDownEventHandler?.Invoke(this, mouseDownCoord);
            Refresh();
        }

        private void UpdateProjection()
        {
            UpdateDefaultDisplayedSize();
            UpdateProjectionForZoomPan();
        }

        // Compute the DefaultDisplayedSize: the size of the smallest rectangluar region
        // that includes the whole BoundingBox and has been extended either horizontally
        // or vertically to match the aspect ratio of the viewport. This is also the size
        // of the rectangle that would be displayed if Pan is 0 and Zoom is 1
        private void UpdateDefaultDisplayedSize()
        {
            //if (BoundingBox.IsEmpty) return;
            XyDeltaInt viewportSize = ViewportSize;
            GeoDelta boundingBoxSize = BoundingBox.MaxCoord - BoundingBox.MinCoord;
            double scaleEastingToX = viewportSize.DeltaX / boundingBoxSize.EastingDelta;
            double scaleNorthingToY = viewportSize.DeltaY / boundingBoxSize.NorthingDelta;
            // Compute a rectangle with an aspect ratio that equals the viewport aspect ratio,
            // otherwise the content will be stretched
            if (scaleEastingToX < scaleNorthingToY)
            {
                double extendNorthingFactor = scaleNorthingToY / scaleEastingToX;
                DefaultDisplayedSize = new GeoDelta(
                    boundingBoxSize.NorthingDelta * extendNorthingFactor,
                    boundingBoxSize.EastingDelta);
            }
            else
            {
                double extendEastingFactor = scaleEastingToX / scaleNorthingToY;
                DefaultDisplayedSize = new GeoDelta(
                    boundingBoxSize.NorthingDelta,
                    boundingBoxSize.EastingDelta * extendEastingFactor);
            }
            DefaultBoundingBoxToViewportScale = Math.Min(scaleEastingToX, scaleNorthingToY);
        }

        private void UpdateProjectionForZoomPan()
        {
            GeoDelta halfZoomedSize = 0.5 * Zoom * DefaultDisplayedSize;
            GeoBoundingBox ZoomedPannedBoundingBox = new GeoBoundingBox(
                ViewportCenter - halfZoomedSize,
                ViewportCenter + halfZoomedSize);
            MakeCurrent();
            GLW.CreateOrthoProjection(ViewportSize, ZoomedPannedBoundingBox);
            Refresh();
        }
    }

}

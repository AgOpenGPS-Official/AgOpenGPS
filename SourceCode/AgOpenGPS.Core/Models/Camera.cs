using AgOpenGPS.Core.DrawLib;
using System;
using System.Windows.Media.Media3D;

namespace AgOpenGPS.Core
{
    public class Camera
    {
        private double _zoomValue;
        public Camera(double pitchInDegrees, double zoomValue)
        {
            PitchInDegrees = pitchInDegrees;
            ZoomValue = zoomValue;
            DistanceToLookAt = 0.5 * 75.0;
            FollowDirectionHint = true;
        }

        public double PitchInDegrees { get; set; } // 0.0 is vertical downwards -90.0 is horizontal

        // Beware: bigger values mean more zoomed out!
        public double ZoomValue
        {
            get { return _zoomValue; }
            private set
            {
                _zoomValue = value;
                DistanceToLookAt = 0.5 * _zoomValue * _zoomValue;
            }
        }

        public double DistanceToLookAt { get; set; }
        public double PanX { get; set; }
        public double PanY { get; set; }
        public bool FollowDirectionHint { get; set; }

        // Deprecated: Only here to avoid numerous changes to existing code.
        // Beware: the name suggests that this always has a positive value, but it is always negative!
        // Please use DistanceToLookAt instead
        public double camSetDistance => -2.0 * DistanceToLookAt;

        public void SetLookAt(double lookAtX, double lookAtY, double directionHintInDegrees)
        {
            //back the camera up
            GLW.Translate(0, 0, -DistanceToLookAt);

            GLW.RotateX(PitchInDegrees);
            GLW.Translate(PanX, PanY);

            if (FollowDirectionHint)
            {
                GLW.RotateZ(directionHintInDegrees);
            }
            GLW.Translate(-lookAtX, -lookAtY, 0.0);
        }

        // Small steps for accurate zooming (with mousewheel)
        public void ZoomInSmallStep()
        {
            Zoom(ZoomValue <= 20 ? 0.94 : 0.98);
        }

        public void ZoomOutSmallStep()
        {
            Zoom(ZoomValue <= 20 ? 1.06 : 1.02);
        }

        public void ZoomIn()
        {
            Zoom(ZoomValue <= 20 ? 0.8 : 0.9);
        }

        public void ZoomOut()
        {
            Zoom(ZoomValue <= 20 ? 1.2 : 1.1);
        }

        private void Zoom(double adjustFactor)
        {
            double zoomFactor = ZoomValue * adjustFactor;
            zoomFactor = Math.Max(4.0, zoomFactor);
            zoomFactor = Math.Min(zoomFactor, 180.0);
            ZoomValue = zoomFactor;
        }
    }
}
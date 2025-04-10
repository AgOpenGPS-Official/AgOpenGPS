using AgOpenGPS.Core.DrawLib;

namespace AgOpenGPS
{
    public class CCamera
    {
        public double camSetDistance = -75;

        public double zoomValue;

        public double camSmoothFactor;

        public CCamera()
        {
            //get the pitch of camera from settings
            PitchInDegrees = Properties.Settings.Default.setDisplay_camPitch;
            zoomValue = Properties.Settings.Default.setDisplay_camZoom;
            FollowDirectionHint = true;
            camSmoothFactor = ((double)(Properties.Settings.Default.setDisplay_camSmooth) * 0.004) + 0.2;
        }

        public double PitchInDegrees { get; set; } // 0.0 is vertical downwards -90.0 is horizontal
        public double PanX { get; set; }
        public double PanY { get; set; }
        public bool FollowDirectionHint { get; set; }

        public void SetLookAt(double lookAtX, double lookAtY, double directionHintInDegrees)
        {
            //back the camera up
            GLW.Translate(0,0, camSetDistance * 0.5);

            GLW.RotateX(PitchInDegrees);
            GLW.Translate(PanX, PanY);

            if (FollowDirectionHint)
            {
                GLW.RotateZ(directionHintInDegrees);
            }
            GLW.Translate(-lookAtX, -lookAtY, 0.0);
        }
    }
}
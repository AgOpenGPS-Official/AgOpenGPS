using AgOpenGPS.Core.DrawLib;

namespace AgOpenGPS
{
    public class CCamera
    {
        public double camPitch;
        public double panX = 0;
        public double panY = 0;
        public double camSetDistance = -75;

        public double zoomValue;

        public bool camFollowing;
        public double camSmoothFactor;

        public CCamera()
        {
            //get the pitch of camera from settings
            camPitch = Properties.Settings.Default.setDisplay_camPitch;
            zoomValue = Properties.Settings.Default.setDisplay_camZoom;
            camFollowing = true;
            camSmoothFactor = ((double)(Properties.Settings.Default.setDisplay_camSmooth) * 0.004) + 0.2;
        }

        public void SetLookAt(double lookAtX, double lookAtY, double directionHintInDegrees)
        {
            //back the camera up
            GLW.Translate(0,0, camSetDistance * 0.5);

            //rotate the camera down to look at fix
            GLW.RotateX(camPitch);
            GLW.Translate(panX, panY);

            //following game style or N fixed cam
            if (camFollowing)
            {
                GLW.RotateZ(directionHintInDegrees);
            }
            GLW.Translate(-lookAtX, -lookAtY, 0.0);
        }
    }
}
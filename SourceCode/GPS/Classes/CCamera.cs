using AgOpenGPS.Core.DrawLib;

namespace AgOpenGPS
{
    public class CCamera
    {
        private double camPosX;
        private double camPosY;
        private readonly double camPosZ;

        private double camYaw;

        public double camPitch;
        public double panX = 0, panY = 0;
        public double camSetDistance = -75;

        public double zoomValue = 15;

        public bool camFollowing;
        public double camSmoothFactor;

        public CCamera()
        {
            //get the pitch of camera from settings
            camPitch = Properties.Settings.Default.setDisplay_camPitch;
            zoomValue = Properties.Settings.Default.setDisplay_camZoom;
            camPosZ = 0.0;
            camFollowing = true;
            camSmoothFactor = ((double)(Properties.Settings.Default.setDisplay_camSmooth) * 0.004) + 0.2;
        }

        public void SetWorldCam(double _fixPosX, double _fixPosY, double _fixHeading)
        {
            camPosX = _fixPosX;
            camPosY = _fixPosY;
            camYaw = _fixHeading;

            //back the camera up
            GLW.Translate(0,0, camSetDistance * 0.5);

            //rotate the camera down to look at fix
            GLW.RotateX(camPitch);

            GLW.Translate(panX, panY);

            //following game style or N fixed cam
            if (camFollowing)
            {
                GLW.RotateZ(camYaw);
            }
            GLW.Translate(-camPosX, -camPosY, -camPosZ);
        }
    }
}
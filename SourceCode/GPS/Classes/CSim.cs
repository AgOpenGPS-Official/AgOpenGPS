using System;

namespace AgOpenGPS
{
    public class CSim
    {
        private readonly FormGPS mf;

        #region properties sim

        public double altitude = 300;

        public double latitude, longitude;

        public double headingTrue, stepDistance = 0.0, steerAngle, steerangleAve = 0.0;
        public double steerAngleScrollBar = 0;

        public bool isAccelForward, isAccelBack;

        #endregion properties sim

        public CSim(FormGPS _f)
        {
            mf = _f;
            latitude = Properties.Settings.Default.setGPS_SimLatitude;
            longitude = Properties.Settings.Default.setGPS_SimLongitude;
        }

        public void DoSimTick(double _st)
        {
            steerAngle = _st;

            double diff = Math.Abs(steerAngle - steerangleAve);

            if (diff > 11)
            {
                if (steerangleAve >= steerAngle)
                {
                    steerangleAve -= 6;
                }
                else steerangleAve += 6;
            }
            else if (diff > 5)
            {
                if (steerangleAve >= steerAngle)
                {
                    steerangleAve -= 2;
                }
                else steerangleAve += 2;
            }
            else if (diff > 1)
            {
                if (steerangleAve >= steerAngle)
                {
                    steerangleAve -= 0.5;
                }
                else steerangleAve += 0.5;
            }
            else
            {
                steerangleAve = steerAngle;
            }

            mf.mc.actualSteerAngleDegrees = steerangleAve;

            double temp = stepDistance * Math.Tan(steerangleAve * 0.0165329252) / 2;
            headingTrue += temp;
            if (headingTrue > glm.twoPI) headingTrue -= glm.twoPI;
            if (headingTrue < 0) headingTrue += glm.twoPI;

            mf.pn.vtgSpeed = Math.Abs(Math.Round(4 * stepDistance * 10, 2));
            mf.pn.AverageTheSpeed();

            //Calculate the next Lat Long based on heading and distance
            CalculateNewPostionFromBearingDistance(glm.toRadians(latitude), glm.toRadians(longitude), headingTrue, stepDistance / 1000.0);

            mf.pn.ConvertWGS84ToLocal(latitude, longitude, out mf.pn.fix.northing, out mf.pn.fix.easting);

            mf.pn.headingTrue = mf.pn.headingTrueDual = glm.toDegrees(headingTrue);
            mf.ahrs.imuHeading = mf.pn.headingTrue;
            if (mf.ahrs.imuHeading >= 360) mf.ahrs.imuHeading -= 360;

            mf.pn.latitude = latitude;
            mf.pn.longitude = longitude;

            mf.pn.hdop = 0.7;

            temp = Math.Abs(mf.pn.latitude * 100);
            temp -= ((int)(temp));
            temp *= 100;
            mf.pn.altitude = temp+200;

            temp = Math.Abs(mf.pn.longitude * 100);
            temp -= ((int)(temp));
            temp *= 100;
            mf.pn.altitude += temp;
            
            mf.pn.satellitesTracked = 12;

            mf.sentenceCounter = 0;
            mf.UpdateFixPosition();

            if (isAccelForward)
            {
                isAccelBack = false;
                stepDistance += 0.02;
                if (stepDistance > 0.12) isAccelForward = false;
            }

            if (isAccelBack)
            {
                isAccelForward = false;
                stepDistance -= 0.01;
                if (stepDistance < -0.06) isAccelBack = false;
            }
        }

        public void CalculateNewPostionFromBearingDistance(double lat, double lng, double bearing, double distance)
        {
            double R = distance / 6371.0; // Earth Radius in Km

            double lat2 = Math.Asin((Math.Sin(lat) * Math.Cos(R)) + (Math.Cos(lat) * Math.Sin(R) * Math.Cos(bearing)));
            double lon2 = lng + Math.Atan2(Math.Sin(bearing) * Math.Sin(R) * Math.Cos(lat), Math.Cos(R) - (Math.Sin(lat) * Math.Sin(lat2)));

            latitude = glm.toDegrees(lat2);
            longitude = glm.toDegrees(lon2);
        }
    }
}
using AgOpenGPS.Core.Models;
using System;

namespace AgOpenGPS
{
    public class CSim
    {
        private readonly FormGPS mf;

        #region properties sim

        public double altitude = 300;

        public Wgs84 CurrentLatLon { get; set; }

        public double headingTrue, stepDistance = 0.0, steerAngle, steerangleAve = 0.0;
        public double steerAngleScrollBar = 0;

        public bool isAccelForward, isAccelBack;

        #endregion properties sim

        public CSim(FormGPS _f)
        {
            mf = _f;
            CurrentLatLon = new Wgs84(
                Properties.Settings.Default.setGPS_SimLatitude,
                Properties.Settings.Default.setGPS_SimLongitude);
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
            CurrentLatLon = CurrentLatLon.CalculateNewPostionFromBearingDistance(headingTrue, stepDistance);

            GeoCoord fixCoord = mf.pn.ConvertWgs84ToGeoCoord(CurrentLatLon);
            mf.pn.fix.northing = fixCoord.Northing;
            mf.pn.fix.easting = fixCoord.Easting;
            mf.pn.headingTrue = mf.pn.headingTrueDual = glm.toDegrees(headingTrue);
            mf.ahrs.imuHeading = mf.pn.headingTrue;
            if (mf.ahrs.imuHeading >= 360) mf.ahrs.imuHeading -= 360;

            mf.AppModel.CurrentLatLon = CurrentLatLon;

            mf.pn.hdop = 0.7;

            mf.pn.altitude = SimulateAltitude(mf.AppModel.CurrentLatLon);
            
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

        private double SimulateAltitude(Wgs84 latLon)
        {
            double temp = Math.Abs(latLon.Latitude * 100);
            temp -= ((int)(temp));
            temp *= 100;
            double altitude = temp + 200;

            temp = Math.Abs(latLon.Longitude * 100);
            temp -= ((int)(temp));
            temp *= 100;
            altitude += temp;
            return altitude;
        }

    }
}
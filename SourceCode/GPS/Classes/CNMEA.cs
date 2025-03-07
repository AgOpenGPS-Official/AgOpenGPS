using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.Text;

namespace AgOpenGPS
{
    public class CNMEA
    {
        
        //our current fix
        public vec2 fix = new vec2(0, 0);

        public vec2 prevSpeedFix = new vec2(0, 0);

        //other GIS Info
        public double altitude, speed, newSpeed, vtgSpeed = float.MaxValue;

        public double headingTrueDual, headingTrue, hdop, age, headingTrueDualOffset;

        public int fixQuality, ageAlarm;
        public int satellitesTracked;

        private readonly FormGPS mf;

        public CNMEA(FormGPS f)
        {
            //constructor, grab the main form reference
            mf = f;
            SharedFieldProperties = new SharedFieldProperties();
            mf.AppModel.LocalPlane = new LocalPlane(new Wgs84(0, 0), SharedFieldProperties);
            ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
        }

        public SharedFieldProperties SharedFieldProperties { get; }

        public void AverageTheSpeed()
        {
            //average the speed
            //if (speed > 70) speed = 70;
            mf.avgSpeed = (mf.avgSpeed * 0.75) + (speed * 0.25);
        }

        public void DefineLocalPlane(Wgs84 origin, bool setSim)
        {
            mf.AppModel.LocalPlane = new LocalPlane(origin, SharedFieldProperties);
            if (setSim && mf.timerSim.Enabled)
            {
                mf.AppModel.CurrentLatLon = origin;
                mf.sim.CurrentLatLon = origin;

                Properties.Settings.Default.setGPS_SimLatitude = mf.AppModel.LocalPlane.Origin.Latitude;
                Properties.Settings.Default.setGPS_SimLongitude = mf.AppModel.LocalPlane.Origin.Longitude;
                Properties.Settings.Default.Save();
            }
            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(mf.AppModel.CurrentLatLon);
            mf.worldGrid.checkZoomWorldGrid(geoCoord);
        }

    }
}
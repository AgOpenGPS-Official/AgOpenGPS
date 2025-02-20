using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.Text;

namespace AgOpenGPS
{
    public class CNMEA
    {
        const double degreesToRadians = 2.0 * Math.PI / 360.0;
        //WGS84 Lat Long
        public double latitude, longitude;

        public double prevLatitude, prevLongitude;

        //local plane geometry
        public double latStart, lonStart;

        public double mPerDegreeLat, mPerDegreeLon;

        //our current fix
        public vec2 fix = new vec2(0, 0);

        public vec2 prevSpeedFix = new vec2(0, 0);

        //used to offset the antenna position to compensate for drift
        public vec2 fixOffset = new vec2(0, 0);

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
            latStart = 0;
            lonStart = 0;
            ageAlarm = Properties.Settings.Default.setGPS_ageAlarm;
        }

        public void AverageTheSpeed()
        {
            //average the speed
            //if (speed > 70) speed = 70;
            mf.avgSpeed = (mf.avgSpeed * 0.75) + (speed * 0.25);
        }

        public void SetLocalMetersPerDegree(bool setSim)
        {
            if (setSim && mf.timerSim.Enabled)
            {
                latitude = mf.sim.latitude = Properties.Settings.Default.setGPS_SimLatitude = latStart;
                longitude = mf.sim.longitude = Properties.Settings.Default.setGPS_SimLongitude = lonStart;
                Properties.Settings.Default.Save();
            }

            mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * latStart * degreesToRadians) + 1.175
            * Math.Cos(4.0 * latStart * degreesToRadians) - 0.0023
            * Math.Cos(6.0 * latStart * degreesToRadians);

            mPerDegreeLon = 111412.84 * Math.Cos(latStart * degreesToRadians) - 93.5
            * Math.Cos(3.0 * latStart * degreesToRadians) + 0.118
            * Math.Cos(5.0 * latStart * degreesToRadians);

            ConvertWGS84ToLocal(latitude, longitude, out double northing, out double easting);
            mf.worldGrid.checkZoomWorldGrid(northing, easting);
        }

        public void ConvertWGS84ToLocal(double Lat, double Lon, out double Northing, out double Easting)
        {
            mPerDegreeLon =
                111412.84 * Math.Cos(Lat * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * Lat * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * Lat * degreesToRadians);

            Northing = (Lat - latStart) * mPerDegreeLat;
            Easting = (Lon - lonStart) * mPerDegreeLon;

            //Northing += mf.RandomNumber(-0.02, 0.02);
            //Easting += mf.RandomNumber(-0.02, 0.02);
        }

        public Wgs84 ConvertGeoCoordToWgs84(GeoCoord geoCoord)
        {
            latitude = ((geoCoord.Northing + fixOffset.northing) / mPerDegreeLat) + latStart;
            mPerDegreeLon =
                111412.84 * Math.Cos(latitude * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * latitude * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * latitude * degreesToRadians);
            longitude = ((geoCoord.Easting + fixOffset.easting) / mPerDegreeLon) + lonStart;
            return new Wgs84(latitude, longitude);
        }

        public string GetGeoCoordToWSG84_KML(GeoCoord geoCoord)
        {
            double Lat = (geoCoord.Northing / mPerDegreeLat) + latStart;
            mPerDegreeLon =
                111412.84 * Math.Cos(Lat * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * Lat * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * Lat * degreesToRadians);
            double Lon = (geoCoord.Easting / mPerDegreeLon) + lonStart;

            return Lon.ToString("N7", CultureInfo.InvariantCulture) + ',' + Lat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ";
        }
    }
}
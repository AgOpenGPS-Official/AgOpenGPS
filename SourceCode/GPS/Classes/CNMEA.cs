using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.Text;

namespace AgOpenGPS
{
    public class CNMEA
    {
        const double degreesToRadians = 2.0 * Math.PI / 360.0;

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
            mf.AppModel.StartLatLon = new Wgs84(0, 0);
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
                mf.AppModel.CurrentLatLon = mf.AppModel.StartLatLon;
                mf.sim.CurrentLatLon = mf.AppModel.StartLatLon;

                Properties.Settings.Default.setGPS_SimLatitude = mf.AppModel.StartLatLon.Latitude;
                Properties.Settings.Default.setGPS_SimLongitude = mf.AppModel.StartLatLon.Longitude;
                Properties.Settings.Default.Save();
            }

            mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * mf.AppModel.StartLatLon.Latitude * degreesToRadians) + 1.175
            * Math.Cos(4.0 * mf.AppModel.StartLatLon.Latitude * degreesToRadians) - 0.0023
            * Math.Cos(6.0 * mf.AppModel.StartLatLon.Latitude * degreesToRadians);

            mPerDegreeLon = 111412.84 * Math.Cos(mf.AppModel.StartLatLon.Latitude * degreesToRadians) - 93.5
            * Math.Cos(3.0 * mf.AppModel.StartLatLon.Latitude * degreesToRadians) + 0.118
            * Math.Cos(5.0 * mf.AppModel.StartLatLon.Latitude * degreesToRadians);

            GeoCoord geoCoord = ConvertWgs84ToGeoCoord(mf.AppModel.CurrentLatLon);
            mf.worldGrid.checkZoomWorldGrid(geoCoord);
        }

        public GeoCoord ConvertWgs84ToGeoCoord(Wgs84 latLon)
        {
            mPerDegreeLon =
                111412.84 * Math.Cos(latLon.Latitude * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * latLon.Latitude * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * latLon.Latitude * degreesToRadians);

            return new GeoCoord(
                (latLon.Latitude - mf.AppModel.StartLatLon.Latitude) * mPerDegreeLat,
                (latLon.Longitude - mf.AppModel.StartLatLon.Longitude) * mPerDegreeLon);
        }

        public Wgs84 ConvertGeoCoordToWgs84(GeoCoord geoCoord)
        {
            double lat = ((geoCoord.Northing + fixOffset.northing) / mPerDegreeLat) + mf.AppModel.StartLatLon.Latitude;
            mPerDegreeLon =
                111412.84 * Math.Cos(lat * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * lat * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * lat * degreesToRadians);
            double lon = ((geoCoord.Easting + fixOffset.easting) / mPerDegreeLon) + mf.AppModel.StartLatLon.Longitude;
            return new Wgs84(lat, lon);
        }

        public string GetGeoCoordToWgs84_KML(GeoCoord geoCoord)
        {
            double Lat = (geoCoord.Northing / mPerDegreeLat) + mf.AppModel.StartLatLon.Latitude;
            mPerDegreeLon =
                111412.84 * Math.Cos(Lat * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * Lat * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * Lat * degreesToRadians);
            double Lon = (geoCoord.Easting / mPerDegreeLon) + mf.AppModel.StartLatLon.Longitude;

            return Lon.ToString("N7", CultureInfo.InvariantCulture) + ',' + Lat.ToString("N7", CultureInfo.InvariantCulture) + ",0 ";
        }
    }
}
using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.Text;

namespace AgOpenGPS
{
    public class CNMEA
    {
        const double degreesToRadians = 2.0 * Math.PI / 360.0;

        public Wgs84 CurrentLatLon { get; set; }
        public double latitude => CurrentLatLon.Latitude;
        public double longitude => CurrentLatLon.Longitude;

        //local plane geometry
        public Wgs84 StartLatLon { get; set; }
        public double latStart => StartLatLon.Latitude;
        public double lonStart => StartLatLon.Longitude;

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
            StartLatLon = new Wgs84(0, 0);
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
                CurrentLatLon = StartLatLon;
                mf.sim.CurrentLatLon = StartLatLon;

                Properties.Settings.Default.setGPS_SimLatitude = latStart;
                Properties.Settings.Default.setGPS_SimLongitude = lonStart;
                Properties.Settings.Default.Save();
            }

            mPerDegreeLat = 111132.92 - 559.82 * Math.Cos(2.0 * latStart * degreesToRadians) + 1.175
            * Math.Cos(4.0 * latStart * degreesToRadians) - 0.0023
            * Math.Cos(6.0 * latStart * degreesToRadians);

            mPerDegreeLon = 111412.84 * Math.Cos(latStart * degreesToRadians) - 93.5
            * Math.Cos(3.0 * latStart * degreesToRadians) + 0.118
            * Math.Cos(5.0 * latStart * degreesToRadians);

            GeoCoord geoCoord = ConvertWgs84ToGeoCoord(new Wgs84(latitude, longitude));
            mf.worldGrid.checkZoomWorldGrid(geoCoord);
        }

        public GeoCoord ConvertWgs84ToGeoCoord(Wgs84 latLon)
        {
            mPerDegreeLon =
                111412.84 * Math.Cos(latLon.Latitude * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * latLon.Latitude * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * latLon.Latitude * degreesToRadians);

            return new GeoCoord(
                (latLon.Latitude - latStart) * mPerDegreeLat,
                (latLon.Longitude - lonStart) * mPerDegreeLon);
        }

        public Wgs84 ConvertGeoCoordToWgs84(GeoCoord geoCoord)
        {
            double lat = ((geoCoord.Northing + fixOffset.northing) / mPerDegreeLat) + latStart;
            mPerDegreeLon =
                111412.84 * Math.Cos(lat * degreesToRadians)
                - 93.5 * Math.Cos(3.0 * lat * degreesToRadians)
                + 0.118 * Math.Cos(5.0 * lat * degreesToRadians);
            double lon = ((geoCoord.Easting + fixOffset.easting) / mPerDegreeLon) + lonStart;
            return new Wgs84(lat, lon);
        }

        public string GetGeoCoordToWgs84_KML(GeoCoord geoCoord)
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
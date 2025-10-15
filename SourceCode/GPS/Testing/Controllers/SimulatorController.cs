using System;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.Testing.Controllers
{
    public class SimulatorController : ISimulatorController
    {
        private readonly FormGPS mf;

        public bool IsEnabled { get; private set; }

        public SimulatorController(FormGPS formGPS)
        {
            mf = formGPS ?? throw new ArgumentNullException(nameof(formGPS));
            IsEnabled = false;
        }

        public void Enable()
        {
            if (IsEnabled)
            {
                return;
            }

            IsEnabled = true;
            mf.sim.stepDistance = 0;
            mf.sim.headingTrue = 0;
        }

        public void Disable()
        {
            IsEnabled = false;
            mf.sim.stepDistance = 0;
        }

        public void SetPosition(double lat, double lon)
        {
            var latLon = new Wgs84(lat, lon);
            mf.sim.CurrentLatLon = latLon;
            mf.AppModel.CurrentLatLon = latLon;

            GeoCoord fixCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(latLon);
            mf.pn.fix.northing = fixCoord.Northing;
            mf.pn.fix.easting = fixCoord.Easting;

            Properties.Settings.Default.setGPS_SimLatitude = lat;
            Properties.Settings.Default.setGPS_SimLongitude = lon;
        }

        public void SetPositionLocal(double easting, double northing)
        {
            // Set position in local ENU coordinates (meters from origin)
            mf.pn.fix.easting = easting;
            mf.pn.fix.northing = northing;

            // Convert local coordinates to lat/lon
            GeoCoord geoCoord = new GeoCoord(easting, northing);
            Wgs84 latLon = mf.AppModel.LocalPlane.ConvertGeoCoordToWgs84(geoCoord);

            mf.sim.CurrentLatLon = latLon;
            mf.AppModel.CurrentLatLon = latLon;

            Properties.Settings.Default.setGPS_SimLatitude = latLon.Latitude;
            Properties.Settings.Default.setGPS_SimLongitude = latLon.Longitude;
        }

        public void SetHeading(double headingDegrees)
        {
            double headingRadians = headingDegrees * 0.0174533; // Convert degrees to radians
            mf.sim.headingTrue = headingRadians;
            mf.pn.headingTrue = headingDegrees;
            mf.pn.headingTrueDual = headingDegrees;
            mf.ahrs.imuHeading = headingDegrees;
        }

        public void SetSpeed(double speedKph)
        {
            // Convert speed to step distance (distance per simulation tick)
            // Assuming 10Hz update rate: stepDistance = (speedKph / 3.6) / 10
            double speedMps = speedKph / 3.6;
            mf.sim.stepDistance = speedMps / 10.0;
            mf.pn.vtgSpeed = Math.Abs(Math.Round(4 * mf.sim.stepDistance * 10, 2));
        }

        public void SetSteerAngle(double angleDegrees)
        {
            mf.sim.steerAngle = angleDegrees;
            mf.mc.actualSteerAngleDegrees = angleDegrees;
        }

        public SimulatorState GetState()
        {
            return new SimulatorState
            {
                Position = new Wgs84(mf.sim.CurrentLatLon.Latitude, mf.sim.CurrentLatLon.Longitude),
                HeadingDegrees = glm.toDegrees(mf.sim.headingTrue),
                SpeedKph = Math.Abs(mf.sim.stepDistance * 10 * 3.6), // Convert back to km/h
                SteerAngleDegrees = mf.sim.steerAngle,
                Easting = mf.pn.fix.easting,
                Northing = mf.pn.fix.northing
            };
        }
    }
}

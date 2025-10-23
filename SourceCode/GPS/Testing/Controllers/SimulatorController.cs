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
            // NOTE: GeoCoord constructor takes (northing, easting) - NOT (easting, northing)!
            GeoCoord geoCoord = new GeoCoord(northing, easting);
            Wgs84 latLon = mf.AppModel.LocalPlane.ConvertGeoCoordToWgs84(geoCoord);

            mf.sim.CurrentLatLon = latLon;
            mf.AppModel.CurrentLatLon = latLon;

            Properties.Settings.Default.setGPS_SimLatitude = latLon.Latitude;
            Properties.Settings.Default.setGPS_SimLongitude = latLon.Longitude;

            // Update position through the full processing pipeline
            mf.sentenceCounter = 0; // Reset counter so system thinks we have valid GPS
            mf.UpdateFixPosition();
        }

        public void SetHeading(double headingDegrees)
        {
            double headingRadians = headingDegrees * 0.0174533; // Convert degrees to radians

            // Set heading in all relevant places
            mf.sim.headingTrue = headingRadians;
            mf.pn.headingTrue = headingDegrees;
            mf.pn.headingTrueDual = headingDegrees;
            mf.ahrs.imuHeading = headingDegrees;

            // Also ensure fixHeading is set for direction detection
            mf.fixHeading = headingRadians;
        }

        public void SetSpeed(double speedKph)
        {
            // CSim.DoSimTick assumes 10Hz (0.1s ticks) with hardcoded formula: vtgSpeed = 4 * stepDistance * 10
            // To work around this WITHOUT modifying CSim.cs:
            // - stepDistance must be distance per 0.1s tick
            // - We ensure DoSimTick is called at 10Hz by calling it multiple times if needed
            double speedMps = speedKph / 3.6;
            mf.sim.stepDistance = speedMps / 10.0; // Distance per 0.1s tick

            // Manually set correct speed since CSim's formula is wrong
            // CSim calculates: vtgSpeed = 4 * stepDistance * 10 = 4 * (speedMps/10) * 10 = 4 * speedMps
            // But vtgSpeed should just be speedMps, so we override it after CSim sets it
            mf.pn.vtgSpeed = Math.Abs(Math.Round(speedMps, 2));
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
                // stepDistance is distance per 0.1s tick, so multiply by 10 to get m/s, then by 3.6 for km/h
                SpeedKph = Math.Abs(mf.sim.stepDistance * 10.0 * 3.6),
                SteerAngleDegrees = mf.sim.steerAngle,
                Easting = mf.pn.fix.easting,
                Northing = mf.pn.fix.northing
            };
        }
    }
}

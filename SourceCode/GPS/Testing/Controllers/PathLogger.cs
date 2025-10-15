using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.Testing.Controllers
{
    public class PathLogger : IPathLogger
    {
        private readonly FormGPS mf;
        private bool isLogging;
        private List<PathPoint> loggedPath;
        private double startTime;

        public bool IsLogging => isLogging;

        public PathLogger(FormGPS formGPS)
        {
            mf = formGPS ?? throw new ArgumentNullException(nameof(formGPS));
            loggedPath = new List<PathPoint>();
            isLogging = false;
        }

        public void StartLogging()
        {
            if (isLogging)
            {
                return;
            }

            isLogging = true;
            loggedPath.Clear();
            startTime = mf.secondsSinceStart;
        }

        public void StopLogging()
        {
            isLogging = false;
        }

        public List<PathPoint> GetLoggedPath()
        {
            return new List<PathPoint>(loggedPath);
        }

        public void ClearLog()
        {
            loggedPath.Clear();
        }

        public void LogCurrentState()
        {
            if (!isLogging)
            {
                return;
            }

            // Convert fix position from local to lat/lon
            GeoCoord fixGeo = new GeoCoord(mf.pn.fix.northing, mf.pn.fix.easting);
            Wgs84 fixWgs = mf.AppModel.LocalPlane.ConvertGeoCoordToWgs84(fixGeo);

            var pathPoint = new PathPoint
            {
                Timestamp = mf.secondsSinceStart - startTime,
                Position = fixWgs,
                Easting = mf.pn.fix.easting,
                Northing = mf.pn.fix.northing,
                HeadingDegrees = mf.AppModel.FixHeading.AngleInDegrees,
                SpeedKph = mf.avgSpeed,
                IsAutosteerActive = mf.isBtnAutoSteerOn,
                IsInUTurn = mf.yt.isYouTurnTriggered,
                CrossTrackError = mf.guidanceLineDistanceOff / 1000.0, // Convert mm to meters
                SteerAngleDegrees = mf.mc.actualSteerAngleDegrees
            };

            loggedPath.Add(pathPoint);
        }
    }
}

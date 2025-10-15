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

            var pathPoint = new PathPoint
            {
                Timestamp = mf.secondsSinceStart - startTime,
                Position = new Wgs84(mf.AppModel.FixPosition.Lat, mf.AppModel.FixPosition.Lon),
                Easting = mf.AppModel.FixPosition.Easting,
                Northing = mf.AppModel.FixPosition.Northing,
                HeadingDegrees = mf.AppModel.FixHeading.AngleInDegrees,
                SpeedKph = mf.AppModel.FixSpeed.KilometersPerHour,
                IsAutosteerActive = mf.isBtnAutoSteerOn,
                IsInUTurn = mf.yt.isYouTurnRight || mf.yt.isYouTurnLeft,
                CrossTrackError = mf.ABLine.distanceFromCurrentLine,
                SteerAngleDegrees = mf.AppModel.SteerAngle.AngleInDegrees
            };

            loggedPath.Add(pathPoint);
        }
    }
}

using System.Collections.Generic;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Testing
{
    public interface IPathLogger
    {
        void StartLogging();
        void StopLogging();
        List<PathPoint> GetLoggedPath();
        void ClearLog();
        void LogCurrentState();
        bool IsLogging { get; }
    }

    public class PathPoint
    {
        public double Timestamp { get; set; }
        public Wgs84 Position { get; set; }
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double HeadingDegrees { get; set; }
        public double SpeedKph { get; set; }
        public bool IsAutosteerActive { get; set; }
        public bool IsInUTurn { get; set; }
        public double CrossTrackError { get; set; }
        public double SteerAngleDegrees { get; set; }
    }
}

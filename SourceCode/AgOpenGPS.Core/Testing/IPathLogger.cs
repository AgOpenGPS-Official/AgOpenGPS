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
        void LogCurrentState(double simulationTime);
        void ExportToJson(string filePath);
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

        // Debug input data for XTE calculation
        public double PivotEasting { get; set; }
        public double PivotNorthing { get; set; }
        public double PivotHeading { get; set; }
        public double CurrentLinePtA_E { get; set; }
        public double CurrentLinePtA_N { get; set; }
        public double CurrentLinePtB_E { get; set; }
        public double CurrentLinePtB_N { get; set; }
        public double ABLineHeading { get; set; }
        public bool IsHeadingSameWay { get; set; }
        public double ToolWidth { get; set; }
        public double ToolOverlap { get; set; }
        public double ToolOffset { get; set; }
        public bool IsABValid { get; set; }
    }
}

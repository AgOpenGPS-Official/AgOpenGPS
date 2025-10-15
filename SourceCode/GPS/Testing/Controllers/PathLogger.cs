using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
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
        private DateTime testStartDateTime;

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
            testStartDateTime = DateTime.Now;
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

        public void LogCurrentState(double simulationTime)
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
                Timestamp = simulationTime - startTime,
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

        public void ExportToJson(string filePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("{");

            // Export tractor path
            sb.AppendLine("  \"tractorPath\": [");
            for (int i = 0; i < loggedPath.Count; i++)
            {
                var pt = loggedPath[i];
                sb.Append($"    {{\"t\": {pt.Timestamp.ToString("F2", CultureInfo.InvariantCulture)}, \"e\": {pt.Easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {pt.Northing.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"heading\": {pt.HeadingDegrees.ToString("F2", CultureInfo.InvariantCulture)}, \"speed\": {pt.SpeedKph.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"xte\": {pt.CrossTrackError.ToString("F3", CultureInfo.InvariantCulture)}, \"autosteer\": {pt.IsAutosteerActive.ToString().ToLower()}, " +
                         $"\"uturn\": {pt.IsInUTurn.ToString().ToLower()}}}");
                if (i < loggedPath.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("  ],");

            // Export field boundary
            sb.AppendLine("  \"fieldBoundary\": [");
            if (mf.bnd.bndList.Count > 0)
            {
                var boundary = mf.bnd.bndList[0];
                for (int i = 0; i < boundary.fenceLineEar.Count; i++)
                {
                    var pt = boundary.fenceLineEar[i];
                    sb.Append($"    {{\"e\": {pt.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {pt.northing.ToString("F2", CultureInfo.InvariantCulture)}}}");
                    if (i < boundary.fenceLineEar.Count - 1) sb.Append(",");
                    sb.AppendLine();
                }
            }
            sb.AppendLine("  ],");

            // Export turn lines
            sb.AppendLine("  \"turnLines\": [");
            if (mf.bnd.bndList.Count > 0)
            {
                var boundary = mf.bnd.bndList[0];
                for (int i = 0; i < boundary.turnLine.Count; i++)
                {
                    var pt = boundary.turnLine[i];
                    sb.Append($"    {{\"e\": {pt.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {pt.northing.ToString("F2", CultureInfo.InvariantCulture)}}}");
                    if (i < boundary.turnLine.Count - 1) sb.Append(",");
                    sb.AppendLine();
                }
            }
            sb.AppendLine("  ],");

            // Export reference AB line (the track points - where user drove)
            sb.AppendLine("  \"refABLine\": [");
            if (mf.trk.gArr.Count > 0 && mf.trk.idx >= 0 && mf.trk.gArr[mf.trk.idx].mode == TrackMode.AB)
            {
                var track = mf.trk.gArr[mf.trk.idx];
                sb.AppendLine($"    {{\"e\": {track.ptA.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {track.ptA.northing.ToString("F2", CultureInfo.InvariantCulture)}}},");
                sb.AppendLine($"    {{\"e\": {track.ptB.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {track.ptB.northing.ToString("F2", CultureInfo.InvariantCulture)}}}");
            }
            else
            {
                sb.AppendLine($"    {{\"e\": 0, \"n\": 0}},");
                sb.AppendLine($"    {{\"e\": 0, \"n\": 1}}");
            }
            sb.AppendLine("  ],");

            // Export current AB line (offset for implement edge)
            sb.AppendLine("  \"currentABLine\": [");
            sb.AppendLine($"    {{\"e\": {mf.ABLine.currentLinePtA.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {mf.ABLine.currentLinePtA.northing.ToString("F2", CultureInfo.InvariantCulture)}}},");
            sb.AppendLine($"    {{\"e\": {mf.ABLine.currentLinePtB.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {mf.ABLine.currentLinePtB.northing.ToString("F2", CultureInfo.InvariantCulture)}}}");
            sb.AppendLine("  ],");

            // Export turn pattern (ytList)
            sb.AppendLine("  \"turnPattern\": [");
            for (int i = 0; i < mf.yt.ytList.Count; i++)
            {
                var pt = mf.yt.ytList[i];
                sb.Append($"    {{\"e\": {pt.easting.ToString("F2", CultureInfo.InvariantCulture)}, \"n\": {pt.northing.ToString("F2", CultureInfo.InvariantCulture)}, \"heading\": {pt.heading.ToString("F2", CultureInfo.InvariantCulture)}}}");
                if (i < mf.yt.ytList.Count - 1) sb.Append(",");
                sb.AppendLine();
            }
            sb.AppendLine("  ],");

            // Export metadata
            sb.AppendLine("  \"metadata\": {");
            sb.AppendLine($"    \"testRunDateTime\": \"{testStartDateTime.ToString("yyyy-MM-dd HH:mm:ss")}\",");
            sb.AppendLine($"    \"youTurnPhase\": {mf.yt.youTurnPhase},");
            sb.AppendLine($"    \"isYouTurnBtnOn\": {mf.yt.isYouTurnBtnOn.ToString().ToLower()},");
            sb.AppendLine($"    \"turnAreaWidth\": {mf.yt.uturnDistanceFromBoundary.ToString("F2", CultureInfo.InvariantCulture)},");
            sb.AppendLine($"    \"toolWidth\": {mf.tool.width.ToString("F2", CultureInfo.InvariantCulture)},");
            sb.AppendLine($"    \"toolOverlap\": {mf.tool.overlap.ToString("F2", CultureInfo.InvariantCulture)},");
            sb.AppendLine($"    \"toolOffset\": {mf.tool.offset.ToString("F2", CultureInfo.InvariantCulture)}");
            sb.AppendLine("  }");

            sb.AppendLine("}");

            File.WriteAllText(filePath, sb.ToString());
        }
    }
}

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
                SteerAngleDegrees = mf.mc.actualSteerAngleDegrees,

                // Debug input data for XTE calculation
                PivotEasting = mf.pivotAxlePos.easting,
                PivotNorthing = mf.pivotAxlePos.northing,
                PivotHeading = mf.pivotAxlePos.heading,
                CurrentLinePtA_E = mf.ABLine.currentLinePtA.easting,
                CurrentLinePtA_N = mf.ABLine.currentLinePtA.northing,
                CurrentLinePtB_E = mf.ABLine.currentLinePtB.easting,
                CurrentLinePtB_N = mf.ABLine.currentLinePtB.northing,
                ABLineHeading = mf.ABLine.abHeading,
                IsHeadingSameWay = mf.ABLine.isHeadingSameWay,
                ToolWidth = mf.tool.width,
                ToolOverlap = mf.tool.overlap,
                ToolOffset = mf.tool.offset,
                IsABValid = mf.ABLine.isABValid,

                // Additional debug data for diagnosing autosteer issues
                HowManyPathsAway = mf.ABLine.howManyPathsAway,
                DistanceFromRefLine = mf.ABLine.distanceFromRefLine,
                DistanceFromCurrentLinePivot = mf.ABLine.distanceFromCurrentLinePivot,
                RefLinePtA_E = (mf.trk.gArr.Count > 0 && mf.trk.idx >= 0) ? mf.trk.gArr[mf.trk.idx].ptA.easting : 0,
                RefLinePtA_N = (mf.trk.gArr.Count > 0 && mf.trk.idx >= 0) ? mf.trk.gArr[mf.trk.idx].ptA.northing : 0,
                RefLinePtB_E = (mf.trk.gArr.Count > 0 && mf.trk.idx >= 0) ? mf.trk.gArr[mf.trk.idx].ptB.easting : 0,
                RefLinePtB_N = (mf.trk.gArr.Count > 0 && mf.trk.idx >= 0) ? mf.trk.gArr[mf.trk.idx].ptB.northing : 0,

                // Heading calculation debug data
                IsFirstHeadingSet = mf.isFirstHeadingSet,
                GpsHeading = glm.toDegrees(mf.gpsHeading),
                FixHeading = glm.toDegrees(mf.fixHeading),
                ImuHeading = mf.ahrs.imuHeading,
                SimHeadingTrue = glm.toDegrees(mf.sim.headingTrue),
                HeadingSource = mf.headingFromSource,

                // Section/Implement control debug data
                IsJobStarted = mf.isJobStarted,
                AutoBtnState = mf.autoBtnState.ToString(),
                ManualBtnState = mf.manualBtnState.ToString(),
                PatchCounter = mf.patchCounter,
                Section0_IsSectionOn = mf.section[0].isSectionOn,
                Section0_SectionBtnState = mf.section[0].sectionBtnState.ToString(),
                Section0_IsMappingOn = mf.section[0].isMappingOn,
                Section0_SectionOnRequest = mf.section[0].sectionOnRequest,
                Section0_SectionOffRequest = mf.section[0].sectionOffRequest,
                Section0_SpeedPixels = mf.section[0].speedPixels,
                AvgSpeed = mf.avgSpeed,
                SlowSpeedCutoff = mf.vehicle.slowSpeedCutoff
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
                sb.Append($"    {{\"t\": {pt.Timestamp.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"e\": {pt.Easting.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"n\": {pt.Northing.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"heading\": {pt.HeadingDegrees.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"speed\": {pt.SpeedKph.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"xte\": {pt.CrossTrackError.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"autosteer\": {pt.IsAutosteerActive.ToString().ToLower()}, " +
                         $"\"uturn\": {pt.IsInUTurn.ToString().ToLower()}, " +
                         $"\"pivotE\": {pt.PivotEasting.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"pivotN\": {pt.PivotNorthing.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"pivotHeading\": {pt.PivotHeading.ToString("F4", CultureInfo.InvariantCulture)}, " +
                         $"\"lineA_E\": {pt.CurrentLinePtA_E.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"lineA_N\": {pt.CurrentLinePtA_N.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"lineB_E\": {pt.CurrentLinePtB_E.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"lineB_N\": {pt.CurrentLinePtB_N.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"abHeading\": {pt.ABLineHeading.ToString("F4", CultureInfo.InvariantCulture)}, " +
                         $"\"sameway\": {pt.IsHeadingSameWay.ToString().ToLower()}, " +
                         $"\"toolW\": {pt.ToolWidth.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"toolOvl\": {pt.ToolOverlap.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"toolOfs\": {pt.ToolOffset.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"abValid\": {pt.IsABValid.ToString().ToLower()}, " +
                         $"\"pathsAway\": {pt.HowManyPathsAway}, " +
                         $"\"distFromRef\": {pt.DistanceFromRefLine.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"distFromCur\": {pt.DistanceFromCurrentLinePivot.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"refA_E\": {pt.RefLinePtA_E.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"refA_N\": {pt.RefLinePtA_N.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"refB_E\": {pt.RefLinePtB_E.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"refB_N\": {pt.RefLinePtB_N.ToString("F3", CultureInfo.InvariantCulture)}, " +
                         $"\"isFirstHeadingSet\": {pt.IsFirstHeadingSet.ToString().ToLower()}, " +
                         $"\"gpsHeading\": {pt.GpsHeading.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"fixHeading\": {pt.FixHeading.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"imuHeading\": {pt.ImuHeading.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"simHeadingTrue\": {pt.SimHeadingTrue.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"headingSource\": \"{pt.HeadingSource}\", " +
                         $"\"jobStarted\": {pt.IsJobStarted.ToString().ToLower()}, " +
                         $"\"autoBtnState\": \"{pt.AutoBtnState}\", " +
                         $"\"manualBtnState\": \"{pt.ManualBtnState}\", " +
                         $"\"patchCounter\": {pt.PatchCounter}, " +
                         $"\"sec0_on\": {pt.Section0_IsSectionOn.ToString().ToLower()}, " +
                         $"\"sec0_btnState\": \"{pt.Section0_SectionBtnState}\", " +
                         $"\"sec0_mapping\": {pt.Section0_IsMappingOn.ToString().ToLower()}, " +
                         $"\"sec0_onReq\": {pt.Section0_SectionOnRequest.ToString().ToLower()}, " +
                         $"\"sec0_offReq\": {pt.Section0_SectionOffRequest.ToString().ToLower()}, " +
                         $"\"sec0_speedPx\": {pt.Section0_SpeedPixels.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"avgSpeed\": {pt.AvgSpeed.ToString("F2", CultureInfo.InvariantCulture)}, " +
                         $"\"slowCutoff\": {pt.SlowSpeedCutoff.ToString("F2", CultureInfo.InvariantCulture)}}}");
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

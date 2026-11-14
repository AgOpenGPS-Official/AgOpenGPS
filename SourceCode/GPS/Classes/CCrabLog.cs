using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace AgOpenGPS
{
    /// <summary>
    /// Temporary CSV log of the crab / tool-on-line guidance values, one row per fix, for analysing
    /// how the rear axle and the tool behave (especially in curves). Written to the AgOpenGPS Logs
    /// folder as CrabLog_yyyyMMdd_HHmmss.csv while a line is active and the vehicle is moving.
    /// Never allowed to disturb guidance: every failure just stops the log.
    /// </summary>
    public class CCrabLog
    {
        private readonly FormGPS mf;
        private StreamWriter writer;
        private bool failed;
        private DateTime startTime;
        private double lastHeading;
        private bool hasLastHeading;

        public CCrabLog(FormGPS _f)
        {
            mf = _f;
        }

        private const string header =
            "t_s,mode,autosteer,speed_kmh,easting,northing,fixHeading_deg,guidanceHeading_deg,yawRate_deg_s," +
            "crab_deg,crabRaw_deg,toolShift_m,toolRef,hitch_m,rigid,crabOn,toolOnLineOn,toolCurveOn,headingErr_deg," +
            "xteRear_mm,xteTool_mm,lookAhead_m,steerReq_deg,steerAct_deg,integral,sameWay," +
            // Outer loop (tool trim) and the heading sources, to measure the gyro drift against the dual heading:
            // drift = unwrap(imuRaw - dualTrue) over time. 99999 = no IMU, 32000 = no value.
            "trim_m,trimOn,trimFiltErr_mm,imuRaw_deg,imuGpsOffset_deg,dualTrue_deg,gpsHeading_deg,imuRoll_deg";

        public void Write()
        {
            if (failed) return;

            try
            {
                if (mf.trk.idx < 0 || mf.trk.gArr == null || mf.trk.idx >= mf.trk.gArr.Count
                    || mf.ct.isContourBtnOn || mf.isStanleyUsed
                    || mf.avgSpeed < 1.0 || mf.guidanceLineDistanceOff >= 29000 && mf.toolLineDistanceOff >= 29000)
                {
                    hasLastHeading = false;
                    return;
                }

                if (writer == null)
                {
                    string dir = RegistrySettings.logsDirectory;
                    if (string.IsNullOrEmpty(dir)) { failed = true; return; }
                    Directory.CreateDirectory(dir);
                    startTime = DateTime.Now;
                    writer = new StreamWriter(
                        Path.Combine(dir, "CrabLog_" + startTime.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + ".csv"),
                        false, new UTF8Encoding(false));
                    writer.AutoFlush = true;
                    writer.WriteLine(header);
                }

                bool isAB = mf.trk.gArr[mf.trk.idx].mode == TrackMode.AB;

                double yawRate = 0;
                if (hasLastHeading)
                {
                    double dh = mf.fixHeading - lastHeading;
                    while (dh > Math.PI) dh -= glm.twoPI;
                    while (dh < -Math.PI) dh += glm.twoPI;
                    yawRate = glm.toDegrees(dh) * 10.0; // fixes come in at about 10 Hz
                }
                lastHeading = mf.fixHeading;
                hasLastHeading = true;

                var c = CultureInfo.InvariantCulture;
                var sb = new StringBuilder(256);
                sb.Append((DateTime.Now - startTime).TotalSeconds.ToString("F2", c)).Append(',')
                  .Append(isAB ? "AB" : "Curve").Append(',')
                  .Append(mf.isBtnAutoSteerOn ? 1 : 0).Append(',')
                  .Append(mf.avgSpeed.ToString("F2", c)).Append(',')
                  .Append(mf.pivotAxlePos.easting.ToString("F3", c)).Append(',')
                  .Append(mf.pivotAxlePos.northing.ToString("F3", c)).Append(',')
                  .Append(glm.toDegrees(mf.fixHeading).ToString("F3", c)).Append(',')
                  .Append(glm.toDegrees(mf.guidanceHeading).ToString("F3", c)).Append(',')
                  .Append(yawRate.ToString("F2", c)).Append(',')
                  .Append(glm.toDegrees(mf.crab.crabAngle).ToString("F3", c)).Append(',')
                  .Append(glm.toDegrees(mf.crab.crabAngleRaw).ToString("F3", c)).Append(',')
                  .Append(mf.crab.toolShift.ToString("F4", c)).Append(',')
                  .Append(mf.curve.isToolRefActive ? 1 : 0).Append(',')
                  .Append(mf.tool.GetHitchLengthFromVehiclePivot().ToString("F2", c)).Append(',')
                  .Append(mf.crab.IsRigidTool ? 1 : 0).Append(',')
                  .Append(Properties.ToolSettings.Default.setVehicle_isCrabCompensationOn ? 1 : 0).Append(',')
                  .Append(Properties.ToolSettings.Default.setVehicle_isCrabToolOnLine ? 1 : 0).Append(',')
                  .Append(Properties.ToolSettings.Default.setVehicle_isToolCurveComp ? 1 : 0).Append(',')
                  .Append(mf.vehicle.modeActualHeadingError.ToString("F3", c)).Append(',')
                  .Append(mf.guidanceLineDistanceOff).Append(',')
                  .Append(mf.toolLineDistanceOff).Append(',')
                  .Append(mf.vehicle.goalDistance.ToString("F2", c)).Append(',')
                  .Append((mf.guidanceLineSteerAngle * 0.01).ToString("F2", c)).Append(',')
                  .Append(mf.mc.actualSteerAngleDegrees.ToString("F2", c)).Append(',')
                  .Append((isAB ? mf.ABLine.inty : mf.curve.inty).ToString("F4", c)).Append(',')
                  .Append((isAB ? mf.ABLine.isHeadingSameWay : mf.curve.isHeadingSameWay) ? 1 : 0).Append(',')
                  .Append(mf.crab.trimShift.ToString("F4", c)).Append(',')
                  .Append(mf.crab.isTrimActive ? 1 : 0).Append(',')
                  .Append((mf.crab.trimFilteredError * 1000.0).ToString("F1", c)).Append(',')
                  .Append(mf.ahrs.imuHeading.ToString("F3", c)).Append(',')
                  .Append(glm.toDegrees(mf.imuGPS_Offset).ToString("F3", c)).Append(',')
                  .Append(mf.pn.headingTrueDual.ToString("F3", c)).Append(',')
                  .Append(glm.toDegrees(mf.gpsHeading).ToString("F3", c)).Append(',')
                  .Append(mf.ahrs.imuRoll.ToString("F2", c));
                writer.WriteLine(sb.ToString());
            }
            catch (Exception)
            {
                failed = true;
                try { writer?.Dispose(); } catch (Exception) { }
                writer = null;
            }
        }
    }
}
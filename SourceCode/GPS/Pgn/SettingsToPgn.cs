using AgOpenGPS;
using AgOpenGPS.Properties;
using System;

namespace AgOpenGPS
{
    public static class SettingsToPgn
    {

        public static void DefaultSettingsToPgn238(Pgn238MachineConfig pgn238)
        {
            pgn238.Settings0 = Properties.Settings.Default.setArdMac_setting0;
            pgn238.RaiseTime = Properties.Settings.Default.setArdMac_hydRaiseTime;
            pgn238.LowerTime = Properties.Settings.Default.setArdMac_hydLowerTime;
            pgn238.User1 = Properties.Settings.Default.setArdMac_user1;
            pgn238.User2 = Properties.Settings.Default.setArdMac_user2;
            pgn238.User3 = Properties.Settings.Default.setArdMac_user3;
            pgn238.User4 = Properties.Settings.Default.setArdMac_user4;
        }

        public static void DefaultSettingsToPgn251(Pgn251AutoSteerBoardConfig pgn251)
        {
            pgn251.Settings0 = Properties.Settings.Default.setArdSteer_setting0;
            pgn251.Settings1 = Properties.Settings.Default.setArdSteer_setting1;
            pgn251.MaxPulseCount = Properties.Settings.Default.setArdSteer_maxPulseCounts;
            pgn251.MinSpeed = Properties.Settings.Default.setAS_minSteerSpeed;
            pgn251.AngVel = Properties.Settings.Default.setAS_isConstantContourOn;
        }

        public static void DefaultSettingsToPgn252(Pgn252AutoSteerSettings pgn252)
        {
            pgn252.CountsPerDegreeByte = Properties.Settings.Default.setAS_countsPerDegree;
            pgn252.AckermanByte = Properties.Settings.Default.setAS_ackerman;
            pgn252.HighSteerPwmByte = Properties.Settings.Default.setAS_highSteerPWM;
            pgn252.LowSteerPwmByte = Properties.Settings.Default.setAS_lowSteerPWM;
            pgn252.ProportionalGainByte = Properties.Settings.Default.setAS_Kp;
            pgn252.MinSteerPwmByte = Properties.Settings.Default.setAS_minSteerPWM;

            pgn252.WheelAngleSensorOffset = Properties.Settings.Default.setAS_wasOffset;
        }


    }
}



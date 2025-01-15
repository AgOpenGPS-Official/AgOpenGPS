namespace AgOpenGPS
{

    public class Pgn252AutoSteerSettings : PgnBase
    {
        private const int _proportionalGain = 5;
        private const int _highSteerPwm = 6;
        private const int _lowSteerPwm = 7;
        private const int _minSteerPWM = 8;
        private const int _countsPerDegree = 9;
        private const int _wasOffsetLo = 10;
        private const int _wasOffsetHi = 11;
        private const int _ackerman = 12;

        public Pgn252AutoSteerSettings() : base(252, 8)
        {
        }

        public byte SetProportionalGain(int proportionalGain)
        {
            byte b = unchecked((byte)proportionalGain);
            message[_proportionalGain] = b;
            return b;
        }

        public byte SetHighSteerPwm(int highSteerPwm)
        {
            byte b = unchecked((byte)highSteerPwm);
            message[_highSteerPwm] = b;
            return b;
        }

        public byte SetLowSteerPwm(int lowSteerPwm)
        {
            byte b = unchecked((byte)lowSteerPwm);
            message[_lowSteerPwm] = b;
            return b;
        }

        public byte SetMinSteerPwm(int minSteerPwm)
        {
            byte b = unchecked((byte)minSteerPwm);
            message[_minSteerPWM] = b;
            return b;
        }

        public byte SetCountsPerDegree(int countsPerDegree)
        {
            byte b = unchecked((byte)countsPerDegree);
            message[_countsPerDegree] = b;
            return b;
        }

        public byte SetAckerman(int ackerman)
        {
            byte b = unchecked((byte)ackerman);
            message[_ackerman] = b;
            return b;
        }

        // Wheel Angle Sensor
        public int SetWasOffset(int wasOffset)
        {
            message[_wasOffsetHi] = unchecked((byte)(wasOffset >> 8));
            message[_wasOffsetLo] = unchecked((byte)(wasOffset));
            return wasOffset;
        }

        public void SetFromDefaultSettings()
        {
            message[_countsPerDegree] = Properties.Settings.Default.setAS_countsPerDegree;
            message[_ackerman] = Properties.Settings.Default.setAS_ackerman;
            message[_wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
            message[_wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));
            message[_highSteerPwm] = Properties.Settings.Default.setAS_highSteerPWM;
            message[_lowSteerPwm] = Properties.Settings.Default.setAS_lowSteerPWM;
            message[_proportionalGain] = Properties.Settings.Default.setAS_Kp;
            message[_minSteerPWM] = Properties.Settings.Default.setAS_minSteerPWM;
        }

    }
}

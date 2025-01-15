namespace AgOpenGPS
{

    public class Pgn252AutoSteerSettings : PgnBase
    {
        private const int _proportionalGainIndex = 5;
        private const int _highSteerPwmIndex = 6;
        private const int _lowSteerPwmIndex = 7;
        private const int _minSteerPwmIndex = 8;
        private const int _countsPerDegreeIndex = 9;
        private const int _wasOffsetLoIndex = 10;
        private const int _wasOffsetHiIndex = 11;
        private const int _ackermanIndex = 12;

        public Pgn252AutoSteerSettings(IPgnErrorPresenter errorPresenter) : base(252, 8, errorPresenter)
        {
        }

        public byte SetProportionalGain(int proportionalGain)
        {
            return SetInt(_proportionalGainIndex, proportionalGain);
        }

        public byte SetHighSteerPwm(int highSteerPwm)
        {
            return SetInt(_highSteerPwmIndex, highSteerPwm);
        }

        public byte SetLowSteerPwm(int lowSteerPwm)
        {
            return SetInt(_lowSteerPwmIndex, lowSteerPwm);
        }

        public byte SetMinSteerPwm(int minSteerPwm)
        {
            return SetInt(_minSteerPwmIndex, minSteerPwm);
        }

        public byte SetCountsPerDegree(int countsPerDegree)
        {
            return SetInt(_countsPerDegreeIndex, countsPerDegree);
        }

        public byte SetAckerman(int ackerman)
        {
            return SetInt(_ackermanIndex, ackerman);
        }

        // Wheel Angle Sensor
        public void SetWasOffset(int wasOffset)
        {
            SetIntLoHi(_wasOffsetLoIndex, wasOffset);
        }

        public void SetFromDefaultSettings()
        {
            _message[_countsPerDegreeIndex] = Properties.Settings.Default.setAS_countsPerDegree;
            _message[_ackermanIndex] = Properties.Settings.Default.setAS_ackerman;
            SetIntLoHi(_wasOffsetLoIndex, Properties.Settings.Default.setAS_wasOffset);
            _message[_highSteerPwmIndex] = Properties.Settings.Default.setAS_highSteerPWM;
            _message[_lowSteerPwmIndex] = Properties.Settings.Default.setAS_lowSteerPWM;
            _message[_proportionalGainIndex] = Properties.Settings.Default.setAS_Kp;
            _message[_minSteerPwmIndex] = Properties.Settings.Default.setAS_minSteerPWM;
        }

    }
}

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

        public int ProportionalGain
        {
            set { SetInt(_proportionalGainIndex, value); }
        }

        public int HighSteerPwm
        {
            set { SetInt(_highSteerPwmIndex, value); }
        }

        public int LowSteerPwm
        {
            set { SetInt(_lowSteerPwmIndex, value); }
        }

        public int MinSteerPwm
        {
            set { SetInt(_minSteerPwmIndex, value); }
        }

        public int CountsPerDegree
        {
            set { SetInt(_countsPerDegreeIndex, value); }
        }

        public int WheelAngleSensorOffset
        {
            set { SetIntLoHi(_wasOffsetLoIndex, value); }
        }

        public int Ackerman
        {
            set { SetInt(_ackermanIndex, value); }
        }

        public byte ProportionalGainByte => _message[_proportionalGainIndex];
        public byte HighSteerPwmByte => _message[_highSteerPwmIndex];
        public byte LowSteerPwmByte => _message[_lowSteerPwmIndex];
        public byte MinSteerPwmByte => _message[_minSteerPwmIndex];
        public byte CountsPerDegreeByte => _message[_countsPerDegreeIndex];
        public byte AckermanByte => _message[_ackermanIndex];


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

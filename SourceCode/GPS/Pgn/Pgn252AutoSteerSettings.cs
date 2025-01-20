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
        private const int _ackermannIndex = 12;

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

        public int Ackermann
        {
            set { SetInt(_ackermannIndex, value); }
        }

        public byte ProportionalGainByte
        {
            get { return _message[_proportionalGainIndex]; }
            set { _message[_proportionalGainIndex] = value; }
        }
            
        public byte HighSteerPwmByte
        {
            get { return _message[_highSteerPwmIndex]; }
            set { _message[_highSteerPwmIndex] = value; }
        }

        public byte LowSteerPwmByte
        {
            get { return _message[_lowSteerPwmIndex]; }
            set { _message[_lowSteerPwmIndex] = value; }
        }

        public byte MinSteerPwmByte
        {
            get { return _message[_minSteerPwmIndex]; }
            set { _message[_minSteerPwmIndex] = value; }
        }

        public byte CountsPerDegreeByte
        {
            get { return _message[_countsPerDegreeIndex]; }
            set { _message[_countsPerDegreeIndex] = value; }
        }

        public byte AckermanByte
        {
            get { return _message[_ackermanIndex]; }
            set { _message[_ackermanIndex] = value; }
        }

    }
}

namespace AgOpenGPS
{
    public class Pgn251AutoSteerBoardConfig : PgnBase
    {
        private const int _settings0Index = 5;
        private const int _maxPulseCountIndex = 6;
        private const int _minSpeedIndex = 7;
        private const int _settings1Index = 8;
        private const int _angVelIndex  = 9;

        public Pgn251AutoSteerBoardConfig(IPgnErrorPresenter errorPresenter) : base(251, 8, errorPresenter)
        {
        }

        public byte Settings0
        {
            set { SetByte(_settings0Index, value); }
        }

        public byte Settings1
        {
            set { SetByte(_settings1Index, value); }
        }

        public byte MaxPulseCount
        {
            set { SetByte(_maxPulseCountIndex, value); ; }
        }

        public double MinSpeed
        {
            set { SetDouble(_minSpeedIndex, 10.0 * value); }
        }

        public bool AngVel
        {
            set { SetBool(_angVelIndex, value); ; }
        }

    }
}

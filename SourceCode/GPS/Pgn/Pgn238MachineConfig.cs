namespace AgOpenGPS
{
    public class Pgn238MachineConfig : PgnBase
    {
        private const int _raiseTimeIndex = 5;
        private const int _lowerTimeIndex = 6;
        private const int _settings0Index = 8;
        private const int _user1Index = 9;
        private const int _user2Index= 10;
        private const int _user3Index= 11;
        private const int _user4Index= 12;

        public Pgn238MachineConfig(
            IPgnErrorPresenter errorPresenter
        )
            :base(238, 8, errorPresenter)
        {
        }

        public byte RaiseTime
        {
            set { SetByte(_raiseTimeIndex, value); }
        }

        public byte LowerTime
        {
            set { SetByte(_lowerTimeIndex, value); }
        }

        public byte Settings0
        {
            set { SetByte(_settings0Index, value); }
        }

        public byte User1
        {
            set { SetByte(_user1Index, value); }
        }

        public byte User2
        {
            set { SetByte(_user2Index, value); }
        }

        public byte User3
        {
            set { SetByte(_user3Index, value); }
        }

        public byte User4
        {
            set { SetByte(_user4Index, value); }
        }

    }
}


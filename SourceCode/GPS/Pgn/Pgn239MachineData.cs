using System;

namespace AgOpenGPS
{
    public class Pgn239MachineData : PgnBase
    {
        private const int _uTurnIndex = 5;
        private const int _speedIndex = 6;
        private const int _hydraulicLiftIndex = 7;
        private const int _tramIndex = 8;
        private const int _sectionControl1to8Index = 11;

        public Pgn239MachineData(
            IPgnErrorPresenter errorPresenter
        )
            : base(239, 8, errorPresenter)
        {
        }

        public bool UTurn
        {
            set { SetBool(_uTurnIndex, value); }
        }

        public double Speed
        {
            set { SetDouble(_speedIndex, 10.0 * value); }
        }

        public byte HydraulicLift
        {
            set { SetByte(_hydraulicLiftIndex, value); }
        }

        public byte Tram
        {
            set { SetByte(_tramIndex, value); }
        }

        public UInt16 SectionControl1to16
        {
            set { SetUInt16LoHi(_sectionControl1to8Index, value); }
        }

    }
}

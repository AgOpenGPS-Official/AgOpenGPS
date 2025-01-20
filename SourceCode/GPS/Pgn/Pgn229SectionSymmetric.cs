using System;

namespace AgOpenGPS
{
    public class Pgn229SectionSymmetric : PgnBase
    {
        private const int _sectionControl1to8Index = 5;
        private const int _toolFarLeftSpeedIndex = 13;
        private const int _toolFarRightSpeedIndex = 14;

        public Pgn229SectionSymmetric(IPgnErrorPresenter errorPresenter) : base(229, 10, errorPresenter)
        {
        }

        public double ToolFarLeftSpeed
        {
            set { SetDouble(_toolFarLeftSpeedIndex, 10 * value); }
        }

        public double ToolFarRightSpeed
        {
            set { SetDouble(_toolFarRightSpeedIndex, 10 * value); }
        }

        public UInt64 SectionControl1to64
        {
            set
            {
                for (int i = 0; i < 8; i++)
                {
                    SetByte(_sectionControl1to8Index + i, (byte)(value >> (8 * i)));
                }
            }
        }

        public UInt16 SectionControl1to16
        {
            get { return GetUInt16(_sectionControl1to8Index); }
            set { SetUInt16LoHi(_sectionControl1to8Index, value); }
        }

    }
}

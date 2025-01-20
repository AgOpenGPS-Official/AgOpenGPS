using System;

namespace AgOpenGPS
{
    public class Pgn254AutoSteerData : PgnBase
    {
        private const int _speedLoIndex = 5;
        private const int _speedHiIndex = 6;
        private const int _statusIndex = 7;
        private const int _steerAngleLoIndex = 8;
        private const int _steerAngleHiIndex = 9;
        private const int _lineDistanceIndex = 10;
        private const int _sectionControl1to8Index = 11;
        private const int _sectionControl9to16Index = 12;

        public Pgn254AutoSteerData(IPgnErrorPresenter errorPresenter) : base(254, 8, errorPresenter)
        {
        }

        public void SetDist(bool isOff, short guidanceLineDistanceOffInCm)
        {
            if (isOff)
            {
                _message[_lineDistanceIndex] = 255;
            }
            else
            {
                int distanceX2 = (int)(guidanceLineDistanceOffInCm * 0.05);

                distanceX2 = Math.Max(distanceX2, -127);
                distanceX2 = Math.Min(distanceX2, 127);
                distanceX2 += 127;
                _message[_lineDistanceIndex] = unchecked((byte)distanceX2);
            }
        }

        public double SpeedInKmh
        {
            set { SetDoubleLoHi(_speedLoIndex, 10.0 * Math.Abs(value)); }
        }

        public bool IsAutoSteerOn
        {
            set { SetBool(_statusIndex, value); }
            get { return GetBool(_statusIndex); }
        }

        public Int16 GuidanceLineSteerAngle
        {
            set { SetInt16LoHi(_steerAngleLoIndex, value); }
        }

        public UInt16 SectionControl1to16
        {
            get { return GetUInt16(_sectionControl1to8Index); }
            set { SetUInt16LoHi(_sectionControl1to8Index, value); }
        }

    }
}

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

        public void SetSpeedInKmh(double speed)
        {
            double scaledSpeed = 10.0 * Math.Abs(speed);
            SetDoubleLoHi(_speedLoIndex, scaledSpeed);
        }

        public void SetStatus(bool isOn)
        {
            SetBool(_statusIndex, isOn);
        }

        public bool GetStatus()
        {
            return 0 != _message[_statusIndex];
        }

        public void SetGuidanceLineSteerAngle(Int16 angle)
        {
            SetInt16LoHi(_steerAngleLoIndex, angle);
        }

        public void SetSectionControl1to16(UInt16 bits)
        {
            SetUInt16LoHi(_sectionControl1to8Index, bits);
        }

        public byte GetSc1to8()
        {
            return _message[_sectionControl1to8Index];
        }

        public byte GetSc9to16()
        {
            return _message[_sectionControl9to16Index];
        }

    }
}

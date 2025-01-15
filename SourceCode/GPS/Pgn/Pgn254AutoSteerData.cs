using System;

namespace AgOpenGPS
{
    public class Pgn254AutoSteerData : PgnBase
    {
        private const int _speedLo = 5;
        private const int _speedHi = 6;
        private const int _status = 7;
        private const int _steerAngleLo = 8;
        private const int _steerAngleHi = 9;
        private const int _lineDistance = 10;
        private const int _sc1to8 = 11;
        private const int _sc9to16 = 12;

        public Pgn254AutoSteerData() : base(254, 8)
        {
        }

        public void SetDist(bool isOff, short guidanceLineDistanceOffInCm)
        {
            if (isOff)
            {
                message[_lineDistance] = 255;
            }
            else
            {
                int distanceX2 = (int)(guidanceLineDistanceOffInCm * 0.05);

                distanceX2 = Math.Max(distanceX2, -127);
                distanceX2 = Math.Min(distanceX2, 127);
                distanceX2 += 127;
                message[_lineDistance] = unchecked((byte)distanceX2);
            }
        }

        public void SetSpeedInKmh(double speed)
        {
            int scaledSpeed = (int)(10.0 * Math.Abs(speed));
            message[_speedLo] = unchecked((byte)scaledSpeed);
            message[_speedHi] = unchecked((byte)(scaledSpeed >> 8));
        }

        public void SetStatus(bool isOn)
        {
            message[_status] = (byte)(isOn ? 1 : 0);
        }

        public bool GetStatus()
        {
            return 0 != message[_status];
        }

        public void SetGuidanceLineSteerAngle(short angle)
        {
            message[_steerAngleHi] = unchecked((byte)(angle >> 8));
            message[_steerAngleLo] = unchecked((byte)(angle));
        }

        public void SetSectionControl1to16(UInt16 bits)
        {
            message[_sc1to8] = unchecked((byte)bits);
            message[_sc9to16] = unchecked((byte)(bits >> 8));
        }

        public byte GetSc1to8()
        {
            return message[_sc1to8];
        }

        public byte GetSc9to16()
        {
            return message[_sc9to16];
        }

    }
}

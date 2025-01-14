using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using static AgOpenGPS.FormGPS;

namespace AgOpenGPS
{
    public class Pgn254AutoSteerData
    {
        private byte[] _message = new byte[] { 0x80, 0x81, 0x7f, 0xFE, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0xCC };
        private const int _speedLo = 5;
        private const int _speedHi = 6;
        private const int _status = 7;
        private const int _steerAngleLo = 8;
        private const int _steerAngleHi = 9;
        private const int _lineDistance = 10;
        private const int _sc1to8 = 11;
        private const int _sc9to16 = 12;

        public void SetDist(bool isOff, short guidanceLineDistanceOffInCm)
        {
            if (isOff)
            {
                _message[_lineDistance] = 255;
            }
            else
            {
                int distanceX2 = (int)(guidanceLineDistanceOffInCm * 0.05);

                distanceX2 = Math.Max(distanceX2, -127);
                distanceX2 = Math.Min(distanceX2, 127);
                distanceX2 += 127;
                _message[_lineDistance] = unchecked((byte)distanceX2);
            }
        }

        public void SetSpeedInKmh(double speed)
        {
            int scaledSpeed = (int)(10.0 * Math.Abs(speed));
            _message[_speedLo] = unchecked((byte)scaledSpeed);
            _message[_speedHi] = unchecked((byte)(scaledSpeed >> 8));
        }

        public void SetStatus(bool isOn)
        {
            _message[_status] = (byte)(isOn ? 1 : 0);
        }

        public bool GetStatus()
        {
            return 0 != _message[_status];
        }

        public void SetGuidanceLineSteerAngle(short angle)
        {
            _message[_steerAngleHi] = unchecked((byte)(angle >> 8));
            _message[_steerAngleLo] = unchecked((byte)(angle));
        }

        public void SetSectionControl1to16(UInt16 bits)
        {
            _message[_sc1to8] = unchecked((byte)bits);
            _message[_sc9to16] = unchecked((byte)(bits >> 8));
        }

        public byte GetSc1to8()
        {
            return _message[_sc1to8];
        }

        public byte GetSc9to16()
        {
            return _message[_sc9to16];
        }

        public void AssertEqual(CPGN_FE p )
        {
            for (int i = 0; i < _message.Length - 1; i++)
            {
                byte a = _message[i];
                byte b = p.pgn[i];

                if (a != b)
                {

                }
                Debug.Assert(_message[i] == p.pgn[i]);
            }
        }

    }
}

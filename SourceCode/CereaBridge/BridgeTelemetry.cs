using System;

namespace CereaBridge
{
    internal sealed partial class BridgeService
    {
        private void OnTelemetryTick(object? state)
        {
            try
            {
                var counts = ReadCounts();
                var countsPerDegree = _countsPerDegree <= 0 ? _cfg.CountsPerDegreeFallback : _countsPerDegree;
                var actualDeg = (counts - _wasOffset) / countsPerDegree;
                if (_cfg.ReverseWas) actualDeg = -actualDeg;

                short heading16 = 0;
                short roll16 = 0;
                ReadImu(ref heading16, ref roll16);

                var errorDeg = _desiredAngleDeg - actualDeg;
                var speedOk = _speedKph >= (_minSpeedX10 * 0.1);
                var shouldDrive = _autosteerEnabled && speedOk;
                var pwm = 0;
                var motorVelocity = 0.0;

                if (shouldDrive)
                {
                    var absError = Math.Abs(errorDeg);
                    if (absError > _cfg.DeadbandDegrees)
                    {
                        pwm = (int)Math.Round((_kp * absError) + _minPwm);
                        if (pwm > _highPwm) pwm = _highPwm;
                        motorVelocity = (pwm / 255.0) * _cfg.VelocityGainMultiplier;
                        if (motorVelocity > _cfg.MaxMotorOutput) motorVelocity = _cfg.MaxMotorOutput;
                        if (errorDeg < 0) motorVelocity = -motorVelocity;
                    }
                }

                if (_cfg.ReverseMotor) motorVelocity = -motorVelocity;
                if (_motor != null) _motor.TargetVelocity = motorVelocity;

                _lastPwm = pwm;
                SendSteerTelemetry((short)Math.Round(actualDeg * 100.0), heading16, roll16, BuildSwitchByte(), (byte)Math.Max(0, Math.Min(255, pwm)));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void OnHelloTick(object? state)
        {
            try
            {
                var counts = ReadCounts();
                var countsLo = (byte)(counts & 0xFF);
                var countsHi = (byte)((counts >> 8) & 0xFF);
                var countsPerDegree = _countsPerDegree <= 0 ? _cfg.CountsPerDegreeFallback : _countsPerDegree;
                var actualDeg = (counts - _wasOffset) / countsPerDegree;
                if (_cfg.ReverseWas) actualDeg = -actualDeg;
                var actualDegX100 = (short)Math.Round(actualDeg * 100.0);
                var actualLo = (byte)(actualDegX100 & 0xFF);
                var actualHi = (byte)((actualDegX100 >> 8) & 0xFF);
                var packet = new byte[] { 0x80, 0x81, 0x7F, 126, 5, actualLo, actualHi, countsLo, countsHi, BuildSwitchByte(), 0 };
                SendPacket(packet);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private byte BuildSwitchByte()
        {
            byte value = 0;
            if (_cfg.WorkSwitchOn) value |= 0x01;
            if (_cfg.SteerSwitchOn) value |= 0x02;
            return value;
        }

        private void SendSteerTelemetry(short actualAngleX100, short heading16, short roll16, byte switchByte, byte pwm)
        {
            var packet = new byte[14];
            packet[0] = 0x80;
            packet[1] = 0x81;
            packet[2] = 0x7F;
            packet[3] = 0xFD;
            packet[4] = 8;
            packet[5] = (byte)(actualAngleX100 & 0xFF);
            packet[6] = (byte)((actualAngleX100 >> 8) & 0xFF);
            packet[7] = (byte)(heading16 & 0xFF);
            packet[8] = (byte)((heading16 >> 8) & 0xFF);
            packet[9] = (byte)(roll16 & 0xFF);
            packet[10] = (byte)((roll16 >> 8) & 0xFF);
            packet[11] = switchByte;
            packet[12] = pwm;
            packet[13] = 0;
            SendPacket(packet);
        }
    }
}

using Phidget22.Devices;
using Tinkerforge;

namespace CereaBridge
{
    internal sealed partial class BridgeService
    {
        private void OpenDevices()
        {
            if (_cfg.UsePhidgets)
            {
                _motor = new DCMotor();
                _encoder = new Encoder();

                if (_cfg.PhidgetsDeviceSerialNumber > 0)
                {
                    _motor.DeviceSerialNumber = _cfg.PhidgetsDeviceSerialNumber;
                    _encoder.DeviceSerialNumber = _cfg.PhidgetsDeviceSerialNumber;
                }

                _motor.Channel = _cfg.PhidgetsMotorChannel;
                _encoder.Channel = _cfg.PhidgetsEncoderChannel;
                _motor.Open(5000);
                _encoder.Open(5000);
                _motor.Acceleration = _motor.MaxAcceleration;
                _motor.TargetVelocity = 0;
            }

            if (_cfg.UseImuBrick && !string.IsNullOrWhiteSpace(_cfg.ImuUid))
            {
                _ipcon = new IPConnection();
                _ipcon.Connect(_cfg.ImuHost, _cfg.ImuPort);
                _imu = new BrickIMUV2(_cfg.ImuUid, _ipcon);
            }
        }

        private void CloseDevices()
        {
            if (_motor != null)
            {
                try { _motor.TargetVelocity = 0; } catch { }
                try { _motor.Close(); } catch { }
            }

            if (_encoder != null)
            {
                try { _encoder.Close(); } catch { }
            }

            if (_ipcon != null)
            {
                try { _ipcon.Disconnect(); } catch { }
            }
        }

        private int ReadCounts()
        {
            if (_encoder == null) return _wasOffset;
            return (int)_encoder.Position;
        }

        private void ReadImu(ref short heading16, ref short roll16)
        {
            if (_imu == null) return;

            short heading;
            short roll;
            short pitch;
            _imu.GetOrientation(out heading, out roll, out pitch);

            heading16 = (short)(heading + _cfg.HeadingOffset16);
            roll16 = roll;

            if (_cfg.ReverseHeading) heading16 = (short)-heading16;
            if (_cfg.ReverseRoll) roll16 = (short)-roll16;
        }
    }
}

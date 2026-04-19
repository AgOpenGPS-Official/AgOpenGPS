using System;
using Phidget22.Devices;
using Tinkerforge;

namespace CereaBridge
{
    internal sealed partial class BridgeService
    {
        private void OpenDevices()
        {
            IsMotorConnected = false;
            IsEncoderConnected = false;
            IsImuConnected = false;

            if (_cfg.UsePhidgets)
            {
                try
                {
                    _motor = new DCMotor();
                    if (_cfg.PhidgetsDeviceSerialNumber > 0)
                    {
                        _motor.DeviceSerialNumber = _cfg.PhidgetsDeviceSerialNumber;
                    }
                    _motor.Channel = _cfg.PhidgetsMotorChannel;
                    _motor.Open(5000);
                    _motor.Acceleration = _motor.MaxAcceleration;
                    _motor.TargetVelocity = 0;
                    IsMotorConnected = true;
                    Console.WriteLine("Phidgets motor connected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Phidgets motor not connected: " + ex.Message);
                }

                try
                {
                    _encoder = new Encoder();
                    if (_cfg.PhidgetsDeviceSerialNumber > 0)
                    {
                        _encoder.DeviceSerialNumber = _cfg.PhidgetsDeviceSerialNumber;
                    }
                    _encoder.Channel = _cfg.PhidgetsEncoderChannel;
                    _encoder.Open(5000);
                    IsEncoderConnected = true;
                    Console.WriteLine("Phidgets encoder connected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Phidgets encoder not connected: " + ex.Message);
                }
            }

            if (_cfg.UseImuBrick && !string.IsNullOrWhiteSpace(_cfg.ImuUid))
            {
                try
                {
                    _ipcon = new IPConnection();
                    _ipcon.Connect(_cfg.ImuHost, _cfg.ImuPort);
                    _imu = new BrickIMUV2(_cfg.ImuUid, _ipcon);
                    IsImuConnected = true;
                    Console.WriteLine("IMU Brick connected.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("IMU Brick not connected: " + ex.Message);
                }
            }
            else if (_cfg.UseImuBrick)
            {
                Console.WriteLine("IMU Brick skipped: UID not set.");
            }
        }

        private void CloseDevices()
        {
            if (_motor != null)
            {
                try { _motor.TargetVelocity = 0; } catch { }
                try { _motor.Close(); } catch { }
                _motor = null;
            }

            if (_encoder != null)
            {
                try { _encoder.Close(); } catch { }
                _encoder = null;
            }

            if (_ipcon != null)
            {
                try { _ipcon.Disconnect(); } catch { }
                _ipcon = null;
            }

            _imu = null;
            IsMotorConnected = false;
            IsEncoderConnected = false;
            IsImuConnected = false;
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

using System;
using System.Configuration;
using System.Globalization;

namespace CereaBridge
{
    internal sealed class BridgeConfig
    {
        public int ListenPort = 18888;
        public int ListenPortFallback = 8888;
        public string AgioHost = "127.0.0.1";
        public int AgioPort = 9999;
        public bool UsePhidgets = true;
        public int PhidgetsDeviceSerialNumber = 0;
        public int PhidgetsMotorChannel = 0;
        public int PhidgetsEncoderChannel = 0;
        public bool ReverseMotor = false;
        public bool ReverseWas = false;
        public bool UseImuBrick = true;
        public string ImuHost = "localhost";
        public int ImuPort = 4223;
        public string ImuUid = "";
        public bool ReverseRoll = false;
        public bool ReverseHeading = false;
        public short HeadingOffset16 = 0;
        public double CountsPerDegreeFallback = 30.0;
        public int WasOffsetFallback = 0;
        public double VelocityGainMultiplier = 1.0;
        public double MaxMotorOutput = 1.0;
        public double DeadbandDegrees = 0.25;
        public int TelemetryPeriodMs = 50;
        public int HelloPeriodMs = 250;
        public bool SteerSwitchOn = true;
        public bool WorkSwitchOn = false;

        public static BridgeConfig Load()
        {
            var cfg = new BridgeConfig();
            cfg.ListenPort = GetInt("ListenPort", cfg.ListenPort);
            cfg.ListenPortFallback = GetInt("ListenPortFallback", cfg.ListenPortFallback);
            cfg.AgioHost = GetString("AgioHost", cfg.AgioHost);
            cfg.AgioPort = GetInt("AgioPort", cfg.AgioPort);
            cfg.UsePhidgets = GetBool("UsePhidgets", cfg.UsePhidgets);
            cfg.PhidgetsDeviceSerialNumber = GetInt("PhidgetsDeviceSerialNumber", cfg.PhidgetsDeviceSerialNumber);
            cfg.PhidgetsMotorChannel = GetInt("PhidgetsMotorChannel", cfg.PhidgetsMotorChannel);
            cfg.PhidgetsEncoderChannel = GetInt("PhidgetsEncoderChannel", cfg.PhidgetsEncoderChannel);
            cfg.ReverseMotor = GetBool("ReverseMotor", cfg.ReverseMotor);
            cfg.ReverseWas = GetBool("ReverseWas", cfg.ReverseWas);
            cfg.UseImuBrick = GetBool("UseImuBrick", cfg.UseImuBrick);
            cfg.ImuHost = GetString("ImuHost", cfg.ImuHost);
            cfg.ImuPort = GetInt("ImuPort", cfg.ImuPort);
            cfg.ImuUid = GetString("ImuUid", cfg.ImuUid);
            cfg.ReverseRoll = GetBool("ReverseRoll", cfg.ReverseRoll);
            cfg.ReverseHeading = GetBool("ReverseHeading", cfg.ReverseHeading);
            cfg.HeadingOffset16 = (short)GetInt("HeadingOffset16", cfg.HeadingOffset16);
            cfg.CountsPerDegreeFallback = GetDouble("CountsPerDegreeFallback", cfg.CountsPerDegreeFallback);
            cfg.WasOffsetFallback = GetInt("WasOffsetFallback", cfg.WasOffsetFallback);
            cfg.VelocityGainMultiplier = GetDouble("VelocityGainMultiplier", cfg.VelocityGainMultiplier);
            cfg.MaxMotorOutput = GetDouble("MaxMotorOutput", cfg.MaxMotorOutput);
            cfg.DeadbandDegrees = GetDouble("DeadbandDegrees", cfg.DeadbandDegrees);
            cfg.TelemetryPeriodMs = GetInt("TelemetryPeriodMs", cfg.TelemetryPeriodMs);
            cfg.HelloPeriodMs = GetInt("HelloPeriodMs", cfg.HelloPeriodMs);
            cfg.SteerSwitchOn = GetBool("SteerSwitchOn", cfg.SteerSwitchOn);
            cfg.WorkSwitchOn = GetBool("WorkSwitchOn", cfg.WorkSwitchOn);
            return cfg;
        }

        private static string GetString(string key, string fallback)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private static int GetInt(string key, int fallback)
        {
            var value = ConfigurationManager.AppSettings[key];
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : fallback;
        }

        private static double GetDouble(string key, double fallback)
        {
            var value = ConfigurationManager.AppSettings[key];
            return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) ? parsed : fallback;
        }

        private static bool GetBool(string key, bool fallback)
        {
            var value = ConfigurationManager.AppSettings[key];
            return bool.TryParse(value, out var parsed) ? parsed : fallback;
        }
    }
}

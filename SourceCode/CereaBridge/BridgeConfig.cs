using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;

namespace CereaBridge
{
    internal sealed class BridgeConfig
    {
        public const string ProfileFileName = "CereaBridge.profile.ini";

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

        public static string GetDefaultProfilePath()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ProfileFileName);
        }

        public static BridgeConfig Load()
        {
            var cfg = new BridgeConfig();
            cfg.LoadFromAppSettings();
            cfg.LoadFromProfileIfExists(GetDefaultProfilePath());
            return cfg;
        }

        public IEnumerable<string> BuildSummaryLines()
        {
            yield return "UDP listen: " + ListenPort.ToString(CultureInfo.InvariantCulture) + " / " + ListenPortFallback.ToString(CultureInfo.InvariantCulture);
            yield return "AgIO target: " + AgioHost + ":" + AgioPort.ToString(CultureInfo.InvariantCulture);
            yield return "Phidgets: " + (UsePhidgets ? "enabled" : "disabled") + ", serial=" + PhidgetsDeviceSerialNumber.ToString(CultureInfo.InvariantCulture) + ", motorCh=" + PhidgetsMotorChannel.ToString(CultureInfo.InvariantCulture) + ", encoderCh=" + PhidgetsEncoderChannel.ToString(CultureInfo.InvariantCulture);
            yield return "IMU Brick: " + (UseImuBrick ? "enabled" : "disabled") + ", host=" + ImuHost + ":" + ImuPort.ToString(CultureInfo.InvariantCulture) + ", uid=" + (string.IsNullOrWhiteSpace(ImuUid) ? "<empty>" : ImuUid);
            yield return "Reverse flags: motor=" + ReverseMotor.ToString() + ", was=" + ReverseWas.ToString() + ", heading=" + ReverseHeading.ToString() + ", roll=" + ReverseRoll.ToString();
            yield return "Fallbacks: counts/deg=" + CountsPerDegreeFallback.ToString(CultureInfo.InvariantCulture) + ", wasOffset=" + WasOffsetFallback.ToString(CultureInfo.InvariantCulture);
            yield return "Motor tuning: gainMul=" + VelocityGainMultiplier.ToString(CultureInfo.InvariantCulture) + ", maxOut=" + MaxMotorOutput.ToString(CultureInfo.InvariantCulture) + ", deadband=" + DeadbandDegrees.ToString(CultureInfo.InvariantCulture);
            yield return "Timing: telemetry=" + TelemetryPeriodMs.ToString(CultureInfo.InvariantCulture) + " ms, hello=" + HelloPeriodMs.ToString(CultureInfo.InvariantCulture) + " ms";
        }

        public IEnumerable<string> BuildWarningLines()
        {
            if (UsePhidgets && PhidgetsMotorChannel == PhidgetsEncoderChannel)
            {
                yield return "Motor and encoder channel are both set to " + PhidgetsMotorChannel.ToString(CultureInfo.InvariantCulture) + ". Check if that is correct for your hardware.";
            }

            if (UseImuBrick && string.IsNullOrWhiteSpace(ImuUid))
            {
                yield return "IMU Brick is enabled but ImuUid is empty.";
            }

            if (CountsPerDegreeFallback <= 0)
            {
                yield return "CountsPerDegreeFallback must be greater than zero.";
            }

            if (MaxMotorOutput <= 0 || MaxMotorOutput > 1.0)
            {
                yield return "MaxMotorOutput should normally be between 0.0 and 1.0.";
            }

            if (TelemetryPeriodMs < 20)
            {
                yield return "TelemetryPeriodMs is very low. That can make debugging harder.";
            }
        }

        public void SaveProfile(string path)
        {
            var lines = new List<string>
            {
                "ListenPort=" + ListenPort.ToString(CultureInfo.InvariantCulture),
                "ListenPortFallback=" + ListenPortFallback.ToString(CultureInfo.InvariantCulture),
                "AgioHost=" + AgioHost,
                "AgioPort=" + AgioPort.ToString(CultureInfo.InvariantCulture),
                "UsePhidgets=" + UsePhidgets.ToString(),
                "PhidgetsDeviceSerialNumber=" + PhidgetsDeviceSerialNumber.ToString(CultureInfo.InvariantCulture),
                "PhidgetsMotorChannel=" + PhidgetsMotorChannel.ToString(CultureInfo.InvariantCulture),
                "PhidgetsEncoderChannel=" + PhidgetsEncoderChannel.ToString(CultureInfo.InvariantCulture),
                "ReverseMotor=" + ReverseMotor.ToString(),
                "ReverseWas=" + ReverseWas.ToString(),
                "UseImuBrick=" + UseImuBrick.ToString(),
                "ImuHost=" + ImuHost,
                "ImuPort=" + ImuPort.ToString(CultureInfo.InvariantCulture),
                "ImuUid=" + ImuUid,
                "ReverseRoll=" + ReverseRoll.ToString(),
                "ReverseHeading=" + ReverseHeading.ToString(),
                "HeadingOffset16=" + HeadingOffset16.ToString(CultureInfo.InvariantCulture),
                "CountsPerDegreeFallback=" + CountsPerDegreeFallback.ToString(CultureInfo.InvariantCulture),
                "WasOffsetFallback=" + WasOffsetFallback.ToString(CultureInfo.InvariantCulture),
                "VelocityGainMultiplier=" + VelocityGainMultiplier.ToString(CultureInfo.InvariantCulture),
                "MaxMotorOutput=" + MaxMotorOutput.ToString(CultureInfo.InvariantCulture),
                "DeadbandDegrees=" + DeadbandDegrees.ToString(CultureInfo.InvariantCulture),
                "TelemetryPeriodMs=" + TelemetryPeriodMs.ToString(CultureInfo.InvariantCulture),
                "HelloPeriodMs=" + HelloPeriodMs.ToString(CultureInfo.InvariantCulture),
                "SteerSwitchOn=" + SteerSwitchOn.ToString(),
                "WorkSwitchOn=" + WorkSwitchOn.ToString()
            };

            File.WriteAllLines(path, lines.ToArray());
        }

        private void LoadFromProfileIfExists(string path)
        {
            if (!File.Exists(path)) return;

            foreach (var rawLine in File.ReadAllLines(path))
            {
                var line = rawLine.Trim();
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;

                var idx = line.IndexOf('=');
                if (idx <= 0) continue;

                var key = line.Substring(0, idx).Trim();
                var value = line.Substring(idx + 1).Trim();
                ApplyValue(key, value);
            }
        }

        private void LoadFromAppSettings()
        {
            ListenPort = GetInt("ListenPort", ListenPort);
            ListenPortFallback = GetInt("ListenPortFallback", ListenPortFallback);
            AgioHost = GetString("AgioHost", AgioHost);
            AgioPort = GetInt("AgioPort", AgioPort);
            UsePhidgets = GetBool("UsePhidgets", UsePhidgets);
            PhidgetsDeviceSerialNumber = GetInt("PhidgetsDeviceSerialNumber", PhidgetsDeviceSerialNumber);
            PhidgetsMotorChannel = GetInt("PhidgetsMotorChannel", PhidgetsMotorChannel);
            PhidgetsEncoderChannel = GetInt("PhidgetsEncoderChannel", PhidgetsEncoderChannel);
            ReverseMotor = GetBool("ReverseMotor", ReverseMotor);
            ReverseWas = GetBool("ReverseWas", ReverseWas);
            UseImuBrick = GetBool("UseImuBrick", UseImuBrick);
            ImuHost = GetString("ImuHost", ImuHost);
            ImuPort = GetInt("ImuPort", ImuPort);
            ImuUid = GetString("ImuUid", ImuUid);
            ReverseRoll = GetBool("ReverseRoll", ReverseRoll);
            ReverseHeading = GetBool("ReverseHeading", ReverseHeading);
            HeadingOffset16 = (short)GetInt("HeadingOffset16", HeadingOffset16);
            CountsPerDegreeFallback = GetDouble("CountsPerDegreeFallback", CountsPerDegreeFallback);
            WasOffsetFallback = GetInt("WasOffsetFallback", WasOffsetFallback);
            VelocityGainMultiplier = GetDouble("VelocityGainMultiplier", VelocityGainMultiplier);
            MaxMotorOutput = GetDouble("MaxMotorOutput", MaxMotorOutput);
            DeadbandDegrees = GetDouble("DeadbandDegrees", DeadbandDegrees);
            TelemetryPeriodMs = GetInt("TelemetryPeriodMs", TelemetryPeriodMs);
            HelloPeriodMs = GetInt("HelloPeriodMs", HelloPeriodMs);
            SteerSwitchOn = GetBool("SteerSwitchOn", SteerSwitchOn);
            WorkSwitchOn = GetBool("WorkSwitchOn", WorkSwitchOn);
        }

        private void ApplyValue(string key, string value)
        {
            switch (key)
            {
                case "ListenPort": ListenPort = ParseInt(value, ListenPort); break;
                case "ListenPortFallback": ListenPortFallback = ParseInt(value, ListenPortFallback); break;
                case "AgioHost": AgioHost = value; break;
                case "AgioPort": AgioPort = ParseInt(value, AgioPort); break;
                case "UsePhidgets": UsePhidgets = ParseBool(value, UsePhidgets); break;
                case "PhidgetsDeviceSerialNumber": PhidgetsDeviceSerialNumber = ParseInt(value, PhidgetsDeviceSerialNumber); break;
                case "PhidgetsMotorChannel": PhidgetsMotorChannel = ParseInt(value, PhidgetsMotorChannel); break;
                case "PhidgetsEncoderChannel": PhidgetsEncoderChannel = ParseInt(value, PhidgetsEncoderChannel); break;
                case "ReverseMotor": ReverseMotor = ParseBool(value, ReverseMotor); break;
                case "ReverseWas": ReverseWas = ParseBool(value, ReverseWas); break;
                case "UseImuBrick": UseImuBrick = ParseBool(value, UseImuBrick); break;
                case "ImuHost": ImuHost = value; break;
                case "ImuPort": ImuPort = ParseInt(value, ImuPort); break;
                case "ImuUid": ImuUid = value; break;
                case "ReverseRoll": ReverseRoll = ParseBool(value, ReverseRoll); break;
                case "ReverseHeading": ReverseHeading = ParseBool(value, ReverseHeading); break;
                case "HeadingOffset16": HeadingOffset16 = (short)ParseInt(value, HeadingOffset16); break;
                case "CountsPerDegreeFallback": CountsPerDegreeFallback = ParseDouble(value, CountsPerDegreeFallback); break;
                case "WasOffsetFallback": WasOffsetFallback = ParseInt(value, WasOffsetFallback); break;
                case "VelocityGainMultiplier": VelocityGainMultiplier = ParseDouble(value, VelocityGainMultiplier); break;
                case "MaxMotorOutput": MaxMotorOutput = ParseDouble(value, MaxMotorOutput); break;
                case "DeadbandDegrees": DeadbandDegrees = ParseDouble(value, DeadbandDegrees); break;
                case "TelemetryPeriodMs": TelemetryPeriodMs = ParseInt(value, TelemetryPeriodMs); break;
                case "HelloPeriodMs": HelloPeriodMs = ParseInt(value, HelloPeriodMs); break;
                case "SteerSwitchOn": SteerSwitchOn = ParseBool(value, SteerSwitchOn); break;
                case "WorkSwitchOn": WorkSwitchOn = ParseBool(value, WorkSwitchOn); break;
            }
        }

        private static string GetString(string key, string fallback)
        {
            var value = ConfigurationManager.AppSettings[key];
            return string.IsNullOrWhiteSpace(value) ? fallback : value;
        }

        private static int GetInt(string key, int fallback)
        {
            return ParseInt(ConfigurationManager.AppSettings[key], fallback);
        }

        private static double GetDouble(string key, double fallback)
        {
            return ParseDouble(ConfigurationManager.AppSettings[key], fallback);
        }

        private static bool GetBool(string key, bool fallback)
        {
            return ParseBool(ConfigurationManager.AppSettings[key], fallback);
        }

        private static int ParseInt(string value, int fallback)
        {
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : fallback;
        }

        private static double ParseDouble(string value, double fallback)
        {
            return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) ? parsed : fallback;
        }

        private static bool ParseBool(string value, bool fallback)
        {
            return bool.TryParse(value, out var parsed) ? parsed : fallback;
        }
    }
}

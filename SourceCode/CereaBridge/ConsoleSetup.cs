using System;
using System.Globalization;

namespace CereaBridge
{
    internal static class ConsoleSetup
    {
        public static void Run(BridgeConfig cfg)
        {
            Console.WriteLine("=== CereaBridge setup ===");
            Console.WriteLine("Leave empty to keep current value.");
            Console.WriteLine();

            cfg.ListenPort = AskInt("ListenPort", cfg.ListenPort);
            cfg.ListenPortFallback = AskInt("ListenPortFallback", cfg.ListenPortFallback);
            cfg.AgioHost = AskString("AgioHost", cfg.AgioHost);
            cfg.AgioPort = AskInt("AgioPort", cfg.AgioPort);

            cfg.UsePhidgets = AskBool("UsePhidgets", cfg.UsePhidgets);
            cfg.PhidgetsDeviceSerialNumber = AskInt("PhidgetsDeviceSerialNumber", cfg.PhidgetsDeviceSerialNumber);
            cfg.PhidgetsMotorChannel = AskInt("PhidgetsMotorChannel", cfg.PhidgetsMotorChannel);
            cfg.PhidgetsEncoderChannel = AskInt("PhidgetsEncoderChannel", cfg.PhidgetsEncoderChannel);
            cfg.ReverseMotor = AskBool("ReverseMotor", cfg.ReverseMotor);
            cfg.ReverseWas = AskBool("ReverseWas", cfg.ReverseWas);

            cfg.UseImuBrick = AskBool("UseImuBrick", cfg.UseImuBrick);
            cfg.ImuHost = AskString("ImuHost", cfg.ImuHost);
            cfg.ImuPort = AskInt("ImuPort", cfg.ImuPort);
            cfg.ImuUid = AskString("ImuUid", cfg.ImuUid);
            cfg.ReverseRoll = AskBool("ReverseRoll", cfg.ReverseRoll);
            cfg.ReverseHeading = AskBool("ReverseHeading", cfg.ReverseHeading);
            cfg.HeadingOffset16 = (short)AskInt("HeadingOffset16", cfg.HeadingOffset16);

            cfg.CountsPerDegreeFallback = AskDouble("CountsPerDegreeFallback", cfg.CountsPerDegreeFallback);
            cfg.WasOffsetFallback = AskInt("WasOffsetFallback", cfg.WasOffsetFallback);
            cfg.VelocityGainMultiplier = AskDouble("VelocityGainMultiplier", cfg.VelocityGainMultiplier);
            cfg.MaxMotorOutput = AskDouble("MaxMotorOutput", cfg.MaxMotorOutput);
            cfg.DeadbandDegrees = AskDouble("DeadbandDegrees", cfg.DeadbandDegrees);
            cfg.TelemetryPeriodMs = AskInt("TelemetryPeriodMs", cfg.TelemetryPeriodMs);
            cfg.HelloPeriodMs = AskInt("HelloPeriodMs", cfg.HelloPeriodMs);
            cfg.SteerSwitchOn = AskBool("SteerSwitchOn", cfg.SteerSwitchOn);
            cfg.WorkSwitchOn = AskBool("WorkSwitchOn", cfg.WorkSwitchOn);

            var profilePath = BridgeConfig.GetDefaultProfilePath();
            cfg.SaveProfile(profilePath);
            Console.WriteLine();
            Console.WriteLine("Saved profile: " + profilePath);
            Console.WriteLine();
        }

        private static string AskString(string name, string current)
        {
            Console.Write(name + " [" + current + "]: ");
            var value = Console.ReadLine();
            return string.IsNullOrWhiteSpace(value) ? current : value.Trim();
        }

        private static int AskInt(string name, int current)
        {
            Console.Write(name + " [" + current.ToString(CultureInfo.InvariantCulture) + "]: ");
            var value = Console.ReadLine();
            return int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var parsed) ? parsed : current;
        }

        private static double AskDouble(string name, double current)
        {
            Console.Write(name + " [" + current.ToString(CultureInfo.InvariantCulture) + "]: ");
            var value = Console.ReadLine();
            return double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsed) ? parsed : current;
        }

        private static bool AskBool(string name, bool current)
        {
            Console.Write(name + " [" + current.ToString() + "]: ");
            var value = Console.ReadLine();
            return bool.TryParse(value, out var parsed) ? parsed : current;
        }
    }
}

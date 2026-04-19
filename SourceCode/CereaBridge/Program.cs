using System;
using System.IO;
using System.Linq;

namespace CereaBridge
{
    internal static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var cfg = BridgeConfig.Load();
            var profilePath = BridgeConfig.GetDefaultProfilePath();
            var forceSetup = args.Any(a => string.Equals(a, "--setup", StringComparison.OrdinalIgnoreCase));
            var firstRun = !File.Exists(profilePath);

            if (forceSetup || firstRun)
            {
                ConsoleSetup.Run(cfg);
            }

            try
            {
                using (var bridge = new BridgeService(cfg))
                {
                    bridge.Start();
                    Console.WriteLine("CereaBridge running.");
                    Console.WriteLine("Profile: " + profilePath);
                    Console.WriteLine("Motor: " + (bridge.IsMotorConnected ? "connected" : "not connected"));
                    Console.WriteLine("Encoder: " + (bridge.IsEncoderConnected ? "connected" : "not connected"));
                    Console.WriteLine("IMU Brick: " + (bridge.IsImuConnected ? "connected" : "not connected"));
                    Console.WriteLine("Use --setup to change saved settings.");
                    Console.WriteLine("Press Enter to exit.");
                    Console.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CereaBridge startup error: " + ex.Message);
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}

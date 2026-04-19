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
            try
            {
                var cfg = BridgeConfig.Load();
                var profilePath = BridgeConfig.GetDefaultProfilePath();
                var forceSetup = args.Any(a => string.Equals(a, "--setup", StringComparison.OrdinalIgnoreCase));
                var firstRun = !File.Exists(profilePath);

                if (forceSetup || firstRun)
                {
                    ConsoleSetup.Run(cfg);
                }

                using (var bridge = new BridgeService(cfg))
                {
                    bridge.Start();
                    Console.WriteLine("CereaBridge running.");
                    Console.WriteLine("Profile: " + profilePath);
                    foreach (var line in cfg.BuildSummaryLines())
                    {
                        Console.WriteLine(line);
                    }

                    var warnings = cfg.BuildWarningLines().ToList();
                    if (warnings.Count > 0)
                    {
                        Console.WriteLine("Warnings:");
                        foreach (var warning in warnings)
                        {
                            Console.WriteLine("- " + warning);
                        }
                    }

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

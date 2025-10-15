using AgOpenGPS.Forms;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Mutex Mutex = new Mutex(true, "{516-0AC5-B9A1-55fd-A8CE-72F04E6BDE8F}");

        public static readonly string Version = Assembly.GetEntryAssembly().GetName().Version.ToString(3); // Major.Minor.Patch
        public static readonly string SemVer = Application.ProductVersion.Split('+').First();
        public static readonly bool IsPreRelease = Application.ProductVersion.Contains('-');
        public static readonly bool IsDevelopVersion = Application.ProductVersion == "1.0.0.0";

        [STAThread]
        private static void Main(string[] args)
        {
            // Check for headless mode
            bool isHeadless = args != null && args.Any(arg =>
                arg.Equals("--headless", StringComparison.OrdinalIgnoreCase) ||
                arg.Equals("-h", StringComparison.OrdinalIgnoreCase));

            if (isHeadless)
            {
                // Run headless mode test
                Console.WriteLine("Starting AgOpenGPS in headless mode...");
                RunHeadlessMode();
                return;
            }

            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                RegistrySettings.Load();
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormGPS());
            }
            else
            {
                FormDialog.Show(
                    "Warning",
                    "AgOpenGPS is Already Running",
                    MessageBoxButtons.OK);
            }
        }

        private static void RunHeadlessMode()
        {
            Console.WriteLine("=== AgOpenGPS Headless Mode Test ===");

            try
            {
                // Load registry settings
                RegistrySettings.Load();
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(RegistrySettings.culture);

                Console.WriteLine("✓ Registry settings loaded");

                // Create FormGPS in headless mode
                FormGPS formGPS = new FormGPS(headless: true);
                Console.WriteLine("✓ FormGPS created successfully in headless mode");

                // Wait for initialization
                Thread.Sleep(500);
                Console.WriteLine("✓ Initialization complete");

                // Check some basic state
                Console.WriteLine("\nFormGPS State:");
                Console.WriteLine($"  - Job started: {formGPS.isJobStarted}");
                Console.WriteLine($"  - Simulator available: {formGPS.sim != null}");
                Console.WriteLine($"  - Machine control available: {formGPS.mc != null}");
                Console.WriteLine($"  - AutoSteer state: {formGPS.isBtnAutoSteerOn}");

                // Enable the simulator
                if (formGPS.sim != null)
                {
                    Console.WriteLine("\nTesting simulator...");
                    formGPS.sim.stepDistance = 0.05; // ~5 km/h
                    formGPS.sim.DoSimTick(0); // Step with 0 steer angle
                    Console.WriteLine("✓ Simulator tick completed");
                    Console.WriteLine($"  - Current lat/lon: {formGPS.sim.CurrentLatLon.Latitude:F6}, {formGPS.sim.CurrentLatLon.Longitude:F6}");

                    // Run a few more steps
                    Console.WriteLine("\nRunning 10 simulation steps...");
                    for (int i = 0; i < 10; i++)
                    {
                        formGPS.sim.DoSimTick(0);
                        Console.Write(".");
                    }
                    Console.WriteLine(" Done!");
                    Console.WriteLine($"  - Final lat/lon: {formGPS.sim.CurrentLatLon.Latitude:F6}, {formGPS.sim.CurrentLatLon.Longitude:F6}");
                }

                Console.WriteLine("\n=== Headless mode test PASSED ===");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n✗ ERROR: {ex.Message}");
                Console.WriteLine($"\nStack trace:\n{ex.StackTrace}");

                if (ex.InnerException != null)
                {
                    Console.WriteLine($"\nInner exception: {ex.InnerException.Message}");
                    Console.WriteLine($"Inner stack trace:\n{ex.InnerException.StackTrace}");
                }

                Console.WriteLine("\n=== Headless mode test FAILED ===");
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
                Environment.Exit(1);
            }
        }
    }
}
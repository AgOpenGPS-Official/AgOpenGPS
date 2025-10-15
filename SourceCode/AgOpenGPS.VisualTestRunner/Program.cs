using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using AgOpenGPS.IntegrationTests.Tests;
using AgOpenGPS.Testing;

namespace AgOpenGPS.VisualTestRunner
{
    /// <summary>
    /// Visual Test Runner with Performance Monitoring
    /// Runs AgOpenGPS integration tests with real-time visualization and detailed performance metrics
    /// </summary>
    class Program
    {
        private static PerformanceMonitor monitor;
        private static TestOrchestrator orchestrator;

        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   AgOpenGPS Visual Test Runner with Performance Monitor  ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            if (args.Length == 0)
            {
                ShowMenu();
            }
            else
            {
                string testName = args[0];
                RunTest(testName);
            }
        }

        static void ShowMenu()
        {
            while (true)
            {
                Console.WriteLine("\nAvailable Tests:");
                Console.WriteLine("  1. Tractor Following Track");
                Console.WriteLine("  2. U-Turn Scenario");
                Console.WriteLine("  3. Run All Tests");
                Console.WriteLine("  4. Exit");
                Console.WriteLine();
                Console.Write("Select test to run (1-4): ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        RunTest("TractorFollowingTrack");
                        break;
                    case "2":
                        RunTest("UTurnScenario");
                        break;
                    case "3":
                        RunTest("TractorFollowingTrack");
                        Console.WriteLine("\nPress any key to continue to next test...");
                        Console.ReadKey();
                        RunTest("UTurnScenario");
                        break;
                    case "4":
                        return;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }

                Console.WriteLine("\nPress any key to return to menu...");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
                Console.WriteLine("║   AgOpenGPS Visual Test Runner with Performance Monitor  ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            }
        }

        static void RunTest(string testName)
        {
            try
            {
                Console.WriteLine($"Starting Test: {testName}");
                Console.WriteLine(new string('─', 60));

                // Initialize performance monitor
                monitor = new PerformanceMonitor(testName);
                monitor.Start();

                // Initialize orchestrator in visual mode
                orchestrator = new TestOrchestrator();
                orchestrator.Initialize(headless: false);
                orchestrator.ShowForm();

                Console.WriteLine("Waiting for UI to initialize (10 seconds)...");
                Thread.Sleep(10000);

                // Run the appropriate test using reflection to access test methods
                var testInstance = new VisualTests();

                // Call Setup method
                var setupMethod = typeof(VisualTests).GetMethod("Setup");
                setupMethod?.Invoke(testInstance, null);

                // Get the private RunUTurnScenarioTest or RunTractorFollowingTrackTest method
                MethodInfo testMethod = null;
                if (testName == "UTurnScenario")
                {
                    testMethod = typeof(VisualTests).GetMethod("RunUTurnScenarioTest",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                }
                else if (testName == "TractorFollowingTrack")
                {
                    testMethod = typeof(VisualTests).GetMethod("RunTractorFollowingTrackTest",
                        BindingFlags.NonPublic | BindingFlags.Instance);
                }

                if (testMethod != null)
                {
                    Console.WriteLine($"\nRunning {testName} test in visual mode...\n");

                    // Start monitoring thread
                    var monitoringThread = new Thread(() => MonitorPerformance());
                    monitoringThread.IsBackground = true;
                    monitoringThread.Start();

                    // Run the test
                    testMethod.Invoke(testInstance, new object[] { true });

                    // Stop monitoring
                    monitor.Stop();
                }
                else
                {
                    Console.WriteLine($"Error: Test method not found for {testName}");
                }

                // Call Teardown
                var teardownMethod = typeof(VisualTests).GetMethod("Teardown");
                teardownMethod?.Invoke(testInstance, null);

                // Display results
                monitor.PrintSummary();
                ExportResults(testName);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError running test: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }
            finally
            {
                orchestrator?.Shutdown();
            }
        }

        static void MonitorPerformance()
        {
            Console.WriteLine("\n--- Real-Time Performance Monitoring ---");
            Console.WriteLine("Time | CPU% | Memory(MB) | Frames");
            Console.WriteLine(new string('-', 45));

            int sampleCount = 0;
            while (!monitor.IsStopped)
            {
                Thread.Sleep(2000); // Update every 2 seconds
                sampleCount++;

                var snapshot = monitor.GetSnapshot();
                Console.WriteLine($"{sampleCount * 2,4}s | {snapshot.CpuPercent,4:F1}% | {snapshot.MemoryMB,10:F1} | {snapshot.FrameCount,6}");
            }
        }

        static void ExportResults(string testName)
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string outputDir = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "..", "..", "..", "TestOutput", "PerformanceReports");

                Directory.CreateDirectory(outputDir);

                string reportPath = Path.Combine(outputDir, $"{testName}_{timestamp}.txt");
                monitor.ExportToFile(reportPath);

                Console.WriteLine($"\nPerformance report saved to: {reportPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nWarning: Could not export results: {ex.Message}");
            }
        }
    }

    /// <summary>
    /// Enhanced performance monitoring with real-time tracking
    /// </summary>
    public class PerformanceMonitor
    {
        private readonly string testName;
        private readonly Stopwatch stopwatch;
        private readonly Process currentProcess;
        private long peakWorkingSet;
        private long peakPrivateMemory;
        private double cpuTimeStart;
        private int frameCount;
        private bool stopped;

        public bool IsStopped => stopped;

        public PerformanceMonitor(string testName)
        {
            this.testName = testName;
            this.stopwatch = new Stopwatch();
            this.currentProcess = Process.GetCurrentProcess();
            this.frameCount = 0;
            this.stopped = false;
        }

        public void Start()
        {
            stopwatch.Start();
            cpuTimeStart = currentProcess.TotalProcessorTime.TotalMilliseconds;
            peakWorkingSet = currentProcess.WorkingSet64;
            peakPrivateMemory = currentProcess.PrivateMemorySize64;
        }

        public void RecordFrame()
        {
            frameCount++;
            UpdatePeakMemory();
        }

        private void UpdatePeakMemory()
        {
            long currentWorkingSet = currentProcess.WorkingSet64;
            long currentPrivateMemory = currentProcess.PrivateMemorySize64;

            if (currentWorkingSet > peakWorkingSet)
                peakWorkingSet = currentWorkingSet;

            if (currentPrivateMemory > peakPrivateMemory)
                peakPrivateMemory = currentPrivateMemory;
        }

        public void Stop()
        {
            stopped = true;
            stopwatch.Stop();
        }

        public PerformanceSnapshot GetSnapshot()
        {
            UpdatePeakMemory();

            double elapsedMs = stopwatch.Elapsed.TotalMilliseconds;
            double cpuTimeEnd = currentProcess.TotalProcessorTime.TotalMilliseconds;
            double cpuTimeUsed = cpuTimeEnd - cpuTimeStart;
            double cpuPercent = elapsedMs > 0 ? (cpuTimeUsed / elapsedMs) * 100.0 : 0;

            return new PerformanceSnapshot
            {
                CpuPercent = cpuPercent,
                MemoryMB = currentProcess.WorkingSet64 / (1024.0 * 1024.0),
                FrameCount = frameCount
            };
        }

        public void PrintSummary()
        {
            double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;
            double cpuTimeEnd = currentProcess.TotalProcessorTime.TotalMilliseconds;
            double cpuTimeUsed = cpuTimeEnd - cpuTimeStart;
            double cpuPercent = (cpuTimeUsed / stopwatch.Elapsed.TotalMilliseconds) * 100.0;

            Console.WriteLine("\n╔═══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                  Performance Summary                      ║");
            Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
            Console.WriteLine($"Test Name:          {testName}");
            Console.WriteLine($"Duration:           {elapsedSeconds:F2}s");

            if (frameCount > 0)
            {
                double fps = frameCount / elapsedSeconds;
                Console.WriteLine($"Total Frames:       {frameCount}");
                Console.WriteLine($"Average FPS:        {fps:F1}");
            }

            Console.WriteLine($"Average CPU Usage:  {cpuPercent:F1}%");
            Console.WriteLine($"Peak Working Set:   {peakWorkingSet / (1024.0 * 1024.0):F1} MB");
            Console.WriteLine($"Peak Private Mem:   {peakPrivateMemory / (1024.0 * 1024.0):F1} MB");
            Console.WriteLine($"Final Working Set:  {currentProcess.WorkingSet64 / (1024.0 * 1024.0):F1} MB");
        }

        public void ExportToFile(string filePath)
        {
            using (var writer = new StreamWriter(filePath))
            {
                writer.WriteLine("AgOpenGPS Visual Test Performance Report");
                writer.WriteLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                writer.WriteLine(new string('=', 60));
                writer.WriteLine();
                writer.WriteLine($"Test Name:          {testName}");
                writer.WriteLine($"Duration:           {stopwatch.Elapsed.TotalSeconds:F2}s");

                if (frameCount > 0)
                {
                    double fps = frameCount / stopwatch.Elapsed.TotalSeconds;
                    writer.WriteLine($"Total Frames:       {frameCount}");
                    writer.WriteLine($"Average FPS:        {fps:F1}");
                }

                double cpuTimeEnd = currentProcess.TotalProcessorTime.TotalMilliseconds;
                double cpuTimeUsed = cpuTimeEnd - cpuTimeStart;
                double cpuPercent = (cpuTimeUsed / stopwatch.Elapsed.TotalMilliseconds) * 100.0;

                writer.WriteLine($"Average CPU Usage:  {cpuPercent:F1}%");
                writer.WriteLine($"Peak Working Set:   {peakWorkingSet / (1024.0 * 1024.0):F1} MB");
                writer.WriteLine($"Peak Private Mem:   {peakPrivateMemory / (1024.0 * 1024.0):F1} MB");
                writer.WriteLine($"Final Working Set:  {currentProcess.WorkingSet64 / (1024.0 * 1024.0):F1} MB");
            }
        }
    }

    public class PerformanceSnapshot
    {
        public double CpuPercent { get; set; }
        public double MemoryMB { get; set; }
        public int FrameCount { get; set; }
    }
}

using NUnit.Framework;
using System;
using System.Threading;

namespace AgOpenGPS.IntegrationTests.Tests
{
    [TestFixture]
    public class SpeedVerificationTest : SimulatedTestBase
    {
        /// <summary>
        /// Headless test: Drives tractor straight at 10 km/h and verifies actual speed matches reported speed
        /// </summary>
        [Test]
        public void Test_SpeedVerification_Headless()
        {
            RunSpeedVerificationTest(visualMode: false, timeStep: 0.1);
        }

        /// <summary>
        /// Visual test: Same as headless but with visualization
        /// </summary>
        [Test]
        [Explicit("Visual test - run manually")]
        [Apartment(ApartmentState.STA)]
        public void Visual_Test_SpeedVerification()
        {
            RunVisualTest("Speed Verification", () => RunSpeedVerificationTest(visualMode: true, timeStep: 0.1));
        }

        /// <summary>
        /// Core test: Drives tractor straight for 30 seconds and verifies speed accuracy
        /// </summary>
        private void RunSpeedVerificationTest(bool visualMode, double timeStep)
        {
            string mode = visualMode ? "VISUAL" : "HEADLESS";
            Console.WriteLine($"=== Speed Verification Test ({mode}, timeStep={timeStep}s) ===\n");

            // Setup simple field
            double fieldWidth = 100;
            double fieldLength = 200;
            double abLineX = 0.0;
            double startEasting = 0.0;
            double startNorthing = -fieldLength / 2.0 + 10.0;

            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "SpeedVerificationTest",
                fieldSize: (fieldWidth, fieldLength),
                trackX: abLineX,
                tractorStart: (startEasting, startNorthing)
            );

            // Override speed to 10 km/h for this test
            simController.SetSpeed(10.0);
            Console.WriteLine($"Speed set to: 10.0 km/h (2.778 m/s)");

            // Initialize path logger
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();

            // Run simulation for 30 seconds
            Console.WriteLine($"\n=== Running Simulation ({timeStep}s timestep) ===");
            double simulationTime = 30.0;
            double elapsedTime = 0;
            int frameCount = 0;

            double initialNorthing = 0;
            double previousNorthing = 0;
            bool firstUpdate = true;

            while (elapsedTime < simulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;
                frameCount++;

                var state = simController.GetState();

                if (firstUpdate)
                {
                    initialNorthing = state.Northing;
                    previousNorthing = state.Northing;
                    firstUpdate = false;
                }

                // Print progress every 5 seconds
                if (frameCount % (int)(5.0 / timeStep) == 0)
                {
                    double distanceTraveled = state.Northing - initialNorthing;
                    double actualSpeed = distanceTraveled / elapsedTime;
                    double expected = 2.778; // 10 km/h = 2.778 m/s
                    double error = actualSpeed - expected;
                    double errorPct = (error / expected) * 100.0;

                    Console.WriteLine($"  [{elapsedTime:F1}s] N={state.Northing:F2}m, " +
                                     $"Distance={distanceTraveled:F2}m, " +
                                     $"Actual={actualSpeed:F3}m/s, " +
                                     $"Expected={expected:F3}m/s, " +
                                     $"Error={errorPct:+0.00}%");
                }

                previousNorthing = state.Northing;
            }

            // Final verification
            var finalState = simController.GetState();
            double totalDistance = finalState.Northing - initialNorthing;
            double averageSpeed = totalDistance / simulationTime;
            double expectedSpeed = 10.0 / 3.6; // 10 km/h = 2.778 m/s
            double speedError = Math.Abs(averageSpeed - expectedSpeed);
            double speedErrorPct = (speedError / expectedSpeed) * 100.0;

            Console.WriteLine($"\n=== Final Results ===");
            Console.WriteLine($"Total time: {simulationTime:F1}s");
            Console.WriteLine($"Total distance: {totalDistance:F2}m");
            Console.WriteLine($"Average speed: {averageSpeed:F3}m/s ({averageSpeed * 3.6:F2} km/h)");
            Console.WriteLine($"Expected speed: {expectedSpeed:F3}m/s ({expectedSpeed * 3.6:F2} km/h)");
            Console.WriteLine($"Speed error: {speedError:F3}m/s ({speedErrorPct:F2}%)");
            Console.WriteLine($"Total frames: {frameCount}");

            // Export path data
            string testOutputDir = System.IO.Path.Combine(
                System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location),
                "..", "..", "..", "TestOutput");
            System.IO.Directory.CreateDirectory(testOutputDir);
            string outputFile = System.IO.Path.Combine(testOutputDir,
                $"SpeedVerification_{mode}_{timeStep.ToString().Replace(".", "_")}s_{DateTime.Now:yyyyMMdd_HHmmss}.json");
            pathLogger.ExportToJson(outputFile);
            Console.WriteLine($"\nPath data exported to: {outputFile}");

            // Assertions
            Assert.That(speedErrorPct, Is.LessThan(1.0),
                $"Speed error ({speedErrorPct:F2}%) should be less than 1%");
            Assert.That(totalDistance, Is.GreaterThan(80.0),
                "Tractor should travel at least 80m in 30s at 10 km/h");
            Assert.That(totalDistance, Is.LessThan(85.0),
                "Tractor should travel at most 85m in 30s at 10 km/h (expected ~83.3m)");

            Console.WriteLine("\nTest PASSED: Speed matches expected value within tolerance");
        }
    }
}

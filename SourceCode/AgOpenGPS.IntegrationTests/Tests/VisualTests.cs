using System;
using System.Threading;
using NUnit.Framework;
using FluentAssertions;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    /// <summary>
    /// Visual tests that display the OpenGL map in real-time
    /// These tests are marked as [Explicit] so they don't run in automated CI
    /// Run manually to visualize test scenarios
    /// </summary>
    [TestFixture]
    public class VisualTests
    {
        private AgOpenGPS.Testing.TestOrchestrator orchestrator;

        [SetUp]
        public void Setup()
        {
            // Note: Initialize with headless: false to enable visualization
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
        }

        [TearDown]
        public void Teardown()
        {
            orchestrator?.Shutdown();
        }

        /// <summary>
        /// Visual test - Shows field creation and tractor movement in real-time
        /// Run this test manually to see the OpenGL visualization
        /// </summary>
        [Test]
        [Explicit("Run manually to visualize - opens UI window")]
        public void Visual_Test_TractorFollowingTrack()
        {
            Console.WriteLine("\n=== Visual Test: Tractor Following Track ===");
            Console.WriteLine("This test will open the AgOpenGPS window and show the tractor following a track.");
            Console.WriteLine("Watch the OpenGL display for real-time visualization.\n");

            // Initialize in VISUAL mode (not headless)
            orchestrator.Initialize(headless: false);
            orchestrator.ShowForm();

            // Give time for window to appear
            Thread.Sleep(500);

            Console.WriteLine("Step 1: Creating field...");
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("VisualTestField", 39.0, -94.0);
            Thread.Sleep(500);

            Console.WriteLine("Step 2: Adding boundary (100m x 200m)...");
            var boundary = CreateRectangularBoundary(39.0, -94.0, 100, 200);
            fieldController.AddBoundary(boundary, isOuter: true);
            Thread.Sleep(500);

            Console.WriteLine("Step 3: Creating track line...");
            var trackPointA = new TestPoint(-10, -80, 0);
            var trackPointB = new TestPoint(-10, 80, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Thread.Sleep(500);

            Console.WriteLine("Step 4: Positioning tractor...");
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);
            simController.SetHeading(0.0);
            simController.SetSpeed(8.0);
            Thread.Sleep(500);

            Console.WriteLine("Step 5: Enabling autosteer...");
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            Thread.Sleep(500);

            Console.WriteLine("\nStep 6: Running simulation - Watch the AgOpenGPS window!");
            Console.WriteLine("The tractor will drive north for 20 seconds...\n");

            // Run simulation for 20 seconds with slower timestep for better visualization
            double simulationTime = 20.0;
            double elapsedTime = 0;
            double timeStep = 0.05; // 50ms steps for smooth visualization
            int frameCount = 0;

            while (elapsedTime < simulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;
                frameCount++;

                // Print progress every 2 seconds
                if (frameCount % 40 == 0)
                {
                    var simState = simController.GetState();
                    var autosteerState = autosteerController.GetState();
                    Console.WriteLine($"  Time: {elapsedTime:F1}s | " +
                        $"Position: N={simState.Northing:F2}m | " +
                        $"XTE: {autosteerState.CrossTrackError:F3}m | " +
                        $"Speed: {simState.SpeedKph:F1} km/h");
                }

                // Small delay to make visualization smoother (approximately 20 Hz)
                Thread.Sleep(50);
            }

            Console.WriteLine("\n=== Visual Test Complete ===");
            Console.WriteLine("The AgOpenGPS window will remain open for 5 seconds.");
            Console.WriteLine("You can close the window early if desired.");

            // Keep window open for 5 more seconds for inspection
            var form = orchestrator.GetFormGPS();
            int remainingSeconds = 5;
            while (form != null && !form.IsDisposed && remainingSeconds > 0)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(1000);
                remainingSeconds--;
            }
        }

        /// <summary>
        /// Visual test - Shows U-turn scenario with real-time visualization
        /// Run this test manually to debug U-turn triggering
        /// </summary>
        [Test]
        [Explicit("Run manually to visualize - opens UI window")]
        public void Visual_Test_UTurnScenario()
        {
            Console.WriteLine("\n=== Visual Test: U-Turn Scenario ===");
            Console.WriteLine("This test will show the tractor approaching a boundary with U-turn enabled.");
            Console.WriteLine("Watch for U-turn trigger and execution.\n");

            // Initialize in VISUAL mode
            orchestrator.Initialize(headless: false);
            orchestrator.ShowForm();
            Thread.Sleep(500);

            Console.WriteLine("Step 1: Creating field...");
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("UTurnVisualTest", 39.0, -94.0);
            Thread.Sleep(500);

            Console.WriteLine("Step 2: Adding smaller boundary (50m x 100m)...");
            var boundary = CreateRectangularBoundary(39.0, -94.0, 50, 100);
            fieldController.AddBoundary(boundary, isOuter: true);
            Thread.Sleep(500);

            Console.WriteLine("Step 3: Creating track...");
            var trackPointA = new TestPoint(-10, -50, 0);
            var trackPointB = new TestPoint(-10, 50, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Thread.Sleep(500);

            Console.WriteLine("Step 4: Positioning tractor at south end...");
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);
            simController.SetHeading(0.0);
            simController.SetSpeed(10.0); // Faster to reach boundary quicker
            Thread.Sleep(500);

            Console.WriteLine("Step 5: Enabling autosteer...");
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            Thread.Sleep(500);

            Console.WriteLine("Step 6: Enabling U-turn (5m from boundary)...");
            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0);
            Thread.Sleep(500);

            Console.WriteLine("\nStep 7: Running simulation - Watch for U-turn trigger!");
            Console.WriteLine("The tractor will drive north until U-turn triggers or 60 seconds elapses...\n");

            double maxSimulationTime = 60.0;
            double elapsedTime = 0;
            double timeStep = 0.05;
            int frameCount = 0;
            bool uturnTriggered = false;

            while (elapsedTime < maxSimulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;
                frameCount++;

                var uturnState = uturnController.GetState();

                // Check for U-turn trigger
                if (!uturnTriggered && uturnState.IsTriggered)
                {
                    uturnTriggered = true;
                    Console.WriteLine($"\n*** U-TURN TRIGGERED at {elapsedTime:F1}s! ***\n");
                }

                // Print progress every 2 seconds
                if (frameCount % 40 == 0)
                {
                    var simState = simController.GetState();
                    var autosteerState = autosteerController.GetState();
                    Console.WriteLine($"  Time: {elapsedTime:F1}s | " +
                        $"N={simState.Northing:F2}m | " +
                        $"XTE: {autosteerState.CrossTrackError:F3}m | " +
                        $"UTurn: {(uturnState.IsTriggered ? "ACTIVE" : "waiting")}");
                }

                // Exit if U-turn completes
                if (uturnTriggered && !uturnState.IsTriggered)
                {
                    Console.WriteLine($"\n*** U-TURN COMPLETED at {elapsedTime:F1}s! ***\n");
                    break;
                }

                Thread.Sleep(50);
            }

            if (!uturnTriggered)
            {
                Console.WriteLine("\n*** WARNING: U-turn never triggered - this is the known issue! ***");
                Console.WriteLine("You can now inspect the FormGPS window to see why.");
            }

            Console.WriteLine("\n=== Visual Test Complete ===");
            Console.WriteLine("The AgOpenGPS window will remain open.");
            Console.WriteLine("Close the window or press Enter to end the test...");

            var form = orchestrator.GetFormGPS();
            while (form != null && !form.IsDisposed && Console.KeyAvailable == false)
            {
                System.Windows.Forms.Application.DoEvents();
                Thread.Sleep(100);
            }

            if (Console.KeyAvailable)
            {
                Console.ReadLine();
            }
        }

        private System.Collections.Generic.List<TestPoint> CreateRectangularBoundary(
            double centerLat, double centerLon,
            double widthMeters, double lengthMeters)
        {
            var boundary = new System.Collections.Generic.List<TestPoint>();

            double halfWidth = widthMeters / 2.0;
            double halfLength = lengthMeters / 2.0;

            boundary.Add(new TestPoint(-halfWidth, -halfLength, 0));
            boundary.Add(new TestPoint(halfWidth, -halfLength, 0));
            boundary.Add(new TestPoint(halfWidth, halfLength, 0));
            boundary.Add(new TestPoint(-halfWidth, halfLength, 0));
            boundary.Add(new TestPoint(-halfWidth, -halfLength, 0));

            return boundary;
        }
    }
}

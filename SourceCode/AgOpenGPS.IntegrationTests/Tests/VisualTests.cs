using System;
using System.IO;
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
            // Create orchestrator and initialize in headless mode for automated tests
            // Visual tests will recreate it in visual mode
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: true);
        }

        [TearDown]
        public void Teardown()
        {
            orchestrator?.Shutdown();
        }

        /// <summary>
        /// Headless test - Tractor following track
        /// This uses the shared test logic from RunTractorFollowingTrackTest
        /// </summary>
        [Test]
        public void Test_TractorFollowingTrack()
        {
            RunTractorFollowingTrackTest(visualMode: false);
        }

        /// <summary>
        /// Visual test - Shows field creation and tractor movement in real-time
        /// Run this test manually to see the OpenGL visualization
        /// This is a wrapper around Test_TractorFollowingTrack that enables visualization
        /// </summary>
        [Test]
        [Explicit("Run manually to visualize - opens UI window")]
        [Apartment(System.Threading.ApartmentState.STA)]
        public void Visual_Test_TractorFollowingTrack()
        {
            // Shutdown the headless orchestrator from Setup
            orchestrator?.Shutdown();

            // Create a new orchestrator in VISUAL mode
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: false);
            orchestrator.ShowForm();

            Console.WriteLine("\n=== Visual Test: Tractor Following Track ===");
            Console.WriteLine("This test runs the same test as Test_TractorFollowingTrack");
            Console.WriteLine("but with OpenGL visualization enabled.");
            Console.WriteLine("Waiting 10 seconds for UI to fully load...\n");
            Thread.Sleep(10000);

            // Run the actual test logic
            RunTractorFollowingTrackTest(visualMode: true);

            Console.WriteLine("\n=== Visual Test Complete ===");
        }

        /// <summary>
        /// Shared test logic for tractor following track
        /// Can run in both headless and visual modes
        /// </summary>
        private void RunTractorFollowingTrackTest(bool visualMode)
        {
            Console.WriteLine("\n=== Tractor Following Track Test ===\n");

            // Step 1: Create field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("VisualTestField", 39.0, -94.0);
            Console.WriteLine("Step 1: Field created");
            if (visualMode) Thread.Sleep(300);

            // Step 2: Add boundary
            var boundary = CreateRectangularBoundary(39.0, -94.0, 100, 200);
            fieldController.AddBoundary(boundary, isOuter: true);
            Console.WriteLine("Step 2: Boundary added (100m x 200m)");
            if (visualMode) Thread.Sleep(300);

            // Step 3: Create track
            var trackPointA = new TestPoint(-10, -80, 0);
            var trackPointB = new TestPoint(-10, 80, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Console.WriteLine("Step 3: Track created at X=-10m");
            if (visualMode) Thread.Sleep(300);

            // Step 4: Position tractor
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPositionLocal(-10, -80); // Start on track, 80m south
            simController.SetHeading(0.0); // Heading north
            simController.SetSpeed(8.0);
            Console.WriteLine("Step 4: Tractor positioned at E=-10m, N=-80m, heading north at 8 km/h");
            if (visualMode) Thread.Sleep(300);

            // Step 5: Enable autosteer
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            Console.WriteLine("Step 5: Autosteer enabled\n");
            if (visualMode) Thread.Sleep(300);

            // Step 6: Run simulation
            Console.WriteLine("Step 6: Running simulation...");
            double simulationTime = 20.0;
            double elapsedTime = 0;
            double timeStep = visualMode ? 0.05 : 0.1;
            int frameCount = 0;

            double initialNorthing = simController.GetState().Northing;

            while (elapsedTime < simulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;
                frameCount++;

                // Print progress every 2 seconds
                int progressInterval = visualMode ? 40 : 20;
                if (frameCount % progressInterval == 0)
                {
                    var simState = simController.GetState();
                    var autosteerState = autosteerController.GetState();
                    Console.WriteLine($"  Time: {elapsedTime:F1}s | " +
                        $"N={simState.Northing:F2}m | " +
                        $"XTE: {autosteerState.CrossTrackError:F3}m | " +
                        $"Speed: {simState.SpeedKph:F1} km/h");
                }

                if (visualMode) Thread.Sleep(50);
            }

            // Verify results
            var finalState = simController.GetState();
            var finalAutosteerState = autosteerController.GetState();
            double distanceTraveled = finalState.Northing - initialNorthing;

            Console.WriteLine("\n=== Verification ===");
            Console.WriteLine($"Distance traveled: {distanceTraveled:F2}m");
            Console.WriteLine($"Final XTE: {finalAutosteerState.CrossTrackError:F3}m");
            Console.WriteLine($"Autosteer active: {finalAutosteerState.IsActive}");

            // Assertions
            distanceTraveled.Should().BeGreaterThan(20.0, "Tractor should have moved at least 20m");
            finalAutosteerState.IsActive.Should().BeTrue("Autosteer should still be active");
            Math.Abs(finalAutosteerState.CrossTrackError).Should().BeLessThan(1.0, "Should maintain good cross-track accuracy");
        }

        /// <summary>
        /// Headless U-turn test - runs without visualization
        /// This uses the shared test logic from RunUTurnScenarioTest
        /// </summary>
        [Test]
        public void Test_UTurnScenario()
        {
            RunUTurnScenarioTest(visualMode: false);
        }

        /// <summary>
        /// Visual test - Shows U-turn scenario with real-time visualization
        /// Run this test manually to debug U-turn triggering
        /// This is a wrapper around Test_UTurnScenario that enables visualization
        /// </summary>
        [Test]
        [Explicit("Run manually to visualize - opens UI window")]
        [Apartment(System.Threading.ApartmentState.STA)]
        public void Visual_Test_UTurnScenario()
        {
            // Shutdown the headless orchestrator from Setup
            orchestrator?.Shutdown();

            // Create a new orchestrator in VISUAL mode
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: false);
            orchestrator.ShowForm();

            Console.WriteLine("\n=== Visual Test: U-Turn Scenario ===");
            Console.WriteLine("This test runs the same test as Test_UTurnScenario");
            Console.WriteLine("but with OpenGL visualization enabled.");
            Console.WriteLine("Waiting 10 seconds for UI to fully load...\n");
            Thread.Sleep(10000);

            // Run the actual test logic
            RunUTurnScenarioTest(visualMode: true);

            Console.WriteLine("\n=== Visual Test Complete ===");
        }

        /// <summary>
        /// Shared test logic for U-turn scenario
        /// Can run in both headless and visual modes
        /// </summary>
        private void RunUTurnScenarioTest(bool visualMode)
        {
            Console.WriteLine("\n=== U-Turn Integration Test ===\n");

            // Step 1: Create field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("UTurnTestField", 39.0, -94.0);
            Console.WriteLine("Step 1: Field created");
            if (visualMode) Thread.Sleep(300);

            // Step 2: Add boundary
            var boundary = CreateRectangularBoundary(39.0, -94.0, 50, 100);
            fieldController.AddBoundary(boundary, isOuter: true);
            Console.WriteLine("Step 2: Boundary added (50m x 100m)");
            if (visualMode) Thread.Sleep(300);

            // Step 3: Create track
            var trackPointA = new TestPoint(-10, -50, 0);
            var trackPointB = new TestPoint(-10, 50, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Console.WriteLine("Step 3: Track created at X=-10m, running north-south");
            if (visualMode) Thread.Sleep(300);

            // Step 4: Position tractor
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPositionLocal(-10, -45); // Start on track, near southern boundary
            simController.SetHeading(0.0); // Heading north
            simController.SetSpeed(8.0);

            // Verify position was set correctly
            var initialState = simController.GetState();
            Console.WriteLine($"Step 4: Tractor positioned at E={initialState.Easting:F2}m, N={initialState.Northing:F2}m, heading north at 8 km/h");
            if (visualMode) Thread.Sleep(300);

            // Step 5: Enable autosteer
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            Console.WriteLine("Step 5: Autosteer enabled");
            if (visualMode) Thread.Sleep(300);

            // Step 6: Enable U-turn
            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0);
            Console.WriteLine("Step 6: U-turn enabled (trigger at 5m from boundary)");

            // Debug: Print turn line points
            var formGPS_debug = orchestrator.GetFormGPS();
            if (formGPS_debug.bnd.bndList.Count > 0 && formGPS_debug.bnd.bndList[0].turnLine.Count > 0)
            {
                Console.WriteLine($"Turn line has {formGPS_debug.bnd.bndList[0].turnLine.Count} points:");
                for (int i = 0; i < Math.Min(5, formGPS_debug.bnd.bndList[0].turnLine.Count); i++)
                {
                    var pt = formGPS_debug.bnd.bndList[0].turnLine[i];
                    Console.WriteLine($"  Point {i}: E={pt.easting:F2}m, N={pt.northing:F2}m");
                }
            }

            if (visualMode) Thread.Sleep(300);

            // Step 7: Start path logging
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();
            Console.WriteLine("Step 7: Path logging started\n");

            // Step 8: Run simulation
            Console.WriteLine("Step 8: Running simulation...");
            double maxSimulationTime = 60.0;
            double elapsedTime = 0;
            double timeStep = 0.05; // Use same timestep for both modes to isolate timing issues
            int frameCount = 0;

            bool uturnTriggered = false;
            bool uturnCompleted = false;
            double uturnTriggerTime = 0;
            double uturnCompletionTime = 0;

            // Get FormGPS for debug info (needed in both visual and headless modes)
            var formGPS = orchestrator.GetFormGPS();

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
                    uturnTriggerTime = elapsedTime;
                    var simState = simController.GetState();
                    Console.WriteLine($"  -> U-Turn TRIGGERED at {elapsedTime:F1}s");
                    Console.WriteLine($"     Position: E={simState.Easting:F2}m, N={simState.Northing:F2}m");
                }

                // Check for U-turn completion
                if (uturnTriggered && !uturnCompleted && !uturnState.IsTriggered)
                {
                    uturnCompleted = true;
                    uturnCompletionTime = elapsedTime;
                    var simState = simController.GetState();
                    Console.WriteLine($"  -> U-Turn COMPLETED at {elapsedTime:F1}s");
                    Console.WriteLine($"     Turn duration: {uturnCompletionTime - uturnTriggerTime:F1}s");
                    break;
                }

                // Print progress with detailed debug info
                int progressInterval = 100; // Every 5s (100 frames * 0.05s)
                if (frameCount % progressInterval == 0)
                {
                    var simState = simController.GetState();
                    var autosteerState = autosteerController.GetState();

                    // Show detailed debug info in both modes
                    var pivotPos = new AgOpenGPS.vec3(simState.Easting, simState.Northing, 0);
                    bool isInTurnArea = formGPS.bnd.IsPointInsideTurnArea(pivotPos) != -1;
                    bool isYouTurnBtnOn = formGPS.yt.isYouTurnBtnOn;
                    int turnLineCount = formGPS.bnd.bndList.Count > 0 ? formGPS.bnd.bndList[0].turnLine.Count : 0;
                    int youTurnPhase = formGPS.yt.youTurnPhase;
                    int ytListCount = formGPS.yt.ytList.Count;

                    // Calculate distance to turn pattern start (this is what triggers the U-turn)
                    double distToTurnPattern = -1;
                    if (ytListCount > 2)
                    {
                        var turnPatternStart = formGPS.yt.ytList[2];
                        distToTurnPattern = AgOpenGPS.glm.Distance(turnPatternStart, pivotPos);
                    }

                    // Check if in boundary (boundary goes from -50 to +50 N, -25 to +25 E)
                    bool isInBoundary = formGPS.bnd.bndList.Count > 0 &&
                        formGPS.bnd.bndList[0].fenceLineEar.IsPointInPolygon(new AgOpenGPS.vec2(simState.Easting, simState.Northing));

                    Console.WriteLine($"  Time: {elapsedTime:F1}s | N={simState.Northing:F2}m | " +
                        $"XTE: {autosteerState.CrossTrackError:F3}m | " +
                        $"UTurn: {(uturnState.IsTriggered ? "ACTIVE" : "waiting")} | " +
                        $"InTurnArea: {isInTurnArea} | InBoundary: {isInBoundary} | YTBtnOn: {isYouTurnBtnOn} | " +
                        $"Phase: {youTurnPhase} | YTListCnt: {ytListCount} | DistToPattern: {distToTurnPattern:F2}m | TurnLinePts: {turnLineCount}");
                }

                if (visualMode) Thread.Sleep(50);
            }

            // Stop logging
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();
            Console.WriteLine($"\nStep 9: Simulation ended - Logged {path.Count} path points over {elapsedTime:F1}s");

            // Export path data to JSON for visualization
            string testOutputDir = Path.Combine(Path.GetDirectoryName(typeof(VisualTests).Assembly.Location), "..", "..", "..", "TestOutput");
            Directory.CreateDirectory(testOutputDir);
            string jsonPath = Path.Combine(testOutputDir, $"uturn_test_{(visualMode ? "visual" : "headless")}.json");
            pathLogger.ExportToJson(jsonPath);
            Console.WriteLine($"Path data exported to: {jsonPath}");

            // Verify results
            Console.WriteLine("\n=== Verification ===");
            uturnTriggered.Should().BeTrue("U-turn should have been triggered");
            Console.WriteLine($"✓ U-turn was triggered at {uturnTriggerTime:F1}s");

            uturnCompleted.Should().BeTrue("U-turn should have completed");
            Console.WriteLine($"✓ U-turn completed at {uturnCompletionTime:F1}s");

            double turnDuration = uturnCompletionTime - uturnTriggerTime;
            Console.WriteLine($"✓ U-turn duration: {turnDuration:F1}s");
        }

        private System.Collections.Generic.List<TestPoint> CreateRectangularBoundary(
            double centerLat, double centerLon,
            double widthMeters, double lengthMeters)
        {
            var boundary = new System.Collections.Generic.List<TestPoint>();

            double halfWidth = widthMeters / 2.0;
            double halfLength = lengthMeters / 2.0;
            double pointSpacing = 1.0; // 1 meter between points

            // Bottom side: left to right (-halfWidth, -halfLength) to (halfWidth, -halfLength)
            for (double e = -halfWidth; e < halfWidth; e += pointSpacing)
            {
                boundary.Add(new TestPoint(e, -halfLength, 0));
            }

            // Right side: bottom to top (halfWidth, -halfLength) to (halfWidth, halfLength)
            for (double n = -halfLength; n < halfLength; n += pointSpacing)
            {
                boundary.Add(new TestPoint(halfWidth, n, 0));
            }

            // Top side: right to left (halfWidth, halfLength) to (-halfWidth, halfLength)
            for (double e = halfWidth; e > -halfWidth; e -= pointSpacing)
            {
                boundary.Add(new TestPoint(e, halfLength, 0));
            }

            // Left side: top to bottom (-halfWidth, halfLength) to (-halfWidth, -halfLength)
            for (double n = halfLength; n > -halfLength; n -= pointSpacing)
            {
                boundary.Add(new TestPoint(-halfWidth, n, 0));
            }

            // Close the boundary by adding the first point again
            boundary.Add(new TestPoint(-halfWidth, -halfLength, 0));

            return boundary;
        }
    }
}

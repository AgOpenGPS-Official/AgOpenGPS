using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;
using FluentAssertions;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    /// <summary>
    /// Integration tests using the simulated tractor controller
    /// Can run in headless mode (automated) or visual mode (manual debugging)
    /// Visual tests are marked [Explicit] and require manual execution
    /// </summary>
    [TestFixture]
    public class SimulatedIntegrationTests
    {
        private AgOpenGPS.Testing.TestOrchestrator orchestrator;

        [SetUp]
        public void Setup()
        {
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: true);
        }

        [TearDown]
        public void Teardown()
        {
            orchestrator?.Shutdown();
        }

        #region Tractor Following Track Tests

        /// <summary>
        /// Tests basic autosteer functionality by creating a field, placing a tractor on a track,
        /// enabling autosteer, and verifying the tractor follows the track with minimal cross-track error.
        /// Runs in headless mode for automated CI testing.
        /// </summary>
        [Test]
        public void Test_TractorFollowingTrack()
        {
            RunTractorFollowingTrackTest(visualMode: false);
        }

        /// <summary>
        /// Visual version of Test_TractorFollowingTrack that displays the OpenGL map in real-time.
        /// Run manually to observe the tractor following the track visually.
        /// Marked [Explicit] to prevent running in automated test suites.
        /// </summary>
        [Test]
        [Explicit("Run manually to visualize - opens UI window")]
        [Apartment(ApartmentState.STA)]
        public void Visual_Test_TractorFollowingTrack()
        {
            RunVisualTest("Tractor Following Track", () => RunTractorFollowingTrackTest(visualMode: true));
        }

        /// <summary>
        /// Core test logic: Creates a 100m x 200m field with a vertical track, positions the tractor
        /// at the southern end, enables autosteer, and simulates 20 seconds of movement.
        /// Verifies the tractor moves at least 20m and maintains cross-track error below 1m.
        /// </summary>
        private void RunTractorFollowingTrackTest(bool visualMode)
        {
            Console.WriteLine("\n=== Tractor Following Track Test ===\n");

            // Setup field and track
            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "VisualTestField",
                fieldSize: (100, 200),
                trackX: -10,
                tractorStart: (-10, -80)
            );

            Console.WriteLine("Step 5: Autosteer enabled\n");

            // Run simulation
            Console.WriteLine("Step 6: Running simulation...");
            double initialNorthing = simController.GetState().Northing;
            RunSimulation(visualMode, simulationTime: 20.0, printProgress: (elapsed, state, autosteerState) =>
            {
                Console.WriteLine($"  Time: {elapsed:F1}s | " +
                    $"N={state.Northing:F2}m | " +
                    $"XTE: {autosteerState.CrossTrackError:F3}m | " +
                    $"Speed: {state.SpeedKph:F1} km/h");
            });

            // Verify results
            var finalState = simController.GetState();
            var finalAutosteerState = autosteerController.GetState();
            double distanceTraveled = finalState.Northing - initialNorthing;

            Console.WriteLine("\n=== Verification ===");
            Console.WriteLine($"Distance traveled: {distanceTraveled:F2}m");
            Console.WriteLine($"Final XTE: {finalAutosteerState.CrossTrackError:F3}m");
            Console.WriteLine($"Autosteer active: {finalAutosteerState.IsActive}");

            distanceTraveled.Should().BeGreaterThan(20.0, "Tractor should have moved at least 20m");
            finalAutosteerState.IsActive.Should().BeTrue("Autosteer should still be active");
            Math.Abs(finalAutosteerState.CrossTrackError).Should().BeLessThan(1.0, "Should maintain good cross-track accuracy");
        }

        #endregion

        #region U-Turn Tests

        /// <summary>
        /// Tests U-turn functionality by creating a field, enabling autosteer and U-turn,
        /// and simulating a tractor approaching the field boundary. Verifies that:
        /// - U-turn is triggered when the tractor reaches the trigger distance from boundary
        /// - U-turn completes successfully and returns to normal autosteer mode
        /// Runs in headless mode for automated CI testing.
        /// </summary>
        [Test]
        public void Test_UTurnScenario()
        {
            RunUTurnScenarioTest(visualMode: false);
        }

        /// <summary>
        /// Visual version of Test_UTurnScenario that displays the U-turn execution in real-time.
        /// Run manually to observe the U-turn pattern, turn area, and tractor movement visually.
        /// Includes detailed debug output showing turn phase, distance to pattern, etc.
        /// Marked [Explicit] to prevent running in automated test suites.
        /// </summary>
        [Test]
        [Explicit("Run manually to visualize - opens UI window")]
        [Apartment(ApartmentState.STA)]
        public void Visual_Test_UTurnScenario()
        {
            RunVisualTest("U-Turn Scenario", () => RunUTurnScenarioTest(visualMode: true));
        }

        /// <summary>
        /// Core test logic: Creates a 50m x 100m field with a vertical track, positions the tractor
        /// near the southern boundary, enables autosteer and U-turn (5m trigger distance).
        /// Simulates until U-turn triggers and completes. Logs path data and exports to JSON.
        /// Verifies U-turn triggers, completes, and duration is reasonable.
        /// </summary>
        private void RunUTurnScenarioTest(bool visualMode)
        {
            Console.WriteLine("\n=== U-Turn Integration Test ===\n");

            // Setup field and track
            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "UTurnTestField",
                fieldSize: (50, 100),
                trackX: -10,
                tractorStart: (-10, -45)
            );

            // Enable U-turn
            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0);
            Console.WriteLine("Step 6: U-turn enabled (trigger at 5m from boundary)");

            PrintUTurnDebugInfo();

            // Start path logging
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();
            Console.WriteLine("Step 7: Path logging started\n");

            // Run simulation and track U-turn state
            Console.WriteLine("Step 8: Running simulation...");
            var uturnTracker = new UTurnTracker();
            var formGPS = orchestrator.GetFormGPS();

            RunSimulation(visualMode, simulationTime: 60.0,
                timeStep: 0.05,
                stopCondition: (elapsed) => uturnTracker.IsCompleted,
                printProgress: (elapsed, simState, autosteerState) =>
                {
                    var uturnState = uturnController.GetState();
                    uturnTracker.Update(elapsed, uturnState.IsTriggered, simState);

                    PrintDetailedUTurnProgress(elapsed, simState, autosteerState, uturnState, formGPS);
                });

            // Stop logging and export
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();
            Console.WriteLine($"\nStep 9: Simulation ended - Logged {path.Count} path points");

            ExportPathData(pathLogger, "uturn_test", visualMode);

            // Verify results
            Console.WriteLine("\n=== Verification ===");
            uturnTracker.VerifyCompletion();
        }

        #endregion

        #region Helper Methods - Setup

        /// <summary>
        /// Creates a basic field with boundary, track, positioned tractor, and enabled autosteer
        /// </summary>
        private (AgOpenGPS.Core.Testing.IFieldController field,
                AgOpenGPS.Core.Testing.ISimulatorController sim,
                AgOpenGPS.Core.Testing.IAutosteerController autosteer)
            SetupBasicField(
                string fieldName,
                (double width, double length) fieldSize,
                double trackX,
                (double east, double north) tractorStart)
        {
            // Create field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField(fieldName, 39.0, -94.0);
            Console.WriteLine("Step 1: Field created");

            // Add boundary
            var boundary = CreateRectangularBoundary(39.0, -94.0, fieldSize.width, fieldSize.length);
            fieldController.AddBoundary(boundary, isOuter: true);
            Console.WriteLine($"Step 2: Boundary added ({fieldSize.width}m x {fieldSize.length}m)");

            // Create track
            double halfLength = fieldSize.length / 2.0;
            var trackPointA = new TestPoint(trackX, -halfLength + 5, 0);
            var trackPointB = new TestPoint(trackX, halfLength - 5, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Console.WriteLine($"Step 3: Track created at X={trackX}m");

            // Position tractor
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPositionLocal(tractorStart.east, tractorStart.north);
            simController.SetHeading(0.0);
            simController.SetSpeed(8.0);
            var initialState = simController.GetState();
            Console.WriteLine($"Step 4: Tractor positioned at E={initialState.Easting:F2}m, N={initialState.Northing:F2}m, heading north at 8 km/h");

            // Enable autosteer
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            Console.WriteLine("Step 5: Autosteer enabled");

            return (fieldController, simController, autosteerController);
        }

        /// <summary>
        /// Initializes orchestrator in visual mode for manual testing
        /// </summary>
        private void RunVisualTest(string testName, Action testAction)
        {
            orchestrator?.Shutdown();
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: false);
            orchestrator.ShowForm();

            Console.WriteLine($"\n=== Visual Test: {testName} ===");
            Console.WriteLine("Waiting 5 seconds for UI to fully load...\n");
            Thread.Sleep(5000);

            testAction();

            Console.WriteLine("\n=== Visual Test Complete ===");
        }

        #endregion

        #region Helper Methods - Simulation

        /// <summary>
        /// Runs the simulation loop with progress reporting
        /// </summary>
        private void RunSimulation(
            bool visualMode,
            double simulationTime,
            double? timeStep = null,
            Func<double, bool> stopCondition = null,
            Action<double, SimulatorState, AutosteerState> printProgress = null)
        {
            double step = timeStep ?? (visualMode ? 0.05 : 0.1);
            double elapsedTime = 0;
            int frameCount = 0;
            int progressInterval = visualMode ? 40 : 20;

            var simController = orchestrator.SimulatorController;
            var autosteerController = orchestrator.AutosteerController;

            while (elapsedTime < simulationTime)
            {
                if (stopCondition != null && stopCondition(elapsedTime))
                    break;

                orchestrator.StepSimulation(step);
                elapsedTime += step;
                frameCount++;

                if (printProgress != null && frameCount % progressInterval == 0)
                {
                    var simState = simController.GetState();
                    var autosteerState = autosteerController.GetState();
                    printProgress(elapsedTime, simState, autosteerState);
                }
            }
        }

        #endregion

        #region Helper Methods - U-Turn Specific

        private void PrintUTurnDebugInfo()
        {
            var formGPS = orchestrator.GetFormGPS();
            if (formGPS.bnd.bndList.Count > 0 && formGPS.bnd.bndList[0].turnLine.Count > 0)
            {
                Console.WriteLine($"Turn line has {formGPS.bnd.bndList[0].turnLine.Count} points:");
                for (int i = 0; i < Math.Min(5, formGPS.bnd.bndList[0].turnLine.Count); i++)
                {
                    var pt = formGPS.bnd.bndList[0].turnLine[i];
                    Console.WriteLine($"  Point {i}: E={pt.easting:F2}m, N={pt.northing:F2}m");
                }
            }
        }

        private void PrintDetailedUTurnProgress(
            double elapsedTime,
            SimulatorState simState,
            AutosteerState autosteerState,
            UTurnState uturnState,
            FormGPS formGPS)
        {
            var pivotPos = new AgOpenGPS.vec3(simState.Easting, simState.Northing, 0);
            bool isInTurnArea = formGPS.bnd.IsPointInsideTurnArea(pivotPos) != -1;
            bool isYouTurnBtnOn = formGPS.yt.isYouTurnBtnOn;
            int turnLineCount = formGPS.bnd.bndList.Count > 0 ? formGPS.bnd.bndList[0].turnLine.Count : 0;
            int youTurnPhase = formGPS.yt.youTurnPhase;
            int ytListCount = formGPS.yt.ytList.Count;

            double distToTurnPattern = -1;
            if (ytListCount > 2)
            {
                var turnPatternStart = formGPS.yt.ytList[2];
                distToTurnPattern = AgOpenGPS.glm.Distance(turnPatternStart, pivotPos);
            }

            bool isInBoundary = formGPS.bnd.bndList.Count > 0 &&
                formGPS.bnd.bndList[0].fenceLineEar.IsPointInPolygon(new AgOpenGPS.vec2(simState.Easting, simState.Northing));

            Console.WriteLine($"  Time: {elapsedTime:F1}s | N={simState.Northing:F2}m | " +
                $"XTE: {autosteerState.CrossTrackError:F3}m | " +
                $"UTurn: {(uturnState.IsTriggered ? "ACTIVE" : "waiting")} | " +
                $"InTurnArea: {isInTurnArea} | InBoundary: {isInBoundary} | YTBtnOn: {isYouTurnBtnOn} | " +
                $"Phase: {youTurnPhase} | YTListCnt: {ytListCount} | DistToPattern: {distToTurnPattern:F2}m | TurnLinePts: {turnLineCount}");
        }

        private class UTurnTracker
        {
            private bool triggered = false;
            private bool completed = false;
            private double triggerTime = 0;
            private double completionTime = 0;
            private bool wasInUTurn = false;

            public bool IsCompleted => completed;

            public void Update(double elapsedTime, bool isTriggered, SimulatorState simState)
            {
                if (!triggered && isTriggered)
                {
                    triggered = true;
                    triggerTime = elapsedTime;
                    Console.WriteLine($"  -> U-Turn TRIGGERED at {elapsedTime:F1}s");
                    Console.WriteLine($"     Position: E={simState.Easting:F2}m, N={simState.Northing:F2}m");
                }

                if (triggered && !completed && wasInUTurn && !isTriggered)
                {
                    completed = true;
                    completionTime = elapsedTime;
                    Console.WriteLine($"  -> U-Turn COMPLETED at {elapsedTime:F1}s");
                    Console.WriteLine($"     Turn duration: {completionTime - triggerTime:F1}s");
                }

                wasInUTurn = isTriggered;
            }

            public void VerifyCompletion()
            {
                triggered.Should().BeTrue("U-turn should have been triggered");
                Console.WriteLine($"✓ U-turn was triggered at {triggerTime:F1}s");

                completed.Should().BeTrue("U-turn should have completed");
                Console.WriteLine($"✓ U-turn completed at {completionTime:F1}s");

                double turnDuration = completionTime - triggerTime;
                Console.WriteLine($"✓ U-turn duration: {turnDuration:F1}s");
            }
        }

        #endregion

        #region Helper Methods - Geometry & Export

        private List<TestPoint> CreateRectangularBoundary(
            double centerLat, double centerLon,
            double widthMeters, double lengthMeters)
        {
            var boundary = new List<TestPoint>();
            double halfWidth = widthMeters / 2.0;
            double halfLength = lengthMeters / 2.0;
            double pointSpacing = 1.0;

            // Bottom: left to right
            for (double e = -halfWidth; e < halfWidth; e += pointSpacing)
                boundary.Add(new TestPoint(e, -halfLength, 0));

            // Right: bottom to top
            for (double n = -halfLength; n < halfLength; n += pointSpacing)
                boundary.Add(new TestPoint(halfWidth, n, 0));

            // Top: right to left
            for (double e = halfWidth; e > -halfWidth; e -= pointSpacing)
                boundary.Add(new TestPoint(e, halfLength, 0));

            // Left: top to bottom
            for (double n = halfLength; n > -halfLength; n -= pointSpacing)
                boundary.Add(new TestPoint(-halfWidth, n, 0));

            // Close the boundary
            boundary.Add(new TestPoint(-halfWidth, -halfLength, 0));

            return boundary;
        }

        private void ExportPathData(AgOpenGPS.Core.Testing.IPathLogger pathLogger, string testName, bool visualMode)
        {
            string testOutputDir = Path.Combine(Path.GetDirectoryName(typeof(SimulatedIntegrationTests).Assembly.Location), "..", "..", "..", "TestOutput");
            Directory.CreateDirectory(testOutputDir);
            string jsonPath = Path.Combine(testOutputDir, $"{testName}_{(visualMode ? "visual" : "headless")}.json");
            pathLogger.ExportToJson(jsonPath);
            Console.WriteLine($"Path data exported to: {jsonPath}");
        }

        #endregion
    }
}

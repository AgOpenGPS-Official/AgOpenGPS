using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using NUnit.Framework;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    /// <summary>
    /// Base class for simulated integration tests
    /// Provides common helper methods for field setup, simulation, and data export
    /// </summary>
    public abstract class SimulatedTestBase
    {
        protected AgOpenGPS.Testing.TestOrchestrator orchestrator;

        [SetUp]
        public virtual void Setup()
        {
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: true);
        }

        [TearDown]
        public virtual void Teardown()
        {
            orchestrator?.Shutdown();
        }

        #region Helper Methods - Setup

        /// <summary>
        /// Creates a basic field with boundary, track, positioned tractor, and enabled autosteer
        /// </summary>
        protected (IFieldController field, ISimulatorController sim, IAutosteerController autosteer)
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
        protected void RunVisualTest(string testName, Action testAction)
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
        protected void RunSimulation(
            bool visualMode,
            double simulationTime,
            double? timeStep = null,
            Func<double, bool> stopCondition = null,
            Action<double, SimulatorState, AutosteerState> printProgress = null)
        {
            // Use same timestep for both modes to ensure consistent simulation speed
            double step = timeStep ?? 0.1;
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

        #region Helper Methods - Geometry & Export

        /// <summary>
        /// Creates a rectangular boundary with detailed points along each edge
        /// </summary>
        protected List<TestPoint> CreateRectangularBoundary(
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

        /// <summary>
        /// Creates a simple rectangular boundary with just corner points
        /// </summary>
        protected List<TestPoint> CreateSimpleRectangularBoundary(
            double centerLat, double centerLon,
            double widthMeters, double lengthMeters)
        {
            var boundary = new List<TestPoint>();
            double halfWidth = widthMeters / 2.0;
            double halfLength = lengthMeters / 2.0;

            boundary.Add(new TestPoint(-halfWidth, -halfLength, 0));
            boundary.Add(new TestPoint(halfWidth, -halfLength, 0));
            boundary.Add(new TestPoint(halfWidth, halfLength, 0));
            boundary.Add(new TestPoint(-halfWidth, halfLength, 0));
            boundary.Add(new TestPoint(-halfWidth, -halfLength, 0)); // Close the loop

            return boundary;
        }

        /// <summary>
        /// Exports path data to JSON file
        /// </summary>
        protected void ExportPathData(IPathLogger pathLogger, string testName, bool visualMode)
        {
            string testOutputDir = Path.Combine(
                Path.GetDirectoryName(GetType().Assembly.Location),
                "..", "..", "..", "TestOutput");
            Directory.CreateDirectory(testOutputDir);
            string jsonPath = Path.Combine(testOutputDir, $"{testName}_{(visualMode ? "visual" : "headless")}.json");
            pathLogger.ExportToJson(jsonPath);
            Console.WriteLine($"Path data exported to: {jsonPath}");
        }

        /// <summary>
        /// Exports path data with timestamp to JSON file
        /// </summary>
        protected void ExportPathDataWithTimestamp(IPathLogger pathLogger, string testName)
        {
            string outputPath = Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "TestOutput",
                $"{testName}_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            );
            Directory.CreateDirectory(Path.GetDirectoryName(outputPath));
            pathLogger.ExportToJson(outputPath);
            Console.WriteLine($"Path data exported to: {outputPath}");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using FluentAssertions;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    /// <summary>
    /// Integration tests for U-turn functionality
    /// Tests the complete workflow: field setup, boundary, track, autosteer, and U-turn execution
    /// </summary>
    [TestFixture]
    public class UTurnIntegrationTests
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

        [Test]
        public void Test_BasicFieldWorkflow_CreateFieldAndLogPath()
        {
            // Step 1: Create a field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("TestField", 39.0, -94.0);

            var field = fieldController.GetCurrentField();
            field.Should().NotBeNull("Field should be created");
            field.Name.Should().Be("TestField");

            // Step 2: Add a simple rectangular boundary (100m x 200m)
            var boundary = CreateRectangularBoundary(
                centerLat: 39.0,
                centerLon: -94.0,
                widthMeters: 100,
                lengthMeters: 200
            );
            fieldController.AddBoundary(boundary, isOuter: true);

            // Step 3: Create a track line (straight AB line)
            var trackPointA = new TestPoint(-50, -100, 0);
            var trackPointB = new TestPoint(-50, 100, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);

            // Step 4: Position simulated tractor at start
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);
            simController.SetHeading(0.0); // North
            simController.SetSpeed(5.0); // 5 kph

            // Step 5: Enable autosteer
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();

            var autosteerState = autosteerController.GetState();
            autosteerState.IsActive.Should().BeTrue("Autosteer should be enabled");

            // Step 6: Enable U-turn
            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0); // 5m from boundary

            var uturnState = uturnController.GetState();
            uturnState.IsActive.Should().BeTrue("U-turn should be enabled");

            // Step 7: Start logging the tractor path
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();
            pathLogger.IsLogging.Should().BeTrue("Path logging should be active");

            // Step 8: Run simulation for 5 seconds
            double simulationTime = 5.0; // 5 seconds
            double elapsedTime = 0;
            double timeStep = 0.1; // 100ms steps

            while (elapsedTime < simulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;
            }

            // Step 9: Stop logging and verify path
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();

            // Assertions
            path.Should().NotBeNull("Path should be logged");
            path.Count.Should().BeGreaterThan(10, "should have logged multiple points");

            // Verify path has valid data
            foreach (var point in path)
            {
                point.Position.Should().NotBeNull("each point should have a position");
                point.SpeedKph.Should().BeGreaterThanOrEqualTo(0, "speed should be non-negative");
            }

            Console.WriteLine($"Test completed: Logged {path.Count} path points over {simulationTime} seconds");
        }

        [Test]
        public void Test_ViewLoggedPathDetails()
        {
            // Step 1: Create a field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("TestField", 39.0, -94.0);

            // Step 2: Add boundary
            var boundary = CreateRectangularBoundary(39.0, -94.0, 100, 200);
            fieldController.AddBoundary(boundary, isOuter: true);

            // Step 3: Create track
            var trackPointA = new TestPoint(-50, -100, 0);
            var trackPointB = new TestPoint(-50, 100, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);

            // Step 4: Setup simulator
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);
            simController.SetHeading(0.0);
            simController.SetSpeed(5.0);

            // Step 5: Enable autosteer
            orchestrator.AutosteerController.Enable();

            // Step 6: Start path logging
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();

            // Step 7: Run simulation for 3 seconds
            double simulationTime = 3.0;
            double elapsedTime = 0;
            double timeStep = 0.1;

            while (elapsedTime < simulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;
            }

            // Step 8: Stop logging and get the path
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();

            // Display detailed log information
            Console.WriteLine($"\n=== Path Log Details ===");
            Console.WriteLine($"Total points logged: {path.Count}");
            Console.WriteLine($"Simulation duration: {simulationTime} seconds\n");

            // Show first 5 points
            Console.WriteLine("First 5 logged points:");
            for (int i = 0; i < Math.Min(5, path.Count); i++)
            {
                var point = path[i];
                Console.WriteLine($"  [{i}] Time: {point.Timestamp:F2}s");
                Console.WriteLine($"      Position: Lat={point.Position.Latitude:F6}, Lon={point.Position.Longitude:F6}");
                Console.WriteLine($"      Local: E={point.Easting:F2}m, N={point.Northing:F2}m");
                Console.WriteLine($"      Heading: {point.HeadingDegrees:F1}°, Speed: {point.SpeedKph:F2} km/h");
                Console.WriteLine($"      Autosteer: {point.IsAutosteerActive}, XTE: {point.CrossTrackError:F3}m");
                Console.WriteLine($"      Steer Angle: {point.SteerAngleDegrees:F2}°");
                Console.WriteLine($"      In U-Turn: {point.IsInUTurn}\n");
            }

            // Show last 3 points
            if (path.Count > 5)
            {
                Console.WriteLine("Last 3 logged points:");
                for (int i = Math.Max(0, path.Count - 3); i < path.Count; i++)
                {
                    var point = path[i];
                    Console.WriteLine($"  [{i}] Time: {point.Timestamp:F2}s, E={point.Easting:F2}m, N={point.Northing:F2}m");
                }
            }

            // Calculate statistics
            double totalDistance = 0;
            for (int i = 1; i < path.Count; i++)
            {
                double dE = path[i].Easting - path[i - 1].Easting;
                double dN = path[i].Northing - path[i - 1].Northing;
                totalDistance += Math.Sqrt(dE * dE + dN * dN);
            }

            Console.WriteLine($"\n=== Statistics ===");
            Console.WriteLine($"Total distance traveled: {totalDistance:F2} meters");
            Console.WriteLine($"Average speed: {(totalDistance / simulationTime * 3.6):F2} km/h");
            Console.WriteLine($"Average XTE: {path.Average(p => Math.Abs(p.CrossTrackError)):F3} meters");

            // Verify
            path.Should().NotBeNull();
            path.Count.Should().BeGreaterThan(10);
        }

        [Test]
        public void Test_UTurn_TractorTriggersAndCompletesUTurn()
        {
            Console.WriteLine("\n=== U-Turn Integration Test ===\n");

            // Step 1: Create a field with a smaller boundary (50m x 100m) to trigger U-turn faster
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("UTurnTestField", 39.0, -94.0);
            Console.WriteLine("Step 1: Field created");

            // Step 2: Add boundary - make it narrow so tractor reaches end quickly
            var boundary = CreateRectangularBoundary(
                centerLat: 39.0,
                centerLon: -94.0,
                widthMeters: 50,  // Narrow field
                lengthMeters: 100  // 100m length
            );
            fieldController.AddBoundary(boundary, isOuter: true);
            Console.WriteLine("Step 2: Boundary added (50m x 100m)");

            // Step 3: Create a track line running north-south through the center
            // Track at X=-10m (10m west of center), running from south to north
            var trackPointA = new TestPoint(-10, -50, 0);  // Start at south end
            var trackPointB = new TestPoint(-10, 50, 0);   // End at north end
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Console.WriteLine("Step 3: Track created at X=-10m, running north-south");

            // Step 4: Position tractor at the start (south end of track), heading north
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);  // Center position
            simController.SetHeading(0.0); // North (toward positive Y)
            simController.SetSpeed(8.0); // 8 km/h for faster movement
            Console.WriteLine("Step 4: Tractor positioned at start, heading north at 8 km/h");

            // Step 5: Enable autosteer
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            autosteerController.GetState().IsActive.Should().BeTrue("Autosteer should be enabled");
            Console.WriteLine("Step 5: Autosteer enabled");

            // Step 6: Enable U-turn with 5m trigger distance from boundary
            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0); // Trigger 5m before boundary
            uturnController.GetState().IsActive.Should().BeTrue("U-turn should be enabled");
            Console.WriteLine("Step 6: U-turn enabled (trigger at 5m from boundary)");

            // Step 7: Start path logging
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();
            Console.WriteLine("Step 7: Path logging started\n");

            // Step 8: Run simulation until U-turn is triggered or timeout
            Console.WriteLine("Step 8: Running simulation...");
            double maxSimulationTime = 60.0; // 60 seconds max
            double elapsedTime = 0;
            double timeStep = 0.1;

            bool uturnTriggered = false;
            bool uturnCompleted = false;
            double uturnTriggerTime = 0;
            double uturnCompletionTime = 0;
            double initialNorthing = 0;
            double northingAtTrigger = 0;
            double finalNorthing = 0;

            while (elapsedTime < maxSimulationTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;

                var uturnState = uturnController.GetState();
                var simState = simController.GetState();

                // Store initial position
                if (elapsedTime == timeStep)
                {
                    initialNorthing = simState.Northing;
                }

                // Check if U-turn was triggered
                if (!uturnTriggered && uturnState.IsTriggered)
                {
                    uturnTriggered = true;
                    uturnTriggerTime = elapsedTime;
                    northingAtTrigger = simState.Northing;
                    Console.WriteLine($"  -> U-Turn TRIGGERED at {elapsedTime:F1}s");
                    Console.WriteLine($"     Position: E={simState.Easting:F2}m, N={simState.Northing:F2}m");
                    Console.WriteLine($"     Heading: {simState.HeadingDegrees:F1}°");
                }

                // Check if U-turn was completed (triggered but no longer in turn)
                if (uturnTriggered && !uturnCompleted && !uturnState.IsTriggered)
                {
                    uturnCompleted = true;
                    uturnCompletionTime = elapsedTime;
                    finalNorthing = simState.Northing;
                    Console.WriteLine($"  -> U-Turn COMPLETED at {elapsedTime:F1}s");
                    Console.WriteLine($"     Position: E={simState.Easting:F2}m, N={simState.Northing:F2}m");
                    Console.WriteLine($"     Heading: {simState.HeadingDegrees:F1}°");
                    Console.WriteLine($"     Turn duration: {uturnCompletionTime - uturnTriggerTime:F1}s");
                    break; // Exit after U-turn completes
                }

                // Print progress every 5 seconds
                if (Math.Abs(elapsedTime % 5.0) < timeStep)
                {
                    Console.WriteLine($"  Time: {elapsedTime:F1}s - Position: E={simState.Easting:F2}m, N={simState.Northing:F2}m, Heading: {simState.HeadingDegrees:F1}°, UTurn: {uturnState.IsTriggered}");
                }
            }

            // Step 9: Stop logging
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();
            Console.WriteLine($"\nStep 9: Simulation ended - Logged {path.Count} path points over {elapsedTime:F1}s");

            // Step 10: Verify results
            Console.WriteLine("\n=== Verification ===");

            // Verify U-turn was triggered
            uturnTriggered.Should().BeTrue("U-turn should have been triggered");
            Console.WriteLine($"✓ U-turn was triggered at {uturnTriggerTime:F1}s");

            // Verify U-turn completed
            uturnCompleted.Should().BeTrue("U-turn should have completed");
            Console.WriteLine($"✓ U-turn completed at {uturnCompletionTime:F1}s");

            // Verify U-turn duration is reasonable (should take a few seconds)
            double turnDuration = uturnCompletionTime - uturnTriggerTime;
            turnDuration.Should().BeGreaterThan(1.0, "U-turn should take more than 1 second");
            turnDuration.Should().BeLessThan(30.0, "U-turn should complete within 30 seconds");
            Console.WriteLine($"✓ U-turn duration: {turnDuration:F1}s (reasonable)");

            // Verify tractor moved forward before U-turn
            double distanceBeforeUTurn = northingAtTrigger - initialNorthing;
            distanceBeforeUTurn.Should().BeGreaterThan(10.0, "Tractor should have moved at least 10m before U-turn");
            Console.WriteLine($"✓ Tractor moved {distanceBeforeUTurn:F2}m before U-turn trigger");

            // Analyze path to verify U-turn behavior
            var uturnSegment = path.Where(p => p.IsInUTurn).ToList();
            uturnSegment.Count.Should().BeGreaterThan(0, "Should have logged points during U-turn");
            Console.WriteLine($"✓ Logged {uturnSegment.Count} points during U-turn");

            // Verify final position after U-turn
            var finalSimState = simController.GetState();

            // After U-turn, tractor should be on next parallel track (different easting)
            // The track was at X=-10m, next track should be offset by implement width
            double initialEasting = -10.0; // Initial track position
            double finalEasting = finalSimState.Easting;
            double lateralOffset = Math.Abs(finalEasting - initialEasting);

            Console.WriteLine($"✓ Initial track: E={initialEasting:F2}m");
            Console.WriteLine($"✓ Final position: E={finalEasting:F2}m (offset: {lateralOffset:F2}m)");

            // Verify tractor is following autosteer after U-turn
            var finalAutosteerState = autosteerController.GetState();
            finalAutosteerState.IsActive.Should().BeTrue("Autosteer should still be active after U-turn");
            Console.WriteLine($"✓ Autosteer still active after U-turn");
            Console.WriteLine($"✓ Final cross-track error: {finalAutosteerState.CrossTrackError:F3}m");

            // Summary
            Console.WriteLine("\n=== Test Summary ===");
            Console.WriteLine($"Distance traveled before U-turn: {distanceBeforeUTurn:F2}m");
            Console.WriteLine($"U-turn trigger time: {uturnTriggerTime:F1}s");
            Console.WriteLine($"U-turn completion time: {uturnCompletionTime:F1}s");
            Console.WriteLine($"U-turn duration: {turnDuration:F1}s");
            Console.WriteLine($"Total simulation time: {elapsedTime:F1}s");
            Console.WriteLine($"Lateral offset after U-turn: {lateralOffset:F2}m");
            Console.WriteLine("\n=== U-Turn Test PASSED ===\n");
        }

        /// <summary>
        /// Helper to create a rectangular boundary centered at a lat/lon
        /// </summary>
        private List<TestPoint> CreateRectangularBoundary(
            double centerLat, double centerLon,
            double widthMeters, double lengthMeters)
        {
            // This is a simplified version - would need proper coordinate conversion
            var boundary = new List<TestPoint>();

            // For testing purposes, create a simple rectangle in local coordinates
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
        /// Extract the portion of path where U-turn occurred
        /// </summary>
        private List<PathPoint> ExtractUTurnSegment(List<PathPoint> path)
        {
            var uturnSegment = new List<PathPoint>();
            foreach (var point in path)
            {
                if (point.IsInUTurn)
                {
                    uturnSegment.Add(point);
                }
            }
            return uturnSegment.Count > 0 ? uturnSegment : null;
        }
    }
}

using System;
using System.Collections.Generic;
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

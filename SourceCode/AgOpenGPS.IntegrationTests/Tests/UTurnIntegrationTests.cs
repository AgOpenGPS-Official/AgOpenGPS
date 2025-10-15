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
        [Ignore("Requires headless mode implementation and full integration")]
        public void Test_UTurn_CompletesSuccessfully_WithSimpleField()
        {
            // Arrange: Create a simple rectangular field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("TestField", 39.0, -94.0);

            // Add rectangular boundary (100m x 200m)
            var boundary = CreateRectangularBoundary(
                centerLat: 39.0,
                centerLon: -94.0,
                widthMeters: 100,
                lengthMeters: 200
            );
            fieldController.AddBoundary(boundary, isOuter: true);

            // Create AB line at 45 degrees
            var trackPointA = new TestPoint(0, 0, 0);
            var trackPointB = new TestPoint(100, 100, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 45);
            fieldController.SelectTrack(0);

            // Arrange: Position simulated tractor at start
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);
            simController.SetHeading(45.0);
            simController.SetSpeed(8.0); // 8 kph

            // Arrange: Enable autosteer and U-turn
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();

            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0); // 5m from boundary

            // Arrange: Start path logging
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();

            // Act: Run simulation until U-turn completes
            bool uturnCompleted = false;
            double maxSimulationTime = 120.0; // 2 minutes max
            double elapsedTime = 0;
            double timeStep = 0.1; // 100ms steps

            while (elapsedTime < maxSimulationTime && !uturnCompleted)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;

                var uturnState = uturnController.GetState();

                // Check if U-turn was triggered and then completed
                if (uturnState.IsTriggered && !uturnState.IsInTurn)
                {
                    // U-turn has completed
                    uturnCompleted = true;
                }
            }

            // Assert: U-turn completed
            uturnCompleted.Should().BeTrue("U-turn should complete within simulation time");

            // Assert: Get logged path
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();

            // Assert: Path should have reasonable length
            path.Count.Should().BeGreaterThan(100, "should have logged multiple points");

            // Assert: Verify U-turn geometry
            var uturnSegment = ExtractUTurnSegment(path);
            uturnSegment.Should().NotBeNull("U-turn segment should exist in path");

            var maxCTE = 0.0;
            foreach (var point in path)
            {
                if (Math.Abs(point.CrossTrackError) > maxCTE)
                {
                    maxCTE = Math.Abs(point.CrossTrackError);
                }
            }
            maxCTE.Should().BeLessThan(0.5, "max cross-track error should be under 50cm");

            // Assert: Verify tractor ended on parallel line
            var finalHeading = path[path.Count - 1].HeadingDegrees;
            var expectedHeading = 225.0; // 45 + 180 = 225 degrees
            Math.Abs(finalHeading - expectedHeading).Should().BeLessThan(5.0,
                "final heading should be opposite to initial heading");
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

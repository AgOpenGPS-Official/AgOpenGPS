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
    /// Integration test for complete field work operation
    /// Tests working a field from edge to edge with proper U-turn directions and implement control
    /// </summary>
    [TestFixture]
    public class FieldWorkTest
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
        public void Test_CompleteFieldWork_FromEdgeToEdge_WithUTurns()
        {
            Console.WriteLine("=== Starting Complete Field Work Test ===\n");

            // Step 1: Create a field
            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("FieldWorkTest", 39.0, -94.0);
            Console.WriteLine("1. Field created: FieldWorkTest");

            // Step 2: Create a small rectangular field for testing (30m wide x 100m long)
            // This ensures we can complete multiple passes quickly in the test
            double fieldWidth = 30;  // meters
            double fieldLength = 100; // meters
            var boundary = CreateRectangularBoundary(
                centerLat: 39.0,
                centerLon: -94.0,
                widthMeters: fieldWidth,
                lengthMeters: fieldLength
            );
            fieldController.AddBoundary(boundary, isOuter: true);
            Console.WriteLine($"2. Boundary added: {fieldWidth}m x {fieldLength}m rectangle");

            // Step 3: Create first track line on the left edge of the field
            // Start 3m from the left edge to give room for the implement
            double startEasting = -fieldWidth / 2.0 + 3.0;
            var trackPointA = new TestPoint(startEasting, -fieldLength / 2.0 + 5, 0);
            var trackPointB = new TestPoint(startEasting, fieldLength / 2.0 - 5, 0);
            fieldController.CreateTrack(trackPointA, trackPointB, headingDegrees: 0);
            fieldController.SelectTrack(0);
            Console.WriteLine($"3. Track created at E={startEasting:F1}m heading North");

            // Step 4: Position tractor at start of first pass
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPositionLocal(startEasting, -fieldLength / 2.0 + 5);
            simController.SetHeading(0.0); // North
            simController.SetSpeed(8.0); // 8 kph - typical field speed
            Console.WriteLine($"4. Tractor positioned at start: E={startEasting:F1}m, N={-fieldLength / 2.0 + 5:F1}m");

            // Step 5: Enable autosteer
            var autosteerController = orchestrator.AutosteerController;
            autosteerController.Enable();
            Console.WriteLine("5. Autosteer enabled");

            // Step 6: Enable U-turn with proper settings
            var uturnController = orchestrator.UTurnController;
            uturnController.Enable();
            uturnController.SetDistanceFromBoundary(5.0); // 5m trigger distance
            Console.WriteLine("6. U-turn enabled (trigger at 5m from boundary)");

            // Step 7: Turn on implement/sections
            var formGPS = orchestrator.GetFormGPS();
            for (int i = 0; i < FormGPS.MAXSECTIONS; i++)
            {
                formGPS.section[i].isSectionOn = true;
            }
            Console.WriteLine($"7. Implement sections enabled ({FormGPS.MAXSECTIONS} sections)");

            // Step 8: Start path logging
            var pathLogger = orchestrator.PathLogger;
            pathLogger.StartLogging();
            Console.WriteLine("8. Path logging started\n");

            // Step 9: Run simulation until tractor crosses field boundary
            Console.WriteLine("=== Starting Simulation ===");
            double timeStep = 0.1; // 100ms steps
            double elapsedTime = 0;
            double maxSimulationTime = 600.0; // 10 minute timeout
            int uturnCount = 0;
            bool wasInUTurn = false;
            bool hasLeftField = false;

            // Track boundary crossing
            double halfWidth = fieldWidth / 2.0;
            double halfLength = fieldLength / 2.0;

            while (elapsedTime < maxSimulationTime && !hasLeftField)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;

                var state = simController.GetState();
                var uturnState = uturnController.GetState();

                // Count U-turns
                if (uturnState.IsTriggered && !wasInUTurn)
                {
                    uturnCount++;
                    Console.WriteLine($"  [{elapsedTime:F1}s] U-turn #{uturnCount} triggered at E={state.Easting:F1}m, N={state.Northing:F1}m");
                }
                wasInUTurn = uturnState.IsTriggered;

                // Check if tractor has left the field boundary
                if (Math.Abs(state.Easting) > halfWidth || Math.Abs(state.Northing) > halfLength)
                {
                    hasLeftField = true;
                    Console.WriteLine($"\n  [{elapsedTime:F1}s] Tractor crossed field boundary at E={state.Easting:F1}m, N={state.Northing:F1}m");
                }

                // Progress update every 30 seconds
                if ((int)(elapsedTime * 10) % 300 == 0)
                {
                    Console.WriteLine($"  [{elapsedTime:F1}s] Position: E={state.Easting:F1}m, N={state.Northing:F1}m, H={state.HeadingDegrees:F1}°, UTurns={uturnCount}");
                }
            }

            // Step 10: Stop logging
            pathLogger.StopLogging();
            var path = pathLogger.GetLoggedPath();
            Console.WriteLine($"\n=== Simulation Complete ===");
            Console.WriteLine($"Total time: {elapsedTime:F1} seconds");
            Console.WriteLine($"Total U-turns: {uturnCount}");
            Console.WriteLine($"Path points logged: {path.Count}");

            // Step 11: Calculate field coverage
            var coverage = CalculateFieldCoverage(formGPS, fieldWidth, fieldLength);
            Console.WriteLine($"\n=== Field Coverage ===");
            Console.WriteLine($"Field area: {coverage.FieldArea:F1} m²");
            Console.WriteLine($"Covered area: {coverage.CoveredArea:F1} m²");
            Console.WriteLine($"Coverage: {coverage.CoveragePercent:F1}%");
            Console.WriteLine($"Overlap area: {coverage.OverlapArea:F1} m²");
            Console.WriteLine($"Overlap: {coverage.OverlapPercent:F1}%");

            // Step 12: Analyze U-turn directions
            var uturnAnalysis = AnalyzeUTurnDirections(path);
            Console.WriteLine($"\n=== U-Turn Analysis ===");
            Console.WriteLine($"Total U-turns detected: {uturnAnalysis.TotalUTurns}");
            Console.WriteLine($"Left turns: {uturnAnalysis.LeftTurns}");
            Console.WriteLine($"Right turns: {uturnAnalysis.RightTurns}");
            Console.WriteLine($"U-turns should alternate direction: {(uturnAnalysis.AlternatesCorrectly ? "PASS" : "FAIL")}");

            // Step 13: Export path data for visualization
            string outputPath = System.IO.Path.Combine(
                TestContext.CurrentContext.TestDirectory,
                "TestOutput",
                $"FieldWork_{DateTime.Now:yyyyMMdd_HHmmss}.json"
            );
            System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));
            pathLogger.ExportToJson(outputPath);
            Console.WriteLine($"\n=== Output ===");
            Console.WriteLine($"Path data exported to: {outputPath}");

            // Assertions
            hasLeftField.Should().BeTrue("Tractor should complete field and cross boundary");
            uturnCount.Should().BeGreaterThan(0, "Should have performed at least one U-turn");
            coverage.CoveragePercent.Should().BeGreaterThan(50, "Should have covered more than 50% of field");
            coverage.CoveragePercent.Should().BeLessThan(150, "Coverage should be realistic (< 150% including overlaps)");
            uturnAnalysis.AlternatesCorrectly.Should().BeTrue("U-turns should alternate between left and right");
        }

        /// <summary>
        /// Helper to create a rectangular boundary centered at a lat/lon
        /// </summary>
        private List<TestPoint> CreateRectangularBoundary(
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
        /// Calculate field coverage percentage from FormGPS field data
        /// </summary>
        private CoverageResult CalculateFieldCoverage(FormGPS formGPS, double fieldWidth, double fieldLength)
        {
            double fieldArea = fieldWidth * fieldLength;

            // Get actual coverage data from FormGPS field data
            double workedArea = formGPS.fd.workedAreaTotal;
            double actualAreaCovered = formGPS.fd.actualAreaCovered;
            double boundaryArea = formGPS.fd.areaBoundaryOuterLessInner;

            // If boundary area is not set (shouldn't happen but just in case), use calculated field area
            if (boundaryArea < 10)
            {
                boundaryArea = fieldArea;
            }

            // Calculate overlap
            double overlapArea = workedArea - actualAreaCovered;

            return new CoverageResult
            {
                FieldArea = boundaryArea,
                CoveredArea = actualAreaCovered,
                CoveragePercent = (actualAreaCovered / boundaryArea) * 100.0,
                OverlapArea = overlapArea,
                OverlapPercent = workedArea > 0 ? (overlapArea / workedArea) * 100.0 : 0
            };
        }

        /// <summary>
        /// Analyze U-turn directions to ensure they alternate properly
        /// </summary>
        private UTurnAnalysis AnalyzeUTurnDirections(List<PathPoint> path)
        {
            var analysis = new UTurnAnalysis();
            var uturnSegments = new List<UTurnSegment>();

            bool inUTurn = false;
            double uturnStartHeading = 0;
            double uturnStartTime = 0;

            for (int i = 0; i < path.Count; i++)
            {
                var point = path[i];

                if (point.IsInUTurn && !inUTurn)
                {
                    // U-turn started
                    inUTurn = true;
                    uturnStartHeading = point.HeadingDegrees;
                    uturnStartTime = point.Timestamp;
                }
                else if (!point.IsInUTurn && inUTurn)
                {
                    // U-turn ended
                    inUTurn = false;
                    double uturnEndHeading = point.HeadingDegrees;

                    // Calculate heading change (normalize to -180 to 180)
                    double headingChange = uturnEndHeading - uturnStartHeading;
                    while (headingChange > 180) headingChange -= 360;
                    while (headingChange < -180) headingChange += 360;

                    var segment = new UTurnSegment
                    {
                        StartTime = uturnStartTime,
                        EndTime = point.Timestamp,
                        HeadingChange = headingChange,
                        IsLeftTurn = headingChange > 0
                    };
                    uturnSegments.Add(segment);
                }
            }

            analysis.TotalUTurns = uturnSegments.Count;
            analysis.LeftTurns = uturnSegments.Count(s => s.IsLeftTurn);
            analysis.RightTurns = uturnSegments.Count(s => !s.IsLeftTurn);

            // Check if turns alternate
            analysis.AlternatesCorrectly = true;
            for (int i = 1; i < uturnSegments.Count; i++)
            {
                if (uturnSegments[i].IsLeftTurn == uturnSegments[i - 1].IsLeftTurn)
                {
                    analysis.AlternatesCorrectly = false;
                    break;
                }
            }

            return analysis;
        }

        private class CoverageResult
        {
            public double FieldArea { get; set; }
            public double CoveredArea { get; set; }
            public double CoveragePercent { get; set; }
            public double OverlapArea { get; set; }
            public double OverlapPercent { get; set; }
        }

        private class UTurnAnalysis
        {
            public int TotalUTurns { get; set; }
            public int LeftTurns { get; set; }
            public int RightTurns { get; set; }
            public bool AlternatesCorrectly { get; set; }
        }

        private class UTurnSegment
        {
            public double StartTime { get; set; }
            public double EndTime { get; set; }
            public double HeadingChange { get; set; }
            public bool IsLeftTurn { get; set; }
        }
    }
}

using System;
using NUnit.Framework;
using FluentAssertions;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    /// <summary>
    /// Tests the accuracy of the grid-based coverage tracker (CCoverageTracker)
    /// by comparing actual coverage against expected coverage in controlled scenarios
    /// </summary>
    [TestFixture]
    public class CoverageAccuracyTest : SimulatedTestBase
    {
        /// <summary>
        /// Test 1: Single straight pass with no overlap
        /// Expected: actualAreaCovered ≈ workedAreaTotal (minimal overlap)
        /// </summary>
        [Test]
        public void Test_SinglePass_NoOverlap()
        {
            Console.WriteLine("=== Test: Single Pass - No Overlap ===\n");

            double fieldWidth = 20;  // 20m wide field
            double fieldLength = 50; // 50m long field
            double implementWidth = 5.0; // 5m implement

            // Single pass down the center
            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "SinglePassTest",
                fieldSize: (fieldWidth, fieldLength),
                trackX: 0.0,  // Center of field
                tractorStart: (0.0, -fieldLength / 2.0 + 5.0)
            );

            var formGPS = orchestrator.GetFormGPS();
            formGPS.JobNew();

            ConfigureImplement(formGPS, implementWidth, numSections: 1);

            // Enable section control
            formGPS.autoBtnState = btnStates.Auto;
            formGPS.section[0].sectionBtnState = btnStates.On;

            // Run straight down the field
            double elapsedTime = 0;
            double timeStep = 0.1;
            double maxTime = 60.0;

            while (elapsedTime < maxTime)
            {
                orchestrator.StepSimulation(timeStep);
                elapsedTime += timeStep;

                var simState = simController.GetState();

                // Stop when we reach the end of the field
                if (simState.Northing > fieldLength / 2.0 - 5.0)
                    break;
            }

            // Get coverage data
            double workedArea = formGPS.fd.workedAreaTotal;
            double actualAreaCovered = formGPS.fd.actualAreaCovered;
            double overlapPercent = formGPS.fd.overlapPercent;

            // Expected: ~5m wide * ~40m long = ~200m²
            double expectedArea = implementWidth * (fieldLength - 10); // Minus start/end buffer

            Console.WriteLine($"=== Coverage Results ===");
            Console.WriteLine($"Worked Area (total):     {workedArea:F2} m²");
            Console.WriteLine($"Actual Area (adjusted):  {actualAreaCovered:F2} m²");
            Console.WriteLine($"Overlap Percentage:      {overlapPercent:F2}%");
            Console.WriteLine($"Expected Area:           {expectedArea:F2} m²");
            Console.WriteLine($"Difference:              {Math.Abs(actualAreaCovered - expectedArea):F2} m² ({Math.Abs(actualAreaCovered - expectedArea) / expectedArea * 100:F1}%)");

            // Assertions for single pass
            workedArea.Should().BeGreaterThan(0, "Should have recorded some coverage");
            actualAreaCovered.Should().BeGreaterThan(0, "actualAreaCovered should be calculated in headless mode");

            // NOTE: Software rasterizer at 1cm resolution accurately detects minimal overlap
            // on straight passes. Previous grid-based tracker (10cm) showed artificial overlap
            // because consecutive quads covered the same grid cells.

            // For a single straight pass with software rasterizer:
            // - workedAreaTotal counts all triangles
            // - actualAreaCovered should be nearly equal to workedAreaTotal (minimal overlap)
            // Ratio typically 0.95-1.0 due to high precision 1cm resolution
            double ratio = actualAreaCovered / workedArea;
            ratio.Should().BeGreaterThan(0.95, "actualAreaCovered should be 95-100% of workedAreaTotal with minimal overlap");
            ratio.Should().BeLessThanOrEqualTo(1.0, "Ratio cannot exceed 1.0");

            // Overlap percentage for single pass should be minimal (< 5%)
            overlapPercent.Should().BeLessThan(5.0, "Single pass shows minimal overlap with 1cm resolution");

            // Actual area should be close to expected (within 10% tolerance with 1cm resolution)
            actualAreaCovered.Should().BeInRange(expectedArea * 0.90, expectedArea * 1.10,
                "Actual coverage should be within 10% of expected area with high-resolution tracking");
        }

        /// <summary>
        /// Test 2: Two parallel passes with perfect spacing (no overlap)
        /// Expected: actualAreaCovered ≈ workedAreaTotal ≈ 2 × implement_width × length
        /// </summary>
        [Test]
        public void Test_TwoParallelPasses_NoOverlap()
        {
            Console.WriteLine("=== Test: Two Parallel Passes - No Overlap ===\n");

            double fieldWidth = 30;
            double fieldLength = 50;
            double implementWidth = 5.0;
            double spacing = implementWidth; // Perfect spacing = no overlap

            // Setup field first
            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "TwoPassTest",
                fieldSize: (fieldWidth, fieldLength),
                trackX: 0.0,
                tractorStart: (0.0, 0.0)
            );

            var formGPS = orchestrator.GetFormGPS();
            formGPS.JobNew();
            ConfigureImplement(formGPS, implementWidth, numSections: 1);

            // Enable section control
            formGPS.autoBtnState = btnStates.Auto;
            formGPS.section[0].sectionBtnState = btnStates.On;

            // Pass 1: X = -2.5m (left of center)
            RunStraightPass(formGPS, -spacing / 2.0, -fieldLength / 2.0 + 5.0, fieldLength - 10);

            // Pass 2: X = +2.5m (right of center)
            RunStraightPass(formGPS, spacing / 2.0, -fieldLength / 2.0 + 5.0, fieldLength - 10);

            // Get coverage data
            double workedArea = formGPS.fd.workedAreaTotal;
            double actualAreaCovered = formGPS.fd.actualAreaCovered;
            double overlapPercent = formGPS.fd.overlapPercent;

            double expectedArea = 2 * implementWidth * (fieldLength - 10);

            Console.WriteLine($"=== Coverage Results ===");
            Console.WriteLine($"Worked Area (total):     {workedArea:F2} m²");
            Console.WriteLine($"Actual Area (adjusted):  {actualAreaCovered:F2} m²");
            Console.WriteLine($"Overlap Percentage:      {overlapPercent:F2}%");
            Console.WriteLine($"Expected Area:           {expectedArea:F2} m²");
            Console.WriteLine($"Difference:              {Math.Abs(actualAreaCovered - expectedArea):F2} m²");

            // Assertions
            actualAreaCovered.Should().BeGreaterThan(0, "actualAreaCovered should be calculated");
            overlapPercent.Should().BeLessThan(10.0, "Two non-overlapping passes should have < 10% overlap");

            double ratio = actualAreaCovered / workedArea;
            ratio.Should().BeGreaterThan(0.90, "Minimal overlap means actualAreaCovered ≈ workedAreaTotal");

            actualAreaCovered.Should().BeInRange(expectedArea * 0.80, expectedArea * 1.20,
                "Actual coverage should be within 20% of expected for two passes");
        }

        /// <summary>
        /// Test 3: Two overlapping passes (50% overlap)
        /// Expected: actualAreaCovered < workedAreaTotal, overlap% ≈ 50%
        /// </summary>
        [Test]
        public void Test_TwoOverlappingPasses_50Percent()
        {
            Console.WriteLine("=== Test: Two Overlapping Passes - 50% Overlap ===\n");

            double fieldWidth = 30;
            double fieldLength = 50;
            double implementWidth = 5.0;
            double spacing = implementWidth / 2.0; // 50% overlap

            // Setup field first
            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "OverlapTest",
                fieldSize: (fieldWidth, fieldLength),
                trackX: 0.0,
                tractorStart: (0.0, 0.0)
            );

            var formGPS = orchestrator.GetFormGPS();
            formGPS.JobNew();
            ConfigureImplement(formGPS, implementWidth, numSections: 1);

            formGPS.autoBtnState = btnStates.Auto;
            formGPS.section[0].sectionBtnState = btnStates.On;

            // Pass 1: X = -1.25m
            RunStraightPass(formGPS, -spacing / 2.0, -fieldLength / 2.0 + 5.0, fieldLength - 10);

            double workedAfterPass1 = formGPS.fd.workedAreaTotal;

            // Pass 2: X = +1.25m (50% overlap with pass 1)
            RunStraightPass(formGPS, spacing / 2.0, -fieldLength / 2.0 + 5.0, fieldLength - 10);

            double workedArea = formGPS.fd.workedAreaTotal;
            double actualAreaCovered = formGPS.fd.actualAreaCovered;
            double overlapPercent = formGPS.fd.overlapPercent;

            // Expected: 1.5 implement widths worth of coverage (50% overlap)
            double expectedArea = 1.5 * implementWidth * (fieldLength - 10);

            Console.WriteLine($"=== Coverage Results ===");
            Console.WriteLine($"Worked Area after pass 1: {workedAfterPass1:F2} m²");
            Console.WriteLine($"Worked Area (total):      {workedArea:F2} m²");
            Console.WriteLine($"Actual Area (adjusted):   {actualAreaCovered:F2} m²");
            Console.WriteLine($"Overlap Percentage:       {overlapPercent:F2}%");
            Console.WriteLine($"Expected Area:            {expectedArea:F2} m²");
            Console.WriteLine($"Expected Overlap:         ~50%");

            // Assertions
            actualAreaCovered.Should().BeGreaterThan(0, "actualAreaCovered should be calculated");
            actualAreaCovered.Should().BeLessThan(workedArea, "With overlap, actual < worked");

            // Overlap should be roughly 50% (allow 30-70% range due to grid approximation)
            overlapPercent.Should().BeInRange(30.0, 70.0, "Overlap should be approximately 50%");

            // Actual area should be roughly 1.5x single pass (allow 20% tolerance)
            actualAreaCovered.Should().BeInRange(expectedArea * 0.80, expectedArea * 1.20,
                "Actual coverage should reflect 50% overlap");
        }

        /// <summary>
        /// Test 4: Complete field coverage with known overlap pattern
        /// This tests the full integration in a realistic scenario
        /// </summary>
        [Test]
        public void Test_CompleteField_KnownOverlap()
        {
            Console.WriteLine("=== Test: Complete Field - Known Overlap Pattern ===\n");

            // Small field for precise validation
            double fieldWidth = 15;   // 15m wide
            double fieldLength = 30;  // 30m long
            double implementWidth = 5.0; // 5m implement
            double overlap = 0.5;     // 0.5m overlap between passes

            double spacing = implementWidth - overlap; // 4.5m spacing

            // Setup field first
            var (fieldController, simController, autosteerController) = SetupBasicField(
                fieldName: "CompleteFieldTest",
                fieldSize: (fieldWidth, fieldLength),
                trackX: 0.0,
                tractorStart: (0.0, 0.0)
            );

            var formGPS = orchestrator.GetFormGPS();
            formGPS.JobNew();
            ConfigureImplement(formGPS, implementWidth, numSections: 1);

            formGPS.autoBtnState = btnStates.Auto;
            formGPS.section[0].sectionBtnState = btnStates.On;

            // Calculate how many passes needed to cover 15m width
            // Pass 1: -5.0 to 0.0 (centered at -2.5)
            // Pass 2: -0.5 to 4.5 (centered at 2.0, 4.5m from pass 1)
            // Pass 3: 4.0 to 9.0 (centered at 6.5, 4.5m from pass 2)
            // This gives us 3 passes with 0.5m overlap each

            double[] passPositions = { -5.0, -0.5, 4.0 };

            foreach (double x in passPositions)
            {
                double centerX = x + implementWidth / 2.0;
                RunStraightPass(formGPS, centerX, -fieldLength / 2.0 + 3.0, fieldLength - 6);
            }

            double workedArea = formGPS.fd.workedAreaTotal;
            double actualAreaCovered = formGPS.fd.actualAreaCovered;
            double overlapPercent = formGPS.fd.overlapPercent;

            // Expected calculations:
            // 3 passes × 5m × 24m = 360m² worked
            // Actual coverage = (5 + 4.5 + 4.5)m × 24m = 14.5m × 24m = 348m²
            // Overlap = (360 - 348) / 360 = 3.3%

            double expectedWorked = 3 * implementWidth * (fieldLength - 6);
            double expectedActual = (implementWidth + 2 * spacing) * (fieldLength - 6);
            double expectedOverlap = ((expectedWorked - expectedActual) / expectedWorked) * 100;

            Console.WriteLine($"=== Coverage Results ===");
            Console.WriteLine($"Worked Area (total):     {workedArea:F2} m² (expected: {expectedWorked:F2} m²)");
            Console.WriteLine($"Actual Area (adjusted):  {actualAreaCovered:F2} m² (expected: {expectedActual:F2} m²)");
            Console.WriteLine($"Overlap Percentage:      {overlapPercent:F2}% (expected: {expectedOverlap:F2}%)");

            // Assertions with tolerances for grid approximation
            workedArea.Should().BeInRange(expectedWorked * 0.90, expectedWorked * 1.10,
                "Worked area should match expected within 10%");

            actualAreaCovered.Should().BeInRange(expectedActual * 0.85, expectedActual * 1.15,
                "Actual coverage should match expected within 15%");

            overlapPercent.Should().BeInRange(0, expectedOverlap + 5.0,
                "Overlap percentage should be reasonable");
        }

        #region Helper Methods

        /// <summary>
        /// Configure implement with specified width and sections
        /// </summary>
        private void ConfigureImplement(FormGPS formGPS, double width, int numSections)
        {
            formGPS.tool.numOfSections = numSections;
            formGPS.tool.overlap = 0.0;
            formGPS.tool.isSectionsNotZones = true;
            formGPS.tool.offset = 0.0;

            double halfWidth = width / 2.0;
            formGPS.section[0].positionLeft = -halfWidth;
            formGPS.section[0].positionRight = halfWidth;
            formGPS.section[0].sectionWidth = width;

            formGPS.SectionCalcWidths();
        }

        /// <summary>
        /// Run a straight pass at a given X position
        /// </summary>
        private void RunStraightPass(FormGPS formGPS, double x, double startY, double distance)
        {
            // Teleport to start position
            var simController = orchestrator.SimulatorController;
            simController.SetPositionLocal(x, startY);
            simController.SetHeading(0.0); // Heading north

            double timeStep = 0.1;
            double distanceTraveled = 0;
            double speed = 8.0 / 3.6; // 8 km/h in m/s

            while (distanceTraveled < distance)
            {
                orchestrator.StepSimulation(timeStep);
                distanceTraveled += speed * timeStep;
            }
        }

        #endregion
    }
}

using AgOpenGPS.Core.Geometry;
using AgOpenGPS.Core.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgOpenGPS.Core.Tests.Geometry
{
    /// <summary>
    /// Comprehensive test suite for GeometryUtils
    /// Tests both correctness and performance according to Performance_First_Guidelines.md
    ///
    /// CRITICAL PERFORMANCE TARGETS:
    /// - FindClosestSegment: <500μs for 1000-point curve ⚡
    /// - FindDistanceToSegmentSquared: <100μs
    /// - Zero allocations in hot paths
    /// </summary>
    [TestFixture]
    public class GeometryUtilsTests
    {
        #region FindClosestSegment - Correctness Tests

        [Test]
        public void FindClosestSegment_EmptyList_ReturnsFalse()
        {
            // Arrange
            var points = new List<vec3>();
            var searchPoint = new vec2(5, 5);

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.False, "Empty list should return false");
            Assert.That(A, Is.EqualTo(-1), "Index A should be -1 for empty list");
            Assert.That(B, Is.EqualTo(-1), "Index B should be -1 for empty list");
        }

        [Test]
        public void FindClosestSegment_NullList_ReturnsFalse()
        {
            // Arrange
            List<vec3> points = null;
            var searchPoint = new vec2(5, 5);

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.False, "Null list should return false");
            Assert.That(A, Is.EqualTo(-1), "Index A should be -1 for null list");
            Assert.That(B, Is.EqualTo(-1), "Index B should be -1 for null list");
        }

        [Test]
        public void FindClosestSegment_SinglePoint_ReturnsFalse()
        {
            // Arrange
            var points = new List<vec3> { new vec3(0, 0, 0) };
            var searchPoint = new vec2(5, 5);

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.False, "Single point cannot form a segment");
        }

        [Test]
        public void FindClosestSegment_TwoPoints_FindsSegment()
        {
            // Arrange - simple horizontal line
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(10, 0, 0)
            };
            var searchPoint = new vec2(5, 3); // Point above middle of line

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.True, "Should find segment");
            Assert.That(A, Is.EqualTo(0), "First point should be A");
            Assert.That(B, Is.EqualTo(1), "Second point should be B");
        }

        [Test]
        public void FindClosestSegment_StraightLine_FindsClosestSegment()
        {
            // Arrange - vertical line with 5 segments
            var points = new List<vec3>();
            for (int i = 0; i < 6; i++)
            {
                points.Add(new vec3(0, i * 10, 0)); // Points at y = 0, 10, 20, 30, 40, 50
            }

            var searchPoint = new vec2(5, 25); // Closest to segment between points 2 and 3

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.True, "Should find segment");
            Assert.That(A, Is.EqualTo(2), "Should find segment starting at point 2");
            Assert.That(B, Is.EqualTo(3), "Should find segment ending at point 3");
        }

        [Test]
        public void FindClosestSegment_Curve_FindsClosestSegment()
        {
            // Arrange - curved path (partial circle)
            var points = new List<vec3>();
            int numPoints = 50;
            double radius = 100;
            for (int i = 0; i < numPoints; i++)
            {
                double angle = i * Math.PI / numPoints; // 0 to PI (semicircle)
                double x = radius * Math.Cos(angle);
                double y = radius * Math.Sin(angle);
                points.Add(new vec3(x, y, 0));
            }

            // Search point near the top of the arc
            var searchPoint = new vec2(10, 90);

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.True, "Should find segment");

            // Closest segment should be near the top of the semicircle (90 degrees)
            int expectedIndex = numPoints / 2;
            Assert.That(A, Is.InRange(expectedIndex - 5, expectedIndex + 5),
                $"Should find segment near middle of semicircle, got A={A} (expected around {expectedIndex})");
        }

        [Test]
        public void FindClosestSegment_Loop_FirstSegmentConnectsLastToFirst()
        {
            // Arrange - square loop
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),   // SW corner
                new vec3(10, 0, 0),  // SE corner
                new vec3(10, 10, 0), // NE corner
                new vec3(0, 10, 0)   // NW corner
            };

            // Search point outside the loop, closest to segment between last and first point
            var searchPoint = new vec2(-5, 5);

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, true);

            // Assert
            Assert.That(result, Is.True, "Should find segment");
            Assert.That(A, Is.EqualTo(3), "Should find segment from last point");
            Assert.That(B, Is.EqualTo(0), "Should connect to first point (loop)");
        }

        [Test]
        public void FindClosestSegment_NonLoop_NoSegmentBeforeFirstPoint()
        {
            // Arrange - open path
            var points = new List<vec3>
            {
                new vec3(0, 0, 0),
                new vec3(10, 0, 0)
            };

            // Search point before the first point
            var searchPoint = new vec2(-5, 0);

            // Act
            bool result = GeometryUtils.FindClosestSegment(points, searchPoint, out int A, out int B, false);

            // Assert
            Assert.That(result, Is.True, "Should find the only segment");
            Assert.That(A, Is.EqualTo(0), "Should find first segment");
            Assert.That(B, Is.EqualTo(1), "Should find first segment");
        }

        #endregion

        #region FindDistanceToSegmentSquared - Correctness Tests

        [Test]
        public void FindDistanceToSegmentSquared_PointOnSegment_ReturnsZero()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);
            var pt = new vec2(5, 0); // Point on segment

            // Act
            double distSq = GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(0).Within(0.001), "Point on segment should have distance 0");
        }

        [Test]
        public void FindDistanceToSegmentSquared_PerpendicularDistance_Correct()
        {
            // Arrange - horizontal segment, point above
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);
            var pt = new vec2(5, 3); // 3 meters above midpoint

            // Act
            double distSq = GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(9.0).Within(0.001), "Distance should be 3^2 = 9");
        }

        [Test]
        public void FindDistanceToSegmentSquared_PointBeforeSegment_DistanceToP1()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);
            var pt = new vec2(-5, 0); // Point before segment

            // Act
            double distSq = GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(25.0).Within(0.001), "Distance should be to p1: 5^2 = 25");
        }

        [Test]
        public void FindDistanceToSegmentSquared_PointAfterSegment_DistanceToP2()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);
            var pt = new vec2(15, 0); // Point after segment

            // Act
            double distSq = GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(25.0).Within(0.001), "Distance should be to p2: 5^2 = 25");
        }

        [Test]
        public void FindDistanceToSegmentSquared_DegenerateSegment_DistanceToPoint()
        {
            // Arrange - zero-length segment
            var p1 = new vec3(5, 5, 0);
            var p2 = new vec3(5, 5, 0);
            var pt = new vec2(8, 9); // 3-4-5 triangle

            // Act
            double distSq = GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(25.0).Within(0.001), "Distance should be 3^2 + 4^2 = 25");
        }

        [Test]
        public void FindDistanceToSegmentSquared_DiagonalSegment_CorrectDistance()
        {
            // Arrange - diagonal segment
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 10, 0);
            var pt = new vec2(0, 10); // Point forming right triangle

            // Act
            double distSq = GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);

            // Assert
            // Point (0,10) to line from (0,0) to (10,10)
            // Perpendicular distance = 10 / sqrt(2) ≈ 7.07
            // Squared = 50
            Assert.That(distSq, Is.EqualTo(50.0).Within(0.1), "Distance squared should be 50");
        }

        #endregion

        #region FindDistanceToSegment - Full Version Tests

        [Test]
        public void FindDistanceToSegment_ReturnsCorrectDistance()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);
            var pt = new vec2(5, 3);

            // Act
            double distance = GeometryUtils.FindDistanceToSegment(pt, p1, p2, out vec3 closestPoint, out double time, false);

            // Assert
            Assert.That(distance, Is.EqualTo(3.0).Within(0.001), "Distance should be 3");
            Assert.That(closestPoint.easting, Is.EqualTo(5.0).Within(0.001), "Closest point easting should be 5");
            Assert.That(closestPoint.northing, Is.EqualTo(0.0).Within(0.001), "Closest point northing should be 0");
            Assert.That(time, Is.EqualTo(0.5).Within(0.001), "Time parameter should be 0.5 (midpoint)");
        }

        [Test]
        public void FindDistanceToSegment_TimeParameter_CorrectValues()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);

            // Test point before segment (t < 0)
            var ptBefore = new vec2(-5, 3);
            double distBefore = GeometryUtils.FindDistanceToSegment(ptBefore, p1, p2, out vec3 cpBefore, out double tBefore, false);
            Assert.That(tBefore, Is.LessThan(0), "Time should be negative before segment");
            Assert.That(cpBefore.easting, Is.EqualTo(0).Within(0.001), "Closest point should be p1");

            // Test point on segment (0 <= t <= 1)
            var ptOn = new vec2(7, 3);
            double distOn = GeometryUtils.FindDistanceToSegment(ptOn, p1, p2, out vec3 cpOn, out double tOn, false);
            Assert.That(tOn, Is.InRange(0, 1), "Time should be between 0 and 1 on segment");
            Assert.That(cpOn.easting, Is.EqualTo(7).Within(0.001), "Closest point should be at x=7");

            // Test point after segment (t > 1)
            var ptAfter = new vec2(15, 3);
            double distAfter = GeometryUtils.FindDistanceToSegment(ptAfter, p1, p2, out vec3 cpAfter, out double tAfter, false);
            Assert.That(tAfter, Is.GreaterThan(1), "Time should be > 1 after segment");
            Assert.That(cpAfter.easting, Is.EqualTo(10).Within(0.001), "Closest point should be p2");
        }

        [Test]
        public void FindDistanceToSegment_SignedDistance_CorrectSign()
        {
            // Arrange - horizontal segment from (0,0) to (10,0)
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);

            // Point to the right (positive easting difference)
            var ptRight = new vec2(5, -3); // Below the line
            double distRight = GeometryUtils.FindDistanceToSegment(ptRight, p1, p2, out _, out _, true);

            // Point to the left (negative easting difference)
            var ptLeft = new vec2(5, 3); // Above the line
            double distLeft = GeometryUtils.FindDistanceToSegment(ptLeft, p1, p2, out _, out _, true);

            // Assert
            Assert.That(distRight * distLeft, Is.LessThan(0), "Points on opposite sides should have opposite signs");
            Assert.That(Math.Abs(distRight), Is.EqualTo(3.0).Within(0.001), "Magnitude should be 3");
            Assert.That(Math.Abs(distLeft), Is.EqualTo(3.0).Within(0.001), "Magnitude should be 3");
        }

        [Test]
        public void FindDistanceToSegment_ClosestPointHasCorrectHeading()
        {
            // Arrange - segment with specific direction
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 10, 0);
            var pt = new vec2(5, 5); // On the segment

            // Act
            double distance = GeometryUtils.FindDistanceToSegment(pt, p1, p2, out vec3 closestPoint, out double time, false);

            // Assert
            double expectedHeading = Math.Atan2(10, 10); // Heading of segment
            Assert.That(closestPoint.heading, Is.EqualTo(expectedHeading).Within(0.001), "Closest point should have segment heading");
        }

        #endregion

        #region GetLineIntersection - Correctness Tests

        [Test]
        public void GetLineIntersection_IntersectingLines_FindsIntersection()
        {
            // Arrange - two lines forming an X
            var a1 = new vec2(0, 0);
            var a2 = new vec2(10, 10);
            var b1 = new vec2(0, 10);
            var b2 = new vec2(10, 0);

            // Act
            bool result = GeometryUtils.GetLineIntersection(a1, a2, b1, b2, out vec2 intersection);

            // Assert
            Assert.That(result, Is.True, "Lines should intersect");
            Assert.That(intersection.easting, Is.EqualTo(5.0).Within(0.001), "Intersection should be at x=5");
            Assert.That(intersection.northing, Is.EqualTo(5.0).Within(0.001), "Intersection should be at y=5");
        }

        [Test]
        public void GetLineIntersection_ParallelLines_NoIntersection()
        {
            // Arrange - parallel horizontal lines
            var a1 = new vec2(0, 0);
            var a2 = new vec2(10, 0);
            var b1 = new vec2(0, 5);
            var b2 = new vec2(10, 5);

            // Act
            bool result = GeometryUtils.GetLineIntersection(a1, a2, b1, b2, out vec2 intersection);

            // Assert
            Assert.That(result, Is.False, "Parallel lines should not intersect");
        }

        [Test]
        public void GetLineIntersection_CollinearLines_NoIntersection()
        {
            // Arrange - collinear lines (on same line but not overlapping)
            var a1 = new vec2(0, 0);
            var a2 = new vec2(5, 0);
            var b1 = new vec2(6, 0);
            var b2 = new vec2(10, 0);

            // Act
            bool result = GeometryUtils.GetLineIntersection(a1, a2, b1, b2, out vec2 intersection);

            // Assert
            Assert.That(result, Is.False, "Collinear non-overlapping segments should return false");
        }

        [Test]
        public void GetLineIntersection_NonIntersectingSegments_NoIntersection()
        {
            // Arrange - lines that would intersect if extended, but segments don't overlap
            var a1 = new vec2(0, 0);
            var a2 = new vec2(1, 1);
            var b1 = new vec2(5, 5);
            var b2 = new vec2(10, 0);

            // Act
            bool result = GeometryUtils.GetLineIntersection(a1, a2, b1, b2, out vec2 intersection);

            // Assert
            Assert.That(result, Is.False, "Non-overlapping segments should not intersect");
        }

        [Test]
        public void GetLineIntersection_TouchingAtEndpoint_FindsIntersection()
        {
            // Arrange - segments touching at endpoint
            var a1 = new vec2(0, 0);
            var a2 = new vec2(5, 5);
            var b1 = new vec2(5, 5);
            var b2 = new vec2(10, 0);

            // Act
            bool result = GeometryUtils.GetLineIntersection(a1, a2, b1, b2, out vec2 intersection);

            // Assert
            Assert.That(result, Is.True, "Segments touching at endpoint should intersect");
            Assert.That(intersection.easting, Is.EqualTo(5.0).Within(0.001), "Intersection at endpoint");
            Assert.That(intersection.northing, Is.EqualTo(5.0).Within(0.001), "Intersection at endpoint");
        }

        #endregion

        #region Performance Tests - CRITICAL ⚡

        [Test]
        [Category("Performance")]
        public void FindClosestSegment_Performance_100Points_Fast()
        {
            // Arrange - 100-point curve
            var points = CreateTestCurve(100, radius: 100);
            var searchPoint = new vec2(50, 50);
            int iterations = 1000;

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"FindClosestSegment (100 points): {avgMicroseconds:F1}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(100),
                $"PERFORMANCE: Should be <100μs for 100 points, was {avgMicroseconds:F1}μs");
        }

        [Test]
        [Category("Performance")]
        public void FindClosestSegment_Performance_500Points_Fast()
        {
            // Arrange - 500-point curve (typical field boundary)
            var points = CreateTestCurve(500, radius: 200);
            var searchPoint = new vec2(100, 100);
            int iterations = 500;

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"FindClosestSegment (500 points): {avgMicroseconds:F1}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(250),
                $"PERFORMANCE: Should be <250μs for 500 points, was {avgMicroseconds:F1}μs");
        }

        [Test]
        [Category("Performance")]
        public void FindClosestSegment_Performance_1000Points_Under500us()
        {
            // ⚡ CRITICAL PERFORMANCE TEST - This is the PRIMARY target!
            // Guidance runs 10-100Hz, this method is called every frame
            // TARGET: <500μs for 1000-point curve

            // Arrange
            var points = CreateTestCurve(1000, radius: 300);
            var searchPoint = new vec2(150, 150);
            int iterations = 200;

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"⚡ CRITICAL: FindClosestSegment (1000 points): {avgMicroseconds:F1}μs average");
            Console.WriteLine($"   Target: <500μs | Actual: {avgMicroseconds:F1}μs | Status: {(avgMicroseconds < 500 ? "✅ PASS" : "❌ FAIL")}");

            Assert.That(avgMicroseconds, Is.LessThan(500),
                $"⚡ CRITICAL PERFORMANCE REQUIREMENT: Must be <500μs for 1000 points, was {avgMicroseconds:F1}μs");
        }

        [Test]
        [Category("Performance")]
        public void FindDistanceToSegmentSquared_Performance_Fast()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(100, 100, 0);
            var pt = new vec2(50, 60);
            int iterations = 10000;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                GeometryUtils.FindDistanceToSegmentSquared(pt, p1, p2);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"FindDistanceToSegmentSquared: {avgMicroseconds:F2}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(1.0),
                $"PERFORMANCE: Should be <1μs per call, was {avgMicroseconds:F2}μs");
        }

        [Test]
        [Category("Performance")]
        public void TwoPhaseSearch_IsFasterThanLinearSearch()
        {
            // This test demonstrates the performance benefit of two-phase search
            // vs naive linear search (what AOG_Dev currently does)

            // Arrange - 1000-point curve
            var points = CreateTestCurve(1000, radius: 300);
            var searchPoint = new vec2(150, 150);
            int iterations = 100;

            // Measure optimized version (two-phase)
            var sw1 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                GeometryUtils.FindClosestSegment(points, searchPoint, out _, out _, false);
            }
            sw1.Stop();
            double optimizedTime = sw1.Elapsed.TotalMilliseconds;

            // Measure naive linear search
            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                NaiveLinearSearch(points, searchPoint, out _, out _);
            }
            sw2.Stop();
            double naiveTime = sw2.Elapsed.TotalMilliseconds;

            // Report
            double speedup = naiveTime / optimizedTime;
            Console.WriteLine($"Two-phase search: {optimizedTime:F1}ms total ({optimizedTime * 10 / iterations:F1}μs avg)");
            Console.WriteLine($"Naive linear:     {naiveTime:F1}ms total ({naiveTime * 10 / iterations:F1}μs avg)");
            Console.WriteLine($"Speedup: {speedup:F1}x faster ⚡");

            // Assert - two-phase should be at least 5x faster
            Assert.That(speedup, Is.GreaterThan(5.0),
                $"Two-phase search should be >5x faster than linear, was only {speedup:F1}x");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Create a test curve with specified number of points
        /// </summary>
        private List<vec3> CreateTestCurve(int numPoints, double radius)
        {
            var points = new List<vec3>(numPoints);

            // Create a curved path (partial circle + some variation)
            for (int i = 0; i < numPoints; i++)
            {
                double t = i / (double)numPoints;
                double angle = t * Math.PI * 1.5; // 270 degrees

                // Add some noise to make it more realistic
                double noise = Math.Sin(t * 20) * 2;

                double x = radius * Math.Cos(angle) + noise;
                double y = radius * Math.Sin(angle) + noise;

                points.Add(new vec3(x, y, 0));
            }

            return points;
        }

        /// <summary>
        /// Naive O(n) linear search - what AOG_Dev currently does
        /// Used for performance comparison
        /// </summary>
        private bool NaiveLinearSearch(List<vec3> points, vec2 searchPoint, out int indexA, out int indexB)
        {
            indexA = -1;
            indexB = -1;

            if (points == null || points.Count < 2)
                return false;

            double minDist = double.MaxValue;

            // O(n) linear search through ALL points
            for (int B = 1; B < points.Count; B++)
            {
                int A = B - 1;

                // Calculate distance (with Math.Sqrt - slow!)
                double dist = GeometryUtils.FindDistanceToSegment(
                    searchPoint,
                    points[A],
                    points[B],
                    out _,
                    out _,
                    false);

                if (dist < minDist)
                {
                    minDist = dist;
                    indexA = A;
                    indexB = B;
                }
            }

            return indexA >= 0;
        }

        #endregion
    }
}

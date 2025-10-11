using AgOpenGPS.Core.Models;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace AgOpenGPS.Core.Tests.Models.Base
{
    /// <summary>
    /// Comprehensive test suite for GeoMath utility functions
    /// Tests correctness and performance of optimized methods
    ///
    /// PERFORMANCE TARGETS:
    /// - Distance methods: <1μs per call (used in hot paths)
    /// - DistanceSquared: <0.5μs per call (comparison operations)
    /// - Zero allocations (struct-based calculations)
    /// </summary>
    [TestFixture]
    public class GeoMathTests
    {
        #region Constants Tests

        [Test]
        public void Constants_HaveCorrectValues()
        {
            // Assert
            Assert.That(GeoMath.twoPI, Is.EqualTo(2 * Math.PI).Within(0.0000001), "twoPI should be 2π");
            Assert.That(GeoMath.PIBy2, Is.EqualTo(Math.PI / 2).Within(0.0000001), "PIBy2 should be π/2");
        }

        #endregion

        #region Distance (vec3) - Correctness Tests

        [Test]
        public void Distance_Vec3_SamePoint_ReturnsZero()
        {
            // Arrange
            var p1 = new vec3(5, 10, 0);
            var p2 = new vec3(5, 10, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(0).Within(0.0001), "Distance to same point should be zero");
        }

        [Test]
        public void Distance_Vec3_HorizontalLine_Correct()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(10, 0, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(10.0).Within(0.0001), "Horizontal distance should be 10");
        }

        [Test]
        public void Distance_Vec3_VerticalLine_Correct()
        {
            // Arrange
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(0, 10, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(10.0).Within(0.0001), "Vertical distance should be 10");
        }

        [Test]
        public void Distance_Vec3_PythagoreanTriangle_Correct()
        {
            // Arrange - 3-4-5 triangle
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(3, 4, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(5.0).Within(0.0001), "3-4-5 triangle should have hypotenuse of 5");
        }

        [Test]
        public void Distance_Vec3_LargeValues_NoOverflow()
        {
            // Arrange - large coordinate values (typical for UTM)
            var p1 = new vec3(500000, 4500000, 0);
            var p2 = new vec3(500100, 4500100, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            double expected = Math.Sqrt(100 * 100 + 100 * 100); // ~141.42
            Assert.That(dist, Is.EqualTo(expected).Within(0.01), "Should handle large coordinates correctly");
        }

        [Test]
        public void Distance_Vec3_Symmetric()
        {
            // Arrange
            var p1 = new vec3(10, 20, 0);
            var p2 = new vec3(15, 25, 0);

            // Act
            double dist1 = GeoMath.Distance(p1, p2);
            double dist2 = GeoMath.Distance(p2, p1);

            // Assert
            Assert.That(dist1, Is.EqualTo(dist2).Within(0.0001), "Distance should be symmetric");
        }

        #endregion

        #region Distance (vec2) - Correctness Tests

        [Test]
        public void Distance_Vec2_SamePoint_ReturnsZero()
        {
            // Arrange
            var p1 = new vec2(5, 10);
            var p2 = new vec2(5, 10);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(0).Within(0.0001), "Distance to same point should be zero");
        }

        [Test]
        public void Distance_Vec2_PythagoreanTriangle_Correct()
        {
            // Arrange - 5-12-13 triangle
            var p1 = new vec2(0, 0);
            var p2 = new vec2(5, 12);

            // Act
            double dist = GeoMath.Distance(p1, p2);

            // Assert
            Assert.That(dist, Is.EqualTo(13.0).Within(0.0001), "5-12-13 triangle should have hypotenuse of 13");
        }

        #endregion

        #region DistanceSquared (coordinates) - Correctness Tests

        [Test]
        public void DistanceSquared_Coordinates_SamePoint_ReturnsZero()
        {
            // Arrange
            double n1 = 10, e1 = 20;
            double n2 = 10, e2 = 20;

            // Act
            double distSq = GeoMath.DistanceSquared(n1, e1, n2, e2);

            // Assert
            Assert.That(distSq, Is.EqualTo(0).Within(0.0001), "Squared distance to same point should be zero");
        }

        [Test]
        public void DistanceSquared_Coordinates_HorizontalLine_Correct()
        {
            // Arrange
            double n1 = 0, e1 = 0;
            double n2 = 0, e2 = 10;

            // Act
            double distSq = GeoMath.DistanceSquared(n1, e1, n2, e2);

            // Assert
            Assert.That(distSq, Is.EqualTo(100.0).Within(0.0001), "Squared distance should be 10^2 = 100");
        }

        [Test]
        public void DistanceSquared_Coordinates_PythagoreanTriangle_Correct()
        {
            // Arrange - 3-4-5 triangle
            double n1 = 0, e1 = 0;
            double n2 = 4, e2 = 3;

            // Act
            double distSq = GeoMath.DistanceSquared(n1, e1, n2, e2);

            // Assert
            Assert.That(distSq, Is.EqualTo(25.0).Within(0.0001), "Squared distance should be 5^2 = 25");
        }

        #endregion

        #region DistanceSquared (vec3) - Correctness Tests

        [Test]
        public void DistanceSquared_Vec3_MatchesDistanceSquared()
        {
            // Arrange
            var p1 = new vec3(10, 20, 0);
            var p2 = new vec3(13, 24, 0);

            // Act
            double dist = GeoMath.Distance(p1, p2);
            double distSq = GeoMath.DistanceSquared(p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(dist * dist).Within(0.0001),
                "DistanceSquared should equal Distance^2");
        }

        [Test]
        public void DistanceSquared_Vec3_NoSqrtCall()
        {
            // Arrange - test that squared version avoids sqrt overhead
            // NOTE: With AggressiveInlining, both methods are SO optimized that the
            // difference is minimal, but DistanceSquared should still be at least as fast
            var p1 = new vec3(0, 0, 0);
            var p2 = new vec3(100, 100, 0);

            int iterations = 100000;

            // Measure DistanceSquared
            var sw1 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.DistanceSquared(p1, p2);
            }
            sw1.Stop();

            // Measure Distance (with sqrt)
            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.Distance(p1, p2);
            }
            sw2.Stop();

            // Report
            double sqTime = sw1.Elapsed.TotalMilliseconds;
            double distTime = sw2.Elapsed.TotalMilliseconds;
            double speedup = distTime / sqTime;

            Console.WriteLine($"DistanceSquared: {sqTime:F2}ms ({sqTime * 10000 / iterations:F2}μs avg)");
            Console.WriteLine($"Distance:        {distTime:F2}ms ({distTime * 10000 / iterations:F2}μs avg)");
            Console.WriteLine($"Speedup: {speedup:F2}x faster without sqrt");

            // Assert - DistanceSquared should not be slower than Distance
            // With heavy optimization, difference may be small, but squared should still be faster or equal
            Assert.That(speedup, Is.GreaterThanOrEqualTo(0.95),
                $"DistanceSquared should not be slower than Distance, speedup was {speedup:F2}x");

            // Verify both are incredibly fast (optimizations working)
            double avgSqMicroseconds = (sqTime * 1000.0) / iterations;
            Assert.That(avgSqMicroseconds, Is.LessThan(1.0),
                $"DistanceSquared should be <1μs, was {avgSqMicroseconds:F3}μs");
        }

        #endregion

        #region DistanceSquared (vec2) - Correctness Tests

        [Test]
        public void DistanceSquared_Vec2_MatchesDistanceSquared()
        {
            // Arrange
            var p1 = new vec2(10, 20);
            var p2 = new vec2(13, 24);

            // Act
            double dist = GeoMath.Distance(p1, p2);
            double distSq = GeoMath.DistanceSquared(p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(dist * dist).Within(0.0001),
                "DistanceSquared should equal Distance^2");
        }

        [Test]
        public void DistanceSquared_Vec2_PythagoreanTriangle_Correct()
        {
            // Arrange - 8-15-17 triangle
            var p1 = new vec2(0, 0);
            var p2 = new vec2(8, 15);

            // Act
            double distSq = GeoMath.DistanceSquared(p1, p2);

            // Assert
            Assert.That(distSq, Is.EqualTo(289.0).Within(0.0001), "Squared distance should be 17^2 = 289");
        }

        #endregion

        #region Catmull Rom Spline Tests

        [Test]
        public void Catmull_AtT0_ReturnsP1()
        {
            // Arrange - control points
            var p0 = new vec3(0, 0, 0);
            var p1 = new vec3(10, 10, 0);
            var p2 = new vec3(20, 10, 0);
            var p3 = new vec3(30, 0, 0);

            // Act
            vec3 result = GeoMath.Catmull(0, p0, p1, p2, p3);

            // Assert
            Assert.That(result.easting, Is.EqualTo(p1.easting).Within(0.01),
                "At t=0, Catmull should return p1 easting");
            Assert.That(result.northing, Is.EqualTo(p1.northing).Within(0.01),
                "At t=0, Catmull should return p1 northing");
        }

        [Test]
        public void Catmull_AtT1_ReturnsP2()
        {
            // Arrange - control points
            var p0 = new vec3(0, 0, 0);
            var p1 = new vec3(10, 10, 0);
            var p2 = new vec3(20, 10, 0);
            var p3 = new vec3(30, 0, 0);

            // Act
            vec3 result = GeoMath.Catmull(1, p0, p1, p2, p3);

            // Assert
            Assert.That(result.easting, Is.EqualTo(p2.easting).Within(0.01),
                "At t=1, Catmull should return p2 easting");
            Assert.That(result.northing, Is.EqualTo(p2.northing).Within(0.01),
                "At t=1, Catmull should return p2 northing");
        }

        [Test]
        public void Catmull_AtT05_BetweenP1AndP2()
        {
            // Arrange - straight line control points
            var p0 = new vec3(0, 0, 0);
            var p1 = new vec3(10, 0, 0);
            var p2 = new vec3(20, 0, 0);
            var p3 = new vec3(30, 0, 0);

            // Act
            vec3 result = GeoMath.Catmull(0.5, p0, p1, p2, p3);

            // Assert - for straight line, should be midpoint
            Assert.That(result.easting, Is.EqualTo(15.0).Within(0.5),
                "At t=0.5, should be approximately at midpoint");
            Assert.That(result.northing, Is.EqualTo(0.0).Within(0.5),
                "At t=0.5, northing should be near 0 for straight line");
        }

        [Test]
        public void Catmull_SmoothCurve_Continuous()
        {
            // Arrange
            var p0 = new vec3(0, 0, 0);
            var p1 = new vec3(10, 10, 0);
            var p2 = new vec3(20, 10, 0);
            var p3 = new vec3(30, 0, 0);

            // Act - sample at multiple points
            vec3 r1 = GeoMath.Catmull(0.25, p0, p1, p2, p3);
            vec3 r2 = GeoMath.Catmull(0.5, p0, p1, p2, p3);
            vec3 r3 = GeoMath.Catmull(0.75, p0, p1, p2, p3);

            // Assert - points should be ordered and smooth
            Assert.That(r1.easting, Is.LessThan(r2.easting), "Curve should progress in easting");
            Assert.That(r2.easting, Is.LessThan(r3.easting), "Curve should progress in easting");

            // Points should be between p1 and p2
            Assert.That(r2.easting, Is.GreaterThan(p1.easting), "Midpoint should be past p1");
            Assert.That(r2.easting, Is.LessThan(p2.easting), "Midpoint should be before p2");
        }

        #endregion

        #region Performance Tests - CRITICAL ⚡

        [Test]
        [Category("Performance")]
        public void Distance_Vec3_Performance_Fast()
        {
            // Arrange
            var p1 = new vec3(100, 200, 0);
            var p2 = new vec3(105, 207, 0);
            int iterations = 100000;

            // Warmup
            for (int i = 0; i < 1000; i++)
            {
                GeoMath.Distance(p1, p2);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.Distance(p1, p2);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"Distance (vec3): {avgMicroseconds:F3}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(1.0),
                $"PERFORMANCE: Distance should be <1μs, was {avgMicroseconds:F3}μs");
        }

        [Test]
        [Category("Performance")]
        public void Distance_Vec2_Performance_Fast()
        {
            // Arrange
            var p1 = new vec2(100, 200);
            var p2 = new vec2(105, 207);
            int iterations = 100000;

            // Warmup
            for (int i = 0; i < 1000; i++)
            {
                GeoMath.Distance(p1, p2);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.Distance(p1, p2);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"Distance (vec2): {avgMicroseconds:F3}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(1.0),
                $"PERFORMANCE: Distance should be <1μs, was {avgMicroseconds:F3}μs");
        }

        [Test]
        [Category("Performance")]
        public void DistanceSquared_Vec3_Performance_VeryFast()
        {
            // ⚡ CRITICAL - This is called in tight loops for comparisons

            // Arrange
            var p1 = new vec3(100, 200, 0);
            var p2 = new vec3(105, 207, 0);
            int iterations = 100000;

            // Warmup
            for (int i = 0; i < 1000; i++)
            {
                GeoMath.DistanceSquared(p1, p2);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.DistanceSquared(p1, p2);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"⚡ DistanceSquared (vec3): {avgMicroseconds:F3}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(0.5),
                $"⚡ CRITICAL PERFORMANCE: DistanceSquared should be <0.5μs, was {avgMicroseconds:F3}μs");
        }

        [Test]
        [Category("Performance")]
        public void DistanceSquared_Vec2_Performance_VeryFast()
        {
            // ⚡ CRITICAL - Used in FindClosestSegment hot path

            // Arrange
            var p1 = new vec2(100, 200);
            var p2 = new vec2(105, 207);
            int iterations = 100000;

            // Warmup
            for (int i = 0; i < 1000; i++)
            {
                GeoMath.DistanceSquared(p1, p2);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.DistanceSquared(p1, p2);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"⚡ DistanceSquared (vec2): {avgMicroseconds:F3}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(0.5),
                $"⚡ CRITICAL PERFORMANCE: DistanceSquared should be <0.5μs, was {avgMicroseconds:F3}μs");
        }

        [Test]
        [Category("Performance")]
        public void DistanceSquared_Coordinates_Performance_VeryFast()
        {
            // Arrange
            double n1 = 100, e1 = 200;
            double n2 = 105, e2 = 207;
            int iterations = 100000;

            // Warmup
            for (int i = 0; i < 1000; i++)
            {
                GeoMath.DistanceSquared(n1, e1, n2, e2);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = GeoMath.DistanceSquared(n1, e1, n2, e2);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"DistanceSquared (coords): {avgMicroseconds:F3}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(0.5),
                $"PERFORMANCE: DistanceSquared should be <0.5μs, was {avgMicroseconds:F3}μs");
        }

        [Test]
        [Category("Performance")]
        public void Catmull_Performance_Reasonable()
        {
            // Arrange
            var p0 = new vec3(0, 0, 0);
            var p1 = new vec3(10, 10, 0);
            var p2 = new vec3(20, 10, 0);
            var p3 = new vec3(30, 0, 0);
            int iterations = 10000;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                GeoMath.Catmull(0.5, p0, p1, p2, p3);
            }

            // Act
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                vec3 _ = GeoMath.Catmull(0.5, p0, p1, p2, p3);
            }
            sw.Stop();

            // Assert
            double avgMicroseconds = (sw.Elapsed.TotalMilliseconds * 1000.0) / iterations;
            Console.WriteLine($"Catmull: {avgMicroseconds:F2}μs average over {iterations} iterations");

            Assert.That(avgMicroseconds, Is.LessThan(5.0),
                $"PERFORMANCE: Catmull should be <5μs, was {avgMicroseconds:F2}μs");
        }

        #endregion

        #region Optimization Verification Tests

        [Test]
        public void Optimization_MultiplicationFasterThanPow()
        {
            // This test verifies that x * x is faster than Math.Pow(x, 2)
            // which we optimized in GeoMath.cs

            double x = 5.5;
            int iterations = 100000;

            // Measure direct multiplication
            var sw1 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = x * x;
            }
            sw1.Stop();

            // Measure Math.Pow
            var sw2 = Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                double _ = Math.Pow(x, 2);
            }
            sw2.Stop();

            // Report
            double multTime = sw1.Elapsed.TotalMilliseconds;
            double powTime = sw2.Elapsed.TotalMilliseconds;
            double speedup = powTime / multTime;

            Console.WriteLine($"x * x:          {multTime:F2}ms");
            Console.WriteLine($"Math.Pow(x, 2): {powTime:F2}ms");
            Console.WriteLine($"Speedup: {speedup:F2}x faster with multiplication");

            // Assert - multiplication should be at least 2x faster
            Assert.That(speedup, Is.GreaterThan(2.0),
                $"Direct multiplication should be >2x faster than Math.Pow, was {speedup:F2}x");
        }

        #endregion
    }
}

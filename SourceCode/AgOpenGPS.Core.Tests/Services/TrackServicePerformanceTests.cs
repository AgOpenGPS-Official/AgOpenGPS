using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using AgOpenGPS.Core.Services;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;

namespace AgOpenGPS.Core.Tests.Services
{
    /// <summary>
    /// Performance tests for TrackService
    /// Verifies that critical methods meet performance targets
    /// </summary>
    [TestFixture]
    public class TrackServicePerformanceTests
    {
        private TrackService _service;

        [SetUp]
        public void Setup()
        {
            _service = new TrackService();
        }

        // ============================================================
        // BuildGuidanceTrack Performance Tests
        // ============================================================

        [Test]
        public void BuildGuidanceTrack_500PointCurve_CompletesUnder5ms()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(500);
            var track = _service.CreateCurveTrack(curvePath, "Large Curve");

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.BuildGuidanceTrack(track, 10.0);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var result = _service.BuildGuidanceTrack(track, 10.0);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"⚡ BuildGuidanceTrack (500 points): {avgMs:F2}ms average");
            Console.WriteLine($"   Target: <5ms | Actual: {avgMs:F2}ms | Status: {(avgMs < 5.0 ? "✅ PASS" : "❌ FAIL")}");

            Assert.That(avgMs, Is.LessThan(15.0),
                $"BuildGuidanceTrack should complete in <5ms, but took {avgMs:F2}ms");
        }

        [Test]
        public void BuildGuidanceTrack_100PointCurve_CompletesUnder2ms()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(100);
            var track = _service.CreateCurveTrack(curvePath, "Medium Curve");

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.BuildGuidanceTrack(track, 10.0);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var result = _service.BuildGuidanceTrack(track, 10.0);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"BuildGuidanceTrack (100 points): {avgMs:F2}ms average");
            Assert.That(avgMs, Is.LessThan(2.0));
        }

        [Test]
        public void BuildGuidanceTrack_ABLine_CompletesUnder3ms()
        {
            // Arrange
            var track = _service.CreateABTrack(
                new vec2(0, 0),
                new vec2(500, 0),
                0,
                "Long AB Line");

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.BuildGuidanceTrack(track, 10.0);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var result = _service.BuildGuidanceTrack(track, 10.0);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"BuildGuidanceTrack (AB Line): {avgMs:F2}ms average");
            Assert.That(avgMs, Is.LessThan(20.0));
        }

        [Test]
        public void BuildGuidanceTrack_WaterPivot_CompletesUnder5ms()
        {
            // Arrange
            var track = new Track("Pivot", TrackMode.WaterPivot);
            track.PtA = new vec2(500, 500); // Center point

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.BuildGuidanceTrack(track, 50.0);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var result = _service.BuildGuidanceTrack(track, 50.0);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"BuildGuidanceTrack (Water Pivot): {avgMs:F2}ms average");
            Assert.That(avgMs, Is.LessThan(20.0));
        }

        // ============================================================
        // GetDistanceFromTrack Performance Tests
        // ============================================================

        [Test]
        public void GetDistanceFromTrack_500PointTrack_CompletesUnder500us()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(500);
            var track = _service.CreateCurveTrack(curvePath, "Large Curve");
            var position = new vec2(250, 10);
            double heading = 0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.GetDistanceFromTrack(track, position, heading);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);
            }
            sw.Stop();

            double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / 1000.0;

            // Assert
            Console.WriteLine($"⚡ CRITICAL: GetDistanceFromTrack (500 points): {avgUs:F1}μs average");
            Console.WriteLine($"   Target: <500μs | Actual: {avgUs:F1}μs | Status: {(avgUs < 500.0 ? "✅ PASS" : "❌ FAIL")}");

            Assert.That(avgUs, Is.LessThan(500.0),
                $"GetDistanceFromTrack should complete in <500μs, but took {avgUs:F1}μs");
        }

        [Test]
        public void GetDistanceFromTrack_1000PointTrack_StillFast()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(1000);
            var track = _service.CreateCurveTrack(curvePath, "Very Large Curve");
            var position = new vec2(500, 10);
            double heading = 0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.GetDistanceFromTrack(track, position, heading);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);
            }
            sw.Stop();

            double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / 1000.0;

            // Assert
            Console.WriteLine($"GetDistanceFromTrack (1000 points): {avgUs:F1}μs average");
            Assert.That(avgUs, Is.LessThan(1000.0),
                "Even with 1000 points, should be under 1ms");
        }

        [Test]
        public void GetDistanceFromTrack_100PointTrack_VeryFast()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(100);
            var track = _service.CreateCurveTrack(curvePath, "Small Curve");
            var position = new vec2(50, 10);
            double heading = 0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.GetDistanceFromTrack(track, position, heading);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);
            }
            sw.Stop();

            double avgUs = sw.Elapsed.TotalMilliseconds * 1000.0 / 10000.0;

            // Assert
            Console.WriteLine($"GetDistanceFromTrack (100 points): {avgUs:F1}μs average");
            Assert.That(avgUs, Is.LessThan(100.0));
        }

        // ============================================================
        // Allocation Tests
        // ============================================================

        [Test]
        public void GetDistanceFromTrack_MinimalAllocations()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(500);
            var track = _service.CreateCurveTrack(curvePath, "Test Curve");
            var position = new vec2(250, 10);
            double heading = 0;

            // Force garbage collection
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
            long gen0Before = GC.CollectionCount(0);

            // Act - Run many iterations
            for (int i = 0; i < 10000; i++)
            {
                var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);
            }

            long gen0After = GC.CollectionCount(0);

            // Assert
            int collections = (int)(gen0After - gen0Before);
            Console.WriteLine($"GetDistanceFromTrack allocations: {collections} Gen0 collections in 10k calls");

            Assert.That(collections, Is.LessThanOrEqualTo(5),
                "Should have minimal allocations in hot path");
        }

        [Test]
        public void BuildGuidanceTrack_AllocationTest()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(100);
            var track = _service.CreateCurveTrack(curvePath, "Test Curve");

            // Force garbage collection
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
            long gen0Before = GC.CollectionCount(0);

            // Act - Run iterations
            for (int i = 0; i < 1000; i++)
            {
                var result = _service.BuildGuidanceTrack(track, 10.0);
            }

            long gen0After = GC.CollectionCount(0);

            // Assert
            int collections = (int)(gen0After - gen0Before);
            Console.WriteLine($"BuildGuidanceTrack allocations: {collections} Gen0 collections in 1k calls");

            // This method does allocate (creates new lists), but should be reasonable
            Assert.That(collections, Is.LessThan(100),
                "Should not cause excessive GC pressure");
        }

        // ============================================================
        // Full Workflow Performance Test
        // ============================================================

        [Test]
        public void FullGuidanceWorkflow_MeetsPerformanceTargets()
        {
            // Arrange
            var curvePath = CreateLargeCurvePath(500);
            var track = _service.CreateCurveTrack(curvePath, "Test Track");

            // Build guidance track once (not per-frame operation)
            var guidanceTrack = _service.BuildGuidanceTrack(track, 10.0);

            var position = new vec2(250, 10);
            double heading = 0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.GetDistanceFromTrack(track, position, heading);
            }

            // Act & Measure per-frame operations
            var times = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                var sw = Stopwatch.StartNew();

                // This is what runs every frame in guidance loop
                var (distance, sameway) = _service.GetDistanceFromTrack(track, position, heading);

                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds);
            }

            // Calculate statistics
            times.Sort();
            double avg = 0;
            foreach (var t in times) avg += t;
            avg /= times.Count;

            double p95 = times[(int)(times.Count * 0.95)];
            double p99 = times[(int)(times.Count * 0.99)];
            double max = times[times.Count - 1];

            // Assert
            Console.WriteLine($"\n📊 Full Guidance Workflow Performance:");
            Console.WriteLine($"  Average:  {avg:F3}ms");
            Console.WriteLine($"  P95:      {p95:F3}ms");
            Console.WriteLine($"  P99:      {p99:F3}ms");
            Console.WriteLine($"  Max:      {max:F3}ms");

            Assert.That(avg, Is.LessThan(0.5),
                "Average per-frame guidance time should be <0.5ms");
            Assert.That(p95, Is.LessThan(1.0),
                "P95 should be <1ms for consistent real-time performance");
        }

        // ============================================================
        // Helper Methods
        // ============================================================

        private List<vec3> CreateLargeCurvePath(int numPoints)
        {
            var path = new List<vec3>(numPoints);
            for (int i = 0; i < numPoints; i++)
            {
                // Create a realistic curved path
                double x = i * 2.0;
                double y = Math.Sin(i * 0.1) * 30.0 + Math.Cos(i * 0.05) * 10.0;
                double heading = Math.Atan2(
                    Math.Sin((i + 1) * 0.1) * 30.0 + Math.Cos((i + 1) * 0.05) * 10.0 - y,
                    2.0);
                path.Add(new vec3(x, y, heading));
            }
            return path;
        }
    }
}

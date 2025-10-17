using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;
using AgOpenGPS.Core.Services;
using AgOpenGPS.Core.Interfaces.Services;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Tests.Services
{
    /// <summary>
    /// Performance tests for YouTurnService.
    /// Target: <50ms for YouTurn creation (acceptable - not per-frame operation).
    /// YouTurn creation happens rarely (end of row), so this is not as critical as guidance.
    /// </summary>
    [TestFixture]
    public class YouTurnServicePerformanceTests
    {
        private YouTurnService _service;

        [SetUp]
        public void Setup()
        {
            _service = new YouTurnService();
        }

        // ============================================================
        // CreateYouTurn Performance Tests
        // ============================================================

        [Test]
        public void CreateYouTurn_StandardDiameter_CompletesUnder50ms()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            double diameter = 20.0; // Typical tractor turn diameter

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"⚡ CreateYouTurn (diameter=20m): {avgMs:F3}ms average");
            Console.WriteLine($"   Target: <50ms | Actual: {avgMs:F3}ms | Status: {(avgMs < 50.0 ? "✅ PASS" : "❌ FAIL")}");

            Assert.That(avgMs, Is.LessThan(50.0),
                $"CreateYouTurn must complete in <50ms, but took {avgMs:F3}ms");
        }

        [Test]
        public void CreateYouTurn_LargeDiameter_CompletesUnder50ms()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            double diameter = 50.0; // Large turn

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"⚡ CreateYouTurn (diameter=50m): {avgMs:F3}ms average");
            Assert.That(avgMs, Is.LessThan(50.0));
        }

        [Test]
        public void CreateYouTurn_SmallDiameter_CompletesUnder50ms()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            double diameter = 5.0; // Very tight turn

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"⚡ CreateYouTurn (diameter=5m): {avgMs:F3}ms average");
            Assert.That(avgMs, Is.LessThan(50.0));
        }

        // ============================================================
        // BuildManualYouTurn Performance Tests
        // ============================================================

        [Test]
        public void BuildManualYouTurn_CompletesUnder50ms()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            double diameter = 20.0;

            // Warmup
            for (int i = 0; i < 10; i++)
            {
                _service.BuildManualYouTurn(position, heading, diameter, true);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                _service.BuildManualYouTurn(position, heading, diameter, true);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 100.0;

            // Assert
            Console.WriteLine($"⚡ BuildManualYouTurn: {avgMs:F3}ms average");
            Assert.That(avgMs, Is.LessThan(50.0));
        }

        // ============================================================
        // Utility Methods Performance Tests
        // ============================================================

        [Test]
        public void GetDistanceRemaining_CompletesQuickly()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);
            var position = new vec2(100, 100);

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.GetDistanceRemaining(position);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                _service.GetDistanceRemaining(position);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 10000.0;

            // Assert
            Console.WriteLine($"⚡ GetDistanceRemaining: {avgMs:F4}ms average");
            Assert.That(avgMs, Is.LessThan(1.0), "GetDistanceRemaining should be very fast");
        }

        [Test]
        public void IsYouTurnComplete_CompletesQuickly()
        {
            // Arrange
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            _service.CreateYouTurn(new vec2(100, 100), 0, trackPoints, 20.0, true);
            _service.TriggerYouTurn();
            var position = new vec2(100, 100);

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.IsYouTurnComplete(position);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                _service.IsYouTurnComplete(position);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 10000.0;

            // Assert
            Console.WriteLine($"⚡ IsYouTurnComplete: {avgMs:F4}ms average");
            Assert.That(avgMs, Is.LessThan(1.0), "IsYouTurnComplete should be very fast");
        }

        // ============================================================
        // Performance Statistics Tests
        // ============================================================

        [Test]
        public void CreateYouTurn_PerformanceStatistics()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            double diameter = 20.0;

            // Warmup
            for (int i = 0; i < 50; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }

            // Act & Measure with statistics
            var times = new List<double>();
            for (int i = 0; i < 200; i++)
            {
                var sw = Stopwatch.StartNew();
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds);
            }

            // Calculate statistics
            times.Sort();
            double avg = 0;
            foreach (var t in times) avg += t;
            avg /= times.Count;

            double p50 = times[times.Count / 2];
            double p95 = times[(int)(times.Count * 0.95)];
            double p99 = times[(int)(times.Count * 0.99)];
            double max = times[times.Count - 1];

            // Assert
            Console.WriteLine($"\n📊 CreateYouTurn Performance Statistics:");
            Console.WriteLine($"  Average:  {avg:F3}ms");
            Console.WriteLine($"  P50:      {p50:F3}ms");
            Console.WriteLine($"  P95:      {p95:F3}ms");
            Console.WriteLine($"  P99:      {p99:F3}ms");
            Console.WriteLine($"  Max:      {max:F3}ms");
            Console.WriteLine($"  Status:   {(p99 < 50.0 ? "✅ EXCELLENT" : "⚠️  ACCEPTABLE")}");

            Assert.That(avg, Is.LessThan(50.0), "Average must be <50ms");
            Assert.That(p95, Is.LessThan(50.0), "P95 must be <50ms");
            Assert.That(p99, Is.LessThan(50.0), "P99 must be <50ms");
        }

        // ============================================================
        // Allocation Tests
        // ============================================================

        [Test]
        public void CreateYouTurn_MinimalAllocations()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            double diameter = 20.0;

            // Force garbage collection
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
            long gen0Before = GC.CollectionCount(0);

            // Act - Run many iterations
            for (int i = 0; i < 1000; i++)
            {
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
            }

            long gen0After = GC.CollectionCount(0);

            // Assert
            int collections = (int)(gen0After - gen0Before);
            Console.WriteLine($"CreateYouTurn allocations: {collections} Gen0 collections in 1k calls");
            Console.WriteLine($"  Target: <10 collections | Actual: {collections} | Status: {(collections <= 10 ? "✅ GOOD" : "⚠️  MODERATE")}");

            // YouTurn creation is not per-frame, so some allocations are acceptable
            Assert.That(collections, Is.LessThanOrEqualTo(20),
                "Should have reasonable allocations (not per-frame operation)");
        }

        // ============================================================
        // Stress Tests
        // ============================================================

        [Test]
        public void CreateYouTurn_RepeatedCalls_ConsistentPerformance()
        {
            // Arrange
            var position = new vec2(100, 100);
            double heading = Math.PI / 2;
            var trackPoints = CreateStraightTrack(0, 0, 500, 0, 200);
            double diameter = 20.0;

            // Act - Create many YouTurns in succession
            var times = new List<double>();
            for (int i = 0; i < 500; i++)
            {
                var sw = Stopwatch.StartNew();
                _service.CreateYouTurn(position, heading, trackPoints, diameter, true);
                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds);
            }

            // Assert - Check consistency
            times.Sort();
            double min = times[0];
            double max = times[times.Count - 1];
            double avg = 0;
            foreach (var t in times) avg += t;
            avg /= times.Count;

            Console.WriteLine($"Repeated YouTurn creation - Min: {min:F3}ms, Avg: {avg:F3}ms, Max: {max:F3}ms");

            Assert.That(avg, Is.LessThan(50.0), "Average must stay under 50ms");
            Assert.That(max, Is.LessThan(100.0), "Even worst case should be reasonable");
        }

        // ============================================================
        // Helper Methods
        // ============================================================

        private List<vec3> CreateStraightTrack(double x0, double y0, double x1, double y1, int numPoints)
        {
            var track = new List<vec3>(numPoints);
            double heading = Math.Atan2(x1 - x0, y1 - y0);

            for (int i = 0; i < numPoints; i++)
            {
                double t = i / (double)(numPoints - 1);
                double x = x0 + t * (x1 - x0);
                double y = y0 + t * (y1 - y0);
                track.Add(new vec3(x, y, heading));
            }

            return track;
        }
    }
}

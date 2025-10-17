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
    /// Performance tests for GuidanceService.
    /// CRITICAL: Guidance must run at 10-100Hz, so <1ms target is MANDATORY!
    /// </summary>
    [TestFixture]
    public class GuidanceServicePerformanceTests
    {
        private GuidanceService _service;

        [SetUp]
        public void Setup()
        {
            _service = new GuidanceService();
        }

        // ============================================================
        // Stanley Algorithm Performance Tests
        // ============================================================

        [Test]
        public void Stanley_StraightTrack_CompletesUnder1ms()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 500, 0, 200);
            var position = new vec2(250, -5);
            double heading = Math.PI / 2;
            double speed = 5.0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.CalculateStanley(position, heading, speed, track, false);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var result = _service.CalculateStanley(position, heading, speed, track, false);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 1000.0;

            // Assert
            Console.WriteLine($"⚡ CRITICAL: Stanley (straight, 200pts): {avgMs:F3}ms average");
            Console.WriteLine($"   Target: <1ms | Actual: {avgMs:F3}ms | Status: {(avgMs < 1.0 ? "✅ PASS" : "❌ FAIL")}");

            Assert.That(avgMs, Is.LessThan(1.0),
                $"Stanley must complete in <1ms for real-time guidance, but took {avgMs:F3}ms");
        }

        [Test]
        public void Stanley_CurvedTrack500Points_CompletesUnder1ms()
        {
            // Arrange
            var track = CreateCurvedTrack(500, 40.0);
            var position = new vec2(250, 10);
            double heading = Math.PI / 2;
            double speed = 5.0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.CalculateStanley(position, heading, speed, track, false);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var result = _service.CalculateStanley(position, heading, speed, track, false);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 1000.0;

            // Assert
            Console.WriteLine($"⚡ Stanley (curved, 500pts): {avgMs:F3}ms average");
            Assert.That(avgMs, Is.LessThan(1.0));
        }

        [Test]
        public void Stanley_VaryingPositions_ConsistentPerformance()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 500, 0, 200);
            double heading = Math.PI / 2;
            double speed = 5.0;

            // Test at different positions along track
            var positions = new[]
            {
                new vec2(10, -5),
                new vec2(100, 10),
                new vec2(250, -15),
                new vec2(400, 5),
                new vec2(490, -3)
            };

            // Warmup
            foreach (var pos in positions)
            {
                for (int i = 0; i < 20; i++)
                {
                    _service.CalculateStanley(pos, heading, speed, track, false);
                }
            }

            // Act & Measure
            var times = new List<double>();
            foreach (var pos in positions)
            {
                var sw = Stopwatch.StartNew();
                for (int i = 0; i < 200; i++)
                {
                    var result = _service.CalculateStanley(pos, heading, speed, track, false);
                }
                sw.Stop();
                times.Add(sw.Elapsed.TotalMilliseconds / 200.0);
            }

            // Assert
            double maxTime = 0;
            foreach (var t in times)
            {
                if (t > maxTime) maxTime = t;
            }

            Console.WriteLine($"Stanley varying positions - Max: {maxTime:F3}ms");
            Assert.That(maxTime, Is.LessThan(1.0),
                "Stanley must be consistently fast regardless of position");
        }

        // ============================================================
        // Pure Pursuit Algorithm Performance Tests
        // ============================================================

        [Test]
        public void PurePursuit_StraightTrack_CompletesUnder1ms()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 500, 0, 200);
            var position = new vec2(250, -5);
            double heading = Math.PI / 2;
            double speed = 5.0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.CalculatePurePursuit(position, heading, speed, track, false);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var result = _service.CalculatePurePursuit(position, heading, speed, track, false);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 1000.0;

            // Assert
            Console.WriteLine($"⚡ CRITICAL: PurePursuit (straight, 200pts): {avgMs:F3}ms average");
            Console.WriteLine($"   Target: <1ms | Actual: {avgMs:F3}ms | Status: {(avgMs < 1.0 ? "✅ PASS" : "❌ FAIL")}");

            Assert.That(avgMs, Is.LessThan(1.0),
                $"Pure Pursuit must complete in <1ms for real-time guidance, but took {avgMs:F3}ms");
        }

        [Test]
        public void PurePursuit_CurvedTrack500Points_CompletesUnder1ms()
        {
            // Arrange
            var track = CreateCurvedTrack(500, 40.0);
            var position = new vec2(250, 10);
            double heading = Math.PI / 2;
            double speed = 5.0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.CalculatePurePursuit(position, heading, speed, track, false);
            }

            // Act & Measure
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 1000; i++)
            {
                var result = _service.CalculatePurePursuit(position, heading, speed, track, false);
            }
            sw.Stop();

            double avgMs = sw.Elapsed.TotalMilliseconds / 1000.0;

            // Assert
            Console.WriteLine($"⚡ PurePursuit (curved, 500pts): {avgMs:F3}ms average");
            Assert.That(avgMs, Is.LessThan(1.0));
        }

        // ============================================================
        // CalculateGuidance (Main API) Performance Tests
        // ============================================================

        [Test]
        public void CalculateGuidance_Stanley_MeetsRealTimeTarget()
        {
            // Arrange
            _service.Algorithm = GuidanceAlgorithm.Stanley;
            var track = CreateStraightTrack(0, 0, 500, 0, 250);
            var position = new vec2(250, -8);
            double heading = Math.PI / 2;
            double speed = 6.0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.CalculateGuidance(position, heading, speed, track, false);
            }

            // Act & Measure with statistics
            var times = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                var sw = Stopwatch.StartNew();
                var result = _service.CalculateGuidance(position, heading, speed, track, false);
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
            Console.WriteLine($"\n📊 CalculateGuidance (Stanley) Performance:");
            Console.WriteLine($"  Average:  {avg:F3}ms");
            Console.WriteLine($"  P95:      {p95:F3}ms");
            Console.WriteLine($"  P99:      {p99:F3}ms");
            Console.WriteLine($"  Max:      {max:F3}ms");
            Console.WriteLine($"  Status:   {(p99 < 1.0 ? "✅ PASS" : "⚠️  BORDERLINE")}");

            Assert.That(avg, Is.LessThan(1.0), "Average must be <1ms");
            Assert.That(p95, Is.LessThan(1.0), "P95 must be <1ms for consistent real-time performance");
        }

        [Test]
        public void CalculateGuidance_PurePursuit_MeetsRealTimeTarget()
        {
            // Arrange
            _service.Algorithm = GuidanceAlgorithm.PurePursuit;
            var track = CreateStraightTrack(0, 0, 500, 0, 250);
            var position = new vec2(250, -8);
            double heading = Math.PI / 2;
            double speed = 6.0;

            // Warmup
            for (int i = 0; i < 100; i++)
            {
                _service.CalculateGuidance(position, heading, speed, track, false);
            }

            // Act & Measure with statistics
            var times = new List<double>();
            for (int i = 0; i < 1000; i++)
            {
                var sw = Stopwatch.StartNew();
                var result = _service.CalculateGuidance(position, heading, speed, track, false);
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
            Console.WriteLine($"\n📊 CalculateGuidance (PurePursuit) Performance:");
            Console.WriteLine($"  Average:  {avg:F3}ms");
            Console.WriteLine($"  P95:      {p95:F3}ms");
            Console.WriteLine($"  P99:      {p99:F3}ms");
            Console.WriteLine($"  Max:      {max:F3}ms");
            Console.WriteLine($"  Status:   {(p99 < 1.0 ? "✅ PASS" : "⚠️  BORDERLINE")}");

            Assert.That(avg, Is.LessThan(1.0), "Average must be <1ms");
            Assert.That(p95, Is.LessThan(1.0), "P95 must be <1ms for consistent real-time performance");
        }

        // ============================================================
        // Allocation Tests (Zero Allocation Target!)
        // ============================================================

        [Test]
        public void CalculateGuidance_MinimalAllocations()
        {
            // Arrange
            _service.Algorithm = GuidanceAlgorithm.Stanley;
            var track = CreateStraightTrack(0, 0, 500, 0, 200);
            var position = new vec2(250, -5);
            double heading = Math.PI / 2;
            double speed = 5.0;

            // Force garbage collection
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
            long gen0Before = GC.CollectionCount(0);

            // Act - Run many iterations
            for (int i = 0; i < 10000; i++)
            {
                var result = _service.CalculateGuidance(position, heading, speed, track, false);
            }

            long gen0After = GC.CollectionCount(0);

            // Assert
            int collections = (int)(gen0After - gen0Before);
            Console.WriteLine($"CalculateGuidance allocations: {collections} Gen0 collections in 10k calls");
            Console.WriteLine($"  Target: 0 collections | Actual: {collections} | Status: {(collections == 0 ? "✅ ZERO ALLOC!" : collections <= 5 ? "⚠️  LOW" : "❌ TOO HIGH")}");

            Assert.That(collections, Is.LessThanOrEqualTo(5),
                "Should have minimal/zero allocations in hot path");
        }

        [Test]
        public void FindGoalPoint_MinimalAllocations()
        {
            // Arrange
            var track = CreateStraightTrack(0, 0, 500, 0, 200);
            var position = new vec2(250, 0);
            double lookahead = 5.0;

            // Force garbage collection
            GC.Collect(2, GCCollectionMode.Forced, blocking: true);
            GC.WaitForPendingFinalizers();
            long gen0Before = GC.CollectionCount(0);

            // Act - Run many iterations
            for (int i = 0; i < 10000; i++)
            {
                _service.FindGoalPoint(position, track, lookahead, out vec3 goalPoint);
            }

            long gen0After = GC.CollectionCount(0);

            // Assert
            int collections = (int)(gen0After - gen0Before);
            Console.WriteLine($"FindGoalPoint allocations: {collections} Gen0 collections in 10k calls");

            Assert.That(collections, Is.LessThanOrEqualTo(5),
                "FindGoalPoint should have minimal allocations");
        }

        // ============================================================
        // Helper Methods
        // ============================================================

        /// <summary>
        /// Creates a straight track with AgOpenGPS heading convention.
        /// </summary>
        private List<vec3> CreateStraightTrack(double x0, double y0, double x1, double y1, int numPoints)
        {
            var track = new List<vec3>(numPoints);
            // AgOpenGPS convention: Atan2(easting_diff, northing_diff)
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

        /// <summary>
        /// Creates a curved track using sine wave with AgOpenGPS heading convention.
        /// </summary>
        private List<vec3> CreateCurvedTrack(int numPoints, double amplitude)
        {
            var track = new List<vec3>(numPoints);

            for (int i = 0; i < numPoints; i++)
            {
                double x = i * 2.0;
                double y = Math.Sin(i * 0.1) * amplitude;

                // Calculate heading from derivative (AgOpenGPS convention: Atan2(dx, dy))
                double nextY = Math.Sin((i + 1) * 0.1) * amplitude;
                double heading = Math.Atan2(2.0, nextY - y);

                track.Add(new vec3(x, y, heading));
            }

            return track;
        }
    }
}

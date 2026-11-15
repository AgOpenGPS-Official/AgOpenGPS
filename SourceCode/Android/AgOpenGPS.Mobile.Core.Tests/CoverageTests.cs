using System;
using System.Collections.Generic;
using System.IO;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class CoverageTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.8150, 15.9819);

        private static GnssFix Fix(double north, double east, double heading)
        {
            Plane.ToWgs84(new GeoPoint(north, east), out double lat, out double lon);
            return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4, SpeedKmh = 10, HeadingTrue = heading };
        }

        /// <summary>Drives from (n0, e) to (n1, e) in 0.3 m steps (10 Hz at about 11 km/h).</summary>
        private static void Drive(GuidanceEngine g, double n0, double n1, double e)
        {
            double heading = n1 > n0 ? 0 : 180;
            int steps = (int)Math.Round(Math.Abs(n1 - n0) / 0.3);
            for (int i = 0; i <= steps; i++) g.OnFix(Fix(n0 + (n1 - n0) * i / steps, e, heading));
        }

        private static GuidanceEngine Engine(double toolWidth = 6)
            => new GuidanceEngine { ToolWidth = toolWidth, PassWidth = toolWidth };

        [Test]
        public void OneStrip_IsLengthTimesWidth()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 100, 0);

            GuidanceSnapshot s = g.GetSnapshot();
            Assert.That(s.WorkedSquareMeters, Is.EqualTo(600).Within(6), "100 m x 6 m, within 1 %");
            Assert.That(s.OverlapSquareMeters, Is.EqualTo(0).Within(1));
            Assert.That(double.IsNaN(s.WorkedInsideSquareMeters), Is.True, "no boundary");
        }

        [Test]
        public void SecondPass_CountsOverlapOnce()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 100, 0);
            g.Working = false;           // implement up at the headland
            Drive(g, 100, 100, 5);       // turn round: not painted
            g.Working = true;
            Drive(g, 100, 0, 5);         // back, 5 m over: 1 m overlap with a 6 m implement

            GuidanceSnapshot s = g.GetSnapshot();
            Assert.That(s.WorkedSquareMeters, Is.EqualTo(1100).Within(11));
            Assert.That(s.OverlapSquareMeters, Is.EqualTo(100).Within(4));
        }

        [Test]
        public void WorkOff_PaintsNothing()
        {
            GuidanceEngine g = Engine();
            Drive(g, 0, 50, 0);
            Assert.That(g.GetSnapshot().WorkedSquareMeters, Is.EqualTo(0));
            Assert.That(g.Coverage, Is.Null);
        }

        [Test]
        public void RecordingSwitchedOff_PaintsNothingEvenWhileWorking()
        {
            GuidanceEngine g = Engine();
            g.CoverageEnabled = false;
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 50, 0);
            Assert.That(g.Coverage, Is.Null);

            g.CoverageEnabled = true;
            Drive(g, 50, 100, 0);
            Assert.That(g.GetSnapshot().WorkedSquareMeters, Is.EqualTo(300).Within(4), "only after switching on");
        }

        [Test]
        public void JumpInPosition_IsNotPainted()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 10, 0);
            g.OnFix(Fix(60, 0, 0));      // signal lost and back 50 m further
            Drive(g, 60, 70, 0);
            Assert.That(g.GetSnapshot().WorkedSquareMeters, Is.EqualTo(120).Within(3));
        }

        [Test]
        public void WithBoundary_WorkedInsideAndLeft()
        {
            // Field: 50 m north x 20 m wide; the strip runs 100 m, half of it outside.
            var boundary = new List<TrackPoint>();
            foreach (var (n, e) in new[] { (0.0, -10.0), (0.0, 10.0), (50.0, 10.0), (50.0, -10.0) })
            {
                Plane.ToWgs84(new GeoPoint(n, e), out double lat, out double lon);
                boundary.Add(new TrackPoint(lat, lon));
            }
            GuidanceEngine g = Engine();
            g.SetActiveField(new Field { Name = "f", Boundary = boundary });
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 100, 0);

            GuidanceSnapshot s = g.GetSnapshot();
            Assert.That(s.FieldSquareMeters, Is.EqualTo(1000).Within(0.5));
            Assert.That(s.WorkedInsideSquareMeters, Is.EqualTo(300).Within(4));
            Assert.That(s.WorkedSquareMeters, Is.EqualTo(600).Within(6));
        }

        [Test]
        public void Coverage_SurvivesSaveAndLoad()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 100, 0);
            Drive(g, 100, 0, 3);
            CoverageMap map = g.Coverage;
            Assert.That(map.Dirty, Is.True);

            string path = Path.Combine(Path.GetTempPath(), "aog-cov-" + Guid.NewGuid().ToString("N") + ".bin");
            try
            {
                map.Save(path);
                Assert.That(map.Dirty, Is.False);
                CoverageMap loaded = CoverageMap.Load(path);
                Assert.That(loaded.CoveredSquareMeters, Is.EqualTo(map.CoveredSquareMeters));
                Assert.That(loaded.OverlapSquareMeters, Is.EqualTo(map.OverlapSquareMeters));
                Assert.That(loaded.Plane.OriginLatitude, Is.EqualTo(map.Plane.OriginLatitude));

                // Continue working on the loaded map.
                GuidanceEngine g2 = Engine();
                g2.SetCoverage(loaded);
                g2.OnFix(Fix(0, 20, 0));
                g2.Working = true;
                Drive(g2, 0, 100, 20);
                Assert.That(g2.GetSnapshot().WorkedSquareMeters, Is.EqualTo(map.CoveredSquareMeters + 600).Within(8));

                Assert.That(CoverageMap.Load(path + ".missing"), Is.Null);
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void Tiles_ListedForDrawing()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(0, 0, 0));
            g.Working = true;
            Drive(g, 0, 40, 0);
            CoverageMap map = g.Coverage;
            List<CoverageTileInfo> tiles = map.TilesIn(-50, 50, -50, 50);
            Assert.That(tiles.Count, Is.InRange(3, 8), "a 40 x 6 m strip crosses a few 16 m tiles");
            Assert.That(map.CopyTile(tiles[0].TileX, tiles[0].TileY), Has.Length.EqualTo(64 * 64));
            Assert.That(map.CopyTile(1000, 1000), Is.Null);
        }
    }
}

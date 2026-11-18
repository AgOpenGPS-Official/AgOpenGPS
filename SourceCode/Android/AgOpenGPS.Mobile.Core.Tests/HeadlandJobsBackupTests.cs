using System;
using System.Collections.Generic;
using System.IO;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class HeadlandTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.8150, 15.9819);

        private static GnssFix Fix(double north, double east, double heading = 0)
        {
            Plane.ToWgs84(new GeoPoint(north, east), out double lat, out double lon);
            return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4, SpeedKmh = 8, HeadingTrue = heading };
        }

        private static Field Square(double size)
        {
            var f = new Field { Name = "sq" };
            foreach (var (n, e) in new[] { (0.0, 0.0), (0.0, size), (size, size), (size, 0.0) })
            {
                Plane.ToWgs84(new GeoPoint(n, e), out double lat, out double lon);
                f.Boundary.Add(new TrackPoint(lat, lon));
            }
            return f;
        }

        private static GuidanceEngine Engine(bool auto = true)
        {
            var g = new GuidanceEngine { ToolWidth = 6, PassWidth = 6, HeadlandWidth = 12, AutoHeadland = auto };
            g.SetActiveField(Square(100));
            return g;
        }

        [Test]
        public void WorkOff_EnteringHeadland_On_BackInTheMainPart()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(50, 50));
            g.Working = true;

            g.OnFix(Fix(80, 50));
            Assert.That(g.Working, Is.True, "main part");
            Assert.That(g.GetSnapshot().DistanceToHeadland, Is.EqualTo(8).Within(0.01));

            g.OnFix(Fix(88.6, 50));
            Assert.That(g.Working, Is.False, "0.6 m into the headland");
            Assert.That(g.GetSnapshot().WorkPausedInHeadland, Is.True);
            g.OnFix(Fix(95, 50, 180)); // turning in the headland
            Assert.That(g.Working, Is.False);

            g.OnFix(Fix(87.4, 50, 180));
            Assert.That(g.Working, Is.True, "0.6 m back into the main part");
        }

        [Test]
        public void NoFlicker_AtTheLine()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(50, 50));
            g.Working = true;
            g.OnFix(Fix(88.2, 50)); // 0.2 m in: inside the hysteresis
            Assert.That(g.Working, Is.True);
            g.OnFix(Fix(87.9, 50));
            Assert.That(g.Working, Is.True);
        }

        [Test]
        public void SwitchingOffByHand_EndsTheAutomatic()
        {
            GuidanceEngine g = Engine();
            g.OnFix(Fix(50, 50));
            g.Working = true;
            g.OnFix(Fix(90, 50));
            Assert.That(g.Working, Is.False);
            g.Working = false;                 // driver: done with this field
            g.OnFix(Fix(70, 50));
            Assert.That(g.Working, Is.False, "stays off");
        }

        [Test]
        public void WithoutAutomatic_OnlyTheLineAndDistance()
        {
            GuidanceEngine g = Engine(auto: false);
            g.OnFix(Fix(50, 50));
            g.Working = true;
            g.OnFix(Fix(95, 50));
            Assert.That(g.Working, Is.True);
            GuidanceSnapshot s = g.GetSnapshot();
            Assert.That(s.InHeadland, Is.True);
            Assert.That(s.HeadlandLine, Has.Count.EqualTo(4));
            Assert.That(Geometry.Area(s.HeadlandLine), Is.EqualTo(76 * 76).Within(1), "12 m inside a 100 m square");
        }

        [Test]
        public void HeadlandAlert_OnceWhenApproaching()
        {
            var m = new AlertMonitor(new AlertSettings());
            var t = DateTime.UtcNow;
            List<AlertKind> Step(double d)
            {
                t = t.AddSeconds(0.25);
                return m.Update(new AlertInputs { Now = t, Receiving = true, FixQuality = 4, InsideField = true, DistanceToBoundary = 50, DistanceToHeadland = d });
            }
            Assert.That(Step(20), Is.Empty);
            Assert.That(Step(4), Is.EqualTo(new[] { AlertKind.HeadlandAhead }));
            Assert.That(Step(3), Is.Empty);
            Assert.That(Step(-2), Is.Empty, "in the headland");
            Assert.That(Step(10), Is.Empty, "back in the main part: re-armed");
            Assert.That(Step(4.5), Is.EqualTo(new[] { AlertKind.HeadlandAhead }));
        }

        [Test]
        public void ActiveLine_HasAAndBForTheMap()
        {
            var g = new GuidanceEngine();
            g.OnFix(Fix(0, 0));
            g.SetA();
            g.OnFix(Fix(40, 0));
            g.SetActiveLine(g.SetB("AB", out _));
            GuidanceSnapshot s = g.GetSnapshot();
            Assert.That(s.LineA.Value.DistanceTo(s.LineB.Value), Is.EqualTo(40).Within(0.01));
        }
    }

    public class JobLogTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.8150, 15.9819);

        private static void At(double n, out double lat, out double lon) => Plane.ToWgs84(new GeoPoint(n, 0), out lat, out lon);

        [Test]
        public void Job_CollectsTimeDistanceAreaAndEndsOnFieldChange()
        {
            string path = Path.Combine(Path.GetTempPath(), "aog-jobs-" + Guid.NewGuid().ToString("N") + ".json");
            try
            {
                var log = new JobLog(path);
                var north = new Field { Name = "North" };
                var south = new Field { Name = "South" };
                var t = new DateTime(2026, 11, 18, 9, 0, 0, DateTimeKind.Local).ToUniversalTime();

                // 100 s at 10 km/h (0.25 s steps), 6 m wide: about 278 m driven, about 1667 m² worked.
                double covered = 0;
                for (int i = 0; i <= 400; i++)
                {
                    At(i * 0.25 * 10 / 3.6, out double lat, out double lon);
                    log.Sample(t.AddSeconds(i * 0.25), true, north, lat, lon, covered, 6);
                    covered += 0.25 * 10 / 3.6 * 6;
                }
                Job j = log.Current;
                Assert.That(j.WorkingSeconds, Is.EqualTo(100).Within(0.01));
                Assert.That(j.WorkingMeters, Is.EqualTo(277.8).Within(0.5));
                Assert.That(j.AverageSpeedKmh, Is.EqualTo(10).Within(0.05));
                Assert.That(j.WorkedSquareMeters, Is.EqualTo(1666.7).Within(5));

                // Work off: time stops.
                log.Sample(t.AddSeconds(200), false, north, double.NaN, double.NaN, covered, 6);
                Assert.That(log.Current.WorkingSeconds, Is.EqualTo(100).Within(0.01));

                // Another field: the North job ends, a South job starts.
                At(0, out double la, out double lo);
                log.Sample(t.AddSeconds(300), true, south, la, lo, 0, 6);
                Assert.That(log.Current.FieldName, Is.EqualTo("South"));
                log.Sample(t.AddSeconds(301), true, south, la, lo, 0, 6);
                log.EndJob();

                var reloaded = new JobLog(path);
                Assert.That(reloaded.Jobs, Has.Count.EqualTo(2));
                string csv = JobLog.ToCsv(reloaded.Jobs);
                Assert.That(csv, Does.StartWith("Date;Field;Start;End;Working time (h:mm);Worked (ha)"));
                Assert.That(csv, Does.Contain(";North;09:00;"));
                Assert.That(csv, Does.Contain(";0:01;0.17;0.28;10.0;6.00"), "1 min 40 s, 0.17 ha, 0.28 km, 10 km/h");
            }
            finally
            {
                File.Delete(path);
            }
        }

        [Test]
        public void OnAndOffAtOnce_IsNoJob()
        {
            var log = new JobLog(null);
            At(0, out double lat, out double lon);
            log.Sample(DateTime.UtcNow, true, new Field { Name = "x" }, lat, lon, 0, 6);
            log.EndJob();
            Assert.That(log.Jobs, Is.Empty);
        }
    }

    public class BackupTests
    {
        [Test]
        public void SettingsAndFields_RoundTrip()
        {
            var settings = new Dictionary<string, object> { ["tool.width"] = "6.5", ["ntrip.enabled"] = true, ["ui.language"] = "hr" };
            string fields = FieldExport.ToGeoJson(new[] { new Field { Name = "F" } }, _ => null);
            string text = Backup.Write(settings, fields, DateTime.UtcNow);

            Backup.Read(text, out Dictionary<string, object> back, out string backFields);
            Assert.That(back["tool.width"], Is.EqualTo("6.5"));
            Assert.That(back["ntrip.enabled"], Is.EqualTo(true));
            Assert.That(FieldImport.Parse(backFields, "x")[0].Name, Is.EqualTo("F"));

            Assert.Throws<FormatException>(() => Backup.Read("{\"type\":\"FeatureCollection\"}", out _, out _));
        }
    }
}

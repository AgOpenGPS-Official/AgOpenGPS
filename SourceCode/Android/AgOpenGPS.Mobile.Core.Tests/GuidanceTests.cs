using System;
using System.IO;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class GuidanceTests
    {
        // A field near Zagreb.
        private const double Lat0 = 45.8150;
        private const double Lon0 = 15.9819;
        private static readonly LocalPlane Field = new LocalPlane(Lat0, Lon0);

        private static void At(double north, double east, out double lat, out double lon)
            => Field.ToWgs84(new GeoPoint(north, east), out lat, out lon);

        private static AbLine Line(double northA, double eastA, double northB, double eastB)
        {
            At(northA, eastA, out double la, out double loa);
            At(northB, eastB, out double lb, out double lob);
            return new AbLine { Name = "test", LatitudeA = la, LongitudeA = loa, LatitudeB = lb, LongitudeB = lob };
        }

        private static AbGuidanceResult Guide(AbLine line, double north, double east, double heading, double width)
        {
            LocalPlane plane = line.CreatePlane();
            At(north, east, out double lat, out double lon);
            return AbGuidance.Compute(line, plane, plane.ToLocal(lat, lon), heading, width);
        }

        private static GnssFix Fix(double north, double east, double speed = double.NaN, double cog = double.NaN, double dual = double.NaN)
        {
            At(north, east, out double lat, out double lon);
            return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4, SpeedKmh = speed, HeadingTrue = cog, HeadingDual = dual };
        }

        [Test]
        public void LocalPlane_RoundTripsAndHasRealisticScale()
        {
            At(123.456, -78.9, out double lat, out double lon);
            GeoPoint back = Field.ToLocal(lat, lon);
            Assert.That(back.Northing, Is.EqualTo(123.456).Within(1e-6));
            Assert.That(back.Easting, Is.EqualTo(-78.9).Within(1e-6));

            Assert.That(Field.ToLocal(Lat0 + 1, Lon0).Northing, Is.EqualTo(111_150).Within(200), "1 degree latitude");
            Assert.That(Field.ToLocal(Lat0, Lon0 + 1).Easting, Is.EqualTo(77_700).Within(300), "1 degree longitude at 45.8 N");
        }

        [Test]
        public void NorthLine_RightOfLineMeansSteerLeft_AndFlipsWhenDrivingBack()
        {
            AbLine line = Line(0, 0, 100, 0);
            Assert.That(line.HeadingDegrees, Is.EqualTo(0).Within(1e-6));
            Assert.That(line.LengthMeters, Is.EqualTo(100).Within(1e-6));

            AbGuidanceResult north = Guide(line, 50, 0.30, 2, 6);
            Assert.That(north.Pass, Is.EqualTo(0));
            Assert.That(north.CrossTrackMeters, Is.EqualTo(0.30).Within(1e-6), "right of line, steer left");
            Assert.That(north.SameDirectionAsAb, Is.True);
            Assert.That(north.HeadingErrorDegrees, Is.EqualTo(2).Within(1e-6));
            Assert.That(north.AlongTrack, Is.EqualTo(50).Within(1e-6));

            AbGuidanceResult south = Guide(line, 50, 0.30, 178, 6);
            Assert.That(south.SameDirectionAsAb, Is.False);
            Assert.That(south.CrossTrackMeters, Is.EqualTo(-0.30).Within(1e-6), "driving south, east is left");
            Assert.That(south.TravelLineHeading, Is.EqualTo(180).Within(1e-6));
            Assert.That(south.HeadingErrorDegrees, Is.EqualTo(-2).Within(1e-6));
        }

        [TestCase(6.2, 1, 0.2)]
        [TestCase(5.9, 1, -0.1)]
        [TestCase(-5.9, -1, 0.1)]
        [TestCase(-14.0, -2, -2.0)]
        [TestCase(3.1, 1, -2.9)]
        [TestCase(2.9, 0, 2.9)]
        public void ParallelPasses_AreOneWidthApart(double east, int pass, double xte)
        {
            AbGuidanceResult r = Guide(Line(0, 0, 100, 0), 10, east, 0, 6);
            Assert.That(r.Pass, Is.EqualTo(pass));
            Assert.That(r.CrossTrackMeters, Is.EqualTo(xte).Within(1e-6));
            Assert.That(r.DistanceFromRefLine, Is.EqualTo(east).Within(1e-6));
        }

        [Test]
        public void DiagonalLine_UsesPerpendicularDistance()
        {
            AbLine line = Line(0, 0, 100, 100); // heading 45
            Assert.That(line.HeadingDegrees, Is.EqualTo(45).Within(1e-6));
            // 1 m perpendicular to the right of a 45 degree line: (+0.7071 E, -0.7071 N) from a point on it.
            AbGuidanceResult r = Guide(line, 50 - Math.Sqrt(0.5), 50 + Math.Sqrt(0.5), double.NaN, 0);
            Assert.That(r.CrossTrackMeters, Is.EqualTo(1.0).Within(1e-6));
            Assert.That(double.IsNaN(r.HeadingErrorDegrees), Is.True, "no heading yet");
        }

        [Test]
        public void HeadingTracker_PrefersDualThenCogThenMovement()
        {
            var t = new HeadingTracker();
            t.Update(Fix(0, 0, speed: 5, cog: 90, dual: 10));
            Assert.That(t.Heading, Is.EqualTo(10).Within(1e-9), "dual antenna wins");

            t = new HeadingTracker();
            t.Update(Fix(0, 0, speed: 5, cog: 90));
            Assert.That(t.Heading, Is.EqualTo(90).Within(1e-9), "course over ground while moving");

            t = new HeadingTracker();
            t.Update(Fix(0, 0, speed: 0.5, cog: 90));
            Assert.That(double.IsNaN(t.Heading), Is.True, "COG ignored when slow, nothing moved yet");
            t.Update(Fix(0.2, 0.2, speed: 0.5, cog: 90));
            Assert.That(double.IsNaN(t.Heading), Is.True, "moved less than 0.5 m");
            t.Update(Fix(1, 1, speed: 0.5, cog: 90));
            Assert.That(t.Heading, Is.EqualTo(45).Within(1e-6), "direction of movement");
            t.Update(Fix(1, 1));
            Assert.That(t.Heading, Is.EqualTo(45).Within(1e-6), "kept while standing");
        }

        [Test]
        public void Engine_RecordsAbLine_AndGuides()
        {
            var e = new GuidanceEngine { PassWidth = 6 };
            Assert.That(e.SetA(), Is.False, "no position yet");

            e.OnFix(Fix(0, 0));
            Assert.That(e.SetA(), Is.True);
            e.OnFix(Fix(3, 0));
            Assert.That(e.GetSnapshot().RecordedMeters, Is.EqualTo(3).Within(1e-6));
            Assert.That(e.SetB("AB 1", out string error), Is.Null);
            Assert.That(error, Does.Contain("at least"));

            e.OnFix(Fix(80, 0.5));
            AbLine line = e.SetB("AB 1", out error);
            Assert.That(line, Is.Not.Null, error);
            Assert.That(e.GetSnapshot().Recording, Is.False);
            Assert.That(line.Kind, Is.EqualTo(LineKind.Straight));

            e.SetActiveLine(line);
            e.OnFix(Fix(40, 6.5, speed: 8, cog: 0));
            GuidanceSnapshot s = e.GetSnapshot();
            Assert.That(s.Result.Pass, Is.EqualTo(1));
            Assert.That(Math.Abs(s.Result.CrossTrackMeters), Is.LessThan(0.5));
            Assert.That(s.Passes, Has.Count.EqualTo(2 * GuidanceEngine.DrawPassesEachSide + 1));
            Assert.That(s.Vehicle, Is.Not.Null);
        }

        // Quarter circle, radius 100 m, centre at (0, 100): starts at the origin heading north,
        // turns right and ends at (100, 100) heading east.
        private static AbLine Arc(int points = 91)
        {
            var line = new AbLine { Name = "arc", Kind = LineKind.Curve };
            for (int i = 0; i < points; i++)
            {
                double a = Math.PI / 2 * i / (points - 1);
                At(100 * Math.Sin(a), 100 - 100 * Math.Cos(a), out double lat, out double lon);
                line.Points.Add(new TrackPoint(lat, lon));
            }
            line.LatitudeA = line.Points[0].Latitude; line.LongitudeA = line.Points[0].Longitude;
            line.LatitudeB = line.Points[^1].Latitude; line.LongitudeB = line.Points[^1].Longitude;
            return line;
        }

        [Test]
        public void Curve_DistanceAndHeadingFollowTheBend()
        {
            AbLine arc = Arc();
            Assert.That(arc.LengthMeters, Is.EqualTo(Math.PI * 50).Within(0.1));

            // Halfway round (45 degrees), 0.3 m outside the curve: outside of a right turn is left.
            double a = Math.PI / 4, r = 100.3;
            AbGuidanceResult r1 = Guide(arc, r * Math.Sin(a), 100 - r * Math.Cos(a), 45, 6);
            Assert.That(r1.Pass, Is.EqualTo(0));
            Assert.That(r1.CrossTrackMeters, Is.EqualTo(-0.3).Within(0.01), "left of the line, steer right");
            Assert.That(r1.TravelLineHeading, Is.EqualTo(45).Within(1.5));
            Assert.That(r1.AlongTrack, Is.EqualTo(Math.PI * 25).Within(0.1));

            // 6.2 m inside the curve = pass +1 (right), 0.2 m past it.
            r = 100 - 6.2;
            AbGuidanceResult r2 = Guide(arc, r * Math.Sin(a), 100 - r * Math.Cos(a), 45, 6);
            Assert.That(r2.Pass, Is.EqualTo(1));
            Assert.That(r2.CrossTrackMeters, Is.EqualTo(0.2).Within(0.01));
        }

        [Test]
        public void Curve_EndsContinueStraight()
        {
            // 20 m past B, 1 m south (= right of an eastward line). The extension follows the last
            // segment, which on this 1-degree polygon points 0.5 degrees short of east: 20 m * sin(0.5) = 0.17 m.
            AbGuidanceResult r = Guide(Arc(), 99, 120, 90, 6);
            Assert.That(r.CrossTrackMeters, Is.EqualTo(1.0 + 20 * Math.Sin(0.5 * Math.PI / 180)).Within(0.01));
            Assert.That(r.TravelLineHeading, Is.EqualTo(90).Within(1.5));
        }

        [Test]
        public void Curve_PassPolylineIsParallel()
        {
            AbLine arc = Arc();
            LocalPlane plane = arc.CreatePlane();
            var pass1 = arc.PassPolyline(plane, 1, 6, extend: 0);
            // Pass +1 lies 6 m to the right of a right turn: radius 94 around the same centre.
            GeoPoint centre = plane.ToLocal(0, 0);
            Field.ToWgs84(new GeoPoint(0, 100), out double clat, out double clon);
            centre = plane.ToLocal(clat, clon);
            foreach (GeoPoint p in pass1) Assert.That(p.DistanceTo(centre), Is.EqualTo(94).Within(0.05));
        }

        [Test]
        public void Engine_RecordsCurveFromDrivenPath()
        {
            var e = new GuidanceEngine { PassWidth = 6 };
            e.OnFix(Fix(0, 0));
            Assert.That(e.SetA(LineKind.Curve), Is.True);

            // Drive the quarter circle at 0.25 m steps (like 10 Hz at 9 km/h).
            for (int i = 1; i <= 628; i++)
            {
                double a = Math.PI / 2 * i / 628;
                e.OnFix(Fix(100 * Math.Sin(a), 100 - 100 * Math.Cos(a)));
            }
            GuidanceSnapshot rec = e.GetSnapshot();
            Assert.That(rec.Recording, Is.True);
            Assert.That(rec.RecordedMeters, Is.EqualTo(Math.PI * 50).Within(2));

            AbLine line = e.SetB("curve", out string error);
            Assert.That(line, Is.Not.Null, error);
            Assert.That(line.Kind, Is.EqualTo(LineKind.Curve));
            Assert.That(line.Points.Count, Is.InRange(10, 160), "simplified, not every metre");
            Assert.That(line.LengthMeters, Is.EqualTo(Math.PI * 50).Within(0.5));

            // Guidance on the recorded curve matches the ideal one within a few centimetres.
            e.SetActiveLine(line);
            double a45 = Math.PI / 4, r = 100 + 0.4;
            e.OnFix(Fix(r * Math.Sin(a45), 100 - r * Math.Cos(a45), speed: 8, cog: 45));
            Assert.That(e.GetSnapshot().Result.CrossTrackMeters, Is.EqualTo(-0.4).Within(0.04));
        }

        [Test]
        public void Simplify_KeepsCornersDropsStraightRuns()
        {
            var pts = new System.Collections.Generic.List<GeoPoint>();
            for (int i = 0; i <= 50; i++) pts.Add(new GeoPoint(i, 0));
            for (int i = 1; i <= 50; i++) pts.Add(new GeoPoint(50, i));
            Assert.That(AbGuidance.Simplify(pts, 0.03), Is.EqualTo(new[] { 0, 50, 100 }));
        }

        [Test]
        public void Engine_SetBWithoutA_Fails()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            Assert.That(e.SetB("x", out string error), Is.Null);
            Assert.That(error, Is.EqualTo("Set point A first"));
        }
    }

    public class AbLineStoreTests
    {
        private string _dir;

        [SetUp]
        public void SetUp()
        {
            _dir = Path.Combine(Path.GetTempPath(), "aog-store-" + Guid.NewGuid().ToString("N"));
        }

        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_dir)) Directory.Delete(_dir, true);
        }

        [Test]
        public void Lines_SurviveRestart()
        {
            string path = Path.Combine(_dir, "ablines.json");
            var store = new AbLineStore(path);
            Assert.That(store.NextName(), Is.EqualTo("AB 1"));

            var a = new AbLine { Name = store.NextName(), LatitudeA = 45, LongitudeA = 16, LatitudeB = 45.001, LongitudeB = 16 };
            store.Add(a, makeActive: true);
            var b = new AbLine { Name = store.NextName(), LatitudeA = 45, LongitudeA = 16, LatitudeB = 45, LongitudeB = 16.001 };
            Assert.That(b.Name, Is.EqualTo("AB 2"));
            store.Add(b, makeActive: false);
            store.Rename(a.Id, "  North edge ");

            var reloaded = new AbLineStore(path);
            Assert.That(reloaded.Lines, Has.Count.EqualTo(2));
            Assert.That(reloaded.Active.Id, Is.EqualTo(a.Id));
            Assert.That(reloaded.Active.Name, Is.EqualTo("North edge"));
            Assert.That(reloaded.Active.LatitudeB, Is.EqualTo(45.001));

            reloaded.Delete(a.Id);
            Assert.That(new AbLineStore(path).Active, Is.Null);
            Assert.That(new AbLineStore(path).Lines, Has.Count.EqualTo(1));
        }

        [Test]
        public void DamagedFile_StartsEmptyAndKeepsACopy()
        {
            Directory.CreateDirectory(_dir);
            string path = Path.Combine(_dir, "ablines.json");
            File.WriteAllText(path, "{ not json");

            var store = new AbLineStore(path);
            Assert.That(store.Lines, Is.Empty);
            Assert.That(File.Exists(path + ".bad"), Is.True);
        }
    }
}

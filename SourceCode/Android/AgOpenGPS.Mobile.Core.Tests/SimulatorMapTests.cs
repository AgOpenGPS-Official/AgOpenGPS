using System;
using System.Threading;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class SimulatorTests
    {
        private const double Lat0 = 45.8150, Lon0 = 15.9819;
        private static readonly LocalPlane Plane = new LocalPlane(Lat0, Lon0);

        private static GeoPoint Where(GnssSimulator s) => Plane.ToLocal(s.Latitude, s.Longitude);

        [Test]
        public void DrivesStraightAtTheSetSpeed()
        {
            var sim = new GnssSimulator(Lat0, Lon0, 0) { SpeedKmh = 18 };
            for (int i = 0; i < 100; i++) sim.Step(0.1);   // 10 s at 5 m/s
            Assert.That(Where(sim).Northing, Is.EqualTo(50).Within(0.01));
            Assert.That(Where(sim).Easting, Is.EqualTo(0).Within(0.01));
        }

        [Test]
        public void TurnsOnACircle_OfWheelbaseOverTanAngle()
        {
            var sim = new GnssSimulator(Lat0, Lon0, 0) { SpeedKmh = 10, WheelAngle = 10, Wheelbase = 3 };
            double radius = 3 / Math.Tan(10 * Math.PI / 180);
            double quarter = Math.PI / 2 * radius / (10 / 3.6);
            int steps = (int)Math.Round(quarter / 0.01);
            for (int i = 0; i < steps; i++) sim.Step(0.01);
            Assert.That(sim.Heading, Is.EqualTo(90).Within(0.2));
            Assert.That(Where(sim).Northing, Is.EqualTo(radius).Within(0.1));
            Assert.That(Where(sim).Easting, Is.EqualTo(radius).Within(0.1));
        }

        [Test]
        public void Sentences_ReadBackThroughTheNormalParser()
        {
            var sim = new GnssSimulator(Lat0, Lon0, 30) { SpeedKmh = 12 };
            var acc = new NmeaAccumulator();
            GnssFix fix = null;
            foreach (string line in sim.Sentences(DateTime.UtcNow).Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
                fix = acc.Process(line, DateTime.UtcNow) ?? fix;

            Assert.That(fix, Is.Not.Null);
            Assert.That(fix.FixQuality, Is.EqualTo(4));
            Assert.That(Plane.ToLocal(fix.Latitude, fix.Longitude).DistanceTo(new GeoPoint(0, 0)), Is.LessThan(0.005));
            // VTG comes after GGA here, so the next GGA carries it; check the parser kept it.
            fix = acc.Process(sim.Sentences(DateTime.UtcNow).Split("\r\n")[0], DateTime.UtcNow);
            Assert.That(fix.SpeedKmh, Is.EqualTo(12).Within(0.01));
            Assert.That(fix.HeadingTrue, Is.EqualTo(30).Within(0.01));
        }

        [Test]
        public void SimulatorLink_DeliversFixesThroughTheSupervisor()
        {
            var sim = new GnssSimulator(Lat0, Lon0, 0) { SpeedKmh = 10 };
            var sup = new LinkSupervisor(_ => new SimulatorLink(sim, hz: 20));
            int fixes = 0;
            sup.FixUpdated += f => Interlocked.Increment(ref fixes);
            sup.Start();
            Assert.That(SpinWait.SpinUntil(() => fixes >= 5, 5000), Is.True);
            Assert.That(sup.GetSnapshot().Session.Latest.FixQuality, Is.EqualTo(4));
            sup.Stop();
        }

        [Test]
        public void AutoSteer_PullsOntoTheLine()
        {
            // Line northwards through the origin; the tractor starts 2 m east, heading north.
            var engine = new GuidanceEngine { PassWidth = 20 };
            Plane.ToWgs84(new GeoPoint(0, 2), out double lat, out double lon);
            var sim = new GnssSimulator(lat, lon, 0) { SpeedKmh = 10, AutoSteer = true };
            sim.SteerSource = () => engine.GetSnapshot().Steer;

            Plane.ToWgs84(new GeoPoint(100, 0), out double latB, out double lonB);
            engine.SetActiveLine(new AbLine { LatitudeA = Lat0, LongitudeA = Lon0, LatitudeB = latB, LongitudeB = lonB });

            var acc = new NmeaAccumulator();
            for (int i = 0; i < 300; i++) // 30 s at 10 Hz
            {
                sim.Step(0.1);
                foreach (string line in sim.Sentences(DateTime.UtcNow).Split("\r\n", StringSplitOptions.RemoveEmptyEntries))
                {
                    GnssFix f = acc.Process(line, DateTime.UtcNow);
                    if (f != null) engine.OnFix(f);
                }
            }
            Assert.That(Math.Abs(engine.GetSnapshot().Result.CrossTrackMeters), Is.LessThan(0.05));
            Assert.That(Math.Abs(LocalPlane.AngleDiff(sim.Heading, 0)), Is.LessThan(1));
        }

        [Test]
        public void PurePursuitWheelAngle_IsLimited()
        {
            Assert.That(Steering.PurePursuitWheelAngle(0, 5, 3), Is.EqualTo(0));
            Assert.That(Steering.PurePursuitWheelAngle(10, 5, 3), Is.GreaterThan(0));
            Assert.That(Steering.PurePursuitWheelAngle(-80, 3, 3), Is.EqualTo(-35));
        }
    }

    public class MapTilesTests
    {
        [Test]
        public void TileMaths_RoundTripsAndKnownValues()
        {
            MapTiles.ToTile(0, 0, 1, out double x, out double y);
            Assert.That(x, Is.EqualTo(1).Within(1e-9));
            Assert.That(y, Is.EqualTo(1).Within(1e-9));

            MapTiles.ToTile(45.8150, 15.9819, 17, out x, out y);
            MapTiles.ToLatLon(x, y, 17, out double lat, out double lon);
            Assert.That(lat, Is.EqualTo(45.8150).Within(1e-9));
            Assert.That(lon, Is.EqualTo(15.9819).Within(1e-9));
            Assert.That((int)x, Is.EqualTo(71354), "(15.9819 + 180) / 360 * 2^17 = 71354.8");
        }

        [Test]
        public void Zoom_FollowsTheScreenScale()
        {
            Assert.That(MapTiles.MetersPerPixel(19, 45.815), Is.EqualTo(0.2084).Within(0.001));
            Assert.That(MapTiles.ZoomFor(0.02, 45.815), Is.EqualTo(19), "capped");
            Assert.That(MapTiles.ZoomFor(0.2084, 45.815), Is.EqualTo(19));
            Assert.That(MapTiles.ZoomFor(0.4168, 45.815), Is.EqualTo(18));
            Assert.That(MapTiles.ZoomFor(0.02, 45.815, maxZoom: 18), Is.EqualTo(18));
        }

        [Test]
        public void TilesFor_AFieldAndUrls()
        {
            // About 300 x 400 m at zoom 18 (tiles about 107 m): 3-5 columns, 4-6 rows.
            var tiles = MapTiles.TilesFor(45.8150, 15.9819, 45.8186, 15.9858, 18);
            Assert.That(tiles.Count, Is.InRange(12, 30));
            Assert.That(MapTiles.Url("https://t/{z}/{y}/{x}.jpg", new TileId(18, 5, 7)), Is.EqualTo("https://t/18/7/5.jpg"));
            Assert.That(MapTiles.SourceKey("a"), Is.Not.EqualTo(MapTiles.SourceKey("b")));
        }
    }
}

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class FieldCentreTests
    {
        private static Field FieldFrom(params (double n, double e)[] pts)
        {
            var plane = new LocalPlane(45.0, 15.0);
            var f = new Field { Name = "t" };
            foreach (var (n, e) in pts)
            {
                plane.ToWgs84(new GeoPoint(n, e), out double lat, out double lon);
                f.Boundary.Add(new TrackPoint(lat, lon));
            }
            return f;
        }

        [Test]
        public void Square_CentreIsTheMiddle()
        {
            Field f = FieldFrom((0, 0), (0, 100), (100, 100), (100, 0));
            Assert.That(f.TryGetCentre(out double lat, out double lon), Is.True);
            GeoPoint c = new LocalPlane(45.0, 15.0).ToLocal(lat, lon);
            Assert.That(c.Northing, Is.EqualTo(50).Within(0.01));
            Assert.That(c.Easting, Is.EqualTo(50).Within(0.01));
        }

        [Test]
        public void UShapedField_CentreStillInside()
        {
            // A U: the centre of area falls in the gap between the arms.
            Field f = FieldFrom((0, 0), (0, 100), (100, 100), (100, 80), (10, 80), (10, 20), (100, 20), (100, 0));
            Assert.That(f.TryGetCentre(out double lat, out double lon), Is.True);
            var plane = new LocalPlane(45.0, 15.0);
            Assert.That(Geometry.Contains(f.BoundaryLocal(plane), plane.ToLocal(lat, lon)), Is.True);
        }

        [Test]
        public void Simulator_MoveTo_StopsTheTractor()
        {
            var sim = new GnssSimulator(45, 15) { SpeedKmh = 10, WheelAngle = 5, AutoSteer = true };
            sim.MoveTo(45.5, 16.25);
            Assert.That(sim.Latitude, Is.EqualTo(45.5));
            Assert.That(sim.SpeedKmh, Is.EqualTo(0));
            Assert.That(sim.WheelAngle, Is.EqualTo(0));
            Assert.That(sim.AutoSteer, Is.False);
        }
    }
}

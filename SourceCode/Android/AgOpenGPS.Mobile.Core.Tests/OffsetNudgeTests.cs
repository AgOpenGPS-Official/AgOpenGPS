using System;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class OffsetNudgeTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.8150, 15.9819);

        private static GnssFix Fix(double north, double east, double heading, double roll = double.NaN)
        {
            Plane.ToWgs84(new GeoPoint(north, east), out double lat, out double lon);
            return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4, SpeedKmh = 10, HeadingTrue = heading, Roll = roll };
        }

        private static GeoPoint Local(GnssFix f) => Plane.ToLocal(f.Latitude, f.Longitude);

        [Test]
        public void Geometry_MovesAntennaToGuideAndToolPoints()
        {
            var g = new VehicleGeometry { AntennaForward = 2, AntennaRight = 0.5, ToolBehind = 3, ToolRight = -0.2 };
            g.Apply(Fix(100, 50, 0), 0, out GnssFix guide, out GnssFix tool);
            // Heading north: pivot is 2 m south and 0.5 m west of the antenna, guide 0.2 m further west.
            Assert.That(Local(guide).Northing, Is.EqualTo(98).Within(1e-4));
            Assert.That(Local(guide).Easting, Is.EqualTo(49.3).Within(1e-4));
            Assert.That(Local(tool).Northing, Is.EqualTo(95).Within(1e-4));
            Assert.That(Local(tool).Easting, Is.EqualTo(49.3).Within(1e-4));

            // Heading east: "ahead" is east, "right" is south.
            g.Apply(Fix(100, 50, 90), 90, out guide, out tool);
            Assert.That(Local(guide).Easting, Is.EqualTo(48).Within(1e-4));
            Assert.That(Local(guide).Northing, Is.EqualTo(100.7).Within(1e-4));
        }

        [Test]
        public void Geometry_CorrectsRollWithAntennaHeight()
        {
            // 3 m high antenna, 5 degrees right side down: the antenna is 0.26 m right of the ground point.
            var g = new VehicleGeometry { AntennaHeight = 3 };
            g.Apply(Fix(0, 0, 0, roll: 5), 0, out GnssFix guide, out _);
            Assert.That(Local(guide).Easting, Is.EqualTo(-3 * Math.Sin(5 * Math.PI / 180)).Within(1e-4));
        }

        [Test]
        public void Geometry_WithoutHeading_UsesTheAntenna()
        {
            var g = new VehicleGeometry { AntennaForward = 2 };
            GnssFix f = Fix(10, 10, double.NaN);
            g.Apply(f, double.NaN, out GnssFix guide, out GnssFix tool);
            Assert.That(guide, Is.SameAs(f));
            Assert.That(tool, Is.SameAs(f));
        }

        [Test]
        public void Engine_GuidesTheImplementCentre_AndPaintsWhereItWorks()
        {
            var e = new GuidanceEngine { PassWidth = 6, ToolWidth = 6 };
            e.Vehicle = new VehicleGeometry { AntennaRight = 1, ToolBehind = 4 };
            e.OnFix(Fix(0, 1, 0));
            e.SetA();
            e.OnFix(Fix(50, 1, 0));
            AbLine line = e.SetB("AB 1", out string error);
            Assert.That(line, Is.Not.Null, error);
            // The line was recorded under the implement centre, 1 m left of the antenna.
            Assert.That(Plane.ToLocal(line.LatitudeA, line.LongitudeA).Easting, Is.EqualTo(0).Within(1e-3));
            e.SetActiveLine(line);

            e.OnFix(Fix(60, 1.1, 0));
            Assert.That(e.GetSnapshot().Result.CrossTrackMeters, Is.EqualTo(0.1).Within(1e-3));

            // Worked area starts 4 m behind the axle.
            e.Working = true;
            for (int i = 0; i <= 100; i++) e.OnFix(Fix(60 + i * 0.3, 1, 0));
            Assert.That(e.GetSnapshot().WorkedSquareMeters, Is.EqualTo(30 * 6).Within(4));
        }

        [Test]
        public void Nudge_ShiftsPassesRelativeToTheDrivingDirection()
        {
            var e = new GuidanceEngine { PassWidth = 6 };
            e.OnFix(Fix(0, 0, 0));
            e.SetA();
            e.OnFix(Fix(50, 0, 0));
            AbLine line = e.SetB("AB 1", out _);
            e.SetActiveLine(line);

            e.OnFix(Fix(60, 0.10, 0));                     // 10 cm right of the line, going north
            Assert.That(e.GetSnapshot().Result.CrossTrackMeters, Is.EqualTo(0.10).Within(1e-3));
            Assert.That(e.NudgeActiveLine(0.05), Is.EqualTo(0.05).Within(1e-9), "line 5 cm to the right");
            Assert.That(e.GetSnapshot().Result.CrossTrackMeters, Is.EqualTo(0.05).Within(1e-3));

            // Driving south, "right" is west: nudging right moves the line back.
            e.OnFix(Fix(60, 0.10, 180));
            e.OnFix(Fix(59, 0.10, 180));
            Assert.That(e.NudgeActiveLine(0.05), Is.EqualTo(0).Within(1e-9));

            e.OnFix(Fix(58, 0.37, 180));
            Assert.That(e.SnapActiveLine(), Is.EqualTo(0.37).Within(1e-3));
            Assert.That(e.GetSnapshot().Result.CrossTrackMeters, Is.EqualTo(0).Within(1e-3));

            // Passes move with the nudge.
            LocalPlane p = line.CreatePlane();
            Assert.That(line.PassPolyline(p, 1, 6, 0)[0].Easting - p.ToLocal(line.LatitudeA, line.LongitudeA).Easting,
                Is.EqualTo(6.37).Within(1e-3));

            e.ResetNudge();
            Assert.That(line.NudgeMeters, Is.EqualTo(0));
        }
    }
}

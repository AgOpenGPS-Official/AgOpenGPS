using System;
using System.Collections.Generic;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class SteeringTests
    {
        private static readonly double Atan1In5 = Math.Atan(1.0 / 5.0) * 180 / Math.PI; // 11.31 degrees

        // A line northwards along easting 0, from far south to far north.
        private static readonly List<GeoPoint> North = new List<GeoPoint> { new GeoPoint(-500, 0), new GeoPoint(500, 0) };

        [Test]
        public void OnLineAndParallel_SteerStraight()
        {
            SteerResult r = Steering.Compute(North, new GeoPoint(0, 0), 0, true, 5);
            Assert.That(r.SteerDegrees, Is.EqualTo(0).Within(1e-9));
            Assert.That(r.GoalPoint.Northing, Is.EqualTo(5).Within(1e-9), "5 m ahead");
            Assert.That(r.GoalPoint.Easting, Is.EqualTo(0).Within(1e-9));
        }

        [Test]
        public void RightOfLine_SteerLeft()
        {
            SteerResult r = Steering.Compute(North, new GeoPoint(0, 1), 0, true, 5);
            Assert.That(r.SteerDegrees, Is.EqualTo(-Atan1In5).Within(1e-6));
        }

        [Test]
        public void AlreadyTurningBack_SteerStraight()
        {
            // 1 m right of the line but already heading at the goal point: hold this heading.
            SteerResult r = Steering.Compute(North, new GeoPoint(0, 1), 360 - Atan1In5, true, 5);
            Assert.That(r.SteerDegrees, Is.EqualTo(0).Within(1e-6));
        }

        [Test]
        public void DrivingBack_GoalPointIsStillAhead()
        {
            // Going south, 1 m east of the line: the line is on the right.
            SteerResult r = Steering.Compute(North, new GeoPoint(0, 1), 180, false, 5);
            Assert.That(r.GoalPoint.Northing, Is.EqualTo(-5).Within(1e-9));
            Assert.That(r.SteerDegrees, Is.EqualTo(Atan1In5).Within(1e-6));
        }

        [Test]
        public void Curve_GoalPointFollowsTheBend()
        {
            // Right-hand circle of radius 100 m round (0, 100), as fine segments.
            var arc = new List<GeoPoint>();
            for (int i = 0; i <= 360; i++)
            {
                double a = Math.PI / 2 * i / 360;
                arc.Add(new GeoPoint(100 * Math.Sin(a), 100 - 100 * Math.Cos(a)));
            }
            // At 45 degrees round, on the curve, heading along it (45 degrees).
            double at = Math.PI / 4;
            var v = new GeoPoint(100 * Math.Sin(at), 100 - 100 * Math.Cos(at));
            SteerResult r = Steering.Compute(arc, v, 45, true, 5);
            // A chord of 5 m on a 100 m circle turns asin(5 / 200) = 1.43 degrees to the right.
            Assert.That(r.SteerDegrees, Is.EqualTo(Math.Asin(5.0 / 200) * 180 / Math.PI).Within(0.05));
            Assert.That(r.GoalPoint.DistanceTo(v), Is.EqualTo(5).Within(0.01));
        }

        [TestCase(0, 3)]
        [TestCase(double.NaN, 3)]
        [TestCase(12, 6)]
        [TestCase(100, 15)]
        public void Lookahead_GrowsWithSpeed(double kmh, double meters)
        {
            Assert.That(Steering.LookaheadMeters(kmh), Is.EqualTo(meters).Within(1e-9));
        }

        [Test]
        public void NoHeading_NoSteer()
        {
            Assert.That(Steering.Compute(North, new GeoPoint(0, 1), double.NaN, true, 5), Is.Null);
        }

        [Test]
        public void Engine_SnapshotHasSteer()
        {
            var plane = new LocalPlane(45.8150, 15.9819);
            GnssFix Fix(double n, double e)
            {
                plane.ToWgs84(new GeoPoint(n, e), out double lat, out double lon);
                return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4, SpeedKmh = 12, HeadingTrue = 0 };
            }

            var engine = new GuidanceEngine { PassWidth = 6 };
            engine.OnFix(Fix(0, 0));
            engine.SetA();
            engine.OnFix(Fix(50, 0));
            engine.SetActiveLine(engine.SetB("AB 1", out _));

            engine.OnFix(Fix(60, 1));
            SteerResult steer = engine.GetSnapshot().Steer;
            Assert.That(steer, Is.Not.Null);
            Assert.That(steer.LookaheadMeters, Is.EqualTo(6).Within(1e-9));
            Assert.That(steer.SteerDegrees, Is.EqualTo(-Math.Atan(1.0 / 6) * 180 / Math.PI).Within(0.05));
        }
    }
}

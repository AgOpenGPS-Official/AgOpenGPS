using System;
using System.Collections.Generic;
using System.IO;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class FieldTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.8150, 15.9819);

        private static GnssFix Fix(double north, double east)
        {
            Plane.ToWgs84(new GeoPoint(north, east), out double lat, out double lon);
            return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4 };
        }

        private static List<GeoPoint> Square(double size)
            => new List<GeoPoint> { new GeoPoint(0, 0), new GeoPoint(0, size), new GeoPoint(size, size), new GeoPoint(size, 0) };

        /// <summary>Drives counter-clockwise round a square (field on the left), a fix every 0.5 m.</summary>
        private static void DriveSquare(GuidanceEngine e, double size, double stopShort = 0)
        {
            var corners = new[] { (0.0, 0.0), (0.0, size), (size, size), (size, 0.0), (0.0, 0.0) }; // (north, east)
            for (int c = 0; c < 4; c++)
            {
                (double n0, double e0) = corners[c];
                (double n1, double e1) = corners[c + 1];
                int steps = (int)(size / 0.5);
                if (c == 3) steps -= (int)(stopShort / 0.5);
                for (int i = 1; i <= steps; i++)
                {
                    double t = (double)i / (size / 0.5);
                    e.OnFix(Fix(n0 + (n1 - n0) * t, e0 + (e1 - e0) * t));
                }
            }
        }

        [Test]
        public void Geometry_AreaContainsDistance()
        {
            List<GeoPoint> sq = Square(100);
            Assert.That(Geometry.Area(sq), Is.EqualTo(10000).Within(1e-6));
            Assert.That(Geometry.Contains(sq, new GeoPoint(50, 50)), Is.True);
            Assert.That(Geometry.Contains(sq, new GeoPoint(50, 101)), Is.False);
            Assert.That(Geometry.DistanceToEdge(sq, new GeoPoint(50, 97)), Is.EqualTo(3).Within(1e-9));
            Assert.That(Geometry.DistanceToEdge(sq, new GeoPoint(50, 104)), Is.EqualTo(4).Within(1e-9));
        }

        [Test]
        public void OffsetClosed_MovesToTheRightOfTravel()
        {
            // Square listed counter-clockwise (east first along the south edge): right is outside.
            var ccw = new List<GeoPoint> { new GeoPoint(0, 0), new GeoPoint(0, 100), new GeoPoint(100, 100), new GeoPoint(100, 0) };
            Assert.That(Geometry.Area(Geometry.OffsetClosed(ccw, 3)), Is.EqualTo(106 * 106).Within(30), "outwards");
            Assert.That(Geometry.Area(Geometry.OffsetClosed(ccw, -3)), Is.EqualTo(94 * 94).Within(30), "inwards");
        }

        [Test]
        public void Boundary_RecordedRoundTheField()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            Assert.That(e.StartBoundary(BoundarySide.Antenna), Is.True);
            DriveSquare(e, 100);

            GuidanceSnapshot rec = e.GetSnapshot();
            Assert.That(rec.RecordingBoundary, Is.True);
            Assert.That(rec.BoundaryRecordedMeters, Is.EqualTo(400).Within(2));

            List<TrackPoint> boundary = e.FinishBoundary(3, out double area, out string error);
            Assert.That(boundary, Is.Not.Null, error);
            Assert.That(area, Is.EqualTo(10000).Within(30));
            Assert.That(boundary.Count, Is.InRange(4, 12), "straight sides are smoothed to their corners");
            Assert.That(e.IsRecordingBoundary, Is.False);

            var field = new Field { Name = "North", Boundary = boundary };
            Assert.That(field.AreaHectares, Is.EqualTo(1.0).Within(0.005));

            e.SetActiveField(field);
            e.OnFix(Fix(50, 97));
            GuidanceSnapshot s = e.GetSnapshot();
            Assert.That(s.InsideField, Is.True);
            Assert.That(s.DistanceToBoundary, Is.EqualTo(3).Within(0.1));
            Assert.That(s.Boundary, Has.Count.EqualTo(boundary.Count));

            e.OnFix(Fix(50, 104));
            Assert.That(e.GetSnapshot().InsideField, Is.False);
        }

        [Test]
        public void Boundary_ImplementSideMovesItByHalfTheWidth()
        {
            // Driving counter-clockwise the field is on the left; with the right end of a 6 m
            // implement on the outer edge, the boundary is 3 m outside the antenna path.
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            e.StartBoundary(BoundarySide.RightOfImplement);
            DriveSquare(e, 100);
            e.FinishBoundary(3, out double area, out string error);
            Assert.That(error, Is.Null);
            Assert.That(area, Is.EqualTo(106 * 106).Within(30));
        }

        [Test]
        public void Boundary_MustCloseAndBeBigEnough()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            e.StartBoundary(BoundarySide.Antenna);
            e.OnFix(Fix(0, 5));
            e.OnFix(Fix(5, 5));
            Assert.That(e.FinishBoundary(3, out _, out string error), Is.Null);
            Assert.That(error, Does.Contain("all the way round"));

            e.CancelBoundary();
            e.OnFix(Fix(0, 0));
            e.StartBoundary(BoundarySide.Antenna);
            DriveSquare(e, 100, stopShort: 60);
            Assert.That(e.FinishBoundary(3, out _, out error), Is.Null);
            Assert.That(error, Does.Contain("60 m to go"));
            Assert.That(e.IsRecordingBoundary, Is.True, "keeps recording so the driver can finish the round");
        }

        [Test]
        public void Fields_AndTheirLines_AreSaved()
        {
            string dir = Path.Combine(Path.GetTempPath(), "aog-fields-" + Guid.NewGuid().ToString("N"));
            try
            {
                string fieldsPath = Path.Combine(dir, "fields.json"), linesPath = Path.Combine(dir, "ablines.json");
                var fields = new FieldStore(fieldsPath);
                var lines = new AbLineStore(linesPath);

                Field north = fields.Create("");
                Assert.That(north.Name, Is.EqualTo("Field 1"));
                Field south = fields.Create("South");
                Assert.That(fields.Active.Id, Is.EqualTo(south.Id));

                fields.SetBoundary(north.Id, new List<TrackPoint> { new TrackPoint(45, 16), new TrackPoint(45.001, 16), new TrackPoint(45.001, 16.001) });
                fields.Rename(north.Id, "North");

                lines.Add(new AbLine { Name = lines.NextName(north.Id), FieldId = north.Id }, true);
                lines.Add(new AbLine { Name = lines.NextName(south.Id), FieldId = south.Id }, false);
                Assert.That(lines.NextName(north.Id), Is.EqualTo("AB 2"));
                Assert.That(lines.NextName(south.Id), Is.EqualTo("AB 2"));

                var fields2 = new FieldStore(fieldsPath);
                Assert.That(fields2.Fields, Has.Count.EqualTo(2));
                Assert.That(fields2.Fields[0].Name, Is.EqualTo("North"), "sorted by name");
                Assert.That(fields2.Fields[0].HasBoundary, Is.True);
                Assert.That(fields2.Active.Name, Is.EqualTo("South"));

                var lines2 = new AbLineStore(linesPath);
                Assert.That(lines2.LinesFor(north.Id), Has.Count.EqualTo(1));
                lines2.DeleteForField(north.Id);
                Assert.That(new AbLineStore(linesPath).LinesFor(north.Id), Is.Empty);
                Assert.That(new AbLineStore(linesPath).Active, Is.Null, "the active line went with its field");
                Assert.That(new AbLineStore(linesPath).LinesFor(south.Id), Has.Count.EqualTo(1));
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class CornerModeTests
    {
        private static readonly LocalPlane Plane = new LocalPlane(45.8150, 15.9819);

        private static GnssFix Fix(double north, double east)
        {
            Plane.ToWgs84(new GeoPoint(north, east), out double lat, out double lon);
            return new GnssFix { Latitude = lat, Longitude = lon, FixQuality = 4 };
        }

        /// <summary>Drives (in steps, not along the edges) to each corner and presses Corner there.</summary>
        private static void MarkCorners(GuidanceEngine e, params (double n, double e)[] corners)
        {
            foreach (var (n, east) in corners)
            {
                e.OnFix(Fix(n * 0.5, east * 0.5 + 7)); // somewhere in between: not part of the boundary
                e.OnFix(Fix(n, east));
                Assert.That(e.AddCorner(), Is.GreaterThan(0));
            }
        }

        [Test]
        public void FourCorners_MakeTheSquare_WithoutDrivingBackToTheStart()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(-20, -20)); // starting somewhere away from the field
            Assert.That(e.StartBoundary(BoundarySide.Antenna, BoundaryMode.Corners), Is.True);
            MarkCorners(e, (0, 0), (0, 100), (100, 100), (100, 0));
            e.OnFix(Fix(60, -30)); // finishing away from the first corner is fine

            GuidanceSnapshot s = e.GetSnapshot();
            Assert.That(s.BoundaryCorners, Is.EqualTo(4));
            Assert.That(s.BoundaryMode, Is.EqualTo(BoundaryMode.Corners));

            List<TrackPoint> b = e.FinishBoundary(3, out double area, out string error);
            Assert.That(b, Is.Not.Null, error);
            Assert.That(b, Has.Count.EqualTo(4), "exactly the corners");
            Assert.That(area, Is.EqualTo(10000).Within(1));
        }

        [Test]
        public void Corners_WithImplementSide_AreMovedOutwards()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            e.StartBoundary(BoundarySide.RightOfImplement, BoundaryMode.Corners);
            MarkCorners(e, (0, 0), (0, 100), (100, 100), (100, 0)); // counter-clockwise: right is outside
            e.FinishBoundary(3, out double area, out string error);
            Assert.That(error, Is.Null);
            Assert.That(area, Is.EqualTo(106 * 106).Within(1), "mitred corners: exact");
        }

        [Test]
        public void DoublePress_Undo_AndTooFewCorners()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            e.StartBoundary(BoundarySide.Antenna, BoundaryMode.Corners);
            Assert.That(e.AddCorner(), Is.EqualTo(1));
            e.OnFix(Fix(0.4, 0));
            Assert.That(e.AddCorner(), Is.EqualTo(-1), "same spot pressed twice");

            e.OnFix(Fix(0, 100));
            Assert.That(e.AddCorner(), Is.EqualTo(2));
            e.OnFix(Fix(30, 40));
            Assert.That(e.AddCorner(), Is.EqualTo(3), "a wrong one");
            Assert.That(e.RemoveLastCorner(), Is.EqualTo(2));

            Assert.That(e.FinishBoundary(3, out _, out string error), Is.Null);
            Assert.That(error, Is.EqualTo("Mark at least 3 corners"));
            Assert.That(e.IsRecordingBoundary, Is.True, "still recording");

            e.OnFix(Fix(100, 100)); e.AddCorner();
            e.OnFix(Fix(100, 0)); e.AddCorner();
            Assert.That(e.FinishBoundary(3, out double area, out error), Is.Not.Null, error);
            Assert.That(area, Is.EqualTo(10000).Within(1), "the undone corner is gone");
        }

        [Test]
        public void DriveMode_IgnoresCornerButtons()
        {
            var e = new GuidanceEngine();
            e.OnFix(Fix(0, 0));
            e.StartBoundary(BoundarySide.Antenna); // drive mode
            Assert.That(e.AddCorner(), Is.EqualTo(-1));
        }
    }
}

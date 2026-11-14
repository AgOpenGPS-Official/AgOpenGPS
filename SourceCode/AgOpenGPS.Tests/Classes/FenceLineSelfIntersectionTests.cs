using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace AgOpenGPS.Tests.Classes
{
    // RemoveSelfIntersections must drop the small loop at a crossing, never the field itself (#1212).
    public class FenceLineSelfIntersectionTests
    {
        // Drive around a 400 x 320 m field starting at a corner, 1 point per meter. The last leg comes
        // back down 1.5 m beside the start point and stops at endNorthing.
        private static CBoundaryList DrivenBoundary(double endNorthing)
        {
            var bnd = new CBoundaryList();
            for (int x = 0; x < 400; x++) bnd.fenceLine.Add(new vec3(x, 0, 0));
            for (int y = 0; y < 320; y++) bnd.fenceLine.Add(new vec3(400, y, 0));
            for (int x = 400; x > 0; x--) bnd.fenceLine.Add(new vec3(x, 320, 0));
            for (double y = 319.6; y > endNorthing; y--) bnd.fenceLine.Add(new vec3(1.5, y, 0));
            return bnd;
        }

        [TestCase(10.0)]  // stops before the start - no crossing
        [TestCase(-1.0)]  // drives just past the start line
        [TestCase(-5.0)]
        [TestCase(-30.0)]
        public void DrivenBoundaryKeepsTheFieldWhenTheEndCrossesTheStart(double endNorthing)
        {
            CBoundaryList bnd = DrivenBoundary(endNorthing);

            bnd.CalculateFenceArea(0);

            // field is 398.5 x 320 m between the first leg and the last leg
            Assert.That(bnd.area, Is.EqualTo(398.5 * 320).Within(400));
            Assert.That(bnd.fenceLine.Count, Is.GreaterThan(1000));
        }

        [Test]
        public void SmallLoopOnASideIsRemoved()
        {
            var bnd = new CBoundaryList();
            for (int x = 0; x < 100; x++) bnd.fenceLine.Add(new vec3(x, 0, 0));
            // a small loop the tractor drove while recording the bottom side
            bnd.fenceLine.Add(new vec3(100, 0, 0));
            bnd.fenceLine.Add(new vec3(103, 3, 0));
            bnd.fenceLine.Add(new vec3(103, -1, 0));
            bnd.fenceLine.Add(new vec3(99.5, 2, 0));
            for (int x = 101; x < 200; x++) bnd.fenceLine.Add(new vec3(x, 0, 0));
            for (int y = 0; y < 200; y++) bnd.fenceLine.Add(new vec3(200, y, 0));
            for (int x = 200; x > 0; x--) bnd.fenceLine.Add(new vec3(x, 200, 0));
            for (int y = 200; y > 0; y--) bnd.fenceLine.Add(new vec3(0, y, 0));
            int rawCount = bnd.fenceLine.Count;

            Assert.That(bnd.RemoveSelfIntersections(), Is.True);

            Assert.That(bnd.fenceLine.Count, Is.LessThan(rawCount));
            Assert.That(Area(bnd.fenceLine), Is.EqualTo(200.0 * 200.0).Within(1));
        }

        [Test]
        public void CrossingAtTheFirstSegmentIsHandledForEveryStartPoint()
        {
            // same shape as the AgOpenGPS.Core GeoPolygon test, every rotation of the start point
            var shape = new List<vec3>
            {
                new vec3(4, 0, 0), new vec3(4, -3, 0), new vec3(-8, 6, 0), new vec3(-8, 0, 0)
            };

            for (int rotate = 0; rotate < shape.Count; rotate++)
            {
                var bnd = new CBoundaryList();
                for (int i = 0; i < shape.Count; i++) bnd.fenceLine.Add(shape[(i + rotate) % shape.Count]);

                bnd.RemoveSelfIntersections();

                Assert.That(Area(bnd.fenceLine), Is.EqualTo(0.5 * 6 * 8).Within(1e-9), "rotation " + rotate);
            }
        }

        private static double Area(List<vec3> pts)
        {
            double sum = 0;
            for (int i = 0, j = pts.Count - 1; i < pts.Count; j = i++)
            {
                sum += (pts[j].easting + pts[i].easting) * (pts[j].northing - pts[i].northing);
            }
            return Math.Abs(sum / 2);
        }
    }
}

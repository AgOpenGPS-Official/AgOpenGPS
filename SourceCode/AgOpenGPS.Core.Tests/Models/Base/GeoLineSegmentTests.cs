using AgOpenGPS.Core.Models;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.Models
{
    public class GeoLineSegmentTests
    {

        [Test]
        public void Test_Intersects()
        {
            GeoCoord coordA = new GeoCoord(13.0, -10.0);
            GeoCoord coordB = new GeoCoord(12.0, -19.0);
            GeoCoord otherCoordA = new GeoCoord(14.0, -18.0);
            GeoCoord otherCoordB = new GeoCoord(-8, -5);
            GeoLineSegment lineSegment = new GeoLineSegment(coordA, coordB);
            GeoLineSegment otherLineSegment = new GeoLineSegment(otherCoordA, otherCoordB);
            GeoCoord? interSectionPoint = lineSegment.IntersectionPoint(otherLineSegment);

            Assert.That(interSectionPoint.HasValue, Is.EqualTo(true));
            // Intersection point must lie on first segment
            Assert.That(
                coordA.Distance(interSectionPoint.Value) + interSectionPoint.Value.Distance(coordB),
                Is.EqualTo(coordA.Distance(coordB))
            );
            // Intersection point must lie on other segment too
            Assert.That(
                otherCoordA.Distance(interSectionPoint.Value) + interSectionPoint.Value.Distance(otherCoordB),
                Is.EqualTo(otherCoordA.Distance(otherCoordB))
            );
        }

        [Test]
        public void Test_NoIntersection()
        {
            GeoCoord coordA = new GeoCoord(13.0, -1);
            GeoCoord coordB = new GeoCoord(18.0, -1);
            GeoCoord otherCoordA = new GeoCoord(-2.0, -18.0);
            GeoCoord otherCoordB = new GeoCoord(-2.0, 100);
            GeoLineSegment northHeadingSegment = new GeoLineSegment(coordA, coordB);
            GeoLineSegment eastHeadingSegment = new GeoLineSegment(otherCoordA, otherCoordB);
            GeoCoord? interSectionPoint = northHeadingSegment.IntersectionPoint(eastHeadingSegment);

            Assert.That(interSectionPoint.HasValue, Is.EqualTo(false));
        }

        [Test]
        public void Test_SymetricalSegmentsCrossInTheMiddle()
        {
            const double minNorthing = -1.0;
            const double maxNorthing = 3.0;
            const double minEasting = 2.0;
            const double maxEasting = 4.0;
            GeoCoord neCoord = new GeoCoord(maxNorthing, maxEasting);
            GeoCoord seCoord = new GeoCoord(minNorthing, maxEasting);
            GeoCoord swCoord = new GeoCoord(minNorthing, minEasting);
            GeoCoord nwCoord = new GeoCoord(maxNorthing, minEasting);
            GeoLineSegment nwseLineSegment = new GeoLineSegment(nwCoord, seCoord);
            GeoLineSegment swneLineSegment = new GeoLineSegment(swCoord, neCoord);
            GeoCoord? interSectionPoint = nwseLineSegment.IntersectionPoint(swneLineSegment);

            // Assert
            Assert.That(interSectionPoint.HasValue, Is.EqualTo(true));
            Assert.That(interSectionPoint.Value.Distance(nwCoord), Is.EqualTo(interSectionPoint.Value.Distance(seCoord)));
        }

        [Test]
        public void Test_ParallelSegmentsDoNotIntersect()
        {
            const double minNorthing = -1.0;
            const double maxNorthing = 3.0;
            const double minEasting = 2.0;
            const double maxEasting = 4.0;
            GeoCoord seCoord = new GeoCoord(minNorthing, maxEasting);
            GeoCoord nwCoord = new GeoCoord(maxNorthing, minEasting);
            GeoDelta shift = new GeoDelta(1.0, 0.0);
            GeoLineSegment nwseLineSegment = new GeoLineSegment(nwCoord, seCoord);
            GeoLineSegment shiftedSegment = new GeoLineSegment(nwCoord + shift, seCoord + shift);
            GeoCoord? intersectionPoint = nwseLineSegment.IntersectionPoint(shiftedSegment);

            Assert.That(intersectionPoint, Is.EqualTo(null));
        }

        [Test]
        public void Test_LongLineSegment()
        {
            const double minNorthing = -1.0;
            const double maxNorthing = 3.0;
            const double minEasting = 2.0;
            const double maxEasting = 4.0;
            GeoCoord seCoord = new GeoCoord(minNorthing, maxEasting);
            GeoCoord nwCoord = new GeoCoord(maxNorthing, minEasting);

            GeoDelta delta = new GeoDelta(nwCoord, seCoord);
            GeoLineSegment nwseLineSegment = new GeoLineSegment(nwCoord, nwCoord + 1000.0 * delta);
            GeoCoord almostEnd = nwCoord + 999.0 * delta;

            GeoLineSegment otherSegment = new GeoLineSegment(
                almostEnd - 1.0 * new GeoDir(delta).PerpendicularLeft,
                almostEnd + 1.0 * new GeoDir(delta).PerpendicularLeft);
            GeoCoord? intersectionPoint = nwseLineSegment.IntersectionPoint(otherSegment);

            Assert.That(intersectionPoint.HasValue, Is.EqualTo(true));
            Assert.That(intersectionPoint.Value.Northing, Is.EqualTo(almostEnd.Northing));
            Assert.That(intersectionPoint.Value.Easting, Is.EqualTo(almostEnd.Easting));
        }

        [Test]
        public void Test_SameEndPoints()
        {
            GeoCoord coordA = new GeoCoord(16.88, -15.488);
            GeoCoord otherCoordA = new GeoCoord(-13.355, 16.09);
            GeoCoord sharedEndPoint = new GeoCoord(16.99, -13.55);
            GeoLineSegment segment = new GeoLineSegment(coordA, sharedEndPoint);
            GeoLineSegment otherSegment = new GeoLineSegment(otherCoordA, sharedEndPoint);
            GeoLineSegment reversedSegment = new GeoLineSegment(sharedEndPoint, coordA);
            GeoLineSegment reversedOtherSegment = new GeoLineSegment(sharedEndPoint, otherCoordA);

            GeoCoord? intersectionPoint = segment.IntersectionPoint(otherSegment);
            Assert.That(intersectionPoint.HasValue, Is.EqualTo(true));
            Assert.That(intersectionPoint.Value, Is.EqualTo(sharedEndPoint));

            GeoCoord? ipNormalReversed = segment.IntersectionPoint(reversedOtherSegment);
            Assert.That(ipNormalReversed.HasValue, Is.EqualTo(true));
            Assert.That(ipNormalReversed.Value, Is.EqualTo(sharedEndPoint));

            GeoCoord? ipReversedNormal = reversedSegment.IntersectionPoint(otherSegment);
            Assert.That(ipReversedNormal.HasValue, Is.EqualTo(true));
            Assert.That(ipReversedNormal.Value, Is.EqualTo(sharedEndPoint));

            GeoCoord? ipReversedReversed = reversedSegment.IntersectionPoint(reversedOtherSegment);
            Assert.That(ipReversedReversed.HasValue, Is.EqualTo(true));
            Assert.That(ipReversedReversed.Value, Is.EqualTo(sharedEndPoint));
        }
    }
}

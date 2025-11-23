using Accord;
using Accord.Math;
using AgOpenGPS.Core.Models;
using NUnit.Framework;
using System;

namespace AgOpenGPS.Core.Tests.Models
{
    public class GeoPolygonTests
    {
        readonly double _minNorthing = -1.0;
        readonly double _maxNorthing = 3.0;
        readonly double _minEasting = 2.0;
        readonly double _maxEasting = 4.0;
        private GeoPolygon _cwPolygon;


        [SetUp]
        public void SetUp()
        {
            GeoCoord neCoord = new GeoCoord(_maxNorthing, _maxEasting);
            GeoCoord seCoord = new GeoCoord(_minNorthing, _maxEasting);
            GeoCoord swCoord = new GeoCoord(_minNorthing, _minEasting);
            GeoCoord nwCoord = new GeoCoord(_maxNorthing, _minEasting);
            _cwPolygon = new GeoPolygon();
            _cwPolygon.Add(neCoord);
            _cwPolygon.Add(seCoord);
            _cwPolygon.Add(swCoord);
            _cwPolygon.Add(nwCoord);
        }


        [Test]
        public void Test_IsClockwise()
        {
            // Act
            bool isClockwise = _cwPolygon.IsClockwise;

            // Assert
            Assert.That(isClockwise, Is.EqualTo(true));
        }

        [Test]
        public void Test_ClockwiseArea()
        {
            // Act
            double area = _cwPolygon.Area;

            // Assert
            Assert.That(area, Is.EqualTo((_maxNorthing - _minNorthing) * (_maxEasting - _minEasting)));
        }

        [Test]
        public void Test_BoundingBox()
        {
            // Act
            GeoBoundingBox bb = _cwPolygon.BoundingBox;

            // Assert
            Assert.That(bb.MinNorthing, Is.EqualTo(_minNorthing));
            Assert.That(bb.MaxNorthing, Is.EqualTo(_maxNorthing));
            Assert.That(bb.MinEasting, Is.EqualTo(_minEasting));
            Assert.That(bb.MaxEasting, Is.EqualTo(_maxEasting));
        }

        [Test]
        public void Test_ForceWinding()
        {
            GeoPolygon cwPolygon = CopyPolygon(_cwPolygon);
            double cwArea = cwPolygon.Area;

            // Assert
            Assert.That(cwPolygon.IsClockwise, Is.EqualTo(true));
            cwPolygon.ForceCounterClockwiseWinding();
            Assert.That(cwPolygon.IsClockwise, Is.EqualTo(false));
            Assert.That(cwPolygon.Area, Is.EqualTo(cwArea));

            // Test no changes after ForceCounterClockwiseWinding when already CCW
            cwPolygon.ForceCounterClockwiseWinding();
            Assert.That(cwPolygon.IsClockwise, Is.EqualTo(false));
            Assert.That(cwPolygon.Area, Is.EqualTo(cwArea));

            // Test changes after ForceClockwiseWinding when CCW
            cwPolygon.ForceClockwiseWinding();
            Assert.That(cwPolygon.IsClockwise, Is.EqualTo(true));
            Assert.That(cwPolygon.Area, Is.EqualTo(cwArea));

            // Test no changes after ForceClockwiseWinding when already CW
            cwPolygon.ForceClockwiseWinding();
            Assert.That(cwPolygon.IsClockwise, Is.EqualTo(true));
            Assert.That(cwPolygon.Area, Is.EqualTo(cwArea));
        }

        [Test]
        public void Test_InvalidateArea()
        {
            GeoPolygon cwPolygon = CopyPolygon(_cwPolygon);
            double orgArea = cwPolygon.Area;

            GeoCoord farNorth = new GeoCoord(10.0, 0.0);
            cwPolygon.Add(farNorth);

            // Assert
            Assert.That(cwPolygon.Area, Is.GreaterThan(orgArea));
        }

        [Test]
        public void Test_InvalidateBoundingBox()
        {
            GeoPolygon cwPolygon = CopyPolygon(_cwPolygon);
            double orgMaxNorthing = cwPolygon.BoundingBox.MaxNorthing;

            GeoCoord farNorth = new GeoCoord(10.0, 0.0);
            cwPolygon.Add(farNorth);

            // Assert
            Assert.That(cwPolygon.BoundingBox.MaxNorthing, Is.GreaterThan(orgMaxNorthing));
        }

        [Test]
        public void Test_GetLength()
        {
            const int nVertices = 120;
            const double radius = 100.0;
            GeoPolygon polygon = new GeoPolygon();
            for (int i = 0; i < nVertices; i++)
            {
                double angle = i * 2.0 * Math.PI / nVertices;
                polygon.Add(new GeoCoord(radius * Math.Cos(angle), radius * Math.Sin(angle)));
            }
            // east, south, west and north half circle
            double eLength = polygon.GetLength(0 * nVertices / 4, 2 * nVertices / 4 + 1);
            double sLength = polygon.GetLength(1 * nVertices / 4, 3 * nVertices / 4 + 1);
            double wLength = polygon.GetLength(2 * nVertices / 4, 0 * nVertices / 4 + 1);
            double nLength = polygon.GetLength(3 * nVertices / 4, 1 * nVertices / 4 + 1);

            Assert.That(eLength.IsGreaterThan(3.1 * radius));
            Assert.That(eLength.IsLessThan(Math.PI * radius));
            Assert.That(sLength.IsGreaterThan(3.1 * radius));
            Assert.That(sLength.IsLessThan(Math.PI * radius));
            Assert.That(wLength.IsGreaterThan(3.1 * radius));
            Assert.That(wLength.IsLessThan(Math.PI * radius));
            Assert.That(nLength.IsGreaterThan(3.1 * radius));
            Assert.That(nLength.IsLessThan(Math.PI * radius));
        }

        [Test]
        public void Test_RemoveSelfIntersections()
        {
            GeoPolygon polygon = new GeoPolygon();
            polygon.Add(new GeoCoord(0, 1));
            polygon.Add(new GeoCoord(0, 2));
            polygon.Add(new GeoCoord(1, 3));
            polygon.Add(new GeoCoord(0, 3));
            polygon.Add(new GeoCoord(1, 2));
            polygon.Add(new GeoCoord(1, 1));
            polygon.Add(new GeoCoord(0, 0));
            polygon.Add(new GeoCoord(1, 0));

            // Make a copy with rotated vertices, to test if it also works
            // if the intersecting segments are at index 0, Count, Count -1 etc
            for (int rotate = 0; rotate < polygon.Count; rotate++)
            {
                GeoPolygon p = new GeoPolygon();
                for (int i = 0; i < polygon.Count; i++)
                {
                    p.Add(polygon[(i + rotate) % polygon.Count]);
                }
                int nInterSection = p.RemoveSelfIntersections();
                Assert.That(nInterSection, Is.EqualTo(2));
                Assert.That(p.Area, Is.EqualTo(1.5));
            }
        }

        private static GeoPolygon CopyPolygon(GeoPolygon p)
        {
            GeoPolygon copy = new GeoPolygon();
            for (int i = 0; i < p.Count; i++)
            {
                copy.Add(p[i]);
            }
            return copy;
        }

    }
}

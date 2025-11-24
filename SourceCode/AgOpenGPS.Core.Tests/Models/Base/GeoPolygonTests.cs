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

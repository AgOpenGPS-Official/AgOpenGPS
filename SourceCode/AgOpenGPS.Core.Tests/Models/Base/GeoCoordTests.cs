using AgOpenGPS.Core.Models;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.Models
{
    public class GeoCoordTests
    {
        [Test]
        public void DistanceSquared_ShouldBeCorrect()
        {
            // Arrange
            GeoCoord coord1 = new GeoCoord(0.0, 3.0);
            GeoCoord coord2 = new GeoCoord(4.0, 0.0);

            // Act
            double result = coord1.DistanceSquared(coord2);

            // Assert
            Assert.That(result, Is.EqualTo(25.0));
        }

        [Test]
        public void Distance_ShouldBeCorrect()
        {
            // Arrange
            GeoCoord coord1 = new GeoCoord(0.0, 3.0);
            GeoCoord coord2 = new GeoCoord(4.0, 0.0);

            // Act
            double result = coord1.Distance(coord2);

            // Assert
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void Test_PlusMinusDelta()
        {
            // Arrange
            GeoCoord coord1 = new GeoCoord(19.0, 67.0);
            GeoDelta deltaN = new GeoDelta(3.0, 0.0);
            GeoDelta deltaE = new GeoDelta(0.0, 40.0);

            // Act
            GeoCoord plusNE = coord1 + deltaN + deltaE;
            GeoCoord plusEN = coord1 + deltaE + deltaN;
            GeoCoord plusNorthMinusEast = coord1 + deltaN - deltaE;
            GeoCoord minusEastPlusNorth = coord1 - deltaE + deltaN;

            // Assert
            Assert.That(plusNE.Northing, Is.EqualTo(22.0));
            Assert.That(plusNE.Easting, Is.EqualTo(107.0));
            Assert.That(plusEN.Northing, Is.EqualTo(22.0));
            Assert.That(plusEN.Easting, Is.EqualTo(107.0));

            Assert.That(plusNorthMinusEast.Northing, Is.EqualTo(22.0));
            Assert.That(plusNorthMinusEast.Easting, Is.EqualTo(27.0));
            Assert.That(minusEastPlusNorth.Northing, Is.EqualTo(22.0));
            Assert.That(minusEastPlusNorth.Easting, Is.EqualTo(27.0));
        }
    }
}

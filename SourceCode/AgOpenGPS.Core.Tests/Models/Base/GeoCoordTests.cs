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
    }
}

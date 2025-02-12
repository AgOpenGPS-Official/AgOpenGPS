using AgOpenGPS.Core.Models;
using NUnit.Framework;
using System;

namespace AgOpenGPS.Core.Tests.Models
{
    public class Wgs84Tests
    {
        [Test]
        public void Test_RegionalDistance()
        {
            // Arrange
            Wgs84 amsterdam = new Wgs84(52.377956, 4.897070);
            Wgs84 denDungen = new Wgs84(51.665, 5.37222);

            // Act
            double distance = amsterdam.Distance(denDungen);
            double distanceBack = denDungen.Distance(amsterdam);

            // Assert
            Assert.That(distance, Is.GreaterThan(85000));
            Assert.That(distance, Is.LessThan(86000));

            Assert.That(distance, Is.EqualTo(distanceBack));
        }

        [Test]
        public void Test_LongDistance()
        {
            // Arrange
            Wgs84 amsterdam = new Wgs84(52.377956, 4.897070);
            Wgs84 opposite = new Wgs84(-52.377956, 4.897070 - 180);

            // Act
            double distance = amsterdam.Distance(opposite);
            double distanceBack = opposite.Distance(amsterdam);

            // Assert
            Assert.That(distance, Is.GreaterThan(20010 * 1000));
            Assert.That(distance, Is.LessThan(20020 * 1000));

            Assert.That(distance, Is.EqualTo(distanceBack));
        }


        [Test]
        public void Test_LocalOffsets()
        {
            // Arrange
            Wgs84 amsterdam = new Wgs84(52.377956, 4.897070);
            Wgs84 amsterdamNorth = new Wgs84(amsterdam.Latitude + 0.01, amsterdam.Longitude);
            Wgs84 amsterdamSouth = new Wgs84(amsterdam.Latitude - 0.01, amsterdam.Longitude);
            Wgs84 amsterdamEast = new Wgs84(amsterdam.Latitude, amsterdam.Longitude + 0.01);
            Wgs84 amsterdamWest = new Wgs84(amsterdam.Latitude, amsterdam.Longitude - 0.01);

            // Act
            double distanceNorth = amsterdam.Distance(amsterdamNorth);
            double distanceSouth = amsterdam.Distance(amsterdamSouth);
            double distanceEast = amsterdam.Distance(amsterdamEast);
            double distanceWest = amsterdam.Distance(amsterdamWest);

            // Assert
            Assert.That(Math.Abs(distanceNorth - distanceSouth), Is.LessThan(0.001));
            Assert.That(Math.Abs(distanceEast - distanceWest), Is.LessThan(0.001));

            Assert.That(distanceNorth, Is.GreaterThan(distanceEast));
        }
    }
}

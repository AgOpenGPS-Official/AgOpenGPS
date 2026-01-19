using AgOpenGPS.Core.Models;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.Models
{
    public class GeoBoundingBoxTests
    {
        [Test]
        public void Test_CenterCoord()
        {
            // Arrange
            GeoCoord coord1 = new GeoCoord(-10.0, 3.0);
            GeoCoord coord2 = new GeoCoord(-20.0, 30.0);
            GeoBoundingBox bb = GeoBoundingBox.CreateEmpty();
            bb.Include(coord1);
            bb.Include(coord2);

            // Act
            GeoCoord centerCoord = bb.CenterCoord;

            // Assert
            GeoCoord correctCenterCoord = new GeoCoord(-15.0, 16.5);
            Assert.That(centerCoord, Is.EqualTo(correctCenterCoord));
        }

        [Test]
        public void Test_IncludeBoundingBox_NotEmpty_NotEmpty()
        {
            // Arrange
            GeoCoord coordA1 = new GeoCoord(-10.0, 20.0);
            GeoCoord coordA2 = new GeoCoord(-30.0, 40.0);
            GeoBoundingBox bbA = GeoBoundingBox.CreateEmpty();
            bbA.Include(coordA1);
            bbA.Include(coordA2);

            GeoCoord coordB1 = new GeoCoord(-20.0, 30.0);
            GeoCoord coordB2 = new GeoCoord(-40.0, 50.0);
            GeoBoundingBox bbB = GeoBoundingBox.CreateEmpty();
            bbA.Include(coordB1);
            bbA.Include(coordB2);

            // Act
            bbA.Include(bbB);

            // Assert
            Assert.That(bbA.IsEmpty, Is.False);
            Assert.That(bbA.MinNorthing, Is.EqualTo(-40.0));
            Assert.That(bbA.MaxNorthing, Is.EqualTo(-10.0));
            Assert.That(bbA.MinEasting, Is.EqualTo(20.0));
            Assert.That(bbA.MaxEasting, Is.EqualTo(50.0));
        }

        [Test]
        public void Test_IncludeBoundingBox_NotEmpty_Empty()
        {
            // Arrange
            GeoCoord coordA1 = new GeoCoord(-10.0, 20.0);
            GeoCoord coordA2 = new GeoCoord(-30.0, 40.0);
            GeoBoundingBox bbA = GeoBoundingBox.CreateEmpty();
            bbA.Include(coordA1);
            bbA.Include(coordA2);
            GeoBoundingBox bbB = GeoBoundingBox.CreateEmpty();

            // Act
            bbA.Include(bbB);

            // Assert
            Assert.That(bbA.IsEmpty, Is.False);
            Assert.That(bbA.MinNorthing, Is.EqualTo(-30.0));
            Assert.That(bbA.MaxNorthing, Is.EqualTo(-10.0));
            Assert.That(bbA.MinEasting, Is.EqualTo(20.0));
            Assert.That(bbA.MaxEasting, Is.EqualTo(40.0));
        }

        [Test]
        public void Test_IncludeBoundingBox_Empty_NotEmpty()
        {
            // Arrange
            GeoBoundingBox bbA = GeoBoundingBox.CreateEmpty();
            GeoCoord coordB1 = new GeoCoord(-20.0, 30.0);
            GeoCoord coordB2 = new GeoCoord(-40.0, 50.0);
            GeoBoundingBox bbB = GeoBoundingBox.CreateEmpty();
            bbA.Include(coordB1);
            bbA.Include(coordB2);

            // Act
            bbA.Include(bbB);

            // Assert
            Assert.That(bbA.IsEmpty, Is.False);
            Assert.That(bbA.MinNorthing, Is.EqualTo(-40.0));
            Assert.That(bbA.MaxNorthing, Is.EqualTo(-20.0));
            Assert.That(bbA.MinEasting, Is.EqualTo(30.0));
            Assert.That(bbA.MaxEasting, Is.EqualTo(50.0));
        }

        [Test]
        public void Test_IncludeBoundingBox_Empty_Empty()
        {
            // Arrange
            GeoBoundingBox bbA = GeoBoundingBox.CreateEmpty();
            GeoBoundingBox bbB = GeoBoundingBox.CreateEmpty();

            // Act
            bbA.Include(bbB);

            // Assert
            Assert.That(bbA.IsEmpty, Is.True);
        }

        [Test]
        public void Test_Scaled()
        {
            // Arrange
            GeoBoundingBox bb = GeoBoundingBox.CreateEmpty();
            bb.Include(new GeoCoord(5.0, -3.0));
            bb.Include(new GeoCoord(9.0, -5.0));

            // Act
            GeoBoundingBox scaledBb = bb.Scaled(1.3, 1.5);

            // Assert
            Assert.That(scaledBb.MinNorthing, Is.EqualTo(4.4));
            Assert.That(scaledBb.MaxNorthing, Is.EqualTo(9.60));
            Assert.That(scaledBb.MinEasting, Is.EqualTo(-5.5));
            Assert.That(scaledBb.MaxEasting, Is.EqualTo(-2.5));
        }
    }
}

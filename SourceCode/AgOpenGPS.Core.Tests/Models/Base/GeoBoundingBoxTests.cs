using AgOpenGPS.Core.Models;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.Models
{
    public class GeoBoundingBoxTests
    {
        [Test]
        public void Test_CenterCoord()
        {
            GeoCoord coord1 = new GeoCoord(-10.0, 3.0);
            GeoCoord coord2 = new GeoCoord(-20.0, 30.0);

            GeoBoundingBox bb = GeoBoundingBox.CreateEmpty();
            bb.Include(coord1);
            bb.Include(coord2);
            GeoCoord centerCoord = bb.CenterCoord;

            GeoCoord correctCenterCoord = new GeoCoord(-15.0, 16.5);
            Assert.That(centerCoord, Is.EqualTo(correctCenterCoord));
        }

    }
}

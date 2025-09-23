// AgOpenGPS.Core.Tests/Math/Distance2DTests.cs
// Purpose: Smoke tests for Distance2D helpers (NUnit).
using AgOpenGPS.Core.Mathx;
using AgOpenGPS.Core.Models;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.Mathx
{
    [TestFixture]
    public class Distance2DTests
    {
        [Test]
        public void Distance_DirectVsGeoCoord_Agree()
        {
            var a = new GeoCoord(0.0, 0.0);   // (northing, easting)
            var b = new GeoCoord(3.0, 4.0);   // distance should be 5.0

            var d1 = Distance2D.Distance(a.Easting, a.Northing, b.Easting, b.Northing);
            var d2 = Distance2D.Distance(a, b);

            Assert.That(d1, Is.EqualTo(5.0).Within(1e-12));
            Assert.That(d2, Is.EqualTo(5.0).Within(1e-12));
        }

        [Test]
        public void DistanceSquared_Matches_SquareOfDistance()
        {
            var a = new GeoCoord(10.0, 10.0);
            var b = new GeoCoord(13.0, 14.0);

            var d = Distance2D.Distance(a, b);
            var ds = Distance2D.DistanceSquared(a, b);

            Assert.That(ds, Is.EqualTo(d * d).Within(1e-12));
        }
    }
}

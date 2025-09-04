// Purpose: Unit test ensuring Positioning footprint compiles and instantiates (NUnit).
using NUnit.Framework;
using AgOpenGPS.Core.Positioning;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Tests.Positioning
{
    [TestFixture]
    public class PositioningTests
    {
        [Test]
        public void Step_NoOp_ForwardsFix()
        {
            var engine = new PositioningEngine();

            var inputs = new SensorInputs
            {
                RawFixLocal = new GeoCoord(20.0, 10.0),
                SpeedKmh = 15.0,
                HeadingSource = HeadingSource.Fix2Fix,
                DeltaTimeSeconds = 0.05,
                GpsHz = 20.0
            };

            var outputs = engine.Step(inputs);

            Assert.That(outputs.CorrectedFixLocal.Easting, Is.EqualTo(10.0).Within(1e-9));
            Assert.That(outputs.CorrectedFixLocal.Northing, Is.EqualTo(20.0).Within(1e-9));
            Assert.That(outputs.FixHeadingRad, Is.EqualTo(0.0).Within(1e-12));
            Assert.That(outputs.ReverseStatus, Is.EqualTo(ReverseStatus.Unknown));
        }
    }
}

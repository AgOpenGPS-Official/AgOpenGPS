// Purpose: Smoke test ensuring Positioning footprint compiles and instantiates.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using AgOpenGPS.Core.Positioning;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Tests.Positioning
{
    [TestClass]
    public class PositioningFootprintTests
    {
        [TestMethod]
        public void Step_NoOp_ForwardsFix()
        {
            var engine = new PositioningEngine();

            var inputs = new SensorInputs
            {
                RawFixLocal = new vec2(10.0, 20.0),
                SpeedKmh = 15.0,
                HeadingSource = HeadingSource.Fix2Fix,
                DeltaTimeSeconds = 0.05,
                GpsHz = 20.0
            };

            var outputs = engine.Step(inputs);

            Assert.AreEqual(10.0, outputs.CorrectedFixLocal.easting, 1e-9);
            Assert.AreEqual(20.0, outputs.CorrectedFixLocal.northing, 1e-9);
            Assert.AreEqual(0.0, outputs.FixHeadingRad, 1e-12);
            Assert.AreEqual(ReverseStatus.Unknown, outputs.ReverseStatus);
        }
    }
}

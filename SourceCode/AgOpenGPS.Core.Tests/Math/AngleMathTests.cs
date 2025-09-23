// AgOpenGPS.Core.Tests/Math/AngleMathTests.cs
// Purpose: Smoke tests for AngleMath helpers (NUnit).
using AgOpenGPS.Core.Mathx;
using NUnit.Framework;
using System;

namespace AgOpenGPS.Core.Tests.Mathx
{
    [TestFixture]
    public class AngleMathTests
    {
        [Test]
        public void ToRadians_ToDegrees_Roundtrip()
        {
            var deg = 123.45;
            var rad = AngleMath.ToRadians(deg);
            var back = AngleMath.ToDegrees(rad);
            Assert.That(back, Is.EqualTo(deg).Within(1e-12));
        }

        [Test]
        public void NormalizePositive_WrapsInto_0_2Pi()
        {
            Assert.That(AngleMath.NormalizePositive(0.0), Is.EqualTo(0.0).Within(1e-12));
            Assert.That(AngleMath.NormalizePositive(-0.1), Is.GreaterThan(6.18)); // near 2π
            Assert.That(AngleMath.NormalizePositive(7.0), Is.LessThan(1.0));      // wrapped
        }

        [Test]
        public void ShortestDelta_IsSignedWithinMinusPiToPi()
        {
            double from = 0.0;
            double to = Math.PI * 1.5; // 270 deg
            var d = AngleMath.ShortestDelta(from, to);
            Assert.That(d, Is.LessThan(0.0)); // rotate -90 deg is shortest
            Assert.That(d, Is.EqualTo(-Math.PI / 2.0).Within(1e-12));
        }

        [Test]
        public void AngleDiff_AbsoluteWithin_0_Pi()
        {
            var d1 = AngleMath.AngleDiff(0, Math.PI / 2);
            var d2 = AngleMath.AngleDiff(AngleMath.TwoPi - 0.1, 0.1);
            Assert.That(d1, Is.EqualTo(Math.PI / 2).Within(1e-12));
            Assert.That(d2, Is.EqualTo(0.2).Within(1e-12));
        }
    }
}

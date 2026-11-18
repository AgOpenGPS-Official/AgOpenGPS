using System;
using System.Collections.Generic;
using System.Linq;
using AgOpenGPS.Mobile.Core;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class AlertTests
    {
        private DateTime _t;
        private AlertMonitor _m;
        private AlertSettings _settings;

        [SetUp]
        public void SetUp()
        {
            _t = new DateTime(2026, 11, 18, 10, 0, 0, DateTimeKind.Utc);
            _settings = new AlertSettings();
            _settings.Enabled.Add(AlertKind.OffLine);
            _m = new AlertMonitor(_settings);
        }

        /// <summary>Feeds the same state for <paramref name="seconds"/> at 4 Hz and returns everything that fired.</summary>
        private List<AlertKind> Run(double seconds, Action<AlertInputs> state)
        {
            var all = new List<AlertKind>();
            for (double s = 0; s < seconds; s += 0.25)
            {
                _t = _t.AddSeconds(0.25);
                var i = new AlertInputs { Now = _t, Receiving = true, FixQuality = 4, InsideField = true, DistanceToBoundary = 50 };
                state(i);
                all.AddRange(_m.Update(i));
            }
            return all;
        }

        [Test]
        public void RtkLost_AfterAMoment_Once_ThenBack()
        {
            Assert.That(Run(2, _ => { }), Is.Empty, "RTK all along");
            Assert.That(Run(1.0, i => i.FixQuality = 5), Is.Empty, "a short flicker is ignored");
            Assert.That(Run(0.5, _ => { }), Is.Empty);

            List<AlertKind> lost = Run(5, i => i.FixQuality = 5);
            Assert.That(lost, Is.EqualTo(new[] { AlertKind.RtkLost }), "once, not every sample");
            Assert.That(Run(5, i => i.FixQuality = 1), Is.Empty, "Float to GPS is still the same loss");
            Assert.That(Run(3, _ => { }), Is.EqualTo(new[] { AlertKind.RtkBack }));
        }

        [Test]
        public void NoRtkLost_IfThereWasNeverRtk()
        {
            Assert.That(Run(10, i => i.FixQuality = 5), Is.Empty);
        }

        [Test]
        public void FieldEdge_Once_UntilClearlyAway()
        {
            Assert.That(Run(1, i => i.DistanceToBoundary = 4.5), Is.EqualTo(new[] { AlertKind.FieldEdge }));
            Assert.That(Run(1, i => i.DistanceToBoundary = 5.5), Is.Empty, "wobbling at the edge: no new alert");
            Assert.That(Run(1, i => i.DistanceToBoundary = 4.8), Is.Empty);
            Assert.That(Run(1, i => i.DistanceToBoundary = 20), Is.Empty, "turned away: re-armed");
            Assert.That(Run(1, i => i.DistanceToBoundary = 3), Is.EqualTo(new[] { AlertKind.FieldEdge }), "next headland");
        }

        [Test]
        public void LeavingTheField()
        {
            Run(1, i => i.DistanceToBoundary = 1);
            Assert.That(Run(1, i => { i.InsideField = false; i.DistanceToBoundary = 1; }), Is.EqualTo(new[] { AlertKind.LeftField }));
            Assert.That(Run(1, i => i.InsideField = false), Is.Empty, "staying outside: once");
        }

        [Test]
        public void CorrectionsStopped_OnlyAfterTheyWereComing()
        {
            Assert.That(Run(1, i => { i.NtripActive = true; i.SecondsSinceCorrection = double.NaN; }), Is.Empty, "never had any");
            Run(1, i => { i.NtripActive = true; i.SecondsSinceCorrection = 1; });
            Assert.That(Run(1, i => { i.NtripActive = true; i.SecondsSinceCorrection = 10; }), Is.Empty);
            Assert.That(Run(1, i => { i.NtripActive = true; i.SecondsSinceCorrection = 16; }), Is.EqualTo(new[] { AlertKind.CorrectionsStopped }));
            Assert.That(Run(1, i => { i.NtripActive = true; i.SecondsSinceCorrection = 40; }), Is.Empty, "once");
        }

        [Test]
        public void LinkLost_AfterThreeSeconds_Once()
        {
            Run(1, _ => { });
            Assert.That(Run(2, i => { i.Receiving = false; i.FixQuality = -1; }), Is.Empty, "short gap");
            Assert.That(Run(2, i => { i.Receiving = false; i.FixQuality = -1; }), Is.EqualTo(new[] { AlertKind.LinkLost }));
            Assert.That(Run(5, i => { i.Receiving = false; i.FixQuality = -1; }), Is.Empty, "once");
        }

        [Test]
        public void OffLine_WhenMovingForTwoSeconds()
        {
            Assert.That(Run(5, i => { i.CrossTrackMeters = 0.3; i.SpeedKmh = 0; }), Is.Empty, "standing still");
            Assert.That(Run(1, i => { i.CrossTrackMeters = 0.3; i.SpeedKmh = 8; }), Is.Empty, "not long enough");
            Assert.That(Run(2, i => { i.CrossTrackMeters = 0.3; i.SpeedKmh = 8; }), Is.EqualTo(new[] { AlertKind.OffLine }));
            Assert.That(Run(1, i => { i.CrossTrackMeters = 0.15; i.SpeedKmh = 8; }), Is.Empty, "not re-armed above 60 %");
            Assert.That(Run(3, i => { i.CrossTrackMeters = 0.3; i.SpeedKmh = 8; }), Is.Empty);
            Run(1, i => { i.CrossTrackMeters = 0.05; i.SpeedKmh = 8; });
            Assert.That(Run(3, i => { i.CrossTrackMeters = -0.3; i.SpeedKmh = 8; }), Is.EqualTo(new[] { AlertKind.OffLine }), "re-armed back on the line");
        }

        [Test]
        public void SwitchedOffAlerts_StaySilent()
        {
            _settings.Enabled.Remove(AlertKind.FieldEdge);
            Assert.That(Run(1, i => i.DistanceToBoundary = 2), Is.Empty);
        }
    }
}

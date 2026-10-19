using System.Collections.Generic;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class SupportReportTests
    {
        private static NtripStatus Status()
        {
            var types = new List<RtcmTypeStat>
            {
                new RtcmTypeStat(1005, 14, 436, 3.1, 10.0),
                new RtcmTypeStat(1074, 143, 61490, 0.4, 1.0),
            };
            return new NtripStatus
            {
                State = NtripState.Streaming,
                BytesReceived = 61926,
                Frames = 157,
                FirstLine = "ICY 200 OK",
                VersionUsed = NtripVersion.V2,
                SecondsSinceLastFrame = 0.4,
                Rtcm = new RtcmStreamSnapshot(157, 0, 0, 120, types, 7),
            };
        }

        [Test]
        public void Report_HasTheEssentials_AndNeverThePassword()
        {
            var profile = new NtripProfile("caster.example.org", 2101, "MOUNT", "farmer@example.org", "TopSecret99");
            var status = Status();
            string report = SupportReport.Build(status, profile, CorrectionChecks.Build(status, 4, 1.2));

            Assert.That(report, Does.Contain("status: Corrections coming in (Streaming)"));
            Assert.That(report, Does.Contain("service: caster.example.org:2101, mountpoint: MOUNT"));
            Assert.That(report, Does.Contain("user: farmer@example.org, password: set"));
            Assert.That(report, Does.Not.Contain("TopSecret99"));
            Assert.That(report, Does.Contain("base station ID: 7"));
            Assert.That(report, Does.Contain("service answered: ICY 200 OK"));
            Assert.That(report, Does.Contain("[Ok] Your GPS: RTK fixed"));
            Assert.That(report, Does.Contain("1074"));
            Assert.That(report, Does.Contain("GPS observations (MSM4)"));
        }

        [Test]
        public void Report_WorksWithoutAProfile_OrAnyData()
        {
            var status = new NtripStatus { State = NtripState.Connecting, Rtcm = new RtcmStreamSnapshot(0, 0, 0, 0, new List<RtcmTypeStat>()) };
            string report = SupportReport.Build(status, null, CorrectionChecks.Build(status, 0, 0));
            Assert.That(report, Does.Contain("status: Connecting... (Connecting)"));
            Assert.That(report, Does.Contain("base station ID: unknown"));
        }

        [Test]
        public void Report_ShowsTheProblem_WhenThereIsOne()
        {
            var status = new NtripStatus
            {
                State = NtripState.GaveUp,
                LastError = NtripCategory.LoginRejected,
                FailuresInRow = 3,
                Message = "User name, password or mountpoint is not correct. AgIO stopped trying: check the settings and press Start.",
                Rtcm = new RtcmStreamSnapshot(0, 0, 0, 0, new List<RtcmTypeStat>()),
            };
            string report = SupportReport.Build(status, new NtripProfile("h", 2101, "M"), CorrectionChecks.Build(status, 0, 0));
            Assert.That(report, Does.Contain("last problem: LoginRejected, failures in a row: 3"));
            Assert.That(report, Does.Contain("password: (none)"));
        }
    }
}

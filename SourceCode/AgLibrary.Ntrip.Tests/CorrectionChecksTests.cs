using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class CorrectionChecksTests
    {
        private static RtcmStreamSnapshot Snapshot(double sessionSeconds, params int[] types)
        {
            var list = new List<RtcmTypeStat>();
            foreach (int t in types) list.Add(new RtcmTypeStat(t, 10, 1000, 0.4, 1.0));
            return new RtcmStreamSnapshot(types.Length * 10, 0, 0, sessionSeconds, list, 0);
        }

        private static NtripStatus Streaming(double sessionSeconds, double sinceLast, params int[] types) => new NtripStatus
        {
            State = NtripState.Streaming,
            Frames = types.Length * 10,
            SecondsSinceLastFrame = sinceLast,
            Rtcm = Snapshot(sessionSeconds, types),
        };

        private static CheckItem Item(IReadOnlyList<CheckItem> items, int index) => items[index];

        [Test]
        public void EverythingFine_FourGreenLines()
        {
            var items = CorrectionChecks.Build(Streaming(60, 0.4, 1005, 1074, 1084, 1094, 1124), 4, 1.2);
            Assert.That(items.Count, Is.EqualTo(4));
            Assert.That(items.Select(i => i.Level), Is.All.EqualTo(CheckLevel.Ok));
            Assert.That(items[1].Detail, Is.EqualTo("GPS + GLONASS + Galileo + BeiDou"));
            Assert.That(items[2].Detail, Is.EqualTo("Last data 0.4 seconds ago."));
            Assert.That(items[3].Title, Is.EqualTo("Your GPS: RTK fixed"));
            Assert.That(items[3].Detail, Is.EqualTo("Corrections are 1.2 seconds old."));
        }

        [Test]
        public void BasePositionMissing_AfterTheGracePeriod_IsRed_BeforeItIsOnlyAWarning()
        {
            Assert.That(CorrectionChecks.Build(Streaming(5, 0.4, 1074), 1, 0)[0].Level, Is.EqualTo(CheckLevel.Warn));
            CheckItem late = CorrectionChecks.Build(Streaming(30, 0.4, 1074), 1, 0)[0];
            Assert.That(late.Level, Is.EqualTo(CheckLevel.Bad));
            Assert.That(late.Detail, Does.Contain("RTK cannot start"));
        }

        [Test]
        public void NoObservations_IsRed()
        {
            var items = CorrectionChecks.Build(Streaming(60, 0.4, 1005, 1230), 1, 0);
            Assert.That(items[1].Level, Is.EqualTo(CheckLevel.Bad));
            Assert.That(items[1].Title, Does.Contain("No satellite data"));
        }

        [TestCase(0.5, CheckLevel.Ok)]
        [TestCase(3.0, CheckLevel.Ok)]
        [TestCase(6.0, CheckLevel.Warn)]
        [TestCase(10.0, CheckLevel.Warn)]
        [TestCase(11.0, CheckLevel.Bad)]
        public void DataAge_Thresholds(double seconds, CheckLevel expected)
        {
            Assert.That(CorrectionChecks.Build(Streaming(60, seconds, 1005, 1074), 4, 0)[2].Level, Is.EqualTo(expected));
        }

        [Test]
        public void NothingYet_IsHonest_NotGreen()
        {
            var status = new NtripStatus { State = NtripState.Connecting, SecondsSinceLastFrame = -1, Rtcm = Snapshot(0) };
            var items = CorrectionChecks.Build(status, 0, 0);
            Assert.That(items[0].Level, Is.EqualTo(CheckLevel.Bad));
            Assert.That(items[1].Level, Is.EqualTo(CheckLevel.Bad));
            Assert.That(items[2].Level, Is.EqualTo(CheckLevel.Warn));
            Assert.That(items[3].Level, Is.EqualTo(CheckLevel.Bad));
            Assert.That(items[3].Title, Is.EqualTo("Your GPS has no fix"));
        }

        [TestCase(4, CheckLevel.Ok, "Your GPS: RTK fixed")]
        [TestCase(5, CheckLevel.Warn, "Your GPS: RTK float")]
        [TestCase(1, CheckLevel.Warn, "Your GPS: no RTK yet")]
        [TestCase(2, CheckLevel.Warn, "Your GPS: no RTK yet")]
        [TestCase(0, CheckLevel.Bad, "Your GPS has no fix")]
        [TestCase(8, CheckLevel.Warn, "Your GPS is in manual or simulator mode")]
        public void ReceiverFix_InPlainWords(int quality, CheckLevel level, string title)
        {
            CheckItem gps = CorrectionChecks.Build(Streaming(60, 0.4, 1005, 1074), quality, 0)[3];
            Assert.That(gps.Level, Is.EqualTo(level));
            Assert.That(gps.Title, Is.EqualTo(title));
        }

        [Test]
        public void NotRtcm3_OnlyTwoLines_AndNoFalseAlarm()
        {
            var status = new NtripStatus { State = NtripState.Streaming, Unframed = true, Rtcm = Snapshot(30) };
            var items = CorrectionChecks.Build(status, 4, 0);
            Assert.That(items.Count, Is.EqualTo(2));
            Assert.That(items[0].Level, Is.EqualTo(CheckLevel.Ok));
        }
    }

    [TestFixture]
    public class StatusHeadlineTests
    {
        private static NtripStatus S(NtripState state, NtripCategory? error = null, string message = "") =>
            new NtripStatus { State = state, LastError = error, Message = message };

        [TestCase(NtripState.Streaming, null, "NTRIP Active", NtripLight.Ok)]
        [TestCase(NtripState.Connecting, null, "Connecting...", NtripLight.Info)]
        [TestCase(NtripState.Connected, null, "Connected, waiting for data", NtripLight.Info)]
        [TestCase(NtripState.Stopped, null, "Stopped", NtripLight.Off)]
        [TestCase(NtripState.GaveUp, NtripCategory.LoginRejected, "Cannot log in", NtripLight.Bad)]
        [TestCase(NtripState.GaveUp, NtripCategory.UnknownMountpoint, "Mountpoint not found", NtripLight.Bad)]
        [TestCase(NtripState.Waiting, NtripCategory.LoginRejected, "Cannot log in", NtripLight.Bad)]
        [TestCase(NtripState.Waiting, NtripCategory.CasterUnreachable, "Cannot reach the service", NtripLight.Warn)]
        [TestCase(NtripState.Waiting, NtripCategory.NoCorrections, "Connected, nothing coming in", NtripLight.Warn)]
        [TestCase(NtripState.Waiting, NtripCategory.NoNetwork, "No network", NtripLight.Warn)]
        [TestCase(NtripState.Waiting, NtripCategory.StreamLost, "Reconnecting...", NtripLight.Warn)]
        public void HeadlineAndLight(NtripState state, NtripCategory? error, string headline, NtripLight light)
        {
            NtripStatus s = S(state, error);
            Assert.That(s.Headline, Is.EqualTo(headline));
            Assert.That(s.Light, Is.EqualTo(light));
        }

        [Test]
        public void CasterBusy_SaysStartingUp_WhenThatIsTheReason()
        {
            Assert.That(S(NtripState.Waiting, NtripCategory.CasterBusy, "The service is starting up. AgIO tries again in about a minute.").Headline,
                Is.EqualTo("Service is starting up"));
            Assert.That(S(NtripState.Waiting, NtripCategory.CasterBusy, "The service cannot take another connection right now.").Headline,
                Is.EqualTo("Service is busy"));
        }
    }
}

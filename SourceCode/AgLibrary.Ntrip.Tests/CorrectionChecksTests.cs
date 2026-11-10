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

        // A 1005 message for a base station at the given place (sea level), the way a base station would send it.
        private static byte[] BasePositionMessage(double latitude, double longitude, int type = 1005)
        {
            const double a = 6378137.0, f = 1 / 298.257223563;
            double e2 = f * (2 - f);
            double lat = latitude * System.Math.PI / 180, lon = longitude * System.Math.PI / 180;
            double n = a / System.Math.Sqrt(1 - e2 * System.Math.Sin(lat) * System.Math.Sin(lat));
            long x = (long)System.Math.Round(n * System.Math.Cos(lat) * System.Math.Cos(lon) * 10000);
            long y = (long)System.Math.Round(n * System.Math.Cos(lat) * System.Math.Sin(lon) * 10000);
            long z = (long)System.Math.Round(n * (1 - e2) * System.Math.Sin(lat) * 10000);

            var msg = new byte[25];
            msg[0] = 0xD3;
            msg[2] = 19;
            SetBits(msg, 24, 12, type);
            SetBits(msg, 24 + 34, 38, x);
            SetBits(msg, 24 + 74, 38, y);
            SetBits(msg, 24 + 114, 38, z);
            return msg;
        }

        private static void SetBits(byte[] msg, int firstBit, int length, long value)
        {
            for (int i = 0; i < length; i++)
            {
                if (((value >> (length - 1 - i)) & 1) == 0) continue;
                int bit = firstBit + i;
                msg[bit >> 3] |= (byte)(0x80 >> (bit & 7));
            }
        }

        [TestCase(52.0, 5.0)]
        [TestCase(-33.9, 151.2)]
        [TestCase(40.7, -74.0)]
        public void BasePosition_IsDecodedFromMessage1005_AndFrom1006(double lat, double lon)
        {
            foreach (int type in new[] { 1005, 1006 })
            {
                Assert.That(RtcmBasePosition.TryDecode(BasePositionMessage(lat, lon, type), out double dLat, out double dLon), Is.True);
                Assert.That(dLat, Is.EqualTo(lat).Within(0.00001));
                Assert.That(dLon, Is.EqualTo(lon).Within(0.00001));
            }
        }

        [Test]
        public void BasePosition_OfARealMessage1005_FromTheRtcmDocumentation()
        {
            // Station 2003, X 1114104.5999 m, Y -4850729.7108 m, Z 3975521.4643 m
            byte[] frame = { 0xD3, 0x00, 0x13, 0x3E, 0xD7, 0xD3, 0x02, 0x02, 0x98, 0x0E, 0xDE, 0xEF, 0x34, 0xB4, 0xBD, 0x62, 0xAC, 0x09, 0x41, 0x98, 0x6F, 0x33, 0x36, 0x0B, 0x98 };
            Assert.That(RtcmBasePosition.TryDecode(frame, out double lat, out double lon), Is.True);

            RtcmBasePosition.ToGeodetic(1114104.5999, -4850729.7108, 3975521.4643, out double expectedLat, out double expectedLon);
            Assert.That(lat, Is.EqualTo(expectedLat).Within(0.000001));
            Assert.That(lon, Is.EqualTo(expectedLon).Within(0.000001));
            Assert.That(lat, Is.EqualTo(38.8).Within(0.2), "somewhere in the eastern United States");
            Assert.That(lon, Is.EqualTo(-77.1).Within(0.2));
        }

        [Test]
        public void BasePosition_OfAllZeros_OrAShortMessage_IsNotAPosition()
        {
            Assert.That(RtcmBasePosition.TryDecode(new byte[25], out _, out _), Is.False);
            Assert.That(RtcmBasePosition.TryDecode(new byte[10], out _, out _), Is.False);
        }

        [Test]
        public void TheStreamStatsKeepTheBasePosition_AndForgetItOnReset()
        {
            var stats = new RtcmStreamStats();
            var frame = BasePositionMessage(52.0, 5.0);
            uint crc = Rtcm3Framer.Crc24Q(new System.ReadOnlySpan<byte>(frame, 0, 22));
            frame[22] = (byte)(crc >> 16); frame[23] = (byte)(crc >> 8); frame[24] = (byte)crc;

            stats.Feed(frame);
            RtcmStreamSnapshot snap = stats.Snapshot();
            Assert.That(snap.HasBaseCoordinates, Is.True);
            Assert.That(snap.BaseLatitude, Is.EqualTo(52.0).Within(0.00001));

            stats.Reset();
            Assert.That(stats.Snapshot().HasBaseCoordinates, Is.False);
        }

        private static CheckItem DistanceTo(double kmNorth)
        {
            // The base is at 52 N 5 E, the receiver is the given number of kilometres further north.
            double receiverLat = 52.0 + kmNorth / 111.195;
            var snap = new RtcmStreamSnapshot(40, 0, 0, 60, new List<RtcmTypeStat> { new RtcmTypeStat(1005, 10, 250, 0.4, 1.0), new RtcmTypeStat(1074, 10, 1000, 0.4, 1.0) }, 0, 52.0, 5.0);
            var status = new NtripStatus { State = NtripState.Streaming, Frames = 20, SecondsSinceLastFrame = 0.4, Rtcm = snap };
            return CorrectionChecks.Build(status, 4, 0, receiverLat, 5.0).Single(i => i.Title.StartsWith("Distance"));
        }

        [TestCase(0.5, CheckLevel.Ok, "Distance to base station: 500 m")]
        [TestCase(12.3, CheckLevel.Ok, "Distance to base station: 12.3 km")]
        [TestCase(39.9, CheckLevel.Ok, "Distance to base station: 39.9 km")]
        [TestCase(41.0, CheckLevel.Warn, "Distance to base station: 41.0 km")]
        [TestCase(59.9, CheckLevel.Warn, "Distance to base station: 59.9 km")]
        [TestCase(61.0, CheckLevel.Bad, "Distance to base station: 61.0 km")]
        public void DistanceToTheBase_IsGreenUpTo40_OrangeUpTo60_AndThenRed(double km, CheckLevel level, string title)
        {
            CheckItem item = DistanceTo(km);
            Assert.That(item.Level, Is.EqualTo(level));
            Assert.That(item.Title, Is.EqualTo(title));
        }

        [Test]
        public void DistanceToTheBase_WithAFixedPosition_SaysWhereItIsMeasuredFrom()
        {
            var snap = new RtcmStreamSnapshot(40, 0, 0, 60, new List<RtcmTypeStat> { new RtcmTypeStat(1005, 10, 250, 0.4, 1.0), new RtcmTypeStat(1074, 10, 1000, 0.4, 1.0) }, 0, 52.0, 5.0);
            var status = new NtripStatus { State = NtripState.Streaming, Frames = 20, SecondsSinceLastFrame = 0.4, Rtcm = snap };

            CheckItem fixedItem = CorrectionChecks.Build(status, 0, 0, 52.1, 5.0, true)[1];
            Assert.That(fixedItem.Level, Is.EqualTo(CheckLevel.Ok));
            Assert.That(fixedItem.Detail, Is.EqualTo("From the fixed position in the NTRIP settings."));
            Assert.That(CorrectionChecks.Build(status, 4, 0, 52.1, 5.0)[1].Detail, Is.Empty);
        }

        [Test]
        public void DistanceToTheBase_IsLeftOut_WithoutABasePosition_OrWhenNoReceiverPositionIsGiven_AndWaitsForAFix()
        {
            Assert.That(CorrectionChecks.Build(Streaming(60, 0.4, 1005, 1074), 4, 0, 52.0, 5.0).Count, Is.EqualTo(4), "no base coordinates yet");

            var snap = new RtcmStreamSnapshot(40, 0, 0, 60, new List<RtcmTypeStat> { new RtcmTypeStat(1005, 10, 250, 0.4, 1.0), new RtcmTypeStat(1074, 10, 1000, 0.4, 1.0) }, 0, 52.0, 5.0);
            var status = new NtripStatus { State = NtripState.Streaming, Frames = 20, SecondsSinceLastFrame = 0.4, Rtcm = snap };
            Assert.That(CorrectionChecks.Build(status, 4, 0).Count, Is.EqualTo(4), "receiver position not given");

            // No position at all yet (0,0): the line is there and says what it waits for.
            var waiting = CorrectionChecks.Build(status, 0, 0, 0, 0);
            Assert.That(waiting.Count, Is.EqualTo(5));
            Assert.That(waiting[1].Title, Is.EqualTo("Distance to base station"));
            Assert.That(waiting[1].Level, Is.EqualTo(CheckLevel.Warn));
            Assert.That(waiting[1].Detail, Is.EqualTo("Waiting for your GPS position."));

            // A position is a position, whatever the fix quality says.
            Assert.That(CorrectionChecks.Build(status, 0, 0, 52.1, 5.0)[1].Title, Does.StartWith("Distance to base station: 11."));
            Assert.That(CorrectionChecks.Build(status, 4, 0, 52.1, 5.0).Count, Is.EqualTo(5));
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

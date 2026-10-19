using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class StreamStatsDetailsTests
    {
        private static RtcmStreamSnapshot Feed(params int[] types)
        {
            var stats = new RtcmStreamStats(() => 0);
            foreach (int t in types) stats.Feed(Rtcm.Message(t, 30));
            return stats.Snapshot();
        }

        [Test]
        public void StationId_IsReadFromTheMessage_NotFromTheFirstByteOfARead()
        {
            // The old panel read bytes 4 and 5 of whatever chunk arrived, frame or not.
            RtcmStreamSnapshot s = Feed(1005);
            Assert.That(s.StationId, Is.EqualTo(5));   // Rtcm.Message fills payload byte 5 with 5
        }

        [Test]
        public void StationId_IgnoresEphemerides_WhichHaveASatelliteIdThere()
        {
            Assert.That(Feed(1019).StationId, Is.EqualTo(-1));
            Assert.That(Feed(1019, 1074).StationId, Is.Not.EqualTo(-1));
        }

        [Test]
        public void Constellations_AreListedInSystemOrder_AndMsmLevelIsTheHighest()
        {
            RtcmStreamSnapshot s = Feed(1124, 1074, 1084, 1094);
            Assert.That(s.Constellations, Is.EqualTo("GPS + GLONASS + Galileo + BeiDou"));
            Assert.That(s.MsmLevel, Is.EqualTo(4));
            Assert.That(s.HasObservations, Is.True);
            Assert.That(s.HasStationPosition, Is.False);
        }

        [Test]
        public void Constellations_LegacyMessagesCountToo()
        {
            RtcmStreamSnapshot s = Feed(1004, 1012, 1005);
            Assert.That(s.Constellations, Is.EqualTo("GPS + GLONASS"));
            Assert.That(s.MsmLevel, Is.EqualTo(0));
            Assert.That(s.HasStationPosition, Is.True);
        }

        [Test]
        public void WithoutObservations_NothingIsClaimed()
        {
            RtcmStreamSnapshot s = Feed(1005, 1230);
            Assert.That(s.HasObservations, Is.False);
            Assert.That(s.Constellations, Is.Empty);
        }
    }

    [TestFixture]
    public class RtcmQueueSettingsTests
    {
        [Test]
        public void ChunkSize_AndInterval_AreConfigurable()
        {
            double now = 1000;
            var q = new RtcmQueue(() => now, chunkSize: 128, intervalMs: 50);
            q.Enqueue(RtcmQueue.Opaque, new byte[400]);

            Assert.That(q.TryDequeue(out byte[] first), Is.True);
            Assert.That(first.Length, Is.EqualTo(128));
            now += 25;
            Assert.That(q.TryDequeue(out _), Is.False, "the gap is 50 ms, not 25");
            now += 25;
            Assert.That(q.TryDequeue(out byte[] second), Is.True);
            Assert.That(second.Length, Is.EqualTo(128));
        }

        [Test]
        public void ChunkSize_IsCappedAt512_ForTheAiOFirmwares()
        {
            var q = new RtcmQueue(() => 0, chunkSize: 4096);
            q.Enqueue(RtcmQueue.Opaque, new byte[2000]);
            Assert.That(q.TryDequeue(out byte[] c), Is.True);
            Assert.That(c.Length, Is.EqualTo(512));
        }
    }

    [TestFixture]
    public class RawTcpTests
    {
        [Test]
        public void RawTcp_SendsNoRequest_ReadsFramesAtOnce_AndSendsGga()
        {
            var body = new List<byte>();
            body.AddRange(Rtcm.Message(1005, 19));
            body.AddRange(Rtcm.Message(1077, 60));
            using (var caster = new FakeCaster { ReadRequest = false })
            {
                caster.Script(async s =>
                {
                    await s.SendAsync(body.ToArray());
                    await s.WaitAsync(2500);
                });
                using (var h = new Harness(caster.Port, gga: () => "$GPGGA,RAW\r\n", rawTcp: true).Start())
                {
                    Assert.That(h.WaitFor(s => s.State == NtripState.Streaming && s.Frames >= 2), Is.True, h.Client.Status.Message);
                    Assert.That(h.FrameTypes(), Is.EqualTo(new[] { 1005, 1077 }));
                    Assert.That(caster.Requests[0], Is.Empty, "nothing is sent as a request");
                    var until = DateTime.UtcNow.AddSeconds(3);
                    while (DateTime.UtcNow < until && !caster.Sessions[0].ReceivedAfterRequest.Contains("$GPGGA,RAW")) Thread.Sleep(20);
                    Assert.That(caster.Sessions[0].ReceivedAfterRequest, Does.Contain("$GPGGA,RAW"));
                }
            }
        }
    }
}

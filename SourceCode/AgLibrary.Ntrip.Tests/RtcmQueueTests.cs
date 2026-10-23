using System.Collections.Generic;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class RtcmQueueTests
    {
        private double _now;
        private RtcmQueue _q;

        [SetUp]
        public void SetUp()
        {
            _now = 1000;
            _q = new RtcmQueue(() => _now);
        }

        private List<byte[]> Drain(double stepMs = RtcmQueue.IntervalMs)
        {
            var chunks = new List<byte[]>();
            int guard = 0;
            while (_q.Count > 0 && guard++ < 10000)
            {
                if (_q.TryDequeue(out byte[] c)) chunks.Add(c);
                _now += stepMs;
            }
            return chunks;
        }

        private static int Total(List<byte[]> chunks)
        {
            int n = 0;
            foreach (byte[] c in chunks) n += c.Length;
            return n;
        }

        [Test]
        public void FirstDatagramGoesOutAtOnce()
        {
            _q.Enqueue(1005, Rtcm.Message(1005));
            Assert.That(_q.TryDequeue(out byte[] c), Is.True);
            Assert.That(c.Length, Is.EqualTo(16));
        }

        [Test]
        public void Datagrams_AreAtMost256Bytes_AndPaced()
        {
            _q.Enqueue(RtcmQueue.Opaque, new byte[600]);
            Assert.That(_q.TryDequeue(out byte[] first), Is.True);
            Assert.That(first.Length, Is.EqualTo(256));
            Assert.That(_q.TryDequeue(out _), Is.False, "second datagram must wait for the gap");
            _now += RtcmQueue.IntervalMs;
            Assert.That(_q.TryDequeue(out byte[] second), Is.True);
            Assert.That(second.Length, Is.EqualTo(256));
            Assert.That(_q.MsUntilDue(), Is.GreaterThan(0));
        }

        [Test]
        public void NewerObservationEpoch_ReplacesUnsentOlderOne()
        {
            _q.Enqueue(1077, Rtcm.Message(1077, 40, epoch: 1000));
            _q.Enqueue(1077, Rtcm.Message(1077, 40, epoch: 2000));
            Assert.That(_q.SupersededObservations, Is.EqualTo(1));
            Assert.That(_q.Count, Is.EqualTo(46));
        }

        [Test]
        public void SameEpoch_IsNotSuperseded_AndOtherTypesAreKept()
        {
            _q.Enqueue(1077, Rtcm.Message(1077, 40, epoch: 1000));
            _q.Enqueue(1087, Rtcm.Message(1087, 40, epoch: 1000));
            _q.Enqueue(1077, Rtcm.Message(1077, 40, epoch: 1000));
            Assert.That(_q.SupersededObservations, Is.EqualTo(0));
            Assert.That(_q.Count, Is.EqualTo(138));
        }

        [Test]
        public void StationMessage_ReplacesUnsentOneOfItsType()
        {
            _q.Enqueue(1005, Rtcm.Message(1005, 19));
            _q.Enqueue(1005, Rtcm.Message(1005, 19));
            Assert.That(_q.SupersededOther, Is.EqualTo(1));
            Assert.That(_q.Count, Is.EqualTo(25));
        }

        [Test]
        public void Ephemeris_OnlyIdenticalCopyIsReplaced()
        {
            byte[] a = Rtcm.Message(1019, 20);
            byte[] b = Rtcm.Message(1019, 21);
            _q.Enqueue(1019, a);
            _q.Enqueue(1019, a);
            _q.Enqueue(1019, b);
            Assert.That(_q.SupersededOther, Is.EqualTo(1));
            Assert.That(_q.Count, Is.EqualTo(26 + 27));
        }

        [Test]
        public void MessageInFlight_IsNeverReplaced()
        {
            _q.Enqueue(1005, new byte[0]);
            byte[] big = Rtcm.Message(1006, 400);
            _q.Enqueue(1006, big);
            Assert.That(_q.TryDequeue(out _), Is.True);   // 256 of 406 bytes are out
            _q.Enqueue(1006, Rtcm.Message(1006, 400));
            Assert.That(_q.SupersededOther, Is.EqualTo(0));
            Assert.That(Total(Drain()), Is.EqualTo(150 + 406));
        }

        [Test]
        public void Everything_ComesOutInOrder_Intact()
        {
            byte[] m1 = Rtcm.Message(1230, 300);
            byte[] m2 = Rtcm.Message(1029, 100);
            _q.Enqueue(1230, m1);
            _q.Enqueue(1029, m2);
            var all = new List<byte>();
            foreach (byte[] c in Drain()) all.AddRange(c);
            var expected = new List<byte>(m1);
            expected.AddRange(m2);
            Assert.That(all, Is.EqualTo(expected));
        }

        [Test]
        public void MemoryGuard_DropsOldestWholeMessages()
        {
            for (int i = 0; i < 100; i++) _q.Enqueue(1029, Rtcm.Message(1029, 1000));
            Assert.That(_q.Count, Is.LessThanOrEqualTo(RtcmQueue.MaxQueuedBytes));
            Assert.That(_q.MemoryGuardDrops, Is.GreaterThan(0));
            Assert.That(_q.Count % 1006, Is.EqualTo(0), "only whole messages are dropped");
        }

        [Test]
        public void LateSender_CatchesUp_ButNeverBelowMinGap()
        {
            _q.Enqueue(RtcmQueue.Opaque, new byte[256 * 6]);
            Assert.That(_q.TryDequeue(out _), Is.True);
            _now += 100;                                  // sender woke 75 ms late
            Assert.That(_q.TryDequeue(out _), Is.True);
            _now += RtcmQueue.MinGapMs - 1;
            Assert.That(_q.TryDequeue(out _), Is.False);
            _now += 1;
            Assert.That(_q.TryDequeue(out _), Is.True);
            Assert.That(_q.TakePacingStats().CatchUp, Is.GreaterThan(0));
        }

        [Test]
        public void Clear_EmptiesQueue_KeepsCounters()
        {
            _q.Enqueue(1005, Rtcm.Message(1005, 19));
            _q.Enqueue(1005, Rtcm.Message(1005, 19));
            _q.Clear();
            Assert.That(_q.Count, Is.EqualTo(0));
            Assert.That(_q.SupersededOther, Is.EqualTo(1));
        }
    }
}

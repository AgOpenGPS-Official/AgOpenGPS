using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    /// <summary>
    /// Synthetic fixtures written from recordings of real casters (19 October 2026). The recordings
    /// themselves stay local; only the public response headers and stream shape are kept here.
    /// </summary>
    [TestFixture]
    public class RecordedCasterFixtureTests
    {
        private static NtripOutcome Classify(string header, string body = "") =>
            ResponseClassifier.Classify(header, Encoding.ASCII.GetBytes(body));

        [Test]
        public void Millipede_Centipede_V2_Accepted()
        {
            NtripOutcome o = Classify("HTTP/1.1 200 OK\r\nServer: NTRIP Millipede Server 0.8.1\r\nDate: Mon, 19 Oct 2026 19:22:34 GMT\r\n"
                + "Ntrip-Version: Ntrip/2.0\r\nConnection: close\r\nContent-Type: gnss/data\r\n"
                + "Cache-Control: no-store, no-cache, max-age=0\r\nPragma: no-cache\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.Accepted));
            Assert.That(o.Chunked, Is.False);
        }

        [Test]
        public void BkgCaster_V2_Chunked_Accepted()
        {
            NtripOutcome o = Classify("HTTP/1.1 200 OK\r\nNtrip-Version: Ntrip/2.0\r\nServer: NTRIP BKG Caster/2.0.46\r\n"
                + "Date: Mon, 19 Oct 2026 19:17:23 GMT\r\nCache-Control: no-store,no-cache,max-age=0\r\nPragma: no-cache\r\n"
                + "Connection: close\r\nContent-Type: gnss/data\r\nTransfer-Encoding: chunked\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.Accepted));
            Assert.That(o.Chunked, Is.True);
        }

        [Test]
        public void VrsNetwork_AnswersV2RequestWithIcy_Accepted()
        {
            // A v2 request that the caster answers v1-style: "ICY 200 OK", no Content-Type.
            Assert.That(Classify("ICY 200 OK\r\n\r\n").Category, Is.EqualTo(NtripCategory.Accepted));
        }

        [Test]
        public void Snip_Rtk2go_V2_Accepted_AfterLogin()
        {
            NtripOutcome o = Classify("HTTP/1.1 200 OK\r\nDate: Mon, 19 October 2026 19:24:48 UTC\r\n"
                + "Server: SubCarrier Systems Corp SNIP simpleNTRIP_Caster_[3.21.57]RWPRO\r\nNtrip-Version: Ntrip/2.0\r\n"
                + "Cache-Control: no-store, no-cache, max-age=0\r\nPragma: no-cache\r\nConnection: close\r\nContent-Type: gnss/data\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.Accepted));
        }

        [Test]
        public void AgOpenNtripCaster_V2Request_AnsweredWithIcyOctetStream_Accepted()
        {
            NtripOutcome o = Classify("ICY 200 OK\r\nServer: AgOpen NtripCaster/1.0\r\nContent-Type: application/octet-stream\r\nNtrip-Version: Ntrip/2.0\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.Accepted));
            Assert.That(o.Chunked, Is.False);
        }

        [Test]
        public void AgOpenNtripCaster_Bare401_ForWrongPasswordOrUnknownMountpoint()
        {
            // The caster sends this for a wrong login and also for a mountpoint that does not exist,
            // so the message must name the mountpoint as a possible cause.
            NtripOutcome o = Classify("HTTP/1.1 401 Unauthorized\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.LoginRejected));
            Assert.That(o.Message, Does.Contain("mountpoint"));
        }

        [Test]
        public void AgOpenNtripCaster_EpochStream_FramesWithoutLoss()
        {
            // Recorded shape: one ~520-byte read per second (several MSM messages), a few bytes of
            // the next message trailing the read, 50 frames in 15 s with no checksum failures.
            var framer = new Rtcm3Framer();
            var types = new List<int>();
            for (int epoch = 0; epoch < 10; epoch++)
            {
                var read = new List<byte>();
                foreach (int t in new[] { 1074, 1084, 1094, 1124, 1230 }) read.AddRange(Rtcm.Message(t, 90, (uint)(epoch * 1000)));
                byte[] bytes = read.ToArray();
                framer.Feed(new ReadOnlySpan<byte>(bytes, 0, bytes.Length - 18), (t, m) => types.Add(t));
                framer.Feed(new ReadOnlySpan<byte>(bytes, bytes.Length - 18, 18), (t, m) => types.Add(t));
            }
            Assert.That(types.Count, Is.EqualTo(50));
            Assert.That(framer.ChecksumFailures, Is.EqualTo(0));
            Assert.That(framer.BytesSkipped, Is.EqualTo(0));
        }

        [Test]
        public void Snip_StartUpPhase_Http406_IsBusyNotUnderstood()
        {
            // Recorded from rtk2go.com a few minutes after a caster restart, for v1 and v2, with and without login.
            string body = "<html> <head><title>Not Acceptable</title></head> <body><h2>In Start Up Phase, Client Request Not Processed</title></h2>"
                + "The NTRIP Caster has just started, please try again in about one minute.<br>Allowing the Base Stations (NTRIP Servers) to start up now.<br></body> </html>";
            NtripOutcome o = Classify("HTTP/1.1 406\r\nContent-Type: text/html\r\nConnection: Closed\r\n\r\n", body);
            Assert.That(o.Category, Is.EqualTo(NtripCategory.CasterBusy));
            Assert.That(o.Message, Does.Contain("starting up"));
        }

        [Test]
        public void Millipede_UserWithoutPassword_Accepted()
        {
            // Centipede accepted user "centipede" with an empty password (Authorization: Basic base64("centipede:")).
            NtripOutcome o = Classify("HTTP/1.1 200 OK\r\nServer: NTRIP Millipede Server 0.8.1\r\nNtrip-Version: Ntrip/2.0\r\n"
                + "Connection: close\r\nContent-Type: gnss/data\r\nCache-Control: no-store, no-cache, max-age=0\r\nPragma: no-cache\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.Accepted));
        }

        [Test]
        public void Snip_Http406_WithoutBody_IsStillNotUnderstoodNotBusy()
        {
            Assert.That(Classify("HTTP/1.1 406\r\n\r\n").Category, Is.EqualTo(NtripCategory.NotUnderstood));
        }

        [Test]
        public void Snip_Rtk2go_V1_WithoutLogin_SendsSourcetable()
        {
            // Not a stream: the old client counted "SOURCETABLE 200 OK" as connected (N5).
            NtripOutcome o = Classify("SOURCETABLE 200 OK\r\nServer: SubCarrier Systems Corp SNIP simpleNTRIP_Caster_[WPRO]R3.21.57/of:Oct 19 2026\r\n"
                + "Date: Mon, 19 October 2026 19:19:51 UTC\r\nContent-Type: text/plain\r\nContent-Length: 135744\r\n\r\n", "STR;AGROORSOLIC;Ostra");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.UnknownMountpoint));
        }

        [Test]
        public void Snip_Rtk2go_V2Sourcetable_IsNotAStream()
        {
            NtripOutcome o = Classify("HTTP/1.1 200 OK\r\nNtrip-Version: Ntrip/2.0\r\nNtrip-Flags: \r\nNTRIP-Filter-Supports: \r\n"
                + "Connection: close\r\nContent-Type: gnss/sourcetable\r\nCache-Control: no-store, no-cache, max-age=0\r\nContent-Length: 133470\r\n\r\n");
            Assert.That(o.Category, Is.EqualTo(NtripCategory.UnknownMountpoint));
        }

        /// <summary>
        /// The BKG caster joins a stream mid-message and sends it in 100-byte chunks, with each
        /// chunk-size line often arriving as its own tiny TCP read (recorded: "received 4 bytes",
        /// then the data). Frames must come out whole, with only the tail of the first one skipped.
        /// </summary>
        [Test]
        public void ChunkedStream_JoinedMidMessage_SizeLinesInSeparateReads()
        {
            var stream = new List<byte>();
            byte[] first = Rtcm.Message(1084, 300);
            for (int i = 220; i < first.Length; i++) stream.Add(first[i]);   // only the tail of a message already under way
            int expected = 0;
            foreach (int type in new[] { 1005, 1084, 1094, 1124, 1230, 1084 })
            {
                stream.AddRange(Rtcm.Message(type, 120 + type % 7));
                expected++;
            }

            var reads = new List<byte[]>();
            byte[] all = stream.ToArray();
            for (int p = 0; p < all.Length; p += 100)
            {
                int n = Math.Min(100, all.Length - p);
                reads.Add(Encoding.ASCII.GetBytes(n.ToString("x") + "\r\n"));    // size line on its own
                var data = new byte[n];
                Array.Copy(all, p, data, 0, n);
                reads.Add(data);
                reads.Add(Encoding.ASCII.GetBytes("\r\n"));
            }
            reads.Add(Encoding.ASCII.GetBytes("0\r\n\r\n"));

            var decoder = new ChunkedDecoder();
            var framer = new Rtcm3Framer();
            var types = new List<int>();
            foreach (byte[] read in reads)
                decoder.Feed(read, d => framer.Feed(d, (t, m) => types.Add(t)));

            Assert.That(decoder.IsDone, Is.True);
            Assert.That(decoder.IsBroken, Is.False);
            Assert.That(types, Is.EqualTo(new[] { 1005, 1084, 1094, 1124, 1230, 1084 }));
            Assert.That(framer.ChecksumFailures, Is.EqualTo(0));
            Assert.That(framer.BytesSkipped, Is.LessThan(first.Length - 220 + 1));
        }
    }
}

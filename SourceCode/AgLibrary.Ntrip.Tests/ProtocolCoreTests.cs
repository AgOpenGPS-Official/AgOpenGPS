using System;
using System.Collections.Generic;
using System.Text;
using AgLibrary.Ntrip;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    internal static class Rtcm
    {
        /// <summary>A valid RTCM 3 message of the given type with a payload of the given size.</summary>
        public static byte[] Message(int type, int payloadLength = 10, uint epoch = 0)
        {
            var msg = new byte[payloadLength + 6];
            msg[0] = 0xD3;
            msg[1] = (byte)((payloadLength >> 8) & 0x03);
            msg[2] = (byte)(payloadLength & 0xFF);
            msg[3] = (byte)(type >> 4);
            msg[4] = (byte)((type & 0x0F) << 4);
            for (int i = 5; i < 3 + payloadLength; i++) msg[i] = (byte)i;
            if (payloadLength >= 8)
            {
                // 30-bit epoch time at payload bit 24 (after message number and station id).
                msg[6] = (byte)(epoch >> 22);
                msg[7] = (byte)(epoch >> 14);
                msg[8] = (byte)(epoch >> 6);
                msg[9] = (byte)((epoch & 0x3F) << 2);
            }
            uint crc = Rtcm3Framer.Crc24Q(new ReadOnlySpan<byte>(msg, 0, msg.Length - 3));
            msg[msg.Length - 3] = (byte)(crc >> 16);
            msg[msg.Length - 2] = (byte)(crc >> 8);
            msg[msg.Length - 1] = (byte)crc;
            return msg;
        }
    }

    [TestFixture]
    public class ResponseClassifierTests
    {
        private static NtripOutcome C(string header, byte[] body = null) =>
            ResponseClassifier.Classify(header, body ?? new byte[0]);

        [Test] public void IcyOk_IsAccepted() => Assert.That(C("ICY 200 OK\r\n\r\n").Category, Is.EqualTo(NtripCategory.Accepted));

        [Test]
        public void Http200_GnssData_IsAccepted() =>
            Assert.That(C("HTTP/1.1 200 OK\r\nContent-Type: gnss/data\r\n\r\n").Category, Is.EqualTo(NtripCategory.Accepted));

        [Test]
        public void Http200_Rtcm_Body_IsAccepted() =>
            Assert.That(C("HTTP/1.1 200 OK\r\n\r\n", new byte[] { 0xD3, 0, 5 }).Category, Is.EqualTo(NtripCategory.Accepted));

        [Test]
        public void SourcetableOk_IsUnknownMountpoint_N5() =>
            Assert.That(C("SOURCETABLE 200 OK\r\nServer: x\r\n\r\n").Category, Is.EqualTo(NtripCategory.UnknownMountpoint));

        [Test]
        public void Http200_SourcetableContentType_IsUnknownMountpoint_N6() =>
            Assert.That(C("HTTP/1.1 200 OK\r\nContent-Type: gnss/sourcetable\r\n\r\n").Category, Is.EqualTo(NtripCategory.UnknownMountpoint));

        [Test]
        public void Http200_StrBody_IsUnknownMountpoint_N6() =>
            Assert.That(C("HTTP/1.0 200 OK\r\n\r\n", Encoding.ASCII.GetBytes("STR;MP1;")).Category, Is.EqualTo(NtripCategory.UnknownMountpoint));

        [Test]
        public void Http200_HtmlBody_IsNotUnderstood() =>
            Assert.That(C("HTTP/1.1 200 OK\r\n\r\n", Encoding.ASCII.GetBytes("<html>")).Category, Is.EqualTo(NtripCategory.NotUnderstood));

        [Test]
        public void HeaderLineWith200_IsNotAcceptance()
        {
            // The old check was Contains("200"): a header line containing 200 must not count.
            Assert.That(C("HTTP/1.1 401 Unauthorized\r\nContent-Length: 200\r\n\r\n").Category, Is.EqualTo(NtripCategory.LoginRejected));
        }

        [TestCase("HTTP/1.1 401 Unauthorized")]
        [TestCase("ERROR - Bad Password")]
        public void LoginRejected(string line) =>
            Assert.That(C(line + "\r\n\r\n").Category, Is.EqualTo(NtripCategory.LoginRejected));

        [TestCase("HTTP/1.1 404 Not Found")]
        [TestCase("ERROR - Bad Mountpoint")]
        public void UnknownMountpoint(string line) =>
            Assert.That(C(line + "\r\n\r\n").Category, Is.EqualTo(NtripCategory.UnknownMountpoint));

        [TestCase("HTTP/1.1 503 Service Unavailable")]
        [TestCase("ERROR - too many connections")]
        [TestCase("ERROR - account already in use")]
        public void CasterBusy(string line) =>
            Assert.That(C(line + "\r\n\r\n").Category, Is.EqualTo(NtripCategory.CasterBusy));

        [Test]
        public void Http403_Banned_IsBusy() =>
            Assert.That(C("HTTP/1.1 403 Forbidden\r\n\r\n", Encoding.ASCII.GetBytes("Your IP is banned")).Category, Is.EqualTo(NtripCategory.CasterBusy));

        [Test]
        public void Rtk2go_Http400_AsksForEmail_IsLoginRejected()
        {
            // Recorded from rtk2go.com when the request carried no login.
            byte[] body = Encoding.ASCII.GetBytes("<html> <head><title>Rejected, not Processed</title></head> <body><h2>Bad Request, not Processed</title></h2>"
                + "This Caster requires a well formed eMail as the user name to connect.<br></body> </html>");
            NtripOutcome o = ResponseClassifier.Classify("HTTP/1.1 400\r\nContent-Type: text/html\r\nConnection: Closed\r\n\r\n", body);
            Assert.That(o.Category, Is.EqualTo(NtripCategory.LoginRejected));
            Assert.That(o.Message, Does.Contain("e-mail"));
        }

        [Test]
        public void Http403_Plain_IsLoginRejected() =>
            Assert.That(C("HTTP/1.1 403 Forbidden\r\n\r\n").Category, Is.EqualTo(NtripCategory.LoginRejected));

        [TestCase("")]
        [TestCase("HTTP/1.1 400 Bad Request")]
        [TestCase("garbage")]
        public void NotUnderstood(string line) =>
            Assert.That(C(line + "\r\n\r\n").Category, Is.EqualTo(NtripCategory.NotUnderstood));

        [Test]
        public void Chunked_IsDetected_CaseInsensitive()
        {
            Assert.That(C("HTTP/1.1 200 OK\r\ntransfer-encoding: Chunked\r\n\r\n").Chunked, Is.True);
            Assert.That(C("HTTP/1.1 200 OK\r\n\r\n").Chunked, Is.False);
        }

        [Test]
        public void FirstLine_IsKeptForTheLog() =>
            Assert.That(C("ERROR - Bad Password\r\nx: y\r\n\r\n").FirstLine, Is.EqualTo("ERROR - Bad Password"));
    }

    [TestFixture]
    public class ChunkedDecoderTests
    {
        private static byte[] Decode(ChunkedDecoder d, params string[] slices)
        {
            var list = new List<byte>();
            foreach (string s in slices)
                d.Feed(Encoding.ASCII.GetBytes(s), data => list.AddRange(data.ToArray()));
            return list.ToArray();
        }

        [Test]
        public void DecodesTwoChunks_AndFinishes()
        {
            var d = new ChunkedDecoder();
            byte[] r = Decode(d, "5\r\nhello\r\n3\r\nabc\r\n0\r\n\r\n");
            Assert.That(Encoding.ASCII.GetString(r), Is.EqualTo("helloabc"));
            Assert.That(d.IsDone, Is.True);
        }

        [Test]
        public void SizeAndDataSplitAcrossReads()
        {
            var d = new ChunkedDecoder();
            byte[] r = Decode(d, "1", "0\r", "\nabcdefgh", "ijklmnop\r", "\n0\r\n\r\n");
            Assert.That(Encoding.ASCII.GetString(r), Is.EqualTo("abcdefghijklmnop"));
            Assert.That(d.IsDone, Is.True);
        }

        [Test]
        public void ExtensionAfterSize_IsIgnored()
        {
            var d = new ChunkedDecoder();
            Assert.That(Encoding.ASCII.GetString(Decode(d, "4;name=val\r\ntest\r\n0\r\n\r\n")), Is.EqualTo("test"));
        }

        [Test]
        public void NotChunked_IsPassedThrough()
        {
            var d = new ChunkedDecoder();
            byte[] r = Decode(d, "Órtcm bytes");
            Assert.That(d.IsBroken, Is.True);
            Assert.That(r.Length, Is.GreaterThan(0));
        }
    }

    [TestFixture]
    public class Rtcm3FramerTests
    {
        private static List<int> Feed(Rtcm3Framer f, params byte[][] slices)
        {
            var types = new List<int>();
            foreach (byte[] s in slices) f.Feed(s, (t, m) => types.Add(t));
            return types;
        }

        [Test]
        public void Crc24Q_KnownVector() =>
            // CRC-24Q of "123456789" is 0xCDE703 (RTCM 10403 / Qualcomm).
            Assert.That(Rtcm3Framer.Crc24Q(Encoding.ASCII.GetBytes("123456789")), Is.EqualTo(0xCDE703u));

        [Test]
        public void FindsMessages_InOneRead()
        {
            var f = new Rtcm3Framer();
            var all = new List<byte>();
            all.AddRange(Rtcm.Message(1005)); all.AddRange(Rtcm.Message(1077, 40));
            Assert.That(Feed(f, all.ToArray()), Is.EqualTo(new[] { 1005, 1077 }));
            Assert.That(f.BytesSkipped, Is.EqualTo(0));
        }

        [Test]
        public void MessageSplitAcrossReads()
        {
            var f = new Rtcm3Framer();
            byte[] m = Rtcm.Message(1087, 100);
            var a = new byte[30]; var b = new byte[m.Length - 30];
            Array.Copy(m, 0, a, 0, 30); Array.Copy(m, 30, b, 0, b.Length);
            Assert.That(Feed(f, a, b), Is.EqualTo(new[] { 1087 }));
        }

        [Test]
        public void GarbageBetweenFrames_IsSkippedAndCounted()
        {
            var f = new Rtcm3Framer();
            var all = new List<byte> { 1, 2, 3 };
            all.AddRange(Rtcm.Message(1005));
            all.AddRange(new byte[] { 9, 9 });
            all.AddRange(Rtcm.Message(1006));
            Assert.That(Feed(f, all.ToArray()), Is.EqualTo(new[] { 1005, 1006 }));
            Assert.That(f.BytesSkipped, Is.EqualTo(5));
        }

        [Test]
        public void BadCrc_IsRejected_AndNextMessageStillFound()
        {
            var f = new Rtcm3Framer();
            byte[] bad = Rtcm.Message(1005);
            bad[bad.Length - 1] ^= 0xFF;
            var all = new List<byte>(bad);
            all.AddRange(Rtcm.Message(1077));
            Assert.That(Feed(f, all.ToArray()), Is.EqualTo(new[] { 1077 }));
            Assert.That(f.ChecksumFailures, Is.EqualTo(1));
        }

        [Test]
        public void MaximumLengthFrame_IsAccepted()
        {
            var f = new Rtcm3Framer();
            Assert.That(Feed(f, Rtcm.Message(1230, Rtcm3Framer.MaxPayload)), Is.EqualTo(new[] { 1230 }));
        }
    }

    [TestFixture]
    public class RtcmStreamStatsTests
    {
        [Test]
        public void CountsPerType_AndAges()
        {
            double now = 100;
            var s = new RtcmStreamStats(() => now);
            s.Feed(Rtcm.Message(1077));
            now = 102;
            s.Feed(Rtcm.Message(1077));
            s.Feed(Rtcm.Message(1005));
            now = 103;
            RtcmStreamSnapshot snap = s.Snapshot();
            Assert.That(snap.Messages, Is.EqualTo(3));
            Assert.That(snap.HasStationPosition, Is.True);
            RtcmTypeStat t1077 = snap.Types[1];
            Assert.That(t1077.Type, Is.EqualTo(1077));
            Assert.That(t1077.Count, Is.EqualTo(2));
            Assert.That(t1077.MeanIntervalSeconds, Is.EqualTo(2).Within(0.001));
            Assert.That(t1077.SecondsSinceLast, Is.EqualTo(1).Within(0.001));
        }
    }

    [TestFixture]
    public class GgaBuilderTests
    {
        private static readonly DateTime Utc = new DateTime(2026, 10, 19, 13, 5, 9, DateTimeKind.Utc);

        [Test]
        public void Manual_Sentence_IsExact()
        {
            string s = GgaBuilder.FromManual(Utc, 52.0, 5.5, 12.0);
            string body = s.Substring(1, s.IndexOf('*') - 1);
            Assert.That(body, Is.EqualTo("GPGGA,130509.00,5200.00000,N,00530.00000,E,1,12,1.0,12.0,M,0.0,M,,"));
            Assert.That(s.EndsWith("\r\n"), Is.True);
        }

        [Test]
        public void Checksum_MatchesContent()
        {
            string s = GgaBuilder.FromManual(Utc, 52.0, 5.5, 12.0);
            int star = s.IndexOf('*');
            Assert.That(s.Substring(star + 1, 2), Is.EqualTo(GgaBuilder.Checksum(s.Substring(0, star + 1))));
        }

        [Test]
        public void LocalTime_IsConvertedToUtc()
        {
            DateTime local = DateTime.SpecifyKind(Utc.ToLocalTime(), DateTimeKind.Local);
            Assert.That(GgaBuilder.FromManual(local, 52, 5, 0).Contains(",130509.00,"), Is.True);
        }

        [Test]
        public void SouthAndWest_UseHemisphereLetters()
        {
            string s = GgaBuilder.FromManual(Utc, -33.5, -70.25, 0);
            Assert.That(s.Contains(",3330.00000,S,07015.00000,W,"), Is.True);
        }

        [Test]
        public void MinutesNeverRoundTo60()
        {
            string s = GgaBuilder.FromManual(Utc, 51.9999999999, 4.0, 0);
            Assert.That(s.Contains(",5200.00000,N,"), Is.True);
        }

        [Test]
        public void Receiver_WithoutFix_ReturnsNull_N11() =>
            Assert.That(GgaBuilder.FromReceiver(Utc, 52, 5, 0, 0, 0, 0, 0), Is.Null);

        [Test]
        public void Receiver_UsesFixQualitySatellitesAndAltitude()
        {
            string s = GgaBuilder.FromReceiver(Utc, 52, 5, 4, 18, 0.7, 43.2, 1.5);
            Assert.That(s.Contains(",4,18,0.7,43.2,M,"), Is.True);
            Assert.That(s.Contains(",1.5,"), Is.True);
        }
    }
}

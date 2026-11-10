using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class NtripHeaderScannerTests
    {
        private static byte[] Bytes(string s) => Encoding.ASCII.GetBytes(s);

        private static byte[] Concat(byte[] a, byte[] b)
        {
            var l = new List<byte>(a);
            l.AddRange(b);
            return l.ToArray();
        }

        private static bool Find(byte[] data, out string header, out int bodyOffset)
        {
            bool ok = NtripHeaderScanner.TryFindEnd(data, out int len, out bodyOffset);
            header = ok ? Encoding.ASCII.GetString(data, 0, len) : null;
            return ok;
        }

        [Test]
        public void Http_EndsAtBlankLine()
        {
            byte[] d = Bytes("HTTP/1.1 200 OK\r\nContent-Type: gnss/data\r\n\r\n");
            Assert.That(Find(d, out string h, out int body), Is.True);
            Assert.That(h, Is.EqualTo("HTTP/1.1 200 OK\r\nContent-Type: gnss/data"));
            Assert.That(body, Is.EqualTo(d.Length));
        }

        [Test]
        public void Http_BareLineFeedEndings()
        {
            byte[] d = Bytes("HTTP/1.0 200 OK\nServer: x\n\nSTR;");
            Assert.That(Find(d, out string h, out int body), Is.True);
            Assert.That(h, Is.EqualTo("HTTP/1.0 200 OK\nServer: x"));
            Assert.That(Encoding.ASCII.GetString(d, body, d.Length - body), Is.EqualTo("STR;"));
        }

        [Test]
        public void IncompleteHeader_NeedsMore()
        {
            Assert.That(Find(Bytes("HTTP/1.1 200 OK\r\nContent-Ty"), out _, out _), Is.False);
            Assert.That(Find(Bytes("HTTP/1.1 200 OK\r\n\r"), out _, out _), Is.False);
        }

        [Test]
        public void Icy_WithBlankLine_EndsAtBlankLine()
        {
            byte[] d = Bytes("ICY 200 OK\r\n\r\n");
            Assert.That(Find(d, out string h, out int body), Is.True);
            Assert.That(h, Is.EqualTo("ICY 200 OK"));
            Assert.That(body, Is.EqualTo(d.Length));
        }

        [Test]
        public void Icy_WithHeaderLines_EndsAtBlankLine()
        {
            byte[] d = Bytes("ICY 200 OK\r\nServer: AgOpen NtripCaster/1.0\r\nContent-Type: application/octet-stream\r\n\r\n");
            Assert.That(Find(d, out string h, out int body), Is.True);
            Assert.That(h, Does.Contain("Content-Type"));
            Assert.That(body, Is.EqualTo(d.Length));
        }

        [Test]
        public void Rtklib_IcyLineThenRtcm_NoBlankLine()
        {
            // Recorded from RTKBase (RTKLIB demo5 b34j): "ICY 200 OK\r\n" and at once 0xD3 ...
            byte[] frame = Rtcm.Message(1005, 19);
            byte[] d = Concat(Bytes("ICY 200 OK\r\n"), frame);
            Assert.That(Find(d, out string h, out int body), Is.True);
            Assert.That(h, Is.EqualTo("ICY 200 OK"));
            Assert.That(body, Is.EqualTo(12));
            Assert.That(d[body], Is.EqualTo(0xD3));
        }

        [Test]
        public void Rtklib_IcyLineAlone_WaitsButMayBeAcceptedAfterAPause()
        {
            // The first read was only the 12 bytes of the status line; the data came 0.9 s later.
            byte[] d = Bytes("ICY 200 OK\r\n");
            Assert.That(Find(d, out _, out _), Is.False);
            Assert.That(NtripHeaderScanner.IsIcyStatusLineOnly(d), Is.True);
        }

        [Test]
        public void Rtklib_IcyLineArrivesFirst_DataInTheNextRead()
        {
            // Recorded: 12 bytes (the status line), then the first epoch 9 ms later.
            var received = new List<byte>(Bytes("ICY 200 OK\r\n"));
            Assert.That(NtripHeaderScanner.TryFindEnd(received.ToArray(), out _, out _), Is.False);

            received.AddRange(Rtcm.Message(1004, 150));
            Assert.That(NtripHeaderScanner.TryFindEnd(received.ToArray(), out int len, out int body), Is.True);
            Assert.That(Encoding.ASCII.GetString(received.ToArray(), 0, len), Is.EqualTo("ICY 200 OK"));
            Assert.That(received.Count - body, Is.EqualTo(156));
        }

        [Test]
        public void IcyLineAlone_IsNotMistakenWhenMoreHeaderTextFollows()
        {
            Assert.That(NtripHeaderScanner.IsIcyStatusLineOnly(Bytes("ICY 200 OK\r\nServer: x\r\n")), Is.False);
            Assert.That(NtripHeaderScanner.IsIcyStatusLineOnly(Bytes("HTTP/1.1 200 OK\r\n")), Is.False);
            Assert.That(NtripHeaderScanner.IsIcyStatusLineOnly(Bytes("ICY 200 OK")), Is.False);
        }

        [Test]
        public void Rtklib_Sourcetable_EndsAtBlankLine()
        {
            byte[] d = Bytes("SOURCETABLE 200 OK\r\nServer: RTKLIB demo5 b34j\r\nContent-Type: text/plain\r\nContent-Length: 186\r\n\r\nSTR;WACHTUM_NLD;");
            Assert.That(Find(d, out string h, out int body), Is.True);
            Assert.That(h, Does.StartWith("SOURCETABLE 200 OK"));
            Assert.That(Encoding.ASCII.GetString(d, body, d.Length - body), Is.EqualTo("STR;WACHTUM_NLD;"));
        }

        [Test]
        public void Rtklib_FullSession_FramesAreFoundAfterTheShortHeader()
        {
            byte[] d = Concat(Bytes("ICY 200 OK\r\n"), Concat(Rtcm.Message(1004, 150), Rtcm.Message(1005, 19)));
            Assert.That(NtripHeaderScanner.TryFindEnd(d, out int len, out int body), Is.True);

            var framer = new Rtcm3Framer();
            var types = new List<int>();
            framer.Feed(new ReadOnlySpan<byte>(d, body, d.Length - body), (t, m) => types.Add(t));
            Assert.That(types, Is.EqualTo(new[] { 1004, 1005 }));
            Assert.That(framer.BytesSkipped, Is.EqualTo(0));

            NtripOutcome outcome = ResponseClassifier.Classify(Encoding.ASCII.GetString(d, 0, len), new ReadOnlySpan<byte>(d, body, 3));
            Assert.That(outcome.Category, Is.EqualTo(NtripCategory.Accepted));
        }
    }
}

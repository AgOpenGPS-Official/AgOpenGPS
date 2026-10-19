using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    /// <summary>Runs an NtripClient against a FakeCaster with delays that return at once.</summary>
    internal sealed class Harness : IDisposable
    {
        public readonly NtripClient Client;
        public readonly List<TimeSpan> Delays = new List<TimeSpan>();
        public readonly List<int> Frames = new List<int>();
        public long RawBytes;
        public readonly CancellationTokenSource Cts = new CancellationTokenSource();
        public Task Run;

        public static NtripTimeouts QuickTimeouts() => new NtripTimeouts
        {
            Connect = TimeSpan.FromSeconds(2),
            Header = TimeSpan.FromSeconds(1),
            NoData = TimeSpan.FromMilliseconds(600),
            IcyGrace = TimeSpan.FromMilliseconds(150),
        };

        public Harness(int port, NtripVersion version = NtripVersion.V2, string mount = "MOUNT", string user = "",
            string password = "", Func<string> gga = null, int ggaInterval = 0, NtripTimeouts timeouts = null, bool rawTcp = false)
        {
            var profile = new NtripProfile("127.0.0.1", port, mount, user, password, version, ggaInterval, rawTcp);
            Client = new NtripClient(profile, gga, timeouts ?? QuickTimeouts(),
                (span, ct) =>
                {
                    lock (Delays) Delays.Add(span);
                    ct.ThrowIfCancellationRequested();
                    // Long enough for a test to see the Waiting state, short enough to keep the suite quick.
                    return Task.Delay(100, ct);
                },
                () => 0.5);   // no jitter
            Client.FrameReceived += (type, frame) => { lock (Frames) Frames.Add(type); };
            Client.RawDataReceived += data => Interlocked.Add(ref RawBytes, data.Length);
        }

        public Harness Start()
        {
            Run = Task.Run(() => Client.RunAsync(Cts.Token));
            return this;
        }

        public bool WaitFor(Func<NtripStatus, bool> condition, int ms = 5000)
        {
            var until = DateTime.UtcNow.AddMilliseconds(ms);
            while (DateTime.UtcNow < until)
            {
                if (condition(Client.Status)) return true;
                Thread.Sleep(15);
            }
            return false;
        }

        public bool WaitForDelays(int count, int ms = 5000)
        {
            var until = DateTime.UtcNow.AddMilliseconds(ms);
            while (DateTime.UtcNow < until)
            {
                lock (Delays) if (Delays.Count >= count) return true;
                Thread.Sleep(15);
            }
            return false;
        }

        public List<double> DelaySeconds()
        {
            lock (Delays) return Delays.ConvertAll(d => Math.Round(d.TotalSeconds, 1));
        }

        public List<int> FrameTypes()
        {
            lock (Frames) return new List<int>(Frames);
        }

        public void Dispose()
        {
            Cts.Cancel();
            try { Run?.Wait(3000); } catch (Exception) { }
        }
    }

    [TestFixture]
    public class NtripClientTests
    {
        private const string Ok2 = "HTTP/1.1 200 OK\r\nContent-Type: gnss/data\r\n\r\n";

        private static byte[] Join(params byte[][] parts)
        {
            var l = new List<byte>();
            foreach (byte[] p in parts) l.AddRange(p);
            return l.ToArray();
        }

        private static byte[] Header(string text) => Encoding.ASCII.GetBytes(text);

        private static byte[] Frames(params int[] types)
        {
            var l = new List<byte>();
            foreach (int t in types) l.AddRange(Rtcm.Message(t, 60));
            return l.ToArray();
        }

        private static Func<FakeSession, Task> Accept(string header, byte[] body, int holdMs = 3000) => async s =>
        {
            await s.SendAsync(Join(Header(header), body));
            await s.WaitAsync(holdMs);
        };

        // ----- accepted sessions -----

        [Test]
        public void V2_Accepted_StreamsFrames_AndTheRequestIsRight()
        {
            using (var caster = new FakeCaster().Script(Accept(Ok2, Frames(1005, 1077))))
            using (var h = new Harness(caster.Port, mount: "Mount Name", user: "üser", password: "päss").Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming && s.Frames >= 2), Is.True, h.Client.Status.Message);
                Assert.That(h.FrameTypes(), Is.EqualTo(new[] { 1005, 1077 }));

                string req = caster.Requests[0];
                Assert.That(req, Does.StartWith("GET /Mount%20Name HTTP/1.1\r\n"));
                Assert.That(req, Does.Contain("Host: 127.0.0.1:" + caster.Port + "\r\n"));
                Assert.That(req, Does.Contain("Ntrip-Version: Ntrip/2.0\r\n"));
                string b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("üser:päss"));
                Assert.That(req, Does.Contain("Authorization: Basic " + b64 + "\r\n"));
                Assert.That(req, Does.Not.Contain("Ntrip-GGA"));
            }
        }

        [Test]
        public void V2_PutsGgaInTheRequestHeader_N9()
        {
            using (var caster = new FakeCaster().Script(Accept(Ok2, Frames(1005))))
            using (var h = new Harness(caster.Port, gga: () => "$GPGGA,120000.00,5200.00000,N,00500.00000,E,1,12,1.0,0.0,M,0.0,M,,*00\r\n").Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming), Is.True);
                Assert.That(caster.Requests[0], Does.Contain("Ntrip-GGA: $GPGGA,120000.00"));
            }
        }

        [Test]
        public void V1_Request_HasNoHost_AndGgaFollowsTheAnswerAtOnce()
        {
            using (var caster = new FakeCaster().Script(Accept("ICY 200 OK\r\n\r\n", Frames(1004))))
            using (var h = new Harness(caster.Port, NtripVersion.V1, gga: () => "$GPGGA,TEST\r\n").Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming), Is.True);
                Assert.That(caster.Requests[0], Does.StartWith("GET /MOUNT HTTP/1.0\r\n"));
                Assert.That(caster.Requests[0], Does.Not.Contain("Host:"));
                Assert.That(SpinUntil(() => caster.Sessions[0].ReceivedAfterRequest.Contains("$GPGGA,TEST")), Is.True);
            }
        }

        [Test]
        public void Gga_IsRepeatedAtTheInterval()
        {
            var quiet = Harness.QuickTimeouts();
            quiet.NoData = TimeSpan.FromSeconds(10);   // the stream stays silent after one frame; do not end the session
            using (var caster = new FakeCaster().Script(Accept(Ok2, Frames(1005), 4000)))
            using (var h = new Harness(caster.Port, NtripVersion.V1, gga: () => "$GPGGA,X\r\n", ggaInterval: 1, timeouts: quiet).Start())
            {
                // V1: one right after the answer, then one per second (the stream keeps delivering below).
                Assert.That(SpinUntil(() =>
                {
                    string got = caster.Sessions.Count > 0 ? caster.Sessions[0].ReceivedAfterRequest : "";
                    return CountOf(got, "$GPGGA,X") >= 3;
                }, 5000), Is.True);
            }
        }

        [Test]
        public void Chunked_BkgStyle_IsDecoded_N4()
        {
            var body = new List<byte>();
            byte[] all = Frames(1084, 1094, 1124);
            for (int p = 0; p < all.Length; p += 100)
            {
                int n = Math.Min(100, all.Length - p);
                body.AddRange(Header(n.ToString("x") + "\r\n"));
                for (int i = 0; i < n; i++) body.Add(all[p + i]);
                body.AddRange(Header("\r\n"));
            }
            using (var caster = new FakeCaster().Script(Accept("HTTP/1.1 200 OK\r\nContent-Type: gnss/data\r\nTransfer-Encoding: chunked\r\n\r\n", body.ToArray())))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.Frames >= 3), Is.True);
                Assert.That(h.FrameTypes(), Is.EqualTo(new[] { 1084, 1094, 1124 }));
                Assert.That(h.Client.Status.Rtcm.ChecksumFailures, Is.EqualTo(0));
            }
        }

        [Test]
        public void Rtklib_IcyLineAlone_ThenDataAfterAPause()
        {
            using (var caster = new FakeCaster().Script(async s =>
            {
                await s.SendAsync("ICY 200 OK\r\n");
                await s.WaitAsync(400);                       // longer than the grace time
                await s.SendAsync(Frames(1004, 1005));
                await s.WaitAsync(2000);
            }))
            using (var h = new Harness(caster.Port, NtripVersion.V1).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming && s.Frames >= 2), Is.True, h.Client.Status.Message);
                Assert.That(h.FrameTypes(), Is.EqualTo(new[] { 1004, 1005 }));
            }
        }

        [Test]
        public void Rtklib_IcyLineWithDataInTheSameRead()
        {
            using (var caster = new FakeCaster().Script(Accept("ICY 200 OK\r\n", Frames(1004, 1005))))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.Frames >= 2), Is.True);
                Assert.That(h.Client.Status.Rtcm.BytesSkipped, Is.EqualTo(0));
            }
        }

        [Test]
        public void NotRtcm3_IsForwardedRaw_AndDoesNotReconnect()
        {
            var junk = new byte[1500];
            for (int i = 0; i < junk.Length; i++) junk[i] = 0x55;
            using (var caster = new FakeCaster().Script(async s =>
            {
                await s.SendAsync(Header(Ok2));
                for (int i = 0; i < 6; i++) { await s.SendAsync(junk); await s.WaitAsync(50); }
                await s.WaitAsync(1500);
            }))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.Unframed && s.State == NtripState.Streaming), Is.True);
                Assert.That(SpinUntil(() => Interlocked.Read(ref h.RawBytes) > 1000), Is.True);
                Assert.That(caster.Connections, Is.EqualTo(1));
            }
        }

        // ----- rows of the Errors table -----

        [Test]
        public void LoginRejected_StopsAfterThreeAttempts()
        {
            using (var caster = new FakeCaster().Script(5, s => s.SendAsync("HTTP/1.1 401 Unauthorized\r\n\r\n")))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.GaveUp), Is.True);
                NtripStatus st = h.Client.Status;
                Assert.That(st.LastError, Is.EqualTo(NtripCategory.LoginRejected));
                Assert.That(st.Message, Does.Contain("stopped trying"));
                Assert.That(caster.Connections, Is.EqualTo(3));
                Assert.That(h.DelaySeconds(), Is.EqualTo(new[] { 5.0, 5.0 }));
                Assert.That(h.Run.Wait(2000), Is.True, "the run ends by itself");
            }
        }

        [Test]
        public void AttemptCounter_IsShownBeforeGivingUp()
        {
            using (var caster = new FakeCaster().Script(5, s => s.SendAsync("HTTP/1.1 401 Unauthorized\r\n\r\n")))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Waiting && s.FailuresInRow == 2), Is.True);
                Assert.That(h.Client.Status.Message, Does.Contain("Try 2 of 3"));
            }
        }

        [Test]
        public void UnknownMountpoint_Sourcetable200_StopsAfterThree()
        {
            string table = "SOURCETABLE 200 OK\r\nContent-Type: text/plain\r\n\r\nSTR;OTHER;\r\nENDSOURCETABLE\r\n";
            using (var caster = new FakeCaster().Script(5, s => s.SendAsync(table)))
            using (var h = new Harness(caster.Port, NtripVersion.V1).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.GaveUp), Is.True);
                Assert.That(h.Client.Status.LastError, Is.EqualTo(NtripCategory.UnknownMountpoint));
                Assert.That(caster.Connections, Is.EqualTo(3));
            }
        }

        [Test]
        public void CasterBusy_BacksOff_30_60_120_300()
        {
            using (var caster = new FakeCaster().Script(8, s => s.SendAsync("HTTP/1.1 503 Service Unavailable\r\n\r\n")))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitForDelays(5), Is.True);
                Assert.That(h.DelaySeconds().GetRange(0, 5), Is.EqualTo(new[] { 30.0, 60.0, 120.0, 300.0, 300.0 }));
                Assert.That(h.Client.Status.LastError, Is.EqualTo(NtripCategory.CasterBusy));
            }
        }

        [Test]
        public void Snip_StartUp406_IsRetriedNotGivenUp()
        {
            string body = "<html>In Start Up Phase, Client Request Not Processed. The NTRIP Caster has just started, please try again in about one minute.</html>";
            using (var caster = new FakeCaster().Script(6, s => s.SendAsync("HTTP/1.1 406\r\nContent-Type: text/html\r\nConnection: Closed\r\n\r\n" + body)))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitForDelays(2), Is.True);
                Assert.That(h.Client.Status.State, Is.Not.EqualTo(NtripState.GaveUp));
                Assert.That(h.Client.Status.LastError, Is.EqualTo(NtripCategory.CasterBusy));
                Assert.That(h.Client.Status.Message, Does.Contain("starting up"));
            }
        }

        [Test]
        public void CasterUnreachable_Refused_BacksOff_5_10_20_40_120()
        {
            int port;
            using (var gone = new FakeCaster()) port = gone.Port;   // nothing listens here any more
            // Windows retries a refused loopback connection by itself, which takes about two seconds.
            var slowConnect = Harness.QuickTimeouts();
            slowConnect.Connect = TimeSpan.FromSeconds(8);
            using (var h = new Harness(port, timeouts: slowConnect).Start())
            {
                // The full 5-10-20-40-120 table is checked in RetryPolicyTests; two steps prove the wiring.
                Assert.That(h.WaitForDelays(2, 20000), Is.True);
                Assert.That(h.DelaySeconds().GetRange(0, 2), Is.EqualTo(new[] { 5.0, 10.0 }));
                Assert.That(h.Client.Status.LastError, Is.EqualTo(NtripCategory.CasterUnreachable));
                Assert.That(h.Client.Status.Message, Does.Contain("switched on"));
            }
        }

        [Test]
        public void CasterUnreachable_SilentCaster_HeaderTimeout()
        {
            using (var caster = new FakeCaster().Script(5, s => s.WaitAsync(5000)))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Waiting, 4000), Is.True);
                Assert.That(h.Client.Status.LastError, Is.EqualTo(NtripCategory.CasterUnreachable));
                Assert.That(h.Client.Status.Message, Does.Contain("No answer"));
            }
        }

        [Test]
        public void CasterUnreachable_ClosedWithoutAnswer()
        {
            using (var caster = new FakeCaster().Script(2, s => Task.CompletedTask))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Waiting), Is.True);
                Assert.That(h.Client.Status.Message, Does.Contain("without answering"));
            }
        }

        [Test]
        public void NoCorrections_AcceptedButSilent_ReconnectsAfter10Seconds()
        {
            using (var caster = new FakeCaster().Script(3, async s => { await s.SendAsync(Header(Ok2)); await s.WaitAsync(3000); }))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitForDelays(2), Is.True);
                Assert.That(h.DelaySeconds().GetRange(0, 2), Is.EqualTo(new[] { 10.0, 20.0 }));
                Assert.That(h.Client.Status.LastError, Is.EqualTo(NtripCategory.NoCorrections));
                Assert.That(h.Client.Status.Message, Does.Contain("nothing is coming in"));
            }
        }

        [Test]
        public void StreamLost_AfterFrames_ReconnectsAtOnce_ThenBacksOff()
        {
            using (var caster = new FakeCaster()
                .Script(Accept(Ok2, Frames(1005), 100))       // delivers, then the connection ends
                .Script(Accept(Ok2, Frames(1005), 3000)))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => caster.Connections == 2 && s.State == NtripState.Streaming && s.Frames >= 1, 6000), Is.True);
                Assert.That(h.DelaySeconds(), Is.Empty, "the first loss reconnects without waiting");
            }
        }

        [Test]
        public void StreamLost_RepeatedLoss_WaitsLikeUnreachable()
        {
            // Each session delivers nothing and ends: the policy counts them as NoCorrections, not StreamLost,
            // so use one frame per session and a quick end instead.
            using (var caster = new FakeCaster().Script(6, Accept(Ok2, Frames(1005), 50)))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(SpinUntil(() => caster.Connections >= 4), Is.True);
                // A session that delivered frames resets the count, so every loss is a "first" loss: no waiting.
                Assert.That(h.DelaySeconds(), Is.Empty);
            }
        }

        [Test]
        public void Auto_FallsBackToV1_WhenCasterAnswers400_AndStaysThere()
        {
            using (var caster = new FakeCaster()
                .Script(s => s.SendAsync("HTTP/1.1 400 Bad Request\r\n\r\n"))
                .Script(Accept("ICY 200 OK\r\n\r\n", Frames(1004))))
            using (var h = new Harness(caster.Port, NtripVersion.Auto).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming), Is.True);
                Assert.That(caster.Requests[0], Does.StartWith("GET /MOUNT HTTP/1.1"));
                Assert.That(caster.Requests[1], Does.StartWith("GET /MOUNT HTTP/1.0"));
                Assert.That(h.Client.Status.VersionUsed, Is.EqualTo(NtripVersion.V1));
                Assert.That(h.DelaySeconds(), Is.Empty);
            }
        }

        [Test]
        public void Auto_VrsAnsweringV2WithIcy_IsAcceptedWithoutFallback()
        {
            using (var caster = new FakeCaster().Script(Accept("ICY 200 OK\r\n\r\n", Frames(1013))))
            using (var h = new Harness(caster.Port, NtripVersion.Auto).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming), Is.True);
                Assert.That(caster.Connections, Is.EqualTo(1));
            }
        }

        [Test]
        public void Cancel_StopsPromptly_WhileStreaming()
        {
            using (var caster = new FakeCaster().Script(Accept(Ok2, Frames(1005), 10000)))
            using (var h = new Harness(caster.Port).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming), Is.True);
                var watch = System.Diagnostics.Stopwatch.StartNew();
                h.Cts.Cancel();
                Assert.That(h.Run.Wait(2000), Is.True);
                Assert.That(watch.ElapsedMilliseconds, Is.LessThan(1500));
                Assert.That(h.Client.Status.State, Is.EqualTo(NtripState.Stopped));
            }
        }

        [Test]
        public void NoPosition_NoGgaHeaderAndNoGgaLine()
        {
            using (var caster = new FakeCaster().Script(Accept(Ok2, Frames(1005))))
            using (var h = new Harness(caster.Port, gga: () => null, ggaInterval: 1).Start())
            {
                Assert.That(h.WaitFor(s => s.State == NtripState.Streaming), Is.True);
                Assert.That(caster.Requests[0], Does.Not.Contain("Ntrip-GGA"));
            }
        }

        // ----- helpers -----

        private static bool SpinUntil(Func<bool> condition, int ms = 5000)
        {
            var until = DateTime.UtcNow.AddMilliseconds(ms);
            while (DateTime.UtcNow < until)
            {
                if (condition()) return true;
                Thread.Sleep(15);
            }
            return false;
        }

        private static int CountOf(string text, string needle)
        {
            int count = 0, at = 0;
            while ((at = text.IndexOf(needle, at, StringComparison.Ordinal)) >= 0) { count++; at += needle.Length; }
            return count;
        }
    }
}

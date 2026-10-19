using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    /// <summary>A stream the test feeds chunk by chunk, like a serial port; reading blocks until data comes.</summary>
    internal sealed class FeedStream : Stream
    {
        private readonly BlockingCollection<byte[]> _chunks = new BlockingCollection<byte[]>();
        private byte[] _current = new byte[0];
        private int _offset;

        public void Feed(byte[] data) => _chunks.Add(data);

        public void End() => _chunks.CompleteAdding();

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_offset >= _current.Length)
            {
                try { _current = _chunks.Take(); }
                catch (InvalidOperationException) { return 0; }   // the end
                _offset = 0;
            }
            int n = Math.Min(count, _current.Length - _offset);
            Array.Copy(_current, _offset, buffer, offset, n);
            _offset += n;
            return n;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_chunks.IsAddingCompleted) _chunks.CompleteAdding();
            base.Dispose(disposing);
        }

        public override bool CanRead => true;
        public override bool CanSeek => false;
        public override bool CanWrite => false;
        public override long Length => throw new NotSupportedException();
        public override long Position { get => throw new NotSupportedException(); set => throw new NotSupportedException(); }
        public override void Flush() { }
        public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
        public override void SetLength(long value) => throw new NotSupportedException();
        public override void Write(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    }

    [TestFixture]
    public class RadioSourceTests
    {
        private static bool WaitFor(Func<bool> condition, int ms = 3000)
        {
            var until = DateTime.UtcNow.AddMilliseconds(ms);
            while (DateTime.UtcNow < until)
            {
                if (condition()) return true;
                Thread.Sleep(10);
            }
            return false;
        }

        private static Func<TimeSpan, CancellationToken, Task> QuickDelay(List<TimeSpan> record) => (span, ct) =>
        {
            lock (record) record.Add(span);
            return Task.Delay(30, ct);
        };

        [Test]
        public void Radio_StreamsFrames_AndSaysRadioActive()
        {
            var feed = new FeedStream();
            var radio = new RadioSource("COM5", () => feed);
            var types = new List<int>();
            radio.FrameReceived += (t, f) => { lock (types) types.Add(t); };
            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => radio.RunAsync(cts.Token));
                Assert.That(WaitFor(() => radio.Status.State == NtripState.Connected), Is.True);

                var data = new List<byte>(Rtcm.Message(1005, 19));
                data.AddRange(Rtcm.Message(1077, 60));
                feed.Feed(data.ToArray());

                Assert.That(WaitFor(() => radio.Status.State == NtripState.Streaming && radio.Status.Frames >= 2), Is.True);
                Assert.That(radio.Status.Headline, Is.EqualTo("Radio Active"));
                Assert.That(radio.Description, Is.EqualTo("Radio COM5"));
                lock (types) Assert.That(types, Is.EqualTo(new[] { 1005, 1077 }));
                Assert.That(radio.Status.Rtcm.HasStationPosition, Is.True);

                cts.Cancel();
                Assert.That(run.Wait(2000), Is.True, "stops promptly");
                Assert.That(radio.Status.State, Is.EqualTo(NtripState.Stopped));
            }
        }

        [Test]
        public void Radio_CannotBeOpened_RetriesAndRecovers()
        {
            var feed = new FeedStream();
            int attempts = 0;
            var delays = new List<TimeSpan>();
            var radio = new RadioSource("COM5", () =>
            {
                if (Interlocked.Increment(ref attempts) < 3) throw new IOException("port busy");
                return feed;
            }, TimeSpan.FromSeconds(5), null, QuickDelay(delays));

            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => radio.RunAsync(cts.Token));
                Assert.That(WaitFor(() => radio.Status.State == NtripState.Connected), Is.True);
                Assert.That(attempts, Is.EqualTo(3));
                lock (delays) Assert.That(delays, Is.EqualTo(new[] { TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(5) }));
                cts.Cancel();
                run.Wait(2000);
            }
        }

        [Test]
        public void Radio_FailureMessage_NamesThePort()
        {
            var delays = new List<TimeSpan>();
            var radio = new RadioSource("COM9", () => throw new IOException("nope"), TimeSpan.FromSeconds(5), null, QuickDelay(delays));
            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => radio.RunAsync(cts.Token));
                Assert.That(WaitFor(() => radio.Status.State == NtripState.Waiting && radio.Status.ConsecutiveFailures >= 2), Is.True);
                Assert.That(radio.Status.Message, Is.EqualTo("Cannot open the radio on COM9."));
                cts.Cancel();
                run.Wait(2000);
            }
        }

        [Test]
        public void Radio_StreamEnds_ReconnectsWithAMessage()
        {
            var first = new FeedStream();
            var second = new FeedStream();
            int opened = 0;
            var delays = new List<TimeSpan>();
            var radio = new RadioSource("COM5", () => Interlocked.Increment(ref opened) == 1 ? first : second, null, null, QuickDelay(delays));
            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => radio.RunAsync(cts.Token));
                first.Feed(Rtcm.Message(1005, 19));
                Assert.That(WaitFor(() => radio.Status.State == NtripState.Streaming), Is.True);

                first.End();                                       // the radio is unplugged
                Assert.That(WaitFor(() => opened == 2), Is.True, "opened again");
                second.Feed(Rtcm.Message(1077, 60));
                Assert.That(WaitFor(() => radio.Status.State == NtripState.Streaming && radio.Status.Frames >= 1), Is.True);
                cts.Cancel();
                run.Wait(2000);
            }
        }

        [Test]
        public void Radio_NotRtcm3_IsForwardedRaw()
        {
            var feed = new FeedStream();
            var radio = new RadioSource("COM5", () => feed);
            long raw = 0;
            radio.RawDataReceived += data => Interlocked.Add(ref raw, data.Length);
            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => radio.RunAsync(cts.Token));
                var junk = new byte[1500];
                for (int i = 0; i < junk.Length; i++) junk[i] = 0x55;
                for (int i = 0; i < 6; i++) feed.Feed(junk);

                Assert.That(WaitFor(() => radio.Status.Unframed), Is.True);
                Assert.That(WaitFor(() => Interlocked.Read(ref raw) > 1000), Is.True);
                Assert.That(radio.Status.State, Is.EqualTo(NtripState.Streaming));
                cts.Cancel();
                run.Wait(2000);
            }
        }

        [Test]
        public void ManagerWithRadioFallback_SwitchesToTheRadio_WhenMainFails()
        {
            var feed = new FeedStream();
            var main = new FakeSource("MAIN", 1005);
            var manager = new CorrectionSourceManager(main, () => new RadioSource("COM5", () => feed), 2);
            var types = new List<int>();
            string notice = null;
            manager.FrameReceived += (t, f) => { lock (types) types.Add(t); };
            manager.Notice += (title, message) => notice = title + "|" + message;

            using (var cts = new CancellationTokenSource())
            {
                Task run = Task.Run(() => manager.RunAsync(cts.Token));
                main.Set(NtripState.Waiting, NtripCategory.CasterUnreachable, 2);
                Assert.That(manager.Active, Is.EqualTo(SourceRole.Fallback));
                Assert.That(notice, Does.Contain("Radio COM5"));

                feed.Feed(Rtcm.Message(1077, 60));
                Assert.That(WaitFor(() => manager.Status.Status.Frames >= 1), Is.True);
                lock (types) Assert.That(types, Is.EqualTo(new[] { 1077 }));
                Assert.That(manager.Status.Status.Headline, Is.EqualTo("Radio Active"));
                Assert.That(manager.Status.Using, Is.EqualTo("Fallback: Radio COM5"));

                cts.Cancel();
                Assert.That(run.Wait(3000), Is.True);
            }
        }
    }

    [TestFixture]
    public class FailoverWithRealClientsTests
    {
        private static byte[] Frames(params int[] types)
        {
            var l = new List<byte>();
            foreach (int t in types) l.AddRange(Rtcm.Message(t, 60));
            return l.ToArray();
        }

        // Header, then the same frames again every 150 ms, so the test can see who is forwarded and who is not.
        private static Func<FakeSession, Task> Stream(byte[] body, int holdMs = 8000) => async s =>
        {
            await s.SendAsync(System.Text.Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContent-Type: gnss/data\r\n\r\n"));
            for (int i = 0; i < holdMs / 150; i++)
            {
                await s.SendAsync(body);
                await s.WaitAsync(150);
            }
        };

        private static bool WaitFor(Func<bool> condition, int ms = 8000)
        {
            var until = DateTime.UtcNow.AddMilliseconds(ms);
            while (DateTime.UtcNow < until)
            {
                if (condition()) return true;
                Thread.Sleep(15);
            }
            return false;
        }

        [Test]
        public void MainCasterBusy_FallbackCasterTakesOver_ThenMainReturns_AndTheUserSwitchesBack()
        {
            using (var mainCaster = new FakeCaster())
            using (var fallbackCaster = new FakeCaster())
            {
                // the main caster refuses three times, then streams
                mainCaster.Script(3, s => s.SendAsync("HTTP/1.1 503 Service Unavailable\r\n\r\n"));
                mainCaster.Script(Stream(Frames(1005, 1074)));
                fallbackCaster.Script(Stream(Frames(1077, 1087)));

                var main = new NtripProfile("127.0.0.1", mainCaster.Port, "MAIN");
                var fallback = new NtripProfile("127.0.0.1", fallbackCaster.Port, "BACKUP");
                var delay = new Func<TimeSpan, CancellationToken, Task>((span, ct) => Task.Delay(100, ct));
                var manager = new CorrectionSourceManager(main, FallbackPlan.ForNtrip(fallback, 2), () => null,
                    Harness.QuickTimeouts(), delay, () => 0.5);

                var types = new List<int>();
                var notices = new List<string>();
                manager.FrameReceived += (t, f) => { lock (types) types.Add(t); };
                manager.Notice += (title, message) => { lock (notices) notices.Add(title); };

                using (var cts = new CancellationTokenSource())
                {
                    Task run = Task.Run(() => manager.RunAsync(cts.Token));

                    Assert.That(WaitFor(() => { lock (notices) return notices.Contains("Switched to the fallback"); }), Is.True, "switched");
                    Assert.That(WaitFor(() => { lock (types) return types.Contains(1077); }), Is.True, "fallback frames arrive");

                    Assert.That(WaitFor(() => { lock (notices) return notices.Contains("The main service is back"); }), Is.True, "main is back");
                    Assert.That(manager.Active, Is.EqualTo(SourceRole.Fallback), "no automatic switch back");
                    lock (types) Assert.That(types, Does.Not.Contain(1005), "main frames are not forwarded while on the fallback");

                    int fallbackBefore;
                    lock (types) fallbackBefore = types.FindAll(x => x == 1077).Count;

                    manager.SwitchBackToMain();
                    Assert.That(manager.Active, Is.EqualTo(SourceRole.Main));

                    Assert.That(WaitFor(() => { lock (types) return types.Contains(1005); }), Is.True, "main frames are forwarded after switching back");
                    Thread.Sleep(600);
                    int fallbackAfter;
                    lock (types) fallbackAfter = types.FindAll(x => x == 1077).Count;
                    Assert.That(fallbackAfter, Is.LessThanOrEqualTo(fallbackBefore + 1), "the fallback no longer delivers");
                    cts.Cancel();
                    Assert.That(run.Wait(5000), Is.True);
                }
            }
        }
    }
}

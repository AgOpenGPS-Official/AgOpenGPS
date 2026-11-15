using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;
using AgOpenGPS.Mobile.Core;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    /// <summary>A link whose reads block like a Bluetooth socket until data comes or it is closed.</summary>
    internal sealed class FakeLink : IGnssLink
    {
        private readonly BlockingStream _in = new BlockingStream();
        public Stream Input => _in;
        public Stream Output { get; } = new MemoryStream();
        public void DeviceSends(string text) => _in.Push(Encoding.ASCII.GetBytes(text));
        public void DeviceHangsUp() => _in.Complete();
        public bool Disposed { get; private set; }
        public void Dispose() { Disposed = true; _in.Complete(); }

        private sealed class BlockingStream : Stream
        {
            private readonly BlockingCollection<byte[]> _q = new BlockingCollection<byte[]>();
            private byte[] _cur = Array.Empty<byte>();
            private int _pos;
            public void Push(byte[] b) { if (!_q.IsAddingCompleted) _q.Add(b); }
            public void Complete() => _q.CompleteAdding();
            public override int Read(byte[] buffer, int offset, int count)
            {
                if (_pos >= _cur.Length)
                {
                    try { _cur = _q.Take(); } catch (InvalidOperationException) { return 0; }
                    _pos = 0;
                }
                int n = Math.Min(count, _cur.Length - _pos);
                Array.Copy(_cur, _pos, buffer, offset, n);
                _pos += n;
                return n;
            }
            public override bool CanRead => true;
            public override bool CanSeek => false;
            public override bool CanWrite => false;
            public override long Length => throw new NotSupportedException();
            public override long Position { get => 0; set => throw new NotSupportedException(); }
            public override void Flush() { }
            public override long Seek(long o, SeekOrigin s) => throw new NotSupportedException();
            public override void SetLength(long v) => throw new NotSupportedException();
            public override void Write(byte[] b, int o, int c) => throw new NotSupportedException();
        }
    }

    public class LinkSupervisorTests
    {
        private static bool WaitFor(Func<bool> condition, int ms = 5000)
        {
            return SpinWait.SpinUntil(condition, ms);
        }

        [Test]
        public void ReconnectsAfterTheDeviceHangsUp()
        {
            var links = new BlockingCollection<FakeLink>();
            int opened = 0;
            var sup = new LinkSupervisor(_ =>
            {
                var l = new FakeLink();
                Interlocked.Increment(ref opened);
                links.Add(l);
                return l;
            }, retryDelay: TimeSpan.FromMilliseconds(50));

            int fixes = 0;
            sup.FixUpdated += _ => Interlocked.Increment(ref fixes);
            sup.Start();

            Assert.That(links.TryTake(out FakeLink first, 5000), Is.True);
            first.DeviceSends(Sentences.Gga);
            Assert.That(WaitFor(() => fixes == 1), Is.True, "fix from the first link");
            Assert.That(sup.GetSnapshot().State, Is.EqualTo(LinkState.Connected));

            first.DeviceHangsUp();
            Assert.That(links.TryTake(out FakeLink second, 5000), Is.True, "reconnected");
            second.DeviceSends(Sentences.Gga);
            Assert.That(WaitFor(() => fixes == 2), Is.True, "fix from the second link");
            Assert.That(sup.GetSnapshot().Connects, Is.EqualTo(2));

            sup.Stop();
            Assert.That(second.Disposed, Is.True);
            Assert.That(sup.GetSnapshot().State, Is.EqualTo(LinkState.Stopped));
        }

        [Test]
        public void SilentLinkIsClosedAndReopened()
        {
            var links = new BlockingCollection<FakeLink>();
            string lastMessage = null;
            var sup = new LinkSupervisor(_ => { var l = new FakeLink(); links.Add(l); return l; },
                retryDelay: TimeSpan.FromMilliseconds(50), silenceLimit: TimeSpan.FromSeconds(1));
            sup.StateChanged += (s, m) => { if (s == LinkState.WaitingToRetry) lastMessage = m; };
            sup.Start();

            Assert.That(links.TryTake(out FakeLink first, 5000), Is.True);
            Assert.That(links.TryTake(out _, 6000), Is.True, "reopened after silence");
            Assert.That(first.Disposed, Is.True);
            Assert.That(lastMessage, Does.StartWith("No data"));
            sup.Stop();
        }

        [Test]
        public void FailedConnectIsRetried()
        {
            int attempts = 0;
            string message = null;
            var sup = new LinkSupervisor(_ =>
            {
                Interlocked.Increment(ref attempts);
                throw new IOException("read failed, socket might closed or timeout");
            }, retryDelay: TimeSpan.FromMilliseconds(20));
            sup.StateChanged += (s, m) => { if (s == LinkState.WaitingToRetry) message = m; };
            sup.Start();

            Assert.That(WaitFor(() => attempts >= 3), Is.True);
            sup.Stop();
            Assert.That(message, Does.Contain("socket might closed"));
        }
    }
}

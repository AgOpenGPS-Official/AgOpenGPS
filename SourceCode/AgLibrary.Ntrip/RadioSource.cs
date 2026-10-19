using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// A serial radio as a source of corrections, the fallback for a lost internet connection.
    /// The stream is opened through a function (a serial port in AgIO, a pipe in tests), framed like
    /// an NTRIP stream and reported with the same status, so the manager treats both alike.
    /// </summary>
    internal sealed class RadioSource : ICorrectionSource
    {
        private const int UnframedAfterBytes = 4096;

        private readonly string _name;
        private readonly Func<Stream> _open;
        private readonly TimeSpan _retry;
        private readonly TimeSpan _silence;
        private readonly Func<TimeSpan, CancellationToken, Task> _delay;
        private readonly RtcmStreamStats _stats = new RtcmStreamStats();
        private readonly object _statusLock = new object();
        private NtripStatus _status = new NtripStatus { SourceName = "Radio" };
        private long _bytes;
        private long _frames;
        private Stopwatch _sinceLastFrame;

        public event NtripClient.FrameHandler FrameReceived;
        public event NtripClient.RawHandler RawDataReceived;
        public event Action<NtripStatus> StatusChanged;

        /// <param name="name">The port name for messages, e.g. COM5.</param>
        /// <param name="open">Opens the radio; throws when it cannot.</param>
        public RadioSource(string name, Func<Stream> open, TimeSpan? retry = null, TimeSpan? silence = null,
            Func<TimeSpan, CancellationToken, Task> delay = null)
        {
            _name = name;
            _open = open;
            _retry = retry ?? TimeSpan.FromSeconds(5);
            _silence = silence ?? TimeSpan.FromSeconds(15);
            _delay = delay ?? ((span, ct) => Task.Delay(span, ct));
        }

        public string Description => "Radio " + _name;

        public NtripStatus Status
        {
            get { lock (_statusLock) return Refresh(_status.Clone()); }
        }

        public async Task RunAsync(CancellationToken ct)
        {
            int failures = 0;
            try
            {
                while (!ct.IsCancellationRequested)
                {
                    Update(s => { s.State = NtripState.Connecting; s.RetryInSeconds = 0; });

                    Stream stream;
                    try
                    {
                        stream = _open();
                    }
                    catch (Exception ex) when (!(ex is OperationCanceledException))
                    {
                        failures++;
                        Fail("Cannot open the radio on " + _name + ".", failures);
                        await _delay(_retry, ct).ConfigureAwait(false);
                        continue;
                    }

                    bool delivered;
                    using (stream)
                    using (ct.Register(() => { try { stream.Dispose(); } catch (Exception) { /* closing */ } }))
                    {
                        delivered = await ReadAsync(stream, ct).ConfigureAwait(false);
                    }

                    if (delivered) failures = 0;
                    failures++;
                    Fail("The radio on " + _name + " stopped. Reconnecting.", failures);
                    await _delay(_retry, ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // stopped by the caller
            }
            finally
            {
                Update(s => { s.State = NtripState.Stopped; s.RetryInSeconds = 0; });
            }
        }

        // Returns true when corrections had arrived before the stream ended.
        private async Task<bool> ReadAsync(Stream stream, CancellationToken ct)
        {
            _stats.Reset();
            _frames = 0;
            _bytes = 0;
            _sinceLastFrame = null;
            Update(s => { s.State = NtripState.Connected; s.LastError = null; s.Message = "The radio is open. Waiting for corrections."; s.Unframed = false; });

            var reader = new TimedReader(stream);
            var silent = Stopwatch.StartNew();
            bool unframed = false;
            long bodyBytes = 0;
            bool delivered = false;
            Rtcm3Framer.MessageHandler onFrame = (type, frame) =>
            {
                _frames++;
                _sinceLastFrame = Stopwatch.StartNew();
                FrameReceived?.Invoke(type, frame);
            };

            while (true)
            {
                int n;
                try { n = await reader.ReadAsync(TimeSpan.FromSeconds(1), ct).ConfigureAwait(false); }
                catch (IOException) { n = 0; }
                catch (ObjectDisposedException) { ct.ThrowIfCancellationRequested(); n = 0; }
                catch (InvalidOperationException) { n = 0; }

                if (n == 0) { reader.Abandon(); return delivered; }
                if (n < 0)
                {
                    if (silent.Elapsed >= _silence)
                    {
                        Update(s => { s.State = NtripState.Connected; s.Message = "The radio is open, but nothing arrives."; });
                    }
                    continue;
                }

                silent.Restart();
                _bytes += n;
                bodyBytes += n;
                long before = _frames;
                if (unframed)
                {
                    RawDataReceived?.Invoke(new ReadOnlySpan<byte>(reader.Buffer, 0, n));
                }
                else
                {
                    _stats.Feed(new ReadOnlySpan<byte>(reader.Buffer, 0, n), onFrame);
                }

                if (!unframed && _frames == 0 && bodyBytes >= UnframedAfterBytes)
                {
                    unframed = true;
                    Update(s => s.Unframed = true);
                }

                if (_frames > before || unframed)
                {
                    delivered = true;
                    Update(s =>
                    {
                        if (s.State != NtripState.Streaming)
                        {
                            s.State = NtripState.Streaming; s.Message = "Receiving corrections from the radio.";
                            s.LastError = null; s.FailuresInRow = 0; s.ConsecutiveFailures = 0;
                        }
                    });
                }
            }
        }

        private void Fail(string message, int failures)
        {
            Update(s =>
            {
                s.State = NtripState.Waiting;
                s.LastError = NtripCategory.CasterUnreachable;
                s.Message = message;
                s.FailuresInRow = failures;
                s.ConsecutiveFailures = failures;
                s.RetryInSeconds = _retry.TotalSeconds;
            });
        }

        private NtripStatus Refresh(NtripStatus s)
        {
            s.BytesReceived = _bytes;
            s.Frames = _frames;
            Stopwatch last = _sinceLastFrame;
            s.SecondsSinceLastFrame = last == null ? -1 : last.Elapsed.TotalSeconds;
            s.Rtcm = _stats.Snapshot();
            return s;
        }

        private void Update(Action<NtripStatus> change)
        {
            NtripStatus snapshot;
            lock (_statusLock)
            {
                NtripStatus next = _status.Clone();
                change(next);
                _status = next;
                snapshot = Refresh(next.Clone());
            }
            StatusChanged?.Invoke(snapshot);
        }
    }
}

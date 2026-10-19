using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// The state machine for one profile: connect, stream, and on any failure classify it, wait
    /// as the retry policy says and try again, until cancelled or until retrying cannot help.
    /// Runs on its own task and never touches the UI thread: frames go to <see cref="FrameReceived"/>
    /// on the receive loop, status snapshots to <see cref="StatusChanged"/>.
    /// </summary>
    public sealed class NtripClient
    {
        /// <summary>One whole RTCM 3 message. The span is only valid during the call.</summary>
        public delegate void FrameHandler(int messageType, ReadOnlySpan<byte> frame);

        /// <summary>Bytes of a stream that is not RTCM 3, forwarded as they come.</summary>
        public delegate void RawHandler(ReadOnlySpan<byte> data);

        /// <summary>Bytes without a single valid frame before the stream counts as not RTCM 3.</summary>
        private const int UnframedAfterBytes = 4096;

        private readonly NtripProfile _profile;
        private readonly Func<string> _ggaProvider;
        private readonly NtripTimeouts _timeouts;
        private readonly Func<TimeSpan, CancellationToken, Task> _delay;
        private readonly RetryPolicy _policy;
        private readonly RtcmStreamStats _stats;
        private readonly object _statusLock = new object();
        private NtripStatus _status = new NtripStatus();
        private long _bytes;
        private long _framesTotal;
        private Stopwatch _sinceLastFrame;

        public event FrameHandler FrameReceived;
        public event RawHandler RawDataReceived;
        public event Action<NtripStatus> StatusChanged;

        /// <param name="ggaProvider">Returns a complete GGA sentence for the caster, or null while there is no position.</param>
        /// <param name="delay">Replaces Task.Delay; tests pass one that returns at once.</param>
        /// <param name="random">Returns 0..1 for the retry jitter.</param>
        public NtripClient(NtripProfile profile, Func<string> ggaProvider = null, NtripTimeouts timeouts = null,
            Func<TimeSpan, CancellationToken, Task> delay = null, Func<double> random = null)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
            _ggaProvider = ggaProvider ?? (() => null);
            _timeouts = timeouts ?? new NtripTimeouts();
            _delay = delay ?? ((span, ct) => Task.Delay(span, ct));
            _policy = new RetryPolicy(random);
            _stats = new RtcmStreamStats();
        }

        public NtripProfile Profile => _profile;

        /// <summary>The latest snapshot.</summary>
        public NtripStatus Status
        {
            get { lock (_statusLock) return RefreshCounters(_status.Clone()); }
        }

        /// <summary>Runs until cancelled or until the caster keeps rejecting the login or mountpoint.</summary>
        public async Task RunAsync(CancellationToken ct)
        {
            // Raw TCP has no request, so the position goes out the way NTRIP 1.0 does it: right after connecting.
            var version = _profile.Version == NtripVersion.V1 || _profile.RawTcp ? NtripVersion.V1 : NtripVersion.V2;
            NtripCategory? rowCategory = null;
            int row = 0;

            try
            {
                while (!ct.IsCancellationRequested)
                {
                    Update(s => { s.State = NtripState.Connecting; s.RetryInSeconds = 0; s.VersionUsed = version; });

                    NtripOutcome outcome;
                    bool streamed = false;
                    using (NtripConnection conn = await NtripConnection.OpenAsync(
                        _profile, version, version == NtripVersion.V2 ? _ggaProvider() : null, _timeouts, ct).ConfigureAwait(false))
                    {
                        outcome = conn.Outcome;
                        Update(s => s.FirstLine = conn.Outcome.FirstLine);

                        // Auto: a caster that does not understand NTRIP 2.0 gets the 1.0 request at once, once.
                        if (outcome.Category == NtripCategory.NotUnderstood && _profile.Version == NtripVersion.Auto
                            && version == NtripVersion.V2)
                        {
                            version = NtripVersion.V1;
                            continue;
                        }

                        if (outcome.IsAccepted)
                        {
                            Update(s => { s.State = NtripState.Connected; s.LastError = null; s.Message = "Connected. Waiting for corrections."; });
                            outcome = await StreamAsync(conn, version, ct).ConfigureAwait(false);
                            streamed = _stats.Snapshot().Messages > 0 || Status.Unframed;
                        }
                    }

                    // A session that delivered corrections ends the earlier run of failures.
                    if (streamed) { rowCategory = null; row = 0; }
                    if (outcome.Category == rowCategory) row++; else { rowCategory = outcome.Category; row = 1; }

                    RetryDecision decision = _policy.Next(outcome.Category, row);
                    string text = Describe(outcome, row, decision);
                    if (decision.GiveUp)
                    {
                        Update(s => { s.State = NtripState.GaveUp; s.LastError = outcome.Category; s.Message = text; s.FailuresInRow = row; s.FirstLine = outcome.FirstLine; });
                        return;
                    }

                    Update(s =>
                    {
                        s.State = NtripState.Waiting; s.LastError = outcome.Category; s.Message = text;
                        s.FailuresInRow = row; s.RetryInSeconds = decision.Delay.TotalSeconds; s.FirstLine = outcome.FirstLine;
                    });
                    // The 1.0 fallback was only a probe: start the next round with 2.0 again.
                    if (_profile.Version == NtripVersion.Auto && !streamed) version = NtripVersion.V2;
                    if (decision.Delay > TimeSpan.Zero) await _delay(decision.Delay, ct).ConfigureAwait(false);
                }
            }
            catch (OperationCanceledException)
            {
                // Stopped by the caller.
            }
            finally
            {
                Update(s => { if (s.State != NtripState.GaveUp) { s.State = NtripState.Stopped; s.RetryInSeconds = 0; } });
            }
        }

        // Reads the body until the stream ends, goes silent or the caller cancels.
        private async Task<NtripOutcome> StreamAsync(NtripConnection conn, NtripVersion version, CancellationToken ct)
        {
            _stats.Reset();
            Update(s => s.Unframed = false);
            _framesTotal = 0;
            _bytes = 0;
            _sinceLastFrame = null;
            var decoder = conn.Outcome.Chunked ? new ChunkedDecoder() : null;
            var session = Stopwatch.StartNew();
            var sinceGga = Stopwatch.StartNew();
            bool unframed = false;
            long bodyBytes = 0;
            Rtcm3Framer.MessageHandler onFrame = (type, frame) =>
            {
                _framesTotal++;
                _sinceLastFrame = Stopwatch.StartNew();
                FrameReceived?.Invoke(type, frame);
            };
            var lastData = Stopwatch.StartNew();   // last frame, or the last bytes while the stream is unframed
            bool first = true;

            // N9: NTRIP 1.0 has no header for the position; send it right after the answer.
            if (version == NtripVersion.V1) await SendGgaAsync(conn, sinceGga).ConfigureAwait(false);
            sinceGga.Restart();

            while (true)
            {
                ct.ThrowIfCancellationRequested();
                TimeSpan untilSilent = _timeouts.NoData - lastData.Elapsed;
                TimeSpan wait = untilSilent;
                if (_profile.GgaIntervalSeconds > 0)
                {
                    TimeSpan untilGga = TimeSpan.FromSeconds(_profile.GgaIntervalSeconds) - sinceGga.Elapsed;
                    if (untilGga < wait) wait = untilGga;
                }

                int n;
                byte[] data;
                if (first)
                {
                    data = conn.InitialBody;
                    n = data.Length;
                    first = false;
                    if (n == 0) continue;
                }
                else
                {
                    try { n = await conn.Reader.ReadAsync(wait, ct).ConfigureAwait(false); }
                    catch (System.IO.IOException) { n = 0; }
                    catch (ObjectDisposedException) { ct.ThrowIfCancellationRequested(); n = 0; }
                    data = conn.Reader.Buffer;
                }

                if (n < 0)
                {
                    if (_profile.GgaIntervalSeconds > 0 && sinceGga.Elapsed >= TimeSpan.FromSeconds(_profile.GgaIntervalSeconds))
                        await SendGgaAsync(conn, sinceGga).ConfigureAwait(false);
                    if (lastData.Elapsed >= _timeouts.NoData)
                    {
                        return _framesTotal > 0 || unframed
                            ? Outcome(NtripCategory.StreamLost, "Corrections stopped. Reconnecting.")
                            : Outcome(NtripCategory.NoCorrections, "Connected, but nothing is coming in. Some services need your position: check that your GPS position is switched on.");
                    }
                    continue;
                }
                if (n == 0)
                {
                    return _framesTotal > 0 || unframed
                        ? Outcome(NtripCategory.StreamLost, "The service closed the connection. Reconnecting.")
                        : Outcome(NtripCategory.NoCorrections, "The service closed the connection without sending corrections.");
                }

                _bytes += n;
                bodyBytes += n;
                long framesBefore = _framesTotal;
                if (unframed)
                {
                    RawDataReceived?.Invoke(new ReadOnlySpan<byte>(data, 0, n));
                    lastData.Restart();
                }
                else if (decoder != null && !decoder.IsBroken)
                {
                    decoder.Feed(new ReadOnlySpan<byte>(data, 0, n), d => _stats.Feed(d, onFrame));
                }
                else
                {
                    _stats.Feed(new ReadOnlySpan<byte>(data, 0, n), onFrame);
                }
                if (_framesTotal > framesBefore) lastData.Restart();

                if (!unframed && _framesTotal == 0 && bodyBytes >= UnframedAfterBytes)
                {
                    // Not RTCM 3 (RTCM 2, CMR, ...): forward the bytes as the old client did.
                    unframed = true;
                    Update(s => s.Unframed = true);
                    lastData.Restart();
                }

                if (_framesTotal > 0 || unframed)
                {
                    Update(s =>
                    {
                        if (s.State != NtripState.Streaming)
                        {
                            s.State = NtripState.Streaming; s.Message = "Receiving corrections";
                            s.LastError = null; s.FailuresInRow = 0; s.RetryInSeconds = 0;
                        }
                    });
                }

                if (_profile.GgaIntervalSeconds > 0 && sinceGga.Elapsed >= TimeSpan.FromSeconds(_profile.GgaIntervalSeconds))
                    await SendGgaAsync(conn, sinceGga).ConfigureAwait(false);
            }
        }

        private async Task SendGgaAsync(NtripConnection conn, Stopwatch sinceGga)
        {
            sinceGga.Restart();
            string gga = _ggaProvider();
            if (gga == null) return;
            try { await conn.WriteAsync(gga).ConfigureAwait(false); }
            catch (System.IO.IOException) { /* the read side reports the broken connection */ }
            catch (ObjectDisposedException) { }
        }

        private static NtripOutcome Outcome(NtripCategory c, string message) => new NtripOutcome(c, message, "", false);

        private static string Describe(NtripOutcome o, int row, RetryDecision d)
        {
            string text = o.Message;
            if (d.MaxAttempts > 0)
                text += d.GiveUp ? " AgIO stopped trying: check the settings and press Start."
                                 : " Try " + row + " of " + d.MaxAttempts + ".";
            return text;
        }

        // Counters change on every read: read them when a snapshot is taken instead of on every update.
        private NtripStatus RefreshCounters(NtripStatus s)
        {
            s.BytesReceived = _bytes;
            s.Frames = _framesTotal;
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
                snapshot = RefreshCounters(next.Clone());
            }
            StatusChanged?.Invoke(snapshot);
        }
    }
}

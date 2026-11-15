using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Ntrip;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>An open connection to the GNSS device. Dispose closes it and unblocks a pending read.</summary>
    public interface IGnssLink : IDisposable
    {
        Stream Input { get; }
        Stream Output { get; }
    }

    public enum LinkState
    {
        Stopped,
        Connecting,
        Connected,
        WaitingToRetry,
    }

    public sealed class LinkSnapshot
    {
        public LinkState State;
        public string Message = "";
        public int Connects;
        public SessionStats Session;
        /// <summary>Null when NTRIP is off.</summary>
        public NtripStatus Ntrip;
        public long NtripBytesSent;
    }

    /// <summary>
    /// Keeps the device connected: opens the link, runs a <see cref="GnssSession"/> on it, forwards
    /// NTRIP corrections while it is up, and reconnects after the link drops or goes silent (a tractor
    /// cab is a noisy radio environment, and the receiver may be powered on after the tablet).
    /// </summary>
    public sealed class LinkSupervisor
    {
        private readonly Func<CancellationToken, IGnssLink> _open;
        private readonly Func<NtripProfile> _ntripProfile;
        private readonly TimeSpan _retryDelay;
        private readonly TimeSpan _silenceLimit;
        private readonly object _lock = new object();

        private CancellationTokenSource _cts;
        private Task _loop;
        private IGnssLink _link;
        private GnssSession _session;
        private CorrectionForwarder _forwarder;
        private LinkState _state = LinkState.Stopped;
        private string _message = "";
        private int _connects;

        public event Action<GnssFix> FixUpdated;
        public event Action<string> SentenceReceived;
        public event Action<LinkState, string> StateChanged;

        /// <param name="open">Opens the link; blocks until connected and throws on failure.</param>
        /// <param name="ntripProfile">Returns the caster to use, or null to leave corrections off. Read on every connect.</param>
        /// <param name="silenceLimit">A connected link that delivers nothing for this long is closed and reopened.</param>
        public LinkSupervisor(Func<CancellationToken, IGnssLink> open, Func<NtripProfile> ntripProfile = null,
            TimeSpan? retryDelay = null, TimeSpan? silenceLimit = null)
        {
            _open = open ?? throw new ArgumentNullException(nameof(open));
            _ntripProfile = ntripProfile ?? (() => null);
            _retryDelay = retryDelay ?? TimeSpan.FromSeconds(3);
            _silenceLimit = silenceLimit ?? TimeSpan.FromSeconds(10);
        }

        public bool IsRunning { get { lock (_lock) return _cts != null; } }

        public void Start()
        {
            lock (_lock)
            {
                if (_cts != null) return;
                _cts = new CancellationTokenSource();
                CancellationToken ct = _cts.Token;
                _loop = Task.Run(() => LoopAsync(ct));
            }
        }

        /// <summary>Stops and waits (briefly) for the link to close.</summary>
        public void Stop()
        {
            Task loop;
            lock (_lock)
            {
                if (_cts == null) return;
                _cts.Cancel();
                _link?.Dispose();
                loop = _loop;
                _cts = null;
            }
            try { loop?.Wait(TimeSpan.FromSeconds(3)); } catch (AggregateException) { }
            SetState(LinkState.Stopped, "");
        }

        public LinkSnapshot GetSnapshot()
        {
            lock (_lock)
            {
                return new LinkSnapshot
                {
                    State = _state,
                    Message = _message,
                    Connects = _connects,
                    Session = _session?.GetStats(),
                    Ntrip = _forwarder?.Status,
                    NtripBytesSent = _forwarder?.BytesSent ?? 0,
                };
            }
        }

        private async Task LoopAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                SetState(LinkState.Connecting, "Connecting...");
                IGnssLink link;
                try
                {
                    link = await Task.Run(() => _open(ct), ct).ConfigureAwait(false);
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    return;
                }
                catch (Exception ex)
                {
                    SetState(LinkState.WaitingToRetry, "Connect failed: " + ex.Message);
                    if (!await DelayAsync(_retryDelay, ct).ConfigureAwait(false)) return;
                    continue;
                }

                string reason = await RunLinkAsync(link, ct).ConfigureAwait(false);
                if (ct.IsCancellationRequested) return;

                SetState(LinkState.WaitingToRetry, reason);
                if (!await DelayAsync(_retryDelay, ct).ConfigureAwait(false)) return;
            }
        }

        private async Task<string> RunLinkAsync(IGnssLink link, CancellationToken ct)
        {
            var session = new GnssSession(link.Input, link.Output);
            session.FixUpdated += f => FixUpdated?.Invoke(f);
            session.SentenceReceived += s => SentenceReceived?.Invoke(s);

            CorrectionForwarder forwarder = null;
            NtripProfile profile = null;
            try { profile = _ntripProfile(); } catch (ArgumentException) { /* incomplete settings: run without */ }
            if (profile != null)
            {
                forwarder = new CorrectionForwarder(profile, chunk => session.Write(chunk, 0, chunk.Length), session.GgaForCaster);
            }

            lock (_lock)
            {
                if (ct.IsCancellationRequested) { link.Dispose(); return "Stopped"; }
                _link = link;
                _session = session;
                _forwarder = forwarder;
                _connects++;
            }
            SetState(LinkState.Connected, "Connected");

            string reason = "Link closed by the device";
            DateTime connectedAt = DateTime.UtcNow;
            using (var linkCts = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                Task ntrip = forwarder != null ? RunQuietly(forwarder.RunAsync(linkCts.Token)) : Task.CompletedTask;
                Task read = RunQuietly(session.RunAsync(linkCts.Token));
                Task watchdog = Task.Run(async () =>
                {
                    while (!linkCts.IsCancellationRequested)
                    {
                        if (!await DelayAsync(TimeSpan.FromSeconds(1), linkCts.Token).ConfigureAwait(false)) return;
                        SessionStats st = session.GetStats();
                        double quiet = st.SecondsSinceData;
                        if (quiet < 0) quiet = (DateTime.UtcNow - connectedAt).TotalSeconds;
                        if (quiet > _silenceLimit.TotalSeconds)
                        {
                            reason = "No data for " + (int)_silenceLimit.TotalSeconds + " s";
                            link.Dispose(); // unblocks the read
                            return;
                        }
                    }
                });

                await read.ConfigureAwait(false);
                linkCts.Cancel();
                link.Dispose();
                await ntrip.ConfigureAwait(false);
                await RunQuietly(watchdog).ConfigureAwait(false);
            }

            lock (_lock)
            {
                if (_link == link) _link = null;
            }
            return reason;
        }

        private static async Task RunQuietly(Task t)
        {
            try { await t.ConfigureAwait(false); }
            catch (OperationCanceledException) { }
            catch (Exception) { /* a failed read or caster just ends this link's run */ }
        }

        private static async Task<bool> DelayAsync(TimeSpan delay, CancellationToken ct)
        {
            try
            {
                await Task.Delay(delay, ct).ConfigureAwait(false);
                return true;
            }
            catch (OperationCanceledException)
            {
                return false;
            }
        }

        private void SetState(LinkState state, string message)
        {
            lock (_lock)
            {
                _state = state;
                _message = message ?? "";
            }
            StateChanged?.Invoke(state, message);
        }
    }
}

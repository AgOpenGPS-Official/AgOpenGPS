using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    public enum SourceRole
    {
        Main,
        Fallback,
    }

    /// <summary>What to switch to when the main service fails: another NTRIP service or a serial radio.</summary>
    public sealed class FallbackPlan
    {
        private readonly NtripProfile _ntrip;
        private readonly string _radioName;
        private readonly Func<Stream> _radioOpen;

        /// <summary>Failed attempts in a row on the main service before AgIO switches.</summary>
        public int SwitchAfterFailures { get; }

        private FallbackPlan(NtripProfile ntrip, string radioName, Func<Stream> radioOpen, int switchAfter)
        {
            _ntrip = ntrip;
            _radioName = radioName;
            _radioOpen = radioOpen;
            SwitchAfterFailures = Math.Max(1, switchAfter);
        }

        public static FallbackPlan ForNtrip(NtripProfile profile, int switchAfterFailures = 2)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            return new FallbackPlan(profile, null, null, switchAfterFailures);
        }

        /// <param name="openRadio">Opens the serial radio; throws when that fails.</param>
        public static FallbackPlan ForRadio(string portName, Func<Stream> openRadio, int switchAfterFailures = 2)
        {
            if (openRadio == null) throw new ArgumentNullException(nameof(openRadio));
            return new FallbackPlan(null, portName, openRadio, switchAfterFailures);
        }

        internal ICorrectionSource Create(Func<string> gga, NtripTimeouts timeouts,
            Func<TimeSpan, CancellationToken, Task> delay, Func<double> random)
        {
            if (_ntrip != null) return new NtripClient(_ntrip, gga, timeouts, delay, random);
            return new RadioSource(_radioName, _radioOpen, null, null, delay);
        }
    }

    /// <summary>A snapshot of the main service and the fallback for the UI.</summary>
    public sealed class ManagerStatus
    {
        /// <summary>The source whose corrections are used.</summary>
        public SourceRole Active { get; internal set; }

        /// <summary>The status of the active source: this is what the status card shows.</summary>
        public NtripStatus Status { get; internal set; }

        public NtripStatus Main { get; internal set; }

        /// <summary>Null until the fallback has been started.</summary>
        public NtripStatus Fallback { get; internal set; }

        public bool FallbackConfigured { get; internal set; }

        /// <summary>On the fallback, and the main service delivers corrections again. Waiting for the user.</summary>
        public bool MainBack { get; internal set; }

        /// <summary>Where the corrections come from at the moment, e.g. "Main" or "Fallback: Radio COM5".</summary>
        public string Using { get; internal set; } = "Main";
    }

    /// <summary>
    /// Runs the main service and, when it fails, the fallback. Switching happens after a number of
    /// failed attempts in a row (default 2) and always with a notice. The main service keeps trying in
    /// the background; when it delivers again the user is asked, and the manager never switches back
    /// by itself: a switch changes the base station, so the receiver loses its RTK fix.
    /// </summary>
    public sealed class CorrectionSourceManager
    {
        private readonly ICorrectionSource _main;
        private readonly Func<ICorrectionSource> _fallbackFactory;
        private readonly int _switchAfter;
        private readonly object _sync = new object();

        private ICorrectionSource _fallback;
        private CancellationTokenSource _fallbackCts;
        private Task _fallbackTask;
        private volatile int _active;                 // SourceRole as int, read on every frame
        private bool _mainBack;

        public event NtripClient.FrameHandler FrameReceived;
        public event NtripClient.RawHandler RawDataReceived;
        public event Action<ManagerStatus> StatusChanged;

        /// <summary>A title and a message for the user. Raised on a worker thread.</summary>
        public event Action<string, string> Notice;

        public CorrectionSourceManager(NtripProfile main, FallbackPlan fallback, Func<string> ggaProvider,
            NtripTimeouts timeouts = null, Func<TimeSpan, CancellationToken, Task> delay = null, Func<double> random = null)
            : this(new NtripClient(main, ggaProvider, timeouts, delay, random),
                  fallback == null ? (Func<ICorrectionSource>)null : () => fallback.Create(ggaProvider, timeouts, delay, random),
                  fallback == null ? 2 : fallback.SwitchAfterFailures)
        {
        }

        internal CorrectionSourceManager(ICorrectionSource main, Func<ICorrectionSource> fallbackFactory, int switchAfter)
        {
            _main = main;
            _fallbackFactory = fallbackFactory;
            _switchAfter = Math.Max(1, switchAfter);
            _active = (int)SourceRole.Main;

            main.FrameReceived += (type, frame) => OnFrame(SourceRole.Main, type, frame);
            main.RawDataReceived += data => OnRaw(SourceRole.Main, data);
            main.StatusChanged += OnMainStatus;
        }

        public SourceRole Active => (SourceRole)_active;

        public ManagerStatus Status
        {
            get { lock (_sync) return Build(); }
        }

        /// <summary>Runs until cancelled. Without a fallback it ends when the main service gives up.</summary>
        public async Task RunAsync(CancellationToken ct)
        {
            try
            {
                await _main.RunAsync(ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // stopped by the caller
            }

            // The main service gave up (wrong login): with a fallback there is still work to do.
            if (_fallbackFactory != null && !ct.IsCancellationRequested)
            {
                try { await Task.Delay(Timeout.Infinite, ct).ConfigureAwait(false); }
                catch (OperationCanceledException) { }
            }

            Task running;
            CancellationTokenSource cts;
            lock (_sync) { running = _fallbackTask; cts = _fallbackCts; }
            if (cts != null) cts.Cancel();
            try { if (running != null) await running.ConfigureAwait(false); }
            catch (OperationCanceledException) { }
        }

        /// <summary>The user accepts going back to the main service.</summary>
        public void SwitchBackToMain()
        {
            CancellationTokenSource cts;
            lock (_sync)
            {
                if (_active != (int)SourceRole.Fallback) return;
                _active = (int)SourceRole.Main;
                _mainBack = false;
                cts = _fallbackCts;
                DetachFallback();
            }

            if (cts != null) cts.Cancel();
            RaiseNotice("Back on the main service", "Corrections now come from the main service again.");
            RaiseStatus();
        }

        private void OnMainStatus(NtripStatus st)
        {
            bool switchNow = false;
            bool mainBackNow = false;

            lock (_sync)
            {
                if (_active == (int)SourceRole.Main)
                {
                    if (_fallbackFactory != null
                        && (st.State == NtripState.GaveUp || (st.State == NtripState.Waiting && st.ConsecutiveFailures >= _switchAfter)))
                    {
                        switchNow = true;
                    }
                }
                else if (st.State == NtripState.Streaming)
                {
                    if (!_mainBack) { _mainBack = true; mainBackNow = true; }
                }
                else
                {
                    _mainBack = false;
                }
            }

            if (switchNow) SwitchToFallback(st);
            if (mainBackNow)
            {
                RaiseNotice("The main service is back",
                    "Switch back to it? Your GPS may lose RTK for a moment. AgIO never switches back by itself.");
            }
            RaiseStatus();
        }

        private void SwitchToFallback(NtripStatus reason)
        {
            ICorrectionSource fallback;
            lock (_sync)
            {
                if (_active != (int)SourceRole.Main || _fallbackFactory == null) return;

                fallback = _fallbackFactory();
                _fallback = fallback;
                _fallbackCts = new CancellationTokenSource();
                _active = (int)SourceRole.Fallback;
                _mainBack = false;

                fallback.FrameReceived += OnFallbackFrame;
                fallback.RawDataReceived += OnFallbackRaw;
                fallback.StatusChanged += OnFallbackStatus;

                CancellationToken token = _fallbackCts.Token;
                ICorrectionSource started = fallback;
                _fallbackTask = Task.Run(() => started.RunAsync(token));
            }

            RaiseNotice("Switched to the fallback",
                "The main service has a problem (" + reason.Headline + "). Now using " + fallback.Description
                + ". Your GPS may lose RTK for a moment, and your position can shift a little if the two base stations use different coordinates.");
            RaiseStatus();
        }

        // Called with the lock held.
        private void DetachFallback()
        {
            ICorrectionSource fallback = _fallback;
            if (fallback != null)
            {
                fallback.FrameReceived -= OnFallbackFrame;
                fallback.RawDataReceived -= OnFallbackRaw;
                fallback.StatusChanged -= OnFallbackStatus;
            }
            _fallback = null;
            _fallbackCts = null;
        }

        private void OnFallbackFrame(int type, ReadOnlySpan<byte> frame) => OnFrame(SourceRole.Fallback, type, frame);

        private void OnFallbackRaw(ReadOnlySpan<byte> data) => OnRaw(SourceRole.Fallback, data);

        private void OnFallbackStatus(NtripStatus st) => RaiseStatus();

        // Only the active source passes its data on; the other one is only watched.
        private void OnFrame(SourceRole role, int type, ReadOnlySpan<byte> frame)
        {
            if (_active != (int)role) return;
            FrameReceived?.Invoke(type, frame);
        }

        private void OnRaw(SourceRole role, ReadOnlySpan<byte> data)
        {
            if (_active != (int)role) return;
            RawDataReceived?.Invoke(data);
        }

        private ManagerStatus Build()
        {
            NtripStatus main = _main.Status;
            NtripStatus fallback = _fallback == null ? null : _fallback.Status;
            bool onFallback = _active == (int)SourceRole.Fallback;
            return new ManagerStatus
            {
                Active = (SourceRole)_active,
                Main = main,
                Fallback = fallback,
                Status = onFallback && fallback != null ? fallback : main,
                FallbackConfigured = _fallbackFactory != null,
                MainBack = _mainBack,
                Using = onFallback ? "Fallback: " + (_fallback == null ? "" : _fallback.Description) : "Main",
            };
        }

        private void RaiseStatus()
        {
            ManagerStatus snapshot;
            lock (_sync) snapshot = Build();
            StatusChanged?.Invoke(snapshot);
        }

        private void RaiseNotice(string title, string message)
        {
            Notice?.Invoke(title, message);
        }
    }
}

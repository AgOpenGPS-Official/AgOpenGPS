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

        /// <summary>Failed attempts in a row on the main service before AgIO switches (or asks).</summary>
        public int SwitchAfterFailures { get; }

        /// <summary>True: AgIO switches by itself. False: AgIO only tells you and you switch by hand.</summary>
        public bool AutoSwitch { get; }

        /// <summary>
        /// Keep trying the main service in the background while on the fallback, to tell when it is back.
        /// Turn this off when both services use the same login on the same caster: a second login can
        /// push the first one off.
        /// </summary>
        public bool KeepMainRunning { get; }

        private FallbackPlan(NtripProfile ntrip, string radioName, Func<Stream> radioOpen, int switchAfter, bool autoSwitch, bool keepMain)
        {
            _ntrip = ntrip;
            _radioName = radioName;
            _radioOpen = radioOpen;
            SwitchAfterFailures = Math.Max(1, switchAfter);
            AutoSwitch = autoSwitch;
            KeepMainRunning = keepMain;
        }

        public static FallbackPlan ForNtrip(NtripProfile profile, int switchAfterFailures = 2, bool autoSwitch = true, bool keepMainRunning = true)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile));
            return new FallbackPlan(profile, null, null, switchAfterFailures, autoSwitch, keepMainRunning);
        }

        /// <param name="openRadio">Opens the serial radio; throws when that fails.</param>
        public static FallbackPlan ForRadio(string portName, Func<Stream> openRadio, int switchAfterFailures = 2, bool autoSwitch = true)
        {
            if (openRadio == null) throw new ArgumentNullException(nameof(openRadio));
            return new FallbackPlan(null, portName, openRadio, switchAfterFailures, autoSwitch, true);
        }

        /// <summary>True when the two services would log in with the same account on the same caster.</summary>
        public static bool SameAccount(NtripProfile main, NtripProfile other)
        {
            return main != null && other != null
                && string.Equals(main.Host, other.Host, StringComparison.OrdinalIgnoreCase)
                && main.Port == other.Port
                && string.Equals(main.User, other.User, StringComparison.Ordinal);
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

        /// <summary>On the main service, which has failed, and AgIO waits for the user to switch (manual mode).</summary>
        public bool MainFailing { get; internal set; }

        /// <summary>On the fallback and the main service is not being tried in the background.</summary>
        public bool MainPaused { get; internal set; }

        /// <summary>Where the corrections come from at the moment, e.g. "Main" or "Fallback: Radio COM5".</summary>
        public string Using { get; internal set; } = "Main";
    }

    /// <summary>
    /// Runs the main service and, when it fails, the fallback. By default AgIO switches by itself after a
    /// number of failed attempts in a row (2), always with a notice; in manual mode it only tells you and you
    /// switch yourself, which you can do at any time. The main service keeps trying in the background and
    /// the user is asked before switching back; the manager never switches back by itself, because a switch
    /// changes the base station and the receiver loses its RTK fix.
    /// </summary>
    public sealed class CorrectionSourceManager
    {
        private readonly ICorrectionSource _main;
        private readonly Func<ICorrectionSource> _fallbackFactory;
        private readonly int _switchAfter;
        private readonly bool _autoSwitch;
        private readonly bool _keepMainRunning;
        private readonly object _sync = new object();

        private ICorrectionSource _fallback;
        private CancellationTokenSource _fallbackCts;
        private Task _fallbackTask;
        private CancellationTokenSource _mainCts;
        private Task _mainTask;
        private CancellationToken _runToken;
        private volatile int _active;                 // SourceRole as int, read on every frame
        private bool _mainBack;
        private bool _mainPaused;
        private bool _mainFailing;
        private bool _failureNotified;
        private bool _switchedByUser;

        public event NtripClient.FrameHandler FrameReceived;
        public event NtripClient.RawHandler RawDataReceived;
        public event Action<ManagerStatus> StatusChanged;

        /// <summary>A title and a message for the user. Raised on a worker thread.</summary>
        public event Action<string, string> Notice;

        public CorrectionSourceManager(NtripProfile main, FallbackPlan fallback, Func<string> ggaProvider,
            NtripTimeouts timeouts = null, Func<TimeSpan, CancellationToken, Task> delay = null, Func<double> random = null)
            : this(new NtripClient(main, ggaProvider, timeouts, delay, random),
                  fallback == null ? (Func<ICorrectionSource>)null : () => fallback.Create(ggaProvider, timeouts, delay, random),
                  fallback == null ? 2 : fallback.SwitchAfterFailures,
                  fallback == null || fallback.AutoSwitch,
                  fallback == null || fallback.KeepMainRunning)
        {
        }

        internal CorrectionSourceManager(ICorrectionSource main, Func<ICorrectionSource> fallbackFactory, int switchAfter,
            bool autoSwitch = true, bool keepMainRunning = true)
        {
            _main = main;
            _fallbackFactory = fallbackFactory;
            _switchAfter = Math.Max(1, switchAfter);
            _autoSwitch = autoSwitch;
            _keepMainRunning = keepMainRunning;
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
            bool startMain;
            lock (_sync) { _runToken = ct; startMain = !_mainPaused; }
            if (startMain) StartMain();

            Task main;
            lock (_sync) main = _mainTask;
            try { if (main != null) await main.ConfigureAwait(false); }
            catch (OperationCanceledException) { }

            // With a fallback there is still work to do after the main service gave up or was paused.
            if (_fallbackFactory != null && !ct.IsCancellationRequested)
            {
                try { await Task.Delay(Timeout.Infinite, ct).ConfigureAwait(false); }
                catch (OperationCanceledException) { }
            }

            Task running;
            CancellationTokenSource fallbackCts, mainCts;
            Task currentMain;
            lock (_sync) { running = _fallbackTask; fallbackCts = _fallbackCts; mainCts = _mainCts; currentMain = _mainTask; }
            if (fallbackCts != null) fallbackCts.Cancel();
            if (mainCts != null) mainCts.Cancel();
            try { if (running != null) await running.ConfigureAwait(false); }
            catch (OperationCanceledException) { }
            try { if (currentMain != null) await currentMain.ConfigureAwait(false); }
            catch (OperationCanceledException) { }
        }

        /// <summary>The user switches to the fallback now, whatever the main service is doing.</summary>
        public void SwitchToFallbackNow()
        {
            if (_fallbackFactory == null) return;
            SwitchToFallback(null, true);
        }

        /// <summary>The user goes back to the main service (after being asked, or by choice).</summary>
        public void SwitchBackToMain()
        {
            CancellationTokenSource cts;
            bool restartMain;
            lock (_sync)
            {
                if (_active != (int)SourceRole.Fallback) return;
                _active = (int)SourceRole.Main;
                _mainBack = false;
                _switchedByUser = false;
                _failureNotified = false;
                restartMain = _mainPaused;
                cts = _fallbackCts;
                DetachFallback();
            }

            if (cts != null) cts.Cancel();
            if (restartMain) StartMain();
            RaiseNotice("Back on the main service", "Corrections now come from the main service again.");
            RaiseStatus();
        }

        /// <summary>Try the main service again while on the fallback, when it is not being tried in the background.</summary>
        public void TryMainAgain()
        {
            bool start;
            lock (_sync) start = _active == (int)SourceRole.Fallback && _mainPaused;
            if (start) StartMain();
            RaiseStatus();
        }

        private void StartMain()
        {
            lock (_sync)
            {
                _mainCts = CancellationTokenSource.CreateLinkedTokenSource(_runToken);
                CancellationToken token = _mainCts.Token;
                _mainPaused = false;
                _mainTask = Task.Run(() => _main.RunAsync(token));
            }
        }

        private void PauseMain()
        {
            CancellationTokenSource cts;
            lock (_sync) { cts = _mainCts; _mainPaused = true; }
            if (cts != null) cts.Cancel();
        }

        private void OnMainStatus(NtripStatus st)
        {
            bool switchNow = false;
            bool askNow = false;
            bool mainBackNow = false;

            lock (_sync)
            {
                if (_active == (int)SourceRole.Main)
                {
                    bool failing = st.State == NtripState.GaveUp
                        || (st.State == NtripState.Waiting && st.ConsecutiveFailures >= _switchAfter);
                    _mainFailing = failing && _fallbackFactory != null && !_autoSwitch;

                    if (failing && _fallbackFactory != null)
                    {
                        if (_autoSwitch) switchNow = true;
                        else if (!_failureNotified) { _failureNotified = true; askNow = true; }
                    }
                    else if (!failing)
                    {
                        _failureNotified = false;
                    }
                }
                else if (st.State == NtripState.Streaming)
                {
                    // Someone who switched by hand knows the main service works; only a failure-driven switch asks.
                    if (!_mainBack && !_switchedByUser) { _mainBack = true; mainBackNow = true; }
                }
                else
                {
                    _mainBack = false;
                }
            }

            if (switchNow) SwitchToFallback(st, false);
            if (askNow)
            {
                RaiseNotice("The main service has a problem",
                    st.Headline + ". Switch to the fallback? You can do that in the NTRIP panel.");
            }
            if (mainBackNow)
            {
                RaiseNotice("The main service is back",
                    "Switch back to it? Your GPS may lose RTK for a moment. AgIO never switches back by itself.");
            }
            RaiseStatus();
        }

        private void SwitchToFallback(NtripStatus reason, bool byUser)
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
                _mainFailing = false;
                _switchedByUser = byUser;

                fallback.FrameReceived += OnFallbackFrame;
                fallback.RawDataReceived += OnFallbackRaw;
                fallback.StatusChanged += OnFallbackStatus;

                CancellationToken token = _fallbackCts.Token;
                ICorrectionSource started = fallback;
                _fallbackTask = Task.Run(() => started.RunAsync(token));
            }

            if (!_keepMainRunning) PauseMain();

            string why = byUser ? "You switched to the fallback." : "The main service has a problem (" + reason.Headline + ").";
            RaiseNotice("Switched to the fallback",
                why + " Now using " + fallback.Description
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
                MainFailing = _mainFailing,
                MainPaused = onFallback && _mainPaused,
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

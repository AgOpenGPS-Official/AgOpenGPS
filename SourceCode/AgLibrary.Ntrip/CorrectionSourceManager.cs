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

    /// <summary>What a notice from the manager is about, so the UI can ask a question or just show a message.</summary>
    public enum NoticeKind
    {
        /// <summary>AgIO or the user switched to the fallback.</summary>
        SwitchedToFallback,
        /// <summary>Back on the main service.</summary>
        SwitchedToMain,
        /// <summary>The main service is back; the user is asked whether to switch back.</summary>
        MainBack,
        /// <summary>Manual mode: the main service has a problem; the user is asked whether to switch.</summary>
        MainFailing,
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
    /// Make before break: when the main service has failed enough, the fallback is started in the
    /// background and AgIO switches (or asks) only once the fallback delivers corrections itself. A lost
    /// internet connection (a phone hotspot that drops) fails both, so nothing is switched and the main
    /// service simply resumes.
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
        public event Action<NoticeKind, string, string> Notice;

        public CorrectionSourceManager(NtripProfile main, FallbackPlan fallback, Func<string> ggaProvider,
            NtripTimeouts timeouts = null, Func<TimeSpan, CancellationToken, Task> delay = null, Func<double> random = null)
            : this(new NtripClient(main, ggaProvider, timeouts, delay, random),
                  fallback == null ? (Func<ICorrectionSource>)null : () => fallback.Create(ggaProvider, timeouts, delay, random),
                  fallback == null ? 2 : fallback.SwitchAfterFailures,
                  fallback == null || fallback.AutoSwitch,
                  fallback == null || fallback.KeepMainRunning)
        {
        }

        /// <summary>The main service is a plan too, so it can be a serial radio instead of an NTRIP service.</summary>
        public CorrectionSourceManager(FallbackPlan main, FallbackPlan fallback, Func<string> ggaProvider,
            NtripTimeouts timeouts = null, Func<TimeSpan, CancellationToken, Task> delay = null, Func<double> random = null)
            : this(main.Create(ggaProvider, timeouts, delay, random),
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
            if (_fallbackFactory != null) _ = Task.Run(() => WatchNetworkAsync(ct));

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
        public void SwitchBackToMain() => SwitchBackToMain(false);

        // quiet: the manager itself goes back to the main service to try it first; no "back on main" message,
        // because it is not known to work yet.
        private void SwitchBackToMain(bool quiet)
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
            if (!quiet) RaiseNotice(NoticeKind.SwitchedToMain, "Back on the main service", "Corrections now come from the main service again.");
            RaiseStatus();
        }

        // The network came back. When nothing delivers, the main service is the first to be tried again; the
        // fallback only gets its turn when the main service fails again (OnMainStatus starts it then).
        // A source that still delivers is left alone: network changes also happen while all is well.
        private void OnNetworkBack()
        {
            CancellationTokenSource stop = null;
            bool backToMain = false;

            lock (_sync)
            {
                if (_fallbackFactory == null) return;
                // A main service that gave up (wrong login) is not running: the network changes nothing for it.
                if (_main.Status.State == NtripState.GaveUp) return;

                if (_active == (int)SourceRole.Main)
                {
                    // Both were failing: drop the fallback that was tried in the background, so it cannot win the race.
                    if (_fallback != null && _fallback.Status.State != NtripState.Streaming
                        && _main.Status.State != NtripState.Streaming)
                    {
                        stop = _fallbackCts;
                        DetachFallback();
                    }
                }
                else if (!_switchedByUser && _fallback != null && _fallback.Status.State != NtripState.Streaming)
                {
                    // On the fallback, which lost its connection too. A fallback chosen by hand stays the user's choice.
                    backToMain = true;
                }
            }

            if (stop != null) stop.Cancel();
            if (backToMain) SwitchBackToMain(true);
            else if (stop != null) RaiseStatus();
        }

        private async Task WatchNetworkAsync(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                try
                {
                    Task changed = NetworkWake.Next;
                    await Task.WhenAny(changed, Task.Delay(Timeout.Infinite, ct)).ConfigureAwait(false);
                    if (ct.IsCancellationRequested) return;
                    OnNetworkBack();
                }
                catch (OperationCanceledException) { return; }
            }
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
            bool stopCandidate = false;
            bool mainBackNow = false;

            lock (_sync)
            {
                if (_active == (int)SourceRole.Main)
                {
                    bool failing = st.State == NtripState.GaveUp
                        || (st.State == NtripState.Waiting && st.ConsecutiveFailures >= _switchAfter);
                    _mainFailing = failing && _fallbackFactory != null && !_autoSwitch;

                    if (failing && _fallbackFactory != null) StartFallbackLocked();
                    else if (!failing) _failureNotified = false;

                    // The main service delivers again before the fallback was needed.
                    stopCandidate = st.State == NtripState.Streaming && _fallback != null;
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

            if (stopCandidate) StopCandidate();
            if (mainBackNow)
            {
                RaiseNotice(NoticeKind.MainBack, "The main service is back",
                    "Switch back to it? Your GPS may lose RTK for a moment. AgIO never switches back by itself.");
            }
            RaiseStatus();
        }

        // The fallback is delivering corrections while still on the main service: it is a service that works.
        private void OnFallbackStatus(NtripStatus st)
        {
            bool switchNow = false;
            bool askNow = false;
            NtripStatus reason = null;

            lock (_sync)
            {
                if (_active == (int)SourceRole.Main && _fallback != null && st.State == NtripState.Streaming)
                {
                    reason = _main.Status;
                    if (_autoSwitch) switchNow = true;
                    else if (!_failureNotified) { _failureNotified = true; askNow = true; }
                }
            }

            if (switchNow) SwitchToFallback(reason, false);
            if (askNow)
            {
                RaiseNotice(NoticeKind.MainFailing, "The main service has a problem",
                    reason.Headline + ". The fallback works. Switch to it? You can do that in the NTRIP panel.");
            }
            RaiseStatus();
        }

        // Starts the fallback without using its corrections yet. Called with the lock held.
        private void StartFallbackLocked()
        {
            if (_fallback != null || _fallbackFactory == null) return;

            ICorrectionSource fallback = _fallbackFactory();
            _fallback = fallback;
            _fallbackCts = new CancellationTokenSource();

            // Its frames are ignored until it is the active source (OnFrame).
            fallback.FrameReceived += OnFallbackFrame;
            fallback.RawDataReceived += OnFallbackRaw;
            fallback.StatusChanged += OnFallbackStatus;

            CancellationToken token = _fallbackCts.Token;
            _fallbackTask = Task.Run(() => fallback.RunAsync(token));
        }

        // The main service is streaming again: the fallback that was started in the background is not needed.
        private void StopCandidate()
        {
            CancellationTokenSource cts;
            lock (_sync)
            {
                if (_active != (int)SourceRole.Main || _fallback == null) return;
                cts = _fallbackCts;
                DetachFallback();
            }
            if (cts != null) cts.Cancel();
            RaiseStatus();
        }

        private void SwitchToFallback(NtripStatus reason, bool byUser)
        {
            ICorrectionSource fallback;
            lock (_sync)
            {
                if (_active != (int)SourceRole.Main || _fallbackFactory == null) return;

                StartFallbackLocked();   // already running when it was tried in the background first
                fallback = _fallback;
                _active = (int)SourceRole.Fallback;
                _mainBack = false;
                _mainFailing = false;
                _switchedByUser = byUser;
            }

            if (!_keepMainRunning) PauseMain();

            string why = byUser ? "You switched to the fallback." : "The main service has a problem (" + (reason == null ? "no corrections" : reason.Headline) + ").";
            RaiseNotice(NoticeKind.SwitchedToFallback, "Switched to the fallback",
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

        private void RaiseNotice(NoticeKind kind, string title, string message)
        {
            Notice?.Invoke(kind, title, message);
        }
    }
}

using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// Runs the NTRIP corrections for AgIO: the main service, an optional fallback (another NTRIP service
    /// or a serial radio) and the output of the corrections to the GPS serial port or to UDP. Everything
    /// here works off the UI thread. The form only reads <see cref="Status"/> and a few counters, and
    /// feeds in the position once a second.
    /// </summary>
    internal sealed class NtripHost : IDisposable
    {
        /// <summary>Largest datagram, as AgIO has always sent it. The AiO firmwares cannot take more than 512.</summary>
        private const int PacketSize = 256;

        private sealed class Position
        {
            public double Latitude, Longitude, Altitude, Age, Hdop;
            public int FixQuality, Satellites;
        }

        private readonly FormLoop mf;
        private readonly object sync = new object();

        private CorrectionSourceManager manager;
        private NtripProfile mainProfile;
        private string mainRadio;
        private NtripProfile fallbackProfile;
        private CancellationTokenSource cts;
        private Task run;
        private RtcmQueue queue;
        private Thread pump;
        private volatile bool pumpStop;
        private readonly AutoResetEvent wake = new AutoResetEvent(false);

        private volatile Position position;
        private bool manualPosition;
        private double manualLat, manualLon, manualAlt;
        private bool toSerial, toUdp;

        private long bytesOut, datagramsOut;
        private long bytesAtLastTake;
        private DateTime lastTake = DateTime.UtcNow;
        private NtripCategory? lastNotified;

        // How long the corrections must be missing before the user is told; the check runs once a second.
        private static readonly TimeSpan AlertAfter = TimeSpan.FromSeconds(60);
        private static readonly TimeSpan AlertAfterHard = TimeSpan.FromSeconds(15);
        private Timer alertTimer;
        private readonly object alertSync = new object();
        private DateTime? downSince;        // when the corrections stopped (or never started); null while streaming
        private bool alerted;              // an alert was shown in this outage
        private bool alertOpen;            // the user has not closed it yet
        private bool retriedSinceAlert;    // a new attempt started after the alert was shown
        private SourceRole alertRole;      // the source the outage clock belongs to
        private int alertDismissals;       // times the user clicked the alert away in this outage
        private const int MaxDismissals = 2;   // after this the user knows, and the alert stays away

        /// <summary>A problem the user should hear about once: title and plain message. Raised on a worker thread.</summary>
        public event Action<string, string> Problem;

        /// <summary>
        /// The corrections have been missing for a minute or more (no network, caster unreachable, or connected
        /// without data). Raised again after every retry that fails, until corrections flow again.
        /// Title and plain message. Raised on a worker thread.
        /// </summary>
        public event Action<string, string> Alert;

        /// <summary>Corrections flow again, so an open alert can go away. Raised on a worker thread.</summary>
        public event Action AlertCleared;

        /// <summary>Corrections flow again after the user was told about the outage. Title and message. Raised on a worker thread.</summary>
        public event Action<string, string> Recovered;

        /// <summary>A notice from the switching between main and fallback. Raised on a worker thread.</summary>
        public event Action<NoticeKind, string, string> Notice;

        public NtripHost(FormLoop form)
        {
            mf = form;
            // A retry waiting out its pause tries at once when the network changes (a hotspot that came back).
            NetworkChange.NetworkAddressChanged += OnNetworkChanged;
            NetworkChange.NetworkAvailabilityChanged += OnNetworkChanged;
        }

        private static void OnNetworkChanged(object sender, EventArgs e)
        {
            NetworkWake.Signal();
        }

        public bool IsRunning
        {
            get { lock (sync) return run != null && !run.IsCompleted; }
        }

        /// <summary>The status of the source that delivers the corrections.</summary>
        public NtripStatus Status
        {
            get
            {
                CorrectionSourceManager m = manager;
                return m == null ? new NtripStatus() : m.Status.Status;
            }
        }

        public ManagerStatus Manager
        {
            get
            {
                CorrectionSourceManager m = manager;
                return m == null ? null : m.Status;
            }
        }

        /// <summary>The caster gets the Fixed Position of the profile instead of the GPS position.</summary>
        public bool UsesFixedPosition
        {
            get { return manualPosition; }
        }

        public double FixedLatitude
        {
            get { return manualLat; }
        }

        public double FixedLongitude
        {
            get { return manualLon; }
        }

        /// <summary>The main service.</summary>
        public NtripProfile Profile
        {
            get { return mainProfile; }
        }

        /// <summary>The port of the main radio, e.g. COM5; null when the main source is an NTRIP service.</summary>
        public string MainRadio
        {
            get { return mainRadio; }
        }

        /// <summary>True while the fallback delivers the corrections.</summary>
        public bool FallbackActive
        {
            get
            {
                ManagerStatus ms = Manager;
                return ms != null && ms.Active == SourceRole.Fallback;
            }
        }

        /// <summary>The service the corrections come from now: the fallback while it is in use. Null for a radio fallback.</summary>
        public NtripProfile ActiveProfile
        {
            get { return FallbackActive ? fallbackProfile : mainProfile; }
        }

        public void SwitchToFallback()
        {
            CorrectionSourceManager m = manager;
            // A stopped host keeps its manager: switching it would start a source that nothing stops again.
            if (m != null && IsRunning) m.SwitchToFallbackNow();
        }

        public void SwitchBackToMain()
        {
            CorrectionSourceManager m = manager;
            if (m != null && IsRunning) m.SwitchBackToMain();
        }

        public void TryMainAgain()
        {
            CorrectionSourceManager m = manager;
            if (m != null && IsRunning) m.TryMainAgain();
        }

        /// <summary>The active profile's main service as connection settings, or null while it has no address.</summary>
        public static NtripProfile MainProfile(NtripProfileSet set)
        {
            return set == null ? null : set.MainProfile(Properties.Settings.Default.setNTRIP_sendGGAInterval);
        }

        private static FallbackPlan BuildFallback(NtripProfileSet set, NtripProfile main)
        {
            if (set == null || set.Fallback == null || !set.Fallback.Enabled) return null;
            FallbackDefinition f = set.Fallback;

            if (f.Type == FallbackType.SerialRadio)
            {
                string port = f.RadioPort;
                int baud = f.RadioBaud;
                if (string.IsNullOrWhiteSpace(port)) return null;
                return FallbackPlan.ForRadio(port, () => new SerialRadioStream(port, baud), f.SwitchAfterFailures, f.AutoSwitch);
            }

            NtripProfile fallback = set.FallbackProfile(Properties.Settings.Default.setNTRIP_sendGGAInterval);
            if (fallback == null) return null;

            // The same login on the same caster: a second connection may push the first one off,
            // so the main service is not tried in the background while the fallback is in use.
            bool keepMain = !FallbackPlan.SameAccount(main, fallback);
            return FallbackPlan.ForNtrip(fallback, f.SwitchAfterFailures, f.AutoSwitch, keepMain);
        }
        // The profile the settings point to is missing on this computer, or has no address yet.
        private static string NoServiceReason(NtripProfileSet set)
        {
            string name = Properties.Settings.Default.setNTRIP_profileName;
            if (string.IsNullOrWhiteSpace(name)) return "No NTRIP profile is chosen yet. Choose or make one in the NTRIP settings.";
            if (!NtripProfiles.Store.Exists(name))
                return "The NTRIP profile \"" + name + "\" is not on this computer. Choose or make a profile in the NTRIP settings.";
            return "The NTRIP profile \"" + name + "\" has no service address yet.";
        }

        /// <summary>Why the last <see cref="Start"/> did not start anything, in a sentence for the user; empty when it did.</summary>
        public string StartProblem { get; private set; } = "";

        /// <summary>Starts (or restarts) with the current settings. Check <see cref="IsRunning"/> and <see cref="StartProblem"/> afterwards.</summary>
        public void Start()
        {
            Stop();
            StartProblem = "";

            var s = Properties.Settings.Default;
            NtripProfileSet set = NtripProfiles.LoadActive();
            NtripProfile main = MainProfile(set);
            FallbackPlan mainPlan;
            if (set.MainIsRadio)
            {
                string port = set.MainRadioPort;
                int baud = set.MainRadioBaud;
                if (string.IsNullOrWhiteSpace(port))
                {
                    StartProblem = "Choose the port of the main radio in the NTRIP profile.";
                    return;
                }
                mainPlan = FallbackPlan.ForRadio(port, () => new SerialRadioStream(port, baud));
                mainRadio = port;
            }
            else
            {
                if (main == null)
                {
                    StartProblem = NoServiceReason(set);
                    return;
                }
                mainPlan = FallbackPlan.ForNtrip(main);
                mainRadio = null;
            }
            mainProfile = main;
            fallbackProfile = set.Fallback != null && set.Fallback.Enabled ? set.FallbackProfile(s.setNTRIP_sendGGAInterval) : null;

            //the Fixed Position belongs to the profile
            FixedPositionDefinition fixedPosition = set.FixedPosition;
            manualPosition = fixedPosition != null && fixedPosition.Enabled;
            manualLat = fixedPosition == null ? 0 : fixedPosition.Latitude;
            manualLon = fixedPosition == null ? 0 : fixedPosition.Longitude;
            manualAlt = fixedPosition == null ? 0 : fixedPosition.Altitude;
            toSerial = s.setNTRIP_sendToSerial;
            toUdp = s.setNTRIP_sendToUDP;

            // Serial ports take about 5 KB/s in the old metering (256 bytes per 50 ms); UDP alone can go faster.
            queue = new RtcmQueue(null, PacketSize, toSerial ? 50 : RtcmQueue.IntervalMs);
            mf.SetNtripEndpoint(s.setNTRIP_sendToUDPPort);

            manager = new CorrectionSourceManager(mainPlan, BuildFallback(set, main), BuildGga);
            manager.FrameReceived += OnFrame;
            manager.RawDataReceived += OnRaw;
            manager.StatusChanged += OnStatusChanged;
            manager.Notice += OnNotice;
            lastNotified = null;
            ResetAlert(true);
            alertTimer = new Timer(_ => CheckAlert(), null, 1000, 1000);

            pumpStop = false;
            pump = new Thread(PumpLoop) { IsBackground = true, Name = "NTRIP output" };
            pump.Start();

            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            CorrectionSourceManager started = manager;
            lock (sync) run = Task.Run(() => started.RunAsync(token));
        }

        public void Stop()
        {
            Timer t = alertTimer;
            alertTimer = null;
            if (t != null) t.Dispose();
            ResetAlert(false);

            CancellationTokenSource c = cts;
            Task r;
            lock (sync) r = run;
            cts = null;

            if (c != null) c.Cancel();
            try { if (r != null) r.Wait(3000); } catch (Exception) { /* cancelled */ }

            pumpStop = true;
            wake.Set();
            Thread p = pump;
            if (p != null) p.Join(1000);
            pump = null;

            CorrectionSourceManager old = manager;
            if (old != null)
            {
                old.FrameReceived -= OnFrame;
                old.RawDataReceived -= OnRaw;
                old.StatusChanged -= OnStatusChanged;
                old.Notice -= OnNotice;
            }
            if (c != null) c.Dispose();
            lock (sync) run = null;
        }

        public void Dispose()
        {
            NetworkChange.NetworkAddressChanged -= OnNetworkChanged;
            NetworkChange.NetworkAvailabilityChanged -= OnNetworkChanged;
            Stop();
            wake.Dispose();
        }

        /// <summary>Called by the form once a second, on the UI thread, with the receiver's latest values.</summary>
        public void UpdatePosition(double latitude, double longitude, int fixQuality, int satellites, double hdop, double altitude, double age)
        {
            position = new Position
            {
                Latitude = latitude,
                Longitude = longitude,
                FixQuality = fixQuality,
                Satellites = satellites,
                Hdop = hdop,
                Altitude = altitude,
                Age = age,
            };
        }

        /// <summary>Datagrams and bytes sent to the outputs since the last call, and the bytes per second.</summary>
        public void TakeTraffic(out int datagrams, out int bytes, out double bytesPerSecond)
        {
            long b = Interlocked.Read(ref bytesOut);
            long d = Interlocked.Exchange(ref datagramsOut, 0);
            DateTime now = DateTime.UtcNow;
            double seconds = Math.Max(0.001, (now - lastTake).TotalSeconds);
            bytes = (int)(b - bytesAtLastTake);
            bytesPerSecond = bytes / seconds;
            datagrams = (int)d;
            bytesAtLastTake = b;
            lastTake = now;
        }

        private string BuildGga()
        {
            DateTime utc = DateTime.UtcNow;
            if (manualPosition)
                return GgaBuilder.FromManual(utc, manualLat, manualLon, manualAlt);

            Position p = position;
            if (p == null) return null;
            return GgaBuilder.FromReceiver(utc, p.Latitude, p.Longitude, p.FixQuality, p.Satellites, p.Hdop, p.Altitude, p.Age);
        }

        private void OnFrame(int messageType, ReadOnlySpan<byte> frame)
        {
            RtcmQueue q = queue;
            if (q == null) return;
            q.Enqueue(messageType, frame);
            wake.Set();
        }

        private void OnRaw(ReadOnlySpan<byte> data)
        {
            RtcmQueue q = queue;
            if (q == null) return;
            q.Enqueue(RtcmQueue.Opaque, data);
            wake.Set();
        }

        private void OnNotice(NoticeKind kind, string title, string message)
        {
            // Corrections of the source that was left must not be sent after the switch.
            if (kind == NoticeKind.SwitchedToFallback || kind == NoticeKind.SwitchedToMain)
                queue?.Clear();

            Action<NoticeKind, string, string> handler = Notice;
            if (handler != null) handler(kind, title, message);
        }

        private void OnStatusChanged(ManagerStatus ms)
        {
            NtripStatus status = ms.Status;

            TrackOutage(ms);

            // One notice per kind of problem, not one per attempt.
            if (status.State == NtripState.Streaming) { lastNotified = null; return; }
            if (status.LastError == null || status.LastError == lastNotified) return;
            if (status.State != NtripState.Waiting && status.State != NtripState.GaveUp) return;

            // Trouble that may pass (no network, caster down or busy, no data) is reported by the one-minute
            // alert. Only what the user has to fix is shown at once.
            if (status.State != NtripState.GaveUp
                && status.LastError != NtripCategory.LoginRejected
                && status.LastError != NtripCategory.UnknownMountpoint
                && status.LastError != NtripCategory.NotUnderstood) return;

            // With a fallback the switching notice tells the story; a second popup for the same failure would only confuse.
            if (ms.FallbackConfigured && ms.Active == SourceRole.Main) return;

            lastNotified = status.LastError;
            Action<string, string> handler = Problem;
            if (handler != null) handler(Title(status.LastError.Value), status.Message);
        }

        private void ResetAlert(bool outage)
        {
            lock (alertSync)
            {
                alertRole = SourceRole.Main;
                alertDismissals = 0;
                downSince = outage ? DateTime.UtcNow : (DateTime?)null;
                alerted = false;
                alertOpen = false;
                retriedSinceAlert = false;
            }
        }

        /// <summary>
        /// Called by the form when the alert is gone. <paramref name="byUser"/> is true when the user clicked it
        /// away; after two of those in one outage the alert stays away, the user knows.
        /// </summary>
        public void AlertClosed(bool byUser)
        {
            lock (alertSync)
            {
                alertOpen = false;
                if (byUser) alertDismissals++;
            }
        }

        // Keeps track of when the corrections stopped and whether a new attempt started since the last alert.
        private void TrackOutage(ManagerStatus ms)
        {
            NtripState state = ms.Status.State;
            bool cleared = false;
            bool recovered = false;
            lock (alertSync)
            {
                // A switch between main and fallback ends the outage of the source that was left: the switching
                // notice tells that story, and the new source gets its own minute before it is called a problem.
                bool switched = alertRole != ms.Active;
                alertRole = ms.Active;

                if (state == NtripState.Streaming || switched)
                {
                    cleared = alerted;
                    // Back online after the user was told about the outage; a switch has its own notice.
                    recovered = alerted && state == NtripState.Streaming && !switched;
                    downSince = state == NtripState.Streaming ? (DateTime?)null : DateTime.UtcNow;
                    alerted = false;
                    alertDismissals = 0;
                    alertOpen = false;
                    retriedSinceAlert = false;
                }
                else
                {
                    if (downSince == null) downSince = DateTime.UtcNow;
                    if (alerted && state == NtripState.Connecting) retriedSinceAlert = true;
                }
            }
            if (cleared)
            {
                Action handler = AlertCleared;
                if (handler != null) handler();
            }
            if (recovered)
            {
                Action<string, string> back = Recovered;
                if (back != null) back("NTRIP is back online", "Corrections are coming in again.");
            }
        }

        // Runs once a second: tells the user when the corrections have been missing for a minute, and again
        // after every retry that fails, so a long outage cannot go unnoticed behind another window.
        private void CheckAlert()
        {
            if (!Properties.Settings.Default.setNTRIP_showErrors) return;   // Advanced settings: NTRIP error messages off
            ManagerStatus ms = Manager;
            if (ms == null) return;
            NtripStatus status = ms.Status;

            // The fallback delivers while the main service is still the active one: the switching notice
            // (automatic switch, or the question in manual mode) is already on its way. Only when the
            // fallback fails too, or on the fallback itself, is there nothing else to tell the user.
            if (ms.Active == SourceRole.Main && ms.Fallback != null && ms.Fallback.State == NtripState.Streaming) return;
            // Waiting for the user to fix the login or the mountpoint: that was shown already.
            if (status.State == NtripState.GaveUp || status.State == NtripState.Stopped) return;

            double secondsDown;
            lock (alertSync)
            {
                if (downSince == null || alertOpen || alertDismissals >= MaxDismissals) return;
                TimeSpan down = DateTime.UtcNow - downSince.Value;
                // No network, or main and fallback both failing: nothing can fix itself soon, so say it earlier.
                bool bothFailing = ms.Active == SourceRole.Main && ms.Fallback != null;
                bool hard = status.LastError == NtripCategory.NoNetwork || bothFailing;
                if (down < (hard ? AlertAfterHard : AlertAfter)) return;
                // After the first alert, wait for a retry that failed again.
                if (alerted && !(retriedSinceAlert && status.State == NtripState.Waiting)) return;

                alerted = true;
                alertOpen = true;
                retriedSinceAlert = false;
                secondsDown = down.TotalSeconds;
            }

            int seconds = (int)secondsDown;
            string title = Title(status.LastError ?? NtripCategory.CasterUnreachable);
            string message = status.Message;
            // With both services down the user should hear about both.
            if (ms.Active == SourceRole.Main && ms.Fallback != null)
                message += "\r\nThe fallback does not work either: " + ms.Fallback.Headline + ".";
            string howLong = seconds < 90 ? seconds + " seconds" : (seconds / 60) + " minutes";
            message += "\r\n\r\nNo corrections for " + howLong + "."
                + " AgIO keeps trying.";
            Action<string, string> handler = Alert;
            if (handler != null) handler(title, message);
        }

        private static string Title(NtripCategory c)
        {
            switch (c)
            {
                case NtripCategory.LoginRejected: return "NTRIP login rejected";
                case NtripCategory.UnknownMountpoint: return "NTRIP mountpoint not found";
                case NtripCategory.CasterBusy: return "NTRIP caster busy";
                case NtripCategory.NoCorrections: return "NTRIP no corrections";
                case NtripCategory.NoNetwork: return "No network";
                case NtripCategory.StreamLost: return "NTRIP stream lost";
                default: return "NTRIP not connected";
            }
        }

        // The one thread that writes corrections out, so a full serial buffer can only delay corrections.
        private void PumpLoop()
        {
            RtcmQueue q = queue;
            while (!pumpStop)
            {
                byte[] chunk;
                if (q.TryDequeue(out chunk))
                {
                    if (toSerial) mf.WriteGpsPortQuiet(chunk);
                    if (toUdp) mf.SendNtripDatagram(chunk);
                    Interlocked.Add(ref bytesOut, chunk.Length);
                    Interlocked.Increment(ref datagramsOut);
                    continue;
                }

                int wait = q.Count > 0 ? (int)Math.Ceiling(q.MsUntilDue()) : 100;
                wake.WaitOne(Math.Max(1, Math.Min(100, wait)));
            }
        }
    }
}

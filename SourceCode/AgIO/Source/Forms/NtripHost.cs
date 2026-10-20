using System;
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

        /// <summary>A problem the user should hear about once: title and plain message. Raised on a worker thread.</summary>
        public event Action<string, string> Problem;

        /// <summary>A notice from the switching between main and fallback. Raised on a worker thread.</summary>
        public event Action<NoticeKind, string, string> Notice;

        public NtripHost(FormLoop form)
        {
            mf = form;
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

        /// <summary>The main service.</summary>
        public NtripProfile Profile
        {
            get { return mainProfile; }
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

        /// <summary>Bytes waiting in the paced queue.</summary>
        public int QueuedBytes
        {
            get { RtcmQueue q = queue; return q == null ? 0 : q.Count; }
        }

        public long BytesOut
        {
            get { return Interlocked.Read(ref bytesOut); }
        }

        public void SwitchToFallback()
        {
            CorrectionSourceManager m = manager;
            if (m != null) m.SwitchToFallbackNow();
        }

        public void SwitchBackToMain()
        {
            CorrectionSourceManager m = manager;
            if (m != null) m.SwitchBackToMain();
        }

        public void TryMainAgain()
        {
            CorrectionSourceManager m = manager;
            if (m != null) m.TryMainAgain();
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
        /// <summary>Starts (or restarts) with the current settings.</summary>
        public void Start()
        {
            Stop();

            var s = Properties.Settings.Default;
            NtripProfileSet set = NtripProfiles.LoadActive();
            NtripProfile main = MainProfile(set);
            if (main == null) return;
            mainProfile = main;

            manualPosition = s.setNTRIP_isGGAManual;
            manualLat = s.setNTRIP_manualLat;
            manualLon = s.setNTRIP_manualLon;
            manualAlt = s.setNTRIP_manualAltitude;
            toSerial = s.setNTRIP_sendToSerial;
            toUdp = s.setNTRIP_sendToUDP;

            // Serial ports take about 5 KB/s in the old metering (256 bytes per 50 ms); UDP alone can go faster.
            queue = new RtcmQueue(null, PacketSize, toSerial ? 50 : RtcmQueue.IntervalMs);
            mf.SetNtripEndpoint(s.setNTRIP_sendToUDPPort);

            manager = new CorrectionSourceManager(main, BuildFallback(set, main), BuildGga);
            manager.FrameReceived += OnFrame;
            manager.RawDataReceived += OnRaw;
            manager.StatusChanged += OnStatusChanged;
            manager.Notice += OnNotice;
            lastNotified = null;

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
            Action<NoticeKind, string, string> handler = Notice;
            if (handler != null) handler(kind, title, message);
        }

        private void OnStatusChanged(ManagerStatus ms)
        {
            NtripStatus status = ms.Status;

            // One notice per kind of problem, not one per attempt.
            if (status.State == NtripState.Streaming) { lastNotified = null; return; }
            if (status.LastError == null || status.LastError == lastNotified) return;
            if (status.State != NtripState.Waiting && status.State != NtripState.GaveUp) return;

            // With a fallback the switching notice tells the story; a second popup for the same failure would only confuse.
            if (ms.FallbackConfigured && ms.Active == SourceRole.Main) return;

            lastNotified = status.LastError;
            Action<string, string> handler = Problem;
            if (handler != null) handler(Title(status.LastError.Value), status.Message);
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

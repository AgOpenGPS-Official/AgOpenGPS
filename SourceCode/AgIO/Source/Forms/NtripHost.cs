using System;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// Runs the NTRIP client for AgIO and carries its corrections to the outputs (the GPS serial
    /// port and UDP). Everything here works off the UI thread: the client's receive loop hands whole
    /// RTCM messages to a paced queue, and one pump thread writes them out. The form only reads
    /// <see cref="Status"/> and a few counters, and feeds in the position once a second.
    /// </summary>
    internal sealed class NtripHost : IDisposable
    {
        private sealed class Position
        {
            public double Latitude, Longitude, Altitude, Age, Hdop;
            public int FixQuality, Satellites;
        }

        private readonly FormLoop mf;
        private readonly object sync = new object();

        private NtripClient client;
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

        public NtripHost(FormLoop form)
        {
            mf = form;
        }

        public bool IsRunning
        {
            get { lock (sync) return run != null && !run.IsCompleted; }
        }

        public NtripStatus Status
        {
            get
            {
                NtripClient c = client;
                return c == null ? new NtripStatus() : c.Status;
            }
        }

        public NtripProfile Profile
        {
            get { NtripClient c = client; return c == null ? null : c.Profile; }
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

        /// <summary>Starts (or restarts) with the current settings.</summary>
        public void Start()
        {
            Stop();

            var s = Properties.Settings.Default;
            string host = !string.IsNullOrWhiteSpace(s.setNTRIP_casterURL) ? s.setNTRIP_casterURL : s.setNTRIP_casterIP;
            if (string.IsNullOrWhiteSpace(host)) return;

            manualPosition = s.setNTRIP_isGGAManual;
            manualLat = s.setNTRIP_manualLat;
            manualLon = s.setNTRIP_manualLon;
            manualAlt = s.setNTRIP_manualAltitude;
            toSerial = s.setNTRIP_sendToSerial;
            toUdp = s.setNTRIP_sendToUDP;

            // "HTTP 1.0" in the old window means the NTRIP 1.0 request; 1.1 means try 2.0 first.
            NtripVersion version = s.setNTRIP_isHTTP10 ? NtripVersion.V1 : NtripVersion.Auto;
            var profile = new NtripProfile(host, s.setNTRIP_casterPort, s.setNTRIP_mount, s.setNTRIP_userName,
                s.setNTRIP_userPassword, version, s.setNTRIP_sendGGAInterval, s.setNTRIP_isTCP);

            // Serial ports take ~5 KB/s in the old metering (256 bytes per 50 ms); UDP alone can go faster.
            queue = new RtcmQueue(null, s.setNTRIP_packetSize, toSerial ? 50 : RtcmQueue.IntervalMs);
            mf.SetNtripEndpoint(s.setNTRIP_sendToUDPPort);

            client = new NtripClient(profile, BuildGga);
            client.FrameReceived += OnFrame;
            client.RawDataReceived += OnRaw;
            client.StatusChanged += OnStatusChanged;
            lastNotified = null;

            pumpStop = false;
            pump = new Thread(PumpLoop) { IsBackground = true, Name = "NTRIP output" };
            pump.Start();

            cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;
            NtripClient started = client;
            lock (sync) run = Task.Run(() => started.RunAsync(token));
        }

        public void Stop()
        {
            CancellationTokenSource c = cts;
            Task r;
            lock (sync) r = run;
            cts = null;

            if (c != null) c.Cancel();
            try { if (r != null) r.Wait(2000); } catch (Exception) { /* cancelled */ }

            pumpStop = true;
            wake.Set();
            Thread p = pump;
            if (p != null) p.Join(1000);
            pump = null;

            NtripClient old = client;
            if (old != null)
            {
                old.FrameReceived -= OnFrame;
                old.RawDataReceived -= OnRaw;
                old.StatusChanged -= OnStatusChanged;
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

        private void OnStatusChanged(NtripStatus status)
        {
            // One notice per kind of problem, not one per attempt.
            if (status.State == NtripState.Streaming) { lastNotified = null; return; }
            if (status.LastError == null || status.LastError == lastNotified) return;
            if (status.State != NtripState.Waiting && status.State != NtripState.GaveUp) return;

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

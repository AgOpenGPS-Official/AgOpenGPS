using System;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Ntrip;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// Runs the AgIO NTRIP client and pumps its RTCM corrections to the device, paced like AgIO's
    /// serial output (256 bytes per 50 ms, about 5 KB/s, which a Bluetooth SPP link at 115200 baud
    /// carries comfortably). The queue drops superseded epochs if the link falls behind, so the
    /// receiver always gets the newest corrections.
    /// </summary>
    public sealed class CorrectionForwarder
    {
        public const int SerialChunkSize = 256;
        public const double SerialIntervalMs = 50;

        private readonly NtripClient _client;
        private readonly RtcmQueue _queue;
        private readonly Func<byte[], bool> _write;
        private readonly AutoResetEvent _wake = new AutoResetEvent(false);
        private long _bytesSent;

        public event Action<NtripStatus> StatusChanged;

        /// <param name="write">Sends one chunk to the device; returns false if the link is down.</param>
        /// <param name="ggaProvider">Current GGA for the caster, or null while there is no position.</param>
        public CorrectionForwarder(NtripProfile profile, Func<byte[], bool> write, Func<string> ggaProvider,
            int chunkSize = SerialChunkSize, double intervalMs = SerialIntervalMs, NtripTimeouts timeouts = null)
        {
            _write = write ?? throw new ArgumentNullException(nameof(write));
            _client = new NtripClient(profile, ggaProvider, timeouts);
            _queue = new RtcmQueue(null, chunkSize, intervalMs);

            _client.FrameReceived += (type, frame) => { _queue.Enqueue(type, frame); _wake.Set(); };
            _client.RawDataReceived += data => { _queue.Enqueue(RtcmQueue.Opaque, data); _wake.Set(); };
            _client.StatusChanged += s => StatusChanged?.Invoke(s);
        }

        public NtripStatus Status => _client.Status;

        public long BytesSent => Interlocked.Read(ref _bytesSent);

        public int Queued => _queue.Count;

        /// <summary>Runs until cancelled (or until the caster keeps rejecting the login).</summary>
        public async Task RunAsync(CancellationToken ct)
        {
            Task pump = Task.Factory.StartNew(() => PumpLoop(ct), ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
            try
            {
                await _client.RunAsync(ct).ConfigureAwait(false);
            }
            finally
            {
                _wake.Set();
                try { await pump.ConfigureAwait(false); } catch (OperationCanceledException) { }
                _queue.Clear();
            }
        }

        private void PumpLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                if (_queue.TryDequeue(out byte[] chunk))
                {
                    if (_write(chunk)) Interlocked.Add(ref _bytesSent, chunk.Length);
                    continue;
                }

                int wait = _queue.Count > 0 ? (int)Math.Ceiling(_queue.MsUntilDue()) : 100;
                _wake.WaitOne(Math.Max(1, Math.Min(100, wait)));
            }
        }
    }
}

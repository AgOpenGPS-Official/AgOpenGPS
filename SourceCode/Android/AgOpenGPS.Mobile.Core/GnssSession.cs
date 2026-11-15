using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Ntrip;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// One connection to the GNSS device (Bluetooth SPP socket, USB serial, TCP - anything that is a
    /// pair of streams). Reads and decodes on a background thread, keeps the latest fix, and lets
    /// corrections be written back. Platform code only has to open the streams.
    /// </summary>
    public sealed class GnssSession
    {
        private readonly Stream _input;
        private readonly Stream _output;
        private readonly Func<DateTime> _utcNow;
        private readonly GnssStreamDecoder _decoder = new GnssStreamDecoder();
        private readonly NmeaAccumulator _nmea = new NmeaAccumulator();
        private readonly object _writeLock = new object();
        private readonly object _stateLock = new object();
        private readonly Queue<DateTime> _fixTimes = new Queue<DateTime>();

        private GnssFix _latest;
        private long _bytesIn, _bytesOut, _sentences, _badSentences, _pgnFrames;
        private DateTime _lastDataUtc;

        /// <summary>Raised on the reader thread for every completed fix.</summary>
        public event Action<GnssFix> FixUpdated;

        /// <summary>Raised on the reader thread for every NMEA line (valid or not), for a raw log.</summary>
        public event Action<string> SentenceReceived;

        public GnssSession(Stream input, Stream output, Func<DateTime> utcNow = null)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _output = output;
            _utcNow = utcNow ?? (() => DateTime.UtcNow);
            _decoder.SentenceReceived += OnSentence;
            _decoder.PgnReceived += OnPgn;
        }

        /// <summary>Reads until the stream ends, fails, or <paramref name="ct"/> is cancelled.
        /// Cancelling cannot interrupt a blocking Bluetooth read: close the socket as well.</summary>
        public Task RunAsync(CancellationToken ct)
        {
            return Task.Factory.StartNew(() => ReadLoop(ct), ct, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        private void ReadLoop(CancellationToken ct)
        {
            var buffer = new byte[2048];
            while (!ct.IsCancellationRequested)
            {
                int n;
                try
                {
                    n = _input.Read(buffer, 0, buffer.Length);
                }
                catch (Exception) when (ct.IsCancellationRequested)
                {
                    return;
                }

                if (n <= 0) return; // the other side closed the link
                Interlocked.Add(ref _bytesIn, n);
                lock (_stateLock) _lastDataUtc = _utcNow();
                _decoder.Feed(buffer, 0, n);
            }
        }

        private void OnSentence(string line)
        {
            SentenceReceived?.Invoke(line);
            if (!Nmea.ChecksumOk(line))
            {
                Interlocked.Increment(ref _badSentences);
                return;
            }
            Interlocked.Increment(ref _sentences);
            Publish(_nmea.Process(line, _utcNow()));
        }

        private void OnPgn(byte[] frame)
        {
            Interlocked.Increment(ref _pgnFrames);
            Publish(Pgn.ParseGpsPosition(frame, _utcNow()));
        }

        private void Publish(GnssFix fix)
        {
            if (fix == null) return;
            lock (_stateLock)
            {
                _latest = fix;
                _fixTimes.Enqueue(fix.ReceivedUtc);
                while (_fixTimes.Count > 0 && (fix.ReceivedUtc - _fixTimes.Peek()).TotalSeconds > 3) _fixTimes.Dequeue();
            }
            FixUpdated?.Invoke(fix);
        }

        /// <summary>Writes bytes to the device (RTCM corrections). Thread safe. Returns false if the link failed.</summary>
        public bool Write(byte[] data, int offset, int count)
        {
            if (_output == null || count <= 0) return false;
            try
            {
                lock (_writeLock)
                {
                    _output.Write(data, offset, count);
                    _output.Flush();
                }
                Interlocked.Add(ref _bytesOut, count);
                return true;
            }
            catch (IOException) { return false; }
            catch (ObjectDisposedException) { return false; }
        }

        /// <summary>A GGA sentence with CR/LF for the NTRIP caster: the receiver's own when it sends GGA,
        /// else one built from the latest fix. Null while there is no recent fix.</summary>
        public string GgaForCaster()
        {
            GnssFix fix;
            lock (_stateLock) fix = _latest;
            if (fix == null || !fix.HasPosition || (_utcNow() - fix.ReceivedUtc).TotalSeconds > 5) return null;

            // The NTRIP client writes this as is, and casters wait for the line end.
            string gga = fix.Source == FixSource.Nmea ? _nmea.LastGga : null;
            if (gga != null) return gga + "\r\n";

            return GgaBuilder.FromReceiver(_utcNow(), fix.Latitude, fix.Longitude, fix.FixQuality,
                Math.Max(0, fix.Satellites), double.IsNaN(fix.Hdop) ? 1.0 : fix.Hdop,
                double.IsNaN(fix.Altitude) ? 0 : fix.Altitude, double.IsNaN(fix.AgeSeconds) ? 0 : fix.AgeSeconds);
        }

        public SessionStats GetStats()
        {
            lock (_stateLock)
            {
                DateTime now = _utcNow();
                double rate = 0;
                if (_fixTimes.Count >= 2)
                {
                    // Over the stored window; a link that went quiet shows 0 via SecondsSinceFix instead.
                    double span = (_latest.ReceivedUtc - _fixTimes.Peek()).TotalSeconds;
                    if (span > 0.2) rate = (_fixTimes.Count - 1) / span;
                }

                return new SessionStats
                {
                    Latest = _latest?.Clone(),
                    BytesIn = Interlocked.Read(ref _bytesIn),
                    BytesOut = Interlocked.Read(ref _bytesOut),
                    Sentences = Interlocked.Read(ref _sentences),
                    BadSentences = Interlocked.Read(ref _badSentences),
                    PgnFrames = Interlocked.Read(ref _pgnFrames),
                    SkippedBytes = _decoder.SkippedBytes,
                    RateHz = rate,
                    SecondsSinceData = _lastDataUtc == default ? -1 : (now - _lastDataUtc).TotalSeconds,
                    SecondsSinceFix = _latest == null ? -1 : (now - _latest.ReceivedUtc).TotalSeconds,
                };
            }
        }
    }

    public sealed class SessionStats
    {
        public GnssFix Latest;
        public long BytesIn, BytesOut, Sentences, BadSentences, PgnFrames, SkippedBytes;
        /// <summary>Fixes per second over the last 3 s.</summary>
        public double RateHz;
        public double SecondsSinceData;
        public double SecondsSinceFix;
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgLibrary.Ntrip
{
    /// <summary>One RTCM message type as seen on the caster's stream.</summary>
    public sealed class RtcmTypeStat
    {
        public int Type { get; }
        public long Count { get; }
        public long Bytes { get; }
        public double SecondsSinceLast { get; }
        /// <summary>Average spacing, or NaN after a single message.</summary>
        public double MeanIntervalSeconds { get; }

        public RtcmTypeStat(int type, long count, long bytes, double secondsSinceLast, double meanIntervalSeconds)
        {
            Type = type;
            Count = count;
            Bytes = bytes;
            SecondsSinceLast = secondsSinceLast;
            MeanIntervalSeconds = meanIntervalSeconds;
        }
    }

    /// <summary>What the caster has sent this session, by message type.</summary>
    public sealed class RtcmStreamSnapshot
    {
        public long Messages { get; }
        public long ChecksumFailures { get; }
        public long BytesSkipped { get; }
        public double SessionSeconds { get; }
        public IReadOnlyList<RtcmTypeStat> Types { get; }
        /// <summary>The reference station ID from the latest message that carries one, or -1.</summary>
        public int StationId { get; }

        public RtcmStreamSnapshot(long messages, long checksumFailures, long bytesSkipped,
            double sessionSeconds, IReadOnlyList<RtcmTypeStat> types, int stationId = -1)
        {
            Messages = messages;
            ChecksumFailures = checksumFailures;
            BytesSkipped = bytesSkipped;
            SessionSeconds = sessionSeconds;
            Types = types;
            StationId = stationId;
        }

        /// <summary>Observation messages (legacy or MSM) have arrived.</summary>
        public bool HasObservations
        {
            get
            {
                foreach (RtcmTypeStat t in Types)
                    if (RtcmMessages.IsObservation(t.Type)) return true;
                return false;
            }
        }

        /// <summary>The satellite systems in the observations, e.g. "GPS + GLONASS + Galileo + BeiDou"; empty when none.</summary>
        public string Constellations
        {
            get
            {
                bool[] seen = new bool[RtcmMessages.SystemNames.Length];
                foreach (RtcmTypeStat t in Types)
                {
                    int s = RtcmMessages.SystemIndex(t.Type);
                    if (s >= 0) seen[s] = true;
                }
                var names = new List<string>();
                for (int i = 0; i < seen.Length; i++)
                    if (seen[i]) names.Add(RtcmMessages.SystemNames[i]);
                return string.Join(" + ", names);
            }
        }

        /// <summary>The highest MSM level seen (4 or 7 are common), or 0 without MSM observations.</summary>
        public int MsmLevel
        {
            get
            {
                int level = 0;
                foreach (RtcmTypeStat t in Types)
                    if (RtcmMessages.IsMsm(t.Type)) level = Math.Max(level, t.Type % 10);
                return level;
            }
        }

        /// <summary>The base position (1005 or 1006) has arrived. Without it there is no RTK solution.</summary>
        public bool HasStationPosition
        {
            get
            {
                foreach (RtcmTypeStat t in Types)
                    if (t.Type == 1005 || t.Type == 1006) return true;
                return false;
            }
        }
    }

    /// <summary>
    /// Frames the caster's stream, counts messages by type and hands each whole message to the
    /// forwarder. Thread-safe: the receive loop feeds it, the UI reads snapshots.
    /// Replaces the old byte-0 parsing of the RTCM list (finding N16).
    /// Ported from AgOpenWeb (RtcmStreamStats).
    /// </summary>
    internal sealed class RtcmStreamStats
    {
        private sealed class Entry { public long Count, Bytes; public double First, Last; }

        private readonly Func<double> _seconds;
        private readonly object _lock = new object();
        private readonly Rtcm3Framer _framer = new Rtcm3Framer();
        private readonly SortedDictionary<int, Entry> _types = new SortedDictionary<int, Entry>();
        private double _sessionStart;
        private int _stationId = -1;

        /// <param name="secondsClock">Monotonic time in seconds; null uses a Stopwatch.</param>
        public RtcmStreamStats(Func<double> secondsClock = null)
        {
            _seconds = secondsClock ?? StopwatchSeconds;
            _sessionStart = _seconds();
        }

        private static double StopwatchSeconds() => Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;

        /// <summary>Start a new session: forget the previous counts and any partial message.</summary>
        public void Reset()
        {
            lock (_lock)
            {
                _framer.Reset();
                _types.Clear();
                _stationId = -1;
                _sessionStart = _seconds();
            }
        }

        /// <param name="onMessage">Called for each whole message, in stream order.</param>
        public void Feed(ReadOnlySpan<byte> data, Rtcm3Framer.MessageHandler onMessage = null)
        {
            lock (_lock)
            {
                double now = _seconds();
                _framer.Feed(data, (type, message) =>
                {
                    if (!_types.TryGetValue(type, out Entry e))
                        _types[type] = e = new Entry { First = now };
                    e.Count++;
                    e.Bytes += message.Length;
                    e.Last = now;
                    if (RtcmMessages.HasStationId(type) && message.Length >= 9)
                        _stationId = (message[4] & 0x0F) << 8 | message[5];
                    onMessage?.Invoke(type, message);
                });
            }
        }

        public RtcmStreamSnapshot Snapshot()
        {
            lock (_lock)
            {
                double now = _seconds();
                var types = new List<RtcmTypeStat>(_types.Count);
                foreach (KeyValuePair<int, Entry> kv in _types)
                {
                    Entry e = kv.Value;
                    double mean = e.Count > 1 ? (e.Last - e.First) / (e.Count - 1) : double.NaN;
                    types.Add(new RtcmTypeStat(kv.Key, e.Count, e.Bytes, now - e.Last, mean));
                }
                return new RtcmStreamSnapshot(_framer.Messages, _framer.ChecksumFailures,
                    _framer.BytesSkipped, now - _sessionStart, types, _stationId);
            }
        }
    }
}

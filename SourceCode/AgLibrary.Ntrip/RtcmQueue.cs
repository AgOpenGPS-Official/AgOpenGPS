using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgLibrary.Ntrip
{
    /// <summary>Pacing numbers since the previous call, for the NTRIP health line.</summary>
    public struct RtcmPacingStats
    {
        public long Datagrams;
        public long CatchUp;
        public double MaxLateMs;
    }

    /// <summary>
    /// The RTCM waiting to go to the GPS module, held as whole messages and released in small,
    /// spaced UDP datagrams (AgIO's metering: 256 bytes per tick; AgOpenWeb #169, an unpaced
    /// backlog stalled the AiO board). Replaces AgIO's byte queue (finding N17).
    ///
    /// Nothing is ever dropped except as a whole message, and a message that has started going
    /// out always finishes. When the caster's stream arrives faster than it can be sent (after an
    /// internet pause TCP hands over everything at once), the backlog is thinned by what the
    /// receiver can still use:
    ///  - Observations (legacy and MSM): a message replaces the unsent ones of its own type
    ///    from an earlier epoch.
    ///  - Station data (base position, antenna, receiver, GLONASS biases): a message replaces
    ///    the unsent one of its type.
    ///  - Ephemerides: a message replaces an unsent byte-for-byte copy of itself.
    ///  - Everything else is kept in order.
    /// A hard cap on the queue is a memory guard only; it drops the oldest unsent messages.
    ///
    /// Pure logic, thread-safe; time comes from a injectable clock so tests do not sleep.
    /// Ported from AgOpenWeb (RtcmQueue).
    /// </summary>
    internal sealed class RtcmQueue
    {
        /// <summary>Largest datagram sent. The AiO firmwares cannot take a 2233 datagram over 512 bytes.</summary>
        public const int ChunkSize = 256;

        /// <summary>Nominal gap between datagrams: 256 B / 25 ms is about 10 KB/s.</summary>
        public const double IntervalMs = 25.0;

        /// <summary>Shortest gap, used only to catch up after the sender was woken late.</summary>
        public const double MinGapMs = 10.0;

        /// <summary>Most lateness that is made up: ten datagrams' worth.</summary>
        public const double MaxCreditMs = 250.0;

        /// <summary>Memory guard: past this the oldest unsent messages are dropped.</summary>
        public const int MaxQueuedBytes = 64 * 1024;

        /// <summary>Message type for bytes that are not framed RTCM 3: queued and sent in order, never thinned.</summary>
        public const int Opaque = -2;

        private enum Kind { Keep, Observation, Latest, Repeat }

        private sealed class Item
        {
            public byte[] Data;
            public Kind Kind;
            public long Key;
            public int Offset;
        }

        private readonly Func<double> _ms;
        private readonly object _lock = new object();
        private readonly LinkedList<Item> _items = new LinkedList<Item>();
        private int _count;
        private double _lastSendMs;
        private bool _hasSent;
        // Time the sender spent late (data was waiting and due) is credit, spent by
        // shortening the following gaps down to MinGapMs.
        private double _creditMs;
        private double _nonEmptySinceMs;
        private long _datagrams, _catchUpDatagrams;
        private double _maxLateMs;

        /// <param name="millisecondsClock">Monotonic time in ms; null uses a Stopwatch.</param>
        public RtcmQueue(Func<double> millisecondsClock = null)
        {
            _ms = millisecondsClock ?? StopwatchMs;
        }

        private static double StopwatchMs() => Stopwatch.GetTimestamp() * 1000.0 / Stopwatch.Frequency;

        /// <summary>Bytes waiting to be sent.</summary>
        public int Count { get { lock (_lock) return _count; } }

        /// <summary>Observation messages replaced by a newer epoch before they were sent.</summary>
        public long SupersededObservations { get; private set; }
        /// <summary>Station messages replaced by a newer one, and repeated ephemerides, before they were sent.</summary>
        public long SupersededOther { get; private set; }
        /// <summary>Messages dropped by the memory guard.</summary>
        public long MemoryGuardDrops { get; private set; }

        public RtcmPacingStats TakePacingStats()
        {
            lock (_lock)
            {
                var stats = new RtcmPacingStats { Datagrams = _datagrams, CatchUp = _catchUpDatagrams, MaxLateMs = _maxLateMs };
                _datagrams = _catchUpDatagrams = 0;
                _maxLateMs = 0;
                return stats;
            }
        }

        /// <summary>Queue one whole message (or, with <see cref="Opaque"/>, one read of an
        /// unframed stream). The bytes are copied.</summary>
        public void Enqueue(int messageType, ReadOnlySpan<byte> message)
        {
            if (message.Length == 0) return;
            Kind kind = Classify(messageType, message, out long key);
            lock (_lock)
            {
                if (kind != Kind.Keep)
                {
                    for (LinkedListNode<Item> node = _items.First; node != null;)
                    {
                        LinkedListNode<Item> next = node.Next;
                        Item it = node.Value;
                        bool superseded = it.Offset == 0 && it.Kind == kind && IsSuperseded(it, kind, key, message);
                        if (superseded)
                        {
                            _items.Remove(node);
                            _count -= it.Data.Length;
                            if (kind == Kind.Observation) SupersededObservations++; else SupersededOther++;
                        }
                        node = next;
                    }
                }

                if (_count == 0) _nonEmptySinceMs = _ms();
                _items.AddLast(new Item { Data = message.ToArray(), Kind = kind, Key = key });
                _count += message.Length;

                // Memory guard. The message in flight (Offset > 0) is never dropped.
                for (LinkedListNode<Item> node = _items.First; _count > MaxQueuedBytes && node != null && node != _items.Last;)
                {
                    LinkedListNode<Item> next = node.Next;
                    if (node.Value.Offset == 0)
                    {
                        _count -= node.Value.Data.Length;
                        _items.Remove(node);
                        MemoryGuardDrops++;
                    }
                    node = next;
                }
            }
        }

        private static bool IsSuperseded(Item it, Kind kind, long key, ReadOnlySpan<byte> message)
        {
            switch (kind)
            {
                case Kind.Observation: return (it.Key >> 32) == (key >> 32) && it.Key != key; // same type, another epoch
                case Kind.Latest: return it.Key == key;                                      // same type
                default: return it.Key == key && message.SequenceEqual(it.Data);             // the same bytes
            }
        }

        /// <summary>Take the next datagram if one is due: the queue holds data and the gap since
        /// the last one has passed (<see cref="IntervalMs"/>, or less while catching up).</summary>
        public bool TryDequeue(out byte[] chunk)
        {
            chunk = Array.Empty<byte>();
            lock (_lock)
            {
                double now = _ms();
                if (_count == 0 || !IsDue(now)) return false;

                if (_hasSent)
                {
                    // How long this datagram sat ready: from the end of its gap, or from when
                    // it arrived if the queue was empty then. An idle queue earns no credit.
                    double gap = Gap;
                    double sinceLast = now - _lastSendMs;
                    double late = Math.Max(0, Math.Min(sinceLast - gap, now - _nonEmptySinceMs));
                    _creditMs = Clamp(_creditMs - (IntervalMs - gap) + late, 0, MaxCreditMs);
                    if (gap < IntervalMs - 0.5) _catchUpDatagrams++;
                    if (late > _maxLateMs) _maxLateMs = late;
                }
                _datagrams++;

                int size = Math.Min(_count, ChunkSize);
                chunk = new byte[size];
                int filled = 0;
                while (filled < size)
                {
                    Item it = _items.First.Value;
                    int take = Math.Min(size - filled, it.Data.Length - it.Offset);
                    Buffer.BlockCopy(it.Data, it.Offset, chunk, filled, take);
                    it.Offset += take;
                    filled += take;
                    if (it.Offset == it.Data.Length) _items.RemoveFirst();
                }
                _count -= size;
                _lastSendMs = now;
                _hasSent = true;
                return true;
            }
        }

        /// <summary>Milliseconds until the next datagram may go (0 when one is due now).</summary>
        public double MsUntilDue()
        {
            lock (_lock)
            {
                if (!_hasSent) return 0;
                double wait = Gap - (_ms() - _lastSendMs);
                return wait > 0 ? wait : 0;
            }
        }

        /// <summary>Drop everything queued (session teardown). The counters are kept.</summary>
        public void Clear()
        {
            lock (_lock)
            {
                _items.Clear();
                _count = 0;
                _creditMs = 0;
            }
        }

        // The gap the next datagram must keep: shorter while there is lateness to make up.
        private double Gap => Math.Max(MinGapMs, IntervalMs - _creditMs);

        private bool IsDue(double now) => !_hasSent || now - _lastSendMs >= Gap;

        private static double Clamp(double v, double lo, double hi) => v < lo ? lo : (v > hi ? hi : v);

        // What a message may replace. Observations: (type, epoch time). Station data: its type.
        // Ephemerides: an identical message (the key only narrows the comparison).
        private static Kind Classify(int type, ReadOnlySpan<byte> message, out long key)
        {
            key = 0;
            if (type < 0 || message.Length < Rtcm3Framer.Overhead) return Kind.Keep;
            ReadOnlySpan<byte> payload = message.Slice(3, message.Length - 6);

            if (RtcmMessages.IsObservation(type))
            {
                // After the 12-bit message number and 12-bit station id comes the epoch time:
                // 27 bits in GLONASS legacy messages, 30 bits in all the others.
                int bits = type >= 1009 && type <= 1012 ? 27 : 30;
                if (payload.Length * 8 < 24 + bits) return Kind.Keep;
                key = ((long)type << 32) | Bits(payload, 24, bits);
                return Kind.Observation;
            }

            switch (type)
            {
                case 1005:
                case 1006:
                case 1007:
                case 1008:
                case 1013:
                case 1033:
                case 1230:
                    key = (long)type << 32;
                    return Kind.Latest;
                case 1019:
                case 1020:
                case 1041:
                case 1042:
                case 1044:
                case 1045:
                case 1046:
                    key = ((long)type << 32) | (uint)message.Length;
                    return Kind.Repeat;
                default:
                    return Kind.Keep;
            }
        }

        private static uint Bits(ReadOnlySpan<byte> data, int start, int count)
        {
            uint value = 0;
            for (int i = start; i < start + count; i++)
                value = (value << 1) | (uint)((data[i >> 3] >> (7 - (i & 7))) & 1);
            return value;
        }
    }
}

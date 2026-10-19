using System;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Finds RTCM 3 messages in a byte stream that arrives in arbitrary slices (TCP reads from an
    /// NTRIP caster). A message is the preamble 0xD3, six reserved zero bits, a 10-bit payload
    /// length, the payload, and a CRC-24Q over everything before it: 6 to 1029 bytes.
    ///
    /// Bytes that are not part of a message with a valid checksum are skipped and counted; after
    /// a failed checksum the search resumes one byte on, so a 0xD3 inside garbage cannot hide the
    /// real message behind it. Pure logic, not thread-safe: the caller serialises
    /// <see cref="Feed"/>. Ported from AgOpenWeb (RtcmFramer), finding N16.
    /// </summary>
    internal sealed class Rtcm3Framer
    {
        public const byte Preamble = 0xD3;
        public const int MaxPayload = 1023;
        public const int Overhead = 6;                       // 3 header + 3 checksum
        public const int MaxMessage = MaxPayload + Overhead;

        /// <summary>Called for each complete message: its type (the payload's first 12 bits; -1
        /// for a payload under two bytes) and the whole message, header and checksum included.
        /// The span is only valid during the call.</summary>
        public delegate void MessageHandler(int messageType, ReadOnlySpan<byte> message);

        private readonly byte[] _buf = new byte[MaxMessage * 4];
        private int _start, _end;

        /// <summary>Messages with a valid checksum.</summary>
        public long Messages { get; private set; }
        /// <summary>Candidate messages (preamble and reserved bits right) whose checksum failed.</summary>
        public long ChecksumFailures { get; private set; }
        /// <summary>Bytes passed over that were not part of a valid message.</summary>
        public long BytesSkipped { get; private set; }

        /// <summary>Bytes held while waiting for the rest of a message.</summary>
        public int Pending => _end - _start;

        public void Reset()
        {
            _start = _end = 0;
            Messages = ChecksumFailures = BytesSkipped = 0;
        }

        public void Feed(ReadOnlySpan<byte> data, MessageHandler onMessage = null)
        {
            while (data.Length > 0)
            {
                if (_start > 0 && _end + data.Length > _buf.Length)
                {
                    Buffer.BlockCopy(_buf, _start, _buf, 0, _end - _start);
                    _end -= _start;
                    _start = 0;
                }
                // Scan() never leaves a whole message's worth unprocessed, so there is room.
                int take = Math.Min(data.Length, _buf.Length - _end);
                data.Slice(0, take).CopyTo(new Span<byte>(_buf, _end, take));
                _end += take;
                data = data.Slice(take);
                Scan(onMessage);
            }
        }

        private void Scan(MessageHandler onMessage)
        {
            while (true)
            {
                int at = Array.IndexOf(_buf, Preamble, _start, _end - _start);
                if (at < 0)
                {
                    BytesSkipped += _end - _start;
                    _start = _end = 0;
                    return;
                }
                BytesSkipped += at - _start;
                _start = at;

                if (_end - _start < 3) return;                       // header not complete
                if ((_buf[_start + 1] & 0xFC) != 0) { Skip(); continue; }

                int payload = ((_buf[_start + 1] & 0x03) << 8) | _buf[_start + 2];
                int total = payload + Overhead;
                if (_end - _start < total) return;                   // message not complete

                uint crc = Crc24Q(new ReadOnlySpan<byte>(_buf, _start, total - 3));
                uint sent = (uint)((_buf[_start + total - 3] << 16) | (_buf[_start + total - 2] << 8) | _buf[_start + total - 1]);
                if (crc != sent)
                {
                    ChecksumFailures++;
                    Skip();
                    continue;
                }

                Messages++;
                int type = payload >= 2 ? (_buf[_start + 3] << 4) | (_buf[_start + 4] >> 4) : -1;
                onMessage?.Invoke(type, new ReadOnlySpan<byte>(_buf, _start, total));
                _start += total;
            }
        }

        private void Skip()
        {
            BytesSkipped++;
            _start++;
        }

        private static readonly uint[] CrcTable = BuildCrcTable();

        private static uint[] BuildCrcTable()
        {
            var table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint crc = i << 16;
                for (int bit = 0; bit < 8; bit++)
                {
                    crc <<= 1;
                    if ((crc & 0x1000000) != 0) crc ^= 0x1864CFB;
                }
                table[i] = crc & 0xFFFFFF;
            }
            return table;
        }

        /// <summary>CRC-24Q (polynomial 0x1864CFB, initial value 0), as RTCM 10403 specifies.</summary>
        public static uint Crc24Q(ReadOnlySpan<byte> data)
        {
            uint crc = 0;
            foreach (byte b in data)
                crc = ((crc << 8) & 0xFFFFFF) ^ CrcTable[(crc >> 16) ^ b];
            return crc;
        }
    }
}

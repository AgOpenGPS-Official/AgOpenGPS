using System;
using System.Text;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// Splits the raw byte stream from the Bluetooth link into NMEA sentences and AgOpenGPS PGN
    /// frames, in any mix, across any chunk boundaries. Everything else (UBX, echoed RTCM, noise)
    /// is skipped and counted. This is what lets one app work with a receiver that sends NMEA
    /// and with a Raspberry Pi that forwards PGN packets, without a setting.
    /// </summary>
    public sealed class GnssStreamDecoder
    {
        private const int MaxSentence = 256;

        private enum State { Idle, Nmea, PgnHeader2, PgnBody }

        private State _state = State.Idle;
        private readonly StringBuilder _line = new StringBuilder(MaxSentence);
        private byte[] _pgn = new byte[5 + 255 + 1];
        private int _pgnPos;
        private int _pgnNeed;

        /// <summary>A complete line that starts with '$' (not yet checksum-checked), without CR/LF.</summary>
        public event Action<string> SentenceReceived;

        /// <summary>A complete PGN frame with a valid CRC. The array is a fresh copy.</summary>
        public event Action<byte[]> PgnReceived;

        public long SkippedBytes { get; private set; }
        public long BadPgnFrames { get; private set; }

        public void Feed(byte[] buffer, int offset, int count)
        {
            for (int i = offset; i < offset + count; i++) Feed(buffer[i]);
        }

        public void Feed(byte b)
        {
            switch (_state)
            {
                case State.Idle:
                    Start(b);
                    break;

                case State.Nmea:
                    if (b == '\n' || b == '\r')
                    {
                        if (_line.Length > 1) SentenceReceived?.Invoke(_line.ToString());
                        _line.Clear();
                        _state = State.Idle;
                    }
                    else if (b == '$' || b == Pgn.Header1)
                    {
                        // A new message started before this line ended: drop the broken line.
                        SkippedBytes += _line.Length;
                        _line.Clear();
                        Start(b);
                    }
                    else if (b < 0x20 || b > 0x7E || _line.Length >= MaxSentence)
                    {
                        SkippedBytes += _line.Length + 1;
                        _line.Clear();
                        _state = State.Idle;
                    }
                    else
                    {
                        _line.Append((char)b);
                    }
                    break;

                case State.PgnHeader2:
                    if (b == Pgn.Header2)
                    {
                        _pgn[0] = Pgn.Header1;
                        _pgn[1] = Pgn.Header2;
                        _pgnPos = 2;
                        _pgnNeed = 5; // up to and including the length byte
                        _state = State.PgnBody;
                    }
                    else
                    {
                        SkippedBytes++;
                        _state = State.Idle;
                        Start(b);
                    }
                    break;

                case State.PgnBody:
                    _pgn[_pgnPos++] = b;
                    if (_pgnPos == 5) _pgnNeed = 5 + b + 1;
                    if (_pgnPos == _pgnNeed)
                    {
                        _state = State.Idle;
                        var frame = new byte[_pgnNeed];
                        Array.Copy(_pgn, frame, _pgnNeed);
                        if (frame[_pgnNeed - 1] == Pgn.Crc(frame, _pgnNeed - 1))
                        {
                            PgnReceived?.Invoke(frame);
                        }
                        else
                        {
                            BadPgnFrames++;
                        }
                    }
                    break;
            }
        }

        private void Start(byte b)
        {
            if (b == '$')
            {
                _line.Clear();
                _line.Append('$');
                _state = State.Nmea;
            }
            else if (b == Pgn.Header1)
            {
                _state = State.PgnHeader2;
            }
            else
            {
                if (b != '\r' && b != '\n') SkippedBytes++;
                _state = State.Idle;
            }
        }
    }
}

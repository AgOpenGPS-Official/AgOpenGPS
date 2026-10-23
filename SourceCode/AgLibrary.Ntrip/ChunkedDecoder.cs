using System;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Removes HTTP/1.1 chunked transfer encoding from a body that arrives in arbitrary slices:
    /// each chunk is a hexadecimal size line (extensions after ';' ignored), CRLF, that many
    /// bytes, CRLF. An NTRIP 2 caster may answer this way; left in place, the size lines land
    /// inside RTCM messages and break them. Pure logic, not thread-safe.
    /// Ported from AgOpenWeb (ChunkedDecoder), finding N4.
    /// </summary>
    internal sealed class ChunkedDecoder
    {
        private enum State { Size, SizeExtension, SizeLf, Data, DataCr, DataLf, Done }

        public delegate void DataHandler(ReadOnlySpan<byte> data);

        private State _state = State.Size;
        private long _remaining;
        private bool _sizeSeen;

        /// <summary>The final zero-size chunk has arrived: the body is over.</summary>
        public bool IsDone => _state == State.Done;

        /// <summary>The stream did not follow the chunk format. Bytes after that point are
        /// passed through untouched, so a caster that announces chunking and does not do it
        /// still gets its data forwarded.</summary>
        public bool IsBroken { get; private set; }

        public void Feed(ReadOnlySpan<byte> data, DataHandler onData)
        {
            int i = 0;
            while (i < data.Length)
            {
                if (IsBroken) { onData(data.Slice(i)); return; }

                byte b = data[i];
                switch (_state)
                {
                    case State.Size:
                        int digit = HexValue(b);
                        if (digit >= 0 && _remaining <= int.MaxValue) { _remaining = _remaining * 16 + digit; _sizeSeen = true; i++; }
                        else if (_sizeSeen && b == ';') { _state = State.SizeExtension; i++; }
                        else if (_sizeSeen && b == '\r') { _state = State.SizeLf; i++; }
                        else if (_sizeSeen && b == ' ') i++;   // some servers pad the size
                        else IsBroken = true;
                        break;
                    case State.SizeExtension:
                        if (b == '\r') _state = State.SizeLf;
                        i++;
                        break;
                    case State.SizeLf:
                        if (b != '\n') { IsBroken = true; break; }
                        _state = _remaining == 0 ? State.Done : State.Data;
                        i++;
                        break;
                    case State.Data:
                        int take = (int)Math.Min(_remaining, data.Length - i);
                        onData(data.Slice(i, take));
                        i += take;
                        _remaining -= take;
                        if (_remaining == 0) _state = State.DataCr;
                        break;
                    case State.DataCr:
                        if (b != '\r') { IsBroken = true; break; }
                        _state = State.DataLf;
                        i++;
                        break;
                    case State.DataLf:
                        if (b != '\n') { IsBroken = true; break; }
                        _state = State.Size;
                        _sizeSeen = false;
                        i++;
                        break;
                    case State.Done:
                        return;   // trailers, if any, are not data
                }
            }
        }

        private static int HexValue(byte b)
        {
            if (b >= (byte)'0' && b <= (byte)'9') return b - '0';
            if (b >= (byte)'a' && b <= (byte)'f') return b - 'a' + 10;
            if (b >= (byte)'A' && b <= (byte)'F') return b - 'A' + 10;
            return -1;
        }
    }
}

using System;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Finds the end of a caster's reply header in the bytes read so far. Normally that is the
    /// blank line. RTKLIB's caster (RTKBase) answers "ICY 200 OK" followed by one line break and
    /// then the RTCM stream, with no blank line, also to an NTRIP 2.0 request (recorded
    /// 19 October 2026). A parser that waits for a blank line waits until its timeout.
    /// Pure logic, no I/O.
    /// </summary>
    internal static class NtripHeaderScanner
    {
        /// <summary>
        /// True when the header is complete. <paramref name="headerLength"/> is the length of the header text
        /// without its closing line break(s); <paramref name="bodyOffset"/> is where the body starts.
        /// </summary>
        public static bool TryFindEnd(ReadOnlySpan<byte> data, out int headerLength, out int bodyOffset)
        {
            headerLength = bodyOffset = -1;

            int firstEnd = LineEnd(data, 0, out int firstNext);
            if (firstEnd >= 0 && StartsWith(data, "ICY ") && firstNext < data.Length && !LooksLikeHeaderText(data[firstNext]))
            {
                // "ICY 200 OK\r\n" and the next byte is not text, a line break or a header name: the stream began.
                headerLength = firstEnd;
                bodyOffset = firstNext;
                return true;
            }

            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] != (byte)'\n') continue;
                // A blank line is "\n" followed by "\n" or "\r\n".
                if (i + 1 < data.Length && data[i + 1] == (byte)'\n')
                {
                    headerLength = i;
                    bodyOffset = i + 2;
                    return true;
                }
                if (i + 2 < data.Length && data[i + 1] == (byte)'\r' && data[i + 2] == (byte)'\n')
                {
                    headerLength = i > 0 && data[i - 1] == (byte)'\r' ? i - 1 : i;
                    bodyOffset = i + 3;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// The data is exactly an "ICY ..." status line with its line break and nothing after it.
        /// RTKLIB sends that line and then waits for the next epoch, so after a short pause the
        /// caller may treat it as the complete header.
        /// </summary>
        public static bool IsIcyStatusLineOnly(ReadOnlySpan<byte> data)
        {
            if (!StartsWith(data, "ICY ")) return false;
            int end = LineEnd(data, 0, out int next);
            return end >= 0 && next == data.Length;
        }

        // Index of the line's last text byte + 1 (the start of its line break), or -1 without a line break.
        private static int LineEnd(ReadOnlySpan<byte> data, int start, out int next)
        {
            for (int i = start; i < data.Length; i++)
            {
                if (data[i] == (byte)'\n')
                {
                    next = i + 1;
                    return i > start && data[i - 1] == (byte)'\r' ? i - 1 : i;
                }
            }
            next = -1;
            return -1;
        }

        // A header line starts with a letter; a blank line starts with \r or \n.
        private static bool LooksLikeHeaderText(byte b) =>
            b == (byte)'\r' || b == (byte)'\n' || (b >= 0x20 && b < 0x7F);

        private static bool StartsWith(ReadOnlySpan<byte> data, string text)
        {
            if (data.Length < text.Length) return false;
            for (int i = 0; i < text.Length; i++)
                if (data[i] != (byte)text[i]) return false;
            return true;
        }
    }
}

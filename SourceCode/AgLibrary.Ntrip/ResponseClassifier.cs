using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AgLibrary.Ntrip
{
    /// <summary>The nine outcomes of an NTRIP attempt (see the rewrite plan, Errors).</summary>
    public enum NtripCategory
    {
        /// <summary>The caster accepted the request and will stream corrections.</summary>
        Accepted,
        NoNetwork,
        CasterUnreachable,
        LoginRejected,
        UnknownMountpoint,
        CasterBusy,
        NoCorrections,
        StreamLost,
        NotUnderstood,
        TlsFailed,
    }

    /// <summary>Result of classifying a caster's answer to the mountpoint request.</summary>
    public sealed class NtripOutcome
    {
        public NtripCategory Category { get; }
        /// <summary>Plain message for the user. Empty when accepted.</summary>
        public string Message { get; }
        /// <summary>The caster's first response line, for the log only.</summary>
        public string FirstLine { get; }
        /// <summary>The body is HTTP chunked and must go through ChunkedDecoder.</summary>
        public bool Chunked { get; }

        public NtripOutcome(NtripCategory category, string message, string firstLine, bool chunked)
        {
            Category = category;
            Message = message;
            FirstLine = firstLine;
            Chunked = chunked;
        }

        public bool IsAccepted => Category == NtripCategory.Accepted;
    }

    /// <summary>
    /// Pure function from status line + headers + first body bytes to an outcome. No I/O.
    /// Tolerant by design: casters answer errors inconsistently (#319), so anything that is
    /// not recognised becomes NotUnderstood and is logged in full, to be added to the table.
    /// Ported from AgOpenWeb (NtripResponse) and extended for N5, N6 and N15.
    /// </summary>
    public static class ResponseClassifier
    {
        private static readonly Regex HttpStatus =
            new Regex(@"^HTTP/\d(\.\d)?\s+(\d{3})", RegexOptions.CultureInvariant);

        private static readonly string[] BusyWords =
            { "too many", "already", "in use", "banned", "blocked", "max", "busy", "limit", "try again" };
        private static readonly string[] LoginWords =
            { "password", "unauthor", "login", "credential", "user" };
        private static readonly string[] MountWords =
            { "mount", "not found", "no such" };

        /// <param name="header">Everything up to the blank line, as text.</param>
        /// <param name="firstBody">The first bytes after the header, if any arrived.</param>
        public static NtripOutcome Classify(string header, ReadOnlySpan<byte> firstBody = default)
        {
            header = header ?? "";
            string[] lines = header.Replace("\r", "").Split('\n');
            string first = lines[0].Trim();
            bool chunked = IsChunked(lines);

            if (first.Length == 0)
                return Out(NtripCategory.NotUnderstood, "The service sent an empty answer.", first, chunked);

            if (first.StartsWith("SOURCETABLE", StringComparison.OrdinalIgnoreCase))
                return Out(NtripCategory.UnknownMountpoint, "This mountpoint does not exist on the service. Pick one from the list.", first, chunked);

            if (first.StartsWith("ICY 200", StringComparison.Ordinal))
                return BodyLooksLikeSourcetable(firstBody)
                    ? Out(NtripCategory.UnknownMountpoint, "This mountpoint does not exist on the service. Pick one from the list.", first, chunked)
                    : Accepted(first, chunked);

            if (first.StartsWith("ERROR", StringComparison.OrdinalIgnoreCase))
                return ClassifyText(first, first, chunked);

            Match m = HttpStatus.Match(first);
            if (!m.Success)
                return ClassifyText(first, first, chunked, NtripCategory.NotUnderstood);

            string code = m.Groups[2].Value;
            switch (code)
            {
                case "200":
                    if (ContentType(lines).IndexOf("sourcetable", StringComparison.OrdinalIgnoreCase) >= 0
                        || BodyLooksLikeSourcetable(firstBody))
                        return Out(NtripCategory.UnknownMountpoint, "This mountpoint does not exist on the service. Pick one from the list.", first, chunked);
                    if (firstBody.Length > 0 && firstBody[0] == (byte)'<')
                        return Out(NtripCategory.NotUnderstood, "This address answered with a web page, not an NTRIP service. Check the address and port.", first, chunked);
                    return Accepted(first, chunked);
                case "401":
                    // Some casters (AgOpenNtripCaster) also answer 401 for an unknown mountpoint.
                    return Out(NtripCategory.LoginRejected, "User name, password or mountpoint is not correct.", first, chunked);
                case "403":
                    return ClassifyText(first + " " + BodyText(firstBody), first, chunked, NtripCategory.LoginRejected);
                case "404":
                    return Out(NtripCategory.UnknownMountpoint, "This mountpoint does not exist on the service. Check the spelling, capitals matter.", first, chunked);
                case "429":
                case "502":
                case "503":
                case "504":
                    return Out(NtripCategory.CasterBusy, "The service cannot take another connection right now. It is busy, or this login is already in use.", first, chunked);
                case "406":
                    return ClassifyText(first + " " + BodyText(firstBody), first, chunked, NtripCategory.NotUnderstood);
                case "400":
                    // RTK2go answers 400 with a text asking for an e-mail address as user name.
                    return ClassifyText(first + " " + BodyText(firstBody), first, chunked, NtripCategory.NotUnderstood);
                default:
                    return ClassifyText(first + " " + BodyText(firstBody), first, chunked, NtripCategory.NotUnderstood);
            }
        }

        /// <summary>The header announces Transfer-Encoding: chunked.</summary>
        public static bool IsChunked(string header)
        {
            return IsChunked((header ?? "").Replace("\r", "").Split('\n'));
        }

        private static bool IsChunked(string[] lines)
        {
            return HeaderValue(lines, "Transfer-Encoding").IndexOf("chunked", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string ContentType(string[] lines) => HeaderValue(lines, "Content-Type");

        private static string HeaderValue(string[] lines, string name)
        {
            for (int i = 1; i < lines.Length; i++)
            {
                int colon = lines[i].IndexOf(':');
                if (colon > 0 && lines[i].Substring(0, colon).Trim().Equals(name, StringComparison.OrdinalIgnoreCase))
                    return lines[i].Substring(colon + 1).Trim();
            }
            return "";
        }

        private static bool BodyLooksLikeSourcetable(ReadOnlySpan<byte> body)
        {
            if (body.Length < 4) return false;
            return StartsWith(body, "STR;") || StartsWith(body, "CAS;") || StartsWith(body, "NET;")
                || StartsWith(body, "SOURCETABLE");
        }

        private static bool StartsWith(ReadOnlySpan<byte> body, string text)
        {
            if (body.Length < text.Length) return false;
            for (int i = 0; i < text.Length; i++)
                if (body[i] != (byte)text[i]) return false;
            return true;
        }

        private static string BodyText(ReadOnlySpan<byte> body)
        {
            int n = Math.Min(body.Length, 256);
            var chars = new char[n];
            for (int i = 0; i < n; i++) chars[i] = body[i] >= 32 && body[i] < 127 ? (char)body[i] : ' ';
            return new string(chars);
        }

        /// <summary>Match free text against the table of known caster messages.</summary>
        private static NtripOutcome ClassifyText(string text, string first, bool chunked,
            NtripCategory fallback = NtripCategory.NotUnderstood)
        {
            string t = text.ToLowerInvariant();
            // SNIP answers 406 for a minute or so after a restart, while the base stations reconnect.
            if (t.IndexOf("start up", StringComparison.Ordinal) >= 0 || t.IndexOf("starting up", StringComparison.Ordinal) >= 0)
                return Out(NtripCategory.CasterBusy, "The service is starting up. AgIO tries again in about a minute.", first, chunked);
            if (ContainsAny(t, BusyWords))
                return Out(NtripCategory.CasterBusy, "The service cannot take another connection right now. It is busy, or this login is already in use.", first, chunked);
            if (t.IndexOf("email", StringComparison.Ordinal) >= 0 || t.IndexOf("e-mail", StringComparison.Ordinal) >= 0)
                return Out(NtripCategory.LoginRejected, "This service needs your e-mail address as user name.", first, chunked);
            if (ContainsAny(t, LoginWords))
                return Out(NtripCategory.LoginRejected, "User name or password is not correct.", first, chunked);
            if (ContainsAny(t, MountWords))
                return Out(NtripCategory.UnknownMountpoint, "This mountpoint does not exist on the service. Check the spelling, capitals matter.", first, chunked);
            if (fallback == NtripCategory.LoginRejected)
                return Out(NtripCategory.LoginRejected, "The service denied access.", first, chunked);
            return Out(NtripCategory.NotUnderstood, "The service answered something AgIO does not understand.", first, chunked);
        }

        private static bool ContainsAny(string text, IEnumerable<string> words)
        {
            foreach (string w in words)
                if (text.IndexOf(w, StringComparison.Ordinal) >= 0) return true;
            return false;
        }

        private static NtripOutcome Accepted(string first, bool chunked) =>
            new NtripOutcome(NtripCategory.Accepted, "", first, chunked);

        private static NtripOutcome Out(NtripCategory c, string message, string first, bool chunked) =>
            new NtripOutcome(c, message, first, chunked);
    }
}

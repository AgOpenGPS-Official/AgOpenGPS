using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>One mountpoint (one STR line) of a caster's sourcetable.</summary>
    public sealed class MountpointInfo
    {
        public string Mount { get; set; } = "";
        /// <summary>The place or name the caster gives it.</summary>
        public string Name { get; set; } = "";
        public string Format { get; set; } = "";
        /// <summary>The satellite systems, e.g. "GPS+GLO+GAL".</summary>
        public string Systems { get; set; } = "";
        public string Network { get; set; } = "";
        public string Country { get; set; } = "";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        /// <summary>The caster wants the receiver's position (nearest base, virtual station).</summary>
        public bool NeedsPosition { get; set; }
        public string Authentication { get; set; } = "";

        public bool HasPosition => Latitude != 0 || Longitude != 0;

        /// <summary>Great-circle distance in kilometres, or NaN when the mountpoint has no position.</summary>
        public double DistanceKm(double latitude, double longitude)
        {
            if (!HasPosition) return double.NaN;
            const double R = 6371.0;
            double dLat = (Latitude - latitude) * Math.PI / 180.0;
            double dLon = (Longitude - longitude) * Math.PI / 180.0;
            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                + Math.Cos(latitude * Math.PI / 180.0) * Math.Cos(Latitude * Math.PI / 180.0) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * R * Math.Asin(Math.Min(1.0, Math.Sqrt(a)));
        }
    }

    public sealed class SourcetableResult
    {
        public IReadOnlyList<MountpointInfo> Mountpoints { get; }
        /// <summary>Null when the table was read.</summary>
        public NtripOutcome Error { get; }
        public bool Success => Error == null;

        public SourcetableResult(IReadOnlyList<MountpointInfo> mountpoints, NtripOutcome error)
        {
            Mountpoints = mountpoints ?? new MountpointInfo[0];
            Error = error;
        }
    }

    /// <summary>Reads a caster's list of mountpoints. Replaces the blocking socket code of the old settings window.</summary>
    public static class Sourcetable
    {
        private const int MaxBytes = 8 * 1024 * 1024;

        /// <summary>The STR lines of a sourcetable text (CAS and NET lines are skipped).</summary>
        public static List<MountpointInfo> Parse(string text)
        {
            var list = new List<MountpointInfo>();
            foreach (string raw in (text ?? "").Replace("\r", "").Split('\n'))
            {
                if (!raw.StartsWith("STR;", StringComparison.Ordinal)) continue;
                string[] f = raw.Split(';');
                if (f.Length < 3 || f[1].Length == 0) continue;

                var m = new MountpointInfo
                {
                    Mount = f[1],
                    Name = Field(f, 2),
                    Format = Field(f, 3),
                    Systems = Field(f, 6),
                    Network = Field(f, 7),
                    Country = Field(f, 8),
                    Latitude = Number(Field(f, 9)),
                    Longitude = Number(Field(f, 10)),
                    NeedsPosition = Field(f, 11) == "1",
                    Authentication = Field(f, 15),
                };
                list.Add(m);
            }
            return list;
        }

        private static string Field(string[] f, int i) => i < f.Length ? f[i].Trim() : "";

        private static double Number(string s)
        {
            double v;
            return double.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out v) ? v : 0;
        }

        /// <summary>Asks the caster for its list. Tries NTRIP 2.0 first and 1.0 when the caster does not understand it.</summary>
        public static async Task<SourcetableResult> FetchAsync(NtripProfile profile, NtripTimeouts timeouts, CancellationToken ct)
        {
            timeouts = timeouts ?? new NtripTimeouts();
            SourcetableResult result = null;
            foreach (NtripVersion version in new[] { NtripVersion.V2, NtripVersion.V1 })
            {
                result = await TryFetchAsync(profile, version, timeouts, ct).ConfigureAwait(false);
                if (result.Success || result.Error.Category != NtripCategory.NotUnderstood) break;
            }
            return result;
        }

        private static async Task<SourcetableResult> TryFetchAsync(NtripProfile p, NtripVersion version, NtripTimeouts t, CancellationToken ct)
        {
            using (var client = new TcpClient(AddressFamily.InterNetwork) { NoDelay = true })
            {
                try
                {
                    Task connect = client.ConnectAsync(p.Host, p.Port);
                    Task first = await Task.WhenAny(connect, Task.Delay(t.Connect, ct)).ConfigureAwait(false);
                    ct.ThrowIfCancellationRequested();
                    if (first != connect)
                        return Fail(ConnectionErrors.Classify(SocketError.TimedOut, p.Host, p.Port));
                    await connect.ConfigureAwait(false);

                    // an empty mountpoint is the request for the sourcetable
                    var ask = new NtripProfile(p.Host, p.Port, "", p.User, p.Password, version);
                    byte[] request = Encoding.ASCII.GetBytes(NtripRequest.Build(ask, version, null));
                    System.IO.Stream stream = client.GetStream();
                    await stream.WriteAsync(request, 0, request.Length).ConfigureAwait(false);

                    var reader = new TimedReader(stream);
                    var header = new List<byte>();
                    var clock = System.Diagnostics.Stopwatch.StartNew();
                    byte[] all;
                    int headerLength, bodyOffset;
                    while (true)
                    {
                        TimeSpan remaining = t.Header - clock.Elapsed;
                        if (remaining <= TimeSpan.Zero)
                            return Fail(new NtripOutcome(NtripCategory.CasterUnreachable, "No answer from " + p.Host + ":" + p.Port + ".", "", false));
                        int n = await reader.ReadAsync(remaining, ct).ConfigureAwait(false);
                        if (n < 0) continue;
                        if (n == 0)
                            return Fail(new NtripOutcome(NtripCategory.CasterUnreachable, p.Host + ":" + p.Port + " closed the connection without answering.", "", false));
                        for (int i = 0; i < n; i++) header.Add(reader.Buffer[i]);
                        all = header.ToArray();
                        if (NtripHeaderScanner.TryFindEnd(all, out headerLength, out bodyOffset)) break;
                        if (header.Count > 16384)
                            return Fail(new NtripOutcome(NtripCategory.NotUnderstood, "The service sent an answer AgIO cannot read.", "", false));
                    }

                    string headerText = Encoding.ASCII.GetString(all, 0, headerLength);
                    var body = new byte[all.Length - bodyOffset];
                    Array.Copy(all, bodyOffset, body, 0, body.Length);

                    NtripOutcome outcome = ResponseClassifier.Classify(headerText, body);
                    if (!ResponseClassifier.IsSourcetable(headerText, body))
                    {
                        // a 401 or an unknown answer: tell the user what the caster said
                        return Fail(outcome.IsAccepted
                            ? new NtripOutcome(NtripCategory.NotUnderstood, "The service did not send a list of mountpoints.", outcome.FirstLine, false)
                            : outcome);
                    }

                    var text = new StringBuilder();
                    var decoder = outcome.Chunked ? new ChunkedDecoder() : null;
                    int total = 0;
                    AppendText(text, body, body.Length, decoder);
                    total += body.Length;
                    while (!text.ToString().Contains("ENDSOURCETABLE") && total < MaxBytes)
                    {
                        int n = await reader.ReadAsync(TimeSpan.FromSeconds(10), ct).ConfigureAwait(false);
                        if (n <= 0) break;
                        AppendText(text, reader.Buffer, n, decoder);
                        total += n;
                    }
                    return new SourcetableResult(Parse(text.ToString()), null);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (SocketException ex)
                {
                    return Fail(ConnectionErrors.Classify(ex.SocketErrorCode, p.Host, p.Port));
                }
                catch (Exception ex) when (ex is System.IO.IOException || ex is ObjectDisposedException || ex is InvalidOperationException)
                {
                    return Fail(new NtripOutcome(NtripCategory.CasterUnreachable, p.Host + ":" + p.Port + " closed the connection.", ex.GetType().Name, false));
                }
            }
        }

        private static void AppendText(StringBuilder text, byte[] data, int count, ChunkedDecoder decoder)
        {
            if (decoder != null && !decoder.IsBroken)
            {
                var plain = new List<byte>();
                decoder.Feed(new ReadOnlySpan<byte>(data, 0, count), d => plain.AddRange(d.ToArray()));
                text.Append(Encoding.UTF8.GetString(plain.ToArray()));
            }
            else
            {
                text.Append(Encoding.UTF8.GetString(data, 0, count));
            }
        }

        private static SourcetableResult Fail(NtripOutcome outcome) => new SourcetableResult(null, outcome);
    }
}

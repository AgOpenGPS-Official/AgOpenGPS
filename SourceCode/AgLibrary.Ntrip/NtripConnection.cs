using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// One attempt: resolve, connect, send the request, read the reply header and classify it.
    /// Never throws for a network problem: the outcome says what happened. When the caster
    /// accepts, the connection stays open and the caller reads the body through <see cref="Reader"/>.
    /// Only cancellation is raised as an exception.
    /// </summary>
    internal sealed class NtripConnection : IDisposable
    {
        private TcpClient _client;
        private NetworkStream _stream;

        public NtripOutcome Outcome { get; private set; }
        public NtripVersion VersionUsed { get; private set; }
        /// <summary>The bytes that arrived behind the header in the same reads.</summary>
        public byte[] InitialBody { get; private set; } = new byte[0];
        public TimedReader Reader { get; private set; }
        public double ConnectMs { get; private set; } = -1;
        public double HeaderMs { get; private set; } = -1;

        private NtripConnection(NtripVersion version)
        {
            VersionUsed = version;
        }

        /// <param name="version">V1 or V2.</param>
        /// <param name="gga">GGA sentence for the request header (V2), or null.</param>
        public static async Task<NtripConnection> OpenAsync(NtripProfile p, NtripVersion version, string gga,
            NtripTimeouts timeouts, CancellationToken ct)
        {
            var c = new NtripConnection(version);
            try
            {
                await c.RunAsync(p, version, gga, timeouts, ct).ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                c.Dispose();
                throw;
            }
            catch (SocketException ex)
            {
                c.Fail(ConnectionErrors.Classify(ex.SocketErrorCode, p.Host, p.Port));
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException || ex is InvalidOperationException)
            {
                c.Fail(new NtripOutcome(NtripCategory.CasterUnreachable, p.Host + ":" + p.Port + " closed the connection.", ex.GetType().Name, false));
            }
            return c;
        }

        private async Task RunAsync(NtripProfile p, NtripVersion version, string gga, NtripTimeouts t, CancellationToken ct)
        {
            var sw = Stopwatch.StartNew();
            _client = new TcpClient(AddressFamily.InterNetwork) { NoDelay = true };

            Task connect = _client.ConnectAsync(p.Host, p.Port);
            Task first = await Task.WhenAny(connect, Task.Delay(t.Connect, ct)).ConfigureAwait(false);
            ct.ThrowIfCancellationRequested();
            if (first != connect)
            {
                var ignored = connect.ContinueWith(x => x.Exception, TaskContinuationOptions.OnlyOnFaulted);
                Fail(ConnectionErrors.Classify(SocketError.TimedOut, p.Host, p.Port));
                return;
            }
            await connect.ConfigureAwait(false);
            ConnectMs = sw.Elapsed.TotalMilliseconds;

            _stream = _client.GetStream();
            Reader = new TimedReader(_stream);
            if (p.RawTcp)
            {
                // No request and no header: whatever arrives is the stream.
                HeaderMs = ConnectMs;
                Outcome = new NtripOutcome(NtripCategory.Accepted, "", "raw TCP", false);
                return;
            }
            byte[] request = Encoding.ASCII.GetBytes(NtripRequest.Build(p, version, gga));
            await _stream.WriteAsync(request, 0, request.Length).ConfigureAwait(false);

            // Read until the header is complete (N7: across as many reads as it takes).
            var header = new List<byte>();
            var headerClock = Stopwatch.StartNew();
            while (true)
            {
                TimeSpan remaining = t.Header - headerClock.Elapsed;
                if (remaining <= TimeSpan.Zero)
                {
                    Fail(new NtripOutcome(NtripCategory.CasterUnreachable, "No answer from " + p.Host + ":" + p.Port + ".", "", false));
                    return;
                }
                bool icyOnly = NtripHeaderScanner.IsIcyStatusLineOnly(header.ToArray());
                TimeSpan wait = icyOnly && t.IcyGrace < remaining ? t.IcyGrace : remaining;

                int n = await Reader.ReadAsync(wait, ct).ConfigureAwait(false);
                if (n < 0)
                {
                    if (icyOnly) { Complete(header.ToArray(), header.Count, header.Count, sw); return; }
                    continue;
                }
                if (n == 0)
                {
                    // Closed before the header was complete: judge what there is.
                    if (header.Count == 0)
                        Fail(new NtripOutcome(NtripCategory.CasterUnreachable, p.Host + ":" + p.Port + " closed the connection without answering.", "", false));
                    else
                        Complete(header.ToArray(), header.Count, header.Count, sw);
                    return;
                }
                for (int i = 0; i < n; i++) header.Add(Reader.Buffer[i]);
                if (header.Count > 16384)
                {
                    Fail(new NtripOutcome(NtripCategory.NotUnderstood, "The service sent an answer AgIO cannot read.", "", false));
                    return;
                }
                byte[] all = header.ToArray();
                if (NtripHeaderScanner.TryFindEnd(all, out int headerLength, out int bodyOffset))
                {
                    Complete(all, headerLength, bodyOffset, sw);
                    return;
                }
            }
        }

        private void Complete(byte[] all, int headerLength, int bodyOffset, Stopwatch sw)
        {
            HeaderMs = sw.Elapsed.TotalMilliseconds;
            string headerText = Encoding.ASCII.GetString(all, 0, headerLength);
            byte[] body = new byte[all.Length - bodyOffset];
            Array.Copy(all, bodyOffset, body, 0, body.Length);
            Outcome = ResponseClassifier.Classify(headerText, body);
            if (Outcome.IsAccepted)
                InitialBody = body;
            else
                Close();
        }

        private void Fail(NtripOutcome outcome)
        {
            Outcome = outcome;
            Close();
        }

        public Task WriteAsync(string text)
        {
            byte[] b = Encoding.ASCII.GetBytes(text);
            return _stream.WriteAsync(b, 0, b.Length);
        }

        private void Close()
        {
            Reader?.Abandon();
            try { _client?.Close(); } catch (Exception) { }
        }

        public void Dispose() => Close();
    }
}

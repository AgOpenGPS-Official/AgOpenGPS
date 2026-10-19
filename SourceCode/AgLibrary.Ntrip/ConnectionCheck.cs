using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    public sealed class ConnectionCheckResult
    {
        public bool Success { get; }
        /// <summary>One sentence for the user.</summary>
        public string Summary { get; }
        /// <summary>What happened, step by step, in plain words.</summary>
        public IReadOnlyList<string> Lines { get; }

        public ConnectionCheckResult(bool success, string summary, IReadOnlyList<string> lines)
        {
            Success = success;
            Summary = summary;
            Lines = lines;
        }
    }

    /// <summary>
    /// "Check connection" in the settings window: tries one service once, without retrying, and says in
    /// plain words what happened. Used for the main service, the fallback service and the fallback radio.
    /// </summary>
    public static class ConnectionCheck
    {
        public static Task<ConnectionCheckResult> RunAsync(NtripProfile profile, Func<string> ggaProvider, Action<string> progress,
            CancellationToken ct, NtripTimeouts timeouts = null)
        {
            return RunAsync(new NtripClient(profile, ggaProvider, timeouts), TimeSpan.FromSeconds(25), progress, ct);
        }

        /// <param name="open">Opens the serial radio; throws when that fails.</param>
        public static Task<ConnectionCheckResult> RunRadioAsync(string portName, Func<Stream> open, Action<string> progress, CancellationToken ct)
        {
            return RunAsync(new RadioSource(portName, open, null, TimeSpan.FromSeconds(5)), TimeSpan.FromSeconds(12), progress, ct);
        }

        internal static async Task<ConnectionCheckResult> RunAsync(ICorrectionSource source, TimeSpan timeout, Action<string> progress, CancellationToken ct)
        {
            var lines = new List<string>();
            var done = new TaskCompletionSource<ConnectionCheckResult>();
            var clock = Stopwatch.StartNew();
            bool announcedConnecting = false, announcedAnswer = false, announcedData = false;

            Action<string> log = text =>
            {
                lock (lines) lines.Add(text);
                if (progress != null) progress(text);
            };
            Func<IReadOnlyList<string>> snapshot = () => { lock (lines) return new List<string>(lines); };

            source.StatusChanged += st =>
            {
                switch (st.State)
                {
                    case NtripState.Connecting:
                        if (!announcedConnecting) { announcedConnecting = true; log("Connecting to " + source.Description + "..."); }
                        break;
                    case NtripState.Connected:
                        if (!announcedAnswer)
                        {
                            announcedAnswer = true;
                            log(st.FirstLine.Length > 0 ? "The service answered: " + st.FirstLine + "." : st.Message);
                        }
                        break;
                    case NtripState.Streaming:
                        if (!announcedData)
                        {
                            announcedData = true;
                            log("Corrections arrive after " + clock.Elapsed.TotalSeconds.ToString("0.0", CultureInfo.InvariantCulture) + " seconds.");
                            Task.Delay(1500).ContinueWith(t =>
                            {
                                NtripStatus now = source.Status;
                                done.TrySetResult(new ConnectionCheckResult(true, "All good: corrections are coming in.", Finish(log, now, snapshot)));
                            });
                        }
                        break;
                    case NtripState.Waiting:
                    case NtripState.GaveUp:
                        string why = st.Reason.Length > 0 ? st.Reason : st.Message;
                        log(why);
                        done.TrySetResult(new ConnectionCheckResult(false, why, snapshot()));
                        break;
                }
            };

            using (CancellationTokenSource cts = CancellationTokenSource.CreateLinkedTokenSource(ct))
            {
                Task run = Task.Run(() => source.RunAsync(cts.Token));
                ConnectionCheckResult result;
                try
                {
                    Task first = await Task.WhenAny(done.Task, Task.Delay(timeout, ct)).ConfigureAwait(false);
                    ct.ThrowIfCancellationRequested();
                    if (first == done.Task)
                    {
                        result = done.Task.Result;
                    }
                    else
                    {
                        string message = "No corrections within " + ((int)timeout.TotalSeconds).ToString(CultureInfo.InvariantCulture) + " seconds.";
                        log(message);
                        result = new ConnectionCheckResult(false, message, snapshot());
                    }
                }
                catch (OperationCanceledException)
                {
                    result = new ConnectionCheckResult(false, "Cancelled.", snapshot());
                }

                cts.Cancel();
                try { await run.ConfigureAwait(false); }
                catch (OperationCanceledException) { }
                return result;
            }
        }

        private static IReadOnlyList<string> Finish(Action<string> log, NtripStatus st, Func<IReadOnlyList<string>> snapshot)
        {
            long damaged = st.Rtcm.ChecksumFailures;
            string damage = damaged == 0 ? ", no errors" : ", " + damaged.ToString(CultureInfo.InvariantCulture) + " damaged";
            log(st.Frames.ToString(CultureInfo.InvariantCulture) + " messages received" + damage + ".");
            log("All good: corrections are coming in.");
            return snapshot();
        }
    }
}

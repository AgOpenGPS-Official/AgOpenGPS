using System.Diagnostics;
using System.Threading;

namespace AgLibrary.Logging
{
    /// <summary>
    /// Thread-safe gate that allows at most one log entry per interval.
    /// Check it on the calling (background) thread BEFORE posting work to the UI thread,
    /// so a flapping error can never queue an unbounded number of UI callbacks.
    /// </summary>
    public sealed class LogRateLimiter
    {
        private readonly long intervalTicks;
        private long nextAllowedTimestamp;

        public LogRateLimiter(double intervalSeconds)
        {
            intervalTicks = (long)(intervalSeconds * Stopwatch.Frequency);
            nextAllowedTimestamp = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// Returns true for exactly one caller per interval.
        /// </summary>
        public bool TryAcquire()
        {
            long now = Stopwatch.GetTimestamp();
            long next = Interlocked.Read(ref nextAllowedTimestamp);
            if (now < next) return false;

            //only the thread that wins the swap may log
            return Interlocked.CompareExchange(ref nextAllowedTimestamp, now + intervalTicks, next) == next;
        }
    }
}

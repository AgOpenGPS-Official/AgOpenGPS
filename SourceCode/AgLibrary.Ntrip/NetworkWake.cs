using System.Threading;
using System.Threading.Tasks;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Told by the host when the network changes (an address appears, the link comes back). A client that
    /// waits out a retry pause then tries at once instead of waiting for the end of the pause, so a dropped
    /// hotspot is back in seconds and not after minutes.
    /// </summary>
    public static class NetworkWake
    {
        private static TaskCompletionSource<bool> next = New();

        private static TaskCompletionSource<bool> New() => new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

        /// <summary>Completes at the next <see cref="Signal"/>.</summary>
        public static Task Next => Volatile.Read(ref next).Task;

        public static void Signal() => Interlocked.Exchange(ref next, New()).TrySetResult(true);
    }
}

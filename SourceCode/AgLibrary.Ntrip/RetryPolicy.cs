using System;

namespace AgLibrary.Ntrip
{
    internal struct RetryDecision
    {
        public TimeSpan Delay;
        /// <summary>Retrying will not help until the user changes something.</summary>
        public bool GiveUp;
        /// <summary>For categories that stop: the number of attempts allowed, otherwise 0.</summary>
        public int MaxAttempts;
    }

    /// <summary>
    /// How long to wait after a failed attempt, per category (the Errors table of the plan).
    /// Delays carry +-10 % jitter and never exceed 5 minutes. Pure logic.
    /// </summary>
    internal sealed class RetryPolicy
    {
        public const int StopAfterAttempts = 3;
        private static readonly int[] Unreachable = { 5, 10, 20, 40, 120 };
        private static readonly int[] Busy = { 30, 60, 120, 300 };
        private static readonly int[] NoCorrections = { 10, 20, 40, 120 };
        private const double CapSeconds = 300;

        private readonly Func<double> _random;

        /// <param name="random">Returns 0..1; null uses a shared Random.</param>
        public RetryPolicy(Func<double> random = null)
        {
            var rnd = new Random();
            _random = random ?? (() => { lock (rnd) return rnd.NextDouble(); });
        }

        /// <param name="failuresInRow">Consecutive failures of this category, counting the one just seen (1 = first).</param>
        public RetryDecision Next(NtripCategory category, int failuresInRow)
        {
            if (failuresInRow < 1) failuresInRow = 1;
            switch (category)
            {
                case NtripCategory.LoginRejected:
                case NtripCategory.UnknownMountpoint:
                    if (failuresInRow >= StopAfterAttempts)
                        return new RetryDecision { GiveUp = true, MaxAttempts = StopAfterAttempts };
                    return Wait(5, StopAfterAttempts);
                case NtripCategory.NoNetwork:
                    return Wait(5, 0);
                case NtripCategory.CasterBusy:
                    return Wait(Pick(Busy, failuresInRow), 0);
                case NtripCategory.NoCorrections:
                    return Wait(Pick(NoCorrections, failuresInRow), 0);
                case NtripCategory.StreamLost:
                    // The first loss reconnects at once; after that it behaves like an unreachable caster.
                    return failuresInRow == 1 ? new RetryDecision() : Wait(Pick(Unreachable, failuresInRow - 1), 0);
                default:   // CasterUnreachable, NotUnderstood, TlsFailed
                    return Wait(Pick(Unreachable, failuresInRow), 0);
            }
        }

        private static int Pick(int[] table, int failuresInRow) => table[Math.Min(failuresInRow, table.Length) - 1];

        private RetryDecision Wait(int seconds, int maxAttempts)
        {
            double jitter = 0.9 + 0.2 * _random();
            double s = Math.Min(CapSeconds, seconds * jitter);
            return new RetryDecision { Delay = TimeSpan.FromSeconds(s), MaxAttempts = maxAttempts };
        }
    }
}

using System;

namespace AgLibrary.Ntrip
{
    public enum NtripVersion
    {
        /// <summary>Try NTRIP 2.0 and fall back to 1.0 once when the caster does not understand it.</summary>
        Auto,
        V1,
        V2,
    }

    /// <summary>Immutable settings for one mountpoint.</summary>
    public sealed class NtripProfile
    {
        public string Host { get; }
        public int Port { get; }
        public string Mount { get; }
        public string User { get; }
        public string Password { get; }
        public NtripVersion Version { get; }
        /// <summary>Seconds between GGA sentences; 0 sends one with the request only.</summary>
        public int GgaIntervalSeconds { get; }

        public NtripProfile(string host, int port, string mount, string user = "", string password = "",
            NtripVersion version = NtripVersion.Auto, int ggaIntervalSeconds = 0)
        {
            if (string.IsNullOrWhiteSpace(host)) throw new ArgumentException("host is empty", nameof(host));
            Host = host.Trim();
            Port = port;
            Mount = (mount ?? "").Trim();
            User = user ?? "";
            Password = password ?? "";
            Version = version;
            GgaIntervalSeconds = Math.Max(0, ggaIntervalSeconds);
        }

        /// <summary>Short name for messages and the log: mount@host:port.</summary>
        public string DisplayName => Mount + "@" + Host + ":" + Port;
    }

    /// <summary>The time limits of one connection attempt. Defaults follow the recordings: every caster
    /// answered within 8 s and sent its first frame within 1.5 s.</summary>
    public sealed class NtripTimeouts
    {
        public TimeSpan Connect = TimeSpan.FromSeconds(10);
        public TimeSpan Header = TimeSpan.FromSeconds(15);
        /// <summary>No RTCM frame for this long ends the session (NoCorrections before the first frame, StreamLost after).</summary>
        public TimeSpan NoData = TimeSpan.FromSeconds(15);
        /// <summary>RTKLIB sends "ICY 200 OK" alone and then waits for the next epoch: after this pause the line counts as the whole header.</summary>
        public TimeSpan IcyGrace = TimeSpan.FromMilliseconds(250);
    }
}

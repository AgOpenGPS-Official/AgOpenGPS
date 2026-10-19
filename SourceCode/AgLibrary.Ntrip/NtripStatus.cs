namespace AgLibrary.Ntrip
{
    public enum NtripState
    {
        /// <summary>Not started, or stopped by the user.</summary>
        Stopped,
        /// <summary>Connecting and waiting for the caster's answer.</summary>
        Connecting,
        /// <summary>Accepted by the caster, no RTCM frame yet.</summary>
        Connected,
        /// <summary>Corrections are arriving.</summary>
        Streaming,
        /// <summary>Last attempt failed, waiting to try again.</summary>
        Waiting,
        /// <summary>Retrying will not help (wrong login or mountpoint); waits for the user.</summary>
        GaveUp,
    }

    /// <summary>A snapshot for the UI: one clear state, the last error in plain words, and counters.</summary>
    public sealed class NtripStatus
    {
        public NtripState State { get; internal set; }
        /// <summary>The category of the last failure; null while nothing has failed.</summary>
        public NtripCategory? LastError { get; internal set; }
        /// <summary>One plain sentence for the user, including "attempt 2 of 3" where it applies.</summary>
        public string Message { get; internal set; } = "";
        /// <summary>The caster's first response line, for the log only.</summary>
        public string FirstLine { get; internal set; } = "";
        public int FailuresInRow { get; internal set; }
        public double RetryInSeconds { get; internal set; }
        public NtripVersion VersionUsed { get; internal set; }
        public long BytesReceived { get; internal set; }
        public long Frames { get; internal set; }
        /// <summary>Seconds since the last RTCM frame; -1 before the first.</summary>
        public double SecondsSinceLastFrame { get; internal set; } = -1;
        public RtcmStreamSnapshot Rtcm { get; internal set; } = new RtcmStreamSnapshot(0, 0, 0, 0, new RtcmTypeStat[0]);
        /// <summary>The stream has no RTCM 3 frames and its bytes are forwarded as they come.</summary>
        public bool Unframed { get; internal set; }

        internal NtripStatus Clone() => (NtripStatus)MemberwiseClone();
    }
}

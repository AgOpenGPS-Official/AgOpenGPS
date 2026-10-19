using System;

namespace AgLibrary.Ntrip
{
    /// <summary>The colour of the status light next to the headline.</summary>
    public enum NtripLight
    {
        Off,
        /// <summary>Busy connecting.</summary>
        Info,
        Ok,
        /// <summary>A problem that fixes itself or is being retried.</summary>
        Warn,
        /// <summary>The user has to do something.</summary>
        Bad,
    }

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
        /// <summary>What delivers the corrections, for the headline: "NTRIP" or "Radio".</summary>
        public string SourceName { get; internal set; } = "NTRIP";
        /// <summary>Failed attempts in a row of any kind, not counting a missing network. 0 after corrections arrived.</summary>
        public int ConsecutiveFailures { get; internal set; }

        /// <summary>One short sentence for the big status line, in words a farmer uses.</summary>
        public string Headline
        {
            get
            {
                switch (State)
                {
                    case NtripState.Streaming: return SourceName + " Active";
                    case NtripState.Connected: return "Connected, waiting for data";
                    case NtripState.Connecting: return "Connecting...";
                    case NtripState.GaveUp: return LastError == NtripCategory.UnknownMountpoint ? "Mountpoint not found" : "Cannot log in";
                    case NtripState.Waiting: return WaitingHeadline();
                    default: return "Stopped";
                }
            }
        }

        private string WaitingHeadline()
        {
            switch (LastError)
            {
                case NtripCategory.LoginRejected: return "Cannot log in";
                case NtripCategory.UnknownMountpoint: return "Mountpoint not found";
                case NtripCategory.CasterBusy: return Message.IndexOf("starting up", StringComparison.Ordinal) >= 0 ? "Service is starting up" : "Service is busy";
                case NtripCategory.NoCorrections: return "Connected, nothing coming in";
                case NtripCategory.NoNetwork: return "No network";
                case NtripCategory.StreamLost: return "Reconnecting...";
                case NtripCategory.NotUnderstood: return "Service not understood";
                default: return "Cannot reach the service";
            }
        }

        /// <summary>The colour of the status light.</summary>
        public NtripLight Light
        {
            get
            {
                switch (State)
                {
                    case NtripState.Streaming: return NtripLight.Ok;
                    case NtripState.Connecting:
                    case NtripState.Connected: return NtripLight.Info;
                    case NtripState.GaveUp: return NtripLight.Bad;
                    case NtripState.Waiting:
                        return LastError == NtripCategory.LoginRejected || LastError == NtripCategory.UnknownMountpoint
                            ? NtripLight.Bad : NtripLight.Warn;
                    default: return NtripLight.Off;
                }
            }
        }

        internal NtripStatus Clone() => (NtripStatus)MemberwiseClone();
    }
}

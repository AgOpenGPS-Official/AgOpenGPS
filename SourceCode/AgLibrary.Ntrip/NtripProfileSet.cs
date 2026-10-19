namespace AgLibrary.Ntrip
{
    public enum FallbackType
    {
        /// <summary>Another NTRIP service.</summary>
        NtripService,
        /// <summary>A serial radio.</summary>
        SerialRadio,
    }

    /// <summary>The settings of one NTRIP service: where it is, the mountpoint and the login. Saved in a profile file.</summary>
    public sealed class ServiceDefinition
    {
        public string Host { get; set; } = "";
        public int Port { get; set; } = 2101;
        public string Mount { get; set; } = "";
        public string User { get; set; } = "";
        public string Password { get; set; } = "";
        public NtripVersion Version { get; set; } = NtripVersion.Auto;
        /// <summary>Read the stream as it comes, without an NTRIP request.</summary>
        public bool RawTcp { get; set; }

        public bool IsComplete => !string.IsNullOrWhiteSpace(Host);

        /// <summary>The connection settings, or null while there is no address.</summary>
        public NtripProfile ToProfile(int ggaIntervalSeconds)
        {
            if (!IsComplete) return null;
            return new NtripProfile(Host, Port, Mount, User, Password, Version, ggaIntervalSeconds, RawTcp);
        }

        public ServiceDefinition Clone() => (ServiceDefinition)MemberwiseClone();
    }

    /// <summary>What AgIO uses when the main service fails.</summary>
    public sealed class FallbackDefinition
    {
        public bool Enabled { get; set; }
        public FallbackType Type { get; set; } = FallbackType.NtripService;
        /// <summary>The fallback service, when the type is NtripService.</summary>
        public ServiceDefinition Service { get; set; } = new ServiceDefinition();
        /// <summary>The serial radio, when the type is SerialRadio.</summary>
        public string RadioPort { get; set; } = "";
        public int RadioBaud { get; set; } = 9600;
        /// <summary>Failed attempts in a row on the main service before AgIO switches (or asks).</summary>
        public int SwitchAfterFailures { get; set; } = 2;
        /// <summary>True: AgIO switches by itself. False: it only asks, and the user switches.</summary>
        public bool AutoSwitch { get; set; } = true;

        public FallbackDefinition Clone()
        {
            var copy = (FallbackDefinition)MemberwiseClone();
            copy.Service = Service == null ? new ServiceDefinition() : Service.Clone();
            return copy;
        }
    }

    /// <summary>
    /// An NTRIP profile: the main service and the fallback together, saved as its own file. AgIO's settings
    /// file only remembers the name of the profile in use, so another settings file brings its own NTRIP
    /// profile, and choosing another NTRIP profile changes nothing else in the settings.
    /// </summary>
    public sealed class NtripProfileSet
    {
        /// <summary>The name of the profile. It is also the name of its file.</summary>
        public string Name { get; set; } = "";
        public ServiceDefinition Main { get; set; } = new ServiceDefinition();
        public FallbackDefinition Fallback { get; set; } = new FallbackDefinition();

        public NtripProfileSet Clone()
        {
            return new NtripProfileSet
            {
                Name = Name,
                Main = Main == null ? new ServiceDefinition() : Main.Clone(),
                Fallback = Fallback == null ? new FallbackDefinition() : Fallback.Clone(),
            };
        }

        /// <summary>The main service as connection settings, or null while it has no address.</summary>
        public NtripProfile MainProfile(int ggaIntervalSeconds) => Main?.ToProfile(ggaIntervalSeconds);

        /// <summary>The fallback as connection settings, or null when it is off, a radio, or incomplete.</summary>
        public NtripProfile FallbackProfile(int ggaIntervalSeconds)
        {
            if (Fallback == null || !Fallback.Enabled || Fallback.Type != FallbackType.NtripService) return null;
            return Fallback.Service?.ToProfile(ggaIntervalSeconds);
        }
    }
}

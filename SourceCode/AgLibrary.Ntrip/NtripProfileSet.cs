namespace AgLibrary.Ntrip
{
    /// <summary>The kind of a source of corrections. Used for the fallback and for the main source.</summary>
    public enum FallbackType
    {
        /// <summary>An NTRIP service (or a raw TCP stream).</summary>
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

    /// <summary>
    /// The position AgIO gives the caster instead of the GPS position ("Fixed Position"). It belongs to the
    /// profile: a service near another field needs another position.
    /// </summary>
    public sealed class FixedPositionDefinition
    {
        /// <summary>True: the position below is sent. False: the position of the GPS receiver is.</summary>
        public bool Enabled { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }

        public FixedPositionDefinition Clone() => (FixedPositionDefinition)MemberwiseClone();
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
        /// <summary>What the main source is: an NTRIP service (<see cref="Main"/>) or a serial radio. A radio is not NTRIP.</summary>
        public FallbackType MainType { get; set; } = FallbackType.NtripService;
        public string MainRadioPort { get; set; } = "";
        public int MainRadioBaud { get; set; } = 9600;
        public FallbackDefinition Fallback { get; set; } = new FallbackDefinition();
        /// <summary>The Fixed Position of this profile; null in a profile file from before it was kept here (the GPS position is used).</summary>
        public FixedPositionDefinition FixedPosition { get; set; }

        public NtripProfileSet Clone()
        {
            return new NtripProfileSet
            {
                FixedPosition = FixedPosition?.Clone(),
                Name = Name,
                Main = Main == null ? new ServiceDefinition() : Main.Clone(),
                MainType = MainType,
                MainRadioPort = MainRadioPort,
                MainRadioBaud = MainRadioBaud,
                Fallback = Fallback == null ? new FallbackDefinition() : Fallback.Clone(),
            };
        }

        public bool MainIsRadio => MainType == FallbackType.SerialRadio;

        /// <summary>The main source can be started: a port for a radio, an address for a service.</summary>
        public bool MainIsComplete => MainIsRadio ? !string.IsNullOrWhiteSpace(MainRadioPort) : Main != null && Main.IsComplete;

        /// <summary>The main service as connection settings, or null while it has no address, or when the main source is a radio.</summary>
        public NtripProfile MainProfile(int ggaIntervalSeconds) => MainIsRadio ? null : Main?.ToProfile(ggaIntervalSeconds);

        /// <summary>The fallback as connection settings, or null when it is off, a radio, or incomplete.</summary>
        public NtripProfile FallbackProfile(int ggaIntervalSeconds)
        {
            if (Fallback == null || !Fallback.Enabled || Fallback.Type != FallbackType.NtripService) return null;
            return Fallback.Service?.ToProfile(ggaIntervalSeconds);
        }
    }
}

using System.Globalization;
using Android.Content;
using AgLibrary.Ntrip;

namespace AgOpenGPS.Droid
{
    /// <summary>Settings kept between runs (Android SharedPreferences).</summary>
    public sealed class AppSettings
    {
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;
        private readonly ISharedPreferences _prefs;

        public AppSettings(Context context)
        {
            _prefs = context.GetSharedPreferences("agopengps", FileCreationMode.Private);
            DeviceAddress = _prefs.GetString("bt.address", "");
            AutoConnect = _prefs.GetBoolean("bt.autoconnect", true);
            NtripEnabled = _prefs.GetBoolean("ntrip.enabled", false);
            NtripHost = _prefs.GetString("ntrip.host", "");
            NtripPort = _prefs.GetString("ntrip.port", "2101");
            NtripMount = _prefs.GetString("ntrip.mount", "");
            NtripUser = _prefs.GetString("ntrip.user", "");
            NtripPassword = _prefs.GetString("ntrip.password", "");
            NtripGgaSeconds = _prefs.GetString("ntrip.gga", "10");
            ToolWidth = _prefs.GetString("tool.width", "6.0");
            ToolOverlap = _prefs.GetString("tool.overlap", "0.0");
            ShowRawLog = _prefs.GetBoolean("ui.rawlog", false);
        }

        public string DeviceAddress;
        public bool AutoConnect;
        public bool NtripEnabled;
        public string NtripHost, NtripPort, NtripMount, NtripUser, NtripPassword, NtripGgaSeconds;
        /// <summary>Implement width and overlap in metres, as typed.</summary>
        public string ToolWidth, ToolOverlap;
        public bool ShowRawLog;

        /// <summary>Distance between passes: implement width minus overlap, like AgOpenGPS.</summary>
        public double WorkingWidthMeters
        {
            get
            {
                double width = ParseMeters(ToolWidth, 6.0);
                double overlap = ParseMeters(ToolOverlap, 0.0);
                return System.Math.Max(0.1, width - overlap);
            }
        }

        public static double ParseMeters(string text, double fallback)
        {
            // Accept a decimal comma too: many tablets have a European keyboard.
            string t = (text ?? "").Trim().Replace(',', '.');
            return double.TryParse(t, NumberStyles.Float, Inv, out double v) && v >= 0 && v < 100 ? v : fallback;
        }

        public void Save()
        {
            _prefs.Edit()
                .PutString("bt.address", DeviceAddress)
                .PutBoolean("bt.autoconnect", AutoConnect)
                .PutBoolean("ntrip.enabled", NtripEnabled)
                .PutString("ntrip.host", NtripHost)
                .PutString("ntrip.port", NtripPort)
                .PutString("ntrip.mount", NtripMount)
                .PutString("ntrip.user", NtripUser)
                .PutString("ntrip.password", NtripPassword)
                .PutString("ntrip.gga", NtripGgaSeconds)
                .PutString("tool.width", ToolWidth)
                .PutString("tool.overlap", ToolOverlap)
                .PutBoolean("ui.rawlog", ShowRawLog)
                .Apply();
        }

        /// <summary>Everything that needs a reconnect when changed.</summary>
        public string ConnectionKey =>
            string.Join("|", DeviceAddress, NtripEnabled, NtripHost, NtripPort, NtripMount, NtripUser, NtripPassword, NtripGgaSeconds);

        /// <summary>The caster to use, or null when NTRIP is off or the settings are incomplete
        /// (the Pi may be doing NTRIP itself).</summary>
        public NtripProfile BuildNtripProfile()
        {
            if (!NtripEnabled || string.IsNullOrWhiteSpace(NtripHost) || string.IsNullOrWhiteSpace(NtripMount)) return null;
            if (!int.TryParse(NtripPort, NumberStyles.Integer, Inv, out int port) || port <= 0 || port > 65535) return null;
            int.TryParse(NtripGgaSeconds, NumberStyles.Integer, Inv, out int gga);
            return new NtripProfile(NtripHost, port, NtripMount, NtripUser, NtripPassword, NtripVersion.Auto, gga);
        }
    }
}

using System.Globalization;
using Android.Content;
using AgLibrary.Ntrip;

namespace AgOpenGPS.Droid
{
    /// <summary>Settings kept between runs (Android SharedPreferences).</summary>
    public sealed class AppSettings
    {
        private readonly ISharedPreferences _prefs;

        public AppSettings(Context context)
        {
            _prefs = context.GetSharedPreferences("agopengps", FileCreationMode.Private);
            DeviceAddress = _prefs.GetString("bt.address", "");
            NtripEnabled = _prefs.GetBoolean("ntrip.enabled", false);
            NtripHost = _prefs.GetString("ntrip.host", "");
            NtripPort = _prefs.GetString("ntrip.port", "2101");
            NtripMount = _prefs.GetString("ntrip.mount", "");
            NtripUser = _prefs.GetString("ntrip.user", "");
            NtripPassword = _prefs.GetString("ntrip.password", "");
            NtripGgaSeconds = _prefs.GetString("ntrip.gga", "10");
            ShowRawLog = _prefs.GetBoolean("ui.rawlog", false);
        }

        public string DeviceAddress;
        public bool NtripEnabled;
        public string NtripHost, NtripPort, NtripMount, NtripUser, NtripPassword, NtripGgaSeconds;
        public bool ShowRawLog;

        public void Save()
        {
            _prefs.Edit()
                .PutString("bt.address", DeviceAddress)
                .PutBoolean("ntrip.enabled", NtripEnabled)
                .PutString("ntrip.host", NtripHost)
                .PutString("ntrip.port", NtripPort)
                .PutString("ntrip.mount", NtripMount)
                .PutString("ntrip.user", NtripUser)
                .PutString("ntrip.password", NtripPassword)
                .PutString("ntrip.gga", NtripGgaSeconds)
                .PutBoolean("ui.rawlog", ShowRawLog)
                .Apply();
        }

        /// <summary>The caster to use, or null when NTRIP is off or the settings are incomplete
        /// (the Pi may be doing NTRIP itself).</summary>
        public NtripProfile BuildNtripProfile()
        {
            if (!NtripEnabled || string.IsNullOrWhiteSpace(NtripHost) || string.IsNullOrWhiteSpace(NtripMount)) return null;
            if (!int.TryParse(NtripPort, NumberStyles.Integer, CultureInfo.InvariantCulture, out int port) || port <= 0 || port > 65535) return null;
            int.TryParse(NtripGgaSeconds, NumberStyles.Integer, CultureInfo.InvariantCulture, out int gga);
            return new NtripProfile(NtripHost, port, NtripMount, NtripUser, NtripPassword, NtripVersion.Auto, gga);
        }
    }
}

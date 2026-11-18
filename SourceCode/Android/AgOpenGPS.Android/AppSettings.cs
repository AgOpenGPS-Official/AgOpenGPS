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
            Language = _prefs.GetString("ui.language", Tr.System);
            DayColours = _prefs.GetBoolean("ui.dayColours", false);
            HeadlandWidth = _prefs.GetString("headland.width", "0");
            AutoHeadland = _prefs.GetBoolean("headland.auto", true);
            foreach (AgOpenGPS.Mobile.Core.AlertKind k in (AgOpenGPS.Mobile.Core.AlertKind[])System.Enum.GetValues(typeof(AgOpenGPS.Mobile.Core.AlertKind)))
                if (_prefs.GetBoolean("alert." + k, k != AgOpenGPS.Mobile.Core.AlertKind.OffLine)) Alerts.Add(k);
            AlertEdgeMeters = _prefs.GetString("alert.edgeMeters", "5");
            AlertOffLineCm = _prefs.GetString("alert.offLineCm", "20");
            RecordCoverage = _prefs.GetBoolean("coverage.record", true);
            AntennaHeight = _prefs.GetString("vehicle.antennaHeight", "0");
            AntennaForward = _prefs.GetString("vehicle.antennaForward", "0");
            AntennaRight = _prefs.GetString("vehicle.antennaRight", "0");
            ToolBehind = _prefs.GetString("tool.behind", "0");
            ToolRight = _prefs.GetString("tool.right", "0");
            Wheelbase = _prefs.GetString("vehicle.wheelbase", "3.0");
            NudgeStepCm = _prefs.GetString("guidance.nudgeCm", "5");
            UseSimulator = _prefs.GetBoolean("sim.enabled", false);
            SimLatitude = _prefs.GetString("sim.lat", "");
            SimLongitude = _prefs.GetString("sim.lon", "");
            ShowMap = _prefs.GetBoolean("map.show", true);
            MapUrl = _prefs.GetString("map.url", AgOpenGPS.Mobile.Core.Guidance.MapTiles.DefaultUrl);
            MapMaxZoom = _prefs.GetString("map.maxZoom", AgOpenGPS.Mobile.Core.Guidance.MapTiles.MaxZoom.ToString(Inv));
            MapAttribution = _prefs.GetString("map.attribution", AgOpenGPS.Mobile.Core.Guidance.MapTiles.DefaultAttribution);
            ShowCoverage = _prefs.GetBoolean("coverage.show", true);
        }

        public string DeviceAddress;
        public bool AutoConnect;
        public bool NtripEnabled;
        public string NtripHost, NtripPort, NtripMount, NtripUser, NtripPassword, NtripGgaSeconds;
        /// <summary>Implement width and overlap in metres, as typed.</summary>
        public string ToolWidth, ToolOverlap;
        public bool ShowRawLog;
        /// <summary>"system", "en" or "hr" (see <see cref="Tr"/>).</summary>
        public string Language;
        /// <summary>Light colours for bright sun; dark (false) for night.</summary>
        public bool DayColours;
        /// <summary>Headland width in metres as typed; 0 = no headland.</summary>
        public string HeadlandWidth;
        /// <summary>Work off in the headland, on again in the main part.</summary>
        public bool AutoHeadland;
        public double HeadlandWidthMeters => ParseMeters(HeadlandWidth, 0);

        /// <summary>All settings as stored, for a backup. The paired Bluetooth device is left out: it belongs to this tablet.</summary>
        public System.Collections.Generic.IDictionary<string, object> ExportAll()
        {
            var d = new System.Collections.Generic.Dictionary<string, object>();
            foreach (var kv in _prefs.All)
            {
                if (kv.Key == "bt.address") continue;
                if (kv.Value is Java.Lang.Boolean jb) d[kv.Key] = jb.BooleanValue();
                else if (kv.Value != null) d[kv.Key] = kv.Value.ToString();
            }
            return d;
        }

        /// <summary>Writes settings from a backup (strings and switches); the app reloads them afterwards.</summary>
        public void ImportAll(System.Collections.Generic.IDictionary<string, object> values)
        {
            ISharedPreferencesEditor e = _prefs.Edit();
            foreach (var kv in values)
            {
                if (kv.Key == "bt.address") continue;
                if (kv.Value is bool b) e.PutBoolean(kv.Key, b);
                else if (kv.Value is string s) e.PutString(kv.Key, s);
            }
            e.Commit();
        }

        /// <summary>Alerts that sound (all but "off the line" by default).</summary>
        public readonly System.Collections.Generic.HashSet<AgOpenGPS.Mobile.Core.AlertKind> Alerts =
            new System.Collections.Generic.HashSet<AgOpenGPS.Mobile.Core.AlertKind>();
        public string AlertEdgeMeters, AlertOffLineCm;

        public AgOpenGPS.Mobile.Core.AlertSettings BuildAlertSettings()
        {
            var a = new AgOpenGPS.Mobile.Core.AlertSettings
            {
                EdgeWarningMeters = System.Math.Max(0.5, ParseMeters(AlertEdgeMeters, 5)),
                OffLineMeters = System.Math.Max(0.02, ParseMeters(AlertOffLineCm, 20) / 100.0),
            };
            a.Enabled.Clear();
            foreach (var k in Alerts) a.Enabled.Add(k);
            return a;
        }
        /// <summary>Vehicle and implement offsets in metres, as typed (see VehicleGeometry).</summary>
        public string AntennaHeight, AntennaForward, AntennaRight, ToolBehind, ToolRight, Wheelbase;
        public string NudgeStepCm;
        public bool UseSimulator;
        /// <summary>Simulator start position; empty = the open field, or the last position.</summary>
        public string SimLatitude, SimLongitude;
        public bool ShowMap;
        public string MapUrl, MapAttribution, MapMaxZoom;

        /// <summary>Most detailed map zoom to request, 10 to 22.</summary>
        public int MapMaxZoomLevel
        {
            get
            {
                int.TryParse(MapMaxZoom, NumberStyles.Integer, Inv, out int z);
                return z < 10 ? AgOpenGPS.Mobile.Core.Guidance.MapTiles.MaxZoom : System.Math.Min(AgOpenGPS.Mobile.Core.Guidance.MapTiles.AbsoluteMaxZoom, z);
            }
        }

        /// <summary>Record the worked area (Work button and statistics).</summary>
        public bool RecordCoverage;
        /// <summary>Draw the worked area on the map.</summary>
        public bool ShowCoverage;

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

        /// <summary>Full implement width: the width of the strip painted as worked.</summary>
        public double ToolWidthMeters => System.Math.Max(0.1, ParseMeters(ToolWidth, 6.0));

        public AgOpenGPS.Mobile.Core.Guidance.VehicleGeometry BuildVehicleGeometry()
        {
            return new AgOpenGPS.Mobile.Core.Guidance.VehicleGeometry
            {
                AntennaHeight = ParseSigned(AntennaHeight, 0),
                AntennaForward = ParseSigned(AntennaForward, 0),
                AntennaRight = ParseSigned(AntennaRight, 0),
                ToolBehind = ParseSigned(ToolBehind, 0),
                ToolRight = ParseSigned(ToolRight, 0),
            };
        }

        public double WheelbaseMeters => System.Math.Max(0.5, ParseMeters(Wheelbase, 3.0));

        /// <summary>Nudge step in metres (typed in cm).</summary>
        public double NudgeStepMeters => System.Math.Max(0.005, ParseMeters(NudgeStepCm, 5) / 100.0);

        /// <summary>A number that may be negative (offsets to the left or behind), with . or , as decimal mark.</summary>
        public static double ParseSigned(string text, double fallback)
        {
            string t = (text ?? "").Trim().Replace(',', '.');
            return double.TryParse(t, NumberStyles.Float, Inv, out double v) && v > -100 && v < 100 ? v : fallback;
        }

        public static double ParseMeters(string text, double fallback)
        {
            // Accept a decimal comma too: many tablets have a European keyboard.
            string t = (text ?? "").Trim().Replace(',', '.');
            return double.TryParse(t, NumberStyles.Float, Inv, out double v) && v >= 0 && v < 100 ? v : fallback;
        }

        public void Save()
        {
            var alertEdit = _prefs.Edit();
            foreach (AgOpenGPS.Mobile.Core.AlertKind k in (AgOpenGPS.Mobile.Core.AlertKind[])System.Enum.GetValues(typeof(AgOpenGPS.Mobile.Core.AlertKind)))
                alertEdit.PutBoolean("alert." + k, Alerts.Contains(k));
            alertEdit.Apply();

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
                .PutString("ui.language", Language)
                .PutBoolean("ui.dayColours", DayColours)
                .PutString("headland.width", HeadlandWidth)
                .PutBoolean("headland.auto", AutoHeadland)
                .PutString("alert.edgeMeters", AlertEdgeMeters)
                .PutString("alert.offLineCm", AlertOffLineCm)
                .PutBoolean("coverage.record", RecordCoverage)
                .PutBoolean("coverage.show", ShowCoverage)
                .PutString("vehicle.antennaHeight", AntennaHeight)
                .PutString("vehicle.antennaForward", AntennaForward)
                .PutString("vehicle.antennaRight", AntennaRight)
                .PutString("tool.behind", ToolBehind)
                .PutString("tool.right", ToolRight)
                .PutString("vehicle.wheelbase", Wheelbase)
                .PutString("guidance.nudgeCm", NudgeStepCm)
                .PutBoolean("sim.enabled", UseSimulator)
                .PutString("sim.lat", SimLatitude)
                .PutString("sim.lon", SimLongitude)
                .PutBoolean("map.show", ShowMap)
                .PutString("map.url", MapUrl)
                .PutString("map.attribution", MapAttribution)
                .PutString("map.maxZoom", MapMaxZoom)
                .Apply();
        }

        /// <summary>Everything that needs a reconnect when changed.</summary>
        public string ConnectionKey =>
            string.Join("|", DeviceAddress, UseSimulator, NtripEnabled, NtripHost, NtripPort, NtripMount, NtripUser, NtripPassword, NtripGgaSeconds);

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

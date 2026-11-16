using System;
using System.Collections.Concurrent;
using System.IO;
using Android.Bluetooth;
using Android.Content;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// State shared by all screens and kept while the app process lives: settings, the Bluetooth
    /// connection, the guidance engine, the saved fields and lines. While connected,
    /// <see cref="GnssService"/> keeps the process running in the background.
    /// </summary>
    public static class AppState
    {
        public const int RawLogLines = 40;

        private static readonly object Lock = new object();
        private static Context _context;

        public static AppSettings Settings { get; private set; }
        public static AbLineStore Lines { get; private set; }
        public static FieldStore Fields { get; private set; }
        public static GuidanceEngine Guidance { get; } = new GuidanceEngine();
        public static LinkSupervisor Link { get; private set; }
        public static ConcurrentQueue<string> RawLog { get; } = new ConcurrentQueue<string>();

        public static bool IsConnected => Link != null && Link.IsRunning;

        /// <summary>The virtual tractor while connected to the simulator, else null.</summary>
        public static GnssSimulator Simulator { get; private set; }

        /// <summary>Satellite map tiles (memory + disk cache).</summary>
        public static MapTileProvider Map { get; private set; }

        public static void Init(Context context)
        {
            lock (Lock)
            {
                if (_context != null) return;
                _context = context.ApplicationContext;
                Settings = new AppSettings(_context);
                Tr.SetLanguage(Settings.Language);
                string dir = _context.FilesDir.AbsolutePath;
                Lines = new AbLineStore(Path.Combine(dir, "ablines.json"));
                Fields = new FieldStore(Path.Combine(dir, "fields.json"));
                Guidance.PassWidth = Settings.WorkingWidthMeters;
                Guidance.ToolWidth = Settings.ToolWidthMeters;
                Guidance.CoverageEnabled = Settings.RecordCoverage;
                Guidance.Vehicle = Settings.BuildVehicleGeometry();
                Map = new MapTileProvider(Path.Combine(dir, "maptiles"));
                Guidance.SetActiveField(Fields.Active);
                Guidance.SetCoverage(LoadCoverage(Fields.Active));
                AbLine line = Lines.Active;
                Guidance.SetActiveLine(line != null && line.FieldId == Fields.Active?.Id ? line : null);
            }
        }

        public static BluetoothAdapter Adapter =>
            ((BluetoothManager)_context.GetSystemService(Context.BluetoothService))?.Adapter;

        // ---------------------------------------------------------------- fields and lines

        /// <summary>Opens a field (null closes it). A line of another field is put away.</summary>
        public static void OpenField(Field field)
        {
            SaveCoverage();
            Guidance.Working = false;
            Fields.SetActive(field?.Id);
            Field active = Fields.Active;
            Guidance.SetActiveField(active);
            Guidance.SetCoverage(LoadCoverage(active));
            AbLine line = Guidance.ActiveLine;
            if (line != null && line.FieldId != active?.Id) UseLine(null);
            MoveSimulatorToField();
        }

        /// <summary>In the simulator, puts the tractor in the middle of the open field. False if it cannot.</summary>
        public static bool MoveSimulatorToField()
        {
            GnssSimulator sim = Simulator;
            Field f = Fields.Active;
            if (sim == null || f == null || !f.TryGetCentre(out double lat, out double lon)) return false;
            sim.MoveTo(lat, lon);
            return true;
        }

        /// <summary>Re-reads the open field after its boundary or name changed.</summary>
        public static void RefreshField() => Guidance.SetActiveField(Fields.Active);

        public static void DeleteField(Field field)
        {
            bool wasOpen = Fields.Active?.Id == field.Id;
            if (wasOpen) Guidance.SetCoverage(null);
            try { File.Delete(CoveragePath(field)); } catch (IOException) { }
            Lines.DeleteForField(field.Id);
            Fields.Delete(field.Id);
            if (wasOpen) OpenField(null);
        }

        /// <summary>
        /// The simulator starts at the position in Settings, else in the open field, else at the
        /// last position, else near Zagreb.
        /// </summary>
        private static GnssSimulator CreateSimulator()
        {
            double lat = ParseDegrees(Settings.SimLatitude, 85);
            double lon = ParseDegrees(Settings.SimLongitude, 180);
            if (double.IsNaN(lon)) lat = double.NaN;
            if (double.IsNaN(lat))
            {
                Field f = Fields.Active;
                GnssFix last = Guidance.GetSnapshot().Fix;
                if (f != null && f.TryGetCentre(out double fLat, out double fLon))
                {
                    lat = fLat;
                    lon = fLon;
                }
                else if (last != null && last.HasPosition)
                {
                    lat = last.Latitude;
                    lon = last.Longitude;
                }
                else
                {
                    lat = 45.8150;
                    lon = 15.9819;
                }
            }

            var sim = new GnssSimulator(lat, lon) { Wheelbase = Settings.WheelbaseMeters };
            sim.SteerSource = () => Guidance.GetSnapshot().Steer;
            return sim;
        }

        private static double ParseDegrees(string text, double limit)
        {
            string t = (text ?? "").Trim().Replace(',', '.');
            return double.TryParse(t, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double v)
                && Math.Abs(v) <= limit ? v : double.NaN;
        }

        // ---------------------------------------------------------------- nudge

        /// <summary>Moves the active line by one nudge step to the right (+1) or left (-1) as seen while driving.</summary>
        public static void Nudge(int direction)
        {
            AbLine line = Guidance.ActiveLine;
            double n = Guidance.NudgeActiveLine(direction * Settings.NudgeStepMeters);
            if (line != null && !double.IsNaN(n)) Lines.SetNudge(line.Id, n);
        }

        public static void SnapLine()
        {
            AbLine line = Guidance.ActiveLine;
            double n = Guidance.SnapActiveLine();
            if (line != null && !double.IsNaN(n)) Lines.SetNudge(line.Id, n);
        }

        public static void ResetNudge()
        {
            AbLine line = Guidance.ActiveLine;
            Guidance.ResetNudge();
            if (line != null) Lines.SetNudge(line.Id, 0);
        }

        // ---------------------------------------------------------------- worked area

        private static DateTime _lastCoverageSave = DateTime.UtcNow;

        private static string CoveragePath(Field field)
            => Path.Combine(_context.FilesDir.AbsolutePath, "coverage", field.Id + ".bin");

        private static CoverageMap LoadCoverage(Field field) => field == null ? null : CoverageMap.Load(CoveragePath(field));

        public static void SetWorking(bool on)
        {
            Guidance.Working = on;
            if (!on) SaveCoverage();
        }

        /// <summary>Saves the open field's worked area if it changed. Coverage without a field is not kept.</summary>
        public static void SaveCoverage()
        {
            Field field = Fields?.Active;
            CoverageMap map = Guidance.Coverage;
            _lastCoverageSave = DateTime.UtcNow;
            if (field == null || map == null || !map.Dirty) return;
            try { map.Save(CoveragePath(field)); }
            catch (IOException) { /* storage full: try again next time */ }
        }

        /// <summary>Called regularly (screen and background service): saves the worked area every 30 s.</summary>
        public static void Tick()
        {
            if ((DateTime.UtcNow - _lastCoverageSave).TotalSeconds >= 30) SaveCoverage();
        }

        /// <summary>Forgets the worked area of the open field.</summary>
        public static void ClearCoverage()
        {
            CoverageMap map = Guidance.Coverage;
            if (map == null) return;
            map.Clear();
            SaveCoverage();
        }

        public static void UseLine(AbLine line)
        {
            Lines.SetActive(line?.Id);
            Guidance.SetActiveLine(line == null ? null : Lines.Active);
        }

        // ---------------------------------------------------------------- connection

        /// <summary>Starts the link to the saved device. Returns a message when it cannot.</summary>
        public static string Connect()
        {
            lock (Lock)
            {
                if (IsConnected) return null;
                AppSettings settings = Settings;
                while (RawLog.TryDequeue(out _)) { }

                LinkSupervisor link;
                if (settings.UseSimulator)
                {
                    GnssSimulator sim = CreateSimulator();
                    Simulator = sim;
                    link = new LinkSupervisor(ct => new SimulatorLink(sim), () => null);
                }
                else
                {
                    string address = Settings.DeviceAddress;
                    if (!BluetoothAdapter.CheckBluetoothAddress(address)) return "Pick the receiver in Settings first";
                    BluetoothAdapter adapter = Adapter;
                    Simulator = null;
                    link = new LinkSupervisor(ct => BluetoothLink.Connect(adapter, address, ct), settings.BuildNtripProfile);
                }
                link.FixUpdated += Guidance.OnFix;
                link.SentenceReceived += line =>
                {
                    if (!Settings.ShowRawLog) return;
                    RawLog.Enqueue(line);
                    while (RawLog.Count > RawLogLines) RawLog.TryDequeue(out _);
                };
                Link = link;
                link.Start();
                StartService();

                if (settings.UseSimulator) return "Simulator: set speed with + / -, steer with < >, or press Auto";
                return settings.NtripEnabled && settings.BuildNtripProfile() == null
                    ? "NTRIP settings incomplete - connecting without corrections"
                    : null;
            }
        }

        /// <summary>Stops the link. Blocks up to a few seconds: call it off the UI thread.</summary>
        public static void Disconnect()
        {
            LinkSupervisor link;
            lock (Lock)
            {
                link = Link;
                Link = null;
            }
            link?.Stop();
            Simulator = null;
            try { _context.StopService(new Intent(_context, typeof(GnssService))); }
            catch (Java.Lang.Exception) { }
        }

        /// <summary>Applies changed connection settings: reconnects if connected. Off the UI thread.</summary>
        public static void Reconnect()
        {
            if (!IsConnected) return;
            Disconnect();
            Connect();
        }

        private static void StartService()
        {
            try
            {
                _context.StartForegroundService(new Intent(_context, typeof(GnssService)));
            }
            catch (Java.Lang.Exception)
            {
                // Android refuses a foreground service started from the background (e.g. a
                // reconnect while the app is not visible). The link still runs while the app is open.
            }
        }
    }
}

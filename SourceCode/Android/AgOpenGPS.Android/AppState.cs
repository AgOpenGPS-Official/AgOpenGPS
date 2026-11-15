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

        public static void Init(Context context)
        {
            lock (Lock)
            {
                if (_context != null) return;
                _context = context.ApplicationContext;
                Settings = new AppSettings(_context);
                string dir = _context.FilesDir.AbsolutePath;
                Lines = new AbLineStore(Path.Combine(dir, "ablines.json"));
                Fields = new FieldStore(Path.Combine(dir, "fields.json"));
                Guidance.PassWidth = Settings.WorkingWidthMeters;
                Guidance.ToolWidth = Settings.ToolWidthMeters;
                Guidance.CoverageEnabled = Settings.RecordCoverage;
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
                string address = Settings.DeviceAddress;
                if (!BluetoothAdapter.CheckBluetoothAddress(address)) return "Pick the receiver in Settings first";

                BluetoothAdapter adapter = Adapter;
                AppSettings settings = Settings;
                while (RawLog.TryDequeue(out _)) { }

                var link = new LinkSupervisor(ct => BluetoothLink.Connect(adapter, address, ct), settings.BuildNtripProfile);
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

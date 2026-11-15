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
    /// connection, the guidance engine and the saved A-B lines. The connection keeps running while
    /// the settings screen is open.
    /// </summary>
    public static class AppState
    {
        public const int RawLogLines = 40;

        private static readonly object Lock = new object();
        private static Context _context;

        public static AppSettings Settings { get; private set; }
        public static AbLineStore Lines { get; private set; }
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
                Lines = new AbLineStore(Path.Combine(_context.FilesDir.AbsolutePath, "ablines.json"));
                Guidance.PassWidth = Settings.WorkingWidthMeters;
                Guidance.SetActiveLine(Lines.Active);
            }
        }

        public static BluetoothAdapter Adapter =>
            ((BluetoothManager)_context.GetSystemService(Context.BluetoothService))?.Adapter;

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
        }

        /// <summary>Applies changed connection settings: reconnects if connected. Off the UI thread.</summary>
        public static void Reconnect()
        {
            if (!IsConnected) return;
            Disconnect();
            Connect();
        }
    }
}

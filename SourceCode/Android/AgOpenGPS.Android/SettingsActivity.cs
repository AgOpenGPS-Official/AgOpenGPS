using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Bluetooth;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Text;
using Android.Views;
using Android.Widget;
using AgLibrary.Ntrip;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// All settings: receiver, implement, NTRIP, plus a diagnostics panel. Changes are saved when
    /// the screen is left; connection changes reconnect a running link.
    /// </summary>
    [Activity(Label = "Settings", Theme = "@android:style/Theme.Material.Light.NoActionBar",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
    public class SettingsActivity : Activity
    {
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        private readonly List<string> _deviceAddresses = new List<string>();
        private string _connectionKeyAtStart;
        private Handler _ui;
        private bool _refreshing;

        private Spinner _devices;
        private CheckBox _autoConnect, _ntripEnabled, _rawLog, _recordCoverage, _showCoverage, _useSim, _showMap;
        private EditText _mapMaxZoom;
        private EditText _antHeight, _antForward, _antRight, _toolBehind, _toolRight, _wheelbase, _nudgeStep, _simLat, _simLon, _mapUrl, _mapAttribution;
        private TextView _mapStatus;
        private System.Threading.CancellationTokenSource _prefetch;
        private EditText _width, _overlap, _host, _port, _mount, _user, _password, _gga;
        private TextView _workingWidth, _status, _raw;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            AppState.Init(this);
            _ui = new Handler(Looper.MainLooper);
            SetContentView(BuildLayout());
            LoadFields();
            LoadPairedDevices();
            _connectionKeyAtStart = AppState.Settings.ConnectionKey;
        }

        protected override void OnResume()
        {
            base.OnResume();
            _refreshing = true;
            _ui.Post(Refresh);
        }

        protected override void OnPause()
        {
            _refreshing = false;
            SaveFields();
            base.OnPause();
        }

        private void SaveFields()
        {
            AppSettings s = AppState.Settings;
            int index = _devices.SelectedItemPosition;
            if (index >= 0 && index < _deviceAddresses.Count) s.DeviceAddress = _deviceAddresses[index];
            s.AutoConnect = _autoConnect.Checked;
            s.ToolWidth = _width.Text.Trim();
            s.ToolOverlap = _overlap.Text.Trim();
            s.NtripEnabled = _ntripEnabled.Checked;
            s.NtripHost = _host.Text.Trim();
            s.NtripPort = _port.Text.Trim();
            s.NtripMount = _mount.Text.Trim();
            s.NtripUser = _user.Text.Trim();
            s.NtripPassword = _password.Text;
            s.NtripGgaSeconds = _gga.Text.Trim();
            s.ShowRawLog = _rawLog.Checked;
            s.AntennaHeight = _antHeight.Text.Trim();
            s.AntennaForward = _antForward.Text.Trim();
            s.AntennaRight = _antRight.Text.Trim();
            s.ToolBehind = _toolBehind.Text.Trim();
            s.ToolRight = _toolRight.Text.Trim();
            s.Wheelbase = _wheelbase.Text.Trim();
            s.NudgeStepCm = _nudgeStep.Text.Trim();
            s.UseSimulator = _useSim.Checked;
            s.SimLatitude = _simLat.Text.Trim();
            s.SimLongitude = _simLon.Text.Trim();
            s.ShowMap = _showMap.Checked;
            s.MapUrl = string.IsNullOrWhiteSpace(_mapUrl.Text) ? MapTiles.DefaultUrl : _mapUrl.Text.Trim();
            s.MapAttribution = _mapAttribution.Text.Trim();
            s.MapMaxZoom = _mapMaxZoom.Text.Trim();
            if (s.RecordCoverage && !_recordCoverage.Checked) AppState.SetWorking(false); // switched off: stop and save
            s.RecordCoverage = _recordCoverage.Checked;
            s.ShowCoverage = _showCoverage.Checked;
            s.Save();
            AppState.Guidance.PassWidth = s.WorkingWidthMeters;
            AppState.Guidance.ToolWidth = s.ToolWidthMeters;
            AppState.Guidance.CoverageEnabled = s.RecordCoverage;
            AppState.Guidance.Vehicle = s.BuildVehicleGeometry();
            AppState.Map.UrlTemplate = s.MapUrl;

            string key = s.ConnectionKey;
            if (key != _connectionKeyAtStart)
            {
                _connectionKeyAtStart = key;
                if (AppState.IsConnected) Task.Run(AppState.Reconnect);
            }
        }

        private void LoadFields()
        {
            AppSettings s = AppState.Settings;
            _autoConnect.Checked = s.AutoConnect;
            _width.Text = s.ToolWidth;
            _overlap.Text = s.ToolOverlap;
            _ntripEnabled.Checked = s.NtripEnabled;
            _host.Text = s.NtripHost;
            _port.Text = s.NtripPort;
            _mount.Text = s.NtripMount;
            _user.Text = s.NtripUser;
            _password.Text = s.NtripPassword;
            _gga.Text = s.NtripGgaSeconds;
            _rawLog.Checked = s.ShowRawLog;
            _recordCoverage.Checked = s.RecordCoverage;
            _showCoverage.Checked = s.ShowCoverage;
            _showCoverage.Enabled = s.RecordCoverage;
            _antHeight.Text = s.AntennaHeight;
            _antForward.Text = s.AntennaForward;
            _antRight.Text = s.AntennaRight;
            _toolBehind.Text = s.ToolBehind;
            _toolRight.Text = s.ToolRight;
            _wheelbase.Text = s.Wheelbase;
            _nudgeStep.Text = s.NudgeStepCm;
            _useSim.Checked = s.UseSimulator;
            _simLat.Text = s.SimLatitude;
            _simLon.Text = s.SimLongitude;
            _showMap.Checked = s.ShowMap;
            _mapUrl.Text = s.MapUrl;
            _mapAttribution.Text = s.MapAttribution;
            _mapMaxZoom.Text = s.MapMaxZoom;
            Task.Run(() =>
            {
                long bytes = AppState.Map.DiskBytes();
                RunOnUiThread(() => _mapStatus.Text = string.Format(Inv, "Saved on the tablet: {0:0.0} MB", bytes / 1048576.0));
            });
            UpdateWorkingWidth();
        }

        private void UpdateWorkingWidth()
        {
            double w = AppSettings.ParseMeters(_width.Text, double.NaN);
            double o = AppSettings.ParseMeters(_overlap.Text, double.NaN);
            _workingWidth.Text = double.IsNaN(w) || double.IsNaN(o)
                ? "Enter the width in metres, e.g. 6.0"
                : string.Format(Inv, "Distance between passes: {0:0.00} m", System.Math.Max(0.1, w - o));
        }

        private void LoadPairedDevices()
        {
            _deviceAddresses.Clear();
            var names = new List<string>();
            BluetoothAdapter adapter = AppState.Adapter;

            if (adapter == null) names.Add("This tablet has no Bluetooth");
            else if (!adapter.IsEnabled) names.Add("Bluetooth is off - turn it on and press Refresh");
            else
            {
                try
                {
                    foreach (BluetoothDevice d in adapter.BondedDevices.OrderBy(d => d.Name ?? ""))
                    {
                        _deviceAddresses.Add(d.Address);
                        names.Add((string.IsNullOrEmpty(d.Name) ? "(no name)" : d.Name) + "  " + d.Address);
                    }
                }
                catch (Java.Lang.SecurityException)
                {
                    names.Add("Allow 'Nearby devices' for AgOpenGPS in Android settings");
                }
                if (names.Count == 0) names.Add("No paired devices - press Pair...");
            }

            var a = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, names);
            a.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _devices.Adapter = a;
            int selected = _deviceAddresses.IndexOf(AppState.Settings.DeviceAddress);
            if (selected >= 0) _devices.SetSelection(selected);
        }

        // ---------------------------------------------------------------- diagnostics, 2 Hz

        private void Refresh()
        {
            if (!_refreshing) return;
            try { RenderStatus(); }
            finally { _ui.PostDelayed(Refresh, 500); }
        }

        private void RenderStatus()
        {
            LinkSnapshot snap = AppState.Link?.GetSnapshot();
            var sb = new StringBuilder();
            if (snap == null)
            {
                sb.Append("Bluetooth: not connected");
            }
            else
            {
                sb.Append("Bluetooth: ").Append(snap.State).Append(" - ").Append(snap.Message);
                if (snap.Connects > 1) sb.Append("  (reconnects: ").Append(snap.Connects - 1).Append(')');
                SessionStats st = snap.Session;
                if (st != null)
                {
                    GnssFix f = st.Latest;
                    sb.Append('\n').AppendFormat(Inv, "Data: {0:0.0} Hz, {1}", st.RateHz,
                        f == null ? "no position yet" : f.Source == FixSource.Pgn ? "AgOpenGPS PGN (bridge)" : "NMEA (receiver)");
                    if (f != null && f.HasPosition)
                        sb.Append('\n').AppendFormat(Inv, "{0}  {1:0.0000000} {2:0.0000000}  sats {3}  HDOP {4:0.00}  age {5:0.0} s",
                            GnssFix.FixQualityName(f.FixQuality), f.Latitude, f.Longitude, f.Satellites, f.Hdop, f.AgeSeconds);
                    sb.Append('\n').AppendFormat(Inv,
                        "In {0:N0} B   out {1:N0} B   NMEA ok {2:N0} / bad {3:N0}   PGN {4:N0}   skipped {5:N0} B",
                        st.BytesIn, st.BytesOut, st.Sentences, st.BadSentences, st.PgnFrames, st.SkippedBytes);
                }
                NtripStatus n = snap.Ntrip;
                sb.Append('\n').Append("NTRIP: ");
                if (n == null) sb.Append(AppState.Settings.NtripEnabled ? "not running (check settings)" : "off");
                else
                {
                    sb.Append(n.Headline);
                    if (n.Frames > 0) sb.AppendFormat(Inv, ", {0} frames, last {1:0.0} s ago, {2:N0} B to receiver", n.Frames, n.SecondsSinceLastFrame, snap.NtripBytesSent);
                    else if (!string.IsNullOrEmpty(n.Message)) sb.Append(" - ").Append(n.Message);
                }
            }
            _status.Text = sb.ToString();

            _raw.Visibility = _rawLog.Checked ? ViewStates.Visible : ViewStates.Gone;
            if (_rawLog.Checked) _raw.Text = string.Join("\n", AppState.RawLog.ToArray());
        }

        // ---------------------------------------------------------------- map download

        /// <summary>Downloads zoom 14 to 19 for the open field's boundary (or 500 m around the tractor), plus 100 m.</summary>
        private void DownloadFieldMap()
        {
            if (_prefetch != null)
            {
                _prefetch.Cancel();
                return;
            }
            SaveFields();

            double south, west, north, east;
            Field field = AppState.Fields.Active;
            GnssFix fix = AppState.Guidance.GetSnapshot().Fix;
            if (field != null && field.HasBoundary)
            {
                south = field.Boundary.Min(p => p.Latitude); north = field.Boundary.Max(p => p.Latitude);
                west = field.Boundary.Min(p => p.Longitude); east = field.Boundary.Max(p => p.Longitude);
            }
            else if (fix != null && fix.HasPosition)
            {
                south = north = fix.Latitude;
                west = east = fix.Longitude;
                double m = 400;
                south -= m / 111132; north += m / 111132;
                double dl = m / (111320 * Math.Cos(fix.Latitude * Math.PI / 180));
                west -= dl; east += dl;
            }
            else
            {
                Toast.MakeText(this, "Open a field with a boundary, or connect to get a position first", ToastLength.Long).Show();
                return;
            }
            double margin = 100;
            south -= margin / 111132; north += margin / 111132;
            double dm = margin / (111320 * Math.Cos(south * Math.PI / 180));
            west -= dm; east += dm;

            var tiles = new List<TileId>();
            for (int z = 14; z <= AppState.Settings.MapMaxZoomLevel; z++) tiles.AddRange(MapTiles.TilesFor(south, west, north, east, z));
            if (tiles.Count > 3000)
            {
                Toast.MakeText(this, "That area is too large (" + tiles.Count + " tiles)", ToastLength.Long).Show();
                return;
            }

            new AlertDialog.Builder(this)
                .SetTitle("Download the map?")
                .SetMessage(string.Format(Inv, "{0} map squares, about {1:0} MB. Best over Wi-Fi.", tiles.Count, tiles.Count * 0.02))
                .SetPositiveButton("Download", (s, e) => StartPrefetch(tiles))
                .SetNegativeButton("Cancel", (s, e) => { })
                .Show();
        }

        private void StartPrefetch(List<TileId> tiles)
        {
            _prefetch = new System.Threading.CancellationTokenSource();
            var ct = _prefetch.Token;
            _mapStatus.Text = "Downloading... (press the button again to stop)";
            Task.Run(async () =>
            {
                int failed = await AppState.Map.PrefetchAsync(tiles,
                    (done, total) => RunOnUiThread(() => _mapStatus.Text = string.Format(Inv, "Downloading {0} / {1}", done, total)), ct);
                long bytes = AppState.Map.DiskBytes();
                RunOnUiThread(() =>
                {
                    _prefetch = null;
                    _mapStatus.Text = ct.IsCancellationRequested ? "Download stopped"
                        : failed == 0 ? string.Format(Inv, "Map ready. Saved on the tablet: {0:0.0} MB", bytes / 1048576.0)
                        : string.Format(Inv, "{0} squares failed (no internet?). Try again later.", failed);
                });
            });
        }

        private void ClearMapCache()
        {
            new AlertDialog.Builder(this)
                .SetTitle("Delete the saved map?")
                .SetMessage("It is downloaded again when needed. Do this only when you want newer imagery.")
                .SetPositiveButton("Delete", (s, e) =>
                {
                    AppState.Map.ClearDisk();
                    _mapStatus.Text = "Saved on the tablet: 0.0 MB";
                })
                .SetNegativeButton("Cancel", (s, e) => { })
                .Show();
        }

        // ---------------------------------------------------------------- layout

        private int Dp(float v) => Ui.Dp(this, v);

        private View BuildLayout()
        {
            var root = new LinearLayout(this) { Orientation = Orientation.Vertical };
            root.SetBackgroundColor(Color.ParseColor("#F2F2F2"));

            // Title bar.
            var bar = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            bar.SetBackgroundColor(Color.ParseColor("#37474F"));
            bar.SetPadding(Dp(8), Dp(4), Dp(8), Dp(4));
            bar.SetGravity(GravityFlags.CenterVertical);
            bar.AddView(Ui.Button(this, "< Back", Finish, 48));
            TextView title = Ui.Text(this, 22, true, "#FFFFFF");
            title.Text = "  Settings";
            bar.AddView(title);
            root.AddView(bar);

            var columns = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            columns.SetPadding(Dp(12), Dp(12), Dp(12), Dp(12));

            // Left column: receiver, implement.
            LinearLayout left = Ui.Card(this);
            left.AddView(Ui.Heading(this, "Receiver (Bluetooth)"));
            _devices = new Spinner(this);
            left.AddView(_devices);
            var devButtons = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            devButtons.AddView(Ui.Button(this, "Refresh", LoadPairedDevices), Ui.Weight(1));
            devButtons.AddView(Ui.Button(this, "Pair...", () => StartActivity(new Intent(global::Android.Provider.Settings.ActionBluetoothSettings))), Ui.Weight(1));
            left.AddView(devButtons);
            _autoConnect = new CheckBox(this) { Text = "Connect automatically when the app starts" };
            left.AddView(_autoConnect);

            left.AddView(Ui.Heading(this, "Implement"));
            _width = Ui.Field(this, left, "Implement width (m)", InputTypes.ClassNumber | InputTypes.NumberFlagDecimal);
            left.AddView(Ui.Hint(this, "The real full width of the implement. The worked area always uses the full width."));
            _overlap = Ui.Field(this, left, "Overlap (m)", InputTypes.ClassNumber | InputTypes.NumberFlagDecimal);
            left.AddView(Ui.Hint(this, "Only for the spacing between passes: passes are implement width minus overlap apart."));
            _workingWidth = Ui.Hint(this, "");
            left.AddView(_workingWidth);
            _width.TextChanged += (s, e) => UpdateWorkingWidth();
            _overlap.TextChanged += (s, e) => UpdateWorkingWidth();
            left.AddView(Ui.Hint(this, "Guidance is for the antenna position."));

            left.AddView(Ui.Heading(this, "Vehicle"));
            left.AddView(Ui.Hint(this, "Metres. Measure from the middle of the rear axle. Left / behind = negative."));
            InputTypes signed = InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned;
            _antHeight = Ui.Field(this, left, "Antenna height above ground (corrects tilt on slopes; needs roll from the receiver)", signed);
            _antForward = Ui.Field(this, left, "Antenna ahead of the rear axle", signed);
            _antRight = Ui.Field(this, left, "Antenna right of the centre line", signed);
            _toolBehind = Ui.Field(this, left, "Implement working line behind the rear axle (front mounted = negative)", signed);
            _toolRight = Ui.Field(this, left, "Implement centre right of the tractor centre line", signed);
            _wheelbase = Ui.Field(this, left, "Wheelbase (simulator and steering)", InputTypes.ClassNumber | InputTypes.NumberFlagDecimal);
            left.AddView(Ui.Hint(this, "Guidance follows the implement's centre line; the worked area is painted where the implement works."));
            _nudgeStep = Ui.Field(this, left, "Nudge step (cm)", InputTypes.ClassNumber | InputTypes.NumberFlagDecimal);

            left.AddView(Ui.Heading(this, "Worked area"));
            _recordCoverage = new CheckBox(this) { Text = "Record worked area (Work button, statistics)" };
            left.AddView(_recordCoverage);
            _showCoverage = new CheckBox(this) { Text = "Show worked area on the map" };
            left.AddView(_showCoverage);
            _recordCoverage.CheckedChange += (s, e) => _showCoverage.Enabled = e.IsChecked;
            left.AddView(Ui.Hint(this, "Recording uses very little; on a slow tablet hide it from the map first."));

            left.AddView(Ui.Heading(this, "Diagnostics"));
            _status = Ui.Text(this, 14);
            left.AddView(_status);
            _rawLog = new CheckBox(this) { Text = "Show raw NMEA" };
            left.AddView(_rawLog);
            _raw = Ui.Text(this, 12);
            _raw.Typeface = Typeface.Monospace;
            left.AddView(_raw);

            var leftScroll = new ScrollView(this);
            leftScroll.AddView(left);
            columns.AddView(leftScroll, Ui.Weight(1, ViewGroup.LayoutParams.MatchParent));

            // Right column: NTRIP.
            LinearLayout right = Ui.Card(this);
            right.AddView(Ui.Heading(this, "NTRIP corrections"));
            _ntripEnabled = new CheckBox(this) { Text = "Send NTRIP corrections to the receiver over Bluetooth" };
            right.AddView(_ntripEnabled);
            right.AddView(Ui.Hint(this, "Turn off if the Raspberry Pi or receiver gets corrections itself."));
            _host = Ui.Field(this, right, "Caster host", InputTypes.ClassText | InputTypes.TextVariationUri);
            _port = Ui.Field(this, right, "Port", InputTypes.ClassNumber);
            _mount = Ui.Field(this, right, "Mountpoint", InputTypes.ClassText);
            _user = Ui.Field(this, right, "User", InputTypes.ClassText);
            _password = Ui.Field(this, right, "Password", InputTypes.ClassText | InputTypes.TextVariationPassword);
            _gga = Ui.Field(this, right, "Send GGA every (s, 0 = once)", InputTypes.ClassNumber);
            right.AddView(Ui.Hint(this, "Changes apply when you leave this screen; a running connection reconnects."));

            right.AddView(Ui.Heading(this, "Simulator"));
            _useSim = new CheckBox(this) { Text = "Use the simulator instead of the receiver" };
            right.AddView(_useSim);
            right.AddView(Ui.Hint(this, "A virtual tractor with RTK fix, to try everything at home. Speed and steering on the driving screen."));
            _simLat = Ui.Field(this, right, "Start latitude (empty = open field or last position)", InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned);
            _simLon = Ui.Field(this, right, "Start longitude", InputTypes.ClassNumber | InputTypes.NumberFlagDecimal | InputTypes.NumberFlagSigned);

            right.AddView(Ui.Heading(this, "Satellite map"));
            _showMap = new CheckBox(this) { Text = "Show the satellite map" };
            right.AddView(_showMap);
            right.AddView(Ui.Hint(this, "Each map square is downloaded once and kept on the tablet; it works without internet afterwards."));
            var mapButtons = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            mapButtons.AddView(Ui.Button(this, "Download map for the open field", DownloadFieldMap), Ui.Weight(2));
            mapButtons.AddView(Ui.Button(this, "Clear map cache", ClearMapCache), Ui.Weight(1));
            right.AddView(mapButtons);
            _mapStatus = Ui.Hint(this, "");
            right.AddView(_mapStatus);
            _mapUrl = Ui.Field(this, right, "Map source (tile URL with {z} {x} {y})", InputTypes.ClassText | InputTypes.TextVariationUri);
            _mapAttribution = Ui.Field(this, right, "Attribution shown on the map", InputTypes.ClassText);
            _mapMaxZoom = Ui.Field(this, right, "Most detailed zoom to download (10-22, default 19)", InputTypes.ClassNumber);
            right.AddView(Ui.Hint(this, "If close-up imagery is missing or grey in your area, try 18 or 17. Closer than this, the map is enlarged."));
            right.AddView(Ui.Hint(this, "Default: Esri World Imagery, free with attribution. Check the source's terms, especially for commercial use."));

            var rightScroll = new ScrollView(this);
            rightScroll.AddView(right);
            var rp = Ui.Weight(1, ViewGroup.LayoutParams.MatchParent);
            rp.LeftMargin = Dp(12);
            columns.AddView(rightScroll, rp);

            root.AddView(columns, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 1));
            return root;
        }
    }
}

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
        private CheckBox _autoConnect, _ntripEnabled, _rawLog, _recordCoverage, _showCoverage;
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
            if (s.RecordCoverage && !_recordCoverage.Checked) AppState.SetWorking(false); // switched off: stop and save
            s.RecordCoverage = _recordCoverage.Checked;
            s.ShowCoverage = _showCoverage.Checked;
            s.Save();
            AppState.Guidance.PassWidth = s.WorkingWidthMeters;
            AppState.Guidance.ToolWidth = s.ToolWidthMeters;
            AppState.Guidance.CoverageEnabled = s.RecordCoverage;

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

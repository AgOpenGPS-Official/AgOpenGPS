using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Android;
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
    /// Bluetooth GNSS / RTK status screen: pick the paired receiver or Raspberry Pi, connect, and see
    /// the fix. Optionally runs the AgIO NTRIP client and sends the corrections back over the link.
    /// Built in code (no XML layouts) to keep the first version small.
    /// </summary>
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@android:style/Theme.Material.Light.NoActionBar",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
    public class MainActivity : Activity
    {
        private const int PermissionRequest = 1;
        private const int RawLogLines = 40;
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        private AppSettings _settings;
        private LinkSupervisor _supervisor;
        private readonly List<string> _deviceAddresses = new List<string>();
        private readonly ConcurrentQueue<string> _rawLines = new ConcurrentQueue<string>();
        private Handler _ui;
        private bool _refreshing;

        private Spinner _devices;
        private Button _connect;
        private CheckBox _ntripEnabled, _rawLog;
        private EditText _host, _port, _mount, _user, _password, _ggaSeconds;
        private TextView _fixBadge, _position, _details, _linkStatus, _ntripStatus, _counters, _raw;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);

            _settings = new AppSettings(this);
            _ui = new Handler(Looper.MainLooper);
            SetContentView(BuildLayout());
            LoadSettingsIntoFields();

            if (HasBluetoothPermission()) LoadPairedDevices();
            else RequestBluetoothPermission();
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
            ReadFieldsIntoSettings();
            _settings.Save();
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            _supervisor?.Stop();
            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode != PermissionRequest) return;
            if (HasBluetoothPermission()) LoadPairedDevices();
            else Toast.MakeText(this, "Bluetooth permission is needed to talk to the receiver", ToastLength.Long).Show();
        }

        private bool HasBluetoothPermission()
        {
            if (!OperatingSystem.IsAndroidVersionAtLeast(31)) return true; // granted at install before Android 12
            return CheckSelfPermission(Manifest.Permission.BluetoothConnect) == Permission.Granted;
        }

        private void RequestBluetoothPermission()
        {
            if (OperatingSystem.IsAndroidVersionAtLeast(31))
                RequestPermissions(new[] { Manifest.Permission.BluetoothConnect }, PermissionRequest);
        }

        private BluetoothAdapter Adapter => ((BluetoothManager)GetSystemService(BluetoothService))?.Adapter;

        private void LoadPairedDevices()
        {
            _deviceAddresses.Clear();
            var names = new List<string>();
            BluetoothAdapter adapter = Adapter;

            if (adapter == null)
            {
                names.Add("This tablet has no Bluetooth");
            }
            else if (!adapter.IsEnabled)
            {
                names.Add("Bluetooth is off - turn it on and press Refresh");
            }
            else
            {
                foreach (BluetoothDevice d in adapter.BondedDevices.OrderBy(d => d.Name ?? ""))
                {
                    _deviceAddresses.Add(d.Address);
                    names.Add((string.IsNullOrEmpty(d.Name) ? "(no name)" : d.Name) + "  " + d.Address);
                }
                if (names.Count == 0) names.Add("No paired devices - pair the receiver in Bluetooth settings");
            }

            var adapterView = new ArrayAdapter<string>(this, global::Android.Resource.Layout.SimpleSpinnerItem, names);
            adapterView.SetDropDownViewResource(global::Android.Resource.Layout.SimpleSpinnerDropDownItem);
            _devices.Adapter = adapterView;

            int selected = _deviceAddresses.IndexOf(_settings.DeviceAddress);
            if (selected >= 0) _devices.SetSelection(selected);
        }

        private void OnConnectClicked()
        {
            if (_supervisor != null && _supervisor.IsRunning)
            {
                LinkSupervisor s = _supervisor;
                _supervisor = null;
                _connect.Enabled = false;
                System.Threading.Tasks.Task.Run(() => s.Stop()).ContinueWith(_ => RunOnUiThread(() =>
                {
                    _connect.Enabled = true;
                    UpdateConnectButton();
                }));
                return;
            }

            if (!HasBluetoothPermission())
            {
                RequestBluetoothPermission();
                return;
            }

            int index = _devices.SelectedItemPosition;
            if (index < 0 || index >= _deviceAddresses.Count)
            {
                Toast.MakeText(this, "Pick a paired device first", ToastLength.Short).Show();
                return;
            }

            ReadFieldsIntoSettings();
            _settings.DeviceAddress = _deviceAddresses[index];
            _settings.Save();

            if (_settings.NtripEnabled && _settings.BuildNtripProfile() == null)
                Toast.MakeText(this, "NTRIP settings incomplete - connecting without corrections", ToastLength.Long).Show();

            string address = _settings.DeviceAddress;
            BluetoothAdapter adapter = Adapter;
            AppSettings settings = _settings;
            while (_rawLines.TryDequeue(out _)) { }

            _supervisor = new LinkSupervisor(ct => BluetoothLink.Connect(adapter, address, ct), settings.BuildNtripProfile);
            _supervisor.SentenceReceived += line =>
            {
                if (!_settings.ShowRawLog) return;
                _rawLines.Enqueue(line);
                while (_rawLines.Count > RawLogLines) _rawLines.TryDequeue(out _);
            };
            _supervisor.Start();
            UpdateConnectButton();
        }

        private void UpdateConnectButton()
        {
            bool running = _supervisor != null && _supervisor.IsRunning;
            _connect.Text = running ? "Disconnect" : "Connect";
            _devices.Enabled = !running;
        }

        // ---------------------------------------------------------------- status refresh, 4 Hz

        private void Refresh()
        {
            if (!_refreshing) return;
            try { Render(_supervisor?.GetSnapshot()); }
            finally { _ui.PostDelayed(Refresh, 250); }
        }

        private void Render(LinkSnapshot snap)
        {
            if (snap == null)
            {
                SetBadge("Not connected", "#757575");
                _linkStatus.Text = "Bluetooth: not connected";
                _ntripStatus.Text = _settings.NtripEnabled ? "NTRIP: waits for the link" : "NTRIP: off";
                return;
            }

            SessionStats st = snap.Session;
            GnssFix fix = st?.Latest;
            bool fresh = fix != null && st.SecondsSinceFix >= 0 && st.SecondsSinceFix < 3 && snap.State == LinkState.Connected;

            if (!fresh)
            {
                SetBadge(snap.State == LinkState.Connected ? "Waiting for position" : StateText(snap.State), "#757575");
            }
            else
            {
                SetBadge(GnssFix.FixQualityName(fix.FixQuality), QualityColor(fix.FixQuality));
            }

            if (fix != null)
            {
                _position.Text = fix.HasPosition
                    ? string.Format(Inv, "{0:0.0000000}°  {1:0.0000000}°", fix.Latitude, fix.Longitude)
                    : "No position";
                var d = new StringBuilder();
                d.Append("Satellites  ").Append(fix.Satellites >= 0 ? fix.Satellites.ToString(Inv) : "-");
                d.Append("     HDOP  ").Append(Num(fix.Hdop, "0.00"));
                d.Append("     Age  ").Append(Num(fix.AgeSeconds, "0.0")).Append(" s\n");
                d.Append("Altitude  ").Append(Num(fix.Altitude, "0.00")).Append(" m");
                d.Append("     Speed  ").Append(Num(fix.SpeedKmh, "0.0")).Append(" km/h\n");
                d.Append("Heading  ").Append(Num(fix.HeadingTrue, "0.0")).Append("°");
                d.Append("     Dual  ").Append(Num(fix.HeadingDual, "0.0")).Append("°");
                d.Append("     Roll  ").Append(Num(fix.Roll, "0.0")).Append("°\n");
                d.Append("Rate  ").Append(st.RateHz.ToString("0.0", Inv)).Append(" Hz");
                d.Append("     Data  ").Append(fix.Source == FixSource.Pgn ? "AgOpenGPS PGN (bridge)" : "NMEA (receiver)");
                _details.Text = d.ToString();
            }
            else
            {
                _position.Text = "";
                _details.Text = "";
            }

            _linkStatus.Text = "Bluetooth: " + StateText(snap.State) + (string.IsNullOrEmpty(snap.Message) ? "" : " - " + snap.Message)
                + (snap.Connects > 1 ? "  (reconnects: " + (snap.Connects - 1) + ")" : "");

            NtripStatus n = snap.Ntrip;
            _ntripStatus.Text = n == null
                ? (_settings.NtripEnabled ? "NTRIP: not running (check settings)" : "NTRIP: off")
                : "NTRIP: " + n.Headline + "   " + (n.Frames > 0
                    ? string.Format(Inv, "{0} frames, last {1:0.0} s ago, {2} bytes to receiver", n.Frames, n.SecondsSinceLastFrame, snap.NtripBytesSent)
                    : n.Message);
            _ntripStatus.SetTextColor(n == null ? Color.DarkGray : NtripColor(n.Light));

            _counters.Text = st == null ? "" : string.Format(Inv,
                "In {0:N0} B   out {1:N0} B   NMEA ok {2:N0} / bad {3:N0}   PGN {4:N0}   skipped {5:N0} B",
                st.BytesIn, st.BytesOut, st.Sentences, st.BadSentences, st.PgnFrames, st.SkippedBytes);

            _raw.Visibility = _settings.ShowRawLog ? ViewStates.Visible : ViewStates.Gone;
            if (_settings.ShowRawLog) _raw.Text = string.Join("\n", _rawLines.ToArray());
        }

        private static string StateText(LinkState s)
        {
            switch (s)
            {
                case LinkState.Connecting: return "connecting";
                case LinkState.Connected: return "connected";
                case LinkState.WaitingToRetry: return "retrying";
                default: return "stopped";
            }
        }

        private static string QualityColor(int q)
        {
            switch (q)
            {
                case 4: return "#2E7D32"; // RTK fix: green
                case 5: return "#EF6C00"; // RTK float: orange
                case 2: return "#1565C0"; // DGPS: blue
                case 0: return "#C62828"; // no fix: red
                default: return "#6D4C41";
            }
        }

        private static Color NtripColor(NtripLight light)
        {
            switch (light)
            {
                case NtripLight.Ok: return Color.ParseColor("#2E7D32");
                case NtripLight.Bad: return Color.ParseColor("#C62828");
                case NtripLight.Warn: return Color.ParseColor("#EF6C00");
                default: return Color.DarkGray;
            }
        }

        private void SetBadge(string text, string color)
        {
            _fixBadge.Text = text;
            _fixBadge.SetBackgroundColor(Color.ParseColor(color));
        }

        private static string Num(double v, string format) => double.IsNaN(v) ? "-" : v.ToString(format, Inv);

        // ---------------------------------------------------------------- settings <-> fields

        private void LoadSettingsIntoFields()
        {
            _ntripEnabled.Checked = _settings.NtripEnabled;
            _host.Text = _settings.NtripHost;
            _port.Text = _settings.NtripPort;
            _mount.Text = _settings.NtripMount;
            _user.Text = _settings.NtripUser;
            _password.Text = _settings.NtripPassword;
            _ggaSeconds.Text = _settings.NtripGgaSeconds;
            _rawLog.Checked = _settings.ShowRawLog;
        }

        private void ReadFieldsIntoSettings()
        {
            _settings.NtripEnabled = _ntripEnabled.Checked;
            _settings.NtripHost = _host.Text.Trim();
            _settings.NtripPort = _port.Text.Trim();
            _settings.NtripMount = _mount.Text.Trim();
            _settings.NtripUser = _user.Text.Trim();
            _settings.NtripPassword = _password.Text;
            _settings.NtripGgaSeconds = _ggaSeconds.Text.Trim();
            _settings.ShowRawLog = _rawLog.Checked;
        }

        // ---------------------------------------------------------------- layout

        private int Dp(float v) => (int)(v * Resources.DisplayMetrics.Density + 0.5f);

        private View BuildLayout()
        {
            var root = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            root.SetBackgroundColor(Color.ParseColor("#F2F2F2"));
            root.SetPadding(Dp(12), Dp(12), Dp(12), Dp(12));

            // Left: connection and NTRIP settings.
            var left = Column();
            left.AddView(Heading("Receiver (Bluetooth)"));
            _devices = new Spinner(this);
            left.AddView(_devices);

            var buttons = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            _connect = BigButton("Connect", OnConnectClicked);
            buttons.AddView(_connect, new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1));
            buttons.AddView(BigButton("Refresh", LoadPairedDevices), new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1));
            buttons.AddView(BigButton("Pair...", () => StartActivity(new Intent(global::Android.Provider.Settings.ActionBluetoothSettings))),
                new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.WrapContent, 1));
            left.AddView(buttons);

            left.AddView(Heading("NTRIP corrections"));
            _ntripEnabled = new CheckBox(this) { Text = "Send NTRIP corrections to the receiver over Bluetooth" };
            left.AddView(_ntripEnabled);
            left.AddView(Hint("Turn off if the Raspberry Pi or receiver gets corrections itself."));
            _host = Field(left, "Caster host", InputTypes.ClassText | InputTypes.TextVariationUri);
            _port = Field(left, "Port", InputTypes.ClassNumber);
            _mount = Field(left, "Mountpoint", InputTypes.ClassText);
            _user = Field(left, "User", InputTypes.ClassText);
            _password = Field(left, "Password", InputTypes.ClassText | InputTypes.TextVariationPassword);
            _ggaSeconds = Field(left, "Send GGA every (s, 0 = once)", InputTypes.ClassNumber);
            left.AddView(Hint("Changes apply on the next Connect."));

            left.AddView(Heading("Diagnostics"));
            _rawLog = new CheckBox(this) { Text = "Show raw NMEA" };
            _rawLog.CheckedChange += (s, e) => _settings.ShowRawLog = e.IsChecked;
            left.AddView(_rawLog);

            var leftScroll = new ScrollView(this);
            leftScroll.AddView(left);
            root.AddView(leftScroll, new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.MatchParent, 1f));

            // Right: live status.
            var right = Column();
            _fixBadge = new TextView(this) { Gravity = GravityFlags.Center };
            _fixBadge.SetTextColor(Color.White);
            _fixBadge.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 40);
            _fixBadge.SetTypeface(null, TypefaceStyle.Bold);
            _fixBadge.SetPadding(Dp(8), Dp(16), Dp(8), Dp(16));
            right.AddView(_fixBadge);

            _position = Text(26, true);
            right.AddView(_position);
            _details = Text(19, false);
            _details.SetLineSpacing(Dp(4), 1f);
            right.AddView(_details);
            _linkStatus = Text(16, false);
            right.AddView(_linkStatus);
            _ntripStatus = Text(16, false);
            right.AddView(_ntripStatus);
            _counters = Text(13, false);
            _counters.SetTextColor(Color.Gray);
            right.AddView(_counters);
            _raw = Text(12, false);
            _raw.Typeface = Typeface.Monospace;
            right.AddView(_raw);

            var rightScroll = new ScrollView(this);
            rightScroll.AddView(right);
            var rightParams = new LinearLayout.LayoutParams(0, ViewGroup.LayoutParams.MatchParent, 1.4f) { LeftMargin = Dp(12) };
            root.AddView(rightScroll, rightParams);

            return root;
        }

        private LinearLayout Column()
        {
            var c = new LinearLayout(this) { Orientation = Orientation.Vertical };
            c.SetBackgroundColor(Color.White);
            c.SetPadding(Dp(16), Dp(8), Dp(16), Dp(16));
            return c;
        }

        private TextView Heading(string text)
        {
            TextView t = Text(18, true);
            t.Text = text;
            t.SetPadding(0, Dp(16), 0, Dp(4));
            return t;
        }

        private TextView Hint(string text)
        {
            TextView t = Text(13, false);
            t.Text = text;
            t.SetTextColor(Color.Gray);
            return t;
        }

        private TextView Text(float sp, bool bold)
        {
            var t = new TextView(this);
            t.SetTextSize(global::Android.Util.ComplexUnitType.Sp, sp);
            t.SetTextColor(Color.ParseColor("#212121"));
            if (bold) t.SetTypeface(null, TypefaceStyle.Bold);
            t.SetPadding(0, Dp(6), 0, Dp(6));
            return t;
        }

        private Button BigButton(string text, Action onClick)
        {
            var b = new Button(this) { Text = text };
            b.SetMinHeight(Dp(56));
            b.Click += (s, e) => onClick();
            return b;
        }

        private EditText Field(LinearLayout parent, string label, InputTypes type)
        {
            TextView l = Text(13, false);
            l.Text = label;
            l.SetPadding(0, Dp(6), 0, 0);
            parent.AddView(l);
            var e = new EditText(this) { InputType = type };
            e.SetSingleLine(true);
            parent.AddView(e);
            return e;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using AgLibrary.Ntrip;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Driving screen: fix status, the guidance view with A-B passes, distance off the line, and
    /// the buttons to record (A / B), pick lines and open the settings.
    /// </summary>
    [Activity(Label = "@string/app_name", MainLauncher = true, Theme = "@android:style/Theme.Material.Light.NoActionBar",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
    public class MainActivity : Activity
    {
        private const int PermissionRequest = 1;
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        private Handler _ui;
        private bool _refreshing;
        private bool _autoConnectTried;
        private LineKind _kind = LineKind.Straight;

        private TextView _fixBadge, _fixInfo, _xte, _lineInfo, _recordInfo;
        private Button _connect, _straight, _curve, _a, _b, _cancel;
        private GuidanceView _view;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            AppState.Init(this);
            _ui = new Handler(Looper.MainLooper);
            SetContentView(BuildLayout());

            if (!HasBluetoothPermission()) RequestBluetoothPermission();
            else TryAutoConnect();
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
            base.OnPause();
        }

        protected override void OnDestroy()
        {
            // Leaving the app with Back ends the connection; rotating does not recreate this screen.
            if (IsFinishing) Task.Run(AppState.Disconnect);
            base.OnDestroy();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode != PermissionRequest) return;
            if (HasBluetoothPermission()) TryAutoConnect();
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

        private void TryAutoConnect()
        {
            if (_autoConnectTried) return;
            _autoConnectTried = true;
            if (AppState.Settings.AutoConnect && !string.IsNullOrEmpty(AppState.Settings.DeviceAddress)) Connect();
        }

        // ---------------------------------------------------------------- actions

        private void Connect()
        {
            if (!HasBluetoothPermission())
            {
                RequestBluetoothPermission();
                return;
            }
            string message = AppState.Connect();
            if (message != null) Toast.MakeText(this, message, ToastLength.Long).Show();
            if (!AppState.IsConnected && message != null && message.StartsWith("Pick", StringComparison.Ordinal))
                StartActivity(new Intent(this, typeof(SettingsActivity)));
        }

        private void OnConnectClicked()
        {
            if (!AppState.IsConnected)
            {
                Connect();
                return;
            }
            _connect.Enabled = false;
            Task.Run(AppState.Disconnect).ContinueWith(_ => RunOnUiThread(() => _connect.Enabled = true));
        }

        private void OnAClicked()
        {
            if (!AppState.Guidance.SetA(_kind))
            {
                Toast.MakeText(this, "No position yet", ToastLength.Short).Show();
                return;
            }
            Toast.MakeText(this, _kind == LineKind.Curve
                ? "A set. Drive along the edge, then press B"
                : "A set. Drive straight along the edge, then press B", ToastLength.Long).Show();
        }

        private void OnBClicked()
        {
            string name = AppState.Lines.NextName();
            AbLine line = AppState.Guidance.SetB(name, out string error);
            if (line == null)
            {
                Toast.MakeText(this, error, ToastLength.Long).Show();
                return;
            }
            AppState.Lines.Add(line, makeActive: true);
            AppState.Guidance.SetActiveLine(line);
            Toast.MakeText(this, string.Format(Inv, "Saved {0} ({1:0} m). Rename it under Lines.", line.Name, line.LengthMeters),
                ToastLength.Long).Show();
        }

        private void OnCancelClicked()
        {
            AppState.Guidance.CancelAb();
        }

        private void SetKind(LineKind kind)
        {
            _kind = kind;
            _straight.Alpha = kind == LineKind.Straight ? 1f : 0.45f;
            _curve.Alpha = kind == LineKind.Curve ? 1f : 0.45f;
        }

        private void ShowLines()
        {
            IReadOnlyList<AbLine> lines = AppState.Lines.Lines;
            AbLine active = AppState.Lines.Active;
            if (lines.Count == 0)
            {
                Toast.MakeText(this, "No lines yet: press A, drive, press B", ToastLength.Long).Show();
                return;
            }

            var items = new string[lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                AbLine l = lines[i];
                items[i] = string.Format(Inv, "{0}{1}   {2}, {3:0} m, {4:0}°",
                    active != null && l.Id == active.Id ? "▶ " : "", l.Name,
                    l.Kind == LineKind.Curve ? "curve" : "straight", l.LengthMeters, l.HeadingDegrees);
            }

            new AlertDialog.Builder(this)
                .SetTitle("Lines")
                .SetItems(items, (s, e) => ShowLineActions(lines[e.Which]))
                .SetNeutralButton("No line", (s, e) => Activate(null))
                .SetNegativeButton("Close", (s, e) => { })
                .Show();
        }

        private void ShowLineActions(AbLine line)
        {
            new AlertDialog.Builder(this)
                .SetTitle(line.Name)
                .SetItems(new[] { "Use this line", "Rename", "Delete" }, (s, e) =>
                {
                    switch (e.Which)
                    {
                        case 0: Activate(line); break;
                        case 1: Rename(line); break;
                        case 2: ConfirmDelete(line); break;
                    }
                })
                .Show();
        }

        private void Activate(AbLine line)
        {
            AppState.Lines.SetActive(line?.Id);
            AppState.Guidance.SetActiveLine(line == null ? null : AppState.Lines.Active);
        }

        private void Rename(AbLine line)
        {
            var input = new EditText(this) { Text = line.Name };
            input.SetSingleLine(true);
            input.SetSelectAllOnFocus(true);
            var frame = new FrameLayout(this);
            frame.SetPadding(Ui.Dp(this, 20), Ui.Dp(this, 8), Ui.Dp(this, 20), 0);
            frame.AddView(input);

            new AlertDialog.Builder(this)
                .SetTitle("Rename line")
                .SetView(frame)
                .SetPositiveButton("OK", (s, e) =>
                {
                    AppState.Lines.Rename(line.Id, input.Text);
                    if (AppState.Guidance.ActiveLine?.Id == line.Id) AppState.Guidance.SetActiveLine(AppState.Lines.Active);
                })
                .SetNegativeButton("Cancel", (s, e) => { })
                .Show();
        }

        private void ConfirmDelete(AbLine line)
        {
            new AlertDialog.Builder(this)
                .SetTitle("Delete " + line.Name + "?")
                .SetPositiveButton("Delete", (s, e) =>
                {
                    bool wasActive = AppState.Guidance.ActiveLine?.Id == line.Id;
                    AppState.Lines.Delete(line.Id);
                    if (wasActive) AppState.Guidance.SetActiveLine(null);
                })
                .SetNegativeButton("Cancel", (s, e) => { })
                .Show();
        }

        // ---------------------------------------------------------------- refresh, 10 Hz

        private void Refresh()
        {
            if (!_refreshing) return;
            try { Render(); }
            finally { _ui.PostDelayed(Refresh, 100); }
        }

        private void Render()
        {
            LinkSnapshot link = AppState.Link?.GetSnapshot();
            GuidanceSnapshot g = AppState.Guidance.GetSnapshot();
            _view.Show(g);

            _connect.Text = AppState.IsConnected ? "Disconnect" : "Connect";

            // Fix badge and info line.
            SessionStats st = link?.Session;
            GnssFix fix = st?.Latest;
            bool fresh = link != null && link.State == LinkState.Connected && fix != null && st.SecondsSinceFix >= 0 && st.SecondsSinceFix < 3;
            if (link == null) SetBadge("Not connected", "#757575");
            else if (link.State != LinkState.Connected) SetBadge(link.State == LinkState.Connecting ? "Connecting" : "Reconnecting", "#757575");
            else if (!fresh) SetBadge("No position", "#757575");
            else SetBadge(GnssFix.FixQualityName(fix.FixQuality), Ui.FixColor(fix.FixQuality));

            if (fresh)
            {
                _fixInfo.Text = string.Format(Inv, "{0} sats   HDOP {1}   {2:0.0} Hz   {3} km/h   {4}",
                    fix.Satellites, Num(fix.Hdop, "0.0"), st.RateHz, Num(fix.SpeedKmh, "0.0"), NtripText(link));
            }
            else
            {
                _fixInfo.Text = link == null ? "Press Connect" : link.Message;
            }

            // Distance off the line.
            AbGuidanceResult r = g.Result;
            if (r != null && fresh)
            {
                double cm = r.CrossTrackMeters * 100.0;
                double abs = Math.Abs(cm);
                string amount = abs < 100 ? abs.ToString("0", Inv) + " cm" : (abs / 100).ToString("0.00", Inv) + " m";
                // Positive = right of the line: steer left.
                _xte.Text = abs < 2 ? "▲ on line" : cm > 0 ? "◀ " + amount : amount + " ▶";
                _xte.SetTextColor(Color.ParseColor(abs < 5 ? "#66BB6A" : abs < 15 ? "#FFCA28" : "#EF5350"));
                _lineInfo.Text = string.Format(Inv, "{0}   pass {1}   {2:0.0}°{3}",
                    g.ActiveLine.Name, r.Pass, r.TravelLineHeading,
                    double.IsNaN(r.HeadingErrorDegrees) ? "" : string.Format(Inv, "   heading err {0:+0.0;-0.0}°", r.HeadingErrorDegrees));
            }
            else
            {
                _xte.Text = "";
                _lineInfo.Text = g.ActiveLine != null ? g.ActiveLine.Name : "No line selected";
            }

            // Recording state.
            _cancel.Visibility = g.Recording ? ViewStates.Visible : ViewStates.Gone;
            _b.Enabled = g.Recording;
            _a.Text = g.Recording ? "Restart A" : "A";
            _recordInfo.Text = g.Recording
                ? string.Format(Inv, "Recording {0}: {1:0} m from A", g.RecordingKind == LineKind.Curve ? "curve" : "line",
                    double.IsNaN(g.RecordedMeters) ? 0 : g.RecordedMeters)
                : "";
        }

        private static string NtripText(LinkSnapshot link)
        {
            NtripStatus n = link.Ntrip;
            if (n == null) return "";
            return n.Frames > 0 && n.SecondsSinceLastFrame >= 0 && n.SecondsSinceLastFrame < 10 ? "NTRIP ok" : "NTRIP: " + n.Headline;
        }

        private void SetBadge(string text, string color)
        {
            _fixBadge.Text = text;
            _fixBadge.SetBackgroundColor(Color.ParseColor(color));
        }

        private static string Num(double v, string format) => double.IsNaN(v) ? "-" : v.ToString(format, Inv);

        // ---------------------------------------------------------------- layout

        private int Dp(float v) => Ui.Dp(this, v);

        private View BuildLayout()
        {
            var root = new LinearLayout(this) { Orientation = Orientation.Vertical };
            root.SetBackgroundColor(Color.ParseColor("#37474F"));

            // Top bar: fix, info, connect, settings.
            var bar = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            bar.SetGravity(GravityFlags.CenterVertical);
            bar.SetPadding(Dp(8), Dp(4), Dp(8), Dp(4));
            _fixBadge = Ui.Text(this, 22, true, "#FFFFFF");
            _fixBadge.Gravity = GravityFlags.Center;
            _fixBadge.SetPadding(Dp(16), Dp(8), Dp(16), Dp(8));
            _fixBadge.SetMinWidth(Dp(170));
            bar.AddView(_fixBadge);
            _fixInfo = Ui.Text(this, 16, false, "#ECEFF1");
            _fixInfo.SetPadding(Dp(12), 0, Dp(12), 0);
            _fixInfo.SetSingleLine(true);
            _fixInfo.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
            bar.AddView(_fixInfo, Ui.Weight(1));
            _connect = Ui.Button(this, "Connect", OnConnectClicked, 48);
            bar.AddView(_connect);
            bar.AddView(Ui.Button(this, "Settings", () => StartActivity(new Intent(this, typeof(SettingsActivity))), 48));
            root.AddView(bar);

            var body = new LinearLayout(this) { Orientation = Orientation.Horizontal };

            // Guidance view with the distance overlay.
            var frame = new FrameLayout(this);
            _view = new GuidanceView(this);
            frame.AddView(_view);
            var overlay = new LinearLayout(this) { Orientation = Orientation.Vertical };
            overlay.SetGravity(GravityFlags.CenterHorizontal);
            _xte = Ui.Text(this, 48, true, "#FFFFFF");
            _xte.Gravity = GravityFlags.Center;
            _xte.SetShadowLayer(6, 0, 0, Color.Black);
            overlay.AddView(_xte);
            _lineInfo = Ui.Text(this, 16, false, "#ECEFF1");
            _lineInfo.Gravity = GravityFlags.Center;
            _lineInfo.SetShadowLayer(4, 0, 0, Color.Black);
            overlay.AddView(_lineInfo);
            frame.AddView(overlay, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Top));
            body.AddView(frame, Ui.Weight(1, ViewGroup.LayoutParams.MatchParent));

            // Right panel: line type, A, B, cancel, lines.
            var panel = new LinearLayout(this) { Orientation = Orientation.Vertical };
            panel.SetPadding(Dp(8), Dp(8), Dp(8), Dp(8));
            TextView typeLabel = Ui.Text(this, 14, false, "#B0BEC5");
            typeLabel.Text = "New line";
            panel.AddView(typeLabel);
            var kinds = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            _straight = Ui.Button(this, "Straight", () => SetKind(LineKind.Straight), 48);
            _curve = Ui.Button(this, "Curve", () => SetKind(LineKind.Curve), 48);
            kinds.AddView(_straight, Ui.Weight(1));
            kinds.AddView(_curve, Ui.Weight(1));
            panel.AddView(kinds);

            var ab = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            _a = Ui.Button(this, "A", OnAClicked, 88);
            _b = Ui.Button(this, "B", OnBClicked, 88);
            _a.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 28);
            _b.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 28);
            ab.AddView(_a, Ui.Weight(1));
            ab.AddView(_b, Ui.Weight(1));
            panel.AddView(ab);
            _recordInfo = Ui.Text(this, 14, false, "#FFAB91");
            panel.AddView(_recordInfo);
            _cancel = Ui.Button(this, "Cancel recording", OnCancelClicked, 48);
            panel.AddView(_cancel);

            var spacer = new View(this);
            panel.AddView(spacer, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 1));
            panel.AddView(Ui.Button(this, "Lines...", ShowLines, 64));

            body.AddView(panel, new LinearLayout.LayoutParams(Dp(240), ViewGroup.LayoutParams.MatchParent));
            root.AddView(body, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 1));

            SetKind(LineKind.Straight);
            return root;
        }
    }
}

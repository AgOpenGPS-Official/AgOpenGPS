using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    /// Driving screen: fix status, the guidance view with passes and field boundary, distance off
    /// the line, edge warning, and buttons for fields, boundary recording, A / B and lines.
    /// </summary>
    [Activity(Label = "@string/app_name", MainLauncher = true, LaunchMode = LaunchMode.SingleTask,
        Theme = "@android:style/Theme.Material.Light.NoActionBar",
        ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize | ConfigChanges.ScreenLayout
            | ConfigChanges.SmallestScreenSize | ConfigChanges.KeyboardHidden | ConfigChanges.Keyboard)]
    public class MainActivity : Activity
    {
        private const int PermissionRequest = 1;
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        private Handler _ui;
        private int _builtLanguage, _builtPalette;
        private DateTime _shownAlertUtc;
        private bool _refreshing;
        private bool _autoConnectTried;
        private LineKind _kind = LineKind.Straight;

        private TextView _workLabel;
        private TextView _fixBadge, _fixInfo, _xte, _lineInfo, _edge, _accuracy, _recordInfo, _stats;
        private LightbarView _lightbar;
        private LinearLayout _simPanel, _cornerRow;
        private Button _followButton;
        private TextView _simSpeed;
        private Button _simAuto;
        private Button _connect, _work, _field, _boundary, _cancelBoundary, _straight, _curve, _a, _b, _cancel;
        private GuidanceView _view;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Window.AddFlags(WindowManagerFlags.KeepScreenOn);
            AppState.Init(this);
            _ui = new Handler(Looper.MainLooper);
            SetContentView(BuildLayout());
            _builtLanguage = Tr.Version;
            _builtPalette = Palette.Version;

            if (!HasBluetoothPermission()) RequestPermissionsNeeded();
            else
            {
                RequestPermissionsNeeded(); // notifications, if not granted yet
                TryAutoConnect();
            }
        }

        protected override void OnResume()
        {
            base.OnResume();
            if (_builtLanguage != Tr.Version || _builtPalette != Palette.Version)
            {
                Recreate(); // the language was changed in Settings: rebuild with the new texts
                return;
            }
            _refreshing = true;
            _ui.Post(Refresh);
        }

        protected override void OnPause()
        {
            _refreshing = false;
            base.OnPause();
        }

        // The connection is not stopped when this screen closes: it keeps running in the
        // background (GnssService) until Disconnect, here or in the notification.

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
            if (requestCode != PermissionRequest) return;
            if (HasBluetoothPermission()) TryAutoConnect();
            else Toast.MakeText(this, Tr.T("Bluetooth permission is needed to talk to the receiver"), ToastLength.Long).Show();
        }

        private bool HasBluetoothPermission()
        {
            if (!OperatingSystem.IsAndroidVersionAtLeast(31)) return true; // granted at install before Android 12
            return CheckSelfPermission(Manifest.Permission.BluetoothConnect) == Permission.Granted;
        }

        private void RequestPermissionsNeeded()
        {
            var wanted = new List<string>();
            if (OperatingSystem.IsAndroidVersionAtLeast(31) && CheckSelfPermission(Manifest.Permission.BluetoothConnect) != Permission.Granted)
                wanted.Add(Manifest.Permission.BluetoothConnect);
            if (OperatingSystem.IsAndroidVersionAtLeast(33) && CheckSelfPermission(Manifest.Permission.PostNotifications) != Permission.Granted)
                wanted.Add(Manifest.Permission.PostNotifications);
            if (wanted.Count > 0) RequestPermissions(wanted.ToArray(), PermissionRequest);
        }

        private void TryAutoConnect()
        {
            if (_autoConnectTried) return;
            _autoConnectTried = true;
            AppSettings s = AppState.Settings;
            if (s.AutoConnect && (s.UseSimulator || !string.IsNullOrEmpty(s.DeviceAddress))) Connect();
        }

        /// <summary>Shows a message, translated (also messages from the core library).</summary>
        private void Toast_(string text, bool longer = false)
            => Toast.MakeText(this, Tr.T(text), longer ? ToastLength.Long : ToastLength.Short).Show();

        // ---------------------------------------------------------------- connection

        private void Connect()
        {
            if (!HasBluetoothPermission())
            {
                RequestPermissionsNeeded();
                return;
            }
            string message = AppState.Connect();
            if (message != null) Toast_(message, true);
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

        // ---------------------------------------------------------------- fields

        private void ShowFields()
        {
            IReadOnlyList<Field> fields = AppState.Fields.Fields;
            Field open = AppState.Fields.Active;
            // The fields, then one more entry to import outlines from a file.
            // The fields, then: export all, import.
            var items = new string[fields.Count + 2];
            for (int i = 0; i < fields.Count; i++)
            {
                Field f = fields[i];
                items[i] = Tr.F("{0}{1}   {2}, {3} lines", open != null && f.Id == open.Id ? "▶ " : "", f.Name,
                    f.HasBoundary ? f.AreaHectares.ToString("0.00", Inv) + " ha" : Tr.T("no boundary"),
                    AppState.Lines.LinesFor(f.Id).Count);
            }
            items[fields.Count] = Tr.T("Export all fields to a file...");
            items[fields.Count + 1] = Tr.T("Import from file (GeoJSON / KML)...");

            var b = new AlertDialog.Builder(this)
                .SetTitle(fields.Count == 0 ? Tr.T("No fields yet") : Tr.T("Fields"))
                .SetPositiveButton(Tr.T("New field"), (s, e) => NewField(null))
                .SetNegativeButton(Tr.T("Close"), (s, e) => { })
                .SetItems(items, (s, e) =>
                {
                    if (e.Which < fields.Count) ShowFieldActions(fields[e.Which]);
                    else if (e.Which == fields.Count) ExportFields(fields, Tr.T("AgOpenGPS fields"));
                    else PickImportFile();
                });
            if (open != null) b.SetNeutralButton(Tr.T("Close field"), (s, e) => AppState.OpenField(null));
            b.Show();
        }

        private void ShowFieldActions(Field field)
        {
            new AlertDialog.Builder(this)
                .SetTitle(field.Name)
                .SetItems(new[] { Tr.T("Open"), Tr.T("Rename"), Tr.T("Clear worked area"), Tr.T("Export to a file..."), Tr.T("Delete") }, (s, e) =>
                {
                    switch (e.Which)
                    {
                        case 0: AppState.OpenField(field); break;
                        case 1:
                            AskText(Tr.T("Rename field"), field.Name, name =>
                            {
                                AppState.Fields.Rename(field.Id, name);
                                AppState.RefreshField();
                            });
                            break;
                        case 2:
                            if (AppState.Fields.Active?.Id != field.Id)
                            {
                                Toast_(Tr.T("Open the field first"));
                                break;
                            }
                            Confirm(Tr.F("Clear the worked area of {0}?", field.Name), Tr.T("Use this to start the next job in this field."),
                                Tr.T("Clear"), AppState.ClearCoverage);
                            break;
                        case 3:
                            ExportFields(new[] { field }, field.Name);
                            break;
                        case 4:
                            Confirm(Tr.F("Delete {0}?", field.Name), Tr.F("Its boundary and its {0} lines are deleted too.",
                                AppState.Lines.LinesFor(field.Id).Count), Tr.T("Delete"), () => AppState.DeleteField(field));
                            break;
                    }
                })
                .Show();
        }

        private const int ImportRequest = 2;

        /// <summary>Asks Android where to save (tablet, Downloads, USB stick, Drive...), then writes the GeoJSON.</summary>
        private void ExportFields(IReadOnlyList<Field> fields, string name)
        {
            if (fields.Count == 0)
            {
                Toast_(Tr.T("No fields to export"));
                return;
            }
            string json = FieldExport.ToGeoJson(fields, id => AppState.Lines.LinesFor(id));
            FileSaver.Save(this, FieldExport.FileName(name), "application/geo+json", new System.Text.UTF8Encoding(false).GetBytes(json),
                Tr.T("Fields saved. Import the file on any tablet to restore them."));
        }

        /// <summary>Opens Android's file picker for a field outline.</summary>
        private void PickImportFile()
        {
            var intent = new Intent(Intent.ActionOpenDocument);
            intent.AddCategory(Intent.CategoryOpenable);
            intent.SetType("*/*"); // .geojson and .kml often have no proper MIME type
            StartActivityForResult(intent, ImportRequest);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (FileSaver.HandleResult(this, requestCode, resultCode, data)) return;
            if (requestCode != ImportRequest || resultCode != Result.Ok || data?.Data == null) return;

            string text, fileName = Tr.T("Imported field");
            try
            {
                using (var stream = ContentResolver.OpenInputStream(data.Data))
                using (var reader = new System.IO.StreamReader(stream))
                    text = reader.ReadToEnd();
                string last = data.Data.LastPathSegment ?? "";
                int slash = last.LastIndexOfAny(new[] { '/', ':' });
                if (slash >= 0) last = last.Substring(slash + 1);
                int dot = last.LastIndexOf('.');
                if (dot > 0) last = last.Substring(0, dot);
                if (!string.IsNullOrWhiteSpace(last)) fileName = last;
            }
            catch (Exception ex)
            {
                Toast_(Tr.F("Could not read the file: {0}", ex.Message), true);
                return;
            }

            List<ImportedField> imported;
            try { imported = FieldImport.Parse(text, fileName); }
            catch (FormatException ex)
            {
                Toast_(ex.Message, true);
                return;
            }

            Field first = null;
            (double hectares, int lineCount, Field firstField) = AppState.ImportFields(imported);
            first = firstField;
            _ui.PostDelayed(() => _view.ShowWholeField(), 300); // after the next refresh has the new field
            Toast_(Tr.F("Imported fields: {0} ({1:0.00} ha), lines: {2}. '{3}' is open.",
                imported.Count, hectares, lineCount, first.Name), true);
        }

        /// <summary>Asks for a name, creates and opens the field, then runs <paramref name="then"/>.</summary>
        private void NewField(Action then)
        {
            AskText(Tr.T("New field"), AppState.Fields.NextName(), name =>
            {
                Field f = AppState.Fields.Create(name);
                AppState.OpenField(f);
                if (then != null) then();
                else Toast_(Tr.F("Field '{0}' created. Record its boundary or an A-B line.", f.Name), true);
            });
        }

        // ---------------------------------------------------------------- jobs

        private void ShowJobs()
        {
            IReadOnlyList<Job> jobs = AppState.Jobs.Jobs;
            Job current = AppState.Jobs.Current;
            var items = new string[jobs.Count];
            for (int i = 0; i < jobs.Count; i++)
            {
                Job j = jobs[i];
                items[i] = Tr.F("{0}{1}  {2}   {3:0.00} ha   {4} h:mm   {5} km/h",
                    current != null && j.Id == current.Id ? "▶ " : "", j.StartUtc.ToLocalTime().ToString("yyyy-MM-dd HH:mm", Inv), j.FieldName,
                    j.Hectares, JobLog.Duration(j.WorkingSeconds),
                    double.IsNaN(j.AverageSpeedKmh) ? "-" : j.AverageSpeedKmh.ToString("0.0", Inv));
            }

            var b = new AlertDialog.Builder(this)
                .SetTitle(jobs.Count == 0 ? Tr.T("No jobs yet: a job starts with Work: ON in a field") : Tr.T("Jobs"))
                .SetNegativeButton(Tr.T("Close"), (s, e) => { });
            if (jobs.Count > 0)
            {
                b.SetItems(items, (s, e) => ConfirmDeleteJob(jobs[e.Which]));
                b.SetPositiveButton(Tr.T("Export..."), (s, e) => ChooseJobExport());
            }
            if (current != null) b.SetNeutralButton(Tr.T("End job"), (s, e) => AppState.Jobs.EndJob());
            b.Show();
        }

        private void ConfirmDeleteJob(Job job)
        {
            Confirm(Tr.F("Delete the job of {0} ({1})?", job.FieldName, job.StartUtc.ToLocalTime().ToString("yyyy-MM-dd", Inv)), null,
                Tr.T("Delete"), () => AppState.Jobs.Delete(job.Id));
        }

        private void ChooseJobExport()
        {
            AppState.Jobs.Save();
            string day = DateTime.Now.ToString("yyyy-MM-dd", Inv);
            new AlertDialog.Builder(this)
                .SetTitle(Tr.T("Export the job log"))
                .SetItems(new[] { Tr.T("CSV (Excel)"), Tr.T("PDF (print)") }, (s, e) =>
                {
                    if (e.Which == 0)
                    {
                        // UTF-8 with BOM so Excel shows č, ć, š, ž, đ correctly.
                        byte[] csv = new System.Text.UTF8Encoding(true).GetPreamble()
                            .Concat(System.Text.Encoding.UTF8.GetBytes(JobLog.ToCsv(AppState.Jobs.Jobs, Tr.T))).ToArray();
                        FileSaver.Save(this, Tr.T("Job log") + " " + day + ".csv", "text/csv", csv, Tr.T("Job log saved"));
                    }
                    else
                    {
                        FileSaver.Save(this, Tr.T("Job log") + " " + day + ".pdf", "application/pdf", JobReport.Pdf(AppState.Jobs.Jobs), Tr.T("Job log saved"));
                    }
                })
                .Show();
        }

        // ---------------------------------------------------------------- boundary

        private void OnBoundaryClicked()
        {
            if (AppState.Guidance.IsRecordingBoundary)
            {
                FinishBoundary();
                return;
            }
            Field field = AppState.Fields.Active;
            if (field == null)
            {
                NewField(OnBoundaryClicked);
                return;
            }
            if (field.HasBoundary)
            {
                Confirm(Tr.F("Replace the boundary of {0}?", field.Name), null, Tr.T("Record new"), ChooseBoundaryMode);
                return;
            }
            ChooseBoundaryMode();
        }

        /// <summary>First: drive the whole edge, or mark only the corners.</summary>
        private void ChooseBoundaryMode()
        {
            new AlertDialog.Builder(this)
                .SetTitle(Tr.T("How to record the boundary?"))
                .SetItems(new[] { Tr.T("Drive along the edge"), Tr.T("Corners (straight sides)") },
                    (s, e) => ChooseBoundarySide(e.Which == 1 ? BoundaryMode.Corners : BoundaryMode.Drive))
                .Show();
        }

        private void ChooseBoundarySide(BoundaryMode mode)
        {
            double half = AppState.Settings.WorkingWidthMeters / 2;
            string[] items =
            {
                Tr.T("Antenna path"),
                string.Format(Inv, Tr.T("Left end of implement ({0:0.0} m left)"), half),
                string.Format(Inv, Tr.T("Right end of implement ({0:0.0} m right)"), half),
            };
            new AlertDialog.Builder(this)
                .SetTitle(Tr.T("What follows the field edge?"))
                .SetItems(items, (s, e) =>
                {
                    var side = (BoundarySide)e.Which;
                    if (!AppState.Guidance.StartBoundary(side, mode))
                    {
                        Toast_(Tr.T("No position yet"));
                        return;
                    }
                    Toast_(mode == BoundaryMode.Corners
                        ? Tr.T("Drive to the first corner and press Corner. After the last corner press Finish.")
                        : Tr.T("Drive round the field with the implement up, then press Finish near where you started"), true);
                })
                .Show();
        }

        private void OnCornerClicked()
        {
            int n = AppState.Guidance.AddCorner();
            if (n < 0) Toast_(Tr.T("Not added: no position, or the same spot as the last corner"));
            else Toast_(Tr.F("Corner {0}", n));
        }

        private void FinishBoundary()
        {
            Field field = AppState.Fields.Active;
            List<TrackPoint> boundary = AppState.Guidance.FinishBoundary(AppState.Settings.WorkingWidthMeters / 2, out double area, out string error);
            if (boundary == null)
            {
                Toast_(error, true);
                return;
            }
            if (field == null)
            {
                Toast_(Tr.T("The field was closed while recording"), true);
                return;
            }
            AppState.Fields.SetBoundary(field.Id, boundary);
            AppState.RefreshField();
            Toast_(string.Format(Inv, Tr.T("Boundary of {0} saved: {1:0.00} ha"), field.Name, area / 10000), true);
        }

        // ---------------------------------------------------------------- lines

        private void OnAClicked()
        {
            if (!AppState.Guidance.SetA(_kind))
            {
                Toast_(Tr.T("No position yet"));
                return;
            }
            Toast_(_kind == LineKind.Curve
                ? Tr.T("A set. Drive along the edge, then press B")
                : Tr.T("A set. Drive straight along the edge, then press B"), true);
        }

        private void OnBClicked()
        {
            string fieldId = AppState.Fields.Active?.Id;
            AbLine line = AppState.Guidance.SetB(AppState.Lines.NextName(fieldId), out string error);
            if (line == null)
            {
                Toast_(error, true);
                return;
            }
            line.FieldId = fieldId;
            AppState.Lines.Add(line, makeActive: true);
            AppState.UseLine(line);
            Toast_(string.Format(Inv, Tr.T("Saved {0} ({1:0} m). Rename it under Lines."), line.Name, line.LengthMeters), true);
        }

        private void SetKind(LineKind kind)
        {
            _kind = kind;
            _straight.Alpha = kind == LineKind.Straight ? 1f : 0.45f;
            _curve.Alpha = kind == LineKind.Curve ? 1f : 0.45f;
        }

        private void ShowLines()
        {
            Field field = AppState.Fields.Active;
            IReadOnlyList<AbLine> lines = AppState.Lines.LinesFor(field?.Id);
            AbLine active = AppState.Guidance.ActiveLine;
            string title = field == null ? Tr.T("Lines without a field") : Tr.F("Lines in {0}", field.Name);
            if (lines.Count == 0)
            {
                Toast_(field == null ? Tr.T("No lines without a field yet: press A, drive, press B")
                    : Tr.F("No lines in {0} yet: press A, drive, press B", field.Name), true);
                return;
            }

            var items = new string[lines.Count];
            for (int i = 0; i < lines.Count; i++)
            {
                AbLine l = lines[i];
                items[i] = string.Format(Inv, "{0}{1}   {2}, {3:0} m, {4:0}°",
                    active != null && l.Id == active.Id ? "▶ " : "", l.Name,
                    l.Kind == LineKind.Curve ? Tr.T("curve") : Tr.T("straight"), l.LengthMeters, l.HeadingDegrees);
            }

            new AlertDialog.Builder(this)
                .SetTitle(title)
                .SetItems(items, (s, e) => ShowLineActions(lines[e.Which]))
                .SetNeutralButton(Tr.T("No line"), (s, e) => AppState.UseLine(null))
                .SetNegativeButton(Tr.T("Close"), (s, e) => { })
                .Show();
        }

        private void ShowLineActions(AbLine line)
        {
            new AlertDialog.Builder(this)
                .SetTitle(line.Name)
                .SetItems(new[] { Tr.T("Use this line"), Tr.T("Rename"), Tr.T("Delete") }, (s, e) =>
                {
                    switch (e.Which)
                    {
                        case 0: AppState.UseLine(line); break;
                        case 1:
                            AskText(Tr.T("Rename line"), line.Name, name =>
                            {
                                AppState.Lines.Rename(line.Id, name);
                                if (AppState.Guidance.ActiveLine?.Id == line.Id) AppState.UseLine(AppState.Lines.Active);
                            });
                            break;
                        case 2:
                            Confirm(Tr.F("Delete {0}?", line.Name), null, Tr.T("Delete"), () =>
                            {
                                bool wasActive = AppState.Guidance.ActiveLine?.Id == line.Id;
                                AppState.Lines.Delete(line.Id);
                                if (wasActive) AppState.UseLine(null);
                            });
                            break;
                    }
                })
                .Show();
        }

        // ---------------------------------------------------------------- dialogs

        private void AskText(string title, string initial, Action<string> ok)
        {
            var input = new EditText(this) { Text = initial };
            input.SetSingleLine(true);
            input.SetSelectAllOnFocus(true);
            var frame = new FrameLayout(this);
            frame.SetPadding(Ui.Dp(this, 20), Ui.Dp(this, 8), Ui.Dp(this, 20), 0);
            frame.AddView(input);

            new AlertDialog.Builder(this)
                .SetTitle(title)
                .SetView(frame)
                .SetPositiveButton(Tr.T("OK"), (s, e) =>
                {
                    string text = input.Text?.Trim();
                    ok(string.IsNullOrEmpty(text) ? initial : text);
                })
                .SetNegativeButton(Tr.T("Cancel"), (s, e) => { })
                .Show();
        }

        private void Confirm(string title, string message, string yes, Action action)
        {
            var b = new AlertDialog.Builder(this)
                .SetTitle(title)
                .SetPositiveButton(yes, (s, e) => action())
                .SetNegativeButton(Tr.T("Cancel"), (s, e) => { });
            if (message != null) b.SetMessage(message);
            b.Show();
        }

        // ---------------------------------------------------------------- refresh, 10 Hz

        private void Refresh()
        {
            if (!_refreshing) return;
            try { Render(); }
            finally { _ui.PostDelayed(Refresh, 100); }
        }

        /// <summary>"Worked 4.32 ha / Left 8.08 ha (35 %) / Overlap 0.12 ha"; without a boundary only worked and overlap.</summary>
        public static string FormatStats(GuidanceSnapshot g)
        {
            double worked = g.WorkedSquareMeters / 10000.0, overlap = g.OverlapSquareMeters / 10000.0;
            if (!double.IsNaN(g.FieldSquareMeters) && g.FieldSquareMeters > 0)
            {
                double field = g.FieldSquareMeters / 10000.0;
                double inside = double.IsNaN(g.WorkedInsideSquareMeters) ? 0 : g.WorkedInsideSquareMeters / 10000.0;
                double left = Math.Max(0, field - inside);
                return string.Format(Inv, Tr.T("Worked {0:0.00} ha\nLeft {1:0.00} ha  ({2:0} % done)\nOverlap {3:0.00} ha"),
                    inside, left, Math.Min(100, inside / field * 100), overlap);
            }
            return string.Format(Inv, Tr.T("Worked {0:0.00} ha\nOverlap {1:0.00} ha"), worked, overlap);
        }

        /// <summary>"◀ 12 cm" = right of the line, steer left; "12 cm ▶" = steer right.</summary>
        public static string FormatOffset(double meters)
        {
            double cm = meters * 100.0;
            double abs = Math.Abs(cm);
            if (abs < 2) return Tr.T("▲ on line");
            string amount = abs < 100 ? abs.ToString("0", Inv) + " cm" : (abs / 100).ToString("0.00", Inv) + " m";
            return cm > 0 ? "◀ " + amount : amount + " ▶";
        }

        private void Render()
        {
            AppState.Tick();
            if (AppState.LastAlertUtc != _shownAlertUtc && AppState.LastAlert != null)
            {
                // The sound plays from AppState (also in the background); here the matching text.
                _shownAlertUtc = AppState.LastAlertUtc;
                if ((DateTime.UtcNow - _shownAlertUtc).TotalSeconds < 5) Toast_(AppState.LastAlert);
            }
            LinkSnapshot link = AppState.Link?.GetSnapshot();
            GuidanceSnapshot g = AppState.Guidance.GetSnapshot();
            AppSettings settings = AppState.Settings;
            AppState.Map.UrlTemplate = string.IsNullOrWhiteSpace(settings.MapUrl) ? MapTiles.DefaultUrl : settings.MapUrl;
            _view.Map = settings.ShowMap ? AppState.Map : null;
            _view.MapAttribution = settings.MapAttribution;
            _view.MapMaxZoom = settings.MapMaxZoomLevel;
            RenderSimulator();

            // Tr.T("Follow") stands out while the map was moved by hand.
            bool following = _view.Following;
            _followButton.SetBackgroundColor(Color.ParseColor(following ? "#E0E0E0" : "#FB8C00"));
            _followButton.SetTextColor(following ? Color.ParseColor("#212121") : Color.White);

            bool coverageOn = AppState.Settings.RecordCoverage;
            _view.ShowCoverage = coverageOn && AppState.Settings.ShowCoverage;
            _view.Show(g);

            _work.Visibility = coverageOn ? ViewStates.Visible : ViewStates.Gone;
            _stats.Visibility = coverageOn ? ViewStates.Visible : ViewStates.Gone;
            _workLabel.Visibility = _work.Visibility;
            _work.Text = g.Working ? Tr.T("Work: ON") : g.WorkPausedInHeadland ? Tr.T("Work: headland") : Tr.T("Work: OFF");
            _work.SetBackgroundColor(Color.ParseColor(g.Working ? "#2E7D32" : "#9E9E9E"));
            _work.SetTextColor(Color.White);
            _stats.Text = g.WorkedSquareMeters > 0 || !double.IsNaN(g.FieldSquareMeters) ? FormatStats(g) : "";

            _connect.Text = AppState.IsConnected ? Tr.T("Disconnect") : Tr.T("Connect");

            // Fix badge and info line.
            SessionStats st = link?.Session;
            GnssFix fix = st?.Latest;
            bool fresh = link != null && link.State == LinkState.Connected && fix != null && st.SecondsSinceFix >= 0 && st.SecondsSinceFix < 3;
            if (link == null) SetBadge(Tr.T("Not connected"), "#757575");
            else if (link.State != LinkState.Connected) SetBadge(link.State == LinkState.Connecting ? Tr.T("Connecting") : Tr.T("Reconnecting"), "#757575");
            else if (!fresh) SetBadge(AppState.Simulator != null ? Tr.T("Sim: no position") : Tr.T("No position"), "#757575");
            else if (AppState.Simulator != null) SetBadge("SIM " + Tr.T(GnssFix.FixQualityName(fix.FixQuality)), "#6A1B9A");
            else SetBadge(Tr.T(GnssFix.FixQualityName(fix.FixQuality)), Ui.FixColor(fix.FixQuality));

            _fixInfo.Text = fresh
                ? string.Format(Inv, Tr.T("{0} sats   HDOP {1}   age {2} s   {3:0.0} Hz   {4} km/h   {5}"),
                    fix.Satellites, Num(fix.Hdop, "0.0"), Num(fix.AgeSeconds, "0.0"), st.RateHz, Num(fix.SpeedKmh, "0.0"), NtripText(link))
                : link == null ? Tr.T("Press Connect") : SourceText(link);

            // Without RTK fix the centimetres are not to be trusted: say so, and grey out the guidance.
            bool rtk = fresh && fix.FixQuality == 4;
            if (fresh && !rtk)
            {
                _accuracy.Text = fix.FixQuality == 5 ? Tr.T("RTK FLOAT - accuracy about ±0.3 m")
                    : Tr.T("NO RTK - accuracy about ±1-2 m") + (link.Ntrip != null ? Tr.T("  (corrections lost)") : "");
                _accuracy.SetBackgroundColor(Color.ParseColor(fix.FixQuality == 5 ? "#EF6C00" : "#C62828"));
                _accuracy.Visibility = ViewStates.Visible;
            }
            else _accuracy.Visibility = ViewStates.Gone;

            _lightbar.Show(fresh && g.Steer != null ? g.Steer.SteerDegrees : double.NaN, !rtk);

            // Distance off the line.
            AbGuidanceResult r = g.Result;
            if (r != null && fresh)
            {
                double abs = Math.Abs(r.CrossTrackMeters * 100.0);
                _xte.Text = FormatOffset(r.CrossTrackMeters);
                _xte.SetTextColor(Color.ParseColor(!rtk ? "#90A4AE" : abs < 5 ? "#66BB6A" : abs < 15 ? "#FFCA28" : "#EF5350"));
                _lineInfo.Text = string.Format(Inv, Tr.T("{0}   pass {1}   {2:0.0}°{3}{4}"),
                    g.ActiveLine.Name, r.Pass, r.TravelLineHeading,
                    double.IsNaN(r.HeadingErrorDegrees) ? "" : string.Format(Inv, Tr.T("   heading err {0:+0.0;-0.0}°"), r.HeadingErrorDegrees),
                    g.ActiveLine.NudgeMeters == 0 ? "" : string.Format(Inv, Tr.T("   nudged {0:+0;-0} cm"), g.ActiveLine.NudgeMeters * 100));
            }
            else
            {
                _xte.Text = "";
                _lineInfo.Text = g.ActiveLine != null ? g.ActiveLine.Name : "";
            }

            // Field edge: warn within half the working width plus 3 m.
            if (fresh && g.InsideField == false)
            {
                _edge.Text = Tr.T("OUTSIDE FIELD");
                _edge.SetBackgroundColor(Color.ParseColor("#C62828"));
                _edge.Visibility = ViewStates.Visible;
            }
            else if (fresh && g.InsideField == true && g.DistanceToBoundary < g.PassWidth / 2 + 3)
            {
                _edge.Text = string.Format(Inv, Tr.T("Field edge {0:0.0} m"), g.DistanceToBoundary);
                _edge.SetBackgroundColor(Color.ParseColor("#EF6C00"));
                _edge.Visibility = ViewStates.Visible;
            }
            else _edge.Visibility = ViewStates.Gone;

            // Field and boundary buttons.
            Field field = g.ActiveField;
            _field.Text = field == null ? Tr.T("Field: none") : Tr.F("Field: {0}", field.Name
                + (field.HasBoundary ? "  " + field.AreaHectares.ToString("0.0", Inv) + " ha" : ""));
            _boundary.Text = g.RecordingBoundary ? Tr.T("Finish boundary") : field != null && field.HasBoundary ? Tr.T("New boundary") : Tr.T("Record boundary");
            _cancelBoundary.Visibility = g.RecordingBoundary ? ViewStates.Visible : ViewStates.Gone;
            ViewStates corners = g.RecordingBoundary && g.BoundaryMode == BoundaryMode.Corners ? ViewStates.Visible : ViewStates.Gone;
            _cornerRow.Visibility = corners;

            // Line recording state.
            _cancel.Visibility = g.Recording ? ViewStates.Visible : ViewStates.Gone;
            _b.Enabled = g.Recording;
            _a.Text = g.Recording ? Tr.T("Restart A") : "A";

            if (g.RecordingBoundary && g.BoundaryMode == BoundaryMode.Corners)
            {
                _recordInfo.Text = g.BoundaryCorners == 0 ? Tr.T("Drive to the first corner and press Corner")
                    : Tr.F("Corners: {0}. Drive to the next one and press Corner, or Finish.", g.BoundaryCorners);
            }
            else if (g.RecordingBoundary)
            {
                _recordInfo.Text = string.Format(Inv, Tr.T("Boundary: {0:0} m driven, {1:0} m to the start"),
                    g.BoundaryRecordedMeters, double.IsNaN(g.BoundaryGapMeters) ? 0 : g.BoundaryGapMeters);
            }
            else if (g.Recording)
            {
                _recordInfo.Text = Tr.F(g.RecordingKind == LineKind.Curve ? Tr.T("Recording curve: {0:0} m from A") : Tr.T("Recording line: {0:0} m from A"),
                    double.IsNaN(g.RecordedMeters) ? 0 : g.RecordedMeters);
            }
            else _recordInfo.Text = "";
        }

        // ---------------------------------------------------------------- simulator

        private void SimSpeed(int step)
        {
            GnssSimulator sim = AppState.Simulator;
            if (sim != null) sim.SpeedKmh = Math.Round(sim.SpeedKmh + step);
        }

        /// <summary>Turns the wheel by <paramref name="degrees"/>; 0 centres it. Manual steering switches Auto off.</summary>
        private void SimSteer(double degrees)
        {
            GnssSimulator sim = AppState.Simulator;
            if (sim == null) return;
            sim.AutoSteer = false;
            sim.WheelAngle = degrees == 0 ? 0 : sim.WheelAngle + degrees;
        }

        private void RenderSimulator()
        {
            GnssSimulator sim = AppState.Simulator;
            _simPanel.Visibility = sim == null ? ViewStates.Gone : ViewStates.Visible;
            if (sim == null) return;
            _simSpeed.Text = string.Format(Inv, "{0:0} km/h\n{1:+0;-0;0}°", sim.SpeedKmh, sim.WheelAngle);
            _simAuto.Text = sim.AutoSteer ? Tr.T("Auto ON") : "Auto";
            _simAuto.SetTextColor(Color.ParseColor(sim.AutoSteer ? "#2E7D32" : "#212121"));
        }

        /// <summary>Says what is connected and why there is no position, e.g. "Bluetooth: connected, but no data from the receiver".</summary>
        private static string SourceText(LinkSnapshot link)
        {
            string source = AppState.Simulator != null ? "Simulator" : "Bluetooth";
            if (link.State != LinkState.Connected) return Tr.F("{0}: {1}", source, Tr.T(link.Message));
            SessionStats st = link.Session;
            if (st == null || st.BytesIn == 0)
                return Tr.F(AppState.Simulator != null ? Tr.T("{0}: connected, but no data")
                    : Tr.T("{0}: connected, but no data from the receiver (is it on and wired to the bridge?)"), source);
            if (st.Sentences == 0 && st.PgnFrames == 0)
                return Tr.F("{0}: data arrives but no position sentences (check the receiver's output and baud rate)", source);
            return Tr.F("{0}: connected, no position fix yet", source);
        }

        private static string NtripText(LinkSnapshot link)
        {
            NtripStatus n = link.Ntrip;
            if (n == null) return "";
            return n.Frames > 0 && n.SecondsSinceLastFrame >= 0 && n.SecondsSinceLastFrame < 10 ? "NTRIP ok" : Tr.F("NTRIP: {0}", Tr.T(n.Headline));
        }

        private void SetBadge(string text, string color)
        {
            _fixBadge.Text = text;
            _fixBadge.SetBackgroundColor(Color.ParseColor(color));
        }

        private static string Num(double v, string format) => double.IsNaN(v) ? "-" : v.ToString(format, Inv);

        // ---------------------------------------------------------------- layout

        private int Dp(float v) => Ui.Dp(this, v);

        private TextView Label(string text)
        {
            TextView t = Ui.Text(this, 14, false, Palette.PanelLabel);
            t.Text = text;
            t.SetPadding(0, Dp(10), 0, 0);
            return t;
        }

        private View BuildLayout()
        {
            var root = new LinearLayout(this) { Orientation = Orientation.Vertical };
            root.SetBackgroundColor(Color.ParseColor(Palette.Screen));

            // Top bar: fix, info, connect, settings.
            var bar = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            bar.SetGravity(GravityFlags.CenterVertical);
            bar.SetPadding(Dp(8), Dp(4), Dp(8), Dp(4));
            _fixBadge = Ui.Text(this, 22, true, "#FFFFFF");
            _fixBadge.Gravity = GravityFlags.Center;
            _fixBadge.SetPadding(Dp(16), Dp(8), Dp(16), Dp(8));
            _fixBadge.SetMinWidth(Dp(170));
            bar.AddView(_fixBadge);
            _fixInfo = Ui.Text(this, 16, false, Palette.InfoText);
            _fixInfo.SetPadding(Dp(12), 0, Dp(12), 0);
            _fixInfo.SetSingleLine(true);
            _fixInfo.Ellipsize = global::Android.Text.TextUtils.TruncateAt.End;
            bar.AddView(_fixInfo, Ui.Weight(1));
            _connect = Ui.Button(this, Tr.T("Connect"), OnConnectClicked, 48);
            bar.AddView(_connect);
            bar.AddView(Ui.Button(this, Tr.T("Help"), () => StartActivity(new Intent(this, typeof(HelpActivity))), 48));
            bar.AddView(Ui.Button(this, Tr.T("Settings"), () => StartActivity(new Intent(this, typeof(SettingsActivity))), 48));
            root.AddView(bar);

            var body = new LinearLayout(this) { Orientation = Orientation.Horizontal };

            // Guidance view with the distance and edge overlay.
            var frame = new FrameLayout(this);
            _view = new GuidanceView(this);
            frame.AddView(_view);
            var overlay = new LinearLayout(this) { Orientation = Orientation.Vertical };
            overlay.SetGravity(GravityFlags.CenterHorizontal);
            _lightbar = new LightbarView(this);
            overlay.AddView(_lightbar, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, Dp(44)));
            _xte = Ui.Text(this, 48, true, "#FFFFFF");
            _xte.Gravity = GravityFlags.Center;
            _xte.SetShadowLayer(6, 0, 0, Color.Black);
            overlay.AddView(_xte);
            _lineInfo = Ui.Text(this, 16, false, "#ECEFF1");
            _lineInfo.Gravity = GravityFlags.Center;
            _lineInfo.SetShadowLayer(4, 0, 0, Color.Black);
            overlay.AddView(_lineInfo);
            _edge = Ui.Text(this, 24, true, "#FFFFFF");
            _edge.SetPadding(Dp(16), Dp(6), Dp(16), Dp(6));
            _edge.Visibility = ViewStates.Gone;
            overlay.AddView(_edge, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent));
            _accuracy = Ui.Text(this, 20, true, "#FFFFFF");
            _accuracy.SetPadding(Dp(16), Dp(6), Dp(16), Dp(6));
            _accuracy.Visibility = ViewStates.Gone;
            var ap = new LinearLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent) { TopMargin = Dp(6) };
            overlay.AddView(_accuracy, ap);
            frame.AddView(overlay, new FrameLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Top));

            // Map controls, right edge: zoom, whole field, back to following the tractor.
            var mapButtons = new LinearLayout(this) { Orientation = Orientation.Vertical };
            Button MapButton(string text, Action onClick)
            {
                Button b = Ui.Button(this, text, onClick, 52);
                b.SetMinWidth(Dp(64));
                b.SetMinimumWidth(Dp(64));
                b.Alpha = 0.92f;
                mapButtons.AddView(b, new LinearLayout.LayoutParams(Dp(96), ViewGroup.LayoutParams.WrapContent) { BottomMargin = Dp(4) });
                return b;
            }
            MapButton("+", () => _view.ZoomBy(1.5));
            MapButton("−", () => _view.ZoomBy(1 / 1.5));
            MapButton(Tr.T("Field"), () =>
            {
                if (!_view.ShowWholeField()) Toast_(Tr.T("Open a field with a boundary, or select a line"));
            });
            _followButton = MapButton(Tr.T("Follow"), () => _view.Follow());
            var mp = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent,
                GravityFlags.Right | GravityFlags.CenterVertical) { RightMargin = Dp(8) };
            frame.AddView(mapButtons, mp);

            // Simulator controls, bottom left, only while the simulator drives.
            _simPanel = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            _simPanel.SetBackgroundColor(Color.Argb(170, 0, 0, 0));
            _simPanel.SetPadding(Dp(6), Dp(6), Dp(6), Dp(6));
            _simPanel.SetGravity(GravityFlags.CenterVertical);
            _simPanel.AddView(Ui.Button(this, "−", () => SimSpeed(-1), 48));
            _simSpeed = Ui.Text(this, 18, true, "#FFFFFF");
            _simSpeed.SetMinWidth(Dp(90));
            _simSpeed.Gravity = GravityFlags.Center;
            _simPanel.AddView(_simSpeed);
            _simPanel.AddView(Ui.Button(this, "+", () => SimSpeed(+1), 48));
            _simPanel.AddView(Ui.Button(this, "<", () => SimSteer(-3), 48));
            _simPanel.AddView(Ui.Button(this, "|", () => SimSteer(0), 48));
            _simPanel.AddView(Ui.Button(this, ">", () => SimSteer(+3), 48));
            _simAuto = Ui.Button(this, "Auto", () => { GnssSimulator sim = AppState.Simulator; if (sim != null) sim.AutoSteer = !sim.AutoSteer; }, 48);
            _simPanel.AddView(_simAuto);
            _simPanel.AddView(Ui.Button(this, Tr.T("To field"), () =>
            {
                if (AppState.MoveSimulatorToField()) _view.Follow();
                else Toast_(Tr.T("Open a field with a boundary first"));
            }, 48));
            _simPanel.Visibility = ViewStates.Gone;
            var sp = new FrameLayout.LayoutParams(ViewGroup.LayoutParams.WrapContent, ViewGroup.LayoutParams.WrapContent, GravityFlags.Bottom | GravityFlags.Left)
            { LeftMargin = Dp(8), BottomMargin = Dp(24) };
            frame.AddView(_simPanel, sp);
            body.AddView(frame, Ui.Weight(1, ViewGroup.LayoutParams.MatchParent));

            // Right panel.
            var panel = new LinearLayout(this) { Orientation = Orientation.Vertical };
            panel.SetPadding(Dp(8), 0, Dp(8), Dp(8));

            _workLabel = Label(Tr.T("Implement"));
            panel.AddView(_workLabel);
            _work = Ui.Button(this, Tr.T("Work: OFF"), () => AppState.SetWorking(!AppState.Guidance.Working), 64);
            _work.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 22);
            panel.AddView(_work);
            _stats = Ui.Text(this, 15, false, Palette.PanelText);
            panel.AddView(_stats);
            panel.AddView(Ui.Button(this, Tr.T("Jobs..."), ShowJobs, 44));

            panel.AddView(Label(Tr.T("Field")));
            _field = Ui.Button(this, Tr.T("Field: none"), ShowFields, 52);
            _field.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 16);
            panel.AddView(_field);
            _boundary = Ui.Button(this, Tr.T("Record boundary"), OnBoundaryClicked, 52);
            panel.AddView(_boundary);
            _cornerRow = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            Button corner = Ui.Button(this, Tr.T("Corner"), OnCornerClicked, 64);
            corner.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 22);
            _cornerRow.AddView(corner, Ui.Weight(2));
            _cornerRow.AddView(Ui.Button(this, Tr.T("Undo"), () => AppState.Guidance.RemoveLastCorner(), 64), Ui.Weight(1));
            _cornerRow.Visibility = ViewStates.Gone;
            panel.AddView(_cornerRow);
            _cancelBoundary = Ui.Button(this, Tr.T("Cancel boundary"), () => AppState.Guidance.CancelBoundary(), 44);
            panel.AddView(_cancelBoundary);

            panel.AddView(Label(Tr.T("New line")));
            var kinds = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            _straight = Ui.Button(this, Tr.T("Straight"), () => SetKind(LineKind.Straight), 48);
            _curve = Ui.Button(this, Tr.T("Curve"), () => SetKind(LineKind.Curve), 48);
            kinds.AddView(_straight, Ui.Weight(1));
            kinds.AddView(_curve, Ui.Weight(1));
            panel.AddView(kinds);

            var ab = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            _a = Ui.Button(this, "A", OnAClicked, 80);
            _b = Ui.Button(this, "B", OnBClicked, 80);
            _a.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 28);
            _b.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 28);
            ab.AddView(_a, Ui.Weight(1));
            ab.AddView(_b, Ui.Weight(1));
            panel.AddView(ab);
            _cancel = Ui.Button(this, Tr.T("Cancel line"), () => AppState.Guidance.CancelAb(), 44);
            panel.AddView(_cancel);
            _recordInfo = Ui.Text(this, 14, false, Palette.RecordText);
            panel.AddView(_recordInfo);
            panel.AddView(Ui.Button(this, Tr.T("Lines..."), ShowLines, 56));

            // Nudge: move the active line a step left/right (as seen from the seat), snap it to here, reset.
            panel.AddView(Label(Tr.T("Nudge line")));
            var nudge = new LinearLayout(this) { Orientation = Orientation.Horizontal };
            nudge.AddView(Ui.Button(this, "◀", () => AppState.Nudge(-1), 52), Ui.Weight(1));
            nudge.AddView(Ui.Button(this, Tr.T("Snap"), AppState.SnapLine, 52), Ui.Weight(1.2f));
            nudge.AddView(Ui.Button(this, "▶", () => AppState.Nudge(+1), 52), Ui.Weight(1));
            panel.AddView(nudge);
            panel.AddView(Ui.Button(this, Tr.T("Reset nudge"), AppState.ResetNudge, 44));

            var scroll = new ScrollView(this);
            scroll.AddView(panel);
            body.AddView(scroll, new LinearLayout.LayoutParams(Dp(250), ViewGroup.LayoutParams.MatchParent));
            root.AddView(body, new LinearLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, 0, 1));

            SetKind(LineKind.Straight);
            return root;
        }
    }
}

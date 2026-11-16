using System;
using System.Globalization;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Graphics.Drawables;
using Android.OS;
using AgLibrary.Ntrip;
using AgOpenGPS.Mobile.Core;
using AgOpenGPS.Mobile.Core.Guidance;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Foreground service that keeps the Bluetooth link, NTRIP and guidance running while the
    /// screen is off or another app is in front. Shows a notification with the fix and a
    /// Disconnect button. The work itself lives in <see cref="AppState"/>; this only keeps the
    /// process and CPU awake.
    /// </summary>
    [Service(Exported = false, ForegroundServiceType = ForegroundService.TypeConnectedDevice)]
    public class GnssService : Service
    {
        public const string ActionStop = "org.agopengps.android.STOP";
        private const string ChannelId = "link";
        private const int NotificationId = 1;
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        private PowerManager.WakeLock _wakeLock;
        private Handler _handler;
        private bool _running;

        public override IBinder OnBind(Intent intent) => null;

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            if (intent?.Action == ActionStop)
            {
                Task.Run(AppState.Disconnect); // stops this service too
                StopSelf();
                return StartCommandResult.NotSticky;
            }

            CreateChannel();
            Notification n = BuildNotification();
            try
            {
                if (OperatingSystem.IsAndroidVersionAtLeast(29)) StartForeground(NotificationId, n, ForegroundService.TypeConnectedDevice);
                else StartForeground(NotificationId, n);
            }
            catch (Java.Lang.Exception)
            {
                // Android refused (permission missing, started from the background): the link still
                // runs while the app is open, just without the background notification.
                StopSelf();
                return StartCommandResult.NotSticky;
            }

            if (_wakeLock == null)
            {
                var pm = (PowerManager)GetSystemService(PowerService);
                _wakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "AgOpenGPS::Link");
                _wakeLock.SetReferenceCounted(false);
                _wakeLock.Acquire();
            }

            if (!_running)
            {
                _running = true;
                _handler = new Handler(Looper.MainLooper);
                _handler.PostDelayed(Update, 2000);
            }

            // If Android kills the process, the link is gone with it: do not restart empty.
            return StartCommandResult.NotSticky;
        }

        public override void OnDestroy()
        {
            _running = false;
            if (_wakeLock != null && _wakeLock.IsHeld) _wakeLock.Release();
            _wakeLock = null;
            base.OnDestroy();
        }

        private void Update()
        {
            if (!_running) return;
            try
            {
                AppState.Tick();
                var nm = (NotificationManager)GetSystemService(NotificationService);
                nm.Notify(NotificationId, BuildNotification());
            }
            finally
            {
                _handler.PostDelayed(Update, 2000);
            }
        }

        private void CreateChannel()
        {
            var nm = (NotificationManager)GetSystemService(NotificationService);
            var channel = new NotificationChannel(ChannelId, Tr.T("Receiver connection"), NotificationImportance.Low)
            {
                Description = Tr.T("Shown while AgOpenGPS is connected to the receiver"),
            };
            channel.SetShowBadge(false);
            nm.CreateNotificationChannel(channel);
        }

        private Notification BuildNotification()
        {
            Intent open = new Intent(this, typeof(MainActivity)).AddFlags(ActivityFlags.SingleTop | ActivityFlags.ReorderToFront);
            PendingIntent openPi = PendingIntent.GetActivity(this, 0, open, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);
            Intent stop = new Intent(this, typeof(GnssService)).SetAction(ActionStop);
            PendingIntent stopPi = PendingIntent.GetService(this, 1, stop, PendingIntentFlags.Immutable | PendingIntentFlags.UpdateCurrent);

            Describe(out string title, out string text);
            var stopAction = new Notification.Action.Builder(
                Icon.CreateWithResource(this, global::Android.Resource.Drawable.IcMenuCloseClearCancel), Tr.T("Disconnect"), stopPi).Build();

            return new Notification.Builder(this, ChannelId)
                .SetSmallIcon(global::Android.Resource.Drawable.IcMenuMyLocation)
                .SetContentTitle(title)
                .SetContentText(text)
                .SetContentIntent(openPi)
                .SetOngoing(true)
                .SetOnlyAlertOnce(true)
                .SetCategory(Notification.CategoryService)
                .AddAction(stopAction)
                .Build();
        }

        private static void Describe(out string title, out string text)
        {
            LinkSnapshot link = AppState.Link?.GetSnapshot();
            if (link == null)
            {
                title = "AgOpenGPS";
                text = Tr.T("Not connected");
                return;
            }

            GnssFix fix = link.Session?.Latest;
            bool fresh = link.State == LinkState.Connected && fix != null && link.Session.SecondsSinceFix >= 0 && link.Session.SecondsSinceFix < 3;
            if (!fresh)
            {
                title = link.State == LinkState.Connected ? Tr.T("Connected, no position") : Tr.T("Reconnecting to the receiver");
            }
            else
            {
                NtripStatus n = link.Ntrip;
                string ntrip = n == null ? "" : n.Frames > 0 && n.SecondsSinceLastFrame < 10 ? "  ·  NTRIP ok" : Tr.F("  ·  NTRIP: {0}", Tr.T(n.Headline));
                title = string.Format(Inv, Tr.T("{0}  ·  {1} sats{2}"), Tr.T(GnssFix.FixQualityName(fix.FixQuality)), fix.Satellites, ntrip);
            }

            GuidanceSnapshot g = AppState.Guidance.GetSnapshot();
            if (g.RecordingBoundary) text = string.Format(Inv, Tr.T("Recording boundary: {0:0} m"), g.BoundaryRecordedMeters);
            else if (g.Recording) text = Tr.T("Recording line: press B in the app");
            else if (g.Result != null && fresh)
                text = string.Format(Inv, Tr.T("{0}  pass {1}:  {2}"), g.ActiveLine.Name, g.Result.Pass, MainActivity.FormatOffset(g.Result.CrossTrackMeters));
            else text = g.ActiveField != null ? g.ActiveField.Name : Tr.T("No field open");

            if (g.InsideField == false) text = Tr.F("OUTSIDE FIELD  ·  {0}", text);
            if (g.WorkedSquareMeters > 0) text += "  ·  " + MainActivity.FormatStats(g).Replace("\n", "  ");
        }
    }
}

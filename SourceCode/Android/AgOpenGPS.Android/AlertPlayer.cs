using System;
using Android.Media;
using AgOpenGPS.Mobile.Core;

namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Plays the alert sounds on Android's alarm channel, so they are heard with the tablet on
    /// silent and over the noise of a cab. Each alert has its own tone.
    /// </summary>
    public sealed class AlertPlayer
    {
        private ToneGenerator _tones;

        private ToneGenerator Tones
        {
            get
            {
                if (_tones == null)
                {
                    try { _tones = new ToneGenerator(Stream.Alarm, 100); }
                    catch (Java.Lang.RuntimeException) { _tones = null; } // no audio right now; try again next time
                }
                return _tones;
            }
        }

        public void Play(AlertKind kind)
        {
            (Tone tone, int ms) = kind switch
            {
                AlertKind.FieldEdge => (Tone.PropBeep2, 400),
                AlertKind.LeftField => (Tone.CdmaAlertCallGuard, 700),
                AlertKind.RtkLost => (Tone.SupError, 900),
                AlertKind.RtkBack => (Tone.PropAck, 300),
                AlertKind.CorrectionsStopped => (Tone.CdmaAbbrAlert, 700),
                AlertKind.LinkLost => (Tone.SupRadioNotavail, 1200),
                AlertKind.OffLine => (Tone.PropBeep, 200),
                AlertKind.HeadlandAhead => (Tone.CdmaPip, 500),
                _ => (Tone.PropBeep, 200),
            };
            try { Tones?.StartTone(tone, ms); }
            catch (Java.Lang.RuntimeException) { }
        }

        /// <summary>The message shown with an alert (English; translated where shown).</summary>
        public static string Message(AlertKind kind)
        {
            switch (kind)
            {
                case AlertKind.FieldEdge: return "Field edge ahead";
                case AlertKind.LeftField: return "You left the field";
                case AlertKind.RtkLost: return "RTK Fix lost";
                case AlertKind.RtkBack: return "RTK Fix is back";
                case AlertKind.CorrectionsStopped: return "NTRIP corrections stopped";
                case AlertKind.LinkLost: return "Connection to the receiver lost";
                case AlertKind.OffLine: return "Off the line";
                case AlertKind.HeadlandAhead: return "Headland ahead";
                default: return kind.ToString();
            }
        }
    }
}

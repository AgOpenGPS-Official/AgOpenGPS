using System;
using System.Collections.Generic;

namespace AgOpenGPS.Mobile.Core
{
    public enum AlertKind
    {
        /// <summary>Close to the field boundary (inside the field).</summary>
        FieldEdge,
        /// <summary>Just left the field.</summary>
        LeftField,
        /// <summary>The fix dropped from RTK Fix.</summary>
        RtkLost,
        /// <summary>RTK Fix is back after it was lost.</summary>
        RtkBack,
        /// <summary>NTRIP corrections stopped arriving.</summary>
        CorrectionsStopped,
        /// <summary>The link to the receiver dropped or went silent.</summary>
        LinkLost,
        /// <summary>Off the guidance line for a while.</summary>
        OffLine,
        /// <summary>Close to the headland line, coming from the main part of the field.</summary>
        HeadlandAhead,
    }

    /// <summary>What the alert checks look at, sampled a few times a second.</summary>
    public sealed class AlertInputs
    {
        public DateTime Now;
        /// <summary>The link is up and positions arrive.</summary>
        public bool Receiving;
        /// <summary>GGA fix quality, -1 without a recent position.</summary>
        public int FixQuality = -1;
        public bool? InsideField;
        public double DistanceToBoundary = double.NaN;
        /// <summary>NTRIP is in use (configured and running).</summary>
        public bool NtripActive;
        /// <summary>Seconds since the last correction frame; NaN before the first one.</summary>
        public double SecondsSinceCorrection = double.NaN;
        public double CrossTrackMeters = double.NaN;
        public double SpeedKmh = double.NaN;
        /// <summary>Distance from the implement to the headland line, positive in the main part; NaN without headland.</summary>
        public double DistanceToHeadland = double.NaN;
    }

    public sealed class AlertSettings
    {
        public readonly HashSet<AlertKind> Enabled = new HashSet<AlertKind>
        {
            AlertKind.FieldEdge, AlertKind.LeftField, AlertKind.RtkLost, AlertKind.RtkBack,
            AlertKind.CorrectionsStopped, AlertKind.LinkLost, AlertKind.HeadlandAhead,
        };
        public double EdgeWarningMeters = 5.0;
        public double OffLineMeters = 0.20;
    }

    /// <summary>
    /// Decides when to sound an alert. Every alert fires once per event and is armed again only
    /// when the situation has clearly cleared (hysteresis), and short flickers are ignored (each
    /// condition must hold for a moment), so a fix jumping between Fix and Float or a position
    /// wobbling at the edge does not beep all the time. Not thread safe: call from one thread.
    /// </summary>
    public sealed class AlertMonitor
    {
        public static readonly TimeSpan RtkLostAfter = TimeSpan.FromSeconds(1.5);
        public static readonly TimeSpan RtkBackAfter = TimeSpan.FromSeconds(1.0);
        public static readonly TimeSpan LinkLostAfter = TimeSpan.FromSeconds(3.0);
        public static readonly TimeSpan OffLineAfter = TimeSpan.FromSeconds(2.0);
        public const double CorrectionsStoppedSeconds = 15.0;
        public const double OffLineMinSpeedKmh = 2.0;

        private readonly AlertSettings _settings;

        private bool _edgeArmed = true;
        private bool _headlandArmed = true;
        private bool? _lastInside;
        private bool _hadRtk;
        private bool _rtkLostAnnounced;
        private DateTime? _notRtkSince, _rtkSince;
        private bool _hadLink;
        private DateTime? _noLinkSince;
        private bool _correctionsArmed;
        private DateTime? _offSince;
        private bool _offArmed = true;

        public AlertMonitor(AlertSettings settings)
        {
            _settings = settings ?? new AlertSettings();
        }

        /// <summary>Forget everything (new connection): nothing is "lost" before it was there.</summary>
        public void Reset()
        {
            _edgeArmed = true;
            _headlandArmed = true;
            _lastInside = null;
            _hadRtk = _rtkLostAnnounced = false;
            _notRtkSince = _rtkSince = null;
            _hadLink = false;
            _noLinkSince = null;
            _correctionsArmed = false;
            _offSince = null;
            _offArmed = true;
        }

        /// <summary>Returns the alerts to sound now (usually none).</summary>
        public List<AlertKind> Update(AlertInputs s)
        {
            var fired = new List<AlertKind>();
            void Fire(AlertKind k)
            {
                if (_settings.Enabled.Contains(k)) fired.Add(k);
            }

            // ---- link
            if (s.Receiving)
            {
                _hadLink = true;
                _noLinkSince = null;
            }
            else if (_hadLink)
            {
                _noLinkSince ??= s.Now;
                if (s.Now - _noLinkSince.Value >= LinkLostAfter)
                {
                    Fire(AlertKind.LinkLost);
                    _hadLink = false; // once, until positions come again
                }
            }

            bool hasFix = s.Receiving && s.FixQuality > 0;

            // ---- RTK lost / back (only once RTK Fix was there; a flicker shorter than the delay is ignored)
            if (s.Receiving)
            {
                bool rtk = s.FixQuality == 4;
                if (rtk)
                {
                    _notRtkSince = null;
                    _rtkSince ??= s.Now;
                    if (_rtkLostAnnounced && s.Now - _rtkSince.Value >= RtkBackAfter)
                    {
                        Fire(AlertKind.RtkBack);
                        _rtkLostAnnounced = false;
                    }
                    _hadRtk = true;
                }
                else
                {
                    _rtkSince = null;
                    if (_hadRtk && !_rtkLostAnnounced)
                    {
                        _notRtkSince ??= s.Now;
                        if (s.Now - _notRtkSince.Value >= RtkLostAfter)
                        {
                            Fire(AlertKind.RtkLost);
                            _rtkLostAnnounced = true;
                        }
                    }
                }
            }

            // ---- corrections (only after they had been arriving)
            if (s.NtripActive && !double.IsNaN(s.SecondsSinceCorrection))
            {
                if (s.SecondsSinceCorrection < 5) _correctionsArmed = true;
                else if (_correctionsArmed && s.SecondsSinceCorrection >= CorrectionsStoppedSeconds)
                {
                    Fire(AlertKind.CorrectionsStopped);
                    _correctionsArmed = false;
                }
            }

            // ---- field edge and leaving the field
            if (hasFix && s.InsideField.HasValue)
            {
                bool inside = s.InsideField.Value;
                if (_lastInside == true && !inside) Fire(AlertKind.LeftField);
                _lastInside = inside;

                double d = s.DistanceToBoundary;
                if (inside && !double.IsNaN(d))
                {
                    if (_edgeArmed && d <= _settings.EdgeWarningMeters)
                    {
                        Fire(AlertKind.FieldEdge);
                        _edgeArmed = false;
                    }
                    else if (d > _settings.EdgeWarningMeters + 2) _edgeArmed = true; // re-arm with 2 m margin
                }
            }

            // ---- headland ahead (in the main part, getting close to the headland line)
            if (hasFix && !double.IsNaN(s.DistanceToHeadland))
            {
                double h = s.DistanceToHeadland;
                if (_headlandArmed && h > 0 && h <= _settings.EdgeWarningMeters)
                {
                    Fire(AlertKind.HeadlandAhead);
                    _headlandArmed = false;
                }
                else if (h > _settings.EdgeWarningMeters + 2) _headlandArmed = true;
            }

            // ---- off the line (moving, RTK, for a moment)
            double xte = Math.Abs(s.CrossTrackMeters);
            bool moving = !double.IsNaN(s.SpeedKmh) && s.SpeedKmh >= OffLineMinSpeedKmh;
            if (hasFix && s.FixQuality == 4 && moving && !double.IsNaN(xte) && xte > _settings.OffLineMeters)
            {
                _offSince ??= s.Now;
                if (_offArmed && s.Now - _offSince.Value >= OffLineAfter)
                {
                    Fire(AlertKind.OffLine);
                    _offArmed = false;
                }
            }
            else
            {
                _offSince = null;
                if (double.IsNaN(xte) || xte < _settings.OffLineMeters * 0.6) _offArmed = true;
            }

            return fired;
        }
    }
}

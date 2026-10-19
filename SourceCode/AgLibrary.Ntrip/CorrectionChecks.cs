using System;
using System.Collections.Generic;
using System.Globalization;

namespace AgLibrary.Ntrip
{
    public enum CheckLevel
    {
        Ok,
        /// <summary>Not wrong yet, or still settling.</summary>
        Warn,
        Bad,
    }

    /// <summary>One line of the "are the corrections good?" panel.</summary>
    public sealed class CheckItem
    {
        public CheckLevel Level { get; }
        public string Title { get; }
        /// <summary>A second line in smaller text; empty when there is nothing to add.</summary>
        public string Detail { get; }

        public CheckItem(CheckLevel level, string title, string detail = "")
        {
            Level = level;
            Title = title;
            Detail = detail ?? "";
        }
    }

    /// <summary>
    /// Answers "are my corrections good?" in plain words from what the client received and what the
    /// receiver reports: four lights instead of message numbers. Pure logic.
    /// </summary>
    public static class CorrectionChecks
    {
        /// <summary>The base station position (1005 or 1006) normally comes every 10 seconds.</summary>
        private const double BasePositionGraceSeconds = 12;

        /// <param name="receiverFixQuality">GGA fix quality of the GPS receiver (4 = RTK fixed, 5 = RTK float).</param>
        /// <param name="differentialAgeSeconds">The receiver's age of corrections, 0 when unknown.</param>
        public static IReadOnlyList<CheckItem> Build(NtripStatus status, int receiverFixQuality, double differentialAgeSeconds)
        {
            var items = new List<CheckItem>();
            RtcmStreamSnapshot rtcm = status.Rtcm;
            bool receiving = status.State == NtripState.Streaming || status.Frames > 0;

            if (status.Unframed)
            {
                items.Add(new CheckItem(CheckLevel.Ok, "Corrections coming in", "This stream cannot be checked in detail."));
                items.Add(GpsItem(receiverFixQuality, differentialAgeSeconds));
                return items;
            }

            // 1. The base station position
            if (rtcm.HasStationPosition)
                items.Add(new CheckItem(CheckLevel.Ok, "Base station position received"));
            else if (receiving && rtcm.SessionSeconds < BasePositionGraceSeconds)
                items.Add(new CheckItem(CheckLevel.Warn, "Base station position", "Waiting for it. It comes every 10 seconds."));
            else if (receiving)
                items.Add(new CheckItem(CheckLevel.Bad, "Base station position is missing", "RTK cannot start without it."));
            else
                items.Add(new CheckItem(CheckLevel.Bad, "Base station position", "Not received yet."));

            // 2. The satellite data
            if (rtcm.HasObservations)
                items.Add(new CheckItem(CheckLevel.Ok, "Satellites in the corrections", rtcm.Constellations));
            else if (receiving)
                items.Add(new CheckItem(CheckLevel.Bad, "No satellite data in the corrections", "The base station sends no observations."));
            else
                items.Add(new CheckItem(CheckLevel.Bad, "Satellites in the corrections", "Nothing received yet."));

            // 3. Fresh data
            double age = status.SecondsSinceLastFrame;
            if (age < 0)
                items.Add(new CheckItem(CheckLevel.Warn, "Fresh data", "Waiting for the first data."));
            else if (age <= 3)
                items.Add(new CheckItem(CheckLevel.Ok, "Fresh data", "Last data " + Seconds(age) + " ago."));
            else if (age <= 10)
                items.Add(new CheckItem(CheckLevel.Warn, "Data is late", "Last data " + Seconds(age) + " ago. Slow connection?"));
            else
                items.Add(new CheckItem(CheckLevel.Bad, "No new data", "Last data " + Seconds(age) + " ago."));

            // 4. What the receiver makes of it
            items.Add(GpsItem(receiverFixQuality, differentialAgeSeconds));
            return items;
        }

        private static CheckItem GpsItem(int fixQuality, double differentialAgeSeconds)
        {
            string ageText = differentialAgeSeconds > 0
                ? "Corrections are " + differentialAgeSeconds.ToString("0.0", CultureInfo.InvariantCulture) + " seconds old."
                : "";
            switch (fixQuality)
            {
                case 4: return new CheckItem(CheckLevel.Ok, "Your GPS: RTK fixed", ageText);
                case 5: return new CheckItem(CheckLevel.Warn, "Your GPS: RTK float", "Still working it out. Wait a moment.");
                case 1:
                case 2: return new CheckItem(CheckLevel.Warn, "Your GPS: no RTK yet", "Normal GPS accuracy only.");
                case 0: return new CheckItem(CheckLevel.Bad, "Your GPS has no fix", "");
                case 7:
                case 8: return new CheckItem(CheckLevel.Warn, "Your GPS is in manual or simulator mode", "");
                default: return new CheckItem(CheckLevel.Warn, "Your GPS: fix type " + fixQuality.ToString(CultureInfo.InvariantCulture), ageText);
            }
        }

        private static string Seconds(double s) =>
            s < 10 ? s.ToString("0.0", CultureInfo.InvariantCulture) + " seconds" : Math.Round(s).ToString("0", CultureInfo.InvariantCulture) + " seconds";
    }
}

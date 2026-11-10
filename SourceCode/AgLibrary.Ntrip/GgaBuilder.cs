using System;
using System.Globalization;
using System.Text;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Builds a $GPGGA sentence for the caster. Time is always UTC (N10). With a receiver fix
    /// the fix quality, satellites, HDOP and altitude come from the receiver; in manual mode a
    /// plausible fixed sentence is built (quality 1, altitude set by the user, N12).
    /// </summary>
    public static class GgaBuilder
    {
        /// <summary>Build from a receiver fix. Returns null while there is no fix (quality 0),
        /// so no GGA goes out that a VRS caster would ignore (N11).</summary>
        public static string FromReceiver(DateTime utc, double latitude, double longitude,
            int fixQuality, int satellites, double hdop, double altitude, double ageSeconds)
        {
            if (fixQuality <= 0) return null;
            return Build(utc, latitude, longitude, fixQuality, satellites, hdop, altitude, ageSeconds);
        }

        /// <summary>Build from a manually entered position.</summary>
        public static string FromManual(DateTime utc, double latitude, double longitude, double altitude)
        {
            return Build(utc, latitude, longitude, 1, 12, 1.0, altitude, 0);
        }

        private static string Build(DateTime utc, double latitude, double longitude,
            int fixQuality, int satellites, double hdop, double altitude, double ageSeconds)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            utc = utc.Kind == DateTimeKind.Local ? utc.ToUniversalTime() : utc;

            var sb = new StringBuilder("$GPGGA,", 96);
            sb.Append(utc.ToString("HHmmss.00", ci)).Append(',');
            AppendCoordinate(sb, latitude, 2, 'N', 'S');
            sb.Append(',');
            AppendCoordinate(sb, longitude, 3, 'E', 'W');
            sb.Append(',');
            sb.Append(fixQuality.ToString(ci)).Append(',');
            sb.Append(satellites.ToString("00", ci)).Append(',');
            sb.Append((hdop > 0 ? hdop : 1.0).ToString("0.0", ci)).Append(',');
            sb.Append(altitude.ToString("0.0", ci)).Append(",M,");
            sb.Append("0.0,M,");                                   // geoid separation unknown
            sb.Append(ageSeconds > 0 ? ageSeconds.ToString("0.0", ci) : "").Append(",");
            sb.Append('*');
            sb.Append(Checksum(sb.ToString()));
            sb.Append("\r\n");
            return sb.ToString();
        }

        /// <summary>ddmm.mmmmm (or dddmm.mmmmm), done in whole units so 59.999999 minutes
        /// cannot round up to an invalid "60.00000".</summary>
        private static void AppendCoordinate(StringBuilder sb, double degrees, int degreeDigits,
            char positive, char negative)
        {
            CultureInfo ci = CultureInfo.InvariantCulture;
            double abs = Math.Abs(degrees);
            // Minutes in units of 1e-5.
            long units = (long)Math.Round(abs * 60.0 * 100000.0);
            long wholeDeg = units / (60L * 100000L);
            long minuteUnits = units % (60L * 100000L);
            sb.Append(wholeDeg.ToString(new string('0', degreeDigits), ci));
            sb.Append((minuteUnits / 100000L).ToString("00", ci)).Append('.');
            sb.Append((minuteUnits % 100000L).ToString("00000", ci));
            sb.Append(',').Append(degrees >= 0 ? positive : negative);
        }

        /// <summary>XOR of the characters between '$' and '*', two hex digits.</summary>
        public static string Checksum(string sentence)
        {
            int sum = 0;
            for (int i = 1; i < sentence.Length && sentence[i] != '*'; i++)
                sum ^= sentence[i];
            return sum.ToString("X2", CultureInfo.InvariantCulture);
        }
    }
}

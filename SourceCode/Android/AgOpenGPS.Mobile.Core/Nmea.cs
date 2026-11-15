using System;
using System.Globalization;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// Parses the NMEA sentences AgIO understands and combines them into a <see cref="GnssFix"/>.
    /// GGA, PANDA and PAOGI complete a fix (like AgIO, which sends PGN 0xD6 on those);
    /// VTG, RMC and HDT only fill in speed and heading for the next one.
    /// </summary>
    public sealed class NmeaAccumulator
    {
        private double _speedKmh = double.NaN;
        private double _headingTrue = double.NaN;
        private double _headingDual = double.NaN;

        /// <summary>The last GGA sentence exactly as received, for the NTRIP caster (VRS needs it).</summary>
        public string LastGga { get; private set; }

        /// <summary>Feed one sentence (with or without the trailing CR/LF). Returns a completed fix or null.</summary>
        public GnssFix Process(string sentence, DateTime receivedUtc)
        {
            if (!Nmea.ChecksumOk(sentence)) return null;

            string body = Nmea.StripChecksum(sentence);
            string[] w = body.Split(',');
            if (w.Length < 2) return null;

            string id = w[0];
            // "$GNGGA" -> "GGA" (any talker); proprietary ids ($PANDA, $PAOGI) stay whole.
            string type = id.Length == 6 && id[1] != 'P' ? id.Substring(3) : id.Substring(1);

            switch (type)
            {
                case "GGA": return w.Length > 13 ? ParseGga(w, sentence, receivedUtc) : null;
                case "VTG": ParseVtg(w); return null;
                case "RMC": ParseRmc(w); return null;
                case "HDT": ParseHdt(w); return null;
                case "PANDA": return w.Length > 14 ? ParsePandaOrOgi(w, false, receivedUtc) : null;
                case "PAOGI": return w.Length > 14 ? ParsePandaOrOgi(w, true, receivedUtc) : null;
                default: return null;
            }
        }

        private GnssFix ParseGga(string[] w, string sentence, DateTime receivedUtc)
        {
            var fix = NewFix(receivedUtc);
            fix.UtcTimeOfDay = Nmea.ParseTime(w[1]);
            fix.Latitude = Nmea.ParseLatLon(w[2], w[3], 2);
            fix.Longitude = Nmea.ParseLatLon(w[4], w[5], 3);
            fix.FixQuality = Nmea.Int(w[6], 0);
            fix.Satellites = Nmea.Int(w[7], -1);
            fix.Hdop = Nmea.Dbl(w[8]);
            fix.Altitude = Nmea.Dbl(w[9]);
            fix.AgeSeconds = Nmea.Dbl(w[13]);
            LastGga = sentence.TrimEnd('\r', '\n');
            return fix;
        }

        private void ParseVtg(string[] w)
        {
            // $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K,A
            if (w.Length > 7)
            {
                double kmh = Nmea.Dbl(w[7]);
                if (!double.IsNaN(kmh)) _speedKmh = kmh;
            }
            double cog = Nmea.Dbl(w[1]);
            if (!double.IsNaN(cog)) _headingTrue = cog;
        }

        private void ParseRmc(string[] w)
        {
            // $GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W
            if (w.Length < 9 || w[2] != "A") return;
            double knots = Nmea.Dbl(w[7]);
            if (!double.IsNaN(knots)) _speedKmh = knots * 1.852;
            double cog = Nmea.Dbl(w[8]);
            if (!double.IsNaN(cog)) _headingTrue = cog;
        }

        private void ParseHdt(string[] w)
        {
            double hdg = Nmea.Dbl(w[1]);
            if (!double.IsNaN(hdg)) _headingDual = hdg;
        }

        // PANDA (single antenna + IMU from the AgOpenGPS firmware) and PAOGI (dual antenna):
        // time, lat, N/S, lon, E/W, quality, sats, hdop, altitude, age, speed knots,
        // heading, roll, pitch, yaw rate. PANDA sends heading and roll in tenths of a degree.
        private GnssFix ParsePandaOrOgi(string[] w, bool isOgi, DateTime receivedUtc)
        {
            var fix = NewFix(receivedUtc);
            fix.UtcTimeOfDay = Nmea.ParseTime(w[1]);
            fix.Latitude = Nmea.ParseLatLon(w[2], w[3], 2);
            fix.Longitude = Nmea.ParseLatLon(w[4], w[5], 3);
            fix.FixQuality = Nmea.Int(w[6], 0);
            fix.Satellites = Nmea.Int(w[7], -1);
            fix.Hdop = Nmea.Dbl(w[8]);
            fix.Altitude = Nmea.Dbl(w[9]);
            fix.AgeSeconds = Nmea.Dbl(w[10]);
            double knots = Nmea.Dbl(w[11]);
            if (!double.IsNaN(knots)) fix.SpeedKmh = knots * 1.852;

            double heading = Nmea.Dbl(w[12]);
            double roll = Nmea.Dbl(w[13]);
            if (isOgi)
            {
                fix.HeadingDual = heading;
                fix.Roll = roll;
            }
            else
            {
                // 65535 / 32767 mean "no IMU".
                if (!double.IsNaN(heading) && heading < 65535) fix.HeadingDual = heading * 0.1;
                if (!double.IsNaN(roll) && roll < 32767) fix.Roll = roll * 0.1;
            }

            // Sentence built by the receiver from its own position: a GGA for NTRIP can be derived.
            LastGga = null;
            return fix;
        }

        private GnssFix NewFix(DateTime receivedUtc)
        {
            return new GnssFix
            {
                Source = FixSource.Nmea,
                ReceivedUtc = receivedUtc,
                SpeedKmh = _speedKmh,
                HeadingTrue = _headingTrue,
                HeadingDual = _headingDual,
            };
        }
    }

    /// <summary>Small NMEA helpers.</summary>
    public static class Nmea
    {
        private static readonly CultureInfo Inv = CultureInfo.InvariantCulture;

        /// <summary>True when the sentence has a correct "*hh" checksum. Sentences without one are rejected.</summary>
        public static bool ChecksumOk(string sentence)
        {
            if (string.IsNullOrEmpty(sentence) || sentence[0] != '$') return false;
            string s = sentence.TrimEnd('\r', '\n');
            int star = s.LastIndexOf('*');
            if (star < 1 || star + 3 != s.Length) return false;

            byte sum = 0;
            for (int i = 1; i < star; i++) sum ^= (byte)s[i];

            return int.TryParse(s.Substring(star + 1, 2), NumberStyles.HexNumber, Inv, out int given) && given == sum;
        }

        /// <summary>Returns the "*hh" checksum (without the star) for the part between '$' and '*'.</summary>
        public static string Checksum(string body)
        {
            byte sum = 0;
            int start = body.Length > 0 && body[0] == '$' ? 1 : 0;
            for (int i = start; i < body.Length; i++) sum ^= (byte)body[i];
            return sum.ToString("X2", Inv);
        }

        internal static string StripChecksum(string sentence)
        {
            string s = sentence.TrimEnd('\r', '\n');
            int star = s.LastIndexOf('*');
            return star > 0 ? s.Substring(0, star) : s;
        }

        /// <summary>"4807.038","N" -> 48.1173. <paramref name="degDigits"/> is 2 for latitude, 3 for longitude.</summary>
        internal static double ParseLatLon(string value, string hemisphere, int degDigits)
        {
            if (string.IsNullOrEmpty(value) || value.Length < degDigits + 2) return double.NaN;
            int dot = value.IndexOf('.');
            int minStart = (dot < 0 ? value.Length : dot) - 2;
            if (minStart < 1) return double.NaN;

            if (!double.TryParse(value.Substring(0, minStart), NumberStyles.Float, Inv, out double deg)) return double.NaN;
            if (!double.TryParse(value.Substring(minStart), NumberStyles.Float, Inv, out double min)) return double.NaN;

            double result = deg + min / 60.0;
            if (hemisphere == "S" || hemisphere == "W") result = -result;
            return result;
        }

        internal static TimeSpan? ParseTime(string hhmmss)
        {
            if (string.IsNullOrEmpty(hhmmss) || hhmmss.Length < 6) return null;
            if (!int.TryParse(hhmmss.Substring(0, 2), NumberStyles.None, Inv, out int h)) return null;
            if (!int.TryParse(hhmmss.Substring(2, 2), NumberStyles.None, Inv, out int m)) return null;
            if (!double.TryParse(hhmmss.Substring(4), NumberStyles.Float, Inv, out double s)) return null;
            if (h > 23 || m > 59 || s >= 61) return null;
            return new TimeSpan(h, m, 0) + TimeSpan.FromSeconds(s);
        }

        internal static double Dbl(string s)
        {
            return double.TryParse(s, NumberStyles.Float, Inv, out double v) ? v : double.NaN;
        }

        internal static int Int(string s, int fallback)
        {
            return double.TryParse(s, NumberStyles.Float, Inv, out double v) ? (int)v : fallback;
        }
    }
}

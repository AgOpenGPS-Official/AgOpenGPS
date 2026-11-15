using System;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>Where a fix came from on the Bluetooth link.</summary>
    public enum FixSource
    {
        None,
        /// <summary>NMEA sentences straight from the receiver (GGA + VTG/RMC/HDT, or PANDA/PAOGI).</summary>
        Nmea,
        /// <summary>AgOpenGPS PGN 0xD6 packets, e.g. from a Raspberry Pi running an AgIO-like bridge.</summary>
        Pgn,
    }

    /// <summary>
    /// One position solution, combined from whatever the link delivers. Values that the source
    /// did not send are NaN (doubles) or -1 (counts).
    /// </summary>
    public sealed class GnssFix
    {
        public double Latitude = double.NaN;
        public double Longitude = double.NaN;
        /// <summary>Metres above mean sea level.</summary>
        public double Altitude = double.NaN;
        /// <summary>GGA fix quality: 0 invalid, 1 GPS, 2 DGPS, 4 RTK fix, 5 RTK float, ...</summary>
        public int FixQuality;
        public int Satellites = -1;
        public double Hdop = double.NaN;
        /// <summary>Seconds since the last correction the receiver used.</summary>
        public double AgeSeconds = double.NaN;
        public double SpeedKmh = double.NaN;
        /// <summary>Course over ground (single antenna), degrees true.</summary>
        public double HeadingTrue = double.NaN;
        /// <summary>Heading from a dual antenna receiver, degrees true.</summary>
        public double HeadingDual = double.NaN;
        /// <summary>Roll in degrees, positive = right side down.</summary>
        public double Roll = double.NaN;
        /// <summary>UTC time of the fix as reported by the receiver, if known.</summary>
        public TimeSpan? UtcTimeOfDay;
        public FixSource Source;
        /// <summary>When the tablet received it (UTC).</summary>
        public DateTime ReceivedUtc;

        public bool HasPosition => !double.IsNaN(Latitude) && !double.IsNaN(Longitude) && FixQuality > 0;

        public GnssFix Clone() => (GnssFix)MemberwiseClone();

        public static string FixQualityName(int quality)
        {
            switch (quality)
            {
                case 0: return "No fix";
                case 1: return "GPS";
                case 2: return "DGPS";
                case 3: return "PPS";
                case 4: return "RTK Fix";
                case 5: return "RTK Float";
                case 6: return "Dead reckoning";
                case 7: return "Manual";
                case 8: return "Simulation";
                default: return "Fix " + quality;
            }
        }
    }
}

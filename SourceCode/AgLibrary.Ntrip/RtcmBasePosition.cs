using System;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// The base station position in RTCM 1005 and 1006: an ECEF position (X, Y, Z) of the antenna reference
    /// point in 0.1 mm steps, turned into latitude and longitude. Pure logic.
    /// </summary>
    internal static class RtcmBasePosition
    {
        // Message layout, in bits from the start of the frame (3 bytes of header first):
        // type 12, station 12, ITRF year 6, 4 indicators, X 38, 1+1, Y 38, 2, Z 38.
        private const int HeaderBits = 24;
        private const int XBit = HeaderBits + 34;
        private const int YBit = HeaderBits + 74;
        private const int ZBit = HeaderBits + 114;
        private const int CoordinateBits = 38;
        private const int MinimumBytes = (HeaderBits + 152 + 7) / 8;

        /// <param name="message">A whole frame, header and checksum included.</param>
        public static bool TryDecode(ReadOnlySpan<byte> message, out double latitude, out double longitude)
        {
            latitude = longitude = double.NaN;
            if (message.Length < MinimumBytes) return false;

            double x = Signed(message, XBit) * 0.0001;
            double y = Signed(message, YBit) * 0.0001;
            double z = Signed(message, ZBit) * 0.0001;

            // A base station that has no position yet sends zeros.
            if (x == 0 && y == 0 && z == 0) return false;

            ToGeodetic(x, y, z, out latitude, out longitude);
            return true;
        }

        private static long Signed(ReadOnlySpan<byte> message, int firstBit)
        {
            long value = 0;
            for (int i = 0; i < CoordinateBits; i++)
            {
                int bit = firstBit + i;
                value = (value << 1) | (long)((message[bit >> 3] >> (7 - (bit & 7))) & 1);
            }
            // two's complement of 38 bits
            if ((value & (1L << (CoordinateBits - 1))) != 0) value -= 1L << CoordinateBits;
            return value;
        }

        /// <summary>WGS84 ECEF to latitude and longitude in degrees.</summary>
        public static void ToGeodetic(double x, double y, double z, out double latitude, out double longitude)
        {
            const double a = 6378137.0;
            const double f = 1 / 298.257223563;
            double e2 = f * (2 - f);

            double p = Math.Sqrt(x * x + y * y);
            double lat = Math.Atan2(z, p * (1 - e2));
            for (int i = 0; i < 6; i++)
            {
                double sin = Math.Sin(lat);
                double n = a / Math.Sqrt(1 - e2 * sin * sin);
                lat = Math.Atan2(z + e2 * n * sin, p);
            }

            latitude = lat * 180.0 / Math.PI;
            longitude = Math.Atan2(y, x) * 180.0 / Math.PI;
        }

        /// <summary>Great-circle distance in kilometres.</summary>
        public static double DistanceKm(double latitude1, double longitude1, double latitude2, double longitude2)
        {
            const double R = 6371.0;
            double dLat = (latitude2 - latitude1) * Math.PI / 180.0;
            double dLon = (longitude2 - longitude1) * Math.PI / 180.0;
            double h = Math.Sin(dLat / 2) * Math.Sin(dLat / 2)
                + Math.Cos(latitude1 * Math.PI / 180.0) * Math.Cos(latitude2 * Math.PI / 180.0) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            return 2 * R * Math.Asin(Math.Min(1.0, Math.Sqrt(h)));
        }
    }
}

using System;

namespace AgOpenGPS.Mobile.Core
{
    /// <summary>
    /// AgOpenGPS PGN frames: [0x80, 0x81, source, pgn, length, data..., crc] where crc is the
    /// byte sum of everything from the source byte to the end of the data. The same format the Windows
    /// AgOpenGPS and AgIO use, so existing modules and bridges keep working.
    /// </summary>
    public static class Pgn
    {
        public const byte Header1 = 0x80;
        public const byte Header2 = 0x81;
        public const byte GpsPosition = 0xD6;
        /// <summary>Data length of PGN 0xD6 (bytes 5..55 of the frame, as AgIO sends it).</summary>
        public const int GpsPositionLength = 51;

        public static byte Crc(byte[] frame, int length)
        {
            byte sum = 0;
            for (int i = 2; i < length; i++) sum += frame[i];
            return sum;
        }

        /// <summary>Checks header, length and CRC of a complete frame.</summary>
        public static bool IsValid(byte[] frame)
        {
            if (frame == null || frame.Length < 6 || frame[0] != Header1 || frame[1] != Header2) return false;
            int end = frame[4] + 5;
            return frame.Length > end && frame[end] == Crc(frame, end);
        }

        /// <summary>Reads a PGN 0xD6 frame the way the Windows AgOpenGPS (FormGPS.ReceiveFromAgIO) does. Null if it is not one.</summary>
        public static GnssFix ParseGpsPosition(byte[] frame, DateTime receivedUtc)
        {
            if (!IsValid(frame) || frame[3] != GpsPosition || frame.Length < 56) return null;

            double lon = BitConverter.ToDouble(frame, 5);
            double lat = BitConverter.ToDouble(frame, 13);
            if (lon == double.MaxValue || lat == double.MaxValue) return null;

            var fix = new GnssFix
            {
                Source = FixSource.Pgn,
                ReceivedUtc = receivedUtc,
                Latitude = lat,
                Longitude = lon,
                HeadingDual = F(frame, 21),
                HeadingTrue = F(frame, 25),
                SpeedKmh = F(frame, 29),
                Roll = F(frame, 33),
                Altitude = F(frame, 37),
            };

            ushort sats = BitConverter.ToUInt16(frame, 41);
            if (sats != ushort.MaxValue) fix.Satellites = sats;
            byte quality = frame[43];
            fix.FixQuality = quality == byte.MaxValue ? 0 : quality;
            ushort hdop = BitConverter.ToUInt16(frame, 44);
            if (hdop != ushort.MaxValue) fix.Hdop = hdop * 0.01;
            ushort age = BitConverter.ToUInt16(frame, 46);
            if (age != ushort.MaxValue) fix.AgeSeconds = age * 0.01;

            // An external IMU heading/roll (bytes 48..51) wins over what the receiver sent.
            ushort imuHeading = BitConverter.ToUInt16(frame, 48);
            if (imuHeading != ushort.MaxValue) fix.HeadingDual = imuHeading * 0.1;
            short imuRoll = BitConverter.ToInt16(frame, 50);
            if (imuRoll != short.MaxValue) fix.Roll = imuRoll * 0.1;

            return fix;
        }

        /// <summary>Builds a PGN 0xD6 frame (the same layout AgIO sends). Used by tests and the simulator.</summary>
        public static byte[] BuildGpsPosition(GnssFix fix)
        {
            var f = new byte[5 + GpsPositionLength + 1];
            f[0] = Header1; f[1] = Header2; f[2] = 0x7F; f[3] = GpsPosition; f[4] = GpsPositionLength;

            Put(f, 5, double.IsNaN(fix.Longitude) ? double.MaxValue : fix.Longitude);
            Put(f, 13, double.IsNaN(fix.Latitude) ? double.MaxValue : fix.Latitude);
            Put(f, 21, ToF(fix.HeadingDual));
            Put(f, 25, ToF(fix.HeadingTrue));
            Put(f, 29, ToF(fix.SpeedKmh));
            Put(f, 33, ToF(fix.Roll));
            Put(f, 37, ToF(fix.Altitude));
            Put(f, 41, fix.Satellites < 0 ? ushort.MaxValue : (ushort)fix.Satellites);
            f[43] = (byte)fix.FixQuality;
            Put(f, 44, double.IsNaN(fix.Hdop) ? ushort.MaxValue : (ushort)Math.Round(fix.Hdop * 100));
            Put(f, 46, double.IsNaN(fix.AgeSeconds) ? ushort.MaxValue : (ushort)Math.Round(fix.AgeSeconds * 100));
            Put(f, 48, ushort.MaxValue);
            Put(f, 50, short.MaxValue);
            Put(f, 52, short.MaxValue);
            Put(f, 54, short.MaxValue);
            f[f.Length - 1] = Crc(f, f.Length - 1);
            return f;
        }

        private static double F(byte[] b, int at)
        {
            float v = BitConverter.ToSingle(b, at);
            return v == float.MaxValue || v == float.MinValue ? double.NaN : v;
        }

        private static float ToF(double v) => double.IsNaN(v) ? float.MaxValue : (float)v;

        private static void Put(byte[] f, int at, double v) => BitConverter.GetBytes(v).CopyTo(f, at);
        private static void Put(byte[] f, int at, float v) => BitConverter.GetBytes(v).CopyTo(f, at);
        private static void Put(byte[] f, int at, ushort v) => BitConverter.GetBytes(v).CopyTo(f, at);
        private static void Put(byte[] f, int at, short v) => BitConverter.GetBytes(v).CopyTo(f, at);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using AgOpenGPS.Mobile.Core;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    internal static class Sentences
    {
        /// <summary>Adds "*hh\r\n" to a sentence body like "$GPGGA,...".</summary>
        public static string With(string body) => body + "*" + Nmea.Checksum(body) + "\r\n";

        public static readonly string Gga = With("$GNGGA,123519.00,4807.038,N,01131.000,E,4,12,0.9,545.4,M,46.9,M,1.2,0000");
        public static readonly string Vtg = With("$GNVTG,054.7,T,034.4,M,005.5,N,010.2,K,A");
        public static readonly string Rmc = With("$GPRMC,123519,A,4807.038,N,01131.000,W,022.4,084.4,230394,003.1,W");
        public static readonly string Hdt = With("$GNHDT,271.5,T");
        public static readonly string Panda = With("$PANDA,123519.00,4807.038,S,01131.000,E,5,10,1.1,300.0,2.0,10.0,1234,-25,0,0");
    }

    public class NmeaTests
    {
        private static readonly DateTime T0 = new DateTime(2026, 11, 15, 12, 35, 19, DateTimeKind.Utc);

        [Test]
        public void Checksum_IsVerified()
        {
            Assert.That(Nmea.ChecksumOk(Sentences.Gga), Is.True);
            Assert.That(Nmea.ChecksumOk(Sentences.Gga.Replace("4807", "4808")), Is.False);
            Assert.That(Nmea.ChecksumOk("$GPGGA,1,2,3"), Is.False, "no checksum");
        }

        [Test]
        public void Gga_GivesPositionQualityAndAge()
        {
            var acc = new NmeaAccumulator();
            GnssFix fix = acc.Process(Sentences.Gga, T0);

            Assert.That(fix, Is.Not.Null);
            Assert.That(fix.Latitude, Is.EqualTo(48 + 7.038 / 60).Within(1e-9));
            Assert.That(fix.Longitude, Is.EqualTo(11 + 31.0 / 60).Within(1e-9));
            Assert.That(fix.FixQuality, Is.EqualTo(4));
            Assert.That(GnssFix.FixQualityName(fix.FixQuality), Is.EqualTo("RTK Fix"));
            Assert.That(fix.Satellites, Is.EqualTo(12));
            Assert.That(fix.Hdop, Is.EqualTo(0.9).Within(1e-9));
            Assert.That(fix.Altitude, Is.EqualTo(545.4).Within(1e-9));
            Assert.That(fix.AgeSeconds, Is.EqualTo(1.2).Within(1e-9));
            Assert.That(fix.UtcTimeOfDay, Is.EqualTo(new TimeSpan(12, 35, 19)));
            Assert.That(fix.Source, Is.EqualTo(FixSource.Nmea));
            Assert.That(acc.LastGga, Is.EqualTo(Sentences.Gga.TrimEnd()));
        }

        [Test]
        public void VtgAndHdt_FillTheNextGga()
        {
            var acc = new NmeaAccumulator();
            Assert.That(acc.Process(Sentences.Vtg, T0), Is.Null);
            Assert.That(acc.Process(Sentences.Hdt, T0), Is.Null);
            GnssFix fix = acc.Process(Sentences.Gga, T0);

            Assert.That(fix.SpeedKmh, Is.EqualTo(10.2).Within(1e-9));
            Assert.That(fix.HeadingTrue, Is.EqualTo(54.7).Within(1e-9));
            Assert.That(fix.HeadingDual, Is.EqualTo(271.5).Within(1e-9));
        }

        [Test]
        public void Rmc_GivesSpeedInKmh()
        {
            var acc = new NmeaAccumulator();
            acc.Process(Sentences.Rmc, T0);
            GnssFix fix = acc.Process(Sentences.Gga, T0);
            Assert.That(fix.SpeedKmh, Is.EqualTo(22.4 * 1.852).Within(1e-9));
            Assert.That(fix.HeadingTrue, Is.EqualTo(84.4).Within(1e-9));
        }

        [Test]
        public void Panda_UsesTenthsForImuAndSouthernHemisphere()
        {
            GnssFix fix = new NmeaAccumulator().Process(Sentences.Panda, T0);

            Assert.That(fix.Latitude, Is.EqualTo(-(48 + 7.038 / 60)).Within(1e-9));
            Assert.That(fix.FixQuality, Is.EqualTo(5));
            Assert.That(fix.SpeedKmh, Is.EqualTo(18.52).Within(1e-9));
            Assert.That(fix.HeadingDual, Is.EqualTo(123.4).Within(1e-9));
            Assert.That(fix.Roll, Is.EqualTo(-2.5).Within(1e-9));
        }

        [Test]
        public void EmptyFields_DoNotThrow()
        {
            string noFix = Sentences.With("$GPGGA,,,,,,0,00,,,M,,M,,");
            GnssFix fix = new NmeaAccumulator().Process(noFix, T0);
            Assert.That(fix, Is.Not.Null);
            Assert.That(fix.HasPosition, Is.False);
        }
    }

    public class PgnTests
    {
        [Test]
        public void GpsPosition_RoundTrips()
        {
            var src = new GnssFix
            {
                Latitude = 45.8150108, Longitude = 15.9819189, Altitude = 120.5f, FixQuality = 4,
                Satellites = 18, Hdop = 0.7, AgeSeconds = 1.5, SpeedKmh = 8.25f, HeadingTrue = 92.5f,
            };
            byte[] frame = Pgn.BuildGpsPosition(src);

            Assert.That(frame.Length, Is.EqualTo(57), "AgIO sends 57 bytes");
            Assert.That(Pgn.IsValid(frame), Is.True);

            GnssFix fix = Pgn.ParseGpsPosition(frame, DateTime.UtcNow);
            Assert.That(fix.Latitude, Is.EqualTo(src.Latitude));
            Assert.That(fix.Longitude, Is.EqualTo(src.Longitude));
            Assert.That(fix.FixQuality, Is.EqualTo(4));
            Assert.That(fix.Satellites, Is.EqualTo(18));
            Assert.That(fix.Hdop, Is.EqualTo(0.7).Within(1e-9));
            Assert.That(fix.AgeSeconds, Is.EqualTo(1.5).Within(1e-9));
            Assert.That(fix.SpeedKmh, Is.EqualTo(8.25).Within(1e-6));
            Assert.That(fix.HeadingTrue, Is.EqualTo(92.5).Within(1e-6));
            Assert.That(double.IsNaN(fix.HeadingDual), Is.True, "not sent");
            Assert.That(double.IsNaN(fix.Roll), Is.True, "not sent");
            Assert.That(fix.Source, Is.EqualTo(FixSource.Pgn));
        }

        [Test]
        public void BadCrc_IsRejected()
        {
            byte[] frame = Pgn.BuildGpsPosition(new GnssFix { Latitude = 1, Longitude = 2, FixQuality = 1 });
            frame[10] ^= 0x01;
            Assert.That(Pgn.ParseGpsPosition(frame, DateTime.UtcNow), Is.Null);
        }
    }

    public class DecoderTests
    {
        private static byte[] MixedStream(out int pgnCount, out int nmeaCount)
        {
            var bytes = new List<byte>();
            bytes.AddRange(new byte[] { 0xB5, 0x62, 0x01, 0x07, 0x00 }); // UBX noise
            bytes.AddRange(Encoding.ASCII.GetBytes(Sentences.Vtg));
            // A PGN frame whose binary body happens to contain '$' and 0x80 bytes.
            var fix = new GnssFix { Latitude = BitConverter.ToDouble(new byte[] { 0x24, 0x80, 0x24, 0x0A, 0x0D, 0x80, 0x81, 0x40 }, 0), Longitude = 15.98, FixQuality = 4 };
            bytes.AddRange(Pgn.BuildGpsPosition(fix));
            bytes.AddRange(Encoding.ASCII.GetBytes(Sentences.Gga));
            bytes.AddRange(new byte[] { 0xD3, 0x00, 0x13 }); // start of an echoed RTCM frame
            bytes.AddRange(Pgn.BuildGpsPosition(new GnssFix { Latitude = 45, Longitude = 16, FixQuality = 1 }));
            bytes.AddRange(Encoding.ASCII.GetBytes("$GPGGA,broken line without end"));
            bytes.AddRange(Encoding.ASCII.GetBytes(Sentences.Hdt));
            pgnCount = 2;
            nmeaCount = 3;
            return bytes.ToArray();
        }

        [TestCase(1)]
        [TestCase(3)]
        [TestCase(17)]
        [TestCase(4096)]
        public void MixedStream_IsSplitAtAnyChunkSize(int chunk)
        {
            byte[] stream = MixedStream(out int pgnCount, out int nmeaCount);
            var decoder = new GnssStreamDecoder();
            var sentences = new List<string>();
            var frames = new List<byte[]>();
            decoder.SentenceReceived += sentences.Add;
            decoder.PgnReceived += frames.Add;

            for (int i = 0; i < stream.Length; i += chunk)
                decoder.Feed(stream, i, Math.Min(chunk, stream.Length - i));

            Assert.That(frames, Has.Count.EqualTo(pgnCount));
            Assert.That(frames, Has.All.Matches<byte[]>(Pgn.IsValid));
            Assert.That(sentences, Has.Count.EqualTo(nmeaCount));
            Assert.That(sentences, Has.All.Matches<string>(Nmea.ChecksumOk));
            Assert.That(decoder.SkippedBytes, Is.GreaterThan(0));
        }

        [Test]
        public void CorruptPgn_IsCountedNotDelivered()
        {
            byte[] frame = Pgn.BuildGpsPosition(new GnssFix { Latitude = 45, Longitude = 16, FixQuality = 1 });
            frame[20] ^= 0xFF;
            var decoder = new GnssStreamDecoder();
            int delivered = 0;
            decoder.PgnReceived += _ => delivered++;
            decoder.Feed(frame, 0, frame.Length);

            Assert.That(delivered, Is.EqualTo(0));
            Assert.That(decoder.BadPgnFrames, Is.EqualTo(1));
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AgLibrary.Ntrip;
using AgOpenGPS.Mobile.Core;
using NUnit.Framework;

namespace AgOpenGPS.Mobile.Core.Tests
{
    public class SessionTests
    {
        [Test]
        public async Task NmeaLink_GivesFixAndReceiverGgaForCaster()
        {
            DateTime now = DateTime.UtcNow;
            var input = new MemoryStream(Encoding.ASCII.GetBytes(Sentences.Vtg + Sentences.Gga));
            var output = new MemoryStream();
            var session = new GnssSession(input, output, () => now);
            var fixes = new List<GnssFix>();
            session.FixUpdated += fixes.Add;

            await session.RunAsync(CancellationToken.None);

            Assert.That(fixes, Has.Count.EqualTo(1));
            Assert.That(fixes[0].FixQuality, Is.EqualTo(4));
            Assert.That(session.GgaForCaster(), Is.EqualTo(Sentences.Gga), "with CR/LF, casters wait for the line end");

            SessionStats stats = session.GetStats();
            Assert.That(stats.Sentences, Is.EqualTo(2));
            Assert.That(stats.Latest.SpeedKmh, Is.EqualTo(10.2).Within(1e-9));

            Assert.That(session.Write(new byte[] { 0xD3, 0x00 }, 0, 2), Is.True);
            Assert.That(output.ToArray(), Is.EqualTo(new byte[] { 0xD3, 0x00 }));
        }

        [Test]
        public async Task PgnLink_BuildsGgaForCaster()
        {
            DateTime now = DateTime.UtcNow;
            byte[] frame = Pgn.BuildGpsPosition(new GnssFix
            {
                Latitude = 45.5, Longitude = 16.25, FixQuality = 5, Satellites = 11, Hdop = 0.8, Altitude = 100,
            });
            var session = new GnssSession(new MemoryStream(frame), null, () => now);

            await session.RunAsync(CancellationToken.None);

            string gga = session.GgaForCaster();
            Assert.That(gga, Does.StartWith("$GPGGA,").And.EndWith("\r\n"));
            Assert.That(gga, Does.Contain(",4530.00000,N,01615.00000,E,5,11,"));
            Assert.That(session.GetStats().PgnFrames, Is.EqualTo(1));
        }

        [Test]
        public async Task StaleFix_GivesNoGga()
        {
            DateTime now = DateTime.UtcNow;
            var session = new GnssSession(new MemoryStream(Encoding.ASCII.GetBytes(Sentences.Gga)), null, () => now);
            await session.RunAsync(CancellationToken.None);
            now = now.AddSeconds(10);
            Assert.That(session.GgaForCaster(), Is.Null);
        }
    }

    public class CorrectionForwarderTests
    {
        // CRC-24Q as used by RTCM 3.
        private static uint Crc24Q(byte[] data, int count)
        {
            uint crc = 0;
            for (int i = 0; i < count; i++)
            {
                crc ^= (uint)data[i] << 16;
                for (int b = 0; b < 8; b++)
                {
                    crc <<= 1;
                    if ((crc & 0x1000000) != 0) crc ^= 0x1864CFB;
                }
            }
            return crc & 0xFFFFFF;
        }

        private static byte[] RtcmFrame(int messageType, int payloadLength, byte fill)
        {
            var f = new byte[3 + payloadLength + 3];
            f[0] = 0xD3;
            f[1] = (byte)(payloadLength >> 8);
            f[2] = (byte)payloadLength;
            for (int i = 0; i < payloadLength; i++) f[3 + i] = fill;
            f[3] = (byte)(messageType >> 4);
            f[4] = (byte)((messageType & 0x0F) << 4 | (fill & 0x0F));
            uint crc = Crc24Q(f, 3 + payloadLength);
            f[3 + payloadLength] = (byte)(crc >> 16);
            f[4 + payloadLength] = (byte)(crc >> 8);
            f[5 + payloadLength] = (byte)crc;
            return f;
        }

        [Test]
        public async Task CasterCorrections_ReachTheDevice_AndGgaReachesTheCaster()
        {
            byte[] station = RtcmFrame(1005, 19, 0x11);
            byte[] gpsObs = RtcmFrame(1077, 300, 0x22);
            byte[] gloObs = RtcmFrame(1087, 200, 0x33);
            byte[] expected = station.Concat(gpsObs).Concat(gloObs).ToArray();

            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            string request = null;
            var ggaFromClient = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(20));
            Task caster = Task.Run(async () =>
            {
                using TcpClient client = await listener.AcceptTcpClientAsync(cts.Token);
                NetworkStream s = client.GetStream();
                var buf = new byte[4096];
                var req = new StringBuilder();
                while (!req.ToString().Contains("\r\n\r\n"))
                {
                    int n = await s.ReadAsync(buf, cts.Token);
                    if (n == 0) return;
                    req.Append(Encoding.ASCII.GetString(buf, 0, n));
                }
                request = req.ToString();
                byte[] header = Encoding.ASCII.GetBytes("ICY 200 OK\r\n\r\n");
                await s.WriteAsync(header, cts.Token);
                await s.WriteAsync(expected, cts.Token);

                // NTRIP 1.0 sends the GGA after the caster's answer.
                var after = new StringBuilder();
                while (!after.ToString().Contains("\r\n"))
                {
                    int n = await s.ReadAsync(buf, cts.Token);
                    if (n == 0) return;
                    after.Append(Encoding.ASCII.GetString(buf, 0, n));
                }
                ggaFromClient.TrySetResult(after.ToString().Trim());
                await Task.Delay(Timeout.Infinite, cts.Token);
            });

            var received = new List<byte>();
            var gotAll = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            string gga = Sentences.Gga;
            var forwarder = new CorrectionForwarder(
                new NtripProfile("127.0.0.1", port, "TEST", "user", "pass", NtripVersion.V1),
                chunk =>
                {
                    lock (received)
                    {
                        received.AddRange(chunk);
                        if (received.Count >= expected.Length) gotAll.TrySetResult(true);
                    }
                    return true;
                },
                () => gga,
                intervalMs: 5);

            Task run = forwarder.RunAsync(cts.Token);
            Task first = await Task.WhenAny(gotAll.Task, Task.Delay(TimeSpan.FromSeconds(15)));
            Assert.That(first, Is.SameAs(gotAll.Task), "corrections did not arrive; status: " + forwarder.Status.Headline);

            lock (received) Assert.That(received.ToArray(), Is.EqualTo(expected));
            Assert.That(forwarder.BytesSent, Is.EqualTo(expected.Length));
            Assert.That(request, Does.StartWith("GET /TEST HTTP/1."));
            Assert.That(request, Does.Contain("Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("user:pass"))));
            Assert.That(await ggaFromClient.Task.WaitAsync(TimeSpan.FromSeconds(10)), Is.EqualTo(gga.TrimEnd()), "VRS casters need the GGA");

            cts.Cancel();
            try { await run; } catch (OperationCanceledException) { }
            try { await caster; } catch (OperationCanceledException) { }
            listener.Stop();
        }
    }
}

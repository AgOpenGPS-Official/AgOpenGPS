using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class SourcetableTests
    {
        private const string Table =
            "STR;AGROORSOLIC;Ostra Luka;RTCM 3.3;1004(1),1005(10),1077(1);2;GPS+GLO+GAL+BeiDou+SBAS;SNIP;BIH;45.01;18.60;1;0;SNIP;none;B;N;11580;\r\n" +
            "STR;WACHTUM_NLD;WACHTUM_NLD;rtcm3;1004,1005(10);;GPS+GLO+GAL+BDS+QZS;NONE;NLD;52.73727820;6.75065200;0;0;RTKBase_U-blox_ZED-F9P,2.6.2;NONE;N;N;;\r\n" +
            "CAS;rtk2go.com;2101;RTK2go;SubCarrier Systems Corp;0;USA;40.71;-74.00;\r\n" +
            "NET;SNIP;SubCarrier;B;N;https://www.snip.com;;;\r\n" +
            "STR;;broken line\r\n" +
            "ENDSOURCETABLE\r\n";

        [Test]
        public void Parse_ReadsMountpoints_AndSkipsOtherLines()
        {
            List<MountpointInfo> list = Sourcetable.Parse(Table);
            Assert.That(list.Count, Is.EqualTo(2));

            MountpointInfo a = list[0];
            Assert.That(a.Mount, Is.EqualTo("AGROORSOLIC"));
            Assert.That(a.Name, Is.EqualTo("Ostra Luka"));
            Assert.That(a.Systems, Is.EqualTo("GPS+GLO+GAL+BeiDou+SBAS"));
            Assert.That(a.Country, Is.EqualTo("BIH"));
            Assert.That(a.Latitude, Is.EqualTo(45.01).Within(1e-9));
            Assert.That(a.Longitude, Is.EqualTo(18.60).Within(1e-9));
            Assert.That(a.NeedsPosition, Is.True);

            Assert.That(list[1].Mount, Is.EqualTo("WACHTUM_NLD"));
            Assert.That(list[1].NeedsPosition, Is.False);
            Assert.That(list[1].Latitude, Is.EqualTo(52.7372782).Within(1e-9));
        }

        [Test]
        public void Parse_EmptyOrNull_GivesNothing()
        {
            Assert.That(Sourcetable.Parse(null), Is.Empty);
            Assert.That(Sourcetable.Parse(""), Is.Empty);
        }

        [Test]
        public void Distance_IsInKilometres_AndNaNWithoutAPosition()
        {
            var wachtum = new MountpointInfo { Latitude = 52.7372782, Longitude = 6.7506520 };
            Assert.That(wachtum.DistanceKm(52.7372782, 6.7506520), Is.EqualTo(0).Within(0.01));
            // Wachtum to Deurne is roughly 100 km
            Assert.That(wachtum.DistanceKm(51.46, 5.79), Is.InRange(120, 160));
            Assert.That(new MountpointInfo().DistanceKm(52, 6), Is.NaN);
        }

        private static Task<SourcetableResult> Fetch(FakeCaster caster, NtripVersion v = NtripVersion.V2, string user = "")
        {
            var profile = new NtripProfile("127.0.0.1", caster.Port, "", user, "", v);
            return Sourcetable.FetchAsync(profile, Harness.QuickTimeouts(), CancellationToken.None);
        }

        [Test]
        public void Fetch_V2_ReadsTheTable()
        {
            using (var caster = new FakeCaster().Script(s => s.SendAsync("HTTP/1.1 200 OK\r\nContent-Type: gnss/sourcetable\r\nConnection: close\r\n\r\n" + Table)))
            {
                SourcetableResult r = Fetch(caster).Result;
                Assert.That(r.Success, Is.True);
                Assert.That(r.Mountpoints.Count, Is.EqualTo(2));
                Assert.That(caster.Requests[0], Does.StartWith("GET / HTTP/1.1"));
            }
        }

        [Test]
        public void Fetch_StopsAtEndSourcetable_EvenWhenTheConnectionStaysOpen()
        {
            using (var caster = new FakeCaster().Script(async s =>
            {
                await s.SendAsync("SOURCETABLE 200 OK\r\nContent-Type: text/plain\r\n\r\n" + Table);
                await s.WaitAsync(8000);
            }))
            {
                var watch = System.Diagnostics.Stopwatch.StartNew();
                SourcetableResult r = Fetch(caster).Result;
                Assert.That(r.Success, Is.True);
                Assert.That(watch.ElapsedMilliseconds, Is.LessThan(3000));
            }
        }

        [Test]
        public void Fetch_Chunked_IsDecoded()
        {
            string half1 = Table.Substring(0, 120);
            string half2 = Table.Substring(120);
            string body = half1.Length.ToString("x") + "\r\n" + half1 + "\r\n" + half2.Length.ToString("x") + "\r\n" + half2 + "\r\n0\r\n\r\n";
            using (var caster = new FakeCaster().Script(s => s.SendAsync("HTTP/1.1 200 OK\r\nContent-Type: gnss/sourcetable\r\nTransfer-Encoding: chunked\r\n\r\n" + body)))
            {
                SourcetableResult r = Fetch(caster).Result;
                Assert.That(r.Success, Is.True);
                Assert.That(r.Mountpoints.Count, Is.EqualTo(2));
            }
        }

        [Test]
        public void Fetch_FallsBackToV1_WhenTheCasterDoesNotUnderstandV2()
        {
            using (var caster = new FakeCaster()
                .Script(s => s.SendAsync("HTTP/1.1 400 Bad Request\r\n\r\n"))
                .Script(s => s.SendAsync("SOURCETABLE 200 OK\r\nContent-Type: text/plain\r\n\r\n" + Table)))
            {
                SourcetableResult r = Fetch(caster, NtripVersion.Auto).Result;
                Assert.That(r.Success, Is.True);
                Assert.That(caster.Requests[1], Does.StartWith("GET / HTTP/1.0"));
            }
        }

        [Test]
        public void Fetch_SendsTheLogin_WhenThereIsOne()
        {
            using (var caster = new FakeCaster().Script(s => s.SendAsync("SOURCETABLE 200 OK\r\n\r\n" + Table)))
            {
                Fetch(caster, NtripVersion.V1, "farmer@example.org").Wait();
                string b64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("farmer@example.org:"));
                Assert.That(caster.Requests[0], Does.Contain("Authorization: Basic " + b64));
            }
        }

        [Test]
        public void Fetch_LoginRefused_SaysSo()
        {
            using (var caster = new FakeCaster().Script(s => s.SendAsync("HTTP/1.1 401 Unauthorized\r\n\r\n")))
            {
                SourcetableResult r = Fetch(caster).Result;
                Assert.That(r.Success, Is.False);
                Assert.That(r.Error.Category, Is.EqualTo(NtripCategory.LoginRejected));
            }
        }

        [Test]
        public void Fetch_NothingListening_SaysTheCasterCannotBeReached()
        {
            int port;
            using (var gone = new FakeCaster()) port = gone.Port;
            var profile = new NtripProfile("127.0.0.1", port, "");
            var timeouts = Harness.QuickTimeouts();
            timeouts.Connect = TimeSpan.FromSeconds(8);
            SourcetableResult r = Sourcetable.FetchAsync(profile, timeouts, CancellationToken.None).Result;
            Assert.That(r.Success, Is.False);
            Assert.That(r.Error.Category, Is.EqualTo(NtripCategory.CasterUnreachable));
        }

        [Test]
        public void Fetch_AMountpointStreamIsNotAList()
        {
            using (var caster = new FakeCaster().Script(s => s.SendAsync("ICY 200 OK\r\n\r\n")))
            {
                SourcetableResult r = Fetch(caster, NtripVersion.V1).Result;
                Assert.That(r.Success, Is.False);
            }
        }
    }

    [TestFixture]
    public class ConnectionCheckTests
    {
        private static byte[] Frames(params int[] types)
        {
            var l = new List<byte>();
            foreach (int t in types) l.AddRange(Rtcm.Message(t, 60));
            return l.ToArray();
        }

        private static Func<FakeSession, Task> Stream(byte[] body, int holdMs = 6000) => async s =>
        {
            await s.SendAsync("HTTP/1.1 200 OK\r\nContent-Type: gnss/data\r\n\r\n");
            for (int i = 0; i < holdMs / 150; i++)
            {
                await s.SendAsync(body);
                await s.WaitAsync(150);
            }
        };

        private static Task<ConnectionCheckResult> Check(FakeCaster caster, List<string> progress = null)
        {
            var profile = new NtripProfile("127.0.0.1", caster.Port, "MOUNT");
            return ConnectionCheck.RunAsync(profile, () => null, text => { if (progress != null) lock (progress) progress.Add(text); },
                CancellationToken.None, Harness.QuickTimeouts());
        }

        [Test]
        public void Works_SaysAllGood_AndTellsTheSteps()
        {
            var progress = new List<string>();
            using (var caster = new FakeCaster().Script(Stream(Frames(1005, 1077, 1087))))
            {
                ConnectionCheckResult r = Check(caster, progress).Result;
                Assert.That(r.Success, Is.True, r.Summary);
                Assert.That(r.Summary, Is.EqualTo("All good: corrections are coming in."));
                Assert.That(r.Lines[0], Does.StartWith("Connecting to MOUNT on 127.0.0.1"));
                Assert.That(string.Join("\n", r.Lines), Does.Contain("The service answered: HTTP/1.1 200 OK."));
                Assert.That(string.Join("\n", r.Lines), Does.Contain("Corrections arrive after"));
                Assert.That(string.Join("\n", r.Lines), Does.Contain("messages received, no errors."));
                lock (progress) Assert.That(progress.Count, Is.GreaterThanOrEqualTo(3), "progress is reported while it runs");
            }
        }

        [Test]
        public void WrongLogin_IsReportedAtOnce_WithoutRetrying()
        {
            using (var caster = new FakeCaster().Script(5, s => s.SendAsync("HTTP/1.1 401 Unauthorized\r\n\r\n")))
            {
                ConnectionCheckResult r = Check(caster).Result;
                Assert.That(r.Success, Is.False);
                Assert.That(r.Summary, Is.EqualTo("User name, password or mountpoint is not correct."));
                Thread.Sleep(300);
                Assert.That(caster.Connections, Is.EqualTo(1), "one try, no retries");
            }
        }

        [Test]
        public void NothingListening_SaysTheService_CannotBeReached()
        {
            int port;
            using (var gone = new FakeCaster()) port = gone.Port;
            var timeouts = Harness.QuickTimeouts();
            timeouts.Connect = TimeSpan.FromSeconds(8);
            var profile = new NtripProfile("127.0.0.1", port, "MOUNT");
            ConnectionCheckResult r = ConnectionCheck.RunAsync(profile, () => null, null, CancellationToken.None, timeouts).Result;
            Assert.That(r.Success, Is.False);
            Assert.That(r.Summary, Does.StartWith("Cannot reach 127.0.0.1"));
        }

        [Test]
        public void Radio_WorksWhenFramesArrive_AndReportsAClosedPort()
        {
            var feed = new FeedStream();
            Task<ConnectionCheckResult> good = ConnectionCheck.RunRadioAsync("COM5", () => feed, null, CancellationToken.None);
            var bytes = new List<byte>(Rtcm.Message(1005, 19));
            for (int i = 0; i < 3; i++) bytes.AddRange(Rtcm.Message(1077, 60));
            feed.Feed(bytes.ToArray());
            Assert.That(good.Result.Success, Is.True, good.Result.Summary);

            ConnectionCheckResult bad = ConnectionCheck.RunRadioAsync("COM9", () => throw new System.IO.IOException("busy"), null, CancellationToken.None).Result;
            Assert.That(bad.Success, Is.False);
            Assert.That(bad.Summary, Is.EqualTo("Cannot open the radio on COM9."));
        }

        [Test]
        public void Cancel_StopsTheCheck()
        {
            using (var caster = new FakeCaster().Script(s => s.WaitAsync(8000)))
            using (var cts = new CancellationTokenSource())
            {
                var profile = new NtripProfile("127.0.0.1", caster.Port, "MOUNT");
                Task<ConnectionCheckResult> run = ConnectionCheck.RunAsync(profile, () => null, null, cts.Token, Harness.QuickTimeouts());
                Thread.Sleep(200);
                cts.Cancel();
                Assert.That(run.Wait(3000), Is.True);
                Assert.That(run.Result.Success, Is.False);
            }
        }
    }
}

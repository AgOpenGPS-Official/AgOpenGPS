using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class RetryPolicyTests
    {
        private static RetryPolicy Policy() => new RetryPolicy(() => 0.5);   // jitter factor exactly 1

        private static List<double> Delays(NtripCategory c, int count)
        {
            var p = Policy();
            var l = new List<double>();
            for (int i = 1; i <= count; i++) l.Add(Math.Round(p.Next(c, i).Delay.TotalSeconds, 1));
            return l;
        }

        [Test] public void Unreachable_5_10_20_40_then_120() => Assert.That(Delays(NtripCategory.CasterUnreachable, 7), Is.EqualTo(new[] { 5.0, 10, 20, 40, 120, 120, 120 }));

        [Test] public void Busy_30_60_120_then_300() => Assert.That(Delays(NtripCategory.CasterBusy, 6), Is.EqualTo(new[] { 30.0, 60, 120, 300, 300, 300 }));

        [Test] public void NoCorrections_10_20_40_then_120() => Assert.That(Delays(NtripCategory.NoCorrections, 5), Is.EqualTo(new[] { 10.0, 20, 40, 120, 120 }));

        [Test]
        public void NotUnderstood_AndTlsFailed_FollowUnreachable()
        {
            Assert.That(Delays(NtripCategory.NotUnderstood, 3), Is.EqualTo(new[] { 5.0, 10, 20 }));
            Assert.That(Delays(NtripCategory.TlsFailed, 3), Is.EqualTo(new[] { 5.0, 10, 20 }));
        }

        [Test] public void NoNetwork_RetriesEveryFiveSeconds() => Assert.That(Delays(NtripCategory.NoNetwork, 4), Is.EqualTo(new[] { 5.0, 5, 5, 5 }));

        [Test]
        public void StreamLost_FirstReconnectsAtOnce_ThenLikeUnreachable()
        {
            var p = Policy();
            Assert.That(p.Next(NtripCategory.StreamLost, 1).Delay, Is.EqualTo(TimeSpan.Zero));
            Assert.That(Math.Round(p.Next(NtripCategory.StreamLost, 2).Delay.TotalSeconds, 1), Is.EqualTo(5.0));
            Assert.That(Math.Round(p.Next(NtripCategory.StreamLost, 3).Delay.TotalSeconds, 1), Is.EqualTo(10.0));
        }

        [TestCase(NtripCategory.LoginRejected)]
        [TestCase(NtripCategory.UnknownMountpoint)]
        public void RejectionsThatWillNotFixThemselves_StopAtThree(NtripCategory c)
        {
            var p = Policy();
            Assert.That(p.Next(c, 1).GiveUp, Is.False);
            Assert.That(p.Next(c, 2).GiveUp, Is.False);
            RetryDecision third = p.Next(c, 3);
            Assert.That(third.GiveUp, Is.True);
            Assert.That(third.MaxAttempts, Is.EqualTo(3));
        }

        [Test]
        public void Jitter_StaysWithinTenPercent_AndTheCap()
        {
            var low = new RetryPolicy(() => 0.0).Next(NtripCategory.CasterBusy, 4).Delay.TotalSeconds;
            var high = new RetryPolicy(() => 1.0).Next(NtripCategory.CasterBusy, 4).Delay.TotalSeconds;
            Assert.That(low, Is.EqualTo(270).Within(0.01));
            Assert.That(high, Is.EqualTo(300).Within(0.01), "capped at 5 minutes");
        }
    }

    [TestFixture]
    public class NtripRequestTests
    {
        [Test]
        public void V1_IsHttp10_WithoutHostOrVersionHeader()
        {
            string r = NtripRequest.Build(new NtripProfile("caster.example.org", 2101, "M1"), NtripVersion.V1, null);
            Assert.That(r, Is.EqualTo("GET /M1 HTTP/1.0\r\nUser-Agent: NTRIP AgOpenGPS\r\n\r\n"));
        }

        [Test]
        public void V2_HasHostVersionAndConnectionClose_N2_N3()
        {
            string r = NtripRequest.Build(new NtripProfile("caster.example.org", 2101, "M1"), NtripVersion.V2, null);
            Assert.That(r, Does.StartWith("GET /M1 HTTP/1.1\r\nHost: caster.example.org:2101\r\nNtrip-Version: Ntrip/2.0\r\n"));
            Assert.That(r, Does.Contain("Connection: close\r\n"));
            Assert.That(r, Does.EndWith("\r\n\r\n"));
        }

        [Test]
        public void Credentials_AreUtf8_N8()
        {
            string r = NtripRequest.Build(new NtripProfile("h", 2101, "M", "jörg", "pässwörd"), NtripVersion.V1, null);
            Assert.That(r, Does.Contain("Authorization: Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("jörg:pässwörd"))));
        }

        [Test]
        public void UserWithoutPassword_SendsTheColon()
        {
            // Centipede accepts "centipede" with an empty password.
            string r = NtripRequest.Build(new NtripProfile("h", 2101, "M", "centipede"), NtripVersion.V2, null);
            Assert.That(r, Does.Contain("Authorization: Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes("centipede:"))));
        }

        [Test]
        public void NoLogin_NoAuthorizationHeader()
        {
            Assert.That(NtripRequest.Build(new NtripProfile("h", 2101, "M"), NtripVersion.V2, null), Does.Not.Contain("Authorization"));
        }

        [Test]
        public void Gga_GoesInTheHeaderOnlyForV2()
        {
            var p = new NtripProfile("h", 2101, "M");
            Assert.That(NtripRequest.Build(p, NtripVersion.V2, "$GPGGA,1*00\r\n"), Does.Contain("Ntrip-GGA: $GPGGA,1*00\r\n"));
            Assert.That(NtripRequest.Build(p, NtripVersion.V1, "$GPGGA,1*00\r\n"), Does.Not.Contain("Ntrip-GGA"));
        }

        [Test]
        public void Port80_HostWithoutPort()
        {
            Assert.That(NtripRequest.Build(new NtripProfile("h.example", 80, "M"), NtripVersion.V2, null), Does.Contain("Host: h.example\r\n"));
        }
    }
}

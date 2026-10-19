using System.Net.Sockets;
using NUnit.Framework;

namespace AgLibrary.Ntrip.Tests
{
    [TestFixture]
    public class ConnectionErrorsTests
    {
        [Test]
        public void Refused_RtkBaseStopped_NamesTheHostAndThePossibleCause()
        {
            // Recorded: the RTKBase service was stopped; Windows reported "actively refused".
            NtripOutcome o = ConnectionErrors.Classify(SocketError.ConnectionRefused, "100.89.71.53", 2101);
            Assert.That(o.Category, Is.EqualTo(NtripCategory.CasterUnreachable));
            Assert.That(o.Message, Does.Contain("100.89.71.53:2101"));
            Assert.That(o.Message, Does.Contain("refused"));
        }

        [TestCase(SocketError.HostNotFound)]
        [TestCase(SocketError.TryAgain)]
        [TestCase(SocketError.NoData)]
        public void DnsFailures_AreCasterUnreachable_WithTheName(SocketError code)
        {
            NtripOutcome o = ConnectionErrors.Classify(code, "caster.example.org", 2101);
            Assert.That(o.Category, Is.EqualTo(NtripCategory.CasterUnreachable));
            Assert.That(o.Message, Does.Contain("caster.example.org"));
        }

        [TestCase(SocketError.NetworkDown)]
        [TestCase(SocketError.NetworkUnreachable)]
        public void NetworkDown_IsNoNetwork(SocketError code) =>
            Assert.That(ConnectionErrors.Classify(code, "h", 1).Category, Is.EqualTo(NtripCategory.NoNetwork));

        [TestCase(SocketError.TimedOut)]
        [TestCase(SocketError.HostUnreachable)]
        [TestCase(SocketError.ConnectionReset)]
        [TestCase(SocketError.AccessDenied)]
        public void OtherErrors_AreCasterUnreachable_AndNeverEmpty(SocketError code)
        {
            NtripOutcome o = ConnectionErrors.Classify(code, "h", 1);
            Assert.That(o.Category, Is.EqualTo(NtripCategory.CasterUnreachable));
            Assert.That(o.Message, Is.Not.Empty);
        }
    }
}

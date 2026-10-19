using System.Net.Sockets;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// Turns a socket error from connecting into a category and a plain message. The text of the
    /// exception is localised by Windows ("de doelcomputer heeft de verbinding actief geweigerd"),
    /// so only the error code is used. Pure logic.
    /// </summary>
    public static class ConnectionErrors
    {
        public static NtripOutcome Classify(SocketError code, string host, int port)
        {
            string where = host + ":" + port;
            switch (code)
            {
                case SocketError.ConnectionRefused:
                    return Unreachable(where + " refused the connection (is the caster or base station running?)", code);
                case SocketError.HostNotFound:
                case SocketError.NoData:
                case SocketError.TryAgain:
                    return Unreachable("Cannot find " + host + " (name lookup failed)", code);
                case SocketError.TimedOut:
                    return Unreachable(where + " did not answer (timed out)", code);
                case SocketError.HostUnreachable:
                    return Unreachable("No route to " + where, code);
                case SocketError.NetworkDown:
                case SocketError.NetworkUnreachable:
                    return new NtripOutcome(NtripCategory.NoNetwork, "No network connection", code.ToString(), false);
                case SocketError.ConnectionReset:
                case SocketError.ConnectionAborted:
                    return Unreachable(where + " closed the connection while connecting", code);
                default:
                    return Unreachable("Cannot connect to " + where + " (" + code + ")", code);
            }
        }

        private static NtripOutcome Unreachable(string message, SocketError code) =>
            new NtripOutcome(NtripCategory.CasterUnreachable, message, code.ToString(), false);
    }
}

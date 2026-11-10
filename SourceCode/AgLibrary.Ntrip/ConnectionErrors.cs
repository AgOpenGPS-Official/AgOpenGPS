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
                    return Unreachable("Cannot reach " + where + ". Is the base station or caster switched on?", code);
                case SocketError.HostNotFound:
                case SocketError.NoData:
                case SocketError.TryAgain:
                    return Unreachable("Cannot find " + host + ". Check the address and your internet connection.", code);
                case SocketError.TimedOut:
                    return Unreachable("No answer from " + where + ".", code);
                case SocketError.HostUnreachable:
                    return Unreachable("Cannot reach " + where + " (no route).", code);
                case SocketError.NetworkDown:
                case SocketError.NetworkUnreachable:
                    return new NtripOutcome(NtripCategory.NoNetwork, "No network connection.", code.ToString(), false);
                case SocketError.ConnectionReset:
                case SocketError.ConnectionAborted:
                    return Unreachable(where + " closed the connection.", code);
                default:
                    return Unreachable("Cannot connect to " + where + " (" + code + ").", code);
            }
        }

        private static NtripOutcome Unreachable(string message, SocketError code) =>
            new NtripOutcome(NtripCategory.CasterUnreachable, message, code.ToString(), false);
    }
}

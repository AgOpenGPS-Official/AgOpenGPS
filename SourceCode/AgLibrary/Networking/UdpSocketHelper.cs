using System;
using System.Net.Sockets;
using System.Threading;

namespace AgLibrary.Networking
{
    public static class UdpSocketHelper
    {
        // SIO_UDP_CONNRESET = _WSAIOW(IOC_VENDOR, 12)
        private const int SIO_UDP_CONNRESET = -1744830452;

        /// <summary>
        /// On Windows, an ICMP "port unreachable" reply (e.g. sending to a loopback port nobody
        /// is listening on yet, or right after resume from standby) makes the next
        /// EndReceiveFrom throw WSAECONNRESET (10054). That error is meaningless for UDP but
        /// kills an async receive loop. Turn that behaviour off.
        /// </summary>
        public static void DisableConnectionReset(Socket socket)
        {
            if (socket == null) return;
            try
            {
                socket.IOControl(SIO_UDP_CONNRESET, new byte[] { 0, 0, 0, 0 }, null);
            }
            catch
            {
                // Not supported on this platform - nothing to do
            }
        }

        /// <summary>
        /// Short pause after a receive error before re-arming the receive, so a socket that
        /// keeps failing can never turn the async receive loop into a CPU-burning hot loop.
        /// Runs on an I/O completion thread, never on the UI thread.
        /// </summary>
        public static void ErrorBackoff()
        {
            Thread.Sleep(50);
        }

        /// <summary>
        /// True when the socket has been closed/disposed on purpose and the receive loop should stop.
        /// </summary>
        public static bool IsSocketGone(Exception ex)
        {
            return ex is ObjectDisposedException
                || (ex is SocketException se && se.SocketErrorCode == SocketError.OperationAborted);
        }
    }
}

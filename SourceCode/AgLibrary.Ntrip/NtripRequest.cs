using System;
using System.Globalization;
using System.Text;

namespace AgLibrary.Ntrip
{
    /// <summary>Builds the mountpoint request. Pure, so it can be tested without a socket.</summary>
    public static class NtripRequest
    {
        public const string UserAgent = "NTRIP AgOpenGPS";

        /// <param name="version">V1 or V2; Auto is resolved by the caller.</param>
        /// <param name="gga">A complete GGA sentence for the Ntrip-GGA header (V2 only), or null.</param>
        public static string Build(NtripProfile p, NtripVersion version, string gga)
        {
            bool v2 = version == NtripVersion.V2;
            var sb = new StringBuilder();
            sb.Append("GET /").Append(Uri.EscapeDataString(p.Mount)).Append(v2 ? " HTTP/1.1\r\n" : " HTTP/1.0\r\n");
            if (v2)
            {
                // N2, N3: HTTP/1.1 needs a Host header, and NTRIP 2.0 announces itself.
                sb.Append("Host: ").Append(p.Host);
                if (p.Port != 80) sb.Append(':').Append(p.Port.ToString(CultureInfo.InvariantCulture));
                sb.Append("\r\nNtrip-Version: Ntrip/2.0\r\n");
            }
            sb.Append("User-Agent: ").Append(UserAgent).Append("\r\n");
            if (p.User.Length > 0 || p.Password.Length > 0)
            {
                // N8: UTF-8, so a non-ASCII password is not turned into question marks.
                sb.Append("Authorization: Basic ")
                  .Append(Convert.ToBase64String(Encoding.UTF8.GetBytes(p.User + ":" + p.Password))).Append("\r\n");
            }
            if (v2 && gga != null) sb.Append("Ntrip-GGA: ").Append(gga.TrimEnd('\r', '\n')).Append("\r\n");
            if (v2) sb.Append("Connection: close\r\n");
            sb.Append("\r\n");
            return sb.ToString();
        }
    }
}

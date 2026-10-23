using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AgLibrary.Ntrip
{
    /// <summary>
    /// A plain-text summary of the NTRIP session to paste into a support question or bug report.
    /// The password is never included. Pure logic.
    /// </summary>
    public static class SupportReport
    {
        public static string Build(NtripStatus status, NtripProfile profile, IReadOnlyList<CheckItem> checks)
        {
            var ci = CultureInfo.InvariantCulture;
            var sb = new StringBuilder();
            sb.AppendLine("NTRIP report");
            sb.AppendLine("status: " + status.Headline + " (" + status.State + ")");
            if (status.Message.Length > 0) sb.AppendLine("message: " + status.Message);
            if (status.LastError != null) sb.AppendLine("last problem: " + status.LastError + ", failures in a row: " + status.FailuresInRow.ToString(ci));
            if (status.FirstLine.Length > 0) sb.AppendLine("service answered: " + status.FirstLine);
            if (profile != null)
            {
                sb.AppendLine("service: " + profile.Host + ":" + profile.Port.ToString(ci) + ", mountpoint: " + profile.Mount);
                sb.AppendLine("user: " + (profile.User.Length > 0 ? profile.User : "(none)") + ", password: " + (profile.Password.Length > 0 ? "set" : "(none)"));
                sb.AppendLine("NTRIP version: " + status.VersionUsed + (profile.RawTcp ? ", raw TCP" : ""));
            }

            RtcmStreamSnapshot rtcm = status.Rtcm;
            sb.AppendLine("received: " + status.BytesReceived.ToString(ci) + " bytes, " + status.Frames.ToString(ci) + " messages");
            sb.AppendLine("checksum errors: " + rtcm.ChecksumFailures.ToString(ci) + ", skipped bytes: " + rtcm.BytesSkipped.ToString(ci)
                + ", base station ID: " + (rtcm.StationId >= 0 ? rtcm.StationId.ToString(ci) : "unknown"));
            if (status.Unframed) sb.AppendLine("stream: not RTCM 3, forwarded as it comes");

            sb.AppendLine();
            sb.AppendLine("checks:");
            foreach (CheckItem c in checks)
                sb.AppendLine("  [" + c.Level + "] " + c.Title + (c.Detail.Length > 0 ? " - " + c.Detail : ""));

            sb.AppendLine();
            sb.AppendLine("type  times     bytes  every(s)  last(s)  what");
            foreach (RtcmTypeStat t in rtcm.Types)
            {
                string every = double.IsNaN(t.MeanIntervalSeconds) ? "-" : t.MeanIntervalSeconds.ToString("F1", ci);
                sb.AppendLine(t.Type.ToString(ci).PadLeft(4) + "  " + t.Count.ToString(ci).PadLeft(5) + "  " + t.Bytes.ToString(ci).PadLeft(8)
                    + "  " + every.PadLeft(8) + "  " + t.SecondsSinceLast.ToString("F1", ci).PadLeft(7) + "  " + RtcmMessages.Describe(t.Type));
            }
            return sb.ToString();
        }
    }
}

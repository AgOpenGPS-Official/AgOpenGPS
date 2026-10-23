using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using AgLibrary.Ntrip;

namespace AgIO
{
    /// <summary>
    /// The technical side of the NTRIP session, for support and bug reports: the same checks as the
    /// fly-out, every message type the service sends, and a button that copies a report.
    /// </summary>
    public partial class FormNtripDetails : Form
    {
        private readonly FormLoop mf;

        public FormNtripDetails(FormLoop callingForm)
        {
            mf = callingForm;
            InitializeComponent();
        }

        private void FormNtripDetails_Load(object sender, EventArgs e)
        {
            RefreshData();
            timer.Start();
        }

        private void FormNtripDetails_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer.Stop();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            RefreshData();
        }

        private NtripStatus CurrentStatus()
        {
            NtripHost host = mf.ntripHost;
            return host == null ? new NtripStatus() : host.Status;
        }

        private void RefreshData()
        {
            NtripStatus st = CurrentStatus();
            NtripHost host = mf.ntripHost;
            NtripProfile profile = host == null ? null : host.ActiveProfile;
            ManagerStatus ms = host == null ? null : host.Manager;
            bool fallback = host != null && host.FallbackActive;
            var ci = CultureInfo.InvariantCulture;

            checks.Alert = fallback;
            info.ForeColor = fallback ? NtripStatusCard.LightColor(NtripLight.Bad) : System.Drawing.SystemColors.ControlText;
            info.BackColor = fallback ? System.Drawing.Color.FromArgb(255, 226, 222) : System.Drawing.SystemColors.Control;
            checks.SetChecks(CorrectionChecks.Build(st, mf.fixQualityData, mf.ageData));

            string where = profile == null ? (fallback && ms != null ? ms.Using.Replace("Fallback: ", "") : "-") : profile.Mount + " on " + profile.Host + ":" + profile.Port.ToString(ci);
            string station = st.Rtcm.StationId >= 0 ? st.Rtcm.StationId.ToString(ci) : "unknown";
            info.Text = (fallback ? "FALLBACK IN USE: the main service is not used now\r\n" : "") + st.Headline + "\r\n" + where + "\r\n"
                + "Base station ID: " + station + "   Damaged data: " + (st.Rtcm.ChecksumFailures == 0 ? "none" : st.Rtcm.ChecksumFailures.ToString(ci))
                + "   Service answered: " + (st.FirstLine.Length > 0 ? st.FirstLine : "-");

            list.BeginUpdate();

            // Update the rows in place: clearing the list would scroll it back to the top at every refresh.
            int row = 0;
            foreach (RtcmTypeStat t in st.Rtcm.Types)
            {
                string what = RtcmMessages.Describe(t.Type);
                string every = double.IsNaN(t.MeanIntervalSeconds) ? "-" : t.MeanIntervalSeconds.ToString("0.#", ci) + " s";
                string name = (what.Length > 0 ? what + " " : "") + "(" + t.Type.ToString(ci) + ")";
                string count = t.Count.ToString(ci);
                string last = t.SecondsSinceLast.ToString("0.#", ci) + " s";

                if (row < list.Items.Count)
                {
                    ListViewItem item = list.Items[row];
                    SetText(item.SubItems[0], name);
                    SetText(item.SubItems[1], count);
                    SetText(item.SubItems[2], every);
                    SetText(item.SubItems[3], last);
                }
                else
                {
                    var item = new ListViewItem(name);
                    item.SubItems.Add(count);
                    item.SubItems.Add(every);
                    item.SubItems.Add(last);
                    list.Items.Add(item);
                }
                row++;
            }
            while (list.Items.Count > row) list.Items.RemoveAt(list.Items.Count - 1);
            list.EndUpdate();
        }

        private static void SetText(ListViewItem.ListViewSubItem sub, string text)
        {
            if (sub.Text != text) sub.Text = text;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            NtripStatus st = CurrentStatus();
            NtripHost host = mf.ntripHost;
            string report = SupportReport.Build(st, host == null ? null : host.ActiveProfile, CorrectionChecks.Build(st, mf.fixQualityData, mf.ageData));
            try
            {
                Clipboard.SetText(report);
                btnCopy.Text = "Copied";
            }
            catch (Exception)
            {
                btnCopy.Text = "Could not copy";
            }
        }
    }
}

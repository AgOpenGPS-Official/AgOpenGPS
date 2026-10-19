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
            NtripProfile profile = host == null ? null : host.Profile;
            var ci = CultureInfo.InvariantCulture;

            checks.SetChecks(CorrectionChecks.Build(st, mf.fixQualityData, mf.ageData));

            string where = profile == null ? "-" : profile.Mount + " on " + profile.Host + ":" + profile.Port.ToString(ci);
            string station = st.Rtcm.StationId >= 0 ? st.Rtcm.StationId.ToString(ci) : "unknown";
            info.Text = st.Headline + "\r\n" + where + "\r\n"
                + "Base station ID: " + station + "   Damaged data: " + (st.Rtcm.ChecksumFailures == 0 ? "none" : st.Rtcm.ChecksumFailures.ToString(ci))
                + "   Service answered: " + (st.FirstLine.Length > 0 ? st.FirstLine : "-");

            list.BeginUpdate();
            list.Items.Clear();
            foreach (RtcmTypeStat t in st.Rtcm.Types)
            {
                string what = RtcmMessages.Describe(t.Type);
                string every = double.IsNaN(t.MeanIntervalSeconds) ? "-" : t.MeanIntervalSeconds.ToString("0.#", ci) + " s";
                var item = new ListViewItem((what.Length > 0 ? what + " " : "") + "(" + t.Type.ToString(ci) + ")");
                item.SubItems.Add(t.Count.ToString(ci));
                item.SubItems.Add(every);
                item.SubItems.Add(t.SecondsSinceLast.ToString("0.#", ci) + " s");
                list.Items.Add(item);
            }
            list.EndUpdate();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            NtripStatus st = CurrentStatus();
            NtripHost host = mf.ntripHost;
            string report = SupportReport.Build(st, host == null ? null : host.Profile, CorrectionChecks.Build(st, mf.fixQualityData, mf.ageData));
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

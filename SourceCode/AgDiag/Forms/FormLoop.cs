using System;
using System.Drawing;
using System.Windows.Forms;
using AgDiag.Protocol;

namespace AgDiag
{
    public partial class FormLoop : Form
    {
        private Pgns _pgns;
        private UdpCommunication _udpCommunication;

        public FormLoop()
        {
            _pgns = new Pgns();
            _udpCommunication = new UdpCommunication(_pgns);

            _udpCommunication.DefaultSendsUpdated += (s, e) => BeginInvoke((MethodInvoker)(() => UpdateDefaultSends(e)));

            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblSection1.BackColor = _pgns.asData.IsSectionOn(1) ? Color.Green : Color.Red;
            lblSection2.BackColor = _pgns.asData.IsSectionOn(2) ? Color.Green : Color.Red;
            lblSection3.BackColor = _pgns.asData.IsSectionOn(3) ? Color.Green : Color.Red;
            lblSection4.BackColor = _pgns.asData.IsSectionOn(4) ? Color.Green : Color.Red;

            lblSection5.BackColor = _pgns.asData.IsSectionOn(5) ? Color.Green : Color.Red;
            lblSection6.BackColor = _pgns.asData.IsSectionOn(6) ? Color.Green : Color.Red;
            lblSection7.BackColor = _pgns.asData.IsSectionOn(7) ? Color.Green : Color.Red;
            lblSection8.BackColor = _pgns.asData.IsSectionOn(8) ? Color.Green : Color.Red;

            lblSpeed.Text = _pgns.asData.Speed.ToString();
            lblSetSteerAngle.Text = _pgns.asData.SteerAngle.ToString();
            lblStatus.Text = _pgns.asData.Status.ToString();

            lblSteerDataPGN.Text = _pgns.asData.ToHexString();

            //from autosteer module
            lblSteerAngleActual.Text = _pgns.asModule.ActualSteerAngle.ToString();
            lblHeading.Text = _pgns.asModule.Heading.ToString();
            lblRoll.Text = _pgns.asModule.Roll.ToString();
            lblPWM.Text = _pgns.asModule.PWM.ToString();

            lblWorkSwitch.BackColor = _pgns.asModule.IsWorkSwitchOn ? Color.Red : Color.Green;
            lblSteerSwitch.BackColor = _pgns.asModule.IsSteerSwitchOn ? Color.Red : Color.Green;

            lblPGNFromAutosteerModule.Text = _pgns.asModule.ToHexString();

            //Autosteer settings
            lblPGNSteerSettings.Text = _pgns.asSet.ToHexString();

            lblP.Text = _pgns.asSet.GainProportional.ToString();
            lblHiPWM.Text = _pgns.asSet.HighPWM.ToString();
            lblLoPWM.Text = _pgns.asSet.LowPWM.ToString();
            lblMinPWM.Text = _pgns.asSet.MinPWM.ToString();
            lblCPD.Text = _pgns.asSet.CountsPerDegree.ToString();
            lblAckerman.Text = _pgns.asSet.Ackerman.ToString();
            lblOffset.Text = _pgns.asSet.SteerOffset.ToString();

            //autosteer config bytes
            lblPGNAutoSteerConfig.Text = _pgns.asConfig.ToHexString();

            lblSet0.Text = _pgns.asConfig.Set0.ToString();
            lblPulseCount.Text = _pgns.asConfig.MaxPulse.ToString();
            lblMinSpeed.Text = _pgns.asConfig.MinSpeed.ToString();

            //machine bytes
            lblPNGMachine.Text = _pgns.maData.ToHexString();

            TreeLbl.Text = _pgns.maData.Speed.ToString();
        }

        private void UpdateDefaultSends(int defaultSends)
        {
            lblDefaultSends.Text = defaultSends.ToString();
        }

        private void FormLoop_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            _udpCommunication.LoadLoopback();
        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            _udpCommunication.CloseLoopback();
        }
    }
}


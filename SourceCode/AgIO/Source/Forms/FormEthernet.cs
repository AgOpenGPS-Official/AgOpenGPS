using System;
using System.IO;
using System.Windows.Forms;
using AgIO.Controls;
using AgLibrary.Logging;

namespace AgIO
{
    public partial class FormEthernet : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormEthernet(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();

            nudFirstIP.Controls[0].Enabled = false;
            nudSecndIP.Controls[0].Enabled = false;
            nudThirdIP.Controls[0].Enabled = false;
        }

        private void FormUDp_Load(object sender, EventArgs e)
        {
            cboxIsUDPOn.Checked = Properties.Settings.Default.setUDP_isOn;
            cboxIsUDPOn.Text = cboxIsUDPOn.Checked ? "UDP On" : "UDP Off";

            //cboxIsSendNMEAToUDP.Checked = Properties.Settings.Default.setUDP_isSendNMEAToUDP;

            //nudSub1.Value = Properties.Settings.Default.etIP_SubnetOne;
            //nudSub2.Value = Properties.Settings.Default.etIP_SubnetTwo;
            //nudSub3.Value = Properties.Settings.Default.etIP_SubnetThree;

            nudFirstIP.Value = Properties.Settings.Default.eth_loopOne;
            nudSecndIP.Value = Properties.Settings.Default.eth_loopTwo;
            nudThirdIP.Value = Properties.Settings.Default.eth_loopThree;
            nudFourthIP.Value = Properties.Settings.Default.eth_loopFour;

            if (!cboxIsUDPOn.Checked) cboxIsUDPOn.BackColor = System.Drawing.Color.Salmon;
        }

        private void nudFirstIP_Click(object sender, EventArgs e)
        {
            ((NumericUpDown)sender).ShowKeypad(this);
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
            //Properties.Settings.Default.etIP_SubnetOne = (byte)nudSub1.Value;
            //Properties.Settings.Default.etIP_SubnetTwo = (byte)nudSub2.Value;
            //Properties.Settings.Default.etIP_SubnetThree = (byte)nudSub3.Value;

            Properties.Settings.Default.eth_loopOne = (byte)nudFirstIP.Value;
            Properties.Settings.Default.eth_loopTwo = (byte)nudSecndIP.Value;
            Properties.Settings.Default.eth_loopThree = (byte)nudThirdIP.Value;
            Properties.Settings.Default.eth_loopFour = (byte)nudFourthIP.Value;

            Properties.Settings.Default.setUDP_isOn = cboxIsUDPOn.Checked;
            //Properties.Settings.Default.setUDP_isSendNMEAToUDP = cboxIsSendNMEAToUDP.Checked;

            Properties.Settings.Default.Save();

            mf.YesMessageBox("AgIO will Restart to Enable UDP Networking Features");
            Log.EventWriter("Program Reset: Start Ethernet Selected");

            Program.Restart();
            Close();
        }

        private void cboxIsUDPOn_Click(object sender, EventArgs e)
        {
            cboxIsUDPOn.Text = cboxIsUDPOn.Checked ? "UDP Is On" : "UDP Is Off";
            Log.EventWriter("UDP Turned on, Etherent Form");
        }
    }
}
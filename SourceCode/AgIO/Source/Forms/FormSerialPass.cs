using System;
using System.IO.Ports;
using System.Windows.Forms;
using AgLibrary.Logging;

namespace AgIO
{
    public partial class FormSerialPass : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormSerialPass(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
        }

        private void FormSerialPass_Load(object sender, EventArgs e)
        {
            cboxSerialPassOn.Checked = Properties.Settings.Default.setPass_isOn;
            cboxToSerial.Checked = Properties.Settings.Default.setNTRIP_sendToSerial;
            cboxToUDP.Checked = Properties.Settings.Default.setNTRIP_sendToUDP;
            nudSendToUDPPort.Value = Properties.Settings.Default.setNTRIP_sendToUDPPort;

            cboxRadioPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxRadioPort.Items.Add(s);
            }

            lblCurrentPort.Text = Properties.Settings.Default.setPort_portNameRadio;
            cboxRadioPort.Text = Properties.Settings.Default.setPort_portNameRadio;
            lblCurrentBaud.Text = Properties.Settings.Default.setPort_baudRateRadio;
            cboxBaud.Text = Properties.Settings.Default.setPort_baudRateRadio;

            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                btnOpenSerial.Enabled = false;
                btnCloseSerial.Enabled = true;
                cboxRadioPort.Enabled = false;
                cboxBaud.Enabled = false;
            }
            else
            {
                btnOpenSerial.Enabled = true;
                btnCloseSerial.Enabled = false;
                cboxRadioPort.Enabled = true;
                cboxBaud.Enabled = true;
            }
        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setPass_isOn = cboxSerialPassOn.Checked;

            if (cboxSerialPassOn.Checked)
            {
                Properties.Settings.Default.setNTRIP_isOn = mf.isNTRIP_RequiredOn = false;
                Properties.Settings.Default.setRadio_isOn = mf.isRadio_RequiredOn = false;
            }

            Properties.Settings.Default.setNTRIP_sendToUDPPort = (int)nudSendToUDPPort.Value;

            Properties.Settings.Default.setNTRIP_sendToSerial = cboxToSerial.Checked;
            Properties.Settings.Default.setNTRIP_sendToUDP = cboxToUDP.Checked;

            mf.isSendToSerial = cboxToSerial.Checked;
            mf.isSendToUDP = cboxToUDP.Checked;

            Properties.Settings.Default.setPort_portNameRadio = cboxRadioPort.Text;
            Properties.Settings.Default.setPort_baudRateRadio = cboxBaud.Text;

            Properties.Settings.Default.Save();

            Log.EventWriter("Program Reset: Button OK on Serial Pass Form");

            Program.Restart();
            Close();
        }

        private void btnSerialCancel_Click(object sender, EventArgs e)
        {
        }

        private void btnOpenSerial_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.Close();
                mf.spRadio.Dispose();
                mf.spRadio = null;
            }

            // Setup and open serial port
            mf.spRadio = new SerialPort(cboxRadioPort.Text, int.Parse(cboxBaud.Text));

            btnOpenSerial.Enabled = false;
            btnCloseSerial.Enabled = true;

            try
            {
                mf.spRadio.Open();

                lblCurrentPort.Text = Properties.Settings.Default.setPort_portNameRadio;
                lblCurrentBaud.Text = Properties.Settings.Default.setPort_baudRateRadio;

                cboxRadioPort.Enabled = false;
                cboxBaud.Enabled = false;
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(3000, "Error opening port", ex.Message);
                Log.EventWriter("Catch - > Error opening port" + ex.ToString());
            }
        }

        private void btnCloseSerial_Click(object sender, EventArgs e)
        {
            if (mf.spRadio != null && mf.spRadio.IsOpen)
            {
                mf.spRadio.Close();
                mf.spRadio.Dispose();
                mf.spRadio = null;

                btnOpenSerial.Enabled = true;
                btnCloseSerial.Enabled = false;

                cboxRadioPort.Enabled = true;
                cboxBaud.Enabled = true;
            }
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            cboxRadioPort.Items.Clear();

            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxRadioPort.Items.Add(s);
            }
        }

        private void cboxToSerial_Click(object sender, EventArgs e)
        {
            if (cboxToUDP.Checked) cboxToUDP.Checked = false;
        }

        private void cboxToUDP_Click(object sender, EventArgs e)
        {
            if (cboxToSerial.Checked) cboxToSerial.Checked = false;
        }

        private void cboxSerialPassOn_Click(object sender, EventArgs e)
        {
        }
    }
}
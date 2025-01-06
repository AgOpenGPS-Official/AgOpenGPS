using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AgLibrary.Logging;

namespace AgIO
{
    public partial class FormNtrip : Form
    {
        //class variables
        private readonly FormLoop mf;

        private bool ntripStatusChanged = false;

        public FormNtrip(Form callingForm)
        {
            mf = callingForm as FormLoop;
            InitializeComponent();

            //this.groupBox2.Text = gStr.gsNetworking;
            //this.cboxIsNTRIPOn.Text = gStr.gsNTRIPOn;
            //this.label6.Text = gStr.gsPort;
            //this.label4.Text = gStr.gsEnterBroadcasterURLOrIP;
            //this.label7.Text = gStr.gsToUDPPort;

            //this.label3.Text = gStr.gsUsername;
            //this.label12.Text = gStr.gsPassword;
            //this.label13.Text = gStr.gsMount;
            //this.label15.Text = gStr.gsGGAIntervalSecs;
            //this.btnGetIP.Text = gStr.gsConfirmIP;

            //this.label9.Text = gStr.gsCurrentGPSFix;
            //this.label17.Text = gStr.gsSendToManualFix;
            //this.btnSetManualPosition.Text = gStr.gsSendToManualFix;
            //this.label18.Text = gStr.gsSetToZeroForSerial;
            //this.btnGetSourceTable.Text = gStr.gsGetSourceTable;

            //this.label1.Text = gStr.gsRestartRequired;
            //this.label19.Text = gStr.gsZeroEqualsOff;

            //this.Text = gStr.gsNTRIPClientSettings;

            //turn off the little arrows
            nudCasterPort.Controls[0].Enabled = false;
            nudGGAInterval.Controls[0].Enabled = false;
            nudLatitude.Controls[0].Enabled = false;
            nudLongitude.Controls[0].Enabled = false;
            nudSendToUDPPort.Controls[0].Enabled = false;
        }

        private void FormNtrip_Load(object sender, EventArgs e)
        {
            cboxIsNTRIPOn.Checked = Properties.Settings.Default.setNTRIP_isOn;

            if (!cboxIsNTRIPOn.Checked) tabControl1.Enabled = false;
            string hostName = Dns.GetHostName(); // Retrieve the Name of HOST
            tboxHostName.Text = hostName;

            //IPAddress[] ipaddress = Dns.GetHostAddresses(hostName);
            GetIP4AddressList();

            cboxToSerial.Checked = Properties.Settings.Default.setNTRIP_sendToSerial;
            cboxToUDP.Checked = Properties.Settings.Default.setNTRIP_sendToUDP;
            nudSendToUDPPort.Value = Properties.Settings.Default.setNTRIP_sendToUDPPort;

            tboxEnterURL.Text = Properties.Settings.Default.setNTRIP_casterURL;

            tboxCasterIP.Text = Properties.Settings.Default.setNTRIP_casterIP;
            nudCasterPort.Value = Properties.Settings.Default.setNTRIP_casterPort;

            tboxUserName.Text = Properties.Settings.Default.setNTRIP_userName;
            tboxUserPassword.Text = Properties.Settings.Default.setNTRIP_userPassword;
            tboxMount.Text = Properties.Settings.Default.setNTRIP_mount;

            nudGGAInterval.Value = Properties.Settings.Default.setNTRIP_sendGGAInterval;

            nudLatitude.Value = (decimal)Properties.Settings.Default.setNTRIP_manualLat;
            nudLongitude.Value = (decimal)Properties.Settings.Default.setNTRIP_manualLon;
            tboxCurrentLat.Text = Properties.Settings.Default.setNTRIP_manualLat.ToString();
            tboxCurrentLon.Text = Properties.Settings.Default.setNTRIP_manualLon.ToString();

            checkBoxusetcp.Checked = Properties.Settings.Default.setNTRIP_isTCP;

            if (Properties.Settings.Default.setNTRIP_isGGAManual) cboxGGAManual.Text = "Use Manual Fix";
            else cboxGGAManual.Text = "Use GPS Fix";

            if (Properties.Settings.Default.setNTRIP_isHTTP10) cboxHTTP.Text = "1.0";
            else cboxHTTP.Text = "1.1";

            comboboxPacketSize.Text = mf.packetSizeNTRIP.ToString();
        }

        private void cboxIsNTRIPOn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setNTRIP_isOn = cboxIsNTRIPOn.Checked;

            if (cboxIsNTRIPOn.Checked)
            {
                Properties.Settings.Default.setRadio_isOn = mf.isRadio_RequiredOn = false;
                Properties.Settings.Default.setPass_isOn = mf.isSerialPass_RequiredOn = false;
                Log.EventWriter("NTRIP Turned on");
            }
            else
            {
                Log.EventWriter("NTRIP Turned off");
            }

            Properties.Settings.Default.Save();

            mf.YesMessageBox("Restart of AgIO is Required - Restarting");
            Log.EventWriter("Program Reset: Selecting NTRIP Feature");

            RegistrySettings.Save();
            Application.Restart();
            Environment.Exit(0);
        }

        //get the ipv4 address only
        public void GetIP4AddressList()
        {
            listboxIP.Items.Clear();

            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    listboxIP.Items.Add(IPA.ToString());
                }
            }
        }

        private void btnGetIP_Click(object sender, EventArgs e)
        {
            string actualIP = tboxEnterURL.Text.Trim();
            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(actualIP);
                if (addresslist != null)
                {
                    tboxCasterIP.Text = "";
                    foreach (var addr in addresslist)
                    {
                        if (addr.AddressFamily == AddressFamily.InterNetwork)
                        {
                            tboxCasterIP.Text = addr.ToString().Trim();
                            mf.broadCasterIP = addr.ToString().Trim();
                            Properties.Settings.Default.setNTRIP_casterIP = mf.broadCasterIP;
                            Properties.Settings.Default.Save();
                            break;
                        }
                    }
                    mf.TimedMessageBox(2500, "IP Located", "Verified: " + actualIP);
                }
                else
                {
                    mf.YesMessageBox("Can't Find: " + actualIP);
                    Log.EventWriter("Can't Find Caster IP");
                }
            }
            catch (Exception ex)
            {
                mf.YesMessageBox("Can't Find: " + actualIP);
                Log.EventWriter("Catch -> Can't Find Caster IP" + ex.ToString());
            }
        }

        public Boolean CheckIPValid(String strIP)
        {
            // Return true for COM Port
            if (strIP.Contains("COM"))
            {
                return true;
            }

            //  Split string by ".", check that array length is 3
            string[] arrOctets = strIP.Split('.');

            //at least 4 groups in the IP
            if (arrOctets.Length != 4) return false;

            //  Check each substring checking that the int value is less than 255 and that is char[] length is !> 2
            const Int16 MAXVALUE = 255;
            Int32 temp; // Parse returns Int32
            foreach (String strOctet in arrOctets)
            {
                //check if at least 3 digits but not more OR 0 length
                if (strOctet.Length > 3 || strOctet.Length == 0) return false;

                //make sure all digits
                if (!int.TryParse(strOctet, out int temp2)) return false;

                //make sure not more then 255
                temp = int.Parse(strOctet);
                if (temp > MAXVALUE || temp < 0) return false;
            }
            return true;
        }

        private void tboxCasterIP_Validating(object sender, CancelEventArgs e)
        {
            if (!CheckIPValid(tboxCasterIP.Text))
            {
                tboxCasterIP.Text = "127.0.0.1";
                tboxCasterIP.Focus();
                mf.TimedMessageBox(2000, "Invalid IP Address", "Set to Default Local 127.0.0.1");
            }
        }

        private void btnSerialOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setNTRIP_casterIP = tboxCasterIP.Text;
            Properties.Settings.Default.setNTRIP_casterPort = (int)nudCasterPort.Value;
            Properties.Settings.Default.setNTRIP_sendToUDPPort = (int)nudSendToUDPPort.Value;

            Properties.Settings.Default.setNTRIP_isOn = cboxIsNTRIPOn.Checked;

            if (cboxIsNTRIPOn.Checked)
            {
                Properties.Settings.Default.setRadio_isOn = mf.isRadio_RequiredOn = false;
                Properties.Settings.Default.setPass_isOn = mf.isSerialPass_RequiredOn = false;
            }

            Properties.Settings.Default.setNTRIP_userName = tboxUserName.Text;
            Properties.Settings.Default.setNTRIP_userPassword = tboxUserPassword.Text;
            Properties.Settings.Default.setNTRIP_mount = tboxMount.Text;

            Properties.Settings.Default.setNTRIP_sendGGAInterval = (int)nudGGAInterval.Value;
            Properties.Settings.Default.setNTRIP_manualLat = (double)nudLatitude.Value;
            Properties.Settings.Default.setNTRIP_manualLon = (double)nudLongitude.Value;

            Properties.Settings.Default.setNTRIP_casterURL = tboxEnterURL.Text;
            Properties.Settings.Default.setNTRIP_isGGAManual = cboxGGAManual.Text == "Use Manual Fix";
            Properties.Settings.Default.setNTRIP_isHTTP10 = cboxHTTP.Text == "1.0";
            Properties.Settings.Default.setNTRIP_isTCP = checkBoxusetcp.Checked;

            Properties.Settings.Default.setNTRIP_sendToSerial = cboxToSerial.Checked;
            Properties.Settings.Default.setNTRIP_sendToUDP = cboxToUDP.Checked;

            mf.isSendToSerial = cboxToSerial.Checked;
            mf.isSendToUDP = cboxToUDP.Checked;

            mf.packetSizeNTRIP = Convert.ToInt32(comboboxPacketSize.Text);
            Properties.Settings.Default.setNTRIP_packetSize = Convert.ToInt32(comboboxPacketSize.Text);

            if (Properties.Settings.Default.setNTRIP_isOn && Properties.Settings.Default.setRadio_isOn)
            {
                mf.TimedMessageBox(2000, "Radio also enabled", "Disable the Radio NTRIP");
                Properties.Settings.Default.setRadio_isOn = false;
            }

            Properties.Settings.Default.Save();

            if (!ntripStatusChanged)
            {
                Close();
                mf.ConfigureNTRIP();
            }
            else
            {
                Log.EventWriter("Program Reset: Button Ok on Ntrip Form");
                
                RegistrySettings.Save();
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void btnSetManualPosition_Click(object sender, EventArgs e)
        {
            nudLatitude.Value = (decimal)mf.latitude;
            nudLongitude.Value = (decimal)mf.longitude;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tboxCurrentLat.Text = mf.latitude.ToString();
            tboxCurrentLon.Text = mf.longitude.ToString();
        }

        private readonly List<string> dataList = new List<string>();

        private void btnGetSourceTable_Click(object sender, EventArgs e)
        {
            btnGetSourceTable.Enabled = false;
            IPAddress casterIP = IPAddress.Parse(tboxCasterIP.Text.Trim()); //Select correct Address
            int casterPort = (int)nudCasterPort.Value; //Select correct port (usually 80)

            Socket sckt;
            dataList?.Clear();

            try
            {
                sckt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    Blocking = true
                };
                sckt.Connect(new IPEndPoint(casterIP, casterPort));

                string msg = "GET / HTTP/1.0\r\n" + "User-Agent: NTRIP iter.dk\r\n" +
                                    "Accept: */*\r\nConnection: close\r\n" + "\r\n";

                //Send request
                byte[] data = System.Text.Encoding.ASCII.GetBytes(msg);
                sckt.Send(data);
                int bytes = 0;
                byte[] bytesReceived = new byte[1024];
                string page = String.Empty;
                Thread.Sleep(200);

                do
                {
                    bytes = sckt.Receive(bytesReceived, bytesReceived.Length, SocketFlags.None);
                    page += Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                }
                while (bytes > 0);

                if (page.Length > 0)
                {
                    string[] words = page.Split(new string[] { System.Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < words.Length; i++)
                    {
                        string[] words2 = words[i].Split(';');

                        if (words2[0] == "STR")
                        {
                            dataList.Add(words2[1].Trim().ToString() + "," + words2[9].ToString() + "," + words2[10].ToString()
                          + "," + words2[3].Trim().ToString() + "," + words2[6].Trim().ToString()
                                );
                        }
                    }
                }
            }
            catch (SocketException ex)
            {
                mf.TimedMessageBox(2000, "Socket Exception", "Invalid IP:Port");
                btnGetSourceTable.Enabled = true;
                Log.EventWriter("Catch -> Socket Exception, Invalid IP:Port" + ex.ToString());
                return;
            }
            catch (Exception ex)
            {
                mf.TimedMessageBox(2000, "Exception", "Get Source Table Error");
                btnGetSourceTable.Enabled = true;
                Log.EventWriter("Catch - > Get Source Table Error" + ex.ToString());
                return;
            }

            if (dataList.Count > 0)
            {
                string syte = "http://monitor.use-snip.com/?hostUrl=" + tboxCasterIP.Text + "&port=" + nudCasterPort.Value.ToString();
                using (FormSource form = new FormSource(this, dataList, mf.latitude, mf.longitude, syte))
                {
                    form.ShowDialog(this);
                }
            }
            else
            {
                mf.TimedMessageBox(2000, "Error", "No Source Data");
            }

            btnGetSourceTable.Enabled = true;


            // Console.WriteLine(page);
            // Process.Start(syte);
        }

        private void NudCasterPort_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnSerialCancel.Focus();
        }

        private void NudGGAInterval_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnSerialCancel.Focus();
        }

        private void NudLatitude_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnSerialCancel.Focus();
        }

        private void NudLongitude_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnSerialCancel.Focus();
        }

        private void NudSendToUDPPort_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnSerialCancel.Focus();
        }

        private void tboxEnterURL_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
            btnGetIP.PerformClick();
        }

        private void tboxMount_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void tboxUserName_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void tboxUserPassword_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnSerialCancel.Focus();
            }
        }

        private void btnPassUsername_Click(object sender, EventArgs e)
        {
            if (tboxUserName.PasswordChar == '*') tboxUserName.PasswordChar = '\0';
            else tboxUserName.PasswordChar = '*';
            tboxUserName.Invalidate();
        }

        private void btnPassPassword_Click(object sender, EventArgs e)
        {
            if (tboxUserPassword.PasswordChar == '*') tboxUserPassword.PasswordChar = '\0';
            else tboxUserPassword.PasswordChar = '*';
            tboxUserPassword.Invalidate();
        }

        private void cboxToUDP_Click(object sender, EventArgs e)
        {
            ntripStatusChanged = true;
            if (cboxToSerial.Checked) cboxToSerial.Checked = false;
        }

        private void cboxToSerial_Click(object sender, EventArgs e)
        {
            ntripStatusChanged = true;
            if (cboxToUDP.Checked) cboxToUDP.Checked = false;
        }

        private void btnHelp_Click(object sender, EventArgs e)
        {
            //System.Diagnostics.Process.Start(gStr.gsNTRIP_Help);
        }
    }
}
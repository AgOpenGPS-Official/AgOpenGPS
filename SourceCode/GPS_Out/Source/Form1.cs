using GPS_Out.PGNs;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GPS_Out
{
    /*
    $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M ,  ,*47
       0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
            Time      Lat       Lon     FixSatsOP Alt
    Where:
         GGA          Global Positioning System Fix Data
         123519       Fix taken at 12:35:19 UTC
         4807.038,N   Latitude 48 deg 07.038' N
         01131.000,E  Longitude 11 deg 31.000' E
         1            Fix quality: 0 = invalid
                                   1 = GPS fix (SPS)
                                   2 = DGPS fix
                                   3 = PPS fix
                                   4 = Real Time Kinematic
                                   5 = Float RTK
                                   6 = estimated (dead reckoning) (2.3 feature)
                                   7 = Manual input mode
                                   8 = Simulation mode
         08           Number of satellites being tracked
         0.9          Horizontal dilution of position
         545.4,M      Altitude, Meters, above mean sea level
         46.9,M       Height of geoid (mean sea level) above WGS84
                          ellipsoid
         (empty field) time in seconds since last DGPS update
         (empty field) DGPS station ID number
         *47          the checksum data, always begins with *
     *
     *
    $GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
     0      1    2   3      4    5      6   7     8     9     10   11
            Time      Lat        Lon       knots  Ang   Date  MagV

    Where:
         RMC          Recommended Minimum sentence C
         123519       Fix taken at 12:35:19 UTC
         A            Status A=active or V=Void.
         4807.038,N   Latitude 48 deg 07.038' N
         01131.000,E  Longitude 11 deg 31.000' E
         022.4        Speed over the ground in knots
         084.4        Track angle in degrees True
         230394       Date - 23rd of March 1994
         003.1,W      Magnetic Variation
         *6A          The checksum data, always begins with *
     *
    $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48
     *
        VTG          Track made good and ground speed
        054.7,T      True track made good (degrees)
        034.4,M      Magnetic track made good
        005.5,N      Ground speed, knots
        010.2,K      Ground speed, Kilometers per hour
        *48          Checksum
    */

    public partial class frmStart : Form
    {
        public UDPComm AGIOcomm;
        public PGN54908 AGIOdata;
        public UDPComm AOGcomm;
        public PGN100 AOGdata;
        public PGN_GGA GGA;
        public string GGAsentence = "";
        public PGN_GSA GSA;
        public PGN254 AutoSteerPGN;
        public string GSAsentence = "";
        public PGN_RMC RMC;
        public string RMCsentence = "";
        public SerialSend SER;
        public clsTools Tls;
        public PGN_VTG VTG;
        public string VTGsentence = "";
        public PGN_ZDA ZDA;
        public string ZDAsentence = "";
        private string HeadingType;
        private Color SimColor = Color.Orange;
        private int Watchdog;

        public frmStart()
        {
            InitializeComponent();
            Tls = new clsTools(this);
            AGIOcomm = new UDPComm(this, 15555, "127.103.104.105");
            AOGcomm = new UDPComm(this, 17777, "127.100.101.102");
            AGIOdata = new PGN54908(this);
            GGA = new PGN_GGA(this);
            VTG = new PGN_VTG(this);
            SER = new SerialSend(this);
            RMC = new PGN_RMC(this);
            AOGdata = new PGN100(this);
            backgroundWorker1.WorkerSupportsCancellation = true;
            ZDA = new PGN_ZDA(this);
            GSA = new PGN_GSA(this);
            AutoSteerPGN = new PGN254(this);
        }

        public int CheckSum(string Data)
        {
            int CK = 0;
            int End = Data.IndexOf("*");
            char[] buf = Data.ToCharArray();
            if (buf[0] == '$' && End > -1)
            {
                for (int i = 1; i < End; i++)
                {
                    CK ^= buf[i];
                }
            }
            return CK;
        }

        public string FixQuality(byte Qu)
        {
            String Result = "";
            switch (Qu)
            {
                case 1:
                    Result = "GPS 1";
                    break;

                case 2:
                    Result = "DGPS";
                    break;

                case 3:
                    Result = "PPS";
                    break;

                case 4:
                    Result = "RTK fix";
                    break;

                case 5:
                    Result = "Float";
                    break;

                case 6:
                    Result = "Estimate";
                    break;

                case 7:
                    Result = "Man IP";
                    break;

                case 8:
                    Result = "Sim";
                    break;
            }
            return Result;
        }

        public string ModeIndicator(byte Qu)
        {
            // NMEA 2.3+ mode indicator, last field of RMC and VTG
            // A = autonomous, D = differential, E = estimated (dead reckoning),
            // M = manual input, S = simulator, N = data not valid
            // RTK fix and float are reported as D. The R and F values are NMEA 4.x only.
            String Result = "N";
            switch (Qu)
            {
                case 1:
                case 3:
                    Result = "A";
                    break;

                case 2:
                case 4:
                case 5:
                    Result = "D";
                    break;

                case 6:
                    Result = "E";
                    break;

                case 7:
                    Result = "M";
                    break;

                case 8:
                    Result = "S";
                    break;
            }
            return Result;
        }

        public bool UseAOGposition()
        {
            // AOG position when Roll Corrected is on, or in simulation when AgIO has no position to give.
            // Without live AgIO data or Simulate nothing of AOG may show, also not with Roll Corrected:
            // AOG's own simulator keeps sending its position, but the title says NO DATA and nothing is sent.
            return AOGdata.Connected()
                && (AGIOdata.Connected() || Properties.Settings.Default.Simulate)
                && (Properties.Settings.Default.UseRollCorrected || !AGIOdata.Connected());
        }

        public double Heading()
        {
            double Result = 0;
            HeadingType = "";

            if (AGIOdata.HeadingDual < 361)
            {
                Result = AGIOdata.HeadingDual;
                HeadingType = "D";
            }
            else if (AOGdata.Fix2FixHeading < 361)
            {
                Result = AOGdata.Fix2FixHeading;
                HeadingType = "F";
            }
            else if (AGIOdata.TrueHeading < 361)
            {
                Result = AGIOdata.TrueHeading;
                HeadingType = "T";
            }
            else if (AGIOdata.IMUheading < 361)
            {
                Result = AGIOdata.IMUheading;
                HeadingType = "I";
            }

            return Result;
        }

        public void SetPortButtons1()
        {
            cboPort1.SelectedIndex = cboPort1.FindStringExact(SER.PortNm);
            cboBaud1.SelectedIndex = cboBaud1.FindStringExact(SER.Baud.ToString());

            if (SER.IsOpen())
            {
                cboBaud1.Enabled = false;
                cboPort1.Enabled = false;
                btnConnect1.Text = "Disconnect";
                PortIndicator1.Image = Properties.Resources.On;
            }
            else
            {
                cboBaud1.Enabled = true;
                cboPort1.Enabled = true;
                btnConnect1.Text = "Connect";
                PortIndicator1.Image = Properties.Resources.Off;
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            if (worker.CancellationPending == true)
            {
                e.Cancel = true;
            }
            else
            {
                if (GGAsentence != "") SER.SendStringData(GGAsentence);
                if (VTGsentence != "") SER.SendStringData(VTGsentence);
                if (RMCsentence != "") SER.SendStringData(RMCsentence);
                if (ZDAsentence != "") SER.SendStringData(ZDAsentence);
                if (GSAsentence != "") SER.SendStringData(GSAsentence);
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            GGAsentence = "";
            VTGsentence = "";
            RMCsentence = "";
            ZDAsentence = "";
            GSAsentence = "";
        }

        private void btnConnect1_Click(object sender, EventArgs e)
        {
            if (btnConnect1.Text == "Connect")
            {
                SER.Open();
            }
            else
            {
                SER.Close();
            }
            SetPortButtons1();
        }

        private void btnGGA_Click(object sender, EventArgs e)
        {
            if (AGIOdata.Connected() || Properties.Settings.Default.Simulate)
            {
                tbGGA.Text = GGA.Sentence;
            }
            else
            {
                tbGGA.Text = "";
            }
            if (tbGGA.Text != "") Clipboard.SetText(tbGGA.Text);
        }

        private void btnRescan_Click(object sender, EventArgs e)
        {
            LoadRCbox();
        }

        private void btnRMC_Click(object sender, EventArgs e)
        {
            if (AGIOdata.Connected() || Properties.Settings.Default.Simulate)
            {
                tbRMC.Text = RMC.Sentence;
            }
            else
            {
                tbRMC.Text = "";
            }
            if (tbRMC.Text != "") Clipboard.SetText(tbRMC.Text);
        }

        private void btnVTG_Click(object sender, EventArgs e)
        {
            if (AGIOdata.Connected() || Properties.Settings.Default.Simulate)
            {
                tbVTG.Text = VTG.Sentence;
            }
            else
            {
                tbVTG.Text = "";
            }
            if (tbVTG.Text != "") Clipboard.SetText(tbVTG.Text);
        }

        private void btnZDA_Click(object sender, EventArgs e)
        {
            if (AGIOdata.Connected() || Properties.Settings.Default.Simulate)
            {
                tbZDA.Text = ZDA.Sentence;
            }
            else
            {
                tbZDA.Text = "";
            }
            if (tbZDA.Text != "") Clipboard.SetText(tbZDA.Text);
        }

        private void cboBaud1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SER.Baud = Convert.ToInt32(cboBaud1.Text);
        }

        private void cboGGA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboGGA.Text == "0")
            {
                tmrGGA.Enabled = false;
            }
            else
            {
                tmrGGA.Interval = 1000 / Convert.ToInt16(cboGGA.Text);
                tmrGGA.Enabled = true;
            }
            Properties.Settings.Default.GGA = cboGGA.SelectedIndex;
        }

        private void cboPort1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SER.PortNm = cboPort1.Text;
        }

        private void cboRMC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboRMC.Text == "0")
            {
                tmrRMC.Enabled = false;
            }
            else
            {
                tmrRMC.Interval = 1000 / Convert.ToInt16(cboRMC.Text);
                tmrRMC.Enabled = true;
            }
            Properties.Settings.Default.RMC = cboRMC.SelectedIndex;
        }

        private void cboVTG_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboVTG.Text == "0")
            {
                tmrVTG.Enabled = false;
            }
            else
            {
                tmrVTG.Interval = 1000 / Convert.ToInt16(cboVTG.Text);
                tmrVTG.Enabled = true;
            }
            Properties.Settings.Default.VTG = cboVTG.SelectedIndex;
        }

        private void cboZDA_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboZDA.Text == "0")
            {
                tmrZDA.Enabled = false;
            }
            else
            {
                tmrZDA.Interval = 1000 / Convert.ToInt16(cboZDA.Text);
                tmrZDA.Enabled = true;
            }
            Properties.Settings.Default.ZDA = cboZDA.SelectedIndex;
        }

        private void cbPrecision_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SentencePrecisionIndex = cboPrecision.SelectedIndex;

            int precision = Convert.ToInt32(cboPrecision.SelectedItem);
            string NewFormat = ".";
            for (int i = 0; i < precision; i++)
            {
                NewFormat += "0";
            }

            Properties.Settings.Default.SentencePrecisionFormat = "00" + NewFormat;
            Properties.Settings.Default.DisplayPrecisionFormat = "0" + NewFormat;

            tbGGA.Text = "";
            tbVTG.Text = "";
            tbRMC.Text = "";
            tbZDA.Text = "";
        }

        private void ckAutoConnect_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoConnect = ckAutoConnect.Checked;
        }

        private void ckAutoHide_CheckedChanged(object sender, EventArgs e)
        {
            tmrMinimize.Enabled = ckAutoHide.Checked;
            if (ckAutoHide.Checked) this.WindowState = FormWindowState.Minimized;
            Properties.Settings.Default.AutoHide = ckAutoHide.Checked;
        }

        private void ckGSA_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.SendGSA = ckGSA.Checked;
            tmrGSA.Enabled = ckGSA.Checked;
        }

        private void ckRoll_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.UseRollCorrected = ckRoll.Checked;
        }

        private void ckSimulate_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Simulate = ckSimulate.Checked;
            if (!ckSimulate.Checked)
            {
                tbGGA.Text = "";
                tbVTG.Text = "";
                tbRMC.Text = "";
                tbZDA.Text = "";
            }
        }

        private void frmStart_FormClosed(object sender, FormClosedEventArgs e)
        {
            Tls.SaveFormData(this);
            Properties.Settings.Default.Save();
            SER.Close();
        }

        private void frmStart_Load(object sender, EventArgs e)
        {
            //Properties.Settings.Default.Reset();
            Tls.LoadFormData(this);
            AGIOcomm.StartUDPServer();
            AOGcomm.StartUDPServer();
            LoadRCbox();
            PortIndicator1.BackColor = Properties.Settings.Default.DayColour;
            this.BackColor = Properties.Settings.Default.DayColour;
            tabPage1.BackColor = Properties.Settings.Default.DayColour;
            tabPage2.BackColor = Properties.Settings.Default.DayColour;

            ckAutoHide.Checked = Properties.Settings.Default.AutoHide;
            ckAutoConnect.Checked = Properties.Settings.Default.AutoConnect;
            ckRoll.Checked = Properties.Settings.Default.UseRollCorrected;
            rbGN.Checked = (Properties.Settings.Default.SentenceStart == "$GN");
            ckGSA.Checked = Properties.Settings.Default.SendGSA;

            cboGGA.SelectedIndex = Properties.Settings.Default.GGA;
            cboVTG.SelectedIndex = Properties.Settings.Default.VTG;
            cboRMC.SelectedIndex = Properties.Settings.Default.RMC;
            cboZDA.SelectedIndex = Properties.Settings.Default.ZDA;

            cboPrecision.SelectedIndex = Properties.Settings.Default.SentencePrecisionIndex;
            ckSimulate.Checked = Properties.Settings.Default.Simulate;

            if (ckAutoHide.Checked) this.WindowState = FormWindowState.Minimized;
        }

        private void frmStart_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                tmrMinimize.Enabled = false;
            }
            else
            {
                tmrMinimize.Enabled = ckAutoHide.Checked;
            }
        }

        private void groupBox1_Paint(object sender, PaintEventArgs e)
        {
            GroupBox box = sender as GroupBox;
            Tls.DrawGroupBox(box, e.Graphics, this.BackColor, Color.Black, Color.Blue);
        }

        private void LoadRCbox()
        {
            cboPort1.Items.Clear();
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboPort1.Items.Add(s);
            }
            SetPortButtons1();
        }

        private void rbGP_CheckedChanged(object sender, EventArgs e)
        {
            if (rbGP.Checked)
            {
                Properties.Settings.Default.SentenceStart = "$GP";
            }
            else
            {
                Properties.Settings.Default.SentenceStart = "$GN";
            }
        }

        private void Send()
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void tmrDisplay_Tick(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void tmrGGA_Tick(object sender, EventArgs e)
        {
            if (GGAsentence == "")
            {
                Watchdog = 0;
                if (AGIOdata.Connected() || Properties.Settings.Default.Simulate)
                {
                    GGAsentence = GGA.Build();
                    Send();
                }
            }
            else
            {
                Watchdog++;
                if (Watchdog > 10 && backgroundWorker1.WorkerSupportsCancellation == true && !backgroundWorker1.CancellationPending)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                }
            }
        }

        private void tmrGSA_Tick(object sender, EventArgs e)
        {
            if (GSAsentence == "")
            {
                Watchdog = 0;
                if (AGIOdata.Connected() || Properties.Settings.Default.Simulate) GSAsentence = GSA.Build();
            }
            else
            {
                Watchdog++;
                if (Watchdog > 10 && backgroundWorker1.WorkerSupportsCancellation == true && !backgroundWorker1.CancellationPending)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                }
            }
        }

        private void tmrMinimize_Tick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void tmrRMC_Tick(object sender, EventArgs e)
        {
            if (RMCsentence == "")
            {
                Watchdog = 0;
                if (AGIOdata.Connected() || Properties.Settings.Default.Simulate) RMCsentence = RMC.Build();
            }
            else
            {
                Watchdog++;
                if (Watchdog > 10 && backgroundWorker1.WorkerSupportsCancellation == true && !backgroundWorker1.CancellationPending)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                }
            }
        }

        private void tmrVTG_Tick(object sender, EventArgs e)
        {
            if (VTGsentence == "")
            {
                Watchdog = 0;
                if (AGIOdata.Connected() || Properties.Settings.Default.Simulate) VTGsentence = VTG.Build();
            }
            else
            {
                Watchdog++;
                if (Watchdog > 10 && backgroundWorker1.WorkerSupportsCancellation == true && !backgroundWorker1.CancellationPending)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                }
            }
        }

        private void tmrZDA_Tick(object sender, EventArgs e)
        {
            if (ZDAsentence == "")
            {
                Watchdog = 0;
                if (AGIOdata.Connected() || Properties.Settings.Default.Simulate) ZDAsentence = ZDA.Build();
            }
            else
            {
                Watchdog++;
                if (Watchdog > 10 && backgroundWorker1.WorkerSupportsCancellation == true && !backgroundWorker1.CancellationPending)
                {
                    // Cancel the asynchronous operation.
                    backgroundWorker1.CancelAsync();
                }
            }
        }

        private void UpdateForm()
        {
            if (this.WindowState != FormWindowState.Minimized)
            {
                if (UseAOGposition())
                {
                    lbLon.Text = AOGdata.Longitude.ToString(Properties.Settings.Default.DisplayPrecisionFormat);
                    lbLat.Text = AOGdata.Latitude.ToString(Properties.Settings.Default.DisplayPrecisionFormat);
                }
                else
                {
                    lbLon.Text = AGIOdata.Longitude.ToString(Properties.Settings.Default.DisplayPrecisionFormat);
                    lbLat.Text = AGIOdata.Latitude.ToString(Properties.Settings.Default.DisplayPrecisionFormat);
                }

                lbAge.Text = AGIOdata.Age.ToString("N2");
                lbSpeed.Text = AGIOdata.Speed.ToString("N1");
                lbQuality.Text = FixQuality(AGIOdata.FixQuality);
                lbHDOP.Text = AGIOdata.HDOP.ToString("N2");
                lbSats.Text = AGIOdata.Satellites.ToString("");
                lbElev.Text = AGIOdata.Altitude.ToString("N2");
                lbAge.Text = AGIOdata.Age.ToString("N1");

                ckRoll.Enabled = AOGdata.Connected();

                if (AGIOdata.Connected())
                {
                    lbQuality.BackColor = Color.Transparent;
                    label4.BackColor = Color.Transparent;
                    lbSim.Visible = false;
                    this.Text = "GPS_Out [" + Tls.AppVersion() + "]   " + HeadingType;
                }
                else if (Properties.Settings.Default.Simulate)
                {
                    lbQuality.BackColor = SimColor;
                    label4.BackColor = SimColor;
                    lbSim.Visible = true;
                    lbSim.BackColor = SimColor;
                    this.Text = "GPS_Out [" + Tls.AppVersion() + "]  Simulated Data   " + HeadingType;
                }
                else
                {
                    lbQuality.BackColor = Color.Transparent;
                    label4.BackColor = Color.Transparent;
                    lbSim.Visible = false;
                    this.Text = "GPS_Out [" + Tls.AppVersion() + "]   NO DATA";
                }
            }
        }
    }
}
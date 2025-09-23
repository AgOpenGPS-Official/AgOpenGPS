using AgIO.Properties;
using AgLibrary.Logging;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormLoop : Form
    {
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        public static extern bool IsIconic(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        //key event to restore window
        private const int ALT = 0xA4;

        private const int EXTENDEDKEY = 0x1;
        private const int KEYUP = 0x2;

        //Stringbuilder
        public StringBuilder logNMEASentence = new StringBuilder();

        public StringBuilder logMonitorSentence = new StringBuilder();
        public StringBuilder logUDPSentence = new StringBuilder();
        public bool isLogNMEA, isLogMonitorOn, isUDPMonitorOn, isGPSLogOn, isNTRIPLogOn;

        private StringBuilder sbRTCM = new StringBuilder();

        public bool isKeyboardOn = true;

        public bool isSendToSerial = true, isSendToUDP = false;

        public bool isGPSSentencesOn = false, isSendNMEAToUDP;

        //timer variables
        public double secondsSinceStart, twoSecondTimer, tenSecondTimer, threeMinuteTimer, pingSecondsStart;

        public bool isNTRIPToggle;

        //usually 256 - send ntrip to serial in chunks
        public int packetSizeNTRIP;

        public bool lastHelloGPS, lastHelloAutoSteer, lastHelloMachine, lastHelloIMU;
        public bool isConnectedIMU, isConnectedSteer, isConnectedMachine;

        //is the fly out displayed
        public bool isViewAdvanced = false;

        //used to hide the window and not update text fields and most counters
        public bool isAppInFocus = true, isLostFocus;

        public int focusSkipCounter = 310;

        public FormLoop()
        {
            InitializeComponent();
        }

        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            if (Settings.Default.setDisplay_StartMinimized)
            {
                this.WindowState = FormWindowState.Minimized;
            }
            if (Settings.Default.setUDP_isOn)
            {
                LoadUDPNetwork();
                Log.EventWriter("UDP Network Is On");
            }
            else
            {
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label9.Visible = false;

                lblSteerAngle.Visible = false;
                lblWASCounts.Visible = false;
                lblSwitchStatus.Visible = false;
                lblWorkSwitchStatus.Visible = false;

                label10.Visible = false;
                label12.Visible = false;
                lbl1To8.Visible = false;
                lbl9To16.Visible = false;

                btnUDP.BackColor = Color.Gainsboro;
                lblIP.Text = "Off";
            }

            //small view
            this.Width = 420;

            LoadLoopback();

            isSendNMEAToUDP = Properties.Settings.Default.setUDP_isSendNMEAToUDP;

            packetSizeNTRIP = Properties.Settings.Default.setNTRIP_packetSize;

            isSendToSerial = Settings.Default.setNTRIP_sendToSerial;
            isSendToUDP = Settings.Default.setNTRIP_sendToUDP;

            //lblMount.Text = Properties.Settings.Default.setNTRIP_mount;

            lblGPS1Comm.Text = "";
            lblIMUComm.Text = "";
            lblMod1Comm.Text = "";
            lblMod2Comm.Text = "";

            //set baud and port from last time run
            baudRateGPS = Settings.Default.setPort_baudRateGPS;
            portNameGPS = Settings.Default.setPort_portNameGPS;
            wasGPSConnectedLastRun = Settings.Default.setPort_wasGPSConnected;
            if (wasGPSConnectedLastRun)
            {
                OpenGPSPort();
                if (spGPS.IsOpen) lblGPS1Comm.Text = portNameGPS;
            }

            // set baud and port for rtcm from last time run
            baudRateRtcm = Settings.Default.setPort_baudRateRtcm;
            portNameRtcm = Settings.Default.setPort_portNameRtcm;
            wasRtcmConnectedLastRun = Settings.Default.setPort_wasRtcmConnected;

            if (wasRtcmConnectedLastRun)
            {
                OpenRtcmPort();
            }

            //Open IMU
            portNameIMU = Settings.Default.setPort_portNameIMU;
            wasIMUConnectedLastRun = Settings.Default.setPort_wasIMUConnected;
            if (wasIMUConnectedLastRun)
            {
                OpenIMUPort();
                if (spIMU.IsOpen) lblIMUComm.Text = portNameIMU;
            }

            //same for SteerModule port
            portNameSteerModule = Settings.Default.setPort_portNameSteer;
            wasSteerModuleConnectedLastRun = Settings.Default.setPort_wasSteerModuleConnected;
            if (wasSteerModuleConnectedLastRun)
            {
                OpenSteerModulePort();
                if (spSteerModule.IsOpen) lblMod1Comm.Text = portNameSteerModule;
            }

            //same for MachineModule port
            portNameMachineModule = Settings.Default.setPort_portNameMachine;
            wasMachineModuleConnectedLastRun = Settings.Default.setPort_wasMachineModuleConnected;
            if (wasMachineModuleConnectedLastRun)
            {
                OpenMachineModulePort();
                if (spMachineModule.IsOpen) lblMod2Comm.Text = portNameMachineModule;
            }

            ConfigureNTRIP();

            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                lblSerialPorts.Text = "None";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    lblSerialPorts.Text = ports[i] + "\r\n";
                }
            }

            isConnectedIMU = cboxIsIMUModule.Checked = Properties.Settings.Default.setMod_isIMUConnected;
            isConnectedSteer = cboxIsSteerModule.Checked = Properties.Settings.Default.setMod_isSteerConnected;
            isConnectedMachine = cboxIsMachineModule.Checked = Properties.Settings.Default.setMod_isMachineConnected;

            //On or off the module rows
            SetModulesOnOff();

            oneSecondLoopTimer.Enabled = true;
            pictureBox1.Visible = true;
            pictureBox1.BringToFront();
            pictureBox1.Width = 430;
            pictureBox1.Height = 500;
            pictureBox1.Left = 0;
            pictureBox1.Top = 0;
            //pictureBox1.Dock = DockStyle.Fill;:

            //update Caster IP from URL, just use the old one if can't find
            if (isNTRIP_RequiredOn)
            {
                //broadCasterIP = Properties.Settings.Default.setNTRIP_casterIP; //Select correct Address
                broadCasterIP = null;
                string actualIP = Properties.Settings.Default.setNTRIP_casterURL.Trim();

                try
                {
                    IPAddress[] addresslist = Dns.GetHostAddresses(actualIP);
                    foreach (IPAddress address in addresslist)
                    {
                        if (address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            broadCasterIP = address.ToString().Trim();
                            Properties.Settings.Default.setNTRIP_casterIP = broadCasterIP;
                            Properties.Settings.Default.Save();
                            break;
                        }
                    }

                    if (broadCasterIP == null) throw new NullReferenceException();
                }
                catch (Exception ex)
                {
                    Log.EventWriter(ex.ToString());
                    TimedMessageBox(1500, "URL Not Located, Network Down?", "Cannot Find: " + Properties.Settings.Default.setNTRIP_casterURL);
                    //if we had a timer already, kill it
                    tmr?.Dispose();

                    //use last known
                    broadCasterIP = Properties.Settings.Default.setNTRIP_casterIP; //Select correct Address

                    // Close the socket if it is still open
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        clientSocket.Shutdown(SocketShutdown.Both);
                        System.Threading.Thread.Sleep(100);
                        clientSocket.Close();
                    }

                    //TimedMessageBox(2000, "NTRIP Not Connected", " Reconnect Request");
                    ntripCounter = 15;
                    isNTRIP_Connected = false;
                    isNTRIP_Starting = false;
                    isNTRIP_Connecting = false;
                    return;
                }
            }

            this.Text =
            "AgIO  v" + Program.Version + " Profile: " + RegistrySettings.profileName;

            if (RegistrySettings.profileName == "")
            {
                Log.EventWriter("Using Default Profile At Start Warning");

                YesMessageBox("AgIO - No Profile Open \r\n\r\n Create or Open a Profile");

                using (var form = new FormProfiles(this))
                {
                    form.ShowDialog(this);
                    if (form.DialogResult == DialogResult.Yes)
                    {
                        Log.EventWriter("Program Reset: Saving or Selecting Profile");

                        Program.Restart();
                    }
                }
                this.Text = "AgIO  v" + Program.Version + " Profile: "
                    + RegistrySettings.profileName;
            }

            if (Settings.Default.setDisplay_isAutoRunGPS_Out)
            {
                StartGPS_Out();
                Log.EventWriter("Run GPS_Out");
            }

        }

        private void FormLoop_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.setPort_wasGPSConnected = wasGPSConnectedLastRun;
            Settings.Default.setPort_wasIMUConnected = wasIMUConnectedLastRun;
            Settings.Default.setPort_wasSteerModuleConnected = wasSteerModuleConnectedLastRun;
            Settings.Default.setPort_wasMachineModuleConnected = wasMachineModuleConnectedLastRun;
            Settings.Default.setPort_wasRtcmConnected = wasRtcmConnectedLastRun;

            Settings.Default.Save();

            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                }
                finally { loopBackSocket.Close(); }
            }

            if (UDPSocket != null)
            {
                try
                {
                    UDPSocket.Shutdown(SocketShutdown.Both);
                }
                finally { UDPSocket.Close(); }
            }

            Process[] processName = Process.GetProcessesByName("GPS_Out");
            if (processName.Length != 0)
            {
                processName[0].CloseMainWindow();
            }

            Log.EventWriter("Program Exit: " +
                DateTime.Now.ToString("f", CultureInfo.InvariantCulture) + "\n\r");

            Log.FileSaveSystemEvents();
        }

        private void oneSecondLoopTimer_Tick(object sender, EventArgs e)
        {
            if (oneSecondLoopTimer.Interval > 1200)
            {
                Controls.Remove(pictureBox1);
                pictureBox1.Dispose();
                oneSecondLoopTimer.Interval = 1000;
                this.Width = 428;
                this.Height = 530;
                return;
            }

            //to check if new data for subnet

            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            if (focusSkipCounter != 0)
            {
                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }

            //do all the NTRIP routines
            DoNTRIPSecondRoutine();

            #region Sleep

            //is this the active window
            isAppInFocus = FormLoop.ActiveForm != null;

            //start counting down to minimize
            if (!isAppInFocus && !isLostFocus)
            {
                focusSkipCounter = 310;
                isLostFocus = true;
            }

            // Is active window again
            if (isAppInFocus && isLostFocus)
            {
                isLostFocus = false;
                focusSkipCounter = int.MaxValue;
            }

            if (isLostFocus && focusSkipCounter != 0)
            {
                if (focusSkipCounter == 1)
                {
                    WindowState = FormWindowState.Minimized;
                }

                focusSkipCounter--;
            }

            #endregion Sleep

            //every couple or so seconds
            if ((secondsSinceStart - twoSecondTimer) > 2)
            {
                TwoSecondLoop();
                twoSecondTimer = secondsSinceStart;
            }

            //every 10 seconds
            if ((secondsSinceStart - tenSecondTimer) > 9.5)
            {
                TenSecondLoop();
                tenSecondTimer = secondsSinceStart;
            }

            //3 minute egg timer
            if ((secondsSinceStart - threeMinuteTimer) > 180)
            {
                ThreeMinuteLoop();
                threeMinuteTimer = secondsSinceStart;
            }

            // 1 Second Loop Part2
            if (isViewAdvanced)
            {
                if (isNTRIP_RequiredOn)
                {
                    sbRTCM.Append(".");
                    lblMessages.Text = sbRTCM.ToString();
                }
            }

            if (focusSkipCounter != 0)
            {
                if (ntripCounter > 30)
                {
                    isNTRIPToggle = !isNTRIPToggle;
                    if (isNTRIPToggle) lblNTRIPBytes.BackColor = Color.CornflowerBlue;
                    else lblNTRIPBytes.BackColor = Color.DarkOrange;
                }
                else
                {
                    lblNTRIPBytes.BackColor = Color.Transparent;
                }
            }
        }

        private void TwoSecondLoop()
        {
            //Hello Alarm logic
            DoHelloAlarmLogic();

            DoTraffic();

            if (isViewAdvanced)
            {
                pingSecondsStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;
                lblPing.Text = lblPingMachine.Text = "*";
            }

            //send a hello to modules
            SendUDPMessage(helloFromAgIO, epModule);


            //if (isLogNMEA)
            //{
            //    using (StreamWriter writer = new StreamWriter("zAgIO_log.txt", true))
            //    {
            //        writer.Write(logNMEASentence.ToString());
            //    }
            //    logNMEASentence.Clear();
            //}

            //if (focusSkipCounter < 310) lblSkipCounter.Text = focusSkipCounter.ToString();
            //else lblSkipCounter.Text = "On";
        }

        private void TenSecondLoop()
        {
            if (focusSkipCounter != 0 && WindowState == FormWindowState.Minimized)
            {
                focusSkipCounter = 0;
                isLostFocus = true;
            }

            if (focusSkipCounter != 0)
            {
                //update connections
                lblIP.Text = "";
                foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
                {
                    if (IPA.AddressFamily == AddressFamily.InterNetwork)
                    {
                        _ = IPA.ToString();
                        lblIP.Text += IPA.ToString() + "\r\n";
                    }
                }

                if (isViewAdvanced && isNTRIP_RequiredOn)
                {
                    try
                    {
                        //add the uniques messages to all the new ones
                        foreach (var item in aList)
                        {
                            rList.Add(item);
                        }

                        //sort and group using Linq
                        sbRTCM.Clear();

                        var g = rList.GroupBy(i => i)
                            .OrderBy(grp => grp.Key);
                        int count = 0;
                        aList.Clear();

                        //Create the text box of unique message numbers
                        foreach (var grp in g)
                        {
                            aList.Add(grp.Key);
                            sbRTCM.AppendLine(grp.Key + " - " + (grp.Count() - 1));
                            count++;
                        }

                        rList?.Clear();

                        //too many messages or trash
                        if (count > 25)
                        {
                            aList?.Clear();
                            sbRTCM.Clear();
                            sbRTCM.Append("Reset..");
                        }

                        lblMessagesFound.Text = count.ToString();
                    }
                    catch
                    {
                        sbRTCM.Clear();
                        sbRTCM.Append("Error");
                        Log.EventWriter("RTCM List compilation error");
                    }
                }

                #region Serial update

                if (wasIMUConnectedLastRun)
                {
                    if (!spIMU.IsOpen)
                    {
                        byte[] imuClose = new byte[] { 0x80, 0x81, 0x7C, 0xD4, 2, 1, 0, 83 };

                        //tell AOG IMU is disconnected
                        SendToLoopBackMessageAOG(imuClose);
                        wasIMUConnectedLastRun = false;
                        lblIMUComm.Text = "";
                    }
                }

                if (wasGPSConnectedLastRun)
                {
                    if (!spGPS.IsOpen)
                    {
                        wasGPSConnectedLastRun = false;
                        lblGPS1Comm.Text = "";
                    }
                }

                if (wasSteerModuleConnectedLastRun)
                {
                    if (!spSteerModule.IsOpen)
                    {
                        wasSteerModuleConnectedLastRun = false;
                        lblMod1Comm.Text = "";
                    }
                }

                if (wasMachineModuleConnectedLastRun)
                {
                    if (!spMachineModule.IsOpen)
                    {
                        wasMachineModuleConnectedLastRun = false;
                        lblMod2Comm.Text = "";
                    }
                }

                #endregion Serial update
            }
        }

        private void ThreeMinuteLoop()
        {
            if (isViewAdvanced)
            {
                btnSlide.PerformClick();
            }
        }

        private void DoHelloAlarmLogic()
        {
            bool currentHello;

            if (isConnectedMachine)
            {
                currentHello = traffic.helloFromMachine < 3;

                if (currentHello != lastHelloMachine)
                {
                    if (currentHello) btnMachine.BackColor = Color.LimeGreen;
                    else btnMachine.BackColor = Color.Red;
                    lastHelloMachine = currentHello;
                    ShowAgIO();
                }
            }

            if (isConnectedSteer)
            {
                currentHello = traffic.helloFromAutoSteer < 3;

                if (currentHello != lastHelloAutoSteer)
                {
                    if (currentHello) btnSteer.BackColor = Color.LimeGreen;
                    else btnSteer.BackColor = Color.Red;
                    lastHelloAutoSteer = currentHello;
                    ShowAgIO();
                }
            }

            if (isConnectedIMU)
            {
                currentHello = traffic.helloFromIMU < 3;

                if (currentHello != lastHelloIMU)
                {
                    if (currentHello) btnIMU.BackColor = Color.LimeGreen;
                    else btnIMU.BackColor = Color.Red;
                    lastHelloIMU = currentHello;
                    ShowAgIO();
                }
            }

            currentHello = traffic.cntrGPSOut != 0;

            if (currentHello != lastHelloGPS)
            {
                if (currentHello) btnGPS.BackColor = Color.LimeGreen;
                else btnGPS.BackColor = Color.Red;
                lastHelloGPS = currentHello;
                ShowAgIO();
            }
        }

        private void FormLoop_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                if (isViewAdvanced) btnSlide.PerformClick();
                isLostFocus = true;
                focusSkipCounter = 0;
            }
        }

        private void ShowAgIO()
        {
            Process[] processName = Process.GetProcessesByName("AgIO");

            if (processName.Length != 0)
            {
                // Guard: check if window already has focus.
                if (processName[0].MainWindowHandle == GetForegroundWindow()) return;

                // Show window maximized.
                ShowWindow(processName[0].MainWindowHandle, 9);

                // Simulate an "ALT" key press.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | 0, 0);

                // Simulate an "ALT" key release.
                keybd_event((byte)ALT, 0x45, EXTENDEDKEY | KEYUP, 0);

                // Show window in forground.
                SetForegroundWindow(processName[0].MainWindowHandle);
            }

            //{
            //    //Set foreground window
            //    if (IsIconic(processName[0].MainWindowHandle))
            //    {
            //        ShowWindow(processName[0].MainWindowHandle, 9);
            //    }
            //    SetForegroundWindow(processName[0].MainWindowHandle);
            //}
        }

        public void SetModulesOnOff()
        {
            if (isConnectedIMU)
            {
                btnIMU.Visible = true;
                lblIMUComm.Visible = true;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnIMU.Visible = false;
                lblIMUComm.Visible = false;
                cboxIsIMUModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedMachine)
            {
                btnMachine.Visible = true;
                lblMod2Comm.Visible = true;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnMachine.Visible = false;
                lblMod2Comm.Visible = false;
                cboxIsMachineModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (isConnectedSteer)
            {
                btnSteer.Visible = true;
                lblMod1Comm.Visible = true;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.Cancel64;
            }
            else
            {
                btnSteer.Visible = false;
                lblMod1Comm.Visible = false;
                cboxIsSteerModule.BackgroundImage = Properties.Resources.AddNew;
            }

            if (cboxIsIMUModule.Checked != Properties.Settings.Default.setMod_isIMUConnected ||
                cboxIsSteerModule.Checked != Properties.Settings.Default.setMod_isSteerConnected ||
                cboxIsMachineModule.Checked != Properties.Settings.Default.setMod_isMachineConnected)
            {

                Properties.Settings.Default.setMod_isIMUConnected = isConnectedIMU;
                Properties.Settings.Default.setMod_isSteerConnected = isConnectedSteer;
                Properties.Settings.Default.setMod_isMachineConnected = isConnectedMachine;

                Properties.Settings.Default.Save();
            }
        }

        private void DoTraffic()
        {
            traffic.helloFromMachine++;
            traffic.helloFromAutoSteer++;
            traffic.helloFromIMU++;

            if (focusSkipCounter != 0)
            {
                lblFromGPS.Text = traffic.cntrGPSOut == 0 ? "---" : ((traffic.cntrGPSOut >> 1)).ToString();

                //reset all counters
                traffic.cntrGPSOut = 0;

                lblCurentLon.Text = longitude.ToString("N7");
                lblCurrentLat.Text = latitude.ToString("N7");
            }
        }

        // Buttons, Checkboxes and Clicks  ***************************************************

        private void RescanPorts()
        {
            string[] ports = System.IO.Ports.SerialPort.GetPortNames();

            if (ports.Length == 0)
            {
                lblSerialPorts.Text = "None";
            }
            else
            {
                for (int i = 0; i < ports.Length; i++)
                {
                    lblSerialPorts.Text = ports[i] + " ";
                }
            }
        }
    }
}
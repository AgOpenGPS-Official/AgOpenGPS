using AgIO.Forms;
using AgIO.Properties;
using AgLibrary.Forms;
using AgLibrary.Logging;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormLoop
    {
        public void TimedMessageBox(int timeout, string title, string message)
        {
            var form = new FormTimedMessage(timeout, title, message);
            form.Show();
        }

        public void YesMessageBox(string s1)
        {
            var form = new FormYes(s1);
            form.ShowDialog(this);
        }

        #region Buttons

        private void btnGPS_Out_Click(object sender, EventArgs e)
        {
            StartGPS_Out();
        }

        private void btnSlide_Click(object sender, EventArgs e)
        {
            if (this.Width < 600)
            {
                this.Width = 760;
                isViewAdvanced = true;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnLeft;
                sbRTCM.Clear();
                lblMessages.Text = "Reading...";
                threeMinuteTimer = secondsSinceStart;
                lblMessagesFound.Text = "-";
                aList.Clear();
                rList.Clear();
            }
            else
            {
                this.Width = 428;
                isViewAdvanced = false;
                btnSlide.BackgroundImage = Properties.Resources.ArrowGrnRight;
                aList.Clear();
                rList.Clear();
                lblMessages.Text = "Reading...";
                lblMessagesFound.Text = "-";
                aList.Clear();
                rList.Clear();
            }
        }

        private void btnStartStopNtrip_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setNTRIP_isOn || Properties.Settings.Default.setRadio_isOn)
            {
                if (isNTRIP_RequiredOn || isRadio_RequiredOn)
                {
                    ShutDownNTRIP();
                    lblWatch.Text = "Stopped";
                    btnStartStopNtrip.Text = "OffLine";
                    isNTRIP_RequiredOn = false;
                    isRadio_RequiredOn = false;
                    lblNTRIP_IP.Text = "--";
                    lblMount.Text = "--";
                }
                else
                {
                    isNTRIP_RequiredOn = Properties.Settings.Default.setNTRIP_isOn;
                    isRadio_RequiredOn = Properties.Settings.Default.setRadio_isOn;
                    lblWatch.Text = "Waiting";
                    lblNTRIP_IP.Text = "--";
                    lblMount.Text = "--";
                }
            }
            else
            {
                TimedMessageBox(2000, "Turn on NTRIP", "NTRIP Client Not Set Up");
            }
        }

        private void btnMinimizeMainForm_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnGPSData_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                isGPSSentencesOn = false;
                return;
            }

            isGPSSentencesOn = true;

            Form form = new FormGPSData(this);
            form.Show(this);
        }

        private void btnBringUpCommSettings_Click(object sender, EventArgs e)
        {
            SettingsCommunicationGPS();
            RescanPorts();
        }

        private void btnUDP_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }
            if (!Settings.Default.setUDP_isOn) SettingsEthernet();
            else SettingsUDP();
        }

        private void btnRunAOG_Click(object sender, EventArgs e)
        {
            StartAOG();
        }

        private void btnNTRIP_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }

            SettingsNTRIP();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            using (var dlg = new FormYes("!!! This will stop communicating with Hardware, Are you Sure? !!!", true))
            {
                var result = dlg.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    Close();
                }
            }
        }

        private void btnRadio_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }

            SettingsRadio();
        }

        #endregion

        #region labels
        private void lblIP_Click(object sender, EventArgs e)
        {
            lblIP.Text = "";
            foreach (IPAddress IPA in Dns.GetHostAddresses(Dns.GetHostName()))
            {
                if (IPA.AddressFamily == AddressFamily.InterNetwork)
                {
                    _ = IPA.ToString();
                    lblIP.Text += IPA.ToString() + "\r\n";
                }
            }
        }
        private void lblMessages_Click(object sender, EventArgs e)
        {
            aList?.Clear();
            sbRTCM.Clear();
            sbRTCM.Append("Reset..");
        }

        private void lblNTRIPBytes_Click(object sender, EventArgs e)
        {
            tripBytes = 0;
        }

        #endregion

        #region CheckBoxes
        private void cboxIsSteerModule_Click(object sender, EventArgs e)
        {
            isConnectedSteer = cboxIsSteerModule.Checked;
            SetModulesOnOff();
        }

        private void cboxIsMachineModule_Click(object sender, EventArgs e)
        {
            isConnectedMachine = cboxIsMachineModule.Checked;
            SetModulesOnOff();
        }

        private void cboxIsIMUModule_Click(object sender, EventArgs e)
        {
            isConnectedIMU = cboxIsIMUModule.Checked;
            SetModulesOnOff();
        }

        #endregion

        #region Menu Strip Items

        private void toolStripLogViewer_Click(object sender, EventArgs e)
        {
            Form form = new FormEventViewer(Path.Combine(RegistrySettings.logsDirectory, "AgIO_Events_Log.txt"));
            form.Show(this);
            this.Activate();
        }

        private void toolStripUDPMonitor_Click(object sender, EventArgs e)
        {
            ShowUDPMonitor();
        }

        private void toolStripSerialMonitor_Click(object sender, EventArgs e)
        {
            ShowSerialMonitor();
        }

        private void toolStripAgDiag_Click(object sender, EventArgs e)
        {
            Process[] processName = Process.GetProcessesByName("AgDiag");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "AgDiag.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgDiag");
                    Log.EventWriter("Catch -> Failed to load AgDiag - Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }

        private void deviceManagerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("devmgmt.msc");
        }

        private void serialPassThroughToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "")
            {
                TimedMessageBox(3000, "Using Default Profile", "Choose Existing or Create New Profile");
                return;
            }

            if (isRadio_RequiredOn)
            {
                TimedMessageBox(2000, "Radio NTRIP ON", "Turn it off before using Serial Pass Thru");
                return;
            }

            if (isNTRIP_RequiredOn)
            {
                TimedMessageBox(2000, "Air NTRIP ON", "Turn it off before using Serial Pass Thru");
                return;
            }

            using (var form = new FormSerialPass(this))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    ////Clicked Save
                    //Application.Restart();
                    //Environment.Exit(0);
                    Settings.Default.Save();
                }
            }
        }
        private void toolStripSettings_Click(object sender, EventArgs e)
        {
            using (var form = new FormAdvancedSettings())
            {
                form.ShowDialog(this);
            }
        }
        private void toolStripMenuProfiles_Click(object sender, EventArgs e)
        {
            if (RegistrySettings.profileName == "")
            {
                TimedMessageBox(3000, "AgIO Default Profile Used", "Create or Choose a Profile");
            }

            using (var form = new FormProfiles(this))
            {
                form.ShowDialog(this);
                if (form.DialogResult == DialogResult.Yes)
                {
                    Log.EventWriter("Program Reset: Saving or Selecting Profile");

                    Program.Restart();
                }
            }
            this.Text = "AgIO  v" + Program.Version + "   Using Profile: "
                + RegistrySettings.profileName;
        }

        private void modSimToolStrip_Click(object sender, EventArgs e)
        {
            Process[] processName = Process.GetProcessesByName("ModSim");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "ModSim.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find Simulator");
                    Log.EventWriter("Catch -> Failed to load ModSim - Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }

        }

        private void toolStripEthernet_Click(object sender, EventArgs e)
        {
            SettingsEthernet();
        }

        #endregion

        public void ShowUDPMonitor()
        {
            var form = new FormUDPMonitor(this);
            form.Show(this);
        }

        public void ShowSerialMonitor()
        {
            var form = new FormSerialMonitor(this);
            form.Show(this);
        }

        private void SettingsCommunicationGPS()
        {
            isGPSCommOpen = true;

            using (FormCommSetGPS form = new FormCommSetGPS(this))
            {
                form.ShowDialog(this);
            }
            isGPSCommOpen = false;
        }

        private void SettingsEthernet()
        {
            using (FormEthernet form = new FormEthernet(this))
            {
                form.ShowDialog(this);
            }
        }

        private void SettingsNTRIP()
        {
            if (isRadio_RequiredOn)
            {
                TimedMessageBox(2000, "Radio NTRIP ON", "Turn it off before using NTRIP");
                return;
            }

            if (isSerialPass_RequiredOn)
            {
                TimedMessageBox(2000, "Serial NTRIP ON", "Turn it off before using NTRIP");
                return;
            }


            using (var form = new FormNtrip(this))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    if (isNTRIP_Connected)
                    {
                        SettingsShutDownNTRIP();
                    }
                    Settings.Default.Save();
                }
            }
        }

        private void SettingsRadio()
        {
            if (isSerialPass_RequiredOn)
            {
                TimedMessageBox(2000, "Serial Pass NTRIP ON", "Turn it off before using Radio NTRIP");
                return;
            }

            if (isNTRIP_RequiredOn)
            {
                TimedMessageBox(2000, "Air NTRIP ON", "Turn it off before using Radio NTRIP");
                return;
            }

            if (isRadio_RequiredOn && isNTRIP_Connected)
            {
                ShutDownNTRIP();
                lblWatch.Text = "Stopped";
                btnStartStopNtrip.Text = "OffLine";
                isRadio_RequiredOn = false;
            }

            using (var form = new FormRadio(this))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    Settings.Default.Save();
                }
            }
        }

        private void SettingsUDP()
        {
            FormUDP formEth = new FormUDP(this);
            {
                formEth.Show(this);
            }
        }

        private void StartAOG()
        {
            Process[] processName = Process.GetProcessesByName("AgOpenGPS");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "AgOpenGPS.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find AgOpenGPS");
                    Log.EventWriter("Can't Find AgOpenGPS - File Not Found");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }

        private void StartGPS_Out()
        {
            Process[] processName = Process.GetProcessesByName("GPS_Out");
            if (processName.Length == 0)
            {
                //Start application here
                string strPath = Path.Combine(Application.StartupPath, "GPS_Out.exe");

                try
                {
                    ProcessStartInfo processInfo = new ProcessStartInfo();
                    processInfo.FileName = strPath;
                    processInfo.WorkingDirectory = Path.GetDirectoryName(strPath);
                    Process proc = Process.Start(processInfo);
                }
                catch
                {
                    TimedMessageBox(2000, "No File Found", "Can't Find GPS_Out");
                    Log.EventWriter("No File Found, Can't Find GPS_Out");
                }
            }
            else
            {
                //Set foreground window
                ShowWindow(processName[0].MainWindowHandle, 9);
                SetForegroundWindow(processName[0].MainWindowHandle);
            }
        }

        private ToolStripDropDownButton toolStripDropDownButton1;
        private ToolStripMenuItem toolStripMenuProfiles;
        private ToolStripMenuItem deviceManagerToolStripMenuItem;
    }
}

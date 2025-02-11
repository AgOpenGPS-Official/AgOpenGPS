using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace AgIO
{
    public partial class FormISOBUS : Form
    {
        private Process aogTaskControllerProcess;

        public FormISOBUS()
        {
            InitializeComponent();
            cboxRadioAdapter.SelectedIndex = Properties.Settings.Default.isobus_canAdapterIndex;
            cboxRadioChannel.SelectedIndex = Properties.Settings.Default.isobus_canChannelIndex;
        }

        private void btnOpenIsobus_Click(object sender, EventArgs e)
        {
            StartAogTaskController();
        }

        private void btnCloseIsobus_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.isobus_isOn = false;
            StopAogTaskControllerProcess();
        }

        public void StartAogTaskController()
        {
            textBoxRcv.Clear();
            try
            {
                string path = GetInstallationPath();
                if (string.IsNullOrEmpty(path))
                {
                    MessageBox.Show("AOG-TaskController is not installed??");
                    return;
                }

                // Stop any other running instances
                Process[] processes = Process.GetProcessesByName("AOG-TaskController");
                foreach (Process process in processes)
                {
                    process.Kill();
                }

                path += @"\bin\AOG-TaskController.exe";

                var arguments = $"--can_adapter={cboxRadioAdapter.SelectedItem} --can_channel={cboxRadioChannel.SelectedItem} --log_level=debug  --log2file";

                aogTaskControllerProcess = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = path,
                        Arguments = arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    },
                    EnableRaisingEvents = true
                };

                aogTaskControllerProcess.OutputDataReceived += AogTaskControllerProcess_OutputDataReceived;
                aogTaskControllerProcess.ErrorDataReceived += AogTaskControllerProcess_OutputDataReceived;
                aogTaskControllerProcess.Exited += (_, __) =>
                {
                    UpdateComponentVisibility();
                };
                aogTaskControllerProcess.Start();
                aogTaskControllerProcess.BeginOutputReadLine();

                UpdateComponentVisibility();

                Properties.Settings.Default.isobus_isOn = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void StopAogTaskControllerProcess()
        {
            try
            {
                if (aogTaskControllerProcess == null || aogTaskControllerProcess.HasExited)
                    return;

                aogTaskControllerProcess.CloseMainWindow();
                if (!aogTaskControllerProcess.WaitForExit(5000))
                {
                    aogTaskControllerProcess.Kill(); // Force close if not exiting gracefully
                }
                aogTaskControllerProcess.Close();
                aogTaskControllerProcess = null;
                AppendLog(">>> AOG-TaskController.exe stopped");
                UpdateComponentVisibility();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            UpdateComponentVisibility();
        }

        private void AogTaskControllerProcess_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (!string.IsNullOrEmpty(e.Data))
            {
                AppendLog(e.Data);
            }
        }

        private void AppendLog(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), message);
            }
            else
            {
                textBoxRcv.AppendText(message + Environment.NewLine);
            }
        }

        private void btnIsobusOK_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void linkDownloadIsobus_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "https://www.github.com/GwnDaan/AOG-TaskController",
                    UseShellExecute = true // Required to open URLs in the browser
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., no default browser set)
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private static string GetInstallationPath()
        {
            string path = @"SOFTWARE\AOG-TaskController";

            foreach (RegistryView view in new[] { RegistryView.Registry64, RegistryView.Registry32 })
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, view))
                using (RegistryKey key = baseKey.OpenSubKey(path))
                {
                    if (key != null)
                    {
                        return key.GetValue("").ToString();
                    }
                }
            }

            return null;
        }

        private void UpdateComponentVisibility()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateComponentVisibility));
            }
            else
            {
                string path = GetInstallationPath();
                if (string.IsNullOrEmpty(path))
                {
                    flowLayoutDownloadIsobus.Visible = true;
                    flowLayoutCANAdapter.Visible = false;
                    btnOpenIsobus.Visible = false;
                    btnCloseIsobus.Visible = false;
                    cboxRadioAdapter.Enabled = false;
                    textBoxRcv.Visible = false;
                }
                else if (aogTaskControllerProcess == null || aogTaskControllerProcess.HasExited)
                {
                    flowLayoutDownloadIsobus.Visible = false;
                    flowLayoutCANAdapter.Visible = true;
                    btnOpenIsobus.Visible = true;
                    btnCloseIsobus.Visible = false;
                    cboxRadioAdapter.Enabled = true;
                    textBoxRcv.Visible = true;
                }
                else
                {
                    flowLayoutDownloadIsobus.Visible = false;
                    flowLayoutCANAdapter.Visible = true;
                    btnOpenIsobus.Visible = false;
                    btnCloseIsobus.Visible = true;
                    cboxRadioAdapter.Enabled = false;
                    textBoxRcv.Visible = true;
                }
                UpdateChannelSelection();
            }
        }

        private void cboxRadioAdapter_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateChannelSelection();
            Properties.Settings.Default.isobus_canAdapterIndex = cboxRadioAdapter.SelectedIndex;
        }

        private void UpdateChannelSelection()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(UpdateChannelSelection));
            }
            else
            {
                string path = GetInstallationPath();
                if (string.IsNullOrEmpty(path))
                {
                    flowLayoutChannel.Visible = false;
                    cboxRadioChannel.Enabled = false;
                }
                else
                {
                    // Check if current adapter allows channel selection
                    string adapter = cboxRadioAdapter.SelectedItem.ToString();

                    Dictionary<string, int> adapterChannels = new Dictionary<string, int>
                    {
                        { "PEAK-PCAN", 16 },
                        { "InnoMaker-USB2CAN", 2 },
                        { "Rusoku-TouCAN", 16 },
                        { "SYS-TEC-USB2CAN", 2 }
                    };
                    if (adapterChannels.ContainsKey(adapter))
                    {
                        flowLayoutChannel.Visible = true;
                        cboxRadioChannel.Items.Clear();
                        for (int i = 1; i <= adapterChannels[adapter]; i++)
                        {
                            cboxRadioChannel.Items.Add(i);
                        }
                    }
                    else
                    {
                        flowLayoutChannel.Visible = false;
                    }

                    // Set the channel index to the known item, only if the adapter is the same
                    if (Properties.Settings.Default.isobus_canAdapterIndex == cboxRadioAdapter.SelectedIndex)
                    {
                        cboxRadioChannel.SelectedIndex = Properties.Settings.Default.isobus_canChannelIndex;
                    }
                    else
                    {
                        cboxRadioChannel.SelectedIndex = 0;
                    }

                    // Disable channel selection if the process is running
                    if (aogTaskControllerProcess == null || aogTaskControllerProcess.HasExited)
                    {
                        cboxRadioChannel.Enabled = true;
                    }
                    else
                    {
                        cboxRadioChannel.Enabled = false;
                    }
                }
            }
        }

        private void cboxRadioChannel_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.isobus_canChannelIndex = cboxRadioChannel.SelectedIndex;
        }
    }
}

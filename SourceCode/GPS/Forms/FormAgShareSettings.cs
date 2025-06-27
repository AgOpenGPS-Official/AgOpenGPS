using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using AgOpenGPS.Controls;

namespace AgOpenGPS
{
    /// <summary>
    /// Form to configure AgShare server URL and API key, with live connection test and clipboard support.
    /// </summary>
    public partial class FormAgShareSettings : Form
    {
        private readonly AgShareClient _agShareClient;
        private Timer clipboardCheckTimer;

        public FormAgShareSettings()
        {
            _agShareClient = new AgShareClient(Settings.Default.AgShareServer, Settings.Default.AgShareApiKey);
            InitializeComponent();
        }

        // Load current settings and start clipboard monitoring
        private void FormAgShareSettings_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = Settings.Default.AgShareServer;
            textBoxApiKey.Text = Settings.Default.AgShareApiKey;

            UpdateAgShareToggleButton();
            UpdateAgShareUploadButton();

            btnPaste.Enabled = Clipboard.ContainsText();
            clipboardCheckTimer = new Timer();
            clipboardCheckTimer.Interval = 500;
            clipboardCheckTimer.Tick += ClipboardCheckTimer_Tick;
            clipboardCheckTimer.Start();

            // Event handlers to enable Save on text change
            textBoxApiKey.TextChanged += textBoxAnySetting_TextChanged;
            textBoxServer.TextChanged += textBoxAnySetting_TextChanged;

            // Disable TextboxServer for now until it's released
            textBoxServer.Enabled = false;
        }

        // Dispose timer when form closes
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (clipboardCheckTimer != null)
            {
                clipboardCheckTimer.Stop();
                clipboardCheckTimer.Dispose();
                clipboardCheckTimer = null;
            }
            base.OnFormClosed(e);
        }

        // Enable or disable the paste button based on clipboard content
        private void ClipboardCheckTimer_Tick(object sender, EventArgs e)
        {
            bool hasText = Clipboard.ContainsText();
            if (btnPaste.Enabled != hasText)
                btnPaste.Enabled = hasText;
        }

        // Test connection with current input values
        private async void buttonTestConnection_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Connecting...";
            labelStatus.ForeColor = Color.Gray;

            _agShareClient.SetBaseUrl(textBoxServer.Text);
            _agShareClient.SetApiKey(textBoxApiKey.Text);

            (bool success, string message) = await _agShareClient.CheckApiAsync();

            if (success)
            {
                labelStatus.Text = "✔ Connection successful";
                labelStatus.ForeColor = Color.Green;
                buttonSave.Enabled = true;
            }
            else
            {
                labelStatus.Text = $"❌ {message}";
                labelStatus.ForeColor = Color.Red;
            }
        }

        // Save current values to settings
        private void buttonSave_Click(object sender, EventArgs e)
        {
            _agShareClient.SetBaseUrl(textBoxServer.Text);
            _agShareClient.SetApiKey(textBoxApiKey.Text);

            Settings.Default.AgShareServer = textBoxServer.Text;
            Settings.Default.AgShareApiKey = textBoxApiKey.Text;
            Settings.Default.Save();

            labelStatus.Text = "✔ Settings saved";
            labelStatus.ForeColor = Color.Blue;
        }

        // Mark Save button active when text is edited
        private void textBoxAnySetting_TextChanged(object sender, EventArgs e)
        {
            buttonSave.Enabled = true;
        }

        // Update toggle button for upload-on/off
        private void UpdateAgShareToggleButton()
        {
            if (Settings.Default.AgShareEnabled)
            {
                btnToggleUpload.Image = Properties.Resources.UploadOn;
                btnToggleUpload.Text = "Activated";
            }
            else
            {
                btnToggleUpload.Image = Properties.Resources.UploadOff;
                btnToggleUpload.Text = "Deactivated";
                buttonSave.Enabled = true;
            }
        }

        // Toggle upload enabled state
        private void btnToggleUpload_Click(object sender, EventArgs e)
        {
            Settings.Default.AgShareEnabled = !Settings.Default.AgShareEnabled;
            UpdateAgShareToggleButton();
            Settings.Default.Save();
        }

        // Paste API key from clipboard
        private void btnPaste_Click(object sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textBoxApiKey.Text = Clipboard.GetText();
                Clipboard.Clear();
                btnPaste.Enabled = false;
            }
        }

        // Show onscreen keyboard for textBoxServer if enabled
        private void textBoxServer_Click(object sender, EventArgs e)
        {
            if (Settings.Default.setDisplay_isKeyboardOn)
            {
                FormGPS mf = this.Owner as FormGPS;
                if (mf != null)
                {
                    ((TextBox)sender).ShowKeyboard(this);
                    btnPaste.Focus();
                }
            }
        }

        // Open register link in default browser
        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "https://agshare.agopengps.com/register",
                UseShellExecute = true
            });
        }

        private void btnDevelop_Click(object sender, EventArgs e)
        {
            textBoxServer.Enabled = true;
        }

        private void btnAutoUpload_Click(object sender, EventArgs e)
        {
            Settings.Default.AgShareUploadActive = !Settings.Default.AgShareUploadActive;
            UpdateAgShareUploadButton();
        }
        private void UpdateAgShareUploadButton()
        {
            if (Settings.Default.AgShareUploadActive)
            {
                btnAutoUpload.Text = "Upload On";
                btnAutoUpload.Image = Resources.AutoUploadOn;
            }
            else
            {
                btnAutoUpload.Text = "Upload Off";
                btnAutoUpload.Image = Resources.AutoUploadOff;
                buttonSave.Enabled = true;
            }
        }
    }
}

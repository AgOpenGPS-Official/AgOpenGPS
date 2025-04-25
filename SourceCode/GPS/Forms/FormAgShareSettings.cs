using System;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public partial class FormAgShareSettings : Form
    {
        private readonly AgShareClient _agShareClient;

        public FormAgShareSettings(AgShareClient agShareClient)
        {
            _agShareClient = agShareClient;

            InitializeComponent();
        }

        private void FormAgShareSettings_Load(object sender, EventArgs e)
        {
            textBoxServer.Text = Settings.Default.AgShareServer;
            textBoxApiKey.Text = Settings.Default.AgShareApiKey ?? "";
        }

        private async void buttonTestConnection_Click(object sender, EventArgs e)
        {
            labelStatus.Text = "Connecting...";
            labelStatus.ForeColor = Color.Gray;

            (bool success, string message) = await _agShareClient.TestConnectionAsync(textBoxServer.Text, textBoxApiKey.Text);

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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            _agShareClient.SetServer(textBoxServer.Text);
            _agShareClient.SetApiKey(textBoxApiKey.Text);

            Settings.Default.AgShareServer = textBoxServer.Text;
            Settings.Default.AgShareApiKey = textBoxApiKey.Text;
            Settings.Default.Save();
        }
    }
}
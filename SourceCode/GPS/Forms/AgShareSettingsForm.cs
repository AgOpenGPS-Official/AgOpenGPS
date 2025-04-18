using System;
using System.Windows.Forms;
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public partial class AgShareSettingsForm : Form
    {
        private readonly AgShareClient _agShareClient;

        public AgShareSettingsForm()
        {
            InitializeComponent();
            txtApiKey.Text = Settings.Default.AgShareApiKey ?? "";
        }

        public AgShareSettingsForm(AgShareClient agShareClient)
            : this()
        {
            _agShareClient = agShareClient;
        }

        private async void btnTest_Click(object sender, EventArgs e)
        {
            lblStatus.Text = "Connecting...";
            lblStatus.ForeColor = System.Drawing.Color.Gray;

            SaveApiKey(txtApiKey.Text);

            bool result = await _agShareClient.TestApiKeyAsync();
            lblStatus.Text = result ? "✔ API key valid" : "❌ API key Not Valid";
            lblStatus.ForeColor = result ? System.Drawing.Color.Green : System.Drawing.Color.Red;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveApiKey(txtApiKey.Text);
            MessageBox.Show("API key opgeslagen.", "Opgeslagen", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveApiKey(string key)
        {
            _agShareClient.ApiKey = key;
            Settings.Default.AgShareApiKey = key;
            Settings.Default.Save();
        }
    }
}
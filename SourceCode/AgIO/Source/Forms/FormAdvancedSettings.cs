using System;
using System.Windows.Forms;
using AgIO.Properties;

namespace AgIO.Forms
{
    public partial class FormAdvancedSettings : Form
    {
        public FormAdvancedSettings()
        {
            InitializeComponent();
            cboxAutoRunGPS_Out.Checked = Settings.Default.setDisplay_isAutoRunGPS_Out;
            cboxStartMinimized.Checked = Settings.Default.setDisplay_StartMinimized;
            cboxShowOnWarning.Checked = Settings.Default.setDisplay_ShowOnWarning;
            cboxNtripErrors.Checked = Settings.Default.setNTRIP_showErrors;
        }

        private void cboxNtripErrors_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.setNTRIP_showErrors = cboxNtripErrors.Checked;
        }

        private void cboxStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.setDisplay_StartMinimized = cboxStartMinimized.Checked;
        }

        private void cboxAutoRunGPS_Out_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.setDisplay_isAutoRunGPS_Out = cboxAutoRunGPS_Out.Checked;
        }

        private void cboxShowOnWarning_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.setDisplay_ShowOnWarning = cboxShowOnWarning.Checked;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Settings.Default.Save();
            Close();
        }
    }
}

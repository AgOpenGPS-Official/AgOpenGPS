using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgIO.Properties;

namespace AgIO.Forms
{
    public partial class FormAdvancedSettings : Form
    {
        //class variables
        private readonly FormLoop mf = null;

        public FormAdvancedSettings(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormLoop;
            InitializeComponent();
            cboxAutoRunGPS_Out.Checked = Settings.Default.setDisplay_isAutoRunGPS_Out;
            cboxStartMinimized.Checked = Settings.Default.setDisplay_StartMinimized;


        }
        private void cboxStartMinimized_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.setDisplay_StartMinimized = cboxStartMinimized.Checked;
            Settings.Default.Save();
        }
        private void cboxAutoRunGPS_Out_CheckedChanged(object sender, EventArgs e)
        {
            Settings.Default.setDisplay_isAutoRunGPS_Out = cboxAutoRunGPS_Out.Checked;
            Settings.Default.Save();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}

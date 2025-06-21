using System;
using System.Windows.Forms;
using AgOpenGPS.Core.Translations;

namespace AgOpenGPS
{
    public partial class FormSaveOrNot : Form
    {
        //class variables
        private readonly FormGPS mf;
        
        int countExit = 4;
        int countShutdown = 5;

        public FormSaveOrNot(FormGPS gps)
        {
            InitializeComponent();
            mf = gps;

            //translations
            labelExit.Text = gStr.gsExit;
            labelShutdown.Text = gStr.gsShutdown;
            labelCancel.Text = gStr.gsCancel;
            labelExitToWindows.Text = gStr.gsExitToWindows;
            labelShutdownIn.Text = gStr.gsShutdownIn;

            //Trigger a snapshot to create a temp data file for the AgShare Upload
            if (mf.isJobStarted && Properties.Settings.Default.AgShareEnabled) mf.AgShareSnapshot();
        }

        private void FormSaveOrNot_Load(object sender, EventArgs e)
        {
            labelExitToWindows.Visible = !Properties.Settings.Default.setWindow_isShutdownComputer;
            lblExitCtr.Visible = !Properties.Settings.Default.setWindow_isShutdownComputer;
            labelShutdownIn.Visible = Properties.Settings.Default.setWindow_isShutdownComputer;
            lblShutCtr.Visible = Properties.Settings.Default.setWindow_isShutdownComputer;

            lblExitCtr.Text = countExit.ToString();
            lblShutCtr.Text = countShutdown.ToString();
        }

        //exit to windows
        private void btnOk_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.OK;
            Properties.Settings.Default.setWindow_isShutdownComputer = false;
            Close();
        }

        //just cancel and return to aog
        private void btnReturn_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Ignore;
            Close();
        }

        //turn off computer
        private void btnShutDown_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Properties.Settings.Default.setWindow_isShutdownComputer = true;
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setWindow_isShutdownComputer)
            {
                countShutdown--;
                lblShutCtr.Text = countShutdown.ToString();
                if (countShutdown < 0)
                {
                    DialogResult = DialogResult.Yes;
                    Close();
                }
            }
            else
            {
                countExit--;
                lblExitCtr.Text = countExit.ToString();
                if (countExit < 0)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }
    }
}
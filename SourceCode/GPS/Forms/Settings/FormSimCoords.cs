using AgOpenGPS.Culture;
using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSimCoords : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormSimCoords(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.gsEnterCoordinatesForSimulator;

            nudLatitude.Controls[0].Enabled = false;
            nudLongitude.Controls[0].Enabled = false;
        }

        private void FormSimCoords_Load(object sender, EventArgs e)
        {
            nudLatitude.Value = (decimal)Properties.Settings.Default.setGPS_SimLatitude;
            nudLongitude.Value = (decimal)Properties.Settings.Default.setGPS_SimLongitude;

            label2.Text = gStr.gsLatitude;
            label18.Text = gStr.gsLatitude;

            label1.Text = gStr.gsLongitude;
            label3.Text = gStr.gsLongitude;

        }



        private void bntOK_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                mf.TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                Close();
            }

            if (!mf.timerSim.Enabled)
            {
                mf.TimedMessageBox(2000, "Simulator is off", "Go Back To Work, No Time For Games");
                Close();
            }

            {
                mf.pn.latStart = (double)nudLatitude.Value;
                mf.pn.lonStart = (double)nudLongitude.Value;

                mf.pn.latitude = (double)nudLatitude.Value;
                mf.pn.longitude = (double)nudLongitude.Value;

                mf.sim.latitude = Properties.Settings.Default.setGPS_SimLatitude = (double)nudLatitude.Value;
                mf.sim.longitude = Properties.Settings.Default.setGPS_SimLongitude = (double)nudLongitude.Value;
                Properties.Settings.Default.Save();
            }

            mf.pn.SetLocalMetersPerDegree();

            Properties.Settings.Default.setGPS_SimLatitude = mf.sim.latitude;
            Properties.Settings.Default.setGPS_SimLongitude = mf.sim.longitude;
            Properties.Settings.Default.Save();
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nud_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);
        }
    }
}
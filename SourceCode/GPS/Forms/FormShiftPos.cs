using AgOpenGPS.Controls;
using AgOpenGPS.Culture;
using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormShiftPos : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormShiftPos(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            label27.Text = gStr.gsNorth;
            label2.Text = gStr.gsWest;
            label3.Text = gStr.gsEast;
            label4.Text = gStr.gsSouth;
            this.Text = gStr.gsShiftGPSPosition;
            nudEast.Controls[0].Enabled = false;
            nudNorth.Controls[0].Enabled = false;
        }

        private void FormShiftPos_Load(object sender, EventArgs e)
        {
            nudEast.Value = (decimal)mf.pn.fixOffset.easting * 100;
            nudNorth.Value = (decimal)mf.pn.fixOffset.northing * 100;
            chkOffsetsOn.Checked = mf.isKeepOffsetsOn;
            if (chkOffsetsOn.Checked) chkOffsetsOn.Text = "On";
            else chkOffsetsOn.Text = "Off";
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            nudNorth.UpButton();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            nudNorth.DownButton();
        }

        private void btnWest_MouseDown(object sender, MouseEventArgs e)
        {
            nudEast.DownButton();
        }

        private void btnEast_MouseDown(object sender, MouseEventArgs e)
        {
            nudEast.UpButton();
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            nudEast.Value = 0;
            nudNorth.Value = 0;
            mf.pn.fixOffset.easting = 0;
            mf.pn.fixOffset.northing = 0;
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.isKeepOffsetsOn = chkOffsetsOn.Checked;
            mf.pn.fixOffset.northing = (double)nudNorth.Value / 100;
            mf.pn.fixOffset.easting = (double)nudEast.Value / 100;
            Close();
        }

        private void chkOffsetsOn_Click(object sender, EventArgs e)
        {
            if (chkOffsetsOn.Checked) chkOffsetsOn.Text = "On";
            else chkOffsetsOn.Text = "Off";
        }

        private void nudNorth_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            mf.pn.fixOffset.northing = (double)nudNorth.Value / 100;
        }

        private void nudEast_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            mf.pn.fixOffset.easting = (double)nudEast.Value / 100;
        }
    }
}
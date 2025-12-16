//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGPSData : Form
    {
        private readonly FormGPS mf = null;

        public FormGPSData(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //lblTram.Text = mf.tram.controlByte.ToString();

            lblFrameTime.Text = mf.frameTime.ToString("N1");
            lblTimeSlice.Text = (1 / mf.timeSliceOfLastFix).ToString("N3");
            lblHz.Text = mf.gpsHz.ToString("N1");

            lblEastingField.Text = Math.Round(mf.pn.fix.easting, 1).ToString();
            lblNorthingField.Text = Math.Round(mf.pn.fix.northing, 1).ToString();

            lblLatitude.Text = mf.Latitude;
            lblLongitude.Text = mf.Longitude;

            //other sat and GPS info
            lblSatsTracked.Text = mf.SatsTracked;
            lblHDOP.Text = mf.HDOP;
            lblAge.Text = mf.pn.age.ToString("N1");
            //lblSpeed.Text = mf.avgSpeed.ToString("N2");

            //lblUturnByte.Text = Convert.ToString(mf.mc.machineData[mf.mc.mdUTurn], 2).PadLeft(6, '0');

            //lblRoll.Text = mf.RollInDegrees;
            lblIMUHeading.Text = mf.GyroInDegrees;
            lblFix2FixHeading.Text = mf.GPSHeading;
            lblFuzeHeading.Text = (mf.fixHeading * 57.2957795).ToString("N1");

            lblAngularVelocity.Text = mf.ahrs.imuYawRate.ToString("N2");

            lbludpWatchCounts.Text = mf.missedSentenceCount.ToString();

            if (mf.isMetric)
            {
                lblAltitude.Text = mf.Altitude;
            }
            else //imperial
            {
                lblAltitude.Text = mf.AltitudeFeet;
            }
        }

        private void FormGPSData_Load(object sender, EventArgs e)
        {
            this.Width = 120;
            this.Height = 350;
        }

        private void FormGPSData_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.isGPSSentencesOn = false;
        }
    }
}

using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Culture;
using AgOpenGPS.Helpers;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEnterFlag : Form
    {
        private readonly FormGPS mf = null;

        public FormEnterFlag(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = gStr.gsFormFlag;
            labelPoint.Text = gStr.gsPoint;
            nudLatitude.Controls[0].Enabled = false;
            nudLongitude.Controls[0].Enabled = false;

            nudLatitude.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitude.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
        }

        private void FormEnterAB_Load(object sender, EventArgs e)
        {
            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

        }

        private void nudLatitude_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudLongitude_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        public void CalcHeading()
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnRed_Click(object sender, EventArgs e)
        {
            Button btnRed = (Button)sender;
            byte flagColor = 0;

            if (btnRed.Name == "btnRed")
            {
                flagColor = 0;
            }
            else if (btnRed.Name == "btnGreen")
            {
                flagColor = 1;
            }
            else if (btnRed.Name == "btnYellow")
            {
                flagColor = 2;
            }

            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84((double)nudLatitude.Value, (double)nudLongitude.Value));
            int nextflag = mf.flagPts.Count + 1;
            CFlag flagPt = new CFlag(
                (double)nudLatitude.Value, (double)nudLongitude.Value,
                geoCoord.Easting, geoCoord.Northing,
                0, flagColor, nextflag, (nextflag).ToString());
            mf.flagPts.Add(flagPt);
            mf.FileSaveFlags();

            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (mf.flagPts.Count > 0)
            {
                mf.flagNumberPicked = nextflag;
                Form form = new FormFlags(mf);
                form.Show(mf);
            }

            Close();

        }
    }
}
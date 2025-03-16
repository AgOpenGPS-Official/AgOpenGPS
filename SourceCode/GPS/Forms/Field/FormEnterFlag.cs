using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Culture;
using AgOpenGPS.Helpers;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using AgOpenGPS.Core.Models;
using static System.Net.WebRequestMethods;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.Data;

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
        // This loads flags of a txt or CSV file with this format: "latitude,longitude,flagColor,flagName"
      
        private void btnLoadFlags_Click(object sender, EventArgs e)
        {
            double east, nort;

            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Text Document | *.txt| CSV Document | *.csv";
            fileDialog.Title = "Please select points file";
            fileDialog.Multiselect = false;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                try
                {
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    for (int i = 1; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        string[] parts = line.Split(',');
                        if (parts.Length == 4 &&
                            double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude) &&
                            int.TryParse(parts[2], out int flagColor))
                        {
                            string flagName = (!string.IsNullOrWhiteSpace(parts[3])) ? parts[3].Trim() : $"{mf.flagPts.Count + 1}";

                            mf.pn.ConvertWGS84ToLocal(latitude, longitude, out nort, out east);
                            int nextflag = mf.flagPts.Count + 1;
                            CFlag flagPt = new CFlag(latitude, longitude, east, nort, 0, flagColor, nextflag, flagName);
                            mf.flagPts.Add(flagPt);
                            mf.FileSaveFlags();
                        }
                        else
                        {
                            MessageBox.Show($"Invalid line in file: {line}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    MessageBox.Show("Flags successfully added!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                catch (Exception ex)
                {
                  MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
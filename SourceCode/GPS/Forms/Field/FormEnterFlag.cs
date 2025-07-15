using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Helpers;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
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

        // load flags from a text file with latitude,longitude,color, notes
        private void btnImportFlags_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog
            {
                Filter = "Text Document | *.txt| CSV Document | *.csv",
                Title = "Please select points file",
                Multiselect = false,
                RestoreDirectory = true
            };

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = fileDialog.FileName;
                try
                {
                    string[] lines = System.IO.File.ReadAllLines(filePath);

                    foreach (string line in lines.Skip(1))
                    {
                        string[] parts = line.Split(',');
                        if (
                            double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double latitude) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double longitude) &&
                            int.TryParse(parts[2], out int flagColor))
                        {
                            string flagName = (!string.IsNullOrWhiteSpace(parts[3])) ? parts[3].Trim() : $"{mf.flagPts.Count + 1}";
                            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(latitude, longitude));
                            int nextflag = mf.flagPts.Count + 1;
                            CFlag flagPt = new CFlag(
                                latitude, longitude,
                                geoCoord.Easting, geoCoord.Northing,
                                0, flagColor, nextflag, flagName);
                            mf.flagPts.Add(flagPt);
                            mf.FileSaveFlags();
                        }
                        else
                        {
                            FormDialog.Show("Error check format", $"Invalid line: {line}", MessageBoxButtons.OK);
                            return;
                        }
                    }
                    FormDialog.Show("Success", "Flags successfully added!", MessageBoxButtons.OK);
                }
                catch (Exception ex)
                {
                    FormDialog.Show("Error", $"Error reading file: {ex.Message}", MessageBoxButtons.OK);
                    Log.EventWriter("Loading Flags by lat lon" + ex.ToString());
                    return;
                }
            }
        }


        // Export flags to a CSV file, with latitude, longitude, color, notes
        private void btnExportFlags_Click(object sender, EventArgs e)
        {
            SaveFileDialog fileDialog = new SaveFileDialog();

            fileDialog.DefaultExt = "txt";
            fileDialog.Filter = "Text Document | *.txt| CSV Document | *.csv| All files| *.*";
            fileDialog.Title = "Export flags information";
            fileDialog.CheckFileExists = false;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                using (StreamWriter writer = new StreamWriter(fileDialog.FileName))
                {
                    try
                    {
                        writer.WriteLine("Latitude,Longitude,Color,Notes");

                        foreach (CFlag flag in mf.flagPts)
                        {
                            writer.WriteLine(
                                flag.latitude.ToString(CultureInfo.InvariantCulture) + "," +
                                flag.longitude.ToString(CultureInfo.InvariantCulture) + "," +
                                flag.color.ToString(CultureInfo.InvariantCulture) + "," +
                                flag.notes);
                        }
                        FormDialog.Show("Success", "Flags successfully saved!", MessageBoxButtons.OK);
                    }
                    catch (Exception ex)
                    {
                        FormDialog.Show("Error", ex.Message + "\nCannot write to file.", MessageBoxButtons.OK);
                        Log.EventWriter("Saving Flags by lat lon" + ex.ToString());
                        return;
                    }
                }
            }
        }
    }
}
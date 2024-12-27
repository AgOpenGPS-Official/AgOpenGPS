using AgOpenGPS.Culture;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJob : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormJob(Form callingForm)
        {
            //get ref of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            btnJobOpen.Text = gStr.gsOpen;
            btnJobNew.Text = gStr.gsNew;
            btnJobResume.Text = gStr.gsResume;
            btnInField.Text = gStr.gsDriveIn;
            btnFromKML.Text = gStr.gsFromKml;
            btnFromExisting.Text = gStr.gsFromExisting;
            btnJobClose.Text = gStr.gsClose;

            this.Text = gStr.gsStartNewField;
        }

        private void FormJob_Load(object sender, EventArgs e)
        {
            //check if directory and file exists, maybe was deleted etc
            if (String.IsNullOrEmpty(mf.currentFieldDirectory)) btnJobResume.Enabled = false;
            string directoryName = Path.Combine(mf.fieldsDirectory, mf.currentFieldDirectory);

            string fileAndDirectory = Path.Combine(directoryName, "Field.txt");

            if (!File.Exists(fileAndDirectory))
            {
                lblResumeField.Text = "";
                btnJobResume.Enabled = false;
                mf.currentFieldDirectory = "";

                Properties.Settings.Default.setF_CurrentDir = "";
                Properties.Settings.Default.Save();
            }
            else
            {
                lblResumeField.Text = gStr.gsResume + ": " + mf.currentFieldDirectory;
            

            if (mf.isJobStarted)
            {

                btnJobResume.Enabled = false;
                lblResumeField.Text = gStr.gsOpen + ": " + mf.currentFieldDirectory;
            }
            else
            {
                btnJobClose.Enabled = false;
            }
            }

            Location = Properties.Settings.Default.setJobMenu_location;
            Size = Properties.Settings.Default.setJobMenu_size;

            mf.CloseTopMosts();

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnJobResume_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();

            //open the Resume.txt and continue from last exit
            mf.FileOpenField("Resume");

            mf.LogEventWriter("Job Form, Field Resume");

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnJobOpen_Click(object sender, EventArgs e)
        {
            mf.filePickerFileAndDirectory = "";

            using (FormFilePicker form = new FormFilePicker(mf))
            {
                //returns full field.txt file dir name
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();
                    mf.FileOpenField(mf.filePickerFileAndDirectory);

                    Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void btnInField_Click(object sender, EventArgs e)
        {
            string infieldList = "";
            int numFields = 0;

            string[] dirs = Directory.GetDirectories(mf.fieldsDirectory);

            foreach (string dir in dirs)
            {
                double lat = 0;
                double lon = 0;

                string fieldDirectory = Path.GetFileName(dir);
                string filename = Path.Combine(dir, "Field.txt");
                string line;

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (StreamReader reader = new StreamReader(filename))
                    {
                        try
                        {
                            //Date time line
                            for (int i = 0; i < 8; i++)
                            {
                                line = reader.ReadLine();
                            }

                            //start positions
                            if (!reader.EndOfStream)
                            {
                                line = reader.ReadLine();
                                string[] offs = line.Split(',');

                                lat = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                lon = (double.Parse(offs[1], CultureInfo.InvariantCulture));

                                double dist = GetDistance(lon, lat, mf.pn.longitude, mf.pn.latitude);

                                if (dist < 500)
                                {
                                    numFields++;
                                    if (string.IsNullOrEmpty(infieldList))
                                        infieldList += Path.GetFileName(dir);
                                    else
                                        infieldList += "," + Path.GetFileName(dir);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            mf.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(infieldList))
            {
                mf.filePickerFileAndDirectory = "";

                if (numFields > 1)
                {
                    using (FormDrivePicker form = new FormDrivePicker(mf, infieldList))
                    {
                        //returns full field.txt file dir name
                        if (form.ShowDialog(this) == DialogResult.Yes)
                        {
                            if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();
                            mf.FileOpenField(mf.filePickerFileAndDirectory);
                            Close();
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else // 1 field found
                {
                    mf.filePickerFileAndDirectory = Path.Combine(mf.fieldsDirectory, infieldList, "Field.txt");
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    Close();
                }
            }
            else //no fields found
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsFieldNotOpen);
            }
        }

        public double GetDistance(double longitude, double latitude, double otherLongitude, double otherLatitude)
        {
            double d1 = latitude * (Math.PI / 180.0);
            double num1 = longitude * (Math.PI / 180.0);
            double d2 = otherLatitude * (Math.PI / 180.0);
            double num2 = otherLongitude * (Math.PI / 180.0) - num1;
            double d3 = Math.Pow(Math.Sin((d2 - d1) / 2.0), 2.0) + Math.Cos(d1) * Math.Cos(d2) * Math.Pow(Math.Sin(num2 / 2.0), 2.0);

            return 6376500.0 * (2.0 * Math.Atan2(Math.Sqrt(d3), Math.Sqrt(1.0 - d3)));
        }

        private void btnFromKML_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();
            //back to FormGPS
            DialogResult = DialogResult.No;
            Close();
        }

        private void btnFromExisting_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.Retry;
            Close();
        }

        private void btnJobClose_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();
            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void FormJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setJobMenu_location = Location;
            Properties.Settings.Default.setJobMenu_size = Size;
            Properties.Settings.Default.Save();
        }

        private void btnFromISOXML_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted) mf.FileSaveEverythingBeforeClosingField();
            //back to FormGPS
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.isCancelJobMenu = true;
        }
    }
}
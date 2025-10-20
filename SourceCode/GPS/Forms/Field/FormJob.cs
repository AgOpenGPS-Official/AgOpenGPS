using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Forms.Field;
using AgOpenGPS.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormJob : System.Windows.Forms.Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormJob(System.Windows.Forms.Form callingForm)
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
            btnJobAgShare.Enabled = Properties.Settings.Default.AgShareEnabled;
            btnAgShareBulkUpload.Enabled = Properties.Settings.Default.AgShareEnabled;

            this.Text = gStr.gsStartNewField;
        }

        private void FormJob_Load(object sender, EventArgs e)
        {
            //check if directory and file exists, maybe was deleted etc
            if (String.IsNullOrEmpty(mf.currentFieldDirectory)) btnJobResume.Enabled = false;
            string directoryName = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);

            string fileAndDirectory = Path.Combine(directoryName, "Field.txt");

            //Trigger a snapshot to create a temp data file for the AgShare Upload
            if (mf.isJobStarted && Properties.Settings.Default.AgShareEnabled) mf.AgShareSnapshot();


            if (!File.Exists(fileAndDirectory))
            {
                lblResumeField.Text = "";
                btnJobResume.Enabled = false;
                mf.currentFieldDirectory = "";

                Log.EventWriter("Field Directory is Empty or Missing");
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

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnJobNew_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }
            //back to FormGPS
            DialogResult = DialogResult.Yes;
            Close();
        }

        private void btnJobResume_Click(object sender, EventArgs e)
        {
            //open the Resume.txt and continue from last exit
            mf.FileOpenField("Resume");

            Log.EventWriter("Job Form, Field Resume");

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private async void btnJobOpen_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            mf.filePickerFileAndDirectory = "";

            using (FormFilePicker form = new FormFilePicker(mf))
            {
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    Close();
                }
            }
        }



        private async void btnInField_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            string infieldList = "";
            string distanceList = "";
            int numFields = 0;

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                string filename = Path.Combine(dir, "Field.txt");

                //make sure directory has a field.txt in it
                if (File.Exists(filename))
                {
                    using (GeoStreamReader reader = new GeoStreamReader(filename))
                    {
                        try
                        {
                            // Skip 8 lines
                            for (int i = 0; i < 8; i++)
                            {
                                reader.ReadLine();
                            }
                            //start positions
                            if (!reader.EndOfStream)
                            {
                                Wgs84 startLatLon = reader.ReadWgs84();
                                double distance = startLatLon.DistanceInKiloMeters(mf.AppModel.CurrentLatLon);

                                if (distance < 0.5)
                                {
                                    numFields++;
                                    if (!string.IsNullOrEmpty(infieldList))
                                    {
                                        infieldList += ",";
                                        distanceList += ",";
                                    }
                                    infieldList += Path.GetFileName(dir);

                                    // Convert to miles if not metric
                                    Distance distanceObj = new Distance(distance * 1000); // Distance expects meters
                                    double displayDistance = mf.isMetric ? distanceObj.InKilometers : distanceObj.InMiles;
                                    distanceList += displayDistance.ToString("F3");
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
                    using (FormDrivePicker form = new FormDrivePicker(mf, infieldList, distanceList))
                    {
                        //returns full field.txt file dir name
                        if (form.ShowDialog(this) == DialogResult.Yes)
                        {
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
                    mf.filePickerFileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, infieldList, "Field.txt");
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    Close();
                }
            }
            else //no fields found
            {
                mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsFieldNotOpen);
            }
        }

        private async void btnFromKML_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }
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
            if (mf.isJobStarted)
            {
                _ = Task.Run(() => mf.FileSaveEverythingBeforeClosingField());
            }
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

        private async void btnFromISOXML_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }
            //back to FormGPS
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.isCancelJobMenu = true;
        }

        private async void btnJobAgShare_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }
            using (var form = new FormAgShareDownloader(mf))
            {
                form.ShowDialog(this);
            }

            DialogResult = DialogResult.Ignore;
            Close();
        }

        private void btnAgShareBulkUpload_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                mf.TimedMessageBox(2000, gStr.gsError, gStr.gsCloseFieldFirst);
                return;
            }

            using (var form = new FormAgShareUploader())
            {
                form.ShowDialog(this);
            }

            DialogResult = DialogResult.Ignore;
            Close();
        }
    }
}
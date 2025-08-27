using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
using AgOpenGPS.Core.Translations;
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

        private void btnJobOpen_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
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



        private void btnInField_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = Task.Run(() => mf.FileSaveEverythingBeforeClosingField());
            }

            string infieldList = "";
            int numFields = 0;

            string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                FileInfo fileInfo = new FileInfo(Path.Combine(dir, "Field.txt"));

                //make sure directory has a field.txt in it
                if (fileInfo.Exists)
                {
                    using (GeoStreamReader reader = new GeoStreamReader(fileInfo))
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
                                    }
                                    infieldList += Path.GetFileName(dir);
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

        private void btnFromKML_Click(object sender, EventArgs e)
        {
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

        private void btnFromISOXML_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }
            //back to FormGPS
            DialogResult = DialogResult.Abort;
            Close();
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.isCancelJobMenu = true;
        }

        private void btnJobAgShare_Click(object sender, EventArgs e)
        {
            using (var form = new FormAgShareDownloader(mf))
            {
                form.ShowDialog(this);
            }

            DialogResult = DialogResult.Ignore;
            Close();
        }
    }
}
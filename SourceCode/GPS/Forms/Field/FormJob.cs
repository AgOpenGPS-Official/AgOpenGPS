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
    public partial class FormJob : Form
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

        // Unified save helper with dialog prompt
        private async Task<bool> SaveIfJobStartedAsync()
        {
            if (!mf.isJobStarted) return true;
            return await mf.PromptAndSaveBeforeClosingFieldAsync(this);
        }

        private async void btnJobNew_Click(object sender, EventArgs e)
        {
            btnJobNew.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled
                DialogResult = DialogResult.Yes; // back to FormGPS
                Close();
            }
            finally
            {
                UseWaitCursor = false;
                btnJobNew.Enabled = true;
            }
        }

        private async void btnJobResume_Click(object sender, EventArgs e)
        {
            btnJobResume.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled

                mf.FileOpenField("Resume");
                Log.EventWriter("Job Form, Field Resume");

                DialogResult = DialogResult.OK;
                Close();
            }
            finally
            {
                UseWaitCursor = false;
                btnJobResume.Enabled = true;
            }
        }

        private async void btnJobOpen_Click(object sender, EventArgs e)
        {
            try
            {
                btnJobOpen.Enabled = false;
                UseWaitCursor = true;

                if (!await SaveIfJobStartedAsync()) return; // user canceled

                mf.filePickerFileAndDirectory = "";

                using (var form = new FormFilePicker(mf))
                {
                    if (form.ShowDialog(this) == DialogResult.Yes)
                    {
                        mf.FileOpenField(mf.filePickerFileAndDirectory);
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Open job failed: " + ex);
                mf.TimedMessageBox(2000, "Open Job", "Saving/opening failed.");
            }
            finally
            {
                UseWaitCursor = false;
                btnJobOpen.Enabled = true;
            }
        }

        private async void btnInField_Click(object sender, EventArgs e)
        {
            btnInField.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled

                string infieldList = "";
                int numFields = 0;

                string[] dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);
                foreach (string dir in dirs)
                {
                    string filename = Path.Combine(dir, "Field.txt");
                    if (!File.Exists(filename)) continue;

                    using (GeoStreamReader reader = new GeoStreamReader(filename))
                    {
                        try
                        {
                            // Skip 8 lines
                            for (int i = 0; i < 8; i++) reader.ReadLine();

                            if (!reader.EndOfStream)
                            {
                                Wgs84 startLatLon = reader.ReadWgs84();
                                double distance = startLatLon.DistanceInKiloMeters(mf.AppModel.CurrentLatLon);
                                if (distance < 0.5)
                                {
                                    numFields++;
                                    if (!string.IsNullOrEmpty(infieldList)) infieldList += ",";
                                    infieldList += Path.GetFileName(dir);
                                }
                            }
                        }
                        catch
                        {
                            mf.TimedMessageBox(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);
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
                            if (form.ShowDialog(this) == DialogResult.Yes)
                            {
                                mf.FileOpenField(mf.filePickerFileAndDirectory);
                                Close();
                            }
                        }
                    }
                    else
                    {
                        mf.filePickerFileAndDirectory = Path.Combine(RegistrySettings.fieldsDirectory, infieldList, "Field.txt");
                        mf.FileOpenField(mf.filePickerFileAndDirectory);
                        Close();
                    }
                }
                else
                {
                    mf.TimedMessageBox(2000, gStr.gsNoFieldsFound, gStr.gsFieldNotOpen);
                }
            }
            finally
            {
                UseWaitCursor = false;
                btnInField.Enabled = true;
            }
        }

        private async void btnFromKML_Click(object sender, EventArgs e)
        {
            btnFromKML.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled
                DialogResult = DialogResult.No;   // back to FormGPS
                Close();
            }
            finally
            {
                UseWaitCursor = false;
                btnFromKML.Enabled = true;
            }
        }

        private void btnFromExisting_Click(object sender, EventArgs e)
        {
            //back to FormGPS
            DialogResult = DialogResult.Retry;
            Close();
        }

        private async void btnJobClose_Click(object sender, EventArgs e)
        {
            btnJobClose.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Close job failed: " + ex);
                mf.TimedMessageBox(2000, "Close Job", "Saving/cleanup failed.");
            }
            finally
            {
                UseWaitCursor = false;
                btnJobClose.Enabled = true;
            }
        }

        private void FormJob_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setJobMenu_location = Location;
            Properties.Settings.Default.setJobMenu_size = Size;
            Properties.Settings.Default.Save();
        }

        private async void btnFromISOXML_Click(object sender, EventArgs e)
        {
            btnFromISOXML.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled
                DialogResult = DialogResult.Abort; // back to FormGPS
                Close();
            }
            finally
            {
                UseWaitCursor = false;
                btnFromISOXML.Enabled = true;
            }
        }

        private void btnDeleteAB_Click(object sender, EventArgs e)
        {
            mf.isCancelJobMenu = true;
        }

        private async void btnJobAgShare_Click(object sender, EventArgs e)
        {
            btnJobAgShare.Enabled = false;
            UseWaitCursor = true;
            try
            {
                if (!await SaveIfJobStartedAsync()) return; // user canceled

                using (var form = new FormAgShareDownloader(mf))
                {
                    form.ShowDialog(this);
                }

                DialogResult = DialogResult.Ignore; // custom result for AgShare
                Close();
            }
            finally
            {
                UseWaitCursor = false;
                btnJobAgShare.Enabled = true;
            }
        }
    }
}

using System;
using System.IO;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Forms.Field;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Start Work Session form - entry point for starting or resuming jobs.
    /// This form replaces FormJob with a job-based workflow.
    /// </summary>
    public partial class FormStartWork : Form
    {
        private readonly FormGPS mf;
        private CJob lastJob;

        public FormStartWork(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormStartWork_Load(object sender, EventArgs e)
        {
            // Initialize WorkTypes with vehicles directory (used as settings location)
            WorkTypes.Initialize(RegistrySettings.vehiclesDirectory);

            // Configure AgShare buttons based on settings
            btnAgShareDownload.Enabled = Properties.Settings.Default.AgShareEnabled;
            btnAgShareUpload.Enabled = Properties.Settings.Default.AgShareEnabled;

            // Check for last active job
            LoadLastJobInfo();

            // Update UI based on current state
            UpdateButtonStates();

            // Center on screen
            if (!AgOpenGPS.Helpers.ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            mf.CloseTopMosts();
        }

        private void LoadLastJobInfo()
        {
            lastJob = null;
            lblLastJob.Text = "";

            // First check if there's an open field with jobs
            if (!string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
                if (Directory.Exists(fieldDir))
                {
                    lastJob = JobFiles.GetLastActiveJob(fieldDir);
                }
            }

            // If no job in current field, check all fields
            if (lastJob == null)
            {
                var allJobs = JobFiles.ListAllActiveJobs(RegistrySettings.fieldsDirectory);
                if (allJobs.Count > 0)
                {
                    lastJob = allJobs[0].Job;
                }
            }

            // Update label
            if (lastJob != null)
            {
                lblLastJob.Text = $"{lastJob.Name}\n{lastJob.FieldName} - {lastJob.WorkType}";
                btnResumeJob.Enabled = true;
            }
            else
            {
                lblLastJob.Text = "No active jobs";
                btnResumeJob.Enabled = false;
            }
        }

        private void UpdateButtonStates()
        {
            // Close button only enabled if a job is running
            btnCloseField.Enabled = mf.isJobStarted;

            // If job is running, show current field info
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                lblCurrentField.Text = $"Current: {mf.displayFieldName}";
                lblCurrentField.Visible = true;
            }
            else
            {
                lblCurrentField.Visible = false;
            }
        }

        private void btnNewJob_Click(object sender, EventArgs e)
        {
            // Save current field if open
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            // Open the job wizard
            using (var wizard = new FormJobWizard(mf))
            {
                if (wizard.ShowDialog(this) == DialogResult.OK)
                {
                    // Job was created and field opened
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void btnResumeJob_Click(object sender, EventArgs e)
        {
            if (lastJob == null)
            {
                // Show job picker
                using (var form = new FormResumeJob(mf))
                {
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        DialogResult = DialogResult.OK;
                        Close();
                    }
                }
            }
            else
            {
                // Resume the last job directly
                ResumeJob(lastJob);
            }
        }

        private void ResumeJob(CJob job)
        {
            if (job == null) return;

            // Save current field if open
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            // Find the field directory for this job
            string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, job.FieldName);
            string fieldFile = Path.Combine(fieldDir, "Field.txt");

            if (!File.Exists(fieldFile))
            {
                mf.TimedMessageBox(2000, gStr.gsError, "Field not found");
                return;
            }

            // Open the field
            mf.FileOpenField(fieldFile);

            // Update job timestamp
            job.Touch();
            JobFiles.Save(job, fieldDir);

            // Set current job in FormGPS (will be implemented later)
            // mf.currentJob = job;

            Log.EventWriter($"Job resumed: {job.Name}");

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnOpenFieldOnly_Click(object sender, EventArgs e)
        {
            // Save current field if open
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            // Show field picker
            mf.filePickerFileAndDirectory = "";
            using (var form = new FormFilePicker(mf))
            {
                if (form.ShowDialog(this) == DialogResult.Yes)
                {
                    mf.FileOpenField(mf.filePickerFileAndDirectory);
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private void btnCloseField_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }
            DialogResult = DialogResult.OK;
            Close();
        }

        private async void btnAgShareDownload_Click(object sender, EventArgs e)
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

        private void btnAgShareUpload_Click(object sender, EventArgs e)
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.isCancelJobMenu = true;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void FormStartWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setJobMenu_location = Location;
            Properties.Settings.Default.setJobMenu_size = Size;
            Properties.Settings.Default.Save();
        }
    }
}

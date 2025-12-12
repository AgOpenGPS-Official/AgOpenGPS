using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Start Work Session form - all-in-one job management.
    /// Contains main menu, wizard steps, and job list as switchable panels.
    /// </summary>
    public partial class FormStartWork : Form
    {
        private readonly FormGPS mf;

        // Current view mode
        private enum ViewMode { Main, WizardStep1, WizardStep2, WizardStep3, ResumeJobList }
        private ViewMode currentView = ViewMode.Main;

        // Job data
        private CJob lastJob;
        private List<(CJob Job, string FieldDirectory)> allActiveJobs;

        // Wizard selections
        private string selectedProfile;
        private string selectedFieldDir;
        private string selectedFieldName;
        private string selectedWorkType;

        public FormStartWork(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormStartWork_Load(object sender, EventArgs e)
        {
            // Initialize WorkTypes
            WorkTypes.Initialize(RegistrySettings.vehiclesDirectory);

            // Configure AgShare buttons
            btnAgShareDownload.Enabled = Properties.Settings.Default.AgShareEnabled;
            btnAgShareUpload.Enabled = Properties.Settings.Default.AgShareEnabled;

            // Load job data
            LoadJobData();

            // Show main view
            ShowView(ViewMode.Main);

            // Center on screen
            if (!AgOpenGPS.Helpers.ScreenHelper.IsOnScreen(Bounds))
            {
                CenterToScreen();
            }

            mf.CloseTopMosts();
        }

        #region View Management

        private void ShowView(ViewMode view)
        {
            currentView = view;

            // Hide all panels
            panelMain.Visible = false;
            panelWizardStep1.Visible = false;
            panelWizardStep2.Visible = false;
            panelWizardStep3.Visible = false;
            panelResumeList.Visible = false;

            // Show the requested panel
            switch (view)
            {
                case ViewMode.Main:
                    panelMain.Visible = true;
                    UpdateMainView();
                    break;
                case ViewMode.WizardStep1:
                    panelWizardStep1.Visible = true;
                    LoadWizardStep1();
                    break;
                case ViewMode.WizardStep2:
                    panelWizardStep2.Visible = true;
                    LoadWizardStep2();
                    break;
                case ViewMode.WizardStep3:
                    panelWizardStep3.Visible = true;
                    LoadWizardStep3();
                    break;
                case ViewMode.ResumeJobList:
                    panelResumeList.Visible = true;
                    LoadResumeJobList();
                    break;
            }
        }

        #endregion

        #region Main View

        private void LoadJobData()
        {
            lastJob = null;
            allActiveJobs = JobFiles.ListAllActiveJobs(RegistrySettings.fieldsDirectory);

            if (allActiveJobs.Count > 0)
            {
                lastJob = allActiveJobs[0].Job;
            }
        }

        private void UpdateMainView()
        {
            // Update resume button and info
            if (lastJob != null)
            {
                lblLastJobInfo.Text = $"{lastJob.Name}\n{lastJob.FieldName} - {lastJob.WorkType}";
                btnResumeJob.Enabled = true;
            }
            else
            {
                lblLastJobInfo.Text = "No active jobs";
                btnResumeJob.Enabled = false;
            }

            // Update current field status
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                lblCurrentField.Text = $"Current: {mf.displayFieldName}";
                lblCurrentField.Visible = true;
                btnCloseField.Enabled = true;
            }
            else
            {
                lblCurrentField.Visible = false;
                btnCloseField.Enabled = false;
            }

            // Update Close Job and Finish Job buttons based on currentJob state
            bool hasActiveJob = mf.currentJob != null;
            btnCloseJob.Enabled = hasActiveJob;
            btnFinishJob.Enabled = hasActiveJob;

            // Disable Resume/New Job if a job is already open (can't have 2 jobs at once)
            if (hasActiveJob)
            {
                btnResumeJob.Enabled = false;
                btnNewJob.Enabled = false;
            }
            else
            {
                btnNewJob.Enabled = true;
                // btnResumeJob already set above based on lastJob
            }
        }

        private void btnNewJob_Click(object sender, EventArgs e)
        {
            // Reset wizard selections
            selectedProfile = null;
            selectedFieldDir = null;
            selectedFieldName = null;
            selectedWorkType = null;

            // If a field is already open, skip to step 3 (work type selection)
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                selectedProfile = RegistrySettings.vehicleFileName;
                selectedFieldName = mf.displayFieldName;
                selectedFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
                ShowView(ViewMode.WizardStep3);
            }
            else
            {
                // Start wizard at step 1
                ShowView(ViewMode.WizardStep1);
            }
        }

        private void btnResumeJob_Click(object sender, EventArgs e)
        {
            if (allActiveJobs.Count > 1)
            {
                // Show job list to pick from
                ShowView(ViewMode.ResumeJobList);
            }
            else if (lastJob != null)
            {
                // Resume directly
                ResumeJob(lastJob, allActiveJobs[0].FieldDirectory);
            }
        }

        private void btnOpenFieldOnly_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

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

        private void btnCloseJob_Click(object sender, EventArgs e)
        {
            if (mf.currentJob == null)
            {
                mf.TimedMessageBox(1500, "No Job", "No active job to close");
                return;
            }

            // Save current job data
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            // Clear the current job reference
            var fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
            JobFiles.Save(mf.currentJob, fieldDir);

            Log.EventWriter($"Job closed: {mf.currentJob.Name}");
            mf.currentJob = null;

            // Reload job data and update view
            LoadJobData();
            UpdateMainView();
        }

        private void btnFinishJob_Click(object sender, EventArgs e)
        {
            if (mf.currentJob == null)
            {
                mf.TimedMessageBox(1500, "No Job", "No active job to finish");
                return;
            }

            // Mark job as completed
            mf.currentJob.IsCompleted = true;

            // Save current job data
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            var fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
            JobFiles.Save(mf.currentJob, fieldDir);

            Log.EventWriter($"Job finished: {mf.currentJob.Name}");
            mf.currentJob = null;

            // Reload job data and update view
            LoadJobData();
            UpdateMainView();

            mf.TimedMessageBox(1500, "Job Completed", "Job has been marked as finished");
        }

        #endregion

        #region Wizard Step 1 - Profile Selection

        private void LoadWizardStep1()
        {
            listProfiles.Items.Clear();

            // Add current profile option
            string currentProfile = RegistrySettings.vehicleFileName;
            if (!string.IsNullOrEmpty(currentProfile))
            {
                listProfiles.Items.Add($"Current: {currentProfile}");
            }

            // Load available profiles
            if (Directory.Exists(RegistrySettings.vehiclesDirectory))
            {
                foreach (string file in Directory.GetFiles(RegistrySettings.vehiclesDirectory, "*.XML"))
                {
                    string name = Path.GetFileNameWithoutExtension(file);
                    if (name != currentProfile)
                    {
                        listProfiles.Items.Add(name);
                    }
                }
            }

            if (listProfiles.Items.Count > 0)
                listProfiles.SelectedIndex = 0;
        }

        private void btnWizard1Next_Click(object sender, EventArgs e)
        {
            if (listProfiles.SelectedItem != null)
            {
                string selected = listProfiles.SelectedItem.ToString();
                selectedProfile = selected.StartsWith("Current:")
                    ? RegistrySettings.vehicleFileName
                    : selected;
            }
            else
            {
                selectedProfile = RegistrySettings.vehicleFileName;
            }

            ShowView(ViewMode.WizardStep2);
        }

        private void btnWizard1Back_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.Main);
        }

        #endregion

        #region Wizard Step 2 - Field Selection

        private void LoadWizardStep2()
        {
            listFields.Items.Clear();

            // Add current field if open
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                listFields.Items.Add($"Current: {mf.displayFieldName}");
            }

            // Load available fields
            if (Directory.Exists(RegistrySettings.fieldsDirectory))
            {
                foreach (string dir in Directory.GetDirectories(RegistrySettings.fieldsDirectory))
                {
                    string fieldFile = Path.Combine(dir, "Field.txt");
                    if (File.Exists(fieldFile))
                    {
                        string name = Path.GetFileName(dir);
                        if (name != mf.currentFieldDirectory)
                        {
                            listFields.Items.Add(name);
                        }
                    }
                }
            }

            if (listFields.Items.Count > 0)
                listFields.SelectedIndex = 0;
        }

        private void btnWizard2Next_Click(object sender, EventArgs e)
        {
            if (listFields.SelectedItem != null)
            {
                string selected = listFields.SelectedItem.ToString();
                if (selected.StartsWith("Current:"))
                {
                    selectedFieldName = mf.displayFieldName;
                    selectedFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
                }
                else
                {
                    selectedFieldName = selected;
                    selectedFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, selected);
                }
            }

            if (string.IsNullOrEmpty(selectedFieldDir))
            {
                mf.TimedMessageBox(1500, "Select Field", "Please select a field");
                return;
            }

            ShowView(ViewMode.WizardStep3);
        }

        private void btnWizard2Back_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.WizardStep1);
        }

        #endregion

        #region Wizard Step 3 - Work Type & Job Name

        private void LoadWizardStep3()
        {
            flpWorkTypes.Controls.Clear();

            // Create work type buttons
            foreach (var workType in WorkTypes.All)
            {
                var btn = new Button
                {
                    Text = workType.Name,
                    Tag = workType.Id,
                    Width = 150,
                    Height = 60,
                    Font = new Font("Tahoma", 12F, FontStyle.Bold),
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.White,
                    Margin = new Padding(5)
                };
                btn.FlatAppearance.BorderSize = 1;
                btn.Click += WorkTypeButton_Click;
                flpWorkTypes.Controls.Add(btn);
            }

            // Generate default job name
            txtJobName.Text = $"{DateTime.Now:yyyy-MM-dd}_Work_{selectedProfile}";
        }

        private void WorkTypeButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            selectedWorkType = btn.Tag.ToString();

            // Highlight selected
            foreach (Control c in flpWorkTypes.Controls)
            {
                if (c is Button b)
                {
                    b.BackColor = (b == btn) ? Color.FromArgb(0, 119, 190) : Color.White;
                    b.ForeColor = (b == btn) ? Color.White : Color.Black;
                }
            }

            // Update job name
            txtJobName.Text = $"{DateTime.Now:yyyy-MM-dd}_{selectedWorkType}_{selectedProfile}";
        }

        private void btnWizard3Start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtJobName.Text))
            {
                mf.TimedMessageBox(1500, "Job Name", "Please enter a job name");
                return;
            }

            if (string.IsNullOrEmpty(selectedWorkType))
            {
                selectedWorkType = "Other";
            }

            // Create the job
            CreateAndStartJob();
        }

        private void btnWizard3Back_Click(object sender, EventArgs e)
        {
            // If field was already open, we skipped steps 1 and 2, so go back to main
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                ShowView(ViewMode.Main);
            }
            else
            {
                ShowView(ViewMode.WizardStep2);
            }
        }

        private void CreateAndStartJob()
        {
            // Check if we're creating a job for the already open field
            bool fieldAlreadyOpen = mf.isJobStarted &&
                Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory) == selectedFieldDir;

            // Save current field data if open (but don't close if it's the same field)
            if (mf.isJobStarted && !fieldAlreadyOpen)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            // Create new job
            var job = new CJob(
                selectedFieldName,
                selectedProfile,
                selectedWorkType,
                mf.tool.width,
                txtJobName.Text
            );

            // Initialize job files
            JobFiles.InitializeJobFiles(job, selectedFieldDir);

            // Set the current job
            mf.currentJob = job;

            // Only open the field if it's not already open
            if (!fieldAlreadyOpen)
            {
                string fieldFile = Path.Combine(selectedFieldDir, "Field.txt");
                mf.FileOpenField(fieldFile);
            }

            Log.EventWriter($"New job created: {job.Name}");

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        #region Resume Job List

        private void LoadResumeJobList()
        {
            flpJobList.Controls.Clear();

            if (allActiveJobs.Count == 0)
            {
                var lbl = new Label
                {
                    Text = "No active jobs found",
                    Font = new Font("Tahoma", 14F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(20)
                };
                flpJobList.Controls.Add(lbl);
                return;
            }

            foreach (var item in allActiveJobs)
            {
                var panel = CreateJobPanel(item.Job, item.FieldDirectory);
                flpJobList.Controls.Add(panel);
            }
        }

        private Panel CreateJobPanel(CJob job, string fieldDir)
        {
            var panel = new Panel
            {
                Width = flpJobList.Width - 30,
                Height = 70,
                BackColor = Color.White,
                Margin = new Padding(5),
                Cursor = Cursors.Hand,
                Tag = new JobSelection { Job = job, FieldDirectory = fieldDir }
            };

            // Field name on the left (prominent)
            var lblField = new Label
            {
                Text = job.FieldName,
                Font = new Font("Tahoma", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 119, 190),
                Location = new Point(15, 10),
                AutoSize = true
            };

            // Job name next to field name
            var lblJobName = new Label
            {
                Text = $"  -  {job.Name}",
                Font = new Font("Tahoma", 14F),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(lblField.PreferredWidth + 15, 12),
                AutoSize = true
            };

            // Work type and last opened on second line
            var lblDetails = new Label
            {
                Text = $"{job.WorkType}  |  Last opened: {job.LastOpenedAt:g}",
                Font = new Font("Tahoma", 10F),
                ForeColor = Color.DimGray,
                Location = new Point(15, 42),
                AutoSize = true
            };

            panel.Controls.Add(lblField);
            panel.Controls.Add(lblJobName);
            panel.Controls.Add(lblDetails);

            panel.Click += JobPanel_Click;
            lblField.Click += (s, e) => JobPanel_Click(panel, e);
            lblJobName.Click += (s, e) => JobPanel_Click(panel, e);
            lblDetails.Click += (s, e) => JobPanel_Click(panel, e);

            panel.MouseEnter += (s, e) => panel.BackColor = Color.FromArgb(230, 240, 250);
            panel.MouseLeave += (s, e) => panel.BackColor = Color.White;

            return panel;
        }

        private void JobPanel_Click(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            if (panel?.Tag is JobSelection selection)
            {
                ResumeJob(selection.Job, selection.FieldDirectory);
            }
        }

        private void btnResumeListBack_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.Main);
        }

        private void ResumeJob(CJob job, string fieldDir)
        {
            string fieldFile = Path.Combine(fieldDir, "Field.txt");
            if (!File.Exists(fieldFile))
            {
                mf.TimedMessageBox(2000, gStr.gsError, "Field not found");
                return;
            }

            // Check if we're resuming a job in the currently open field
            bool sameField = mf.isJobStarted &&
                Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory) == fieldDir;

            // Close current field if open (save first)
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();

                // If it's a different field, we need to properly close and reopen
                if (!sameField)
                {
                    mf.AppModel.Fields.CloseField();
                }
            }

            // Open the field (or refresh if same field)
            mf.FileOpenField(fieldFile);

            // Set the current job
            mf.currentJob = job;

            job.Touch();
            JobFiles.Save(job, fieldDir);

            Log.EventWriter($"Job resumed: {job.Name}");

            DialogResult = DialogResult.OK;
            Close();
        }

        #endregion

        private void FormStartWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setJobMenu_location = Location;
            Properties.Settings.Default.setJobMenu_size = Size;
            Properties.Settings.Default.Save();
        }

        private class JobSelection
        {
            public CJob Job { get; set; }
            public string FieldDirectory { get; set; }
        }
    }
}

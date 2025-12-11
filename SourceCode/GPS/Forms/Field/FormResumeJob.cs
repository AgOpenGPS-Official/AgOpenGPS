using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Form for selecting and resuming an existing job
    /// </summary>
    public partial class FormResumeJob : Form
    {
        private readonly FormGPS mf;
        private List<(CJob Job, string FieldDirectory)> allJobs;

        public CJob SelectedJob { get; private set; }
        public string SelectedFieldDirectory { get; private set; }

        public FormResumeJob(FormGPS callingForm)
        {
            mf = callingForm;
            InitializeComponent();
        }

        private void FormResumeJob_Load(object sender, EventArgs e)
        {
            LoadAllJobs();
        }

        private void LoadAllJobs()
        {
            flpJobList.Controls.Clear();
            allJobs = JobFiles.ListAllActiveJobs(RegistrySettings.fieldsDirectory);

            if (allJobs.Count == 0)
            {
                lblStatus.Text = "No active jobs found";
                return;
            }

            lblStatus.Text = $"{allJobs.Count} active job(s)";

            foreach (var item in allJobs)
            {
                var jobPanel = CreateJobPanel(item.Job, item.FieldDirectory);
                flpJobList.Controls.Add(jobPanel);
            }
        }

        private Panel CreateJobPanel(CJob job, string fieldDirectory)
        {
            var panel = new Panel
            {
                Width = flpJobList.Width - 30,
                Height = 80,
                BackColor = Color.White,
                Margin = new Padding(5),
                Cursor = Cursors.Hand,
                Tag = new JobSelection { Job = job, FieldDirectory = fieldDirectory }
            };

            var lblName = new Label
            {
                Text = job.Name,
                Font = new Font("Tahoma", 14F, FontStyle.Bold),
                Location = new Point(10, 10),
                AutoSize = true
            };

            var lblDetails = new Label
            {
                Text = $"{job.FieldName} - {job.WorkType} | {job.LastOpenedAt:g}",
                Font = new Font("Tahoma", 10F),
                ForeColor = Color.DimGray,
                Location = new Point(10, 40),
                AutoSize = true
            };

            panel.Controls.Add(lblName);
            panel.Controls.Add(lblDetails);

            panel.Click += JobPanel_Click;
            lblName.Click += (s, e) => JobPanel_Click(panel, e);
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
                SelectedJob = selection.Job;
                SelectedFieldDirectory = selection.FieldDirectory;
                ResumeSelectedJob();
            }
        }

        private void ResumeSelectedJob()
        {
            if (SelectedJob == null) return;

            // Save current field if open
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            // Open the field
            string fieldFile = Path.Combine(SelectedFieldDirectory, "Field.txt");
            if (File.Exists(fieldFile))
            {
                mf.FileOpenField(fieldFile);

                // Update job timestamp
                SelectedJob.Touch();
                JobFiles.Save(SelectedJob, SelectedFieldDirectory);

                Log.EventWriter($"Job resumed: {SelectedJob.Name}");

                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsError, "Field not found");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private class JobSelection
        {
            public CJob Job { get; set; }
            public string FieldDirectory { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Forms.Field;
using AgOpenGPS.IO;

namespace AgOpenGPS
{
    /// <summary>
    /// Start Work Session form - all-in-one task management UI.
    /// Contains main menu, wizard steps, and task list as switchable panels.
    /// Business logic is handled by TaskManager.
    /// </summary>
    public partial class FormStartWork : Form
    {
        private readonly FormGPS mf;

        // Current view mode
        private enum ViewMode { Main, OpenField, WizardStep1, WizardStep2, WizardStep3, ResumeTaskList }
        private ViewMode currentView = ViewMode.Main;

        // Task data
        private CTask lastTask;
        private List<(CTask Task, string FieldDirectory)> allActiveTasks;

        // Wizard selections
        private string selectedProfile;
        private string selectedFieldDir;
        private string selectedFieldName;
        private string selectedWorkType;

        // Coverage import
        private bool shouldImportCoverage = false;

        // New field creation
        private bool isCreatingNewField = false;

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
            btnOpenFromAgShare.Enabled = Properties.Settings.Default.AgShareEnabled;
            btnUploadToAgShare.Enabled = Properties.Settings.Default.AgShareEnabled;

            // Load task data
            LoadTaskData();

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
            panelOpenField.Visible = false;
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
                case ViewMode.OpenField:
                    panelOpenField.Visible = true;
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
                case ViewMode.ResumeTaskList:
                    panelResumeList.Visible = true;
                    LoadResumeTaskList();
                    break;
            }
        }

        #endregion

        #region Main View

        private void LoadTaskData()
        {
            allActiveTasks = mf.taskManager.GetAllActiveTasks();
            lastTask = allActiveTasks.Count > 0 ? allActiveTasks[0].Task : null;
        }

        private void UpdateMainView()
        {
            // Update last task info label (below tableTasks)
            if (lastTask != null)
            {
                lblLastTaskInfo.Text = $"Last Task: {lastTask.Name} ({lastTask.FieldName} - {lastTask.WorkType})";
                btnResumeTask.Enabled = true;
                btnResumeLastTask.Enabled = true;
            }
            else
            {
                lblLastTaskInfo.Text = "Last Task: --";
                btnResumeTask.Enabled = false;
                btnResumeLastTask.Enabled = false;
            }

            // Update Close Task and Finish Task buttons based on currentTask state
            bool hasActiveTask = mf.currentTask != null;
            btnCloseTask.Enabled = hasActiveTask;
            btnFinishTask.Enabled = hasActiveTask;

            // Update last field info label (below tableFields)
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                lblCurrentField.Text = $"Current Field: {mf.displayFieldName}";
                // Only enable Close Field if no task is open (must close task first)
                btnCloseField.Enabled = !hasActiveTask;
            }
            else if (lastTask != null)
            {
                lblCurrentField.Text = $"Last Field: {lastTask.FieldName}";
                btnCloseField.Enabled = false;
            }
            else
            {
                lblCurrentField.Text = "Last Field: --";
                btnCloseField.Enabled = false;
            }

            // Disable Resume/New Task if a task is already open
            if (hasActiveTask)
            {
                btnResumeTask.Enabled = false;
                btnResumeLastTask.Enabled = false;
                btnNewTask.Enabled = false;
            }
            else
            {
                btnNewTask.Enabled = true;
            }

            // Hide Field section when a task is active
            lblFieldSection.Visible = !hasActiveTask;
            tableFields.Visible = !hasActiveTask;
            lblCurrentField.Visible = !hasActiveTask;
        }

        private void btnNewTask_Click(object sender, EventArgs e)
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
                ShowView(ViewMode.WizardStep1);
            }
        }

        private void btnResumeLastTask_Click(object sender, EventArgs e)
        {
            if (lastTask != null && allActiveTasks.Count > 0)
            {
                DoResumeTask(lastTask, allActiveTasks[0].FieldDirectory);
            }
            else
            {
                mf.TimedMessageBox(1500, "No Tasks", "No active tasks found");
            }
        }

        private void btnResumeTask_Click(object sender, EventArgs e)
        {
            if (allActiveTasks.Count > 0)
            {
                ShowView(ViewMode.ResumeTaskList);
            }
            else
            {
                mf.TimedMessageBox(1500, "No Tasks", "No active tasks found");
            }
        }

        private async void btnCloseField_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            mf.AppModel.Fields.CloseField();

            // Refresh task data and update view
            LoadTaskData();
            UpdateMainView();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.isCancelJobMenu = true;
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnOpenField_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.OpenField);
        }

        #endregion

        #region Open Field Panel

        private void btnOpenFieldLocal_Click(object sender, EventArgs e)
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

        private async void btnOpenFromAgShare_Click(object sender, EventArgs e)
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

        private void btnUploadToAgShare_Click(object sender, EventArgs e)
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

        private void btnOpenFieldBack_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.Main);
        }

        private void btnNewField_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            using (var form = new FormFieldDir(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private async void btnCloseTask_Click(object sender, EventArgs e)
        {
            if (mf.currentTask == null)
            {
                mf.TimedMessageBox(1500, "No Task", "No active task to close");
                return;
            }

            // Save field data first
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            // Close the task
            mf.taskManager.CloseCurrentTask();

            // Close the field
            mf.JobClose();

            // Refresh task data and update view
            LoadTaskData();
            UpdateMainView();
        }

        private async void btnFinishTask_Click(object sender, EventArgs e)
        {
            if (mf.currentTask == null)
            {
                mf.TimedMessageBox(1500, "No Task", "No active task to finish");
                return;
            }

            // Save field data first
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            // Finish the task
            mf.taskManager.FinishCurrentTask();

            // Close the field
            mf.JobClose();

            // Refresh task data and update view
            LoadTaskData();
            UpdateMainView();
        }

        #endregion

        #region Wizard Step 1 - Profile Selection

        private void LoadWizardStep1()
        {
            listProfiles.Items.Clear();

            string currentProfile = RegistrySettings.vehicleFileName;
            if (!string.IsNullOrEmpty(currentProfile))
            {
                listProfiles.Items.Add($"Current: {currentProfile}");
            }

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

            // Load the profile if different
            if (selectedProfile != RegistrySettings.vehicleFileName)
            {
                if (!mf.taskManager.LoadProfile(selectedProfile))
                {
                    mf.TimedMessageBox(2000, gStr.gsError, $"Error loading profile {selectedProfile}");
                    return;
                }
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

            string currentFieldDir = null;
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                currentFieldDir = mf.currentFieldDirectory;
                listFields.Items.Add($"Current: {mf.displayFieldName}");
            }

            // Get last field from last task if available
            string lastFieldName = lastTask?.FieldName;
            int lastFieldIndex = -1;

            if (Directory.Exists(RegistrySettings.fieldsDirectory))
            {
                foreach (string dir in Directory.GetDirectories(RegistrySettings.fieldsDirectory))
                {
                    string fieldFile = Path.Combine(dir, "Field.txt");
                    if (File.Exists(fieldFile))
                    {
                        string name = Path.GetFileName(dir);
                        if (currentFieldDir == null || name != currentFieldDir)
                        {
                            int index = listFields.Items.Add(name);
                            // Track if this is the last opened field
                            if (name == lastFieldName)
                            {
                                lastFieldIndex = index;
                            }
                        }
                    }
                }
            }

            // Select last opened field, or current field, or first item
            if (lastFieldIndex >= 0)
            {
                listFields.SelectedIndex = lastFieldIndex;
            }
            else if (listFields.Items.Count > 0)
            {
                listFields.SelectedIndex = 0;
            }
        }

        private void btnWizard2Next_Click(object sender, EventArgs e)
        {
            if (listFields.SelectedItem != null)
            {
                string selected = listFields.SelectedItem.ToString();

                isCreatingNewField = false;

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

        private void btnWizard2NewField_Click(object sender, EventArgs e)
        {
            isCreatingNewField = true;
            selectedFieldName = null;
            selectedFieldDir = null;
            ShowView(ViewMode.WizardStep3);
        }

        private void btnWizard2Back_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.WizardStep1);
        }

        #endregion

        #region Wizard Step 3 - Work Type & Task Name

        private void LoadWizardStep3()
        {
            flpWorkTypes.Controls.Clear();

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

            // Show/hide field name input based on mode
            if (isCreatingNewField)
            {
                lblFieldNameLabel.Visible = true;
                txtFieldName.Visible = true;
                txtFieldName.Text = "";

                // Move work types panel down and make smaller to fit field name input
                flpWorkTypes.Location = new Point(30, 135);
                flpWorkTypes.Size = new Size(906, 220);
                lblTaskNameLabel.Location = new Point(30, 365);
                txtTaskName.Location = new Point(30, 395);

                lblStep3Title.Text = "Step 3: Create New Field + Task";

                // Focus after UI is updated
                BeginInvoke(new Action(() => txtFieldName.Focus()));
            }
            else
            {
                lblFieldNameLabel.Visible = false;
                txtFieldName.Visible = false;

                // Reset work types panel position and size
                flpWorkTypes.Location = new Point(30, 75);
                flpWorkTypes.Size = new Size(906, 280);
                lblTaskNameLabel.Location = new Point(30, 375);
                txtTaskName.Location = new Point(30, 405);

                lblStep3Title.Text = "Step 3: Select Work Type";
            }

            txtTaskName.Text = $"{DateTime.Now:yyyy-MM-dd}_Work_{selectedProfile}";

            // Check for existing coverage (only for existing fields)
            shouldImportCoverage = false;
            if (!isCreatingNewField && mf.taskManager.HasExistingCoverage(selectedFieldDir))
            {
                ShowImportDialog();
            }
        }

        private void WorkTypeButton_Click(object sender, EventArgs e)
        {
            var btn = sender as Button;
            selectedWorkType = btn.Tag.ToString();

            foreach (Control c in flpWorkTypes.Controls)
            {
                if (c is Button b)
                {
                    b.BackColor = (b == btn) ? Color.FromArgb(0, 119, 190) : Color.White;
                    b.ForeColor = (b == btn) ? Color.White : Color.Black;
                }
            }

            txtTaskName.Text = $"{DateTime.Now:yyyy-MM-dd}_{selectedWorkType}_{selectedProfile}";
        }

        private void btnWizard3Start_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTaskName.Text))
            {
                mf.TimedMessageBox(1500, "Task Name", "Please enter a task name");
                return;
            }

            if (string.IsNullOrEmpty(selectedWorkType))
            {
                selectedWorkType = "Other";
            }

            CTask task;

            if (isCreatingNewField)
            {
                // Validate field name
                if (string.IsNullOrWhiteSpace(txtFieldName.Text))
                {
                    mf.TimedMessageBox(1500, "Field Name", "Please enter a field name");
                    txtFieldName.Focus();
                    return;
                }

                // Check for valid GPS before creating field
                if (mf.pn.fixQuality == 0)
                {
                    mf.TimedMessageBox(2000, "No GPS", "Valid GPS signal required to create a new field");
                    return;
                }

                // Create field and task together
                task = mf.taskManager.CreateFieldAndTask(
                    txtFieldName.Text.Trim(),
                    selectedProfile,
                    selectedWorkType,
                    txtTaskName.Text
                );

                if (task == null)
                {
                    mf.TimedMessageBox(2000, gStr.gsError, "Failed to create field. The field name may already exist.");
                    return;
                }
            }
            else
            {
                // Create task in existing field
                task = mf.taskManager.CreateTask(
                    selectedFieldDir,
                    selectedFieldName,
                    selectedProfile,
                    selectedWorkType,
                    txtTaskName.Text,
                    shouldImportCoverage
                );
            }

            if (task != null)
            {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void btnWizard3Back_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                ShowView(ViewMode.Main);
            }
            else
            {
                ShowView(ViewMode.WizardStep2);
            }
        }

        private void ShowImportDialog()
        {
            string message = "Existing coverage data was found in this field.\n\n" +
                           "Would you like to import it to the new task?";

            DialogResult result = FormDialog.Show(
                "Import Coverage Data",
                message,
                MessageBoxButtons.OKCancel
            );

            shouldImportCoverage = (result == DialogResult.OK);
        }

        private void txtFieldName_Enter(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
            }
        }

        private void txtTaskName_Enter(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                ((TextBox)sender).ShowKeyboard(this);
            }
        }

        #endregion

        #region Resume Task List

        private void LoadResumeTaskList()
        {
            flpTaskList.Controls.Clear();

            if (allActiveTasks.Count == 0)
            {
                var lbl = new Label
                {
                    Text = "No active tasks found",
                    Font = new Font("Tahoma", 14F),
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Margin = new Padding(20)
                };
                flpTaskList.Controls.Add(lbl);
                return;
            }

            foreach (var item in allActiveTasks)
            {
                var panel = CreateTaskPanel(item.Task, item.FieldDirectory);
                flpTaskList.Controls.Add(panel);
            }
        }

        private Panel CreateTaskPanel(CTask task, string fieldDir)
        {
            var panel = new Panel
            {
                Width = flpTaskList.Width - 30,
                Height = 70,
                BackColor = Color.White,
                Margin = new Padding(5),
                Cursor = Cursors.Hand,
                Tag = new TaskSelection { Task = task, FieldDirectory = fieldDir }
            };

            var lblField = new Label
            {
                Text = task.FieldName,
                Font = new Font("Tahoma", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 119, 190),
                Location = new Point(15, 10),
                AutoSize = true
            };

            var lblTaskName = new Label
            {
                Text = $"  -  {task.Name}",
                Font = new Font("Tahoma", 14F),
                ForeColor = Color.FromArgb(60, 60, 60),
                Location = new Point(lblField.PreferredWidth + 15, 12),
                AutoSize = true
            };

            var lblDetails = new Label
            {
                Text = $"{task.WorkType}  |  Last opened: {task.LastOpenedAt:g}",
                Font = new Font("Tahoma", 10F),
                ForeColor = Color.DimGray,
                Location = new Point(15, 42),
                AutoSize = true
            };

            panel.Controls.Add(lblField);
            panel.Controls.Add(lblTaskName);
            panel.Controls.Add(lblDetails);

            panel.Click += TaskPanel_Click;
            lblField.Click += (s, ev) => TaskPanel_Click(panel, ev);
            lblTaskName.Click += (s, ev) => TaskPanel_Click(panel, ev);
            lblDetails.Click += (s, ev) => TaskPanel_Click(panel, ev);

            panel.MouseEnter += (s, ev) => panel.BackColor = Color.FromArgb(230, 240, 250);
            panel.MouseLeave += (s, ev) => panel.BackColor = Color.White;

            return panel;
        }

        private void TaskPanel_Click(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            if (panel?.Tag is TaskSelection selection)
            {
                DoResumeTask(selection.Task, selection.FieldDirectory);
            }
        }

        private void btnResumeListBack_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.Main);
        }

        private void DoResumeTask(CTask task, string fieldDir)
        {
            // Save current field if open
            if (mf.isJobStarted)
            {
                _ = mf.FileSaveEverythingBeforeClosingField();
            }

            if (mf.taskManager.ResumeTask(task, fieldDir))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsError, "Failed to resume task");
            }
        }

        #endregion

        private void FormStartWork_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setJobMenu_location = Location;
            Properties.Settings.Default.setJobMenu_size = Size;
            Properties.Settings.Default.Save();
        }

        private class TaskSelection
        {
            public CTask Task { get; set; }
            public string FieldDirectory { get; set; }
        }
    }
}

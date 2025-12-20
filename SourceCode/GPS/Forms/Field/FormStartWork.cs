using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Streamers;
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
        private enum ViewMode { Main, OpenField, AddField, WizardStep1, WizardStep2, WizardStep3, ResumeTaskList }
        private ViewMode currentView = ViewMode.Main;

        // Task data
        private CTask lastTask;
        private List<(CTask Task, string FieldDirectory)> allActiveTasks;

        // Wizard selections
        private string selectedProfile;
        private string selectedFieldDir;
        private string selectedFieldName;
        private string selectedWorkType;

        // Previous tasks for current field
        private List<CTask> previousTasksForField;

        // Coverage import
        private bool shouldImportCoverage = false;

        // New field creation
        private bool isCreatingNewField = false;

        // Field list data
        private readonly List<FieldInfo> fieldInfoList = new List<FieldInfo>();
        private int fieldSortOrder = 0; // 0=Name, 1=Distance, 2=Area

        private class FieldInfo
        {
            public string Name { get; set; }
            public double Distance { get; set; } // in km, double.MaxValue if unknown
            public double Area { get; set; } // in ha or acres, -1 if unknown
            public string DistanceDisplay { get; set; }
            public string AreaDisplay { get; set; }
        }

        public FormStartWork(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void FormStartWork_Load(object sender, EventArgs e)
        {
            // Initialize WorkTypes
            WorkTypes.Initialize(RegistrySettings.vehiclesDirectory);

            // Initialize WorkTypeTemplates
            WorkTypeTemplates.Initialize(RegistrySettings.fieldsDirectory);

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
            panelAddField.Visible = false;
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
                case ViewMode.AddField:
                    panelAddField.Visible = true;
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
                string taskInfo = $"Last Task: {lastTask.Name} ({lastTask.FieldName} - {lastTask.WorkType})";
                if (!string.IsNullOrEmpty(lastTask.Notes))
                {
                    taskInfo += $" - {TruncateNotes(lastTask.Notes, 40)}";
                }
                lblLastTaskInfo.Text = taskInfo;
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

        private async void btnOpenFieldLocal_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
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
            ShowView(ViewMode.AddField);
        }

        #endregion

        #region Add Field Panel

        private void btnAddFieldBack_Click(object sender, EventArgs e)
        {
            ShowView(ViewMode.Main);
        }

        private async void btnAddFieldNew_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
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

        private async void btnAddFieldFromISOXML_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            using (var form = new FormFieldIsoXml(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        private async void btnAddFieldFromKML_Click(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
            }

            using (var form = new FormFieldKML(mf))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
        }

        #endregion

        #region Task Management

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
            LoadFieldList();
            UpdateFieldListView();
        }

        private void LoadFieldList()
        {
            fieldInfoList.Clear();

            string[] dirs;
            try
            {
                dirs = Directory.GetDirectories(RegistrySettings.fieldsDirectory);
            }
            catch
            {
                dirs = Array.Empty<string>();
            }

            if (dirs == null || dirs.Length < 1)
                return;

            string currentFieldDir = null;
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                currentFieldDir = mf.currentFieldDirectory;
            }

            foreach (string dir in dirs)
            {
                string fieldDirectory = Path.GetFileName(dir);
                string fieldFile = Path.Combine(dir, "Field.txt");

                // Skip current field and folders without Field.txt
                if (!File.Exists(fieldFile))
                    continue;
                if (currentFieldDir != null && fieldDirectory == currentFieldDir)
                    continue;

                var info = new FieldInfo { Name = fieldDirectory };

                // Get distance
                GetFieldDistance(fieldFile, info);

                // Get area
                GetFieldArea(dir, info);

                fieldInfoList.Add(info);
            }
        }

        private void GetFieldDistance(string filename, FieldInfo info)
        {
            try
            {
                using (var reader = new GeoStreamReader(filename))
                {
                    // Skip 8 lines to get to position
                    for (int i = 0; i < 8; i++) reader.ReadLine();

                    if (!reader.EndOfStream)
                    {
                        var startLatLon = reader.ReadWgs84();
                        double km = startLatLon.DistanceInKiloMeters(mf.AppModel.CurrentLatLon);
                        info.Distance = km;
                        info.DistanceDisplay = km.ToString("N2");
                    }
                    else
                    {
                        info.Distance = double.MaxValue;
                        info.DistanceDisplay = "---";
                    }
                }
            }
            catch
            {
                info.Distance = double.MaxValue;
                info.DistanceDisplay = "---";
            }
        }

        private void GetFieldArea(string dir, FieldInfo info)
        {
            string filename = Path.Combine(dir, "Boundary.txt");

            if (!File.Exists(filename))
            {
                info.Area = -1;
                info.AreaDisplay = "---";
                return;
            }

            try
            {
                double area = CalculateBoundaryArea(filename);
                if (area == 0)
                {
                    info.Area = -1;
                    info.AreaDisplay = "No Bndry";
                }
                else
                {
                    info.Area = area;
                    info.AreaDisplay = area.ToString("N1");
                }
            }
            catch
            {
                info.Area = -1;
                info.AreaDisplay = "---";
            }
        }

        private double CalculateBoundaryArea(string filename)
        {
            var pointList = new List<vec3>();
            using (var reader = new StreamReader(filename))
            {
                string line = reader.ReadLine();
                if (line == null) return 0;

                line = reader.ReadLine();
                if (line == null) return 0;

                if (line == "True" || line == "False")
                {
                    line = reader.ReadLine();
                    if (line == null) return 0;
                }
                if (line == "True" || line == "False")
                {
                    line = reader.ReadLine();
                    if (line == null) return 0;
                }

                if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out int numPoints))
                    return 0;

                if (numPoints <= 0) return 0;

                for (int i = 0; i < numPoints; i++)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) return 0;

                    var words = line.Split(',');
                    if (words.Length < 3) return 0;

                    if (!double.TryParse(words[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double e)) return 0;
                    if (!double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double n)) return 0;
                    if (!double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double h)) return 0;

                    pointList.Add(new vec3(e, n, h));
                }
            }

            int ptCount = pointList.Count;
            if (ptCount <= 5) return 0;

            // Shoelace algorithm
            double acc = 0;
            int j = ptCount - 1;
            for (int i = 0; i < ptCount; j = i++)
            {
                acc += (pointList[j].easting + pointList[i].easting) *
                       (pointList[j].northing - pointList[i].northing);
            }

            double areaM2 = Math.Abs(acc / 2.0);
            return mf.isMetric ? (areaM2 * 0.0001) : (areaM2 * 0.00024711);
        }

        private void UpdateFieldListView()
        {
            lvFields.Items.Clear();

            // Sort the list
            var sortedList = GetSortedFieldList();

            // Add header row (bold)
            string distanceUnit = mf.isMetric ? " (km)" : " (mi)";
            string areaUnit = mf.isMetric ? " (ha)" : " (ac)";
            var headerItem = new ListViewItem(new[] { gStr.gsField, gStr.gsDistance + distanceUnit, gStr.gsArea + areaUnit });
            headerItem.Font = new Font(lvFields.Font, FontStyle.Bold);
            headerItem.BackColor = Color.FromArgb(230, 230, 230);
            headerItem.Tag = "header";
            lvFields.Items.Add(headerItem);

            // Add current field if open
            if (mf.isJobStarted && !string.IsNullOrEmpty(mf.currentFieldDirectory))
            {
                var currentItem = new ListViewItem(new[] { $"Current: {mf.displayFieldName}", "0.00", "---" });
                currentItem.Tag = "current";
                lvFields.Items.Add(currentItem);
            }

            // Get last field from last task for auto-selection
            string lastFieldName = lastTask?.FieldName;
            int lastFieldIndex = -1;

            foreach (var info in sortedList)
            {
                var item = new ListViewItem(new[] { info.Name, info.DistanceDisplay, info.AreaDisplay });
                item.Tag = info.Name;
                lvFields.Items.Add(item);

                if (info.Name == lastFieldName)
                {
                    lastFieldIndex = lvFields.Items.Count - 1;
                }
            }

            UpdateFieldColumnHeaders();
            UpdateSortButton();

            // Select last opened field, or current field, or first item (skip header)
            if (lastFieldIndex >= 0)
            {
                lvFields.Items[lastFieldIndex].Selected = true;
                lvFields.Items[lastFieldIndex].EnsureVisible();
            }
            else if (lvFields.Items.Count > 1)
            {
                lvFields.Items[1].Selected = true;
            }
        }

        private List<FieldInfo> GetSortedFieldList()
        {
            var sorted = new List<FieldInfo>(fieldInfoList);

            switch (fieldSortOrder)
            {
                case 0: // Sort by Name (A-Z)
                    sorted.Sort((a, b) => string.Compare(a.Name, b.Name, StringComparison.OrdinalIgnoreCase));
                    break;
                case 1: // Sort by Distance (nearest first)
                    sorted.Sort((a, b) => a.Distance.CompareTo(b.Distance));
                    break;
                case 2: // Sort by Area (largest first)
                    sorted.Sort((a, b) => b.Area.CompareTo(a.Area));
                    break;
            }

            return sorted;
        }

        private void UpdateFieldColumnHeaders()
        {
            chFieldName.Width = 520;
            chFieldDistance.Width = 200;
            chFieldArea.Width = 150;
        }

        private void UpdateSortButton()
        {
            switch (fieldSortOrder)
            {
                case 0:
                    btnSortFields.Text = "Sort: Name";
                    break;
                case 1:
                    btnSortFields.Text = "Sort: Distance";
                    break;
                case 2:
                    btnSortFields.Text = "Sort: Area";
                    break;
            }
        }

        private void btnSortFields_Click(object sender, EventArgs e)
        {
            fieldSortOrder = (fieldSortOrder + 1) % 3;
            UpdateFieldListView();
        }

        private void btnWizard2Next_Click(object sender, EventArgs e)
        {
            if (lvFields.SelectedItems.Count > 0)
            {
                var selectedItem = lvFields.SelectedItems[0];
                string tag = selectedItem.Tag?.ToString();

                // Ignore header row
                if (tag == "header")
                {
                    mf.TimedMessageBox(1500, "Select Field", "Please select a field");
                    return;
                }

                isCreatingNewField = false;

                if (tag == "current")
                {
                    selectedFieldName = mf.displayFieldName;
                    selectedFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory);
                }
                else
                {
                    selectedFieldName = tag;
                    selectedFieldDir = Path.Combine(RegistrySettings.fieldsDirectory, tag);
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

            // Hide the work type buttons panel
            flpWorkTypes.Visible = false;

            // Show selected work type label and change button
            lblSelectedWorkType.Text = $"Work Type: {selectedWorkType}";
            lblSelectedWorkType.Visible = true;
            btnChangeWorkType.Visible = true;

            // Update task name
            txtTaskName.Text = $"{DateTime.Now:yyyy-MM-dd}_{selectedWorkType}_{selectedProfile}";

            // Load notes template for this work type
            LoadNotesTemplate(selectedWorkType);

            // Load previous tasks for this field
            LoadPreviousTasks();
        }

        private void btnChangeWorkType_Click(object sender, EventArgs e)
        {
            // Hide the selected label and change button
            lblSelectedWorkType.Visible = false;
            btnChangeWorkType.Visible = false;

            // Hide notes and previous tasks panels
            panelNotes.Visible = false;
            panelPreviousTasks.Visible = false;

            // Show work type buttons again
            flpWorkTypes.Visible = true;

            // Clear selected work type
            selectedWorkType = null;
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

            // Save notes to task
            if (task != null)
            {
                SaveNotesToTask(task);

                // Save the updated task with notes
                TaskFiles.Save(task, isCreatingNewField
                    ? Path.Combine(RegistrySettings.fieldsDirectory, txtFieldName.Text.Trim())
                    : selectedFieldDir);
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

        #region Notes and Previous Tasks

        /// <summary>
        /// Loads the notes template for the selected work type
        /// </summary>
        private void LoadNotesTemplate(string workType)
        {
            if (string.IsNullOrEmpty(workType))
            {
                panelNotes.Visible = false;
                return;
            }

            // Get the template for this work type
            var template = WorkTypeTemplates.GetTemplate(workType);

            // Clear existing note fields
            flpNoteFields.Controls.Clear();

            // Create UI controls for each template field
            foreach (var field in template.Fields)
            {
                CreateNoteField(field);
            }

            // Show the notes panel
            panelNotes.Visible = true;

            // Load last used values if available
            LoadLastUsedNotes(workType);
        }

        /// <summary>
        /// Creates a UI control for a template field
        /// </summary>
        private void CreateNoteField(TemplateField field)
        {
            var panel = new Panel
            {
                Width = 400,
                Height = 40,
                Margin = new Padding(0, 5, 0, 5)
            };

            var label = new Label
            {
                Text = field.Label + ":",
                Font = new Font("Tahoma", 11F),
                ForeColor = Color.FromArgb(60, 60, 60),
                AutoSize = true,
                Location = new Point(0, 7)
            };
            panel.Controls.Add(label);

            if (field.FieldType == "dropdown")
            {
                var combo = new ComboBox
                {
                    Name = "note_" + field.Name,
                    Font = new Font("Tahoma", 11F),
                    Width = 250,
                    Location = new Point(150, 3),
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Tag = field.Name
                };

                if (!string.IsNullOrEmpty(field.Options))
                {
                    string[] options = field.Options.Split(',');
                    combo.Items.AddRange(options);
                    if (combo.Items.Count > 0)
                        combo.SelectedIndex = 0;
                }

                panel.Controls.Add(combo);
            }
            else // text
            {
                var textBox = new TextBox
                {
                    Name = "note_" + field.Name,
                    Font = new Font("Tahoma", 11F),
                    Width = 250,
                    Location = new Point(150, 5),
                    Tag = field.Name
                };

                if (!string.IsNullOrEmpty(field.DefaultValue))
                    textBox.Text = field.DefaultValue;

                panel.Controls.Add(textBox);
            }

            flpNoteFields.Controls.Add(panel);
        }

        /// <summary>
        /// Loads last used note values for a work type
        /// </summary>
        private void LoadLastUsedNotes(string workType)
        {
            var lastValues = WorkTypeTemplates.GetLastUsedValues(workType);
            if (lastValues.Count == 0)
                return;

            foreach (Control panelCtrl in flpNoteFields.Controls)
            {
                if (panelCtrl is Panel panel)
                {
                    foreach (Control ctrl in panel.Controls)
                    {
                        string fieldName = ctrl.Tag?.ToString();
                        if (string.IsNullOrEmpty(fieldName))
                            continue;

                        if (lastValues.ContainsKey(fieldName))
                        {
                            if (ctrl is TextBox tb)
                                tb.Text = lastValues[fieldName];
                            else if (ctrl is ComboBox cb)
                            {
                                int index = cb.Items.IndexOf(lastValues[fieldName]);
                                if (index >= 0)
                                    cb.SelectedIndex = index;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves note field values to the task
        /// </summary>
        private void SaveNotesToTask(CTask task)
        {
            if (task == null)
                return;

            task.NoteFields = new Dictionary<string, string>();
            var notesParts = new List<string>();

            foreach (Control panelCtrl in flpNoteFields.Controls)
            {
                if (panelCtrl is Panel panel)
                {
                    foreach (Control ctrl in panel.Controls)
                    {
                        string fieldName = ctrl.Tag?.ToString();
                        if (string.IsNullOrEmpty(fieldName))
                            continue;

                        string value = "";
                        if (ctrl is TextBox tb)
                            value = tb.Text;
                        else if (ctrl is ComboBox cb)
                            value = cb.Text;

                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            task.NoteFields[fieldName] = value;
                            notesParts.Add(value);
                        }
                    }
                }
            }

            // Create formatted notes string
            task.Notes = string.Join(" ", notesParts);

            // Save as last used values for this work type
            if (task.NoteFields.Count > 0)
            {
                WorkTypeTemplates.SaveLastUsedValues(task.WorkType, task.NoteFields);
            }
        }

        /// <summary>
        /// Loads previous tasks for the selected field
        /// </summary>
        private void LoadPreviousTasks()
        {
            lvPreviousTasks.Items.Clear();
            previousTasksForField = null;

            if (string.IsNullOrEmpty(selectedFieldName))
            {
                panelPreviousTasks.Visible = false;
                return;
            }

            var tasks = TaskFiles.ListAllTasksForField(RegistrySettings.fieldsDirectory, selectedFieldName);

            if (tasks.Count == 0)
            {
                panelPreviousTasks.Visible = false;
                return;
            }

            // Store the tasks list for later reference
            previousTasksForField = tasks.Take(10).ToList();

            // Show up to 10 most recent tasks
            foreach (var task in previousTasksForField)
            {
                var item = new ListViewItem(new[] {
                    task.CreatedAt.ToString("dd-MM-yyyy"),
                    task.WorkType,
                    TruncateNotes(task.Notes, 30)
                });
                lvPreviousTasks.Items.Add(item);
            }

            panelPreviousTasks.Visible = true;
        }

        /// <summary>
        /// Truncates notes to a maximum length
        /// </summary>
        private string TruncateNotes(string notes, int maxLength)
        {
            if (string.IsNullOrEmpty(notes))
                return "-";

            if (notes.Length <= maxLength)
                return notes;

            return notes.Substring(0, maxLength - 3) + "...";
        }

        /// <summary>
        /// Event handler for Previous Tasks list selection change - auto-loads selected task's notes
        /// </summary>
        private void lvPreviousTasks_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvPreviousTasks.SelectedIndices.Count == 0 || previousTasksForField == null)
                return;

            int selectedIndex = lvPreviousTasks.SelectedIndices[0];
            if (selectedIndex < 0 || selectedIndex >= previousTasksForField.Count)
                return;

            var selectedTask = previousTasksForField[selectedIndex];
            if (selectedTask.NoteFields == null || selectedTask.NoteFields.Count == 0)
                return;

            // Auto-load notes from the selected task
            foreach (Control panelCtrl in flpNoteFields.Controls)
            {
                if (panelCtrl is Panel panel)
                {
                    foreach (Control ctrl in panel.Controls)
                    {
                        string fieldName = ctrl.Tag?.ToString();
                        if (string.IsNullOrEmpty(fieldName))
                            continue;

                        if (selectedTask.NoteFields.ContainsKey(fieldName))
                        {
                            if (ctrl is TextBox tb)
                                tb.Text = selectedTask.NoteFields[fieldName];
                            else if (ctrl is ComboBox cb)
                            {
                                int index = cb.Items.IndexOf(selectedTask.NoteFields[fieldName]);
                                if (index >= 0)
                                    cb.SelectedIndex = index;
                            }
                        }
                    }
                }
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
                Height = string.IsNullOrEmpty(task.Notes) ? 70 : 95,
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

            // Add notes if available
            if (!string.IsNullOrEmpty(task.Notes))
            {
                var lblNotes = new Label
                {
                    Text = $"Notes: {TruncateNotes(task.Notes, 80)}",
                    Font = new Font("Tahoma", 9F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(100, 100, 100),
                    Location = new Point(15, 62),
                    AutoSize = true,
                    MaximumSize = new Size(panel.Width - 30, 0)
                };
                panel.Controls.Add(lblNotes);
                lblNotes.Click += (s, ev) => TaskPanel_Click(panel, ev);
            }

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

        private async void DoResumeTask(CTask task, string fieldDir)
        {
            // Save current field if open
            if (mf.isJobStarted)
            {
                await mf.FileSaveEverythingBeforeClosingField();
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

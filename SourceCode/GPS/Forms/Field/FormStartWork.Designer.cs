using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AgOpenGPS
{
    partial class FormStartWork
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelContainer = new System.Windows.Forms.Panel();
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.tableTasks = new System.Windows.Forms.TableLayoutPanel();
            this.btnResumeLastTask = new System.Windows.Forms.Button();
            this.btnResumeTask = new System.Windows.Forms.Button();
            this.btnFinishTask = new System.Windows.Forms.Button();
            this.btnNewTask = new System.Windows.Forms.Button();
            this.btnCloseTask = new System.Windows.Forms.Button();
            this.btnExportTasks = new System.Windows.Forms.Button();
            this.lblFieldSection = new System.Windows.Forms.Label();
            this.tableFields = new System.Windows.Forms.TableLayoutPanel();
            this.btnOpenField = new System.Windows.Forms.Button();
            this.btnNewField = new System.Windows.Forms.Button();
            this.btnCloseField = new System.Windows.Forms.Button();
            this.lblLastTaskInfo = new System.Windows.Forms.Label();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.panelOpenField = new System.Windows.Forms.Panel();
            this.lblLocalStorageTitle = new System.Windows.Forms.Label();
            this.tableLocalStorage = new System.Windows.Forms.TableLayoutPanel();
            this.btnOpenFieldLocal = new System.Windows.Forms.Button();
            this.lblAgShareCloudTitle = new System.Windows.Forms.Label();
            this.tableAgShareCloud = new System.Windows.Forms.TableLayoutPanel();
            this.btnOpenFromAgShare = new System.Windows.Forms.Button();
            this.btnUploadToAgShare = new System.Windows.Forms.Button();
            this.btnOpenFieldBack = new System.Windows.Forms.Button();
            this.panelAddField = new System.Windows.Forms.Panel();
            this.lblAddFieldTitle = new System.Windows.Forms.Label();
            this.tableAddField = new System.Windows.Forms.TableLayoutPanel();
            this.btnAddFieldNew = new System.Windows.Forms.Button();
            this.btnAddFieldFromISOXML = new System.Windows.Forms.Button();
            this.btnAddFieldFromKML = new System.Windows.Forms.Button();
            this.btnAddFieldBack = new System.Windows.Forms.Button();
            this.panelWizardStep1 = new System.Windows.Forms.Panel();
            this.lblStep1Title = new System.Windows.Forms.Label();
            this.listProfiles = new System.Windows.Forms.ListBox();
            this.btnWizard1Next = new System.Windows.Forms.Button();
            this.btnWizard1Back = new System.Windows.Forms.Button();
            this.panelWizardStep2 = new System.Windows.Forms.Panel();
            this.lblStep2Title = new System.Windows.Forms.Label();
            this.lvFields = new System.Windows.Forms.ListView();
            this.chFieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFieldDistance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chFieldArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnSortFields = new System.Windows.Forms.Button();
            this.btnWizard2NewField = new System.Windows.Forms.Button();
            this.btnWizard2Next = new System.Windows.Forms.Button();
            this.btnWizard2Back = new System.Windows.Forms.Button();
            this.panelWizardStep3 = new System.Windows.Forms.Panel();
            this.lblStep3Title = new System.Windows.Forms.Label();
            this.lblFieldNameLabel = new System.Windows.Forms.Label();
            this.txtFieldName = new System.Windows.Forms.TextBox();
            this.flpWorkTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTaskNameLabel = new System.Windows.Forms.Label();
            this.txtTaskName = new System.Windows.Forms.TextBox();
            this.btnWizard3Start = new System.Windows.Forms.Button();
            this.btnWizard3Back = new System.Windows.Forms.Button();
            this.panelResumeList = new System.Windows.Forms.Panel();
            this.lblResumeTitle = new System.Windows.Forms.Label();
            this.flpTaskList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnResumeListBack = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnGoMode = new System.Windows.Forms.Button();
            this.panelContainer.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tableTasks.SuspendLayout();
            this.tableFields.SuspendLayout();
            this.panelOpenField.SuspendLayout();
            this.tableLocalStorage.SuspendLayout();
            this.tableAgShareCloud.SuspendLayout();
            this.panelAddField.SuspendLayout();
            this.tableAddField.SuspendLayout();
            this.panelWizardStep1.SuspendLayout();
            this.panelWizardStep2.SuspendLayout();
            this.panelWizardStep3.SuspendLayout();
            this.panelResumeList.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelContainer
            // 
            this.panelContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.panelContainer.Controls.Add(this.panelHeader);
            this.panelContainer.Controls.Add(this.panelMain);
            this.panelContainer.Controls.Add(this.panelOpenField);
            this.panelContainer.Controls.Add(this.panelAddField);
            this.panelContainer.Controls.Add(this.panelWizardStep1);
            this.panelContainer.Controls.Add(this.panelWizardStep2);
            this.panelContainer.Controls.Add(this.panelWizardStep3);
            this.panelContainer.Controls.Add(this.panelResumeList);
            this.panelContainer.Controls.Add(this.btnCancel);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(2, 2);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Size = new System.Drawing.Size(996, 746);
            this.panelContainer.TabIndex = 0;
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(996, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Tahoma", 22F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(996, 60);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Start Work Session";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelMain
            // 
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.tableTasks);
            this.panelMain.Controls.Add(this.lblFieldSection);
            this.panelMain.Controls.Add(this.tableFields);
            this.panelMain.Controls.Add(this.lblLastTaskInfo);
            this.panelMain.Controls.Add(this.lblCurrentField);
            this.panelMain.Location = new System.Drawing.Point(15, 75);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(966, 601);
            this.panelMain.TabIndex = 0;
            // 
            // tableTasks
            // 
            this.tableTasks.ColumnCount = 2;
            this.tableTasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableTasks.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableTasks.Controls.Add(this.btnResumeLastTask, 0, 0);
            this.tableTasks.Controls.Add(this.btnResumeTask, 1, 0);
            this.tableTasks.Controls.Add(this.btnFinishTask, 0, 1);
            this.tableTasks.Controls.Add(this.btnNewTask, 1, 1);
            this.tableTasks.Controls.Add(this.btnCloseTask, 0, 2);
            this.tableTasks.Controls.Add(this.btnExportTasks, 1, 2);
            this.tableTasks.Location = new System.Drawing.Point(0, 0);
            this.tableTasks.Name = "tableTasks";
            this.tableTasks.RowCount = 3;
            this.tableTasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableTasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableTasks.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableTasks.Size = new System.Drawing.Size(966, 300);
            this.tableTasks.TabIndex = 0;
            // 
            // btnResumeLastTask
            // 
            this.btnResumeLastTask.BackColor = System.Drawing.Color.White;
            this.btnResumeLastTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResumeLastTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResumeLastTask.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnResumeLastTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnResumeLastTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnResumeLastTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeLastTask.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnResumeLastTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnResumeLastTask.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnResumeLastTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeLastTask.Location = new System.Drawing.Point(12, 12);
            this.btnResumeLastTask.Margin = new System.Windows.Forms.Padding(12);
            this.btnResumeLastTask.Name = "btnResumeLastTask";
            this.btnResumeLastTask.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnResumeLastTask.Size = new System.Drawing.Size(459, 75);
            this.btnResumeLastTask.TabIndex = 0;
            this.btnResumeLastTask.Text = "  Resume Last Task";
            this.btnResumeLastTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeLastTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResumeLastTask.UseVisualStyleBackColor = false;
            this.btnResumeLastTask.Click += new System.EventHandler(this.btnResumeLastTask_Click);
            // 
            // btnResumeTask
            // 
            this.btnResumeTask.BackColor = System.Drawing.Color.White;
            this.btnResumeTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResumeTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResumeTask.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnResumeTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnResumeTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnResumeTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeTask.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnResumeTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnResumeTask.Image = global::AgOpenGPS.Properties.Resources.FileExisting;
            this.btnResumeTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeTask.Location = new System.Drawing.Point(495, 12);
            this.btnResumeTask.Margin = new System.Windows.Forms.Padding(12);
            this.btnResumeTask.Name = "btnResumeTask";
            this.btnResumeTask.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnResumeTask.Size = new System.Drawing.Size(459, 75);
            this.btnResumeTask.TabIndex = 1;
            this.btnResumeTask.Text = "  Resume Task (List)";
            this.btnResumeTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResumeTask.UseVisualStyleBackColor = false;
            this.btnResumeTask.Click += new System.EventHandler(this.btnResumeTask_Click);
            // 
            // btnFinishTask
            // 
            this.btnFinishTask.BackColor = System.Drawing.Color.White;
            this.btnFinishTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFinishTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFinishTask.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnFinishTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnFinishTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.btnFinishTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinishTask.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnFinishTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnFinishTask.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnFinishTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinishTask.Location = new System.Drawing.Point(12, 111);
            this.btnFinishTask.Margin = new System.Windows.Forms.Padding(12);
            this.btnFinishTask.Name = "btnFinishTask";
            this.btnFinishTask.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnFinishTask.Size = new System.Drawing.Size(459, 75);
            this.btnFinishTask.TabIndex = 5;
            this.btnFinishTask.Text = "  Finish Task";
            this.btnFinishTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinishTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFinishTask.UseVisualStyleBackColor = false;
            this.btnFinishTask.Click += new System.EventHandler(this.btnFinishTask_Click);
            // 
            // btnNewTask
            // 
            this.btnNewTask.BackColor = System.Drawing.Color.White;
            this.btnNewTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewTask.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnNewTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnNewTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnNewTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewTask.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnNewTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnNewTask.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnNewTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewTask.Location = new System.Drawing.Point(495, 111);
            this.btnNewTask.Margin = new System.Windows.Forms.Padding(12);
            this.btnNewTask.Name = "btnNewTask";
            this.btnNewTask.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNewTask.Size = new System.Drawing.Size(459, 75);
            this.btnNewTask.TabIndex = 1;
            this.btnNewTask.Text = "  New Task";
            this.btnNewTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewTask.UseVisualStyleBackColor = false;
            this.btnNewTask.Click += new System.EventHandler(this.btnNewTask_Click);
            // 
            // btnCloseTask
            // 
            this.btnCloseTask.BackColor = System.Drawing.Color.White;
            this.btnCloseTask.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseTask.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseTask.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnCloseTask.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnCloseTask.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(230)))));
            this.btnCloseTask.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseTask.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnCloseTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCloseTask.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnCloseTask.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseTask.Location = new System.Drawing.Point(12, 210);
            this.btnCloseTask.Margin = new System.Windows.Forms.Padding(12);
            this.btnCloseTask.Name = "btnCloseTask";
            this.btnCloseTask.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnCloseTask.Size = new System.Drawing.Size(459, 78);
            this.btnCloseTask.TabIndex = 4;
            this.btnCloseTask.Text = "  Close Task";
            this.btnCloseTask.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseTask.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseTask.UseVisualStyleBackColor = false;
            this.btnCloseTask.Click += new System.EventHandler(this.btnCloseTask_Click);
            // 
            // btnExportTasks
            // 
            this.btnExportTasks.BackColor = System.Drawing.Color.White;
            this.btnExportTasks.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnExportTasks.Enabled = false;
            this.btnExportTasks.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnExportTasks.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnExportTasks.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnExportTasks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportTasks.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnExportTasks.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(150)))), ((int)(((byte)(150)))), ((int)(((byte)(150)))));
            this.btnExportTasks.Image = global::AgOpenGPS.Properties.Resources.FileSaveAs;
            this.btnExportTasks.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportTasks.Location = new System.Drawing.Point(495, 210);
            this.btnExportTasks.Margin = new System.Windows.Forms.Padding(12);
            this.btnExportTasks.Name = "btnExportTasks";
            this.btnExportTasks.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnExportTasks.Size = new System.Drawing.Size(459, 78);
            this.btnExportTasks.TabIndex = 9;
            this.btnExportTasks.Text = "  Export Tasks";
            this.btnExportTasks.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnExportTasks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExportTasks.UseVisualStyleBackColor = false;
            // 
            // lblFieldSection
            // 
            this.lblFieldSection.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblFieldSection.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblFieldSection.ForeColor = System.Drawing.Color.White;
            this.lblFieldSection.Location = new System.Drawing.Point(0, 333);
            this.lblFieldSection.Name = "lblFieldSection";
            this.lblFieldSection.Size = new System.Drawing.Size(966, 35);
            this.lblFieldSection.TabIndex = 1;
            this.lblFieldSection.Text = "  Field Management";
            this.lblFieldSection.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableFields
            // 
            this.tableFields.ColumnCount = 2;
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.Controls.Add(this.btnGoMode, 1, 1);
            this.tableFields.Controls.Add(this.btnOpenField, 0, 0);
            this.tableFields.Controls.Add(this.btnNewField, 1, 0);
            this.tableFields.Controls.Add(this.btnCloseField, 0, 1);
            this.tableFields.Location = new System.Drawing.Point(0, 368);
            this.tableFields.Name = "tableFields";
            this.tableFields.RowCount = 2;
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableFields.Size = new System.Drawing.Size(966, 200);
            this.tableFields.TabIndex = 6;
            // 
            // btnOpenField
            // 
            this.btnOpenField.BackColor = System.Drawing.Color.White;
            this.btnOpenField.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnOpenField.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnOpenField.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnOpenField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnOpenField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnOpenField.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenField.Location = new System.Drawing.Point(12, 12);
            this.btnOpenField.Margin = new System.Windows.Forms.Padding(12);
            this.btnOpenField.Name = "btnOpenField";
            this.btnOpenField.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnOpenField.Size = new System.Drawing.Size(459, 76);
            this.btnOpenField.TabIndex = 2;
            this.btnOpenField.Text = "  Open Field";
            this.btnOpenField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenField.UseVisualStyleBackColor = false;
            this.btnOpenField.Click += new System.EventHandler(this.btnOpenField_Click);
            // 
            // btnNewField
            // 
            this.btnNewField.BackColor = System.Drawing.Color.White;
            this.btnNewField.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnNewField.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnNewField.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.btnNewField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnNewField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnNewField.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnNewField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewField.Location = new System.Drawing.Point(495, 12);
            this.btnNewField.Margin = new System.Windows.Forms.Padding(12);
            this.btnNewField.Name = "btnNewField";
            this.btnNewField.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNewField.Size = new System.Drawing.Size(459, 76);
            this.btnNewField.TabIndex = 7;
            this.btnNewField.Text = "  Add Field";
            this.btnNewField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewField.UseVisualStyleBackColor = false;
            this.btnNewField.Click += new System.EventHandler(this.btnNewField_Click);
            // 
            // btnCloseField
            // 
            this.btnCloseField.BackColor = System.Drawing.Color.White;
            this.btnCloseField.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseField.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnCloseField.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnCloseField.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnCloseField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnCloseField.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCloseField.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnCloseField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.Location = new System.Drawing.Point(12, 112);
            this.btnCloseField.Margin = new System.Windows.Forms.Padding(12);
            this.btnCloseField.Name = "btnCloseField";
            this.btnCloseField.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnCloseField.Size = new System.Drawing.Size(459, 76);
            this.btnCloseField.TabIndex = 3;
            this.btnCloseField.Text = "  Close Field";
            this.btnCloseField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseField.UseVisualStyleBackColor = false;
            this.btnCloseField.Click += new System.EventHandler(this.btnCloseField_Click);
            // 
            // lblLastTaskInfo
            // 
            this.lblLastTaskInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lblLastTaskInfo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLastTaskInfo.ForeColor = System.Drawing.Color.White;
            this.lblLastTaskInfo.Location = new System.Drawing.Point(0, 300);
            this.lblLastTaskInfo.Name = "lblLastTaskInfo";
            this.lblLastTaskInfo.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblLastTaskInfo.Size = new System.Drawing.Size(966, 25);
            this.lblLastTaskInfo.TabIndex = 1;
            this.lblLastTaskInfo.Text = "Last Task: --";
            this.lblLastTaskInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCurrentField
            // 
            this.lblCurrentField.BackColor = System.Drawing.Color.Green;
            this.lblCurrentField.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentField.ForeColor = System.Drawing.Color.White;
            this.lblCurrentField.Location = new System.Drawing.Point(0, 568);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.lblCurrentField.Size = new System.Drawing.Size(966, 25);
            this.lblCurrentField.TabIndex = 2;
            this.lblCurrentField.Text = "Last Field: --";
            this.lblCurrentField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelOpenField
            // 
            this.panelOpenField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOpenField.Controls.Add(this.lblLocalStorageTitle);
            this.panelOpenField.Controls.Add(this.tableLocalStorage);
            this.panelOpenField.Controls.Add(this.lblAgShareCloudTitle);
            this.panelOpenField.Controls.Add(this.tableAgShareCloud);
            this.panelOpenField.Controls.Add(this.btnOpenFieldBack);
            this.panelOpenField.Location = new System.Drawing.Point(15, 75);
            this.panelOpenField.Name = "panelOpenField";
            this.panelOpenField.Size = new System.Drawing.Size(966, 595);
            this.panelOpenField.TabIndex = 5;
            this.panelOpenField.Visible = false;
            // 
            // lblLocalStorageTitle
            // 
            this.lblLocalStorageTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblLocalStorageTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblLocalStorageTitle.ForeColor = System.Drawing.Color.White;
            this.lblLocalStorageTitle.Location = new System.Drawing.Point(0, 0);
            this.lblLocalStorageTitle.Name = "lblLocalStorageTitle";
            this.lblLocalStorageTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblLocalStorageTitle.Size = new System.Drawing.Size(966, 45);
            this.lblLocalStorageTitle.TabIndex = 0;
            this.lblLocalStorageTitle.Text = "Local Storage";
            this.lblLocalStorageTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLocalStorage
            // 
            this.tableLocalStorage.ColumnCount = 1;
            this.tableLocalStorage.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLocalStorage.Controls.Add(this.btnOpenFieldLocal, 0, 0);
            this.tableLocalStorage.Location = new System.Drawing.Point(0, 45);
            this.tableLocalStorage.Name = "tableLocalStorage";
            this.tableLocalStorage.RowCount = 1;
            this.tableLocalStorage.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLocalStorage.Size = new System.Drawing.Size(966, 100);
            this.tableLocalStorage.TabIndex = 1;
            // 
            // btnOpenFieldLocal
            // 
            this.btnOpenFieldLocal.BackColor = System.Drawing.Color.White;
            this.btnOpenFieldLocal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFieldLocal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFieldLocal.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnOpenFieldLocal.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnOpenFieldLocal.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnOpenFieldLocal.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFieldLocal.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnOpenFieldLocal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnOpenFieldLocal.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenFieldLocal.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldLocal.Location = new System.Drawing.Point(12, 12);
            this.btnOpenFieldLocal.Margin = new System.Windows.Forms.Padding(12);
            this.btnOpenFieldLocal.Name = "btnOpenFieldLocal";
            this.btnOpenFieldLocal.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnOpenFieldLocal.Size = new System.Drawing.Size(942, 76);
            this.btnOpenFieldLocal.TabIndex = 0;
            this.btnOpenFieldLocal.Text = "  Open Local";
            this.btnOpenFieldLocal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldLocal.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFieldLocal.UseVisualStyleBackColor = false;
            this.btnOpenFieldLocal.Click += new System.EventHandler(this.btnOpenFieldLocal_Click);
            // 
            // lblAgShareCloudTitle
            // 
            this.lblAgShareCloudTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblAgShareCloudTitle.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblAgShareCloudTitle.ForeColor = System.Drawing.Color.White;
            this.lblAgShareCloudTitle.Location = new System.Drawing.Point(0, 155);
            this.lblAgShareCloudTitle.Name = "lblAgShareCloudTitle";
            this.lblAgShareCloudTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblAgShareCloudTitle.Size = new System.Drawing.Size(966, 45);
            this.lblAgShareCloudTitle.TabIndex = 2;
            this.lblAgShareCloudTitle.Text = "AgShare Cloud";
            this.lblAgShareCloudTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableAgShareCloud
            // 
            this.tableAgShareCloud.ColumnCount = 2;
            this.tableAgShareCloud.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableAgShareCloud.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableAgShareCloud.Controls.Add(this.btnOpenFromAgShare, 0, 0);
            this.tableAgShareCloud.Controls.Add(this.btnUploadToAgShare, 1, 0);
            this.tableAgShareCloud.Location = new System.Drawing.Point(0, 200);
            this.tableAgShareCloud.Name = "tableAgShareCloud";
            this.tableAgShareCloud.RowCount = 1;
            this.tableAgShareCloud.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableAgShareCloud.Size = new System.Drawing.Size(966, 100);
            this.tableAgShareCloud.TabIndex = 3;
            // 
            // btnOpenFromAgShare
            // 
            this.btnOpenFromAgShare.BackColor = System.Drawing.Color.White;
            this.btnOpenFromAgShare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFromAgShare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFromAgShare.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnOpenFromAgShare.FlatAppearance.BorderSize = 2;
            this.btnOpenFromAgShare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.btnOpenFromAgShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFromAgShare.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnOpenFromAgShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnOpenFromAgShare.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnOpenFromAgShare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFromAgShare.Location = new System.Drawing.Point(12, 12);
            this.btnOpenFromAgShare.Margin = new System.Windows.Forms.Padding(12);
            this.btnOpenFromAgShare.Name = "btnOpenFromAgShare";
            this.btnOpenFromAgShare.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnOpenFromAgShare.Size = new System.Drawing.Size(459, 76);
            this.btnOpenFromAgShare.TabIndex = 1;
            this.btnOpenFromAgShare.Text = "  Download";
            this.btnOpenFromAgShare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFromAgShare.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFromAgShare.UseVisualStyleBackColor = false;
            this.btnOpenFromAgShare.Click += new System.EventHandler(this.btnOpenFromAgShare_Click);
            // 
            // btnUploadToAgShare
            // 
            this.btnUploadToAgShare.BackColor = System.Drawing.Color.White;
            this.btnUploadToAgShare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUploadToAgShare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnUploadToAgShare.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnUploadToAgShare.FlatAppearance.BorderSize = 2;
            this.btnUploadToAgShare.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.btnUploadToAgShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadToAgShare.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnUploadToAgShare.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnUploadToAgShare.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnUploadToAgShare.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadToAgShare.Location = new System.Drawing.Point(495, 12);
            this.btnUploadToAgShare.Margin = new System.Windows.Forms.Padding(12);
            this.btnUploadToAgShare.Name = "btnUploadToAgShare";
            this.btnUploadToAgShare.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnUploadToAgShare.Size = new System.Drawing.Size(459, 76);
            this.btnUploadToAgShare.TabIndex = 2;
            this.btnUploadToAgShare.Text = "  Upload";
            this.btnUploadToAgShare.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnUploadToAgShare.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnUploadToAgShare.UseVisualStyleBackColor = false;
            this.btnUploadToAgShare.Click += new System.EventHandler(this.btnUploadToAgShare_Click);
            // 
            // btnOpenFieldBack
            // 
            this.btnOpenFieldBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnOpenFieldBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFieldBack.FlatAppearance.BorderSize = 0;
            this.btnOpenFieldBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFieldBack.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnOpenFieldBack.ForeColor = System.Drawing.Color.White;
            this.btnOpenFieldBack.Location = new System.Drawing.Point(30, 320);
            this.btnOpenFieldBack.Name = "btnOpenFieldBack";
            this.btnOpenFieldBack.Size = new System.Drawing.Size(140, 55);
            this.btnOpenFieldBack.TabIndex = 4;
            this.btnOpenFieldBack.Text = "< Back";
            this.btnOpenFieldBack.UseVisualStyleBackColor = false;
            this.btnOpenFieldBack.Click += new System.EventHandler(this.btnOpenFieldBack_Click);
            // 
            // panelAddField
            // 
            this.panelAddField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAddField.Controls.Add(this.lblAddFieldTitle);
            this.panelAddField.Controls.Add(this.tableAddField);
            this.panelAddField.Controls.Add(this.btnAddFieldBack);
            this.panelAddField.Location = new System.Drawing.Point(15, 75);
            this.panelAddField.Name = "panelAddField";
            this.panelAddField.Size = new System.Drawing.Size(966, 595);
            this.panelAddField.TabIndex = 6;
            this.panelAddField.Visible = false;
            // 
            // lblAddFieldTitle
            // 
            this.lblAddFieldTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblAddFieldTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblAddFieldTitle.ForeColor = System.Drawing.Color.White;
            this.lblAddFieldTitle.Location = new System.Drawing.Point(0, 0);
            this.lblAddFieldTitle.Name = "lblAddFieldTitle";
            this.lblAddFieldTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblAddFieldTitle.Size = new System.Drawing.Size(966, 55);
            this.lblAddFieldTitle.TabIndex = 0;
            this.lblAddFieldTitle.Text = "Add Field";
            this.lblAddFieldTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableAddField
            // 
            this.tableAddField.ColumnCount = 1;
            this.tableAddField.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableAddField.Controls.Add(this.btnAddFieldNew, 0, 0);
            this.tableAddField.Controls.Add(this.btnAddFieldFromISOXML, 0, 1);
            this.tableAddField.Controls.Add(this.btnAddFieldFromKML, 0, 2);
            this.tableAddField.Location = new System.Drawing.Point(0, 55);
            this.tableAddField.Name = "tableAddField";
            this.tableAddField.RowCount = 3;
            this.tableAddField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableAddField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableAddField.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableAddField.Size = new System.Drawing.Size(966, 300);
            this.tableAddField.TabIndex = 1;
            // 
            // btnAddFieldNew
            // 
            this.btnAddFieldNew.BackColor = System.Drawing.Color.White;
            this.btnAddFieldNew.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddFieldNew.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddFieldNew.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnAddFieldNew.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnAddFieldNew.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.btnAddFieldNew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFieldNew.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnAddFieldNew.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAddFieldNew.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnAddFieldNew.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFieldNew.Location = new System.Drawing.Point(12, 12);
            this.btnAddFieldNew.Margin = new System.Windows.Forms.Padding(12);
            this.btnAddFieldNew.Name = "btnAddFieldNew";
            this.btnAddFieldNew.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnAddFieldNew.Size = new System.Drawing.Size(942, 75);
            this.btnAddFieldNew.TabIndex = 0;
            this.btnAddFieldNew.Text = "  Create New Field";
            this.btnAddFieldNew.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFieldNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddFieldNew.UseVisualStyleBackColor = false;
            this.btnAddFieldNew.Click += new System.EventHandler(this.btnAddFieldNew_Click);
            // 
            // btnAddFieldFromISOXML
            // 
            this.btnAddFieldFromISOXML.BackColor = System.Drawing.Color.White;
            this.btnAddFieldFromISOXML.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddFieldFromISOXML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddFieldFromISOXML.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnAddFieldFromISOXML.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnAddFieldFromISOXML.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnAddFieldFromISOXML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFieldFromISOXML.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnAddFieldFromISOXML.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAddFieldFromISOXML.Image = global::AgOpenGPS.Properties.Resources.BoundaryLoadFromGE;
            this.btnAddFieldFromISOXML.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFieldFromISOXML.Location = new System.Drawing.Point(12, 111);
            this.btnAddFieldFromISOXML.Margin = new System.Windows.Forms.Padding(12);
            this.btnAddFieldFromISOXML.Name = "btnAddFieldFromISOXML";
            this.btnAddFieldFromISOXML.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnAddFieldFromISOXML.Size = new System.Drawing.Size(942, 75);
            this.btnAddFieldFromISOXML.TabIndex = 1;
            this.btnAddFieldFromISOXML.Text = "  From ISO-XML";
            this.btnAddFieldFromISOXML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFieldFromISOXML.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddFieldFromISOXML.UseVisualStyleBackColor = false;
            this.btnAddFieldFromISOXML.Click += new System.EventHandler(this.btnAddFieldFromISOXML_Click);
            // 
            // btnAddFieldFromKML
            // 
            this.btnAddFieldFromKML.BackColor = System.Drawing.Color.White;
            this.btnAddFieldFromKML.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddFieldFromKML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAddFieldFromKML.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnAddFieldFromKML.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnAddFieldFromKML.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnAddFieldFromKML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFieldFromKML.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnAddFieldFromKML.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnAddFieldFromKML.Image = global::AgOpenGPS.Properties.Resources.BoundaryLoadFromGE;
            this.btnAddFieldFromKML.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFieldFromKML.Location = new System.Drawing.Point(12, 210);
            this.btnAddFieldFromKML.Margin = new System.Windows.Forms.Padding(12);
            this.btnAddFieldFromKML.Name = "btnAddFieldFromKML";
            this.btnAddFieldFromKML.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnAddFieldFromKML.Size = new System.Drawing.Size(942, 78);
            this.btnAddFieldFromKML.TabIndex = 2;
            this.btnAddFieldFromKML.Text = "  From KML";
            this.btnAddFieldFromKML.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddFieldFromKML.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAddFieldFromKML.UseVisualStyleBackColor = false;
            this.btnAddFieldFromKML.Click += new System.EventHandler(this.btnAddFieldFromKML_Click);
            // 
            // btnAddFieldBack
            // 
            this.btnAddFieldBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnAddFieldBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddFieldBack.FlatAppearance.BorderSize = 0;
            this.btnAddFieldBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFieldBack.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnAddFieldBack.ForeColor = System.Drawing.Color.White;
            this.btnAddFieldBack.Location = new System.Drawing.Point(30, 380);
            this.btnAddFieldBack.Name = "btnAddFieldBack";
            this.btnAddFieldBack.Size = new System.Drawing.Size(140, 55);
            this.btnAddFieldBack.TabIndex = 3;
            this.btnAddFieldBack.Text = "< Back";
            this.btnAddFieldBack.UseVisualStyleBackColor = false;
            this.btnAddFieldBack.Click += new System.EventHandler(this.btnAddFieldBack_Click);
            // 
            // panelWizardStep1
            // 
            this.panelWizardStep1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizardStep1.Controls.Add(this.lblStep1Title);
            this.panelWizardStep1.Controls.Add(this.listProfiles);
            this.panelWizardStep1.Controls.Add(this.btnWizard1Next);
            this.panelWizardStep1.Controls.Add(this.btnWizard1Back);
            this.panelWizardStep1.Location = new System.Drawing.Point(15, 75);
            this.panelWizardStep1.Name = "panelWizardStep1";
            this.panelWizardStep1.Size = new System.Drawing.Size(966, 595);
            this.panelWizardStep1.TabIndex = 1;
            this.panelWizardStep1.Visible = false;
            // 
            // lblStep1Title
            // 
            this.lblStep1Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblStep1Title.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblStep1Title.ForeColor = System.Drawing.Color.White;
            this.lblStep1Title.Location = new System.Drawing.Point(0, 0);
            this.lblStep1Title.Name = "lblStep1Title";
            this.lblStep1Title.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblStep1Title.Size = new System.Drawing.Size(966, 55);
            this.lblStep1Title.TabIndex = 0;
            this.lblStep1Title.Text = "Step 1: Select Machine Profile";
            this.lblStep1Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // listProfiles
            // 
            this.listProfiles.BackColor = System.Drawing.Color.White;
            this.listProfiles.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listProfiles.Font = new System.Drawing.Font("Tahoma", 20F);
            this.listProfiles.FormattingEnabled = true;
            this.listProfiles.ItemHeight = 33;
            this.listProfiles.Location = new System.Drawing.Point(30, 75);
            this.listProfiles.Name = "listProfiles";
            this.listProfiles.Size = new System.Drawing.Size(906, 332);
            this.listProfiles.TabIndex = 1;
            // 
            // btnWizard1Next
            // 
            this.btnWizard1Next.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnWizard1Next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard1Next.FlatAppearance.BorderSize = 0;
            this.btnWizard1Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard1Next.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnWizard1Next.ForeColor = System.Drawing.Color.White;
            this.btnWizard1Next.Location = new System.Drawing.Point(796, 460);
            this.btnWizard1Next.Name = "btnWizard1Next";
            this.btnWizard1Next.Size = new System.Drawing.Size(140, 55);
            this.btnWizard1Next.TabIndex = 3;
            this.btnWizard1Next.Text = "Next >";
            this.btnWizard1Next.UseVisualStyleBackColor = false;
            this.btnWizard1Next.Click += new System.EventHandler(this.btnWizard1Next_Click);
            // 
            // btnWizard1Back
            // 
            this.btnWizard1Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnWizard1Back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard1Back.FlatAppearance.BorderSize = 0;
            this.btnWizard1Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard1Back.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnWizard1Back.ForeColor = System.Drawing.Color.White;
            this.btnWizard1Back.Location = new System.Drawing.Point(30, 460);
            this.btnWizard1Back.Name = "btnWizard1Back";
            this.btnWizard1Back.Size = new System.Drawing.Size(140, 55);
            this.btnWizard1Back.TabIndex = 2;
            this.btnWizard1Back.Text = "< Cancel";
            this.btnWizard1Back.UseVisualStyleBackColor = false;
            this.btnWizard1Back.Click += new System.EventHandler(this.btnWizard1Back_Click);
            // 
            // panelWizardStep2
            // 
            this.panelWizardStep2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizardStep2.Controls.Add(this.lblStep2Title);
            this.panelWizardStep2.Controls.Add(this.lvFields);
            this.panelWizardStep2.Controls.Add(this.btnSortFields);
            this.panelWizardStep2.Controls.Add(this.btnWizard2NewField);
            this.panelWizardStep2.Controls.Add(this.btnWizard2Next);
            this.panelWizardStep2.Controls.Add(this.btnWizard2Back);
            this.panelWizardStep2.Location = new System.Drawing.Point(15, 75);
            this.panelWizardStep2.Name = "panelWizardStep2";
            this.panelWizardStep2.Size = new System.Drawing.Size(966, 595);
            this.panelWizardStep2.TabIndex = 2;
            this.panelWizardStep2.Visible = false;
            // 
            // lblStep2Title
            // 
            this.lblStep2Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblStep2Title.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblStep2Title.ForeColor = System.Drawing.Color.White;
            this.lblStep2Title.Location = new System.Drawing.Point(0, 0);
            this.lblStep2Title.Name = "lblStep2Title";
            this.lblStep2Title.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblStep2Title.Size = new System.Drawing.Size(966, 55);
            this.lblStep2Title.TabIndex = 0;
            this.lblStep2Title.Text = "Step 2: Select Field";
            this.lblStep2Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvFields
            // 
            this.lvFields.BackColor = System.Drawing.Color.White;
            this.lvFields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chFieldName,
            this.chFieldDistance,
            this.chFieldArea});
            this.lvFields.Font = new System.Drawing.Font("Tahoma", 20F);
            this.lvFields.FullRowSelect = true;
            this.lvFields.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvFields.HideSelection = false;
            this.lvFields.Location = new System.Drawing.Point(30, 75);
            this.lvFields.MultiSelect = false;
            this.lvFields.Name = "lvFields";
            this.lvFields.Size = new System.Drawing.Size(906, 332);
            this.lvFields.TabIndex = 1;
            this.lvFields.UseCompatibleStateImageBehavior = false;
            this.lvFields.View = System.Windows.Forms.View.Details;
            //
            // chFieldName
            //
            this.chFieldName.Text = "Field";
            this.chFieldName.Width = 520;
            //
            // chFieldDistance
            //
            this.chFieldDistance.Text = "Distance";
            this.chFieldDistance.Width = 200;
            //
            // chFieldArea
            //
            this.chFieldArea.Text = "Area";
            this.chFieldArea.Width = 150;
            // 
            // btnSortFields
            // 
            this.btnSortFields.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnSortFields.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSortFields.FlatAppearance.BorderSize = 0;
            this.btnSortFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSortFields.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.btnSortFields.ForeColor = System.Drawing.Color.White;
            this.btnSortFields.Location = new System.Drawing.Point(736, 420);
            this.btnSortFields.Name = "btnSortFields";
            this.btnSortFields.Size = new System.Drawing.Size(200, 35);
            this.btnSortFields.TabIndex = 5;
            this.btnSortFields.Text = "Sort: Name";
            this.btnSortFields.UseVisualStyleBackColor = false;
            this.btnSortFields.Click += new System.EventHandler(this.btnSortFields_Click);
            // 
            // btnWizard2NewField
            // 
            this.btnWizard2NewField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnWizard2NewField.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard2NewField.FlatAppearance.BorderSize = 0;
            this.btnWizard2NewField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard2NewField.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnWizard2NewField.ForeColor = System.Drawing.Color.White;
            this.btnWizard2NewField.Location = new System.Drawing.Point(383, 460);
            this.btnWizard2NewField.Name = "btnWizard2NewField";
            this.btnWizard2NewField.Size = new System.Drawing.Size(200, 55);
            this.btnWizard2NewField.TabIndex = 4;
            this.btnWizard2NewField.Text = "+ New Field";
            this.btnWizard2NewField.UseVisualStyleBackColor = false;
            this.btnWizard2NewField.Click += new System.EventHandler(this.btnWizard2NewField_Click);
            // 
            // btnWizard2Next
            // 
            this.btnWizard2Next.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.btnWizard2Next.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard2Next.FlatAppearance.BorderSize = 0;
            this.btnWizard2Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard2Next.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnWizard2Next.ForeColor = System.Drawing.Color.White;
            this.btnWizard2Next.Location = new System.Drawing.Point(796, 460);
            this.btnWizard2Next.Name = "btnWizard2Next";
            this.btnWizard2Next.Size = new System.Drawing.Size(140, 55);
            this.btnWizard2Next.TabIndex = 3;
            this.btnWizard2Next.Text = "Next >";
            this.btnWizard2Next.UseVisualStyleBackColor = false;
            this.btnWizard2Next.Click += new System.EventHandler(this.btnWizard2Next_Click);
            // 
            // btnWizard2Back
            // 
            this.btnWizard2Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnWizard2Back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard2Back.FlatAppearance.BorderSize = 0;
            this.btnWizard2Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard2Back.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnWizard2Back.ForeColor = System.Drawing.Color.White;
            this.btnWizard2Back.Location = new System.Drawing.Point(30, 460);
            this.btnWizard2Back.Name = "btnWizard2Back";
            this.btnWizard2Back.Size = new System.Drawing.Size(140, 55);
            this.btnWizard2Back.TabIndex = 2;
            this.btnWizard2Back.Text = "< Back";
            this.btnWizard2Back.UseVisualStyleBackColor = false;
            this.btnWizard2Back.Click += new System.EventHandler(this.btnWizard2Back_Click);
            // 
            // panelWizardStep3
            // 
            this.panelWizardStep3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizardStep3.Controls.Add(this.lblStep3Title);
            this.panelWizardStep3.Controls.Add(this.lblFieldNameLabel);
            this.panelWizardStep3.Controls.Add(this.txtFieldName);
            this.panelWizardStep3.Controls.Add(this.flpWorkTypes);
            this.panelWizardStep3.Controls.Add(this.lblTaskNameLabel);
            this.panelWizardStep3.Controls.Add(this.txtTaskName);
            this.panelWizardStep3.Controls.Add(this.btnWizard3Start);
            this.panelWizardStep3.Controls.Add(this.btnWizard3Back);
            this.panelWizardStep3.Location = new System.Drawing.Point(15, 75);
            this.panelWizardStep3.Name = "panelWizardStep3";
            this.panelWizardStep3.Size = new System.Drawing.Size(966, 595);
            this.panelWizardStep3.TabIndex = 3;
            this.panelWizardStep3.Visible = false;
            // 
            // lblStep3Title
            // 
            this.lblStep3Title.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblStep3Title.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblStep3Title.ForeColor = System.Drawing.Color.White;
            this.lblStep3Title.Location = new System.Drawing.Point(0, 0);
            this.lblStep3Title.Name = "lblStep3Title";
            this.lblStep3Title.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblStep3Title.Size = new System.Drawing.Size(966, 55);
            this.lblStep3Title.TabIndex = 0;
            this.lblStep3Title.Text = "Step 3: Select Work Type";
            this.lblStep3Title.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblFieldNameLabel
            // 
            this.lblFieldNameLabel.AutoSize = true;
            this.lblFieldNameLabel.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblFieldNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblFieldNameLabel.Location = new System.Drawing.Point(30, 65);
            this.lblFieldNameLabel.Name = "lblFieldNameLabel";
            this.lblFieldNameLabel.Size = new System.Drawing.Size(117, 22);
            this.lblFieldNameLabel.TabIndex = 6;
            this.lblFieldNameLabel.Text = "Field Name:";
            this.lblFieldNameLabel.Visible = false;
            // 
            // txtFieldName
            // 
            this.txtFieldName.BackColor = System.Drawing.Color.White;
            this.txtFieldName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFieldName.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtFieldName.Location = new System.Drawing.Point(30, 90);
            this.txtFieldName.Name = "txtFieldName";
            this.txtFieldName.Size = new System.Drawing.Size(906, 33);
            this.txtFieldName.TabIndex = 7;
            this.txtFieldName.Visible = false;
            this.txtFieldName.Enter += new System.EventHandler(this.txtFieldName_Enter);
            // 
            // flpWorkTypes
            // 
            this.flpWorkTypes.AutoScroll = true;
            this.flpWorkTypes.BackColor = System.Drawing.Color.White;
            this.flpWorkTypes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpWorkTypes.Location = new System.Drawing.Point(30, 75);
            this.flpWorkTypes.Name = "flpWorkTypes";
            this.flpWorkTypes.Padding = new System.Windows.Forms.Padding(10);
            this.flpWorkTypes.Size = new System.Drawing.Size(906, 280);
            this.flpWorkTypes.TabIndex = 1;
            // 
            // lblTaskNameLabel
            // 
            this.lblTaskNameLabel.AutoSize = true;
            this.lblTaskNameLabel.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblTaskNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblTaskNameLabel.Location = new System.Drawing.Point(30, 375);
            this.lblTaskNameLabel.Name = "lblTaskNameLabel";
            this.lblTaskNameLabel.Size = new System.Drawing.Size(117, 22);
            this.lblTaskNameLabel.TabIndex = 2;
            this.lblTaskNameLabel.Text = "Task Name:";
            // 
            // txtTaskName
            // 
            this.txtTaskName.BackColor = System.Drawing.Color.White;
            this.txtTaskName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTaskName.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtTaskName.Location = new System.Drawing.Point(30, 405);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.Size = new System.Drawing.Size(906, 33);
            this.txtTaskName.TabIndex = 3;
            this.txtTaskName.Enter += new System.EventHandler(this.txtTaskName_Enter);
            // 
            // btnWizard3Start
            // 
            this.btnWizard3Start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.btnWizard3Start.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard3Start.FlatAppearance.BorderSize = 0;
            this.btnWizard3Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard3Start.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWizard3Start.ForeColor = System.Drawing.Color.White;
            this.btnWizard3Start.Location = new System.Drawing.Point(746, 460);
            this.btnWizard3Start.Name = "btnWizard3Start";
            this.btnWizard3Start.Size = new System.Drawing.Size(190, 55);
            this.btnWizard3Start.TabIndex = 5;
            this.btnWizard3Start.Text = "START TASK";
            this.btnWizard3Start.UseVisualStyleBackColor = false;
            this.btnWizard3Start.Click += new System.EventHandler(this.btnWizard3Start_Click);
            // 
            // btnWizard3Back
            // 
            this.btnWizard3Back.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnWizard3Back.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWizard3Back.FlatAppearance.BorderSize = 0;
            this.btnWizard3Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard3Back.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnWizard3Back.ForeColor = System.Drawing.Color.White;
            this.btnWizard3Back.Location = new System.Drawing.Point(30, 460);
            this.btnWizard3Back.Name = "btnWizard3Back";
            this.btnWizard3Back.Size = new System.Drawing.Size(140, 55);
            this.btnWizard3Back.TabIndex = 4;
            this.btnWizard3Back.Text = "< Back";
            this.btnWizard3Back.UseVisualStyleBackColor = false;
            this.btnWizard3Back.Click += new System.EventHandler(this.btnWizard3Back_Click);
            // 
            // panelResumeList
            // 
            this.panelResumeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelResumeList.Controls.Add(this.lblResumeTitle);
            this.panelResumeList.Controls.Add(this.flpTaskList);
            this.panelResumeList.Controls.Add(this.btnResumeListBack);
            this.panelResumeList.Location = new System.Drawing.Point(15, 75);
            this.panelResumeList.Name = "panelResumeList";
            this.panelResumeList.Size = new System.Drawing.Size(966, 557);
            this.panelResumeList.TabIndex = 4;
            this.panelResumeList.Visible = false;
            // 
            // lblResumeTitle
            // 
            this.lblResumeTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(130)))), ((int)(((byte)(180)))));
            this.lblResumeTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblResumeTitle.ForeColor = System.Drawing.Color.White;
            this.lblResumeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblResumeTitle.Name = "lblResumeTitle";
            this.lblResumeTitle.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.lblResumeTitle.Size = new System.Drawing.Size(966, 55);
            this.lblResumeTitle.TabIndex = 0;
            this.lblResumeTitle.Text = "Select Task to Resume";
            this.lblResumeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpTaskList
            // 
            this.flpTaskList.AutoScroll = true;
            this.flpTaskList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.flpTaskList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpTaskList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpTaskList.Location = new System.Drawing.Point(30, 75);
            this.flpTaskList.Name = "flpTaskList";
            this.flpTaskList.Padding = new System.Windows.Forms.Padding(5);
            this.flpTaskList.Size = new System.Drawing.Size(906, 366);
            this.flpTaskList.TabIndex = 1;
            this.flpTaskList.WrapContents = false;
            // 
            // btnResumeListBack
            // 
            this.btnResumeListBack.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.btnResumeListBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResumeListBack.FlatAppearance.BorderSize = 0;
            this.btnResumeListBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeListBack.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.btnResumeListBack.ForeColor = System.Drawing.Color.White;
            this.btnResumeListBack.Location = new System.Drawing.Point(30, 460);
            this.btnResumeListBack.Name = "btnResumeListBack";
            this.btnResumeListBack.Size = new System.Drawing.Size(140, 55);
            this.btnResumeListBack.TabIndex = 2;
            this.btnResumeListBack.Text = "< Back";
            this.btnResumeListBack.UseVisualStyleBackColor = false;
            this.btnResumeListBack.Click += new System.EventHandler(this.btnResumeListBack_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.ForeColor = System.Drawing.Color.White;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(917, 676);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5, 0, 10, 0);
            this.btnCancel.Size = new System.Drawing.Size(64, 64);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnGoMode
            // 
            this.btnGoMode.BackColor = System.Drawing.Color.White;
            this.btnGoMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGoMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnGoMode.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnGoMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnGoMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.btnGoMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGoMode.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnGoMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnGoMode.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOn;
            this.btnGoMode.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGoMode.Location = new System.Drawing.Point(495, 112);
            this.btnGoMode.Margin = new System.Windows.Forms.Padding(12);
            this.btnGoMode.Name = "btnGoMode";
            this.btnGoMode.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnGoMode.Size = new System.Drawing.Size(459, 76);
            this.btnGoMode.TabIndex = 8;
            this.btnGoMode.Text = "  Go Mode";
            this.btnGoMode.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnGoMode.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGoMode.UseVisualStyleBackColor = false;
            // 
            // FormStartWork
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1000, 750);
            this.ControlBox = false;
            this.Controls.Add(this.panelContainer);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStartWork";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Start Work Session";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStartWork_FormClosing);
            this.Load += new System.EventHandler(this.FormStartWork_Load);
            this.panelContainer.ResumeLayout(false);
            this.panelHeader.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.tableTasks.ResumeLayout(false);
            this.tableFields.ResumeLayout(false);
            this.panelOpenField.ResumeLayout(false);
            this.tableLocalStorage.ResumeLayout(false);
            this.tableAgShareCloud.ResumeLayout(false);
            this.panelAddField.ResumeLayout(false);
            this.tableAddField.ResumeLayout(false);
            this.panelWizardStep1.ResumeLayout(false);
            this.panelWizardStep2.ResumeLayout(false);
            this.panelWizardStep3.ResumeLayout(false);
            this.panelWizardStep3.PerformLayout();
            this.panelResumeList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        // Container
        private System.Windows.Forms.Panel panelContainer;

        // Header for drag
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;

        // Main Menu
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TableLayoutPanel tableTasks;
        private System.Windows.Forms.Button btnResumeLastTask;
        private System.Windows.Forms.Button btnResumeTask;
        private System.Windows.Forms.Button btnNewTask;
        private System.Windows.Forms.Button btnCloseTask;
        private System.Windows.Forms.Button btnFinishTask;
        private System.Windows.Forms.Button btnExportTasks;
        private System.Windows.Forms.Label lblFieldSection;
        private System.Windows.Forms.TableLayoutPanel tableFields;
        private System.Windows.Forms.Button btnOpenField;
        private System.Windows.Forms.Button btnNewField;
        private System.Windows.Forms.Button btnCloseField;
        private System.Windows.Forms.Label lblLastTaskInfo;
        private System.Windows.Forms.Label lblCurrentField;

        // Open Field Panel
        private System.Windows.Forms.Panel panelOpenField;
        private System.Windows.Forms.Label lblLocalStorageTitle;
        private System.Windows.Forms.TableLayoutPanel tableLocalStorage;
        private System.Windows.Forms.Button btnOpenFieldLocal;
        private System.Windows.Forms.Label lblAgShareCloudTitle;
        private System.Windows.Forms.TableLayoutPanel tableAgShareCloud;
        private System.Windows.Forms.Button btnOpenFromAgShare;
        private System.Windows.Forms.Button btnUploadToAgShare;
        private System.Windows.Forms.Button btnOpenFieldBack;

        // Add Field Panel
        private System.Windows.Forms.Panel panelAddField;
        private System.Windows.Forms.Label lblAddFieldTitle;
        private System.Windows.Forms.TableLayoutPanel tableAddField;
        private System.Windows.Forms.Button btnAddFieldNew;
        private System.Windows.Forms.Button btnAddFieldFromISOXML;
        private System.Windows.Forms.Button btnAddFieldFromKML;
        private System.Windows.Forms.Button btnAddFieldBack;

        // Wizard Step 1
        private System.Windows.Forms.Panel panelWizardStep1;
        private System.Windows.Forms.Label lblStep1Title;
        private System.Windows.Forms.ListBox listProfiles;
        private System.Windows.Forms.Button btnWizard1Next;
        private System.Windows.Forms.Button btnWizard1Back;

        // Wizard Step 2
        private System.Windows.Forms.Panel panelWizardStep2;
        private System.Windows.Forms.Label lblStep2Title;
        private System.Windows.Forms.ListView lvFields;
        private System.Windows.Forms.ColumnHeader chFieldName;
        private System.Windows.Forms.ColumnHeader chFieldDistance;
        private System.Windows.Forms.ColumnHeader chFieldArea;
        private System.Windows.Forms.Button btnSortFields;
        private System.Windows.Forms.Button btnWizard2Next;
        private System.Windows.Forms.Button btnWizard2Back;
        private System.Windows.Forms.Button btnWizard2NewField;

        // Wizard Step 3
        private System.Windows.Forms.Panel panelWizardStep3;
        private System.Windows.Forms.Label lblStep3Title;
        private System.Windows.Forms.Label lblFieldNameLabel;
        private System.Windows.Forms.TextBox txtFieldName;
        private System.Windows.Forms.FlowLayoutPanel flpWorkTypes;
        private System.Windows.Forms.Label lblTaskNameLabel;
        private System.Windows.Forms.TextBox txtTaskName;
        private System.Windows.Forms.Button btnWizard3Start;
        private System.Windows.Forms.Button btnWizard3Back;

        // Resume Task List
        private System.Windows.Forms.Panel panelResumeList;
        private System.Windows.Forms.Label lblResumeTitle;
        private System.Windows.Forms.FlowLayoutPanel flpTaskList;
        private System.Windows.Forms.Button btnResumeListBack;

        // Cancel
        private System.Windows.Forms.Button btnCancel;
        private Button btnGoMode;
    }
}

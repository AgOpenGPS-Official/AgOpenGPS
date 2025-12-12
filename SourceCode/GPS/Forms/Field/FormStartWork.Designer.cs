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
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnResumeJob = new System.Windows.Forms.Button();
            this.btnNewJob = new System.Windows.Forms.Button();
            this.btnOpenFieldOnly = new System.Windows.Forms.Button();
            this.btnCloseField = new System.Windows.Forms.Button();
            this.btnCloseJob = new System.Windows.Forms.Button();
            this.btnFinishJob = new System.Windows.Forms.Button();
            this.lblLastJobInfo = new System.Windows.Forms.Label();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.panelAgShare = new System.Windows.Forms.Panel();
            this.btnAgShareDownload = new System.Windows.Forms.Button();
            this.btnAgShareUpload = new System.Windows.Forms.Button();
            this.panelWizardStep1 = new System.Windows.Forms.Panel();
            this.lblStep1Title = new System.Windows.Forms.Label();
            this.listProfiles = new System.Windows.Forms.ListBox();
            this.btnWizard1Next = new System.Windows.Forms.Button();
            this.btnWizard1Back = new System.Windows.Forms.Button();
            this.panelWizardStep2 = new System.Windows.Forms.Panel();
            this.lblStep2Title = new System.Windows.Forms.Label();
            this.listFields = new System.Windows.Forms.ListBox();
            this.btnWizard2Next = new System.Windows.Forms.Button();
            this.btnWizard2Back = new System.Windows.Forms.Button();
            this.panelWizardStep3 = new System.Windows.Forms.Panel();
            this.lblStep3Title = new System.Windows.Forms.Label();
            this.flpWorkTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblJobNameLabel = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.btnWizard3Start = new System.Windows.Forms.Button();
            this.btnWizard3Back = new System.Windows.Forms.Button();
            this.panelResumeList = new System.Windows.Forms.Panel();
            this.lblResumeTitle = new System.Windows.Forms.Label();
            this.flpJobList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnResumeListBack = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panelContainer.SuspendLayout();
            this.panelHeader.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.panelAgShare.SuspendLayout();
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
            this.panelMain.Controls.Add(this.tableMain);
            this.panelMain.Controls.Add(this.lblLastJobInfo);
            this.panelMain.Controls.Add(this.lblCurrentField);
            this.panelMain.Controls.Add(this.panelAgShare);
            this.panelMain.Location = new System.Drawing.Point(15, 75);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(966, 601);
            this.panelMain.TabIndex = 0;
            // 
            // tableMain
            // 
            this.tableMain.ColumnCount = 2;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.Controls.Add(this.btnResumeJob, 0, 0);
            this.tableMain.Controls.Add(this.btnNewJob, 1, 0);
            this.tableMain.Controls.Add(this.btnOpenFieldOnly, 0, 1);
            this.tableMain.Controls.Add(this.btnCloseField, 1, 1);
            this.tableMain.Controls.Add(this.btnCloseJob, 0, 2);
            this.tableMain.Controls.Add(this.btnFinishJob, 1, 2);
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 3;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.34F));
            this.tableMain.Size = new System.Drawing.Size(966, 330);
            this.tableMain.TabIndex = 0;
            // 
            // btnResumeJob
            // 
            this.btnResumeJob.BackColor = System.Drawing.Color.White;
            this.btnResumeJob.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnResumeJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResumeJob.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnResumeJob.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnResumeJob.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnResumeJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnResumeJob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnResumeJob.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnResumeJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeJob.Location = new System.Drawing.Point(12, 12);
            this.btnResumeJob.Margin = new System.Windows.Forms.Padding(12);
            this.btnResumeJob.Name = "btnResumeJob";
            this.btnResumeJob.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnResumeJob.Size = new System.Drawing.Size(459, 85);
            this.btnResumeJob.TabIndex = 0;
            this.btnResumeJob.Text = "  Resume Job";
            this.btnResumeJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResumeJob.UseVisualStyleBackColor = false;
            this.btnResumeJob.Click += new System.EventHandler(this.btnResumeJob_Click);
            // 
            // btnNewJob
            // 
            this.btnNewJob.BackColor = System.Drawing.Color.White;
            this.btnNewJob.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNewJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewJob.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnNewJob.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnNewJob.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnNewJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnNewJob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnNewJob.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnNewJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.Location = new System.Drawing.Point(495, 12);
            this.btnNewJob.Margin = new System.Windows.Forms.Padding(12);
            this.btnNewJob.Name = "btnNewJob";
            this.btnNewJob.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnNewJob.Size = new System.Drawing.Size(459, 85);
            this.btnNewJob.TabIndex = 1;
            this.btnNewJob.Text = "  New Job";
            this.btnNewJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewJob.UseVisualStyleBackColor = false;
            this.btnNewJob.Click += new System.EventHandler(this.btnNewJob_Click);
            // 
            // btnOpenFieldOnly
            // 
            this.btnOpenFieldOnly.BackColor = System.Drawing.Color.White;
            this.btnOpenFieldOnly.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFieldOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFieldOnly.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnOpenFieldOnly.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(235)))), ((int)(((byte)(250)))));
            this.btnOpenFieldOnly.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.btnOpenFieldOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFieldOnly.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnOpenFieldOnly.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnOpenFieldOnly.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenFieldOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldOnly.Location = new System.Drawing.Point(12, 121);
            this.btnOpenFieldOnly.Margin = new System.Windows.Forms.Padding(12);
            this.btnOpenFieldOnly.Name = "btnOpenFieldOnly";
            this.btnOpenFieldOnly.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnOpenFieldOnly.Size = new System.Drawing.Size(459, 85);
            this.btnOpenFieldOnly.TabIndex = 2;
            this.btnOpenFieldOnly.Text = "  Open Field";
            this.btnOpenFieldOnly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldOnly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFieldOnly.UseVisualStyleBackColor = false;
            this.btnOpenFieldOnly.Click += new System.EventHandler(this.btnOpenFieldOnly_Click);
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
            this.btnCloseField.Location = new System.Drawing.Point(495, 121);
            this.btnCloseField.Margin = new System.Windows.Forms.Padding(12);
            this.btnCloseField.Name = "btnCloseField";
            this.btnCloseField.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnCloseField.Size = new System.Drawing.Size(459, 85);
            this.btnCloseField.TabIndex = 3;
            this.btnCloseField.Text = "  Close Field";
            this.btnCloseField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseField.UseVisualStyleBackColor = false;
            this.btnCloseField.Click += new System.EventHandler(this.btnCloseField_Click);
            // 
            // btnCloseJob
            // 
            this.btnCloseJob.BackColor = System.Drawing.Color.White;
            this.btnCloseJob.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCloseJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseJob.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnCloseJob.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnCloseJob.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(248)))), ((int)(((byte)(230)))));
            this.btnCloseJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnCloseJob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnCloseJob.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnCloseJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseJob.Location = new System.Drawing.Point(12, 230);
            this.btnCloseJob.Margin = new System.Windows.Forms.Padding(12);
            this.btnCloseJob.Name = "btnCloseJob";
            this.btnCloseJob.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnCloseJob.Size = new System.Drawing.Size(459, 88);
            this.btnCloseJob.TabIndex = 4;
            this.btnCloseJob.Text = "  Close Job";
            this.btnCloseJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseJob.UseVisualStyleBackColor = false;
            this.btnCloseJob.Click += new System.EventHandler(this.btnCloseJob_Click);
            // 
            // btnFinishJob
            // 
            this.btnFinishJob.BackColor = System.Drawing.Color.White;
            this.btnFinishJob.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFinishJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnFinishJob.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.btnFinishJob.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(235)))), ((int)(((byte)(200)))));
            this.btnFinishJob.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(255)))), ((int)(((byte)(230)))));
            this.btnFinishJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFinishJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnFinishJob.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnFinishJob.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnFinishJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinishJob.Location = new System.Drawing.Point(495, 230);
            this.btnFinishJob.Margin = new System.Windows.Forms.Padding(12);
            this.btnFinishJob.Name = "btnFinishJob";
            this.btnFinishJob.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnFinishJob.Size = new System.Drawing.Size(459, 88);
            this.btnFinishJob.TabIndex = 5;
            this.btnFinishJob.Text = "  Finish Job";
            this.btnFinishJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinishJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFinishJob.UseVisualStyleBackColor = false;
            this.btnFinishJob.Click += new System.EventHandler(this.btnFinishJob_Click);
            // 
            // lblLastJobInfo
            // 
            this.lblLastJobInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.lblLastJobInfo.Font = new System.Drawing.Font("Tahoma", 13F);
            this.lblLastJobInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblLastJobInfo.Location = new System.Drawing.Point(0, 340);
            this.lblLastJobInfo.Name = "lblLastJobInfo";
            this.lblLastJobInfo.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.lblLastJobInfo.Size = new System.Drawing.Size(966, 60);
            this.lblLastJobInfo.TabIndex = 1;
            this.lblLastJobInfo.Text = "Last Job Info";
            this.lblLastJobInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCurrentField
            // 
            this.lblCurrentField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(139)))), ((int)(((byte)(87)))));
            this.lblCurrentField.Font = new System.Drawing.Font("Tahoma", 15F, System.Drawing.FontStyle.Bold);
            this.lblCurrentField.ForeColor = System.Drawing.Color.White;
            this.lblCurrentField.Location = new System.Drawing.Point(0, 410);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Padding = new System.Windows.Forms.Padding(10);
            this.lblCurrentField.Size = new System.Drawing.Size(966, 50);
            this.lblCurrentField.TabIndex = 2;
            this.lblCurrentField.Text = "Current: FieldName";
            this.lblCurrentField.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCurrentField.Visible = false;
            // 
            // panelAgShare
            // 
            this.panelAgShare.Controls.Add(this.btnAgShareDownload);
            this.panelAgShare.Controls.Add(this.btnAgShareUpload);
            this.panelAgShare.Location = new System.Drawing.Point(0, 470);
            this.panelAgShare.Name = "panelAgShare";
            this.panelAgShare.Size = new System.Drawing.Size(966, 88);
            this.panelAgShare.TabIndex = 3;
            // 
            // btnAgShareDownload
            // 
            this.btnAgShareDownload.BackColor = System.Drawing.Color.White;
            this.btnAgShareDownload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgShareDownload.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnAgShareDownload.FlatAppearance.BorderSize = 2;
            this.btnAgShareDownload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.btnAgShareDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgShareDownload.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnAgShareDownload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnAgShareDownload.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnAgShareDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareDownload.Location = new System.Drawing.Point(12, 5);
            this.btnAgShareDownload.Name = "btnAgShareDownload";
            this.btnAgShareDownload.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnAgShareDownload.Size = new System.Drawing.Size(459, 80);
            this.btnAgShareDownload.TabIndex = 0;
            this.btnAgShareDownload.Text = "  AgShare Download";
            this.btnAgShareDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgShareDownload.UseVisualStyleBackColor = false;
            this.btnAgShareDownload.Click += new System.EventHandler(this.btnAgShareDownload_Click);
            // 
            // btnAgShareUpload
            // 
            this.btnAgShareUpload.BackColor = System.Drawing.Color.White;
            this.btnAgShareUpload.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAgShareUpload.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnAgShareUpload.FlatAppearance.BorderSize = 2;
            this.btnAgShareUpload.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(244)))), ((int)(((byte)(255)))));
            this.btnAgShareUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgShareUpload.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnAgShareUpload.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnAgShareUpload.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnAgShareUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareUpload.Location = new System.Drawing.Point(495, 5);
            this.btnAgShareUpload.Name = "btnAgShareUpload";
            this.btnAgShareUpload.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnAgShareUpload.Size = new System.Drawing.Size(459, 80);
            this.btnAgShareUpload.TabIndex = 1;
            this.btnAgShareUpload.Text = "  AgShare Upload";
            this.btnAgShareUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgShareUpload.UseVisualStyleBackColor = false;
            this.btnAgShareUpload.Click += new System.EventHandler(this.btnAgShareUpload_Click);
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
            this.panelWizardStep2.Controls.Add(this.listFields);
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
            // listFields
            // 
            this.listFields.BackColor = System.Drawing.Color.White;
            this.listFields.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.listFields.Font = new System.Drawing.Font("Tahoma", 20F);
            this.listFields.FormattingEnabled = true;
            this.listFields.ItemHeight = 33;
            this.listFields.Location = new System.Drawing.Point(30, 75);
            this.listFields.Name = "listFields";
            this.listFields.Size = new System.Drawing.Size(906, 332);
            this.listFields.TabIndex = 1;
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
            this.panelWizardStep3.Controls.Add(this.flpWorkTypes);
            this.panelWizardStep3.Controls.Add(this.lblJobNameLabel);
            this.panelWizardStep3.Controls.Add(this.txtJobName);
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
            // lblJobNameLabel
            // 
            this.lblJobNameLabel.AutoSize = true;
            this.lblJobNameLabel.Font = new System.Drawing.Font("Tahoma", 13F, System.Drawing.FontStyle.Bold);
            this.lblJobNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblJobNameLabel.Location = new System.Drawing.Point(30, 375);
            this.lblJobNameLabel.Name = "lblJobNameLabel";
            this.lblJobNameLabel.Size = new System.Drawing.Size(106, 22);
            this.lblJobNameLabel.TabIndex = 2;
            this.lblJobNameLabel.Text = "Job Name:";
            // 
            // txtJobName
            // 
            this.txtJobName.BackColor = System.Drawing.Color.White;
            this.txtJobName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtJobName.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtJobName.Location = new System.Drawing.Point(30, 405);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(906, 33);
            this.txtJobName.TabIndex = 3;
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
            this.btnWizard3Start.Text = "START JOB";
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
            this.panelResumeList.Controls.Add(this.flpJobList);
            this.panelResumeList.Controls.Add(this.btnResumeListBack);
            this.panelResumeList.Location = new System.Drawing.Point(15, 75);
            this.panelResumeList.Name = "panelResumeList";
            this.panelResumeList.Size = new System.Drawing.Size(966, 595);
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
            this.lblResumeTitle.Text = "Select Job to Resume";
            this.lblResumeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // flpJobList
            // 
            this.flpJobList.AutoScroll = true;
            this.flpJobList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(249)))), ((int)(((byte)(250)))));
            this.flpJobList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.flpJobList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpJobList.Location = new System.Drawing.Point(30, 75);
            this.flpJobList.Name = "flpJobList";
            this.flpJobList.Padding = new System.Windows.Forms.Padding(5);
            this.flpJobList.Size = new System.Drawing.Size(906, 366);
            this.flpJobList.TabIndex = 1;
            this.flpJobList.WrapContents = false;
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
            this.btnCancel.Location = new System.Drawing.Point(920, 679);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(5, 0, 10, 0);
            this.btnCancel.Size = new System.Drawing.Size(64, 64);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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
            this.tableMain.ResumeLayout(false);
            this.panelAgShare.ResumeLayout(false);
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
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.Button btnNewJob;
        private System.Windows.Forms.Button btnResumeJob;
        private System.Windows.Forms.Button btnOpenFieldOnly;
        private System.Windows.Forms.Button btnCloseField;
        private System.Windows.Forms.Button btnCloseJob;
        private System.Windows.Forms.Button btnFinishJob;
        private System.Windows.Forms.Label lblLastJobInfo;
        private System.Windows.Forms.Label lblCurrentField;
        private System.Windows.Forms.Panel panelAgShare;
        private System.Windows.Forms.Button btnAgShareDownload;
        private System.Windows.Forms.Button btnAgShareUpload;

        // Wizard Step 1
        private System.Windows.Forms.Panel panelWizardStep1;
        private System.Windows.Forms.Label lblStep1Title;
        private System.Windows.Forms.ListBox listProfiles;
        private System.Windows.Forms.Button btnWizard1Next;
        private System.Windows.Forms.Button btnWizard1Back;

        // Wizard Step 2
        private System.Windows.Forms.Panel panelWizardStep2;
        private System.Windows.Forms.Label lblStep2Title;
        private System.Windows.Forms.ListBox listFields;
        private System.Windows.Forms.Button btnWizard2Next;
        private System.Windows.Forms.Button btnWizard2Back;

        // Wizard Step 3
        private System.Windows.Forms.Panel panelWizardStep3;
        private System.Windows.Forms.Label lblStep3Title;
        private System.Windows.Forms.FlowLayoutPanel flpWorkTypes;
        private System.Windows.Forms.Label lblJobNameLabel;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Button btnWizard3Start;
        private System.Windows.Forms.Button btnWizard3Back;

        // Resume Job List
        private System.Windows.Forms.Panel panelResumeList;
        private System.Windows.Forms.Label lblResumeTitle;
        private System.Windows.Forms.FlowLayoutPanel flpJobList;
        private System.Windows.Forms.Button btnResumeListBack;

        // Cancel
        private System.Windows.Forms.Button btnCancel;
    }
}

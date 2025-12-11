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
            // Main container
            this.panelContainer = new System.Windows.Forms.Panel();

            // Main Menu Panel
            this.panelMain = new System.Windows.Forms.Panel();
            this.tableMain = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewJob = new System.Windows.Forms.Button();
            this.btnResumeJob = new System.Windows.Forms.Button();
            this.btnOpenFieldOnly = new System.Windows.Forms.Button();
            this.btnCloseField = new System.Windows.Forms.Button();
            this.lblLastJobInfo = new System.Windows.Forms.Label();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.panelAgShare = new System.Windows.Forms.Panel();
            this.btnAgShareDownload = new System.Windows.Forms.Button();
            this.btnAgShareUpload = new System.Windows.Forms.Button();

            // Wizard Step 1 Panel
            this.panelWizardStep1 = new System.Windows.Forms.Panel();
            this.lblStep1Title = new System.Windows.Forms.Label();
            this.listProfiles = new System.Windows.Forms.ListBox();
            this.btnWizard1Next = new System.Windows.Forms.Button();
            this.btnWizard1Back = new System.Windows.Forms.Button();

            // Wizard Step 2 Panel
            this.panelWizardStep2 = new System.Windows.Forms.Panel();
            this.lblStep2Title = new System.Windows.Forms.Label();
            this.listFields = new System.Windows.Forms.ListBox();
            this.btnWizard2Next = new System.Windows.Forms.Button();
            this.btnWizard2Back = new System.Windows.Forms.Button();

            // Wizard Step 3 Panel
            this.panelWizardStep3 = new System.Windows.Forms.Panel();
            this.lblStep3Title = new System.Windows.Forms.Label();
            this.flpWorkTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.lblJobNameLabel = new System.Windows.Forms.Label();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.btnWizard3Start = new System.Windows.Forms.Button();
            this.btnWizard3Back = new System.Windows.Forms.Button();

            // Resume Job List Panel
            this.panelResumeList = new System.Windows.Forms.Panel();
            this.lblResumeTitle = new System.Windows.Forms.Label();
            this.flpJobList = new System.Windows.Forms.FlowLayoutPanel();
            this.btnResumeListBack = new System.Windows.Forms.Button();

            // Cancel button (always visible)
            this.btnCancel = new System.Windows.Forms.Button();

            this.panelContainer.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.tableMain.SuspendLayout();
            this.panelAgShare.SuspendLayout();
            this.panelWizardStep1.SuspendLayout();
            this.panelWizardStep2.SuspendLayout();
            this.panelWizardStep3.SuspendLayout();
            this.panelResumeList.SuspendLayout();
            this.SuspendLayout();

            // =====================================================
            // panelContainer - holds all content panels
            // =====================================================
            this.panelContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelContainer.Controls.Add(this.panelMain);
            this.panelContainer.Controls.Add(this.panelWizardStep1);
            this.panelContainer.Controls.Add(this.panelWizardStep2);
            this.panelContainer.Controls.Add(this.panelWizardStep3);
            this.panelContainer.Controls.Add(this.panelResumeList);
            this.panelContainer.Controls.Add(this.btnCancel);
            this.panelContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContainer.Location = new System.Drawing.Point(0, 0);
            this.panelContainer.Name = "panelContainer";
            this.panelContainer.Padding = new System.Windows.Forms.Padding(15);
            this.panelContainer.Size = new System.Drawing.Size(1000, 700);
            this.panelContainer.TabIndex = 0;

            // =====================================================
            // MAIN MENU PANEL
            // =====================================================
            this.panelMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelMain.Controls.Add(this.tableMain);
            this.panelMain.Controls.Add(this.lblLastJobInfo);
            this.panelMain.Controls.Add(this.lblCurrentField);
            this.panelMain.Controls.Add(this.panelAgShare);
            this.panelMain.Location = new System.Drawing.Point(15, 15);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(970, 600);
            this.panelMain.TabIndex = 0;

            // tableMain - 2x2 grid for main buttons
            this.tableMain.ColumnCount = 2;
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.Controls.Add(this.btnResumeJob, 0, 0);
            this.tableMain.Controls.Add(this.btnNewJob, 1, 0);
            this.tableMain.Controls.Add(this.btnOpenFieldOnly, 0, 1);
            this.tableMain.Controls.Add(this.btnCloseField, 1, 1);
            this.tableMain.Location = new System.Drawing.Point(0, 0);
            this.tableMain.Name = "tableMain";
            this.tableMain.RowCount = 2;
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableMain.Size = new System.Drawing.Size(970, 280);
            this.tableMain.TabIndex = 0;

            // btnResumeJob
            this.btnResumeJob.BackColor = System.Drawing.Color.Transparent;
            this.btnResumeJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResumeJob.FlatAppearance.BorderSize = 0;
            this.btnResumeJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeJob.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.btnResumeJob.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnResumeJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeJob.Location = new System.Drawing.Point(8, 8);
            this.btnResumeJob.Margin = new System.Windows.Forms.Padding(8);
            this.btnResumeJob.Name = "btnResumeJob";
            this.btnResumeJob.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnResumeJob.Size = new System.Drawing.Size(469, 124);
            this.btnResumeJob.TabIndex = 0;
            this.btnResumeJob.Text = "Resume Job";
            this.btnResumeJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResumeJob.UseVisualStyleBackColor = false;
            this.btnResumeJob.Click += new System.EventHandler(this.btnResumeJob_Click);

            // btnNewJob
            this.btnNewJob.BackColor = System.Drawing.Color.Transparent;
            this.btnNewJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewJob.FlatAppearance.BorderSize = 0;
            this.btnNewJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewJob.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.btnNewJob.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnNewJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.Location = new System.Drawing.Point(493, 8);
            this.btnNewJob.Margin = new System.Windows.Forms.Padding(8);
            this.btnNewJob.Name = "btnNewJob";
            this.btnNewJob.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnNewJob.Size = new System.Drawing.Size(469, 124);
            this.btnNewJob.TabIndex = 1;
            this.btnNewJob.Text = "New Job";
            this.btnNewJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewJob.UseVisualStyleBackColor = false;
            this.btnNewJob.Click += new System.EventHandler(this.btnNewJob_Click);

            // btnOpenFieldOnly
            this.btnOpenFieldOnly.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFieldOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFieldOnly.FlatAppearance.BorderSize = 0;
            this.btnOpenFieldOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFieldOnly.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.btnOpenFieldOnly.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenFieldOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldOnly.Location = new System.Drawing.Point(8, 148);
            this.btnOpenFieldOnly.Margin = new System.Windows.Forms.Padding(8);
            this.btnOpenFieldOnly.Name = "btnOpenFieldOnly";
            this.btnOpenFieldOnly.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnOpenFieldOnly.Size = new System.Drawing.Size(469, 124);
            this.btnOpenFieldOnly.TabIndex = 2;
            this.btnOpenFieldOnly.Text = "Open Field";
            this.btnOpenFieldOnly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldOnly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFieldOnly.UseVisualStyleBackColor = false;
            this.btnOpenFieldOnly.Click += new System.EventHandler(this.btnOpenFieldOnly_Click);

            // btnCloseField
            this.btnCloseField.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseField.FlatAppearance.BorderSize = 0;
            this.btnCloseField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseField.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.btnCloseField.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnCloseField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.Location = new System.Drawing.Point(493, 148);
            this.btnCloseField.Margin = new System.Windows.Forms.Padding(8);
            this.btnCloseField.Name = "btnCloseField";
            this.btnCloseField.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnCloseField.Size = new System.Drawing.Size(469, 124);
            this.btnCloseField.TabIndex = 3;
            this.btnCloseField.Text = "Close Field";
            this.btnCloseField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseField.UseVisualStyleBackColor = false;
            this.btnCloseField.Click += new System.EventHandler(this.btnCloseField_Click);

            // lblLastJobInfo
            this.lblLastJobInfo.Font = new System.Drawing.Font("Tahoma", 14F);
            this.lblLastJobInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblLastJobInfo.Location = new System.Drawing.Point(0, 290);
            this.lblLastJobInfo.Name = "lblLastJobInfo";
            this.lblLastJobInfo.Size = new System.Drawing.Size(970, 60);
            this.lblLastJobInfo.TabIndex = 1;
            this.lblLastJobInfo.Text = "Last Job Info";
            this.lblLastJobInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // lblCurrentField
            this.lblCurrentField.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.lblCurrentField.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.lblCurrentField.ForeColor = System.Drawing.Color.White;
            this.lblCurrentField.Location = new System.Drawing.Point(0, 360);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Size = new System.Drawing.Size(970, 45);
            this.lblCurrentField.TabIndex = 2;
            this.lblCurrentField.Text = "Current: FieldName";
            this.lblCurrentField.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCurrentField.Visible = false;

            // panelAgShare
            this.panelAgShare.Controls.Add(this.btnAgShareDownload);
            this.panelAgShare.Controls.Add(this.btnAgShareUpload);
            this.panelAgShare.Location = new System.Drawing.Point(0, 420);
            this.panelAgShare.Name = "panelAgShare";
            this.panelAgShare.Size = new System.Drawing.Size(970, 100);
            this.panelAgShare.TabIndex = 3;

            // btnAgShareDownload
            this.btnAgShareDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnAgShareDownload.FlatAppearance.BorderSize = 0;
            this.btnAgShareDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgShareDownload.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.btnAgShareDownload.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnAgShareDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareDownload.Location = new System.Drawing.Point(0, 0);
            this.btnAgShareDownload.Name = "btnAgShareDownload";
            this.btnAgShareDownload.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAgShareDownload.Size = new System.Drawing.Size(480, 100);
            this.btnAgShareDownload.TabIndex = 0;
            this.btnAgShareDownload.Text = "AgShare Download";
            this.btnAgShareDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgShareDownload.UseVisualStyleBackColor = false;
            this.btnAgShareDownload.Click += new System.EventHandler(this.btnAgShareDownload_Click);

            // btnAgShareUpload
            this.btnAgShareUpload.BackColor = System.Drawing.Color.Transparent;
            this.btnAgShareUpload.FlatAppearance.BorderSize = 0;
            this.btnAgShareUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgShareUpload.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.btnAgShareUpload.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnAgShareUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareUpload.Location = new System.Drawing.Point(490, 0);
            this.btnAgShareUpload.Name = "btnAgShareUpload";
            this.btnAgShareUpload.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.btnAgShareUpload.Size = new System.Drawing.Size(480, 100);
            this.btnAgShareUpload.TabIndex = 1;
            this.btnAgShareUpload.Text = "AgShare Upload";
            this.btnAgShareUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgShareUpload.UseVisualStyleBackColor = false;
            this.btnAgShareUpload.Click += new System.EventHandler(this.btnAgShareUpload_Click);

            // =====================================================
            // WIZARD STEP 1 - Profile Selection
            // =====================================================
            this.panelWizardStep1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizardStep1.Controls.Add(this.lblStep1Title);
            this.panelWizardStep1.Controls.Add(this.listProfiles);
            this.panelWizardStep1.Controls.Add(this.btnWizard1Next);
            this.panelWizardStep1.Controls.Add(this.btnWizard1Back);
            this.panelWizardStep1.Location = new System.Drawing.Point(15, 15);
            this.panelWizardStep1.Name = "panelWizardStep1";
            this.panelWizardStep1.Size = new System.Drawing.Size(970, 600);
            this.panelWizardStep1.TabIndex = 1;
            this.panelWizardStep1.Visible = false;

            // lblStep1Title
            this.lblStep1Title.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.lblStep1Title.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.lblStep1Title.ForeColor = System.Drawing.Color.White;
            this.lblStep1Title.Location = new System.Drawing.Point(0, 0);
            this.lblStep1Title.Name = "lblStep1Title";
            this.lblStep1Title.Size = new System.Drawing.Size(970, 60);
            this.lblStep1Title.TabIndex = 0;
            this.lblStep1Title.Text = "Step 1: Select Machine Profile";
            this.lblStep1Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // listProfiles
            this.listProfiles.Font = new System.Drawing.Font("Tahoma", 18F);
            this.listProfiles.FormattingEnabled = true;
            this.listProfiles.ItemHeight = 29;
            this.listProfiles.Location = new System.Drawing.Point(50, 80);
            this.listProfiles.Name = "listProfiles";
            this.listProfiles.Size = new System.Drawing.Size(870, 410);
            this.listProfiles.TabIndex = 1;

            // btnWizard1Back
            this.btnWizard1Back.BackColor = System.Drawing.Color.LightGray;
            this.btnWizard1Back.FlatAppearance.BorderSize = 0;
            this.btnWizard1Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard1Back.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWizard1Back.Location = new System.Drawing.Point(50, 510);
            this.btnWizard1Back.Name = "btnWizard1Back";
            this.btnWizard1Back.Size = new System.Drawing.Size(150, 60);
            this.btnWizard1Back.TabIndex = 2;
            this.btnWizard1Back.Text = "< Cancel";
            this.btnWizard1Back.UseVisualStyleBackColor = false;
            this.btnWizard1Back.Click += new System.EventHandler(this.btnWizard1Back_Click);

            // btnWizard1Next
            this.btnWizard1Next.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.btnWizard1Next.FlatAppearance.BorderSize = 0;
            this.btnWizard1Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard1Next.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWizard1Next.ForeColor = System.Drawing.Color.White;
            this.btnWizard1Next.Location = new System.Drawing.Point(770, 510);
            this.btnWizard1Next.Name = "btnWizard1Next";
            this.btnWizard1Next.Size = new System.Drawing.Size(150, 60);
            this.btnWizard1Next.TabIndex = 3;
            this.btnWizard1Next.Text = "Next >";
            this.btnWizard1Next.UseVisualStyleBackColor = false;
            this.btnWizard1Next.Click += new System.EventHandler(this.btnWizard1Next_Click);

            // =====================================================
            // WIZARD STEP 2 - Field Selection
            // =====================================================
            this.panelWizardStep2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizardStep2.Controls.Add(this.lblStep2Title);
            this.panelWizardStep2.Controls.Add(this.listFields);
            this.panelWizardStep2.Controls.Add(this.btnWizard2Next);
            this.panelWizardStep2.Controls.Add(this.btnWizard2Back);
            this.panelWizardStep2.Location = new System.Drawing.Point(15, 15);
            this.panelWizardStep2.Name = "panelWizardStep2";
            this.panelWizardStep2.Size = new System.Drawing.Size(970, 600);
            this.panelWizardStep2.TabIndex = 2;
            this.panelWizardStep2.Visible = false;

            // lblStep2Title
            this.lblStep2Title.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.lblStep2Title.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.lblStep2Title.ForeColor = System.Drawing.Color.White;
            this.lblStep2Title.Location = new System.Drawing.Point(0, 0);
            this.lblStep2Title.Name = "lblStep2Title";
            this.lblStep2Title.Size = new System.Drawing.Size(970, 60);
            this.lblStep2Title.TabIndex = 0;
            this.lblStep2Title.Text = "Step 2: Select Field";
            this.lblStep2Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // listFields
            this.listFields.Font = new System.Drawing.Font("Tahoma", 18F);
            this.listFields.FormattingEnabled = true;
            this.listFields.ItemHeight = 29;
            this.listFields.Location = new System.Drawing.Point(50, 80);
            this.listFields.Name = "listFields";
            this.listFields.Size = new System.Drawing.Size(870, 410);
            this.listFields.TabIndex = 1;

            // btnWizard2Back
            this.btnWizard2Back.BackColor = System.Drawing.Color.LightGray;
            this.btnWizard2Back.FlatAppearance.BorderSize = 0;
            this.btnWizard2Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard2Back.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWizard2Back.Location = new System.Drawing.Point(50, 510);
            this.btnWizard2Back.Name = "btnWizard2Back";
            this.btnWizard2Back.Size = new System.Drawing.Size(150, 60);
            this.btnWizard2Back.TabIndex = 2;
            this.btnWizard2Back.Text = "< Back";
            this.btnWizard2Back.UseVisualStyleBackColor = false;
            this.btnWizard2Back.Click += new System.EventHandler(this.btnWizard2Back_Click);

            // btnWizard2Next
            this.btnWizard2Next.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.btnWizard2Next.FlatAppearance.BorderSize = 0;
            this.btnWizard2Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard2Next.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWizard2Next.ForeColor = System.Drawing.Color.White;
            this.btnWizard2Next.Location = new System.Drawing.Point(770, 510);
            this.btnWizard2Next.Name = "btnWizard2Next";
            this.btnWizard2Next.Size = new System.Drawing.Size(150, 60);
            this.btnWizard2Next.TabIndex = 3;
            this.btnWizard2Next.Text = "Next >";
            this.btnWizard2Next.UseVisualStyleBackColor = false;
            this.btnWizard2Next.Click += new System.EventHandler(this.btnWizard2Next_Click);

            // =====================================================
            // WIZARD STEP 3 - Work Type & Job Name
            // =====================================================
            this.panelWizardStep3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelWizardStep3.Controls.Add(this.lblStep3Title);
            this.panelWizardStep3.Controls.Add(this.flpWorkTypes);
            this.panelWizardStep3.Controls.Add(this.lblJobNameLabel);
            this.panelWizardStep3.Controls.Add(this.txtJobName);
            this.panelWizardStep3.Controls.Add(this.btnWizard3Start);
            this.panelWizardStep3.Controls.Add(this.btnWizard3Back);
            this.panelWizardStep3.Location = new System.Drawing.Point(15, 15);
            this.panelWizardStep3.Name = "panelWizardStep3";
            this.panelWizardStep3.Size = new System.Drawing.Size(970, 600);
            this.panelWizardStep3.TabIndex = 3;
            this.panelWizardStep3.Visible = false;

            // lblStep3Title
            this.lblStep3Title.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.lblStep3Title.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.lblStep3Title.ForeColor = System.Drawing.Color.White;
            this.lblStep3Title.Location = new System.Drawing.Point(0, 0);
            this.lblStep3Title.Name = "lblStep3Title";
            this.lblStep3Title.Size = new System.Drawing.Size(970, 60);
            this.lblStep3Title.TabIndex = 0;
            this.lblStep3Title.Text = "Step 3: Select Work Type";
            this.lblStep3Title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // flpWorkTypes
            this.flpWorkTypes.AutoScroll = true;
            this.flpWorkTypes.BackColor = System.Drawing.Color.White;
            this.flpWorkTypes.Location = new System.Drawing.Point(50, 80);
            this.flpWorkTypes.Name = "flpWorkTypes";
            this.flpWorkTypes.Size = new System.Drawing.Size(870, 320);
            this.flpWorkTypes.TabIndex = 1;

            // lblJobNameLabel
            this.lblJobNameLabel.AutoSize = true;
            this.lblJobNameLabel.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblJobNameLabel.Location = new System.Drawing.Point(50, 420);
            this.lblJobNameLabel.Name = "lblJobNameLabel";
            this.lblJobNameLabel.Size = new System.Drawing.Size(105, 23);
            this.lblJobNameLabel.TabIndex = 2;
            this.lblJobNameLabel.Text = "Job Name:";

            // txtJobName
            this.txtJobName.Font = new System.Drawing.Font("Tahoma", 18F);
            this.txtJobName.Location = new System.Drawing.Point(50, 450);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(870, 36);
            this.txtJobName.TabIndex = 3;

            // btnWizard3Back
            this.btnWizard3Back.BackColor = System.Drawing.Color.LightGray;
            this.btnWizard3Back.FlatAppearance.BorderSize = 0;
            this.btnWizard3Back.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard3Back.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnWizard3Back.Location = new System.Drawing.Point(50, 510);
            this.btnWizard3Back.Name = "btnWizard3Back";
            this.btnWizard3Back.Size = new System.Drawing.Size(150, 60);
            this.btnWizard3Back.TabIndex = 4;
            this.btnWizard3Back.Text = "< Back";
            this.btnWizard3Back.UseVisualStyleBackColor = false;
            this.btnWizard3Back.Click += new System.EventHandler(this.btnWizard3Back_Click);

            // btnWizard3Start
            this.btnWizard3Start.BackColor = System.Drawing.Color.Green;
            this.btnWizard3Start.FlatAppearance.BorderSize = 0;
            this.btnWizard3Start.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWizard3Start.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.btnWizard3Start.ForeColor = System.Drawing.Color.White;
            this.btnWizard3Start.Location = new System.Drawing.Point(720, 510);
            this.btnWizard3Start.Name = "btnWizard3Start";
            this.btnWizard3Start.Size = new System.Drawing.Size(200, 60);
            this.btnWizard3Start.TabIndex = 5;
            this.btnWizard3Start.Text = "START JOB";
            this.btnWizard3Start.UseVisualStyleBackColor = false;
            this.btnWizard3Start.Click += new System.EventHandler(this.btnWizard3Start_Click);

            // =====================================================
            // RESUME JOB LIST PANEL
            // =====================================================
            this.panelResumeList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
            this.panelResumeList.Controls.Add(this.lblResumeTitle);
            this.panelResumeList.Controls.Add(this.flpJobList);
            this.panelResumeList.Controls.Add(this.btnResumeListBack);
            this.panelResumeList.Location = new System.Drawing.Point(15, 15);
            this.panelResumeList.Name = "panelResumeList";
            this.panelResumeList.Size = new System.Drawing.Size(970, 600);
            this.panelResumeList.TabIndex = 4;
            this.panelResumeList.Visible = false;

            // lblResumeTitle
            this.lblResumeTitle.BackColor = System.Drawing.Color.FromArgb(0, 119, 190);
            this.lblResumeTitle.Font = new System.Drawing.Font("Tahoma", 20F, System.Drawing.FontStyle.Bold);
            this.lblResumeTitle.ForeColor = System.Drawing.Color.White;
            this.lblResumeTitle.Location = new System.Drawing.Point(0, 0);
            this.lblResumeTitle.Name = "lblResumeTitle";
            this.lblResumeTitle.Size = new System.Drawing.Size(970, 60);
            this.lblResumeTitle.TabIndex = 0;
            this.lblResumeTitle.Text = "Select Job to Resume";
            this.lblResumeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // flpJobList
            this.flpJobList.AutoScroll = true;
            this.flpJobList.BackColor = System.Drawing.Color.LightGray;
            this.flpJobList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flpJobList.Location = new System.Drawing.Point(50, 80);
            this.flpJobList.Name = "flpJobList";
            this.flpJobList.Size = new System.Drawing.Size(870, 410);
            this.flpJobList.TabIndex = 1;
            this.flpJobList.WrapContents = false;

            // btnResumeListBack
            this.btnResumeListBack.BackColor = System.Drawing.Color.LightGray;
            this.btnResumeListBack.FlatAppearance.BorderSize = 0;
            this.btnResumeListBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeListBack.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnResumeListBack.Location = new System.Drawing.Point(50, 510);
            this.btnResumeListBack.Name = "btnResumeListBack";
            this.btnResumeListBack.Size = new System.Drawing.Size(150, 60);
            this.btnResumeListBack.TabIndex = 2;
            this.btnResumeListBack.Text = "< Back";
            this.btnResumeListBack.UseVisualStyleBackColor = false;
            this.btnResumeListBack.Click += new System.EventHandler(this.btnResumeListBack_Click);

            // =====================================================
            // CANCEL BUTTON (always visible)
            // =====================================================
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(870, 625);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(115, 60);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);

            // =====================================================
            // FORM SETTINGS
            // =====================================================
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.ControlBox = false;
            this.Controls.Add(this.panelContainer);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormStartWork";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Start Work Session";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormStartWork_FormClosing);
            this.Load += new System.EventHandler(this.FormStartWork_Load);
            this.panelContainer.ResumeLayout(false);
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

        // Main Menu
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TableLayoutPanel tableMain;
        private System.Windows.Forms.Button btnNewJob;
        private System.Windows.Forms.Button btnResumeJob;
        private System.Windows.Forms.Button btnOpenFieldOnly;
        private System.Windows.Forms.Button btnCloseField;
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

namespace AgOpenGPS.Forms.Field
{
    partial class FormAgShareDownloader
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbFields = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.glControl1 = new OpenTK.GLControl.GLControl();
            this.lblSelectedField = new System.Windows.Forms.Label();
            this.progressBarDownloadAll = new System.Windows.Forms.ProgressBar();
            this.chkForceOverwrite = new System.Windows.Forms.CheckBox();
            this.btnOpen = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSaveAll = new System.Windows.Forms.Button();
            this.lblDownloading = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbFields
            // 
            this.lbFields.BackColor = System.Drawing.Color.LightGreen;
            this.lbFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lbFields.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFields.FullRowSelect = true;
            this.lbFields.GridLines = true;
            this.lbFields.HideSelection = false;
            this.lbFields.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lbFields.Location = new System.Drawing.Point(12, 12);
            this.lbFields.MultiSelect = false;
            this.lbFields.Name = "lbFields";
            this.lbFields.Size = new System.Drawing.Size(492, 567);
            this.lbFields.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lbFields.TabIndex = 1;
            this.lbFields.UseCompatibleStateImageBehavior = false;
            this.lbFields.View = System.Windows.Forms.View.Details;
            this.lbFields.SelectedIndexChanged += new System.EventHandler(this.lbFields_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Field";
            this.chName.Width = 480;
            // 
            // glControl1
            // 
            this.glControl1.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.glControl1.APIVersion = new System.Version(3, 3, 0, 0);
            this.glControl1.BackColor = System.Drawing.Color.DarkGray;
            this.glControl1.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.glControl1.IsEventDriven = true;
            this.glControl1.Location = new System.Drawing.Point(510, 12);
            this.glControl1.Name = "glControl1";
            this.glControl1.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.glControl1.SharedContext = null;
            this.glControl1.Size = new System.Drawing.Size(654, 567);
            this.glControl1.TabIndex = 3;
            // 
            // lblSelectedField
            // 
            this.lblSelectedField.AutoSize = true;
            this.lblSelectedField.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSelectedField.Location = new System.Drawing.Point(12, 592);
            this.lblSelectedField.Name = "lblSelectedField";
            this.lblSelectedField.Size = new System.Drawing.Size(170, 25);
            this.lblSelectedField.TabIndex = 5;
            this.lblSelectedField.Text = "Selected Field:";
            // 
            // progressBarDownloadAll
            // 
            this.progressBarDownloadAll.Location = new System.Drawing.Point(614, 628);
            this.progressBarDownloadAll.Name = "progressBarDownloadAll";
            this.progressBarDownloadAll.Size = new System.Drawing.Size(387, 23);
            this.progressBarDownloadAll.TabIndex = 7;
            // 
            // chkForceOverwrite
            // 
            this.chkForceOverwrite.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkForceOverwrite.BackColor = System.Drawing.Color.Transparent;
            this.chkForceOverwrite.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.chkForceOverwrite.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkForceOverwrite.Cursor = System.Windows.Forms.Cursors.Default;
            this.chkForceOverwrite.FlatAppearance.BorderSize = 0;
            this.chkForceOverwrite.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightSalmon;
            this.chkForceOverwrite.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkForceOverwrite.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkForceOverwrite.Image = global::AgOpenGPS.Properties.Resources.ForceOverwrite;
            this.chkForceOverwrite.Location = new System.Drawing.Point(458, 595);
            this.chkForceOverwrite.Name = "chkForceOverwrite";
            this.chkForceOverwrite.Size = new System.Drawing.Size(64, 64);
            this.chkForceOverwrite.TabIndex = 8;
            this.chkForceOverwrite.UseVisualStyleBackColor = false;
            // 
            // btnOpen
            // 
            this.btnOpen.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnOpen.FlatAppearance.BorderSize = 0;
            this.btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpen.Image = global::AgOpenGPS.Properties.Resources.DownloadAndUse;
            this.btnOpen.Location = new System.Drawing.Point(1030, 598);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(64, 64);
            this.btnOpen.TabIndex = 6;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnClose.Location = new System.Drawing.Point(1100, 598);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 64);
            this.btnClose.TabIndex = 4;
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSaveAll
            // 
            this.btnSaveAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSaveAll.FlatAppearance.BorderSize = 0;
            this.btnSaveAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveAll.Image = global::AgOpenGPS.Properties.Resources.DownloadAll;
            this.btnSaveAll.Location = new System.Drawing.Point(544, 592);
            this.btnSaveAll.Name = "btnSaveAll";
            this.btnSaveAll.Size = new System.Drawing.Size(64, 64);
            this.btnSaveAll.TabIndex = 2;
            this.btnSaveAll.UseVisualStyleBackColor = true;
            this.btnSaveAll.Click += new System.EventHandler(this.BtnDownloadAll_Click);
            // 
            // lblDownloading
            // 
            this.lblDownloading.AutoSize = true;
            this.lblDownloading.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDownloading.Location = new System.Drawing.Point(617, 592);
            this.lblDownloading.Name = "lblDownloading";
            this.lblDownloading.Size = new System.Drawing.Size(294, 25);
            this.lblDownloading.TabIndex = 9;
            this.lblDownloading.Text = "Downloading...Please Wait";
            // 
            // FormAgShareDownloader
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AutoValidate = System.Windows.Forms.AutoValidate.Disable;
            this.ClientSize = new System.Drawing.Size(1161, 663);
            this.ControlBox = false;
            this.Controls.Add(this.lblDownloading);
            this.Controls.Add(this.chkForceOverwrite);
            this.Controls.Add(this.progressBarDownloadAll);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.lblSelectedField);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.glControl1);
            this.Controls.Add(this.btnSaveAll);
            this.Controls.Add(this.lbFields);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormAgShareDownloader";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Download Fields From AgShare";
            this.Load += new System.EventHandler(this.FormAgShareDownloader_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lbFields;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Button btnSaveAll;
        private OpenTK.GLControl.GLControl glControl1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblSelectedField;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ProgressBar progressBarDownloadAll;
        private System.Windows.Forms.CheckBox chkForceOverwrite;
        private System.Windows.Forms.Label lblDownloading;
    }
}
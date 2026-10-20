namespace AgIO
{
    partial class FormNtripProfiles
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
            this.lblProfilesInfo = new System.Windows.Forms.Label();
            this.lstProfiles = new System.Windows.Forms.ListBox();
            this.lblProfileDetails = new System.Windows.Forms.Label();
            this.btnProfNew = new AgIO.RoundedButton();
            this.btnProfCopy = new AgIO.RoundedButton();
            this.btnProfRename = new AgIO.RoundedButton();
            this.btnProfDelete = new AgIO.RoundedButton();
            this.btnProfCancel = new AgIO.RoundedButton();
            this.btnProfEdit = new AgIO.RoundedButton();
            this.btnProfUse = new AgIO.RoundedButton();
            this.SuspendLayout();
            // 
            // lblProfilesInfo
            // 
            this.lblProfilesInfo.AutoSize = false;
            this.lblProfilesInfo.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfilesInfo.Location = new System.Drawing.Point(20, 12);
            this.lblProfilesInfo.Name = "lblProfilesInfo";
            this.lblProfilesInfo.Size = new System.Drawing.Size(860, 34);
            this.lblProfilesInfo.TabIndex = 0;
            this.lblProfilesInfo.Text = "Choose an NTRIP profile";
            // 
            // lstProfiles
            // 
            this.lstProfiles.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstProfiles.FormattingEnabled = true;
            this.lstProfiles.IntegralHeight = false;
            this.lstProfiles.ItemHeight = 29;
            this.lstProfiles.Location = new System.Drawing.Point(20, 52);
            this.lstProfiles.Name = "lstProfiles";
            this.lstProfiles.Size = new System.Drawing.Size(400, 430);
            this.lstProfiles.TabIndex = 1;
            this.lstProfiles.SelectedIndexChanged += new System.EventHandler(this.lstProfiles_SelectedIndexChanged);
            this.lstProfiles.DoubleClick += new System.EventHandler(this.lstProfiles_DoubleClick);
            // 
            // lblProfileDetails
            // 
            this.lblProfileDetails.AutoSize = false;
            this.lblProfileDetails.BackColor = System.Drawing.Color.White;
            this.lblProfileDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblProfileDetails.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfileDetails.Padding = new System.Windows.Forms.Padding(8);
            this.lblProfileDetails.Location = new System.Drawing.Point(440, 52);
            this.lblProfileDetails.Name = "lblProfileDetails";
            this.lblProfileDetails.Size = new System.Drawing.Size(440, 226);
            this.lblProfileDetails.TabIndex = 2;
            this.lblProfileDetails.Text = "";
            // 
            // btnProfNew
            // 
            this.btnProfNew.Location = new System.Drawing.Point(440, 294);
            this.btnProfNew.Name = "btnProfNew";
            this.btnProfNew.Size = new System.Drawing.Size(210, 60);
            this.btnProfNew.TabIndex = 3;
            this.btnProfNew.Text = "New profile";
            this.btnProfNew.Click += new System.EventHandler(this.btnProfNew_Click);
            // 
            // btnProfCopy
            // 
            this.btnProfCopy.Location = new System.Drawing.Point(670, 294);
            this.btnProfCopy.Name = "btnProfCopy";
            this.btnProfCopy.Size = new System.Drawing.Size(210, 60);
            this.btnProfCopy.TabIndex = 4;
            this.btnProfCopy.Text = "Copy";
            this.btnProfCopy.Click += new System.EventHandler(this.btnProfCopy_Click);
            // 
            // btnProfRename
            // 
            this.btnProfRename.Location = new System.Drawing.Point(440, 364);
            this.btnProfRename.Name = "btnProfRename";
            this.btnProfRename.Size = new System.Drawing.Size(210, 60);
            this.btnProfRename.TabIndex = 5;
            this.btnProfRename.Text = "Rename";
            this.btnProfRename.Click += new System.EventHandler(this.btnProfRename_Click);
            // 
            // btnProfDelete
            // 
            this.btnProfDelete.Tone = AgIO.ButtonTone.Negative;
            this.btnProfDelete.Location = new System.Drawing.Point(670, 364);
            this.btnProfDelete.Name = "btnProfDelete";
            this.btnProfDelete.Size = new System.Drawing.Size(210, 60);
            this.btnProfDelete.TabIndex = 6;
            this.btnProfDelete.Text = "Delete";
            this.btnProfDelete.Click += new System.EventHandler(this.btnProfDelete_Click);
            // 
            // btnProfCancel
            // 
            this.btnProfCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnProfCancel.Location = new System.Drawing.Point(440, 490);
            this.btnProfCancel.Name = "btnProfCancel";
            this.btnProfCancel.Size = new System.Drawing.Size(130, 70);
            this.btnProfCancel.TabIndex = 7;
            this.btnProfCancel.Text = "Cancel";
            // 
            // btnProfEdit
            // 
            this.btnProfEdit.Location = new System.Drawing.Point(580, 490);
            this.btnProfEdit.Name = "btnProfEdit";
            this.btnProfEdit.Size = new System.Drawing.Size(140, 70);
            this.btnProfEdit.TabIndex = 8;
            this.btnProfEdit.Text = "Edit";
            this.btnProfEdit.Click += new System.EventHandler(this.btnProfEdit_Click);
            // 
            // btnProfUse
            // 
            this.btnProfUse.Tone = AgIO.ButtonTone.Positive;
            this.btnProfUse.Location = new System.Drawing.Point(730, 490);
            this.btnProfUse.Name = "btnProfUse";
            this.btnProfUse.Size = new System.Drawing.Size(150, 70);
            this.btnProfUse.TabIndex = 9;
            this.btnProfUse.Text = "Use";
            this.btnProfUse.Click += new System.EventHandler(this.btnProfUse_Click);
            // 
            // FormNtripProfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 580);
            this.Controls.Add(this.lblProfilesInfo);
            this.Controls.Add(this.lstProfiles);
            this.Controls.Add(this.lblProfileDetails);
            this.Controls.Add(this.btnProfNew);
            this.Controls.Add(this.btnProfCopy);
            this.Controls.Add(this.btnProfRename);
            this.Controls.Add(this.btnProfDelete);
            this.Controls.Add(this.btnProfCancel);
            this.Controls.Add(this.btnProfEdit);
            this.Controls.Add(this.btnProfUse);
            this.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AcceptButton = this.btnProfUse;
            this.CancelButton = this.btnProfCancel;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.BackColor = System.Drawing.Color.FromArgb(240, 240, 240);
            this.Name = "FormNtripProfiles";
            this.Text = "NTRIP profiles";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblProfilesInfo;
        private System.Windows.Forms.ListBox lstProfiles;
        private System.Windows.Forms.Label lblProfileDetails;
        private AgIO.RoundedButton btnProfNew;
        private AgIO.RoundedButton btnProfCopy;
        private AgIO.RoundedButton btnProfRename;
        private AgIO.RoundedButton btnProfDelete;
        private AgIO.RoundedButton btnProfCancel;
        private AgIO.RoundedButton btnProfEdit;
        private AgIO.RoundedButton btnProfUse;
    }
}

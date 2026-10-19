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
            this.btnProfNew = new AgIO.RoundedButton();
            this.btnProfCopy = new AgIO.RoundedButton();
            this.btnProfRename = new AgIO.RoundedButton();
            this.btnProfDelete = new AgIO.RoundedButton();
            this.btnProfCancel = new AgIO.RoundedButton();
            this.btnProfUse = new AgIO.RoundedButton();
            this.SuspendLayout();
            // 
            // lblProfilesInfo
            // 
            this.lblProfilesInfo.AutoSize = false;
            this.lblProfilesInfo.Font = new System.Drawing.Font("Tahoma", 13.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfilesInfo.Location = new System.Drawing.Point(20, 12);
            this.lblProfilesInfo.Name = "lblProfilesInfo";
            this.lblProfilesInfo.Size = new System.Drawing.Size(720, 34);
            this.lblProfilesInfo.TabIndex = 0;
            this.lblProfilesInfo.Text = "NTRIP profiles: a main service and a fallback together.";
            // 
            // lstProfiles
            // 
            this.lstProfiles.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstProfiles.FormattingEnabled = true;
            this.lstProfiles.IntegralHeight = false;
            this.lstProfiles.ItemHeight = 29;
            this.lstProfiles.Location = new System.Drawing.Point(20, 52);
            this.lstProfiles.Name = "lstProfiles";
            this.lstProfiles.Size = new System.Drawing.Size(460, 400);
            this.lstProfiles.TabIndex = 1;
            this.lstProfiles.DoubleClick += new System.EventHandler(this.lstProfiles_DoubleClick);
            // 
            // btnProfNew
            // 
            this.btnProfNew.Location = new System.Drawing.Point(500, 52);
            this.btnProfNew.Name = "btnProfNew";
            this.btnProfNew.Size = new System.Drawing.Size(240, 64);
            this.btnProfNew.TabIndex = 2;
            this.btnProfNew.Text = "New profile";
            this.btnProfNew.Click += new System.EventHandler(this.btnProfNew_Click);
            // 
            // btnProfCopy
            // 
            this.btnProfCopy.Location = new System.Drawing.Point(500, 126);
            this.btnProfCopy.Name = "btnProfCopy";
            this.btnProfCopy.Size = new System.Drawing.Size(240, 64);
            this.btnProfCopy.TabIndex = 3;
            this.btnProfCopy.Text = "Copy";
            this.btnProfCopy.Click += new System.EventHandler(this.btnProfCopy_Click);
            // 
            // btnProfRename
            // 
            this.btnProfRename.Location = new System.Drawing.Point(500, 200);
            this.btnProfRename.Name = "btnProfRename";
            this.btnProfRename.Size = new System.Drawing.Size(240, 64);
            this.btnProfRename.TabIndex = 4;
            this.btnProfRename.Text = "Rename";
            this.btnProfRename.Click += new System.EventHandler(this.btnProfRename_Click);
            // 
            // btnProfDelete
            // 
            this.btnProfDelete.Tone = AgIO.ButtonTone.Negative;
            this.btnProfDelete.Location = new System.Drawing.Point(500, 274);
            this.btnProfDelete.Name = "btnProfDelete";
            this.btnProfDelete.Size = new System.Drawing.Size(240, 64);
            this.btnProfDelete.TabIndex = 5;
            this.btnProfDelete.Text = "Delete";
            this.btnProfDelete.Click += new System.EventHandler(this.btnProfDelete_Click);
            // 
            // btnProfCancel
            // 
            this.btnProfCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnProfCancel.Location = new System.Drawing.Point(500, 388);
            this.btnProfCancel.Name = "btnProfCancel";
            this.btnProfCancel.Size = new System.Drawing.Size(115, 64);
            this.btnProfCancel.TabIndex = 6;
            this.btnProfCancel.Text = "Cancel";
            // 
            // btnProfUse
            // 
            this.btnProfUse.Tone = AgIO.ButtonTone.Positive;
            this.btnProfUse.Location = new System.Drawing.Point(625, 388);
            this.btnProfUse.Name = "btnProfUse";
            this.btnProfUse.Size = new System.Drawing.Size(115, 64);
            this.btnProfUse.TabIndex = 7;
            this.btnProfUse.Text = "Use";
            this.btnProfUse.Click += new System.EventHandler(this.btnProfUse_Click);
            // 
            // FormNtripProfiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 470);
            this.Controls.Add(this.lblProfilesInfo);
            this.Controls.Add(this.lstProfiles);
            this.Controls.Add(this.btnProfNew);
            this.Controls.Add(this.btnProfCopy);
            this.Controls.Add(this.btnProfRename);
            this.Controls.Add(this.btnProfDelete);
            this.Controls.Add(this.btnProfCancel);
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
        private AgIO.RoundedButton btnProfNew;
        private AgIO.RoundedButton btnProfCopy;
        private AgIO.RoundedButton btnProfRename;
        private AgIO.RoundedButton btnProfDelete;
        private AgIO.RoundedButton btnProfCancel;
        private AgIO.RoundedButton btnProfUse;
    }
}

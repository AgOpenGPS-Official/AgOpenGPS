namespace AgOpenGPS
{
    partial class FormTermsAndConditions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormTermsAndConditions));
            this.labelTermsAndConditions = new System.Windows.Forms.Label();
            this.labelTerms = new System.Windows.Forms.Label();
            this.textBoxLicense = new System.Windows.Forms.TextBox();
            this.buttonYouTube = new System.Windows.Forms.Button();
            this.buttonDiscourse = new System.Windows.Forms.Button();
            this.buttonGitHub = new System.Windows.Forms.Button();
            this.labelDisagree = new System.Windows.Forms.Button();
            this.labelAgree = new System.Windows.Forms.Button();
            this.pictureBoxWarning = new System.Windows.Forms.PictureBox();
            this.labelVersion = new System.Windows.Forms.Label();
            this.labelVersionActual = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).BeginInit();
            this.SuspendLayout();
            // 
            // labelTermsAndConditions
            // 
            this.labelTermsAndConditions.BackColor = System.Drawing.Color.Transparent;
            this.labelTermsAndConditions.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTermsAndConditions.Location = new System.Drawing.Point(380, 30);
            this.labelTermsAndConditions.Name = "labelTermsAndConditions";
            this.labelTermsAndConditions.Size = new System.Drawing.Size(580, 45);
            this.labelTermsAndConditions.TabIndex = 34;
            this.labelTermsAndConditions.Text = "Terms and Conditions";
            this.labelTermsAndConditions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelTerms
            // 
            this.labelTerms.BackColor = System.Drawing.Color.Transparent;
            this.labelTerms.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTerms.Location = new System.Drawing.Point(180, 135);
            this.labelTerms.Name = "labelTerms";
            this.labelTerms.Size = new System.Drawing.Size(780, 230);
            this.labelTerms.TabIndex = 132;
            this.labelTerms.Text = resources.GetString("labelTerms.Text");
            this.labelTerms.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBoxLicense
            // 
            this.textBoxLicense.BackColor = System.Drawing.Color.White;
            this.textBoxLicense.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textBoxLicense.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxLicense.Location = new System.Drawing.Point(180, 385);
            this.textBoxLicense.Multiline = true;
            this.textBoxLicense.Name = "textBoxLicense";
            this.textBoxLicense.ReadOnly = true;
            this.textBoxLicense.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxLicense.Size = new System.Drawing.Size(780, 135);
            this.textBoxLicense.TabIndex = 147;
            this.textBoxLicense.Text = resources.GetString("textBoxLicense.Text");
            // 
            // buttonYouTube
            // 
            this.buttonYouTube.BackColor = System.Drawing.Color.Transparent;
            this.buttonYouTube.FlatAppearance.BorderSize = 0;
            this.buttonYouTube.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.buttonYouTube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonYouTube.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.buttonYouTube.Image = global::AgOpenGPS.Properties.Resources.YouTube;
            this.buttonYouTube.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonYouTube.Location = new System.Drawing.Point(30, 412);
            this.buttonYouTube.Name = "buttonYouTube";
            this.buttonYouTube.Size = new System.Drawing.Size(120, 110);
            this.buttonYouTube.TabIndex = 146;
            this.buttonYouTube.Text = "YouTube Tutorials";
            this.buttonYouTube.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonYouTube.UseVisualStyleBackColor = false;
            this.buttonYouTube.Click += new System.EventHandler(this.buttonYouTube_Click);
            // 
            // buttonDiscourse
            // 
            this.buttonDiscourse.BackColor = System.Drawing.Color.Transparent;
            this.buttonDiscourse.FlatAppearance.BorderSize = 0;
            this.buttonDiscourse.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.buttonDiscourse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDiscourse.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.buttonDiscourse.Image = global::AgOpenGPS.Properties.Resources.Discourse;
            this.buttonDiscourse.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDiscourse.Location = new System.Drawing.Point(30, 280);
            this.buttonDiscourse.Name = "buttonDiscourse";
            this.buttonDiscourse.Size = new System.Drawing.Size(120, 120);
            this.buttonDiscourse.TabIndex = 145;
            this.buttonDiscourse.Text = "Discourse Forum";
            this.buttonDiscourse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDiscourse.UseVisualStyleBackColor = false;
            this.buttonDiscourse.Click += new System.EventHandler(this.buttonDiscourse_Click);
            // 
            // buttonGitHub
            // 
            this.buttonGitHub.BackColor = System.Drawing.Color.Transparent;
            this.buttonGitHub.FlatAppearance.BorderSize = 0;
            this.buttonGitHub.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.buttonGitHub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGitHub.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Bold);
            this.buttonGitHub.Image = global::AgOpenGPS.Properties.Resources.GitHub;
            this.buttonGitHub.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonGitHub.Location = new System.Drawing.Point(30, 140);
            this.buttonGitHub.Name = "buttonGitHub";
            this.buttonGitHub.Size = new System.Drawing.Size(120, 120);
            this.buttonGitHub.TabIndex = 144;
            this.buttonGitHub.Text = "Check for Updates";
            this.buttonGitHub.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonGitHub.UseVisualStyleBackColor = false;
            this.buttonGitHub.Click += new System.EventHandler(this.buttonGitHub_Click);
            // 
            // labelDisagree
            // 
            this.labelDisagree.BackColor = System.Drawing.Color.Transparent;
            this.labelDisagree.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.labelDisagree.FlatAppearance.BorderSize = 0;
            this.labelDisagree.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.labelDisagree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelDisagree.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDisagree.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.labelDisagree.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelDisagree.Location = new System.Drawing.Point(685, 535);
            this.labelDisagree.Name = "labelDisagree";
            this.labelDisagree.Size = new System.Drawing.Size(135, 90);
            this.labelDisagree.TabIndex = 39;
            this.labelDisagree.Text = "Disagree";
            this.labelDisagree.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.labelDisagree.UseVisualStyleBackColor = false;
            // 
            // labelAgree
            // 
            this.labelAgree.BackColor = System.Drawing.Color.Transparent;
            this.labelAgree.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.labelAgree.FlatAppearance.BorderSize = 0;
            this.labelAgree.FlatAppearance.MouseOverBackColor = System.Drawing.Color.CornflowerBlue;
            this.labelAgree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.labelAgree.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAgree.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.labelAgree.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelAgree.Location = new System.Drawing.Point(825, 535);
            this.labelAgree.Name = "labelAgree";
            this.labelAgree.Size = new System.Drawing.Size(135, 90);
            this.labelAgree.TabIndex = 10;
            this.labelAgree.Text = "Agree";
            this.labelAgree.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.labelAgree.UseVisualStyleBackColor = false;
            // 
            // pictureBoxWarning
            // 
            this.pictureBoxWarning.BackColor = System.Drawing.Color.Transparent;
            this.pictureBoxWarning.Image = global::AgOpenGPS.Properties.Resources.Warning;
            this.pictureBoxWarning.Location = new System.Drawing.Point(300, 20);
            this.pictureBoxWarning.Name = "pictureBoxWarning";
            this.pictureBoxWarning.Size = new System.Drawing.Size(64, 64);
            this.pictureBoxWarning.TabIndex = 148;
            this.pictureBoxWarning.TabStop = false;
            // 
            // labelVersion
            // 
            this.labelVersion.BackColor = System.Drawing.Color.Transparent;
            this.labelVersion.Font = new System.Drawing.Font("Tahoma", 11F);
            this.labelVersion.Location = new System.Drawing.Point(380, 75);
            this.labelVersion.Name = "labelVersion";
            this.labelVersion.Size = new System.Drawing.Size(120, 25);
            this.labelVersion.TabIndex = 149;
            this.labelVersion.Text = "Version:";
            this.labelVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelVersionActual
            // 
            this.labelVersionActual.BackColor = System.Drawing.Color.Transparent;
            this.labelVersionActual.Font = new System.Drawing.Font("Tahoma", 11F);
            this.labelVersionActual.Location = new System.Drawing.Point(505, 75);
            this.labelVersionActual.Name = "labelVersionActual";
            this.labelVersionActual.Size = new System.Drawing.Size(350, 25);
            this.labelVersionActual.TabIndex = 150;
            this.labelVersionActual.Text = "x.y.z";
            this.labelVersionActual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormTermsAndConditions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Lavender;
            this.ClientSize = new System.Drawing.Size(1000, 640);
            this.Controls.Add(this.pictureBoxWarning);
            this.Controls.Add(this.labelTermsAndConditions);
            this.Controls.Add(this.labelVersion);
            this.Controls.Add(this.labelVersionActual);
            this.Controls.Add(this.labelTerms);
            this.Controls.Add(this.textBoxLicense);
            this.Controls.Add(this.labelAgree);
            this.Controls.Add(this.labelDisagree);
            this.Controls.Add(this.buttonGitHub);
            this.Controls.Add(this.buttonDiscourse);
            this.Controls.Add(this.buttonYouTube);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "FormTermsAndConditions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Form_About_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWarning)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button labelAgree;
        private System.Windows.Forms.Label labelTermsAndConditions;
        private System.Windows.Forms.Button labelDisagree;
        private System.Windows.Forms.Label labelTerms;
        private System.Windows.Forms.Button buttonGitHub;
        private System.Windows.Forms.Button buttonDiscourse;
        private System.Windows.Forms.Button buttonYouTube;
        private System.Windows.Forms.TextBox textBoxLicense;
        private System.Windows.Forms.PictureBox pictureBoxWarning;
        private System.Windows.Forms.Label labelVersion;
        private System.Windows.Forms.Label labelVersionActual;
    }
}
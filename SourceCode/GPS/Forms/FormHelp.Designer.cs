namespace AgOpenGPS
{
    partial class FormHelp
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
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonGitHub = new System.Windows.Forms.Button();
            this.buttonDiscourse = new System.Windows.Forms.Button();
            this.buttonYouTube = new System.Windows.Forms.Button();
            this.pictureBoxQRGitHub = new System.Windows.Forms.PictureBox();
            this.pictureBoxQRDiscourse = new System.Windows.Forms.PictureBox();
            this.pictureBoxQRYouTube = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRGitHub)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRDiscourse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRYouTube)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonClose
            // 
            this.buttonClose.BackColor = System.Drawing.Color.Transparent;
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.buttonClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(630, 380);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(90, 90);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonClose.UseVisualStyleBackColor = false;
            // 
            // buttonGitHub
            // 
            this.buttonGitHub.BackColor = System.Drawing.Color.Transparent;
            this.buttonGitHub.FlatAppearance.BorderSize = 0;
            this.buttonGitHub.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonGitHub.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.buttonGitHub.Image = global::AgOpenGPS.Properties.Resources.GitHub;
            this.buttonGitHub.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonGitHub.Location = new System.Drawing.Point(20, 260);
            this.buttonGitHub.Name = "buttonGitHub";
            this.buttonGitHub.Size = new System.Drawing.Size(220, 110);
            this.buttonGitHub.TabIndex = 560;
            this.buttonGitHub.Text = "Check for Updates";
            this.buttonGitHub.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonGitHub.UseVisualStyleBackColor = false;
            this.buttonGitHub.Click += new System.EventHandler(this.buttonGitHub_Click);
            // 
            // buttonDiscourse
            // 
            this.buttonDiscourse.BackColor = System.Drawing.Color.Transparent;
            this.buttonDiscourse.FlatAppearance.BorderSize = 0;
            this.buttonDiscourse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDiscourse.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.buttonDiscourse.Image = global::AgOpenGPS.Properties.Resources.Discourse;
            this.buttonDiscourse.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDiscourse.Location = new System.Drawing.Point(260, 260);
            this.buttonDiscourse.Name = "buttonDiscourse";
            this.buttonDiscourse.Size = new System.Drawing.Size(220, 110);
            this.buttonDiscourse.TabIndex = 561;
            this.buttonDiscourse.Text = "Discourse Forum";
            this.buttonDiscourse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDiscourse.UseVisualStyleBackColor = false;
            this.buttonDiscourse.Click += new System.EventHandler(this.buttonDiscourse_Click);
            // 
            // buttonYouTube
            // 
            this.buttonYouTube.BackColor = System.Drawing.Color.Transparent;
            this.buttonYouTube.FlatAppearance.BorderSize = 0;
            this.buttonYouTube.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonYouTube.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.buttonYouTube.Image = global::AgOpenGPS.Properties.Resources.YouTube;
            this.buttonYouTube.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonYouTube.Location = new System.Drawing.Point(500, 260);
            this.buttonYouTube.Name = "buttonYouTube";
            this.buttonYouTube.Size = new System.Drawing.Size(220, 110);
            this.buttonYouTube.TabIndex = 562;
            this.buttonYouTube.Text = "YouTube Tutorials";
            this.buttonYouTube.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonYouTube.UseVisualStyleBackColor = false;
            this.buttonYouTube.Click += new System.EventHandler(this.buttonYouTube_Click);
            // 
            // pictureBoxQRGitHub
            // 
            this.pictureBoxQRGitHub.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxQRGitHub.Image = global::AgOpenGPS.Properties.Resources.QRGitHub;
            this.pictureBoxQRGitHub.Location = new System.Drawing.Point(20, 20);
            this.pictureBoxQRGitHub.Name = "pictureBoxQRGitHub";
            this.pictureBoxQRGitHub.Size = new System.Drawing.Size(220, 220);
            this.pictureBoxQRGitHub.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQRGitHub.TabIndex = 563;
            this.pictureBoxQRGitHub.TabStop = false;
            // 
            // pictureBoxQRDiscourse
            // 
            this.pictureBoxQRDiscourse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxQRDiscourse.Image = global::AgOpenGPS.Properties.Resources.QRDiscourse;
            this.pictureBoxQRDiscourse.Location = new System.Drawing.Point(260, 20);
            this.pictureBoxQRDiscourse.Name = "pictureBoxQRDiscourse";
            this.pictureBoxQRDiscourse.Size = new System.Drawing.Size(220, 220);
            this.pictureBoxQRDiscourse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQRDiscourse.TabIndex = 564;
            this.pictureBoxQRDiscourse.TabStop = false;
            // 
            // pictureBoxQRYouTube
            // 
            this.pictureBoxQRYouTube.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBoxQRYouTube.Image = global::AgOpenGPS.Properties.Resources.QRYouTube;
            this.pictureBoxQRYouTube.Location = new System.Drawing.Point(500, 20);
            this.pictureBoxQRYouTube.Name = "pictureBoxQRYouTube";
            this.pictureBoxQRYouTube.Size = new System.Drawing.Size(220, 220);
            this.pictureBoxQRYouTube.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBoxQRYouTube.TabIndex = 565;
            this.pictureBoxQRYouTube.TabStop = false;
            // 
            // FormHelp
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(739, 480);
            this.ControlBox = false;
            this.Controls.Add(this.pictureBoxQRGitHub);
            this.Controls.Add(this.pictureBoxQRDiscourse);
            this.Controls.Add(this.pictureBoxQRYouTube);
            this.Controls.Add(this.buttonGitHub);
            this.Controls.Add(this.buttonDiscourse);
            this.Controls.Add(this.buttonYouTube);
            this.Controls.Add(this.buttonClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MinimizeBox = false;
            this.Name = "FormHelp";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Help";
            this.Load += new System.EventHandler(this.FormHelp_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRGitHub)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRDiscourse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxQRYouTube)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonGitHub;
        private System.Windows.Forms.Button buttonDiscourse;
        private System.Windows.Forms.Button buttonYouTube;
        private System.Windows.Forms.PictureBox pictureBoxQRGitHub;
        private System.Windows.Forms.PictureBox pictureBoxQRDiscourse;
        private System.Windows.Forms.PictureBox pictureBoxQRYouTube;
    }
}
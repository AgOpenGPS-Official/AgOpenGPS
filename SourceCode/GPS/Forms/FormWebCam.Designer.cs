namespace AgOpenGPS
{
    partial class FormWebCam
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
            this.webCamPictureBox = new System.Windows.Forms.PictureBox();
            this.webCamBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.webCamPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // webCamPictureBox
            // 
            this.webCamPictureBox.BackColor = System.Drawing.Color.Black;
            this.webCamPictureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webCamPictureBox.Location = new System.Drawing.Point(0, 0);
            this.webCamPictureBox.Name = "webCamPictureBox";
            this.webCamPictureBox.Size = new System.Drawing.Size(398, 268);
            this.webCamPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.webCamPictureBox.TabIndex = 14;
            this.webCamPictureBox.TabStop = false;
            // 
            // webCamBackgroundWorker
            // 
            this.webCamBackgroundWorker.WorkerReportsProgress = true;
            this.webCamBackgroundWorker.WorkerSupportsCancellation = true;
            this.webCamBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.webCamBackgroundWorker_DoWork);
            this.webCamBackgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.webCamBackgroundWorker_ProgressChanged);
            // 
            // FormWebCam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(398, 268);
            this.Controls.Add(this.webCamPictureBox);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "FormWebCam";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "WebCam";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormWebCam_FormClosing);
            this.Load += new System.EventHandler(this.FormWebCam_Load);
            ((System.ComponentModel.ISupportInitialize)(this.webCamPictureBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox webCamPictureBox;
        private System.ComponentModel.BackgroundWorker webCamBackgroundWorker;
    }
}
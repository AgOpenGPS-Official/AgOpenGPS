namespace AgOpenGPS
{
    partial class FormStartWork
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnNewJob = new System.Windows.Forms.Button();
            this.btnResumeJob = new System.Windows.Forms.Button();
            this.btnOpenFieldOnly = new System.Windows.Forms.Button();
            this.btnCloseField = new System.Windows.Forms.Button();
            this.lblLastJob = new System.Windows.Forms.Label();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.panelAgShare = new System.Windows.Forms.Panel();
            this.btnAgShareDownload = new System.Windows.Forms.Button();
            this.btnAgShareUpload = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panelAgShare.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Controls.Add(this.lblLastJob);
            this.panel1.Controls.Add(this.lblCurrentField);
            this.panel1.Controls.Add(this.panelAgShare);
            this.panel1.Controls.Add(this.btnCancel);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(624, 500);
            this.panel1.TabIndex = 0;
            //
            // tableLayoutPanel1
            //
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnNewJob, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnResumeJob, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnOpenFieldOnly, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnCloseField, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(10, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(604, 220);
            this.tableLayoutPanel1.TabIndex = 0;
            //
            // btnNewJob
            //
            this.btnNewJob.BackColor = System.Drawing.Color.Transparent;
            this.btnNewJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNewJob.FlatAppearance.BorderSize = 0;
            this.btnNewJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnNewJob.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnNewJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.Location = new System.Drawing.Point(307, 5);
            this.btnNewJob.Margin = new System.Windows.Forms.Padding(5);
            this.btnNewJob.Name = "btnNewJob";
            this.btnNewJob.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnNewJob.Size = new System.Drawing.Size(292, 100);
            this.btnNewJob.TabIndex = 1;
            this.btnNewJob.Text = "New Job";
            this.btnNewJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnNewJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnNewJob.UseVisualStyleBackColor = false;
            this.btnNewJob.Click += new System.EventHandler(this.btnNewJob_Click);
            //
            // btnResumeJob
            //
            this.btnResumeJob.BackColor = System.Drawing.Color.Transparent;
            this.btnResumeJob.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResumeJob.FlatAppearance.BorderSize = 0;
            this.btnResumeJob.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumeJob.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnResumeJob.Image = global::AgOpenGPS.Properties.Resources.FilePrevious;
            this.btnResumeJob.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeJob.Location = new System.Drawing.Point(5, 5);
            this.btnResumeJob.Margin = new System.Windows.Forms.Padding(5);
            this.btnResumeJob.Name = "btnResumeJob";
            this.btnResumeJob.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnResumeJob.Size = new System.Drawing.Size(292, 100);
            this.btnResumeJob.TabIndex = 0;
            this.btnResumeJob.Text = "Resume Job";
            this.btnResumeJob.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnResumeJob.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnResumeJob.UseVisualStyleBackColor = false;
            this.btnResumeJob.Click += new System.EventHandler(this.btnResumeJob_Click);
            //
            // btnOpenFieldOnly
            //
            this.btnOpenFieldOnly.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenFieldOnly.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOpenFieldOnly.FlatAppearance.BorderSize = 0;
            this.btnOpenFieldOnly.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFieldOnly.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnOpenFieldOnly.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenFieldOnly.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldOnly.Location = new System.Drawing.Point(5, 115);
            this.btnOpenFieldOnly.Margin = new System.Windows.Forms.Padding(5);
            this.btnOpenFieldOnly.Name = "btnOpenFieldOnly";
            this.btnOpenFieldOnly.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnOpenFieldOnly.Size = new System.Drawing.Size(292, 100);
            this.btnOpenFieldOnly.TabIndex = 2;
            this.btnOpenFieldOnly.Text = "Open Field";
            this.btnOpenFieldOnly.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenFieldOnly.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenFieldOnly.UseVisualStyleBackColor = false;
            this.btnOpenFieldOnly.Click += new System.EventHandler(this.btnOpenFieldOnly_Click);
            //
            // btnCloseField
            //
            this.btnCloseField.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseField.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCloseField.FlatAppearance.BorderSize = 0;
            this.btnCloseField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.btnCloseField.Image = global::AgOpenGPS.Properties.Resources.FileClose;
            this.btnCloseField.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.Location = new System.Drawing.Point(307, 115);
            this.btnCloseField.Margin = new System.Windows.Forms.Padding(5);
            this.btnCloseField.Name = "btnCloseField";
            this.btnCloseField.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnCloseField.Size = new System.Drawing.Size(292, 100);
            this.btnCloseField.TabIndex = 3;
            this.btnCloseField.Text = "Close Field";
            this.btnCloseField.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnCloseField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCloseField.UseVisualStyleBackColor = false;
            this.btnCloseField.Click += new System.EventHandler(this.btnCloseField_Click);
            //
            // lblLastJob
            //
            this.lblLastJob.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLastJob.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblLastJob.ForeColor = System.Drawing.Color.DimGray;
            this.lblLastJob.Location = new System.Drawing.Point(10, 235);
            this.lblLastJob.Name = "lblLastJob";
            this.lblLastJob.Size = new System.Drawing.Size(604, 50);
            this.lblLastJob.TabIndex = 1;
            this.lblLastJob.Text = "Last Job Info";
            this.lblLastJob.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // lblCurrentField
            //
            this.lblCurrentField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.lblCurrentField.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.lblCurrentField.ForeColor = System.Drawing.Color.White;
            this.lblCurrentField.Location = new System.Drawing.Point(10, 290);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Size = new System.Drawing.Size(604, 35);
            this.lblCurrentField.TabIndex = 2;
            this.lblCurrentField.Text = "Current: FieldName";
            this.lblCurrentField.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblCurrentField.Visible = false;
            //
            // panelAgShare
            //
            this.panelAgShare.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelAgShare.Controls.Add(this.btnAgShareDownload);
            this.panelAgShare.Controls.Add(this.btnAgShareUpload);
            this.panelAgShare.Location = new System.Drawing.Point(10, 335);
            this.panelAgShare.Name = "panelAgShare";
            this.panelAgShare.Size = new System.Drawing.Size(604, 90);
            this.panelAgShare.TabIndex = 3;
            //
            // btnAgShareDownload
            //
            this.btnAgShareDownload.BackColor = System.Drawing.Color.Transparent;
            this.btnAgShareDownload.Dock = System.Windows.Forms.DockStyle.Left;
            this.btnAgShareDownload.FlatAppearance.BorderSize = 0;
            this.btnAgShareDownload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgShareDownload.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnAgShareDownload.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnAgShareDownload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareDownload.Location = new System.Drawing.Point(0, 0);
            this.btnAgShareDownload.Name = "btnAgShareDownload";
            this.btnAgShareDownload.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnAgShareDownload.Size = new System.Drawing.Size(297, 90);
            this.btnAgShareDownload.TabIndex = 0;
            this.btnAgShareDownload.Text = "AgShare Download";
            this.btnAgShareDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareDownload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgShareDownload.UseVisualStyleBackColor = false;
            this.btnAgShareDownload.Click += new System.EventHandler(this.btnAgShareDownload_Click);
            //
            // btnAgShareUpload
            //
            this.btnAgShareUpload.BackColor = System.Drawing.Color.Transparent;
            this.btnAgShareUpload.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnAgShareUpload.FlatAppearance.BorderSize = 0;
            this.btnAgShareUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAgShareUpload.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnAgShareUpload.Image = global::AgOpenGPS.Properties.Resources.AgShare;
            this.btnAgShareUpload.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareUpload.Location = new System.Drawing.Point(307, 0);
            this.btnAgShareUpload.Name = "btnAgShareUpload";
            this.btnAgShareUpload.Padding = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnAgShareUpload.Size = new System.Drawing.Size(297, 90);
            this.btnAgShareUpload.TabIndex = 1;
            this.btnAgShareUpload.Text = "AgShare Upload";
            this.btnAgShareUpload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAgShareUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAgShareUpload.UseVisualStyleBackColor = false;
            this.btnAgShareUpload.Click += new System.EventHandler(this.btnAgShareUpload_Click);
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(485, 435);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(129, 57);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // FormStartWork
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(624, 500);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
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
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panelAgShare.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnNewJob;
        private System.Windows.Forms.Button btnResumeJob;
        private System.Windows.Forms.Button btnOpenFieldOnly;
        private System.Windows.Forms.Button btnCloseField;
        private System.Windows.Forms.Label lblLastJob;
        private System.Windows.Forms.Label lblCurrentField;
        private System.Windows.Forms.Panel panelAgShare;
        private System.Windows.Forms.Button btnAgShareDownload;
        private System.Windows.Forms.Button btnAgShareUpload;
        private System.Windows.Forms.Button btnCancel;
    }
}

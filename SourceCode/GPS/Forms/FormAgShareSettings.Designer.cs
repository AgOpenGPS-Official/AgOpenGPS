namespace AgOpenGPS
{
    partial class FormAgShareSettings
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
            this.textBoxApiKey = new System.Windows.Forms.TextBox();
            this.buttonTestConnection = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.labelApiKey = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxServer = new System.Windows.Forms.TextBox();
            this.btnPaste = new System.Windows.Forms.Button();
            this.linkRegister = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDevelop = new System.Windows.Forms.Button();
            this.btnAutoUpload = new System.Windows.Forms.Button();
            this.btnToggleUpload = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBoxApiKey
            // 
            this.textBoxApiKey.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxApiKey.BackColor = System.Drawing.Color.White;
            this.textBoxApiKey.Location = new System.Drawing.Point(122, 59);
            this.textBoxApiKey.Name = "textBoxApiKey";
            this.textBoxApiKey.Size = new System.Drawing.Size(441, 30);
            this.textBoxApiKey.TabIndex = 0;
            // 
            // buttonTestConnection
            // 
            this.buttonTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTestConnection.Location = new System.Drawing.Point(343, 96);
            this.buttonTestConnection.Name = "buttonTestConnection";
            this.buttonTestConnection.Size = new System.Drawing.Size(220, 35);
            this.buttonTestConnection.TabIndex = 1;
            this.buttonTestConnection.Text = "Test Connection";
            this.buttonTestConnection.UseVisualStyleBackColor = true;
            this.buttonTestConnection.Click += new System.EventHandler(this.buttonTestConnection_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.labelStatus.ForeColor = System.Drawing.Color.DarkRed;
            this.labelStatus.Location = new System.Drawing.Point(20, 141);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(543, 25);
            this.labelStatus.TabIndex = 4;
            this.labelStatus.Text = "Enter Details Above and Tick Test Connection";
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelApiKey
            // 
            this.labelApiKey.BackColor = System.Drawing.Color.Transparent;
            this.labelApiKey.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelApiKey.ForeColor = System.Drawing.Color.Black;
            this.labelApiKey.Location = new System.Drawing.Point(16, 62);
            this.labelApiKey.Name = "labelApiKey";
            this.labelApiKey.Size = new System.Drawing.Size(100, 23);
            this.labelApiKey.TabIndex = 5;
            this.labelApiKey.Text = "API Key:";
            this.labelApiKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(100, 23);
            this.label1.TabIndex = 6;
            this.label1.Text = "Server:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBoxServer
            // 
            this.textBoxServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxServer.BackColor = System.Drawing.Color.White;
            this.textBoxServer.Location = new System.Drawing.Point(122, 23);
            this.textBoxServer.Name = "textBoxServer";
            this.textBoxServer.Size = new System.Drawing.Size(441, 30);
            this.textBoxServer.TabIndex = 7;
            this.textBoxServer.Click += new System.EventHandler(this.textBoxServer_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnPaste.Location = new System.Drawing.Point(122, 96);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(192, 35);
            this.btnPaste.TabIndex = 9;
            this.btnPaste.Text = "Paste from Clipboard";
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // linkRegister
            // 
            this.linkRegister.AutoSize = true;
            this.linkRegister.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            this.linkRegister.Location = new System.Drawing.Point(147, 341);
            this.linkRegister.Name = "linkRegister";
            this.linkRegister.Size = new System.Drawing.Size(280, 23);
            this.linkRegister.TabIndex = 10;
            this.linkRegister.TabStop = true;
            this.linkRegister.Text = "https://agshare.agopengps.com";
            this.linkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkRegister_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(158, 318);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(256, 23);
            this.label2.TabIndex = 11;
            this.label2.Text = "Register here to use AgShare";
            // 
            // btnDevelop
            // 
            this.btnDevelop.BackColor = System.Drawing.Color.Transparent;
            this.btnDevelop.FlatAppearance.BorderSize = 0;
            this.btnDevelop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDevelop.Location = new System.Drawing.Point(12, 338);
            this.btnDevelop.Name = "btnDevelop";
            this.btnDevelop.Size = new System.Drawing.Size(22, 23);
            this.btnDevelop.TabIndex = 12;
            this.btnDevelop.UseVisualStyleBackColor = false;
            this.btnDevelop.Click += new System.EventHandler(this.btnDevelop_Click);
            // 
            // btnAutoUpload
            // 
            this.btnAutoUpload.FlatAppearance.BorderSize = 0;
            this.btnAutoUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoUpload.Image = global::AgOpenGPS.Properties.Resources.AutoUploadOn;
            this.btnAutoUpload.Location = new System.Drawing.Point(12, 173);
            this.btnAutoUpload.Name = "btnAutoUpload";
            this.btnAutoUpload.Size = new System.Drawing.Size(128, 128);
            this.btnAutoUpload.TabIndex = 13;
            this.btnAutoUpload.Text = "Auto-Upload";
            this.btnAutoUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAutoUpload.UseVisualStyleBackColor = true;
            this.btnAutoUpload.Click += new System.EventHandler(this.btnAutoUpload_Click);
            // 
            // btnToggleUpload
            // 
            this.btnToggleUpload.FlatAppearance.BorderSize = 0;
            this.btnToggleUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnToggleUpload.Image = global::AgOpenGPS.Properties.Resources.UploadOff;
            this.btnToggleUpload.Location = new System.Drawing.Point(224, 173);
            this.btnToggleUpload.Name = "btnToggleUpload";
            this.btnToggleUpload.Size = new System.Drawing.Size(128, 128);
            this.btnToggleUpload.TabIndex = 8;
            this.btnToggleUpload.Text = "Activate";
            this.btnToggleUpload.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnToggleUpload.UseVisualStyleBackColor = true;
            this.btnToggleUpload.Click += new System.EventHandler(this.btnToggleUpload_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonCancel.Location = new System.Drawing.Point(406, 209);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 92);
            this.buttonCancel.TabIndex = 3;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonSave.Enabled = false;
            this.buttonSave.FlatAppearance.BorderSize = 0;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonSave.Location = new System.Drawing.Point(492, 209);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(80, 92);
            this.buttonSave.TabIndex = 2;
            this.buttonSave.Text = "Save";
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // FormAgShareSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(584, 373);
            this.ControlBox = false;
            this.Controls.Add(this.btnAutoUpload);
            this.Controls.Add(this.btnDevelop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.linkRegister);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnToggleUpload);
            this.Controls.Add(this.textBoxServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelApiKey);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.buttonTestConnection);
            this.Controls.Add(this.textBoxApiKey);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.MinimumSize = new System.Drawing.Size(550, 340);
            this.Name = "FormAgShareSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AgShare Settings";
            this.Load += new System.EventHandler(this.FormAgShareSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonTestConnection;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Label labelApiKey;
        private System.Windows.Forms.TextBox textBoxApiKey;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxServer;
        private System.Windows.Forms.Button btnToggleUpload;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.LinkLabel linkRegister;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDevelop;
        private System.Windows.Forms.Button btnAutoUpload;
    }
}
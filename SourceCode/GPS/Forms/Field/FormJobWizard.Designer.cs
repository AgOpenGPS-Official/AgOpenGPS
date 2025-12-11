namespace AgOpenGPS
{
    partial class FormJobWizard
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
            this.lblStepTitle = new System.Windows.Forms.Label();
            this.panelStep1 = new System.Windows.Forms.Panel();
            this.lblStep1Info = new System.Windows.Forms.Label();
            this.listProfiles = new System.Windows.Forms.ListBox();
            this.panelStep2 = new System.Windows.Forms.Panel();
            this.lblStep2Info = new System.Windows.Forms.Label();
            this.listFields = new System.Windows.Forms.ListBox();
            this.panelStep3 = new System.Windows.Forms.Panel();
            this.txtJobName = new System.Windows.Forms.TextBox();
            this.lblJobName = new System.Windows.Forms.Label();
            this.flpWorkTypes = new System.Windows.Forms.FlowLayoutPanel();
            this.panelButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panelStep1.SuspendLayout();
            this.panelStep2.SuspendLayout();
            this.panelStep3.SuspendLayout();
            this.panelButtons.SuspendLayout();
            this.SuspendLayout();
            //
            // panel1
            //
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.lblStepTitle);
            this.panel1.Controls.Add(this.panelStep1);
            this.panel1.Controls.Add(this.panelStep2);
            this.panel1.Controls.Add(this.panelStep3);
            this.panel1.Controls.Add(this.panelButtons);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(10);
            this.panel1.Size = new System.Drawing.Size(700, 550);
            this.panel1.TabIndex = 0;
            //
            // lblStepTitle
            //
            this.lblStepTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStepTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.lblStepTitle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.lblStepTitle.ForeColor = System.Drawing.Color.White;
            this.lblStepTitle.Location = new System.Drawing.Point(10, 10);
            this.lblStepTitle.Name = "lblStepTitle";
            this.lblStepTitle.Size = new System.Drawing.Size(680, 50);
            this.lblStepTitle.TabIndex = 0;
            this.lblStepTitle.Text = "Step 1: Select Machine Profile";
            this.lblStepTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // panelStep1
            //
            this.panelStep1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep1.Controls.Add(this.lblStep1Info);
            this.panelStep1.Controls.Add(this.listProfiles);
            this.panelStep1.Location = new System.Drawing.Point(10, 70);
            this.panelStep1.Name = "panelStep1";
            this.panelStep1.Size = new System.Drawing.Size(680, 400);
            this.panelStep1.TabIndex = 1;
            //
            // lblStep1Info
            //
            this.lblStep1Info.AutoSize = true;
            this.lblStep1Info.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblStep1Info.Location = new System.Drawing.Point(10, 10);
            this.lblStep1Info.Name = "lblStep1Info";
            this.lblStep1Info.Size = new System.Drawing.Size(350, 19);
            this.lblStep1Info.TabIndex = 0;
            this.lblStep1Info.Text = "Select the machine profile for this work session:";
            //
            // listProfiles
            //
            this.listProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listProfiles.Font = new System.Drawing.Font("Tahoma", 16F);
            this.listProfiles.FormattingEnabled = true;
            this.listProfiles.ItemHeight = 26;
            this.listProfiles.Items.AddRange(new object[] {
            "Current Profile (use loaded profile)"});
            this.listProfiles.Location = new System.Drawing.Point(10, 40);
            this.listProfiles.Name = "listProfiles";
            this.listProfiles.Size = new System.Drawing.Size(660, 342);
            this.listProfiles.TabIndex = 1;
            //
            // panelStep2
            //
            this.panelStep2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep2.Controls.Add(this.lblStep2Info);
            this.panelStep2.Controls.Add(this.listFields);
            this.panelStep2.Location = new System.Drawing.Point(10, 70);
            this.panelStep2.Name = "panelStep2";
            this.panelStep2.Size = new System.Drawing.Size(680, 400);
            this.panelStep2.TabIndex = 2;
            this.panelStep2.Visible = false;
            //
            // lblStep2Info
            //
            this.lblStep2Info.AutoSize = true;
            this.lblStep2Info.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblStep2Info.Location = new System.Drawing.Point(10, 10);
            this.lblStep2Info.Name = "lblStep2Info";
            this.lblStep2Info.Size = new System.Drawing.Size(248, 19);
            this.lblStep2Info.TabIndex = 0;
            this.lblStep2Info.Text = "Select the field to work on:";
            //
            // listFields
            //
            this.listFields.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listFields.Font = new System.Drawing.Font("Tahoma", 16F);
            this.listFields.FormattingEnabled = true;
            this.listFields.ItemHeight = 26;
            this.listFields.Items.AddRange(new object[] {
            "Current Field (use open field)"});
            this.listFields.Location = new System.Drawing.Point(10, 40);
            this.listFields.Name = "listFields";
            this.listFields.Size = new System.Drawing.Size(660, 342);
            this.listFields.TabIndex = 1;
            //
            // panelStep3
            //
            this.panelStep3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelStep3.Controls.Add(this.txtJobName);
            this.panelStep3.Controls.Add(this.lblJobName);
            this.panelStep3.Controls.Add(this.flpWorkTypes);
            this.panelStep3.Location = new System.Drawing.Point(10, 70);
            this.panelStep3.Name = "panelStep3";
            this.panelStep3.Size = new System.Drawing.Size(680, 400);
            this.panelStep3.TabIndex = 3;
            this.panelStep3.Visible = false;
            //
            // txtJobName
            //
            this.txtJobName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtJobName.Font = new System.Drawing.Font("Tahoma", 16F);
            this.txtJobName.Location = new System.Drawing.Point(10, 360);
            this.txtJobName.Name = "txtJobName";
            this.txtJobName.Size = new System.Drawing.Size(660, 33);
            this.txtJobName.TabIndex = 2;
            this.txtJobName.Text = "2024-12-11_Work";
            //
            // lblJobName
            //
            this.lblJobName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblJobName.AutoSize = true;
            this.lblJobName.Font = new System.Drawing.Font("Tahoma", 12F);
            this.lblJobName.Location = new System.Drawing.Point(10, 335);
            this.lblJobName.Name = "lblJobName";
            this.lblJobName.Size = new System.Drawing.Size(76, 19);
            this.lblJobName.TabIndex = 1;
            this.lblJobName.Text = "Job Name:";
            //
            // flpWorkTypes
            //
            this.flpWorkTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flpWorkTypes.AutoScroll = true;
            this.flpWorkTypes.Location = new System.Drawing.Point(10, 10);
            this.flpWorkTypes.Name = "flpWorkTypes";
            this.flpWorkTypes.Size = new System.Drawing.Size(660, 315);
            this.flpWorkTypes.TabIndex = 0;
            //
            // panelButtons
            //
            this.panelButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelButtons.Controls.Add(this.btnCancel);
            this.panelButtons.Controls.Add(this.btnNext);
            this.panelButtons.Controls.Add(this.btnBack);
            this.panelButtons.Location = new System.Drawing.Point(10, 480);
            this.panelButtons.Name = "panelButtons";
            this.panelButtons.Size = new System.Drawing.Size(680, 60);
            this.panelButtons.TabIndex = 4;
            //
            // btnCancel
            //
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(0, 0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(80, 60);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            //
            // btnNext
            //
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(119)))), ((int)(((byte)(190)))));
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnNext.ForeColor = System.Drawing.Color.White;
            this.btnNext.Location = new System.Drawing.Point(530, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(150, 50);
            this.btnNext.TabIndex = 2;
            this.btnNext.Text = "Next >";
            this.btnNext.UseVisualStyleBackColor = false;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            //
            // btnBack
            //
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.BackColor = System.Drawing.Color.LightGray;
            this.btnBack.Enabled = false;
            this.btnBack.FlatAppearance.BorderSize = 0;
            this.btnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBack.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.btnBack.ForeColor = System.Drawing.Color.Black;
            this.btnBack.Location = new System.Drawing.Point(370, 5);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(150, 50);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "< Back";
            this.btnBack.UseVisualStyleBackColor = false;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            //
            // FormJobWizard
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(700, 550);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormJobWizard";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "New Job Wizard";
            this.Load += new System.EventHandler(this.FormJobWizard_Load);
            this.panel1.ResumeLayout(false);
            this.panelStep1.ResumeLayout(false);
            this.panelStep1.PerformLayout();
            this.panelStep2.ResumeLayout(false);
            this.panelStep2.PerformLayout();
            this.panelStep3.ResumeLayout(false);
            this.panelStep3.PerformLayout();
            this.panelButtons.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStepTitle;
        private System.Windows.Forms.Panel panelStep1;
        private System.Windows.Forms.Label lblStep1Info;
        private System.Windows.Forms.ListBox listProfiles;
        private System.Windows.Forms.Panel panelStep2;
        private System.Windows.Forms.Label lblStep2Info;
        private System.Windows.Forms.ListBox listFields;
        private System.Windows.Forms.Panel panelStep3;
        private System.Windows.Forms.TextBox txtJobName;
        private System.Windows.Forms.Label lblJobName;
        private System.Windows.Forms.FlowLayoutPanel flpWorkTypes;
        private System.Windows.Forms.Panel panelButtons;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnBack;
    }
}

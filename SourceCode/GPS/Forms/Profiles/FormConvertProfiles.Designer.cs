namespace AgOpenGPS.Forms.Profiles
{
    partial class FormConvertProfiles
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
            this.listViewFiles = new System.Windows.Forms.ListView();
            this.columnHeaderName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelDetails = new System.Windows.Forms.Panel();
            this.checkBoxVehicle = new System.Windows.Forms.CheckBox();
            this.textBoxVehicleName = new System.Windows.Forms.TextBox();
            this.labelToolName = new System.Windows.Forms.Label();
            this.textBoxToolName = new System.Windows.Forms.TextBox();
            this.checkBoxEnvironment = new System.Windows.Forms.CheckBox();
            this.textBoxEnvName = new System.Windows.Forms.TextBox();
            this.labelOldFiles = new System.Windows.Forms.Label();
            this.labelSaveTo = new System.Windows.Forms.Label();
            this.panelDetails.SuspendLayout();
            this.SuspendLayout();
            //
            // labelTitle
            //
            this.labelTitle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(12, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(660, 28);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Convert Old Profile Files";
            this.labelTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // labelOldFiles
            //
            this.labelOldFiles.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelOldFiles.Location = new System.Drawing.Point(12, 42);
            this.labelOldFiles.Name = "labelOldFiles";
            this.labelOldFiles.Size = new System.Drawing.Size(280, 25);
            this.labelOldFiles.TabIndex = 20;
            this.labelOldFiles.Text = "Old format files:";
            this.labelOldFiles.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // listViewFiles
            //
            this.listViewFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.listViewFiles.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName});
            this.listViewFiles.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.listViewFiles.FullRowSelect = true;
            this.listViewFiles.GridLines = true;
            this.listViewFiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewFiles.HideSelection = false;
            this.listViewFiles.LabelWrap = false;
            this.listViewFiles.Location = new System.Drawing.Point(12, 70);
            this.listViewFiles.Margin = new System.Windows.Forms.Padding(0);
            this.listViewFiles.MultiSelect = false;
            this.listViewFiles.Name = "listViewFiles";
            this.listViewFiles.Size = new System.Drawing.Size(280, 330);
            this.listViewFiles.TabIndex = 1;
            this.listViewFiles.UseCompatibleStateImageBehavior = false;
            this.listViewFiles.View = System.Windows.Forms.View.Details;
            this.listViewFiles.SelectedIndexChanged += new System.EventHandler(this.listViewFiles_SelectedIndexChanged);
            //
            // columnHeaderName
            //
            this.columnHeaderName.Text = "File Name";
            this.columnHeaderName.Width = 250;
            //
            // labelSaveTo
            //
            this.labelSaveTo.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.labelSaveTo.Location = new System.Drawing.Point(305, 42);
            this.labelSaveTo.Name = "labelSaveTo";
            this.labelSaveTo.Size = new System.Drawing.Size(360, 25);
            this.labelSaveTo.TabIndex = 21;
            this.labelSaveTo.Text = "Save as:";
            this.labelSaveTo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // panelDetails
            //
            this.panelDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetails.Controls.Add(this.checkBoxVehicle);
            this.panelDetails.Controls.Add(this.textBoxVehicleName);
            this.panelDetails.Controls.Add(this.labelToolName);
            this.panelDetails.Controls.Add(this.textBoxToolName);
            this.panelDetails.Controls.Add(this.checkBoxEnvironment);
            this.panelDetails.Controls.Add(this.textBoxEnvName);
            this.panelDetails.Enabled = false;
            this.panelDetails.Location = new System.Drawing.Point(305, 70);
            this.panelDetails.Name = "panelDetails";
            this.panelDetails.Size = new System.Drawing.Size(370, 240);
            this.panelDetails.TabIndex = 2;
            //
            // checkBoxVehicle
            //
            this.checkBoxVehicle.Checked = true;
            this.checkBoxVehicle.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxVehicle.Font = new System.Drawing.Font("Tahoma", 12F);
            this.checkBoxVehicle.Location = new System.Drawing.Point(5, 5);
            this.checkBoxVehicle.Name = "checkBoxVehicle";
            this.checkBoxVehicle.Size = new System.Drawing.Size(350, 25);
            this.checkBoxVehicle.TabIndex = 0;
            this.checkBoxVehicle.Text = "Also export Vehicle settings";
            this.checkBoxVehicle.CheckedChanged += new System.EventHandler(this.checkBoxVehicle_CheckedChanged);
            //
            // textBoxVehicleName
            //
            this.textBoxVehicleName.Font = new System.Drawing.Font("Tahoma", 14F);
            this.textBoxVehicleName.Location = new System.Drawing.Point(5, 33);
            this.textBoxVehicleName.Name = "textBoxVehicleName";
            this.textBoxVehicleName.Size = new System.Drawing.Size(350, 30);
            this.textBoxVehicleName.TabIndex = 1;
            this.textBoxVehicleName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            //
            // labelToolName
            //
            this.labelToolName.Font = new System.Drawing.Font("Tahoma", 12F);
            this.labelToolName.Location = new System.Drawing.Point(5, 73);
            this.labelToolName.Name = "labelToolName";
            this.labelToolName.Size = new System.Drawing.Size(350, 25);
            this.labelToolName.TabIndex = 2;
            this.labelToolName.Text = "Tool name:";
            this.labelToolName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // textBoxToolName
            //
            this.textBoxToolName.Font = new System.Drawing.Font("Tahoma", 14F);
            this.textBoxToolName.Location = new System.Drawing.Point(5, 101);
            this.textBoxToolName.Name = "textBoxToolName";
            this.textBoxToolName.Size = new System.Drawing.Size(350, 30);
            this.textBoxToolName.TabIndex = 3;
            this.textBoxToolName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            //
            // checkBoxEnvironment
            //
            this.checkBoxEnvironment.Font = new System.Drawing.Font("Tahoma", 12F);
            this.checkBoxEnvironment.Location = new System.Drawing.Point(5, 145);
            this.checkBoxEnvironment.Name = "checkBoxEnvironment";
            this.checkBoxEnvironment.Size = new System.Drawing.Size(350, 25);
            this.checkBoxEnvironment.TabIndex = 4;
            this.checkBoxEnvironment.Text = "Also export Environment settings";
            this.checkBoxEnvironment.CheckedChanged += new System.EventHandler(this.checkBoxEnvironment_CheckedChanged);
            //
            // textBoxEnvName
            //
            this.textBoxEnvName.Enabled = false;
            this.textBoxEnvName.Font = new System.Drawing.Font("Tahoma", 14F);
            this.textBoxEnvName.Location = new System.Drawing.Point(5, 175);
            this.textBoxEnvName.Name = "textBoxEnvName";
            this.textBoxEnvName.Size = new System.Drawing.Size(350, 30);
            this.textBoxEnvName.TabIndex = 5;
            this.textBoxEnvName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            //
            // labelStatus
            //
            this.labelStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelStatus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Italic);
            this.labelStatus.Location = new System.Drawing.Point(12, 408);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(400, 25);
            this.labelStatus.TabIndex = 3;
            this.labelStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // buttonClose
            //
            this.buttonClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonClose.FlatAppearance.BorderSize = 0;
            this.buttonClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonClose.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.buttonClose.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.buttonClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonClose.Location = new System.Drawing.Point(595, 340);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(80, 92);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "Close";
            this.buttonClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // buttonConvert
            //
            this.buttonConvert.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConvert.Enabled = false;
            this.buttonConvert.FlatAppearance.BorderSize = 0;
            this.buttonConvert.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConvert.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.buttonConvert.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonConvert.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonConvert.Location = new System.Drawing.Point(509, 340);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(80, 92);
            this.buttonConvert.TabIndex = 4;
            this.buttonConvert.Text = "Convert";
            this.buttonConvert.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            //
            // FormConvertProfiles
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(688, 445);
            this.ControlBox = false;
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.panelDetails);
            this.Controls.Add(this.labelSaveTo);
            this.Controls.Add(this.listViewFiles);
            this.Controls.Add(this.labelOldFiles);
            this.Controls.Add(this.labelTitle);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.MinimumSize = new System.Drawing.Size(700, 454);
            this.Name = "FormConvertProfiles";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Convert Old Profiles";
            this.Load += new System.EventHandler(this.FormConvertProfiles_Load);
            this.panelDetails.ResumeLayout(false);
            this.panelDetails.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListView listViewFiles;
        private System.Windows.Forms.ColumnHeader columnHeaderName;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelDetails;
        private System.Windows.Forms.CheckBox checkBoxVehicle;
        private System.Windows.Forms.TextBox textBoxVehicleName;
        private System.Windows.Forms.Label labelToolName;
        private System.Windows.Forms.TextBox textBoxToolName;
        private System.Windows.Forms.CheckBox checkBoxEnvironment;
        private System.Windows.Forms.TextBox textBoxEnvName;
        private System.Windows.Forms.Label labelOldFiles;
        private System.Windows.Forms.Label labelSaveTo;
    }
}

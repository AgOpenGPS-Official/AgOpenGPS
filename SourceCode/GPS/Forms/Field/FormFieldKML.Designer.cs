namespace AgOpenGPS
{
    partial class FormFieldKML
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
            this.tboxFieldName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnLoadKML = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSerialCancel = new System.Windows.Forms.Button();
            this.labelFieldname = new System.Windows.Forms.Label();
            this.btnAddDate = new System.Windows.Forms.Button();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tboxFieldName
            // 
            this.tboxFieldName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxFieldName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFieldName.Location = new System.Drawing.Point(15, 38);
            this.tboxFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxFieldName.Name = "tboxFieldName";
            this.tboxFieldName.Size = new System.Drawing.Size(565, 36);
            this.tboxFieldName.TabIndex = 0;
            this.tboxFieldName.Click += new System.EventHandler(this.tboxFieldName_Click);
            this.tboxFieldName.TextChanged += new System.EventHandler(this.tboxFieldName_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(172, 153);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 23);
            this.label3.TabIndex = 150;
            this.label3.Text = "+";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(22, 152);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 23);
            this.label2.TabIndex = 149;
            this.label2.Text = "+";
            // 
            // btnLoadKML
            // 
            this.btnLoadKML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnLoadKML.BackColor = System.Drawing.Color.Transparent;
            this.btnLoadKML.Enabled = false;
            this.btnLoadKML.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnLoadKML.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadKML.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnLoadKML.Image = global::AgOpenGPS.Properties.Resources.BoundaryLoadFromGE;
            this.btnLoadKML.Location = new System.Drawing.Point(53, 238);
            this.btnLoadKML.Name = "btnLoadKML";
            this.btnLoadKML.Size = new System.Drawing.Size(83, 79);
            this.btnLoadKML.TabIndex = 3;
            this.btnLoadKML.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnLoadKML.UseVisualStyleBackColor = false;
            this.btnLoadKML.Click += new System.EventHandler(this.btnLoadKML_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.Enabled = false;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSave.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnSave.Location = new System.Drawing.Point(520, 248);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 79);
            this.btnSave.TabIndex = 3;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSerialCancel
            // 
            this.btnSerialCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSerialCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnSerialCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSerialCancel.FlatAppearance.BorderSize = 0;
            this.btnSerialCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSerialCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSerialCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSerialCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnSerialCancel.Location = new System.Drawing.Point(376, 247);
            this.btnSerialCancel.Name = "btnSerialCancel";
            this.btnSerialCancel.Size = new System.Drawing.Size(77, 79);
            this.btnSerialCancel.TabIndex = 4;
            this.btnSerialCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSerialCancel.UseVisualStyleBackColor = false;
            this.btnSerialCancel.Click += new System.EventHandler(this.btnSerialCancel_Click);
            // 
            // labelFieldname
            // 
            this.labelFieldname.AutoSize = true;
            this.labelFieldname.BackColor = System.Drawing.Color.Transparent;
            this.labelFieldname.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFieldname.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelFieldname.Location = new System.Drawing.Point(18, 11);
            this.labelFieldname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFieldname.Name = "labelFieldname";
            this.labelFieldname.Size = new System.Drawing.Size(174, 23);
            this.labelFieldname.TabIndex = 4;
            this.labelFieldname.Text = "Enter Field Name";
            // 
            // btnAddDate
            // 
            this.btnAddDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDate.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDate.FlatAppearance.BorderSize = 0;
            this.btnAddDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDate.Image = global::AgOpenGPS.Properties.Resources.JobNameCalendar;
            this.btnAddDate.Location = new System.Drawing.Point(46, 125);
            this.btnAddDate.Name = "btnAddDate";
            this.btnAddDate.Size = new System.Drawing.Size(83, 79);
            this.btnAddDate.TabIndex = 154;
            this.btnAddDate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddDate.UseVisualStyleBackColor = false;
            this.btnAddDate.Click += new System.EventHandler(this.btnAddDate_Click);
            // 
            // btnAddTime
            // 
            this.btnAddTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTime.Image = global::AgOpenGPS.Properties.Resources.JobNameTime;
            this.btnAddTime.Location = new System.Drawing.Point(203, 126);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(83, 79);
            this.btnAddTime.TabIndex = 153;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // FormFieldKML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(608, 332);
            this.ControlBox = false;
            this.Controls.Add(this.btnAddDate);
            this.Controls.Add(this.btnAddTime);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.labelFieldname);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tboxFieldName);
            this.Controls.Add(this.btnLoadKML);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSerialCancel);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormFieldKML";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Field ";
            this.Load += new System.EventHandler(this.FormFieldDir_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tboxFieldName;
        private System.Windows.Forms.Button btnSerialCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelFieldname;
        private System.Windows.Forms.Button btnLoadKML;
        private System.Windows.Forms.Button btnAddDate;
        private System.Windows.Forms.Button btnAddTime;
    }
}

namespace AgOpenGPS.Forms
{
    partial class FormRecordName
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
            this.labelFilename = new System.Windows.Forms.Label();
            this.labelEnterRecordName = new System.Windows.Forms.Label();
            this.tboxFieldName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBoxRecordAddTime = new System.Windows.Forms.CheckBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.checkBoxRecordAddDate = new System.Windows.Forms.CheckBox();
            this.buttonRecordCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelFilename
            // 
            this.labelFilename.AutoSize = true;
            this.labelFilename.BackColor = System.Drawing.Color.Transparent;
            this.labelFilename.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFilename.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelFilename.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelFilename.Location = new System.Drawing.Point(31, 86);
            this.labelFilename.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFilename.Name = "labelFilename";
            this.labelFilename.Size = new System.Drawing.Size(83, 19);
            this.labelFilename.TabIndex = 153;
            this.labelFilename.Text = "Filename";
            // 
            // labelEnterRecordName
            // 
            this.labelEnterRecordName.AutoSize = true;
            this.labelEnterRecordName.BackColor = System.Drawing.Color.Transparent;
            this.labelEnterRecordName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEnterRecordName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelEnterRecordName.Location = new System.Drawing.Point(31, 22);
            this.labelEnterRecordName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelEnterRecordName.Name = "labelEnterRecordName";
            this.labelEnterRecordName.Size = new System.Drawing.Size(175, 23);
            this.labelEnterRecordName.TabIndex = 152;
            this.labelEnterRecordName.Text = "Enter Record Name";
            // 
            // tboxFieldName
            // 
            this.tboxFieldName.BackColor = System.Drawing.Color.AliceBlue;
            this.tboxFieldName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFieldName.Location = new System.Drawing.Point(24, 45);
            this.tboxFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxFieldName.Name = "tboxFieldName";
            this.tboxFieldName.Size = new System.Drawing.Size(634, 36);
            this.tboxFieldName.TabIndex = 151;
            this.tboxFieldName.Click += new System.EventHandler(this.tboxFieldName_Click);
            this.tboxFieldName.TextChanged += new System.EventHandler(this.tboxFieldName_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(188, 166);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 23);
            this.label4.TabIndex = 150;
            this.label4.Text = "+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(31, 166);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(24, 23);
            this.label5.TabIndex = 149;
            this.label5.Text = "+";
            // 
            // checkBoxRecordAddTime
            // 
            this.checkBoxRecordAddTime.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRecordAddTime.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRecordAddTime.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.checkBoxRecordAddTime.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.checkBoxRecordAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxRecordAddTime.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRecordAddTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBoxRecordAddTime.Image = global::AgOpenGPS.Properties.Resources.JobNameTime;
            this.checkBoxRecordAddTime.Location = new System.Drawing.Point(219, 142);
            this.checkBoxRecordAddTime.Name = "checkBoxRecordAddTime";
            this.checkBoxRecordAddTime.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxRecordAddTime.Size = new System.Drawing.Size(86, 70);
            this.checkBoxRecordAddTime.TabIndex = 2;
            this.checkBoxRecordAddTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRecordAddTime.UseVisualStyleBackColor = false;
            // 
            // buttonSave
            // 
            this.buttonSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSave.BackColor = System.Drawing.Color.Transparent;
            this.buttonSave.FlatAppearance.BorderSize = 0;
            this.buttonSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSave.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonSave.Location = new System.Drawing.Point(579, 144);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(83, 79);
            this.buttonSave.TabIndex = 3;
            this.buttonSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonSave.UseVisualStyleBackColor = false;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // checkBoxRecordAddDate
            // 
            this.checkBoxRecordAddDate.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxRecordAddDate.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxRecordAddDate.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.checkBoxRecordAddDate.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.checkBoxRecordAddDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxRecordAddDate.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxRecordAddDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBoxRecordAddDate.Image = global::AgOpenGPS.Properties.Resources.JobNameCalendar;
            this.checkBoxRecordAddDate.Location = new System.Drawing.Point(62, 142);
            this.checkBoxRecordAddDate.Name = "checkBoxRecordAddDate";
            this.checkBoxRecordAddDate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxRecordAddDate.Size = new System.Drawing.Size(86, 70);
            this.checkBoxRecordAddDate.TabIndex = 1;
            this.checkBoxRecordAddDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxRecordAddDate.UseVisualStyleBackColor = false;
            // 
            // buttonRecordCancel
            // 
            this.buttonRecordCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonRecordCancel.BackColor = System.Drawing.Color.Transparent;
            this.buttonRecordCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonRecordCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonRecordCancel.FlatAppearance.BorderSize = 0;
            this.buttonRecordCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRecordCancel.Font = new System.Drawing.Font("Tahoma", 12F);
            this.buttonRecordCancel.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonRecordCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.buttonRecordCancel.Location = new System.Drawing.Point(458, 143);
            this.buttonRecordCancel.Name = "buttonRecordCancel";
            this.buttonRecordCancel.Size = new System.Drawing.Size(77, 79);
            this.buttonRecordCancel.TabIndex = 4;
            this.buttonRecordCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonRecordCancel.UseVisualStyleBackColor = false;
            this.buttonRecordCancel.Click += new System.EventHandler(this.buttonRecordCancel_Click);
            // 
            // FormRecordName
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(676, 234);
            this.ControlBox = false;
            this.Controls.Add(this.labelFilename);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.labelEnterRecordName);
            this.Controls.Add(this.tboxFieldName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.checkBoxRecordAddTime);
            this.Controls.Add(this.buttonRecordCancel);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.checkBoxRecordAddDate);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormRecordName";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Record";
            this.Load += new System.EventHandler(this.FormRecordName_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label labelFilename;
        private System.Windows.Forms.Label labelEnterRecordName;
        private System.Windows.Forms.TextBox tboxFieldName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBoxRecordAddTime;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.CheckBox checkBoxRecordAddDate;
        private System.Windows.Forms.Button buttonRecordCancel;
    }
}
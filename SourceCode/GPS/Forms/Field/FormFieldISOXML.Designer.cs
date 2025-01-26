namespace AgOpenGPS
{
    partial class FormFieldISOXML
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
            this.labelFieldname = new System.Windows.Forms.Label();
            this.btnSerialCancel = new System.Windows.Forms.Button();
            this.labelField = new System.Windows.Forms.Label();
            this.tboxFieldName = new System.Windows.Forms.TextBox();
            this.btnBuildFields = new System.Windows.Forms.Button();
            this.tree = new System.Windows.Forms.TreeView();
            this.btnAddDate = new System.Windows.Forms.Button();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelFieldname
            // 
            this.labelFieldname.BackColor = System.Drawing.Color.Transparent;
            this.labelFieldname.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFieldname.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelFieldname.Location = new System.Drawing.Point(6, 54);
            this.labelFieldname.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelFieldname.Name = "labelFieldname";
            this.labelFieldname.Size = new System.Drawing.Size(296, 23);
            this.labelFieldname.TabIndex = 4;
            this.labelFieldname.Text = "Field Name";
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
            this.btnSerialCancel.Location = new System.Drawing.Point(524, 303);
            this.btnSerialCancel.Name = "btnSerialCancel";
            this.btnSerialCancel.Size = new System.Drawing.Size(83, 79);
            this.btnSerialCancel.TabIndex = 4;
            this.btnSerialCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSerialCancel.UseVisualStyleBackColor = false;
            this.btnSerialCancel.Click += new System.EventHandler(this.btnSerialCancel_Click);
            // 
            // labelField
            // 
            this.labelField.AutoSize = true;
            this.labelField.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelField.Location = new System.Drawing.Point(5, 140);
            this.labelField.Name = "labelField";
            this.labelField.Size = new System.Drawing.Size(119, 25);
            this.labelField.TabIndex = 155;
            this.labelField.Text = "Fieldname";
            // 
            // tboxFieldName
            // 
            this.tboxFieldName.Enabled = false;
            this.tboxFieldName.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFieldName.Location = new System.Drawing.Point(5, 15);
            this.tboxFieldName.Name = "tboxFieldName";
            this.tboxFieldName.Size = new System.Drawing.Size(602, 33);
            this.tboxFieldName.TabIndex = 156;
            this.tboxFieldName.Click += new System.EventHandler(this.tboxFieldName_Click);
            this.tboxFieldName.TextChanged += new System.EventHandler(this.tboxFieldName_TextChanged);
            // 
            // btnBuildFields
            // 
            this.btnBuildFields.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBuildFields.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildFields.Enabled = false;
            this.btnBuildFields.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnBuildFields.FlatAppearance.BorderSize = 0;
            this.btnBuildFields.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuildFields.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBuildFields.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.btnBuildFields.Location = new System.Drawing.Point(524, 414);
            this.btnBuildFields.Name = "btnBuildFields";
            this.btnBuildFields.Size = new System.Drawing.Size(83, 79);
            this.btnBuildFields.TabIndex = 158;
            this.btnBuildFields.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBuildFields.UseVisualStyleBackColor = false;
            this.btnBuildFields.Click += new System.EventHandler(this.btnBuildFields_Click);
            // 
            // tree
            // 
            this.tree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tree.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tree.Location = new System.Drawing.Point(7, 166);
            this.tree.Name = "tree";
            this.tree.Size = new System.Drawing.Size(499, 328);
            this.tree.TabIndex = 161;
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            // 
            // btnAddDate
            // 
            this.btnAddDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddDate.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDate.Enabled = false;
            this.btnAddDate.FlatAppearance.BorderSize = 0;
            this.btnAddDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDate.Image = global::AgOpenGPS.Properties.Resources.JobNameCalendar;
            this.btnAddDate.Location = new System.Drawing.Point(358, 54);
            this.btnAddDate.Name = "btnAddDate";
            this.btnAddDate.Size = new System.Drawing.Size(83, 66);
            this.btnAddDate.TabIndex = 163;
            this.btnAddDate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddDate.UseVisualStyleBackColor = false;
            this.btnAddDate.Click += new System.EventHandler(this.btnAddDate_Click);
            // 
            // btnAddTime
            // 
            this.btnAddTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.Enabled = false;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTime.Image = global::AgOpenGPS.Properties.Resources.JobNameTime;
            this.btnAddTime.Location = new System.Drawing.Point(524, 54);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(83, 66);
            this.btnAddTime.TabIndex = 162;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(336, 76);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 23);
            this.label2.TabIndex = 164;
            this.label2.Text = "+";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(506, 76);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 23);
            this.label3.TabIndex = 165;
            this.label3.Text = "+";
            // 
            // FormFieldISOXML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(614, 524);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnAddDate);
            this.Controls.Add(this.btnAddTime);
            this.Controls.Add(this.tree);
            this.Controls.Add(this.btnBuildFields);
            this.Controls.Add(this.tboxFieldName);
            this.Controls.Add(this.labelField);
            this.Controls.Add(this.labelFieldname);
            this.Controls.Add(this.btnSerialCancel);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MinimumSize = new System.Drawing.Size(630, 540);
            this.Name = "FormFieldISOXML";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Field From ISO-XML";
            this.Load += new System.EventHandler(this.FormFieldISOXML_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSerialCancel;
        private System.Windows.Forms.Label labelFieldname;
        private System.Windows.Forms.Label labelField;
        private System.Windows.Forms.TextBox tboxFieldName;
        private System.Windows.Forms.Button btnBuildFields;
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.Button btnAddDate;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
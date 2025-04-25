namespace AgOpenGPS.Forms.Field

{
    partial class FormFieldBrowser
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.glControl = new OpenTK.GLControl();
            this.lstFields = new System.Windows.Forms.ListView();
            this.fieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.fieldDate = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnUseField = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(12, 9);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(38, 13);
            this.lblName.TabIndex = 1;
            this.lblName.Text = "Name:";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(163, 9);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(33, 13);
            this.lblDate.TabIndex = 2;
            this.lblDate.Text = "Date:";
            // 
            // glControl
            // 
            this.glControl.BackColor = System.Drawing.Color.White;
            this.glControl.Location = new System.Drawing.Point(607, 29);
            this.glControl.Name = "glControl";
            this.glControl.Size = new System.Drawing.Size(403, 458);
            this.glControl.TabIndex = 3;
            this.glControl.VSync = false;
            // 
            // lstFields
            // 
            this.lstFields.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lstFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.fieldName,
            this.fieldDate});
            this.lstFields.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstFields.FullRowSelect = true;
            this.lstFields.GridLines = true;
            this.lstFields.HideSelection = false;
            this.lstFields.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lstFields.Location = new System.Drawing.Point(12, 29);
            this.lstFields.MultiSelect = false;
            this.lstFields.Name = "lstFields";
            this.lstFields.Size = new System.Drawing.Size(589, 566);
            this.lstFields.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lstFields.TabIndex = 275;
            this.lstFields.UseCompatibleStateImageBehavior = false;
            this.lstFields.View = System.Windows.Forms.View.Details;
            this.lstFields.SelectedIndexChanged += new System.EventHandler(this.lstFields_SelectedIndexChanged);
            // 
            // fieldName
            // 
            this.fieldName.Text = "Field";
            this.fieldName.Width = 400;
            // 
            // fieldDate
            // 
            this.fieldDate.Text = "Date";
            this.fieldDate.Width = 185;
            // 
            // btnUseField
            // 
            this.btnUseField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUseField.Location = new System.Drawing.Point(607, 493);
            this.btnUseField.Name = "btnUseField";
            this.btnUseField.Size = new System.Drawing.Size(203, 72);
            this.btnUseField.TabIndex = 276;
            this.btnUseField.Text = "Use Field";
            this.btnUseField.UseVisualStyleBackColor = true;
            this.btnUseField.Click += new System.EventHandler(this.btnUseField_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(204, 264);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(173, 55);
            this.lblStatus.TabIndex = 277;
            this.lblStatus.Text = "Status:";
            this.lblStatus.Visible = false;
            // 
            // FormFieldBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 661);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnUseField);
            this.Controls.Add(this.lstFields);
            this.Controls.Add(this.glControl);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.lblName);
            this.Name = "FormFieldBrowser";
            this.Text = "FormFieldBrowser";
            this.Load += new System.EventHandler(this.FormFieldBrowser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblDate;
        private OpenTK.GLControl glControl;
        private System.Windows.Forms.ListView lstFields;
        private System.Windows.Forms.ColumnHeader fieldName;
        private System.Windows.Forms.ColumnHeader fieldDate;
        private System.Windows.Forms.Button btnUseField;
        private System.Windows.Forms.Label lblStatus;
    }
}
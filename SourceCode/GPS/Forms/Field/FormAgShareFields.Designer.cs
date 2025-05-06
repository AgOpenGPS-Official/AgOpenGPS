namespace AgOpenGPS.Forms.Field
{
    partial class FormAgShareFields
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
            this.buttonPreview = new System.Windows.Forms.Button();
            this.labelSelectedField = new System.Windows.Forms.Label();
            this.buttonSaveAndUse = new System.Windows.Forms.Button();
            this.glControlPreview = new OpenTK.GLControl();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewFields = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // buttonPreview
            // 
            this.buttonPreview.Location = new System.Drawing.Point(12, 413);
            this.buttonPreview.Name = "buttonPreview";
            this.buttonPreview.Size = new System.Drawing.Size(122, 37);
            this.buttonPreview.TabIndex = 0;
            this.buttonPreview.Text = "Preview";
            this.buttonPreview.UseVisualStyleBackColor = true;
            this.buttonPreview.Click += new System.EventHandler(this.buttonPreview_Click);
            // 
            // labelSelectedField
            // 
            this.labelSelectedField.AutoSize = true;
            this.labelSelectedField.Location = new System.Drawing.Point(12, 9);
            this.labelSelectedField.Name = "labelSelectedField";
            this.labelSelectedField.Size = new System.Drawing.Size(93, 13);
            this.labelSelectedField.TabIndex = 1;
            this.labelSelectedField.Text = "labelSelectedField";
            // 
            // buttonSaveAndUse
            // 
            this.buttonSaveAndUse.Location = new System.Drawing.Point(397, 413);
            this.buttonSaveAndUse.Name = "buttonSaveAndUse";
            this.buttonSaveAndUse.Size = new System.Drawing.Size(122, 37);
            this.buttonSaveAndUse.TabIndex = 276;
            this.buttonSaveAndUse.Text = "Save and Use";
            this.buttonSaveAndUse.UseVisualStyleBackColor = true;
            this.buttonSaveAndUse.Click += new System.EventHandler(this.buttonSaveAndUse_Click);
            // 
            // glControlPreview
            // 
            this.glControlPreview.BackColor = System.Drawing.Color.Black;
            this.glControlPreview.Location = new System.Drawing.Point(526, 9);
            this.glControlPreview.Name = "glControlPreview";
            this.glControlPreview.Size = new System.Drawing.Size(479, 441);
            this.glControlPreview.TabIndex = 277;
            this.glControlPreview.VSync = false;
            // 
            // chName
            // 
            this.chName.Text = "Field Name";
            this.chName.Width = 500;
            // 
            // listViewFields
            // 
            this.listViewFields.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.listViewFields.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewFields.FullRowSelect = true;
            this.listViewFields.GridLines = true;
            this.listViewFields.HideSelection = false;
            this.listViewFields.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.listViewFields.Location = new System.Drawing.Point(12, 25);
            this.listViewFields.MultiSelect = false;
            this.listViewFields.Name = "listViewFields";
            this.listViewFields.Size = new System.Drawing.Size(507, 384);
            this.listViewFields.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.listViewFields.TabIndex = 275;
            this.listViewFields.UseCompatibleStateImageBehavior = false;
            this.listViewFields.View = System.Windows.Forms.View.Details;
            this.listViewFields.SelectedIndexChanged += new System.EventHandler(this.listViewFields_SelectedIndexChanged);
            // 
            // FormAgShareFields
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1005, 450);
            this.Controls.Add(this.glControlPreview);
            this.Controls.Add(this.buttonSaveAndUse);
            this.Controls.Add(this.listViewFields);
            this.Controls.Add(this.labelSelectedField);
            this.Controls.Add(this.buttonPreview);
            this.Name = "FormAgShareFields";
            this.Text = "FormAgShareFields";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonPreview;
        private System.Windows.Forms.Label labelSelectedField;
        private System.Windows.Forms.Button buttonSaveAndUse;
        private OpenTK.GLControl glControlPreview;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ListView listViewFields;
    }
}
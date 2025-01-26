
namespace AgOpenGPS.Forms.Pickers
{
    partial class FormRecordPicker
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
            this.lvLines = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnDeleteField = new System.Windows.Forms.Button();
            this.labelDeleteRecord = new System.Windows.Forms.Label();
            this.btnDeleteAB = new System.Windows.Forms.Button();
            this.labelCancel = new System.Windows.Forms.Label();
            this.buttonOpenExistingLv = new System.Windows.Forms.Button();
            this.btnTurnOffRecPath = new System.Windows.Forms.Button();
            this.labelPathOff = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvLines
            // 
            this.lvLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLines.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.Location = new System.Drawing.Point(5, 12);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(967, 459);
            this.lvLines.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvLines.TabIndex = 87;
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Record Name";
            this.chName.Width = 680;
            // 
            // btnDeleteField
            // 
            this.btnDeleteField.FlatAppearance.BorderSize = 0;
            this.btnDeleteField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteField.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteField.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteField.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteField.Location = new System.Drawing.Point(47, 503);
            this.btnDeleteField.Name = "btnDeleteField";
            this.btnDeleteField.Size = new System.Drawing.Size(71, 63);
            this.btnDeleteField.TabIndex = 95;
            this.btnDeleteField.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteField.Click += new System.EventHandler(this.btnDeleteField_Click);
            // 
            // labelDeleteRecord
            // 
            this.labelDeleteRecord.AutoSize = true;
            this.labelDeleteRecord.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeleteRecord.Location = new System.Drawing.Point(41, 489);
            this.labelDeleteRecord.Name = "labelDeleteRecord";
            this.labelDeleteRecord.Size = new System.Drawing.Size(87, 16);
            this.labelDeleteRecord.TabIndex = 96;
            this.labelDeleteRecord.Text = "Delete Record";
            // 
            // btnDeleteAB
            // 
            this.btnDeleteAB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeleteAB.FlatAppearance.BorderSize = 0;
            this.btnDeleteAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAB.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteAB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteAB.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnDeleteAB.Location = new System.Drawing.Point(565, 503);
            this.btnDeleteAB.Name = "btnDeleteAB";
            this.btnDeleteAB.Size = new System.Drawing.Size(71, 63);
            this.btnDeleteAB.TabIndex = 97;
            this.btnDeleteAB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelCancel
            // 
            this.labelCancel.AutoSize = true;
            this.labelCancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCancel.Location = new System.Drawing.Point(576, 489);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(45, 16);
            this.labelCancel.TabIndex = 98;
            this.labelCancel.Text = "Cancel";
            // 
            // buttonOpenExistingLv
            // 
            this.buttonOpenExistingLv.BackColor = System.Drawing.Color.Transparent;
            this.buttonOpenExistingLv.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.buttonOpenExistingLv.FlatAppearance.BorderSize = 0;
            this.buttonOpenExistingLv.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOpenExistingLv.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpenExistingLv.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.buttonOpenExistingLv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.buttonOpenExistingLv.Location = new System.Drawing.Point(702, 502);
            this.buttonOpenExistingLv.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.buttonOpenExistingLv.Name = "buttonOpenExistingLv";
            this.buttonOpenExistingLv.Size = new System.Drawing.Size(261, 63);
            this.buttonOpenExistingLv.TabIndex = 99;
            this.buttonOpenExistingLv.Text = " Use Selected";
            this.buttonOpenExistingLv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonOpenExistingLv.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOpenExistingLv.UseVisualStyleBackColor = false;
            this.buttonOpenExistingLv.Click += new System.EventHandler(this.btnOpenExistingLv_Click);
            // 
            // btnTurnOffRecPath
            // 
            this.btnTurnOffRecPath.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnTurnOffRecPath.FlatAppearance.BorderSize = 0;
            this.btnTurnOffRecPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnOffRecPath.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnTurnOffRecPath.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnTurnOffRecPath.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnTurnOffRecPath.Location = new System.Drawing.Point(223, 502);
            this.btnTurnOffRecPath.Name = "btnTurnOffRecPath";
            this.btnTurnOffRecPath.Size = new System.Drawing.Size(71, 63);
            this.btnTurnOffRecPath.TabIndex = 100;
            this.btnTurnOffRecPath.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTurnOffRecPath.Click += new System.EventHandler(this.btnTurnOffRecPath_Click);
            // 
            // labelPathOff
            // 
            this.labelPathOff.AutoSize = true;
            this.labelPathOff.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPathOff.Location = new System.Drawing.Point(230, 489);
            this.labelPathOff.Name = "labelPathOff";
            this.labelPathOff.Size = new System.Drawing.Size(53, 16);
            this.labelPathOff.TabIndex = 101;
            this.labelPathOff.Text = "Path Off";
            // 
            // FormRecordPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 578);
            this.ControlBox = false;
            this.Controls.Add(this.labelPathOff);
            this.Controls.Add(this.btnTurnOffRecPath);
            this.Controls.Add(this.buttonOpenExistingLv);
            this.Controls.Add(this.labelCancel);
            this.Controls.Add(this.btnDeleteAB);
            this.Controls.Add(this.labelDeleteRecord);
            this.Controls.Add(this.btnDeleteField);
            this.Controls.Add(this.lvLines);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormRecordPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormRecordPicker";
            this.Load += new System.EventHandler(this.FormRecordPicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.Button btnDeleteField;
        private System.Windows.Forms.Label labelDeleteRecord;
        private System.Windows.Forms.Button btnDeleteAB;
        private System.Windows.Forms.Label labelCancel;
        private System.Windows.Forms.Button buttonOpenExistingLv;
        private System.Windows.Forms.Button btnTurnOffRecPath;
        private System.Windows.Forms.Label labelPathOff;
    }
}
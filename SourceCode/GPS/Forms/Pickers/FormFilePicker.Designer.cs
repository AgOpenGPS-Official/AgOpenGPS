namespace AgOpenGPS
{
    partial class FormFilePicker
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
            this.components = new System.ComponentModel.Container();
            this.lvLines = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDistance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.btnByDistance = new System.Windows.Forms.Button();
            this.btnOpenExistingLv = new System.Windows.Forms.Button();
            this.btnDeleteAB = new System.Windows.Forms.Button();
            this.btnDeleteField = new System.Windows.Forms.Button();
            this.labelDeleteField = new System.Windows.Forms.Label();
            this.labelCancel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lvLines
            // 
            this.lvLines.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLines.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName,
            this.chDistance,
            this.chArea});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.Location = new System.Drawing.Point(5, 12);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(986, 459);
            this.lvLines.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvLines.TabIndex = 86;
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Details;
            // 
            // chName
            // 
            this.chName.Text = "Field";
            this.chName.Width = 680;
            // 
            // chDistance
            // 
            this.chDistance.Text = "Distance";
            this.chDistance.Width = 140;
            // 
            // chArea
            // 
            this.chArea.Text = "Area";
            this.chArea.Width = 140;
            // 
            // timer1
            // 
            this.timer1.Interval = 300;
            // 
            // btnByDistance
            // 
            this.btnByDistance.BackColor = System.Drawing.Color.Transparent;
            this.btnByDistance.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnByDistance.Image = global::AgOpenGPS.Properties.Resources.Sort;
            this.btnByDistance.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnByDistance.Location = new System.Drawing.Point(210, 507);
            this.btnByDistance.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnByDistance.Name = "btnByDistance";
            this.btnByDistance.Size = new System.Drawing.Size(147, 63);
            this.btnByDistance.TabIndex = 93;
            this.btnByDistance.Text = "Sort";
            this.btnByDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnByDistance.UseVisualStyleBackColor = false;
            this.btnByDistance.Click += new System.EventHandler(this.btnByDistance_Click);
            // 
            // btnOpenExistingLv
            // 
            this.btnOpenExistingLv.BackColor = System.Drawing.Color.Transparent;
            this.btnOpenExistingLv.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnOpenExistingLv.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenExistingLv.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.btnOpenExistingLv.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnOpenExistingLv.Location = new System.Drawing.Point(697, 507);
            this.btnOpenExistingLv.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOpenExistingLv.Name = "btnOpenExistingLv";
            this.btnOpenExistingLv.Size = new System.Drawing.Size(261, 63);
            this.btnOpenExistingLv.TabIndex = 92;
            this.btnOpenExistingLv.Text = "Use Selected";
            this.btnOpenExistingLv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnOpenExistingLv.UseVisualStyleBackColor = false;
            this.btnOpenExistingLv.Click += new System.EventHandler(this.btnOpenExistingLv_Click);
            // 
            // btnDeleteAB
            // 
            this.btnDeleteAB.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnDeleteAB.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteAB.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteAB.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnDeleteAB.Location = new System.Drawing.Point(502, 507);
            this.btnDeleteAB.Name = "btnDeleteAB";
            this.btnDeleteAB.Size = new System.Drawing.Size(71, 63);
            this.btnDeleteAB.TabIndex = 91;
            this.btnDeleteAB.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteAB.Click += new System.EventHandler(this.btnDeleteAB_Click);
            // 
            // btnDeleteField
            // 
            this.btnDeleteField.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnDeleteField.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnDeleteField.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteField.Location = new System.Drawing.Point(46, 507);
            this.btnDeleteField.Name = "btnDeleteField";
            this.btnDeleteField.Size = new System.Drawing.Size(71, 63);
            this.btnDeleteField.TabIndex = 94;
            this.btnDeleteField.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteField.Click += new System.EventHandler(this.btnDeleteField_Click);
            // 
            // labelDeleteField
            // 
            this.labelDeleteField.AutoSize = true;
            this.labelDeleteField.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeleteField.Location = new System.Drawing.Point(43, 488);
            this.labelDeleteField.Name = "labelDeleteField";
            this.labelDeleteField.Size = new System.Drawing.Size(74, 16);
            this.labelDeleteField.TabIndex = 95;
            this.labelDeleteField.Text = "Delete Field";
            // 
            // labelCancel
            // 
            this.labelCancel.AutoSize = true;
            this.labelCancel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCancel.Location = new System.Drawing.Point(515, 488);
            this.labelCancel.Name = "labelCancel";
            this.labelCancel.Size = new System.Drawing.Size(45, 16);
            this.labelCancel.TabIndex = 96;
            this.labelCancel.Text = "Cancel";
            // 
            // FormFilePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(991, 578);
            this.ControlBox = false;
            this.Controls.Add(this.labelCancel);
            this.Controls.Add(this.labelDeleteField);
            this.Controls.Add(this.btnDeleteField);
            this.Controls.Add(this.btnByDistance);
            this.Controls.Add(this.btnOpenExistingLv);
            this.Controls.Add(this.btnDeleteAB);
            this.Controls.Add(this.lvLines);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormFilePicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "FormFilePicker";
            this.Load += new System.EventHandler(this.FormFilePicker_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chDistance;
        private System.Windows.Forms.Button btnByDistance;
        private System.Windows.Forms.Button btnOpenExistingLv;
        private System.Windows.Forms.Button btnDeleteAB;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ColumnHeader chArea;
        private System.Windows.Forms.Button btnDeleteField;
        private System.Windows.Forms.Label labelDeleteField;
        private System.Windows.Forms.Label labelCancel;
    }
}
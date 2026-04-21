namespace AgOpenGPS.Forms.Field
{
    partial class FormCopyTracks
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
            this.lblTracksHeader = new System.Windows.Forms.Label();
            this.lblFieldsHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnCopyToCurrentField = new System.Windows.Forms.Button();
            this.btnDeselectAllTracks = new System.Windows.Forms.Button();
            this.btnSelectAllTracks = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lvTracks = new System.Windows.Forms.ListView();
            this.chTrackName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbFields = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Controls.Add(this.lblTracksHeader);
            this.panel1.Controls.Add(this.lblFieldsHeader);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnCopyToCurrentField);
            this.panel1.Controls.Add(this.btnDeselectAllTracks);
            this.panel1.Controls.Add(this.btnSelectAllTracks);
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.lvTracks);
            this.panel1.Controls.Add(this.lbFields);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(990, 690);
            this.panel1.TabIndex = 0;
            // 
            // lblTracksHeader
            // 
            this.lblTracksHeader.AutoSize = true;
            this.lblTracksHeader.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTracksHeader.Location = new System.Drawing.Point(495, 5);
            this.lblTracksHeader.Name = "lblTracksHeader";
            this.lblTracksHeader.Size = new System.Drawing.Size(165, 23);
            this.lblTracksHeader.TabIndex = 13;
            this.lblTracksHeader.Text = "Available Tracks";
            // 
            // lblFieldsHeader
            // 
            this.lblFieldsHeader.AutoSize = true;
            this.lblFieldsHeader.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFieldsHeader.Location = new System.Drawing.Point(10, 5);
            this.lblFieldsHeader.Name = "lblFieldsHeader";
            this.lblFieldsHeader.Size = new System.Drawing.Size(159, 23);
            this.lblFieldsHeader.TabIndex = 12;
            this.lblFieldsHeader.Text = "Available Fields";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnClose.Location = new System.Drawing.Point(880, 590);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 90);
            this.btnClose.TabIndex = 11;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnCopyToCurrentField
            // 
            this.btnCopyToCurrentField.BackColor = System.Drawing.Color.Transparent;
            this.btnCopyToCurrentField.FlatAppearance.BorderSize = 0;
            this.btnCopyToCurrentField.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCopyToCurrentField.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCopyToCurrentField.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnCopyToCurrentField.Location = new System.Drawing.Point(770, 590);
            this.btnCopyToCurrentField.Name = "btnCopyToCurrentField";
            this.btnCopyToCurrentField.Size = new System.Drawing.Size(100, 90);
            this.btnCopyToCurrentField.TabIndex = 10;
            this.btnCopyToCurrentField.Text = "Import";
            this.btnCopyToCurrentField.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCopyToCurrentField.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnCopyToCurrentField.UseVisualStyleBackColor = false;
            this.btnCopyToCurrentField.Click += new System.EventHandler(this.btnCopyToCurrentField_Click);
            // 
            // btnDeselectAllTracks
            // 
            this.btnDeselectAllTracks.BackColor = System.Drawing.Color.Transparent;
            this.btnDeselectAllTracks.FlatAppearance.BorderSize = 0;
            this.btnDeselectAllTracks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeselectAllTracks.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDeselectAllTracks.Image = global::AgOpenGPS.Properties.Resources.DeselectAll;
            this.btnDeselectAllTracks.Location = new System.Drawing.Point(600, 590);
            this.btnDeselectAllTracks.Name = "btnDeselectAllTracks";
            this.btnDeselectAllTracks.Size = new System.Drawing.Size(100, 90);
            this.btnDeselectAllTracks.TabIndex = 9;
            this.btnDeselectAllTracks.Text = "Deselect All";
            this.btnDeselectAllTracks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnDeselectAllTracks.UseVisualStyleBackColor = false;
            this.btnDeselectAllTracks.Click += new System.EventHandler(this.btnDeselectAllTracks_Click);
            // 
            // btnSelectAllTracks
            // 
            this.btnSelectAllTracks.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectAllTracks.FlatAppearance.BorderSize = 0;
            this.btnSelectAllTracks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectAllTracks.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectAllTracks.Image = global::AgOpenGPS.Properties.Resources.SelectAll;
            this.btnSelectAllTracks.Location = new System.Drawing.Point(495, 590);
            this.btnSelectAllTracks.Name = "btnSelectAllTracks";
            this.btnSelectAllTracks.Size = new System.Drawing.Size(100, 90);
            this.btnSelectAllTracks.TabIndex = 8;
            this.btnSelectAllTracks.Text = "Select All";
            this.btnSelectAllTracks.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSelectAllTracks.UseVisualStyleBackColor = false;
            this.btnSelectAllTracks.Click += new System.EventHandler(this.btnSelectAllTracks_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(10, 595);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(470, 50);
            this.lblStatus.TabIndex = 4;
            this.lblStatus.Text = "Select a field from the left, then select tracks to copy";
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lvTracks
            // 
            this.lvTracks.BackColor = System.Drawing.Color.AliceBlue;
            this.lvTracks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chTrackName});
            this.lvTracks.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvTracks.FullRowSelect = true;
            this.lvTracks.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.lvTracks.Location = new System.Drawing.Point(495, 32);
            this.lvTracks.MultiSelect = false;
            this.lvTracks.Name = "lvTracks";
            this.lvTracks.Size = new System.Drawing.Size(485, 550);
            this.lvTracks.TabIndex = 2;
            this.lvTracks.UseCompatibleStateImageBehavior = false;
            this.lvTracks.View = System.Windows.Forms.View.Details;
            this.lvTracks.MouseClick += new System.Windows.Forms.MouseEventHandler(this.lvTracks_MouseClick);
            // 
            // lbFields
            // 
            this.lbFields.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lbFields.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chName});
            this.lbFields.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbFields.FullRowSelect = true;
            this.lbFields.GridLines = true;
            this.lbFields.HideSelection = false;
            this.lbFields.Location = new System.Drawing.Point(10, 32);
            this.lbFields.MultiSelect = false;
            this.lbFields.Name = "lbFields";
            this.lbFields.Size = new System.Drawing.Size(470, 550);
            this.lbFields.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lbFields.TabIndex = 1;
            this.lbFields.UseCompatibleStateImageBehavior = false;
            this.lbFields.View = System.Windows.Forms.View.Details;
            this.lbFields.SelectedIndexChanged += new System.EventHandler(this.lbFields_SelectedIndexChanged);
            // 
            // chName
            // 
            this.chName.Text = "Field Name";
            this.chName.Width = 452;
            // 
            // FormCopyTracks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Name = "FormCopyTracks";
            this.Padding = new System.Windows.Forms.Padding(5);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Import Tracks From Another Field";
            this.Load += new System.EventHandler(this.FormCopyTracks_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ListView lbFields;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ListView lvTracks;
        private System.Windows.Forms.ColumnHeader chTrackName;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnSelectAllTracks;
        private System.Windows.Forms.Button btnDeselectAllTracks;
        private System.Windows.Forms.Button btnCopyToCurrentField;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblFieldsHeader;
        private System.Windows.Forms.Label lblTracksHeader;
    }
}

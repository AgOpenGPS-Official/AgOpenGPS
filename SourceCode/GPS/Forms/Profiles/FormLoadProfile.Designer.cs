namespace AgOpenGPS.Forms.Profiles
{
    partial class FormLoadProfile
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
            this.listViewProfiles = new System.Windows.Forms.ListView();
            this.columnHeaderProfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.labelLoadProfile = new System.Windows.Forms.Label();
            this.buttonProfileDelete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewProfiles
            // 
            this.listViewProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewProfiles.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderProfile});
            this.listViewProfiles.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewProfiles.FullRowSelect = true;
            this.listViewProfiles.GridLines = true;
            this.listViewProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewProfiles.HideSelection = false;
            this.listViewProfiles.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.listViewProfiles.LabelWrap = false;
            this.listViewProfiles.Location = new System.Drawing.Point(9, 63);
            this.listViewProfiles.Margin = new System.Windows.Forms.Padding(0);
            this.listViewProfiles.MultiSelect = false;
            this.listViewProfiles.Name = "listViewProfiles";
            this.listViewProfiles.Size = new System.Drawing.Size(534, 254);
            this.listViewProfiles.TabIndex = 507;
            this.listViewProfiles.TileSize = new System.Drawing.Size(490, 35);
            this.listViewProfiles.UseCompatibleStateImageBehavior = false;
            this.listViewProfiles.View = System.Windows.Forms.View.Details;
            this.listViewProfiles.SelectedIndexChanged += new System.EventHandler(this.listViewProfiles_SelectedIndexChanged);
            // 
            // columnHeaderProfile
            // 
            this.columnHeaderProfile.Text = "Profiles";
            this.columnHeaderProfile.Width = 500;
            // 
            // buttonCancel
            // 
            this.buttonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.buttonCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonCancel.Location = new System.Drawing.Point(461, 333);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 92);
            this.buttonCancel.TabIndex = 509;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // buttonLoad
            // 
            this.buttonLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonLoad.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonLoad.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonLoad.Enabled = false;
            this.buttonLoad.FlatAppearance.BorderSize = 0;
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoad.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonLoad.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonLoad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonLoad.Location = new System.Drawing.Point(546, 333);
            this.buttonLoad.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(80, 92);
            this.buttonLoad.TabIndex = 508;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonLoad.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // labelLoadProfile
            // 
            this.labelLoadProfile.BackColor = System.Drawing.Color.Transparent;
            this.labelLoadProfile.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLoadProfile.ForeColor = System.Drawing.Color.Black;
            this.labelLoadProfile.Location = new System.Drawing.Point(31, 26);
            this.labelLoadProfile.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelLoadProfile.Name = "labelLoadProfile";
            this.labelLoadProfile.Size = new System.Drawing.Size(198, 23);
            this.labelLoadProfile.TabIndex = 510;
            this.labelLoadProfile.Text = "Load Profile:";
            this.labelLoadProfile.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // buttonProfileDelete
            // 
            this.buttonProfileDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonProfileDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonProfileDelete.Enabled = false;
            this.buttonProfileDelete.FlatAppearance.BorderSize = 0;
            this.buttonProfileDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonProfileDelete.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.buttonProfileDelete.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.buttonProfileDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonProfileDelete.Location = new System.Drawing.Point(546, 63);
            this.buttonProfileDelete.Name = "buttonProfileDelete";
            this.buttonProfileDelete.Size = new System.Drawing.Size(80, 92);
            this.buttonProfileDelete.TabIndex = 511;
            this.buttonProfileDelete.Text = "Delete";
            this.buttonProfileDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonProfileDelete.UseVisualStyleBackColor = false;
            this.buttonProfileDelete.Click += new System.EventHandler(this.buttonProfileDelete_Click);
            // 
            // FormLoadProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(639, 437);
            this.ControlBox = false;
            this.Controls.Add(this.buttonProfileDelete);
            this.Controls.Add(this.labelLoadProfile);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonLoad);
            this.Controls.Add(this.listViewProfiles);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(650, 360);
            this.Name = "FormLoadProfile";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Profile";
            this.Load += new System.EventHandler(this.FormLoadProfile_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewProfiles;
        private System.Windows.Forms.ColumnHeader columnHeaderProfile;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Label labelLoadProfile;
        private System.Windows.Forms.Button buttonProfileDelete;
    }
}
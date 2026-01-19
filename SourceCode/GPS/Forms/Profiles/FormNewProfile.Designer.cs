namespace AgOpenGPS.Forms.Profiles
{
    partial class FormNewProfile
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
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelName = new System.Windows.Forms.Label();
            this.buttonCreate = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.listViewProfiles = new System.Windows.Forms.ListView();
            this.columnHeaderProfile = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeaderCurrent = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.labelCopyFrom = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // textBoxName
            // 
            this.textBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxName.BackColor = System.Drawing.Color.White;
            this.textBoxName.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxName.Location = new System.Drawing.Point(160, 20);
            this.textBoxName.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(435, 33);
            this.textBoxName.TabIndex = 498;
            this.textBoxName.Click += new System.EventHandler(this.textBoxName_Click);
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // labelName
            // 
            this.labelName.BackColor = System.Drawing.Color.Transparent;
            this.labelName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.ForeColor = System.Drawing.Color.Black;
            this.labelName.Location = new System.Drawing.Point(15, 25);
            this.labelName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(135, 23);
            this.labelName.TabIndex = 499;
            this.labelName.Text = "Name:";
            this.labelName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // buttonCreate
            // 
            this.buttonCreate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.buttonCreate.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonCreate.Enabled = false;
            this.buttonCreate.FlatAppearance.BorderSize = 0;
            this.buttonCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCreate.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonCreate.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonCreate.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonCreate.Location = new System.Drawing.Point(546, 268);
            this.buttonCreate.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.buttonCreate.Name = "buttonCreate";
            this.buttonCreate.Size = new System.Drawing.Size(80, 92);
            this.buttonCreate.TabIndex = 500;
            this.buttonCreate.Text = "Create";
            this.buttonCreate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCreate.Click += new System.EventHandler(this.buttonOK_Click);
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
            this.buttonCancel.Location = new System.Drawing.Point(461, 268);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 92);
            this.buttonCancel.TabIndex = 501;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonCancel.UseVisualStyleBackColor = false;
            // 
            // listViewProfiles
            // 
            this.listViewProfiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.listViewProfiles.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewProfiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderProfile,
            this.columnHeaderCurrent});
            this.listViewProfiles.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewProfiles.FullRowSelect = true;
            this.listViewProfiles.GridLines = true;
            this.listViewProfiles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewProfiles.HideSelection = false;
            this.listViewProfiles.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.listViewProfiles.LabelWrap = false;
            this.listViewProfiles.Location = new System.Drawing.Point(13, 102);
            this.listViewProfiles.Margin = new System.Windows.Forms.Padding(0);
            this.listViewProfiles.MultiSelect = false;
            this.listViewProfiles.Name = "listViewProfiles";
            this.listViewProfiles.Size = new System.Drawing.Size(611, 166);
            this.listViewProfiles.TabIndex = 506;
            this.listViewProfiles.TileSize = new System.Drawing.Size(490, 35);
            this.listViewProfiles.UseCompatibleStateImageBehavior = false;
            this.listViewProfiles.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderProfile
            // 
            this.columnHeaderProfile.Text = "Profiles";
            this.columnHeaderProfile.Width = 450;
            // 
            // columnHeaderCurrent
            // 
            this.columnHeaderCurrent.Width = 150;
            // 
            // labelCopyFrom
            // 
            this.labelCopyFrom.BackColor = System.Drawing.Color.Transparent;
            this.labelCopyFrom.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCopyFrom.ForeColor = System.Drawing.Color.Black;
            this.labelCopyFrom.Location = new System.Drawing.Point(15, 68);
            this.labelCopyFrom.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelCopyFrom.Name = "labelCopyFrom";
            this.labelCopyFrom.Size = new System.Drawing.Size(135, 23);
            this.labelCopyFrom.TabIndex = 507;
            this.labelCopyFrom.Text = "Copy from:";
            this.labelCopyFrom.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormNewProfile
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(637, 390);
            this.ControlBox = false;
            this.Controls.Add(this.labelCopyFrom);
            this.Controls.Add(this.listViewProfiles);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonCreate);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MinimumSize = new System.Drawing.Size(650, 370);
            this.Name = "FormNewProfile";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create New Profile";
            this.Load += new System.EventHandler(this.FormNewProfile_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Button buttonCreate;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ListView listViewProfiles;
        private System.Windows.Forms.ColumnHeader columnHeaderProfile;
        private System.Windows.Forms.ColumnHeader columnHeaderCurrent;
        private System.Windows.Forms.Label labelCopyFrom;
    }
}
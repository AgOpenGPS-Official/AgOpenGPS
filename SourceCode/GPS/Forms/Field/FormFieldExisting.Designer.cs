namespace AgOpenGPS
{
    partial class FormFieldExisting
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
            this.label1 = new System.Windows.Forms.Label();
            this.tboxFieldName = new System.Windows.Forms.TextBox();
            this.btnSerialCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.chkApplied = new System.Windows.Forms.CheckBox();
            this.chkHeadland = new System.Windows.Forms.CheckBox();
            this.chkGuidanceLines = new System.Windows.Forms.CheckBox();
            this.chkFlags = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnAddDate = new System.Windows.Forms.Button();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.btnSort = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lvLines = new System.Windows.Forms.ListView();
            this.chName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chDistance = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.btnAddVehicleName = new System.Windows.Forms.Button();
            this.btnBackSpace = new AgLibrary.Controls.RepeatButton();
            this.lblTemplateChosen = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(4, 527);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 23);
            this.label1.TabIndex = 4;
            this.label1.Text = "Edit Field Name";
            // 
            // tboxFieldName
            // 
            this.tboxFieldName.BackColor = System.Drawing.Color.White;
            this.tboxFieldName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxFieldName.Location = new System.Drawing.Point(6, 486);
            this.tboxFieldName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tboxFieldName.Name = "tboxFieldName";
            this.tboxFieldName.Size = new System.Drawing.Size(657, 36);
            this.tboxFieldName.TabIndex = 0;
            this.tboxFieldName.Click += new System.EventHandler(this.tboxFieldName_Click);
            this.tboxFieldName.TextChanged += new System.EventHandler(this.tboxFieldName_TextChanged);
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
            this.btnSerialCancel.Location = new System.Drawing.Point(814, 582);
            this.btnSerialCancel.Name = "btnSerialCancel";
            this.btnSerialCancel.Size = new System.Drawing.Size(88, 77);
            this.btnSerialCancel.TabIndex = 4;
            this.btnSerialCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSerialCancel.UseVisualStyleBackColor = false;
            this.btnSerialCancel.Click += new System.EventHandler(this.btnSerialCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSave.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.Location = new System.Drawing.Point(911, 580);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(86, 79);
            this.btnSave.TabIndex = 3;
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // chkApplied
            // 
            this.chkApplied.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkApplied.BackColor = System.Drawing.Color.Transparent;
            this.chkApplied.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.chkApplied.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkApplied.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkApplied.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkApplied.ForeColor = System.Drawing.Color.White;
            this.chkApplied.Image = global::AgOpenGPS.Properties.Resources.SectionMasterOff;
            this.chkApplied.Location = new System.Drawing.Point(485, 575);
            this.chkApplied.Name = "chkApplied";
            this.chkApplied.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkApplied.Size = new System.Drawing.Size(76, 74);
            this.chkApplied.TabIndex = 257;
            this.chkApplied.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkApplied.UseVisualStyleBackColor = false;
            // 
            // chkHeadland
            // 
            this.chkHeadland.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkHeadland.BackColor = System.Drawing.Color.Transparent;
            this.chkHeadland.Checked = true;
            this.chkHeadland.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHeadland.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.chkHeadland.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkHeadland.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkHeadland.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHeadland.ForeColor = System.Drawing.Color.White;
            this.chkHeadland.Image = global::AgOpenGPS.Properties.Resources.HeadlandMenu;
            this.chkHeadland.Location = new System.Drawing.Point(590, 575);
            this.chkHeadland.Name = "chkHeadland";
            this.chkHeadland.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkHeadland.Size = new System.Drawing.Size(76, 74);
            this.chkHeadland.TabIndex = 258;
            this.chkHeadland.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkHeadland.UseVisualStyleBackColor = false;
            // 
            // chkGuidanceLines
            // 
            this.chkGuidanceLines.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkGuidanceLines.BackColor = System.Drawing.Color.Transparent;
            this.chkGuidanceLines.Checked = true;
            this.chkGuidanceLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkGuidanceLines.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.chkGuidanceLines.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkGuidanceLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkGuidanceLines.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkGuidanceLines.ForeColor = System.Drawing.Color.White;
            this.chkGuidanceLines.Image = global::AgOpenGPS.Properties.Resources.ABTrackAB;
            this.chkGuidanceLines.Location = new System.Drawing.Point(695, 575);
            this.chkGuidanceLines.Name = "chkGuidanceLines";
            this.chkGuidanceLines.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkGuidanceLines.Size = new System.Drawing.Size(76, 74);
            this.chkGuidanceLines.TabIndex = 259;
            this.chkGuidanceLines.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkGuidanceLines.UseVisualStyleBackColor = false;
            // 
            // chkFlags
            // 
            this.chkFlags.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkFlags.BackColor = System.Drawing.Color.Transparent;
            this.chkFlags.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.chkFlags.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkFlags.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkFlags.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkFlags.ForeColor = System.Drawing.Color.White;
            this.chkFlags.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.chkFlags.Location = new System.Drawing.Point(380, 575);
            this.chkFlags.Name = "chkFlags";
            this.chkFlags.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkFlags.Size = new System.Drawing.Size(76, 74);
            this.chkFlags.TabIndex = 260;
            this.chkFlags.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkFlags.UseVisualStyleBackColor = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label4.Location = new System.Drawing.Point(233, 591);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(24, 23);
            this.label4.TabIndex = 265;
            this.label4.Text = "+";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label2.Location = new System.Drawing.Point(120, 591);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 23);
            this.label2.TabIndex = 264;
            this.label2.Text = "+";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(602, 651);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 266;
            this.label5.Text = "Headland";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label6.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(722, 651);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 13);
            this.label6.TabIndex = 267;
            this.label6.Text = "Lines";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label7.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(499, 651);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 268;
            this.label7.Text = "Mapping";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(403, 651);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(32, 13);
            this.label8.TabIndex = 269;
            this.label8.Text = "Flags";
            // 
            // btnAddDate
            // 
            this.btnAddDate.BackColor = System.Drawing.Color.Transparent;
            this.btnAddDate.FlatAppearance.BorderSize = 0;
            this.btnAddDate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddDate.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddDate.Image = global::AgOpenGPS.Properties.Resources.JobNameCalendar;
            this.btnAddDate.Location = new System.Drawing.Point(138, 568);
            this.btnAddDate.Name = "btnAddDate";
            this.btnAddDate.Size = new System.Drawing.Size(76, 74);
            this.btnAddDate.TabIndex = 271;
            this.btnAddDate.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddDate.UseVisualStyleBackColor = false;
            this.btnAddDate.Click += new System.EventHandler(this.btnAddDate_Click);
            // 
            // btnAddTime
            // 
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddTime.Image = global::AgOpenGPS.Properties.Resources.JobNameTime;
            this.btnAddTime.Location = new System.Drawing.Point(249, 568);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(76, 74);
            this.btnAddTime.TabIndex = 270;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // btnSort
            // 
            this.btnSort.BackColor = System.Drawing.Color.Transparent;
            this.btnSort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSort.Image = global::AgOpenGPS.Properties.Resources.Sort;
            this.btnSort.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSort.Location = new System.Drawing.Point(834, 470);
            this.btnSort.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(147, 63);
            this.btnSort.TabIndex = 273;
            this.btnSort.Text = "Sort";
            this.btnSort.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSort.UseVisualStyleBackColor = false;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lvLines
            // 
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
            this.lvLines.Location = new System.Drawing.Point(6, 5);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(993, 445);
            this.lvLines.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lvLines.TabIndex = 274;
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Details;
            this.lvLines.SelectedIndexChanged += new System.EventHandler(this.lvLines_SelectedIndexChanged);
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
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label3.Location = new System.Drawing.Point(4, 591);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 23);
            this.label3.TabIndex = 275;
            this.label3.Text = "+";
            // 
            // btnAddVehicleName
            // 
            this.btnAddVehicleName.BackColor = System.Drawing.Color.Transparent;
            this.btnAddVehicleName.FlatAppearance.BorderSize = 0;
            this.btnAddVehicleName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddVehicleName.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAddVehicleName.Image = global::AgOpenGPS.Properties.Resources.Con_VehicleMenu;
            this.btnAddVehicleName.Location = new System.Drawing.Point(22, 568);
            this.btnAddVehicleName.Name = "btnAddVehicleName";
            this.btnAddVehicleName.Size = new System.Drawing.Size(76, 74);
            this.btnAddVehicleName.TabIndex = 276;
            this.btnAddVehicleName.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddVehicleName.UseVisualStyleBackColor = false;
            this.btnAddVehicleName.Click += new System.EventHandler(this.btnAddVehicleName_Click);
            // 
            // btnBackSpace
            // 
            this.btnBackSpace.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBackSpace.FlatAppearance.BorderSize = 0;
            this.btnBackSpace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackSpace.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBackSpace.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.btnBackSpace.Image = global::AgOpenGPS.Properties.Resources.BackSpace;
            this.btnBackSpace.Location = new System.Drawing.Point(669, 478);
            this.btnBackSpace.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnBackSpace.Name = "btnBackSpace";
            this.btnBackSpace.Size = new System.Drawing.Size(71, 51);
            this.btnBackSpace.TabIndex = 457;
            this.btnBackSpace.UseVisualStyleBackColor = true;
            this.btnBackSpace.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnBackSpace_MouseDown);
            // 
            // lblTemplateChosen
            // 
            this.lblTemplateChosen.AutoSize = true;
            this.lblTemplateChosen.BackColor = System.Drawing.Color.Transparent;
            this.lblTemplateChosen.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTemplateChosen.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblTemplateChosen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTemplateChosen.Location = new System.Drawing.Point(8, 453);
            this.lblTemplateChosen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTemplateChosen.Name = "lblTemplateChosen";
            this.lblTemplateChosen.Size = new System.Drawing.Size(34, 23);
            this.lblTemplateChosen.TabIndex = 458;
            this.lblTemplateChosen.Text = "---";
            // 
            // FormFieldExisting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(1004, 670);
            this.ControlBox = false;
            this.Controls.Add(this.lblTemplateChosen);
            this.Controls.Add(this.btnBackSpace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnAddVehicleName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lvLines);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.btnAddDate);
            this.Controls.Add(this.btnAddTime);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tboxFieldName);
            this.Controls.Add(this.chkHeadland);
            this.Controls.Add(this.chkGuidanceLines);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.chkFlags);
            this.Controls.Add(this.btnSerialCancel);
            this.Controls.Add(this.chkApplied);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FormFieldExisting";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Field Save As";
            this.Load += new System.EventHandler(this.FormFieldExisting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tboxFieldName;
        private System.Windows.Forms.Button btnSerialCancel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.CheckBox chkApplied;
        private System.Windows.Forms.CheckBox chkHeadland;
        private System.Windows.Forms.CheckBox chkGuidanceLines;
        private System.Windows.Forms.CheckBox chkFlags;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnAddDate;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.Button btnSort;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chName;
        private System.Windows.Forms.ColumnHeader chDistance;
        private System.Windows.Forms.ColumnHeader chArea;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnAddVehicleName;
        private AgLibrary.Controls.RepeatButton btnBackSpace;
        private System.Windows.Forms.Label lblTemplateChosen;
    }
}
namespace AgOpenGPS.Forms.Profiles
{
    partial class FormLoadVehicleTool
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.listViewVehicles = new System.Windows.Forms.ListView();
            this.columnHeaderVehicle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewTools = new System.Windows.Forms.ListView();
            this.columnHeaderTool = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lblCurrentVehicle = new System.Windows.Forms.Label();
            this.lblCurrentTool = new System.Windows.Forms.Label();
            this.lblSelectedVehicle = new System.Windows.Forms.Label();
            this.lblSelectedTool = new System.Windows.Forms.Label();
            this.lblVehType = new System.Windows.Forms.Label();
            this.lblVehWheelbase = new System.Windows.Forms.Label();
            this.lblVehAntPivot = new System.Windows.Forms.Label();
            this.lblVehAntOffset = new System.Windows.Forms.Label();
            this.lblVehTrackWidth = new System.Windows.Forms.Label();
            this.lblVehHitch = new System.Windows.Forms.Label();
            this.lblToolWidth = new System.Windows.Forms.Label();
            this.lblToolOverlap = new System.Windows.Forms.Label();
            this.lblToolOffset = new System.Windows.Forms.Label();
            this.lblToolSections = new System.Windows.Forms.Label();
            this.lblToolAttach = new System.Windows.Forms.Label();
            this.lblToolHitch = new System.Windows.Forms.Label();
            this.buttonLoad = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonNewVehicle = new System.Windows.Forms.Button();
            this.buttonNewTool = new System.Windows.Forms.Button();
            this.buttonDeleteVehicle = new System.Windows.Forms.Button();
            this.buttonDeleteTool = new System.Windows.Forms.Button();
            this.buttonConvertOld = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // listViewVehicles
            // 
            this.listViewVehicles.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewVehicles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderVehicle});
            this.listViewVehicles.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.listViewVehicles.FullRowSelect = true;
            this.listViewVehicles.GridLines = true;
            this.listViewVehicles.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewVehicles.HideSelection = false;
            this.listViewVehicles.LabelWrap = false;
            this.listViewVehicles.Location = new System.Drawing.Point(12, 35);
            this.listViewVehicles.MultiSelect = false;
            this.listViewVehicles.Name = "listViewVehicles";
            this.listViewVehicles.Size = new System.Drawing.Size(400, 320);
            this.listViewVehicles.TabIndex = 1;
            this.listViewVehicles.UseCompatibleStateImageBehavior = false;
            this.listViewVehicles.View = System.Windows.Forms.View.Details;
            this.listViewVehicles.SelectedIndexChanged += new System.EventHandler(this.listViewVehicles_SelectedIndexChanged);
            // 
            // columnHeaderVehicle
            // 
            this.columnHeaderVehicle.Width = 375;
            // 
            // listViewTools
            // 
            this.listViewTools.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.listViewTools.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderTool});
            this.listViewTools.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Bold);
            this.listViewTools.FullRowSelect = true;
            this.listViewTools.GridLines = true;
            this.listViewTools.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewTools.HideSelection = false;
            this.listViewTools.LabelWrap = false;
            this.listViewTools.Location = new System.Drawing.Point(500, 35);
            this.listViewTools.MultiSelect = false;
            this.listViewTools.Name = "listViewTools";
            this.listViewTools.Size = new System.Drawing.Size(400, 320);
            this.listViewTools.TabIndex = 2;
            this.listViewTools.UseCompatibleStateImageBehavior = false;
            this.listViewTools.View = System.Windows.Forms.View.Details;
            this.listViewTools.SelectedIndexChanged += new System.EventHandler(this.listViewTools_SelectedIndexChanged);
            // 
            // columnHeaderTool
            // 
            this.columnHeaderTool.Width = 375;
            // 
            // lblCurrentVehicle
            // 
            this.lblCurrentVehicle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrentVehicle.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblCurrentVehicle.Location = new System.Drawing.Point(12, 8);
            this.lblCurrentVehicle.Name = "lblCurrentVehicle";
            this.lblCurrentVehicle.Size = new System.Drawing.Size(400, 24);
            this.lblCurrentVehicle.TabIndex = 0;
            this.lblCurrentVehicle.Text = "Current: -";
            // 
            // lblCurrentTool
            // 
            this.lblCurrentTool.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.lblCurrentTool.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblCurrentTool.Location = new System.Drawing.Point(500, 8);
            this.lblCurrentTool.Name = "lblCurrentTool";
            this.lblCurrentTool.Size = new System.Drawing.Size(400, 24);
            this.lblCurrentTool.TabIndex = 12;
            this.lblCurrentTool.Text = "Current: -";
            // 
            // lblSelectedVehicle
            // 
            this.lblSelectedVehicle.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Italic);
            this.lblSelectedVehicle.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSelectedVehicle.Location = new System.Drawing.Point(12, 359);
            this.lblSelectedVehicle.Name = "lblSelectedVehicle";
            this.lblSelectedVehicle.Size = new System.Drawing.Size(400, 22);
            this.lblSelectedVehicle.TabIndex = 5;
            this.lblSelectedVehicle.Text = "Selected: -";
            // 
            // lblSelectedTool
            // 
            this.lblSelectedTool.Font = new System.Drawing.Font("Tahoma", 11F, System.Drawing.FontStyle.Italic);
            this.lblSelectedTool.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblSelectedTool.Location = new System.Drawing.Point(500, 359);
            this.lblSelectedTool.Name = "lblSelectedTool";
            this.lblSelectedTool.Size = new System.Drawing.Size(400, 22);
            this.lblSelectedTool.TabIndex = 16;
            this.lblSelectedTool.Text = "Selected: -";
            // 
            // lblVehType
            // 
            this.lblVehType.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVehType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVehType.Location = new System.Drawing.Point(12, 385);
            this.lblVehType.Name = "lblVehType";
            this.lblVehType.Size = new System.Drawing.Size(220, 20);
            this.lblVehType.TabIndex = 6;
            this.lblVehType.Text = "Type:";
            // 
            // lblVehWheelbase
            // 
            this.lblVehWheelbase.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVehWheelbase.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVehWheelbase.Location = new System.Drawing.Point(12, 407);
            this.lblVehWheelbase.Name = "lblVehWheelbase";
            this.lblVehWheelbase.Size = new System.Drawing.Size(220, 20);
            this.lblVehWheelbase.TabIndex = 7;
            this.lblVehWheelbase.Text = "Wheelbase:";
            // 
            // lblVehAntPivot
            // 
            this.lblVehAntPivot.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVehAntPivot.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVehAntPivot.Location = new System.Drawing.Point(12, 429);
            this.lblVehAntPivot.Name = "lblVehAntPivot";
            this.lblVehAntPivot.Size = new System.Drawing.Size(220, 20);
            this.lblVehAntPivot.TabIndex = 8;
            this.lblVehAntPivot.Text = "Ant. Pivot:";
            // 
            // lblVehAntOffset
            // 
            this.lblVehAntOffset.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVehAntOffset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVehAntOffset.Location = new System.Drawing.Point(242, 385);
            this.lblVehAntOffset.Name = "lblVehAntOffset";
            this.lblVehAntOffset.Size = new System.Drawing.Size(220, 20);
            this.lblVehAntOffset.TabIndex = 9;
            this.lblVehAntOffset.Text = "Ant. Offset:";
            // 
            // lblVehTrackWidth
            // 
            this.lblVehTrackWidth.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVehTrackWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVehTrackWidth.Location = new System.Drawing.Point(242, 407);
            this.lblVehTrackWidth.Name = "lblVehTrackWidth";
            this.lblVehTrackWidth.Size = new System.Drawing.Size(220, 20);
            this.lblVehTrackWidth.TabIndex = 10;
            this.lblVehTrackWidth.Text = "Track Width:";
            // 
            // lblVehHitch
            // 
            this.lblVehHitch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblVehHitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblVehHitch.Location = new System.Drawing.Point(242, 429);
            this.lblVehHitch.Name = "lblVehHitch";
            this.lblVehHitch.Size = new System.Drawing.Size(220, 20);
            this.lblVehHitch.TabIndex = 11;
            this.lblVehHitch.Text = "Hitch:";
            // 
            // lblToolWidth
            // 
            this.lblToolWidth.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblToolWidth.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblToolWidth.Location = new System.Drawing.Point(500, 385);
            this.lblToolWidth.Name = "lblToolWidth";
            this.lblToolWidth.Size = new System.Drawing.Size(220, 20);
            this.lblToolWidth.TabIndex = 17;
            this.lblToolWidth.Text = "Width:";
            // 
            // lblToolOverlap
            // 
            this.lblToolOverlap.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblToolOverlap.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblToolOverlap.Location = new System.Drawing.Point(500, 407);
            this.lblToolOverlap.Name = "lblToolOverlap";
            this.lblToolOverlap.Size = new System.Drawing.Size(220, 20);
            this.lblToolOverlap.TabIndex = 18;
            this.lblToolOverlap.Text = "Overlap:";
            // 
            // lblToolOffset
            // 
            this.lblToolOffset.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblToolOffset.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblToolOffset.Location = new System.Drawing.Point(500, 429);
            this.lblToolOffset.Name = "lblToolOffset";
            this.lblToolOffset.Size = new System.Drawing.Size(220, 20);
            this.lblToolOffset.TabIndex = 19;
            this.lblToolOffset.Text = "Offset:";
            // 
            // lblToolSections
            // 
            this.lblToolSections.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblToolSections.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblToolSections.Location = new System.Drawing.Point(730, 385);
            this.lblToolSections.Name = "lblToolSections";
            this.lblToolSections.Size = new System.Drawing.Size(220, 20);
            this.lblToolSections.TabIndex = 20;
            this.lblToolSections.Text = "Sections:";
            // 
            // lblToolAttach
            // 
            this.lblToolAttach.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblToolAttach.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblToolAttach.Location = new System.Drawing.Point(730, 407);
            this.lblToolAttach.Name = "lblToolAttach";
            this.lblToolAttach.Size = new System.Drawing.Size(220, 20);
            this.lblToolAttach.TabIndex = 21;
            this.lblToolAttach.Text = "Attach:";
            // 
            // lblToolHitch
            // 
            this.lblToolHitch.Font = new System.Drawing.Font("Tahoma", 10F);
            this.lblToolHitch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(40)))), ((int)(((byte)(40)))));
            this.lblToolHitch.Location = new System.Drawing.Point(730, 429);
            this.lblToolHitch.Name = "lblToolHitch";
            this.lblToolHitch.Size = new System.Drawing.Size(220, 20);
            this.lblToolHitch.TabIndex = 22;
            this.lblToolHitch.Text = "Trail Hitch:";
            // 
            // buttonLoad
            // 
            this.buttonLoad.Enabled = false;
            this.buttonLoad.FlatAppearance.BorderSize = 0;
            this.buttonLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonLoad.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.buttonLoad.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.buttonLoad.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonLoad.Location = new System.Drawing.Point(924, 450);
            this.buttonLoad.Name = "buttonLoad";
            this.buttonLoad.Size = new System.Drawing.Size(80, 98);
            this.buttonLoad.TabIndex = 25;
            this.buttonLoad.Text = "Load";
            this.buttonLoad.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonLoad.Click += new System.EventHandler(this.buttonLoad_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.FlatAppearance.BorderSize = 0;
            this.buttonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.buttonCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.buttonCancel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonCancel.Location = new System.Drawing.Point(838, 450);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(80, 98);
            this.buttonCancel.TabIndex = 24;
            this.buttonCancel.Text = "Cancel";
            this.buttonCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // buttonNewVehicle
            // 
            this.buttonNewVehicle.FlatAppearance.BorderSize = 0;
            this.buttonNewVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNewVehicle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.buttonNewVehicle.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.buttonNewVehicle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonNewVehicle.Location = new System.Drawing.Point(424, 115);
            this.buttonNewVehicle.Name = "buttonNewVehicle";
            this.buttonNewVehicle.Size = new System.Drawing.Size(65, 70);
            this.buttonNewVehicle.TabIndex = 3;
            this.buttonNewVehicle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonNewVehicle.Click += new System.EventHandler(this.buttonNewVehicle_Click);
            // 
            // buttonNewTool
            // 
            this.buttonNewTool.FlatAppearance.BorderSize = 0;
            this.buttonNewTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonNewTool.Font = new System.Drawing.Font("Tahoma", 9F);
            this.buttonNewTool.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.buttonNewTool.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonNewTool.Location = new System.Drawing.Point(918, 115);
            this.buttonNewTool.Name = "buttonNewTool";
            this.buttonNewTool.Size = new System.Drawing.Size(65, 70);
            this.buttonNewTool.TabIndex = 14;
            this.buttonNewTool.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonNewTool.Click += new System.EventHandler(this.buttonNewTool_Click);
            // 
            // buttonDeleteVehicle
            // 
            this.buttonDeleteVehicle.Enabled = false;
            this.buttonDeleteVehicle.FlatAppearance.BorderSize = 0;
            this.buttonDeleteVehicle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteVehicle.Font = new System.Drawing.Font("Tahoma", 9F);
            this.buttonDeleteVehicle.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.buttonDeleteVehicle.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDeleteVehicle.Location = new System.Drawing.Point(424, 35);
            this.buttonDeleteVehicle.Name = "buttonDeleteVehicle";
            this.buttonDeleteVehicle.Size = new System.Drawing.Size(65, 70);
            this.buttonDeleteVehicle.TabIndex = 2;
            this.buttonDeleteVehicle.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDeleteVehicle.Click += new System.EventHandler(this.buttonDeleteVehicle_Click);
            // 
            // buttonDeleteTool
            // 
            this.buttonDeleteTool.Enabled = false;
            this.buttonDeleteTool.FlatAppearance.BorderSize = 0;
            this.buttonDeleteTool.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDeleteTool.Font = new System.Drawing.Font("Tahoma", 9F);
            this.buttonDeleteTool.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.buttonDeleteTool.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.buttonDeleteTool.Location = new System.Drawing.Point(918, 35);
            this.buttonDeleteTool.Name = "buttonDeleteTool";
            this.buttonDeleteTool.Size = new System.Drawing.Size(65, 70);
            this.buttonDeleteTool.TabIndex = 13;
            this.buttonDeleteTool.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.buttonDeleteTool.Click += new System.EventHandler(this.buttonDeleteTool_Click);
            // 
            // buttonConvertOld
            // 
            this.buttonConvertOld.BackColor = System.Drawing.Color.LightSalmon;
            this.buttonConvertOld.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonConvertOld.Font = new System.Drawing.Font("Tahoma", 11F);
            this.buttonConvertOld.Location = new System.Drawing.Point(12, 481);
            this.buttonConvertOld.Name = "buttonConvertOld";
            this.buttonConvertOld.Size = new System.Drawing.Size(161, 49);
            this.buttonConvertOld.TabIndex = 23;
            this.buttonConvertOld.Text = "Convert Old profiles";
            this.buttonConvertOld.UseVisualStyleBackColor = false;
            this.buttonConvertOld.Click += new System.EventHandler(this.buttonConvertOld_Click);
            // 
            // FormLoadVehicleTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1008, 545);
            this.ControlBox = false;
            this.Controls.Add(this.lblCurrentVehicle);
            this.Controls.Add(this.listViewVehicles);
            this.Controls.Add(this.buttonDeleteVehicle);
            this.Controls.Add(this.buttonNewVehicle);
            this.Controls.Add(this.lblSelectedVehicle);
            this.Controls.Add(this.lblVehType);
            this.Controls.Add(this.lblVehWheelbase);
            this.Controls.Add(this.lblVehAntPivot);
            this.Controls.Add(this.lblVehAntOffset);
            this.Controls.Add(this.lblVehTrackWidth);
            this.Controls.Add(this.lblVehHitch);
            this.Controls.Add(this.lblCurrentTool);
            this.Controls.Add(this.listViewTools);
            this.Controls.Add(this.buttonDeleteTool);
            this.Controls.Add(this.buttonNewTool);
            this.Controls.Add(this.lblSelectedTool);
            this.Controls.Add(this.lblToolWidth);
            this.Controls.Add(this.lblToolOverlap);
            this.Controls.Add(this.lblToolOffset);
            this.Controls.Add(this.lblToolSections);
            this.Controls.Add(this.lblToolAttach);
            this.Controls.Add(this.lblToolHitch);
            this.Controls.Add(this.buttonConvertOld);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.buttonLoad);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.Name = "FormLoadVehicleTool";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vehicle / Tool";
            this.Load += new System.EventHandler(this.FormLoadVehicleTool_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewVehicles;
        private System.Windows.Forms.ColumnHeader columnHeaderVehicle;
        private System.Windows.Forms.ListView listViewTools;
        private System.Windows.Forms.ColumnHeader columnHeaderTool;
        private System.Windows.Forms.Label lblCurrentVehicle;
        private System.Windows.Forms.Label lblCurrentTool;
        private System.Windows.Forms.Label lblSelectedVehicle;
        private System.Windows.Forms.Label lblSelectedTool;
        private System.Windows.Forms.Label lblVehType;
        private System.Windows.Forms.Label lblVehWheelbase;
        private System.Windows.Forms.Label lblVehAntPivot;
        private System.Windows.Forms.Label lblVehAntOffset;
        private System.Windows.Forms.Label lblVehTrackWidth;
        private System.Windows.Forms.Label lblVehHitch;
        private System.Windows.Forms.Label lblToolWidth;
        private System.Windows.Forms.Label lblToolOverlap;
        private System.Windows.Forms.Label lblToolOffset;
        private System.Windows.Forms.Label lblToolSections;
        private System.Windows.Forms.Label lblToolAttach;
        private System.Windows.Forms.Label lblToolHitch;
        private System.Windows.Forms.Button buttonLoad;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Button buttonNewVehicle;
        private System.Windows.Forms.Button buttonNewTool;
        private System.Windows.Forms.Button buttonDeleteVehicle;
        private System.Windows.Forms.Button buttonDeleteTool;
        private System.Windows.Forms.Button buttonConvertOld;
    }
}

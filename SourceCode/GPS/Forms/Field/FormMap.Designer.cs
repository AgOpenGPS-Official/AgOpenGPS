namespace AgOpenGPS
{
    partial class FormMap
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblPoints = new System.Windows.Forms.Label();
            this.lblBnds = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.labelNewBoundary = new System.Windows.Forms.Label();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnGo = new System.Windows.Forms.Button();
            this.cboxEnableLineDraw = new System.Windows.Forms.CheckBox();
            this.btnDeleteAll = new System.Windows.Forms.Button();
            this.labelBackground = new System.Windows.Forms.Label();
            this.btnAddFence = new System.Windows.Forms.Button();
            this.btnDeletePoint = new System.Windows.Forms.Button();
            this.labelBoundary = new System.Windows.Forms.Label();
            this.cboxDrawMap = new System.Windows.Forms.CheckBox();
            this.gMapControl = new GMap.NET.WindowsForms.GMapControl();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblPoints
            // 
            this.lblPoints.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblPoints.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.lblPoints, 2);
            this.lblPoints.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoints.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPoints.Location = new System.Drawing.Point(108, 170);
            this.lblPoints.Name = "lblPoints";
            this.lblPoints.Size = new System.Drawing.Size(117, 23);
            this.lblPoints.TabIndex = 476;
            this.lblPoints.Text = "Points: 12";
            this.lblPoints.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblBnds
            // 
            this.lblBnds.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBnds.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.lblBnds, 2);
            this.lblBnds.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBnds.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblBnds.Location = new System.Drawing.Point(108, 274);
            this.lblBnds.Name = "lblBnds";
            this.lblBnds.Size = new System.Drawing.Size(130, 80);
            this.lblBnds.TabIndex = 489;
            this.lblBnds.Text = "2";
            this.lblBnds.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 43.57143F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 19.64286F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.78571F));
            this.tableLayoutPanel1.Controls.Add(this.labelNewBoundary, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.btnZoomIn, 2, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnZoomOut, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.btnExit, 2, 9);
            this.tableLayoutPanel1.Controls.Add(this.btnGo, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.cboxEnableLineDraw, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnDeleteAll, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.labelBackground, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.btnAddFence, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnDeletePoint, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.labelBoundary, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblBnds, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblPoints, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.cboxDrawMap, 0, 6);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(886, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 10;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12066F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.351676F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.38177F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.54257F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12066F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12066F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12066F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12066F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.12066F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(241, 776);
            this.tableLayoutPanel1.TabIndex = 491;
            // 
            // labelNewBoundary
            // 
            this.labelNewBoundary.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelNewBoundary.BackColor = System.Drawing.Color.Transparent;
            this.labelNewBoundary.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNewBoundary.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelNewBoundary.Location = new System.Drawing.Point(3, 82);
            this.labelNewBoundary.Name = "labelNewBoundary";
            this.labelNewBoundary.Size = new System.Drawing.Size(99, 23);
            this.labelNewBoundary.TabIndex = 491;
            this.labelNewBoundary.Text = "New Bnd";
            this.labelNewBoundary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZoomIn.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Image = global::AgOpenGPS.Properties.Resources.ZoomOGL;
            this.btnZoomIn.Location = new System.Drawing.Point(162, 623);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(69, 56);
            this.btnZoomIn.TabIndex = 484;
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZoomOut.BackColor = System.Drawing.Color.Transparent;
            this.btnZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Image = global::AgOpenGPS.Properties.Resources.ZoomOut;
            this.btnZoomOut.Location = new System.Drawing.Point(20, 619);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(64, 63);
            this.btnZoomOut.TabIndex = 485;
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.Location = new System.Drawing.Point(166, 702);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(61, 63);
            this.btnExit.TabIndex = 234;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnGo
            // 
            this.btnGo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGo.BackColor = System.Drawing.Color.Transparent;
            this.btnGo.FlatAppearance.BorderSize = 0;
            this.btnGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGo.Image = global::AgOpenGPS.Properties.Resources.FlagGrn;
            this.btnGo.Location = new System.Drawing.Point(21, 705);
            this.btnGo.Name = "btnGo";
            this.btnGo.Size = new System.Drawing.Size(63, 57);
            this.btnGo.TabIndex = 465;
            this.btnGo.UseVisualStyleBackColor = false;
            this.btnGo.Click += new System.EventHandler(this.btnGo_Click);
            // 
            // cboxEnableLineDraw
            // 
            this.cboxEnableLineDraw.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxEnableLineDraw.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxEnableLineDraw.BackColor = System.Drawing.Color.Transparent;
            this.cboxEnableLineDraw.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.cboxEnableLineDraw.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxEnableLineDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxEnableLineDraw.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxEnableLineDraw.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxEnableLineDraw.Image = global::AgOpenGPS.Properties.Resources.BoundaryOuter;
            this.cboxEnableLineDraw.Location = new System.Drawing.Point(11, 5);
            this.cboxEnableLineDraw.Name = "cboxEnableLineDraw";
            this.cboxEnableLineDraw.Size = new System.Drawing.Size(83, 72);
            this.cboxEnableLineDraw.TabIndex = 472;
            this.cboxEnableLineDraw.UseVisualStyleBackColor = false;
            this.cboxEnableLineDraw.Click += new System.EventHandler(this.cboxEnableLineDraw_Click);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteAll.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteAll.Enabled = false;
            this.btnDeleteAll.FlatAppearance.BorderSize = 0;
            this.btnDeleteAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAll.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteAll.Location = new System.Drawing.Point(166, 11);
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(61, 59);
            this.btnDeleteAll.TabIndex = 471;
            this.btnDeleteAll.UseVisualStyleBackColor = false;
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // labelBackground
            // 
            this.labelBackground.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.labelBackground.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.labelBackground, 3);
            this.labelBackground.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBackground.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelBackground.Location = new System.Drawing.Point(15, 528);
            this.labelBackground.Name = "labelBackground";
            this.labelBackground.Size = new System.Drawing.Size(210, 31);
            this.labelBackground.TabIndex = 490;
            this.labelBackground.Text = "Background ";
            this.labelBackground.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddFence
            // 
            this.btnAddFence.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddFence.BackColor = System.Drawing.Color.Transparent;
            this.btnAddFence.Enabled = false;
            this.btnAddFence.FlatAppearance.BorderSize = 0;
            this.btnAddFence.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddFence.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.btnAddFence.Location = new System.Drawing.Point(22, 283);
            this.btnAddFence.Name = "btnAddFence";
            this.btnAddFence.Size = new System.Drawing.Size(61, 61);
            this.btnAddFence.TabIndex = 470;
            this.btnAddFence.UseVisualStyleBackColor = false;
            this.btnAddFence.Click += new System.EventHandler(this.btnAddFence_Click);
            // 
            // btnDeletePoint
            // 
            this.btnDeletePoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeletePoint.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletePoint.Enabled = false;
            this.btnDeletePoint.FlatAppearance.BorderSize = 0;
            this.btnDeletePoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeletePoint.Image = global::AgOpenGPS.Properties.Resources.PointDelete;
            this.btnDeletePoint.Location = new System.Drawing.Point(9, 142);
            this.btnDeletePoint.Name = "btnDeletePoint";
            this.btnDeletePoint.Size = new System.Drawing.Size(87, 78);
            this.btnDeletePoint.TabIndex = 468;
            this.btnDeletePoint.UseVisualStyleBackColor = false;
            this.btnDeletePoint.Click += new System.EventHandler(this.btnDeletePoint_Click);
            // 
            // labelBoundary
            // 
            this.labelBoundary.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelBoundary.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.labelBoundary, 2);
            this.labelBoundary.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelBoundary.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelBoundary.Location = new System.Drawing.Point(108, 234);
            this.labelBoundary.Name = "labelBoundary";
            this.labelBoundary.Size = new System.Drawing.Size(130, 23);
            this.labelBoundary.TabIndex = 492;
            this.labelBoundary.Text = "Boundary";
            this.labelBoundary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxDrawMap
            // 
            this.cboxDrawMap.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cboxDrawMap.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxDrawMap.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.SetColumnSpan(this.cboxDrawMap, 3);
            this.cboxDrawMap.FlatAppearance.BorderSize = 0;
            this.cboxDrawMap.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxDrawMap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxDrawMap.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDrawMap.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxDrawMap.Image = global::AgOpenGPS.Properties.Resources.MappingOff;
            this.cboxDrawMap.Location = new System.Drawing.Point(81, 449);
            this.cboxDrawMap.Name = "cboxDrawMap";
            this.cboxDrawMap.Size = new System.Drawing.Size(78, 76);
            this.cboxDrawMap.TabIndex = 482;
            this.cboxDrawMap.UseVisualStyleBackColor = false;
            this.cboxDrawMap.Click += new System.EventHandler(this.cboxDrawMap_Click);
            // 
            // gMapControl
            // 
            this.gMapControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gMapControl.BackColor = System.Drawing.Color.White;
            this.gMapControl.Bearing = 0F;
            this.gMapControl.CanDragMap = true;
            this.gMapControl.Cursor = System.Windows.Forms.Cursors.Cross;
            this.gMapControl.EmptyTileColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.gMapControl.ForeColor = System.Drawing.Color.Black;
            this.gMapControl.GrayScaleMode = false;
            this.gMapControl.HelperLineOption = GMap.NET.WindowsForms.HelperLineOptions.DontShow;
            this.gMapControl.LevelsKeepInMemory = 5;
            this.gMapControl.Location = new System.Drawing.Point(3, 3);
            this.gMapControl.MarkersEnabled = true;
            this.gMapControl.MaxZoom = 19;
            this.gMapControl.MinZoom = 1;
            this.gMapControl.MouseWheelZoomEnabled = true;
            this.gMapControl.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.gMapControl.Name = "gMapControl";
            this.gMapControl.NegativeMode = false;
            this.gMapControl.PolygonsEnabled = true;
            this.gMapControl.RetryLoadTile = 0;
            this.gMapControl.RoutesEnabled = true;
            this.gMapControl.ScaleMode = GMap.NET.WindowsForms.ScaleModes.Integer;
            this.gMapControl.SelectedAreaFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(65)))), ((int)(((byte)(105)))), ((int)(((byte)(225)))));
            this.gMapControl.ShowTileGridLines = false;
            this.gMapControl.Size = new System.Drawing.Size(880, 776);
            this.gMapControl.TabIndex = 492;
            this.gMapControl.Zoom = 2D;
            this.gMapControl.OnMapClick += new GMap.NET.WindowsForms.MapClick(this.gMapControl_OnMapClick);
            this.gMapControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.gMapControl_MouseMove);
            this.gMapControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.gMapControl_MouseWheel);
            // 
            // FormMap
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1130, 780);
            this.ControlBox = false;
            this.Controls.Add(this.gMapControl);
            this.Controls.Add(this.tableLayoutPanel1);
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1016, 753);
            this.Name = "FormMap";
            this.ShowInTaskbar = false;
            this.Text = "Bing Maps for Background";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMap_FormClosing);
            this.Load += new System.EventHandler(this.FormMap_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnGo;
        private System.Windows.Forms.Button btnDeletePoint;
        private System.Windows.Forms.Button btnAddFence;
        private System.Windows.Forms.Button btnDeleteAll;
        private System.Windows.Forms.CheckBox cboxEnableLineDraw;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.CheckBox cboxDrawMap;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Label lblBnds;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label labelBackground;
        private System.Windows.Forms.Label labelNewBoundary;
        private System.Windows.Forms.Label labelBoundary;
        private GMap.NET.WindowsForms.GMapControl gMapControl;
    }
}
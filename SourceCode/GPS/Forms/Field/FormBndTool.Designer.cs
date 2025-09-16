namespace AgOpenGPS
{
    partial class FormBndTool
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
            this.oglSelf = new OpenTK.GLControl.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cboxPointDistance = new System.Windows.Forms.ComboBox();
            this.labelReducedPoints = new System.Windows.Forms.Label();
            this.cboxSmooth = new System.Windows.Forms.ComboBox();
            this.labelSpacing = new System.Windows.Forms.Label();
            this.labelSmooth = new System.Windows.Forms.Label();
            this.cboxIsZoom = new System.Windows.Forms.CheckBox();
            this.btnCenterOGL = new System.Windows.Forms.Button();
            this.btnSlice = new System.Windows.Forms.Button();
            this.btnMakeBoundary = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveDn = new System.Windows.Forms.Button();
            this.labelCreate = new System.Windows.Forms.Label();
            this.btnAddPoints = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPleaseWait = new System.Windows.Forms.Label();
            this.lblPointToProcess = new System.Windows.Forms.Label();
            this.labelPointsToProcess = new System.Windows.Forms.Label();
            this.lblI = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.tlp1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // oglSelf
            // 
            this.oglSelf.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.oglSelf.APIVersion = new System.Version(3, 3, 0, 0);
            this.oglSelf.BackColor = System.Drawing.Color.Black;
            this.oglSelf.Cursor = System.Windows.Forms.Cursors.Cross;
            this.oglSelf.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.oglSelf.IsEventDriven = true;
            this.oglSelf.Location = new System.Drawing.Point(0, 0);
            this.oglSelf.Margin = new System.Windows.Forms.Padding(0);
            this.oglSelf.Name = "oglSelf";
            this.oglSelf.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.oglSelf.SharedContext = null;
            this.oglSelf.Size = new System.Drawing.Size(700, 700);
            this.oglSelf.TabIndex = 183;
            this.oglSelf.Load += new System.EventHandler(this.oglSelf_Load);
            this.oglSelf.Paint += new System.Windows.Forms.PaintEventHandler(this.oglSelf_Paint);
            this.oglSelf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglSelf_MouseDown);
            this.oglSelf.Resize += new System.EventHandler(this.oglSelf_Resize);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // cboxPointDistance
            // 
            this.cboxPointDistance.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cboxPointDistance.BackColor = System.Drawing.Color.Lavender;
            this.tlp1.SetColumnSpan(this.cboxPointDistance, 3);
            this.cboxPointDistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPointDistance.Enabled = false;
            this.cboxPointDistance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxPointDistance.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxPointDistance.FormattingEnabled = true;
            this.cboxPointDistance.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "?"});
            this.cboxPointDistance.Location = new System.Drawing.Point(26, 120);
            this.cboxPointDistance.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxPointDistance.Name = "cboxPointDistance";
            this.cboxPointDistance.Size = new System.Drawing.Size(101, 53);
            this.cboxPointDistance.TabIndex = 541;
            this.cboxPointDistance.SelectedIndexChanged += new System.EventHandler(this.cboxPointDistance_SelectedIndexChanged);
            // 
            // labelReducedPoints
            // 
            this.labelReducedPoints.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tlp1.SetColumnSpan(this.labelReducedPoints, 3);
            this.labelReducedPoints.Enabled = false;
            this.labelReducedPoints.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReducedPoints.ForeColor = System.Drawing.Color.Black;
            this.labelReducedPoints.Location = new System.Drawing.Point(166, 92);
            this.labelReducedPoints.Name = "labelReducedPoints";
            this.labelReducedPoints.Size = new System.Drawing.Size(120, 25);
            this.labelReducedPoints.TabIndex = 542;
            this.labelReducedPoints.Text = "***";
            this.labelReducedPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxSmooth
            // 
            this.cboxSmooth.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cboxSmooth.BackColor = System.Drawing.Color.Lavender;
            this.tlp1.SetColumnSpan(this.cboxSmooth, 3);
            this.cboxSmooth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSmooth.Enabled = false;
            this.cboxSmooth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxSmooth.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSmooth.FormattingEnabled = true;
            this.cboxSmooth.Items.AddRange(new object[] {
            "0",
            "4",
            "8",
            "16",
            "32",
            "64",
            "?"});
            this.cboxSmooth.Location = new System.Drawing.Point(26, 232);
            this.cboxSmooth.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxSmooth.Name = "cboxSmooth";
            this.cboxSmooth.Size = new System.Drawing.Size(101, 53);
            this.cboxSmooth.TabIndex = 547;
            this.cboxSmooth.SelectedIndexChanged += new System.EventHandler(this.cboxSmooth_SelectedIndexChanged);
            // 
            // labelSpacing
            // 
            this.labelSpacing.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.tlp1.SetColumnSpan(this.labelSpacing, 3);
            this.labelSpacing.Enabled = false;
            this.labelSpacing.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpacing.ForeColor = System.Drawing.Color.Black;
            this.labelSpacing.Location = new System.Drawing.Point(12, 92);
            this.labelSpacing.Name = "labelSpacing";
            this.labelSpacing.Size = new System.Drawing.Size(130, 25);
            this.labelSpacing.TabIndex = 562;
            this.labelSpacing.Text = "Spacing (m)";
            this.labelSpacing.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelSmooth
            // 
            this.labelSmooth.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelSmooth.AutoSize = true;
            this.tlp1.SetColumnSpan(this.labelSmooth, 3);
            this.labelSmooth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSmooth.ForeColor = System.Drawing.Color.Black;
            this.labelSmooth.Location = new System.Drawing.Point(41, 210);
            this.labelSmooth.Name = "labelSmooth";
            this.labelSmooth.Size = new System.Drawing.Size(71, 19);
            this.labelSmooth.TabIndex = 569;
            this.labelSmooth.Text = "Smooth";
            this.labelSmooth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxIsZoom
            // 
            this.cboxIsZoom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsZoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsZoom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboxIsZoom.Checked = true;
            this.cboxIsZoom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tlp1.SetColumnSpan(this.cboxIsZoom, 3);
            this.cboxIsZoom.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsZoom.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(160)))));
            this.cboxIsZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsZoom.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsZoom.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsZoom.Image = global::AgOpenGPS.Properties.Resources.ZoomOGL;
            this.cboxIsZoom.Location = new System.Drawing.Point(39, 342);
            this.cboxIsZoom.Name = "cboxIsZoom";
            this.cboxIsZoom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsZoom.Size = new System.Drawing.Size(76, 71);
            this.cboxIsZoom.TabIndex = 561;
            this.cboxIsZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsZoom.UseVisualStyleBackColor = false;
            // 
            // btnCenterOGL
            // 
            this.btnCenterOGL.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCenterOGL.BackColor = System.Drawing.Color.White;
            this.btnCenterOGL.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnCenterOGL, 3);
            this.btnCenterOGL.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCenterOGL.FlatAppearance.BorderSize = 0;
            this.btnCenterOGL.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCenterOGL.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCenterOGL.Image = global::AgOpenGPS.Properties.Resources.ConS_SourceFix;
            this.btnCenterOGL.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCenterOGL.Location = new System.Drawing.Point(39, 450);
            this.btnCenterOGL.Name = "btnCenterOGL";
            this.btnCenterOGL.Size = new System.Drawing.Size(76, 71);
            this.btnCenterOGL.TabIndex = 560;
            this.btnCenterOGL.UseVisualStyleBackColor = false;
            this.btnCenterOGL.Click += new System.EventHandler(this.btnCenterOGL_Click);
            // 
            // btnSlice
            // 
            this.btnSlice.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSlice.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnSlice.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnSlice, 3);
            this.btnSlice.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSlice.FlatAppearance.BorderSize = 0;
            this.btnSlice.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSlice.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSlice.Image = global::AgOpenGPS.Properties.Resources.BoundaryMakeLine;
            this.btnSlice.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSlice.Location = new System.Drawing.Point(188, 342);
            this.btnSlice.Name = "btnSlice";
            this.btnSlice.Size = new System.Drawing.Size(76, 71);
            this.btnSlice.TabIndex = 548;
            this.btnSlice.UseVisualStyleBackColor = false;
            this.btnSlice.Click += new System.EventHandler(this.btnSlice_Click);
            // 
            // btnMakeBoundary
            // 
            this.btnMakeBoundary.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnMakeBoundary.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnMakeBoundary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnMakeBoundary, 3);
            this.btnMakeBoundary.Enabled = false;
            this.btnMakeBoundary.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMakeBoundary.FlatAppearance.BorderSize = 0;
            this.btnMakeBoundary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeBoundary.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeBoundary.Image = global::AgOpenGPS.Properties.Resources.BoundaryOuter;
            this.btnMakeBoundary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeBoundary.Location = new System.Drawing.Point(171, 232);
            this.btnMakeBoundary.Name = "btnMakeBoundary";
            this.btnMakeBoundary.Size = new System.Drawing.Size(110, 68);
            this.btnMakeBoundary.TabIndex = 545;
            this.btnMakeBoundary.UseVisualStyleBackColor = false;
            this.btnMakeBoundary.Click += new System.EventHandler(this.btnMakeBoundary_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnStartStop.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStartStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnStartStop, 3);
            this.btnStartStop.Enabled = false;
            this.btnStartStop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStartStop.FlatAppearance.BorderSize = 0;
            this.btnStartStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStop.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnStartStop.Image = global::AgOpenGPS.Properties.Resources.BoundaryReduce;
            this.btnStartStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartStop.Location = new System.Drawing.Point(169, 120);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(115, 61);
            this.btnStartStop.TabIndex = 544;
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
            // 
            // btnStart
            // 
            this.btnStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStart.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnStart, 3);
            this.btnStart.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStart.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnStart.Image = global::AgOpenGPS.Properties.Resources.SwitchOn;
            this.btnStart.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStart.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStart.Location = new System.Drawing.Point(30, 4);
            this.btnStart.Margin = new System.Windows.Forms.Padding(0);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(93, 81);
            this.btnStart.TabIndex = 540;
            this.btnStart.Text = "Start";
            this.btnStart.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStart.UseVisualStyleBackColor = false;
            this.btnStart.Click += new System.EventHandler(this.btnResetReduce_Click);
            // 
            // btnCancelTouch
            // 
            this.btnCancelTouch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelTouch.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnCancelTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnCancelTouch, 3);
            this.btnCancelTouch.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnCancelTouch.FlatAppearance.BorderSize = 0;
            this.btnCancelTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTouch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelTouch.Image = global::AgOpenGPS.Properties.Resources.HeadlandDeletePoints;
            this.btnCancelTouch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelTouch.Location = new System.Drawing.Point(188, 450);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(76, 71);
            this.btnCancelTouch.TabIndex = 470;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.btnCancelTouch_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.tlp1.SetColumnSpan(this.btnExit, 2);
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.Location = new System.Drawing.Point(229, 623);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 70);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZoomOut.BackColor = System.Drawing.Color.White;
            this.btnZoomOut.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnZoomOut, 2);
            this.btnZoomOut.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnZoomOut.FlatAppearance.BorderSize = 0;
            this.btnZoomOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomOut.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomOut.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZoomOut.Location = new System.Drawing.Point(15, 628);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(55, 60);
            this.btnZoomOut.TabIndex = 570;
            this.btnZoomOut.Text = "-";
            this.btnZoomOut.UseVisualStyleBackColor = false;
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnZoomIn.BackColor = System.Drawing.Color.White;
            this.btnZoomIn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnZoomIn, 2);
            this.btnZoomIn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZoomIn.Location = new System.Drawing.Point(126, 628);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(55, 60);
            this.btnZoomIn.TabIndex = 571;
            this.btnZoomIn.Text = "+";
            this.btnZoomIn.UseVisualStyleBackColor = false;
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tlp1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlp1.ColumnCount = 6;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.75251F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.675585F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.07692F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.675585F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.40803F));
            this.tlp1.Controls.Add(this.btnMoveUp, 2, 7);
            this.tlp1.Controls.Add(this.btnMoveLeft, 3, 7);
            this.tlp1.Controls.Add(this.btnMoveRight, 5, 7);
            this.tlp1.Controls.Add(this.btnMoveDn, 0, 7);
            this.tlp1.Controls.Add(this.labelSmooth, 0, 3);
            this.tlp1.Controls.Add(this.btnStart, 0, 0);
            this.tlp1.Controls.Add(this.cboxPointDistance, 0, 2);
            this.tlp1.Controls.Add(this.btnCancelTouch, 3, 6);
            this.tlp1.Controls.Add(this.btnSlice, 3, 5);
            this.tlp1.Controls.Add(this.btnCenterOGL, 0, 6);
            this.tlp1.Controls.Add(this.btnStartStop, 3, 2);
            this.tlp1.Controls.Add(this.labelSpacing, 0, 1);
            this.tlp1.Controls.Add(this.labelReducedPoints, 3, 1);
            this.tlp1.Controls.Add(this.btnMakeBoundary, 3, 4);
            this.tlp1.Controls.Add(this.cboxSmooth, 0, 4);
            this.tlp1.Controls.Add(this.cboxIsZoom, 0, 5);
            this.tlp1.Controls.Add(this.labelCreate, 3, 3);
            this.tlp1.Controls.Add(this.btnAddPoints, 3, 0);
            this.tlp1.Controls.Add(this.btnZoomOut, 0, 8);
            this.tlp1.Controls.Add(this.btnZoomIn, 2, 8);
            this.tlp1.Controls.Add(this.btnExit, 4, 8);
            this.tlp1.Location = new System.Drawing.Point(701, 0);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 9;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.57143F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.428571F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.28571F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.85714F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.19544F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.82085F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.84365F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.Size = new System.Drawing.Size(299, 700);
            this.tlp1.TabIndex = 572;
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveUp.BackColor = System.Drawing.Color.White;
            this.btnMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMoveUp.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMoveUp.FlatAppearance.BorderSize = 0;
            this.btnMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveUp.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnMoveUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMoveUp.Location = new System.Drawing.Point(92, 544);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(55, 60);
            this.btnMoveUp.TabIndex = 575;
            this.btnMoveUp.UseVisualStyleBackColor = false;
            this.btnMoveUp.Click += new System.EventHandler(this.btnMoveUp_Click);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveLeft.BackColor = System.Drawing.Color.White;
            this.btnMoveLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMoveLeft.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMoveLeft.FlatAppearance.BorderSize = 0;
            this.btnMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveLeft.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveLeft.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnMoveLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMoveLeft.Location = new System.Drawing.Point(161, 544);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(55, 60);
            this.btnMoveLeft.TabIndex = 576;
            this.btnMoveLeft.UseVisualStyleBackColor = false;
            this.btnMoveLeft.Click += new System.EventHandler(this.btnMoveLeft_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveRight.BackColor = System.Drawing.Color.White;
            this.btnMoveRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMoveRight.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMoveRight.FlatAppearance.BorderSize = 0;
            this.btnMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveRight.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveRight.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnMoveRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMoveRight.Location = new System.Drawing.Point(237, 544);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(55, 60);
            this.btnMoveRight.TabIndex = 577;
            this.btnMoveRight.UseVisualStyleBackColor = false;
            this.btnMoveRight.Click += new System.EventHandler(this.btnMoveRight_Click);
            // 
            // btnMoveDn
            // 
            this.btnMoveDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveDn.BackColor = System.Drawing.Color.White;
            this.btnMoveDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMoveDn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnMoveDn.FlatAppearance.BorderSize = 0;
            this.btnMoveDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveDn.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDn.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnMoveDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMoveDn.Location = new System.Drawing.Point(11, 544);
            this.btnMoveDn.Name = "btnMoveDn";
            this.btnMoveDn.Size = new System.Drawing.Size(55, 60);
            this.btnMoveDn.TabIndex = 574;
            this.btnMoveDn.UseVisualStyleBackColor = false;
            this.btnMoveDn.Click += new System.EventHandler(this.btnMoveDn_Click);
            // 
            // labelCreate
            // 
            this.labelCreate.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelCreate.AutoSize = true;
            this.tlp1.SetColumnSpan(this.labelCreate, 3);
            this.labelCreate.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCreate.ForeColor = System.Drawing.Color.Black;
            this.labelCreate.Location = new System.Drawing.Point(194, 210);
            this.labelCreate.Name = "labelCreate";
            this.labelCreate.Size = new System.Drawing.Size(64, 19);
            this.labelCreate.TabIndex = 572;
            this.labelCreate.Text = "Create";
            this.labelCreate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddPoints
            // 
            this.btnAddPoints.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddPoints.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddPoints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnAddPoints, 3);
            this.btnAddPoints.Enabled = false;
            this.btnAddPoints.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddPoints.FlatAppearance.BorderSize = 0;
            this.btnAddPoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddPoints.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAddPoints.Image = global::AgOpenGPS.Properties.Resources.PointAdd;
            this.btnAddPoints.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddPoints.Location = new System.Drawing.Point(189, 11);
            this.btnAddPoints.Name = "btnAddPoints";
            this.btnAddPoints.Size = new System.Drawing.Size(74, 68);
            this.btnAddPoints.TabIndex = 574;
            this.btnAddPoints.UseVisualStyleBackColor = false;
            this.btnAddPoints.Click += new System.EventHandler(this.btnAddPoints_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.MistyRose;
            this.panel1.Controls.Add(this.labelPleaseWait);
            this.panel1.Controls.Add(this.lblPointToProcess);
            this.panel1.Controls.Add(this.labelPointsToProcess);
            this.panel1.Controls.Add(this.lblI);
            this.panel1.Controls.Add(this.labelPoints);
            this.panel1.Location = new System.Drawing.Point(137, 181);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(445, 305);
            this.panel1.TabIndex = 573;
            // 
            // labelPleaseWait
            // 
            this.labelPleaseWait.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPleaseWait.ForeColor = System.Drawing.Color.Black;
            this.labelPleaseWait.Location = new System.Drawing.Point(159, 71);
            this.labelPleaseWait.Name = "labelPleaseWait";
            this.labelPleaseWait.Size = new System.Drawing.Size(161, 25);
            this.labelPleaseWait.TabIndex = 529;
            this.labelPleaseWait.Text = "Please Wait";
            this.labelPleaseWait.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPointToProcess
            // 
            this.lblPointToProcess.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPointToProcess.ForeColor = System.Drawing.Color.Black;
            this.lblPointToProcess.Location = new System.Drawing.Point(260, 152);
            this.lblPointToProcess.Name = "lblPointToProcess";
            this.lblPointToProcess.Size = new System.Drawing.Size(98, 25);
            this.lblPointToProcess.TabIndex = 527;
            this.lblPointToProcess.Text = "100,000";
            this.lblPointToProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPointsToProcess
            // 
            this.labelPointsToProcess.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPointsToProcess.ForeColor = System.Drawing.Color.Black;
            this.labelPointsToProcess.Location = new System.Drawing.Point(84, 151);
            this.labelPointsToProcess.Name = "labelPointsToProcess";
            this.labelPointsToProcess.Size = new System.Drawing.Size(180, 25);
            this.labelPointsToProcess.TabIndex = 526;
            this.labelPointsToProcess.Text = "Points to Process: ";
            this.labelPointsToProcess.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblI
            // 
            this.lblI.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblI.ForeColor = System.Drawing.Color.Black;
            this.lblI.Location = new System.Drawing.Point(205, 202);
            this.lblI.Name = "lblI";
            this.lblI.Size = new System.Drawing.Size(139, 25);
            this.lblI.TabIndex = 525;
            this.lblI.Text = "Points";
            this.lblI.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelPoints
            // 
            this.labelPoints.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPoints.ForeColor = System.Drawing.Color.Black;
            this.labelPoints.Location = new System.Drawing.Point(132, 201);
            this.labelPoints.Name = "labelPoints";
            this.labelPoints.Size = new System.Drawing.Size(82, 25);
            this.labelPoints.TabIndex = 528;
            this.labelPoints.Text = "Points:";
            this.labelPoints.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FormBndTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightGray;
            this.ClientSize = new System.Drawing.Size(1006, 726);
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tlp1);
            this.Controls.Add(this.oglSelf);
            this.ForeColor = System.Drawing.Color.Black;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1022, 742);
            this.Name = "FormBndTool";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Create Boundary From Mapping";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormBndTool_FormClosing);
            this.Load += new System.EventHandler(this.FormBndTool_Load);
            this.ResizeEnd += new System.EventHandler(this.FormBndTool_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            this.tlp1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.ComboBox cboxPointDistance;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Label labelReducedPoints;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnMakeBoundary;
        private System.Windows.Forms.ComboBox cboxSmooth;
        private System.Windows.Forms.Button btnSlice;
        private System.Windows.Forms.Button btnCenterOGL;
        private System.Windows.Forms.CheckBox cboxIsZoom;
        private System.Windows.Forms.Label labelSpacing;
        private System.Windows.Forms.Label labelSmooth;
        private System.Windows.Forms.Button btnZoomOut;
        private System.Windows.Forms.Button btnZoomIn;
        private System.Windows.Forms.TableLayoutPanel tlp1;
        private System.Windows.Forms.Label labelCreate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblI;
        private System.Windows.Forms.Label lblPointToProcess;
        private System.Windows.Forms.Label labelPointsToProcess;
        private System.Windows.Forms.Label labelPoints;
        private System.Windows.Forms.Label labelPleaseWait;
        private System.Windows.Forms.Button btnAddPoints;
        private System.Windows.Forms.Button btnMoveDn;
        private System.Windows.Forms.Button btnMoveUp;
        private System.Windows.Forms.Button btnMoveLeft;
        private System.Windows.Forms.Button btnMoveRight;
    }
}
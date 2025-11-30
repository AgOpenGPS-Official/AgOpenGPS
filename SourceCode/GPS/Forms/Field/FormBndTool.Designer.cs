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
            this.oglSelf = new OpenTK.GLControl();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.cboxPointDistance = new System.Windows.Forms.ComboBox();
            this.cboxSmooth = new System.Windows.Forms.ComboBox();
            this.labelSpacing = new System.Windows.Forms.Label();
            this.labelSmooth = new System.Windows.Forms.Label();
            this.cboxIsZoom = new System.Windows.Forms.CheckBox();
            this.btnCenterOGL = new System.Windows.Forms.Button();
            this.btnSlice = new System.Windows.Forms.Button();
            this.btnAddBoundary = new System.Windows.Forms.Button();
            this.btnStartStop = new System.Windows.Forms.Button();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnZoomOut = new System.Windows.Forms.Button();
            this.btnZoomIn = new System.Windows.Forms.Button();
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.labelReset = new System.Windows.Forms.Label();
            this.btnMoveUp = new System.Windows.Forms.Button();
            this.btnMoveLeft = new System.Windows.Forms.Button();
            this.btnMoveRight = new System.Windows.Forms.Button();
            this.btnMoveDn = new System.Windows.Forms.Button();
            this.labelCreate = new System.Windows.Forms.Label();
            this.btnAddPoints = new System.Windows.Forms.Button();
            this.btnResetReduce = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelPleaseWait = new System.Windows.Forms.Label();
            this.lblPointToProcess = new System.Windows.Forms.Label();
            this.labelPointsToProcess = new System.Windows.Forms.Label();
            this.lblI = new System.Windows.Forms.Label();
            this.labelPoints = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tlp1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // oglSelf
            // 
            this.oglSelf.BackColor = System.Drawing.Color.Black;
            this.oglSelf.Cursor = System.Windows.Forms.Cursors.Cross;
            this.oglSelf.Location = new System.Drawing.Point(0, 0);
            this.oglSelf.Margin = new System.Windows.Forms.Padding(0);
            this.oglSelf.Name = "oglSelf";
            this.oglSelf.Size = new System.Drawing.Size(700, 700);
            this.oglSelf.TabIndex = 183;
            this.oglSelf.VSync = false;
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
            this.cboxPointDistance.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cboxPointDistance.BackColor = System.Drawing.Color.Lavender;
            this.tlp1.SetColumnSpan(this.cboxPointDistance, 3);
            this.cboxPointDistance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.cboxPointDistance.Location = new System.Drawing.Point(26, 16);
            this.cboxPointDistance.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxPointDistance.Name = "cboxPointDistance";
            this.cboxPointDistance.Size = new System.Drawing.Size(101, 53);
            this.cboxPointDistance.TabIndex = 541;
            this.cboxPointDistance.SelectedIndexChanged += new System.EventHandler(this.cboxPointDistance_SelectedIndexChanged);
            // 
            // cboxSmooth
            // 
            this.cboxSmooth.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cboxSmooth.BackColor = System.Drawing.Color.Lavender;
            this.tlp1.SetColumnSpan(this.cboxSmooth, 3);
            this.cboxSmooth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
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
            this.cboxSmooth.Location = new System.Drawing.Point(176, 16);
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
            this.labelSpacing.Location = new System.Drawing.Point(12, 80);
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
            this.labelSmooth.Location = new System.Drawing.Point(191, 86);
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
            this.tlp1.SetColumnSpan(this.cboxIsZoom, 3);
            this.cboxIsZoom.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsZoom.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(160)))));
            this.cboxIsZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsZoom.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsZoom.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsZoom.Image = global::AgOpenGPS.Properties.Resources.ZoomOGL;
            this.cboxIsZoom.Location = new System.Drawing.Point(188, 457);
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
            this.btnCenterOGL.Location = new System.Drawing.Point(39, 457);
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
            this.btnSlice.Location = new System.Drawing.Point(188, 352);
            this.btnSlice.Name = "btnSlice";
            this.btnSlice.Size = new System.Drawing.Size(76, 71);
            this.btnSlice.TabIndex = 548;
            this.btnSlice.UseVisualStyleBackColor = false;
            this.btnSlice.Click += new System.EventHandler(this.btnSlice_Click);
            // 
            // btnAddBoundary
            // 
            this.btnAddBoundary.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnAddBoundary.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnAddBoundary.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnAddBoundary, 3);
            this.btnAddBoundary.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddBoundary.FlatAppearance.BorderSize = 0;
            this.btnAddBoundary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddBoundary.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAddBoundary.Image = global::AgOpenGPS.Properties.Resources.BoundaryOuter;
            this.btnAddBoundary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddBoundary.Location = new System.Drawing.Point(171, 235);
            this.btnAddBoundary.Name = "btnAddBoundary";
            this.btnAddBoundary.Size = new System.Drawing.Size(110, 68);
            this.btnAddBoundary.TabIndex = 545;
            this.btnAddBoundary.UseVisualStyleBackColor = false;
            this.btnAddBoundary.Click += new System.EventHandler(this.btnAddBoundary_Click);
            // 
            // btnStartStop
            // 
            this.btnStartStop.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
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
            this.btnStartStop.Location = new System.Drawing.Point(19, 242);
            this.btnStartStop.Name = "btnStartStop";
            this.btnStartStop.Size = new System.Drawing.Size(115, 61);
            this.btnStartStop.TabIndex = 544;
            this.btnStartStop.UseVisualStyleBackColor = false;
            this.btnStartStop.Click += new System.EventHandler(this.btnStartStop_Click);
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
            this.btnCancelTouch.Location = new System.Drawing.Point(39, 352);
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
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.Location = new System.Drawing.Point(229, 625);
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
            this.btnZoomOut.Location = new System.Drawing.Point(15, 630);
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
            this.btnZoomIn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnZoomIn.FlatAppearance.BorderSize = 0;
            this.btnZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZoomIn.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZoomIn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZoomIn.Location = new System.Drawing.Point(92, 630);
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
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.83893F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.684564F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.15436F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.15436F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.684564F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.48323F));
            this.tlp1.Controls.Add(this.btnCancel, 3, 9);
            this.tlp1.Controls.Add(this.label3, 0, 5);
            this.tlp1.Controls.Add(this.labelReset, 0, 3);
            this.tlp1.Controls.Add(this.btnMoveUp, 2, 8);
            this.tlp1.Controls.Add(this.btnMoveLeft, 3, 8);
            this.tlp1.Controls.Add(this.btnMoveRight, 5, 8);
            this.tlp1.Controls.Add(this.cboxIsZoom, 3, 7);
            this.tlp1.Controls.Add(this.btnMoveDn, 0, 8);
            this.tlp1.Controls.Add(this.btnStartStop, 0, 4);
            this.tlp1.Controls.Add(this.cboxPointDistance, 0, 0);
            this.tlp1.Controls.Add(this.cboxSmooth, 3, 0);
            this.tlp1.Controls.Add(this.btnSlice, 3, 6);
            this.tlp1.Controls.Add(this.btnCenterOGL, 0, 7);
            this.tlp1.Controls.Add(this.labelSpacing, 0, 1);
            this.tlp1.Controls.Add(this.btnAddBoundary, 3, 4);
            this.tlp1.Controls.Add(this.labelCreate, 3, 3);
            this.tlp1.Controls.Add(this.btnZoomOut, 0, 9);
            this.tlp1.Controls.Add(this.btnZoomIn, 2, 9);
            this.tlp1.Controls.Add(this.btnExit, 4, 9);
            this.tlp1.Controls.Add(this.btnAddPoints, 3, 2);
            this.tlp1.Controls.Add(this.btnResetReduce, 0, 2);
            this.tlp1.Controls.Add(this.labelSmooth, 3, 1);
            this.tlp1.Controls.Add(this.label2, 3, 5);
            this.tlp1.Controls.Add(this.btnCancelTouch, 0, 6);
            this.tlp1.Location = new System.Drawing.Point(701, 0);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 10;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.00456F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.506683F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.71025F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.003683F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.8699F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 21F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20.21404F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.8345F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.8564F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.Size = new System.Drawing.Size(299, 700);
            this.tlp1.TabIndex = 572;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label3.AutoSize = true;
            this.tlp1.SetColumnSpan(this.label3, 3);
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(52, 308);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 19);
            this.label3.TabIndex = 581;
            this.label3.Text = "Build";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReset
            // 
            this.labelReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.labelReset.AutoSize = true;
            this.tlp1.SetColumnSpan(this.labelReset, 3);
            this.labelReset.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReset.ForeColor = System.Drawing.Color.Black;
            this.labelReset.Location = new System.Drawing.Point(49, 204);
            this.labelReset.Name = "labelReset";
            this.labelReset.Size = new System.Drawing.Size(56, 19);
            this.labelReset.TabIndex = 579;
            this.labelReset.Text = "Reset";
            this.labelReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btnMoveUp.Location = new System.Drawing.Point(92, 548);
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
            this.btnMoveLeft.Location = new System.Drawing.Point(161, 548);
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
            this.btnMoveRight.Location = new System.Drawing.Point(237, 548);
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
            this.btnMoveDn.Location = new System.Drawing.Point(11, 548);
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
            this.labelCreate.Location = new System.Drawing.Point(178, 204);
            this.labelCreate.Name = "labelCreate";
            this.labelCreate.Size = new System.Drawing.Size(97, 19);
            this.labelCreate.TabIndex = 572;
            this.labelCreate.Text = "Add Points";
            this.labelCreate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAddPoints
            // 
            this.btnAddPoints.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
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
            this.btnAddPoints.Location = new System.Drawing.Point(189, 128);
            this.btnAddPoints.Name = "btnAddPoints";
            this.btnAddPoints.Size = new System.Drawing.Size(74, 68);
            this.btnAddPoints.TabIndex = 574;
            this.btnAddPoints.UseVisualStyleBackColor = false;
            this.btnAddPoints.Click += new System.EventHandler(this.btnAddPoints_Click);
            // 
            // btnResetReduce
            // 
            this.btnResetReduce.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnResetReduce.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnResetReduce.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnResetReduce, 3);
            this.btnResetReduce.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnResetReduce.FlatAppearance.BorderSize = 0;
            this.btnResetReduce.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetReduce.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnResetReduce.Image = global::AgOpenGPS.Properties.Resources.back_button;
            this.btnResetReduce.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnResetReduce.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetReduce.Location = new System.Drawing.Point(39, 128);
            this.btnResetReduce.Margin = new System.Windows.Forms.Padding(0);
            this.btnResetReduce.Name = "btnResetReduce";
            this.btnResetReduce.Size = new System.Drawing.Size(76, 71);
            this.btnResetReduce.TabIndex = 578;
            this.btnResetReduce.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnResetReduce.UseVisualStyleBackColor = false;
            this.btnResetReduce.Click += new System.EventHandler(this.btnResetReduce_Click_1);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.label2.AutoSize = true;
            this.tlp1.SetColumnSpan(this.label2, 3);
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(202, 308);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 19);
            this.label2.TabIndex = 580;
            this.label2.Text = "Save";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(157, 625);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(63, 70);
            this.btnCancel.TabIndex = 582;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
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

        private OpenTK.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.ComboBox cboxPointDistance;
        private System.Windows.Forms.Button btnStartStop;
        private System.Windows.Forms.Button btnAddBoundary;
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
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelReset;
        private System.Windows.Forms.Button btnResetReduce;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
    }
}
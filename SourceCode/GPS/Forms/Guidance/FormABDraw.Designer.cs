namespace AgOpenGPS
{
    partial class FormABDraw
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
            this.lblCurveSelected = new System.Windows.Forms.Label();
            this.tboxNameCurve = new System.Windows.Forms.TextBox();
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnBLength = new System.Windows.Forms.Button();
            this.btnDrawSections = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnALength = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnDeleteCurve = new System.Windows.Forms.Button();
            this.btnMakeCurve = new System.Windows.Forms.Button();
            this.btnMakeABLine = new System.Windows.Forms.Button();
            this.cboxIsZoom = new System.Windows.Forms.CheckBox();
            this.cboxIsVisible = new System.Windows.Forms.CheckBox();
            this.btnMakeBoundaryCurve = new System.Windows.Forms.Button();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.btnSelectCurveBk = new System.Windows.Forms.Button();
            this.btnSelectCurve = new System.Windows.Forms.Button();
            this.btnAddTime = new System.Windows.Forms.Button();
            this.tlp1.SuspendLayout();
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
            this.oglSelf.Location = new System.Drawing.Point(2, 2);
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
            // lblCurveSelected
            // 
            this.lblCurveSelected.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCurveSelected.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveSelected.ForeColor = System.Drawing.Color.Black;
            this.lblCurveSelected.Location = new System.Drawing.Point(9, 570);
            this.lblCurveSelected.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurveSelected.Name = "lblCurveSelected";
            this.lblCurveSelected.Size = new System.Drawing.Size(130, 26);
            this.lblCurveSelected.TabIndex = 329;
            this.lblCurveSelected.Text = "1";
            this.lblCurveSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tboxNameCurve
            // 
            this.tboxNameCurve.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxNameCurve.BackColor = System.Drawing.SystemColors.ControlLight;
            this.tboxNameCurve.CausesValidation = false;
            this.tlp1.SetColumnSpan(this.tboxNameCurve, 2);
            this.tboxNameCurve.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxNameCurve.Location = new System.Drawing.Point(0, 519);
            this.tboxNameCurve.Margin = new System.Windows.Forms.Padding(0);
            this.tboxNameCurve.MaxLength = 100;
            this.tboxNameCurve.Name = "tboxNameCurve";
            this.tboxNameCurve.Size = new System.Drawing.Size(299, 36);
            this.tboxNameCurve.TabIndex = 10;
            this.tboxNameCurve.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxNameCurve.Enter += new System.EventHandler(this.tboxNameCurve_Enter);
            this.tboxNameCurve.Leave += new System.EventHandler(this.tboxNameCurve_Leave);
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp1.ColumnCount = 2;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp1.Controls.Add(this.btnBLength, 0, 0);
            this.tlp1.Controls.Add(this.btnDrawSections, 1, 0);
            this.tlp1.Controls.Add(this.btnCancel, 0, 8);
            this.tlp1.Controls.Add(this.btnALength, 0, 1);
            this.tlp1.Controls.Add(this.btnExit, 1, 8);
            this.tlp1.Controls.Add(this.btnDeleteCurve, 1, 3);
            this.tlp1.Controls.Add(this.btnMakeCurve, 0, 4);
            this.tlp1.Controls.Add(this.btnMakeABLine, 1, 4);
            this.tlp1.Controls.Add(this.cboxIsZoom, 0, 3);
            this.tlp1.Controls.Add(this.cboxIsVisible, 0, 2);
            this.tlp1.Controls.Add(this.btnMakeBoundaryCurve, 1, 2);
            this.tlp1.Controls.Add(this.btnCancelTouch, 1, 1);
            this.tlp1.Controls.Add(this.btnSelectCurveBk, 0, 5);
            this.tlp1.Controls.Add(this.btnSelectCurve, 1, 5);
            this.tlp1.Controls.Add(this.lblCurveSelected, 0, 7);
            this.tlp1.Controls.Add(this.btnAddTime, 1, 7);
            this.tlp1.Controls.Add(this.tboxNameCurve, 0, 6);
            this.tlp1.Location = new System.Drawing.Point(703, 2);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 9;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.14701F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.47011F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 17.7706F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.50888F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.34733F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37803F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 39F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 52F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.37803F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.Size = new System.Drawing.Size(299, 700);
            this.tlp1.TabIndex = 564;
            // 
            // btnBLength
            // 
            this.btnBLength.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBLength.BackColor = System.Drawing.Color.Transparent;
            this.btnBLength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBLength.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnBLength.FlatAppearance.BorderSize = 0;
            this.btnBLength.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBLength.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBLength.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBLength.Image = global::AgOpenGPS.Properties.Resources.APlusPlusB;
            this.btnBLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBLength.Location = new System.Drawing.Point(19, 12);
            this.btnBLength.Name = "btnBLength";
            this.btnBLength.Size = new System.Drawing.Size(110, 43);
            this.btnBLength.TabIndex = 351;
            this.btnBLength.UseVisualStyleBackColor = false;
            this.btnBLength.Click += new System.EventHandler(this.btnBLength_Click);
            // 
            // btnDrawSections
            // 
            this.btnDrawSections.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDrawSections.BackColor = System.Drawing.Color.Transparent;
            this.btnDrawSections.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDrawSections.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDrawSections.FlatAppearance.BorderSize = 0;
            this.btnDrawSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawSections.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDrawSections.Image = global::AgOpenGPS.Properties.Resources.MappingOff;
            this.btnDrawSections.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDrawSections.Location = new System.Drawing.Point(188, 3);
            this.btnDrawSections.Name = "btnDrawSections";
            this.btnDrawSections.Size = new System.Drawing.Size(72, 61);
            this.btnDrawSections.TabIndex = 11;
            this.btnDrawSections.UseVisualStyleBackColor = false;
            this.btnDrawSections.Click += new System.EventHandler(this.btnDrawSections_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(30, 626);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 56);
            this.btnCancel.TabIndex = 469;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnALength
            // 
            this.btnALength.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnALength.BackColor = System.Drawing.Color.Transparent;
            this.btnALength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnALength.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.btnALength.FlatAppearance.BorderSize = 0;
            this.btnALength.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnALength.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnALength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnALength.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnALength.Image = global::AgOpenGPS.Properties.Resources.APlusPlusA;
            this.btnALength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnALength.Location = new System.Drawing.Point(19, 80);
            this.btnALength.Name = "btnALength";
            this.btnALength.Size = new System.Drawing.Size(110, 43);
            this.btnALength.TabIndex = 352;
            this.btnALength.UseVisualStyleBackColor = false;
            this.btnALength.Click += new System.EventHandler(this.btnALength_Click);
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
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(180, 626);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(88, 56);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnDeleteCurve
            // 
            this.btnDeleteCurve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDeleteCurve.FlatAppearance.BorderSize = 0;
            this.btnDeleteCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteCurve.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteCurve.Location = new System.Drawing.Point(188, 265);
            this.btnDeleteCurve.Name = "btnDeleteCurve";
            this.btnDeleteCurve.Size = new System.Drawing.Size(72, 52);
            this.btnDeleteCurve.TabIndex = 6;
            this.btnDeleteCurve.UseVisualStyleBackColor = false;
            this.btnDeleteCurve.Click += new System.EventHandler(this.btnDeleteCurve_Click);
            // 
            // btnMakeCurve
            // 
            this.btnMakeCurve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMakeCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeCurve.Enabled = false;
            this.btnMakeCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMakeCurve.FlatAppearance.BorderSize = 0;
            this.btnMakeCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeCurve.Image = global::AgOpenGPS.Properties.Resources.ABTrackCurve;
            this.btnMakeCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeCurve.Location = new System.Drawing.Point(29, 344);
            this.btnMakeCurve.Name = "btnMakeCurve";
            this.btnMakeCurve.Size = new System.Drawing.Size(90, 80);
            this.btnMakeCurve.TabIndex = 2;
            this.btnMakeCurve.UseVisualStyleBackColor = false;
            this.btnMakeCurve.Click += new System.EventHandler(this.BtnMakeCurve_Click);
            // 
            // btnMakeABLine
            // 
            this.btnMakeABLine.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMakeABLine.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeABLine.Enabled = false;
            this.btnMakeABLine.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMakeABLine.FlatAppearance.BorderSize = 0;
            this.btnMakeABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeABLine.Image = global::AgOpenGPS.Properties.Resources.ABTrackAB;
            this.btnMakeABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeABLine.Location = new System.Drawing.Point(179, 344);
            this.btnMakeABLine.Name = "btnMakeABLine";
            this.btnMakeABLine.Size = new System.Drawing.Size(90, 80);
            this.btnMakeABLine.TabIndex = 3;
            this.btnMakeABLine.UseVisualStyleBackColor = false;
            this.btnMakeABLine.Click += new System.EventHandler(this.BtnMakeABLine_Click);
            // 
            // cboxIsZoom
            // 
            this.cboxIsZoom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsZoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsZoom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboxIsZoom.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsZoom.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(160)))));
            this.cboxIsZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsZoom.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsZoom.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsZoom.Image = global::AgOpenGPS.Properties.Resources.ZoomOGL;
            this.cboxIsZoom.Location = new System.Drawing.Point(38, 257);
            this.cboxIsZoom.Name = "cboxIsZoom";
            this.cboxIsZoom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsZoom.Size = new System.Drawing.Size(72, 68);
            this.cboxIsZoom.TabIndex = 563;
            this.cboxIsZoom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsZoom.UseVisualStyleBackColor = false;
            this.cboxIsZoom.CheckedChanged += new System.EventHandler(this.cboxIsZoom_CheckedChanged);
            // 
            // cboxIsVisible
            // 
            this.cboxIsVisible.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsVisible.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsVisible.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cboxIsVisible.Checked = true;
            this.cboxIsVisible.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsVisible.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsVisible.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.cboxIsVisible.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsVisible.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsVisible.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsVisible.Image = global::AgOpenGPS.Properties.Resources.TrackVisible;
            this.cboxIsVisible.Location = new System.Drawing.Point(38, 156);
            this.cboxIsVisible.Name = "cboxIsVisible";
            this.cboxIsVisible.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsVisible.Size = new System.Drawing.Size(72, 68);
            this.cboxIsVisible.TabIndex = 468;
            this.cboxIsVisible.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsVisible.UseVisualStyleBackColor = false;
            this.cboxIsVisible.Click += new System.EventHandler(this.cboxIsVisible_Click);
            // 
            // btnMakeBoundaryCurve
            // 
            this.btnMakeBoundaryCurve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMakeBoundaryCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnMakeBoundaryCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeBoundaryCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnMakeBoundaryCurve.FlatAppearance.BorderSize = 0;
            this.btnMakeBoundaryCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeBoundaryCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeBoundaryCurve.Image = global::AgOpenGPS.Properties.Resources.BoundaryCurveLine;
            this.btnMakeBoundaryCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeBoundaryCurve.Location = new System.Drawing.Point(174, 156);
            this.btnMakeBoundaryCurve.Name = "btnMakeBoundaryCurve";
            this.btnMakeBoundaryCurve.Size = new System.Drawing.Size(100, 68);
            this.btnMakeBoundaryCurve.TabIndex = 4;
            this.btnMakeBoundaryCurve.UseVisualStyleBackColor = false;
            this.btnMakeBoundaryCurve.Click += new System.EventHandler(this.btnMakeBoundaryCurve_Click);
            // 
            // btnCancelTouch
            // 
            this.btnCancelTouch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelTouch.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelTouch.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancelTouch.FlatAppearance.BorderSize = 0;
            this.btnCancelTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTouch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelTouch.Image = global::AgOpenGPS.Properties.Resources.HeadlandDeletePoints;
            this.btnCancelTouch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelTouch.Location = new System.Drawing.Point(188, 70);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(72, 63);
            this.btnCancelTouch.TabIndex = 470;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.btnCancelTouch_Click);
            // 
            // btnSelectCurveBk
            // 
            this.btnSelectCurveBk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSelectCurveBk.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectCurveBk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSelectCurveBk.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSelectCurveBk.FlatAppearance.BorderSize = 0;
            this.btnSelectCurveBk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCurveBk.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSelectCurveBk.Image = global::AgOpenGPS.Properties.Resources.ABLineCycleBk;
            this.btnSelectCurveBk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectCurveBk.Location = new System.Drawing.Point(10, 440);
            this.btnSelectCurveBk.Name = "btnSelectCurveBk";
            this.btnSelectCurveBk.Size = new System.Drawing.Size(128, 68);
            this.btnSelectCurveBk.TabIndex = 472;
            this.btnSelectCurveBk.UseVisualStyleBackColor = false;
            this.btnSelectCurveBk.Click += new System.EventHandler(this.btnSelectCurveBk_Click);
            // 
            // btnSelectCurve
            // 
            this.btnSelectCurve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSelectCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnSelectCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSelectCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSelectCurve.FlatAppearance.BorderSize = 0;
            this.btnSelectCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSelectCurve.Image = global::AgOpenGPS.Properties.Resources.ABLineCycle;
            this.btnSelectCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectCurve.Location = new System.Drawing.Point(160, 440);
            this.btnSelectCurve.Name = "btnSelectCurve";
            this.btnSelectCurve.Size = new System.Drawing.Size(128, 68);
            this.btnSelectCurve.TabIndex = 5;
            this.btnSelectCurve.UseVisualStyleBackColor = false;
            this.btnSelectCurve.Click += new System.EventHandler(this.btnSelectCurve_Click);
            // 
            // btnAddTime
            // 
            this.btnAddTime.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddTime.BackColor = System.Drawing.Color.Transparent;
            this.btnAddTime.BackgroundImage = global::AgOpenGPS.Properties.Resources.JobNameTime;
            this.btnAddTime.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAddTime.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnAddTime.FlatAppearance.BorderSize = 0;
            this.btnAddTime.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddTime.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddTime.Location = new System.Drawing.Point(152, 562);
            this.btnAddTime.Name = "btnAddTime";
            this.btnAddTime.Size = new System.Drawing.Size(144, 42);
            this.btnAddTime.TabIndex = 356;
            this.btnAddTime.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddTime.UseVisualStyleBackColor = false;
            this.btnAddTime.Click += new System.EventHandler(this.btnAddTime_Click);
            // 
            // FormABDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1006, 703);
            this.ControlBox = false;
            this.Controls.Add(this.tlp1);
            this.Controls.Add(this.oglSelf);
            this.ForeColor = System.Drawing.Color.Black;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1022, 742);
            this.Name = "FormABDraw";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Draw AB - Click 2 points on the Boundary to Begin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormABDraw_FormClosing);
            this.Load += new System.EventHandler(this.FormABDraw_Load);
            this.ResizeEnd += new System.EventHandler(this.FormABDraw_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            this.tlp1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnMakeABLine;
        private System.Windows.Forms.Button btnMakeCurve;
        private System.Windows.Forms.Button btnSelectCurve;
        private System.Windows.Forms.Button btnDeleteCurve;
        private System.Windows.Forms.Label lblCurveSelected;
        private System.Windows.Forms.Button btnDrawSections;
        private System.Windows.Forms.Button btnMakeBoundaryCurve;
        private System.Windows.Forms.TextBox tboxNameCurve;
        private System.Windows.Forms.Button btnBLength;
        private System.Windows.Forms.Button btnALength;
        private System.Windows.Forms.Button btnAddTime;
        private System.Windows.Forms.CheckBox cboxIsVisible;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.Button btnSelectCurveBk;
        private System.Windows.Forms.CheckBox cboxIsZoom;
        private System.Windows.Forms.TableLayoutPanel tlp1;
    }
}
namespace AgOpenGPS
{
    partial class FormTramLine
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
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnDnAlpha = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.labelAlpha = new System.Windows.Forms.Label();
            this.cboxIsOuter = new System.Windows.Forms.CheckBox();
            this.btnDeleteAllTrams = new System.Windows.Forms.Button();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.btnSwapAB = new System.Windows.Forms.Button();
            this.lblNumPasses = new System.Windows.Forms.Label();
            this.lblStartPass = new System.Windows.Forms.Label();
            this.btnSelectCurveBk = new System.Windows.Forms.Button();
            this.btnSelectCurve = new System.Windows.Forms.Button();
            this.btnAddLines = new System.Windows.Forms.Button();
            this.lblAplha = new System.Windows.Forms.Label();
            this.btnUpAlpha = new System.Windows.Forms.Button();
            this.btnDnTrams = new System.Windows.Forms.Button();
            this.btnDnStartTram = new System.Windows.Forms.Button();
            this.btnUpStartTram = new System.Windows.Forms.Button();
            this.btnUpTrams = new System.Windows.Forms.Button();
            this.btnResize = new System.Windows.Forms.Button();
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
            this.oglSelf.Location = new System.Drawing.Point(1, 0);
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
            this.lblCurveSelected.Location = new System.Drawing.Point(109, 287);
            this.lblCurveSelected.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurveSelected.Name = "lblCurveSelected";
            this.lblCurveSelected.Size = new System.Drawing.Size(78, 26);
            this.lblCurveSelected.TabIndex = 329;
            this.lblCurveSelected.Text = "1";
            this.lblCurveSelected.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp1.BackColor = System.Drawing.Color.Transparent;
            this.tlp1.ColumnCount = 3;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 32.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlp1.Controls.Add(this.btnDnAlpha, 0, 0);
            this.tlp1.Controls.Add(this.btnCancel, 0, 8);
            this.tlp1.Controls.Add(this.btnSave, 2, 8);
            this.tlp1.Controls.Add(this.labelAlpha, 1, 0);
            this.tlp1.Controls.Add(this.cboxIsOuter, 2, 3);
            this.tlp1.Controls.Add(this.btnDeleteAllTrams, 1, 2);
            this.tlp1.Controls.Add(this.btnCancelTouch, 0, 3);
            this.tlp1.Controls.Add(this.btnSwapAB, 0, 2);
            this.tlp1.Controls.Add(this.lblNumPasses, 1, 6);
            this.tlp1.Controls.Add(this.btnSelectCurveBk, 0, 4);
            this.tlp1.Controls.Add(this.lblCurveSelected, 1, 4);
            this.tlp1.Controls.Add(this.btnSelectCurve, 2, 4);
            this.tlp1.Controls.Add(this.btnAddLines, 1, 7);
            this.tlp1.Controls.Add(this.lblAplha, 1, 1);
            this.tlp1.Controls.Add(this.btnUpAlpha, 2, 0);
            this.tlp1.Controls.Add(this.btnDnTrams, 0, 6);
            this.tlp1.Controls.Add(this.btnDnStartTram, 0, 5);
            this.tlp1.Controls.Add(this.btnUpStartTram, 2, 5);
            this.tlp1.Controls.Add(this.btnUpTrams, 2, 6);
            this.tlp1.Controls.Add(this.btnResize, 2, 2);
            this.tlp1.Controls.Add(this.lblStartPass, 1, 5);
            this.tlp1.Location = new System.Drawing.Point(703, 0);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 9;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.10334F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.30843F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.83148F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.62639F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.85215F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.7186F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.55962F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.Size = new System.Drawing.Size(300, 701);
            this.tlp1.TabIndex = 564;
            // 
            // btnDnAlpha
            // 
            this.btnDnAlpha.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDnAlpha.BackColor = System.Drawing.Color.Transparent;
            this.btnDnAlpha.FlatAppearance.BorderSize = 0;
            this.btnDnAlpha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDnAlpha.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDnAlpha.ForeColor = System.Drawing.Color.White;
            this.btnDnAlpha.Image = global::AgOpenGPS.Properties.Resources.IncrementMinus;
            this.btnDnAlpha.Location = new System.Drawing.Point(13, 5);
            this.btnDnAlpha.Name = "btnDnAlpha";
            this.tlp1.SetRowSpan(this.btnDnAlpha, 2);
            this.btnDnAlpha.Size = new System.Drawing.Size(72, 62);
            this.btnDnAlpha.TabIndex = 584;
            this.btnDnAlpha.UseVisualStyleBackColor = false;
            this.btnDnAlpha.Click += new System.EventHandler(this.btnDnAlpha_Click);
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
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(5, 626);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 70);
            this.btnCancel.TabIndex = 469;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSave.BackColor = System.Drawing.Color.Transparent;
            this.btnSave.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.btnSave.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSave.Location = new System.Drawing.Point(204, 626);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(88, 70);
            this.btnSave.TabIndex = 0;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // labelAlpha
            // 
            this.labelAlpha.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.labelAlpha.AutoSize = true;
            this.labelAlpha.BackColor = System.Drawing.Color.WhiteSmoke;
            this.labelAlpha.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAlpha.ForeColor = System.Drawing.Color.Black;
            this.labelAlpha.Location = new System.Drawing.Point(119, 2);
            this.labelAlpha.Name = "labelAlpha";
            this.labelAlpha.Size = new System.Drawing.Size(57, 23);
            this.labelAlpha.TabIndex = 571;
            this.labelAlpha.Text = "Alpha";
            this.labelAlpha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cboxIsOuter
            // 
            this.cboxIsOuter.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsOuter.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsOuter.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsOuter.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsOuter.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(250)))), ((int)(((byte)(220)))));
            this.cboxIsOuter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsOuter.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsOuter.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsOuter.Image = global::AgOpenGPS.Properties.Resources.TramOuter;
            this.cboxIsOuter.Location = new System.Drawing.Point(205, 178);
            this.cboxIsOuter.Name = "cboxIsOuter";
            this.cboxIsOuter.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsOuter.Size = new System.Drawing.Size(87, 68);
            this.cboxIsOuter.TabIndex = 579;
            this.cboxIsOuter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsOuter.UseVisualStyleBackColor = false;
            this.cboxIsOuter.Click += new System.EventHandler(this.cboxIsOuter_Click);
            // 
            // btnDeleteAllTrams
            // 
            this.btnDeleteAllTrams.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteAllTrams.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteAllTrams.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteAllTrams.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDeleteAllTrams.FlatAppearance.BorderSize = 0;
            this.btnDeleteAllTrams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteAllTrams.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteAllTrams.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteAllTrams.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteAllTrams.Location = new System.Drawing.Point(111, 93);
            this.btnDeleteAllTrams.Name = "btnDeleteAllTrams";
            this.btnDeleteAllTrams.Size = new System.Drawing.Size(73, 52);
            this.btnDeleteAllTrams.TabIndex = 6;
            this.btnDeleteAllTrams.UseVisualStyleBackColor = false;
            this.btnDeleteAllTrams.Click += new System.EventHandler(this.btnDeleteAllTrams_Click);
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
            this.btnCancelTouch.Location = new System.Drawing.Point(13, 183);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(72, 58);
            this.btnCancelTouch.TabIndex = 575;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.btnCancelTouch_Click);
            // 
            // btnSwapAB
            // 
            this.btnSwapAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSwapAB.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapAB.FlatAppearance.BorderSize = 0;
            this.btnSwapAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapAB.ForeColor = System.Drawing.Color.White;
            this.btnSwapAB.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnSwapAB.Location = new System.Drawing.Point(13, 88);
            this.btnSwapAB.Name = "btnSwapAB";
            this.btnSwapAB.Size = new System.Drawing.Size(72, 62);
            this.btnSwapAB.TabIndex = 568;
            this.btnSwapAB.UseVisualStyleBackColor = false;
            this.btnSwapAB.Click += new System.EventHandler(this.btnSwapAB_Click);
            // 
            // lblNumPasses
            // 
            this.lblNumPasses.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNumPasses.AutoSize = true;
            this.lblNumPasses.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblNumPasses.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumPasses.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.lblNumPasses.Location = new System.Drawing.Point(104, 460);
            this.lblNumPasses.Name = "lblNumPasses";
            this.lblNumPasses.Size = new System.Drawing.Size(87, 58);
            this.lblNumPasses.TabIndex = 581;
            this.lblNumPasses.Text = "12";
            this.lblNumPasses.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStartPass
            // 
            this.lblStartPass.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStartPass.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblStartPass.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartPass.ForeColor = System.Drawing.Color.Black;
            this.lblStartPass.Location = new System.Drawing.Point(102, 344);
            this.lblStartPass.Name = "lblStartPass";
            this.lblStartPass.Size = new System.Drawing.Size(92, 92);
            this.lblStartPass.TabIndex = 580;
            this.lblStartPass.Text = "Start\r\n 12";
            this.lblStartPass.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            this.btnSelectCurveBk.Location = new System.Drawing.Point(3, 268);
            this.btnSelectCurveBk.Name = "btnSelectCurveBk";
            this.btnSelectCurveBk.Size = new System.Drawing.Size(93, 65);
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
            this.btnSelectCurve.Location = new System.Drawing.Point(200, 268);
            this.btnSelectCurve.Name = "btnSelectCurve";
            this.btnSelectCurve.Size = new System.Drawing.Size(96, 65);
            this.btnSelectCurve.TabIndex = 5;
            this.btnSelectCurve.UseVisualStyleBackColor = false;
            this.btnSelectCurve.Click += new System.EventHandler(this.btnSelectCurve_Click);
            // 
            // btnAddLines
            // 
            this.btnAddLines.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAddLines.BackColor = System.Drawing.Color.Transparent;
            this.btnAddLines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnAddLines.FlatAppearance.BorderSize = 0;
            this.btnAddLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddLines.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddLines.ForeColor = System.Drawing.Color.White;
            this.btnAddLines.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.btnAddLines.Location = new System.Drawing.Point(113, 552);
            this.btnAddLines.Name = "btnAddLines";
            this.btnAddLines.Size = new System.Drawing.Size(70, 59);
            this.btnAddLines.TabIndex = 574;
            this.btnAddLines.UseVisualStyleBackColor = false;
            this.btnAddLines.Click += new System.EventHandler(this.btnAddLines_Click);
            // 
            // lblAplha
            // 
            this.lblAplha.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblAplha.AutoSize = true;
            this.lblAplha.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblAplha.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAplha.ForeColor = System.Drawing.Color.Black;
            this.lblAplha.Location = new System.Drawing.Point(113, 38);
            this.lblAplha.Name = "lblAplha";
            this.lblAplha.Size = new System.Drawing.Size(69, 23);
            this.lblAplha.TabIndex = 570;
            this.lblAplha.Text = "100%";
            this.lblAplha.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnUpAlpha
            // 
            this.btnUpAlpha.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpAlpha.BackColor = System.Drawing.Color.Transparent;
            this.btnUpAlpha.FlatAppearance.BorderSize = 0;
            this.btnUpAlpha.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpAlpha.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpAlpha.ForeColor = System.Drawing.Color.White;
            this.btnUpAlpha.Image = global::AgOpenGPS.Properties.Resources.IncrementPlus;
            this.btnUpAlpha.Location = new System.Drawing.Point(212, 5);
            this.btnUpAlpha.Name = "btnUpAlpha";
            this.tlp1.SetRowSpan(this.btnUpAlpha, 2);
            this.btnUpAlpha.Size = new System.Drawing.Size(72, 62);
            this.btnUpAlpha.TabIndex = 583;
            this.btnUpAlpha.UseVisualStyleBackColor = false;
            this.btnUpAlpha.Click += new System.EventHandler(this.btnUpAlpha_Click);
            // 
            // btnDnTrams
            // 
            this.btnDnTrams.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDnTrams.BackColor = System.Drawing.Color.Transparent;
            this.btnDnTrams.FlatAppearance.BorderSize = 0;
            this.btnDnTrams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDnTrams.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDnTrams.ForeColor = System.Drawing.Color.White;
            this.btnDnTrams.Image = global::AgOpenGPS.Properties.Resources.IncrementMinus;
            this.btnDnTrams.Location = new System.Drawing.Point(13, 458);
            this.btnDnTrams.Name = "btnDnTrams";
            this.btnDnTrams.Size = new System.Drawing.Size(72, 62);
            this.btnDnTrams.TabIndex = 565;
            this.btnDnTrams.UseVisualStyleBackColor = false;
            this.btnDnTrams.Click += new System.EventHandler(this.btnDnTrams_Click);
            // 
            // btnDnStartTram
            // 
            this.btnDnStartTram.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDnStartTram.BackColor = System.Drawing.Color.Transparent;
            this.btnDnStartTram.FlatAppearance.BorderSize = 0;
            this.btnDnStartTram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDnStartTram.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDnStartTram.ForeColor = System.Drawing.Color.White;
            this.btnDnStartTram.Image = global::AgOpenGPS.Properties.Resources.IncrementMinus;
            this.btnDnStartTram.Location = new System.Drawing.Point(13, 359);
            this.btnDnStartTram.Name = "btnDnStartTram";
            this.btnDnStartTram.Size = new System.Drawing.Size(72, 62);
            this.btnDnStartTram.TabIndex = 576;
            this.btnDnStartTram.UseVisualStyleBackColor = false;
            this.btnDnStartTram.Click += new System.EventHandler(this.btnDnStartTram_Click);
            // 
            // btnUpStartTram
            // 
            this.btnUpStartTram.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpStartTram.BackColor = System.Drawing.Color.Transparent;
            this.btnUpStartTram.FlatAppearance.BorderSize = 0;
            this.btnUpStartTram.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpStartTram.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpStartTram.ForeColor = System.Drawing.Color.White;
            this.btnUpStartTram.Image = global::AgOpenGPS.Properties.Resources.IncrementPlus;
            this.btnUpStartTram.Location = new System.Drawing.Point(212, 359);
            this.btnUpStartTram.Name = "btnUpStartTram";
            this.btnUpStartTram.Size = new System.Drawing.Size(72, 62);
            this.btnUpStartTram.TabIndex = 577;
            this.btnUpStartTram.UseVisualStyleBackColor = false;
            this.btnUpStartTram.Click += new System.EventHandler(this.btnUpStartTram_Click);
            // 
            // btnUpTrams
            // 
            this.btnUpTrams.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUpTrams.BackColor = System.Drawing.Color.Transparent;
            this.btnUpTrams.FlatAppearance.BorderSize = 0;
            this.btnUpTrams.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpTrams.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpTrams.ForeColor = System.Drawing.Color.White;
            this.btnUpTrams.Image = global::AgOpenGPS.Properties.Resources.IncrementPlus;
            this.btnUpTrams.Location = new System.Drawing.Point(212, 458);
            this.btnUpTrams.Name = "btnUpTrams";
            this.btnUpTrams.Size = new System.Drawing.Size(72, 62);
            this.btnUpTrams.TabIndex = 566;
            this.btnUpTrams.UseVisualStyleBackColor = false;
            this.btnUpTrams.Click += new System.EventHandler(this.btnUpTrams_Click);
            // 
            // btnResize
            // 
            this.btnResize.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnResize.BackColor = System.Drawing.Color.Transparent;
            this.btnResize.FlatAppearance.BorderSize = 0;
            this.btnResize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResize.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResize.ForeColor = System.Drawing.Color.White;
            this.btnResize.Image = global::AgOpenGPS.Properties.Resources.ConD_FullScreenBegin;
            this.btnResize.Location = new System.Drawing.Point(212, 88);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(72, 62);
            this.btnResize.TabIndex = 582;
            this.btnResize.UseVisualStyleBackColor = false;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // FormTramLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1004, 701);
            this.ControlBox = false;
            this.Controls.Add(this.tlp1);
            this.Controls.Add(this.oglSelf);
            this.ForeColor = System.Drawing.Color.Black;
            this.HelpButton = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormTramLine";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Draw AB - Click 2 points on the Boundary to Begin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormTramLine_FormClosing);
            this.Load += new System.EventHandler(this.FormTramLine_Load);
            this.ResizeEnd += new System.EventHandler(this.FormTramLine_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            this.tlp1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSelectCurve;
        private System.Windows.Forms.Button btnDeleteAllTrams;
        private System.Windows.Forms.Label lblCurveSelected;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnSelectCurveBk;
        private System.Windows.Forms.TableLayoutPanel tlp1;
        private System.Windows.Forms.Button btnDnTrams;
        private System.Windows.Forms.Button btnUpTrams;
        private System.Windows.Forms.Button btnSwapAB;
        private System.Windows.Forms.Label lblAplha;
        private System.Windows.Forms.Label labelAlpha;
        private System.Windows.Forms.Button btnAddLines;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.Button btnUpStartTram;
        private System.Windows.Forms.Button btnDnStartTram;
        private System.Windows.Forms.CheckBox cboxIsOuter;
        private System.Windows.Forms.Label lblStartPass;
        private System.Windows.Forms.Label lblNumPasses;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Button btnDnAlpha;
        private System.Windows.Forms.Button btnUpAlpha;
    }
}
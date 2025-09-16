namespace AgOpenGPS
{
    partial class FormHeadLine
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
            this.headingGroupBox = new System.Windows.Forms.GroupBox();
            this.lblToolWidth = new System.Windows.Forms.Label();
            this.cboxToolWidths = new System.Windows.Forms.ComboBox();
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClipLine = new System.Windows.Forms.Button();
            this.nudSetDistance = new AgOpenGPS.NudlessNumericUpDown();
            this.checkBoxZoomIn = new System.Windows.Forms.CheckBox();
            this.btnDeletePoints = new System.Windows.Forms.Button();
            this.btnBndLoop = new System.Windows.Forms.Button();
            this.btnAShrink = new System.Windows.Forms.Button();
            this.btnBShrink = new System.Windows.Forms.Button();
            this.btnBLength = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnALength = new System.Windows.Forms.Button();
            this.btnHeadlandOff = new System.Windows.Forms.Button();
            this.cboxIsSectionControlled = new System.Windows.Forms.CheckBox();
            this.rbtnLine = new System.Windows.Forms.RadioButton();
            this.rbtnCurve = new System.Windows.Forms.RadioButton();
            this.btnUndo = new System.Windows.Forms.Button();
            this.tlp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSetDistance)).BeginInit();
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
            this.oglSelf.Location = new System.Drawing.Point(1, 2);
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
            // headingGroupBox
            // 
            this.headingGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.headingGroupBox.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headingGroupBox.ForeColor = System.Drawing.Color.Black;
            this.headingGroupBox.Location = new System.Drawing.Point(899, 2);
            this.headingGroupBox.Name = "headingGroupBox";
            this.headingGroupBox.Size = new System.Drawing.Size(102, 229);
            this.headingGroupBox.TabIndex = 438;
            this.headingGroupBox.TabStop = false;
            // 
            // lblToolWidth
            // 
            this.lblToolWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp1.SetColumnSpan(this.lblToolWidth, 2);
            this.lblToolWidth.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolWidth.ForeColor = System.Drawing.Color.Black;
            this.lblToolWidth.Location = new System.Drawing.Point(3, 309);
            this.lblToolWidth.Name = "lblToolWidth";
            this.lblToolWidth.Size = new System.Drawing.Size(292, 26);
            this.lblToolWidth.TabIndex = 517;
            this.lblToolWidth.Text = "43.56 m";
            this.lblToolWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxToolWidths
            // 
            this.cboxToolWidths.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cboxToolWidths.BackColor = System.Drawing.Color.Lavender;
            this.cboxToolWidths.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxToolWidths.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxToolWidths.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxToolWidths.FormattingEnabled = true;
            this.cboxToolWidths.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10"});
            this.cboxToolWidths.Location = new System.Drawing.Point(182, 252);
            this.cboxToolWidths.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxToolWidths.Name = "cboxToolWidths";
            this.cboxToolWidths.Size = new System.Drawing.Size(88, 53);
            this.cboxToolWidths.TabIndex = 516;
            this.cboxToolWidths.SelectedIndexChanged += new System.EventHandler(this.cboxToolWidths_SelectedIndexChanged);
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tlp1.ColumnCount = 2;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.30263F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.69737F));
            this.tlp1.Controls.Add(this.btnClipLine, 0, 6);
            this.tlp1.Controls.Add(this.nudSetDistance, 0, 3);
            this.tlp1.Controls.Add(this.checkBoxZoomIn, 0, 7);
            this.tlp1.Controls.Add(this.btnDeletePoints, 1, 5);
            this.tlp1.Controls.Add(this.btnBndLoop, 0, 5);
            this.tlp1.Controls.Add(this.btnAShrink, 1, 1);
            this.tlp1.Controls.Add(this.btnBShrink, 1, 0);
            this.tlp1.Controls.Add(this.btnBLength, 0, 0);
            this.tlp1.Controls.Add(this.btnExit, 1, 8);
            this.tlp1.Controls.Add(this.btnALength, 0, 1);
            this.tlp1.Controls.Add(this.btnHeadlandOff, 0, 8);
            this.tlp1.Controls.Add(this.cboxIsSectionControlled, 1, 7);
            this.tlp1.Controls.Add(this.cboxToolWidths, 1, 3);
            this.tlp1.Controls.Add(this.rbtnLine, 1, 2);
            this.tlp1.Controls.Add(this.rbtnCurve, 0, 2);
            this.tlp1.Controls.Add(this.lblToolWidth, 0, 4);
            this.tlp1.Controls.Add(this.btnUndo, 1, 6);
            this.tlp1.Location = new System.Drawing.Point(703, 1);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 9;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.249717F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.249715F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.49281F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.84249F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.90964F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.90964F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.10241F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.13218F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.Size = new System.Drawing.Size(298, 701);
            this.tlp1.TabIndex = 565;
            // 
            // btnClipLine
            // 
            this.btnClipLine.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnClipLine.BackColor = System.Drawing.Color.Transparent;
            this.btnClipLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnClipLine.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnClipLine.FlatAppearance.BorderSize = 0;
            this.btnClipLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClipLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnClipLine.Image = global::AgOpenGPS.Properties.Resources.HeadlandSlice;
            this.btnClipLine.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClipLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnClipLine.Location = new System.Drawing.Point(3, 439);
            this.btnClipLine.Name = "btnClipLine";
            this.btnClipLine.Size = new System.Drawing.Size(149, 93);
            this.btnClipLine.TabIndex = 519;
            this.btnClipLine.Text = "Clip Line";
            this.btnClipLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClipLine.UseVisualStyleBackColor = false;
            this.btnClipLine.Click += new System.EventHandler(this.btnSlice_Click);
            // 
            // nudSetDistance
            // 
            this.nudSetDistance.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nudSetDistance.BackColor = System.Drawing.Color.White;
            this.nudSetDistance.DecimalPlaces = 1;
            this.nudSetDistance.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSetDistance.Location = new System.Drawing.Point(6, 253);
            this.nudSetDistance.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSetDistance.Name = "nudSetDistance";
            this.nudSetDistance.ReadOnly = true;
            this.nudSetDistance.Size = new System.Drawing.Size(143, 52);
            this.nudSetDistance.TabIndex = 464;
            this.nudSetDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSetDistance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudSetDistance.Click += new System.EventHandler(this.nudSetDistance_Click);
            // 
            // checkBoxZoomIn
            // 
            this.checkBoxZoomIn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.checkBoxZoomIn.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxZoomIn.BackColor = System.Drawing.Color.WhiteSmoke;
            this.checkBoxZoomIn.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.checkBoxZoomIn.FlatAppearance.BorderSize = 0;
            this.checkBoxZoomIn.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(160)))));
            this.checkBoxZoomIn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.checkBoxZoomIn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxZoomIn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.checkBoxZoomIn.Image = global::AgOpenGPS.Properties.Resources.ZoomOGL;
            this.checkBoxZoomIn.Location = new System.Drawing.Point(25, 539);
            this.checkBoxZoomIn.Name = "checkBoxZoomIn";
            this.checkBoxZoomIn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBoxZoomIn.Size = new System.Drawing.Size(104, 81);
            this.checkBoxZoomIn.TabIndex = 564;
            this.checkBoxZoomIn.Text = "Zoom In";
            this.checkBoxZoomIn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxZoomIn.UseVisualStyleBackColor = false;
            this.checkBoxZoomIn.CheckedChanged += new System.EventHandler(this.cboxIsZoom_CheckedChanged);
            // 
            // btnDeletePoints
            // 
            this.btnDeletePoints.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeletePoints.BackColor = System.Drawing.Color.Transparent;
            this.btnDeletePoints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDeletePoints.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.btnDeletePoints.FlatAppearance.BorderSize = 0;
            this.btnDeletePoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeletePoints.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeletePoints.Image = global::AgOpenGPS.Properties.Resources.HeadlandReset;
            this.btnDeletePoints.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDeletePoints.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeletePoints.Location = new System.Drawing.Point(174, 339);
            this.btnDeletePoints.Name = "btnDeletePoints";
            this.btnDeletePoints.Size = new System.Drawing.Size(104, 93);
            this.btnDeletePoints.TabIndex = 506;
            this.btnDeletePoints.Text = "Reset";
            this.btnDeletePoints.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeletePoints.UseVisualStyleBackColor = false;
            this.btnDeletePoints.Click += new System.EventHandler(this.btnDeletePoints_Click);
            // 
            // btnBndLoop
            // 
            this.btnBndLoop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBndLoop.BackColor = System.Drawing.Color.Transparent;
            this.btnBndLoop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnBndLoop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnBndLoop.FlatAppearance.BorderSize = 0;
            this.btnBndLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBndLoop.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBndLoop.Image = global::AgOpenGPS.Properties.Resources.HeadlandBuild;
            this.btnBndLoop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBndLoop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBndLoop.Location = new System.Drawing.Point(6, 339);
            this.btnBndLoop.Name = "btnBndLoop";
            this.btnBndLoop.Size = new System.Drawing.Size(142, 93);
            this.btnBndLoop.TabIndex = 504;
            this.btnBndLoop.Text = "Build";
            this.btnBndLoop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBndLoop.UseVisualStyleBackColor = false;
            this.btnBndLoop.Click += new System.EventHandler(this.btnBndLoop_Click);
            // 
            // btnAShrink
            // 
            this.btnAShrink.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAShrink.BackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnAShrink.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.btnAShrink.FlatAppearance.BorderSize = 0;
            this.btnAShrink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAShrink.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAShrink.Image = global::AgOpenGPS.Properties.Resources.APlusMinusA;
            this.btnAShrink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAShrink.Location = new System.Drawing.Point(179, 71);
            this.btnAShrink.Name = "btnAShrink";
            this.btnAShrink.Size = new System.Drawing.Size(94, 43);
            this.btnAShrink.TabIndex = 529;
            this.btnAShrink.UseVisualStyleBackColor = false;
            this.btnAShrink.Click += new System.EventHandler(this.btnAShrink_Click);
            // 
            // btnBShrink
            // 
            this.btnBShrink.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBShrink.BackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnBShrink.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnBShrink.FlatAppearance.BorderSize = 0;
            this.btnBShrink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBShrink.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBShrink.Image = global::AgOpenGPS.Properties.Resources.APlusMinusB;
            this.btnBShrink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBShrink.Location = new System.Drawing.Point(179, 8);
            this.btnBShrink.Name = "btnBShrink";
            this.btnBShrink.Size = new System.Drawing.Size(94, 45);
            this.btnBShrink.TabIndex = 528;
            this.btnBShrink.UseVisualStyleBackColor = false;
            this.btnBShrink.Click += new System.EventHandler(this.btnBShrink_Click);
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
            this.btnBLength.Location = new System.Drawing.Point(30, 8);
            this.btnBLength.Name = "btnBLength";
            this.btnBLength.Size = new System.Drawing.Size(94, 45);
            this.btnBLength.TabIndex = 351;
            this.btnBLength.UseVisualStyleBackColor = false;
            this.btnBLength.Click += new System.EventHandler(this.btnBLength_Click);
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
            this.btnExit.Location = new System.Drawing.Point(163, 630);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(126, 64);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
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
            this.btnALength.Location = new System.Drawing.Point(30, 71);
            this.btnALength.Name = "btnALength";
            this.btnALength.Size = new System.Drawing.Size(94, 43);
            this.btnALength.TabIndex = 352;
            this.btnALength.UseVisualStyleBackColor = false;
            this.btnALength.Click += new System.EventHandler(this.btnALength_Click);
            // 
            // btnHeadlandOff
            // 
            this.btnHeadlandOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHeadlandOff.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnHeadlandOff.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHeadlandOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOff.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnHeadlandOff.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnHeadlandOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOff.Location = new System.Drawing.Point(26, 634);
            this.btnHeadlandOff.Name = "btnHeadlandOff";
            this.btnHeadlandOff.Size = new System.Drawing.Size(103, 57);
            this.btnHeadlandOff.TabIndex = 518;
            this.btnHeadlandOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOff.Click += new System.EventHandler(this.btnHeadlandOff_Click);
            // 
            // cboxIsSectionControlled
            // 
            this.cboxIsSectionControlled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsSectionControlled.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsSectionControlled.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.Checked = true;
            this.cboxIsSectionControlled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsSectionControlled.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.cboxIsSectionControlled.FlatAppearance.BorderSize = 0;
            this.cboxIsSectionControlled.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsSectionControlled.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsSectionControlled.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsSectionControlled.Image = global::AgOpenGPS.Properties.Resources.HeadlandSectionOn;
            this.cboxIsSectionControlled.Location = new System.Drawing.Point(177, 544);
            this.cboxIsSectionControlled.Name = "cboxIsSectionControlled";
            this.cboxIsSectionControlled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsSectionControlled.Size = new System.Drawing.Size(99, 71);
            this.cboxIsSectionControlled.TabIndex = 467;
            this.cboxIsSectionControlled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsSectionControlled.UseVisualStyleBackColor = false;
            this.cboxIsSectionControlled.Click += new System.EventHandler(this.cboxIsSectionControlled_Click);
            // 
            // rbtnLine
            // 
            this.rbtnLine.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtnLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnLine.BackColor = System.Drawing.Color.AliceBlue;
            this.rbtnLine.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.rbtnLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnLine.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnLine.ForeColor = System.Drawing.Color.Black;
            this.rbtnLine.Image = global::AgOpenGPS.Properties.Resources.ABTrackAB;
            this.rbtnLine.Location = new System.Drawing.Point(174, 137);
            this.rbtnLine.Name = "rbtnLine";
            this.rbtnLine.Size = new System.Drawing.Size(104, 85);
            this.rbtnLine.TabIndex = 2;
            this.rbtnLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnLine.UseVisualStyleBackColor = false;
            // 
            // rbtnCurve
            // 
            this.rbtnCurve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtnCurve.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnCurve.BackColor = System.Drawing.Color.AliceBlue;
            this.rbtnCurve.Checked = true;
            this.rbtnCurve.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.rbtnCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnCurve.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnCurve.ForeColor = System.Drawing.Color.Black;
            this.rbtnCurve.Image = global::AgOpenGPS.Properties.Resources.ABTrackCurve;
            this.rbtnCurve.Location = new System.Drawing.Point(25, 137);
            this.rbtnCurve.Name = "rbtnCurve";
            this.rbtnCurve.Size = new System.Drawing.Size(104, 85);
            this.rbtnCurve.TabIndex = 0;
            this.rbtnCurve.TabStop = true;
            this.rbtnCurve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnCurve.UseVisualStyleBackColor = false;
            // 
            // btnUndo
            // 
            this.btnUndo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnUndo.BackColor = System.Drawing.Color.Transparent;
            this.btnUndo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnUndo.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnUndo.FlatAppearance.BorderSize = 0;
            this.btnUndo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUndo.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnUndo.Image = global::AgOpenGPS.Properties.Resources.back_button;
            this.btnUndo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnUndo.Location = new System.Drawing.Point(177, 457);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Size = new System.Drawing.Size(99, 57);
            this.btnUndo.TabIndex = 514;
            this.btnUndo.UseVisualStyleBackColor = false;
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // FormHeadLine
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1006, 726);
            this.ControlBox = false;
            this.Controls.Add(this.tlp1);
            this.Controls.Add(this.headingGroupBox);
            this.Controls.Add(this.oglSelf);
            this.ForeColor = System.Drawing.Color.Black;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1022, 742);
            this.Name = "FormHeadLine";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Click 2 points on the Boundary to Begin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHeadLine_FormClosing);
            this.Load += new System.EventHandler(this.FormHeadLine_Load);
            this.ResizeEnd += new System.EventHandler(this.FormHeadLine_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSetDistance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnBLength;
        private System.Windows.Forms.Button btnALength;
        private System.Windows.Forms.GroupBox headingGroupBox;
        private System.Windows.Forms.RadioButton rbtnLine;
        private System.Windows.Forms.RadioButton rbtnCurve;
        private NudlessNumericUpDown nudSetDistance;
        private System.Windows.Forms.CheckBox cboxIsSectionControlled;
        private System.Windows.Forms.Button btnBndLoop;
        private System.Windows.Forms.Button btnDeletePoints;
        private System.Windows.Forms.Button btnUndo;
        private System.Windows.Forms.Label lblToolWidth;
        private System.Windows.Forms.ComboBox cboxToolWidths;
        private System.Windows.Forms.Button btnHeadlandOff;
        private System.Windows.Forms.Button btnClipLine;
        private System.Windows.Forms.Button btnAShrink;
        private System.Windows.Forms.Button btnBShrink;
        private System.Windows.Forms.CheckBox checkBoxZoomIn;
        private System.Windows.Forms.TableLayoutPanel tlp1;
    }
}
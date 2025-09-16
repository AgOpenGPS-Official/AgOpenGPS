namespace AgOpenGPS
{
    partial class FormHeadAche
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
            this.rbtnLine = new System.Windows.Forms.RadioButton();
            this.rbtnCurve = new System.Windows.Forms.RadioButton();
            this.cboxToolWidths = new System.Windows.Forms.ComboBox();
            this.lblToolWidth = new System.Windows.Forms.Label();
            this.btnAShrink = new System.Windows.Forms.Button();
            this.btnBShrink = new System.Windows.Forms.Button();
            this.btnHeadlandOff = new System.Windows.Forms.Button();
            this.btnCycleBackward = new System.Windows.Forms.Button();
            this.btnBndLoop = new System.Windows.Forms.Button();
            this.btnDeleteHeadland = new System.Windows.Forms.Button();
            this.cboxIsSectionControlled = new System.Windows.Forms.CheckBox();
            this.btnCycleForward = new System.Windows.Forms.Button();
            this.btnALength = new System.Windows.Forms.Button();
            this.btnBLength = new System.Windows.Forms.Button();
            this.btnDeleteCurve = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.cboxIsZoom = new System.Windows.Forms.CheckBox();
            this.tlp1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.nudSetDistance = new AgOpenGPS.NudlessNumericUpDown();
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
            this.oglSelf.Location = new System.Drawing.Point(0, 1);
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
            this.headingGroupBox.Location = new System.Drawing.Point(905, 4);
            this.headingGroupBox.Name = "headingGroupBox";
            this.headingGroupBox.Size = new System.Drawing.Size(97, 217);
            this.headingGroupBox.TabIndex = 438;
            this.headingGroupBox.TabStop = false;
            // 
            // rbtnLine
            // 
            this.rbtnLine.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtnLine.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnLine.BackColor = System.Drawing.Color.AliceBlue;
            this.tlp1.SetColumnSpan(this.rbtnLine, 3);
            this.rbtnLine.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.rbtnLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnLine.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnLine.ForeColor = System.Drawing.Color.Black;
            this.rbtnLine.Image = global::AgOpenGPS.Properties.Resources.ABTrackAB;
            this.rbtnLine.Location = new System.Drawing.Point(163, 192);
            this.rbtnLine.Name = "rbtnLine";
            this.rbtnLine.Size = new System.Drawing.Size(120, 72);
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
            this.tlp1.SetColumnSpan(this.rbtnCurve, 3);
            this.rbtnCurve.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleTurquoise;
            this.rbtnCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnCurve.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnCurve.ForeColor = System.Drawing.Color.Black;
            this.rbtnCurve.Image = global::AgOpenGPS.Properties.Resources.ABTrackCurve;
            this.rbtnCurve.Location = new System.Drawing.Point(13, 192);
            this.rbtnCurve.Name = "rbtnCurve";
            this.rbtnCurve.Size = new System.Drawing.Size(120, 72);
            this.rbtnCurve.TabIndex = 0;
            this.rbtnCurve.TabStop = true;
            this.rbtnCurve.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnCurve.UseVisualStyleBackColor = false;
            // 
            // cboxToolWidths
            // 
            this.cboxToolWidths.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cboxToolWidths.BackColor = System.Drawing.Color.Lavender;
            this.tlp1.SetColumnSpan(this.cboxToolWidths, 2);
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
            this.cboxToolWidths.Location = new System.Drawing.Point(202, 297);
            this.cboxToolWidths.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxToolWidths.Name = "cboxToolWidths";
            this.cboxToolWidths.Size = new System.Drawing.Size(90, 53);
            this.cboxToolWidths.TabIndex = 510;
            this.cboxToolWidths.SelectedIndexChanged += new System.EventHandler(this.cboxToolWidths_SelectedIndexChanged);
            // 
            // lblToolWidth
            // 
            this.lblToolWidth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tlp1.SetColumnSpan(this.lblToolWidth, 6);
            this.lblToolWidth.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToolWidth.ForeColor = System.Drawing.Color.Black;
            this.lblToolWidth.Location = new System.Drawing.Point(3, 358);
            this.lblToolWidth.Name = "lblToolWidth";
            this.lblToolWidth.Size = new System.Drawing.Size(293, 26);
            this.lblToolWidth.TabIndex = 511;
            this.lblToolWidth.Text = "3.86";
            this.lblToolWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAShrink
            // 
            this.btnAShrink.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAShrink.BackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnAShrink, 2);
            this.btnAShrink.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.btnAShrink.FlatAppearance.BorderSize = 0;
            this.btnAShrink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAShrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAShrink.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAShrink.Image = global::AgOpenGPS.Properties.Resources.APlusMinusA;
            this.btnAShrink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAShrink.Location = new System.Drawing.Point(101, 111);
            this.btnAShrink.Name = "btnAShrink";
            this.btnAShrink.Size = new System.Drawing.Size(92, 52);
            this.btnAShrink.TabIndex = 525;
            this.btnAShrink.UseVisualStyleBackColor = false;
            this.btnAShrink.Click += new System.EventHandler(this.btnAShrink_Click);
            // 
            // btnBShrink
            // 
            this.btnBShrink.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBShrink.BackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnBShrink, 2);
            this.btnBShrink.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnBShrink.FlatAppearance.BorderSize = 0;
            this.btnBShrink.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBShrink.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBShrink.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBShrink.Image = global::AgOpenGPS.Properties.Resources.APlusMinusB;
            this.btnBShrink.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBShrink.Location = new System.Drawing.Point(101, 21);
            this.btnBShrink.Name = "btnBShrink";
            this.btnBShrink.Size = new System.Drawing.Size(92, 52);
            this.btnBShrink.TabIndex = 524;
            this.btnBShrink.UseVisualStyleBackColor = false;
            this.btnBShrink.Click += new System.EventHandler(this.btnBShrink_Click);
            // 
            // btnHeadlandOff
            // 
            this.btnHeadlandOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHeadlandOff.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnHeadlandOff, 2);
            this.btnHeadlandOff.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnHeadlandOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOff.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnHeadlandOff.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnHeadlandOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOff.Location = new System.Drawing.Point(17, 621);
            this.btnHeadlandOff.Name = "btnHeadlandOff";
            this.btnHeadlandOff.Size = new System.Drawing.Size(63, 70);
            this.btnHeadlandOff.TabIndex = 519;
            this.btnHeadlandOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOff.Click += new System.EventHandler(this.btnHeadlandOff_Click);
            // 
            // btnCycleBackward
            // 
            this.btnCycleBackward.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleBackward.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleBackward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnCycleBackward, 2);
            this.btnCycleBackward.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCycleBackward.FlatAppearance.BorderSize = 0;
            this.btnCycleBackward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleBackward.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCycleBackward.Image = global::AgOpenGPS.Properties.Resources.ABLineCycleBk;
            this.btnCycleBackward.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleBackward.Location = new System.Drawing.Point(115, 522);
            this.btnCycleBackward.Name = "btnCycleBackward";
            this.btnCycleBackward.Size = new System.Drawing.Size(63, 68);
            this.btnCycleBackward.TabIndex = 507;
            this.btnCycleBackward.UseVisualStyleBackColor = false;
            this.btnCycleBackward.Click += new System.EventHandler(this.btnCycleBackward_Click);
            // 
            // btnBndLoop
            // 
            this.btnBndLoop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBndLoop.BackColor = System.Drawing.Color.Transparent;
            this.btnBndLoop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnBndLoop, 3);
            this.btnBndLoop.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnBndLoop.FlatAppearance.BorderSize = 0;
            this.btnBndLoop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBndLoop.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBndLoop.Image = global::AgOpenGPS.Properties.Resources.HeadlandBuild;
            this.btnBndLoop.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBndLoop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBndLoop.Location = new System.Drawing.Point(31, 392);
            this.btnBndLoop.Name = "btnBndLoop";
            this.btnBndLoop.Size = new System.Drawing.Size(85, 103);
            this.btnBndLoop.TabIndex = 504;
            this.btnBndLoop.Text = "Build";
            this.btnBndLoop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBndLoop.UseVisualStyleBackColor = false;
            this.btnBndLoop.Click += new System.EventHandler(this.btnBndLoop_Click);
            // 
            // btnDeleteHeadland
            // 
            this.btnDeleteHeadland.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteHeadland.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteHeadland.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tlp1.SetColumnSpan(this.btnDeleteHeadland, 3);
            this.btnDeleteHeadland.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnDeleteHeadland.FlatAppearance.BorderSize = 0;
            this.btnDeleteHeadland.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteHeadland.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteHeadland.Image = global::AgOpenGPS.Properties.Resources.HeadlandReset;
            this.btnDeleteHeadland.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDeleteHeadland.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteHeadland.Location = new System.Drawing.Point(177, 392);
            this.btnDeleteHeadland.Name = "btnDeleteHeadland";
            this.btnDeleteHeadland.Size = new System.Drawing.Size(92, 103);
            this.btnDeleteHeadland.TabIndex = 465;
            this.btnDeleteHeadland.Text = "Reset";
            this.btnDeleteHeadland.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDeleteHeadland.UseVisualStyleBackColor = false;
            this.btnDeleteHeadland.Click += new System.EventHandler(this.btnDeleteHeadland_Click);
            // 
            // cboxIsSectionControlled
            // 
            this.cboxIsSectionControlled.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsSectionControlled.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsSectionControlled.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.Checked = true;
            this.cboxIsSectionControlled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tlp1.SetColumnSpan(this.cboxIsSectionControlled, 2);
            this.cboxIsSectionControlled.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.cboxIsSectionControlled.FlatAppearance.BorderSize = 0;
            this.cboxIsSectionControlled.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsSectionControlled.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsSectionControlled.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsSectionControlled.Image = global::AgOpenGPS.Properties.Resources.HeadlandSectionOff;
            this.cboxIsSectionControlled.Location = new System.Drawing.Point(216, 15);
            this.cboxIsSectionControlled.Name = "cboxIsSectionControlled";
            this.cboxIsSectionControlled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsSectionControlled.Size = new System.Drawing.Size(63, 65);
            this.cboxIsSectionControlled.TabIndex = 467;
            this.cboxIsSectionControlled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsSectionControlled.UseVisualStyleBackColor = false;
            this.cboxIsSectionControlled.Click += new System.EventHandler(this.cboxIsSectionControlled_Click);
            // 
            // btnCycleForward
            // 
            this.btnCycleForward.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleForward.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleForward.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnCycleForward, 2);
            this.btnCycleForward.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCycleForward.FlatAppearance.BorderSize = 0;
            this.btnCycleForward.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleForward.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCycleForward.Image = global::AgOpenGPS.Properties.Resources.ABLineCycle;
            this.btnCycleForward.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleForward.Location = new System.Drawing.Point(216, 522);
            this.btnCycleForward.Name = "btnCycleForward";
            this.btnCycleForward.Size = new System.Drawing.Size(63, 68);
            this.btnCycleForward.TabIndex = 5;
            this.btnCycleForward.UseVisualStyleBackColor = false;
            this.btnCycleForward.Click += new System.EventHandler(this.btnCycleForward_Click);
            // 
            // btnALength
            // 
            this.btnALength.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnALength.BackColor = System.Drawing.Color.Transparent;
            this.btnALength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnALength, 2);
            this.btnALength.FlatAppearance.BorderColor = System.Drawing.Color.DarkOrange;
            this.btnALength.FlatAppearance.BorderSize = 0;
            this.btnALength.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnALength.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnALength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnALength.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnALength.Image = global::AgOpenGPS.Properties.Resources.APlusPlusA;
            this.btnALength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnALength.Location = new System.Drawing.Point(3, 111);
            this.btnALength.Name = "btnALength";
            this.btnALength.Size = new System.Drawing.Size(92, 52);
            this.btnALength.TabIndex = 352;
            this.btnALength.UseVisualStyleBackColor = false;
            this.btnALength.Click += new System.EventHandler(this.btnALength_Click);
            // 
            // btnBLength
            // 
            this.btnBLength.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBLength.BackColor = System.Drawing.Color.Transparent;
            this.btnBLength.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnBLength, 2);
            this.btnBLength.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnBLength.FlatAppearance.BorderSize = 0;
            this.btnBLength.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBLength.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBLength.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBLength.Image = global::AgOpenGPS.Properties.Resources.APlusPlusB;
            this.btnBLength.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBLength.Location = new System.Drawing.Point(3, 21);
            this.btnBLength.Name = "btnBLength";
            this.btnBLength.Size = new System.Drawing.Size(92, 52);
            this.btnBLength.TabIndex = 351;
            this.btnBLength.UseVisualStyleBackColor = false;
            this.btnBLength.Click += new System.EventHandler(this.btnBLength_Click);
            // 
            // btnDeleteCurve
            // 
            this.btnDeleteCurve.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDeleteCurve.BackColor = System.Drawing.Color.Transparent;
            this.btnDeleteCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnDeleteCurve, 2);
            this.btnDeleteCurve.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnDeleteCurve.FlatAppearance.BorderSize = 0;
            this.btnDeleteCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteCurve.Image = global::AgOpenGPS.Properties.Resources.Trash;
            this.btnDeleteCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteCurve.Location = new System.Drawing.Point(118, 630);
            this.btnDeleteCurve.Name = "btnDeleteCurve";
            this.btnDeleteCurve.Size = new System.Drawing.Size(57, 51);
            this.btnDeleteCurve.TabIndex = 6;
            this.btnDeleteCurve.UseVisualStyleBackColor = false;
            this.btnDeleteCurve.Click += new System.EventHandler(this.btnDeleteCurve_Click);
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
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnExit.Location = new System.Drawing.Point(216, 621);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(63, 70);
            this.btnExit.TabIndex = 0;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cboxIsZoom
            // 
            this.cboxIsZoom.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxIsZoom.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsZoom.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tlp1.SetColumnSpan(this.cboxIsZoom, 2);
            this.cboxIsZoom.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxIsZoom.FlatAppearance.BorderSize = 0;
            this.cboxIsZoom.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(255)))), ((int)(((byte)(160)))));
            this.cboxIsZoom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsZoom.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsZoom.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxIsZoom.Image = global::AgOpenGPS.Properties.Resources.ZoomOGL;
            this.cboxIsZoom.Location = new System.Drawing.Point(3, 504);
            this.cboxIsZoom.Name = "cboxIsZoom";
            this.cboxIsZoom.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsZoom.Size = new System.Drawing.Size(92, 104);
            this.cboxIsZoom.TabIndex = 564;
            this.cboxIsZoom.Text = "Zoom";
            this.cboxIsZoom.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cboxIsZoom.UseVisualStyleBackColor = false;
            this.cboxIsZoom.CheckedChanged += new System.EventHandler(this.cboxIsZoom_CheckedChanged);
            // 
            // tlp1
            // 
            this.tlp1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.tlp1.ColumnCount = 6;
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlp1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66666F));
            this.tlp1.Controls.Add(this.cboxIsZoom, 0, 6);
            this.tlp1.Controls.Add(this.btnCancelTouch, 4, 1);
            this.tlp1.Controls.Add(this.btnALength, 0, 1);
            this.tlp1.Controls.Add(this.btnBLength, 0, 0);
            this.tlp1.Controls.Add(this.rbtnCurve, 0, 2);
            this.tlp1.Controls.Add(this.rbtnLine, 3, 2);
            this.tlp1.Controls.Add(this.btnBShrink, 2, 0);
            this.tlp1.Controls.Add(this.btnAShrink, 2, 1);
            this.tlp1.Controls.Add(this.cboxToolWidths, 4, 3);
            this.tlp1.Controls.Add(this.btnExit, 4, 7);
            this.tlp1.Controls.Add(this.cboxIsSectionControlled, 4, 0);
            this.tlp1.Controls.Add(this.btnHeadlandOff, 0, 7);
            this.tlp1.Controls.Add(this.nudSetDistance, 0, 3);
            this.tlp1.Controls.Add(this.btnDeleteCurve, 2, 7);
            this.tlp1.Controls.Add(this.lblToolWidth, 0, 4);
            this.tlp1.Controls.Add(this.btnCycleBackward, 2, 6);
            this.tlp1.Controls.Add(this.btnCycleForward, 4, 6);
            this.tlp1.Controls.Add(this.btnBndLoop, 0, 5);
            this.tlp1.Controls.Add(this.btnDeleteHeadland, 3, 5);
            this.tlp1.Location = new System.Drawing.Point(701, 3);
            this.tlp1.Name = "tlp1";
            this.tlp1.RowCount = 8;
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.57143F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.14286F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.85714F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.142857F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.57143F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.57143F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 11.85714F));
            this.tlp1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp1.Size = new System.Drawing.Size(299, 698);
            this.tlp1.TabIndex = 565;
            // 
            // btnCancelTouch
            // 
            this.btnCancelTouch.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancelTouch.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tlp1.SetColumnSpan(this.btnCancelTouch, 2);
            this.btnCancelTouch.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnCancelTouch.FlatAppearance.BorderSize = 0;
            this.btnCancelTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTouch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelTouch.Image = global::AgOpenGPS.Properties.Resources.HeadlandDeletePoints;
            this.btnCancelTouch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelTouch.Location = new System.Drawing.Point(211, 108);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(72, 58);
            this.btnCancelTouch.TabIndex = 565;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.btnCancelTouch_Click);
            // 
            // nudSetDistance
            // 
            this.nudSetDistance.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.nudSetDistance.BackColor = System.Drawing.Color.White;
            this.tlp1.SetColumnSpan(this.nudSetDistance, 4);
            this.nudSetDistance.DecimalPlaces = 1;
            this.nudSetDistance.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSetDistance.Location = new System.Drawing.Point(17, 298);
            this.nudSetDistance.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.nudSetDistance.Name = "nudSetDistance";
            this.nudSetDistance.ReadOnly = true;
            this.nudSetDistance.Size = new System.Drawing.Size(161, 52);
            this.nudSetDistance.TabIndex = 464;
            this.nudSetDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSetDistance.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudSetDistance.Click += new System.EventHandler(this.nudSetDistance_Click);
            // 
            // FormHeadAche
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1006, 709);
            this.ControlBox = false;
            this.Controls.Add(this.tlp1);
            this.Controls.Add(this.oglSelf);
            this.Controls.Add(this.headingGroupBox);
            this.ForeColor = System.Drawing.Color.Black;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1022, 742);
            this.Name = "FormHeadAche";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Click 2 points on the Boundary to Begin";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormHeadLine_FormClosing);
            this.Load += new System.EventHandler(this.FormHeadLine_Load);
            this.ResizeEnd += new System.EventHandler(this.FormHeadAche_ResizeEnd);
            this.tlp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSetDistance)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl.GLControl oglSelf;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCycleForward;
        private System.Windows.Forms.Button btnDeleteCurve;
        private System.Windows.Forms.Button btnBLength;
        private System.Windows.Forms.Button btnALength;
        private System.Windows.Forms.GroupBox headingGroupBox;
        private System.Windows.Forms.RadioButton rbtnLine;
        private System.Windows.Forms.RadioButton rbtnCurve;
        private NudlessNumericUpDown nudSetDistance;
        private System.Windows.Forms.Button btnDeleteHeadland;
        private System.Windows.Forms.CheckBox cboxIsSectionControlled;
        private System.Windows.Forms.Button btnBndLoop;
        private System.Windows.Forms.Button btnCycleBackward;
        private System.Windows.Forms.ComboBox cboxToolWidths;
        private System.Windows.Forms.Label lblToolWidth;
        private System.Windows.Forms.Button btnHeadlandOff;
        private System.Windows.Forms.Button btnAShrink;
        private System.Windows.Forms.Button btnBShrink;
        private System.Windows.Forms.CheckBox cboxIsZoom;
        private System.Windows.Forms.TableLayoutPanel tlp1;
        private System.Windows.Forms.Button btnCancelTouch;
    }
}
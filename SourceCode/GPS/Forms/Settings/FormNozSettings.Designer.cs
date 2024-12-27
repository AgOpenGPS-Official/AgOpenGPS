namespace AgOpenGPS
{
    partial class FormNozSettings
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
            this.bntOK = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.lblRateSet1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblVolumeTank = new System.Windows.Forms.Label();
            this.lblVolumeApplied = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lblTankRemain = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblAcresAvailable = new System.Windows.Forms.Label();
            this.lblStatArea = new System.Windows.Forms.Label();
            this.lblRateSet2 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnZeroVolume = new System.Windows.Forms.Button();
            this.lblRateSet = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.nudTankVolume = new AgOpenGPS.NudlessNumericUpDown();
            this.nudZeroVolume = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSprayRateSet2 = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSprayMinPressure = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSprayRateSet1 = new AgOpenGPS   .NudlessNumericUpDown();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.nudNudge = new AgOpenGPS.NudlessNumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.nudRateAlarmPercent = new AgOpenGPS.NudlessNumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nudTankVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZeroVolume)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayRateSet2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayMinPressure)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayRateSet1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNudge)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRateAlarmPercent)).BeginInit();
            this.SuspendLayout();
            // 
            // bntOK
            // 
            this.bntOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.FlatAppearance.BorderSize = 0;
            this.bntOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(576, 420);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(166, 65);
            this.bntOK.TabIndex = 0;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.bntOK_Click);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.White;
            this.label10.Location = new System.Drawing.Point(29, 150);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(166, 28);
            this.label10.TabIndex = 590;
            this.label10.Text = "Min Pressure";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRateSet1
            // 
            this.lblRateSet1.BackColor = System.Drawing.Color.Transparent;
            this.lblRateSet1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRateSet1.ForeColor = System.Drawing.Color.White;
            this.lblRateSet1.Location = new System.Drawing.Point(551, 143);
            this.lblRateSet1.Name = "lblRateSet1";
            this.lblRateSet1.Size = new System.Drawing.Size(166, 28);
            this.lblRateSet1.TabIndex = 578;
            this.lblRateSet1.Text = " Gal / Acre";
            this.lblRateSet1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.White;
            this.label7.Location = new System.Drawing.Point(32, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 19);
            this.label7.TabIndex = 603;
            this.label7.Text = "Tank";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVolumeTank
            // 
            this.lblVolumeTank.BackColor = System.Drawing.Color.Transparent;
            this.lblVolumeTank.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeTank.ForeColor = System.Drawing.Color.White;
            this.lblVolumeTank.Location = new System.Drawing.Point(20, 31);
            this.lblVolumeTank.Name = "lblVolumeTank";
            this.lblVolumeTank.Size = new System.Drawing.Size(101, 38);
            this.lblVolumeTank.TabIndex = 604;
            this.lblVolumeTank.Text = "1200";
            this.lblVolumeTank.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVolumeApplied
            // 
            this.lblVolumeApplied.BackColor = System.Drawing.Color.Transparent;
            this.lblVolumeApplied.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVolumeApplied.ForeColor = System.Drawing.Color.White;
            this.lblVolumeApplied.Location = new System.Drawing.Point(121, 31);
            this.lblVolumeApplied.Name = "lblVolumeApplied";
            this.lblVolumeApplied.Size = new System.Drawing.Size(101, 38);
            this.lblVolumeApplied.TabIndex = 606;
            this.lblVolumeApplied.Text = "200";
            this.lblVolumeApplied.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.White;
            this.label14.Location = new System.Drawing.Point(133, 12);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(77, 19);
            this.label14.TabIndex = 605;
            this.label14.Text = "Applied";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTankRemain
            // 
            this.lblTankRemain.BackColor = System.Drawing.Color.Transparent;
            this.lblTankRemain.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTankRemain.ForeColor = System.Drawing.Color.White;
            this.lblTankRemain.Location = new System.Drawing.Point(222, 31);
            this.lblTankRemain.Name = "lblTankRemain";
            this.lblTankRemain.Size = new System.Drawing.Size(101, 38);
            this.lblTankRemain.TabIndex = 608;
            this.lblTankRemain.Text = "1000";
            this.lblTankRemain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.ForeColor = System.Drawing.Color.White;
            this.label16.Location = new System.Drawing.Point(234, 12);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 19);
            this.label16.TabIndex = 607;
            this.label16.Text = "Remain";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAcresAvailable
            // 
            this.lblAcresAvailable.BackColor = System.Drawing.Color.Transparent;
            this.lblAcresAvailable.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcresAvailable.ForeColor = System.Drawing.Color.White;
            this.lblAcresAvailable.Location = new System.Drawing.Point(323, 31);
            this.lblAcresAvailable.Name = "lblAcresAvailable";
            this.lblAcresAvailable.Size = new System.Drawing.Size(101, 38);
            this.lblAcresAvailable.TabIndex = 610;
            this.lblAcresAvailable.Text = "85.2";
            this.lblAcresAvailable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStatArea
            // 
            this.lblStatArea.BackColor = System.Drawing.Color.Transparent;
            this.lblStatArea.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatArea.ForeColor = System.Drawing.Color.White;
            this.lblStatArea.Location = new System.Drawing.Point(335, 12);
            this.lblStatArea.Name = "lblStatArea";
            this.lblStatArea.Size = new System.Drawing.Size(77, 19);
            this.lblStatArea.TabIndex = 609;
            this.lblStatArea.Text = "Ac";
            this.lblStatArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblRateSet2
            // 
            this.lblRateSet2.BackColor = System.Drawing.Color.Transparent;
            this.lblRateSet2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRateSet2.ForeColor = System.Drawing.Color.White;
            this.lblRateSet2.Location = new System.Drawing.Point(551, 283);
            this.lblRateSet2.Name = "lblRateSet2";
            this.lblRateSet2.Size = new System.Drawing.Size(166, 28);
            this.lblRateSet2.TabIndex = 612;
            this.lblRateSet2.Text = " Gal / Acre";
            this.lblRateSet2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(497, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 33);
            this.label2.TabIndex = 613;
            this.label2.Text = "1:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Arial", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(497, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 33);
            this.label3.TabIndex = 614;
            this.label3.Text = "2:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(276, 433);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 28);
            this.label1.TabIndex = 618;
            this.label1.Text = "Zero Applied";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnZeroVolume
            // 
            this.btnZeroVolume.BackColor = System.Drawing.Color.White;
            this.btnZeroVolume.BackgroundImage = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnZeroVolume.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnZeroVolume.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnZeroVolume.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroVolume.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroVolume.ForeColor = System.Drawing.Color.White;
            this.btnZeroVolume.Location = new System.Drawing.Point(285, 366);
            this.btnZeroVolume.Name = "btnZeroVolume";
            this.btnZeroVolume.Size = new System.Drawing.Size(149, 65);
            this.btnZeroVolume.TabIndex = 617;
            this.btnZeroVolume.UseVisualStyleBackColor = false;
            this.btnZeroVolume.Click += new System.EventHandler(this.btnZeroVolume_Click);
            // 
            // lblRateSet
            // 
            this.lblRateSet.BackColor = System.Drawing.Color.Transparent;
            this.lblRateSet.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRateSet.ForeColor = System.Drawing.Color.White;
            this.lblRateSet.Location = new System.Drawing.Point(276, 290);
            this.lblRateSet.Name = "lblRateSet";
            this.lblRateSet.Size = new System.Drawing.Size(166, 28);
            this.lblRateSet.TabIndex = 616;
            this.lblRateSet.Text = "Applied";
            this.lblRateSet.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(276, 149);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(166, 28);
            this.label4.TabIndex = 620;
            this.label4.Text = "Tank Volume";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudTankVolume
            // 
            this.nudTankVolume.BackColor = System.Drawing.Color.White;
            this.nudTankVolume.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTankVolume.Location = new System.Drawing.Point(259, 84);
            this.nudTankVolume.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudTankVolume.Name = "nudTankVolume";
            this.nudTankVolume.ReadOnly = true;
            this.nudTankVolume.Size = new System.Drawing.Size(201, 65);
            this.nudTankVolume.TabIndex = 619;
            this.nudTankVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudTankVolume.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudTankVolume.Click += new System.EventHandler(this.nudTankVolume_Click);
            // 
            // nudZeroVolume
            // 
            this.nudZeroVolume.BackColor = System.Drawing.Color.White;
            this.nudZeroVolume.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudZeroVolume.Location = new System.Drawing.Point(259, 225);
            this.nudZeroVolume.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudZeroVolume.Name = "nudZeroVolume";
            this.nudZeroVolume.ReadOnly = true;
            this.nudZeroVolume.Size = new System.Drawing.Size(201, 65);
            this.nudZeroVolume.TabIndex = 615;
            this.nudZeroVolume.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudZeroVolume.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudZeroVolume.Click += new System.EventHandler(this.nudZeroVolume_Click);
            // 
            // nudSprayRateSet2
            // 
            this.nudSprayRateSet2.BackColor = System.Drawing.Color.PaleGreen;
            this.nudSprayRateSet2.DecimalPlaces = 1;
            this.nudSprayRateSet2.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSprayRateSet2.Location = new System.Drawing.Point(541, 222);
            this.nudSprayRateSet2.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudSprayRateSet2.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudSprayRateSet2.Name = "nudSprayRateSet2";
            this.nudSprayRateSet2.ReadOnly = true;
            this.nudSprayRateSet2.Size = new System.Drawing.Size(186, 65);
            this.nudSprayRateSet2.TabIndex = 611;
            this.nudSprayRateSet2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSprayRateSet2.Value = new decimal(new int[] {
            9999,
            0,
            0,
            65536});
            this.nudSprayRateSet2.Click += new System.EventHandler(this.nudSprayRateSet2_Click);
            // 
            // nudSprayMinPressure
            // 
            this.nudSprayMinPressure.BackColor = System.Drawing.Color.White;
            this.nudSprayMinPressure.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSprayMinPressure.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudSprayMinPressure.Location = new System.Drawing.Point(37, 84);
            this.nudSprayMinPressure.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSprayMinPressure.Name = "nudSprayMinPressure";
            this.nudSprayMinPressure.ReadOnly = true;
            this.nudSprayMinPressure.Size = new System.Drawing.Size(150, 65);
            this.nudSprayMinPressure.TabIndex = 591;
            this.nudSprayMinPressure.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSprayMinPressure.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            this.nudSprayMinPressure.Click += new System.EventHandler(this.nudSprayMinPressure_Click);
            // 
            // nudSprayRateSet1
            // 
            this.nudSprayRateSet1.BackColor = System.Drawing.Color.PaleGreen;
            this.nudSprayRateSet1.DecimalPlaces = 1;
            this.nudSprayRateSet1.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSprayRateSet1.Location = new System.Drawing.Point(541, 82);
            this.nudSprayRateSet1.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudSprayRateSet1.Minimum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudSprayRateSet1.Name = "nudSprayRateSet1";
            this.nudSprayRateSet1.ReadOnly = true;
            this.nudSprayRateSet1.Size = new System.Drawing.Size(186, 65);
            this.nudSprayRateSet1.TabIndex = 577;
            this.nudSprayRateSet1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSprayRateSet1.Value = new decimal(new int[] {
            9999,
            0,
            0,
            65536});
            this.nudSprayRateSet1.Click += new System.EventHandler(this.nudSprayRateSet1_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // nudNudge
            // 
            this.nudNudge.BackColor = System.Drawing.Color.White;
            this.nudNudge.DecimalPlaces = 1;
            this.nudNudge.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudNudge.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudNudge.Location = new System.Drawing.Point(37, 366);
            this.nudNudge.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudNudge.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudNudge.Name = "nudNudge";
            this.nudNudge.ReadOnly = true;
            this.nudNudge.Size = new System.Drawing.Size(150, 65);
            this.nudNudge.TabIndex = 623;
            this.nudNudge.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudNudge.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudNudge.Click += new System.EventHandler(this.nudNudge_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(29, 435);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(166, 28);
            this.label5.TabIndex = 622;
            this.label5.Text = "Rate Nudge";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudRateAlarmPercent
            // 
            this.nudRateAlarmPercent.BackColor = System.Drawing.Color.White;
            this.nudRateAlarmPercent.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudRateAlarmPercent.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.nudRateAlarmPercent.Location = new System.Drawing.Point(37, 222);
            this.nudRateAlarmPercent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudRateAlarmPercent.Name = "nudRateAlarmPercent";
            this.nudRateAlarmPercent.ReadOnly = true;
            this.nudRateAlarmPercent.Size = new System.Drawing.Size(150, 65);
            this.nudRateAlarmPercent.TabIndex = 625;
            this.nudRateAlarmPercent.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudRateAlarmPercent.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudRateAlarmPercent.Click += new System.EventHandler(this.nudRateAlarmPercent_Click);
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(29, 288);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(166, 28);
            this.label6.TabIndex = 624;
            this.label6.Text = "Rate Alarm %";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormNozSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(45)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(752, 498);
            this.ControlBox = false;
            this.Controls.Add(this.nudRateAlarmPercent);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.nudNudge);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudTankVolume);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnZeroVolume);
            this.Controls.Add(this.nudZeroVolume);
            this.Controls.Add(this.lblRateSet);
            this.Controls.Add(this.nudSprayRateSet2);
            this.Controls.Add(this.lblRateSet2);
            this.Controls.Add(this.lblAcresAvailable);
            this.Controls.Add(this.lblStatArea);
            this.Controls.Add(this.lblTankRemain);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.lblVolumeApplied);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.lblVolumeTank);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.nudSprayMinPressure);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.nudSprayRateSet1);
            this.Controls.Add(this.bntOK);
            this.Controls.Add(this.lblRateSet1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormNozSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Nozzle Configuration";
            this.Load += new System.EventHandler(this.FormDisplaySettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudTankVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudZeroVolume)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayRateSet2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayMinPressure)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSprayRateSet1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNudge)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRateAlarmPercent)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button bntOK;
        private NudlessNumericUpDown nudSprayMinPressure;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblRateSet1;
        private NudlessNumericUpDown nudSprayRateSet1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblVolumeTank;
        private System.Windows.Forms.Label lblVolumeApplied;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lblTankRemain;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblAcresAvailable;
        private System.Windows.Forms.Label lblStatArea;
        private NudlessNumericUpDown nudSprayRateSet2;
        private System.Windows.Forms.Label lblRateSet2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnZeroVolume;
        private NudlessNumericUpDown nudZeroVolume;
        private System.Windows.Forms.Label lblRateSet;
        private NudlessNumericUpDown nudTankVolume;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private NudlessNumericUpDown nudNudge;
        private System.Windows.Forms.Label label5;
        private NudlessNumericUpDown nudRateAlarmPercent;
        private System.Windows.Forms.Label label6;
    }
}
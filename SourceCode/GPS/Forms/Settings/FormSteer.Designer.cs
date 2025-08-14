namespace AgOpenGPS
{
    partial class FormSteer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSteer));
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblSteerAngle = new System.Windows.Forms.Label();
            this.lblSteerAngleActual = new System.Windows.Forms.Label();
            this.lblError = new System.Windows.Forms.Label();
            this.lblPWMDisplay = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.labelSteerAngle = new System.Windows.Forms.Label();
            this.labelDiameter = new System.Windows.Forms.Label();
            this.lblCalcSteerAngleInner = new System.Windows.Forms.Label();
            this.lblDiameter = new System.Windows.Forms.Label();
            this.pbarSensor = new System.Windows.Forms.ProgressBar();
            this.lblPercentFS = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPP = new System.Windows.Forms.TabPage();
            this.labelAquire = new System.Windows.Forms.Label();
            this.label71 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.labelIntegralInfo = new System.Windows.Forms.Label();
            this.labelSlow = new System.Windows.Forms.Label();
            this.labelFast = new System.Windows.Forms.Label();
            this.labelSteerResponse = new System.Windows.Forms.Label();
            this.lblHoldLookAhead = new System.Windows.Forms.Label();
            this.hsbarHoldLookAhead = new System.Windows.Forms.HScrollBar();
            this.hsbarIntegralPurePursuit = new System.Windows.Forms.HScrollBar();
            this.label26 = new System.Windows.Forms.Label();
            this.labelIntegralPP = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.lblAcquirePP = new System.Windows.Forms.Label();
            this.lblPureIntegral = new System.Windows.Forms.Label();
            this.tabStan = new System.Windows.Forms.TabPage();
            this.label74 = new System.Windows.Forms.Label();
            this.label73 = new System.Windows.Forms.Label();
            this.label72 = new System.Windows.Forms.Label();
            this.lblIntegralPercent = new System.Windows.Forms.Label();
            this.hsbarIntegral = new System.Windows.Forms.HScrollBar();
            this.labelIntergralStanley = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblHeadingErrorGain = new System.Windows.Forms.Label();
            this.lblStanleyGain = new System.Windows.Forms.Label();
            this.labelHeading = new System.Windows.Forms.Label();
            this.labelDistance = new System.Windows.Forms.Label();
            this.hsbarStanleyGain = new System.Windows.Forms.HScrollBar();
            this.hsbarHeadingErrorGain = new System.Windows.Forms.HScrollBar();
            this.tabSteer = new System.Windows.Forms.TabPage();
            this.label80 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.labelMaxSteerAngle = new System.Windows.Forms.Label();
            this.lblAV_Set = new System.Windows.Forms.Label();
            this.lblAV_Act = new System.Windows.Forms.Label();
            this.lblMaxSteerAngle = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.labelAckermann = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.labelCountsPerDegree = new System.Windows.Forms.Label();
            this.hsbarAckerman = new System.Windows.Forms.HScrollBar();
            this.hsbarMaxSteerAngle = new System.Windows.Forms.HScrollBar();
            this.lblAckerman = new System.Windows.Forms.Label();
            this.pbarRight = new System.Windows.Forms.ProgressBar();
            this.pbarLeft = new System.Windows.Forms.ProgressBar();
            this.lblActualSteerAngleUpper = new System.Windows.Forms.Label();
            this.btnZeroWAS = new System.Windows.Forms.Button();
            this.hsbarCountsPerDegree = new System.Windows.Forms.HScrollBar();
            this.labelWasZero = new System.Windows.Forms.Label();
            this.lblCountsPerDegree = new System.Windows.Forms.Label();
            this.hsbarWasOffset = new System.Windows.Forms.HScrollBar();
            this.lblSteerAngleSensorZero = new System.Windows.Forms.Label();
            this.label81 = new System.Windows.Forms.Label();
            this.tabPPAdv = new System.Windows.Forms.TabPage();
            this.labelAquireFactor = new System.Windows.Forms.Label();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.label59 = new System.Windows.Forms.Label();
            this.lblDistanceAdv = new System.Windows.Forms.Label();
            this.hsbarLookAheadMult = new System.Windows.Forms.HScrollBar();
            this.labelDist = new System.Windows.Forms.Label();
            this.lblLookAheadMult = new System.Windows.Forms.Label();
            this.lblHoldAdv = new System.Windows.Forms.Label();
            this.labelSpeedFactor = new System.Windows.Forms.Label();
            this.lblAcqAdv = new System.Windows.Forms.Label();
            this.labelDeadzone = new System.Windows.Forms.Label();
            this.hsbarAcquireFactor = new System.Windows.Forms.HScrollBar();
            this.labelAquire2 = new System.Windows.Forms.Label();
            this.lblAcquireFactor = new System.Windows.Forms.Label();
            this.labelHold = new System.Windows.Forms.Label();
            this.labelAquireDescription = new System.Windows.Forms.Label();
            this.labelOnDelay = new System.Windows.Forms.Label();
            this.labelHeadingDegree = new System.Windows.Forms.Label();
            this.nudDeadZoneDelay = new AgOpenGPS.NudlessNumericUpDown();
            this.nudDeadZoneHeading = new AgOpenGPS.NudlessNumericUpDown();
            this.tabGain = new System.Windows.Forms.TabPage();
            this.label75 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.label77 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.labelMinToMove = new System.Windows.Forms.Label();
            this.labelMaxLimit = new System.Windows.Forms.Label();
            this.labelProportionalGain = new System.Windows.Forms.Label();
            this.hsbarMinPWM = new System.Windows.Forms.HScrollBar();
            this.hsbarProportionalGain = new System.Windows.Forms.HScrollBar();
            this.lblProportionalGain = new System.Windows.Forms.Label();
            this.lblHighSteerPWM = new System.Windows.Forms.Label();
            this.lblMinPWM = new System.Windows.Forms.Label();
            this.hsbarHighSteerPWM = new System.Windows.Forms.HScrollBar();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.lblSideHillComp = new System.Windows.Forms.Label();
            this.hsbarSideHillComp = new System.Windows.Forms.HScrollBar();
            this.labelSideHill = new System.Windows.Forms.Label();
            this.labelSteerDescription = new System.Windows.Forms.Label();
            this.labelPressureTurnSensor = new System.Windows.Forms.Label();
            this.labelCurrentTurnSensor = new System.Windows.Forms.Label();
            this.labelEncoder = new System.Windows.Forms.Label();
            this.labelInvertMotor = new System.Windows.Forms.Label();
            this.labelInvertRelays = new System.Windows.Forms.Label();
            this.labelSendAndSave = new System.Windows.Forms.Label();
            this.cboxMotorDrive = new System.Windows.Forms.ComboBox();
            this.cboxSteerEnable = new System.Windows.Forms.ComboBox();
            this.labelSteerEnable = new System.Windows.Forms.Label();
            this.cboxConv = new System.Windows.Forms.ComboBox();
            this.labelMotorDriver = new System.Windows.Forms.Label();
            this.labelADConverter = new System.Windows.Forms.Label();
            this.labelTurnSensor = new System.Windows.Forms.Label();
            this.label55 = new System.Windows.Forms.Label();
            this.labelInvertWas = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnExpand = new AgLibrary.Controls.RepeatButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnStartSA = new System.Windows.Forms.Button();
            this.btnFreeDriveZero = new System.Windows.Forms.Button();
            this.btnSteerAngleUp = new AgLibrary.Controls.RepeatButton();
            this.btnFreeDrive = new System.Windows.Forms.Button();
            this.btnSteerAngleDown = new AgLibrary.Controls.RepeatButton();
            this.hsbarSensor = new System.Windows.Forms.HScrollBar();
            this.lblhsbarSensor = new System.Windows.Forms.Label();
            this.labelReset = new System.Windows.Forms.Label();
            this.cboxXY = new System.Windows.Forms.ComboBox();
            this.labelIMUAxis = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.tabSteerSettings = new System.Windows.Forms.TabControl();
            this.tabSensors = new System.Windows.Forms.TabPage();
            this.nudMaxCounts = new AgOpenGPS.NudlessNumericUpDown();
            this.cboxCurrentSensor = new System.Windows.Forms.CheckBox();
            this.cboxEncoder = new System.Windows.Forms.CheckBox();
            this.cboxPressureSensor = new System.Windows.Forms.CheckBox();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.chkInvertWAS = new System.Windows.Forms.CheckBox();
            this.chkInvertSteer = new System.Windows.Forms.CheckBox();
            this.chkSteerInvertRelays = new System.Windows.Forms.CheckBox();
            this.cboxDanfoss = new System.Windows.Forms.CheckBox();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.labelSteerInReverse = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.hsbarUTurnCompensation = new System.Windows.Forms.HScrollBar();
            this.lblUTurnCompensation = new System.Windows.Forms.Label();
            this.labelUturnCompensation = new System.Windows.Forms.Label();
            this.cboxSteerInReverse = new System.Windows.Forms.CheckBox();
            this.btnStanleyPure = new System.Windows.Forms.Button();
            this.tabAlarm = new System.Windows.Forms.TabPage();
            this.labelMinSpeed = new System.Windows.Forms.Label();
            this.label166 = new System.Windows.Forms.Label();
            this.labelMaxSpeed = new System.Windows.Forms.Label();
            this.label163 = new System.Windows.Forms.Label();
            this.label160 = new System.Windows.Forms.Label();
            this.labelManualTurns = new System.Windows.Forms.Label();
            this.pictureBox17 = new System.Windows.Forms.PictureBox();
            this.pictureBox16 = new System.Windows.Forms.PictureBox();
            this.pictureBox10 = new System.Windows.Forms.PictureBox();
            this.nudMinSteerSpeed = new AgOpenGPS.NudlessNumericUpDown();
            this.nudMaxSteerSpeed = new AgOpenGPS.NudlessNumericUpDown();
            this.nudGuidanceSpeedLimit = new AgOpenGPS.NudlessNumericUpDown();
            this.tabOnTheLine = new System.Windows.Forms.TabPage();
            this.labelLineWidth = new System.Windows.Forms.Label();
            this.label44 = new System.Windows.Forms.Label();
            this.labelNudgeDistance = new System.Windows.Forms.Label();
            this.label43 = new System.Windows.Forms.Label();
            this.groupboxlabelGuidanceBar = new System.Windows.Forms.GroupBox();
            this.rbtnSteerBar = new System.Windows.Forms.RadioButton();
            this.labelSteerBar = new System.Windows.Forms.Label();
            this.labelOnOff = new System.Windows.Forms.Label();
            this.rbtnLightBar = new System.Windows.Forms.RadioButton();
            this.labelLightbar = new System.Windows.Forms.Label();
            this.chkDisplayLightbar = new System.Windows.Forms.CheckBox();
            this.labelCmPix = new System.Windows.Forms.Label();
            this.label107 = new System.Windows.Forms.Label();
            this.label46 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.labelNextGuidanceLine = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox12 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.nudcmPerPixel = new AgOpenGPS.NudlessNumericUpDown();
            this.nudLineWidth = new AgOpenGPS.NudlessNumericUpDown();
            this.nudSnapDistance = new AgOpenGPS.NudlessNumericUpDown();
            this.nudGuidanceLookAhead = new AgOpenGPS.NudlessNumericUpDown();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.labelWizard = new System.Windows.Forms.Label();
            this.btnSteerWizard = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.pboxSendSteer = new System.Windows.Forms.PictureBox();
            this.btnSendSteerConfigPGN = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPP.SuspendLayout();
            this.tabStan.SuspendLayout();
            this.tabSteer.SuspendLayout();
            this.tabPPAdv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadZoneDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadZoneHeading)).BeginInit();
            this.tabGain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabSteerSettings.SuspendLayout();
            this.tabSensors.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxCounts)).BeginInit();
            this.tabConfig.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabAlarm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSteerSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxSteerSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGuidanceSpeedLimit)).BeginInit();
            this.tabOnTheLine.SuspendLayout();
            this.groupboxlabelGuidanceBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudcmPerPixel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSnapDistance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGuidanceLookAhead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxSendSteer)).BeginInit();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // lblSteerAngle
            // 
            this.lblSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngle.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngle.Location = new System.Drawing.Point(32, 8);
            this.lblSteerAngle.Name = "lblSteerAngle";
            this.lblSteerAngle.Size = new System.Drawing.Size(71, 23);
            this.lblSteerAngle.TabIndex = 306;
            this.lblSteerAngle.Text = "-55.5";
            this.lblSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSteerAngle.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // lblSteerAngleActual
            // 
            this.lblSteerAngleActual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSteerAngleActual.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngleActual.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleActual.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngleActual.Location = new System.Drawing.Point(131, 8);
            this.lblSteerAngleActual.Name = "lblSteerAngleActual";
            this.lblSteerAngleActual.Size = new System.Drawing.Size(71, 23);
            this.lblSteerAngleActual.TabIndex = 311;
            this.lblSteerAngleActual.Text = "-55.5";
            this.lblSteerAngleActual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSteerAngleActual.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // lblError
            // 
            this.lblError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Black;
            this.lblError.Location = new System.Drawing.Point(225, 8);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(71, 23);
            this.lblError.TabIndex = 312;
            this.lblError.Text = "-30.0";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblError.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // lblPWMDisplay
            // 
            this.lblPWMDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPWMDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblPWMDisplay.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPWMDisplay.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPWMDisplay.Location = new System.Drawing.Point(78, 76);
            this.lblPWMDisplay.Name = "lblPWMDisplay";
            this.lblPWMDisplay.Size = new System.Drawing.Size(64, 23);
            this.lblPWMDisplay.TabIndex = 316;
            this.lblPWMDisplay.Text = "255";
            this.lblPWMDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(17, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 23);
            this.label9.TabIndex = 318;
            this.label9.Text = "PWM:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(101, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(30, 16);
            this.label11.TabIndex = 319;
            this.label11.Text = "Act:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label11.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(4, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(31, 16);
            this.label12.TabIndex = 320;
            this.label12.Text = "Set:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label12.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(198, 11);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 16);
            this.label13.TabIndex = 321;
            this.label13.Text = "Err:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.label13.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // label14
            // 
            this.label14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label14.Location = new System.Drawing.Point(289, 77);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 16);
            this.label14.TabIndex = 322;
            this.label14.Text = "Or +5";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSteerAngle
            // 
            this.labelSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSteerAngle.AutoSize = true;
            this.labelSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.labelSteerAngle.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSteerAngle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelSteerAngle.Location = new System.Drawing.Point(113, 113);
            this.labelSteerAngle.Name = "labelSteerAngle";
            this.labelSteerAngle.Size = new System.Drawing.Size(114, 23);
            this.labelSteerAngle.TabIndex = 328;
            this.labelSteerAngle.Text = "Steer Angle:";
            this.labelSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDiameter
            // 
            this.labelDiameter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelDiameter.AutoSize = true;
            this.labelDiameter.BackColor = System.Drawing.Color.Transparent;
            this.labelDiameter.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDiameter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelDiameter.Location = new System.Drawing.Point(115, 148);
            this.labelDiameter.Name = "labelDiameter";
            this.labelDiameter.Size = new System.Drawing.Size(93, 23);
            this.labelDiameter.TabIndex = 327;
            this.labelDiameter.Text = "Diameter:";
            this.labelDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCalcSteerAngleInner
            // 
            this.lblCalcSteerAngleInner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCalcSteerAngleInner.AutoSize = true;
            this.lblCalcSteerAngleInner.BackColor = System.Drawing.Color.Transparent;
            this.lblCalcSteerAngleInner.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalcSteerAngleInner.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCalcSteerAngleInner.Location = new System.Drawing.Point(238, 113);
            this.lblCalcSteerAngleInner.Name = "lblCalcSteerAngleInner";
            this.lblCalcSteerAngleInner.Size = new System.Drawing.Size(40, 23);
            this.lblCalcSteerAngleInner.TabIndex = 326;
            this.lblCalcSteerAngleInner.Text = "0.0";
            this.lblCalcSteerAngleInner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiameter
            // 
            this.lblDiameter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDiameter.AutoSize = true;
            this.lblDiameter.BackColor = System.Drawing.Color.Transparent;
            this.lblDiameter.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiameter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDiameter.Location = new System.Drawing.Point(238, 148);
            this.lblDiameter.Name = "lblDiameter";
            this.lblDiameter.Size = new System.Drawing.Size(64, 23);
            this.lblDiameter.TabIndex = 325;
            this.lblDiameter.Text = "0.0 m";
            this.lblDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbarSensor
            // 
            this.pbarSensor.BackColor = System.Drawing.Color.White;
            this.pbarSensor.Location = new System.Drawing.Point(176, 205);
            this.pbarSensor.Maximum = 255;
            this.pbarSensor.Name = "pbarSensor";
            this.pbarSensor.Size = new System.Drawing.Size(257, 53);
            this.pbarSensor.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbarSensor.TabIndex = 496;
            // 
            // lblPercentFS
            // 
            this.lblPercentFS.AutoSize = true;
            this.lblPercentFS.BackColor = System.Drawing.Color.Transparent;
            this.lblPercentFS.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercentFS.ForeColor = System.Drawing.Color.Black;
            this.lblPercentFS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPercentFS.Location = new System.Drawing.Point(439, 216);
            this.lblPercentFS.Name = "lblPercentFS";
            this.lblPercentFS.Size = new System.Drawing.Size(57, 29);
            this.lblPercentFS.TabIndex = 495;
            this.lblPercentFS.Text = "0%";
            this.lblPercentFS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabPP);
            this.tabControl1.Controls.Add(this.tabStan);
            this.tabControl1.Controls.Add(this.tabSteer);
            this.tabControl1.Controls.Add(this.tabPPAdv);
            this.tabControl1.Controls.Add(this.tabGain);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.ItemSize = new System.Drawing.Size(65, 48);
            this.tabControl1.Location = new System.Drawing.Point(2, 5);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 402);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 347;
            // 
            // tabPP
            // 
            this.tabPP.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPP.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_PP;
            this.tabPP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabPP.Controls.Add(this.labelAquire);
            this.tabPP.Controls.Add(this.label71);
            this.tabPP.Controls.Add(this.label69);
            this.tabPP.Controls.Add(this.labelIntegralInfo);
            this.tabPP.Controls.Add(this.labelSlow);
            this.tabPP.Controls.Add(this.labelFast);
            this.tabPP.Controls.Add(this.labelSteerResponse);
            this.tabPP.Controls.Add(this.lblHoldLookAhead);
            this.tabPP.Controls.Add(this.hsbarHoldLookAhead);
            this.tabPP.Controls.Add(this.hsbarIntegralPurePursuit);
            this.tabPP.Controls.Add(this.label26);
            this.tabPP.Controls.Add(this.labelIntegralPP);
            this.tabPP.Controls.Add(this.label20);
            this.tabPP.Controls.Add(this.label18);
            this.tabPP.Controls.Add(this.lblAcquirePP);
            this.tabPP.Controls.Add(this.lblPureIntegral);
            this.tabPP.ForeColor = System.Drawing.Color.Black;
            this.tabPP.ImageIndex = 3;
            this.tabPP.Location = new System.Drawing.Point(4, 52);
            this.tabPP.Name = "tabPP";
            this.tabPP.Size = new System.Drawing.Size(365, 346);
            this.tabPP.TabIndex = 16;
            // 
            // labelAquire
            // 
            this.labelAquire.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAquire.ForeColor = System.Drawing.Color.Black;
            this.labelAquire.Location = new System.Drawing.Point(85, 159);
            this.labelAquire.Name = "labelAquire";
            this.labelAquire.Size = new System.Drawing.Size(111, 19);
            this.labelAquire.TabIndex = 548;
            this.labelAquire.Text = "Acquire:";
            this.labelAquire.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.labelAquire.Visible = false;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.BackColor = System.Drawing.Color.Transparent;
            this.label71.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.ForeColor = System.Drawing.Color.Black;
            this.label71.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label71.Location = new System.Drawing.Point(69, 274);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(14, 16);
            this.label71.TabIndex = 547;
            this.label71.Text = "0";
            this.label71.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.BackColor = System.Drawing.Color.Transparent;
            this.label69.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label69.ForeColor = System.Drawing.Color.Black;
            this.label69.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label69.Location = new System.Drawing.Point(51, 121);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(25, 16);
            this.label69.TabIndex = 546;
            this.label69.Text = "3.0";
            this.label69.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelIntegralInfo
            // 
            this.labelIntegralInfo.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIntegralInfo.ForeColor = System.Drawing.Color.Black;
            this.labelIntegralInfo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelIntegralInfo.Location = new System.Drawing.Point(6, 290);
            this.labelIntegralInfo.Name = "labelIntegralInfo";
            this.labelIntegralInfo.Size = new System.Drawing.Size(288, 46);
            this.labelIntegralInfo.TabIndex = 361;
            this.labelIntegralInfo.Text = "It will slowly increase steer angle to reduce cross track error";
            this.labelIntegralInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelSlow
            // 
            this.labelSlow.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSlow.ForeColor = System.Drawing.Color.Black;
            this.labelSlow.Location = new System.Drawing.Point(279, 64);
            this.labelSlow.Name = "labelSlow";
            this.labelSlow.Size = new System.Drawing.Size(84, 19);
            this.labelSlow.TabIndex = 360;
            this.labelSlow.Text = "Slow";
            this.labelSlow.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelFast
            // 
            this.labelFast.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFast.ForeColor = System.Drawing.Color.Black;
            this.labelFast.Location = new System.Drawing.Point(10, 64);
            this.labelFast.Name = "labelFast";
            this.labelFast.Size = new System.Drawing.Size(72, 19);
            this.labelFast.TabIndex = 358;
            this.labelFast.Text = "Fast";
            this.labelFast.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSteerResponse
            // 
            this.labelSteerResponse.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSteerResponse.ForeColor = System.Drawing.Color.Black;
            this.labelSteerResponse.Location = new System.Drawing.Point(81, 60);
            this.labelSteerResponse.Name = "labelSteerResponse";
            this.labelSteerResponse.Size = new System.Drawing.Size(196, 23);
            this.labelSteerResponse.TabIndex = 356;
            this.labelSteerResponse.Text = "Steer Response";
            this.labelSteerResponse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHoldLookAhead
            // 
            this.lblHoldLookAhead.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoldLookAhead.ForeColor = System.Drawing.Color.Black;
            this.lblHoldLookAhead.Location = new System.Drawing.Point(3, 83);
            this.lblHoldLookAhead.Name = "lblHoldLookAhead";
            this.lblHoldLookAhead.Size = new System.Drawing.Size(47, 35);
            this.lblHoldLookAhead.TabIndex = 355;
            this.lblHoldLookAhead.Text = "5.2";
            this.lblHoldLookAhead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarHoldLookAhead
            // 
            this.hsbarHoldLookAhead.LargeChange = 1;
            this.hsbarHoldLookAhead.Location = new System.Drawing.Point(53, 86);
            this.hsbarHoldLookAhead.Maximum = 70;
            this.hsbarHoldLookAhead.Minimum = 10;
            this.hsbarHoldLookAhead.Name = "hsbarHoldLookAhead";
            this.hsbarHoldLookAhead.Size = new System.Drawing.Size(264, 30);
            this.hsbarHoldLookAhead.TabIndex = 354;
            this.hsbarHoldLookAhead.Value = 25;
            this.hsbarHoldLookAhead.ValueChanged += new System.EventHandler(this.hsbarHoldLookAhead_ValueChanged);
            // 
            // hsbarIntegralPurePursuit
            // 
            this.hsbarIntegralPurePursuit.LargeChange = 1;
            this.hsbarIntegralPurePursuit.Location = new System.Drawing.Point(53, 239);
            this.hsbarIntegralPurePursuit.Name = "hsbarIntegralPurePursuit";
            this.hsbarIntegralPurePursuit.Size = new System.Drawing.Size(264, 30);
            this.hsbarIntegralPurePursuit.TabIndex = 349;
            this.hsbarIntegralPurePursuit.Value = 5;
            this.hsbarIntegralPurePursuit.ValueChanged += new System.EventHandler(this.hsbarIntegralPurePursuit_ValueChanged);
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.SteelBlue;
            this.label26.Location = new System.Drawing.Point(8, 3);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(351, 26);
            this.label26.TabIndex = 348;
            this.label26.Text = "Pure Pursuit";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelIntegralPP
            // 
            this.labelIntegralPP.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIntegralPP.ForeColor = System.Drawing.Color.Black;
            this.labelIntegralPP.Location = new System.Drawing.Point(53, 217);
            this.labelIntegralPP.Name = "labelIntegralPP";
            this.labelIntegralPP.Size = new System.Drawing.Size(264, 19);
            this.labelIntegralPP.TabIndex = 342;
            this.labelIntegralPP.Text = "Integral";
            this.labelIntegralPP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelIntegralPP.UseCompatibleTextRendering = true;
            // 
            // label20
            // 
            this.label20.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label20.Location = new System.Drawing.Point(579, 228);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(204, 30);
            this.label20.TabIndex = 302;
            this.label20.Text = "Look Ahead Min";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label18.Location = new System.Drawing.Point(561, 13);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(204, 30);
            this.label18.TabIndex = 300;
            this.label18.Text = "Look Ahead";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAcquirePP
            // 
            this.lblAcquirePP.AutoSize = true;
            this.lblAcquirePP.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcquirePP.ForeColor = System.Drawing.Color.Black;
            this.lblAcquirePP.Location = new System.Drawing.Point(190, 159);
            this.lblAcquirePP.Name = "lblAcquirePP";
            this.lblAcquirePP.Size = new System.Drawing.Size(34, 19);
            this.lblAcquirePP.TabIndex = 515;
            this.lblAcquirePP.Text = "2.6";
            this.lblAcquirePP.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAcquirePP.Visible = false;
            // 
            // lblPureIntegral
            // 
            this.lblPureIntegral.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPureIntegral.ForeColor = System.Drawing.Color.Black;
            this.lblPureIntegral.Location = new System.Drawing.Point(5, 236);
            this.lblPureIntegral.Name = "lblPureIntegral";
            this.lblPureIntegral.Size = new System.Drawing.Size(50, 35);
            this.lblPureIntegral.TabIndex = 350;
            this.lblPureIntegral.Text = "20";
            this.lblPureIntegral.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabStan
            // 
            this.tabStan.BackColor = System.Drawing.Color.Gainsboro;
            this.tabStan.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_Stanley;
            this.tabStan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabStan.Controls.Add(this.label74);
            this.tabStan.Controls.Add(this.label73);
            this.tabStan.Controls.Add(this.label72);
            this.tabStan.Controls.Add(this.lblIntegralPercent);
            this.tabStan.Controls.Add(this.hsbarIntegral);
            this.tabStan.Controls.Add(this.labelIntergralStanley);
            this.tabStan.Controls.Add(this.label25);
            this.tabStan.Controls.Add(this.lblHeadingErrorGain);
            this.tabStan.Controls.Add(this.lblStanleyGain);
            this.tabStan.Controls.Add(this.labelHeading);
            this.tabStan.Controls.Add(this.labelDistance);
            this.tabStan.Controls.Add(this.hsbarStanleyGain);
            this.tabStan.Controls.Add(this.hsbarHeadingErrorGain);
            this.tabStan.ImageIndex = 2;
            this.tabStan.Location = new System.Drawing.Point(4, 52);
            this.tabStan.Name = "tabStan";
            this.tabStan.Size = new System.Drawing.Size(365, 346);
            this.tabStan.TabIndex = 15;
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.BackColor = System.Drawing.Color.Transparent;
            this.label74.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label74.ForeColor = System.Drawing.Color.Black;
            this.label74.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label74.Location = new System.Drawing.Point(68, 293);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(14, 16);
            this.label74.TabIndex = 549;
            this.label74.Text = "0";
            this.label74.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.BackColor = System.Drawing.Color.Transparent;
            this.label73.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label73.ForeColor = System.Drawing.Color.Black;
            this.label73.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label73.Location = new System.Drawing.Point(68, 203);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(25, 16);
            this.label73.TabIndex = 548;
            this.label73.Text = "1.0";
            this.label73.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.BackColor = System.Drawing.Color.Transparent;
            this.label72.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label72.ForeColor = System.Drawing.Color.Black;
            this.label72.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label72.Location = new System.Drawing.Point(68, 113);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(25, 16);
            this.label72.TabIndex = 547;
            this.label72.Text = "1.0";
            this.label72.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblIntegralPercent
            // 
            this.lblIntegralPercent.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntegralPercent.ForeColor = System.Drawing.Color.Black;
            this.lblIntegralPercent.Location = new System.Drawing.Point(3, 259);
            this.lblIntegralPercent.Name = "lblIntegralPercent";
            this.lblIntegralPercent.Size = new System.Drawing.Size(60, 35);
            this.lblIntegralPercent.TabIndex = 352;
            this.lblIntegralPercent.Text = "888";
            this.lblIntegralPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarIntegral
            // 
            this.hsbarIntegral.LargeChange = 1;
            this.hsbarIntegral.Location = new System.Drawing.Point(68, 261);
            this.hsbarIntegral.Name = "hsbarIntegral";
            this.hsbarIntegral.Size = new System.Drawing.Size(202, 30);
            this.hsbarIntegral.TabIndex = 351;
            this.hsbarIntegral.Value = 5;
            this.hsbarIntegral.ValueChanged += new System.EventHandler(this.hsbarIntegral_ValueChanged);
            // 
            // labelIntergralStanley
            // 
            this.labelIntergralStanley.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelIntergralStanley.ForeColor = System.Drawing.Color.Black;
            this.labelIntergralStanley.Location = new System.Drawing.Point(63, 232);
            this.labelIntergralStanley.Name = "labelIntergralStanley";
            this.labelIntergralStanley.Size = new System.Drawing.Size(204, 30);
            this.labelIntergralStanley.TabIndex = 350;
            this.labelIntergralStanley.Text = "Integral";
            this.labelIntergralStanley.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelIntergralStanley.UseCompatibleTextRendering = true;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.Chocolate;
            this.label25.Location = new System.Drawing.Point(70, 3);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(201, 31);
            this.label25.TabIndex = 347;
            this.label25.Text = "Stanley Gains";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeadingErrorGain
            // 
            this.lblHeadingErrorGain.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeadingErrorGain.ForeColor = System.Drawing.Color.Black;
            this.lblHeadingErrorGain.Location = new System.Drawing.Point(9, 166);
            this.lblHeadingErrorGain.Name = "lblHeadingErrorGain";
            this.lblHeadingErrorGain.Size = new System.Drawing.Size(54, 35);
            this.lblHeadingErrorGain.TabIndex = 295;
            this.lblHeadingErrorGain.Text = "888";
            this.lblHeadingErrorGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStanleyGain
            // 
            this.lblStanleyGain.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStanleyGain.ForeColor = System.Drawing.Color.Black;
            this.lblStanleyGain.Location = new System.Drawing.Point(9, 78);
            this.lblStanleyGain.Name = "lblStanleyGain";
            this.lblStanleyGain.Size = new System.Drawing.Size(54, 35);
            this.lblStanleyGain.TabIndex = 299;
            this.lblStanleyGain.Text = "888";
            this.lblStanleyGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHeading
            // 
            this.labelHeading.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeading.ForeColor = System.Drawing.Color.Black;
            this.labelHeading.Location = new System.Drawing.Point(62, 140);
            this.labelHeading.Name = "labelHeading";
            this.labelHeading.Size = new System.Drawing.Size(204, 30);
            this.labelHeading.TabIndex = 296;
            this.labelHeading.Text = "Heading";
            this.labelHeading.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDistance
            // 
            this.labelDistance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDistance.ForeColor = System.Drawing.Color.Black;
            this.labelDistance.Location = new System.Drawing.Point(63, 51);
            this.labelDistance.Name = "labelDistance";
            this.labelDistance.Size = new System.Drawing.Size(204, 28);
            this.labelDistance.TabIndex = 298;
            this.labelDistance.Text = "Distance";
            this.labelDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarStanleyGain
            // 
            this.hsbarStanleyGain.LargeChange = 1;
            this.hsbarStanleyGain.Location = new System.Drawing.Point(68, 80);
            this.hsbarStanleyGain.Maximum = 40;
            this.hsbarStanleyGain.Minimum = 1;
            this.hsbarStanleyGain.Name = "hsbarStanleyGain";
            this.hsbarStanleyGain.Size = new System.Drawing.Size(200, 30);
            this.hsbarStanleyGain.TabIndex = 297;
            this.hsbarStanleyGain.Value = 10;
            this.hsbarStanleyGain.ValueChanged += new System.EventHandler(this.hsbarStanleyGain_ValueChanged);
            // 
            // hsbarHeadingErrorGain
            // 
            this.hsbarHeadingErrorGain.LargeChange = 1;
            this.hsbarHeadingErrorGain.Location = new System.Drawing.Point(68, 170);
            this.hsbarHeadingErrorGain.Maximum = 15;
            this.hsbarHeadingErrorGain.Minimum = 1;
            this.hsbarHeadingErrorGain.Name = "hsbarHeadingErrorGain";
            this.hsbarHeadingErrorGain.Size = new System.Drawing.Size(200, 30);
            this.hsbarHeadingErrorGain.TabIndex = 294;
            this.hsbarHeadingErrorGain.Value = 10;
            this.hsbarHeadingErrorGain.ValueChanged += new System.EventHandler(this.hsbarHeadingErrorGain_ValueChanged);
            // 
            // tabSteer
            // 
            this.tabSteer.AutoScroll = true;
            this.tabSteer.BackColor = System.Drawing.Color.Gainsboro;
            this.tabSteer.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_SteerTab;
            this.tabSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabSteer.Controls.Add(this.label80);
            this.tabSteer.Controls.Add(this.label79);
            this.tabSteer.Controls.Add(this.label78);
            this.tabSteer.Controls.Add(this.labelMaxSteerAngle);
            this.tabSteer.Controls.Add(this.lblAV_Set);
            this.tabSteer.Controls.Add(this.lblAV_Act);
            this.tabSteer.Controls.Add(this.lblMaxSteerAngle);
            this.tabSteer.Controls.Add(this.label36);
            this.tabSteer.Controls.Add(this.labelAckermann);
            this.tabSteer.Controls.Add(this.label38);
            this.tabSteer.Controls.Add(this.labelCountsPerDegree);
            this.tabSteer.Controls.Add(this.hsbarAckerman);
            this.tabSteer.Controls.Add(this.hsbarMaxSteerAngle);
            this.tabSteer.Controls.Add(this.lblAckerman);
            this.tabSteer.Controls.Add(this.pbarRight);
            this.tabSteer.Controls.Add(this.pbarLeft);
            this.tabSteer.Controls.Add(this.lblActualSteerAngleUpper);
            this.tabSteer.Controls.Add(this.btnZeroWAS);
            this.tabSteer.Controls.Add(this.hsbarCountsPerDegree);
            this.tabSteer.Controls.Add(this.labelWasZero);
            this.tabSteer.Controls.Add(this.lblCountsPerDegree);
            this.tabSteer.Controls.Add(this.hsbarWasOffset);
            this.tabSteer.Controls.Add(this.lblSteerAngleSensorZero);
            this.tabSteer.Controls.Add(this.label81);
            this.tabSteer.ImageIndex = 0;
            this.tabSteer.Location = new System.Drawing.Point(4, 52);
            this.tabSteer.Name = "tabSteer";
            this.tabSteer.Size = new System.Drawing.Size(365, 346);
            this.tabSteer.TabIndex = 5;
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.BackColor = System.Drawing.Color.Transparent;
            this.label80.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label80.ForeColor = System.Drawing.Color.Black;
            this.label80.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label80.Location = new System.Drawing.Point(66, 252);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(28, 16);
            this.label80.TabIndex = 547;
            this.label80.Text = "100";
            this.label80.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.BackColor = System.Drawing.Color.Transparent;
            this.label79.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label79.ForeColor = System.Drawing.Color.Black;
            this.label79.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label79.Location = new System.Drawing.Point(66, 183);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(28, 16);
            this.label79.TabIndex = 546;
            this.label79.Text = "110";
            this.label79.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.BackColor = System.Drawing.Color.Transparent;
            this.label78.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label78.ForeColor = System.Drawing.Color.Black;
            this.label78.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label78.Location = new System.Drawing.Point(78, 110);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(25, 16);
            this.label78.TabIndex = 545;
            this.label78.Text = "0.0";
            this.label78.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelMaxSteerAngle
            // 
            this.labelMaxSteerAngle.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxSteerAngle.ForeColor = System.Drawing.Color.Black;
            this.labelMaxSteerAngle.Location = new System.Drawing.Point(65, 268);
            this.labelMaxSteerAngle.Name = "labelMaxSteerAngle";
            this.labelMaxSteerAngle.Size = new System.Drawing.Size(200, 19);
            this.labelMaxSteerAngle.TabIndex = 341;
            this.labelMaxSteerAngle.Text = "Max Steer Angle";
            this.labelMaxSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAV_Set
            // 
            this.lblAV_Set.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAV_Set.AutoSize = true;
            this.lblAV_Set.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAV_Set.Location = new System.Drawing.Point(-328, 18);
            this.lblAV_Set.Name = "lblAV_Set";
            this.lblAV_Set.Size = new System.Drawing.Size(51, 19);
            this.lblAV_Set.TabIndex = 529;
            this.lblAV_Set.Text = "-55.8";
            this.lblAV_Set.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblAV_Act
            // 
            this.lblAV_Act.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAV_Act.AutoSize = true;
            this.lblAV_Act.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAV_Act.Location = new System.Drawing.Point(-328, 42);
            this.lblAV_Act.Name = "lblAV_Act";
            this.lblAV_Act.Size = new System.Drawing.Size(54, 19);
            this.lblAV_Act.TabIndex = 528;
            this.lblAV_Act.Text = "66.89";
            this.lblAV_Act.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMaxSteerAngle
            // 
            this.lblMaxSteerAngle.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxSteerAngle.ForeColor = System.Drawing.Color.Black;
            this.lblMaxSteerAngle.Location = new System.Drawing.Point(11, 291);
            this.lblMaxSteerAngle.Name = "lblMaxSteerAngle";
            this.lblMaxSteerAngle.Size = new System.Drawing.Size(52, 35);
            this.lblMaxSteerAngle.TabIndex = 303;
            this.lblMaxSteerAngle.Text = "888";
            this.lblMaxSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label36.AutoSize = true;
            this.label36.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.Location = new System.Drawing.Point(-372, 44);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(50, 16);
            this.label36.TabIndex = 530;
            this.label36.Text = "AV Act:";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelAckermann
            // 
            this.labelAckermann.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAckermann.ForeColor = System.Drawing.Color.Black;
            this.labelAckermann.Location = new System.Drawing.Point(65, 199);
            this.labelAckermann.Name = "labelAckermann";
            this.labelAckermann.Size = new System.Drawing.Size(200, 19);
            this.labelAckermann.TabIndex = 335;
            this.labelAckermann.Text = "Ackermann";
            this.labelAckermann.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(-373, 20);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(51, 16);
            this.label38.TabIndex = 531;
            this.label38.Text = "AV Set:";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelCountsPerDegree
            // 
            this.labelCountsPerDegree.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCountsPerDegree.ForeColor = System.Drawing.Color.Black;
            this.labelCountsPerDegree.Location = new System.Drawing.Point(77, 126);
            this.labelCountsPerDegree.Name = "labelCountsPerDegree";
            this.labelCountsPerDegree.Size = new System.Drawing.Size(188, 19);
            this.labelCountsPerDegree.TabIndex = 334;
            this.labelCountsPerDegree.Text = "Counts per Degree";
            this.labelCountsPerDegree.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarAckerman
            // 
            this.hsbarAckerman.LargeChange = 1;
            this.hsbarAckerman.Location = new System.Drawing.Point(65, 220);
            this.hsbarAckerman.Maximum = 200;
            this.hsbarAckerman.Minimum = 1;
            this.hsbarAckerman.Name = "hsbarAckerman";
            this.hsbarAckerman.Size = new System.Drawing.Size(200, 30);
            this.hsbarAckerman.TabIndex = 331;
            this.hsbarAckerman.Value = 100;
            this.hsbarAckerman.ValueChanged += new System.EventHandler(this.hsbarAckerman_ValueChanged);
            // 
            // hsbarMaxSteerAngle
            // 
            this.hsbarMaxSteerAngle.LargeChange = 1;
            this.hsbarMaxSteerAngle.Location = new System.Drawing.Point(65, 294);
            this.hsbarMaxSteerAngle.Maximum = 80;
            this.hsbarMaxSteerAngle.Minimum = 10;
            this.hsbarMaxSteerAngle.Name = "hsbarMaxSteerAngle";
            this.hsbarMaxSteerAngle.Size = new System.Drawing.Size(200, 30);
            this.hsbarMaxSteerAngle.TabIndex = 299;
            this.hsbarMaxSteerAngle.Value = 10;
            this.hsbarMaxSteerAngle.ValueChanged += new System.EventHandler(this.hsbarMaxSteerAngle_ValueChanged);
            // 
            // lblAckerman
            // 
            this.lblAckerman.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAckerman.ForeColor = System.Drawing.Color.Black;
            this.lblAckerman.Location = new System.Drawing.Point(9, 218);
            this.lblAckerman.Name = "lblAckerman";
            this.lblAckerman.Size = new System.Drawing.Size(55, 35);
            this.lblAckerman.TabIndex = 333;
            this.lblAckerman.Text = "888";
            this.lblAckerman.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbarRight
            // 
            this.pbarRight.Location = new System.Drawing.Point(171, 2);
            this.pbarRight.Maximum = 50;
            this.pbarRight.Name = "pbarRight";
            this.pbarRight.Size = new System.Drawing.Size(159, 10);
            this.pbarRight.TabIndex = 330;
            // 
            // pbarLeft
            // 
            this.pbarLeft.Location = new System.Drawing.Point(11, 2);
            this.pbarLeft.Maximum = 50;
            this.pbarLeft.Name = "pbarLeft";
            this.pbarLeft.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pbarLeft.RightToLeftLayout = true;
            this.pbarLeft.Size = new System.Drawing.Size(159, 10);
            this.pbarLeft.TabIndex = 329;
            // 
            // lblActualSteerAngleUpper
            // 
            this.lblActualSteerAngleUpper.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblActualSteerAngleUpper.AutoSize = true;
            this.lblActualSteerAngleUpper.BackColor = System.Drawing.Color.Transparent;
            this.lblActualSteerAngleUpper.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblActualSteerAngleUpper.ForeColor = System.Drawing.Color.Black;
            this.lblActualSteerAngleUpper.Location = new System.Drawing.Point(-567, 16);
            this.lblActualSteerAngleUpper.Name = "lblActualSteerAngleUpper";
            this.lblActualSteerAngleUpper.Size = new System.Drawing.Size(39, 19);
            this.lblActualSteerAngleUpper.TabIndex = 324;
            this.lblActualSteerAngleUpper.Text = "255";
            this.lblActualSteerAngleUpper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnZeroWAS
            // 
            this.btnZeroWAS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZeroWAS.FlatAppearance.BorderSize = 0;
            this.btnZeroWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroWAS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroWAS.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZeroWAS.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnZeroWAS.Location = new System.Drawing.Point(134, 19);
            this.btnZeroWAS.Name = "btnZeroWAS";
            this.btnZeroWAS.Size = new System.Drawing.Size(70, 30);
            this.btnZeroWAS.TabIndex = 323;
            this.btnZeroWAS.UseVisualStyleBackColor = true;
            this.btnZeroWAS.Click += new System.EventHandler(this.btnZeroWAS_Click);
            // 
            // hsbarCountsPerDegree
            // 
            this.hsbarCountsPerDegree.LargeChange = 1;
            this.hsbarCountsPerDegree.Location = new System.Drawing.Point(65, 152);
            this.hsbarCountsPerDegree.Maximum = 255;
            this.hsbarCountsPerDegree.Minimum = 1;
            this.hsbarCountsPerDegree.Name = "hsbarCountsPerDegree";
            this.hsbarCountsPerDegree.Size = new System.Drawing.Size(200, 30);
            this.hsbarCountsPerDegree.TabIndex = 304;
            this.hsbarCountsPerDegree.Value = 20;
            this.hsbarCountsPerDegree.ValueChanged += new System.EventHandler(this.hsbarCountsPerDegree_ValueChanged);
            // 
            // labelWasZero
            // 
            this.labelWasZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.labelWasZero.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWasZero.ForeColor = System.Drawing.Color.Black;
            this.labelWasZero.Location = new System.Drawing.Point(77, 52);
            this.labelWasZero.Name = "labelWasZero";
            this.labelWasZero.Size = new System.Drawing.Size(188, 19);
            this.labelWasZero.TabIndex = 295;
            this.labelWasZero.Text = "WAS Zero";
            this.labelWasZero.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCountsPerDegree
            // 
            this.lblCountsPerDegree.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountsPerDegree.ForeColor = System.Drawing.Color.Black;
            this.lblCountsPerDegree.Location = new System.Drawing.Point(9, 147);
            this.lblCountsPerDegree.Name = "lblCountsPerDegree";
            this.lblCountsPerDegree.Size = new System.Drawing.Size(55, 35);
            this.lblCountsPerDegree.TabIndex = 308;
            this.lblCountsPerDegree.Text = "888";
            this.lblCountsPerDegree.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarWasOffset
            // 
            this.hsbarWasOffset.LargeChange = 20;
            this.hsbarWasOffset.Location = new System.Drawing.Point(77, 80);
            this.hsbarWasOffset.Maximum = 4000;
            this.hsbarWasOffset.Minimum = -4000;
            this.hsbarWasOffset.Name = "hsbarWasOffset";
            this.hsbarWasOffset.Size = new System.Drawing.Size(188, 30);
            this.hsbarWasOffset.SmallChange = 2;
            this.hsbarWasOffset.TabIndex = 294;
            this.hsbarWasOffset.ValueChanged += new System.EventHandler(this.hsbarSteerAngleSensorZero_ValueChanged);
            // 
            // lblSteerAngleSensorZero
            // 
            this.lblSteerAngleSensorZero.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleSensorZero.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngleSensorZero.Location = new System.Drawing.Point(9, 75);
            this.lblSteerAngleSensorZero.Name = "lblSteerAngleSensorZero";
            this.lblSteerAngleSensorZero.Size = new System.Drawing.Size(70, 35);
            this.lblSteerAngleSensorZero.TabIndex = 298;
            this.lblSteerAngleSensorZero.Text = "-55.88";
            this.lblSteerAngleSensorZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label81
            // 
            this.label81.AutoSize = true;
            this.label81.BackColor = System.Drawing.Color.Transparent;
            this.label81.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label81.ForeColor = System.Drawing.Color.Black;
            this.label81.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label81.Location = new System.Drawing.Point(66, 325);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(21, 16);
            this.label81.TabIndex = 548;
            this.label81.Text = "30";
            this.label81.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tabPPAdv
            // 
            this.tabPPAdv.BackColor = System.Drawing.Color.Gainsboro;
            this.tabPPAdv.Controls.Add(this.labelAquireFactor);
            this.tabPPAdv.Controls.Add(this.label67);
            this.tabPPAdv.Controls.Add(this.label66);
            this.tabPPAdv.Controls.Add(this.label65);
            this.tabPPAdv.Controls.Add(this.label59);
            this.tabPPAdv.Controls.Add(this.lblDistanceAdv);
            this.tabPPAdv.Controls.Add(this.hsbarLookAheadMult);
            this.tabPPAdv.Controls.Add(this.labelDist);
            this.tabPPAdv.Controls.Add(this.lblLookAheadMult);
            this.tabPPAdv.Controls.Add(this.lblHoldAdv);
            this.tabPPAdv.Controls.Add(this.labelSpeedFactor);
            this.tabPPAdv.Controls.Add(this.lblAcqAdv);
            this.tabPPAdv.Controls.Add(this.labelDeadzone);
            this.tabPPAdv.Controls.Add(this.hsbarAcquireFactor);
            this.tabPPAdv.Controls.Add(this.labelAquire2);
            this.tabPPAdv.Controls.Add(this.lblAcquireFactor);
            this.tabPPAdv.Controls.Add(this.labelHold);
            this.tabPPAdv.Controls.Add(this.labelAquireDescription);
            this.tabPPAdv.Controls.Add(this.labelOnDelay);
            this.tabPPAdv.Controls.Add(this.labelHeadingDegree);
            this.tabPPAdv.Controls.Add(this.nudDeadZoneDelay);
            this.tabPPAdv.Controls.Add(this.nudDeadZoneHeading);
            this.tabPPAdv.ImageIndex = 4;
            this.tabPPAdv.Location = new System.Drawing.Point(4, 52);
            this.tabPPAdv.Name = "tabPPAdv";
            this.tabPPAdv.Padding = new System.Windows.Forms.Padding(3);
            this.tabPPAdv.Size = new System.Drawing.Size(365, 346);
            this.tabPPAdv.TabIndex = 17;
            // 
            // labelAquireFactor
            // 
            this.labelAquireFactor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAquireFactor.ForeColor = System.Drawing.Color.Black;
            this.labelAquireFactor.Location = new System.Drawing.Point(42, 199);
            this.labelAquireFactor.Name = "labelAquireFactor";
            this.labelAquireFactor.Size = new System.Drawing.Size(307, 26);
            this.labelAquireFactor.TabIndex = 548;
            this.labelAquireFactor.Text = "Acquire Factor";
            this.labelAquireFactor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.BackColor = System.Drawing.Color.Transparent;
            this.label67.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.ForeColor = System.Drawing.Color.Black;
            this.label67.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label67.Location = new System.Drawing.Point(310, 48);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(14, 16);
            this.label67.TabIndex = 547;
            this.label67.Text = "5";
            this.label67.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.ForeColor = System.Drawing.Color.Black;
            this.label66.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label66.Location = new System.Drawing.Point(31, 48);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(25, 16);
            this.label66.TabIndex = 546;
            this.label66.Text = "0.1";
            this.label66.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.BackColor = System.Drawing.Color.Transparent;
            this.label65.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label65.ForeColor = System.Drawing.Color.Black;
            this.label65.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label65.Location = new System.Drawing.Point(73, 269);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(25, 16);
            this.label65.TabIndex = 545;
            this.label65.Text = "0.9";
            this.label65.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label59
            // 
            this.label59.AutoSize = true;
            this.label59.BackColor = System.Drawing.Color.Transparent;
            this.label59.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.Color.Black;
            this.label59.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label59.Location = new System.Drawing.Point(73, 177);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(25, 16);
            this.label59.TabIndex = 544;
            this.label59.Text = "1.5";
            this.label59.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblDistanceAdv
            // 
            this.lblDistanceAdv.AutoSize = true;
            this.lblDistanceAdv.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistanceAdv.ForeColor = System.Drawing.Color.Black;
            this.lblDistanceAdv.Location = new System.Drawing.Point(91, 312);
            this.lblDistanceAdv.Name = "lblDistanceAdv";
            this.lblDistanceAdv.Size = new System.Drawing.Size(46, 23);
            this.lblDistanceAdv.TabIndex = 516;
            this.lblDistanceAdv.Text = "888";
            this.lblDistanceAdv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarLookAheadMult
            // 
            this.hsbarLookAheadMult.LargeChange = 1;
            this.hsbarLookAheadMult.Location = new System.Drawing.Point(73, 143);
            this.hsbarLookAheadMult.Maximum = 60;
            this.hsbarLookAheadMult.Minimum = 5;
            this.hsbarLookAheadMult.Name = "hsbarLookAheadMult";
            this.hsbarLookAheadMult.Size = new System.Drawing.Size(276, 30);
            this.hsbarLookAheadMult.TabIndex = 298;
            this.hsbarLookAheadMult.Value = 6;
            this.hsbarLookAheadMult.ValueChanged += new System.EventHandler(this.hsbarLookAheadMult_ValueChanged);
            // 
            // labelDist
            // 
            this.labelDist.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDist.ForeColor = System.Drawing.Color.Black;
            this.labelDist.Location = new System.Drawing.Point(3, 311);
            this.labelDist.Name = "labelDist";
            this.labelDist.Size = new System.Drawing.Size(88, 23);
            this.labelDist.TabIndex = 515;
            this.labelDist.Text = "Distance:";
            this.labelDist.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLookAheadMult
            // 
            this.lblLookAheadMult.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAheadMult.ForeColor = System.Drawing.Color.Black;
            this.lblLookAheadMult.Location = new System.Drawing.Point(8, 142);
            this.lblLookAheadMult.Name = "lblLookAheadMult";
            this.lblLookAheadMult.Size = new System.Drawing.Size(60, 35);
            this.lblLookAheadMult.TabIndex = 299;
            this.lblLookAheadMult.Text = "888";
            this.lblLookAheadMult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHoldAdv
            // 
            this.lblHoldAdv.AutoSize = true;
            this.lblHoldAdv.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHoldAdv.ForeColor = System.Drawing.Color.Black;
            this.lblHoldAdv.Location = new System.Drawing.Point(315, 312);
            this.lblHoldAdv.Name = "lblHoldAdv";
            this.lblHoldAdv.Size = new System.Drawing.Size(46, 23);
            this.lblHoldAdv.TabIndex = 514;
            this.lblHoldAdv.Text = "888";
            this.lblHoldAdv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelSpeedFactor
            // 
            this.labelSpeedFactor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSpeedFactor.ForeColor = System.Drawing.Color.Black;
            this.labelSpeedFactor.Location = new System.Drawing.Point(38, 116);
            this.labelSpeedFactor.Name = "labelSpeedFactor";
            this.labelSpeedFactor.Size = new System.Drawing.Size(311, 26);
            this.labelSpeedFactor.TabIndex = 301;
            this.labelSpeedFactor.Text = "Speed Factor";
            this.labelSpeedFactor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAcqAdv
            // 
            this.lblAcqAdv.AutoSize = true;
            this.lblAcqAdv.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcqAdv.ForeColor = System.Drawing.Color.Black;
            this.lblAcqAdv.Location = new System.Drawing.Point(216, 312);
            this.lblAcqAdv.Name = "lblAcqAdv";
            this.lblAcqAdv.Size = new System.Drawing.Size(46, 23);
            this.lblAcqAdv.TabIndex = 513;
            this.lblAcqAdv.Text = "888";
            this.lblAcqAdv.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelDeadzone
            // 
            this.labelDeadzone.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDeadzone.ForeColor = System.Drawing.Color.Black;
            this.labelDeadzone.Location = new System.Drawing.Point(7, 9);
            this.labelDeadzone.Name = "labelDeadzone";
            this.labelDeadzone.Size = new System.Drawing.Size(352, 25);
            this.labelDeadzone.TabIndex = 541;
            this.labelDeadzone.Text = " ------ Dead Zone -----";
            this.labelDeadzone.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarAcquireFactor
            // 
            this.hsbarAcquireFactor.LargeChange = 1;
            this.hsbarAcquireFactor.Location = new System.Drawing.Point(73, 235);
            this.hsbarAcquireFactor.Maximum = 300;
            this.hsbarAcquireFactor.Minimum = 20;
            this.hsbarAcquireFactor.Name = "hsbarAcquireFactor";
            this.hsbarAcquireFactor.Size = new System.Drawing.Size(276, 30);
            this.hsbarAcquireFactor.TabIndex = 508;
            this.hsbarAcquireFactor.Value = 75;
            this.hsbarAcquireFactor.ValueChanged += new System.EventHandler(this.hsbarAcquireFactor_ValueChanged);
            // 
            // labelAquire2
            // 
            this.labelAquire2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAquire2.ForeColor = System.Drawing.Color.Black;
            this.labelAquire2.Location = new System.Drawing.Point(139, 312);
            this.labelAquire2.Name = "labelAquire2";
            this.labelAquire2.Size = new System.Drawing.Size(80, 23);
            this.labelAquire2.TabIndex = 512;
            this.labelAquire2.Text = "Acquire:";
            this.labelAquire2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAcquireFactor
            // 
            this.lblAcquireFactor.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAcquireFactor.ForeColor = System.Drawing.Color.Black;
            this.lblAcquireFactor.Location = new System.Drawing.Point(8, 232);
            this.lblAcquireFactor.Name = "lblAcquireFactor";
            this.lblAcquireFactor.Size = new System.Drawing.Size(60, 35);
            this.lblAcquireFactor.TabIndex = 509;
            this.lblAcquireFactor.Text = "888";
            this.lblAcquireFactor.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelHold
            // 
            this.labelHold.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHold.ForeColor = System.Drawing.Color.Black;
            this.labelHold.Location = new System.Drawing.Point(265, 311);
            this.labelHold.Name = "labelHold";
            this.labelHold.Size = new System.Drawing.Size(55, 23);
            this.labelHold.TabIndex = 511;
            this.labelHold.Text = "Hold:";
            this.labelHold.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelAquireDescription
            // 
            this.labelAquireDescription.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAquireDescription.ForeColor = System.Drawing.Color.Black;
            this.labelAquireDescription.Location = new System.Drawing.Point(103, 269);
            this.labelAquireDescription.Name = "labelAquireDescription";
            this.labelAquireDescription.Size = new System.Drawing.Size(246, 23);
            this.labelAquireDescription.TabIndex = 510;
            this.labelAquireDescription.Text = "Acquire = Factor * Hold";
            this.labelAquireDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOnDelay
            // 
            this.labelOnDelay.BackColor = System.Drawing.Color.Transparent;
            this.labelOnDelay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOnDelay.ForeColor = System.Drawing.Color.Black;
            this.labelOnDelay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelOnDelay.Location = new System.Drawing.Point(190, 72);
            this.labelOnDelay.Name = "labelOnDelay";
            this.labelOnDelay.Size = new System.Drawing.Size(172, 22);
            this.labelOnDelay.TabIndex = 543;
            this.labelOnDelay.Text = "On Delay (sec)";
            this.labelOnDelay.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // labelHeadingDegree
            // 
            this.labelHeadingDegree.BackColor = System.Drawing.Color.Transparent;
            this.labelHeadingDegree.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelHeadingDegree.ForeColor = System.Drawing.Color.Black;
            this.labelHeadingDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelHeadingDegree.Location = new System.Drawing.Point(5, 72);
            this.labelHeadingDegree.Name = "labelHeadingDegree";
            this.labelHeadingDegree.Size = new System.Drawing.Size(179, 22);
            this.labelHeadingDegree.TabIndex = 539;
            this.labelHeadingDegree.Text = "Heading (Degree)";
            this.labelHeadingDegree.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // nudDeadZoneDelay
            // 
            this.nudDeadZoneDelay.BackColor = System.Drawing.Color.White;
            this.nudDeadZoneDelay.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDeadZoneDelay.InterceptArrowKeys = false;
            this.nudDeadZoneDelay.Location = new System.Drawing.Point(198, 37);
            this.nudDeadZoneDelay.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudDeadZoneDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDeadZoneDelay.Name = "nudDeadZoneDelay";
            this.nudDeadZoneDelay.ReadOnly = true;
            this.nudDeadZoneDelay.Size = new System.Drawing.Size(107, 36);
            this.nudDeadZoneDelay.TabIndex = 542;
            this.nudDeadZoneDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDeadZoneDelay.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDeadZoneDelay.Click += new System.EventHandler(this.nudDeadZoneDelay_Click);
            // 
            // nudDeadZoneHeading
            // 
            this.nudDeadZoneHeading.BackColor = System.Drawing.Color.White;
            this.nudDeadZoneHeading.DecimalPlaces = 1;
            this.nudDeadZoneHeading.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDeadZoneHeading.InterceptArrowKeys = false;
            this.nudDeadZoneHeading.Location = new System.Drawing.Point(60, 38);
            this.nudDeadZoneHeading.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudDeadZoneHeading.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudDeadZoneHeading.Name = "nudDeadZoneHeading";
            this.nudDeadZoneHeading.ReadOnly = true;
            this.nudDeadZoneHeading.Size = new System.Drawing.Size(107, 36);
            this.nudDeadZoneHeading.TabIndex = 538;
            this.nudDeadZoneHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDeadZoneHeading.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDeadZoneHeading.Click += new System.EventHandler(this.nudDeadZoneHeading_Click);
            // 
            // tabGain
            // 
            this.tabGain.AutoScroll = true;
            this.tabGain.BackColor = System.Drawing.Color.Gainsboro;
            this.tabGain.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_GainTab;
            this.tabGain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabGain.Controls.Add(this.label75);
            this.tabGain.Controls.Add(this.label76);
            this.tabGain.Controls.Add(this.label77);
            this.tabGain.Controls.Add(this.label6);
            this.tabGain.Controls.Add(this.labelMinToMove);
            this.tabGain.Controls.Add(this.labelMaxLimit);
            this.tabGain.Controls.Add(this.labelProportionalGain);
            this.tabGain.Controls.Add(this.hsbarMinPWM);
            this.tabGain.Controls.Add(this.hsbarProportionalGain);
            this.tabGain.Controls.Add(this.lblProportionalGain);
            this.tabGain.Controls.Add(this.lblHighSteerPWM);
            this.tabGain.Controls.Add(this.lblMinPWM);
            this.tabGain.Controls.Add(this.hsbarHighSteerPWM);
            this.tabGain.ImageIndex = 1;
            this.tabGain.Location = new System.Drawing.Point(4, 52);
            this.tabGain.Name = "tabGain";
            this.tabGain.Size = new System.Drawing.Size(365, 346);
            this.tabGain.TabIndex = 13;
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.BackColor = System.Drawing.Color.Transparent;
            this.label75.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label75.ForeColor = System.Drawing.Color.Black;
            this.label75.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label75.Location = new System.Drawing.Point(68, 302);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(21, 16);
            this.label75.TabIndex = 552;
            this.label75.Text = "25";
            this.label75.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.BackColor = System.Drawing.Color.Transparent;
            this.label76.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label76.ForeColor = System.Drawing.Color.Black;
            this.label76.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label76.Location = new System.Drawing.Point(68, 212);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(28, 16);
            this.label76.TabIndex = 551;
            this.label76.Text = "180";
            this.label76.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.BackColor = System.Drawing.Color.Transparent;
            this.label77.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label77.ForeColor = System.Drawing.Color.Black;
            this.label77.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label77.Location = new System.Drawing.Point(68, 122);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(21, 16);
            this.label77.TabIndex = 550;
            this.label77.Text = "50";
            this.label77.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(3, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(360, 25);
            this.label6.TabIndex = 339;
            this.label6.Text = "Gain";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMinToMove
            // 
            this.labelMinToMove.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinToMove.ForeColor = System.Drawing.Color.Black;
            this.labelMinToMove.Location = new System.Drawing.Point(68, 246);
            this.labelMinToMove.Name = "labelMinToMove";
            this.labelMinToMove.Size = new System.Drawing.Size(202, 19);
            this.labelMinToMove.TabIndex = 338;
            this.labelMinToMove.Text = "Minimum to Move";
            this.labelMinToMove.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelMaxLimit
            // 
            this.labelMaxLimit.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxLimit.ForeColor = System.Drawing.Color.Black;
            this.labelMaxLimit.Location = new System.Drawing.Point(71, 156);
            this.labelMaxLimit.Name = "labelMaxLimit";
            this.labelMaxLimit.Size = new System.Drawing.Size(199, 19);
            this.labelMaxLimit.TabIndex = 336;
            this.labelMaxLimit.Text = "Maximum Limit";
            this.labelMaxLimit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelProportionalGain
            // 
            this.labelProportionalGain.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProportionalGain.ForeColor = System.Drawing.Color.Black;
            this.labelProportionalGain.Location = new System.Drawing.Point(68, 66);
            this.labelProportionalGain.Name = "labelProportionalGain";
            this.labelProportionalGain.Size = new System.Drawing.Size(202, 19);
            this.labelProportionalGain.TabIndex = 335;
            this.labelProportionalGain.Text = "Proportional Gain";
            this.labelProportionalGain.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarMinPWM
            // 
            this.hsbarMinPWM.LargeChange = 1;
            this.hsbarMinPWM.Location = new System.Drawing.Point(68, 269);
            this.hsbarMinPWM.Maximum = 200;
            this.hsbarMinPWM.Name = "hsbarMinPWM";
            this.hsbarMinPWM.Size = new System.Drawing.Size(202, 30);
            this.hsbarMinPWM.TabIndex = 284;
            this.hsbarMinPWM.Value = 10;
            this.hsbarMinPWM.ValueChanged += new System.EventHandler(this.hsbarMinPWM_ValueChanged);
            // 
            // hsbarProportionalGain
            // 
            this.hsbarProportionalGain.LargeChange = 1;
            this.hsbarProportionalGain.Location = new System.Drawing.Point(68, 89);
            this.hsbarProportionalGain.Maximum = 200;
            this.hsbarProportionalGain.Minimum = 1;
            this.hsbarProportionalGain.Name = "hsbarProportionalGain";
            this.hsbarProportionalGain.Size = new System.Drawing.Size(202, 30);
            this.hsbarProportionalGain.TabIndex = 254;
            this.hsbarProportionalGain.Value = 4;
            this.hsbarProportionalGain.ValueChanged += new System.EventHandler(this.hsbarProportionalGain_ValueChanged);
            // 
            // lblProportionalGain
            // 
            this.lblProportionalGain.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProportionalGain.ForeColor = System.Drawing.Color.Black;
            this.lblProportionalGain.Location = new System.Drawing.Point(8, 87);
            this.lblProportionalGain.Name = "lblProportionalGain";
            this.lblProportionalGain.Size = new System.Drawing.Size(61, 35);
            this.lblProportionalGain.TabIndex = 258;
            this.lblProportionalGain.Text = "888";
            this.lblProportionalGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHighSteerPWM
            // 
            this.lblHighSteerPWM.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighSteerPWM.ForeColor = System.Drawing.Color.Black;
            this.lblHighSteerPWM.Location = new System.Drawing.Point(8, 176);
            this.lblHighSteerPWM.Name = "lblHighSteerPWM";
            this.lblHighSteerPWM.Size = new System.Drawing.Size(61, 35);
            this.lblHighSteerPWM.TabIndex = 278;
            this.lblHighSteerPWM.Text = "888";
            this.lblHighSteerPWM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMinPWM
            // 
            this.lblMinPWM.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinPWM.ForeColor = System.Drawing.Color.Black;
            this.lblMinPWM.Location = new System.Drawing.Point(8, 266);
            this.lblMinPWM.Name = "lblMinPWM";
            this.lblMinPWM.Size = new System.Drawing.Size(61, 35);
            this.lblMinPWM.TabIndex = 288;
            this.lblMinPWM.Text = "888";
            this.lblMinPWM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarHighSteerPWM
            // 
            this.hsbarHighSteerPWM.LargeChange = 2;
            this.hsbarHighSteerPWM.Location = new System.Drawing.Point(68, 179);
            this.hsbarHighSteerPWM.Maximum = 255;
            this.hsbarHighSteerPWM.Name = "hsbarHighSteerPWM";
            this.hsbarHighSteerPWM.Size = new System.Drawing.Size(202, 30);
            this.hsbarHighSteerPWM.TabIndex = 274;
            this.hsbarHighSteerPWM.Value = 50;
            this.hsbarHighSteerPWM.ValueChanged += new System.EventHandler(this.hsbarHighSteerPWM_ValueChanged);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ST_SteerTab.png");
            this.imageList1.Images.SetKeyName(1, "ST_GainTab.png");
            this.imageList1.Images.SetKeyName(2, "ST_StanleyTab.png");
            this.imageList1.Images.SetKeyName(3, "Sf_PPTab.png");
            this.imageList1.Images.SetKeyName(4, "ST_NerdAdv.png");
            // 
            // lblSideHillComp
            // 
            this.lblSideHillComp.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSideHillComp.ForeColor = System.Drawing.Color.Black;
            this.lblSideHillComp.Location = new System.Drawing.Point(18, 199);
            this.lblSideHillComp.Name = "lblSideHillComp";
            this.lblSideHillComp.Size = new System.Drawing.Size(88, 35);
            this.lblSideHillComp.TabIndex = 353;
            this.lblSideHillComp.Text = "888";
            this.lblSideHillComp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarSideHillComp
            // 
            this.hsbarSideHillComp.LargeChange = 1;
            this.hsbarSideHillComp.Location = new System.Drawing.Point(109, 189);
            this.hsbarSideHillComp.Name = "hsbarSideHillComp";
            this.hsbarSideHillComp.Size = new System.Drawing.Size(339, 53);
            this.hsbarSideHillComp.TabIndex = 352;
            this.hsbarSideHillComp.Value = 5;
            this.hsbarSideHillComp.ValueChanged += new System.EventHandler(this.hsbarSideHillComp_ValueChanged);
            // 
            // labelSideHill
            // 
            this.labelSideHill.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSideHill.ForeColor = System.Drawing.Color.Black;
            this.labelSideHill.Location = new System.Drawing.Point(109, 166);
            this.labelSideHill.Name = "labelSideHill";
            this.labelSideHill.Size = new System.Drawing.Size(339, 19);
            this.labelSideHill.TabIndex = 351;
            this.labelSideHill.Text = "Sidehill Deg Turn per Deg of Roll";
            this.labelSideHill.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelSideHill.UseCompatibleTextRendering = true;
            // 
            // labelSteerDescription
            // 
            this.labelSteerDescription.AutoSize = true;
            this.labelSteerDescription.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSteerDescription.ForeColor = System.Drawing.Color.Black;
            this.labelSteerDescription.Location = new System.Drawing.Point(317, 383);
            this.labelSteerDescription.Name = "labelSteerDescription";
            this.labelSteerDescription.Size = new System.Drawing.Size(193, 32);
            this.labelSteerDescription.TabIndex = 513;
            this.labelSteerDescription.Text = "Button - Push On. Push Off\r\nSwitch - Pushed On, Release Off";
            // 
            // labelPressureTurnSensor
            // 
            this.labelPressureTurnSensor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPressureTurnSensor.ForeColor = System.Drawing.Color.Black;
            this.labelPressureTurnSensor.Location = new System.Drawing.Point(0, 137);
            this.labelPressureTurnSensor.Name = "labelPressureTurnSensor";
            this.labelPressureTurnSensor.Size = new System.Drawing.Size(174, 19);
            this.labelPressureTurnSensor.TabIndex = 512;
            this.labelPressureTurnSensor.Text = "Pressure Sensor";
            this.labelPressureTurnSensor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelCurrentTurnSensor
            // 
            this.labelCurrentTurnSensor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCurrentTurnSensor.ForeColor = System.Drawing.Color.Black;
            this.labelCurrentTurnSensor.Location = new System.Drawing.Point(3, 273);
            this.labelCurrentTurnSensor.Name = "labelCurrentTurnSensor";
            this.labelCurrentTurnSensor.Size = new System.Drawing.Size(171, 19);
            this.labelCurrentTurnSensor.TabIndex = 511;
            this.labelCurrentTurnSensor.Text = "Current Sensor";
            this.labelCurrentTurnSensor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelEncoder
            // 
            this.labelEncoder.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelEncoder.ForeColor = System.Drawing.Color.Black;
            this.labelEncoder.Location = new System.Drawing.Point(0, 10);
            this.labelEncoder.Name = "labelEncoder";
            this.labelEncoder.Size = new System.Drawing.Size(174, 19);
            this.labelEncoder.TabIndex = 506;
            this.labelEncoder.Text = "Count Sensor";
            this.labelEncoder.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelInvertMotor
            // 
            this.labelInvertMotor.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInvertMotor.ForeColor = System.Drawing.Color.Black;
            this.labelInvertMotor.Location = new System.Drawing.Point(141, 173);
            this.labelInvertMotor.Name = "labelInvertMotor";
            this.labelInvertMotor.Size = new System.Drawing.Size(155, 16);
            this.labelInvertMotor.TabIndex = 505;
            this.labelInvertMotor.Text = "Invert Motor Dir";
            this.labelInvertMotor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelInvertRelays
            // 
            this.labelInvertRelays.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInvertRelays.ForeColor = System.Drawing.Color.Black;
            this.labelInvertRelays.Location = new System.Drawing.Point(141, 307);
            this.labelInvertRelays.Name = "labelInvertRelays";
            this.labelInvertRelays.Size = new System.Drawing.Size(147, 16);
            this.labelInvertRelays.TabIndex = 504;
            this.labelInvertRelays.Text = "Invert Relays";
            this.labelInvertRelays.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // labelSendAndSave
            // 
            this.labelSendAndSave.BackColor = System.Drawing.Color.Transparent;
            this.labelSendAndSave.Font = new System.Drawing.Font("Tahoma", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSendAndSave.ForeColor = System.Drawing.Color.Black;
            this.labelSendAndSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSendAndSave.Location = new System.Drawing.Point(641, 530);
            this.labelSendAndSave.Name = "labelSendAndSave";
            this.labelSendAndSave.Size = new System.Drawing.Size(175, 21);
            this.labelSendAndSave.TabIndex = 502;
            this.labelSendAndSave.Text = "Send + Save";
            this.labelSendAndSave.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboxMotorDrive
            // 
            this.cboxMotorDrive.BackColor = System.Drawing.Color.White;
            this.cboxMotorDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMotorDrive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxMotorDrive.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMotorDrive.FormattingEnabled = true;
            this.cboxMotorDrive.Items.AddRange(new object[] {
            "Cytron",
            "IBT2"});
            this.cboxMotorDrive.Location = new System.Drawing.Point(329, 46);
            this.cboxMotorDrive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxMotorDrive.Name = "cboxMotorDrive";
            this.cboxMotorDrive.Size = new System.Drawing.Size(175, 37);
            this.cboxMotorDrive.TabIndex = 495;
            this.cboxMotorDrive.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // cboxSteerEnable
            // 
            this.cboxSteerEnable.BackColor = System.Drawing.Color.White;
            this.cboxSteerEnable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSteerEnable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxSteerEnable.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSteerEnable.FormattingEnabled = true;
            this.cboxSteerEnable.Items.AddRange(new object[] {
            "None",
            "Switch",
            "Button"});
            this.cboxSteerEnable.Location = new System.Drawing.Point(329, 340);
            this.cboxSteerEnable.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxSteerEnable.Name = "cboxSteerEnable";
            this.cboxSteerEnable.Size = new System.Drawing.Size(175, 37);
            this.cboxSteerEnable.TabIndex = 498;
            this.cboxSteerEnable.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // labelSteerEnable
            // 
            this.labelSteerEnable.BackColor = System.Drawing.Color.Transparent;
            this.labelSteerEnable.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.labelSteerEnable.ForeColor = System.Drawing.Color.Black;
            this.labelSteerEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSteerEnable.Location = new System.Drawing.Point(326, 309);
            this.labelSteerEnable.Name = "labelSteerEnable";
            this.labelSteerEnable.Size = new System.Drawing.Size(181, 29);
            this.labelSteerEnable.TabIndex = 499;
            this.labelSteerEnable.Text = "Steer Enable";
            this.labelSteerEnable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboxConv
            // 
            this.cboxConv.BackColor = System.Drawing.Color.White;
            this.cboxConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxConv.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxConv.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxConv.FormattingEnabled = true;
            this.cboxConv.Items.AddRange(new object[] {
            "Single",
            "Differential"});
            this.cboxConv.Location = new System.Drawing.Point(329, 144);
            this.cboxConv.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxConv.Name = "cboxConv";
            this.cboxConv.Size = new System.Drawing.Size(175, 37);
            this.cboxConv.TabIndex = 500;
            this.cboxConv.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // labelMotorDriver
            // 
            this.labelMotorDriver.BackColor = System.Drawing.Color.Transparent;
            this.labelMotorDriver.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.labelMotorDriver.ForeColor = System.Drawing.Color.Black;
            this.labelMotorDriver.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelMotorDriver.Location = new System.Drawing.Point(326, 16);
            this.labelMotorDriver.Name = "labelMotorDriver";
            this.labelMotorDriver.Size = new System.Drawing.Size(181, 29);
            this.labelMotorDriver.TabIndex = 496;
            this.labelMotorDriver.Text = "Motor Driver";
            this.labelMotorDriver.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelADConverter
            // 
            this.labelADConverter.BackColor = System.Drawing.Color.Transparent;
            this.labelADConverter.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.labelADConverter.ForeColor = System.Drawing.Color.Black;
            this.labelADConverter.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelADConverter.Location = new System.Drawing.Point(326, 112);
            this.labelADConverter.Name = "labelADConverter";
            this.labelADConverter.Size = new System.Drawing.Size(181, 29);
            this.labelADConverter.TabIndex = 497;
            this.labelADConverter.Text = "A2D Convertor";
            this.labelADConverter.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // labelTurnSensor
            // 
            this.labelTurnSensor.BackColor = System.Drawing.Color.Transparent;
            this.labelTurnSensor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTurnSensor.ForeColor = System.Drawing.Color.Black;
            this.labelTurnSensor.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelTurnSensor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelTurnSensor.Location = new System.Drawing.Point(226, 25);
            this.labelTurnSensor.Name = "labelTurnSensor";
            this.labelTurnSensor.Size = new System.Drawing.Size(168, 17);
            this.labelTurnSensor.TabIndex = 494;
            this.labelTurnSensor.Text = "Turn Sensor";
            this.labelTurnSensor.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label55
            // 
            this.label55.BackColor = System.Drawing.Color.Transparent;
            this.label55.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label55.Location = new System.Drawing.Point(6, 37);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(117, 16);
            this.label55.TabIndex = 489;
            this.label55.Text = "Danfoss";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelInvertWas
            // 
            this.labelInvertWas.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelInvertWas.ForeColor = System.Drawing.Color.Black;
            this.labelInvertWas.Location = new System.Drawing.Point(166, 37);
            this.labelInvertWas.Name = "labelInvertWas";
            this.labelInvertWas.Size = new System.Drawing.Size(108, 16);
            this.labelInvertWas.TabIndex = 515;
            this.labelInvertWas.Text = "Invert WAS";
            this.labelInvertWas.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel2.Controls.Add(this.btnExpand);
            this.panel2.Controls.Add(this.lblError);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.lblSteerAngle);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.lblSteerAngleActual);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Location = new System.Drawing.Point(4, 403);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(365, 43);
            this.panel2.TabIndex = 324;
            // 
            // btnExpand
            // 
            this.btnExpand.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExpand.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnExpand.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnExpand.FlatAppearance.BorderSize = 0;
            this.btnExpand.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExpand.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExpand.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnExpand.Location = new System.Drawing.Point(299, 3);
            this.btnExpand.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnExpand.Name = "btnExpand";
            this.btnExpand.Size = new System.Drawing.Size(62, 37);
            this.btnExpand.TabIndex = 329;
            this.btnExpand.UseVisualStyleBackColor = true;
            this.btnExpand.Click += new System.EventHandler(this.expandWindow_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.lblPWMDisplay);
            this.panel1.Controls.Add(this.btnStartSA);
            this.panel1.Controls.Add(this.labelSteerAngle);
            this.panel1.Controls.Add(this.btnFreeDriveZero);
            this.panel1.Controls.Add(this.btnSteerAngleUp);
            this.panel1.Controls.Add(this.btnFreeDrive);
            this.panel1.Controls.Add(this.labelDiameter);
            this.panel1.Controls.Add(this.btnSteerAngleDown);
            this.panel1.Controls.Add(this.lblCalcSteerAngleInner);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.lblDiameter);
            this.panel1.Location = new System.Drawing.Point(5, 446);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(364, 176);
            this.panel1.TabIndex = 323;
            // 
            // btnStartSA
            // 
            this.btnStartSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartSA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStartSA.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartSA.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStartSA.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.btnStartSA.Location = new System.Drawing.Point(15, 104);
            this.btnStartSA.Name = "btnStartSA";
            this.btnStartSA.Size = new System.Drawing.Size(71, 67);
            this.btnStartSA.TabIndex = 323;
            this.btnStartSA.UseVisualStyleBackColor = true;
            this.btnStartSA.Click += new System.EventHandler(this.btnStartSA_Click);
            // 
            // btnFreeDriveZero
            // 
            this.btnFreeDriveZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFreeDriveZero.BackColor = System.Drawing.Color.White;
            this.btnFreeDriveZero.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnFreeDriveZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFreeDriveZero.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFreeDriveZero.ForeColor = System.Drawing.Color.White;
            this.btnFreeDriveZero.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnFreeDriveZero.Location = new System.Drawing.Point(277, 17);
            this.btnFreeDriveZero.Name = "btnFreeDriveZero";
            this.btnFreeDriveZero.Size = new System.Drawing.Size(73, 56);
            this.btnFreeDriveZero.TabIndex = 313;
            this.btnFreeDriveZero.UseVisualStyleBackColor = false;
            this.btnFreeDriveZero.Click += new System.EventHandler(this.btnFreeDriveZero_Click);
            // 
            // btnSteerAngleUp
            // 
            this.btnSteerAngleUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSteerAngleUp.BackColor = System.Drawing.Color.White;
            this.btnSteerAngleUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSteerAngleUp.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnSteerAngleUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerAngleUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSteerAngleUp.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnSteerAngleUp.Location = new System.Drawing.Point(186, 17);
            this.btnSteerAngleUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSteerAngleUp.Name = "btnSteerAngleUp";
            this.btnSteerAngleUp.Size = new System.Drawing.Size(73, 56);
            this.btnSteerAngleUp.TabIndex = 315;
            this.btnSteerAngleUp.UseVisualStyleBackColor = false;
            this.btnSteerAngleUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSteerAngleUp_MouseDown);
            // 
            // btnFreeDrive
            // 
            this.btnFreeDrive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFreeDrive.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFreeDrive.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnFreeDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFreeDrive.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFreeDrive.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFreeDrive.Image = global::AgOpenGPS.Properties.Resources.SteerDriveOff;
            this.btnFreeDrive.Location = new System.Drawing.Point(4, 17);
            this.btnFreeDrive.Name = "btnFreeDrive";
            this.btnFreeDrive.Size = new System.Drawing.Size(73, 56);
            this.btnFreeDrive.TabIndex = 228;
            this.btnFreeDrive.UseVisualStyleBackColor = false;
            this.btnFreeDrive.Click += new System.EventHandler(this.btnFreeDrive_Click);
            // 
            // btnSteerAngleDown
            // 
            this.btnSteerAngleDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSteerAngleDown.BackColor = System.Drawing.Color.White;
            this.btnSteerAngleDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSteerAngleDown.FlatAppearance.BorderColor = System.Drawing.SystemColors.AppWorkspace;
            this.btnSteerAngleDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerAngleDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSteerAngleDown.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnSteerAngleDown.Location = new System.Drawing.Point(95, 17);
            this.btnSteerAngleDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSteerAngleDown.Name = "btnSteerAngleDown";
            this.btnSteerAngleDown.Size = new System.Drawing.Size(73, 56);
            this.btnSteerAngleDown.TabIndex = 314;
            this.btnSteerAngleDown.UseVisualStyleBackColor = false;
            this.btnSteerAngleDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSteerAngleDown_MouseDown);
            // 
            // hsbarSensor
            // 
            this.hsbarSensor.LargeChange = 1;
            this.hsbarSensor.Location = new System.Drawing.Point(176, 320);
            this.hsbarSensor.Maximum = 255;
            this.hsbarSensor.Name = "hsbarSensor";
            this.hsbarSensor.Size = new System.Drawing.Size(257, 53);
            this.hsbarSensor.TabIndex = 516;
            this.hsbarSensor.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarSensor_Scroll);
            // 
            // lblhsbarSensor
            // 
            this.lblhsbarSensor.AutoSize = true;
            this.lblhsbarSensor.BackColor = System.Drawing.Color.Transparent;
            this.lblhsbarSensor.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblhsbarSensor.ForeColor = System.Drawing.Color.Black;
            this.lblhsbarSensor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblhsbarSensor.Location = new System.Drawing.Point(439, 329);
            this.lblhsbarSensor.Name = "lblhsbarSensor";
            this.lblhsbarSensor.Size = new System.Drawing.Size(57, 29);
            this.lblhsbarSensor.TabIndex = 518;
            this.lblhsbarSensor.Text = "0%";
            this.lblhsbarSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelReset
            // 
            this.labelReset.BackColor = System.Drawing.Color.Transparent;
            this.labelReset.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelReset.ForeColor = System.Drawing.Color.Black;
            this.labelReset.Location = new System.Drawing.Point(485, 530);
            this.labelReset.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelReset.Name = "labelReset";
            this.labelReset.Size = new System.Drawing.Size(150, 23);
            this.labelReset.TabIndex = 523;
            this.labelReset.Text = "Reset All To Defaults";
            this.labelReset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxXY
            // 
            this.cboxXY.BackColor = System.Drawing.Color.White;
            this.cboxXY.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxXY.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxXY.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxXY.FormattingEnabled = true;
            this.cboxXY.Items.AddRange(new object[] {
            "X",
            "Y"});
            this.cboxXY.Location = new System.Drawing.Point(329, 242);
            this.cboxXY.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxXY.Name = "cboxXY";
            this.cboxXY.Size = new System.Drawing.Size(175, 37);
            this.cboxXY.TabIndex = 525;
            this.cboxXY.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // labelIMUAxis
            // 
            this.labelIMUAxis.BackColor = System.Drawing.Color.Transparent;
            this.labelIMUAxis.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.labelIMUAxis.ForeColor = System.Drawing.Color.Black;
            this.labelIMUAxis.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelIMUAxis.Location = new System.Drawing.Point(326, 211);
            this.labelIMUAxis.Name = "labelIMUAxis";
            this.labelIMUAxis.Size = new System.Drawing.Size(181, 29);
            this.labelIMUAxis.TabIndex = 524;
            this.labelIMUAxis.Text = "IMU X or Y Axis";
            this.labelIMUAxis.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label34
            // 
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.Black;
            this.label34.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label34.Location = new System.Drawing.Point(148, 273);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(103, 45);
            this.label34.TabIndex = 527;
            this.label34.Text = "Stanley/ Pure";
            this.label34.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tabSteerSettings
            // 
            this.tabSteerSettings.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabSteerSettings.Controls.Add(this.tabSensors);
            this.tabSteerSettings.Controls.Add(this.tabConfig);
            this.tabSteerSettings.Controls.Add(this.tabSettings);
            this.tabSteerSettings.Controls.Add(this.tabAlarm);
            this.tabSteerSettings.Controls.Add(this.tabOnTheLine);
            this.tabSteerSettings.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSteerSettings.ImageList = this.imageList2;
            this.tabSteerSettings.ItemSize = new System.Drawing.Size(100, 48);
            this.tabSteerSettings.Location = new System.Drawing.Point(373, 5);
            this.tabSteerSettings.Name = "tabSteerSettings";
            this.tabSteerSettings.SelectedIndex = 0;
            this.tabSteerSettings.Size = new System.Drawing.Size(520, 525);
            this.tabSteerSettings.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabSteerSettings.TabIndex = 528;
            // 
            // tabSensors
            // 
            this.tabSensors.BackColor = System.Drawing.Color.LightGray;
            this.tabSensors.Controls.Add(this.hsbarSensor);
            this.tabSensors.Controls.Add(this.nudMaxCounts);
            this.tabSensors.Controls.Add(this.labelTurnSensor);
            this.tabSensors.Controls.Add(this.pbarSensor);
            this.tabSensors.Controls.Add(this.lblPercentFS);
            this.tabSensors.Controls.Add(this.lblhsbarSensor);
            this.tabSensors.Controls.Add(this.labelPressureTurnSensor);
            this.tabSensors.Controls.Add(this.labelCurrentTurnSensor);
            this.tabSensors.Controls.Add(this.labelEncoder);
            this.tabSensors.Controls.Add(this.cboxCurrentSensor);
            this.tabSensors.Controls.Add(this.cboxEncoder);
            this.tabSensors.Controls.Add(this.cboxPressureSensor);
            this.tabSensors.ImageIndex = 0;
            this.tabSensors.Location = new System.Drawing.Point(4, 52);
            this.tabSensors.Name = "tabSensors";
            this.tabSensors.Padding = new System.Windows.Forms.Padding(3);
            this.tabSensors.Size = new System.Drawing.Size(512, 469);
            this.tabSensors.TabIndex = 0;
            // 
            // nudMaxCounts
            // 
            this.nudMaxCounts.BackColor = System.Drawing.Color.White;
            this.nudMaxCounts.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxCounts.InterceptArrowKeys = false;
            this.nudMaxCounts.Location = new System.Drawing.Point(256, 45);
            this.nudMaxCounts.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudMaxCounts.Name = "nudMaxCounts";
            this.nudMaxCounts.ReadOnly = true;
            this.nudMaxCounts.Size = new System.Drawing.Size(107, 52);
            this.nudMaxCounts.TabIndex = 493;
            this.nudMaxCounts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMaxCounts.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.nudMaxCounts.Click += new System.EventHandler(this.nudMaxCounts_Click);
            // 
            // cboxCurrentSensor
            // 
            this.cboxCurrentSensor.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxCurrentSensor.BackColor = System.Drawing.Color.White;
            this.cboxCurrentSensor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxCurrentSensor.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxCurrentSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxCurrentSensor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxCurrentSensor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxCurrentSensor.Image = global::AgOpenGPS.Properties.Resources.ConSt_TurnSensorCurrent;
            this.cboxCurrentSensor.Location = new System.Drawing.Point(28, 295);
            this.cboxCurrentSensor.Name = "cboxCurrentSensor";
            this.cboxCurrentSensor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxCurrentSensor.Size = new System.Drawing.Size(114, 78);
            this.cboxCurrentSensor.TabIndex = 510;
            this.cboxCurrentSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxCurrentSensor.UseVisualStyleBackColor = false;
            this.cboxCurrentSensor.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // cboxEncoder
            // 
            this.cboxEncoder.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxEncoder.BackColor = System.Drawing.Color.White;
            this.cboxEncoder.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxEncoder.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxEncoder.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxEncoder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxEncoder.Image = global::AgOpenGPS.Properties.Resources.ConSt_TurnSensor;
            this.cboxEncoder.Location = new System.Drawing.Point(25, 32);
            this.cboxEncoder.Name = "cboxEncoder";
            this.cboxEncoder.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxEncoder.Size = new System.Drawing.Size(114, 78);
            this.cboxEncoder.TabIndex = 492;
            this.cboxEncoder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxEncoder.UseVisualStyleBackColor = false;
            this.cboxEncoder.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // cboxPressureSensor
            // 
            this.cboxPressureSensor.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxPressureSensor.BackColor = System.Drawing.Color.White;
            this.cboxPressureSensor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxPressureSensor.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxPressureSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxPressureSensor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxPressureSensor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxPressureSensor.Image = global::AgOpenGPS.Properties.Resources.ConSt_TurnSensorPressure;
            this.cboxPressureSensor.Location = new System.Drawing.Point(27, 159);
            this.cboxPressureSensor.Name = "cboxPressureSensor";
            this.cboxPressureSensor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxPressureSensor.Size = new System.Drawing.Size(114, 78);
            this.cboxPressureSensor.TabIndex = 508;
            this.cboxPressureSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxPressureSensor.UseVisualStyleBackColor = false;
            this.cboxPressureSensor.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // tabConfig
            // 
            this.tabConfig.BackColor = System.Drawing.Color.LightGray;
            this.tabConfig.Controls.Add(this.cboxXY);
            this.tabConfig.Controls.Add(this.labelIMUAxis);
            this.tabConfig.Controls.Add(this.labelInvertWas);
            this.tabConfig.Controls.Add(this.label55);
            this.tabConfig.Controls.Add(this.labelSteerDescription);
            this.tabConfig.Controls.Add(this.labelADConverter);
            this.tabConfig.Controls.Add(this.labelInvertMotor);
            this.tabConfig.Controls.Add(this.labelMotorDriver);
            this.tabConfig.Controls.Add(this.labelInvertRelays);
            this.tabConfig.Controls.Add(this.cboxConv);
            this.tabConfig.Controls.Add(this.labelSteerEnable);
            this.tabConfig.Controls.Add(this.cboxMotorDrive);
            this.tabConfig.Controls.Add(this.cboxSteerEnable);
            this.tabConfig.Controls.Add(this.chkInvertWAS);
            this.tabConfig.Controls.Add(this.chkInvertSteer);
            this.tabConfig.Controls.Add(this.chkSteerInvertRelays);
            this.tabConfig.Controls.Add(this.cboxDanfoss);
            this.tabConfig.ImageIndex = 1;
            this.tabConfig.Location = new System.Drawing.Point(4, 52);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(512, 469);
            this.tabConfig.TabIndex = 1;
            // 
            // chkInvertWAS
            // 
            this.chkInvertWAS.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertWAS.BackColor = System.Drawing.Color.White;
            this.chkInvertWAS.Checked = true;
            this.chkInvertWAS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInvertWAS.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertWAS.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkInvertWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertWAS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertWAS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkInvertWAS.Image = global::AgOpenGPS.Properties.Resources.ConSt_InvertWAS;
            this.chkInvertWAS.Location = new System.Drawing.Point(165, 57);
            this.chkInvertWAS.Name = "chkInvertWAS";
            this.chkInvertWAS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertWAS.Size = new System.Drawing.Size(109, 78);
            this.chkInvertWAS.TabIndex = 490;
            this.chkInvertWAS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertWAS.UseVisualStyleBackColor = false;
            this.chkInvertWAS.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // chkInvertSteer
            // 
            this.chkInvertSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertSteer.BackColor = System.Drawing.Color.White;
            this.chkInvertSteer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkInvertSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertSteer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkInvertSteer.Image = global::AgOpenGPS.Properties.Resources.ConSt_InvertDirection;
            this.chkInvertSteer.Location = new System.Drawing.Point(165, 192);
            this.chkInvertSteer.Name = "chkInvertSteer";
            this.chkInvertSteer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertSteer.Size = new System.Drawing.Size(109, 78);
            this.chkInvertSteer.TabIndex = 491;
            this.chkInvertSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertSteer.UseVisualStyleBackColor = false;
            this.chkInvertSteer.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // chkSteerInvertRelays
            // 
            this.chkSteerInvertRelays.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSteerInvertRelays.BackColor = System.Drawing.Color.White;
            this.chkSteerInvertRelays.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSteerInvertRelays.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSteerInvertRelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSteerInvertRelays.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSteerInvertRelays.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSteerInvertRelays.Image = global::AgOpenGPS.Properties.Resources.ConSt_InvertRelay;
            this.chkSteerInvertRelays.Location = new System.Drawing.Point(165, 324);
            this.chkSteerInvertRelays.Name = "chkSteerInvertRelays";
            this.chkSteerInvertRelays.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkSteerInvertRelays.Size = new System.Drawing.Size(109, 78);
            this.chkSteerInvertRelays.TabIndex = 503;
            this.chkSteerInvertRelays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSteerInvertRelays.UseVisualStyleBackColor = false;
            this.chkSteerInvertRelays.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // cboxDanfoss
            // 
            this.cboxDanfoss.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxDanfoss.BackColor = System.Drawing.Color.White;
            this.cboxDanfoss.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxDanfoss.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxDanfoss.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxDanfoss.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDanfoss.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxDanfoss.Image = global::AgOpenGPS.Properties.Resources.ConSt_Danfoss;
            this.cboxDanfoss.Location = new System.Drawing.Point(9, 57);
            this.cboxDanfoss.Name = "cboxDanfoss";
            this.cboxDanfoss.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxDanfoss.Size = new System.Drawing.Size(114, 78);
            this.cboxDanfoss.TabIndex = 507;
            this.cboxDanfoss.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxDanfoss.UseVisualStyleBackColor = false;
            this.cboxDanfoss.Click += new System.EventHandler(this.EnableAlert_Click);
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.Color.LightGray;
            this.tabSettings.Controls.Add(this.labelSteerInReverse);
            this.tabSettings.Controls.Add(this.label41);
            this.tabSettings.Controls.Add(this.label39);
            this.tabSettings.Controls.Add(this.hsbarUTurnCompensation);
            this.tabSettings.Controls.Add(this.lblUTurnCompensation);
            this.tabSettings.Controls.Add(this.labelUturnCompensation);
            this.tabSettings.Controls.Add(this.hsbarSideHillComp);
            this.tabSettings.Controls.Add(this.label34);
            this.tabSettings.Controls.Add(this.lblSideHillComp);
            this.tabSettings.Controls.Add(this.labelSideHill);
            this.tabSettings.Controls.Add(this.cboxSteerInReverse);
            this.tabSettings.Controls.Add(this.btnStanleyPure);
            this.tabSettings.ImageIndex = 2;
            this.tabSettings.Location = new System.Drawing.Point(4, 52);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(512, 469);
            this.tabSettings.TabIndex = 2;
            this.tabSettings.Enter += new System.EventHandler(this.tabSettings_Enter);
            this.tabSettings.Leave += new System.EventHandler(this.tabSettings_Leave);
            // 
            // labelSteerInReverse
            // 
            this.labelSteerInReverse.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSteerInReverse.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelSteerInReverse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelSteerInReverse.Location = new System.Drawing.Point(266, 273);
            this.labelSteerInReverse.Name = "labelSteerInReverse";
            this.labelSteerInReverse.Size = new System.Drawing.Size(201, 42);
            this.labelSteerInReverse.TabIndex = 534;
            this.labelSteerInReverse.Text = "Steer In Reverse?";
            this.labelSteerInReverse.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.BackColor = System.Drawing.Color.Transparent;
            this.label41.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.Black;
            this.label41.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label41.Location = new System.Drawing.Point(426, 103);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(18, 16);
            this.label41.TabIndex = 532;
            this.label41.Text = "In";
            this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label39
            // 
            this.label39.AutoSize = true;
            this.label39.BackColor = System.Drawing.Color.Transparent;
            this.label39.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.Black;
            this.label39.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label39.Location = new System.Drawing.Point(112, 103);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(27, 16);
            this.label39.TabIndex = 531;
            this.label39.Text = "Out";
            this.label39.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // hsbarUTurnCompensation
            // 
            this.hsbarUTurnCompensation.LargeChange = 1;
            this.hsbarUTurnCompensation.Location = new System.Drawing.Point(109, 46);
            this.hsbarUTurnCompensation.Maximum = 20;
            this.hsbarUTurnCompensation.Minimum = 2;
            this.hsbarUTurnCompensation.Name = "hsbarUTurnCompensation";
            this.hsbarUTurnCompensation.Size = new System.Drawing.Size(339, 53);
            this.hsbarUTurnCompensation.TabIndex = 529;
            this.hsbarUTurnCompensation.Value = 5;
            this.hsbarUTurnCompensation.ValueChanged += new System.EventHandler(this.hsbarUTurnCompensation_ValueChanged);
            // 
            // lblUTurnCompensation
            // 
            this.lblUTurnCompensation.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUTurnCompensation.ForeColor = System.Drawing.Color.Black;
            this.lblUTurnCompensation.Location = new System.Drawing.Point(18, 56);
            this.lblUTurnCompensation.Name = "lblUTurnCompensation";
            this.lblUTurnCompensation.Size = new System.Drawing.Size(88, 35);
            this.lblUTurnCompensation.TabIndex = 530;
            this.lblUTurnCompensation.Text = "888";
            this.lblUTurnCompensation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelUturnCompensation
            // 
            this.labelUturnCompensation.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelUturnCompensation.ForeColor = System.Drawing.Color.Black;
            this.labelUturnCompensation.Location = new System.Drawing.Point(109, 25);
            this.labelUturnCompensation.Name = "labelUturnCompensation";
            this.labelUturnCompensation.Size = new System.Drawing.Size(339, 19);
            this.labelUturnCompensation.TabIndex = 528;
            this.labelUturnCompensation.Text = "UTurn Compensation";
            this.labelUturnCompensation.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelUturnCompensation.UseCompatibleTextRendering = true;
            // 
            // cboxSteerInReverse
            // 
            this.cboxSteerInReverse.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxSteerInReverse.BackColor = System.Drawing.Color.White;
            this.cboxSteerInReverse.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxSteerInReverse.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightGreen;
            this.cboxSteerInReverse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxSteerInReverse.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSteerInReverse.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxSteerInReverse.Image = global::AgOpenGPS.Properties.Resources.ConV_RevSteer;
            this.cboxSteerInReverse.Location = new System.Drawing.Point(323, 318);
            this.cboxSteerInReverse.Name = "cboxSteerInReverse";
            this.cboxSteerInReverse.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxSteerInReverse.Size = new System.Drawing.Size(86, 90);
            this.cboxSteerInReverse.TabIndex = 533;
            this.cboxSteerInReverse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.cboxSteerInReverse.UseVisualStyleBackColor = false;
            this.cboxSteerInReverse.Click += new System.EventHandler(this.cboxSteerInReverse_Click);
            // 
            // btnStanleyPure
            // 
            this.btnStanleyPure.BackColor = System.Drawing.Color.White;
            this.btnStanleyPure.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStanleyPure.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnStanleyPure.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStanleyPure.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStanleyPure.ForeColor = System.Drawing.Color.Black;
            this.btnStanleyPure.Image = global::AgOpenGPS.Properties.Resources.ModeStanley;
            this.btnStanleyPure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStanleyPure.Location = new System.Drawing.Point(153, 318);
            this.btnStanleyPure.Margin = new System.Windows.Forms.Padding(0);
            this.btnStanleyPure.Name = "btnStanleyPure";
            this.btnStanleyPure.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnStanleyPure.Size = new System.Drawing.Size(86, 90);
            this.btnStanleyPure.TabIndex = 526;
            this.btnStanleyPure.UseVisualStyleBackColor = false;
            this.btnStanleyPure.Click += new System.EventHandler(this.btnStanleyPure_Click);
            // 
            // tabAlarm
            // 
            this.tabAlarm.BackColor = System.Drawing.Color.Gainsboro;
            this.tabAlarm.Controls.Add(this.labelMinSpeed);
            this.tabAlarm.Controls.Add(this.label166);
            this.tabAlarm.Controls.Add(this.labelMaxSpeed);
            this.tabAlarm.Controls.Add(this.label163);
            this.tabAlarm.Controls.Add(this.label160);
            this.tabAlarm.Controls.Add(this.labelManualTurns);
            this.tabAlarm.Controls.Add(this.pictureBox17);
            this.tabAlarm.Controls.Add(this.pictureBox16);
            this.tabAlarm.Controls.Add(this.pictureBox10);
            this.tabAlarm.Controls.Add(this.nudMinSteerSpeed);
            this.tabAlarm.Controls.Add(this.nudMaxSteerSpeed);
            this.tabAlarm.Controls.Add(this.nudGuidanceSpeedLimit);
            this.tabAlarm.ImageIndex = 3;
            this.tabAlarm.Location = new System.Drawing.Point(4, 52);
            this.tabAlarm.Name = "tabAlarm";
            this.tabAlarm.Size = new System.Drawing.Size(512, 469);
            this.tabAlarm.TabIndex = 3;
            this.tabAlarm.Enter += new System.EventHandler(this.tabAlarm_Enter);
            this.tabAlarm.Leave += new System.EventHandler(this.tabAlarm_Leave);
            // 
            // labelMinSpeed
            // 
            this.labelMinSpeed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinSpeed.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelMinSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelMinSpeed.Location = new System.Drawing.Point(108, 249);
            this.labelMinSpeed.Name = "labelMinSpeed";
            this.labelMinSpeed.Size = new System.Drawing.Size(118, 19);
            this.labelMinSpeed.TabIndex = 504;
            this.labelMinSpeed.Text = "Min Speed";
            this.labelMinSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label166
            // 
            this.label166.AutoSize = true;
            this.label166.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label166.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label166.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label166.Location = new System.Drawing.Point(149, 409);
            this.label166.Name = "label166";
            this.label166.Size = new System.Drawing.Size(36, 18);
            this.label166.TabIndex = 507;
            this.label166.Text = "kmh";
            this.label166.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelMaxSpeed
            // 
            this.labelMaxSpeed.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxSpeed.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelMaxSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelMaxSpeed.Location = new System.Drawing.Point(298, 251);
            this.labelMaxSpeed.Name = "labelMaxSpeed";
            this.labelMaxSpeed.Size = new System.Drawing.Size(118, 19);
            this.labelMaxSpeed.TabIndex = 500;
            this.labelMaxSpeed.Text = "Max Speed";
            this.labelMaxSpeed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label163
            // 
            this.label163.AutoSize = true;
            this.label163.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label163.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label163.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label163.Location = new System.Drawing.Point(339, 409);
            this.label163.Name = "label163";
            this.label163.Size = new System.Drawing.Size(36, 18);
            this.label163.TabIndex = 503;
            this.label163.Text = "kmh";
            this.label163.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label160
            // 
            this.label160.AutoSize = true;
            this.label160.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label160.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label160.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label160.Location = new System.Drawing.Point(244, 195);
            this.label160.Name = "label160";
            this.label160.Size = new System.Drawing.Size(38, 18);
            this.label160.TabIndex = 494;
            this.label160.Text = "Kmh";
            this.label160.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelManualTurns
            // 
            this.labelManualTurns.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelManualTurns.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelManualTurns.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelManualTurns.Location = new System.Drawing.Point(152, 38);
            this.labelManualTurns.Name = "labelManualTurns";
            this.labelManualTurns.Size = new System.Drawing.Size(223, 19);
            this.labelManualTurns.TabIndex = 499;
            this.labelManualTurns.Text = "Manual Turns";
            this.labelManualTurns.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox17
            // 
            this.pictureBox17.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConV_MinAutoSteer;
            this.pictureBox17.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox17.Location = new System.Drawing.Point(108, 273);
            this.pictureBox17.Name = "pictureBox17";
            this.pictureBox17.Size = new System.Drawing.Size(118, 75);
            this.pictureBox17.TabIndex = 506;
            this.pictureBox17.TabStop = false;
            // 
            // pictureBox16
            // 
            this.pictureBox16.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConV_MaxAutoSteer;
            this.pictureBox16.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox16.Location = new System.Drawing.Point(298, 273);
            this.pictureBox16.Name = "pictureBox16";
            this.pictureBox16.Size = new System.Drawing.Size(118, 75);
            this.pictureBox16.TabIndex = 502;
            this.pictureBox16.TabStop = false;
            // 
            // pictureBox10
            // 
            this.pictureBox10.BackgroundImage = global::AgOpenGPS.Properties.Resources.con_VehicleFunctionSpeedLimit;
            this.pictureBox10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox10.Location = new System.Drawing.Point(203, 60);
            this.pictureBox10.Name = "pictureBox10";
            this.pictureBox10.Size = new System.Drawing.Size(118, 74);
            this.pictureBox10.TabIndex = 493;
            this.pictureBox10.TabStop = false;
            // 
            // nudMinSteerSpeed
            // 
            this.nudMinSteerSpeed.BackColor = System.Drawing.Color.White;
            this.nudMinSteerSpeed.DecimalPlaces = 1;
            this.nudMinSteerSpeed.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinSteerSpeed.InterceptArrowKeys = false;
            this.nudMinSteerSpeed.Location = new System.Drawing.Point(112, 354);
            this.nudMinSteerSpeed.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudMinSteerSpeed.Name = "nudMinSteerSpeed";
            this.nudMinSteerSpeed.ReadOnly = true;
            this.nudMinSteerSpeed.Size = new System.Drawing.Size(110, 52);
            this.nudMinSteerSpeed.TabIndex = 505;
            this.nudMinSteerSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMinSteerSpeed.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudMinSteerSpeed.Click += new System.EventHandler(this.nudMinSteerSpeed_Click);
            // 
            // nudMaxSteerSpeed
            // 
            this.nudMaxSteerSpeed.BackColor = System.Drawing.Color.White;
            this.nudMaxSteerSpeed.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxSteerSpeed.InterceptArrowKeys = false;
            this.nudMaxSteerSpeed.Location = new System.Drawing.Point(302, 354);
            this.nudMaxSteerSpeed.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudMaxSteerSpeed.Name = "nudMaxSteerSpeed";
            this.nudMaxSteerSpeed.ReadOnly = true;
            this.nudMaxSteerSpeed.Size = new System.Drawing.Size(110, 52);
            this.nudMaxSteerSpeed.TabIndex = 501;
            this.nudMaxSteerSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMaxSteerSpeed.Value = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.nudMaxSteerSpeed.Click += new System.EventHandler(this.nudMaxSteerSpeed_Click);
            // 
            // nudGuidanceSpeedLimit
            // 
            this.nudGuidanceSpeedLimit.BackColor = System.Drawing.Color.White;
            this.nudGuidanceSpeedLimit.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudGuidanceSpeedLimit.InterceptArrowKeys = false;
            this.nudGuidanceSpeedLimit.Location = new System.Drawing.Point(207, 140);
            this.nudGuidanceSpeedLimit.Maximum = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudGuidanceSpeedLimit.Name = "nudGuidanceSpeedLimit";
            this.nudGuidanceSpeedLimit.ReadOnly = true;
            this.nudGuidanceSpeedLimit.Size = new System.Drawing.Size(110, 52);
            this.nudGuidanceSpeedLimit.TabIndex = 492;
            this.nudGuidanceSpeedLimit.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudGuidanceSpeedLimit.Value = new decimal(new int[] {
            7,
            0,
            0,
            0});
            this.nudGuidanceSpeedLimit.Click += new System.EventHandler(this.nudGuidanceSpeedLimit_Click);
            // 
            // tabOnTheLine
            // 
            this.tabOnTheLine.BackColor = System.Drawing.Color.Gainsboro;
            this.tabOnTheLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.tabOnTheLine.Controls.Add(this.labelLineWidth);
            this.tabOnTheLine.Controls.Add(this.label44);
            this.tabOnTheLine.Controls.Add(this.labelNudgeDistance);
            this.tabOnTheLine.Controls.Add(this.label43);
            this.tabOnTheLine.Controls.Add(this.groupboxlabelGuidanceBar);
            this.tabOnTheLine.Controls.Add(this.labelCmPix);
            this.tabOnTheLine.Controls.Add(this.label107);
            this.tabOnTheLine.Controls.Add(this.label46);
            this.tabOnTheLine.Controls.Add(this.pictureBox1);
            this.tabOnTheLine.Controls.Add(this.labelNextGuidanceLine);
            this.tabOnTheLine.Controls.Add(this.pictureBox2);
            this.tabOnTheLine.Controls.Add(this.pictureBox12);
            this.tabOnTheLine.Controls.Add(this.pictureBox5);
            this.tabOnTheLine.Controls.Add(this.nudcmPerPixel);
            this.tabOnTheLine.Controls.Add(this.nudLineWidth);
            this.tabOnTheLine.Controls.Add(this.nudSnapDistance);
            this.tabOnTheLine.Controls.Add(this.nudGuidanceLookAhead);
            this.tabOnTheLine.ImageIndex = 4;
            this.tabOnTheLine.Location = new System.Drawing.Point(4, 52);
            this.tabOnTheLine.Name = "tabOnTheLine";
            this.tabOnTheLine.Padding = new System.Windows.Forms.Padding(3);
            this.tabOnTheLine.Size = new System.Drawing.Size(512, 469);
            this.tabOnTheLine.TabIndex = 4;
            this.tabOnTheLine.Enter += new System.EventHandler(this.tabOnTheLine_Enter);
            this.tabOnTheLine.Leave += new System.EventHandler(this.tabOnTheLine_Leave);
            // 
            // labelLineWidth
            // 
            this.labelLineWidth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLineWidth.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelLineWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelLineWidth.Location = new System.Drawing.Point(13, 22);
            this.labelLineWidth.Name = "labelLineWidth";
            this.labelLineWidth.Size = new System.Drawing.Size(122, 19);
            this.labelLineWidth.TabIndex = 523;
            this.labelLineWidth.Text = "Line Width";
            this.labelLineWidth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label44
            // 
            this.label44.AutoSize = true;
            this.label44.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label44.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label44.Location = new System.Drawing.Point(171, 95);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(43, 18);
            this.label44.TabIndex = 522;
            this.label44.Text = "pixels";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // labelNudgeDistance
            // 
            this.labelNudgeDistance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNudgeDistance.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelNudgeDistance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelNudgeDistance.Location = new System.Drawing.Point(285, 22);
            this.labelNudgeDistance.Name = "labelNudgeDistance";
            this.labelNudgeDistance.Size = new System.Drawing.Size(205, 19);
            this.labelNudgeDistance.TabIndex = 519;
            this.labelNudgeDistance.Text = "Nudge Distance";
            this.labelNudgeDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label43.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label43.Location = new System.Drawing.Point(437, 95);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(28, 18);
            this.label43.TabIndex = 518;
            this.label43.Text = "cm";
            this.label43.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupboxlabelGuidanceBar
            // 
            this.groupboxlabelGuidanceBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.groupboxlabelGuidanceBar.Controls.Add(this.rbtnSteerBar);
            this.groupboxlabelGuidanceBar.Controls.Add(this.labelSteerBar);
            this.groupboxlabelGuidanceBar.Controls.Add(this.labelOnOff);
            this.groupboxlabelGuidanceBar.Controls.Add(this.rbtnLightBar);
            this.groupboxlabelGuidanceBar.Controls.Add(this.labelLightbar);
            this.groupboxlabelGuidanceBar.Controls.Add(this.chkDisplayLightbar);
            this.groupboxlabelGuidanceBar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.groupboxlabelGuidanceBar.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupboxlabelGuidanceBar.ForeColor = System.Drawing.Color.Black;
            this.groupboxlabelGuidanceBar.Location = new System.Drawing.Point(42, 311);
            this.groupboxlabelGuidanceBar.Name = "groupboxlabelGuidanceBar";
            this.groupboxlabelGuidanceBar.Size = new System.Drawing.Size(424, 139);
            this.groupboxlabelGuidanceBar.TabIndex = 528;
            this.groupboxlabelGuidanceBar.TabStop = false;
            this.groupboxlabelGuidanceBar.Text = "Guidance Bar";
            // 
            // rbtnSteerBar
            // 
            this.rbtnSteerBar.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnSteerBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rbtnSteerBar.Checked = true;
            this.rbtnSteerBar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.rbtnSteerBar.FlatAppearance.BorderSize = 2;
            this.rbtnSteerBar.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightGreen;
            this.rbtnSteerBar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnSteerBar.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnSteerBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbtnSteerBar.Image = global::AgOpenGPS.Properties.Resources.ConD_SteerBarBar;
            this.rbtnSteerBar.Location = new System.Drawing.Point(150, 40);
            this.rbtnSteerBar.Name = "rbtnSteerBar";
            this.rbtnSteerBar.Size = new System.Drawing.Size(121, 74);
            this.rbtnSteerBar.TabIndex = 469;
            this.rbtnSteerBar.TabStop = true;
            this.rbtnSteerBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnSteerBar.UseVisualStyleBackColor = false;
            this.rbtnSteerBar.Click += new System.EventHandler(this.rbtnSteerBar_Click);
            // 
            // labelSteerBar
            // 
            this.labelSteerBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelSteerBar.BackColor = System.Drawing.Color.Transparent;
            this.labelSteerBar.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSteerBar.ForeColor = System.Drawing.Color.Black;
            this.labelSteerBar.Location = new System.Drawing.Point(150, 118);
            this.labelSteerBar.Name = "labelSteerBar";
            this.labelSteerBar.Size = new System.Drawing.Size(121, 19);
            this.labelSteerBar.TabIndex = 533;
            this.labelSteerBar.Text = "Steer Bar";
            this.labelSteerBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelOnOff
            // 
            this.labelOnOff.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelOnOff.BackColor = System.Drawing.Color.Transparent;
            this.labelOnOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelOnOff.ForeColor = System.Drawing.Color.Black;
            this.labelOnOff.Location = new System.Drawing.Point(307, 118);
            this.labelOnOff.Name = "labelOnOff";
            this.labelOnOff.Size = new System.Drawing.Size(98, 19);
            this.labelOnOff.TabIndex = 534;
            this.labelOnOff.Text = "On/Off";
            this.labelOnOff.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rbtnLightBar
            // 
            this.rbtnLightBar.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnLightBar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.rbtnLightBar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.rbtnLightBar.FlatAppearance.BorderSize = 2;
            this.rbtnLightBar.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightGreen;
            this.rbtnLightBar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnLightBar.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbtnLightBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.rbtnLightBar.Image = global::AgOpenGPS.Properties.Resources.ConD_LightBar;
            this.rbtnLightBar.Location = new System.Drawing.Point(11, 40);
            this.rbtnLightBar.Name = "rbtnLightBar";
            this.rbtnLightBar.Size = new System.Drawing.Size(121, 74);
            this.rbtnLightBar.TabIndex = 468;
            this.rbtnLightBar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.rbtnLightBar.UseVisualStyleBackColor = false;
            this.rbtnLightBar.Click += new System.EventHandler(this.rbtnLightBar_Click);
            // 
            // labelLightbar
            // 
            this.labelLightbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelLightbar.BackColor = System.Drawing.Color.Transparent;
            this.labelLightbar.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelLightbar.ForeColor = System.Drawing.Color.Black;
            this.labelLightbar.Location = new System.Drawing.Point(11, 118);
            this.labelLightbar.Name = "labelLightbar";
            this.labelLightbar.Size = new System.Drawing.Size(121, 19);
            this.labelLightbar.TabIndex = 520;
            this.labelLightbar.Text = "Lightbar";
            this.labelLightbar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkDisplayLightbar
            // 
            this.chkDisplayLightbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkDisplayLightbar.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkDisplayLightbar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.chkDisplayLightbar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkDisplayLightbar.FlatAppearance.BorderSize = 2;
            this.chkDisplayLightbar.FlatAppearance.CheckedBackColor = System.Drawing.Color.LightGreen;
            this.chkDisplayLightbar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkDisplayLightbar.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkDisplayLightbar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkDisplayLightbar.Image = global::AgOpenGPS.Properties.Resources.SwitchOn;
            this.chkDisplayLightbar.Location = new System.Drawing.Point(307, 39);
            this.chkDisplayLightbar.Name = "chkDisplayLightbar";
            this.chkDisplayLightbar.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkDisplayLightbar.Size = new System.Drawing.Size(100, 74);
            this.chkDisplayLightbar.TabIndex = 529;
            this.chkDisplayLightbar.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkDisplayLightbar.UseVisualStyleBackColor = false;
            this.chkDisplayLightbar.Click += new System.EventHandler(this.chkDisplayLightbar_Click);
            // 
            // labelCmPix
            // 
            this.labelCmPix.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCmPix.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelCmPix.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelCmPix.Location = new System.Drawing.Point(289, 167);
            this.labelCmPix.Name = "labelCmPix";
            this.labelCmPix.Size = new System.Drawing.Size(201, 19);
            this.labelCmPix.TabIndex = 527;
            this.labelCmPix.Text = "cm Per Pixel";
            this.labelCmPix.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label107
            // 
            this.label107.AutoSize = true;
            this.label107.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label107.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label107.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label107.Location = new System.Drawing.Point(160, 245);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(62, 18);
            this.label107.TabIndex = 513;
            this.label107.Text = "Seconds";
            this.label107.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label46.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label46.Location = new System.Drawing.Point(436, 242);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(30, 19);
            this.label46.TabIndex = 526;
            this.label46.Text = "cm";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConV_CmPixel;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Location = new System.Drawing.Point(285, 190);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(118, 75);
            this.pictureBox1.TabIndex = 525;
            this.pictureBox1.TabStop = false;
            // 
            // labelNextGuidanceLine
            // 
            this.labelNextGuidanceLine.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelNextGuidanceLine.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.labelNextGuidanceLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelNextGuidanceLine.Location = new System.Drawing.Point(10, 164);
            this.labelNextGuidanceLine.Name = "labelNextGuidanceLine";
            this.labelNextGuidanceLine.Size = new System.Drawing.Size(236, 24);
            this.labelNextGuidanceLine.TabIndex = 514;
            this.labelNextGuidanceLine.Text = "Next Guidance Line Search Time";
            this.labelNextGuidanceLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConV_SnapDistance;
            this.pictureBox2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox2.Location = new System.Drawing.Point(285, 44);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(118, 75);
            this.pictureBox2.TabIndex = 517;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox12
            // 
            this.pictureBox12.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConV_GuidanceLookAhead;
            this.pictureBox12.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox12.Location = new System.Drawing.Point(17, 190);
            this.pictureBox12.Name = "pictureBox12";
            this.pictureBox12.Size = new System.Drawing.Size(118, 75);
            this.pictureBox12.TabIndex = 515;
            this.pictureBox12.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConV_LineWith;
            this.pictureBox5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox5.Location = new System.Drawing.Point(17, 44);
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.Size = new System.Drawing.Size(118, 75);
            this.pictureBox5.TabIndex = 521;
            this.pictureBox5.TabStop = false;
            // 
            // nudcmPerPixel
            // 
            this.nudcmPerPixel.BackColor = System.Drawing.Color.White;
            this.nudcmPerPixel.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudcmPerPixel.InterceptArrowKeys = false;
            this.nudcmPerPixel.Location = new System.Drawing.Point(412, 193);
            this.nudcmPerPixel.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            0});
            this.nudcmPerPixel.Name = "nudcmPerPixel";
            this.nudcmPerPixel.ReadOnly = true;
            this.nudcmPerPixel.Size = new System.Drawing.Size(78, 46);
            this.nudcmPerPixel.TabIndex = 524;
            this.nudcmPerPixel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudcmPerPixel.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudcmPerPixel.Click += new System.EventHandler(this.nudcmPerPixel_Click);
            // 
            // nudLineWidth
            // 
            this.nudLineWidth.BackColor = System.Drawing.Color.White;
            this.nudLineWidth.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLineWidth.InterceptArrowKeys = false;
            this.nudLineWidth.Location = new System.Drawing.Point(153, 47);
            this.nudLineWidth.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudLineWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLineWidth.Name = "nudLineWidth";
            this.nudLineWidth.ReadOnly = true;
            this.nudLineWidth.Size = new System.Drawing.Size(78, 46);
            this.nudLineWidth.TabIndex = 520;
            this.nudLineWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLineWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLineWidth.Click += new System.EventHandler(this.nudLineWidth_Click);
            // 
            // nudSnapDistance
            // 
            this.nudSnapDistance.BackColor = System.Drawing.Color.White;
            this.nudSnapDistance.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudSnapDistance.InterceptArrowKeys = false;
            this.nudSnapDistance.Location = new System.Drawing.Point(412, 47);
            this.nudSnapDistance.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSnapDistance.Name = "nudSnapDistance";
            this.nudSnapDistance.ReadOnly = true;
            this.nudSnapDistance.Size = new System.Drawing.Size(78, 46);
            this.nudSnapDistance.TabIndex = 516;
            this.nudSnapDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSnapDistance.Value = new decimal(new int[] {
            499,
            0,
            0,
            0});
            this.nudSnapDistance.Click += new System.EventHandler(this.nudSnapDistance_Click);
            // 
            // nudGuidanceLookAhead
            // 
            this.nudGuidanceLookAhead.BackColor = System.Drawing.Color.White;
            this.nudGuidanceLookAhead.DecimalPlaces = 1;
            this.nudGuidanceLookAhead.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudGuidanceLookAhead.InterceptArrowKeys = false;
            this.nudGuidanceLookAhead.Location = new System.Drawing.Point(142, 196);
            this.nudGuidanceLookAhead.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudGuidanceLookAhead.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudGuidanceLookAhead.Name = "nudGuidanceLookAhead";
            this.nudGuidanceLookAhead.ReadOnly = true;
            this.nudGuidanceLookAhead.Size = new System.Drawing.Size(100, 46);
            this.nudGuidanceLookAhead.TabIndex = 512;
            this.nudGuidanceLookAhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudGuidanceLookAhead.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudGuidanceLookAhead.Click += new System.EventHandler(this.nudGuidanceLookAhead_Click);
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Sensors.png");
            this.imageList2.Images.SetKeyName(1, "ConS_Pins.png");
            this.imageList2.Images.SetKeyName(2, "ConS_ModulesSteer.png");
            this.imageList2.Images.SetKeyName(3, "ConS_Alarm.png");
            this.imageList2.Images.SetKeyName(4, "ConS_VehicleConfig.png");
            // 
            // labelWizard
            // 
            this.labelWizard.BackColor = System.Drawing.Color.Transparent;
            this.labelWizard.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWizard.ForeColor = System.Drawing.Color.Black;
            this.labelWizard.Location = new System.Drawing.Point(381, 530);
            this.labelWizard.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelWizard.Name = "labelWizard";
            this.labelWizard.Size = new System.Drawing.Size(100, 23);
            this.labelWizard.TabIndex = 530;
            this.labelWizard.Text = "Wizard";
            this.labelWizard.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSteerWizard
            // 
            this.btnSteerWizard.BackColor = System.Drawing.Color.Transparent;
            this.btnSteerWizard.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSteerWizard.FlatAppearance.BorderSize = 0;
            this.btnSteerWizard.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnSteerWizard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerWizard.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnSteerWizard.Image = global::AgOpenGPS.Properties.Resources.WizardWand;
            this.btnSteerWizard.Location = new System.Drawing.Point(393, 553);
            this.btnSteerWizard.Name = "btnSteerWizard";
            this.btnSteerWizard.Size = new System.Drawing.Size(75, 69);
            this.btnSteerWizard.TabIndex = 529;
            this.btnSteerWizard.UseVisualStyleBackColor = false;
            this.btnSteerWizard.Click += new System.EventHandler(this.btnSteerWizard_Click);
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.AliceBlue;
            this.button2.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.button2.Image = global::AgOpenGPS.Properties.Resources.Reset_Default;
            this.button2.Location = new System.Drawing.Point(523, 554);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 69);
            this.button2.TabIndex = 522;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnVehicleReset_Click);
            // 
            // pboxSendSteer
            // 
            this.pboxSendSteer.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConSt_Mandatory1;
            this.pboxSendSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pboxSendSteer.Location = new System.Drawing.Point(753, 560);
            this.pboxSendSteer.Name = "pboxSendSteer";
            this.pboxSendSteer.Size = new System.Drawing.Size(61, 62);
            this.pboxSendSteer.TabIndex = 509;
            this.pboxSendSteer.TabStop = false;
            this.pboxSendSteer.Visible = false;
            // 
            // btnSendSteerConfigPGN
            // 
            this.btnSendSteerConfigPGN.BackColor = System.Drawing.Color.White;
            this.btnSendSteerConfigPGN.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnSendSteerConfigPGN.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSendSteerConfigPGN.Image = global::AgOpenGPS.Properties.Resources.ToolAcceptChange;
            this.btnSendSteerConfigPGN.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSendSteerConfigPGN.Location = new System.Drawing.Point(643, 560);
            this.btnSendSteerConfigPGN.Name = "btnSendSteerConfigPGN";
            this.btnSendSteerConfigPGN.Size = new System.Drawing.Size(103, 62);
            this.btnSendSteerConfigPGN.TabIndex = 501;
            this.btnSendSteerConfigPGN.UseVisualStyleBackColor = false;
            this.btnSendSteerConfigPGN.Click += new System.EventHandler(this.btnSendSteerConfigPGN_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.Transparent;
            this.btnClose.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Silver;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.btnClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnClose.Location = new System.Drawing.Point(824, 558);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 64);
            this.btnClose.TabIndex = 531;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FormSteer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(898, 628);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.labelWizard);
            this.Controls.Add(this.btnSteerWizard);
            this.Controls.Add(this.tabSteerSettings);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelReset);
            this.Controls.Add(this.labelSendAndSave);
            this.Controls.Add(this.pboxSendSteer);
            this.Controls.Add(this.btnSendSteerConfigPGN);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(918, 673);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(387, 478);
            this.Name = "FormSteer";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Steer Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSteer_FormClosing);
            this.Load += new System.EventHandler(this.FormSteer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPP.ResumeLayout(false);
            this.tabPP.PerformLayout();
            this.tabStan.ResumeLayout(false);
            this.tabStan.PerformLayout();
            this.tabSteer.ResumeLayout(false);
            this.tabSteer.PerformLayout();
            this.tabPPAdv.ResumeLayout(false);
            this.tabPPAdv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadZoneDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeadZoneHeading)).EndInit();
            this.tabGain.ResumeLayout(false);
            this.tabGain.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabSteerSettings.ResumeLayout(false);
            this.tabSensors.ResumeLayout(false);
            this.tabSensors.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxCounts)).EndInit();
            this.tabConfig.ResumeLayout(false);
            this.tabConfig.PerformLayout();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.tabAlarm.ResumeLayout(false);
            this.tabAlarm.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox17)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox16)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox10)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinSteerSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxSteerSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGuidanceSpeedLimit)).EndInit();
            this.tabOnTheLine.ResumeLayout(false);
            this.tabOnTheLine.PerformLayout();
            this.groupboxlabelGuidanceBar.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox12)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudcmPerPixel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLineWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSnapDistance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGuidanceLookAhead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pboxSendSteer)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnFreeDrive;
        private System.Windows.Forms.Label lblHighSteerPWM;
        private System.Windows.Forms.HScrollBar hsbarHighSteerPWM;
        private System.Windows.Forms.Label lblProportionalGain;
        private System.Windows.Forms.HScrollBar hsbarProportionalGain;
        private System.Windows.Forms.Label lblMinPWM;
        private System.Windows.Forms.HScrollBar hsbarMinPWM;
        private System.Windows.Forms.Label lblMaxSteerAngle;
        private System.Windows.Forms.HScrollBar hsbarMaxSteerAngle;
        private System.Windows.Forms.HScrollBar hsbarLookAheadMult;
        private System.Windows.Forms.Label lblLookAheadMult;
        private System.Windows.Forms.HScrollBar hsbarHeadingErrorGain;
        private System.Windows.Forms.Label lblStanleyGain;
        private System.Windows.Forms.HScrollBar hsbarStanleyGain;
        private System.Windows.Forms.Label labelDistance;
        private System.Windows.Forms.Label labelHeading;
        private System.Windows.Forms.Label lblHeadingErrorGain;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblSteerAngle;
        private System.Windows.Forms.Label lblSteerAngleActual;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Button btnFreeDriveZero;
        private AgLibrary.Controls.RepeatButton btnSteerAngleDown;
        private AgLibrary.Controls.RepeatButton btnSteerAngleUp;
        private System.Windows.Forms.Label lblPWMDisplay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGain;
        private System.Windows.Forms.TabPage tabStan;
        private System.Windows.Forms.TabPage tabPP;
        private System.Windows.Forms.Button btnStartSA;
        private System.Windows.Forms.Label lblCalcSteerAngleInner;
        private System.Windows.Forms.Label lblDiameter;
        private System.Windows.Forms.Label labelSteerAngle;
        private System.Windows.Forms.Label labelDiameter;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label labelMinToMove;
        private System.Windows.Forms.Label labelMaxLimit;
        private System.Windows.Forms.Label labelProportionalGain;
        private System.Windows.Forms.Label labelIntegralPP;
        private System.Windows.Forms.Label labelMaxSteerAngle;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label labelSpeedFactor;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tabSteer;
        private System.Windows.Forms.Label labelAckermann;
        private System.Windows.Forms.Label labelCountsPerDegree;
        private System.Windows.Forms.HScrollBar hsbarAckerman;
        private System.Windows.Forms.Label lblAckerman;
        private System.Windows.Forms.ProgressBar pbarRight;
        private System.Windows.Forms.ProgressBar pbarLeft;
        private System.Windows.Forms.Label lblActualSteerAngleUpper;
        private System.Windows.Forms.Button btnZeroWAS;
        private System.Windows.Forms.HScrollBar hsbarCountsPerDegree;
        private System.Windows.Forms.Label lblCountsPerDegree;
        private System.Windows.Forms.HScrollBar hsbarWasOffset;
        private System.Windows.Forms.Label lblSteerAngleSensorZero;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label labelIntergralStanley;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.HScrollBar hsbarIntegral;
        private System.Windows.Forms.HScrollBar hsbarIntegralPurePursuit;
        private System.Windows.Forms.Label lblIntegralPercent;
        private System.Windows.Forms.Label lblPureIntegral;
        private System.Windows.Forms.Label lblSideHillComp;
        private System.Windows.Forms.HScrollBar hsbarSideHillComp;
        private System.Windows.Forms.Label labelSideHill;
        private System.Windows.Forms.ProgressBar pbarSensor;
        private System.Windows.Forms.Label lblPercentFS;
        private System.Windows.Forms.Label labelSteerDescription;
        private System.Windows.Forms.Label labelPressureTurnSensor;
        private System.Windows.Forms.Label labelCurrentTurnSensor;
        private System.Windows.Forms.Label labelEncoder;
        private System.Windows.Forms.Label labelInvertMotor;
        private System.Windows.Forms.Label labelInvertRelays;
        private System.Windows.Forms.Label labelSendAndSave;
        private System.Windows.Forms.ComboBox cboxMotorDrive;
        private System.Windows.Forms.ComboBox cboxSteerEnable;
        private System.Windows.Forms.Label labelSteerEnable;
        private System.Windows.Forms.ComboBox cboxConv;
        private System.Windows.Forms.Label labelMotorDriver;
        private System.Windows.Forms.Label labelADConverter;
        private System.Windows.Forms.Label labelTurnSensor;
        private NudlessNumericUpDown nudMaxCounts;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.CheckBox cboxCurrentSensor;
        private System.Windows.Forms.CheckBox cboxPressureSensor;
        private System.Windows.Forms.PictureBox pboxSendSteer;
        private System.Windows.Forms.CheckBox cboxDanfoss;
        private System.Windows.Forms.CheckBox chkSteerInvertRelays;
        private System.Windows.Forms.CheckBox chkInvertSteer;
        private System.Windows.Forms.CheckBox cboxEncoder;
        private System.Windows.Forms.CheckBox chkInvertWAS;
        private System.Windows.Forms.Button btnSendSteerConfigPGN;
        private System.Windows.Forms.Label labelInvertWas;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hsbarSensor;
        private System.Windows.Forms.Label lblhsbarSensor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label labelReset;
        private System.Windows.Forms.ComboBox cboxXY;
        private System.Windows.Forms.Label labelIMUAxis;
        private System.Windows.Forms.Label label34;
        public System.Windows.Forms.Button btnStanleyPure;
        private System.Windows.Forms.Label lblAV_Set;
        private System.Windows.Forms.Label lblAV_Act;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label label38;
        private AgLibrary.Controls.RepeatButton btnExpand;
        private System.Windows.Forms.TabControl tabSteerSettings;
        private System.Windows.Forms.TabPage tabSensors;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.Button btnSteerWizard;
        private System.Windows.Forms.Label labelWizard;
        private System.Windows.Forms.HScrollBar hsbarUTurnCompensation;
        private System.Windows.Forms.Label lblUTurnCompensation;
        private System.Windows.Forms.Label labelUturnCompensation;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.TabPage tabAlarm;
        private NudlessNumericUpDown nudMinSteerSpeed;
        private System.Windows.Forms.Label labelMinSpeed;
        private System.Windows.Forms.Label label166;
        private System.Windows.Forms.Label labelMaxSpeed;
        private System.Windows.Forms.Label label163;
        private System.Windows.Forms.Label label160;
        private System.Windows.Forms.Label labelManualTurns;
        private NudlessNumericUpDown nudMaxSteerSpeed;
        private NudlessNumericUpDown nudGuidanceSpeedLimit;
        private System.Windows.Forms.PictureBox pictureBox17;
        private System.Windows.Forms.PictureBox pictureBox16;
        private System.Windows.Forms.PictureBox pictureBox10;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.Label labelNextGuidanceLine;
        private NudlessNumericUpDown nudGuidanceLookAhead;
        private System.Windows.Forms.PictureBox pictureBox12;
        private System.Windows.Forms.Label labelNudgeDistance;
        private System.Windows.Forms.Label label43;
        private NudlessNumericUpDown nudSnapDistance;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label labelLineWidth;
        private System.Windows.Forms.Label label44;
        private NudlessNumericUpDown nudLineWidth;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label labelCmPix;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.PictureBox pictureBox1;
        private NudlessNumericUpDown nudcmPerPixel;
        private System.Windows.Forms.Label labelSteerInReverse;
        private System.Windows.Forms.CheckBox cboxSteerInReverse;
        private System.Windows.Forms.Label labelHeadingDegree;
        private NudlessNumericUpDown nudDeadZoneHeading;
        private System.Windows.Forms.TabPage tabOnTheLine;
        private System.Windows.Forms.GroupBox groupboxlabelGuidanceBar;
        private System.Windows.Forms.RadioButton rbtnSteerBar;
        private System.Windows.Forms.Label labelSteerBar;
        private System.Windows.Forms.RadioButton rbtnLightBar;
        private System.Windows.Forms.Label labelLightbar;
        private System.Windows.Forms.CheckBox chkDisplayLightbar;
        private System.Windows.Forms.Label labelSlow;
        private System.Windows.Forms.Label labelFast;
        private System.Windows.Forms.Label labelSteerResponse;
        private System.Windows.Forms.Label lblHoldLookAhead;
        private System.Windows.Forms.HScrollBar hsbarHoldLookAhead;
        private System.Windows.Forms.Label labelIntegralInfo;
        private System.Windows.Forms.Label labelOnOff;
        private System.Windows.Forms.Label labelOnDelay;
        private NudlessNumericUpDown nudDeadZoneDelay;
        private System.Windows.Forms.Label labelDeadzone;
        private System.Windows.Forms.Label labelHold;
        private System.Windows.Forms.Label labelAquireDescription;
        private System.Windows.Forms.Label lblAcquireFactor;
        private System.Windows.Forms.HScrollBar hsbarAcquireFactor;
        private System.Windows.Forms.Label lblHoldAdv;
        private System.Windows.Forms.Label lblAcqAdv;
        private System.Windows.Forms.Label labelAquire2;
        private System.Windows.Forms.Label lblDistanceAdv;
        private System.Windows.Forms.Label labelDist;
        private System.Windows.Forms.TabPage tabPPAdv;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label labelAquireFactor;
        private System.Windows.Forms.Label labelAquire;
        private System.Windows.Forms.Label lblAcquirePP;
        private System.Windows.Forms.Label labelWasZero;
        private System.Windows.Forms.Button btnClose;
    }
}
namespace AgOpenGPS
{
    partial class FormSteerWiz
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSteerWiz));
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
            this.label8 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblCalcSteerAngleInner = new System.Windows.Forms.Label();
            this.lblDiameter = new System.Windows.Forms.Label();
            this.pbarSensor = new System.Windows.Forms.ProgressBar();
            this.lblPercentFS = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabSteer = new System.Windows.Forms.TabPage();
            this.hsbarLowSteerPWM = new System.Windows.Forms.HScrollBar();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLowSteerPWM = new System.Windows.Forms.Label();
            this.tabGain = new System.Windows.Forms.TabPage();
            this.tabStan = new System.Windows.Forms.TabPage();
            this.lblIntegralPercent = new System.Windows.Forms.Label();
            this.hsbarIntegral = new System.Windows.Forms.HScrollBar();
            this.label27 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.lblHeadingErrorGain = new System.Windows.Forms.Label();
            this.lblStanleyGain = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.hsbarStanleyGain = new System.Windows.Forms.HScrollBar();
            this.hsbarHeadingErrorGain = new System.Windows.Forms.HScrollBar();
            this.tabInt = new System.Windows.Forms.TabPage();
            this.lblSideHillComp = new System.Windows.Forms.Label();
            this.hsbarSideHillComp = new System.Windows.Forms.HScrollBar();
            this.label22 = new System.Windows.Forms.Label();
            this.lblPureIntegral = new System.Windows.Forms.Label();
            this.hsbarIntegralPurePursuit = new System.Windows.Forms.HScrollBar();
            this.label26 = new System.Windows.Forms.Label();
            this.label24 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.hsbarLookAheadMult = new System.Windows.Forms.HScrollBar();
            this.lblLookAhead = new System.Windows.Forms.Label();
            this.lblLookAheadMult = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label15 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.hsbarMinPWM = new System.Windows.Forms.HScrollBar();
            this.hsbarProportionalGain = new System.Windows.Forms.HScrollBar();
            this.lblProportionalGain = new System.Windows.Forms.Label();
            this.lblHighSteerPWM = new System.Windows.Forms.Label();
            this.lblMinPWM = new System.Windows.Forms.Label();
            this.hsbarHighSteerPWM = new System.Windows.Forms.HScrollBar();
            this.lblMaxSteerAngle = new System.Windows.Forms.Label();
            this.hsbarMaxSteerAngle = new System.Windows.Forms.HScrollBar();
            this.label7 = new System.Windows.Forms.Label();
            this.hsbarCountsPerDegree = new System.Windows.Forms.HScrollBar();
            this.label10 = new System.Windows.Forms.Label();
            this.lblCountsPerDegree = new System.Windows.Forms.Label();
            this.hsbarWasOffset = new System.Windows.Forms.HScrollBar();
            this.lblSteerAngleSensorZero = new System.Windows.Forms.Label();
            this.pbarRight = new System.Windows.Forms.ProgressBar();
            this.pbarLeft = new System.Windows.Forms.ProgressBar();
            this.label28 = new System.Windows.Forms.Label();
            this.labelPressureTurnSensor = new System.Windows.Forms.Label();
            this.labelCurrentTurnSensor = new System.Windows.Forms.Label();
            this.labelEncoder = new System.Windows.Forms.Label();
            this.label70 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.cboxMotorDrive = new System.Windows.Forms.ComboBox();
            this.cboxSteerEnable = new System.Windows.Forms.ComboBox();
            this.label62 = new System.Windows.Forms.Label();
            this.cboxConv = new System.Windows.Forms.ComboBox();
            this.label63 = new System.Windows.Forms.Label();
            this.label64 = new System.Windows.Forms.Label();
            this.label61 = new System.Windows.Forms.Label();
            this.nudMaxCounts = new AgOpenGPS.NudlessNumericUpDown();
            this.label55 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnSteerStatus = new System.Windows.Forms.Button();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label38 = new System.Windows.Forms.Label();
            this.btnFreeDriveZero = new System.Windows.Forms.Button();
            this.btnSteerAngleUp = new AgLibrary.Controls.RepeatButton();
            this.btnFreeDrive = new System.Windows.Forms.Button();
            this.btnSteerAngleDown = new AgLibrary.Controls.RepeatButton();
            this.hsbarSensor = new System.Windows.Forms.HScrollBar();
            this.lblhsbarSensor = new System.Windows.Forms.Label();
            this.tabWiz = new System.Windows.Forms.TabControl();
            this.tabStart = new System.Windows.Forms.TabPage();
            this.btnStartWizard = new System.Windows.Forms.Button();
            this.tabLoadDef = new System.Windows.Forms.TabPage();
            this.label81 = new System.Windows.Forms.Label();
            this.label80 = new System.Windows.Forms.Label();
            this.btnOkNext_LoadDefault = new System.Windows.Forms.Button();
            this.btnLoadDefaults = new System.Windows.Forms.Button();
            this.tabWheelBase = new System.Windows.Forms.TabPage();
            this.label117 = new System.Windows.Forms.Label();
            this.label89 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label90 = new System.Windows.Forms.Label();
            this.nudWheelbase = new AgOpenGPS.NudlessNumericUpDown();
            this.label91 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.tabWheelTrack = new System.Windows.Forms.TabPage();
            this.label118 = new System.Windows.Forms.Label();
            this.nudVehicleTrack = new AgOpenGPS.NudlessNumericUpDown();
            this.label95 = new System.Windows.Forms.Label();
            this.label96 = new System.Windows.Forms.Label();
            this.button7 = new System.Windows.Forms.Button();
            this.label94 = new System.Windows.Forms.Label();
            this.button8 = new System.Windows.Forms.Button();
            this.tabAntennaDistance = new System.Windows.Forms.TabPage();
            this.label100 = new System.Windows.Forms.Label();
            this.button9 = new System.Windows.Forms.Button();
            this.label98 = new System.Windows.Forms.Label();
            this.nudAntennaPivot = new AgOpenGPS.NudlessNumericUpDown();
            this.button10 = new System.Windows.Forms.Button();
            this.label99 = new System.Windows.Forms.Label();
            this.tabAntennaHeight = new System.Windows.Forms.TabPage();
            this.label114 = new System.Windows.Forms.Label();
            this.label97 = new System.Windows.Forms.Label();
            this.button13 = new System.Windows.Forms.Button();
            this.label92 = new System.Windows.Forms.Label();
            this.nudAntennaHeight = new AgOpenGPS.NudlessNumericUpDown();
            this.label93 = new System.Windows.Forms.Label();
            this.button14 = new System.Windows.Forms.Button();
            this.tabAntennaOffset = new System.Windows.Forms.TabPage();
            this.button11 = new System.Windows.Forms.Button();
            this.label102 = new System.Windows.Forms.Label();
            this.nudAntennaOffset = new AgOpenGPS.NudlessNumericUpDown();
            this.label104 = new System.Windows.Forms.Label();
            this.button12 = new System.Windows.Forms.Button();
            this.label103 = new System.Windows.Forms.Label();
            this.tabButtonSwitch = new System.Windows.Forms.TabPage();
            this.button15 = new System.Windows.Forms.Button();
            this.label53 = new System.Windows.Forms.Label();
            this.btnOkNext_ButtonSwitch = new System.Windows.Forms.Button();
            this.tabA2DConv = new System.Windows.Forms.TabPage();
            this.btnPrev_A2D = new System.Windows.Forms.Button();
            this.label52 = new System.Windows.Forms.Label();
            this.btnOkNext_A2D = new System.Windows.Forms.Button();
            this.tabMotorDriver = new System.Windows.Forms.TabPage();
            this.btnPrev_MotorDriver = new System.Windows.Forms.Button();
            this.label43 = new System.Windows.Forms.Label();
            this.btnOkNext_MotorDriver = new System.Windows.Forms.Button();
            this.tabInvertRelays = new System.Windows.Forms.TabPage();
            this.label59 = new System.Windows.Forms.Label();
            this.btnPrev_InvertRelays = new System.Windows.Forms.Button();
            this.label51 = new System.Windows.Forms.Label();
            this.btnOkNext_InvertRelays = new System.Windows.Forms.Button();
            this.chkSteerInvertRelays = new System.Windows.Forms.CheckBox();
            this.tabDanfoss = new System.Windows.Forms.TabPage();
            this.btnPrev_Danfoss = new System.Windows.Forms.Button();
            this.label50 = new System.Windows.Forms.Label();
            this.btnOkNext_Danfoss = new System.Windows.Forms.Button();
            this.cboxDanfoss = new System.Windows.Forms.CheckBox();
            this.tabRollInv = new System.Windows.Forms.TabPage();
            this.label115 = new System.Windows.Forms.Label();
            this.lblRoll = new System.Windows.Forms.Label();
            this.pictureBox9 = new System.Windows.Forms.PictureBox();
            this.cboxDataInvertRoll = new System.Windows.Forms.CheckBox();
            this.label36 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label46 = new System.Windows.Forms.Label();
            this.button18 = new System.Windows.Forms.Button();
            this.label107 = new System.Windows.Forms.Label();
            this.tabRollZero = new System.Windows.Forms.TabPage();
            this.lblRoll2 = new System.Windows.Forms.Label();
            this.label110 = new System.Windows.Forms.Label();
            this.label47 = new System.Windows.Forms.Label();
            this.label57 = new System.Windows.Forms.Label();
            this.lblRollZeroOffset = new System.Windows.Forms.Label();
            this.btnZeroRoll = new System.Windows.Forms.Button();
            this.btnRemoveZeroOffset = new System.Windows.Forms.Button();
            this.label48 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label56 = new System.Windows.Forms.Label();
            this.button19 = new System.Windows.Forms.Button();
            this.tabWAS = new System.Windows.Forms.TabPage();
            this.label45 = new System.Windows.Forms.Label();
            this.btnPrev_InvertWAS = new System.Windows.Forms.Button();
            this.label42 = new System.Windows.Forms.Label();
            this.btnOkWAS = new System.Windows.Forms.Button();
            this.chkInvertWAS = new System.Windows.Forms.CheckBox();
            this.tabWAS_Zero = new System.Windows.Forms.TabPage();
            this.label112 = new System.Windows.Forms.Label();
            this.lblCurrentHeading = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.button6 = new System.Windows.Forms.Button();
            this.btnRemoveWasOffset = new System.Windows.Forms.Button();
            this.label40 = new System.Windows.Forms.Label();
            this.btnOkNext_WAS_Zero = new System.Windows.Forms.Button();
            this.btnZeroWAS = new System.Windows.Forms.Button();
            this.tabMotorDirection = new System.Windows.Forms.TabPage();
            this.label58 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.btnPrev_MotorDirection = new System.Windows.Forms.Button();
            this.btnOkNext_MotorDirection = new System.Windows.Forms.Button();
            this.btnSteerLeft = new System.Windows.Forms.Button();
            this.btnSteerRight = new System.Windows.Forms.Button();
            this.chkInvertSteer = new System.Windows.Forms.CheckBox();
            this.tabCPD_Setup = new System.Windows.Forms.TabPage();
            this.label60 = new System.Windows.Forms.Label();
            this.btnPrev_CountsPerDegree = new System.Windows.Forms.Button();
            this.label74 = new System.Windows.Forms.Label();
            this.label75 = new System.Windows.Forms.Label();
            this.btnSkipCPD_Setup = new System.Windows.Forms.Button();
            this.btnOKNext_CPDSetup = new System.Windows.Forms.Button();
            this.tabCountsPerDeg = new System.Windows.Forms.TabPage();
            this.label109 = new System.Windows.Forms.Label();
            this.lblRightStartAngle = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.lblCPDError = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.btnOkNext_CountsPerDeg = new System.Windows.Forms.Button();
            this.btnStartSA = new System.Windows.Forms.Button();
            this.tabAckCPD = new System.Windows.Forms.TabPage();
            this.btnAckReset = new System.Windows.Forms.Button();
            this.label101 = new System.Windows.Forms.Label();
            this.lblStartAngleLeft = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.hsbarAckerman = new System.Windows.Forms.HScrollBar();
            this.lblAckerman = new System.Windows.Forms.Label();
            this.button16 = new System.Windows.Forms.Button();
            this.lblAckermannError = new System.Windows.Forms.Label();
            this.label105 = new System.Windows.Forms.Label();
            this.label106 = new System.Windows.Forms.Label();
            this.label108 = new System.Windows.Forms.Label();
            this.lblDiameterLeft = new System.Windows.Forms.Label();
            this.lblCalcSteerAngleLeft = new System.Windows.Forms.Label();
            this.label111 = new System.Windows.Forms.Label();
            this.button17 = new System.Windows.Forms.Button();
            this.btnStartSA_Left = new System.Windows.Forms.Button();
            this.tabMaxSteerAngle = new System.Windows.Forms.TabPage();
            this.label113 = new System.Windows.Forms.Label();
            this.btnOkSetMaximumSteerAngle = new System.Windows.Forms.Button();
            this.label73 = new System.Windows.Forms.Label();
            this.btnPrev_MaxSteerAngle = new System.Windows.Forms.Button();
            this.label49 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.btnOkNextMaxSteerAngle = new System.Windows.Forms.Button();
            this.tabCancelGuidance = new System.Windows.Forms.TabPage();
            this.btnPrev_CancelGuidance = new System.Windows.Forms.Button();
            this.btnOkNextCancelGuidance = new System.Windows.Forms.Button();
            this.label44 = new System.Windows.Forms.Label();
            this.cboxPressureSensor = new System.Windows.Forms.CheckBox();
            this.cboxEncoder = new System.Windows.Forms.CheckBox();
            this.cboxCurrentSensor = new System.Windows.Forms.CheckBox();
            this.tabPanicStop = new System.Windows.Forms.TabPage();
            this.btnPrev_Panic = new System.Windows.Forms.Button();
            this.label54 = new System.Windows.Forms.Label();
            this.btnOkNext_PanicStop = new System.Windows.Forms.Button();
            this.tab_MinimumGain = new System.Windows.Forms.TabPage();
            this.label72 = new System.Windows.Forms.Label();
            this.label79 = new System.Windows.Forms.Label();
            this.label78 = new System.Windows.Forms.Label();
            this.btnMinGainLeft = new System.Windows.Forms.Button();
            this.btnZeroMinMovementSetting = new System.Windows.Forms.Button();
            this.label77 = new System.Windows.Forms.Label();
            this.label76 = new System.Windows.Forms.Label();
            this.btnMinGainRight = new System.Windows.Forms.Button();
            this.label68 = new System.Windows.Forms.Label();
            this.btnPrev_Gain = new System.Windows.Forms.Button();
            this.btnOK_Next = new System.Windows.Forms.Button();
            this.tabPGain = new System.Windows.Forms.TabPage();
            this.label88 = new System.Windows.Forms.Label();
            this.label83 = new System.Windows.Forms.Label();
            this.label84 = new System.Windows.Forms.Label();
            this.btnLeftPGain = new System.Windows.Forms.Button();
            this.btnZeroPGain = new System.Windows.Forms.Button();
            this.label85 = new System.Windows.Forms.Label();
            this.label86 = new System.Windows.Forms.Label();
            this.btnRightPGain = new System.Windows.Forms.Button();
            this.label87 = new System.Windows.Forms.Label();
            this.btnPrev_PGain = new System.Windows.Forms.Button();
            this.btnNext_PGain = new System.Windows.Forms.Button();
            this.tabEnd = new System.Windows.Forms.TabPage();
            this.label116 = new System.Windows.Forms.Label();
            this.btnRestartWizard = new System.Windows.Forms.Button();
            this.label69 = new System.Windows.Forms.Label();
            this.btnPrev_End = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnStopWizard = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.pbarProgress = new System.Windows.Forms.ProgressBar();
            this.label71 = new System.Windows.Forms.Label();
            this.lblBarCPD = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.lblBarAck = new System.Windows.Forms.Label();
            this.sideBarTimer = new System.Windows.Forms.Timer(this.components);
            this.label67 = new System.Windows.Forms.Label();
            this.lblBarWasOffset = new System.Windows.Forms.Label();
            this.btnCloseAll = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabSteer.SuspendLayout();
            this.tabStan.SuspendLayout();
            this.tabInt.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxCounts)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabWiz.SuspendLayout();
            this.tabStart.SuspendLayout();
            this.tabLoadDef.SuspendLayout();
            this.tabWheelBase.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheelbase)).BeginInit();
            this.tabWheelTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVehicleTrack)).BeginInit();
            this.tabAntennaDistance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaPivot)).BeginInit();
            this.tabAntennaHeight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaHeight)).BeginInit();
            this.tabAntennaOffset.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaOffset)).BeginInit();
            this.tabButtonSwitch.SuspendLayout();
            this.tabA2DConv.SuspendLayout();
            this.tabMotorDriver.SuspendLayout();
            this.tabInvertRelays.SuspendLayout();
            this.tabDanfoss.SuspendLayout();
            this.tabRollInv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).BeginInit();
            this.tabRollZero.SuspendLayout();
            this.tabWAS.SuspendLayout();
            this.tabWAS_Zero.SuspendLayout();
            this.tabMotorDirection.SuspendLayout();
            this.tabCPD_Setup.SuspendLayout();
            this.tabCountsPerDeg.SuspendLayout();
            this.tabAckCPD.SuspendLayout();
            this.tabMaxSteerAngle.SuspendLayout();
            this.tabCancelGuidance.SuspendLayout();
            this.tabPanicStop.SuspendLayout();
            this.tab_MinimumGain.SuspendLayout();
            this.tabPGain.SuspendLayout();
            this.tabEnd.SuspendLayout();
            this.panel3.SuspendLayout();
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
            this.lblSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngle.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngle.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngle.Location = new System.Drawing.Point(278, 4);
            this.lblSteerAngle.Name = "lblSteerAngle";
            this.lblSteerAngle.Size = new System.Drawing.Size(100, 31);
            this.lblSteerAngle.TabIndex = 306;
            this.lblSteerAngle.Text = "-55.5";
            this.lblSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSteerAngleActual
            // 
            this.lblSteerAngleActual.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngleActual.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleActual.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngleActual.Location = new System.Drawing.Point(484, 4);
            this.lblSteerAngleActual.Name = "lblSteerAngleActual";
            this.lblSteerAngleActual.Size = new System.Drawing.Size(100, 31);
            this.lblSteerAngleActual.TabIndex = 311;
            this.lblSteerAngleActual.Text = "-55.5";
            this.lblSteerAngleActual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.ForeColor = System.Drawing.Color.Black;
            this.lblError.Location = new System.Drawing.Point(352, 84);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(101, 31);
            this.lblError.TabIndex = 312;
            this.lblError.Text = "-30.0";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblPWMDisplay
            // 
            this.lblPWMDisplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPWMDisplay.BackColor = System.Drawing.Color.White;
            this.lblPWMDisplay.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPWMDisplay.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblPWMDisplay.Location = new System.Drawing.Point(607, 96);
            this.lblPWMDisplay.Name = "lblPWMDisplay";
            this.lblPWMDisplay.Size = new System.Drawing.Size(46, 23);
            this.lblPWMDisplay.TabIndex = 316;
            this.lblPWMDisplay.Text = "255";
            this.lblPWMDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.BackColor = System.Drawing.Color.White;
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(547, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(64, 23);
            this.label9.TabIndex = 318;
            this.label9.Text = "PWM:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(411, 4);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 33);
            this.label11.TabIndex = 319;
            this.label11.Text = "WAS:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(217, 2);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 33);
            this.label12.TabIndex = 320;
            this.label12.Text = "Set:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(286, 84);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(75, 33);
            this.label13.TabIndex = 321;
            this.label13.Text = "Error:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label14.Location = new System.Drawing.Point(355, 71);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 16);
            this.label14.TabIndex = 322;
            this.label14.Text = "Or +5";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label8.Location = new System.Drawing.Point(296, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(128, 25);
            this.label8.TabIndex = 328;
            this.label8.Text = "Steer Angle:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(320, 189);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 25);
            this.label1.TabIndex = 327;
            this.label1.Text = "Diameter:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCalcSteerAngleInner
            // 
            this.lblCalcSteerAngleInner.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCalcSteerAngleInner.AutoSize = true;
            this.lblCalcSteerAngleInner.BackColor = System.Drawing.Color.Transparent;
            this.lblCalcSteerAngleInner.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalcSteerAngleInner.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCalcSteerAngleInner.Location = new System.Drawing.Point(419, 158);
            this.lblCalcSteerAngleInner.Name = "lblCalcSteerAngleInner";
            this.lblCalcSteerAngleInner.Size = new System.Drawing.Size(45, 25);
            this.lblCalcSteerAngleInner.TabIndex = 326;
            this.lblCalcSteerAngleInner.Text = "0.0";
            this.lblCalcSteerAngleInner.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDiameter
            // 
            this.lblDiameter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDiameter.AutoSize = true;
            this.lblDiameter.BackColor = System.Drawing.Color.Transparent;
            this.lblDiameter.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiameter.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDiameter.Location = new System.Drawing.Point(419, 190);
            this.lblDiameter.Name = "lblDiameter";
            this.lblDiameter.Size = new System.Drawing.Size(71, 25);
            this.lblDiameter.TabIndex = 325;
            this.lblDiameter.Text = "0.0 m";
            this.lblDiameter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pbarSensor
            // 
            this.pbarSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbarSensor.Location = new System.Drawing.Point(37, 295);
            this.pbarSensor.Maximum = 255;
            this.pbarSensor.Name = "pbarSensor";
            this.pbarSensor.Size = new System.Drawing.Size(280, 26);
            this.pbarSensor.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbarSensor.TabIndex = 496;
            // 
            // lblPercentFS
            // 
            this.lblPercentFS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblPercentFS.AutoSize = true;
            this.lblPercentFS.BackColor = System.Drawing.Color.Transparent;
            this.lblPercentFS.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPercentFS.ForeColor = System.Drawing.Color.Black;
            this.lblPercentFS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblPercentFS.Location = new System.Drawing.Point(336, 295);
            this.lblPercentFS.Name = "lblPercentFS";
            this.lblPercentFS.Size = new System.Drawing.Size(57, 29);
            this.lblPercentFS.TabIndex = 495;
            this.lblPercentFS.Text = "0%";
            this.lblPercentFS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabSteer);
            this.tabControl1.Controls.Add(this.tabGain);
            this.tabControl1.Controls.Add(this.tabStan);
            this.tabControl1.Controls.Add(this.tabInt);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ImageList = this.imageList1;
            this.tabControl1.ItemSize = new System.Drawing.Size(86, 48);
            this.tabControl1.Location = new System.Drawing.Point(988, 204);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Multiline = true;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(356, 379);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 347;
            // 
            // tabSteer
            // 
            this.tabSteer.AutoScroll = true;
            this.tabSteer.BackColor = System.Drawing.Color.Gainsboro;
            this.tabSteer.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_SteerTab;
            this.tabSteer.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabSteer.Controls.Add(this.hsbarLowSteerPWM);
            this.tabSteer.Controls.Add(this.label6);
            this.tabSteer.Controls.Add(this.lblLowSteerPWM);
            this.tabSteer.ImageIndex = 0;
            this.tabSteer.Location = new System.Drawing.Point(4, 52);
            this.tabSteer.Name = "tabSteer";
            this.tabSteer.Size = new System.Drawing.Size(348, 323);
            this.tabSteer.TabIndex = 5;
            // 
            // hsbarLowSteerPWM
            // 
            this.hsbarLowSteerPWM.LargeChange = 1;
            this.hsbarLowSteerPWM.Location = new System.Drawing.Point(65, 225);
            this.hsbarLowSteerPWM.Maximum = 255;
            this.hsbarLowSteerPWM.Name = "hsbarLowSteerPWM";
            this.hsbarLowSteerPWM.Size = new System.Drawing.Size(195, 30);
            this.hsbarLowSteerPWM.TabIndex = 269;
            this.hsbarLowSteerPWM.Value = 1;
            this.hsbarLowSteerPWM.ValueChanged += new System.EventHandler(this.hsbarLowSteerPWM_ValueChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(196, 203);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 19);
            this.label6.TabIndex = 337;
            this.label6.Text = "Low";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLowSteerPWM
            // 
            this.lblLowSteerPWM.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowSteerPWM.ForeColor = System.Drawing.Color.Black;
            this.lblLowSteerPWM.Location = new System.Drawing.Point(5, 223);
            this.lblLowSteerPWM.Name = "lblLowSteerPWM";
            this.lblLowSteerPWM.Size = new System.Drawing.Size(61, 35);
            this.lblLowSteerPWM.TabIndex = 273;
            this.lblLowSteerPWM.Text = "888";
            this.lblLowSteerPWM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabGain
            // 
            this.tabGain.AutoScroll = true;
            this.tabGain.BackColor = System.Drawing.Color.Gainsboro;
            this.tabGain.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_GainTab;
            this.tabGain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabGain.ImageIndex = 1;
            this.tabGain.Location = new System.Drawing.Point(4, 52);
            this.tabGain.Name = "tabGain";
            this.tabGain.Size = new System.Drawing.Size(348, 323);
            this.tabGain.TabIndex = 13;
            // 
            // tabStan
            // 
            this.tabStan.BackColor = System.Drawing.Color.Gainsboro;
            this.tabStan.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_Stanley;
            this.tabStan.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabStan.Controls.Add(this.lblIntegralPercent);
            this.tabStan.Controls.Add(this.hsbarIntegral);
            this.tabStan.Controls.Add(this.label27);
            this.tabStan.Controls.Add(this.label25);
            this.tabStan.Controls.Add(this.lblHeadingErrorGain);
            this.tabStan.Controls.Add(this.lblStanleyGain);
            this.tabStan.Controls.Add(this.label5);
            this.tabStan.Controls.Add(this.label3);
            this.tabStan.Controls.Add(this.hsbarStanleyGain);
            this.tabStan.Controls.Add(this.hsbarHeadingErrorGain);
            this.tabStan.ImageIndex = 2;
            this.tabStan.Location = new System.Drawing.Point(4, 52);
            this.tabStan.Name = "tabStan";
            this.tabStan.Size = new System.Drawing.Size(348, 323);
            this.tabStan.TabIndex = 15;
            // 
            // lblIntegralPercent
            // 
            this.lblIntegralPercent.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIntegralPercent.ForeColor = System.Drawing.Color.Black;
            this.lblIntegralPercent.Location = new System.Drawing.Point(3, 245);
            this.lblIntegralPercent.Name = "lblIntegralPercent";
            this.lblIntegralPercent.Size = new System.Drawing.Size(60, 35);
            this.lblIntegralPercent.TabIndex = 352;
            this.lblIntegralPercent.Text = "888";
            this.lblIntegralPercent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarIntegral
            // 
            this.hsbarIntegral.LargeChange = 1;
            this.hsbarIntegral.Location = new System.Drawing.Point(68, 247);
            this.hsbarIntegral.Name = "hsbarIntegral";
            this.hsbarIntegral.Size = new System.Drawing.Size(202, 30);
            this.hsbarIntegral.TabIndex = 351;
            this.hsbarIntegral.Value = 5;
            this.hsbarIntegral.ValueChanged += new System.EventHandler(this.hsbarIntegral_ValueChanged);
            // 
            // label27
            // 
            this.label27.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.ForeColor = System.Drawing.Color.Black;
            this.label27.Location = new System.Drawing.Point(63, 218);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(204, 30);
            this.label27.TabIndex = 350;
            this.label27.Text = "Integral";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label27.UseCompatibleTextRendering = true;
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.ForeColor = System.Drawing.Color.SandyBrown;
            this.label25.Location = new System.Drawing.Point(117, 3);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(114, 31);
            this.label25.TabIndex = 347;
            this.label25.Text = "Stanley";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeadingErrorGain
            // 
            this.lblHeadingErrorGain.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeadingErrorGain.ForeColor = System.Drawing.Color.Black;
            this.lblHeadingErrorGain.Location = new System.Drawing.Point(9, 152);
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
            this.lblStanleyGain.Location = new System.Drawing.Point(9, 64);
            this.lblStanleyGain.Name = "lblStanleyGain";
            this.lblStanleyGain.Size = new System.Drawing.Size(54, 35);
            this.lblStanleyGain.TabIndex = 299;
            this.lblStanleyGain.Text = "888";
            this.lblStanleyGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(62, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(204, 30);
            this.label5.TabIndex = 296;
            this.label5.Text = "Heading";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(63, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(204, 28);
            this.label3.TabIndex = 298;
            this.label3.Text = "Distance";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarStanleyGain
            // 
            this.hsbarStanleyGain.LargeChange = 1;
            this.hsbarStanleyGain.Location = new System.Drawing.Point(68, 66);
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
            this.hsbarHeadingErrorGain.Location = new System.Drawing.Point(68, 156);
            this.hsbarHeadingErrorGain.Maximum = 15;
            this.hsbarHeadingErrorGain.Minimum = 1;
            this.hsbarHeadingErrorGain.Name = "hsbarHeadingErrorGain";
            this.hsbarHeadingErrorGain.Size = new System.Drawing.Size(200, 30);
            this.hsbarHeadingErrorGain.TabIndex = 294;
            this.hsbarHeadingErrorGain.Value = 10;
            this.hsbarHeadingErrorGain.ValueChanged += new System.EventHandler(this.hsbarHeadingErrorGain_ValueChanged);
            // 
            // tabInt
            // 
            this.tabInt.BackColor = System.Drawing.Color.Gainsboro;
            this.tabInt.BackgroundImage = global::AgOpenGPS.Properties.Resources.Sf_PP;
            this.tabInt.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabInt.Controls.Add(this.lblSideHillComp);
            this.tabInt.Controls.Add(this.hsbarSideHillComp);
            this.tabInt.Controls.Add(this.label22);
            this.tabInt.Controls.Add(this.lblPureIntegral);
            this.tabInt.Controls.Add(this.hsbarIntegralPurePursuit);
            this.tabInt.Controls.Add(this.label26);
            this.tabInt.Controls.Add(this.label24);
            this.tabInt.Controls.Add(this.label21);
            this.tabInt.Controls.Add(this.label20);
            this.tabInt.Controls.Add(this.label19);
            this.tabInt.Controls.Add(this.label18);
            this.tabInt.Controls.Add(this.hsbarLookAheadMult);
            this.tabInt.Controls.Add(this.lblLookAhead);
            this.tabInt.Controls.Add(this.lblLookAheadMult);
            this.tabInt.ForeColor = System.Drawing.Color.Black;
            this.tabInt.ImageIndex = 3;
            this.tabInt.Location = new System.Drawing.Point(4, 52);
            this.tabInt.Name = "tabInt";
            this.tabInt.Size = new System.Drawing.Size(348, 323);
            this.tabInt.TabIndex = 16;
            // 
            // lblSideHillComp
            // 
            this.lblSideHillComp.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSideHillComp.ForeColor = System.Drawing.Color.Black;
            this.lblSideHillComp.Location = new System.Drawing.Point(6, 209);
            this.lblSideHillComp.Name = "lblSideHillComp";
            this.lblSideHillComp.Size = new System.Drawing.Size(60, 35);
            this.lblSideHillComp.TabIndex = 353;
            this.lblSideHillComp.Text = "888";
            this.lblSideHillComp.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarSideHillComp
            // 
            this.hsbarSideHillComp.LargeChange = 1;
            this.hsbarSideHillComp.Location = new System.Drawing.Point(69, 208);
            this.hsbarSideHillComp.Name = "hsbarSideHillComp";
            this.hsbarSideHillComp.Size = new System.Drawing.Size(217, 30);
            this.hsbarSideHillComp.TabIndex = 352;
            this.hsbarSideHillComp.Value = 5;
            this.hsbarSideHillComp.ValueChanged += new System.EventHandler(this.hsbarSideHillComp_ValueChanged);
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.ForeColor = System.Drawing.Color.Black;
            this.label22.Location = new System.Drawing.Point(73, 187);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(210, 19);
            this.label22.TabIndex = 351;
            this.label22.Text = "Sidehill Deg per Deg of Roll";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label22.UseCompatibleTextRendering = true;
            // 
            // lblPureIntegral
            // 
            this.lblPureIntegral.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPureIntegral.ForeColor = System.Drawing.Color.Black;
            this.lblPureIntegral.Location = new System.Drawing.Point(5, 279);
            this.lblPureIntegral.Name = "lblPureIntegral";
            this.lblPureIntegral.Size = new System.Drawing.Size(60, 35);
            this.lblPureIntegral.TabIndex = 350;
            this.lblPureIntegral.Text = "888";
            this.lblPureIntegral.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarIntegralPurePursuit
            // 
            this.hsbarIntegralPurePursuit.LargeChange = 1;
            this.hsbarIntegralPurePursuit.Location = new System.Drawing.Point(68, 280);
            this.hsbarIntegralPurePursuit.Name = "hsbarIntegralPurePursuit";
            this.hsbarIntegralPurePursuit.Size = new System.Drawing.Size(217, 30);
            this.hsbarIntegralPurePursuit.TabIndex = 349;
            this.hsbarIntegralPurePursuit.Value = 5;
            this.hsbarIntegralPurePursuit.ValueChanged += new System.EventHandler(this.hsbarIntegralPurePursuit_ValueChanged);
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.ForeColor = System.Drawing.Color.SteelBlue;
            this.label26.Location = new System.Drawing.Point(88, 4);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(172, 28);
            this.label26.TabIndex = 348;
            this.label26.Text = "Pure Pursuit";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label24
            // 
            this.label24.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(74, 257);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(204, 19);
            this.label24.TabIndex = 342;
            this.label24.Text = "Integral";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label24.UseCompatibleTextRendering = true;
            // 
            // label21
            // 
            this.label21.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(74, 42);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(204, 19);
            this.label21.TabIndex = 303;
            this.label21.Text = "Look Ahead (m)";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.ForeColor = System.Drawing.Color.Black;
            this.label19.Location = new System.Drawing.Point(74, 114);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(204, 19);
            this.label19.TabIndex = 301;
            this.label19.Text = "Look Ahead Speed Gain";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // hsbarLookAheadMult
            // 
            this.hsbarLookAheadMult.LargeChange = 1;
            this.hsbarLookAheadMult.Location = new System.Drawing.Point(68, 136);
            this.hsbarLookAheadMult.Maximum = 30;
            this.hsbarLookAheadMult.Minimum = 5;
            this.hsbarLookAheadMult.Name = "hsbarLookAheadMult";
            this.hsbarLookAheadMult.Size = new System.Drawing.Size(217, 30);
            this.hsbarLookAheadMult.TabIndex = 298;
            this.hsbarLookAheadMult.Value = 6;
            this.hsbarLookAheadMult.ValueChanged += new System.EventHandler(this.hsbarLookAheadMult_ValueChanged);
            // 
            // lblLookAhead
            // 
            this.lblLookAhead.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAhead.ForeColor = System.Drawing.Color.Black;
            this.lblLookAhead.Location = new System.Drawing.Point(4, 61);
            this.lblLookAhead.Name = "lblLookAhead";
            this.lblLookAhead.Size = new System.Drawing.Size(60, 35);
            this.lblLookAhead.TabIndex = 293;
            this.lblLookAhead.Text = "888";
            this.lblLookAhead.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblLookAheadMult
            // 
            this.lblLookAheadMult.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAheadMult.ForeColor = System.Drawing.Color.Black;
            this.lblLookAheadMult.Location = new System.Drawing.Point(4, 141);
            this.lblLookAheadMult.Name = "lblLookAheadMult";
            this.lblLookAheadMult.Size = new System.Drawing.Size(60, 35);
            this.lblLookAheadMult.TabIndex = 299;
            this.lblLookAheadMult.Text = "888";
            this.lblLookAheadMult.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "ST_SteerTab.png");
            this.imageList1.Images.SetKeyName(1, "ST_GainTab.png");
            this.imageList1.Images.SetKeyName(2, "ST_StanleyTab.png");
            this.imageList1.Images.SetKeyName(3, "Sf_PPTab.png");
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.ForeColor = System.Drawing.Color.Black;
            this.label15.Location = new System.Drawing.Point(169, 255);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(228, 23);
            this.label15.TabIndex = 338;
            this.label15.Text = "PWM Minimum Movement";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(213, 261);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 19);
            this.label4.TabIndex = 336;
            this.label4.Text = "PWM Maximum";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(204, 181);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(133, 19);
            this.label2.TabIndex = 335;
            this.label2.Text = "Proportional Gain";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarMinPWM
            // 
            this.hsbarMinPWM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hsbarMinPWM.LargeChange = 1;
            this.hsbarMinPWM.Location = new System.Drawing.Point(115, 281);
            this.hsbarMinPWM.Name = "hsbarMinPWM";
            this.hsbarMinPWM.Size = new System.Drawing.Size(309, 44);
            this.hsbarMinPWM.TabIndex = 284;
            this.hsbarMinPWM.Value = 10;
            this.hsbarMinPWM.ValueChanged += new System.EventHandler(this.hsbarMinPWM_ValueChanged);
            // 
            // hsbarProportionalGain
            // 
            this.hsbarProportionalGain.LargeChange = 1;
            this.hsbarProportionalGain.Location = new System.Drawing.Point(136, 203);
            this.hsbarProportionalGain.Maximum = 200;
            this.hsbarProportionalGain.Name = "hsbarProportionalGain";
            this.hsbarProportionalGain.Size = new System.Drawing.Size(270, 41);
            this.hsbarProportionalGain.TabIndex = 254;
            this.hsbarProportionalGain.Value = 4;
            this.hsbarProportionalGain.ValueChanged += new System.EventHandler(this.hsbarProportionalGain_ValueChanged);
            // 
            // lblProportionalGain
            // 
            this.lblProportionalGain.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProportionalGain.ForeColor = System.Drawing.Color.Black;
            this.lblProportionalGain.Location = new System.Drawing.Point(46, 207);
            this.lblProportionalGain.Name = "lblProportionalGain";
            this.lblProportionalGain.Size = new System.Drawing.Size(83, 35);
            this.lblProportionalGain.TabIndex = 258;
            this.lblProportionalGain.Text = "888";
            this.lblProportionalGain.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblHighSteerPWM
            // 
            this.lblHighSteerPWM.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighSteerPWM.ForeColor = System.Drawing.Color.Black;
            this.lblHighSteerPWM.Location = new System.Drawing.Point(46, 283);
            this.lblHighSteerPWM.Name = "lblHighSteerPWM";
            this.lblHighSteerPWM.Size = new System.Drawing.Size(83, 35);
            this.lblHighSteerPWM.TabIndex = 278;
            this.lblHighSteerPWM.Text = "888";
            this.lblHighSteerPWM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblMinPWM
            // 
            this.lblMinPWM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMinPWM.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinPWM.ForeColor = System.Drawing.Color.Black;
            this.lblMinPWM.Location = new System.Drawing.Point(27, 284);
            this.lblMinPWM.Name = "lblMinPWM";
            this.lblMinPWM.Size = new System.Drawing.Size(82, 35);
            this.lblMinPWM.TabIndex = 288;
            this.lblMinPWM.Text = "888";
            this.lblMinPWM.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarHighSteerPWM
            // 
            this.hsbarHighSteerPWM.LargeChange = 2;
            this.hsbarHighSteerPWM.Location = new System.Drawing.Point(136, 280);
            this.hsbarHighSteerPWM.Maximum = 255;
            this.hsbarHighSteerPWM.Name = "hsbarHighSteerPWM";
            this.hsbarHighSteerPWM.Size = new System.Drawing.Size(270, 41);
            this.hsbarHighSteerPWM.TabIndex = 274;
            this.hsbarHighSteerPWM.Value = 50;
            this.hsbarHighSteerPWM.ValueChanged += new System.EventHandler(this.hsbarHighSteerPWM_ValueChanged);
            // 
            // lblMaxSteerAngle
            // 
            this.lblMaxSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblMaxSteerAngle.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxSteerAngle.ForeColor = System.Drawing.Color.Black;
            this.lblMaxSteerAngle.Location = new System.Drawing.Point(66, 255);
            this.lblMaxSteerAngle.Name = "lblMaxSteerAngle";
            this.lblMaxSteerAngle.Size = new System.Drawing.Size(92, 35);
            this.lblMaxSteerAngle.TabIndex = 303;
            this.lblMaxSteerAngle.Text = "888";
            this.lblMaxSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarMaxSteerAngle
            // 
            this.hsbarMaxSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hsbarMaxSteerAngle.LargeChange = 1;
            this.hsbarMaxSteerAngle.Location = new System.Drawing.Point(172, 247);
            this.hsbarMaxSteerAngle.Maximum = 80;
            this.hsbarMaxSteerAngle.Minimum = 10;
            this.hsbarMaxSteerAngle.Name = "hsbarMaxSteerAngle";
            this.hsbarMaxSteerAngle.Size = new System.Drawing.Size(270, 50);
            this.hsbarMaxSteerAngle.TabIndex = 299;
            this.hsbarMaxSteerAngle.Value = 10;
            this.hsbarMaxSteerAngle.ValueChanged += new System.EventHandler(this.hsbarMaxSteerAngle_ValueChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label7.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(17, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(632, 57);
            this.label7.TabIndex = 334;
            this.label7.Text = "Turn steering wheel to RIGHT about 20 degrees.\r\nWhile driving in a steady circle," +
    " Press Rec and wait.";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // hsbarCountsPerDegree
            // 
            this.hsbarCountsPerDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hsbarCountsPerDegree.LargeChange = 1;
            this.hsbarCountsPerDegree.Location = new System.Drawing.Point(134, 271);
            this.hsbarCountsPerDegree.Maximum = 255;
            this.hsbarCountsPerDegree.Minimum = 1;
            this.hsbarCountsPerDegree.Name = "hsbarCountsPerDegree";
            this.hsbarCountsPerDegree.Size = new System.Drawing.Size(270, 50);
            this.hsbarCountsPerDegree.TabIndex = 304;
            this.hsbarCountsPerDegree.Value = 20;
            this.hsbarCountsPerDegree.ValueChanged += new System.EventHandler(this.hsbarCountsPerDegree_ValueChanged);
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(50, 52);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(544, 28);
            this.label10.TabIndex = 295;
            this.label10.Text = "Drive straight forward and Touch Zero";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblCountsPerDegree
            // 
            this.lblCountsPerDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCountsPerDegree.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountsPerDegree.ForeColor = System.Drawing.Color.Black;
            this.lblCountsPerDegree.Location = new System.Drawing.Point(39, 278);
            this.lblCountsPerDegree.Name = "lblCountsPerDegree";
            this.lblCountsPerDegree.Size = new System.Drawing.Size(86, 35);
            this.lblCountsPerDegree.TabIndex = 308;
            this.lblCountsPerDegree.Text = "888";
            this.lblCountsPerDegree.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // hsbarWasOffset
            // 
            this.hsbarWasOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hsbarWasOffset.LargeChange = 20;
            this.hsbarWasOffset.Location = new System.Drawing.Point(188, 192);
            this.hsbarWasOffset.Maximum = 4000;
            this.hsbarWasOffset.Minimum = -4000;
            this.hsbarWasOffset.Name = "hsbarWasOffset";
            this.hsbarWasOffset.Size = new System.Drawing.Size(270, 50);
            this.hsbarWasOffset.SmallChange = 2;
            this.hsbarWasOffset.TabIndex = 294;
            this.hsbarWasOffset.ValueChanged += new System.EventHandler(this.hsbarSteerAngleSensorZero_ValueChanged);
            // 
            // lblSteerAngleSensorZero
            // 
            this.lblSteerAngleSensorZero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSteerAngleSensorZero.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleSensorZero.ForeColor = System.Drawing.Color.Black;
            this.lblSteerAngleSensorZero.Location = new System.Drawing.Point(461, 200);
            this.lblSteerAngleSensorZero.Name = "lblSteerAngleSensorZero";
            this.lblSteerAngleSensorZero.Size = new System.Drawing.Size(109, 35);
            this.lblSteerAngleSensorZero.TabIndex = 298;
            this.lblSteerAngleSensorZero.Text = "-55.88";
            this.lblSteerAngleSensorZero.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbarRight
            // 
            this.pbarRight.Location = new System.Drawing.Point(401, 36);
            this.pbarRight.Name = "pbarRight";
            this.pbarRight.Size = new System.Drawing.Size(212, 35);
            this.pbarRight.TabIndex = 330;
            // 
            // pbarLeft
            // 
            this.pbarLeft.Location = new System.Drawing.Point(187, 36);
            this.pbarLeft.Name = "pbarLeft";
            this.pbarLeft.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pbarLeft.RightToLeftLayout = true;
            this.pbarLeft.Size = new System.Drawing.Size(212, 35);
            this.pbarLeft.TabIndex = 329;
            // 
            // label28
            // 
            this.label28.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label28.AutoSize = true;
            this.label28.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label28.ForeColor = System.Drawing.Color.Black;
            this.label28.Location = new System.Drawing.Point(182, 167);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(218, 95);
            this.label28.TabIndex = 513;
            this.label28.Text = "Button - Push Release, On.   \r\n               Push Release, Off\r\n\r\nSwitch - While" +
    " Pushed is On. \r\n               Released is Off.";
            // 
            // labelPressureTurnSensor
            // 
            this.labelPressureTurnSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelPressureTurnSensor.AutoSize = true;
            this.labelPressureTurnSensor.ForeColor = System.Drawing.Color.Black;
            this.labelPressureTurnSensor.Location = new System.Drawing.Point(429, 93);
            this.labelPressureTurnSensor.Name = "labelPressureTurnSensor";
            this.labelPressureTurnSensor.Size = new System.Drawing.Size(110, 13);
            this.labelPressureTurnSensor.TabIndex = 512;
            this.labelPressureTurnSensor.Text = "Pressure Turn Sensor";
            // 
            // labelCurrentTurnSensor
            // 
            this.labelCurrentTurnSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelCurrentTurnSensor.AutoSize = true;
            this.labelCurrentTurnSensor.ForeColor = System.Drawing.Color.Black;
            this.labelCurrentTurnSensor.Location = new System.Drawing.Point(282, 93);
            this.labelCurrentTurnSensor.Name = "labelCurrentTurnSensor";
            this.labelCurrentTurnSensor.Size = new System.Drawing.Size(105, 13);
            this.labelCurrentTurnSensor.TabIndex = 511;
            this.labelCurrentTurnSensor.Text = "Current Turn Sensor";
            // 
            // labelEncoder
            // 
            this.labelEncoder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelEncoder.AutoSize = true;
            this.labelEncoder.ForeColor = System.Drawing.Color.Black;
            this.labelEncoder.Location = new System.Drawing.Point(158, 93);
            this.labelEncoder.Name = "labelEncoder";
            this.labelEncoder.Size = new System.Drawing.Size(65, 13);
            this.labelEncoder.TabIndex = 506;
            this.labelEncoder.Text = "Turn Sensor";
            // 
            // label70
            // 
            this.label70.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label70.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label70.ForeColor = System.Drawing.Color.Black;
            this.label70.Location = new System.Drawing.Point(8, 70);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(626, 52);
            this.label70.TabIndex = 505;
            this.label70.Text = "Drive slowly and enable Auto Steer. Invert Direction if steer motor turns the wro" +
    "ng way.\r\nTest with Arrows.";
            this.label70.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label29
            // 
            this.label29.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label29.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.ForeColor = System.Drawing.Color.Black;
            this.label29.Location = new System.Drawing.Point(60, 109);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(541, 32);
            this.label29.TabIndex = 504;
            this.label29.Text = "Invert Hydraulic Enable Relay?";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxMotorDrive
            // 
            this.cboxMotorDrive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxMotorDrive.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxMotorDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMotorDrive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxMotorDrive.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMotorDrive.FormattingEnabled = true;
            this.cboxMotorDrive.Items.AddRange(new object[] {
            "Cytron",
            "IBT2"});
            this.cboxMotorDrive.Location = new System.Drawing.Point(238, 176);
            this.cboxMotorDrive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxMotorDrive.Name = "cboxMotorDrive";
            this.cboxMotorDrive.Size = new System.Drawing.Size(175, 37);
            this.cboxMotorDrive.TabIndex = 495;
            this.cboxMotorDrive.SelectedIndexChanged += new System.EventHandler(this.cboxMotorDrive_SelectedIndexChanged);
            // 
            // cboxSteerEnable
            // 
            this.cboxSteerEnable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxSteerEnable.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxSteerEnable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSteerEnable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxSteerEnable.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSteerEnable.FormattingEnabled = true;
            this.cboxSteerEnable.Items.AddRange(new object[] {
            "None",
            "Switch",
            "Button"});
            this.cboxSteerEnable.Location = new System.Drawing.Point(195, 111);
            this.cboxSteerEnable.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxSteerEnable.Name = "cboxSteerEnable";
            this.cboxSteerEnable.Size = new System.Drawing.Size(175, 37);
            this.cboxSteerEnable.TabIndex = 498;
            this.cboxSteerEnable.SelectedIndexChanged += new System.EventHandler(this.cboxSteerEnable_SelectedIndexChanged);
            // 
            // label62
            // 
            this.label62.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label62.BackColor = System.Drawing.Color.Transparent;
            this.label62.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label62.ForeColor = System.Drawing.Color.Black;
            this.label62.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label62.Location = new System.Drawing.Point(84, 76);
            this.label62.Name = "label62";
            this.label62.Size = new System.Drawing.Size(396, 29);
            this.label62.TabIndex = 499;
            this.label62.Text = "Steer Enable Mode";
            this.label62.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboxConv
            // 
            this.cboxConv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxConv.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxConv.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxConv.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxConv.FormattingEnabled = true;
            this.cboxConv.Items.AddRange(new object[] {
            "Single",
            "Differential"});
            this.cboxConv.Location = new System.Drawing.Point(238, 176);
            this.cboxConv.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxConv.Name = "cboxConv";
            this.cboxConv.Size = new System.Drawing.Size(175, 37);
            this.cboxConv.TabIndex = 500;
            this.cboxConv.SelectedIndexChanged += new System.EventHandler(this.cboxConv_SelectedIndexChanged);
            // 
            // label63
            // 
            this.label63.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label63.BackColor = System.Drawing.Color.Transparent;
            this.label63.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label63.ForeColor = System.Drawing.Color.Black;
            this.label63.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label63.Location = new System.Drawing.Point(67, 139);
            this.label63.Name = "label63";
            this.label63.Size = new System.Drawing.Size(529, 25);
            this.label63.TabIndex = 496;
            this.label63.Text = "What Style of Motor Driver?";
            this.label63.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label64
            // 
            this.label64.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label64.BackColor = System.Drawing.Color.Transparent;
            this.label64.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label64.ForeColor = System.Drawing.Color.Black;
            this.label64.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label64.Location = new System.Drawing.Point(114, 138);
            this.label64.Name = "label64";
            this.label64.Size = new System.Drawing.Size(436, 25);
            this.label64.TabIndex = 497;
            this.label64.Text = "A/D Convertor Connection?";
            this.label64.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label61
            // 
            this.label61.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label61.BackColor = System.Drawing.Color.Transparent;
            this.label61.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label61.ForeColor = System.Drawing.Color.Black;
            this.label61.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label61.Location = new System.Drawing.Point(53, 204);
            this.label61.Name = "label61";
            this.label61.Size = new System.Drawing.Size(112, 32);
            this.label61.TabIndex = 494;
            this.label61.Text = "Turn Counts";
            this.label61.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudMaxCounts
            // 
            this.nudMaxCounts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudMaxCounts.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMaxCounts.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMaxCounts.InterceptArrowKeys = false;
            this.nudMaxCounts.Location = new System.Drawing.Point(58, 239);
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
            // label55
            // 
            this.label55.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label55.BackColor = System.Drawing.Color.Transparent;
            this.label55.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label55.ForeColor = System.Drawing.Color.Black;
            this.label55.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label55.Location = new System.Drawing.Point(77, 109);
            this.label55.Name = "label55";
            this.label55.Size = new System.Drawing.Size(501, 31);
            this.label55.TabIndex = 489;
            this.label55.Text = "Do you have use a Danfoss Valve?";
            this.label55.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.ForeColor = System.Drawing.Color.Black;
            this.label30.Location = new System.Drawing.Point(12, 84);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(635, 56);
            this.label30.TabIndex = 515;
            this.label30.Text = "Steer Angle must be POSITIVE when turning to the RIGHT.\r\nIf not, select the Inver" +
    "t WAS.";
            this.label30.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.btnSteerStatus);
            this.panel2.Controls.Add(this.label35);
            this.panel2.Controls.Add(this.label34);
            this.panel2.Controls.Add(this.lblError);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.lblSteerAngle);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.lblPWMDisplay);
            this.panel2.Controls.Add(this.lblSteerAngleActual);
            this.panel2.Controls.Add(this.label13);
            this.panel2.Controls.Add(this.pbarLeft);
            this.panel2.Controls.Add(this.pbarRight);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Location = new System.Drawing.Point(5, 430);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(665, 123);
            this.panel2.TabIndex = 324;
            // 
            // btnSteerStatus
            // 
            this.btnSteerStatus.BackColor = System.Drawing.Color.White;
            this.btnSteerStatus.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnSteerStatus.FlatAppearance.BorderSize = 0;
            this.btnSteerStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerStatus.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSteerStatus.Image = global::AgOpenGPS.Properties.Resources.WizSteerDot;
            this.btnSteerStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSteerStatus.Location = new System.Drawing.Point(14, 4);
            this.btnSteerStatus.Name = "btnSteerStatus";
            this.btnSteerStatus.Size = new System.Drawing.Size(135, 118);
            this.btnSteerStatus.TabIndex = 531;
            this.btnSteerStatus.UseVisualStyleBackColor = false;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.BackColor = System.Drawing.Color.Transparent;
            this.label35.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label35.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label35.Location = new System.Drawing.Point(194, 74);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(33, 16);
            this.label35.TabIndex = 332;
            this.label35.Text = "Left";
            this.label35.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.BackColor = System.Drawing.Color.Transparent;
            this.label34.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label34.Location = new System.Drawing.Point(572, 74);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(41, 16);
            this.label34.TabIndex = 331;
            this.label34.Text = "Right";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panel1.Controls.Add(this.label38);
            this.panel1.Controls.Add(this.btnFreeDriveZero);
            this.panel1.Controls.Add(this.btnSteerAngleUp);
            this.panel1.Controls.Add(this.btnFreeDrive);
            this.panel1.Controls.Add(this.btnSteerAngleDown);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Location = new System.Drawing.Point(915, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(429, 100);
            this.panel1.TabIndex = 323;
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.BackColor = System.Drawing.Color.Transparent;
            this.label38.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label38.Location = new System.Drawing.Point(13, 72);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(73, 16);
            this.label38.TabIndex = 325;
            this.label38.Text = "Free Drive";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnFreeDriveZero
            // 
            this.btnFreeDriveZero.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnFreeDriveZero.FlatAppearance.BorderSize = 0;
            this.btnFreeDriveZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFreeDriveZero.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFreeDriveZero.ForeColor = System.Drawing.Color.White;
            this.btnFreeDriveZero.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnFreeDriveZero.Location = new System.Drawing.Point(340, 13);
            this.btnFreeDriveZero.Name = "btnFreeDriveZero";
            this.btnFreeDriveZero.Size = new System.Drawing.Size(73, 56);
            this.btnFreeDriveZero.TabIndex = 313;
            this.btnFreeDriveZero.UseVisualStyleBackColor = true;
            this.btnFreeDriveZero.Click += new System.EventHandler(this.btnFreeDriveZero_Click);
            // 
            // btnSteerAngleUp
            // 
            this.btnSteerAngleUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSteerAngleUp.FlatAppearance.BorderSize = 0;
            this.btnSteerAngleUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerAngleUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSteerAngleUp.Image = global::AgOpenGPS.Properties.Resources.SteerRight;
            this.btnSteerAngleUp.Location = new System.Drawing.Point(230, 14);
            this.btnSteerAngleUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSteerAngleUp.Name = "btnSteerAngleUp";
            this.btnSteerAngleUp.Size = new System.Drawing.Size(73, 74);
            this.btnSteerAngleUp.TabIndex = 315;
            this.btnSteerAngleUp.UseVisualStyleBackColor = true;
            this.btnSteerAngleUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSteerAngleUp_MouseDown);
            // 
            // btnFreeDrive
            // 
            this.btnFreeDrive.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnFreeDrive.FlatAppearance.BorderSize = 0;
            this.btnFreeDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFreeDrive.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFreeDrive.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnFreeDrive.Image = global::AgOpenGPS.Properties.Resources.SteerDriveOff;
            this.btnFreeDrive.Location = new System.Drawing.Point(13, 13);
            this.btnFreeDrive.Name = "btnFreeDrive";
            this.btnFreeDrive.Size = new System.Drawing.Size(73, 56);
            this.btnFreeDrive.TabIndex = 228;
            this.btnFreeDrive.UseVisualStyleBackColor = false;
            this.btnFreeDrive.Click += new System.EventHandler(this.btnFreeDrive_Click);
            // 
            // btnSteerAngleDown
            // 
            this.btnSteerAngleDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSteerAngleDown.FlatAppearance.BorderSize = 0;
            this.btnSteerAngleDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerAngleDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSteerAngleDown.Image = global::AgOpenGPS.Properties.Resources.SteerLeft;
            this.btnSteerAngleDown.Location = new System.Drawing.Point(124, 13);
            this.btnSteerAngleDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnSteerAngleDown.Name = "btnSteerAngleDown";
            this.btnSteerAngleDown.Size = new System.Drawing.Size(73, 74);
            this.btnSteerAngleDown.TabIndex = 314;
            this.btnSteerAngleDown.UseVisualStyleBackColor = true;
            this.btnSteerAngleDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSteerAngleDown_MouseDown);
            // 
            // hsbarSensor
            // 
            this.hsbarSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hsbarSensor.LargeChange = 1;
            this.hsbarSensor.Location = new System.Drawing.Point(37, 247);
            this.hsbarSensor.Maximum = 255;
            this.hsbarSensor.Name = "hsbarSensor";
            this.hsbarSensor.Size = new System.Drawing.Size(280, 30);
            this.hsbarSensor.TabIndex = 516;
            this.hsbarSensor.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarSensor_Scroll);
            // 
            // lblhsbarSensor
            // 
            this.lblhsbarSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblhsbarSensor.AutoSize = true;
            this.lblhsbarSensor.BackColor = System.Drawing.Color.Transparent;
            this.lblhsbarSensor.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblhsbarSensor.ForeColor = System.Drawing.Color.Black;
            this.lblhsbarSensor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblhsbarSensor.Location = new System.Drawing.Point(336, 250);
            this.lblhsbarSensor.Name = "lblhsbarSensor";
            this.lblhsbarSensor.Size = new System.Drawing.Size(57, 29);
            this.lblhsbarSensor.TabIndex = 518;
            this.lblhsbarSensor.Text = "0%";
            this.lblhsbarSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabWiz
            // 
            this.tabWiz.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabWiz.Controls.Add(this.tabStart);
            this.tabWiz.Controls.Add(this.tabLoadDef);
            this.tabWiz.Controls.Add(this.tabWheelBase);
            this.tabWiz.Controls.Add(this.tabWheelTrack);
            this.tabWiz.Controls.Add(this.tabAntennaDistance);
            this.tabWiz.Controls.Add(this.tabAntennaHeight);
            this.tabWiz.Controls.Add(this.tabAntennaOffset);
            this.tabWiz.Controls.Add(this.tabButtonSwitch);
            this.tabWiz.Controls.Add(this.tabA2DConv);
            this.tabWiz.Controls.Add(this.tabMotorDriver);
            this.tabWiz.Controls.Add(this.tabInvertRelays);
            this.tabWiz.Controls.Add(this.tabDanfoss);
            this.tabWiz.Controls.Add(this.tabRollInv);
            this.tabWiz.Controls.Add(this.tabRollZero);
            this.tabWiz.Controls.Add(this.tabWAS);
            this.tabWiz.Controls.Add(this.tabWAS_Zero);
            this.tabWiz.Controls.Add(this.tabMotorDirection);
            this.tabWiz.Controls.Add(this.tabCPD_Setup);
            this.tabWiz.Controls.Add(this.tabCountsPerDeg);
            this.tabWiz.Controls.Add(this.tabAckCPD);
            this.tabWiz.Controls.Add(this.tabMaxSteerAngle);
            this.tabWiz.Controls.Add(this.tabCancelGuidance);
            this.tabWiz.Controls.Add(this.tabPanicStop);
            this.tabWiz.Controls.Add(this.tab_MinimumGain);
            this.tabWiz.Controls.Add(this.tabPGain);
            this.tabWiz.Controls.Add(this.tabEnd);
            this.tabWiz.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabWiz.ItemSize = new System.Drawing.Size(60, 15);
            this.tabWiz.Location = new System.Drawing.Point(0, 0);
            this.tabWiz.Name = "tabWiz";
            this.tabWiz.Padding = new System.Drawing.Point(0, 0);
            this.tabWiz.SelectedIndex = 0;
            this.tabWiz.Size = new System.Drawing.Size(663, 371);
            this.tabWiz.TabIndex = 522;
            // 
            // tabStart
            // 
            this.tabStart.BackColor = System.Drawing.Color.White;
            this.tabStart.Controls.Add(this.btnStartWizard);
            this.tabStart.Location = new System.Drawing.Point(4, 19);
            this.tabStart.Name = "tabStart";
            this.tabStart.Size = new System.Drawing.Size(655, 348);
            this.tabStart.TabIndex = 15;
            this.tabStart.Text = "tabStart";
            // 
            // btnStartWizard
            // 
            this.btnStartWizard.BackColor = System.Drawing.Color.PaleGreen;
            this.btnStartWizard.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnStartWizard.FlatAppearance.BorderSize = 2;
            this.btnStartWizard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartWizard.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnStartWizard.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnStartWizard.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStartWizard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartWizard.Location = new System.Drawing.Point(226, 106);
            this.btnStartWizard.Name = "btnStartWizard";
            this.btnStartWizard.Size = new System.Drawing.Size(199, 80);
            this.btnStartWizard.TabIndex = 526;
            this.btnStartWizard.Text = "  Start Wizard";
            this.btnStartWizard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStartWizard.UseVisualStyleBackColor = false;
            this.btnStartWizard.Click += new System.EventHandler(this.btnStartWizard_Click);
            // 
            // tabLoadDef
            // 
            this.tabLoadDef.BackColor = System.Drawing.Color.White;
            this.tabLoadDef.Controls.Add(this.label81);
            this.tabLoadDef.Controls.Add(this.label80);
            this.tabLoadDef.Controls.Add(this.btnOkNext_LoadDefault);
            this.tabLoadDef.Controls.Add(this.btnLoadDefaults);
            this.tabLoadDef.Location = new System.Drawing.Point(4, 19);
            this.tabLoadDef.Name = "tabLoadDef";
            this.tabLoadDef.Size = new System.Drawing.Size(655, 348);
            this.tabLoadDef.TabIndex = 35;
            this.tabLoadDef.Text = "LoadDef";
            // 
            // label81
            // 
            this.label81.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label81.BackColor = System.Drawing.Color.Transparent;
            this.label81.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label81.ForeColor = System.Drawing.Color.Black;
            this.label81.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label81.Location = new System.Drawing.Point(115, 188);
            this.label81.Name = "label81";
            this.label81.Size = new System.Drawing.Size(436, 25);
            this.label81.TabIndex = 536;
            this.label81.Text = "Highly Recommended";
            this.label81.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label80
            // 
            this.label80.AutoSize = true;
            this.label80.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label80.ForeColor = System.Drawing.Color.Black;
            this.label80.Location = new System.Drawing.Point(6, 5);
            this.label80.Name = "label80";
            this.label80.Size = new System.Drawing.Size(157, 25);
            this.label80.TabIndex = 535;
            this.label80.Text = "Load Defaults";
            this.label80.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_LoadDefault
            // 
            this.btnOkNext_LoadDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_LoadDefault.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_LoadDefault.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_LoadDefault.FlatAppearance.BorderSize = 0;
            this.btnOkNext_LoadDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_LoadDefault.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_LoadDefault.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_LoadDefault.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_LoadDefault.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_LoadDefault.Name = "btnOkNext_LoadDefault";
            this.btnOkNext_LoadDefault.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_LoadDefault.TabIndex = 534;
            this.btnOkNext_LoadDefault.UseVisualStyleBackColor = false;
            this.btnOkNext_LoadDefault.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // btnLoadDefaults
            // 
            this.btnLoadDefaults.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadDefaults.BackColor = System.Drawing.Color.LightSteelBlue;
            this.btnLoadDefaults.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnLoadDefaults.FlatAppearance.BorderSize = 2;
            this.btnLoadDefaults.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoadDefaults.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnLoadDefaults.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnLoadDefaults.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnLoadDefaults.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLoadDefaults.Location = new System.Drawing.Point(234, 225);
            this.btnLoadDefaults.Name = "btnLoadDefaults";
            this.btnLoadDefaults.Size = new System.Drawing.Size(199, 80);
            this.btnLoadDefaults.TabIndex = 533;
            this.btnLoadDefaults.Text = "Load Defaults";
            this.btnLoadDefaults.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLoadDefaults.UseVisualStyleBackColor = false;
            this.btnLoadDefaults.Click += new System.EventHandler(this.btnLoadDefaults_Click);
            // 
            // tabWheelBase
            // 
            this.tabWheelBase.BackColor = System.Drawing.Color.White;
            this.tabWheelBase.BackgroundImage = global::AgOpenGPS.Properties.Resources.RadiusWheelBase;
            this.tabWheelBase.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabWheelBase.Controls.Add(this.label117);
            this.tabWheelBase.Controls.Add(this.label89);
            this.tabWheelBase.Controls.Add(this.button1);
            this.tabWheelBase.Controls.Add(this.label90);
            this.tabWheelBase.Controls.Add(this.nudWheelbase);
            this.tabWheelBase.Controls.Add(this.label91);
            this.tabWheelBase.Controls.Add(this.button2);
            this.tabWheelBase.Location = new System.Drawing.Point(4, 19);
            this.tabWheelBase.Name = "tabWheelBase";
            this.tabWheelBase.Size = new System.Drawing.Size(655, 348);
            this.tabWheelBase.TabIndex = 37;
            this.tabWheelBase.Text = "WBase";
            // 
            // label117
            // 
            this.label117.BackColor = System.Drawing.Color.White;
            this.label117.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label117.ForeColor = System.Drawing.Color.Black;
            this.label117.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label117.Location = new System.Drawing.Point(3, 8);
            this.label117.Name = "label117";
            this.label117.Size = new System.Drawing.Size(455, 154);
            this.label117.TabIndex = 544;
            this.label117.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label89
            // 
            this.label89.BackColor = System.Drawing.Color.White;
            this.label89.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label89.ForeColor = System.Drawing.Color.Black;
            this.label89.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label89.Location = new System.Drawing.Point(385, 5);
            this.label89.Name = "label89";
            this.label89.Size = new System.Drawing.Size(224, 257);
            this.label89.TabIndex = 543;
            this.label89.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button1.BackColor = System.Drawing.Color.Transparent;
            this.button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button1.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(486, 268);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(72, 72);
            this.button1.TabIndex = 542;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label90
            // 
            this.label90.AutoSize = true;
            this.label90.BackColor = System.Drawing.Color.Transparent;
            this.label90.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label90.ForeColor = System.Drawing.Color.Black;
            this.label90.Location = new System.Drawing.Point(6, 6);
            this.label90.Name = "label90";
            this.label90.Size = new System.Drawing.Size(126, 25);
            this.label90.TabIndex = 541;
            this.label90.Text = "Wheelbase";
            this.label90.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudWheelbase
            // 
            this.nudWheelbase.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudWheelbase.BackColor = System.Drawing.Color.AliceBlue;
            this.nudWheelbase.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudWheelbase.InterceptArrowKeys = false;
            this.nudWheelbase.Location = new System.Drawing.Point(90, 231);
            this.nudWheelbase.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudWheelbase.Name = "nudWheelbase";
            this.nudWheelbase.ReadOnly = true;
            this.nudWheelbase.Size = new System.Drawing.Size(143, 65);
            this.nudWheelbase.TabIndex = 537;
            this.nudWheelbase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudWheelbase.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            this.nudWheelbase.Click += new System.EventHandler(this.nudWheelbase_Click);
            // 
            // label91
            // 
            this.label91.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label91.BackColor = System.Drawing.Color.Linen;
            this.label91.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label91.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label91.ForeColor = System.Drawing.Color.Black;
            this.label91.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label91.Location = new System.Drawing.Point(67, 153);
            this.label91.Name = "label91";
            this.label91.Size = new System.Drawing.Size(180, 187);
            this.label91.TabIndex = 538;
            this.label91.Text = "Set Wheelbase";
            this.label91.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.BackColor = System.Drawing.Color.Transparent;
            this.button2.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button2.FlatAppearance.BorderSize = 0;
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button2.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(577, 268);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(72, 72);
            this.button2.TabIndex = 540;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabWheelTrack
            // 
            this.tabWheelTrack.BackColor = System.Drawing.Color.White;
            this.tabWheelTrack.BackgroundImage = global::AgOpenGPS.Properties.Resources.RadiusWheelBase;
            this.tabWheelTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabWheelTrack.Controls.Add(this.label118);
            this.tabWheelTrack.Controls.Add(this.nudVehicleTrack);
            this.tabWheelTrack.Controls.Add(this.label95);
            this.tabWheelTrack.Controls.Add(this.label96);
            this.tabWheelTrack.Controls.Add(this.button7);
            this.tabWheelTrack.Controls.Add(this.label94);
            this.tabWheelTrack.Controls.Add(this.button8);
            this.tabWheelTrack.Location = new System.Drawing.Point(4, 19);
            this.tabWheelTrack.Name = "tabWheelTrack";
            this.tabWheelTrack.Size = new System.Drawing.Size(655, 348);
            this.tabWheelTrack.TabIndex = 38;
            this.tabWheelTrack.Text = "Track";
            // 
            // label118
            // 
            this.label118.BackColor = System.Drawing.Color.White;
            this.label118.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label118.ForeColor = System.Drawing.Color.Black;
            this.label118.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label118.Location = new System.Drawing.Point(2, 6);
            this.label118.Name = "label118";
            this.label118.Size = new System.Drawing.Size(294, 335);
            this.label118.TabIndex = 545;
            this.label118.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // nudVehicleTrack
            // 
            this.nudVehicleTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudVehicleTrack.BackColor = System.Drawing.Color.AliceBlue;
            this.nudVehicleTrack.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudVehicleTrack.InterceptArrowKeys = false;
            this.nudVehicleTrack.Location = new System.Drawing.Point(423, 185);
            this.nudVehicleTrack.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudVehicleTrack.Name = "nudVehicleTrack";
            this.nudVehicleTrack.ReadOnly = true;
            this.nudVehicleTrack.Size = new System.Drawing.Size(157, 65);
            this.nudVehicleTrack.TabIndex = 537;
            this.nudVehicleTrack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudVehicleTrack.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            this.nudVehicleTrack.Click += new System.EventHandler(this.nudVehicleTrack_Click);
            // 
            // label95
            // 
            this.label95.BackColor = System.Drawing.Color.Linen;
            this.label95.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label95.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label95.ForeColor = System.Drawing.Color.Black;
            this.label95.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label95.Location = new System.Drawing.Point(380, 144);
            this.label95.Name = "label95";
            this.label95.Size = new System.Drawing.Size(255, 131);
            this.label95.TabIndex = 538;
            this.label95.Text = "Set Track Width";
            this.label95.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label96
            // 
            this.label96.BackColor = System.Drawing.Color.White;
            this.label96.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label96.ForeColor = System.Drawing.Color.Black;
            this.label96.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label96.Location = new System.Drawing.Point(300, 20);
            this.label96.Name = "label96";
            this.label96.Size = new System.Drawing.Size(57, 80);
            this.label96.TabIndex = 544;
            this.label96.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button7
            // 
            this.button7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button7.BackColor = System.Drawing.Color.Transparent;
            this.button7.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button7.FlatAppearance.BorderSize = 0;
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button7.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button7.Location = new System.Drawing.Point(486, 269);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(72, 72);
            this.button7.TabIndex = 542;
            this.button7.UseVisualStyleBackColor = false;
            this.button7.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label94
            // 
            this.label94.AutoSize = true;
            this.label94.BackColor = System.Drawing.Color.Transparent;
            this.label94.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label94.ForeColor = System.Drawing.Color.Black;
            this.label94.Location = new System.Drawing.Point(6, 6);
            this.label94.Name = "label94";
            this.label94.Size = new System.Drawing.Size(142, 25);
            this.label94.TabIndex = 541;
            this.label94.Text = "Wheel Track";
            this.label94.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button8
            // 
            this.button8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button8.BackColor = System.Drawing.Color.Transparent;
            this.button8.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button8.FlatAppearance.BorderSize = 0;
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button8.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button8.Location = new System.Drawing.Point(577, 269);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(72, 72);
            this.button8.TabIndex = 540;
            this.button8.UseVisualStyleBackColor = false;
            this.button8.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabAntennaDistance
            // 
            this.tabAntennaDistance.BackColor = System.Drawing.Color.White;
            this.tabAntennaDistance.BackgroundImage = global::AgOpenGPS.Properties.Resources.AntennaTractor;
            this.tabAntennaDistance.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabAntennaDistance.Controls.Add(this.label100);
            this.tabAntennaDistance.Controls.Add(this.button9);
            this.tabAntennaDistance.Controls.Add(this.label98);
            this.tabAntennaDistance.Controls.Add(this.nudAntennaPivot);
            this.tabAntennaDistance.Controls.Add(this.button10);
            this.tabAntennaDistance.Controls.Add(this.label99);
            this.tabAntennaDistance.Location = new System.Drawing.Point(4, 19);
            this.tabAntennaDistance.Name = "tabAntennaDistance";
            this.tabAntennaDistance.Size = new System.Drawing.Size(655, 348);
            this.tabAntennaDistance.TabIndex = 39;
            this.tabAntennaDistance.Text = "AntDist";
            // 
            // label100
            // 
            this.label100.BackColor = System.Drawing.Color.White;
            this.label100.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label100.ForeColor = System.Drawing.Color.Black;
            this.label100.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label100.Location = new System.Drawing.Point(315, 0);
            this.label100.Name = "label100";
            this.label100.Size = new System.Drawing.Size(151, 68);
            this.label100.TabIndex = 545;
            this.label100.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button9
            // 
            this.button9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button9.BackColor = System.Drawing.Color.Transparent;
            this.button9.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button9.FlatAppearance.BorderSize = 0;
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button9.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button9.Location = new System.Drawing.Point(486, 269);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(72, 72);
            this.button9.TabIndex = 542;
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label98
            // 
            this.label98.AutoSize = true;
            this.label98.BackColor = System.Drawing.Color.Transparent;
            this.label98.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label98.ForeColor = System.Drawing.Color.Black;
            this.label98.Location = new System.Drawing.Point(6, 6);
            this.label98.Name = "label98";
            this.label98.Size = new System.Drawing.Size(99, 50);
            this.label98.TabIndex = 541;
            this.label98.Text = "Antenna\r\nPosition";
            this.label98.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudAntennaPivot
            // 
            this.nudAntennaPivot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudAntennaPivot.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAntennaPivot.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAntennaPivot.InterceptArrowKeys = false;
            this.nudAntennaPivot.Location = new System.Drawing.Point(230, 226);
            this.nudAntennaPivot.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudAntennaPivot.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.nudAntennaPivot.Name = "nudAntennaPivot";
            this.nudAntennaPivot.ReadOnly = true;
            this.nudAntennaPivot.Size = new System.Drawing.Size(165, 65);
            this.nudAntennaPivot.TabIndex = 537;
            this.nudAntennaPivot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAntennaPivot.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            this.nudAntennaPivot.Click += new System.EventHandler(this.nudAntennaPivot_Click);
            // 
            // button10
            // 
            this.button10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button10.BackColor = System.Drawing.Color.Transparent;
            this.button10.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button10.FlatAppearance.BorderSize = 0;
            this.button10.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button10.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button10.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button10.Location = new System.Drawing.Point(577, 269);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(72, 72);
            this.button10.TabIndex = 540;
            this.button10.UseVisualStyleBackColor = false;
            this.button10.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // label99
            // 
            this.label99.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label99.BackColor = System.Drawing.Color.Linen;
            this.label99.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label99.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label99.ForeColor = System.Drawing.Color.Black;
            this.label99.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label99.Location = new System.Drawing.Point(133, 155);
            this.label99.Name = "label99";
            this.label99.Size = new System.Drawing.Size(352, 185);
            this.label99.TabIndex = 538;
            this.label99.Text = "Set Distance Antenna Is From Pivot Axle";
            this.label99.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabAntennaHeight
            // 
            this.tabAntennaHeight.BackColor = System.Drawing.Color.White;
            this.tabAntennaHeight.BackgroundImage = global::AgOpenGPS.Properties.Resources.AntennaTractor;
            this.tabAntennaHeight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabAntennaHeight.Controls.Add(this.label114);
            this.tabAntennaHeight.Controls.Add(this.label97);
            this.tabAntennaHeight.Controls.Add(this.button13);
            this.tabAntennaHeight.Controls.Add(this.label92);
            this.tabAntennaHeight.Controls.Add(this.nudAntennaHeight);
            this.tabAntennaHeight.Controls.Add(this.label93);
            this.tabAntennaHeight.Controls.Add(this.button14);
            this.tabAntennaHeight.Location = new System.Drawing.Point(4, 19);
            this.tabAntennaHeight.Name = "tabAntennaHeight";
            this.tabAntennaHeight.Size = new System.Drawing.Size(655, 348);
            this.tabAntennaHeight.TabIndex = 41;
            this.tabAntennaHeight.Text = "AntHigh";
            // 
            // label114
            // 
            this.label114.BackColor = System.Drawing.Color.White;
            this.label114.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label114.ForeColor = System.Drawing.Color.Black;
            this.label114.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label114.Location = new System.Drawing.Point(163, 0);
            this.label114.Name = "label114";
            this.label114.Size = new System.Drawing.Size(162, 24);
            this.label114.TabIndex = 545;
            this.label114.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label97
            // 
            this.label97.BackColor = System.Drawing.Color.White;
            this.label97.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label97.ForeColor = System.Drawing.Color.Black;
            this.label97.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label97.Location = new System.Drawing.Point(109, 185);
            this.label97.Name = "label97";
            this.label97.Size = new System.Drawing.Size(371, 163);
            this.label97.TabIndex = 544;
            this.label97.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button13
            // 
            this.button13.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button13.BackColor = System.Drawing.Color.Transparent;
            this.button13.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button13.FlatAppearance.BorderSize = 0;
            this.button13.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button13.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button13.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button13.Location = new System.Drawing.Point(486, 269);
            this.button13.Name = "button13";
            this.button13.Size = new System.Drawing.Size(72, 72);
            this.button13.TabIndex = 542;
            this.button13.UseVisualStyleBackColor = false;
            this.button13.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label92
            // 
            this.label92.AutoSize = true;
            this.label92.BackColor = System.Drawing.Color.Transparent;
            this.label92.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label92.ForeColor = System.Drawing.Color.Black;
            this.label92.Location = new System.Drawing.Point(6, 6);
            this.label92.Name = "label92";
            this.label92.Size = new System.Drawing.Size(99, 50);
            this.label92.TabIndex = 541;
            this.label92.Text = "Antenna\r\nHeight";
            this.label92.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudAntennaHeight
            // 
            this.nudAntennaHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudAntennaHeight.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAntennaHeight.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAntennaHeight.InterceptArrowKeys = false;
            this.nudAntennaHeight.Location = new System.Drawing.Point(465, 87);
            this.nudAntennaHeight.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudAntennaHeight.Name = "nudAntennaHeight";
            this.nudAntennaHeight.ReadOnly = true;
            this.nudAntennaHeight.Size = new System.Drawing.Size(145, 65);
            this.nudAntennaHeight.TabIndex = 537;
            this.nudAntennaHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAntennaHeight.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            this.nudAntennaHeight.Click += new System.EventHandler(this.nudAntennaHeight_Click);
            // 
            // label93
            // 
            this.label93.BackColor = System.Drawing.Color.Linen;
            this.label93.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label93.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label93.ForeColor = System.Drawing.Color.Black;
            this.label93.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label93.Location = new System.Drawing.Point(436, 12);
            this.label93.Name = "label93";
            this.label93.Size = new System.Drawing.Size(198, 173);
            this.label93.TabIndex = 538;
            this.label93.Text = "Set Antenna Height";
            this.label93.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // button14
            // 
            this.button14.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button14.BackColor = System.Drawing.Color.Transparent;
            this.button14.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button14.FlatAppearance.BorderSize = 0;
            this.button14.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button14.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button14.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button14.Location = new System.Drawing.Point(577, 269);
            this.button14.Name = "button14";
            this.button14.Size = new System.Drawing.Size(72, 72);
            this.button14.TabIndex = 540;
            this.button14.UseVisualStyleBackColor = false;
            this.button14.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabAntennaOffset
            // 
            this.tabAntennaOffset.BackColor = System.Drawing.Color.White;
            this.tabAntennaOffset.BackgroundImage = global::AgOpenGPS.Properties.Resources.AntennaTractor;
            this.tabAntennaOffset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabAntennaOffset.Controls.Add(this.button11);
            this.tabAntennaOffset.Controls.Add(this.label102);
            this.tabAntennaOffset.Controls.Add(this.nudAntennaOffset);
            this.tabAntennaOffset.Controls.Add(this.label104);
            this.tabAntennaOffset.Controls.Add(this.button12);
            this.tabAntennaOffset.Controls.Add(this.label103);
            this.tabAntennaOffset.Location = new System.Drawing.Point(4, 19);
            this.tabAntennaOffset.Name = "tabAntennaOffset";
            this.tabAntennaOffset.Size = new System.Drawing.Size(655, 348);
            this.tabAntennaOffset.TabIndex = 40;
            this.tabAntennaOffset.Text = "AntOff";
            // 
            // button11
            // 
            this.button11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button11.BackColor = System.Drawing.Color.Transparent;
            this.button11.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button11.FlatAppearance.BorderSize = 0;
            this.button11.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button11.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button11.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button11.Location = new System.Drawing.Point(485, 269);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(72, 72);
            this.button11.TabIndex = 542;
            this.button11.UseVisualStyleBackColor = false;
            this.button11.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label102
            // 
            this.label102.AutoSize = true;
            this.label102.BackColor = System.Drawing.Color.Transparent;
            this.label102.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label102.ForeColor = System.Drawing.Color.Black;
            this.label102.Location = new System.Drawing.Point(0, 6);
            this.label102.Name = "label102";
            this.label102.Size = new System.Drawing.Size(169, 25);
            this.label102.TabIndex = 541;
            this.label102.Text = "Antenna Offset";
            this.label102.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // nudAntennaOffset
            // 
            this.nudAntennaOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudAntennaOffset.BackColor = System.Drawing.Color.AliceBlue;
            this.nudAntennaOffset.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudAntennaOffset.InterceptArrowKeys = false;
            this.nudAntennaOffset.Location = new System.Drawing.Point(250, 65);
            this.nudAntennaOffset.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.nudAntennaOffset.Minimum = new decimal(new int[] {
            999,
            0,
            0,
            -2147483648});
            this.nudAntennaOffset.Name = "nudAntennaOffset";
            this.nudAntennaOffset.ReadOnly = true;
            this.nudAntennaOffset.Size = new System.Drawing.Size(165, 65);
            this.nudAntennaOffset.TabIndex = 537;
            this.nudAntennaOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudAntennaOffset.Value = new decimal(new int[] {
            333,
            0,
            0,
            0});
            this.nudAntennaOffset.Click += new System.EventHandler(this.nudAntennaOffset_Click);
            // 
            // label104
            // 
            this.label104.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label104.AutoSize = true;
            this.label104.BackColor = System.Drawing.Color.Transparent;
            this.label104.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label104.ForeColor = System.Drawing.Color.Black;
            this.label104.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label104.Location = new System.Drawing.Point(471, 247);
            this.label104.Name = "label104";
            this.label104.Size = new System.Drawing.Size(86, 25);
            this.label104.TabIndex = 539;
            this.label104.Text = "0 = Off";
            this.label104.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // button12
            // 
            this.button12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button12.BackColor = System.Drawing.Color.Transparent;
            this.button12.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button12.FlatAppearance.BorderSize = 0;
            this.button12.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button12.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button12.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button12.Location = new System.Drawing.Point(576, 269);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(72, 72);
            this.button12.TabIndex = 540;
            this.button12.UseVisualStyleBackColor = false;
            this.button12.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // label103
            // 
            this.label103.BackColor = System.Drawing.Color.Linen;
            this.label103.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label103.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label103.ForeColor = System.Drawing.Color.Black;
            this.label103.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label103.Location = new System.Drawing.Point(171, 1);
            this.label103.Name = "label103";
            this.label103.Size = new System.Drawing.Size(313, 187);
            this.label103.TabIndex = 538;
            this.label103.Text = "Set Antenna Offset";
            this.label103.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // tabButtonSwitch
            // 
            this.tabButtonSwitch.BackColor = System.Drawing.Color.White;
            this.tabButtonSwitch.Controls.Add(this.button15);
            this.tabButtonSwitch.Controls.Add(this.label53);
            this.tabButtonSwitch.Controls.Add(this.cboxSteerEnable);
            this.tabButtonSwitch.Controls.Add(this.label28);
            this.tabButtonSwitch.Controls.Add(this.label62);
            this.tabButtonSwitch.Controls.Add(this.btnOkNext_ButtonSwitch);
            this.tabButtonSwitch.Location = new System.Drawing.Point(4, 19);
            this.tabButtonSwitch.Name = "tabButtonSwitch";
            this.tabButtonSwitch.Size = new System.Drawing.Size(655, 348);
            this.tabButtonSwitch.TabIndex = 19;
            this.tabButtonSwitch.Text = "ButtonSwitch";
            // 
            // button15
            // 
            this.button15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button15.BackColor = System.Drawing.Color.Transparent;
            this.button15.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button15.FlatAppearance.BorderSize = 0;
            this.button15.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button15.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button15.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button15.Location = new System.Drawing.Point(485, 269);
            this.button15.Name = "button15";
            this.button15.Size = new System.Drawing.Size(72, 72);
            this.button15.TabIndex = 544;
            this.button15.UseVisualStyleBackColor = false;
            this.button15.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label53
            // 
            this.label53.AutoSize = true;
            this.label53.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label53.ForeColor = System.Drawing.Color.Black;
            this.label53.Location = new System.Drawing.Point(5, 4);
            this.label53.Name = "label53";
            this.label53.Size = new System.Drawing.Size(144, 25);
            this.label53.TabIndex = 533;
            this.label53.Text = "Steer Switch";
            this.label53.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_ButtonSwitch
            // 
            this.btnOkNext_ButtonSwitch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_ButtonSwitch.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_ButtonSwitch.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_ButtonSwitch.FlatAppearance.BorderSize = 0;
            this.btnOkNext_ButtonSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_ButtonSwitch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_ButtonSwitch.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_ButtonSwitch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_ButtonSwitch.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_ButtonSwitch.Name = "btnOkNext_ButtonSwitch";
            this.btnOkNext_ButtonSwitch.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_ButtonSwitch.TabIndex = 525;
            this.btnOkNext_ButtonSwitch.UseVisualStyleBackColor = false;
            this.btnOkNext_ButtonSwitch.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabA2DConv
            // 
            this.tabA2DConv.BackColor = System.Drawing.Color.White;
            this.tabA2DConv.Controls.Add(this.btnPrev_A2D);
            this.tabA2DConv.Controls.Add(this.label52);
            this.tabA2DConv.Controls.Add(this.cboxConv);
            this.tabA2DConv.Controls.Add(this.label64);
            this.tabA2DConv.Controls.Add(this.btnOkNext_A2D);
            this.tabA2DConv.Location = new System.Drawing.Point(4, 19);
            this.tabA2DConv.Name = "tabA2DConv";
            this.tabA2DConv.Size = new System.Drawing.Size(655, 348);
            this.tabA2DConv.TabIndex = 27;
            this.tabA2DConv.Text = "A2DConv";
            // 
            // btnPrev_A2D
            // 
            this.btnPrev_A2D.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_A2D.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_A2D.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_A2D.FlatAppearance.BorderSize = 0;
            this.btnPrev_A2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_A2D.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_A2D.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_A2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_A2D.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_A2D.Name = "btnPrev_A2D";
            this.btnPrev_A2D.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_A2D.TabIndex = 537;
            this.btnPrev_A2D.UseVisualStyleBackColor = false;
            this.btnPrev_A2D.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label52
            // 
            this.label52.AutoSize = true;
            this.label52.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label52.ForeColor = System.Drawing.Color.Black;
            this.label52.Location = new System.Drawing.Point(5, 4);
            this.label52.Name = "label52";
            this.label52.Size = new System.Drawing.Size(164, 25);
            this.label52.TabIndex = 533;
            this.label52.Text = "A/D Convertor";
            this.label52.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_A2D
            // 
            this.btnOkNext_A2D.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_A2D.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_A2D.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_A2D.FlatAppearance.BorderSize = 0;
            this.btnOkNext_A2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_A2D.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_A2D.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_A2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_A2D.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_A2D.Name = "btnOkNext_A2D";
            this.btnOkNext_A2D.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_A2D.TabIndex = 527;
            this.btnOkNext_A2D.UseVisualStyleBackColor = false;
            this.btnOkNext_A2D.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabMotorDriver
            // 
            this.tabMotorDriver.BackColor = System.Drawing.Color.White;
            this.tabMotorDriver.Controls.Add(this.btnPrev_MotorDriver);
            this.tabMotorDriver.Controls.Add(this.label43);
            this.tabMotorDriver.Controls.Add(this.cboxMotorDrive);
            this.tabMotorDriver.Controls.Add(this.label63);
            this.tabMotorDriver.Controls.Add(this.btnOkNext_MotorDriver);
            this.tabMotorDriver.Location = new System.Drawing.Point(4, 19);
            this.tabMotorDriver.Name = "tabMotorDriver";
            this.tabMotorDriver.Size = new System.Drawing.Size(655, 348);
            this.tabMotorDriver.TabIndex = 16;
            this.tabMotorDriver.Text = "MotorDriver";
            // 
            // btnPrev_MotorDriver
            // 
            this.btnPrev_MotorDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_MotorDriver.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_MotorDriver.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_MotorDriver.FlatAppearance.BorderSize = 0;
            this.btnPrev_MotorDriver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_MotorDriver.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_MotorDriver.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_MotorDriver.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_MotorDriver.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_MotorDriver.Name = "btnPrev_MotorDriver";
            this.btnPrev_MotorDriver.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_MotorDriver.TabIndex = 537;
            this.btnPrev_MotorDriver.UseVisualStyleBackColor = false;
            this.btnPrev_MotorDriver.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label43
            // 
            this.label43.AutoSize = true;
            this.label43.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label43.ForeColor = System.Drawing.Color.Black;
            this.label43.Location = new System.Drawing.Point(4, 4);
            this.label43.Name = "label43";
            this.label43.Size = new System.Drawing.Size(213, 25);
            this.label43.TabIndex = 532;
            this.label43.Text = "Motor Driver Board";
            this.label43.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_MotorDriver
            // 
            this.btnOkNext_MotorDriver.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_MotorDriver.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_MotorDriver.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_MotorDriver.FlatAppearance.BorderSize = 0;
            this.btnOkNext_MotorDriver.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_MotorDriver.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_MotorDriver.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_MotorDriver.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_MotorDriver.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_MotorDriver.Name = "btnOkNext_MotorDriver";
            this.btnOkNext_MotorDriver.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_MotorDriver.TabIndex = 526;
            this.btnOkNext_MotorDriver.UseVisualStyleBackColor = false;
            this.btnOkNext_MotorDriver.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabInvertRelays
            // 
            this.tabInvertRelays.BackColor = System.Drawing.Color.White;
            this.tabInvertRelays.Controls.Add(this.label59);
            this.tabInvertRelays.Controls.Add(this.btnPrev_InvertRelays);
            this.tabInvertRelays.Controls.Add(this.label51);
            this.tabInvertRelays.Controls.Add(this.label29);
            this.tabInvertRelays.Controls.Add(this.btnOkNext_InvertRelays);
            this.tabInvertRelays.Controls.Add(this.chkSteerInvertRelays);
            this.tabInvertRelays.Location = new System.Drawing.Point(4, 19);
            this.tabInvertRelays.Name = "tabInvertRelays";
            this.tabInvertRelays.Size = new System.Drawing.Size(655, 348);
            this.tabInvertRelays.TabIndex = 29;
            this.tabInvertRelays.Text = "InvertRelays";
            // 
            // label59
            // 
            this.label59.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label59.AutoSize = true;
            this.label59.BackColor = System.Drawing.Color.Transparent;
            this.label59.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label59.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label59.Location = new System.Drawing.Point(282, 236);
            this.label59.Name = "label59";
            this.label59.Size = new System.Drawing.Size(95, 16);
            this.label59.TabIndex = 539;
            this.label59.Text = "Invert Relays";
            this.label59.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_InvertRelays
            // 
            this.btnPrev_InvertRelays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_InvertRelays.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_InvertRelays.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_InvertRelays.FlatAppearance.BorderSize = 0;
            this.btnPrev_InvertRelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_InvertRelays.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_InvertRelays.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_InvertRelays.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_InvertRelays.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_InvertRelays.Name = "btnPrev_InvertRelays";
            this.btnPrev_InvertRelays.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_InvertRelays.TabIndex = 537;
            this.btnPrev_InvertRelays.UseVisualStyleBackColor = false;
            this.btnPrev_InvertRelays.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label51
            // 
            this.label51.AutoSize = true;
            this.label51.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label51.ForeColor = System.Drawing.Color.Black;
            this.label51.Location = new System.Drawing.Point(5, 4);
            this.label51.Name = "label51";
            this.label51.Size = new System.Drawing.Size(223, 25);
            this.label51.TabIndex = 533;
            this.label51.Text = "Driver Control Relay";
            this.label51.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_InvertRelays
            // 
            this.btnOkNext_InvertRelays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_InvertRelays.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_InvertRelays.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_InvertRelays.FlatAppearance.BorderSize = 0;
            this.btnOkNext_InvertRelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_InvertRelays.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_InvertRelays.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_InvertRelays.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_InvertRelays.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_InvertRelays.Name = "btnOkNext_InvertRelays";
            this.btnOkNext_InvertRelays.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_InvertRelays.TabIndex = 528;
            this.btnOkNext_InvertRelays.UseVisualStyleBackColor = false;
            this.btnOkNext_InvertRelays.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // chkSteerInvertRelays
            // 
            this.chkSteerInvertRelays.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkSteerInvertRelays.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSteerInvertRelays.BackColor = System.Drawing.Color.AliceBlue;
            this.chkSteerInvertRelays.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkSteerInvertRelays.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkSteerInvertRelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSteerInvertRelays.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSteerInvertRelays.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkSteerInvertRelays.Image = global::AgOpenGPS.Properties.Resources.ConSt_InvertRelay;
            this.chkSteerInvertRelays.Location = new System.Drawing.Point(274, 155);
            this.chkSteerInvertRelays.Name = "chkSteerInvertRelays";
            this.chkSteerInvertRelays.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkSteerInvertRelays.Size = new System.Drawing.Size(109, 78);
            this.chkSteerInvertRelays.TabIndex = 503;
            this.chkSteerInvertRelays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkSteerInvertRelays.UseVisualStyleBackColor = false;
            this.chkSteerInvertRelays.CheckedChanged += new System.EventHandler(this.chkSteerInvertRelays_CheckedChanged);
            // 
            // tabDanfoss
            // 
            this.tabDanfoss.BackColor = System.Drawing.Color.White;
            this.tabDanfoss.Controls.Add(this.btnPrev_Danfoss);
            this.tabDanfoss.Controls.Add(this.label50);
            this.tabDanfoss.Controls.Add(this.label55);
            this.tabDanfoss.Controls.Add(this.btnOkNext_Danfoss);
            this.tabDanfoss.Controls.Add(this.cboxDanfoss);
            this.tabDanfoss.Location = new System.Drawing.Point(4, 19);
            this.tabDanfoss.Name = "tabDanfoss";
            this.tabDanfoss.Size = new System.Drawing.Size(655, 348);
            this.tabDanfoss.TabIndex = 28;
            this.tabDanfoss.Text = "Danfoss";
            // 
            // btnPrev_Danfoss
            // 
            this.btnPrev_Danfoss.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_Danfoss.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_Danfoss.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_Danfoss.FlatAppearance.BorderSize = 0;
            this.btnPrev_Danfoss.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_Danfoss.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_Danfoss.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_Danfoss.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_Danfoss.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_Danfoss.Name = "btnPrev_Danfoss";
            this.btnPrev_Danfoss.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_Danfoss.TabIndex = 537;
            this.btnPrev_Danfoss.UseVisualStyleBackColor = false;
            this.btnPrev_Danfoss.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label50
            // 
            this.label50.AutoSize = true;
            this.label50.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label50.ForeColor = System.Drawing.Color.Black;
            this.label50.Location = new System.Drawing.Point(5, 4);
            this.label50.Name = "label50";
            this.label50.Size = new System.Drawing.Size(160, 25);
            this.label50.TabIndex = 533;
            this.label50.Text = "Danfoss Valve";
            this.label50.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_Danfoss
            // 
            this.btnOkNext_Danfoss.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_Danfoss.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_Danfoss.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_Danfoss.FlatAppearance.BorderSize = 0;
            this.btnOkNext_Danfoss.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_Danfoss.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_Danfoss.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_Danfoss.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_Danfoss.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_Danfoss.Name = "btnOkNext_Danfoss";
            this.btnOkNext_Danfoss.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_Danfoss.TabIndex = 528;
            this.btnOkNext_Danfoss.UseVisualStyleBackColor = false;
            this.btnOkNext_Danfoss.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // cboxDanfoss
            // 
            this.cboxDanfoss.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxDanfoss.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxDanfoss.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxDanfoss.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxDanfoss.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxDanfoss.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxDanfoss.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDanfoss.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxDanfoss.Image = global::AgOpenGPS.Properties.Resources.ConSt_Danfoss;
            this.cboxDanfoss.Location = new System.Drawing.Point(274, 155);
            this.cboxDanfoss.Name = "cboxDanfoss";
            this.cboxDanfoss.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxDanfoss.Size = new System.Drawing.Size(114, 78);
            this.cboxDanfoss.TabIndex = 507;
            this.cboxDanfoss.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxDanfoss.UseVisualStyleBackColor = false;
            this.cboxDanfoss.CheckedChanged += new System.EventHandler(this.cboxDanfoss_CheckedChanged);
            // 
            // tabRollInv
            // 
            this.tabRollInv.BackColor = System.Drawing.Color.White;
            this.tabRollInv.Controls.Add(this.label115);
            this.tabRollInv.Controls.Add(this.lblRoll);
            this.tabRollInv.Controls.Add(this.pictureBox9);
            this.tabRollInv.Controls.Add(this.cboxDataInvertRoll);
            this.tabRollInv.Controls.Add(this.label36);
            this.tabRollInv.Controls.Add(this.button4);
            this.tabRollInv.Controls.Add(this.label46);
            this.tabRollInv.Controls.Add(this.button18);
            this.tabRollInv.Controls.Add(this.label107);
            this.tabRollInv.Location = new System.Drawing.Point(4, 19);
            this.tabRollInv.Name = "tabRollInv";
            this.tabRollInv.Size = new System.Drawing.Size(655, 348);
            this.tabRollInv.TabIndex = 42;
            this.tabRollInv.Text = "RollInv";
            // 
            // label115
            // 
            this.label115.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label115.BackColor = System.Drawing.Color.Transparent;
            this.label115.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label115.ForeColor = System.Drawing.Color.Black;
            this.label115.Location = new System.Drawing.Point(139, 105);
            this.label115.Name = "label115";
            this.label115.Size = new System.Drawing.Size(268, 56);
            this.label115.TabIndex = 552;
            this.label115.Text = "If not Select Invert Roll";
            this.label115.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRoll
            // 
            this.lblRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRoll.BackColor = System.Drawing.Color.Transparent;
            this.lblRoll.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoll.ForeColor = System.Drawing.Color.Black;
            this.lblRoll.Location = new System.Drawing.Point(274, 274);
            this.lblRoll.Name = "lblRoll";
            this.lblRoll.Size = new System.Drawing.Size(100, 31);
            this.lblRoll.TabIndex = 532;
            this.lblRoll.Text = "-55.5";
            this.lblRoll.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pictureBox9
            // 
            this.pictureBox9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pictureBox9.BackgroundImage = global::AgOpenGPS.Properties.Resources.ConD_RollHelper;
            this.pictureBox9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pictureBox9.Location = new System.Drawing.Point(486, 38);
            this.pictureBox9.Name = "pictureBox9";
            this.pictureBox9.Size = new System.Drawing.Size(154, 217);
            this.pictureBox9.TabIndex = 550;
            this.pictureBox9.TabStop = false;
            // 
            // cboxDataInvertRoll
            // 
            this.cboxDataInvertRoll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxDataInvertRoll.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxDataInvertRoll.BackColor = System.Drawing.Color.WhiteSmoke;
            this.cboxDataInvertRoll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxDataInvertRoll.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxDataInvertRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxDataInvertRoll.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxDataInvertRoll.ForeColor = System.Drawing.Color.Black;
            this.cboxDataInvertRoll.Image = global::AgOpenGPS.Properties.Resources.ConDa_InvertRoll;
            this.cboxDataInvertRoll.Location = new System.Drawing.Point(218, 169);
            this.cboxDataInvertRoll.Name = "cboxDataInvertRoll";
            this.cboxDataInvertRoll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxDataInvertRoll.Size = new System.Drawing.Size(130, 95);
            this.cboxDataInvertRoll.TabIndex = 549;
            this.cboxDataInvertRoll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxDataInvertRoll.UseVisualStyleBackColor = false;
            this.cboxDataInvertRoll.Click += new System.EventHandler(this.cboxDataInvertRoll_Click);
            // 
            // label36
            // 
            this.label36.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label36.BackColor = System.Drawing.Color.Transparent;
            this.label36.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label36.ForeColor = System.Drawing.Color.Black;
            this.label36.Location = new System.Drawing.Point(64, 50);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(401, 56);
            this.label36.TabIndex = 545;
            this.label36.Text = "Put vehicle leaning strongly to the right. Roll to the right must be POSITIVE.";
            this.label36.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button4
            // 
            this.button4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button4.BackColor = System.Drawing.Color.Transparent;
            this.button4.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button4.FlatAppearance.BorderSize = 0;
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button4.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button4.Location = new System.Drawing.Point(486, 270);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(72, 72);
            this.button4.TabIndex = 544;
            this.button4.UseVisualStyleBackColor = false;
            this.button4.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label46
            // 
            this.label46.AutoSize = true;
            this.label46.BackColor = System.Drawing.Color.Transparent;
            this.label46.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label46.ForeColor = System.Drawing.Color.Black;
            this.label46.Location = new System.Drawing.Point(6, 6);
            this.label46.Name = "label46";
            this.label46.Size = new System.Drawing.Size(148, 25);
            this.label46.TabIndex = 543;
            this.label46.Text = "Roll Function";
            this.label46.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button18.BackColor = System.Drawing.Color.Transparent;
            this.button18.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button18.FlatAppearance.BorderSize = 0;
            this.button18.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button18.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button18.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button18.Location = new System.Drawing.Point(577, 270);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(72, 72);
            this.button18.TabIndex = 542;
            this.button18.UseVisualStyleBackColor = false;
            this.button18.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // label107
            // 
            this.label107.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label107.BackColor = System.Drawing.Color.Transparent;
            this.label107.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label107.ForeColor = System.Drawing.Color.Black;
            this.label107.Location = new System.Drawing.Point(181, 274);
            this.label107.Name = "label107";
            this.label107.Size = new System.Drawing.Size(100, 31);
            this.label107.TabIndex = 551;
            this.label107.Text = "Roll:";
            this.label107.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tabRollZero
            // 
            this.tabRollZero.BackColor = System.Drawing.Color.White;
            this.tabRollZero.Controls.Add(this.lblRoll2);
            this.tabRollZero.Controls.Add(this.label110);
            this.tabRollZero.Controls.Add(this.label47);
            this.tabRollZero.Controls.Add(this.label57);
            this.tabRollZero.Controls.Add(this.lblRollZeroOffset);
            this.tabRollZero.Controls.Add(this.btnZeroRoll);
            this.tabRollZero.Controls.Add(this.btnRemoveZeroOffset);
            this.tabRollZero.Controls.Add(this.label48);
            this.tabRollZero.Controls.Add(this.button3);
            this.tabRollZero.Controls.Add(this.label56);
            this.tabRollZero.Controls.Add(this.button19);
            this.tabRollZero.Location = new System.Drawing.Point(4, 19);
            this.tabRollZero.Name = "tabRollZero";
            this.tabRollZero.Size = new System.Drawing.Size(655, 348);
            this.tabRollZero.TabIndex = 44;
            this.tabRollZero.Text = "RollZero";
            // 
            // lblRoll2
            // 
            this.lblRoll2.BackColor = System.Drawing.Color.Transparent;
            this.lblRoll2.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRoll2.ForeColor = System.Drawing.Color.Black;
            this.lblRoll2.Location = new System.Drawing.Point(274, 263);
            this.lblRoll2.Name = "lblRoll2";
            this.lblRoll2.Size = new System.Drawing.Size(100, 31);
            this.lblRoll2.TabIndex = 561;
            this.lblRoll2.Text = "-2.5";
            this.lblRoll2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label110
            // 
            this.label110.BackColor = System.Drawing.Color.Transparent;
            this.label110.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label110.ForeColor = System.Drawing.Color.Black;
            this.label110.Location = new System.Drawing.Point(181, 263);
            this.label110.Name = "label110";
            this.label110.Size = new System.Drawing.Size(100, 31);
            this.label110.TabIndex = 562;
            this.label110.Text = "Roll:";
            this.label110.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label47
            // 
            this.label47.AutoSize = true;
            this.label47.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label47.ForeColor = System.Drawing.Color.Black;
            this.label47.Location = new System.Drawing.Point(334, 117);
            this.label47.Name = "label47";
            this.label47.Size = new System.Drawing.Size(57, 13);
            this.label47.TabIndex = 560;
            this.label47.Text = "Zero Roll";
            // 
            // label57
            // 
            this.label57.AutoSize = true;
            this.label57.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label57.ForeColor = System.Drawing.Color.Black;
            this.label57.Location = new System.Drawing.Point(111, 117);
            this.label57.Name = "label57";
            this.label57.Size = new System.Drawing.Size(91, 13);
            this.label57.TabIndex = 559;
            this.label57.Text = "Remove Offset";
            // 
            // lblRollZeroOffset
            // 
            this.lblRollZeroOffset.AutoSize = true;
            this.lblRollZeroOffset.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.lblRollZeroOffset.ForeColor = System.Drawing.Color.Black;
            this.lblRollZeroOffset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblRollZeroOffset.Location = new System.Drawing.Point(469, 164);
            this.lblRollZeroOffset.Name = "lblRollZeroOffset";
            this.lblRollZeroOffset.Size = new System.Drawing.Size(87, 33);
            this.lblRollZeroOffset.TabIndex = 556;
            this.lblRollZeroOffset.Text = "Offset";
            // 
            // btnZeroRoll
            // 
            this.btnZeroRoll.BackColor = System.Drawing.Color.Transparent;
            this.btnZeroRoll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnZeroRoll.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.btnZeroRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroRoll.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroRoll.ForeColor = System.Drawing.Color.Black;
            this.btnZeroRoll.Image = global::AgOpenGPS.Properties.Resources.ConDa_RollSetZero;
            this.btnZeroRoll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZeroRoll.Location = new System.Drawing.Point(326, 136);
            this.btnZeroRoll.Name = "btnZeroRoll";
            this.btnZeroRoll.Size = new System.Drawing.Size(130, 95);
            this.btnZeroRoll.TabIndex = 557;
            this.btnZeroRoll.UseVisualStyleBackColor = false;
            this.btnZeroRoll.Click += new System.EventHandler(this.btnZeroRoll_Click);
            // 
            // btnRemoveZeroOffset
            // 
            this.btnRemoveZeroOffset.BackColor = System.Drawing.Color.Transparent;
            this.btnRemoveZeroOffset.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnRemoveZeroOffset.FlatAppearance.CheckedBackColor = System.Drawing.Color.Teal;
            this.btnRemoveZeroOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveZeroOffset.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveZeroOffset.ForeColor = System.Drawing.Color.Black;
            this.btnRemoveZeroOffset.Image = global::AgOpenGPS.Properties.Resources.ConDa_RemoveOffset;
            this.btnRemoveZeroOffset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRemoveZeroOffset.Location = new System.Drawing.Point(103, 136);
            this.btnRemoveZeroOffset.Name = "btnRemoveZeroOffset";
            this.btnRemoveZeroOffset.Size = new System.Drawing.Size(130, 95);
            this.btnRemoveZeroOffset.TabIndex = 558;
            this.btnRemoveZeroOffset.UseVisualStyleBackColor = false;
            this.btnRemoveZeroOffset.Click += new System.EventHandler(this.btnRemoveZeroOffset_Click);
            // 
            // label48
            // 
            this.label48.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label48.BackColor = System.Drawing.Color.Transparent;
            this.label48.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label48.ForeColor = System.Drawing.Color.Black;
            this.label48.Location = new System.Drawing.Point(69, 44);
            this.label48.Name = "label48";
            this.label48.Size = new System.Drawing.Size(484, 56);
            this.label48.TabIndex = 555;
            this.label48.Text = "Park on level surface and touch \"Zero Roll\"";
            this.label48.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.BackColor = System.Drawing.Color.Transparent;
            this.button3.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button3.FlatAppearance.BorderSize = 0;
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button3.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button3.Location = new System.Drawing.Point(486, 270);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(72, 72);
            this.button3.TabIndex = 554;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label56
            // 
            this.label56.AutoSize = true;
            this.label56.BackColor = System.Drawing.Color.Transparent;
            this.label56.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label56.ForeColor = System.Drawing.Color.Black;
            this.label56.Location = new System.Drawing.Point(6, 6);
            this.label56.Name = "label56";
            this.label56.Size = new System.Drawing.Size(105, 25);
            this.label56.TabIndex = 553;
            this.label56.Text = "Roll Zero";
            this.label56.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button19
            // 
            this.button19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button19.BackColor = System.Drawing.Color.Transparent;
            this.button19.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.button19.FlatAppearance.BorderSize = 0;
            this.button19.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button19.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button19.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button19.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button19.Location = new System.Drawing.Point(577, 270);
            this.button19.Name = "button19";
            this.button19.Size = new System.Drawing.Size(72, 72);
            this.button19.TabIndex = 552;
            this.button19.UseVisualStyleBackColor = false;
            this.button19.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabWAS
            // 
            this.tabWAS.BackColor = System.Drawing.Color.White;
            this.tabWAS.Controls.Add(this.label45);
            this.tabWAS.Controls.Add(this.btnPrev_InvertWAS);
            this.tabWAS.Controls.Add(this.label42);
            this.tabWAS.Controls.Add(this.label30);
            this.tabWAS.Controls.Add(this.btnOkWAS);
            this.tabWAS.Controls.Add(this.chkInvertWAS);
            this.tabWAS.Location = new System.Drawing.Point(4, 19);
            this.tabWAS.Name = "tabWAS";
            this.tabWAS.Size = new System.Drawing.Size(655, 348);
            this.tabWAS.TabIndex = 18;
            this.tabWAS.Text = "WAS";
            // 
            // label45
            // 
            this.label45.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label45.AutoSize = true;
            this.label45.BackColor = System.Drawing.Color.Transparent;
            this.label45.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label45.Location = new System.Drawing.Point(290, 236);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(83, 16);
            this.label45.TabIndex = 538;
            this.label45.Text = "Invert WAS";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_InvertWAS
            // 
            this.btnPrev_InvertWAS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_InvertWAS.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_InvertWAS.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_InvertWAS.FlatAppearance.BorderSize = 0;
            this.btnPrev_InvertWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_InvertWAS.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_InvertWAS.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_InvertWAS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_InvertWAS.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_InvertWAS.Name = "btnPrev_InvertWAS";
            this.btnPrev_InvertWAS.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_InvertWAS.TabIndex = 537;
            this.btnPrev_InvertWAS.UseVisualStyleBackColor = false;
            this.btnPrev_InvertWAS.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label42
            // 
            this.label42.AutoSize = true;
            this.label42.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label42.ForeColor = System.Drawing.Color.Black;
            this.label42.Location = new System.Drawing.Point(4, 4);
            this.label42.Name = "label42";
            this.label42.Size = new System.Drawing.Size(233, 25);
            this.label42.TabIndex = 532;
            this.label42.Text = "Invert WAS Direction";
            this.label42.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkWAS
            // 
            this.btnOkWAS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkWAS.BackColor = System.Drawing.Color.Transparent;
            this.btnOkWAS.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkWAS.FlatAppearance.BorderSize = 0;
            this.btnOkWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkWAS.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkWAS.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkWAS.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkWAS.Location = new System.Drawing.Point(576, 269);
            this.btnOkWAS.Name = "btnOkWAS";
            this.btnOkWAS.Size = new System.Drawing.Size(72, 72);
            this.btnOkWAS.TabIndex = 526;
            this.btnOkWAS.UseVisualStyleBackColor = false;
            this.btnOkWAS.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // chkInvertWAS
            // 
            this.chkInvertWAS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInvertWAS.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertWAS.BackColor = System.Drawing.Color.AliceBlue;
            this.chkInvertWAS.Checked = true;
            this.chkInvertWAS.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkInvertWAS.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertWAS.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkInvertWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertWAS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertWAS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkInvertWAS.Image = global::AgOpenGPS.Properties.Resources.ConSt_InvertWAS;
            this.chkInvertWAS.Location = new System.Drawing.Point(274, 155);
            this.chkInvertWAS.Name = "chkInvertWAS";
            this.chkInvertWAS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertWAS.Size = new System.Drawing.Size(109, 78);
            this.chkInvertWAS.TabIndex = 490;
            this.chkInvertWAS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertWAS.UseVisualStyleBackColor = false;
            this.chkInvertWAS.CheckedChanged += new System.EventHandler(this.chkInvertWAS_CheckedChanged);
            // 
            // tabWAS_Zero
            // 
            this.tabWAS_Zero.BackColor = System.Drawing.Color.White;
            this.tabWAS_Zero.Controls.Add(this.label112);
            this.tabWAS_Zero.Controls.Add(this.lblCurrentHeading);
            this.tabWAS_Zero.Controls.Add(this.label37);
            this.tabWAS_Zero.Controls.Add(this.button6);
            this.tabWAS_Zero.Controls.Add(this.btnRemoveWasOffset);
            this.tabWAS_Zero.Controls.Add(this.label40);
            this.tabWAS_Zero.Controls.Add(this.hsbarWasOffset);
            this.tabWAS_Zero.Controls.Add(this.lblSteerAngleSensorZero);
            this.tabWAS_Zero.Controls.Add(this.label10);
            this.tabWAS_Zero.Controls.Add(this.btnOkNext_WAS_Zero);
            this.tabWAS_Zero.Controls.Add(this.btnZeroWAS);
            this.tabWAS_Zero.Location = new System.Drawing.Point(4, 19);
            this.tabWAS_Zero.Name = "tabWAS_Zero";
            this.tabWAS_Zero.Size = new System.Drawing.Size(655, 348);
            this.tabWAS_Zero.TabIndex = 21;
            this.tabWAS_Zero.Text = "WAS_Zero";
            // 
            // label112
            // 
            this.label112.AutoSize = true;
            this.label112.BackColor = System.Drawing.Color.Transparent;
            this.label112.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label112.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label112.Location = new System.Drawing.Point(75, 161);
            this.label112.Name = "label112";
            this.label112.Size = new System.Drawing.Size(45, 16);
            this.label112.TabIndex = 532;
            this.label112.Text = "Reset";
            this.label112.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblCurrentHeading
            // 
            this.lblCurrentHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCurrentHeading.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentHeading.ForeColor = System.Drawing.Color.Black;
            this.lblCurrentHeading.Location = new System.Drawing.Point(278, 290);
            this.lblCurrentHeading.Name = "lblCurrentHeading";
            this.lblCurrentHeading.Size = new System.Drawing.Size(102, 35);
            this.lblCurrentHeading.TabIndex = 539;
            this.lblCurrentHeading.Text = "358.8";
            this.lblCurrentHeading.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label37
            // 
            this.label37.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label37.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.ForeColor = System.Drawing.Color.Black;
            this.label37.Location = new System.Drawing.Point(33, 289);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(248, 35);
            this.label37.TabIndex = 540;
            this.label37.Text = "Current Heading:";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button6.BackColor = System.Drawing.Color.Transparent;
            this.button6.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button6.FlatAppearance.BorderSize = 0;
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button6.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button6.Location = new System.Drawing.Point(485, 269);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(72, 72);
            this.button6.TabIndex = 538;
            this.button6.UseVisualStyleBackColor = false;
            this.button6.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnRemoveWasOffset
            // 
            this.btnRemoveWasOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRemoveWasOffset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRemoveWasOffset.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.btnRemoveWasOffset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRemoveWasOffset.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRemoveWasOffset.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnRemoveWasOffset.Image = global::AgOpenGPS.Properties.Resources.WizWasZeroReset;
            this.btnRemoveWasOffset.Location = new System.Drawing.Point(71, 180);
            this.btnRemoveWasOffset.Name = "btnRemoveWasOffset";
            this.btnRemoveWasOffset.Size = new System.Drawing.Size(100, 72);
            this.btnRemoveWasOffset.TabIndex = 533;
            this.btnRemoveWasOffset.UseVisualStyleBackColor = true;
            this.btnRemoveWasOffset.Click += new System.EventHandler(this.btnRemoveWasOffset_Click);
            // 
            // label40
            // 
            this.label40.AutoSize = true;
            this.label40.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label40.ForeColor = System.Drawing.Color.Black;
            this.label40.Location = new System.Drawing.Point(3, 4);
            this.label40.Name = "label40";
            this.label40.Size = new System.Drawing.Size(146, 25);
            this.label40.TabIndex = 532;
            this.label40.Text = "WAS Zeroing";
            this.label40.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnOkNext_WAS_Zero
            // 
            this.btnOkNext_WAS_Zero.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_WAS_Zero.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_WAS_Zero.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_WAS_Zero.FlatAppearance.BorderSize = 0;
            this.btnOkNext_WAS_Zero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_WAS_Zero.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_WAS_Zero.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_WAS_Zero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_WAS_Zero.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_WAS_Zero.Name = "btnOkNext_WAS_Zero";
            this.btnOkNext_WAS_Zero.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_WAS_Zero.TabIndex = 529;
            this.btnOkNext_WAS_Zero.UseVisualStyleBackColor = false;
            this.btnOkNext_WAS_Zero.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // btnZeroWAS
            // 
            this.btnZeroWAS.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZeroWAS.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnZeroWAS.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnZeroWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroWAS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroWAS.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZeroWAS.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnZeroWAS.Location = new System.Drawing.Point(273, 97);
            this.btnZeroWAS.Name = "btnZeroWAS";
            this.btnZeroWAS.Size = new System.Drawing.Size(100, 77);
            this.btnZeroWAS.TabIndex = 323;
            this.btnZeroWAS.UseVisualStyleBackColor = true;
            this.btnZeroWAS.Click += new System.EventHandler(this.btnZeroWAS_Click);
            // 
            // tabMotorDirection
            // 
            this.tabMotorDirection.BackColor = System.Drawing.Color.White;
            this.tabMotorDirection.Controls.Add(this.label58);
            this.tabMotorDirection.Controls.Add(this.label41);
            this.tabMotorDirection.Controls.Add(this.label17);
            this.tabMotorDirection.Controls.Add(this.label33);
            this.tabMotorDirection.Controls.Add(this.label70);
            this.tabMotorDirection.Controls.Add(this.btnPrev_MotorDirection);
            this.tabMotorDirection.Controls.Add(this.btnOkNext_MotorDirection);
            this.tabMotorDirection.Controls.Add(this.btnSteerLeft);
            this.tabMotorDirection.Controls.Add(this.btnSteerRight);
            this.tabMotorDirection.Controls.Add(this.chkInvertSteer);
            this.tabMotorDirection.Location = new System.Drawing.Point(4, 19);
            this.tabMotorDirection.Name = "tabMotorDirection";
            this.tabMotorDirection.Size = new System.Drawing.Size(655, 348);
            this.tabMotorDirection.TabIndex = 17;
            this.tabMotorDirection.Text = "MotorDirection";
            this.tabMotorDirection.Enter += new System.EventHandler(this.tabMotorDirection_Enter);
            this.tabMotorDirection.Leave += new System.EventHandler(this.tabMotorDirection_Leave);
            // 
            // label58
            // 
            this.label58.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label58.AutoSize = true;
            this.label58.BackColor = System.Drawing.Color.Transparent;
            this.label58.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label58.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label58.Location = new System.Drawing.Point(274, 236);
            this.label58.Name = "label58";
            this.label58.Size = new System.Drawing.Size(110, 16);
            this.label58.TabIndex = 540;
            this.label58.Text = "Invert Direction";
            this.label58.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label41
            // 
            this.label41.AutoSize = true;
            this.label41.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.ForeColor = System.Drawing.Color.Black;
            this.label41.Location = new System.Drawing.Point(3, 4);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(237, 25);
            this.label41.TabIndex = 532;
            this.label41.Text = "Steer Motor Direction";
            this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label17
            // 
            this.label17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label17.Location = new System.Drawing.Point(174, 241);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(33, 16);
            this.label17.TabIndex = 530;
            this.label17.Text = "Left";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label33
            // 
            this.label33.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label33.AutoSize = true;
            this.label33.BackColor = System.Drawing.Color.Transparent;
            this.label33.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label33.Location = new System.Drawing.Point(442, 241);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(41, 16);
            this.label33.TabIndex = 529;
            this.label33.Text = "Right";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_MotorDirection
            // 
            this.btnPrev_MotorDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_MotorDirection.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_MotorDirection.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_MotorDirection.FlatAppearance.BorderSize = 0;
            this.btnPrev_MotorDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_MotorDirection.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_MotorDirection.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_MotorDirection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_MotorDirection.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_MotorDirection.Name = "btnPrev_MotorDirection";
            this.btnPrev_MotorDirection.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_MotorDirection.TabIndex = 537;
            this.btnPrev_MotorDirection.UseVisualStyleBackColor = false;
            this.btnPrev_MotorDirection.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnOkNext_MotorDirection
            // 
            this.btnOkNext_MotorDirection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_MotorDirection.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_MotorDirection.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_MotorDirection.FlatAppearance.BorderSize = 0;
            this.btnOkNext_MotorDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_MotorDirection.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_MotorDirection.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_MotorDirection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_MotorDirection.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_MotorDirection.Name = "btnOkNext_MotorDirection";
            this.btnOkNext_MotorDirection.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_MotorDirection.TabIndex = 528;
            this.btnOkNext_MotorDirection.UseVisualStyleBackColor = false;
            this.btnOkNext_MotorDirection.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // btnSteerLeft
            // 
            this.btnSteerLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSteerLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnSteerLeft.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.btnSteerLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSteerLeft.Image = global::AgOpenGPS.Properties.Resources.SteerLeft;
            this.btnSteerLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSteerLeft.Location = new System.Drawing.Point(150, 158);
            this.btnSteerLeft.Name = "btnSteerLeft";
            this.btnSteerLeft.Size = new System.Drawing.Size(76, 81);
            this.btnSteerLeft.TabIndex = 525;
            this.btnSteerLeft.UseVisualStyleBackColor = false;
            this.btnSteerLeft.Click += new System.EventHandler(this.btnSteerLeft_Click);
            // 
            // btnSteerRight
            // 
            this.btnSteerRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSteerRight.BackColor = System.Drawing.Color.Transparent;
            this.btnSteerRight.FlatAppearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.btnSteerRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSteerRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSteerRight.Image = global::AgOpenGPS.Properties.Resources.SteerRight;
            this.btnSteerRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSteerRight.Location = new System.Drawing.Point(425, 158);
            this.btnSteerRight.Name = "btnSteerRight";
            this.btnSteerRight.Size = new System.Drawing.Size(76, 80);
            this.btnSteerRight.TabIndex = 526;
            this.btnSteerRight.UseVisualStyleBackColor = false;
            this.btnSteerRight.Click += new System.EventHandler(this.btnSteerRight_Click);
            // 
            // chkInvertSteer
            // 
            this.chkInvertSteer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkInvertSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertSteer.BackColor = System.Drawing.Color.AliceBlue;
            this.chkInvertSteer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.chkInvertSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertSteer.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkInvertSteer.Image = global::AgOpenGPS.Properties.Resources.ConSt_InvertDirection;
            this.chkInvertSteer.Location = new System.Drawing.Point(274, 155);
            this.chkInvertSteer.Name = "chkInvertSteer";
            this.chkInvertSteer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertSteer.Size = new System.Drawing.Size(109, 78);
            this.chkInvertSteer.TabIndex = 491;
            this.chkInvertSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertSteer.UseVisualStyleBackColor = false;
            this.chkInvertSteer.CheckedChanged += new System.EventHandler(this.chkInvertSteer_CheckedChanged);
            // 
            // tabCPD_Setup
            // 
            this.tabCPD_Setup.BackColor = System.Drawing.Color.White;
            this.tabCPD_Setup.Controls.Add(this.label60);
            this.tabCPD_Setup.Controls.Add(this.btnPrev_CountsPerDegree);
            this.tabCPD_Setup.Controls.Add(this.label74);
            this.tabCPD_Setup.Controls.Add(this.label75);
            this.tabCPD_Setup.Controls.Add(this.btnSkipCPD_Setup);
            this.tabCPD_Setup.Controls.Add(this.btnOKNext_CPDSetup);
            this.tabCPD_Setup.Location = new System.Drawing.Point(4, 19);
            this.tabCPD_Setup.Name = "tabCPD_Setup";
            this.tabCPD_Setup.Size = new System.Drawing.Size(655, 348);
            this.tabCPD_Setup.TabIndex = 33;
            this.tabCPD_Setup.Text = "CPD_Setup";
            // 
            // label60
            // 
            this.label60.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label60.BackColor = System.Drawing.Color.Transparent;
            this.label60.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label60.ForeColor = System.Drawing.Color.Black;
            this.label60.Location = new System.Drawing.Point(13, 263);
            this.label60.Name = "label60";
            this.label60.Size = new System.Drawing.Size(468, 35);
            this.label60.TabIndex = 544;
            this.label60.Text = "* Begin * Calibration";
            this.label60.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_CountsPerDegree
            // 
            this.btnPrev_CountsPerDegree.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_CountsPerDegree.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_CountsPerDegree.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnPrev_CountsPerDegree.FlatAppearance.BorderSize = 0;
            this.btnPrev_CountsPerDegree.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_CountsPerDegree.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_CountsPerDegree.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_CountsPerDegree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_CountsPerDegree.Location = new System.Drawing.Point(485, 244);
            this.btnPrev_CountsPerDegree.Name = "btnPrev_CountsPerDegree";
            this.btnPrev_CountsPerDegree.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_CountsPerDegree.TabIndex = 537;
            this.btnPrev_CountsPerDegree.UseVisualStyleBackColor = false;
            this.btnPrev_CountsPerDegree.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.BackColor = System.Drawing.Color.Transparent;
            this.label74.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label74.ForeColor = System.Drawing.Color.Black;
            this.label74.Location = new System.Drawing.Point(5, 5);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(182, 25);
            this.label74.TabIndex = 543;
            this.label74.Text = "WAS Calibration";
            this.label74.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label75
            // 
            this.label75.BackColor = System.Drawing.Color.Transparent;
            this.label75.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label75.ForeColor = System.Drawing.Color.Black;
            this.label75.Location = new System.Drawing.Point(79, 97);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(402, 86);
            this.label75.TabIndex = 542;
            this.label75.Text = "* SKIP *  Counts Per Degree and Ackermann Setting?";
            this.label75.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnSkipCPD_Setup
            // 
            this.btnSkipCPD_Setup.BackColor = System.Drawing.Color.Transparent;
            this.btnSkipCPD_Setup.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnSkipCPD_Setup.FlatAppearance.BorderSize = 0;
            this.btnSkipCPD_Setup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSkipCPD_Setup.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSkipCPD_Setup.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnSkipCPD_Setup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSkipCPD_Setup.Location = new System.Drawing.Point(487, 112);
            this.btnSkipCPD_Setup.Name = "btnSkipCPD_Setup";
            this.btnSkipCPD_Setup.Size = new System.Drawing.Size(72, 61);
            this.btnSkipCPD_Setup.TabIndex = 539;
            this.btnSkipCPD_Setup.UseVisualStyleBackColor = false;
            this.btnSkipCPD_Setup.Click += new System.EventHandler(this.btnSkipCPD_Setup_Click);
            // 
            // btnOKNext_CPDSetup
            // 
            this.btnOKNext_CPDSetup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOKNext_CPDSetup.BackColor = System.Drawing.Color.Transparent;
            this.btnOKNext_CPDSetup.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOKNext_CPDSetup.FlatAppearance.BorderSize = 0;
            this.btnOKNext_CPDSetup.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOKNext_CPDSetup.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOKNext_CPDSetup.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOKNext_CPDSetup.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOKNext_CPDSetup.Location = new System.Drawing.Point(576, 244);
            this.btnOKNext_CPDSetup.Name = "btnOKNext_CPDSetup";
            this.btnOKNext_CPDSetup.Size = new System.Drawing.Size(72, 72);
            this.btnOKNext_CPDSetup.TabIndex = 541;
            this.btnOKNext_CPDSetup.UseVisualStyleBackColor = false;
            this.btnOKNext_CPDSetup.Click += new System.EventHandler(this.btnOKNext_CPDSetup_Click);
            // 
            // tabCountsPerDeg
            // 
            this.tabCountsPerDeg.BackColor = System.Drawing.Color.White;
            this.tabCountsPerDeg.Controls.Add(this.label109);
            this.tabCountsPerDeg.Controls.Add(this.lblRightStartAngle);
            this.tabCountsPerDeg.Controls.Add(this.button5);
            this.tabCountsPerDeg.Controls.Add(this.lblCPDError);
            this.tabCountsPerDeg.Controls.Add(this.label39);
            this.tabCountsPerDeg.Controls.Add(this.hsbarCountsPerDegree);
            this.tabCountsPerDeg.Controls.Add(this.label8);
            this.tabCountsPerDeg.Controls.Add(this.lblCountsPerDegree);
            this.tabCountsPerDeg.Controls.Add(this.label7);
            this.tabCountsPerDeg.Controls.Add(this.lblDiameter);
            this.tabCountsPerDeg.Controls.Add(this.lblCalcSteerAngleInner);
            this.tabCountsPerDeg.Controls.Add(this.label1);
            this.tabCountsPerDeg.Controls.Add(this.btnOkNext_CountsPerDeg);
            this.tabCountsPerDeg.Controls.Add(this.btnStartSA);
            this.tabCountsPerDeg.Location = new System.Drawing.Point(4, 19);
            this.tabCountsPerDeg.Name = "tabCountsPerDeg";
            this.tabCountsPerDeg.Size = new System.Drawing.Size(655, 348);
            this.tabCountsPerDeg.TabIndex = 22;
            this.tabCountsPerDeg.Text = "CountsPerDeg";
            // 
            // label109
            // 
            this.label109.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label109.AutoSize = true;
            this.label109.BackColor = System.Drawing.Color.Transparent;
            this.label109.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label109.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label109.Location = new System.Drawing.Point(300, 125);
            this.label109.Name = "label109";
            this.label109.Size = new System.Drawing.Size(124, 25);
            this.label109.TabIndex = 557;
            this.label109.Text = "Start Angle:";
            this.label109.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRightStartAngle
            // 
            this.lblRightStartAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRightStartAngle.AutoSize = true;
            this.lblRightStartAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblRightStartAngle.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRightStartAngle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblRightStartAngle.Location = new System.Drawing.Point(419, 125);
            this.lblRightStartAngle.Name = "lblRightStartAngle";
            this.lblRightStartAngle.Size = new System.Drawing.Size(45, 25);
            this.lblRightStartAngle.TabIndex = 556;
            this.lblRightStartAngle.Text = "0.0";
            this.lblRightStartAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button5
            // 
            this.button5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button5.BackColor = System.Drawing.Color.Transparent;
            this.button5.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button5.FlatAppearance.BorderSize = 0;
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button5.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button5.Location = new System.Drawing.Point(485, 269);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(72, 72);
            this.button5.TabIndex = 538;
            this.button5.UseVisualStyleBackColor = false;
            this.button5.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblCPDError
            // 
            this.lblCPDError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCPDError.AutoSize = true;
            this.lblCPDError.BackColor = System.Drawing.Color.Transparent;
            this.lblCPDError.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPDError.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCPDError.Location = new System.Drawing.Point(155, 235);
            this.lblCPDError.Name = "lblCPDError";
            this.lblCPDError.Size = new System.Drawing.Size(33, 25);
            this.lblCPDError.TabIndex = 532;
            this.lblCPDError.Text = "...";
            this.lblCPDError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label39
            // 
            this.label39.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.ForeColor = System.Drawing.Color.Black;
            this.label39.Location = new System.Drawing.Point(3, 3);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(258, 26);
            this.label39.TabIndex = 531;
            this.label39.Text = "Counts per Degree";
            this.label39.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOkNext_CountsPerDeg
            // 
            this.btnOkNext_CountsPerDeg.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_CountsPerDeg.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_CountsPerDeg.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_CountsPerDeg.FlatAppearance.BorderSize = 0;
            this.btnOkNext_CountsPerDeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_CountsPerDeg.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_CountsPerDeg.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_CountsPerDeg.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_CountsPerDeg.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_CountsPerDeg.Name = "btnOkNext_CountsPerDeg";
            this.btnOkNext_CountsPerDeg.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_CountsPerDeg.TabIndex = 529;
            this.btnOkNext_CountsPerDeg.UseVisualStyleBackColor = false;
            this.btnOkNext_CountsPerDeg.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // btnStartSA
            // 
            this.btnStartSA.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartSA.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStartSA.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartSA.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStartSA.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.btnStartSA.Location = new System.Drawing.Point(188, 129);
            this.btnStartSA.Name = "btnStartSA";
            this.btnStartSA.Size = new System.Drawing.Size(73, 78);
            this.btnStartSA.TabIndex = 323;
            this.btnStartSA.UseVisualStyleBackColor = true;
            this.btnStartSA.Click += new System.EventHandler(this.btnStartSA_Click);
            // 
            // tabAckCPD
            // 
            this.tabAckCPD.BackColor = System.Drawing.Color.White;
            this.tabAckCPD.Controls.Add(this.btnAckReset);
            this.tabAckCPD.Controls.Add(this.label101);
            this.tabAckCPD.Controls.Add(this.lblStartAngleLeft);
            this.tabAckCPD.Controls.Add(this.label32);
            this.tabAckCPD.Controls.Add(this.hsbarAckerman);
            this.tabAckCPD.Controls.Add(this.lblAckerman);
            this.tabAckCPD.Controls.Add(this.button16);
            this.tabAckCPD.Controls.Add(this.lblAckermannError);
            this.tabAckCPD.Controls.Add(this.label105);
            this.tabAckCPD.Controls.Add(this.label106);
            this.tabAckCPD.Controls.Add(this.label108);
            this.tabAckCPD.Controls.Add(this.lblDiameterLeft);
            this.tabAckCPD.Controls.Add(this.lblCalcSteerAngleLeft);
            this.tabAckCPD.Controls.Add(this.label111);
            this.tabAckCPD.Controls.Add(this.button17);
            this.tabAckCPD.Controls.Add(this.btnStartSA_Left);
            this.tabAckCPD.Location = new System.Drawing.Point(4, 19);
            this.tabAckCPD.Name = "tabAckCPD";
            this.tabAckCPD.Size = new System.Drawing.Size(655, 348);
            this.tabAckCPD.TabIndex = 43;
            this.tabAckCPD.Text = "AckCPD";
            // 
            // btnAckReset
            // 
            this.btnAckReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAckReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAckReset.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAckReset.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnAckReset.Location = new System.Drawing.Point(113, 130);
            this.btnAckReset.Name = "btnAckReset";
            this.btnAckReset.Size = new System.Drawing.Size(73, 78);
            this.btnAckReset.TabIndex = 556;
            this.btnAckReset.Text = "Reset To 100%";
            this.btnAckReset.UseVisualStyleBackColor = true;
            this.btnAckReset.Click += new System.EventHandler(this.btnAckReset_Click);
            // 
            // label101
            // 
            this.label101.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label101.AutoSize = true;
            this.label101.BackColor = System.Drawing.Color.Transparent;
            this.label101.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label101.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label101.Location = new System.Drawing.Point(337, 125);
            this.label101.Name = "label101";
            this.label101.Size = new System.Drawing.Size(124, 25);
            this.label101.TabIndex = 555;
            this.label101.Text = "Start Angle:";
            this.label101.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStartAngleLeft
            // 
            this.lblStartAngleLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStartAngleLeft.AutoSize = true;
            this.lblStartAngleLeft.BackColor = System.Drawing.Color.Transparent;
            this.lblStartAngleLeft.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartAngleLeft.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblStartAngleLeft.Location = new System.Drawing.Point(456, 125);
            this.lblStartAngleLeft.Name = "lblStartAngleLeft";
            this.lblStartAngleLeft.Size = new System.Drawing.Size(45, 25);
            this.lblStartAngleLeft.TabIndex = 554;
            this.lblStartAngleLeft.Text = "0.0";
            this.lblStartAngleLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label32
            // 
            this.label32.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label32.BackColor = System.Drawing.Color.Transparent;
            this.label32.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label32.ForeColor = System.Drawing.Color.Black;
            this.label32.Location = new System.Drawing.Point(165, 254);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(266, 29);
            this.label32.TabIndex = 553;
            this.label32.Text = "Ackermann Value:";
            this.label32.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarAckerman
            // 
            this.hsbarAckerman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.hsbarAckerman.LargeChange = 1;
            this.hsbarAckerman.Location = new System.Drawing.Point(164, 285);
            this.hsbarAckerman.Maximum = 200;
            this.hsbarAckerman.Minimum = 1;
            this.hsbarAckerman.Name = "hsbarAckerman";
            this.hsbarAckerman.Size = new System.Drawing.Size(270, 50);
            this.hsbarAckerman.TabIndex = 551;
            this.hsbarAckerman.Value = 100;
            this.hsbarAckerman.ValueChanged += new System.EventHandler(this.hsbarAckerman_ValueChanged);
            // 
            // lblAckerman
            // 
            this.lblAckerman.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAckerman.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAckerman.ForeColor = System.Drawing.Color.Black;
            this.lblAckerman.Location = new System.Drawing.Point(64, 290);
            this.lblAckerman.Name = "lblAckerman";
            this.lblAckerman.Size = new System.Drawing.Size(91, 35);
            this.lblAckerman.TabIndex = 552;
            this.lblAckerman.Text = "888";
            this.lblAckerman.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button16
            // 
            this.button16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button16.BackColor = System.Drawing.Color.Transparent;
            this.button16.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button16.FlatAppearance.BorderSize = 0;
            this.button16.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button16.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button16.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.button16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button16.Location = new System.Drawing.Point(486, 271);
            this.button16.Name = "button16";
            this.button16.Size = new System.Drawing.Size(72, 72);
            this.button16.TabIndex = 550;
            this.button16.UseVisualStyleBackColor = false;
            this.button16.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // lblAckermannError
            // 
            this.lblAckermannError.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAckermannError.AutoSize = true;
            this.lblAckermannError.BackColor = System.Drawing.Color.Transparent;
            this.lblAckermannError.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAckermannError.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblAckermannError.Location = new System.Drawing.Point(18, 219);
            this.lblAckermannError.Name = "lblAckermannError";
            this.lblAckermannError.Size = new System.Drawing.Size(33, 25);
            this.lblAckermannError.TabIndex = 549;
            this.lblAckermannError.Text = "...";
            this.lblAckermannError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label105
            // 
            this.label105.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label105.ForeColor = System.Drawing.Color.Black;
            this.label105.Location = new System.Drawing.Point(4, 5);
            this.label105.Name = "label105";
            this.label105.Size = new System.Drawing.Size(258, 26);
            this.label105.TabIndex = 548;
            this.label105.Text = "Ackermann";
            this.label105.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label106
            // 
            this.label106.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label106.AutoSize = true;
            this.label106.BackColor = System.Drawing.Color.Transparent;
            this.label106.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label106.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label106.Location = new System.Drawing.Point(333, 155);
            this.label106.Name = "label106";
            this.label106.Size = new System.Drawing.Size(128, 25);
            this.label106.TabIndex = 545;
            this.label106.Text = "Steer Angle:";
            this.label106.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label108
            // 
            this.label108.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label108.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label108.ForeColor = System.Drawing.Color.Black;
            this.label108.Location = new System.Drawing.Point(18, 43);
            this.label108.Name = "label108";
            this.label108.Size = new System.Drawing.Size(632, 57);
            this.label108.TabIndex = 546;
            this.label108.Text = "Turn steering wheel to LEFT roughly the same degrees as right.\r\nWhile driving in " +
    "a steady circle, Press Rec and wait.";
            this.label108.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblDiameterLeft
            // 
            this.lblDiameterLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDiameterLeft.AutoSize = true;
            this.lblDiameterLeft.BackColor = System.Drawing.Color.Transparent;
            this.lblDiameterLeft.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDiameterLeft.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblDiameterLeft.Location = new System.Drawing.Point(456, 185);
            this.lblDiameterLeft.Name = "lblDiameterLeft";
            this.lblDiameterLeft.Size = new System.Drawing.Size(71, 25);
            this.lblDiameterLeft.TabIndex = 542;
            this.lblDiameterLeft.Text = "0.0 m";
            this.lblDiameterLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblCalcSteerAngleLeft
            // 
            this.lblCalcSteerAngleLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblCalcSteerAngleLeft.AutoSize = true;
            this.lblCalcSteerAngleLeft.BackColor = System.Drawing.Color.Transparent;
            this.lblCalcSteerAngleLeft.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCalcSteerAngleLeft.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCalcSteerAngleLeft.Location = new System.Drawing.Point(456, 155);
            this.lblCalcSteerAngleLeft.Name = "lblCalcSteerAngleLeft";
            this.lblCalcSteerAngleLeft.Size = new System.Drawing.Size(45, 25);
            this.lblCalcSteerAngleLeft.TabIndex = 543;
            this.lblCalcSteerAngleLeft.Text = "0.0";
            this.lblCalcSteerAngleLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label111
            // 
            this.label111.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label111.AutoSize = true;
            this.label111.BackColor = System.Drawing.Color.Transparent;
            this.label111.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label111.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label111.Location = new System.Drawing.Point(357, 185);
            this.label111.Name = "label111";
            this.label111.Size = new System.Drawing.Size(104, 25);
            this.label111.TabIndex = 544;
            this.label111.Text = "Diameter:";
            this.label111.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // button17
            // 
            this.button17.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button17.BackColor = System.Drawing.Color.Transparent;
            this.button17.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.button17.FlatAppearance.BorderSize = 0;
            this.button17.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button17.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button17.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.button17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button17.Location = new System.Drawing.Point(577, 271);
            this.button17.Name = "button17";
            this.button17.Size = new System.Drawing.Size(72, 72);
            this.button17.TabIndex = 547;
            this.button17.UseVisualStyleBackColor = false;
            this.button17.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // btnStartSA_Left
            // 
            this.btnStartSA_Left.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartSA_Left.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnStartSA_Left.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartSA_Left.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStartSA_Left.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.btnStartSA_Left.Location = new System.Drawing.Point(238, 130);
            this.btnStartSA_Left.Name = "btnStartSA_Left";
            this.btnStartSA_Left.Size = new System.Drawing.Size(73, 78);
            this.btnStartSA_Left.TabIndex = 541;
            this.btnStartSA_Left.UseVisualStyleBackColor = true;
            this.btnStartSA_Left.Click += new System.EventHandler(this.btnStartSA_Left_Click);
            // 
            // tabMaxSteerAngle
            // 
            this.tabMaxSteerAngle.BackColor = System.Drawing.Color.White;
            this.tabMaxSteerAngle.Controls.Add(this.label113);
            this.tabMaxSteerAngle.Controls.Add(this.btnOkSetMaximumSteerAngle);
            this.tabMaxSteerAngle.Controls.Add(this.label73);
            this.tabMaxSteerAngle.Controls.Add(this.btnPrev_MaxSteerAngle);
            this.tabMaxSteerAngle.Controls.Add(this.label49);
            this.tabMaxSteerAngle.Controls.Add(this.label23);
            this.tabMaxSteerAngle.Controls.Add(this.lblMaxSteerAngle);
            this.tabMaxSteerAngle.Controls.Add(this.hsbarMaxSteerAngle);
            this.tabMaxSteerAngle.Controls.Add(this.btnOkNextMaxSteerAngle);
            this.tabMaxSteerAngle.Location = new System.Drawing.Point(4, 19);
            this.tabMaxSteerAngle.Name = "tabMaxSteerAngle";
            this.tabMaxSteerAngle.Size = new System.Drawing.Size(655, 348);
            this.tabMaxSteerAngle.TabIndex = 26;
            this.tabMaxSteerAngle.Text = "MaxSteerAngle";
            // 
            // label113
            // 
            this.label113.AutoSize = true;
            this.label113.BackColor = System.Drawing.Color.Transparent;
            this.label113.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label113.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label113.Location = new System.Drawing.Point(503, 166);
            this.label113.Name = "label113";
            this.label113.Size = new System.Drawing.Size(29, 16);
            this.label113.TabIndex = 532;
            this.label113.Text = "Set";
            this.label113.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOkSetMaximumSteerAngle
            // 
            this.btnOkSetMaximumSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkSetMaximumSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnOkSetMaximumSteerAngle.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnOkSetMaximumSteerAngle.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnOkSetMaximumSteerAngle.FlatAppearance.BorderSize = 0;
            this.btnOkSetMaximumSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkSetMaximumSteerAngle.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkSetMaximumSteerAngle.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnOkSetMaximumSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkSetMaximumSteerAngle.Location = new System.Drawing.Point(464, 92);
            this.btnOkSetMaximumSteerAngle.Name = "btnOkSetMaximumSteerAngle";
            this.btnOkSetMaximumSteerAngle.Size = new System.Drawing.Size(104, 72);
            this.btnOkSetMaximumSteerAngle.TabIndex = 541;
            this.btnOkSetMaximumSteerAngle.UseVisualStyleBackColor = false;
            this.btnOkSetMaximumSteerAngle.Click += new System.EventHandler(this.btnOkSetMaximumSteerAngle_Click);
            // 
            // label73
            // 
            this.label73.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label73.BackColor = System.Drawing.Color.Transparent;
            this.label73.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label73.ForeColor = System.Drawing.Color.Black;
            this.label73.Location = new System.Drawing.Point(44, 99);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(401, 56);
            this.label73.TabIndex = 538;
            this.label73.Text = "Turn steering to your desired limit for guidance and touch OK ->";
            this.label73.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_MaxSteerAngle
            // 
            this.btnPrev_MaxSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_MaxSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_MaxSteerAngle.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_MaxSteerAngle.FlatAppearance.BorderSize = 0;
            this.btnPrev_MaxSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_MaxSteerAngle.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_MaxSteerAngle.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_MaxSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_MaxSteerAngle.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_MaxSteerAngle.Name = "btnPrev_MaxSteerAngle";
            this.btnPrev_MaxSteerAngle.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_MaxSteerAngle.TabIndex = 537;
            this.btnPrev_MaxSteerAngle.UseVisualStyleBackColor = false;
            this.btnPrev_MaxSteerAngle.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label49
            // 
            this.label49.AutoSize = true;
            this.label49.BackColor = System.Drawing.Color.Transparent;
            this.label49.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label49.ForeColor = System.Drawing.Color.Black;
            this.label49.Location = new System.Drawing.Point(5, 5);
            this.label49.Name = "label49";
            this.label49.Size = new System.Drawing.Size(293, 25);
            this.label49.TabIndex = 532;
            this.label49.Text = "Guidance Steer Angle Limit";
            this.label49.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label23
            // 
            this.label23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.ForeColor = System.Drawing.Color.Black;
            this.label23.Location = new System.Drawing.Point(21, 206);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(572, 29);
            this.label23.TabIndex = 531;
            this.label23.Text = "Or Manually Set Guidance Steer Angle Limit";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnOkNextMaxSteerAngle
            // 
            this.btnOkNextMaxSteerAngle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNextMaxSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNextMaxSteerAngle.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnOkNextMaxSteerAngle.FlatAppearance.BorderSize = 0;
            this.btnOkNextMaxSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNextMaxSteerAngle.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNextMaxSteerAngle.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNextMaxSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNextMaxSteerAngle.Location = new System.Drawing.Point(576, 269);
            this.btnOkNextMaxSteerAngle.Name = "btnOkNextMaxSteerAngle";
            this.btnOkNextMaxSteerAngle.Size = new System.Drawing.Size(72, 72);
            this.btnOkNextMaxSteerAngle.TabIndex = 530;
            this.btnOkNextMaxSteerAngle.UseVisualStyleBackColor = false;
            this.btnOkNextMaxSteerAngle.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabCancelGuidance
            // 
            this.tabCancelGuidance.BackColor = System.Drawing.Color.White;
            this.tabCancelGuidance.Controls.Add(this.btnPrev_CancelGuidance);
            this.tabCancelGuidance.Controls.Add(this.btnOkNextCancelGuidance);
            this.tabCancelGuidance.Controls.Add(this.label44);
            this.tabCancelGuidance.Controls.Add(this.labelEncoder);
            this.tabCancelGuidance.Controls.Add(this.labelCurrentTurnSensor);
            this.tabCancelGuidance.Controls.Add(this.labelPressureTurnSensor);
            this.tabCancelGuidance.Controls.Add(this.pbarSensor);
            this.tabCancelGuidance.Controls.Add(this.nudMaxCounts);
            this.tabCancelGuidance.Controls.Add(this.label61);
            this.tabCancelGuidance.Controls.Add(this.hsbarSensor);
            this.tabCancelGuidance.Controls.Add(this.lblPercentFS);
            this.tabCancelGuidance.Controls.Add(this.lblhsbarSensor);
            this.tabCancelGuidance.Controls.Add(this.cboxPressureSensor);
            this.tabCancelGuidance.Controls.Add(this.cboxEncoder);
            this.tabCancelGuidance.Controls.Add(this.cboxCurrentSensor);
            this.tabCancelGuidance.Location = new System.Drawing.Point(4, 19);
            this.tabCancelGuidance.Name = "tabCancelGuidance";
            this.tabCancelGuidance.Size = new System.Drawing.Size(655, 348);
            this.tabCancelGuidance.TabIndex = 20;
            this.tabCancelGuidance.Text = "CancelGuidance";
            // 
            // btnPrev_CancelGuidance
            // 
            this.btnPrev_CancelGuidance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_CancelGuidance.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_CancelGuidance.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_CancelGuidance.FlatAppearance.BorderSize = 0;
            this.btnPrev_CancelGuidance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_CancelGuidance.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_CancelGuidance.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_CancelGuidance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_CancelGuidance.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_CancelGuidance.Name = "btnPrev_CancelGuidance";
            this.btnPrev_CancelGuidance.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_CancelGuidance.TabIndex = 536;
            this.btnPrev_CancelGuidance.UseVisualStyleBackColor = false;
            this.btnPrev_CancelGuidance.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnOkNextCancelGuidance
            // 
            this.btnOkNextCancelGuidance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNextCancelGuidance.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNextCancelGuidance.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNextCancelGuidance.FlatAppearance.BorderSize = 0;
            this.btnOkNextCancelGuidance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNextCancelGuidance.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNextCancelGuidance.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNextCancelGuidance.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNextCancelGuidance.Location = new System.Drawing.Point(576, 269);
            this.btnOkNextCancelGuidance.Name = "btnOkNextCancelGuidance";
            this.btnOkNextCancelGuidance.Size = new System.Drawing.Size(72, 72);
            this.btnOkNextCancelGuidance.TabIndex = 533;
            this.btnOkNextCancelGuidance.UseVisualStyleBackColor = false;
            this.btnOkNextCancelGuidance.Click += new System.EventHandler(this.btnOkNextCancelGuidance_Click);
            // 
            // label44
            // 
            this.label44.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label44.ForeColor = System.Drawing.Color.Black;
            this.label44.Location = new System.Drawing.Point(3, 5);
            this.label44.Name = "label44";
            this.label44.Size = new System.Drawing.Size(557, 26);
            this.label44.TabIndex = 532;
            this.label44.Text = "Automatic Canceling Feature";
            this.label44.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cboxPressureSensor
            // 
            this.cboxPressureSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxPressureSensor.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxPressureSensor.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxPressureSensor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxPressureSensor.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxPressureSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxPressureSensor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxPressureSensor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxPressureSensor.Image = global::AgOpenGPS.Properties.Resources.ConSt_TurnSensorPressure;
            this.cboxPressureSensor.Location = new System.Drawing.Point(427, 110);
            this.cboxPressureSensor.Name = "cboxPressureSensor";
            this.cboxPressureSensor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxPressureSensor.Size = new System.Drawing.Size(114, 78);
            this.cboxPressureSensor.TabIndex = 508;
            this.cboxPressureSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxPressureSensor.UseVisualStyleBackColor = false;
            this.cboxPressureSensor.Click += new System.EventHandler(this.cboxCancelGuidance_Click);
            // 
            // cboxEncoder
            // 
            this.cboxEncoder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxEncoder.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxEncoder.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxEncoder.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxEncoder.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxEncoder.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxEncoder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxEncoder.Image = global::AgOpenGPS.Properties.Resources.ConSt_TurnSensor;
            this.cboxEncoder.Location = new System.Drawing.Point(129, 109);
            this.cboxEncoder.Name = "cboxEncoder";
            this.cboxEncoder.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxEncoder.Size = new System.Drawing.Size(114, 78);
            this.cboxEncoder.TabIndex = 492;
            this.cboxEncoder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxEncoder.UseVisualStyleBackColor = false;
            this.cboxEncoder.Click += new System.EventHandler(this.cboxCancelGuidance_Click);
            // 
            // cboxCurrentSensor
            // 
            this.cboxCurrentSensor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxCurrentSensor.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxCurrentSensor.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxCurrentSensor.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxCurrentSensor.FlatAppearance.CheckedBackColor = System.Drawing.Color.MediumAquamarine;
            this.cboxCurrentSensor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxCurrentSensor.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxCurrentSensor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.cboxCurrentSensor.Image = global::AgOpenGPS.Properties.Resources.ConSt_TurnSensorCurrent;
            this.cboxCurrentSensor.Location = new System.Drawing.Point(278, 109);
            this.cboxCurrentSensor.Name = "cboxCurrentSensor";
            this.cboxCurrentSensor.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxCurrentSensor.Size = new System.Drawing.Size(114, 78);
            this.cboxCurrentSensor.TabIndex = 510;
            this.cboxCurrentSensor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxCurrentSensor.UseVisualStyleBackColor = false;
            this.cboxCurrentSensor.Click += new System.EventHandler(this.cboxCancelGuidance_Click);
            // 
            // tabPanicStop
            // 
            this.tabPanicStop.BackColor = System.Drawing.Color.White;
            this.tabPanicStop.Controls.Add(this.btnPrev_Panic);
            this.tabPanicStop.Controls.Add(this.label54);
            this.tabPanicStop.Controls.Add(this.btnOkNext_PanicStop);
            this.tabPanicStop.Location = new System.Drawing.Point(4, 19);
            this.tabPanicStop.Name = "tabPanicStop";
            this.tabPanicStop.Size = new System.Drawing.Size(655, 348);
            this.tabPanicStop.TabIndex = 30;
            this.tabPanicStop.Text = "PanicStop";
            // 
            // btnPrev_Panic
            // 
            this.btnPrev_Panic.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_Panic.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_Panic.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_Panic.FlatAppearance.BorderSize = 0;
            this.btnPrev_Panic.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_Panic.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_Panic.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_Panic.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_Panic.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_Panic.Name = "btnPrev_Panic";
            this.btnPrev_Panic.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_Panic.TabIndex = 535;
            this.btnPrev_Panic.UseVisualStyleBackColor = false;
            this.btnPrev_Panic.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // label54
            // 
            this.label54.AutoSize = true;
            this.label54.BackColor = System.Drawing.Color.Transparent;
            this.label54.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label54.ForeColor = System.Drawing.Color.Black;
            this.label54.Location = new System.Drawing.Point(5, 5);
            this.label54.Name = "label54";
            this.label54.Size = new System.Drawing.Size(192, 25);
            this.label54.TabIndex = 534;
            this.label54.Text = "Panic Stop Speed";
            this.label54.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOkNext_PanicStop
            // 
            this.btnOkNext_PanicStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOkNext_PanicStop.BackColor = System.Drawing.Color.Transparent;
            this.btnOkNext_PanicStop.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnOkNext_PanicStop.FlatAppearance.BorderSize = 0;
            this.btnOkNext_PanicStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOkNext_PanicStop.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOkNext_PanicStop.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOkNext_PanicStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOkNext_PanicStop.Location = new System.Drawing.Point(576, 269);
            this.btnOkNext_PanicStop.Name = "btnOkNext_PanicStop";
            this.btnOkNext_PanicStop.Size = new System.Drawing.Size(72, 72);
            this.btnOkNext_PanicStop.TabIndex = 533;
            this.btnOkNext_PanicStop.UseVisualStyleBackColor = false;
            this.btnOkNext_PanicStop.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tab_MinimumGain
            // 
            this.tab_MinimumGain.BackColor = System.Drawing.Color.White;
            this.tab_MinimumGain.Controls.Add(this.label72);
            this.tab_MinimumGain.Controls.Add(this.label79);
            this.tab_MinimumGain.Controls.Add(this.label78);
            this.tab_MinimumGain.Controls.Add(this.btnMinGainLeft);
            this.tab_MinimumGain.Controls.Add(this.btnZeroMinMovementSetting);
            this.tab_MinimumGain.Controls.Add(this.label77);
            this.tab_MinimumGain.Controls.Add(this.label76);
            this.tab_MinimumGain.Controls.Add(this.btnMinGainRight);
            this.tab_MinimumGain.Controls.Add(this.label68);
            this.tab_MinimumGain.Controls.Add(this.label15);
            this.tab_MinimumGain.Controls.Add(this.btnPrev_Gain);
            this.tab_MinimumGain.Controls.Add(this.btnOK_Next);
            this.tab_MinimumGain.Controls.Add(this.hsbarMinPWM);
            this.tab_MinimumGain.Controls.Add(this.lblMinPWM);
            this.tab_MinimumGain.Location = new System.Drawing.Point(4, 19);
            this.tab_MinimumGain.Name = "tab_MinimumGain";
            this.tab_MinimumGain.Size = new System.Drawing.Size(655, 348);
            this.tab_MinimumGain.TabIndex = 34;
            this.tab_MinimumGain.Text = "MinGain";
            this.tab_MinimumGain.Enter += new System.EventHandler(this.tab_MinimumGain_Enter);
            this.tab_MinimumGain.Leave += new System.EventHandler(this.tab_MinimumGain_Leave);
            // 
            // label72
            // 
            this.label72.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label72.AutoSize = true;
            this.label72.BackColor = System.Drawing.Color.Transparent;
            this.label72.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label72.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label72.Location = new System.Drawing.Point(516, 106);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(36, 16);
            this.label72.TabIndex = 549;
            this.label72.Text = "Zero";
            this.label72.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label79
            // 
            this.label79.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label79.AutoSize = true;
            this.label79.BackColor = System.Drawing.Color.Transparent;
            this.label79.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label79.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label79.Location = new System.Drawing.Point(576, 212);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(41, 16);
            this.label79.TabIndex = 548;
            this.label79.Text = "Right";
            this.label79.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label78
            // 
            this.label78.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label78.AutoSize = true;
            this.label78.BackColor = System.Drawing.Color.Transparent;
            this.label78.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label78.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label78.Location = new System.Drawing.Point(457, 212);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(33, 16);
            this.label78.TabIndex = 547;
            this.label78.Text = "Left";
            this.label78.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnMinGainLeft
            // 
            this.btnMinGainLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMinGainLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnMinGainLeft.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnMinGainLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinGainLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMinGainLeft.Image = global::AgOpenGPS.Properties.Resources.SteerLeft;
            this.btnMinGainLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMinGainLeft.Location = new System.Drawing.Point(433, 142);
            this.btnMinGainLeft.Name = "btnMinGainLeft";
            this.btnMinGainLeft.Size = new System.Drawing.Size(76, 68);
            this.btnMinGainLeft.TabIndex = 546;
            this.btnMinGainLeft.UseVisualStyleBackColor = false;
            this.btnMinGainLeft.Click += new System.EventHandler(this.btnMinGainLeft_Click);
            // 
            // btnZeroMinMovementSetting
            // 
            this.btnZeroMinMovementSetting.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZeroMinMovementSetting.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnZeroMinMovementSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroMinMovementSetting.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroMinMovementSetting.ForeColor = System.Drawing.Color.White;
            this.btnZeroMinMovementSetting.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnZeroMinMovementSetting.Location = new System.Drawing.Point(496, 37);
            this.btnZeroMinMovementSetting.Name = "btnZeroMinMovementSetting";
            this.btnZeroMinMovementSetting.Size = new System.Drawing.Size(76, 68);
            this.btnZeroMinMovementSetting.TabIndex = 545;
            this.btnZeroMinMovementSetting.UseVisualStyleBackColor = true;
            this.btnZeroMinMovementSetting.Click += new System.EventHandler(this.btnZeroMinMovementSetting_Click);
            // 
            // label77
            // 
            this.label77.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label77.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label77.ForeColor = System.Drawing.Color.Black;
            this.label77.Location = new System.Drawing.Point(26, 97);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(369, 137);
            this.label77.TabIndex = 544;
            this.label77.Text = "Change Setpoint with steer buttons. \r\n\r\nAdjust Minumum Movement as high as possib" +
    "le without steer motor or hydraulics moving steering";
            this.label77.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label76
            // 
            this.label76.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label76.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label76.ForeColor = System.Drawing.Color.Black;
            this.label76.Location = new System.Drawing.Point(26, 36);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(371, 55);
            this.label76.TabIndex = 543;
            this.label76.Text = "Drive slowly straight forward, Allow system to steer";
            this.label76.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnMinGainRight
            // 
            this.btnMinGainRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMinGainRight.BackColor = System.Drawing.Color.Transparent;
            this.btnMinGainRight.FlatAppearance.BorderColor = System.Drawing.Color.LightGray;
            this.btnMinGainRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinGainRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMinGainRight.Image = global::AgOpenGPS.Properties.Resources.SteerRight;
            this.btnMinGainRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMinGainRight.Location = new System.Drawing.Point(557, 142);
            this.btnMinGainRight.Name = "btnMinGainRight";
            this.btnMinGainRight.Size = new System.Drawing.Size(76, 68);
            this.btnMinGainRight.TabIndex = 542;
            this.btnMinGainRight.UseVisualStyleBackColor = false;
            this.btnMinGainRight.Click += new System.EventHandler(this.btnMinGainRight_Click);
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.BackColor = System.Drawing.Color.Transparent;
            this.label68.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label68.ForeColor = System.Drawing.Color.Black;
            this.label68.Location = new System.Drawing.Point(5, 5);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(306, 25);
            this.label68.TabIndex = 538;
            this.label68.Text = "Minimum Movement Setting";
            this.label68.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_Gain
            // 
            this.btnPrev_Gain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_Gain.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_Gain.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_Gain.FlatAppearance.BorderSize = 0;
            this.btnPrev_Gain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_Gain.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_Gain.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_Gain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_Gain.Location = new System.Drawing.Point(485, 269);
            this.btnPrev_Gain.Name = "btnPrev_Gain";
            this.btnPrev_Gain.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_Gain.TabIndex = 537;
            this.btnPrev_Gain.UseVisualStyleBackColor = false;
            this.btnPrev_Gain.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnOK_Next
            // 
            this.btnOK_Next.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK_Next.BackColor = System.Drawing.Color.Transparent;
            this.btnOK_Next.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnOK_Next.FlatAppearance.BorderSize = 0;
            this.btnOK_Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK_Next.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnOK_Next.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnOK_Next.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnOK_Next.Location = new System.Drawing.Point(576, 269);
            this.btnOK_Next.Name = "btnOK_Next";
            this.btnOK_Next.Size = new System.Drawing.Size(72, 72);
            this.btnOK_Next.TabIndex = 536;
            this.btnOK_Next.UseVisualStyleBackColor = false;
            this.btnOK_Next.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabPGain
            // 
            this.tabPGain.BackColor = System.Drawing.Color.White;
            this.tabPGain.Controls.Add(this.label88);
            this.tabPGain.Controls.Add(this.label83);
            this.tabPGain.Controls.Add(this.label84);
            this.tabPGain.Controls.Add(this.label4);
            this.tabPGain.Controls.Add(this.btnLeftPGain);
            this.tabPGain.Controls.Add(this.btnZeroPGain);
            this.tabPGain.Controls.Add(this.label2);
            this.tabPGain.Controls.Add(this.label85);
            this.tabPGain.Controls.Add(this.label86);
            this.tabPGain.Controls.Add(this.btnRightPGain);
            this.tabPGain.Controls.Add(this.label87);
            this.tabPGain.Controls.Add(this.hsbarHighSteerPWM);
            this.tabPGain.Controls.Add(this.btnPrev_PGain);
            this.tabPGain.Controls.Add(this.btnNext_PGain);
            this.tabPGain.Controls.Add(this.hsbarProportionalGain);
            this.tabPGain.Controls.Add(this.lblProportionalGain);
            this.tabPGain.Controls.Add(this.lblHighSteerPWM);
            this.tabPGain.Location = new System.Drawing.Point(4, 19);
            this.tabPGain.Name = "tabPGain";
            this.tabPGain.Size = new System.Drawing.Size(655, 348);
            this.tabPGain.TabIndex = 36;
            this.tabPGain.Text = "PGain";
            this.tabPGain.Enter += new System.EventHandler(this.tabPGain_Enter);
            this.tabPGain.Leave += new System.EventHandler(this.tabPGain_Leave);
            // 
            // label88
            // 
            this.label88.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label88.AutoSize = true;
            this.label88.BackColor = System.Drawing.Color.Transparent;
            this.label88.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label88.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label88.Location = new System.Drawing.Point(586, 228);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(41, 16);
            this.label88.TabIndex = 560;
            this.label88.Text = "Right";
            this.label88.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label83
            // 
            this.label83.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label83.AutoSize = true;
            this.label83.BackColor = System.Drawing.Color.Transparent;
            this.label83.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label83.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label83.Location = new System.Drawing.Point(515, 60);
            this.label83.Name = "label83";
            this.label83.Size = new System.Drawing.Size(77, 16);
            this.label83.TabIndex = 559;
            this.label83.Text = "Zero or +5";
            this.label83.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label84
            // 
            this.label84.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label84.AutoSize = true;
            this.label84.BackColor = System.Drawing.Color.Transparent;
            this.label84.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label84.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label84.Location = new System.Drawing.Point(492, 228);
            this.label84.Name = "label84";
            this.label84.Size = new System.Drawing.Size(33, 16);
            this.label84.TabIndex = 558;
            this.label84.Text = "Left";
            this.label84.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnLeftPGain
            // 
            this.btnLeftPGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeftPGain.BackColor = System.Drawing.Color.Transparent;
            this.btnLeftPGain.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnLeftPGain.FlatAppearance.BorderSize = 0;
            this.btnLeftPGain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeftPGain.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnLeftPGain.Image = global::AgOpenGPS.Properties.Resources.SteerLeft;
            this.btnLeftPGain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLeftPGain.Location = new System.Drawing.Point(468, 158);
            this.btnLeftPGain.Name = "btnLeftPGain";
            this.btnLeftPGain.Size = new System.Drawing.Size(76, 68);
            this.btnLeftPGain.TabIndex = 557;
            this.btnLeftPGain.UseVisualStyleBackColor = false;
            this.btnLeftPGain.Click += new System.EventHandler(this.btnLeftPGain_Click);
            // 
            // btnZeroPGain
            // 
            this.btnZeroPGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnZeroPGain.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnZeroPGain.FlatAppearance.BorderSize = 0;
            this.btnZeroPGain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZeroPGain.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZeroPGain.ForeColor = System.Drawing.Color.White;
            this.btnZeroPGain.Image = global::AgOpenGPS.Properties.Resources.SteerZero;
            this.btnZeroPGain.Location = new System.Drawing.Point(520, 72);
            this.btnZeroPGain.Name = "btnZeroPGain";
            this.btnZeroPGain.Size = new System.Drawing.Size(76, 67);
            this.btnZeroPGain.TabIndex = 556;
            this.btnZeroPGain.UseVisualStyleBackColor = true;
            this.btnZeroPGain.Click += new System.EventHandler(this.btnZeroPGain_Click);
            // 
            // label85
            // 
            this.label85.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label85.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label85.ForeColor = System.Drawing.Color.Black;
            this.label85.Location = new System.Drawing.Point(27, 95);
            this.label85.Name = "label85";
            this.label85.Size = new System.Drawing.Size(435, 56);
            this.label85.TabIndex = 555;
            this.label85.Text = "Set Proportional for good response. \r\nSet PWM Maximum to limit turning speed.";
            this.label85.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label86
            // 
            this.label86.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label86.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label86.ForeColor = System.Drawing.Color.Black;
            this.label86.Location = new System.Drawing.Point(27, 50);
            this.label86.Name = "label86";
            this.label86.Size = new System.Drawing.Size(435, 33);
            this.label86.TabIndex = 554;
            this.label86.Text = "Drive forward and enable Auto Steer";
            this.label86.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnRightPGain
            // 
            this.btnRightPGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRightPGain.BackColor = System.Drawing.Color.Transparent;
            this.btnRightPGain.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnRightPGain.FlatAppearance.BorderSize = 0;
            this.btnRightPGain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRightPGain.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnRightPGain.Image = global::AgOpenGPS.Properties.Resources.SteerRight;
            this.btnRightPGain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRightPGain.Location = new System.Drawing.Point(568, 158);
            this.btnRightPGain.Name = "btnRightPGain";
            this.btnRightPGain.Size = new System.Drawing.Size(76, 68);
            this.btnRightPGain.TabIndex = 553;
            this.btnRightPGain.UseVisualStyleBackColor = false;
            this.btnRightPGain.Click += new System.EventHandler(this.btnRightPGain_Click);
            // 
            // label87
            // 
            this.label87.AutoSize = true;
            this.label87.BackColor = System.Drawing.Color.Transparent;
            this.label87.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label87.ForeColor = System.Drawing.Color.Black;
            this.label87.Location = new System.Drawing.Point(6, 6);
            this.label87.Name = "label87";
            this.label87.Size = new System.Drawing.Size(141, 25);
            this.label87.TabIndex = 552;
            this.label87.Text = "Gain Setting";
            this.label87.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_PGain
            // 
            this.btnPrev_PGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_PGain.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_PGain.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_PGain.FlatAppearance.BorderSize = 0;
            this.btnPrev_PGain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_PGain.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_PGain.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_PGain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_PGain.Location = new System.Drawing.Point(486, 270);
            this.btnPrev_PGain.Name = "btnPrev_PGain";
            this.btnPrev_PGain.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_PGain.TabIndex = 551;
            this.btnPrev_PGain.UseVisualStyleBackColor = false;
            this.btnPrev_PGain.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnNext_PGain
            // 
            this.btnNext_PGain.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNext_PGain.BackColor = System.Drawing.Color.Transparent;
            this.btnNext_PGain.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnNext_PGain.FlatAppearance.BorderSize = 0;
            this.btnNext_PGain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext_PGain.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnNext_PGain.Image = global::AgOpenGPS.Properties.Resources.Next;
            this.btnNext_PGain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNext_PGain.Location = new System.Drawing.Point(577, 270);
            this.btnNext_PGain.Name = "btnNext_PGain";
            this.btnNext_PGain.Size = new System.Drawing.Size(72, 72);
            this.btnNext_PGain.TabIndex = 550;
            this.btnNext_PGain.UseVisualStyleBackColor = false;
            this.btnNext_PGain.Click += new System.EventHandler(this.btnOkNext_Click);
            // 
            // tabEnd
            // 
            this.tabEnd.BackColor = System.Drawing.Color.White;
            this.tabEnd.Controls.Add(this.label116);
            this.tabEnd.Controls.Add(this.btnRestartWizard);
            this.tabEnd.Controls.Add(this.label69);
            this.tabEnd.Controls.Add(this.btnPrev_End);
            this.tabEnd.Controls.Add(this.btnExit);
            this.tabEnd.Location = new System.Drawing.Point(4, 19);
            this.tabEnd.Name = "tabEnd";
            this.tabEnd.Size = new System.Drawing.Size(655, 348);
            this.tabEnd.TabIndex = 32;
            this.tabEnd.Text = "End";
            // 
            // label116
            // 
            this.label116.BackColor = System.Drawing.Color.Transparent;
            this.label116.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label116.ForeColor = System.Drawing.Color.Black;
            this.label116.Location = new System.Drawing.Point(50, 114);
            this.label116.Name = "label116";
            this.label116.Size = new System.Drawing.Size(530, 152);
            this.label116.TabIndex = 538;
            this.label116.Text = "Now go drive an AB Line and fine tune settings based on your speed and soil condi" +
    "tions.\r\n\r\nRemember: CPD, Min PWM, and Look Ahead are critical settings. ";
            this.label116.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRestartWizard
            // 
            this.btnRestartWizard.BackColor = System.Drawing.Color.LightBlue;
            this.btnRestartWizard.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnRestartWizard.FlatAppearance.BorderSize = 0;
            this.btnRestartWizard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRestartWizard.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnRestartWizard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRestartWizard.Location = new System.Drawing.Point(222, 53);
            this.btnRestartWizard.Name = "btnRestartWizard";
            this.btnRestartWizard.Size = new System.Drawing.Size(178, 33);
            this.btnRestartWizard.TabIndex = 533;
            this.btnRestartWizard.Text = "Restart Wizard";
            this.btnRestartWizard.UseVisualStyleBackColor = false;
            this.btnRestartWizard.Click += new System.EventHandler(this.btnRestartWizard_Click);
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.BackColor = System.Drawing.Color.Transparent;
            this.label69.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label69.ForeColor = System.Drawing.Color.Black;
            this.label69.Location = new System.Drawing.Point(258, 20);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(111, 25);
            this.label69.TabIndex = 537;
            this.label69.Text = "Finished !";
            this.label69.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnPrev_End
            // 
            this.btnPrev_End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnPrev_End.BackColor = System.Drawing.Color.Transparent;
            this.btnPrev_End.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnPrev_End.FlatAppearance.BorderSize = 0;
            this.btnPrev_End.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrev_End.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnPrev_End.Image = global::AgOpenGPS.Properties.Resources.Previous;
            this.btnPrev_End.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPrev_End.Location = new System.Drawing.Point(451, 271);
            this.btnPrev_End.Name = "btnPrev_End";
            this.btnPrev_End.Size = new System.Drawing.Size(72, 72);
            this.btnPrev_End.TabIndex = 536;
            this.btnPrev_End.UseVisualStyleBackColor = false;
            this.btnPrev_End.Click += new System.EventHandler(this.btnPrev_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExit.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnExit.Location = new System.Drawing.Point(546, 271);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(104, 72);
            this.btnExit.TabIndex = 530;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnStopWizard
            // 
            this.btnStopWizard.BackColor = System.Drawing.Color.LightCoral;
            this.btnStopWizard.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStopWizard.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnStopWizard.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStopWizard.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnStopWizard.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnStopWizard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStopWizard.Location = new System.Drawing.Point(18, 7);
            this.btnStopWizard.Name = "btnStopWizard";
            this.btnStopWizard.Size = new System.Drawing.Size(148, 38);
            this.btnStopWizard.TabIndex = 527;
            this.btnStopWizard.Text = "Restart Wizard";
            this.btnStopWizard.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnStopWizard.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnStopWizard.UseVisualStyleBackColor = false;
            this.btnStopWizard.Click += new System.EventHandler(this.btnStopWizard_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.tabWiz);
            this.panel3.Location = new System.Drawing.Point(5, 51);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(665, 373);
            this.panel3.TabIndex = 531;
            // 
            // pbarProgress
            // 
            this.pbarProgress.Location = new System.Drawing.Point(336, 18);
            this.pbarProgress.Name = "pbarProgress";
            this.pbarProgress.Size = new System.Drawing.Size(323, 15);
            this.pbarProgress.Step = 1;
            this.pbarProgress.TabIndex = 532;
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.BackColor = System.Drawing.Color.Transparent;
            this.label71.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label71.ForeColor = System.Drawing.Color.Black;
            this.label71.Location = new System.Drawing.Point(235, 12);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(100, 23);
            this.label71.TabIndex = 532;
            this.label71.Text = "Progress:";
            this.label71.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBarCPD
            // 
            this.lblBarCPD.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBarCPD.AutoSize = true;
            this.lblBarCPD.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarCPD.ForeColor = System.Drawing.Color.Black;
            this.lblBarCPD.Location = new System.Drawing.Point(675, 146);
            this.lblBarCPD.Name = "lblBarCPD";
            this.lblBarCPD.Size = new System.Drawing.Size(51, 25);
            this.lblBarCPD.TabIndex = 534;
            this.lblBarCPD.Text = "888";
            this.lblBarCPD.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label66
            // 
            this.label66.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label66.AutoSize = true;
            this.label66.BackColor = System.Drawing.Color.Transparent;
            this.label66.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label66.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label66.Location = new System.Drawing.Point(675, 124);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(123, 23);
            this.label66.TabIndex = 535;
            this.label66.Text = "Counts P Deg";
            this.label66.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label65
            // 
            this.label65.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label65.AutoSize = true;
            this.label65.BackColor = System.Drawing.Color.Transparent;
            this.label65.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label65.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label65.Location = new System.Drawing.Point(675, 176);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(104, 23);
            this.label65.TabIndex = 537;
            this.label65.Text = "Ackermann";
            this.label65.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBarAck
            // 
            this.lblBarAck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBarAck.AutoSize = true;
            this.lblBarAck.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarAck.ForeColor = System.Drawing.Color.Black;
            this.lblBarAck.Location = new System.Drawing.Point(675, 198);
            this.lblBarAck.Name = "lblBarAck";
            this.lblBarAck.Size = new System.Drawing.Size(51, 25);
            this.lblBarAck.TabIndex = 536;
            this.lblBarAck.Text = "888";
            this.lblBarAck.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // sideBarTimer
            // 
            this.sideBarTimer.Enabled = true;
            this.sideBarTimer.Interval = 1000;
            this.sideBarTimer.Tick += new System.EventHandler(this.sideBarTimer_Tick);
            // 
            // label67
            // 
            this.label67.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label67.AutoSize = true;
            this.label67.BackColor = System.Drawing.Color.Transparent;
            this.label67.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label67.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label67.Location = new System.Drawing.Point(675, 72);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(104, 23);
            this.label67.TabIndex = 539;
            this.label67.Text = "WAS Offset";
            this.label67.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblBarWasOffset
            // 
            this.lblBarWasOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblBarWasOffset.AutoSize = true;
            this.lblBarWasOffset.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBarWasOffset.ForeColor = System.Drawing.Color.Black;
            this.lblBarWasOffset.Location = new System.Drawing.Point(675, 93);
            this.lblBarWasOffset.Name = "lblBarWasOffset";
            this.lblBarWasOffset.Size = new System.Drawing.Size(51, 25);
            this.lblBarWasOffset.TabIndex = 538;
            this.lblBarWasOffset.Text = "888";
            this.lblBarWasOffset.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCloseAll
            // 
            this.btnCloseAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCloseAll.BackColor = System.Drawing.Color.Transparent;
            this.btnCloseAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseAll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnCloseAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCloseAll.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCloseAll.Image = global::AgOpenGPS.Properties.Resources.boundaryStop;
            this.btnCloseAll.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCloseAll.Location = new System.Drawing.Point(688, 479);
            this.btnCloseAll.Name = "btnCloseAll";
            this.btnCloseAll.Size = new System.Drawing.Size(104, 72);
            this.btnCloseAll.TabIndex = 540;
            this.btnCloseAll.UseVisualStyleBackColor = false;
            this.btnCloseAll.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // FormSteerWiz
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(805, 558);
            this.ControlBox = false;
            this.Controls.Add(this.btnCloseAll);
            this.Controls.Add(this.label67);
            this.Controls.Add(this.lblBarWasOffset);
            this.Controls.Add(this.label65);
            this.Controls.Add(this.lblBarAck);
            this.Controls.Add(this.label66);
            this.Controls.Add(this.lblBarCPD);
            this.Controls.Add(this.label71);
            this.Controls.Add(this.pbarProgress);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btnStopWizard);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.HelpButton = true;
            this.MinimizeBox = false;
            this.Name = "FormSteerWiz";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Steer Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSteer_FormClosing);
            this.Load += new System.EventHandler(this.FormSteer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabSteer.ResumeLayout(false);
            this.tabSteer.PerformLayout();
            this.tabStan.ResumeLayout(false);
            this.tabInt.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudMaxCounts)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabWiz.ResumeLayout(false);
            this.tabStart.ResumeLayout(false);
            this.tabLoadDef.ResumeLayout(false);
            this.tabLoadDef.PerformLayout();
            this.tabWheelBase.ResumeLayout(false);
            this.tabWheelBase.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudWheelbase)).EndInit();
            this.tabWheelTrack.ResumeLayout(false);
            this.tabWheelTrack.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudVehicleTrack)).EndInit();
            this.tabAntennaDistance.ResumeLayout(false);
            this.tabAntennaDistance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaPivot)).EndInit();
            this.tabAntennaHeight.ResumeLayout(false);
            this.tabAntennaHeight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaHeight)).EndInit();
            this.tabAntennaOffset.ResumeLayout(false);
            this.tabAntennaOffset.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntennaOffset)).EndInit();
            this.tabButtonSwitch.ResumeLayout(false);
            this.tabButtonSwitch.PerformLayout();
            this.tabA2DConv.ResumeLayout(false);
            this.tabA2DConv.PerformLayout();
            this.tabMotorDriver.ResumeLayout(false);
            this.tabMotorDriver.PerformLayout();
            this.tabInvertRelays.ResumeLayout(false);
            this.tabInvertRelays.PerformLayout();
            this.tabDanfoss.ResumeLayout(false);
            this.tabDanfoss.PerformLayout();
            this.tabRollInv.ResumeLayout(false);
            this.tabRollInv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox9)).EndInit();
            this.tabRollZero.ResumeLayout(false);
            this.tabRollZero.PerformLayout();
            this.tabWAS.ResumeLayout(false);
            this.tabWAS.PerformLayout();
            this.tabWAS_Zero.ResumeLayout(false);
            this.tabWAS_Zero.PerformLayout();
            this.tabMotorDirection.ResumeLayout(false);
            this.tabMotorDirection.PerformLayout();
            this.tabCPD_Setup.ResumeLayout(false);
            this.tabCPD_Setup.PerformLayout();
            this.tabCountsPerDeg.ResumeLayout(false);
            this.tabCountsPerDeg.PerformLayout();
            this.tabAckCPD.ResumeLayout(false);
            this.tabAckCPD.PerformLayout();
            this.tabMaxSteerAngle.ResumeLayout(false);
            this.tabMaxSteerAngle.PerformLayout();
            this.tabCancelGuidance.ResumeLayout(false);
            this.tabCancelGuidance.PerformLayout();
            this.tabPanicStop.ResumeLayout(false);
            this.tabPanicStop.PerformLayout();
            this.tab_MinimumGain.ResumeLayout(false);
            this.tab_MinimumGain.PerformLayout();
            this.tabPGain.ResumeLayout(false);
            this.tabPGain.PerformLayout();
            this.tabEnd.ResumeLayout(false);
            this.tabEnd.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnFreeDrive;
        private System.Windows.Forms.Label lblHighSteerPWM;
        private System.Windows.Forms.HScrollBar hsbarHighSteerPWM;
        private System.Windows.Forms.Label lblLowSteerPWM;
        private System.Windows.Forms.HScrollBar hsbarLowSteerPWM;
        private System.Windows.Forms.Label lblProportionalGain;
        private System.Windows.Forms.HScrollBar hsbarProportionalGain;
        private System.Windows.Forms.Label lblLookAhead;
        private System.Windows.Forms.Label lblMinPWM;
        private System.Windows.Forms.HScrollBar hsbarMinPWM;
        private System.Windows.Forms.Label lblMaxSteerAngle;
        private System.Windows.Forms.HScrollBar hsbarMaxSteerAngle;
        private System.Windows.Forms.Label lblLookAheadMult;
        private System.Windows.Forms.HScrollBar hsbarHeadingErrorGain;
        private System.Windows.Forms.Label lblStanleyGain;
        private System.Windows.Forms.HScrollBar hsbarStanleyGain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
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
        private System.Windows.Forms.TabPage tabInt;
        private System.Windows.Forms.Button btnStartSA;
        private System.Windows.Forms.Label lblCalcSteerAngleInner;
        private System.Windows.Forms.Label lblDiameter;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TabPage tabSteer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar pbarRight;
        private System.Windows.Forms.ProgressBar pbarLeft;
        private System.Windows.Forms.Button btnZeroWAS;
        private System.Windows.Forms.HScrollBar hsbarCountsPerDegree;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblCountsPerDegree;
        private System.Windows.Forms.HScrollBar hsbarWasOffset;
        private System.Windows.Forms.Label lblSteerAngleSensorZero;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.HScrollBar hsbarIntegral;
        private System.Windows.Forms.HScrollBar hsbarIntegralPurePursuit;
        private System.Windows.Forms.Label lblIntegralPercent;
        private System.Windows.Forms.Label lblPureIntegral;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.ProgressBar pbarSensor;
        private System.Windows.Forms.Label lblPercentFS;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label labelPressureTurnSensor;
        private System.Windows.Forms.Label labelCurrentTurnSensor;
        private System.Windows.Forms.Label labelEncoder;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.ComboBox cboxMotorDrive;
        private System.Windows.Forms.ComboBox cboxSteerEnable;
        private System.Windows.Forms.Label label62;
        private System.Windows.Forms.ComboBox cboxConv;
        private System.Windows.Forms.Label label63;
        private System.Windows.Forms.Label label64;
        private System.Windows.Forms.Label label61;
        private NudlessNumericUpDown nudMaxCounts;
        private System.Windows.Forms.Label label55;
        private System.Windows.Forms.CheckBox cboxCurrentSensor;
        private System.Windows.Forms.CheckBox cboxPressureSensor;
        private System.Windows.Forms.CheckBox cboxDanfoss;
        private System.Windows.Forms.CheckBox chkSteerInvertRelays;
        private System.Windows.Forms.CheckBox chkInvertSteer;
        private System.Windows.Forms.CheckBox cboxEncoder;
        private System.Windows.Forms.CheckBox chkInvertWAS;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.HScrollBar hsbarSensor;
        private System.Windows.Forms.Label lblhsbarSensor;
        private System.Windows.Forms.TabControl tabWiz;
        private System.Windows.Forms.TabPage tabStart;
        private System.Windows.Forms.TabPage tabMotorDriver;
        private System.Windows.Forms.TabPage tabMotorDirection;
        private System.Windows.Forms.Button btnSteerLeft;
        private System.Windows.Forms.Button btnSteerRight;
        private System.Windows.Forms.TabPage tabWAS;
        private System.Windows.Forms.TabPage tabButtonSwitch;
        private System.Windows.Forms.Button btnStartWizard;
        private System.Windows.Forms.Button btnStopWizard;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.TabPage tabCancelGuidance;
        private System.Windows.Forms.TabPage tabWAS_Zero;
        private System.Windows.Forms.TabPage tabCountsPerDeg;
        private System.Windows.Forms.Button btnOkWAS;
        private System.Windows.Forms.Button btnOkNext_MotorDriver;
        private System.Windows.Forms.Button btnOkNext_MotorDirection;
        private System.Windows.Forms.Button btnOkNext_WAS_Zero;
        private System.Windows.Forms.Button btnOkNext_CountsPerDeg;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnOkNext_ButtonSwitch;
        private System.Windows.Forms.TabPage tabMaxSteerAngle;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnOkNextMaxSteerAngle;
        private System.Windows.Forms.TabPage tabA2DConv;
        private System.Windows.Forms.Button btnOkNext_A2D;
        private System.Windows.Forms.TabPage tabInvertRelays;
        private System.Windows.Forms.TabPage tabDanfoss;
        private System.Windows.Forms.Button btnOkNext_InvertRelays;
        private System.Windows.Forms.Button btnOkNext_Danfoss;
        private System.Windows.Forms.Label label43;
        private System.Windows.Forms.Label label42;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label40;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label44;
        private System.Windows.Forms.Button btnOkNextCancelGuidance;
        private System.Windows.Forms.Label label53;
        private System.Windows.Forms.Label label52;
        private System.Windows.Forms.Label label51;
        private System.Windows.Forms.Label label50;
        private System.Windows.Forms.Label label49;
        private System.Windows.Forms.Button btnSteerStatus;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.TabPage tabPanicStop;
        private System.Windows.Forms.Label label54;
        private System.Windows.Forms.Button btnOkNext_PanicStop;
        private System.Windows.Forms.Button btnPrev_A2D;
        private System.Windows.Forms.Button btnPrev_MotorDriver;
        private System.Windows.Forms.Button btnPrev_InvertRelays;
        private System.Windows.Forms.Button btnPrev_Danfoss;
        private System.Windows.Forms.Button btnPrev_InvertWAS;
        private System.Windows.Forms.Button btnPrev_MotorDirection;
        private System.Windows.Forms.Button btnPrev_CountsPerDegree;
        private System.Windows.Forms.Button btnPrev_MaxSteerAngle;
        private System.Windows.Forms.Button btnPrev_CancelGuidance;
        private System.Windows.Forms.Button btnPrev_Panic;
        private System.Windows.Forms.TabPage tabEnd;
        private System.Windows.Forms.Button btnPrev_End;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.ProgressBar pbarProgress;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.TabPage tabCPD_Setup;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.Button btnSkipCPD_Setup;
        private System.Windows.Forms.Button btnOKNext_CPDSetup;
        private System.Windows.Forms.Button btnRestartWizard;
        private System.Windows.Forms.Button btnRemoveWasOffset;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.Label label58;
        private System.Windows.Forms.Label label59;
        private System.Windows.Forms.Label label60;
        private System.Windows.Forms.Button btnLoadDefaults;
        private System.Windows.Forms.Label lblBarCPD;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.Label lblBarAck;
        private System.Windows.Forms.Timer sideBarTimer;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label lblBarWasOffset;
        private System.Windows.Forms.Label lblCPDError;
        private System.Windows.Forms.Button btnCloseAll;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.TabPage tab_MinimumGain;
        private System.Windows.Forms.Button btnPrev_Gain;
        private System.Windows.Forms.Button btnOK_Next;
        private System.Windows.Forms.Button btnMinGainRight;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Button btnOkSetMaximumSteerAngle;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.Button btnZeroMinMovementSetting;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.Button btnMinGainLeft;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.TabPage tabLoadDef;
        private System.Windows.Forms.Label label81;
        private System.Windows.Forms.Label label80;
        private System.Windows.Forms.Button btnOkNext_LoadDefault;
        private System.Windows.Forms.TabPage tabPGain;
        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Label label83;
        private System.Windows.Forms.Label label84;
        private System.Windows.Forms.Button btnLeftPGain;
        private System.Windows.Forms.Button btnZeroPGain;
        private System.Windows.Forms.Label label85;
        private System.Windows.Forms.Label label86;
        private System.Windows.Forms.Button btnRightPGain;
        private System.Windows.Forms.Label label87;
        private System.Windows.Forms.Button btnPrev_PGain;
        private System.Windows.Forms.Button btnNext_PGain;
        private System.Windows.Forms.TabPage tabWheelBase;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label90;
        private NudlessNumericUpDown nudWheelbase;
        private System.Windows.Forms.Label label91;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabPage tabWheelTrack;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Label label94;
        private NudlessNumericUpDown nudVehicleTrack;
        private System.Windows.Forms.Label label95;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.TabPage tabAntennaDistance;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Label label98;
        private NudlessNumericUpDown nudAntennaPivot;
        private System.Windows.Forms.Label label99;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TabPage tabAntennaOffset;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label102;
        private NudlessNumericUpDown nudAntennaOffset;
        private System.Windows.Forms.Label label103;
        private System.Windows.Forms.Label label104;
        private System.Windows.Forms.Button button12;
        private System.Windows.Forms.TabPage tabAntennaHeight;
        private System.Windows.Forms.Button button13;
        private System.Windows.Forms.Label label92;
        private NudlessNumericUpDown nudAntennaHeight;
        private System.Windows.Forms.Label label93;
        private System.Windows.Forms.Button button14;
        private System.Windows.Forms.TabPage tabRollInv;
        private System.Windows.Forms.Label label89;
        private System.Windows.Forms.Label label96;
        private System.Windows.Forms.Label label97;
        private System.Windows.Forms.Label label100;
        private System.Windows.Forms.Button button15;
        private System.Windows.Forms.TabPage tabAckCPD;
        private System.Windows.Forms.Button button16;
        private System.Windows.Forms.Label lblAckermannError;
        private System.Windows.Forms.Label label105;
        private System.Windows.Forms.Label label106;
        private System.Windows.Forms.Label label108;
        private System.Windows.Forms.Label lblDiameterLeft;
        private System.Windows.Forms.Label lblCalcSteerAngleLeft;
        private System.Windows.Forms.Label label111;
        private System.Windows.Forms.Button button17;
        private System.Windows.Forms.Button btnStartSA_Left;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.HScrollBar hsbarAckerman;
        private System.Windows.Forms.Label lblAckerman;
        private System.Windows.Forms.Label label101;
        private System.Windows.Forms.Label lblStartAngleLeft;
        private System.Windows.Forms.Button btnAckReset;
        private System.Windows.Forms.Label lblCurrentHeading;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label46;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Label lblRoll;
        private System.Windows.Forms.PictureBox pictureBox9;
        private System.Windows.Forms.CheckBox cboxDataInvertRoll;
        private System.Windows.Forms.TabPage tabRollZero;
        private System.Windows.Forms.Label label48;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label56;
        private System.Windows.Forms.Button button19;
        private System.Windows.Forms.Label label107;
        private System.Windows.Forms.Label lblRoll2;
        private System.Windows.Forms.Label label110;
        private System.Windows.Forms.Label label47;
        private System.Windows.Forms.Label label57;
        private System.Windows.Forms.Label lblRollZeroOffset;
        private System.Windows.Forms.Button btnZeroRoll;
        private System.Windows.Forms.Button btnRemoveZeroOffset;
        private System.Windows.Forms.Label label109;
        private System.Windows.Forms.Label lblRightStartAngle;
        private System.Windows.Forms.Label label112;
        private System.Windows.Forms.Label label113;
        private System.Windows.Forms.Label label114;
        private System.Windows.Forms.Label label115;
        private System.Windows.Forms.Label label116;
        private System.Windows.Forms.Label label117;
        private System.Windows.Forms.Label label118;
        private System.Windows.Forms.Label lblSideHillComp;
        private System.Windows.Forms.HScrollBar hsbarSideHillComp;
        private System.Windows.Forms.HScrollBar hsbarLookAheadMult;
    }
}
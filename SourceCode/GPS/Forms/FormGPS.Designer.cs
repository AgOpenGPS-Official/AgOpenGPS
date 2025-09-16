namespace AgOpenGPS
{
    partial class FormGPS
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormGPS));
            this.contextMenuStripOpenGL = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.googleEarthOpenGLContextMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.profileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadProfileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menustripLanguage = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.simulatorOnToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.enterSimCoordsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.kioskModeToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.resetALLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resetEverythingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AgShareApiMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrWatchdog = new System.Windows.Forms.Timer(this.components);
            this.contextMenuStripFlag = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemFlagRed = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagGrn = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuFlagYel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuFlagForm = new System.Windows.Forms.ToolStripMenuItem();
            this.cboxpRowWidth = new System.Windows.Forms.ComboBox();
            this.oglZoom = new OpenTK.GLControl.GLControl();
            this.panelDrag = new System.Windows.Forms.TableLayoutPanel();
            this.btnPathGoStop = new System.Windows.Forms.Button();
            this.btnPickPath = new System.Windows.Forms.Button();
            this.btnPathRecordStop = new System.Windows.Forms.Button();
            this.btnResumePath = new System.Windows.Forms.Button();
            this.btnSwapABRecordedPath = new System.Windows.Forms.Button();
            this.btnResetSim = new System.Windows.Forms.Button();
            this.btnResetSteerAngle = new System.Windows.Forms.Button();
            this.timerSim = new System.Windows.Forms.Timer(this.components);
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.hsbarSteerAngle = new System.Windows.Forms.HScrollBar();
            this.btnSection8Man = new System.Windows.Forms.Button();
            this.btnSection7Man = new System.Windows.Forms.Button();
            this.btnSection6Man = new System.Windows.Forms.Button();
            this.btnSection5Man = new System.Windows.Forms.Button();
            this.btnSection4Man = new System.Windows.Forms.Button();
            this.btnSection3Man = new System.Windows.Forms.Button();
            this.btnSection2Man = new System.Windows.Forms.Button();
            this.btnSection1Man = new System.Windows.Forms.Button();
            this.btnSection9Man = new System.Windows.Forms.Button();
            this.btnSection10Man = new System.Windows.Forms.Button();
            this.btnSection11Man = new System.Windows.Forms.Button();
            this.btnSection12Man = new System.Windows.Forms.Button();
            this.oglMain = new OpenTK.GLControl.GLControl();
            this.oglBack = new OpenTK.GLControl.GLControl();
            this.lblHz = new System.Windows.Forms.Label();
            this.statusStripLeft = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton4 = new System.Windows.Forms.ToolStripDropDownButton();
            this.wizardsMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.steerWizardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.steerChartStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.steerChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headingChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.xTEChartToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rollCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.boundaryToolToolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.eventViewerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.guidelinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SmoothABtoolStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteContourPathsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.webcamToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.offsetFixToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSim = new System.Windows.Forms.TableLayoutPanel();
            this.btnSpeedDn = new AgLibrary.Controls.RepeatButton();
            this.btnSimSpeedUp = new AgLibrary.Controls.RepeatButton();
            this.btnSimSetSpeedToZero = new System.Windows.Forms.Button();
            this.btnSimReverseDirection = new System.Windows.Forms.Button();
            this.btnSection16Man = new System.Windows.Forms.Button();
            this.btnSection15Man = new System.Windows.Forms.Button();
            this.btnSection14Man = new System.Windows.Forms.Button();
            this.btnSection13Man = new System.Windows.Forms.Button();
            this.panelNavigation = new System.Windows.Forms.TableLayoutPanel();
            this.btnTiltDn = new System.Windows.Forms.Button();
            this.btnTiltUp = new System.Windows.Forms.Button();
            this.btnBrightnessDn = new System.Windows.Forms.Button();
            this.btnBrightnessUp = new System.Windows.Forms.Button();
            this.btnDayNightMode = new System.Windows.Forms.Button();
            this.btn3D = new System.Windows.Forms.Button();
            this.btn2D = new System.Windows.Forms.Button();
            this.btnGrid = new System.Windows.Forms.Button();
            this.btnN2D = new System.Windows.Forms.Button();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblFix = new System.Windows.Forms.Label();
            this.btnZone1 = new System.Windows.Forms.Button();
            this.btnZone2 = new System.Windows.Forms.Button();
            this.btnZone3 = new System.Windows.Forms.Button();
            this.btnZone4 = new System.Windows.Forms.Button();
            this.btnZone5 = new System.Windows.Forms.Button();
            this.btnZone6 = new System.Windows.Forms.Button();
            this.btnZone7 = new System.Windows.Forms.Button();
            this.btnZone8 = new System.Windows.Forms.Button();
            this.panelLeft = new System.Windows.Forms.TableLayoutPanel();
            this.statusStrip2 = new System.Windows.Forms.StatusStrip();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripConfig = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSteerSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripAllSettings = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripWorkingDirectories = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripGPSData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripColors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSectionColors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripHotkeys = new System.Windows.Forms.ToolStripMenuItem();
            this.btnNavigationSettings = new System.Windows.Forms.Button();
            this.btnAutoSteerConfig = new System.Windows.Forms.Button();
            this.btnStartAgIO = new System.Windows.Forms.Button();
            this.btnJobMenu = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripBtnFieldTools = new System.Windows.Forms.ToolStripDropDownButton();
            this.boundariesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlandToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.headlandBuildToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tramLinesMenuField = new System.Windows.Forms.ToolStripMenuItem();
            this.tramsMultiMenuField = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteAppliedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.flagByLatLonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recordedPathStripMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.lblCurrentField = new System.Windows.Forms.Label();
            this.lblGuidanceLine = new System.Windows.Forms.Label();
            this.flp1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNudge = new System.Windows.Forms.Button();
            this.btnABDraw = new System.Windows.Forms.Button();
            this.btnPlusAB = new System.Windows.Forms.Button();
            this.btnBuildTracks = new System.Windows.Forms.Button();
            this.btnTracksOff = new System.Windows.Forms.Button();
            this.cboxAutoSnapToPivot = new System.Windows.Forms.CheckBox();
            this.btnRefNudge = new System.Windows.Forms.Button();
            this.panelBottom = new System.Windows.Forms.FlowLayoutPanel();
            this.btnTrack = new System.Windows.Forms.Button();
            this.btnSnapToPivot = new System.Windows.Forms.Button();
            this.btnAdjRight = new System.Windows.Forms.Button();
            this.btnAdjLeft = new System.Windows.Forms.Button();
            this.btnFlag = new System.Windows.Forms.Button();
            this.btnHeadlandOnOff = new System.Windows.Forms.Button();
            this.cboxIsSectionControlled = new System.Windows.Forms.CheckBox();
            this.btnHydLift = new System.Windows.Forms.Button();
            this.btnTramDisplayMode = new System.Windows.Forms.Button();
            this.btnResetToolHeading = new System.Windows.Forms.Button();
            this.btnChangeMappingColor = new System.Windows.Forms.Button();
            this.btnYouSkipEnable = new System.Windows.Forms.Button();
            this.panelRight = new System.Windows.Forms.FlowLayoutPanel();
            this.btnAutoSteer = new System.Windows.Forms.Button();
            this.btnAutoYouTurn = new System.Windows.Forms.Button();
            this.btnSectionMasterAuto = new System.Windows.Forms.Button();
            this.btnSectionMasterManual = new System.Windows.Forms.Button();
            this.btnAutoTrack = new System.Windows.Forms.Button();
            this.btnCycleLinesBk = new System.Windows.Forms.Button();
            this.btnCycleLines = new System.Windows.Forms.Button();
            this.btnContour = new System.Windows.Forms.Button();
            this.btnContourLock = new System.Windows.Forms.Button();
            this.lblNumCu = new System.Windows.Forms.Label();
            this.panelControlBox = new System.Windows.Forms.Panel();
            this.btnChargeStatus = new System.Windows.Forms.Button();
            this.btnGPSData = new System.Windows.Forms.Button();
            this.btnShutdown = new System.Windows.Forms.Button();
            this.btnMaximizeMainForm = new System.Windows.Forms.Button();
            this.btnMinimizeMainForm = new System.Windows.Forms.Button();
            this.btnFieldStats = new System.Windows.Forms.Button();
            this.lblHardwareMessage = new System.Windows.Forms.Label();
            this.contextMenuStripOpenGL.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuStripFlag.SuspendLayout();
            this.panelDrag.SuspendLayout();
            this.statusStripLeft.SuspendLayout();
            this.panelSim.SuspendLayout();
            this.panelNavigation.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.statusStrip2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.flp1.SuspendLayout();
            this.panelBottom.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.panelControlBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStripOpenGL
            // 
            this.contextMenuStripOpenGL.AutoSize = false;
            this.contextMenuStripOpenGL.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator5,
            this.googleEarthOpenGLContextMenu});
            this.contextMenuStripOpenGL.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuStripOpenGL.Name = "contextMenuStripOpenGL";
            this.contextMenuStripOpenGL.Size = new System.Drawing.Size(72, 160);
            this.contextMenuStripOpenGL.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStripOpenGL_Opening);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(68, 6);
            // 
            // googleEarthOpenGLContextMenu
            // 
            this.googleEarthOpenGLContextMenu.AutoSize = false;
            this.googleEarthOpenGLContextMenu.Image = global::AgOpenGPS.Properties.Resources.GoogleEarth;
            this.googleEarthOpenGLContextMenu.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.googleEarthOpenGLContextMenu.Name = "googleEarthOpenGLContextMenu";
            this.googleEarthOpenGLContextMenu.Size = new System.Drawing.Size(70, 70);
            this.googleEarthOpenGLContextMenu.Text = ".";
            this.googleEarthOpenGLContextMenu.Click += new System.EventHandler(this.googleEarthOpenGLContextMenu_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Font = new System.Drawing.Font("Tahoma", 22F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(20, 3);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.menuStrip1.Size = new System.Drawing.Size(56, 44);
            this.menuStrip1.TabIndex = 49;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator9,
            this.profileToolStripMenuItem,
            this.menustripLanguage,
            this.toolStripSeparator11,
            this.simulatorOnToolStripMenuItem,
            this.enterSimCoordsToolStripMenuItem,
            this.toolStripSeparator4,
            this.kioskModeToolStrip,
            this.resetALLToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.AgShareApiMenuItem,
            this.helpMenuItem});
            this.fileToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.fileMenu;
            this.fileToolStripMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.fileToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(55, 44);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(379, 6);
            // 
            // profileToolStripMenuItem
            // 
            this.profileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProfileToolStripMenuItem,
            this.loadProfileToolStripMenuItem});
            this.profileToolStripMenuItem.Name = "profileToolStripMenuItem";
            this.profileToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.profileToolStripMenuItem.Text = "Profile";
            // 
            // newProfileToolStripMenuItem
            // 
            this.newProfileToolStripMenuItem.Name = "newProfileToolStripMenuItem";
            this.newProfileToolStripMenuItem.Size = new System.Drawing.Size(210, 50);
            this.newProfileToolStripMenuItem.Text = "New...";
            this.newProfileToolStripMenuItem.Click += new System.EventHandler(this.newProfileToolStripMenuItem_Click);
            // 
            // loadProfileToolStripMenuItem
            // 
            this.loadProfileToolStripMenuItem.Name = "loadProfileToolStripMenuItem";
            this.loadProfileToolStripMenuItem.Size = new System.Drawing.Size(210, 50);
            this.loadProfileToolStripMenuItem.Text = "Load...";
            this.loadProfileToolStripMenuItem.Click += new System.EventHandler(this.loadProfileToolStripMenuItem_Click);
            // 
            // menustripLanguage
            // 
            this.menustripLanguage.Name = "menustripLanguage";
            this.menustripLanguage.Size = new System.Drawing.Size(382, 50);
            this.menustripLanguage.Text = "Language";
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(379, 6);
            // 
            // simulatorOnToolStripMenuItem
            // 
            this.simulatorOnToolStripMenuItem.CheckOnClick = true;
            this.simulatorOnToolStripMenuItem.Name = "simulatorOnToolStripMenuItem";
            this.simulatorOnToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.simulatorOnToolStripMenuItem.Text = "Simulator On";
            this.simulatorOnToolStripMenuItem.Click += new System.EventHandler(this.simulatorOnToolStripMenuItem_Click);
            // 
            // enterSimCoordsToolStripMenuItem
            // 
            this.enterSimCoordsToolStripMenuItem.Name = "enterSimCoordsToolStripMenuItem";
            this.enterSimCoordsToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.enterSimCoordsToolStripMenuItem.Text = "Enter Sim Coords";
            this.enterSimCoordsToolStripMenuItem.Click += new System.EventHandler(this.enterSimCoordsToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(379, 6);
            // 
            // kioskModeToolStrip
            // 
            this.kioskModeToolStrip.Name = "kioskModeToolStrip";
            this.kioskModeToolStrip.Size = new System.Drawing.Size(382, 50);
            this.kioskModeToolStrip.Text = "Kiosk Mode";
            this.kioskModeToolStrip.Click += new System.EventHandler(this.kioskModeToolStrip_Click);
            // 
            // resetALLToolStripMenuItem
            // 
            this.resetALLToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.resetEverythingToolStripMenuItem});
            this.resetALLToolStripMenuItem.Name = "resetALLToolStripMenuItem";
            this.resetALLToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.resetALLToolStripMenuItem.Text = "Reset All";
            // 
            // resetEverythingToolStripMenuItem
            // 
            this.resetEverythingToolStripMenuItem.Name = "resetEverythingToolStripMenuItem";
            this.resetEverythingToolStripMenuItem.Size = new System.Drawing.Size(372, 50);
            this.resetEverythingToolStripMenuItem.Text = "Reset To Default";
            this.resetEverythingToolStripMenuItem.Click += new System.EventHandler(this.resetALLToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(382, 50);
            this.aboutToolStripMenuItem.Text = "About...";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // AgShareApiMenuItem
            // 
            this.AgShareApiMenuItem.Name = "AgShareApiMenuItem";
            this.AgShareApiMenuItem.Size = new System.Drawing.Size(382, 50);
            this.AgShareApiMenuItem.Text = "AgShare API";
            this.AgShareApiMenuItem.Click += new System.EventHandler(this.AgShareApiMenuItem_Click);
            // 
            // helpMenuItem
            // 
            this.helpMenuItem.Name = "helpMenuItem";
            this.helpMenuItem.Size = new System.Drawing.Size(382, 50);
            this.helpMenuItem.Text = "Help";
            this.helpMenuItem.Click += new System.EventHandler(this.helpMenuItem_Click);
            // 
            // tmrWatchdog
            // 
            this.tmrWatchdog.Interval = 250;
            this.tmrWatchdog.Tick += new System.EventHandler(this.tmrWatchdog_tick);
            // 
            // contextMenuStripFlag
            // 
            this.contextMenuStripFlag.AutoSize = false;
            this.contextMenuStripFlag.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.contextMenuStripFlag.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemFlagRed,
            this.toolStripMenuFlagGrn,
            this.toolStripMenuFlagYel,
            this.toolStripSeparator12,
            this.toolStripMenuFlagForm});
            this.contextMenuStripFlag.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
            this.contextMenuStripFlag.Name = "contextMenuStripFlag";
            this.contextMenuStripFlag.Size = new System.Drawing.Size(72, 312);
            // 
            // toolStripMenuItemFlagRed
            // 
            this.toolStripMenuItemFlagRed.AutoSize = false;
            this.toolStripMenuItemFlagRed.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuItemFlagRed.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuItemFlagRed.Image = global::AgOpenGPS.Properties.Resources.FlagYel;
            this.toolStripMenuItemFlagRed.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuItemFlagRed.Name = "toolStripMenuItemFlagRed";
            this.toolStripMenuItemFlagRed.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuItemFlagRed.Text = ".";
            this.toolStripMenuItemFlagRed.Click += new System.EventHandler(this.toolStripMenuYel_Click);
            // 
            // toolStripMenuFlagGrn
            // 
            this.toolStripMenuFlagGrn.AutoSize = false;
            this.toolStripMenuFlagGrn.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuFlagGrn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuFlagGrn.Image = global::AgOpenGPS.Properties.Resources.FlagGrn;
            this.toolStripMenuFlagGrn.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagGrn.Name = "toolStripMenuFlagGrn";
            this.toolStripMenuFlagGrn.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuFlagGrn.Text = ".";
            this.toolStripMenuFlagGrn.Click += new System.EventHandler(this.toolStripMenuGrn_Click);
            // 
            // toolStripMenuFlagYel
            // 
            this.toolStripMenuFlagYel.AutoSize = false;
            this.toolStripMenuFlagYel.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStripMenuFlagYel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripMenuFlagYel.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.toolStripMenuFlagYel.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagYel.Name = "toolStripMenuFlagYel";
            this.toolStripMenuFlagYel.Size = new System.Drawing.Size(70, 70);
            this.toolStripMenuFlagYel.Text = ".";
            this.toolStripMenuFlagYel.Click += new System.EventHandler(this.toolStripMenuItemFlagRed_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(68, 6);
            // 
            // toolStripMenuFlagForm
            // 
            this.toolStripMenuFlagForm.Image = global::AgOpenGPS.Properties.Resources.FileEditName;
            this.toolStripMenuFlagForm.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripMenuFlagForm.Name = "toolStripMenuFlagForm";
            this.toolStripMenuFlagForm.Size = new System.Drawing.Size(228, 70);
            this.toolStripMenuFlagForm.Text = "toolStripMenuItem3";
            this.toolStripMenuFlagForm.Click += new System.EventHandler(this.toolStripMenuFlagForm_Click);
            // 
            // cboxpRowWidth
            // 
            this.cboxpRowWidth.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxpRowWidth.BackColor = System.Drawing.Color.Lavender;
            this.cboxpRowWidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxpRowWidth.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxpRowWidth.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxpRowWidth.FormattingEnabled = true;
            this.cboxpRowWidth.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9"});
            this.cboxpRowWidth.Location = new System.Drawing.Point(2, 7);
            this.cboxpRowWidth.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxpRowWidth.Name = "cboxpRowWidth";
            this.cboxpRowWidth.Size = new System.Drawing.Size(52, 47);
            this.cboxpRowWidth.TabIndex = 247;
            this.cboxpRowWidth.SelectedIndexChanged += new System.EventHandler(this.cboxpRowWidth_SelectedIndexChanged);
            // 
            // oglZoom
            // 
            this.oglZoom.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.oglZoom.APIVersion = new System.Version(3, 3, 0, 0);
            this.oglZoom.BackColor = System.Drawing.Color.Black;
            this.oglZoom.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.oglZoom.IsEventDriven = true;
            this.oglZoom.Location = new System.Drawing.Point(122, 63);
            this.oglZoom.Margin = new System.Windows.Forms.Padding(0);
            this.oglZoom.Name = "oglZoom";
            this.oglZoom.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.oglZoom.SharedContext = null;
            this.oglZoom.Size = new System.Drawing.Size(41, 44);
            this.oglZoom.TabIndex = 182;
            this.oglZoom.Load += new System.EventHandler(this.oglZoom_Load);
            this.oglZoom.Paint += new System.Windows.Forms.PaintEventHandler(this.oglZoom_Paint);
            this.oglZoom.Resize += new System.EventHandler(this.oglZoom_Resize);
            // 
            // panelDrag
            // 
            this.panelDrag.BackColor = System.Drawing.Color.White;
            this.panelDrag.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.panelDrag.ColumnCount = 1;
            this.panelDrag.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelDrag.Controls.Add(this.btnPathGoStop, 0, 0);
            this.panelDrag.Controls.Add(this.btnPickPath, 0, 3);
            this.panelDrag.Controls.Add(this.btnPathRecordStop, 0, 2);
            this.panelDrag.Controls.Add(this.btnResumePath, 0, 1);
            this.panelDrag.Controls.Add(this.btnSwapABRecordedPath, 0, 4);
            this.panelDrag.Location = new System.Drawing.Point(852, 58);
            this.panelDrag.Name = "panelDrag";
            this.panelDrag.RowCount = 6;
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelDrag.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.panelDrag.Size = new System.Drawing.Size(64, 381);
            this.panelDrag.TabIndex = 445;
            this.panelDrag.Visible = false;
            // 
            // btnPathGoStop
            // 
            this.btnPathGoStop.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.btnPathGoStop.BackColor = System.Drawing.Color.Transparent;
            this.btnPathGoStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPathGoStop.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPathGoStop.FlatAppearance.BorderSize = 0;
            this.btnPathGoStop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPathGoStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPathGoStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPathGoStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPathGoStop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPathGoStop.ForeColor = System.Drawing.Color.DarkGray;
            this.btnPathGoStop.Image = global::AgOpenGPS.Properties.Resources.boundaryPlay;
            this.btnPathGoStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPathGoStop.Location = new System.Drawing.Point(0, 0);
            this.btnPathGoStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnPathGoStop.Name = "btnPathGoStop";
            this.btnPathGoStop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPathGoStop.Size = new System.Drawing.Size(64, 61);
            this.btnPathGoStop.TabIndex = 468;
            this.btnPathGoStop.UseVisualStyleBackColor = false;
            this.btnPathGoStop.Click += new System.EventHandler(this.btnPathGoStop_Click);
            // 
            // btnPickPath
            // 
            this.btnPickPath.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnPickPath.BackColor = System.Drawing.Color.Transparent;
            this.btnPickPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPickPath.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPickPath.FlatAppearance.BorderSize = 0;
            this.btnPickPath.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPickPath.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPickPath.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPickPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPickPath.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPickPath.ForeColor = System.Drawing.Color.DarkGray;
            this.btnPickPath.Image = global::AgOpenGPS.Properties.Resources.FileExplorerWindows;
            this.btnPickPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPickPath.Location = new System.Drawing.Point(0, 227);
            this.btnPickPath.Margin = new System.Windows.Forms.Padding(0);
            this.btnPickPath.Name = "btnPickPath";
            this.btnPickPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPickPath.Size = new System.Drawing.Size(64, 61);
            this.btnPickPath.TabIndex = 471;
            this.btnPickPath.UseVisualStyleBackColor = false;
            this.btnPickPath.Click += new System.EventHandler(this.btnPickPath_Click);
            // 
            // btnPathRecordStop
            // 
            this.btnPathRecordStop.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPathRecordStop.BackColor = System.Drawing.Color.Transparent;
            this.btnPathRecordStop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnPathRecordStop.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPathRecordStop.FlatAppearance.BorderSize = 0;
            this.btnPathRecordStop.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPathRecordStop.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPathRecordStop.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPathRecordStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPathRecordStop.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPathRecordStop.ForeColor = System.Drawing.Color.DarkGray;
            this.btnPathRecordStop.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.btnPathRecordStop.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPathRecordStop.Location = new System.Drawing.Point(0, 149);
            this.btnPathRecordStop.Margin = new System.Windows.Forms.Padding(0);
            this.btnPathRecordStop.Name = "btnPathRecordStop";
            this.btnPathRecordStop.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPathRecordStop.Size = new System.Drawing.Size(64, 61);
            this.btnPathRecordStop.TabIndex = 470;
            this.btnPathRecordStop.UseVisualStyleBackColor = false;
            this.btnPathRecordStop.Click += new System.EventHandler(this.btnPathRecordStop_Click);
            // 
            // btnResumePath
            // 
            this.btnResumePath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnResumePath.BackColor = System.Drawing.Color.Transparent;
            this.btnResumePath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResumePath.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnResumePath.FlatAppearance.BorderSize = 0;
            this.btnResumePath.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnResumePath.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResumePath.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResumePath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResumePath.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResumePath.ForeColor = System.Drawing.Color.Red;
            this.btnResumePath.Image = global::AgOpenGPS.Properties.Resources.pathResumeStart;
            this.btnResumePath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResumePath.Location = new System.Drawing.Point(0, 76);
            this.btnResumePath.Margin = new System.Windows.Forms.Padding(0);
            this.btnResumePath.Name = "btnResumePath";
            this.btnResumePath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnResumePath.Size = new System.Drawing.Size(64, 64);
            this.btnResumePath.TabIndex = 472;
            this.btnResumePath.UseVisualStyleBackColor = false;
            this.btnResumePath.Click += new System.EventHandler(this.btnResumePath_Click);
            // 
            // btnSwapABRecordedPath
            // 
            this.btnSwapABRecordedPath.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSwapABRecordedPath.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapABRecordedPath.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSwapABRecordedPath.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnSwapABRecordedPath.FlatAppearance.BorderSize = 0;
            this.btnSwapABRecordedPath.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSwapABRecordedPath.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSwapABRecordedPath.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSwapABRecordedPath.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapABRecordedPath.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapABRecordedPath.ForeColor = System.Drawing.Color.DarkGray;
            this.btnSwapABRecordedPath.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnSwapABRecordedPath.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSwapABRecordedPath.Location = new System.Drawing.Point(0, 293);
            this.btnSwapABRecordedPath.Margin = new System.Windows.Forms.Padding(0);
            this.btnSwapABRecordedPath.Name = "btnSwapABRecordedPath";
            this.btnSwapABRecordedPath.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSwapABRecordedPath.Size = new System.Drawing.Size(64, 61);
            this.btnSwapABRecordedPath.TabIndex = 470;
            this.btnSwapABRecordedPath.UseVisualStyleBackColor = false;
            this.btnSwapABRecordedPath.Click += new System.EventHandler(this.btnSwapABRecordedPath_Click);
            // 
            // btnResetSim
            // 
            this.btnResetSim.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSim.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnResetSim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResetSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSim.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSim.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSim.Location = new System.Drawing.Point(4, 4);
            this.btnResetSim.Name = "btnResetSim";
            this.btnResetSim.Size = new System.Drawing.Size(59, 34);
            this.btnResetSim.TabIndex = 164;
            this.btnResetSim.Text = "Reset";
            this.btnResetSim.UseVisualStyleBackColor = false;
            this.btnResetSim.Click += new System.EventHandler(this.btnResetSim_Click);
            // 
            // btnResetSteerAngle
            // 
            this.btnResetSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.btnResetSteerAngle.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnResetSteerAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnResetSteerAngle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetSteerAngle.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnResetSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetSteerAngle.Location = new System.Drawing.Point(70, 4);
            this.btnResetSteerAngle.Name = "btnResetSteerAngle";
            this.btnResetSteerAngle.Size = new System.Drawing.Size(59, 34);
            this.btnResetSteerAngle.TabIndex = 162;
            this.btnResetSteerAngle.Text = ">0<";
            this.btnResetSteerAngle.UseVisualStyleBackColor = false;
            this.btnResetSteerAngle.Click += new System.EventHandler(this.btnResetSteerAngle_Click);
            // 
            // timerSim
            // 
            this.timerSim.Interval = 93;
            this.timerSim.Tick += new System.EventHandler(this.timerSim_Tick);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(334, 62);
            this.toolStripMenuItem2.Text = "toolStripMenuItem2";
            // 
            // hsbarSteerAngle
            // 
            this.hsbarSteerAngle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hsbarSteerAngle.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.hsbarSteerAngle.LargeChange = 20;
            this.hsbarSteerAngle.Location = new System.Drawing.Point(158, 1);
            this.hsbarSteerAngle.Maximum = 800;
            this.hsbarSteerAngle.Name = "hsbarSteerAngle";
            this.hsbarSteerAngle.Size = new System.Drawing.Size(153, 40);
            this.hsbarSteerAngle.TabIndex = 179;
            this.hsbarSteerAngle.Value = 400;
            this.hsbarSteerAngle.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hsbarSteerAngle_Scroll);
            // 
            // btnSection8Man
            // 
            this.btnSection8Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection8Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection8Man.Enabled = false;
            this.btnSection8Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection8Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection8Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection8Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection8Man.Location = new System.Drawing.Point(817, 243);
            this.btnSection8Man.Name = "btnSection8Man";
            this.btnSection8Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection8Man.TabIndex = 125;
            this.btnSection8Man.Text = "8";
            this.btnSection8Man.UseVisualStyleBackColor = false;
            this.btnSection8Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection7Man
            // 
            this.btnSection7Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection7Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection7Man.Enabled = false;
            this.btnSection7Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection7Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection7Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection7Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection7Man.Location = new System.Drawing.Point(817, 216);
            this.btnSection7Man.Name = "btnSection7Man";
            this.btnSection7Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection7Man.TabIndex = 126;
            this.btnSection7Man.Text = "7";
            this.btnSection7Man.UseVisualStyleBackColor = false;
            this.btnSection7Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection6Man
            // 
            this.btnSection6Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection6Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection6Man.Enabled = false;
            this.btnSection6Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection6Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection6Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection6Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection6Man.Location = new System.Drawing.Point(817, 189);
            this.btnSection6Man.Name = "btnSection6Man";
            this.btnSection6Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection6Man.TabIndex = 127;
            this.btnSection6Man.Text = "6";
            this.btnSection6Man.UseVisualStyleBackColor = false;
            this.btnSection6Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection5Man
            // 
            this.btnSection5Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection5Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection5Man.Enabled = false;
            this.btnSection5Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection5Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection5Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection5Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection5Man.Location = new System.Drawing.Point(817, 162);
            this.btnSection5Man.Name = "btnSection5Man";
            this.btnSection5Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection5Man.TabIndex = 103;
            this.btnSection5Man.Text = "5";
            this.btnSection5Man.UseVisualStyleBackColor = false;
            this.btnSection5Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection4Man
            // 
            this.btnSection4Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection4Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection4Man.Enabled = false;
            this.btnSection4Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection4Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection4Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection4Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection4Man.Location = new System.Drawing.Point(817, 136);
            this.btnSection4Man.Name = "btnSection4Man";
            this.btnSection4Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection4Man.TabIndex = 102;
            this.btnSection4Man.Text = "4";
            this.btnSection4Man.UseVisualStyleBackColor = false;
            this.btnSection4Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection3Man
            // 
            this.btnSection3Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection3Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection3Man.Enabled = false;
            this.btnSection3Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection3Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection3Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection3Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection3Man.Location = new System.Drawing.Point(817, 110);
            this.btnSection3Man.Name = "btnSection3Man";
            this.btnSection3Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection3Man.TabIndex = 101;
            this.btnSection3Man.Text = "3";
            this.btnSection3Man.UseVisualStyleBackColor = false;
            this.btnSection3Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection2Man
            // 
            this.btnSection2Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection2Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection2Man.Enabled = false;
            this.btnSection2Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection2Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection2Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection2Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection2Man.Location = new System.Drawing.Point(817, 83);
            this.btnSection2Man.Name = "btnSection2Man";
            this.btnSection2Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection2Man.TabIndex = 100;
            this.btnSection2Man.Text = "2";
            this.btnSection2Man.UseVisualStyleBackColor = false;
            this.btnSection2Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection1Man
            // 
            this.btnSection1Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection1Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection1Man.Enabled = false;
            this.btnSection1Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection1Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection1Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection1Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection1Man.Location = new System.Drawing.Point(817, 56);
            this.btnSection1Man.Name = "btnSection1Man";
            this.btnSection1Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection1Man.TabIndex = 99;
            this.btnSection1Man.Text = "1";
            this.btnSection1Man.UseVisualStyleBackColor = false;
            this.btnSection1Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection9Man
            // 
            this.btnSection9Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection9Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection9Man.Enabled = false;
            this.btnSection9Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection9Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection9Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection9Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection9Man.Location = new System.Drawing.Point(777, 56);
            this.btnSection9Man.Name = "btnSection9Man";
            this.btnSection9Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection9Man.TabIndex = 174;
            this.btnSection9Man.Text = "9";
            this.btnSection9Man.UseVisualStyleBackColor = false;
            this.btnSection9Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection10Man
            // 
            this.btnSection10Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection10Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection10Man.Enabled = false;
            this.btnSection10Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection10Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection10Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection10Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection10Man.Location = new System.Drawing.Point(777, 83);
            this.btnSection10Man.Name = "btnSection10Man";
            this.btnSection10Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection10Man.TabIndex = 175;
            this.btnSection10Man.Text = "10";
            this.btnSection10Man.UseVisualStyleBackColor = false;
            this.btnSection10Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection11Man
            // 
            this.btnSection11Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection11Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection11Man.Enabled = false;
            this.btnSection11Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection11Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection11Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection11Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection11Man.Location = new System.Drawing.Point(777, 110);
            this.btnSection11Man.Name = "btnSection11Man";
            this.btnSection11Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection11Man.TabIndex = 176;
            this.btnSection11Man.Text = "11";
            this.btnSection11Man.UseVisualStyleBackColor = false;
            this.btnSection11Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection12Man
            // 
            this.btnSection12Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection12Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection12Man.Enabled = false;
            this.btnSection12Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection12Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection12Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection12Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection12Man.Location = new System.Drawing.Point(777, 136);
            this.btnSection12Man.Name = "btnSection12Man";
            this.btnSection12Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection12Man.TabIndex = 177;
            this.btnSection12Man.Text = "12";
            this.btnSection12Man.UseVisualStyleBackColor = false;
            this.btnSection12Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // oglMain
            // 
            this.oglMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.oglMain.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.oglMain.APIVersion = new System.Version(3, 3, 0, 0);
            this.oglMain.BackColor = System.Drawing.Color.Black;
            this.oglMain.ContextMenuStrip = this.contextMenuStripOpenGL;
            this.oglMain.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.oglMain.IsEventDriven = true;
            this.oglMain.Location = new System.Drawing.Point(78, 50);
            this.oglMain.Margin = new System.Windows.Forms.Padding(0);
            this.oglMain.Name = "oglMain";
            this.oglMain.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.oglMain.SharedContext = null;
            this.oglMain.Size = new System.Drawing.Size(848, 601);
            this.oglMain.TabIndex = 180;
            this.oglMain.Load += new System.EventHandler(this.oglMain_Load);
            this.oglMain.Paint += new System.Windows.Forms.PaintEventHandler(this.oglMain_Paint);
            this.oglMain.MouseDown += new System.Windows.Forms.MouseEventHandler(this.oglMain_MouseDown);
            this.oglMain.Resize += new System.EventHandler(this.oglMain_Resize);
            // 
            // oglBack
            // 
            this.oglBack.API = OpenTK.Windowing.Common.ContextAPI.OpenGL;
            this.oglBack.APIVersion = new System.Version(3, 3, 0, 0);
            this.oglBack.BackColor = System.Drawing.Color.Black;
            this.oglBack.Flags = OpenTK.Windowing.Common.ContextFlags.Default;
            this.oglBack.ForeColor = System.Drawing.Color.Transparent;
            this.oglBack.IsEventDriven = true;
            this.oglBack.Location = new System.Drawing.Point(122, 70);
            this.oglBack.Margin = new System.Windows.Forms.Padding(2, 1, 2, 1);
            this.oglBack.Name = "oglBack";
            this.oglBack.Profile = OpenTK.Windowing.Common.ContextProfile.Compatability;
            this.oglBack.SharedContext = null;
            this.oglBack.Size = new System.Drawing.Size(500, 300);
            this.oglBack.TabIndex = 181;
            this.oglBack.Load += new System.EventHandler(this.oglBack_Load);
            this.oglBack.Paint += new System.Windows.Forms.PaintEventHandler(this.oglBack_Paint);
            this.oglBack.Resize += new System.EventHandler(this.oglBack_Resize);
            // 
            // lblHz
            // 
            this.lblHz.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblHz.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHz.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblHz.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHz.Location = new System.Drawing.Point(92, 276);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(84, 79);
            this.lblHz.TabIndex = 249;
            this.lblHz.Text = "5 Hz 32\r\nPPS";
            this.lblHz.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // statusStripLeft
            // 
            this.statusStripLeft.AllowMerge = false;
            this.statusStripLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusStripLeft.AutoSize = false;
            this.statusStripLeft.BackColor = System.Drawing.Color.Transparent;
            this.statusStripLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStripLeft.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStripLeft.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStripLeft.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStripLeft.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.statusStripLeft.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton4});
            this.statusStripLeft.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStripLeft.Location = new System.Drawing.Point(2, 67);
            this.statusStripLeft.Name = "statusStripLeft";
            this.statusStripLeft.Size = new System.Drawing.Size(67, 109);
            this.statusStripLeft.SizingGrip = false;
            this.statusStripLeft.TabIndex = 324;
            this.statusStripLeft.Text = "UDP";
            // 
            // toolStripDropDownButton4
            // 
            this.toolStripDropDownButton4.AutoSize = false;
            this.toolStripDropDownButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton4.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wizardsMenu,
            this.steerChartStripMenu,
            this.boundaryToolToolStripMenu,
            this.eventViewerToolStripMenuItem,
            this.guidelinesToolStripMenuItem,
            this.SmoothABtoolStripMenu,
            this.deleteContourPathsToolStripMenuItem,
            this.webcamToolStrip,
            this.offsetFixToolStrip});
            this.toolStripDropDownButton4.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton4.Image = global::AgOpenGPS.Properties.Resources.SpecialFunctions;
            this.toolStripDropDownButton4.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton4.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripDropDownButton4.Name = "toolStripDropDownButton4";
            this.toolStripDropDownButton4.ShowDropDownArrow = false;
            this.toolStripDropDownButton4.Size = new System.Drawing.Size(66, 80);
            this.toolStripDropDownButton4.Text = "toolStripDropDownButton3";
            // 
            // wizardsMenu
            // 
            this.wizardsMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.steerWizardMenuItem});
            this.wizardsMenu.Image = global::AgOpenGPS.Properties.Resources.WizardWand;
            this.wizardsMenu.Name = "wizardsMenu";
            this.wizardsMenu.Size = new System.Drawing.Size(450, 70);
            this.wizardsMenu.Text = "Wizards";
            // 
            // steerWizardMenuItem
            // 
            this.steerWizardMenuItem.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOn;
            this.steerWizardMenuItem.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.steerWizardMenuItem.Name = "steerWizardMenuItem";
            this.steerWizardMenuItem.Size = new System.Drawing.Size(301, 44);
            this.steerWizardMenuItem.Text = "Steer Wizard";
            this.steerWizardMenuItem.Click += new System.EventHandler(this.steerWizardMenuItem_Click);
            // 
            // steerChartStripMenu
            // 
            this.steerChartStripMenu.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.steerChartToolStripMenuItem,
            this.headingChartToolStripMenuItem,
            this.xTEChartToolStripMenuItem,
            this.rollCheckToolStripMenuItem});
            this.steerChartStripMenu.Image = global::AgOpenGPS.Properties.Resources.Chart;
            this.steerChartStripMenu.Name = "steerChartStripMenu";
            this.steerChartStripMenu.Size = new System.Drawing.Size(450, 70);
            this.steerChartStripMenu.Text = "Charts";
            // 
            // steerChartToolStripMenuItem
            // 
            this.steerChartToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOn;
            this.steerChartToolStripMenuItem.Name = "steerChartToolStripMenuItem";
            this.steerChartToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.steerChartToolStripMenuItem.Text = "Steer Chart";
            this.steerChartToolStripMenuItem.Click += new System.EventHandler(this.toolStripAutoSteerChart_Click);
            // 
            // headingChartToolStripMenuItem
            // 
            this.headingChartToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConS_SourcesHeading;
            this.headingChartToolStripMenuItem.Name = "headingChartToolStripMenuItem";
            this.headingChartToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.headingChartToolStripMenuItem.Text = "Heading Chart";
            this.headingChartToolStripMenuItem.Click += new System.EventHandler(this.headingChartToolStripMenuItem_Click);
            // 
            // xTEChartToolStripMenuItem
            // 
            this.xTEChartToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.AutoManualIsAuto;
            this.xTEChartToolStripMenuItem.Name = "xTEChartToolStripMenuItem";
            this.xTEChartToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.xTEChartToolStripMenuItem.Text = "XTE Chart";
            this.xTEChartToolStripMenuItem.Click += new System.EventHandler(this.xTEChartToolStripMenuItem_Click);
            // 
            // rollCheckToolStripMenuItem
            // 
            this.rollCheckToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConS_SourcesRoll;
            this.rollCheckToolStripMenuItem.Name = "rollCheckToolStripMenuItem";
            this.rollCheckToolStripMenuItem.Size = new System.Drawing.Size(322, 44);
            this.rollCheckToolStripMenuItem.Text = "Roll Check";
            this.rollCheckToolStripMenuItem.Click += new System.EventHandler(this.correctionToolStrip_Click);
            // 
            // boundaryToolToolStripMenu
            // 
            this.boundaryToolToolStripMenu.Image = global::AgOpenGPS.Properties.Resources.Boundary;
            this.boundaryToolToolStripMenu.Name = "boundaryToolToolStripMenu";
            this.boundaryToolToolStripMenu.Size = new System.Drawing.Size(450, 70);
            this.boundaryToolToolStripMenu.Text = "Boundary Tool";
            this.boundaryToolToolStripMenu.Click += new System.EventHandler(this.boundaryToolToolStripMenu_Click);
            // 
            // eventViewerToolStripMenuItem
            // 
            this.eventViewerToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ABTracks;
            this.eventViewerToolStripMenuItem.Name = "eventViewerToolStripMenuItem";
            this.eventViewerToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.eventViewerToolStripMenuItem.Text = "Log Viewer";
            this.eventViewerToolStripMenuItem.Click += new System.EventHandler(this.eventViewerToolStripMenuItem_Click);
            // 
            // guidelinesToolStripMenuItem
            // 
            this.guidelinesToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.ConD_ExtraGuides;
            this.guidelinesToolStripMenuItem.Name = "guidelinesToolStripMenuItem";
            this.guidelinesToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.guidelinesToolStripMenuItem.Text = "Extra Guides";
            this.guidelinesToolStripMenuItem.Click += new System.EventHandler(this.guidelinesToolStripMenuItem_Click);
            // 
            // SmoothABtoolStripMenu
            // 
            this.SmoothABtoolStripMenu.Image = global::AgOpenGPS.Properties.Resources.ABSmooth;
            this.SmoothABtoolStripMenu.Name = "SmoothABtoolStripMenu";
            this.SmoothABtoolStripMenu.Size = new System.Drawing.Size(450, 70);
            this.SmoothABtoolStripMenu.Text = "Smooth AB Curve";
            this.SmoothABtoolStripMenu.Click += new System.EventHandler(this.SmoothABtoolStripMenu_Click);
            // 
            // deleteContourPathsToolStripMenuItem
            // 
            this.deleteContourPathsToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.TrashContourRef;
            this.deleteContourPathsToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.deleteContourPathsToolStripMenuItem.Name = "deleteContourPathsToolStripMenuItem";
            this.deleteContourPathsToolStripMenuItem.Size = new System.Drawing.Size(450, 70);
            this.deleteContourPathsToolStripMenuItem.Text = "Hide Contour Paths";
            this.deleteContourPathsToolStripMenuItem.Click += new System.EventHandler(this.deleteContourPathsToolStripMenuItem_Click);
            // 
            // webcamToolStrip
            // 
            this.webcamToolStrip.Image = global::AgOpenGPS.Properties.Resources.Webcam;
            this.webcamToolStrip.Name = "webcamToolStrip";
            this.webcamToolStrip.Size = new System.Drawing.Size(450, 70);
            this.webcamToolStrip.Text = "WebCam";
            this.webcamToolStrip.Click += new System.EventHandler(this.webcamToolStrip_Click);
            // 
            // offsetFixToolStrip
            // 
            this.offsetFixToolStrip.Image = global::AgOpenGPS.Properties.Resources.YouTurnReverse;
            this.offsetFixToolStrip.Name = "offsetFixToolStrip";
            this.offsetFixToolStrip.Size = new System.Drawing.Size(450, 70);
            this.offsetFixToolStrip.Text = "Offset Fix";
            this.offsetFixToolStrip.Click += new System.EventHandler(this.offsetFixToolStrip_Click);
            // 
            // panelSim
            // 
            this.panelSim.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.panelSim.BackColor = System.Drawing.Color.Transparent;
            this.panelSim.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.panelSim.ColumnCount = 9;
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 24F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 153F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 17F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelSim.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 64F));
            this.panelSim.Controls.Add(this.btnSpeedDn, 5, 0);
            this.panelSim.Controls.Add(this.btnSimSpeedUp, 7, 0);
            this.panelSim.Controls.Add(this.btnResetSim, 0, 0);
            this.panelSim.Controls.Add(this.btnSimSetSpeedToZero, 6, 0);
            this.panelSim.Controls.Add(this.btnResetSteerAngle, 1, 0);
            this.panelSim.Controls.Add(this.hsbarSteerAngle, 3, 0);
            this.panelSim.Controls.Add(this.btnSimReverseDirection, 8, 0);
            this.panelSim.Location = new System.Drawing.Point(301, 594);
            this.panelSim.Name = "panelSim";
            this.panelSim.RowCount = 1;
            this.panelSim.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelSim.Size = new System.Drawing.Size(615, 42);
            this.panelSim.TabIndex = 325;
            // 
            // btnSpeedDn
            // 
            this.btnSpeedDn.BackColor = System.Drawing.Color.Transparent;
            this.btnSpeedDn.BackgroundImage = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnSpeedDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSpeedDn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSpeedDn.FlatAppearance.BorderSize = 0;
            this.btnSpeedDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSpeedDn.Location = new System.Drawing.Point(333, 4);
            this.btnSpeedDn.Name = "btnSpeedDn";
            this.btnSpeedDn.Size = new System.Drawing.Size(74, 34);
            this.btnSpeedDn.TabIndex = 533;
            this.btnSpeedDn.UseVisualStyleBackColor = false;
            this.btnSpeedDn.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSpeedDn_MouseDown);
            // 
            // btnSimSpeedUp
            // 
            this.btnSimSpeedUp.BackColor = System.Drawing.Color.Transparent;
            this.btnSimSpeedUp.BackgroundImage = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnSimSpeedUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimSpeedUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSimSpeedUp.FlatAppearance.BorderSize = 0;
            this.btnSimSpeedUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimSpeedUp.Location = new System.Drawing.Point(471, 4);
            this.btnSimSpeedUp.Name = "btnSimSpeedUp";
            this.btnSimSpeedUp.Size = new System.Drawing.Size(74, 34);
            this.btnSimSpeedUp.TabIndex = 532;
            this.btnSimSpeedUp.UseVisualStyleBackColor = false;
            this.btnSimSpeedUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnSimSpeedUp_MouseDown);
            // 
            // btnSimSetSpeedToZero
            // 
            this.btnSimSetSpeedToZero.BackColor = System.Drawing.Color.Transparent;
            this.btnSimSetSpeedToZero.BackgroundImage = global::AgOpenGPS.Properties.Resources.AutoStop;
            this.btnSimSetSpeedToZero.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimSetSpeedToZero.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnSimSetSpeedToZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimSetSpeedToZero.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSimSetSpeedToZero.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSimSetSpeedToZero.Location = new System.Drawing.Point(414, 4);
            this.btnSimSetSpeedToZero.Name = "btnSimSetSpeedToZero";
            this.btnSimSetSpeedToZero.Size = new System.Drawing.Size(50, 34);
            this.btnSimSetSpeedToZero.TabIndex = 453;
            this.btnSimSetSpeedToZero.UseVisualStyleBackColor = false;
            this.btnSimSetSpeedToZero.Click += new System.EventHandler(this.btnSimSetSpeedToZero_Click);
            // 
            // btnSimReverseDirection
            // 
            this.btnSimReverseDirection.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSimReverseDirection.BackColor = System.Drawing.Color.Transparent;
            this.btnSimReverseDirection.BackgroundImage = global::AgOpenGPS.Properties.Resources.Youturn80;
            this.btnSimReverseDirection.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSimReverseDirection.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnSimReverseDirection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSimReverseDirection.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.btnSimReverseDirection.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSimReverseDirection.Location = new System.Drawing.Point(564, 4);
            this.btnSimReverseDirection.Name = "btnSimReverseDirection";
            this.btnSimReverseDirection.Size = new System.Drawing.Size(35, 34);
            this.btnSimReverseDirection.TabIndex = 537;
            this.btnSimReverseDirection.UseVisualStyleBackColor = false;
            this.btnSimReverseDirection.Click += new System.EventHandler(this.btnSimReverseDirection_Click);
            // 
            // btnSection16Man
            // 
            this.btnSection16Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection16Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection16Man.Enabled = false;
            this.btnSection16Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection16Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection16Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection16Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection16Man.Location = new System.Drawing.Point(777, 243);
            this.btnSection16Man.Name = "btnSection16Man";
            this.btnSection16Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection16Man.TabIndex = 448;
            this.btnSection16Man.Text = "16";
            this.btnSection16Man.UseVisualStyleBackColor = false;
            this.btnSection16Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection15Man
            // 
            this.btnSection15Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection15Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection15Man.Enabled = false;
            this.btnSection15Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection15Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection15Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection15Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection15Man.Location = new System.Drawing.Point(777, 216);
            this.btnSection15Man.Name = "btnSection15Man";
            this.btnSection15Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection15Man.TabIndex = 449;
            this.btnSection15Man.Text = "15";
            this.btnSection15Man.UseVisualStyleBackColor = false;
            this.btnSection15Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection14Man
            // 
            this.btnSection14Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection14Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection14Man.Enabled = false;
            this.btnSection14Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection14Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection14Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection14Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection14Man.Location = new System.Drawing.Point(777, 189);
            this.btnSection14Man.Name = "btnSection14Man";
            this.btnSection14Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection14Man.TabIndex = 450;
            this.btnSection14Man.Text = "14";
            this.btnSection14Man.UseVisualStyleBackColor = false;
            this.btnSection14Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // btnSection13Man
            // 
            this.btnSection13Man.BackColor = System.Drawing.Color.Silver;
            this.btnSection13Man.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnSection13Man.Enabled = false;
            this.btnSection13Man.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnSection13Man.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSection13Man.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSection13Man.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSection13Man.Location = new System.Drawing.Point(777, 162);
            this.btnSection13Man.Name = "btnSection13Man";
            this.btnSection13Man.Size = new System.Drawing.Size(34, 25);
            this.btnSection13Man.TabIndex = 451;
            this.btnSection13Man.Text = "13";
            this.btnSection13Man.UseVisualStyleBackColor = false;
            this.btnSection13Man.Click += new System.EventHandler(this.btnSectionXMan_Click);
            // 
            // panelNavigation
            // 
            this.panelNavigation.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelNavigation.ColumnCount = 2;
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelNavigation.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.panelNavigation.Controls.Add(this.btnTiltDn, 0, 0);
            this.panelNavigation.Controls.Add(this.btnTiltUp, 1, 0);
            this.panelNavigation.Controls.Add(this.btnBrightnessDn, 0, 4);
            this.panelNavigation.Controls.Add(this.btnBrightnessUp, 1, 4);
            this.panelNavigation.Controls.Add(this.btnDayNightMode, 0, 3);
            this.panelNavigation.Controls.Add(this.lblHz, 1, 3);
            this.panelNavigation.Controls.Add(this.btn3D, 1, 1);
            this.panelNavigation.Controls.Add(this.btn2D, 0, 1);
            this.panelNavigation.Controls.Add(this.btnGrid, 1, 2);
            this.panelNavigation.Controls.Add(this.btnN2D, 0, 2);
            this.panelNavigation.Location = new System.Drawing.Point(505, 63);
            this.panelNavigation.Name = "panelNavigation";
            this.panelNavigation.RowCount = 5;
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.panelNavigation.Size = new System.Drawing.Size(179, 460);
            this.panelNavigation.TabIndex = 468;
            this.panelNavigation.Visible = false;
            // 
            // btnTiltDn
            // 
            this.btnTiltDn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTiltDn.BackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTiltDn.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTiltDn.FlatAppearance.BorderSize = 0;
            this.btnTiltDn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTiltDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTiltDn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiltDn.Image = global::AgOpenGPS.Properties.Resources.TiltDown;
            this.btnTiltDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTiltDn.Location = new System.Drawing.Point(16, 18);
            this.btnTiltDn.Name = "btnTiltDn";
            this.btnTiltDn.Size = new System.Drawing.Size(57, 55);
            this.btnTiltDn.TabIndex = 543;
            this.btnTiltDn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTiltDn.UseVisualStyleBackColor = false;
            this.btnTiltDn.Click += new System.EventHandler(this.btnTiltDn_Click);
            // 
            // btnTiltUp
            // 
            this.btnTiltUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTiltUp.BackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTiltUp.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTiltUp.FlatAppearance.BorderSize = 0;
            this.btnTiltUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTiltUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTiltUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTiltUp.Image = global::AgOpenGPS.Properties.Resources.TiltUp;
            this.btnTiltUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTiltUp.Location = new System.Drawing.Point(105, 18);
            this.btnTiltUp.Name = "btnTiltUp";
            this.btnTiltUp.Size = new System.Drawing.Size(57, 55);
            this.btnTiltUp.TabIndex = 544;
            this.btnTiltUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTiltUp.UseVisualStyleBackColor = false;
            this.btnTiltUp.Click += new System.EventHandler(this.btnTiltUp_Click);
            // 
            // btnBrightnessDn
            // 
            this.btnBrightnessDn.BackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrightnessDn.FlatAppearance.BorderSize = 0;
            this.btnBrightnessDn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessDn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightnessDn.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrightnessDn.Image = global::AgOpenGPS.Properties.Resources.BrightnessDn;
            this.btnBrightnessDn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrightnessDn.Location = new System.Drawing.Point(3, 371);
            this.btnBrightnessDn.Name = "btnBrightnessDn";
            this.btnBrightnessDn.Size = new System.Drawing.Size(83, 75);
            this.btnBrightnessDn.TabIndex = 474;
            this.btnBrightnessDn.Text = "20%";
            this.btnBrightnessDn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnBrightnessDn.UseVisualStyleBackColor = false;
            this.btnBrightnessDn.Click += new System.EventHandler(this.btnBrightnessDn_Click);
            // 
            // btnBrightnessUp
            // 
            this.btnBrightnessUp.BackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBrightnessUp.FlatAppearance.BorderSize = 0;
            this.btnBrightnessUp.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBrightnessUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBrightnessUp.Image = global::AgOpenGPS.Properties.Resources.BrightnessUp;
            this.btnBrightnessUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBrightnessUp.Location = new System.Drawing.Point(92, 371);
            this.btnBrightnessUp.Name = "btnBrightnessUp";
            this.btnBrightnessUp.Size = new System.Drawing.Size(84, 75);
            this.btnBrightnessUp.TabIndex = 473;
            this.btnBrightnessUp.UseVisualStyleBackColor = false;
            this.btnBrightnessUp.Click += new System.EventHandler(this.btnBrightnessUp_Click);
            // 
            // btnDayNightMode
            // 
            this.btnDayNightMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDayNightMode.BackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDayNightMode.FlatAppearance.BorderSize = 0;
            this.btnDayNightMode.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnDayNightMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDayNightMode.Image = global::AgOpenGPS.Properties.Resources.WindowNightMode;
            this.btnDayNightMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDayNightMode.Location = new System.Drawing.Point(16, 294);
            this.btnDayNightMode.Name = "btnDayNightMode";
            this.btnDayNightMode.Size = new System.Drawing.Size(57, 55);
            this.btnDayNightMode.TabIndex = 452;
            this.btnDayNightMode.UseVisualStyleBackColor = false;
            this.btnDayNightMode.Click += new System.EventHandler(this.btnDayNightMode_Click);
            // 
            // btn3D
            // 
            this.btn3D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn3D.BackColor = System.Drawing.Color.Transparent;
            this.btn3D.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn3D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn3D.FlatAppearance.BorderSize = 0;
            this.btn3D.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn3D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn3D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn3D.Image = global::AgOpenGPS.Properties.Resources.Camera3D64;
            this.btn3D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn3D.Location = new System.Drawing.Point(105, 110);
            this.btn3D.Name = "btn3D";
            this.btn3D.Size = new System.Drawing.Size(57, 55);
            this.btn3D.TabIndex = 471;
            this.btn3D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn3D.UseVisualStyleBackColor = false;
            this.btn3D.Click += new System.EventHandler(this.btn3D_Click);
            // 
            // btn2D
            // 
            this.btn2D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btn2D.BackColor = System.Drawing.Color.Transparent;
            this.btn2D.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn2D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btn2D.FlatAppearance.BorderSize = 0;
            this.btn2D.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btn2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn2D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btn2D.Image = global::AgOpenGPS.Properties.Resources.Camera2D64;
            this.btn2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btn2D.Location = new System.Drawing.Point(16, 110);
            this.btn2D.Name = "btn2D";
            this.btn2D.Size = new System.Drawing.Size(57, 55);
            this.btn2D.TabIndex = 469;
            this.btn2D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btn2D.UseVisualStyleBackColor = false;
            this.btn2D.Click += new System.EventHandler(this.btn2D_Click);
            // 
            // btnGrid
            // 
            this.btnGrid.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnGrid.BackColor = System.Drawing.Color.Transparent;
            this.btnGrid.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnGrid.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnGrid.FlatAppearance.BorderSize = 0;
            this.btnGrid.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnGrid.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGrid.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGrid.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGrid.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGrid.Image = global::AgOpenGPS.Properties.Resources.GridRotate;
            this.btnGrid.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGrid.Location = new System.Drawing.Point(94, 188);
            this.btnGrid.Name = "btnGrid";
            this.btnGrid.Size = new System.Drawing.Size(80, 83);
            this.btnGrid.TabIndex = 543;
            this.btnGrid.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnGrid.UseVisualStyleBackColor = false;
            this.btnGrid.Click += new System.EventHandler(this.btnGrid_Click);
            // 
            // btnN2D
            // 
            this.btnN2D.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnN2D.BackColor = System.Drawing.Color.Transparent;
            this.btnN2D.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnN2D.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnN2D.FlatAppearance.BorderSize = 0;
            this.btnN2D.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnN2D.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnN2D.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnN2D.Image = global::AgOpenGPS.Properties.Resources.CameraNorth2D;
            this.btnN2D.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnN2D.Location = new System.Drawing.Point(16, 202);
            this.btnN2D.Name = "btnN2D";
            this.btnN2D.Size = new System.Drawing.Size(57, 55);
            this.btnN2D.TabIndex = 469;
            this.btnN2D.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnN2D.UseVisualStyleBackColor = false;
            this.btnN2D.Click += new System.EventHandler(this.btnN2D_Click);
            // 
            // lblSpeed
            // 
            this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSpeed.BackColor = System.Drawing.Color.Transparent;
            this.lblSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblSpeed.Font = new System.Drawing.Font("Arial", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSpeed.ForeColor = System.Drawing.Color.Black;
            this.lblSpeed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSpeed.Location = new System.Drawing.Point(193, 7);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(87, 35);
            this.lblSpeed.TabIndex = 116;
            this.lblSpeed.Text = "88.88";
            this.lblSpeed.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblFix
            // 
            this.lblFix.AutoSize = true;
            this.lblFix.BackColor = System.Drawing.Color.Transparent;
            this.lblFix.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFix.Location = new System.Drawing.Point(84, 1);
            this.lblFix.Name = "lblFix";
            this.lblFix.Size = new System.Drawing.Size(87, 19);
            this.lblFix.TabIndex = 489;
            this.lblFix.Text = "Fix       sec";
            this.lblFix.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnZone1
            // 
            this.btnZone1.BackColor = System.Drawing.Color.Silver;
            this.btnZone1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone1.Enabled = false;
            this.btnZone1.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone1.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone1.Location = new System.Drawing.Point(712, 56);
            this.btnZone1.Name = "btnZone1";
            this.btnZone1.Size = new System.Drawing.Size(59, 25);
            this.btnZone1.TabIndex = 496;
            this.btnZone1.Text = "1";
            this.btnZone1.UseVisualStyleBackColor = false;
            this.btnZone1.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone2
            // 
            this.btnZone2.BackColor = System.Drawing.Color.Silver;
            this.btnZone2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone2.Enabled = false;
            this.btnZone2.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone2.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone2.Location = new System.Drawing.Point(712, 82);
            this.btnZone2.Name = "btnZone2";
            this.btnZone2.Size = new System.Drawing.Size(59, 25);
            this.btnZone2.TabIndex = 497;
            this.btnZone2.Text = "2";
            this.btnZone2.UseVisualStyleBackColor = false;
            this.btnZone2.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone3
            // 
            this.btnZone3.BackColor = System.Drawing.Color.Silver;
            this.btnZone3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone3.Enabled = false;
            this.btnZone3.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone3.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone3.Location = new System.Drawing.Point(712, 108);
            this.btnZone3.Name = "btnZone3";
            this.btnZone3.Size = new System.Drawing.Size(59, 25);
            this.btnZone3.TabIndex = 498;
            this.btnZone3.Text = "3";
            this.btnZone3.UseVisualStyleBackColor = false;
            this.btnZone3.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone4
            // 
            this.btnZone4.BackColor = System.Drawing.Color.Silver;
            this.btnZone4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone4.Enabled = false;
            this.btnZone4.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone4.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone4.Location = new System.Drawing.Point(712, 134);
            this.btnZone4.Name = "btnZone4";
            this.btnZone4.Size = new System.Drawing.Size(59, 25);
            this.btnZone4.TabIndex = 499;
            this.btnZone4.Text = "4";
            this.btnZone4.UseVisualStyleBackColor = false;
            this.btnZone4.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone5
            // 
            this.btnZone5.BackColor = System.Drawing.Color.Silver;
            this.btnZone5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone5.Enabled = false;
            this.btnZone5.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone5.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone5.Location = new System.Drawing.Point(712, 160);
            this.btnZone5.Name = "btnZone5";
            this.btnZone5.Size = new System.Drawing.Size(59, 25);
            this.btnZone5.TabIndex = 500;
            this.btnZone5.Text = "5";
            this.btnZone5.UseVisualStyleBackColor = false;
            this.btnZone5.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone6
            // 
            this.btnZone6.BackColor = System.Drawing.Color.Silver;
            this.btnZone6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone6.Enabled = false;
            this.btnZone6.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone6.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone6.Location = new System.Drawing.Point(712, 186);
            this.btnZone6.Name = "btnZone6";
            this.btnZone6.Size = new System.Drawing.Size(59, 25);
            this.btnZone6.TabIndex = 501;
            this.btnZone6.Text = "6";
            this.btnZone6.UseVisualStyleBackColor = false;
            this.btnZone6.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone7
            // 
            this.btnZone7.BackColor = System.Drawing.Color.Silver;
            this.btnZone7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone7.Enabled = false;
            this.btnZone7.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone7.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone7.Location = new System.Drawing.Point(712, 212);
            this.btnZone7.Name = "btnZone7";
            this.btnZone7.Size = new System.Drawing.Size(59, 25);
            this.btnZone7.TabIndex = 503;
            this.btnZone7.Text = "7";
            this.btnZone7.UseVisualStyleBackColor = false;
            this.btnZone7.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // btnZone8
            // 
            this.btnZone8.BackColor = System.Drawing.Color.Silver;
            this.btnZone8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnZone8.Enabled = false;
            this.btnZone8.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnZone8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnZone8.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnZone8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnZone8.Location = new System.Drawing.Point(712, 238);
            this.btnZone8.Name = "btnZone8";
            this.btnZone8.Size = new System.Drawing.Size(59, 25);
            this.btnZone8.TabIndex = 504;
            this.btnZone8.Text = "8";
            this.btnZone8.UseVisualStyleBackColor = false;
            this.btnZone8.Click += new System.EventHandler(this.btnZoneX_Click);
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.panelLeft.ColumnCount = 1;
            this.panelLeft.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.panelLeft.Controls.Add(this.statusStrip2, 0, 3);
            this.panelLeft.Controls.Add(this.btnNavigationSettings, 0, 1);
            this.panelLeft.Controls.Add(this.btnAutoSteerConfig, 0, 6);
            this.panelLeft.Controls.Add(this.btnStartAgIO, 0, 7);
            this.panelLeft.Controls.Add(this.btnJobMenu, 0, 4);
            this.panelLeft.Controls.Add(this.statusStripLeft, 0, 2);
            this.panelLeft.Controls.Add(this.statusStrip1, 0, 5);
            this.panelLeft.Location = new System.Drawing.Point(3, 50);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.RowCount = 8;
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.40268F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.63087F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.91946F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.9396F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.10068F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.75167F));
            this.panelLeft.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.25503F));
            this.panelLeft.Size = new System.Drawing.Size(72, 600);
            this.panelLeft.TabIndex = 529;
            // 
            // statusStrip2
            // 
            this.statusStrip2.AllowMerge = false;
            this.statusStrip2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusStrip2.AutoSize = false;
            this.statusStrip2.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip2.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip2.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip2.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.statusStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1});
            this.statusStrip2.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip2.Location = new System.Drawing.Point(2, 181);
            this.statusStrip2.Name = "statusStrip2";
            this.statusStrip2.Size = new System.Drawing.Size(67, 74);
            this.statusStrip2.SizingGrip = false;
            this.statusStrip2.TabIndex = 544;
            this.statusStrip2.Text = "UDP";
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoSize = false;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripConfig,
            this.toolStripSteerSettings,
            this.toolStripAllSettings,
            this.toolStripWorkingDirectories,
            this.toolStripGPSData,
            this.toolStripColors,
            this.toolStripSectionColors,
            this.toolStripHotkeys});
            this.toolStripDropDownButton1.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripDropDownButton1.Image = global::AgOpenGPS.Properties.Resources.Settings48;
            this.toolStripDropDownButton1.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.ShowDropDownArrow = false;
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(66, 80);
            this.toolStripDropDownButton1.Text = "All";
            // 
            // toolStripConfig
            // 
            this.toolStripConfig.Image = global::AgOpenGPS.Properties.Resources.Settings48;
            this.toolStripConfig.Name = "toolStripConfig";
            this.toolStripConfig.Size = new System.Drawing.Size(419, 44);
            this.toolStripConfig.Text = "Configuration";
            this.toolStripConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // toolStripSteerSettings
            // 
            this.toolStripSteerSettings.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOff;
            this.toolStripSteerSettings.Name = "toolStripSteerSettings";
            this.toolStripSteerSettings.Size = new System.Drawing.Size(419, 44);
            this.toolStripSteerSettings.Text = "Auto Steer";
            this.toolStripSteerSettings.Click += new System.EventHandler(this.btnAutoSteerConfig_Click);
            // 
            // toolStripAllSettings
            // 
            this.toolStripAllSettings.Image = global::AgOpenGPS.Properties.Resources.ScreenShot;
            this.toolStripAllSettings.Name = "toolStripAllSettings";
            this.toolStripAllSettings.Size = new System.Drawing.Size(419, 44);
            this.toolStripAllSettings.Text = "View All Settings";
            this.toolStripAllSettings.Click += new System.EventHandler(this.allSettingsMenuItem_Click);
            // 
            // toolStripWorkingDirectories
            // 
            this.toolStripWorkingDirectories.Image = global::AgOpenGPS.Properties.Resources.FileOpen;
            this.toolStripWorkingDirectories.Name = "toolStripWorkingDirectories";
            this.toolStripWorkingDirectories.Size = new System.Drawing.Size(419, 44);
            this.toolStripWorkingDirectories.Text = "Directories";
            this.toolStripWorkingDirectories.Click += new System.EventHandler(this.setWorkingDirectoryToolStripMenuItem_Click);
            // 
            // toolStripGPSData
            // 
            this.toolStripGPSData.Image = global::AgOpenGPS.Properties.Resources.GPSQuality;
            this.toolStripGPSData.Name = "toolStripGPSData";
            this.toolStripGPSData.Size = new System.Drawing.Size(419, 44);
            this.toolStripGPSData.Text = "GPS Data";
            this.toolStripGPSData.Click += new System.EventHandler(this.btnGPSData_Click);
            // 
            // toolStripColors
            // 
            this.toolStripColors.Image = global::AgOpenGPS.Properties.Resources.ColourPick;
            this.toolStripColors.Name = "toolStripColors";
            this.toolStripColors.Size = new System.Drawing.Size(419, 44);
            this.toolStripColors.Text = "Colors";
            this.toolStripColors.Click += new System.EventHandler(this.colorsToolStripMenuItem_Click);
            // 
            // toolStripSectionColors
            // 
            this.toolStripSectionColors.Image = global::AgOpenGPS.Properties.Resources.SectionMapping;
            this.toolStripSectionColors.Name = "toolStripSectionColors";
            this.toolStripSectionColors.Size = new System.Drawing.Size(419, 44);
            this.toolStripSectionColors.Text = "Multi-Section Colors";
            this.toolStripSectionColors.Click += new System.EventHandler(this.colorsSectionToolStripMenuItem_Click);
            // 
            // toolStripHotkeys
            // 
            this.toolStripHotkeys.Image = global::AgOpenGPS.Properties.Resources.ConD_KeyBoard;
            this.toolStripHotkeys.Name = "toolStripHotkeys";
            this.toolStripHotkeys.Size = new System.Drawing.Size(419, 44);
            this.toolStripHotkeys.Text = "HotKeys";
            this.toolStripHotkeys.Click += new System.EventHandler(this.hotKeysToolStripMenuItem_Click);
            // 
            // btnNavigationSettings
            // 
            this.btnNavigationSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnNavigationSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnNavigationSettings.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnNavigationSettings.FlatAppearance.BorderSize = 0;
            this.btnNavigationSettings.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNavigationSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNavigationSettings.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNavigationSettings.ForeColor = System.Drawing.Color.DarkGray;
            this.btnNavigationSettings.Image = global::AgOpenGPS.Properties.Resources.NavigationSettings;
            this.btnNavigationSettings.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNavigationSettings.Location = new System.Drawing.Point(0, 1);
            this.btnNavigationSettings.Margin = new System.Windows.Forms.Padding(0);
            this.btnNavigationSettings.Name = "btnNavigationSettings";
            this.btnNavigationSettings.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNavigationSettings.Size = new System.Drawing.Size(72, 62);
            this.btnNavigationSettings.TabIndex = 542;
            this.btnNavigationSettings.UseVisualStyleBackColor = false;
            this.btnNavigationSettings.Click += new System.EventHandler(this.btnNavigationSettings_Click);
            // 
            // btnAutoSteerConfig
            // 
            this.btnAutoSteerConfig.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnAutoSteerConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnAutoSteerConfig.FlatAppearance.BorderSize = 0;
            this.btnAutoSteerConfig.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteerConfig.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSteerConfig.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoSteerConfig.ForeColor = System.Drawing.Color.Black;
            this.btnAutoSteerConfig.Image = global::AgOpenGPS.Properties.Resources.AutoSteerConf;
            this.btnAutoSteerConfig.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoSteerConfig.Location = new System.Drawing.Point(0, 442);
            this.btnAutoSteerConfig.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteerConfig.Name = "btnAutoSteerConfig";
            this.btnAutoSteerConfig.Padding = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.btnAutoSteerConfig.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAutoSteerConfig.Size = new System.Drawing.Size(72, 76);
            this.btnAutoSteerConfig.TabIndex = 475;
            this.btnAutoSteerConfig.Text = "-38.8.";
            this.btnAutoSteerConfig.UseVisualStyleBackColor = false;
            this.btnAutoSteerConfig.Click += new System.EventHandler(this.btnAutoSteerConfig_Click);
            // 
            // btnStartAgIO
            // 
            this.btnStartAgIO.BackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnStartAgIO.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnStartAgIO.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnStartAgIO.FlatAppearance.BorderSize = 0;
            this.btnStartAgIO.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnStartAgIO.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartAgIO.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartAgIO.ForeColor = System.Drawing.Color.DarkGray;
            this.btnStartAgIO.Image = global::AgOpenGPS.Properties.Resources.AgIO;
            this.btnStartAgIO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartAgIO.Location = new System.Drawing.Point(0, 518);
            this.btnStartAgIO.Margin = new System.Windows.Forms.Padding(0);
            this.btnStartAgIO.Name = "btnStartAgIO";
            this.btnStartAgIO.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnStartAgIO.Size = new System.Drawing.Size(72, 82);
            this.btnStartAgIO.TabIndex = 467;
            this.btnStartAgIO.UseVisualStyleBackColor = false;
            this.btnStartAgIO.Click += new System.EventHandler(this.btnStartAgIO_Click);
            // 
            // btnJobMenu
            // 
            this.btnJobMenu.BackColor = System.Drawing.Color.Transparent;
            this.btnJobMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnJobMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnJobMenu.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnJobMenu.FlatAppearance.BorderSize = 0;
            this.btnJobMenu.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnJobMenu.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnJobMenu.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnJobMenu.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnJobMenu.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnJobMenu.ForeColor = System.Drawing.Color.DarkGray;
            this.btnJobMenu.Image = global::AgOpenGPS.Properties.Resources.JobActive;
            this.btnJobMenu.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnJobMenu.Location = new System.Drawing.Point(0, 257);
            this.btnJobMenu.Margin = new System.Windows.Forms.Padding(0);
            this.btnJobMenu.Name = "btnJobMenu";
            this.btnJobMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnJobMenu.Size = new System.Drawing.Size(72, 95);
            this.btnJobMenu.TabIndex = 536;
            this.btnJobMenu.UseVisualStyleBackColor = false;
            this.btnJobMenu.Click += new System.EventHandler(this.btnJobMenu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.AllowMerge = false;
            this.statusStrip1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.Transparent;
            this.statusStrip1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.statusStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.statusStrip1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(60, 60);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripBtnFieldTools});
            this.statusStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.statusStrip1.Location = new System.Drawing.Point(2, 355);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(67, 84);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 543;
            this.statusStrip1.Text = "UDP";
            // 
            // toolStripBtnFieldTools
            // 
            this.toolStripBtnFieldTools.AutoSize = false;
            this.toolStripBtnFieldTools.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.toolStripBtnFieldTools.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripBtnFieldTools.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.boundariesToolStripMenuItem,
            this.headlandToolStripMenuItem,
            this.headlandBuildToolStripMenuItem,
            this.tramLinesMenuField,
            this.tramsMultiMenuField,
            this.deleteAppliedToolStripMenuItem,
            this.flagByLatLonToolStripMenuItem,
            this.recordedPathStripMenu});
            this.toolStripBtnFieldTools.Enabled = false;
            this.toolStripBtnFieldTools.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripBtnFieldTools.Image = global::AgOpenGPS.Properties.Resources.FieldTools;
            this.toolStripBtnFieldTools.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripBtnFieldTools.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripBtnFieldTools.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripBtnFieldTools.Name = "toolStripBtnFieldTools";
            this.toolStripBtnFieldTools.ShowDropDownArrow = false;
            this.toolStripBtnFieldTools.Size = new System.Drawing.Size(65, 80);
            // 
            // boundariesToolStripMenuItem
            // 
            this.boundariesToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.boundariesToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.Boundary;
            this.boundariesToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.boundariesToolStripMenuItem.Name = "boundariesToolStripMenuItem";
            this.boundariesToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.boundariesToolStripMenuItem.Text = "Boundary";
            this.boundariesToolStripMenuItem.Click += new System.EventHandler(this.boundariesToolStripMenuItem_Click);
            // 
            // headlandToolStripMenuItem
            // 
            this.headlandToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headlandToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.HeadlandBuild;
            this.headlandToolStripMenuItem.Name = "headlandToolStripMenuItem";
            this.headlandToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.headlandToolStripMenuItem.Text = "Headland";
            this.headlandToolStripMenuItem.Click += new System.EventHandler(this.headlandToolStripMenuItem_Click);
            // 
            // headlandBuildToolStripMenuItem
            // 
            this.headlandBuildToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.headlandBuildToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.Headache;
            this.headlandBuildToolStripMenuItem.Name = "headlandBuildToolStripMenuItem";
            this.headlandBuildToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.headlandBuildToolStripMenuItem.Text = "Headland (Build)";
            this.headlandBuildToolStripMenuItem.Click += new System.EventHandler(this.headlandBuildToolStripMenuItem_Click);
            // 
            // tramLinesMenuField
            // 
            this.tramLinesMenuField.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tramLinesMenuField.Image = global::AgOpenGPS.Properties.Resources.TramAll;
            this.tramLinesMenuField.Name = "tramLinesMenuField";
            this.tramLinesMenuField.Size = new System.Drawing.Size(445, 70);
            this.tramLinesMenuField.Text = "TramLines";
            this.tramLinesMenuField.Click += new System.EventHandler(this.tramLinesMenuField_Click);
            // 
            // tramsMultiMenuField
            // 
            this.tramsMultiMenuField.Image = global::AgOpenGPS.Properties.Resources.TramMulti;
            this.tramsMultiMenuField.Name = "tramsMultiMenuField";
            this.tramsMultiMenuField.Size = new System.Drawing.Size(445, 70);
            this.tramsMultiMenuField.Text = "Tramlines";
            this.tramsMultiMenuField.Click += new System.EventHandler(this.tramLinesMenuMulti_Click);
            // 
            // deleteAppliedToolStripMenuItem
            // 
            this.deleteAppliedToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteAppliedToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.TrashApplied;
            this.deleteAppliedToolStripMenuItem.Name = "deleteAppliedToolStripMenuItem";
            this.deleteAppliedToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.deleteAppliedToolStripMenuItem.Text = "Delete Applied";
            this.deleteAppliedToolStripMenuItem.Click += new System.EventHandler(this.toolStripAreYouSure_Click);
            // 
            // flagByLatLonToolStripMenuItem
            // 
            this.flagByLatLonToolStripMenuItem.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flagByLatLonToolStripMenuItem.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.flagByLatLonToolStripMenuItem.Name = "flagByLatLonToolStripMenuItem";
            this.flagByLatLonToolStripMenuItem.Size = new System.Drawing.Size(445, 70);
            this.flagByLatLonToolStripMenuItem.Text = "Flag By Lat Lon";
            this.flagByLatLonToolStripMenuItem.Click += new System.EventHandler(this.flagByLatLonToolStripMenuItem_Click);
            // 
            // recordedPathStripMenu
            // 
            this.recordedPathStripMenu.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.recordedPathStripMenu.Image = global::AgOpenGPS.Properties.Resources.RecPath;
            this.recordedPathStripMenu.Name = "recordedPathStripMenu";
            this.recordedPathStripMenu.Size = new System.Drawing.Size(445, 70);
            this.recordedPathStripMenu.Text = "Recorded Path";
            this.recordedPathStripMenu.Click += new System.EventHandler(this.recordedPathStripMenu_Click);
            // 
            // lblCurrentField
            // 
            this.lblCurrentField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCurrentField.AutoSize = true;
            this.lblCurrentField.BackColor = System.Drawing.Color.Transparent;
            this.lblCurrentField.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentField.Location = new System.Drawing.Point(82, 25);
            this.lblCurrentField.Name = "lblCurrentField";
            this.lblCurrentField.Size = new System.Drawing.Size(140, 24);
            this.lblCurrentField.TabIndex = 534;
            this.lblCurrentField.Text = "Current Field";
            this.lblCurrentField.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblCurrentField.Click += new System.EventHandler(this.lblCurrentField_Click);
            // 
            // lblGuidanceLine
            // 
            this.lblGuidanceLine.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblGuidanceLine.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblGuidanceLine.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGuidanceLine.Location = new System.Drawing.Point(211, 66);
            this.lblGuidanceLine.Name = "lblGuidanceLine";
            this.lblGuidanceLine.Size = new System.Drawing.Size(583, 83);
            this.lblGuidanceLine.TabIndex = 538;
            this.lblGuidanceLine.Text = "This is the line description";
            this.lblGuidanceLine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblGuidanceLine.Visible = false;
            // 
            // flp1
            // 
            this.flp1.AutoSize = true;
            this.flp1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flp1.Controls.Add(this.btnNudge);
            this.flp1.Controls.Add(this.btnABDraw);
            this.flp1.Controls.Add(this.btnPlusAB);
            this.flp1.Controls.Add(this.btnBuildTracks);
            this.flp1.Controls.Add(this.btnTracksOff);
            this.flp1.Controls.Add(this.cboxAutoSnapToPivot);
            this.flp1.Controls.Add(this.btnRefNudge);
            this.flp1.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.flp1.Location = new System.Drawing.Point(800, 48);
            this.flp1.Name = "flp1";
            this.flp1.Size = new System.Drawing.Size(86, 571);
            this.flp1.TabIndex = 539;
            // 
            // btnNudge
            // 
            this.btnNudge.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnNudge.BackColor = System.Drawing.Color.Transparent;
            this.btnNudge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNudge.FlatAppearance.BorderSize = 0;
            this.btnNudge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnNudge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnNudge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnNudge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNudge.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNudge.Image = global::AgOpenGPS.Properties.Resources.ABSnapNudgeMenu;
            this.btnNudge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNudge.Location = new System.Drawing.Point(3, 496);
            this.btnNudge.Name = "btnNudge";
            this.btnNudge.Size = new System.Drawing.Size(80, 72);
            this.btnNudge.TabIndex = 489;
            this.btnNudge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNudge.UseVisualStyleBackColor = false;
            this.btnNudge.Click += new System.EventHandler(this.btnNudge_Click);
            // 
            // btnABDraw
            // 
            this.btnABDraw.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnABDraw.BackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnABDraw.Enabled = false;
            this.btnABDraw.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnABDraw.FlatAppearance.BorderSize = 0;
            this.btnABDraw.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnABDraw.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnABDraw.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnABDraw.Image = global::AgOpenGPS.Properties.Resources.ABDraw;
            this.btnABDraw.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnABDraw.Location = new System.Drawing.Point(3, 418);
            this.btnABDraw.Name = "btnABDraw";
            this.btnABDraw.Size = new System.Drawing.Size(80, 72);
            this.btnABDraw.TabIndex = 250;
            this.btnABDraw.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnABDraw.UseVisualStyleBackColor = false;
            this.btnABDraw.Click += new System.EventHandler(this.btnABDraw_Click);
            // 
            // btnPlusAB
            // 
            this.btnPlusAB.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPlusAB.BackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnPlusAB.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnPlusAB.FlatAppearance.BorderSize = 0;
            this.btnPlusAB.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnPlusAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlusAB.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnPlusAB.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.btnPlusAB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPlusAB.Location = new System.Drawing.Point(3, 335);
            this.btnPlusAB.Name = "btnPlusAB";
            this.btnPlusAB.Size = new System.Drawing.Size(80, 77);
            this.btnPlusAB.TabIndex = 545;
            this.btnPlusAB.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPlusAB.UseVisualStyleBackColor = false;
            this.btnPlusAB.Click += new System.EventHandler(this.btnPlusAB_Click);
            // 
            // btnBuildTracks
            // 
            this.btnBuildTracks.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBuildTracks.BackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnBuildTracks.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnBuildTracks.FlatAppearance.BorderSize = 0;
            this.btnBuildTracks.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnBuildTracks.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBuildTracks.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnBuildTracks.Image = global::AgOpenGPS.Properties.Resources.ABTracks;
            this.btnBuildTracks.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBuildTracks.Location = new System.Drawing.Point(3, 252);
            this.btnBuildTracks.Name = "btnBuildTracks";
            this.btnBuildTracks.Size = new System.Drawing.Size(80, 77);
            this.btnBuildTracks.TabIndex = 539;
            this.btnBuildTracks.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnBuildTracks.UseVisualStyleBackColor = false;
            this.btnBuildTracks.Click += new System.EventHandler(this.btnBuildTracks_Click);
            // 
            // btnTracksOff
            // 
            this.btnTracksOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTracksOff.BackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnTracksOff.FlatAppearance.BorderSize = 0;
            this.btnTracksOff.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTracksOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTracksOff.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTracksOff.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnTracksOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTracksOff.Location = new System.Drawing.Point(3, 169);
            this.btnTracksOff.Name = "btnTracksOff";
            this.btnTracksOff.Size = new System.Drawing.Size(80, 77);
            this.btnTracksOff.TabIndex = 539;
            this.btnTracksOff.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnTracksOff.UseVisualStyleBackColor = false;
            this.btnTracksOff.Click += new System.EventHandler(this.btnTracksOff_Click);
            // 
            // cboxAutoSnapToPivot
            // 
            this.cboxAutoSnapToPivot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cboxAutoSnapToPivot.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxAutoSnapToPivot.BackColor = System.Drawing.Color.Transparent;
            this.cboxAutoSnapToPivot.FlatAppearance.BorderColor = System.Drawing.Color.Silver;
            this.cboxAutoSnapToPivot.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxAutoSnapToPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxAutoSnapToPivot.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.cboxAutoSnapToPivot.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxAutoSnapToPivot.Image = global::AgOpenGPS.Properties.Resources.AutoSteerSnapToPivot;
            this.cboxAutoSnapToPivot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cboxAutoSnapToPivot.Location = new System.Drawing.Point(3, 86);
            this.cboxAutoSnapToPivot.Name = "cboxAutoSnapToPivot";
            this.cboxAutoSnapToPivot.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxAutoSnapToPivot.Size = new System.Drawing.Size(80, 77);
            this.cboxAutoSnapToPivot.TabIndex = 468;
            this.cboxAutoSnapToPivot.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.cboxAutoSnapToPivot.UseVisualStyleBackColor = false;
            this.cboxAutoSnapToPivot.Click += new System.EventHandler(this.cboxAutoSnapToPivot_Click);
            // 
            // btnRefNudge
            // 
            this.btnRefNudge.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnRefNudge.BackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnRefNudge.FlatAppearance.BorderSize = 0;
            this.btnRefNudge.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnRefNudge.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefNudge.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefNudge.Image = global::AgOpenGPS.Properties.Resources.ABSnapNudgeMenuRef;
            this.btnRefNudge.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRefNudge.Location = new System.Drawing.Point(3, 3);
            this.btnRefNudge.Name = "btnRefNudge";
            this.btnRefNudge.Size = new System.Drawing.Size(80, 77);
            this.btnRefNudge.TabIndex = 538;
            this.btnRefNudge.Text = "Ref";
            this.btnRefNudge.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefNudge.UseVisualStyleBackColor = false;
            this.btnRefNudge.Click += new System.EventHandler(this.btnRefNudge_Click);
            // 
            // panelBottom
            // 
            this.panelBottom.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelBottom.AutoSize = true;
            this.panelBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelBottom.Controls.Add(this.btnTrack);
            this.panelBottom.Controls.Add(this.btnSnapToPivot);
            this.panelBottom.Controls.Add(this.btnAdjRight);
            this.panelBottom.Controls.Add(this.btnAdjLeft);
            this.panelBottom.Controls.Add(this.btnFlag);
            this.panelBottom.Controls.Add(this.btnHeadlandOnOff);
            this.panelBottom.Controls.Add(this.cboxIsSectionControlled);
            this.panelBottom.Controls.Add(this.btnHydLift);
            this.panelBottom.Controls.Add(this.btnTramDisplayMode);
            this.panelBottom.Controls.Add(this.btnResetToolHeading);
            this.panelBottom.Controls.Add(this.btnChangeMappingColor);
            this.panelBottom.Controls.Add(this.btnYouSkipEnable);
            this.panelBottom.Controls.Add(this.cboxpRowWidth);
            this.panelBottom.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.panelBottom.Location = new System.Drawing.Point(-54, 652);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(974, 62);
            this.panelBottom.TabIndex = 540;
            // 
            // btnTrack
            // 
            this.btnTrack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTrack.BackColor = System.Drawing.Color.Transparent;
            this.btnTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnTrack.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnTrack.FlatAppearance.BorderSize = 0;
            this.btnTrack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTrack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTrack.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrack.Image = global::AgOpenGPS.Properties.Resources.TrackOn;
            this.btnTrack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTrack.Location = new System.Drawing.Point(902, 3);
            this.btnTrack.Margin = new System.Windows.Forms.Padding(0);
            this.btnTrack.Name = "btnTrack";
            this.btnTrack.Size = new System.Drawing.Size(72, 56);
            this.btnTrack.TabIndex = 173;
            this.btnTrack.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnTrack.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnTrack.UseVisualStyleBackColor = false;
            this.btnTrack.Click += new System.EventHandler(this.btnTrack_Click);
            // 
            // btnSnapToPivot
            // 
            this.btnSnapToPivot.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSnapToPivot.BackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSnapToPivot.FlatAppearance.BorderSize = 0;
            this.btnSnapToPivot.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSnapToPivot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSnapToPivot.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSnapToPivot.Image = global::AgOpenGPS.Properties.Resources.SnapToPivot;
            this.btnSnapToPivot.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSnapToPivot.Location = new System.Drawing.Point(827, 3);
            this.btnSnapToPivot.Name = "btnSnapToPivot";
            this.btnSnapToPivot.Size = new System.Drawing.Size(72, 56);
            this.btnSnapToPivot.TabIndex = 544;
            this.btnSnapToPivot.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSnapToPivot.UseVisualStyleBackColor = false;
            this.btnSnapToPivot.Click += new System.EventHandler(this.btnSnapToPivot_Click);
            // 
            // btnAdjRight
            // 
            this.btnAdjRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdjRight.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjRight.FlatAppearance.BorderSize = 0;
            this.btnAdjRight.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdjRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjRight.Image = global::AgOpenGPS.Properties.Resources.SnapRight;
            this.btnAdjRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjRight.Location = new System.Drawing.Point(749, 3);
            this.btnAdjRight.Name = "btnAdjRight";
            this.btnAdjRight.Size = new System.Drawing.Size(72, 56);
            this.btnAdjRight.TabIndex = 546;
            this.btnAdjRight.UseVisualStyleBackColor = false;
            this.btnAdjRight.Click += new System.EventHandler(this.btnAdjRight_Click);
            // 
            // btnAdjLeft
            // 
            this.btnAdjLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAdjLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjLeft.FlatAppearance.BorderSize = 0;
            this.btnAdjLeft.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAdjLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjLeft.Image = global::AgOpenGPS.Properties.Resources.SnapLeft;
            this.btnAdjLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjLeft.Location = new System.Drawing.Point(671, 3);
            this.btnAdjLeft.Name = "btnAdjLeft";
            this.btnAdjLeft.Size = new System.Drawing.Size(72, 56);
            this.btnAdjLeft.TabIndex = 545;
            this.btnAdjLeft.UseVisualStyleBackColor = false;
            this.btnAdjLeft.Click += new System.EventHandler(this.btnAdjLeft_Click);
            // 
            // btnFlag
            // 
            this.btnFlag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFlag.BackColor = System.Drawing.Color.Transparent;
            this.btnFlag.ContextMenuStrip = this.contextMenuStripFlag;
            this.btnFlag.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnFlag.FlatAppearance.BorderSize = 0;
            this.btnFlag.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFlag.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFlag.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFlag.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFlag.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFlag.Image = global::AgOpenGPS.Properties.Resources.FlagRed;
            this.btnFlag.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFlag.Location = new System.Drawing.Point(593, 3);
            this.btnFlag.Name = "btnFlag";
            this.btnFlag.Size = new System.Drawing.Size(72, 56);
            this.btnFlag.TabIndex = 121;
            this.btnFlag.Text = "P";
            this.btnFlag.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnFlag.UseVisualStyleBackColor = false;
            this.btnFlag.Click += new System.EventHandler(this.btnFlag_Click);
            // 
            // btnHeadlandOnOff
            // 
            this.btnHeadlandOnOff.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHeadlandOnOff.BackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnHeadlandOnOff.FlatAppearance.BorderSize = 0;
            this.btnHeadlandOnOff.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHeadlandOnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHeadlandOnOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHeadlandOnOff.Image = global::AgOpenGPS.Properties.Resources.HeadlandOff;
            this.btnHeadlandOnOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHeadlandOnOff.Location = new System.Drawing.Point(515, 3);
            this.btnHeadlandOnOff.Name = "btnHeadlandOnOff";
            this.btnHeadlandOnOff.Size = new System.Drawing.Size(72, 56);
            this.btnHeadlandOnOff.TabIndex = 447;
            this.btnHeadlandOnOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnHeadlandOnOff.UseVisualStyleBackColor = false;
            this.btnHeadlandOnOff.Visible = false;
            this.btnHeadlandOnOff.Click += new System.EventHandler(this.btnHeadlandOnOff_Click);
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
            this.cboxIsSectionControlled.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.cboxIsSectionControlled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsSectionControlled.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsSectionControlled.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.cboxIsSectionControlled.Image = global::AgOpenGPS.Properties.Resources.HeadlandSectionOn;
            this.cboxIsSectionControlled.Location = new System.Drawing.Point(437, 3);
            this.cboxIsSectionControlled.Name = "cboxIsSectionControlled";
            this.cboxIsSectionControlled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsSectionControlled.Size = new System.Drawing.Size(72, 56);
            this.cboxIsSectionControlled.TabIndex = 544;
            this.cboxIsSectionControlled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsSectionControlled.UseVisualStyleBackColor = false;
            this.cboxIsSectionControlled.Click += new System.EventHandler(this.cboxIsSectionControlled_Click);
            // 
            // btnHydLift
            // 
            this.btnHydLift.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnHydLift.BackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnHydLift.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnHydLift.FlatAppearance.BorderSize = 0;
            this.btnHydLift.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnHydLift.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnHydLift.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnHydLift.Image = global::AgOpenGPS.Properties.Resources.HydraulicLiftOff;
            this.btnHydLift.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnHydLift.Location = new System.Drawing.Point(359, 3);
            this.btnHydLift.Name = "btnHydLift";
            this.btnHydLift.Size = new System.Drawing.Size(72, 56);
            this.btnHydLift.TabIndex = 453;
            this.btnHydLift.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnHydLift.UseVisualStyleBackColor = false;
            this.btnHydLift.Visible = false;
            this.btnHydLift.Click += new System.EventHandler(this.btnHydLift_Click);
            // 
            // btnTramDisplayMode
            // 
            this.btnTramDisplayMode.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTramDisplayMode.BackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnTramDisplayMode.FlatAppearance.BorderSize = 0;
            this.btnTramDisplayMode.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnTramDisplayMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTramDisplayMode.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTramDisplayMode.Image = global::AgOpenGPS.Properties.Resources.TramOff;
            this.btnTramDisplayMode.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTramDisplayMode.Location = new System.Drawing.Point(281, 3);
            this.btnTramDisplayMode.Name = "btnTramDisplayMode";
            this.btnTramDisplayMode.Size = new System.Drawing.Size(72, 56);
            this.btnTramDisplayMode.TabIndex = 480;
            this.btnTramDisplayMode.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTramDisplayMode.UseVisualStyleBackColor = false;
            this.btnTramDisplayMode.Click += new System.EventHandler(this.btnTramDisplayMode_Click);
            // 
            // btnResetToolHeading
            // 
            this.btnResetToolHeading.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnResetToolHeading.BackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnResetToolHeading.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnResetToolHeading.FlatAppearance.BorderSize = 0;
            this.btnResetToolHeading.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnResetToolHeading.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnResetToolHeading.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnResetToolHeading.ForeColor = System.Drawing.Color.Black;
            this.btnResetToolHeading.Image = global::AgOpenGPS.Properties.Resources.ResetTool;
            this.btnResetToolHeading.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnResetToolHeading.Location = new System.Drawing.Point(206, 3);
            this.btnResetToolHeading.Margin = new System.Windows.Forms.Padding(0);
            this.btnResetToolHeading.Name = "btnResetToolHeading";
            this.btnResetToolHeading.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnResetToolHeading.Size = new System.Drawing.Size(72, 56);
            this.btnResetToolHeading.TabIndex = 491;
            this.btnResetToolHeading.UseVisualStyleBackColor = false;
            this.btnResetToolHeading.Click += new System.EventHandler(this.btnResetToolHeading_Click);
            // 
            // btnChangeMappingColor
            // 
            this.btnChangeMappingColor.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnChangeMappingColor.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChangeMappingColor.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnChangeMappingColor.FlatAppearance.BorderSize = 0;
            this.btnChangeMappingColor.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChangeMappingColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeMappingColor.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangeMappingColor.ForeColor = System.Drawing.Color.Black;
            this.btnChangeMappingColor.Image = global::AgOpenGPS.Properties.Resources.SectionMapping;
            this.btnChangeMappingColor.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeMappingColor.Location = new System.Drawing.Point(134, 3);
            this.btnChangeMappingColor.Margin = new System.Windows.Forms.Padding(0);
            this.btnChangeMappingColor.Name = "btnChangeMappingColor";
            this.btnChangeMappingColor.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnChangeMappingColor.Size = new System.Drawing.Size(72, 56);
            this.btnChangeMappingColor.TabIndex = 476;
            this.btnChangeMappingColor.Text = "5.8.4";
            this.btnChangeMappingColor.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnChangeMappingColor.UseVisualStyleBackColor = false;
            this.btnChangeMappingColor.Click += new System.EventHandler(this.btnChangeMappingColor_Click);
            // 
            // btnYouSkipEnable
            // 
            this.btnYouSkipEnable.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnYouSkipEnable.BackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnYouSkipEnable.FlatAppearance.BorderSize = 0;
            this.btnYouSkipEnable.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnYouSkipEnable.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnYouSkipEnable.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnYouSkipEnable.Image = global::AgOpenGPS.Properties.Resources.YouSkipOff;
            this.btnYouSkipEnable.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnYouSkipEnable.Location = new System.Drawing.Point(59, 3);
            this.btnYouSkipEnable.Name = "btnYouSkipEnable";
            this.btnYouSkipEnable.Size = new System.Drawing.Size(72, 56);
            this.btnYouSkipEnable.TabIndex = 490;
            this.btnYouSkipEnable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnYouSkipEnable.UseVisualStyleBackColor = false;
            this.btnYouSkipEnable.Click += new System.EventHandler(this.btnYouSkipEnable_Click);
            // 
            // panelRight
            // 
            this.panelRight.AllowDrop = true;
            this.panelRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panelRight.AutoSize = true;
            this.panelRight.Controls.Add(this.btnAutoSteer);
            this.panelRight.Controls.Add(this.btnAutoYouTurn);
            this.panelRight.Controls.Add(this.btnSectionMasterAuto);
            this.panelRight.Controls.Add(this.btnSectionMasterManual);
            this.panelRight.Controls.Add(this.btnAutoTrack);
            this.panelRight.Controls.Add(this.btnCycleLinesBk);
            this.panelRight.Controls.Add(this.btnCycleLines);
            this.panelRight.Controls.Add(this.btnContour);
            this.panelRight.Controls.Add(this.btnContourLock);
            this.panelRight.Controls.Add(this.lblNumCu);
            this.panelRight.FlowDirection = System.Windows.Forms.FlowDirection.BottomUp;
            this.panelRight.Location = new System.Drawing.Point(926, 53);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(70, 650);
            this.panelRight.TabIndex = 541;
            // 
            // btnAutoSteer
            // 
            this.btnAutoSteer.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAutoSteer.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoSteer.FlatAppearance.BorderSize = 0;
            this.btnAutoSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoSteer.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnAutoSteer.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOff;
            this.btnAutoSteer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoSteer.Location = new System.Drawing.Point(3, 586);
            this.btnAutoSteer.Margin = new System.Windows.Forms.Padding(0);
            this.btnAutoSteer.Name = "btnAutoSteer";
            this.btnAutoSteer.Size = new System.Drawing.Size(64, 64);
            this.btnAutoSteer.TabIndex = 128;
            this.btnAutoSteer.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.btnAutoSteer.UseVisualStyleBackColor = false;
            this.btnAutoSteer.Click += new System.EventHandler(this.btnAutoSteer_Click);
            // 
            // btnAutoYouTurn
            // 
            this.btnAutoYouTurn.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAutoYouTurn.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnAutoYouTurn.FlatAppearance.BorderSize = 0;
            this.btnAutoYouTurn.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoYouTurn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoYouTurn.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAutoYouTurn.Image = global::AgOpenGPS.Properties.Resources.YouTurnNo;
            this.btnAutoYouTurn.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoYouTurn.Location = new System.Drawing.Point(3, 519);
            this.btnAutoYouTurn.Name = "btnAutoYouTurn";
            this.btnAutoYouTurn.Size = new System.Drawing.Size(64, 64);
            this.btnAutoYouTurn.TabIndex = 132;
            this.btnAutoYouTurn.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAutoYouTurn.UseVisualStyleBackColor = false;
            this.btnAutoYouTurn.Click += new System.EventHandler(this.btnAutoYouTurn_Click);
            // 
            // btnSectionMasterAuto
            // 
            this.btnSectionMasterAuto.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSectionMasterAuto.BackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSectionMasterAuto.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSectionMasterAuto.FlatAppearance.BorderSize = 0;
            this.btnSectionMasterAuto.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterAuto.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionMasterAuto.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionMasterAuto.Image = global::AgOpenGPS.Properties.Resources.SectionMasterOff;
            this.btnSectionMasterAuto.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSectionMasterAuto.Location = new System.Drawing.Point(3, 452);
            this.btnSectionMasterAuto.Margin = new System.Windows.Forms.Padding(0);
            this.btnSectionMasterAuto.Name = "btnSectionMasterAuto";
            this.btnSectionMasterAuto.Size = new System.Drawing.Size(64, 64);
            this.btnSectionMasterAuto.TabIndex = 152;
            this.btnSectionMasterAuto.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSectionMasterAuto.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSectionMasterAuto.UseVisualStyleBackColor = false;
            this.btnSectionMasterAuto.Click += new System.EventHandler(this.btnSectionMasterAuto_Click);
            // 
            // btnSectionMasterManual
            // 
            this.btnSectionMasterManual.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnSectionMasterManual.BackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSectionMasterManual.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnSectionMasterManual.FlatAppearance.BorderSize = 0;
            this.btnSectionMasterManual.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnSectionMasterManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSectionMasterManual.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSectionMasterManual.Image = global::AgOpenGPS.Properties.Resources.ManualOff;
            this.btnSectionMasterManual.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSectionMasterManual.Location = new System.Drawing.Point(3, 385);
            this.btnSectionMasterManual.Name = "btnSectionMasterManual";
            this.btnSectionMasterManual.Size = new System.Drawing.Size(64, 64);
            this.btnSectionMasterManual.TabIndex = 98;
            this.btnSectionMasterManual.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSectionMasterManual.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.btnSectionMasterManual.UseVisualStyleBackColor = false;
            this.btnSectionMasterManual.Click += new System.EventHandler(this.btnSectionMasterManual_Click);
            // 
            // btnAutoTrack
            // 
            this.btnAutoTrack.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAutoTrack.BackColor = System.Drawing.Color.Transparent;
            this.btnAutoTrack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAutoTrack.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnAutoTrack.FlatAppearance.BorderSize = 0;
            this.btnAutoTrack.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnAutoTrack.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnAutoTrack.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnAutoTrack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAutoTrack.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAutoTrack.Image = global::AgOpenGPS.Properties.Resources.AutoTrackOff;
            this.btnAutoTrack.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAutoTrack.Location = new System.Drawing.Point(3, 315);
            this.btnAutoTrack.Name = "btnAutoTrack";
            this.btnAutoTrack.Size = new System.Drawing.Size(64, 64);
            this.btnAutoTrack.TabIndex = 543;
            this.btnAutoTrack.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAutoTrack.UseVisualStyleBackColor = false;
            this.btnAutoTrack.Click += new System.EventHandler(this.btnAutoTrack_Click);
            // 
            // btnCycleLinesBk
            // 
            this.btnCycleLinesBk.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleLinesBk.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCycleLinesBk.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCycleLinesBk.FlatAppearance.BorderSize = 0;
            this.btnCycleLinesBk.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLinesBk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleLinesBk.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCycleLinesBk.Image = global::AgOpenGPS.Properties.Resources.ABLineCycleBk;
            this.btnCycleLinesBk.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleLinesBk.Location = new System.Drawing.Point(3, 245);
            this.btnCycleLinesBk.Name = "btnCycleLinesBk";
            this.btnCycleLinesBk.Size = new System.Drawing.Size(64, 64);
            this.btnCycleLinesBk.TabIndex = 252;
            this.btnCycleLinesBk.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleLinesBk.UseVisualStyleBackColor = false;
            this.btnCycleLinesBk.Click += new System.EventHandler(this.btnCycleLinesBk_Click);
            // 
            // btnCycleLines
            // 
            this.btnCycleLines.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCycleLines.BackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnCycleLines.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnCycleLines.FlatAppearance.BorderSize = 0;
            this.btnCycleLines.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnCycleLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCycleLines.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCycleLines.Image = global::AgOpenGPS.Properties.Resources.ABLineCycle;
            this.btnCycleLines.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCycleLines.Location = new System.Drawing.Point(3, 175);
            this.btnCycleLines.Name = "btnCycleLines";
            this.btnCycleLines.Size = new System.Drawing.Size(64, 64);
            this.btnCycleLines.TabIndex = 251;
            this.btnCycleLines.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCycleLines.UseVisualStyleBackColor = false;
            this.btnCycleLines.Click += new System.EventHandler(this.btnCycleLines_Click);
            // 
            // btnContour
            // 
            this.btnContour.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnContour.BackColor = System.Drawing.Color.Transparent;
            this.btnContour.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnContour.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnContour.FlatAppearance.BorderSize = 0;
            this.btnContour.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnContour.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnContour.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnContour.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContour.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContour.Image = global::AgOpenGPS.Properties.Resources.ContourOff;
            this.btnContour.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnContour.Location = new System.Drawing.Point(3, 105);
            this.btnContour.Name = "btnContour";
            this.btnContour.Size = new System.Drawing.Size(64, 64);
            this.btnContour.TabIndex = 105;
            this.btnContour.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContour.UseVisualStyleBackColor = false;
            this.btnContour.Click += new System.EventHandler(this.btnContour_Click);
            // 
            // btnContourLock
            // 
            this.btnContourLock.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnContourLock.BackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnContourLock.FlatAppearance.BorderColor = System.Drawing.Color.RoyalBlue;
            this.btnContourLock.FlatAppearance.BorderSize = 0;
            this.btnContourLock.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnContourLock.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContourLock.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnContourLock.Image = global::AgOpenGPS.Properties.Resources.ColorUnlocked;
            this.btnContourLock.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnContourLock.Location = new System.Drawing.Point(3, 35);
            this.btnContourLock.Name = "btnContourLock";
            this.btnContourLock.Size = new System.Drawing.Size(64, 64);
            this.btnContourLock.TabIndex = 542;
            this.btnContourLock.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContourLock.UseVisualStyleBackColor = false;
            this.btnContourLock.Click += new System.EventHandler(this.btnContourLock_Click);
            // 
            // lblNumCu
            // 
            this.lblNumCu.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblNumCu.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNumCu.ForeColor = System.Drawing.Color.Black;
            this.lblNumCu.Location = new System.Drawing.Point(6, 6);
            this.lblNumCu.Margin = new System.Windows.Forms.Padding(0);
            this.lblNumCu.Name = "lblNumCu";
            this.lblNumCu.Size = new System.Drawing.Size(58, 26);
            this.lblNumCu.TabIndex = 542;
            this.lblNumCu.Text = "12/24";
            this.lblNumCu.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelControlBox
            // 
            this.panelControlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControlBox.BackColor = System.Drawing.Color.Transparent;
            this.panelControlBox.Controls.Add(this.btnChargeStatus);
            this.panelControlBox.Controls.Add(this.btnGPSData);
            this.panelControlBox.Controls.Add(this.lblSpeed);
            this.panelControlBox.Controls.Add(this.btnShutdown);
            this.panelControlBox.Controls.Add(this.btnMaximizeMainForm);
            this.panelControlBox.Controls.Add(this.btnMinimizeMainForm);
            this.panelControlBox.Controls.Add(this.btnFieldStats);
            this.panelControlBox.Location = new System.Drawing.Point(508, 4);
            this.panelControlBox.Name = "panelControlBox";
            this.panelControlBox.Size = new System.Drawing.Size(489, 43);
            this.panelControlBox.TabIndex = 542;
            // 
            // btnChargeStatus
            // 
            this.btnChargeStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChargeStatus.BackColor = System.Drawing.Color.Silver;
            this.btnChargeStatus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnChargeStatus.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnChargeStatus.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnChargeStatus.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnChargeStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChargeStatus.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChargeStatus.Image = global::AgOpenGPS.Properties.Resources.ChargeIndicator;
            this.btnChargeStatus.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChargeStatus.Location = new System.Drawing.Point(86, 5);
            this.btnChargeStatus.Name = "btnChargeStatus";
            this.btnChargeStatus.Size = new System.Drawing.Size(25, 32);
            this.btnChargeStatus.TabIndex = 537;
            this.btnChargeStatus.UseVisualStyleBackColor = false;
            // 
            // btnGPSData
            // 
            this.btnGPSData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGPSData.BackColor = System.Drawing.Color.Yellow;
            this.btnGPSData.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnGPSData.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnGPSData.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnGPSData.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnGPSData.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGPSData.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGPSData.Image = global::AgOpenGPS.Properties.Resources.GPSQuality;
            this.btnGPSData.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnGPSData.Location = new System.Drawing.Point(124, 2);
            this.btnGPSData.Name = "btnGPSData";
            this.btnGPSData.Size = new System.Drawing.Size(52, 38);
            this.btnGPSData.TabIndex = 536;
            this.btnGPSData.UseVisualStyleBackColor = false;
            this.btnGPSData.Click += new System.EventHandler(this.btnGPSData_Click);
            // 
            // btnShutdown
            // 
            this.btnShutdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnShutdown.BackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.BackgroundImage = global::AgOpenGPS.Properties.Resources.WindowClose;
            this.btnShutdown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnShutdown.FlatAppearance.BorderSize = 0;
            this.btnShutdown.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnShutdown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShutdown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnShutdown.Location = new System.Drawing.Point(434, 3);
            this.btnShutdown.Name = "btnShutdown";
            this.btnShutdown.Size = new System.Drawing.Size(52, 38);
            this.btnShutdown.TabIndex = 447;
            this.btnShutdown.UseVisualStyleBackColor = false;
            this.btnShutdown.Click += new System.EventHandler(this.btnShutdown_Click);
            // 
            // btnMaximizeMainForm
            // 
            this.btnMaximizeMainForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMaximizeMainForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.BackgroundImage = global::AgOpenGPS.Properties.Resources.WindowMaximize;
            this.btnMaximizeMainForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMaximizeMainForm.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMaximizeMainForm.FlatAppearance.BorderSize = 0;
            this.btnMaximizeMainForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMaximizeMainForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximizeMainForm.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMaximizeMainForm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMaximizeMainForm.Location = new System.Drawing.Point(361, 3);
            this.btnMaximizeMainForm.Name = "btnMaximizeMainForm";
            this.btnMaximizeMainForm.Size = new System.Drawing.Size(52, 38);
            this.btnMaximizeMainForm.TabIndex = 482;
            this.btnMaximizeMainForm.UseVisualStyleBackColor = false;
            this.btnMaximizeMainForm.Click += new System.EventHandler(this.btnMaximizeMainForm_Click);
            // 
            // btnMinimizeMainForm
            // 
            this.btnMinimizeMainForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMinimizeMainForm.BackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.BackgroundImage = global::AgOpenGPS.Properties.Resources.WindowMinimize;
            this.btnMinimizeMainForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnMinimizeMainForm.FlatAppearance.BorderSize = 0;
            this.btnMinimizeMainForm.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnMinimizeMainForm.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimizeMainForm.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMinimizeMainForm.ForeColor = System.Drawing.Color.DimGray;
            this.btnMinimizeMainForm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMinimizeMainForm.Location = new System.Drawing.Point(290, 3);
            this.btnMinimizeMainForm.Name = "btnMinimizeMainForm";
            this.btnMinimizeMainForm.Size = new System.Drawing.Size(52, 38);
            this.btnMinimizeMainForm.TabIndex = 481;
            this.btnMinimizeMainForm.UseVisualStyleBackColor = false;
            this.btnMinimizeMainForm.Click += new System.EventHandler(this.btnMinimizeMainForm_Click);
            // 
            // btnFieldStats
            // 
            this.btnFieldStats.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnFieldStats.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnFieldStats.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFieldStats.FlatAppearance.BorderSize = 0;
            this.btnFieldStats.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.btnFieldStats.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldStats.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFieldStats.Image = global::AgOpenGPS.Properties.Resources.FieldStats;
            this.btnFieldStats.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldStats.Location = new System.Drawing.Point(2, 2);
            this.btnFieldStats.Name = "btnFieldStats";
            this.btnFieldStats.Size = new System.Drawing.Size(75, 38);
            this.btnFieldStats.TabIndex = 535;
            this.btnFieldStats.UseVisualStyleBackColor = false;
            this.btnFieldStats.Visible = false;
            this.btnFieldStats.Click += new System.EventHandler(this.btnFieldStats_Click);
            // 
            // lblHardwareMessage
            // 
            this.lblHardwareMessage.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lblHardwareMessage.BackColor = System.Drawing.Color.Bisque;
            this.lblHardwareMessage.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.lblHardwareMessage.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHardwareMessage.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.lblHardwareMessage.Location = new System.Drawing.Point(62, 110);
            this.lblHardwareMessage.Name = "lblHardwareMessage";
            this.lblHardwareMessage.Size = new System.Drawing.Size(888, 43);
            this.lblHardwareMessage.TabIndex = 543;
            this.lblHardwareMessage.Text = "This message is very long and boring no one should read 01234";
            this.lblHardwareMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHardwareMessage.Visible = false;
            this.lblHardwareMessage.Click += new System.EventHandler(this.lblHardwareMessage_Click);
            // 
            // FormGPS
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1000, 720);
            this.Controls.Add(this.panelControlBox);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.flp1);
            this.Controls.Add(this.panelSim);
            this.Controls.Add(this.lblFix);
            this.Controls.Add(this.panelLeft);
            this.Controls.Add(this.btnZone8);
            this.Controls.Add(this.btnZone7);
            this.Controls.Add(this.btnZone6);
            this.Controls.Add(this.btnZone5);
            this.Controls.Add(this.btnZone4);
            this.Controls.Add(this.btnZone3);
            this.Controls.Add(this.btnZone2);
            this.Controls.Add(this.btnZone1);
            this.Controls.Add(this.panelNavigation);
            this.Controls.Add(this.btnSection13Man);
            this.Controls.Add(this.btnSection14Man);
            this.Controls.Add(this.btnSection15Man);
            this.Controls.Add(this.btnSection16Man);
            this.Controls.Add(this.panelDrag);
            this.Controls.Add(this.btnSection8Man);
            this.Controls.Add(this.btnSection7Man);
            this.Controls.Add(this.btnSection6Man);
            this.Controls.Add(this.btnSection5Man);
            this.Controls.Add(this.btnSection4Man);
            this.Controls.Add(this.btnSection3Man);
            this.Controls.Add(this.btnSection2Man);
            this.Controls.Add(this.btnSection1Man);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.btnSection12Man);
            this.Controls.Add(this.btnSection11Man);
            this.Controls.Add(this.btnSection10Man);
            this.Controls.Add(this.btnSection9Man);
            this.Controls.Add(this.oglMain);
            this.Controls.Add(this.oglZoom);
            this.Controls.Add(this.lblCurrentField);
            this.Controls.Add(this.lblGuidanceLine);
            this.Controls.Add(this.lblHardwareMessage);
            this.Controls.Add(this.oglBack);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Tahoma", 12F);
            this.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(1000, 720);
            this.Name = "FormGPS";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "AgOpenGPS";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormGPS_FormClosing);
            this.Load += new System.EventHandler(this.FormGPS_Load);
            this.ResizeEnd += new System.EventHandler(this.FormGPS_ResizeEnd);
            this.Move += new System.EventHandler(this.FormGPS_Move);
            this.contextMenuStripOpenGL.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuStripFlag.ResumeLayout(false);
            this.panelDrag.ResumeLayout(false);
            this.statusStripLeft.ResumeLayout(false);
            this.statusStripLeft.PerformLayout();
            this.panelSim.ResumeLayout(false);
            this.panelNavigation.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.statusStrip2.ResumeLayout(false);
            this.statusStrip2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.flp1.ResumeLayout(false);
            this.panelBottom.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.panelControlBox.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Timer tmrWatchdog;
        private System.Windows.Forms.Button btnSection1Man;
        private System.Windows.Forms.Button btnSection2Man;
        private System.Windows.Forms.Button btnSection3Man;
        private System.Windows.Forms.Button btnSection4Man;
        private System.Windows.Forms.Button btnSection5Man;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripFlag;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemFlagRed;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagGrn;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagYel;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripOpenGL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem googleEarthOpenGLContextMenu;
        private System.Windows.Forms.Button btnSection8Man;
        private System.Windows.Forms.Button btnSection7Man;
        private System.Windows.Forms.Button btnSection6Man;
        private System.Windows.Forms.Button btnFlag;
        private System.Windows.Forms.Button btnResetSteerAngle;
        private System.Windows.Forms.Button btnResetSim;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        public System.Windows.Forms.Button btnSectionMasterAuto;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        public System.Windows.Forms.Button btnTrack;
        private System.Windows.Forms.Button btnSection9Man;
        private System.Windows.Forms.Button btnSection10Man;
        private System.Windows.Forms.Button btnSection11Man;
        private System.Windows.Forms.Button btnSection12Man;
        private System.Windows.Forms.ToolStripMenuItem enterSimCoordsToolStripMenuItem;
        public System.Windows.Forms.Button btnAutoYouTurn;
        public System.Windows.Forms.Button btnAutoSteer;
        private System.Windows.Forms.HScrollBar hsbarSteerAngle;
        private OpenTK.GLControl.GLControl oglZoom;
        private OpenTK.GLControl.GLControl oglMain;
        private OpenTK.GLControl.GLControl oglBack;
        private System.Windows.Forms.ComboBox cboxpRowWidth;
        private System.Windows.Forms.Label lblHz;
        public System.Windows.Forms.Button btnContour;
        public System.Windows.Forms.Timer timerSim;
        public System.Windows.Forms.Button btnSectionMasterManual;
        public System.Windows.Forms.Button btnABDraw;
        public System.Windows.Forms.ToolStripMenuItem menustripLanguage;
        public System.Windows.Forms.Button btnCycleLines;
        private System.Windows.Forms.StatusStrip statusStripLeft;
        private System.Windows.Forms.TableLayoutPanel panelSim;
        public System.Windows.Forms.TableLayoutPanel panelDrag;
        private System.Windows.Forms.Button btnHeadlandOnOff;
        private System.Windows.Forms.Button btnShutdown;
        private System.Windows.Forms.Button btnSection16Man;
        private System.Windows.Forms.Button btnSection15Man;
        private System.Windows.Forms.Button btnSection14Man;
        private System.Windows.Forms.Button btnSection13Man;
        private System.Windows.Forms.Button btnSimSetSpeedToZero;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuFlagForm;
        public System.Windows.Forms.Button btnHydLift;
        public System.Windows.Forms.Button btnDayNightMode;
        public System.Windows.Forms.Button btnStartAgIO;
        public System.Windows.Forms.Button btnPickPath;
        public System.Windows.Forms.Button btnPathRecordStop;
        public System.Windows.Forms.Button btnPathGoStop;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton4;
        public System.Windows.Forms.ToolStripMenuItem steerChartStripMenu;
        private System.Windows.Forms.ToolStripMenuItem webcamToolStrip;
        private System.Windows.Forms.ToolStripMenuItem offsetFixToolStrip;
        private System.Windows.Forms.TableLayoutPanel panelNavigation;
        public System.Windows.Forms.Button btn2D;
        public System.Windows.Forms.Button btn3D;
        private System.Windows.Forms.ToolStripMenuItem deleteContourPathsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SmoothABtoolStripMenu;
        public System.Windows.Forms.Button btnAutoSteerConfig;
        public System.Windows.Forms.Button btnChangeMappingColor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem simulatorOnToolStripMenuItem;
        private System.Windows.Forms.Button btnMinimizeMainForm;
        private System.Windows.Forms.Button btnMaximizeMainForm;
        private System.Windows.Forms.ToolStripMenuItem resetALLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetEverythingToolStripMenuItem;
        public System.Windows.Forms.Button btnTramDisplayMode;
        public System.Windows.Forms.Button btnYouSkipEnable;
        private System.Windows.Forms.Label lblFix;
        private System.Windows.Forms.ToolStripMenuItem helpMenuItem;
        private System.Windows.Forms.Button btnNudge;
        public System.Windows.Forms.Button btnResumePath;
        public System.Windows.Forms.Button btnResetToolHeading;
        private System.Windows.Forms.ToolStripMenuItem wizardsMenu;
        private System.Windows.Forms.ToolStripMenuItem steerWizardMenuItem;
        public System.Windows.Forms.Button btnBrightnessDn;
        public System.Windows.Forms.Button btnBrightnessUp;
        private System.Windows.Forms.Button btnZone1;
        private System.Windows.Forms.Button btnZone2;
        private System.Windows.Forms.Button btnZone3;
        private System.Windows.Forms.Button btnZone4;
        private System.Windows.Forms.Button btnZone5;
        private System.Windows.Forms.Button btnZone6;
        private System.Windows.Forms.Button btnZone7;
        private System.Windows.Forms.Button btnZone8;
        private System.Windows.Forms.ToolStripMenuItem rollCheckToolStripMenuItem;
        public System.Windows.Forms.Button btnCycleLinesBk;
        private System.Windows.Forms.TableLayoutPanel panelLeft;
        private AgLibrary.Controls.RepeatButton btnSimSpeedUp;
        private AgLibrary.Controls.RepeatButton btnSpeedDn;
        private System.Windows.Forms.Label lblCurrentField;
        private System.Windows.Forms.Button btnFieldStats;
        public System.Windows.Forms.Button btnJobMenu;
        private System.Windows.Forms.Button btnGPSData;
        public System.Windows.Forms.Button btnSwapABRecordedPath;
        private System.Windows.Forms.Button btnSimReverseDirection;
        private System.Windows.Forms.CheckBox cboxAutoSnapToPivot;
        public System.Windows.Forms.Button btnBuildTracks;
        private System.Windows.Forms.Button btnRefNudge;
        private System.Windows.Forms.Label lblGuidanceLine;
        private System.Windows.Forms.Button btnTracksOff;
        private System.Windows.Forms.FlowLayoutPanel flp1;
        private System.Windows.Forms.FlowLayoutPanel panelBottom;
        public System.Windows.Forms.Button btnContourLock;
        public System.Windows.Forms.FlowLayoutPanel panelRight;
        private System.Windows.Forms.Label lblNumCu;
        public System.Windows.Forms.Button btnAutoTrack;
        public System.Windows.Forms.Button btnNavigationSettings;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripBtnFieldTools;
        private System.Windows.Forms.ToolStripMenuItem boundariesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headlandToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem headlandBuildToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tramLinesMenuField;
        private System.Windows.Forms.ToolStripMenuItem flagByLatLonToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recordedPathStripMenu;
        private System.Windows.Forms.Panel panelControlBox;
        public System.Windows.Forms.Button btnTiltDn;
        public System.Windows.Forms.Button btnTiltUp;
        private System.Windows.Forms.ToolStripMenuItem boundaryToolToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteAppliedToolStripMenuItem;
        public System.Windows.Forms.Button btnPlusAB;
        private System.Windows.Forms.CheckBox cboxIsSectionControlled;
        public System.Windows.Forms.Button btnSnapToPivot;
        private System.Windows.Forms.Button btnAdjLeft;
        private System.Windows.Forms.Button btnAdjRight;
        public System.Windows.Forms.Button btnGrid;
        public System.Windows.Forms.Button btnN2D;
        private System.Windows.Forms.ToolStripMenuItem kioskModeToolStrip;
        private System.Windows.Forms.StatusStrip statusStrip2;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem toolStripConfig;
        private System.Windows.Forms.ToolStripMenuItem toolStripSteerSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripAllSettings;
        private System.Windows.Forms.ToolStripMenuItem toolStripWorkingDirectories;
        private System.Windows.Forms.ToolStripMenuItem toolStripGPSData;
        private System.Windows.Forms.ToolStripMenuItem toolStripColors;
        private System.Windows.Forms.ToolStripMenuItem toolStripSectionColors;
        private System.Windows.Forms.ToolStripMenuItem toolStripHotkeys;
        private System.Windows.Forms.Button btnChargeStatus;
        private System.Windows.Forms.Label lblHardwareMessage;
        private System.Windows.Forms.ToolStripMenuItem eventViewerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem guidelinesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tramsMultiMenuField;
        private System.Windows.Forms.ToolStripMenuItem AgShareApiMenuItem;
        private System.Windows.Forms.ToolStripMenuItem profileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newProfileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadProfileToolStripMenuItem;
    }
}


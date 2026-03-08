using AgLibrary.Settings;
using AgOpenGPS.Core.Models;
using System.Drawing;

namespace AgOpenGPS.Properties
{
    /// <summary>
    /// Legacy Settings class for migration purposes only.
    /// Contains all old properties before the split into Vehicle/Tool/Environment.
    /// </summary>
    public class SettingsLegacy
    {
        // Include all vehicle properties that were moved to VehicleSettings
        public double setVehicle_wheelbase = 3.3;
        public double setVehicle_antennaHeight = 3;
        public double setVehicle_antennaPivot = 0.1;
        public double setVehicle_antennaOffset = 0;
        public double setVehicle_maxSteerAngle = 30;
        public double setVehicle_maxAngularVelocity = 0.64;
        public double setVehicle_trackWidth = 1.9;
        public double setVehicle_hitchLength = -1;
        public double setVehicle_tankTrailingHitchLength = 3;

        public byte setAS_Kp = 50;
        public byte setAS_countsPerDegree = 110;
        public byte setAS_minSteerPWM = 25;
        public byte setAS_highSteerPWM = 180;
        public byte setAS_lowSteerPWM = 30;
        public int setAS_wasOffset = 3;
        public double setAS_snapDistance = 20.0;
        public double setAS_snapDistanceRef = 5;
        public bool setAS_isAutoSteerAutoOn = false;
        public double setAS_guidanceLookAheadTime = 1.5;
        public bool setAS_isConstantContourOn = false;
        public double setAS_sideHillComp = 0.0;
        public byte setAS_ackerman = 100;
        public double setAS_ModeXTE = 0.1;
        public int setAS_ModeTime = 1;
        public double setAS_functionSpeedLimit = 12;
        public double setAS_maxSteerSpeed = 15;
        public double setAS_minSteerSpeed = 0;
        public bool setAS_isSteerInReverse = false;
        public int setAS_deadZoneDistance = 1;
        public int setAS_deadZoneHeading = 10;
        public int setAS_deadZoneDelay = 5;
        public int setAS_numGuideLines = 10;
        public double setAS_uTurnSmoothing = 14;
        public double setAS_uTurnCompensation = 1;

        public double setIMU_rollZero = 0.0;
        public double setIMU_rollFilter = 0.0;
        public bool setIMU_invertRoll = false;
        public bool setIMU_isDualAsIMU = false;
        public double setIMU_fusionWeight2 = 0.06;

        public string setGPS_headingFromWhichSource = "Fix";
        public double setGPS_SimLatitude = 53.4360564;
        public double setGPS_SimLongitude = -111.160047;
        public double setGPS_forwardComp = 0.15;
        public double setGPS_reverseComp = 0.3;
        public int setGPS_ageAlarm = 20;
        public bool setGPS_isRTK = false;
        public bool setGPS_isRTK_KillAutoSteer = false;
        public double setGPS_dualHeadingOffset = 0.0;
        public double setGPS_dualReverseDetectionDistance = 0.25;
        public double setGPS_minimumStepLimit = 0.05;
        public int setGPS_jumpFixAlarmDistance = 0;

        public byte setArdSteer_setting0 = 56;
        public byte setArdSteer_setting1 = 0;
        public byte setArdSteer_maxPulseCounts = 3;
        public bool setArdMac_isDanfoss = false;

        public byte setArdMac_setting0 = 0;
        public byte setArdMac_isHydEnabled = 0;
        public byte setArdMac_hydRaiseTime = 3;
        public byte setArdMac_hydLowerTime = 4;
        public byte setArdMac_user1 = 1;
        public byte setArdMac_user2 = 2;
        public byte setArdMac_user3 = 3;
        public byte setArdMac_user4 = 4;

        public string setRelay_pinConfig = "1,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";

        public TractorBrand setBrand_TBrand = TractorBrand.AGOpenGPS;
        public HarvesterBrand setBrand_HBrand = HarvesterBrand.AgOpenGPS;
        public ArticulatedBrand setBrand_WDBrand = ArticulatedBrand.AgOpenGPS;

        public double purePursuitIntegralGainAB = 0;
        public double stanleyDistanceErrorGain = 1;
        public double stanleyHeadingErrorGain = 1;
        public double stanleyIntegralGainAB = 0;
        public bool setVehicle_isStanleyUsed = false;

        public int setVehicle_vehicleType = 0;
        public double setVehicle_panicStopSpeed = 0;

        // Include all tool properties that were moved to ToolSettings
        public double setVehicle_toolWidth = 4.0;
        public double setVehicle_toolOverlap = 0.0;
        public double setVehicle_toolOffset = 0;
        public double setVehicle_toolTrailingHitchLength = -2.5;
        public double setVehicle_toolLookAheadOn = 1;
        public double setVehicle_toolLookAheadOff = 0.5;
        public bool setTool_isToolTrailing = true;
        public bool setTool_isToolRearFixed = false;
        public bool setTool_isToolTBT = false;
        public bool setTool_isToolFront = false;
        public bool setTool_isSectionsNotZones = true;
        public bool setTool_isSectionOffWhenOut = true;
        public bool setTool_isDisplayTramControl = true;
        public bool setTool_isTramOuterInverted = false;
        public bool setTool_isDirectionMarkers = true;
        public double setTool_trailingToolToPivotLength = 0;

        public int setVehicle_numSections = 3;
        public int setTool_numSectionsMulti = 20;
        public bool setSection_isFast = true;
        public double setTool_defaultSectionWidth = 2;
        public double setTool_sectionWidthMulti = 0.5;
        public string setTool_zones = "2,10,20,0,0,0,0,0,0,0";

        public decimal setSection_position1 = -2;
        public decimal setSection_position2 = -1;
        public decimal setSection_position3 = 1;
        public decimal setSection_position4 = 2;
        public decimal setSection_position5 = 0;
        public decimal setSection_position6 = 0;
        public decimal setSection_position7 = 0;
        public decimal setSection_position8 = 0;
        public decimal setSection_position9 = 0;
        public decimal setSection_position10 = 0;
        public decimal setSection_position11 = 0;
        public decimal setSection_position12 = 0;
        public decimal setSection_position13 = 0;
        public decimal setSection_position14 = 0;
        public decimal setSection_position15 = 0;
        public decimal setSection_position16 = 0;
        public decimal setSection_position17 = 0;

        public Color setColor_sec01 = Color.FromArgb(249, 22, 10);
        public Color setColor_sec02 = Color.FromArgb(68, 84, 254);
        public Color setColor_sec03 = Color.FromArgb(8, 243, 8);
        public Color setColor_sec04 = Color.FromArgb(233, 6, 233);
        public Color setColor_sec05 = Color.FromArgb(200, 191, 86);
        public Color setColor_sec06 = Color.FromArgb(0, 252, 246);
        public Color setColor_sec07 = Color.FromArgb(144, 36, 246);
        public Color setColor_sec08 = Color.FromArgb(232, 102, 21);
        public Color setColor_sec09 = Color.FromArgb(255, 160, 170);
        public Color setColor_sec10 = Color.FromArgb(205, 204, 246);
        public Color setColor_sec11 = Color.FromArgb(213, 239, 190);
        public Color setColor_sec12 = Color.FromArgb(247, 200, 247);
        public Color setColor_sec13 = Color.FromArgb(253, 241, 144);
        public Color setColor_sec14 = Color.FromArgb(187, 250, 250);
        public Color setColor_sec15 = Color.FromArgb(227, 201, 249);
        public Color setColor_sec16 = Color.FromArgb(247, 229, 215);
        public bool setColor_isMultiColorSections = false;

        public double setTram_tramWidth = 24.0;
        public int setTram_passes = 1;
        public double setTram_alpha = 0.8;

        public bool setHeadland_isSectionControlled = true;

        public double setVehicle_goalPointLookAheadHold = 3;
        public double setVehicle_toolOffDelay = 0;
        public double setVehicle_goalPointLookAheadMult = 1.5;
        public double setVehicle_goalPointAcquireFactor = 0.9;
        public double setVehicle_slowSpeedCutoff = 0.5;
        public double setVehicle_minCoverage = 100;
        public double setVehicle_hydraulicLiftLookAhead = 2;
        public bool setF_isSteerWorkSwitchEnabled = false;

        // ===== ENVIRONMENT SETTINGS =====
        // Window positions
        public Point setWindow_Location = new Point(30, 30);
        public Size setWindow_Size = new Size(1005, 730);
        public bool setWindow_Maximized = false;
        public bool setWindow_Minimized = false;
        public Point setJobMenu_location = new Point(200, 200);
        public Size setJobMenu_size = new Size(640, 530);
        public Point setWindow_steerSettingsLocation = new Point(40, 40);
        public Point setWindow_buildTracksLocation = new Point(40, 40);
        public Point setWindow_formNudgeLocation = new Point(200, 200);
        public Size setWindow_formNudgeSize = new Size(200, 400);
        public Size setWindow_abDrawSize = new Size(1022, 742);
        public Size setWindow_HeadlineSize = new Size(1022, 742);
        public Size setWindow_HeadAcheSize = new Size(1022, 742);
        public Size setWindow_MapBndSize = new Size(1022, 742);
        public Size setWindow_BingMapSize = new Size(965, 700);
        public int setWindow_BingZoom = 15;
        public Point setWindow_QuickABLocation = new Point(100, 100);
        public Size setWindow_gridSize = new Size(400, 400);
        public Point setWindow_gridLocation = new Point(20, 20);
        public Size setWindow_tramLineSize = new Size(921, 676);

        // Display settings
        public bool setMenu_isMetric = true;
        public bool setMenu_isGridOn = true;
        public bool setMenu_isLightbarOn = true;
        public bool setMenu_isSideGuideLines = false;
        public bool setMenu_isPureOn = true;
        public bool setMenu_isSimulatorOn = true;
        public bool setMenu_isSpeedoOn = false;
        public bool setMenu_isLightbarNotSteerBar = false;
        public bool setDisplay_isDayMode = true;
        public bool setDisplay_isStartFullScreen = false;
        public bool setDisplay_isKeyboardOn = true;
        public bool setDisplay_isVehicleImage = true;
        public bool setDisplay_isTextureOn = true;
        public bool setDisplay_isBrightnessOn = false;
        public bool setDisplay_isLogElevation = false;
        public bool setDisplay_isSvennArrowOn = false;
        public bool setDisplay_isSectionLinesOn = true;
        public bool setDisplay_isLineSmooth = false;
        public bool setDisplay_isTermsAccepted = false;
        public bool setDisplay_isHardwareMessages = false;
        public bool setDisplay_isAutoStartAgIO = true;
        public bool setDisplay_isAutoOffAgIO = true;
        public bool setWindow_isKioskMode = false;
        public bool setWindow_isShutdownComputer = false;
        public bool setDisplay_isShutdownWhenNoPower = false;

        public int setDisplay_lightbarCmPerPixel = 5;
        public int setDisplay_lineWidth = 2;
        public int setDisplay_brightness = 40;
        public int setDisplay_brightnessSystem = 40;
        public int setDisplay_camSmooth = 50;
        public double setDisplay_camZoom = 9.0;
        public double setDisplay_camPitch = -62;
        public int setDisplay_vehicleOpacity = 100;

        public Color setDisplay_colorDayFrame = Color.FromArgb(210, 210, 230);
        public Color setDisplay_colorNightFrame = Color.FromArgb(50, 50, 65);
        public Color setDisplay_colorSectionsDay = Color.FromArgb(27, 151, 160);
        public Color setDisplay_colorFieldDay = Color.FromArgb(100, 100, 125);
        public Color setDisplay_colorFieldNight = Color.FromArgb(60, 60, 60);
        public Color setDisplay_colorTextNight = Color.FromArgb(230, 230, 230);
        public Color setDisplay_colorTextDay = Color.FromArgb(10, 10, 20);
        public Color setDisplay_colorVehicle = Color.White;

        public string setDisplay_customColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-154671,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";
        public string setDisplay_customSectionColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-154671,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";
        public string setDisplay_buttonOrder = "0,1,2,3,4,5,6,7";

        // Sound settings
        public bool setSound_isUturnOn = true;
        public bool setSound_isHydLiftOn = true;
        public bool setSound_isAutoSteerOn = true;
        public bool setSound_isSectionsOn = true;

        // Keyboard settings
        public string setKey_hotkeys = "ACFGMNPTYVW12345678";

        // Field settings
        public string setF_CurrentDir = "";
        public bool setF_isWorkSwitchEnabled = false;
        public bool setF_isWorkSwitchManualSections = false;
        public bool setF_isWorkSwitchActiveLow = true;
        public bool setF_isSteerWorkSwitchManualSections = false;
        public double setF_minHeadingStepDistance = 0.5;
        public double setF_UserTotalArea = 0.0;
        public bool setF_isRemoteWorkSystemOn = false;

        // Global settings
        public CFeatureSettings setFeatures = new CFeatureSettings();
        public bool setIMU_isReverseOn = true;
        public bool setAutoSwitchDualFixOn = false;
        public double setAutoSwitchDualFixSpeed = 2.0;
        public bool setBnd_isDrawPivot = true;
        public bool isHeadlandDistanceOn = false;
        public int bndToolSpacing = 1;
        public int bndToolSmooth = 1;

        // UTurn settings
        public int set_youTurnExtensionLength = 20;
        public double set_youTurnDistanceFromBoundary = 2;
        public int set_youSkipWidth = 1;
        public double set_youTurnRadius = 8.1;
        public int set_uTurnStyle = 0;

        // AgShare settings
        public string AgShareServer = "https://agshare.agopengps.com";
        public string AgShareApiKey = "";
        public bool PublicField = false;
        public bool AgShareEnabled = false;
        public bool AgShareUploadActive = false;

        // UDP settings
        public int SetGPS_udpWatchMsec = 50;
    }
}

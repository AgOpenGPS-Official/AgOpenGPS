using AgLibrary.Settings;
using AgLibrary.Logging;
using System.Drawing;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Properties
{
    public sealed class Settings
    {
        private static Settings settings_ = new Settings();
        public static Settings Default
        {
            get { return settings_; }
        }

        // ===== WINDOW POSITIONS =====
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

        // ===== DISPLAY SETTINGS =====
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

        // ===== SOUND SETTINGS =====
        public bool setSound_isUturnOn = true;
        public bool setSound_isHydLiftOn = true;
        public bool setSound_isAutoSteerOn = true;
        public bool setSound_isSectionsOn = true;

        // ===== KEYBOARD SETTINGS =====
        public string setKey_hotkeys = "ACFGMNPTYVW12345678";

        // ===== FIELD SETTINGS =====
        public string setF_CurrentDir = "";
        public bool setF_isWorkSwitchEnabled = false;
        public bool setF_isWorkSwitchManualSections = false;
        public bool setF_isWorkSwitchActiveLow = true;
        public bool setF_isSteerWorkSwitchManualSections = false;
        public double setF_minHeadingStepDistance = 0.5;
        public double setF_UserTotalArea = 0.0;
        public bool setF_isRemoteWorkSystemOn = false;

        // ===== GLOBAL SETTINGS =====
        public CFeatureSettings setFeatures = new CFeatureSettings();
        public bool setIMU_isReverseOn = true;
        public bool setAutoSwitchDualFixOn = false;
        public double setAutoSwitchDualFixSpeed = 2.0;
        public bool setBnd_isDrawPivot = true;
        public bool isHeadlandDistanceOn = false;
        public int bndToolSpacing = 1;
        public int bndToolSmooth = 1;

        // ===== AUTOSTEER SETTINGS (verplaatst van Vehicle naar Environment) =====
        public double setAS_snapDistance = 20.0;
        public double setAS_snapDistanceRef = 5;
        public bool setAS_isAutoSteerAutoOn = false;
        public double setAS_guidanceLookAheadTime = 1.5;
        public bool setAS_isConstantContourOn = false;

        // ===== GPS SETTINGS (verplaatst van Vehicle naar Environment) =====
        public double setGPS_SimLatitude = 53.4360564;
        public double setGPS_SimLongitude = -111.160047;
        public bool setGPS_isRTK = false;
        public bool setGPS_isRTK_KillAutoSteer = false;
        public int setGPS_ageAlarm = 20;
        public int setGPS_jumpFixAlarmDistance = 0;

        // ===== UTURN SETTINGS (verplaatst van Vehicle naar Environment) =====
        public int set_youTurnExtensionLength = 20;
        public double set_youTurnDistanceFromBoundary = 2;
        public int set_youSkipWidth = 1;
        public double set_youTurnRadius = 8.1;
        public int set_uTurnStyle = 0;
        public double setAS_uTurnSmoothing = 14;
        public double setAS_uTurnCompensation = 1;

        // ===== AGSHARE SETTINGS =====
        public string AgShareServer = "https://agshare.agopengps.com";
        public string AgShareApiKey = "";
        public bool PublicField = false;
        public bool AgShareEnabled = false;
        public bool AgShareUploadActive = false;

        // ===== UDP SETTINGS =====
        public int SetGPS_udpWatchMsec = 50;

        public LoadResult Load()
        {
            // Use environmentFileName from registry (default = "Default")
            string envFileName = string.IsNullOrEmpty(RegistrySettings.environmentFileName) ? "Default" : RegistrySettings.environmentFileName;
            string path = Path.Combine(RegistrySettings.environmentDirectory, envFileName + ".xml");

            if (!File.Exists(path))
            {
                // Try migrating from old file
                return MigrateFromOld();
            }

            var result = XmlSettingsHandler.LoadXMLFile(path, this);
            return result;
        }

        public void Save()
        {
            // Use environmentFileName from registry (default = "Default")
            string envFileName = string.IsNullOrEmpty(RegistrySettings.environmentFileName) ? "Default" : RegistrySettings.environmentFileName;
            string path = Path.Combine(RegistrySettings.environmentDirectory, envFileName + ".xml");
            XmlSettingsHandler.SaveXMLFile(path, this);
        }

        private LoadResult MigrateFromOld()
        {
            // Check if old vehicle file exists
            if (!string.IsNullOrEmpty(RegistrySettings.vehicleFileName))
            {
                string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, RegistrySettings.vehicleFileName + ".XML");
                if (File.Exists(oldPath))
                {
                    SettingsLegacy oldSettings = new SettingsLegacy();
                    var result = XmlSettingsHandler.LoadXMLFile(oldPath, oldSettings);
                    if (result == LoadResult.Ok)
                    {
                        // Copy environment settings
                        CopyEnvironmentSettings(oldSettings, this);
                        Save();
                        return LoadResult.Ok;
                    }
                }
            }
            return LoadResult.MissingFile;
        }

        private void CopyEnvironmentSettings(SettingsLegacy source, Settings dest)
        {
            // Copy all environment-related fields from source to dest
            // Window positions
            dest.setWindow_Location = source.setWindow_Location;
            dest.setWindow_Size = source.setWindow_Size;
            dest.setWindow_Maximized = source.setWindow_Maximized;
            dest.setWindow_Minimized = source.setWindow_Minimized;
            dest.setJobMenu_location = source.setJobMenu_location;
            dest.setJobMenu_size = source.setJobMenu_size;
            dest.setWindow_steerSettingsLocation = source.setWindow_steerSettingsLocation;
            dest.setWindow_buildTracksLocation = source.setWindow_buildTracksLocation;
            dest.setWindow_formNudgeLocation = source.setWindow_formNudgeLocation;
            dest.setWindow_formNudgeSize = source.setWindow_formNudgeSize;
            dest.setWindow_abDrawSize = source.setWindow_abDrawSize;
            dest.setWindow_HeadlineSize = source.setWindow_HeadlineSize;
            dest.setWindow_HeadAcheSize = source.setWindow_HeadAcheSize;
            dest.setWindow_MapBndSize = source.setWindow_MapBndSize;
            dest.setWindow_BingMapSize = source.setWindow_BingMapSize;
            dest.setWindow_BingZoom = source.setWindow_BingZoom;
            dest.setWindow_QuickABLocation = source.setWindow_QuickABLocation;
            dest.setWindow_gridSize = source.setWindow_gridSize;
            dest.setWindow_gridLocation = source.setWindow_gridLocation;
            dest.setWindow_tramLineSize = source.setWindow_tramLineSize;

            // Display settings
            dest.setMenu_isMetric = source.setMenu_isMetric;
            dest.setMenu_isGridOn = source.setMenu_isGridOn;
            dest.setMenu_isLightbarOn = source.setMenu_isLightbarOn;
            dest.setMenu_isSideGuideLines = source.setMenu_isSideGuideLines;
            dest.setMenu_isPureOn = source.setMenu_isPureOn;
            dest.setMenu_isSimulatorOn = source.setMenu_isSimulatorOn;
            dest.setMenu_isSpeedoOn = source.setMenu_isSpeedoOn;
            dest.setMenu_isLightbarNotSteerBar = source.setMenu_isLightbarNotSteerBar;
            dest.setDisplay_isDayMode = source.setDisplay_isDayMode;
            dest.setDisplay_isStartFullScreen = source.setDisplay_isStartFullScreen;
            dest.setDisplay_isKeyboardOn = source.setDisplay_isKeyboardOn;
            dest.setDisplay_isVehicleImage = source.setDisplay_isVehicleImage;
            dest.setDisplay_isTextureOn = source.setDisplay_isTextureOn;
            dest.setDisplay_isBrightnessOn = source.setDisplay_isBrightnessOn;
            dest.setDisplay_isLogElevation = source.setDisplay_isLogElevation;
            dest.setDisplay_isSvennArrowOn = source.setDisplay_isSvennArrowOn;
            dest.setDisplay_isSectionLinesOn = source.setDisplay_isSectionLinesOn;
            dest.setDisplay_isLineSmooth = source.setDisplay_isLineSmooth;
            dest.setDisplay_isTermsAccepted = source.setDisplay_isTermsAccepted;
            dest.setDisplay_isHardwareMessages = source.setDisplay_isHardwareMessages;
            dest.setDisplay_isAutoStartAgIO = source.setDisplay_isAutoStartAgIO;
            dest.setDisplay_isAutoOffAgIO = source.setDisplay_isAutoOffAgIO;
            dest.setWindow_isKioskMode = source.setWindow_isKioskMode;
            dest.setWindow_isShutdownComputer = source.setWindow_isShutdownComputer;
            dest.setDisplay_isShutdownWhenNoPower = source.setDisplay_isShutdownWhenNoPower;
            dest.setDisplay_lightbarCmPerPixel = source.setDisplay_lightbarCmPerPixel;
            dest.setDisplay_lineWidth = source.setDisplay_lineWidth;
            dest.setDisplay_brightness = source.setDisplay_brightness;
            dest.setDisplay_brightnessSystem = source.setDisplay_brightnessSystem;
            dest.setDisplay_camSmooth = source.setDisplay_camSmooth;
            dest.setDisplay_camZoom = source.setDisplay_camZoom;
            dest.setDisplay_camPitch = source.setDisplay_camPitch;
            dest.setDisplay_vehicleOpacity = source.setDisplay_vehicleOpacity;
            dest.setDisplay_colorDayFrame = source.setDisplay_colorDayFrame;
            dest.setDisplay_colorNightFrame = source.setDisplay_colorNightFrame;
            dest.setDisplay_colorSectionsDay = source.setDisplay_colorSectionsDay;
            dest.setDisplay_colorFieldDay = source.setDisplay_colorFieldDay;
            dest.setDisplay_colorFieldNight = source.setDisplay_colorFieldNight;
            dest.setDisplay_colorTextNight = source.setDisplay_colorTextNight;
            dest.setDisplay_colorTextDay = source.setDisplay_colorTextDay;
            dest.setDisplay_colorVehicle = source.setDisplay_colorVehicle;
            dest.setDisplay_customColors = source.setDisplay_customColors;
            dest.setDisplay_customSectionColors = source.setDisplay_customSectionColors;
            dest.setDisplay_buttonOrder = source.setDisplay_buttonOrder;

            // Sound settings
            dest.setSound_isUturnOn = source.setSound_isUturnOn;
            dest.setSound_isHydLiftOn = source.setSound_isHydLiftOn;
            dest.setSound_isAutoSteerOn = source.setSound_isAutoSteerOn;
            dest.setSound_isSectionsOn = source.setSound_isSectionsOn;

            // Keyboard settings
            dest.setKey_hotkeys = source.setKey_hotkeys;

            // Field settings
            dest.setF_CurrentDir = source.setF_CurrentDir;
            dest.setF_isWorkSwitchEnabled = source.setF_isWorkSwitchEnabled;
            dest.setF_isWorkSwitchManualSections = source.setF_isWorkSwitchManualSections;
            dest.setF_isWorkSwitchActiveLow = source.setF_isWorkSwitchActiveLow;
            dest.setF_isSteerWorkSwitchManualSections = source.setF_isSteerWorkSwitchManualSections;
            dest.setF_minHeadingStepDistance = source.setF_minHeadingStepDistance;
            dest.setF_UserTotalArea = source.setF_UserTotalArea;
            dest.setF_isRemoteWorkSystemOn = source.setF_isRemoteWorkSystemOn;
            // setF_isSteerWorkSwitchEnabled is now in ToolSettings

            // Global settings
            dest.setFeatures = source.setFeatures;
            dest.setIMU_isReverseOn = source.setIMU_isReverseOn;
            dest.setAutoSwitchDualFixOn = source.setAutoSwitchDualFixOn;
            dest.setAutoSwitchDualFixSpeed = source.setAutoSwitchDualFixSpeed;
            dest.setBnd_isDrawPivot = source.setBnd_isDrawPivot;
            dest.isHeadlandDistanceOn = source.isHeadlandDistanceOn;
            dest.bndToolSpacing = source.bndToolSpacing;
            dest.bndToolSmooth = source.bndToolSmooth;

            // AutoSteer settings (verplaatst van Vehicle)
            dest.setAS_snapDistance = source.setAS_snapDistance;
            dest.setAS_snapDistanceRef = source.setAS_snapDistanceRef;
            dest.setAS_isAutoSteerAutoOn = source.setAS_isAutoSteerAutoOn;
            dest.setAS_guidanceLookAheadTime = source.setAS_guidanceLookAheadTime;
            dest.setAS_isConstantContourOn = source.setAS_isConstantContourOn;

            // GPS settings (verplaatst van Vehicle)
            dest.setGPS_SimLatitude = source.setGPS_SimLatitude;
            dest.setGPS_SimLongitude = source.setGPS_SimLongitude;
            dest.setGPS_isRTK = source.setGPS_isRTK;
            dest.setGPS_isRTK_KillAutoSteer = source.setGPS_isRTK_KillAutoSteer;
            dest.setGPS_ageAlarm = source.setGPS_ageAlarm;
            dest.setGPS_jumpFixAlarmDistance = source.setGPS_jumpFixAlarmDistance;

            // UTurn settings
            dest.set_youTurnExtensionLength = source.set_youTurnExtensionLength;
            dest.set_youTurnDistanceFromBoundary = source.set_youTurnDistanceFromBoundary;
            dest.set_youSkipWidth = source.set_youSkipWidth;
            dest.set_youTurnRadius = source.set_youTurnRadius;
            dest.set_uTurnStyle = source.set_uTurnStyle;
            dest.setAS_uTurnSmoothing = source.setAS_uTurnSmoothing;
            dest.setAS_uTurnCompensation = source.setAS_uTurnCompensation;

            // AgShare settings
            dest.AgShareServer = source.AgShareServer;
            dest.AgShareApiKey = source.AgShareApiKey;
            dest.PublicField = source.PublicField;
            dest.AgShareEnabled = source.AgShareEnabled;
            dest.AgShareUploadActive = source.AgShareUploadActive;

            // UDP settings
            dest.SetGPS_udpWatchMsec = source.SetGPS_udpWatchMsec;
        }

        public void Reset()
        {
            settings_ = new Settings();
            settings_.Save();
        }
    }
}

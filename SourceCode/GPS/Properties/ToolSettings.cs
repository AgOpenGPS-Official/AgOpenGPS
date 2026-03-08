using AgLibrary.Settings;
using System.Drawing;
using System.IO;

namespace AgOpenGPS.Properties
{
    public sealed class ToolSettings
    {
        private static ToolSettings settings_ = new ToolSettings();

        public static ToolSettings Default
        {
            get { return settings_; }
        }

        // Tool dimensions
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

        // Sections
        public int setVehicle_numSections = 3;
        public int setTool_numSectionsMulti = 20;
        public bool setSection_isFast = true;
        public double setTool_defaultSectionWidth = 2;
        public double setTool_sectionWidthMulti = 0.5;
        public string setTool_zones = "2,10,20,0,0,0,0,0,0";

        // Section positions
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

        // Section colors
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

        // Tram
        public double setTram_tramWidth = 24.0;
        public int setTram_passes = 1;
        public double setTram_alpha = 0.8;

        // Headland
        public bool setHeadland_isSectionControlled = true;

        // Arduino Machine (verplaatst van Vehicle naar Tool)
        public byte setArdMac_setting0 = 0;
        public byte setArdMac_isHydEnabled = 0;
        public byte setArdMac_hydRaiseTime = 3;
        public byte setArdMac_hydLowerTime = 4;
        public byte setArdMac_user1 = 1;
        public byte setArdMac_user2 = 2;
        public byte setArdMac_user3 = 3;
        public byte setArdMac_user4 = 4;

        // Relay (verplaatst van Vehicle naar Tool)
        public string setRelay_pinConfig = "1,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";

        // Tool specific Steer Parameters (verplaatst van Vehicle naar Tool)
        public double setVehicle_goalPointLookAheadHold = 3;
        public double setVehicle_toolOffDelay = 0;
        public double setVehicle_goalPointLookAheadMult = 1.5;
        public double setVehicle_goalPointAcquireFactor = 0.9;
        public double setVehicle_slowSpeedCutoff = 0.5;
        public double setVehicle_minCoverage = 100;
        public double setVehicle_hydraulicLiftLookAhead = 2;
        public bool setF_isSteerWorkSwitchEnabled = false;

        public LoadResult Load(string toolFileName)
        {
            string path = Path.Combine(RegistrySettings.toolsDirectory, toolFileName + ".xml");
            var result = XmlSettingsHandler.LoadXMLFile(path, this);
            if (result == LoadResult.MissingFile)
            {
                // Try loading from old format and migrate
                return CSettingsMigration.MigrateTool(toolFileName, this);
            }
            return result;
        }

        public void Save(string toolFileName)
        {
            string path = Path.Combine(RegistrySettings.toolsDirectory, toolFileName + ".xml");
            if (!string.IsNullOrEmpty(toolFileName))
                XmlSettingsHandler.SaveXMLFile(path, this);
        }

        public void Reset()
        {
            settings_ = new ToolSettings();
        }
    }
}

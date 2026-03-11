using System;
using System.IO;
using System.Linq;
using System.Xml;
using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public static class CSettingsMigration
    {
        /// <summary>
        /// Reads the XML settings type name from the third-level element.
        /// Returns the element name (e.g. "AgOpenGPS.Properties.VehicleSettings") or null on failure.
        /// </summary>
        public static string GetXmlSettingsType(string filePath)
        {
            try
            {
                using (var reader = new XmlTextReader(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Depth == 2)
                            return reader.Name;
                    }
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Checks if the XML file contains the expected settings type.
        /// </summary>
        public static bool IsSettingsType(string filePath, string expectedType)
        {
            string type = GetXmlSettingsType(filePath);
            return type != null && type.Contains(expectedType);
        }

        /// <summary>
        /// Returns true if the file is an old format (not VehicleSettings, not ToolSettings).
        /// </summary>
        public static bool IsOldFormat(string filePath)
        {
            string type = GetXmlSettingsType(filePath);
            if (type == null) return false;
            return !type.Contains("VehicleSettings") && !type.Contains("ToolSettings");
        }

        public static bool NeedsMigration(string fileName)
        {
            string path = Path.Combine(RegistrySettings.vehiclesDirectory, fileName + ".xml");
            return File.Exists(path) && IsOldFormat(path);
        }

        public static LoadResult MigrateVehicle(string vehicleFileName, VehicleSettings vehicleSettings)
        {
            return MigrateVehicle(vehicleFileName, vehicleFileName, vehicleSettings);
        }

        public static LoadResult MigrateVehicle(string sourceFileName, string outputName, VehicleSettings vehicleSettings)
        {
            string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, sourceFileName + ".xml");

            if (!File.Exists(oldPath))
                return LoadResult.MissingFile;

            // Load old settings
            SettingsLegacy oldSettings = new SettingsLegacy();
            var loadResult = XmlSettingsHandler.LoadXMLFile(oldPath, oldSettings);
            if (loadResult != LoadResult.Ok)
                return loadResult;

            // Copy vehicle settings from old to new
            CopyVehicleSettings(oldSettings, vehicleSettings);

            // Save as new format
            vehicleSettings.Save(outputName);

            return LoadResult.Ok;
        }

        public static LoadResult MigrateTool(string toolFileName, ToolSettings toolSettings)
        {
            return MigrateTool(toolFileName, toolFileName, toolSettings);
        }

        public static LoadResult MigrateTool(string sourceFileName, string outputName, ToolSettings toolSettings)
        {
            string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, sourceFileName + ".xml");

            if (!File.Exists(oldPath))
                return LoadResult.MissingFile;

            // Load old settings
            SettingsLegacy oldSettings = new SettingsLegacy();
            var loadResult = XmlSettingsHandler.LoadXMLFile(oldPath, oldSettings);
            if (loadResult != LoadResult.Ok)
                return loadResult;

            // Copy tool settings from old to new
            CopyToolSettings(oldSettings, toolSettings);

            // Save as new format
            toolSettings.Save(outputName);

            return LoadResult.Ok;
        }

        private static void CopyVehicleSettings(SettingsLegacy source, VehicleSettings dest)
        {
            // Vehicle dimensions
            dest.setVehicle_wheelbase = source.setVehicle_wheelbase;
            dest.setVehicle_antennaHeight = source.setVehicle_antennaHeight;
            dest.setVehicle_antennaPivot = source.setVehicle_antennaPivot;
            dest.setVehicle_antennaOffset = source.setVehicle_antennaOffset;
            dest.setVehicle_maxSteerAngle = source.setVehicle_maxSteerAngle;
            dest.setVehicle_maxAngularVelocity = source.setVehicle_maxAngularVelocity;
            dest.setVehicle_trackWidth = source.setVehicle_trackWidth;
            dest.setVehicle_hitchLength = source.setVehicle_hitchLength;

            // AutoSteer (zonder snapDistance, isAutoSteerAutoOn, guidanceLookAheadTime, isConstantContourOn, uTurnSmoothing, uTurnCompensation - die zitten nu in Environment)
            dest.setAS_Kp = source.setAS_Kp;
            dest.setAS_countsPerDegree = source.setAS_countsPerDegree;
            dest.setAS_minSteerPWM = source.setAS_minSteerPWM;
            dest.setAS_highSteerPWM = source.setAS_highSteerPWM;
            dest.setAS_lowSteerPWM = source.setAS_lowSteerPWM;
            dest.setAS_wasOffset = source.setAS_wasOffset;
            dest.setAS_sideHillComp = source.setAS_sideHillComp;
            dest.setAS_ackerman = source.setAS_ackerman;
            dest.setAS_ModeXTE = source.setAS_ModeXTE;
            dest.setAS_ModeTime = source.setAS_ModeTime;
            dest.setAS_functionSpeedLimit = source.setAS_functionSpeedLimit;
            dest.setAS_maxSteerSpeed = source.setAS_maxSteerSpeed;
            dest.setAS_minSteerSpeed = source.setAS_minSteerSpeed;
            dest.setAS_isSteerInReverse = source.setAS_isSteerInReverse;
            dest.setAS_deadZoneDistance = source.setAS_deadZoneDistance;
            dest.setAS_deadZoneHeading = source.setAS_deadZoneHeading;
            dest.setAS_deadZoneDelay = source.setAS_deadZoneDelay;
            dest.setAS_numGuideLines = source.setAS_numGuideLines;

            // IMU
            dest.setIMU_rollZero = source.setIMU_rollZero;
            dest.setIMU_rollFilter = source.setIMU_rollFilter;
            dest.setIMU_invertRoll = source.setIMU_invertRoll;
            dest.setIMU_isDualAsIMU = source.setIMU_isDualAsIMU;
            dest.setIMU_fusionWeight2 = source.setIMU_fusionWeight2;

            // GPS (zonder SimLatitude/SimLongitude, isRTK, isRTK_KillAutoSteer, ageAlarm, jumpFixAlarmDistance - die zitten nu in Environment)
            dest.setGPS_headingFromWhichSource = source.setGPS_headingFromWhichSource;
            dest.setGPS_forwardComp = source.setGPS_forwardComp;
            dest.setGPS_reverseComp = source.setGPS_reverseComp;
            dest.setGPS_dualHeadingOffset = source.setGPS_dualHeadingOffset;
            dest.setGPS_dualReverseDetectionDistance = source.setGPS_dualReverseDetectionDistance;
            dest.setGPS_minimumStepLimit = source.setGPS_minimumStepLimit;

            // Arduino Steer
            dest.setArdSteer_setting0 = source.setArdSteer_setting0;
            dest.setArdSteer_setting1 = source.setArdSteer_setting1;
            dest.setArdSteer_maxPulseCounts = source.setArdSteer_maxPulseCounts;
            dest.setArdMac_isDanfoss = source.setArdMac_isDanfoss;

            // Brands
            dest.setBrand_TBrand = source.setBrand_TBrand;
            dest.setBrand_HBrand = source.setBrand_HBrand;
            dest.setBrand_WDBrand = source.setBrand_WDBrand;

            // Vehicle type (zonder goalPointLookAheadMult, goalPointLookAheadHold, goalPointAcquireFactor, slowSpeedCutoff, minCoverage, hydraulicLiftLookAhead, toolOffDelay, isSteerWorkSwitchEnabled - die zitten nu in Tool)
            dest.setVehicle_vehicleType = source.setVehicle_vehicleType;
            dest.setVehicle_panicStopSpeed = source.setVehicle_panicStopSpeed;
        }

        private static void CopyToolSettings(SettingsLegacy source, ToolSettings dest)
        {
            // Tool dimensions
            dest.setVehicle_toolWidth = source.setVehicle_toolWidth;
            dest.setVehicle_toolOverlap = source.setVehicle_toolOverlap;
            dest.setVehicle_toolOffset = source.setVehicle_toolOffset;
            dest.setVehicle_tankTrailingHitchLength = source.setVehicle_tankTrailingHitchLength;
            dest.setVehicle_toolTrailingHitchLength = source.setVehicle_toolTrailingHitchLength;
            dest.setVehicle_toolLookAheadOn = source.setVehicle_toolLookAheadOn;
            dest.setVehicle_toolLookAheadOff = source.setVehicle_toolLookAheadOff;
            dest.setTool_isToolTrailing = source.setTool_isToolTrailing;
            dest.setTool_isToolRearFixed = source.setTool_isToolRearFixed;
            dest.setTool_isToolTBT = source.setTool_isToolTBT;
            dest.setTool_isToolFront = source.setTool_isToolFront;
            dest.setTool_isSectionsNotZones = source.setTool_isSectionsNotZones;
            dest.setTool_isSectionOffWhenOut = source.setTool_isSectionOffWhenOut;
            dest.setTool_isDisplayTramControl = source.setTool_isDisplayTramControl;
            dest.setTool_isTramOuterInverted = source.setTool_isTramOuterInverted;
            dest.setTool_isDirectionMarkers = source.setTool_isDirectionMarkers;
            dest.setTool_trailingToolToPivotLength = source.setTool_trailingToolToPivotLength;

            // Sections
            dest.setVehicle_numSections = source.setVehicle_numSections;
            dest.setTool_numSectionsMulti = source.setTool_numSectionsMulti;
            dest.setSection_isFast = source.setSection_isFast;
            dest.setTool_defaultSectionWidth = source.setTool_defaultSectionWidth;
            dest.setTool_sectionWidthMulti = source.setTool_sectionWidthMulti;
            // Old format had 10 zones values, new format uses 9 - truncate to prevent IndexOutOfRangeException
            var zoneWords = source.setTool_zones.Split(',');
            dest.setTool_zones = string.Join(",", zoneWords.Take(9));

            // Section positions
            dest.setSection_position1 = source.setSection_position1;
            dest.setSection_position2 = source.setSection_position2;
            dest.setSection_position3 = source.setSection_position3;
            dest.setSection_position4 = source.setSection_position4;
            dest.setSection_position5 = source.setSection_position5;
            dest.setSection_position6 = source.setSection_position6;
            dest.setSection_position7 = source.setSection_position7;
            dest.setSection_position8 = source.setSection_position8;
            dest.setSection_position9 = source.setSection_position9;
            dest.setSection_position10 = source.setSection_position10;
            dest.setSection_position11 = source.setSection_position11;
            dest.setSection_position12 = source.setSection_position12;
            dest.setSection_position13 = source.setSection_position13;
            dest.setSection_position14 = source.setSection_position14;
            dest.setSection_position15 = source.setSection_position15;
            dest.setSection_position16 = source.setSection_position16;
            dest.setSection_position17 = source.setSection_position17;

            // Section colors
            dest.setColor_sec01 = source.setColor_sec01;
            dest.setColor_sec02 = source.setColor_sec02;
            dest.setColor_sec03 = source.setColor_sec03;
            dest.setColor_sec04 = source.setColor_sec04;
            dest.setColor_sec05 = source.setColor_sec05;
            dest.setColor_sec06 = source.setColor_sec06;
            dest.setColor_sec07 = source.setColor_sec07;
            dest.setColor_sec08 = source.setColor_sec08;
            dest.setColor_sec09 = source.setColor_sec09;
            dest.setColor_sec10 = source.setColor_sec10;
            dest.setColor_sec11 = source.setColor_sec11;
            dest.setColor_sec12 = source.setColor_sec12;
            dest.setColor_sec13 = source.setColor_sec13;
            dest.setColor_sec14 = source.setColor_sec14;
            dest.setColor_sec15 = source.setColor_sec15;
            dest.setColor_sec16 = source.setColor_sec16;
            dest.setColor_isMultiColorSections = source.setColor_isMultiColorSections;

            // Tram
            dest.setTram_tramWidth = source.setTram_tramWidth;
            dest.setTram_passes = source.setTram_passes;
            dest.setTram_alpha = source.setTram_alpha;

            // Headland
            dest.setHeadland_isSectionControlled = source.setHeadland_isSectionControlled;

            // Arduino Machine (verplaatst van Vehicle naar Tool)
            dest.setArdMac_setting0 = source.setArdMac_setting0;
            dest.setArdMac_isHydEnabled = source.setArdMac_isHydEnabled;
            dest.setArdMac_hydRaiseTime = source.setArdMac_hydRaiseTime;
            dest.setArdMac_hydLowerTime = source.setArdMac_hydLowerTime;
            dest.setArdMac_user1 = source.setArdMac_user1;
            dest.setArdMac_user2 = source.setArdMac_user2;
            dest.setArdMac_user3 = source.setArdMac_user3;
            dest.setArdMac_user4 = source.setArdMac_user4;

            // Relay (verplaatst van Vehicle naar Tool)
            dest.setRelay_pinConfig = source.setRelay_pinConfig;

            // Guidance algorithm (verplaatst van Vehicle naar Tool)
            dest.purePursuitIntegralGainAB = source.purePursuitIntegralGainAB;
            dest.stanleyDistanceErrorGain = source.stanleyDistanceErrorGain;
            dest.stanleyHeadingErrorGain = source.stanleyHeadingErrorGain;
            dest.stanleyIntegralGainAB = source.stanleyIntegralGainAB;
            dest.setVehicle_isStanleyUsed = source.setVehicle_isStanleyUsed;

            // Tool specific Steer Parameters (verplaatst van Vehicle naar Tool)
            dest.setVehicle_goalPointLookAheadHold = source.setVehicle_goalPointLookAheadHold;
            dest.setVehicle_toolOffDelay = source.setVehicle_toolOffDelay;
            dest.setVehicle_goalPointLookAheadMult = source.setVehicle_goalPointLookAheadMult;
            dest.setVehicle_goalPointAcquireFactor = source.setVehicle_goalPointAcquireFactor;
            dest.setVehicle_slowSpeedCutoff = source.setVehicle_slowSpeedCutoff;
            dest.setVehicle_minCoverage = source.setVehicle_minCoverage;
            dest.setVehicle_hydraulicLiftLookAhead = source.setVehicle_hydraulicLiftLookAhead;
            dest.setF_isSteerWorkSwitchEnabled = source.setF_isSteerWorkSwitchEnabled;
        }

        private static void CheckAndBackupOldFile(string fileName)
        {
            string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, fileName + ".XML");
            string vehiclePath = Path.Combine(RegistrySettings.vehiclesDirectory, fileName + ".xml");
            string toolPath = Path.Combine(RegistrySettings.toolsDirectory, fileName + ".xml");

            // If both new files exist, backup the old one
            if (File.Exists(vehiclePath) && File.Exists(toolPath) && File.Exists(oldPath))
            {
                string backupDir = Path.Combine(RegistrySettings.vehiclesDirectory, "oldSettingsFiles");
                if (!Directory.Exists(backupDir))
                    Directory.CreateDirectory(backupDir);

                string backupPath = Path.Combine(backupDir, fileName + ".XML.backup");
                if (File.Exists(backupPath))
                    File.Delete(backupPath);

                File.Move(oldPath, backupPath);
                Log.EventWriter($"Settings migrated for {fileName}. Old file backed up to oldSettingsFiles.");
            }
        }

        public static LoadResult MigrateEnvironment(string sourceFileName, string outputName)
        {
            string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, sourceFileName + ".xml");

            if (!File.Exists(oldPath))
                return LoadResult.MissingFile;

            // Load old settings
            SettingsLegacy oldSettings = new SettingsLegacy();
            var loadResult = XmlSettingsHandler.LoadXMLFile(oldPath, oldSettings);
            if (loadResult != LoadResult.Ok)
                return loadResult;

            // Copy environment settings from old to new
            var envSettings = new Settings();
            Settings.MigrateFromOldToTarget(oldSettings, envSettings);

            // Save as environment file
            string envPath = Path.Combine(RegistrySettings.environmentDirectory, outputName + ".xml");
            XmlSettingsHandler.SaveXMLFile(envPath, envSettings);

            return LoadResult.Ok;
        }

        /// <summary>
        /// Gets list of old format XML files in the Vehicles directory that can be converted.
        /// Uses XML type detection to distinguish old from new format.
        /// </summary>
        public static string[] GetConvertibleFiles()
        {
            string vehiclesDir = RegistrySettings.vehiclesDirectory;
            if (!Directory.Exists(vehiclesDir))
                return new string[0];

            var files = new System.Collections.Generic.List<string>();
            foreach (string file in Directory.GetFiles(vehiclesDir, "*.xml"))
            {
                if (IsOldFormat(file))
                    files.Add(Path.GetFileNameWithoutExtension(file));
            }
            return files.ToArray();
        }

        /// <summary>
        /// Backs up an old format file after conversion.
        /// </summary>
        public static void BackupOldFile(string fileName)
        {
            string oldPath = Path.Combine(RegistrySettings.vehiclesDirectory, fileName + ".xml");
            if (!File.Exists(oldPath)) return;

            // Don't backup if it's already a new-format file (happens when vehicleName == sourceFile)
            if (!IsOldFormat(oldPath)) return;

            string backupDir = Path.Combine(RegistrySettings.vehiclesDirectory, "oldSettingsFiles");
            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);

            string backupPath = Path.Combine(backupDir, fileName + ".xml.backup");
            if (File.Exists(backupPath))
                File.Delete(backupPath);

            File.Move(oldPath, backupPath);
            Log.EventWriter($"Old settings file backed up: {fileName}");
        }

        public static void MigrateAllVehicles()
        {
            string[] files = GetConvertibleFiles();

            foreach (string fileName in files)
            {
                // Migrate vehicle settings
                var vehicleSettings = new VehicleSettings();
                MigrateVehicle(fileName, fileName, vehicleSettings);

                // Migrate tool settings
                var toolSettings = new ToolSettings();
                MigrateTool(fileName, fileName, toolSettings);

                // Backup old file
                BackupOldFile(fileName);
            }
        }
    }
}

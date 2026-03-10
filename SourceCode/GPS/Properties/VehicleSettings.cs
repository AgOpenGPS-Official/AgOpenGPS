using AgLibrary.Settings;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Properties
{
    public sealed class VehicleSettings
    {
        private static VehicleSettings settings_ = new VehicleSettings();

        public static VehicleSettings Default
        {
            get { return settings_; }
        }

        // Vehicle dimensions
        public double setVehicle_wheelbase = 3.3;
        public double setVehicle_antennaHeight = 3;
        public double setVehicle_antennaPivot = 0.1;
        public double setVehicle_antennaOffset = 0;
        public double setVehicle_maxSteerAngle = 30;
        public double setVehicle_maxAngularVelocity = 0.64;
        public double setVehicle_trackWidth = 1.9;
        public double setVehicle_hitchLength = -1;

        // AutoSteer settings (zonder snapDistance, isAutoSteerAutoOn, guidanceLookAheadTime, isConstantContourOn, uTurnSmoothing, uTurnCompensation - die zitten nu in Environment)
        public byte setAS_Kp = 50;
        public byte setAS_countsPerDegree = 110;
        public byte setAS_minSteerPWM = 25;
        public byte setAS_highSteerPWM = 180;
        public byte setAS_lowSteerPWM = 30;
        public int setAS_wasOffset = 3;
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

        // IMU settings
        public double setIMU_rollZero = 0.0;
        public double setIMU_rollFilter = 0.0;
        public bool setIMU_invertRoll = false;
        public bool setIMU_isDualAsIMU = false;
        public double setIMU_fusionWeight2 = 0.06;

        // GPS settings (zonder SimLatitude/SimLongitude, isRTK, isRTK_KillAutoSteer, ageAlarm, jumpFixAlarmDistance - die zitten nu in Environment)
        public string setGPS_headingFromWhichSource = "Fix";
        public double setGPS_forwardComp = 0.15;
        public double setGPS_reverseComp = 0.3;
        public double setGPS_dualHeadingOffset = 0.0;
        public double setGPS_dualReverseDetectionDistance = 0.25;
        public double setGPS_minimumStepLimit = 0.05;

        // Arduino Steer
        public byte setArdSteer_setting0 = 56;
        public byte setArdSteer_setting1 = 0;
        public byte setArdSteer_maxPulseCounts = 3;
        public bool setArdMac_isDanfoss = false;

        // Brands
        public TractorBrand setBrand_TBrand = TractorBrand.AGOpenGPS;
        public HarvesterBrand setBrand_HBrand = HarvesterBrand.AgOpenGPS;
        public ArticulatedBrand setBrand_WDBrand = ArticulatedBrand.AgOpenGPS;

        // PurePursuit/Stanley
        public double purePursuitIntegralGainAB = 0;
        public double stanleyDistanceErrorGain = 1;
        public double stanleyHeadingErrorGain = 1;
        public double stanleyIntegralGainAB = 0;
        public bool setVehicle_isStanleyUsed = false;

        // Vehicle type (zonder goalPointLookAheadMult, goalPointLookAheadHold, goalPointAcquireFactor, slowSpeedCutoff, minCoverage, hydraulicLiftLookAhead, toolOffDelay, isSteerWorkSwitchEnabled - die zitten nu in Tool)
        public int setVehicle_vehicleType = 0;
        public double setVehicle_panicStopSpeed = 0;

        public LoadResult Load(string vehicleFileName)
        {
            string path = Path.Combine(RegistrySettings.vehiclesDirectory, vehicleFileName + ".xml");
            var result = XmlSettingsHandler.LoadXMLFile(path, this);
            if (result == LoadResult.MissingFile)
            {
                // Try loading from old format and migrate
                return CSettingsMigration.MigrateVehicle(vehicleFileName, this);
            }
            return result;
        }

        public void Save(string vehicleFileName)
        {
            string path = Path.Combine(RegistrySettings.vehiclesDirectory, vehicleFileName + ".xml");
            if (!string.IsNullOrEmpty(vehicleFileName))
                XmlSettingsHandler.SaveXMLFile(path, this);
        }

        public void Reset()
        {
            settings_ = new VehicleSettings();
        }
    }
}

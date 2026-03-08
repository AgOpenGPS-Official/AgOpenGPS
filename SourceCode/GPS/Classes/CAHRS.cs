namespace AgOpenGPS
{
    public class CAHRS
    {
        //private readonly FormGPS mf;

        //Roll and heading from the IMU
        public double imuHeading = 99999, imuRoll = 0, imuPitch = 0, imuYawRate = 0;

        public System.Int16 angVel;

        //actual value in degrees
        public double rollZero;

        //Roll Filter Value
        public double rollFilter;

        //is the auto steer in auto turn on mode or not
        public bool isAutoSteerAuto, isRollInvert, isDualAsIMU, isReverseOn;

        // AutoswitchDualFix 
        public bool autoSwitchDualFixOn;
        public double autoSwitchDualFixSpeed;

        //the factor for fusion of GPS and IMU
        public double forwardComp, reverseComp, fusionWeight;

        //constructor
        public CAHRS()
        {
            rollZero = Properties.VehicleSettings.Default.setIMU_rollZero;

            rollFilter = Properties.VehicleSettings.Default.setIMU_rollFilter;

            fusionWeight = Properties.VehicleSettings.Default.setIMU_fusionWeight2;

            //isAutoSteerAuto = Properties.Settings.Default.setAS_isAutoSteerAutoOn;
            isAutoSteerAuto = true;

            forwardComp = Properties.VehicleSettings.Default.setGPS_forwardComp;

            reverseComp = Properties.VehicleSettings.Default.setGPS_reverseComp;

            isRollInvert = Properties.VehicleSettings.Default.setIMU_invertRoll;

            isReverseOn = Properties.Settings.Default.setIMU_isReverseOn;

            autoSwitchDualFixOn = Properties.Settings.Default.setAutoSwitchDualFixOn;

            autoSwitchDualFixSpeed = Properties.Settings.Default.setAutoSwitchDualFixSpeed;
        }
    }
}
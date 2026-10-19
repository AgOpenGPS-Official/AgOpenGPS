namespace AgLibrary.Ntrip
{
    /// <summary>RTCM 3 message numbers the forwarder and the UI care about.</summary>
    public static class RtcmMessages
    {
        /// <summary>Per-epoch satellite observations: legacy GPS / GLONASS, or any MSM.</summary>
        public static bool IsObservation(int type) =>
            (type >= 1001 && type <= 1004) || (type >= 1009 && type <= 1012) || IsMsm(type);

        /// <summary>Multiple Signal Messages: 1071-1137, the last digit 1-7 being the MSM level.</summary>
        public static bool IsMsm(int type) => type >= 1071 && type <= 1137 && type % 10 >= 1 && type % 10 <= 7;

        public static string Describe(int type)
        {
            if (IsMsm(type))
            {
                string system;
                switch ((type - 1070) / 10)
                {
                    case 0: system = "GPS"; break;
                    case 1: system = "GLONASS"; break;
                    case 2: system = "Galileo"; break;
                    case 3: system = "SBAS"; break;
                    case 4: system = "QZSS"; break;
                    case 5: system = "BeiDou"; break;
                    case 6: system = "NavIC"; break;
                    default: system = "?"; break;
                }
                return system + " observations (MSM" + (type % 10) + ")";
            }
            if (type >= 1001 && type <= 1004) return "GPS observations (legacy)";
            if (type >= 1009 && type <= 1012) return "GLONASS observations (legacy)";
            if (type >= 4001 && type <= 4095) return "proprietary";
            switch (type)
            {
                case 1005: return "base position";
                case 1006: return "base position + antenna height";
                case 1007: case 1008: return "antenna descriptor";
                case 1013: return "system parameters";
                case 1019: return "GPS ephemeris";
                case 1020: return "GLONASS ephemeris";
                case 1029: return "text";
                case 1033: return "receiver + antenna descriptor";
                case 1042: return "BeiDou ephemeris";
                case 1044: return "QZSS ephemeris";
                case 1045: case 1046: return "Galileo ephemeris";
                case 1230: return "GLONASS code-phase biases";
                case -1: return "empty";
                default: return "";
            }
        }
    }
}

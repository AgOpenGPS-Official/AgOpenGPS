namespace AgOpenGPS.Droid
{
    /// <summary>
    /// Colours of the driving screen: dark for night (the default), light for bright sun.
    /// Screens built before a change are rebuilt (<see cref="Version"/>).
    /// </summary>
    public static class Palette
    {
        public static bool Day { get; private set; }
        public static int Version { get; private set; }

        public static void SetDay(bool day)
        {
            if (day == Day) return;
            Day = day;
            Version++;
        }

        // Screen and panels
        public static string Screen => Day ? "#CFD8DC" : "#37474F";
        public static string MapBackground => Day ? "#EEF1EC" : "#263238";
        public static string PanelLabel => Day ? "#455A64" : "#B0BEC5";
        public static string PanelText => Day ? "#212121" : "#ECEFF1";
        public static string InfoText => Day ? "#212121" : "#ECEFF1";
        public static string RecordText => Day ? "#BF360C" : "#FFAB91";
        public static string Hint => Day ? "#546E7A" : "#B0BEC5";

        // Map lines
        public static string CurrentPass => Day ? "#F57F17" : "#FFD54F";
        public static string OtherPass => Day ? "#78909C" : "#607D8B";
        public static string RefLine => Day ? "#0277BD" : "#4FC3F7";
        public static string Boundary => Day ? "#2E7D32" : "#A5D6A7";
        public static string Headland => Day ? "#E65100" : "#FFB74D";
        public static string Recording => Day ? "#D84315" : "#FF7043";
        public static string BoundaryRecording => Day ? "#00838F" : "#4DD0E1";
        public static string Vehicle => Day ? "#263238" : "#FFFFFF";
        public static string VehicleEdge => Day ? "#FFFFFF" : "#212121";
        public static string Goal => Day ? "#263238" : "#FFFFFF";
        public static string LightbarBackground => Day ? "#B0BEC5" : "#263238";
        public static string LightbarOff => Day ? "#90A4AE" : "#37474F";
    }
}

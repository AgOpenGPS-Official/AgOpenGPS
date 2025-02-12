namespace AgOpenGPS.Core.Models
{
    public enum VehicleType
    {
        Tractor = 0,
        Harvester = 1,
        Articulated = 2
    }

    public class VehicleConfig
    {
        public VehicleType Type { get; set; }

        public ColorRgb Color { get; set; }
        public double Opacity { get; set; }

        public double AntennaHeight { get; set; }
        public double AntennaPivot { get; set; }
        public double AntennaOffset { get; set; }

        public double Wheelbase { get; set; }
        public double TrackWidth { get; set; }
    }
}

using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Testing
{
    public interface ISimulatorController
    {
        void Enable();
        void Disable();
        bool IsEnabled { get; }
        void SetPosition(double lat, double lon);
        void SetPositionLocal(double easting, double northing);
        void SetHeading(double headingDegrees);
        void SetSpeed(double speedKph);
        void SetSteerAngle(double angleDegrees);
        SimulatorState GetState();
    }

    public class SimulatorState
    {
        public Wgs84 Position { get; set; }
        public double HeadingDegrees { get; set; }
        public double SpeedKph { get; set; }
        public double SteerAngleDegrees { get; set; }
        public double Easting { get; set; }
        public double Northing { get; set; }
    }
}

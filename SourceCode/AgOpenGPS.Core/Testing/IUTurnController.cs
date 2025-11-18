namespace AgOpenGPS.Core.Testing
{
    public interface IUTurnController
    {
        void Enable();
        void Disable();
        bool IsEnabled { get; }
        void SetDistanceFromBoundary(double distanceMeters);
        void SetTurnMode(UTurnMode mode);
        UTurnState GetState();
    }

    public enum UTurnMode
    {
        Off,
        On
    }

    public class UTurnState
    {
        public bool IsActive { get; set; }
        public bool IsTriggered { get; set; }
        public bool IsInTurn { get; set; }
        public double DistanceFromBoundary { get; set; }
    }
}

namespace AgOpenGPS.Core.Testing
{
    public interface IAutosteerController
    {
        void Enable();
        void Disable();
        bool IsEnabled { get; }
        void SetGuidanceMode(GuidanceMode mode);
        AutosteerState GetState();
    }

    public enum GuidanceMode
    {
        PurePursuit,
        Stanley
    }

    public class AutosteerState
    {
        public bool IsActive { get; set; }
        public double CrossTrackError { get; set; }
        public double SteerAngleDemand { get; set; }
        public GuidanceMode Mode { get; set; }
        public double GoalPointDistance { get; set; }
    }
}

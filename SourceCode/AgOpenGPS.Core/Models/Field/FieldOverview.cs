
namespace AgOpenGPS.Core.Models
{
    public class FieldOverview
    {
        public FieldOverview(string creator, string offsets, string convergence, Wgs84 start)
        {
            Creator = creator;
            Offsets = offsets;
            Convergence = convergence;
            Start = start;
        }

        public string Creator { get; }
        public string Offsets { get; }
        public string Convergence { get; }

        public Wgs84 Start { get; }
    }
}

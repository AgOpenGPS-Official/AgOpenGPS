namespace AgOpenGPS.Core.Models
{
    public enum TramMode
    {
        None,
        All,
        FillTracks,
        BoundaryTracks
    }

    public static class TramModeExtensions
    {
        public static bool IncludesFillTracks(this TramMode mode)
        {
            return
                mode == TramMode.All ||
                mode == TramMode.FillTracks;
        }

        public static bool IncludesBoundaryTracks(this TramMode mode)
        {
            return
                mode == TramMode.All ||
                mode == TramMode.BoundaryTracks;
        }
    }
}

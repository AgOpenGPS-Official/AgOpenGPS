namespace AgOpenGPS.Core.Models
{
    public enum TramDisplayMode
    {
        Hide,
        DisplayAll,
        DisplayFillTracks,
        DisplayBoundaryTracks
    }

    public enum TramGenerateMode
    {
        GenerateAll,
        GenerateFillTracks,
        GenerateBoundaryTracks
    }

    public static class TramDisplayModeExt
    {
        public static bool MustDisplayFillTracks(this TramDisplayMode displayMode)
        {
            return
                displayMode == TramDisplayMode.DisplayAll ||
                displayMode == TramDisplayMode.DisplayFillTracks;
        }

        public static bool MustDisplayBoundaryTracks(this TramDisplayMode displayMode)
        {
            return
                displayMode == TramDisplayMode.DisplayAll ||
                displayMode == TramDisplayMode.DisplayBoundaryTracks;
        }
    }

    public static class TramGenerateModeExt
    {
        public static bool MustGenerateFillTracks(this TramGenerateMode generateMode)
        {
            return
                generateMode == TramGenerateMode.GenerateAll ||
                generateMode == TramGenerateMode.GenerateFillTracks;
        }

        public static bool MustGenerateBoundaryTracks(TramGenerateMode generateMode)
        {
            return
                generateMode == TramGenerateMode.GenerateAll ||
                generateMode == TramGenerateMode.GenerateBoundaryTracks;
        }
    }
}

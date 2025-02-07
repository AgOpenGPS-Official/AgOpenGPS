namespace AgOpenGPS.Core.Interfaces
{
    public interface IFieldStreamerPresenter
    {
        void PresentBoundaryFileMissing();
        void PresentBoundaryFileCorrupt();

        void PresentContourFileMissing();
        void PresentContourFileCorrupt();

        void PresentCurveLineFileCorrupt();

        void PresentFlagsFileMissing();
        void PresentFlagsFileCorrupt();

        void PresentRecordedPathFileCorrupt();

        void PresentSectionFileMissing();
        void PresentSectionFileCorrupt();

        void PresentTramLinesFileCorrupt();
    }
}

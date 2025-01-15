using System.Diagnostics;

namespace AgOpenGPS.Pgn
{
    public class Pgn235SectionDimensions : PgnBase
    {
        private const int _contentSize = 33;
        private const int _maxSections = (_contentSize - 1) / 2;
        private const int _section0LoIndex = 5;
        private const int _section0HiIndex = 6;
        private const int _numberOfSectionsIndex = 37;

        public Pgn235SectionDimensions(IPgnErrorPresenter errorPresenter) : base(235, _contentSize, errorPresenter)
        {
        }

        public void SetSectionWidth(int section, double sectionWidthInMeters)
        {
            Debug.Assert(0 <= section);
            Debug.Assert(section < _maxSections);

            int sectionLoIndex = _section0LoIndex + 2 * section;
            SetDoubleLoHi((byte)sectionLoIndex, 100.0 * sectionWidthInMeters);
        }

        public void SetNumberOfSections(int numberOfSections)
        {
            SetInt(_numberOfSectionsIndex, numberOfSections);
        }
    }
}

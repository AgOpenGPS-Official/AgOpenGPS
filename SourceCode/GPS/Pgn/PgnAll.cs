using AgOpenGPS.Pgn;

namespace AgOpenGPS
{
    public class PgnAll
    {
        private readonly Pgn254AutoSteerData _pgn254;
        private readonly Pgn252AutoSteerSettings _pgn252;
        private readonly Pgn236RelayConfig _pgn236;
        private readonly Pgn235SectionDimensions _pgn235;

        public PgnAll(IPgnErrorPresenter errorPresenter)
        {
            _pgn254 = new Pgn254AutoSteerData(errorPresenter);
            _pgn252 = new Pgn252AutoSteerSettings(errorPresenter);
            _pgn236 = new Pgn236RelayConfig(errorPresenter);
            _pgn235 = new Pgn235SectionDimensions(errorPresenter);
        }

        public Pgn254AutoSteerData Pgn254 => _pgn254;
        public Pgn252AutoSteerSettings Pgn252 => _pgn252;
        public Pgn236RelayConfig Pgn236 => _pgn236;
        public Pgn235SectionDimensions Pgn235 => _pgn235;
    }
}

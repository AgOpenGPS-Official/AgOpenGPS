using AgOpenGPS.Pgn;

namespace AgOpenGPS
{
    public class PgnAll
    {
        private readonly Pgn100CorrectedPosition _pgn100;
        private readonly Pgn229SectionSymmetric _pgn229;
        private readonly Pgn235SectionDimensions _pgn235;
        private readonly Pgn236RelayConfig _pgn236;
        private readonly Pgn238MachineConfig _pgn238;
        private readonly Pgn239MachineData _pgn239;
        private readonly Pgn251AutoSteerBoardConfig _pgn251;
        private readonly Pgn252AutoSteerSettings _pgn252;
        private readonly Pgn254AutoSteerData _pgn254;

        public PgnAll(IPgnErrorPresenter errorPresenter)
        {
            _pgn100 = new Pgn100CorrectedPosition(errorPresenter);
            _pgn229 = new Pgn229SectionSymmetric(errorPresenter);
            _pgn235 = new Pgn235SectionDimensions(errorPresenter);
            _pgn236 = new Pgn236RelayConfig(errorPresenter);
            _pgn238 = new Pgn238MachineConfig(errorPresenter);
            _pgn239 = new Pgn239MachineData(errorPresenter);
            _pgn251 = new Pgn251AutoSteerBoardConfig(errorPresenter);
            _pgn252 = new Pgn252AutoSteerSettings(errorPresenter);
            _pgn254 = new Pgn254AutoSteerData(errorPresenter);
        }

        public Pgn100CorrectedPosition Pgn100 => _pgn100;
        public Pgn229SectionSymmetric Pgn229 => _pgn229;
        public Pgn235SectionDimensions Pgn235 => _pgn235;
        public Pgn236RelayConfig Pgn236 => _pgn236;
        public Pgn238MachineConfig Pgn238 => _pgn238;
        public Pgn239MachineData Pgn239 => _pgn239;
        public Pgn251AutoSteerBoardConfig Pgn251 => _pgn251;
        public Pgn252AutoSteerSettings Pgn252 => _pgn252;
        public Pgn254AutoSteerData Pgn254 => _pgn254;
    }
}

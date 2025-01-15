namespace AgOpenGPS
{
    public class PgnAll
    {
        private readonly Pgn254AutoSteerData _pgn254;
        private readonly Pgn252AutoSteerSettings _pgn252;

        public PgnAll(IPgnErrorPresenter errorPresenter)
        {
         _pgn254 = new Pgn254AutoSteerData(errorPresenter);
         _pgn252 = new Pgn252AutoSteerSettings(errorPresenter);
        }

        public Pgn254AutoSteerData Pgn254 => _pgn254;
        public Pgn252AutoSteerSettings Pgn252 => _pgn252;

    }
}

namespace AgOpenGPS
{
    public class PgnAll
    {
        private readonly Pgn254AutoSteerData _pgn254 = new Pgn254AutoSteerData();
        private readonly Pgn252AutoSteerSettings _pgn252 = new Pgn252AutoSteerSettings();

        public PgnAll()
        {
         _pgn254 = new Pgn254AutoSteerData();
         _pgn252 = new Pgn252AutoSteerSettings();
        }

        public Pgn254AutoSteerData Pgn254 => _pgn254;
        public Pgn252AutoSteerSettings Pgn252 => _pgn252;

    }
}

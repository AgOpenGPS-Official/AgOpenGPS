namespace AgOpenGPS.Core.Models
{
    public struct GeoCoordDir
    {
        public GeoCoordDir(GeoCoord coord, GeoDir direction)
        {
            Coord = coord;
            Direction = direction;
        }

        public GeoCoord Coord { get; }
        public GeoDir Direction { get; }
    }
}

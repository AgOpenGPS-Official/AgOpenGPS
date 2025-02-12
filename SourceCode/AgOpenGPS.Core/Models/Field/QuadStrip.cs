namespace AgOpenGPS.Core.Models
{
    public class QuadStrip
    {
        private readonly GeoPath _leftCoords = new GeoPath();
        private readonly GeoPath _rightCoords = new GeoPath();

        private QuadStrip() { }

        public QuadStrip(ColorRgb colorRgb)
        {
            ColorRgb = colorRgb;
            Area = new GeoArea(0.0);
            BoundingBox = GeoBoundingBox.CreateEmpty();
        }

        public GeoArea AddQuad(GeoCoord left, GeoCoord right)
        {
            GeoArea quadArea = new GeoArea();
            BoundingBox.Include(left);
            BoundingBox.Include(right);
            if (0 < _leftCoords.Count)
            {
                quadArea = left.QuadArea(right, _rightCoords.Last, _leftCoords.Last);
                Area += quadArea;
            }
            _leftCoords.Add(left);
            _rightCoords.Add(right);
            return quadArea;
        }

        public ColorRgb ColorRgb { get; }
        public GeoArea Area { get; private set; }
        public GeoBoundingBox BoundingBox { get; }

        public int NumberOfPairs => _leftCoords.Count;
        public int NumberOfQuads => NumberOfPairs - 1;

        public GeoCoord GetLeft(int index)
        {
            return _leftCoords[index];
        }

        public GeoCoord GetRight(int index)
        {
            return _rightCoords[index];
        }
    }

}

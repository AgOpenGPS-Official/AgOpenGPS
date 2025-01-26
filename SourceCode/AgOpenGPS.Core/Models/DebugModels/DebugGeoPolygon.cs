namespace AgOpenGPS.Core.Models
{
    public  class DebugGeoPolygon : GeoPolygon
    {
        private readonly GeoPolygon _goldenPolygon;
        private DebugGeoPolygon()
        {
        }

        public DebugGeoPolygon(GeoPolygon goldenPolygon)
        {
            _goldenPolygon = goldenPolygon;
        }

        public override void Add(GeoCoord coord)
        {
            base.Add(coord);
            DebugAsserts.AreEqual(coord, _goldenPolygon[Count - 1]);

        }

        public void AreEqual()
        {
            DebugAsserts.AreEqual(Count, _goldenPolygon.Count);
        }
    }
}

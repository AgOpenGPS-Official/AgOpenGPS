namespace AgOpenGPS.Core.Models
{
    public class DebugGeoPath : GeoPath
    {
        private readonly GeoPath _goldenPath;
        private DebugGeoPath()
        {
        }

        public DebugGeoPath(GeoPath goldenPath)
        {
            _goldenPath = goldenPath;
        }

        public override void Add(GeoCoord coord)
        {
            base.Add(coord);
            DebugAsserts.AreEqual(coord, _goldenPath[Count - 1]);

        }

        public void AssertEqual()
        {
            DebugAsserts.AreEqual(Count, _goldenPath.Count);
        }
    }
}

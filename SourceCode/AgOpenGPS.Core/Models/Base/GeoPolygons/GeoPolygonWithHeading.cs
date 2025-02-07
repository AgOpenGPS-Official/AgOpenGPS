using AgOpenGPS.Core.Models;

using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class GeoPolygonWithHeading
    {
        private readonly GeoPolygon _polygon;
        private readonly List<GeoDir> _headings;

        public GeoPolygonWithHeading()
        {
            _polygon = new GeoPolygon();
            _headings = new List<GeoDir>();
        }

        public int Count => _polygon.Count;
        public double Area => _polygon.Area;

        public GeoCoord this[int index]
        {
            get { return _polygon[index]; }
        }

        public void Add(GeoCoord geoCoord, GeoDir heading)
        {
            _polygon.Add(geoCoord);
            _headings.Add(heading);
        }

        public GeoDir GetHeading(int i)
        {
            return _headings[i];
        }

        public bool IsInside(GeoCoord coord)
        {
            return _polygon.IsInside(coord);
        }

        public bool IsFarAwayFromPath(GeoCoord coord, double dstSquared)
        {
            return _polygon.IsFarAwayFromPath(coord, dstSquared);
        }

    }

}

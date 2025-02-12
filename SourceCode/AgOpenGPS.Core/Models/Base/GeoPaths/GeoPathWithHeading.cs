using AgOpenGPS.Core.Models;

using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class GeoPathWithHeading
    {
        private readonly GeoPath _path;
        private readonly List<GeoDir> _headings;

        public GeoPathWithHeading()
        {
            _path = new GeoPath();
            _headings = new List<GeoDir>();
        }

        public int Count => _path.Count;

        public GeoCoord this[int index]
        {
            get { return _path[index]; }
        }

        public void Add(GeoCoord geoCoord, GeoDir heading)
        {
            _path.Add(geoCoord);
            _headings.Add(heading);
        }

        public GeoDir GetHeading(int i)
        {
            return _headings[i];
        }

        public bool IsFarAwayFromPath(GeoCoord coord, double minDistSquare)
        {
            return _path.IsFarAwayFromPath(coord, minDistSquare);
        }
    }
}

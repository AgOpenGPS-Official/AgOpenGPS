using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class TramLines
    {
        public TramLines()
        {
            TramList = new List<GeoPath>();
        }

        public GeoPolygon OuterTrack { get; set; }

        public GeoPolygon InnerTrack { get; set; }

        public List<GeoPath> TramList { get; set; }

        public void Clear()
        {
            OuterTrack = null;
            InnerTrack = null;
            TramList.Clear();
        }

        public bool IsEmpty => 0 == TramList.Count && null == OuterTrack;

    }

}

using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class TramLines
    {
        public TramLines()
        {
            TramList = new List<GeoCoord[]>();
        }

        public GeoCoord[] OuterTrack { get; set; }

        public GeoCoord[] InnerTrack { get; set; }

        public List<GeoCoord[]> TramList { get; set; }

        public void Clear()
        {
            OuterTrack = null;
            InnerTrack = null;
            TramList.Clear();
        }

        public bool IsEmpty => 0 == TramList.Count && null == OuterTrack;

    }

}

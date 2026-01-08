using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class TramLines
    {
        public TramLines()
        {
            FillTracks = new List<GeoCoord[]>();
        }

        public GeoCoord[] OuterTrack { get; set; }

        public GeoCoord[] InnerTrack { get; set; }

        public List<GeoCoord[]> FillTracks { get; set; }

        public void Clear()
        {
            OuterTrack = null;
            InnerTrack = null;
            FillTracks.Clear();
        }

        public bool IsEmpty => 0 == FillTracks.Count && null == OuterTrack;

    }

}

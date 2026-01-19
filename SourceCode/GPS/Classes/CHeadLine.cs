using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CHeadLine
    {
        public List<CHeadPath> tracksArr = new List<CHeadPath>();

        public int idx;

        public List<vec3> desList = new List<vec3>();

        public CHeadLine()
        {
        }

    }

    public class CHeadPath
    {
        public List<vec3> trackPts = new List<vec3>();
        public string name = "";
        public double moveDistance = 0;
        public int mode = 0;
        public int a_point = 0;

        public GeoLineSegment GetHeadPathSegment(int index)
        {
            int nextIndex = (index + 1) % trackPts.Count;
            return new GeoLineSegment(trackPts[index].ToGeoCoord(), trackPts[nextIndex].ToGeoCoord());
        }
    }
}
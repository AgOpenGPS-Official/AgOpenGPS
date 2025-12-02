using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Helpers
{
    public class GeoRefactorHelper
    {

        public static GeoLineSegment GetLineSegment(List<vec3> list, int index)
        {
            int nextIndex = (index + 1) % list.Count;
            return new GeoLineSegment(list[index].ToGeoCoord(), list[nextIndex].ToGeoCoord());
        }

        public static GeoPolygon ToGeoPolygon(List<vec3> list)
        {
            GeoPolygon p = new GeoPolygon(list.Count);
            foreach (vec3 v in list)
            {
                p.Add(v.ToGeoCoord());
            }
            return p;
        }

        public static List<vec3> ToVec3List(GeoPolygon p)
        {
            List<vec3> list = new List<vec3>(p.Count);
            for (int v = 0; v < p.Count; v++)
            {
                list.Add(new vec3());
            }
            return list;
        }
    }
}

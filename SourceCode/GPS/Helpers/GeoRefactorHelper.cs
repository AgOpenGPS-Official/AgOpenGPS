using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Helpers
{
    public class GeoRefactorHelper
    {

        public static GeoLineSegment GetLineSegment(List<vec2> list, int index)
        {
            int nextIndex = (index + 1) % list.Count;
            return new GeoLineSegment(list[index].ToGeoCoord(), list[nextIndex].ToGeoCoord());
        }

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

        public static GeoCoord[] ToGeoCoordArray(List<vec2> list)
        {
            GeoCoord[] array = new GeoCoord[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i].ToGeoCoord();
            }
            return array;
        }

        public static List<GeoCoord> ToGeoCoordList(List<vec2> list)
        {
            List<GeoCoord> geoList = new List<GeoCoord>(list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                geoList.Add(list[i].ToGeoCoord());
            }
            return geoList;
        }

        public static List<vec2> ToVec2List(List<GeoCoord> geoList)
        {
            List<vec2> list = new List<vec2>(geoList.Count);
            for (int i = 0; i < geoList.Count; i++)
            {
                list.Add(new vec2(geoList[i]));
            }
            return list;
        }

        public static GeoCoord[] ToGeoCoordArray(List<vec3> list)
        {
            GeoCoord[] array = new GeoCoord[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                array[i] = list[i].ToGeoCoord();
            }
            return array;
        }
    }
}

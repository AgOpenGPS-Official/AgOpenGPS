using AgOpenGPS.Core.Models;
using AgOpenGPS.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AgOpenGPS
{
    public partial class CBoundaryList
    {
        //area variable
        public double area;

        //boundary variables
        public bool isDriveThru;

        public void CalculateFenceLineHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = fenceLine.Count;
            vec3[] arr = new vec3[cnt];
            cnt--;
            fenceLine.CopyTo(arr);
            fenceLine.Clear();

            //first point needs last, first, second points
            vec3 pt3 = arr[0];
            pt3.heading = Math.Atan2(arr[1].easting - arr[cnt].easting, arr[1].northing - arr[cnt].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
            fenceLine.Add(pt3);

            //middle points
            for (int i = 1; i < cnt; i++)
            {
                pt3 = arr[i];
                pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                fenceLine.Add(pt3);
            }

            //last and first point
            pt3 = arr[cnt];
            pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 1].easting, arr[0].northing - arr[cnt - 1].northing);
            if (pt3.heading < 0) pt3.heading += glm.twoPI;
            fenceLine.Add(pt3);
        }

        public void FixFenceLine(int bndNum)
        {
            double spacing;
            //close if less then 20 ha, 40ha, more
            if (area < 200000) spacing = 1.1;
            else if (area < 400000) spacing = 2.2;
            else spacing = 3.3;

            if (bndNum > 0) spacing *= 0.5;

            int bndCount = fenceLine.Count;
            double distance;

            //make sure distance isn't too big between points on boundary
            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                distance = glm.Distance(fenceLine[i], fenceLine[j]);
                if (distance > spacing * 1.5)
                {
                    vec3 pointB = new vec3((fenceLine[i].easting + fenceLine[j].easting) / 2.0,
                        (fenceLine[i].northing + fenceLine[j].northing) / 2.0, fenceLine[i].heading);

                    fenceLine.Insert(j, pointB);
                    bndCount = fenceLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too big between points on boundary
            bndCount = fenceLine.Count;

            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                distance = glm.Distance(fenceLine[i], fenceLine[j]);
                if (distance > spacing * 1.6)
                {
                    vec3 pointB = new vec3((fenceLine[i].easting + fenceLine[j].easting) / 2.0,
                        (fenceLine[i].northing + fenceLine[j].northing) / 2.0, fenceLine[i].heading);

                    fenceLine.Insert(j, pointB);
                    bndCount = fenceLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too small between points on headland
            spacing *= 0.9;
            bndCount = fenceLine.Count;
            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = glm.Distance(fenceLine[i], fenceLine[i + 1]);
                if (distance < spacing)
                {
                    fenceLine.RemoveAt(i + 1);
                    bndCount = fenceLine.Count;
                    i--;
                }
            }

            //make sure headings are correct for calculated points
            CalculateFenceLineHeadings();

            double delta = 0;
            fenceLineEar?.Clear();

            for (int i = 0; i < fenceLine.Count; i++)
            {
                if (i == 0)
                {
                    fenceLineEar.Add(new vec2(fenceLine[i].easting, fenceLine[i].northing));
                    continue;
                }
                delta += (fenceLine[i - 1].heading - fenceLine[i].heading);
                if (Math.Abs(delta) > 0.005)
                {
                    fenceLineEar.Add(new vec2(fenceLine[i].easting, fenceLine[i].northing));
                    delta = 0;
                }
            }
        }

        // Warning: this function does a lot more than the name suggests
        public void CalculateFenceArea(int idx)
        {
            Debug.WriteLine("CalculateFenceArea is Called");
            GeoPolygon polygon = GeoRefactorHelper.ToGeoPolygon(fenceLine);
            polygon.RemoveSelfIntersections();
            if (polygon.Count < 1) return;
            area = polygon.Area;
            bool isClockwise = polygon.IsClockwise;
            if (idx == 0)
            {
                polygon.ForceCounterClockwiseWinding();
            }
            else
            {
                polygon.ForceClockwiseWinding();
            }
            fenceLine = GeoRefactorHelper.ToVec3List(polygon);
        }

    }
}
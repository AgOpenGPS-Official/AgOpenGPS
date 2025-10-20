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

        public void ReverseWinding()
        {
            //reverse the boundary
            int cnt = fenceLine.Count;
            vec3[] arr = new vec3[cnt];
            cnt--;
            fenceLine.CopyTo(arr);
            fenceLine.Clear();
            for (int i = cnt; i >= 0; i--)
            {
                arr[i].heading -= Math.PI;
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                fenceLine.Add(arr[i]);
            }
        }

        //obvious
        public bool CalculateFenceArea(int idx)
        {
            Debug.WriteLine("CalculateFenceArea is Called");
            RemoveSelfIntersections();
            int ptCount = fenceLine.Count;
            if (ptCount < 1) return false;

            area = 0;         // Accumulates area in the loop
            int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

            for (int i = 0; i < ptCount; j = i++)
            {
                area += (fenceLine[j].easting + fenceLine[i].easting) * (fenceLine[j].northing - fenceLine[i].northing);
            }

            bool isClockwise = area >= 0;

            area = Math.Abs(area / 2);

            //make sure is clockwise for outer counter clockwise for inner
            if ((idx == 0 && isClockwise) || (idx > 0 && !isClockwise))
            {
                ReverseWinding();
            }

            return isClockwise;
        }
        public bool RemoveSelfIntersections()
        {
            var original = new List<vec3>(fenceLine);
            int originalCount = fenceLine.Count;
            if (originalCount < 4) return false;

            // Work on a closed copy (last point == first point) to simplify segment math.
            var working = new List<vec3>(originalCount + 1);
            working.AddRange(fenceLine);
            working.Add(fenceLine[0]);

            bool removedAny = false;
            bool changed = true;
            int guard = 0;
            const int maxIterations = 128;

            while (changed && guard++ < maxIterations)
            {
                changed = false;

                for (int i = 0; i < working.Count - 1 && !changed; i++)
                {
                    vec2 a0 = working[i].ToVec2();
                    vec2 a1 = working[i + 1].ToVec2();

                    for (int j = i + 2; j < working.Count - 1; j++)
                    {
                        // Skip the segment that shares a vertex with the current one.
                        if (j == i + 1) continue;
                        if (i == 0 && j + 1 == working.Count - 1) continue;

                        vec2 b0 = working[j].ToVec2();
                        vec2 b1 = working[j + 1].ToVec2();

                        if (!TrySegmentIntersection(a0, a1, b0, b1, out vec2 intersection)) continue;

                        // Remove the loop between the two segments and insert the intersection point once.
                        working.RemoveRange(i + 1, j - i);
                        working.Insert(i + 1, new vec3(intersection.easting, intersection.northing, 0));
                        working[working.Count - 1] = new vec3(working[0]);

                        removedAny = true;
                        changed = true;
                        break;
                    }
                }
            }

            if (!removedAny) return false;

            fenceLine.Clear();

            for (int i = 0; i < working.Count - 1; i++)
            {
                vec3 candidate = working[i];

                if (fenceLine.Count > 0)
                {
                    vec3 previous = fenceLine[fenceLine.Count - 1];
                    if (Math.Abs(previous.easting - candidate.easting) < 0.001 &&
                        Math.Abs(previous.northing - candidate.northing) < 0.001)
                    {
                        continue;
                    }
                }

                fenceLine.Add(candidate);
            }

            if (fenceLine.Count > 1)
            {
                vec3 first = fenceLine[0];
                vec3 last = fenceLine[fenceLine.Count - 1];
                if (Math.Abs(first.easting - last.easting) < 0.001 &&
                    Math.Abs(first.northing - last.northing) < 0.001)
                {
                    fenceLine.RemoveAt(fenceLine.Count - 1);
                }
            }

            if (fenceLine.Count < 3)
            {
                fenceLine.Clear();
                fenceLine.AddRange(original);
                return false;
            }

            CalculateFenceLineHeadings();

            return true;
        }

        private static bool TrySegmentIntersection(vec2 a0, vec2 a1, vec2 b0, vec2 b1, out vec2 intersection)
        {
            intersection = default;

            double denominator = (a0.easting - a1.easting) * (b0.northing - b1.northing)
                - (a0.northing - a1.northing) * (b0.easting - b1.easting);

            if (Math.Abs(denominator) < 1e-6)
            {
                return false;
            }

            double t = ((a0.easting - b0.easting) * (b0.northing - b1.northing)
                - (a0.northing - b0.northing) * (b0.easting - b1.easting)) / denominator;

            double u = ((a0.easting - b0.easting) * (a0.northing - a1.northing)
                - (a0.northing - b0.northing) * (a0.easting - a1.easting)) / denominator;

            const double epsilon = 1e-6;

            if (t <= epsilon || t >= 1 - epsilon || u <= epsilon || u >= 1 - epsilon)
            {
                return false;
            }

            intersection = new vec2(
                a0.easting + t * (a1.easting - a0.easting),
                a0.northing + t * (a1.northing - a0.northing));

            return true;
        }
    }
}
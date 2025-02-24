using OpenTK.Graphics.OpenGL;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public enum TrackMode { None = 0, AB = 2, Curve = 4, bndTrackOuter = 8, bndTrackInner = 16, bndCurve = 32, waterPivot = 64};//, Heading, Circle, Spiral

    public class CTrack
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        public List<CTrk> gArr = new List<CTrk>();

        public int idx, autoTrack3SecTimer;

        public bool isLine, isAutoTrack = false, isAutoSnapToPivot = false, isAutoSnapped;

        public CTrack(FormGPS _f)
        {
            //constructor
            mf = _f;
            idx = -1;
        }

        public int FindClosestRefTrack(vec3 pivot)
        {
            if (idx < 0 || gArr.Count == 0) return -1;

            //only 1 track
            if (gArr.Count == 1) return idx;

            int trak = -1;
            int cntr = 0;

            //Count visible
            for (int i = 0; i < gArr.Count; i++)
            {
                if (gArr[i].isVisible)
                {
                    cntr++;
                    trak = i;
                }
            }

            //only 1 track visible of the group
            if (cntr == 1) return trak;

            //no visible tracks
            if (cntr == 0) return -1;

            //determine if any aligned reasonably close
            bool[] isAlignedArr = new bool[gArr.Count];
            for (int i = 0; i < gArr.Count; i++)
            {
                if (gArr[i].mode == TrackMode.Curve) isAlignedArr[i] = true;
                else
                {
                    double diff = Math.PI - Math.Abs(Math.Abs(pivot.heading - gArr[i].heading) - Math.PI);
                    if (diff < 1 || diff > 2.14)
                        isAlignedArr[i] = true;
                    else
                        isAlignedArr[i] = false;
                }
            }

            double minDistA = double.MaxValue;
            double dist;

            vec2 endPtA, endPtB;

            for (int i = 0; i < gArr.Count; i++)
            {
                if (!isAlignedArr[i]) continue;
                if (!gArr[i].isVisible) continue;

                if (gArr[i].mode == TrackMode.AB)
                {
                    double abHeading = mf.trk.gArr[i].heading;

                    endPtA.easting = mf.trk.gArr[i].ptA.easting - (Math.Sin(abHeading) * 2000);
                    endPtA.northing = mf.trk.gArr[i].ptA.northing - (Math.Cos(abHeading) * 2000);

                    endPtB.easting = mf.trk.gArr[i].ptB.easting + (Math.Sin(abHeading) * 2000);
                    endPtB.northing = mf.trk.gArr[i].ptB.northing + (Math.Cos(abHeading) * 2000);

                    //x2-x1
                    double dx = endPtB.easting - endPtA.easting;
                    //z2-z1
                    double dy = endPtB.northing - endPtA.northing;

                    dist = ((dy * mf.steerAxlePos.easting) - (dx * mf.steerAxlePos.northing) + (endPtB.easting
                                            * endPtA.northing) - (endPtB.northing * endPtA.easting))
                                                / Math.Sqrt((dy * dy) + (dx * dx));

                    dist *= dist;

                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        trak = i;
                    }
                }
                else
                {
                    for (int j = 0; j < gArr[i].curvePts.Count; j ++)
                    {

                        dist = glm.DistanceSquared(gArr[i].curvePts[j], pivot);

                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            trak = i;
                        }
                    }
                }
            }

            return trak;
        }

        public void NudgeTrack(double dist)
        {
            if (idx > -1)
            {
                if (gArr[idx].mode == TrackMode.AB)
                {
                    mf.ABLine.isABValid = false;
                    gArr[idx].nudgeDistance += mf.ABLine.isHeadingSameWay ? dist : -dist;
                }
                else
                {
                    mf.curve.isCurveValid = false;
                    gArr[idx].nudgeDistance += mf.curve.isHeadingSameWay ? dist : -dist;

                }

                //if (gArr[idx].nudgeDistance > 0.5 * mf.tool.width) gArr[idx].nudgeDistance -= mf.tool.width;
                //else if (gArr[idx].nudgeDistance < -0.5 * mf.tool.width) gArr[idx].nudgeDistance += mf.tool.width;
            }
        }

        public void NudgeDistanceReset()
        {
            if (idx > -1 && gArr.Count > 0)
            {
                if (gArr[idx].mode == TrackMode.AB)
                {
                    mf.ABLine.isABValid = false;
                }
                else
                {
                    mf.curve.isCurveValid = false;
                }

                gArr[idx].nudgeDistance = 0;
            }
        }

        public void SnapToPivot()
        {
            if (idx > -1)
            {
                NudgeTrack(gArr[idx].mode == TrackMode.AB ? mf.ABLine.distanceFromCurrentLinePivot : mf.curve.distanceFromCurrentLinePivot);
            }
        }

        public void NudgeRefTrack(double dist)
        {
            if (idx > -1)
            {
                if (gArr[idx].mode == TrackMode.AB)
                {
                    mf.ABLine.isABValid = false;
                    NudgeRefABLine( mf.ABLine.isHeadingSameWay ? dist : -dist);
                }
                else
                {
                    mf.curve.isCurveValid = false;
                    NudgeRefCurve( mf.curve.isHeadingSameWay ? dist : -dist);
                }
            }
        }

        public void NudgeRefABLine(double dist)
        {
            double head = gArr[idx].heading;

            gArr[idx].ptA.easting += (Math.Sin(head+glm.PIBy2) * (dist));
            gArr[idx].ptA.northing += (Math.Cos(head + glm.PIBy2) * (dist));

            gArr[idx].ptB.easting += (Math.Sin(head + glm.PIBy2) * (dist));
            gArr[idx].ptB.northing += (Math.Cos(head + glm.PIBy2) * (dist));
        }

        public void NudgeRefCurve(double distAway)
        {
            mf.curve.isCurveValid = false;

            List<vec3> curList = new List<vec3>();

            double distSqAway = (distAway * distAway) - 0.01;
            vec3 point;

            for (int i = 0; i < gArr[idx].curvePts.Count; i++)
            {
                point = new vec3(
                gArr[idx].curvePts[i].easting + (Math.Sin(glm.PIBy2 + gArr[idx].curvePts[i].heading) * distAway),
                gArr[idx].curvePts[i].northing + (Math.Cos(glm.PIBy2 + gArr[idx].curvePts[i].heading) * distAway),
                gArr[idx].curvePts[i].heading);
                bool Add = true;

                for (int t = 0; t < gArr[idx].curvePts.Count; t++)
                {
                    double dist = ((point.easting - gArr[idx].curvePts[t].easting) * (point.easting - gArr[idx].curvePts[t].easting))
                        + ((point.northing - gArr[idx].curvePts[t].northing) * (point.northing - gArr[idx].curvePts[t].northing));
                    if (dist < distSqAway)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add)
                {
                    if (curList.Count > 0)
                    {
                        double dist = ((point.easting - curList[curList.Count - 1].easting) * (point.easting - curList[curList.Count - 1].easting))
                            + ((point.northing - curList[curList.Count - 1].northing) * (point.northing - curList[curList.Count - 1].northing));
                        if (dist > 1.0)
                            curList.Add(point);
                    }
                    else curList.Add(point);
                }
            }

            int cnt = curList.Count;
            if (cnt > 6)
            {
                vec3[] arr = new vec3[cnt];
                curList.CopyTo(arr);

                curList.Clear();

                for (int i = 0; i < (arr.Length - 1); i++)
                {
                    arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                    if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                    if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                }

                arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;

                //replace the array
                cnt = arr.Length;
                double distance;
                double spacing = 1.2;

                //add the first point of loop - it will be p1
                curList.Add(arr[0]);

                for (int i = 0; i < cnt - 3; i++)
                {
                    // add p2
                    curList.Add(arr[i + 1]);

                    distance = glm.Distance(arr[i + 1], arr[i + 2]);

                    if (distance > spacing)
                    {
                        int loopTimes = (int)(distance / spacing + 1);
                        for (int j = 1; j < loopTimes; j++)
                        {
                            vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                            curList.Add(pos);
                        }
                    }
                }

                curList.Add(arr[cnt - 2]);
                curList.Add(arr[cnt - 1]);

                mf.curve.CalculateHeadings(ref curList);

                gArr[idx].curvePts.Clear();

                foreach (var item in curList)
                {
                    gArr[idx].curvePts.Add(new vec3(item));
                }

                //for (int i = 0; i < cnt; i++)
                //{
                //    arr[i].easting += Math.Cos(arr[i].heading) * (dist);
                //    arr[i].northing -= Math.Sin(arr[i].heading) * (dist);
                //    gArr[idx].curvePts.Add(arr[i]);
                //}
            }
        }
    }

    public class CTrk
    {
        public List<vec3> curvePts = new List<vec3>();
        public double heading;
        public string name;
        public bool isVisible;
        public vec2 ptA;
        public vec2 ptB;
        public vec2 endPtA;
        public vec2 endPtB;
        public TrackMode mode;
        public double nudgeDistance;
        public List<double> workedTracks = new List<double>();

        public CTrk()
        {
            curvePts = new List<vec3>();
            heading = 3;
            name = "New Track";
            isVisible = true;
            ptA = new vec2();
            ptB = new vec2();
            endPtA = new vec2();
            endPtB = new vec2();
            mode = TrackMode.None;
            nudgeDistance = 0;
        }

        public CTrk(CTrk _trk)
        {
            curvePts = new List<vec3>(_trk.curvePts);
            heading = _trk.heading;
            name = _trk.name;
            isVisible = _trk.isVisible;
            ptA = _trk.ptA;
            ptB = _trk.ptB;
            endPtA = new vec2();
            endPtB = new vec2();
            mode = _trk.mode;
            nudgeDistance = _trk.nudgeDistance;
        }
    }
}

        



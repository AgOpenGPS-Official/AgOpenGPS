using AgLibrary.Logging;
using AgOpenGPS.Culture;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public class CABCurve
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        //flag for starting stop adding points
        public bool isBtnTrackOn, isMakingCurve, isRecordingCurve;

        public double distanceFromCurrentLinePivot;
        public double distanceFromRefLine;

        public bool isHeadingSameWay = true, lastIsHeadingSameWay = true;

        public double howManyPathsAway, lastHowManyPathsAway;
        public vec2 refPoint1 = new vec2(1, 1), refPoint2 = new vec2(2, 2);

        private int A, B, C;
        private int rA, rB;

        public int currentLocationIndex;

        //pure pursuit values
        public vec2 goalPointCu = new vec2(0, 0);

        public vec2 radiusPointCu = new vec2(0, 0);
        public double steerAngleCu, rEastCu, rNorthCu, ppRadiusCu, manualUturnHeading;

        public bool isSmoothWindowOpen, isLooping;
        public List<vec3> smooList = new List<vec3>();

        //the list of points of curve to drive on
        public List<vec3> curList = new List<vec3>();

        //guidelines
        private List<List<vec3>> guideArr = new List<List<vec3>>();

        public bool isCurveValid;

        public double lastSecond = 0;

        public List<vec3> desList = new List<vec3>();
        public string desName = "**";

        public double pivotDistanceError, pivotDistanceErrorLast, pivotDerivative, pivotDerivativeSmoothed, lastCurveDistance = 10000;

        //derivative counters
        private int counter2;

        public double inty;

        // Should we find the global nearest curve point (instead of local) on the next search.
        private bool findGlobalNearestCurvePoint = true;
        private CancellationTokenSource cts;
        private Task<List<vec3>> build;
        private Task<List<List<vec3>>> buildList;

        public CABCurve(FormGPS _f)
        {
            //constructor
            mf = _f;
            desList.Capacity = 1024;
            curList.Capacity = 1024;
        }

        public async void BuildCurveCurrentList(vec3 pivot)
        {
            double minDistA = 1000000, minDistB;

            //move the ABLine over based on the overlap amount set in vehicle
            double widthMinusOverlap = mf.tool.width - mf.tool.overlap;

            CTrk track = mf.trk.gArr[mf.trk.idx];

            if (!isCurveValid || ((mf.secondsSinceStart - lastSecond) > 0.66 && (!mf.isBtnAutoSteerOn || mf.mc.steerSwitchHigh)))
            {
                lastSecond = mf.secondsSinceStart;

                if (track.mode != TrackMode.waterPivot)
                {
                    int refCount = track.curvePts.Count;
                    if (refCount < 2)
                    {
                        curList?.Clear();
                        return;
                    }

                    //close call hit
                    int cc = 0, dd;

                    for (int j = 0; j < refCount; j += 10)
                    {
                        double dist = ((mf.guidanceLookPos.easting - track.curvePts[j].easting)
                            * (mf.guidanceLookPos.easting - track.curvePts[j].easting))
                                        + ((mf.guidanceLookPos.northing - track.curvePts[j].northing)
                                        * (mf.guidanceLookPos.northing - track.curvePts[j].northing));
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            cc = j;
                        }
                    }

                    minDistA = minDistB = 1000000;

                    dd = cc + 7; if (dd > refCount - 1) dd = refCount;
                    cc -= 7; if (cc < 0) cc = 0;

                    //find the closest 2 points to current close call
                    for (int j = cc; j < dd; j++)
                    {
                        double dist = ((mf.guidanceLookPos.easting - track.curvePts[j].easting)
                            * (mf.guidanceLookPos.easting - track.curvePts[j].easting))
                                        + ((mf.guidanceLookPos.northing - track.curvePts[j].northing)
                                        * (mf.guidanceLookPos.northing - track.curvePts[j].northing));
                        if (dist < minDistA)
                        {
                            minDistB = minDistA;
                            rB = rA;
                            minDistA = dist;
                            rA = j;
                        }
                        else if (dist < minDistB)
                        {
                            minDistB = dist;
                            rB = j;
                        }
                    }

                    if (rA > rB) { C = rA; rA = rB; rB = C; }

                    //same way as line creation or not
                    isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - track.curvePts[rA].heading) - Math.PI) < glm.PIBy2;

                    //which side of the closest point are we on is next
                    //calculate endpoints of reference line based on closest point
                    refPoint1.easting = track.curvePts[rA].easting - (Math.Sin(track.curvePts[rA].heading) * 300.0);
                    refPoint1.northing = track.curvePts[rA].northing - (Math.Cos(track.curvePts[rA].heading) * 300.0);

                    refPoint2.easting = track.curvePts[rA].easting + (Math.Sin(track.curvePts[rA].heading) * 300.0);
                    refPoint2.northing = track.curvePts[rA].northing + (Math.Cos(track.curvePts[rA].heading) * 300.0);

                    //x2-x1
                    double dx = refPoint2.easting - refPoint1.easting;
                    //z2-z1
                    double dz = refPoint2.northing - refPoint1.northing;

                    //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                    distanceFromRefLine = ((dz * mf.guidanceLookPos.easting) - (dx * mf.guidanceLookPos.northing) + (refPoint2.easting
                                        * refPoint1.northing) - (refPoint2.northing * refPoint1.easting))
                                        / Math.Sqrt((dz * dz) + (dx * dx));
                }
                else //pivot guide list
                {
                    //cross product
                    isHeadingSameWay = ((mf.pivotAxlePos.easting - track.ptA.easting) * (mf.steerAxlePos.northing - track.ptA.northing)
                        - (mf.pivotAxlePos.northing - track.ptA.northing) * (mf.steerAxlePos.easting - track.ptA.easting)) < 0;

                    //pivot circle center
                    distanceFromRefLine = -glm.Distance(mf.guidanceLookPos, track.ptA);
                }

                distanceFromRefLine -= (0.5 * widthMinusOverlap);

                double RefDist = (distanceFromRefLine + (isHeadingSameWay ? mf.tool.offset : -mf.tool.offset) - track.nudgeDistance) / widthMinusOverlap;

                if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
                else howManyPathsAway = (int)(RefDist + 0.5);
            }

            if (!isCurveValid || howManyPathsAway != lastHowManyPathsAway || (isHeadingSameWay != lastIsHeadingSameWay && mf.tool.offset != 0))
            {
                //is boundary curve - use task
                isCurveValid = true;
                lastHowManyPathsAway = howManyPathsAway;
                lastIsHeadingSameWay = isHeadingSameWay;
                double distAway = widthMinusOverlap * howManyPathsAway + (isHeadingSameWay ? -mf.tool.offset : mf.tool.offset) + track.nudgeDistance;

                distAway += (0.5 * widthMinusOverlap);

                if (cts != null)
                {
                    cts.Cancel();
                }
                cts = new CancellationTokenSource();

                if (build != null) await build;

                build = Task.Run(() => BuildNewOffsetList(distAway, track, cts.Token), cts.Token);
                curList = await build;
                findGlobalNearestCurvePoint = true;

                if (mf.isSideGuideLines && mf.camera.camSetDistance > mf.tool.width * -400)
                {
                    if (buildList != null)
                        await buildList;
                    //build the list list of guide lines
                    buildList = Task.Run(() => BuildCurveGuidelines(distAway, mf.ABLine.numGuideLines, track, cts.Token), cts.Token);
                    guideArr = await buildList;
                }
                else
                {
                    if (buildList != null) await buildList;
                    guideArr?.Clear();
                }
            }
        }

        
        public List<vec3> BuildNewOffsetList(double distAway, CTrk track, CancellationToken ct = default)
        {
            //the list of points of curve new list from async
            List<vec3> newCurList = new List<vec3>();

            try
            {
                if (track.mode == TrackMode.AB)
                {
                    //move the curline as well. 
                    vec2 nudgePtA = new vec2(track.ptA);
                    vec2 nudgePtB = new vec2(track.ptB);

                    //depending which way you are going, the offset can be either side
                    vec2 point1 = new vec2((Math.Cos(-track.heading) * distAway) + nudgePtA.easting,
                    (Math.Sin(-track.heading) * distAway) + nudgePtA.northing);

                    vec2 point2 = new vec2((Math.Cos(-track.heading) * distAway) + nudgePtB.easting,
                    (Math.Sin(-track.heading) * distAway) + nudgePtB.northing);

                    //create the new line extent points for current ABLine based on original heading of AB line
                    double easting1 = point1.easting - (Math.Sin(track.heading) * mf.ABLine.abLength);
                    double northing1 = point1.northing - (Math.Cos(track.heading) * mf.ABLine.abLength);

                    newCurList.Add(new vec3(easting1, northing1, track.heading));

                    double easting2 = point2.easting + (Math.Sin(track.heading) * mf.ABLine.abLength);
                    double northing2 = point2.northing + (Math.Cos(track.heading) * mf.ABLine.abLength);
                    newCurList.Add(new vec3(easting2, northing2, track.heading));
                }
                else if (track.mode == TrackMode.waterPivot)
                {
                    //max 2 cm offset from correct circle or limit to 500 points
                    double Angle = glm.twoPI / Math.Min(Math.Max(Math.Ceiling(glm.twoPI / (2 * Math.Acos(1 - (0.02 / Math.Abs(distAway))))), 50), 500);//limit between 50 and 500 points

                    vec3 centerPos = new vec3(track.ptA.easting, track.ptA.northing, 0);
                    double rotation = 0;

                    while (rotation < glm.twoPI)
                    {
                        //Update the heading
                        rotation += Angle;
                        //Add the new coordinate to the path
                        newCurList.Add(new vec3(centerPos.easting + distAway * Math.Sin(rotation), centerPos.northing + distAway * Math.Cos(rotation), 0));
                    }

                    if (newCurList.Count > 1)
                    {
                        vec3[] arr = new vec3[newCurList.Count];
                        newCurList.CopyTo(arr);
                        newCurList.Clear();

                        for (int i = 0; i < (arr.Length - 1); i++)
                        {
                            arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                            if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                            if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                        }

                        arr[arr.Length - 1].heading = Math.Atan2(arr[0].easting - arr[arr.Length - 1].easting, arr[0].northing - arr[arr.Length - 1].northing);

                        newCurList.AddRange(arr);
                    }
                }
                else
                {
                    vec3 point;

                    double step = (mf.tool.width - mf.tool.overlap) * 0.48;
                    if (step > 4) step = 4;
                    if (step < 1) step = 1;

                    double distSqAway = (distAway * distAway) - 0.01;

                    int refCount = track.curvePts.Count;
                    for (int i = 0; i < refCount; i++)
                    {
                        if (ct.IsCancellationRequested)
                            break;
                        point = new vec3(
                        track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                        track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * distAway),
                        track.curvePts[i].heading);
                        bool Add = true;

                        for (int t = 0; t < refCount; t++)
                        {
                            double dist = ((point.easting - track.curvePts[t].easting) * (point.easting - track.curvePts[t].easting))
                                + ((point.northing - track.curvePts[t].northing) * (point.northing - track.curvePts[t].northing));
                            if (dist < distSqAway)
                            {
                                Add = false;
                                break;
                            }
                        }

                        if (Add)
                        {
                            if (newCurList.Count > 0)
                            {
                                double dist = ((point.easting - newCurList[newCurList.Count - 1].easting) * (point.easting - newCurList[newCurList.Count - 1].easting))
                                    + ((point.northing - newCurList[newCurList.Count - 1].northing) * (point.northing - newCurList[newCurList.Count - 1].northing));
                                if (dist > step)
                                    newCurList.Add(point);
                            }
                            else newCurList.Add(point);
                        }
                    }

                    int cnt = newCurList.Count;
                    if (cnt > 6 && !ct.IsCancellationRequested)
                    {
                        vec3[] arr = new vec3[cnt];
                        newCurList.CopyTo(arr);

                        newCurList.Clear();

                        for (int i = 0; i < (arr.Length - 1); i++)
                        {
                            if (ct.IsCancellationRequested)
                                break;
                            arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                            if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                            if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                        }

                        arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;

                        //if (mf.tool.isToolTrailing)
                        //{
                        //    //depending on hitch is different profile of draft
                        //    double hitch;
                        //    if (mf.tool.isToolTBT && mf.tool.tankTrailingHitchLength < 0)
                        //    {
                        //        hitch = mf.tool.tankTrailingHitchLength * 0.65;
                        //        hitch += mf.tool.trailingHitchLength * 0.5;
                        //    }
                        //    else hitch = mf.tool.trailingHitchLength * 1.0;// - mf.vehicle.wheelbase;

                        //    //move the line forward based on hitch length ratio
                        //    for (int i = 0; i < arr.Length; i++)
                        //    {
                        //        arr[i].easting -= Math.Sin(arr[i].heading) * (hitch);
                        //        arr[i].northing -= Math.Cos(arr[i].heading) * (hitch);
                        //    }

                        //    ////average the points over 3, center weighted
                        //    //for (int i = 1; i < arr.Length - 2; i++)
                        //    //{
                        //    //    arr2[i].easting = (arr[i - 1].easting + arr[i].easting + arr[i + 1].easting) / 3;
                        //    //    arr2[i].northing = (arr[i - 1].northing + arr[i].northing + arr[i + 1].northing) / 3;
                        //    //}

                        //    //recalculate the heading
                        //    for (int i = 0; i < (arr.Length - 1); i++)
                        //    {
                        //        arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                        //        if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                        //        if (arr[i].heading >= glm.twoPI) arr[i].heading -= glm.twoPI;
                        //    }

                        //    arr[arr.Length - 1].heading = arr[arr.Length - 2].heading;
                        //}

                        cnt = arr.Length;
                        double distance;

                        //add the first point of loop - it will be p1
                        newCurList.Add(arr[0]);
                        //newCurList.Add(arr[1]);

                        for (int i = 0; i < cnt - 3; i++)
                        {
                            if (ct.IsCancellationRequested)
                                break;
                            // add p1
                            newCurList.Add(arr[i + 1]);

                            distance = glm.Distance(arr[i + 1], arr[i + 2]);

                            if (distance > step)
                            {
                                int loopTimes = (int)(distance / step + 1);
                                for (int j = 1; j < loopTimes; j++)
                                {
                                    vec3 pos = new vec3(glm.Catmull(j / (double)(loopTimes), arr[i], arr[i + 1], arr[i + 2], arr[i + 3]));
                                    newCurList.Add(pos);
                                }
                            }
                        }

                        newCurList.Add(arr[cnt - 2]);
                        newCurList.Add(arr[cnt - 1]);

                        //to calc heading based on next and previous points to give an average heading.
                        cnt = newCurList.Count;
                        arr = new vec3[cnt];
                        cnt--;
                        newCurList.CopyTo(arr);
                        newCurList.Clear();

                        newCurList.Add(new vec3(arr[0]));

                        //middle points
                        for (int i = 1; i < cnt; i++)
                        {
                            vec3 pt3 = new vec3(arr[i])
                            {
                                heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing)
                            };
                            if (pt3.heading < 0) pt3.heading += glm.twoPI;
                            newCurList.Add(pt3);
                        }

                        int k = arr.Length - 1;
                        vec3 pt33 = new vec3(arr[k])
                        {
                            heading = Math.Atan2(arr[k].easting - arr[k - 1].easting, arr[k].northing - arr[k - 1].northing)
                        };
                        if (pt33.heading < 0) pt33.heading += glm.twoPI;
                        newCurList.Add(pt33);

                        if (!ct.IsCancellationRequested && mf.bnd.bndList.Count > 0 && !(track.mode == TrackMode.bndCurve))
                        {
                            int ptCnt = newCurList.Count - 1;

                            bool isAdding = false;
                            //end
                            while (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(newCurList[newCurList.Count - 1]))
                            {
                                if (ct.IsCancellationRequested)
                                    break;
                                isAdding = true;
                                for (int i = 1; i < 10; i++)
                                {
                                    vec3 pt = new vec3(newCurList[ptCnt]);
                                    pt.easting += (Math.Sin(pt.heading) * i * 2);
                                    pt.northing += (Math.Cos(pt.heading) * i * 2);
                                    newCurList.Add(pt);
                                }
                                ptCnt = newCurList.Count - 1;
                            }

                            if (isAdding)
                            {
                                vec3 pt = new vec3(newCurList[newCurList.Count - 1]);
                                for (int i = 1; i < 5; i++)
                                {
                                    pt.easting += (Math.Sin(pt.heading) * 2);
                                    pt.northing += (Math.Cos(pt.heading) * 2);
                                    newCurList.Add(pt);
                                }
                            }

                            isAdding = false;

                            //and the beginning
                            pt33 = new vec3(newCurList[0]);

                            while (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(newCurList[0]))
                            {
                                if (ct.IsCancellationRequested)
                                    break;
                                isAdding = true;
                                pt33 = new vec3(newCurList[0]);

                                for (int i = 1; i < 10; i++)
                                {
                                    vec3 pt = new vec3(pt33);
                                    pt.easting -= (Math.Sin(pt.heading) * i * 2);
                                    pt.northing -= (Math.Cos(pt.heading) * i * 2);
                                    newCurList.Insert(0, pt);
                                }
                            }

                            if (isAdding)
                            {
                                vec3 pt = new vec3(newCurList[0]);
                                for (int i = 1; i < 5; i++)
                                {
                                    pt.easting -= (Math.Sin(pt.heading) * 2);
                                    pt.northing -= (Math.Cos(pt.heading) * 2);
                                    newCurList.Insert(0, pt);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.EventWriter("Exception Build new offset curve" + e.ToString());
            }
            
            return newCurList;
        }

        private List<List<vec3>> BuildCurveGuidelines(double distAway, int _passes, CTrk track, CancellationToken ct)
        {
            // the listlist of all the guidelines
            List<List<vec3>> newGuideLL = new List<List<vec3>>();

            try
            {
                for (int numGuides = -_passes; numGuides <= _passes; numGuides++)
                {
                    if (ct.IsCancellationRequested)
                        break;
                    if (numGuides == 0) continue;

                    //the list of points of curve new list from async
                    List<vec3> newGuideList = new List<vec3>();

                    newGuideLL.Add(newGuideList);

                    double nextGuideDist = (mf.tool.width - mf.tool.overlap) * numGuides +
                        (isHeadingSameWay ? -mf.tool.offset : mf.tool.offset) + track.nudgeDistance;

                    //nextGuideDist += (0.5 * (mf.tool.width - mf.tool.overlap));

                    nextGuideDist += distAway;

                    double step = (mf.tool.width - mf.tool.overlap) * 0.48;
                    if (step > 4) step = 4;
                    if (step < 1) step = 1;

                    double distSqAway = (nextGuideDist * nextGuideDist) - 0.01;

                    int refCount = track.curvePts.Count;
                    for (int i = 0; i < refCount; i++)
                    {
                        if (ct.IsCancellationRequested)
                            break;

                        vec3 point = new vec3(
                        track.curvePts[i].easting + (Math.Sin(glm.PIBy2 + track.curvePts[i].heading) * nextGuideDist),
                        track.curvePts[i].northing + (Math.Cos(glm.PIBy2 + track.curvePts[i].heading) * nextGuideDist),
                        track.curvePts[i].heading);
                        bool add = true;

                        for (int t = 0; t < refCount; t++)
                        {
                            double dist = ((point.easting - track.curvePts[t].easting) * (point.easting - track.curvePts[t].easting))
                                + ((point.northing - track.curvePts[t].northing) * (point.northing - track.curvePts[t].northing));
                            if (dist < distSqAway)
                            {
                                add = false;
                                break;
                            }
                        }

                        if (add)
                        {
                            if (newGuideList.Count > 0)
                            {
                                double dist = ((point.easting - newGuideList[newGuideList.Count - 1].easting) * (point.easting - newGuideList[newGuideList.Count - 1].easting))
                                    + ((point.northing - newGuideList[newGuideList.Count - 1].northing) * (point.northing - newGuideList[newGuideList.Count - 1].northing));
                                if (dist > step)
                                {
                                    if (mf.bnd.bndList.Count > 0)
                                    {
                                        if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                                        {
                                            newGuideList.Add(point);
                                        }
                                    }
                                    else
                                    {
                                        newGuideList.Add(point);
                                    }
                                }
                            }
                            else
                            {
                                if (mf.bnd.bndList.Count > 0)
                                {
                                    if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                                    {
                                        newGuideList.Add(point);
                                    }
                                }
                                else
                                {
                                    newGuideList.Add(point);
                                }
                            }

                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.EventWriter("Exception Build new offset curve" + e.ToString());
            }

            return newGuideLL;
        }


        public void GetCurrentCurveLine(vec3 pivot, vec3 steer)
        {
            if (mf.trk.gArr[mf.trk.idx].curvePts == null || mf.trk.gArr[mf.trk.idx].curvePts.Count < 5)
            {
                if (mf.trk.gArr[mf.trk.idx].mode != TrackMode.waterPivot)
                {
                    return;
                }
            }

            double dist, dx, dz;
            //int ptCount = curList.Count;

            if (curList.Count > 0)
            {
                // Update based on autosteer settings and distance from line
                double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();
                bool ReverseHeading = mf.isReverse ? !isHeadingSameWay : isHeadingSameWay;
                
                if (mf.yt.isYouTurnTriggered && mf.yt.DistanceFromYouTurnLine())//do the pure pursuit from youTurn
                {
                    //now substitute what it thinks are AB line values with auto turn values
                    steerAngleCu = mf.yt.steerAngleYT;
                    distanceFromCurrentLinePivot = mf.yt.distanceFromCurrentLine;

                    goalPointCu = mf.yt.goalPointYT;
                    radiusPointCu.easting = mf.yt.radiusPointYT.easting;
                    radiusPointCu.northing = mf.yt.radiusPointYT.northing;
                    ppRadiusCu = mf.yt.ppRadiusYT;
                    mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);
                }
                else if (mf.isStanleyUsed)//Stanley
                {
                    mf.gyd.StanleyGuidanceCurve(pivot, steer, ref curList);
                }
                else// Pure Pursuit ------------------------------------------
                {
                    double minDistA;
                    double minDistB;

                    //If is a curve
                    if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
                    {
                        minDistB = double.MaxValue;
                        //close call hit
                        int cc, dd;

                        if (findGlobalNearestCurvePoint)
                        {
                            // When not already following some line, find the globally nearest point

                            cc = findNearestGlobalCurvePoint(pivot, 10);

                            findGlobalNearestCurvePoint = false;
                        }
                        else
                        {
                            // When already "locked" to follow some line, try to find the "local" nearest point
                            // based on the last one. This prevents jumping between lines close to each other (or crossing lines).
                            // As this is prone to find a "local minimum", this should only be used when already following some line.

                            cc = findNearestLocalCurvePoint(pivot, currentLocationIndex, goalPointDistance, ReverseHeading);
                        }

                        minDistA = double.MaxValue;

                        dd = cc + 8; if (dd > curList.Count - 1) dd = curList.Count;
                        cc -= 8; if (cc < 0) cc = 0;

                        //find the closest 2 points to current close call
                        for (int j = cc; j < dd; j++)
                        {
                            dist = glm.DistanceSquared(pivot, curList[j]);
                            if (dist < minDistA)
                            {
                                minDistB = minDistA;
                                B = A;
                                minDistA = dist;
                                A = j;
                            }
                            else if (dist < minDistB)
                            {
                                minDistB = dist;
                                B = j;
                            }
                        }

                        //just need to make sure the points continue ascending or heading switches all over the place
                        if (A > B) { C = A; A = B; B = C; }

                        currentLocationIndex = A;

                        if (A > curList.Count - 1 || B > curList.Count - 1)
                            return;
                    }
                    else
                    {
                        if (findGlobalNearestCurvePoint)
                        {
                            // When not already following some line, find the globally nearest point

                            A = findNearestGlobalCurvePoint(pivot);

                            findGlobalNearestCurvePoint = false;
                        }
                        else
                        {
                            // When already "locked" to follow some line, try to find the "local" nearest point
                            // based on the last one. This prevents jumping between lines close to each other (or crossing lines).
                            // As this is prone to find a "local minimum", this should only be used when already following some line.

                            A = findNearestLocalCurvePoint(pivot, currentLocationIndex, goalPointDistance, ReverseHeading);
                        }

                        currentLocationIndex = A;

                        if (A > curList.Count - 1)
                            return;

                        //initial forward Test if pivot InRange AB
                        if (A == curList.Count - 1) B = 0;
                        else B = A + 1;

                        if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                             curList[B].easting, curList[B].northing, pivot.easting, pivot.northing))
                            goto SegmentFound;

                        //step back one
                        if (A == 0)
                        {
                            A = curList.Count - 1;
                            B = 0;
                        }
                        else
                        {
                            A --;
                            B = A + 1;
                        }

                        if (glm.InRangeBetweenAB(curList[A].easting, curList[A].northing,
                            curList[B].easting, curList[B].northing, pivot.easting, pivot.northing))
                            goto SegmentFound;

                        //realy really lost
                        return;
                    }

                SegmentFound:

                    //get the distance from currently active AB line

                    dx = curList[B].easting - curList[A].easting;
                    dz = curList[B].northing - curList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                    //abHeading = Math.Atan2(dz, dx);

                    //how far from current AB Line is fix
                    distanceFromCurrentLinePivot = ((dz * pivot.easting) - (dx * pivot.northing) + (curList[B].easting
                                * curList[A].northing) - (curList[B].northing * curList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    //integral slider is set to 0
                    if (mf.vehicle.purePursuitIntegralGain != 0 && !mf.isReverse)
                    {
                        pivotDistanceError = distanceFromCurrentLinePivot * 0.2 + pivotDistanceError * 0.8;

                        if (counter2++ > 4)
                        {
                            pivotDerivative = pivotDistanceError - pivotDistanceErrorLast;
                            pivotDistanceErrorLast = pivotDistanceError;
                            counter2 = 0;
                            pivotDerivative *= 2;

                            //limit the derivative
                            //if (pivotDerivative > 0.03) pivotDerivative = 0.03;
                            //if (pivotDerivative < -0.03) pivotDerivative = -0.03;
                            //if (Math.Abs(pivotDerivative) < 0.01) pivotDerivative = 0;
                        }

                        //pivotErrorTotal = pivotDistanceError + pivotDerivative;

                        if (mf.isBtnAutoSteerOn && mf.avgSpeed > 2.5 && Math.Abs(pivotDerivative) < 0.1)
                        {
                            //if over the line heading wrong way, rapidly decrease integral
                            if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                            {
                                inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.04;
                            }
                            else
                            {
                                if (Math.Abs(distanceFromCurrentLinePivot) > 0.02)
                                {
                                    inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.02;
                                    if (inty > 0.2) inty = 0.2;
                                    else if (inty < -0.2) inty = -0.2;
                                }
                            }
                        }
                        else inty *= 0.95;
                    }
                    else inty = 0;

                    // ** Pure pursuit ** - calc point on ABLine closest to current position
                    double U = (((pivot.easting - curList[A].easting) * dx)
                                + ((pivot.northing - curList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    rEastCu = curList[A].easting + (U * dx);
                    rNorthCu = curList[A].northing + (U * dz);
                    manualUturnHeading = curList[A].heading;

                    int count = ReverseHeading ? 1 : -1;
                    vec3 start = new vec3(rEastCu, rNorthCu, 0);
                    double distSoFar = 0;

                    for (int i = ReverseHeading ? B : A; i < curList.Count && i >= 0;)
                    {
                        // used for calculating the length squared of next segment.
                        double tempDist = glm.Distance(start, curList[i]);

                        //will we go too far?
                        if ((tempDist + distSoFar) > goalPointDistance)
                        {
                            double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                            goalPointCu.easting = (((1 - j) * start.easting) + (j * curList[i].easting));
                            goalPointCu.northing = (((1 - j) * start.northing) + (j * curList[i].northing));
                            break;
                        }
                        else distSoFar += tempDist;
                        start = curList[i];
                        i += count;
                        if (i < 0) i = curList.Count - 1;
                        if (i > curList.Count - 1) i = 0;
                    }

                    if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
                    {
                        if (mf.isBtnAutoSteerOn && !mf.isReverse)
                        {
                            if (isHeadingSameWay)
                            {
                                if (glm.Distance(goalPointCu, curList[(curList.Count - 1)]) < 0.5)
                                {
                                    mf.btnAutoSteer.PerformClick();
                                    mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, gStr.gsPastEndOfCurve);
                                    Log.EventWriter("Autosteer Stop, Past End of Curve");

                                }
                            }
                            else
                            {
                                if (glm.Distance(goalPointCu, curList[0]) < 0.5)
                                {
                                    mf.btnAutoSteer.PerformClick();
                                    mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, gStr.gsPastEndOfCurve);
                                    Log.EventWriter("Autosteer Stop, Past End of Curve");
                                }
                            }
                        }
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = glm.DistanceSquared(goalPointCu.northing, goalPointCu.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    //double localHeading = glm.twoPI - mf.fixHeading;

                    double localHeading;
                    if (ReverseHeading) localHeading = glm.twoPI - mf.fixHeading + inty;
                    else localHeading = glm.twoPI - mf.fixHeading - inty;

                    ppRadiusCu = goalPointDistanceSquared / (2 * (((goalPointCu.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPointCu.northing - pivot.northing) * Math.Sin(localHeading))));

                    steerAngleCu = glm.toDegrees(Math.Atan(2 * (((goalPointCu.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointCu.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (mf.ahrs.imuRoll != 88888)
                        steerAngleCu += mf.ahrs.imuRoll * -mf.gyd.sideHillCompFactor;

                    if (steerAngleCu < -mf.vehicle.maxSteerAngle) steerAngleCu = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCu > mf.vehicle.maxSteerAngle) steerAngleCu = mf.vehicle.maxSteerAngle;

                    if (!isHeadingSameWay)
                        distanceFromCurrentLinePivot *= -1.0;

                    //used for acquire/hold mode
                    mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);

                    double steerHeadingError = (pivot.heading - curList[A].heading);
                    //Fix the circular error
                    if (steerHeadingError > Math.PI)
                        steerHeadingError -= Math.PI;
                    else if (steerHeadingError < -Math.PI)
                        steerHeadingError += Math.PI;

                    if (steerHeadingError > glm.PIBy2)
                        steerHeadingError -= Math.PI;
                    else if (steerHeadingError < -glm.PIBy2)
                        steerHeadingError += Math.PI;

                    mf.vehicle.modeActualHeadingError = glm.toDegrees(steerHeadingError);

                    //Convert to centimeters
                    mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
                    mf.guidanceLineSteerAngle = (short)(steerAngleCu * 100);
                }
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLinePivot = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        public void DrawCurveNew()
        {
            if (desList.Count > 0)
            {
                GL.Color3(0.95f, 0.42f, 0.750f);
                GL.LineWidth(4.0f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < desList.Count; h++) GL.Vertex3(desList[h].easting, desList[h].northing, 0);
                GL.End();

                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0F00);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(0.99f, 0.99f, 0.0);
                GL.Vertex3(desList[desList.Count - 1].easting, desList[desList.Count - 1].northing, 0);
                GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
                GL.End();

                GL.Disable(EnableCap.LineStipple);
            }
        }

        public void DrawCurve()
        {
            if (mf.trk.idx == -1) return;

            int ptCount = mf.trk.gArr[mf.trk.idx].curvePts.Count;

            if (mf.trk.gArr[mf.trk.idx].mode != TrackMode.waterPivot)
            {
                if (mf.trk.gArr[mf.trk.idx].curvePts == null || mf.trk.gArr[mf.trk.idx].curvePts.Count == 0) return;

                GL.LineWidth(4);
                GL.Color3(0.96, 0.2f, 0.2f);
                GL.Begin(PrimitiveType.Lines);

                for (int h = 0; h < ptCount; h++) GL.Vertex3(
                    mf.trk.gArr[mf.trk.idx].curvePts[h].easting,
                    mf.trk.gArr[mf.trk.idx].curvePts[h].northing,
                    0);

                GL.End();

                if (mf.font.isFontOn)
                {
                    GL.Color3(0.40f, 0.90f, 0.95f);
                    mf.font.DrawText3D(mf.trk.gArr[mf.trk.idx].ptA.easting, mf.trk.gArr[mf.trk.idx].ptA.northing, "&A");
                    mf.font.DrawText3D(mf.trk.gArr[mf.trk.idx].ptB.easting, mf.trk.gArr[mf.trk.idx].ptB.northing, "&B");
                }

                //just draw ref and smoothed line if smoothing window is open
                if (isSmoothWindowOpen)
                {
                    if (smooList == null || smooList.Count == 0) return;

                    GL.LineWidth(mf.ABLine.lineWidth);
                    GL.Color3(0.930f, 0.92f, 0.260f);
                    GL.Begin(PrimitiveType.Lines);
                    for (int h = 0; h < smooList.Count; h++) GL.Vertex3(smooList[h].easting, smooList[h].northing, 0);
                    GL.End();
                }
            }
            if (!isSmoothWindowOpen) //normal. Smoothing window is not open.
            {
                if (curList.Count > 0)
                {
                    GL.LineWidth(mf.ABLine.lineWidth*3);
                    GL.Color3(0,0,0);

                    //GL.Enable(EnableCap.LineSmooth);

                    //ablines and curves are a line - the rest a loop
                    if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                    }
                    else
                    {
                        if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.waterPivot)
                        {
                            GL.PointSize(15.0f);
                            GL.Begin(PrimitiveType.Points);
                            GL.Vertex3(mf.trk.gArr[mf.trk.idx].ptA.easting, mf.trk.gArr[mf.trk.idx].ptA.northing, 0);
                            GL.End();
                        }

                        GL.Begin(PrimitiveType.LineLoop);
                    }

                    for (int h = 0; h < curList.Count; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                    GL.End();

                    GL.LineWidth(mf.ABLine.lineWidth);
                    GL.Color3(0.95f, 0.2f, 0.95f);

                    //ablines and curves are a line - the rest a loop
                    if (mf.trk.gArr[mf.trk.idx].mode <= TrackMode.Curve)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                    }
                    else
                    {
                        if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.waterPivot)
                        {
                            GL.PointSize(15.0f);
                            GL.Begin(PrimitiveType.Points);
                            GL.Vertex3(mf.trk.gArr[mf.trk.idx].ptA.easting, mf.trk.gArr[mf.trk.idx].ptA.northing, 0);
                            GL.End();
                        }

                        GL.Begin(PrimitiveType.LineLoop);
                    }

                    for (int h = 0; h < curList.Count; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                    GL.End();

                    if (!mf.isStanleyUsed && mf.camera.camSetDistance > -200)
                    {
                        //Draw lookahead Point
                        GL.PointSize(4.0f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(1.0f, 0.95f, 0.195f);
                        GL.Vertex3(goalPointCu.easting, goalPointCu.northing, 0.0);
                        GL.End();
                    }

                    //GL.Disable(EnableCap.LineSmooth);

                    mf.yt.DrawYouTurn();

                    //GL.PointSize(3.0f);
                    //GL.Begin(PrimitiveType.Points);
                    //GL.Color3(0.920f, 0.6f, 0.950f);
                    //for (int h = 0; h < curList.Count; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                    //GL.End();
                    //GL.PointSize(1.0f);
                }
            }

            if (guideArr.Count > 0)
            {
                GL.LineWidth(mf.ABLine.lineWidth * 3);
                GL.Color3(0, 0, 0);

                if (mf.trk.gArr[mf.trk.idx].mode != TrackMode.bndCurve)
                    GL.Begin(PrimitiveType.LineStrip);
                else
                    GL.Begin(PrimitiveType.LineLoop);

                for (int i = 0; i < guideArr.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < guideArr[i].Count; h++)
                        GL.Vertex3(guideArr[i][h].easting, guideArr[i][h].northing, 0);
                    GL.End();
                }
                GL.End();

                GL.LineWidth(mf.ABLine.lineWidth);
                GL.Color4(0.2, 0.5, 0.2, 0.6);

                if (mf.trk.gArr[mf.trk.idx].mode != TrackMode.bndCurve)
                    GL.Begin(PrimitiveType.LineStrip);
                else
                    GL.Begin(PrimitiveType.LineLoop);

                for (int i = 0; i < guideArr.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < guideArr[i].Count; h++)
                        GL.Vertex3(guideArr[i][h].easting, guideArr[i][h].northing, 0);
                    GL.End();
                }
                GL.End();
            }

            GL.PointSize(1.0f);
        }

        public void BuildTram()
        {
            //if all or bnd only then make outer loop pass
            if (mf.tram.generateMode != 1)
            {
                mf.tram.BuildTramBnd();
            }
            else
            {
                mf.tram.tramBndOuterArr?.Clear();
                mf.tram.tramBndInnerArr?.Clear();
            }

            mf.tram.tramList?.Clear();
            mf.tram.tramArr?.Clear();

            if (mf.tram.generateMode == 2) return;

            bool isBndExist = mf.bnd.bndList.Count != 0;

            int refCount = mf.trk.gArr[mf.trk.idx].curvePts.Count;

            int cntr = 0;
            if (isBndExist)
            {
                if (mf.tram.generateMode == 1)
                    cntr = 0;
                else
                    cntr = 1;
            }

            double widd;

            for (int i = cntr; i <= mf.tram.passes; i++)
            {
                mf.tram.tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                mf.tram.tramList.Add(mf.tram.tramArr);

                widd = (mf.tram.tramWidth * 0.5) - mf.tram.halfWheelTrack;
                widd += (mf.tram.tramWidth * i);

                double distSqAway = widd * widd * 0.999999;

                for (int j = 0; j < refCount; j += 1)
                {
                    vec2 point = new vec2(
                    (Math.Sin(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd) + mf.trk.gArr[mf.trk.idx].curvePts[j].easting,
                    (Math.Cos(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd) + mf.trk.gArr[mf.trk.idx].curvePts[j].northing
                        );

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = ((point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting) * (point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting))
                            + ((point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing) * (point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing));
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        //a new point only every 2 meters
                        double dist = mf.tram.tramArr.Count > 0 ? ((point.easting - mf.tram.tramArr[mf.tram.tramArr.Count - 1].easting) * (point.easting - mf.tram.tramArr[mf.tram.tramArr.Count - 1].easting))
                            + ((point.northing - mf.tram.tramArr[mf.tram.tramArr.Count - 1].northing) * (point.northing - mf.tram.tramArr[mf.tram.tramArr.Count - 1].northing)) : 3.0;
                        if (dist > 2)
                        {
                            //if inside the boundary, add
                            if (!isBndExist || mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                            {
                                mf.tram.tramArr.Add(point);
                            }
                        }
                    }
                }
            }

            for (int i = cntr; i <= mf.tram.passes; i++)
            {
                mf.tram.tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                mf.tram.tramList.Add(mf.tram.tramArr);

                widd = (mf.tram.tramWidth * 0.5) + mf.tram.halfWheelTrack;
                widd += (mf.tram.tramWidth * i);
                double distSqAway = widd * widd * 0.999999;

                for (int j = 0; j < refCount; j += 1)
                {
                    vec2 point = new vec2(
                    Math.Sin(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd + mf.trk.gArr[mf.trk.idx].curvePts[j].easting,
                    Math.Cos(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd + mf.trk.gArr[mf.trk.idx].curvePts[j].northing
                        );

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = ((point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting) * (point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting))
                            + ((point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing) * (point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing));
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        //a new point only every 2 meters
                        double dist = mf.tram.tramArr.Count > 0 ? ((point.easting - mf.tram.tramArr[mf.tram.tramArr.Count - 1].easting) * (point.easting - mf.tram.tramArr[mf.tram.tramArr.Count - 1].easting))
                            + ((point.northing - mf.tram.tramArr[mf.tram.tramArr.Count - 1].northing) * (point.northing - mf.tram.tramArr[mf.tram.tramArr.Count - 1].northing)) : 3.0;
                        if (dist > 2)
                        {
                            //if inside the boundary, add
                            if (!isBndExist || mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                            {
                                mf.tram.tramArr.Add(point);
                            }
                        }
                    }
                }
            }
        }

        //for calculating for display the averaged new line
        public void SmoothAB(int smPts)
        {
            //countExit the reference list of original curve
            int cnt = mf.trk.gArr[mf.trk.idx].curvePts.Count;

            //just go back if not very long
            if (cnt < 100) return;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = mf.trk.gArr[mf.trk.idx].curvePts[s].easting;
                arr[s].northing = mf.trk.gArr[mf.trk.idx].curvePts[s].northing;
                arr[s].heading = mf.trk.gArr[mf.trk.idx].curvePts[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = mf.trk.gArr[mf.trk.idx].curvePts[s].easting;
                arr[s].northing = mf.trk.gArr[mf.trk.idx].curvePts[s].northing;
                arr[s].heading = mf.trk.gArr[mf.trk.idx].curvePts[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += mf.trk.gArr[mf.trk.idx].curvePts[j + i].easting;
                    arr[i].northing += mf.trk.gArr[mf.trk.idx].curvePts[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = mf.trk.gArr[mf.trk.idx].curvePts[i].heading;
            }

            //make a list to draw
            smooList?.Clear();

            if (arr == null || cnt < 1) return;
            if (smooList == null) return;

            for (int i = 0; i < cnt; i++)
            {
                smooList.Add(arr[i]);
            }
        }

        public void CalculateHeadings(ref List<vec3> xList)
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = xList.Count;
            if (cnt > 3)
            {
                vec3[] arr = new vec3[cnt];
                cnt--;
                xList.CopyTo(arr);
                xList.Clear();

                vec3 pt3 = arr[0];
                pt3.heading = Math.Atan2(arr[1].easting - arr[0].easting, arr[1].northing - arr[0].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                xList.Add(pt3);

                //middle points
                for (int i = 1; i < cnt; i++)
                {
                    pt3 = arr[i];
                    pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                    if (pt3.heading < 0) pt3.heading += glm.twoPI;
                    xList.Add(pt3);
                }

                pt3 = arr[arr.Length - 1];
                pt3.heading = Math.Atan2(arr[arr.Length - 1].easting - arr[arr.Length - 2].easting,
                    arr[arr.Length - 1].northing - arr[arr.Length - 2].northing);
                if (pt3.heading < 0) pt3.heading += glm.twoPI;
                xList.Add(pt3);
            }
        }

        public void MakePointMinimumSpacing(ref List<vec3> xList, double minDistance)
        {
            int cnt = xList.Count;
            if (cnt > 3)
            {
                //make sure point distance isn't too big
                for (int i = 0; i < cnt - 1; i++)
                {
                    int j = i + 1;
                    //if (j == cnt) j = 0;
                    double distance = glm.Distance(xList[i], xList[j]);
                    if (distance > minDistance)
                    {
                        vec3 pointB = new vec3((xList[i].easting + xList[j].easting) / 2.0,
                            (xList[i].northing + xList[j].northing) / 2.0,
                            xList[i].heading);

                        xList.Insert(j, pointB);
                        cnt = xList.Count;
                        i = -1;
                    }
                }
            }
        }


        //turning the visual line into the real reference line to use
        public void SaveSmoothList()
        {
            //oops no smooth list generated
            if (smooList == null) return;
            int cnt = smooList.Count;
            if (cnt == 0) return;

            //eek
            mf.trk.gArr[mf.trk.idx].curvePts?.Clear();

            //copy to an array to calculate all the new headings
            vec3[] arr = new vec3[cnt];
            smooList.CopyTo(arr);

            //calculate new headings on smoothed line
            for (int i = 1; i < cnt - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                mf.trk.gArr[mf.trk.idx].curvePts.Add(arr[i]);
            }
        }

        public bool PointOnLine(vec3 pt1, vec3 pt2, vec3 pt)
        {
            vec2 r = new vec2(0, 0);
            if (pt1.northing == pt2.northing && pt1.easting == pt2.easting) { pt1.northing -= 0.00001; }

            double U = ((pt.northing - pt1.northing) * (pt2.northing - pt1.northing)) + ((pt.easting - pt1.easting) * (pt2.easting - pt1.easting));

            double Udenom = Math.Pow(pt2.northing - pt1.northing, 2) + Math.Pow(pt2.easting - pt1.easting, 2);

            U /= Udenom;

            r.northing = pt1.northing + (U * (pt2.northing - pt1.northing));
            r.easting = pt1.easting + (U * (pt2.easting - pt1.easting));

            double minx, maxx, miny, maxy;

            minx = Math.Min(pt1.northing, pt2.northing);
            maxx = Math.Max(pt1.northing, pt2.northing);

            miny = Math.Min(pt1.easting, pt2.easting);
            maxy = Math.Max(pt1.easting, pt2.easting);
            return _ = r.northing >= minx && r.northing <= maxx && (r.easting >= miny && r.easting <= maxy);
        }

        //add extensons
        public void AddFirstLastPoints(ref List<vec3> xList)
        {
            int ptCnt = xList.Count - 1;
            vec3 start;

            if (mf.bnd.bndList.Count > 0)
            {
                for (int i = 1; i < 100; i++)
                {
                    vec3 pt = new vec3(xList[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    xList.Add(pt);
                }

                //and the beginning
                start = new vec3(xList[0]);

                for (int i = 1; i < 100; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    xList.Insert(0, pt);
                }

            }
            else
            {
                for (int i = 1; i < 300; i++)
                {
                    vec3 pt = new vec3(xList[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    xList.Add(pt);
                }

                //and the beginning
                start = new vec3(xList[0]);

                for (int i = 1; i < 300; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    xList.Insert(0, pt);
                }
            }
        }

        public void ResetCurveLine()
        {
            curList?.Clear();
            mf.trk.idx = -1;
        }

        // Searches for the nearest "global" curve point to the refPoint by checking all points of the curve.
        // Parameter "increment" added here to give possibility to make a "sparser" search (to speed it up?)
        // Return: index to the nearest point
        private int findNearestGlobalCurvePoint(vec3 refPoint, int increment = 1)
        {
            double minDist = double.MaxValue;
            int minDistIndex = 0;

            for (int i = 0; i < curList.Count; i += increment)
            {
                double dist = glm.DistanceSquared(refPoint, curList[i]);
                if (dist < minDist)
                {
                    minDist = dist;
                    minDistIndex = i;
                }
            }
            return minDistIndex;
        }

        // Searches for the nearest "local" curve point to the refPoint by traversing forward and backward on the curve
        // startIndex means the starting point (index to curList) of the search.
        // Return: index to the nearest (local) point
        private int findNearestLocalCurvePoint(vec3 refPoint, int startIndex, double minSearchDistance, bool reverseSearchDirection)
        {
            double minDist = glm.DistanceSquared(refPoint, curList[(startIndex + curList.Count) % curList.Count]);
            int minDistIndex = startIndex;

            int directionMultiplier = reverseSearchDirection ? 1 : -1;
            double distSoFar = 0;
            vec3 start = curList[startIndex];

            // Check all points' distances from the pivot inside the "look ahead"-distance and find the nearest
            int offset = 1;

            while (offset < curList.Count)
            {
                int pointIndex = (startIndex + (offset * directionMultiplier) + curList.Count) % curList.Count;  // Wrap around
                double dist = glm.DistanceSquared(refPoint, curList[pointIndex]);

                if (dist < minDist)
                {
                    minDist = dist;
                    minDistIndex = pointIndex;
                }

                distSoFar += glm.Distance(start, curList[pointIndex]);
                start = curList[pointIndex];

                offset++;

                if (distSoFar > minSearchDistance)
                {
                    break;
                }
            }

            // Continue traversing until the distance starts growing
            while (offset < curList.Count)
            {
                int pointIndex = (startIndex + (offset * directionMultiplier) + curList.Count) % curList.Count;  // Wrap around
                double dist = glm.DistanceSquared(refPoint, curList[pointIndex]);
                if (dist < minDist)
                {
                    // Getting closer
                    minDist = dist;
                    minDistIndex = pointIndex;
                }
                else
                {
                    // Getting farther, no point to continue
                    break;
                }
                offset++;
            }

            // Traverse from the start point also into another direction to be sure we choose the minimum local distance.
            // (This is also needed due to the way AB-curve is handled (the search may start one off from the last known nearest point)).
            for (offset = 1; offset < curList.Count; offset++)
            {
                int pointIndex = (startIndex + (offset * (-directionMultiplier)) + curList.Count) % curList.Count;  // Wrap around
                double dist = glm.DistanceSquared(refPoint, curList[pointIndex]);
                if (dist < minDist)
                {
                    // Getting closer
                    minDist = dist;
                    minDistIndex = pointIndex;
                }
                else
                {
                    // Getting farther, no point to continue
                    break;
                }
            }

            return minDistIndex;
        }
    }
}
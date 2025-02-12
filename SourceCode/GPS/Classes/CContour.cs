using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CContour
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public bool isContourOn, isContourBtnOn, isRightPriority = true;

        // for closest line point to current fix
        public double minDistance = 99999.0, refX, refZ;

        public double distanceFromCurrentLinePivot;

        private int A, B, C, stripNum, lastLockPt = int.MaxValue;

        public double abFixHeadingDelta, abHeading;

        public vec2 boxA = new vec2(0, 0), boxB = new vec2(0, 2);

        public bool isHeadingSameWay = true;

        public vec2 goalPointCT = new vec2(0, 0);
        public double steerAngleCT;
        public double rEastCT, rNorthCT;
        public double ppRadiusCT;

        public double pivotDistanceError, pivotDistanceErrorLast, pivotDerivative, pivotDerivativeSmoothed;

        //derivative counters
        private int counter2;

        public double inty;
        public double steerAngleSmoothed, pivotErrorTotal;
        public double distSteerError, lastDistSteerError, derivativeDistError;

        //list of strip data individual points
        public List<vec3> ptList = new List<vec3>();

        //list of the list of individual Lines for entire field
        public List<List<vec3>> stripList = new List<List<vec3>>();

        //list of points for the new contour line
        public List<vec3> ctList = new List<vec3>();

        //constructor
        public CContour(FormGPS _f)
        {
            mf = _f;
            ctList.Capacity = 128;
            ptList.Capacity = 128;
        }

        public bool isLocked = false;

        //determine closest point on left side

        //hitting the cycle lines buttons lock to current line
        public bool SetLockToLine()
        {
            if (ctList.Count > 5) isLocked = !isLocked;
            mf.SetContourLockImage(isLocked);
            return isLocked;
        }

        #region
        //double sin2HL;
        //double cos2HL;
        //double sin2HR;
        //double cos2HR;

        //if (mf.tool.toolOffset < 0)
        //{
        //    //sticks out more left
        //    sin2HL = Math.Sin(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset * 2)));
        //    cos2HL = Math.Cos(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset * 2)));

        //    sin2HR = Math.Sin(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset)));
        //    cos2HR = Math.Cos(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset)));
        //}
        //else
        //{
        //    //sticks out more right
        //    sin2HL = Math.Sin(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset)));
        //    cos2HL = Math.Cos(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset)));

        //    sin2HR = Math.Sin(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset * 2)));
        //    cos2HR = Math.Cos(pivot.heading + glm.PIBy2) * (1.33 * (toolWid + Math.Abs(mf.tool.toolOffset * 2)));
        //}

        ////narrow equipment needs bigger bounding box.
        //if (mf.tool.toolWidth < 6)
        //{
        //    sinH = Math.Sin(pivot.heading) * 4 * toolWid;
        //    cosH = Math.Cos(pivot.heading) * 4 * toolWid;
        //}

        //double sin3H = Math.Sin(pivot.heading + glm.PIBy2) * 0.5;
        //double cos3H = Math.Cos(pivot.heading + glm.PIBy2) * 0.5;

        ////build a frustum box ahead of fix to find adjacent paths and points

        ////left
        //boxA.easting = pivot.easting - sin2HL;
        //boxA.northing = pivot.northing - cos2HL;
        //boxA.easting -= (sinH * 0.25); //bottom left outside
        //boxA.northing -= (cosH * 0.25);

        //boxD.easting = boxA.easting + sinH; //top left outside
        //boxD.northing = boxA.northing + cosH;

        //boxE.easting = pivot.easting - sin3H; // inside bottom
        //boxE.northing = pivot.northing - cos3H;

        //boxG.easting = boxE.easting + (sinH * 0.3); //inside top
        //boxG.northing = boxE.northing + (cosH * 0.3);

        ////right
        //boxB.easting = pivot.easting + sin2HR;
        //boxB.northing = pivot.northing + cos2HR;
        //boxB.easting -= (sinH * 0.25);
        //boxB.northing -= (cosH * 0.25);

        //boxC.easting = boxB.easting + sinH;
        //boxC.northing = boxB.northing + cosH;

        //boxF.easting = pivot.easting + sin3H;
        //boxF.northing = pivot.northing + cos3H;

        //boxH.easting = boxF.easting + (sinH * 0.3); //inside top
        //boxH.northing = boxF.northing + (cosH * 0.3);

        //conList.Clear();
        //ctList.Clear();
        //int ptCount;

        ////check if no strips yet, return
        //int stripCount = stripList.Count;
        //if (stripCount == 0) return;

        //cvec pointC = new cvec();
        //if (isRightPriority)
        //{
        //    //determine if points are in right side frustum box
        //    for (int s = 0; s < stripCount; s++)
        //    {
        //        ptCount = stripList[s].Count;
        //        for (int p = 0; p < ptCount; p++)
        //        {
        //            //FHCBF
        //            if ((((boxH.easting - boxC.easting) * (stripList[s][p].northing - boxC.northing))
        //                    - ((boxH.northing - boxC.northing) * (stripList[s][p].easting - boxC.easting))) < 0) { continue; }

        //            if ((((boxC.easting - boxB.easting) * (stripList[s][p].northing - boxB.northing))
        //                    - ((boxC.northing - boxB.northing) * (stripList[s][p].easting - boxB.easting))) < 0) { continue; }

        //            if ((((boxB.easting - boxF.easting) * (stripList[s][p].northing - boxF.northing))
        //                    - ((boxB.northing - boxF.northing) * (stripList[s][p].easting - boxF.easting))) < 0) { continue; }

        //            if ((((boxF.easting - boxH.easting) * (stripList[s][p].northing - boxH.northing))
        //                    - ((boxF.northing - boxH.northing) * (stripList[s][p].easting - boxH.easting))) < 0) { continue; }

        //            //in the box so is it parallelish or perpedicularish to current heading
        //            ref2 = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[s][p].heading) - Math.PI);
        //            if (ref2 < 1.2 || ref2 > 1.9)
        //            {
        //                //it's in the box and parallelish so add to list
        //                pointC.x = stripList[s][p].easting;
        //                pointC.z = stripList[s][p].northing;
        //                pointC.h = stripList[s][p].heading;
        //                pointC.strip = s;
        //                pointC.pt = p;
        //                conList.Add(pointC);
        //            }
        //        }
        //    }

        //    if (conList.Count == 0)
        //    {
        //        //determine if points are in frustum box
        //        for (int s = 0; s < stripCount; s++)
        //        {
        //            ptCount = stripList[s].Count;
        //            for (int p = 0; p < ptCount; p++)
        //            {
        //                //EADGE
        //                if ((((boxG.easting - boxE.easting) * (stripList[s][p].northing - boxE.northing))
        //                        - ((boxG.northing - boxE.northing) * (stripList[s][p].easting - boxE.easting))) < 0) { continue; }

        //                if ((((boxE.easting - boxA.easting) * (stripList[s][p].northing - boxA.northing))
        //                        - ((boxE.northing - boxA.northing) * (stripList[s][p].easting - boxA.easting))) < 0) { continue; }

        //                if ((((boxA.easting - boxD.easting) * (stripList[s][p].northing - boxD.northing))
        //                        - ((boxA.northing - boxD.northing) * (stripList[s][p].easting - boxD.easting))) < 0) { continue; }

        //                if ((((boxD.easting - boxG.easting) * (stripList[s][p].northing - boxG.northing))
        //                        - ((boxD.northing - boxG.northing) * (stripList[s][p].easting - boxG.easting))) < 0) { continue; }

        //                //in the box so is it parallelish or perpedicularish to current heading
        //                ref2 = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[s][p].heading) - Math.PI);
        //                if (ref2 < 1.2 || ref2 > 1.9)
        //                {
        //                    //it's in the box and parallelish so add to list
        //                    pointC.x = stripList[s][p].easting;
        //                    pointC.z = stripList[s][p].northing;
        //                    pointC.h = stripList[s][p].heading;
        //                    pointC.strip = s;
        //                    pointC.pt = p;
        //                    conList.Add(pointC);
        //                }
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    for (int s = 0; s < stripCount; s++)
        //    {
        //        ptCount = stripList[s].Count;
        //        for (int p = 0; p < ptCount; p++)
        //        {
        //            //EADGE
        //            if ((((boxG.easting - boxE.easting) * (stripList[s][p].northing - boxE.northing))
        //                    - ((boxG.northing - boxE.northing) * (stripList[s][p].easting - boxE.easting))) < 0) { continue; }

        //            if ((((boxE.easting - boxA.easting) * (stripList[s][p].northing - boxA.northing))
        //                    - ((boxE.northing - boxA.northing) * (stripList[s][p].easting - boxA.easting))) < 0) { continue; }

        //            if ((((boxA.easting - boxD.easting) * (stripList[s][p].northing - boxD.northing))
        //                    - ((boxA.northing - boxD.northing) * (stripList[s][p].easting - boxD.easting))) < 0) { continue; }

        //            if ((((boxD.easting - boxG.easting) * (stripList[s][p].northing - boxG.northing))
        //                    - ((boxD.northing - boxG.northing) * (stripList[s][p].easting - boxG.easting))) < 0) { continue; }

        //            //in the box so is it parallelish or perpedicularish to current heading
        //            ref2 = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[s][p].heading) - Math.PI);
        //            if (ref2 < 1.2 || ref2 > 1.9)
        //            {
        //                //it's in the box and parallelish so add to list
        //                pointC.x = stripList[s][p].easting;
        //                pointC.z = stripList[s][p].northing;
        //                pointC.h = stripList[s][p].heading;
        //                pointC.strip = s;
        //                pointC.pt = p;
        //                conList.Add(pointC);
        //            }
        //        }
        //    }

        //    if (conList.Count == 0)
        //    {
        //        //determine if points are in frustum box
        //        for (int s = 0; s < stripCount; s++)
        //        {
        //            ptCount = stripList[s].Count;
        //            for (int p = 0; p < ptCount; p++)
        //            {
        //                if ((((boxH.easting - boxC.easting) * (stripList[s][p].northing - boxC.northing))
        //                        - ((boxH.northing - boxC.northing) * (stripList[s][p].easting - boxC.easting))) < 0) { continue; }

        //                if ((((boxC.easting - boxB.easting) * (stripList[s][p].northing - boxB.northing))
        //                        - ((boxC.northing - boxB.northing) * (stripList[s][p].easting - boxB.easting))) < 0) { continue; }

        //                if ((((boxB.easting - boxF.easting) * (stripList[s][p].northing - boxF.northing))
        //                        - ((boxB.northing - boxF.northing) * (stripList[s][p].easting - boxF.easting))) < 0) { continue; }

        //                if ((((boxF.easting - boxH.easting) * (stripList[s][p].northing - boxH.northing))
        //                        - ((boxF.northing - boxH.northing) * (stripList[s][p].easting - boxH.easting))) < 0) { continue; }

        //                //in the box so is it parallelish or perpedicularish to current heading
        //                ref2 = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[s][p].heading) - Math.PI);
        //                if (ref2 < 1.2 || ref2 > 1.9)
        //                {
        //                    //it's in the box and parallelish so add to list
        //                    pointC.x = stripList[s][p].easting;
        //                    pointC.z = stripList[s][p].northing;
        //                    pointC.h = stripList[s][p].heading;
        //                    pointC.strip = s;
        //                    pointC.pt = p;
        //                    conList.Add(pointC);
        //                }
        //            }
        //        }
        //    }
        //}
        #endregion
        private double lastSecond;
        private int pt = 0;

        public void BuildContourGuidanceLine(vec3 pivot)
        {
            if (ctList.Count == 0)
            {
                if ((mf.secondsSinceStart - lastSecond) < 0.3) return;
            }
            else
            {
                if ((mf.secondsSinceStart - lastSecond) < 2) return;
            }

            lastSecond = mf.secondsSinceStart;
            int ptCount;
            minDistance = double.MaxValue;
            int start, stop;

            double toolContourDistance = (mf.tool.width * 3 + Math.Abs(mf.tool.offset));

            //check if no strips yet, return
            int stripCount = stripList.Count;

            //if making a new strip ignore it or it will win always
            //stripCount--;
            if (stripCount < 1) return;

            double sinH = Math.Sin(pivot.heading) * 0.2;
            double cosH = Math.Cos(pivot.heading) * 0.2;

            double sin2HL = Math.Sin(pivot.heading + glm.PIBy2);
            double cos2HL = Math.Cos(pivot.heading + glm.PIBy2);

            boxA.easting = pivot.easting - sin2HL + sinH;
            boxA.northing = pivot.northing - cos2HL + cosH;

            boxB.easting = pivot.easting + sin2HL + sinH;
            boxB.northing = pivot.northing + cos2HL + cosH;

            if (!isLocked)
            {
                stripNum = -1;
                for (int s = 0; s < stripCount; s++)
                {
                    int p;
                    ptCount = stripList[s].Count;
                    if (ptCount == 0) continue;
                    double dist;
                    for (p = 0; p < ptCount; p += 3)
                    {
                        //if (s == stripCount - 1)
                        {
                            if ((((boxA.easting - boxB.easting) * (stripList[s][p].northing - boxB.northing))
                                    - ((boxA.northing - boxB.northing) * (stripList[s][p].easting - boxB.easting))) > 0)
                            {
                                continue;
                            }
                        }

                        dist = ((pivot.easting - stripList[s][p].easting) * (pivot.easting - stripList[s][p].easting))
                            + ((pivot.northing - stripList[s][p].northing) * (pivot.northing - stripList[s][p].northing));
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            stripNum = s;
                            pt = lastLockPt = p;
                            //B = p;
                        }
                    }
                }
                minDistance = Math.Sqrt(minDistance);

                if (stripNum < 0 || minDistance > toolContourDistance || stripList[stripNum].Count < 4)
                {
                    //no points in the box, exit
                    ctList.Clear();
                    isLocked = false;
                    mf.SetContourLockImage(isLocked);
                    return;
                }
            }

            //locked to this stripNum so find closest within a range
            else
            {
                //no points in the box, exit
                ptCount = stripList[stripNum].Count;

                if (ptCount < 2)
                {
                    ctList.Clear();
                    isLocked = false;
                    mf.SetContourLockImage(isLocked);
                    return;
                }

                start = lastLockPt - 20; if (start < 0) start = 0;
                stop = lastLockPt + 20; if (stop > ptCount) stop = ptCount;

                //determine closest point
                minDistance = double.MaxValue;

                //if being built, start high, keep from guiding latest points made
                //int currentStripBox = 0;
                //if (stripNum == stripCount) currentStripBox = 10;
                for (int i = start; i < stop; i += 3)
                {
                    double dist = ((pivot.easting - stripList[stripNum][i].easting) * (pivot.easting - stripList[stripNum][i].easting))
                        + ((pivot.northing - stripList[stripNum][i].northing) * (pivot.northing - stripList[stripNum][i].northing));

                    if (minDistance >= dist)
                    {
                        minDistance = dist;
                        pt = lastLockPt = i;
                    }
                }

                minDistance = Math.Sqrt(minDistance);

                if (minDistance > toolContourDistance)
                {
                    ctList.Clear();
                    isLocked = false;
                    mf.SetContourLockImage(isLocked);
                    return;
                }
            }

            //now we have closest point, the distance squared from it, and which patch and point its from
            refX = stripList[stripNum][pt].easting;
            refZ = stripList[stripNum][pt].northing;

            double dx, dz, distanceFromRefLine;

            if (pt < stripList[stripNum].Count - 1)
            {
                dx = stripList[stripNum][pt + 1].easting - refX;
                dz = stripList[stripNum][pt + 1].northing - refZ;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (stripList[stripNum][pt + 1].easting
                                        * refZ) - (stripList[stripNum][pt + 1].northing * refX))
                                        / Math.Sqrt((dz * dz) + (dx * dx));
            }
            else if (pt > 0)
            {
                dx = refX - stripList[stripNum][pt - 1].easting;
                dz = refZ - stripList[stripNum][pt - 1].northing;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dz * pivot.easting) - (dx * pivot.northing) + (refX
                                        * stripList[stripNum][pt - 1].northing) - (refZ * stripList[stripNum][pt - 1].easting))
                                        / Math.Sqrt((dz * dz) + (dx * dx));
            }
            else return;

            //are we going same direction as stripList was created?
            bool isSameWay = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[stripNum][pt].heading) - Math.PI) < 1.57;

            double RefDist = (distanceFromRefLine + (isSameWay ? mf.tool.offset : -mf.tool.offset))
                                / (mf.tool.width - mf.tool.overlap);

            double howManyPathsAway;

            if (Math.Abs(distanceFromRefLine) > mf.tool.halfWidth
                || Math.Abs(mf.tool.offset) > mf.tool.halfWidth)
            {
                //beside what is done
                if (RefDist < 0) howManyPathsAway = -1;
                else howManyPathsAway = 1;
            }
            else
            {
                //driving on what is done
                howManyPathsAway = 0;
            }

            if (howManyPathsAway >= -1 && howManyPathsAway <= 1)
            {
                ctList.Clear();

                //don't guide behind yourself
                //if (stripNum == stripList.Count-1 && howManyPathsAway == 0)
                //return;

                //make the new guidance line list called guideList
                ptCount = stripList[stripNum].Count;

                //shorter behind you
                if (isSameWay)
                {
                    start = pt - 20; if (start < 0) start = 0;
                    stop = pt + 70; if (stop > ptCount) stop = ptCount;
                }
                else
                {
                    start = pt - 70; if (start < 0) start = 0;
                    stop = pt + 20; if (stop > ptCount) stop = ptCount;
                }

                //if (howManyPathsAway != 0 && (mf.tool.halfToolWidth < (0.5*mf.tool.toolOffset)))
                {
                    double distAway = (mf.tool.width - mf.tool.overlap) * howManyPathsAway
                        + (isSameWay ? -mf.tool.offset : mf.tool.offset);
                    double distSqAway = (distAway * distAway) * 0.97;

                    for (int i = start; i < stop; i++)
                    {
                        vec3 point = new vec3(
                            stripList[stripNum][i].easting + (Math.Cos(stripList[stripNum][i].heading) * distAway),
                            stripList[stripNum][i].northing - (Math.Sin(stripList[stripNum][i].heading) * distAway),
                            stripList[stripNum][i].heading);

                        bool isOkToAdd = true;
                        //make sure its not closer then 1 eq width
                        for (int j = start; j < stop; j++)
                        {
                            double check = glm.DistanceSquared(point.northing, point.easting,
                                stripList[stripNum][j].northing, stripList[stripNum][j].easting);
                            if (check < distSqAway)
                            {
                                isOkToAdd = false;
                                break;
                            }
                        }

                        if (isOkToAdd)
                        {
                            if (ctList.Count > 0)
                            {
                                double dist =
                                    ((point.easting - ctList[ctList.Count - 1].easting) * (point.easting - ctList[ctList.Count - 1].easting))
                                    + ((point.northing - ctList[ctList.Count - 1].northing) * (point.northing - ctList[ctList.Count - 1].northing));
                                if (dist > 0.2)
                                    ctList.Add(point);
                            }
                            else ctList.Add(point);
                        }
                    }
                }

                int ptc = ctList.Count;
                if (ptc < 5)
                {
                    ctList.Clear();
                    isLocked = false;
                    mf.SetContourLockImage(isLocked);
                    return;
                }
            }
            else
            {
                ctList.Clear();
                isLocked = false;
                mf.SetContourLockImage(isLocked);
                return;
            }
        }

        //determine distance from contour guidance line
        public void DistanceFromContourLine(vec3 pivot, vec3 steer)
        {
            double minDistA = 1000000, minDistB = 1000000;
            int ptCount = ctList.Count;
            //distanceFromCurrentLine = 9999;
            if (ptCount > 8)
            {
                if (mf.isStanleyUsed)
                {
                    //find the closest 2 points to current fix
                    for (int t = 0; t < ptCount; t++)
                    {
                        double dist = ((steer.easting - ctList[t].easting) * (steer.easting - ctList[t].easting))
                                        + ((steer.northing - ctList[t].northing) * (steer.northing - ctList[t].northing));
                        if (dist < minDistA)
                        {
                            minDistB = minDistA;
                            B = A;
                            minDistA = dist;
                            A = t;
                        }
                        else if (dist < minDistB)
                        {
                            minDistB = dist;
                            B = t;
                        }
                    }

                    //just need to make sure the points continue ascending in list order or heading switches all over the place
                    if (A > B) { C = A; A = B; B = C; }

                    //get the distance from currently active AB line
                    //x2-x1
                    double dx = ctList[B].easting - ctList[A].easting;
                    //z2-z1
                    double dy = ctList[B].northing - ctList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon) return;

                    //how far from current AB Line is fix
                    distanceFromCurrentLinePivot = ((dy * steer.easting) - (dx * steer.northing) + (ctList[B].easting
                                * ctList[A].northing) - (ctList[B].northing * ctList[A].easting))
                                    / Math.Sqrt((dy * dy) + (dx * dx));

                    abHeading = Math.Atan2(dx, dy);
                    if (abHeading < 0) abHeading += glm.twoPI;

                    isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - abHeading) - Math.PI) < glm.PIBy2;

                    // calc point on ABLine closest to current position
                    double U = (((steer.easting - ctList[A].easting) * dx) + ((steer.northing - ctList[A].northing) * dy))
                                / ((dx * dx) + (dy * dy));

                    rEastCT = ctList[A].easting + (U * dx);
                    rNorthCT = ctList[A].northing + (U * dy);

                    //distance is negative if on left, positive if on right
                    if (isHeadingSameWay)
                    {
                        abFixHeadingDelta = (steer.heading - abHeading);
                    }
                    else
                    {
                        distanceFromCurrentLinePivot *= -1.0;
                        abFixHeadingDelta = (steer.heading - abHeading + Math.PI);
                    }

                    //Fix the circular error
                    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;

                    if (abFixHeadingDelta > glm.PIBy2) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < -glm.PIBy2) abFixHeadingDelta += Math.PI;

                    if (mf.isReverse) abFixHeadingDelta *= -1;

                    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    steerAngleCT = Math.Atan((distanceFromCurrentLinePivot * mf.vehicle.stanleyDistanceErrorGain)
                        / ((Math.Abs(mf.avgSpeed) * 0.277777) + 1));

                    if (steerAngleCT > 0.74) steerAngleCT = 0.74;
                    if (steerAngleCT < -0.74) steerAngleCT = -0.74;

                    steerAngleCT = glm.toDegrees((steerAngleCT + abFixHeadingDelta) * -1.0);

                    if (steerAngleCT < -mf.vehicle.maxSteerAngle) steerAngleCT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCT > mf.vehicle.maxSteerAngle) steerAngleCT = mf.vehicle.maxSteerAngle;
                }
                else
                {
                    //find the closest 2 points to current fix
                    for (int t = 0; t < ptCount; t++)
                    {
                        double dist = ((pivot.easting - ctList[t].easting) * (pivot.easting - ctList[t].easting))
                                        + ((pivot.northing - ctList[t].northing) * (pivot.northing - ctList[t].northing));
                        if (dist < minDistA)
                        {
                            minDistB = minDistA;
                            B = A;
                            minDistA = dist;
                            A = t;
                        }
                        else if (dist < minDistB)
                        {
                            minDistB = dist;
                            B = t;
                        }
                    }

                    //just need to make sure the points continue ascending in list order or heading switches all over the place
                    if (A > B) { C = A; A = B; B = C; }

                    if (isLocked && (A < 2 || B > ptCount - 3))
                    {
                        //ctList.Clear();
                        isLocked = false;
                        mf.SetContourLockImage(isLocked);
                        lastLockPt = int.MaxValue;
                        return;
                    }

                    //get the distance from currently active AB line
                    //x2-x1
                    double dx = ctList[B].easting - ctList[A].easting;
                    //z2-z1
                    double dy = ctList[B].northing - ctList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon) return;

                    //how far from current AB Line is fix
                    distanceFromCurrentLinePivot = ((dy * mf.pn.fix.easting) - (dx * mf.pn.fix.northing) + (ctList[B].easting
                                * ctList[A].northing) - (ctList[B].northing * ctList[A].easting))
                                    / Math.Sqrt((dy * dy) + (dx * dx));

                    //integral slider is set to 0
                    if (mf.vehicle.purePursuitIntegralGain != 0)
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

                        if (mf.isBtnAutoSteerOn
                            && Math.Abs(pivotDerivative) < (0.1)
                            && mf.avgSpeed > 2.5
                            && !mf.yt.isYouTurnTriggered)
                        {
                            //if over the line heading wrong way, rapidly decrease integral
                            if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                            {
                                inty += pivotDistanceError * mf.vehicle.purePursuitIntegralGain * -0.06;
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

                    if (mf.isReverse) inty = 0;

                    isHeadingSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - ctList[A].heading) - Math.PI) < glm.PIBy2;

                    if (!isHeadingSameWay)
                        distanceFromCurrentLinePivot *= -1.0;

                    // ** Pure pursuit ** - calc point on ABLine closest to current position
                    double U = (((pivot.easting - ctList[A].easting) * dx) + ((pivot.northing - ctList[A].northing) * dy))
                            / ((dx * dx) + (dy * dy));

                    rEastCT = ctList[A].easting + (U * dx);
                    rNorthCT = ctList[A].northing + (U * dy);

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

                    bool ReverseHeading = mf.isReverse ? !isHeadingSameWay : isHeadingSameWay;

                    int count = ReverseHeading ? 1 : -1;
                    vec3 start = new vec3(rEastCT, rNorthCT, 0);
                    double distSoFar = 0;

                    for (int i = ReverseHeading ? B : A; i < ptCount && i >= 0; i += count)
                    {
                        // used for calculating the length squared of next segment.
                        double tempDist = glm.Distance(start, ctList[i]);

                        //will we go too far?
                        if ((tempDist + distSoFar) > goalPointDistance)
                        {
                            double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                            goalPointCT.easting = (((1 - j) * start.easting) + (j * ctList[i].easting));
                            goalPointCT.northing = (((1 - j) * start.northing) + (j * ctList[i].northing));
                            break;
                        }
                        else distSoFar += tempDist;
                        start = ctList[i];
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = glm.DistanceSquared(goalPointCT.northing, goalPointCT.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading;// = glm.twoPI - mf.fixHeading;

                    if (isHeadingSameWay) localHeading = glm.twoPI - mf.fixHeading + inty;
                    else localHeading = glm.twoPI - mf.fixHeading - inty;

                    steerAngleCT = glm.toDegrees(Math.Atan(2 * (((goalPointCT.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointCT.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.VehicleConfig.Wheelbase / goalPointDistanceSquared));

                    if (mf.ahrs.imuRoll != 88888)
                        steerAngleCT += mf.ahrs.imuRoll * -mf.gyd.sideHillCompFactor;

                    if (steerAngleCT < -mf.vehicle.maxSteerAngle) steerAngleCT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCT > mf.vehicle.maxSteerAngle) steerAngleCT = mf.vehicle.maxSteerAngle;

                    //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                    //double angVel = glm.twoPI * 0.277777 * mf.avgSpeed * (Math.Tan(glm.toRadians(steerAngleCT))) / mf.vehicle.wheelbase;

                    //clamp the steering angle to not exceed safe angular velocity
                    //if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
                    //{
                    //    steerAngleCT = glm.toDegrees(steerAngleCT > 0 ?
                    //            (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777)))
                    //        : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.avgSpeed * 0.277777))));
                    //}
                }

                //used for smooth mode
                mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);

                //fill in the autosteer variables
                mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
                mf.guidanceLineSteerAngle = (short)(steerAngleCT * 100);
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLinePivot = 0;
                mf.guidanceLineDistanceOff = 0;
            }
        }

        //start stop and add points to list
        public void StartContourLine()
        {
            //if (stripList.Count == 0)
            //{
            //make new ptList
            ptList = new List<vec3>(16);
            //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading)
            //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
            stripList.Add(ptList);
            isContourOn = true;
            return;
            //}
            //else
            //{
            //    //reuse ptList
            //    ptList?.Clear();
            //    //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading)
            //    //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
            //    isContourOn = true;
            //}
        }

        //Add current position to stripList
        public void AddPoint(vec3 pivot)
        {
            ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * mf.tool.offset,
                pivot.northing - Math.Sin(pivot.heading) * mf.tool.offset,
                pivot.heading));
        }

        //End the strip
        public void StopContourLine()
        {
            //make sure its long enough to bother
            if (ptList.Count > 5)
            {
                //ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading)
                //    * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));

                ////build tale
                //double head = ptList[0].heading;
                //int length = (int)(mf.tool.toolWidth*0.5);
                //vec3 pnt;
                //for (int a = 0; a < length; a ++)
                //{
                //    pnt.easting = ptList[0].easting - (Math.Sin(head));
                //    pnt.northing = ptList[0].northing - (Math.Cos(head));
                //    pnt.heading = ptList[0].heading;
                //    ptList.Insert(0, pnt);
                //}

                //int ptc = ptList.Count - 1;
                //head = ptList[ptc].heading;

                //for (double i = 1; i < length; i ++)
                //{
                //    pnt.easting = ptList[ptc].easting + (Math.Sin(head) * i);
                //    pnt.northing = ptList[ptc].northing + (Math.Cos(head) * i);
                //    pnt.heading = head;
                //    ptList.Add(pnt);
                //}

                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(ptList);

                //ptList = new List<vec3>(32);
                //stripList.Add(ptList);
            }

            //delete ptList
            else
            {
                ptList.Clear();
            }

            //turn it off
            isContourOn = false;
        }

        //build contours for boundaries
        public void BuildFenceContours(double spacingInt)
        {
            spacingInt *= 0.01;
            if (mf.bnd.bndList.Count == 0)
            {
                mf.TimedMessageBox(1500, "Boundary Contour Error", "No Boundaries Made");
                return;
            }

            if (mf.patchCounter != 0)
            {
                mf.TimedMessageBox(1500, "Section Control On", "Turn Off Section Control");
                return;
            }

            vec3 point = new vec3();
            double totalHeadWidth;
            int signPass;

            signPass = -1;
            //determine how wide a headland space
            totalHeadWidth = ((mf.tool.width - mf.tool.overlap) * 0.5) - spacingInt;

            //totalHeadWidth = (mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5 + 0.2 + (mf.tool.toolWidth - mf.tool.toolOverlap);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                //countExit the points from the boundary
                int ptCount = mf.bnd.bndList[j].fenceLine.Count;

                ptList = new List<vec3>(128);
                stripList.Add(ptList);

                for (int i = ptCount - 1; i >= 0; i--)
                {
                    //calculate the point inside the boundary
                    point.easting = mf.bnd.bndList[j].fenceLine[i].easting - (signPass * Math.Sin(glm.PIBy2 + mf.bnd.bndList[j].fenceLine[i].heading) * totalHeadWidth);
                    point.northing = mf.bnd.bndList[j].fenceLine[i].northing - (signPass * Math.Cos(glm.PIBy2 + mf.bnd.bndList[j].fenceLine[i].heading) * totalHeadWidth);
                    point.heading = mf.bnd.bndList[j].fenceLine[i].heading - Math.PI;
                    if (point.heading < -glm.twoPI) point.heading += glm.twoPI;

                    ptList.Add(point);
                }
            }

            mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
            //GL.Color3(0.98f, 0.98f, 0.50f);
            //GL.Begin(PrimitiveType.Lines);
            //GL.Vertex3(boxA.easting, boxA.northing, 0);
            //GL.Vertex3(boxB.easting, boxB.northing, 0);
            //GL.End();

            ////draw the guidance line
            int ptCount = ctList.Count;
            if (ptCount < 2) return;
            GL.LineWidth(mf.ABLine.lineWidth);
            GL.Color3(0.98f, 0.2f, 0.980f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(ctList[h].easting, ctList[h].northing, 0);
            GL.End();

            GL.PointSize(mf.ABLine.lineWidth);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.87f, 08.7f, 0.25f);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(ctList[h].easting, ctList[h].northing, 0);

            GL.End();

            //Draw the captured ref strip, red if locked
            if (isLocked)
            {
                GL.Color3(0.983f, 0.92f, 0.420f);
                GL.LineWidth(4);
            }
            else
            {
                GL.Color3(0.3f, 0.982f, 0.0f);
                GL.LineWidth(mf.ABLine.lineWidth);
            }

            //GL.PointSize(6.0f);
            if (stripNum > -1)
            {
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < stripList[stripNum].Count; h++) GL.Vertex3(stripList[stripNum][h].easting, stripList[stripNum][h].northing, 0);
                GL.End();
            }

            //GL.Begin(PrimitiveType.Points);
            //GL.Color3(1.0f, 0.95f, 0.095f);
            //GL.Vertex3(rEastCT, rNorthCT, 0.0);
            //GL.End();
            //GL.PointSize(1.0f);

            //GL.Begin(PrimitiveType.LineStrip);
            //GL.Vertex3(boxF.easting, boxF.northing, 0);
            //GL.Vertex3(boxH.easting, boxH.northing, 0);
            //GL.Vertex3(boxC.easting, boxC.northing, 0);
            //GL.Vertex3(boxB.easting, boxB.northing, 0);
            //GL.Vertex3(boxF.easting, boxF.northing, 0);
            //GL.End();

            ////draw the reference line
            //GL.PointSize(3.0f);
            ////if (isContourBtnOn)
            //{
            //    ptCount = stripList.Count;
            //    if (ptCount > 0)
            //    {
            //        ptCount = stripList[closestRefPatch].Count;
            //        GL.Begin(PrimitiveType.Points);
            //        for (int i = 0; i < ptCount; i++)
            //        {
            //            GL.Vertex2(stripList[closestRefPatch][i].easting, stripList[closestRefPatch][i].northing);
            //        }
            //        GL.End();
            //    }
            //}

            //ptCount = conList.Count;
            //if (ptCount > 0)
            //{
            //    //draw closest point and side of line points
            //    GL.Color3(0.5f, 0.900f, 0.90f);
            //    GL.PointSize(4.0f);
            //    GL.Begin(PrimitiveType.Points);
            //    for (int i = 0; i < ptCount; i++) GL.Vertex3(conList[i].x, conList[i].z, 0);
            //    GL.End();

            GL.Color3(0.35f, 0.30f, 0.90f);
            GL.PointSize(6.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(stripList[stripNum][pt].easting, stripList[stripNum][pt].northing, 0);
            GL.End();
            //}

            if (mf.isPureDisplayOn && distanceFromCurrentLinePivot != 32000 && !mf.isStanleyUsed)
            {
                //if (ppRadiusCT < 50 && ppRadiusCT > -50)
                //{
                //    const int numSegments = 100;
                //    double theta = glm.twoPI / numSegments;
                //    double c = Math.Cos(theta);//precalculate the sine and cosine
                //    double s = Math.Sin(theta);
                //    double x = ppRadiusCT;//we start at angle = 0
                //    double y = 0;

                //    GL.LineWidth(1);
                //    GL.Color3(0.795f, 0.230f, 0.7950f);
                //    GL.Begin(PrimitiveType.LineLoop);
                //    for (int ii = 0; ii < numSegments; ii++)
                //    {
                //        //glVertex2f(x + cx, y + cy);//output vertex
                //        GL.Vertex3(x + radiusPointCT.easting, y + radiusPointCT.northing, 0);//output vertex

                //        //apply the rotation matrix
                //        double t = x;
                //        x = (c * x) - (s * y);
                //        y = (s * t) + (c * y);
                //    }
                //    GL.End();
                //}

                //Draw lookahead Point
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(1.0f, 0.95f, 0.095f);
                GL.Vertex3(goalPointCT.easting, goalPointCT.northing, 0.0);
                GL.End();
                GL.PointSize(1.0f);
            }
        }

        //Reset the contour to zip
        public void ResetContour()
        {
            stripList.Clear();
            ptList?.Clear();
            ctList?.Clear();
        }
    }//class
}//namespace
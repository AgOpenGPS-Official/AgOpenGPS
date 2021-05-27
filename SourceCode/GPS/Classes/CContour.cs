﻿using OpenTK.Graphics.OpenGL;
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

        private int A, B, C, stripNum;

        public double abFixHeadingDelta, abHeading;

        public bool isABSameAsVehicleHeading = true;

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
        }

        public bool isLocked = false;

        //determine closest point on left side

        //hitting the cycle lines buttons lock to current line
        public void SetLockToLine()
        {
            if (ctList.Count > 5) isLocked = !isLocked;
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
        public void BuildContourGuidanceLine(vec3 pivot)
        {
            if ((mf.secondsSinceStart - lastSecond) < 3) return;

            lastSecond = mf.secondsSinceStart;

            int ptCount;
            double minDistA = double.MaxValue;

            //check if no strips yet, return
            int stripCount = stripList.Count;

            //if making a new strip ignore it or it will win always
            if (mf.sectionCounter > 0) stripCount--;
            if (stripCount < 1) return;

            if (!isLocked)
            {
                for (int s = 0; s < stripCount; s++)
                {
                    ptCount = stripList[s].Count;
                    for (int p = 0; p < ptCount; p += 3)
                    {
                        double dist = ((pivot.easting - stripList[s][p].easting) * (pivot.easting - stripList[s][p].easting))
                            + ((pivot.northing - stripList[s][p].northing) * (pivot.northing - stripList[s][p].northing));
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            stripNum = s;
                            B = p;
                        }
                    }
                }
            }

            //no points in the box, exit
            ptCount = stripList[stripNum].Count;
            if (ptCount < 2)
            {
                ctList.Clear();
                isLocked = false;
                return;
            }

            //determine closest point
            minDistance = 99999;
            int pt = 0;
            for (int i = 0; i < ptCount; i++)
            {
                double dist = ((pivot.easting - stripList[stripNum][i].easting) * (pivot.easting - stripList[stripNum][i].easting))
                    + ((pivot.northing - stripList[stripNum][i].northing) * (pivot.northing - stripList[stripNum][i].northing));

                if (minDistance >= dist)
                {
                    minDistance = dist;
                    pt = i;
                }
            }

            minDistance = Math.Sqrt(minDistance);

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
            bool isSameWay = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[stripNum][pt].heading) - Math.PI) < 1.4;

            double RefDist = (distanceFromRefLine + (isSameWay ? mf.tool.toolOffset : -mf.tool.toolOffset)) / (mf.tool.toolWidth - mf.tool.toolOverlap);

            double howManyPathsAway;
            if (RefDist < 0) howManyPathsAway = (int)(RefDist - 0.5);
            else howManyPathsAway = (int)(RefDist + 0.5);

            if (howManyPathsAway == -1 || howManyPathsAway == 1)
            {
                //Is our angle of attack too high? Stops setting the wrong mapped path sometimes
                double refToPivotDelta = Math.PI - Math.Abs(Math.Abs(pivot.heading - stripList[stripNum][pt].heading) - Math.PI);
                if (refToPivotDelta > glm.PIBy2) refToPivotDelta = Math.Abs(refToPivotDelta - Math.PI);

                if (refToPivotDelta > 0.8)
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }

                ctList.Clear();

                //make the new guidance line list called guideList
                ptCount = stripList[stripNum].Count;
                int start, stop;

                start = pt - 35; if (start < 0) start = 0;
                stop = pt + 35; if (stop > ptCount) stop = ptCount;

                double distAway = (mf.tool.toolWidth - mf.tool.toolOverlap) * howManyPathsAway + (isSameWay ? -mf.tool.toolOffset : mf.tool.toolOffset);
                double distSqAway = (distAway * distAway) - 0.01;

                for (int i = start; i < stop; i++)
                {
                    vec3 point = new vec3(
                        stripList[stripNum][i].easting + (Math.Cos(stripList[stripNum][i].heading) * distAway),
                        stripList[stripNum][i].northing - (Math.Sin(stripList[stripNum][i].heading) * distAway),
                        stripList[stripNum][i].heading);

                    bool Add = true;
                    //make sure its not closer then 1 eq width
                    for (int j = start; j < stop; j++)
                    {
                        double check = glm.DistanceSquared(point.northing, point.easting, stripList[stripNum][j].northing, stripList[stripNum][j].easting);
                        if (check < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        if (false && ctList.Count > 0)
                        {
                            double dist = ((point.easting - ctList[ctList.Count - 1].easting) * (point.easting - ctList[ctList.Count - 1].easting))
                                + ((point.northing - ctList[ctList.Count - 1].northing) * (point.northing - ctList[ctList.Count - 1].northing));
                            if (dist > 0.3)
                                ctList.Add(point);
                        }
                        else ctList.Add(point);
                    }
                }

                int ctCount = ctList.Count;
                if (ctCount < 6)
                {
                    ctList.Clear();
                    isLocked = false;
                    return;
                }
            }
            else
            {
                ctList.Clear();
                isLocked = false;
                return;
            }
        }

        //start stop and add points to list
        public void StartContourLine(vec3 pivot)
        {
            isContourOn = true;
            if (ptList.Count == 1)
            {
                //reuse ptList
                ptList.Clear();
            }
            else
            {
                //make new ptList
                ptList = new List<vec3>();
                stripList.Add(ptList);
            }

            ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
        }

        //Add current position to stripList
        public void AddPoint(vec3 pivot)
        {
            ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));
        }

        //End the strip
        public void StopContourLine(vec3 pivot)
        {
            //make sure its long enough to bother
            if (ptList.Count > 10)
            {
                ptList.Add(new vec3(pivot.easting + Math.Cos(pivot.heading) * mf.tool.toolOffset, pivot.northing - Math.Sin(pivot.heading) * mf.tool.toolOffset, pivot.heading));

                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(ptList);
            }

            //delete ptList
            else
            {
                ptList.Clear();
                int ra = stripList.Count - 1;
                if (ra > 0) stripList.RemoveAt(ra);
            }

            //turn it off
            isContourOn = false;
        }

        //build contours for boundaries
        public void BuildBoundaryContours(int pass, int spacingInt)
        {
            if (mf.bnd.bndArr.Count == 0)
            {
                mf.TimedMessageBox(1500, "Boundary Contour Error", "No Boundaries Made");
                return;
            }

            //convert to meters
            double spacing = spacingInt;
            spacing *= 0.01;

            vec3 point = new vec3();
            double totalHeadWidth;
            int signPass;

            if (pass == 1)
            {
                signPass = -1;
                //determine how wide a headland space
                totalHeadWidth = ((mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5) - spacing;
            }

            else
            {
                signPass = 1;
                totalHeadWidth = ((mf.tool.toolWidth - mf.tool.toolOverlap) * pass) + spacing +
                    ((mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5);
            }


            //outside boundary

            //count the points from the boundary
            int ptCount = mf.bnd.bndArr[0].bndLine.Count;

            ptList = new List<vec3>();
            stripList.Add(ptList);

            for (int i = ptCount - 1; i >= 0; i--)
            {
                //calculate the point inside the boundary
                point.easting = mf.bnd.bndArr[0].bndLine[i].easting - (signPass * Math.Sin(glm.PIBy2 + mf.bnd.bndArr[0].bndLine[i].heading) * totalHeadWidth);
                point.northing = mf.bnd.bndArr[0].bndLine[i].northing - (signPass * Math.Cos(glm.PIBy2 + mf.bnd.bndArr[0].bndLine[i].heading) * totalHeadWidth);
                point.heading = mf.bnd.bndArr[0].bndLine[i].heading - Math.PI;
                if (point.heading < -glm.twoPI) point.heading += glm.twoPI;
                ptList.Add(point);
            }

            //totalHeadWidth = (mf.tool.toolWidth - mf.tool.toolOverlap) * 0.5 + 0.2 + (mf.tool.toolWidth - mf.tool.toolOverlap);

            for (int j = 1; j < mf.bnd.bndArr.Count; j++)
            {
                if (!mf.bnd.bndArr[j].isSet) continue;

                //count the points from the boundary
                ptCount = mf.bnd.bndArr[j].bndLine.Count;

                ptList = new List<vec3>();
                stripList.Add(ptList);

                for (int i = ptCount - 1; i >= 0; i--)
                {
                    //calculate the point inside the boundary
                    point.easting = mf.bnd.bndArr[j].bndLine[i].easting - (signPass * Math.Sin(glm.PIBy2 + mf.bnd.bndArr[j].bndLine[i].heading) * totalHeadWidth);
                    point.northing = mf.bnd.bndArr[j].bndLine[i].northing - (signPass * Math.Cos(glm.PIBy2 + mf.bnd.bndArr[j].bndLine[i].heading) * totalHeadWidth);
                    point.heading = mf.bnd.bndArr[j].bndLine[i].heading - Math.PI;
                    if (point.heading < -glm.twoPI) point.heading += glm.twoPI;

                    //only add if inside actual field boundary
                    ptList.Add(point);
                }

                //add the point list to the save list for appending to contour file
                //mf.contourSaveList.Add(ptList);
            }

            mf.TimedMessageBox(1500, "Boundary Contour", "Contour Path Created");
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

                    abHeading = Math.Atan2(dx, dy);
                    if (abHeading < 0) abHeading += glm.twoPI;
                    //if (abHeading > Math.PI) abHeading -= glm.twoPI;

                    //abHeading = ctList[A].heading;

                    //how far from current AB Line is fix
                    distanceFromCurrentLinePivot = ((dy * steer.easting) - (dx * steer.northing) + (ctList[B].easting
                                * ctList[A].northing) - (ctList[B].northing * ctList[A].easting))
                                    / Math.Sqrt((dy * dy) + (dx * dx));

                    //Subtract the two headings, if > 1.57 its going the opposite heading as refAB
                    abFixHeadingDelta = (Math.Abs(mf.fixHeading - abHeading));
                    if (abFixHeadingDelta >= Math.PI) abFixHeadingDelta = Math.Abs(abFixHeadingDelta - glm.twoPI);

                    isABSameAsVehicleHeading = abFixHeadingDelta < glm.PIBy2;

                    // calc point on ABLine closest to current position
                    double U = (((steer.easting - ctList[A].easting) * dx) + ((steer.northing - ctList[A].northing) * dy))
                                / ((dx * dx) + (dy * dy));

                    rEastCT = ctList[A].easting + (U * dx);
                    rNorthCT = ctList[A].northing + (U * dy);

                    ////find closest point to goal to get heading.
                    //minDistA = 99999;
                    //for (int t = 0; t < ptCount; t++)
                    //{
                    //    double dist = ((rEastCT - ctList[t].easting) * (rEastCT - ctList[t].easting))
                    //                    + ((rNorthCT - ctList[t].northing) * (rNorthCT - ctList[t].northing));
                    //    if (dist < minDistA)
                    //    {
                    //        A = t;
                    //    }
                    //}

                    //abHeading = ctList[A].heading;

                    //distance is negative if on left, positive if on right
                    if (isABSameAsVehicleHeading)
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

                    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    steerAngleCT = Math.Atan((distanceFromCurrentLinePivot * mf.vehicle.stanleyDistanceErrorGain) 
                        / ((Math.Abs(mf.pn.speed) * 0.277777) + 1));

                    if (steerAngleCT > 0.74) steerAngleCT = 0.74;
                    if (steerAngleCT < -0.74) steerAngleCT = -0.74;

                    if (mf.pn.speed > -0.1)
                        steerAngleCT = glm.toDegrees((steerAngleCT + abFixHeadingDelta) * -1.0);
                    else
                        steerAngleCT = glm.toDegrees((steerAngleCT - abFixHeadingDelta) * -1.0);


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

                    //get the distance from currently active AB line
                    //x2-x1
                    double dx = ctList[B].easting - ctList[A].easting;
                    //z2-z1
                    double dy = ctList[B].northing - ctList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon) return;

                    //abHeading = Math.Atan2(dz, dx);
                    abHeading = ctList[A].heading;

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

                        if (mf.isAutoSteerBtnOn
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

                    // ** Pure pursuit ** - calc point on ABLine closest to current position
                    double U = (((pivot.easting - ctList[A].easting) * dx) + ((pivot.northing - ctList[A].northing) * dy))
                                / ((dx * dx) + (dy * dy));

                    rEastCT = ctList[A].easting + (U * dx);
                    rNorthCT = ctList[A].northing + (U * dy);

                    //Subtract the two headings, if > 1.57 its going the opposite heading as refAB
                    abFixHeadingDelta = (Math.Abs(mf.fixHeading - abHeading));
                    if (abFixHeadingDelta >= Math.PI) abFixHeadingDelta = Math.Abs(abFixHeadingDelta - glm.twoPI);

                    //used for accumulating distance to find goal point
                    double distSoFar;

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

                    // used for calculating the length squared of next segment.
                    double tempDist = 0.0;

                    if (abFixHeadingDelta >= glm.PIBy2)
                    {
                        //counting down
                        isABSameAsVehicleHeading = false;
                        distSoFar = glm.Distance(ctList[A], rEastCT, rNorthCT);
                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            goalPointCT.easting = rEastCT - (Math.Sin(ctList[A].heading) * goalPointDistance);
                            goalPointCT.northing = rNorthCT - (Math.Cos(ctList[A].heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if start and break if so.
                            while (A > 0)
                            {
                                B--; A--;
                                tempDist = glm.Distance(ctList[B], ctList[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance)
                                {
                                    //A++; B++;
                                    break; //tempDist contains the full length of next segment
                                }
                                else
                                {
                                    distSoFar += tempDist;
                                }
                            }

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            goalPointCT.easting = (((1 - t) * ctList[B].easting) + (t * ctList[A].easting));
                            goalPointCT.northing = (((1 - t) * ctList[B].northing) + (t * ctList[A].northing));
                        }
                    }
                    else
                    {
                        //counting up
                        isABSameAsVehicleHeading = true;
                        distSoFar = glm.Distance(ctList[B], rEastCT, rNorthCT);

                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            goalPointCT.easting = rEastCT + (Math.Sin(ctList[A].heading) * goalPointDistance);
                            goalPointCT.northing = rNorthCT + (Math.Cos(ctList[A].heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if end and break if so.
                            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                            while (B < ptCount - 1)
                            {
                                B++; A++;
                                tempDist = glm.Distance(ctList[B], ctList[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance)
                                {
                                    //A--; B--;
                                    break; //tempDist contains the full length of next segment
                                }

                                distSoFar += tempDist;
                            }

                            //xt = (((1 - t) * x0 + t * x1)
                            //yt = ((1 - t) * y0 + t * y1))

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            goalPointCT.easting = (((1 - t) * ctList[A].easting) + (t * ctList[B].easting));
                            goalPointCT.northing = (((1 - t) * ctList[A].northing) + (t * ctList[B].northing));
                        }
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = glm.DistanceSquared(goalPointCT.northing, goalPointCT.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading;// = glm.twoPI - mf.fixHeading;

                    if (isABSameAsVehicleHeading) localHeading = glm.twoPI - mf.fixHeading + inty;
                    else localHeading = glm.twoPI - mf.fixHeading - inty;

                    steerAngleCT = glm.toDegrees(Math.Atan(2 * (((goalPointCT.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointCT.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (steerAngleCT < -mf.vehicle.maxSteerAngle) steerAngleCT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCT > mf.vehicle.maxSteerAngle) steerAngleCT = mf.vehicle.maxSteerAngle;

                    //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                    double angVel = glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(glm.toRadians(steerAngleCT))) / mf.vehicle.wheelbase;

                    //clamp the steering angle to not exceed safe angular velocity
                    if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
                    {
                        steerAngleCT = glm.toDegrees(steerAngleCT > 0 ?
                                (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.pn.speed * 0.277777)))
                            : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (glm.twoPI * mf.pn.speed * 0.277777))));
                    }
                }

                //fill in the autosteer variables
                mf.guidanceLineDistanceOff = mf.distanceDisplayPivot = (Int16)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
                mf.guidanceLineSteerAngle = (Int16)(steerAngleCT * 100);
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLinePivot = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
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

            //GL.PointSize(6.0f);
            //GL.Begin(PrimitiveType.Points);
            //GL.Color3(1.0f, 0.95f, 0.095f);
            //GL.Vertex3(rEastCT, rNorthCT, 0.0);
            //GL.End();
            //GL.PointSize(1.0f);

            //GL.Color3(0.98f, 0.98f, 0.50f);
            //GL.Begin(PrimitiveType.LineStrip);
            //GL.Vertex3(boxE.easting, boxE.northing, 0);
            //GL.Vertex3(boxA.easting, boxA.northing, 0);
            //GL.Vertex3(boxD.easting, boxD.northing, 0);
            //GL.Vertex3(boxG.easting, boxG.northing, 0);
            //GL.Vertex3(boxE.easting, boxE.northing, 0);
            //GL.End();

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

            //    GL.Color3(0.35f, 0.30f, 0.90f);
            //    GL.PointSize(6.0f);
            //    GL.Begin(PrimitiveType.Points);
            //    GL.Vertex3(conList[closestRefPoint].x, conList[closestRefPoint].z, 0);
            //    GL.End();
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
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

        public double pivotDistanceError, pivotDistanceErrorLast, pivotDerivative;

        //derivative counters
        private int counter2;

        public double inty;
        public double pivotErrorTotal;

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
                        if ((((boxA.easting - boxB.easting) * (stripList[s][p].northing - boxB.northing))
                                - ((boxA.northing - boxB.northing) * (stripList[s][p].easting - boxB.easting))) > 0)
                        {
                            continue;
                        }

                        dist = ((pivot.easting - stripList[s][p].easting) * (pivot.easting - stripList[s][p].easting))
                            + ((pivot.northing - stripList[s][p].northing) * (pivot.northing - stripList[s][p].northing));
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            stripNum = s;
                            pt = lastLockPt = p;
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
                        }

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
                    double localHeading;

                    if (isHeadingSameWay) localHeading = glm.twoPI - mf.fixHeading + inty;
                    else localHeading = glm.twoPI - mf.fixHeading - inty;

                    steerAngleCT = glm.toDegrees(Math.Atan(2 * (((goalPointCT.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointCT.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.VehicleConfig.Wheelbase / goalPointDistanceSquared));

                    if (mf.ahrs.imuRoll != 88888)
                        steerAngleCT += mf.ahrs.imuRoll * -mf.gyd.sideHillCompFactor;

                    if (steerAngleCT < -mf.vehicle.maxSteerAngle) steerAngleCT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCT > mf.vehicle.maxSteerAngle) steerAngleCT = mf.vehicle.maxSteerAngle;
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
            //make new ptList
            ptList = new List<vec3>(16);
            stripList.Add(ptList);
            isContourOn = true;
            return;
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
                //add the point list to the save list for appending to contour file
                mf.contourSaveList.Add(ptList);
            }
            //delete ptList
            else
            {
                ptList.Clear();
            }

            //turn it off
            isContourOn = false;
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
            int ptCount = ctList.Count;
            if (ptCount < 2) return;
            GL.LineWidth(mf.ABLine.lineWidth);
            GL.Color3(0.98f, 0.2f, 0.980f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++)
            {
                GL.Vertex2(ctList[h].easting, ctList[h].northing);
            }
            GL.End();

            GL.PointSize(mf.ABLine.lineWidth);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0.87f, 08.7f, 0.25f);
            for (int h = 0; h < ptCount; h++)
            {
                GL.Vertex2(ctList[h].easting, ctList[h].northing);
            }

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

            if (stripNum > -1)
            {
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < stripList[stripNum].Count; h++)
                {
                    GL.Vertex2(stripList[stripNum][h].easting, stripList[stripNum][h].northing);
                }
                GL.End();
            }

            GL.Color3(0.35f, 0.30f, 0.90f);
            GL.PointSize(6.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(stripList[stripNum][pt].easting, stripList[stripNum][pt].northing);
            GL.End();

            if (mf.isPureDisplayOn && distanceFromCurrentLinePivot != 32000 && !mf.isStanleyUsed)
            {
                //Draw lookahead Point
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(1.0f, 0.95f, 0.095f);
                GL.Vertex2(goalPointCT.easting, goalPointCT.northing);
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
    }
}
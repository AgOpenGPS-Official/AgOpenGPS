using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public enum SkipMode { Normal, Alternative, IgnoreWorkedTracks, Outward, Inward };

    public class CYouTurn
    {
        #region Fields
        //copy of the mainform address
        private readonly FormGPS mf;

        /// <summary>/// triggered right after youTurnTriggerPoint is set /// </summary>
        public bool isYouTurnTriggered, isGoingStraightThrough = false;

        /// <summary>  /// turning right or left?/// </summary>
        public bool isTurnLeft;
        private int semiCircleIndex = -1;

        /// <summary> /// Is the youturn button enabled? /// </summary>
        public bool isYouTurnBtnOn;

        public double youTurnRadius;

        public int rowSkipsWidth = 1, uTurnSmoothing;

        public bool previousBigSkip = true;
        public SkipMode skipMode = SkipMode.Normal;
        public int rowSkipsWidth2 = 3, turnSkips = 2;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double distanceFromCurrentLine, uturnDistanceFromBoundary;

        private int A, B;
        private bool isHeadingSameWay = true;
        public bool isTurnCreationTooClose = false, isTurnCreationNotCrossingError = false, turnTooCloseTrigger = false;

        public vec2 goalPointYT = new vec2(0, 0);
        public vec2 radiusPointYT = new vec2(0, 0);
        public double steerAngleYT, rEastYT, rNorthYT, ppRadiusYT;

        //list of points for scaled and rotated YouTurn line, used for pattern, dubins, abcurve, abline
        public List<vec3> ytList = new List<vec3>();
        private List<vec3> ytList2 = new List<vec3>();

        //next curve or line to build out turn and point over
        public List<vec3> nextCurve = new List<vec3>();

        //if we continue on the same line or change to the next one after the uTurn
        public bool isOutSameCurve;

        public int uTurnStyle = 0;

        //is UTurn pattern in or out of bounds
        public bool isOutOfBounds = false;

        //sequence of operations of finding the next turn 0 to 3
        public int youTurnPhase;

        // Returns 1 if the lines intersect, otherwis
        public double iE = 0, iN = 0;

        // the list of possible bounds points
        public List<CClose> turnClosestList = new List<CClose>();

        //point at the farthest turn segment from pivotAxle
        public CClose closestTurnPt = new CClose();

        //where the in and out tangents cross for Albin curve
        public CClose inClosestTurnPt = new CClose();
        public CClose outClosestTurnPt = new CClose();
        public CClose startOfTurnPt = new CClose();

        //how far should the distance between points on the uTurn be
        private double pointSpacing;
        #endregion

        //constructor
        public CYouTurn(FormGPS _f)
        {
            mf = _f;

            uturnDistanceFromBoundary = Properties.Settings.Default.set_youTurnDistanceFromBoundary;

            //how far before or after boundary line should turn happen
            youTurnStartOffset = Properties.Settings.Default.set_youTurnExtensionLength;

            rowSkipsWidth = Properties.Settings.Default.set_youSkipWidth;
            Set_Alternate_skips();

            ytList.Capacity = 128;

            youTurnRadius = Properties.Settings.Default.set_youTurnRadius;

            uTurnStyle = Properties.Settings.Default.set_uTurnStyle;

            uTurnSmoothing = Properties.Settings.Default.setAS_uTurnSmoothing;
        }

        //find next not worked lane after the defined lanes to skip
        private int GetNextNotWorkedTrack(bool isTurnLeft, int rowSkipsWidth, bool isAB)
        {
            int goalLane;

            if (isAB)
            {

                if ((isTurnLeft && !mf.ABLine.isHeadingSameWay) || (!isTurnLeft && mf.ABLine.isHeadingSameWay))
                    goalLane = mf.ABLine.howManyPathsAway + rowSkipsWidth;
                else
                    goalLane = mf.ABLine.howManyPathsAway - rowSkipsWidth;

                while (mf.trk.gArr[mf.trk.idx].workedTracks.Contains(goalLane))
                {
                    rowSkipsWidth++;
                    if ((isTurnLeft && !mf.ABLine.isHeadingSameWay) || (!isTurnLeft && mf.ABLine.isHeadingSameWay))
                        goalLane++;
                    else
                        goalLane--;
                }
            }
            else
            {
                if ((isTurnLeft && !mf.curve.isHeadingSameWay) || (!isTurnLeft && mf.curve.isHeadingSameWay))
                    goalLane = mf.curve.howManyPathsAway + rowSkipsWidth;
                else
                    goalLane = mf.curve.howManyPathsAway - rowSkipsWidth;

                while (mf.trk.gArr[mf.trk.idx].workedTracks.Contains(goalLane))
                {
                    rowSkipsWidth++;
                    if ((isTurnLeft && !mf.curve.isHeadingSameWay) || (!isTurnLeft && mf.curve.isHeadingSameWay))
                        goalLane++;
                    else
                        goalLane--;
                }
            }


            return rowSkipsWidth;
        }

        //Finds the point where an AB Curve crosses the turn line
        public bool BuildCurveDubinsYouTurn()
        {
            //if mode is skip workedTracks -> mark current track as worked if sections were on, then find next
            if (skipMode == SkipMode.IgnoreWorkedTracks)
            {
                // Mark the current track as worked only if sections were actually on
                if (mf.trk.idx >= 0 && mf.trk.idx < mf.trk.gArr.Count)
                {
                    if (mf.autoBtnState == btnStates.Auto || mf.manualBtnState == btnStates.On)
                    {
                        mf.trk.gArr[mf.trk.idx].workedTracks.Add(mf.curve.howManyPathsAway);
                    }
                }

                rowSkipsWidth = GetNextNotWorkedTrack(isTurnLeft, Properties.Settings.Default.set_youSkipWidth, false);
            }

            //TODO: is calculated many taimes after the priveous turn is complete
            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);
            pointSpacing = youTurnRadius * 0.1;

            if (uTurnStyle == 0)
            {
                //Albin turn
                if (turnOffset > (youTurnRadius * 2.0))
                {
                    return CreateCurveWideTurn();
                }

                //Ohmega turn
                else
                {
                    return CreateCurveOmegaTurn();
                }
            }
            else if (uTurnStyle == 1)
            {
                return (KStyleTurnCurve());
            }

            //prgramming error if you got here
            return false;
        }

        public bool BuildABLineDubinsYouTurn()
        {
            //if mode is skip workedTracks -> mark current track as worked if sections were on, then find next
            if (skipMode == SkipMode.IgnoreWorkedTracks)
            {
                // Mark the current track as worked only if sections were actually on
                if (mf.trk.idx >= 0 && mf.trk.idx < mf.trk.gArr.Count)
                {
                    if (mf.autoBtnState == btnStates.Auto || mf.manualBtnState == btnStates.On)
                    {
                        mf.trk.gArr[mf.trk.idx].workedTracks.Add(mf.ABLine.howManyPathsAway);
                    }
                }

                rowSkipsWidth = GetNextNotWorkedTrack(isTurnLeft, Properties.Settings.Default.set_youSkipWidth, true);
            }

            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth
                + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);

            pointSpacing = youTurnRadius * 0.1;

            if (uTurnStyle == 0)
            {

                //Wide turn
                if (turnOffset > (youTurnRadius * 2.0))
                {
                    return CreateABWideTurn();
                }
                //Small turn
                else
                {
                    return CreateABOmegaTurn();
                }
            }
            else if (uTurnStyle == 1)
            {
                return KStyleTurnAB();
            }

            //prgramming error if you got here
            return false;
        }

        #region CreateTurn

        private bool CreateCurveOmegaTurn()
        {
            //keep from making turns constantly - wait 1.5 seconds
            if (mf.makeUTurnCounter < 4)
            {
                youTurnPhase = 0;
                return true;
            }

            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return true;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);

            switch (youTurnPhase)
            {
                case 0: //find the crossing points
                    if (!FindCurveTurnPoint(mf.curve, false))
                    {
                        if (track.mode == TrackMode.waterPivot || track.mode == TrackMode.bndCurve)
                        {
                            youTurnPhase = 11;//ignore
                        }
                        else
                            FailCreate();
                        return false;
                    }

                    //save a copy 
                    inClosestTurnPt = new CClose(closestTurnPt);

                    ytList?.Clear();

                    int count = mf.curve.isHeadingSameWay ? -1 : 1;
                    int curveIndex = inClosestTurnPt.curveIndex;

                    isOutOfBounds = true;
                    int stopIfWayOut = 0;

                    double head = 0; //gives error since isnt updated

                    while (isOutOfBounds)
                    {
                        stopIfWayOut++;
                        isOutOfBounds = false;

                        //creates half a circle starting at the crossing point
                        ytList.Clear();

                        curveIndex += count;

                        vec3 currentPos = new vec3(mf.curve.curList[curveIndex]);

                        if (!mf.curve.isHeadingSameWay) currentPos.heading += Math.PI;
                        if (currentPos.heading >= glm.twoPI) currentPos.heading -= glm.twoPI;
                        head = currentPos.heading;

                        CDubins dubYouTurnPath = new CDubins();
                        CDubins.turningRadius = youTurnRadius;

                        //now we go the other way to turn round
                        double invertHead = currentPos.heading - Math.PI;
                        if (invertHead <= -Math.PI) invertHead += glm.twoPI;
                        if (invertHead >= Math.PI) invertHead -= glm.twoPI;

                        vec3 goal = new vec3();

                        //neat trick to not have to add pi/2
                        if (isTurnLeft)
                        {
                            goal.easting = mf.curve.curList[curveIndex - count].easting + (Math.Cos(-invertHead) * turnOffset);
                            goal.northing = mf.curve.curList[curveIndex - count].northing + (Math.Sin(-invertHead) * turnOffset);
                        }
                        else
                        {
                            goal.easting = mf.curve.curList[curveIndex - count].easting - (Math.Cos(-invertHead) * turnOffset);
                            goal.northing = mf.curve.curList[curveIndex - count].northing - (Math.Sin(-invertHead) * turnOffset);
                        }

                        goal.heading = invertHead;

                        //generate the turn points
                        ytList = dubYouTurnPath.GenerateDubins(currentPos, goal);
                        if (ytList.Count == 0)
                        {
                            FailCreate();
                            return false;
                        }

                        if (stopIfWayOut == 300 || curveIndex < 1 || curveIndex > (mf.curve.curList.Count - 2))
                        {
                            //for some reason it doesn't go inside boundary
                            FailCreate();
                            return false;
                        }

                        for (int i = 0; i < ytList.Count; i++)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList[i]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                    }
                    inClosestTurnPt.curveIndex = curveIndex;

                    //too many points from Dubins - so cut
                    double distance;
                    int cnt = ytList.Count;
                    for (int i = 1; i < cnt - 2; i++)
                    {
                        distance = glm.DistanceSquared(ytList[i], ytList[i + 1]);
                        if (distance < pointSpacing)
                        {
                            ytList.RemoveAt(i + 1);
                            i--;
                            cnt = ytList.Count;
                        }
                    }

                    //move the turn to exact at the turnline
                    ytList = MoveTurnInsideTurnLine(ytList, head, false, false);
                    if (ytList.Count == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    youTurnPhase = 1;
                    break;

                case 1:
                    //build the next line to add sequencelines
                    double widthMinusOverlap = mf.tool.width - mf.tool.overlap;

                    double distAway = widthMinusOverlap * (mf.curve.howManyPathsAway + ((isTurnLeft ^ mf.curve.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth)) + (mf.curve.isHeadingSameWay ? mf.tool.offset : -mf.tool.offset) + track.nudgeDistance;

                    distAway += (0.5 * widthMinusOverlap);

                    //create the next line
                    nextCurve = mf.curve.BuildNewOffsetList(distAway, track);

                    //get the index of the last yt point
                    double dis = double.MaxValue;
                    if (nextCurve.Count > 1)
                    {
                        for (int i = 1; i < nextCurve.Count; i++)
                        {
                            double newdis = glm.Distance(nextCurve[i], ytList[ytList.Count - 1]);
                            if (newdis < dis)
                            {
                                dis = newdis;
                                if (mf.curve.isHeadingSameWay) outClosestTurnPt.curveIndex = i - 1;
                                else outClosestTurnPt.curveIndex = i;
                            }
                        }

                        if (outClosestTurnPt.curveIndex >= 0)
                        {
                            outClosestTurnPt.closePt = new vec3(nextCurve[outClosestTurnPt.curveIndex]);
                            inClosestTurnPt.closePt = new vec3(mf.curve.curList[inClosestTurnPt.curveIndex]);

                            if (!AddCurveSequenceLines()) return false;
                        }
                    }

                    //fill in the gaps
                    double distanc;

                    int cnt4 = ytList.Count;
                    for (int i = 1; i < cnt4 - 2; i++)
                    {
                        int j = i + 1;
                        if (j == cnt4 - 1) continue;
                        distanc = glm.DistanceSquared(ytList[i], ytList[j]);
                        if (distanc > 1)
                        {
                            vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                                (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                            ytList.Insert(j, pointB);
                            cnt4 = ytList.Count;
                            i--;
                        }
                    }

                    //calculate the new points headings based on fore and aft of point - smoother turns
                    cnt4 = ytList.Count;
                    vec3[] arr = new vec3[cnt4];
                    cnt4 -= 2;
                    ytList.CopyTo(arr);
                    ytList.Clear();

                    for (int i = 2; i < cnt4; i++)
                    {
                        vec3 pt3 = new vec3(arr[i]);
                        pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting,
                            arr[i + 1].northing - arr[i - 1].northing);
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        ytList.Add(pt3);
                    }

                    //check to close
                    if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                    {
                        FailCreate();
                        return false;
                    }

                    isOutOfBounds = false;
                    youTurnPhase = 10;
                    turnTooCloseTrigger = false;
                    isTurnCreationTooClose = false;
                    return true;
            }
            return true;
        }

        private bool CreateCurveWideTurn()
        {
            //keep from making turns constantly - wait 1.5 seconds
            if (mf.makeUTurnCounter < 4)
            {
                youTurnPhase = 0;
                return true;
            }

            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return true;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            //we are doing a wide turn
            double head = 0;
            int count = mf.curve.isHeadingSameWay ? -1 : 1;
            switch (youTurnPhase)
            {
                case 0:
                    //Create first semicircle
                    if (!FindCurveTurnPoint(mf.curve, false))
                    {
                        if (track.mode == TrackMode.waterPivot || track.mode == TrackMode.bndCurve)
                        {
                            youTurnPhase = 11;//ignore
                        }
                        else
                            FailCreate();
                        return false;
                    }
                    inClosestTurnPt = new CClose(closestTurnPt);
                    startOfTurnPt = new CClose(inClosestTurnPt);

                    int stopIfWayOut = 0;
                    isOutOfBounds = true;

                    while (isOutOfBounds)
                    {
                        isOutOfBounds = false;
                        stopIfWayOut++;

                        vec3 currentPos = new vec3(mf.curve.curList[inClosestTurnPt.curveIndex]);

                        head = currentPos.heading;
                        if (!mf.curve.isHeadingSameWay) head += Math.PI;
                        if (head > glm.twoPI) head -= glm.twoPI;
                        currentPos.heading = head;

                        // creates half a circle starting at the crossing point
                        ytList.Clear();
                        ytList.Add(currentPos);

                        //Taken from Dubbins
                        while (Math.Abs(head - currentPos.heading) < Math.PI)
                        {
                            //Update the position of the car
                            currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                            currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                            //Which way are we turning?
                            double turnParameter = isTurnLeft ? -1.0 : 1.0;

                            //Update the heading
                            currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                            //Add the new coordinate to the path
                            ytList.Add(currentPos);
                        }

                        int cnt4 = ytList.Count;
                        if (cnt4 == 0)
                        {
                            FailCreate();
                            return false;
                        }

                        //Are we out of bounds?
                        for (int j = 0; j < cnt4; j += 2)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList[j]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }

                        //first check if not out of bounds, add a bit more to clear turn line, set to phase 2
                        if (!isOutOfBounds)
                        {
                            ytList = MoveTurnInsideTurnLine(ytList, head, true, false);
                            if (ytList.Count == 0)
                            {
                                FailCreate();
                                return false;
                            }
                            youTurnPhase = 1;
                            return true;
                        }

                        if (stopIfWayOut == 300 || inClosestTurnPt.curveIndex < 1 || inClosestTurnPt.curveIndex > (mf.curve.curList.Count - 2))
                        {
                            //for some reason it doesn't go inside boundary
                            FailCreate();
                            return false;
                        }

                        //keep moving infield till pattern is all inside
                        inClosestTurnPt.curveIndex = inClosestTurnPt.curveIndex + count;
                        inClosestTurnPt.closePt = new vec3(mf.curve.curList[inClosestTurnPt.curveIndex]);


                        //set the flag to Critical stop machine
                        if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                        {
                            FailCreate();
                            return false;
                        }
                    }

                    return false;

                case 1:
                    //build the next line to add sequencelines
                    double widthMinusOverlap = mf.tool.width - mf.tool.overlap;

                    double distAway = widthMinusOverlap * (mf.curve.howManyPathsAway + ((isTurnLeft ^ mf.curve.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth)) + (mf.curve.isHeadingSameWay ? mf.tool.offset : -mf.tool.offset) + track.nudgeDistance;

                    distAway += (0.5 * widthMinusOverlap);

                    //create the next line
                    nextCurve = mf.curve.BuildNewOffsetList(distAway, track);

                    //going with or against boundary?
                    bool isTurnLineSameWay = true;
                    double headingDifference = Math.Abs(inClosestTurnPt.turnLineHeading - ytList[ytList.Count - 1].heading);
                    if (headingDifference > glm.PIBy2 && headingDifference < 3 * glm.PIBy2) isTurnLineSameWay = false;

                    if (!FindCurveOutTurnPoint(mf.curve, ref nextCurve, startOfTurnPt, isTurnLineSameWay))
                    {
                        //error
                        FailCreate();
                        return false;
                    }
                    outClosestTurnPt = new CClose(closestTurnPt);

                    //move the turn inside of turnline with help from the crossingCurvePoints
                    isOutOfBounds = true;
                    while (isOutOfBounds)
                    {
                        isOutOfBounds = false;
                        vec3 currentPos = new vec3(nextCurve[outClosestTurnPt.curveIndex]);

                        head = currentPos.heading;
                        if ((!mf.curve.isHeadingSameWay && !isOutSameCurve) || (mf.curve.isHeadingSameWay && isOutSameCurve)) head += Math.PI;
                        if (head > glm.twoPI) head -= glm.twoPI;
                        currentPos.heading = head;

                        // creates half a circle starting at the crossing point
                        ytList2?.Clear();
                        ytList2.Add(currentPos);

                        //Taken from Dubbins
                        while (Math.Abs(head - currentPos.heading) < Math.PI)
                        {
                            //Update the position of the car
                            currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                            currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                            //Which way are we turning?
                            double turnParameter = isTurnLeft ? 1.0 : -1.0;

                            //Update the heading
                            currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                            //Add the new coordinate to the path
                            ytList2.Add(currentPos);
                        }

                        int cnt3 = ytList2.Count;
                        if (cnt3 == 0)
                        {
                            FailCreate();
                            return false;
                        }

                        //Are we out of bounds?
                        for (int j = 0; j < cnt3; j += 2)
                        {
                            if (mf.bnd.IsPointInsideTurnArea(ytList2[j]) != 0)
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }

                        //first check if not out of bounds, add a bit more to clear turn line, set to phase 2
                        if (!isOutOfBounds)
                        {
                            ytList2 = MoveTurnInsideTurnLine(ytList2, head, true, true);
                            if (ytList2.Count == 0)
                            {
                                FailCreate();
                                return false;
                            }
                            youTurnPhase = 2;
                            return true;
                        }

                        if (outClosestTurnPt.curveIndex < 1 || outClosestTurnPt.curveIndex > (nextCurve.Count - 2))
                        {
                            //for some reason it doesn't go inside boundary
                            FailCreate();
                            return false;
                        }

                        //keep moving infield till pattern is all inside
                        if (!isOutSameCurve) outClosestTurnPt.curveIndex = outClosestTurnPt.curveIndex + count;
                        else outClosestTurnPt.curveIndex = outClosestTurnPt.curveIndex - count;
                        outClosestTurnPt.closePt = new vec3(nextCurve[outClosestTurnPt.curveIndex]);

                    }
                    return false;

                case 2:
                    //Bind the two turns together
                    int cnt1 = ytList.Count;
                    int cnt2 = ytList2.Count;

                    //Validate that both lists have elements to prevent crashes
                    if (cnt1 == 0 || cnt2 == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    //Find if the turn goes same way as turnline heading
                    bool isFirstTurnLineSameWay = true;
                    double firstHeadingDifference = Math.Abs(inClosestTurnPt.turnLineHeading - ytList[ytList.Count - 1].heading);
                    if (firstHeadingDifference > glm.PIBy2 && firstHeadingDifference < 3 * glm.PIBy2) isFirstTurnLineSameWay = false;

                    //finds out start and goal point along the tunline
                    FindInnerTurnPoints(ytList[cnt1 - 1], ytList[0].heading, inClosestTurnPt, isFirstTurnLineSameWay);
                    CClose startClosestTurnPt = new CClose(closestTurnPt);

                    FindInnerTurnPoints(ytList2[cnt2 - 1], ytList2[0].heading + Math.PI, outClosestTurnPt, !isFirstTurnLineSameWay);
                    CClose goalClosestTurnPt = new CClose(closestTurnPt);

                    //we have 2 different turnLine crossings
                    if (startClosestTurnPt.turnLineNum != goalClosestTurnPt.turnLineNum)
                    {
                        FailCreate();
                        return false;
                    }

                    //segment index is the "A" of the segment. segmentIndex+1 would be the "B"
                    //is in and out on same segment? so only 1 segment
                    if (startClosestTurnPt.turnLineIndex == goalClosestTurnPt.turnLineIndex)
                    {
                        for (int a = 0; a < cnt2; a++)
                        {
                            ytList.Add(new vec3(ytList2[cnt2 - 1 - a]));
                        }
                    }
                    else
                    {
                        //mulitple segments
                        vec3 tPoint = new vec3();
                        int turnCount = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine.Count;

                        //how many points from turnline do we add
                        int loops = Math.Abs(startClosestTurnPt.turnLineIndex - goalClosestTurnPt.turnLineIndex);

                        //are we crossing a border?
                        if (loops > (mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine.Count / 2))
                        {
                            if (startClosestTurnPt.turnLineIndex < goalClosestTurnPt.turnLineIndex)
                            {
                                loops = (turnCount - goalClosestTurnPt.turnLineIndex) + startClosestTurnPt.turnLineIndex;
                            }
                            else
                            {
                                loops = (turnCount - startClosestTurnPt.turnLineIndex) + goalClosestTurnPt.turnLineIndex;
                            }
                        }

                        //countExit up - start with B which is next A
                        if (isFirstTurnLineSameWay)
                        {
                            for (int i = 0; i < loops; i++)
                            {
                                if ((startClosestTurnPt.turnLineIndex + 1) >= turnCount) startClosestTurnPt.turnLineIndex = -1;

                                tPoint = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine[startClosestTurnPt.turnLineIndex + 1];
                                startClosestTurnPt.turnLineIndex++;
                                if (startClosestTurnPt.turnLineIndex >= turnCount)
                                    startClosestTurnPt.turnLineIndex = 0;
                                ytList.Add(tPoint);
                            }
                        }
                        else //countExit down = start with A
                        {
                            for (int i = 0; i < loops; i++)
                            {
                                tPoint = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine[startClosestTurnPt.turnLineIndex];
                                startClosestTurnPt.turnLineIndex--;
                                if (startClosestTurnPt.turnLineIndex == -1)
                                    startClosestTurnPt.turnLineIndex = turnCount - 1;
                                ytList.Add(tPoint);
                            }
                        }

                        //add the out from ytList2
                        for (int a = 0; a < cnt2; a++)
                        {
                            ytList.Add(new vec3(ytList2[cnt2 - 1 - a]));
                        }
                    }

                    if (!AddCurveSequenceLines()) return false;

                    //fill in the gaps
                    double distance;

                    int cnt = ytList.Count;
                    for (int i = 1; i < cnt - 2; i++)
                    {
                        int j = i + 1;
                        if (j == cnt - 1) continue;
                        distance = glm.DistanceSquared(ytList[i], ytList[j]);
                        if (distance > 1)
                        {
                            vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                                (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                            ytList.Insert(j, pointB);
                            cnt = ytList.Count;
                            i--;
                        }
                    }

                    //calculate the new points headings based on fore and aft of point - smoother turns
                    cnt = ytList.Count;
                    vec3[] arr = new vec3[cnt];
                    cnt -= 2;
                    ytList.CopyTo(arr);
                    ytList.Clear();

                    for (int i = 2; i < cnt; i++)
                    {
                        vec3 pt3 = new vec3(arr[i]);
                        pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting,
                            arr[i + 1].northing - arr[i - 1].northing);
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        ytList.Add(pt3);
                    }

                    //check to close
                    if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                    {
                        FailCreate();
                        return false;
                    }

                    //are we continuing the same way?
                    isGoingStraightThrough = Math.PI - Math.Abs(Math.Abs(ytList[ytList.Count - 2].heading - ytList[1].heading) - Math.PI) < glm.PIBy2;
                    ytList2?.Clear();
                    isOutOfBounds = false;
                    youTurnPhase = 10;
                    turnTooCloseTrigger = false;
                    isTurnCreationTooClose = false;
                    return true;
            }

            // just in case
            return false;
        }

        private bool CreateABOmegaTurn()
        {
            //keep from making turns constantly - wait 1.5 seconds
            if (mf.makeUTurnCounter < 4)
            {
                youTurnPhase = 0;
                return true;
            }

            double head = mf.ABLine.abHeading;
            if (!mf.ABLine.isHeadingSameWay) head += Math.PI;
            if (head >= glm.twoPI) head -= glm.twoPI;

            //we are doing an omega turn
            switch (youTurnPhase)
            {
                case 0:
                    //how far are we from any turn boundary
                    FindABTurnPoint(new vec3(mf.ABLine.rEastAB, mf.ABLine.rNorthAB, head));

                    //or did we lose the turnLine - we are on the highway cuz we left the outer/inner turn boundary
                    if (closestTurnPt.turnLineIndex != -1)
                    {
                        //calculate the distance to the turnline
                        mf.distancePivotToTurnLine = glm.Distance(mf.pivotAxlePos, closestTurnPt.closePt);
                    }
                    else
                    {
                        //Full emergency stop code goes here, it thinks its auto turn, but its not!
                        FailCreate();
                        return false;
                    }
                    inClosestTurnPt = new CClose(closestTurnPt);

                    CDubins dubYouTurnPath = new CDubins();
                    CDubins.turningRadius = youTurnRadius;

                    //grab the vehicle widths and offsets
                    double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);

                    vec3 start = new vec3(inClosestTurnPt.closePt);
                    start.heading = head;

                    vec3 goal = new vec3(start);

                    //now we go the other way to turn round
                    double invertedHead = head - Math.PI;
                    if (invertedHead < 0) invertedHead += glm.twoPI;
                    if (invertedHead > glm.twoPI) invertedHead -= glm.twoPI;

                    if (isTurnLeft)
                    {
                        goal.easting = goal.easting + (Math.Cos(-invertedHead) * turnOffset);
                        goal.northing = goal.northing + (Math.Sin(-invertedHead) * turnOffset);
                    }
                    else
                    {
                        goal.easting = goal.easting - (Math.Cos(-invertedHead) * turnOffset);
                        goal.northing = goal.northing - (Math.Sin(-invertedHead) * turnOffset);
                    }

                    goal.heading = invertedHead;

                    //generate the turn points
                    ytList = dubYouTurnPath.GenerateDubins(start, goal);
                    isOutOfBounds = true;

                    if (ytList.Count == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    youTurnPhase = 1;

                    double distance;

                    int cnt = ytList.Count;
                    for (int i = 1; i < cnt - 2; i++)
                    {
                        distance = glm.DistanceSquared(ytList[i], ytList[i + 1]);
                        if (distance < pointSpacing)
                        {
                            ytList.RemoveAt(i + 1);
                            i--;
                            cnt = ytList.Count;
                        }
                    }

                    return true;

                case 1:
                    //move out
                    ytList = MoveTurnInsideTurnLine(ytList, head, false, false);

                    if (ytList.Count == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    isOutOfBounds = false;
                    youTurnPhase = 10;
                    turnTooCloseTrigger = false;
                    isTurnCreationTooClose = false;

                    if (!AddABSequenceLines()) return false;

                    return true;
            }

            return true;

        }

        private bool CreateABWideTurn()
        {
            //keep from making turns constantly - wait 1.5 seconds
            if (mf.makeUTurnCounter < 4)
            {
                youTurnPhase = 0;
                return true;
            }

            double head = mf.ABLine.abHeading;
            if (!mf.ABLine.isHeadingSameWay) head += Math.PI;
            if (head >= glm.twoPI) head -= glm.twoPI;


            //step 1 turn in to the turnline
            switch (youTurnPhase)
            {
                case 0:
                    //grab the pure pursuit point right on ABLine
                    vec3 onPurePoint = new vec3(mf.ABLine.rEastAB, mf.ABLine.rNorthAB, 0);

                    //how far are we from any turn boundary
                    FindABTurnPoint(onPurePoint);

                    //save a copy for first point
                    inClosestTurnPt = new CClose(closestTurnPt);
                    startOfTurnPt = new CClose(closestTurnPt);

                    //already no turnline
                    if (inClosestTurnPt.turnLineIndex == -1)
                    {
                        FailCreate();
                        return false;
                    }

                    //creates half a circle starting at the crossing point
                    ytList?.Clear();
                    vec3 currentPos = new vec3(inClosestTurnPt.closePt.easting, inClosestTurnPt.closePt.northing, head);
                    ytList.Add(currentPos);

                    //Taken from Dubbins
                    while (Math.Abs(head - currentPos.heading) < Math.PI)
                    {
                        //Update the position of the car
                        currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                        currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                        //Which way are we turning?
                        double turnParameter = isTurnLeft ? -1.0 : 1.0;

                        //Update the heading
                        currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                        //Add the new coordinate to the path
                        ytList.Add(currentPos);
                    }

                    //move the half circle to tangent the turnline
                    isOutOfBounds = true;
                    ytList = MoveTurnInsideTurnLine(ytList, head, true, false);

                    //if it couldn't be done this will trigger
                    if (ytList.Count == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    mf.distancePivotToTurnLine = glm.Distance(ytList[0], mf.pivotAxlePos);

                    youTurnPhase = 1;
                    return true;

                case 1:
                    //build the next line to add sequencelines
                    double widthMinusOverlap = mf.tool.width - mf.tool.overlap;

                    CTrk track = mf.trk.gArr[mf.trk.idx];

                    double distAway = widthMinusOverlap * (mf.ABLine.howManyPathsAway + ((isTurnLeft ^ mf.ABLine.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth)) + (mf.ABLine.isHeadingSameWay ? mf.tool.offset : -mf.tool.offset) + track.nudgeDistance;

                    distAway += (0.5 * widthMinusOverlap);

                    nextCurve = mf.curve.BuildNewOffsetList(distAway, track);

                    //going with or against boundary?
                    bool isTurnLineSameWay = true;
                    double headingDifference = Math.Abs(startOfTurnPt.closePt.heading - ytList[ytList.Count - 1].heading);
                    if (headingDifference > glm.PIBy2 && headingDifference < 3 * glm.PIBy2) isTurnLineSameWay = false;

                    if (!FindABOutTurnPoint(mf.ABLine, ref nextCurve, inClosestTurnPt, isTurnLineSameWay))
                    {
                        //error
                        FailCreate();
                        return false;
                    }
                    outClosestTurnPt = new CClose(closestTurnPt);

                    vec3 pointPos = new vec3(outClosestTurnPt.closePt.easting, outClosestTurnPt.closePt.northing, 0);
                    double headie;
                    if (!isOutSameCurve)
                    {
                        headie = head;
                    }
                    else
                    {
                        headie = head + Math.PI;
                        if (headie >= glm.twoPI) headie -= glm.twoPI;
                    }
                    pointPos.heading = headie;

                    //step 3 create half cirkle in new list
                    ytList2?.Clear();
                    ytList2.Add(pointPos);

                    //Taken from Dubbins
                    while (Math.Abs(headie - pointPos.heading) < Math.PI)
                    {
                        //Update the position of the car
                        pointPos.easting += pointSpacing * Math.Sin(pointPos.heading);
                        pointPos.northing += pointSpacing * Math.Cos(pointPos.heading);

                        //Which way are we turning?
                        double turnParameter = isTurnLeft ? 1.0 : -1.0;

                        //Update the heading
                        pointPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                        //Add the new coordinate to the path
                        ytList2.Add(pointPos);
                    }

                    //move the half circle to tangent the turnline
                    isOutOfBounds = true;
                    ytList2 = MoveTurnInsideTurnLine(ytList2, headie, true, true);

                    if (ytList2.Count == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    youTurnPhase = 2;
                    return true;

                case 2:
                    int cnt1 = ytList.Count;
                    int cnt2 = ytList2.Count;

                    //Validate that both lists have elements to prevent crashes
                    if (cnt1 == 0 || cnt2 == 0)
                    {
                        FailCreate();
                        return false;
                    }

                    //Find if the turn goes same way as turnline heading
                    bool isFirstTurnLineSameWay = true;
                    double firstHeadingDifference = Math.Abs(inClosestTurnPt.turnLineHeading - ytList[ytList.Count - 1].heading);

                    if (firstHeadingDifference > glm.PIBy2 && firstHeadingDifference < 3 * glm.PIBy2) isFirstTurnLineSameWay = false;

                    //finds out start and goal point along the tunline
                    FindInnerTurnPoints(ytList[cnt1 - 1], ytList[0].heading, inClosestTurnPt, isFirstTurnLineSameWay);
                    CClose startClosestTurnPt = new CClose(closestTurnPt);

                    FindInnerTurnPoints(ytList2[cnt2 - 1], ytList2[0].heading + Math.PI, outClosestTurnPt, !isFirstTurnLineSameWay);
                    CClose goalClosestTurnPt = new CClose(closestTurnPt);

                    //we have 2 different turnLine crossings
                    if (startClosestTurnPt.turnLineNum != goalClosestTurnPt.turnLineNum)
                    {
                        FailCreate();
                        return false;
                    }

                    //segment index is the "A" of the segment. segmentIndex+1 would be the "B"
                    //is in and out on same segment? so only 1 segment
                    if (startClosestTurnPt.turnLineIndex == goalClosestTurnPt.turnLineIndex)
                    {
                        for (int a = 0; a < cnt2; a++)
                        {
                            ytList.Add(new vec3(ytList2[cnt2 - 1 - a]));
                        }

                    }
                    else
                    {
                        //multiple segments
                        vec3 tPoint = new vec3();
                        int turnCount = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine.Count;

                        //how many points from turnline do we add
                        int loops = Math.Abs(startClosestTurnPt.turnLineIndex - goalClosestTurnPt.turnLineIndex);

                        //TODO: Generates error if trying to go around more than half of something...... Do we ever?
                        //are we crossing a border?
                        if (loops > (mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine.Count / 2))
                        {
                            if (startClosestTurnPt.turnLineIndex < goalClosestTurnPt.turnLineIndex)
                            {
                                loops = (turnCount - goalClosestTurnPt.turnLineIndex) + startClosestTurnPt.turnLineIndex;
                            }
                            else
                            {
                                loops = (turnCount - startClosestTurnPt.turnLineIndex) + goalClosestTurnPt.turnLineIndex;
                            }
                        }

                        //countExit up - start with B which is next A
                        if (isFirstTurnLineSameWay)
                        {
                            for (int i = 0; i < loops; i++)
                            {
                                if ((startClosestTurnPt.turnLineIndex + 1) >= turnCount) startClosestTurnPt.turnLineIndex = -1;

                                tPoint = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine[startClosestTurnPt.turnLineIndex + 1];
                                startClosestTurnPt.turnLineIndex++;
                                if (startClosestTurnPt.turnLineIndex >= turnCount)
                                    startClosestTurnPt.turnLineIndex = 0;
                                ytList.Add(tPoint);
                            }
                        }
                        else //countExit down = start with A
                        {
                            for (int i = 0; i < loops; i++)
                            {
                                tPoint = mf.bnd.bndList[startClosestTurnPt.turnLineNum].turnLine[startClosestTurnPt.turnLineIndex];
                                startClosestTurnPt.turnLineIndex--;
                                if (startClosestTurnPt.turnLineIndex == -1)
                                    startClosestTurnPt.turnLineIndex = turnCount - 1;
                                ytList.Add(tPoint);
                            }
                        }

                        //add the out from ytList2
                        for (int a = 0; a < cnt2; a++)
                        {
                            ytList.Add(new vec3(ytList2[cnt2 - 1 - a]));
                        }
                    }

                    //fill in the gaps
                    double distance;

                    int cnt = ytList.Count;
                    for (int i = 1; i < cnt - 2; i++)
                    {
                        int j = i + 1;
                        if (j == cnt - 1) continue;
                        distance = glm.DistanceSquared(ytList[i], ytList[j]);
                        if (distance > 1)
                        {
                            vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                                (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                            ytList.Insert(j, pointB);
                            cnt = ytList.Count;
                            i--;
                        }
                    }

                    //calculate the new points headings based on fore and aft of point - smoother turns
                    cnt = ytList.Count;
                    vec3[] arr = new vec3[cnt];
                    cnt -= 2;
                    ytList.CopyTo(arr);
                    ytList.Clear();

                    for (int i = 2; i < cnt; i++)
                    {
                        vec3 pt3 = new vec3(arr[i]);
                        pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting,
                            arr[i + 1].northing - arr[i - 1].northing);
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        ytList.Add(pt3);
                    }

                    //AddABSequenceLines
                    //check to close
                    if (glm.Distance(ytList[0], mf.pivotAxlePos) < 3)
                    {
                        FailCreate();
                        return false;
                    }

                    //are we continuing the same way?
                    isGoingStraightThrough = Math.PI - Math.Abs(Math.Abs(ytList[ytList.Count - 2].heading - ytList[1].heading) - Math.PI) < glm.PIBy2;

                    isOutOfBounds = false;
                    youTurnPhase = 10;
                    turnTooCloseTrigger = false;
                    isTurnCreationTooClose = false;
                    ytList2?.Clear();

                    if (!AddABSequenceLines()) return false;

                    return true;
            }

            //just in case
            return false;
        }

        #endregion


        #region KStyle turns
        public bool KStyleTurnCurve()
        {
            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? -mf.tool.offset * 2.0 : mf.tool.offset * 2.0);
            double pointSpacing = youTurnRadius * 0.1;

            isHeadingSameWay = mf.curve.isHeadingSameWay;

            int turnIndex = mf.bnd.IsPointInsideTurnArea(mf.pivotAxlePos);
            if (mf.makeUTurnCounter < 4 || turnIndex != 0)
            {
                youTurnPhase = 0;
                return true;
            }

            mf.makeUTurnCounter = 0;

            if (!FindCurveTurnPoint(mf.curve, true))
            {
                FailCreate();
                return false;
            }

            //save a copy 
            inClosestTurnPt = new CClose(closestTurnPt);

            ytList?.Clear();

            int count = isHeadingSameWay ? -1 : 1;
            int curveIndex = inClosestTurnPt.curveIndex + count;

            bool pointOutOfBnd = true;
            int stopIfWayOut = 0;

            double head = 0;

            while (pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;

                //creates half a circle starting at the crossing point
                ytList.Clear();
                if (curveIndex >= mf.curve.curList.Count || curveIndex < 0)
                {
                    FailCreate();
                    return false;
                }
                vec3 currentPos = new vec3(mf.curve.curList[curveIndex]);

                curveIndex += count;

                if (!isHeadingSameWay) currentPos.heading += Math.PI;
                if (currentPos.heading >= glm.twoPI) currentPos.heading -= glm.twoPI;

                ytList.Add(currentPos);

                while (Math.Abs(ytList[0].heading - currentPos.heading) < 2.2)
                {
                    //Update the position of the car
                    currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                    currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                    //Which way are we turning?
                    double turnParameter = isTurnLeft ? -1.0 : 1.0;

                    //Update the heading
                    currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                    //Add the new coordinate to the path
                    ytList.Add(currentPos);
                }

                for (int i = 0; i < ytList.Count; i++)
                {
                    if (mf.bnd.IsPointInsideTurnArea(ytList[i]) != 0)
                    {
                        pointOutOfBnd = true;
                        break;
                    }
                }
            }

            //move out
            head = ytList[0].heading;
            double cosHead = Math.Cos(head) * 0.1;
            double sinHead = Math.Sin(head) * 0.1;
            vec3[] arr2 = new vec3[ytList.Count];
            ytList.CopyTo(arr2);
            ytList.Clear();

            //step 2 move the turn inside with steps of 0.1 meter
            int j = 0;
            pointOutOfBnd = false;

            while (!pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;
                mf.distancePivotToTurnLine = glm.DistanceSquared(arr2[0], mf.pivotAxlePos);

                for (int i = 0; i < arr2.Length; i++)
                {
                    arr2[i].easting += (sinHead);
                    arr2[i].northing += (cosHead);
                }

                for (j = 0; j < arr2.Length; j++)
                {
                    int bob = mf.bnd.IsPointInsideTurnArea(arr2[j]);
                    if (bob != 0)
                    {
                        pointOutOfBnd = true;
                        break;
                    }
                }

                if (stopIfWayOut == 300 || (mf.distancePivotToTurnLine < 6))
                {
                    //for some reason it doesn't go inside boundary, return empty list
                    return false;
                }
            }

            ytList.AddRange(arr2);

            //add start extension from curve points
            curveIndex -= count;

            //point used to set next guidance line

            //now we go the other way to turn round
            head = ytList[0].heading;
            head -= Math.PI;
            if (head < -Math.PI) head += glm.twoPI;
            if (head > Math.PI) head -= glm.twoPI;

            if (head >= glm.twoPI) head -= glm.twoPI;
            else if (head < 0) head += glm.twoPI;

            //add the tail to first turn
            head = ytList[ytList.Count - 1].heading;

            vec3 pt;
            for (int i = 1; i <= (int)(3 * turnOffset); i++)
            {
                pt.easting = ytList[ytList.Count - 1].easting + (Math.Sin(head) * 0.5);
                pt.northing = ytList[ytList.Count - 1].northing + (Math.Cos(head) * 0.5);
                pt.heading = 0;
                ytList.Add(pt);
            }

            //leading in line of turn
            for (int i = 0; i < 4; i++)
            {
                ytList.Insert(0, new vec3(mf.curve.curList[curveIndex + i * count]));
            }

            //fill in the gaps
            double distance;

            int cnt = ytList.Count;
            for (int i = 1; i < cnt - 2; i++)
            {
                j = i + 1;
                if (j == cnt - 1) continue;
                distance = glm.DistanceSquared(ytList[i], ytList[j]);
                if (distance > 1)
                {
                    vec3 pointB = new vec3((ytList[i].easting + ytList[j].easting) / 2.0,
                        (ytList[i].northing + ytList[j].northing) / 2.0, ytList[i].heading);

                    ytList.Insert(j, pointB);
                    cnt = ytList.Count;
                    i--;
                }
            }

            //calculate line headings
            vec3[] arr = new vec3[ytList.Count];
            ytList.CopyTo(arr);
            ytList.Clear();

            for (int i = 0; i < arr.Length - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                ytList.Add(arr[i]);
            }

            mf.distancePivotToTurnLine = glm.Distance(ytList[0], mf.pivotAxlePos);

            isOutOfBounds = false;
            youTurnPhase = 10;
            turnTooCloseTrigger = false;
            isTurnCreationTooClose = false;

            return true;
        }

        public bool KStyleTurnAB()
        {
            double pointSpacing = youTurnRadius * 0.1;

            int turnIndex = mf.bnd.IsPointInsideTurnArea(mf.pivotAxlePos);
            if (mf.makeUTurnCounter < 4 || turnIndex != 0)
            {
                youTurnPhase = 0;
                return true;
            }

            mf.makeUTurnCounter = 0;

            //step 1 turn in to the turnline
            if (youTurnPhase == 0)
            {
                isOutOfBounds = true;


                //timer.Start();
                //grab the pure pursuit point right on ABLine
                vec3 onPurePoint = new vec3(mf.ABLine.rEastAB, mf.ABLine.rNorthAB, 0);

                //how far are we from any turn boundary
                FindClosestTurnPoint(onPurePoint);

                //save a copy for first point
                inClosestTurnPt = new CClose(closestTurnPt);

                //already no turnline
                if (inClosestTurnPt.turnLineIndex == -1) return false;

                //calculate the distance to the turnline
                mf.distancePivotToTurnLine = glm.Distance(mf.pivotAxlePos, closestTurnPt.closePt);

                //point on AB line closest to pivot axle point from ABLine PurePursuit aka where we are
                rEastYT = mf.ABLine.rEastAB;
                rNorthYT = mf.ABLine.rNorthAB;
                isHeadingSameWay = mf.ABLine.isHeadingSameWay;
                double head = mf.ABLine.abHeading;

                if (!isHeadingSameWay) head += Math.PI;
                if (head >= glm.twoPI) head -= glm.twoPI;

                //thistance to turnline from where we are
                double turnDiagDistance = mf.distancePivotToTurnLine;

                //moves the point to the crossing with the turnline
                rEastYT += (Math.Sin(head) * turnDiagDistance);
                rNorthYT += (Math.Cos(head) * turnDiagDistance);

                //creates half a circle starting at the crossing point
                ytList.Clear();
                vec3 currentPos = new vec3(rEastYT, rNorthYT, head);
                ytList.Add(currentPos);

                //make semi circle - not quite
                while (Math.Abs(head - currentPos.heading) < 2.2)
                {
                    //Update the position of the car
                    currentPos.easting += pointSpacing * Math.Sin(currentPos.heading);
                    currentPos.northing += pointSpacing * Math.Cos(currentPos.heading);

                    //Which way are we turning?
                    double turnParameter = 1.0;

                    if (isTurnLeft) turnParameter = -1.0;

                    //Update the heading
                    currentPos.heading += (pointSpacing / youTurnRadius) * turnParameter;

                    //Add the new coordinate to the path
                    ytList.Add(currentPos);
                }

                //move the half circle to tangent the turnline
                ytList = MoveABTurnInsideTurnLine(ytList, head);

                //if it couldn't be done this will trigger
                if (ytList.Count < 5 || semiCircleIndex == -1)
                {
                    FailCreate();
                    return false;
                }

                //grab the vehicle widths and offsets
                double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnLeft ? mf.tool.offset : -mf.tool.offset);

                //add the tail to first turn
                int count = ytList.Count;
                head = ytList[count - 1].heading;

                vec3 pt;
                for (int i = 1; i <= (int)(3 * turnOffset); i++)
                {
                    pt.easting = ytList[count - 1].easting + (Math.Sin(head) * i * 0.5);
                    pt.northing = ytList[count - 1].northing + (Math.Cos(head) * i * 0.5);
                    pt.heading = 0;
                    ytList.Add(pt);
                }

                //leading in line of turn
                head = mf.ABLine.abHeading;
                if (isHeadingSameWay) head += Math.PI;
                if (head >= glm.twoPI) head -= glm.twoPI;

                for (int a = 0; a < 8; a++)
                {
                    pt.easting = ytList[0].easting + (Math.Sin(head) * 0.511);
                    pt.northing = ytList[0].northing + (Math.Cos(head) * 0.511);
                    pt.heading = ytList[0].heading;
                    ytList.Insert(0, pt);
                }

                //calculate line headings
                vec3[] arr = new vec3[ytList.Count];
                ytList.CopyTo(arr);
                ytList.Clear();

                //headings of line one
                for (int i = 0; i < arr.Length - 1; i++)
                {
                    arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                    if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                    ytList.Add(arr[i]);
                }

                mf.distancePivotToTurnLine = glm.Distance(ytList[0], mf.pivotAxlePos);

                isOutOfBounds = false;
                youTurnPhase = 10;
                turnTooCloseTrigger = false;
                isTurnCreationTooClose = false;
                return true;
            }

            return true;
        }

        private List<vec3> MoveABTurnInsideTurnLine(List<vec3> uTurnList, double head)
        {
            //step 1 make array out of the list so that we can modify the position
            double cosHead = Math.Cos(head);
            double sinHead = Math.Sin(head);
            int cnt = uTurnList.Count;
            vec3[] arr2 = new vec3[cnt];
            uTurnList.CopyTo(arr2);
            uTurnList.Clear();

            semiCircleIndex = -1;
            //step 2 move the turn inside with steps of 1 meter
            bool pointOutOfBnd = true;
            int j = 0;
            int stopIfWayOut = 0;
            while (pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;
                mf.distancePivotToTurnLine = glm.DistanceSquared(arr2[0], mf.pivotAxlePos);

                for (int i = 0; i < cnt; i++)
                {
                    arr2[i].easting -= (sinHead);
                    arr2[i].northing -= (cosHead);
                }

                for (; j < cnt; j += 1)
                {
                    if (mf.bnd.IsPointInsideTurnArea(arr2[j]) != 0)
                    {
                        pointOutOfBnd = true;
                        if (j > 0) j--;
                        break;
                    }
                }

                if (stopIfWayOut == 1000 || (mf.distancePivotToTurnLine < 6))
                {
                    //for some reason it doesn't go inside boundary, return empty list
                    return uTurnList;
                }
            }

            //step 5, we ar now inside turnfence by 0-0.1 meters, move the turn forward until it hits the turnfence in steps of 0.05 meters
            while (!pointOutOfBnd)
            {

                for (int i = 0; i < cnt; i++)
                {
                    arr2[i].easting += (sinHead * 0.1);
                    arr2[i].northing += (cosHead * 0.1);
                }

                for (int a = 0; a < cnt; a++)
                {
                    if (mf.bnd.IsPointInsideTurnArea(arr2[a]) != 0)
                    {
                        semiCircleIndex = a;
                        uTurnList.AddRange(arr2);
                        return uTurnList;
                    }
                }
            }

            //if empty - no creation.
            return uTurnList;

            //step 6, we have now placed the turn, so send it back in a list

        }

        public void FindClosestTurnPoint(vec3 fromPt)
        {
            double eP = fromPt.easting;
            double nP = fromPt.northing;
            double eAB, nAB;
            turnClosestList?.Clear();

            CClose cClose;

            if (mf.ABLine.isHeadingSameWay)
            {
                eAB = mf.ABLine.currentLinePtB.easting;
                nAB = mf.ABLine.currentLinePtB.northing;
            }
            else
            {
                eAB = mf.ABLine.currentLinePtA.easting;
                nAB = mf.ABLine.currentLinePtA.northing;
            }

            turnClosestList.Clear();

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                for (int i = 0; i < mf.bnd.bndList[j].turnLine.Count - 1; i++)
                {
                    int res = GetLineIntersection(
                        mf.bnd.bndList[j].turnLine[i].easting,
                        mf.bnd.bndList[j].turnLine[i].northing,
                        mf.bnd.bndList[j].turnLine[i + 1].easting,
                        mf.bnd.bndList[j].turnLine[i + 1].northing,
                        eP, nP, eAB, nAB, ref iE, ref iN
                    );

                    if (res == 1)
                    {
                        cClose = new CClose();
                        cClose.closePt.easting = iE;
                        cClose.closePt.northing = iN;

                        double hed = Math.Atan2(mf.bnd.bndList[j].turnLine[i + 1].easting - mf.bnd.bndList[j].turnLine[i].easting,
                            mf.bnd.bndList[j].turnLine[i + 1].northing - mf.bnd.bndList[j].turnLine[i].northing);
                        if (hed < 0) hed += glm.twoPI;
                        cClose.closePt.heading = hed;
                        cClose.turnLineNum = j;
                        cClose.turnLineIndex = i;

                        turnClosestList.Add(new CClose(cClose));
                    }
                }
            }

            //determine closest point
            double minDistance = double.MaxValue;

            if (turnClosestList.Count > 0)
            {
                for (int i = 0; i < turnClosestList.Count; i++)
                {
                    double dist = (((fromPt.easting - turnClosestList[i].closePt.easting) * (fromPt.easting - turnClosestList[i].closePt.easting))
                                    + ((fromPt.northing - turnClosestList[i].closePt.northing) * (fromPt.northing - turnClosestList[i].closePt.northing)));

                    if (minDistance >= dist)
                    {

                        minDistance = dist;
                        closestTurnPt = new CClose(turnClosestList[i]);
                    }
                }
            }
        }
        #endregion

        #region FindTurnPoint

        public bool FindCurveOutTurnPoint(CABCurve thisCurve, ref List<vec3> nextCurve, CClose inPt, bool isTurnLineSameWay)
        {
            int a = isTurnLineSameWay ? 1 : -1;

            int turnLineIndex = inPt.turnLineIndex;
            int turnLineNum = inPt.turnLineNum;
            int stopTurnLineIndex = inPt.turnLineIndex - a;
            if (stopTurnLineIndex < 0) stopTurnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 3;
            if (stopTurnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 1) turnLineIndex = 3;


            for (; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 2; //AAA could be less than 0???
                if (turnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 2) turnLineIndex = 0;

                for (int i = 0; i < nextCurve.Count - 2; i++)
                {
                    int res = GetLineIntersection(
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                    nextCurve[i].easting,
                                    nextCurve[i].northing,
                                    nextCurve[i + 1].easting,
                                    nextCurve[i + 1].northing,
                                     ref iE, ref iN);
                    if (res == 1)
                    {
                        closestTurnPt = new CClose();
                        closestTurnPt.closePt.easting = iE;
                        closestTurnPt.closePt.northing = iN;
                        closestTurnPt.closePt.heading = nextCurve[i].heading;
                        closestTurnPt.turnLineIndex = turnLineIndex;
                        closestTurnPt.curveIndex = i;
                        closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                        closestTurnPt.turnLineNum = turnLineNum;
                        return true;
                    }
                }

                for (int i = 0; i < thisCurve.curList.Count - 2; i++)
                {
                    int res = GetLineIntersection(
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                    thisCurve.curList[i].easting,
                                    thisCurve.curList[i].northing,
                                    thisCurve.curList[i + 1].easting,
                                    thisCurve.curList[i + 1].northing,

                                     ref iE, ref iN);
                    if (res == 1)
                    {
                        if ((i < inPt.curveIndex && thisCurve.isHeadingSameWay) || (i > inPt.curveIndex && !thisCurve.isHeadingSameWay))
                        {
                            return false; //hitting the curve behind us
                        }
                        else if (i == inPt.curveIndex)
                        {
                            //do nothing hitting the curve at the same place as in
                        }
                        else
                        {
                            closestTurnPt = new CClose();
                            closestTurnPt.closePt.easting = iE;
                            closestTurnPt.closePt.northing = iN;
                            closestTurnPt.closePt.heading = thisCurve.curList[i].heading; //ändrad nyss till this curve
                            closestTurnPt.turnLineIndex = turnLineIndex;
                            closestTurnPt.curveIndex = i;
                            closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                            closestTurnPt.turnLineNum = turnLineNum;
                            isOutSameCurve = true;
                            nextCurve = thisCurve.curList;//???? already created the line so why change it
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool FindABOutTurnPoint(CABLine thisCurve, ref List<vec3> nextCurve, CClose inPt, bool isTurnLineSameWay)
        {
            int a = isTurnLineSameWay ? 1 : -1;

            int turnLineIndex = inPt.turnLineIndex;
            int turnLineNum = inPt.turnLineNum;
            int stopTurnLineIndex = inPt.turnLineIndex - a;
            if (stopTurnLineIndex < 0) stopTurnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 3;
            if (stopTurnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 1) turnLineIndex = 3;


            for (; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 2;
                if (turnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 2) turnLineIndex = 0;

                if (nextCurve.Count > 1)
                {
                    int res = GetLineIntersection(
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                    mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                    nextCurve[0].easting,
                                    nextCurve[0].northing,
                                    nextCurve[1].easting,
                                    nextCurve[1].northing,
                                     ref iE, ref iN);
                    if (res == 1)
                    {
                        closestTurnPt = new CClose();
                        closestTurnPt.closePt.easting = iE;
                        closestTurnPt.closePt.northing = iN;
                        closestTurnPt.closePt.heading = thisCurve.abHeading;
                        closestTurnPt.turnLineIndex = turnLineIndex;
                        closestTurnPt.curveIndex = -1;
                        closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                        closestTurnPt.turnLineNum = turnLineNum;
                        return true;
                    }
                }



                int res2 = GetLineIntersection(
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                thisCurve.currentLinePtA.easting,
                                thisCurve.currentLinePtA.northing,
                                thisCurve.currentLinePtB.easting,
                                thisCurve.currentLinePtB.northing,

                                 ref iE, ref iN);
                if (res2 == 1)
                {
                    double hed;
                    if (mf.ABLine.isHeadingSameWay) hed = Math.Atan2(mf.ABLine.rEastAB - iE, mf.ABLine.rNorthAB - iN);
                    else hed = Math.Atan2(iE - mf.ABLine.rEastAB, iN - mf.ABLine.rNorthAB);
                    if (hed < 0) hed += glm.twoPI;
                    hed = Math.Round(hed, 3);
                    double hedAB = Math.Round(thisCurve.abHeading, 3);

                    if (hed == hedAB)
                    {
                        return false; //hitting the curve behind us
                    }
                    else if (turnLineIndex == inPt.turnLineIndex)
                    {
                        //do nothing hitting the curve at the same place as in
                    }
                    else
                    {
                        closestTurnPt = new CClose();
                        closestTurnPt.closePt.easting = iE;
                        closestTurnPt.closePt.northing = iN;
                        closestTurnPt.closePt.heading = thisCurve.abHeading;
                        closestTurnPt.turnLineIndex = turnLineIndex;
                        closestTurnPt.curveIndex = -1;
                        closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                        closestTurnPt.turnLineNum = turnLineNum;
                        isOutSameCurve = true;
                        return true;
                    }
                }

            }
            return false;
        }

        private bool FindInnerTurnPoints(vec3 fromPt, double inDirection, CClose refClosePt, bool isTurnLineSameWay)
        {
            double eP, nP;

            eP = fromPt.easting + Math.Sin(inDirection);
            nP = fromPt.northing + Math.Cos(inDirection);

            int a = isTurnLineSameWay ? 1 : -1;

            int turnLineIndex = refClosePt.turnLineIndex;
            int turnLineNum = refClosePt.turnLineNum;
            int stopTurnLineIndex = refClosePt.turnLineIndex - a;
            if (stopTurnLineIndex < 0) stopTurnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 3;
            if (stopTurnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 1) turnLineIndex = 3;


            for (; turnLineIndex != stopTurnLineIndex; turnLineIndex += a)
            {
                if (turnLineIndex < 0) turnLineIndex = mf.bnd.bndList[turnLineNum].turnLine.Count - 2;
                if (turnLineIndex > mf.bnd.bndList[turnLineNum].turnLine.Count - 2) turnLineIndex = 0;


                int res = GetLineIntersection(
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].easting,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].northing,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].easting,
                                mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex + 1].northing,

                                fromPt.easting,
                                fromPt.northing, eP, nP, ref iE, ref iN);
                if (res == 1)
                {
                    closestTurnPt = new CClose();
                    closestTurnPt.closePt.easting = iE;
                    closestTurnPt.closePt.northing = iN;
                    closestTurnPt.closePt.heading = -1; //isnt needed but could be calculated
                    closestTurnPt.turnLineIndex = turnLineIndex;
                    closestTurnPt.curveIndex = -1;
                    closestTurnPt.turnLineHeading = mf.bnd.bndList[turnLineNum].turnLine[turnLineIndex].heading;
                    closestTurnPt.turnLineNum = turnLineNum;
                    return true;
                }

            }
            return false;
        }

        private bool FindCurveTurnPoint(CABCurve thisCurve, bool noIdea)
        {
            //AAA Is updated but not tested....
            //find closet AB Curve point that will cross and go out of bounds
            int Count = mf.curve.isHeadingSameWay ? 1 : -1;
            int turnNum = 99;
            int j;

            closestTurnPt = new CClose();

            if (mf.trk.idx < 0 || mf.trk.gArr.Count < mf.trk.idx) return true;
            CTrk track = mf.trk.gArr[mf.trk.idx];

            bool loop = track.mode == TrackMode.bndCurve || track.mode == TrackMode.waterPivot;

            for (j = thisCurve.currentLocationIndex; j > 0 && j < thisCurve.curList.Count; j += Count)
            {
                if (j < 0)
                {
                    if (loop)
                    {
                        loop = false;
                        j = thisCurve.curList.Count;
                        continue;
                    }
                    break;
                }
                else if (j >= thisCurve.curList.Count)
                {
                    if (loop)
                    {
                        loop = false;
                        j = -1;
                        continue;
                    }
                    break;
                }

                int turnIndex = mf.bnd.IsPointInsideTurnArea(thisCurve.curList[j]);
                if (turnIndex != 0)
                {
                    closestTurnPt.curveIndex = j - Count;
                    closestTurnPt.turnLineNum = turnIndex;
                    turnNum = turnIndex;
                    break;
                }
            }

            if (turnNum < 0) //uturn will be on outer boundary turn
            {
                closestTurnPt.turnLineNum = 0;
                turnNum = 0;
            }
            else if (turnNum == 99)
            {
                //curve does not cross a boundary - oops
                isTurnCreationNotCrossingError = true;
                return false;
            }

            if (closestTurnPt.curveIndex == -1)
            {
                isTurnCreationNotCrossingError = true;
                return false;
            }


            for (int i = 0; i < mf.bnd.bndList[turnNum].turnLine.Count - 1; i++)
            {
                int res = GetLineIntersection(
                        mf.bnd.bndList[turnNum].turnLine[i].easting,
                        mf.bnd.bndList[turnNum].turnLine[i].northing,
                        mf.bnd.bndList[turnNum].turnLine[i + 1].easting,
                        mf.bnd.bndList[turnNum].turnLine[i + 1].northing,

                        thisCurve.curList[closestTurnPt.curveIndex].easting,
                        thisCurve.curList[closestTurnPt.curveIndex].northing,
                        thisCurve.curList[closestTurnPt.curveIndex + Count].easting,
                        thisCurve.curList[closestTurnPt.curveIndex + Count].northing,

                         ref iE, ref iN);

                if (res == 1)
                {
                    closestTurnPt.closePt.easting = iE;
                    closestTurnPt.closePt.northing = iN;
                    if (noIdea)
                    {
                        double hed = Math.Atan2(mf.bnd.bndList[turnNum].turnLine[i + 1].easting - mf.bnd.bndList[turnNum].turnLine[i].easting,
                            mf.bnd.bndList[turnNum].turnLine[i + 1].northing - mf.bnd.bndList[turnNum].turnLine[i].northing);
                        if (hed < 0) hed += glm.twoPI;
                        closestTurnPt.closePt.heading = hed;
                        closestTurnPt.turnLineIndex = i;
                    }
                    else
                    {
                        closestTurnPt.closePt.heading = thisCurve.curList[closestTurnPt.curveIndex].heading;
                        closestTurnPt.turnLineIndex = i;
                        closestTurnPt.turnLineNum = turnNum;
                        closestTurnPt.turnLineHeading = mf.bnd.bndList[turnNum].turnLine[i].heading;
                        if (!thisCurve.isHeadingSameWay && closestTurnPt.curveIndex > 0) closestTurnPt.curveIndex--;
                    }
                    break;
                }
            }

            return closestTurnPt.turnLineIndex != -1 && closestTurnPt.curveIndex != -1;
        }

        private void FindABTurnPoint(vec3 fromPt)
        {
            double eP = fromPt.easting;
            double nP = fromPt.northing;
            double eAB, nAB;
            turnClosestList?.Clear();

            CClose cClose;

            if (mf.ABLine.isHeadingSameWay)
            {
                eAB = mf.ABLine.currentLinePtB.easting;
                nAB = mf.ABLine.currentLinePtB.northing;
            }
            else
            {
                eAB = mf.ABLine.currentLinePtA.easting;
                nAB = mf.ABLine.currentLinePtA.northing;
            }

            turnClosestList.Clear();

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                for (int i = 0; i < mf.bnd.bndList[j].turnLine.Count - 1; i++)
                {
                    int res = GetLineIntersection(
                        mf.bnd.bndList[j].turnLine[i].easting,
                        mf.bnd.bndList[j].turnLine[i].northing,
                        mf.bnd.bndList[j].turnLine[i + 1].easting,
                        mf.bnd.bndList[j].turnLine[i + 1].northing,
                        eP, nP, eAB, nAB, ref iE, ref iN
                    );

                    if (res == 1)
                    {
                        cClose = new CClose();
                        cClose.closePt.easting = iE;
                        cClose.closePt.northing = iN;

                        double hed = Math.Atan2(mf.bnd.bndList[j].turnLine[i + 1].easting - mf.bnd.bndList[j].turnLine[i].easting,
                            mf.bnd.bndList[j].turnLine[i + 1].northing - mf.bnd.bndList[j].turnLine[i].northing);
                        if (hed < 0) hed += glm.twoPI;
                        cClose.closePt.heading = hed;
                        cClose.turnLineHeading = mf.bnd.bndList[j].turnLine[i].heading;
                        cClose.turnLineNum = j;
                        cClose.turnLineIndex = i;

                        turnClosestList.Add(new CClose(cClose));
                    }
                }
            }

            //determine closest point
            double minDistance = double.MaxValue;

            if (turnClosestList.Count > 0)
            {
                for (int i = 0; i < turnClosestList.Count; i++)
                {
                    double dist = (((fromPt.easting - turnClosestList[i].closePt.easting) * (fromPt.easting - turnClosestList[i].closePt.easting))
                                    + ((fromPt.northing - turnClosestList[i].closePt.northing) * (fromPt.northing - turnClosestList[i].closePt.northing)));

                    if (minDistance >= dist)
                    {

                        minDistance = dist;
                        closestTurnPt = new CClose(turnClosestList[i]);
                    }
                }
            }
        }

        #endregion

        #region SequenceLines

        private bool AddABSequenceLines()
        {
            double inhead = mf.ABLine.abHeading;
            if (!mf.ABLine.isHeadingSameWay) inhead += Math.PI;
            if (inhead > glm.twoPI) inhead -= glm.twoPI;

            double outhead = inhead;
            if (isOutSameCurve) outhead += Math.PI;
            if (outhead > glm.twoPI) outhead -= glm.twoPI;

            //how many points straight out
            double lenny = 15;

            vec3 pt;
            for (int a = 0; a < lenny; a++)
            {
                pt.easting = ytList[0].easting - (Math.Sin(inhead) * pointSpacing);
                pt.northing = ytList[0].northing - (Math.Cos(inhead) * pointSpacing);
                pt.heading = inhead;
                ytList.Insert(0, pt);
            }

            int count = ytList.Count;

            for (int i = 0; i <= lenny; i++)
            {
                pt.easting = ytList[count - 1].easting - (Math.Sin(outhead) * i * pointSpacing);
                pt.northing = ytList[count - 1].northing - (Math.Cos(outhead) * i * pointSpacing);
                pt.heading = outhead;
                ytList.Add(pt);
            }

            double distancePivotToTurnLine;
            count = ytList.Count;
            for (int i = 0; i < count; i += 2)
            {
                distancePivotToTurnLine = glm.DistanceSquared(ytList[i], mf.pivotAxlePos);
                if (distancePivotToTurnLine > 3)
                {
                    isTurnCreationTooClose = false;
                }
                else
                {
                    //set the flag to Critical stop machine
                    FailCreate();
                    return false;
                }
            }

            return true;
        }

        //TODO: is for some reason making longer for omegaturn....
        private bool AddCurveSequenceLines()
        {
            //how many points striaght out
            double lenny = 5;
            bool sameWay = mf.curve.isHeadingSameWay;
            int a = sameWay ? -1 : 1;

            for (int i = 0; i < lenny && i > -lenny; i += a)
            {
                ytList.Insert(0, new vec3(mf.curve.curList[inClosestTurnPt.curveIndex]));
                inClosestTurnPt.curveIndex += a;
                if (inClosestTurnPt.curveIndex < 2 || inClosestTurnPt.curveIndex > mf.curve.curList.Count - 3)
                {
                    FailCreate();
                    return false;
                }
            }
            if (isOutSameCurve) sameWay = !sameWay;
            a = sameWay ? -1 : 1;

            for (int i = 0; i < lenny && i > -lenny; i += a)
            {
                ytList.Add(new vec3(nextCurve[outClosestTurnPt.curveIndex]));
                outClosestTurnPt.curveIndex += a;
                if (outClosestTurnPt.curveIndex < 2 || outClosestTurnPt.curveIndex > mf.curve.curList.Count - 3)
                {
                    FailCreate();
                    return false;
                }
            }

            return true;
        }

        #endregion

        /// <summary>/// Calculates the crosing point between the two lines and returns the easting and northing in the reference vaiables
        /// returns 0 for no collision and 1 for collision /// </summary>
        public int GetLineIntersection(double p0x, double p0y, double p1x, double p1y,
                double p2x, double p2y, double p3x, double p3y, ref double iEast, ref double iNorth)
        {
            double s1x, s1y, s2x, s2y;
            s1x = p1x - p0x;
            s1y = p1y - p0y;

            s2x = p3x - p2x;
            s2y = p3y - p2y;

            double s, t;
            s = (-s1y * (p0x - p2x) + s1x * (p0y - p2y)) / (-s2x * s1y + s1x * s2y);

            if (s >= 0 && s <= 1)
            {
                //check oher side
                t = (s2x * (p0y - p2y) - s2y * (p0x - p2x)) / (-s2x * s1y + s1x * s2y);
                if (t >= 0 && t <= 1)
                {
                    // Collision detected
                    iEast = p0x + (t * s1x);
                    iNorth = p0y + (t * s1y);
                    return 1;
                }
            }

            return 0; // No collision
        }

        private List<vec3> MoveTurnInsideTurnLine(List<vec3> uTurnList, double head, bool deleteSecondHalf, bool invertHeading)
        {
            //step 1 make array out of the list so that we can modify the position
            double cosHead = Math.Cos(head);
            double sinHead = Math.Sin(head);
            int cnt = uTurnList.Count;
            vec3[] arr2 = new vec3[cnt];
            uTurnList.CopyTo(arr2);
            uTurnList.Clear();

            semiCircleIndex = -1;
            //step 2 move the turn inside with steps of 1 meter
            bool pointOutOfBnd = isOutOfBounds;
            int j = 0;
            int stopIfWayOut = 0;
            while (pointOutOfBnd)
            {
                stopIfWayOut++;
                pointOutOfBnd = false;
                mf.distancePivotToTurnLine = glm.Distance(arr2[0], mf.pivotAxlePos);

                for (int i = 0; i < cnt; i++)
                {
                    arr2[i].easting -= (sinHead);
                    arr2[i].northing -= (cosHead);
                }

                for (; j < cnt; j += 1)
                {
                    if (mf.bnd.IsPointInsideTurnArea(arr2[j]) != 0)
                    {
                        pointOutOfBnd = true;
                        if (j > 0) j--;
                        break;
                    }
                }

                if (stopIfWayOut == 1000 || (mf.distancePivotToTurnLine < 3))
                {
                    //for some reason it doesn't go inside boundary, return empty list
                    return uTurnList;
                }
            }

            //step 3, we ar now inside turnline, move the turn forward until it hits the turnfence in steps of 0.1 meters
            while (!pointOutOfBnd)
            {
                for (int i = 0; i < cnt; i++)
                {
                    arr2[i].easting += (sinHead * 0.1);
                    arr2[i].northing += (cosHead * 0.1);
                }

                for (int a = 0; a < cnt; a++)
                {
                    if (mf.bnd.IsPointInsideTurnArea(arr2[a]) != 0)
                    {
                        semiCircleIndex = a;
                        pointOutOfBnd = true;
                        break;

                    }
                }
            }

            //step 4, Should we delete the points after the one that is outside? and where the points made in the wrong direction?
            for (int i = 0; i < cnt; i++)
            {
                if (i == semiCircleIndex && deleteSecondHalf)
                    break;
                if (invertHeading) arr2[i].heading += Math.PI;
                if (arr2[i].heading >= glm.twoPI) arr2[i].heading -= glm.twoPI;
                else if (arr2[i].heading < 0) arr2[i].heading += glm.twoPI;
                uTurnList.Add(arr2[i]);
            }

            //we have succesfully moved the turn inside
            isOutOfBounds = false;

            //if empty - no creation.
            return uTurnList;

        }

        public void SmoothYouTurn(int smPts)
        {
            //countExit the reference list of original curve
            int cnt = ytList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = ytList[s].easting;
                arr[s].northing = ytList[s].northing;
                arr[s].heading = ytList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = ytList[s].easting;
                arr[s].northing = ytList[s].northing;
                arr[s].heading = ytList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += ytList[j + i].easting;
                    arr[i].northing += ytList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = ytList[i].heading;
            }

            ytList?.Clear();

            //calculate new headings on smoothed line
            for (int i = 1; i < cnt - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += glm.twoPI;
                ytList.Add(arr[i]);
            }
        }

        //called to initiate turn
        public void YouTurnTrigger()
        {
            //trigger pulled
            isYouTurnTriggered = true;

            if (!isGoingStraightThrough)
            {
                mf.curve.howManyPathsAway += (isTurnLeft ^ mf.curve.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth;
                mf.curve.isHeadingSameWay = !mf.curve.isHeadingSameWay;

                mf.ABLine.howManyPathsAway += (isTurnLeft ^ mf.ABLine.isHeadingSameWay) ? rowSkipsWidth : -rowSkipsWidth;
                mf.ABLine.isHeadingSameWay = !mf.ABLine.isHeadingSameWay;

                switch (skipMode)
                {
                    case SkipMode.Alternative:
                        if (rowSkipsWidth2 > 1)
                        {
                            if (--turnSkips == 0)
                            {
                                isTurnLeft = !isTurnLeft;
                                turnSkips = rowSkipsWidth2 * 2 - 1;
                            }
                            else if (previousBigSkip = !previousBigSkip)
                                rowSkipsWidth = rowSkipsWidth2 - 1;
                            else
                                rowSkipsWidth = rowSkipsWidth2;
                        }
                        else isTurnLeft = !isTurnLeft;
                        break;

                    case SkipMode.Outward:
                        rowSkipsWidth = rowSkipsWidth + 1;
                        break;

                    case SkipMode.Inward:
                        rowSkipsWidth = Math.Max(1, rowSkipsWidth - 1);
                        break;

                    default:
                        isTurnLeft = !isTurnLeft;
                        break;
                }
            }
        }

        //Normal copmpletion of youturn
        public void CompleteYouTurn()
        {
            isYouTurnTriggered = false;
            ResetCreatedYouTurn();
            mf.sounds.isBoundAlarming = false;
        }

        public void Set_Alternate_skips()
        {
            rowSkipsWidth2 = rowSkipsWidth;
            turnSkips = rowSkipsWidth2 * 2 - 1;
            previousBigSkip = false;
        }

        //something went seriously wrong so reset everything
        public void ResetYouTurn()
        {
            //fix you turn
            isYouTurnTriggered = false;
            mf.makeUTurnCounter = 0;
            ytList?.Clear();
            ResetCreatedYouTurn();
            mf.sounds.isBoundAlarming = false;
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;
            mf.p_239.pgn[mf.p_239.uturn] = 0;
        }

        public void ResetCreatedYouTurn()
        {
            youTurnPhase = 0;
            ytList?.Clear();
            mf.makeUTurnCounter = 0;
            mf.p_239.pgn[mf.p_239.uturn] = 0;
            isOutSameCurve = false;
            isGoingStraightThrough = false;
            maxProgressIndexReached = 0;
        }

        public void FailCreate()
        {
            //fail
            isOutOfBounds = true;
            isTurnCreationTooClose = true;
            mf.mc.isOutOfBounds = true;
            youTurnPhase = 11;
        }

        public void BuildManualYouLateral(bool isTurnLeft)
        {
            //point on AB line closest to pivot axle point from ABLine PurePursuit
            if (mf.trk.idx > -1 && mf.trk.gArr.Count > 0)
            {
                if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.AB)
                {
                    isHeadingSameWay = mf.ABLine.isHeadingSameWay;
                }
                else
                {
                    isHeadingSameWay = mf.curve.isHeadingSameWay;
                }
                if (isHeadingSameWay == isTurnLeft)
                {
                    mf.curve.howManyPathsAway += 1;
                    mf.ABLine.howManyPathsAway += 1;
                }
                else
                {
                    mf.curve.howManyPathsAway -= 1;
                    mf.ABLine.howManyPathsAway -= 1;
                }
                return;
            }
            else return;
        }

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight, bool isTurnButtonTriggered)
        {
            double head;
            //point on AB line closest to pivot axle point from ABLine PurePursuit
            if (mf.trk.idx > -1 && mf.trk.gArr.Count > 0)
            {
                if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.AB)
                {
                    rEastYT = mf.ABLine.rEastAB;
                    rNorthYT = mf.ABLine.rNorthAB;
                    isHeadingSameWay = mf.ABLine.isHeadingSameWay;
                    head = mf.ABLine.abHeading;
                }

                else
                {
                    rEastYT = mf.curve.rEastCu;
                    rNorthYT = mf.curve.rNorthCu;
                    isHeadingSameWay = mf.curve.isHeadingSameWay;
                    head = mf.curve.manualUturnHeading;
                }
            }
            else return;

            //grab the vehicle widths and offsets
            double turnOffset = (mf.tool.width - mf.tool.overlap) * rowSkipsWidth + (isTurnRight ? mf.tool.offset * 2.0 : -mf.tool.offset * 2.0);

            CDubins dubYouTurnPath = new CDubins();
            CDubins.turningRadius = youTurnRadius;

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (!isHeadingSameWay) head += 3.14;
            else head -= 0.01;

            //move the start forward 2 meters, this point is critical to formation of uturn
            rEastYT += (Math.Sin(head) * 4);
            rNorthYT += (Math.Cos(head) * 4);

            //now we have our start point
            vec3 start = new vec3(rEastYT, rNorthYT, head);
            vec3 goal = new vec3();

            //now we go the other way to turn round
            head -= Math.PI;
            if (head < 0) head += glm.twoPI;

            //set up the goal point for Dubins
            goal.heading = head;
            if (isTurnButtonTriggered)
            {
                if (isTurnRight)
                {
                    goal.easting = rEastYT - (Math.Cos(-head) * turnOffset);
                    goal.northing = rNorthYT - (Math.Sin(-head) * turnOffset);
                }
                else
                {
                    goal.easting = rEastYT + (Math.Cos(-head) * turnOffset);
                    goal.northing = rNorthYT + (Math.Sin(-head) * turnOffset);
                }
            }

            //generate the turn points
            ytList = dubYouTurnPath.GenerateDubins(start, goal);

            isTurnLeft = !isTurnRight;
            YouTurnTrigger();
        }

        public int onA;
        private int maxProgressIndexReached = 0;

        //determine distance from youTurn guidance line
        public bool DistanceFromYouTurnLine()
        {
            //grab a copy from main - the steer position
            double minDistA = 1000000, minDistB = 1000000;
            int ptCount = ytList.Count;

            if (ptCount > 0)
            {
                if (mf.isStanleyUsed)
                {
                    vec3 pivot = mf.steerAxlePos;

                    //find the closest 2 points to current fix
                    //Start search from max progress minus small lookback to prevent jumping back to start
                    //when start and end positions overlap
                    int searchStartIndex = Math.Max(0, maxProgressIndexReached - 5);
                    for (int t = searchStartIndex; t < ptCount; t++)
                    {
                        double dist = ((pivot.easting - ytList[t].easting) * (pivot.easting - ytList[t].easting))
                                        + ((pivot.northing - ytList[t].northing) * (pivot.northing - ytList[t].northing));
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

                    if (minDistA > 16)
                    {
                        CompleteYouTurn();
                        return false;
                    }

                    //just need to make sure the points continue ascending or heading switches all over the place
                    if (A > B)
                    {
                        (B, A) = (A, B);
                    }

                    //minDistA = 100;
                    //int closestPt = 0;
                    //for (int i = 0; i < ptCount; i++)
                    //{
                    //    double distancePiv = glm.DistanceSquared(ytList[i], pivot);
                    //    if (distancePiv < minDistA)
                    //    {
                    //        minDistA = distancePiv;
                    //    }
                    //}

                    //feed backward to turn slower to keep pivot on
                    if (A < 0)
                    {
                        A = 0;
                    }
                    B = A + 1;

                    // Track maximum progress through the path
                    if (A > maxProgressIndexReached)
                        maxProgressIndexReached = A;

                    // Only complete if we've progressed through at least 50% of the path
                    // This prevents premature completion when start and end positions are the same
                    bool hasProgressedEnough = maxProgressIndexReached > (ptCount / 2);

                    //return and reset if too far away or end of the line
                    if (B >= ptCount - 1 && hasProgressedEnough)
                    {
                        CompleteYouTurn();
                        return false;
                    }

                    if (uTurnStyle == 1 && mf.isReverse)
                    {
                        CompleteYouTurn();
                        return true;
                    }

                    //get the distance from currently active AB line, precalc the norm of line
                    double dx = ytList[B].easting - ytList[A].easting;
                    double dz = ytList[B].northing - ytList[A].northing;
                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return false;

                    double abHeading = ytList[A].heading;

                    //how far from current AB Line is steer point 90 degrees from steer position
                    distanceFromCurrentLine = ((dz * pivot.easting) - (dx * pivot.northing) + (ytList[B].easting
                                * ytList[A].northing) - (ytList[B].northing * ytList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    //Calc point on ABLine closest to current position and 90 degrees to segment heading
                    double U = (((pivot.easting - ytList[A].easting) * dx)
                                + ((pivot.northing - ytList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    //critical point used as start for the uturn path - critical
                    rEastYT = ytList[A].easting + (U * dx);
                    rNorthYT = ytList[A].northing + (U * dz);

                    //the first part of stanley is to extract heading error
                    double abFixHeadingDelta = (pivot.heading - abHeading);

                    //Fix the circular error - get it from -Pi/2 to Pi/2
                    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;
                    if (abFixHeadingDelta > glm.PIBy2) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < -glm.PIBy2) abFixHeadingDelta += Math.PI;

                    if (mf.isReverse) abFixHeadingDelta *= -1;
                    //normally set to 1, less then unity gives less heading error.
                    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    //the non linear distance error part of stanley
                    steerAngleYT = Math.Atan((distanceFromCurrentLine * mf.vehicle.stanleyDistanceErrorGain) / ((mf.avgSpeed * 0.277777) + 1));

                    //clamp it to max 42 degrees
                    if (steerAngleYT > 0.74) steerAngleYT = 0.74;
                    if (steerAngleYT < -0.74) steerAngleYT = -0.74;

                    //add them up and clamp to max in vehicle settings
                    steerAngleYT = glm.toDegrees((steerAngleYT + abFixHeadingDelta * mf.vehicle.uturnCompensation) * -1.0);
                    if (steerAngleYT < -mf.vehicle.maxSteerAngle) steerAngleYT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleYT > mf.vehicle.maxSteerAngle) steerAngleYT = mf.vehicle.maxSteerAngle;
                }
                else
                {
                    vec3 pivot = mf.pivotAxlePos;

                    //find the closest 2 points to current fix
                    //Start search from max progress minus small lookback to prevent jumping back to start
                    //when start and end positions overlap
                    int searchStartIndex = Math.Max(0, maxProgressIndexReached - 5);
                    for (int t = searchStartIndex; t < ptCount; t++)
                    {
                        double dist = ((pivot.easting - ytList[t].easting) * (pivot.easting - ytList[t].easting))
                                        + ((pivot.northing - ytList[t].northing) * (pivot.northing - ytList[t].northing));
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

                    //just need to make sure the points continue ascending or heading switches all over the place
                    if (A > B)
                    {
                        (B, A) = (A, B);
                    }

                    onA = A;

                    // Track maximum progress through the path
                    if (A > maxProgressIndexReached)
                        maxProgressIndexReached = A;

                    double distancePiv = glm.Distance(ytList[A], pivot);

                    // Only complete if we've progressed through at least 50% of the path
                    // This prevents premature completion when start and end positions are the same
                    bool hasProgressedEnough = maxProgressIndexReached > (ptCount / 2);

                    if (((A > 0 && distancePiv > 2) || (B >= ptCount - 1)) && hasProgressedEnough)
                    {
                        CompleteYouTurn();
                        return false;
                    }

                    //get the distance from currently active AB line
                    double dx = ytList[B].easting - ytList[A].easting;
                    double dz = ytList[B].northing - ytList[A].northing;

                    if (Math.Abs(dx) < double.Epsilon && Math.Abs(dz) < double.Epsilon) return false;

                    //how far from current AB Line is fix
                    distanceFromCurrentLine = ((dz * pivot.easting) - (dx * pivot.northing) + (ytList[B].easting
                                * ytList[A].northing) - (ytList[B].northing * ytList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    // ** Pure pursuit ** - calc point on ABLine closest to current position
                    double U = (((pivot.easting - ytList[A].easting) * dx)
                                + ((pivot.northing - ytList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    rEastYT = ytList[A].easting + (U * dx);
                    rNorthYT = ytList[A].northing + (U * dz);

                    //sharp turns on you turn.
                    //update base on autosteer settings and distance from line
                    double goalPointDistance = mf.vehicle.UpdateGoalPointDistance();

                    //goalPointDistance *= mf.vehicle.uturnCompensation;

                    isHeadingSameWay = true;
                    bool ReverseHeading = !mf.isReverse;

                    int count = ReverseHeading ? 1 : -1;
                    vec3 start = new vec3(rEastYT, rNorthYT, 0);
                    double distSoFar = 0;

                    for (int i = ReverseHeading ? B : A; i < ptCount && i >= 0; i += count)
                    {
                        // used for calculating the length squared of next segment.
                        double tempDist = glm.Distance(start, ytList[i]);

                        //will we go too far?
                        if ((tempDist + distSoFar) > goalPointDistance)
                        {
                            double j = (goalPointDistance - distSoFar) / tempDist; // the remainder to yet travel

                            goalPointYT.easting = (((1 - j) * start.easting) + (j * ytList[i].easting));
                            goalPointYT.northing = (((1 - j) * start.northing) + (j * ytList[i].northing));
                            break;
                        }
                        else distSoFar += tempDist;

                        start = ytList[i];

                        if (i == ptCount - 1)//goalPointDistance is longer than remaining u-turn
                        {
                            CompleteYouTurn();
                            return false;
                        }

                        if (uTurnStyle == 1 && mf.isReverse)
                        {
                            CompleteYouTurn();
                            return true;
                        }
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = glm.DistanceSquared(goalPointYT.northing, goalPointYT.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = glm.twoPI - mf.fixHeading;
                    ppRadiusYT = goalPointDistanceSquared / (2 * (((goalPointYT.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPointYT.northing - pivot.northing) * Math.Sin(localHeading))));

                    steerAngleYT = glm.toDegrees(Math.Atan(2 * (((goalPointYT.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointYT.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.VehicleConfig.Wheelbase / goalPointDistanceSquared));

                    steerAngleYT *= mf.vehicle.uturnCompensation;

                    if (steerAngleYT < -mf.vehicle.maxSteerAngle) steerAngleYT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleYT > mf.vehicle.maxSteerAngle) steerAngleYT = mf.vehicle.maxSteerAngle;

                    if (ppRadiusYT < -500) ppRadiusYT = -500;
                    if (ppRadiusYT > 500) ppRadiusYT = 500;

                    radiusPointYT.easting = pivot.easting + (ppRadiusYT * Math.Cos(localHeading));
                    radiusPointYT.northing = pivot.northing + (ppRadiusYT * Math.Sin(localHeading));

                    //distance is negative if on left, positive if on right
                    if (!isHeadingSameWay)
                        distanceFromCurrentLine *= -1.0;
                }

                //used for smooth mode
                mf.vehicle.modeActualXTE = (distanceFromCurrentLine);

                //Convert to centimeters
                mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);
                mf.guidanceLineSteerAngle = (short)(steerAngleYT * 100);
                return true;
            }
            else
            {
                CompleteYouTurn();
                return false;
            }
        }

        //Duh.... What does this do....
        public void DrawYouTurn()
        {
            if (ytList.Count < 3) return;

            GL.PointSize(mf.ABLine.lineWidth + 2);

            if (isYouTurnTriggered)
                GL.Color3(0.95f, 0.5f, 0.95f);
            else if (isOutOfBounds)
                GL.Color3(0.9495f, 0.395f, 0.325f);
            else
                GL.Color3(0.395f, 0.925f, 0.30f);

            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < ytList.Count; i++)
            {
                GL.Vertex3(ytList[i].easting, ytList[i].northing, 0);
            }
            GL.End();

            //GL.PointSize(12.0f);
            //GL.Begin(PrimitiveType.Points);
            //GL.Color3(0.95f, 0.73f, 1.0f);
            //GL.Vertex3(inClosestTurnPt.closePt.easting, inClosestTurnPt.closePt.northing, 0);
            //GL.Color3(0.395f, 0.925f, 0.30f);
            //GL.Vertex3(outClosestTurnPt.closePt.easting, outClosestTurnPt.closePt.northing, 0);
            //GL.End();
            //GL.PointSize(1.0f);

            //if (nextCurve != null)
            //{
            //    GL.Begin(PrimitiveType.Points);
            //    GL.Color3(0.95f, 0.41f, 0.980f);
            //    for (int i = 0; i < nextCurve.curList.Count; i++)
            //    {
            //        GL.Vertex3(nextCurve.curList[i].easting, nextCurve.curList[i].northing, 0);
            //    }
            //    GL.End();
            //}

            //if (ytList2?.Count > 0)
            //{
            //    GL.PointSize(mf.ABLine.lineWidth + 2);
            //    GL.Color3(0.3f, 0.941f, 0.980f);
            //    GL.Begin(PrimitiveType.Points);
            //    for (int i = 0; i < ytList2.Count; i++)
            //    {
            //        GL.Vertex3(ytList2[i].easting, ytList2[i].northing, 0);
            //    }
            //    GL.End();
            //}
        }

        public class CClose
        {
            public vec3 closePt = new vec3();
            public int turnLineNum;
            public int turnLineIndex;
            public double turnLineHeading;
            public int curveIndex;

            public CClose()
            {
                closePt = new vec3();
                turnLineNum = -1;
                turnLineIndex = -1;
                turnLineHeading = -1;
                curveIndex = -1;
            }

            public CClose(CClose _clo)
            {
                closePt = new vec3(_clo.closePt);
                turnLineNum = _clo.turnLineNum;
                turnLineIndex = _clo.turnLineIndex;
                turnLineHeading = _clo.turnLineHeading;
                curveIndex = _clo.curveIndex;
            }
        }
    }
}
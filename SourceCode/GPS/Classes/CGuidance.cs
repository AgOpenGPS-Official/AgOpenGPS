using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CGuidance
    {
        private readonly FormGPS mf;

        //steer, pivot, and ref indexes
        private int sA, sB, C, pA, pB;

        //private int rA, rB;

        public double distanceFromCurrentLineSteer, distanceFromCurrentLinePivot;
        public double steerAngleGu, rEastSteer, rNorthSteer, rEastPivot, rNorthPivot;

        public double inty, xTrackSteerCorrection = 0;
        public double steerHeadingError;

        public double distSteerError, lastDistSteerError, derivativeDistError;

        public double pivotDistanceError;

        //public int modeTimeCounter = 0;

        //for adding steering angle based on side slope hill
        public double sideHillCompFactor;

        //derivative counter
        private int counter;

        public CGuidance(FormGPS _f)
        {
            //constructor
            mf = _f;
            sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;
        }

        #region Stanley

        private void DoSteerAngleCalc()
        {
            if (mf.isReverse) steerHeadingError *= -1;
            //Overshoot setting on Stanley tab
            steerHeadingError *= mf.vehicle.stanleyHeadingErrorGain;

            double sped = Math.Abs(mf.avgSpeed);
            if (sped > 1) sped = 1 + 0.277 * (sped - 1);
            else sped = 1;
            double XTEc = Math.Atan((distanceFromCurrentLineSteer * mf.vehicle.stanleyDistanceErrorGain)
                / (sped));

            xTrackSteerCorrection = (xTrackSteerCorrection * 0.5) + XTEc * (0.5);

            //derivative of steer distance error
            distSteerError = (distSteerError * 0.95) + ((xTrackSteerCorrection * 60) * 0.05);
            if (counter++ > 5)
            {
                derivativeDistError = distSteerError - lastDistSteerError;
                lastDistSteerError = distSteerError;
                counter = 0;
            }

            steerAngleGu = glm.toDegrees((xTrackSteerCorrection + steerHeadingError) * -1.0);

            if (Math.Abs(distanceFromCurrentLineSteer) > 0.5) steerAngleGu *= 0.5;
            else steerAngleGu *= (1 - Math.Abs(distanceFromCurrentLineSteer));

            //pivot PID
            pivotDistanceError = (pivotDistanceError * 0.6) + (distanceFromCurrentLinePivot * 0.4);
            //pivotDistanceError = Math.Atan((distanceFromCurrentLinePivot) / (sped)) * 0.2;
            //pivotErrorTotal = pivotDistanceError + pivotDerivative;

            if (mf.avgSpeed > 1
                && mf.isBtnAutoSteerOn
                && Math.Abs(derivativeDistError) < 1
                && Math.Abs(pivotDistanceError) < 0.25)
            {
                //if over the line heading wrong way, rapidly decrease integral
                if ((inty < 0 && distanceFromCurrentLinePivot < 0) || (inty > 0 && distanceFromCurrentLinePivot > 0))
                {
                    inty += pivotDistanceError * mf.vehicle.stanleyIntegralGainAB * -0.03;
                }
                else
                {
                    inty += pivotDistanceError * mf.vehicle.stanleyIntegralGainAB * -0.01;
                }

                //integral slider is set to 0
                if (mf.vehicle.stanleyIntegralGainAB == 0) inty = 0;
            }
            else inty *= 0.7;

            if (mf.isReverse) inty = 0;

            if (mf.ahrs.imuRoll != 88888)
                steerAngleGu += mf.ahrs.imuRoll * -sideHillCompFactor;

            if (steerAngleGu < -mf.vehicle.maxSteerAngle) steerAngleGu = -mf.vehicle.maxSteerAngle;
            else if (steerAngleGu > mf.vehicle.maxSteerAngle) steerAngleGu = mf.vehicle.maxSteerAngle;

            //used for smooth mode
            mf.vehicle.modeActualXTE = (distanceFromCurrentLinePivot);

            //Convert to millimeters from meters
            mf.guidanceLineDistanceOff = (short)Math.Round(distanceFromCurrentLinePivot * 1000.0, MidpointRounding.AwayFromZero);
            mf.guidanceLineSteerAngle = (short)(steerAngleGu * 100);
        }

        /// <summary>
        /// Function to calculate steer angle for AB Line Segment only
        /// No curvature calc on straight line
        /// </summary>
        /// <param name="curPtA"></param>
        /// <param name="curPtB"></param>
        /// <param name="pivot"></param>
        /// <param name="steer"></param>
        /// <param name="isValid"></param>
        public void StanleyGuidanceABLine(vec3 curPtA, vec3 curPtB, vec3 pivot, vec3 steer)
        {
            //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
            double dx = curPtB.easting - curPtA.easting;
            double dy = curPtB.northing - curPtA.northing;
            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLinePivot = ((dy * pivot.easting) - (dx * pivot.northing) + (curPtB.easting
                        * curPtA.northing) - (curPtB.northing * curPtA.easting))
                            / Math.Sqrt((dy * dy) + (dx * dx));

            if (!mf.ABLine.isHeadingSameWay)
                distanceFromCurrentLinePivot *= -1.0;

            mf.ABLine.distanceFromCurrentLinePivot = distanceFromCurrentLinePivot;
            double U = (((pivot.easting - curPtA.easting) * dx)
                            + ((pivot.northing - curPtA.northing) * dy))
                            / ((dx * dx) + (dy * dy));

            rEastPivot = curPtA.easting + (U * dx);
            rNorthPivot = curPtA.northing + (U * dy);

            mf.ABLine.rEastAB = rEastPivot;
            mf.ABLine.rNorthAB = rNorthPivot;

            //get the distance from currently active AB segment of steer axle //////// steer /////////////
            vec3 steerA = new vec3(curPtA);
            vec3 steerB = new vec3(curPtB);

            //create the AB segment to offset
            steerA.easting += (Math.Sin(steerA.heading + glm.PIBy2) * (inty));
            steerA.northing += (Math.Cos(steerA.heading + glm.PIBy2) * (inty));

            steerB.easting += (Math.Sin(steerB.heading + glm.PIBy2) * (inty));
            steerB.northing += (Math.Cos(steerB.heading + glm.PIBy2) * (inty));

            dx = steerB.easting - steerA.easting;
            dy = steerB.northing - steerA.northing;

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon) return;

            //how far from current AB Line is fix
            distanceFromCurrentLineSteer = ((dy * steer.easting) - (dx * steer.northing) + (steerB.easting
                        * steerA.northing) - (steerB.northing * steerA.easting))
                            / Math.Sqrt((dy * dy) + (dx * dx));

            if (!mf.ABLine.isHeadingSameWay)
                distanceFromCurrentLineSteer *= -1.0;

            // calc point on ABLine closest to current position - for display only
            U = (((steer.easting - steerA.easting) * dx)
                            + ((steer.northing - steerA.northing) * dy))
                            / ((dx * dx) + (dy * dy));

            rEastSteer = steerA.easting + (U * dx);
            rNorthSteer = steerA.northing + (U * dy);

            double steerErr = Math.Atan2(rEastSteer - rEastPivot, rNorthSteer - rNorthPivot);
            steerHeadingError = (steer.heading - steerErr);
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

            DoSteerAngleCalc();
        }

        /// <summary>
        /// Find the steer angle for a curve list, curvature and integral
        /// </summary>
        /// <param name="pivot">Pivot position vector</param>
        /// <param name="steer">Steer position vector</param>
        /// <param name="curList">the current list of guidance points</param>
        public void StanleyGuidanceCurve(vec3 pivot, vec3 steer, ref List<vec3> curList)
        {            //calculate required steer angle
            //find the closest point roughly
            int cc = 0, dd;
            int ptCount = curList.Count;
            if (ptCount > 5)
            {
                double minDistA = 1000000, minDistB;

                for (int j = 0; j < ptCount; j += 10)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        cc = j;
                    }
                }

                minDistA = minDistB = 1000000;
                dd = cc + 7; if (dd > ptCount - 1) dd = ptCount;
                cc -= 7; if (cc < 0) cc = 0;

                //find the closest 2 points to current close call
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        sB = sA;
                        minDistA = dist;
                        sA = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        sB = j;
                    }
                }

                ////too far from guidance line? Lost? Fresh delete of ref?
                //if (minDistA < (1.5 * (mf.tool.toolWidth * mf.tool.toolWidth)))
                //{
                //    if (minDistA == 100000000)
                //        return;
                //}
                //else
                //{
                //    curList.Clear();
                //    return;
                //}

                //just need to make sure the points continue ascending or heading switches all over the place
                if (sA > sB) { C = sA; sA = sB; sB = C; }

                //currentLocationIndex = sA;
                if (sA > ptCount - 1 || sB > ptCount - 1) return;

                minDistA = minDistB = 1000000;

                if (mf.curve.isHeadingSameWay)
                {
                    dd = sB; cc = dd - 12; if (cc < 0) cc = 0;
                }
                else
                {
                    cc = sA; dd = sA + 12; if (dd >= ptCount) dd = ptCount - 1;
                }

                //find the closest 2 points of pivot back from steer
                for (int j = cc; j < dd; j++)
                {
                    double dist = ((steer.easting - curList[j].easting) * (steer.easting - curList[j].easting))
                                    + ((steer.northing - curList[j].northing) * (steer.northing - curList[j].northing));
                    if (dist < minDistA)
                    {
                        minDistB = minDistA;
                        pB = pA;
                        minDistA = dist;
                        pA = j;
                    }
                    else if (dist < minDistB)
                    {
                        minDistB = dist;
                        pB = j;
                    }
                }

                //just need to make sure the points continue ascending or heading switches all over the place
                if (pA > pB) { C = pA; pA = pB; pB = C; }

                if (pA > ptCount - 1 || pB > ptCount - 1)
                {
                    pA = ptCount - 2;
                    pB = ptCount - 1;
                }

                vec3 pivA = new vec3(curList[pA]);
                vec3 pivB = new vec3(curList[pB]);

                if (!mf.curve.isHeadingSameWay)
                {
                    pivA = curList[pB];
                    pivB = curList[pA];

                    pivA.heading += Math.PI;
                    if (pivA.heading > glm.twoPI) pivA.heading -= glm.twoPI;
                }

                mf.curve.manualUturnHeading = pivA.heading;

                //get the pivot distance from currently active AB segment   ///////////  Pivot  ////////////
                double dx = pivB.easting - pivA.easting;
                double dz = pivB.northing - pivA.northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLinePivot = ((dz * steer.easting) - (dx * steer.northing) + (pivB.easting
                            * pivA.northing) - (pivB.northing * pivA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                mf.curve.distanceFromCurrentLinePivot = distanceFromCurrentLinePivot;
                double U = (((steer.easting - pivA.easting) * dx)
                                + ((steer.northing - pivA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEastPivot = pivA.easting + (U * dx);
                rNorthPivot = pivA.northing + (U * dz);

                mf.curve.rEastCu = rEastPivot;
                mf.curve.rNorthCu = rNorthPivot;

                mf.curve.currentLocationIndex = pA;

                //get the distance from currently active AB segment of steer axle //////// steer /////////////
                vec3 steerA = new vec3(curList[sA]);
                vec3 steerB = new vec3(curList[sB]);

                if (!mf.curve.isHeadingSameWay)
                {
                    steerA = curList[sB];
                    steerA.heading += Math.PI;
                    if (steerA.heading > glm.twoPI) steerA.heading -= glm.twoPI;

                    steerB = curList[sA];
                    steerB.heading += Math.PI;
                    if (steerB.heading > glm.twoPI) steerB.heading -= glm.twoPI;
                }

                //double curvature = pivA.heading - steerA.heading;
                //if (curvature > Math.PI) curvature -= Math.PI; else if (curvature < Math.PI) curvature += Math.PI;
                //if (curvature > glm.PIBy2) curvature -= Math.PI; else if (curvature < -glm.PIBy2) curvature += Math.PI;

                ////because of draft
                //curvature = Math.Sin(curvature) * mf.vehicle.wheelbase * 0.8;
                //pivotCurvatureOffset = (pivotCurvatureOffset * 0.7) + (curvature * 0.3);
                //pivotCurvatureOffset = 0;

                //create the AB segment to offset
                steerA.easting += (Math.Sin(steerA.heading + glm.PIBy2) * (inty));
                steerA.northing += (Math.Cos(steerA.heading + glm.PIBy2) * (inty));

                steerB.easting += (Math.Sin(steerB.heading + glm.PIBy2) * (inty));
                steerB.northing += (Math.Cos(steerB.heading + glm.PIBy2) * (inty));

                dx = steerB.easting - steerA.easting;
                dz = steerB.northing - steerA.northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLineSteer = ((dz * steer.easting) - (dx * steer.northing) + (steerB.easting
                            * steerA.northing) - (steerB.northing * steerA.easting))
                                / Math.Sqrt((dz * dz) + (dx * dx));

                // calc point on ABLine closest to current position - for display only
                U = (((steer.easting - steerA.easting) * dx)
                                + ((steer.northing - steerA.northing) * dz))
                                / ((dx * dx) + (dz * dz));

                rEastSteer = steerA.easting + (U * dx);
                rNorthSteer = steerA.northing + (U * dz);

                //double segHeading = Math.Atan2(rEastSteer - rEastPivot, rNorthSteer - rNorthPivot);

                //steerHeadingError = Math.PI - Math.Abs(Math.Abs(pivot.heading - segHeading) - Math.PI);
                steerHeadingError = steer.heading - steerB.heading;

                //Fix the circular error
                if (steerHeadingError > Math.PI) steerHeadingError -= Math.PI;
                else if (steerHeadingError < Math.PI) steerHeadingError += Math.PI;

                if (steerHeadingError > glm.PIBy2) steerHeadingError -= Math.PI;
                else if (steerHeadingError < -glm.PIBy2) steerHeadingError += Math.PI;

                DoSteerAngleCalc();
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLineSteer = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        #endregion Stanley
    }
}
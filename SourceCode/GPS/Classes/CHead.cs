﻿using System;

namespace AgOpenGPS
{
    public partial class CBoundary
    {
        public bool isOn;
        public double leftToolDistance;
        public double rightToolDistance;

        //generated box for finding closest point
        public vec2 downL = new vec2(9000, 9000), downR = new vec2(9000, 9002);

        public bool isToolInHeadland, isToolUp, isToolOuterPointsInHeadland;

        public bool isToolLeftIn = false;
        public bool isToolRightIn = false;
        public bool isLookRightIn = false;
        public bool isLookLeftIn = false;

        public void SetHydPosition()
        {
            if (mf.vehicle.isHydLiftOn && mf.pn.speed > 0.2 && mf.autoBtnState == FormGPS.btnStates.Auto)
            {
                if (isToolInHeadland)
                {
                    mf.p_239.pgn[mf.p_239.hydLift] = 2;
                    isToolUp = true;
                }
                else
                {
                    mf.p_239.pgn[mf.p_239.hydLift] = 1;
                    isToolUp = false;
                }
            }
        }

        public void WhereAreToolCorners()
        {
            if (bndArr.Count > 0 && bndArr[0].hdLine.Count > 0)
            {
                bool isLeftInWk, isRightInWk = true;

                if (isOn)
                {
                    for (int j = 0; j < mf.tool.numOfSections; j++)
                    {
                        if (j == 0)
                        {
                            //only one first left point, the rest are all rights moved over to left
                            isLeftInWk = bndArr[0].IsPointInHeadArea(mf.section[j].leftPoint);
                            isRightInWk = bndArr[0].IsPointInHeadArea(mf.section[j].rightPoint);

                            //save left side
                            mf.tool.isLeftSideInHeadland = !isLeftInWk;

                            //merge the two sides into in or out
                            mf.section[j].isInHeadlandArea = !isLeftInWk && !isRightInWk;

                        }
                        else
                        {
                            //grab the right of previous section, its the left of this section
                            isLeftInWk = isRightInWk;
                            isRightInWk = bndArr[0].IsPointInHeadArea(mf.section[j].rightPoint);

                            mf.section[j].isInHeadlandArea = !isLeftInWk && !isRightInWk;
                        }
                    }

                    //save right side
                    mf.tool.isRightSideInHeadland = !isRightInWk;

                    //is the tool in or out based on endpoints
                    isToolOuterPointsInHeadland = mf.tool.isLeftSideInHeadland && mf.tool.isRightSideInHeadland;
                }
                else
                {
                    //set all to true;
                    isToolOuterPointsInHeadland = true;
                }
            }
        }

        public void WhereAreToolLookOnPoints()
        {
            if (bndArr.Count > 0 && bndArr[0].hdLine.Count > 0)
            {
                vec3 toolFix = mf.toolPos;
                double sinAB = Math.Sin(toolFix.heading);
                double cosAB = Math.Cos(toolFix.heading);

                double pos = mf.section[0].rpSectionWidth;
                double mOn = (mf.tool.lookAheadDistanceOnPixelsRight - mf.tool.lookAheadDistanceOnPixelsLeft) / mf.tool.rpWidth;
                double endHeight = (mf.tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * 0.1;

                for (int j = 0; j < mf.tool.numOfSections; j++)
                {
                    if (j == 0)
                    {
                        downL.easting = mf.section[j].leftPoint.easting + (sinAB * mf.tool.lookAheadDistanceOnPixelsLeft * 0.1);
                        downL.northing = mf.section[j].leftPoint.northing + (cosAB * mf.tool.lookAheadDistanceOnPixelsLeft * 0.1);

                        downR.easting = mf.section[j].rightPoint.easting + (sinAB * endHeight);
                        downR.northing = mf.section[j].rightPoint.northing + (cosAB * endHeight);

                        isLookLeftIn = IsPointInsideHeadLine(downL);
                        isLookRightIn = IsPointInsideHeadLine(downR);

                        mf.section[j].isLookOnInHeadland = !isLookLeftIn && !isLookRightIn;
                        isLookLeftIn = isLookRightIn;
                    }
                    else
                    {
                        pos += mf.section[j].rpSectionWidth;
                        endHeight = (mf.tool.lookAheadDistanceOnPixelsLeft + (mOn * pos)) * 0.1;

                        downR.easting = mf.section[j].rightPoint.easting + (sinAB * endHeight);
                        downR.northing = mf.section[j].rightPoint.northing + (cosAB * endHeight);

                        isLookRightIn = IsPointInsideHeadLine(downR);
                        mf.section[j].isLookOnInHeadland = !isLookLeftIn && !isLookRightIn;
                        isLookLeftIn = isLookRightIn;
                    }
                }
            }
        }

        public void DrawHeadLines()
        {
            for (int i = 0; i < bndArr.Count; i++)
            {
                if (bndArr[i].hdLine.Count > 0) bndArr[i].DrawHeadLine();
            }

            //GL.LineWidth(4.0f);
            //GL.Color3(0.9219f, 0.2f, 0.970f);
            //GL.Begin(PrimitiveType.Lines);
            //{
            //    GL.Vertex3(headArr[0].hdLine[A].easting, headArr[0].hdLine[A].northing, 0);
            //    GL.Vertex3(headArr[0].hdLine[B].easting, headArr[0].hdLine[B].northing, 0);
            //    GL.Vertex3(headArr[0].hdLine[C].easting, headArr[0].hdLine[C].northing, 0);
            //    GL.Vertex3(headArr[0].hdLine[D].easting, headArr[0].hdLine[D].northing, 0);
            //}
            //GL.End();

            //GL.PointSize(6.0f);
            //GL.Color3(0.219f, 0.932f, 0.970f);
            //GL.Begin(PrimitiveType.Points);
            //GL.Vertex3(downL.easting, downL.northing, 0);
            //GL.Vertex3(downR.easting, downR.northing, 0);
            //GL.End();
        }

        public bool IsPointInsideHeadLine(vec2 pt)
        {
            //if inside outer boundary, then potentially add
            if (bndArr.Count > 0 && bndArr[0].IsPointInHeadArea(pt))
            {
                for (int b = 1; b < bndArr.Count; b++)
                {
                    if (bndArr[b].IsPointInHeadArea(pt))
                    {
                        //point is in an inner turn area but inside outer
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
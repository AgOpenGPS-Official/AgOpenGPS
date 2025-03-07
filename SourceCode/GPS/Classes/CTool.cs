using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AgOpenGPS
{
    public class CTool
    {
        private readonly FormGPS mf;

        public double width, halfWidth, contourWidth;
        public double farLeftPosition = 0;
        public double farLeftSpeed = 0;
        public double farRightPosition = 0;
        public double farRightSpeed = 0;

        public double overlap;
        public double trailingHitchLength, tankTrailingHitchLength, trailingToolToPivotLength;
        public double offset;

        public double lookAheadOffSetting, lookAheadOnSetting;
        public double turnOffDelay;

        public double lookAheadDistanceOnPixelsLeft, lookAheadDistanceOnPixelsRight;
        public double lookAheadDistanceOffPixelsLeft, lookAheadDistanceOffPixelsRight;

        public bool isToolTrailing, isToolTBT;
        public bool isToolRearFixed, isToolFrontFixed;

        public bool isMultiColoredSections, isSectionOffWhenOut;
        public string toolAttachType;

        public double hitchLength;

        //how many individual sections
        public int numOfSections;

        //used for super section off on
        public int minCoverage;

        public bool areAllSectionBtnsOn = true;

        public bool isLeftSideInHeadland = true, isRightSideInHeadland = true, isSectionsNotZones;

        //read pixel values
        public int rpXPosition;

        public int rpWidth;

        private double textRotate;

        public Color[] secColors = new Color[16];

        public int zones;
        public int[] zoneRanges = new int[9];

        public bool isDisplayTramControl;

        //Constructor called by FormGPS
        public CTool(FormGPS _f)
        {
            mf = _f;

            //from settings grab the vehicle specifics

            trailingToolToPivotLength = Properties.Settings.Default.setTool_trailingToolToPivotLength;
            width = Properties.Settings.Default.setVehicle_toolWidth;
            overlap = Properties.Settings.Default.setVehicle_toolOverlap;

            offset = Properties.Settings.Default.setVehicle_toolOffset;

            trailingHitchLength = Properties.Settings.Default.setTool_toolTrailingHitchLength;
            tankTrailingHitchLength = Properties.Settings.Default.setVehicle_tankTrailingHitchLength;
            hitchLength = Properties.Settings.Default.setVehicle_hitchLength;

            isToolRearFixed = Properties.Settings.Default.setTool_isToolRearFixed;
            isToolTrailing = Properties.Settings.Default.setTool_isToolTrailing;
            isToolTBT = Properties.Settings.Default.setTool_isToolTBT;
            isToolFrontFixed = Properties.Settings.Default.setTool_isToolFront;

            lookAheadOnSetting = Properties.Settings.Default.setVehicle_toolLookAheadOn;
            lookAheadOffSetting = Properties.Settings.Default.setVehicle_toolLookAheadOff;
            turnOffDelay = Properties.Settings.Default.setVehicle_toolOffDelay;

            isSectionOffWhenOut = Properties.Settings.Default.setTool_isSectionOffWhenOut;

            isSectionsNotZones = Properties.Settings.Default.setTool_isSectionsNotZones;

            if (isSectionsNotZones)
                numOfSections = Properties.Settings.Default.setVehicle_numSections;
            else
                numOfSections = Properties.Settings.Default.setTool_numSectionsMulti;

            minCoverage = Properties.Settings.Default.setVehicle_minCoverage;
            isMultiColoredSections = Properties.Settings.Default.setColor_isMultiColorSections;

            secColors[0] = Properties.Settings.Default.setColor_sec01.CheckColorFor255();
            secColors[1] = Properties.Settings.Default.setColor_sec02.CheckColorFor255();
            secColors[2] = Properties.Settings.Default.setColor_sec03.CheckColorFor255();
            secColors[3] = Properties.Settings.Default.setColor_sec04.CheckColorFor255();
            secColors[4] = Properties.Settings.Default.setColor_sec05.CheckColorFor255();
            secColors[5] = Properties.Settings.Default.setColor_sec06.CheckColorFor255();
            secColors[6] = Properties.Settings.Default.setColor_sec07.CheckColorFor255();
            secColors[7] = Properties.Settings.Default.setColor_sec08.CheckColorFor255();
            secColors[8] = Properties.Settings.Default.setColor_sec09.CheckColorFor255();
            secColors[9] = Properties.Settings.Default.setColor_sec10.CheckColorFor255();
            secColors[10] = Properties.Settings.Default.setColor_sec11.CheckColorFor255();
            secColors[11] = Properties.Settings.Default.setColor_sec12.CheckColorFor255();
            secColors[12] = Properties.Settings.Default.setColor_sec13.CheckColorFor255();
            secColors[13] = Properties.Settings.Default.setColor_sec14.CheckColorFor255();
            secColors[14] = Properties.Settings.Default.setColor_sec15.CheckColorFor255();
            secColors[15] = Properties.Settings.Default.setColor_sec16.CheckColorFor255();

            string[] words = Properties.Settings.Default.setTool_zones.Split(',');
            zones = int.Parse(words[0]);

            for (int i = 0; i < words.Length; i++)
            {
                zoneRanges[i] = int.Parse(words[i]);
            }

            isDisplayTramControl = Properties.Settings.Default.setTool_isDisplayTramControl;
        }

        private void DrawHitchLayer(bool isBackgroundLayer, double trailingTank)
        {
            GL.LineWidth(isBackgroundLayer ? 6 : 1);
            (isBackgroundLayer ? Colors.Black : Colors.HitchColor).SetColor();

            //draw the rigid hitch
            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(-0.57, trailingTank, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0.57, trailingTank, 0);
            GL.End();
        }

        private void DrawTrailingHitchLayer(bool isBackgroundLayer, double trailingTool)
        {
            GL.LineWidth(isBackgroundLayer ? 6 : 1);
            (isBackgroundLayer ? Colors.Black : Colors.TrailingToolColor).SetColor();

            GL.Begin(PrimitiveType.LineLoop);
            GL.Vertex3(-0.65 + mf.tool.offset, trailingTool, 0);
            GL.Vertex3(0, 0, 0);
            GL.Vertex3(0.65 + mf.tool.offset, trailingTool, 0);
            GL.End();
        }

        public void DrawTool()
        {
            //translate and rotate at pivot axle
            GL.Translate(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
            GL.PushMatrix();

            //translate down to the hitch pin
            GL.Translate(Math.Sin(mf.fixHeading) * (hitchLength),
                            Math.Cos(mf.fixHeading) * (hitchLength), 0);

            //settings doesn't change trailing hitch length if set to rigid, so do it here
            double trailingTank, trailingTool;
            if (isToolTrailing)
            {
                trailingTank = tankTrailingHitchLength;
                trailingTool = trailingHitchLength;
            }
            else { trailingTank = 0; trailingTool = 0; }

            // if there is a trailing tow between hitch
            if (isToolTBT && isToolTrailing)
            {
                //rotate to tank heading
                GL.Rotate(glm.toDegrees(-mf.tankPos.heading), 0.0, 0.0, 1.0);

                DrawHitchLayer(true, trailingTank);
                DrawHitchLayer(false, trailingTank);

                GL.Color4(1, 1, 1, 0.75);
                XyCoord toolAxleCenter = new XyCoord(0.0, trailingTank);
                XyDelta deltaToU1V1 = new XyDelta(1.5, 1.0);
                mf.VehicleTextures.ToolAxle.DrawCentered(toolAxleCenter, deltaToU1V1);

                //move down the tank hitch, unwind, rotate to section heading
                GL.Translate(0.0, trailingTank, 0.0);
                GL.Rotate(glm.toDegrees(mf.tankPos.heading), 0.0, 0.0, 1.0);
            }
            GL.Rotate(glm.toDegrees(-mf.toolPivotPos.heading), 0.0, 0.0, 1.0);

            //draw the hitch if trailing
            if (isToolTrailing)
            {
                DrawTrailingHitchLayer(true, trailingTool);
                DrawTrailingHitchLayer(false, trailingTool);

                if (Math.Abs(trailingToolToPivotLength) > 1 && mf.camera.camSetDistance > -100)
                {
                    textRotate += (mf.sim.stepDistance);
                    GL.Color4(1, 1, 1, 0.75);
                    XyCoord rightTire00 = new XyCoord(0.75 + offset, trailingTool + 0.51);
                    XyCoord rightTire11 = new XyCoord(1.4 + offset, trailingTool - 0.51);
                    XyCoord leftTire00 = new XyCoord(-0.75 + offset, trailingTool + 0.51);
                    XyCoord lefttTire11 = new XyCoord(-1.4 + offset, trailingTool - 0.51);
                    mf.VehicleTextures.Tire.Draw(rightTire00, rightTire11);
                    mf.VehicleTextures.Tire.Draw(leftTire00, lefttTire11);
                }
                trailingTool -= trailingToolToPivotLength;
            }

            if (mf.isJobStarted)
            {
                //look ahead lines
                GL.LineWidth(3);
                GL.Begin(PrimitiveType.Lines);

                //lookahead section on
                GL.Color3(0.20f, 0.7f, 0.2f);
                GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOnPixelsLeft) * 0.1 + trailingTool, 0);
                GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOnPixelsRight) * 0.1 + trailingTool, 0);

                //lookahead section off
                GL.Color3(0.70f, 0.2f, 0.2f);
                GL.Vertex3(mf.tool.farLeftPosition, (mf.tool.lookAheadDistanceOffPixelsLeft) * 0.1 + trailingTool, 0);
                GL.Vertex3(mf.tool.farRightPosition, (mf.tool.lookAheadDistanceOffPixelsRight) * 0.1 + trailingTool, 0);

                if (mf.vehicle.isHydLiftOn)
                {
                    GL.Color3(0.70f, 0.2f, 0.72f);
                    GL.Vertex3(mf.section[0].positionLeft, (mf.vehicle.hydLiftLookAheadDistanceLeft * 0.1) + trailingTool, 0);
                    GL.Vertex3(mf.section[mf.tool.numOfSections - 1].positionRight, (mf.vehicle.hydLiftLookAheadDistanceRight * 0.1) + trailingTool, 0);
                }

                GL.End();
            }

            //draw the sections
            GL.LineWidth(2);

            double hite = mf.camera.camSetDistance / -250;
            if (hite > 4) hite = 4;
            if (hite < 1) hite = 1;

            //TooDoo
            //hite = 0.2;

            for (int j = 0; j < numOfSections; j++)
            {
                //if section is on, green, if off, red color
                if (mf.section[j].isSectionOn)
                {
                    if (mf.section[j].sectionBtnState == btnStates.Auto)
                    {
                        //GL.Color3(0.0f, 0.9f, 0.0f);
                        if (mf.section[j].isMappingOn) GL.Color3(0.0f, 0.95f, 0.0f);
                        else GL.Color3(0.970f, 0.30f, 0.970f);
                    }
                    else GL.Color3(0.97, 0.97, 0);
                }
                else
                {
                    if (!mf.section[j].isMappingOn) GL.Color3(0.950f, 0.2f, 0.2f);
                    else GL.Color3(0.00f, 0.250f, 0.97f);
                    //GL.Color3(0.7f, 0.2f, 0.2f);
                }

                double mid = (mf.section[j].positionRight - mf.section[j].positionLeft) / 2 + mf.section[j].positionLeft;

                GL.Begin(PrimitiveType.TriangleFan);
                {
                    GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                    GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                    GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                    GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                    GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                }
                GL.End();

                if (mf.camera.camSetDistance > -width * 200)
                {
                    GL.Begin(PrimitiveType.LineLoop);
                    {
                        GL.Color3(0.0, 0.0, 0.0);
                        GL.Vertex3(mf.section[j].positionLeft, trailingTool, 0);
                        GL.Vertex3(mf.section[j].positionLeft, trailingTool - hite, 0);

                        GL.Vertex3(mid, trailingTool - hite * 1.5, 0);

                        GL.Vertex3(mf.section[j].positionRight, trailingTool - hite, 0);
                        GL.Vertex3(mf.section[j].positionRight, trailingTool, 0);
                    }
                    GL.End();
                }
            }

            //zones

            if (!isSectionsNotZones && zones > 0 && mf.camera.camSetDistance > -150)
            {
                //GL.PointSize(8);

                GL.Begin(PrimitiveType.Lines);
                for (int i = 1; i < zones; i++)
                {
                    GL.Color3(0.5f, 0.80f, 0.950f);
                    GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, trailingTool - 0.4, 0);
                    GL.Vertex3(mf.section[zoneRanges[i]].positionLeft, trailingTool + 0.2, 0);
                }

                GL.End();
            }

            //tram Dots
            if (isDisplayTramControl && mf.tram.displayMode != 0)
            {
                if (mf.camera.camSetDistance > -300)
                {
                    if (mf.camera.camSetDistance > -100)
                        GL.PointSize(12);
                    else GL.PointSize(8);

                    ColorRgb rightMarkerColor = ((mf.tram.controlByte) & 1) != 0 ? Colors.TramMarkerOnColor : Colors.Black;
                    ColorRgb leftMarkerColor = ((mf.tram.controlByte) & 2) != 0 ? Colors.TramMarkerOnColor : Colors.Black;
                    double rightX = mf.tram.isOuter ? farRightPosition - mf.tram.halfWheelTrack : mf.tram.halfWheelTrack;
                    double leftX = mf.tram.isOuter ? farLeftPosition + mf.tram.halfWheelTrack : -mf.tram.halfWheelTrack;
                    // section markers
                    GL.Begin(PrimitiveType.Points);
                    // right side
                    rightMarkerColor.SetColor();
                    GL.Vertex3(rightX, trailingTool, 0);
                    // left size
                    leftMarkerColor.SetColor();
                    GL.Vertex3(leftX, trailingTool, 0);
                    GL.End();
                }
            }

            GL.PopMatrix();
        }
    }
}
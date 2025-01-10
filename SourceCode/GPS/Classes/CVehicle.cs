//Please, if you use this, share the improvements

using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS
{
    public class CVehicle
    {
        private readonly FormGPS mf;

        public bool isSteerAxleAhead;
        public bool isPivotBehindAntenna;

        public double antennaHeight;
        public double antennaPivot;
        public double wheelbase;
        public double antennaOffset;
        public int deadZoneHeading, deadZoneDelay;
        public int vehicleType, deadZoneDelayCounter;
        public bool isInDeadZone;

        //min vehicle speed allowed before turning shit off
        public double slowSpeedCutoff = 0;

        //autosteer values
        public double goalPointLookAheadHold, goalPointLookAheadMult, goalPointAcquireFactor, uturnCompensation;

        public double stanleyDistanceErrorGain, stanleyHeadingErrorGain;
        public double minLookAheadDistance = 2.0;
        public double maxSteerAngle, maxSteerSpeed, minSteerSpeed;
        public double maxAngularVelocity;
        public double hydLiftLookAheadTime, trackWidth;

        public double hydLiftLookAheadDistanceLeft, hydLiftLookAheadDistanceRight;

        public bool isHydLiftOn;
        public double stanleyIntegralDistanceAwayTriggerAB, stanleyIntegralGainAB, purePursuitIntegralGain;

        //flag for free drive window to control autosteer
        public bool isInFreeDriveMode;

        //the trackbar angle for free drive
        public double driveFreeSteerAngle = 0;

        public double modeXTE, modeActualXTE = 0, modeActualHeadingError = 0;
        public int modeTime = 0;

        public double functionSpeedLimit;

        public CVehicle(FormGPS _f)
        {
            //constructor
            mf = _f;

            isPivotBehindAntenna = Properties.Settings.Default.setVehicle_isPivotBehindAntenna;
            antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
            antennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
            antennaOffset = Properties.Settings.Default.setVehicle_antennaOffset;

            wheelbase = Properties.Settings.Default.setVehicle_wheelbase;
            isSteerAxleAhead = Properties.Settings.Default.setVehicle_isSteerAxleAhead;

            slowSpeedCutoff = Properties.Settings.Default.setVehicle_slowSpeedCutoff;

            goalPointLookAheadHold = Properties.Settings.Default.setVehicle_goalPointLookAheadHold;
            goalPointLookAheadMult = Properties.Settings.Default.setVehicle_goalPointLookAheadMult;
            goalPointAcquireFactor = Properties.Settings.Default.setVehicle_goalPointAcquireFactor;

            stanleyDistanceErrorGain = Properties.Settings.Default.stanleyDistanceErrorGain;
            stanleyHeadingErrorGain = Properties.Settings.Default.stanleyHeadingErrorGain;

            maxAngularVelocity = Properties.Settings.Default.setVehicle_maxAngularVelocity;
            maxSteerAngle = Properties.Settings.Default.setVehicle_maxSteerAngle;

            isHydLiftOn = false;

            trackWidth = Properties.Settings.Default.setVehicle_trackWidth;

            stanleyIntegralGainAB = Properties.Settings.Default.stanleyIntegralGainAB;
            stanleyIntegralDistanceAwayTriggerAB = Properties.Settings.Default.stanleyIntegralDistanceAwayTriggerAB;

            purePursuitIntegralGain = Properties.Settings.Default.purePursuitIntegralGainAB;
            vehicleType = Properties.Settings.Default.setVehicle_vehicleType;

            hydLiftLookAheadTime = Properties.Settings.Default.setVehicle_hydraulicLiftLookAhead;

            deadZoneHeading = Properties.Settings.Default.setAS_deadZoneHeading;
            deadZoneDelay = Properties.Settings.Default.setAS_deadZoneDelay;

            isInFreeDriveMode = false;

            //how far from line before it becomes Hold
            modeXTE = 0.2;

            //how long before hold is activated
            modeTime = 1;

            functionSpeedLimit = Properties.Settings.Default.setAS_functionSpeedLimit;
            maxSteerSpeed = Properties.Settings.Default.setAS_maxSteerSpeed;
            minSteerSpeed = Properties.Settings.Default.setAS_minSteerSpeed;

            uturnCompensation = Properties.Settings.Default.setAS_uTurnCompensation;
        }

        public int modeTimeCounter = 0;
        public double goalDistance = 0;

        public double UpdateGoalPointDistance()
        {
            double xTE = Math.Abs(modeActualXTE);
            double goalPointDistance = mf.avgSpeed * 0.05 * goalPointLookAheadMult;

            double LoekiAheadHold = goalPointLookAheadHold;
            double LoekiAheadAcquire = goalPointLookAheadHold * goalPointAcquireFactor;

            if (!mf.isBtnAutoSteerOn)
            {
                LoekiAheadHold = 5;
                LoekiAheadAcquire = LoekiAheadHold * goalPointAcquireFactor;
            }

            if (xTE <= 0.1)
            {
                goalPointDistance *= LoekiAheadHold; 
                goalPointDistance += LoekiAheadHold;
            }

            else if (xTE > 0.1 && xTE < 0.4)
            {
                xTE -= 0.1;

                LoekiAheadHold = (1 - (xTE / 0.3)) * (LoekiAheadHold - LoekiAheadAcquire);
                LoekiAheadHold += LoekiAheadAcquire;

                goalPointDistance *= LoekiAheadHold; 
                goalPointDistance += LoekiAheadHold;

            }
            else
            {
                goalPointDistance *= LoekiAheadAcquire; 
                goalPointDistance += LoekiAheadAcquire;
            }

            ////how far should goal point be away  - speed * seconds * kmph -> m/s then limit min value
            ////double goalPointDistance = mf.avgSpeed * goalPointLookAhead * 0.05 * goalPointLookAheadMult;
            //double goalPointDistance = mf.avgSpeed * goalPointLookAhead * 0.07; //0.05 * 1.4
            //goalPointDistance += goalPointLookAhead;

            //if (xTE < (modeXTE))
            //{
            //    if (modeTimeCounter > modeTime * 10)
            //    {
            //        //goalPointDistance = mf.avgSpeed * goalPointLookAheadHold * 0.05 * goalPointLookAheadMult;
            //        goalPointDistance = mf.avgSpeed * goalPointLookAheadHold * 0.07; //0.05 * 1.4
            //        goalPointDistance += goalPointLookAheadHold;
            //    }
            //    else
            //    {
            //        modeTimeCounter++;
            //    }
            //}
            //else
            //{
            //    modeTimeCounter = 0;
            //}

            if (goalPointDistance < 2) goalPointDistance = 2;
            goalDistance = goalPointDistance;

            return goalPointDistance;
        }

        public void DrawVehicle()
        {
            //draw vehicle
            GL.Rotate(glm.toDegrees(-mf.fixHeading), 0.0, 0.0, 1.0);
            //mf.font.DrawText3D(0, 0, "&TGF");
            if (mf.isFirstHeadingSet && !mf.tool.isToolFrontFixed)
            {
                if (!mf.tool.isToolRearFixed)
                {
                    GL.LineWidth(4);
                    //draw the rigid hitch
                    GL.Color3(0, 0, 0);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(0, mf.tool.hitchLength, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.End();

                    GL.LineWidth(1);
                    GL.Color3(1.237f, 0.037f, 0.0397f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(0, mf.tool.hitchLength, 0);
                    GL.Vertex3(0, 0, 0);
                    GL.End();
                }
                else
                {
                    GL.LineWidth(4);
                    //draw the rigid hitch
                    GL.Color3(0, 0, 0);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(-0.35, mf.tool.hitchLength, 0);
                    GL.Vertex3(-0.350, 0, 0);
                    GL.Vertex3(0.35, mf.tool.hitchLength, 0);
                    GL.Vertex3(0.350, 0, 0);
                    GL.End();

                    GL.LineWidth(1);
                    GL.Color3(1.237f, 0.037f, 0.0397f);
                    GL.Begin(PrimitiveType.Lines);
                    GL.Vertex3(-0.35, mf.tool.hitchLength, 0);
                    GL.Vertex3(-0.35, 0, 0);
                    GL.Vertex3(0.35, mf.tool.hitchLength, 0);
                    GL.Vertex3(0.35, 0, 0);
                    GL.End();
                }
            }
            //GL.Enable(EnableCap.Blend);

            //draw the vehicle Body

            if (!mf.isFirstHeadingSet && mf.headingFromSource != "Dual")
            {
                GL.Enable(EnableCap.Texture2D);
                GL.Color4(1,1,1, 0.75);
                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.QuestionMark]);        // Select Our Texture
                GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                GL.TexCoord2(1, 0); GL.Vertex2(5, 5); // Top Right
                GL.TexCoord2(0, 0); GL.Vertex2(1, 5); // Top Left
                GL.TexCoord2(1, 1); GL.Vertex2(5, 1); // Bottom Right
                GL.TexCoord2(0, 1); GL.Vertex2(1, 1); // Bottom Left
                GL.End();                       // Done Building Triangle Strip
                GL.Disable(EnableCap.Texture2D);
            }

            //3 vehicle types  tractor=0 harvestor=1 4wd=2

            if (mf.isVehicleImage)
            {
                if (vehicleType == 0)
                {
                    //vehicle body
                    GL.Enable(EnableCap.Texture2D);
                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);
                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);        // Select Our Texture

                    double leftAckermam, rightAckerman;

                    if (mf.timerSim.Enabled)
                    {
                        if (mf.sim.steerangleAve < 0)
                        {
                            leftAckermam = 1.25 * -mf.sim.steerangleAve;
                            rightAckerman = -mf.sim.steerangleAve;
                        }
                        else
                        {
                            leftAckermam = -mf.sim.steerangleAve;
                            rightAckerman = 1.25 * -mf.sim.steerangleAve;
                        }
                    }
                    else
                    {
                        if (mf.mc.actualSteerAngleDegrees < 0)
                        {
                            leftAckermam = 1.25 * -mf.mc.actualSteerAngleDegrees;
                            rightAckerman = -mf.mc.actualSteerAngleDegrees;
                        }
                        else
                        {
                            leftAckermam = -mf.mc.actualSteerAngleDegrees;
                            rightAckerman = 1.25 * -mf.mc.actualSteerAngleDegrees;
                        }
                    }

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth, wheelbase * 1.5); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth, wheelbase * 1.5); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth, -wheelbase * 0.5); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth, -wheelbase * 0.5); // Bottom Left

                    GL.End();                       // Done Building Triangle Strip

                    //right wheel
                    GL.PushMatrix();
                    GL.Translate(trackWidth * 0.5, wheelbase, 0);
                    GL.Rotate(rightAckerman, 0, 0, 1);

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FrontWheels]);        // Select Our Texture
                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth * 0.5, wheelbase * 0.75); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth * 0.5, wheelbase * 0.75); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth * 0.5, -wheelbase * 0.75); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth * 0.5, -wheelbase * 0.75); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip

                    GL.PopMatrix();

                    //Left Wheel
                    GL.PushMatrix();

                    GL.Translate(-trackWidth * 0.5, wheelbase, 0);
                    GL.Rotate(leftAckermam, 0, 0, 1);

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth * 0.5, wheelbase * 0.75); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth * 0.5, wheelbase * 0.75); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth * 0.5, -wheelbase * 0.75); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth * 0.5, -wheelbase * 0.75); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip

                    GL.PopMatrix();
                    //disable, straight color
                    GL.Disable(EnableCap.Texture2D);
                    //GL.Disable(EnableCap.Blend);
                }
                else if (vehicleType == 1) //Harvestor
                {
                    //vehicle body
                    GL.Enable(EnableCap.Texture2D);

                    double leftAckermam, rightAckerman;

                    if (mf.timerSim.Enabled)
                    {
                        if (mf.sim.steerAngle < 0)
                        {
                            leftAckermam = 1.25 * mf.sim.steerAngle;
                            rightAckerman = mf.sim.steerAngle;
                        }
                        else
                        {
                            leftAckermam = mf.sim.steerAngle;
                            rightAckerman = 1.25 * mf.sim.steerAngle;
                        }
                    }
                    else
                    {
                        if (mf.mc.actualSteerAngleDegrees < 0)
                        {
                            leftAckermam = 1.25 * mf.mc.actualSteerAngleDegrees;
                            rightAckerman = mf.mc.actualSteerAngleDegrees;
                        }
                        else
                        {
                            leftAckermam = mf.mc.actualSteerAngleDegrees;
                            rightAckerman = 1.25 * mf.mc.actualSteerAngleDegrees;
                        }
                    }

                    GL.Color4((byte)20, (byte)20, (byte)20, mf.vehicleOpacityByte);
                    //right wheel
                    GL.PushMatrix();
                    GL.Translate(trackWidth * 0.5, -wheelbase, 0);
                    GL.Rotate(rightAckerman, 0, 0, 1);

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FrontWheels]);        // Select Our Texture

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth * 0.25, wheelbase * 0.5); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth * 0.25, wheelbase * 0.5); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth * 0.25, -wheelbase * 0.5); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth * 0.25, -wheelbase * 0.5); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip

                    GL.PopMatrix();

                    //Left Wheel
                    GL.PushMatrix();

                    GL.Translate(-trackWidth * 0.5, -wheelbase, 0);
                    GL.Rotate(leftAckermam, 0, 0, 1);

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth * 0.25, wheelbase * 0.5); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth * 0.25, wheelbase * 0.5); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth * 0.25, -wheelbase * 0.5); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth * 0.25, -wheelbase * 0.5); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip

                    GL.PopMatrix();

                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);
                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(uint)FormGPS.textures.Harvester]);        // Select Our Texture
                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth, wheelbase * 1.5); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth, wheelbase * 1.5); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth, -wheelbase * 1.5); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth, -wheelbase * 1.5); // Bottom Left

                    GL.End();                       // Done Building Triangle Strip

                    //disable, straight color
                    GL.Disable(EnableCap.Texture2D);
                    //GL.Disable(EnableCap.Blend);
                }
                else if (vehicleType == 2) //4WD - Image Text # Front is 16 Rear is 17
                {
                    double modelSteerAngle;

                    if (mf.timerSim.Enabled)
                        modelSteerAngle = 0.5 * mf.sim.steerAngle;
                    else
                        modelSteerAngle = 0.5 * mf.mc.actualSteerAngleDegrees;

                    GL.Enable(EnableCap.Texture2D);
                    GL.Color4(mf.vehicleColor.R, mf.vehicleColor.G, mf.vehicleColor.B, mf.vehicleOpacityByte);

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDRear]);        // Select Our Texture

                    GL.PushMatrix();
                    GL.Translate(0, -wheelbase * 0.5, 0);
                    GL.Rotate(modelSteerAngle, 0, 0, 1);

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth, wheelbase * 0.65); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth, wheelbase * 0.65); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth, -wheelbase * 0.65); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth, -wheelbase * 0.65); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip

                    GL.PopMatrix();

                    GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.FourWDFront]);        // Select Our Texture

                    GL.PushMatrix();
                    GL.Translate(0, wheelbase * 0.5, 0);
                    GL.Rotate(-modelSteerAngle, 0, 0, 1);

                    GL.Begin(PrimitiveType.TriangleStrip);              // Build Quad From A Triangle Strip
                    GL.TexCoord2(1, 0); GL.Vertex2(trackWidth, wheelbase * 0.65); // Top Right
                    GL.TexCoord2(0, 0); GL.Vertex2(-trackWidth, wheelbase * 0.65); // Top Left
                    GL.TexCoord2(1, 1); GL.Vertex2(trackWidth, -wheelbase * 0.65); // Bottom Right
                    GL.TexCoord2(0, 1); GL.Vertex2(-trackWidth, -wheelbase * 0.65); // Bottom Left
                    GL.End();                       // Done Building Triangle Strip

                    GL.PopMatrix();
                    GL.Disable(EnableCap.Texture2D);
                }
            }
            else
            {
                GL.Color4(1.2, 1.20, 0.0, mf.vehicleOpacity);
                GL.Begin(PrimitiveType.TriangleFan);
                GL.Vertex3(0, antennaPivot, -0.0);
                GL.Vertex3(1.0, -0, 0.0);
                GL.Color4(0.0, 1.20, 1.22, mf.vehicleOpacity);
                GL.Vertex3(0, wheelbase, 0.0);
                GL.Color4(1.220, 0.0, 1.2, mf.vehicleOpacity);
                GL.Vertex3(-1.0, -0, 0.0);
                GL.Vertex3(1.0, -0, 0.0);
                GL.End();

                GL.LineWidth(3);
                GL.Color3(0.12, 0.12, 0.12);
                GL.Begin(PrimitiveType.LineLoop);
                {
                    GL.Vertex3(-1.0, 0, 0);
                    GL.Vertex3(1.0, 0, 0);
                    GL.Vertex3(0, wheelbase, 0);
                }
                GL.End();
            }

            //if (mf.camera.camSetDistance > -75 && mf.isFirstHeadingSet)
            //{
            //    //draw the bright antenna dot
            //    GL.PointSize(16);
            //    GL.Begin(PrimitiveType.Points);
            //    GL.Color3(0, 0, 0);
            //    GL.Vertex3(-antennaOffset, antennaPivot, 0.1);
            //    GL.End();

            //    GL.PointSize(10);
            //    GL.Begin(PrimitiveType.Points);
            //    GL.Color3(0.20, 0.98, 0.98);
            //    GL.Vertex3(-antennaOffset, antennaPivot, 0.1);
            //    GL.End();
            //}

            if (mf.bnd.isBndBeingMade && mf.bnd.isDrawAtPivot)
            {
                if (mf.bnd.isDrawRightSide)
                {
                    GL.LineWidth(2);
                    GL.Color3(0.0, 1.270, 0.0);
                    GL.Begin(PrimitiveType.LineStrip);
                    {
                        GL.Vertex3(0.0, 0, 0);
                        GL.Color3(1.270, 1.220, 0.20);
                        GL.Vertex3(mf.bnd.createBndOffset, 0, 0);
                        GL.Vertex3(mf.bnd.createBndOffset * 0.75, 0.25, 0);
                    }
                    GL.End();
                }

                //draw on left side
                else
                {
                    GL.LineWidth(2);
                    GL.Color3(0.0, 1.270, 0.0);
                    GL.Begin(PrimitiveType.LineStrip);
                    {
                        GL.Vertex3(0.0, 0, 0);
                        GL.Color3(1.270, 1.220, 0.20);
                        GL.Vertex3(-mf.bnd.createBndOffset, 0, 0);
                        GL.Vertex3(-mf.bnd.createBndOffset * 0.75, 0.25, 0);
                    }
                    GL.End();
                }
            }

            //Svenn Arrow
            if (mf.isSvennArrowOn && mf.camera.camSetDistance > -1000)
            {
                //double offs = mf.curve.distanceFromCurrentLinePivot * 0.3;
                double svennDist = mf.camera.camSetDistance * -0.07;
                double svennWidth = svennDist * 0.22;
                GL.LineWidth(mf.ABLine.lineWidth);
                GL.Color3(0.95, 0.95, 0.10);
                GL.Begin(PrimitiveType.LineStrip);
                {
                    GL.Vertex3(svennWidth, wheelbase + svennDist, 0.0);
                    GL.Vertex3(0, wheelbase + svennWidth + 0.5 + svennDist, 0.0);
                    GL.Vertex3(-svennWidth, wheelbase + svennDist, 0.0);
                }
                GL.End();
            }

            GL.LineWidth(1);

            //if (mf.camera.camSetDistance < -500)
            //{
            //    GL.Color4(0.5f, 0.5f, 1.2f, 0.25);
            //    double theta = glm.twoPI / 20;
            //    double c = Math.Cos(theta);//precalculate the sine and cosine
            //    double s = Math.Sin(theta);

            //    double x = mf.camera.camSetDistance * -.015;//we start at angle = 0
            //    double y = 0;
            //    GL.LineWidth(1);
            //    GL.Begin(PrimitiveType.TriangleFan);
            //    GL.Vertex3(x, y, 0.0);
            //    for (int ii = 0; ii < 20; ii++)
            //    {
            //        //output vertex
            //        GL.Vertex3(x, y, 0.0);

            //        //apply the rotation matrix
            //        double t = x;
            //        x = (c * x) - (s * y);
            //        y = (s * t) + (c * y);
            //        // GL.Vertex3(x, y, 0.0);
            //    }
            //    GL.End();
            //    GL.Color3(0.5f, 1.2f, 0.2f);
            //    GL.LineWidth(2);
            //    GL.Begin(PrimitiveType.LineLoop);

            //    for (int ii = 0; ii < 20; ii++)
            //    {
            //        //output vertex
            //        GL.Vertex3(x, y, 0.0);

            //        //apply the rotation matrix
            //        double t = x;
            //        x = (c * x) - (s * y);
            //        y = (s * t) + (c * y);
            //        // GL.Vertex3(x, y, 0.0);
            //    }
            //    GL.End();
            //}
        }
    }
}

//just a triangle for vehicle
//GL.LineWidth(3);
//GL.Color3(0.80, 0.80, 1.29);
//GL.Begin(PrimitiveType.LineLoop);
//{
//    GL.Vertex3(-1.0, 0, 0);
//    GL.Vertex3(1.0, 0, 0);
//    GL.Vertex3(0, wheelbase, 0);
//}
//GL.End();

//GL.Begin(PrimitiveType.TriangleFan);
//{
//    GL.Color3(1.250, 1.25, 0.32);
//    GL.Vertex3(0, 5.5, -0.0);
//    GL.Vertex3(0.35, 4.85, 0.0);
//    GL.Vertex3(0, 5.2, 0.0);
//    GL.Vertex3(-0.35, 4.85, 0.0);
//    GL.Vertex3(0, 5.5, 0.0);
//}
//GL.End();

//GL.LineWidth(1);
//GL.Begin(PrimitiveType.LineLoop);
//{
//    GL.Color3(0.0, 0.0, 0.0);
//    GL.Vertex3(0, 5.5, -0.0);
//    GL.Vertex3(0.35, 4.85, 0.0);
//    GL.Vertex3(0, 5.2, 0.0);
//    GL.Vertex3(-0.35, 4.85, 0.0);
//    GL.Vertex3(0, 5.5, 0.0);
//}
//GL.End();
//    GL.LineWidth(2);
//    //Svenn Arrow
//    GL.Color3(1.2, 1.25, 0.10);
//    GL.Begin(PrimitiveType.LineStrip);
//    {
//        GL.Vertex3(0.6, wheelbase + 6, 0.0);
//        GL.Vertex3(0, wheelbase + 8, 0.0);
//        GL.Vertex3(-0.6, wheelbase + 6, 0.0);
//    }
//    GL.End();

////draw the vehicle Body

//if (!mf.vehicle.isHydLiftOn)
//{
//    GL.Color3(1.2, 1.20, 0.0);
//    GL.Begin(PrimitiveType.TriangleFan);
//    GL.Vertex3(0, antennaPivot, -0.0);
//    GL.Vertex3(1.0, -0, 0.0);
//    GL.Color3(0.0, 1.20, 1.22);
//    GL.Vertex3(0, wheelbase, 0.0);
//    GL.Color3(1.220, 0.0, 1.2);
//    GL.Vertex3(-1.0, -0, 0.0);
//    GL.Vertex3(1.0, -0, 0.0);
//    GL.End();
//}
//else
//{
//    if (mf.hd.isToolUp)
//    {
//        GL.Color3(0.0, 1.250, 0.0);
//        GL.Begin(PrimitiveType.TriangleFan);
//        GL.Vertex3(0, antennaPivot, -0.0);
//        GL.Vertex3(1.0, -0, 0.0);
//        GL.Vertex3(0, wheelbase, 0.0);
//        GL.Vertex3(-1.0, -0, 0.0);
//        GL.Vertex3(1.0, -0, 0.0);
//        GL.End();
//    }
//    else
//    {
//        GL.Color3(1.250, 0.0, 0.0);
//        GL.Begin(PrimitiveType.TriangleFan);
//        GL.Vertex3(0, antennaPivot, -0.0);
//        GL.Vertex3(1.0, -0, 0.0);
//        GL.Vertex3(0, wheelbase, 0.0);
//        GL.Vertex3(-1.0, -0, 0.0);
//        GL.Vertex3(1.0, -0, 0.0);
//        GL.End();
//    }
//}
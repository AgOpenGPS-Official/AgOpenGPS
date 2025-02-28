//Please, if you use this, share the improvements

using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Culture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //very first fix to setup grid etc
        public bool isFirstFixPositionSet = false, isGPSPositionInitialized = false, isFirstHeadingSet = false,
            isReverse = false, isSteerInReverse = true, isSuperSlow = false;
        public double startGPSHeading = 0;

        //string to record fixes for elevation maps
        public StringBuilder sbGrid = new StringBuilder();

        // autosteer variables for sending serial
        public short guidanceLineDistanceOff, guidanceLineSteerAngle;
        public double avGuidanceSteerAngle;

        public short errorAngVel;
        public double setAngVel, actAngVel;
        public bool isConstantContourOn;

        //guidance line look ahead
        public double guidanceLookAheadTime = 2;
        public vec2 guidanceLookPos = new vec2(0, 0);
        public double dualReverseDetectionDistance;

        //for heading or Atan2 as camera
        public string headingFromSource, headingFromSourceBak;

        public vec3 pivotAxlePos = new vec3(0, 0, 0);
        public vec3 steerAxlePos = new vec3(0, 0, 0);
        public vec3 toolPivotPos = new vec3(0, 0, 0);
        public vec3 toolPos = new vec3(0, 0, 0);
        public vec3 tankPos = new vec3(0, 0, 0);
        public vec2 hitchPos = new vec2(0, 0);

        //history
        public vec2 prevFix = new vec2(0, 0);
        public vec2 prevJumpFix = new vec2(0, 0);
        public vec2 prevDistFix = new vec2(0, 0);
        public vec2 lastReverseFix = new vec2(0, 0);

        //headings
        public double fixHeading = 0.0, camHeading = 0.0, smoothCamHeading = 0, gpsHeading = 10.0, prevGPSHeading = 0.0;

        //storage for the cos and sin of heading
        public double cosSectionHeading = 1.0, sinSectionHeading = 0.0;

        //how far travelled since last section was added, section points
        double sectionTriggerDistance = 0, contourTriggerDistance = 0, sectionTriggerStepDistance = 0, gridTriggerDistance = 0;

        public vec2 prevSectionPos = new vec2(0, 0);
        public vec2 prevContourPos = new vec2(0, 0);
        public vec2 prevGridPos = new vec2(0, 0);
        public int patchCounter = 0;

        public vec2 prevBoundaryPos = new vec2(0, 0);

        //Everything is so wonky at the start
        int startCounter = 0;

        //individual points for the flags in a list
        public List<CFlag> flagPts = new List<CFlag>();

        //tally counters for display
        //public double totalSquareMetersWorked = 0, totalUserSquareMeters = 0, userSquareMetersAlarm = 0;

        public double avgSpeed, previousSpeed;//for average speed
        public int crossTrackError;

        //youturn
        public double distancePivotToTurnLine = -2222;
        public double distanceToolToTurnLine = -2222;

        //the value to fill in you turn progress bar
        public int youTurnProgressBar = 0;

        //IMU 
        public double rollCorrectionDistance = 0;
        public double imuGPS_Offset, imuCorrected;

        //step position - slow speed spinner killer
        private int currentStepFix = 0;
        private const int totalFixSteps = 10;
        public vecFix2Fix[] stepFixPts = new vecFix2Fix[totalFixSteps];
        public double distanceCurrentStepFix = 0, distanceCurrentStepFixDisplay = 0, minHeadingStepDist = 1, startSpeed = 0.5;
        public double fixToFixHeadingDistance = 0, gpsMinimumStepDistance = 0.05;

        public bool isChangingDirection, isReverseWithIMU;

        private double nowHz = 0, filteredDelta = 0, delta = 0;

        public bool isRTK_AlarmOn, isRTK_KillAutosteer;

        public double headlandDistanceDelta = 0, boundaryDistanceDelta = 0;

        public vec2 lastGPS = new vec2(0, 0);

        public double uncorrectedEastingGraph = 0;
        public double correctionDistanceGraph = 0;

        double frameTimeRough = 3;
        public double timeSliceOfLastFix = 0;

        public bool isMaxAngularVelocity = false;

        public int minSteerSpeedTimer = 0;

        //public vec2 jumpFix = new vec2(0, 0);
        //public double jumpDistance = 0, jumpDistanceMax;
        //public double jumpDistanceAlarm = 20;
        //public int jumpCounter = 0;

        public void UpdateFixPosition()
        {
            //swFrame.Stop();
            //Measure the frequency of the GPS updates
            timeSliceOfLastFix = (double)(swFrame.ElapsedTicks) / (double)System.Diagnostics.Stopwatch.Frequency;
            swFrame.Reset();
            swFrame.Start();

            //get Hz from timeslice
            nowHz = 1 / timeSliceOfLastFix;
            if (nowHz > 70) nowHz = 70;
            if (nowHz < 3) nowHz = 3;

            //simple comp filter
            gpsHz = 0.98 * gpsHz + 0.02 * nowHz;

            //Initialization counter
            startCounter++;

            if (!isGPSPositionInitialized)
            {
                InitializeFirstFewGPSPositions();
                return;
            }

            pn.speed = pn.vtgSpeed;
            pn.AverageTheSpeed();

            if (Properties.Settings.Default.setGPS_headingFromWhichSource == "Dual" && ahrs.autoSwitchDualFixOn)
            {
                if (Math.Abs(pn.speed) > ahrs.autoSwitchDualFixSpeed)
                {
                    headingFromSource = "Fix";
                    ahrs.isDualAsIMU = true;
                }
                else
                {
                    headingFromSource = "Dual";
                    ahrs.isDualAsIMU = false;
                    ahrs.imuHeading = 99999;
                }
            }

            #region Heading
            switch (headingFromSource)
            {
                //calculate current heading only when moving, otherwise use last
                case "Fix":
                    {
                        #region Start

                        if (Properties.Settings.Default.setGPS_headingFromWhichSource == "Dual" && ahrs.autoSwitchDualFixOn)
                        {
                            lblSpeed.ForeColor = System.Drawing.Color.Red;
                        }

                        distanceCurrentStepFixDisplay = glm.Distance(prevDistFix, pn.fix);
                        distanceCurrentStepFixDisplay *= 100;
                        prevDistFix = pn.fix;

                        if (Math.Abs(avgSpeed) < 1.5 && !isFirstHeadingSet)
                            goto byPass;

                        if (!isFirstHeadingSet) //set in steer settings, Stanley
                        {
                            prevFix.easting = stepFixPts[0].easting; prevFix.northing = stepFixPts[0].northing;

                            if (stepFixPts[2].isSet == 0)
                            {
                                //this is the first position no roll or offset correction
                                if (stepFixPts[0].isSet == 0)
                                {
                                    stepFixPts[0].easting = pn.fix.easting;
                                    stepFixPts[0].northing = pn.fix.northing;
                                    stepFixPts[0].isSet = 1;
                                    return;
                                }

                                //and the second
                                if (stepFixPts[1].isSet == 0)
                                {
                                    for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                                    stepFixPts[0].easting = pn.fix.easting;
                                    stepFixPts[0].northing = pn.fix.northing;
                                    stepFixPts[0].isSet = 1;
                                    return;
                                }

                                //the critcal moment for checking initial direction/heading.
                                for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                                stepFixPts[0].easting = pn.fix.easting;
                                stepFixPts[0].northing = pn.fix.northing;
                                stepFixPts[0].isSet = 1;

                                gpsHeading = Math.Atan2(pn.fix.easting - stepFixPts[2].easting,
                                    pn.fix.northing - stepFixPts[2].northing);

                                if (gpsHeading < 0) gpsHeading += glm.twoPI;
                                else if (gpsHeading >= glm.twoPI) gpsHeading -= glm.twoPI;

                                fixHeading = gpsHeading;

                                //set the imu to gps heading offset
                                if (ahrs.imuHeading != 99999)
                                {
                                    double imuHeading = (glm.toRadians(ahrs.imuHeading));
                                    imuGPS_Offset = 0;

                                    //Difference between the IMU heading and the GPS heading
                                    double gyroDelta = (imuHeading + imuGPS_Offset) - gpsHeading;

                                    if (gyroDelta < 0) gyroDelta += glm.twoPI;
                                    else if (gyroDelta > glm.twoPI) gyroDelta -= glm.twoPI;

                                    //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                                    if (gyroDelta >= -glm.PIBy2 && gyroDelta <= glm.PIBy2) gyroDelta *= -1.0;
                                    else
                                    {
                                        if (gyroDelta > glm.PIBy2) { gyroDelta = glm.twoPI - gyroDelta; }
                                        else { gyroDelta = (glm.twoPI + gyroDelta) * -1.0; }
                                    }
                                    if (gyroDelta > glm.twoPI) gyroDelta -= glm.twoPI;
                                    else if (gyroDelta < -glm.twoPI) gyroDelta += glm.twoPI;

                                    //moe the offset to line up imu with gps
                                    imuGPS_Offset = (gyroDelta);
                                    imuGPS_Offset = Math.Round(imuGPS_Offset, 6);

                                    if (imuGPS_Offset >= glm.twoPI) imuGPS_Offset -= glm.twoPI;
                                    else if (imuGPS_Offset <= 0) imuGPS_Offset += glm.twoPI;

                                    //determine the Corrected heading based on gyro and GPS
                                    imuCorrected = imuHeading + imuGPS_Offset;
                                    if (imuCorrected >= glm.twoPI) imuCorrected -= glm.twoPI;
                                    else if (imuCorrected < 0) imuCorrected += glm.twoPI;

                                    fixHeading = imuCorrected;
                                }

                                //set the camera 
                                camHeading = glm.toDegrees(gpsHeading);

                                //now we have a heading, fix the first 3
                                if (vehicle.VehicleConfig.AntennaOffset != 0)
                                {
                                    for (int i = 0; i < 3; i++)
                                    {
                                        stepFixPts[i].easting = (Math.Cos(-gpsHeading) * vehicle.VehicleConfig.AntennaOffset) + stepFixPts[i].easting;
                                        stepFixPts[i].northing = (Math.Sin(-gpsHeading) * vehicle.VehicleConfig.AntennaOffset) + stepFixPts[i].northing;
                                    }
                                }

                                if (ahrs.imuRoll != 88888)
                                {
                                    //change for roll to the right is positive times -1
                                    rollCorrectionDistance = Math.Tan(glm.toRadians((ahrs.imuRoll))) * -vehicle.VehicleConfig.AntennaHeight;

                                    // roll to left is positive  **** important!!
                                    // not any more - April 30, 2019 - roll to right is positive Now! Still Important
                                    for (int i = 0; i < 3; i++)
                                    {
                                        stepFixPts[i].easting = (Math.Cos(-gpsHeading) * rollCorrectionDistance) + stepFixPts[i].easting;
                                        stepFixPts[i].northing = (Math.Sin(-gpsHeading) * rollCorrectionDistance) + stepFixPts[i].northing;
                                    }
                                }

                                //get the distance from first to 2nd point, update fix with new offset/roll
                                stepFixPts[0].distance = glm.Distance(stepFixPts[1], stepFixPts[0]);
                                pn.fix.easting = stepFixPts[0].easting;
                                pn.fix.northing = stepFixPts[0].northing;

                                isFirstHeadingSet = true;
                                TimedMessageBox(2000, "Direction Reset", "Forward is Set");
                                Log.EventWriter("Forward Is Set");

                                lastGPS = pn.fix;

                                return;
                            }
                        }
                        #endregion

                        #region Offset Roll
                        if (vehicle.VehicleConfig.AntennaOffset != 0)
                        {
                            pn.fix.easting = (Math.Cos(-gpsHeading) * vehicle.VehicleConfig.AntennaOffset) + pn.fix.easting;
                            pn.fix.northing = (Math.Sin(-gpsHeading) * vehicle.VehicleConfig.AntennaOffset) + pn.fix.northing;
                        }

                        uncorrectedEastingGraph = pn.fix.easting;

                        //originalEasting = pn.fix.easting;
                        if (ahrs.imuRoll != 88888)
                        {
                            //change for roll to the right is positive times -1
                            rollCorrectionDistance = Math.Sin(glm.toRadians((ahrs.imuRoll))) * -vehicle.VehicleConfig.AntennaHeight;
                            correctionDistanceGraph = rollCorrectionDistance;

                            pn.fix.easting = (Math.Cos(-gpsHeading) * rollCorrectionDistance) + pn.fix.easting;
                            pn.fix.northing = (Math.Sin(-gpsHeading) * rollCorrectionDistance) + pn.fix.northing;
                        }

                        #endregion

                        #region Fix Heading

                        //how far since last fix
                        distanceCurrentStepFix = glm.Distance(stepFixPts[0], pn.fix);

                        if (distanceCurrentStepFix < gpsMinimumStepDistance)
                        {
                            goto byPass;
                        }

                        //save a copy of previous for jump test
                        //jumpFix.easting = stepFixPts[0].easting; jumpFix.northing = stepFixPts[0].northing;
                        
                        if ((fd.distanceUser += distanceCurrentStepFix) > 9999) fd.distanceUser = 0;

                        double minFixHeadingDistSquared = minHeadingStepDist * minHeadingStepDist;
                        fixToFixHeadingDistance = 0;

                        for (int i = 0; i < totalFixSteps; i++)
                        {
                            fixToFixHeadingDistance = glm.DistanceSquared(stepFixPts[i], pn.fix);
                            currentStepFix = i;

                            if (fixToFixHeadingDistance > minFixHeadingDistSquared)
                            {
                                break;
                            }
                        }

                        if (fixToFixHeadingDistance < minFixHeadingDistSquared * 0.5)
                            goto byPass;

                        double newGPSHeading = Math.Atan2(pn.fix.easting - stepFixPts[currentStepFix].easting,
                                                pn.fix.northing - stepFixPts[currentStepFix].northing);
                        if (newGPSHeading < 0) newGPSHeading += glm.twoPI;


                        //imu on board
                        if (ahrs.imuHeading != 99999)
                        {
                            isChangingDirection = false;

                            if (ahrs.isReverseOn)
                            {
                                ////what is angle between the last valid heading before stopping and one just now
                                delta = Math.Abs(Math.PI - Math.Abs(Math.Abs(newGPSHeading - imuCorrected) - Math.PI));

                                //ie change in direction
                                if (delta > 1.57) //
                                {
                                    isReverse = true;
                                    newGPSHeading += Math.PI;
                                    if (newGPSHeading < 0) newGPSHeading += glm.twoPI;
                                    else if (newGPSHeading >= glm.twoPI) newGPSHeading -= glm.twoPI;
                                    isReverseWithIMU = true;
                                }
                                else
                                {
                                    isReverse = false;
                                    isReverseWithIMU = false;
                                }
                            }
                            else
                            {
                                isReverse = false;
                            }

                            if (isReverse)
                                newGPSHeading -= glm.toRadians(vehicle.VehicleConfig.AntennaPivot / 1
                                    * mc.actualSteerAngleDegrees * ahrs.reverseComp);
                            else
                                newGPSHeading -= glm.toRadians(vehicle.VehicleConfig.AntennaPivot / 1
                                    * mc.actualSteerAngleDegrees * ahrs.forwardComp);

                            if (newGPSHeading < 0) newGPSHeading += glm.twoPI;
                            else if (newGPSHeading >= glm.twoPI) newGPSHeading -= glm.twoPI;

                            gpsHeading = newGPSHeading;

                            #region IMU Fusion

                            // IMU Fusion with heading correction, add the correction
                            //current gyro angle in radians
                            double imuHeading = (glm.toRadians(ahrs.imuHeading));

                            //Difference between the IMU heading and the GPS heading
                            double gyroDelta = 0;

                            //if (!isReverseWithIMU)
                            gyroDelta = (imuHeading + imuGPS_Offset) - gpsHeading;
                            //else
                            //{
                            //    gyroDelta = 0;
                            //}

                            if (gyroDelta < 0) gyroDelta += glm.twoPI;
                            else if (gyroDelta >= glm.twoPI) gyroDelta -= glm.twoPI;

                            //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                            if (gyroDelta >= -glm.PIBy2 && gyroDelta <= glm.PIBy2) gyroDelta *= -1.0;
                            else
                            {
                                if (gyroDelta > glm.PIBy2) { gyroDelta = glm.twoPI - gyroDelta; }
                                else { gyroDelta = (glm.twoPI + gyroDelta) * -1.0; }
                            }
                            if (gyroDelta > glm.twoPI) gyroDelta -= glm.twoPI;
                            else if (gyroDelta < -glm.twoPI) gyroDelta += glm.twoPI;

                            //moe the offset to line up imu with gps
                            if (!isReverseWithIMU)
                                imuGPS_Offset += (gyroDelta * (ahrs.fusionWeight));
                            else
                                imuGPS_Offset += (gyroDelta * (0.02));

                            if (imuGPS_Offset > glm.twoPI) imuGPS_Offset -= glm.twoPI;
                            else if (imuGPS_Offset < 0) imuGPS_Offset += glm.twoPI;

                            //determine the Corrected heading based on gyro and GPS
                            imuCorrected = imuHeading + imuGPS_Offset;
                            if (imuCorrected >= glm.twoPI) imuCorrected -= glm.twoPI;
                            else if (imuCorrected < 0) imuCorrected += glm.twoPI;

                            //use imu as heading when going slow
                            fixHeading = imuCorrected;

                            #endregion
                        }
                        else
                        {
                            if (ahrs.isReverseOn)
                            {

                                ////what is angle between the last valid heading before stopping and one just now
                                delta = Math.Abs(Math.PI - Math.Abs(Math.Abs(newGPSHeading - gpsHeading) - Math.PI));

                                filteredDelta = delta * 0.2 + filteredDelta * 0.8;

                                //filtered delta different then delta
                                if (Math.Abs(filteredDelta - delta) > 0.5)
                                {
                                    isChangingDirection = true;
                                }
                                else
                                {
                                    isChangingDirection = false;
                                }

                                //we can't be sure if changing direction so do nothing
                                if (isChangingDirection)
                                    goto byPass;

                                //ie change in direction
                                if (filteredDelta > 1.57) //
                                {
                                    isReverse = true;
                                    newGPSHeading += Math.PI;
                                    if (newGPSHeading < 0) newGPSHeading += glm.twoPI;
                                    else if (newGPSHeading >= glm.twoPI) newGPSHeading -= glm.twoPI;
                                }
                                else
                                    isReverse = false;

                                if (isReverse)
                                    newGPSHeading -= glm.toRadians(vehicle.VehicleConfig.AntennaPivot / 1
                                        * mc.actualSteerAngleDegrees * ahrs.reverseComp);
                                else
                                    newGPSHeading -= glm.toRadians(vehicle.VehicleConfig.AntennaPivot / 1
                                        * mc.actualSteerAngleDegrees * ahrs.forwardComp);

                                if (newGPSHeading < 0) newGPSHeading += glm.twoPI;
                                else if (newGPSHeading >= glm.twoPI) newGPSHeading -= glm.twoPI;
                            }
                            else
                            {
                                isReverse = false;
                                isChangingDirection = false;
                            }

                            //set the headings
                            fixHeading = gpsHeading = newGPSHeading;
                        }

                        //save current fix and set as valid
                        for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                        stepFixPts[0].easting = pn.fix.easting;
                        stepFixPts[0].northing = pn.fix.northing;
                        stepFixPts[0].isSet = 1;

                        #endregion

                        #region Camera

                        double camDelta = fixHeading - smoothCamHeading;

                        if (camDelta < 0) camDelta += glm.twoPI;
                        else if (camDelta > glm.twoPI) camDelta -= glm.twoPI;

                        //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                        if (camDelta >= -glm.PIBy2 && camDelta <= glm.PIBy2) camDelta *= -1.0;
                        else
                        {
                            if (camDelta > glm.PIBy2) { camDelta = glm.twoPI - camDelta; }
                            else { camDelta = (glm.twoPI + camDelta) * -1.0; }
                        }
                        if (camDelta > glm.twoPI) camDelta -= glm.twoPI;
                        else if (camDelta < -glm.twoPI) camDelta += glm.twoPI;

                        smoothCamHeading -= camDelta * camera.camSmoothFactor;

                        if (smoothCamHeading > glm.twoPI) smoothCamHeading -= glm.twoPI;
                        else if (smoothCamHeading < -glm.twoPI) smoothCamHeading += glm.twoPI;

                        camHeading = glm.toDegrees(smoothCamHeading);

                    #endregion


                    //Calculate a million other things
                    byPass:
                        if (ahrs.imuHeading != 99999)
                        {
                            imuCorrected = (glm.toRadians(ahrs.imuHeading)) + imuGPS_Offset;
                            if (imuCorrected >= glm.twoPI) imuCorrected -= glm.twoPI;
                            else if (imuCorrected < 0) imuCorrected += glm.twoPI;

                            //use imu as heading when going slow
                            fixHeading = imuCorrected;
                        }

                        camDelta = fixHeading - smoothCamHeading;

                        if (camDelta < 0) camDelta += glm.twoPI;
                        else if (camDelta > glm.twoPI) camDelta -= glm.twoPI;

                        //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                        if (camDelta >= -glm.PIBy2 && camDelta <= glm.PIBy2) camDelta *= -1.0;
                        else
                        {
                            if (camDelta > glm.PIBy2) { camDelta = glm.twoPI - camDelta; }
                            else { camDelta = (glm.twoPI + camDelta) * -1.0; }
                        }
                        if (camDelta > glm.twoPI) camDelta -= glm.twoPI;
                        else if (camDelta < -glm.twoPI) camDelta += glm.twoPI;

                        smoothCamHeading -= camDelta * camera.camSmoothFactor;

                        if (smoothCamHeading > glm.twoPI) smoothCamHeading -= glm.twoPI;
                        else if (smoothCamHeading < -glm.twoPI) smoothCamHeading += glm.twoPI;

                        camHeading = glm.toDegrees(smoothCamHeading);

                        TheRest();
                        break;
                    }

                case "VTG":
                    {
                        isFirstHeadingSet = true;
                        if (avgSpeed > 1)
                        {
                            //use NMEA headings for camera and tractor graphic
                            fixHeading = glm.toRadians(pn.headingTrue);
                            camHeading = pn.headingTrue;
                            gpsHeading = fixHeading;
                        }

                        //grab the most current fix to last fix distance
                        distanceCurrentStepFix = glm.Distance(pn.fix, prevFix);

                        #region Antenna Offset

                        if (vehicle.VehicleConfig.AntennaOffset != 0)
                        {
                            pn.fix.easting = (Math.Cos(-fixHeading) * vehicle.VehicleConfig.AntennaOffset) + pn.fix.easting;
                            pn.fix.northing = (Math.Sin(-fixHeading) * vehicle.VehicleConfig.AntennaOffset) + pn.fix.northing;
                        }
                        #endregion

                        uncorrectedEastingGraph = pn.fix.easting;

                        //an IMU with heading correction, add the correction
                        if (ahrs.imuHeading != 99999)
                        {
                            //current gyro angle in radians
                            double correctionHeading = (glm.toRadians(ahrs.imuHeading));

                            //Difference between the IMU heading and the GPS heading
                            double gyroDelta = (correctionHeading + imuGPS_Offset) - gpsHeading;
                            if (gyroDelta < 0) gyroDelta += glm.twoPI;

                            //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                            if (gyroDelta >= -glm.PIBy2 && gyroDelta <= glm.PIBy2) gyroDelta *= -1.0;
                            else
                            {
                                if (gyroDelta > glm.PIBy2) { gyroDelta = glm.twoPI - gyroDelta; }
                                else { gyroDelta = (glm.twoPI + gyroDelta) * -1.0; }
                            }
                            if (gyroDelta > glm.twoPI) gyroDelta -= glm.twoPI;
                            if (gyroDelta < -glm.twoPI) gyroDelta += glm.twoPI;

                            //if the gyro and last corrected fix is < 10 degrees, super low pass for gps
                            if (Math.Abs(gyroDelta) < 0.18)
                            {
                                //a bit of delta and add to correction to current gyro
                                imuGPS_Offset += (gyroDelta * (0.1));
                                if (imuGPS_Offset > glm.twoPI) imuGPS_Offset -= glm.twoPI;
                                if (imuGPS_Offset < -glm.twoPI) imuGPS_Offset += glm.twoPI;
                            }
                            else
                            {
                                //a bit of delta and add to correction to current gyro
                                imuGPS_Offset += (gyroDelta * (0.2));
                                if (imuGPS_Offset > glm.twoPI) imuGPS_Offset -= glm.twoPI;
                                if (imuGPS_Offset < -glm.twoPI) imuGPS_Offset += glm.twoPI;
                            }

                            //determine the Corrected heading based on gyro and GPS
                            imuCorrected = correctionHeading + imuGPS_Offset;
                            if (imuCorrected > glm.twoPI) imuCorrected -= glm.twoPI;
                            if (imuCorrected < 0) imuCorrected += glm.twoPI;

                            fixHeading = imuCorrected;

                            camHeading = fixHeading;
                            if (camHeading > glm.twoPI) camHeading -= glm.twoPI;
                            camHeading = glm.toDegrees(camHeading);
                        }


                        #region Roll

                        if (ahrs.imuRoll != 88888)
                        {
                            //change for roll to the right is positive times -1
                            rollCorrectionDistance = Math.Sin(glm.toRadians((ahrs.imuRoll))) * -vehicle.VehicleConfig.AntennaHeight;
                            correctionDistanceGraph = rollCorrectionDistance;

                            // roll to left is positive  **** important!!
                            // not any more - April 30, 2019 - roll to right is positive Now! Still Important
                            pn.fix.easting = (Math.Cos(-fixHeading) * rollCorrectionDistance) + pn.fix.easting;
                            pn.fix.northing = (Math.Sin(-fixHeading) * rollCorrectionDistance) + pn.fix.northing;
                        }

                        #endregion Roll

                        TheRest();

                        //most recent fixes are now the prev ones
                        //jumpFix = prevFix;
                        //prevFix = pn.fix;

                        break;
                    }

                case "Dual":
                    {
                        if (Properties.Settings.Default.setGPS_headingFromWhichSource == "Dual" && ahrs.autoSwitchDualFixOn)
                        {
                            lblSpeed.ForeColor = System.Drawing.Color.Green;
                            isChangingDirection = false;
                        }

                        isFirstHeadingSet = true;
                        //use Dual Antenna heading for camera and tractor graphic
                        fixHeading = glm.toRadians(pn.headingTrueDual);
                        gpsHeading = fixHeading;

                        uncorrectedEastingGraph = pn.fix.easting;

                        if (vehicle.VehicleConfig.AntennaOffset != 0)
                        {
                            pn.fix.easting = (Math.Cos(-fixHeading) * vehicle.VehicleConfig.AntennaOffset) + pn.fix.easting;
                            pn.fix.northing = (Math.Sin(-fixHeading) * vehicle.VehicleConfig.AntennaOffset) + pn.fix.northing;
                        }

                        if (ahrs.imuRoll != 88888 && vehicle.VehicleConfig.AntennaHeight != 0)
                        {

                            //change for roll to the right is positive times -1
                            rollCorrectionDistance = Math.Sin(glm.toRadians((ahrs.imuRoll))) * -vehicle.VehicleConfig.AntennaHeight;
                            correctionDistanceGraph = rollCorrectionDistance;

                            pn.fix.easting = (Math.Cos(-gpsHeading) * rollCorrectionDistance) + pn.fix.easting;
                            pn.fix.northing = (Math.Sin(-gpsHeading) * rollCorrectionDistance) + pn.fix.northing;
                        }

                        //grab the most current fix and save the distance from the last fix
                        distanceCurrentStepFix = glm.Distance(pn.fix, prevDistFix);
                        //jumpFix = prevDistFix;
                        prevDistFix  = pn.fix;

                        //userDistance can be reset
                        distanceCurrentStepFixDisplay = distanceCurrentStepFix * 100;

                        distanceCurrentStepFix = glm.Distance(prevFix, pn.fix);

                        if (distanceCurrentStepFix > 0.1)
                        {
                            if ((fd.distanceUser += distanceCurrentStepFix) > 9999) fd.distanceUser = 0;
                            prevFix = pn.fix;
                        }

                        if (glm.Distance(lastReverseFix, pn.fix) > dualReverseDetectionDistance)
                        {
                            //most recent heading
                            double newHeading = Math.Atan2(pn.fix.easting - lastReverseFix.easting,
                                                        pn.fix.northing - lastReverseFix.northing);

                            if (newHeading < 0) newHeading += glm.twoPI;


                            //what is angle between the last reverse heading and current dual heading
                            double delta = Math.Abs(Math.PI - Math.Abs(Math.Abs(newHeading - fixHeading) - Math.PI));

                            //are we going backwards
                            isReverse = delta > 2 ? true : false;

                            //save for next meter check
                            lastReverseFix = pn.fix;
                        }

                        double camDelta = fixHeading - smoothCamHeading;

                        if (camDelta < 0) camDelta += glm.twoPI;
                        else if (camDelta > glm.twoPI) camDelta -= glm.twoPI;

                        //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                        if (camDelta >= -glm.PIBy2 && camDelta <= glm.PIBy2) camDelta *= -1.0;
                        else
                        {
                            if (camDelta > glm.PIBy2) { camDelta = glm.twoPI - camDelta; }
                            else { camDelta = (glm.twoPI + camDelta) * -1.0; }
                        }
                        if (camDelta > glm.twoPI) camDelta -= glm.twoPI;
                        else if (camDelta < -glm.twoPI) camDelta += glm.twoPI;

                        smoothCamHeading -= camDelta * camera.camSmoothFactor;

                        if (smoothCamHeading > glm.twoPI) smoothCamHeading -= glm.twoPI;
                        else if (smoothCamHeading < -glm.twoPI) smoothCamHeading += glm.twoPI;

                        camHeading = glm.toDegrees(smoothCamHeading);

                        // FOR AUTOSWITCH DUALFIX2FIX START
                        // save current fix and set as valid to make switch dual to fix2fix fluid
                        for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];
                        stepFixPts[0].easting = pn.fix.easting;
                        stepFixPts[0].northing = pn.fix.northing;
                        stepFixPts[0].isSet = 1;
                        // FOR AUTOSWITCH DUALFIX2FIX END

                        TheRest();

                        break;
                    }

                default:
                    break;
            }

            if (fixHeading >= glm.twoPI)
                fixHeading -= glm.twoPI;

            //vec2 ptA = new vec2(jumpFix.easting - (Math.Sin(gpsHeading) * 10), jumpFix.northing - (Math.Cos(gpsHeading) * 10));
            //vec2 ptB = new vec2(jumpFix.easting + (Math.Sin(gpsHeading) * 10), jumpFix.northing + (Math.Cos(gpsHeading) * 10));

            //double dx = ptB.easting - ptA.easting;
            ////z2-z1
            //double dy = ptB.northing - ptA.northing;

            ////how far from current AB Line is fix
            //jumpDistance = ((dy * pn.fix.easting) - (dx * pn.fix.northing) 
            //                + (ptB.easting * ptA.northing) - (ptB.northing * ptA.easting))
            //                / Math.Sqrt((dy * dy) + (dx * dx));

            //jumpDistance = Math.Abs(jumpDistance) * 100;

            //if (jumpDistance > jumpDistanceMax) jumpDistanceMax = jumpDistance;

            //if (jumpCounter++ > 200)
            //{
            //    jumpDistanceMax = jumpCounter = 0;
            //    lblJumpDistanceMax.Text = "*";
            //}

            //if (jumpDistance > 200) jumpDistance = 0;

            //if (isFirstHeadingSet && jumpDistanceAlarm > 0 && jumpDistance > jumpDistanceAlarm)
            //{
            //    Log.EventWriter(": " + jumpDistance.ToString("N0") + " cm");

            //    if (isBtnAutoSteerOn)
            //    {
            //        btnAutoSteer.PerformClick();
            //        TimedMessageBox(3000, gStr.gsAutoSteer, "Big Jump in GPS position:" + jumpDistance.ToString("N0") + " cm");
            //        Log.EventWriter("Autosteer Off, Jump in GPS position: " + jumpDistance.ToString("N0") + " cm");
            //    }

            //}

            //jumpFix.easting = pn.fix.easting;
            //jumpFix.northing = pn.fix.northing;

            #endregion

            #region Corrected Position
            Wgs84 latLon = pn.ConvertGeoCoordToWgs84(pn.fix.ToGeoCoord());
            byte[] correctedPosition = new byte[30];
            correctedPosition[0] = 0x80;
            correctedPosition[1] = 0x81;
            correctedPosition[2] = 0x7F;
            correctedPosition[3] = 0x64;
            correctedPosition[4] = 24;
            Buffer.BlockCopy(BitConverter.GetBytes(latLon.Longitude), 0, correctedPosition, 5, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(latLon.Latitude), 0, correctedPosition, 13, 8);
            Buffer.BlockCopy(BitConverter.GetBytes(glm.toDegrees(gpsHeading)), 0, correctedPosition, 21, 8);
            SendPgnToLoop(correctedPosition);
            #endregion

            #region AutoSteer

            //preset the values
            guidanceLineDistanceOff = 32000;

            if (ct.isContourBtnOn)
            {
                ct.DistanceFromContourLine(pivotAxlePos, steerAxlePos);
            }
            else
            {
                //auto track routine
                if (trk.isAutoTrack && !isBtnAutoSteerOn && trk.autoTrack3SecTimer >= 1)
                {
                    trk.autoTrack3SecTimer = 0;
                    int lastIndex = trk.idx;
                    trk.idx = trk.FindClosestRefTrack(steerAxlePos);
                    if (lastIndex != trk.idx)
                    {
                        curve.isCurveValid = false;
                        ABLine.isABValid = false;
                    }
                }

                //like normal
                if (trk.gArr.Count > 0 && trk.idx > -1)
                {
                    if (trk.gArr[trk.idx].mode == TrackMode.AB)
                    {
                        ABLine.BuildCurrentABLineList(pivotAxlePos);

                        ABLine.GetCurrentABLine(pivotAxlePos, steerAxlePos);
                    }
                    else
                    {
                        //build new current ref line if required
                        curve.BuildCurveCurrentList(pivotAxlePos);

                        curve.GetCurrentCurveLine(pivotAxlePos, steerAxlePos);
                    }
                }
            }

            // autosteer at full speed of updates

            //if the whole path driving driving process is green
            if (recPath.isDrivingRecordedPath) recPath.UpdatePosition();

            // If Drive button off - normal autosteer 
            if (!vehicle.isInFreeDriveMode)
            {
                //fill up0 the appropriate arrays with new values
                p_254.pgn[p_254.speedHi] = unchecked((byte)((int)(Math.Abs(avgSpeed) * 10.0) >> 8));
                p_254.pgn[p_254.speedLo] = unchecked((byte)((int)(Math.Abs(avgSpeed) * 10.0)));
                //mc.machineControlData[mc.cnSpeed] = mc.autoSteerData[mc.sdSpeed];

                //save distance for display
                lightbarDistance = guidanceLineDistanceOff;

                if (!isBtnAutoSteerOn) //32020 means auto steer is off
                {
                    guidanceLineDistanceOff = 32020;
                    p_254.pgn[p_254.status] = 0;
                }

                else p_254.pgn[p_254.status] = 1;

                if (recPath.isDrivingRecordedPath || recPath.isFollowingDubinsToPath) p_254.pgn[p_254.status] = 1;

                //mc.autoSteerData[7] = unchecked((byte)(guidanceLineDistanceOff >> 8));
                //mc.autoSteerData[8] = unchecked((byte)(guidanceLineDistanceOff));

                //convert to cm from mm and divide by 2 - lightbar
                int distanceX2;
                if (guidanceLineDistanceOff == 32020 || guidanceLineDistanceOff == 32000)
                    distanceX2 = 255;

                else
                {
                    distanceX2 = (int)(guidanceLineDistanceOff * 0.05);

                    if (distanceX2 < -127) distanceX2 = -127;
                    else if (distanceX2 > 127) distanceX2 = 127;
                    distanceX2 += 127;
                }

                p_254.pgn[p_254.lineDistance] = unchecked((byte)distanceX2);

                if (!timerSim.Enabled)
                {
                    if (isBtnAutoSteerOn && avgSpeed > vehicle.maxSteerSpeed)
                    {
                        btnAutoSteer.PerformClick();
                    }

                    if (isBtnAutoSteerOn && avgSpeed < vehicle.minSteerSpeed)
                    {
                        minSteerSpeedTimer++;
                        if (minSteerSpeedTimer > 80)
                        {
                            btnAutoSteer.PerformClick();
                            if (isMetric)
                                TimedMessageBox(3000, "AutoSteer Disabled", "Below Minimum Safe Steering Speed: " + vehicle.minSteerSpeed.ToString("N0") + " Kmh");
                            else
                                TimedMessageBox(3000, "AutoSteer Disabled", "Below Minimum Safe Steering Speed: " + (vehicle.minSteerSpeed * 0.621371).ToString("N1") + " MPH");
                            
                            Log.EventWriter("Steer Off, Below Min Steering Speed");
                        }
                    }
                    else
                    {
                        minSteerSpeedTimer = 0;
                    }
                }

                //double tanSteerAngle = Math.Tan(glm.toRadians(((double)(guidanceLineSteerAngle)) * 0.01));
                //double tanActSteerAngle = Math.Tan(glm.toRadians(mc.actualSteerAngleDegrees));

                //setAngVel = 0.277777 * avgSpeed * tanSteerAngle / vehicle.wheelbase;
                //actAngVel = glm.toDegrees(0.277777 * avgSpeed * tanActSteerAngle / vehicle.wheelbase);


                //isMaxAngularVelocity = false;
                ////greater then settings rads/sec limit steer angle
                //if (Math.Abs(setAngVel) > vehicle.maxAngularVelocity)
                //{
                //    setAngVel = vehicle.maxAngularVelocity;
                //    tanSteerAngle = 3.6 * setAngVel * vehicle.wheelbase / avgSpeed;
                //    if (guidanceLineSteerAngle < 0)
                //        guidanceLineSteerAngle = (short)(glm.toDegrees(Math.Atan(tanSteerAngle)) * -100);
                //    else
                //        guidanceLineSteerAngle = (short)(glm.toDegrees(Math.Atan(tanSteerAngle)) * 100);
                //    isMaxAngularVelocity = true;
                //}

                //setAngVel = glm.toDegrees(setAngVel);

                if (isChangingDirection && ahrs.imuHeading == 99999)
                    p_254.pgn[p_254.status] = 0;

                //for now if backing up, turn off autosteer
                if (!isSteerInReverse)
                {
                    if (isReverse) p_254.pgn[p_254.status] = 0;
                }                

                // delay on dead zone.
                if (p_254.pgn[p_254.status] == 1 && !isReverse
                    && Math.Abs(guidanceLineSteerAngle - mc.actualSteerAngleDegrees*100) < vehicle.deadZoneHeading)
                {
                    if (vehicle.deadZoneDelayCounter > vehicle.deadZoneDelay)
                    {
                        vehicle.isInDeadZone = true;
                    }
                }
                else
                {
                    vehicle.deadZoneDelayCounter = 0;
                    vehicle.isInDeadZone = false;
                }

                 if (!vehicle.isInDeadZone)
                {
                    p_254.pgn[p_254.steerAngleHi] = unchecked((byte)(guidanceLineSteerAngle >> 8));
                    p_254.pgn[p_254.steerAngleLo] = unchecked((byte)(guidanceLineSteerAngle));
                }
            }

            else //Drive button is on
            {
                //fill up the auto steer array with free drive values
                p_254.pgn[p_254.speedHi] = unchecked((byte)((int)(80) >> 8));
                p_254.pgn[p_254.speedLo] = unchecked((byte)((int)(80)));

                //turn on status to operate
                p_254.pgn[p_254.status] = 1;

                //send the steer angle
                guidanceLineSteerAngle = (Int16)(vehicle.driveFreeSteerAngle * 100);

                p_254.pgn[p_254.steerAngleHi] = unchecked((byte)(guidanceLineSteerAngle >> 8));
                p_254.pgn[p_254.steerAngleLo] = unchecked((byte)(guidanceLineSteerAngle));


            }

            //out serial to autosteer module  //indivdual classes load the distance and heading deltas 
            SendPgnToLoop(p_254.pgn);

            //for average cross track error
            if (guidanceLineDistanceOff < 29000)
            {
                crossTrackError = (int)((double)crossTrackError * 0.90 + Math.Abs((double)guidanceLineDistanceOff) * 0.1);
            }
            else
            {
                crossTrackError = 0;
            }

            #endregion

            #region Youturn

            //if an outer boundary is set, then apply critical stop logic
            if (bnd.bndList.Count > 0)
            {
                //check if inside all fence
                if (!yt.isYouTurnBtnOn)
                {
                    mc.isOutOfBounds = !bnd.IsPointInsideFenceArea(pivotAxlePos);
                }
                else //Youturn is on
                {
                    bool isInTurnBounds = bnd.IsPointInsideTurnArea(pivotAxlePos) != -1;
                    //Are we inside outer and outside inner all turn boundaries, no turn creation problems
                    //if we are too much off track > 1.3m, kill the diagnostic creation, start again
                    //if (!yt.isYouTurnTriggered) 
                    if (isInTurnBounds)
                    {
                        mc.isOutOfBounds = false;
                        //now check to make sure we are not in an inner turn boundary - drive thru is ok
                        if (yt.youTurnPhase != 10)
                        {
                            if (crossTrackError > 1000)
                            {
                                yt.ResetCreatedYouTurn();
                            }
                            else
                            {
                                if (trk.gArr[trk.idx].mode == TrackMode.AB)
                                {
                                    yt.BuildABLineDubinsYouTurn();
                                }
                                else yt.BuildCurveDubinsYouTurn();
                            }

                            if (yt.uTurnStyle == 0 && yt.youTurnPhase == 10)
                            {
                                yt.SmoothYouTurn(6);
                            }

                            if (yt.isTurnCreationTooClose && !yt.turnTooCloseTrigger)
                            {
                                yt.turnTooCloseTrigger = true;
                                if (sounds.isTurnSoundOn)
                                {
                                    sounds.sndUTurnTooClose.Play();
                                    Log.EventWriter("U Turn Creation Failure");
                                }
                            }
                        }
                        else if (yt.ytList.Count > 5)//wait to trigger the actual turn since its made and waiting
                        {
                            //distance from current pivot to first point of youturn pattern
                            distancePivotToTurnLine = glm.Distance(yt.ytList[2], pivotAxlePos);

                            if ((distancePivotToTurnLine <= 20.0) && (distancePivotToTurnLine >= 18.0) && !yt.isYouTurnTriggered)

                                if (!sounds.isBoundAlarming)
                                {
                                    if (sounds.isTurnSoundOn) sounds.sndBoundaryAlarm.Play();
                                    sounds.isBoundAlarming = true;
                                }

                            //if we are close enough to pattern, trigger.
                            if ((distancePivotToTurnLine <= 1.0) && (distancePivotToTurnLine >= 0) && !yt.isYouTurnTriggered)
                            {
                                yt.YouTurnTrigger();
                                sounds.isBoundAlarming = false;
                            }

                            //if (isBtnAutoSteerOn && guidanceLineDistanceOff > 300 && !yt.isYouTurnTriggered)
                            //{
                            //    yt.ResetCreatedYouTurn();
                            //}
                        }
                    }
                    else
                    {
                        if (!yt.isYouTurnTriggered)
                        {
                            yt.ResetCreatedYouTurn();
                            mc.isOutOfBounds = !bnd.IsPointInsideFenceArea(pivotAxlePos);
                        }

                    }

                    //}
                    //// here is stop logic for out of bounds - in an inner or out the outer turn border.
                    //else
                    //{
                    //    //mc.isOutOfBounds = true;
                    //    if (isBtnAutoSteerOn)
                    //    {
                    //        if (yt.isYouTurnBtnOn)
                    //        {
                    //            yt.ResetCreatedYouTurn();
                    //            //sim.stepDistance = 0 / 17.86;
                    //        }
                    //    }
                    //    else
                    //    {
                    //        yt.isTurnCreationTooClose = false;
                    //    }

                    //}
                }
            }
            else
            {
                mc.isOutOfBounds = false;
            }

            #endregion



            //update main window
            oglMain.MakeCurrent();
            oglMain.Refresh();

            //end of UppdateFixPosition

            //stop the timer and calc how long it took to do calcs and draw
            frameTimeRough = (double)(swFrame.ElapsedTicks * 1000) / (double)System.Diagnostics.Stopwatch.Frequency;

            if (frameTimeRough > 80) frameTimeRough = 80;
            frameTime = frameTime * 0.90 + frameTimeRough * 0.1;
        }

        private void TheRest()
        {
            //positions and headings 
            CalculatePositionHeading();

            //calculate lookahead at full speed, no sentence misses
            CalculateSectionLookAhead(toolPos.northing, toolPos.easting, cosSectionHeading, sinSectionHeading);

            //To prevent drawing high numbers of triangles, determine and test before drawing vertex
            sectionTriggerDistance = glm.Distance(pn.fix, prevSectionPos);
            contourTriggerDistance = glm.Distance(pn.fix, prevContourPos);
            gridTriggerDistance = glm.DistanceSquared(pn.fix, prevGridPos);

            if ( isLogElevation && gridTriggerDistance > 2.9 && patchCounter !=0 && isJobStarted)
            {
                //grab fix and elevation
                sbGrid.Append(
                      pn.latitude.ToString("N7", CultureInfo.InvariantCulture) + ","
                    + pn.longitude.ToString("N7", CultureInfo.InvariantCulture) + ","
                    + Math.Round((pn.altitude - vehicle.VehicleConfig.AntennaHeight),3).ToString(CultureInfo.InvariantCulture) + ","
                    + pn.fixQuality.ToString(CultureInfo.InvariantCulture) + ","
                    + pn.fix.easting.ToString("N2", CultureInfo.InvariantCulture) + ","
                    + pn.fix.northing.ToString("N2", CultureInfo.InvariantCulture) + ","
                    + pivotAxlePos.heading.ToString("N3", CultureInfo.InvariantCulture) + ","
                    + Math.Round(ahrs.imuRoll,3).ToString(CultureInfo.InvariantCulture) + 
                    "\r\n");

                prevGridPos.easting = pivotAxlePos.easting;
                prevGridPos.northing = pivotAxlePos.northing;
            }

            //contour points
            if (isJobStarted &&(contourTriggerDistance > tool.contourWidth 
                || contourTriggerDistance > sectionTriggerStepDistance))
            {
                AddContourPoints();
            }

            //section on off and points
            if (sectionTriggerDistance > sectionTriggerStepDistance && isJobStarted)
            {
                AddSectionOrPathPoints();
            }

            //test if travelled far enough for new boundary point
            if (bnd.isOkToAddPoints)
            {
                double boundaryDistance = glm.Distance(pn.fix, prevBoundaryPos);
                if (boundaryDistance > 1) AddBoundaryPoint();
            }

            //calc distance travelled since last GPS fix
            //distance = glm.Distance(pn.fix, prevFix);
            //if (avgSpeed > 1)

            //if ((avgSpeed - previousSpeed  ) < -vehicle.panicStopSpeed && vehicle.panicStopSpeed != 0)
            //{
            //    if (isBtnAutoSteerOn)
            //    {
            //       btnAutoSteer.PerformClick();
            //        TimedMessageBox(2000, gStr.gsGuidanceStopped, "Panic Stop");
            //        Log.EventWriter("Steer Off, Panic Stop Exceeded");
            //    }
            //}

            //previousSpeed = avgSpeed;   
        }

        //all the hitch, pivot, section, trailing hitch, headings and fixes
        private void CalculatePositionHeading()
        {
            #region pivot hitch trail

            //translate from pivot position to steer axle and pivot axle position
            //translate world to the pivot axle
            pivotAxlePos.easting = pn.fix.easting - (Math.Sin(fixHeading) * vehicle.VehicleConfig.AntennaPivot);
            pivotAxlePos.northing = pn.fix.northing - (Math.Cos(fixHeading) * vehicle.VehicleConfig.AntennaPivot);
            pivotAxlePos.heading = fixHeading;

            steerAxlePos.easting = pivotAxlePos.easting + (Math.Sin(fixHeading) * vehicle.VehicleConfig.Wheelbase);
            steerAxlePos.northing = pivotAxlePos.northing + (Math.Cos(fixHeading) * vehicle.VehicleConfig.Wheelbase);
            steerAxlePos.heading = fixHeading;

            //guidance look ahead distance based on time or tool width at least 
            
            double guidanceLookDist = (Math.Max(tool.width * 0.5, avgSpeed * 0.277777 * guidanceLookAheadTime));
            guidanceLookPos.easting = pivotAxlePos.easting + (Math.Sin(fixHeading) * guidanceLookDist);
            guidanceLookPos.northing = pivotAxlePos.northing + (Math.Cos(fixHeading) * guidanceLookDist);
            

            //determine where the rigid vehicle hitch ends
            hitchPos.easting = pn.fix.easting + (Math.Sin(fixHeading) * (tool.hitchLength - vehicle.VehicleConfig.AntennaPivot));
            hitchPos.northing = pn.fix.northing + (Math.Cos(fixHeading) * (tool.hitchLength - vehicle.VehicleConfig.AntennaPivot));

            //tool attached via a trailing hitch
            if (tool.isToolTrailing)
            {
                double over;
                if (tool.isToolTBT)
                {
                    //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                    if (distanceCurrentStepFix != 0)
                    {
                        tankPos.heading = Math.Atan2(hitchPos.easting - tankPos.easting, hitchPos.northing - tankPos.northing);
                        if (tankPos.heading < 0) tankPos.heading += glm.twoPI;
                    }

                    ////the tool is seriously jacknifed or just starting out so just spring it back.
                    over = Math.Abs(Math.PI - Math.Abs(Math.Abs(tankPos.heading - fixHeading) - Math.PI));

                    if (over < 2.0 && startCounter > 50)
                    {
                        tankPos.easting = hitchPos.easting + (Math.Sin(tankPos.heading) * (tool.tankTrailingHitchLength));
                        tankPos.northing = hitchPos.northing + (Math.Cos(tankPos.heading) * (tool.tankTrailingHitchLength));
                    }

                    //criteria for a forced reset to put tool directly behind vehicle
                    if (over > 2.0 | startCounter < 51)
                    {
                        tankPos.heading = fixHeading;
                        tankPos.easting = hitchPos.easting + (Math.Sin(tankPos.heading) * (tool.tankTrailingHitchLength));
                        tankPos.northing = hitchPos.northing + (Math.Cos(tankPos.heading) * (tool.tankTrailingHitchLength));
                    }
                }
                else
                {
                    tankPos.heading = fixHeading;
                    tankPos.easting = hitchPos.easting;
                    tankPos.northing = hitchPos.northing;
                }

                //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                if (distanceCurrentStepFix != 0)
                {
                    toolPivotPos.heading = Math.Atan2(tankPos.easting - toolPivotPos.easting, tankPos.northing - toolPivotPos.northing);
                    if (toolPivotPos.heading < 0) toolPivotPos.heading += glm.twoPI;
                }

                ////the tool is seriously jacknifed or just starting out so just spring it back.
                over = Math.Abs(Math.PI - Math.Abs(Math.Abs(toolPivotPos.heading - tankPos.heading) - Math.PI));

                if (over < 1.9 && startCounter > 50)
                {
                    toolPivotPos.easting = tankPos.easting + (Math.Sin(toolPivotPos.heading) * (tool.trailingHitchLength));
                    toolPivotPos.northing = tankPos.northing + (Math.Cos(toolPivotPos.heading) * (tool.trailingHitchLength));
                }

                //criteria for a forced reset to put tool directly behind vehicle
                if (over > 1.9 | startCounter < 51)
                {
                    toolPivotPos.heading = tankPos.heading;
                    toolPivotPos.easting = tankPos.easting + (Math.Sin(toolPivotPos.heading) * (tool.trailingHitchLength));
                    toolPivotPos.northing = tankPos.northing + (Math.Cos(toolPivotPos.heading) * (tool.trailingHitchLength));
                }

                toolPos.heading = toolPivotPos.heading;
                toolPos.easting = tankPos.easting + 
                    (Math.Sin(toolPivotPos.heading) * (tool.trailingHitchLength - tool.trailingToolToPivotLength));
                toolPos.northing = tankPos.northing + 
                    (Math.Cos(toolPivotPos.heading) * (tool.trailingHitchLength - tool.trailingToolToPivotLength));
            }

            //rigidly connected to vehicle
            else
            {
                toolPivotPos.heading = fixHeading;
                toolPivotPos.easting = hitchPos.easting;
                toolPivotPos.northing = hitchPos.northing;

                toolPos.heading = fixHeading;
                toolPos.easting = hitchPos.easting;
                toolPos.northing = hitchPos.northing;
            }

            #endregion

            //used to increase triangle countExit when going around corners, less on straight
            //pick the slow moving side edge of tool
            double distance = tool.width*0.75;
            if (distance > 8) distance = 8;

            //whichever is less
            if (tool.farLeftSpeed < tool.farRightSpeed)
            {
                double twist = tool.farLeftSpeed * (tool.width / 50) / tool.farRightSpeed * (50/ tool.width);
                twist *= twist;
                if (twist < 0.2) twist = 0.2;
                sectionTriggerStepDistance = distance * twist;
            }
            else
            {
                double twist = tool.farRightSpeed * (tool.width / 50) / tool.farLeftSpeed * (50 / tool.width);
                twist *= twist;
                if (twist < 0.2) twist = 0.2;

                sectionTriggerStepDistance = distance * twist;
            }

            if (sectionTriggerStepDistance < 1) sectionTriggerStepDistance = 1;
            //if (sectionTriggerStepDistance > 5) sectionTriggerStepDistance = 5;

            //finally fixed distance for making a curve line
            if (curve.isRecordingCurve) sectionTriggerStepDistance *= 0.5;

            //precalc the sin and cos of heading * -1
            sinSectionHeading = Math.Sin(-toolPivotPos.heading);
            cosSectionHeading = Math.Cos(-toolPivotPos.heading);
        }

        //calculate the extreme tool left, right velocities, each section lookahead, and whether or not its going backwards
        public void CalculateSectionLookAhead(double northing, double easting, double cosHeading, double sinHeading)
        {
            //calculate left side of section 1
            vec2 left = new vec2();
            vec2 right = left;
            double leftSpeed = 0, rightSpeed = 0;

            //speed max for section kmh*0.277 to m/s * 10 cm per pixel * 1.7 max speed
            double meterPerSecPerPixel = Math.Abs(avgSpeed) * 4.5;

            //now loop all the section rights and the one extreme left
            for (int j = 0; j < tool.numOfSections; j++)
            {
                if (j == 0)
                {
                    //only one first left point, the rest are all rights moved over to left
                    section[j].leftPoint = new vec2(cosHeading * (section[j].positionLeft) + easting,
                                       sinHeading * (section[j].positionLeft) + northing);

                    left = section[j].leftPoint - section[j].lastLeftPoint;

                    //save a copy for next time
                    section[j].lastLeftPoint = section[j].leftPoint;

                    //get the speed for left side only once

                    leftSpeed = left.GetLength() * gpsHz * 10;
                    if (leftSpeed > meterPerSecPerPixel) leftSpeed = meterPerSecPerPixel;
                }
                else
                {
                    //right point from last section becomes this left one
                    section[j].leftPoint = section[j - 1].rightPoint;
                    left = section[j].leftPoint - section[j].lastLeftPoint;

                    //save a copy for next time
                    section[j].lastLeftPoint = section[j].leftPoint;
                    
                    //Save the slower of the 2
                    if (leftSpeed > rightSpeed) leftSpeed = rightSpeed;                    
                }

                section[j].rightPoint = new vec2(cosHeading * (section[j].positionRight) + easting,
                                    sinHeading * (section[j].positionRight) + northing);

                //now we have left and right for this section
                right = section[j].rightPoint - section[j].lastRightPoint;

                //save a copy for next time
                section[j].lastRightPoint = section[j].rightPoint;

                //grab vector length and convert to meters/sec/10 pixels per meter                
                rightSpeed = right.GetLength() * gpsHz * 10;
                if (rightSpeed > meterPerSecPerPixel) rightSpeed = meterPerSecPerPixel;

                //Is section outer going forward or backward
                double head = left.HeadingXZ();

                if (head < 0) head += glm.twoPI;

                if (Math.PI - Math.Abs(Math.Abs(head - toolPivotPos.heading) - Math.PI) > glm.PIBy2)
                {
                    if (leftSpeed > 0) leftSpeed *= -1;
                }

                head = right.HeadingXZ();
                if (head < 0) head += glm.twoPI;
                if (Math.PI - Math.Abs(Math.Abs(head - toolPivotPos.heading) - Math.PI) > glm.PIBy2)
                {
                    if (rightSpeed > 0) rightSpeed *= -1;
                }

                double sped = 0;
                //save the far left and right speed in m/sec averaged over 20%
                if (j==0)
                {
                    sped = (leftSpeed * 0.1);
                    if (sped < 0.1) sped = 0.1;
                    tool.farLeftSpeed = tool.farLeftSpeed * 0.7 + sped * 0.3;
                }
                if (j == tool.numOfSections - 1)
                {
                    sped = (rightSpeed * 0.1);
                    if (sped < 0.1) sped = 0.1;
                    tool.farRightSpeed = tool.farRightSpeed * 0.7 + sped * 0.3;
                }

                //choose fastest speed
                if (leftSpeed > rightSpeed)
                {
                    sped = leftSpeed;
                    leftSpeed = rightSpeed;
                }
                else sped = rightSpeed;
                section[j].speedPixels = section[j].speedPixels * 0.7 + sped * 0.3;
            }
        }

        //perimeter and boundary point generation
        public void AddBoundaryPoint()
        {
            //save the north & east as previous
            prevBoundaryPos.easting = pn.fix.easting;
            prevBoundaryPos.northing = pn.fix.northing;

            //build the boundary line

            if (bnd.isOkToAddPoints && (!bnd.isRecBoundaryWhenSectionOn || 
                (bnd.isRecBoundaryWhenSectionOn && (manualBtnState == btnStates.On || autoBtnState == btnStates.Auto))))
            {
                if (bnd.isDrawAtPivot)
                {
                    if (bnd.isDrawRightSide)
                    {
                        //Right side
                        vec3 point = new vec3(
                            pivotAxlePos.easting + (Math.Sin(pivotAxlePos.heading - glm.PIBy2) * -bnd.createBndOffset),
                            pivotAxlePos.northing + (Math.Cos(pivotAxlePos.heading - glm.PIBy2) * -bnd.createBndOffset),
                            pivotAxlePos.heading);
                        bnd.bndBeingMadePts.Add(point);
                    }

                    //draw on left side
                    else
                    {
                        //Right side
                        vec3 point = new vec3(
                            pivotAxlePos.easting + (Math.Sin(pivotAxlePos.heading - glm.PIBy2) * bnd.createBndOffset),
                            pivotAxlePos.northing + (Math.Cos(pivotAxlePos.heading - glm.PIBy2) * bnd.createBndOffset),
                            pivotAxlePos.heading);
                        bnd.bndBeingMadePts.Add(point);
                    }
                }
                else
                {
                    //draw at tool
                    if (bnd.isDrawRightSide)
                    {
                        //Right side
                        vec3 point = new vec3(section[tool.numOfSections-1].rightPoint.easting, section[tool.numOfSections - 1].rightPoint.northing, 0);
                        bnd.bndBeingMadePts.Add(point);
                    }

                    //draw on left side
                    else
                    {
                        //Right side
                        vec3 point = new vec3(section[0].leftPoint.easting, section[0].leftPoint.northing, 0);
                        bnd.bndBeingMadePts.Add(point);
                    }
                }
            }
        }

        private void AddContourPoints()
        {
            //if (isConstantContourOn)
            {
                //record contour all the time
                //Contour Base Track.... At least One section on, turn on if not
                if (patchCounter != 0)
                {
                    //keep the line going, everything is on for recording path
                    if (ct.isContourOn) ct.AddPoint(pivotAxlePos);
                    else
                    {
                        ct.StartContourLine();
                        ct.AddPoint(pivotAxlePos);
                    }
                }

                //All sections OFF so if on, turn off
                else
                {
                    if (ct.isContourOn)
                    { ct.StopContourLine(); }
                }

                //Build contour line if close enough to a patch
                if (ct.isContourBtnOn) ct.BuildContourGuidanceLine(pivotAxlePos);
            }
            //else
            //{
            //    if ((ABLine.isBtnABLineOn && !ct.isContourBtnOn && ABLine.isABLineSet && isBtnAutoSteerOn) ||
            //                (!ct.isContourBtnOn && curve.isBtnTrackOn && curve.isCurveSet && isBtnAutoSteerOn))
            //    {
            //        //no contour recorded
            //        if (ct.isContourOn) { ct.StopContourLine(); }
            //    }
            //    else
            //    {
            //        //Contour Base Track.... At least One section on, turn on if not
            //        if (patchCounter != 0)
            //        {
            //            //keep the line going, everything is on for recording path
            //            if (ct.isContourOn) ct.AddPoint(pivotAxlePos);
            //            else
            //            {
            //                ct.StartContourLine();
            //                ct.AddPoint(pivotAxlePos);
            //            }
            //        }

            //        //All sections OFF so if on, turn off
            //        else { if (ct.isContourOn) { ct.StopContourLine(); } }

            //        //Build contour line if close enough to a patch
            //        if (ct.isContourBtnOn) ct.BuildContourGuidanceLine(pivotAxlePos);
            //    }
            //}

            //save the north & east as previous
            prevContourPos.northing = pivotAxlePos.northing;
            prevContourPos.easting = pivotAxlePos.easting;
        }

        //add the points for section, contour line points, Area Calc feature
        private void AddSectionOrPathPoints()
        {
            if (recPath.isRecordOn)
            {
                //keep minimum speed of 1.0
                double speed = avgSpeed;
                if (avgSpeed < 1.0) speed = 1.0;
                bool autoBtn = (autoBtnState == btnStates.Auto);

                recPath.recList.Add(new CRecPathPt(pivotAxlePos.easting, pivotAxlePos.northing, pivotAxlePos.heading, speed, autoBtn));
            }

            if (curve.isRecordingCurve)
            {
                curve.desList.Add(new vec3(pivotAxlePos.easting, pivotAxlePos.northing, pivotAxlePos.heading));
            }

            //save the north & east as previous
            prevSectionPos.northing = pn.fix.northing;
            prevSectionPos.easting = pn.fix.easting;

            // if non zero, at least one section is on.
            patchCounter = 0;

            //send the current and previous GPS fore/aft corrected fix to each section
            for (int j = 0; j < triStrip.Count; j++)
            {
                if (triStrip[j].isDrawing)
                {
                    if (isPatchesChangingColor)
                    {
                        triStrip[j].numTriangles = 64;
                        isPatchesChangingColor = false;
                    }

                    triStrip[j].AddMappingPoint(j);
                    patchCounter++;
                }
            }
        }

        //the start of first few frames to initialize entire program
        private void InitializeFirstFewGPSPositions()
        {
            if (!isFirstFixPositionSet)
            {
                if (!isJobStarted)
                {
                    pn.latStart = pn.latitude;
                    pn.lonStart = pn.longitude;
                    pn.SetLocalMetersPerDegree(false);
                }

                GeoCoord fixCoord = pn.ConvertWgs84ToGeoCoord(new Wgs84(pn.latitude, pn.longitude));
                pn.fix.northing = fixCoord.Northing;
                pn.fix.easting = fixCoord.Easting;
                //Draw a grid once we know where in the world we are.
                isFirstFixPositionSet = true;

                //most recent fixes
                prevFix = pn.fix;

                //run once and return
                isFirstFixPositionSet = true;

                return;
            }

            else
            {
                prevFix.easting = pn.fix.easting; prevFix.northing = pn.fix.northing;

                //keep here till valid data
                if (startCounter > (20))
                {
                    isGPSPositionInitialized = true;
                    lastReverseFix = pn.fix;
                }

                //in radians
                fixHeading = 0;
                toolPivotPos.heading = fixHeading;

                //send out initial zero settings
                if (isGPSPositionInitialized)
                {
                    //set display accordingly
                    isDayTime = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);

                    SetZoom();
                }
                return;
            }
        }
    }//end class
}//end namespace
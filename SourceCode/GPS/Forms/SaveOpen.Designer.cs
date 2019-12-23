﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Drawing;

namespace AgOpenGPS
{

    public partial class FormGPS
    {
        //public void FileAppendCurveLine(string curveName)
        //{
        //    //get the directory and make sure it exists, create if not
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";
        //    string directoryName = Path.GetDirectoryName(dirField);

        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    string filename = directoryName + "\\CurveLines.txt";

        //    //use Streamwriter to create and append to existing curveLines file
        //    using (StreamWriter writer = new StreamWriter(filename, true))
        //    {
        //        try
        //        {
        //            if (curve.refList.Count > 0)
        //            {
        //                if (curveName.Length > 0)
        //                {
        //                    curve.curveArr.Add(new CCurveLines());
        //                    curve.curveArr[curve.curveArr.Count - 1].Name = curveName;
        //                    curve.curveArr[curve.curveArr.Count - 1].aveHeading = curve.aveLineHeading;

        //                    //write out the ABLine
        //                    writer.WriteLine(curveName);

        //                    //write out the aveheading
        //                    writer.WriteLine(curve.aveLineHeading.ToString(CultureInfo.InvariantCulture));

        //                    //write out the points of ref line
        //                    writer.WriteLine(curve.refList.Count.ToString(CultureInfo.InvariantCulture));

        //                    for (int j = 0; j < curve.refList.Count; j++)
        //                    {
        //                        curve.curveArr[curve.curveArr.Count - 1].curvePts.Add(curve.refList[j]);
        //                        writer.WriteLine(Math.Round(curve.refList[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                                Math.Round(curve.refList[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                                    Math.Round(curve.refList[j].heading, 5).ToString(CultureInfo.InvariantCulture));
        //                    }
        //                }
        //                else
        //                {
        //                    //MessageBox.Show("Currently no ABCurve name\n      create ABCurve name");
        //                    var form2 = new FormTimedMessage(2000, gStr.gsNoNameEntered, gStr.gsEnterUniqueABCurveName);
        //                    form2.Show();
        //                }
        //            }
        //            else
        //            {
        //                var form2 = new FormTimedMessage(2000, gStr.gsNoABCurveCreated, gStr.gsCompleteAnABCurveLineFirst);
        //                form2.Show();
        //            }
        //        }
        //        catch (Exception er)
        //        {
        //            WriteErrorLog("Saving Curve Line" + er.ToString());

        //            return;
        //        }
        //    }

        //    if (curve.numCurveLines == 0) curve.numCurveLineSelected = 0;
        //    if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = curve.numCurveLines;

        //}
        //public void FileAppendABLine()
        //{
        //    //make sure at least a global blank AB Line file exists
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";
        //    string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    string filename = directoryName + "\\ABLines.txt";

        //    ABLine.numABLines = ABLine.lineArr.Count;

        //    if (ABLine.numABLines > 0)
        //    {
        //        int idx = ABLine.numABLines - 1;

        //        using (StreamWriter writer = new StreamWriter(filename, true))
        //        {
        //            try
        //            {

        //                //make it culture invariant
        //                string line = ABLine.lineArr[idx].Name.Trim()
        //                    + ',' + (Math.Round(glm.toDegrees(ABLine.lineArr[idx].heading), 8)).ToString(CultureInfo.InvariantCulture)
        //                    + ',' + (Math.Round(ABLine.lineArr[idx].origin.easting, 3)).ToString(CultureInfo.InvariantCulture)
        //                    + ',' + (Math.Round(ABLine.lineArr[idx].origin.northing, 3)).ToString(CultureInfo.InvariantCulture);

        //                //write out to file
        //                writer.WriteLine(line);
        //            }
        //            catch (Exception er)
        //            {
        //                Console.WriteLine(er.Message + "\n Cannot write to file.");
        //                WriteErrorLog("Saving ABLines Draw append" + er.ToString());
        //                return;
        //            }
        //        }
        //    }

        //    if (ABLine.numABLines == 0) ABLine.numABLines = 0;
        //    if (ABLine.numABLineSelected > ABLine.numABLines) ABLine.numABLineSelected = ABLine.numABLines;
        //}

        //list of the list of patch data individual triangles for field sections
        public List<List<vec2>> patchSaveList = new List<List<vec2>>();


        //list of the list of patch data individual triangles for contour tracking
        public List<List<vec3>> contourSaveList = new List<List<vec3>>();

        public void FileSaveCurveLines()
        {
            curve.moveDistance = 0;

            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            int cnt = curve.curveArr.Count;
            curve.numCurveLines = cnt;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                try
                {
                    if (cnt > 0)
                    {
                        writer.WriteLine("$CurveLines");

                        for (int i = 0; i < cnt; i++)
                        {
                            //write out the Name
                            writer.WriteLine(curve.curveArr[i].Name);

                            //write out the aveheading
                            writer.WriteLine(curve.curveArr[i].aveHeading.ToString(CultureInfo.InvariantCulture));

                            //write out the points of ref line
                            int cnt2 = curve.curveArr[i].curvePts.Count;

                            writer.WriteLine(cnt2.ToString(CultureInfo.InvariantCulture));
                            if (curve.curveArr[i].curvePts.Count > 0)
                            {
                                for (int j = 0; j < cnt2; j++)
                                    writer.WriteLine(Math.Round(curve.curveArr[i].curvePts[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                        Math.Round(curve.curveArr[i].curvePts[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                            Math.Round(curve.curveArr[i].curvePts[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                            }
                        }
                    }
                    else
                    {
                        writer.WriteLine("$CurveLines");
                    }
                }
                catch (Exception er)
                {
                    WriteErrorLog("Saving Curve Line" + er.ToString());

                    return;
                }
            }

            if (curve.numCurveLines == 0) curve.numCurveLineSelected = 0;
            if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = curve.numCurveLines;

        }

        public void FileLoadCurveLines()
        {
            curve.moveDistance = 0;

            curve.curveArr?.Clear();
            curve.numCurveLines = 0;

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\CurveLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                    writer.WriteLine("$CurveLines");
                }
            }

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.gsFileError, gStr.gsMissingABCurveFile);
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;

                        //read header $CurveLine
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            curve.curveArr.Add(new CCurveLines());

                            //read header $CurveLine
                            curve.curveArr[curve.numCurveLines].Name = reader.ReadLine();
                            // get the average heading
                            line = reader.ReadLine();
                            curve.curveArr[curve.numCurveLines].aveHeading = double.Parse(line, CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            int numPoints = int.Parse(line);

                            if (numPoints > 1)
                            {
                                curve.curveArr[curve.numCurveLines].curvePts?.Clear();

                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    vec3 vecPt = new vec3(double.Parse(words[0], CultureInfo.InvariantCulture),
                                        double.Parse(words[1], CultureInfo.InvariantCulture),
                                        double.Parse(words[2], CultureInfo.InvariantCulture));
                                    curve.curveArr[curve.numCurveLines].curvePts.Add(vecPt);
                                }
                                curve.numCurveLines++;
                            }
                            else
                            {
                                if (curve.curveArr.Count > 0)
                                {
                                    curve.curveArr.RemoveAt(curve.numCurveLines);
                                }
                            }
                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsCurveLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show();
                        WriteErrorLog("Load Curve Line" + er.ToString());
                    }
                }
            }

            if (curve.numCurveLines == 0) curve.numCurveLineSelected = 0;
            if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = curve.numCurveLines;
        }

        public void FileSaveABLines()
        {
            ABLine.moveDistance = 0;

            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            //get the file of previous AB Lines
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";
            int cnt = ABLine.lineArr.Count;

            using (StreamWriter writer = new StreamWriter(filename, false))
            {
                if (cnt > 0)
                {
                    foreach (var item in ABLine.lineArr)
                    {
                        //make it culture invariant
                        string line = item.Name
                            + ',' + (Math.Round(glm.toDegrees(item.heading), 8)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.origin.easting, 3)).ToString(CultureInfo.InvariantCulture)
                            + ',' + (Math.Round(item.origin.northing, 3)).ToString(CultureInfo.InvariantCulture);

                        //write out to file
                        writer.WriteLine(line);
                    }
                }
            }

            if (ABLine.numABLines == 0) ABLine.numABLineSelected = 0;
            if (ABLine.numABLineSelected > ABLine.numABLines) ABLine.numABLineSelected = ABLine.numABLines;
        }

        public void FileLoadABLines()
        {
            ABLine.moveDistance = 0;

            //make sure at least a global blank AB Line file exists
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string filename = directoryName + "\\ABLines.txt";

            if (!File.Exists(filename))
            {
                using (StreamWriter writer = new StreamWriter(filename))
                {
                }
            }

            if (!File.Exists(filename))
            {
                TimedMessageBox(2000, gStr.gsFileError, gStr.gsMissingABLinesFile);
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line;
                        ABLine.numABLines = 0;
                        ABLine.numABLineSelected = 0;
                        ABLine.lineArr?.Clear();

                        //read all the lines
                        for (int i = 0; !reader.EndOfStream; i++)
                        {

                            line = reader.ReadLine();
                            string[] words = line.Split(',');

                            if (words.Length != 4) break;

                            ABLine.lineArr.Add(new CABLines());

                            ABLine.lineArr[i].Name = words[0];


                            ABLine.lineArr[i].heading = glm.toRadians(double.Parse(words[1], CultureInfo.InvariantCulture));
                            ABLine.lineArr[i].origin.easting = double.Parse(words[2], CultureInfo.InvariantCulture);
                            ABLine.lineArr[i].origin.northing = double.Parse(words[3], CultureInfo.InvariantCulture);

                            ABLine.lineArr[i].ref1.easting = ABLine.lineArr[i].origin.easting - (Math.Sin(ABLine.lineArr[i].heading) * 1000.0);
                            ABLine.lineArr[i].ref1.northing = ABLine.lineArr[i].origin.northing - (Math.Cos(ABLine.lineArr[i].heading) * 1000.0);

                            ABLine.lineArr[i].ref2.easting = ABLine.lineArr[i].origin.easting + (Math.Sin(ABLine.lineArr[i].heading) * 1000.0);
                            ABLine.lineArr[i].ref2.northing = ABLine.lineArr[i].origin.northing + (Math.Cos(ABLine.lineArr[i].heading) * 1000.0);
                            ABLine.numABLines++;
                        }
                    }
                    catch (Exception er)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsABLineFileIsCorrupt, "Please delete it!!!");
                        form.Show();
                        WriteErrorLog("FieldOpen, Loading ABLine, Corrupt ABLine File" + er);
                    }
                }
            }

            if (ABLine.numABLines == 0) ABLine.numABLineSelected = 0;
            if (ABLine.numABLineSelected > ABLine.numABLines) ABLine.numABLineSelected = ABLine.numABLines;
        }

        //function that save vehicle and section settings
        public void FileSaveVehicle()
        {
            //in the current application directory
            //string dir = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //string fieldDir = dir + "\\fields\\";

            string dirVehicle = vehiclesDirectory;

            string directoryName = Path.GetDirectoryName(dirVehicle).ToString(CultureInfo.InvariantCulture);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            SaveFileDialog saveDialog = new SaveFileDialog();

            saveDialog.Title = "Save Vehicle";
            saveDialog.Filter = "Text Files (*.txt)|*.txt";
            saveDialog.InitialDirectory = directoryName;

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                vehiclefileName = Path.GetFileNameWithoutExtension(saveDialog.FileName) + " - ";
                Properties.Vehicle.Default.setVehicle_Name = vehiclefileName;
                Properties.Vehicle.Default.Save();

                using (StreamWriter writer = new StreamWriter(saveDialog.FileName))
                {
                    writer.WriteLine("Version," + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Overlap," + Properties.Vehicle.Default.setVehicle_toolOverlap.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ToolTrailingHitchLength," + Properties.Vehicle.Default.setVehicle_toolTrailingHitchLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("TankTrailingHitchLength," + Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("AntennaHeight," + Properties.Vehicle.Default.setVehicle_antennaHeight.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("LookAhead," + Properties.Vehicle.Default.setVehicle_lookAhead.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("AntennaPivot," + Properties.Vehicle.Default.setVehicle_antennaPivot.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("HitchLength," + Properties.Vehicle.Default.setVehicle_hitchLength.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ToolOffset," + Properties.Vehicle.Default.setVehicle_toolOffset.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("TurnOffDelay," + Properties.Vehicle.Default.setVehicle_turnOffDelay.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Wheelbase," + Properties.Vehicle.Default.setVehicle_wheelbase.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("IsPivotBehindAntenna," + Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsSteerAxleAhead," + Properties.Vehicle.Default.setVehicle_isSteerAxleAhead.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsToolBehindPivot," + Properties.Vehicle.Default.setVehicle_isToolBehindPivot.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsToolTrailing," + Properties.Vehicle.Default.setVehicle_isToolTrailing.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("Spinner1," + Properties.Vehicle.Default.setSection_position1.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner2," + Properties.Vehicle.Default.setSection_position2.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner3," + Properties.Vehicle.Default.setSection_position3.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner4," + Properties.Vehicle.Default.setSection_position4.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner5," + Properties.Vehicle.Default.setSection_position5.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner6," + Properties.Vehicle.Default.setSection_position6.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner7," + Properties.Vehicle.Default.setSection_position7.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner8," + Properties.Vehicle.Default.setSection_position8.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner9," + Properties.Vehicle.Default.setSection_position9.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner10," + Properties.Vehicle.Default.setSection_position10.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner11," + Properties.Vehicle.Default.setSection_position11.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner12," + Properties.Vehicle.Default.setSection_position12.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Spinner13," + Properties.Vehicle.Default.setSection_position13.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("Sections," + Properties.Vehicle.Default.setVehicle_numSections.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ToolWidth," + Properties.Vehicle.Default.setVehicle_toolWidth.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("WorkSwitch," + Properties.Settings.Default.setF_IsWorkSwitchEnabled.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ActiveLow," + Properties.Settings.Default.setF_IsWorkSwitchActiveLow.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("CamPitch," + Properties.Settings.Default.setCam_pitch.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("HeadingFromSource," + Properties.Settings.Default.setGPS_headingFromWhichSource);
                    writer.WriteLine("TriangleResolution," + Properties.Settings.Default.setDisplay_triangleResolution.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsMetric," + Properties.Settings.Default.setMenu_isMetric.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsGridOn," + Properties.Settings.Default.setMenu_isGridOn.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsLightBarOn," + Properties.Settings.Default.setMenu_isLightbarOn.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsAreaRight," + Properties.Settings.Default.setMenu_isAreaRight.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsPurePursuitLineOn," + Properties.Settings.Default.setMenu_isPureOn.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsGuideLinesOn," + Properties.Settings.Default.setMenu_isSideGuideLines.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("AntennaOffset," + Properties.Vehicle.Default.setVehicle_antennaOffset.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");

                    writer.WriteLine("FieldColorR," + Properties.Settings.Default.setF_FieldColorR.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("FieldColorG," + Properties.Settings.Default.setF_FieldColorG.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("FieldColorB," + Properties.Settings.Default.setF_FieldColorB.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("SectionColorR," + Properties.Settings.Default.setF_SectionColorR.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("SectionColorG," + Properties.Settings.Default.setF_SectionColorG.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("SectionColorB," + Properties.Settings.Default.setF_SectionColorB.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("SlowSpeedCutoff," + Properties.Vehicle.Default.setVehicle_slowSpeedCutoff.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("ToolMinUnappliedPixels," + Properties.Vehicle.Default.setVehicle_minApplied.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("MinTurningRadius," + Properties.Vehicle.Default.setVehicle_minTurningRadius.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("YouTurnUseDubins," + Properties.Vehicle.Default.set_youUseDubins.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");

                    writer.WriteLine("IMUPitchZero," + Properties.Settings.Default.setIMU_pitchZeroX16.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IMURollZero," + Properties.Settings.Default.setIMU_rollZeroX16.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("IsLogNMEA," + Properties.Settings.Default.setMenu_isLogNMEA.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("MinFixStep," + Properties.Settings.Default.setF_minFixStep.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("pidP," + Properties.Settings.Default.setAS_Kp.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("pidI," + Properties.Settings.Default.setAS_Ki.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("pidD," + Properties.Settings.Default.setAS_Kd.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("pidO," + Properties.Settings.Default.setAS_Ko.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("SteerAngleOffset," + Properties.Settings.Default.setAS_steerAngleOffset.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("minPWM," + Properties.Settings.Default.setAS_minSteerPWM.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("MaxIntegral," + Properties.Settings.Default.setAS_maxIntegral.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("CountsPerDegree," + Properties.Settings.Default.setAS_countsPerDegree.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");

                    writer.WriteLine("GoalPointLookAhead," + Properties.Vehicle.Default.setVehicle_goalPointLookAhead.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("MaxSteerAngle," + Properties.Vehicle.Default.setVehicle_maxSteerAngle.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("MaxAngularVelocity," + Properties.Vehicle.Default.setVehicle_maxAngularVelocity.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("SequenceFunctionEnter;" + Properties.Vehicle.Default.seq_FunctionEnter);
                    writer.WriteLine("SequenceFunctionExit;" + Properties.Vehicle.Default.seq_FunctionExit);
                    writer.WriteLine("SequenceActionEnter;" + Properties.Vehicle.Default.seq_ActionEnter);
                    writer.WriteLine("SequenceActionExit;" + Properties.Vehicle.Default.seq_ActionExit);
                    writer.WriteLine("SequenceDistanceEnter;" + Properties.Vehicle.Default.seq_DistanceEnter);
                    writer.WriteLine("SequenceDistanceExit;" + Properties.Vehicle.Default.seq_DistanceExit);

                    writer.WriteLine("FunctionList;" + Properties.Vehicle.Default.seq_FunctionList);
                    writer.WriteLine("ActionList;" + Properties.Vehicle.Default.seq_ActionList);

                    writer.WriteLine("RollFromBrick," + Properties.Settings.Default.setIMU_isRollFromBrick);
                    writer.WriteLine("HeadingFromBrick," + Properties.Settings.Default.setIMU_isHeadingFromBrick);
                    writer.WriteLine("RollFromDogs," + Properties.Settings.Default.setIMU_isRollFromAutoSteer);
                    writer.WriteLine("HeadingFromBNO," + Properties.Settings.Default.setIMU_isHeadingFromAutoSteer);

                    writer.WriteLine("GoalPointLookAheadUTurnMult," +
                        Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("GoalPointLookAheadMinumum," +
                        Properties.Vehicle.Default.setVehicle_lookAheadMinimum.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("GoalPointLookAheadDistanceFromLine," +
                        Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine("SwitchManual," + Properties.Settings.Default.setF_IsWorkSwitchManual.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine("Empty," + "10");
                    writer.WriteLine("Empty," + "10");
                }

                //little show to say saved and where
                var form = new FormTimedMessage(3000, gStr.gsSavedInFolder, dirVehicle);
                form.Show();
            }

        }

        //function to open a previously saved field
        public bool FileOpenVehicle()
        {
            OpenFileDialog ofd = new OpenFileDialog();

            //get the directory where the fields are stored
            string directoryName = vehiclesDirectory;

            //make sure the directory exists, if not, create it
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //the initial directory, fields, for the open dialog
            ofd.InitialDirectory = directoryName;

            //When leaving dialog put windows back where it was
            ofd.RestoreDirectory = true;

            //set the filter to text files only
            ofd.Filter = "txt files (*.txt)|*.txt";

            //was a file selected
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //if job started close it
                if (isJobStarted) JobClose();

                //make sure the file if fully valid and vehicle matches sections
                using (StreamReader reader = new StreamReader(ofd.FileName))
                {
                    try
                    {
                        string line;
                        Properties.Vehicle.Default.setVehicle_Name = ofd.FileName;
                        string[] words;
                        line = reader.ReadLine(); words = line.Split(',');

                        if (words[0] != "Version")

                        {
                            var form = new FormTimedMessage(5000, gStr.gsVehicleFileIsWrongVersion, gStr.gsMustBeVersion + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " or higher");
                            form.Show();
                            return false;
                        }

                        double fileVersion = double.Parse(words[1], CultureInfo.InvariantCulture);
                        string vers = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
                        double appVersion = double.Parse(vers, CultureInfo.InvariantCulture);

                        if (fileVersion < 3.0)
                        {
                            var form = new FormTimedMessage(5000, gStr.gsVehicleFileIsWrongVersion, gStr.gsMustBeVersion + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " or higher");
                            form.Show();
                            return false;
                        }

                        if (fileVersion == 3.0)
                        {
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_toolOverlap = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_toolTrailingHitchLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_antennaHeight = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_lookAhead = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_antennaPivot = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_hitchLength = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_toolOffset = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_turnOffDelay = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_wheelbase = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_isSteerAxleAhead = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_isToolBehindPivot = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_isToolTrailing = bool.Parse(words[1]);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position1 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position2 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position3 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position4 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position5 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position6 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position7 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position8 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position9 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position10 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position11 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position12 = decimal.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setSection_position13 = decimal.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_numSections = int.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_toolWidth = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_IsWorkSwitchEnabled = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_IsWorkSwitchActiveLow = bool.Parse(words[1]);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setCam_pitch = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setGPS_headingFromWhichSource = (words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setDisplay_triangleResolution = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isMetric = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isGridOn = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isLightbarOn = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isAreaRight = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isPureOn = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isSideGuideLines = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_antennaOffset = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_FieldColorR = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_FieldColorG = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_FieldColorB = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_SectionColorR = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_SectionColorG = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_SectionColorB = byte.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_slowSpeedCutoff = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_minApplied = int.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_minTurningRadius = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.set_youUseDubins = bool.Parse(words[1]);

                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();
                            line = reader.ReadLine();

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setIMU_pitchZeroX16 = int.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setIMU_rollZeroX16 = int.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setMenu_isLogNMEA = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setF_minFixStep = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_Kp = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_Ki = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_Kd = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_Ko = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_steerAngleOffset = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_minSteerPWM = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_maxIntegral = byte.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setAS_countsPerDegree = byte.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine();
                            line = reader.ReadLine();

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_goalPointLookAhead = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_maxSteerAngle = double.Parse(words[1], CultureInfo.InvariantCulture);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Vehicle.Default.setVehicle_maxAngularVelocity = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_FunctionEnter = words[1];
                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_FunctionExit = words[1];
                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_ActionEnter = words[1];
                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_ActionExit = words[1];
                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_DistanceEnter = words[1];
                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_DistanceExit = words[1];

                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_FunctionList = words[1];
                            line = reader.ReadLine(); words = line.Split(';');
                            Properties.Vehicle.Default.seq_ActionList = words[1];

                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setIMU_isRollFromBrick = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setIMU_isHeadingFromBrick = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setIMU_isRollFromAutoSteer = bool.Parse(words[1]);
                            line = reader.ReadLine(); words = line.Split(',');
                            Properties.Settings.Default.setIMU_isHeadingFromAutoSteer = bool.Parse(words[1]);

                            line = reader.ReadLine(); words = line.Split(',');
                            if (words[0] == "Empty") Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult = 0.7;
                            else Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            if (words[0] == "Empty") Properties.Vehicle.Default.setVehicle_lookAheadMinimum = 3;
                            else Properties.Vehicle.Default.setVehicle_lookAheadMinimum = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            if (words[0] == "Empty") Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine = 1.2;
                            else Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine = double.Parse(words[1], CultureInfo.InvariantCulture);

                            line = reader.ReadLine(); words = line.Split(',');
                            if (words[0] == "Empty") Properties.Settings.Default.setF_IsWorkSwitchManual = false;
                            else Properties.Settings.Default.setF_IsWorkSwitchManual = bool.Parse(words[1]);

                            line = reader.ReadLine();
                            line = reader.ReadLine();

                            //fill in the current variables with restored data
                            vehiclefileName = Path.GetFileNameWithoutExtension(ofd.FileName) + " - ";
                            Properties.Vehicle.Default.setVehicle_Name = vehiclefileName;

                            Properties.Settings.Default.Save();
                            Properties.Vehicle.Default.Save();

                            //get the number of sections from settings
                            vehicle.numOfSections = Properties.Vehicle.Default.setVehicle_numSections;
                            vehicle.numSuperSection = vehicle.numOfSections + 1;

                            //from settings grab the vehicle specifics
                            vehicle.toolOverlap = Properties.Vehicle.Default.setVehicle_toolOverlap;
                            vehicle.toolTrailingHitchLength = Properties.Vehicle.Default.setVehicle_toolTrailingHitchLength;
                            vehicle.tankTrailingHitchLength = Properties.Vehicle.Default.setVehicle_tankTrailingHitchLength;
                            vehicle.antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
                            vehicle.antennaOffset = Properties.Vehicle.Default.setVehicle_antennaOffset;

                            vehicle.toolLookAhead = Properties.Vehicle.Default.setVehicle_lookAhead;
                            vehicle.goalPointLookAheadMinimumDistance = Properties.Vehicle.Default.setVehicle_lookAheadMinimum;
                            vehicle.goalPointDistanceMultiplier = Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine;
                            vehicle.goalPointLookAheadUturnMult = Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult;

                            vehicle.antennaPivot = Properties.Vehicle.Default.setVehicle_antennaPivot;
                            vehicle.hitchLength = Properties.Vehicle.Default.setVehicle_hitchLength;
                            vehicle.toolOffset = Properties.Vehicle.Default.setVehicle_toolOffset;
                            vehicle.toolTurnOffDelay = Properties.Vehicle.Default.setVehicle_turnOffDelay;
                            vehicle.wheelbase = Properties.Vehicle.Default.setVehicle_wheelbase;
                            vehicle.minTurningRadius = Properties.Vehicle.Default.setVehicle_minTurningRadius;
                            yt.isUsingDubinsTurn = Properties.Vehicle.Default.set_youUseDubins;

                            vehicle.isToolTrailing = Properties.Vehicle.Default.setVehicle_isToolTrailing;
                            vehicle.isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
                            vehicle.isSteerAxleAhead = Properties.Vehicle.Default.setVehicle_isSteerAxleAhead;
                            vehicle.isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isToolBehindPivot;
                            vehicle.toolMinUnappliedPixels = Properties.Vehicle.Default.setVehicle_minApplied;

                            vehicle.maxAngularVelocity = Properties.Vehicle.Default.setVehicle_maxAngularVelocity;
                            vehicle.maxSteerAngle = Properties.Vehicle.Default.setVehicle_maxSteerAngle;

                            mc.autoSteerSettings[mc.ssKp] = Properties.Settings.Default.setAS_Kp;
                            mc.autoSteerSettings[mc.ssKi] = Properties.Settings.Default.setAS_Ki;
                            mc.autoSteerSettings[mc.ssKd] = Properties.Settings.Default.setAS_Kd;
                            mc.autoSteerSettings[mc.ssKo] = Properties.Settings.Default.setAS_Ko;
                            mc.autoSteerSettings[mc.ssSteerOffset] = Properties.Settings.Default.setAS_steerAngleOffset;
                            mc.autoSteerSettings[mc.ssMinPWM] = Properties.Settings.Default.setAS_minSteerPWM;
                            mc.autoSteerSettings[mc.ssMaxIntegral] = Properties.Settings.Default.setAS_maxIntegral;
                            mc.autoSteerSettings[mc.ssCountsPerDegree] = Properties.Settings.Default.setAS_countsPerDegree;

                            mc.isWorkSwitchEnabled = Properties.Settings.Default.setF_IsWorkSwitchEnabled;
                            mc.isWorkSwitchActiveLow = Properties.Settings.Default.setF_IsWorkSwitchActiveLow;
                            mc.isWorkSwitchManual = Properties.Settings.Default.setF_IsWorkSwitchManual;
                        }

                        //Set width of section and positions for each section
                        SectionSetPosition();

                        //Calculate total width and each section width
                        SectionCalcWidths();

                        //enable disable manual buttons
                        LineUpManualBtns();

                        btnSection1Man.Enabled = false;
                        btnSection2Man.Enabled = false;
                        btnSection3Man.Enabled = false;
                        btnSection4Man.Enabled = false;
                        btnSection5Man.Enabled = false;
                        btnSection6Man.Enabled = false;
                        btnSection7Man.Enabled = false;
                        btnSection8Man.Enabled = false;

                        camera.camPitch = Properties.Settings.Default.setCam_pitch;

                        headingFromSource = Properties.Settings.Default.setGPS_headingFromWhichSource;

                        isMetric = Properties.Settings.Default.setMenu_isMetric;
                        metricToolStrip.Checked = isMetric;
                        imperialToolStrip.Checked = !isMetric;

                        isGridOn = Properties.Settings.Default.setMenu_isGridOn;
                        gridOnToolStripMenuItem.Checked = (isGridOn);

                        isLightbarOn = Properties.Settings.Default.setMenu_isLightbarOn;
                        lightbarToolStripMenuItem.Checked = isLightbarOn;

                        isPureDisplayOn = Properties.Settings.Default.setMenu_isPureOn;
                        pursuitOnToolStripMenuItem.Checked = isPureDisplayOn;

                        isSideGuideLines = Properties.Settings.Default.setMenu_isSideGuideLines;
                        extraGuidesToolStripMenuItem.Checked = isSideGuideLines;

                        isAreaOnRight = Properties.Settings.Default.setMenu_isAreaRight;

                        redSections = Properties.Settings.Default.setF_SectionColorR;
                        grnSections = Properties.Settings.Default.setF_SectionColorG;
                        bluSections = Properties.Settings.Default.setF_SectionColorB;
                        redField = Properties.Settings.Default.setF_FieldColorR;
                        grnField = Properties.Settings.Default.setF_FieldColorG;
                        bluField = Properties.Settings.Default.setF_FieldColorB;

                        vehicle.slowSpeedCutoff = Properties.Vehicle.Default.setVehicle_slowSpeedCutoff;

                        ahrs.pitchZeroX16 = Properties.Settings.Default.setIMU_pitchZeroX16;
                        ahrs.rollZeroX16 = Properties.Settings.Default.setIMU_rollZeroX16;
                        isLogNMEA = Properties.Settings.Default.setMenu_isLogNMEA;
                        minFixStepDist = Properties.Settings.Default.setF_minFixStep;

                        string sentence = Properties.Vehicle.Default.seq_FunctionEnter;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqEnter[i].function);

                        sentence = Properties.Vehicle.Default.seq_ActionEnter;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqEnter[i].action);

                        sentence = Properties.Vehicle.Default.seq_DistanceEnter;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++)
                            double.TryParse(words[i], NumberStyles.Float, CultureInfo.InvariantCulture, out seq.seqEnter[i].distance);

                        sentence = Properties.Vehicle.Default.seq_FunctionExit;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqExit[i].function);

                        sentence = Properties.Vehicle.Default.seq_ActionExit;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++) int.TryParse(words[i], out seq.seqExit[i].action);

                        sentence = Properties.Vehicle.Default.seq_DistanceExit;
                        words = sentence.Split(',');
                        for (int i = 0; i < FormGPS.MAXFUNCTIONS; i++)
                            double.TryParse(words[i], NumberStyles.Float, CultureInfo.InvariantCulture, out seq.seqExit[i].distance);

                        ahrs.isRollFromBrick = Properties.Settings.Default.setIMU_isRollFromBrick;
                        ahrs.isHeadingFromBrick = Properties.Settings.Default.setIMU_isHeadingFromBrick;
                        ahrs.isRollFromAutoSteer = Properties.Settings.Default.setIMU_isRollFromAutoSteer;
                        ahrs.isHeadingFromAutoSteer = Properties.Settings.Default.setIMU_isHeadingFromAutoSteer;

                        return true;
                        //Application.Exit();
                    }
                    catch (Exception e) //FormatException e || IndexOutOfRangeException e2)
                    {
                        WriteErrorLog("Open Vehicle" + e.ToString());

                        //vehicle is corrupt, reload with all default information
                        Properties.Settings.Default.Reset();
                        Properties.Settings.Default.Save();
                        MessageBox.Show(gStr.gsProgramWillResetToRecoverPleaseRestart, gStr.gsVehicleFileIsCorrupt, MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        Application.Exit();
                        return false;
                    }
                }
            }      //cancelled out of open file

            return false;
        }//end of open file

        //function to open a previously saved field, resume, open exisiting, open named field
        public void FileOpenField(string _openType)
        {
            string fileAndDirectory = "";
            if (_openType.Contains("Field.txt"))
            {
                fileAndDirectory = _openType;
                _openType = "Load";
            }

            else fileAndDirectory = "Cancel";

            //get the directory where the fields are stored
            //string directoryName = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+ "\\fields\\";
            switch (_openType)
            {
                case "Resume":
                    {
                        //Either exit or update running save
                        fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Field.txt";
                        if (!File.Exists(fileAndDirectory)) fileAndDirectory = "Cancel";
                        break;
                    }

                case "Open":
                    {
                        //create the dialog instance
                        OpenFileDialog ofd = new OpenFileDialog();

                        //the initial directory, fields, for the open dialog
                        ofd.InitialDirectory = fieldsDirectory;

                        //When leaving dialog put windows back where it was
                        ofd.RestoreDirectory = true;

                        //set the filter to text files only
                        ofd.Filter = "Field files (Field.txt)|Field.txt";

                        //was a file selected
                        if (ofd.ShowDialog() == DialogResult.Cancel) fileAndDirectory = "Cancel";
                        else fileAndDirectory = ofd.FileName;
                        break;
                    }
            }

            if (fileAndDirectory == "Cancel") return;

            //close the existing job and reset everything
            this.JobClose();

            //and open a new job
            this.JobNew();

            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //start to read the file
            string line;
            using (StreamReader reader = new StreamReader(fileAndDirectory))
            {
                try
                {
                    //Date time line
                    line = reader.ReadLine();

                    //dir header $FieldDir
                    line = reader.ReadLine();

                    //read field directory
                    line = reader.ReadLine();

                    currentFieldDirectory = line.Trim();

                    //Offset header
                    line = reader.ReadLine();

                    //read the Offsets 
                    line = reader.ReadLine();
                    string[] offs = line.Split(',');
                    pn.utmEast = int.Parse(offs[0]);
                    pn.utmNorth = int.Parse(offs[1]);
                    pn.zone = int.Parse(offs[2]);

                    //create a new grid
                    worldGrid.CreateWorldGrid(pn.actualNorthing - pn.utmNorth, pn.actualEasting - pn.utmEast);

                    //convergence angle update
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        pn.convergenceAngle = double.Parse(line, CultureInfo.InvariantCulture);
                        lblConvergenceAngle.Text = Math.Round(glm.toDegrees(pn.convergenceAngle), 3).ToString();
                    }

                    //start positions
                    if (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        offs = line.Split(',');

                        pn.latStart = double.Parse(offs[0], CultureInfo.InvariantCulture);
                        pn.lonStart = double.Parse(offs[1], CultureInfo.InvariantCulture);
                    }
                }

                catch (Exception e)
                {
                    WriteErrorLog("While Opening Field" + e.ToString());

                    var form = new FormTimedMessage(2000, gStr.gsFieldFileIsCorrupt, gStr.gsChooseADifferentField);

                    form.Show();
                    JobClose();
                    return;
                }
            }

            // ABLine -------------------------------------------------------------------------------------------------
            FileLoadABLines();

            if (ABLine.lineArr.Count > 0)
            {
                ABLine.numABLineSelected = 1;
                ABLine.refPoint1 = ABLine.lineArr[ABLine.numABLineSelected - 1].origin;
                //ABLine.refPoint2 = ABLine.lineArr[ABLine.numABLineSelected - 1].ref2;
                ABLine.abHeading = ABLine.lineArr[ABLine.numABLineSelected - 1].heading;
                ABLine.SetABLineByHeading();
                ABLine.isABLineSet = false;
                ABLine.isABLineLoaded = true;
            }
            else
            {
                ABLine.isABLineSet = false;
                ABLine.isABLineLoaded = false;
            }


            //CurveLines
            FileLoadCurveLines();
            if (curve.curveArr.Count > 0)
            {
                curve.numCurveLineSelected = 1;
                int idx = curve.numCurveLineSelected - 1;
                curve.aveLineHeading = curve.curveArr[idx].aveHeading;

                curve.refList?.Clear();
                for (int i = 0; i < curve.curveArr[idx].curvePts.Count; i++)
                {
                    curve.refList.Add(curve.curveArr[idx].curvePts[i]);
                }
                curve.isCurveSet = true;
            }
            else
            {
                curve.isCurveSet = false;
                curve.refList?.Clear();
            }

            ////Either exit or update running save
            //fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\ABLine.txt";
            //if (!File.Exists(fileAndDirectory))
            //{
            //    var form = new FormTimedMessage(2000, gStr.gsMissingABLinesFile, gStr.gsButFieldIsLoaded);
            //    form.Show();
            //}

            //else
            //{
            //    using (StreamReader reader = new StreamReader(fileAndDirectory))
            //    {
            //        try
            //        {
            //            ABLine.isABLineLoaded = false;

            //            //read header
            //            line = reader.ReadLine();

            //            line = reader.ReadLine();
            //            bool isAB = bool.Parse(line);

            //            if (isAB)
            //            {
            //                //Heading  , ,refPoint2x,z                    
            //                line = reader.ReadLine();
            //                ABLine.abHeading = double.Parse(line, CultureInfo.InvariantCulture);

            //                //refPoint1x,z
            //                line = reader.ReadLine();
            //                string[] words = line.Split(',');
            //                ABLine.refPoint1.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
            //                ABLine.refPoint1.northing = double.Parse(words[1], CultureInfo.InvariantCulture);

            //                //refPoint2x,z
            //                line = reader.ReadLine();
            //                words = line.Split(',');
            //                ABLine.refPoint2.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
            //                ABLine.refPoint2.northing = double.Parse(words[1], CultureInfo.InvariantCulture);

            //                //Tramline
            //                line = reader.ReadLine();
            //                words = line.Split(',');
            //                ABLine.tramPassEvery = int.Parse(words[0]);
            //                ABLine.passBasedOn = int.Parse(words[1]);

            //                ABLine.refABLineP1.easting = ABLine.refPoint1.easting - Math.Sin(ABLine.abHeading) * 4000.0;
            //                ABLine.refABLineP1.northing = ABLine.refPoint1.northing - Math.Cos(ABLine.abHeading) * 4000.0;

            //                ABLine.refABLineP2.easting = ABLine.refPoint1.easting + Math.Sin(ABLine.abHeading) * 4000.0;
            //                ABLine.refABLineP2.northing = ABLine.refPoint1.northing + Math.Cos(ABLine.abHeading) * 4000.0;

            //                ABLine.isABLineLoaded = true;

            //            }

            //            //clean up from last field maybe
            //            ABLine.isABLineSet = false;
            //            ABLine.isBtnABLineOn = false;
            //            DisableYouTurnButtons();
            //            btnContourPriority.Enabled = true;
            //            btnABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOff;
            //        }

            //        catch (Exception e)
            //        {
            //            var form = new FormTimedMessage(2000, gStr.gsABLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
            //            form.Show();
            //            WriteErrorLog("Load AB Line" + e.ToString());

            //        }
            //    }
            //}
            //// CurveLine  -------------------------------------------------------------------------------------------------

            ////Either exit or update running save
            //fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\CurveLine.txt";
            //if (!File.Exists(fileAndDirectory))
            //{
            //    var form = new FormTimedMessage(2000, gStr.gsMissingABCurveFile, gStr.gsButFieldIsLoaded);
            //    form.Show();
            //}

            //else
            //{
            //    using (StreamReader reader = new StreamReader(fileAndDirectory))
            //    {
            //        try
            //        {
            //            //read header
            //            line = reader.ReadLine();

            //            // get the average heading
            //            line = reader.ReadLine();
            //            curve.aveLineHeading = double.Parse(line, CultureInfo.InvariantCulture);


            //            line = reader.ReadLine();
            //            int numPoints = int.Parse(line);

            //            if (numPoints > 0)
            //            {
            //                curve.refList?.Clear();

            //                //load the line
            //                for (int i = 0; i < numPoints; i++)
            //                {
            //                    line = reader.ReadLine();
            //                    string[] words = line.Split(',');
            //                    vec3 vecPt = new vec3(
            //                    double.Parse(words[0], CultureInfo.InvariantCulture),
            //                    double.Parse(words[1], CultureInfo.InvariantCulture),
            //                    double.Parse(words[2], CultureInfo.InvariantCulture));

            //                    curve.refList.Add(vecPt);
            //                }
            //            }
            //        }

            //        catch (Exception e)
            //        {
            //            var form = new FormTimedMessage(2000, gStr.gsCurveLineFileIsCorrupt, gStr.gsButFieldIsLoaded);
            //            form.Show();
            //            WriteErrorLog("Load Boundary Line" + e.ToString());

            //        }
            //    }
            //}

            //section patches
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Sections.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingSectionFile, gStr.gsButFieldIsLoaded);
                form.Show();
                //return;
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        fd.workedAreaTotal = 0;
                        fd.distanceUser = 0;
                        vec2 vecFix = new vec2(0, 0);

                        //read header
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            section[0].triangleList = new List<vec2>();
                            section[0].patchList.Add(section[0].triangleList);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                section[0].triangleList.Add(vecFix);
                            }

                            //calculate area of this patch - AbsoluteValue of (Ax(By-Cy) + Bx(Cy-Ay) + Cx(Ay-By)/2)
                            verts -= 2;
                            if (verts >= 2)
                            {
                                for (int j = 0; j < verts; j++)
                                {
                                    double temp = 0;
                                    temp = section[0].triangleList[j].easting * (section[0].triangleList[j + 1].northing - section[0].triangleList[j + 2].northing) +
                                              section[0].triangleList[j + 1].easting * (section[0].triangleList[j + 2].northing - section[0].triangleList[j].northing) +
                                                  section[0].triangleList[j + 2].easting * (section[0].triangleList[j].northing - section[0].triangleList[j + 1].northing);

                                    fd.workedAreaTotal += Math.Abs((temp * 0.5));
                                }
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Section file" + e.ToString());

                        var form = new FormTimedMessage(2000, gStr.gsSectionFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show();
                    }
                }
            }

            // Contour points ----------------------------------------------------------------------------

            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Contour.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingContourFile, gStr.gsButFieldIsLoaded);
                form.Show();
                //return;
            }

            //Points in Patch followed by easting, heading, northing, altitude
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            //read how many vertices in the following patch
                            line = reader.ReadLine();
                            int verts = int.Parse(line);

                            vec3 vecFix = new vec3(0, 0, 0);

                            ct.ptList = new List<vec3>();
                            ct.stripList.Add(ct.ptList);

                            for (int v = 0; v < verts; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                vecFix.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                vecFix.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                vecFix.heading = double.Parse(words[2], CultureInfo.InvariantCulture);
                                ct.ptList.Add(vecFix);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        WriteErrorLog("Loading Contour file" + e.ToString());

                        var form = new FormTimedMessage(2000, gStr.gsContourFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show();
                    }
                }
            }


            // Flags -------------------------------------------------------------------------------------------------

            //Either exit or update running save
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Flags.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingFlagsFile, gStr.gsButFieldIsLoaded);
                form.Show();
            }

            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();

                        line = reader.ReadLine();
                        int points = int.Parse(line);

                        if (points > 0)
                        {
                            double lat;
                            double longi;
                            double east;
                            double nort;
                            int color, ID;
                            string flgtxty;

                            for (int v = 0; v < points; v++)
                            {

                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                int ct = words.Length;

                                lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                                longi = double.Parse(words[1], CultureInfo.InvariantCulture);
                                east = double.Parse(words[2], CultureInfo.InvariantCulture);
                                nort = double.Parse(words[3], CultureInfo.InvariantCulture);
                                color = int.Parse(words[4]);
                                ID = int.Parse(words[5]);
                                if (ct == 7)
                                {
                                    flgtxty = words[6];
                                    CFlag flagPt = new CFlag(lat, longi, east, nort, color, ID, flgtxty);
                                    flagPts.Add(flagPt);
                                }
                                else
                                {
                                    CFlag flagPt = new CFlag(lat, longi, east, nort, color, ID, "");
                                    flagPts.Add(flagPt);
                                }



                            }

                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsFlagFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show();
                        WriteErrorLog("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                    }
                }
            }


            //Boundaries
            //Either exit or update running save
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Boundary.txt";
            if (!File.Exists(fileAndDirectory))
            {
                var form = new FormTimedMessage(2000, gStr.gsMissingBoundaryFile, gStr.gsButFieldIsLoaded);
                form.Show();
            }
            else
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();//Boundary

                        for (int k = 0; true; k++)
                        {
                            if (reader.EndOfStream) break;

                            bnd.bndArr.Add(new CBoundaryLines());
                            turn.turnArr.Add(new CTurnLines());
                            gf.geoFenceArr.Add(new CGeoFenceLines());

                            //True or False OR points from older boundary files
                            line = reader.ReadLine();

                            //Check for older boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                bnd.bndArr[k].isDriveThru = bool.Parse(line);
                                line = reader.ReadLine(); //number of points
                            }

                            //Check for latest boundary files, then above line string is num of points
                            if (line == "True" || line == "False")
                            {
                                bnd.bndArr[k].isDriveAround = bool.Parse(line);
                                line = reader.ReadLine(); //number of points
                            }

                            //Check for latest boundary files, then above line string is num of points
                            //bool turnheading = false;
                            //if (line == "True" || line == "False")
                            //{
                            //    bnd.bndArr[k].isOwnField = bool.Parse(line);
                            //    line = reader.ReadLine(); //number of points
                            //}
                            //else if (k == 0)
                            //{
                            //    turnheading = true;
                            //    bnd.bndArr[k].isOwnField = true;
                            //}

                            int numPoints = int.Parse(line);

                            if (numPoints > 0)
                            {
                                //load the line
                                for (int i = 0; i < numPoints; i++)
                                {
                                    line = reader.ReadLine();
                                    string[] words = line.Split(',');
                                    CBndPt vecPt = new CBndPt(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture),
                                    double.Parse(words[2], CultureInfo.InvariantCulture));

                                    //if (turnheading)
                                    //{
                                    //    vecPt.heading = vecPt.heading + Math.PI;
                                    //}
                                    bnd.bndArr[k].bndLine.Add(vecPt);
                                }

                                bnd.bndArr[k].CalculateBoundaryArea();
                                bnd.bndArr[k].PreCalcBoundaryLines();
                                if (bnd.bndArr[k].area > 0) bnd.bndArr[k].isSet = true;
                                else bnd.bndArr[k].isSet = false;
                            }
                            else
                            {
                                bnd.bndArr.RemoveAt(bnd.bndArr.Count - 1);
                                turn.turnArr.RemoveAt(bnd.bndArr.Count - 1);
                                gf.geoFenceArr.RemoveAt(bnd.bndArr.Count - 1);
                                k = k - 1;
                            }
                            if (reader.EndOfStream) break;
                        }

                        CalculateMinMax();
                        turn.BuildTurnLines();
                        gf.BuildGeoFenceLines();
                        mazeGrid.BuildMazeGridArray();

                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsBoundaryLineFilesAreCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show();
                        WriteErrorLog("Load Boundary Line" + e.ToString());
                    }
                }
            }

            //// Headland  -------------------------------------------------------------------------------------------------
            //fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\Headland.txt";
            //if (!File.Exists(fileAndDirectory))
            //{
            //    var form = new FormTimedMessage(2000, "Missing Headland File", "But Field is Loaded");
            //    form.Show();
            //}

            //else
            //{
            //    using (StreamReader reader = new StreamReader(fileAndDirectory))
            //    {
            //        try
            //        {
            //            //read header
            //            line = reader.ReadLine();

            //            for (int j = 0; j < FormGPS.MAXHEADS; j++)
            //            {
            //                //read the number of points
            //                line = reader.ReadLine();
            //                int numPoints = int.Parse(line);

            //                if (numPoints > 0)
            //                {
            //                    hlArr[j].hlLine.Clear();

            //                    //load the line
            //                    for (int i = 0; i < numPoints; i++)
            //                    {
            //                        line = reader.ReadLine();
            //                        string[] words = line.Split(',');
            //                        vec3 vecPt = new vec3(
            //                            double.Parse(words[0], CultureInfo.InvariantCulture),
            //                            double.Parse(words[1], CultureInfo.InvariantCulture),
            //                            double.Parse(words[2], CultureInfo.InvariantCulture));
            //                        hlArr[j].hlLine.Add(vecPt);
            //                    }

            //                    hlArr[j].PreCalcHeadlandLines();
            //                    hlArr[j].isSet = true;
            //                }
            //                else
            //                {
            //                    hlArr[j].isSet = false;
            //                }

            //                //if older versions or different number, there is only 1
            //                if (reader.EndOfStream) break;
            //            }
            //        }

            //        catch (Exception e)
            //        {
            //            var form = new FormTimedMessage(2000, "Headland File is Corrupt", "But Field is Loaded");
            //            form.Show();
            //            WriteErrorLog("Load Headland Loop" + e.ToString());
            //        }
            //    }
            //}


            //Recorded Path
            fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\RecPath.txt";
            if (File.Exists(fileAndDirectory))
            {
                using (StreamReader reader = new StreamReader(fileAndDirectory))
                {
                    try
                    {
                        //read header
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        int numPoints = int.Parse(line);
                        recPath.recList.Clear();

                        while (!reader.EndOfStream)
                        {
                            for (int v = 0; v < numPoints; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');
                                CRecPathPt point = new CRecPathPt(
                                    double.Parse(words[0], CultureInfo.InvariantCulture),
                                    double.Parse(words[1], CultureInfo.InvariantCulture),
                                    double.Parse(words[2], CultureInfo.InvariantCulture),
                                    double.Parse(words[3], CultureInfo.InvariantCulture),
                                    bool.Parse(words[4]));

                                //add the point
                                recPath.recList.Add(point);
                            }
                        }
                    }

                    catch (Exception e)
                    {
                        var form = new FormTimedMessage(2000, gStr.gsRecordedPathFileIsCorrupt, gStr.gsButFieldIsLoaded);
                        form.Show();
                        WriteErrorLog("Load Recorded Path" + e.ToString());
                    }
                }
            }


        }//end of open file

        //creates the field file when starting new field
        public void FileCreateField()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            if (!isJobStarted)
            {
                using (var form = new FormTimedMessage(3000, gStr.gsFieldNotOpen, gStr.gsCreateNewField))
                { form.Show(); }
                return;
            }
            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Field.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.utmNorth.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.zone.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Convergence");
                writer.WriteLine(pn.convergenceAngle.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));

            }
        }

        public void FileCreateElevation()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FieldDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //if (!isJobStarted)
            //{
            //    using (var form = new FormTimedMessage(3000, "Ooops, Job Not Started", "Start a Job First"))
            //    { form.Show(); }
            //    return;
            //}

            string myFileName, dirField;

            //get the directory and make sure it exists, create if not
            dirField = fieldsDirectory + currentFieldDirectory + "\\";
            string directoryName = Path.GetDirectoryName(dirField);

            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            myFileName = "Elevation.txt";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //Write out the date
                writer.WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));

                writer.WriteLine("$FieldDir");
                writer.WriteLine(currentFieldDirectory.ToString(CultureInfo.InvariantCulture));

                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteLine(pn.utmEast.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.utmNorth.ToString(CultureInfo.InvariantCulture) + "," +
                    pn.zone.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Convergence");
                writer.WriteLine(pn.convergenceAngle.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("StartFix");
                writer.WriteLine(pn.latitude.ToString(CultureInfo.InvariantCulture) + "," + pn.longitude.ToString(CultureInfo.InvariantCulture));
            }
        }

        //save field Patches
        public void FileSaveSections()
        {
            //make sure there is something to save
            if (patchSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Sections.txt"), true))
                {
                    //for each patch, write out the list of triangles to the file
                    foreach (var triList in patchSaveList)
                    {
                        int count2 = triList.Count();
                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                            writer.WriteLine((Math.Round(triList[i].easting, 3)).ToString(CultureInfo.InvariantCulture) +
                                "," + (Math.Round(triList[i].northing, 3)).ToString(CultureInfo.InvariantCulture));
                    }
                }

                //clear out that patchList and begin adding new ones for next save
                patchSaveList.Clear();
            }
        }

        //Create contour file
        public void FileCreateSections()
        {
            //$Sections
            //10 - points in this patch
            //10.1728031317344,0.723157039771303 -easting, northing

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Sections.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                writer.WriteLine("$Sections");
            }
        }

        //Create contour file
        public void FileCreateContour()
        {
            //12  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "Contour.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                writer.WriteLine("$Contour");
            }
        }

        //save the contour points which include elevation values
        public void FileSaveContour()
        {
            //1  - points in patch
            //64.697,0.168,-21.654,0 - east, heading, north, altitude

            //make sure there is something to save
            if (contourSaveList.Count() > 0)
            {
                //Append the current list to the field file
                using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Contour.txt"), true))
                {

                    //for every new chunk of patch in the whole section
                    foreach (var triList in contourSaveList)
                    {
                        int count2 = triList.Count;

                        writer.WriteLine(count2.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < count2; i++)
                        {
                            writer.WriteLine(Math.Round((triList[i].easting), 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                Math.Round(triList[i].heading, 3).ToString(CultureInfo.InvariantCulture));
                        }
                    }
                }

                contourSaveList.Clear();

            }
        }

        //save the boundary
        public void FileSaveBoundary()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + "Boundary.Txt"))
            {
                writer.WriteLine("$Boundary");
                for (int i = 0; i < bnd.bndArr.Count; i++)
                {
                    writer.WriteLine(bnd.bndArr[i].isDriveThru);
                    writer.WriteLine(bnd.bndArr[i].isDriveAround);
                    //writer.WriteLine(bnd.bndArr[i].isOwnField);

                    writer.WriteLine(bnd.bndArr[i].bndLine.Count.ToString(CultureInfo.InvariantCulture));
                    if (bnd.bndArr[i].bndLine.Count > 0)
                    {
                        for (int j = 0; j < bnd.bndArr[i].bndLine.Count; j++)
                            writer.WriteLine(Math.Round(bnd.bndArr[i].bndLine[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                Math.Round(bnd.bndArr[i].bndLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                                                    Math.Round(bnd.bndArr[i].bndLine[j].heading, 5).ToString(CultureInfo.InvariantCulture));
                    }
                }
            }
        }

        ////save the headland
        //public void FileSaveHeadland()
        //{
        //    //get the directory and make sure it exists, create if not
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";

        //    string directoryName = Path.GetDirectoryName(dirField);
        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    //write out the file
        //    using (StreamWriter writer = new StreamWriter(dirField + "Headland.Txt"))
        //    {
        //        writer.WriteLine("$Headland");

        //        //writer.WriteLine("$StartPoint");
        //        //writer.WriteLine(hl.startPoint.easting.ToString(CultureInfo.InvariantCulture) + "," + 
        //        //                hl.startPoint.northing.ToString(CultureInfo.InvariantCulture) + "," + 
        //        //                hl.startPoint.heading.ToString(CultureInfo.InvariantCulture));

        //        for (int i = 0; i < FormGPS.MAXHEADS; i++)
        //        {
        //            writer.WriteLine(hlArr[i].hlLine.Count.ToString(CultureInfo.InvariantCulture));
        //            if (hlArr[0].hlLine.Count > 0)
        //            {
        //                for (int j = 0; j < hlArr[i].hlLine.Count; j++)
        //                    writer.WriteLine(Math.Round(hlArr[i].hlLine[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                     Math.Round(hlArr[i].hlLine[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                     Math.Round(hlArr[i].hlLine[j].heading, 3).ToString(CultureInfo.InvariantCulture));
        //            }
        //        }
        //    }
        //}

        //Create contour file
        public void FileCreateRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName = "RecPath.txt";

            //write out the file
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {
                //write paths # of sections
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }

        //save the recorded path
        public void FileSaveRecPath()
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //string fileAndDirectory = fieldsDirectory + currentFieldDirectory + "\\RecPath.txt";
            //if (!File.Exists(fileAndDirectory)) FileCreateRecPath();

            //write out the file
            using (StreamWriter writer = new StreamWriter((dirField + "RecPath.Txt")))
            {
                writer.WriteLine("$RecPath");
                writer.WriteLine(recPath.recList.Count.ToString(CultureInfo.InvariantCulture));
                if (recPath.recList.Count > 0)
                {
                    for (int j = 0; j < recPath.recList.Count; j++)
                        writer.WriteLine(
                            Math.Round(recPath.recList[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].heading, 3).ToString(CultureInfo.InvariantCulture) + "," +
                            Math.Round(recPath.recList[j].speed, 1).ToString(CultureInfo.InvariantCulture) + "," +
                            (recPath.recList[j].autoBtnState).ToString());

                    //Clear list
                    //recPath.recList.Clear();
                }
            }
        }

        //save all the flag markers
        public void FileSaveFlags()
        {
            //Saturday, February 11, 2017  -->  7:26:52 AM
            //$FlagsDir
            //Bob_Feb11
            //$Offsets
            //533172,5927719,12 - offset easting, northing, zone

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            //use Streamwriter to create and overwrite existing flag file
            using (StreamWriter writer = new StreamWriter(dirField + "Flags.txt"))
            {
                try
                {
                    writer.WriteLine("$Flags");

                    int count2 = flagPts.Count;
                    writer.WriteLine(count2);

                    for (int i = 0; i < count2; i++)
                    {
                        writer.WriteLine(
                            flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].easting.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].northing.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].color.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].ID.ToString(CultureInfo.InvariantCulture) + "," +
                            flagPts[i].flgtxt.ToString(CultureInfo.InvariantCulture));
                    }
                }

                catch (Exception e)
                {
                    Console.WriteLine(e.Message + "\n Cannot write to file.");
                    WriteErrorLog("Saving Flags" + e.ToString());
                    return;
                }
            }
        }

        //save all the flag markers
        //public void FileSaveABLine()
        //{
        //    //Saturday, February 11, 2017  -->  7:26:52 AM

        //    //get the directory and make sure it exists, create if not
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";

        //    string directoryName = Path.GetDirectoryName(dirField);
        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    //use Streamwriter to create and overwrite existing ABLine file
        //    using (StreamWriter writer = new StreamWriter(dirField + "ABLine.txt"))
        //    {
        //        try
        //        {
        //            //write out the ABLine
        //            writer.WriteLine("$ABLine");

        //            //true or false if ABLine is set
        //            if (ABLine.isABLineSet) writer.WriteLine(true);
        //            else writer.WriteLine(false);

        //            writer.WriteLine(ABLine.abHeading.ToString(CultureInfo.InvariantCulture));
        //            writer.WriteLine(ABLine.refPoint1.easting.ToString(CultureInfo.InvariantCulture) + "," + ABLine.refPoint1.northing.ToString(CultureInfo.InvariantCulture));
        //            writer.WriteLine(ABLine.refPoint2.easting.ToString(CultureInfo.InvariantCulture) + "," + ABLine.refPoint2.northing.ToString(CultureInfo.InvariantCulture));
        //            writer.WriteLine(ABLine.tramPassEvery.ToString(CultureInfo.InvariantCulture) + "," + ABLine.passBasedOn.ToString(CultureInfo.InvariantCulture));
        //        }

        //        catch (Exception e)
        //        {
        //            Console.WriteLine(e.Message + "\n Cannot write to file.");
        //            WriteErrorLog("Saving AB Line" + e.ToString());

        //            return;
        //        }

        //    }
        //}

        //save all the flag markers
        //public void FileSaveCurveLine()
        //{
        //    //Saturday, February 11, 2017  -->  7:26:52 AM

        //    //get the directory and make sure it exists, create if not
        //    string dirField = fieldsDirectory + currentFieldDirectory + "\\";

        //    string directoryName = Path.GetDirectoryName(dirField);
        //    if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
        //    { Directory.CreateDirectory(directoryName); }

        //    //use Streamwriter to create and overwrite existing ABLine file
        //    using (StreamWriter writer = new StreamWriter(dirField + "CurveLine.txt"))
        //    {
        //        try
        //        {
        //            //write out the ABLine
        //            writer.WriteLine("$CurveLine");

        //            //write out the aveheading
        //            writer.WriteLine(curve.aveLineHeading.ToString(CultureInfo.InvariantCulture));

        //            //write out the points of ref line
        //            writer.WriteLine(curve.refList.Count.ToString(CultureInfo.InvariantCulture));
        //            if (curve.refList.Count > 0)
        //            {
        //                for (int j = 0; j < curve.refList.Count; j++)
        //                    writer.WriteLine(Math.Round(curve.refList[j].easting, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                        Math.Round(curve.refList[j].northing, 3).ToString(CultureInfo.InvariantCulture) + "," +
        //                                            Math.Round(curve.refList[j].heading, 5).ToString(CultureInfo.InvariantCulture));
        //            }
        //        }

        //        catch (Exception e)
        //        {
        //            WriteErrorLog("Saving Curve Line" + e.ToString());

        //            return;
        //        }

        //    }
        //}

        //save nmea sentences
        public void FileSaveNMEA()
        {
            using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\NMEA_log.txt"), true))
            {
                writer.Write(pn.logNMEASentence.ToString());
            }
            pn.logNMEASentence.Clear();
        }

        //save nmea sentences
        public void FileSaveElevation()
        {
            using (StreamWriter writer = new StreamWriter((fieldsDirectory + currentFieldDirectory + "\\Elevation.txt"), true))
            {
                writer.Write(sbFix.ToString());
            }
            sbFix.Clear();
        }

        //generate KML file from flags
        public void FileSaveFlagsKML()
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flags.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                for (int i = 0; i < count2; i++)
                {
                    writer.WriteLine(@"  <Placemark>                                  ");
                    writer.WriteLine(@"<Style> <IconStyle>");
                    if (flagPts[i].color == 0)  //red - xbgr
                        writer.WriteLine(@"<color>ff4400ff</color>");
                    if (flagPts[i].color == 1)  //grn - xbgr
                        writer.WriteLine(@"<color>ff44ff00</color>");
                    if (flagPts[i].color == 2)  //yel - xbgr
                        writer.WriteLine(@"<color>ff44ffff</color>");

                    writer.WriteLine(@"</IconStyle> </Style>");
                    writer.WriteLine(@" <name> " + (i + 1) + " " + flagPts[i].flgtxt + @"</name>");
                    writer.WriteLine(@"<Point><coordinates> " +
                                    flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                    @"</coordinates> </Point> ");
                    writer.WriteLine(@"  </Placemark>                                 ");

                }

                writer.WriteLine(@"</Document>");

                writer.WriteLine(@"</kml>                                         ");



            }

        }

        public void BuildBoundaryLATLON()
        {
            int ptCount = bnd.bndArr[0].bndLine.Count;
            bndkml = new List<CBndPt>();
            double XO = 0;
            double YO = 0;

            if (ptCount > 1)
            {

                for (int h = 0; h < ptCount; h++)
                {
                    XO = bnd.bndArr[0].bndLine[h].easting;
                    YO = bnd.bndArr[0].bndLine[h].northing;
                    BuildPointLATLON(XO, YO);
                    CBndPt vecPt = new CBndPt(utmLat, utmLon, 0);

                    
                    bndkml.Add(vecPt);//builds the boundary list.  Does not connect front to rear.
                }

                
            }
        }
        public void BuildCutList()//Three times bigger than needed so you don't "fall off" the field
        {
            BuildPointLATLON(minFieldX - maxFieldDistance, minFieldY - maxFieldDistance);
            CBndPt vecPt = new CBndPt(utmLat, utmLon, 0);
            cutList.Add(vecPt);
            BuildPointLATLON(maxFieldX + maxFieldDistance, minFieldY - maxFieldDistance);
            vecPt = new CBndPt(utmLat, utmLon, 0);
            cutList.Add(vecPt);
            BuildPointLATLON(maxFieldX + maxFieldDistance, maxFieldY + maxFieldDistance);
            vecPt = new CBndPt(utmLat, utmLon, 0);
            cutList.Add(vecPt);
            BuildPointLATLON(minFieldX - maxFieldDistance, maxFieldY + maxFieldDistance);
            vecPt = new CBndPt(utmLat, utmLon, 0);
            cutList.Add(vecPt);
            BuildPointLATLON(minFieldX - maxFieldDistance, minFieldY - maxFieldDistance);
            vecPt = new CBndPt(utmLat, utmLon, 0);
            cutList.Add(vecPt);
            
        }

        public void BuildPointLATLON(double easting, double northing)
        {
            double XO = 0;
            double YO = 0;
            XO = (Math.Cos(pn.convergenceAngle) * easting) - (Math.Sin(pn.convergenceAngle) * northing);
            YO = (Math.Sin(pn.convergenceAngle) * easting) + (Math.Cos(pn.convergenceAngle) * northing);
            XO = XO + pn.utmEast;
            YO = YO + pn.utmNorth;
            UTMToLatLon(XO, YO);
        }

        public void FileSaveJobKML()//called at field closing
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "AOGkml" + "-" + (DateTime.Now.ToString("yyyyMMddhhmmss")) + ".kml";

            //myFileName = "AOGkml"+"-"+(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))+".kml";
            driveGroupList.Add(driveList);
            if (bnd.bndArr.Count>0)
            {
                BuildBoundaryLATLON();
                BuildCutList();
            }



            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" ");
                writer.WriteLine(@"xmlns:gx=""http://www.google.com/kml/ext/2.2"">");

                int count1 = driveGroupList.Count;//not needed
                int count2 = autoGroupList.Count;//not needed
                int count3 = manualGroupList.Count;//not needed
                int count7 = bndkml.Count;//not needed
                int count9 = flagPts.Count;//defined here but changed later.
                //All the counts could be done from one count, but this was eaiser to follow




                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"  <name> AOGkml-" + (DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture))+ "</name>");

                //Style for Total path
                writer.WriteLine(@"<Style id = ""PathStyle"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color> ff000000 </color>");//Black at 100%

                writer.WriteLine(@"<width> 1 </width>");//Just a Line

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");

                //Style for Just driving around blue
                writer.WriteLine(@"<Style id = ""DriveStyle"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color> ffff0000 </color>");//Blue at 100%

                writer.WriteLine(@"<width> 2 </width>");//Just a Line

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");

                //Style for auto tool green
                writer.WriteLine(@"<Style id = ""AutoStyle"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color> 5000ff00 </color>");//green at 50%
                writer.WriteLine(@"<gx:physicalWidth>"+ vehicle.toolWidth +"</gx:physicalWidth>");

                //writer.WriteLine(@" < width> 5 </width>");//Just a single line

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");

                //Style for Manual yellow
                writer.WriteLine(@"<Style id = ""ManualStyle"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color> 5000ffff </color>");//yellow at 50%
                writer.WriteLine(@"<gx:physicalWidth>" + vehicle.toolWidth + "</gx:physicalWidth>");
                //writer.WriteLine(@"<width> 5 </width>");//Just a single line

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");

                //Style for Boundary Purple
                writer.WriteLine(@"<Style id = ""BoundaryStyle"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color>  ff800080 </color>");//purple at full

                writer.WriteLine(@"<width> 5 </width>");

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");

                //Style for CutLine
                writer.WriteLine(@"<Style id = ""CutStyle"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color>  500000ff </color>");//red at 50%

                writer.WriteLine(@"<width> 1 </width>");

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");

                //Build total pathList
                writer.WriteLine(@"  <Placemark>");
                writer.WriteLine(@"<name> Total Job </name>");
                writer.WriteLine(@"<styleUrl >#PathStyle</styleUrl>");
                writer.WriteLine(@"<LineString>");
                writer.WriteLine(@"<tessellate> 1 </tessellate>");
                writer.WriteLine(@" <coordinates>");
                int count4 = pathList.Count;

                for (int i = 0; i < count4; i++)
                {
                    writer.WriteLine(@pathList[i].northing + "," + pathList[i].easting + ",0");
                }
                writer.WriteLine(@"</coordinates>");
                writer.WriteLine(@"</LineString>");
                writer.WriteLine(@"  </Placemark>");

                foreach (var driveList in driveGroupList)//Build the Just Driving Line
                {
                    count4 = driveList.Count;
                    if (count4 >= 2)
                    {
                        writer.WriteLine(@"  <Placemark>");
                        writer.WriteLine(@"<name> Driving </name>");
                        writer.WriteLine(@"<styleUrl >#DriveStyle</styleUrl>");
                        writer.WriteLine(@"<LineString>");
                        writer.WriteLine(@"<tessellate> 1 </tessellate>");
                        writer.WriteLine(@" <coordinates>");


                        for (int i = 0; i < count4; i++)
                        {
                            writer.WriteLine(@driveList[i].northing + "," + driveList[i].easting + ",0");
                        }
                        writer.WriteLine(@"</coordinates>");
                        writer.WriteLine(@"</LineString>");
                        writer.WriteLine(@"  </Placemark>");
                    }
                }
                foreach (var driveList in autoGroupList)//Build the Auto Tool
                {

                    writer.WriteLine(@"  <Placemark>");
                    writer.WriteLine(@"<name> Auto Tool </name>");
                    writer.WriteLine(@"<styleUrl >#AutoStyle</styleUrl>");
                    writer.WriteLine(@"<LineString>");
                    writer.WriteLine(@"<tessellate> 1 </tessellate>");
                    writer.WriteLine(@" <coordinates>");
                    int count5 = driveList.Count;

                    for (int i = 0; i < count5; i++)
                    {
                        writer.WriteLine(@driveList[i].northing + "," + driveList[i].easting + ",0");
                    }
                    writer.WriteLine(@"</coordinates>");
                    writer.WriteLine(@"</LineString>");
                    writer.WriteLine(@"  </Placemark>");
                }
                foreach (var driveList in manualGroupList)//Build the Manual Tool
                {

                    writer.WriteLine(@"  <Placemark>");
                    writer.WriteLine(@"<name> Manual Tool </name>");
                    writer.WriteLine(@"<styleUrl >#ManualStyle</styleUrl>");
                    writer.WriteLine(@"<LineString>");
                    writer.WriteLine(@"<tessellate> 1 </tessellate>");
                    writer.WriteLine(@" <coordinates>");
                    int count6 = driveList.Count;

                    for (int i = 0; i < count6; i++)
                    {
                        writer.WriteLine(@driveList[i].northing + "," + driveList[i].easting + ",0");
                    }
                    writer.WriteLine(@"</coordinates>");
                    writer.WriteLine(@"</LineString>");
                    writer.WriteLine(@"  </Placemark>");
                }

                // Boundary

                if (bndkml.Count > 0)
                {

                    writer.WriteLine(@"  <Placemark>");
                    writer.WriteLine(@"<name> Boundary </name>");
                    writer.WriteLine(@"<styleUrl >#BoundaryStyle</styleUrl>");
                    writer.WriteLine(@"<LineString>");
                    writer.WriteLine(@"<tessellate> 1 </tessellate>");
                    writer.WriteLine(@" <coordinates>");
                    int count8 = bndkml.Count;

                    for (int i = 0; i < count8; i++)
                    {
                        writer.WriteLine(@bndkml[i].northing + "," + bndkml[i].easting + ",0");
                    }
                    writer.WriteLine(@bndkml[0].northing + "," + bndkml[0].easting + ",0");//Close the loop
                    writer.WriteLine(@"</coordinates>");
                    writer.WriteLine(@"</LineString>");
                    writer.WriteLine(@"  </Placemark>");
                }
                //Flags

                for (int i = 0; i < count9; i++)
                {
                    writer.WriteLine(@"  <Placemark>                                  ");
                    writer.WriteLine(@"<Style> <IconStyle>");
                    if (flagPts[i].color == 0)  //red - xbgr
                        writer.WriteLine(@"<color>ff4400ff</color>");
                    if (flagPts[i].color == 1)  //grn - xbgr
                        writer.WriteLine(@"<color>ff44ff00</color>");
                    if (flagPts[i].color == 2)  //yel - xbgr
                        writer.WriteLine(@"<color>ff44ffff</color>");

                    writer.WriteLine(@"</IconStyle> </Style>");
                    writer.WriteLine(@" <name> " + (i + 1) + " " + flagPts[i].flgtxt + @"</name>");
                    writer.WriteLine(@"<Point><coordinates> " +
                                    flagPts[i].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[i].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                    @"</coordinates> </Point> ");
                    writer.WriteLine(@"  </Placemark>                                 ");

                }
                //Cut Area
                

                writer.WriteLine(@"  <Placemark>");
                writer.WriteLine(@"<name> Cut Area </name>");
                writer.WriteLine(@"<styleUrl >#CutStyle</styleUrl>");
                writer.WriteLine(@"<LineString>");
                writer.WriteLine(@"<tessellate> 1 </tessellate>");
                writer.WriteLine(@" <coordinates>");
                count9= cutList.Count;

                for (int i = 0; i < count9; i++)
                {
                    writer.WriteLine(@cutList[i].northing + "," + cutList[i].easting + ",0");
                }
                writer.WriteLine(@"</coordinates>");
                writer.WriteLine(@"</LineString>");
                writer.WriteLine(@"  </Placemark>");

                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");
            }

        }
        public void FileSaveBndryKML()//called at field closing
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "BoundaryKml.kml";

            //myFileName = "AOGkml"+"-"+(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture))+".kml";
           
            if (bnd.bndArr.Count > 0)
            {
                BuildBoundaryLATLON();
            }
            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2"" ");
                writer.WriteLine(@"xmlns:gx=""http://www.google.com/kml/ext/2.2"">");

                
                writer.WriteLine(@"<Document>");
                writer.WriteLine(@"  <name> BoundaryKml-" + (DateTime.Now.ToString("yyyyMMddhhmmss", CultureInfo.InvariantCulture)) + "</name>");


                //Style for Boundary Red
                writer.WriteLine(@"<Style id = ""BoundaryStyleRed"">");

                writer.WriteLine(@"<LineStyle>");

                writer.WriteLine(@"<color>  ff0000ff </color>");//red at full

                writer.WriteLine(@"<width> 5 </width>");

                writer.WriteLine(@" </LineStyle>");

                writer.WriteLine(@"</Style>");


                // Boundary

                if (bndkml.Count > 0)
                {

                    writer.WriteLine(@"  <Placemark>");
                    writer.WriteLine(@"<name> Boundary </name>");
                    writer.WriteLine(@"<styleUrl >#BoundaryStyleRed</styleUrl>");
                    writer.WriteLine(@"<LineString>");
                    writer.WriteLine(@"<tessellate> 1 </tessellate>");
                    writer.WriteLine(@" <coordinates>");
                    int count8 = bndkml.Count;

                    for (int i = 0; i < count8; i++)
                    {
                        writer.WriteLine(@bndkml[i].northing + "," + bndkml[i].easting + ",0");
                    }
                    writer.WriteLine(@bndkml[0].northing + "," + bndkml[0].easting + ",0");//Close the loop
                    writer.WriteLine(@"</coordinates>");
                    writer.WriteLine(@"</LineString>");
                    writer.WriteLine(@"  </Placemark>");
                }
                
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");
            }

        }

        //generate KML file from flag
        public void FileSaveSingleFlagKML(int flagNumber,string flgtext)
        {

            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }

            string myFileName;
            myFileName = "Flag.kml";

            using (StreamWriter writer = new StreamWriter(dirField + myFileName))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                    writer.WriteLine(@"  <Placemark>                                  ");
                    writer.WriteLine(@"<Style> <IconStyle>");
                    if (flagPts[flagNumber - 1].color == 0)  //red - xbgr
                        writer.WriteLine(@"<color>ff4400ff</color>");
                    if (flagPts[flagNumber - 1].color == 1)  //grn - xbgr
                        writer.WriteLine(@"<color>ff44ff00</color>");
                    if (flagPts[flagNumber - 1].color == 2)  //yel - xbgr
                        writer.WriteLine(@"<color>ff44ffff</color>");
                    writer.WriteLine(@"</IconStyle> </Style>");
                    writer.WriteLine(@" <name> " + flagNumber.ToString(CultureInfo.InvariantCulture) + " " + flgtext + @"</name>");
                    writer.WriteLine(@"<Point><coordinates> " +
                                    flagPts[flagNumber-1].longitude.ToString(CultureInfo.InvariantCulture) + "," + flagPts[flagNumber-1].latitude.ToString(CultureInfo.InvariantCulture) + ",0" +
                                    @"</coordinates> </Point> ");
                    writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }

        //generate KML file from flag
        public void FileMakeCurrentKML(double lat, double lon)
        {
            //get the directory and make sure it exists, create if not
            string dirField = fieldsDirectory + currentFieldDirectory + "\\";

            string directoryName = Path.GetDirectoryName(dirField);
            if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
            { Directory.CreateDirectory(directoryName); }


            using (StreamWriter writer = new StreamWriter(dirField + "CurrentPosition.kml"))
            {

                writer.WriteLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>     ");
                writer.WriteLine(@"<kml xmlns=""http://www.opengis.net/kml/2.2""> ");

                int count2 = flagPts.Count;

                writer.WriteLine(@"<Document>");

                writer.WriteLine(@"  <Placemark>                                  ");
                writer.WriteLine(@"<Style> <IconStyle>");
                writer.WriteLine(@"<color>ff4400ff</color>");
                writer.WriteLine(@"</IconStyle> </Style>");
                writer.WriteLine(@" <name> Your Current Position </name>");
                writer.WriteLine(@"<Point><coordinates> " +
                                lon.ToString(CultureInfo.InvariantCulture) + "," + lat.ToString(CultureInfo.InvariantCulture) + ",0" +
                                @"</coordinates> </Point> ");
                writer.WriteLine(@"  </Placemark>                                 ");
                writer.WriteLine(@"</Document>");
                writer.WriteLine(@"</kml>                                         ");

            }
        }
    }
}
﻿using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace AgOpenGPS.Properties
{
    public static class RegistrySettings
    {
        public static string culture = "en";
        public static string WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string VehicleFileName = "Default Vehicle";

        public static void Load()
        {
            try
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                ////create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                    //storing the values
                    Key.SetValue("Language", "en");
                    Key.SetValue("WorkingDirectory", "Default");
                    Key.SetValue("VehicleFileName", "Default Vehicle");

                    Key.Close();
                }
                else
                {
                    //check for corrupt settings file
                    try
                    {
                        object dir = regKey.GetValue("WorkingDirectory");
                        if (dir != null && dir.ToString() != "Default")
                            WorkingDirectory = dir.ToString();

                        object name = regKey.GetValue("VehicleFileName");
                        if (name != null)
                            VehicleFileName = name.ToString();

                        var lang = regKey.GetValue("Language");
                        if (lang != null)
                            culture = lang.ToString();
                    }
                    catch (Exception)
                    {
                    }
                    regKey.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey  
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                if (name == "VehicleFileName")
                    VehicleFileName = value;

                if (name == "Directory" && value == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                    key.SetValue(name, "Default");
                else//storing the value
                    key.SetValue(name, value);

                key.Close();
            }
            catch (Exception)
            {
            }
        }

        public static void Reset()
        {
            try
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                regKey.SetValue("Language", "en");
                regKey.SetValue("WorkingDirectory", "Default");
                regKey.SetValue("VehicleFileName", "Default Vehicle");

                regKey.Close();
            }
            catch (Exception)
            {
            }
        }
    }

    public sealed class Settings
    {
        private static Settings settings_ = new Settings();
        public static Settings Default
        {
            get
            {
                return settings_;
            }
        }

        public Point setWindow_Location = new Point(30, 30);
        public Size setWindow_Size = new Size(1005, 730);
        public bool setWindow_Maximized = false;
        public bool setWindow_Minimized = false;
        public double setDisplay_triangleResolution = 1.0;
        public bool setMenu_isMetric = true;
        public bool setMenu_isGridOn = true;
        public bool setMenu_isLightbarOn = true;
        public string setF_CurrentDir = "";
        public bool setF_isWorkSwitchEnabled = false;
        public int setIMU_pitchZeroX16 = 0;
        public double setIMU_rollZero = 0.0;
        public double setF_minHeadingStepDistance = 0.5;
        public byte setAS_lowSteerPWM = 30;
        public int setAS_wasOffset = 3;
        public double setF_UserTotalArea = 0.0;
        public byte setAS_minSteerPWM = 25;
        public double setF_boundaryTriggerDistance = 1.0;
        public byte setAS_highSteerPWM = 180;
        public bool setMenu_isSideGuideLines = false;
        public byte setAS_countsPerDegree = 110;
        public bool setMenu_isPureOn = true;
        public bool setMenu_isSimulatorOn = true;
        public bool setMenu_isSkyOn = true;
        public int setDisplay_lightbarCmPerPixel = 5;
        public string setGPS_headingFromWhichSource = "Fix";
        public double setGPS_SimLatitude = 53.4360564;
        public double setGPS_SimLongitude = -111.160047;
        public double setAS_snapDistance = 20.0;
        public bool setF_isWorkSwitchManualSections = false;
        public bool setAS_isAutoSteerAutoOn = false;
        public int setDisplay_lineWidth = 2;
        public Point setDisplay_panelSimLocation = new Point(97, 600);
        public double setTram_tramWidth = 24.0;
        public double setTram_snapAdj = 1.0;
        public int setTram_passes = 1;
        public double setTram_offset = 0.0;
        public int setMenu_isOGLZoomOn = 0;
        public bool setMenu_isCompassOn = true;
        public bool setMenu_isSpeedoOn = false;
        public Color setDisplay_colorDayFrame = Color.FromArgb(210, 210, 230);
        public Color setDisplay_colorNightFrame = Color.FromArgb(50, 50, 65);
        public Color setDisplay_colorSectionsDay = Color.FromArgb(27, 151, 160);
        public Color setDisplay_colorFieldDay = Color.FromArgb(100, 100, 125);
        public bool setDisplay_isDayMode = true;
        public Color setDisplay_colorSectionsNight = Color.FromArgb(27, 100, 100);
        public Color setDisplay_colorFieldNight = Color.FromArgb(60, 60, 60);
        public bool setDisplay_isAutoDayNight = false;
        public string setDisplay_customColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";
        public bool setDisplay_isTermsAccepted = false;
        public bool setGPS_isRTK = false;
        public bool setDisplay_isStartFullScreen = false;
        public bool setDisplay_isKeyboardOn = true;
        public double setIMU_rollFilter = 0.0;
        public int setAS_uTurnSmoothing = 14;
        public bool setIMU_invertRoll = false;
        public byte setAS_ackerman = 100;
        public bool setF_isWorkSwitchActiveLow = true;
        public byte setAS_Kp = 50;
        public bool setSound_isUturnOn = true;
        public bool setSound_isHydLiftOn = true;
        public Color setDisplay_colorTextNight = Color.FromArgb(230, 230, 230);
        public Color setDisplay_colorTextDay = Color.FromArgb(10, 10, 20);
        public bool setTram_isTramOnBackBuffer = true;
        public double setDisplay_camZoom = 9.0;
        public Color setDisplay_colorVehicle = Color.White;
        public int setDisplay_vehicleOpacity = 100;
        public bool setDisplay_isVehicleImage = true;
        public bool setIMU_isHeadingCorrectionFromAutoSteer = false;
        public bool setDisplay_isTextureOn = true;
        public double setAB_lineLength = 1600.0;
        public int SetGPS_udpWatchMsec = 50;
        public bool setF_isSteerWorkSwitchManualSections = false;
        public bool setAS_isConstantContourOn = false;
        public double setAS_guidanceLookAheadTime = 1.5;
        public CFeatureSettings setFeatures = new CFeatureSettings();
        public bool setIMU_isDualAsIMU = false;
        public double setAS_sideHillComp = 0.0;
        public bool setIMU_isReverseOn = true;
        public double setGPS_forwardComp = 0.15;
        public double setGPS_reverseComp = 0.3;
        public int setGPS_ageAlarm = 20;
        public bool setGPS_isRTK_KillAutoSteer = false;
        public Color setColor_sec01 = Color.FromArgb(249, 22, 10);
        public Color setColor_sec02 = Color.FromArgb(68, 84, 254);
        public Color setColor_sec03 = Color.FromArgb(8, 243, 8);
        public Color setColor_sec04 = Color.FromArgb(233, 6, 233);
        public Color setColor_sec05 = Color.FromArgb(200, 191, 86);
        public Color setColor_sec06 = Color.FromArgb(0, 252, 246);
        public Color setColor_sec07 = Color.FromArgb(144, 36, 246);
        public Color setColor_sec08 = Color.FromArgb(232, 102, 21);
        public Color setColor_sec09 = Color.FromArgb(255, 160, 170);
        public Color setColor_sec10 = Color.FromArgb(205, 204, 246);
        public Color setColor_sec11 = Color.FromArgb(213, 239, 190);
        public Color setColor_sec12 = Color.FromArgb(247, 200, 247);
        public Color setColor_sec13 = Color.FromArgb(253, 241, 144);
        public Color setColor_sec14 = Color.FromArgb(187, 250, 250);
        public Color setColor_sec15 = Color.FromArgb(227, 201, 249);
        public Color setColor_sec16 = Color.FromArgb(247, 229, 215);
        public bool setColor_isMultiColorSections = false;
        public string setDisplay_customSectionColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";
        public TBrand setBrand_TBrand = TBrand.AGOpenGPS;
        public bool setHeadland_isSectionControlled = true;
        public bool setSound_isAutoSteerOn = true;
        public string setRelay_pinConfig = "1,2,3,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0";
        public int setDisplay_camSmooth = 50;
        public double setGPS_dualHeadingOffset = 0.0;
        public bool setF_isSteerWorkSwitchEnabled = false;
        public bool setF_isRemoteWorkSystemOn = false;
        public bool setDisplay_isAutoStartAgIO = true;
        public double setAS_ModeXTE = 0.1;
        public int setAS_ModeTime = 1;
        public double setVehicle_toolWidth = 4.0;
        public double setVehicle_toolOverlap = 0.0;
        public double setTool_toolTrailingHitchLength = -2.5;
        public int setVehicle_numSections = 3;
        public decimal setSection_position1 = -2;
        public decimal setSection_position2 = -1;
        public decimal setSection_position3 = 1;
        public decimal setSection_position4 = 2;
        public decimal setSection_position5 = 0;
        public decimal setSection_position6 = 0;
        public decimal setSection_position7 = 0;
        public decimal setSection_position8 = 0;
        public decimal setSection_position9 = 0;
        public decimal setSection_position10 = 0;
        public decimal setSection_position11 = 0;
        public decimal setSection_position12 = 0;
        public decimal setSection_position13 = 0;
        public decimal setSection_position14 = 0;
        public decimal setSection_position15 = 0;
        public decimal setSection_position16 = 0;
        public decimal setSection_position17 = 0;
        public double purePursuitIntegralGainAB = 0;
        public double set_youMoveDistance = 0.25;
        public double setVehicle_antennaHeight = 3;
        public double setVehicle_toolLookAheadOn = 1;
        public bool setTool_isToolTrailing = true;
        public double setVehicle_toolOffset = 0;
        public bool setTool_isToolRearFixed = false;
        public double setVehicle_antennaPivot = 0.1;
        public double setVehicle_wheelbase = 3.3;
        public double setVehicle_hitchLength = -1;
        public double setVehicle_toolLookAheadOff = 0.5;
        public bool setVehicle_isPivotBehindAntenna = true;
        public bool setVehicle_isSteerAxleAhead = true;
        public double setVehicle_slowSpeedCutoff = 0.5;
        public double setVehicle_tankTrailingHitchLength = 3;
        public int setVehicle_minCoverage = 100;
        public double setVehicle_goalPointLookAhead = 3;
        public double setVehicle_maxAngularVelocity = 0.64;
        public double setVehicle_maxSteerAngle = 30;
        public int set_youTurnExtensionLength = 20;
        public double set_youToolWidths = 2;
        public double setVehicle_minTurningRadius = 8.1;
        public double setVehicle_antennaOffset = 0;
        public double set_youTurnDistanceFromBoundary = 2;
        public double setVehicle_lookAheadMinimum = 2;
        public double setVehicle_goalPointLookAheadMult = 1.5;
        public double stanleyDistanceErrorGain = 1;
        public double stanleyHeadingErrorGain = 1;
        public bool setVehicle_isStanleyUsed = false;
        public int setTram_BasedOn = 0;
        public int setTram_Skips = 0;
        public bool setTool_isToolTBT = false;
        public int setVehicle_vehicleType = 0;
        public int set_youSkipWidth = 1;
        public byte setArdSteer_setting1 = 0;
        public byte setArdSteer_minSpeed = 0;
        public byte setArdSteer_maxSpeed = 20;
        public byte setArdSteer_setting0 = 56;
        public double setVehicle_hydraulicLiftLookAhead = 2;
        public bool setVehicle_isMachineControlToAutoSteer = false;
        public byte setArdSteer_maxPulseCounts = 3;
        public byte setArdMac_hydRaiseTime = 3;
        public byte setArdMac_hydLowerTime = 4;
        public byte setArdMac_isHydEnabled = 0;
        public double setTool_defaultSectionWidth = 2;
        public double setVehicle_toolOffDelay = 0;
        public byte setArdMac_setting0 = 0;
        public byte setArdSteer_setting2 = 0;
        public double stanleyIntegralDistanceAwayTriggerAB = 0.25;
        public bool setTool_isToolFront = false;
        public double setVehicle_trackWidth = 1.9;
        public bool setArdMac_isDanfoss = false;
        public double stanleyIntegralGainAB = 0;
        public bool setSection_isFast = true;
        public byte setArdMac_user1 = 1;
        public byte setArdMac_user2 = 2;
        public byte setArdMac_user3 = 3;
        public byte setArdMac_user4 = 4;
        public double setVehicle_panicStopSpeed = 0;
        public double setAS_ModeMultiplierStanley = 0.6;
        public int setDisplay_brightness = 40;
        public double set_youTurnRadius = 8.1;
        public int setDisplay_brightnessSystem = 40;
        public bool setTool_isSectionsNotZones = true;
        public int setTool_numSectionsMulti = 20;
        public string setTool_zones = "2,10,20,0,0,0,0,0,0";
        public double setTool_sectionWidthMulti = 0.5;
        public bool setDisplay_isBrightnessOn = false;
        public string setKey_hotkeys = "ACFGMNPTYVW12345678";
        public double setVehicle_goalPointLookAheadHold = 3;
        public bool setTool_isSectionOffWhenOut = true;
        public int set_uTurnStyle = 0;
        public double setGPS_minimumStepLimit = 0.05;
        public bool setAS_isSteerInReverse = false;
        public double setAS_functionSpeedLimit = 12;
        public double setAS_maxSteerSpeed = 15;
        public double setAS_minSteerSpeed = 0;
        public HBrand setBrand_HBrand = HBrand.AgOpenGPS;
        public WDBrand setBrand_WDBrand = WDBrand.AgOpenGPS;
        public double setIMU_fusionWeight2 = 0.06;
        public bool setDisplay_isSvennArrowOn = false;
        public bool setTool_isTramOuterInverted = false;
        public Point setJobMenu_location = new Point(200, 200);
        public Size setJobMenu_size = new Size(640, 530);
        public Point setWindow_steerSettingsLocation = new Point(40, 40);
        public Point setWindow_buildTracksLocation = new Point(40, 40);
        public double setTool_trailingToolToPivotLength = 0;
        public Point setWindow_formNudgeLocation = new Point(200, 200);
        public Size setWindow_formNudgeSize = new Size(200, 400);
        public double setAS_snapDistanceRef = 5;
        public string setDisplay_buttonOrder = "0,1,2,3,4,5,6,7";
        public double setDisplay_camPitch = -62;
        public Size setWindow_abDrawSize = new Size(1022, 742);
        public Size setWindow_HeadlineSize = new Size(1022, 742);
        public Size setWindow_HeadAcheSize = new Size(1022, 742);
        public Size setWindow_MapBndSize = new Size(1022, 742);
        public Size setWindow_BingMapSize = new Size(965, 700);
        public int setWindow_BingZoom = 15;
        public Size setWindow_RateMapSize = new Size(1022, 742);
        public int setWindow_RateMapZoom = 15;
        public Point setWindow_QuickABLocation = new Point(100, 100);
        public bool setDisplay_isLogElevation = false;
        public bool setSound_isSectionsOn = true;
        public double setGPS_dualReverseDetectionDistance = 0.25;
        public bool setTool_isDisplayTramControl = true;
        public double setAS_uTurnCompensation = 1;
        public Size setWindow_gridSize = new Size(400, 400);
        public Point setWindow_gridLocation = new Point(20, 20);
        public bool setWindow_isKioskMode = false;
        public bool setDisplay_isAutoOffAgIO = true;
        public bool setWindow_isShutdownComputer = false;
        public bool setDisplay_isShutdownWhenNoPower = false;
        public bool setDisplay_isHardwareMessages = false;
        public int setGPS_jumpFixAlarmDistance = 0;
        public int setAS_deadZoneDistance = 1;
        public int setAS_deadZoneHeading = 10;
        public bool setMenu_isLightbarNotSteerBar = false;
        public bool setTool_isDirectionMarkers = true;
        public int setAS_numGuideLines = 10;
        public int setAS_deadZoneDelay = 5;
        public CNozzleSettings setNozzleSettings = new CNozzleSettings();
        public bool setApp_isNozzleApp = false;
        public double setTram_alpha = 0.8;
        public double setVehicle_goalPointAcquireFactor = 0.9;
        public bool setBnd_isDrawPivot = true;
        public bool setDisplay_isSectionLinesOn = true;
        public bool setDisplay_isLineSmooth = false;

        public DialogResult Load()
        {
            string path = Path.Combine(RegistrySettings.WorkingDirectory, "AgOpenGPS", "Vehicles", RegistrySettings.VehicleFileName + ".XML");
            var result = LoadXMLFile(path, this);
            if (result != DialogResult.OK)
                RegistrySettings.Save("VehicleFileName", "Default Vehicle");

            return result;
        }

        public void Save()
        {
            string path = Path.Combine(RegistrySettings.WorkingDirectory, "AgOpenGPS", "Vehicles", RegistrySettings.VehicleFileName + ".XML");

            if (RegistrySettings.VehicleFileName != "")
                SaveXMLFile(path, this);
        }

        public void Reset()
        {
            settings_ = new Settings();
            settings_.Save();
        }

        public static DialogResult LoadXMLFile(string filePath, object obj)
        {
            try
            {
                if (!File.Exists(filePath)) return DialogResult.Ignore;

                using (XmlTextReader reader = new XmlTextReader(filePath))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element && reader.Name == "setting")
                        {
                            string fieldName = reader.GetAttribute("name");
                            if (!string.IsNullOrEmpty(fieldName))
                            {
                                var fieldInfo = obj.GetType().GetField(fieldName);
                                if (fieldInfo != null && reader.ReadToDescendant("value"))
                                {
                                    if (!ProcessNode(reader, fieldInfo, ref obj))
                                        return DialogResult.No;
                                }
                            }
                        }
                    }
                    reader.Close();
                }
                return DialogResult.OK; // Successfully loaded the XML data
            }
            catch (Exception e)
            {
                //glm.WriteErrorLog(ex);
            }
            return DialogResult.No;// Return false if the file doesn't exist or an error occurs
        }

        private static bool ProcessNode(XmlTextReader reader, FieldInfo fieldInfo, ref object obj, Type dd = null)
        {
            int initialDepth = reader.Depth; // Track the starting depth for this node
            bool allOK = true;
            try
            {
                Type type;
                if (fieldInfo == null)
                {
                    type = dd;
                }
                else
                    type = fieldInfo.FieldType;

                if (type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>)))
                {
                    Type elementType = type.IsArray ? type.GetElementType() : type.GetGenericArguments()[0];

                    var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

                    while (reader.Read())
                    {
                        try
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                object element;
                                if (elementType.IsArray)
                                {
                                    Type elementType2 = elementType.GetElementType();
                                    element = Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType2));
                                }
                                else if (elementType.IsEnum)
                                {
                                    element = Enum.GetValues(elementType).GetValue(0);
                                }
                                else if (elementType == typeof(Point))
                                {
                                    element = new Point(0, 0);
                                }
                                else if (elementType == typeof(Size))
                                {
                                    element = new Size(0, 0);
                                }
                                else if (elementType == typeof(Color))
                                {
                                    element = Color.FromArgb(0, 0, 0);
                                }
                                else if (elementType == typeof(string))
                                {
                                    element = string.Empty;
                                }
                                else
                                    element = Activator.CreateInstance(elementType);

                                allOK &= ProcessNode(reader, null, ref element, elementType);

                                list.Add(element);
                            }
                        }
                        catch (Exception e)
                        {
                        
                        }

                        if (reader.Depth <= initialDepth)
                        {
                            break; // Exit when the end of this node's scope is reached
                        }
                    }

                    if (type.IsArray)
                    {
                        Array array = Array.CreateInstance(elementType, list.Count);
                        list.CopyTo(array, 0);
                        if (fieldInfo == null)
                            obj = array;
                        else
                            fieldInfo.SetValue(obj, array);
                    }
                    else if (fieldInfo == null)
                        obj = list;
                    else
                    {
                        fieldInfo.SetValue(obj, list);
                    }
                }
                else if (type.IsClass && type != typeof(string))
                {
                    var nestedObj = Activator.CreateInstance(type);

                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            var nestedField = nestedObj.GetType().GetField(reader.Name);
                            if (nestedField != null)
                            {
                                allOK &= ProcessNode(reader, nestedField, ref nestedObj);
                            }
                        }
                        
                        if (reader.Depth <= initialDepth)
                        {
                            break;
                        }
                    }
                    if (fieldInfo == null)
                        obj = nestedObj;
                    else
                        fieldInfo.SetValue(obj, nestedObj);
                }
                else
                {
                    string valueString = reader.ReadElementContentAsString();
                    if (ParseValueString(type, valueString, out object value))
                    {
                        if (fieldInfo == null)
                        {
                            obj = value;
                        }
                        else
                        {
                            fieldInfo.SetValue(obj, value);
                        }
                    }
                    else
                        allOK = false;
                }
            }
            catch (Exception ex)
            {
                // Skip to the end of this node's scope in case of an error
                while (reader.Depth > initialDepth && reader.Read()) { }

                return false;
            }
            return allOK;
        }

        private static bool ParseValueString(Type type, string valueString, out object value)
        {
            value = null;

            if (type == typeof(string))
            {
                value = valueString;
            }
            else if (type.IsPrimitive || type == typeof(decimal))
            {
                value = Convert.ChangeType(valueString, type, CultureInfo.InvariantCulture);
            }
            else if (type.IsEnum)
            {
                value = Enum.Parse(type, valueString);
            }
            else if (type == typeof(Point))
            {
                var parts = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out int x) && int.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out int y))
                {
                    value = new Point(x, y);
                }
            }
            else if (type == typeof(Size))
            {
                var parts = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out int width) && int.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out int height))
                {
                    value = new Size(width, height);
                }
            }
            else if (type == typeof(Color))
            {
                var parts = valueString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 3 && int.TryParse(parts[0], NumberStyles.Any, CultureInfo.InvariantCulture, out int r) && int.TryParse(parts[1], NumberStyles.Any, CultureInfo.InvariantCulture, out int g) && int.TryParse(parts[2], NumberStyles.Any, CultureInfo.InvariantCulture, out int b))
                {
                    value = Color.FromArgb(r, g, b);
                }
            }

            return value != null;
        }

        public static void SaveXMLFile(string filePath, object obj)
        {
            try
            {
                var dirName = Path.GetDirectoryName(filePath);
                if (!string.IsNullOrEmpty(dirName) && !Directory.Exists(dirName))
                {
                    Directory.CreateDirectory(dirName);
                }

                using (XmlTextWriter xml = new XmlTextWriter(filePath + ".tmp", Encoding.UTF8)
                {
                    Formatting = Formatting.Indented,
                    Indentation = 4
                })
                {
                    xml.WriteStartDocument();

                    // Start the root element
                    xml.WriteStartElement("configuration");
                    xml.WriteStartElement("userSettings");
                    xml.WriteStartElement("AgOpenGPS.Properties.Settings");

                    SerializeObjectToXml(xml, obj, 0);

                    // Close all open elements
                    xml.WriteEndElement(); // AgOpenGPS.Properties.Settings
                    xml.WriteEndElement(); // userSettings
                    xml.WriteEndElement(); // configuration

                    // End the document
                    xml.WriteEndDocument();
                    xml.Flush();
                }

                if (File.Exists(filePath))
                    File.Delete(filePath);

                if (File.Exists(filePath + ".tmp"))
                    File.Move(filePath + ".tmp", filePath);
            }
            catch (Exception e)
            {
                //glm.WriteErrorLog(ex);
            }
        }

        private static void SerializeObjectToXml(XmlTextWriter xml, object obj, int depth, FieldInfo field2 = null)
        {
            if (field2 != null && field2.FieldType.Name == "Object")
            {
                throw new Exception("cant save type object in xml");
            }

            try
            {
                if (obj == null)
                    return;

                if (depth == 8)//LIMIT
                    return;

                if (obj is IEnumerable enumerable && obj.GetType() != typeof(string))
                {
                    if (depth == 1)
                    {
                        xml.WriteAttributeString("serializeAs", "Xml");
                        xml.WriteStartElement("value");
                        xml.WriteStartElement(GenerateNestedElementName(obj.GetType()));
                        xml.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                        xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    }
                    else if (depth > 1)
                    {
                        if (field2 != null)
                            xml.WriteStartElement(field2.Name);
                        else
                            xml.WriteStartElement(GenerateNestedElementName(obj.GetType()));
                    }

                    foreach (var item in enumerable)
                    {
                        SerializeObjectToXml(xml, item, depth + 1);
                    }

                    xml.WriteEndElement(); // Close Array/List
                    if (depth == 1)
                    {
                        xml.WriteEndElement();
                    }
                }
                else if (obj.GetType().IsClass && obj.GetType() != typeof(string))
                {
                    if (depth == 1)
                    {
                        xml.WriteAttributeString("serializeAs", "Xml");
                        xml.WriteStartElement("value");
                        xml.WriteStartElement(obj.GetType().Name);
                        xml.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                        xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                    }
                    else if (depth > 1)
                    {
                        if (field2 != null)
                            xml.WriteStartElement(field2.Name);
                        else
                            xml.WriteStartElement(obj.GetType().Name);//ArrayOrList
                    }

                    foreach (var field in obj.GetType().GetFields())
                    {
                        if (depth == 0)
                        {
                            // Start a "setting" element
                            xml.WriteStartElement("setting");
                            // Add attributes to the "setting" element
                            xml.WriteAttributeString("name", field.Name);
                        }

                        var value = field.GetValue(obj);
                        if (value != null)
                            SerializeObjectToXml(xml, value, depth + 1, field);

                        if (depth == 0)
                        {
                            xml.WriteEndElement(); // Close class object
                        }
                    }

                    if (depth == 1)
                        xml.WriteEndElement();
                    if (depth > 0)
                        xml.WriteEndElement();
                }
                else
                {
                    if (depth == 1)
                        xml.WriteAttributeString("serializeAs", "String");

                    string name = depth == 1 ? "value" : field2 != null ? field2.Name : GetTypeCode(obj.GetType());

                    if (obj is Point pointValue)
                    {
                        xml.WriteElementString(name, $"{pointValue.X.ToString(CultureInfo.InvariantCulture)}, {pointValue.Y.ToString(CultureInfo.InvariantCulture)}");
                    }
                    else if (obj is Size sizeValue)
                    {
                        xml.WriteElementString(name, $"{sizeValue.Width.ToString(CultureInfo.InvariantCulture)}, {sizeValue.Height.ToString(CultureInfo.InvariantCulture)}");
                    }
                    else if (obj is Color dd)
                    {
                        xml.WriteElementString(name, $"{dd.R.ToString(CultureInfo.InvariantCulture)}, {dd.G.ToString(CultureInfo.InvariantCulture)}, {dd.B.ToString(CultureInfo.InvariantCulture)}");
                    }
                    else
                    {
                        // Write primitive types or strings
                        string stringValue = Convert.ToString(obj, CultureInfo.InvariantCulture);
                        xml.WriteElementString(name, stringValue);
                    }
                }
            }
            catch (Exception e)
            {
                // Handle exceptions (log or rethrow)
            }
        }

        private static string GenerateNestedElementName(Type type)
        {
            if (type.IsArray)
            {
                // If it's an array, recursively get the element type
                string innerName = GenerateNestedElementName(type.GetElementType());
                return "ArrayOf" + innerName;
            }

            if (type.IsGenericType)
            {
                // If it's a generic type (e.g., List<T>), recursively get the generic argument type
                string innerName = GenerateNestedElementName(type.GetGenericArguments()[0]);
                return "ArrayOf" + innerName;
            }

            // Handle primitive type names for readability
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int32: return "Int";
                case TypeCode.String: return "String";
                case TypeCode.Boolean: return "Bool";
                case TypeCode.Double: return "Double";
                case TypeCode.Single: return "Float";
                case TypeCode.Decimal: return "Decimal";
                case TypeCode.Int64: return "Long";
                case TypeCode.Int16: return "Short";
                case TypeCode.Byte: return "Byte";
                case TypeCode.Char: return "Char";
                default:
                    // If not a primitive type, return the type's simple name
                    return type.Name;
            }
        }

        private static string GetTypeCode(Type type)
        {
            // Handle primitive type names for readability
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Int32: return "int";
                case TypeCode.String: return "string";
                case TypeCode.Boolean: return "bool";
                case TypeCode.Double: return "double";
                case TypeCode.Single: return "float";
                case TypeCode.Decimal: return "decimal";
                case TypeCode.Int64: return "long";
                case TypeCode.Int16: return "short";
                case TypeCode.Byte: return "byte";
                case TypeCode.Char: return "char";
                default:
                    // If not a primitive type, return the type's simple name
                    return type.Name;
            }
        }
        
        public static string GetTypeDescription(Type type)
        {
            if (type == null)
                return "null";

            if (type.IsArray)
                return $"{GetTypeDescription(type.GetElementType())}[]"; // For array types

            if (type.IsGenericType)
            {
                var genericTypeDefinition = type.GetGenericTypeDefinition();
                if (genericTypeDefinition == typeof(List<>))
                {
                    var elementType = type.GetGenericArguments()[0];
                    return $"List<{GetTypeDescription(elementType)}>";
                }
            }

            // For other types, just return the full name
            return type.Name;
        }
    }
}
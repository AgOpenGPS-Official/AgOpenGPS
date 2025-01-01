using AgIO.Logging;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AgIO.Properties
{
    public static class RegistrySettings
    {
        public static string culture = "en";
        public static string WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string ProfileName = "";

        public static void Load()
        {
            try
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgIO");

                ////create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");

                    //storing the values
                    Key.SetValue("Language", "en");
                    Key.SetValue("WorkingDirectory", "Default");
                    Key.SetValue("ProfileName", "");

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

                        object name = regKey.GetValue("ProfileName");
                        if (name != null)
                            ProfileName = name.ToString();

                        var lang = regKey.GetValue("Language");
                        if (lang != null)
                            culture = lang.ToString();
                    }
                    catch (Exception e)
                    {
                        Log.System.Write("Registry Keys Settings Load Error: " + e.ToString());
                    }
                    regKey.Close();
                }
            }
            catch (Exception e)
            {
                Log.System.Write("Registry Key Creation Error: " + e.ToString());
            }
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey  
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");

                if (name == "ProfileName")
                    ProfileName = value;

                if (name == "Directory" && value == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                    key.SetValue(name, "Default");
                else//storing the value
                    key.SetValue(name, value);

                key.Close();
            }
            catch (Exception e)
            {
                Log.System.Write("Registry Settings Save Error: " + e.ToString());

            }
        }

        public static void Reset()
        {
            try
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgIO");

                regKey.SetValue("Language", "en");
                regKey.SetValue("WorkingDirectory", "Default");
                regKey.SetValue("ProfileName", "Default Profile");

                regKey.Close();
            }
            catch (Exception e)
            {
                Log.System.Write("Registry Settings Reset Error: " + e.ToString());
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

        // General application settings
        public string setPort_portNameGPS = "GPS**";
        public int setPort_baudRateGPS = 9600;
        public bool setUDP_isOn = false;
        public bool setPort_wasSteerModuleConnected = false;
        public string setPort_portNameSteer = "Steer*";
        public bool setPort_wasModule3Connected = false;
        public string setPort_portNameTool = "Tool**";
        public int setIP_thisPort = 9999;
        public int setIP_autoSteerPort = 8888;

        // NTRIP settings
        public string setNTRIP_casterIP = "69.75.31.235";
        public int setNTRIP_casterPort = 2101;
        public string setNTRIP_mount = "SCSC";
        public string setNTRIP_userName = string.Empty;
        public string setNTRIP_userPassword = string.Empty;
        public bool setNTRIP_isOn = false;
        public int setNTRIP_sendGGAInterval = 10;
        public int setNTRIP_sendToUDPPort = 2233;
        public double setNTRIP_manualLat = 53;
        public double setNTRIP_manualLon = -111;
        public string setNTRIP_casterURL = "NTRIP.itsware.net";
        public bool setNTRIP_isGGAManual = false;
        public bool setNTRIP_isTCP = false;
        public bool setNTRIP_isHTTP10 = false;
        public bool setNTRIP_sendToSerial = true;
        public bool setNTRIP_sendToUDP = true;
        public int setNTRIP_packetSize = 256;

        // Port settings
        public string setPort_portNameGPS2 = "GPS2";
        public int setPort_baudRateGPS2 = 9600;
        public string setPort_portNameMachine = "Mach**";
        public bool setPort_wasMachineModuleConnected = false;
        public bool setPort_wasIMUConnected = false;
        public string setPort_portNameIMU = "IMU";
        public bool setPort_wasGPSConnected = false;
        public string setPort_portNameRadio = "***";
        public string setPort_baudRateRadio = "9600";
        public string setPort_radioChannel = "439.000";

        // Radio settings
        public bool setRadio_isOn = false;
        public List<CRadioChannel> setRadio_Channels = new List<CRadioChannel>();
        public bool setUDP_isSendNMEAToUDP = false;

        // RTCM settings
        public string setPort_portNameRtcm = "RTCM";
        public int setPort_baudRateRtcm = 9600;
        public bool setPort_wasRtcmConnected = false;

        // Module connection statuses
        public bool setMod_isIMUConnected = true;
        public bool setMod_isMachineConnected = true;
        public bool setMod_isSteerConnected = true;

        // Pass settings
        public bool setPass_isOn = false;

        // Ethernet and network settings
        public byte etIP_SubnetOne = 192;
        public byte etIP_SubnetTwo = 168;
        public byte etIP_SubnetThree = 5;
        public byte eth_loopOne = 127;
        public byte eth_loopTwo = 255;
        public byte eth_loopThree = 255;
        public byte eth_loopFour = 255;

        // Display settings
        public bool setDisplay_isAutoRunGPS_Out = false;



        public bool Load()
        {
            string path = Path.Combine(FormLoop.profileDirectory, RegistrySettings.ProfileName + ".XML");
            if (!LoadXMLFile(path, this))
            {
                RegistrySettings.Save("ProfileName", "");
                Log.System.Write("Profile Not Found, Loading Default");
                return false;
            }
            else
            {
                Log.System.Write(RegistrySettings.ProfileName + ".XML" + " Loaded");
            }
            return true;
        }

        public void Save()
        {
            string path = Path.Combine(FormLoop.profileDirectory, RegistrySettings.ProfileName + ".XML");

            if (RegistrySettings.ProfileName != "")
                SaveXMLFile(path, this);
        }

        public void Reset()
        {
            settings_ = new Settings();
            settings_.Save();
        }

        public static bool LoadXMLFile(string filePath, object obj)
        {
            bool Errors = false;
            try
            {
                if (File.Exists(filePath))
                {
                    using (XmlTextReader reader = new XmlTextReader(filePath))
                    {
                        string currentParent = "";
                        string name = "";
                        while (reader.Read())
                        {
                            switch (reader.NodeType)
                            {
                                case XmlNodeType.Element:
                                    if (reader.Name == "AgIO.Properties.Settings")
                                    {
                                        currentParent = reader.Name;
                                    }
                                    else if (reader.Name == "setting")
                                    {
                                        name = reader.GetAttribute("name");
                                        string serializeAs = reader.GetAttribute("serializeAs");
                                    }
                                    else if (reader.Name == "value")
                                    {
                                        if (!string.IsNullOrEmpty(name))
                                        {
                                            var pinfo = obj.GetType().GetField(name);
                                            if (pinfo != null)
                                            {
                                                try
                                                {
                                                    // Read string values
                                                    string value = reader.ReadString();

                                                    if (pinfo.FieldType == typeof(List<CRadioChannel>))
                                                    {
                                                        string innerXml = reader.ReadInnerXml();//.ReadString();

                                                        // Add the missing root element
                                                        string wrappedXml = $"<ArrayOfCRadioChannel>{innerXml}</ArrayOfCRadioChannel>";


                                                        // Deserialize the inner XML into a List<CRadioChannel>
                                                        var serializer = new XmlSerializer(typeof(List<CRadioChannel>), new XmlRootAttribute("ArrayOfCRadioChannel"));
                                                        using (var stringReader = new StringReader(wrappedXml))
                                                        {
                                                            var nestedObj = serializer.Deserialize(stringReader);
                                                            pinfo.SetValue(obj, nestedObj);
                                                        }
                                                    }
                                                    else if (pinfo.FieldType == typeof(string))
                                                    {
                                                        pinfo.SetValue(obj, value);
                                                    }
                                                    else if (pinfo.FieldType == typeof(double))
                                                    {
                                                        pinfo.SetValue(obj, double.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(bool))
                                                    {
                                                        pinfo.SetValue(obj, bool.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(int))
                                                    {
                                                        pinfo.SetValue(obj, int.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(byte))
                                                    {
                                                        pinfo.SetValue(obj, byte.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(float))
                                                    {
                                                        pinfo.SetValue(obj, float.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(Color))
                                                    {
                                                        var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                                        if (parts.Length == 3 && int.TryParse(parts[0], out int r) && int.TryParse(parts[1], out int g) && int.TryParse(parts[2], out int b))
                                                        {
                                                            pinfo.SetValue(obj, Color.FromArgb(r, g, b));
                                                        }
                                                    }
                                                    else if (pinfo.FieldType == typeof(short))
                                                    {
                                                        pinfo.SetValue(obj, short.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(decimal))
                                                    {
                                                        pinfo.SetValue(obj, decimal.Parse(value));
                                                    }
                                                    else if (pinfo.FieldType == typeof(Point))
                                                    {
                                                        var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                                        if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                                                        {
                                                            pinfo.SetValue(obj, new Point(x, y));
                                                        }
                                                    }
                                                    else if (pinfo.FieldType == typeof(Size))
                                                    {
                                                        var parts = value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                                        if (parts.Length == 2 && int.TryParse(parts[0], out int width) && int.TryParse(parts[1], out int height))
                                                        {
                                                            pinfo.SetValue(obj, new Size(width, height));
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Errors = true;
                                                        continue;
                                                        throw new ArgumentException("type not found");
                                                    }
                                                }
                                                catch (Exception e)
                                                {
                                                    Log.System.Write("Registry Settings Type Not Found " + e.ToString());

                                                    Errors = true;
                                                    continue;
                                                }
                                            }
                                        }
                                    }
                                    break;

                                case XmlNodeType.EndElement:
                                    if (reader.Name == "AgOpenGPS.Properties.Settings")
                                    {
                                        currentParent = "";
                                    }
                                    break;
                            }
                        }
                        reader.Close();
                    }
                    return !Errors;
                }
            }
            catch (Exception ex)
            {
                Log.System.Write("Registry Settings LoadXML Error: " + ex.ToString());
            }
            return false;
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
                    xml.WriteStartElement("AgIO.Properties.Settings");

                    foreach (var fld in obj.GetType().GetFields())
                    {
                        var value = fld.GetValue(obj);

                        // Start a "setting" element
                        xml.WriteStartElement("setting");

                        // Add attributes to the "setting" element
                        xml.WriteAttributeString("name", fld.Name);

                        // Determine if serializeAs="Xml" is needed
                        if (value.GetType() == typeof(List<CRadioChannel>))
                        {
                            xml.WriteAttributeString("serializeAs", "Xml");

                            // Write the serialized object to a nested "value" element
                            xml.WriteStartElement("value");

                            var serializer = new XmlSerializer(value.GetType());
                            serializer.Serialize(xml, value);
                            
                            xml.WriteEndElement(); // value
                        }
                        else
                        {
                            xml.WriteAttributeString("serializeAs", "String");

                            if (value is Point pointValue)
                            {
                                xml.WriteElementString("value", $"{pointValue.X}, {pointValue.Y}");
                            }
                            else if (value is Size sizeValue)
                            {
                                xml.WriteElementString("value", $"{sizeValue.Width}, {sizeValue.Height}");
                            }
                            else if (value is Color dd)
                            {
                                xml.WriteElementString("value", $"{dd.R}, {dd.G}, {dd.B}");
                            }
                            else
                            {
                                xml.WriteElementString("value", value.ToString());
                            }
                        }

                        xml.WriteEndElement(); // setting
                    }

                    // Close all open elements
                    xml.WriteEndElement(); // AgIO.Properties.Settings
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
            catch (Exception ex)
            {
                Log.System.Write("Registry Settings SaveXML Error: " + ex.ToString());
            }
        }
    }
}
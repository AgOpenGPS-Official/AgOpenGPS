using AgLibrary.Logging;
using AgOpenGPS.Properties;
using Microsoft.Win32;
using OpenTK.Input;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AgOpenGPS
{
    public class CFeatureSettings
    {
        public CFeatureSettings()
        { }

        //public bool ;
        public bool isHeadlandOn = true;

        public bool isTramOn = false;
        public bool isBoundaryOn = true;
        public bool isBndContourOn = false;
        public bool isRecPathOn = false;
        public bool isABSmoothOn = false;

        public bool isHideContourOn = false;
        public bool isWebCamOn = false;
        public bool isOffsetFixOn = false;
        public bool isAgIOOn = true;

        public bool isContourOn = true;
        public bool isYouTurnOn = true;
        public bool isSteerModeOn = true;

        public bool isManualSectionOn = true;
        public bool isAutoSectionOn = true;
        public bool isCycleLinesOn = true;
        public bool isABLineOn = true;
        public bool isCurveOn = true;
        public bool isAutoSteerOn = true;

        public bool isUTurnOn = true;
        public bool isLateralOn = true;

        public CFeatureSettings(CFeatureSettings _feature)
        {
            isHeadlandOn = _feature.isHeadlandOn;
            isTramOn = _feature.isTramOn;
            isBoundaryOn = _feature.isBoundaryOn;
            isBndContourOn = _feature.isBndContourOn;
            isRecPathOn = _feature.isRecPathOn;

            isABSmoothOn = _feature.isABSmoothOn;
            isHideContourOn = _feature.isHideContourOn;
            isWebCamOn = _feature.isWebCamOn;
            isOffsetFixOn = _feature.isOffsetFixOn;
            isAgIOOn = _feature.isAgIOOn;

            isContourOn = _feature.isContourOn;
            isYouTurnOn = _feature.isYouTurnOn;
            isSteerModeOn = _feature.isSteerModeOn;

            isManualSectionOn = _feature.isManualSectionOn;
            isAutoSectionOn = _feature.isAutoSectionOn;
            isCycleLinesOn = _feature.isCycleLinesOn;
            isABLineOn = _feature.isABLineOn;
            isCurveOn = _feature.isCurveOn;

            isAutoSteerOn = _feature.isAutoSteerOn;
            isLateralOn = _feature.isLateralOn;
            isUTurnOn = _feature.isUTurnOn;
        }
    }

    public static class SettingsIO
    {
        /// <summary>
        /// Import an XML and save to 1 section of user.config
        /// </summary>
        /// <param name="settingFile">Either Settings or Vehicle or Tools</param>
        /// <param name="settingsFilePath">Usually Documents.Drive.Folder</param>
        internal static void ImportSingle(string settingFile, string settingsFilePath)
        {
            if (!File.Exists(settingsFilePath))
            {
                throw new FileNotFoundException();
            }

            //var appSettings = Properties.Settings.Default;

            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);

                string sectionName = "";

                if (settingFile == "Settings")
                {
                    sectionName = Properties.Settings.Default.Context["GroupName"].ToString();
                }
                //else if (settingFile == "Tool")
                //{
                //    sectionName = Properties.Tool.Default.Context["GroupName"].ToString();
                //}
                //else if (settingFile == "DataSource")
                //{
                //    sectionName = Properties.Tool.Default.Context["GroupName"].ToString();
                //}

                XDocument document = XDocument.Load(Path.Combine(settingsFilePath));
                string settingsSection = document.XPathSelectElements($"//{sectionName}").Single().ToString();
                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsSection);
                config.Save(ConfigurationSaveMode.Modified);

                if (settingFile == "Settings")
                {
                    Properties.Settings.Default.Reload();
                }
            }
            catch // Should make this more specific
            {
                // Could not import settings.
                if (settingFile == "Settings")
                {
                    Properties.Settings.Default.Reload();
                }                
            }
        }

        internal static void ExportSingle(string settingsFilePath)
        {
            Properties.Settings.Default.Save();

            //Export the entire settings as an xml
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            config.SaveAs(settingsFilePath);
        }

        internal static void ExportAll(string settingsFilePath)
        {
            Properties.Settings.Default.Save();

            //Export the entire settings as an xml
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            config.SaveAs(settingsFilePath);
        }

        internal static bool ImportAll(string settingsFilePath)
        {
            if (!File.Exists(settingsFilePath))
            {
                return (false);
            }
            try
            {
                using (StreamReader xmlFile = new StreamReader(settingsFilePath))
                using (var output = new StreamWriter("Output999.xml"))

                {
                    string line;
                    int step = 0;

                    line = xmlFile.ReadLine();
                    output.WriteLine(line);
                    line = xmlFile.ReadLine();
                    output.WriteLine(line);
                    line = xmlFile.ReadLine();
                    output.WriteLine(line);
                    line = xmlFile.ReadLine();
                    if (line == null)
                    {
                        MessageBox.Show("Fatal Error with Settings File");
                        return (false);
                    }

                    if (line.Contains("ies.Vehicle"))
                    {
                        output.WriteLine("        <AgOpenGPS.Properties.Settings>");

                        while (!xmlFile.EndOfStream)
                        {
                            line = xmlFile.ReadLine();

                            if (step < 2)
                            {
                                if (line.Contains("ies.Vehicle")
                                    || line.Contains("ies.Settings"))
                                {
                                    step++;
                                }
                                else
                                {
                                    output.WriteLine(line);
                                }
                            }
                            else output.WriteLine(line);
                        }
                        settingsFilePath = "Output999.xml";
                        output.Close();
                    }
                    else
                    {
                        //nothing to do
                    }

                    xmlFile.Close();
                }
            }

            //while (!xmlFile.EndOfStream)
            //{
            //    var texx  = File.ReadLine();
            //    if (texx == "        <AgOpenGPS.Properties.Vehicle>")
            //    {
            //    }
            //    //"        <AgOpenGPS.Properties.Vehicle>"
            //}
            catch (Exception)
            {
                MessageBox.Show("Fatal Error with Settings File");
                return (false);
            }

            try
            {
                Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                string sectionName = Properties.Settings.Default.Context["GroupName"].ToString();

                XDocument document = XDocument.Load(Path.Combine(settingsFilePath));
                string settingsA = document.XPathSelectElements($"//{sectionName}").Single().ToString();

                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsA);
                config.Save(ConfigurationSaveMode.Modified);

                //ConfigurationManager.RefreshSection(sectionName);
                Properties.Settings.Default.Reload();

                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
                sectionName = Properties.Settings.Default.Context["GroupName"].ToString();

                document = XDocument.Load(Path.Combine(settingsFilePath));
                settingsA = document.XPathSelectElements($"//{sectionName}").Single().ToString();

                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsA);
                config.Save(ConfigurationSaveMode.Modified);

                Properties.Settings.Default.Reload();
                return (true);
            }
            catch (Exception) // Should make this more specific
            {
                // Could not import settings.
                Properties.Settings.Default.Reload();
                MessageBox.Show("Fatal Error with Settings File");
                return (false);
            }
        }
    }

    public static class RegistrySettings
    {
        public static string culture = "en";
        public static string vehiclesDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Vehicles");
        public static string logsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Logs");
        public static string vehicleFileName = "Default Vehicle";
        public static string workingDirectory = "Default";
        public static string baseDirectory = workingDirectory;
        public static string fieldsDirectory = workingDirectory;

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
                    Key.SetValue("VehicleFileName", "Default Vehicle");
                    Key.SetValue("WorkingDirectory", "Default");
                    Key.Close();
                }

                //Base Directory Registry Key
                regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                if (regKey.GetValue("WorkingDirectory") == null || regKey.GetValue("WorkingDirectory").ToString() == "")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    key.SetValue("WorkingDirectory", "Default");
                    key.Close();
                }
                workingDirectory = regKey.GetValue("WorkingDirectory").ToString();

                if (workingDirectory == "Default")
                {
                    baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS");
                }
                else //user set to other
                {
                    baseDirectory = Path.Combine(workingDirectory, "AgOpenGPS");
                }

                //Vehicle File Name Registry Key
                if (regKey.GetValue("VehicleFileName") == null || regKey.GetValue("VehicleFileName").ToString() == "")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    key.SetValue("VehicleFileName", "Default Vehicle");
                    key.Close();
                }

                vehicleFileName = regKey.GetValue("VehicleFileName").ToString();

                //Language Registry Key
                if (regKey.GetValue("Language") == null || regKey.GetValue("Language").ToString() == "")
                {
                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                    key.SetValue("Language", "en");
                    key.Close();
                }

                culture = regKey.GetValue("Language").ToString();

                //close registry
                regKey.Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Creating Registry keys: " + ex.ToString());
                Reset();
            }

            //make sure directories exist and are in right place if not default workingDir
            CreateDirectories();

            //keep below 500 kb
            Log.CheckLogSize(Path.Combine(logsDirectory, "AgOpenGPS_Events_Log.txt"), 500000);

            //what's in the vehicle directory
            try
            {
                DirectoryInfo dinfo = new DirectoryInfo(vehiclesDirectory);
                FileInfo[] vehicleFiles = dinfo.GetFiles("*.xml");

                bool isVehicleExist = false;

                foreach (FileInfo file in vehicleFiles)
                {
                    string temp = Path.GetFileNameWithoutExtension(file.Name).Trim();

                    if (temp == vehicleFileName)
                    {
                        isVehicleExist = true;
                    }
                }

                //does current vehicle exist?
                if (isVehicleExist && vehicleFileName != "Default Vehicle")
                {
                    SettingsIO.ImportAll(Path.Combine(RegistrySettings.vehiclesDirectory, vehicleFileName + ".XML"));
                }
                else
                {
                    vehicleFileName = "Default Vehicle";
                    Log.EventWriter("Vehicle file does not exist or is Default, Default Vehicle selected");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Loading Vehicle, Doing Registry Reset: " + ex.ToString());
                Reset();

                //reset to Default Vehicle and save
                Settings.Default.Reset();
                Settings.Default.Save();
            }
        }        

        public static void Save()
        {
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
            try
            {
                key.SetValue("VehicleFileName", vehicleFileName);
                key.SetValue("Language", culture);
                key.SetValue("WorkingDirectory", workingDirectory);

                //Log.EventWriter(vehicleFileName + " Saved to registry key");
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Saving keys: " + ex.ToString());
            }
            key.Close();

            try
            {
                if (vehicleFileName != "Default Vehicle")
                {
                    SettingsIO.ExportAll(Path.Combine(vehiclesDirectory, vehicleFileName + ".xml"));
                    //Log.EventWriter(vehicleFileName + ".XML Saved to Vehicles");
                }
                else
                {
                    //Log.EventWriter("Default Vehicle Not saved to Vehicles");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Unable to save Vehicle FileName: " + ex.ToString());
            }
        }

        public static void Reset()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgOpenGPS");

                //create all new key
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                key.SetValue("Language", "en");
                key.SetValue("VehicleFileName", "Default Vehicle");
                key.SetValue("WorkingDirectory", "Default");
                key.Close();

                Log.EventWriter("Registry -> Resetting Registry SubKey Tree and Full Default Reset");

                culture = "en";
                vehiclesDirectory =
                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Vehicles");
                logsDirectory =
                   Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Logs");
                vehicleFileName = "Default Vehicle";
                workingDirectory = "Default";
                baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS");
                fieldsDirectory = Path.Combine(baseDirectory, "Fields");

                CreateDirectories();

                

            }
            catch (Exception ex)
            {
                Log.EventWriter("\"Registry -> Catch, Serious Problem Resetting Registry keys: " + ex.ToString());
            }
        }

        public static void CreateDirectories()
        {
            //get the vehicles directory, if not exist, create
            try
            {
                vehiclesDirectory = Path.Combine(baseDirectory, "Vehicles");
                if (!string.IsNullOrEmpty(vehiclesDirectory) && !Directory.Exists(vehiclesDirectory))
                {
                    Directory.CreateDirectory(vehiclesDirectory);
                    Log.EventWriter("Vehicles Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Vehicles Directory: " + ex.ToString());
            }

            //get the fields directory, if not exist, create
            try
            {
                fieldsDirectory = Path.Combine(baseDirectory, "Fields");
                if (!string.IsNullOrEmpty(fieldsDirectory) && !Directory.Exists(fieldsDirectory))
                {
                    Directory.CreateDirectory(fieldsDirectory);
                    Log.EventWriter("Fields Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Fields Directory: " + ex.ToString());
            }

            //get the logs directory, if not exist, create
            try
            {
                if (!string.IsNullOrEmpty(logsDirectory) && !Directory.Exists(logsDirectory))
                {
                    Directory.CreateDirectory(logsDirectory);
                    Log.EventWriter("Logs Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Logs Directory: " + ex.ToString());
            }
        }
    }
}
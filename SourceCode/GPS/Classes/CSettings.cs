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
}
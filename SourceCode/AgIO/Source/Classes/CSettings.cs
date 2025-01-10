using AgIO.Properties;
using AgLibrary.Logging;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AgIO
{
    public static class SettingsIO
    {
        /// <summary>
        /// Import an XML and save to 1 section of user.config
        /// </summary>
        /// <param name="settingFile">Either Settings or Vehicle or Tools</param>
        /// <param name="settingsFilePath">Usually Documents.Drive.Folder</param>
        internal static void ImportSettings(string settingsFilePath)
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

                sectionName = Properties.Settings.Default.Context["GroupName"].ToString();

                XDocument document = XDocument.Load(Path.Combine(settingsFilePath));
                string settingsSection = document.XPathSelectElements($"//{sectionName}").Single().ToString();
                config.GetSectionGroup("userSettings").Sections[sectionName].SectionInformation.SetRawXml(settingsSection);
                config.Save(ConfigurationSaveMode.Modified);

                {
                    Properties.Settings.Default.Reload();
                }
            }
            catch (Exception ex) // Should make this more specific
            {
                // Could not import settings.
                {
                    Properties.Settings.Default.Reload();
                    Log.EventWriter("Catch -> Failed to Import Settings: " +  ex.ToString());
                }
            }
        }

        internal static void ExportSettings(string settingsFilePath)
        {
            Properties.Settings.Default.Save();

            //Export the entire settings as an xml
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            config.SaveAs(settingsFilePath);
        }
    }

    public static class RegistrySettings
    {
        public static string culture = "en";
        public static string profileDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "AgIO");
        public static string logsDirectory =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "Logs");
        public static string profileName = "Default Profile";

        public static void Load()
        {
            try
            {
                try
                {
                    //create Logs directory if not exist
                    if (!string.IsNullOrEmpty(logsDirectory) && !Directory.Exists(logsDirectory))
                    {
                        Directory.CreateDirectory(logsDirectory);
                        Log.EventWriter("Logs Dir Created\r");
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Catch, Serious Problem Making Logs Directory: " + ex.ToString());
                }

                //keep below 500 kb
                Log.CheckLogSize(Path.Combine(logsDirectory, "AgIO_Events_Log.txt"), 1000000);

                try
                {
                    //create Logs directory if not exist
                    if (!string.IsNullOrEmpty(profileDirectory) && !Directory.Exists(profileDirectory))
                    {
                        Directory.CreateDirectory(profileDirectory);
                        Log.EventWriter("Profile Dir Created\r");
                    }
                }
                catch (Exception ex)
                {
                    Log.EventWriter("Catch, Serious Problem Making Profile Directory: " + ex.ToString());
                }

                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgIO");

                //create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");

                    //storing the values
                    Key.SetValue("Language", "en");
                    Key.SetValue("ProfileName", "Default Profile");
                    Key.Close();
                    Log.EventWriter("Registry -> SubKey AgIO and Keys Created\r");
                }
                else
                {
                    try
                    {
                        //Profile File Name from Registry Key
                        if (regKey.GetValue("ProfileName") == null || regKey.GetValue("ProfileName").ToString() == null)
                        {
                            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");
                            key.SetValue("ProfileName", "Default Profile");
                            Log.EventWriter("Registry -> Key Profile Name was null and Created");
                        }
                        else
                        {
                            //Culture from Registry Key
                            if (regKey.GetValue("AgOne_Culture") == null || regKey.GetValue("Language").ToString() == "")
                            {
                                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");
                                key.SetValue("Language", "en");
                                Log.EventWriter("Registry -> Culture was null and Created");
                            }
                            else
                            {
                                culture = regKey.GetValue("Language").ToString();
                            }

                            profileName = regKey.GetValue("ProfileName").ToString();

                            //get the Documents directory, if not exist, create
                            profileDirectory =
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS", "AgIO");

                            if (!string.IsNullOrEmpty(profileDirectory) && !Directory.Exists(profileDirectory))
                            {
                                Directory.CreateDirectory(profileDirectory);
                            }

                            //what's in the vehicle directory
                            DirectoryInfo dinfo = new DirectoryInfo(RegistrySettings.profileDirectory);
                            FileInfo[] vehicleFiles = dinfo.GetFiles("*.xml");

                            bool isProfileExist = false;

                            foreach (FileInfo file in vehicleFiles)
                            {
                                string temp = Path.GetFileNameWithoutExtension(file.Name).Trim();

                                if (temp == profileName)
                                {
                                    isProfileExist = true;
                                }
                            }

                            //does current vehicle exist?
                            if (isProfileExist && profileName != "Default Profile")
                            {
                                SettingsIO.ImportSettings(Path.Combine(profileDirectory, profileName + ".XML"));
                                Log.EventWriter("Registry -> " + profileName + ".XML Profile Loaded");
                            }
                            else
                            {
                                Log.EventWriter("Registry -> " + profileName + ".XML Profile does not exist. Called in Program.cs");
                                profileName = "Default Profile";
                                Save();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.EventWriter("Registry -> Catch, Serious Problem Loading Profile, Doing Registry Reset: " + ex.ToString());
                        Reset();

                        //reset to Default Profile and save
                        Settings.Default.Reset();
                        Settings.Default.Save();
                    }
                    regKey.Close();
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Creating Registry keys: " + ex.ToString());
                Reset();
            }
        }

        public static void Save()
        {
            Properties.Settings.Default.Save();

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");
            try
            {
                key.SetValue("ProfileName", profileName);
                Log.EventWriter(profileName + " Saved to registry key");
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Saving keys: " + ex.ToString());
            }
            key.Close();

            if (RegistrySettings.profileName != "Default Profile")
            {
                Thread.Sleep(500);
                SettingsIO.ExportSettings(Path.Combine(RegistrySettings.profileDirectory, RegistrySettings.profileName + ".xml"));
            }
        }
        

        public static void Reset()
        {
            Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgIO");

            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgIO");
            try
            {
                key.SetValue("ProfileName", "Default Profile");
                key.SetValue("Language", "en");
                Log.EventWriter("Registry -> Resetting Registry keys");
            }
            catch (Exception ex)
            {
                Log.EventWriter("\"Registry -> Catch, Serious Problem Resetting Registry keys: " + ex.ToString());
            }
            key.Close();
        }
    }
}
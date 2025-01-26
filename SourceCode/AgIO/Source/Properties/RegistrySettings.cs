using AgIO.Properties;
using AgLibrary.Logging;
using Microsoft.Win32;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace AgIO
{
    public static class RegKeys
    {
        public const string profileName = "ProfileName";
        public const string workingDirectory = "WorkingDirectory";
        public const string language = "Language";
    }

    public static class RegistrySettings
    {
        public const string defaultString = "Default";

        public static string culture = "en";
        public static string workingDirectory = "Default";
        public static string baseDirectory;
        public static string profileDirectory;
        public static string logsDirectory;
        public static string profileName = "";

        public static void Load()
        {
            try
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                if (regKey.GetValue(RegKeys.workingDirectory) == null || regKey.GetValue(RegKeys.workingDirectory).ToString() == "")
                {
                    regKey.SetValue(RegKeys.workingDirectory, defaultString);
                    Log.EventWriter("Registry -> Key workingDirectory was null");
                }
                workingDirectory = regKey.GetValue(RegKeys.workingDirectory).ToString();

                //Profile File Name from Registry Key
                if (regKey.GetValue(RegKeys.profileName) == null)
                {
                    regKey.SetValue(RegKeys.profileName, "");
                    Log.EventWriter("Registry -> Key Profile Name was null and Created");
                }
                profileName = regKey.GetValue(RegKeys.profileName).ToString();

                //Culture from Registry Key
                if (regKey.GetValue("Language").ToString() == "")
                {
                    regKey.SetValue("Language", "en");
                    Log.EventWriter("Registry -> Culture was null and Created");
                }
                culture = regKey.GetValue("Language").ToString();
                
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
            Log.CheckLogSize(Path.Combine(logsDirectory, "AgIO_Events_Log.txt"), 1000000);

            Properties.Settings.Default.Load();
        }

        public static void Save(string name, string value)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                key.SetValue(name, value);
                Log.EventWriter("Registry -> Key " + name + " Saved to registry key with value: " + value);

                if (name == RegKeys.profileName)
                    profileName = value;
                key.Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Saving keys: " + ex.ToString());
            }
        }

        public static void Reset()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgOpenGPS");

                Log.EventWriter("Registry -> Resetting Registry SubKey Tree and Full Default Reset");
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Resetting Registry keys: " + ex.ToString());

                Log.FileSaveSystemEvents();

                MessageBox.Show("Can't delete the Registery SubKeyTree",
                "Critical Registry Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                Environment.Exit(0);
            }
        }

        private static void CreateDirectories()
        {
            try
            {
                if (workingDirectory == defaultString)
                {
                    baseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "AgOpenGPS");
                }
                else //user set to other
                {
                    baseDirectory = Path.Combine(workingDirectory, "AgOpenGPS");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Working Directory: " + ex.ToString());

                if (workingDirectory != defaultString)
                {
                    workingDirectory = defaultString;
                    Save(RegKeys.workingDirectory, defaultString);
                    CreateDirectories();
                    return;
                }
                else//program will crash anyways!
                {
                    Log.FileSaveSystemEvents();
                    Environment.Exit(0);
                }
            }

            try
            {
                logsDirectory = Path.Combine(baseDirectory, "Logs");
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

            try
            {
                //get the Documents directory, if not exist, create
                profileDirectory = Path.Combine(baseDirectory, "AgIO");

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
        }
    }
}
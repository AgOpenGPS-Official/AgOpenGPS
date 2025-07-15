using AgLibrary.Logging;
using AgOpenGPS.Forms;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public static class RegKeys
    {
        public const string vehicleFileName = "VehicleFileName";
        public const string workingDirectory = "WorkingDirectory";
        public const string language = "Language";
    }

    public static class RegistrySettings
    {
        public const string defaultString = "Default";
        public static string culture = "en";
        public static string vehicleFileName = "";
        public static string workingDirectory = "Default";
        public static string vehiclesDirectory;
        public static string logsDirectory;
        public static string baseDirectory;
        public static string fieldsDirectory;

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

                //Vehicle File Name Registry Key
                if (regKey.GetValue(RegKeys.vehicleFileName) == null)
                {
                    regKey.SetValue(RegKeys.vehicleFileName, "");
                    Log.EventWriter("Registry -> Key vehicleFileName was null");
                }
                vehicleFileName = regKey.GetValue(RegKeys.vehicleFileName).ToString();

                //Language Registry Key
                if (regKey.GetValue(RegKeys.language) == null || regKey.GetValue(RegKeys.language).ToString() == "")
                {
                    regKey.SetValue(RegKeys.language, "en");
                    Log.EventWriter("Registry -> Key language was null");
                }
                culture = regKey.GetValue(RegKeys.language).ToString();

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
            Log.CheckLogSize(Path.Combine(logsDirectory, "AgOpenGPS_Events_Log.txt"), 1000000);

            Properties.Settings.Default.Load();
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey  
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                if (name == RegKeys.vehicleFileName)
                    vehicleFileName = value;
                else if (name == RegKeys.language)
                    culture = value;

                if (name == RegKeys.workingDirectory && value == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                {
                    key.SetValue(name, defaultString);
                    Log.EventWriter("Registry -> Key " + name + " Saved to registry key with value: " + defaultString);
                }
                else//storing the value
                {
                    key.SetValue(name, value);
                    Log.EventWriter("Registry -> Key " + name + " Saved to registry key with value: " + value);
                }

                key.Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Unable to save " + name + ": " + ex.ToString());
            }
        }

        public static void Reset()
        {
            try
            {
                Registry.CurrentUser.DeleteSubKeyTree(@"SOFTWARE\AgOpenGPS");

                Log.EventWriter("Registry -> Resetting Registry SubKey Tree and Full Default Reset");
            }
            catch (Exception ex)//program will crash anyways!
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Resetting Registry keys: " + ex.ToString());

                Log.FileSaveSystemEvents();

                // Show critical registry error
                FormDialog.Show(
                    "Critical Registry Error",
                    "Can't delete the Registry SubKeyTree",
                    MessageBoxButtons.OK);


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
                logsDirectory = Path.Combine(baseDirectory, "Logs");
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

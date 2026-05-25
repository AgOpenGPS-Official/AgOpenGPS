using AgLibrary.Logging;
using AgLibrary.Settings;
using AgOpenGPS.Forms;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public static class RegKeys
    {
        // New profile system (v7+) - split Vehicle/Tool/Environment profiles
        public const string vehicleProfileName = "VehicleProfileName";
        public const string toolProfileName = "ToolProfileName";
        public const string environmentFileName = "EnvironmentFileName";

        // Legacy (v6 and earlier) - single profile file
        public const string vehicleFileName = "VehicleFileName";

        public const string workingDirectory = "WorkingDirectory";
        public const string language = "Language";
    }

    public static class RegistrySettings
    {
        public const string defaultString = "Default";
        public static string culture = "en";

        // New profile system values
        public static string vehicleProfileName = "";
        public static string toolProfileName = "";
        public static string environmentFileName = "Default";

        // Legacy value (only for migration detection)
        public static string legacyVehicleFileName = "";

        public static string workingDirectory = "Default";
        public static string vehiclesDirectory;
        public static string toolsDirectory;
        public static string logsDirectory;
        public static string baseDirectory;
        public static string fieldsDirectory;
        public static string environmentDirectory;

        public static LoadResult vehicleProfileLoadResult = LoadResult.MissingFile;
        public static LoadResult toolProfileLoadResult = LoadResult.MissingFile;
        public static bool vehicleProfileLoadedFromBackup = false;
        public static bool toolProfileLoadedFromBackup = false;

        // Indicates if migration from legacy single profile is needed
        public static bool NeedsMigration { get; private set; }

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

                // NEW: Vehicle Profile Name (v7+)
                if (regKey.GetValue(RegKeys.vehicleProfileName) == null)
                {
                    regKey.SetValue(RegKeys.vehicleProfileName, "");
                    Log.EventWriter("Registry -> Key vehicleProfileName was null");
                }
                vehicleProfileName = regKey.GetValue(RegKeys.vehicleProfileName).ToString();

                // NEW: Vehicle Profile Name (v7+)
                if (regKey.GetValue(RegKeys.vehicleProfileName) == null)
                {
                    regKey.SetValue(RegKeys.vehicleProfileName, "");
                    Log.EventWriter("Registry -> Key vehicleProfileName was null");
                }
                vehicleProfileName = regKey.GetValue(RegKeys.vehicleProfileName).ToString();

                // NEW: Tool Profile Name (v7+)
                if (regKey.GetValue(RegKeys.toolProfileName) == null)
                {
                    regKey.SetValue(RegKeys.toolProfileName, "");
                    Log.EventWriter("Registry -> Key toolProfileName was null");
                }
                toolProfileName = regKey.GetValue(RegKeys.toolProfileName).ToString();

                // LEGACY: Vehicle File Name (v6 and earlier) - only for migration detection
                if (regKey.GetValue(RegKeys.vehicleFileName) == null)
                {
                    regKey.SetValue(RegKeys.vehicleFileName, "");
                    Log.EventWriter("Registry -> Key vehicleFileName was null");
                }
                legacyVehicleFileName = regKey.GetValue(RegKeys.vehicleFileName).ToString();

                // Environment File Name Registry Key
                if (regKey.GetValue(RegKeys.environmentFileName) == null)
                {
                    regKey.SetValue(RegKeys.environmentFileName, "Default");
                    Log.EventWriter("Registry -> Key environmentFileName was null");
                }
                environmentFileName = regKey.GetValue(RegKeys.environmentFileName).ToString();

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

            // Detect if migration is needed (has legacy VehicleFileName but no new profile keys)
            NeedsMigration = !string.IsNullOrEmpty(legacyVehicleFileName) &&
                            string.IsNullOrEmpty(vehicleProfileName) &&
                            string.IsNullOrEmpty(toolProfileName);

            if (NeedsMigration)
            {
                Log.EventWriter($"Legacy profile detected: {legacyVehicleFileName}, migration needed");
            }

            //make sure directories exist and are in right place if not default workingDir
            CreateDirectories();

            Log.CheckLogSize(Path.Combine(logsDirectory, "AgOpenGPS_Events_Log.txt"));

            // Load Environment settings
            Properties.Settings.Default.Load();

            // Load Vehicle settings if vehicleProfileName is set
            if (!string.IsNullOrEmpty(vehicleProfileName))
            {
                vehicleProfileLoadResult = Properties.VehicleSettings.Default.Load(vehicleProfileName);
                if (vehicleProfileLoadResult != LoadResult.Ok)
                {
                    Log.EventWriter($"Vehicle profile load failed: {vehicleProfileName} ({vehicleProfileLoadResult})");
                }
            }
            else vehicleProfileLoadResult = LoadResult.MissingFile;

            // Load Tool settings if toolProfileName is set
            if (!string.IsNullOrEmpty(toolProfileName))
            {
                toolProfileLoadResult = Properties.ToolSettings.Default.Load(toolProfileName);
                if (toolProfileLoadResult != LoadResult.Ok)
                {
                    Log.EventWriter($"Tool profile load failed: {toolProfileName} ({toolProfileLoadResult})");
                }
            }
            else toolProfileLoadResult = LoadResult.MissingFile;
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                if (name == RegKeys.vehicleProfileName)
                    vehicleProfileName = value;
                else if (name == RegKeys.toolProfileName)
                    toolProfileName = value;
                else if (name == RegKeys.environmentFileName)
                    environmentFileName = value;
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
                    DialogSeverity.Error);


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

                // Ensure baseDirectory exists
                if (!string.IsNullOrEmpty(baseDirectory) && !Directory.Exists(baseDirectory))
                {
                    Directory.CreateDirectory(baseDirectory);
                    Log.EventWriter("Base directory created: " + baseDirectory);
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
                vehiclesDirectory = Path.Combine(baseDirectory, "VehicleProfiles");
                if (!string.IsNullOrEmpty(vehiclesDirectory) && !Directory.Exists(vehiclesDirectory))
                {
                    Directory.CreateDirectory(vehiclesDirectory);
                    Log.EventWriter("VehicleProfiles Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making VehicleProfiles Directory: " + ex.ToString());
            }

            //get the tools directory, if not exist, create
            try
            {
                toolsDirectory = Path.Combine(baseDirectory, "ToolProfiles");
                if (!string.IsNullOrEmpty(toolsDirectory) && !Directory.Exists(toolsDirectory))
                {
                    Directory.CreateDirectory(toolsDirectory);
                    Log.EventWriter("ToolProfiles Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making ToolProfiles Directory: " + ex.ToString());
            }

            //get the environment directory, if not exist, create
            try
            {
                environmentDirectory = Path.Combine(baseDirectory, "Environment");
                if (!string.IsNullOrEmpty(environmentDirectory) && !Directory.Exists(environmentDirectory))
                {
                    Directory.CreateDirectory(environmentDirectory);
                    Log.EventWriter("Environment Dir Created");
                }
            }
            catch (Exception ex)
            {
                Log.EventWriter("Catch, Serious Problem Making Environment Directory: " + ex.ToString());
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

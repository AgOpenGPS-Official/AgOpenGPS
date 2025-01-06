using Microsoft.Win32;
using System;
using System.IO;

namespace AgOpenGPS
{
    public enum LoadResult { Ok, MissingFile, Failed };

    public static class RegistrySettings
    {
        public static string culture = "en";
        public static string workingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string vehicleFileName = "Default Vehicle";

        public static void Load()
        {
            try
            {
                //opening/creating the subkey
                RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");
                
                object dir = regKey.GetValue("WorkingDirectory");
                if (dir != null && dir.ToString() != "Default")
                    workingDirectory = dir.ToString();

                object name = regKey.GetValue("VehicleFileName");
                if (name != null)
                    vehicleFileName = name.ToString();

                var lang = regKey.GetValue("Language");
                if (lang != null)
                    culture = lang.ToString();

                //close registry
                regKey.Close();
            }
            catch (Exception ex)
            {
                Log.EventWriter("Registry -> Catch, Serious Problem Creating Registry keys: " + ex.ToString());
                Reset();
            }
        }

        public static void Save(string name, string value)
        {
            try
            {
                //adding or editing "Language" subkey to the "SOFTWARE" subkey  
                RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                if (name == "VehicleFileName")
                    vehicleFileName = value;
                else if (name == "Language")
                    culture = value;


                if (name == "WorkingDirectory" && value == Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
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
}

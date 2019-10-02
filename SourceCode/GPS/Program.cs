﻿using AgOpenGPS.Properties;
using Microsoft.Win32;
using System;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        private static readonly Mutex Mutex = new Mutex(true, "{8F6F0AC5-B9A1-55fd-A8CF-72F04E6BDE8F}");

        [STAThread]
        private static void Main()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                //opening the subkey
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS");

                //create default keys if not existing
                if (regKey == null)
                {
                    RegistryKey Key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                    //storing the values
                    Key.SetValue("Language", "en");
                    Key.SetValue("Directory", "Default");
                    Key.Close();

                    Settings.Default.set_culture = "en";
                    Settings.Default.setF_workingDirectory = "Default";
                    Settings.Default.Save();
                }
                else
                {
                    Settings.Default.set_culture = regKey.GetValue("Language").ToString();
                    Settings.Default.setF_workingDirectory = regKey.GetValue("Directory").ToString();
                    Settings.Default.Save();
                    regKey.Close();
                }

                //if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
                Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(Properties.Settings.Default.set_culture);
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(Properties.Settings.Default.set_culture);
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormGPS());
            }
            else
            {
                MessageBox.Show("AgOpenGPS is Already Running");
            }
        }

        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //private static extern bool SetProcessDPIAware();
    }
}
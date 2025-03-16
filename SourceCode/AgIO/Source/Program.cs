using AgLibrary.Logging;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace AgIO
{
    internal static class Program
    {
        private static Mutex _mutex;

        public static readonly string Version = Assembly.GetEntryAssembly().GetName().Version.ToString(3); // Major.Minor.Patch

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            using (_mutex = new Mutex(true, "{8F6F0AC4-B9A1-55fd-A8CF-72F04E6BDE8F}", out bool mutexCreated))
            {
                if (mutexCreated)
                {
                    //load the profile name and set profile directory
                    RegistrySettings.Load();

                    Log.EventWriter("Program Started: " + DateTime.Now.ToString("f", CultureInfo.InvariantCulture));
                    Log.EventWriter("AgIO Version: " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));

                    Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                    Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(RegistrySettings.culture);
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new FormLoop());
                }
            }
        }

        // If the application needs to be restarted, it should be done via this method.
        // Manually disposing the Mutex before calling Application.Restart() ensures
        // that the Mutex is not still owned by the current application instance while
        // the new application instance is being started.
        // See: https://stackoverflow.com/a/9456822
        public static void Restart()
        {
            if (_mutex != null)
            {
                _mutex.ReleaseMutex();
                _mutex.Dispose();
                _mutex = null;
                Application.Restart();
            }
        }
    }
}

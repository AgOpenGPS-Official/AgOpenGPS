using System;
using System.Threading;
using System.Windows.Forms;

namespace AgDiag
{
    internal static class Program
    {
        private static readonly Mutex Mutex = new Mutex(true, "{8F6F0AC4-B9A7-55fd-A8CF-72F04E6BDE8F}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (Mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormLoop());
            }
        }
    }
}

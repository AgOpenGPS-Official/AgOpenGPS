using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;

namespace AgLibrary.Logging
{
    /// <summary>
    /// Builds a one-line snapshot of process resource usage for the event log, so slow
    /// degradation (leaks, handle/thread growth, GC pressure) can be diagnosed from the field.
    /// </summary>
    public static class PerformanceDiagnostics
    {
        private const uint GR_GDIOBJECTS = 0;
        private const uint GR_USEROBJECTS = 1;

        [DllImport("user32.dll")]
        private static extern uint GetGuiResources(IntPtr hProcess, uint uiFlags);

        public static string Snapshot()
        {
            try
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    long gdi = -1, user = -1;
                    try
                    {
                        gdi = GetGuiResources(p.Handle, GR_GDIOBJECTS);
                        user = GetGuiResources(p.Handle, GR_USEROBJECTS);
                    }
                    catch { /* not available on this platform */ }

                    return string.Format(CultureInfo.InvariantCulture,
                        "WorkingSet {0} MB, Private {1} MB, Managed {2} MB, Handles {3}, Threads {4}, GDI {5}, USER {6}, GC0/1/2 {7}/{8}/{9}",
                        p.WorkingSet64 >> 20,
                        p.PrivateMemorySize64 >> 20,
                        GC.GetTotalMemory(false) >> 20,
                        p.HandleCount,
                        p.Threads.Count,
                        gdi,
                        user,
                        GC.CollectionCount(0),
                        GC.CollectionCount(1),
                        GC.CollectionCount(2));
                }
            }
            catch (Exception ex)
            {
                return "Snapshot failed: " + ex.Message;
            }
        }
    }
}

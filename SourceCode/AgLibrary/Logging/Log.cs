using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace AgLibrary.Logging
{
    public static class Log
    {
        public static StringBuilder sbEvents = new StringBuilder();
        private static string logsDirectory = "";

        public static void EventWriter(string message)
        {
            sbEvents.Append(DateTime.Now.ToString("T", CultureInfo.InvariantCulture));
            sbEvents.Append("-> ");
            sbEvents.Append(message);
            sbEvents.Append("\r");
        }

        /// <summary>
        /// Logs an error with details for conversion failures
        /// </summary>
        public static void ErrorWriter(string source, string component, string result, Exception ex = null)
        {
            sbEvents.Append(DateTime.Now.ToString("T", CultureInfo.InvariantCulture));
            sbEvents.Append("-> ");
            sbEvents.Append($"Error in {source} - {component}: {result}");
            if (ex != null)
            {
                sbEvents.Append(" | Exception: ");
                sbEvents.Append(ex.Message);
            }
            sbEvents.Append("\r");
        }

        public static void FileSaveSystemEvents()
        {
            if (logsDirectory != "")
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(logsDirectory, true))
                    {
                        writer.Write(sbEvents);
                    }
                }
                catch
                {
                    // suppressing an edge-case error for eg if someone has onedrive client locking the file
                }
                finally
                {
                    sbEvents.Clear();
                }
            }
        }

        public static void CheckLogSize(string logFile, int maxLines = 250)
        {
            logsDirectory = logFile;

            if (!File.Exists(logFile)) return;

            string[] lines = File.ReadAllLines(logFile);
            if (lines.Length > maxLines)
            {
                string[] trimmed = new string[maxLines];
                Array.Copy(lines, lines.Length - maxLines, trimmed, 0, maxLines);
                File.WriteAllLines(logFile, trimmed);
                sbEvents.Append("Log trimmed to last " + maxLines + " lines\r");
            }
        }
    }
}

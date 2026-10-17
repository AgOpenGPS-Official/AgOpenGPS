using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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

        //all file writes go through this chain so background and blocking saves keep their order
        private static readonly object writeChainLock = new object();
        private static Task writeChain = Task.FromResult(true);

        public static void FileSaveSystemEvents()
        {
            if (logsDirectory != "")
            {
                //blocking save - wait for this write (and any queued background writes before it)
                Task write = QueueWrite(TakePendingText());
                try { write.Wait(); } catch { /* write errors are already suppressed */ }
            }
        }

        /// <summary>
        /// Same as FileSaveSystemEvents, but the disk write happens on a background thread.
        /// Use this from the UI thread, where AgOpenGPS also runs its position/render hot path:
        /// only the (cheap) copy of the buffered text happens on the calling thread.
        /// </summary>
        public static void FileSaveSystemEventsInBackground()
        {
            if (logsDirectory != "" && sbEvents.Length > 0)
            {
                QueueWrite(TakePendingText());
            }
        }

        private static string TakePendingText()
        {
            //sbEvents is only touched from the UI thread - copy and clear it there
            string text = sbEvents.ToString();
            sbEvents.Clear();
            return text;
        }

        private static Task QueueWrite(string text)
        {
            string path = logsDirectory;
            lock (writeChainLock)
            {
                writeChain = writeChain.ContinueWith(_ => AppendToFile(path, text), TaskScheduler.Default);
                return writeChain;
            }
        }

        private static void AppendToFile(string path, string text)
        {
            if (text.Length == 0) return;
            try
            {
                using (StreamWriter writer = new StreamWriter(path, true))
                {
                    writer.Write(text);
                }
            }
            catch
            {
                // suppressing an edge-case error for eg if someone has onedrive client locking the file
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

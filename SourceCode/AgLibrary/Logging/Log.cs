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

        public static void FileSaveSystemEvents()
        {
            if (logsDirectory != "")
            {
                using (StreamWriter writer = new StreamWriter(logsDirectory, true))
                {
                    writer.Write(sbEvents);
                    sbEvents.Clear();
                }
            }
        }

        public static void CheckLogSize(string logFile, int sizeLimit)
        {
            logsDirectory = logFile;

            //system event log file
            FileInfo txtfile = new FileInfo(logFile);
            if (txtfile.Exists)
            {
                if (txtfile.Length > (sizeLimit))       // ## NOTE: 0.5MB max file size
                {
                    StringBuilder sbF = new StringBuilder();
                    long bytes = txtfile.Length - sizeLimit;
                    bytes = (sizeLimit * 2) / 10 + bytes;
                    sbEvents.Append("Log File Reduced by: " + bytes.ToString());

                    //create some extra space
                    int bytesSoFar = 0;

                    using (StreamReader reader = new StreamReader(logFile))
                    {
                        try
                        {
                            //Date time line
                            while (!reader.EndOfStream)
                            {
                                bytesSoFar += reader.ReadLine().Length;
                                if (bytesSoFar > bytes)
                                    break;
                            }

                            while (!reader.EndOfStream)
                            {
                                sbF.AppendLine(reader.ReadLine());
                            }
                        }
                        catch { }
                    }

                    using (StreamWriter writer = new StreamWriter(logFile))
                    {
                        writer.WriteLine(sbF);
                    }
                }
            }
        }
    }
}

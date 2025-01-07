using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public static class Log
    {
        public static StringBuilder sbEvents = new StringBuilder();

        public static void EventWriter(string message)
        {
            sbEvents.Append(DateTime.Now.ToString("T"));
            sbEvents.Append("-> ");
            sbEvents.Append(message);
            sbEvents.Append("\r");
        }

        public static void CheckLogSize(string logFile, int sizeLimit)
        {
            //system event log file
            FileInfo txtfile = new FileInfo(logFile);
            if (txtfile.Exists)
            {
                if (txtfile.Length > (sizeLimit))       // ## NOTE: 0.5MB max file size
                {
                    StringBuilder sbF = new StringBuilder();
                    long bytes = txtfile.Length - sizeLimit;
                    bytes = (sizeLimit *2)/10 + bytes;
                    Log.sbEvents.Append("Log File Reduced by: " + bytes.ToString());

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

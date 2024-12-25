using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AgOpenGPS.Logging
{
    public class SystemEventsLogger
    {
        private string _logsDirectory;
        private string _logFilename;
        private StringBuilder _systemEvents = new StringBuilder();

        public string RawLogs => _systemEvents.ToString();

        public void Initialize(string logsDirectory, string logFilename)
        {
            _logsDirectory = logsDirectory;
            _logFilename = logFilename;

            FileInfo logFile = new FileInfo(Path.Combine(_logsDirectory, _logFilename));
            if (logFile.Exists)
            {
                if (logFile.Length > (500000))       // ## NOTE: 0.5MB max file size
                {
                    _systemEvents.Append("Log File Reduced by 100Kb\r");
                    StringBuilder sbF = new StringBuilder();
                    long lines = logFile.Length - 450000;

                    //create some extra space
                    lines /= 30;

                    using (StreamReader reader = new StreamReader(Path.Combine(_logsDirectory, _logFilename)))
                    {
                        try
                        {
                            //Date time line
                            for (long i = 0; i < lines; i++)
                            {
                                reader.ReadLine();
                            }

                            while (!reader.EndOfStream)
                            {
                                sbF.AppendLine(reader.ReadLine());
                            }
                        }
                        catch { }
                    }

                    using (StreamWriter writer = new StreamWriter(Path.Combine(_logsDirectory, _logFilename)))
                    {
                        writer.WriteLine(sbF);
                    }
                }
            }
            else
            {
                _systemEvents.Append("Events Log File Created\r");
            }
        }

        public void LogEvent(string message)
        {
            _systemEvents.Append(DateTime.Now.ToString("T"));
            _systemEvents.Append("-> ");
            _systemEvents.Append(message);
            _systemEvents.Append("\r");
        }

        public void LogRaw(string message)
        {
            _systemEvents.Append(message);
        }

        public void SaveEvents()
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(_logsDirectory, _logFilename), true))
            {
                writer.Write(_systemEvents);
            }
        }

        public void ClearEvents()
        {
            _systemEvents.Clear();
        }

        public void OpenLogFileWithNotepad()
        {
            FileInfo txtfile = new FileInfo(Path.Combine(_logsDirectory, _logFilename));
            if (txtfile.Exists)
            {
                Process.Start("notepad.exe", txtfile.FullName);
            }
        }
    }
}

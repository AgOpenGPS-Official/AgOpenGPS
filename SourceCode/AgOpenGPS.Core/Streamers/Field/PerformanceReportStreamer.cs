using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Performance;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class PerformanceReportStreamer : FieldAspectStreamer
    {
        public PerformanceReportStreamer(
            IFieldStreamerPresenter presenter
        ) :
            base("PerformanceReport.txt", presenter)
        {
        }

        public void Write(PerformanceTool performanceTool, DirectoryInfo fieldDirectory)
        {
            fieldDirectory.Create();
            var timeSpans = performanceTool.TimeSpans;
            using (GeoStreamWriter writer = new GeoStreamWriter(GetFileInfo(fieldDirectory)))
            {
                writer.WriteInt(timeSpans.Count);
                foreach (PerformanceTimeSpan timeSpan in timeSpans)
                {
                    writer.WriteLine(
                        writer.IntString(timeSpan.TimerId)
                        + ", " + writer.DoubleString(timeSpan.StartTick)
                        + ", " + writer.DoubleString(timeSpan.StopTick));
                }
                writer.WriteLine("Total time :" + writer.DoubleString(performanceTool.TotalTime));
                writer.WriteLine("Id,  Name,  Count, AverageTime[ns], Fraction   ");
                for (int i = 0; i < performanceTool.TimerCount; i++)
                {
                    PerformanceTimerResult result = performanceTool.GetResult(i);
                    writer.WriteLine(
                        i
                        + ", " + performanceTool.GetName(i)
                        + ", " + result.SpanCount
                        + ", " + writer.DoubleString(result.AverageTime * 1000_000, "N3")
                        + ", " + writer.DoubleString(result.Fraction, "N4"));
                }
            }
        }
    }
}

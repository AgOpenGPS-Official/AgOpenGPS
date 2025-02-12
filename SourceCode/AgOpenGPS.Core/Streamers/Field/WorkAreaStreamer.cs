using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class WorkedAreaStreamer : FieldAspectStreamer
    {
        public WorkedAreaStreamer(
            ILogger logger
        )
            : base(logger, "Sections.txt")
        {
        }

        public void TryRead(WorkedArea workedArea, string fieldPath)
        {
            if (!File.Exists(FullPath(fieldPath)))
            {
                _presenter.PresentSectionFileMissing();
            }
            try
            {
                Read(workedArea, fieldPath);
            }
            catch (System.Exception e)
            {
                _presenter.PresentSectionFileCorrupt();
                _logger.LogError("Section file" + e.ToString());
            }
        }

        public void Read(WorkedArea workedArea, string fieldPath)
        {
            using (GeoStreamReader reader = new GeoStreamReader(FullPath(fieldPath)))
            {
                //read header
                while (!reader.EndOfStream)
                {
                    int n = reader.ReadInt();
                    int nPairs = (n - 1) / 2; // -1 beacuse first line holds ColorRGB

                    QuadStrip strip = new QuadStrip(reader.ReadColorRgb());
                    for (int i = 0; i < nPairs; i++)
                    {
                        var leftCoord = reader.ReadGeoCoord();
                        var rightCoord = reader.ReadGeoCoord();
                        strip.AddQuad(leftCoord, rightCoord);
                    }
                    workedArea.AddStrip(strip);
                }
            }
        }

        public void AppendUnsavedWork(WorkedArea workedArea, string fieldPath)
        {
            using (GeoStreamWriter writer = new GeoStreamWriter(FullPath(fieldPath), true))
            {
                //for each patch, write out the list of triangles to the file
                foreach (var quadStrip in workedArea.UnsavedWork)
                {
                    writer.WriteInt(1 + 2 * quadStrip.NumberOfPairs);
                    writer.WriteColorRgb(quadStrip.ColorRgb);
                    for (int i = 0; i < quadStrip.NumberOfPairs; i++)
                    {
                        // Add ", 0.0" to end of each line to stay backwards compatible
                        writer.WriteLine(writer.GeoCoordStringEN(quadStrip.GetLeft(i)) + ", 0.0");
                        writer.WriteLine(writer.GeoCoordStringEN(quadStrip.GetRight(i)) + ", 0.0");
                    }
                }
            }
            workedArea.ResetUnsavedWork();
        }
    }
}

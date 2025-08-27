using AgLibrary.Logging;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class WorkedAreaStreamer : FieldAspectStreamer
    {
        public WorkedAreaStreamer(
            IFieldStreamerPresenter presenter
        ) :
            base("Sections.txt", presenter)
        {
        }

        public WorkedArea TryRead(DirectoryInfo fieldDirectory)
        {
            WorkedArea workedArea = null;
            if (!GetFileInfo(fieldDirectory).Exists)
            {
                _presenter.PresentSectionFileMissing();
            }
            try
            {
                workedArea = Read(fieldDirectory);
            }
            catch (System.Exception e)
            {
                _presenter.PresentSectionFileCorrupt();
                Log.EventWriter("Section file" + e.ToString());
            }
            return workedArea;
        }

        public WorkedArea Read(DirectoryInfo fieldDirectory)
        {
            WorkedArea workedArea = new WorkedArea();
            using (GeoStreamReader reader = new GeoStreamReader(GetFileInfo(fieldDirectory)))
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
            return workedArea;
        }

        public void AppendUnsavedWork(WorkedArea workedArea, DirectoryInfo fieldDirectory)
        {
            using (GeoStreamWriter writer = new GeoStreamWriter(GetFileInfo(fieldDirectory), true))
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

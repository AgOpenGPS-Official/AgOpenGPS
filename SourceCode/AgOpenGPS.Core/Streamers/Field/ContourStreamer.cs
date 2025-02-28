using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class ContourStreamer : FieldAspectStreamer
    {
        public ContourStreamer() : base("Contour.txt")
        {
        }

        public Contour TryRead(DirectoryInfo fieldDirectory)
        {
            Contour contour = null;
            if (!File.Exists(FullPath(fieldDirectory)))
            {
                _presenter.PresentSectionFileMissing();
            }
            try
            {
                contour = Read(fieldDirectory);
            }
            catch (System.Exception e)
            {
                _presenter.PresentSectionFileCorrupt();
                Log.EventWriter("Section file" + e.ToString());
            }
            return contour;
        }

        public Contour Read(DirectoryInfo fieldDirectory)
        {
            Contour contour = new Contour();
            using (GeoStreamReader reader = new GeoStreamReader(FullPath(fieldDirectory)))
            {
                //read header
                while (!reader.EndOfStream)
                {
                    GeoPathWithHeading path = reader.ReadGeoPathWithHeading();
                    contour.Strips.Add(path);
                }
            }
            return contour;
        }

        public void AppendUnsavedWork(Contour contour, DirectoryInfo fieldDirectory)
        {
            using (GeoStreamWriter writer = new GeoStreamWriter(FullPath(fieldDirectory), true))
            {
                
                foreach (var path in contour.UnsavedStrips)
                {
                    writer.WriteGeoPathWithHeading(path);
                }
            }
            contour.UnsavedStrips.Clear();
        }
    }
}

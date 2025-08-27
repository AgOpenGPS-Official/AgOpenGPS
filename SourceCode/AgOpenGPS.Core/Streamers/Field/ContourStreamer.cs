using AgLibrary.Logging;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class ContourStreamer : FieldAspectStreamer
    {
        public ContourStreamer(
            IFieldStreamerPresenter presenter
        ) :
            base("Contour.txt", presenter)
        {
        }

        public Contour TryRead(DirectoryInfo fieldDirectory)
        {
            Contour contour = null;
            if (!GetFileInfo(fieldDirectory).Exists)
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
            using (GeoStreamReader reader = new GeoStreamReader(GetFileInfo(fieldDirectory)))
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
            using (GeoStreamWriter writer = new GeoStreamWriter(GetFileInfo(fieldDirectory), true))
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

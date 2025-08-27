using AgLibrary.Logging;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class OverviewStreamer : FieldAspectStreamer
    {
        public OverviewStreamer(
            IFieldStreamerPresenter presenter
        ) :
            base("Field.txt", presenter)
        {
        }

        public FieldOverview TryRead(DirectoryInfo directoryInfo)
        {
            FieldOverview fieldOverview = null;
            if (!GetFileInfo(directoryInfo).Exists)
            {
                _presenter.PresentBoundaryFileMissing();
            }
            else
            {
                try
                {
                    fieldOverview = Read(directoryInfo);
                }
                catch (Exception e)
                {
                    _presenter.PresentBoundaryFileCorrupt();
                    Log.EventWriter("FieldOpen, Loading Field.txt, Corrupt Field file" + e.ToString());
                }
            }
            return fieldOverview;
        }

        public FieldOverview Read(DirectoryInfo fieldDirectory)
        {
            FieldOverview fieldOverview = null;
            using (GeoStreamReader reader = new GeoStreamReader(GetFileInfo(fieldDirectory)))
            {
                reader.ReadLine(); // Skip Date time
                reader.ReadLine(); // Skip "$FieldDir"
                string creator = reader.ReadLine();
                reader.ReadLine(); // Skip "$Offsets"
                string offsets = reader.ReadLine();
                reader.ReadLine(); // Skip "Convergence
                string convergence = reader.ReadLine();
                reader.ReadLine(); // Skip "StartFix"
                Wgs84 wgs84 = reader.ReadWgs84();
                fieldOverview = new FieldOverview(creator, offsets, convergence, wgs84);
            }
            return fieldOverview;
        }

        public void Write(FieldOverview fieldOverview, DirectoryInfo fieldDirectory)
        {
            fieldDirectory.Create();
            using (GeoStreamWriter writer = new GeoStreamWriter(GetFileInfo(fieldDirectory)))
            {
                writer.WriteDateTime();
                writer.WriteLine("$FieldDir");
                writer.WriteLine(fieldOverview.Creator);
                //write out the easting and northing Offsets
                writer.WriteLine("$Offsets");
                writer.WriteString(fieldOverview.Offsets);
                writer.WriteLine("Convergence");
                writer.WriteString(fieldOverview.Convergence);
                writer.WriteLine("StartFix");
                writer.WriteWgs84(fieldOverview.Start);
            }
        }

    }
}


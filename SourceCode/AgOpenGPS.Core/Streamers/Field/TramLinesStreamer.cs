using System;
using System.IO;
using AgLibrary.Logging;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Streamers
{
    public class TramLinesStreamer : FieldAspectStreamer
    {
        public TramLinesStreamer(
            IFieldStreamerPresenter presenter
        ) :
            base("Tram.txt", presenter)
        {
        }

        public TramLines TryRead(DirectoryInfo fieldDirectory)
        {
            TramLines tramLines = null;
            try
            {
                tramLines = Read(fieldDirectory);
            }
            catch (Exception e)
            {
                _presenter.PresentTramLinesFileCorrupt();
                Log.EventWriter("Load Boundary Line" + e.ToString());
            }
            return tramLines;
        }

        public TramLines Read(DirectoryInfo fieldDirectory)
        {
            FileInfo fileInfo = GetFileInfo(fieldDirectory);
            if (!fileInfo.Exists)
            {
                return null;
            }
            TramLines tramLines = new TramLines();
            using (GeoStreamReader reader = new GeoStreamReader(fileInfo))
            {
                reader.ReadLine(); // skip header: $Tram
                if (reader.Peek() != -1)
                {
                    tramLines.OuterTrack = reader.ReadGeoCoordArray();
                    tramLines.InnerTrack = reader.ReadGeoCoordArray();

                    if (-1 != reader.Peek())
                    {
                        int nTramLines = reader.ReadInt();
                        for (int i = 0; i < nTramLines; i++)
                        {
                            GeoCoord[] tramLine = reader.ReadGeoCoordArray();
                            tramLines.TramList.Add(tramLine);
                        }
                    }
                }
            }
            return tramLines;
        }

        public void Write(TramLines tramLines, DirectoryInfo fieldDirectory)
        {
            fieldDirectory.Create();
            using (GeoStreamWriter writer = new GeoStreamWriter(GetFileInfo(fieldDirectory)))
            {
                writer.WriteLine("$Tram");
                if (null != tramLines)
                {
                    writer.WriteGeoCoordArray(tramLines.OuterTrack);
                    writer.WriteGeoCoordArray(tramLines.InnerTrack);

                    if (0 < tramLines.TramList.Count)
                    {
                        writer.WriteInt(tramLines.TramList.Count);
                        foreach (var tramLine in tramLines.TramList)
                        {
                            writer.WriteGeoCoordArray(tramLine);
                        }
                    }
                }
            }
        }
    }
}

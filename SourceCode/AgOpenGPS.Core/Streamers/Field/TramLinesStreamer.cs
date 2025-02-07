using System;
using System.IO;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Core.Streamers
{
    public class TramLinesStreamer : FieldAspectStreamer
    {
        public TramLinesStreamer(
            ILogger logger
        ) : base(logger, "Tram.txt")
        {
        }

        public TramLines TryRead(string fieldPath)
        {
            TramLines tramLines = null;
            try
            {
                tramLines = Read(fieldPath);
            }
            catch (Exception e)
            {
                _presenter.PresentTramLinesFileCorrupt();
                _logger.LogError("Load Boundary Line" + e.ToString());
            }
            return tramLines;
        }

        public TramLines Read(string fieldPath)
        {
            string fullPath = FullPath(fieldPath);
            if (!File.Exists(fullPath))
            {
                return null;
            }
            TramLines tramLines = new TramLines();
            using (GeoStreamReader reader = new GeoStreamReader(fullPath))
            {
                reader.ReadLine(); // skip header: $Tram
                if (reader.Peek() != -1)
                {
                    tramLines.OuterTrack = reader.ReadGeoPolygon();
                    tramLines.InnerTrack = reader.ReadGeoPolygon();

                    if (-1 != reader.Peek())
                    {
                        int nTramLines = reader.ReadInt();
                        for (int i = 0; i < nTramLines; i++)
                        {
                            GeoPath tramLne = reader.ReadGeoPath();
                            tramLines.TramList.Add(tramLne);
                        }
                    }
                }
            }
            return tramLines;
        }

        public void Write(TramLines tramLines, string fieldPath)
        {
            CreateDirectory(fieldPath);

            using (GeoStreamWriter writer = new GeoStreamWriter(FullPath(fieldPath)))
            {
                writer.WriteLine("$Tram");
                if (null != tramLines)
                {
                    writer.WriteGeoPolygon(tramLines.OuterTrack);
                    writer.WriteGeoPolygon(tramLines.InnerTrack);

                    if (0 < tramLines.TramList.Count)
                    {
                        writer.WriteInt(tramLines.TramList.Count);
                        foreach (var tramLine in tramLines.TramList)
                        {
                            writer.WriteGeoPath(tramLine);
                        }
                    }
                }
            }
        }
    }
}

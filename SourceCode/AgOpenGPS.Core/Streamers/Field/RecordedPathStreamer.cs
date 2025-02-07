using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class RecordedPathStreamer : FieldAspectStreamer
    {
        public RecordedPathStreamer(
            ILogger logger
        )
            : base(logger, "RecPath.txt")
        {
        }

        public RecordedPath TryRead(string fieldPath, string fileName)
        {
            RecordedPath recordedPath = null;
            try
            {
                recordedPath = Read(fieldPath, fileName);
            }
            catch (Exception e)
            {
                _presenter.PresentRecordedPathFileCorrupt();
                _logger.LogError("Load Recorded Path" + e.ToString());
            }
            return recordedPath;
        }

        private RecordedPath Read(string fieldPath, string fileName)
        {
            string fileAndDirectory = FullPath(fieldPath, fileName);
            if (!File.Exists(fileAndDirectory))
            {
                return null;
            }
            RecordedPath recordedPath = new RecordedPath();
            using (GeoStreamReader reader = new GeoStreamReader(fileAndDirectory))
            {
                reader.ReadLine(); // skip header
                int numPoints = reader.ReadInt();

                while (!reader.EndOfStream)
                {
                    for (int v = 0; v < numPoints; v++)
                    {
                        string line = reader.ReadLine();
                        string[] words = line.Split(',');
                        RecordedPoint point = new RecordedPoint(
                            reader.ParseGeoCoordDir(words[0], words[1], words[2]),
                            reader.ParseDouble(words[3]),
                            reader.ParseBool(words[4]));
                        recordedPath.Add(point);
                    }
                }
            }
            return recordedPath;
        }

        public void Write(RecordedPath path, string fieldPath, string fileName)
        {
            CreateDirectory(fieldPath);
            using (GeoStreamWriter writer = new GeoStreamWriter(FullPath(fieldPath, fileName)))
            {
                writer.WriteLine("$RecPath");
                writer.WriteInt(path.Count);
                foreach (var point in path.PointList)
                {
                    writer.WriteLine(
                        writer.GeoCoordDirStringENH(point.GeoCoordDir.Coord, point.GeoCoordDir.Direction)
                        + "," + writer.DoubleString(point.Speed, "N1")
                        + "," + writer.BoolString(point.AutoButtonState));
                }
            }
        }

        public void CreateFile(string fieldPath)
        {
            CreateDirectory(fieldPath);
            using (StreamWriter writer = new StreamWriter(FullPath(fieldPath)))
            {
                //write paths # of sections
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }
    }
}

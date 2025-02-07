using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class BoundaryStreamer : FieldAspectStreamer
    {
        public BoundaryStreamer(
            ILogger logger
        ) :
            base(logger, "Boundary.txt")
        {
        }

        public Boundary TryRead(string fieldPath)
        {
            Boundary boundary = new Boundary();
            string fullPath = FullPath(fieldPath);
            if (!File.Exists(fullPath))
            {
                _presenter.PresentBoundaryFileMissing();
            }
            else
            {
                try
                {
                    boundary = Read(fieldPath);
                }

                catch (Exception e)
                {
                    _presenter.PresentBoundaryFileCorrupt();
                    _logger.LogError("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                }
            }
            return boundary;
        }

        public Boundary Read(string fieldPath)
        {
            Boundary boundary = new Boundary();
            using (GeoStreamReader reader = new GeoStreamReader(FullPath(fieldPath)))
            {
                reader.ReadLine(); // skip header Boundary
                boundary.OuterBoundary = ReadBoundaryPolygon(reader);
                BoundaryPolygon polygon = ReadBoundaryPolygon(reader);
                while (null != polygon)
                {
                    boundary.InnerBoundaries.Add(polygon);
                    polygon = ReadBoundaryPolygon(reader);
                }
            }
            return boundary;
        }

        private BoundaryPolygon ReadBoundaryPolygon(GeoStreamReader reader)
        {
            if (reader.EndOfStream) return null;

            BoundaryPolygon polygon = new BoundaryPolygon();
            if (reader.PeekReadBool(out bool peekedBool))
            {
                polygon.IsDriveThru = peekedBool;
            }
            reader.ReadGeoPolygonWithHeading(polygon);

            return polygon;
        }

        private void WriteBoundaryPolygon(GeoStreamWriter writer, BoundaryPolygon polygon)
        {
            writer.WriteBool(polygon.IsDriveThru);
            writer.WriteGeoPolygonWithHeading(polygon);
        }

        public void Write(Boundary boundary, string fieldPath)
        {
            CreateDirectory(fieldPath);
            using (GeoStreamWriter writer = new GeoStreamWriter(FullPath(fieldPath)))
            {
                writer.WriteLine("$Boundary");
                if (boundary.OuterBoundary != null)
                {
                    WriteBoundaryPolygon(writer, boundary.OuterBoundary);
                }
                foreach (var polygon in boundary.InnerBoundaries)
                {
                    WriteBoundaryPolygon(writer, polygon);
                }
            }
        }

        public void CreateFile(string fieldPath)
        {
            CreateDirectory(fieldPath);
            File.Create(FullPath(fieldPath));
        }
    }
}


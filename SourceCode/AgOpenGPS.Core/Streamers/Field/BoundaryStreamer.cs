using AgLibrary.Logging;
using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class BoundaryStreamer : FieldAspectStreamer
    {
        public BoundaryStreamer(
            IFieldStreamerPresenter presenter
        ) :
            base("Boundary.txt", presenter)
        {
        }

        public Boundary TryRead(DirectoryInfo fieldDirectory)
        {
            Boundary boundary = new Boundary();
            FileInfo fileInfo = GetFileInfo(fieldDirectory);
            if (!fileInfo.Exists)
            {
                _presenter.PresentBoundaryFileMissing();
            }
            else
            {
                try
                {
                    boundary = Read(fieldDirectory);
                }

                catch (Exception e)
                {
                    _presenter.PresentBoundaryFileCorrupt();
                    Log.EventWriter("FieldOpen, Loading Boundary, Corrupt Boundary File" + e.ToString());
                }
            }
            return boundary;
        }

        public Boundary Read(DirectoryInfo fieldDirectory)
        {
            Boundary boundary = new Boundary();
            using (GeoStreamReader reader = new GeoStreamReader(GetFileInfo(fieldDirectory)))
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

        public void Write(Boundary boundary, DirectoryInfo fieldDirectory)
        {
            fieldDirectory.Create();
            using (GeoStreamWriter writer = new GeoStreamWriter(GetFileInfo(fieldDirectory)))
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

        public void CreateFile(DirectoryInfo fieldDirectory)
        {
            fieldDirectory.Create();
            GetFileInfo(fieldDirectory).Create();
        }
    }
}


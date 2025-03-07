using AgLibrary.Logging;
using AgOpenGPS.Core.Streamers;
using System;
using System.Diagnostics.Tracing;
using System.IO;

namespace AgOpenGPS.Core.Models
{
    // Small summary of a field.
    // Used to build a FieldDescriptionViewModel to display a row in a table of fields
    public class FieldDescription
    {
        private readonly DirectoryInfo _fieldDirectory;
        public FieldDescription(DirectoryInfo fieldDirectory, Wgs84? wgs84Start, double? area)
        {
            _fieldDirectory = fieldDirectory;
            Wgs84Start = wgs84Start;
            Area = area;
        }

        public static FieldDescription CreateFieldDescription(DirectoryInfo fieldDirectory)
        {
            Wgs84? wgs84Start = null;
            double? area = null;
            try
            {
                var overview = new OverviewStreamer().Read(fieldDirectory);
                wgs84Start = overview.Start;
            }
            catch (Exception)
            {
                Log.EventWriter("Field (" + fieldDirectory.Name + ") file (Field.txt) could not be read.");
            }
            try
            {
                var boundary = new BoundaryStreamer().Read(fieldDirectory);
                area = boundary.Area;
            }
            catch (Exception)
            {
                Log.EventWriter("Field (" + fieldDirectory.Name + ") file (Boundary.txt) could not be read.");
            }
            return new FieldDescription(fieldDirectory, wgs84Start, area);
        }

        public string Name => _fieldDirectory.Name;
        public Wgs84? Wgs84Start { get; set; } // No value indicates error in Field.txt file
        public double? Area { get; set; } // In Square meters. No value indicates error in reading Boundary file
    }

}


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
        public FieldDescription(DirectoryInfo fieldDirectory)
        {
            _fieldDirectory = fieldDirectory;
            try
            {
                var overview = new OverviewStreamer().Read(fieldDirectory);
                Wgs84Start = overview.Start;
            }
            catch (Exception )
            {
                Log.EventWriter("Field (" + _fieldDirectory.Name + ") file (Field.txt) could not be read.");
            }
            try
            {
                var boundary = new BoundaryStreamer().Read(fieldDirectory);
                Area = boundary.Area;
            }
            catch (Exception)
            {
                Log.EventWriter("Field (" + _fieldDirectory.Name + ") file (Boundary.txt) could not be read.");
            }
        }

        public string Name => _fieldDirectory.Name;
        public Wgs84? Wgs84Start { get; set; } // No value indicates error in Field.txt file
        public double? Area { get; set; } // In Square meters. No value indicates error in reading Boundary file
    }

}


using AgLibrary.Logging;
using System;
using System.Diagnostics.Tracing;
using System.IO;

namespace AgOpenGPS.Core.Models
{
    // Small summary of a field.
    // Used to build a FieldDescriptionViewModel to display a row in a table of fields
    public class FieldDescription
    {
        public FieldDescription(DirectoryInfo fieldDirectory, Wgs84? wgs84Start, double? area)
        {
            FieldDirectory = fieldDirectory;
            Wgs84Start = wgs84Start;
            Area = area;
        }

        public DirectoryInfo FieldDirectory { get; }

        public string Name => FieldDirectory.Name;
        public Wgs84? Wgs84Start { get; set; } // No value indicates error in Field.txt file
        public double? Area { get; set; } // In Square meters. No value indicates error in reading Boundary file
    }

}


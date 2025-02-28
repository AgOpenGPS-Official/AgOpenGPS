using System.Diagnostics.Tracing;

namespace AgOpenGPS.Core.Models
{
    // Small summary of a field.
    // Used to build a FieldDescriptionViewModel to display a row in a table of fields
    public class FieldDescription
    {
        public FieldDescription(string name)
        {
            Name = name;
        }

        public string Name { get; }
        public Wgs84? Wgs84Start { get; set; } // No value indicates error in Field.txt file
        public double? Area { get; set; } // In Square meters. No value indicates error in reading Boundary file

    }

}


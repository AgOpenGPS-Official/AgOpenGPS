using AgOpenGPS.Core.Models;
using System.Collections.Generic;
using System.IO;

namespace AgOpenGPS.Core.Models
{
    public class Field
    {

        // Read a Field from an already existing directory
        public Field(DirectoryInfo fieldDirectory)
        {
            FieldDirectory = fieldDirectory;
        }

        public DirectoryInfo FieldDirectory { get; }

        public string Name => FieldDirectory.Name;
        public BackgroundPicture BackgroundPicture { get; set; }
        public Boundary Boundary { get; set; }
        public Contour Contour { get; set; }
        public FieldOverview FieldOverview { get; set; }
        public List<Flag> Flags { get; set; }
        public List<HeadPath> HeadLines { get; set; }
        public RecordedPath RecordedPath { get; set; }
        public TramLines TramLines { get; set; }
        public WorkedArea WorkedArea { get; set; }

    }
}

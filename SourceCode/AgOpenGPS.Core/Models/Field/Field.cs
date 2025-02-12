using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class Field
    {

        public Field(string name)
        {
            Name = name;
        }

        public string Name { get; }
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

using AgOpenGPS.Core.Models;
using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class Field
    {
        public Field()
        {
        }

        public Boundary Boundary { get; set; }
        public List<Flag> Flags { get; set; }
        public RecordedPath RecordedPath { get; set; }
        public TramLines TramLines { get; set; }
        public WorkedArea WorkedArea { get; set; }

    }
}

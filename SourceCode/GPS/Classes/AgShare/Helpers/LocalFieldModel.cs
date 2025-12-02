using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.AgShare.Helpers
{
    /// <summary>
    /// Parsed field data ready to be written to disk.
    /// Uses domain types directly (CBoundaryList, CTrk).
    /// </summary>
    public class ParsedField
    {
        public Guid FieldId;
        public string Name;
        public Wgs84 Origin;
        public List<CBoundaryList> Boundaries = new List<CBoundaryList>();
        public List<CTrk> Tracks = new List<CTrk>();
    }
}

using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class Contour
    {
        public Contour()
        {
            Strips = new List<GeoPathWithHeading>();
            UnsavedStrips = new List<GeoPathWithHeading>();
        }

        public List<GeoPathWithHeading> Strips { get; set; }
        public List<GeoPathWithHeading> UnsavedStrips { get; set; }
    }
}

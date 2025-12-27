using System.Collections.Generic;

namespace AgOpenGPS.Core.AgShare.Models
{
    public class AbLineDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<CoordinateDto> Coords { get; set; }
    }
}

using System.Collections.Generic;

namespace AgOpenGPS.Core.AgShare.Models
{
    public class PolygonDto
    {
        public List<CoordinateDto> Outer { get; set; }
        public List<List<CoordinateDto>> Holes { get; set; }
    }
}

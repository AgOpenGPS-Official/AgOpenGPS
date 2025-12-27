using System.Collections.Generic;

namespace AgOpenGPS.Core.AgShare.Models
{
    public class UploadFieldDto
    {
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public CoordinateDto Origin { get; set; }
        public PolygonDto Boundary { get; set; }
        public List<AbLineDto> AbLines { get; set; }
    }
}

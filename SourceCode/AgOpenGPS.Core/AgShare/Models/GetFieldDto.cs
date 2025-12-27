using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.AgShare.Models
{
    public class GetFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<List<CoordinateDto>> Boundaries { get; set; }
        public List<GuidanceTrackDto> AbLines { get; set; }
    }
}

using System;

namespace AgOpenGPS.Core.AgShare.Models
{
    public class GetOwnFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public bool IsPublic { get; set; }
    }
}

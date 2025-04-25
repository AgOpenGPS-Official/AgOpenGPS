using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.AgShare
{
    public class FieldDownloadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double OriginLat { get; set; }
        public double OriginLon { get; set; }
        public List<List<double>> Boundary { get; set; }
        public List<AbLineUploadDto> AbLines { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPublic { get; set; }
        public string BoundaryJson { get; set; }
        public string ABLinesJson { get; set; }

        public FieldDownloadDto()
        {
            Name = string.Empty;
            Boundary = new List<List<double>>();
            AbLines = new List<AbLineUploadDto>();
        }
    }

    public class AbLineUploadDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<List<double>> Coords { get; set; }

        public AbLineUploadDto()
        {
            Name = string.Empty;
            Type = "AB";
            Coords = new List<List<double>>();
        }
    }
}

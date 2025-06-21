using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Classes.AgShare.Helpers;

namespace AgOpenGPS
{
    // DTO for Downloading and Uploading Fields From and to AgShare
    public class FieldSnapshot
    {
        public string FieldName { get; set; }
        public string FieldDirectory { get; set; }
        public Guid FieldId { get; set; }
        public double OriginLat { get; set; }
        public double OriginLon { get; set; }
        public double Convergence { get; set; }
        public List<List<vec3>> Boundaries { get; set; }
        public List<CTrk> Tracks { get; set; }
        public LocalPlane Converter { get; set; }
    }

    public class CoordinateDto
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class AgShareFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public List<List<CoordinateDto>> Boundaries { get; set; }
        public List<AbLineUploadDto> AbLines { get; set; }
    }


    public class AbLineUploadDto
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<CoordinateDto> Coords { get; set; }
    }

    public class AgShareGetOwnFieldDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<CoordinateDto> OuterBoundary { get; set; }
        public double AreaHa => GeoUtils.CalculateAreaInHa(OuterBoundary);

    }

}




using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;


namespace AgOpenGPS.Core.AgShare
{
    public class FieldDownloadDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsPublic { get; set; }

        [JsonPropertyName("y")]
        public double OriginLat { get; set; }

        [JsonPropertyName("x")]
        public double OriginLon { get; set; }

        public object Boundary { get; set; }
        public object AbLines { get; set; }

        [JsonIgnore]
        public List<ParsedLatLon> ParsedBoundary { get; set; }

        [JsonIgnore]
        public List<ParsedTrackLatLon> ParsedTracks { get; set; }
    }

    public class ParsedLatLon
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public class ParsedTrackLatLon
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public List<ParsedLatLon> Coords { get; set; }

        public ParsedLatLon PtA => Coords != null && Coords.Count > 0 ? Coords[0] : null;
        public ParsedLatLon PtB => Coords != null && Coords.Count > 1 ? Coords[1] : null;

        public List<ParsedLatLon> CurvePoints => Type == "Curve" ? Coords : null;
    }


    public class FieldItem
    {
        public string Name { get; set; }
        public Guid Id { get; set; }

        // Override the ToString method to display Name in the ListBox
        public override string ToString()
        {
            return $"{Name} ({Id})"; // Display Name and ID in the ListBox
        }
    }

}

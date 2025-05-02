using System;
using System.Collections.Generic;

namespace AgOpenGPS.Core.AgShare
{
    public class FieldDownloadDto
    {
        public Guid Id { get; set; } // Field ID (for agshare.txt)
        public string Name { get; set; } // Field name
        public Geometry Boundary { get; set; } // Geometry for boundary (Polygon or MultiPolygon)
        public object AbLines { get; set; } // AB Lines and Curves, as GeoJSON FeatureCollection
        public double OriginLat { get; set; } // Latitude of the origin point
        public double OriginLon { get; set; } // Longitude of the origin point

        public List<ParsedLatLon> ParsedBoundary { get; set; } // Parsed boundary for preview
        public List<ParsedTrackLatLon> ParsedTracks { get; set; } // Parsed AB lines and Curves for preview

    }

    public class Geometry
    {
        public string Type { get; set; } // "Polygon" or "MultiPolygon"
        public object Coordinates { get; set; } // Flexible: can be List<List<double>> or List<List<List<double>>>
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

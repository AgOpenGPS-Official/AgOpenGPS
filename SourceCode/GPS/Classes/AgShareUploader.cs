using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using AgOpenGPS;
using AgOpenGPS.Core.Models;

public static class AgShareUploader
{
    public static Guid GetOrGenerateFieldId(string fieldDirectory)
    {
        string path = Path.Combine(fieldDirectory, "agshare.txt");
        if (File.Exists(path))
        {
            var idText = File.ReadAllText(path).Trim();
            if (Guid.TryParse(idText, out Guid existingId))
                return existingId;
        }

        var newId = Guid.NewGuid();
        File.WriteAllText(path, newId.ToString());
        return newId;
    }

    public static object BuildFieldUploadJsonWithConversion(
    string fieldName,
    List<vec2> localBoundary,
    List<CTrk> tracks,
    LocalPlane converter)
    {
        var boundary = new List<List<double>>();
        foreach (var p in localBoundary)
        {
            var geo = new GeoCoord(p.northing, p.easting);
            var wgs = converter.ConvertGeoCoordToWgs84(geo);
            boundary.Add(new List<double> { wgs.Latitude, wgs.Longitude });
        }

        var abLines = new List<object>();

        foreach (var t in tracks)
        {
            var rawPoints = t.curvePts.Count > 0
                ? t.curvePts.Select(p => new { p.northing, p.easting })
                : new[] { t.ptA, t.ptB }.Select(p => new { p.northing, p.easting });

            var line = new List<List<double>>();

            foreach (var p in rawPoints)
            {
                var geo = new GeoCoord(p.northing, p.easting);
                var wgs = converter.ConvertGeoCoordToWgs84(geo);
                line.Add(new List<double> { wgs.Latitude, wgs.Longitude });
            }

            abLines.Add(new
            {
                name = t.name ?? "Unnamed",
                type = t.curvePts.Count > 0 ? "Curve" : "AB",
                coords = line
            });
        }

        return new
        {
            name = fieldName,
            isPublic = false,
            originLat = boundary.Average(p => p[0]),
            originLon = boundary.Average(p => p[1]),
            boundary = boundary,
            abLines = abLines
        };
    }
}

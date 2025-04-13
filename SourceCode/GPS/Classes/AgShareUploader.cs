using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AgOpenGPS;

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
    CNMEA converter)

    {
        var boundary = new List<List<double>>();
        foreach (var p in localBoundary)
        {
            converter.ConvertLocalToWGS84(p.northing, p.easting, out double lat, out double lon);
            boundary.Add(new List<double> { lat, lon });
        }

        var abLines = tracks.Select(t => new
        {
            name = t.name,
            type = t.curvePts.Count > 0 ? "Curve" : "AB",
            points = t.curvePts.Count > 0
                ? t.curvePts.Select(c =>
                {
                    converter.ConvertLocalToWGS84(c.northing, c.easting, out double lat, out double lon);
                    return new List<double> { lat, lon };
                })
                : new[] { t.ptA, t.ptB }.Select(p =>
                {
                    converter.ConvertLocalToWGS84(p.northing, p.easting, out double lat, out double lon);
                    return new List<double> { lat, lon };
                })
        }).ToList();

        return new
        {
            name = fieldName,
            isPublic = false,
            originLat = boundary.Average(p => p[0]),
            originLon = boundary.Average(p => p[1]),
            boundary,
            abLines
        };
    }

    public static async Task<bool> UploadFieldAsync(Guid fieldId, object jsonPayload)
    {
        return await AgShareApi.UploadIsoXmlFieldAsync(fieldId.ToString(), jsonPayload);
        
    }
}

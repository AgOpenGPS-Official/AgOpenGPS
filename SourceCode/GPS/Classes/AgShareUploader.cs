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

        var abLines = new List<object>();

        foreach (var t in tracks)
        {
            var rawPoints = t.curvePts.Count > 0
                ? t.curvePts.Select(p => new { p.northing, p.easting })
                : new[] { t.ptA, t.ptB }.Select(p => new { p.northing, p.easting });

            var line = new List<List<double>>();

            foreach (var p in rawPoints)
            {
                converter.ConvertLocalToWGS84(p.northing, p.easting, out double lat, out double lon);
                line.Add(new List<double> { lat, lon });
            }

            abLines.Add(new
            {
                name = t.name ?? "Unnamed",
                type = t.curvePts.Count > 0 ? "Curve" : "AB",
                coords = line
            });
        }



        // Return the final field object including the boundary and AB lines
        return new
        {
            name = fieldName,
            isPublic = false,
            originLat = boundary.Average(p => p[0]),
            originLon = boundary.Average(p => p[1]),
            boundary = boundary,
            abLines = abLines // bevat nu name, type, coords[]
        };



    }

    public static async Task<bool> UploadFieldAsync(Guid fieldId, object jsonPayload)
    {
        return await AgShareApi.UploadIsoXmlFieldAsync(fieldId.ToString(), jsonPayload);
        
    }
}

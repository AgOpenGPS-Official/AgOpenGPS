using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.IO
{
    public static class FieldPlaneFiles
    {
        public static LocalPlane LoadLocalPlaneOrDefault(string fieldDirectory, Wgs84 fallbackOrigin)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                return new LocalPlane(fallbackOrigin, new SharedFieldProperties());

            var path = Path.Combine(fieldDirectory, "Field.txt");
            if (!File.Exists(path))
                return new LocalPlane(fallbackOrigin, new SharedFieldProperties());

            var lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (lines[i].Trim().Equals("StartFix", System.StringComparison.OrdinalIgnoreCase))
                {
                    var parts = lines[i + 1].Split(',');
                    double lat, lon;
                    if (parts.Length >= 2
                        && double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lat)
                        && double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lon))
                    {
                        return new LocalPlane(new Wgs84(lat, lon), new SharedFieldProperties());
                    }
                }
            }

            return new LocalPlane(fallbackOrigin, new SharedFieldProperties());
        }
    }
}

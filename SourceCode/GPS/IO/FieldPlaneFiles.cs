using System;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class FieldPlaneFiles
    {
        /// <summary>
        /// Load the origin WGS84 coordinate from Field.txt. 
        /// Throws an exception if no valid StartFix is present.
        /// </summary>
        public static Wgs84 LoadOrigin(string fieldDirectory)
        {
            var path = Path.Combine(fieldDirectory, "Field.txt");
            if (!File.Exists(path))
                throw new FileNotFoundException("Field.txt not found", path);

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null && line.StartsWith("StartFix", StringComparison.OrdinalIgnoreCase))
                    {
                        var next = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(next))
                            throw new InvalidDataException("StartFix line missing or empty in Field.txt");

                        var parts = next.Split(',');
                        if (parts.Length >= 2 &&
                            double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var lat) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
                        {
                            return new Wgs84(lat, lon);
                        }

                        throw new InvalidDataException("Invalid StartFix format in Field.txt");
                    }
                }
            }

            throw new InvalidDataException("StartFix not found in Field.txt");
        }
    }

}

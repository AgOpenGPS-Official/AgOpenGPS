// AgOpenGPS.IO/FieldPlaneFiles.cs
// Purpose: Read/write helpers for Field.txt (StartFix origin + creation).
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
            {
                throw new FileNotFoundException("Field.txt not found", path);
            }

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (line != null && line.StartsWith("StartFix", StringComparison.OrdinalIgnoreCase))
                    {
                        var next = reader.ReadLine();
                        if (string.IsNullOrWhiteSpace(next))
                        {
                            throw new InvalidDataException("StartFix line missing or empty in Field.txt");
                        }

                        var parts = next.Split(',');
                        double lat, lon;
                        if (parts.Length >= 2 &&
                            double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lat) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lon))
                        {
                            return new Wgs84(lat, lon);
                        }

                        throw new InvalidDataException("Invalid StartFix format in Field.txt");
                    }
                }
            }

            throw new InvalidDataException("StartFix not found in Field.txt");
        }

        /// <summary>
        /// Create or overwrite Field.txt with a StartFix origin and standard header.
        /// Returns true on success; false on failure (error contains a short description).
        /// Stateless: no UI, no globals, only file IO.
        /// </summary>
        public static void Save(string fieldDirectory, DateTime timestamp, Wgs84 startFix)
        {
            var path = Path.Combine(fieldDirectory, "Field.txt");
            using (var writer = new StreamWriter(path, false))
            {
                writer.WriteLine(timestamp.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                writer.WriteLine("$FieldDir");
                writer.WriteLine("FieldNew");
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");
                writer.WriteLine("Convergence");
                writer.WriteLine("0");
                writer.WriteLine("StartFix");
                writer.WriteLine(
                    startFix.Latitude.ToString(CultureInfo.InvariantCulture) + "," +
                    startFix.Longitude.ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}

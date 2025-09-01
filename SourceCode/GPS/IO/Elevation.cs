// AgOpenGPS.IO/ElevationFiles.cs
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    /// <summary>
    /// Stateless reader/writer helpers for Elevation.txt.
    /// </summary>
    public static class ElevationFiles
    {
        /// <summary>
        /// Create or overwrite Elevation.txt with header.
        /// </summary>
        public static void CreateHeader(string fieldDirectory, DateTime timestamp, Wgs84 startFix)
        {
            if (string.IsNullOrEmpty(fieldDirectory))
            {
                throw new ArgumentNullException(nameof(fieldDirectory));
            }

            if (!Directory.Exists(fieldDirectory))
            {
                Directory.CreateDirectory(fieldDirectory);
            }

            var path = Path.Combine(fieldDirectory, "Elevation.txt");
            using (var writer = new StreamWriter(path, false))
            {
                writer.WriteLine(timestamp.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
                writer.WriteLine("$FieldDir");
                writer.WriteLine("Elevation");
                writer.WriteLine("$Offsets");
                writer.WriteLine("0,0");
                writer.WriteLine("Convergence");
                writer.WriteLine("0");
                writer.WriteLine("StartFix");
                writer.WriteLine(
                    startFix.Latitude.ToString(CultureInfo.InvariantCulture) + "," +
                    startFix.Longitude.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("Latitude,Longitude,Elevation,Quality,Easting,Northing,Heading,Roll");
            }
        }

        /// <summary>
        /// Append elevation grid text to Elevation.txt.
        /// </summary>
        public static void Append(string fieldDirectory, string gridText)
        {
            if (string.IsNullOrEmpty(fieldDirectory))
            {
                throw new ArgumentNullException(nameof(fieldDirectory));
            }

            if (!Directory.Exists(fieldDirectory))
            {
                Directory.CreateDirectory(fieldDirectory);
            }

            var path = Path.Combine(fieldDirectory, "Elevation.txt");
            using (var writer = new StreamWriter(path, true))
            {
                writer.Write(gridText);
            }
        }
        public sealed class ElevationData
        {
            public DateTime Created { get; set; }
            public Wgs84 StartFix { get; set; }
            public List<string> RawLines { get; set; } = new List<string>();
        }

        public static ElevationData Load(string fieldDirectory)
        {
            var path = Path.Combine(fieldDirectory, "Elevation.txt");
            if (!File.Exists(path)) return new ElevationData();

            var data = new ElevationData();
            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    data.RawLines.Add(line);
                }
            }
            return data;
        }

    }
}

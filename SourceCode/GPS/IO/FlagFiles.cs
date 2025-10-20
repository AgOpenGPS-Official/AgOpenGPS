using AgLibrary.Logging;
using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AgOpenGPS.IO
{
    public static class FlagsFiles
    {
        public static List<CFlag> DeduplicateFlags(IEnumerable<CFlag> flags)
        {
            var distinctFlags = new List<CFlag>();
            foreach (var f in flags)
            {
                bool duplicate = distinctFlags.Any(d =>
                    Math.Abs(d.latitude - f.latitude) < 1e-8 &&
                    Math.Abs(d.longitude - f.longitude) < 1e-8);

                if (!duplicate)
                {
                    distinctFlags.Add(f);
                }
            }
            return distinctFlags;
        }

        public static List<CFlag> Load(string fieldDirectory)
        {
            var result = new List<CFlag>();
            var path = Path.Combine(fieldDirectory, "Flags.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                reader.ReadLine(); // header
                var line = reader.ReadLine();
                int count;
                if (!int.TryParse(line, out count)) return result;

                for (int i = 0; i < count; i++)
                {
                    var words = (reader.ReadLine() ?? string.Empty).Split(',');
                    if (words.Length < 6) continue;

                    double lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                    double lon = double.Parse(words[1], CultureInfo.InvariantCulture);
                    double easting = double.Parse(words[2], CultureInfo.InvariantCulture);
                    double northing = double.Parse(words[3], CultureInfo.InvariantCulture);
                    double heading = (words.Length >= 8)
                        ? double.Parse(words[4], CultureInfo.InvariantCulture)
                        : 0;
                    int color = int.Parse(words[words.Length >= 8 ? 5 : 4], CultureInfo.InvariantCulture);
                    int id = int.Parse(words[words.Length >= 8 ? 6 : 5], CultureInfo.InvariantCulture);
                    string notes = (words.Length >= 8 ? words[7] : "").Trim();

                    // Use the same duplicate check as in Save
                    bool duplicate = result.Any(d =>
                        Math.Abs(d.latitude - lat) < 1e-8 &&
                        Math.Abs(d.longitude - lon) < 1e-8);

                    if (!duplicate)
                    {
                        result.Add(new CFlag(lat, lon, easting, northing, heading, color, id, notes));
                    }
                }
            }

            return result;
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CFlag> flags)
        {

            var filename = Path.Combine(fieldDirectory, "Flags.txt");

            // Prevent saving duplicates based on latitude and longitude
            var distinctFlags = new List<CFlag>();
            if (flags != null)
            {
                foreach (var f in flags ?? new List<CFlag>())
                {
                    bool duplicate = distinctFlags.Any(d =>
                        Math.Abs(d.latitude - f.latitude) < 1e-8 &&
                        Math.Abs(d.longitude - f.longitude) < 1e-8);

                    if (!duplicate)
                    {
                        distinctFlags.Add(f);
                    }
                }
            }
            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Flags");
                writer.WriteLine(distinctFlags.Count.ToString(CultureInfo.InvariantCulture));

                for (int i = 0; i < distinctFlags.Count; i++)
                {
                    var f = distinctFlags[i];
                    writer.WriteLine(
                        f.latitude.ToString(CultureInfo.InvariantCulture) + "," +
                        f.longitude.ToString(CultureInfo.InvariantCulture) + "," +
                        f.easting.ToString(CultureInfo.InvariantCulture) + "," +
                        f.northing.ToString(CultureInfo.InvariantCulture) + "," +
                        f.heading.ToString(CultureInfo.InvariantCulture) + "," +
                        f.color.ToString(CultureInfo.InvariantCulture) + "," +
                        f.ID.ToString(CultureInfo.InvariantCulture) + "," +
                        (f.notes ?? string.Empty));
                }
            }
        }
    }
}

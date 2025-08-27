using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;
using AgLibrary.Logging;

namespace AgOpenGPS.IO
{
    public static class FlagsFiles
    {
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

                    result.Add(new CFlag(lat, lon, easting, northing, heading, color, id, notes));
                }
            }

            return result;
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CFlag> flags)
        {
            var filename = Path.Combine(fieldDirectory, "Flags.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Flags");

                var list = flags ?? new List<CFlag>();
                writer.WriteLine(list.Count.ToString(CultureInfo.InvariantCulture));

                for (int i = 0; i < list.Count; i++)
                {
                    var f = list[i];
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

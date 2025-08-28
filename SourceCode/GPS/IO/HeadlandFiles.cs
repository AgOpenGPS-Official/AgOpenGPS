using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class HeadlandFiles
    {
        public static void AttachLoad(string fieldDirectory, List<CBoundaryList> boundaries)
        {
            if (boundaries == null || boundaries.Count == 0) return;

            var path = Path.Combine(fieldDirectory, "Headland.txt");
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path);
            int idx = 0;

            if (idx < lines.Length && lines[idx].Trim().StartsWith("$")) idx++;

            for (int k = 0; k < boundaries.Count && idx < lines.Length; k++)
            {
                int count;
                if (!int.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
                    break;

                var hd = boundaries[k].hdLine;
                if (hd != null) hd.Clear();

                for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                {
                    var parts = (lines[idx] ?? string.Empty).Split(',');
                    if (parts.Length < 3) continue;

                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double easting) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double northing) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double heading))
                    {
                        boundaries[k].hdLine.Add(new vec3(easting, northing, heading));
                    }

                }
            }
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CBoundaryList> boundaries)
        {
            var filename = Path.Combine(fieldDirectory, "Headland.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Headland");

                if (boundaries == null || boundaries.Count == 0) return;
                if (boundaries[0].hdLine == null || boundaries[0].hdLine.Count == 0) return;

                for (int i = 0; i < boundaries.Count; i++)
                {
                    var hd = boundaries[i].hdLine ?? new List<vec3>();
                    writer.WriteLine(hd.Count.ToString(CultureInfo.InvariantCulture));

                    for (int j = 0; j < hd.Count; j++)
                    {
                        var p = hd[j];
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)},{FileIoUtils.FormatDouble(p.northing, 3)},{FileIoUtils.FormatDouble(p.heading, 5)}");
                    }
                }
            }
        }
    }
}

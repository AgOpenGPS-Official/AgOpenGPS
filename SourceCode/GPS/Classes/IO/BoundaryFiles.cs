using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.IO
{
    public static class BoundaryFiles
    {
        public static List<CBoundaryList> Load(string fieldDirectory)
        {
            var result = new List<CBoundaryList>();
            var path = Path.Combine(fieldDirectory ?? "", "Boundary.txt");
            if (!File.Exists(path)) return result;

            var lines = File.ReadAllLines(path);
            int idx = 0;

            if (idx < lines.Length && lines[idx].Trim().StartsWith("$", StringComparison.Ordinal)) idx++;

            for (int ringIndex = 0; idx < lines.Length; ringIndex++)
            {
                if (idx >= lines.Length) break;
                var line = (lines[idx++] ?? string.Empty).Trim();

                var b = new CBoundaryList();

                bool isFlag;
                if (bool.TryParse(line, out isFlag))
                {
                    b.isDriveThru = isFlag;
                    if (idx >= lines.Length) break;
                    line = (lines[idx++] ?? string.Empty).Trim();
                }

                int count;
                if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
                    break;

                for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                {
                    var parts = (lines[idx] ?? string.Empty).Split(',');
                    if (parts.Length < 3) continue;

                    double e, n, h;
                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out h))
                    {
                        b.fenceLine.Add(new vec3(e, n, h));
                    }
                }

                b.CalculateFenceArea(ringIndex);

                if (b.fenceLineEar != null) b.fenceLineEar.Clear();
                double delta = 0;
                for (int i = 0; i < b.fenceLine.Count; i++)
                {
                    if (i == 0)
                    {
                        b.fenceLineEar.Add(new vec2(b.fenceLine[i].easting, b.fenceLine[i].northing));
                        continue;
                    }
                    delta += (b.fenceLine[i - 1].heading - b.fenceLine[i].heading);
                    if (Math.Abs(delta) > 0.005)
                    {
                        b.fenceLineEar.Add(new vec2(b.fenceLine[i].easting, b.fenceLine[i].northing));
                        delta = 0;
                    }
                }

                result.Add(b);
                while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
            }

            return result;
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CBoundaryList> boundaries)
        {
            FileIoUtils.EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "Boundary.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Boundary");

                if (boundaries == null || boundaries.Count == 0) return;

                for (int i = 0; i < boundaries.Count; i++)
                {
                    var b = boundaries[i];
                    var fence = b.fenceLine ?? new List<vec3>();

                    writer.WriteLine(b.isDriveThru.ToString());
                    writer.WriteLine(fence.Count.ToString(CultureInfo.InvariantCulture));

                    for (int j = 0; j < fence.Count; j++)
                    {
                        var p = fence[j];
                        writer.WriteLine($"{FileIoUtils.F3(p.easting)},{FileIoUtils.F3(p.northing)},{FileIoUtils.F5(p.heading)}");
                    }
                }
            }
        }
    }
}

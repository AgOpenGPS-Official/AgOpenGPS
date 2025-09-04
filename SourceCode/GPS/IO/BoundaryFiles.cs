// BoundaryFiles.cs - Load tolerant to duplicate True/False lines and extra whitespace.
// Purpose: Some legacy files wrote the drive-through flag twice; we accept that pattern.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class BoundaryFiles
    {
        public static List<CBoundaryList> Load(string fieldDirectory)
        {
            var result = new List<CBoundaryList>();
            var path = Path.Combine(fieldDirectory, "Boundary.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                // Skip optional header
                string line = reader.ReadLine();
                if (line != null && !line.TrimStart().StartsWith("$", StringComparison.OrdinalIgnoreCase))
                {
                    // first line was not header -> treat as first data line
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    reader.DiscardBufferedData();
                }

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    var b = new CBoundaryList();

                    // Some legacy wrote "True/False" twice; accept and consume up to two flags.
                    for (int pass = 0; pass < 2; pass++)
                    {
                        bool flag;
                        if (bool.TryParse(line.Trim(), out flag))
                        {
                            b.isDriveThru = flag;
                            line = reader.ReadLine();
                            if (line == null) break;
                            continue;
                        }
                        break;
                    }

                    if (line == null) break;
                    var countLine = line.Trim();
                    int count;
                    if (!int.TryParse(countLine, NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
                    {
                        break; // malformed count -> stop parsing rings
                    }

                    // Points
                    for (int i = 0; i < count; i++)
                    {
                        line = reader.ReadLine();
                        if (line == null) break;
                        var parts = line.Split(',');
                        if (parts.Length < 3) continue;
                        double easting, northing, heading;
                        if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out easting) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out northing) &&
                            double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out heading))
                        {
                            b.fenceLine.Add(new vec3(easting, northing, heading));
                        }
                    }

                    // Compute area and ear
                    b.CalculateFenceArea(result.Count);
                    if (b.fenceLineEar != null) b.fenceLineEar.Clear();

                    double delta = 0;
                    for (int i = 0; i < b.fenceLine.Count; i++)
                    {
                        if (i == 0)
                        {
                            b.fenceLineEar.Add(new vec2(b.fenceLine[i].easting, b.fenceLine[i].northing));
                            continue;
                        }
                        delta += b.fenceLine[i - 1].heading - b.fenceLine[i].heading;
                        if (Math.Abs(delta) > 0.005)
                        {
                            b.fenceLineEar.Add(new vec2(b.fenceLine[i].easting, b.fenceLine[i].northing));
                            delta = 0;
                        }
                    }

                    result.Add(b);
                }

                return result;
            }
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CBoundaryList> boundaries)
        {
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
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)},{FileIoUtils.FormatDouble(p.northing, 3)},{FileIoUtils.FormatDouble(p.heading, 5)}");
                    }
                }
            }
        }
        public static void CreateEmpty(string fieldDirectory)
        {
            if (string.IsNullOrEmpty(fieldDirectory))
            {
                throw new ArgumentNullException(nameof(fieldDirectory));
            }

            if (!Directory.Exists(fieldDirectory))
            {
                Directory.CreateDirectory(fieldDirectory);
            }

            var path = Path.Combine(fieldDirectory, "Boundary.txt");
            File.WriteAllText(path, "$Boundary" + Environment.NewLine, Encoding.UTF8);
        }
    }
}

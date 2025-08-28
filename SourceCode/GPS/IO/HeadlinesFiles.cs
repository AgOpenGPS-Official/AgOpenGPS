using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class HeadlinesFiles
    {
        public static List<CHeadPath> Load(string fieldDirectory)
        {
            var result = new List<CHeadPath>();
            var path = Path.Combine(fieldDirectory, "Headlines.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                try
                {
                    reader.ReadLine(); // optional header
                    while (!reader.EndOfStream)
                    {
                        var hp = new CHeadPath();
                        hp.name = reader.ReadLine() ?? string.Empty;

                        var line = reader.ReadLine(); if (line == null) break;
                        hp.moveDistance = double.Parse(line, CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); if (line == null) break;
                        hp.mode = int.Parse(line, CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); if (line == null) break;
                        hp.a_point = int.Parse(line, CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); if (line == null) break;
                        int numPoints = int.Parse(line, CultureInfo.InvariantCulture);

                        hp.trackPts = new List<vec3>();
                        for (int i = 0; i < numPoints && !reader.EndOfStream; i++)
                        {
                            var words = (reader.ReadLine() ?? string.Empty).Split(',');
                            if (words.Length < 3) continue;

                            if (double.TryParse(words[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double easting) &&
                                double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double northing) &&
                                double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double heading))
                            {
                                hp.trackPts.Add(new vec3(easting, northing, heading));
                            }
                        }

                        if (hp.trackPts.Count > 3) result.Add(hp);
                    }
                }
                catch
                {
                    result.Clear();
                }
            }

            return result;
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CHeadPath> headPaths)
        {
            var filename = Path.Combine(fieldDirectory, "Headlines.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$HeadLines");
                if (headPaths == null || headPaths.Count == 0) return;

                for (int i = 0; i < headPaths.Count; i++)
                {
                    var hp = headPaths[i];
                    writer.WriteLine(hp.name);
                    writer.WriteLine(hp.moveDistance.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(hp.mode.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(hp.a_point.ToString(CultureInfo.InvariantCulture));

                    var pts = hp.trackPts ?? new List<vec3>();
                    writer.WriteLine(pts.Count.ToString(CultureInfo.InvariantCulture));

                    for (int j = 0; j < pts.Count; j++)
                    {
                        var p = pts[j];
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)} , {FileIoUtils.FormatDouble(p.northing, 3)} , {FileIoUtils.FormatDouble(p.heading, 5)}");
                    }
                }
            }
        }
    }
}

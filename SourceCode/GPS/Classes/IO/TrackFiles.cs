using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.IO
{
    public static class TrackFiles
    {
        public static List<CTrk> Load(string fieldDirectory)
        {
            var result = new List<CTrk>();
            var path = Path.Combine(fieldDirectory ?? "", "TrackLines.txt");
            if (!File.Exists(path)) return result;

            var lines = File.ReadAllLines(path);
            int idx = 0;

            if (idx < lines.Length && lines[idx].Trim().StartsWith("$", StringComparison.Ordinal)) idx++;

            while (idx < lines.Length)
            {
                var name = (lines[idx++] ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(name)) { FileIoUtils.SkipBlanks(lines, ref idx); continue; }

                if (idx >= lines.Length) break;
                double avgHeading;
                if (!double.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out avgHeading))
                    avgHeading = 0.0;

                if (idx >= lines.Length) break;
                double aE, aN;
                if (!FileIoUtils.TryParseEN(lines[idx++], out aE, out aN)) { FileIoUtils.SkipBlanks(lines, ref idx); continue; }

                if (idx >= lines.Length) break;
                double bE, bN;
                if (!FileIoUtils.TryParseEN(lines[idx++], out bE, out bN)) { FileIoUtils.SkipBlanks(lines, ref idx); continue; }

                if (idx < lines.Length) idx++; // nudge
                var mode = 0;
                if (idx < lines.Length) int.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out mode);
                if (idx < lines.Length) idx++; // isVisible

                var curveCount = 0;
                if (idx < lines.Length) int.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out curveCount);

                var curvePts = new List<vec3>();
                for (int i = 0; i < curveCount && idx < lines.Length; i++, idx++)
                {
                    var parts = (lines[idx] ?? string.Empty).Split(',');
                    if (parts.Length >= 3)
                    {
                        double e, n, h;
                        if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n) &&
                            double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out h))
                        {
                            curvePts.Add(new vec3(e, n, h));
                        }
                    }
                }

                if (mode == (int)TrackMode.AB)
                {
                    var abHeading = Math.Atan2(bE - aE, bN - aN);
                    var tr = new CTrk { name = name, mode = TrackMode.AB, ptA = new vec2(aE, aN), ptB = new vec2(bE, bN), heading = abHeading };
                    result.Add(tr);
                }
                else if (mode == (int)TrackMode.Curve && curvePts.Count > 1)
                {
                    var tr = new CTrk { name = name, mode = TrackMode.Curve, curvePts = curvePts, heading = avgHeading };
                    result.Add(tr);
                }

                FileIoUtils.SkipBlanks(lines, ref idx);
            }

            return result;
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CTrk> tracks)
        {
            FileIoUtils.EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "TrackLines.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$TrackLines");

                if (tracks == null || tracks.Count == 0) return;

                for (int i = 0; i < tracks.Count; i++)
                {
                    var t = tracks[i];

                    writer.WriteLine(t.name ?? string.Empty);
                    writer.WriteLine(t.heading.ToString(CultureInfo.InvariantCulture));

                    writer.WriteLine($"{FileIoUtils.F3(t.ptA.easting)},{FileIoUtils.F3(t.ptA.northing)}");
                    writer.WriteLine($"{FileIoUtils.F3(t.ptB.easting)},{FileIoUtils.F3(t.ptB.northing)}");

                    writer.WriteLine(t.nudgeDistance.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(((int)t.mode).ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(t.isVisible.ToString());

                    var pts = t.curvePts ?? new List<vec3>();
                    writer.WriteLine(pts.Count.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < pts.Count; j++)
                    {
                        var p = pts[j];
                        writer.WriteLine($"{FileIoUtils.F3(p.easting)},{FileIoUtils.F3(p.northing)},{FileIoUtils.F5(p.heading)}");
                    }
                }
            }
        }
    }
}

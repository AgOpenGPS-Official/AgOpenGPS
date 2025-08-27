using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class TrackFiles
    {
        public static List<CTrk> Load(string fieldDirectory)
        {
            var result = new List<CTrk>();
            var path = Path.Combine(fieldDirectory, "TrackLines.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                // Skip optional header
                var firstLine = reader.ReadLine();
                if (firstLine == null) return result;
                if (!firstLine.TrimStart().StartsWith("$", StringComparison.Ordinal))
                {
                    // rewind logic: treat this line as first "name"
                    reader.BaseStream.Seek(0, SeekOrigin.Begin);
                    reader.DiscardBufferedData();
                }

                while (!reader.EndOfStream)
                {
                    // --- Name ---
                    var name = (reader.ReadLine() ?? string.Empty).Trim();
                    if (string.IsNullOrEmpty(name)) continue;

                    // --- Average heading ---
                    if (reader.EndOfStream) break;
                    var headingLine = reader.ReadLine() ?? string.Empty;
                    double avgHeading;
                    if (!double.TryParse(headingLine.Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out avgHeading))
                        avgHeading = 0.0;

                    // --- A point ---
                    if (reader.EndOfStream) break;
                    var aLine = reader.ReadLine();
                    double aE, aN;
                    if (!FileIoUtils.TryParseEN(aLine, out aE, out aN)) continue;

                    // --- B point ---
                    if (reader.EndOfStream) break;
                    var bLine = reader.ReadLine();
                    double bE, bN;
                    if (!FileIoUtils.TryParseEN(bLine, out bE, out bN)) continue;

                    // --- Nudge (skip) ---
                    if (reader.EndOfStream) break;
                    reader.ReadLine();

                    // --- Mode ---
                    int mode = 0;
                    if (!reader.EndOfStream)
                    {
                        var modeLine = reader.ReadLine() ?? string.Empty;
                        int.TryParse(modeLine.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out mode);
                    }

                    // --- Visibility (skip) ---
                    if (!reader.EndOfStream) reader.ReadLine();

                    // --- Curve count ---
                    int curveCount = 0;
                    if (!reader.EndOfStream)
                    {
                        var countLine = reader.ReadLine() ?? string.Empty;
                        int.TryParse(countLine.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out curveCount);
                    }

                    // --- Curve points ---
                    var curvePts = new List<vec3>();
                    for (int i = 0; i < curveCount && !reader.EndOfStream; i++)
                    {
                        var parts = (reader.ReadLine() ?? string.Empty).Split(',');
                        if (parts.Length >= 3 &&
                            double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double easting) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double northing) &&
                            double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double heading))
                        {
                            curvePts.Add(new vec3(easting, northing, heading));
                        }
                    }

                    // --- Build track object ---
                    if (mode == (int)TrackMode.AB)
                    {
                        var abHeading = Math.Atan2(bE - aE, bN - aN);
                        var tr = new CTrk
                        {
                            name = name,
                            mode = TrackMode.AB,
                            ptA = new vec2(aE, aN),
                            ptB = new vec2(bE, bN),
                            heading = abHeading
                        };
                        result.Add(tr);
                    }
                    else if (mode == (int)TrackMode.Curve && curvePts.Count > 1)
                    {
                        var tr = new CTrk
                        {
                            name = name,
                            mode = TrackMode.Curve,
                            curvePts = curvePts,
                            heading = avgHeading
                        };
                        result.Add(tr);
                    }
                }
            }

            return result;
        }


        public static void Save(string fieldDirectory, IReadOnlyList<CTrk> tracks)
        {
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

                    writer.WriteLine($"{FileIoUtils.FormatDouble(t.ptA.easting, 3)},{FileIoUtils.FormatDouble(t.ptA.northing, 3)}");
                    writer.WriteLine($"{FileIoUtils.FormatDouble(t.ptB.easting, 3)},{FileIoUtils.FormatDouble(t.ptB.northing, 3)}");

                    writer.WriteLine(t.nudgeDistance.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(((int)t.mode).ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(t.isVisible.ToString());

                    var pts = t.curvePts ?? new List<vec3>();
                    writer.WriteLine(pts.Count.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < pts.Count; j++)
                    {
                        var p = pts[j];
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)},{FileIoUtils.FormatDouble(p.northing, 3)},{FileIoUtils.FormatDouble(p.heading, 5)}");
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AgOpenGPS.Classes
{
    internal static class CTrackLineReader
    {
        public static (List<CTrk> tracks, string error) LoadTrackLines(string fieldDirectory)
        {
            var result = new List<CTrk>();
            string error = null;

            string trackFilePath = Path.Combine(RegistrySettings.fieldsDirectory, fieldDirectory, "TrackLines.txt");

            if (!File.Exists(trackFilePath))
            {
                return (result, "TrackLines.txt not found.");
            }

            try
            {
                string[] lines = File.ReadAllLines(trackFilePath);
                int index = 0;

                if (lines.Length == 0 || !lines[0].StartsWith("$TrackLines"))
                {
                    return (result, "Invalid or missing header in TrackLines.txt.");
                }

                index++; // Skip $TrackLines

                while (index < lines.Length)
                {
                    if (index + 7 >= lines.Length)
                        break;

                    var trk = new CTrk();

                    trk.name = lines[index++];
                    double.TryParse(lines[index++], NumberStyles.Float, CultureInfo.InvariantCulture, out trk.heading);

                    trk.ptA = ParseVec2(lines[index++]);
                    trk.ptB = ParseVec2(lines[index++]);

                    double.TryParse(lines[index++], NumberStyles.Float, CultureInfo.InvariantCulture, out trk.nudgeDistance);

                    int modeInt;
                    int.TryParse(lines[index++], out modeInt);
                    trk.mode = (TrackMode)modeInt;

                    bool.TryParse(lines[index++], out trk.isVisible);

                    int curveCount;
                    int.TryParse(lines[index++], out curveCount);

                    trk.curvePts.Clear();
                    for (int i = 0; i < curveCount && index < lines.Length; i++)
                    {
                        var parts = lines[index++].Split(',');
                        if (parts.Length >= 3 &&
                            double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double e) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double n) &&
                            double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out double h))
                        {
                            trk.curvePts.Add(new vec3(e, n, h));
                        }
                    }

                    result.Add(trk);
                }
            }
            catch (Exception ex)
            {
                error = $"Error reading TrackLines.txt: {ex.Message}";
            }

            return (result, error);
        }

        private static vec2 ParseVec2(string line)
        {
            return vec2.TryParse(line, out var v) ? v : new vec2();
        }
    }
}

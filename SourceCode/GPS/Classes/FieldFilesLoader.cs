using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes
{
    /// <summary>
    /// Stateless file readers for field data.
    /// All methods only parse files from a field directory and return plain models.
    /// No UI dependencies, no global state access.
    /// </summary>
    public static class FieldFilesLoader
    {
        /// <summary>
        /// Tries to read a LocalPlane origin from Field.txt ("StartFix" lat,lon).
        /// If not found or invalid, returns a plane using <paramref name="fallbackOrigin"/>.
        /// </summary>
        public static LocalPlane LoadLocalPlaneOrDefault(string fieldDirectory, Wgs84 fallbackOrigin)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                return new LocalPlane(fallbackOrigin, new SharedFieldProperties());

            var path = Path.Combine(fieldDirectory, "Field.txt");
            if (!File.Exists(path))
                return new LocalPlane(fallbackOrigin, new SharedFieldProperties());

            var lines = File.ReadAllLines(path);
            for (int i = 0; i < lines.Length - 1; i++)
            {
                if (lines[i].Trim().Equals("StartFix", StringComparison.OrdinalIgnoreCase))
                {
                    var parts = lines[i + 1].Split(',');
                    if (parts.Length >= 2
                        && double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var lat)
                        && double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var lon))
                    {
                        return new LocalPlane(new Wgs84(lat, lon), new SharedFieldProperties());
                    }
                }
            }

            return new LocalPlane(fallbackOrigin, new SharedFieldProperties());
        }

        /// <summary>
        /// Reads boundaries from Boundary.txt into a list of CBoundaryList, including:
        /// - fenceLine (vec3 E,N,H from file),
        /// - fenceLineEar (decimated vec2(E,N) like MainForm),
        /// - area calculation via CBoundaryList.CalculateFenceArea(index).
        /// The "ring"/drive-thru flags are preserved if present.
        /// </summary>
        public static List<CBoundaryList> LoadBoundaries(string fieldDirectory)
        {
            var result = new List<CBoundaryList>();
            var path = Path.Combine(fieldDirectory ?? "", "Boundary.txt");
            if (!File.Exists(path)) return result;

            var lines = File.ReadAllLines(path);
            int idx = 0;

            // Skip "$Boundary" header if present
            if (idx < lines.Length && lines[idx].Trim().StartsWith("$", StringComparison.Ordinal)) idx++;

            for (int ringIndex = 0; idx < lines.Length; ringIndex++)
            {
                // Line can be: "True"/"False" (drive-thru/inner/outer) or the point count (older files)
                if (idx >= lines.Length) break;
                var line = lines[idx++].Trim();

                var b = new CBoundaryList();

                bool isFlag;
                if (bool.TryParse(line, out isFlag))
                {
                    // Current format: first a boolean flag, then a point count
                    b.isDriveThru = isFlag;                   // same behavior as in FormGPS
                    if (idx >= lines.Length) break;
                    line = lines[idx++].Trim();
                }

                if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out var count))
                    break;

                // Read E,N,H points as vec3
                for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                {
                    var parts = lines[idx].Split(',');
                    if (parts.Length < 3) continue;

                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var n) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var h))
                    {
                        b.fenceLine.Add(new vec3(e, n, h));
                    }
                }

                // Match MainForm behavior: compute area and decimate fenceLineEar
                b.CalculateFenceArea(ringIndex);

                b.fenceLineEar?.Clear();
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

                // Skip blank separators
                while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
            }

            return result;
        }

        /// <summary>
        /// Reads headland(s) and attaches them to the provided boundary list:
        /// each block contains a point count followed by E,N,H lines -> stored in hdLine.
        /// If file is missing, boundaries are left unchanged.
        /// </summary>
        public static void AttachHeadlands(string fieldDirectory, List<CBoundaryList> boundaries)
        {
            if (boundaries == null || boundaries.Count == 0) return;

            var path = Path.Combine(fieldDirectory ?? "", "Headland.txt");
            if (!File.Exists(path)) return;

            var lines = File.ReadAllLines(path);
            int idx = 0;

            // Skip "$Headland" header if present
            if (idx < lines.Length && lines[idx].Trim().StartsWith("$", StringComparison.Ordinal)) idx++;

            for (int k = 0; k < boundaries.Count && idx < lines.Length; k++)
            {
                if (!int.TryParse(lines[idx++].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var count))
                    break;

                var hd = boundaries[k].hdLine;
                hd?.Clear();

                for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                {
                    var parts = lines[idx].Split(',');
                    if (parts.Length < 3) continue;

                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var n) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var h))
                    {
                        hd.Add(new vec3(e, n, h));
                    }
                }
            }
        }

        /// <summary>
        /// Reads AB and Curve tracks from TrackLines.txt into a List&lt;CTrk&gt;.
        /// - For AB: ptA/ptB as vec2 (E,N) and heading from A->B.
        /// - For Curve: curvePts as vec3(E,N,H) and heading from the file's average heading.
        /// Matches the FormGPS parser behavior.
        /// </summary>
        public static List<CTrk> LoadTracks(string fieldDirectory)
        {
            var result = new List<CTrk>();
            var path = Path.Combine(fieldDirectory ?? "", "TrackLines.txt");
            if (!File.Exists(path)) return result;

            var lines = File.ReadAllLines(path);
            int idx = 0;

            // Skip "$TrackLines"
            if (idx < lines.Length && lines[idx].Trim().StartsWith("$", StringComparison.Ordinal)) idx++;

            while (idx < lines.Length)
            {
                // Name
                if (idx >= lines.Length) break;
                var name = lines[idx++].Trim();
                if (string.IsNullOrEmpty(name)) { SkipBlanks(lines, ref idx); continue; }

                // Average heading (radians)
                if (idx >= lines.Length) break;
                if (!double.TryParse(lines[idx++].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out var avgHeading))
                    avgHeading = 0.0;

                // Point A (E,N)
                if (idx >= lines.Length) break;
                if (!TryParseEN(lines[idx++], out var aE, out var aN)) { SkipBlanks(lines, ref idx); continue; }

                // Point B (E,N)
                if (idx >= lines.Length) break;
                if (!TryParseEN(lines[idx++], out var bE, out var bN)) { SkipBlanks(lines, ref idx); continue; }

                // Nudge (ignored)
                if (idx < lines.Length) idx++;

                // Mode
                var mode = 0;
                if (idx < lines.Length)
                    int.TryParse(lines[idx++].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out mode);

                // Visible (ignored)
                if (idx < lines.Length) idx++;

                // Curve count
                var curveCount = 0;
                if (idx < lines.Length)
                    int.TryParse(lines[idx++].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out curveCount);

                // Curve points (E,N,H)
                var curvePts = new List<vec3>();
                for (int i = 0; i < curveCount && idx < lines.Length; i++, idx++)
                {
                    var parts = lines[idx].Split(',');
                    if (parts.Length >= 3 &&
                        double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out var e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out var n) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var h))
                    {
                        curvePts.Add(new vec3(e, n, h));
                    }
                }

                if (mode == (int)TrackMode.AB)
                {
                    // Heading from A->B (E,N)
                    var abHeading = Math.Atan2(bE - aE, bN - aN);

                    result.Add(new CTrk
                    {
                        name = name,
                        mode = TrackMode.AB,
                        ptA = new vec2(aE, aN),
                        ptB = new vec2(bE, bN),
                        heading = abHeading
                    });
                }
                else if (mode == (int)TrackMode.Curve && curvePts.Count > 1)
                {
                    result.Add(new CTrk
                    {
                        name = name,
                        mode = TrackMode.Curve,
                        curvePts = curvePts,
                        heading = avgHeading
                    });
                }

                SkipBlanks(lines, ref idx);
            }

            return result;
        }

        /// <summary>
        /// Reads flags (if present). File format matches FormGPS implementation.
        /// </summary>
        public static List<CFlag> LoadFlags(string fieldDirectory)
        {
            var result = new List<CFlag>();
            var path = Path.Combine(fieldDirectory ?? "", "Flags.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                try
                {
                    // Header
                    reader.ReadLine();
                    // Count
                    var line = reader.ReadLine();
                    if (!int.TryParse(line, out var count)) return result;

                    for (int i = 0; i < count; i++)
                    {
                        var words = (reader.ReadLine() ?? "").Split(',');
                        if (words.Length < 6) continue;

                        double lat = double.Parse(words[0], CultureInfo.InvariantCulture);
                        double lon = double.Parse(words[1], CultureInfo.InvariantCulture);
                        double east = double.Parse(words[2], CultureInfo.InvariantCulture);
                        double north = double.Parse(words[3], CultureInfo.InvariantCulture);
                        double head = (words.Length >= 8) ? double.Parse(words[4], CultureInfo.InvariantCulture) : 0;
                        int color = int.Parse(words[words.Length >= 8 ? 5 : 4], CultureInfo.InvariantCulture);
                        int id = int.Parse(words[words.Length >= 8 ? 6 : 5], CultureInfo.InvariantCulture);
                        string notes = (words.Length >= 8 ? words[7] : "").Trim();

                        result.Add(new CFlag(lat, lon, east, north, head, color, id, notes));
                    }
                }
                catch
                {
                    // Keep it silent (match Form behavior); return partial/empty list.
                }
            }

            return result;
        }

        // ----------------------------- Helpers -----------------------------

        private static bool TryParseEN(string line, out double e, out double n)
        {
            e = n = 0;
            if (string.IsNullOrWhiteSpace(line)) return false;
            var parts = line.Split(',');
            if (parts.Length < 2) return false;

            return double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out e)
                && double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out n);
        }

        private static void SkipBlanks(string[] lines, ref int idx)
        {
            while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
        }
    }
}

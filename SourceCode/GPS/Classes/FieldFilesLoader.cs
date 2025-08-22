// All comment lines are in English
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
        // ----------------------------- Small DTOs for extended loaders -----------------------------

        /// <summary>
        /// Parsed result for Sections.txt: raw triangle-strip patches and total worked area (m²).
        /// </summary>
        public sealed class SectionsData
        {
            // Holds patches, each patch is a list of vec3 (E, N, H)
            public List<List<vec3>> Patches { get; } = new List<List<vec3>>();

            // Sum of patch areas (m²) using fan triangulation like legacy code
            public double TotalArea { get; set; }
        }

        /// <summary>
        /// Parsed result for Tram.txt: boundary tram bands and inner line sets.
        /// </summary>
        public sealed class TramData
        {
            public List<vec2> Outer { get; } = new List<vec2>();
            public List<vec2> Inner { get; } = new List<vec2>();
            public List<List<vec2>> Lines { get; } = new List<List<vec2>>();
        }

        /// <summary>
        /// Parsed result for BackPic.txt: georeferenced background image settings.
        /// Image loading remains in UI; we only return flags/paths/values.
        /// </summary>
        public sealed class BackPicInfo
        {
            public bool IsGeoMap { get; set; }
            public double EastingMax { get; set; }
            public double EastingMin { get; set; }
            public double NorthingMax { get; set; }
            public double NorthingMin { get; set; }

            // May be null if BackPic.png is not found
            public string ImagePathPng { get; set; }
        }

        // ----------------------------- Field.txt -> LocalPlane -----------------------------

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
                    double lat, lon;
                    if (parts.Length >= 2
                        && double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out lat)
                        && double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out lon))
                    {
                        return new LocalPlane(new Wgs84(lat, lon), new SharedFieldProperties());
                    }
                }
            }

            return new LocalPlane(fallbackOrigin, new SharedFieldProperties());
        }

        // ----------------------------- Boundary.txt -> CBoundaryList -----------------------------

        /// <summary>
        /// Reads boundaries from Boundary.txt into a list of CBoundaryList:
        /// - fenceLine (vec3 E,N,H from file),
        /// - fenceLineEar (decimated vec2 like legacy),
        /// - area via CBoundaryList.CalculateFenceArea(ringIndex).
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
                if (idx >= lines.Length) break;
                var line = (lines[idx++] ?? string.Empty).Trim();

                var b = new CBoundaryList();

                bool isFlag;
                if (bool.TryParse(line, out isFlag))
                {
                    // Newer format: boolean flag first, then point count
                    b.isDriveThru = isFlag;
                    if (idx >= lines.Length) break;
                    line = (lines[idx++] ?? string.Empty).Trim();
                }

                int count;
                if (!int.TryParse(line, NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
                    break;

                // Read E,N,H points as vec3
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

                // Match legacy behavior: compute area and decimate fenceLineEar
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

                // Skip blank separators
                while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
            }

            return result;
        }

        /// <summary>
        /// Reads Headlines.txt into a list of CHeadPath (name, moveDistance, mode, a_point, trackPts).
        /// Header "$HeadLines" is optional. Returns empty list if file missing/corrupt.
        /// </summary>
        public static List<CHeadPath> LoadHeadLines(string fieldDirectory)
        {
            var result = new List<CHeadPath>();
            var path = Path.Combine(fieldDirectory ?? "", "Headlines.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                try
                {
                    // Optional header
                    string line = reader.ReadLine();

                    while (!reader.EndOfStream)
                    {
                        var hp = new CHeadPath();
                        hp.name = reader.ReadLine() ?? string.Empty;

                        line = reader.ReadLine(); if (line == null) break;
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

                            double e, n, h;
                            if (double.TryParse(words[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                                double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n) &&
                                double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out h))
                            {
                                hp.trackPts.Add(new vec3(e, n, h));
                            }
                        }

                        // Keep legacy behavior: discard entries with too few points
                        if (hp.trackPts.Count > 3) result.Add(hp);
                    }
                }
                catch
                {
                    // Soft-fail: return empty/partial list on error
                    result.Clear();
                }
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
                int count;
                if (!int.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out count))
                    break;

                var hd = boundaries[k].hdLine;
                if (hd != null) hd.Clear();

                for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                {
                    var parts = (lines[idx] ?? string.Empty).Split(',');
                    if (parts.Length < 3) continue;

                    double e, n, h;
                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n) &&
                        double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out h))
                    {
                        boundaries[k].hdLine.Add(new vec3(e, n, h));
                    }
                }
            }
        }

        // ----------------------------- TrackLines.txt -> CTrk -----------------------------

        /// <summary>
        /// Reads AB and Curve tracks from TrackLines.txt into a List&lt;CTrk&gt;.
        /// - For AB: ptA/ptB as vec2 (E,N) and heading computed from A->B.
        /// - For Curve: curvePts vec3(E,N,H) and heading from the file's average heading.
        /// Matches legacy parser behavior.
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
                var name = (lines[idx++] ?? string.Empty).Trim();
                if (string.IsNullOrEmpty(name)) { SkipBlanks(lines, ref idx); continue; }

                // Average heading (radians)
                if (idx >= lines.Length) break;
                double avgHeading;
                if (!double.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out avgHeading))
                    avgHeading = 0.0;

                // Point A (E,N)
                if (idx >= lines.Length) break;
                double aE, aN;
                if (!TryParseEN(lines[idx++], out aE, out aN)) { SkipBlanks(lines, ref idx); continue; }

                // Point B (E,N)
                if (idx >= lines.Length) break;
                double bE, bN;
                if (!TryParseEN(lines[idx++], out bE, out bN)) { SkipBlanks(lines, ref idx); continue; }

                // Nudge (ignored)
                if (idx < lines.Length) idx++;

                // Mode
                var mode = 0;
                if (idx < lines.Length)
                    int.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out mode);

                // Visible (ignored)
                if (idx < lines.Length) idx++;

                // Curve count
                var curveCount = 0;
                if (idx < lines.Length)
                    int.TryParse((lines[idx++] ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out curveCount);

                // Curve points (E,N,H)
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
                    // Heading from A->B (E,N) (atan2(dx, dy) with dx=easting diff, dy=northing diff)
                    var abHeading = Math.Atan2(bE - aE, bN - aN);

                    var tr = new CTrk();
                    tr.name = name;
                    tr.mode = TrackMode.AB;
                    tr.ptA = new vec2(aE, aN);
                    tr.ptB = new vec2(bE, bN);
                    tr.heading = abHeading;
                    result.Add(tr);
                }
                else if (mode == (int)TrackMode.Curve && curvePts.Count > 1)
                {
                    var tr = new CTrk();
                    tr.name = name;
                    tr.mode = TrackMode.Curve;
                    tr.curvePts = curvePts;
                    tr.heading = avgHeading;
                    result.Add(tr);
                }

                SkipBlanks(lines, ref idx);
            }

            return result;
        }

        // ----------------------------- Flags.txt -> CFlag -----------------------------

        /// <summary>
        /// Reads flags (if present). File format matches legacy FormGPS implementation.
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
                    int count;
                    if (!int.TryParse(line, out count)) return result;

                    for (int i = 0; i < count; i++)
                    {
                        var words = (reader.ReadLine() ?? string.Empty).Split(',');
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
                    // Keep it silent (legacy behavior); return partial/empty list.
                }
            }

            return result;
        }

        // ----------------------------- Sections.txt -> SectionsData -----------------------------

        /// <summary>
        /// Reads Sections.txt patches (triangle strips) and computes total area (m²).
        /// Each block: Count + Count x "E,N,H" (3 decimals). Header lines are ignored.
        /// </summary>
        public static SectionsData LoadSections(string fieldDirectory)
        {
            var result = new SectionsData();
            var path = Path.Combine(fieldDirectory ?? "", "Sections.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int verts;
                    if (!int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out verts))
                    {
                        // Not a numeric count (may be "$Sectionsv4" or similar) -> skip
                        continue;
                    }

                    var patch = ReadVec3Block(reader, verts);
                    if (patch.Count > 0)
                    {
                        result.Patches.Add(patch);

                        // Compute area like legacy (fan triangulation on the strip)
                        if (patch.Count >= 3)
                        {
                            int vertsForArea = patch.Count - 2;
                            for (int j = 1; j < vertsForArea; j++)
                            {
                                var a = patch[j];
                                var b = patch[j + 1];
                                var c = patch[j + 2];

                                double twiceArea =
                                    a.easting * (b.northing - c.northing) +
                                    b.easting * (c.northing - a.northing) +
                                    c.easting * (a.northing - b.northing);

                                result.TotalArea += Math.Abs(0.5 * twiceArea);
                            }
                        }
                    }
                }
            }

            return result;
        }

        // ----------------------------- Contour.txt -> patches -----------------------------

        /// <summary>
        /// Reads Contour.txt triangle-strip patches. Header "$Contour" is optional.
        /// Each block: Count + Count x "E,N,H" (3 decimals).
        /// </summary>
        public static List<List<vec3>> LoadContour(string fieldDirectory)
        {
            var result = new List<List<vec3>>();
            var path = Path.Combine(fieldDirectory ?? "", "Contour.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                // Try to read first line; it may be a header or the first count
                string first = reader.ReadLine();
                if (!string.IsNullOrEmpty(first) && !first.TrimStart().StartsWith("$", StringComparison.Ordinal))
                {
                    int verts0;
                    if (int.TryParse(first.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out verts0))
                    {
                        var patch0 = ReadVec3Block(reader, verts0);
                        if (patch0.Count > 0) result.Add(patch0);
                    }
                    // If not a number, just ignore and continue
                }

                // Subsequent blocks
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int verts;
                    if (!int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out verts))
                        continue;

                    var patch = ReadVec3Block(reader, verts);
                    if (patch.Count > 0) result.Add(patch);
                }
            }

            return result;
        }

        // ----------------------------- Tram.txt -> TramData -----------------------------

        /// <summary>
        /// Reads Tram.txt: outer/inner tram boundaries and optional line set.
        /// </summary>
        public static TramData LoadTram(string fieldDirectory)
        {
            var data = new TramData();
            var path = Path.Combine(fieldDirectory ?? "", "Tram.txt");
            if (!File.Exists(path)) return data;

            using (var reader = new StreamReader(path))
            {
                // Read first line and decide if it's a header or a number
                string first = reader.ReadLine();

                // If it's a header, read the next line as the first count
                if (!string.IsNullOrEmpty(first) && first.TrimStart().StartsWith("$", StringComparison.Ordinal))
                {
                    first = reader.ReadLine();
                }

                // ---- Outer ----
                int outerCount = ParseIntSafe(first);
                for (int i = 0; i < outerCount && !reader.EndOfStream; i++)
                {
                    var parts = (reader.ReadLine() ?? string.Empty).Split(',');
                    if (parts.Length < 2) continue;

                    double e, n;
                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n))
                    {
                        data.Outer.Add(new vec2(e, n));
                    }
                }

                // ---- Inner ----
                int innerCount = ParseIntSafe(reader.ReadLine());
                for (int i = 0; i < innerCount && !reader.EndOfStream; i++)
                {
                    var parts = (reader.ReadLine() ?? string.Empty).Split(',');
                    if (parts.Length < 2) continue;

                    double e, n;
                    if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                        double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n))
                    {
                        data.Inner.Add(new vec2(e, n));
                    }
                }

                // ---- Optional lines ----
                string lineCountStr = reader.EndOfStream ? null : reader.ReadLine();
                int lineCount = ParseIntSafe(lineCountStr);
                for (int k = 0; k < lineCount && !reader.EndOfStream; k++)
                {
                    int pts = ParseIntSafe(reader.ReadLine());
                    var ln = new List<vec2>();
                    for (int i = 0; i < pts && !reader.EndOfStream; i++)
                    {
                        var parts = (reader.ReadLine() ?? string.Empty).Split(',');
                        if (parts.Length < 2) continue;

                        double e, n;
                        if (double.TryParse(parts[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                            double.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n))
                        {
                            ln.Add(new vec2(e, n));
                        }
                    }
                    data.Lines.Add(ln);
                }
            }

            return data;
        }

        // ----------------------------- RecPath.txt -> List<CRecPathPt> -----------------------------

        /// <summary>
        /// Reads RecPath.Txt (or custom name) into a list of recorded points.
        /// Format: "$RecPath" + Count + Count x "E,N,H,Speed,AutoBtnState".
        /// </summary>
        public static List<CRecPathPt> LoadRecPath(string fieldDirectory, string fileName = "RecPath.txt")
        {
            var list = new List<CRecPathPt>();
            var path = Path.Combine(fieldDirectory ?? "", fileName);
            if (!File.Exists(path)) return list;

            using (var reader = new StreamReader(path))
            {
                try
                {
                    // First two lines: header and count (legacy)
                    string headerOrCount = reader.ReadLine(); // "$RecPath" or first number
                    string cntLine = reader.ReadLine();
                    int numPoints;

                    // If file only has a count on first line (older variants), handle that
                    if (cntLine == null && headerOrCount != null && int.TryParse(headerOrCount.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out numPoints))
                    {
                        // numPoints already set
                    }
                    else
                    {
                        if (cntLine == null || !int.TryParse(cntLine.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out numPoints))
                            return list;
                    }

                    for (int i = 0; i < numPoints && !reader.EndOfStream; i++)
                    {
                        var words = (reader.ReadLine() ?? string.Empty).Split(',');
                        if (words.Length < 5) continue;

                        var pt = new CRecPathPt(
                            double.Parse(words[0], CultureInfo.InvariantCulture),
                            double.Parse(words[1], CultureInfo.InvariantCulture),
                            double.Parse(words[2], CultureInfo.InvariantCulture),
                            double.Parse(words[3], CultureInfo.InvariantCulture),
                            bool.Parse(words[4]));
                        list.Add(pt);
                    }
                }
                catch
                {
                    // Return partial/empty list on error (soft fail)
                }
            }

            return list;
        }

        // ----------------------------- BackPic.txt -> BackPicInfo -----------------------------

        /// <summary>
        /// Reads BackPic.txt georeference settings. Returns IsGeoMap + bounds + PNG path if present.
        /// </summary>
        public static BackPicInfo LoadBackPicSettings(string fieldDirectory)
        {
            var info = new BackPicInfo();
            var txt = Path.Combine(fieldDirectory ?? "", "BackPic.txt");
            var png = Path.Combine(fieldDirectory ?? "", "BackPic.png");

            if (!File.Exists(txt)) return info;

            using (var reader = new StreamReader(txt))
            {
                try
                {
                    // Optional header
                    reader.ReadLine();

                    // isGeoMap
                    string line = reader.ReadLine();
                    bool v;
                    info.IsGeoMap = bool.TryParse(line == null ? string.Empty : line.Trim(), out v) && v;

                    // Bounds
                    info.EastingMax = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.EastingMin = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.NorthingMax = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.NorthingMin = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);

                    // Image path
                    info.ImagePathPng = File.Exists(png) ? png : null;
                }
                catch
                {
                    // On any error, return default (IsGeoMap=false, no image)
                    info = new BackPicInfo();
                }
            }

            return info;
        }

        // ----------------------------- Helpers -----------------------------

        // Parse one "E,N" line
        private static bool TryParseEN(string line, out double e, out double n)
        {
            e = 0; n = 0;
            if (string.IsNullOrWhiteSpace(line)) return false;
            var parts = line.Split(',');
            if (parts.Length < 2) return false;

            return double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out e)
                && double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out n);
        }

        // Skip empty/whitespace lines
        private static void SkipBlanks(string[] lines, ref int idx)
        {
            while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
        }

        // Read N lines of "E,N,H" into a vec3 list
        private static List<vec3> ReadVec3Block(StreamReader r, int count)
        {
            var list = new List<vec3>(count > 0 ? count : 0);
            for (int i = 0; i < count && !r.EndOfStream; i++)
            {
                var words = (r.ReadLine() ?? string.Empty).Split(',');
                if (words.Length < 3) continue;

                double e, n, h;
                if (double.TryParse(words[0], NumberStyles.Float, CultureInfo.InvariantCulture, out e) &&
                    double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out n) &&
                    double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out h))
                {
                    list.Add(new vec3(e, n, h));
                }
            }
            return list;
        }

        // Safe int parse helper for lines
        private static int ParseIntSafe(string line)
        {
            int v;
            if (!string.IsNullOrWhiteSpace(line) &&
                int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
            {
                return v;
            }
            return 0;
        }
    }
}

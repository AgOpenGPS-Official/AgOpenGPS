// File: FieldBatchExportLogic.cs
// All comments in English (C# 7.3 compatible)
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Protocols.ISOBUS;

namespace AgOpenGPS.Classes.Exporters
{
    /// <summary>
    /// Logic helper for the "Export Multiple Fields" workflow.
    /// Keeps UI clean: loads names, moves selections, builds export data, and invokes the exporter.
    /// </summary>
    public sealed class FieldBatchExportLogic
    {
        // Filenames used inside a field directory
        private const string FieldTxt = "Field.txt";
        private const string BoundaryTxt = "Boundary.txt";
        private const string HeadlandTxt = "Headland.txt";
        private const string TrackLinesTxt = "TrackLines.txt";

        // Always provide a parameterless constructor (no DI)
        public FieldBatchExportLogic() { }

        /// <summary>
        /// Returns the root folder containing all field subfolders.
        /// </summary>
        public string GetFieldsRoot()
        {
            // Use global/static setting as agreed
            return RegistrySettings.fieldsDirectory;
        }

        /// <summary>
        /// Scans the fields root and returns all field folder names (single level).
        /// </summary>
        public List<string> LoadAvailableFieldNames()
        {
            var root = GetFieldsRoot();
            var result = new List<string>();

            if (!string.IsNullOrWhiteSpace(root) && Directory.Exists(root))
            {
                foreach (var dir in Directory.EnumerateDirectories(root))
                {
                    var name = Path.GetFileName(dir);
                    if (!string.IsNullOrWhiteSpace(name))
                        result.Add(name);
                }
            }

            result.Sort(StringComparer.OrdinalIgnoreCase);
            return result;
        }

        /// <summary>
        /// Moves all items from source to target (no duplicates).
        /// </summary>
        public void MoveAll(IList<string> source, IList<string> target)
        {
            if (source == null || target == null) return;

            foreach (var it in source)
                if (!target.Contains(it)) target.Add(it);

            source.Clear();
        }

        /// <summary>
        /// Moves selected items from source to target (no duplicates).
        /// </summary>
        public void MoveSelected(IList<string> source, IList<string> target, IEnumerable<string> selectedItems)
        {
            if (source == null || target == null || selectedItems == null) return;

            var toMove = selectedItems.Distinct().ToList();

            foreach (var it in toMove)
                if (source.Contains(it) && !target.Contains(it))
                    target.Add(it);

            foreach (var it in toMove)
                source.Remove(it);
        }

        /// <summary>
        /// Builds FieldExportData for the selected field names by reading each field folder.
        /// </summary>
        public List<FieldExportData> BuildExportData(IEnumerable<string> selectedFieldNames)
        {
            var root = GetFieldsRoot();
            var list = new List<FieldExportData>();

            foreach (var name in selectedFieldNames ?? Enumerable.Empty<string>())
            {
                var fieldDir = Path.Combine(root, name);
                if (!Directory.Exists(fieldDir)) continue;

                var data = FieldDataLoader.LoadForField(fieldDir, name);
                if (data != null) list.Add(data);
            }

            return list;
        }

        /// <summary>
        /// Exports multiple fields into a single ISOXML TaskData directory.
        /// </summary>
        public void ExportIsoXmlBatch(IEnumerable<FieldExportData> fields, string outputDirectory, ISO11783_TaskFile.Version version)
        {
            if (fields == null || !fields.Any())
                throw new InvalidOperationException("No fields to export.");

            if (string.IsNullOrWhiteSpace(outputDirectory))
                throw new ArgumentNullException(nameof(outputDirectory));

            Directory.CreateDirectory(outputDirectory);

            // ISO11783_TaskFile.ExportMultiple must accept IEnumerable<AgOpenGPS.Classes.Exporters.FieldExportData>
            ISO11783_TaskFile.ExportMultiple(outputDirectory, version, fields);
        }

        /// <summary>
        /// Returns whether AgShare export should be enabled (adjust to your real condition).
        /// </summary>
        public static bool IsAgShareEnabled()
        {
            // Keep original behavior (reads a boolean flag from settings)
            return Properties.Settings.Default.AgShareEnabled;
        }

        // ----------------------------- Loader -----------------------------

        /// <summary>
        /// Loader that gathers the per-field models needed by the exporter.
        /// Reads Field.txt, Boundary.txt, Headland.txt, TrackLines.txt.
        /// </summary>
        internal static class FieldDataLoader
        {
            /// <summary>
            /// Loads all data needed for exporting a single field.
            /// </summary>
            public static FieldExportData LoadForField(string fieldDirectory, string designator)
            {
                if (!Directory.Exists(fieldDirectory)) return null;

                // 1) Local plane (Field.txt)
                var localPlane = TryLoadLocalPlane(Path.Combine(fieldDirectory, FieldTxt))
                                 ?? new LocalPlane(new Wgs84(52.0, 5.0), new SharedFieldProperties()); // safe fallback

                // 2) Boundaries as Geo -> map to CBoundaryList
                var geoRings = TryLoadBoundaryRings(Path.Combine(fieldDirectory, BoundaryTxt)) ?? new List<GeoPolygon>();
                var boundaries = MapGeoToBoundaryLists(geoRings);

                // 3) Headland -> attach to first boundary
                TryAttachHeadland(Path.Combine(fieldDirectory, HeadlandTxt), boundaries);

                // 4) Tracks (TrackLines.txt) -> List<CTrk>
                var tracks = TryLoadTracks(Path.Combine(fieldDirectory, TrackLinesTxt)) ?? new List<CTrk>();

                // 5) Area (outer ring area)
                var area = TryComputeAreaFromGeo(geoRings);

                return new FieldExportData
                {
                    Designator = designator,
                    Area = area,
                    BoundaryList = boundaries,
                    LocalPlane = localPlane,
                    Tracks = tracks
                };
            }

            // -------- Field.txt --------

            /// <summary>
            /// Reads origin from Field.txt: line "StartFix" and on the next line "lat,lon".
            /// </summary>
            private static LocalPlane TryLoadLocalPlane(string fieldTxtPath)
            {
                if (!File.Exists(fieldTxtPath)) return null;

                var lines = File.ReadAllLines(fieldTxtPath);
                for (int i = 0; i < lines.Length - 1; i++)
                {
                    if (lines[i].Trim().StartsWith("StartFix", StringComparison.OrdinalIgnoreCase))
                    {
                        var parts = lines[i + 1].Trim().Split(',');
                        if (parts.Length >= 2 &&
                            TryParseDouble(parts[0], out var lat) &&
                            TryParseDouble(parts[1], out var lon))
                        {
                            return new LocalPlane(new Wgs84(lat, lon), new SharedFieldProperties());
                        }
                    }
                }
                return null;
            }

            // -------- Boundary.txt --------

            /// <summary>
            /// Parses Boundary.txt into a list of GeoPolygon rings.
            /// Format:
            ///   $Boundary
            ///   [IsHole: True/False]  // optional, currently ignored (no functional change)
            ///   Count
            ///   E,N,H (repeated Count lines; H optional)
            /// </summary>
            private static List<GeoPolygon> TryLoadBoundaryRings(string boundaryPath)
            {
                if (!File.Exists(boundaryPath)) return null;

                var rings = new List<GeoPolygon>();
                var lines = File.ReadAllLines(boundaryPath);
                var idx = 0;

                SkipHeader(lines, ref idx);

                while (idx < lines.Length)
                {
                    // Optional hole flag (ignored by design to keep functional output identical)
                    var trimmed = lines[idx].Trim();
                    bool dummy;
                    if (bool.TryParse(trimmed, out dummy)) idx++;

                    if (idx >= lines.Length) break;

                    // Count
                    int count;
                    if (!TryParseInt(lines[idx++], out count)) break;

                    var ring = new GeoPolygon();

                    for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                    {
                        var line = lines[idx].Trim();
                        if (string.IsNullOrWhiteSpace(line)) continue;

                        // Expecting E,N[,H]
                        double e, n;
                        if (TryParseEN(line, out e, out n))
                        {
                            // GeoCoord constructor is (Northing, Easting)
                            ring.Add(new GeoCoord(n, e));
                        }
                    }

                    if (ring.Count > 2) rings.Add(ring);

                    SkipBlankLines(lines, ref idx);
                }

                return rings;
            }

            /// <summary>
            /// Maps GeoPolygon rings to CBoundaryList (fenceLineEar vec2(E,N)); headlands are attached later.
            /// </summary>
            private static List<CBoundaryList> MapGeoToBoundaryLists(List<GeoPolygon> rings)
            {
                var list = new List<CBoundaryList>();
                if (rings == null || rings.Count == 0) return list;

                foreach (var ring in rings)
                {
                    var b = new CBoundaryList
                    {
                        fenceLineEar = new List<vec2>(),
                        hdLine = new List<vec3>()
                    };

                    for (int i = 0; i < ring.Count; i++)
                    {
                        var gc = ring[i]; // GeoCoord(N,E)
                        b.fenceLineEar.Add(new vec2(gc.Easting, gc.Northing)); // vec2(E,N)
                    }

                    list.Add(b);
                }
                return list;
            }

            /// <summary>
            /// Attaches headland from Headland.txt to the first boundary (hdLine vec3 E,N,H).
            /// </summary>
            private static void TryAttachHeadland(string headlandPath, List<CBoundaryList> boundaries)
            {
                if (boundaries == null || boundaries.Count == 0) return;
                if (!File.Exists(headlandPath)) return;

                var lines = File.ReadAllLines(headlandPath);
                var idx = 0;

                SkipHeader(lines, ref idx);
                if (idx >= lines.Length) return;

                int count;
                if (!TryParseInt(lines[idx++], out count)) return;

                var hd = new List<vec3>();
                for (int i = 0; i < count && idx < lines.Length; i++, idx++)
                {
                    var line = lines[idx].Trim();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    // Expecting E,N,H
                    double e, n, h;
                    if (TryParseENH(line, out e, out n, out h))
                        hd.Add(new vec3(e, n, h));
                }

                if (hd.Count > 0)
                    boundaries[0].hdLine = hd;
            }

            // -------- TrackLines.txt --------

            /// <summary>
            /// Parses TrackLines.txt blocks (AOG format):
            ///   $TrackLines
            ///   Name
            ///   AverageHeadingRadians
            ///   A_E,A_N
            ///   B_E,B_N
            ///   NudgeMeters (ignored)
            ///   ModeInt (4=Curve)
            ///   Visible True/False (ignored)
            ///   CurveCount
            ///   E,N,H * CurveCount
            /// Emits:
            ///   - AB (A->B) with heading derived from A->B
            ///   - Curve with curvePts (vec3 E,N,H) and heading from AverageHeading (if mode==Curve and points>1)
            /// </summary>
            private static List<CTrk> TryLoadTracks(string trackPath)
            {
                if (!File.Exists(trackPath)) return null;

                var lines = File.ReadAllLines(trackPath);
                var tracks = new List<CTrk>();
                var idx = 0;

                SkipHeader(lines, ref idx);

                // Parse until end of file
                while (idx < lines.Length)
                {
                    // Name
                    var name = lines[idx++].Trim();
                    if (string.IsNullOrEmpty(name)) { SkipBlankLines(lines, ref idx); continue; }

                    // Average heading (radians)
                    if (idx >= lines.Length) break;
                    double avgHeading;
                    if (!TryParseDouble(lines[idx++], out avgHeading)) avgHeading = 0.0;

                    // A (E,N)
                    if (idx >= lines.Length) break;
                    double aE, aN;
                    if (!TryParseEN(lines[idx++].Trim(), out aE, out aN)) { SkipBlankLines(lines, ref idx); continue; }

                    // B (E,N)
                    if (idx >= lines.Length) break;
                    double bE, bN;
                    if (!TryParseEN(lines[idx++].Trim(), out bE, out bN)) { SkipBlankLines(lines, ref idx); continue; }

                    // Nudge (ignored)
                    if (idx < lines.Length) idx++;

                    // Mode
                    var mode = 0;
                    if (idx < lines.Length) TryParseInt(lines[idx++], out mode);

                    // Visible (ignored)
                    if (idx < lines.Length) idx++;

                    // CurveCount
                    var curveCount = 0;
                    if (idx < lines.Length) TryParseInt(lines[idx++], out curveCount);

                    // Curve points (E,N,H)
                    var curvePts = new List<vec3>();
                    for (int i = 0; i < curveCount && idx < lines.Length; i++, idx++)
                    {
                        var ln = lines[idx].Trim();
                        double e, n, h;
                        if (TryParseENH(ln, out e, out n, out h))
                            curvePts.Add(new vec3(e, n, h));
                    }

                    // AB
                    if (mode == (int)TrackMode.AB)
                    {
                        var aCoord = new GeoCoord(aN, aE); // GeoCoord(N,E)
                        var bCoord = new GeoCoord(bN, bE);
                        var abHeading = new GeoDir(new GeoDelta(aCoord, bCoord)).AngleInRadians;

                        tracks.Add(new CTrk
                        {
                            name = name,
                            mode = TrackMode.AB,
                            ptA = new vec2(aE, aN),  // vec2(E,N)
                            ptB = new vec2(bE, bN),
                            heading = abHeading
                        });
                    }
                    // Curve
                    else if (mode == (int)TrackMode.Curve && curvePts.Count > 1)
                    {
                        tracks.Add(new CTrk
                        {
                            name = name,
                            mode = TrackMode.Curve,
                            curvePts = curvePts,
                            heading = avgHeading
                        });
                    }

                    // Skip blank lines between blocks
                    SkipBlankLines(lines, ref idx);
                }

                return tracks;
            }

            // -------- Area --------

            /// <summary>
            /// Uses first GeoPolygon (outer) area in m²; falls back to 0 if missing.
            /// </summary>
            private static int TryComputeAreaFromGeo(List<GeoPolygon> rings)
            {
                if (rings == null || rings.Count == 0) return 0;
                var area = rings[0].Area;
                return (int)Math.Round(area, MidpointRounding.AwayFromZero);
            }

            // -------- Small parsing helpers (stateless) --------

            /// <summary>
            /// Skips lines starting with '$' (e.g., headers).
            /// </summary>
            private static void SkipHeader(string[] lines, ref int idx)
            {
                while (idx < lines.Length && lines[idx].Trim().StartsWith("$")) idx++;
            }

            /// <summary>
            /// Skips blank lines until a non-empty line or EOF is reached.
            /// </summary>
            private static void SkipBlankLines(string[] lines, ref int idx)
            {
                while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
            }

            /// <summary>
            /// Parses "E,N" into doubles using InvariantCulture.
            /// </summary>
            private static bool TryParseEN(string line, out double e, out double n)
            {
                e = n = 0;
                var parts = (line ?? string.Empty).Split(',');
                if (parts.Length < 2) return false;

                return TryParseDouble(parts[0], out e)
                    && TryParseDouble(parts[1], out n);
            }

            /// <summary>
            /// Parses "E,N,H" into doubles using InvariantCulture.
            /// </summary>
            private static bool TryParseENH(string line, out double e, out double n, out double h)
            {
                e = n = h = 0;
                var parts = (line ?? string.Empty).Split(',');
                if (parts.Length < 3) return false;

                return TryParseDouble(parts[0], out e)
                    && TryParseDouble(parts[1], out n)
                    && TryParseDouble(parts[2], out h);
            }

            /// <summary>
            /// Parses an int using InvariantCulture; returns false on failure.
            /// </summary>
            private static bool TryParseInt(string s, out int value)
            {
                return int.TryParse((s ?? string.Empty).Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
            }

            /// <summary>
            /// Parses a double using InvariantCulture; returns false on failure.
            /// </summary>
            private static bool TryParseDouble(string s, out double value)
            {
                return double.TryParse((s ?? string.Empty).Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out value);
            }
        }
    }
}

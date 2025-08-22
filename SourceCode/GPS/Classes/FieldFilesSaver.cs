using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes
{
    /// <summary>
    /// Stateless file writers for field assets (Tracks, Boundaries, Headland, etc.).
    /// Formats are kept identical to legacy FormGPS.FileSave* methods.
    /// </summary>
    public static class FieldFilesSaver
    {
        private static void EnsureDir(string fieldDirectory)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                throw new ArgumentNullException(nameof(fieldDirectory));
            Directory.CreateDirectory(fieldDirectory);
        }

        private static string F3(double v) => Math.Round(v, 3).ToString(CultureInfo.InvariantCulture);
        private static string F5(double v) => Math.Round(v, 5).ToString(CultureInfo.InvariantCulture);
        private static string F1(double v) => Math.Round(v, 1).ToString(CultureInfo.InvariantCulture);

        /// <summary>
        /// Writes Headlines.txt with the exact format:
        /// $HeadLines
        /// Name
        /// moveDistance
        /// mode
        /// a_point
        /// Count
        /// E,N,H (e:3, n:3, h:5) * Count
        /// </summary>
        public static void SaveHeadLines(string fieldDirectory, IReadOnlyList<CHeadPath> headPaths)
        {
            EnsureDir(fieldDirectory);
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
                        writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F5(p.heading)}");
                    }
                }
            }
        }

        /// <summary>
        /// Writes TrackLines.txt exactly like legacy FileSaveTracks:
        /// $TrackLines
        /// name
        /// heading (raw ToString)
        /// A.easting,A.northing (3)
        /// B.easting,B.northing (3)
        /// nudgeDistance (raw)
        /// mode (int)
        /// isVisible (True/False)
        /// curveCount
        /// e,n,h (e:3, n:3, h:5) * curveCount
        /// </summary>
        public static void SaveTracks(string fieldDirectory, IReadOnlyList<CTrk> tracks)
        {
            EnsureDir(fieldDirectory);
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

                    writer.WriteLine($"{F3(t.ptA.easting)},{F3(t.ptA.northing)}");
                    writer.WriteLine($"{F3(t.ptB.easting)},{F3(t.ptB.northing)}");

                    writer.WriteLine(t.nudgeDistance.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(((int)t.mode).ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(t.isVisible.ToString()); // legacy uses True/False

                    var pts = t.curvePts ?? new List<vec3>();
                    writer.WriteLine(pts.Count.ToString(CultureInfo.InvariantCulture));
                    for (int j = 0; j < pts.Count; j++)
                    {
                        var p = pts[j];
                        writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F5(p.heading)}");
                    }
                }
            }
        }

        /// <summary>
        /// Writes Boundary.txt exactly like legacy FileSaveBoundary:
        /// $Boundary
        /// For each boundary:
        /// isDriveThru
        /// Count
        /// e,n,h (e:3, n:3, h:5) * Count
        /// </summary>
        public static void SaveBoundary(string fieldDirectory, IReadOnlyList<CBoundaryList> boundaries)
        {
            EnsureDir(fieldDirectory);
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
                        writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F5(p.heading)}");
                    }
                }
            }
        }

        /// <summary>
        /// Writes Headland.txt like legacy FileSaveHeadland:
        /// $Headland
        /// Only if at least the first boundary has hdLine > 0, emit per-boundary:
        /// Count
        /// e,n,h (e:3, n:3, h:3) * Count
        /// </summary>
        public static void SaveHeadland(string fieldDirectory, IReadOnlyList<CBoundaryList> boundaries)
        {
            EnsureDir(fieldDirectory);
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
                        writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F3(p.heading)}");
                    }
                }
            }
        }

        /// <summary>
        /// Writes Tram.txt like legacy FileSaveTram:
        /// $Tram
        /// OuterCount + points (E,N 3)
        /// InnerCount + points (E,N 3)
        /// [If tram lines exist]
        ///   LineCount
        ///   For each line: Count + points (E,N 3)
        /// </summary>
        public static void SaveTram(
            string fieldDirectory,
            IReadOnlyList<vec2> tramOuter,
            IReadOnlyList<vec2> tramInner,
            IReadOnlyList<IReadOnlyList<vec2>> tramLines)
        {
            EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "Tram.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Tram");

                var outer = tramOuter ?? new List<vec2>();
                var inner = tramInner ?? new List<vec2>();

                if (outer.Count > 0)
                {
                    writer.WriteLine(outer.Count.ToString(CultureInfo.InvariantCulture));
                    for (int i = 0; i < outer.Count; i++)
                        writer.WriteLine($"{F3(outer[i].easting)},{F3(outer[i].northing)}");

                    writer.WriteLine(inner.Count.ToString(CultureInfo.InvariantCulture));
                    for (int i = 0; i < inner.Count; i++)
                        writer.WriteLine($"{F3(inner[i].easting)},{F3(inner[i].northing)}");
                }
                else
                {
                    writer.WriteLine("0");
                    writer.WriteLine("0");
                }

                if (tramLines != null && tramLines.Count > 0)
                {
                    writer.WriteLine(tramLines.Count.ToString(CultureInfo.InvariantCulture));

                    for (int k = 0; k < tramLines.Count; k++)
                    {
                        var line = tramLines[k] ?? new List<vec2>();
                        writer.WriteLine(line.Count.ToString(CultureInfo.InvariantCulture));

                        for (int i = 0; i < line.Count; i++)
                            writer.WriteLine($"{F3(line[i].easting)},{F3(line[i].northing)}");
                    }
                }
            }
        }

        /// <summary>
        /// Appends section triangle strips like legacy FileSaveSections:
        /// For each patch:
        ///   Count
        ///   e,n,h (all 3 decimals) * Count
        /// </summary>
        public static void AppendSections(string fieldDirectory, IEnumerable<IReadOnlyList<vec3>> patches)
        {
            EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "Sections.txt");

            if (patches == null) return;

            using (var writer = new StreamWriter(filename, true))
            {
                foreach (var triList in patches)
                {
                    if (triList == null) continue;
                    writer.WriteLine(triList.Count.ToString(CultureInfo.InvariantCulture));

                    for (int i = 0; i < triList.Count; i++)
                    {
                        var p = triList[i];
                        writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F3(p.heading)}");
                    }
                }
            }
        }

        /// <summary>
        /// Creates/empties Sections.txt (no header)
        /// </summary>
        public static void CreateEmptySections(string fieldDirectory)
        {
            EnsureDir(fieldDirectory);
            using (var writer = new StreamWriter(Path.Combine(fieldDirectory, "Sections.txt"), false))
            {
                // Intentionally empty (legacy behavior).
            }
        }

        /// <summary>
        /// Appends contour patches like legacy FileSaveContour:
        /// For each patch:
        ///   Count
        ///   e,n,h (all 3 decimals) * Count
        /// </summary>
        public static void AppendContour(string fieldDirectory, IEnumerable<IReadOnlyList<vec3>> contourPatches)
        {
            EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "Contour.txt");

            if (contourPatches == null) return;

            using (var writer = new StreamWriter(filename, true))
            {
                foreach (var triList in contourPatches)
                {
                    if (triList == null) continue;
                    writer.WriteLine(triList.Count.ToString(CultureInfo.InvariantCulture));

                    for (int i = 0; i < triList.Count; i++)
                    {
                        var p = triList[i];
                        writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F3(p.heading)}");
                    }
                }
            }
        }

        /// <summary>
        /// Creates/overwrites Contour.txt with "$Contour" header (legacy FileCreateContour).
        /// </summary>
        public static void CreateContourFile(string fieldDirectory)
        {
            EnsureDir(fieldDirectory);
            using (var writer = new StreamWriter(Path.Combine(fieldDirectory, "Contour.txt"), false))
            {
                writer.WriteLine("$Contour");
            }
        }

        /// <summary>
        /// Writes Flags.txt exactly like legacy FileSaveFlags:
        /// $Flags
        /// Count
        /// lat,lon,e,n,heading,color,ID,notes
        /// </summary>
        public static void SaveFlags(string fieldDirectory, IReadOnlyList<CFlag> flags)
        {
            EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "Flags.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Flags");

                var list = flags ?? new List<CFlag>();
                writer.WriteLine(list.Count.ToString(CultureInfo.InvariantCulture));

                for (int i = 0; i < list.Count; i++)
                {
                    var f = list[i];
                    writer.WriteLine(
                        f.latitude.ToString(CultureInfo.InvariantCulture) + "," +
                        f.longitude.ToString(CultureInfo.InvariantCulture) + "," +
                        f.easting.ToString(CultureInfo.InvariantCulture) + "," +
                        f.northing.ToString(CultureInfo.InvariantCulture) + "," +
                        f.heading.ToString(CultureInfo.InvariantCulture) + "," +
                        f.color.ToString(CultureInfo.InvariantCulture) + "," +
                        f.ID.ToString(CultureInfo.InvariantCulture) + "," +
                        (f.notes ?? string.Empty));
                }
            }
        }

        /// <summary>
        /// Writes a recorded path file (default "RecPath.Txt") exactly like legacy FileSaveRecPath:
        /// $RecPath
        /// Count
        /// e(3),n(3),heading(3),speed(1),autoBtnState
        /// </summary>
        public static void SaveRecPath(string fieldDirectory, IReadOnlyList<CRecPathPt> recList, string fileName = "RecPath.Txt")
        {
            EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, fileName ?? "RecPath.Txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$RecPath");
                var list = recList ?? new List<CRecPathPt>();

                writer.WriteLine(list.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    writer.WriteLine($"{F3(p.easting)},{F3(p.northing)},{F3(p.heading)},{F1(p.speed)},{p.autoBtnState}");
                }
            }
        }

        // Empty file creators

        public static void CreateBoundaryFile(string fieldDirectory)
        {
            EnsureDir(fieldDirectory);
            using (var writer = new StreamWriter(Path.Combine(fieldDirectory, "Boundary.txt"), false))
                writer.WriteLine("$Boundary");
        }

        public static void CreateFlagsFile(string fieldDirectory)
        {
            EnsureDir(fieldDirectory);
            using (var writer = new StreamWriter(Path.Combine(fieldDirectory, "Flags.txt"), false))
            {
                // legacy creates header only (or empty); here we keep it empty like FormGPS did.
            }
        }
    }
}

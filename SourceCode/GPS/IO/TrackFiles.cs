using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Models.Guidance;

namespace AgOpenGPS.IO
{
    public static class TrackFiles
    {
        /// <summary>
        /// Load tracks from TrackLines.txt. Throws on malformed content or missing header.
        /// </summary>
        public static List<Track> Load(string fieldDirectory)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                throw new ArgumentNullException(nameof(fieldDirectory));

            var result = new List<Track>();
            var path = Path.Combine(fieldDirectory, "TrackLines.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                // Require header
                var header = reader.ReadLine();
                if (header == null || !header.TrimStart().StartsWith("$", StringComparison.Ordinal))
                    throw new InvalidDataException("TrackLines.txt missing $ header.");

                while (!reader.EndOfStream)
                {
                    // --- Name ---
                    var name = reader.ReadLine();
                    if (name == null) break;
                    name = name.Trim();
                    if (name.Length == 0) continue;

                    // --- Heading
                    var headingLine = reader.ReadLine();
                    if (headingLine == null) throw new InvalidDataException("Unexpected EOF after track name.");
                    var heading = double.Parse(headingLine.Trim(), CultureInfo.InvariantCulture);


                    // --- A point (easting,northing) ---
                    var aLine = reader.ReadLine();
                    if (aLine == null) throw new InvalidDataException("Unexpected EOF reading point A.");
                    var aParts = aLine.Split(',');
                    var aEasting = double.Parse(aParts[0], CultureInfo.InvariantCulture);
                    var aNorthing = double.Parse(aParts[1], CultureInfo.InvariantCulture);
                    var ptA = new vec2(aEasting, aNorthing);

                    // --- B point (easting,northing) ---
                    var bLine = reader.ReadLine();
                    if (bLine == null) throw new InvalidDataException("Unexpected EOF reading point B.");
                    var bParts = bLine.Split(',');
                    var bEasting = double.Parse(bParts[0], CultureInfo.InvariantCulture);
                    var bNorthing = double.Parse(bParts[1], CultureInfo.InvariantCulture);
                    var ptB = new vec2(bEasting, bNorthing);

                    // --- Nudge ---
                    var nudgeLine = reader.ReadLine();
                    if (nudgeLine == null) throw new InvalidDataException("Unexpected EOF reading nudge.");
                    var nudgeDistance = double.Parse(nudgeLine.Trim(), CultureInfo.InvariantCulture);

                    // --- Mode ---
                    var modeLine = reader.ReadLine();
                    if (modeLine == null) throw new InvalidDataException("Unexpected EOF reading mode.");
                    var modeInt = int.Parse(modeLine.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture);
                    var modeEnum = (TrackMode)modeInt;

                    // --- Visibility ---
                    var visLine = reader.ReadLine();
                    if (visLine == null) throw new InvalidDataException("Unexpected EOF reading visibility.");
                    var isVisible = bool.Parse(visLine.Trim());

                    // --- Curve count ---
                    var countLine = reader.ReadLine();
                    if (countLine == null) throw new InvalidDataException("Unexpected EOF reading curve count.");
                    var curveCount = int.Parse(countLine.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture);

                    // --- Curve points ---
                    var curvePts = new List<vec3>();
                    for (int i = 0; i < curveCount; i++)
                    {
                        var line = reader.ReadLine();
                        if (line == null) throw new InvalidDataException("Unexpected EOF in curve points.");
                        var parts = line.Split(',');
                        var easting = double.Parse(parts[0], CultureInfo.InvariantCulture);
                        var northing = double.Parse(parts[1], CultureInfo.InvariantCulture);
                        var pointheading = double.Parse(parts[2], CultureInfo.InvariantCulture);
                        curvePts.Add(new vec3(easting, northing, pointheading));
                    }

                    // Build Track (NEW: Core model)
                    var tr = new Track
                    {
                        Id = Guid.NewGuid(),  // Generate unique ID
                        Name = name,
                        Mode = (Core.Models.Guidance.TrackMode)modeEnum,  // Cast to correct enum
                        PtA = ptA,  // Already vec2
                        PtB = ptB,  // Already vec2
                        NudgeDistance = nudgeDistance,
                        IsVisible = isVisible,
                        Heading = heading,
                        CurvePts = curvePts
                    };

                    result.Add(tr);

                }

                return result;
            }
        }

        /// <summary>
        /// Save tracks to TrackLines.txt. Overwrites the file.
        /// </summary>
        public static void Save(string fieldDirectory, IReadOnlyList<Track> tracks)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                throw new ArgumentNullException(nameof(fieldDirectory));

            var filename = Path.Combine(fieldDirectory, "TrackLines.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$TrackLines");
                if (tracks == null || tracks.Count == 0) return;

                foreach (Track track in tracks)
                {
                    writer.WriteLine(track.Name ?? string.Empty);
                    writer.WriteLine(track.Heading.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine($"{FileIoUtils.FormatDouble(track.PtA.easting, 3)},{FileIoUtils.FormatDouble(track.PtA.northing, 3)}");
                    writer.WriteLine($"{FileIoUtils.FormatDouble(track.PtB.easting, 3)},{FileIoUtils.FormatDouble(track.PtB.northing, 3)}");
                    writer.WriteLine(track.NudgeDistance.ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(((int)track.Mode).ToString(CultureInfo.InvariantCulture));
                    writer.WriteLine(track.IsVisible.ToString());

                    var pts = track.CurvePts ?? new List<vec3>();
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

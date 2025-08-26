using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.IO
{
    public static class TramFiles
    {
        /// <summary>
        /// Plain DTO for tram data as used in UI/controllers.
        /// </summary>
        public sealed class TramData
        {
            public List<vec2> Outer { get; } = new List<vec2>();
            public List<vec2> Inner { get; } = new List<vec2>();
            public List<List<vec2>> Lines { get; } = new List<List<vec2>>();
        }

        /// <summary>
        /// Load Tram.txt from a field directory. Missing file returns an empty TramData.
        /// </summary>
        public static TramData Load(string fieldDirectory)
        {
            var data = new TramData();
            var path = Path.Combine(fieldDirectory ?? "", "Tram.txt");
            if (!File.Exists(path)) return data;

            using (var reader = new StreamReader(path))
            {
                // Optional header line
                string first = reader.ReadLine();
                if (!string.IsNullOrEmpty(first) && first.TrimStart().StartsWith("$"))
                {
                    // If there is a header, the next line should be the outer count
                    first = reader.ReadLine();
                }

                // --- Outer ring ---
                int outerCount = FileIoUtils.ParseIntSafe(first);
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

                // --- Inner ring ---
                int innerCount = FileIoUtils.ParseIntSafe(reader.ReadLine());
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

                // --- Optional lines ---
                string lineCountStr = reader.EndOfStream ? null : reader.ReadLine();
                int lineCount = FileIoUtils.ParseIntSafe(lineCountStr);
                for (int k = 0; k < lineCount && !reader.EndOfStream; k++)
                {
                    int pts = FileIoUtils.ParseIntSafe(reader.ReadLine());
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

        /// <summary>
        /// Save Tram.txt (outer, inner, and optional lines). Creates/overwrites the file.
        /// </summary>
        public static void Save(
            string fieldDirectory,
            IReadOnlyList<vec2> tramOuter,
            IReadOnlyList<vec2> tramInner,
            IReadOnlyList<IReadOnlyList<vec2>> tramLines)
        {
            FileIoUtils.EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, "Tram.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$Tram");

                var outer = tramOuter ?? new List<vec2>();
                var inner = tramInner ?? new List<vec2>();

                // Outer
                writer.WriteLine(outer.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < outer.Count; i++)
                    writer.WriteLine($"{FileIoUtils.F3(outer[i].easting)},{FileIoUtils.F3(outer[i].northing)}");

                // Inner
                writer.WriteLine(inner.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < inner.Count; i++)
                    writer.WriteLine($"{FileIoUtils.F3(inner[i].easting)},{FileIoUtils.F3(inner[i].northing)}");

                // Lines (optional)
                if (tramLines != null && tramLines.Count > 0)
                {
                    writer.WriteLine(tramLines.Count.ToString(CultureInfo.InvariantCulture));
                    for (int k = 0; k < tramLines.Count; k++)
                    {
                        var line = tramLines[k] ?? (IReadOnlyList<vec2>)new List<vec2>();
                        writer.WriteLine(line.Count.ToString(CultureInfo.InvariantCulture));
                        for (int i = 0; i < line.Count; i++)
                            writer.WriteLine($"{FileIoUtils.F3(line[i].easting)},{FileIoUtils.F3(line[i].northing)}");
                    }
                }
            }
        }
    }
}

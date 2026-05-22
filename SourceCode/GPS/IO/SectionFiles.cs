using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AgOpenGPS.IO
{
    public static class SectionsFiles
    {
        public sealed class SectionsData
        {
            public List<List<vec3>> Patches { get; } = new List<List<vec3>>();
            public double TotalArea { get; set; }
        }

        public static List<List<vec3>> Load(string fieldDirectory)
        {
            var result = new List<List<vec3>>();
            var path = Path.Combine(fieldDirectory, "Sections.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int verts;
                    if (!int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out verts))
                        continue;

                    var patch = FileIoUtils.ReadVec3Block(reader, verts);
                    if (patch.Count > 0)
                    {
                        // Ensure patch has odd vertex count (first vertex is RGB color, rest are coordinates)
                        // If even, it means the color vertex is missing, so prepend a default color
                        if (patch.Count % 2 == 0)
                        {
                            // The first line was likely a coordinate misinterpreted as color
                            // Insert default color at the beginning
                            var colorVec = new vec3(27, 151, 160); // Default teal color
                            patch.Insert(0, colorVec);
                        }
                        result.Add(patch);
                    }
                }
            }

            return result;
        }

        public static void Append(string fieldDirectory, IEnumerable<IReadOnlyList<vec3>> patches)
        {
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
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)},{FileIoUtils.FormatDouble(p.northing, 3)},{FileIoUtils.FormatDouble(p.heading, 5)}");
                    }
                }
            }
        }

        public static void CreateEmpty(string fieldDirectory)
        {
            using (var writer = new StreamWriter(Path.Combine(fieldDirectory, "Sections.txt"), false))
            {
                // Intentionally empty
            }
        }
    }
}

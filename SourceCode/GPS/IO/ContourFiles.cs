using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class ContourFiles
    {
        public static List<List<vec3>> Load(string fieldDirectory)
        {
            var result = new List<List<vec3>>();
            var path = Path.Combine(fieldDirectory, "Contour.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                string first = reader.ReadLine();
                int verts0;
                if (!string.IsNullOrEmpty(first) && !first.TrimStart().StartsWith("$") &&
                    int.TryParse(first.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out verts0))
                {
                    var patch0 = FileIoUtils.ReadVec3Block(reader, verts0);
                    if (patch0.Count > 0) result.Add(patch0);
                }

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    int verts;
                    if (!int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out verts))
                        continue;

                    var patch = FileIoUtils.ReadVec3Block(reader, verts);
                    if (patch.Count > 0) result.Add(patch);
                }
            }

            return result;
        }

        public static void Append(string fieldDirectory, IEnumerable<IReadOnlyList<vec3>> contourPatches)
        {
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
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)} , {FileIoUtils.FormatDouble(p.northing, 3)} , {FileIoUtils.FormatDouble(p.heading, 5)}");
                    }
                }
            }
        }

        public static void CreateFile(string fieldDirectory)
        {
            using (var writer = new StreamWriter(Path.Combine(fieldDirectory, "Contour.txt"), false))
            {
                writer.WriteLine("$Contour");
            }
        }
    }
}

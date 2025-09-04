using System;
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
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                throw new ArgumentNullException(nameof(fieldDirectory));

            var result = new List<List<vec3>>();
            var path = Path.Combine(fieldDirectory, "Contour.txt");
            if (!File.Exists(path)) return result;

            using (var reader = new StreamReader(path))
            {
                var header = reader.ReadLine();
                if (header == null || !header.TrimStart().StartsWith("$", StringComparison.Ordinal))
                    throw new InvalidDataException("Contour.txt missing header.");

                while (!reader.EndOfStream)
                {
                    var countLine = reader.ReadLine();
                    if (countLine == null) break;

                    countLine = countLine.Trim();
                    if (countLine.Length == 0) continue; // skip blank lines

                    var count = int.Parse(countLine, NumberStyles.Integer, CultureInfo.InvariantCulture);
                    if (count <= 0) continue;

                    var patch = FileIoUtils.ReadVec3Block(reader, count);
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
                        writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)},{FileIoUtils.FormatDouble(p.northing, 3)},{FileIoUtils.FormatDouble(p.heading, 5)}");
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

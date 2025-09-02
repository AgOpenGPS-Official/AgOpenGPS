using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class RecPathFiles
    {
        public static List<CRecPathPt> Load(string fieldDirectory)
        {
            var list = new List<CRecPathPt>();
            var path = Path.Combine(fieldDirectory, "RecPath.txt");
            if (!File.Exists(path)) return list;

            using (var reader = new StreamReader(path))
            {
                string headerOrCount = reader.ReadLine();
                string cntLine = reader.ReadLine();
                int numPoints;

                if (cntLine == null && headerOrCount != null && int.TryParse(headerOrCount.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out numPoints))
                {
                    // single-line count
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
                        double.Parse(words[0], CultureInfo.InvariantCulture), // easting
                        double.Parse(words[1], CultureInfo.InvariantCulture), // northing
                        double.Parse(words[2], CultureInfo.InvariantCulture), // heading
                        double.Parse(words[3], CultureInfo.InvariantCulture), // speed
                        bool.Parse(words[4]));
                    list.Add(pt);
                }
            }

            return list;

        }
        public static void Save(string fieldDirectory, IReadOnlyList<CRecPathPt> recList, string fileName = "RecPath.txt")
        {
            var filename = Path.Combine(fieldDirectory, fileName ?? "RecPath.txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$RecPath");
                var list = recList ?? new List<CRecPathPt>();

                writer.WriteLine(list.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    writer.WriteLine($"{FileIoUtils.FormatDouble(p.easting, 3)},{FileIoUtils.FormatDouble(p.northing, 3)},{FileIoUtils.FormatDouble(p.heading, 3)},{FileIoUtils.FormatDouble(p.speed, 1)},{p.autoBtnState}");
                }
            }
        }

        /// <summary>
        /// Create or overwrite an empty RecPath.txt with legacy-compatible header.
        /// </summary>
        public static void CreateEmpty(string fieldDirectory)
        {
            if (string.IsNullOrEmpty(fieldDirectory))
            {
                throw new ArgumentNullException(nameof(fieldDirectory));
            }

            if (!Directory.Exists(fieldDirectory))
            {
                Directory.CreateDirectory(fieldDirectory);
            }

            var path = Path.Combine(fieldDirectory, "RecPath.txt");
            using (var writer = new StreamWriter(path, false, Encoding.UTF8))
            {
                writer.WriteLine("$RecPath");
                writer.WriteLine("0");
            }
        }
    }
}

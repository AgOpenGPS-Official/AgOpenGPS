using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.Classes.IO
{
    public static class RecPathFiles
    {
        public static List<CRecPathPt> Load(string fieldDirectory, string fileName = "RecPath.txt")
        {
            var list = new List<CRecPathPt>();
            var path = Path.Combine(fieldDirectory ?? "", fileName);
            if (!File.Exists(path)) return list;

            using (var reader = new StreamReader(path))
            {
                try
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
                    // Legacy behavior: soft fail
                }
            }

            return list;
        }

        public static void Save(string fieldDirectory, IReadOnlyList<CRecPathPt> recList, string fileName = "RecPath.Txt")
        {
            FileIoUtils.EnsureDir(fieldDirectory);
            var filename = Path.Combine(fieldDirectory, fileName ?? "RecPath.Txt");

            using (var writer = new StreamWriter(filename, false))
            {
                writer.WriteLine("$RecPath");
                var list = recList ?? new List<CRecPathPt>();

                writer.WriteLine(list.Count.ToString(CultureInfo.InvariantCulture));
                for (int i = 0; i < list.Count; i++)
                {
                    var p = list[i];
                    writer.WriteLine($"{FileIoUtils.F3(p.easting)},{FileIoUtils.F3(p.northing)},{FileIoUtils.F3(p.heading)},{FileIoUtils.F1(p.speed)},{p.autoBtnState}");
                }
            }
        }
    }
}

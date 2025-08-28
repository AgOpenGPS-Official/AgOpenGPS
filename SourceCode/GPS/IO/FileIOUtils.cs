using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS.IO
{
    public static class FileIoUtils
    {
        // ---- Formatting helper ----
        public static string FormatDouble(double value, int decimals)
        {
            return Math.Round(value, decimals).ToString(CultureInfo.InvariantCulture);
        }

        // ---- Parsing helpers ----

        public static int ParseIntSafe(string line)
        {
            int v;
            if (!string.IsNullOrWhiteSpace(line) &&
                int.TryParse(line.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out v))
            {
                return v;
            }
            return 0;
        }

        public static List<vec3> ReadVec3Block(StreamReader r, int count)
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
    }
}

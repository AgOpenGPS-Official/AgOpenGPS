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

        // ---- IO helpers ----
        public static void EnsureDir(string fieldDirectory)
        {
            if (string.IsNullOrWhiteSpace(fieldDirectory))
                throw new ArgumentNullException(nameof(fieldDirectory));
            Directory.CreateDirectory(fieldDirectory);
        }

        public static void SkipBlanks(string[] lines, ref int idx)
        {
            while (idx < lines.Length && string.IsNullOrWhiteSpace(lines[idx])) idx++;
        }

        // ---- Parsing helpers ----
        public static bool TryParseEN(string line, out double e, out double n)
        {
            e = 0; n = 0;
            if (string.IsNullOrWhiteSpace(line)) return false;
            var parts = line.Split(',');
            if (parts.Length < 2) return false;

            return double.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out e)
                && double.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out n);
        }

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

using System.IO;
using System.Globalization;

namespace AgOpenGPS.Classes.IO
{
    public static class BackPicFiles
    {
        public sealed class BackPicInfo
        {
            public bool IsGeoMap { get; set; }
            public double EastingMax { get; set; }
            public double EastingMin { get; set; }
            public double NorthingMax { get; set; }
            public double NorthingMin { get; set; }
            public string ImagePathPng { get; set; }
        }

        public static BackPicInfo Load(string fieldDirectory)
        {
            var info = new BackPicInfo();
            var txt = Path.Combine(fieldDirectory ?? "", "BackPic.txt");
            var png = Path.Combine(fieldDirectory ?? "", "BackPic.png");

            if (!File.Exists(txt)) return info;

            using (var reader = new StreamReader(txt))
            {
                try
                {
                    reader.ReadLine(); // optional header

                    string line = reader.ReadLine();
                    bool v;
                    info.IsGeoMap = bool.TryParse(line == null ? string.Empty : line.Trim(), out v) && v;

                    info.EastingMax = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.EastingMin = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.NorthingMax = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.NorthingMin = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);

                    info.ImagePathPng = File.Exists(png) ? png : null;
                }
                catch
                {
                    info = new BackPicInfo();
                }
            }

            return info;
        }
    }
}

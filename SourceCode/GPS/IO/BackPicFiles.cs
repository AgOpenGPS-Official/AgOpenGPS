using System.IO;
using System.Globalization;

namespace AgOpenGPS.IO
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
            var txt = Path.Combine(fieldDirectory, "BackPic.txt");
            var png = Path.Combine(fieldDirectory, "BackPic.png");

            if (!File.Exists(txt)) return info;

            using (var reader = new StreamReader(txt))
            {
                try
                {
                    reader.ReadLine(); // header

                    string line = reader.ReadLine();
                    bool v;
                    info.IsGeoMap = bool.TryParse(line == null ? string.Empty : line.Trim(), out v) && v;

                    info.EastingMax = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.EastingMin = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.NorthingMax = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);
                    info.NorthingMin = double.Parse(reader.ReadLine() ?? "0", CultureInfo.InvariantCulture);

                    // Legacy parity: if PNG is missing, turn IsGeoMap off.
                    if (File.Exists(png))
                    {
                        info.ImagePathPng = png;
                    }
                    else
                    {
                        info.ImagePathPng = null;
                        info.IsGeoMap = false;
                    }
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
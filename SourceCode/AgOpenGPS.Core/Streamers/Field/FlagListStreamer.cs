using AgOpenGPS.Core.Interfaces;
using AgOpenGPS.Core.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class FlagListStreamer : FieldAspectStreamer
    {
        public FlagListStreamer(
            ILogger logger
        )
            : base(logger, "Flags.txt")
        {
        }

        public List<Flag> TryRead(string fieldPath)
        {
            List<Flag> flagList = null;
            string fullPath = FullPath(fieldPath);
            if (!File.Exists(fullPath))
            {
                _presenter.PresentFlagsFileMissing();
            }
            else
            {
                try
                {
                    flagList = Read(fieldPath);
                }

                catch (Exception e)
                {
                    _presenter.PresentFlagsFileCorrupt();
                    _logger.LogError("FieldOpen, Loading Flags, Corrupt Flag File" + e.ToString());
                }
            }
            return flagList;
        }

        public void TryWrite(List<Flag> flags, string fieldPath)
        {
            try
            {
                Write(flags, fieldPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                _logger.LogError("Saving Flags" + e.ToString());
            }
        }

        private FlagColor FlagColorFromInt(int intColor)
        {
            switch (intColor)
            {
                case 1:
                    return FlagColor.Green;
                case 2:
                    return FlagColor.Yellow;
            }
            return FlagColor.Red;
        }

        private List<Flag> Read(string fieldPath)
        {
            List<Flag> flagList = new List<Flag>();
            using (GeoStreamReader reader = new GeoStreamReader(FullPath(fieldPath)))
            {
                string line = reader.ReadLine(); // skip header

                int nFlags = reader.ReadInt();
                for (int v = 0; v < nFlags; v++)
                {
                    line = reader.ReadLine();
                    string[] words = line.Split(',');

                    Wgs84 wgs84 = reader.ParseWgs84(words[0], words[1]);
                    GeoCoord geoCoord = reader.ParseGeoCoord(words[3], words[2]);

                    bool isComplete = words.Length == 8;
                    double head = isComplete ? reader.ParseDouble(words[4]) : 0.0;
                    int intColor = int.Parse(words[isComplete ? 5 : 4]);
                    int number = int.Parse(words[isComplete ? 6 : 5]);
                    string notes = isComplete ? words[7].Trim() : "";

                    Flag flag = new Flag(wgs84, geoCoord, new GeoDir(head), FlagColorFromInt(intColor), number, notes);
                    flagList.Add(flag);
                }
            }
            return flagList;
        }

        private int FlagColorToInt(FlagColor flagColor)
        {
            switch (flagColor)
            {
                case FlagColor.Green:
                    return 1;
                case FlagColor.Yellow:
                    return 2;
            }
            return 0;
        }

        private void Write(List<Flag> flags, string fieldPath)
        {
            CreateDirectory(fieldPath);
            using (GeoStreamWriter writer = new GeoStreamWriter(FullPath(fieldPath)))
            {
                writer.WriteLine("$Flags");

                int nFlags = flags.Count;
                writer.WriteLine(nFlags);

                foreach (Flag flag in flags)
                {
                    writer.WriteLine(
                        writer.Wgs84String(flag.Wgs84)
                        + "," + writer.GeoCoordDirStringENH(flag.GeoCoord, flag.Heading)
                        + "," + writer.IntString(FlagColorToInt(flag.FlagColor))
                        + "," + writer.IntString(flag.UniqueNumber)
                        + "," + flag.Notes);
                }
            }
        }

        public void CreateFile(string fieldPath)
        {
            CreateDirectory(fieldPath);
            using (StreamWriter writer = new StreamWriter(FullPath(fieldPath)))
            {
            }
        }
    }
}

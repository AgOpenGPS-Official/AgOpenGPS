using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class GeoStreamReader : StreamReader
    {
        public GeoStreamReader(string fullPath) : base(fullPath)
        {
        }

        public bool ParseBool(string boolWord)
        {
            return bool.Parse(boolWord);
        }

        public byte ParseByte(string byteWord)
        {
            return byte.Parse(byteWord);
        }

        public double ParseDouble(string doubleWord)
        {
            return double.Parse(doubleWord, CultureInfo.InvariantCulture);
        }

        public Wgs84 ParseWgs84(string latWord, string longWord)
        {
            return new Wgs84(ParseDouble(latWord), ParseDouble(longWord));
        }

        public GeoCoord ParseGeoCoord(string northingWord, string eastingWord)
        {
            return new GeoCoord(ParseDouble(northingWord), ParseDouble(eastingWord));
        }

        public GeoCoordDir ParseGeoCoordDir(string northingWord, string eastingWord, string directionWord)
        {
            return new GeoCoordDir(
                ParseGeoCoord(northingWord, eastingWord),
                ParseGeoDir(directionWord));
        }

        public GeoDir ParseGeoDir(string angleWord)
        {
            return new GeoDir(ParseDouble(angleWord));
        }

        // Read line and parse integer
        public int ReadInt()
        {
            return int.Parse(ReadLine());
        }

        public ColorRgb ReadColorRgb()
        {
            string line = ReadLine();
            string[] words = line.Split(',');

            return new ColorRgb(ParseByte(words[0]), ParseByte(words[1]), ParseByte(words[2]));
        }

        public GeoCoord ReadGeoCoord()
        {
            string line = ReadLine();
            string[] words = line.Split(',');
            return ParseGeoCoord(words[1], words[0]);
        }

        public GeoCoordDir ReadGeoCoordDir()
        {
            string line = ReadLine();
            string[] words = line.Split(',');
            return ParseGeoCoordDir(words[1], words[0], words[2]);
        }

        public GeoPath ReadGeoPath()
        {
            var result = new GeoPath();
            int count = ReadInt();
            for (int i = 0; i < count; i++)
            {
                result.Add(ReadGeoCoord());
            }
            return result;
        }

        // Returns null, (not an empty GeoPolygon) when the polygon has no points
        public GeoPolygon ReadGeoPolygon()
        {
            GeoPolygon result = null;
            int count = ReadInt();
            if (0 < count) result = new GeoPolygon();
            for (int i = 0; i < count; i++)
            {
                result.Add(ReadGeoCoord());
            }
            return result;
        }

        public void ReadGeoPolygonWithHeading(GeoPolygonWithHeading polygon)
        {
            int count = ReadInt();
            for (int i = 0; i < count; i++)
            {
                var coordDir = ReadGeoCoordDir();
                polygon.Add(coordDir.Coord, coordDir.Direction);
            }
        }

        public bool PeekReadBool(out bool boolValue)
        {
            int asciiCode = Peek();
            if ('A' <= asciiCode && asciiCode <= 'Z')
            {
                boolValue = bool.Parse(ReadLine());
                return true;
            }
            boolValue = false;
            return false;
        }
    }
}

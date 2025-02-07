using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class GeoStreamWriter : StreamWriter
    {
        public GeoStreamWriter(string fullPath, bool append = false) : base(fullPath, append)
        {
        }

        public string BoolString(bool boolValue)
        {
            return boolValue.ToString(CultureInfo.InvariantCulture);
        }

        public string IntString(int intValue)
        {
            return intValue.ToString(CultureInfo.InvariantCulture);
        }

        public string DoubleString(double value, string formatString)
        {
            return value.ToString(formatString, CultureInfo.InvariantCulture);
        }

        public string Wgs84String(Wgs84 wgs84, string formatString = "N7")
        {
            return
               DoubleString(wgs84.Latitude, formatString) + "," +
               DoubleString(wgs84.Longitude, formatString);
        }

        public string GeoCoordStringEN(GeoCoord geoCoord, string formatString = "N3")
        {
            return
               DoubleString(geoCoord.Easting, formatString) + "," +
               DoubleString(geoCoord.Northing, formatString);
        }

        public string HeadingStringH(GeoDir geoDir, string formatString = "N3")
        {
            return
                geoDir.Angle.ToString(formatString, CultureInfo.InvariantCulture);
        }

        public string GeoCoordDirStringENH(GeoCoord coord, GeoDir heading, string coordFormatString = "N3", string headingFormatString = "N5")
        {
            return
                GeoCoordStringEN(coord, coordFormatString) + "," +
                HeadingStringH(heading, headingFormatString);
        }

        public void WriteBool(bool boolValue)
        {
            WriteLine(BoolString(boolValue));
        }

        public void WriteInt(int intValue)
        {
            WriteLine(IntString(intValue));
        }

        public void WriteColorRgb(ColorRgb colorRgb)
        {
            WriteLine(IntString(colorRgb.Red) + "," + IntString(colorRgb.Green) + "," + IntString(colorRgb.Blue));
        }

        public void WriteGeoCoordEN(GeoCoord coord)
        {
            WriteLine(GeoCoordStringEN(coord));
        }

        public void WriteGeoCoordDirENH(GeoCoord coord, GeoDir direction)
        {
            WriteLine(GeoCoordDirStringENH(coord, direction));
        }

        public void WriteGeoPath(GeoPath path)
        {
            WriteInt(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                WriteGeoCoordEN(path[i]);
            }
        }

        public void WriteGeoPolygon(GeoPolygon polygon)
        {
            if (null == polygon)
            {
                WriteInt(0);
                return;
            }
            WriteInt(polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                WriteGeoCoordEN(polygon[i]);
            }
        }

        public void WriteGeoPolygonWithHeading(GeoPolygonWithHeading polygon)
        {
            if (null == polygon)
            {
                WriteInt(0);
                return;
            }
            WriteInt(polygon.Count);
            for (int i = 0; i < polygon.Count; i++)
            {
                WriteGeoCoordDirENH(polygon[i], polygon.GetHeading(i));
            }
        }
    }
}

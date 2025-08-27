using AgOpenGPS.Core.Models;
using System;
using System.Globalization;
using System.IO;

namespace AgOpenGPS.Core.Streamers
{
    public class GeoStreamWriter : StreamWriter
    {
        public GeoStreamWriter(FileInfo fileInfo, bool append = false) : base(fileInfo.FullName, append)
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
                geoDir.AngleInRadians.ToString(formatString, CultureInfo.InvariantCulture);
        }

        public string GeoCoordDirStringENH(GeoCoord coord, GeoDir heading, string coordFormatString = "N3", string headingFormatString = "N5")
        {
            return
                GeoCoordStringEN(coord, coordFormatString) + "," +
                HeadingStringH(heading, headingFormatString);
        }

        public void WriteDateTime()
        {
            WriteLine(DateTime.Now.ToString("yyyy-MMMM-dd hh:mm:ss tt", CultureInfo.InvariantCulture));
        }

        public void WriteBool(bool boolValue)
        {
            WriteLine(BoolString(boolValue));
        }

        public void WriteInt(int intValue)
        {
            WriteLine(IntString(intValue));
        }

        public void WriteDouble(double doubleValue)
        {
            WriteLine(DoubleString(doubleValue, "N7"));
        }

        public void WriteString(string stringValue)
        {
            WriteLine(stringValue ?? "");
        }

        public void WriteColorRgb(ColorRgb colorRgb)
        {
            WriteLine(IntString(colorRgb.Red) + "," + IntString(colorRgb.Green) + "," + IntString(colorRgb.Blue));
        }

        public void WriteWgs84(Wgs84 wgs84)
        {
            WriteLine(Wgs84String(wgs84));
        }

        public void WriteGeoCoordEN(GeoCoord coord)
        {
            WriteLine(GeoCoordStringEN(coord));
        }

        public void WriteGeoCoordDirENH(GeoCoord coord, GeoDir direction)
        {
            WriteLine(GeoCoordDirStringENH(coord, direction));
        }

        public void WriteGeoBoundingBox(GeoBoundingBox bb)
        {
            WriteDouble(bb.MaxEasting);
            WriteDouble(bb.MinEasting);
            WriteDouble(bb.MaxNorthing);
            WriteDouble(bb.MinNorthing);
        }

        public void WriteGeoPath(GeoPath path)
        {
            WriteInt(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                WriteGeoCoordEN(path[i]);
            }
        }

        public void WriteGeoPathWithHeading(GeoPathWithHeading path)
        {
            if (null == path)
            {
                WriteInt(0);
                return;
            }
            WriteInt(path.Count);
            for (int i = 0; i < path.Count; i++)
            {
                WriteGeoCoordDirENH(path[i], path.GetHeading(i));
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

using System;
using System.Windows.Forms;

namespace AgIO
{
    public static class glm
    {
        //Regex file expression
        public const string fileRegex = "(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\.]+)";

        //Degrees Radians Conversions
        public static double toDegrees(double radians)
        {
            return radians * 57.295779513082325225835265587528;
        }

        public static double toRadians(double degrees)
        {
            return degrees * 0.01745329251994329576923690768489;
        }

        //Distance calcs of all kinds
        public static double DistanceLonLat(double lon1, double lat1, double lon2, double lat2)
        {
            const int EarthMeanRadius = 6371;

            double dlon = toRadians(lon2 - lon1);
            double dlat = toRadians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(toRadians(lat1)) * Math.Cos(toRadians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * EarthMeanRadius;
        }

    }
}
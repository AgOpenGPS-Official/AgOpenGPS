using System;
using System.Windows.Forms;

namespace AgIO
{
    public static class NudChk
    {
        public static bool CheckValue(this NumericUpDown numericUpDown, ref decimal value)
        {
            if (value < numericUpDown.Minimum)
            {
                value = numericUpDown.Minimum;
                MessageBox.Show("Serious Settings Problem With - " + numericUpDown.Name
                    + " \n\rMinimum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return true;
            }
            else if (value > numericUpDown.Maximum)
            {
                value = numericUpDown.Maximum;
                MessageBox.Show("Serious Settings Problem with - " + numericUpDown.Name
                    + " \n\rMaximum has been exceeded\n\rDouble Check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                return true;
            }

            //value is ok
            return false;
        }

        public static bool CheckValueCm(this NumericUpDown numericUpDown, ref double value)
        {
            //convert to cm
            value *= 100;
            bool isChanged = false;

            if (value < (double)numericUpDown.Minimum)
            {
                value = (double)numericUpDown.Minimum / 2.4;
                MessageBox.Show("Serious Settings Problem with - " + numericUpDown.Name
                    + " \n\rMinimum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                isChanged = true;
            }
            else if (value > (double)numericUpDown.Maximum)
            {
                value = (double)numericUpDown.Maximum / 2.6;
                MessageBox.Show("Serious Settings Problem with - " + numericUpDown.Name
                    + " \n\rMaximum has been exceeded\n\rDouble check ALL your Settings and \n\rFix it and Resave Vehicle File",
                "Critical Settings Warning",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
                isChanged = true;
            }

            //revert back to meters
            value *= 0.01;

            //value is ok
            return isChanged;
        }
    }

    public static class glm
    {
        public static byte[] Combine(byte[] first, byte[] second)
        {
            byte[] ret = new byte[first.Length + second.Length];
            Buffer.BlockCopy(first, 0, ret, 0, first.Length);
            Buffer.BlockCopy(second, 0, ret, first.Length, second.Length);
            return ret;
        }

        //Regex file expression
        public const string fileRegex = "(^(PRN|AUX|NUL|CON|COM[1-9]|LPT[1-9]|(\\.+)$)(\\..*)?$)|(([\\x00-\\x1f\\\\?*:\";‌​|/<>])+)|([\\.]+)";

        //inches to meters
        public const double in2m = 0.0254;

        //meters to inches
        public const double m2in = 39.3701;

        //meters to feet
        public const double m2ft = 3.28084;

        //Hectare to Acres
        public const double ha2ac = 2.47105;

        //Acres to Hectare
        public const double ac2ha = 0.404686;

        //Meters to Acres
        public const double m2ac = 0.000247105;

        //Meters to Hectare
        public const double m2ha = 0.0001;

        // liters per hectare to us gal per acre
        public const double galAc2Lha = 9.35396;

        //us gal per acre to liters per hectare
        public const double LHa2galAc = 0.106907;

        //Liters to Gallons
        public const double L2Gal = 0.264172;

        //Gallons to Liters
        public const double Gal2L = 3.785412534258;

        //the pi's
        public const double twoPI = 6.28318530717958647692;

        public const double PIBy2 = 1.57079632679489661923;

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
        public static double Distance(double east1, double north1, double east2, double north2)
        {
            return Math.Sqrt(
                Math.Pow(east1 - east2, 2)
                + Math.Pow(north1 - north2, 2));
        }

        public static double DistanceLonLat(double lon1, double lat1, double lon2, double lat2)
        {
            const int EarthMeanRadius = 6371;

            double dlon = toRadians(lon2 - lon1);
            double dlat = toRadians(lat2 - lat1);

            double a = (Math.Sin(dlat / 2) * Math.Sin(dlat / 2)) + Math.Cos(toRadians(lat1)) * Math.Cos(toRadians(lat2)) * (Math.Sin(dlon / 2) * Math.Sin(dlon / 2));
            double angle = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return angle * EarthMeanRadius;
        }

        //float functions
        public static float acos(float x)
        {
            return (float)Math.Acos(x);
        }

        public static float acosh(float x)
        {
            if (x < 1f) return 0f;
            return (float)Math.Log(x + Math.Sqrt((x * x) - 1f));
        }

        public static float asin(float x)
        {
            return (float)Math.Asin(x);
        }

        public static float asinh(float x)
        {
            return (x < 0f ? -1f : (x > 0f ? 1f : 0f)) * (float)Math.Log(Math.Abs(x) + Math.Sqrt(1f + (x * x)));
        }

        public static float atan(float y, float x)
        {
            return (float)Math.Atan2(y, x);
        }

        public static float atan(float y_over_x)
        {
            return (float)Math.Atan(y_over_x);
        }

        public static float atanh(float x)
        {
            if (Math.Abs(x) >= 1f) return 0;
            return 0.5f * (float)Math.Log((1f + x) / (1f - x));
        }

        public static float cos(float angle)
        {
            return (float)Math.Cos(angle);
        }

        public static float cosh(float angle)
        {
            return (float)Math.Cosh(angle);
        }

        public static float toDegrees(float radians)
        {
            return radians * 57.295779513082325225835265587528f;
        }

        public static float toRadians(float degrees)
        {
            return degrees * 0.01745329251994329576923690766743f;
        }

        public static float sin(float angle)
        {
            return (float)Math.Sin(angle);
        }

        public static float sinh(float angle)
        {
            return (float)Math.Sinh(angle);
        }

        public static float tan(float angle)
        {
            return (float)Math.Tan(angle);
        }

        public static float tanh(float angle)
        {
            return (float)Math.Tanh(angle);
        }
    }
}
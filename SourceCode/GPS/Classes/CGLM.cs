using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace AgOpenGPS
{

    public static class glm
    {
        public static void GetClosestSegmentLooping(this vec2 Point, List<vec3> curList, out int AA, out int BB, out double minDistA, int Start = 0, int End = int.MaxValue)
        {
            bool isLoop = true;
            AA = -1; BB = -1;
            minDistA = double.MaxValue;
            int A = Start > 0 ? Start - 1 : curList.Count - 1;
            bool looping = isLoop & Start > End;
            for (int B = (Start > 0 ? Start : 0); B < End || looping; A = B++)
            {
                if (B == curList.Count)
                {
                    if (looping)
                    {
                        B = -1; looping = false;
                        continue;
                    }
                    else break;
                }
                if (B == 0 && !isLoop) continue;
                else if (B == 0) A = curList.Count - 1;

                double dist2 = DistanceSquared(curList[A], curList[B]);
                if (dist2 < minDistA)
                {
                    minDistA = dist2; AA = A;
                    BB = B;
                }
            }
        }

        public static bool InRangeBetweenAB(double start_x, double start_y, double end_x, double end_y,
          double point_x, double point_y)
        {
            double dx = end_x - start_x;
            double dy = end_y - start_y;
            double innerProduct = (point_x - start_x) * dx + (point_y - start_y) * dy;
            return 0 <= innerProduct && innerProduct <= dx * dx + dy * dy;
        }

        public static bool IsPointInPolygon(this List<vec3> polygon, vec3 testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((polygon[i].easting < testPoint.easting && polygon[j].easting >= testPoint.easting)
                    || (polygon[j].easting < testPoint.easting && polygon[i].easting >= testPoint.easting))
                {
                    if (polygon[i].northing + (testPoint.easting - polygon[i].easting)
                        / (polygon[j].easting - polygon[i].easting) * (polygon[j].northing - polygon[i].northing)
                        < testPoint.northing)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static bool IsPointInPolygon(this List<vec3> polygon, vec2 testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((polygon[i].easting < testPoint.easting && polygon[j].easting >= testPoint.easting)
                    || (polygon[j].easting < testPoint.easting && polygon[i].easting >= testPoint.easting))
                {
                    if (polygon[i].northing + (testPoint.easting - polygon[i].easting)
                        / (polygon[j].easting - polygon[i].easting) * (polygon[j].northing - polygon[i].northing)
                        < testPoint.northing)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static bool IsPointInPolygon(this List<vec2> polygon, vec2 testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((polygon[i].easting < testPoint.easting && polygon[j].easting >= testPoint.easting)
                    || (polygon[j].easting < testPoint.easting && polygon[i].easting >= testPoint.easting))
                {
                    if (polygon[i].northing + (testPoint.easting - polygon[i].easting)
                        / (polygon[j].easting - polygon[i].easting) * (polygon[j].northing - polygon[i].northing)
                        < testPoint.northing)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static bool IsPointInPolygon(this List<vec2> polygon, vec3 testPoint)
        {
            bool result = false;
            int j = polygon.Count - 1;
            for (int i = 0; i < polygon.Count; i++)
            {
                if ((polygon[i].easting < testPoint.easting && polygon[j].easting >= testPoint.easting)
                    || (polygon[j].easting < testPoint.easting && polygon[i].easting >= testPoint.easting))
                {
                    if (polygon[i].northing + (testPoint.easting - polygon[i].easting)
                        / (polygon[j].easting - polygon[i].easting) * (polygon[j].northing - polygon[i].northing)
                        < testPoint.northing)
                    {
                        result = !result;
                    }
                }
                j = i;
            }
            return result;
        }

        public static void DrawPolygon(this List<vec3> polygon)
        {
            if (polygon.Count > 2)
            {
                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < polygon.Count; i++)
                {
                    GL.Vertex3(polygon[i].easting, polygon[i].northing, 0);
                }
                GL.End();

                //GL.PointSize(4.0f);
                //GL.Begin(PrimitiveType.Points);
                //GL.Color3(1.0f, 1.0f, 0.50f);
                //for (int i = 0; i < polygon.Count; i++)
                //{
                //    GL.Vertex3(polygon[i].easting, polygon[i].northing, 0);
                //}
                //GL.End();
                //GL.PointSize(1.0f);
            }
        }

        public static void DrawPolygon(this List<vec2> polygon)
        {
            if (polygon.Count > 2)
            {
                GL.Begin(PrimitiveType.LineLoop);
                for (int i = 0; i < polygon.Count; i++)
                {
                    GL.Vertex3(polygon[i].easting, polygon[i].northing, 0);
                }
                GL.End();

                //GL.PointSize(8.0f);
                //GL.Begin(PrimitiveType.Points);
                //GL.Color3(1.0f, 1.0f, 0.50f);
                //for (int i = 0; i < polygon.Count; i++)
                //{
                //    GL.Vertex3(polygon[i].easting, polygon[i].northing, 0);
                //}
                //GL.End();
                //GL.PointSize(1.0f);
            }
        }

        // Catmull Rom interpoint spline calculation
        public static vec3 Catmull(double t, vec3 p0, vec3 p1, vec3 p2, vec3 p3)
        {
            double tt = t * t;
            double ttt = tt * t;

            double q1 = -ttt + 2.0f * tt - t;
            double q2 = 3.0f * ttt - 5.0f * tt + 2.0f;
            double q3 = -3.0f * ttt + 4.0f * tt + t;
            double q4 = ttt - tt;

            double tx = 0.5f * (p0.easting * q1 + p1.easting * q2 + p2.easting * q3 + p3.easting * q4);
            double ty = 0.5f * (p0.northing * q1 + p1.northing * q2 + p2.northing * q3 + p3.northing * q4);

            vec3 ret = new vec3(tx, ty, 0);
            return ret;

            //f(t) = a_3 * t^3 + a_2 * t^2 + a_1 * t + a_0  cubic polynomial
            //vec3 a = 2.0 * p1;
            //vec3 b = p2 - p0;
            //vec3 c = 2.0 * p0 - 5.0 * p1 + 4.0 * p2 - p3;
            //vec3 d = -1.0 * p0 + 3.0 * p1 - 3.0 * p2 + p3;

            //return (0.5 * (a + (t * b) + (t * t * c) + (t * t * t * d)));
            //

            //vec2 p0 = new vec2(1, 0);
            //vec2 p1 = new vec2(3, 2);
            //vec2 p2 = new vec2(5, 3);
            //vec2 p3 = new vec2(6, 1);

            //vec2 a = 2.0 * p1;
            //vec2 b = p2 - p0;
            //vec2 c = 2.0 * p0 - 5.0 * p1 + 4.0 * p2 - p3;
            //vec2 d = -1.0 * p0 + 3.0 * p1 - 3.0 * p2 + p3;

            //double tt = 0.25;

            //vec2 pos = 0.5 * (a + (tt*b) + (tt * tt * c) + (tt * tt * tt * d));
        }

        // Catmull Rom gradient calculation
        public static double CatmullGradient(double t, vec3 p0, vec3 p1, vec3 p2, vec3 p3)
        {
            double tt = t * t;

            double q1 = -3.0f * tt + 4.0f * t - 1;
            double q2 = 9.0f * tt - 10.0f * t;
            double q3 = -9.0f * tt + 8.0f * t + 1.0f;
            double q4 = 3.0f * tt - 2.0f * t;

            double tx = 0.5f * (p0.easting * q1 + p1.easting * q2 + p2.easting * q3 + p3.easting * q4);
            double ty = 0.5f * (p0.northing * q1 + p1.northing * q2 + p2.northing * q3 + p3.northing * q4);

            return Math.Atan2(tx, ty);

            //f(t) = a_3 * t^3 + a_2 * t^2 + a_1 * t + a_0  cubic polynomial
            //vec3 a = 2.0 * p1;
            //vec3 b = p2 - p0;
            //vec3 c = 2.0 * p0 - 5.0 * p1 + 4.0 * p2 - p3;
            //vec3 d = -1.0 * p0 + 3.0 * p1 - 3.0 * p2 + p3;

            //return (0.5 * (a + (t * b) + (t * t * c) + (t * t * t * d)));
            //
        }

        //Regex file expression
        public const string fileRegex = " /^(?!.{256,})(?!(aux|clock\\$|con|nul|prn|com[1-9]|lpt[1-9])(?:$|\\.))[^ ][ \\.\\w-$()+=[\\];#@~,&amp;']+[^\\. ]$/i";

        //inches to meters
        public const double in2m = 0.0254;

        //meters to inches
        public const double m2in = 39.3701;

        //meters to feet
        public const double m2ft = 3.28084;

        //feet to meters
        public const double ft2m = 0.3048;

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

        public static double Distance(vec2 first, vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vec2 first, vec3 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vec3 first, vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vec3 first, vec3 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vec2 first, double east, double north)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - east, 2)
                + Math.Pow(first.northing - north, 2));
        }

        public static double Distance(vec3 first, double east, double north)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - east, 2)
                + Math.Pow(first.northing - north, 2));
        }

        public static double Distance(vecFix2Fix first, vec2 second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static double Distance(vecFix2Fix first, vecFix2Fix second)
        {
            return Math.Sqrt(
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        //not normalized distance, no square root
        public static double DistanceSquared(double northing1, double easting1, double northing2, double easting2)
        {
            return Math.Pow(easting1 - easting2, 2) + Math.Pow(northing1 - northing2, 2);
        }

        public static double DistanceSquared(vec3 first, vec2 second)
        {
            return (
            Math.Pow(first.easting - second.easting, 2)
            + Math.Pow(first.northing - second.northing, 2));
        }

        public static double DistanceSquared(vec2 first, vec3 second)
        {
            return (
            Math.Pow(first.easting - second.easting, 2)
            + Math.Pow(first.northing - second.northing, 2));
        }

        public static double DistanceSquared(vec3 first, vec3 second)
        {
            return (
            Math.Pow(first.easting - second.easting, 2)
            + Math.Pow(first.northing - second.northing, 2));
        }

        public static double DistanceSquared(vec2 first, vec2 second)
        {
            return (
            Math.Pow(first.easting - second.easting, 2)
            + Math.Pow(first.northing - second.northing, 2));
        }

        public static double DistanceSquared(vecFix2Fix first, vec2 second)
        {
            return (
                Math.Pow(first.easting - second.easting, 2)
                + Math.Pow(first.northing - second.northing, 2));
        }

        public static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);
            //get a graphics object from the new image
            Graphics g = Graphics.FromImage(newBitmap);
            //create the grayscale ColorMatrix
            ColorMatrix colorMatrix = new ColorMatrix(
               new float[][]
              {
                 new float[] {.3f, .3f, .3f, 0, 0},
                 new float[] {.59f, .59f, .59f, 0, 0},
                 new float[] {.11f, .11f, .11f, 0, 0},
                 new float[] {0, 0, 0, 1, 0},
                 new float[] {0, 0, 0, 0, 1}
              });
            //create some image attributes
            ImageAttributes attributes = new ImageAttributes();
            //set the color matrix attribute
            attributes.SetColorMatrix(colorMatrix);
            //draw the original image on the new image
            //using the grayscale color matrix
            g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
               0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
            //dispose the Graphics object
            g.Dispose();
            return newBitmap;
        }
    }
}
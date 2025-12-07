using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {
        public static void DrawPoint(GeoCoord geoCoord)
        {
            GL.Begin(PrimitiveType.Points);
            GL.Vertex2(geoCoord.Easting, geoCoord.Northing);
            GL.End();
        }

        public static void DrawLinesPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.Lines, vertices);
        }
        public static void DrawLineLoopPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineLoop, vertices);
        }

        public static void DrawLineStripPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineStrip, vertices);
        }

        public static void DrawPointsPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.Points, vertices);
        }

        public static void DrawTriangleFanPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.TriangleFan, vertices);
        }

        public static void DrawPointLayered(
            PointStyle[] pointStyles,
            GeoCoord coord)
        {
            foreach (PointStyle pointStyle in pointStyles)
            {
                SetPointStyle(pointStyle);
                DrawPoint(coord);
            }
        }

        public static void DrawLinesPrimitiveLayered(
            LineStyle[] lineStyles,
            GeoCoord[] vertices)
        {
            DrawPrimitiveLayered(PrimitiveType.Lines, lineStyles, vertices);
        }

        public static void DrawLineLoopPrimitiveLayered(
            LineStyle[] lineStyles,
            GeoCoord[] vertices)
        {
            DrawPrimitiveLayered(PrimitiveType.LineLoop, lineStyles, vertices);
        }

        private static void DrawPrimitive(PrimitiveType primitiveType, GeoCoord[] vertices)
        {
            if (vertices.Length >= MinVerticesForArray)
            {
                Vertex2Array vertex2Array = new Vertex2Array(vertices);
                GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
                vertex2Array.Dispose();
            }
            else
            {
                GL.Begin(primitiveType);
                foreach (GeoCoord vertex in vertices)
                {
                    Vertex2(vertex);
                }
                GL.End();
            }
        }

        private static void DrawPrimitiveLayered(
            PrimitiveType primitiveType,
            LineStyle[] lineStyles,
            GeoCoord[] vertices)
        {
            if (lineStyles.Length * vertices.Length >= MinVerticesForArray)
            {
                Vertex2Array vertex2Array = new Vertex2Array(vertices);
                foreach (LineStyle lineStyle in lineStyles)
                {
                    SetLineStyle(lineStyle);
                    GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
                }
                vertex2Array.Dispose();
            }
            else
            {
                foreach (LineStyle lineStyle in lineStyles)
                {
                    SetLineStyle(lineStyle);
                    GL.Begin(primitiveType);
                    foreach (GeoCoord vertex in vertices)
                    {
                        Vertex2(vertex);
                    }
                    GL.End();
                }
            }
        }

        public static void Vertex2(GeoCoord geoCoord)
        {
            GL.Vertex2(geoCoord.Easting, geoCoord.Northing);
        }

    }
}

using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {

        public static void DrawPoint(double x, double y, double z)
        {
            GL.Begin(PrimitiveType.Points);
            GL.Vertex3(x, y, z);
            GL.End();
        }

        public static void DrawLinesPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.Lines, vertices);
        }

        public static void DrawLineLoopPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineLoop, vertices);
        }

        public static void DrawLineStripPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineStrip, vertices);
        }

        public static void DrawTriangleFanPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.TriangleFan, vertices);
        }

        public static void DrawPointLayered(
            PointStyle[] pointStyles,
            double x, double y, double z)
        {
            foreach (PointStyle pointStyle in pointStyles)
            {
                SetPointStyle(pointStyle);
                DrawPoint(x, y, z);
            }
        }

        public static void DrawLinesPrimitiveLayered(
            LineStyle[] lineStyles,
            XyCoord[] vertices)
        {
            DrawPrimitiveLayered(PrimitiveType.Lines, lineStyles, vertices);
        }

        public static void DrawLineLoopPrimitiveLayered(
            LineStyle[] lineStyles,
            XyCoord[] vertices)
        {
            DrawPrimitiveLayered(PrimitiveType.LineLoop, lineStyles, vertices);
        }

        private static void DrawPrimitive(PrimitiveType primitiveType, XyCoord[] vertices)
        {
            Vertex2Array vertex2Array = new Vertex2Array(vertices);
            GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
            vertex2Array.Dispose();
        }

        private static void DrawPrimitiveLayered(
            PrimitiveType primitiveType,
            LineStyle[] lineStyles,
            XyCoord[] vertices)
        {
            Vertex2Array vertex2Array = new Vertex2Array(vertices);
            foreach (LineStyle lineStyle in lineStyles)
            {
                SetLineStyle(lineStyle);
                GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
            }
            vertex2Array.Dispose();
        }

    }
}

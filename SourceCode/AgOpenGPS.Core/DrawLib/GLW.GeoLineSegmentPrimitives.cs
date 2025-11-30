using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {
        public static void DrawLinesPrimitive(GeoLineSegment[] lineSegments)
        {
            DrawPrimitive(PrimitiveType.Lines, lineSegments);
        }

        public static void DrawLinesPrimitiveLayered(
            LineStyle[] lineStyles,
            GeoLineSegment[] segments)
        {
            DrawPrimitiveLayered(PrimitiveType.Lines, lineStyles, segments);
        }

        private static void DrawPrimitive(PrimitiveType primitiveType, GeoLineSegment[] lineSegments)
        {
            Vertex2Array vertex2Array = new Vertex2Array(lineSegments);
            GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
            vertex2Array.Dispose();
        }

        private static void DrawPrimitiveLayered(
            PrimitiveType primitiveType,
            LineStyle[] lineStyles,
            GeoLineSegment[] lineSegments)
        {
            Vertex2Array vertex2Array = new Vertex2Array(lineSegments);
            foreach (LineStyle lineStyle in lineStyles)
            {
                SetLineStyle(lineStyle);
                GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
            }
            vertex2Array.Dispose();
        }

    }
}
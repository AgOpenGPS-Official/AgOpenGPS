using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System.Linq;

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
            if (2 * lineSegments.Length >= MinVerticesForArray)
            {
                Vertex2Array vertex2Array = new Vertex2Array(lineSegments);
                GL.DrawArrays(primitiveType, 0, vertex2Array.Length);
                vertex2Array.Dispose();
            }
            else
            {
                GL.Begin(primitiveType);
                foreach (var segment in lineSegments)
                {
                    Vertex2(segment.CoordA);
                    Vertex2(segment.CoordB);
                }
                GL.End();
            }
        }

        private static void DrawPrimitiveLayered(
            PrimitiveType primitiveType,
            LineStyle[] lineStyles,
            GeoLineSegment[] lineSegments)
        {
            if (2 * lineStyles.Length * lineSegments.Length >= MinVerticesForArray)
            {
                Vertex2Array vertex2Array = new Vertex2Array(lineSegments);
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
                    foreach (var segment in lineSegments)
                    {
                        Vertex2(segment.CoordA);
                        Vertex2(segment.CoordB);
                    }
                    GL.End();
                }
            }
        }

    }
}
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
            GeoLineSegment[] lineSegments,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            DrawPrimitiveLayered(PrimitiveType.Lines, lineSegments, backgroundStyle, foregroundStyle);
        }

        private static void DrawPrimitive(PrimitiveType primitiveType, GeoLineSegment[] lineSegments)
        {
            const int nVerticesPerSegment = 2;
            if (nVerticesPerSegment * lineSegments.Length >= MinVerticesForArray)
            {
                using (VertexBuffer vertexBuffer = new VertexBuffer(lineSegments))
                {
                    DrawArrays(primitiveType, vertexBuffer);
                }
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
            GeoLineSegment[] lineSegments,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            const int nLayers = 2;
            const int nVerticesPerSegment = 2;
            if (nLayers * nVerticesPerSegment * lineSegments.Length >= MinVerticesForArray)
            {
                using (VertexBuffer vertexBuffer = new VertexBuffer(lineSegments))
                {
                    // background layer
                    SetLineWidth(backgroundStyle.Width);
                    SetColor(backgroundStyle.Color);
                    DrawArrays(primitiveType, vertexBuffer);
                    // foreground layer
                    SetLineWidth(foregroundStyle.Width);
                    SetColor(foregroundStyle.Color);
                    DrawArrays(primitiveType, vertexBuffer);
                }
            }
            else
            {
                // background layer
                SetLineWidth(backgroundStyle.Width);
                SetColor(backgroundStyle.Color);
                GL.Begin(primitiveType);
                foreach (var segment in lineSegments)
                {
                    Vertex2(segment.CoordA);
                    Vertex2(segment.CoordB);
                }
                GL.End();
                // foreground layer
                SetLineWidth(foregroundStyle.Width);
                SetColor(foregroundStyle.Color);
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
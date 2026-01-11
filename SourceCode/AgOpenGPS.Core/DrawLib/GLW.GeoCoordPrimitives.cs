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

        // Inlined by the compiler, so no function call overhead
        public static void DrawLinesPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.Lines, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineLoopPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineLoop, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineStripPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineStrip, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawPointsPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.Points, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawTriangleFanPrimitive(GeoCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.TriangleFan, vertices);
        }

        public static void DrawLinesPrimitiveLayered(
            GeoCoord[] vertices,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            DrawPrimitiveLayered(
                PrimitiveType.Lines,
                vertices,
                backgroundStyle,
                foregroundStyle);
        }

        public static void DrawLineLoopPrimitiveLayered(
            GeoCoord[] vertices,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            DrawPrimitiveLayered(
                PrimitiveType.LineLoop,
                vertices,
                backgroundStyle,
                foregroundStyle);
        }

        private static void DrawPrimitive(PrimitiveType primitiveType, GeoCoord[] vertices)
        {
            if (vertices.Length >= MinVerticesForArray)
            {
                using (VertexBuffer vertexBuffer = new VertexBuffer(vertices))
                {
                    DrawArrays(primitiveType, vertexBuffer);
                }
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
            GeoCoord[] vertices,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            const int nLayers = 2;
            if (nLayers * vertices.Length >= MinVerticesForArray)
            {
                using (VertexBuffer vertexBuffer = new VertexBuffer(vertices))
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
                foreach (GeoCoord vertex in vertices)
                {
                    Vertex2(vertex);
                }
                GL.End();
                // foreground layer
                SetLineWidth(foregroundStyle.Width);
                SetColor(foregroundStyle.Color);
                GL.Begin(primitiveType);
                foreach (GeoCoord vertex in vertices)
                {
                    Vertex2(vertex);
                }
                GL.End();
            }
        }

        public static void Vertex2(GeoCoord geoCoord)
        {
            GL.Vertex2(geoCoord.Easting, geoCoord.Northing);
        }

    }
}

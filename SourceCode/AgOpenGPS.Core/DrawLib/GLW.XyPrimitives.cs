using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

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

        // Inlined by the compiler, so no function call overhead
        public static void DrawLinesPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.Lines, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineLoopPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineLoop, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineStripPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.LineStrip, vertices);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawTriangleFanPrimitive(XyCoord[] vertices)
        {
            DrawPrimitive(PrimitiveType.TriangleFan, vertices);
        }

        public static void DrawLinesPrimitiveLayered(
            XyCoord[] vertices,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            DrawPrimitiveLayered(PrimitiveType.Lines, vertices, backgroundStyle, foregroundStyle);
        }

        public static void DrawLineLoopPrimitiveLayered(
            XyCoord[] vertices,
            LineStyle backgroundStyle,
            LineStyle foregroundStyle)
        {
            DrawPrimitiveLayered(PrimitiveType.LineLoop, vertices, backgroundStyle, foregroundStyle);
        }

        private static void DrawPrimitive(PrimitiveType primitiveType, XyCoord[] vertices)
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
                foreach (XyCoord vertex in vertices)
                {
                    GL.Vertex2(vertex.X, vertex.Y);
                }
                GL.End();
            }
        }

        private static void DrawPrimitiveLayered(
            PrimitiveType primitiveType,
            XyCoord[] vertices,
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
                foreach (XyCoord vertex in vertices)
                {
                    GL.Vertex2(vertex.X, vertex.Y);
                }
                GL.End();
                // foreground layer
                SetLineWidth(foregroundStyle.Width);
                SetColor(foregroundStyle.Color);
                GL.Begin(primitiveType);
                foreach (XyCoord vertex in vertices)
                {
                    GL.Vertex2(vertex.X, vertex.Y);
                }
                GL.End();
            }
        }

    }
}

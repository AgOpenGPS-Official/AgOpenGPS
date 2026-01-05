using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineLoopArrays(VertexBuffer vertexBuffer)
        {
            DrawArrays(PrimitiveType.LineLoop, vertexBuffer);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineStripArrays(VertexBuffer vertexBuffer)
        {
            DrawArrays(PrimitiveType.LineStrip, vertexBuffer);
        }

        private static void DrawArrays(PrimitiveType primitiveType, VertexBuffer vertexBuffer)
        {
            if (vertexBuffer == null) return;
            vertexBuffer.BindBuffer();

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Double, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.DrawArrays(primitiveType, 0, vertexBuffer.Length);

            GL.DisableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

    }
}

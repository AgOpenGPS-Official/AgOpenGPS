using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineLoopArrays(Vertex2Array array)
        {
            DrawArrays(PrimitiveType.LineLoop, array);
        }

        // Inlined by the compiler, so no function call overhead
        public static void DrawLineStripArrays(Vertex2Array array)
        {
            DrawArrays(PrimitiveType.LineStrip, array);
        }

        private static void DrawArrays(PrimitiveType primitiveType, Vertex2Array vertex2Array)
        {
            if (vertex2Array == null) return;
            vertex2Array.BindBuffer();

            GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Double, false, 0, 0);
            GL.EnableVertexAttribArray(0);

            GL.DrawArrays(primitiveType, 0, vertex2Array.Length);

            GL.DisableVertexAttribArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

    }
}

using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {
        // To optimize performance of the various DrawPrimitive functions, these functions will
        // take the number of vertices into account.
        // When the number of vertices is MinVerticesForArray or higher, a Vertex2Array is used
        // to pass the vertices as bulk.
        // For smaller numbers, the vertices are simply passed one by one.
        private const int MinVerticesForArray = 40;

        public static void BeginPointsPrimitive()
        {
            GL.Begin(PrimitiveType.Points);
        }

        public static void EndPrimitive()
        {
            GL.End();
        }

    }
}


using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {
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


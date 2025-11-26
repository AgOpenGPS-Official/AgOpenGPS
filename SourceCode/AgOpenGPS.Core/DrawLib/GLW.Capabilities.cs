using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {
        public static void EnableLineStipple()
        {
            GL.Enable(EnableCap.LineStipple);
        }

        public static void SetLineStipple(int factor, short pattern)
        {
            GL.LineStipple(factor, pattern);
        }

        public static void DisableLineStipple()
        {
            GL.Disable(EnableCap.LineStipple);
        }

    }
}

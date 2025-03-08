using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.Drawing
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static class GLW
    {
        public static void SetColor(ColorRgb color)
        {
            GL.Color3(color.Red, color.Green, color.Blue);
        }

        public static void SetColor(ColorRgba color)
        {
            GL.Color4(color.Red, color.Green, color.Blue, color.Alpha);
        }

        public static void SetLineStyle(LineStyle lineStyle)
        {
            GL.LineWidth(lineStyle.Width);
            GLW.SetColor(lineStyle.Color);
        }
    }
}

using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
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
            SetColor(lineStyle.Color);
        }

        public static void SetPointStyle(PointStyle pointStyle)
        {
            GL.PointSize(pointStyle.Size);
            SetColor(pointStyle.Color);
        }

        public static void Translate(double x, double y, double z)
        {
            GL.Translate(x, y, z);
        }

        public static void Translate(double x, double y)
        {
            GL.Translate(x, y, 0.0);
        }

        public static void RotateX(double angleInDegrees)
        {
            GL.Rotate(angleInDegrees, 1.0, 0.0, 0.0);
        }

        public static void RotateZ(double angleInDegrees)
        {
            GL.Rotate(angleInDegrees, 0.0, 0.0, 1.0);
        }

    }
}

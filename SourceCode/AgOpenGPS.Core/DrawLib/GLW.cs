using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    // GLW is short for GL Wrapper.
    // Please use this class in stead of direct calls to functions in the GL toolkit.
    public static partial class GLW
    {

        // Inlined by the compiler, so no function call overhead
        public static void SetColor(ColorRgba color)
        {
            GL.Color4(color.ByteArray);
        }

        // Inlined by the compiler, so no function call overhead
        public static void SetLineWidth(float lineWidth)
        {
            GL.LineWidth(lineWidth);
        }

        // Inlined by the compiler, so no function call overhead
        public static void SetPointSize(float pointSize)
        {
            GL.PointSize(pointSize);
        }

        public static void Translate(GeoDelta delta)
        {
            GL.Translate(delta.EastingDelta, delta.NorthingDelta, 0.0);
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

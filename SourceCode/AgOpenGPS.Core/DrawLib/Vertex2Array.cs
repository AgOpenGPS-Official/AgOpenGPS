using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    public class Vertex2Array : VertexArrayBase
    {
        public Vertex2Array(XyCoord[] vertices) : base(2)
        {
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
        }

        public Vertex2Array(GeoCoord[] vertices) : base(2)
        {
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
        }

        public Vertex2Array(GeoLineSegment[] lineSegments) : base(2)
        {
            Length = 2 * lineSegments.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, lineSegments.Length * 4 * sizeof(double), lineSegments, BufferUsageHint.StaticDraw);
        }
    }

}

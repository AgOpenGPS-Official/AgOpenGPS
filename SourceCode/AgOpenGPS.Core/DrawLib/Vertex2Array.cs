using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    public class Vertex2Array : VertexBuffer
    {
        public Vertex2Array()
        {
        }

        public Vertex2Array(XyCoord[] vertices)
        {
            SetBufferData(vertices);
        }

        public Vertex2Array(GeoCoord[] vertices)
        {
            SetBufferData(vertices);
        }

        public Vertex2Array(GeoLineSegment[] lineSegments)
        {
            SetBufferData(lineSegments);
        }

        public void SetBufferData(XyCoord[] vertices)
        {
            BindBuffer();
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void SetBufferData(GeoCoord[] vertices)
        {
            BindBuffer();
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        public void SetBufferData(GeoLineSegment[] lineSegments)
        {
            BindBuffer();
            Length = 2 * lineSegments.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, lineSegments.Length * 4 * sizeof(double), lineSegments, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

    }

}

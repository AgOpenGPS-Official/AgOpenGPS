using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS.Core.DrawLib
{
    public class Vertex2Array : VertexArrayBase
    {
        public Vertex2Array(XyCoord[] vertices) : base(2)
        {
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
        }
    }

}

using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS.Core.Drawing
{
    public abstract class VertexArrayBase
    {
        private readonly int _bufId;

        public VertexArrayBase(int nDimensions)
        {
            _bufId = GL.GenBuffer();
            Bind();
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, nDimensions, VertexAttribPointerType.Double, false, 0, 0);
        }

        public int Length { get; protected set; }

        public void Bind()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufId);
        }

        public void DeleteBuffer()
        {
            GL.DeleteBuffer(_bufId);
        }
    }

}

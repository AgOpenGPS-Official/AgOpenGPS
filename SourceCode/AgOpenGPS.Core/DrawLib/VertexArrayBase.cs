using AgOpenGPS.Core.Base;
using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    public abstract class VertexArrayBase : DisposableObject
    {
        private int _bufId;

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

        private void DeleteBuffer()
        {
            if (0 != _bufId)
            {
                GL.DeleteBuffer(_bufId);
            }
            _bufId = 0;
        }

        protected override void OnDispose()
        {
            DeleteBuffer();
        }

    }

}

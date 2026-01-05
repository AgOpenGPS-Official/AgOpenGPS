using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS.Core.DrawLib
{
    public abstract class VertexArrayBase : IDisposable
    {
        private int _bufId;
        private bool _isDisposed;

        public VertexArrayBase()
        {
            _bufId = GL.GenBuffer();
        }

        public int Length { get; protected set; }

        public void BindBuffer()
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

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                DeleteBuffer();
                _isDisposed = true;
            }
        }

        ~VertexArrayBase()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

}

using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.DrawLib;

namespace AgOpenGPS.Core.Visuals
{
    public abstract class PrimitiveRenderer
    {
        protected VertexBuffer _vertexBuffer;
        private GeoCoord[] _vertices;
        private bool _verticesInvalidated = false;

        public PrimitiveRenderer()
        {
        }

        public void UpdateVertices(GeoCoord[] vertices)
        {
            _vertices = vertices;
            _verticesInvalidated = true;
            // Delay the update of the VertexBuffer to method Draw() for two reasons:
            // - in method Draw(), the correct OpenGL context is already made current.
            // - if UpdateVertices is called more than once per frame, we don't waste
            //   time in SetBufferData for data that is never rendered.
        }

        public void Draw()
        {
            if (_verticesInvalidated)
            {
                if (_vertices != null)
                {
                    if (_vertexBuffer == null)
                    {
                        _vertexBuffer = new VertexBuffer();
                    }
                    _vertexBuffer.SetBufferData(_vertices);
                }
                else
                {
                    _vertexBuffer?.Dispose();
                    _vertexBuffer = null;
                }
                _verticesInvalidated = false;
            }
            DrawPrimitive();
        }

        public abstract void DrawPrimitive();

        public void Dispose()
        {
            _vertexBuffer?.Dispose();
        }
    }
}

using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;
using System;

namespace AgOpenGPS.Core.DrawLib
{
    public struct VertexBufferInfo
    {
        public int elementsPerVertex;
        public VertexAttribPointerType type;
        public int strideInBytes;
        public int offsetInBytes;
    }

    // The current implementation of VertexBuffer only supports one type of data at a time.
    // It does not support, for example, vertex positions in combination with vertex colors, or normals.
    // More precisely: this version only supports vertex positions.
    // Because it only supports one type of data, we only need one VertexBufferInfo to describe the
    // layout of the data in the buffer.
    // Future versions might also support mixed content, but it seems that we won't need it.
    public class VertexBuffer : IDisposable
    {
        private int _bufId;
        private bool _isDisposed;

        public VertexBuffer()
        {
            _bufId = GL.GenBuffer();
        }

        public VertexBuffer(
            XyCoord[] vertices
        )
            : this()
        {
            SetBufferData(vertices);
        }

        public VertexBuffer(
            GeoCoord[] vertices
        )
            : this()
        {
            SetBufferData(vertices);
        }

        public VertexBuffer(
            GeoLineSegment[] lineSegments
        )
            : this()
        {
            SetBufferData(lineSegments);
        }


        public int Length { get; protected set; }

        public VertexBufferInfo? VertexBufferInfo { get; private set; }

        public void BindBuffer()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufId);
        }

        public void SetBufferData(XyCoord[] vertices)
        {
            BindBuffer();
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            VertexBufferInfo = new VertexBufferInfo
            {
                elementsPerVertex = 2, // x and y
                type = VertexAttribPointerType.Double,
                strideInBytes = 2 * sizeof(double),
                offsetInBytes = 0
            };
        }

        public void SetBufferData(GeoCoord[] vertices)
        {
            BindBuffer();
            Length = vertices.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * 2 * sizeof(double), vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            VertexBufferInfo = new VertexBufferInfo
            {
                elementsPerVertex = 2, // easting and northing
                type = VertexAttribPointerType.Double,
                strideInBytes = 2 * sizeof(double),
                offsetInBytes = 0
            };
        }

        public void SetBufferData(GeoLineSegment[] lineSegments)
        {
            BindBuffer();
            Length = 2 * lineSegments.Length;
            GL.BufferData(BufferTarget.ArrayBuffer, lineSegments.Length * 4 * sizeof(double), lineSegments, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            VertexBufferInfo = new VertexBufferInfo
            {
                elementsPerVertex = 2, // easting and northing
                type = VertexAttribPointerType.Double,
                strideInBytes = 2 * sizeof(double),
                offsetInBytes = 0
            };
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

        ~VertexBuffer()
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

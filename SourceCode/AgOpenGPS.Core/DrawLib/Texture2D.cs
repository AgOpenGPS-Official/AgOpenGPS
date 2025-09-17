using System.Drawing;
using System.Drawing.Imaging;
using AgOpenGPS.Core.Base;
using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    public class Texture2D : DisposableObject
    {
        private readonly Bitmap _bitmap;
        private int _textureId;

        public Texture2D(Bitmap bitmap)
        {
            _bitmap = bitmap;
            // To avoid crashes during start-upup (when no OpenGL context has been created yet),
            // delay creation of texture to the first call to Bind() 
        }

        public void Bind()
        {
            if (0 == _textureId) CreateTexture();
            GL.BindTexture(TextureTarget.Texture2D, _textureId);
        }

        public void Draw(
            XyCoord u0v0, // The corner (u==0.0 && v==0.0) of the texture will be mapped to this coord
            XyCoord u1v1  // The corner (u==1.0 && v==1.0) of the texture will be mapped to this coord
        )
        {
            GL.Enable(EnableCap.Texture2D);

            Bind();
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex2(u0v0.X, u0v0.Y);
            GL.TexCoord2(1, 0); GL.Vertex2(u1v1.X, u0v0.Y);
            GL.TexCoord2(1, 1); GL.Vertex2(u1v1.X, u1v1.Y);
            GL.TexCoord2(0, 1); GL.Vertex2(u0v0.X, u1v1.Y);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }

        public void DrawCenteredAroundOrigin(
            XyDelta centerToU1V1)
        {
            XyCoord origin = new XyCoord(0.0, 0.0);
            Draw(origin - centerToU1V1, origin + centerToU1V1);
        }

        public void DrawCentered(
            XyCoord center,      // The center of the texture will be mapped to this coord
            XyDelta centerToU1V1 // Typically 0.5 * the size. Negative values for X and/or Y
                                 // will flip the corresponding U and/or V axis of the texture.
        )
        {
            Draw(center - centerToU1V1, center + centerToU1V1);
        }

        public void SetBitmap(Bitmap bitmap)
        {
            Bind();
            BitmapData bitmapData = bitmap.LockBits(
                new Rectangle(0, 0, bitmap.Width, bitmap.Height),
                ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(
                TextureTarget.Texture2D,
                0,
                PixelInternalFormat.Rgba,
                bitmapData.Width,
                bitmapData.Height,
                0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
                PixelType.UnsignedByte,
                bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
            GL.BindTexture(TextureTarget.Texture2D, 0);
        }

        private void CreateTexture()
        {
            _textureId = GL.GenTexture();
            if (_bitmap != null) SetBitmap(_bitmap);
        }

        private void DeleteTexture()
        {
            if (0 != _textureId)
            {
                GL.DeleteTexture(_textureId);
            }
            _textureId = 0;
        }

        protected override void OnDispose()
        {
            DeleteTexture();
        }

    }
}

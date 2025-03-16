using System.Drawing;
using AgOpenGPS.Core.Models;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS.Core.DrawLib
{
    public class GeoTexture2D : Texture2D
    {
        public GeoTexture2D(Bitmap bitmap) : base(bitmap)
        {
        }

        public void DrawZ(
            GeoCoord u0v0, // The corner (u==0.0 && v==0.0) of the texture will be mapped to this coord
            GeoCoord u1v1, // The corner (u==1.0 && v==1.0) of the texture will be mapped to this coord
            double zCoord
        )
        {
            GL.Enable(EnableCap.Texture2D);

            Bind();
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex3(u0v0.Northing, u0v0.Easting, zCoord);
            GL.TexCoord2(1, 0); GL.Vertex3(u1v1.Northing, u0v0.Easting, zCoord);
            GL.TexCoord2(1, 1); GL.Vertex3(u1v1.Northing, u1v1.Easting, zCoord);
            GL.TexCoord2(0, 1); GL.Vertex3(u0v0.Northing, u1v1.Easting, zCoord);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }

        public void DrawRepeatedZ(
            GeoCoord u0v0, // The corner (u==0.0 && v==0.0) of the texture will be mapped to this coord
            GeoCoord u1v1, // The corner (u==1.0 && v==1.0) of the texture will be mapped to this coord
            double zCoord,
            double nRepeat)
        {
            GL.Enable(EnableCap.Texture2D);

            Bind();
            GL.Begin(PrimitiveType.Quads);
            GL.TexCoord2(0, 0); GL.Vertex3(u0v0.Northing, u0v0.Easting, zCoord);
            GL.TexCoord2(nRepeat, 0); GL.Vertex3(u1v1.Northing, u0v0.Easting, zCoord);
            GL.TexCoord2(nRepeat, nRepeat); GL.Vertex3(u1v1.Northing, u1v1.Easting, zCoord);
            GL.TexCoord2(0, nRepeat); GL.Vertex3(u0v0.Northing, u1v1.Easting, zCoord);
            GL.End();
            GL.Disable(EnableCap.Texture2D);
        }

    }
}

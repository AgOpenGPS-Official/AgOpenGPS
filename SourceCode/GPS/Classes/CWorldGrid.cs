//Please, if you use this, share the improvements

using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Properties;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AgOpenGPS
{
    public class CWorldGrid
    {
        private readonly FormGPS mf;

        //Y
        public double northingMax;

        public double northingMin;

        //X
        public double eastingMax;

        public double eastingMin;

        //Y
        public double northingMaxGeo;

        public double northingMinGeo;

        //X
        public double eastingMaxGeo;

        public double eastingMinGeo;

        //Y
        public double northingMaxRate;

        public double northingMinRate;

        //X
        public double eastingMaxRate;

        public double eastingMinRate;

        public double GridSize = 6000;
        public double Count = 40;
        public bool isGeoMap = false;

        public double gridRotation = 0.0;

        private GeoTexture2D _floorTexture;
        private GeoTexture2D _bingGridTexture;

        public CWorldGrid(FormGPS _f)
        {
            mf = _f;

            northingMaxGeo = 300;
            northingMinGeo = -300;
            eastingMaxGeo = 300;
            eastingMinGeo = -300;
            northingMaxRate = 300;
            northingMinRate = -300;
            eastingMaxRate = 300;
            eastingMinRate = -300;
        }

        public GeoTexture2D FloorTexture
        {
            get
            {
                if (_floorTexture == null) _floorTexture = new GeoTexture2D(Resources.z_Floor);
                return _floorTexture;
            }
        }

        public GeoTexture2D BingGridTexture
        {
            get
            {
                if (_bingGridTexture == null) _bingGridTexture = new GeoTexture2D(null);
                return _bingGridTexture;
            }
        }

        public void ResetBingGridTexture()
        {
            Bitmap bitmap = Properties.Resources.z_bingMap;
            BingGridTexture.SetBitmap(bitmap);
        }

        public void DrawFieldSurface()
        {
            Color field = mf.isDay ? mf.fieldColorDay : mf.fieldColorNight;

            //adjust bitmap zoom based on cam zoom
            if (mf.camera.zoomValue > 100) Count = 4;
            else if (mf.camera.zoomValue > 80) Count = 8;
            else if (mf.camera.zoomValue > 50) Count = 16;
            else if (mf.camera.zoomValue > 20) Count = 32;
            else if (mf.camera.zoomValue > 10) Count = 64;
            else Count = 80;

            GL.Color3(field.R, field.G, field.B);
            if (mf.isTextureOn)
            {
                GeoCoord u0v0 = new GeoCoord(eastingMin, northingMax);
                GeoCoord uCountvCount = new GeoCoord(eastingMax, northingMin);
                FloorTexture.DrawRepeatedZ(u0v0, uCountvCount, -0.10, Count);
                if (isGeoMap)
                {
                    GeoCoord u0v0Map = new GeoCoord(eastingMinGeo, northingMaxGeo);
                    GeoCoord u1v1Map = new GeoCoord(eastingMaxGeo, northingMinGeo);
                    BingGridTexture.DrawZ(u0v0Map, u1v1Map, -0.05);
                }
            }
        }

        public void DrawWorldGrid(double _gridZoom)
        {
            //_gridZoom *= 0.5;

            GL.Rotate(-gridRotation, 0, 0, 1.0);

            if (mf.isDay)
            {
                GL.Color3(0.25, 0.25, 0.25);
            }
            else
            {
                GL.Color3(0.12, 0.12, 0.12);
            }
            GL.LineWidth(1);
            GL.Begin(PrimitiveType.Lines);
            for (double num = Math.Round(eastingMin / _gridZoom, MidpointRounding.AwayFromZero) * _gridZoom; num < eastingMax; num += _gridZoom)
            {
                if (num < eastingMin) continue;

                GL.Vertex3(num, northingMax, 0.1);
                GL.Vertex3(num, northingMin, 0.1);
            }
            for (double num2 = Math.Round(northingMin / _gridZoom, MidpointRounding.AwayFromZero) * _gridZoom; num2 < northingMax; num2 += _gridZoom)
            {
                if (num2 < northingMin) continue;

                GL.Vertex3(eastingMax, num2, 0.1);
                GL.Vertex3(eastingMin, num2, 0.1);
            }
            GL.End();

            GL.Rotate(gridRotation, 0, 0, 1.0);

        }

        public void checkZoomWorldGrid(GeoCoord geoCoord)
        {
            double n = Math.Round(geoCoord.Northing / (GridSize / Count * 2), MidpointRounding.AwayFromZero) * (GridSize / Count * 2);
            double e = Math.Round(geoCoord.Easting / (GridSize / Count * 2), MidpointRounding.AwayFromZero) * (GridSize / Count * 2);

            northingMax = n + GridSize;
            northingMin = n - GridSize;
            eastingMax = e + GridSize;
            eastingMin = e - GridSize;
        }
    }
}
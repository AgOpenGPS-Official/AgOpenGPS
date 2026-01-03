//Please, if you use this, share the improvements

using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Visuals;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace AgOpenGPS.Core
{
    public class WorldGrid
    {
        private readonly FieldGridVisual _fieldGridVisual;
        private BingMap _bingMap;
        private BingMapVisual _bingMapVisual;
        private Bitmap _floorBitmap;
        private GeoTexture2D _floorTexture;

        //Y
        private double northingMax;

        private double northingMin;

        //X
        private double eastingMax;

        private double eastingMin;

        private double GridSize = 6000;
        private double Count = 40;

        public WorldGrid(Bitmap floorBitmap)
        {
            _floorBitmap = floorBitmap;
            FieldGrid = new FieldGrid();
            _fieldGridVisual = new FieldGridVisual(FieldGrid);
        }

        public FieldGrid FieldGrid { get; }

        public BingMap BingMap
        {
            private get
            {
                return _bingMap;
            }
            set
            {
                _bingMap = value;
                _bingMapVisual = (_bingMap != null) ? new BingMapVisual(_bingMap) : null;
            }
        }

        public bool HasBingMap => BingMap != null;

        private GeoTexture2D FloorTexture
        {
            get
            {
                if (null == _floorTexture) _floorTexture = new GeoTexture2D(_floorBitmap);
                return _floorTexture;
            }
        }

        public void DrawFieldSurface(ColorRgba fieldColor, double cameraZoom, bool mustDrawFieldTexture)
        {
            //adjust bitmap zoom based on cam zoom
            if (cameraZoom > 100) Count = 4;
            else if (cameraZoom > 80) Count = 8;
            else if (cameraZoom > 50) Count = 16;
            else if (cameraZoom > 20) Count = 32;
            else if (cameraZoom > 10) Count = 64;
            else Count = 80;

            GLW.SetColor(fieldColor);
            GL.Begin(PrimitiveType.TriangleStrip);
            GL.TexCoord2(0, 0);
            GL.Vertex3(eastingMin, northingMax, -0.10);
            GL.TexCoord2(Count, 0.0);
            GL.Vertex3(eastingMax, northingMax, -0.10);
            GL.TexCoord2(0.0, Count);
            GL.Vertex3(eastingMin, northingMin, -0.10);
            GL.TexCoord2(Count, Count);
            GL.Vertex3(eastingMax, northingMin, -0.10);
            GL.End();

            if (mustDrawFieldTexture)
            {
                GeoCoord u0v0 = new GeoCoord(eastingMin, northingMax);
                GeoCoord uCountvCount = new GeoCoord(eastingMax, northingMin);
                FloorTexture.DrawRepeatedZ(u0v0, uCountvCount, -0.10, Count);
            }
            _bingMapVisual?.Draw();
        }

        public void DrawFieldGrid(bool isDay, GeoBoundingBox fieldBoundingBox)
        {
            _fieldGridVisual.Draw(isDay, fieldBoundingBox);
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
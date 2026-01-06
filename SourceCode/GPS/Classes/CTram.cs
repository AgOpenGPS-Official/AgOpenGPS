using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Visuals;
using AgOpenGPS.Helpers;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AgOpenGPS
{
    public class CTram
    {
        private readonly FormGPS mf;
        private readonly TramLinesVisual _tramLinesVisual;

        public List<vec2> tramBndOuterArr = new List<vec2>();
        public List<vec2> tramBndInnerArr = new List<vec2>();

        //tram settings
        //public double wheelTrack;
        public double tramWidth;

        public double halfWheelTrack, alpha;
        public int passes;
        public bool isOuter;

        public bool isLeftManualOn, isRightManualOn;

        //tramlines
        public List<vec2> tramArr = new List<vec2>();

        public List<List<vec2>> tramList = new List<List<vec2>>();

        public TramMode displayMode;
        public TramMode generateMode = TramMode.All;

        internal int controlByte;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;
            _tramLinesVisual = new TramLinesVisual();

            tramWidth = Properties.Settings.Default.setTram_tramWidth;
            //halfTramWidth = (Math.Round((Properties.Settings.Default.setTram_tramWidth) / 2.0, 3));

            halfWheelTrack = Properties.Settings.Default.setVehicle_trackWidth * 0.5;

            IsTramOuterOrInner();

            passes = Properties.Settings.Default.setTram_passes;
            displayMode = 0;

            alpha = Properties.Settings.Default.setTram_alpha;
        }


        public static Bitmap GetModeBitmap(TramMode mode)
        {
            Bitmap modeBitMap;
            switch (mode)
            {
                case TramMode.None:
                    modeBitMap = Properties.Resources.TramOff;
                    break;
                case TramMode.All:
                    modeBitMap = Properties.Resources.TramAll;
                    break;
                case TramMode.FillTracks:
                    modeBitMap = Properties.Resources.TramLines;
                    break;
                case TramMode.BoundaryTracks:
                    modeBitMap = Properties.Resources.TramOuter;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), "TramMode argument out of range");
            }
            return modeBitMap;
        }


        public void IsTramOuterOrInner()
        {
            isOuter = ((int)(tramWidth / mf.tool.width + 0.5)) % 2 == 0;
            if (Properties.Settings.Default.setTool_isTramOuterInverted) isOuter = !isOuter;
        }

        public void DrawTram()
        {
            LineStyle backgroundLineStyle = new LineStyle(
                mf.camera.camSetDistance > -500 ? 10 : 6,
                new ColorRgba(0.0f, 0.0f, 0.0f, (float)alpha));

            LineStyle foregroundLineStyle = new LineStyle(
                mf.camera.camSetDistance > -500 ? 4 : 2,
                new ColorRgba(0.930f, 0.72f, 0.735f, (float)alpha));

            _tramLinesVisual.DrawTramLinesLayered(
                displayMode.IncludesFillTracks(),
                displayMode.IncludesBoundaryTracks(),
                backgroundLineStyle,
                foregroundLineStyle);
        }

        public void BuildBoundaryTracks()
        {
            bool isBndExist = mf.bnd.bndList.Count != 0;

            if (generateMode.IncludesBoundaryTracks() && isBndExist)
            {
                tramBndOuterArr = CreateBoundaryTrack(0.5 * tramWidth - halfWheelTrack);
                tramBndInnerArr = CreateBoundaryTrack(0.5 * tramWidth + halfWheelTrack);
                _tramLinesVisual.UpdateBoundaryTracks(
                    GeoRefactorHelper.ToGeoCoordArray(tramBndOuterArr),
                    GeoRefactorHelper.ToGeoCoordArray(tramBndInnerArr));
            }
            else
            {
                ClearBoundaryTracks();
            }
        }

        public void ClearBoundaryTracks()
        {
            tramBndOuterArr?.Clear();
            tramBndInnerArr?.Clear();
            _tramLinesVisual.UpdateBoundaryTracks(null, null);
        }

        public void UpdateFillTracks(List<GeoCoord[]> fillTracks)
        {
            _tramLinesVisual.UpdateFillTracks(fillTracks);
        }

        private List<vec2> CreateBoundaryTrack(double distance)
        {
            List<vec2> newTrack = new List<vec2>();

            //countExit the points from the boundary
            int ptCount = mf.bnd.bndList[0].fenceLine.Count;

            //outside point
            vec2 pt3 = new vec2();

            double distSq = distance * distance * 0.999;

            for (int i = 0; i < ptCount; i++)
            {
                //calculate the point inside the boundary
                pt3.easting = mf.bnd.bndList[0].fenceLine[i].easting -
                    (Math.Sin(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * distance);

                pt3.northing = mf.bnd.bndList[0].fenceLine[i].northing -
                    (Math.Cos(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * distance);

                bool Add = true;

                for (int j = 0; j < ptCount; j++)
                {
                    double check = glm.DistanceSquared(pt3.northing, pt3.easting,
                                        mf.bnd.bndList[0].fenceLine[j].northing, mf.bnd.bndList[0].fenceLine[j].easting);
                    if (check < distSq)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add)
                {
                    if (newTrack.Count > 0)
                    {
                        double dist = ((pt3.easting - newTrack[newTrack.Count - 1].easting) * (pt3.easting - newTrack[newTrack.Count - 1].easting))
                            + ((pt3.northing - newTrack[newTrack.Count - 1].northing) * (pt3.northing - newTrack[newTrack.Count - 1].northing));
                        if (dist > 2)
                            newTrack.Add(pt3);
                    }
                    else newTrack.Add(pt3);
                }
            }
            return newTrack;
        }

    }
}
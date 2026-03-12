using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CTram
    {
        private readonly FormGPS mf;

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

        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode, generateMode = 0;

        internal int controlByte;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            tramWidth = Properties.Settings.Default.setTram_tramWidth;
            //halfTramWidth = (Math.Round((Properties.Settings.Default.setTram_tramWidth) / 2.0, 3));

            halfWheelTrack = Properties.VehicleSettings.Default.setVehicle_trackWidth * 0.5;

            IsTramOuterOrInner();

            passes = Properties.Settings.Default.setTram_passes;
            displayMode = 0;

            alpha = Properties.Settings.Default.setTram_alpha;
        }

        public void IsTramOuterOrInner()
        {
            isOuter = ((int)(tramWidth / mf.tool.width + 0.5)) % 2 == 0;
            if (Properties.ToolSettings.Default.setTool_isTramOuterInverted) isOuter = !isOuter;
        }

        public void DrawTram()
        {
            if (mf.camera.camSetDistance > -500) GL.LineWidth(10);
            else GL.LineWidth(6);

            GL.Color4(0, 0, 0, alpha);

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 2)
            {
                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < tramList[i].Count; h++)
                            GL.Vertex3(tramList[i][h].easting, tramList[i][h].northing, 0);
                        GL.End();
                    }
                }
            }

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 3)
            {
                if (tramBndOuterArr.Count > 0)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < tramBndOuterArr.Count; h++) GL.Vertex3(tramBndOuterArr[h].easting, tramBndOuterArr[h].northing, 0);
                    GL.End();
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < tramBndInnerArr.Count; h++) GL.Vertex3(tramBndInnerArr[h].easting, tramBndInnerArr[h].northing, 0);
                    GL.End();
                }
            }

            if (mf.camera.camSetDistance > -500) GL.LineWidth(4);
            else GL.LineWidth(2);

            GL.Color4(0.930f, 0.72f, 0.73530f, alpha);

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 2)
            {
                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < tramList[i].Count; h++)
                            GL.Vertex3(tramList[i][h].easting, tramList[i][h].northing, 0);
                        GL.End();
                    }
                }
            }

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 3)
            {
                if (tramBndOuterArr.Count > 0)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < tramBndOuterArr.Count; h++) GL.Vertex3(tramBndOuterArr[h].easting, tramBndOuterArr[h].northing, 0);
                    GL.End();
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < tramBndInnerArr.Count; h++) GL.Vertex3(tramBndInnerArr[h].easting, tramBndInnerArr[h].northing, 0);
                    GL.End();
                }
            }
        }

        public void BuildTramBnd()
        {
            bool isBndExist = mf.bnd.bndList.Count != 0;

            if (isBndExist)
            {
                CreateBoundaryOuterTrack();
                CreateBoundaryInnerTrack();
            }
            else
            {
                tramBndOuterArr?.Clear();
                tramBndInnerArr?.Clear();
            }
        }

        public void CreateBoundaryOuterTrack()
        {
            tramBndOuterArr = CreateBoundaryTrack(0.5 * tramWidth - halfWheelTrack);
            tramBndOuterArr.ReducePointsByAngle(0.01, 50);
        }

        public void CreateBoundaryInnerTrack()
        {
            tramBndInnerArr = CreateBoundaryTrack(0.5 * tramWidth + halfWheelTrack);
            tramBndInnerArr.ReducePointsByAngle(0.01, 50);
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
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CTram
    {
        private readonly FormGPS mf;

        //the list of constants and multiples of the boundary
        public List<vec2> calcList = new List<vec2>();

        //the triangle strip of the outer tram highlight
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

        internal byte controlByte;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            tramWidth = Properties.Settings.Default.setTram_tramWidth;
            //halfTramWidth = (Math.Round((Properties.Settings.Default.setTram_tramWidth) / 2.0, 3));

            halfWheelTrack = Properties.Settings.Default.setVehicle_trackWidth * 0.5;

            IsTramOuterOrInner();

            passes = Properties.Settings.Default.setTram_passes;
            displayMode = 0;

            alpha = Properties.Settings.Default.setTram_alpha;
        }

        public void IsTramOuterOrInner()
        {
            isOuter = ((int)(tramWidth / mf.tool.width + 0.5)) % 2 == 0;
            if (Properties.Settings.Default.setTool_isTramOuterInverted) isOuter = !isOuter;
        }

        public void DrawTram()
        {
            if (mf.camera.camSetDistance > -500)  GL.LineWidth(10);
            else GL.LineWidth(6);

            GL.Color4(0,0,0, alpha);

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
                CreateBndOuterTramTrack();
                CreateBndInnerTramTrack();
            }
            else
            {
                tramBndOuterArr?.Clear();
                tramBndInnerArr?.Clear();
            }
        }

        private void CreateBndInnerTramTrack()
        {
            //countExit the points from the boundary
            int ptCount = mf.bnd.bndList[0].fenceLine.Count;
            tramBndInnerArr?.Clear();

            //outside point
            vec2 pt3 = new vec2();

            double distSq = ((tramWidth * 0.5) + halfWheelTrack) * ((tramWidth * 0.5) + halfWheelTrack) * 0.999;

            //make the boundary tram outer array
            for (int i = 0; i < ptCount; i++)
            {
                //calculate the point inside the boundary
                pt3.easting = mf.bnd.bndList[0].fenceLine[i].easting -
                    (Math.Sin(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (tramWidth * 0.5 + halfWheelTrack));

                pt3.northing = mf.bnd.bndList[0].fenceLine[i].northing -
                    (Math.Cos(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (tramWidth * 0.5 + halfWheelTrack));

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
                    if (tramBndInnerArr.Count > 0)
                    {
                        double dist = ((pt3.easting - tramBndInnerArr[tramBndInnerArr.Count - 1].easting) * (pt3.easting - tramBndInnerArr[tramBndInnerArr.Count - 1].easting))
                            + ((pt3.northing - tramBndInnerArr[tramBndInnerArr.Count - 1].northing) * (pt3.northing - tramBndInnerArr[tramBndInnerArr.Count - 1].northing));
                        if (dist > 2)
                            tramBndInnerArr.Add(pt3);
                    }
                    else tramBndInnerArr.Add(pt3);
                }
            }
        }

        public void CreateBndOuterTramTrack()
        {
            //countExit the points from the boundary
            int ptCount = mf.bnd.bndList[0].fenceLine.Count;
            tramBndOuterArr?.Clear();

            //outside point
            vec2 pt3 = new vec2();

            double distSq = ((tramWidth * 0.5) - halfWheelTrack) * ((tramWidth * 0.5) - halfWheelTrack) * 0.999;

            //make the boundary tram outer array
            for (int i = 0; i < ptCount; i++)
            {
                //calculate the point inside the boundary
                pt3.easting = mf.bnd.bndList[0].fenceLine[i].easting -
                    (Math.Sin(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (tramWidth * 0.5 - halfWheelTrack));

                pt3.northing = mf.bnd.bndList[0].fenceLine[i].northing -
                    (Math.Cos(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (tramWidth * 0.5 - halfWheelTrack));

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
                    if (tramBndOuterArr.Count > 0)
                    {
                        double dist = ((pt3.easting - tramBndOuterArr[tramBndOuterArr.Count - 1].easting) * (pt3.easting - tramBndOuterArr[tramBndOuterArr.Count - 1].easting))
                            + ((pt3.northing - tramBndOuterArr[tramBndOuterArr.Count - 1].northing) * (pt3.northing - tramBndOuterArr[tramBndOuterArr.Count - 1].northing));
                        if (dist > 2)
                            tramBndOuterArr.Add(pt3);
                    }
                    else tramBndOuterArr.Add(pt3);
                }
            }
        }
    }
}
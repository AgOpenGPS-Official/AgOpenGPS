using AgOpenGPS.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormGrid : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private Point fixPt;

        private bool isA = true;
        private int start = 99999, end = 99999;
        private int bndSelect = 0, originalLine;

        private double zoom = 1, sX = 0, sY = 0;

        public vec3 pint = new vec3(0.0, 1.0, 0.0);
        public vec3 pntA = new vec3(0.0, 1.0, 0.0);
        public vec3 pntB = new vec3(0.0, 1.0, 0.0);

        private bool isDrawSections = true;

        public FormGrid(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            mf.CalculateMinMax();
        }

        private void FormABDraw_Load(object sender, EventArgs e)
        {
            originalLine = mf.trk.idx;

            Size = Properties.Settings.Default.setWindow_gridSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            Location = Properties.Settings.Default.setWindow_gridLocation;
            FormABDraw_ResizeEnd(this, e);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormABDraw_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.curve.isCurveValid = false;
            mf.ABLine.isABValid = false;

            mf.twoSecondCounter = 100;

            Properties.Settings.Default.setWindow_gridSize = Size;
            Properties.Settings.Default.setWindow_gridLocation = Location;
            Properties.Settings.Default.Save();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.worldGrid.gridRotation = 0;
            Close();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            //update the arrays
            start = 99999; end = 99999;
            isA = true;

            mf.curve.desList?.Clear();

            zoom = 1;
            sX = 0;
            sY = 0;

            btnExit.Focus();
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            Point pt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            //if (cboxIsZoom.Checked && !zoomToggle)
            //{
            //    sX = (( halfWid - (double)pt.X) / wid)*1.1;
            //    sY = ((halfWid - (double)pt.Y) / -wid)*1.1;
            //    zoom = 0.1;
            //    return;
            //}

            //Convert to Origin in the center of window, 800 pixels
            fixPt.X = pt.X - halfWid;
            fixPt.Y = (wid - pt.Y - halfWid);
            vec3 plotPt = new vec3
            {
                //convert screen coordinates to field coordinates
                easting = fixPt.X * mf.maxFieldDistance / scale * zoom,
                northing = fixPt.Y * mf.maxFieldDistance / scale * zoom,
                heading = 0
            };

            plotPt.easting += mf.fieldCenterX + mf.maxFieldDistance * -sX;
            plotPt.northing += mf.fieldCenterY + mf.maxFieldDistance * -sY;

            pint.easting = plotPt.easting;
            pint.northing = plotPt.northing;

            zoom = 1;
            sX = 0;
            sY = 0;

            if (isA)
            {
                start = 99999; end = 99999;
                start = 1;
                pntA = new vec3(plotPt);
                isA = false;
            }
            else
            {
                isA = true;
                pntB = new vec3(plotPt);
                end = 1;

                mf.worldGrid.gridRotation =
            Math.Atan2(
                pntB.easting - pntA.easting,
                pntB.northing - pntA.northing);
                if (mf.worldGrid.gridRotation < 0) mf.worldGrid.gridRotation += glm.twoPI;

                mf.worldGrid.gridRotation = glm.toDegrees(mf.worldGrid.gridRotation);
            }

            oglSelf.Refresh();
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -mf.maxFieldDistance * zoom);

            //translate to that spot in the world
            GL.Translate(-mf.fieldCenterX + sX * mf.maxFieldDistance, -mf.fieldCenterY + sY * mf.maxFieldDistance, 0);

            if (isDrawSections) DrawSections();

            GL.LineWidth(3);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == bndSelect)
                    GL.Color3(1.0f, 1.0f, 1.0f);
                else
                    GL.Color3(0.62f, 0.635f, 0.635f);

                GL.Begin(PrimitiveType.LineLoop);
                for (int i = 0; i < mf.bnd.bndList[j].fenceLineEar.Count; i++)
                {
                    GL.Vertex3(mf.bnd.bndList[j].fenceLineEar[i].easting, mf.bnd.bndList[j].fenceLineEar[i].northing, 0);
                }
                GL.End();
            }

            //the vehicle
            GL.PointSize(16.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(1.0f, 0.00f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            GL.PointSize(8.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.00f, 0.0f, 0.0f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();


            //draw the line building graphics
            if (start != 99999 || end != 99999) DrawABTouchPoints();

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawABTouchPoints()
        {
            GL.Color3(0.65, 0.650, 0.0);
            GL.PointSize(24);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0, 0, 0);
            if (start != 99999) GL.Vertex3(pntA.easting, pntA.northing, 0);
            if (end != 99999) GL.Vertex3(pntB.easting, pntB.northing, 0);
            GL.End();

            GL.PointSize(16);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(1.0f, 0.75f, 0.350f);
            if (start != 99999) GL.Vertex3(pntA.easting, pntA.northing, 0);

            GL.Color3(0.5f, 0.5f, 1.0f);
            if (end != 99999) GL.Vertex3(pntB.easting, pntB.northing, 0);
            GL.End();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
        }

        private void btnAlignToTrack_Click(object sender, EventArgs e)
        {
            if (mf.trk.idx > -1)
            {
                mf.worldGrid.gridRotation = Math.Atan2(
                    mf.trk.gArr[mf.trk.idx].ptB.easting - mf.trk.gArr[mf.trk.idx].ptA.easting,
                    mf.trk.gArr[mf.trk.idx].ptB.northing - mf.trk.gArr[mf.trk.idx].ptA.northing);
                if (mf.worldGrid.gridRotation < 0) mf.worldGrid.gridRotation += glm.twoPI;
                mf.worldGrid.gridRotation = glm.toDegrees(mf.worldGrid.gridRotation);
            }
            Close();
        }

        private void FormABDraw_ResizeEnd(object sender, EventArgs e)
        {
            Width = (int)((double)Height * 1.09);

            oglSelf.Height = oglSelf.Width = Height - 40;

            oglSelf.Left = 1;
            oglSelf.Top = 0;

            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            GL.Viewport(0, 0, oglSelf.Width, oglSelf.Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);

            tlp1.Width = Width - oglSelf.Width - 10;
            tlp1.Left = oglSelf.Width - 2;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            //this.Top = (area.Height - this.Height) / 2;
            //this.Left = (area.Width - this.Width) / 2;
        }

        private void oglSelf_Resize(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            GL.Viewport(0, 0, oglSelf.Width, oglSelf.Height);

            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        private void DrawSections()
        {
            int cnt, step, patchCount;
            int mipmap = 8;

            GL.Color3(0.9f, 0.9f, 0.8f);

            //draw patches j= # of sections
            for (int j = 0; j < mf.triStrip.Count; j++)
            {
                //every time the section turns off and on is a new patch
                patchCount = mf.triStrip[j].patchList.Count;

                if (patchCount > 0)
                {
                    //for every new chunk of patch
                    foreach (System.Collections.Generic.List<vec3> triList in mf.triStrip[j].patchList)
                    {
                        //draw the triangle in each triangle strip
                        GL.Begin(PrimitiveType.TriangleStrip);
                        cnt = triList.Count;

                        //if large enough patch and camera zoomed out, fake mipmap the patches, skip triangles
                        if (cnt >= (mipmap))
                        {
                            step = mipmap;
                            for (int i = 1; i < cnt; i += step)
                            {
                                GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;
                                GL.Vertex3(triList[i].easting, triList[i].northing, 0); i++;

                                //too small to mipmap it
                                if (cnt - i <= (mipmap + 2))
                                    step = 0;
                            }
                        }
                        else { for (int i = 1; i < cnt; i++) GL.Vertex3(triList[i].easting, triList[i].northing, 0); }
                        GL.End();
                    }
                }
            } //end of section patches
        }
    }
}
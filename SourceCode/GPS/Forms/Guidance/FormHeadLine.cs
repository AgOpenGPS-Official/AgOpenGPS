using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormHeadLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private Point fixPt;

        private bool isA = true;
        private int start = 99999, end = 99999;
        private int bndSelect = 0;
        TrackMode mode = TrackMode.None;
        public List<vec3> sliceArr = new List<vec3>();
        public List<vec3> backupList = new List<vec3>();

        private bool zoomToggle;

        private double zoom = 1, sX = 0, sY = 0;

        public vec3 pint = new vec3(0.0, 1.0, 0.0);

        public FormHeadLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            mf.CalculateMinMax();
        }

        private void FormHeadLine_Load(object sender, EventArgs e)
        {
            this.Text = "1: Set distance, 2: Tap Build, 3: Create Clip Lines";
            mf.hdl.idx = -1;
            //label3.Text = mf.unitsFtM +"       Tool: ";

            lblToolWidth.Text = "( " + mf.unitsFtM + " )           Tool: " + ((mf.tool.width - mf.tool.overlap) * mf.m2FtOrM).ToString("N1") + " " + mf.unitsFtM;

            start = 99999; end = 99999;
            isA = true;
            mf.hdl.desList?.Clear();
            sliceArr?.Clear();
            backupList?.Clear();

            btnClipLine.Enabled = false;

            if (mf.bnd.bndList[0].hdLine.Count == 0)
            {
                mf.bnd.bndList[0].hdLine?.Clear();

                if (mf.bnd.bndList[0].fenceLine.Count > 0)
                {
                    for (int i = 0; i < mf.bnd.bndList[0].fenceLine.Count; i++)
                    {
                        mf.bnd.bndList[0].hdLine.Add(new vec3(mf.bnd.bndList[0].fenceLine[i]));
                    }
                }
            }
            else
            {
                //make sure point distance isn't too big 
                mf.curve.MakePointMinimumSpacing(ref mf.bnd.bndList[0].hdLine, 1.2);
                mf.curve.CalculateHeadings(ref mf.bnd.bndList[0].hdLine);
            }

            cboxIsSectionControlled.Checked = Properties.Settings.Default.setHeadland_isSectionControlled;
            if (cboxIsSectionControlled.Checked) cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOn;
            else cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOff;

            checkBoxZoomIn.Checked = false;

            Size = Properties.Settings.Default.setWindow_HeadlineSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormHeadLine_ResizeEnd(this, e);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
            //translate
            this.Text = gStr.gsHeadlandForm;
            btnBndLoop.Text = gStr.gsBuildAround;
            btnDeletePoints.Text = gStr.gsReset;
            btnClipLine.Text = gStr.gsClipLine;
            checkBoxZoomIn.Text = gStr.gsZoomIn;
        }

        private void FormHeadLine_ResizeEnd(object sender, EventArgs e)
        {
            Width = (Height * 4 / 3);

            oglSelf.Height = oglSelf.Width = Height - 50;

            oglSelf.Left = 2;
            oglSelf.Top = 2;

            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            GL.Viewport(0, 0, oglSelf.Width, oglSelf.Height);
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);

            tlp1.Width = Width - oglSelf.Width - 4;
            tlp1.Left = oglSelf.Width;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;

        }

        private void FormHeadLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sliceArr.Count > 0)
            {
                mf.hdl.idx = 0;
            }
            else mf.hdl.idx = -1;

            Properties.Settings.Default.setWindow_HeadlineSize = Size;
            Properties.Settings.Default.Save();

        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {

            if (nudSetDistance.Value == 0 && rbtnCurve.Checked)
            {
                mf.TimedMessageBox(3000, "Distance Error", "Distance Set to 0, Nothing to Move");
                Log.EventWriter("Headland, Distance=0, Can't Move");
                return;
            }
            sliceArr?.Clear();

            Point ptt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            if (checkBoxZoomIn.Checked && !zoomToggle)
            {
                sX = ((halfWid - (double)ptt.X) / wid) * 1.1;
                sY = ((halfWid - (double)ptt.Y) / -wid) * 1.1;
                zoom = 0.1;
                zoomToggle = true;
                return;
            }

            //Convert to Origin in the center of window, 800 pixels
            fixPt.X = ptt.X - halfWid;
            fixPt.Y = (wid - ptt.Y - halfWid);
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

            zoomToggle = false;
            zoom = 1;
            sX = 0;
            sY = 0;

            if (isA)
            {
                double minDistA = double.MaxValue;
                start = 99999; end = 99999;

                for (int j = 0; j < mf.bnd.bndList.Count; j++)
                {
                    for (int i = 0; i < mf.bnd.bndList[j].fenceLine.Count; i++)
                    {
                        double dist = ((pint.easting - mf.bnd.bndList[j].fenceLine[i].easting) * (pint.easting - mf.bnd.bndList[j].fenceLine[i].easting))
                                        + ((pint.northing - mf.bnd.bndList[j].fenceLine[i].northing) * (pint.northing - mf.bnd.bndList[j].fenceLine[i].northing));
                        if (dist < minDistA)
                        {
                            minDistA = dist;
                            bndSelect = j;
                            start = i;
                        }
                    }
                }

                isA = false;
            }
            else
            {
                double minDistA = double.MaxValue;
                int j = bndSelect;

                for (int i = 0; i < mf.bnd.bndList[j].fenceLine.Count; i++)
                {
                    double dist = ((pint.easting - mf.bnd.bndList[j].fenceLine[i].easting) * (pint.easting - mf.bnd.bndList[j].fenceLine[i].easting))
                                    + ((pint.northing - mf.bnd.bndList[j].fenceLine[i].northing) * (pint.northing - mf.bnd.bndList[j].fenceLine[i].northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        end = i;
                    }
                }

                isA = true;

                //build the lines
                if (rbtnCurve.Checked)
                {
                    bool isLoop = false;
                    int limit = end;

                    if ((Math.Abs(start - end)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
                    {
                        if (start < end)
                        {
                            (start, end) = (end, start);
                        }

                        isLoop = true;
                        if (start < end)
                        {
                            limit = end;
                            end = 0;
                        }
                        else
                        {
                            limit = end;
                            end = mf.bnd.bndList[bndSelect].fenceLine.Count;
                        }
                    }
                    else
                    {
                        if (start > end)
                        {
                            (start, end) = (end, start);
                        }
                    }

                    sliceArr?.Clear();
                    vec3 pt3 = new vec3();

                    if (start < end)
                    {
                        for (int i = start; i <= end; i++)
                        {
                            //calculate the point inside the boundary
                            pt3 = mf.bnd.bndList[bndSelect].fenceLine[i];
                            sliceArr.Add(pt3);

                            if (isLoop && i == mf.bnd.bndList[bndSelect].fenceLine.Count - 1)
                            {
                                i = -1;
                                isLoop = false;
                                end = limit;
                            }
                        }
                    }
                    else
                    {
                        for (int i = start; i >= end; i--)
                        {
                            //calculate the point inside the boundary
                            pt3 = mf.bnd.bndList[bndSelect].fenceLine[i];
                            sliceArr.Add(pt3);

                            if (isLoop && i == 0)
                            {
                                i = mf.bnd.bndList[bndSelect].fenceLine.Count - 1;
                                isLoop = false;
                                end = limit;
                            }
                        }
                    }

                    int ptCnt = sliceArr.Count - 1;

                    if (ptCnt > 0)
                    {
                        //who knows which way it actually goes
                        mf.curve.CalculateHeadings(ref sliceArr);

                        for (int i = 1; i < 30; i++)
                        {
                            vec3 pt = new vec3(sliceArr[ptCnt]);
                            pt.easting += (Math.Sin(pt.heading) * i);
                            pt.northing += (Math.Cos(pt.heading) * i);
                            sliceArr.Add(pt);
                        }

                        vec3 stat = new vec3(sliceArr[0]);

                        for (int i = 1; i < 30; i++)
                        {
                            vec3 pt = new vec3(stat);
                            pt.easting -= (Math.Sin(pt.heading) * i);
                            pt.northing -= (Math.Cos(pt.heading) * i);
                            sliceArr.Insert(0, pt);
                        }

                        mode = TrackMode.Curve;
                    }
                    else
                    {
                        start = 99999; end = 99999;
                        return;
                    }

                    //update the arrays
                    start = 99999; end = 99999;

                    btnExit.Focus();
                }
                else if (rbtnLine.Checked)
                {
                    if ((Math.Abs(start - end)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
                    {
                        if (start < end)
                        {
                            (start, end) = (end, start);
                        }
                    }
                    else
                    {
                        if (start > end)
                        {
                            (start, end) = (end, start);
                        }
                    }

                    vec3 ptA = new vec3(mf.bnd.bndList[bndSelect].fenceLine[start]);
                    vec3 ptB = new vec3(mf.bnd.bndList[bndSelect].fenceLine[end]);

                    //calculate the AB Heading
                    double abHead = Math.Atan2(
                        mf.bnd.bndList[bndSelect].fenceLine[end].easting - mf.bnd.bndList[bndSelect].fenceLine[start].easting,
                        mf.bnd.bndList[bndSelect].fenceLine[end].northing - mf.bnd.bndList[bndSelect].fenceLine[start].northing);
                    if (abHead < 0) abHead += glm.twoPI;

                    sliceArr?.Clear();

                    ptA.heading = abHead;
                    ptB.heading = abHead;

                    for (int i = 0; i <= (int)(glm.Distance(ptA, ptB)); i++)
                    {
                        vec3 ptC = new vec3(ptA)
                        {
                            easting = (Math.Sin(abHead) * i) + ptA.easting,
                            northing = (Math.Cos(abHead) * i) + ptA.northing,
                            heading = abHead
                        };
                        sliceArr.Add(ptC);
                    }

                    int ptCnt = sliceArr.Count - 1;

                    for (int i = 1; i < 30; i++)
                    {
                        vec3 pt = new vec3(sliceArr[ptCnt]);
                        pt.easting += (Math.Sin(pt.heading) * i);
                        pt.northing += (Math.Cos(pt.heading) * i);
                        sliceArr.Add(pt);
                    }

                    vec3 stat = new vec3(sliceArr[0]);

                    for (int i = 1; i < 30; i++)
                    {
                        vec3 pt = new vec3(stat);
                        pt.easting -= (Math.Sin(pt.heading) * i);
                        pt.northing -= (Math.Cos(pt.heading) * i);
                        sliceArr.Insert(0, pt);
                    }

                    mode = TrackMode.AB;

                    start = 99999; end = 99999;
                }

                //Move the line
                if (nudSetDistance.Value != 0)
                    SetLineDistance();

                btnClipLine.Enabled = true;
            }
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

            //draw all the boundaries
            GL.LineWidth(4);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == bndSelect)
                    GL.Color3(0.75f, 0.75f, 0.750f);
                else
                    GL.Color3(0.0f, 0.25f, 0.10f);

                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < mf.bnd.bndList[j].fenceLine.Count; i++)
                {
                    GL.Vertex3(mf.bnd.bndList[j].fenceLine[i].easting, mf.bnd.bndList[j].fenceLine[i].northing, 0);
                }
                GL.End();
            }

            //the vehicle
            GL.PointSize(16.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.190f, 0.20f);
            GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0.0);
            GL.End();

            //draw the line building graphics
            if (start != 99999 || end != 99999) DrawABTouchLine();

            //draw the actual built lines
            //if (start == 99999 && end == 99999)
            {
                DrawBuiltLines();
            }

            GL.Disable(EnableCap.Blend);

            GL.Flush();
            oglSelf.SwapBuffers();
        }
        private void DrawBuiltLines()
        {
            //GL.LineWidth(8);
            //GL.Color3(0.03f, 0.0f, 0.150f);
            //GL.Begin(PrimitiveType.LineLoop);

            //for (int i = 0; i < mf.bnd.bndList[0].hdLine.Count; i++)
            //{
            //    GL.Vertex3(mf.bnd.bndList[0].hdLine[i].easting, mf.bnd.bndList[0].hdLine[i].northing, 0);
            //}
            //GL.End();

            GL.LineWidth(8);
            GL.Color3(0.943f, 0.9083f, 0.09150f);
            GL.Begin(PrimitiveType.LineLoop);

            for (int i = 0; i < mf.bnd.bndList[0].hdLine.Count; i++)
            {
                GL.Vertex3(mf.bnd.bndList[0].hdLine[i].easting, mf.bnd.bndList[0].hdLine[i].northing, 0);
            }
            GL.End();

            if (sliceArr.Count > 0)
            {
                //GL.Enable(EnableCap.LineStipple);
                //GL.LineStipple(1, 0x7070);
                GL.PointSize(8);

                if (mode == TrackMode.AB)
                {
                    GL.Color3(0.95f, 0.09f, 0.0f);
                }
                else
                {
                    GL.Color3(0.13f, 0.95f, 0.020f);
                }

                GL.Begin(PrimitiveType.LineStrip);
                foreach (vec3 item in sliceArr)
                {
                    GL.Vertex3(item.easting, item.northing, 0);
                }
                GL.End();

                int cnt = sliceArr.Count - 1;
                GL.PointSize(24);
                GL.Color3(1.0f, 0.6f, 0.3f);
                GL.Begin(PrimitiveType.Points);
                GL.Vertex3(sliceArr[0].easting, sliceArr[0].northing, 0);
                GL.Color3(0.5f, 0.73f, 0.99f);
                GL.Vertex3(sliceArr[cnt].easting, sliceArr[cnt].northing, 0);
                GL.End();
            }
        }

        private void DrawABTouchLine()
        {
            GL.PointSize(24);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(0, 0, 0);
            if (start != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);
            if (end != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            GL.End();

            GL.PointSize(18);
            GL.Begin(PrimitiveType.Points);

            GL.Color3(1.0f, 0.75f, 0.350f);
            if (start != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);

            GL.Color3(0.5f, 0.75f, 1.0f);
            if (end != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            GL.End();
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(TriangleFace.Back);
            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
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


        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
            if (sliceArr.Count == 0)
            {
                btnClipLine.Enabled = false;
                btnALength.Enabled = false;
                btnBLength.Enabled = false;
                btnAShrink.Enabled = false;
                btnBShrink.Enabled = false;
            }
            else
            {
                btnClipLine.Enabled = true;
                btnBLength.Enabled = true;
                btnALength.Enabled = true;
                btnAShrink.Enabled = true;
                btnBShrink.Enabled = true;
            }

            if (backupList.Count == 0) btnUndo.Enabled = false; else btnUndo.Enabled = true;
            if (nudSetDistance.Value == 0) btnBndLoop.Enabled = false; else btnBndLoop.Enabled = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            vec3[] hdArr;

            if (mf.bnd.bndList[0].hdLine.Count > 0)
            {
                hdArr = new vec3[mf.bnd.bndList[0].hdLine.Count];
                mf.bnd.bndList[0].hdLine.CopyTo(hdArr);
                mf.bnd.bndList[0].hdLine?.Clear();

                //does headland control sections
                mf.bnd.isSectionControlledByHeadland = cboxIsSectionControlled.Checked;
                Properties.Settings.Default.setHeadland_isSectionControlled = cboxIsSectionControlled.Checked;
                Properties.Settings.Default.Save();

                //middle points
                for (int i = 1; i < hdArr.Length; i++)
                {
                    hdArr[i - 1].heading = Math.Atan2(hdArr[i - 1].easting - hdArr[i].easting, hdArr[i - 1].northing - hdArr[i].northing);
                    if (hdArr[i].heading < 0) hdArr[i].heading += glm.twoPI;
                }

                double delta = 0;
                for (int i = 0; i < hdArr.Length; i++)
                {
                    if (i == 0)
                    {
                        mf.bnd.bndList[0].hdLine.Add(new vec3(hdArr[i].easting, hdArr[i].northing, hdArr[i].heading));
                        continue;
                    }
                    delta += (hdArr[i - 1].heading - hdArr[i].heading);

                    if (Math.Abs(delta) > 0.005)
                    {
                        vec3 pt = new vec3(hdArr[i].easting, hdArr[i].northing, hdArr[i].heading);

                        mf.bnd.bndList[0].hdLine.Add(pt);
                        delta = 0;
                    }
                }
                vec3 ptEnd = new vec3(hdArr[hdArr.Length - 1].easting, hdArr[hdArr.Length - 1].northing, hdArr[hdArr.Length - 1].heading);

                mf.bnd.bndList[0].hdLine.Add(ptEnd);
            }

            mf.FileSaveHeadland();
            Close();
        }

        private void SetLineDistance()
        {
            mf.hdl.desList?.Clear();

            if (sliceArr.Count < 1) return;

            double distAway = (double)nudSetDistance.Value * mf.ftOrMtoM;

            double distSqAway = (distAway * distAway) - 0.01;
            vec3 point;

            int refCount = sliceArr.Count;
            for (int i = 0; i < refCount; i++)
            {
                point = new vec3(
                sliceArr[i].easting - (Math.Sin(glm.PIBy2 + sliceArr[i].heading) * distAway),
                sliceArr[i].northing - (Math.Cos(glm.PIBy2 + sliceArr[i].heading) * distAway),
                sliceArr[i].heading);
                bool Add = true;

                for (int t = 0; t < refCount; t++)
                {
                    double dist = ((point.easting - sliceArr[t].easting) * (point.easting - sliceArr[t].easting))
                        + ((point.northing - sliceArr[t].northing) * (point.northing - sliceArr[t].northing));
                    if (dist < distSqAway)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add)
                {
                    if (mf.hdl.desList.Count > 0)
                    {
                        double dist = ((point.easting - mf.hdl.desList[mf.hdl.desList.Count - 1].easting) * (point.easting - mf.hdl.desList[mf.hdl.desList.Count - 1].easting))
                            + ((point.northing - mf.hdl.desList[mf.hdl.desList.Count - 1].northing) * (point.northing - mf.hdl.desList[mf.hdl.desList.Count - 1].northing));
                        if (dist > 1)
                            mf.hdl.desList.Add(point);
                    }
                    else mf.hdl.desList.Add(point);
                }
            }

            sliceArr.Clear();

            for (int i = 0; i < mf.hdl.desList.Count; i++)
            {
                sliceArr.Add(new vec3(mf.hdl.desList[i]));
            }

            mf.hdl.desList?.Clear();
        }

        private void nudSetDistance_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            btnExit.Focus();
        }

        // Returns 1 if the lines intersect, otherwis
        public double iE = 0, iN = 0;

        private void btnBndLoop_Click(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndList[0].fenceLine.Count;

            if (nudSetDistance.Value == 0)
            {
                mf.hdl.desList.Clear();

                mf.bnd.bndList[0].hdLine?.Clear();

                for (int i = 0; i < ptCount; i++)
                {
                    mf.bnd.bndList[0].hdLine.Add(new vec3(mf.bnd.bndList[0].fenceLine[i]));
                }
            }
            else
            {
                mf.hdl.desList?.Clear();

                //outside point
                vec3 pt3 = new vec3();

                double moveDist = (double)nudSetDistance.Value * mf.ftOrMtoM;
                double distSq = (moveDist) * (moveDist) * 0.999;

                //make the boundary tram outer array
                for (int i = 0; i < ptCount; i++)
                {
                    //calculate the point inside the boundary
                    pt3.easting = mf.bnd.bndList[0].fenceLine[i].easting -
                        (Math.Sin(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (moveDist));

                    pt3.northing = mf.bnd.bndList[0].fenceLine[i].northing -
                        (Math.Cos(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (moveDist));

                    pt3.heading = mf.bnd.bndList[0].fenceLine[i].heading;

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
                        if (mf.hdl.desList.Count > 0)
                        {
                            double dist = ((pt3.easting - mf.hdl.desList[mf.hdl.desList.Count - 1].easting) * (pt3.easting - mf.hdl.desList[mf.hdl.desList.Count - 1].easting))
                                + ((pt3.northing - mf.hdl.desList[mf.hdl.desList.Count - 1].northing) * (pt3.northing - mf.hdl.desList[mf.hdl.desList.Count - 1].northing));
                            if (dist > 1)
                                mf.hdl.desList.Add(pt3);
                        }
                        else mf.hdl.desList.Add(pt3);
                    }
                }

                if (mf.hdl.desList.Count == 0)
                {
                    return;
                }

                pt3 = new vec3(mf.hdl.desList[0]);
                mf.hdl.desList.Add(pt3);

                int cnt = mf.hdl.desList.Count;
                if (cnt > 3)
                {
                    pt3 = new vec3(mf.hdl.desList[0]);
                    mf.hdl.desList.Add(pt3);

                    //make sure point distance isn't too big 
                    mf.curve.MakePointMinimumSpacing(ref mf.hdl.desList, 1.2);
                    mf.curve.CalculateHeadings(ref mf.hdl.desList);

                    mf.bnd.bndList[0].hdLine.Clear();

                    //write out the Points
                    foreach (vec3 item in mf.hdl.desList)
                    {
                        mf.bnd.bndList[0].hdLine.Add(item);
                    }
                }
            }

            mf.FileSaveHeadland();
        }

        private void btnSlice_Click(object sender, EventArgs e)
        {
            int startBnd = 0, endBnd = 0, startLine = 0, endLine = 0;
            int isStart = 0;

            if (sliceArr.Count == 0) return;

            //save a backup
            backupList?.Clear();
            foreach (var item in mf.bnd.bndList[0].hdLine)
            {
                backupList.Add(item);
            }

            for (int i = 0; i < sliceArr.Count - 2; i++)
            {
                for (int k = 0; k < mf.bnd.bndList[0].hdLine.Count - 2; k++)
                {
                    int res = GetLineIntersection(
                    sliceArr[i].easting,
                    sliceArr[i].northing,
                    sliceArr[i + 1].easting,
                    sliceArr[i + 1].northing,

                    mf.bnd.bndList[0].hdLine[k].easting,
                    mf.bnd.bndList[0].hdLine[k].northing,
                    mf.bnd.bndList[0].hdLine[k + 1].easting,
                    mf.bnd.bndList[0].hdLine[k + 1].northing,
                    ref iE, ref iN);
                    if (res == 1)
                    {
                        if (isStart == 0)
                        {
                            startBnd = k + 1;
                            startLine = i + 1;
                        }
                        else
                        {
                            endBnd = k + 1;
                            endLine = i;
                        }
                        isStart++;
                    }
                }
            }

            if (isStart < 2)
            {
                mf.TimedMessageBox(2000, "Error", "Crossings not Found");
                Log.EventWriter("Headland, Crossings Not Found");

                return;
            }

            //overlaps start finish
            if ((Math.Abs(startBnd - endBnd)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
            {
                if (startBnd < endBnd)
                {
                    (startBnd, endBnd) = (endBnd, startBnd);
                }

                mf.hdl.desList?.Clear();

                //first bnd segment
                for (int i = endBnd; i < startBnd; i++)
                {
                    mf.hdl.desList.Add(mf.bnd.bndList[0].hdLine[i]);
                }

                for (int i = startLine; i < endLine; i++)
                {
                    mf.hdl.desList.Add(sliceArr[i]);
                }

                //build headline from desList
                mf.bnd.bndList[0].hdLine.Clear();

                foreach (var item in mf.hdl.desList)
                {
                    mf.bnd.bndList[0].hdLine.Add(item);
                }
            }
            // completely in between start finish
            else
            {
                if (startBnd > endBnd)
                {
                    (startBnd, endBnd) = (endBnd, startBnd);
                }

                mf.hdl.desList?.Clear();

                //first bnd segment
                for (int i = 0; i < startBnd; i++)
                {
                    mf.hdl.desList.Add(mf.bnd.bndList[0].hdLine[i]);
                }

                //line segment
                for (int i = startLine; i < endLine; i++)
                {
                    mf.hdl.desList.Add(sliceArr[i]);
                }

                //final bnd segment
                for (int i = endBnd; i < mf.bnd.bndList[0].hdLine.Count; i++)
                {
                    mf.hdl.desList.Add(mf.bnd.bndList[0].hdLine[i]);
                }

                //build headline from desList
                mf.bnd.bndList[0].hdLine.Clear();

                foreach (var item in mf.hdl.desList)
                {
                    mf.bnd.bndList[0].hdLine.Add(item);
                }
            }

            mf.hdl.desList?.Clear();
            sliceArr?.Clear();
        }

        private void btnDeletePoints_Click(object sender, EventArgs e)
        {
            start = 99999; end = 99999;
            isA = true;
            mf.hdl.desList?.Clear();
            sliceArr?.Clear();
            backupList?.Clear();
            mf.bnd.bndList[0].hdLine?.Clear();

            int ptCount = mf.bnd.bndList[0].fenceLine.Count;

            for (int i = 0; i < ptCount; i++)
            {
                mf.bnd.bndList[0].hdLine.Add(new vec3(mf.bnd.bndList[0].fenceLine[i]));
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            mf.bnd.bndList[0].hdLine?.Clear();
            foreach (var item in backupList)
            {
                mf.bnd.bndList[0].hdLine.Add(item);
            }
            backupList?.Clear();
        }

        private void cboxToolWidths_SelectedIndexChanged(object sender, EventArgs e)
        {
            nudSetDistance.Value = (decimal)((Math.Round((mf.tool.width - mf.tool.overlap) * cboxToolWidths.SelectedIndex, 1)) * mf.m2FtOrM);
        }

        private void btnALength_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 0)
            {
                //and the beginning
                vec3 start = new vec3(sliceArr[0]);

                for (int i = 1; i < 10; i++)
                {
                    vec3 pt = new vec3(start);
                    pt.easting -= (Math.Sin(pt.heading) * i);
                    pt.northing -= (Math.Cos(pt.heading) * i);
                    sliceArr.Insert(0, pt);
                }
            }
        }

        private void btnBLength_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 0)
            {
                int ptCnt = sliceArr.Count - 1;

                for (int i = 1; i < 10; i++)
                {
                    vec3 pt = new vec3(sliceArr[ptCnt]);
                    pt.easting += (Math.Sin(pt.heading) * i);
                    pt.northing += (Math.Cos(pt.heading) * i);
                    sliceArr.Add(pt);
                }
            }
        }

        private void btnBShrink_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 8)
                sliceArr.RemoveRange(sliceArr.Count - 5, 5);
        }

        private void btnAShrink_Click(object sender, EventArgs e)
        {
            if (sliceArr.Count > 8)
                sliceArr.RemoveRange(0, 5);
        }

        private void cboxIsSectionControlled_Click(object sender, EventArgs e)
        {
            if (cboxIsSectionControlled.Checked) cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOn;
            else cboxIsSectionControlled.Image = Properties.Resources.HeadlandSectionOff;
        }

        private void cboxIsZoom_CheckedChanged(object sender, EventArgs e)
        {
            zoomToggle = false;
        }

        private void btnHeadlandOff_Click(object sender, EventArgs e)
        {
            mf.bnd.bndList[0].hdLine?.Clear();
            mf.FileSaveHeadland();
            mf.bnd.isHeadlandOn = false;
            mf.vehicle.isHydLiftOn = false;
            Close();
        }

        public int GetLineIntersection(double p0x, double p0y, double p1x, double p1y,
        double p2x, double p2y, double p3x, double p3y, ref double iEast, ref double iNorth)
        {
            double s1x, s1y, s2x, s2y;
            s1x = p1x - p0x;
            s1y = p1y - p0y;

            s2x = p3x - p2x;
            s2y = p3y - p2y;

            double s, t;
            s = (-s1y * (p0x - p2x) + s1x * (p0y - p2y)) / (-s2x * s1y + s1x * s2y);

            if (s >= 0 && s <= 1)
            {
                //check oher side
                t = (s2x * (p0y - p2y) - s2y * (p0x - p2x)) / (-s2x * s1y + s1x * s2y);
                if (t >= 0 && t <= 1)
                {
                    // Collision detected
                    iEast = p0x + (t * s1x);
                    iNorth = p0y + (t * s1y);
                    return 1;
                }
            }

            return 0; // No collision
        }

    }
}
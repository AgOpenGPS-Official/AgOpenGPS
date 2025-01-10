using AgOpenGPS.Culture;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AgOpenGPS
{
    public partial class FormTramLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isCancel = false;

        private int indx = -1;

        private Point fixPt;
        private vec2 ptA = new vec2(9999999, 9999999);
        private vec2 ptB = new vec2(9999999, 9999999);
        private vec2 ptCut = new vec2(9999999, 9999999);

        private int step = 0;

        public vec2 pint = new vec2();

        //tramlines
        public List<vec2> tramArr = new List<vec2>();

        public List<List<vec2>> tramList = new List<List<vec2>>();

        private bool isDrawSections = false;

        private int displayMode = 0;

        public FormTramLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            nudPasses.Controls[0].Enabled = false;
            this.Text = gStr.gsTramLines;

            mf.CalculateMinMax();
        }

        private void FormTramLine_Load(object sender, EventArgs e)
        {
            //trams
            tbarTramAlpha.Value = (int)(mf.tram.tramAlpha * 100);
            lblAplha.Text = tbarTramAlpha.Value.ToString() + "%";

            nudPasses.Value = 2;
            nudPasses.ValueChanged += nudPasses_ValueChanged;

            lblTrack.Text = (mf.vehicle.trackWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;
            lblTramWidth.Text = (mf.tram.tramWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;
            lblSeedWidth.Text = (mf.tool.width * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            mf.tool.halfWidth = (mf.tool.width - mf.tool.overlap) / 2.0;

            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;

            if (mf.trk.gArr.Count > 0)
            {
                if (mf.trk.idx > -1 && mf.trk.idx <= mf.trk.gArr.Count)
                {
                    indx = mf.trk.idx;
                }
                else
                    indx = 0;
            }

            FixLabelsCurve();

            //Window Properties
            Size = Properties.Settings.Default.setWindow_tramLineSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormTramLine_ResizeEnd(this, e);

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }

            //build outer trams
            BuildTramBnd();
        }

        private void FormTramLine_ResizeEnd(object sender, EventArgs e)
        {
            Width = (Height + 300);

            oglSelf.Height = oglSelf.Width = Height - 85;

            oglSelf.Left = 1;
            oglSelf.Top = 1;

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

        private void FormTramLine_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isCancel)
            {
                mf.tram.tramArr?.Clear();
                mf.tram.tramList?.Clear();
                mf.tram.tramBndOuterArr?.Clear();
                mf.tram.tramBndInnerArr?.Clear();

                mf.tram.displayMode = 0;
            }
            
            mf.FileSaveTram();
            mf.PanelUpdateRightAndBottom();
            mf.FixTramModeButton();
            

            Properties.Settings.Default.setWindow_tramLineSize = Size;
            Properties.Settings.Default.setTram_alpha = mf.tram.tramAlpha;
        }

        public void BuildTramBnd()
        {
            mf.tram.displayMode = 1;
            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();
            CreateBndOuterTramTrack();
            CreateBndInnerTramTrack();
        }

        private void CreateBndInnerTramTrack()
        {
            //countExit the points from the boundary
            int ptCount = mf.bnd.bndList[0].fenceLine.Count;
            mf.tram.tramBndInnerArr?.Clear();

            //outside point
            vec2 pt3 = new vec2();

            double distSq = ((mf.tram.tramWidth * 0.5) + mf.tram.halfWheelTrack) * ((mf.tram.tramWidth * 0.5) + mf.tram.halfWheelTrack) * 0.999;

            //make the boundary tram outer array
            for (int i = 0; i < ptCount; i++)
            {
                //calculate the point inside the boundary
                pt3.easting = mf.bnd.bndList[0].fenceLine[i].easting -
                    (Math.Sin(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (mf.tram.tramWidth * 0.5 + mf.tram.halfWheelTrack));

                pt3.northing = mf.bnd.bndList[0].fenceLine[i].northing -
                    (Math.Cos(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (mf.tram.tramWidth * 0.5 + mf.tram.halfWheelTrack));

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
                    if (mf.tram.tramBndInnerArr.Count > 0)
                    {
                        double dist = ((pt3.easting - mf.tram.tramBndInnerArr[mf.tram.tramBndInnerArr.Count - 1].easting) * (pt3.easting - mf.tram.tramBndInnerArr[mf.tram.tramBndInnerArr.Count - 1].easting))
                            + ((pt3.northing - mf.tram.tramBndInnerArr[mf.tram.tramBndInnerArr.Count - 1].northing) * (pt3.northing - mf.tram.tramBndInnerArr[mf.tram.tramBndInnerArr.Count - 1].northing));
                        if (dist > 1.2)
                            mf.tram.tramBndInnerArr.Add(pt3);
                    }
                    else mf.tram.tramBndInnerArr.Add(pt3);
                }
            }
        }

        public void CreateBndOuterTramTrack()
        {
            //countExit the points from the boundary
            int ptCount = mf.bnd.bndList[0].fenceLine.Count;
            mf.tram.tramBndOuterArr?.Clear();

            //outside point
            vec2 pt3 = new vec2();

            double distSq = ((mf.tram.tramWidth * 0.5) - mf.tram.halfWheelTrack) * ((mf.tram.tramWidth * 0.5) - mf.tram.halfWheelTrack) * 0.999;

            //make the boundary tram outer array
            for (int i = 0; i < ptCount; i++)
            {
                //calculate the point inside the boundary
                pt3.easting = mf.bnd.bndList[0].fenceLine[i].easting -
                    (Math.Sin(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (mf.tram.tramWidth * 0.5 - mf.tram.halfWheelTrack));

                pt3.northing = mf.bnd.bndList[0].fenceLine[i].northing -
                    (Math.Cos(glm.PIBy2 + mf.bnd.bndList[0].fenceLine[i].heading) * (mf.tram.tramWidth * 0.5 - mf.tram.halfWheelTrack));

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
                    if (mf.tram.tramBndOuterArr.Count > 0)
                    {
                        double dist = ((pt3.easting - mf.tram.tramBndOuterArr[mf.tram.tramBndOuterArr.Count - 1].easting) * (pt3.easting - mf.tram.tramBndOuterArr[mf.tram.tramBndOuterArr.Count - 1].easting))
                            + ((pt3.northing - mf.tram.tramBndOuterArr[mf.tram.tramBndOuterArr.Count - 1].northing) * (pt3.northing - mf.tram.tramBndOuterArr[mf.tram.tramBndOuterArr.Count - 1].northing));
                        if (dist > 1.2)
                            mf.tram.tramBndOuterArr.Add(pt3);
                    }
                    else mf.tram.tramBndOuterArr.Add(pt3);
                }
            }
        }

        private void btnAddLines_Click(object sender, EventArgs e)
        {
            if (tramList.Count > 0)
            {
                for (int i = 0; i < tramList.Count; i++)
                {
                    mf.tram.tramArr = new List<vec2>
                    {
                        Capacity = 32
                    };

                    mf.tram.tramList.Add(mf.tram.tramArr);

                    for (int j = 0; j < tramList[i].Count; j++)
                    {
                        vec2 tr = new vec2(tramList[i][j]);
                        mf.tram.tramArr.Add(tr);
                    }
                }
            }

            tramList?.Clear();
            tramArr?.Clear();
        }

        public void BuildTram()
        {
            mf.trk.idx = indx;

            if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.Curve)
            {
                //if (Dist != 0)
                //mf.trk.NudgeRefCurve(Dist);
                BuildCurveTram();
            }
            else if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.AB)
            {
                //if (Dist != 0)
                //mf.trk.NudgeRefABLine(Dist);
                BuildABTram();
            }
            else
            {
                mf.TimedMessageBox(2000, "Invalid Line", "Use AB LIne or Curve Only");
            }
        }

        private void BuildCurveTram()
        {
            tramList?.Clear();
            tramArr?.Clear();

            bool isBndExist = mf.bnd.bndList.Count != 0;

            int refCount = mf.trk.gArr[mf.trk.idx].curvePts.Count;

            int cntr = 0;
            if (isBndExist)
            {
                if (mf.tram.generateMode == 1)
                    cntr = 0;
                else
                    cntr = 1;
            }

            double widd;

            for (int i = cntr; i <= mf.tram.passes; i++)
            {
                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);

                widd = (mf.tram.tramWidth * 0.5) - mf.tram.halfWheelTrack;
                widd += (mf.tram.tramWidth * i);

                double distSqAway = widd * widd * 0.999999;

                for (int j = 0; j < refCount; j += 1)
                {
                    vec2 point = new vec2(
                    (Math.Sin(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd) + mf.trk.gArr[mf.trk.idx].curvePts[j].easting,
                    (Math.Cos(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd) + mf.trk.gArr[mf.trk.idx].curvePts[j].northing
                        );

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = ((point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting) * (point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting))
                            + ((point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing) * (point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing));
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        //a new point only every 2 meters
                        double dist = tramArr.Count > 0 ? ((point.easting - tramArr[tramArr.Count - 1].easting) * (point.easting - tramArr[tramArr.Count - 1].easting))
                            + ((point.northing - tramArr[tramArr.Count - 1].northing) * (point.northing - tramArr[tramArr.Count - 1].northing)) : 3.0;
                        if (dist > 1.2)
                        {
                            //if inside the boundary, add
                            if (!isBndExist || mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                            {
                                tramArr.Add(point);
                            }
                        }
                    }
                }
            }

            for (int i = cntr; i <= mf.tram.passes; i++)
            {
                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);

                widd = (mf.tram.tramWidth * 0.5) + mf.tram.halfWheelTrack;
                widd += (mf.tram.tramWidth * i);
                double distSqAway = widd * widd * 0.999999;

                for (int j = 0; j < refCount; j += 1)
                {
                    vec2 point = new vec2(
                    Math.Sin(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd + mf.trk.gArr[mf.trk.idx].curvePts[j].easting,
                    Math.Cos(glm.PIBy2 + mf.trk.gArr[mf.trk.idx].curvePts[j].heading) *
                        widd + mf.trk.gArr[mf.trk.idx].curvePts[j].northing
                        );

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = ((point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting) * (point.easting - mf.trk.gArr[mf.trk.idx].curvePts[t].easting))
                            + ((point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing) * (point.northing - mf.trk.gArr[mf.trk.idx].curvePts[t].northing));
                        if (dist < distSqAway)
                        {
                            Add = false;
                            break;
                        }
                    }
                    if (Add)
                    {
                        //a new point only every 2 meters
                        double dist = tramArr.Count > 0 ? ((point.easting - tramArr[tramArr.Count - 1].easting) * (point.easting - tramArr[tramArr.Count - 1].easting))
                            + ((point.northing - tramArr[tramArr.Count - 1].northing) * (point.northing - tramArr[tramArr.Count - 1].northing)) : 3.0;
                        if (dist > 1.2)
                        {
                            //if inside the boundary, add
                            if (!isBndExist || mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                            {
                                tramArr.Add(point);
                            }
                        }
                    }
                }
            }
        }

        private void BuildABTram()
        {
            if (mf.tram.generateMode == 2) return;

            List<vec2> tramRef = new List<vec2>();

            bool isBndExist = mf.bnd.bndList.Count != 0;

            mf.ABLine.abHeading = mf.trk.gArr[mf.trk.idx].heading;

            double hsin = Math.Sin(mf.ABLine.abHeading);
            double hcos = Math.Cos(mf.ABLine.abHeading);

            double len = glm.Distance(mf.trk.gArr[mf.trk.idx].endPtA, mf.trk.gArr[mf.trk.idx].endPtB);
            //divide up the AB line into segments
            vec2 P1 = new vec2();
            for (int i = 0; i < (int)len; i += 2)
            {
                P1.easting = (hsin * i) + mf.trk.gArr[mf.trk.idx].endPtA.easting;
                P1.northing = (hcos * i) + mf.trk.gArr[mf.trk.idx].endPtA.northing;
                tramRef.Add(P1);
            }

            //create list of list of points of triangle strip of AB Highlight
            double headingCalc = mf.ABLine.abHeading + glm.PIBy2;

            hsin = Math.Sin(headingCalc);
            hcos = Math.Cos(headingCalc);

            tramList?.Clear();
            tramArr?.Clear();

            //no boundary starts on first pass
            int cntr = 1;

            double widd;
            for (int i = cntr; i < mf.tram.passes+1; i++)
            {
                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);

                widd = (mf.tram.tramWidth * 0.5) - mf.tram.halfWheelTrack;
                widd += (mf.tram.tramWidth * i);

                for (int j = 0; j < tramRef.Count; j++)
                {
                    P1.easting = hsin * widd + tramRef[j].easting;
                    P1.northing = (hcos * widd) + tramRef[j].northing;

                    if (!isBndExist || mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(P1))
                    {
                        tramArr.Add(P1);
                    }
                }
            }

            for (int i = cntr; i < mf.tram.passes+1; i++)
            {
                tramArr = new List<vec2>
                {
                    Capacity = 128
                };

                tramList.Add(tramArr);

                widd = (mf.tram.tramWidth * 0.5) + mf.tram.halfWheelTrack;
                widd += (mf.tram.tramWidth * i);

                for (int j = 0; j < tramRef.Count; j++)
                {
                    P1.easting = (hsin * widd) + tramRef[j].easting;
                    P1.northing = (hcos * widd) + tramRef[j].northing;

                    if (!isBndExist || mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(P1))
                    {
                        tramArr.Add(P1);
                    }
                }
            }

            tramRef?.Clear();
            //outside tram

            if (mf.bnd.bndList.Count == 0 || mf.tram.passes != 0)
            {
                //return;
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isCancel = false;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            isCancel = true;
            Close();
        }

        private void FixLabelsCurve()
        {
            if (indx > -1 && mf.trk.gArr.Count > 0)
            {
                tboxNameCurve.Text = mf.trk.gArr[indx].name;
                tboxNameCurve.Enabled = true;
                lblCurveSelected.Text = (indx + 1).ToString() + " / " + mf.trk.gArr.Count.ToString();
            }
            else
            {
                tboxNameCurve.Text = "***";
                tboxNameCurve.Enabled = false;
                lblCurveSelected.Text = "*";
            }
        }

        private void btnSelectCurve_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();

            if (mf.trk.gArr.Count > 0)
            {
                indx++;
                if (indx > (mf.trk.gArr.Count - 1)) indx = 0;
            }
            else
            {
                indx = -1;
            }
            mf.trk.idx = indx;

            FixLabelsCurve();
        }

        private void btnSelectCurveBk_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();

            if (mf.trk.gArr.Count > 0)
            {
                indx--;
                if (indx < 0) indx = mf.trk.gArr.Count - 1;
            }
            else
            {
                indx = -1;
            }
            mf.trk.idx = indx;

            FixLabelsCurve();
        }

        private void btnDrawSections_Click(object sender, EventArgs e)
        {
            isDrawSections = !isDrawSections;
            if (isDrawSections) btnDrawSections.Image = Properties.Resources.MappingOn;
            else btnDrawSections.Image = Properties.Resources.MappingOff;
        }

        //private void btnMode_Click(object sender, EventArgs e)
        //{
        //    displayMode++;

        //    if (displayMode > 2) displayMode = 0;

        //    switch (displayMode)
        //    {
        //        case 0:
        //            btnMode.Image = Properties.Resources.TramAll;
        //            break;

        //        case 1:
        //            btnMode.Image = Properties.Resources.TramLines;
        //            break;

        //        case 2:
        //            btnMode.Image = Properties.Resources.TramOuter;
        //            break;

        //        default:
        //            break;
        //    }

        //    //MoveBuildTramLine(0);
        //}

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            step++;

            Point ptt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            //Convert to Origin in the center of window, 800 pixels
            fixPt.X = ptt.X - halfWid;
            fixPt.Y = (wid - ptt.Y - halfWid);
            vec2 plotPt = new vec2
            {
                //convert screen coordinates to field coordinates
                easting = fixPt.X * mf.maxFieldDistance / scale,
                northing = fixPt.Y * mf.maxFieldDistance / scale,
            };

            plotPt.easting += mf.fieldCenterX;
            plotPt.northing += mf.fieldCenterY;

            if (step == 1)
            {
                ptA = plotPt;
            }
            else if (step == 2)
            {
                ptB = plotPt;
            }
            else
            {
                ptCut = plotPt;

                bool isLeft = (ptB.easting - ptA.easting) * (ptCut.northing - ptA.northing) 
                    > (ptB.northing - ptA.northing) * (ptCut.easting - ptA.easting);

                if (tramList.Count > 0)
                {
                    if (isLeft)
                    {
                        for (int i = 0; i < tramList.Count; i++)
                        {
                            for (int h = 0; h < tramList[i].Count; h++)
                            {
                                if ((ptB.easting - ptA.easting) * (tramList[i][h].northing - ptA.northing)
                                    > (ptB.northing - ptA.northing) * (tramList[i][h].easting - ptA.easting))
                                {
                                    tramList[i].RemoveAt(h);
                                    h = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < tramList.Count; i++)
                        {
                            for (int h = 0; h < tramList[i].Count; h++)
                            {
                                if ((ptB.easting - ptA.easting) * (tramList[i][h].northing - ptA.northing)
                                    < (ptB.northing - ptA.northing) * (tramList[i][h].easting - ptA.easting))
                                {
                                    tramList[i].RemoveAt(h);
                                    h = -1;
                                }
                            }
                        }
                    }
                }

                ptB.easting = 9999999;
                ptB.northing = 9999999;
                ptA.easting = 9999999;
                ptA.northing = 9999999;
                step = 0;
            }
        }


        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -mf.maxFieldDistance);

            //translate to that spot in the world
            GL.Translate(-mf.fieldCenterX, -mf.fieldCenterY, 0);

            if (isDrawSections) DrawSections();

            GL.LineWidth(3);

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                if (j == 0)
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

            DrawBuiltLines();

            DrawTrams();

            DrawNewTrams();

            GL.PointSize(18);

            GL.Begin(PrimitiveType.Points);
            GL.Color3(1.0, 0, 0);
            GL.Vertex3(ptA.easting, ptA.northing, 0);
            GL.End();

            GL.Begin(PrimitiveType.Points);
            GL.Color3(0,1.0, 0);
            GL.Vertex3(ptB.easting, ptB.northing, 0);
            GL.End();

            if (step == 2)
            {
                GL.LineWidth(6);

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(1.0, 0, 0);
                GL.Vertex3(ptA.easting, ptA.northing, 0);
                GL.Color3(0, 1.0, 0);
                GL.Vertex3(ptB.easting, ptB.northing, 0);

                GL.End();
            }

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawTrams()
        {
            GL.LineWidth(6);

            GL.Color4(0.930f, 0.72f, 0.73530f, mf.tram.tramAlpha);

            if (displayMode == 0 || displayMode == 1)
            {
                if (mf.tram.tramList.Count > 0)
                {
                    for (int i = 0; i < mf.tram.tramList.Count; i++)
                    {
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < mf.tram.tramList[i].Count; h++)
                            GL.Vertex3(mf.tram.tramList[i][h].easting, mf.tram.tramList[i][h].northing, 0);
                        GL.End();
                    }
                }
            }

            if (displayMode == 0 || displayMode == 2)
            {
                if (mf.tram.tramBndOuterArr.Count > 0)
                {
                    GL.Color4(0.830f, 0.72f, 0.3530f, mf.tram.tramAlpha);

                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < mf.tram.tramBndOuterArr.Count; h++) GL.Vertex3(mf.tram.tramBndOuterArr[h].easting, mf.tram.tramBndOuterArr[h].northing, 0);
                    GL.End();
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < mf.tram.tramBndInnerArr.Count; h++) GL.Vertex3(mf.tram.tramBndInnerArr[h].easting, mf.tram.tramBndInnerArr[h].northing, 0);
                    GL.End();
                }
            }
        }

        private void DrawNewTrams()
        {
            GL.LineWidth(2);

            GL.Color4(0.530f, 0.972f, 0.973530f, 0.9);

            if (displayMode == 0 || displayMode == 1)
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
        }

        private void DrawSections()
        {
            int cnt, step, patchCount;
            int mipmap = 8;

            GL.Color3(0.12f, 0.12f, 0.12f);

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

        private void DrawBuiltLines()
        {
            GL.LineStipple(1, 0x0707);
            for (int i = 0; i < mf.trk.gArr.Count; i++)
            {
                //AB Lines
                if (mf.trk.gArr[i].mode == TrackMode.AB)
                {
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineWidth(4);

                    if (i == indx)
                    {
                        GL.LineWidth(8);
                        GL.Disable(EnableCap.LineStipple);
                    }

                    GL.Color3(1.0f, 0.20f, 0.20f);

                    GL.Begin(PrimitiveType.Lines);

                    GL.Vertex3(mf.trk.gArr[i].ptA.easting - (Math.Sin(mf.trk.gArr[i].heading) * mf.ABLine.abLength), mf.trk.gArr[i].ptA.northing - (Math.Cos(mf.trk.gArr[i].heading) * mf.ABLine.abLength), 0);
                    GL.Vertex3(mf.trk.gArr[i].ptB.easting + (Math.Sin(mf.trk.gArr[i].heading) * mf.ABLine.abLength), mf.trk.gArr[i].ptB.northing + (Math.Cos(mf.trk.gArr[i].heading) * mf.ABLine.abLength), 0);

                    GL.End();

                    GL.Disable(EnableCap.LineStipple);

                    //if (mf.ABLine.numABLineSelected > 0)
                    //{
                    //    GL.Color3(1.0f, 0.0f, 0.0f);

                    //    GL.LineWidth(4);
                    //    GL.Begin(PrimitiveType.Lines);

                    //    GL.Vertex3(mf.trk.gArr[mf.ABLine.numABLineSelected - 1].ptA.easting - (Math.Sin(mf.trk.gArr[mf.ABLine.numABLineSelected - 1].heading) * mf.ABLine.abLength),
                    //        mf.trk.gArr[mf.ABLine.numABLineSelected - 1].ptA.northing - (Math.Cos(mf.trk.gArr[mf.ABLine.numABLineSelected - 1].heading) * mf.ABLine.abLength), 0);
                    //    GL.Vertex3(mf.trk.gArr[mf.ABLine.numABLineSelected - 1].ptA.easting + (Math.Sin(mf.trk.gArr[mf.ABLine.numABLineSelected - 1].heading) * mf.ABLine.abLength),
                    //        mf.trk.gArr[mf.ABLine.numABLineSelected - 1].ptA.northing + (Math.Cos(mf.trk.gArr[mf.ABLine.numABLineSelected - 1].heading) * mf.ABLine.abLength), 0);

                    //    GL.End();
                    //}

                }

                else if (mf.trk.gArr[i].mode == TrackMode.Curve || mf.trk.gArr[i].mode == TrackMode.bndCurve)
                {
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineWidth(5);

                    if (mf.trk.gArr[i].mode == TrackMode.bndCurve) GL.LineStipple(1, 0x0007);
                    else GL.LineStipple(1, 0x0707);


                    if (i == indx)
                    {
                        GL.LineWidth(8);
                        GL.Disable(EnableCap.LineStipple);
                    }

                    GL.Color3(0.30f, 0.97f, 0.30f);
                    if (mf.trk.gArr[i].mode == TrackMode.bndCurve) GL.Color3(0.70f, 0.5f, 0.2f);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach (vec3 pts in mf.trk.gArr[i].curvePts)
                    {
                        GL.Vertex3(pts.easting, pts.northing, 0);
                    }
                    GL.End();

                    GL.Disable(EnableCap.LineStipple);

                    if (i == indx) GL.PointSize(16);
                    else GL.PointSize(8);

                    GL.Color3(1.0f, 0.75f, 0.350f);
                    GL.Begin(PrimitiveType.Points);

                    GL.Vertex3(mf.trk.gArr[i].curvePts[0].easting,
                                mf.trk.gArr[i].curvePts[0].northing,
                                0);


                    GL.Color3(0.5f, 0.5f, 1.0f);
                    GL.Vertex3(mf.trk.gArr[i].curvePts[mf.trk.gArr[i].curvePts.Count - 1].easting,
                                mf.trk.gArr[i].curvePts[mf.trk.gArr[i].curvePts.Count - 1].northing,
                                0);
                    GL.End();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();
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
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
        }

        private void tbarTramAlpha_Scroll(object sender, EventArgs e)
        {
            mf.tram.tramAlpha = (double)tbarTramAlpha.Value * 0.01;
            lblAplha.Text = tbarTramAlpha.Value.ToString() + "%";
        }

        private void nudPasses_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.passes = (int)nudPasses.Value;
            Properties.Settings.Default.setTram_passes = mf.tram.passes;

            BuildTram();
        }

        private void nudPasses_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);
        }

        private void btnUpTrams_Click(object sender, EventArgs e)
        {
            nudPasses.UpButton();
        }

        private void btnDnTrams_Click(object sender, EventArgs e)
        {
            nudPasses.DownButton();
        }

        private void btnDeleteOuter_Click(object sender, EventArgs e)
        {
            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            mf.trk.idx = indx;

            if (mf.trk.gArr[mf.trk.idx].mode == TrackMode.AB)
            {
                vec2 bob = mf.trk.gArr[mf.trk.idx].ptA;
                mf.trk.gArr[mf.trk.idx].ptA = mf.trk.gArr[mf.trk.idx].ptB;
                mf.trk.gArr[mf.trk.idx].ptB = new vec2(bob);

                mf.trk.gArr[mf.trk.idx].heading += Math.PI;
                if (mf.trk.gArr[mf.trk.idx].heading < 0) mf.trk.gArr[mf.trk.idx].heading += glm.twoPI;
                if (mf.trk.gArr[mf.trk.idx].heading > glm.twoPI) mf.trk.gArr[mf.trk.idx].heading -= glm.twoPI;

                double abHeading = mf.trk.gArr[mf.trk.idx].heading;
                mf.trk.gArr[mf.trk.idx].endPtA.easting = mf.trk.gArr[mf.trk.idx].ptA.easting - (Math.Sin(abHeading) * mf.ABLine.abLength);
                mf.trk.gArr[mf.trk.idx].endPtA.northing = mf.trk.gArr[mf.trk.idx].ptA.northing - (Math.Cos(abHeading) * mf.ABLine.abLength);

                mf.trk.gArr[mf.trk.idx].endPtB.easting = mf.trk.gArr[mf.trk.idx].ptB.easting + (Math.Sin(abHeading) * mf.ABLine.abLength);
                mf.trk.gArr[mf.trk.idx].endPtB.northing = mf.trk.gArr[mf.trk.idx].ptB.northing + (Math.Cos(abHeading) * mf.ABLine.abLength);

            }
            else
            {
                int cnt = mf.trk.gArr[mf.trk.idx].curvePts.Count;
                if (cnt > 0)
                {
                    mf.trk.gArr[mf.trk.idx].curvePts.Reverse();

                    vec3[] arr = new vec3[cnt];
                    cnt--;
                    mf.trk.gArr[mf.trk.idx].curvePts.CopyTo(arr);
                    mf.trk.gArr[mf.trk.idx].curvePts.Clear();

                    mf.trk.gArr[mf.trk.idx].heading += Math.PI;
                    if (mf.trk.gArr[mf.trk.idx].heading < 0) mf.trk.gArr[mf.trk.idx].heading += glm.twoPI;
                    if (mf.trk.gArr[mf.trk.idx].heading > glm.twoPI) mf.trk.gArr[mf.trk.idx].heading -= glm.twoPI;

                    for (int i = 1; i < cnt; i++)
                    {
                        vec3 pt3 = arr[i];
                        pt3.heading += Math.PI;
                        if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        mf.trk.gArr[mf.trk.idx].curvePts.Add(pt3);
                    }

                    vec2 temp = new vec2(mf.trk.gArr[mf.trk.idx].ptA);

                    (mf.trk.gArr[mf.trk.idx].ptA) = new vec2(mf.trk.gArr[mf.trk.idx].ptB);
                    (mf.trk.gArr[mf.trk.idx].ptB) = new vec2(temp);
                }
            }

            mf.FileSaveTracks();

            nudPasses.Value = 2;
        }

        private void btnDeleteCurve_Click_1(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();
            mf.tram.tramList?.Clear();
            mf.tram.tramArr?.Clear();

            nudPasses.ValueChanged -= nudPasses_ValueChanged;
            nudPasses.Value = 2;
            nudPasses.ValueChanged += nudPasses_ValueChanged;

            //build outer trams
            BuildTramBnd();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            ptB.easting = 9999999;
            ptB.northing = 9999999;
            ptA.easting = 9999999;
            ptA.northing = 9999999;
            step = 0;
        }
    }
}
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Core.Visuals;
using AgOpenGPS.Helpers;
using AgOpenGPS.WinForms;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTramLine : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;
        private GeoViewport _viewport;

        private bool isCancel = false;

        private int indx = -1;

        private vec2 ptA = new vec2(9999999, 9999999);
        private vec2 ptB = new vec2(9999999, 9999999);
        private vec2 ptCut = new vec2(9999999, 9999999);

        private int step = 0;

        //tramTrams
        private List<vec2> tramArr = new List<vec2>();

        private List<List<vec2>> tramList = new List<List<vec2>>();

        private List<CTrk> gTemp = new List<CTrk>();

        private int passes, startPass;

        #region Form
        public FormTramLine(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
            mf.CalculateMinMax();
        }

        private void FormTramLine_Load(object sender, EventArgs e)
        {
            //translate all the controls
            Text = gStr.gsAdvancedTramLines;
            lblAplha.Text = ((int)(mf.tram.alpha * 100)).ToString();

            mf.tool.halfWidth = (mf.tool.width - mf.tool.overlap) / 2.0;

            FixLabelsCurve();

            //Window Properties
            Size = Properties.Settings.Default.setWindow_tramLineSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;
            FormTramLine_ResizeEnd(this, e);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            ResetStartNumLabels();
            LoadAndFixLines();
        }

        private void FormTramLine_ResizeEnd(object sender, EventArgs e)
        {
            Width = (Height + 300);

            oglSelf.Height = oglSelf.Width = Height - 40;

            oglSelf.Left = 1;
            oglSelf.Top = 1;

            _viewport.Resize(oglSelf.Width, oglSelf.Height);

            tlp1.Width = Width - oglSelf.Width - 4;
            tlp1.Left = oglSelf.Width;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;
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
            Properties.Settings.Default.setTram_alpha = mf.tram.alpha;
            Properties.Settings.Default.Save();
        }

        private void LoadAndFixLines()
        {
            //load the lines from Trks
            gTemp?.Clear();

            foreach (var item in mf.trk.gArr)
            {
                if ((item.mode == TrackMode.AB || item.mode == TrackMode.Curve) && item.isVisible)
                {
                    //default side assuming built in AB Draw - isVisible is used for side to draw
                    gTemp.Add(new CTrk(item));
                    if (item.mode == TrackMode.AB)
                        gTemp[gTemp.Count - 1].isVisible = false;
                    else
                        gTemp[gTemp.Count - 1].isVisible = true;
                }
            }

            if (gTemp == null || gTemp.Count == 0)
            {
                mf.YesMessageBox(gStr.gsNoGuidanceLines + "\r\n\r\n  Exiting");
                isCancel = true;
                Close();
            }
            else
            {
                indx = 0;
            }

            for (indx = 0; indx < gTemp.Count; indx++)
            {
                BuildTram();
                if (tramList[0].Count == 0)
                {
                    gTemp[indx].isVisible = !gTemp[indx].isVisible;
                    tramList.Clear();
                    tramArr.Clear();
                }
            }

            indx = 0;
            FixLabelsCurve();
            BuildTram();
        }

        private void ResetStartNumLabels()
        {
            if (cboxIsOuter.Checked)
            {
                startPass = 1;
            }
            else
            {
                startPass = 0;
            }

            if (mf.tram.tramBndOuterArr.Count > 0)
            {
                startPass = 1;
            }

            passes = 2;
            lblStartPass.Text = gStr.gsStart + ":\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
        }

        #endregion

        #region Building Lines

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

        private void BuildTram()
        {
            if (gTemp[indx].mode == TrackMode.Curve)
            {
                //if (Dist != 0)
                //mf.trk.NudgeRefCurve(Dist);
                BuildCurveTram();
            }
            else if (gTemp[indx].mode == TrackMode.AB)
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

            int refCount = gTemp[indx].curvePts.Count;

            int cntr = startPass;

            double widd;

            //draw on correct side
            double sideHeading = 0;
            if (gTemp[indx].isVisible) sideHeading = Math.PI;

            for (int i = cntr; i <= (passes + startPass) - 1; i++)
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
                    (Math.Sin(glm.PIBy2 + gTemp[indx].curvePts[j].heading + sideHeading) *
                        widd) + gTemp[indx].curvePts[j].easting,
                    (Math.Cos(glm.PIBy2 + gTemp[indx].curvePts[j].heading + sideHeading) *
                        widd) + gTemp[indx].curvePts[j].northing
                        );

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = ((point.easting - gTemp[indx].curvePts[t].easting) * (point.easting - gTemp[indx].curvePts[t].easting))
                            + ((point.northing - gTemp[indx].curvePts[t].northing) * (point.northing - gTemp[indx].curvePts[t].northing));
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
                            if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
                            {
                                tramArr.Add(point);
                            }
                        }
                    }
                }
            }

            for (int i = cntr; i <= (passes + startPass) - 1; i++)
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
                    Math.Sin(glm.PIBy2 + gTemp[indx].curvePts[j].heading + sideHeading) *
                        widd + gTemp[indx].curvePts[j].easting,
                    Math.Cos(glm.PIBy2 + gTemp[indx].curvePts[j].heading + sideHeading) *
                        widd + gTemp[indx].curvePts[j].northing
                        );

                    bool Add = true;
                    for (int t = 0; t < refCount; t++)
                    {
                        //distance check to be not too close to ref line
                        double dist = ((point.easting - gTemp[indx].curvePts[t].easting) * (point.easting - gTemp[indx].curvePts[t].easting))
                            + ((point.northing - gTemp[indx].curvePts[t].northing) * (point.northing - gTemp[indx].curvePts[t].northing));
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
                            if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(point))
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
            List<vec2> tramRef = new List<vec2>();

            double abHeading = gTemp[indx].heading;

            double hsin = Math.Sin(abHeading);
            double hcos = Math.Cos(abHeading);

            gTemp[indx].endPtA.easting = gTemp[indx].ptA.easting - (Math.Sin(abHeading) * mf.maxFieldDistance);
            gTemp[indx].endPtA.northing = gTemp[indx].ptA.northing - (Math.Cos(abHeading) * mf.maxFieldDistance);

            gTemp[indx].endPtB.easting = gTemp[indx].ptB.easting + (Math.Sin(abHeading) * mf.maxFieldDistance);
            gTemp[indx].endPtB.northing = gTemp[indx].ptB.northing + (Math.Cos(abHeading) * mf.maxFieldDistance);

            double len = glm.Distance(gTemp[indx].endPtA, gTemp[indx].endPtB);
            //divide up the AB line into segments
            vec2 P1 = new vec2();
            for (int i = 0; i < (int)len; i += 2)
            {
                P1.easting = (hsin * i) + gTemp[indx].endPtA.easting;
                P1.northing = (hcos * i) + gTemp[indx].endPtA.northing;
                tramRef.Add(P1);
            }

            //create list of list of points of triangle strip of AB Highlight
            double headingCalc = abHeading + glm.PIBy2;

            if (headingCalc < 0) headingCalc += glm.twoPI;
            if (headingCalc > glm.twoPI) headingCalc -= glm.twoPI;

            if (gTemp[indx].isVisible) headingCalc += Math.PI;
            if (headingCalc > glm.twoPI) headingCalc -= glm.twoPI;

            hsin = Math.Sin(headingCalc);
            hcos = Math.Cos(headingCalc);

            tramList?.Clear();
            tramArr?.Clear();

            //no boundary starts on first pass
            int cntr = startPass;

            double widd;
            for (int i = cntr; i < passes + startPass; i++)
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

                    if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(P1))
                    {
                        tramArr.Add(P1);
                    }
                }
            }

            for (int i = cntr; i < passes + startPass; i++)
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

                    if (mf.bnd.bndList[0].fenceLineEar.IsPointInPolygon(P1))
                    {
                        tramArr.Add(P1);
                    }
                }
            }

            tramRef?.Clear();
            //outside tram

            if (mf.bnd.bndList.Count == 0 || passes != 0)
            {
                //return;
            }
        }

        #endregion

        #region OpenGL and Drawing
        private void OnViewportMouseDown(object sender, GeoCoord mouseDownCoord)
        {
            step++;

            if (step == 1)
            {
                ptA = new vec2(mouseDownCoord);
            }
            else if (step == 2)
            {
                ptB = new vec2(mouseDownCoord);
            }
            else
            {
                ptCut = new vec2(mouseDownCoord);

                bool isLeft = (ptB.easting - ptA.easting) * (ptCut.northing - ptA.northing)
                    > (ptB.northing - ptA.northing) * (ptCut.easting - ptA.easting);

                bool isIntersect = false;

                if (tramList.Count > 0)
                {
                    for (int i = 0; i < tramList.Count; i++)
                    {
                        ////check for line intersection
                        for (int j = 0; j < tramList[i].Count - 1; j++)
                        {
                            if (GetLineIntersection(
                                tramList[i][j].easting, tramList[i][j].northing,
                                tramList[i][j + 1].easting, tramList[i][j + 1].northing,
                                ptA.easting, ptA.northing, ptB.easting, ptB.northing))
                            {
                                isIntersect = true;
                                break;
                            }
                        }

                        if (isIntersect)
                        {
                            for (int h = 0; h < tramList[i].Count; h++)
                            {
                                if (isLeft)
                                {
                                    if ((ptB.easting - ptA.easting) * (tramList[i][h].northing - ptA.northing)
                                        > (ptB.northing - ptA.northing) * (tramList[i][h].easting - ptA.easting))
                                    {
                                        tramList[i].RemoveAt(h);
                                        h = -1;
                                    }
                                }
                                else
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
                        isIntersect = false;
                    }
                }

                ptB.easting = 9999999;
                ptB.northing = 9999999;
                ptA.easting = 9999999;
                ptA.northing = 9999999;
                step = 0;
            }
        }

        private bool GetLineIntersection(double p0x, double p0y, double p1x, double p1y,
        double p2x, double p2y, double p3x, double p3y)
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
                    return true;
                }
            }

            return false; // No collision
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            _viewport.BeginPaint();

            for (int j = 0; j < mf.bnd.bndList.Count; j++)
            {
                GeoCoord[] fenceLineEar = GeoRefactorHelper.ToGeoCoordArray(mf.bnd.bndList[j].fenceLineEar);
                bool isSelected = j == 0;
                FenceLineVisual.DrawFenceLine(fenceLineEar, isSelected);
            }
            DrawBuiltLines();
            DrawTrams();
            DrawNewTrams();

            GL.PointSize(18);

            GL.Begin(PrimitiveType.Points);
            GL.Color3(1.0, 0, 0);
            GL.Vertex2(ptA.easting, ptA.northing);
            GL.End();

            GL.Begin(PrimitiveType.Points);
            GL.Color3(0, 1.0, 0);
            GL.Vertex2(ptB.easting, ptB.northing);
            GL.End();

            if (step == 2)
            {
                GL.LineWidth(6);

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(1.0, 0, 0);
                GL.Vertex2(ptA.easting, ptA.northing);
                GL.Color3(0, 1.0, 0);
                GL.Vertex2(ptB.easting, ptB.northing);
                GL.End();
            }
            _viewport.EndPaint();
        }

        private void DrawTrams()
        {
            GL.LineWidth(6);

            GL.Color4(0.730f, 0.52f, 0.63530f, mf.tram.alpha);

            if (mf.tram.tramList.Count > 0)
            {
                for (int i = 0; i < mf.tram.tramList.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < mf.tram.tramList[i].Count; h++)
                    {
                        GL.Vertex2(mf.tram.tramList[i][h].easting, mf.tram.tramList[i][h].northing);
                    }
                    GL.End();
                }
            }

            if (mf.tram.tramBndOuterArr.Count > 0)
            {
                GL.Color4(0.830f, 0.72f, 0.3530f, mf.tram.alpha);

                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < mf.tram.tramBndOuterArr.Count; h++)
                {
                    GL.Vertex2(mf.tram.tramBndOuterArr[h].easting, mf.tram.tramBndOuterArr[h].northing);
                }
                GL.End();
                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < mf.tram.tramBndInnerArr.Count; h++)
                {
                    GL.Vertex2(mf.tram.tramBndInnerArr[h].easting, mf.tram.tramBndInnerArr[h].northing);
                }
                GL.End();
            }
        }

        private void DrawNewTrams()
        {
            GL.LineWidth(2);

            GL.Color4(0.97530f, 0.972f, 0.973530f, 1.0);

            if (tramList.Count > 0)
            {
                for (int i = 0; i < tramList.Count; i++)
                {
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < tramList[i].Count; h++)
                    {
                        GL.Vertex2(tramList[i][h].easting, tramList[i][h].northing);
                    }
                    GL.End();
                }
            }
        }

        private void DrawBuiltLines()
        {
            GL.LineStipple(1, 0x0707);
            for (int i = 0; i < gTemp.Count; i++)
            {
                //AB Lines
                if (gTemp[i].mode == TrackMode.AB)
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
                    GL.Vertex2(
                        gTemp[i].ptA.easting - (Math.Sin(gTemp[i].heading) * mf.ABLine.abLength),
                        gTemp[i].ptA.northing - (Math.Cos(gTemp[i].heading) * mf.ABLine.abLength));
                    GL.Vertex2(
                        gTemp[i].ptB.easting + (Math.Sin(gTemp[i].heading) * mf.ABLine.abLength),
                        gTemp[i].ptB.northing + (Math.Cos(gTemp[i].heading) * mf.ABLine.abLength));
                    GL.End();

                    GL.Disable(EnableCap.LineStipple);
                }

                else if (gTemp[i].mode == TrackMode.Curve || gTemp[i].mode == TrackMode.bndCurve)
                {
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineWidth(5);

                    if (gTemp[i].mode == TrackMode.bndCurve) GL.LineStipple(1, 0x0007);
                    else GL.LineStipple(1, 0x0707);


                    if (i == indx)
                    {
                        GL.LineWidth(8);
                        GL.Disable(EnableCap.LineStipple);
                    }

                    GL.Color3(0.30f, 0.97f, 0.30f);
                    if (gTemp[i].mode == TrackMode.bndCurve) GL.Color3(0.70f, 0.5f, 0.2f);
                    GL.Begin(PrimitiveType.LineStrip);
                    foreach (vec3 pts in gTemp[i].curvePts)
                    {
                        GL.Vertex2(pts.easting, pts.northing);
                    }
                    GL.End();

                    GL.Disable(EnableCap.LineStipple);

                    if (i == indx) GL.PointSize(16);
                    else GL.PointSize(8);

                    GL.Color3(1.0f, 0.75f, 0.350f);
                    GL.Begin(PrimitiveType.Points);

                    GL.Vertex2(gTemp[i].curvePts[0].easting, gTemp[i].curvePts[0].northing);

                    GL.Color3(0.5f, 0.5f, 1.0f);
                    GL.Vertex2(
                        gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].easting,
                        gTemp[i].curvePts[gTemp[i].curvePts.Count - 1].northing);
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
            if (_viewport == null)
            {
                CreateViewport();
            }
            _viewport.Resize(oglSelf.Width, oglSelf.Height);
        }

        private void oglSelf_Load(object sender, EventArgs e)
        {
            if (_viewport == null)
            {
                CreateViewport();
            }
        }

        private void CreateViewport()
        {
            if (_viewport == null)
            {
                _viewport = new GeoViewport(mf.FieldBoundingBox, oglSelf);
                _viewport.MouseDownEventHandler += OnViewportMouseDown;
            }
        }

        #endregion

        #region Buttons and misc functions
        private void btnSelectCurve_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();

            if (gTemp.Count > 0)
            {
                indx++;
                if (indx > (gTemp.Count - 1)) indx = 0;
            }
            else
            {
                indx = -1;
            }

            ResetStartNumLabels();
            FixLabelsCurve();
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();

        }

        private void btnSelectCurveBk_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();

            if (gTemp.Count > 0)
            {
                indx--;
                if (indx < 0) indx = gTemp.Count - 1;
            }
            else
            {
                indx = -1;
            }

            FixLabelsCurve();
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
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

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            gTemp[indx].isVisible = !gTemp[indx].isVisible;
            ResetStartNumLabels();
            BuildTram();
        }

        private void btnDeleteAllTrams_Click(object sender, EventArgs e)
        {
            tramList?.Clear();
            tramArr?.Clear();
            mf.tram.tramList?.Clear();
            mf.tram.tramArr?.Clear();

            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();

            ResetStartNumLabels();

            cboxIsOuter.Checked = false;
            BuildTram();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            ptB.easting = 9999999;
            ptB.northing = 9999999;
            ptA.easting = 9999999;
            ptA.northing = 9999999;
            step = 0;
        }

        private void FixLabelsCurve()
        {
            Text = gStr.gsTramLines;
            Text += "    Track: " + (mf.vehicle.VehicleConfig.TrackWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;
            Text += "    Tram: " + (mf.tram.tramWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;
            Text += "    Seed: " + (mf.tool.width * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            if (indx > -1 && gTemp.Count > 0)
            {
                this.Text += "   " + gTemp[indx].name;
                lblCurveSelected.Text = (indx + 1).ToString() + " / " + gTemp.Count.ToString();
            }
            else
            {
                Text += "   Line ***";
                lblCurveSelected.Text = "*";
            }
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            Screen myScreen = Screen.PrimaryScreen;
            Rectangle area = myScreen.WorkingArea;
            Height = area.Height;
            FormTramLine_ResizeEnd(this, e);
        }

        private void btnDnAlpha_Click(object sender, EventArgs e)
        {
            mf.tram.alpha -= 0.1;
            if (mf.tram.alpha < 0.2) mf.tram.alpha = 0.2;
            lblAplha.Text = ((int)(mf.tram.alpha * 100)).ToString();
        }

        private void btnUpAlpha_Click(object sender, EventArgs e)
        {
            mf.tram.alpha += 0.1;
            if (mf.tram.alpha > 1.0) mf.tram.alpha = 1.0;
            lblAplha.Text = ((int)(mf.tram.alpha * 100)).ToString();
        }

        #endregion

        #region Outer Tram
        private void cboxIsOuter_Click(object sender, EventArgs e)
        {
            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();
            if (cboxIsOuter.Checked)
            {
                BuildTramBnd();
            }
            ResetStartNumLabels();
            BuildTram();
        }

        private void BuildTramBnd()
        {
            mf.tram.displayMode = TramDisplayMode.DisplayAll;
            mf.tram.CreateBoundaryOuterTrack();
            mf.tram.CreateBoundaryInnerTrack();
        }

        #endregion

        #region Start And Passes Controls

        private void btnUpTrams_Click(object sender, EventArgs e)
        {
            passes++;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        private void btnDnTrams_Click(object sender, EventArgs e)
        {
            passes--;
            if (passes < 1) passes = 1;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        private void btnUpStartTram_Click(object sender, EventArgs e)
        {
            startPass++;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        private void btnDnStartTram_Click(object sender, EventArgs e)
        {
            startPass--;
            if (startPass < 0) startPass = 0;
            lblStartPass.Text = "Start\r\n" + startPass.ToString();
            lblNumPasses.Text = passes.ToString();
            BuildTram();
        }

        #endregion


    }
}
using AgOpenGPS.Core.Drawing;
using AgOpenGPS.Core.DrawLib;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Core.Visuals;
using AgOpenGPS.Helpers;
using AgOpenGPS.WinForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormBndTool : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;
        private GeoViewport _viewport;

        private static readonly ColorRgba boundaryColor = new ColorRgba(0xffb8f2f2);

        private static readonly ColorRgba newBoundaryStripColor = new ColorRgba(0xffe53f19);
        private static readonly ColorRgba newBoundaryPointsColor = new ColorRgba(0xffe53fe8);
        private static readonly ColorRgba newBoundaryLoopColor = new ColorRgba(0xffd1d47f);

        private static readonly ColorRgba stepSectionColor = new ColorRgba(0xffa3a399);

        private vec3 ptA = new vec3();
        private vec3 ptB = new vec3();
        public vec3 pint = new vec3(0.0, 1.0, 0.0);

        private bool isA = true;
        private bool isC = false;
        private int start = 99999, end = 99999;
        private int bndSelect = 0, smPtsChoose = 1, smPts = 4;

        public List<vec3> secList = new List<vec3>();
        public List<vec3> bndList = new List<vec3>();
        public List<vec3> smooList = new List<vec3>();

        private double minDistSq = 1, minDistDisp = 1;

        private bool isStep = false;

        private double mdA = double.MaxValue;
        private double mdB = double.MaxValue;
        private double mdC = double.MaxValue;
        private double mdD = double.MaxValue;
        private double mdE = double.MaxValue;
        private double mdF = double.MaxValue;
        private double mdG = double.MaxValue;
        private int rA, rB, rC, rD, rE, rF, rG;

        private int firstPoint, currentPoint;

        //find 3 closest points
        private vec3[] arr;

        //baseline to calc the most right vector - starts at 270 deg.
        private double prevHeading = Math.PI + glm.PIBy2;

        public FormBndTool(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();
        }

        private void FormBndTool_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;

            cboxPointDistance.SelectedIndexChanged -= cboxPointDistance_SelectedIndexChanged;
            cboxPointDistance.SelectedIndex = Properties.Settings.Default.bndToolSpacing;
            cboxPointDistance.SelectedIndexChanged += cboxPointDistance_SelectedIndexChanged;

            cboxSmooth.SelectedIndexChanged -= cboxSmooth_SelectedIndexChanged;
            cboxSmooth.SelectedIndex = Properties.Settings.Default.bndToolSmooth;
            cboxSmooth.SelectedIndexChanged += cboxSmooth_SelectedIndexChanged;

            cboxIsZoom.Checked = false;

            Size = Properties.Settings.Default.setWindow_MapBndSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;
            FormBndTool_ResizeEnd(this, e);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            //translate
            labelCreate.Text = gStr.gsCreate;
            labelSmooth.Text = gStr.gsSmooth;
            labelPleaseWait.Text = gStr.gsPleaseWait + "...";
            labelReset.Text = "Reset";
            labelSpacing.Text = $"{gStr.gsSpacing} ({gStr.gsCm})";
            labelPoints.Text = gStr.gsPoints;
            labelPointsToProcess.Text = gStr.gsPointsToProcess;

            //load sections if bnd exists, load bnd if exists
            if (mf.bnd.bndList.Count == 0)
            {
                Reset();
            }
        }

        private void FormBndTool_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setWindow_MapBndSize = Size;
            Properties.Settings.Default.Save();
        }

        private void FormBndTool_ResizeEnd(object sender, EventArgs e)
        {
            Width = (Height * 4 / 3);

            oglSelf.Height = oglSelf.Width = Height - 50;

            oglSelf.Left = 2;
            oglSelf.Top = 2;

            _viewport.Resize(oglSelf.Width, oglSelf.Height);

            tlp1.Width = Width - oglSelf.Width - 4;
            tlp1.Left = oglSelf.Width;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            Top = (area.Height - Height) / 2;
            Left = (area.Width - Width) / 2;
        }

        private void KNN()
        {
            timer1.Enabled = false;
            rA = rB = rC = rD = rE = rF = rG = 0;

            for (int j = 0; j < secList.Count; j++)
            {
                if (j == currentPoint) continue;

                if (arr[j].heading == 1)
                    continue;

                double dist = glm.DistanceSquared(secList[currentPoint], secList[j]);

                if (dist < mdA)
                {
                    mdG = mdF; mdF = mdE; mdE = mdD; mdD = mdC; mdC = mdB; mdB = mdA; mdA = dist; rG = rF; rF = rE; rE = rD; rD = rC; rC = rB; rB = rA; rA = j;
                }
                else if (dist < mdB)
                {
                    rG = rF; rF = rE; rE = rD; rD = rC; rC = rB; rB = j; mdG = mdF; mdF = mdE; mdE = mdD; mdD = mdC; mdC = mdB; mdB = dist;
                }
                else if (dist < mdC)
                {
                    mdG = mdF; mdF = mdE; mdE = mdD; mdD = mdC; mdC = dist; rG = rF; rF = rE; rE = rD; rD = rC; rC = j;
                }
                else if (dist < mdD)
                {
                    mdG = mdF; mdF = mdE; mdE = mdD; mdD = dist; rG = rF; rF = rE; rE = rD; rD = j;
                }
                else if (dist < mdE)
                {
                    mdG = mdF; mdF = mdE; mdE = dist; rG = rF; rF = rE; rE = j;
                }
                else if (dist < mdF)
                {
                    mdG = mdF; mdF = dist; rG = rF; rF = j;
                }
                else if (dist < mdG)
                {
                    mdG = dist; rG = j;
                }
            }

            double aMax = 5;
            double aMin = 1.14;

            double aA = Math.Atan2(secList[rA].easting - secList[currentPoint].easting,
                secList[rA].northing - secList[currentPoint].northing);
            double pA = aA;

            aA -= prevHeading;
            if (aA < 0) aA += glm.twoPI; if (aA < 0) aA += glm.twoPI;
            if (aA > aMax || aA < aMin) aA = 0;

            double aB = Math.Atan2(secList[rB].easting - secList[currentPoint].easting,
                secList[rB].northing - secList[currentPoint].northing);
            double pB = aB;
            aB -= prevHeading;
            if (aB < 0) aB += glm.twoPI; if (aB < 0) aB += glm.twoPI;

            if (aB > aMax || aB < aMin) aB = 0;

            double aC = Math.Atan2(secList[rC].easting - secList[currentPoint].easting,
                secList[rC].northing - secList[currentPoint].northing);
            double pC = aC;

            aC -= prevHeading;
            if (aC < 0) aC += glm.twoPI; if (aC < 0) aC += glm.twoPI;
            if (aC > aMax || aC < aMin) aC = 0;

            double aD = Math.Atan2(secList[rD].easting - secList[currentPoint].easting,
                secList[rD].northing - secList[currentPoint].northing);
            double pD = aD;

            aD -= prevHeading;
            if (aD < 0) aD += glm.twoPI; if (aD < 0) aD += glm.twoPI;
            if (aD > aMax || aD < aMin) aD = 0;

            double aE = Math.Atan2(secList[rE].easting - secList[currentPoint].easting,
                secList[rE].northing - secList[currentPoint].northing);
            double pE = aE;

            aE -= prevHeading;
            if (aE < 0) aE += glm.twoPI; if (aE < 0) aE += glm.twoPI;
            if (aE > aMax || aE < aMin) aE = 0;

            double aF = Math.Atan2(secList[rF].easting - secList[currentPoint].easting,
                secList[rF].northing - secList[currentPoint].northing);
            double pF = aF;

            aF -= prevHeading;
            if (aF < 0) aF += glm.twoPI; if (aF < 0) aF += glm.twoPI;
            if (aF > aMax || aF < aMin) aF = 0;

            double aG = Math.Atan2(secList[rG].easting - secList[currentPoint].easting,
                secList[rG].northing - secList[currentPoint].northing);
            double pG = aG;

            aG -= prevHeading;
            if (aG < 0) aG += glm.twoPI; if (aG < 0) aG += glm.twoPI;
            if (aG > aMax || aG < aMin) aG = 0;

            //double maxAngle = 999;
            //switch (k)
            //{
            //    case 3:
            //        maxAngle = Math.Max(Math.Max(aA, aB), aC);
            //        break;
            //    case 4:
            //        maxAngle = Math.Max(Math.Max(Math.Max(aA, aB), aC), aD);
            //        break;
            //    case 5:
            //        maxAngle = Math.Max(Math.Max(Math.Max(Math.Max(aA, aB), aC), aD), aE);
            //        break;
            //    case 6:
            //        maxAngle = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(aA, aB), aC), aD), aE), aF);
            //        break;
            //    case 7:
            //        maxAngle = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(aA, aB), aC), aD), aE), aF), aG);
            //        break;
            //}

            double maxAngle = Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(Math.Max(aA, aB), aC), aD), aE), aF), aG);

            //remove from list
            arr[currentPoint].heading = 1;

            if (maxAngle == aA) { currentPoint = rA; prevHeading = pA + Math.PI; }
            else if (maxAngle == aB) { currentPoint = rB; prevHeading = pB + Math.PI; }
            else if (maxAngle == aC) { currentPoint = rC; prevHeading = pC + Math.PI; }
            else if (maxAngle == aD) { currentPoint = rD; prevHeading = pD + Math.PI; }
            else if (maxAngle == aE) { currentPoint = rE; prevHeading = pE + Math.PI; }
            else if (maxAngle == aF) { currentPoint = rF; prevHeading = pF + Math.PI; }
            else if (maxAngle == aG) { currentPoint = rG; prevHeading = pG + Math.PI; }

            if (prevHeading >= glm.twoPI) prevHeading -= glm.twoPI;
            if (prevHeading < 0) prevHeading += glm.twoPI;

            mdA = double.MaxValue;
            mdB = double.MaxValue;
            mdC = double.MaxValue;
            mdD = double.MaxValue;
            mdE = double.MaxValue;
            mdF = double.MaxValue;
            mdG = double.MaxValue;

            if (bndList.Count > 7)
            {
                //unhide first point
                arr[firstPoint].heading = 0;

                //are we back to start?
                if (rA == firstPoint || rB == firstPoint || rC == firstPoint ||
                        rD == firstPoint || rE == firstPoint || rF == firstPoint || rG == firstPoint)
                {
                    EndStep();
                    timer1.Interval = 500;
                    timer1.Enabled = true;
                    cboxSmooth.Enabled = true;

                    int bndCount = bndList.Count;

                    for (int i = 0; i < bndCount; i++)
                    {
                        int j = i + 1;

                        if (j == bndCount) j = 0;
                        double distance = glm.Distance(bndList[i], bndList[j]);
                        if (distance > 1.1)
                        {
                            vec3 pointB = new vec3((bndList[i].easting + bndList[j].easting) / 2.0,
                                (bndList[i].northing + bndList[j].northing) / 2.0, bndList[i].heading);

                            bndList.Insert(j, pointB);
                            bndCount = bndList.Count;
                            i--;
                        }
                    }
                    return;
                }
            }

            bndList.Add(new vec3(secList[currentPoint]));
            timer1.Enabled = true;
        }

        private void btnAddPoints_Click(object sender, EventArgs e)
        {
            if (timer1.Interval == 50) return;

            double abHead = Math.Atan2(
                ptB.easting - ptA.easting,
                ptB.northing - ptA.northing);
            //if (abHead < 0) abHead += glm.twoPI;
            //ptA.heading = abHead;
            secList.Add(ptA);
            secList.Add(ptB);

            int dist = (int)(glm.Distance(ptA, ptB));

            if (dist > 2)
            {
                for (int i = 1; i < dist; i++)
                {
                    vec3 pt = new vec3(ptA);
                    pt.easting += (Math.Sin(abHead) * i);
                    pt.northing += (Math.Cos(abHead) * i);
                    secList.Add(pt);
                }
            }

            btnAddPoints.Enabled = false;

            //update the arrays
            start = 99999; end = 99999;
            btnExit.Focus();
            isC = false;
            isA = true;

            btnAddPoints.Enabled = false;
        }

        private void btnResetReduce_Click(object sender, EventArgs e)
        {
            Reset();
        }

        private void Reset()
        {
            btnSlice.Visible = false;

            //start all over
            start = end = 99999;
            _viewport.ResetZoomPan();

            btnStartStop.Enabled = true;

            EndStep();
            timer1.Interval = 500;
            prevHeading = Math.PI + glm.PIBy2;

            secList?.Clear();
            bndList?.Clear();
            smooList?.Clear();

            DeleteBoundary();

            //for every new chunk of patch
            for (int j = 0; j < mf.triStrip.Count; j++)
            {
                //every time the section turns off and on is a new patch
                int patchCount = mf.triStrip[j].patchList.Count;

                if (patchCount > 0)
                {
                    //for every new chunk of patch
                    foreach (var triList in mf.triStrip[j].patchList)
                    {
                        for (int i = 1; i < triList.Count; i++)
                        {
                            vec3 bob = new vec3(triList[i].easting, triList[i].northing, 0);

                            secList.Add(bob);
                        }
                    }
                }
            }

            rA = rB = rC = rD = rE = rF = rG = firstPoint = currentPoint = 0;
            bndList?.Clear();

            btnStartStop.BackColor = Color.LightGreen;

            cboxPointDistance.SelectedIndexChanged -= cboxPointDistance_SelectedIndexChanged;
            cboxPointDistance.SelectedIndex = Properties.Settings.Default.bndToolSpacing;
            cboxPointDistance.SelectedIndexChanged += cboxPointDistance_SelectedIndexChanged;

            cboxSmooth.SelectedIndexChanged -= cboxSmooth_SelectedIndexChanged;
            cboxSmooth.SelectedIndex = Properties.Settings.Default.bndToolSmooth;
            cboxSmooth.SelectedIndexChanged += cboxSmooth_SelectedIndexChanged;
        }

        private void Spacing()
        {
            if (cboxPointDistance.SelectedIndex == 10) return;
            timer1.Interval = 500;
            EndStep();

            minDistDisp = (double)(cboxPointDistance.SelectedIndex + 1);
            minDistSq = minDistDisp * minDistDisp;

            rA = rB = rC = rD = rE = rF = rG = firstPoint = currentPoint = 0;

            vec3[] arr = new vec3[secList.Count];
            secList.CopyTo(arr);

            int cntr = 0;

            lblPointToProcess.Text = secList.Count.ToString();

            panel1.Visible = true;

            for (int i = 0; i < secList.Count; i++)
            {
                //already checked
                if (arr[i].heading > 0)
                    continue;

                for (int j = 0; j < secList.Count; j++)
                {
                    if (j == i) continue;
                    //if (arr[j].heading != 0) continue;

                    if (arr[j].heading == 0)
                    {
                        double dist = glm.DistanceSquared(secList[i], secList[j]);
                        if (dist < minDistSq)
                        {
                            //means delete this point
                            arr[j].heading = 1;
                        }
                    }
                }

                //points all around it are removed or > minDist
                arr[i].heading = 2;

                cntr++;

                if (cntr > 200)
                {
                    lblI.Text = i.ToString();
                    panel1.Refresh();
                    cntr = 0;
                }
            }

            panel1.Visible = false;

            secList?.Clear();
            foreach (var item in arr)
            {
                //0 will mean visible
                if (item.heading == 2) secList.Add(new vec3(item.easting, item.northing, 0));
            }

            //Find most South point
            double minny = double.MaxValue;

            for (int j = 0; j < secList.Count; j++)
            {
                if (minny > secList[j].northing)
                {
                    firstPoint = j;
                    minny = secList[j].northing;
                }
            }

            btnStartStop.BackColor = Color.LightGreen;
            btnStartStop.Enabled = true;
        }

        private void PacMan()
        {
            btnStartStop.Enabled = false;
            if (secList.Count < 20)
            {
                mf.YesMessageBox("Not enough points to make a boundary");
                return;
            }

            arr = new vec3[secList.Count];
            prevHeading = Math.PI + glm.PIBy2;

            //find most southerly - lowest Y point
            double minny = double.MaxValue;
            for (int j = 0; j < secList.Count; j++)
            {
                if (minny > secList[j].northing)
                {
                    firstPoint = j;
                    minny = secList[j].northing;
                }
            }

            //keep firstPoint
            currentPoint = firstPoint;

            //first point of bnd
            bndList?.Clear();
            bndList.Add(new vec3(secList[currentPoint]));

            secList.CopyTo(arr);

            isStep = !isStep;


            if (isStep)
            {
                timer1.Interval = 50;
            }
            else
            {
                timer1.Interval = 500;
                EndStep();
            }
            btnStartStop.BackColor = Color.WhiteSmoke;

            //btnStartStop.Enabled = false;
        }

        private void Smooth()
        {
            if (cboxSmooth.SelectedIndex == 6) return;

            smPtsChoose = cboxSmooth.SelectedIndex;

            if (smPtsChoose == 0)
            {
                smPts = 0;
                cboxSmooth.Text = smPts.ToString();
            }
            else
            {
                smPts = 2;
                for (int i = 1; i <= smPtsChoose; i++)
                    smPts *= 2;
                cboxSmooth.Text = smPts.ToString();
            }
            SmoothList();
        }

        private void BuildBnd()
        {
            if (smooList.Count == 0) return;

            if (smooList.Count > 5)
            {
                secList?.Clear();

                //just in case
                DeleteBoundary();

                CBoundaryList New = new CBoundaryList();

                for (int i = 0; i < smooList.Count; i++)
                {
                    New.fenceLine.Add(new vec3(smooList[i]));
                }

                New.CalculateFenceArea(0);
                New.FixFenceLine(0);
                mf.bnd.bndList.Add(New);
                smooList.Clear();
                bndList?.Clear();

                //turn lines made from boundaries
                mf.CalculateMinMax();
                mf.bnd.BuildTurnLines();

                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.FileSaveBoundary();
            }

            btnStartStop.Enabled = false;

            btnSlice.Visible = true;
            btnCancelTouch.Visible = true;
            btnZoomIn.Visible = true;
            btnZoomOut.Visible = true;
        }

        private void SmoothList()
        {
            secList?.Clear();

            //just go back if not very long
            if (bndList.Count < 20) return;

            int cnt = bndList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            if (smPts != 0)
            {

                //read the points before and after the setpoint
                for (int s = 0; s < smPts / 2; s++)
                {
                    arr[s].easting = bndList[s].easting;
                    arr[s].northing = bndList[s].northing;
                    arr[s].heading = bndList[s].heading;
                }

                for (int s = cnt - (smPts / 2); s < cnt; s++)
                {
                    arr[s].easting = bndList[s].easting;
                    arr[s].northing = bndList[s].northing;
                    arr[s].heading = bndList[s].heading;
                }

                //average them - center weighted average
                for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
                {
                    for (int j = -smPts / 2; j < smPts / 2; j++)
                    {
                        arr[i].easting += bndList[j + i].easting;
                        arr[i].northing += bndList[j + i].northing;
                    }
                    arr[i].easting /= smPts;
                    arr[i].northing /= smPts;
                    arr[i].heading = bndList[i].heading;
                }
            }
            else
            {
                for (int i = 0; i < bndList.Count; i++)
                {
                    arr[i] = bndList[i];
                }
            }

            //make a list to draw
            smooList?.Clear();

            if (arr == null || cnt < 1) return;
            if (smooList == null) return;

            for (int i = 0; i < cnt; i++)
            {
                smooList.Add(arr[i]);
            }

            CABCurve.CalculateHeadings(ref smooList);

            List<vec3> smList = new List<vec3>();

            for (int i = 0; i < smooList.Count; i++)
            {
                smList.Add(new vec3(smooList[i]));
            }
            double delta = 0;
            smooList?.Clear();

            for (int i = 0; i < smList.Count; i++)
            {
                if (i == 0)
                {
                    smooList.Add(new vec3(smList[i]));
                    continue;
                }
                delta += (smList[i - 1].heading - smList[i].heading);
                if (Math.Abs(delta) > 0.02)
                {
                    smooList.Add(new vec3(smList[i]));
                    delta = 0;
                }
            }

            int bndCount = smooList.Count;

            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;

                if (j == bndCount) j = 0;
                double distance = glm.Distance(smooList[i], smooList[j]);
                if (distance > 1.6)
                {
                    vec3 pointB = new vec3((smooList[i].easting + smooList[j].easting) / 2.0,
                        (smooList[i].northing + smooList[j].northing) / 2.0, smooList[i].heading);

                    smooList.Insert(j, pointB);
                    bndCount = smooList.Count;
                    i--;
                }
            }
            CABCurve.CalculateHeadings(ref smooList);
        }

        private void DeleteBoundary()
        {
            mf.bnd.bndList?.Clear();
            mf.FileSaveBoundary();
            mf.fd.UpdateFieldBoundaryGUIAreas();
            mf.FileSaveHeadland();
        }

        private void btnStartStop_Click(object sender, EventArgs e)
        {
            Spacing();

            PacMan();
        }

        private void btnAddBoundary_Click(object sender, EventArgs e)
        {
            if (timer1.Interval == 50) return;
            if (bndList.Count < 10) return;

            Smooth();

            BuildBnd();
        }

        private void cboxSmooth_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bndToolSmooth = cboxSmooth.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void cboxPointDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.bndToolSpacing = cboxPointDistance.SelectedIndex;
            Properties.Settings.Default.Save();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            _viewport.ZoomOutStep();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            _viewport.ZoomInStep();
        }

        private void btnMoveDn_Click(object sender, EventArgs e)
        {
            _viewport.PanDown();
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            _viewport.PanUp();
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            _viewport.PanLeft();
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            _viewport.PanRight();
        }

        private void btnResetReduce_Click_1(object sender, EventArgs e)
        {
            Reset();
        }

        private void btnSlice_Click(object sender, EventArgs e)
        {
            bool isLoop = false;
            int limit = end;
            if (end == 99999 || start == 99999) return;

            if (bndSelect >= 0 && bndSelect < mf.bnd.bndList.Count && mf.bnd.bndList[bndSelect].fenceLine.Count > 0)
            {
                if ((Math.Abs(start - end)) > (mf.bnd.bndList[bndSelect].fenceLine.Count * 0.5))
                {
                    isLoop = true;
                    if (start < end)
                    {
                        (end, start) = (start, end);
                    }

                    limit = end;
                    end = mf.bnd.bndList[bndSelect].fenceLine.Count;
                }
                else //normal
                {
                    if (start > end)
                    {
                        (end, start) = (start, end);
                    }
                }

                vec3[] arr = new vec3[mf.bnd.bndList[bndSelect].fenceLine.Count];
                mf.bnd.bndList[bndSelect].fenceLine.CopyTo(arr);

                if (start++ == arr.Length) start--;
                //if (end-- == -1) end = 0;
                if (start == end) return;

                for (int i = start; i < end; i++)
                {
                    //calculate the point inside the boundary
                    arr[i].heading = 999;

                    if (isLoop && i == mf.bnd.bndList[bndSelect].fenceLine.Count - 1)
                    {
                        i = -1;
                        isLoop = false;
                        end = limit;
                    }
                }

                if (isC)
                    arr[start] = new vec3(pint);

                mf.bnd.bndList[bndSelect].fenceLine.Clear();

                for (int i = 0; i < arr.Length; i++)
                {
                    //calculate the point inside the boundary
                    if (arr[i].heading != 999)
                        mf.bnd.bndList[bndSelect].fenceLine.Add(new vec3(arr[i]));

                    if (isLoop && i == arr.Length - 1)
                    {
                        i = -1;
                        isLoop = false;
                        end = limit;
                    }
                }

                mf.bnd.bndList[bndSelect].FixFenceLine(bndSelect);

                mf.CalculateMinMax();
                mf.bnd.BuildTurnLines();

                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.FileSaveBoundary();
            }

            start = 99999; end = 99999;
            isA = true;
            isC = false;
            _viewport.Refresh();
        }

        private void btnCenterOGL_Click(object sender, EventArgs e)
        {
            _viewport.ResetZoomPan();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            start = 99999; end = 99999;
            btnExit.Focus();
            isC = false;
            isA = true;

            btnAddPoints.Enabled = false;
            _viewport.Refresh();
        }

        private void OnViewportMouseDown(object sender, GeoCoord mouseDownCoord)
        {
            if (cboxIsZoom.Checked)
            {
                _viewport.PointZoom(mouseDownCoord, 0.125);
                cboxIsZoom.Checked = false;
                return;
            }

            pint = new vec3(mouseDownCoord);

            if (mf.bnd.bndList.Count != 0)
            {
                if (start != 99999 & end != 99999)
                {
                    isC = true;
                    return;
                }
            }

            if (isA)
            {
                double minDistA = double.MaxValue;
                start = 99999; end = 99999;
                if (mf.bnd.bndList.Count != 0)
                {
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
                }
                else
                {
                    start = 1;
                    ptA = pint;
                    btnAddPoints.Enabled = false;
                }

                isA = false;
            }
            else
            {
                double minDistA = double.MaxValue;
                int j = bndSelect;

                if (mf.bnd.bndList.Count != 0)
                {

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
                }
                else
                {
                    end = 1;
                    ptB = pint;
                    btnAddPoints.Enabled = true;
                }
                isA = true;
            }
        }

        private void oglSelf_Paint(object sender, PaintEventArgs e)
        {
            if (isStep)
            {
                KNN();
                _viewport.PointZoom(secList[currentPoint].ToGeoCoord(), 0.5);
            }
            _viewport.BeginPaint();

            //draw all the boundaries
            if (mf.bnd.bndList.Count > 0)
            {
                // outter boundary
                GLW.SetLineWidth(2.0f);
                GLW.SetColor(boundaryColor);
                GeoCoord[] fenceLineArray = GeoRefactorHelper.ToGeoCoordArray(mf.bnd.bndList[0].fenceLine);
                GLW.DrawLineLoopPrimitive(fenceLineArray);

                GLW.SetPointSize(4.0f);
                GLW.DrawPointsPrimitive(fenceLineArray);

                if (mf.bnd.bndList.Count > 1)
                {
                    //inner boundaries
                    for (int i = 1; i < mf.bnd.bndList.Count; i++)
                    {
                        GLW.SetLineWidth(2.0f);
                        GLW.SetColor(boundaryColor); //Change color to something else than main boundary color, maybe yellow?
                        GeoCoord[] innerFenceLineArray = GeoRefactorHelper.ToGeoCoordArray(mf.bnd.bndList[i].fenceLine);
                        GLW.DrawLineLoopPrimitive(innerFenceLineArray);

                        GLW.SetPointSize(4.0f);
                        GLW.DrawPointsPrimitive(innerFenceLineArray);
                    }
                }
            }

            //new boundary being made
            if (bndList.Count > 0)
            {
                GeoCoord[] boundaryArray = GeoRefactorHelper.ToGeoCoordArray(bndList);
                GLW.SetLineWidth(2.0f);
                GLW.SetColor(newBoundaryStripColor);
                GLW.DrawLineStripPrimitive(boundaryArray);

                GLW.SetPointSize(4.0f);
                GLW.SetColor(newBoundaryPointsColor);
                GLW.DrawPointsPrimitive(boundaryArray);

                GLW.SetColor(newBoundaryLoopColor);
                GLW.DrawLineLoopPrimitive(GeoRefactorHelper.ToGeoCoordArray(smooList));
            }

            //the section grid if loaded
            if (secList.Count > 0)
            {
                GLW.SetPointSize(2.0f);
                GLW.SetColor(isStep ? stepSectionColor : Colors.Yellow);
                GLW.DrawPointsPrimitive(GeoRefactorHelper.ToGeoCoordArray(secList));
            }
            DrawTouchPointsLine();
            _viewport.EndPaint();
        }

        private void DrawTouchPointsLine()
        {
            GeoCoord? coordA = null;
            GeoCoord? coordB = null;
            GeoCoord? coordC = null;
            if (start != 99999)
            {
                coordA = ((mf.bnd.bndList.Count != 0) ? mf.bnd.bndList[bndSelect].fenceLine[start] : ptA).ToGeoCoord();
            }
            if (end != 99999)
            {
                coordB = ((mf.bnd.bndList.Count != 0) ? mf.bnd.bndList[bndSelect].fenceLine[end] : ptB).ToGeoCoord();
            }
            if (isC)
            {
                coordC = pint.ToGeoCoord();
            }
            TouchPointsLineVisual.DrawTouchPointsLine(coordA, coordB, coordC);
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
            _viewport = new GeoViewport(mf.FieldBoundingBox, oglSelf);
            _viewport.MouseDownEventHandler += OnViewportMouseDown;
        }

        private void EndStep()
        {
            isStep = false;
            _viewport.ResetZoomPan();
        }
    }
}
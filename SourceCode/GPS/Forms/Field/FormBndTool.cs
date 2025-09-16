using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Helpers;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;
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

        private Point fixPt;
        private vec3 ptA = new vec3();
        private vec3 ptB = new vec3();

        private bool isA = true;
        private bool isC = false;
        private int start = 99999, end = 99999;
        private int bndSelect = 0, smPtsChoose = 1, smPts = 4;

        private double zoom = 1, sX = 0, sY = 0;

        public vec3 pint = new vec3(0.0, 1.0, 0.0);

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

            mf.CalculateMinMax();
        }

        private void FormBndTool_Load(object sender, EventArgs e)
        {
            panel1.Visible = false;
            //translate
            labelCreate.Text = gStr.gsCreate;
            labelSmooth.Text = gStr.gsSmooth;
            labelPleaseWait.Text = gStr.gsPleaseWait;
            labelReducedPoints.Text = gStr.gsReducedPoints;
            labelSpacing.Text = gStr.gsSpacing;
            labelPoints.Text = gStr.gsPoints;
            labelPointsToProcess.Text = gStr.gsPointsToProcess;


            //already have a boundary
            if (mf.bnd.bndList.Count == 0)
            {
                //draw patches j= # of sections
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
            }
            else
            {

            }

            cboxPointDistance.SelectedIndexChanged -= cboxPointDistance_SelectedIndexChanged;
            cboxPointDistance.Text = "?";
            cboxPointDistance.SelectedIndexChanged += cboxPointDistance_SelectedIndexChanged;

            cboxSmooth.SelectedIndexChanged -= cboxSmooth_SelectedIndexChanged;
            cboxSmooth.Text = "?";
            cboxSmooth.SelectedIndexChanged += cboxSmooth_SelectedIndexChanged;
            cboxIsZoom.Checked = false;

            Size = Properties.Settings.Default.setWindow_MapBndSize;

            Screen myScreen = Screen.FromControl(this);
            Rectangle area = myScreen.WorkingArea;

            this.Top = (area.Height - this.Height) / 2;
            this.Left = (area.Width - this.Width) / 2;
            FormBndTool_ResizeEnd(this, e);

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
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
                    isStep = false;
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

        private void DeleteBoundary()
        {
            mf.bnd.bndList?.Clear();
            mf.FileSaveBoundary();
            mf.fd.UpdateFieldBoundaryGUIAreas();
            mf.FileSaveHeadland();
        }

        private void btnAddPoints_Click(object sender, EventArgs e)
        {
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
            cboxIsZoom.Visible = false;
            btnSlice.Visible = false;
            btnCenterOGL.Visible = false;
            //btnCancelTouch.Visible = false;
            btnZoomIn.Visible = false;
            btnZoomOut.Visible = false;
            btnMoveDn.Visible = false;
            btnMoveUp.Visible = false;
            btnMoveLeft.Visible = false;
            btnMoveRight.Visible = false;

            //start all over
            start = end = 99999;
            zoom = 1;
            sX = 0;
            sY = 0;

            btnStartStop.Enabled = false;
            cboxPointDistance.Enabled = false;
            cboxSmooth.Enabled = false;
            btnMakeBoundary.Enabled = false;

            if (mf.bnd.bndList.Count > 0)
            {
                // Show custom confirmation dialog for boundary deletion
                DialogResult result3 = FormDialog.Show(
                    gStr.gsDeleteForSure,
                    gStr.gsDeleteBoundaryMapping,
                    MessageBoxButtons.YesNo);

                if (result3 != DialogResult.OK)
                {
                    return;
                }
            }

            DeleteBoundary();


            isStep = false;
            timer1.Interval = 500;
            prevHeading = Math.PI + glm.PIBy2;

            secList?.Clear();
            bndList?.Clear();
            smooList?.Clear();

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

            labelReducedPoints.Text = secList.Count.ToString();

            rA = rB = rC = rD = rE = rF = rG = firstPoint = currentPoint = 0;
            bndList?.Clear();

            btnStartStop.BackColor = Color.OrangeRed;

            cboxPointDistance.Enabled = true;

            cboxPointDistance.SelectedIndexChanged -= cboxPointDistance_SelectedIndexChanged;
            cboxPointDistance.Text = "?";
            cboxPointDistance.SelectedIndexChanged += cboxPointDistance_SelectedIndexChanged;

            cboxSmooth.SelectedIndexChanged -= cboxSmooth_SelectedIndexChanged;
            cboxSmooth.Text = "?";
            cboxSmooth.SelectedIndexChanged += cboxSmooth_SelectedIndexChanged;

        }

        private void btnMakeBoundary_Click(object sender, EventArgs e)
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
            cboxPointDistance.Enabled = false;
            cboxSmooth.Enabled = false;
            btnMakeBoundary.Enabled = false;

            cboxIsZoom.Visible = true;
            btnSlice.Visible = true;
            btnCenterOGL.Visible = true;
            btnCancelTouch.Visible = true;
            btnZoomIn.Visible = true;
            btnZoomOut.Visible = true;

            btnMoveDn.Visible = false;
            btnMoveUp.Visible = false;
            btnMoveLeft.Visible = false;
            btnMoveRight.Visible = false;
        }

        private void cboxSmooth_SelectedIndexChanged(object sender, EventArgs e)
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

        private void cboxPointDistance_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboxPointDistance.SelectedIndex == 10) return;
            timer1.Interval = 500;
            isStep = false;
            cboxPointDistance.Enabled = false;

            minDistDisp = (double)(cboxPointDistance.SelectedIndex + 1);
            minDistSq = minDistDisp * minDistDisp;


            rA = rB = rC = rD = rE = rF = rG = firstPoint = currentPoint = 0;

            //secList?.Clear();

            //for (int j = 0; j < mf.triStrip.Count; j++)
            //{
            //    //every time the section turns off and on is a new patch
            //    int patchCount = mf.triStrip[j].patchList.Count;

            //    if (patchCount > 0)
            //    {
            //        //for every new chunk of patch
            //        foreach (var triList in mf.triStrip[j].patchList)
            //        {
            //            for (int i = 1; i < triList.Count; i++)
            //            {
            //                vec3 bob = new vec3(triList[i].easting, triList[i].northing, 0);

            //                secList.Add(bob);
            //            }
            //        }
            //    }
            //}

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

            labelReducedPoints.Text = secList.Count.ToString();

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

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            zoom += 0.1;
            if (zoom > 1) zoom = 1;
        }

        private void btnMoveDn_Click(object sender, EventArgs e)
        {
            if (zoom == 0.1)
                sY += 0.01;
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (zoom == 0.1)
                sY -= 0.01;
        }

        private void btnMoveLeft_Click(object sender, EventArgs e)
        {
            if (zoom == 0.1)
                sX += 0.01;
        }

        private void btnMoveRight_Click(object sender, EventArgs e)
        {
            if (zoom == 0.1)
                sX -= 0.01;
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            zoom -= 0.1;
            if (zoom < 0.1) zoom = 0.1;
        }

        private void btnStartStop_Click(object sender, EventArgs e)
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

            if (isStep) timer1.Interval = 50;
            else timer1.Interval = 500;
            btnStartStop.BackColor = Color.WhiteSmoke;
            //btnStartStop.Enabled = false;
            cboxSmooth.Enabled = false;
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

            mf.curve.CalculateHeadings(ref smooList);

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
                if (Math.Abs(delta) > 0.05)
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

            mf.curve.CalculateHeadings(ref smooList);

            btnMakeBoundary.Enabled = true;
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

                vec3[] arr = new vec3[mf.bnd.bndList[0].fenceLine.Count];
                mf.bnd.bndList[0].fenceLine.CopyTo(arr);

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

                mf.bnd.bndList[0].FixFenceLine(0);

                mf.CalculateMinMax();
                mf.bnd.BuildTurnLines();

                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.FileSaveBoundary();
            }

            start = 99999; end = 99999;
            isA = true;
            isC = false;
            //zoom = 1;
            //sX = 0;
            //sY = 0;
        }

        private void btnCenterOGL_Click(object sender, EventArgs e)
        {
            zoom = 1;
            sX = 0;
            sY = 0;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnCancelTouch_Click(object sender, EventArgs e)
        {
            //update the arrays
            start = 99999; end = 99999;
            btnExit.Focus();
            isC = false;
            isA = true;

            btnAddPoints.Enabled = false;
        }

        private void oglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            Point ptt = oglSelf.PointToClient(Cursor.Position);

            int wid = oglSelf.Width;
            int halfWid = oglSelf.Width / 2;
            double scale = (double)wid * 0.903;

            if (cboxIsZoom.Checked)
            {
                sX = ((halfWid - (double)ptt.X) / wid) * 1.1;
                sY = ((halfWid - (double)ptt.Y) / -wid) * 1.1;
                zoom = 0.1;
                cboxIsZoom.Checked = false;
                return;
            }

            //if (mf.bnd.bndList.Count < 1) { return; }

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
            if (isStep) KNN();


            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            if (isStep)
            {
                //back the camera up
                GL.Translate(0, 0, -mf.maxFieldDistance * 0.5);

                //translate to that spot in the world
                GL.Translate(-secList[currentPoint].easting, -secList[currentPoint].northing, 0);
            }
            else
            {
                //back the camera up
                GL.Translate(0, 0, -mf.maxFieldDistance * zoom);

                //translate to that spot in the world
                GL.Translate(-mf.fieldCenterX + sX * mf.maxFieldDistance, -mf.fieldCenterY + sY * mf.maxFieldDistance, 0);
            }

            //draw all the boundaries

            GL.LineWidth(2);

            GL.Color3(0.725f, 0.95f, 0.950f);

            if (mf.bnd.bndList.Count > 0)
            {
                GL.Begin(PrimitiveType.LineLoop);
                for (int i = 0; i < mf.bnd.bndList[0].fenceLine.Count; i++)
                {
                    GL.Vertex3(mf.bnd.bndList[0].fenceLine[i].easting, mf.bnd.bndList[0].fenceLine[i].northing, 0);
                }
                GL.End();

                GL.PointSize(4);
                GL.Begin(PrimitiveType.Points);
                for (int i = 0; i < mf.bnd.bndList[0].fenceLine.Count; i++)
                {
                    GL.Vertex3(mf.bnd.bndList[0].fenceLine[i].easting, mf.bnd.bndList[0].fenceLine[i].northing, 0);
                }
                GL.End();
            }

            //new boundary being made
            if (bndList.Count > 0)
            {
                GL.LineWidth(2);
                GL.Color3(0.90f, 0.25f, 0.10f);
                GL.Begin(PrimitiveType.LineStrip);
                for (int i = 0; i < bndList.Count; i++)
                {
                    GL.Vertex3(bndList[i].easting, bndList[i].northing, 0);
                }
                GL.End();

                GL.PointSize(4);
                GL.Color3(0.90f, 0.25f, 0.910f);
                GL.Begin(PrimitiveType.Points);
                for (int i = 0; i < bndList.Count; i++)
                {
                    GL.Vertex3(bndList[i].easting, bndList[i].northing, 0);
                }
                GL.End();


                GL.Color3(0.82f, 0.835f, 0.5f);
                GL.Begin(PrimitiveType.LineLoop);
                for (int j = 0; j < smooList.Count; j++)
                {
                    GL.Vertex3(smooList[j].easting, smooList[j].northing, 0);
                }
                GL.End();

            }


            //the section grid if loaded
            if (secList.Count > 0)
            {
                GL.PointSize(2);
                if (isStep)
                    GL.Color3(0.64f, 0.64f, 0.6);
                else
                    GL.Color3(1.0f, 1.0f, 0);

                GL.Begin(PrimitiveType.Points);

                for (int j = 0; j < secList.Count; j++)
                    GL.Vertex3(secList[j].easting, secList[j].northing, 0);
                GL.End();

                //GL.PointSize(24);
                //GL.Color3(0.90f, 0.25f, 0.910f);
                //GL.Begin(PrimitiveType.Points);
                //    GL.Vertex3(0,0, 0);
                //GL.End();
            }

            //draw the line building graphics
            if (start != 99999 || end != 99999) DrawABTouchPoints();

            if (mf.bnd.bndList.Count != 0)
            {
                //draw the actual built lines
                if (start != 99999 && end != 99999)
                {
                    if (isC)
                    {
                        GL.LineWidth(4);
                        GL.Color3(0.90f, 0.5f, 0.25f);
                        GL.Begin(PrimitiveType.LineStrip);
                        {
                            GL.Vertex3(mf.bnd.bndList[0].fenceLine[start].easting, mf.bnd.bndList[0].fenceLine[start].northing, 0);
                            GL.Vertex3(pint.easting, pint.northing, 0);
                            GL.Vertex3(mf.bnd.bndList[0].fenceLine[end].easting, mf.bnd.bndList[0].fenceLine[end].northing, 0);
                        }
                        GL.End();
                    }
                    else
                    {
                        GL.LineWidth(4);
                        GL.Color3(0.90f, 0.5f, 0.25f);
                        GL.Begin(PrimitiveType.Lines);
                        {
                            GL.Vertex3(mf.bnd.bndList[0].fenceLine[start].easting, mf.bnd.bndList[0].fenceLine[start].northing, 0);
                            GL.Vertex3(mf.bnd.bndList[0].fenceLine[end].easting, mf.bnd.bndList[0].fenceLine[end].northing, 0);
                        }
                        GL.End();

                    }
                }
            }

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void DrawABTouchPoints()
        {
            GL.PointSize(24);
            GL.Begin(PrimitiveType.Points);

            if (mf.bnd.bndList.Count != 0)
            {
                GL.Color3(0, 0, 0);
                if (start != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);
                if (end != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
                GL.End();

                GL.PointSize(16);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(.950f, 0.75f, 0.50f);
                if (start != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[start].easting, mf.bnd.bndList[bndSelect].fenceLine[start].northing, 0);

                GL.Color3(0.5f, 0.5f, 0.935f);
                if (end != 99999) GL.Vertex3(mf.bnd.bndList[bndSelect].fenceLine[end].easting, mf.bnd.bndList[bndSelect].fenceLine[end].northing, 0);
            }
            else
            {
                GL.Color3(0, 0, 0);
                if (start != 99999) GL.Vertex3(ptA.easting, ptA.northing, 0);
                if (end != 99999) GL.Vertex3(ptB.easting, ptB.northing, 0);
                GL.End();

                GL.PointSize(16);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(.950f, 0.75f, 0.50f);
                if (start != 99999) GL.Vertex3(ptA.easting, ptA.northing, 0);

                GL.Color3(0.5f, 0.5f, 0.935f);
                if (end != 99999) GL.Vertex3(ptB.easting, ptB.northing, 0);
            }
            if (isC)
            {
                GL.Color3(0.95f, 0.95f, 0.35f);
                GL.Vertex3(pint.easting, pint.northing, 0);
            }

            GL.End();



        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            oglSelf.Refresh();

            if (timer1.Interval == 500)
            {
                if (zoom == 0.1)
                {
                    btnMoveDn.Visible = true;
                    btnMoveUp.Visible = true;
                    btnMoveLeft.Visible = true;
                    btnMoveRight.Visible = true;
                }
                else
                {
                    btnMoveDn.Visible = false;
                    btnMoveUp.Visible = false;
                    btnMoveLeft.Visible = false;
                    btnMoveRight.Visible = false;
                }
            }
        }

        private void oglSelf_Resize(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
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
    }
}
using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace AgOpenGPS
{
    public partial class FormBuildTracks : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double aveLineHeading;
        private int originalLine = 0;
        private bool isClosing;
        private int selectedItem = -1;
        public List<CTrk> gTemp = new List<CTrk>();

        private bool isRefRightSide = true; //left side 0 middle 1 right 2
        TrackMode mode = TrackMode.None;
        private vec2 ptAa = new vec2();
        private vec2 ptBb = new vec2();

        private bool isOn = true;

        //used throughout to acces the master Track list
        private int idx;

        public FormBuildTracks(Form _mf)
        {
            mf = _mf as FormGPS;
            InitializeComponent();

            //btnPausePlay.Text = gStr.gsPause;
            this.Text = gStr.gsTracks;
            labelABLine.Text = gStr.gsABline;
            labelCurve.Text = gStr.gsCurve;
            labelAPlus.Text = gStr.gsAPlus;
            labelABLine.Text = gStr.gsABline;
            labelABLine2.Text = gStr.gsABline;
            labelABCurve.Text = gStr.gsCurve;
            labelCurve2.Text = gStr.gsCurve;
            labelEditName.Text = gStr.gsEnterName;
            labelEnterName.Text = gStr.gsEnterName;
            labelLatLon.Text = gStr.gsLatLon;
            labelLatLonHeading.Text = gStr.gsLatLon + " " + gStr.gsHeading;
            labelLatitude.Text = gStr.gsLatitude;
            labelLongtitude.Text = gStr.gsLongtitude;
            labelPivot.Text = gStr.gsPivot;
            labelHeading.Text = gStr.gsHeading;
            labelLatitudeA.Text = gStr.gsLatitude + " A";
            labelLongtitudeA.Text = gStr.gsLongtitude + " A";
            labelLatitudeB.Text = gStr.gsLatitude + " B";
            labelLongtitudeB.Text = gStr.gsLongtitude + "B";
            labelStatus.Text = gStr.gsStatus + ":";

        }

        private void FormBuildTracks_Load(object sender, EventArgs e)
        {
            idx = mf.trk.gArr.Count - 1;

            gTemp.Clear();

            foreach (var item in mf.trk.gArr)
            {
                gTemp.Add(new CTrk(item));
            }

            panelMain.Top = 3; panelMain.Left = 3;
            panelCurve.Top = 3; panelCurve.Left = 3;
            panelName.Top = 3; panelName.Left = 3;
            panelKML.Top = 3; panelKML.Left = 3;
            panelEditName.Top = 3; panelEditName.Left = 3;
            panelChoose.Top = 3; panelChoose.Left = 3;
            panelABLine.Top = 3; panelABLine.Left = 3;
            panelAPlus.Top = 3; panelAPlus.Left = 3;
            panelLatLonPlus.Top = 3; panelLatLonPlus.Left = 3;
            panelLatLonLatLon.Top = 3; panelLatLonLatLon.Left = 3;
            panelPivot.Top = 3; panelPivot.Left = 3;

            panelEditName.Visible = false;
            panelMain.Visible = true;
            panelCurve.Visible = false;
            panelName.Visible = false;
            panelKML.Visible = false;
            panelChoose.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;
            panelLatLonPlus.Visible = false;
            panelLatLonLatLon.Visible = false;
            panelPivot.Visible = false;

            this.Size = new System.Drawing.Size(650, 480);

            originalLine = mf.trk.idx;

            selectedItem = -1;
            Location = Properties.Settings.Default.setWindow_buildTracksLocation;

            nudLatitudeA.Controls[0].Enabled = false;
            nudLongitudeA.Controls[0].Enabled = false;
            nudLatitudeB.Controls[0].Enabled = false;
            nudLatitudeB.Controls[0].Enabled = false;
            nudHeading.Controls[0].Enabled = false;
            nudLatitudePlus.Controls[0].Enabled = false;
            nudLongitudePlus.Controls[0].Enabled = false;
            nudHeadingLatLonPlus.Controls[0].Enabled = false;

            nudLatitudeA.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLatitudeB.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude + 0.000005m;
            nudLongitudeA.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
            nudLongitudeB.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude + 0.000005m;
            nudLatitudePlus.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudePlus.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
            nudHeading.Value = 0;
            nudHeadingLatLonPlus.Value = 0;

            UpdateTable();

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormBuildTracks_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!isClosing)
            {
                e.Cancel = true;
                return;
            }

            Properties.Settings.Default.setWindow_buildTracksLocation = Location;
            Properties.Settings.Default.Save();

            mf.twoSecondCounter = 100;

            mf.PanelUpdateRightAndBottom();
        }

        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            //reload what was
            isClosing = true;
            mf.curve.desList?.Clear();

            if (mf.isBtnAutoSteerOn)
            {
                mf.btnAutoSteer.PerformClick();
                mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, "Return From Editing");
            }
            if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

            mf.trk.gArr.Clear();

            foreach (var item in gTemp)
            {
                mf.trk.gArr.Add(new CTrk(item));
            }

            mf.trk.idx = originalLine;

            mf.curve.isCurveValid = false;
            mf.ABLine.isABValid = false;

            mf.twoSecondCounter = 100;

            Close();
        }

        private void btnListUse_Click(object sender, EventArgs e)
        {
            isClosing = true;
            //reset to generate new reference
            mf.curve.isCurveValid = false;
            mf.ABLine.isABValid = false;
            mf.curve.desList?.Clear();

            if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

            mf.FileSaveTracks();

            if (selectedItem > -1 && mf.trk.gArr.Count > 0 && mf.trk.gArr[selectedItem].isVisible)
            {
                mf.trk.idx = selectedItem;
                mf.yt.ResetYouTurn();

                Close();
            }

            else if (mf.trk.gArr.Count > 0)
            {
                bool isOneVis = false;
                int trac = -1;

                foreach (var item in mf.trk.gArr)
                {
                    trac++;
                    if (item.isVisible)
                    {
                        isOneVis = true;
                        break;
                    }
                }

                //just choose a visible something
                if (isOneVis)
                {
                    mf.trk.idx = trac;
                    mf.yt.ResetYouTurn();
                    Close();
                }
                else //nothing visible
                {
                    idx = -1;
                    mf.DisableYouTurnButtons();
                    if (mf.isBtnAutoSteerOn)
                    {
                        mf.btnAutoSteer.PerformClick();
                        mf.TimedMessageBox(2000, gStr.gsGuidanceStopped, gStr.gsNoGuidanceLines);
                        Log.EventWriter("Autosteer Stop, No Tracks Available");

                    }
                    Close();
                }
            }
            else
            {
                idx = -1;
                mf.DisableYouTurnButtons();
                if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();

                //mf.curve.numCurveLineSelected = 0;
                Close();
            }
        }

        #region Main Controls
        private void UpdateTable()
        {
            int scrollPixels = flp.VerticalScroll.Value;

            System.Drawing.Font backupfont = new System.Drawing.Font(base.Font.FontFamily, 18F, FontStyle.Regular);
            flp.Controls.Clear();


            for (int i = 0; i < mf.trk.gArr.Count; i++)
            {
                //outer inner
                Button a = new Button
                {
                    Margin = new Padding(20, 10, 2, 10),
                    Size = new Size(40, 25),
                    Name = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                };
                a.Click += A_Click;

                if (mf.trk.gArr[i].isVisible)
                    a.BackColor = System.Drawing.Color.Green;
                else
                    a.BackColor = System.Drawing.Color.Red;

                Button b = new Button
                {
                    Margin = new Padding(1, 10, 3, 10),
                    Size = new Size(35, 25),
                    Name = i.ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    FlatStyle = FlatStyle.Flat,
                };

                if (mf.trk.gArr[i].mode == TrackMode.AB)
                    b.Image = Properties.Resources.TrackLine;
                else if (mf.trk.gArr[i].mode == TrackMode.waterPivot)
                    b.Image = Properties.Resources.TrackPivot;
                else
                    b.Image = Properties.Resources.TrackCurve;

                b.FlatAppearance.BorderSize = 0;

                TextBox t = new TextBox
                {
                    Margin = new Padding(3),
                    Size = new Size(330, 35),
                    Text = mf.trk.gArr[i].name,
                    Name = i.ToString(),
                    Font = backupfont,
                    ReadOnly = true
                };
                t.Click += LineSelected_Click;
                t.Cursor = System.Windows.Forms.Cursors.Default;

                if (mf.trk.gArr[i].isVisible)
                    t.ForeColor = System.Drawing.Color.Black;
                else
                    t.ForeColor = System.Drawing.Color.Gray;

                if (i == selectedItem)
                {
                    t.BackColor = Color.LightBlue;
                }
                else
                {
                    t.BackColor = Color.AliceBlue;
                }

                flp.Controls.Add(b);
                flp.Controls.Add(t);
                flp.Controls.Add(a);
            }

            flp.VerticalScroll.Value = 1;
            flp.VerticalScroll.Value = scrollPixels;
            flp.PerformLayout();
        }

        private void A_Click(object sender, EventArgs e)
        {
            if (sender is Button b)
            {
                int line = Convert.ToInt32(b.Name);
                mf.trk.gArr[line].isVisible = !mf.trk.gArr[line].isVisible;

                for (int i = 0; i < mf.trk.gArr.Count; i++)
                {
                    flp.Controls[(i) * 3 + 1].BackColor = Color.AliceBlue;
                }
                selectedItem = -1;

                b.BackColor = mf.trk.gArr[line].isVisible ? System.Drawing.Color.Green : System.Drawing.Color.Red;

                flp.Controls[(line) * 3 + 1].ForeColor = mf.trk.gArr[line].isVisible ? System.Drawing.Color.Black : System.Drawing.Color.Gray;
            }
        }

        private void LineSelected_Click(object sender, EventArgs e)
        {
            if (sender is TextBox t)
            {
                int line = Convert.ToInt32(t.Name);
                int numLines = mf.trk.gArr.Count;

                //un highlight selected item
                for (int i = 0; i < numLines; i++)
                {
                    flp.Controls[(i) * 3 + 1].BackColor = Color.AliceBlue;
                }

                if (mf.trk.gArr[line].isVisible)
                {
                    //just highlight it
                    if (selectedItem == -1)
                    {
                        selectedItem = line;
                        selectedItem = Convert.ToInt32(t.Name);
                        flp.Controls[(line) * 3 + 1].BackColor = Color.LightBlue;
                    }

                    //a different line was selcted and one already was
                    else if (selectedItem != line)
                    {
                        selectedItem = line;
                        flp.Controls[(line) * 3 + 1].BackColor = Color.LightBlue;
                    }
                }
                else
                {
                    selectedItem = -1;
                }

                //UpdateTable();
            }
        }

        private void btnMoveUp_Click(object sender, EventArgs e)
        {
            if (selectedItem == -1 || selectedItem == 0)
                return;

            mf.trk.gArr.Reverse(selectedItem - 1, 2);
            selectedItem--;
            idx = selectedItem;

            int scrollPixels = flp.VerticalScroll.Value;

            scrollPixels -= 45;
            if (scrollPixels < 0) scrollPixels = 0;

            flp.VerticalScroll.Value = 1;
            flp.VerticalScroll.Value = scrollPixels;
            flp.PerformLayout();

            UpdateTable();
        }

        private void btnMoveDn_Click(object sender, EventArgs e)
        {
            if (selectedItem == -1 || selectedItem == (mf.trk.gArr.Count - 1))
                return;

            mf.trk.gArr.Reverse(selectedItem, 2);
            selectedItem++;

            idx = selectedItem;

            int scrollPixels = flp.VerticalScroll.Value;

            scrollPixels += 45;
            if (scrollPixels > flp.VerticalScroll.Maximum) scrollPixels = flp.VerticalScroll.Maximum;

            flp.VerticalScroll.Value = 1;
            flp.VerticalScroll.Value = scrollPixels;
            flp.PerformLayout();

            UpdateTable();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            if (selectedItem > -1)
            {
                idx = selectedItem;

                if (mf.trk.gArr[idx].mode == TrackMode.AB)
                {
                    vec2 bob = mf.trk.gArr[idx].ptA;
                    mf.trk.gArr[idx].ptA = mf.trk.gArr[idx].ptB;
                    mf.trk.gArr[idx].ptB = new vec2(bob);

                    mf.trk.gArr[idx].heading += Math.PI;
                    if (mf.trk.gArr[idx].heading < 0) mf.trk.gArr[idx].heading += glm.twoPI;
                    if (mf.trk.gArr[idx].heading > glm.twoPI) mf.trk.gArr[idx].heading -= glm.twoPI;
                }
                else
                {
                    int cnt = mf.trk.gArr[idx].curvePts.Count;
                    if (cnt > 0)
                    {
                        mf.trk.gArr[idx].curvePts.Reverse();

                        vec3[] arr = new vec3[cnt];
                        cnt--;
                        mf.trk.gArr[idx].curvePts.CopyTo(arr);
                        mf.trk.gArr[idx].curvePts.Clear();

                        mf.trk.gArr[idx].heading += Math.PI;
                        if (mf.trk.gArr[idx].heading < 0) mf.trk.gArr[idx].heading += glm.twoPI;
                        if (mf.trk.gArr[idx].heading > glm.twoPI) mf.trk.gArr[idx].heading -= glm.twoPI;

                        for (int i = 1; i < cnt; i++)
                        {
                            vec3 pt3 = arr[i];
                            pt3.heading += Math.PI;
                            if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                            if (pt3.heading < 0) pt3.heading += glm.twoPI;
                            mf.trk.gArr[idx].curvePts.Add(new vec3(pt3));
                        }

                        vec2 temp = new vec2(mf.trk.gArr[idx].ptA);

                        (mf.trk.gArr[idx].ptA) = new vec2(mf.trk.gArr[idx].ptB);
                        (mf.trk.gArr[idx].ptB) = new vec2(temp);
                    }
                }

                UpdateTable();
                flp.Focus();

                mf.TimedMessageBox(1500, "A B Swapped", "Curve is Reversed");
            }
        }

        private void btnHideShow_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < mf.trk.gArr.Count; i++)
            {
                mf.trk.gArr[i].isVisible = isOn;
            }

            isOn = !isOn;

            UpdateTable();
        }

        private void btnNewTrack_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelMain.Visible = false;
            panelCurve.Visible = false;
            panelName.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;
            panelKML.Visible = false;

            mf.curve.desList?.Clear();
            panelChoose.Visible = true;
        }

        private void btnListDelete_Click(object sender, EventArgs e)
        {
            if (selectedItem > -1)
            {
                mf.trk.gArr.RemoveAt(selectedItem);
                selectedItem = -1;

                mf.trk.idx = mf.trk.gArr.Count - 1;

                UpdateTable();
                flp.Focus();
            }
        }

        private void btnDuplicate_Click(object sender, EventArgs e)
        {
            if (selectedItem > -1)
            {
                int idx = selectedItem;

                panelMain.Visible = false;
                panelName.Visible = true;
                this.Size = new System.Drawing.Size(270, 360);

                mf.trk.gArr.Add(new CTrk(mf.trk.gArr[idx]));

                idx = mf.trk.gArr.Count - 1;

                selectedItem = -1;

                textBox1.Text = mf.trk.gArr[idx].name + " Copy";
            }
        }

        private void btnEditName_Click(object sender, EventArgs e)
        {
            if (selectedItem > -1)
            {
                idx = selectedItem;

                textBox2.Text = mf.trk.gArr[idx].name;

                panelMain.Visible = false;
                panelEditName.Visible = true;

                this.Size = new System.Drawing.Size(270, 360);
            }
        }

        #endregion

        #region Pick
        private void btnzABCurve_Click(object sender, EventArgs e)
        {
            mode = TrackMode.Curve;
            panelChoose.Visible = false;
            panelCurve.Visible = true;

            btnACurve.Enabled = true;
            btnACurve.Image = Properties.Resources.LetterABlue;
            btnBCurve.Enabled = false;
            btnPausePlay.Enabled = false;
            btnPausePlay.Image = Properties.Resources.boundaryPause;
            mf.curve.desList?.Clear();

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnzAPlus_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelAPlus.Visible = true;

            btnAPlus.Enabled = true;
            mf.curve.desList?.Clear();
            nudHeading.Enabled = false;

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnzABLine_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelABLine.Visible = true;

            btnALine.Enabled = true;
            btnBLine.Enabled = false;
            btnEnter_AB.Enabled = false;
            mf.curve.desList?.Clear();

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private void btnzLatLonPlusHeading_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelLatLonPlus.Visible = true;
            this.Size = new System.Drawing.Size(370, 460);

            nudLatitudePlus.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudePlus.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
            mf.Activate();
        }

        private void btnzLatLon_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelLatLonLatLon.Visible = true;
            this.Size = new System.Drawing.Size(370, 460);
            mf.Activate();
        }
        private void btnLatLonPivot_Click(object sender, EventArgs e)
        {

            panelChoose.Visible = false;
            panelPivot.Visible = true;
            this.Size = new System.Drawing.Size(370, 360);

            nudLatitudePivot.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudePivot.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
            mf.Activate();
        }

        private void btnLatLonPivot2_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelCurve.Visible = true;

            mf.curve.isMakingCurve = true;
            mf.curve.isRecordingCurve = false;

            btnRefSideCurve.Visible = false;
            btnPausePlay.Enabled = false;
            btnPausePlay.Image = Properties.Resources.PointDelete;
            mode = TrackMode.waterPivot;
            btnACurve.Image = Properties.Resources.PointAdd;
            btnACurve.Enabled = true;
            btnBCurve.Enabled = false;


            mf.curve.desList?.Clear();

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        #endregion

        #region Curve
        private void btnRefSideCurve_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideCurve.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnACurve_Click(object sender, System.EventArgs e)
        {
            if (mf.curve.isMakingCurve)
            {
                mf.curve.desList.Add(new vec3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, mf.pivotAxlePos.heading));
                btnBCurve.Enabled = mf.curve.desList.Count > 2;
                if (mode == TrackMode.waterPivot)
                {
                    btnPausePlay.Enabled = mf.curve.desList.Count > 0;
                    btnACurve.Enabled = mf.curve.desList.Count < 3;
                }
            }
            else
            {
                lblCurveExists.Text = gStr.gsDriving;
                ptAa.easting = mf.pivotAxlePos.easting;
                ptAa.northing = mf.pivotAxlePos.northing;

                btnBCurve.Enabled = true;
                btnACurve.Enabled = false;
                btnACurve.Image = Properties.Resources.PointAdd;
                btnPausePlay.Enabled = true;

                mf.curve.isMakingCurve = true;
                mf.curve.isRecordingCurve = true;
            }
            mf.Activate();
        }

        private void btnBCurve_Click(object sender, System.EventArgs e)
        {
            aveLineHeading = 0;
            mf.curve.isMakingCurve = false;
            mf.curve.isRecordingCurve = false;
            panelCurve.Visible = false;
            panelName.Visible = true;

            ptBb.easting = mf.pivotAxlePos.easting;
            ptBb.northing = mf.pivotAxlePos.northing;

            int cnt = mf.curve.desList.Count;
            if (mode == TrackMode.waterPivot && cnt > 2)
            {
                mf.trk.gArr.Add(new CTrk());
                //array number is 1 less since it starts at zero
                idx = mf.trk.gArr.Count - 1;

                mf.trk.gArr[idx].ptA = FindCircleCenter(mf.curve.desList[0], mf.curve.desList[1], mf.curve.desList[2]);
                mf.trk.gArr[idx].mode = TrackMode.waterPivot;
                mf.ABLine.desName = "Piv";
                textBox1.Text = mf.ABLine.desName;

                panelPivot.Visible = false;
                panelName.Visible = true;
            }
            else if (cnt > 2)
            {
                //make sure point distance isn't too big 
                mf.curve.MakePointMinimumSpacing(ref mf.curve.desList, 1.6);
                mf.curve.CalculateHeadings(ref mf.curve.desList);

                mf.trk.gArr.Add(new CTrk());
                //array number is 1 less since it starts at zero
                idx = mf.trk.gArr.Count - 1;

                mf.trk.gArr[idx].ptA = new vec2(ptAa);
                mf.trk.gArr[idx].ptB = new vec2(ptBb);

                mf.trk.gArr[idx].mode = TrackMode.Curve;

                //calculate average heading of line
                double x = 0, y = 0;
                foreach (vec3 pt in mf.curve.desList)
                {
                    x += Math.Cos(pt.heading);
                    y += Math.Sin(pt.heading);
                }
                x /= mf.curve.desList.Count;
                y /= mf.curve.desList.Count;
                aveLineHeading = Math.Atan2(y, x);
                if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                mf.trk.gArr[idx].heading = aveLineHeading;

                //build the tail extensions
                mf.curve.AddFirstLastPoints(ref mf.curve.desList);
                SmoothAB(4);
                mf.curve.CalculateHeadings(ref mf.curve.desList);

                //write out the Curve Points
                foreach (vec3 item in mf.curve.desList)
                {
                    mf.trk.gArr[idx].curvePts.Add(item);
                }

                mf.curve.desName = "Cu " +
                    (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                textBox1.Text = mf.curve.desName;

                panelCurve.Visible = false;
                panelName.Visible = true;

                double dist;

                if (isRefRightSide)
                {
                    dist = (mf.tool.width - mf.tool.overlap) * 0.5 + mf.tool.offset;
                    mf.trk.idx = idx;
                    mf.trk.NudgeRefCurve(dist);
                }
                else
                {
                    dist = (mf.tool.width - mf.tool.overlap) * -0.5 + mf.tool.offset;
                    mf.trk.idx = idx;
                    mf.trk.NudgeRefCurve(dist);
                }
            }
            else
            {
                mf.curve.desList?.Clear();

                panelMain.Visible = true;
                panelCurve.Visible = false;
                panelName.Visible = false;
                panelChoose.Visible = false;

                this.Size = new System.Drawing.Size(650, 480);
            }
            mf.Activate();
        }

        private void btnPausePlayCurve_Click(object sender, EventArgs e)
        {
            if (mode == TrackMode.waterPivot)
            {
                if (mf.curve.desList.Count > 0) mf.curve.desList.RemoveAt(mf.curve.desList.Count - 1);
                btnPausePlay.Enabled = mf.curve.desList.Count > 0;
                btnACurve.Enabled = mf.curve.desList.Count < 3;
            }
            else if (mf.curve.isRecordingCurve)
            {
                mf.curve.isRecordingCurve = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                //btnPausePlay.Text = gStr.gsRecord;
                btnACurve.Enabled = true;
            }
            else
            {
                mf.curve.isRecordingCurve = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                //btnPausePlay.Text = gStr.gsPause;
                btnACurve.Enabled = false;
            }
            btnBCurve.Enabled = mf.curve.desList.Count > 2;
            mf.Activate();
        }

        #endregion

        #region AB Line
        private void btnRefSideAB_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideAB.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnALine_Click(object sender, EventArgs e)
        {
            mf.ABLine.isMakingABLine = true;
            btnALine.Enabled = false;
            btnEnter_AB.Enabled = false;

            mf.ABLine.desPtA = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

            mf.ABLine.desLineEndA.easting = mf.ABLine.desPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.ABLine.desLineEndA.northing = mf.ABLine.desPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);


            mf.ABLine.desLineEndB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.ABLine.desLineEndB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            btnBLine.Enabled = true;
            btnALine.Enabled = false;

            timer1.Enabled = true;
            mf.Activate();
        }

        private void btnBLine_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            btnEnter_AB.Enabled = true;

            mf.ABLine.desPtB = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

            btnBLine.BackColor = System.Drawing.Color.Teal;

            mf.ABLine.desHeading = Math.Atan2(mf.ABLine.desPtB.easting - mf.ABLine.desPtA.easting,
               mf.ABLine.desPtB.northing - mf.ABLine.desPtA.northing);
            if (mf.ABLine.desHeading < 0) mf.ABLine.desHeading += glm.twoPI;

            mf.ABLine.desLineEndA.easting = mf.ABLine.desPtA.easting - (Math.Sin(mf.ABLine.desHeading) * 1000);
            mf.ABLine.desLineEndA.northing = mf.ABLine.desPtA.northing - (Math.Cos(mf.ABLine.desHeading) * 1000);

            mf.ABLine.desLineEndB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.ABLine.desHeading) * 1000);
            mf.ABLine.desLineEndB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.ABLine.desHeading) * 1000);
            mf.Activate();
        }

        private void btnEnter_AB_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            mf.ABLine.isMakingABLine = false;
            mf.trk.gArr.Add(new CTrk());

            idx = mf.trk.gArr.Count - 1;

            mf.trk.gArr[idx].ptA = new vec2(mf.ABLine.desPtA);
            mf.trk.gArr[idx].ptB = new vec2(mf.ABLine.desPtB);

            mf.trk.gArr[idx].mode = TrackMode.AB;

            mf.trk.gArr[idx].heading = mf.ABLine.desHeading;

            mf.ABLine.desName = "AB " +
                (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";
            textBox1.Text = mf.ABLine.desName;

            double dist;
            if (isRefRightSide)
            {
                dist = (mf.tool.width - mf.tool.overlap) * 0.5 + mf.tool.offset;
                mf.trk.idx = idx;
                mf.trk.NudgeRefABLine(dist);

            }
            else
            {
                dist = (mf.tool.width - mf.tool.overlap) * -0.5 + mf.tool.offset;
                mf.trk.idx = idx;
                mf.trk.NudgeRefABLine(dist);
            }


            panelABLine.Visible = false;
            panelName.Visible = true;

            mf.Activate();
        }

        #endregion

        #region A Plus

        private void btnRefSideAPlus_Click(object sender, EventArgs e)
        {
            isRefRightSide = !isRefRightSide;
            btnRefSideAPlus.Image = isRefRightSide ?
            Properties.Resources.BoundaryRight : Properties.Resources.BoundaryLeft;
            mf.Activate();
        }

        private void btnAPlus_Click(object sender, EventArgs e)
        {
            mf.ABLine.isMakingABLine = true;

            mf.ABLine.desPtA = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

            mf.ABLine.desPtB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1);
            mf.ABLine.desPtB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1);

            mf.ABLine.desLineEndA.easting = mf.ABLine.desPtA.easting - (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.ABLine.desLineEndA.northing = mf.ABLine.desPtA.northing - (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.ABLine.desLineEndB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.pivotAxlePos.heading) * 1000);
            mf.ABLine.desLineEndB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.pivotAxlePos.heading) * 1000);

            mf.ABLine.desHeading = mf.pivotAxlePos.heading;

            btnEnter_AB.Enabled = true;
            nudHeading.Enabled = true;

            nudHeading.Value = (decimal)(glm.toDegrees(mf.ABLine.desHeading));

            timer1.Enabled = true;
            mf.Activate();
        }

        private void nudHeading_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                //original A pt. 
                mf.ABLine.desHeading = glm.toRadians((double)nudHeading.Value);

                //start end of line
                mf.ABLine.desPtB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.ABLine.desHeading) * 200);
                mf.ABLine.desPtB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.ABLine.desHeading) * 200);

                mf.ABLine.desLineEndA.easting = mf.ABLine.desPtA.easting - (Math.Sin(mf.ABLine.desHeading) * 1000);
                mf.ABLine.desLineEndA.northing = mf.ABLine.desPtA.northing - (Math.Cos(mf.ABLine.desHeading) * 1000);

                mf.ABLine.desLineEndB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.ABLine.desHeading) * 1000);
                mf.ABLine.desLineEndB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.ABLine.desHeading) * 1000);
            }
            mf.Activate();
        }

        private void btnEnter_APlus_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;

            mf.ABLine.isMakingABLine = false;
            mf.trk.gArr.Add(new CTrk());

            idx = mf.trk.gArr.Count - 1;

            mf.trk.gArr[idx].ptA = new vec2(mf.ABLine.desPtA);
            mf.trk.gArr[idx].ptB = new vec2(mf.ABLine.desPtB);

            mf.trk.gArr[idx].mode = TrackMode.AB;

            mf.trk.gArr[idx].heading = mf.ABLine.desHeading;

            mf.ABLine.desName = "A+" +
                (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";
            textBox1.Text = mf.ABLine.desName;

            double dist;
            if (isRefRightSide)
            {
                dist = (mf.tool.width - mf.tool.overlap) * 0.5 + mf.tool.offset;
                mf.trk.idx = idx;
                mf.trk.NudgeRefABLine(dist);

            }
            else
            {
                dist = (mf.tool.width - mf.tool.overlap) * -0.5 + mf.tool.offset;
                mf.trk.idx = idx;
                mf.trk.NudgeRefABLine(dist);
            }

            panelAPlus.Visible = false;
            panelName.Visible = true;
            mf.Activate();
        }

        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            mf.ABLine.desPtB = new vec2(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing);

            mf.ABLine.desHeading = Math.Atan2(mf.ABLine.desPtB.easting - mf.ABLine.desPtA.easting,
               mf.ABLine.desPtB.northing - mf.ABLine.desPtA.northing);
            if (mf.ABLine.desHeading < 0) mf.ABLine.desHeading += glm.twoPI;

            mf.ABLine.desLineEndA.easting = mf.ABLine.desPtA.easting - (Math.Sin(mf.ABLine.desHeading) * 1000);
            mf.ABLine.desLineEndA.northing = mf.ABLine.desPtA.northing - (Math.Cos(mf.ABLine.desHeading) * 1000);

            mf.ABLine.desLineEndB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.ABLine.desHeading) * 1000);
            mf.ABLine.desLineEndB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.ABLine.desHeading) * 1000);
        }

        #region KML Curve and line

        private void btnLoadABFromKML_Click(object sender, EventArgs e)
        {
            panelChoose.Visible = false;
            panelKML.Visible = true;

            mf.curve.desList?.Clear();

            this.Size = new System.Drawing.Size(270, 360);

            string fileAndDirectory;
            {
                //create the dialog instance
                OpenFileDialog ofd = new OpenFileDialog
                {
                    //set the filter to text KML only
                    Filter = "KML files (*.KML)|*.KML",

                    //the initial directory, fields, for the open dialog
                    InitialDirectory = Path.Combine(RegistrySettings.fieldsDirectory, mf.currentFieldDirectory)
                };

                //was a file selected
                if (ofd.ShowDialog(this) == DialogResult.Cancel) return;
                else fileAndDirectory = ofd.FileName;
            }

            XmlDocument doc = new XmlDocument
            {
                PreserveWhitespace = false
            };

            try
            {
                doc.Load(fileAndDirectory);
                string trackName = Path.GetFileName(fileAndDirectory);
                trackName = trackName.Substring(0, trackName.Length - 4);

                XmlElement root = doc.DocumentElement;
                XmlNodeList trackList = root.GetElementsByTagName("coordinates");
                XmlNodeList namelist = root.GetElementsByTagName("name");

                if (namelist.Count > 1)
                {
                    trackName = namelist[1].InnerText;
                }

                //each element in the list is a track
                for (int i = 0; i < trackList.Count; i++)
                {
                    string line = trackList[i].InnerText;
                    line.Trim();
                    //line = coordinates;
                    char[] delimiterChars = { ' ', '\t', '\r', '\n' };
                    string[] numberSets = line.Split(delimiterChars);

                    //at least 3 points
                    if (numberSets.Length > 1)
                    {
                        foreach (string item in numberSets)
                        {
                            string[] fix = item.Split(',');
                            if (fix.Length != 3) continue;
                            double.TryParse(fix[0], NumberStyles.Float, CultureInfo.InvariantCulture, out double lonK);
                            double.TryParse(fix[1], NumberStyles.Float, CultureInfo.InvariantCulture, out double latK);

                            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84(latK, lonK));
                            mf.curve.desList.Add(new vec3(geoCoord));
                        }
                    }

                    //2 points
                    if (mf.curve.desList.Count == 2)
                    {
                        mf.ABLine.desPtA.easting = mf.curve.desList[0].easting;
                        mf.ABLine.desPtA.northing = mf.curve.desList[0].northing;

                        mf.ABLine.desPtB.easting = mf.curve.desList[1].easting;
                        mf.ABLine.desPtB.northing = mf.curve.desList[1].northing;

                        // heading based on AB points
                        mf.ABLine.desHeading = Math.Atan2(mf.ABLine.desPtB.easting - mf.ABLine.desPtA.easting,
                            mf.ABLine.desPtB.northing - mf.ABLine.desPtA.northing);
                        if (mf.ABLine.desHeading < 0) mf.ABLine.desHeading += glm.twoPI;

                        if (namelist.Count > i)
                        {
                            trackName = namelist[i + 1].InnerText;
                            mf.ABLine.desName = trackName;
                        }
                        else mf.ABLine.desName = "AB " +
                            (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                        mf.trk.gArr.Add(new CTrk());

                        idx = mf.trk.gArr.Count - 1;

                        mf.trk.gArr[idx].heading = mf.ABLine.desHeading;
                        mf.trk.gArr[idx].mode = TrackMode.AB;

                        mf.trk.gArr[idx].ptA = new vec2(mf.ABLine.desPtA);
                        mf.trk.gArr[idx].ptB = new vec2(mf.ABLine.desPtB);

                        //create a name
                        mf.trk.gArr[idx].name = mf.ABLine.desName;

                        mf.curve.desList?.Clear();
                    }
                    else if (mf.curve.desList.Count > 2)
                    {
                        //make sure point distance isn't too big 
                        mf.curve.MakePointMinimumSpacing(ref mf.curve.desList, 1.6);
                        mf.curve.CalculateHeadings(ref mf.curve.desList);

                        mf.trk.gArr.Add(new CTrk());

                        //array number is 1 less since it starts at zero
                        idx = mf.trk.gArr.Count - 1;

                        mf.trk.gArr[idx].ptA =
                            new vec2(mf.curve.desList[0].easting, mf.curve.desList[0].northing);
                        mf.trk.gArr[idx].ptB =
                            new vec2(mf.curve.desList[mf.curve.desList.Count - 1].easting,
                            mf.curve.desList[mf.curve.desList.Count - 1].northing);

                        mf.trk.gArr[idx].mode = TrackMode.Curve;

                        //calculate average heading of line
                        double x = 0, y = 0;
                        foreach (vec3 pt in mf.curve.desList)
                        {
                            x += Math.Cos(pt.heading);
                            y += Math.Sin(pt.heading);
                        }
                        x /= mf.curve.desList.Count;
                        y /= mf.curve.desList.Count;
                        aveLineHeading = Math.Atan2(y, x);
                        if (aveLineHeading < 0) aveLineHeading += glm.twoPI;

                        mf.trk.gArr[idx].heading = aveLineHeading;

                        //build the tail extensions
                        mf.curve.AddFirstLastPoints(ref mf.curve.desList);
                        //SmoothAB(4);
                        mf.curve.CalculateHeadings(ref mf.curve.desList);

                        //write out the Curve Points
                        foreach (vec3 item in mf.curve.desList)
                        {
                            mf.trk.gArr[idx].curvePts.Add(new vec3(item));
                        }
                        if (namelist.Count > i)
                        {
                            trackName = namelist[i + 1].InnerText;
                            mf.curve.desName = trackName;
                        }
                        else mf.curve.desName = "Cu " +
                                 (Math.Round(glm.toDegrees(aveLineHeading), 1)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";

                        mf.trk.gArr[idx].name = mf.curve.desName;

                        mf.curve.desList?.Clear();
                    }
                    else
                    {
                        mf.TimedMessageBox(2000, gStr.gsErrorreadingKML, gStr.gsMissingABLinesFile);
                    }
                }
            }
            catch (Exception ed)
            {
                Log.EventWriter("Tracks from KML " + ed.ToString());
                return;
            }

            panelKML.Visible = false;
            panelName.Visible = false;
            panelMain.Visible = true;

            this.Size = new System.Drawing.Size(650, 480);

            mf.curve.desList?.Clear();

            UpdateTable();
            flp.Focus();
        }

        #endregion

        #region LatLon LatLon

        private void nudLatitudeA_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudLongitudeA_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudLatitudeB_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudLongitudeB_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void btnEnter_LatLonLatLon_Click(object sender, EventArgs e)
        {
            CalcHeadingAB();

            mf.ABLine.isMakingABLine = false;
            mf.trk.gArr.Add(new CTrk());

            idx = mf.trk.gArr.Count - 1;

            mf.trk.gArr[idx].ptA = new vec2(mf.ABLine.desPtA);
            mf.trk.gArr[idx].ptB = new vec2(mf.ABLine.desPtB);

            mf.trk.gArr[idx].mode = TrackMode.AB;

            mf.trk.gArr[idx].heading = mf.ABLine.desHeading;

            mf.ABLine.desName = "AB " +
                (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";
            textBox1.Text = mf.ABLine.desName;

            panelLatLonLatLon.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
        }

        public void CalcHeadingAB()
        {
            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84((double)nudLatitudeA.Value, (double)nudLongitudeA.Value));

            mf.ABLine.desPtA = new vec2(geoCoord);

            geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84((double)nudLatitudeB.Value, (double)nudLongitudeB.Value));
            mf.ABLine.desPtB = new vec2(geoCoord);

            // heading based on AB points
            mf.ABLine.desHeading = Math.Atan2(mf.ABLine.desPtB.easting - mf.ABLine.desPtA.easting,
                mf.ABLine.desPtB.northing - mf.ABLine.desPtA.northing);
            if (mf.ABLine.desHeading < 0) mf.ABLine.desHeading += glm.twoPI;
        }

        private void btnFillLatLonLatLonA_Click(object sender, EventArgs e)
        {
            nudLatitudeA.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudeA.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
        }

        private void btnFillLatLonLatLonB_Click(object sender, EventArgs e)
        {
            nudLatitudeB.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudeB.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
        }

        #endregion

        #region LatLon +

        private void nudLatitudePlus_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudLongitudePlus_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudHeadingLatLonPlus_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void btnEnter_LatLonPlus_Click(object sender, EventArgs e)
        {
            CalcHeadingAPlus();

            mf.ABLine.isMakingABLine = false;
            mf.trk.gArr.Add(new CTrk());

            idx = mf.trk.gArr.Count - 1;

            //start end of line
            mf.ABLine.desPtB.easting = mf.ABLine.desPtA.easting + (Math.Sin(mf.ABLine.desHeading) * 200);
            mf.ABLine.desPtB.northing = mf.ABLine.desPtA.northing + (Math.Cos(mf.ABLine.desHeading) * 200);

            mf.trk.gArr[idx].ptA = new vec2(mf.ABLine.desPtA);
            mf.trk.gArr[idx].ptB = new vec2(mf.ABLine.desPtB);

            mf.trk.gArr[idx].mode = TrackMode.AB;

            mf.trk.gArr[idx].heading = mf.ABLine.desHeading;

            mf.ABLine.desName = "A+ " +
                (Math.Round(glm.toDegrees(mf.ABLine.desHeading), 5)).ToString(CultureInfo.InvariantCulture) + "\u00B0 ";
            textBox1.Text = mf.ABLine.desName;

            panelLatLonPlus.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
        }

        private void btnFillLatLonPlus_Click(object sender, EventArgs e)
        {
            nudLatitudePlus.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudePlus.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
        }

        public void CalcHeadingAPlus()
        {
            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84((double)nudLatitudePlus.Value, (double)nudLongitudePlus.Value));

            mf.ABLine.desHeading = glm.toRadians((double)nudHeadingLatLonPlus.Value);
            mf.ABLine.desPtA = new vec2(geoCoord);
        }

        #endregion

        #region Lat Lon Pivot

        private void nudLatitudePivot_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudLongitudePivot_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void btnEnter_Pivot_Click(object sender, EventArgs e)
        {
            GeoCoord geoCoord = mf.AppModel.LocalPlane.ConvertWgs84ToGeoCoord(new Wgs84((double)nudLatitudePivot.Value, (double)nudLongitudePivot.Value));

            mf.trk.gArr.Add(new CTrk());

            idx = mf.trk.gArr.Count - 1;

            mf.trk.gArr[idx].ptA = new vec2(geoCoord);
            mf.trk.gArr[idx].mode = TrackMode.waterPivot;

            mf.ABLine.desName = "Piv";
            textBox1.Text = mf.ABLine.desName;

            panelPivot.Visible = false;
            panelName.Visible = true;

            this.Size = new System.Drawing.Size(270, 360);
            mf.Activate();
        }

        private vec2 FindCircleCenter(vec3 p1, vec3 p2, vec3 p3)
        {
            var d2 = p2.northing * p2.northing + p2.easting * p2.easting;
            var bc = (p1.northing * p1.northing + p1.easting * p1.easting - d2) / 2;
            var cd = (d2 - p3.northing * p3.northing - p3.easting * p3.easting) / 2;
            var det = (p1.northing - p2.northing) * (p2.easting - p3.easting) - (p2.northing - p3.northing) * (p1.easting - p2.easting);
            if (Math.Abs(det) > 1e-10)
                return new vec2(
              ((p1.northing - p2.northing) * cd - (p2.northing - p3.northing) * bc) / det,
              (bc * (p2.easting - p3.easting) - cd * (p1.easting - p2.easting)) / det
            );
            else return new vec2();
        }

        private void btnFillLAtLonPivot_Click(object sender, EventArgs e)
        {
            nudLatitudePivot.Value = (decimal)mf.AppModel.CurrentLatLon.Latitude;
            nudLongitudePivot.Value = (decimal)mf.AppModel.CurrentLatLon.Longitude;
        }

        #endregion

        private void btnCancelCurve_Click(object sender, EventArgs e)
        {
            mf.curve.isMakingCurve = false;
            mf.curve.isRecordingCurve = false;
            mf.curve.desList?.Clear();
            mf.ABLine.isMakingABLine = false;

            panelMain.Visible = true;
            panelEditName.Visible = false;
            panelName.Visible = false;
            panelChoose.Visible = false;
            panelCurve.Visible = false;
            panelABLine.Visible = false;
            panelAPlus.Visible = false;
            panelLatLonLatLon.Visible = false;
            panelLatLonPlus.Visible = false;
            panelKML.Visible = false;
            panelPivot.Visible = false;

            this.Size = new System.Drawing.Size(650, 480);
            mf.Activate();
        }

        private void textBox_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
                ((TextBox)sender).ShowKeyboard(this);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0) textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            int idx = mf.trk.gArr.Count - 1;

            mf.trk.gArr[idx].name = textBox1.Text.Trim();

            panelMain.Visible = true;
            panelName.Visible = false;

            this.Size = new System.Drawing.Size(650, 480);

            mf.curve.desList?.Clear();
            UpdateTable();
            mf.Activate();
        }

        private void btnAddTime_Click(object sender, EventArgs e)
        {
            textBox1.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
            mf.curve.desName = textBox1.Text;
            mf.Activate();
        }

        private void btnAddTimeEdit_Click(object sender, EventArgs e)
        {
            textBox2.Text += DateTime.Now.ToString(" hh:mm:ss", CultureInfo.InvariantCulture);
            mf.Activate();
        }

        private void btnSaveEditName_Click(object sender, EventArgs e)
        {
            if (textBox2.Text.Trim() == "") textBox2.Text = "No Name " + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            panelEditName.Visible = false;
            panelMain.Visible = true;

            mf.curve.desList?.Clear();

            mf.trk.gArr[idx].name = textBox2.Text.Trim();

            this.Size = new System.Drawing.Size(650, 480);

            UpdateTable();
            flp.Focus();
            mf.Activate();
        }

        public void SmoothAB(int smPts)
        {
            //countExit the reference list of original curve
            int cnt = mf.curve.desList.Count;

            //the temp array
            vec3[] arr = new vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = mf.curve.desList[s].easting;
                arr[s].northing = mf.curve.desList[s].northing;
                arr[s].heading = mf.curve.desList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = mf.curve.desList[s].easting;
                arr[s].northing = mf.curve.desList[s].northing;
                arr[s].heading = mf.curve.desList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += mf.curve.desList[j + i].easting;
                    arr[i].northing += mf.curve.desList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = mf.curve.desList[i].heading;
            }

            //make a list to draw
            mf.curve.desList?.Clear();
            for (int i = 0; i < cnt; i++)
            {
                mf.curve.desList.Add(arr[i]);
            }
            mf.Activate();
        }
    }
}
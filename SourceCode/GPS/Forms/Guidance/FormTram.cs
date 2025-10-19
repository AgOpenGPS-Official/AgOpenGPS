using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTram : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf = null;

        private bool isSaving;
        private static bool isCurve;

        public FormTram(Form callingForm, bool Curve)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.gsSimpleTramLines;
            labelPasses.Text = gStr.gsPasses;
            labelMode.Text = gStr.gsMode;
            labelAlpha.Text = gStr.gsAlpha;
            labelSeed.Text = gStr.gsWorkWidth;
            labelSprayWidth.Text = gStr.gsTramWidth;
            labelTrack.Text = gStr.gsTrack;
            lblTramWidth.Text = (mf.tram.tramWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;
            lblSeedWidth.Text = (mf.tool.width * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            nudPasses.Controls[0].Enabled = false;

            isCurve = Curve;
        }

        private void FormTram_Load(object sender, EventArgs e)
        {
            tbarTramAlpha.Value = (int)(mf.tram.alpha * 100);
            lblAplha.Text = tbarTramAlpha.Value.ToString() + "%";

            if (Properties.Settings.Default.setTram_passes < 1)
            {
                Properties.Settings.Default.setTram_passes = 1;
                Properties.Settings.Default.Save();
            }
            nudPasses.Value = Properties.Settings.Default.setTram_passes;
            nudPasses.ValueChanged += nudPasses_ValueChanged;

            lblTrack.Text = (mf.vehicle.VehicleConfig.TrackWidth * mf.m2FtOrM).ToString("N2") + mf.unitsFtM;

            mf.tool.halfWidth = (mf.tool.width - mf.tool.overlap) / 2.0;

            //if off, turn it on because they obviously want a tram.
            mf.tram.generateMode = 0;

            if (mf.tram.tramList.Count > 0 && mf.tram.tramBndOuterArr.Count > 0)
                mf.tram.generateMode = 0;
            else if (mf.tram.tramBndOuterArr.Count == 0)
                mf.tram.generateMode = 1;
            else if (mf.tram.tramList.Count == 0)
                mf.tram.generateMode = 2;
            else mf.tram.generateMode = 0;

            if (mf.bnd.bndList.Count == 0) mf.tram.generateMode = 1;

            switch (mf.tram.generateMode)
            {
                case 0:
                    btnMode.BackgroundImage = Properties.Resources.TramAll;
                    break;

                case 1:
                    btnMode.BackgroundImage = Properties.Resources.TramLines;
                    break;

                case 2:
                    btnMode.BackgroundImage = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }

            if (mf.bnd.bndList.Count == 0) btnMode.Enabled = false;

            mf.CloseTopMosts();

            if (mf.tram.tramList.Count > 0 || mf.tram.tramBndOuterArr.Count > 0)
            {
                //don't rebuild as trams exist
            }
            else
            {
                MoveBuildTramLine(0);
            }

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormTram_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isSaving)
            {
            }
            else
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

            Properties.Settings.Default.setTram_alpha = mf.tram.alpha;
            Properties.Settings.Default.Save();
        }

        private void MoveBuildTramLine(double Dist)
        {
            mf.tram.displayMode = 1;

            if (isCurve)
            {
                //if (Dist != 0)
                //    mf.trk.NudgeRefCurve(Dist);
                mf.curve.BuildTram();
            }
            else
            {
                //if (Dist != 0)
                //    mf.trk.NudgeRefABLine(Dist);
                mf.ABLine.BuildTram();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            isSaving = true;
            Close();
        }

        private void nudPasses_ValueChanged(object sender, EventArgs e)
        {
            mf.tram.passes = (int)nudPasses.Value;
            Properties.Settings.Default.setTram_passes = mf.tram.passes;
            Properties.Settings.Default.Save();
            MoveBuildTramLine(0);
        }

        private void nudPasses_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }
        private void btnUpTrams_Click(object sender, EventArgs e)
        {
            nudPasses.UpButton();
        }

        private void btnDnTrams_Click(object sender, EventArgs e)
        {
            nudPasses.DownButton();
        }

        private void btnSwapAB_Click(object sender, EventArgs e)
        {
            // NEW Phase 6.5: Use _trackService instead of trk.gArr/trk.idx
            var currentTrack = mf._trackService.GetCurrentTrack();
            if (currentTrack == null) return;

            // Create CTrk manually from Track for legacy manipulation
            var ctrk = new CTrk
            {
                name = currentTrack.Name,
                mode = (TrackMode)currentTrack.Mode,
                isVisible = currentTrack.IsVisible,
                nudgeDistance = currentTrack.NudgeDistance,
                ptA = currentTrack.PtA,
                ptB = currentTrack.PtB,
                heading = currentTrack.Heading,
                curvePts = new List<vec3>(currentTrack.CurvePts),
                endPtA = new vec2(),
                endPtB = new vec2()
            };

            if (ctrk.mode == TrackMode.AB)
            {
                vec2 bob = ctrk.ptA;
                ctrk.ptA = ctrk.ptB;
                ctrk.ptB = new vec2(bob);

                ctrk.heading += Math.PI;
                if (ctrk.heading < 0) ctrk.heading += glm.twoPI;
                if (ctrk.heading > glm.twoPI) ctrk.heading -= glm.twoPI;

                double abHeading = ctrk.heading;
                ctrk.endPtA.easting = ctrk.ptA.easting - (Math.Sin(abHeading) * mf.ABLine.abLength);
                ctrk.endPtA.northing = ctrk.ptA.northing - (Math.Cos(abHeading) * mf.ABLine.abLength);

                ctrk.endPtB.easting = ctrk.ptB.easting + (Math.Sin(abHeading) * mf.ABLine.abLength);
                ctrk.endPtB.northing = ctrk.ptB.northing + (Math.Cos(abHeading) * mf.ABLine.abLength);

            }
            else
            {
                int cnt = ctrk.curvePts.Count;
                if (cnt > 0)
                {
                    ctrk.curvePts.Reverse();

                    vec3[] arr = new vec3[cnt];
                    cnt--;
                    ctrk.curvePts.CopyTo(arr);
                    ctrk.curvePts.Clear();

                    ctrk.heading += Math.PI;
                    if (ctrk.heading < 0) ctrk.heading += glm.twoPI;
                    if (ctrk.heading > glm.twoPI) ctrk.heading -= glm.twoPI;

                    for (int i = 1; i < cnt; i++)
                    {
                        vec3 pt3 = arr[i];
                        pt3.heading += Math.PI;
                        if (pt3.heading > glm.twoPI) pt3.heading -= glm.twoPI;
                        if (pt3.heading < 0) pt3.heading += glm.twoPI;
                        ctrk.curvePts.Add(pt3);
                    }

                    vec2 temp = new vec2(ctrk.ptA);

                    (ctrk.ptA) = new vec2(ctrk.ptB);
                    (ctrk.ptB) = new vec2(temp);
                }
            }

            // Convert back and replace in track list
            currentTrack.Name = ctrk.name;
            currentTrack.Mode = (AgOpenGPS.Core.Models.Guidance.TrackMode)ctrk.mode;
            currentTrack.IsVisible = ctrk.isVisible;
            currentTrack.NudgeDistance = ctrk.nudgeDistance;
            currentTrack.PtA = ctrk.ptA;
            currentTrack.PtB = ctrk.ptB;
            currentTrack.Heading = ctrk.heading;
            currentTrack.CurvePts = ctrk.curvePts;

            mf.FileSaveTracks();

            mf.tram.tramArr?.Clear();
            mf.tram.tramList?.Clear();
            mf.tram.tramBndOuterArr?.Clear();
            mf.tram.tramBndInnerArr?.Clear();

            MoveBuildTramLine(0);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

            Close();
        }

        private void btnMode_Click(object sender, EventArgs e)
        {
            mf.tram.generateMode++;
            if (mf.tram.generateMode > 2) mf.tram.generateMode = 0;

            switch (mf.tram.generateMode)
            {
                case 0:
                    btnMode.BackgroundImage = Properties.Resources.TramAll;
                    break;

                case 1:
                    btnMode.BackgroundImage = Properties.Resources.TramLines;
                    break;

                case 2:
                    btnMode.BackgroundImage = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }

            MoveBuildTramLine(0);
        }

        private void tbarTramAlpha_Scroll(object sender, EventArgs e)
        {
            mf.tram.alpha = (double)tbarTramAlpha.Value * 0.01;
            lblAplha.Text = tbarTramAlpha.Value.ToString() + "%";
        }
    }
}
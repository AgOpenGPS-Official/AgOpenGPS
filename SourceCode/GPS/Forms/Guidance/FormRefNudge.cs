using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Helpers;

namespace AgOpenGPS
{
    public partial class FormRefNudge : Form
    {
        private readonly FormGPS mf = null;
        // BIG BANG Step 2: CTrk → Track
        public List<AgOpenGPS.Core.Models.Guidance.Track> gTemp = new List<AgOpenGPS.Core.Models.Guidance.Track>();


        private double snapAdj = 0, distanceMoved = 0;
        public FormRefNudge(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = gStr.gsNudgeRefTrack;
        }

        private void FormEditTrack_Load(object sender, EventArgs e)
        {
            if (mf.isMetric)
            {
                nudSnapDistance.DecimalPlaces = 0;
                nudSnapDistance.Value = (int)((double)Properties.Settings.Default.setAS_snapDistanceRef);
            }
            else
            {
                nudSnapDistance.DecimalPlaces = 1;
                nudSnapDistance.Value = (decimal)Math.Round(((double)Properties.Settings.Default.setAS_snapDistanceRef * mf.cm2CmOrIn), 1);
            }

            snapAdj = Properties.Settings.Default.setAS_snapDistanceRef * 0.01;

            // NEW Phase 6.5: Use _trackService instead of trk.gArr
            // BIG BANG Step 2: gTemp is now List<Track>, so just clone the tracks directly
            foreach (var item in mf._trackService.GetAllTracks())
            {
                // Clone Track for temporary backup
                gTemp.Add(item.Clone());
            }

            lblOffset.Text = ((int)(distanceMoved * mf.m2InchOrCm)).ToString() + " " + mf.unitsInCm;

            //Location = Properties.Settings.Default.setWindow_formNudgeLocation;
            //Size = Properties.Settings.Default.setWindow_formNudgeSize;

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormEditTrack_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.panelRight.Visible = true;
        }

        private void nudSnapDistance_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            snapAdj = (double)nudSnapDistance.Value * mf.inchOrCm2m;
            Properties.Settings.Default.setAS_snapDistanceRef = snapAdj * 100;
            Properties.Settings.Default.Save();
            mf.Activate();
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(snapAdj);
            distanceMoved += snapAdj;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack(-snapAdj);
            distanceMoved += -snapAdj;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnHalfToolRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack((mf.tool.width - mf.tool.overlap) * 0.5);
            distanceMoved += (mf.tool.width - mf.tool.overlap) * 0.5;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void btnHalfToolLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeRefTrack((mf.tool.width - mf.tool.overlap) * -0.5);
            distanceMoved += (mf.tool.width - mf.tool.overlap) * -0.5;
            DistanceMovedLabel();
            mf.Activate();
        }

        private void DistanceMovedLabel()
        {
            lblOffset.Text = ((int)(distanceMoved * mf.m2InchOrCm)).ToString() + " " + mf.unitsInCm;
            mf.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            //if (mf.trk.gArr.Count > 0)
            {
                //save entire list
                mf.FileSaveTracks();
            }
            Close();
        }

        private void btnCancelMain_Click(object sender, EventArgs e)
        {
            // NEW Phase 6.5: Use _trackService instead of trk.gArr
            // BIG BANG Step 2: gTemp is now List<Track>, so just add them directly
            mf._trackService.ClearTracks();

            foreach (var item in gTemp)
            {
                mf._trackService.AddTrack(item);
            }

            mf.ABLine.isABValid = false;
            mf.curve.isCurveValid = false;

            //mf.FileSaveTracks();
            Close();
        }
    }
}

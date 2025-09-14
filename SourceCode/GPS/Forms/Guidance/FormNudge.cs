using System;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Controls;
using AgOpenGPS.Helpers;

namespace AgOpenGPS
{
    public partial class FormNudge : Form
    {
        private readonly FormGPS mf = null;

        private double snapAdj = 0;
        public FormNudge(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;

            InitializeComponent();

            UpdateMoveLabel();

            this.Text = "";
        }

        private void FormEditTrack_Load(object sender, EventArgs e)
        {
            if (mf.isMetric)
            {
                nudSnapDistance.DecimalPlaces = 0;
                nudSnapDistance.Value = (int)((double)Properties.Settings.Default.setAS_snapDistance);
            }
            else
            {
                nudSnapDistance.DecimalPlaces = 1;
                nudSnapDistance.Value = (decimal)Math.Round(((double)Properties.Settings.Default.setAS_snapDistance * mf.cm2CmOrIn), 1);
            }

            snapAdj = Properties.Settings.Default.setAS_snapDistance * 0.01;

            Location = Properties.Settings.Default.setWindow_formNudgeLocation;
            UpdateMoveLabel();

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }
        private void FormEditTrack_MouseEnter(object sender, EventArgs e)
        {
            UpdateMoveLabel();
        }
        private void FormEditTrack_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.setWindow_formNudgeLocation = Location;
            Properties.Settings.Default.Save();

            //save entire list
            mf.FileSaveTracks();
        }

        private void UpdateMoveLabel()
        {
            if (mf.trk.idx > -1)
            {
                if (mf.trk.gArr[mf.trk.idx].nudgeDistance == 0)
                    lblOffset.Text = ((int)(mf.trk.gArr[mf.trk.idx].nudgeDistance * mf.m2InchOrCm * -1)).ToString() + mf.unitsInCm;
                else if (mf.trk.gArr[mf.trk.idx].nudgeDistance < 0)
                    lblOffset.Text = "< " + ((int)(mf.trk.gArr[mf.trk.idx].nudgeDistance * mf.m2InchOrCm * -1)).ToString() + mf.unitsInCm;
                else
                    lblOffset.Text = ((int)(mf.trk.gArr[mf.trk.idx].nudgeDistance * mf.m2InchOrCm)).ToString() + " >" + mf.unitsInCm;
            }
        }

        private void btnZeroMove_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeDistanceReset();
            UpdateMoveLabel();
            mf.Activate();
        }

        private void nudSnapDistance_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            snapAdj = (double)nudSnapDistance.Value * mf.inchOrCm2m;
            Properties.Settings.Default.setAS_snapDistance = snapAdj * 100;
            Properties.Settings.Default.Save();
            mf.Activate();
        }

        private void btnAdjRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack(snapAdj);
            UpdateMoveLabel();
            mf.Activate();
        }

        private void btnAdjLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack(-snapAdj);
            UpdateMoveLabel();
            mf.Activate();
        }

        private void btnSnapToPivot_Click(object sender, EventArgs e)
        {
            mf.trk.SnapToPivot();
            UpdateMoveLabel();
            mf.Activate();
        }

        private void bntOk_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mf.trk.idx > -1 && mf.trk.gArr.Count > 0)
            {
                if (mf.trk.gArr[mf.trk.idx].nudgeDistance < 0)
                    lblOffset.Text = "< " + ((int)(mf.trk.gArr[mf.trk.idx].nudgeDistance * mf.m2InchOrCm * -1)).ToString();
                else
                    lblOffset.Text = ((int)(mf.trk.gArr[mf.trk.idx].nudgeDistance * mf.m2InchOrCm)).ToString() + " >";
            }
        }

        private void btnHalfToolRight_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack((mf.tool.width - mf.tool.overlap) * 0.5);
            UpdateMoveLabel();
            mf.Activate();
        }

        private void btnHalfToolLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack((mf.tool.width - mf.tool.overlap) * -0.5);
            UpdateMoveLabel();
            mf.Activate();
        }

        protected override CreateParams CreateParams
        {
            get
            {
                // Enable drop shadow for a borderless form (CS_DROPSHADOW = 0x00020000)
                const int CS_DROPSHADOW = 0x00020000;
                var cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_NCLBUTTONDBLCLK = 0x00A3;
            const int WM_SYSCOMMAND = 0x0112;
            const int WM_NCHITTEST = 0x0084;

            const int SC_MAXIMIZE = 0xF030;
            const int HTCLIENT = 0x0001;
            const int HTCAPTION = 0x0002;

            switch (m.Msg)
            {
                // Prevent double-click on caption area from maximizing the form
                case WM_NCLBUTTONDBLCLK:
                    m.Result = IntPtr.Zero;
                    return;

                // Block all system commands that try to maximize the form
                case WM_SYSCOMMAND:
                    if ((m.WParam.ToInt32() & 0xFFF0) == SC_MAXIMIZE)
                    {
                        m.Result = IntPtr.Zero;
                        return;
                    }
                    break;

                // Allow dragging the form when clicking on empty client area (no controls)
                case WM_NCHITTEST:
                    base.WndProc(ref m);
                    if ((int)m.Result == HTCLIENT)
                    {
                        // Convert cursor position to client coordinates
                        Point screen = new Point(m.LParam.ToInt32());
                        Point client = PointToClient(screen);

                        // Check if there is a child control under the cursor
                        var child = GetChildAtPoint(
                            client,
                            GetChildAtPointSkip.Invisible | GetChildAtPointSkip.Disabled
                        );

                        // If no control is under the cursor, treat the area as caption (draggable)
                        if (child == null)
                        {
                            m.Result = (IntPtr)HTCAPTION;
                            return;
                        }
                    }
                    return;
            }

            base.WndProc(ref m);
        }


    }
}

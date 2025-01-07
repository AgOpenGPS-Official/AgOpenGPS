using System;
using System.Drawing;
using System.Windows.Forms;

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
            Size = Properties.Settings.Default.setWindow_formNudgeSize;
            UpdateMoveLabel();

            if (!mf.IsOnScreen(Location, Size, 1))
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
            Properties.Settings.Default.setWindow_formNudgeSize = Size;

            Properties.Settings.Default.Save();

            //save entire list
            mf.FileSaveTracks();
        }

        private void UpdateMoveLabel()
        {
            if (mf.trk.idx > -1)
            {
                if (mf.trk.gArr[mf.trk.idx].nudgeDistance == 0 )
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
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);
            snapAdj = (double)nudSnapDistance.Value * mf.inchOrCm2m;
            Properties.Settings.Default.setAS_snapDistance = snapAdj*100;
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
            mf.trk.NudgeTrack((mf.tool.width-mf.tool.overlap) * 0.5);
            UpdateMoveLabel();
            mf.Activate();
        }

        private void btnHalfToolLeft_Click(object sender, EventArgs e)
        {
            mf.trk.NudgeTrack((mf.tool.width - mf.tool.overlap) * -0.5);
            UpdateMoveLabel();
            mf.Activate();
        }

        protected override void WndProc(ref Message m)
        {
            const int RESIZE_HANDLE_SIZE = 10;

            switch (m.Msg)
            {
                case 0x0084/*NCHITTEST*/ :
                    base.WndProc(ref m);

                    if ((int)m.Result == 0x01/*HTCLIENT*/)
                    {
                        Point screenPoint = new Point(m.LParam.ToInt32());
                        Point clientPoint = this.PointToClient(screenPoint);
                        if (clientPoint.Y <= RESIZE_HANDLE_SIZE)
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)13/*HTTOPLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)12/*HTTOP*/ ;
                            else
                                m.Result = (IntPtr)14/*HTTOPRIGHT*/ ;
                        }
                        else if (clientPoint.Y <= (Size.Height - RESIZE_HANDLE_SIZE))
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)10/*HTLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)2/*HTCAPTION*/ ;
                            else
                                m.Result = (IntPtr)11/*HTRIGHT*/ ;
                        }
                        else
                        {
                            if (clientPoint.X <= RESIZE_HANDLE_SIZE)
                                m.Result = (IntPtr)16/*HTBOTTOMLEFT*/ ;
                            else if (clientPoint.X < (Size.Width - RESIZE_HANDLE_SIZE))
                                m.Result = (IntPtr)15/*HTBOTTOM*/ ;
                            else
                                m.Result = (IntPtr)17/*HTBOTTOMRIGHT*/ ;
                        }
                    }
                    return;
            }
            base.WndProc(ref m);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                //drop shadow
                CreateParams cp = base.CreateParams;
                cp.Style |= 0x20000; // <--- use 0x20000
                return cp;
            }
        }

    }
}

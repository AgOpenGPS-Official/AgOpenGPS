using AgOpenGPS.Culture;
using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSmoothAB : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private int smoothCount = 20;

        public FormSmoothAB(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.bntOK.Text = gStr.gsForNow;
            this.btnSave.Text = gStr.gsToFile;

            this.Text = gStr.gsSmoothABCurve;
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.curve.isSmoothWindowOpen = false;
            mf.curve.SaveSmoothList();
            mf.curve.smooList?.Clear();
            Close();
        }

        private void FormSmoothAB_Load(object sender, EventArgs e)
        {
            mf.curve.isSmoothWindowOpen = true;
            smoothCount = 20;
            lblSmooth.Text = "**";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            mf.curve.isSmoothWindowOpen = false;
            mf.curve.smooList?.Clear();
            Close();
        }

        private void btnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            if (smoothCount++ > 100) smoothCount = 100;
            mf.curve.SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            smoothCount--;
            if (smoothCount < 2) smoothCount = 2;
            mf.curve.SmoothAB(smoothCount * 2);
            lblSmooth.Text = smoothCount.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            mf.curve.isSmoothWindowOpen = false;
            mf.curve.SaveSmoothList();
            mf.curve.smooList?.Clear();

            //save entire list
            mf.FileSaveTracks();

            //mf.FileSaveCurveLines();
            Close();
        }
    }
}
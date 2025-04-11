//Please, if you use this give me some credit
//Copyright BrianTee, copy right out of it.

using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormPan : Form
    {
        private readonly FormGPS mf = null;
        private const double AdjustFactor = 0.04;

        public FormPan(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
        }

        private void btnPanUp_Click(object sender, EventArgs e)
        {
            mf.camera.PanY += AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnPanDn_Click(object sender, EventArgs e)
        {
            mf.camera.PanY -= AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnPanRight_Click(object sender, EventArgs e)
        {
            mf.camera.PanX += AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnPanLeft_Click(object sender, EventArgs e)
        {
            mf.camera.PanX -= AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnUpLeft_Click(object sender, EventArgs e)
        {
            mf.camera.PanY += AdjustFactor * mf.camera.camSetDistance;
            mf.camera.PanX -= AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnDownRight_Click(object sender, EventArgs e)
        {
            mf.camera.PanY -= AdjustFactor * mf.camera.camSetDistance;
            mf.camera.PanX += AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnUpRight_Click(object sender, EventArgs e)
        {
            mf.camera.PanY += AdjustFactor * mf.camera.camSetDistance;
            mf.camera.PanX += AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnDownLeft_Click(object sender, EventArgs e)
        {
            mf.camera.PanY -= AdjustFactor * mf.camera.camSetDistance;
            mf.camera.PanX -= AdjustFactor * mf.camera.camSetDistance;
        }

        private void btnPanCancel_Click(object sender, EventArgs e)
        {
            mf.camera.PanX = 0;
            mf.camera.PanY = 0;
            mf.isPanFormVisible = false;
            Close();
        }

        private void FormPan_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.isPanFormVisible = false;
        }
    }
}

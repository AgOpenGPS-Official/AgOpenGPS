using AgOpenGPS.Culture;
using System;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgOpenGPS
{
    public partial class FormCorrection : Form
    {
        private readonly FormGPS mf = null;
        private bool isPole = true;

        //chart data
        private string roll = "0.1";

        private string east = "0";
        private string ost = "0";

        private bool isScroll = true;

        public FormCorrection(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.Text = gStr.gsRollCorrectionGraph;
            label3.Text = gStr.gsAntenna;
            label5.Text = gStr.gsCorrectedEasting;
            label1.Text = gStr.gsCorrectionDistance;
            label6.Text = gStr.gsRollDegrees;

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            {
                roll = (mf.correctionDistanceGraph * 20).ToString("N2", CultureInfo.InvariantCulture);
                east = (mf.pn.fix.easting * 20).ToString("N2", CultureInfo.InvariantCulture);
                ost = (mf.uncorrectedEastingGraph * 20).ToString("N2", CultureInfo.InvariantCulture);

                if (!isPole) roll = ((mf.correctionDistanceGraph + mf.uncorrectedEastingGraph) * 20).ToString("N2", CultureInfo.InvariantCulture);

                lblCorrectionDistance.Text = (mf.correctionDistanceGraph).ToString("N2", CultureInfo.InvariantCulture); ;
                lblEast.Text = (mf.pn.fix.easting).ToString("N2", CultureInfo.InvariantCulture); ;
                lblOst.Text = (mf.uncorrectedEastingGraph).ToString("N2", CultureInfo.InvariantCulture);
                lblRollDegrees.Text = (mf.RollInDegrees);
                lblEastOnGraph.Text = ((int)(mf.pn.fix.easting * 100)).ToString(CultureInfo.InvariantCulture);
            }

            if (isScroll)
            {
                //chart data
                Series r = rollChart.Series["Ro"];
                Series t = rollChart.Series["Ze"];
                Series u = rollChart.Series["Oe"];

                double nextx6 = 1;
                double nextx7 = 1;
                double nextx8 = 1;

                if (r.Points.Count > 0) nextx6 = r.Points[r.Points.Count - 1].XValue + 1;
                if (t.Points.Count > 0) nextx7 = t.Points[t.Points.Count - 1].XValue + 1;
                if (u.Points.Count > 0) nextx8 = u.Points[u.Points.Count - 1].XValue + 1;

                rollChart.Series["Ro"].Points.AddXY(nextx6, roll);
                rollChart.Series["Ze"].Points.AddXY(nextx7, east);
                rollChart.Series["Oe"].Points.AddXY(nextx8, ost);

                while (r.Points.Count > 50)
                {
                    r.Points.RemoveAt(0);
                }
                while (t.Points.Count > 50)
                {
                    t.Points.RemoveAt(0);
                }
                while (u.Points.Count > 50)
                {
                    u.Points.RemoveAt(0);
                }
                //rollChart.ChartAreas[0].RecalculateAxesScale();
                rollChart.ResetAutoValues();
            }
        }

        private void FormSteerGraph_Load(object sender, EventArgs e)
        {
            timer1.Interval = (int)((1 / mf.gpsHz) * 1000);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        //private void btnAuto_Click(object sender, EventArgs e)
        //{
        //    unoChart.ChartAreas[0].AxisY.Maximum = Double.NaN;
        //    unoChart.ChartAreas[0].AxisY.Minimum = Double.NaN;
        //    unoChart.ChartAreas[0].RecalculateAxesScale();
        //    unoChart.ResetAutoValues();
        //}

        //private void btnPlus_Click(object sender, EventArgs e)
        //{
        //    if (Math.Abs(unoChart.ChartAreas[0].AxisY.Minimum) > Math.Abs(unoChart.ChartAreas[0].AxisY.Maximum))
        //        unoChart.ChartAreas[0].AxisY.Maximum = Math.Abs(unoChart.ChartAreas[0].AxisY.Minimum);
        //    else unoChart.ChartAreas[0].AxisY.Minimum = Math.Abs(unoChart.ChartAreas[0].AxisY.Maximum) * -1;
        //    unoChart.ChartAreas[0].AxisY.Minimum *=1.5;
        //    unoChart.ChartAreas[0].AxisY.Maximum *=1.5;

        //    unoChart.ChartAreas[0].AxisY.Minimum = (int)unoChart.ChartAreas[0].AxisY.Minimum;
        //    unoChart.ChartAreas[0].AxisY.Maximum = (int)unoChart.ChartAreas[0].AxisY.Maximum;

        //    unoChart.ResetAutoValues();

        //}

        //private void btnMinus_Click(object sender, EventArgs e)
        //{
        //    if (Math.Abs(unoChart.ChartAreas[0].AxisY.Minimum) > Math.Abs(unoChart.ChartAreas[0].AxisY.Maximum))
        //        unoChart.ChartAreas[0].AxisY.Maximum = Math.Abs(unoChart.ChartAreas[0].AxisY.Minimum);
        //    else unoChart.ChartAreas[0].AxisY.Minimum = Math.Abs(unoChart.ChartAreas[0].AxisY.Maximum) * -1;

        //    unoChart.ChartAreas[0].AxisY.Minimum *=0.75;
        //    unoChart.ChartAreas[0].AxisY.Maximum *=0.75;

        //    if (unoChart.ChartAreas[0].AxisY.Maximum <=10)
        //    {
        //        unoChart.ChartAreas[0].AxisY.Maximum =  10;
        //        unoChart.ChartAreas[0].AxisY.Minimum = -10;
        //    }

        //    unoChart.ChartAreas[0].AxisY.Minimum = (int)unoChart.ChartAreas[0].AxisY.Minimum;
        //    unoChart.ChartAreas[0].AxisY.Maximum = (int)unoChart.ChartAreas[0].AxisY.Maximum;

        //    unoChart.ResetAutoValues();
        //}

        private void btnScroll_Click(object sender, EventArgs e)
        {
            isScroll = !isScroll;
        }

        private void btnZero_Click(object sender, EventArgs e)
        {
            //roond = (int)(mf.fixedEasting);
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }

        private void btnScroll_Click_1(object sender, EventArgs e)
        {
            isScroll = !isScroll;
        }

        private void btnPoleOrMoving_Click(object sender, EventArgs e)
        {
            isPole = !isPole;
            if (isPole) btnPoleOrMoving.Text = "Pole";
            else btnPoleOrMoving.Text = "Moving";
        }
    }
}
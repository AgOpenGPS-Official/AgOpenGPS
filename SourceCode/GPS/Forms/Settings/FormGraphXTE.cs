using System;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgOpenGPS
{
    public partial class FormGraphXTE : Form
    {
        private readonly FormGPS mf = null;

        //chart data
        private string dataSteerAngle = "0";

        private string dataPWM = "0";

        private bool isAuto = true;

        public FormGraphXTE(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();

            this.label5.Text = "HE";
            this.label1.Text = "XTE";

            this.Text = "XTE Chart";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            DrawChart();
        }

        private void DrawChart()
        {
            {
                //word 0 - steerangle, 1 - pwmDisplay
                //dataSteerAngle = mf.mc.actualSteerAngleChart.ToString(CultureInfo.InvariantCulture);

                dataPWM = ((int)(mf.vehicle.modeActualXTE * 100)).ToString(CultureInfo.InvariantCulture);

                dataSteerAngle = (Math.Round(mf.vehicle.modeActualHeadingError, 1)).ToString(CultureInfo.InvariantCulture);

                lblSteerAng.Text = dataSteerAngle + "\u00B0";
                lblPWM.Text = dataPWM + " cm";
            }

            //chart data
            Series s = unoChart.Series["S"];
            Series w = unoChart.Series["PWM"];
            double nextX = 1;
            double nextX5 = 1;

            if (s.Points.Count > 0) nextX = s.Points[s.Points.Count - 1].XValue + 1;
            if (w.Points.Count > 0) nextX5 = w.Points[w.Points.Count - 1].XValue + 1;

            unoChart.Series["S"].Points.AddXY(nextX, dataSteerAngle);
            unoChart.Series["PWM"].Points.AddXY(nextX5, dataPWM);

            //if (isScroll)
            while (s.Points.Count > 120)
            {
                s.Points.RemoveAt(0);
            }
            while (w.Points.Count > 120)
            {
                w.Points.RemoveAt(0);
            }
            unoChart.ResetAutoValues();
        }

        private void FormSteerGraph_Load(object sender, EventArgs e)
        {
            timer1.Interval = (int)((1 / mf.gpsHz) * 1000);

            unoChart.ChartAreas[0].AxisY.Minimum = -80;
            unoChart.ChartAreas[0].AxisY.Maximum = 80;
            unoChart.ResetAutoValues();

            lblMax.Text = ((int)(unoChart.ChartAreas[0].AxisY.Maximum)).ToString() + " cm";
            lblMin.Text = ((int)(unoChart.ChartAreas[0].AxisY.Minimum)).ToString() + " cm";

            isAuto = false;
            //lblMax.Text = "Auto";
            //lblMin.Text = "0";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnGainUp_Click(object sender, EventArgs e)
        {
            if (isAuto)
            {
                unoChart.ChartAreas[0].AxisY.Minimum = -80;
                unoChart.ChartAreas[0].AxisY.Maximum = 80;
                unoChart.ResetAutoValues();
                lblMax.Text = (unoChart.ChartAreas[0].AxisY.Maximum).ToString() + " cm";
                lblMin.Text = (unoChart.ChartAreas[0].AxisY.Minimum).ToString() + " cm";
                isAuto = false;
                return;
            }

            if (unoChart.ChartAreas[0].AxisY.Maximum >= 5120)
            {
                unoChart.ChartAreas[0].AxisY.Minimum = -5120;
                unoChart.ChartAreas[0].AxisY.Maximum = 5120;
                unoChart.ResetAutoValues();
                lblMax.Text = (unoChart.ChartAreas[0].AxisY.Maximum).ToString() + " cm";
                lblMin.Text = (unoChart.ChartAreas[0].AxisY.Minimum).ToString() + " cm";
                return;
            }

            unoChart.ChartAreas[0].AxisY.Minimum *= 2;
            unoChart.ChartAreas[0].AxisY.Maximum *= 2;
            unoChart.ChartAreas[0].AxisY.Minimum = (int)unoChart.ChartAreas[0].AxisY.Minimum;
            unoChart.ChartAreas[0].AxisY.Maximum = (int)unoChart.ChartAreas[0].AxisY.Maximum;
            unoChart.ResetAutoValues();
            lblMax.Text = (unoChart.ChartAreas[0].AxisY.Maximum).ToString() + " cm";
            lblMin.Text = (unoChart.ChartAreas[0].AxisY.Minimum).ToString() + " cm";
        }

        private void btnGainAuto_Click(object sender, EventArgs e)
        {
            unoChart.ChartAreas[0].AxisY.Maximum = Double.NaN;
            unoChart.ChartAreas[0].AxisY.Minimum = Double.NaN;
            unoChart.ChartAreas[0].RecalculateAxesScale();
            unoChart.ResetAutoValues();
            lblMax.Text = "Auto";
            lblMin.Text = "";
            isAuto = true;
        }

        private void btnGainDown_Click(object sender, EventArgs e)
        {
            if (isAuto)
            {
                unoChart.ChartAreas[0].AxisY.Minimum = -80;
                unoChart.ChartAreas[0].AxisY.Maximum = 80;
                unoChart.ResetAutoValues();
                lblMax.Text = (unoChart.ChartAreas[0].AxisY.Maximum).ToString() + " cm";
                lblMin.Text = (unoChart.ChartAreas[0].AxisY.Minimum).ToString() + " cm";
                isAuto = false;
                return;
            }

            if (unoChart.ChartAreas[0].AxisY.Minimum >= -19.999999)
            {
                unoChart.ChartAreas[0].AxisY.Minimum = -10;
                unoChart.ChartAreas[0].AxisY.Maximum = 10;
                unoChart.ResetAutoValues();
                lblMax.Text = (unoChart.ChartAreas[0].AxisY.Maximum).ToString() + " cm";
                lblMin.Text = (unoChart.ChartAreas[0].AxisY.Minimum).ToString() + " cm";
                return;
            }

            unoChart.ChartAreas[0].AxisY.Minimum *= 0.5;
            unoChart.ChartAreas[0].AxisY.Maximum *= 0.5;
            unoChart.ChartAreas[0].AxisY.Minimum = (int)unoChart.ChartAreas[0].AxisY.Minimum;
            unoChart.ChartAreas[0].AxisY.Maximum = (int)unoChart.ChartAreas[0].AxisY.Maximum;
            unoChart.ResetAutoValues();
            lblMax.Text = (unoChart.ChartAreas[0].AxisY.Maximum).ToString() + " cm";
            lblMin.Text = (unoChart.ChartAreas[0].AxisY.Minimum).ToString() + " cm";
        }
    }
}
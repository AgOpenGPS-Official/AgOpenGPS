//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AgOpenGPS
{
    public partial class FormNozConfig : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private string unitsSet = "10";
        private string unitsActual = "0";


        //Nozzz constructor
        public FormNozConfig(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = "Controller Configuration";
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            nudSprayFlowCal.Value = (decimal)(Properties.Settings.Default.setNozzleSettings.flowCal);
            nudSprayPressureCal.Value = (decimal)(Properties.Settings.Default.setNozzleSettings.pressureCal);

            nudSprayKp.Value = Properties.Settings.Default.setNozzleSettings.Kp;
            //nudSprayKi.Value = Properties.Settings.Default.setNozzleSettings.Ki;

            nudFastPWM.Value = Properties.Settings.Default.setNozzleSettings.fastPWM;
            nudSlowPWM.Value = Properties.Settings.Default.setNozzleSettings.slowPWM;
            nudDeadbandError.Value = (decimal)(Properties.Settings.Default.setNozzleSettings.deadbandError) * 0.01M;
            nudSwitchAtFlowError.Value = (decimal)(Properties.Settings.Default.setNozzleSettings.switchAtFlowError) * 0.01M;

            cboxBypass.Checked = Properties.Settings.Default.setNozzleSettings.isBypass;
            if (cboxBypass.Checked)
            {
                cboxBypass.Text = "Bypass";
            }
            else
            {
                cboxBypass.Text = "Closed";
            }

            tboxUnitsApplied.Text = mf.nozz.unitsApplied.Trim();
            tboxUnitsPerArea.Text = mf.nozz.unitsPerArea.Trim();

            cboxSectionValve3Wire.Checked = Properties.Settings.Default.setNozzleSettings.isSectionValve3Wire;
            if (cboxSectionValve3Wire.Checked)
            {
                cboxSectionValve3Wire.Text = "3 Wire";
            }
            else
            {
                cboxSectionValve3Wire.Text = "Reverse";
            }
            

        }

        private void nudSprayFlowCal_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.flowCal = (int)nudSprayFlowCal.Value;
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.flowCalHi] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.flowCal >> 8)); ;
            mf.p_226.pgn[mf.p_226.flowCaLo] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.flowCal));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void nudSprayPressureCal_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.pressureCal = (int)nudSprayPressureCal.Value;
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.pressureCalHi] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.pressureCal >> 8)); ;
            mf.p_226.pgn[mf.p_226.pressureCalLo] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.pressureCal));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void nudFastPWM_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.fastPWM = (byte)nudFastPWM.Value;
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.fastPWM] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.fastPWM));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void nudSlowPWM_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.slowPWM = (byte)nudSlowPWM.Value;
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.slowPWM] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.slowPWM));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void nudDeadbandError_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.deadbandError = (byte)(nudDeadbandError.Value * 100);
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.deadbandError] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.deadbandError));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void nudSwitchAtFlowError_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.switchAtFlowError = (byte)(nudSwitchAtFlowError.Value * 100);
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.switchAtFlowError] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.switchAtFlowError));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void nudSprayKpClick(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.Kp = (byte)nudSprayKp.Value;
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.Kp] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.Kp));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void cboxSectionValve3Wire_Click(object sender, EventArgs e)
        {
            if (cboxSectionValve3Wire.Checked)
            {
                cboxSectionValve3Wire.Text = "3 Wire";
                mf.nozz.isSectionValve3Wire = true;
                mf.p_226.pgn[mf.p_226.isSectionValve3Wire] = 1;
            }
            else
            {
                cboxSectionValve3Wire.Text = "Reverse";
                mf.nozz.isSectionValve3Wire = false;
                mf.p_226.pgn[mf.p_226.isSectionValve3Wire] = 0;
            }

            Properties.Settings.Default.setNozzleSettings.isSectionValve3Wire = mf.nozz.isSectionValve3Wire;

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void cboxBypass_Click(object sender, EventArgs e)
        {
            if (cboxBypass.Checked)
            {
                cboxBypass.Text = "Bypass";
                mf.nozz.isBypass = true;
                mf.p_226.pgn[mf.p_226.isBypass] = 1;
            }
            else
            {
                cboxBypass.Text = "Closed";
                mf.nozz.isBypass = false;
                mf.p_226.pgn[mf.p_226.isBypass] = 0;
            }

            Properties.Settings.Default.setNozzleSettings.isBypass = mf.nozz.isBypass;

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnSprayAcres.Text = mf.nozz.pwmDriveActual.ToString();
            DrawChart();
        }

        private void DrawChart()
        {
            unitsSet = lblUnitsSet.Text = mf.nozz.volumePerMinuteSet.ToString();
            unitsActual = lblUnitsActual.Text = mf.nozz.volumePerMinuteActual.ToString();
            //chart data
            Series s = unoChart.Series["S"];
            Series w = unoChart.Series["PWM"];

            double nextX = 1;
            double nextX5 = 1;

            if (s.Points.Count > 0) nextX = s.Points[s.Points.Count - 1].XValue + 1;
            if (w.Points.Count > 0) nextX5 = w.Points[w.Points.Count - 1].XValue + 1;

            unoChart.Series["S"].Points.AddXY(nextX, unitsSet);
            unoChart.Series["PWM"].Points.AddXY(nextX5, unitsActual);

            while (s.Points.Count > 100)
            {
                s.Points.RemoveAt(0);
            }
            while (w.Points.Count > 100)
            {
                w.Points.RemoveAt(0);
            }
            unoChart.ChartAreas[0].RecalculateAxesScale();
        }


        private void tboxUnitsApplied_TextChanged(object sender, EventArgs e)
        {
            mf.nozz.unitsApplied = " " + tboxUnitsApplied.Text.Trim();
            Properties.Settings.Default.setNozzleSettings.unitsApplied = mf.nozz.unitsApplied;
        }

        private void tboxUnitsPerArea_TextChanged(object sender, EventArgs e)
        {
            mf.nozz.unitsPerArea = " " + tboxUnitsPerArea.Text.Trim();
            Properties.Settings.Default.setNozzleSettings.unitsPerArea = mf.nozz.unitsPerArea;
        }

        private void FormNozConfig_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!mf.nozz.isAppliedUnitsNotTankDisplayed)
                mf.lbl_Volume.Text = "Tank " + mf.nozz.unitsApplied;
            else
                mf.lbl_Volume.Text = "App " + mf.nozz.unitsApplied;

            Settings.Default.Save();
        }
    }
}
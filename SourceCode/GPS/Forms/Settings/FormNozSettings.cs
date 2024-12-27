//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormNozSettings : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        //Nozzz constructor
        public FormNozSettings(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = "Sprayer Settings";
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            nudSprayRateSet1.Value = (decimal)(Properties.Settings.Default.setNozzleSettings.volumePerAreaSet1);
            nudSprayRateSet2.Value = (decimal)(Properties.Settings.Default.setNozzleSettings.volumePerAreaSet2);
            nudSprayMinPressure.Value = Properties.Settings.Default.setNozzleSettings.pressureMin;

            if (mf.isMetric)
            {
                lblRateSet1.Text = mf.nozz.unitsPerArea;
                lblRateSet2.Text = mf.nozz.unitsPerArea;
                nudSprayRateSet1.Maximum = 999M;
                nudSprayRateSet1.Minimum = 5M;
                nudSprayRateSet1.DecimalPlaces = 0;
                nudSprayRateSet2.Maximum = 999M;
                nudSprayRateSet2.Minimum = 5M;
                nudSprayRateSet2.DecimalPlaces = 0;

                lblVolumeTank.Text = mf.nozz.volumeTankStart.ToString();
                lblVolumeApplied.Text = mf.nozz.volumeApplied.ToString();
                lblRateSet.Text = mf.nozz.unitsApplied + " Applied";
                lblStatArea.Text = "Ha";
            }
            else
            {
                lblRateSet1.Text = mf.nozz.unitsPerArea;
                lblRateSet2.Text = mf.nozz.unitsPerArea;
                nudSprayRateSet1.Maximum = 99.9M;
                nudSprayRateSet1.Minimum = 1M;
                nudSprayRateSet1.DecimalPlaces = 1;
                nudSprayRateSet2.Maximum = 99.9M;
                nudSprayRateSet2.Minimum = 1M;
                nudSprayRateSet2.DecimalPlaces = 1;
                lblRateSet.Text = mf.nozz.unitsApplied + "Applied";
                lblStatArea.Text = "Acre";
            }

            nudTankVolume.Value = Properties.Settings.Default.setNozzleSettings.volumeTankStart;
            nudZeroVolume.Value = (decimal)mf.nozz.volumeApplied;

            lblVolumeTank.Text = mf.nozz.volumeTankStart.ToString();
            lblVolumeApplied.Text = mf.nozz.volumeApplied.ToString("N1");
            lblTankRemain.Text = (mf.nozz.volumeTankStart - mf.nozz.volumeApplied).ToString("N1");
            lblAcresAvailable.Text = ((mf.nozz.volumeTankStart - mf.nozz.volumeApplied) / mf.nozz.volumePerAreaSetSelected).ToString("N1");
            
            nudNudge.Value = (decimal)Properties.Settings.Default.setNozzleSettings.rateNudge;
            nudRateAlarmPercent.Value = (int)(mf.nozz.rateAlarmPercent * 100);
        }

        private void nudSprayRateSet1_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (mf.isMetric)
                {
                    nudSprayRateSet1.Value = Math.Round(nudSprayRateSet1.Value, 0);
                }
                mf.nozz.volumePerAreaSet1 = (double)nudSprayRateSet1.Value;

                Properties.Settings.Default.setNozzleSettings.volumePerAreaSet1 = (double)nudSprayRateSet1.Value;
                Settings.Default.Save();
            }
        }
        private void nudSprayRateSet2_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (mf.isMetric)
                {
                    nudSprayRateSet2.Value = Math.Round(nudSprayRateSet2.Value, 0);
                }

                mf.nozz.volumePerAreaSet2 = (double)nudSprayRateSet2.Value;
                Properties.Settings.Default.setNozzleSettings.volumePerAreaSet2 = (double)nudSprayRateSet2.Value;
                Settings.Default.Save();
            }
        }

        private void nudSprayMinPressure_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);

            Properties.Settings.Default.setNozzleSettings.pressureMin = (int)nudSprayMinPressure.Value;
            Settings.Default.Save();

            mf.p_226.pgn[mf.p_226.minPressure] = unchecked((byte)(Properties.Settings.Default.setNozzleSettings.pressureMin));

            mf.SendPgnToLoop(mf.p_226.pgn);
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 0;
            mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;

            Settings.Default.Save();
            Close();
        }

        private void nudTankVolume_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.nozz.volumeTankStart = (int)nudTankVolume.Value;
                Properties.Settings.Default.setNozzleSettings.volumeTankStart = mf.nozz.volumeTankStart;
            }
        }

        private void nudZeroVolume_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (nudZeroVolume.Value < 2)
                {
                    mf.nozz.volumeApplied = 0;

                    mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 1;
                    mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;

                    mf.SendPgnToLoop(mf.p_225.pgn);

                    mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 0;
                    mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;
                }
                else
                {
                    mf.nozz.volumeApplied = (double)nudZeroVolume.Value;

                    int vol = (int)nudZeroVolume.Value;

                    mf.p_226.pgn[mf.p_226.flowCalHi] = unchecked((byte)(vol >> 8));
                    mf.p_226.pgn[mf.p_226.flowCaLo] = unchecked((byte)(vol));


                    mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 1;
                    mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;

                    mf.SendPgnToLoop(mf.p_225.pgn);

                    mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 0;
                    mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;
                }
            }
        }

        private void btnZeroVolume_Click(object sender, EventArgs e)
        {
            mf.nozz.volumeApplied = 0;
            nudZeroVolume.Value = 0;

            mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 1;
            mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;

            mf.SendPgnToLoop(mf.p_225.pgn);

            mf.p_225.pgn[mf.p_225.zeroTankVolumeLo] = 0;
            mf.p_225.pgn[mf.p_225.zeroTankVolumeHi] = 0;
        }

        private void timer1_Tick(object sender, EventArgs e)
        { 
            lblVolumeTank.Text = mf.nozz.volumeTankStart.ToString();
            lblVolumeApplied.Text = mf.nozz.volumeApplied.ToString("N1");
            lblTankRemain.Text = (mf.nozz.volumeTankStart - mf.nozz.volumeApplied).ToString("N1");
            lblAcresAvailable.Text = ((mf.nozz.volumeTankStart - mf.nozz.volumeApplied) / mf.nozz.volumePerAreaSetSelected).ToString("N1");
        }

        private void nudNudge_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setNozzleSettings.rateNudge = (double)nudNudge.Value;
                Settings.Default.Save();
                mf.nozz.rateNudge = (double)nudNudge.Value;
            }
        }

        private void nudRateAlarmPercent_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.nozz.rateAlarmPercent = (double)nudRateAlarmPercent.Value * 0.01;
                Properties.Settings.Default.setNozzleSettings.rateAlarmPercent = mf.nozz.rateAlarmPercent;
                Settings.Default.Save();
            }
        }
    }
}
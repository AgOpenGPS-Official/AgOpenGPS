using System;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public partial class FormAutoTuning : Form
    {
        private readonly FormGPS mf;
        private readonly Timer updateTimer;

        public FormAutoTuning(FormGPS mainForm)
        {
            if (mainForm == null)
                throw new ArgumentNullException(nameof(mainForm), "FormGPS instance cannot be null");
                
            mf = mainForm;
            InitializeComponent();
            
            // Initialize timer for real-time updates
            updateTimer = new Timer();
            updateTimer.Interval = 500; // Update every 500ms
            updateTimer.Tick += UpdateTimer_Tick;
            
            // Set initial values
            LoadCurrentSettings();
        }

        private void FormAutoTuning_Load(object sender, EventArgs e)
        {
            // Set window position and size
            this.Size = new Size(500, 700);
            this.Text = "Auto-Tuning Settings";
            this.StartPosition = FormStartPosition.CenterParent;
            
            // Initialize auto-tuner if needed
            if (mf.autoTuner == null)
            {
                mf.autoTuner = new CAutoTuner(mf);
            }
            
            UpdateDisplayValues();
            updateTimer.Start();
        }

        private void FormAutoTuning_FormClosing(object sender, FormClosingEventArgs e)
        {
            updateTimer.Stop();
            
            // Save settings
            Properties.Settings.Default.Save();
        }

        private void LoadCurrentSettings()
        {
            if (mf.autoTuner != null)
            {
                // Load current auto-tuning settings
                chkAutoTuningEnabled.Checked = mf.autoTuner.IsAutoTuningEnabled;
                
                // Load configuration values
                if (mf.autoTuner.Config != null)
                {
                    nudLearningRate.Value = (decimal)(mf.autoTuner.Config.LearningRate * 100);
                    nudAdaptationSpeed.Value = (decimal)(mf.autoTuner.Config.AdaptationSpeed * 100);
                    
                    // Load new enhanced settings
                    nudMaxSimultaneousAdjustments.Value = mf.autoTuner.Config.MaxSimultaneousAdjustments;
                    hsbarAccuracyVsSmoothness.Value = (int)(mf.autoTuner.Config.AccuracyVsSmoothness * 100);
                    lblMaxSimultaneousAdjustments.Text = mf.autoTuner.Config.MaxSimultaneousAdjustments.ToString();
                    lblAccuracyVsSmoothness.Text = (mf.autoTuner.Config.AccuracyVsSmoothness * 100).ToString("F0") + "%";
                }
            }
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            if (mf.autoTuner != null && this.Visible)
            {
                UpdateDisplayValues();
            }
        }

        private void UpdateDisplayValues()
        {
            if (mf.autoTuner == null) return;

            // Update status
            lblAutoTuningStatus.Text = mf.autoTuner.IsAutoTuningEnabled ? "ACTIVE" : "DISABLED";
            lblAutoTuningStatus.ForeColor = mf.autoTuner.IsAutoTuningEnabled ? Color.Green : Color.Red;
            
            lblLearningStatus.Text = mf.autoTuner.IsLearning ? "LEARNING" : "WAITING";
            lblLearningStatus.ForeColor = mf.autoTuner.IsLearning ? Color.Blue : Color.Gray;

            // Update performance metrics
            if (mf.autoTuner.IsAutoTuningEnabled)
            {
                lblCrossTrackError.Text = mf.autoTuner.GetAverageCrossTrackError().ToString("F3") + " m";
                lblOscillation.Text = mf.autoTuner.GetOscillationMagnitude().ToString("F2") + "°";
                lblOvershoot.Text = (mf.autoTuner.GetOvershootLevel() * 100).ToString("F1") + "%";
                lblPerformanceScore.Text = mf.autoTuner.GetCurrentPerformanceScore().ToString("F2");
                
                // Update progress bar for performance
                double score = mf.autoTuner.GetCurrentPerformanceScore();
                progressPerformance.Value = Math.Min(100, (int)(score * 10));
            }
            else
            {
                lblCrossTrackError.Text = "N/A";
                lblOscillation.Text = "N/A";
                lblOvershoot.Text = "N/A";
                lblPerformanceScore.Text = "N/A";
                progressPerformance.Value = 0;
            }

            // Update current parameter values
            if (mf.autoTuner.Config != null)
            {
                lblCurrentProportional.Text = mf.autoTuner.Config.ProportionalGain.ToString("F1");
                lblCurrentMaxLimit.Text = mf.autoTuner.Config.MaxLimit.ToString("F0");
                lblCurrentMinMove.Text = mf.autoTuner.Config.SteerResponse.ToString("F1");
                lblCurrentIntegral.Text = mf.autoTuner.Config.Integral.ToString("F0");
                lblCurrentSpeedFactor.Text = mf.autoTuner.Config.SpeedFactor.ToString("F2");
                lblCurrentAcquireFactor.Text = mf.autoTuner.Config.AcquireFactor.ToString("F2");
            }
        }

        private void chkAutoTuningEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (mf.autoTuner != null)
            {
                mf.autoTuner.IsAutoTuningEnabled = chkAutoTuningEnabled.Checked;
                
                if (mf.autoTuner.IsAutoTuningEnabled)
                {
                    mf.autoTuner.StartLearning();
                    btnStartLearning.Text = "Stop Learning";
                    btnStartLearning.BackColor = Color.Orange;
                    
                    // Show message
                    mf.TimedMessageBox(3000, "Auto-Tuning Activated", 
                        "The system will automatically tune steering parameters.");
                }
                else
                {
                    mf.autoTuner.StopLearning();
                    btnStartLearning.Text = "Start Learning";
                    btnStartLearning.BackColor = Color.LightGreen;
                    
                    mf.TimedMessageBox(3000, "Auto-Tuning Disabled", 
                        "Returning to manual parameter settings.");
                }
            }
        }

        private void btnStartLearning_Click(object sender, EventArgs e)
        {
            if (mf.autoTuner == null) return;

            if (mf.autoTuner.IsLearning)
            {
                mf.autoTuner.StopLearning();
                btnStartLearning.Text = "Start Learning";
                btnStartLearning.BackColor = Color.LightGreen;
            }
            else
            {
                mf.autoTuner.StartLearning();
                btnStartLearning.Text = "Stop Learning";
                btnStartLearning.BackColor = Color.Orange;
            }
        }

        private void btnResetToDefaults_Click(object sender, EventArgs e)
        {
            if (mf.autoTuner != null)
            {
                DialogResult result = MessageBox.Show(
                    "Are you sure you want to reset to default values?\nThis will delete all learned settings.",
                    "Confirm Reset",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    mf.autoTuner.ResetToManualDefaults();
                    mf.TimedMessageBox(2000, "Reset Complete", "Parameters reset to default values.");
                }
            }
        }

        private void nudLearningRate_ValueChanged(object sender, EventArgs e)
        {
            if (mf.autoTuner?.Config != null)
            {
                mf.autoTuner.Config.LearningRate = (double)nudLearningRate.Value / 100.0;
            }
        }

        private void nudAdaptationSpeed_ValueChanged(object sender, EventArgs e)
        {
            if (mf.autoTuner?.Config != null)
            {
                mf.autoTuner.Config.AdaptationSpeed = (double)nudAdaptationSpeed.Value / 100.0;
            }
        }

        private void btnSaveConfig_Click(object sender, EventArgs e)
        {
            if (mf.autoTuner != null)
            {
                mf.autoTuner.SaveConfig();
                mf.TimedMessageBox(2000, "Saved", "Auto-tuning configuration saved.");
            }
        }

        private void btnLoadConfig_Click(object sender, EventArgs e)
        {
            if (mf.autoTuner != null)
            {
                mf.autoTuner.LoadConfig();
                LoadCurrentSettings();
                mf.TimedMessageBox(2000, "Loaded", "Auto-tuning configuration loaded.");
            }
        }
        
        private void nudMaxSimultaneousAdjustments_ValueChanged(object sender, EventArgs e)
        {
            if (mf.autoTuner?.Config != null)
            {
                mf.autoTuner.Config.MaxSimultaneousAdjustments = (int)nudMaxSimultaneousAdjustments.Value;
                lblMaxSimultaneousAdjustments.Text = nudMaxSimultaneousAdjustments.Value.ToString();
            }
        }
        
        private void hsbarAccuracyVsSmoothness_ValueChanged(object sender, EventArgs e)
        {
            if (mf.autoTuner?.Config != null)
            {
                mf.autoTuner.Config.AccuracyVsSmoothness = hsbarAccuracyVsSmoothness.Value / 100.0;
                lblAccuracyVsSmoothness.Text = hsbarAccuracyVsSmoothness.Value.ToString() + "%";
            }
        }
    }
}
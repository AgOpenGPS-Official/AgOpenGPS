using System;
using System.Linq;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Controls;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        #region Heading
        private void tabDHeading_Enter(object sender, EventArgs e)
        {
            //heading
            if (Properties.Settings.Default.setGPS_headingFromWhichSource == "Fix") rbtnHeadingFix.Checked = true;
            //else if (Properties.Settings.Default.setGPS_headingFromWhichSource == "VTG") rbtnHeadingGPS.Checked = true;
            else if (Properties.Settings.Default.setGPS_headingFromWhichSource == "Dual") rbtnHeadingHDT.Checked = true;

            if (rbtnHeadingHDT.Checked)
            {
                if (Properties.Settings.Default.setAutoSwitchDualFixOn)
                {
                    rbtnHeadingFix.Enabled = false;
                    labelGboxSingle.Enabled = true;
                    labelGboxDual.Enabled = true;
                }
                else
                {
                    labelGboxSingle.Enabled = false;
                    labelGboxDual.Enabled = true;
                }
            }
            else
            {
                labelGboxSingle.Enabled = true;
                labelGboxDual.Enabled = false;
            }

            nudDualHeadingOffset.Value = (decimal)Properties.Settings.Default.setGPS_dualHeadingOffset;
            nudDualReverseDistance.Value = (decimal)Properties.Settings.Default.setGPS_dualReverseDetectionDistance;

            hsbarFusion.Value = (int)(Properties.Settings.Default.setIMU_fusionWeight2 * 500);
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString();

            cboxIsRTK.Checked = Properties.Settings.Default.setGPS_isRTK;
            cboxIsRTK_KillAutoSteer.Checked = Properties.Settings.Default.setGPS_isRTK_KillAutoSteer;

            nudFixJumpDistance.Value = Properties.Settings.Default.setGPS_jumpFixAlarmDistance;

            cboxIsReverseOn.Checked = Properties.Settings.Default.setIMU_isReverseOn;
            cboxIsAutoSwitchDualFixOn.Checked = Properties.Settings.Default.setAutoSwitchDualFixOn;
            nudAutoSwitchDualFixSpeed.Value = (decimal)Properties.Settings.Default.setAutoSwitchDualFixSpeed;

            if (Properties.Settings.Default.setF_minHeadingStepDistance == 1.0)
                cboxMinGPSStep.Checked = true;
            else
                cboxMinGPSStep.Checked = false;

            if (cboxMinGPSStep.Checked)
            {
                Properties.Settings.Default.setF_minHeadingStepDistance = 1.0;
                Properties.Settings.Default.setGPS_minimumStepLimit = 0.1;
                cboxMinGPSStep.Text = "10 cm";
                lblHeadingDistance.Text = "100 cm";
            }
            else
            {
                Properties.Settings.Default.setF_minHeadingStepDistance = 0.5;
                Properties.Settings.Default.setGPS_minimumStepLimit = 0.05;
                cboxMinGPSStep.Text = "5 cm";
                lblHeadingDistance.Text = "50 cm";
            }

            if (mf.ahrs.imuHeading != 99999)
            {
                hsbarFusion.Enabled = true;
            }
            else
            {
                hsbarFusion.Enabled = false;
            }

            if (cboxIsAutoSwitchDualFixOn.Checked)
            {
                hsbarFusion.Enabled = true;
            }

            //nudMinimumFrameTime.Value = Properties.Settings.Default.SetGPS_udpWatchMsec;

            //nudForwardComp.Value = (decimal)(Properties.Settings.Default.setGPS_forwardComp);
            //nudReverseComp.Value = (decimal)(Properties.Settings.Default.setGPS_reverseComp);
            //nudAgeAlarm.Value = Properties.Settings.Default.setGPS_ageAlarm;
        }

        private void tabDHeading_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.setIMU_fusionWeight2 = (double)hsbarFusion.Value * 0.002;
            mf.ahrs.fusionWeight = (double)hsbarFusion.Value * 0.002;

            Properties.Settings.Default.setGPS_isRTK = mf.isRTK_AlarmOn = cboxIsRTK.Checked;

            Properties.Settings.Default.setIMU_isReverseOn = mf.ahrs.isReverseOn = cboxIsReverseOn.Checked;
            Properties.Settings.Default.setAutoSwitchDualFixOn = mf.ahrs.autoSwitchDualFixOn = cboxIsAutoSwitchDualFixOn.Checked;

            Properties.Settings.Default.setGPS_isRTK_KillAutoSteer = mf.isRTK_KillAutosteer = cboxIsRTK_KillAutoSteer.Checked;

            if (cboxMinGPSStep.Checked)
            {
                Properties.Settings.Default.setF_minHeadingStepDistance = 1.0;
                Properties.Settings.Default.setGPS_minimumStepLimit = 0.1;
            }
            else
            {
                Properties.Settings.Default.setF_minHeadingStepDistance = 0.5;
                Properties.Settings.Default.setGPS_minimumStepLimit = 0.05;
            }

            Properties.Settings.Default.Save();
        }
        private void rbtnHeadingFix_CheckedChanged(object sender, EventArgs e)
        {
            var checkedButton = headingGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            Properties.Settings.Default.setGPS_headingFromWhichSource = checkedButton.Text;
            mf.headingFromSource = checkedButton.Text;

            if (rbtnHeadingHDT.Checked)
            {
                SetAutoSwitchDualFixPanelOptions();
            }
            else
            {
                rbtnHeadingFix.Enabled = true;
                labelGboxSingle.Enabled = true;
                labelGboxDual.Enabled = false;
            }
        }

        private void nudFixJumpDistance_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setGPS_jumpFixAlarmDistance = ((int)nudFixJumpDistance.Value);
                //mf.jumpDistanceAlarm = Properties.Settings.Default.setGPS_dualHeadingOffset;
            }
        }

        private void nudDualHeadingOffset_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setGPS_dualHeadingOffset = ((double)nudDualHeadingOffset.Value);
                mf.pn.headingTrueDualOffset = Properties.Settings.Default.setGPS_dualHeadingOffset;
            }
        }

        private void nudDualReverseDistance_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setGPS_dualReverseDetectionDistance = ((double)nudDualReverseDistance.Value);
                mf.dualReverseDetectionDistance = Properties.Settings.Default.setGPS_dualReverseDetectionDistance;
            }
        }
        //private void nudMinimumFrameTime_Click(object sender, EventArgs e)
        //{
        //    if (((NudlessNumericUpDown)sender).ShowKeypad(this))
        //    {
        //        Properties.Settings.Default.SetGPS_udpWatchMsec = ((int)nudMinimumFrameTime.Value);
        //        mf.udpWatchLimit = Properties.Settings.Default.SetGPS_udpWatchMsec;
        //    }
        //}
        private void cboxMinGPSStep_Click(object sender, EventArgs e)
        {
            if (cboxMinGPSStep.Checked)
            {
                Properties.Settings.Default.setF_minHeadingStepDistance = 1;
                Properties.Settings.Default.setGPS_minimumStepLimit = 0.1;
                cboxMinGPSStep.Text = "10 cm";
                lblHeadingDistance.Text = "100 cm";
                mf.isFirstHeadingSet = false;
            }
            else
            {
                Properties.Settings.Default.setF_minHeadingStepDistance = 0.5;
                Properties.Settings.Default.setGPS_minimumStepLimit = 0.05;
                cboxMinGPSStep.Text = "5 cm";
                lblHeadingDistance.Text = "50 cm";
                mf.isFirstHeadingSet = false;
            }

        }

        private void hsbarFusion_ValueChanged(object sender, EventArgs e)
        {
            lblFusion.Text = (hsbarFusion.Value).ToString() + "%";
            lblFusionIMU.Text = (100 - hsbarFusion.Value).ToString() + "%";

            mf.ahrs.fusionWeight = (double)hsbarFusion.Value * 0.002;
        }

        private void cboxIsAutoSwitchDualFixOn_CheckedChanged(object sender, EventArgs e)
        {
            SetAutoSwitchDualFixPanelOptions();
        }

        private void nudAutoSwitchDualFixSpeed_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setAutoSwitchDualFixSpeed = ((double)nudAutoSwitchDualFixSpeed.Value);
                mf.ahrs.autoSwitchDualFixSpeed = Properties.Settings.Default.setAutoSwitchDualFixSpeed;
            }
        }

        private void SetAutoSwitchDualFixPanelOptions()
        {
            if (cboxIsAutoSwitchDualFixOn.Checked)
            {
                rbtnHeadingFix.Enabled = false;
                labelGboxSingle.Enabled = true;
                labelGboxDual.Enabled = true;
            }
            else
            {
                rbtnHeadingFix.Enabled = true;
                labelGboxSingle.Enabled = false;
                labelGboxDual.Enabled = true;
            }
        }

        //private void nudForwardComp_Click(object sender, EventArgs e)
        //{
        //    if (((NudlessNumericUpDown)sender).ShowKeypad(this))
        //    {
        //        Properties.Settings.Default.setGPS_forwardComp = (double)nudForwardComp.Value;
        //    }
        //}

        //private void nudReverseComp_Click(object sender, EventArgs e)
        //{
        //    if (((NudlessNumericUpDown)sender).ShowKeypad(this))
        //    {
        //        Properties.Settings.Default.setGPS_reverseComp = (double)nudReverseComp.Value;
        //    }
        //}

        //private void nudAgeAlarm_Click(object sender, EventArgs e)
        //{
        //    if (((NudlessNumericUpDown)sender).ShowKeypad(this))
        //    {
        //        Properties.Settings.Default.setGPS_ageAlarm = (int)nudAgeAlarm.Value;
        //    }
        //}

        #endregion

        #region Roll

        private void tabDRoll_Enter(object sender, EventArgs e)
        {
            //Roll
            lblRollZeroOffset.Text = ((double)Properties.Settings.Default.setIMU_rollZero).ToString("N2");
            hsbarRollFilter.Value = (int)(Properties.Settings.Default.setIMU_rollFilter * 100);
            cboxDataInvertRoll.Checked = Properties.Settings.Default.setIMU_invertRoll;
        }

        private void tabDRoll_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.setIMU_rollFilter = (double)hsbarRollFilter.Value * 0.01;
            Properties.Settings.Default.setIMU_rollZero = mf.ahrs.rollZero;
            Properties.Settings.Default.setIMU_invertRoll = cboxDataInvertRoll.Checked;

            mf.ahrs.rollFilter = Properties.Settings.Default.setIMU_rollFilter;
            mf.ahrs.isRollInvert = Properties.Settings.Default.setIMU_invertRoll;

            Properties.Settings.Default.Save();
        }

        private void hsbarRollFilter_ValueChanged(object sender, EventArgs e)
        {
            lblRollFilterPercent.Text = hsbarRollFilter.Value.ToString();
        }

        private void btnRollOffsetDown_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.rollZero -= 0.1;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnRollOffsetUp_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.rollZero += 0.1;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }
        private void btnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.imuRoll += mf.ahrs.rollZero;
                mf.ahrs.rollZero = mf.ahrs.imuRoll;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
                Log.EventWriter("Roll Zeroed with " + mf.ahrs.rollZero.ToString());
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void btnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            mf.ahrs.rollZero = 0;
            lblRollZeroOffset.Text = "0.00";
            Log.EventWriter("Roll Zero Offset Removed");
        }

        private void btnResetIMU_Click(object sender, EventArgs e)
        {
            mf.ahrs.imuHeading = 99999;
            mf.ahrs.imuRoll = 88888;
        }

        #endregion

        #region Features On Off

        private void tabBtns_Enter(object sender, EventArgs e)
        {
            cboxFeatureTram.Checked = Properties.Settings.Default.setFeatures.isTramOn;
            cboxFeatureHeadland.Checked = Properties.Settings.Default.setFeatures.isHeadlandOn;
            cboxFeatureBoundary.Checked = Properties.Settings.Default.setFeatures.isBoundaryOn;

            //the nudge controls at bottom menu
            cboxFeatureNudge.Checked = Properties.Settings.Default.setFeatures.isABLineOn;
            //cboxFeatureBoundaryContour.Checked = Properties.Settings.Default.setFeatures.isBndContourOn;
            cboxFeatureRecPath.Checked = Properties.Settings.Default.setFeatures.isRecPathOn;
            cboxFeatureABSmooth.Checked = Properties.Settings.Default.setFeatures.isABSmoothOn;
            cboxFeatureHideContour.Checked = Properties.Settings.Default.setFeatures.isHideContourOn;
            cboxFeatureWebcam.Checked = Properties.Settings.Default.setFeatures.isWebCamOn;
            cboxFeatureOffsetFix.Checked = Properties.Settings.Default.setFeatures.isOffsetFixOn;

            cboxFeatureUTurn.Checked = Properties.Settings.Default.setFeatures.isUTurnOn;
            cboxFeatureLateral.Checked = Properties.Settings.Default.setFeatures.isLateralOn;

            cboxTurnSound.Checked = Properties.Settings.Default.setSound_isUturnOn;
            cboxSteerSound.Checked = Properties.Settings.Default.setSound_isAutoSteerOn;
            cboxHydLiftSound.Checked = Properties.Settings.Default.setSound_isHydLiftOn;
            cboxSectionsSound.Checked = Properties.Settings.Default.setSound_isSectionsOn;

            cboxAutoStartAgIO.Checked = Properties.Settings.Default.setDisplay_isAutoStartAgIO;
            cboxAutoOffAgIO.Checked = Properties.Settings.Default.setDisplay_isAutoOffAgIO;
            cboxShutdownWhenNoPower.Checked = Properties.Settings.Default.setDisplay_isShutdownWhenNoPower;
            cboxHardwareMessages.Checked = Properties.Settings.Default.setDisplay_isHardwareMessages;
        }

        private void tabBtns_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.setFeatures.isTramOn = cboxFeatureTram.Checked;
            Properties.Settings.Default.setFeatures.isHeadlandOn = cboxFeatureHeadland.Checked;

            Properties.Settings.Default.setFeatures.isABLineOn = cboxFeatureNudge.Checked;

            Properties.Settings.Default.setFeatures.isBoundaryOn = cboxFeatureBoundary.Checked;
            //Properties.Settings.Default.setFeatures.isBndContourOn = cboxFeatureBoundaryContour.Checked;
            Properties.Settings.Default.setFeatures.isRecPathOn = cboxFeatureRecPath.Checked;
            Properties.Settings.Default.setFeatures.isABSmoothOn = cboxFeatureABSmooth.Checked;
            Properties.Settings.Default.setFeatures.isHideContourOn = cboxFeatureHideContour.Checked;
            Properties.Settings.Default.setFeatures.isWebCamOn = cboxFeatureWebcam.Checked;
            Properties.Settings.Default.setFeatures.isOffsetFixOn = cboxFeatureOffsetFix.Checked;

            Properties.Settings.Default.setFeatures.isLateralOn = cboxFeatureLateral.Checked;
            Properties.Settings.Default.setFeatures.isUTurnOn = cboxFeatureUTurn.Checked;

            Properties.Settings.Default.setSound_isUturnOn = cboxTurnSound.Checked;
            mf.sounds.isTurnSoundOn = cboxTurnSound.Checked;
            Properties.Settings.Default.setSound_isAutoSteerOn = cboxSteerSound.Checked;
            mf.sounds.isSteerSoundOn = cboxSteerSound.Checked;
            Properties.Settings.Default.setSound_isSectionsOn = cboxSectionsSound.Checked;
            mf.sounds.isSectionsSoundOn = cboxSectionsSound.Checked;
            Properties.Settings.Default.setSound_isHydLiftOn = cboxHydLiftSound.Checked;
            mf.sounds.isHydLiftSoundOn = cboxHydLiftSound.Checked;

            Properties.Settings.Default.setDisplay_isAutoStartAgIO = cboxAutoStartAgIO.Checked;
            mf.isAutoStartAgIO = cboxAutoStartAgIO.Checked;

            Properties.Settings.Default.setDisplay_isAutoOffAgIO = cboxAutoOffAgIO.Checked;

            Properties.Settings.Default.setDisplay_isShutdownWhenNoPower = cboxShutdownWhenNoPower.Checked;

            Properties.Settings.Default.setDisplay_isHardwareMessages = cboxHardwareMessages.Checked;

            Properties.Settings.Default.Save();
        }

        private void btnRightMenuOrder_Click(object sender, EventArgs e)
        {
            using (var form = new FormButtonsRightPanel(mf))
            {
                form.ShowDialog(mf);
            }
        }

        #endregion
    }
}

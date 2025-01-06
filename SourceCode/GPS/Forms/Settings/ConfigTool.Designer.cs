using AgOpenGPS.Culture;
using AgOpenGPS.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        private decimal[] sectionWidthArr = new decimal[16];
        private decimal[] sectionPositionArr = new decimal[17];

        private double defaultSectionWidth;

        private int numberOfSections;

        #region Config
        private void tabTConfig_Enter(object sender, EventArgs e)
        {
            lblInchCm2.Text = mf.unitsInCm.ToString();

            if (mf.vehicle.vehicleType != 1)
            {
                pboxConfigHarvester.Visible = false;

                rbtnTBT.Visible = true;
                rbtnTrailing.Visible = true;
                rbtnFixedRear.Visible = true;
                rbtnFront.Visible = true;

                if (Properties.Settings.Default.setTool_isToolFront)
                {
                    rbtnTBT.Checked = false;
                    rbtnTrailing.Checked = false;
                    rbtnFixedRear.Checked = false;
                    rbtnFront.Checked = true;
                }
                else if (Properties.Settings.Default.setTool_isToolTBT)
                {
                    rbtnTBT.Checked = true;
                    rbtnTrailing.Checked = false;
                    rbtnFixedRear.Checked = false;
                    rbtnFront.Checked = false;
                }
                else if (Properties.Settings.Default.setTool_isToolTrailing)
                {
                    rbtnTBT.Checked = false;
                    rbtnTrailing.Checked = true;
                    rbtnFixedRear.Checked = false;
                    rbtnFront.Checked = false;
                }
                else if (Properties.Settings.Default.setTool_isToolRearFixed)
                {
                    rbtnTBT.Checked = false;
                    rbtnTrailing.Checked = false;
                    rbtnFixedRear.Checked = true;
                    rbtnFront.Checked = false;
                }
            }
            else
            {
                pboxConfigHarvester.Visible = true;

                rbtnTBT.Visible = false;
                rbtnTrailing.Visible = false;
                rbtnFixedRear.Visible = false;
                rbtnFront.Visible = false;

            }
        }
        private void tabTConfig_Leave(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType != 1)
            {
                if (rbtnFront.Checked)
                {
                    Properties.Settings.Default.setTool_isToolFront = true;
                    Properties.Settings.Default.setTool_isToolTBT = false;
                    Properties.Settings.Default.setTool_isToolTrailing = false;
                    Properties.Settings.Default.setTool_isToolRearFixed = false;
                }
                else if (rbtnTBT.Checked)
                {
                    Properties.Settings.Default.setTool_isToolFront = false;
                    Properties.Settings.Default.setTool_isToolTBT = true;
                    Properties.Settings.Default.setTool_isToolTrailing = true;
                    Properties.Settings.Default.setTool_isToolRearFixed = false;
                }
                else if (rbtnTrailing.Checked)
                {
                    Properties.Settings.Default.setTool_isToolFront = false;
                    Properties.Settings.Default.setTool_isToolTBT = false;
                    Properties.Settings.Default.setTool_isToolTrailing = true;
                    Properties.Settings.Default.setTool_isToolRearFixed = false;
                }
                else if (rbtnFixedRear.Checked)
                {
                    Properties.Settings.Default.setTool_isToolFront = false;
                    Properties.Settings.Default.setTool_isToolTBT = false;
                    Properties.Settings.Default.setTool_isToolTrailing = false;
                    Properties.Settings.Default.setTool_isToolRearFixed = true;
                }
            }
            else
            {
                Properties.Settings.Default.setTool_isToolFront = true;
                Properties.Settings.Default.setTool_isToolTBT = false;
                Properties.Settings.Default.setTool_isToolTrailing = false;
                Properties.Settings.Default.setTool_isToolRearFixed = false;
            }

            mf.tool.isToolRearFixed = Properties.Settings.Default.setTool_isToolRearFixed;
            mf.tool.isToolTrailing = Properties.Settings.Default.setTool_isToolTrailing;
            mf.tool.isToolTBT = Properties.Settings.Default.setTool_isToolTBT;
            mf.tool.isToolFrontFixed = Properties.Settings.Default.setTool_isToolFront;

            //mf.tool.hitchLength = (double)nudDrawbarLength.Value * mf.inchOrCm2m;
            if (Properties.Settings.Default.setTool_isToolFront == mf.tool.hitchLength < 0)
            {
                mf.tool.hitchLength *= -1;
                Properties.Settings.Default.setVehicle_hitchLength = mf.tool.hitchLength;
            }
            Properties.Settings.Default.Save();
        }

        #endregion

        #region  Hitch

        private void tabTHitch_Enter(object sender, EventArgs e)
        {
            if (mf.vehicle.vehicleType != 1)
            {
                //fixed -hitch only on vehicle
                if (Properties.Settings.Default.setTool_isToolFront)
                {
                    nudTrailingHitchLength.Visible = false;
                    nudDrawbarLength.Visible = true;
                    nudTankHitch.Visible = false;

                    nudDrawbarLength.Left = 401;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFront;
                }
                else if (Properties.Settings.Default.setTool_isToolRearFixed)
                {
                    nudTrailingHitchLength.Visible = false;
                    nudDrawbarLength.Visible = true;
                    nudTankHitch.Visible = false;

                    nudDrawbarLength.Left = 259;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageRear;
                }

                //trailing
                else if (Properties.Settings.Default.setTool_isToolTBT)
                {
                    nudTrailingHitchLength.Visible = true;
                    nudDrawbarLength.Visible = false;
                    nudTankHitch.Visible = true;

                    nudTrailingHitchLength.Left = 326;
                    nudTankHitch.Left = 643;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTBT;
                }
                else if (Properties.Settings.Default.setTool_isToolTrailing)
                {
                    nudTrailingHitchLength.Visible = true;
                    nudDrawbarLength.Visible = false;
                    nudTankHitch.Visible = false;

                    nudTrailingHitchLength.Left = 438;

                    picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageTrailing;
                }

                label112.Text = mf.unitsInCm;
            }
            else
            {
                nudTrailingHitchLength.Visible = false;
                nudDrawbarLength.Visible = true;
                nudTankHitch.Visible = false;

                nudDrawbarLength.Left = 580;

                picboxToolHitch.BackgroundImage = Properties.Resources.ToolHitchPageFrontHarvester;
            }

            nudDrawbarLength.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_hitchLength) * mf.m2InchOrCm);

            nudTrailingHitchLength.Value = (int)(Math.Abs(Properties.Settings.Default.setTool_toolTrailingHitchLength) * mf.m2InchOrCm);
            nudTankHitch.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_tankTrailingHitchLength) * mf.m2InchOrCm);
        }

        private void tabTHitch_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void nudDrawbarLength_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.hitchLength = (double)nudDrawbarLength.Value * mf.inchOrCm2m;
                if (!Properties.Settings.Default.setTool_isToolFront)
                {
                    mf.tool.hitchLength *= -1;
                }
                Properties.Settings.Default.setVehicle_hitchLength = mf.tool.hitchLength;
            }
        }

        private void nudTankHitch_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.tankTrailingHitchLength = (double)nudTankHitch.Value * -mf.inchOrCm2m;
                Properties.Settings.Default.setVehicle_tankTrailingHitchLength = mf.tool.tankTrailingHitchLength;

            }
        }

        private void nudTrailingHitchLength_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.trailingHitchLength = (double)nudTrailingHitchLength.Value * -mf.inchOrCm2m;
                Properties.Settings.Default.setTool_toolTrailingHitchLength = mf.tool.trailingHitchLength;
            }
        }

        #endregion

        #region Settings

        private void tabTSettings_Enter(object sender, EventArgs e)
        {
            nudLookAhead.Value = (decimal)Properties.Settings.Default.setVehicle_toolLookAheadOn;
            nudLookAheadOff.Value = (decimal)Properties.Settings.Default.setVehicle_toolLookAheadOff;
            nudTurnOffDelay.Value = (decimal)Properties.Settings.Default.setVehicle_toolOffDelay;
            //pictureBox3.Image = Resources.ToolLookaheadOn;
            //pictureBox4.Image = Resources.ToolLookaheadOff;
        }

        private void tabTSettings_Leave(object sender, EventArgs e)
        {

            Properties.Settings.Default.setVehicle_toolLookAheadOn = mf.tool.lookAheadOnSetting;
            Properties.Settings.Default.setVehicle_toolLookAheadOff = mf.tool.lookAheadOffSetting;
            Properties.Settings.Default.setVehicle_toolOffDelay = mf.tool.turnOffDelay;

            ////line up manual buttons based on # of sections
            //mf.LineUpManualBtns();

            ////update the sections to newly configured widths and positions in main
            //mf.SectionSetPosition();

            ////update the widths of sections and tool width in main
            //mf.SectionCalcWidths();

            Properties.Settings.Default.Save();
        }
        private void nudLookAhead_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (nudLookAheadOff.Value > (nudLookAhead.Value * 0.8m))
                {
                    nudLookAheadOff.Value = nudLookAhead.Value * 0.8m;
                }

                mf.tool.lookAheadOnSetting = (double)nudLookAhead.Value;
                mf.tool.lookAheadOffSetting = (double)nudLookAheadOff.Value;
                mf.tool.turnOffDelay = 0;
            }
        }

        private void nudLookAheadOff_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (nudLookAheadOff.Value > (nudLookAhead.Value * 0.8m))
                {
                    nudLookAheadOff.Value = nudLookAhead.Value * 0.8m;
                }
                mf.tool.lookAheadOffSetting = (double)nudLookAheadOff.Value;

                if (nudLookAheadOff.Value > 0)
                {
                    mf.tool.turnOffDelay = 0;
                    nudTurnOffDelay.Value = 0;
                }

                mf.tool.lookAheadOnSetting = (double)nudLookAhead.Value;
                mf.tool.lookAheadOffSetting = (double)nudLookAheadOff.Value;
                mf.tool.turnOffDelay = 0;


            }
        }

        private void nudTurnOffDelay_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (nudTurnOffDelay.Value > 0)
                {
                    nudLookAheadOff.Value = 0;
                }
                mf.tool.turnOffDelay = (double)nudTurnOffDelay.Value;
                mf.tool.lookAheadOffSetting = (double)nudLookAheadOff.Value;
                mf.tool.lookAheadOnSetting = (double)nudLookAhead.Value;
            }
        }

        #endregion

        #region offset
        private void tabToolOffset_Enter(object sender, EventArgs e)
        {
            nudOffset.Value = (decimal)(Math.Abs(Properties.Settings.Default.setVehicle_toolOffset) * mf.m2InchOrCm);

            rbtnToolRightPositive.Checked = false;
            rbtnLeftNegative.Checked = false;
            rbtnToolRightPositive.Checked = Properties.Settings.Default.setVehicle_toolOffset > 0;
            rbtnLeftNegative.Checked = Properties.Settings.Default.setVehicle_toolOffset < 0;

            nudOverlap.Value = (decimal)(Math.Abs(Properties.Settings.Default.setVehicle_toolOverlap) * mf.m2InchOrCm);

            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;
            rbtnToolOverlap.Checked = Properties.Settings.Default.setVehicle_toolOverlap > 0;
            rbtnToolGap.Checked = Properties.Settings.Default.setVehicle_toolOverlap < 0;

            label175.Text = mf.unitsInCm;
            label176.Text = mf.unitsInCm;
        }

        private void tabToolOffset_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void nudOffset_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (!rbtnToolRightPositive.Checked && !rbtnLeftNegative.Checked)
                    rbtnToolRightPositive.Checked = true;

                if (rbtnToolRightPositive.Checked)
                    mf.tool.offset = (double)nudOffset.Value * mf.inchOrCm2m;
                else
                    mf.tool.offset = (double)nudOffset.Value * -mf.inchOrCm2m;

                Properties.Settings.Default.setVehicle_toolOffset = mf.tool.offset;
            }

            rbtnToolRightPositive.Checked = false;
            rbtnLeftNegative.Checked = false;
            rbtnToolRightPositive.Checked = Properties.Settings.Default.setVehicle_toolOffset > 0;
            rbtnLeftNegative.Checked = Properties.Settings.Default.setVehicle_toolOffset < 0;
        }

        private void btnZeroToolOffset_Click(object sender, EventArgs e)
        {
            nudOffset.Value = 0;
            rbtnToolRightPositive.Checked = false;
            rbtnLeftNegative.Checked = false;

            mf.tool.offset = 0;
            Properties.Settings.Default.setVehicle_toolOffset = mf.tool.offset;
        }

        private void rbtnToolRightPositive_Click(object sender, EventArgs e)
        {
            if (rbtnToolRightPositive.Checked)
                mf.tool.offset = (double)nudOffset.Value * mf.inchOrCm2m;
            else
                mf.tool.offset = (double)nudOffset.Value * -mf.inchOrCm2m;
            Properties.Settings.Default.setVehicle_toolOffset = mf.tool.offset;

            rbtnToolRightPositive.Checked = false;
            rbtnLeftNegative.Checked = false;
            rbtnToolRightPositive.Checked = Properties.Settings.Default.setVehicle_toolOffset > 0;
            rbtnLeftNegative.Checked = Properties.Settings.Default.setVehicle_toolOffset < 0;
        }

        private void nudOverlap_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (!rbtnToolOverlap.Checked && !rbtnToolGap.Checked)
                    rbtnToolOverlap.Checked = true;

                if (rbtnToolOverlap.Checked)
                    mf.tool.overlap = (double)nudOverlap.Value * mf.inchOrCm2m;
                else
                    mf.tool.overlap = (double)nudOverlap.Value * -mf.inchOrCm2m;

                Properties.Settings.Default.setVehicle_toolOverlap = mf.tool.overlap;
            }

            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;
            rbtnToolOverlap.Checked = Properties.Settings.Default.setVehicle_toolOverlap > 0;
            rbtnToolGap.Checked = Properties.Settings.Default.setVehicle_toolOverlap < 0;
        }

        private void btnZeroOverlap_Click(object sender, EventArgs e)
        {
            nudOverlap.Value = 0;
            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;

            mf.tool.overlap = 0;
            Properties.Settings.Default.setVehicle_toolOverlap = mf.tool.overlap;
        }

        private void rbtnToolOverlap_Click(object sender, EventArgs e)
        {
            if (rbtnToolOverlap.Checked)
                mf.tool.overlap = (double)nudOverlap.Value * mf.inchOrCm2m;
            else
                mf.tool.overlap = (double)nudOverlap.Value * -mf.inchOrCm2m;
            Properties.Settings.Default.setVehicle_toolOverlap = mf.tool.overlap;
            Properties.Settings.Default.Save();

            rbtnToolOverlap.Checked = false;
            rbtnToolGap.Checked = false;
            rbtnToolOverlap.Checked = Properties.Settings.Default.setVehicle_toolOverlap > 0;
            rbtnToolGap.Checked = Properties.Settings.Default.setVehicle_toolOverlap < 0;
        }

        #endregion

        #region PivotDistance

        private void tabToolPivot_Enter(object sender, EventArgs e)
        {
            nudTrailingToolToPivotLength.Value = (decimal)(Math.Abs(Properties.Settings.Default.setTool_trailingToolToPivotLength) * mf.m2InchOrCm);

            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;
            rbtnPivotBehindPos.Checked = Properties.Settings.Default.setTool_trailingToolToPivotLength > 0;
            rbtnPivotAheadNeg.Checked = Properties.Settings.Default.setTool_trailingToolToPivotLength < 0;

            label177.Text = mf.unitsInCm;
        }

        private void tabToolPivot_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }


        private void btnPivotOffsetZero_Click(object sender, EventArgs e)
        {
            nudTrailingToolToPivotLength.Value = 0;
            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;

            mf.tool.trailingToolToPivotLength = 0;
            Properties.Settings.Default.setTool_trailingToolToPivotLength = mf.tool.trailingToolToPivotLength;
        }


        private void rbtnPivotBehindPos_Click(object sender, EventArgs e)
        {
            if (rbtnPivotBehindPos.Checked)
                mf.tool.trailingToolToPivotLength = (double)nudTrailingToolToPivotLength.Value * mf.inchOrCm2m;
            else
                mf.tool.trailingToolToPivotLength = (double)nudTrailingToolToPivotLength.Value * -mf.inchOrCm2m;
            Properties.Settings.Default.setTool_trailingToolToPivotLength = mf.tool.trailingToolToPivotLength;

            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;
            rbtnPivotBehindPos.Checked = Properties.Settings.Default.setTool_trailingToolToPivotLength > 0;
            rbtnPivotAheadNeg.Checked = Properties.Settings.Default.setTool_trailingToolToPivotLength < 0;
        }

        private void nudTrailingToolToPivotLength_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (rbtnPivotBehindPos.Checked)
                    mf.tool.trailingToolToPivotLength = (double)nudTrailingToolToPivotLength.Value * mf.inchOrCm2m;
                else
                    mf.tool.trailingToolToPivotLength = (double)nudTrailingToolToPivotLength.Value * -mf.inchOrCm2m;

                Properties.Settings.Default.setTool_trailingToolToPivotLength = mf.tool.trailingToolToPivotLength;
            }

            rbtnPivotBehindPos.Checked = false;
            rbtnPivotAheadNeg.Checked = false;
            rbtnPivotBehindPos.Checked = Properties.Settings.Default.setTool_trailingToolToPivotLength > 0;
            rbtnPivotAheadNeg.Checked = Properties.Settings.Default.setTool_trailingToolToPivotLength < 0;
        }


        #endregion

        #region Sections 

        private void tabTSections_Enter(object sender, EventArgs e)
        {
            if (mf.isJobStarted)
            {
                if (mf.autoBtnState == btnStates.Auto)
                    mf.btnSectionMasterAuto.PerformClick();

                if (mf.manualBtnState == btnStates.On)
                    mf.btnSectionMasterManual.PerformClick();
            }

            if (mf.tool.isSectionsNotZones)
            {
                //fix ManualOffOnAuto buttons
                mf.manualBtnState = btnStates.Off;
                mf.btnSectionMasterManual.Image = Properties.Resources.ManualOff;

                //fix auto button
                mf.autoBtnState = btnStates.Off;
                mf.btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOff;

                //Update the button colors and text
                mf.AllSectionsAndButtonsToState(mf.autoBtnState);

                //enable disable manual buttons
                mf.LineUpIndividualSectionBtns();

                nudDefaultSectionWidth.DecimalPlaces = 0;
            }
            else
            {
                //turn section buttons all OFF
                mf.AllZonesAndButtonsToState(btnStates.Off);

                mf.LineUpAllZoneButtons();

                nudDefaultSectionWidth.DecimalPlaces = 1;
            }

            cboxIsUnique.Checked = !mf.tool.isSectionsNotZones;

            cboxSectionBoundaryControl.Checked = Properties.Settings.Default.setTool_isSectionOffWhenOut;
            if (cboxSectionBoundaryControl.Checked)
            {
                cboxSectionBoundaryControl.BackgroundImage = Properties.Resources.SectionOffBoundary;
            }
            else
            {
                cboxSectionBoundaryControl.BackgroundImage = Properties.Resources.SectionOnBoundary;
            }

            nudCutoffSpeed.Value = (decimal)Properties.Settings.Default.setVehicle_slowSpeedCutoff;

            if (cboxIsUnique.Checked)
            {
                cboxIsUnique.BackgroundImage = Properties.Resources.ConT_Symmetric;
                cboxNumberOfZones.Visible = lblZonesBox.Visible = true;
            }
            else
            {
                cboxIsUnique.BackgroundImage = Properties.Resources.ConT_Asymmetric;
                cboxNumberOfZones.Visible = lblZonesBox.Visible = false;
            }

            nudNumberOfSections.Maximum = FormGPS.MAXSECTIONS;
            
            //fix ManualOffOnAuto buttons
            mf.manualBtnState = btnStates.Off;
            mf.btnSectionMasterManual.Image = Properties.Resources.ManualOff;

            //fix auto button
            mf.autoBtnState = btnStates.Off;
            mf.btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOff;

            nudMinCoverage.Value = Properties.Settings.Default.setVehicle_minCoverage;

            if (mf.tool.isSectionsNotZones)
            {
                //Update the button colors and text
                mf.AllSectionsAndButtonsToState(btnStates.Off);

                //enable disable manual buttons
                mf.LineUpIndividualSectionBtns();

                numberOfSections = Properties.Settings.Default.setVehicle_numSections;

                cboxNumSections.Text = numberOfSections.ToString();
                defaultSectionWidth = Properties.Settings.Default.setTool_defaultSectionWidth;
                nudDefaultSectionWidth.Value = (int)(defaultSectionWidth * mf.m2InchOrCm);

                panelSymmetricSections.Visible = false;

                nudNumberOfSections.Visible = false;
                cboxNumSections.Visible = true;

                nudSection01.Value = Math.Abs((Properties.Settings.Default.setSection_position2 - Properties.Settings.Default.setSection_position1) * (decimal)mf.m2InchOrCm);
                nudSection02.Value = Math.Abs((Properties.Settings.Default.setSection_position3 - Properties.Settings.Default.setSection_position2) * (decimal)mf.m2InchOrCm);
                nudSection03.Value = Math.Abs((Properties.Settings.Default.setSection_position4 - Properties.Settings.Default.setSection_position3) * (decimal)mf.m2InchOrCm);
                nudSection04.Value = Math.Abs((Properties.Settings.Default.setSection_position5 - Properties.Settings.Default.setSection_position4) * (decimal)mf.m2InchOrCm);
                nudSection05.Value = Math.Abs((Properties.Settings.Default.setSection_position6 - Properties.Settings.Default.setSection_position5) * (decimal)mf.m2InchOrCm);
                nudSection06.Value = Math.Abs((Properties.Settings.Default.setSection_position7 - Properties.Settings.Default.setSection_position6) * (decimal)mf.m2InchOrCm);
                nudSection07.Value = Math.Abs((Properties.Settings.Default.setSection_position8 - Properties.Settings.Default.setSection_position7) * (decimal)mf.m2InchOrCm);
                nudSection08.Value = Math.Abs((Properties.Settings.Default.setSection_position9 - Properties.Settings.Default.setSection_position8) * (decimal)mf.m2InchOrCm);
                nudSection09.Value = Math.Abs((Properties.Settings.Default.setSection_position10 - Properties.Settings.Default.setSection_position9) * (decimal)mf.m2InchOrCm);
                nudSection10.Value = Math.Abs((Properties.Settings.Default.setSection_position11 - Properties.Settings.Default.setSection_position10) * (decimal)mf.m2InchOrCm);
                nudSection11.Value = Math.Abs((Properties.Settings.Default.setSection_position12 - Properties.Settings.Default.setSection_position11) * (decimal)mf.m2InchOrCm);
                nudSection12.Value = Math.Abs((Properties.Settings.Default.setSection_position13 - Properties.Settings.Default.setSection_position12) * (decimal)mf.m2InchOrCm);
                nudSection13.Value = Math.Abs((Properties.Settings.Default.setSection_position14 - Properties.Settings.Default.setSection_position13) * (decimal)mf.m2InchOrCm);
                nudSection14.Value = Math.Abs((Properties.Settings.Default.setSection_position15 - Properties.Settings.Default.setSection_position14) * (decimal)mf.m2InchOrCm);
                nudSection15.Value = Math.Abs((Properties.Settings.Default.setSection_position16 - Properties.Settings.Default.setSection_position15) * (decimal)mf.m2InchOrCm);
                nudSection16.Value = Math.Abs((Properties.Settings.Default.setSection_position17 - Properties.Settings.Default.setSection_position16) * (decimal)mf.m2InchOrCm);

                //based on number of sections and values update the page before displaying
                UpdateSpinners();
            }
            else
            {
                //turn section buttons all OFF
                mf.AllZonesAndButtonsToState(btnStates.Off);

                cboxNumSections.Visible = false;

                panelSymmetricSections.Visible = true;
                nudNumberOfSections.Visible = true;

                numberOfSections = Properties.Settings.Default.setTool_numSectionsMulti;
                nudNumberOfSections.Value = numberOfSections;

                defaultSectionWidth = Properties.Settings.Default.setTool_sectionWidthMulti;
                nudDefaultSectionWidth.Value = (decimal)(Math.Round((defaultSectionWidth * mf.m2InchOrCm),1));

                SetNudZoneMinMax();

                nudZone1To.Value = mf.tool.zoneRanges[1];
                nudZone2To.Value = mf.tool.zoneRanges[2];
                nudZone3To.Value = mf.tool.zoneRanges[3];
                nudZone4To.Value = mf.tool.zoneRanges[4];
                nudZone5To.Value = mf.tool.zoneRanges[5];
                nudZone6To.Value = mf.tool.zoneRanges[6];
                nudZone7To.Value = mf.tool.zoneRanges[7];
                nudZone8To.Value = mf.tool.zoneRanges[8];

                cboxNumberOfZones.SelectedIndexChanged -= cboxNumberOfZones_SelectedIndexChanged;
                cboxNumberOfZones.Text = mf.tool.zones.ToString();
                cboxNumberOfZones.SelectedIndexChanged += cboxNumberOfZones_SelectedIndexChanged;

                words = Properties.Settings.Default.setTool_zones.Split(',');
                lblVehicleToolWidth.Text = Convert.ToString((int)(numberOfSections * defaultSectionWidth * 100 * mf.cm2CmOrIn));

                mf.LineUpAllZoneButtons();
                SetNudZoneVisibility();
            }

            label178.Text = mf.unitsInCm;
        }

        private void tabTSections_Leave(object sender, EventArgs e)
        {
            if (mf.tool.isSectionsNotZones)
            {
                //take the section widths and convert to meters and positions along tool.
                CalculateSectionPositions();

                Properties.Settings.Default.setTool_isSectionOffWhenOut = cboxSectionBoundaryControl.Checked;
                mf.tool.isSectionOffWhenOut = cboxSectionBoundaryControl.Checked;

                //save the values in each spinner for section position widths in settings
                Properties.Settings.Default.setSection_position1 = sectionPositionArr[0];
                Properties.Settings.Default.setSection_position2 = sectionPositionArr[1];
                Properties.Settings.Default.setSection_position3 = sectionPositionArr[2];
                Properties.Settings.Default.setSection_position4 = sectionPositionArr[3];
                Properties.Settings.Default.setSection_position5 = sectionPositionArr[4];
                Properties.Settings.Default.setSection_position6 = sectionPositionArr[5];
                Properties.Settings.Default.setSection_position7 = sectionPositionArr[6];
                Properties.Settings.Default.setSection_position8 = sectionPositionArr[7];
                Properties.Settings.Default.setSection_position9 = sectionPositionArr[8];
                Properties.Settings.Default.setSection_position10 = sectionPositionArr[9];
                Properties.Settings.Default.setSection_position11 = sectionPositionArr[10];
                Properties.Settings.Default.setSection_position12 = sectionPositionArr[11];
                Properties.Settings.Default.setSection_position13 = sectionPositionArr[12];
                Properties.Settings.Default.setSection_position14 = sectionPositionArr[13];
                Properties.Settings.Default.setSection_position15 = sectionPositionArr[14];
                Properties.Settings.Default.setSection_position16 = sectionPositionArr[15];
                Properties.Settings.Default.setSection_position17 = sectionPositionArr[16];
                                                                    
                mf.tool.numOfSections = numberOfSections;

                Properties.Settings.Default.setVehicle_numSections = mf.tool.numOfSections;

                //line up manual buttons based on # of sections
                mf.LineUpIndividualSectionBtns();

                //update the sections to newly configured widths and positions in main
                mf.SectionSetPosition();

                //update the widths of sections and tool width in main
                mf.SectionCalcWidths();

                mf.tram.IsTramOuterOrInner();

                Properties.Settings.Default.setVehicle_toolWidth = mf.tool.width;

                SendRelaySettingsToMachineModule();
            }
            else
            {
                mf.tool.numOfSections = numberOfSections;
                Properties.Settings.Default.setTool_numSectionsMulti = mf.tool.numOfSections;

                mf.tool.width = numberOfSections * defaultSectionWidth;
                Properties.Settings.Default.setVehicle_toolWidth = mf.tool.width;

                mf.tram.IsTramOuterOrInner();

                Properties.Settings.Default.Save();

                mf.SectionCalcMulti();

                for (int i = 0; i < 9; i++)
                {
                    mf.tool.zoneRanges[i] = 0;
                }

                mf.tool.zoneRanges[0] = mf.tool.zones;

                if (mf.tool.zones == 2)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                }
                else if (mf.tool.zones == 3)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                }
                else if (mf.tool.zones == 4)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                }
                else if (mf.tool.zones == 5)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                }
                else if (mf.tool.zones == 6)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                    mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                }
                else if (mf.tool.zones == 7)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                    mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                    mf.tool.zoneRanges[7] = (int)nudZone7To.Value;
                }
                else if (mf.tool.zones == 8)
                {
                    mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                    mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                    mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                    mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                    mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                    mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                    mf.tool.zoneRanges[7] = (int)nudZone7To.Value;
                    mf.tool.zoneRanges[8] = (int)nudZone8To.Value;
                }

                String str = "";
                str = String.Join(",",mf.tool.zoneRanges);
                Properties.Settings.Default.setTool_zones = str;

                mf.LineUpAllZoneButtons();
            }

            //no multi color zones
            if (mf.tool.isSectionsNotZones)
                Properties.Settings.Default.setColor_isMultiColorSections = mf.tool.isMultiColoredSections = false;

            Properties.Settings.Default.Save();
        }

        private void nudZone1To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[1] = (int)nudZone1To.Value;
                SetNudZoneVisibility(); 
            }
        }

        private void nudZone2To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[2] = (int)nudZone2To.Value;
                SetNudZoneVisibility();
            }
        }

        private void nudZone3To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[3] = (int)nudZone3To.Value;
                SetNudZoneVisibility();
            }
        }

        private void nudZone4To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[4] = (int)nudZone4To.Value;
                SetNudZoneVisibility();
            }
        }

        private void nudZone5To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[5] = (int)nudZone5To.Value;
                SetNudZoneVisibility();
            }
        }

        private void nudZone6To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[6] = (int)nudZone6To.Value;
                SetNudZoneVisibility();
            }
        }

        private void nudZone7To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[7] = (int)nudZone7To.Value;
                SetNudZoneVisibility();
            }
        }

        private void nudZone8To_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.zoneRanges[8] = (int)nudZone8To.Value;
                SetNudZoneVisibility();
            }
        }

        private void cboxNumberOfZones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((cboxNumberOfZones.SelectedIndex+2) > (int)nudNumberOfSections.Value)
            {
                mf.YesMessageBox("You can't have more zones then sections");
                cboxNumberOfZones.SelectedIndex = mf.tool.zones - 2;

                return;
            }

            mf.tool.zones = cboxNumberOfZones.SelectedIndex + 2;
            //if (mf.tool.zones == 1) mf.tool.zones = 0;


            SetNudZoneMinMax();
            FillZoneNudsWithDefaultValues();
            SetNudZoneVisibility();
        }

        private void FillZoneNudsWithDefaultValues()
        {
            nudZone1To.Value = 0;
            nudZone2To.Value = 0;
            nudZone3To.Value = 0;
            nudZone4To.Value = 0;
            nudZone5To.Value = 0;
            nudZone6To.Value = 0;
            nudZone7To.Value = 0;
            nudZone8To.Value = 0;

            if (mf.tool.zones != 0)
            {
                int defa = numberOfSections / mf.tool.zones;
                if (mf.tool.zones == 2)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 3)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value += 2 * defa;
                    nudZone3To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 4)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value += 2 * defa;
                    nudZone3To.Value += 3 * defa;
                    nudZone4To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 5)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value += 2 * defa;
                    nudZone3To.Value += 3 * defa;
                    nudZone4To.Value += 4 * defa;
                    nudZone5To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 6)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value += 2 * defa;
                    nudZone3To.Value += 3 * defa;
                    nudZone4To.Value += 4 * defa;
                    nudZone5To.Value += 5 * defa;
                    nudZone6To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 7)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value += 2 * defa;
                    nudZone3To.Value += 3 * defa;
                    nudZone4To.Value += 4 * defa;
                    nudZone5To.Value += 5 * defa;
                    nudZone6To.Value += 6 * defa;
                    nudZone7To.Value = numberOfSections;
                }
                else if (mf.tool.zones == 8)
                {
                    nudZone1To.Value += defa;
                    nudZone2To.Value += 2 * defa;
                    nudZone3To.Value += 3 * defa;
                    nudZone4To.Value += 4 * defa;
                    nudZone5To.Value += 5 * defa;
                    nudZone6To.Value += 6 * defa;
                    nudZone7To.Value += 7 * defa;
                    nudZone8To.Value = numberOfSections;
                }
            }
        }

        private void SetNudZoneMinMax()
        {
            nudZone1To.Maximum = numberOfSections;
            nudZone2To.Maximum = numberOfSections;
            nudZone3To.Maximum = numberOfSections;
            nudZone4To.Maximum = numberOfSections;
            nudZone5To.Maximum = numberOfSections;
            nudZone6To.Maximum = numberOfSections;
            nudZone7To.Maximum = numberOfSections;
            nudZone8To.Maximum = numberOfSections;
        }

        private void SetNudZoneVisibility()
        {
            nudZone1To.Visible = false;
            nudZone2To.Visible = false;
            nudZone3To.Visible = false;
            nudZone4To.Visible = false;
            nudZone5To.Visible = false;
            nudZone6To.Visible = false;
            nudZone7To.Visible = false;
            nudZone8To.Visible = false;

            nudZone1To.Enabled = true;
            nudZone2To.Enabled = true;
            nudZone3To.Enabled = true;
            nudZone4To.Enabled = true;
            nudZone5To.Enabled = true;
            nudZone6To.Enabled = true;
            nudZone7To.Enabled = true;
            nudZone8To.Enabled = true;

            lblZoneStart1.Visible = false;
            lblZoneStart2.Visible = false;
            lblZoneStart3.Visible = false;
            lblZoneStart4.Visible = false;
            lblZoneStart5.Visible = false;
            lblZoneStart6.Visible = false;
            lblZoneStart7.Visible = false;
            lblZoneStart8.Visible = false;

            if (mf.tool.zones > 1)
            {
                nudZone2To.Visible = true;
                nudZone1To.Visible = true;
                lblZoneStart1.Visible = true;
                lblZoneStart2.Visible = true;
                lblZoneStart2.Text = (nudZone1To.Value + 1).ToString();
                if (mf.tool.zones == 2) nudZone2To.Enabled = false;
            }

            if (mf.tool.zones > 2)
            {
                nudZone3To.Visible = true;
                lblZoneStart3.Visible = true;
                lblZoneStart3.Text = (nudZone2To.Value + 1).ToString();
                if (mf.tool.zones == 3) nudZone3To.Enabled = false;
            }

            if (mf.tool.zones > 3)
            {
                nudZone4To.Visible = true;
                lblZoneStart4.Visible = true;
                lblZoneStart4.Text = (nudZone3To.Value + 1).ToString();
                if (mf.tool.zones == 4) nudZone4To.Enabled = false;
            }

            if (mf.tool.zones > 4)
            {
                nudZone5To.Visible = true;
                lblZoneStart5.Visible = true;
                lblZoneStart5.Text = (nudZone4To.Value + 1).ToString();
                if (mf.tool.zones == 5) nudZone5To.Enabled = false;
            }

            if (mf.tool.zones > 5)
            {
                nudZone6To.Visible = true;
                lblZoneStart6.Visible = true;
                lblZoneStart6.Text = (nudZone5To.Value + 1).ToString();
                if (mf.tool.zones == 6) nudZone6To.Enabled = false;
            }

            if (mf.tool.zones > 6)
            {
                nudZone7To.Visible = true;
                lblZoneStart7.Visible = true;
                lblZoneStart7.Text = (nudZone6To.Value + 1).ToString();
                if (mf.tool.zones == 7) nudZone7To.Enabled = false;
            }

            if (mf.tool.zones > 7)
            {
                nudZone8To.Visible = true;
                lblZoneStart8.Visible = true;
                lblZoneStart8.Text = (nudZone7To.Value + 1).ToString();
                if (mf.tool.zones == 8) nudZone8To.Enabled = false;
            }

        }

        private void cboxSectionBoundaryControl_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setTool_isSectionOffWhenOut = !Properties.Settings.Default.setTool_isSectionOffWhenOut;
            Properties.Settings.Default.Save();

            cboxSectionBoundaryControl.Checked = Properties.Settings.Default.setTool_isSectionOffWhenOut;
            if (cboxSectionBoundaryControl.Checked)
            {
                cboxSectionBoundaryControl.BackgroundImage = Properties.Resources.SectionOffBoundary;
            }
            else
            {
                cboxSectionBoundaryControl.BackgroundImage = Properties.Resources.SectionOnBoundary;
            }
        }

        private void cboxIsUnique_Click(object sender, EventArgs e)
        {
            mf.tool.isSectionsNotZones = !cboxIsUnique.Checked;
            Properties.Settings.Default.setTool_isSectionsNotZones = !cboxIsUnique.Checked;
            tabTSections_Enter(this, e);
        }

        private void nudNumberOfSections_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if ((int)nudNumberOfSections.Value < mf.tool.zones)
                {
                    mf.YesMessageBox("You can't have more zones then sections");
                    nudNumberOfSections.Value = numberOfSections;
                    return;
                }    
                numberOfSections = (int)nudNumberOfSections.Value;
                SetNudZoneMinMax();

                Properties.Settings.Default.setTool_numSectionsMulti = numberOfSections;
                Properties.Settings.Default.Save();

                lblVehicleToolWidth.Text = Convert.ToString((int)(numberOfSections * defaultSectionWidth * 100 * mf.cm2CmOrIn));
                SectionFeetInchesTotalWidthLabelUpdate();
                FillZoneNudsWithDefaultValues();
                SetNudZoneVisibility(); 
            }
        }

        private void nudDefaultSectionWidth_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                defaultSectionWidth = (double)nudDefaultSectionWidth.Value * mf.inchOrCm2m;

                if (mf.tool.isSectionsNotZones)
                    Properties.Settings.Default.setTool_defaultSectionWidth = defaultSectionWidth;
                else
                    Properties.Settings.Default.setTool_sectionWidthMulti = defaultSectionWidth;

                Properties.Settings.Default.Save();

                //lblVehicleToolWidth.Text = Convert.ToString((int)(numberOfSections * defaultSectionWidth * 100 * mf.cm2CmOrIn));
                //SectionFeetInchesTotalWidthLabelUpdate();
            }
        }

        private void cboxNumSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (mf.tool.isSectionsNotZones)
            {
                numberOfSections = cboxNumSections.SelectedIndex + 1;

                decimal wide = nudDefaultSectionWidth.Value;

                if (mf.isMetric)
                {
                    if (numberOfSections * wide > 5000)
                    {
                        wide = 99;
                        mf.TimedMessageBox(3000, "Too Wide", "Max 50 Meters");
                        Log.EventWriter("Sections, Tool Set Too Wide");

                    }
                }
                else
                {
                    if (numberOfSections * wide > 1900)
                    {
                        wide = 19;
                        mf.TimedMessageBox(3000, "Too Wide", "Max 164 Feet");
                        Log.EventWriter("Sections, Tool Set Too Wide");
                    }
                }

                nudSection01.Value = wide;
                nudSection02.Value = wide;
                nudSection03.Value = wide;
                nudSection04.Value = wide;
                nudSection05.Value = wide;
                nudSection06.Value = wide;
                nudSection07.Value = wide;
                nudSection08.Value = wide;
                nudSection09.Value = wide;
                nudSection10.Value = wide;
                nudSection11.Value = wide;
                nudSection12.Value = wide;
                nudSection13.Value = wide;
                nudSection14.Value = wide;
                nudSection15.Value = wide;
                nudSection16.Value = wide;

                UpdateSpinners();

                //take the section widths and convert to meters and positions along tool.
                CalculateSectionPositions();
                //line up manual buttons based on # of sections
                mf.LineUpIndividualSectionBtns();

                //update the sections to newly configured widths and positions in main
                mf.SectionSetPosition();

                //update the widths of sections and tool width in main
                mf.SectionCalcWidths();
            }
        }

        private void NudSection1_Click(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NudlessNumericUpDown)sender, this);
            UpdateSpinners();
        }

        private void nudCutoffSpeed_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.vehicle.slowSpeedCutoff = (double)nudCutoffSpeed.Value;
                Properties.Settings.Default.setVehicle_slowSpeedCutoff = (double)nudCutoffSpeed.Value;
            }
        }

        private void nudMinCoverage_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                mf.tool.minCoverage = (int)nudMinCoverage.Value;
                Properties.Settings.Default.setVehicle_minCoverage = mf.tool.minCoverage;
            }
        }

        public void UpdateSpinners()
        {
            int i = (int)numberOfSections;

            decimal toolWidth = 0;

            foreach (var item in tab1.TabPages[9].Controls)
            {
                if (item is NudlessNumericUpDown)
                {
                    var item2 = (NudlessNumericUpDown)item;
                    if (item2.Name.Substring(0, 6) == "nudSec")
                    {
                        //grab the number from nudSection01
                        string bob = item2.Name.Substring(10, 2);
                        int nudNum = Convert.ToInt32(bob);
                        if (nudNum <= i)
                        {
                            item2.Enabled = true;
                            item2.Visible = true;
                            toolWidth += item2.Value;

                            if (mf.isMetric)
                            {
                                if (toolWidth > 5000)
                                {
                                    mf.TimedMessageBox(3000, "Too Wide", "Set to 99, Max 50 Meters");
                                    Log.EventWriter("Sections, Tool Set Too Wide");
                                    toolWidth = 0;
                                    nudSection01.Value =  99;
                                    nudSection02.Value =  99;
                                    nudSection03.Value =  99;
                                    nudSection04.Value =  99;
                                    nudSection05.Value =  99;
                                    nudSection06.Value =  99;
                                    nudSection07.Value =  99;
                                    nudSection08.Value =  99;
                                    nudSection09.Value =  99;
                                    nudSection10.Value =  99;
                                    nudSection11.Value =  99;
                                    nudSection12.Value =  99;
                                    nudSection13.Value =  99;
                                    nudSection14.Value =  99;
                                    nudSection15.Value =  99;
                                    nudSection16.Value =  99;
                                }
                            }
                            else
                            {
                                if (toolWidth > 1900)
                                {
                                    mf.TimedMessageBox(3000, "Too Wide", "Set to 99, Max 164 Feet");
                                    Log.EventWriter("Sections, Tool Set Too Wide");
                                    toolWidth = 0;
                                    nudSection01.Value = 99;
                                    nudSection02.Value = 99;
                                    nudSection03.Value = 99;
                                    nudSection04.Value = 99;
                                    nudSection05.Value = 99;
                                    nudSection06.Value = 99;
                                    nudSection07.Value = 99;
                                    nudSection08.Value = 99;
                                    nudSection09.Value = 99;
                                    nudSection10.Value = 99;
                                    nudSection11.Value = 99;
                                    nudSection12.Value = 99;
                                    nudSection13.Value = 99;
                                    nudSection14.Value = 99;
                                    nudSection15.Value = 99;
                                    nudSection16.Value = 99;
                                }
                            }

                        }
                        else
                        {
                            item2.Enabled = false;
                            item2.Visible = false;
                        }
                    }
                }                
            }

            lblVehicleToolWidth.Text = Convert.ToString((int)toolWidth);

            SectionFeetInchesTotalWidthLabelUpdate();
        }

        //update tool width label at bottom of window
        private void SectionFeetInchesTotalWidthLabelUpdate()
        {
            if (mf.isMetric)
            {
                lblInchesCm.Text = gStr.gsCentimeters;
                lblFeetMeters.Text = gStr.gsMeters;
                lblSecTotalWidthFeet.Visible = false;
                lblSecTotalWidthInches.Visible = false;
                lblSecTotalWidthMeters.Visible = true;
            }
            else
            {
                lblInchesCm.Text = gStr.gsInches;
                lblFeetMeters.Text = "Feet";
                lblSecTotalWidthFeet.Visible = true;
                lblSecTotalWidthInches.Visible = true;
                lblSecTotalWidthMeters.Visible = false;
            }

            if (mf.isMetric)
            {
                lblSecTotalWidthMeters.Text = ((int)(mf.tool.width * 100)).ToString() + " cm";
                lblSummaryWidth.Text = mf.tool.width.ToString("N2") + " m";
            }
            else
            {
                double toFeet = (Convert.ToDouble(lblVehicleToolWidth.Text) * 0.08334);
                lblSecTotalWidthFeet.Text = Convert.ToString((int)toFeet) + "'";
                double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
                lblSecTotalWidthInches.Text = Convert.ToString(temp) + '"';

                lblSummaryWidth.Text = lblSecTotalWidthFeet.Text + " " + lblSecTotalWidthInches.Text;
            }
        }

        //Convert section width to positions along toolbar
        private void CalculateSectionPositions()
        {
            int i = numberOfSections;

            //convert to meters spinner value
            sectionWidthArr[0] = nudSection01.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[1] = nudSection02.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[2] = nudSection03.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[3] = nudSection04.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[4] = nudSection05.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[5] = nudSection06.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[6] = nudSection07.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[7] = nudSection08.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[8] = nudSection09.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[9] = nudSection10.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[10] = nudSection11.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[11] = nudSection12.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[12] = nudSection13.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[13] = nudSection14.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[14] = nudSection15.Value * (decimal)mf.inchOrCm2m;
            sectionWidthArr[15] = nudSection16.Value * (decimal)mf.inchOrCm2m;

            //add up the set widths
            decimal setWidth = 0;
            for (int j = 0; j < i; j++)
            {
                setWidth += sectionWidthArr[j];
            }

            //leftmost position.
            setWidth *= -0.5M;

            sectionPositionArr[0] = setWidth;

            for (int j = 1; j < 17; j++)
            {
                if (j <= i) sectionPositionArr[j] = sectionPositionArr[j - 1] + sectionWidthArr[j-1];
                else sectionPositionArr[j] = 0;                
            }
        }

        #endregion

        #region Switch

        private void tabTSwitches_Enter(object sender, EventArgs e)
        {
            //set accordingly
            chkSelectSteerSwitch.Checked = mf.mc.isSteerWorkSwitchEnabled;
            chkSelectWorkSwitch.Checked = mf.mc.isWorkSwitchEnabled;

            if (Properties.Settings.Default.setF_isWorkSwitchManualSections)
            {
                chkSetManualSections.Checked = true;
                chkSetAutoSections.Checked = false;
            }
            else
            {
                chkSetManualSections.Checked = false;
                chkSetAutoSections.Checked = true;
            }

            if (Properties.Settings.Default.setF_isSteerWorkSwitchManualSections)
            {
                chkSetManualSectionsSteer.Checked = true;
                chkSetAutoSectionsSteer.Checked = false;
            }
            else
            {
                chkSetManualSectionsSteer.Checked = false;
                chkSetAutoSectionsSteer.Checked = true;
            }

            chkSetManualSections.Enabled = chkSetAutoSections.Enabled = chkWorkSwActiveLow.Enabled = chkSelectWorkSwitch.Checked;
            chkSetManualSectionsSteer.Enabled = chkSetAutoSectionsSteer.Enabled = chkSelectSteerSwitch.Checked;


            chkWorkSwActiveLow.Checked = Properties.Settings.Default.setF_isWorkSwitchActiveLow;
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;
        }

        private void tabTSwitches_Leave(object sender, EventArgs e)
        {
            //active low on work switch
            mf.mc.isWorkSwitchActiveLow = Properties.Settings.Default.setF_isWorkSwitchActiveLow = chkWorkSwActiveLow.Checked;

            //is work switch enabled
            mf.mc.isWorkSwitchEnabled = Properties.Settings.Default.setF_isWorkSwitchEnabled = chkSelectWorkSwitch.Checked;

            //Are auto or manual sections controlled. 
            mf.mc.isWorkSwitchManualSections = Properties.Settings.Default.setF_isWorkSwitchManualSections = chkSetManualSections.Checked;

            //Are auto or manual sections controlled for steer
            mf.mc.isSteerWorkSwitchEnabled = Properties.Settings.Default.setF_isSteerWorkSwitchEnabled = chkSelectSteerSwitch.Checked;

            //does steer switch control manual or auto sections
            mf.mc.isSteerWorkSwitchManualSections = Properties.Settings.Default.setF_isSteerWorkSwitchManualSections = chkSetManualSectionsSteer.Checked;

            if (!mf.mc.isSteerWorkSwitchEnabled && !mf.mc.isWorkSwitchEnabled)
                mf.mc.isRemoteWorkSystemOn = Properties.Settings.Default.setF_isRemoteWorkSystemOn = false;
            else
                mf.mc.isRemoteWorkSystemOn = Properties.Settings.Default.setF_isRemoteWorkSystemOn = true;
            //save
            Properties.Settings.Default.Save();
        }

        private void chkSelectWorkSwitch_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Enabled = chkSetAutoSections.Enabled= chkWorkSwActiveLow.Enabled = chkSelectWorkSwitch.Checked;
        }

        private void chkSelectSteerSwitch_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Enabled = chkSetAutoSectionsSteer.Enabled = chkSelectSteerSwitch.Checked;
        }

        private void chkWorkSwActiveLow_Click(object sender, EventArgs e)
        {
            if (chkWorkSwActiveLow.Checked) chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveClosed;
            else chkWorkSwActiveLow.Image = Properties.Resources.SwitchActiveOpen;
        }

        private void chkSetManualSections_Click(object sender, EventArgs e)
        {
            chkSetAutoSections.Checked = false;
            chkSetManualSections.Checked = true;
        }

        private void chkSetAutoSections_Click(object sender, EventArgs e)
        {
            chkSetManualSections.Checked = false;
            chkSetAutoSections.Checked = true;
        }

        private void chkSetAutoSectionsSteer_Click(object sender, EventArgs e)
        {
            chkSetManualSectionsSteer.Checked = false;
            chkSetAutoSectionsSteer.Checked = true;
        }

        private void chkSetManualSectionsSteer_Click(object sender, EventArgs e)
        {
            chkSetAutoSectionsSteer.Checked = false;
            chkSetManualSectionsSteer.Checked = true;
        }
        #endregion
    }
}

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgLibrary.Settings;
using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Properties;
using AgOpenGPS.ResourcesBrands;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        #region Vehicle Save---------------------------------------------

        private void SaveDisplaySettings()
        {
            mf.isTextureOn = chkDisplayFloor.Checked;
            mf.isGridOn = chkDisplayGrid.Checked;
            mf.isSpeedoOn = chkDisplaySpeedo.Checked;
            mf.isSideGuideLines = chkDisplayExtraGuides.Checked;

            mf.isDrawPolygons = chkDisplayPolygons.Checked;
            mf.isKeyboardOn = chkDisplayKeyboard.Checked;

            mf.isBrightnessOn = chkDisplayBrightness.Checked;
            mf.isSvennArrowOn = chkSvennArrow.Checked;
            mf.isLogElevation = chkDisplayLogElevation.Checked;

            mf.isDirectionMarkers = chkDirectionMarkers.Checked;
            mf.isSectionlinesOn = chkSectionLines.Checked;
            mf.isLineSmooth = chkLineSmooth.Checked;
            mf.isHeadlandDistanceOn = chkboxHeadlandDist.Checked;

            //mf.timeToShowMenus = (int)nudMenusOnTime.Value;

            Properties.Settings.Default.setDisplay_isBrightnessOn = mf.isBrightnessOn;
            Properties.Settings.Default.setDisplay_isTextureOn = mf.isTextureOn;
            Properties.Settings.Default.setMenu_isGridOn = mf.isGridOn;

            Properties.Settings.Default.setDisplay_isSvennArrowOn = mf.isSvennArrowOn;
            Properties.Settings.Default.setMenu_isSpeedoOn = mf.isSpeedoOn;
            Properties.Settings.Default.setDisplay_isStartFullScreen = chkDisplayStartFullScreen.Checked;
            Properties.Settings.Default.setMenu_isSideGuideLines = mf.isSideGuideLines;

            Properties.Settings.Default.setMenu_isPureOn = mf.isPureDisplayOn;
            Properties.Settings.Default.setMenu_isLightbarOn = mf.isLightbarOn;
            Properties.Settings.Default.setDisplay_isKeyboardOn = mf.isKeyboardOn;
            Properties.Settings.Default.isHeadlandDistanceOn = mf.isHeadlandDistanceOn;
            Properties.Settings.Default.setDisplay_isLogElevation = mf.isLogElevation;

            Properties.Settings.Default.setMenu_isMetric = rbtnDisplayMetric.Checked;
            mf.isMetric = rbtnDisplayMetric.Checked;

            Properties.ToolSettings.Default.setTool_isDirectionMarkers = mf.isDirectionMarkers;

            Properties.VehicleSettings.Default.setAS_numGuideLines = mf.ABLine.numGuideLines;
            Properties.Settings.Default.setDisplay_isSectionLinesOn = mf.isSectionlinesOn;
            Properties.Settings.Default.setDisplay_isLineSmooth = mf.isLineSmooth;
            Properties.Settings.Default.isHeadlandDistanceOn = mf.isHeadlandDistanceOn;

            Properties.VehicleSettings.Default.Save(RegistrySettings.vehicleFileName);
            Properties.ToolSettings.Default.Save(RegistrySettings.toolFileName);
            Properties.Settings.Default.Save();
        }

        #endregion

        #region Antenna Enter/Leave
        private void tabVAntenna_Enter(object sender, EventArgs e)
        {
            nudAntennaHeight.Value = (int)(Properties.VehicleSettings.Default.setVehicle_antennaHeight * mf.m2InchOrCm);

            nudAntennaPivot.Value = (int)((Properties.VehicleSettings.Default.setVehicle_antennaPivot) * mf.m2InchOrCm);

            //negative is to the right
            nudAntennaOffset.Value = (int)(Math.Abs(Properties.VehicleSettings.Default.setVehicle_antennaOffset) * mf.m2InchOrCm);

            rbtnAntennaLeft.Checked = false;
            rbtnAntennaRight.Checked = false;
            rbtnAntennaCenter.Checked = false;
            rbtnAntennaLeft.Checked = Properties.VehicleSettings.Default.setVehicle_antennaOffset > 0;
            rbtnAntennaRight.Checked = Properties.VehicleSettings.Default.setVehicle_antennaOffset < 0;
            rbtnAntennaCenter.Checked = Properties.VehicleSettings.Default.setVehicle_antennaOffset == 0;

            if (Properties.VehicleSettings.Default.setVehicle_vehicleType == 0)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaTractor;

            else if (Properties.VehicleSettings.Default.setVehicle_vehicleType == 1)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaHarvester;

            else if (Properties.VehicleSettings.Default.setVehicle_vehicleType == 2)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaArticulated;

            label98.Text = mf.unitsInCm;
            label99.Text = mf.unitsInCm;
            label100.Text = mf.unitsInCm;
        }

        private void tabVAntenna_Leave(object sender, EventArgs e)
        {
            Properties.VehicleSettings.Default.Save(RegistrySettings.vehicleFileName);
        }

        private void rbtnAntennaLeft_Click(object sender, EventArgs e)
        {
            if (rbtnAntennaRight.Checked)
                mf.vehicle.VehicleConfig.AntennaOffset = (double)nudAntennaOffset.Value * -mf.inchOrCm2m;
            else if (rbtnAntennaLeft.Checked)
                mf.vehicle.VehicleConfig.AntennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
            else
            {
                mf.vehicle.VehicleConfig.AntennaOffset = 0;
                nudAntennaOffset.Value = 0;
            }

            Properties.VehicleSettings.Default.setVehicle_antennaOffset = mf.vehicle.VehicleConfig.AntennaOffset;
        }

        private void nudAntennaOffset_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                if ((double)nudAntennaOffset.Value == 0)
                {
                    rbtnAntennaLeft.Checked = false;
                    rbtnAntennaRight.Checked = false;
                    rbtnAntennaCenter.Checked = true;
                    mf.vehicle.VehicleConfig.AntennaOffset = 0;
                }
                else
                {
                    if (!rbtnAntennaLeft.Checked && !rbtnAntennaRight.Checked)
                        rbtnAntennaRight.Checked = true;

                    if (rbtnAntennaRight.Checked)
                        mf.vehicle.VehicleConfig.AntennaOffset = (double)nudAntennaOffset.Value * -mf.inchOrCm2m;
                    else
                        mf.vehicle.VehicleConfig.AntennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
                }

                Properties.VehicleSettings.Default.setVehicle_antennaOffset = mf.vehicle.VehicleConfig.AntennaOffset;
            }

            //rbtnAntennaLeft.Checked = false;
            //rbtnAntennaRight.Checked = false;
            //rbtnAntennaLeft.Checked = Properties.VehicleSettings.Default.setVehicle_antennaOffset > 0;
            //rbtnAntennaRight.Checked = Properties.VehicleSettings.Default.setVehicle_antennaOffset < 0;
        }

        private void nudAntennaPivot_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setVehicle_antennaPivot = (double)nudAntennaPivot.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.AntennaPivot = Properties.VehicleSettings.Default.setVehicle_antennaPivot;
            }
        }

        private void nudAntennaHeight_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setVehicle_antennaHeight = (double)nudAntennaHeight.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.AntennaHeight = Properties.VehicleSettings.Default.setVehicle_antennaHeight;
            }
        }

        #endregion

        #region Vehicle Dimensions

        private void tabVDimensions_Enter(object sender, EventArgs e)
        {
            nudWheelbase.Value = (int)(Math.Abs(Properties.VehicleSettings.Default.setVehicle_wheelbase) * mf.m2InchOrCm);

            nudVehicleTrack.Value = (int)(Math.Abs(Properties.VehicleSettings.Default.setVehicle_trackWidth) * mf.m2InchOrCm);

            nudTractorHitchLength.Value = (int)(Math.Abs(Properties.VehicleSettings.Default.setVehicle_hitchLength) * mf.m2InchOrCm);

            if (mf.vehicle.VehicleConfig.Type == VehicleType.Tractor)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBase;
            }
            else if (mf.vehicle.VehicleConfig.Type == VehicleType.Harvester)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBaseHarvester;
            }
            else if (mf.vehicle.VehicleConfig.Type == VehicleType.Articulated)
            {
                pictureBox1.Image = Properties.Resources.RadiusWheelBaseArticulated;
            }

            nudTractorHitchLength.Visible = rbtnTBT.Checked || rbtnTrailing.Checked;
            label94.Visible = rbtnTBT.Checked || rbtnTrailing.Checked;
            labelHitchLength.Visible = rbtnTBT.Checked || rbtnTrailing.Checked;
            HitchLengthBlindBox.Visible = rbtnFixedRear.Checked || rbtnFront.Checked;

            label94.Text = mf.unitsInCm;
            label95.Text = mf.unitsInCm;
            label97.Text = mf.unitsInCm;
        }

        private void nudTractorHitchLength_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                mf.tool.hitchLength = (double)nudTractorHitchLength.Value * mf.inchOrCm2m;
                if (!Properties.ToolSettings.Default.setTool_isToolFront)
                {
                    mf.tool.hitchLength *= -1;
                }
                Properties.VehicleSettings.Default.setVehicle_hitchLength = mf.tool.hitchLength;
            }
        }

        private void nudWheelbase_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setVehicle_wheelbase = (double)nudWheelbase.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.Wheelbase = Properties.VehicleSettings.Default.setVehicle_wheelbase;
                Properties.VehicleSettings.Default.Save(RegistrySettings.vehicleFileName);
            }
        }

        private void nudVehicleTrack_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setVehicle_trackWidth = (double)nudVehicleTrack.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.TrackWidth = Properties.VehicleSettings.Default.setVehicle_trackWidth;
                mf.tram.halfWheelTrack = mf.vehicle.VehicleConfig.TrackWidth * 0.5;
                Properties.VehicleSettings.Default.Save(RegistrySettings.vehicleFileName);
            }
        }

        #endregion

        #region Vehicle Guidance

        private void tabVGuidance_Enter(object sender, EventArgs e)
        {
        }

        private void tabVGuidance_Leave(object sender, EventArgs e)
        {
        }
        #endregion

        #region VConfig Enter/Leave

        private void tabVConfig_Enter(object sender, EventArgs e)
        {
            configVehicleControl.Initialize(mf.vehicle.VehicleConfig);
        }

        private void tabVConfig_Leave(object sender, EventArgs e)
        {
            configVehicleControl.UpdateSettings();

            if (mf.vehicle.VehicleConfig.Type == VehicleType.Harvester)
            {
                if (mf.tool.hitchLength < 0) mf.tool.hitchLength *= -1;

                Properties.ToolSettings.Default.setTool_isToolFront = true;
                Properties.ToolSettings.Default.setTool_isToolTBT = false;
                Properties.ToolSettings.Default.setTool_isToolTrailing = false;
                Properties.ToolSettings.Default.setTool_isToolRearFixed = false;
            }

            switch (mf.vehicle.VehicleConfig.Type)
            {
                case VehicleType.Tractor:
                    mf.VehicleTextures.Tractor.SetBitmap(TractorBitmaps.GetBitmap(configVehicleControl.TractorBrand));
                    break;
                case VehicleType.Harvester:
                    mf.VehicleTextures.Harvester.SetBitmap(HarvesterBitmaps.GetBitmap(configVehicleControl.HarvesterBrand));
                    break;
                case VehicleType.Articulated:
                    mf.VehicleTextures.ArticulatedFront.SetBitmap(ArticulatedBitmaps.GetFrontBitmap(configVehicleControl.ArticulatedBrand));
                    mf.VehicleTextures.ArticulatedRear.SetBitmap(ArticulatedBitmaps.GetRearBitmap(configVehicleControl.ArticulatedBrand));
                    break;
            }

            Properties.VehicleSettings.Default.Save(RegistrySettings.vehicleFileName);
            Properties.ToolSettings.Default.Save(RegistrySettings.toolFileName);
            Settings.Default.Save();
        }

        #endregion
    }
}


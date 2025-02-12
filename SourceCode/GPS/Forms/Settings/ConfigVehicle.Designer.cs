using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using AgLibrary.Settings;
using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Culture;
using AgOpenGPS.Properties;
using OpenTK.Graphics.OpenGL;
using AgOpenGPS.Core.Models;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        #region Vehicle Save---------------------------------------------

        private void btnVehicleLoad_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (lvVehicles.SelectedItems.Count > 0)
                {
                    string newVehicleName = lvVehicles.SelectedItems[0].SubItems[0].Text;
                    DialogResult result3 = MessageBox.Show(
                        "Open: " + newVehicleName + ".XML ?",
                        gStr.gsSaveAndReturn,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                    if (result3 == DialogResult.Yes)
                    {
                        RegistrySettings.Save(RegKeys.vehicleFileName, newVehicleName);

                        var result = Settings.Default.Load();
                        if (result != LoadResult.Ok)
                        {
                            Log.EventWriter("Vehicle Loaded: " + newVehicleName + ".XML With Error:" + result.ToString());

                            MessageBox.Show("There was an error while opening the xml \r\n\r\n Please check your settings");
                        }

                        Log.EventWriter("Vehicle Loaded: " + RegistrySettings.vehicleFileName + ".XML");

                        LoadBrandImage();

                        mf.vehicle = new CVehicle(mf);
                        mf.tool = new CTool(mf);

                        //reset AOG
                        mf.LoadSettings();

                        SectionFeetInchesTotalWidthLabelUpdate();

                        SendSettings();

                        //Send Pin configuration
                        SendRelaySettingsToMachineModule();

                        ///Remind the user
                        mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");
                    }
                    UpdateSummary();
                }
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
            }

            btnOK.PerformClick();
        }

        private void btnVehicleDelete_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (lvVehicles.SelectedItems.Count > 0)
                {
                    if (lvVehicles.SelectedItems[0].SubItems[0].Text != RegistrySettings.vehicleFileName)
                    {
                        DialogResult result3 = MessageBox.Show(
                        "Delete: " + lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML",
                        gStr.gsSaveAndReturn,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Error,
                        MessageBoxDefaultButton.Button2);
                        if (result3 == DialogResult.Yes)
                        {
                            File.Delete(Path.Combine(RegistrySettings.vehiclesDirectory, lvVehicles.SelectedItems[0].SubItems[0].Text + ".XML"));
                        }
                    }
                    else
                    {
                        mf.TimedMessageBox(2000, "Vehicle In Use", "Select Different Vehicle");
                    }
                }
            }

            UpdateVehicleListView();
        }

        //Save As Vehicle
        private void btnVehicleSave_Click(object sender, EventArgs e)
        {
            btnVehicleSave.BackColor = Color.Transparent;
            btnVehicleSave.Enabled = false;

            string newVehicleName = SanitizeFileName(tboxVehicleNameSave.Text.Trim()).Trim();
            tboxVehicleNameSave.Text = "";

            if (newVehicleName.Length > 0)
            {
                RegistrySettings.Save(RegKeys.vehicleFileName, newVehicleName);
                Settings.Default.Save();
            }
            UpdateVehicleListView();
            UpdateSummary();
        }

        private void tboxVehicleNameSave_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();

            if (String.IsNullOrEmpty(tboxVehicleNameSave.Text.Trim()))
            {
                btnVehicleSave.Enabled = false;
                btnVehicleSave.BackColor = Color.Transparent;
            }
            else
            {
                btnVehicleSave.Enabled = true;
                btnVehicleSave.BackColor = Color.LimeGreen;
            }
        }
        private void tboxVehicleNameSave_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (mf.isKeyboardOn)
                {
                    ((TextBox)sender).ShowKeyboard(this);
                }
            }
            else
            {
                mf.TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                tboxVehicleNameSave.Enabled = false;
            }
        }

        private void tboxVehicleNameSave_Enter(object sender, EventArgs e)
        {
            //btnVehicleSaveAs.Enabled = false;
            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();
        }

        //New Vehicle
        private void tboxCreateNewVehicle_TextChanged(object sender, EventArgs e)
        {
            var textboxSender = (TextBox)sender;
            var cursorPosition = textboxSender.SelectionStart;
            textboxSender.Text = Regex.Replace(textboxSender.Text, glm.fileRegex, "");
            textboxSender.SelectionStart = cursorPosition;

            btnVehicleSave.Enabled = false;
            btnVehicleLoad.Enabled = false;
            btnVehicleDelete.Enabled = false;

            lvVehicles.SelectedItems.Clear();

            if (String.IsNullOrEmpty(tboxCreateNewVehicle.Text.Trim()))
            {
                btnVehicleNewSave.Enabled = false;
                btnVehicleNewSave.BackColor = Color.Transparent;
            }
            else
            {
                btnVehicleNewSave.Enabled = true;
                btnVehicleNewSave.BackColor = Color.LimeGreen;
            }
        }

        private void tboxCreateNewVehicle_Click(object sender, EventArgs e)
        {
            if (!mf.isJobStarted)
            {
                if (mf.isKeyboardOn)
                {
                    ((TextBox)sender).ShowKeyboard(this);
                }
            }
            else
            {
                var form = new FormTimedMessage(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                form.Show(this);
                tboxCreateNewVehicle.Enabled = false;
            }
        }

        private void btnVehicleNewSave_Click(object sender, EventArgs e)
        {
            btnVehicleNewSave.BackColor = Color.Transparent;
            btnVehicleNewSave.Enabled = false;

            string newVehicleName = SanitizeFileName(tboxCreateNewVehicle.Text.Trim()).Trim();
            tboxCreateNewVehicle.Text = "";

            if (newVehicleName.Length > 0)
            {
                RegistrySettings.Save(RegKeys.vehicleFileName, newVehicleName);
                
                Settings.Default.Reset();
                Settings.Default.Save();

                Log.EventWriter("New Vehicle Loaded: " + RegistrySettings.vehicleFileName + ".XML");

                LoadBrandImage();

                mf.vehicle = new CVehicle(mf);
                mf.tool = new CTool(mf);

                //reset AOG
                mf.LoadSettings();

                SectionFeetInchesTotalWidthLabelUpdate();

                SendSettings();

                //Send Pin configuration
                SendRelaySettingsToMachineModule();

                ///Remind the user
                mf.TimedMessageBox(2500, "Steer and Machine Settings Sent", "Were Modules Connected?");
            }
            UpdateVehicleListView();
            UpdateSummary();
        }        

        //Functions
        private static readonly Regex InvalidFileRegex = new Regex(string.Format("[{0}]", Regex.Escape(@"<>:""/\|?*")));
        public static string SanitizeFileName(string fileName)
        {
            return InvalidFileRegex.Replace(fileName, string.Empty);
        }

        private void UpdateVehicleListView()
        {
            DirectoryInfo dinfo = new DirectoryInfo(RegistrySettings.vehiclesDirectory);
            FileInfo[] Files = dinfo.GetFiles("*.XML");

            //load the listbox
            lvVehicles.Items.Clear();
            foreach (FileInfo file in Files)
            {
                lvVehicles.Items.Add(Path.GetFileNameWithoutExtension(file.Name));
            }

            //deselect everything
            lvVehicles.SelectedItems.Clear();
            lblSummaryVehicleName.Text = RegistrySettings.vehicleFileName;

            //tboxCreateNewVehicle.Text = "";
            //tboxVehicleNameSave.Text = "";
        }

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

            //mf.timeToShowMenus = (int)nudMenusOnTime.Value;

            Properties.Settings.Default.setDisplay_isBrightnessOn = mf.isBrightnessOn;
            Properties.Settings.Default.setDisplay_isTextureOn = mf.isTextureOn;
            Properties.Settings.Default.setMenu_isGridOn = mf.isGridOn;
            Properties.Settings.Default.setMenu_isCompassOn = mf.isCompassOn;

            Properties.Settings.Default.setDisplay_isSvennArrowOn = mf.isSvennArrowOn;
            Properties.Settings.Default.setMenu_isSpeedoOn = mf.isSpeedoOn;
            Properties.Settings.Default.setDisplay_isStartFullScreen = chkDisplayStartFullScreen.Checked;
            Properties.Settings.Default.setMenu_isSideGuideLines = mf.isSideGuideLines;

            Properties.Settings.Default.setMenu_isPureOn = mf.isPureDisplayOn;
            Properties.Settings.Default.setMenu_isLightbarOn = mf.isLightbarOn;
            Properties.Settings.Default.setDisplay_isKeyboardOn = mf.isKeyboardOn;
            Properties.Settings.Default.setDisplay_isLogElevation = mf.isLogElevation;

            Properties.Settings.Default.setMenu_isMetric = rbtnDisplayMetric.Checked;
            mf.isMetric = rbtnDisplayMetric.Checked;

            Properties.Settings.Default.setTool_isDirectionMarkers = mf.isDirectionMarkers;

            Properties.Settings.Default.setAS_numGuideLines = mf.ABLine.numGuideLines;
            Properties.Settings.Default.setDisplay_isSectionLinesOn = mf.isSectionlinesOn;
            Properties.Settings.Default.setDisplay_isLineSmooth = mf.isLineSmooth;

            Properties.Settings.Default.Save();
        }

        #endregion

        #region Antenna Enter/Leave
        private void tabVAntenna_Enter(object sender, EventArgs e)
        {
            nudAntennaHeight.Value = (int)(Properties.Settings.Default.setVehicle_antennaHeight * mf.m2InchOrCm);

            nudAntennaPivot.Value = (int)((Properties.Settings.Default.setVehicle_antennaPivot) * mf.m2InchOrCm);

            //negative is to the right
            nudAntennaOffset.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_antennaOffset) * mf.m2InchOrCm);

            rbtnAntennaLeft.Checked = false;
            rbtnAntennaRight.Checked = false;
            rbtnAntennaCenter.Checked = false;
            rbtnAntennaLeft.Checked = Properties.Settings.Default.setVehicle_antennaOffset > 0;
            rbtnAntennaRight.Checked = Properties.Settings.Default.setVehicle_antennaOffset < 0;
            rbtnAntennaCenter.Checked = Properties.Settings.Default.setVehicle_antennaOffset == 0;

            if (Properties.Settings.Default.setVehicle_vehicleType == 0)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaTractor;

            else if (Properties.Settings.Default.setVehicle_vehicleType == 1)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaHarvester;

            else if (Properties.Settings.Default.setVehicle_vehicleType == 2)
                pboxAntenna.BackgroundImage = Properties.Resources.AntennaArticulated;

            label98.Text = mf.unitsInCm;
            label99.Text = mf.unitsInCm;
            label100.Text = mf.unitsInCm;
        }

        private void tabVAntenna_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
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

            Properties.Settings.Default.setVehicle_antennaOffset = mf.vehicle.VehicleConfig.AntennaOffset;
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

                Properties.Settings.Default.setVehicle_antennaOffset = mf.vehicle.VehicleConfig.AntennaOffset;
            }

            //rbtnAntennaLeft.Checked = false;
            //rbtnAntennaRight.Checked = false;
            //rbtnAntennaLeft.Checked = Properties.Settings.Default.setVehicle_antennaOffset > 0;
            //rbtnAntennaRight.Checked = Properties.Settings.Default.setVehicle_antennaOffset < 0;
        }

        private void nudAntennaPivot_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setVehicle_antennaPivot = (double)nudAntennaPivot.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.AntennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
            }
        }

        private void nudAntennaHeight_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setVehicle_antennaHeight = (double)nudAntennaHeight.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.AntennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
            }
        }

        #endregion

        #region Vehicle Dimensions

        private void tabVDimensions_Enter(object sender, EventArgs e)
        {
            nudWheelbase.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_wheelbase) * mf.m2InchOrCm);

            nudVehicleTrack.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_trackWidth) * mf.m2InchOrCm);

            nudTractorHitchLength.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_hitchLength) * mf.m2InchOrCm);

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

            nudTractorHitchLength.Visible = Properties.Settings.Default.setTool_isToolTrailing || Properties.Settings.Default.setTool_isToolTBT;
            label94.Visible = nudTractorHitchLength.Visible;
            labelHitchLength.Visible = nudTractorHitchLength.Visible;

            label94.Text = mf.unitsInCm;
            label95.Text = mf.unitsInCm;
            label97.Text = mf.unitsInCm;
        }

        private void nudTractorHitchLength_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                mf.tool.hitchLength = (double)nudTractorHitchLength.Value * mf.inchOrCm2m;
                if (!Properties.Settings.Default.setTool_isToolFront)
                {
                    mf.tool.hitchLength *= -1;
                }
                Properties.Settings.Default.setVehicle_hitchLength = mf.tool.hitchLength;
            }
        }

        private void nudWheelbase_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setVehicle_wheelbase = (double)nudWheelbase.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.Wheelbase = Properties.Settings.Default.setVehicle_wheelbase;
                Properties.Settings.Default.Save();
            }
        }

        private void nudVehicleTrack_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setVehicle_trackWidth = (double)nudVehicleTrack.Value * mf.inchOrCm2m;
                mf.vehicle.VehicleConfig.TrackWidth = Properties.Settings.Default.setVehicle_trackWidth;
                mf.tram.halfWheelTrack = mf.vehicle.VehicleConfig.TrackWidth * 0.5;
                Properties.Settings.Default.Save();
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
            if (mf.vehicle.VehicleConfig.Type == VehicleType.Tractor) rbtnTractor.Checked = true;
            else if (mf.vehicle.VehicleConfig.Type == VehicleType.Harvester) rbtnHarvester.Checked = true;
            else if (mf.vehicle.VehicleConfig.Type == VehicleType.Articulated) rbtnArticulated.Checked = true;

            original = null;
            TabImageSetup();
        }

        private void tabVConfig_Leave(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked)
            {
                mf.vehicle.VehicleConfig.Type = VehicleType.Tractor;
                Properties.Settings.Default.setVehicle_vehicleType = 0;
            }
            if (rbtnHarvester.Checked)
            {
                mf.vehicle.VehicleConfig.Type = VehicleType.Harvester;
                Properties.Settings.Default.setVehicle_vehicleType = 1;

                if ( mf.tool.hitchLength < 0) mf.tool.hitchLength *= -1;

                Properties.Settings.Default.setTool_isToolFront = true;
                Properties.Settings.Default.setTool_isToolTBT = false;
                Properties.Settings.Default.setTool_isToolTrailing = false;
                Properties.Settings.Default.setTool_isToolRearFixed = false;
            }
            if (rbtnArticulated.Checked)
            {
                mf.vehicle.VehicleConfig.Type = VehicleType.Articulated;
                Properties.Settings.Default.setVehicle_vehicleType = 2;
            }

            //the old brand code
            Properties.Settings.Default.setDisplay_isVehicleImage = !cboxIsImage.Checked;

            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicle.VehicleConfig.Opacity * 100);

            Properties.Settings.Default.setDisplay_colorVehicle = (Color)mf.vehicle.VehicleConfig.Color;

            if (rbtnTractor.Checked)
            {
                Settings.Default.setBrand_TBrand = tractorBrand;

                Bitmap bitmap = mf.GetTractorBrand(tractorBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }

            if (rbtnHarvester.Checked)
            {
                Settings.Default.setBrand_HBrand = harvesterBrand;

                Bitmap bitmap = mf.GetHarvesterBrand(harvesterBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Harvester]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }

            if (rbtnArticulated.Checked)
            {
                Settings.Default.setBrand_WDBrand = articulatedBrand;

                Bitmap bitmap = mf.GetArticulatedBrandFront(articulatedBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ArticulatedFront]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

                bitmap = mf.GetArticulatedBrandRear(articulatedBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ArticulatedRear]);
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
            Properties.Settings.Default.Save();
        }

        //brand variables
        TractorBrand tractorBrand;
        HarvesterBrand harvesterBrand;
        ArticulatedBrand articulatedBrand;

        //Opacity Bar

        Image original = null;

        private void rbtnVehicleType_Click(object sender, EventArgs e)
        {
            if (rbtnTractor.Checked)
            {
                mf.vehicle.VehicleConfig.Type = VehicleType.Tractor;
                Properties.Settings.Default.setVehicle_vehicleType = 0;
            }
            if (rbtnHarvester.Checked)
            {
                mf.vehicle.VehicleConfig.Type = VehicleType.Harvester;
                Properties.Settings.Default.setVehicle_vehicleType = 1;
            }
            if (rbtnArticulated.Checked)
            {
                mf.vehicle.VehicleConfig.Type = VehicleType.Articulated;
                Properties.Settings.Default.setVehicle_vehicleType = 2;
            }

            original = null;
            TabImageSetup();
        }

        private void SetOpacity()
        {
            if (original == null) original = (Bitmap)pboxAlpha.BackgroundImage.Clone();
            pboxAlpha.BackColor = Color.Transparent;
            pboxAlpha.BackgroundImage = SetAlpha((Bitmap)original, (byte)(255 * mf.vehicle.VehicleConfig.Opacity));
        }

        private void btnOpacityUp_Click(object sender, EventArgs e)
        {
            mf.vehicle.VehicleConfig.Opacity = Math.Min(mf.vehicle.VehicleConfig.Opacity + 0.2, 1);
            OpacityChanged();
        }

        private void btnOpacityDn_Click(object sender, EventArgs e)
        {
            mf.vehicle.VehicleConfig.Opacity = Math.Max(mf.vehicle.VehicleConfig.Opacity - 0.2, 0.2);
            OpacityChanged();
        }

        private void OpacityChanged()
        {
            lblOpacityPercent.Text = ((int)(mf.vehicle.VehicleConfig.Opacity * 100)).ToString() + "%";
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicle.VehicleConfig.Opacity * 100);
            Properties.Settings.Default.Save();
            SetOpacity();
        }

        private void hsbarOpacity_ValueChanged(object sender, EventArgs e)
        {
            //lblOpacityPercent.Text = hsbarOpacity.Value.ToString() + "%";
            //mf.vehicleOpacityByte = (byte)(255 * (hsbarOpacity.Value * 0.01));
            //Properties.Settings.Default.setDisplay_colorVehicle = mf.vehicleColor;

            //if (original == null) original = (Bitmap)pboxAlpha.BackgroundImage.Clone();
            //pboxAlpha.BackColor = Color.Transparent;
            //pboxAlpha.BackgroundImage = SetAlpha((Bitmap)original, (byte)(255 * (hsbarOpacity.Value * 0.01)));
        }

        private void cboxIsImage_Click(object sender, EventArgs e)
        {
            //mf.vehicleOpacity = (hsbarOpacity.Value * 0.01);
            Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicle.VehicleConfig.Opacity * 100);

            mf.isVehicleImage = (!cboxIsImage.Checked);
            Properties.Settings.Default.setDisplay_isVehicleImage = mf.isVehicleImage;
            Properties.Settings.Default.Save();
            //original = null;
            TabImageSetup();
        }

        private void TabImageSetup()
        {
            panelArticulatedBrands.Visible = false;
            panelTractorBrands.Visible = false;
            panelHarvesterBrands.Visible = false;

            if (mf.isVehicleImage)
            {
                if (mf.vehicle.VehicleConfig.Type == VehicleType.Tractor)
                {
                    panelTractorBrands.Visible = true;

                    tractorBrand = Settings.Default.setBrand_TBrand;

                    if (tractorBrand == TractorBrand.AGOpenGPS)
                        rbtnBrandTAgOpenGPS.Checked = true;
                    else if (tractorBrand == TractorBrand.Case)
                        rbtnBrandTCase.Checked = true;
                    else if (tractorBrand == TractorBrand.Claas)
                        rbtnBrandTClaas.Checked = true;
                    else if (tractorBrand == TractorBrand.Deutz)
                        rbtnBrandTDeutz.Checked = true;
                    else if (tractorBrand == TractorBrand.Fendt)
                        rbtnBrandTFendt.Checked = true;
                    else if (tractorBrand == TractorBrand.JDeere)
                        rbtnBrandTJDeere.Checked = true;
                    else if (tractorBrand == TractorBrand.Kubota)
                        rbtnBrandTKubota.Checked = true;
                    else if (tractorBrand == TractorBrand.Massey)
                        rbtnBrandTMassey.Checked = true;
                    else if (tractorBrand == TractorBrand.NewHolland)
                        rbtnBrandTNH.Checked = true;
                    else if (tractorBrand == TractorBrand.Same)
                        rbtnBrandTSame.Checked = true;
                    else if (tractorBrand == TractorBrand.Steyr)
                        rbtnBrandTSteyr.Checked = true;
                    else if (tractorBrand == TractorBrand.Ursus)
                        rbtnBrandTUrsus.Checked = true;
                    else if (tractorBrand == TractorBrand.Valtra)
                        rbtnBrandTValtra.Checked = true;
                    else if (tractorBrand == TractorBrand.JCB)
                        rbtnBrandTJCB.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetTractorBrand(Settings.Default.setBrand_TBrand);
                }
                else if (mf.vehicle.VehicleConfig.Type == VehicleType.Harvester)
                {
                    panelHarvesterBrands.Visible = true;

                    harvesterBrand = Settings.Default.setBrand_HBrand;

                    if (harvesterBrand == HarvesterBrand.AgOpenGPS)
                        rbtnBrandHAgOpenGPS.Checked = true;
                    else if (harvesterBrand == HarvesterBrand.Case)
                        rbtnBrandHCase.Checked = true;
                    else if (harvesterBrand == HarvesterBrand.Claas)
                        rbtnBrandHClaas.Checked = true;
                    else if (harvesterBrand == HarvesterBrand.JDeere)
                        rbtnBrandHJDeere.Checked = true;
                    else if (harvesterBrand == HarvesterBrand.NewHolland)
                        rbtnBrandHNH.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(Settings.Default.setBrand_HBrand);
                }
                else if (mf.vehicle.VehicleConfig.Type == VehicleType.Articulated)
                {
                    panelArticulatedBrands.Visible = true;

                    articulatedBrand = Settings.Default.setBrand_WDBrand;

                    if (articulatedBrand == ArticulatedBrand.AgOpenGPS)
                        rbtnBrandAAgOpenGPS.Checked = true;
                    else if (articulatedBrand == ArticulatedBrand.Case)
                        rbtnBrandACase.Checked = true;
                    else if (articulatedBrand == ArticulatedBrand.Challenger)
                        rbtnBrandAChallenger.Checked = true;
                    else if (articulatedBrand == ArticulatedBrand.JDeere)
                        rbtnBrandAJDeere.Checked = true;
                    else if (articulatedBrand == ArticulatedBrand.NewHolland)
                        rbtnBrandANH.Checked = true;
                    else if (articulatedBrand == ArticulatedBrand.Holder)
                        rbtnBrandAHolder.Checked = true;

                    pboxAlpha.BackgroundImage = mf.GetArticulatedBrandFront(Settings.Default.setBrand_WDBrand);
                }

                Properties.Settings.Default.setDisplay_vehicleOpacity = (int)(mf.vehicle.VehicleConfig.Opacity * 100);
                lblOpacityPercent.Text = ((int)(mf.vehicle.VehicleConfig.Opacity * 100)).ToString() + "%";
                mf.vehicle.VehicleConfig.Color = new ColorRgb(254, 254, 254);
            }
            else
            {
                pboxAlpha.BackgroundImage = Properties.Resources.TriangleVehicle;
                lblOpacityPercent.Text = ((int)(mf.vehicle.VehicleConfig.Opacity * 100)).ToString() + "%";
                mf.vehicle.VehicleConfig.Color = new ColorRgb(254, 254, 254);
            }

            cboxIsImage.Checked = !mf.isVehicleImage;

            original = null;
            SetOpacity();
        }

        static Bitmap SetAlpha(Bitmap bmpIn, int alpha)
        {
            Bitmap bmpOut = new Bitmap(bmpIn.Width, bmpIn.Height);
            float a = alpha / 255f;
            Rectangle r = new Rectangle(0, 0, bmpIn.Width, bmpIn.Height);

            float[][] matrixItems = {
                            new float[] {1, 0, 0, 0, 0},
                            new float[] {0, 1, 0, 0, 0},
                            new float[] {0, 0, 1, 0, 0},
                            new float[] {0, 0, 0, a, 0},
                            new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrixItems);

            ImageAttributes imageAtt = new ImageAttributes();
            imageAtt.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            using (Graphics g = Graphics.FromImage(bmpOut))
                g.DrawImage(bmpIn, r, r.X, r.Y, r.Width, r.Height, GraphicsUnit.Pixel, imageAtt);

            return bmpOut;
        }

        private void LoadBrandImage()
        {
            if (rbtnTractor.Checked == true)
            {
                Bitmap bitmap = mf.GetTractorBrand(Settings.Default.setBrand_TBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Tractor]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtnHarvester.Checked == true)

            {
                Bitmap bitmap = mf.GetHarvesterBrand(Settings.Default.setBrand_HBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.Harvester]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);

            }

            if (rbtnArticulated.Checked == true)

            {
                Bitmap bitmap = mf.GetArticulatedBrandFront(Settings.Default.setBrand_WDBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ArticulatedFront]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
                bitmap = mf.GetArticulatedBrandRear(Settings.Default.setBrand_WDBrand);

                GL.BindTexture(TextureTarget.Texture2D, mf.texture[(int)FormGPS.textures.ArticulatedRear]);
                bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
            }
        }

        private void SendSettings()
        {
            //Form Steer Settings
            mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
            mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

            mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
            mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

            mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
            mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
            mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
            mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

            mf.SendPgnToLoop(mf.p_252.pgn);

            //steer config
            mf.p_251.pgn[mf.p_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
            mf.p_251.pgn[mf.p_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
            mf.p_251.pgn[mf.p_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
            mf.p_251.pgn[mf.p_251.minSpeed] = unchecked((byte)(Properties.Settings.Default.setAS_minSteerSpeed * 10));

            if (Properties.Settings.Default.setAS_isConstantContourOn)
                mf.p_251.pgn[mf.p_251.angVel] = 1;
            else mf.p_251.pgn[mf.p_251.angVel] = 0;

            mf.SendPgnToLoop(mf.p_251.pgn);

            //machine settings    
            mf.p_238.pgn[mf.p_238.set0] = Properties.Settings.Default.setArdMac_setting0;
            mf.p_238.pgn[mf.p_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
            mf.p_238.pgn[mf.p_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

            mf.p_238.pgn[mf.p_238.user1] = Properties.Settings.Default.setArdMac_user1;
            mf.p_238.pgn[mf.p_238.user2] = Properties.Settings.Default.setArdMac_user2;
            mf.p_238.pgn[mf.p_238.user3] = Properties.Settings.Default.setArdMac_user3;
            mf.p_238.pgn[mf.p_238.user4] = Properties.Settings.Default.setArdMac_user4;

            mf.SendPgnToLoop(mf.p_238.pgn);
        }

        private void HarvesterBrandCheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                harvesterBrand = (HarvesterBrand)radioButton.Tag;
                pboxAlpha.BackgroundImage = mf.GetHarvesterBrand(harvesterBrand);
                original = null;
                SetOpacity();
            }
        }

        private void TractorBrandCheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                tractorBrand = (TractorBrand)radioButton.Tag;
                pboxAlpha.BackgroundImage = mf.GetTractorBrand(tractorBrand);
                original = null;
                SetOpacity();
            }
        }

        private void ArticulatedBrandCheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                articulatedBrand = (ArticulatedBrand)radioButton.Tag;
                pboxAlpha.BackgroundImage = mf.GetArticulatedBrandFront(articulatedBrand);
                original = null;
                SetOpacity();
            }
        }

        #endregion
    }
}


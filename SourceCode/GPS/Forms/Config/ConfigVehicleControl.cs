using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Properties;
using AgOpenGPS.ResourcesBrands;

namespace AgOpenGPS.Forms.Config
{
    public partial class ConfigVehicleControl : UserControl
    {
        private VehicleConfig _vehicleConfig;
        private TractorBrand _tractorBrand;
        private HarvesterBrand _harvesterBrand;
        private ArticulatedBrand _articulatedBrand;
        private Image _original = null;

        public ConfigVehicleControl()
        {
            InitializeComponent();

            labelVehicleGroupBox.Text = gStr.gsVehiclegroupbox;
            labelImage.Text = gStr.gsImage + ":";
            labelOpacity.Text = gStr.gsOpacity + ":";
        }

        public TractorBrand TractorBrand
        {
            get => _tractorBrand;
            private set
            {
                _tractorBrand = value;

                pboxAlpha.BackgroundImage = TractorBitmaps.GetBitmap(_tractorBrand);
            }
        }

        public HarvesterBrand HarvesterBrand
        {
            get => _harvesterBrand;
            private set
            {
                _harvesterBrand = value;

                pboxAlpha.BackgroundImage = HarvesterBitmaps.GetBitmap(_harvesterBrand);
            }
        }

        public ArticulatedBrand ArticulatedBrand
        {
            get => _articulatedBrand;
            private set
            {
                _articulatedBrand = value;

                pboxAlpha.BackgroundImage = ArticulatedBitmaps.GetFrontBitmap(_articulatedBrand);
            }
        }

        public void Initialize(VehicleConfig vehicleConfig)
        {
            _vehicleConfig = vehicleConfig;

            switch (_vehicleConfig.Type)
            {
                case VehicleType.Tractor:
                    rbtnTractor.Checked = true;
                    break;
                case VehicleType.Harvester:
                    rbtnHarvester.Checked = true;
                    break;
                case VehicleType.Articulated:
                    rbtnArticulated.Checked = true;
                    break;
            }

            UpdateImage();
        }

        public void UpdateSettings()
        {
            switch (_vehicleConfig.Type)
            {
                case VehicleType.Tractor:
                    Settings.Default.setVehicle_vehicleType = 0;
                    Settings.Default.setBrand_TBrand = TractorBrand;
                    break;
                case VehicleType.Harvester:
                    Settings.Default.setVehicle_vehicleType = 1;
                    Settings.Default.setBrand_HBrand = HarvesterBrand;
                    break;
                case VehicleType.Articulated:
                    Settings.Default.setVehicle_vehicleType = 2;
                    Settings.Default.setBrand_WDBrand = ArticulatedBrand;
                    break;
            }

            Settings.Default.setDisplay_isVehicleImage = _vehicleConfig.IsImage;
            Settings.Default.setDisplay_vehicleOpacity = (int)(_vehicleConfig.Opacity * 100);
            Settings.Default.setDisplay_colorVehicle = (Color)_vehicleConfig.Color;
        }

        private void UpdateImage()
        {
            panelArticulatedBrands.Visible = false;
            panelTractorBrands.Visible = false;
            panelHarvesterBrands.Visible = false;

            if (_vehicleConfig.IsImage)
            {
                if (_vehicleConfig.Type == VehicleType.Tractor)
                {
                    panelTractorBrands.Visible = true;

                    TractorBrand = Settings.Default.setBrand_TBrand;
                    UpdateTractorBrand();
                }
                else if (_vehicleConfig.Type == VehicleType.Harvester)
                {
                    panelHarvesterBrands.Visible = true;

                    HarvesterBrand = Settings.Default.setBrand_HBrand;
                    UpdateHarvesterBrand();
                }
                else if (_vehicleConfig.Type == VehicleType.Articulated)
                {
                    panelArticulatedBrands.Visible = true;

                    ArticulatedBrand = Settings.Default.setBrand_WDBrand;
                    UpdateArticulatedBrand();
                }

                Settings.Default.setDisplay_vehicleOpacity = (int)(_vehicleConfig.Opacity * 100);
            }
            else
            {
                pboxAlpha.BackgroundImage = BrandImages.BrandTriangleVehicle;
            }

            _vehicleConfig.Color = new ColorRgb(254, 254, 254);

            cboxIsImage.Checked = !_vehicleConfig.IsImage;
            ResetImage();
        }

        private void UpdateTractorBrand()
        {
            if (TractorBrand == TractorBrand.AGOpenGPS)
                rbtnBrandTAgOpenGPS.Checked = true;
            else if (TractorBrand == TractorBrand.Case)
                rbtnBrandTCase.Checked = true;
            else if (TractorBrand == TractorBrand.Claas)
                rbtnBrandTClaas.Checked = true;
            else if (TractorBrand == TractorBrand.Deutz)
                rbtnBrandTDeutz.Checked = true;
            else if (TractorBrand == TractorBrand.Fendt)
                rbtnBrandTFendt.Checked = true;
            else if (TractorBrand == TractorBrand.JohnDeere)
                rbtnBrandTJDeere.Checked = true;
            else if (TractorBrand == TractorBrand.Kubota)
                rbtnBrandTKubota.Checked = true;
            else if (TractorBrand == TractorBrand.Massey)
                rbtnBrandTMassey.Checked = true;
            else if (TractorBrand == TractorBrand.NewHolland)
                rbtnBrandTNH.Checked = true;
            else if (TractorBrand == TractorBrand.Same)
                rbtnBrandTSame.Checked = true;
            else if (TractorBrand == TractorBrand.Steyr)
                rbtnBrandTSteyr.Checked = true;
            else if (TractorBrand == TractorBrand.Ursus)
                rbtnBrandTUrsus.Checked = true;
            else if (TractorBrand == TractorBrand.Valtra)
                rbtnBrandTValtra.Checked = true;
            else if (TractorBrand == TractorBrand.JCB)
                rbtnBrandTJCB.Checked = true;
        }

        private void UpdateHarvesterBrand()
        {
            if (HarvesterBrand == HarvesterBrand.AgOpenGPS)
                rbtnBrandHAgOpenGPS.Checked = true;
            else if (HarvesterBrand == HarvesterBrand.Case)
                rbtnBrandHCase.Checked = true;
            else if (HarvesterBrand == HarvesterBrand.Claas)
                rbtnBrandHClaas.Checked = true;
            else if (HarvesterBrand == HarvesterBrand.JohnDeere)
                rbtnBrandHJDeere.Checked = true;
            else if (HarvesterBrand == HarvesterBrand.NewHolland)
                rbtnBrandHNH.Checked = true;
        }

        private void UpdateArticulatedBrand()
        {
            if (ArticulatedBrand == ArticulatedBrand.AgOpenGPS)
                rbtnBrandAAgOpenGPS.Checked = true;
            else if (ArticulatedBrand == ArticulatedBrand.Case)
                rbtnBrandACase.Checked = true;
            else if (ArticulatedBrand == ArticulatedBrand.Challenger)
                rbtnBrandAChallenger.Checked = true;
            else if (ArticulatedBrand == ArticulatedBrand.JohnDeere)
                rbtnBrandAJDeere.Checked = true;
            else if (ArticulatedBrand == ArticulatedBrand.NewHolland)
                rbtnBrandANH.Checked = true;
            else if (ArticulatedBrand == ArticulatedBrand.Holder)
                rbtnBrandAHolder.Checked = true;
        }

        private void ResetImage()
        {
            _original = null;
            UpdateOpacity();
        }

        private void UpdateOpacity()
        {
            if (_original == null)
                _original = (Bitmap)pboxAlpha.BackgroundImage.Clone();

            pboxAlpha.BackColor = Color.Transparent;
            pboxAlpha.BackgroundImage = SetAlpha((Bitmap)_original, (byte)(255 * _vehicleConfig.Opacity));

            lblOpacityPercent.Text = ((int)(_vehicleConfig.Opacity * 100)).ToString() + "%";
        }

        private void rbtnTractor_Click(object sender, EventArgs e)
        {
            _vehicleConfig.Type = VehicleType.Tractor;
            Settings.Default.setVehicle_vehicleType = 0;
            UpdateImage();
        }

        private void rbtnHarvester_Click(object sender, EventArgs e)
        {
            _vehicleConfig.Type = VehicleType.Harvester;
            Settings.Default.setVehicle_vehicleType = 1;
            UpdateImage();
        }

        private void rbtnArticulated_Click(object sender, EventArgs e)
        {
            _vehicleConfig.Type = VehicleType.Articulated;
            Settings.Default.setVehicle_vehicleType = 2;
            UpdateImage();
        }

        private void rbtnTractorBrand_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                TractorBrand = (TractorBrand)radioButton.Tag;
                ResetImage();
            }
        }

        private void rbtnHarvesterBrand_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                HarvesterBrand = (HarvesterBrand)radioButton.Tag;
                ResetImage();
            }
        }

        private void rbtnArticulatedBrand_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = sender as RadioButton;
            if (radioButton.Checked)
            {
                ArticulatedBrand = (ArticulatedBrand)radioButton.Tag;
                ResetImage();
            }
        }

        private void cboxIsImage_Click(object sender, EventArgs e)
        {
            _vehicleConfig.IsImage = !cboxIsImage.Checked;

            Settings.Default.setDisplay_vehicleOpacity = (int)(_vehicleConfig.Opacity * 100);
            Settings.Default.setDisplay_isVehicleImage = _vehicleConfig.IsImage;
            Settings.Default.Save();

            UpdateImage();
        }

        private void btnOpacityDn_Click(object sender, EventArgs e)
        {
            _vehicleConfig.Opacity = Math.Max(_vehicleConfig.Opacity - 0.2, 0.2);

            Settings.Default.setDisplay_vehicleOpacity = (int)(_vehicleConfig.Opacity * 100);
            Settings.Default.Save();

            UpdateOpacity();
        }

        private void btnOpacityUp_Click(object sender, EventArgs e)
        {
            _vehicleConfig.Opacity = Math.Min(_vehicleConfig.Opacity + 0.2, 1);

            Settings.Default.setDisplay_vehicleOpacity = (int)(_vehicleConfig.Opacity * 100);
            Settings.Default.Save();

            UpdateOpacity();
        }

        private static Bitmap SetAlpha(Bitmap bmpIn, int alpha)
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
    }
}

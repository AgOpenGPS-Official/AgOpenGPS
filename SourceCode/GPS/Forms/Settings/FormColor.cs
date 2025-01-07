//Please, if you use this, share the improvements

using AgOpenGPS.Culture;
using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormColor : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        private bool daySet;

        //constructor
        public FormColor(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = gStr.gsColors;
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            daySet = mf.isDay;
            hsbarSmooth.Value = Properties.Settings.Default.setDisplay_camSmooth;
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";

            if (!mf.IsOnScreen(Location, Size, 1))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void bntOK_Click(object sender, EventArgs e)
        {
            if (daySet != mf.isDay) mf.SwapDayNightMode();
            Properties.Settings.Default.setDisplay_camSmooth = hsbarSmooth.Value;

            mf.camera.camSmoothFactor = ((double)(hsbarSmooth.Value) * 0.004) + 0.15;

            Settings.Default.Save();
            Close();
        }

        private void btnVehicleColor_Click(object sender, EventArgs e)
        {
            using (FormColorPicker form = new FormColorPicker(mf, mf.vehicleColor))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.vehicleColor = form.useThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorVehicle = mf.vehicleColor;
            Settings.Default.Save();
        }

        private void btnFrameDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.frameDayColor))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.frameDayColor = form.useThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorDayFrame = mf.frameDayColor;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFrameNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.frameNightColor))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.frameNightColor = form.useThisColor;
                }
            }

            Properties.Settings.Default.setDisplay_colorNightFrame = mf.frameNightColor;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFieldDay_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.fieldColorDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.fieldColorDay = form.useThisColor;
                }
            }

            Settings.Default.setDisplay_colorFieldDay = mf.fieldColorDay;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnFieldNight_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.fieldColorNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.fieldColorNight = form.useThisColor;
                }
            }

            Settings.Default.setDisplay_colorFieldNight = mf.fieldColorNight;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnSwap_Click(object sender, EventArgs e)
        {
            mf.SwapDayNightMode();
        }

        private void btnNightText_Click(object sender, EventArgs e)
        {
            if (mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.textColorNight))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.textColorNight = form.useThisColor;
                }
            }

            Settings.Default.setDisplay_colorTextNight = mf.textColorNight;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void btnDayText_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();

            using (FormColorPicker form = new FormColorPicker(mf, mf.textColorDay))
            {
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    mf.textColorDay = form.useThisColor;
                }
            }

            Settings.Default.setDisplay_colorTextDay = mf.textColorDay;
            Settings.Default.Save();

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }

        private void hsbarSmooth_ValueChanged(object sender, EventArgs e)
        {
            lblSmoothCam.Text = hsbarSmooth.Value.ToString() + "%";
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            if (!mf.isDay) mf.SwapDayNightMode();
            mf.frameDayColor = Color.FromArgb(210, 210, 230);
            mf.BackColor = mf.frameDayColor;
            mf.frameNightColor = Color.FromArgb(50, 50, 65);
            mf.sectionColorDay = Color.FromArgb(27, 151, 160);
            mf.fieldColorDay = Color.FromArgb(100, 100, 125);
            mf.fieldColorNight = Color.FromArgb(60, 60, 60);

            Properties.Settings.Default.setDisplay_colorDayFrame =   mf.frameDayColor;
            Properties.Settings.Default.setDisplay_colorNightFrame = mf.frameNightColor;
            Properties.Settings.Default.setDisplay_colorSectionsDay =mf.sectionColorDay;
            Properties.Settings.Default.setDisplay_colorFieldDay =   mf.fieldColorDay;
            Properties.Settings.Default.setDisplay_colorFieldNight = mf.fieldColorNight;

            mf.textColorNight = Color.FromArgb(230, 230, 230);
            mf.textColorDay = Color.FromArgb(10, 10, 20);
 
            Settings.Default.setDisplay_colorTextDay = mf.textColorDay;
            Settings.Default.setDisplay_colorTextNight = mf.textColorNight;

            Settings.Default.setDisplay_customColors = "-62208,-12299010,-16190712,-1505559,-3621034,-16712458,-7330570,-1546731,-24406,-3289866,-2756674,-538377,-134768,-4457734,-1848839,-530985";

            Properties.Settings.Default.Save();

            string[] words = Properties.Settings.Default.setDisplay_customColors.Split(',');

            for (int i = 0; i < 16; i++)
            {
                Color test;
                mf.customColorsList[i] = int.Parse(words[i], CultureInfo.InvariantCulture);
                test = Color.FromArgb(mf.customColorsList[i]).CheckColorFor255();
                int iCol = (test.A << 24) | (test.R << 16) | (test.G << 8) | test.B;
                mf.customColorsList[i] = iCol;
            }

            mf.SwapDayNightMode();
            mf.SwapDayNightMode();
        }
    }
}
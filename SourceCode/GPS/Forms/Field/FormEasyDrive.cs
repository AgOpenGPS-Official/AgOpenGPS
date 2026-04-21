using System;
using System.Windows.Forms;
using AgOpenGPS.Controls;

namespace AgOpenGPS
{
    public partial class FormEasyDrive : Form
    {
        private readonly FormGPS mf;

        public FormEasyDrive(FormGPS callingForm)
        {
            mf = callingForm;
            InitializeComponent();
        }

        private void FormEasyDrive_Load(object sender, EventArgs e)
        {
            if (mf.isMetric)
            {
                lblUnitWidth.Text = "m";
                lblUnitPivot.Text = "m";
                nudWidth.DecimalPlaces = 1;
                nudWidth.Minimum = 0.5m;
                nudWidth.Maximum = 100m;
                nudWidth.Value = 6.0m;
                nudPivotDistance.DecimalPlaces = 2;
                nudPivotDistance.Minimum = 0m;
                nudPivotDistance.Maximum = 20m;
                nudPivotDistance.Value = 1.00m;
            }
            else
            {
                lblUnitWidth.Text = "ft";
                lblUnitPivot.Text = "ft";
                nudWidth.DecimalPlaces = 1;
                nudWidth.Minimum = 1m;
                nudWidth.Maximum = 330m;
                nudWidth.Value = 20.0m;
                nudPivotDistance.DecimalPlaces = 1;
                nudPivotDistance.Minimum = 0m;
                nudPivotDistance.Maximum = 66m;
                nudPivotDistance.Value = 3.3m;
            }

            nudWidth.Controls[0].Enabled = false;
            nudPivotDistance.Controls[0].Enabled = false;
        }

        private void nudWidth_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void nudPivotDistance_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            double width;
            double pivotDistance;

            if (mf.isMetric)
            {
                width = (double)nudWidth.Value;
                pivotDistance = (double)nudPivotDistance.Value;
            }
            else
            {
                // Convert feet to meters for internal use
                width = (double)nudWidth.Value * 0.3048;
                pivotDistance = (double)nudPivotDistance.Value * 0.3048;
            }

            // Configure tool in memory (rigid, 1 section)
            mf.tool.isSectionsNotZones = true;
            mf.tool.numOfSections = 1;
            mf.tool.isToolRearFixed = true;
            mf.tool.isToolTrailing = false;
            mf.tool.isToolTBT = false;
            mf.tool.isToolFrontFixed = false;
            mf.tool.hitchLength = pivotDistance;
            mf.tool.trailingHitchLength = 0;
            mf.tool.tankTrailingHitchLength = 0;
            mf.tool.trailingToolToPivotLength = 0;
            mf.tool.offset = 0;
            mf.tool.overlap = 0;

            // Set single section spanning full width
            double halfWidth = width / 2.0;
            mf.section[0].positionLeft = -halfWidth;
            mf.section[0].positionRight = halfWidth;
            mf.SectionCalcWidths();

            // Define local plane at current GPS position
            mf.pn.DefineLocalPlane(mf.AppModel.CurrentLatLon, false);

            // Set up a temporary field (no directory on disk)
            mf.currentFieldDirectory = "Easy Drive";
            mf.JobNew();

            mf.isEasyDriveMode = true;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}

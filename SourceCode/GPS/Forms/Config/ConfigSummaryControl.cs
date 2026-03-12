using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;

namespace AgOpenGPS.Forms.Config
{
    public partial class ConfigSummaryControl : UserControl
    {
        public ConfigSummaryControl()
        {
            InitializeComponent();

            labelProfileMenuHint.Text = gStr.gsProfileMenuHint;
            labelUnits.Text = gStr.gsUnits + ":";
            labelWidth.Text = gStr.gsWidth + ":";
            labelSections.Text = gStr.gsSections + ":";
            labelOffset.Text = gStr.gsOffset + ":";
            labelOverlap.Text = gStr.gsOverlap + ":";
            labelLookAhead.Text = gStr.gsLookAhead + ":";
            labelNudge.Text = gStr.gsNudge + ":";
            labelTramW.Text = gStr.gsTramWidth + ":";
            labelWheelBase.Text = gStr.gsWheelbase + ":";
            labelVehicleType.Text = gStr.gsVehiclegroupbox + ":";
            labelAntPivot.Text = gStr.gsPivot + ":";
            labelAntOffset.Text = gStr.gsAntennaOffset + ":";
            labelHitch.Text = gStr.gsHitchLength + ":";
        }

        public void UpdateSummary(FormGPS mf)
        {
            var vs = Properties.VehicleSettings.Default;
            var ts = Properties.ToolSettings.Default;

            // Vehicle panel
            lblSummaryVehicleName.Text = RegistrySettings.vehicleFileName;
            lblSumVehicleType.Text = vs.setVehicle_vehicleType == 0 ? "Tractor"
                : vs.setVehicle_vehicleType == 1 ? "Harvester" : "Articulated";
            lblSumWheelbase.Text = Distance.SmallDistanceString(mf.isMetric, vs.setVehicle_wheelbase);
            lblAntPivot.Text = Distance.SmallDistanceString(mf.isMetric, vs.setVehicle_antennaPivot);
            lblAntOffset.Text = Distance.SmallDistanceString(mf.isMetric, vs.setVehicle_antennaOffset);
            lblHitch.Text = Distance.SmallDistanceString(mf.isMetric, vs.setVehicle_hitchLength);

            // Tool panel
            lblSummaryToolName.Text = RegistrySettings.toolFileName;
            lblSumNumSections.Text = mf.tool.numOfSections.ToString();
            lblToolOffset.Text = Distance.SmallDistanceString(mf.isMetric, ts.setVehicle_toolOffset);
            lblOverlap.Text = Distance.SmallDistanceString(mf.isMetric, ts.setVehicle_toolOverlap);
            lblLookahead.Text = ts.setVehicle_toolLookAheadOn.ToString() + " sec";
            lblNudgeDistance.Text = Distance.VerySmallDistanceString(mf.isMetric, 0.01 * Properties.Settings.Default.setAS_snapDistance);
            lblTramWidth.Text = Distance.MediumDistanceString(mf.isMetric, Properties.Settings.Default.setTram_tramWidth);

            // Buiten panels
            lblUnits.Text = mf.isMetric ? "Metric" : "Imperial";
        }

        public void SetSummaryWidth(string widthText)
        {
            lblSummaryWidth.Text = widthText;
        }

    }
}

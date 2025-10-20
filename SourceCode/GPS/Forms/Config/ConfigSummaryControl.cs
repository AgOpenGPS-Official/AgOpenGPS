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

            labelUnits.Text = gStr.gsUnit;
            labelWidth.Text = gStr.gsWidth;
            labelSections.Text = gStr.gsSections;
            labelOffset.Text = gStr.gsOffset;
            labelOverlap.Text = gStr.gsOverlap;
            labelLookAhead.Text = gStr.gsLookAhead;
            labelNudge.Text = gStr.gsNudge;
            labelTramW.Text = gStr.gsTramWidth;
            labelWheelBase.Text = gStr.gsWheelbase;
        }

        public void UpdateSummary(FormGPS mf)
        {
            lblSumWheelbase.Text = Distance.SmallDistanceString(mf.isMetric, Properties.Settings.Default.setVehicle_wheelbase);

            lblSumNumSections.Text = mf.tool.numOfSections.ToString();

            lblNudgeDistance.Text = Distance.VerySmallDistanceString(mf.isMetric, 0.01 * Properties.Settings.Default.setAS_snapDistance);
            lblUnits.Text = mf.isMetric ? "Metric" : "Imperial";

            lblSummaryVehicleName.Text = gStr.gsCurrent + ": " + RegistrySettings.vehicleFileName;

            lblTramWidth.Text = Distance.MediumDistanceString(mf.isMetric, Properties.Settings.Default.setTram_tramWidth);

            lblToolOffset.Text = Distance.SmallDistanceString(mf.isMetric, Properties.Settings.Default.setVehicle_toolOffset);
            lblOverlap.Text = Distance.SmallDistanceString(mf.isMetric, Properties.Settings.Default.setVehicle_toolOverlap);

            lblLookahead.Text = Properties.Settings.Default.setVehicle_toolLookAheadOn.ToString() + " sec";
        }

        public void SetSummaryWidth(string widthText)
        {
            lblSummaryWidth.Text = widthText;
        }

    }
}

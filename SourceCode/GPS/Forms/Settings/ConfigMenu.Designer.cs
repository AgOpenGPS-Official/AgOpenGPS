using AgOpenGPS.Core.Translations;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormConfig
    {
        private void HideSubMenu()
        {
            panelVehicleSubMenu.Visible = false;
            panelToolSubMenu.Visible = false;
            panelDataSourcesSubMenu.Visible = false;
            panelArduinoSubMenu.Visible = false;
        }

        private void ShowSubMenu(Panel subMenu, Button btn)
        {
            ClearVehicleSubBackgrounds();
            ClearToolSubBackgrounds();
            ClearMachineSubBackgrounds();
            ClearDataSubBackgrounds();
            ClearNoSubBackgrounds();

            if (subMenu.Visible == false)
            {
                HideSubMenu();
                subMenu.Visible = true;
                if (subMenu.Name == "panelVehicleSubMenu")
                {
                    tab1.SelectedTab = tabVConfig;
                }
                else if (subMenu.Name == "panelToolSubMenu")
                {
                    tab1.SelectedTab = tabTConfig;
                }
                else if (subMenu.Name == "panelDataSourcesSubMenu")
                {
                    tab1.SelectedTab = tabDHeading;
                }
                else if (subMenu.Name == "panelArduinoSubMenu")
                {
                    tab1.SelectedTab = tabAMachine;
                }
                else if (btn.Name == "btnUTurn") tab1.SelectedTab = tabUTurn;
                else if (btn.Name == "btnFeatureHides") tab1.SelectedTab = tabBtns;
                else if (btn.Name == "btnDisplay") tab1.SelectedTab = tabDisplay;
            }
            else
            {
                tab1.SelectedTab = tabSummary;
                subMenu.Visible = false;
            }
        }

        private void UpdateSummary()
        {
            lblSumWheelbase.Text = (mf.isMetric ?
                (Properties.Settings.Default.setVehicle_wheelbase * mf.m2InchOrCm).ToString("N0") :
                (Properties.Settings.Default.setVehicle_wheelbase * mf.m2InchOrCm).ToString("N0"))
                + mf.unitsInCm;

            lblSumNumSections.Text = mf.tool.numOfSections.ToString();

            string snapDist = mf.isMetric ?
                Properties.Settings.Default.setAS_snapDistance.ToString() :
                (Properties.Settings.Default.setAS_snapDistance * mf.cm2CmOrIn).ToString("N1");

            lblNudgeDistance.Text = snapDist + mf.unitsInCm.ToString();
            lblUnits.Text = mf.isMetric ? "Metric" : "Imperial";

            labelCurrentVehicle.Text = gStr.gsCurrent + ": " + RegistrySettings.vehicleFileName;
            lblSummaryVehicleName.Text = labelCurrentVehicle.Text;

            lblTramWidth.Text = mf.isMetric ?
                ((Properties.Settings.Default.setTram_tramWidth).ToString() + " m") :
                ConvertMeterToFeet(Properties.Settings.Default.setTram_tramWidth);

            lblToolOffset.Text = (mf.isMetric ?
                (Properties.Settings.Default.setVehicle_toolOffset * mf.m2InchOrCm).ToString() :
                (Properties.Settings.Default.setVehicle_toolOffset * mf.m2InchOrCm).ToString("N1")) +
                mf.unitsInCm;

            lblOverlap.Text = (mf.isMetric ?
                (Properties.Settings.Default.setVehicle_toolOverlap * mf.m2InchOrCm).ToString() :
                (Properties.Settings.Default.setVehicle_toolOverlap * mf.m2InchOrCm).ToString("N1")) +
                mf.unitsInCm;

            lblLookahead.Text = Properties.Settings.Default.setVehicle_toolLookAheadOn.ToString() + " sec";
        }

        public string ConvertMeterToFeet(double meter)
        {
            double toFeet = meter * 3.28;
            string feetInch = Convert.ToString((int)toFeet) + "' ";
            double temp = Math.Round((toFeet - Math.Truncate(toFeet)) * 12, 0);
            feetInch += Convert.ToString(temp) + '"';
            return feetInch;
        }

        #region No Sub menu Buttons

        private void ClearNoSubBackgrounds()
        {
            btnTram.BackColor = SystemColors.GradientInactiveCaption;
            btnUTurn.BackColor = SystemColors.GradientInactiveCaption;
            btnDisplay.BackColor = SystemColors.GradientInactiveCaption;
            btnFeatureHides.BackColor = SystemColors.GradientInactiveCaption;
        }
        private void btnTram_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ClearNoSubBackgrounds();
            if (tab1.SelectedTab == tabTram)
            {
                tab1.SelectedTab = tabSummary;
            }
            else
            {
                tab1.SelectedTab = tabTram;
                btnTram.BackColor = SystemColors.GradientActiveCaption;
            }
        }

        private void btnUTurn_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ClearNoSubBackgrounds();
            if (tab1.SelectedTab == tabUTurn)
            {
                tab1.SelectedTab = tabSummary;
            }
            else
            {
                tab1.SelectedTab = tabUTurn;
                btnUTurn.BackColor = SystemColors.GradientActiveCaption;
            }
        }

        private void btnFeatureHides_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ClearNoSubBackgrounds();
            if (tab1.SelectedTab == tabBtns)
            {
                tab1.SelectedTab = tabSummary;
            }
            else
            {
                tab1.SelectedTab = tabBtns;
                btnFeatureHides.BackColor = SystemColors.GradientActiveCaption;
            }
        }

        private void btnDisplay_Click(object sender, EventArgs e)
        {
            HideSubMenu();
            ClearNoSubBackgrounds();
            if (tab1.SelectedTab == tabDisplay)
            {
                tab1.SelectedTab = tabSummary;
            }
            else
            {
                tab1.SelectedTab = tabDisplay;
                btnDisplay.BackColor = SystemColors.GradientActiveCaption;
            }
        }

        #endregion

        #region Vehicle Sub Menu Btns
        private void btnVehicle_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelVehicleSubMenu, btnVehicle);
            btnSubVehicleType.BackColor = SystemColors.GradientActiveCaption;
            UpdateSummary();
        }

        private void ClearVehicleSubBackgrounds()
        {
            btnSubVehicleType.BackColor = SystemColors.GradientInactiveCaption;
            btnSubAntenna.BackColor = SystemColors.GradientInactiveCaption;
            btnSubDimensions.BackColor = SystemColors.GradientInactiveCaption;
            //btnSubGuidance.BackColor = SystemColors.GradientInactiveCaption;
        }
        private void btnSubVehicleType_Click(object sender, EventArgs e)
        {
            ClearVehicleSubBackgrounds();
            tab1.SelectedTab = tabVConfig;
            btnSubVehicleType.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubDimensions_Click(object sender, EventArgs e)
        {
            ClearVehicleSubBackgrounds();
            tab1.SelectedTab = tabVDimensions;
            btnSubDimensions.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubAntenna_Click(object sender, EventArgs e)
        {
            ClearVehicleSubBackgrounds();
            tab1.SelectedTab = tabVAntenna;
            btnSubAntenna.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubGuidance_Click(object sender, EventArgs e)
        {
            ClearVehicleSubBackgrounds();
            tab1.SelectedTab = tabVGuidance;
            //btnSubGuidance.BackColor = SystemColors.GradientActiveCaption;               
        }

        #endregion Region

        #region Tool Sub Menu
        private void btnTool_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelToolSubMenu, btnTool);
            btnSubToolType.BackColor = SystemColors.GradientActiveCaption;
        }

        private void ClearToolSubBackgrounds()
        {
            btnSubToolType.BackColor = SystemColors.GradientInactiveCaption;
            btnSubHitch.BackColor = SystemColors.GradientInactiveCaption;
            btnSubSections.BackColor = SystemColors.GradientInactiveCaption;
            btnSubSwitches.BackColor = SystemColors.GradientInactiveCaption;
            btnSubToolSettings.BackColor = SystemColors.GradientInactiveCaption;
            btnSubToolOffset.BackColor = SystemColors.GradientInactiveCaption;
            btnSubPivot.BackColor = SystemColors.GradientInactiveCaption;
        }

        private void btnSubToolType_Click(object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabTConfig;
            btnSubToolType.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubHitch_Click(object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabTHitch;
            btnSubHitch.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubToolOffset_Click
            (object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabToolOffset;
            btnSubToolOffset.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubPivot_Click(object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabToolPivot;
            btnSubPivot.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubSections_Click(object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabTSections;
            btnSubSections.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubSwitches_Click(object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabTSwitches;
            btnSubSwitches.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubToolSettings_Click(object sender, EventArgs e)
        {
            ClearToolSubBackgrounds();
            tab1.SelectedTab = tabTSettings;
            btnSubToolSettings.BackColor = SystemColors.GradientActiveCaption;
        }
        #endregion

        #region SubMenu Data Sources

        private void ClearDataSubBackgrounds()
        {
            btnSubHeading.BackColor = SystemColors.GradientInactiveCaption;
            btnSubRoll.BackColor = SystemColors.GradientInactiveCaption;
        }
        private void btnDataSources_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelDataSourcesSubMenu, btnDataSources);
            btnSubHeading.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubHeading_Click(object sender, EventArgs e)
        {
            ClearDataSubBackgrounds();
            tab1.SelectedTab = tabDHeading;
            btnSubHeading.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnSubRoll_Click(object sender, EventArgs e)
        {
            ClearDataSubBackgrounds();
            tab1.SelectedTab = tabDRoll;
            btnSubRoll.BackColor = SystemColors.GradientActiveCaption;
        }

        #endregion

        #region Module
        private void ClearMachineSubBackgrounds()
        {
            btnMachineModule.BackColor = SystemColors.GradientInactiveCaption;
            btnMachineRelay.BackColor = SystemColors.GradientInactiveCaption;
        }

        private void btnArduino_Click(object sender, EventArgs e)
        {
            ShowSubMenu(panelArduinoSubMenu, btnArduino);
            btnMachineModule.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnMachineModule_Click(object sender, EventArgs e)
        {
            ClearMachineSubBackgrounds();
            tab1.SelectedTab = tabAMachine;
            btnMachineModule.BackColor = SystemColors.GradientActiveCaption;
        }

        private void btnMachineRelay_Click(object sender, EventArgs e)
        {
            ClearMachineSubBackgrounds();
            tab1.SelectedTab = tabRelay;
            btnMachineRelay.BackColor = SystemColors.GradientActiveCaption;
        }
        #endregion
    }
}

using AgLibrary.Logging;
using AgOpenGPS.Controls;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Forms;
using AgOpenGPS.Helpers;
using AgOpenGPS.Properties;
using System;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSteer : Form
    {
        private readonly FormGPS mf = null;

        private bool toSend = false, isSA = false;
        private int counter = 0, secondCntr = 0, cntr;
        private vec3 startFix;
        private double diameter, steerAngleRight, dist;
        private int windowSizeState = 0;

        //Form stuff
        public FormSteer(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
            nudMaxCounts.Controls[0].Enabled = false;

            nudSnapDistance.Controls[0].Enabled = false;
            nudLineWidth.Controls[0].Enabled = false;
            nudGuidanceLookAhead.Controls[0].Enabled = false;
            nudGuidanceSpeedLimit.Controls[0].Enabled = false;
            nudMaxSteerSpeed.Controls[0].Enabled = false;
            nudMinSteerSpeed.Controls[0].Enabled = false;

            nudSnapDistance.Maximum = Math.Round(nudSnapDistance.Maximum / 2.54M);
            nudSnapDistance.Minimum = Math.Round(nudSnapDistance.Minimum / 2.54M);

            nudSnapDistance.Minimum = Math.Round(nudSnapDistance.Minimum / 2.54M);
            nudSnapDistance.Maximum = Math.Round(nudSnapDistance.Maximum / 2.54M);

            //translate
            this.Text = gStr.gsAutoSteerConfiguration;
            labelFast.Text = gStr.gsFast;
            labelSteerResponse.Text = gStr.gsSteerResponse;
            labelSlow.Text = gStr.gsSlow;
            labelIntegralPP.Text = gStr.gsIntegral;
            labelIntegralInfo.Text = gStr.gsIntegralInfo;
            labelSteerAngle.Text = gStr.gsSteerAngle;
            labelDiameter.Text = gStr.gsDiameter;
            labelDistance.Text = gStr.gsAgressiveness;
            labelHeading.Text = gStr.gsOvershootReduction;
            labelIntergralStanley.Text = gStr.gsIntegral;
            labelProportionalGain.Text = gStr.gsProportionalGain;
            labelMaxLimit.Text = gStr.gsMaxLimit;
            labelMinToMove.Text = gStr.gsMinToMove;
            labelWasZero.Text = gStr.gsWasZero;
            labelCountsPerDegree.Text = gStr.gsCountsPerDegree;
            labelAckermann.Text = gStr.gsAckermann;
            labelMaxSteerAngle.Text = gStr.gsMaxSteerAngle;
            labelDeadzone.Text = gStr.gsDeadzone;
            labelHeadingDegree.Text = gStr.gsHeading;
            labelOnDelay.Text = gStr.gsOnDelay;
            labelSpeedFactor.Text = gStr.gsSpeedFactor;
            labelAcquireFactor.Text = gStr.gsAcquireFactor;
            labelAcquireDescription.Text = $"{gStr.gsAcquire} = {gStr.gsFactor} * {gStr.gsHold}";
            labelDist.Text = gStr.gsDistance;
            labelAcquire2.Text = gStr.gsAcquire;
            labelHold.Text = gStr.gsHold;

            //translate pop-out
            labelEncoder.Text = gStr.gsTurnSensor;
            labelTurnSensor.Text = gStr.gsTurnSensor;
            labelPressureTurnSensor.Text = gStr.gsPressureTurnSensor;
            labelCurrentTurnSensor.Text = gStr.gsCurrentTurnSensor;
            labelInvertWas.Text = gStr.gsInvertWas;
            labelInvertMotor.Text = gStr.gsInvertMotor;
            labelInvertRelays.Text = gStr.gsInvertRelays;
            labelMotorDriver.Text = gStr.gsMotorDriver;
            labelADConverter.Text = gStr.gsADConverter;
            labelIMUAxis.Text = gStr.gsIMUAxis;
            labelSteerEnable.Text = gStr.gsSteerEnable;
            labelSteerDescription.Text = $"{gStr.gsButton} - {gStr.gsButtonDescription}\n{gStr.gsSwitch} - {gStr.gsSwitchDescription}";
            labelUturnCompensation.Text = gStr.gsUturnCompensation;
            labelSideHill.Text = gStr.gsSideHillComp;
            labelSteerInReverse.Text = gStr.gsSteerInReverse;
            labelManualTurns.Text = gStr.gsManualTurns;
            labelMinSpeed.Text = gStr.gsMinSpeed;
            labelMaxSpeed.Text = gStr.gsMaxSpeed;
            labelLineWidth.Text = gStr.gsLineWidth;
            labelNudgeDistance.Text = gStr.gsNudgeDistance;
            labelNextGuidanceLine.Text = gStr.gsNextGuidanceLine;
            labelCmPix.Text = $"{gStr.gsCm} -> {gStr.gsPixel}";
            labelOnOff.Text = $"{gStr.gsOn}/{gStr.gsOff}";
            labelLightbar.Text = gStr.gsLightbar;
            labelSteerBar.Text = gStr.gsSteerBar;
            labelWizard.Text = gStr.gsWizard;
            labelReset.Text = gStr.gsReset;
            labelSendAndSave.Text = gStr.gsSendAndSave;

            this.Width = 388;
            this.Height = 490;
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadHold = Properties.ToolSettings.Default.setVehicle_goalPointLookAheadHold;
            cboxSteerInReverse.Checked = Properties.VehicleSettings.Default.setAS_isSteerInReverse;

            if (mf.isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
            }

            if (mf.isStanleyUsed)
            {
                tabControl1.TabPages.Remove(tabPP);
                tabControl1.TabPages.Remove(tabPPAdv);
                this.tabControl1.ItemSize = new System.Drawing.Size(105, 48);

            }
            else
            {
                tabControl1.TabPages.Remove(tabStan);
                this.tabControl1.ItemSize = new System.Drawing.Size(89, 48);
            }

            Location = Properties.Settings.Default.setWindow_steerSettingsLocation;
            //WAS Zero, CPD
            hsbarWasOffset.ValueChanged -= hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged -= hsbarCountsPerDegree_ValueChanged;

            hsbarWasOffset.Value = Properties.VehicleSettings.Default.setAS_wasOffset;
            hsbarCountsPerDegree.Value = Properties.VehicleSettings.Default.setAS_countsPerDegree;

            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");

            hsbarWasOffset.ValueChanged += hsbarSteerAngleSensorZero_ValueChanged;
            hsbarCountsPerDegree.ValueChanged += hsbarCountsPerDegree_ValueChanged;

            hsbarAckerman.ValueChanged -= hsbarAckerman_ValueChanged;
            hsbarAckerman.Value = Properties.VehicleSettings.Default.setAS_ackerman;
            lblAckerman.Text = hsbarAckerman.Value.ToString();
            hsbarAckerman.ValueChanged += hsbarAckerman_ValueChanged;

            //min pwm, kP
            hsbarMinPWM.ValueChanged -= hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged -= hsbarProportionalGain_ValueChanged;

            hsbarMinPWM.Value = Properties.VehicleSettings.Default.setAS_minSteerPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Properties.VehicleSettings.Default.setAS_Kp;
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            hsbarMinPWM.ValueChanged += hsbarMinPWM_ValueChanged;
            hsbarProportionalGain.ValueChanged += hsbarProportionalGain_ValueChanged;

            //low steer, high steer
            //hsbarLowSteerPWM.ValueChanged -= hsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged -= hsbarHighSteerPWM_ValueChanged;

            //hsbarLowSteerPWM.Value = Properties.VehicleSettings.Default.setAS_lowSteerPWM;
            //lblLowSteerPWM.Text = hsbarLowSteerPWM.Value.ToString();

            hsbarHighSteerPWM.Value = Properties.VehicleSettings.Default.setAS_highSteerPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            //hsbarLowSteerPWM.ValueChanged += hsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged += hsbarHighSteerPWM_ValueChanged;

            hsbarMaxSteerAngle.Value = (Int16)Properties.VehicleSettings.Default.setVehicle_maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            mf.vehicle.stanleyDistanceErrorGain = Properties.VehicleSettings.Default.stanleyDistanceErrorGain;
            hsbarStanleyGain.Value = (Int16)(mf.vehicle.stanleyDistanceErrorGain * 10);
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();

            mf.vehicle.stanleyHeadingErrorGain = Properties.VehicleSettings.Default.stanleyHeadingErrorGain;
            hsbarHeadingErrorGain.Value = (Int16)(mf.vehicle.stanleyHeadingErrorGain * 10);
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();

            mf.vehicle.stanleyIntegralGainAB = Properties.VehicleSettings.Default.stanleyIntegralGainAB;
            hsbarIntegral.Value = (int)(Properties.VehicleSettings.Default.stanleyIntegralGainAB * 100);
            lblIntegralPercent.Text = ((int)(mf.vehicle.stanleyIntegralGainAB * 100)).ToString();

            mf.vehicle.purePursuitIntegralGain = Properties.VehicleSettings.Default.purePursuitIntegralGainAB;
            hsbarIntegralPurePursuit.Value = (int)(Properties.VehicleSettings.Default.purePursuitIntegralGainAB * 100);
            lblPureIntegral.Text = ((int)(mf.vehicle.purePursuitIntegralGain * 100)).ToString();

            mf.gyd.sideHillCompFactor = Properties.VehicleSettings.Default.setAS_sideHillComp;
            hsbarSideHillComp.Value = (int)(Properties.VehicleSettings.Default.setAS_sideHillComp * 100);

            mf.vehicle.goalPointLookAheadHold = Properties.ToolSettings.Default.setVehicle_goalPointLookAheadHold;
            hsbarHoldLookAhead.Value = (Int16)(mf.vehicle.goalPointLookAheadHold * 10);
            lblHoldLookAhead.Text = mf.vehicle.goalPointLookAheadHold.ToString();

            hsbarLookAheadMult.Value = (Int16)(Properties.ToolSettings.Default.setVehicle_goalPointLookAheadMult * 10);
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();

            hsbarAcquireFactor.Value = (int)(Properties.ToolSettings.Default.setVehicle_goalPointAcquireFactor * 100);
            lblAcquireFactor.Text = mf.vehicle.goalPointAcquireFactor.ToString();

            lblAcquirePP.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");

            hsbarUTurnCompensation.Value = (Int16)(Properties.Settings.Default.setAS_uTurnCompensation * 10);
            lblUTurnCompensation.Text = (hsbarUTurnCompensation.Value - 10).ToString();

            //make sure free drive is off
            btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
            mf.vehicle.isInFreeDriveMode = false;
            btnSteerAngleDown.Enabled = false;
            btnSteerAngleUp.Enabled = false;
            mf.vehicle.driveFreeSteerAngle = 0;

            //nudDeadZoneDistance.Value = (decimal)((double)(Properties.VehicleSettings.Default.setAS_deadZoneDistance)/10);
            nudDeadZoneHeading.Value = (decimal)((double)(Properties.VehicleSettings.Default.setAS_deadZoneHeading) / 100);
            nudDeadZoneDelay.Value = (decimal)(mf.vehicle.deadZoneDelay);

            toSend = false;

            int sett = Properties.VehicleSettings.Default.setArdSteer_setting0;

            if ((sett & 1) == 0) chkInvertWAS.Checked = false;
            else chkInvertWAS.Checked = true;

            if ((sett & 2) == 0) chkSteerInvertRelays.Checked = false;
            else chkSteerInvertRelays.Checked = true;

            if ((sett & 4) == 0) chkInvertSteer.Checked = false;
            else chkInvertSteer.Checked = true;

            if ((sett & 8) == 0) cboxConv.Text = "Differential";
            else cboxConv.Text = "Single";

            if ((sett & 16) == 0) cboxMotorDrive.Text = "IBT2";
            else cboxMotorDrive.Text = "Cytron";

            if ((sett & 32) == 32) cboxSteerEnable.Text = "Switch";
            else if ((sett & 64) == 64) cboxSteerEnable.Text = "Button";
            else cboxSteerEnable.Text = "None";

            if ((sett & 128) == 0) cboxEncoder.Checked = false;
            else cboxEncoder.Checked = true;

            nudMaxCounts.Value = (decimal)Properties.VehicleSettings.Default.setArdSteer_maxPulseCounts;
            hsbarSensor.Value = (int)Properties.VehicleSettings.Default.setArdSteer_maxPulseCounts;
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";

            sett = Properties.VehicleSettings.Default.setArdSteer_setting1;

            if ((sett & 1) == 0) cboxDanfoss.Checked = false;
            else cboxDanfoss.Checked = true;

            if ((sett & 8) == 0) cboxXY.Text = "X";
            else cboxXY.Text = "Y";

            if ((sett & 2) == 0) cboxPressureSensor.Checked = false;
            else cboxPressureSensor.Checked = true;

            if ((sett & 4) == 0) cboxCurrentSensor.Checked = false;
            else cboxCurrentSensor.Checked = true;

            if (cboxEncoder.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                labelTurnSensor.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = true;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
                labelTurnSensor.Text = gStr.gsEncoderCounts;
            }
            else if (cboxPressureSensor.Checked)
            {
                cboxEncoder.Checked = false;
                cboxCurrentSensor.Checked = false;
                labelTurnSensor.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                labelTurnSensor.Text = "Off at %";
            }
            else if (cboxCurrentSensor.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxEncoder.Checked = false;
                labelTurnSensor.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                labelTurnSensor.Text = "Off at %";
            }
            else
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                cboxEncoder.Checked = false;
                labelTurnSensor.Visible = false;
                lblPercentFS.Visible = false;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
            }

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }

            if (mf.isLightBarNotSteerBar) rbtnLightBar.Checked = true;
            else rbtnSteerBar.Checked = true;

            // Start Smart WAS Calibration data collection
            mf.smartWAS.Reset();
            mf.smartWAS.Start();
        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Stop Smart WAS Calibration data collection
            mf.smartWAS.Stop();

            mf.vehicle.isInFreeDriveMode = false;

            Properties.ToolSettings.Default.setVehicle_goalPointLookAheadHold = mf.vehicle.goalPointLookAheadHold;
            Properties.ToolSettings.Default.setVehicle_goalPointLookAheadMult = mf.vehicle.goalPointLookAheadMult;
            Properties.ToolSettings.Default.setVehicle_goalPointAcquireFactor = mf.vehicle.goalPointAcquireFactor;

            Properties.VehicleSettings.Default.stanleyHeadingErrorGain = mf.vehicle.stanleyHeadingErrorGain;
            Properties.VehicleSettings.Default.stanleyDistanceErrorGain = mf.vehicle.stanleyDistanceErrorGain;
            Properties.VehicleSettings.Default.stanleyIntegralGainAB = mf.vehicle.stanleyIntegralGainAB;
            Properties.VehicleSettings.Default.purePursuitIntegralGainAB = mf.vehicle.purePursuitIntegralGain;
            Properties.VehicleSettings.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle;

            Properties.VehicleSettings.Default.setAS_countsPerDegree = mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
            Properties.VehicleSettings.Default.setAS_ackerman = mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

            Properties.VehicleSettings.Default.setAS_wasOffset = hsbarWasOffset.Value;
            mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
            mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

            Properties.VehicleSettings.Default.setAS_highSteerPWM = mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
            Properties.VehicleSettings.Default.setAS_lowSteerPWM = mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)(hsbarHighSteerPWM.Value / 3));
            Properties.VehicleSettings.Default.setAS_Kp = mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
            Properties.VehicleSettings.Default.setAS_minSteerPWM = mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

            Properties.VehicleSettings.Default.setAS_deadZoneHeading = mf.vehicle.deadZoneHeading;
            Properties.VehicleSettings.Default.setAS_deadZoneDelay = mf.vehicle.deadZoneDelay;

            Properties.VehicleSettings.Default.setAS_ModeXTE = mf.vehicle.modeXTE;
            Properties.VehicleSettings.Default.setAS_ModeTime = mf.vehicle.modeTime;

            Properties.Settings.Default.setWindow_steerSettingsLocation = Location;

            Properties.Settings.Default.setAS_uTurnCompensation = mf.vehicle.uturnCompensation;

            //save current vehicle
            Properties.Settings.Default.Save();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (isSA)
            {
                //bool isSame = Math.PI - Math.Abs(Math.Abs(cH - mf.gpsHeading) - Math.PI) < (glm.PIBy2+0.1);
                dist = glm.Distance(startFix, mf.pivotAxlePos);
                cntr++;
                if (dist > diameter)
                {
                    diameter = dist;
                    cntr = 0;
                }
                lblDiameter.Text = diameter.ToString("N2") + " m";

                if (cntr > 9)
                {
                    steerAngleRight = Math.Atan(mf.vehicle.VehicleConfig.Wheelbase / ((diameter - mf.vehicle.VehicleConfig.TrackWidth * 0.5) / 2));
                    steerAngleRight = glm.toDegrees(steerAngleRight);
                    //steerAngleLeft = Math.Atan(mf.vehicle.wheelbase / (diameter / 2 ));
                    //steerAngleLeft = glm.toDegrees(steerAngleLeft);

                    lblCalcSteerAngleInner.Text = steerAngleRight.ToString("N1") + "\u00B0";
                    //lblCalcSteerAngleOuter.Text = steerAngleLeft.ToString("N1") + "\u00B0";
                    lblDiameter.Text = diameter.ToString("N2") + " m";
                    btnStartSA.Image = Properties.Resources.BoundaryRecord;
                    isSA = false;
                }
            }

            double actAng = mf.mc.actualSteerAngleDegrees * 5;
            if (actAng > 0)
            {
                if (actAng > 49) actAng = 49;
                CExtensionMethods.SetProgressNoAnimation(pbarRight, (int)actAng);
                pbarLeft.Value = 0;
            }
            else
            {
                if (actAng < -49) actAng = -49;
                pbarRight.Value = 0;
                CExtensionMethods.SetProgressNoAnimation(pbarLeft, (int)-actAng);
            }

            lblSteerAngle.Text = mf.SetSteerAngle;
            lblSteerAngleActual.Text = mf.mc.actualSteerAngleDegrees.ToString("N1") + "\u00B0";
            lblActualSteerAngleUpper.Text = lblSteerAngleActual.Text;
            double err = (mf.mc.actualSteerAngleDegrees - mf.guidanceLineSteerAngle * 0.01);
            lblError.Text = Math.Abs(err).ToString("N1") + "\u00B0";
            if (err > 0) lblError.ForeColor = Color.Red;
            else lblError.ForeColor = Color.DarkGreen;

            lblAV_Act.Text = mf.actAngVel.ToString("N1");
            lblAV_Set.Text = mf.setAngVel.ToString("N1");

            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();

            counter++;

            if (toSend && counter > 4)
            {
                mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
                mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

                mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
                mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

                mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
                mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)(hsbarHighSteerPWM.Value / 3));
                mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
                mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

                mf.SendPgnToLoop(mf.p_252.pgn);
                toSend = false;
                counter = 0;
            }

            if (secondCntr++ > 2)
            {
                secondCntr = 0;

                if (tabControl1.SelectedTab == tabPPAdv)
                {
                    lblHoldAdv.Text = mf.vehicle.goalPointLookAheadHold.ToString("N1");
                    lblAcqAdv.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");
                    lblDistanceAdv.Text = mf.vehicle.goalDistance.ToString("N1");
                    lblAcquirePP.Text = lblAcqAdv.Text;
                }
                //else if (tabControl1.SelectedTab == tabPP)
                //{
                //    lblHoldAdv.Text = mf.vehicle.goalPointLookAheadHold.ToString("N1");
                //    lblAcqAdv.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");
                //    lblDistanceAdv.Text = mf.vehicle.goalDistance.ToString("N1");
                //}
            }

            //if (hsbarMinPWM.Value > hsbarLowSteerPWM.Value) lblMinPWM.ForeColor = Color.OrangeRed;
            //else lblMinPWM.ForeColor = SystemColors.ControlText;

            if (mf.mc.sensorData != -1)
            {
                if (mf.mc.sensorData < 0 || mf.mc.sensorData > 255) mf.mc.sensorData = 0;
                CExtensionMethods.SetProgressNoAnimation(pbarSensor, mf.mc.sensorData);
                if (nudMaxCounts.Visible == false)
                    lblPercentFS.Text = ((int)((double)mf.mc.sensorData * 0.3921568627)).ToString() + "%";
                else
                    lblPercentFS.Text = mf.mc.sensorData.ToString();
            }

            // Update Smart WAS Calibration status
            UpdateSmartWASStatus();
        }

        #region Tab Sensors

        private void EnableAlert_Click(object sender, EventArgs e)
        {
            pboxSendSteer.Visible = true;
            btnClose.Enabled = false;

            if (sender is CheckBox checkbox)
            {
                if (checkbox.Name == "cboxEncoder" || checkbox.Name == "cboxPressureSensor"
                    || checkbox.Name == "cboxCurrentSensor")
                {
                    if (!checkbox.Checked)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        labelTurnSensor.Visible = false;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        return;
                    }

                    if (checkbox == cboxPressureSensor)
                    {
                        cboxEncoder.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        labelTurnSensor.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = true;
                        labelTurnSensor.Text = "Off at %";
                        hsbarSensor.Visible = true;
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxCurrentSensor)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        labelTurnSensor.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        hsbarSensor.Visible = true;
                        pbarSensor.Visible = true;
                        labelTurnSensor.Text = "Off at %";
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxEncoder)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        labelTurnSensor.Visible = true;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = true;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        labelTurnSensor.Text = gStr.gsEncoderCounts;
                    }
                }
            }
        }

        private void nudMaxCounts_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                pboxSendSteer.Visible = true;
                btnClose.Enabled = false;

            }
        }

        private void hsbarSensor_Scroll(object sender, ScrollEventArgs e)
        {
            pboxSendSteer.Visible = true;
            btnClose.Enabled = false;
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";
        }

        #endregion


        #region Tab Settings

        private void tabSettings_Enter(object sender, EventArgs e)
        {
            cboxSteerInReverse.Checked = Properties.VehicleSettings.Default.setAS_isSteerInReverse;

            if (mf.isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
            }
        }

        private void tabSettings_Leave(object sender, EventArgs e)
        {
            Properties.VehicleSettings.Default.setAS_isSteerInReverse = cboxSteerInReverse.Checked;
            Properties.Settings.Default.Save();
        }

        private void hsbarUTurnCompensation_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.uturnCompensation = hsbarUTurnCompensation.Value * 0.1;
            lblUTurnCompensation.Text = (hsbarUTurnCompensation.Value - 10).ToString();
        }

        private void cboxSteerInReverse_Click(object sender, EventArgs e)
        {
            Properties.VehicleSettings.Default.setAS_isSteerInReverse = cboxSteerInReverse.Checked;
            mf.isSteerInReverse = cboxSteerInReverse.Checked;
        }

        private void hsbarSideHillComp_ValueChanged(object sender, EventArgs e)
        {
            double deg = hsbarSideHillComp.Value;
            deg *= 0.01;
            lblSideHillComp.Text = (deg.ToString("N2") + "\u00B0");
            Properties.VehicleSettings.Default.setAS_sideHillComp = deg;
            mf.gyd.sideHillCompFactor = deg;
        }

        private void btnStanleyPure_Click(object sender, EventArgs e)
        {
            mf.isStanleyUsed = !mf.isStanleyUsed;

            if (mf.isStanleyUsed)
            {
                btnStanleyPure.Image = Resources.ModeStanley;
                Log.EventWriter("Stanley Steer Mode Selectede");
            }
            else
            {
                btnStanleyPure.Image = Resources.ModePurePursuit;
                Log.EventWriter("Pure Pursuit Steer Mode Selected");
            }

            tabControl1.TabPages.Remove(tabPP);
            tabControl1.TabPages.Remove(tabPPAdv);
            tabControl1.TabPages.Remove(tabGain);
            tabControl1.TabPages.Remove(tabSteer);
            tabControl1.TabPages.Remove(tabStan);


            Properties.VehicleSettings.Default.setVehicle_isStanleyUsed = mf.isStanleyUsed;
            Properties.Settings.Default.Save();

            if (mf.isStanleyUsed)
            {
                this.tabControl1.ItemSize = new System.Drawing.Size(105, 48);
                tabControl1.TabPages.Add(tabStan);
                tabControl1.TabPages.Add(tabGain);
                tabControl1.TabPages.Add(tabSteer);
            }
            else
            {
                tabControl1.TabPages.Add(tabPP);
                tabControl1.TabPages.Add(tabGain);
                tabControl1.TabPages.Add(tabSteer);
                tabControl1.TabPages.Add(tabPPAdv);

                this.tabControl1.ItemSize = new System.Drawing.Size(89, 48);
            }
        }

        #endregion


        #region Alarms Tab

        private void tabAlarm_Enter(object sender, EventArgs e)
        {
            if (mf.isMetric)
            {
                nudMaxSteerSpeed.Value = (decimal)(Properties.VehicleSettings.Default.setAS_maxSteerSpeed);
                nudMinSteerSpeed.Value = (decimal)(Properties.VehicleSettings.Default.setAS_minSteerSpeed);
                nudGuidanceSpeedLimit.Value = (decimal)Properties.VehicleSettings.Default.setAS_functionSpeedLimit;
                label160.Text = label163.Text = label166.Text = "kmh";
            }
            else
            {
                nudMaxSteerSpeed.Value = (decimal)Speed.KmhToMph(Properties.VehicleSettings.Default.setAS_maxSteerSpeed);
                nudMinSteerSpeed.Value = (decimal)Speed.KmhToMph(Properties.VehicleSettings.Default.setAS_minSteerSpeed);
                nudGuidanceSpeedLimit.Value = (decimal)Speed.KmhToMph(Properties.VehicleSettings.Default.setAS_functionSpeedLimit);
                label160.Text = label163.Text = label166.Text = "mph";
            }

            label20.Text = mf.unitsInCm;
        }

        private void tabAlarm_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }


        private void nudMinSteerSpeed_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setAS_minSteerSpeed = ((double)nudMinSteerSpeed.Value);
                if (!mf.isMetric) Properties.VehicleSettings.Default.setAS_minSteerSpeed = Speed.MphToKmh(Properties.VehicleSettings.Default.setAS_minSteerSpeed);
                mf.vehicle.minSteerSpeed = Properties.VehicleSettings.Default.setAS_minSteerSpeed;
            }
        }

        private void nudMaxSteerSpeed_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setAS_maxSteerSpeed = ((double)nudMaxSteerSpeed.Value);
                if (!mf.isMetric) Properties.VehicleSettings.Default.setAS_maxSteerSpeed = Speed.MphToKmh(Properties.VehicleSettings.Default.setAS_maxSteerSpeed);
                mf.vehicle.maxSteerSpeed = Properties.VehicleSettings.Default.setAS_maxSteerSpeed;
            }
        }

        private void nudGuidanceSpeedLimit_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.VehicleSettings.Default.setAS_functionSpeedLimit = ((double)nudGuidanceSpeedLimit.Value);
                if (!mf.isMetric) Properties.VehicleSettings.Default.setAS_functionSpeedLimit = Speed.MphToKmh(Properties.VehicleSettings.Default.setAS_functionSpeedLimit);
                mf.vehicle.functionSpeedLimit = Properties.VehicleSettings.Default.setAS_functionSpeedLimit;
            }
        }

        #endregion


        #region Tab On the Line

        private void tabOnTheLine_Enter(object sender, EventArgs e)
        {
            chkDisplayLightbar.Checked = mf.isLightbarOn;
            if (chkDisplayLightbar.Checked) { chkDisplayLightbar.Image = Resources.SwitchOn; }
            else { chkDisplayLightbar.Image = Resources.SwitchOff; }

            if (mf.isMetric)
            {
                nudSnapDistance.DecimalPlaces = 0;
                nudSnapDistance.Value = (int)((double)Properties.Settings.Default.setAS_snapDistance * mf.cm2CmOrIn);
            }
            else
            {
                nudSnapDistance.DecimalPlaces = 1;
                nudSnapDistance.Value = (decimal)Math.Round(((double)Properties.Settings.Default.setAS_snapDistance * mf.cm2CmOrIn), 1, MidpointRounding.AwayFromZero);
            }

            nudGuidanceLookAhead.Value = (decimal)Properties.Settings.Default.setAS_guidanceLookAheadTime;

            nudLineWidth.Value = Properties.Settings.Default.setDisplay_lineWidth;

            nudcmPerPixel.Value = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;

            label20.Text = mf.unitsInCm;
            label43.Text = mf.unitsInCm;
        }

        private void tabOnTheLine_Leave(object sender, EventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        private void nudcmPerPixel_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setDisplay_lightbarCmPerPixel = ((int)nudcmPerPixel.Value);
                mf.lightbarCmPerPixel = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;
            }
        }

        private void nudLineWidth_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setDisplay_lineWidth = (int)nudLineWidth.Value;
                mf.ABLine.lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
            }
        }

        private void nudSnapDistance_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setAS_snapDistance = ((double)nudSnapDistance.Value * mf.inOrCm2Cm);
                mf.ABLine.snapDistance = Properties.Settings.Default.setAS_snapDistance;
            }
        }

        private void nudGuidanceLookAhead_Click(object sender, EventArgs e)
        {
            if (((NudlessNumericUpDown)sender).ShowKeypad(this))
            {
                Properties.Settings.Default.setAS_guidanceLookAheadTime = ((double)nudGuidanceLookAhead.Value);
                mf.guidanceLookAheadTime = Properties.Settings.Default.setAS_guidanceLookAheadTime;
            }
        }

        private void rbtnLightBar_Click(object sender, EventArgs e)
        {
            mf.isLightBarNotSteerBar = true;
            Properties.Settings.Default.setMenu_isLightbarNotSteerBar = mf.isLightBarNotSteerBar;
            Properties.Settings.Default.Save();
        }

        private void rbtnSteerBar_Click(object sender, EventArgs e)
        {
            mf.isLightBarNotSteerBar = false;
            Properties.Settings.Default.setMenu_isLightbarNotSteerBar = mf.isLightBarNotSteerBar;
            Properties.Settings.Default.Save();
        }

        private void chkDisplayLightbar_Click(object sender, EventArgs e)
        {
            if (chkDisplayLightbar.Checked) { chkDisplayLightbar.Image = Resources.SwitchOn; }
            else { chkDisplayLightbar.Image = Resources.SwitchOff; }

            Properties.Settings.Default.setMenu_isLightbarOn = chkDisplayLightbar.Checked;
            Properties.Settings.Default.Save();
            mf.isLightbarOn = chkDisplayLightbar.Checked;
        }

        #endregion


        //main first tabform 
        #region Gain

        private void hsbarMinPWM_ValueChanged(object sender, EventArgs e)
        {
            lblMinPWM.Text = unchecked((byte)hsbarMinPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarProportionalGain_ValueChanged(object sender, EventArgs e)
        {
            lblProportionalGain.Text = unchecked((byte)hsbarProportionalGain.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            //if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarLowSteerPWM.Value = hsbarHighSteerPWM.Value;
            lblHighSteerPWM.Text = unchecked((byte)hsbarHighSteerPWM.Value).ToString();
            toSend = true;
            counter = 0;
        }


        //private void hsbarLowSteerPWM_ValueChanged(object sender, EventArgs e)
        //{
        //    if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarHighSteerPWM.Value = hsbarLowSteerPWM.Value;
        //    lblLowSteerPWM.Text = unchecked((byte)hsbarLowSteerPWM.Value).ToString();
        //    toSend = true;
        //    counter = 0;
        //}

        #endregion Gain

        #region Steer

        private void hsbarAckerman_ValueChanged(object sender, EventArgs e)
        {
            lblAckerman.Text = unchecked((byte)hsbarAckerman.Value).ToString();
            toSend = true;
            counter = 0;
        }

        private void hsbarMaxSteerAngle_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.maxSteerAngle = hsbarMaxSteerAngle.Value;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();
        }

        private void hsbarCountsPerDegree_ValueChanged(object sender, EventArgs e)
        {
            lblCountsPerDegree.Text = unchecked((byte)hsbarCountsPerDegree.Value).ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");
            toSend = true;
            counter = 0;
        }

        private void hsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");
            toSend = true;
            counter = 0;
        }

        private void btnZeroWAS_Click(object sender, EventArgs e)
        {
            int offset = (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees + hsbarWasOffset.Value);
            if (Math.Abs(offset) > 3900)
            {
                FormDialog.Show("Exceeded Range", "Excessive Steer Angle - Cannot Zero", DialogSeverity.Error);
                Log.EventWriter("Excessive Steer Angle, No Zero " + offset);
            }
            else
            {
                hsbarWasOffset.Value += (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees);
            }
        }

        private void btnStartSA_Click(object sender, EventArgs e)
        {
            if (!isSA)
            {
                isSA = true;
                startFix = mf.pivotAxlePos;
                dist = 0;
                diameter = 0;
                cntr = 0;
                btnStartSA.Image = Properties.Resources.boundaryStop;
                lblDiameter.Text = "0";
                lblCalcSteerAngleInner.Text = "Drive Steady";
                //lblCalcSteerAngleOuter.Text = "Consistent Steering Angle!!";
            }
            else
            {
                isSA = false;
                lblCalcSteerAngleInner.Text = "0.0" + "\u00B0";
                //lblCalcSteerAngleOuter.Text = "0.0" + "\u00B0";
                btnStartSA.Image = Properties.Resources.BoundaryRecord;
            }
        }

        #endregion Steer

        # region Stanley

        private void hsbarStanleyGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyDistanceErrorGain = hsbarStanleyGain.Value * 0.1;
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();
        }

        private void hsbarHeadingErrorGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyHeadingErrorGain = hsbarHeadingErrorGain.Value * 0.1;
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();
        }

        private void hsbarIntegral_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyIntegralGainAB = hsbarIntegral.Value * 0.01;
            lblIntegralPercent.Text = hsbarIntegral.Value.ToString();
        }

        #endregion

        #region Pure

        private void hsbarHoldLookAhead_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadHold = hsbarHoldLookAhead.Value * 0.1;
            lblHoldLookAhead.Text = mf.vehicle.goalPointLookAheadHold.ToString();
            lblAcquirePP.Text = (mf.vehicle.goalPointLookAheadHold * mf.vehicle.goalPointAcquireFactor).ToString("N1");
        }

        private void hsbarIntegralPurePursuit_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.purePursuitIntegralGain = hsbarIntegralPurePursuit.Value * 0.01;
            lblPureIntegral.Text = hsbarIntegralPurePursuit.Value.ToString();
        }

        private void hsbarLookAheadMult_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadMult = hsbarLookAheadMult.Value * 0.1;
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();
        }

        private void hsbarAcquireFactor_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointAcquireFactor = hsbarAcquireFactor.Value * 0.01;
            lblAcquireFactor.Text = mf.vehicle.goalPointAcquireFactor.ToString();
        }

        private void nudDeadZoneHeading_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            mf.vehicle.deadZoneHeading = (int)(nudDeadZoneHeading.Value * 100);
        }

        private void nudDeadZoneDelay_Click(object sender, EventArgs e)
        {
            ((NudlessNumericUpDown)sender).ShowKeypad(this);
            mf.vehicle.deadZoneDelay = (int)(nudDeadZoneDelay.Value);
        }

        private void expandWindow_Click(object sender, EventArgs e)
        {
            if (windowSizeState++ > 0) windowSizeState = 0;
            if (windowSizeState == 1)
            {
                this.Size = new System.Drawing.Size(918, 673);
                btnExpand.Image = Properties.Resources.ArrowLeft;
            }
            else if (windowSizeState == 0)
            {
                this.Size = new System.Drawing.Size(392, 492);
                btnExpand.Image = Properties.Resources.ArrowRight;
            }
        }


        #endregion

        #region Free Drive

        private void btnFreeDrive_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.isInFreeDriveMode)
            {
                //turn OFF free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
                btnFreeDrive.BackColor = Color.FromArgb(50, 50, 70);
                mf.vehicle.isInFreeDriveMode = false;
                btnSteerAngleDown.Enabled = false;
                btnSteerAngleUp.Enabled = false;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeSteerAngle = 0;
            }
            else
            {
                //turn ON free drive mode
                btnFreeDrive.Image = Properties.Resources.SteerDriveOn;
                btnFreeDrive.BackColor = Color.LightGreen;
                mf.vehicle.isInFreeDriveMode = true;
                btnSteerAngleDown.Enabled = true;
                btnSteerAngleUp.Enabled = true;
                //hSBarFreeDrive.Value = 0;
                mf.vehicle.driveFreeSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
        }

        private void btnFreeDriveZero_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.driveFreeSteerAngle == 0)
                mf.vehicle.driveFreeSteerAngle = 5;
            else mf.vehicle.driveFreeSteerAngle = 0;
            //hSBarFreeDrive.Value = mf.driveFreeSteerAngle;
        }

        private void btnSteerAngleUp_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle++;
            if (mf.vehicle.driveFreeSteerAngle > 40) mf.vehicle.driveFreeSteerAngle = 40;
        }

        private void btnSteerAngleDown_MouseDown(object sender, MouseEventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle--;
            if (mf.vehicle.driveFreeSteerAngle < -40) mf.vehicle.driveFreeSteerAngle = -40;
        }

        #endregion

        private void btnSendSteerConfigPGN_Click(object sender, EventArgs e)
        {
            SaveSettings();
            mf.SendPgnToLoop(mf.p_251.pgn);
            pboxSendSteer.Visible = false;
            btnClose.Enabled = true;
            Log.EventWriter("Steer Form, Send and Save Pressed");

            mf.TimedMessageBox(2000, gStr.gsAutoSteerPort, "Settings Sent To Steer Module");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void SaveSettings()
        {
            int set = 1;
            int reset = 2046;
            int sett = 0;

            if (chkInvertWAS.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (chkSteerInvertRelays.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (chkInvertSteer.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxConv.Text == "Single") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxMotorDrive.Text == "Cytron") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxSteerEnable.Text == "Switch") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxSteerEnable.Text == "Button") sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxEncoder.Checked) sett |= set;
            else sett &= reset;

            //set = (set << 1);
            //reset = (reset << 1);
            //reset = (reset + 1);
            //if ( ) sett |= set;
            //else sett &= reset;

            Properties.VehicleSettings.Default.setArdSteer_setting0 = (byte)sett;
            Properties.VehicleSettings.Default.setArdMac_isDanfoss = cboxDanfoss.Checked;

            if (cboxCurrentSensor.Checked || cboxPressureSensor.Checked)
            {
                Properties.VehicleSettings.Default.setArdSteer_maxPulseCounts = (byte)hsbarSensor.Value;
            }
            else
            {
                Properties.VehicleSettings.Default.setArdSteer_maxPulseCounts = (byte)nudMaxCounts.Value;
            }

            // Settings1
            set = 1;
            reset = 2046;
            sett = 0;

            if (cboxDanfoss.Checked) sett |= set;
            else sett &= reset;

            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxPressureSensor.Checked) sett |= set;
            else sett &= reset;

            //bit 2
            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxCurrentSensor.Checked) sett |= set;
            else sett &= reset;

            //bit 3
            set <<= 1;
            reset <<= 1;
            reset += 1;
            if (cboxXY.Text == "Y") sett |= set;
            else sett &= reset;

            Properties.VehicleSettings.Default.setArdSteer_setting1 = (byte)sett;

            Properties.Settings.Default.Save();

            mf.p_251.pgn[mf.p_251.set0] = Properties.VehicleSettings.Default.setArdSteer_setting0;
            mf.p_251.pgn[mf.p_251.set1] = Properties.VehicleSettings.Default.setArdSteer_setting1;
            mf.p_251.pgn[mf.p_251.maxPulse] = Properties.VehicleSettings.Default.setArdSteer_maxPulseCounts;
            mf.p_251.pgn[mf.p_251.minSpeed] = unchecked((byte)(Properties.VehicleSettings.Default.setAS_minSteerSpeed * 10));

            if (Properties.Settings.Default.setAS_isConstantContourOn)
                mf.p_251.pgn[mf.p_251.angVel] = 1;
            else mf.p_251.pgn[mf.p_251.angVel] = 0;

            pboxSendSteer.Visible = false;
        }


        private void btnSteerWizard_Click(object sender, EventArgs e)
        {
            Close();
            Form form = new FormSteerWiz(mf);
            form.Show(mf);
        }

        private void btnVehicleReset_Click(object sender, EventArgs e)
        {
            DialogResult result = FormDialog.ShowQuestion(
                "Reset This Page to Defaults",
                "Are you Sure");

            if (result == DialogResult.OK)
            {
                Log.EventWriter("Steer Form - Steer Settings Set to Default");

                mf.TimedMessageBox(2000, "Reset To Default", "Values Set to Inital Default");
                Properties.VehicleSettings.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle
                    = 45;

                Properties.VehicleSettings.Default.setAS_countsPerDegree = 110;

                Properties.VehicleSettings.Default.setAS_ackerman = 100;

                Properties.VehicleSettings.Default.setAS_wasOffset = 3;

                Properties.VehicleSettings.Default.setAS_highSteerPWM = 180;
                Properties.VehicleSettings.Default.setAS_Kp = 50;
                Properties.VehicleSettings.Default.setAS_minSteerPWM = 25;

                Properties.VehicleSettings.Default.setArdSteer_setting0 = 56;
                Properties.VehicleSettings.Default.setArdSteer_setting1 = 0;
                Properties.VehicleSettings.Default.setArdMac_isDanfoss = false;

                Properties.VehicleSettings.Default.setArdSteer_maxPulseCounts = 3;

                Properties.ToolSettings.Default.setVehicle_goalPointAcquireFactor = 0.85;
                Properties.ToolSettings.Default.setVehicle_goalPointLookAheadHold = 3;
                Properties.ToolSettings.Default.setVehicle_goalPointLookAheadMult = 1.5;

                Properties.VehicleSettings.Default.stanleyHeadingErrorGain = 1;
                Properties.VehicleSettings.Default.stanleyDistanceErrorGain = 1;
                Properties.VehicleSettings.Default.stanleyIntegralGainAB = 0;

                Properties.VehicleSettings.Default.purePursuitIntegralGainAB = 0;

                Properties.VehicleSettings.Default.setAS_sideHillComp = 0;

                Properties.Settings.Default.setAS_uTurnCompensation = 1;

                Properties.VehicleSettings.Default.setIMU_invertRoll = false;

                Properties.VehicleSettings.Default.setIMU_rollZero = 0;

                Properties.VehicleSettings.Default.setAS_minSteerSpeed = 0;
                Properties.VehicleSettings.Default.setAS_maxSteerSpeed = 15;
                Properties.VehicleSettings.Default.setAS_functionSpeedLimit = 12;
                Properties.Settings.Default.setDisplay_lightbarCmPerPixel = 5;
                Properties.Settings.Default.setDisplay_lineWidth = 2;
                Properties.Settings.Default.setAS_snapDistance = 20;
                Properties.Settings.Default.setAS_guidanceLookAheadTime = 1.5;
                Properties.Settings.Default.setAS_uTurnCompensation = 1;

                Properties.VehicleSettings.Default.setVehicle_isStanleyUsed = false;
                mf.isStanleyUsed = false;

                Properties.VehicleSettings.Default.setAS_isSteerInReverse = false;
                mf.isSteerInReverse = false;

                //save current vehicle
                Properties.Settings.Default.Save();

                mf.vehicle = new CVehicle(mf);

                FormSteer_Load(this, e);

                toSend = true; counter = 6;

                pboxSendSteer.Visible = true;

                tabControl1.SelectTab(1);
                tabControl1.SelectTab(0);
                tabSteerSettings.SelectTab(1);
                tabSteerSettings.SelectTab(0);
            }
        }

        #region Smart WAS Calibration

        /// <summary>
        /// Smart WAS Zero button click - applies the recommended offset
        /// </summary>
        private void btnSmartZeroWAS_Click(object sender, EventArgs e)
        {
            if (!mf.smartWAS.HasValidCalibration)
            {
                return;
            }

            double recommendedOffset = mf.smartWAS.RecommendedOffset;
            int offsetCounts = (int)Math.Round(recommendedOffset * hsbarCountsPerDegree.Value);

            // Safety check - limit maximum offset
            if (Math.Abs(offsetCounts) > 50)
            {
                mf.TimedMessageBox(3000, "Exceeded Range", "Offset too large - cannot apply");
                Log.EventWriter($"Smart WAS: Offset {offsetCounts} too large, rejected");
                return;
            }

            // Apply the offset
            hsbarWasOffset.Value += offsetCounts;

            // Apply correction to collected data to prevent double-correction
            mf.smartWAS.ApplyOffsetCorrection(recommendedOffset);

            // Show confirmation
            mf.TimedMessageBox(2500, "Smart WAS Zero",
                $"Applied {recommendedOffset:F2}° ({offsetCounts} counts)");

            Log.EventWriter($"Smart WAS: Applied {recommendedOffset:F2}° ({offsetCounts} counts)");

            toSend = true;
            counter = 6;
        }

        /// <summary>
        /// Update the Smart WAS status display
        /// </summary>
        private void UpdateSmartWASStatus()
        {
            lblSmartCalSamples.Text = $"Samples: {mf.smartWAS.SampleCount}";
            lblSmartCalConfidence.Text = $"Confidence: {mf.smartWAS.Confidence:F0}%";

            if (mf.smartWAS.IsCollecting)
            {
                if (mf.smartWAS.HasValidCalibration)
                {
                    lblSmartCalStatus.Text = "Ready to Apply";
                    lblSmartCalStatus.ForeColor = Color.Green;
                    btnSmartZeroWAS.Text = $"{mf.smartWAS.RecommendedOffset:+0.0;-0.0}°";
                    btnSmartZeroWAS.Enabled = true;
                }
                else
                {
                    lblSmartCalStatus.Text = "Collecting...";
                    lblSmartCalStatus.ForeColor = Color.Orange;
                    btnSmartZeroWAS.Text = "";
                    btnSmartZeroWAS.Enabled = false;
                }
            }
            else
            {
                lblSmartCalStatus.Text = "Stopped";
                lblSmartCalStatus.ForeColor = Color.Gray;
                btnSmartZeroWAS.Text = "";
                btnSmartZeroWAS.Enabled = false;
            }
        }

        #endregion
    }
}
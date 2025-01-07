using AgLibrary.Logging;
using AgOpenGPS.Culture;
using AgOpenGPS.Helpers;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSteerWiz : Form
    {
        private readonly FormGPS mf = null;

        private bool toSend252 = false, toSend251 = false, isSARight = false, isSALeft = false;
        private int counter252 = 0, counter251 = 0, cntr;
        private vec3 startFix;
        private double diameter, steerAngleRight, steerAngleLeft, dist, startAngleLeft;
        private bool isWizardStarted = false;

        //Form stuff
        public FormSteerWiz(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
            nudMaxCounts.Controls[0].Enabled = false;

            nudAntennaHeight.Controls[0].Enabled = false;
            nudAntennaOffset.Controls[0].Enabled = false;
            nudAntennaPivot.Controls[0].Enabled = false;
            nudVehicleTrack.Controls[0].Enabled = false;
            nudWheelbase.Controls[0].Enabled = false;

            this.label3.Text = gStr.gsAgressiveness;
            this.label5.Text = gStr.gsOvershootReduction;
            this.Text = gStr.gsAutoSteerConfiguration;
            //this.Width = 378;
            //this.Height = 462;
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            tabWiz.Appearance = TabAppearance.FlatButtons;
            tabWiz.ItemSize = new Size(0, 1);
            tabWiz.SizeMode = TabSizeMode.Fixed;

            pbarProgress.Maximum = tabWiz.TabCount - 1;

            hsbarWasOffset.Value = Properties.Settings.Default.setAS_wasOffset;
            hsbarCountsPerDegree.Value = Properties.Settings.Default.setAS_countsPerDegree;

            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");

            hsbarAckerman.Value = Properties.Settings.Default.setAS_ackerman;
            lblAckerman.Text = hsbarAckerman.Value.ToString();

            //min pwm, kP
            hsbarMinPWM.Value = Properties.Settings.Default.setAS_minSteerPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Properties.Settings.Default.setAS_Kp;
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            //low steer, high steer
            hsbarLowSteerPWM.Value = Properties.Settings.Default.setAS_lowSteerPWM;
            lblLowSteerPWM.Text = hsbarLowSteerPWM.Value.ToString();

            hsbarHighSteerPWM.Value = Properties.Settings.Default.setAS_highSteerPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            hsbarMaxSteerAngle.Value = (Int16)Properties.Settings.Default.setVehicle_maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            mf.vehicle.stanleyDistanceErrorGain = Properties.Settings.Default.stanleyDistanceErrorGain;
            hsbarStanleyGain.Value = (Int16)(mf.vehicle.stanleyDistanceErrorGain * 10);
            lblStanleyGain.Text = mf.vehicle.stanleyDistanceErrorGain.ToString();

            mf.vehicle.stanleyHeadingErrorGain = Properties.Settings.Default.stanleyHeadingErrorGain;
            hsbarHeadingErrorGain.Value = (Int16)(mf.vehicle.stanleyHeadingErrorGain * 10);
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();

            hsbarIntegral.Value = (int)(Properties.Settings.Default.stanleyIntegralGainAB * 100);
            lblIntegralPercent.Text = ((int)(mf.vehicle.stanleyIntegralGainAB * 100)).ToString();

            hsbarIntegralPurePursuit.Value = (int)(Properties.Settings.Default.purePursuitIntegralGainAB * 100);
            lblPureIntegral.Text = ((int)(mf.vehicle.purePursuitIntegralGain * 100)).ToString();

            hsbarSideHillComp.Value = (int)(Properties.Settings.Default.setAS_sideHillComp * 100);
            mf.gyd.sideHillCompFactor = Properties.Settings.Default.setAS_sideHillComp;

            mf.vehicle.goalPointLookAheadMult = Properties.Settings.Default.setVehicle_goalPointLookAheadMult;
            hsbarLookAheadMult.Value = (Int16)(mf.vehicle.goalPointLookAheadMult * 10);
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();

            nudAntennaPivot.Value = (int)((Properties.Settings.Default.setVehicle_antennaPivot) * mf.m2InchOrCm);
            nudAntennaHeight.Value = (int)(Properties.Settings.Default.setVehicle_antennaHeight * mf.m2InchOrCm);
            nudAntennaOffset.Value = (int)(Properties.Settings.Default.setVehicle_antennaOffset * mf.m2InchOrCm);
            nudWheelbase.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_wheelbase) * mf.m2InchOrCm);
            nudVehicleTrack.Value = (int)(Math.Abs(Properties.Settings.Default.setVehicle_trackWidth) * mf.m2InchOrCm);

            cboxDataInvertRoll.Checked = Properties.Settings.Default.setIMU_invertRoll;
            mf.ahrs.isRollInvert = Properties.Settings.Default.setIMU_invertRoll;

            lblRollZeroOffset.Text = ((double)Properties.Settings.Default.setIMU_rollZero).ToString("N2");
            mf.ahrs.rollZero = Properties.Settings.Default.setIMU_rollZero;
            lblRollZeroOffset.Text = "0.00";

            //make sure free drive is off
            btnFreeDrive.Image = Properties.Resources.SteerDriveOff;
            mf.vehicle.isInFreeDriveMode = false;
            btnSteerAngleDown.Enabled = false;
            btnSteerAngleUp.Enabled = false;
            btnFreeDriveZero.Enabled = false;
            //hSBarFreeDrive.Value = 0;
            mf.vehicle.driveFreeSteerAngle = 0;

            toSend252 = false;
            toSend251 = false;

            int sett = Properties.Settings.Default.setArdSteer_setting0;

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

            nudMaxCounts.Value = (decimal)Properties.Settings.Default.setArdSteer_maxPulseCounts;
            hsbarSensor.Value = (int)Properties.Settings.Default.setArdSteer_maxPulseCounts;
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";

            sett = Properties.Settings.Default.setArdSteer_setting1;

            if ((sett & 1) == 0) cboxDanfoss.Checked = false;
            else cboxDanfoss.Checked = true;

            if ((sett & 2) == 0) cboxPressureSensor.Checked = false;
            else cboxPressureSensor.Checked = true;

            if ((sett & 4) == 0) cboxCurrentSensor.Checked = false;
            else cboxCurrentSensor.Checked = true;

            if (cboxEncoder.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                label61.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = true;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
                label61.Text = gStr.gsEncoderCounts;
            }
            else if (cboxPressureSensor.Checked)
            {
                cboxEncoder.Checked = false;
                cboxCurrentSensor.Checked = false;
                label61.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                label61.Text = "Off at %";
            }
            else if (cboxCurrentSensor.Checked)
            {
                cboxPressureSensor.Checked = false;
                cboxEncoder.Checked = false;
                label61.Visible = true;
                lblPercentFS.Visible = true;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = true;
                hsbarSensor.Visible = true;
                lblhsbarSensor.Visible = true;

                label61.Text = "Off at %";
            }
            else
            {
                cboxPressureSensor.Checked = false;
                cboxCurrentSensor.Checked = false;
                cboxEncoder.Checked = false;
                label61.Visible = false;
                lblPercentFS.Visible = false;
                nudMaxCounts.Visible = false;
                pbarSensor.Visible = false;
                hsbarSensor.Visible = false;
                lblhsbarSensor.Visible = false;
                return;
            }

            if (!ScreenHelper.IsOnScreen(Bounds))
            {
                Top = 0;
                Left = 0;
            }
        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.vehicle.isInFreeDriveMode = false;

            Properties.Settings.Default.stanleyHeadingErrorGain = mf.vehicle.stanleyHeadingErrorGain;
            Properties.Settings.Default.stanleyDistanceErrorGain = mf.vehicle.stanleyDistanceErrorGain;
            Properties.Settings.Default.stanleyIntegralGainAB = mf.vehicle.stanleyIntegralGainAB;

            Properties.Settings.Default.purePursuitIntegralGainAB = mf.vehicle.purePursuitIntegralGain;
            Properties.Settings.Default.setVehicle_goalPointLookAheadMult = mf.vehicle.goalPointLookAheadMult;

            Properties.Settings.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle;

            Properties.Settings.Default.setAS_countsPerDegree = mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
            Properties.Settings.Default.setAS_ackerman = mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

            Properties.Settings.Default.setAS_wasOffset = hsbarWasOffset.Value;
            mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
            mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

            Properties.Settings.Default.setAS_highSteerPWM = mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
            Properties.Settings.Default.setAS_lowSteerPWM = mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)hsbarLowSteerPWM.Value);
            Properties.Settings.Default.setAS_Kp = mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
            Properties.Settings.Default.setAS_minSteerPWM = mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

            hsbarSideHillComp.Value = (int)(Properties.Settings.Default.setAS_sideHillComp * 100);

            Properties.Settings.Default.setIMU_invertRoll = mf.ahrs.isRollInvert;

            Properties.Settings.Default.Save();

            //save current vehicle
            RegistrySettings.Save();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (isSARight)
            {
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
                    steerAngleRight = Math.Atan(mf.vehicle.wheelbase / ((diameter - mf.vehicle.trackWidth * 0.5) / 2));
                    steerAngleRight = glm.toDegrees(steerAngleRight);

                    lblCalcSteerAngleInner.Text = steerAngleRight.ToString("N1") + "\u00B0";
                    lblDiameter.Text = diameter.ToString("N2") + " m";
                    btnStartSA.Image = Properties.Resources.BoundaryRecord;
                    isSARight = false;

                    try
                    {
                        //force cpd a bit on the low side
                        double cpd = (mf.mc.actualSteerAngleDegrees / steerAngleRight * hsbarCountsPerDegree.Value);
                        cpd *= 0.9;
                        hsbarCountsPerDegree.Value = (int)cpd;
                        lblCPDError.Text = "CPD set to: " + hsbarCountsPerDegree.Value.ToString();
                    }
                    catch (Exception ed)
                    {
                        hsbarCountsPerDegree.Value = 100;
                        lblCPDError.Text = "Error, CPD set to 100";
                        Log.EventWriter("Error, CPD set to 100" + ed.ToString());
                    }
                }
            }

            if (isSALeft)
            {
                dist = glm.Distance(startFix, mf.pivotAxlePos);
                cntr++;
                if (dist > diameter)
                {
                    diameter = dist;
                    cntr = 0;
                }
                lblDiameterLeft.Text = diameter.ToString("N2") + " m";

                if (cntr > 9)
                {
                    steerAngleLeft = Math.Atan(mf.vehicle.wheelbase / ((diameter - mf.vehicle.trackWidth * 0.5) / 2));
                    steerAngleLeft = glm.toDegrees(steerAngleLeft);

                    lblCalcSteerAngleLeft.Text = steerAngleLeft.ToString("N1") + "\u00B0";
                    lblDiameterLeft.Text = diameter.ToString("N2") + " m";
                    btnStartSA_Left.Image = Properties.Resources.BoundaryRecord;
                    isSALeft = false;

                    try
                    {
                        hsbarAckerman.Value = (int)((steerAngleLeft / Math.Abs(startAngleLeft)) * 100);
                        lblAckermannError.Text = "Ackermann Set to: " + hsbarAckerman.Value.ToString();
                    }
                    catch (Exception eh)
                    {
                        hsbarAckerman.Value = 100;
                        lblAckermannError.Text = "Error, Ackermann set to 100";
                        Log.EventWriter("Error, Ackermann set to 100"+ eh.ToString());
                    }
                }
            }

            //steering bar
            double actAng = mf.mc.actualSteerAngleDegrees;
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

            //wizard progress bar
            CExtensionMethods.SetProgressNoAnimation(pbarProgress, tabWiz.SelectedIndex);

            lblSteerAngle.Text = mf.SetSteerAngle;
            lblSteerAngleActual.Text = mf.mc.actualSteerAngleDegrees.ToString("N1") + "\u00B0";
            //lblActualSteerAngleUpper.Text = lblSteerAngleActual.Text;
            double err = (mf.mc.actualSteerAngleDegrees - mf.guidanceLineSteerAngle * 0.01);
            lblError.Text = Math.Abs(err).ToString("N1") + "\u00B0";
            if (err > 0) lblError.ForeColor = Color.Red;
            else lblError.ForeColor = Color.DarkGreen;

            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();
            counter252++;
            counter251++;

            if (toSend252 && counter252 > 3)
            {
                Properties.Settings.Default.setAS_countsPerDegree = mf.p_252.pgn[mf.p_252.countsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
                Properties.Settings.Default.setAS_ackerman = mf.p_252.pgn[mf.p_252.ackerman] = unchecked((byte)hsbarAckerman.Value);

                Properties.Settings.Default.setAS_wasOffset = hsbarWasOffset.Value;
                mf.p_252.pgn[mf.p_252.wasOffsetHi] = unchecked((byte)(hsbarWasOffset.Value >> 8));
                mf.p_252.pgn[mf.p_252.wasOffsetLo] = unchecked((byte)(hsbarWasOffset.Value));

                Properties.Settings.Default.setAS_highSteerPWM = mf.p_252.pgn[mf.p_252.highPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
                Properties.Settings.Default.setAS_lowSteerPWM = mf.p_252.pgn[mf.p_252.lowPWM] = unchecked((byte)hsbarLowSteerPWM.Value);
                Properties.Settings.Default.setAS_Kp = mf.p_252.pgn[mf.p_252.gainProportional] = unchecked((byte)hsbarProportionalGain.Value);
                Properties.Settings.Default.setAS_minSteerPWM = mf.p_252.pgn[mf.p_252.minPWM] = unchecked((byte)hsbarMinPWM.Value);

                Properties.Settings.Default.Save();

                mf.SendPgnToLoop(mf.p_252.pgn);
                toSend252 = false;
                counter252 = 0;
            }
            //*************************************************************
            else if (toSend251 && counter251 > 3)
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

                Properties.Settings.Default.setArdSteer_setting0 = (byte)sett;
                Properties.Settings.Default.setArdMac_isDanfoss = cboxDanfoss.Checked;

                if (cboxCurrentSensor.Checked || cboxPressureSensor.Checked)
                {
                    Properties.Settings.Default.setArdSteer_maxPulseCounts = (byte)hsbarSensor.Value;
                }
                else
                {
                    Properties.Settings.Default.setArdSteer_maxPulseCounts = (byte)nudMaxCounts.Value;
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

                set <<= 1;
                reset <<= 1;
                reset += 1;
                if (cboxCurrentSensor.Checked) sett |= set;
                else sett &= reset;

                Properties.Settings.Default.setArdSteer_setting1 = (byte)sett;

                Properties.Settings.Default.Save();

                mf.p_251.pgn[mf.p_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
                mf.p_251.pgn[mf.p_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
                mf.p_251.pgn[mf.p_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
                mf.p_251.pgn[mf.p_251.minSpeed] = unchecked((byte)(Properties.Settings.Default.setAS_minSteerSpeed * 10)); //0.5 kmh

                mf.SendPgnToLoop(mf.p_251.pgn);

                toSend251 = false;
                counter251 = 0;
            }

            if (hsbarMinPWM.Value > hsbarLowSteerPWM.Value) lblMinPWM.ForeColor = Color.OrangeRed;
            else lblMinPWM.ForeColor = SystemColors.ControlText;

            if (mf.mc.sensorData != -1)
            {
                if (mf.mc.sensorData < 0 || mf.mc.sensorData > 255) mf.mc.sensorData = 0;
                CExtensionMethods.SetProgressNoAnimation(pbarSensor, mf.mc.sensorData);
                lblPercentFS.Text = ((int)((double)mf.mc.sensorData * 0.3921568627)).ToString() + "%";
            }

            // Emulate the OGL Steer circle
            if (mf.mc.steerSwitchHigh)
                btnSteerStatus.BackColor = Color.Red;
            else if (mf.isBtnAutoSteerOn)
                btnSteerStatus.BackColor = Color.Green;
            else
                btnSteerStatus.BackColor = Color.Yellow;

            //we have lost connection to steer module
            if (mf.steerModuleConnectedCounter > 30)
            {
                btnSteerStatus.BackColor = Color.Magenta;
            }
        }

        private void sideBarTimer_Tick(object sender, EventArgs e)
        {
            //roll zero
            if (tabWiz.SelectedTab.Name == "tabWAS_Zero") lblCurrentHeading.Text = mf.Heading;

            //ackermann
            else if (tabWiz.SelectedTab.Name == "tabAckCPD")
            {
                if (hsbarAckerman.Value != 100)
                {
                    btnStartSA_Left.Enabled = false;
                }
                else
                {
                    btnStartSA_Left.Enabled = true;
                }
            }

            //roll invert
            else if (tabWiz.SelectedTab.Name == "tabRollInv")
            {
                lblRoll.Text = mf.RollInDegrees;
            }

            //roll zero
            else if (tabWiz.SelectedTab.Name == "tabRollZero")
            {
                lblRoll2.Text = mf.RollInDegrees;
            }

            lblBarAck.Text = hsbarAckerman.Value.ToString();
            lblBarWasOffset.Text = hsbarWasOffset.Value.ToString();
            lblBarCPD.Text = hsbarCountsPerDegree.Value.ToString();
        }

        #region ButtonControl

        private void btnLoadDefaults_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(2000, "Reset To Default", "Values Set to Inital Default");
            Properties.Settings.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle
                = 45;

            Properties.Settings.Default.setAS_countsPerDegree = 100;

            Properties.Settings.Default.setAS_ackerman = 100;

            Properties.Settings.Default.setAS_wasOffset = 0;

            Properties.Settings.Default.setAS_highSteerPWM = 150;
            Properties.Settings.Default.setAS_lowSteerPWM = 30;
            Properties.Settings.Default.setAS_Kp = 120;
            Properties.Settings.Default.setAS_minSteerPWM = 25;

            Properties.Settings.Default.setArdSteer_setting0 = 56;
            Properties.Settings.Default.setArdSteer_setting1 = 0;
            Properties.Settings.Default.setArdMac_isDanfoss = false;

            Properties.Settings.Default.setArdSteer_maxPulseCounts = 0;

            Properties.Settings.Default.setVehicle_goalPointLookAhead = 2.5;
            Properties.Settings.Default.setVehicle_goalPointLookAheadMult = 1;

            Properties.Settings.Default.stanleyHeadingErrorGain = 1;
            Properties.Settings.Default.stanleyDistanceErrorGain = 1;
            Properties.Settings.Default.stanleyIntegralGainAB = 0;

            Properties.Settings.Default.purePursuitIntegralGainAB = 0;

            Properties.Settings.Default.setAS_sideHillComp = 0;

            Properties.Settings.Default.setVehicle_wheelbase = 2.8;

            Properties.Settings.Default.setVehicle_trackWidth = 1.9;

            Properties.Settings.Default.setVehicle_antennaPivot = 0.1;

            Properties.Settings.Default.setVehicle_antennaHeight = 3;

            Properties.Settings.Default.setVehicle_antennaOffset = 0;

            Properties.Settings.Default.setIMU_invertRoll = false;

            Properties.Settings.Default.setIMU_rollZero = mf.ahrs.rollZero;

            Properties.Settings.Default.Save();

            toSend252 = true;
            counter252 = 3;
            toSend251 = true;
            counter251 = 2;

            //save current vehicle
            RegistrySettings.Save();

            FormSteer_Load(this, e);
        }

        private void btnStartWizard_Click(object sender, EventArgs e)
        {
            panel1.Visible = false;
            isWizardStarted = true;
            tabWiz.SelectedIndex++;
        }

        private void btnStopWizard_Click(object sender, EventArgs e)
        {
            isWizardStarted = false;
            FreeDrive(false);
            tabWiz.SelectedIndex = 0;
            panel1.Visible = true;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion ButtonControl

        #region Wizard

        private void nudMaxCounts_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                if (isWizardStarted)
                {
                    toSend251 = true;
                    counter251 = 0;
                }
            }
        }

        private void cboxMotorDrive_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void cboxConv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void chkInvertWAS_CheckedChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void chkInvertSteer_CheckedChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void cboxSteerEnable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void cboxDanfoss_CheckedChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void chkSteerInvertRelays_CheckedChanged(object sender, EventArgs e)
        {
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void cboxCancelGuidance_Click(object sender, EventArgs e)
        {
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
                        label61.Visible = false;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        if (isWizardStarted)
                        {
                            toSend251 = true;
                            counter251 = 0;
                        }
                        return;
                    }

                    if (checkbox == cboxPressureSensor)
                    {
                        cboxEncoder.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        label61.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        pbarSensor.Visible = true;
                        label61.Text = "Off at %";
                        hsbarSensor.Visible = true;
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxCurrentSensor)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxEncoder.Checked = false;
                        label61.Visible = true;
                        lblPercentFS.Visible = true;
                        nudMaxCounts.Visible = false;
                        hsbarSensor.Visible = true;
                        pbarSensor.Visible = true;
                        label61.Text = "Off at %";
                        lblhsbarSensor.Visible = true;
                    }
                    else if (checkbox == cboxEncoder)
                    {
                        cboxPressureSensor.Checked = false;
                        cboxCurrentSensor.Checked = false;
                        label61.Visible = true;
                        lblPercentFS.Visible = false;
                        nudMaxCounts.Visible = true;
                        pbarSensor.Visible = false;
                        hsbarSensor.Visible = false;
                        lblhsbarSensor.Visible = false;
                        label61.Text = gStr.gsEncoderCounts;
                    }
                }
            }

            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void btnOkNextCancelGuidance_Click(object sender, EventArgs e)
        {
            //btnSendSteerConfigPGN.PerformClick();
            tabWiz.SelectedIndex = (tabWiz.SelectedIndex + 1 < tabWiz.TabCount) ?
                tabWiz.SelectedIndex + 1 : tabWiz.SelectedIndex;
        }

        private void hsbarSensor_Scroll(object sender, ScrollEventArgs e)
        {
            lblhsbarSensor.Text = ((int)((double)hsbarSensor.Value * 0.3921568627)).ToString() + "%";
            if (isWizardStarted)
            {
                toSend251 = true;
                counter251 = 0;
            }
        }

        private void btnOkNext_Click(object sender, EventArgs e)
        {
            tabWiz.SelectedIndex = (tabWiz.SelectedIndex + 1 < tabWiz.TabCount) ?
                 tabWiz.SelectedIndex + 1 : tabWiz.SelectedIndex;
            lblCPDError.Text = "...";
            lblAckermannError.Text = "...";
            lblStartAngleLeft.Text = "...";
            lblRightStartAngle.Text = "...";
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            if (tabWiz.SelectedTab == tabWiz.TabPages["tabMaxSteerAngle"])
            {
                tabWiz.SelectedIndex = (tabWiz.SelectedIndex - 3 < tabWiz.TabCount) ?
                                 tabWiz.SelectedIndex - 3 : tabWiz.SelectedIndex;
            }
            else
            {
                tabWiz.SelectedIndex = (tabWiz.SelectedIndex - 1 < tabWiz.TabCount) ?
                                 tabWiz.SelectedIndex - 1 : tabWiz.SelectedIndex;
            }

            lblCPDError.Text = "...";
            lblAckermannError.Text = "...";
            lblStartAngleLeft.Text = "...";
            lblRightStartAngle.Text = "...";
        }

        private void btnSkipCPD_Setup_Click(object sender, EventArgs e)
        {
            tabWiz.SelectedIndex = (tabWiz.SelectedIndex + 3 < tabWiz.TabCount) ?
                tabWiz.SelectedIndex + 3 : tabWiz.SelectedIndex;
        }

        private void btnOKNext_CPDSetup_Click(object sender, EventArgs e)
        {
            mf.TimedMessageBox(3000, "Defaults Set", "CPD and Ackermann set to defaults");
            hsbarCountsPerDegree.Value = 100;
            hsbarAckerman.Value = 100;

            tabWiz.SelectedIndex = (tabWiz.SelectedIndex + 1 < tabWiz.TabCount) ?
             tabWiz.SelectedIndex + 1 : tabWiz.SelectedIndex;
        }

        private void btnRemoveWasOffset_Click(object sender, EventArgs e)
        {
            hsbarWasOffset.Value = 0;
        }

        private void btnRestartWizard_Click(object sender, EventArgs e)
        {
            isWizardStarted = false;
            FreeDrive(false);
            tabWiz.SelectedIndex = 0;
        }

        private void tabMotorDirection_Enter(object sender, EventArgs e)
        {
            FreeDrive(true);
        }

        private void tabMotorDirection_Leave(object sender, EventArgs e)
        {
            FreeDrive(false);
        }

        private void FreeDrive(bool isOn)
        {
            if (isOn)
            {
                mf.vehicle.isInFreeDriveMode = true;
                mf.vehicle.driveFreeSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
            else
            {
                mf.vehicle.isInFreeDriveMode = false;
                mf.vehicle.driveFreeSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
        }

        #endregion Wizard

        #region Gain

        private void hsbarMinPWM_ValueChanged(object sender, EventArgs e)
        {
            lblMinPWM.Text = unchecked((byte)hsbarMinPWM.Value).ToString();
            hsbarLowSteerPWM.Value = hsbarMinPWM.Value + 2;
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        private void hsbarProportionalGain_ValueChanged(object sender, EventArgs e)
        {
            lblProportionalGain.Text = unchecked((byte)hsbarProportionalGain.Value).ToString();
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        private void hsbarLowSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarHighSteerPWM.Value = hsbarLowSteerPWM.Value;
            lblLowSteerPWM.Text = unchecked((byte)hsbarLowSteerPWM.Value).ToString();
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        private void hsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarLowSteerPWM.Value = hsbarHighSteerPWM.Value;
            lblHighSteerPWM.Text = unchecked((byte)hsbarHighSteerPWM.Value).ToString();
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        #endregion Gain

        #region WAS

        private void hsbarAckerman_ValueChanged(object sender, EventArgs e)
        {
            lblAckerman.Text = unchecked((byte)hsbarAckerman.Value).ToString();

            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
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
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        private void hsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = (hsbarWasOffset.Value / (double)(hsbarCountsPerDegree.Value)).ToString("N2");
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        private void btnZeroWAS_Click(object sender, EventArgs e)
        {
            int offset = (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees + hsbarWasOffset.Value);
            if (Math.Abs(offset) > 3900) mf.TimedMessageBox(2000, "Exceeded Range", "Excessive Steer Angle - Cannot Zero");
            else
            {
                hsbarWasOffset.Value += (int)(hsbarCountsPerDegree.Value * -mf.mc.actualSteerAngleDegrees);
            }
            if (isWizardStarted)
            {
                toSend252 = true;
                counter252 = 0;
            }
        }

        private void btnStartSA_Click(object sender, EventArgs e)
        {
            if (!isSARight)
            {
                isSARight = true;
                startFix = mf.pivotAxlePos;
                dist = 0;
                diameter = 0;
                cntr = 0;
                btnStartSA.Image = Properties.Resources.boundaryStop;
                lblDiameter.Text = "0";
                lblCalcSteerAngleInner.Text = "Drive Steady";
                lblRightStartAngle.Text = mf.mc.actualSteerAngleDegrees.ToString("N1");
            }
            else
            {
                isSARight = false;
                lblCalcSteerAngleInner.Text = "0.0" + "\u00B0";
                btnStartSA.Image = Properties.Resources.BoundaryRecord;
                lblRightStartAngle.Text = "..." + "\u00B0";
            }

            lblCPDError.Text = "...";
        }

        private void btnStartSA_Left_Click(object sender, EventArgs e)
        {
            if (!isSALeft)
            {
                isSALeft = true;
                startFix = mf.pivotAxlePos;
                startAngleLeft = mf.mc.actualSteerAngleDegrees;
                dist = 0;
                diameter = 0;
                cntr = 0;
                btnStartSA_Left.Image = Properties.Resources.boundaryStop;
                lblDiameterLeft.Text = "0";
                lblCalcSteerAngleLeft.Text = "Drive Steady";
                lblStartAngleLeft.Text = startAngleLeft.ToString("N1") + "\u00B0";
            }
            else
            {
                isSALeft = false;
                lblCalcSteerAngleLeft.Text = "..." + "\u00B0";
                btnStartSA_Left.Image = Properties.Resources.BoundaryRecord;
            }

            lblAckermannError.Text = "...";
        }

        #endregion WAS

        #region Stanley

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

        #endregion Stanley

        #region Pure

        private void hsbarIntegralPurePursuit_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.purePursuitIntegralGain = hsbarIntegralPurePursuit.Value * 0.01;
            lblPureIntegral.Text = hsbarIntegralPurePursuit.Value.ToString();
        }

        private void hsbarSideHillComp_ValueChanged(object sender, EventArgs e)
        {
            double deg = hsbarSideHillComp.Value;
            deg *= 0.01;
            lblSideHillComp.Text = (deg.ToString("N2") + "\u00B0");
            Properties.Settings.Default.setAS_sideHillComp = deg;
            mf.gyd.sideHillCompFactor = deg;
        }

        //private void hsbarLookAhead_ValueChanged(object sender, EventArgs e)
        //{
        //    mf.vehicle.goalPointLookAhead = hsbarLookAhead.Value * 0.1;
        //    lblLookAhead.Text = mf.vehicle.goalPointLookAhead.ToString();
        //    //mf.AutoSteerSettingsOutToPort();
        //}

        private void btnOkSetMaximumSteerAngle_Click(object sender, EventArgs e)
        {
            if (Math.Abs((int)mf.mc.actualSteerAngleDegrees) < 5)
            {
                mf.TimedMessageBox(1500, "Steer Angle Too Low", "Must be Greater than 5 degrees");
                return;
            }

            hsbarMaxSteerAngle.Value = Math.Abs((int)(mf.mc.actualSteerAngleDegrees));
        }

        private void hsbarLookAheadMult_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadMult = hsbarLookAheadMult.Value * 0.1;
            lblLookAheadMult.Text = mf.vehicle.goalPointLookAheadMult.ToString();
        }

        #endregion Pure

        #region MinMovement

        private void btnMinGainLeft_Click(object sender, EventArgs e)
        {
            if (CheckSteerSwitch())
                mf.vehicle.driveFreeSteerAngle -= 2;
            else
                mf.TimedMessageBox(1500, "Steering Disabled", "Enable Steer Switch");
        }

        private void btnMinGainRight_Click(object sender, EventArgs e)
        {
            if (CheckSteerSwitch())
                mf.vehicle.driveFreeSteerAngle += 2;
            else
                mf.TimedMessageBox(1500, "Steering Disabled", "Enable Steer Switch");
        }

        private void btnZeroMinMovementSetting_Click(object sender, EventArgs e)
        {
            if (CheckSteerSwitch())
                mf.vehicle.driveFreeSteerAngle = 0;
            else
                mf.TimedMessageBox(1500, "Steering Disabled", "Enable Steer Switch");
        }

        private void tab_MinimumGain_Enter(object sender, EventArgs e)
        {
            hsbarProportionalGain.Value = 1;
            FreeDrive(true);
            mf.TimedMessageBox(2000, "P Gain Change", "Proportional Gain set to 1");
        }

        private void tab_MinimumGain_Leave(object sender, EventArgs e)
        {
            FreeDrive(false);
            hsbarProportionalGain.Value = 40;
        }

        private void tabPGain_Enter(object sender, EventArgs e)
        {
            FreeDrive(true);
        }

        private void tabPGain_Leave(object sender, EventArgs e)
        {
            FreeDrive(false);
        }

        private void btnZeroPGain_Click(object sender, EventArgs e)
        {
            if (mf.vehicle.driveFreeSteerAngle == 0)
                mf.vehicle.driveFreeSteerAngle = 5;
            else mf.vehicle.driveFreeSteerAngle = 0;
        }

        private void btnLeftPGain_Click(object sender, EventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle--;
            if (mf.vehicle.driveFreeSteerAngle < -40) mf.vehicle.driveFreeSteerAngle = -40;
        }

        private void btnRightPGain_Click(object sender, EventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle++;
            if (mf.vehicle.driveFreeSteerAngle > 40) mf.vehicle.driveFreeSteerAngle = 40;
        }

        private void nudWheelbase_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_wheelbase = (double)nudWheelbase.Value * mf.inchOrCm2m;
                mf.vehicle.wheelbase = Properties.Settings.Default.setVehicle_wheelbase;
                Properties.Settings.Default.Save();
            }
        }

        private void nudVehicleTrack_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_trackWidth = (double)nudVehicleTrack.Value * mf.inchOrCm2m;
                mf.vehicle.trackWidth = Properties.Settings.Default.setVehicle_trackWidth;
                mf.tram.halfWheelTrack = mf.vehicle.trackWidth * 0.5;
                Properties.Settings.Default.Save();
            }
        }

        private void nudAntennaPivot_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_antennaPivot = (double)nudAntennaPivot.Value * mf.inchOrCm2m;
                mf.vehicle.antennaPivot = Properties.Settings.Default.setVehicle_antennaPivot;
                Properties.Settings.Default.Save();
            }
        }

        private void btnAckReset_Click(object sender, EventArgs e)
        {
            hsbarAckerman.Value = 100;
        }

        private void cboxDataInvertRoll_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setIMU_invertRoll = cboxDataInvertRoll.Checked;
            mf.ahrs.isRollInvert = Properties.Settings.Default.setIMU_invertRoll;
        }

        private void btnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.imuRoll != 88888)
            {
                mf.ahrs.imuRoll += mf.ahrs.rollZero;
                mf.ahrs.rollZero = mf.ahrs.imuRoll;
                lblRollZeroOffset.Text = (mf.ahrs.rollZero).ToString("N2");
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }

            Properties.Settings.Default.setIMU_rollZero = mf.ahrs.rollZero;
        }

        private void btnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            mf.ahrs.rollZero = 0;
            lblRollZeroOffset.Text = "0.00";
            Properties.Settings.Default.setIMU_rollZero = mf.ahrs.rollZero;
        }

        private void nudAntennaHeight_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_antennaHeight = (double)nudAntennaHeight.Value * mf.inchOrCm2m;
                mf.vehicle.antennaHeight = Properties.Settings.Default.setVehicle_antennaHeight;
                Properties.Settings.Default.Save();
            }
        }

        private void nudAntennaOffset_Click(object sender, EventArgs e)
        {
            if (mf.KeypadToNUD((NudlessNumericUpDown)sender, this))
            {
                Properties.Settings.Default.setVehicle_antennaOffset = (double)nudAntennaOffset.Value * mf.inchOrCm2m;
                mf.vehicle.antennaOffset = Properties.Settings.Default.setVehicle_antennaOffset;
                Properties.Settings.Default.Save();
            }
        }

        private bool CheckSteerSwitch()
        {
            return (btnSteerStatus.BackColor == Color.Yellow);
        }

        #endregion MinMovement

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
                btnFreeDriveZero.Enabled = false;
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
                btnFreeDriveZero.Enabled = true;
                mf.vehicle.driveFreeSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
        }

        private void btnSteerLeft_Click(object sender, EventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle--;
            if (mf.vehicle.driveFreeSteerAngle < -40) mf.vehicle.driveFreeSteerAngle = -40;
        }

        private void btnSteerRight_Click(object sender, EventArgs e)
        {
            mf.vehicle.driveFreeSteerAngle++;
            if (mf.vehicle.driveFreeSteerAngle > 40) mf.vehicle.driveFreeSteerAngle = 40;
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

        #endregion Free Drive
    }
}
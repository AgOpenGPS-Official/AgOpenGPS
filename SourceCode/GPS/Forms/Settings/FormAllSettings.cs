//Please, if you use this, share the improvements

using AgLibrary.Logging;
using AgOpenGPS.Properties;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Reflection.Emit;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormAllSettings : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        public FormAllSettings(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
        }

        private void LoadLabels()
        {
            label4.Text = Properties.Settings.Default.setVehicle_maxSteerAngle.ToString();
            label6.Text = Properties.Settings.Default.setAS_countsPerDegree.ToString();
            label8.Text = Properties.Settings.Default.setAS_ackerman.ToString();
            label10.Text = Properties.Settings.Default.setAS_wasOffset.ToString();
            label12.Text = Properties.Settings.Default.setAS_highSteerPWM.ToString();
            label14.Text = Properties.Settings.Default.setAS_lowSteerPWM.ToString();
            label16.Text = Properties.Settings.Default.setAS_minSteerPWM.ToString();
            label18.Text = Properties.Settings.Default.setAS_Kp.ToString();
            label20.Text = Properties.Settings.Default.setVehicle_panicStopSpeed.ToString();
           
            label22.Text = Properties.Settings.Default.setVehicle_goalPointAcquireFactor.ToString("N2");
            label24.Text = Properties.Settings.Default.setVehicle_goalPointLookAheadHold.ToString();
            label168.Text = Properties.Settings.Default.setVehicle_goalPointLookAheadMult.ToString();
            label26.Text = Properties.Settings.Default.stanleyHeadingErrorGain.ToString();
            label28.Text = Properties.Settings.Default.stanleyDistanceErrorGain.ToString();
            label30.Text = Properties.Settings.Default.stanleyIntegralGainAB.ToString();
            label32.Text = Properties.Settings.Default.setAS_sideHillComp.ToString();
            label34.Text = Properties.Settings.Default.setVehicle_wheelbase.ToString();
            label36.Text = Properties.Settings.Default.setVehicle_trackWidth.ToString();
            label38.Text = Properties.Settings.Default.setVehicle_antennaPivot.ToString();
            label40.Text = Properties.Settings.Default.setVehicle_antennaHeight.ToString();
            label42.Text = Properties.Settings.Default.setVehicle_antennaOffset.ToString();
            label46.Text = Properties.Settings.Default.setIMU_rollZero.ToString();
            label48.Text = Properties.Settings.Default.purePursuitIntegralGainAB.ToString();
            label50.Text = Properties.Settings.Default.setAS_snapDistance.ToString();
            label52.Text = Properties.Settings.Default.setAS_snapDistanceRef.ToString();
            label56.Text = Properties.Settings.Default.setDisplay_isAutoStartAgIO.ToString();
            label58.Text = Properties.Settings.Default.setDisplay_isAutoOffAgIO.ToString();

            label60.Text = RegistrySettings.culture;
            label64.Text = Properties.Settings.Default.setF_isRemoteWorkSystemOn.ToString();
            label66.Text = Properties.Settings.Default.setF_isSteerWorkSwitchEnabled.ToString(); 
            label68.Text = Properties.Settings.Default.setF_isSteerWorkSwitchManualSections.ToString();
            label70.Text = Properties.Settings.Default.setF_isWorkSwitchActiveLow.ToString();
            label72.Text = Properties.Settings.Default.setF_isWorkSwitchEnabled.ToString();
            label74.Text = Properties.Settings.Default.setF_isWorkSwitchManualSections.ToString();
            label76.Text = Properties.Settings.Default.setF_minHeadingStepDistance.ToString();
            label78.Text = RegistrySettings.vehiclesDirectory + " -> " 
                    + RegistrySettings.vehicleFileName + ".xml";
            
            label80.Text = Properties.Settings.Default.setGPS_ageAlarm.ToString();
            label82.Text = Properties.Settings.Default.setGPS_dualHeadingOffset.ToString();
            label84.Text = Properties.Settings.Default.setGPS_dualReverseDetectionDistance.ToString();
            label88.Text = Properties.Settings.Default.setGPS_headingFromWhichSource.ToString();
            label92.Text = Properties.Settings.Default.setGPS_isRTK.ToString();
            label94.Text = Properties.Settings.Default.setGPS_isRTK_KillAutoSteer.ToString();
            label96.Text = Properties.Settings.Default.setGPS_minimumStepLimit.ToString();
            label98.Text = Properties.Settings.Default.setHeadland_isSectionControlled.ToString();
            label100.Text = Properties.Settings.Default.setIMU_fusionWeight2.ToString();
            label102.Text = Properties.Settings.Default.setIMU_invertRoll.ToString();
            label104.Text = Properties.Settings.Default.setIMU_isDualAsIMU.ToString();
            label106.Text = Properties.Settings.Default.setIMU_isReverseOn.ToString();
            label108.Text = Properties.Settings.Default.setIMU_rollFilter.ToString();
            label110.Text = Properties.Settings.Default.setSection_isFast.ToString();
            label112.Text = Properties.Settings.Default.setTool_isSectionOffWhenOut.ToString();
            label114.Text = Properties.Settings.Default.setTool_isSectionsNotZones.ToString();
            label116.Text = Properties.Settings.Default.setTool_isToolFront.ToString();
            label118.Text = Properties.Settings.Default.setTool_isToolRearFixed.ToString();
            label120.Text = Properties.Settings.Default.setTool_isToolTBT.ToString();
            label122.Text = Properties.Settings.Default.setTool_isToolTrailing.ToString();

            label124.Text = Properties.Settings.Default.setTool_toolTrailingHitchLength.ToString();
            label126.Text = Properties.Settings.Default.setTool_trailingToolToPivotLength.ToString();
            label128.Text = Properties.Settings.Default.setVehicle_hitchLength.ToString();
            label130.Text = Properties.Settings.Default.setVehicle_hydraulicLiftLookAhead.ToString();
            label132.Text = "*****";
            label134.Text = Properties.Settings.Default.setVehicle_isStanleyUsed.ToString();
            label136.Text = "*****";
            label138.Text = Properties.Settings.Default.setVehicle_maxAngularVelocity.ToString();
            label140.Text = Properties.Settings.Default.set_youTurnRadius.ToString();
            label142.Text = Properties.Settings.Default.setVehicle_numSections.ToString();
            label144.Text = Properties.Settings.Default.setVehicle_slowSpeedCutoff.ToString();
            label146.Text = Properties.Settings.Default.setVehicle_tankTrailingHitchLength.ToString();
            label148.Text = Properties.Settings.Default.setVehicle_toolLookAheadOn.ToString();
            label150.Text = Properties.Settings.Default.setVehicle_toolLookAheadOff.ToString();
            label152.Text = Properties.Settings.Default.setVehicle_toolOffDelay.ToString();
            label154.Text = Properties.Settings.Default.setVehicle_toolOffset.ToString();
            label156.Text = Properties.Settings.Default.setVehicle_toolOverlap.ToString();
            label158.Text = Properties.Settings.Default.setVehicle_toolWidth.ToString();
            label162.Text = Properties.Settings.Default.setVehicle_vehicleType.ToString();
            label164.Text = Properties.Settings.Default.setAS_isSteerInReverse.ToString();

            label251.Text = Settings.Default.setAS_deadZoneDelay.ToString();
            label252.Text = Settings.Default.setAS_deadZoneHeading.ToString();

            lblFrameTime.Text = mf.frameTime.ToString("N1");
            lblTimeSlice.Text = (1 / mf.timeSliceOfLastFix).ToString("N3");
            lblHz.Text = mf.gpsHz.ToString("N1");

            lblEastingField.Text = Math.Round(mf.pn.fix.easting, 2).ToString();
            lblNorthingField.Text = Math.Round(mf.pn.fix.northing, 2).ToString();

            //lblLatitude.Text = mf.Latitude;
            //lblLongitude.Text = mf.Longitude;

            //lblEastingField2.Text = Math.Round(mf.pnTwo.fix.easting, 2).ToString();
            //lblNorthingField2.Text = Math.Round(mf.pnTwo.fix.northing, 2).ToString();

            //lblLatitude2.Text = mf.pnTwo.latitude.ToString("N7");
            //lblLongitude2.Text = mf.pnTwo.longitude.ToString("N7");

            //other sat and GPS info
            lblSatsTracked.Text = mf.SatsTracked;
            lblHDOP.Text = mf.HDOP;

            lblIMUHeading.Text = mf.GyroInDegrees;
            lblFix2FixHeading.Text = mf.GPSHeading;
            lblFuzeHeading.Text = (mf.fixHeading * 57.2957795).ToString("N1");

            lblAngularVelocity.Text = mf.ahrs.imuYawRate.ToString("N2");

            lbludpWatchCounts.Text = mf.missedSentenceCount.ToString();

            if (mf.isMetric)
            {
                lblAltitude.Text = mf.Altitude;
            }
            else //imperial
            {
                lblAltitude.Text = mf.AltitudeFeet;
            }

            label254.Text = mf.FixQuality;
        }

        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bm, new Rectangle(0, 0, this.Width, this.Height));
            Clipboard.SetImage(bm);
            mf.TimedMessageBox(2000, "Captured", "Copied to Clipboard, Paste (CTRL-V) in Telegram");
            Log.EventWriter("View All Settings to Clipboard");
        }

        private void btnCreatePNG_Click(object sender, EventArgs e)
        {
            Bitmap bm = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bm, new Rectangle(0, 0, this.Width, this.Height));
            bm.Save(Path.Combine(RegistrySettings.baseDirectory, "AllSet.PNG"), ImageFormat.Png);
            System.Diagnostics.Process.Start("explorer.exe", RegistrySettings.baseDirectory);
            Log.EventWriter("View All Settings to PNG");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadLabels();
        }
    }
}
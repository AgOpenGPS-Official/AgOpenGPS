//Please, if you use this, share the improvements

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Forms;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public partial class FormAllSettings : Form
    {
        private readonly FormGPS mf = null;

        // Defaults: fresh instances met velddefaults voor vergelijking
        private static readonly VehicleSettings vsDefault = new VehicleSettings();
        private static readonly ToolSettings tsDefault = new ToolSettings();
        private static readonly Settings esDefault = new Settings();

        private static readonly Color ColorChanged = Color.LightYellow;
        private static readonly Color ColorNormal = Color.WhiteSmoke;
        private static readonly Color ColorHeader = Color.FromArgb(200, 220, 240);

        public FormAllSettings(Form callingForm)
        {
            mf = callingForm as FormGPS;
            InitializeComponent();
            SetupGrids();
            PopulateAllSettings();
            UpdateHeader();
        }

        // ── Grid setup (eenmalig) ──────────────────────────────────────────────

        private void SetupGrids()
        {
            SetupGrid(dgvVehicleL);
            SetupGrid(dgvVehicleM);
            SetupGrid(dgvVehicleR);
            SetupGrid(dgvToolL);
            SetupGrid(dgvToolM);
            SetupGrid(dgvToolR);
            SetupGrid(dgvEnvironmentL);
            SetupGrid(dgvEnvironmentM);
            SetupGrid(dgvEnvironmentR);
            SetupSystemGrid(dgvSystem);
        }

        private static void SetupGrid(DataGridView dgv)
        {
            dgv.Columns.Clear();
            // Kolom volgorde: Setting | Profile Value | Default (Default als laatste)
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSetting", HeaderText = "Setting", Width = 140, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colCurrent", HeaderText = "Profile", Width = 70, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colDefault", HeaderText = "Default", Width = 70, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns["colDefault"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 22;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        }

        private static void SetupSystemGrid(DataGridView dgv)
        {
            dgv.Columns.Clear();
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colSetting", HeaderText = "Metric", Width = 280, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { Name = "colValue", HeaderText = "Value", Width = 400, SortMode = DataGridViewColumnSortMode.NotSortable });
            dgv.Columns["colValue"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.EnableHeadersVisualStyles = false;
            dgv.RowTemplate.Height = 22;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 250);
        }

        // ── Helpers ───────────────────────────────────────────────────────────

        private static void AddHeader(DataGridView dgv, string title)
        {
            int i = dgv.Rows.Add(title, "", "");
            var style = dgv.Rows[i].DefaultCellStyle;
            style.BackColor = ColorHeader;
            style.Font = new Font("Tahoma", 10F, FontStyle.Bold);
            style.ForeColor = Color.DarkBlue;
        }

        private static void AddRow(DataGridView dgv, string name, object def, object current)
        {
            // Volgorde in grid: Setting | Profile | Default
            int i = dgv.Rows.Add(name, current?.ToString() ?? "", def?.ToString() ?? "");
            string ds = def?.ToString() ?? "";
            string cs = current?.ToString() ?? "";
            dgv.Rows[i].DefaultCellStyle.BackColor = ds != cs ? ColorChanged : ColorNormal;
        }

        private static void AddSysRow(DataGridView dgv, string name, object value)
        {
            dgv.Rows.Add(name, value?.ToString() ?? "");
        }

        // ── Populeren ─────────────────────────────────────────────────────────

        private void UpdateHeader()
        {
            lblVehicleName.Text = "Vehicle: " + RegistrySettings.vehicleFileName;
            lblToolName.Text = "Tool: " + RegistrySettings.toolFileName;
            lblEnvironmentName.Text = "Environment: " + (RegistrySettings.environmentFileName ?? "Default");
        }

        private void PopulateAllSettings()
        {
            PopulateVehicle(dgvVehicleL, dgvVehicleM, dgvVehicleR, VehicleSettings.Default);
            PopulateTool(dgvToolL, dgvToolM, dgvToolR, ToolSettings.Default);
            PopulateEnvironment(dgvEnvironmentL, dgvEnvironmentM, dgvEnvironmentR, Settings.Default);
        }

        // ── Vehicle: L = Steer + Arduino, M = Geometry, R = GPS + IMU + Brand ──

        private static void PopulateVehicle(DataGridView left, DataGridView mid, DataGridView right, VehicleSettings vs)
        {
            left.Rows.Clear();
            AddHeader(left, "── Steer");
            AddRow(left, "Max Steer Angle", vsDefault.setVehicle_maxSteerAngle, vs.setVehicle_maxSteerAngle);
            AddRow(left, "Max Angular Vel.", vsDefault.setVehicle_maxAngularVelocity, vs.setVehicle_maxAngularVelocity);
            AddRow(left, "Panic Stop Speed", vsDefault.setVehicle_panicStopSpeed, vs.setVehicle_panicStopSpeed);
            AddRow(left, "Steer In Reverse", vsDefault.setAS_isSteerInReverse, vs.setAS_isSteerInReverse);
            AddRow(left, "Side Hill Comp", vsDefault.setAS_sideHillComp, vs.setAS_sideHillComp);
            AddRow(left, "Dead Zone Heading", vsDefault.setAS_deadZoneHeading, vs.setAS_deadZoneHeading);
            AddRow(left, "Dead Zone Delay", vsDefault.setAS_deadZoneDelay, vs.setAS_deadZoneDelay);

            AddHeader(left, "── Arduino Steer");
            AddRow(left, "Counts/Degree", vsDefault.setAS_countsPerDegree, vs.setAS_countsPerDegree);
            AddRow(left, "Ackerman", vsDefault.setAS_ackerman, vs.setAS_ackerman);
            AddRow(left, "WAS Offset", vsDefault.setAS_wasOffset, vs.setAS_wasOffset);
            AddRow(left, "High Steer PWM", vsDefault.setAS_highSteerPWM, vs.setAS_highSteerPWM);
            AddRow(left, "Low Steer PWM", vsDefault.setAS_lowSteerPWM, vs.setAS_lowSteerPWM);
            AddRow(left, "Min Steer PWM", vsDefault.setAS_minSteerPWM, vs.setAS_minSteerPWM);
            AddRow(left, "Kp (Proportional)", vsDefault.setAS_Kp, vs.setAS_Kp);
            AddRow(left, "Min Steer Speed", vsDefault.setAS_minSteerSpeed, vs.setAS_minSteerSpeed);
            AddRow(left, "Max Steer Speed", vsDefault.setAS_maxSteerSpeed, vs.setAS_maxSteerSpeed);
            AddRow(left, "Func Speed Limit", vsDefault.setAS_functionSpeedLimit, vs.setAS_functionSpeedLimit);

            mid.Rows.Clear();
            AddHeader(mid, "── Geometry");
            AddRow(mid, "Vehicle Type", vsDefault.setVehicle_vehicleType, vs.setVehicle_vehicleType);
            AddRow(mid, "Wheelbase", vsDefault.setVehicle_wheelbase, vs.setVehicle_wheelbase);
            AddRow(mid, "Track Width", vsDefault.setVehicle_trackWidth, vs.setVehicle_trackWidth);
            AddRow(mid, "Antenna Pivot", vsDefault.setVehicle_antennaPivot, vs.setVehicle_antennaPivot);
            AddRow(mid, "Antenna Offset", vsDefault.setVehicle_antennaOffset, vs.setVehicle_antennaOffset);
            AddRow(mid, "Antenna Height", vsDefault.setVehicle_antennaHeight, vs.setVehicle_antennaHeight);
            AddRow(mid, "Hitch Length", vsDefault.setVehicle_hitchLength, vs.setVehicle_hitchLength);

            right.Rows.Clear();
            AddHeader(right, "── GPS");
            AddRow(right, "Heading Source", vsDefault.setGPS_headingFromWhichSource, vs.setGPS_headingFromWhichSource);
            AddRow(right, "Dual Hdg Offset", vsDefault.setGPS_dualHeadingOffset, vs.setGPS_dualHeadingOffset);
            AddRow(right, "Dual Rev. Dist.", vsDefault.setGPS_dualReverseDetectionDistance, vs.setGPS_dualReverseDetectionDistance);
            AddRow(right, "Min Step Limit", vsDefault.setGPS_minimumStepLimit, vs.setGPS_minimumStepLimit);

            AddHeader(right, "── IMU");
            AddRow(right, "Roll Zero", vsDefault.setIMU_rollZero, vs.setIMU_rollZero);
            AddRow(right, "Roll Filter", vsDefault.setIMU_rollFilter, vs.setIMU_rollFilter);
            AddRow(right, "Fusion Weight", vsDefault.setIMU_fusionWeight2, vs.setIMU_fusionWeight2);
            AddRow(right, "Invert Roll", vsDefault.setIMU_invertRoll, vs.setIMU_invertRoll);
            AddRow(right, "Dual As IMU", vsDefault.setIMU_isDualAsIMU, vs.setIMU_isDualAsIMU);

            AddHeader(right, "── Brand");
            AddRow(right, "Tractor Brand", vsDefault.setBrand_TBrand, vs.setBrand_TBrand);
            AddRow(right, "Harvester Brand", vsDefault.setBrand_HBrand, vs.setBrand_HBrand);
            AddRow(right, "Articulated Brand", vsDefault.setBrand_WDBrand, vs.setBrand_WDBrand);
        }

        // ── Tool: L = Dimensions + Type, M = Sections + Lookahead + Guidance, R = Tram + Work Switch ──

        private static void PopulateTool(DataGridView left, DataGridView mid, DataGridView right, ToolSettings ts)
        {
            left.Rows.Clear();
            AddHeader(left, "── Dimensions");
            AddRow(left, "Tool Width", tsDefault.setVehicle_toolWidth, ts.setVehicle_toolWidth);
            AddRow(left, "Tool Overlap", tsDefault.setVehicle_toolOverlap, ts.setVehicle_toolOverlap);
            AddRow(left, "Tool Offset", tsDefault.setVehicle_toolOffset, ts.setVehicle_toolOffset);
            AddRow(left, "Num Sections", tsDefault.setVehicle_numSections, ts.setVehicle_numSections);
            AddRow(left, "Trailing Hitch", tsDefault.setVehicle_toolTrailingHitchLength, ts.setVehicle_toolTrailingHitchLength);
            AddRow(left, "Tank Trailing", tsDefault.setVehicle_tankTrailingHitchLength, ts.setVehicle_tankTrailingHitchLength);
            AddRow(left, "Trailing To Pivot", tsDefault.setTool_trailingToolToPivotLength, ts.setTool_trailingToolToPivotLength);

            AddHeader(left, "── Tool Type");
            AddRow(left, "Is Trailing", tsDefault.setTool_isToolTrailing, ts.setTool_isToolTrailing);
            AddRow(left, "Is Rear Fixed", tsDefault.setTool_isToolRearFixed, ts.setTool_isToolRearFixed);
            AddRow(left, "Is TBT", tsDefault.setTool_isToolTBT, ts.setTool_isToolTBT);
            AddRow(left, "Is Front", tsDefault.setTool_isToolFront, ts.setTool_isToolFront);

            mid.Rows.Clear();
            AddHeader(mid, "── Sections");
            AddRow(mid, "Sections Not Zones", tsDefault.setTool_isSectionsNotZones, ts.setTool_isSectionsNotZones);
            AddRow(mid, "Off When Out", tsDefault.setTool_isSectionOffWhenOut, ts.setTool_isSectionOffWhenOut);
            AddRow(mid, "Fast Section", tsDefault.setSection_isFast, ts.setSection_isFast);

            AddHeader(mid, "── Look Ahead / Timing");
            AddRow(mid, "Look Ahead On", tsDefault.setVehicle_toolLookAheadOn, ts.setVehicle_toolLookAheadOn);
            AddRow(mid, "Look Ahead Off", tsDefault.setVehicle_toolLookAheadOff, ts.setVehicle_toolLookAheadOff);
            AddRow(mid, "Tool Off Delay", tsDefault.setVehicle_toolOffDelay, ts.setVehicle_toolOffDelay);
            AddRow(mid, "Hydraulic Lift LA", tsDefault.setVehicle_hydraulicLiftLookAhead, ts.setVehicle_hydraulicLiftLookAhead);
            AddRow(mid, "Headland Sect. Ctrl", tsDefault.setHeadland_isSectionControlled, ts.setHeadland_isSectionControlled);

            AddHeader(mid, "── Guidance (per Tool)");
            AddRow(mid, "Stanley Used", tsDefault.setVehicle_isStanleyUsed, ts.setVehicle_isStanleyUsed);
            AddRow(mid, "Stanley Dist. Gain", tsDefault.stanleyDistanceErrorGain, ts.stanleyDistanceErrorGain);
            AddRow(mid, "Stanley Hdg. Gain", tsDefault.stanleyHeadingErrorGain, ts.stanleyHeadingErrorGain);
            AddRow(mid, "Stanley Integral", tsDefault.stanleyIntegralGainAB, ts.stanleyIntegralGainAB);
            AddRow(mid, "PurePursuit Intgrl", tsDefault.purePursuitIntegralGainAB, ts.purePursuitIntegralGainAB);
            AddRow(mid, "Goal Pt Acq Factor", tsDefault.setVehicle_goalPointAcquireFactor, ts.setVehicle_goalPointAcquireFactor);
            AddRow(mid, "Goal Pt Hold", tsDefault.setVehicle_goalPointLookAheadHold, ts.setVehicle_goalPointLookAheadHold);
            AddRow(mid, "Goal Pt Mult", tsDefault.setVehicle_goalPointLookAheadMult, ts.setVehicle_goalPointLookAheadMult);
            AddRow(mid, "Slow Speed Cutoff", tsDefault.setVehicle_slowSpeedCutoff, ts.setVehicle_slowSpeedCutoff);

            right.Rows.Clear();
            AddHeader(right, "── Tram");
            AddRow(right, "Tram Width", tsDefault.setTram_tramWidth, ts.setTram_tramWidth);
            AddRow(right, "Tram Passes", tsDefault.setTram_passes, ts.setTram_passes);
            AddRow(right, "Display Tram Ctrl", tsDefault.setTool_isDisplayTramControl, ts.setTool_isDisplayTramControl);
            AddRow(right, "Tram Outer Inv.", tsDefault.setTool_isTramOuterInverted, ts.setTool_isTramOuterInverted);

            AddHeader(right, "── Work Switch");
            AddRow(right, "Steer Work Switch", tsDefault.setF_isSteerWorkSwitchEnabled, ts.setF_isSteerWorkSwitchEnabled);
            AddRow(right, "Min Coverage", tsDefault.setVehicle_minCoverage, ts.setVehicle_minCoverage);
        }

        // ── Environment: L = Display/Menu, M = AutoSteer + UTurn + GPS, R = Sound + Global + Work Switch ──

        private static void PopulateEnvironment(DataGridView left, DataGridView mid, DataGridView right, Settings es)
        {
            left.Rows.Clear();
            AddHeader(left, "── Display / Menu");
            AddRow(left, "Metric", esDefault.setMenu_isMetric, es.setMenu_isMetric);
            AddRow(left, "Grid On", esDefault.setMenu_isGridOn, es.setMenu_isGridOn);
            AddRow(left, "Lightbar On", esDefault.setMenu_isLightbarOn, es.setMenu_isLightbarOn);
            AddRow(left, "Side Guide Lines", esDefault.setMenu_isSideGuideLines, es.setMenu_isSideGuideLines);
            AddRow(left, "Pure On", esDefault.setMenu_isPureOn, es.setMenu_isPureOn);
            AddRow(left, "Simulator On", esDefault.setMenu_isSimulatorOn, es.setMenu_isSimulatorOn);
            AddRow(left, "Speedo On", esDefault.setMenu_isSpeedoOn, es.setMenu_isSpeedoOn);
            AddRow(left, "Lightbar Not SteerBar", esDefault.setMenu_isLightbarNotSteerBar, es.setMenu_isLightbarNotSteerBar);
            AddRow(left, "Day Mode", esDefault.setDisplay_isDayMode, es.setDisplay_isDayMode);
            AddRow(left, "Start Fullscreen", esDefault.setDisplay_isStartFullScreen, es.setDisplay_isStartFullScreen);
            AddRow(left, "Keyboard On", esDefault.setDisplay_isKeyboardOn, es.setDisplay_isKeyboardOn);
            AddRow(left, "Vehicle Image", esDefault.setDisplay_isVehicleImage, es.setDisplay_isVehicleImage);
            AddRow(left, "Texture On", esDefault.setDisplay_isTextureOn, es.setDisplay_isTextureOn);
            AddRow(left, "Brightness On", esDefault.setDisplay_isBrightnessOn, es.setDisplay_isBrightnessOn);
            AddRow(left, "Log Elevation", esDefault.setDisplay_isLogElevation, es.setDisplay_isLogElevation);
            AddRow(left, "Svenn Arrow", esDefault.setDisplay_isSvennArrowOn, es.setDisplay_isSvennArrowOn);
            AddRow(left, "Section Lines", esDefault.setDisplay_isSectionLinesOn, es.setDisplay_isSectionLinesOn);
            AddRow(left, "Line Smooth", esDefault.setDisplay_isLineSmooth, es.setDisplay_isLineSmooth);
            AddRow(left, "Hardware Messages", esDefault.setDisplay_isHardwareMessages, es.setDisplay_isHardwareMessages);
            AddRow(left, "Kiosk Mode", esDefault.setWindow_isKioskMode, es.setWindow_isKioskMode);
            AddRow(left, "Shutdown Computer", esDefault.setWindow_isShutdownComputer, es.setWindow_isShutdownComputer);
            AddRow(left, "Shutdown No Power", esDefault.setDisplay_isShutdownWhenNoPower, es.setDisplay_isShutdownWhenNoPower);
            AddRow(left, "Auto Start AgIO", esDefault.setDisplay_isAutoStartAgIO, es.setDisplay_isAutoStartAgIO);
            AddRow(left, "Auto Off AgIO", esDefault.setDisplay_isAutoOffAgIO, es.setDisplay_isAutoOffAgIO);
            AddRow(left, "Lightbar cm/px", esDefault.setDisplay_lightbarCmPerPixel, es.setDisplay_lightbarCmPerPixel);
            AddRow(left, "Line Width", esDefault.setDisplay_lineWidth, es.setDisplay_lineWidth);
            AddRow(left, "Brightness", esDefault.setDisplay_brightness, es.setDisplay_brightness);
            AddRow(left, "Cam Zoom", esDefault.setDisplay_camZoom, es.setDisplay_camZoom);
            AddRow(left, "Cam Pitch", esDefault.setDisplay_camPitch, es.setDisplay_camPitch);

            mid.Rows.Clear();
            AddHeader(mid, "── AutoSteer");
            AddRow(mid, "Snap Distance", esDefault.setAS_snapDistance, es.setAS_snapDistance);
            AddRow(mid, "Snap Distance Ref", esDefault.setAS_snapDistanceRef, es.setAS_snapDistanceRef);
            AddRow(mid, "Guidance Lookahead", esDefault.setAS_guidanceLookAheadTime, es.setAS_guidanceLookAheadTime);
            AddRow(mid, "AutoSteer Auto On", esDefault.setAS_isAutoSteerAutoOn, es.setAS_isAutoSteerAutoOn);
            AddRow(mid, "Constant Contour", esDefault.setAS_isConstantContourOn, es.setAS_isConstantContourOn);

            AddHeader(mid, "── U-Turn");
            AddRow(mid, "Turn Radius", esDefault.set_youTurnRadius, es.set_youTurnRadius);
            AddRow(mid, "Extension Length", esDefault.set_youTurnExtensionLength, es.set_youTurnExtensionLength);
            AddRow(mid, "Dist From Boundary", esDefault.set_youTurnDistanceFromBoundary, es.set_youTurnDistanceFromBoundary);
            AddRow(mid, "Skip Width", esDefault.set_youSkipWidth, es.set_youSkipWidth);
            AddRow(mid, "U-Turn Style", esDefault.set_uTurnStyle, es.set_uTurnStyle);
            AddRow(mid, "U-Turn Smoothing", esDefault.setAS_uTurnSmoothing, es.setAS_uTurnSmoothing);
            AddRow(mid, "U-Turn Compensation", esDefault.setAS_uTurnCompensation, es.setAS_uTurnCompensation);

            AddHeader(mid, "── GPS");
            AddRow(mid, "GPS Age Alarm", esDefault.setGPS_ageAlarm, es.setGPS_ageAlarm);
            AddRow(mid, "Is RTK", esDefault.setGPS_isRTK, es.setGPS_isRTK);
            AddRow(mid, "RTK Kill AutoSteer", esDefault.setGPS_isRTK_KillAutoSteer, es.setGPS_isRTK_KillAutoSteer);
            AddRow(mid, "Jump Fix Alarm", esDefault.setGPS_jumpFixAlarmDistance, es.setGPS_jumpFixAlarmDistance);
            AddRow(mid, "Sim Latitude", esDefault.setGPS_SimLatitude, es.setGPS_SimLatitude);
            AddRow(mid, "Sim Longitude", esDefault.setGPS_SimLongitude, es.setGPS_SimLongitude);
            AddRow(mid, "UDP Watch ms", esDefault.SetGPS_udpWatchMsec, es.SetGPS_udpWatchMsec);

            right.Rows.Clear();
            AddHeader(right, "── Sound");
            AddRow(right, "U-Turn Sound", esDefault.setSound_isUturnOn, es.setSound_isUturnOn);
            AddRow(right, "Hyd Lift Sound", esDefault.setSound_isHydLiftOn, es.setSound_isHydLiftOn);
            AddRow(right, "AutoSteer Sound", esDefault.setSound_isAutoSteerOn, es.setSound_isAutoSteerOn);
            AddRow(right, "Sections Sound", esDefault.setSound_isSectionsOn, es.setSound_isSectionsOn);

            AddHeader(right, "── IMU / Global");
            AddRow(right, "Reverse On", esDefault.setIMU_isReverseOn, es.setIMU_isReverseOn);
            AddRow(right, "AutoSwitch Dual Fix", esDefault.setAutoSwitchDualFixOn, es.setAutoSwitchDualFixOn);
            AddRow(right, "AutoSwitch Speed", esDefault.setAutoSwitchDualFixSpeed, es.setAutoSwitchDualFixSpeed);
            AddRow(right, "Draw Pivot", esDefault.setBnd_isDrawPivot, es.setBnd_isDrawPivot);
            AddRow(right, "Headland Dist On", esDefault.isHeadlandDistanceOn, es.isHeadlandDistanceOn);
            AddRow(right, "Bnd Tool Spacing", esDefault.bndToolSpacing, es.bndToolSpacing);
            AddRow(right, "Bnd Tool Smooth", esDefault.bndToolSmooth, es.bndToolSmooth);

            AddHeader(right, "── Work Switch");
            AddRow(right, "Work Switch", esDefault.setF_isWorkSwitchEnabled, es.setF_isWorkSwitchEnabled);
            AddRow(right, "Active Low", esDefault.setF_isWorkSwitchActiveLow, es.setF_isWorkSwitchActiveLow);
            AddRow(right, "Manual Sections", esDefault.setF_isWorkSwitchManualSections, es.setF_isWorkSwitchManualSections);
            AddRow(right, "Remote Work Sys", esDefault.setF_isRemoteWorkSystemOn, es.setF_isRemoteWorkSystemOn);
            AddRow(right, "Steer WS Manual", esDefault.setF_isSteerWorkSwitchManualSections, es.setF_isSteerWorkSwitchManualSections);
            AddRow(right, "Min Hdg Step Dist", esDefault.setF_minHeadingStepDistance, es.setF_minHeadingStepDistance);

            AddHeader(right, "── AgShare");
            AddRow(right, "AgShare Enabled", esDefault.AgShareEnabled, es.AgShareEnabled);
            AddRow(right, "Upload Active", esDefault.AgShareUploadActive, es.AgShareUploadActive);
            AddRow(right, "Public Field", esDefault.PublicField, es.PublicField);

            AddHeader(right, "── Other");
            AddRow(right, "Culture", RegistrySettings.culture, RegistrySettings.culture);
        }

        private void PopulateSystem(DataGridView dgv)
        {
            dgv.Rows.Clear();

            AddHeader(dgv, "── GPS / Fix");
            AddSysRow(dgv, "Frame Time (ms)", mf.frameTime.ToString("N1"));
            AddSysRow(dgv, "Time Slice", (1 / mf.timeSliceOfLastFix).ToString("N3"));
            AddSysRow(dgv, "GPS Hz", mf.gpsHz.ToString("N1"));
            AddSysRow(dgv, "Fix Quality", mf.FixQuality);
            AddSysRow(dgv, "Sats Tracked", mf.SatsTracked);
            AddSysRow(dgv, "HDOP", mf.HDOP);
            AddSysRow(dgv, "Altitude", mf.isMetric ? mf.Altitude : mf.AltitudeFeet);
            AddSysRow(dgv, "Easting", Math.Round(mf.pn.fix.easting, 2).ToString());
            AddSysRow(dgv, "Northing", Math.Round(mf.pn.fix.northing, 2).ToString());
            AddSysRow(dgv, "Missed UDP Sentences", mf.missedSentenceCount.ToString());

            AddHeader(dgv, "── Heading");
            AddSysRow(dgv, "IMU Heading", mf.GyroInDegrees);
            AddSysRow(dgv, "Fix-to-Fix Heading", mf.GPSHeading);
            AddSysRow(dgv, "Fused Heading (deg)", (mf.fixHeading * 57.2957795).ToString("N1"));
            AddSysRow(dgv, "Angular Velocity", mf.ahrs.imuYawRate.ToString("N2"));

            AddHeader(dgv, "── Application");
            AddSysRow(dgv, "Version", Program.SemVer);
            AddSysRow(dgv, "Vehicle File", RegistrySettings.vehiclesDirectory + "\\" + RegistrySettings.vehicleFileName + ".xml");
            AddSysRow(dgv, "Tool File", RegistrySettings.toolsDirectory + "\\" + RegistrySettings.toolFileName + ".xml");
        }

        // ── CSV Export ────────────────────────────────────────────────────────

        private void ExportToCSV(string path)
        {
            // Tab-gescheiden zodat het in alle Excel-taalinstellingen correct opent
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                WriteSettingsTabToCSV(sw, "VEHICLE", dgvVehicleL, dgvVehicleM, dgvVehicleR);
                WriteSettingsTabToCSV(sw, "TOOL", dgvToolL, dgvToolM, dgvToolR);
                WriteSettingsTabToCSV(sw, "ENVIRONMENT", dgvEnvironmentL, dgvEnvironmentM, dgvEnvironmentR);
                WriteSettingsTabToCSV(sw, "SYSTEM / GPS", dgvSystem);
            }
        }

        private static void WriteSettingsTabToCSV(StreamWriter sw, string tabName, params DataGridView[] grids)
        {
            sw.WriteLine($"=== {tabName} ===");
            sw.WriteLine();

            bool firstSection = true;
            foreach (var dgv in grids)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    string setting = row.Cells[0].Value?.ToString() ?? "";

                    if (setting.StartsWith("──"))
                    {
                        if (!firstSection) sw.WriteLine();
                        firstSection = false;
                        string sectionName = setting.Replace("──", "").Trim();
                        sw.WriteLine($"--- {sectionName} ---");
                        if (dgv.Columns.Count >= 3)
                            sw.WriteLine("Setting\tProfile Value\tDefault");
                        else
                            sw.WriteLine("Setting\tValue");
                    }
                    else if (!string.IsNullOrWhiteSpace(setting))
                    {
                        if (dgv.Columns.Count >= 3)
                        {
                            string profile = row.Cells[1].Value?.ToString() ?? "";
                            string def = row.Cells[2].Value?.ToString() ?? "";
                            sw.WriteLine($"{setting}\t{profile}\t{def}");
                        }
                        else
                        {
                            string val = row.Cells.Count > 1 ? row.Cells[1].Value?.ToString() ?? "" : "";
                            sw.WriteLine($"{setting}\t{val}");
                        }
                    }
                }
            }

            sw.WriteLine();
            sw.WriteLine();
        }

        private static string CsvEscape(string s)
        {
            if (s.Contains(",") || s.Contains("\"") || s.Contains("\n"))
                return "\"" + s.Replace("\"", "\"\"") + "\"";
            return s;
        }

        // ── Screenshot: capture alle 3 settings tabs en samenvoegen ──────────

        private Bitmap CaptureTabsBitmap()
        {
            int savedTab = tabControl.SelectedIndex;

            Bitmap[] pages = new Bitmap[3];
            for (int t = 0; t < 3; t++)
            {
                tabControl.SelectedIndex = t;
                tabControl.Update();
                Application.DoEvents();
                var page = tabControl.SelectedTab;
                var bm = new Bitmap(page.Width, page.Height);
                page.DrawToBitmap(bm, new Rectangle(0, 0, page.Width, page.Height));
                pages[t] = bm;
            }

            tabControl.SelectedIndex = savedTab;

            int headerH = panelHeader.Height;
            int tabStripH = tabControl.ItemSize.Height + 4;
            int totalH = headerH + (tabStripH + pages[0].Height) * 3;
            int totalW = pages[0].Width;

            var combined = new Bitmap(totalW, totalH);
            using (var g = Graphics.FromImage(combined))
            {
                var headerBm = new Bitmap(panelHeader.Width, panelHeader.Height);
                panelHeader.DrawToBitmap(headerBm, new Rectangle(0, 0, panelHeader.Width, panelHeader.Height));
                g.DrawImage(headerBm, 0, 0);
                headerBm.Dispose();

                int y = headerH;
                string[] labels = { "Vehicle", "Tool", "Environment" };
                using (var headerFont = new Font("Tahoma", 12F, FontStyle.Bold))
                using (var headerBrush = new SolidBrush(Color.SteelBlue))
                using (var textBrush = new SolidBrush(Color.White))
                {
                    for (int t = 0; t < 3; t++)
                    {
                        g.FillRectangle(headerBrush, 0, y, totalW, tabStripH);
                        g.DrawString(labels[t], headerFont, textBrush, 10, y + 8);
                        y += tabStripH;
                        g.DrawImage(pages[t], 0, y);
                        y += pages[t].Height;
                        pages[t].Dispose();
                    }
                }
            }
            return combined;
        }

        // ── Buttons ───────────────────────────────────────────────────────────

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnExportCSV_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(RegistrySettings.baseDirectory, "AllSettings.tsv");
            ExportToCSV(path);
            System.Diagnostics.Process.Start("explorer.exe", RegistrySettings.baseDirectory);
            Log.EventWriter("View All Settings to CSV");
        }

        private void btnScreenShot_Click(object sender, EventArgs e)
        {
            using (var bm = CaptureTabsBitmap())
            {
                Clipboard.SetImage(bm);
            }
            FormDialog.Show("Captured", "Copied to Clipboard, Paste (CTRL-V) in Telegram", DialogSeverity.Info);
            Log.EventWriter("View All Settings to Clipboard");
        }

        private void btnCreatePNG_Click(object sender, EventArgs e)
        {
            string path = Path.Combine(RegistrySettings.baseDirectory, "AllSet.PNG");
            using (var bm = CaptureTabsBitmap())
            {
                bm.Save(path, ImageFormat.Png);
            }
            System.Diagnostics.Process.Start("explorer.exe", RegistrySettings.baseDirectory);
            Log.EventWriter("View All Settings to PNG");
            Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            // Alleen System tab verversen (live GPS data) - geen scroll reset op andere tabs
            UpdateHeader();
            PopulateSystem(dgvSystem);
        }
    }
}

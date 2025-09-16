//Please, if you use this, share the improvements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgLibrary.Logging;
using AgOpenGPS.Classes;
using AgOpenGPS.Controls;
using AgOpenGPS.Core;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Core.ViewModels;
using AgOpenGPS.Forms.Profiles;
using AgOpenGPS.Properties;
using OpenTK.Graphics.OpenGL;
using OpenTK.Mathematics;

namespace AgOpenGPS
{
    //the main form object
    public partial class FormGPS : Form
    {
        public ApplicationCore AppCore { get; }

        public ApplicationModel AppModel => AppCore.AppModel;
        public ApplicationViewModel AppViewModel => AppCore.AppViewModel;

        // Deprecated. Only here to avoid numerous changes to existing code that not has been refactored.
        // Please use AppViewModel.IsMetric directly
        public bool isMetric
        {
            get { return AppViewModel.IsMetric; }
            set
            {
                AppViewModel.IsMetric = value;
            }
        }

        // Deprecated. Only here to avoid numerous changes to existing code that not has been refactored.
        // Please use AppViewModel.IsDay directly
        public bool isDay
        {
            get { return AppViewModel.IsDay; }
            set
            {
                AppViewModel.IsDay = value;
            }
        }

        // Deprecated. Only here to avoid numerous changes to existing code that not has been refactored.
        // Please use AppViewModel.Fields directly
        public string currentFieldDirectory
        {
            get { return AppModel.Fields.CurrentFieldName; }
            set { AppModel.Fields.SetCurrentFieldByName(value); }
        }

        // Deprecated. Only here to avoid numerous changes to existing code that not has been refactored.
        // Please use AppModel.FixHeading directly
        public double fixHeading
        {
            get { return AppModel.FixHeading.AngleInRadians; }
            set { AppModel.FixHeading = new GeoDir(value); }
        }

        public bool isJobStarted => AppModel.Fields.ActiveField != null;

        public string displayFieldName => AppModel.Fields.ActiveField != null ? AppModel.Fields.ActiveField.Name : gStr.gsNone;


        //To bring forward AgIO if running
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        private Task agShareUploadTask = null;


        #region // Class Props and instances

        //maximum sections available
        public const int MAXSECTIONS = 64;

        private bool leftMouseDownOnOpenGL; //mousedown event in opengl window
        public int flagNumberPicked = 0;

        public bool isBtnAutoSteerOn;

        //if we are saving a file
        public bool isSavingFile = false;

        //texture holders
        public ScreenTextures ScreenTextures = new ScreenTextures();
        public VehicleTextures VehicleTextures = new VehicleTextures();

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        private readonly Stopwatch swFrame = new Stopwatch();

        public double secondsSinceStart;
        public double gridToolSpacing;

        //private readonly Stopwatch swDraw = new Stopwatch();
        //swDraw.Reset();
        //swDraw.Start();
        //swDraw.Stop();
        //label3.Text = ((double) swDraw.ElapsedTicks / (double) System.Diagnostics.Stopwatch.Frequency * 1000).ToString();

        //Time to do fix position update and draw routine
        public double frameTime = 0;

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        //private readonly Stopwatch swHz = new Stopwatch();

        //Time to do fix position update and draw routine
        public double gpsHz = 10;

        //whether or not to use Stanley control
        public bool isStanleyUsed = true;

        public double m2InchOrCm, inchOrCm2m, m2FtOrM, ftOrMtoM, cm2CmOrIn, inOrCm2Cm;
        public string unitsFtM, unitsInCm, unitsInCmNS;

        public char[] hotkeys;

        //used by filePicker Form to return picked file and directory
        public string filePickerFileAndDirectory;

        //the position of the GPS Data window within the FormGPS window
        public int GPSDataWindowLeft = 80, GPSDataWindowTopOffset = 220;

        //isGPSData form up
        public bool isGPSSentencesOn = false, isKeepOffsetsOn = false;

        public Camera camera;

        /// <summary>
        /// create world grid
        /// </summary>
        public WorldGrid worldGrid;

        /// <summary>
        /// The NMEA class that decodes it
        /// </summary>
        public CNMEA pn;

        /// <summary>
        /// an array of sections
        /// </summary>
        public CSection[] section;

        /// <summary>
        /// an array of patches to draw
        /// </summary>
        //public CPatches[] triStrip;
        public List<CPatches> triStrip;

        /// <summary>
        /// AB Line object
        /// </summary>
        public CABLine ABLine;

        /// <summary>
        /// TramLine class for boundary and settings
        /// </summary>
        public CTram tram;

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
        public CContour ct;

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
        public CTrack trk;

        /// <summary>
        /// ABCurve instance
        /// </summary>
        public CABCurve curve;

        /// <summary>
        /// Auto Headland YouTurn
        /// </summary>
        public CYouTurn yt;

        /// <summary>
        /// Our vehicle only
        /// </summary>
        public CVehicle vehicle;

        /// <summary>
        /// Just the tool attachment that includes the sections
        /// </summary>
        public CTool tool;

        /// <summary>
        /// All the structs for recv and send of information out ports
        /// </summary>
        public CModuleComm mc;

        /// <summary>
        /// The boundary object
        /// </summary>
        public CBoundary bnd;

        /// <summary>
        /// Building a headland instance
        /// </summary>
        public CHeadLine hdl;

        /// <summary>
        /// The internal simulator
        /// </summary>
        public CSim sim;

        /// <summary>
        /// Heading, Roll, Pitch, GPS, Properties
        /// </summary>
        public CAHRS ahrs;

        /// <summary>
        /// Recorded Path
        /// </summary>
        public CRecordedPath recPath;

        /// <summary>
        /// Most of the displayed field data for GUI
        /// </summary>
        public CFieldData fd;

        ///// <summary>
        ///// Sound
        ///// </summary>
        public CSound sounds;

        public AgOpenGPS.Core.DrawLib.Font font;

        /// <summary>
        /// The new steer algorithms
        /// </summary>
        public CGuidance gyd;

        /// <summary>
        /// The new brightness code
        /// </summary>
        public CWindowsSettingsBrightnessController displayBrightness;

        /// <summary>
        /// AgShare client for uploading fields
        /// </summary>
        private AgShareClient agShareClient;


        #endregion // Class Props and instances

        //The method assigned to the PowerModeChanged event call
        private void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            //We are interested only in StatusChange cases
            if (e.Mode.HasFlag(Microsoft.Win32.PowerModes.StatusChange))
            {
                PowerLineStatus powerLineStatus = SystemInformation.PowerStatus.PowerLineStatus;

                Log.EventWriter($"Power Line Status Change to: {powerLineStatus}");

                if (powerLineStatus == PowerLineStatus.Online)
                {
                    btnChargeStatus.BackColor = Color.YellowGreen;

                    Form f = Application.OpenForms["FormSaveOrNot"];

                    if (f != null)
                    {
                        f.Focus();
                        f.Close();
                    }
                }
                else
                {
                    btnChargeStatus.BackColor = Color.LightCoral;
                }

                if (Settings.Default.setDisplay_isShutdownWhenNoPower && powerLineStatus == PowerLineStatus.Offline)
                {
                    Log.EventWriter("Shutdown Computer By Power Lost Setting");
                    Close();
                }
            }
        }

        public FormGPS()
        {
            //winform initialization
            InitializeComponent();

            InitializeLanguages();

            AppCore = new ApplicationCore(
                new DirectoryInfo(RegistrySettings.baseDirectory),
                null,
                null);

            //time keeper
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            camera = new Camera(
                Properties.Settings.Default.setDisplay_camPitch,
                Properties.Settings.Default.setDisplay_camZoom);

            worldGrid = new WorldGrid(Resources.z_Floor, Resources.z_bingMap);

            //our vehicle made with gl object and pointer of mainform
            vehicle = new CVehicle(this);

            tool = new CTool(this);

            //create a new section and set left and right positions
            //created whether used or not, saves restarting program

            section = new CSection[MAXSECTIONS];
            for (int j = 0; j < MAXSECTIONS; j++) section[j] = new CSection();

            triStrip = new List<CPatches>
            {
                new CPatches(this)
            };

            //our NMEA parser
            pn = new CNMEA(this);

            //create the ABLine instance
            ABLine = new CABLine(this);

            //new instance of contour mode
            ct = new CContour(this);

            //new instance of contour mode
            curve = new CABCurve(this);

            //new track instance
            trk = new CTrack(this);

            //new instance of contour mode
            hdl = new CHeadLine(this);

            ////new instance of auto headland turn
            yt = new CYouTurn(this);

            //module communication
            mc = new CModuleComm(this);

            //boundary object
            bnd = new CBoundary(this);

            //nmea simulator built in.
            sim = new CSim(this);

            ////all the attitude, heading, roll, pitch reference system
            ahrs = new CAHRS();

            //A recorded path
            recPath = new CRecordedPath(this);

            //fieldData all in one place
            fd = new CFieldData(this);

            //start the stopwatch
            //swFrame.Start();

            //instance of tram
            tram = new CTram(this);

            font = new AgOpenGPS.Core.DrawLib.Font(camera, ScreenTextures.Font);

            //the new steer algorithms
            gyd = new CGuidance(this);

            //sounds class
            sounds = new CSound();

            //brightness object class
            displayBrightness = new CWindowsSettingsBrightnessController(Properties.Settings.Default.setDisplay_isBrightnessOn);
        }

        private void FormGPS_Load(object sender, EventArgs e)
        {
            Log.EventWriter("Program Started: "
                + DateTime.Now.ToString("f", CultureInfo.InvariantCulture));
            Log.EventWriter("AOG Version: " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));

            if (!Properties.Settings.Default.setDisplay_isTermsAccepted)
            {
                using (var form = new Form_First(this))
                {
                    if (form.ShowDialog(this) != DialogResult.OK)
                    {
                        Log.EventWriter("Terms Not Accepted");
                        Log.FileSaveSystemEvents();
                        Environment.Exit(0);
                    }
                    else
                    {
                        Log.EventWriter("Terms Accepted");
                    }
                }
            }
            else Log.EventWriter("Terms Already Accepted");

            //The way we subscribe to the System Event to check when Power Mode has changed.
            Microsoft.Win32.SystemEvents.PowerModeChanged += SystemEvents_PowerModeChanged;

            //start udp server is required
            StartLoopbackServer();

            //boundaryToolStripBtn.Enabled = false;
            FieldMenuButtonEnableDisable(false);

            panelRight.Enabled = false;

            oglMain.Left = 75;
            oglMain.Width = this.Width - statusStripLeft.Width - 84;

            panelSim.Left = Width / 2 - 330;
            panelSim.Width = 700;
            panelSim.Top = Height - 60;

            //set the language to last used
            SetLanguage(RegistrySettings.culture);

            //make sure current field directory exists, null if not
            currentFieldDirectory = Settings.Default.setF_CurrentDir;

            Log.EventWriter("Program Directory: " + Application.StartupPath);
            Log.EventWriter("Fields Directory: " + (RegistrySettings.fieldsDirectory));

            if (isBrightnessOn)
            {
                if (displayBrightness.isWmiMonitor)
                {
                    Settings.Default.setDisplay_brightnessSystem = displayBrightness.GetBrightness();
                    Settings.Default.Save();
                }
                else
                {
                    btnBrightnessDn.Enabled = false;
                    btnBrightnessUp.Enabled = false;
                }

                //display brightness
                if (displayBrightness.isWmiMonitor)
                {
                    if (Settings.Default.setDisplay_brightness < Settings.Default.setDisplay_brightnessSystem)
                    {
                        Settings.Default.setDisplay_brightness = Settings.Default.setDisplay_brightnessSystem;
                        Settings.Default.Save();
                    }

                    displayBrightness.SetBrightness(Settings.Default.setDisplay_brightness);
                }
                else
                {
                    btnBrightnessDn.Enabled = false;
                    btnBrightnessUp.Enabled = false;
                }
            }

            // load all the gui elements in gui.designer.cs
            LoadSettings();

            //for field data and overlap
            oglZoom.Width = 400;
            oglZoom.Height = 400;
            oglZoom.Left = 100;
            oglZoom.Top = 100;

            if (RegistrySettings.vehicleFileName != "" && Properties.Settings.Default.setDisplay_isAutoStartAgIO)
            {
                //Start AgIO process
                Process[] processName = Process.GetProcessesByName("AgIO");
                if (processName.Length == 0)
                {
                    //Start application here
                    string strPath = Path.Combine(Application.StartupPath, "AgIO.exe");
                    try
                    {
                        ProcessStartInfo processInfo = new ProcessStartInfo
                        {
                            FileName = strPath,
                            WorkingDirectory = Path.GetDirectoryName(strPath)
                        };
                        Process proc = Process.Start(processInfo);
                        Log.EventWriter("AgIO Started");
                    }
                    catch
                    {
                        TimedMessageBox(2000, "No File Found", "Can't Find AgIO");
                        Log.EventWriter("Can't Find AgIO, File not Found");
                    }
                }
            }

            //nmea limiter
            udpWatch.Start();

            panelDrag.Draggable(true);

            hotkeys = new char[19];

            hotkeys = Properties.Settings.Default.setKey_hotkeys.ToCharArray();

            if (RegistrySettings.vehicleFileName == "")
            {
                Log.EventWriter("No profile selected, prompt to create a new one");

                YesMessageBox("No profile selected\n\nCreate a new profile to save your configuration\n\nIf no profile is created, NO changes will be saved!");

                using (FormNewProfile form = new FormNewProfile(this))
                {
                    form.ShowDialog(this);
                }
            }
            //Init AgShareClient
            agShareClient = new AgShareClient(Settings.Default.AgShareServer, Settings.Default.AgShareApiKey);
        }

        #region Shutdown Handling

        // Centralized shutdown coordinator
        private bool isShuttingDown = false;

        private void FormGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isShuttingDown) return;
            //set the shutdown flag to true to prevent re-entrance
            isShuttingDown = true;

            e.Cancel = true; // Prevent immediate close

            // Close subforms
            string[] formNames = { "FormGPSData", "FormFieldData", "FormPan", "FormTimedMessage" };
            foreach (string name in formNames)
            {
                Form f = Application.OpenForms[name];
                if (f != null && !f.IsDisposed)
                {
                    try { f.Close(); } catch { }
                }
            }

            // Cancel shutdown if owned forms are still open
            if (this.OwnedForms.Any())
            {
                TimedMessageBox(2000, gStr.gsWindowsStillOpen, gStr.gsCloseAllWindowsFirst);
                isShuttingDown = false;
                e.Cancel = true;
                return;
            }

            // Get user choice for shutdown behavior
            int choice = SaveOrNot();
            if (choice == 1)
            {
                isShuttingDown = false;
                e.Cancel = true;
                return;
            }
            // Save and upload field data if applicable
            if (isJobStarted)
            {
                if (autoBtnState == btnStates.Auto)
                    btnSectionMasterAuto.PerformClick();
            }

            BeginInvoke(new Func<Task>(async () => await ShowSavingFormAndShutdown(choice)));
        }


        private async Task ShowSavingFormAndShutdown(int choice)
        {
            using (FormSaving savingForm = new FormSaving())
            {
                if (isJobStarted)
                {
                    bool isAgShareStepEnabled = Settings.Default.AgShareEnabled && Settings.Default.AgShareUploadActive && !isAgShareUploadStarted;

                    savingForm.AddStep("Params", gStr.gsSaveFieldParam);
                    if (isAgShareStepEnabled) savingForm.AddStep("AgShare", gStr.gsSaveUploadToAgshare);
                    savingForm.AddStep("Field", gStr.gsSaveField);
                    savingForm.AddStep("Settings", gStr.gsSaveSettings);
                    savingForm.AddStep("Finalize", gStr.gsSaveFinalizeShutdown);

                    savingForm.Show();

                    await Task.Delay(300); // Let UI settle

                    // STEP 0: Parameters
                    await Task.Delay(300);
                    savingForm.UpdateStep("Params", gStr.gsSaveFieldParamSaved, SavingStepState.Done);

                    // STEP 1: AgShare Upload
                    if (isAgShareStepEnabled)
                    {
                        isAgShareUploadStarted = true;

                        try
                        {
                            agShareUploadTask = CAgShareUploader.UploadAsync(snapshot, agShareClient, this);
                            await agShareUploadTask;
                            savingForm.UpdateStep("AgShare", gStr.gsSaveUploadCompleted, SavingStepState.Done);
                        }
                        catch (Exception ex)
                        {
                            Log.EventWriter("AgShare upload error during shutdown: " + ex.Message);
                            savingForm.UpdateStep("AgShare", gStr.gsSaveUploadFailed, SavingStepState.Failed);
                        }
                    }

                    // STEP 2: Save Field
                    await FileSaveEverythingBeforeClosingField();
                    savingForm.UpdateStep("Field", gStr.gsSaveFieldSavedLocal, SavingStepState.Done);

                    // STEP 3: Settings
                    Settings.Default.Save();
                    await Task.Delay(300);
                    savingForm.UpdateStep("Settings", gStr.gsSaveSettingsSaved, SavingStepState.Done);

                    // STEP 4: Finalizing
                    await Task.Delay(500);
                    savingForm.UpdateStep("Finalize", gStr.gsSaveAllDone, SavingStepState.Done);
                    await Task.Delay(750);
                    savingForm.Finish();
                }
                else
                {
                    savingForm.AddStep("Settings", gStr.gsSaveSettings);
                    savingForm.AddStep("Finalize", gStr.gsSaveFinalizeShutdown);

                    savingForm.Show();

                    await Task.Delay(300); // Let UI settle

                    // Only saving settings and finalizing
                    Settings.Default.Save();
                    await Task.Delay(300);
                    savingForm.UpdateStep("Settings", gStr.gsSaveSettingsSaved, SavingStepState.Done);
                    await Task.Delay(300);
                    savingForm.UpdateStep("Finalize", gStr.gsSaveAllDone, SavingStepState.Done);
                    await Task.Delay(750);
                    savingForm.Finish();
                }

                await Task.Delay(2000);
                savingForm.Close();
            }

            FinishShutdown(choice);
        }


        private void FinishShutdown(int choice)
        {
            SaveFormGPSWindowSettings();

            double minutesSinceStart = ((DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds) / 60;
            if (minutesSinceStart < 1) minutesSinceStart = 1;

            Log.EventWriter("Missed Sentence Counter Total: " + missedSentenceCount.ToString()
                + "   Missed Per Minute: " + ((double)missedSentenceCount / minutesSinceStart).ToString("N4"));

            Log.EventWriter("Program Exit: " + DateTime.Now.ToString("f", CultureInfo.CreateSpecificCulture(RegistrySettings.culture)) + "\r");

            // Restore display brightness
            if (displayBrightness.isWmiMonitor)
            {
                try { displayBrightness.SetBrightness(Settings.Default.setDisplay_brightnessSystem); }
                catch { }
            }

            // Perform Windows shutdown if user selected it
            if (choice == 2)
            {
                try
                {
                    Process[] agio = Process.GetProcessesByName("AgIO");
                    if (agio.Length > 0) agio[0].CloseMainWindow();
                }
                catch { }

                try
                {
                    Process.Start("shutdown", "/s /t 0");
                }
                catch { }
            }

            // Close loopback socket if active
            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                    loopBackSocket.Close();
                }
                catch { }
            }

            // Auto close AgIO process if enabled
            if (Settings.Default.setDisplay_isAutoOffAgIO)
            {
                try
                {
                    Process[] agio = Process.GetProcessesByName("AgIO");
                    if (agio.Length > 0) agio[0].CloseMainWindow();
                }
                catch { }
            }

            // Close the main application form
            try { Close(); }
            catch (ObjectDisposedException) { }
        }
        #endregion

        public int SaveOrNot()
        {
            CloseTopMosts();

            using (FormSaveOrNot form = new FormSaveOrNot(this))
            {
                DialogResult result = form.ShowDialog(this);

                if (result == DialogResult.OK) return 0;      //Exit to windows
                if (result == DialogResult.Ignore) return 1;   //Ignore & return
                if (result == DialogResult.Yes) return 2;   //Shutdown computer

                return 1;  // oops something is really busted
            }
        }

        private void FormGPS_ResizeEnd(object sender, EventArgs e)
        {
            PanelsAndOGLSize();
            if (isGPSPositionInitialized) SetZoom();

            Form f = Application.OpenForms["FormGPSData"];
            if (f != null)
            {
                f.Top = this.Top + this.Height / 2 - GPSDataWindowTopOffset;
                f.Left = this.Left + GPSDataWindowLeft;
            }

            f = Application.OpenForms["FormFieldData"];
            if (f != null)
            {
                f.Top = this.Top + this.Height / 2 - GPSDataWindowTopOffset;
                f.Left = this.Left + GPSDataWindowLeft;
            }

            f = Application.OpenForms["FormPan"];
            if (f != null)
            {
                f.Top = this.Height / 3 + this.Top;
                f.Left = this.Width - 400 + this.Left;
            }
        }

        private void FormGPS_Move(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];
            if (f != null)
            {
                f.Top = this.Top + this.Height / 2 - GPSDataWindowTopOffset;
                f.Left = this.Left + GPSDataWindowLeft;
            }

            f = Application.OpenForms["FormFieldData"];
            if (f != null)
            {
                f.Top = this.Top + this.Height / 2 - GPSDataWindowTopOffset;
                f.Left = this.Left + GPSDataWindowLeft;
            }

            f = Application.OpenForms["FormPan"];
            if (f != null)
            {
                f.Top = this.Top + 75;
                f.Left = this.Left + this.Width - 380;
            }
        }

        //request a new job
        public void JobNew()
        {
            //SendSteerSettingsOutAutoSteerPort();

            AppModel.Fields.OpenField();
            startCounter = 0;

            btnFieldStats.Visible = true;

            btnSectionMasterManual.Enabled = true;
            manualBtnState = btnStates.Off;
            btnSectionMasterManual.Image = Properties.Resources.ManualOff;

            btnSectionMasterAuto.Enabled = true;
            autoBtnState = btnStates.Off;
            btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOff;

            btnSection1Man.BackColor = Color.Red;
            btnSection2Man.BackColor = Color.Red;
            btnSection3Man.BackColor = Color.Red;
            btnSection4Man.BackColor = Color.Red;
            btnSection5Man.BackColor = Color.Red;
            btnSection6Man.BackColor = Color.Red;
            btnSection7Man.BackColor = Color.Red;
            btnSection8Man.BackColor = Color.Red;
            btnSection9Man.BackColor = Color.Red;
            btnSection10Man.BackColor = Color.Red;
            btnSection11Man.BackColor = Color.Red;
            btnSection12Man.BackColor = Color.Red;
            btnSection13Man.BackColor = Color.Red;
            btnSection14Man.BackColor = Color.Red;
            btnSection15Man.BackColor = Color.Red;
            btnSection16Man.BackColor = Color.Red;

            btnSection1Man.Enabled = true;
            btnSection2Man.Enabled = true;
            btnSection3Man.Enabled = true;
            btnSection4Man.Enabled = true;
            btnSection5Man.Enabled = true;
            btnSection6Man.Enabled = true;
            btnSection7Man.Enabled = true;
            btnSection8Man.Enabled = true;
            btnSection9Man.Enabled = true;
            btnSection10Man.Enabled = true;
            btnSection11Man.Enabled = true;
            btnSection12Man.Enabled = true;
            btnSection13Man.Enabled = true;
            btnSection14Man.Enabled = true;
            btnSection15Man.Enabled = true;
            btnSection16Man.Enabled = true;

            btnZone1.BackColor = Color.Red;
            btnZone2.BackColor = Color.Red;
            btnZone3.BackColor = Color.Red;
            btnZone4.BackColor = Color.Red;
            btnZone5.BackColor = Color.Red;
            btnZone6.BackColor = Color.Red;
            btnZone7.BackColor = Color.Red;
            btnZone8.BackColor = Color.Red;

            btnZone1.Enabled = true;
            btnZone2.Enabled = true;
            btnZone3.Enabled = true;
            btnZone4.Enabled = true;
            btnZone5.Enabled = true;
            btnZone6.Enabled = true;
            btnZone7.Enabled = true;
            btnZone8.Enabled = true;

            btnContour.Enabled = true;
            btnTrack.Enabled = true;
            btnABDraw.Enabled = true;
            btnCycleLines.Image = Properties.Resources.ABLineCycle;
            btnCycleLinesBk.Image = Properties.Resources.ABLineCycleBk;

            ABLine.abHeading = 0.00;
            btnAutoSteer.Enabled = true;

            DisableYouTurnButtons();
            btnFlag.Enabled = true;

            if (tool.isSectionsNotZones)
            {
                LineUpIndividualSectionBtns();
            }
            else
            {
                LineUpAllZoneButtons();
            }

            //update the menu
            this.menustripLanguage.Enabled = false;
            panelRight.Enabled = true;
            //boundaryToolStripBtn.Enabled = true;
            isPanelBottomHidden = false;

            FieldMenuButtonEnableDisable(true);
            PanelUpdateRightAndBottom();
            PanelsAndOGLSize();
            SetZoom();

            fileSaveCounter = 25;
            lblGuidanceLine.Visible = false;
            lblHardwareMessage.Visible = false;
            btnAutoTrack.Image = Resources.AutoTrackOff;
            trk.isAutoTrack = false;
        }

        //close the current job
        public void JobClose()
        {
            recPath.resumeState = 0;
            btnResumePath.Image = Properties.Resources.pathResumeStart;
            recPath.currentPositonIndex = 0;

            sbGrid.Clear();

            //reset field offsets
            if (!isKeepOffsetsOn)
            {
                AppModel.SharedFieldProperties.DriftCompensation = new GeoDelta(0.0, 0.0);
            }

            //turn off headland
            bnd.isHeadlandOn = false;

            btnFieldStats.Visible = false;

            recPath.recList.Clear();
            recPath.StopDrivingRecordedPath();
            panelDrag.Visible = false;

            //make sure hydraulic lift is off
            p_239.pgn[p_239.hydLift] = 0;
            vehicle.isHydLiftOn = false;
            btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            btnHydLift.Visible = false;
            lblHardwareMessage.Visible = false;

            lblGuidanceLine.Visible = false;
            lblHardwareMessage.Visible = false;

            //zoom gone
            oglZoom.SendToBack();

            //clean all the lines
            bnd.bndList.Clear();

            panelRight.Enabled = false;
            FieldMenuButtonEnableDisable(false);

            menustripLanguage.Enabled = true;

            AppModel.Fields.CloseField();


            //fix ManualOffOnAuto buttons
            manualBtnState = btnStates.Off;
            btnSectionMasterManual.Image = Properties.Resources.ManualOff;

            //fix auto button
            autoBtnState = btnStates.Off;
            btnSectionMasterAuto.Image = Properties.Resources.SectionMasterOff;

            if (tool.isSectionsNotZones)
            {
                //Update the button colors and text
                AllSectionsAndButtonsToState(btnStates.Off);

                //enable disable manual buttons
                LineUpIndividualSectionBtns();
            }
            else
            {
                AllZonesAndButtonsToState(autoBtnState);
                LineUpAllZoneButtons();
            }

            btnZone1.BackColor = Color.Silver;
            btnZone2.BackColor = Color.Silver;
            btnZone3.BackColor = Color.Silver;
            btnZone4.BackColor = Color.Silver;
            btnZone5.BackColor = Color.Silver;
            btnZone6.BackColor = Color.Silver;
            btnZone7.BackColor = Color.Silver;
            btnZone8.BackColor = Color.Silver;

            btnZone1.Enabled = false;
            btnZone2.Enabled = false;
            btnZone3.Enabled = false;
            btnZone4.Enabled = false;
            btnZone5.Enabled = false;
            btnZone6.Enabled = false;
            btnZone7.Enabled = false;
            btnZone8.Enabled = false;

            btnSection1Man.Enabled = false;
            btnSection2Man.Enabled = false;
            btnSection3Man.Enabled = false;
            btnSection4Man.Enabled = false;
            btnSection5Man.Enabled = false;
            btnSection6Man.Enabled = false;
            btnSection7Man.Enabled = false;
            btnSection8Man.Enabled = false;
            btnSection9Man.Enabled = false;
            btnSection10Man.Enabled = false;
            btnSection11Man.Enabled = false;
            btnSection12Man.Enabled = false;
            btnSection13Man.Enabled = false;
            btnSection14Man.Enabled = false;
            btnSection15Man.Enabled = false;
            btnSection16Man.Enabled = false;

            btnSection1Man.BackColor = Color.Silver;
            btnSection2Man.BackColor = Color.Silver;
            btnSection3Man.BackColor = Color.Silver;
            btnSection4Man.BackColor = Color.Silver;
            btnSection5Man.BackColor = Color.Silver;
            btnSection6Man.BackColor = Color.Silver;
            btnSection7Man.BackColor = Color.Silver;
            btnSection8Man.BackColor = Color.Silver;
            btnSection9Man.BackColor = Color.Silver;
            btnSection10Man.BackColor = Color.Silver;
            btnSection11Man.BackColor = Color.Silver;
            btnSection12Man.BackColor = Color.Silver;
            btnSection13Man.BackColor = Color.Silver;
            btnSection14Man.BackColor = Color.Silver;
            btnSection15Man.BackColor = Color.Silver;
            btnSection16Man.BackColor = Color.Silver;

            //clear the section lists
            for (int j = 0; j < triStrip.Count; j++)
            {
                //clean out the lists
                triStrip[j].patchList?.Clear();
                triStrip[j].triangleList?.Clear();
            }

            triStrip?.Clear();
            triStrip.Add(new CPatches(this));

            //clear the flags
            flagPts.Clear();

            //ABLine
            tram.tramList?.Clear();

            //curve line
            curve.ResetCurveLine();

            //tracks
            trk.gArr?.Clear();
            trk.idx = -1;

            //clean up tram
            tram.displayMode = 0;
            tram.generateMode = 0;
            tram.tramBndInnerArr?.Clear();
            tram.tramBndOuterArr?.Clear();

            //clear out contour and Lists
            btnContour.Enabled = false;
            //btnContourPriority.Enabled = false;
            //btnSnapToPivot.Image = Properties.Resources.SnapToPivot;
            ct.ResetContour();
            ct.isContourBtnOn = false;
            btnContour.Image = Properties.Resources.ContourOff;
            ct.isContourOn = false;

            btnABDraw.Enabled = false;
            btnCycleLines.Image = Properties.Resources.ABLineCycle;
            //btnCycleLines.Enabled = false;
            btnCycleLinesBk.Image = Properties.Resources.ABLineCycleBk;
            //btnCycleLinesBk.Enabled = false;

            //AutoSteer
            btnAutoSteer.Enabled = false;
            isBtnAutoSteerOn = false;
            btnAutoSteer.Image = trk.isAutoSnapToPivot ? Properties.Resources.AutoSteerOffSnapToPivot : Properties.Resources.AutoSteerOff;

            //auto YouTurn shutdown
            yt.isYouTurnBtnOn = false;
            btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;

            btnABDraw.Visible = false;

            yt.ResetYouTurn();
            DisableYouTurnButtons();

            //reset acre and distance counters
            fd.workedAreaTotal = 0;

            //reset GUI areas
            fd.UpdateFieldBoundaryGUIAreas();

            recPath.recList?.Clear();
            recPath.shortestDubinsList?.Clear();
            recPath.shuttleDubinsList?.Clear();

            isPanelBottomHidden = false;

            PanelsAndOGLSize();
            SetZoom();
            worldGrid.isGeoMap = false;

            panelSim.Top = Height - 60;

            PanelUpdateRightAndBottom();

            btnSection1Man.Text = "1";

            worldGrid.BingBitmap = Properties.Resources.z_bingMap;

            // Reset AgShare upload state and clear snapshot after field is closed
            isAgShareUploadStarted = false;
            snapshot = null;

        }

        public void FieldMenuButtonEnableDisable(bool isOn)
        {
            SmoothABtoolStripMenu.Enabled = isOn;
            deleteContourPathsToolStripMenuItem.Enabled = isOn;
            boundaryToolToolStripMenu.Enabled = isOn;
            offsetFixToolStrip.Enabled = isOn;
            toolStripBtnFieldTools.Enabled = isOn;

            boundariesToolStripMenuItem.Enabled = isOn;
            headlandToolStripMenuItem.Enabled = isOn;
            headlandBuildToolStripMenuItem.Enabled = isOn;
            flagByLatLonToolStripMenuItem.Enabled = isOn;
            tramLinesMenuField.Enabled = isOn;
            tramsMultiMenuField.Enabled = isOn;
            recordedPathStripMenu.Enabled = isOn;
        }

        //take the distance from object and convert to camera data
        public void SetZoom()
        {
            //match grid to cam distance and redo perspective
            double gridStep = camera.camSetDistance / -15;

            gridToolSpacing = (int)(gridStep / tool.width + 0.5);
            if (gridToolSpacing < 1) gridToolSpacing = 1;
            worldGrid.GridStep = gridToolSpacing * tool.width;

            oglMain.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView((float)fovy, oglMain.AspectRatio, 1f, (float)(camDistanceFactor * camera.camSetDistance));
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        public void SendSettings()
        {
            //Form Steer Settings
            p_252.pgn[p_252.countsPerDegree] = unchecked((byte)Properties.Settings.Default.setAS_countsPerDegree);
            p_252.pgn[p_252.ackerman] = unchecked((byte)Properties.Settings.Default.setAS_ackerman);

            p_252.pgn[p_252.wasOffsetHi] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset >> 8));
            p_252.pgn[p_252.wasOffsetLo] = unchecked((byte)(Properties.Settings.Default.setAS_wasOffset));

            p_252.pgn[p_252.highPWM] = unchecked((byte)Properties.Settings.Default.setAS_highSteerPWM);
            p_252.pgn[p_252.lowPWM] = unchecked((byte)Properties.Settings.Default.setAS_lowSteerPWM);
            p_252.pgn[p_252.gainProportional] = unchecked((byte)Properties.Settings.Default.setAS_Kp);
            p_252.pgn[p_252.minPWM] = unchecked((byte)Properties.Settings.Default.setAS_minSteerPWM);

            SendPgnToLoop(p_252.pgn);

            //steer config
            p_251.pgn[p_251.set0] = Properties.Settings.Default.setArdSteer_setting0;
            p_251.pgn[p_251.set1] = Properties.Settings.Default.setArdSteer_setting1;
            p_251.pgn[p_251.maxPulse] = Properties.Settings.Default.setArdSteer_maxPulseCounts;
            p_251.pgn[p_251.minSpeed] = unchecked((byte)(Properties.Settings.Default.setAS_minSteerSpeed * 10));

            if (Properties.Settings.Default.setAS_isConstantContourOn)
                p_251.pgn[p_251.angVel] = 1;
            else p_251.pgn[p_251.angVel] = 0;

            SendPgnToLoop(p_251.pgn);

            //machine settings    
            p_238.pgn[p_238.set0] = Properties.Settings.Default.setArdMac_setting0;
            p_238.pgn[p_238.raiseTime] = Properties.Settings.Default.setArdMac_hydRaiseTime;
            p_238.pgn[p_238.lowerTime] = Properties.Settings.Default.setArdMac_hydLowerTime;

            p_238.pgn[p_238.user1] = Properties.Settings.Default.setArdMac_user1;
            p_238.pgn[p_238.user2] = Properties.Settings.Default.setArdMac_user2;
            p_238.pgn[p_238.user3] = Properties.Settings.Default.setArdMac_user3;
            p_238.pgn[p_238.user4] = Properties.Settings.Default.setArdMac_user4;

            SendPgnToLoop(p_238.pgn);
        }

        public void SendRelaySettingsToMachineModule()
        {
            string[] words = Properties.Settings.Default.setRelay_pinConfig.Split(',');

            //load the pgn
            p_236.pgn[p_236.pin0] = (byte)int.Parse(words[0]);
            p_236.pgn[p_236.pin1] = (byte)int.Parse(words[1]);
            p_236.pgn[p_236.pin2] = (byte)int.Parse(words[2]);
            p_236.pgn[p_236.pin3] = (byte)int.Parse(words[3]);
            p_236.pgn[p_236.pin4] = (byte)int.Parse(words[4]);
            p_236.pgn[p_236.pin5] = (byte)int.Parse(words[5]);
            p_236.pgn[p_236.pin6] = (byte)int.Parse(words[6]);
            p_236.pgn[p_236.pin7] = (byte)int.Parse(words[7]);
            p_236.pgn[p_236.pin8] = (byte)int.Parse(words[8]);
            p_236.pgn[p_236.pin9] = (byte)int.Parse(words[9]);

            p_236.pgn[p_236.pin10] = (byte)int.Parse(words[10]);
            p_236.pgn[p_236.pin11] = (byte)int.Parse(words[11]);
            p_236.pgn[p_236.pin12] = (byte)int.Parse(words[12]);
            p_236.pgn[p_236.pin13] = (byte)int.Parse(words[13]);
            p_236.pgn[p_236.pin14] = (byte)int.Parse(words[14]);
            p_236.pgn[p_236.pin15] = (byte)int.Parse(words[15]);
            p_236.pgn[p_236.pin16] = (byte)int.Parse(words[16]);
            p_236.pgn[p_236.pin17] = (byte)int.Parse(words[17]);
            p_236.pgn[p_236.pin18] = (byte)int.Parse(words[18]);
            p_236.pgn[p_236.pin19] = (byte)int.Parse(words[19]);

            p_236.pgn[p_236.pin20] = (byte)int.Parse(words[20]);
            p_236.pgn[p_236.pin21] = (byte)int.Parse(words[21]);
            p_236.pgn[p_236.pin22] = (byte)int.Parse(words[22]);
            p_236.pgn[p_236.pin23] = (byte)int.Parse(words[23]);
            SendPgnToLoop(p_236.pgn);


            p_235.pgn[p_235.sec0Lo] = unchecked((byte)(section[0].sectionWidth * 100));
            p_235.pgn[p_235.sec0Hi] = unchecked((byte)((int)((section[0].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec1Lo] = unchecked((byte)(section[1].sectionWidth * 100));
            p_235.pgn[p_235.sec1Hi] = unchecked((byte)((int)((section[1].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec2Lo] = unchecked((byte)(section[2].sectionWidth * 100));
            p_235.pgn[p_235.sec2Hi] = unchecked((byte)((int)((section[2].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec3Lo] = unchecked((byte)(section[3].sectionWidth * 100));
            p_235.pgn[p_235.sec3Hi] = unchecked((byte)((int)((section[3].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec4Lo] = unchecked((byte)(section[4].sectionWidth * 100));
            p_235.pgn[p_235.sec4Hi] = unchecked((byte)((int)((section[4].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec5Lo] = unchecked((byte)(section[5].sectionWidth * 100));
            p_235.pgn[p_235.sec5Hi] = unchecked((byte)((int)((section[5].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec6Lo] = unchecked((byte)(section[6].sectionWidth * 100));
            p_235.pgn[p_235.sec6Hi] = unchecked((byte)((int)((section[6].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec7Lo] = unchecked((byte)(section[7].sectionWidth * 100));
            p_235.pgn[p_235.sec7Hi] = unchecked((byte)((int)((section[7].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec8Lo] = unchecked((byte)(section[8].sectionWidth * 100));
            p_235.pgn[p_235.sec8Hi] = unchecked((byte)((int)((section[8].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec9Lo] = unchecked((byte)(section[9].sectionWidth * 100));
            p_235.pgn[p_235.sec9Hi] = unchecked((byte)((int)((section[9].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec10Lo] = unchecked((byte)(section[10].sectionWidth * 100));
            p_235.pgn[p_235.sec10Hi] = unchecked((byte)((int)((section[10].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec11Lo] = unchecked((byte)(section[11].sectionWidth * 100));
            p_235.pgn[p_235.sec11Hi] = unchecked((byte)((int)((section[11].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec12Lo] = unchecked((byte)(section[12].sectionWidth * 100));
            p_235.pgn[p_235.sec12Hi] = unchecked((byte)((int)((section[12].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec13Lo] = unchecked((byte)(section[13].sectionWidth * 100));
            p_235.pgn[p_235.sec13Hi] = unchecked((byte)((int)((section[13].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec14Lo] = unchecked((byte)(section[14].sectionWidth * 100));
            p_235.pgn[p_235.sec14Hi] = unchecked((byte)((int)((section[14].sectionWidth * 100)) >> 8));
            p_235.pgn[p_235.sec15Lo] = unchecked((byte)(section[15].sectionWidth * 100));
            p_235.pgn[p_235.sec15Hi] = unchecked((byte)((int)((section[15].sectionWidth * 100)) >> 8));

            p_235.pgn[p_235.numSections] = (byte)tool.numOfSections;

            SendPgnToLoop(p_235.pgn);
        }

        //message box pops up with info then goes away
        public void TimedMessageBox(int timeout, string s1, string s2)
        {
            FormTimedMessage form = new FormTimedMessage(timeout, s1, s2);
            form.Show(this);
            this.Activate();
        }

        public void YesMessageBox(string s1)
        {
            var form = new FormYes(s1);
            form.ShowDialog(this);
        }
    }//class FormGPS
}//namespace AgOpenGPS



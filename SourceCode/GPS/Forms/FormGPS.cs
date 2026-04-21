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
using AgOpenGPS.Core.AgShare;
using AgOpenGPS.Core.Models;
using AgOpenGPS.Core.Translations;
using AgOpenGPS.Core.ViewModels;
using AgOpenGPS.Forms.Profiles;
using AgOpenGPS.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;
// AGROPARALLEL_MOD_START
using AgroParallel.Common;
// AGROPARALLEL_MOD_END
// VISTAX_MOD_START
using AgroParallel.VistaX;
// VISTAX_MOD_END
// QUANTIX_MOD_START
using AgroParallel.QuantiX;
// QUANTIX_MOD_END
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

        // VISTAX_MOD_START
        private SeedMonitor vistaXMonitor;
        private VistaXPanel vistaXPanel;
        public VistaXConfig vistaXConfig;
        // VISTAX_MOD_END

        // SHAPEFILE_MOD_START
        private ShapefileLayer shapefileLayer;
        private ToolStripMenuItem shapefileToggleItem;
        private ToolStripMenuItem shapefileStyleItem;
        private ToolStripMenuItem shapefileInspectItem;
        private ShapefileLegendControl shapefileLegend;

        // Modo inspeccion (paso 12): al hacer click en el mapa se abre un
        // popup con los atributos DBF del poligono bajo el cursor.
        private bool shapefileInspectMode;

        // Snapshots de matrices GL tomadas en cada frame — usadas por la
        // unprojection para convertir mouse (screen) -> mundo (easting/northing).
        private readonly double[] _glModelView = new double[16];
        private readonly double[] _glProjection = new double[16];
        private readonly int[] _glViewport = new int[4];
        private bool _glMatricesValid;
        // SHAPEFILE_MOD_END

        // QUANTIX_MOD_START
        private QuantiXConfig quantiXConfig;
        private QuantiXSender quantiXSender;
        // QUANTIX_MOD_END

        // COREX_FIELD_MOD_START
        // Último campo notificado al sistema AgroParallel (CoreX / VistaX)
        // Permite detectar si es "nuevo", "abierto" o "continuar"
        private string _apLastFieldNotified = "";
        // COREX_FIELD_MOD_END


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
        public AgShareClient agShareClient;


        /// <summary>
        /// The ISOBUS communication class
        /// </summary>
        public CISOBUS isobus;

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
            if (CefSharp.Cef.IsInitialized == false)
            {
                var settings = new CefSharp.WinForms.CefSettings();

                // Opcional: Desactiva aceleración por hardware si el panel se ve negro
                settings.CefCommandLineArgs.Add("disable-gpu", "1");

                CefSharp.Cef.Initialize(settings);
            }

            //winform initialization
            InitializeComponent();

            InitializeLanguages();

            AppCore = new ApplicationCore(
                new DirectoryInfo(RegistrySettings.baseDirectory),
                null,
                null);
            // Uncomment next line for Performance analysis
            // InitializePerformanceTool(AppCore);

            //time keeper
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            camera = new Camera(
                Properties.Settings.Default.setDisplay_camPitch,
                Properties.Settings.Default.setDisplay_camZoom);

            worldGrid = new WorldGrid(Resources.z_Floor);

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

            curve = new CABCurve(this);

            //new track instance
            trk = new CTrack(this);

            hdl = new CHeadLine();

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

            isobus = new CISOBUS(this);
        }

        private void FormGPS_Load(object sender, EventArgs e)
        {
            Log.EventWriter("Program Started: "
                + DateTime.Now.ToString("f", CultureInfo.InvariantCulture));
            Log.EventWriter("AOG Version: " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));

            if (!Properties.Settings.Default.setDisplay_isTermsAccepted)
            {
                using (var form = new FormTermsAndConditions())
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

            // VISTAX_MOD_START
            vistaXConfig = VistaXConfig.Load();
            // VISTAX_MOD_END

            this.MouseWheel += ZoomByMouseWheel;

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

            // VISTAX_MOD_START
            InitVistaX();
            // VISTAX_MOD_END

            // AGROPARALLEL_MOD_START
            InitAgroParallelModulesMenu();
            // AGROPARALLEL_MOD_END

            // SHAPEFILE_MOD_START
            InitShapefileMenu();
            // SHAPEFILE_MOD_END
        }

        #region Shutdown Handling

        // Centralized shutdown coordinator
        private bool isShuttingDown = false;

        private async void FormGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isShuttingDown) return;

            // Set shutdown flag to prevent re-entrance
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
                return;
            }

            // Get user choice for shutdown behavior
            int choice = SaveOrNot();
            if (choice == 1)
            {
                // User cancelled shutdown
                isShuttingDown = false;
                return;
            }

            // Turn off auto sections if active
            if (isJobStarted && autoBtnState == btnStates.Auto)
            {
                btnSectionMasterAuto.PerformClick();
            }

            // Execute shutdown with proper exception handling
            try
            {
                // VISTAX_MOD_START
                CleanupVistaX();
                // VISTAX_MOD_END

                Log.EventWriter("Closing Application " + DateTime.Now);
                await ShowSavingFormAndShutdown(choice);
            }
            catch (Exception ex)
            {
                Log.EventWriter($"CRITICAL: Shutdown error: {ex}");
                MessageBox.Show($"Error during shutdown: {ex.Message}\n\nAttempting force exit...",
                    "Shutdown Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // Ensure application exits even if shutdown fails
                Application.Exit();
            }
        }


        private async Task ShowSavingFormAndShutdown(int choice)
        {
            FormSaving savingForm = null;

            try
            {
                savingForm = new FormSaving();

                if (isJobStarted)
                {
                    // Check if AgShare is enabled (step will be added regardless of whether upload already started)
                    bool isAgShareEnabled = Settings.Default.AgShareEnabled &&
                                           Settings.Default.AgShareUploadActive;

                    // Setup progress steps
                    savingForm.AddStep("Field", gStr.gsSaveField);
                    if (isAgShareEnabled) savingForm.AddStep("AgShare", gStr.gsSaveUploadToAgshare);
                    savingForm.AddStep("Settings", gStr.gsSaveSettings);
                    savingForm.AddStep("Finalize", gStr.gsSaveFinalizeShutdown);

                    savingForm.Show();
                    await Task.Delay(300); // Let UI settle

                    // STEP 1: Save Field (Boundary, Tracks, Sections, Contour, etc.)
                    // NOTE: This also starts AND waits for AgShare upload if enabled
                    try
                    {
                        await FileSaveEverythingBeforeClosingField();
                        savingForm.UpdateStep("Field", gStr.gsSaveFieldSavedLocal, SavingStepState.Done);

                        // STEP 2: Update AgShare status (upload was completed in FileSaveEverythingBeforeClosingField)
                        if (isAgShareEnabled && isAgShareUploadStarted)
                        {
                            // The upload was already awaited in FileSaveEverythingBeforeClosingField
                            // Check if the task completed successfully
                            if (agShareUploadTask != null)
                            {
                                if (agShareUploadTask.Status == TaskStatus.RanToCompletion)
                                {
                                    savingForm.UpdateStep("AgShare", gStr.gsSaveUploadCompleted, SavingStepState.Done);
                                }
                                else if (agShareUploadTask.Status == TaskStatus.Faulted)
                                {
                                    savingForm.UpdateStep("AgShare", gStr.gsSaveUploadFailed, SavingStepState.Failed);
                                }
                                else
                                {
                                    // Still running or cancelled? This shouldn't happen as FileSaveEverythingBeforeClosingField awaits it
                                    savingForm.UpdateStep("AgShare", "Upload status unknown", SavingStepState.Failed);
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.EventWriter($"CRITICAL: Field save error during shutdown: {ex}");
                        savingForm.UpdateStep("Field", "Field save FAILED: " + ex.Message, SavingStepState.Failed);

                        // Ask user if they want to continue despite error
                        DialogResult result = MessageBox.Show(
                            $"Field data save failed:\n{ex.Message}\n\nContinue shutdown anyway? (data may be lost)",
                            "Critical Save Error",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                        if (result == DialogResult.No)
                        {
                            isShuttingDown = false;
                            if (savingForm != null && !savingForm.IsDisposed) savingForm.Close();
                            return; // Exit without calling FinishShutdown - user cancelled
                        }
                    }

                    // STEP 3: Settings + System Log
                    try
                    {
                        Settings.Default.Save();
                        Log.FileSaveSystemEvents();
                        await Task.Delay(300);
                        savingForm.UpdateStep("Settings", gStr.gsSaveSettingsSaved, SavingStepState.Done);
                    }
                    catch (Exception ex)
                    {
                        Log.EventWriter($"Settings save error: {ex}");
                        savingForm.UpdateStep("Settings", "Settings save failed", SavingStepState.Failed);
                    }

                    // STEP 4: Finalizing
                    await Task.Delay(500);
                    savingForm.UpdateStep("Finalize", gStr.gsSaveAllDone, SavingStepState.Done);
                    await Task.Delay(750);
                    savingForm.Finish();
                }
                else
                {
                    // Job not started - just save settings with visual feedback
                    savingForm.AddStep("Settings", gStr.gsSaveSettings);
                    savingForm.AddStep("Finalize", gStr.gsSaveFinalizeShutdown);

                    savingForm.Show();
                    await Task.Delay(300); // Let UI settle

                    try
                    {
                        Settings.Default.Save();
                        Log.FileSaveSystemEvents();
                        await Task.Delay(300);
                        savingForm.UpdateStep("Settings", gStr.gsSaveSettingsSaved, SavingStepState.Done);
                    }
                    catch (Exception ex)
                    {
                        Log.EventWriter($"Settings save error: {ex}");
                        savingForm.UpdateStep("Settings", "Settings save failed", SavingStepState.Failed);
                    }

                    // Finalizing
                    await Task.Delay(500);
                    savingForm.UpdateStep("Finalize", gStr.gsSaveAllDone, SavingStepState.Done);
                    await Task.Delay(750);
                    savingForm.Finish();
                }
            }
            finally
            {
                // Ensure form is disposed
                savingForm?.Dispose();
            }

            // Only finish shutdown if we didn't return early due to user cancellation
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

        private void btnIsobusSC_Click(object sender, EventArgs e)
        {
            isobus.RequestSectionControlEnabled(!isobus.SectionControlEnabled);
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

            // COREX_FIELD_MOD_START
            NotificarCampoExterno_JobNew();
            // COREX_FIELD_MOD_END

            // SHAPEFILE_MOD_START
            TryAutoLoadShapefileForCurrentField();
            // SHAPEFILE_MOD_END

            // QUANTIX_MOD_START
            StartQuantiXSender();
            // QUANTIX_MOD_END
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

            // COREX_FIELD_MOD_START
            // Notificar ANTES de limpiar currentFieldDirectory
            NotificarCampoExterno_JobClose();
            // COREX_FIELD_MOD_END

            // SHAPEFILE_MOD_START
            ClearShapefileLayer();
            // SHAPEFILE_MOD_END

            // QUANTIX_MOD_START
            StopQuantiXSender();
            // QUANTIX_MOD_END

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
            worldGrid.BingMap = null;

            panelSim.Top = Height - 60;

            PanelUpdateRightAndBottom();

            btnSection1Man.Text = "1";

            // Reset AgShare upload state and clear snapshot after field is closed
            // NOTE: Don't reset during shutdown - the shutdown flow needs to check this flag
            if (!isShuttingDown)
            {
                isAgShareUploadStarted = false;
                snapshot = null;
            }
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
            worldGrid.FieldGrid.GridStep = gridToolSpacing * tool.width;

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

        // AGROPARALLEL_MOD_START
        private void toolStripAgroParallel_Click(object sender, EventArgs e)
        {
            ToggleVistaX();
        }

        private void ToggleVistaX()
        {
            var cfg = VistaXConfig.Load();

            if (vistaXPanel != null)
            {
                // VistaX está activo → desactivar
                this.Controls.Remove(vistaXPanel);
                vistaXPanel.Dispose();
                vistaXPanel = null;

                cfg.Enabled = false;
                cfg.Save();

                System.Diagnostics.Debug.WriteLine("[VistaX] Desactivado por el usuario");
            }
            else
            {
                // VistaX está inactivo → activar
                cfg.Enabled = true;
                cfg.Save();

                InitVistaX();

                if (vistaXPanel == null)
                {
                    // Falló la inicialización (server Node no corre, etc.)
                    cfg.Enabled = false;
                    cfg.Save();
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("[VistaX] Activado por el usuario");
                }
            }
        }
        // AGROPARALLEL_MOD_END

        // VISTAX_MOD_START
        private void InitVistaX()
        {
            try
            {
                // Guard: no crear doble instancia
                if (vistaXPanel != null) return;

                var cfg = VistaXConfig.Load();
                System.Diagnostics.Debug.WriteLine("[VistaX] Config: Enabled=" + cfg.Enabled
                    + " ServerUrl=" + cfg.ServerUrl);

                if (!cfg.Enabled)
                {
                    System.Diagnostics.Debug.WriteLine("[VistaX] Deshabilitado");
                    return;
                }

                // Solo crear el panel CefSharp — el servidor Node maneja MQTT y Socket.IO
                vistaXPanel = new VistaXPanel(cfg);
                vistaXPanel.Visible = true;

                this.Controls.Add(vistaXPanel);
                vistaXPanel.Reposition();

                // Reposicionar al redimensionar
                this.Resize += (s, e) => { if (vistaXPanel != null) vistaXPanel.Reposition(); };

                System.Diagnostics.Debug.WriteLine("[VistaX] Panel CefSharp iniciado OK → " + cfg.ServerUrl);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[VistaX] ERROR: " + ex.ToString());
            }
        }

        private void CleanupVistaX()
        {
            if (vistaXPanel != null)
            {
                try { vistaXPanel.Dispose(); } catch { }
                vistaXPanel = null;
            }
            if (vistaXMonitor != null)
            {
                try
                {
                    vistaXMonitor.StopAsync().GetAwaiter().GetResult();
                    vistaXMonitor.Dispose();
                }
                catch { }
                vistaXMonitor = null;
            }
        }
        // VISTAX_MOD_END

        // AGROPARALLEL_MOD_START
        // Lee agroParallelModules.json del directorio base y agrega un item en el
        // dropdown de configuracion por cada modulo habilitado. Cada item abre una
        // ventana popup (ModulePopupForm) que embebe CefSharp apuntando a su Url.
        private void InitAgroParallelModulesMenu()
        {
            try
            {
                var cfg = AgroParallelModulesConfig.Load();
                if (cfg == null || cfg.Modules == null) return;

                foreach (var m in cfg.Modules)
                {
                    if (m == null || !m.Enabled) continue;
                    if (string.IsNullOrWhiteSpace(m.Name)) continue;

                    var item = new ToolStripMenuItem();
                    string prefix = string.IsNullOrEmpty(m.Emoji) ? "" : m.Emoji + " ";
                    item.Text = prefix + m.Name;
                    item.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                    item.ForeColor = AgroParallelModulesConfig.ResolveAccentColor(m);

                    string iconFull = AgroParallelModulesConfig.ResolveIconFullPath(m.IconPath);
                    if (iconFull != null && File.Exists(iconFull))
                    {
                        try { item.Image = Image.FromFile(iconFull); }
                        catch { /* icono invalido: seguimos con solo texto+emoji */ }
                    }

                    var capture = m;
                    item.Click += (s, e) => OpenAgroParallelModulePopup(capture);

                    toolStripDropDownButton1.DropDownItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[AgroParallel] InitAgroParallelModulesMenu: "
                    + ex.Message);
            }
        }

        private void OpenAgroParallelModulePopup(AgroParallelModuleEntry module)
        {
            try
            {
                var popup = new ModulePopupForm(module);
                popup.Show(this);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[AgroParallel] Popup " + module.Name
                    + " error: " + ex.Message);
                TimedMessageBox(2500, "AgroParallel", module.Name + ": " + ex.Message);
            }
        }
        // AGROPARALLEL_MOD_END

        // SHAPEFILE_MOD_START
        // Agrega "Cargar Shapefile" al dropdown de configuracion. El handler abre
        // un OpenFileDialog, parsea con ShapefileReader y muestra un resumen.
        // Sin render todavia — eso es el paso siguiente.
        private void InitShapefileMenu()
        {
            try
            {
                var itemLoad = new ToolStripMenuItem();
                itemLoad.Text = "\U0001F5FA Cargar Shapefile";
                itemLoad.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                itemLoad.ForeColor = Color.FromArgb(0, 140, 200);
                itemLoad.Click += (s, e) => OpenShapefileLoadDialog();
                toolStripDropDownButton1.DropDownItems.Add(itemLoad);

                shapefileStyleItem = new ToolStripMenuItem();
                shapefileStyleItem.Text = "\U0001F3A8 Estilo Shapefile";
                shapefileStyleItem.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                shapefileStyleItem.ForeColor = Color.FromArgb(0, 140, 200);
                shapefileStyleItem.Enabled = false;
                shapefileStyleItem.Click += (s, e) => OpenShapefileStyleDialog();
                toolStripDropDownButton1.DropDownItems.Add(shapefileStyleItem);

                shapefileToggleItem = new ToolStripMenuItem();
                shapefileToggleItem.Text = "\U0001F441 Mostrar Shapefile";
                shapefileToggleItem.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                shapefileToggleItem.ForeColor = Color.FromArgb(0, 140, 200);
                shapefileToggleItem.CheckOnClick = true;
                shapefileToggleItem.Checked = true;
                shapefileToggleItem.Enabled = false;
                shapefileToggleItem.CheckedChanged += (s, e) =>
                {
                    if (shapefileLayer != null)
                        shapefileLayer.IsVisible = shapefileToggleItem.Checked;
                    UpdateShapefileLegendVisibility();
                    SaveShapefileState();
                };
                toolStripDropDownButton1.DropDownItems.Add(shapefileToggleItem);

                shapefileInspectItem = new ToolStripMenuItem();
                shapefileInspectItem.Text = "\U0001F50D Modo inspeccion";
                shapefileInspectItem.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                shapefileInspectItem.ForeColor = Color.FromArgb(0, 140, 200);
                shapefileInspectItem.CheckOnClick = true;
                shapefileInspectItem.CheckedChanged += (s, e) =>
                {
                    shapefileInspectMode = shapefileInspectItem.Checked;
                    oglMain.Cursor = shapefileInspectMode ? Cursors.Cross : Cursors.Default;
                };
                toolStripDropDownButton1.DropDownItems.Add(shapefileInspectItem);

                var itemExport = new ToolStripMenuItem();
                itemExport.Text = "\U0001F4BE Exportar Shapefile...";
                itemExport.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                itemExport.ForeColor = Color.FromArgb(0, 140, 200);
                itemExport.Click += (s, e) => OpenShapefileExportDialog();
                toolStripDropDownButton1.DropDownItems.Add(itemExport);

                // QUANTIX_MOD_START
                var itemQuantiX = new ToolStripMenuItem();
                itemQuantiX.Text = "⚙ QuantiX (UDP)";
                itemQuantiX.Font = new Font("Tahoma", 18F, FontStyle.Bold);
                itemQuantiX.ForeColor = Color.FromArgb(230, 150, 30);
                itemQuantiX.Click += (s, e) => OpenQuantiXConfigDialog();
                toolStripDropDownButton1.DropDownItems.Add(itemQuantiX);
                // QUANTIX_MOD_END

                InitShapefileLegend();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Shapefile] InitShapefileMenu: "
                    + ex.Message);
            }
        }

        private void OpenShapefileLoadDialog()
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Title = "Cargar Shapefile";
                ofd.Filter = "Shapefile (*.shp)|*.shp|Todos (*.*)|*.*";

                string initial = RegistrySettings.fieldsDirectory;
                if (!string.IsNullOrEmpty(initial) && Directory.Exists(initial))
                    ofd.InitialDirectory = initial;

                if (ofd.ShowDialog(this) != DialogResult.OK) return;

                LoadShapefileFromPath(ofd.FileName, showSummary: true, promptStyle: true);
            }
        }

        // Carga un shapefile desde un path dado. Si promptStyle=true abre el
        // dialogo de estilo tras la carga; si ya hay StyleField guardado se aplica
        // en silencio (auto-load). Persiste el estado al final.
        private bool LoadShapefileFromPath(string shpPath, bool showSummary, bool promptStyle,
            string autoStyleField = null, bool autoVisible = true, bool autoShowFill = true,
            bool autoShowOutline = true)
        {
            ShapefileReadResult result;
            Cursor oldCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                result = ShapefileReader.ReadPolygons(shpPath);
            }
            catch (Exception ex)
            {
                Cursor.Current = oldCursor;
                System.Diagnostics.Debug.WriteLine("[Shapefile] Error leyendo "
                    + shpPath + ": " + ex);
                if (showSummary)
                {
                    MessageBox.Show(this,
                        "Error leyendo shapefile:\n\n" + ex.Message,
                        "Shapefile",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return false;
            }
            Cursor.Current = oldCursor;

            if (showSummary)
                ShowShapefileSummary(Path.GetFileName(shpPath), result);

            if (!isJobStarted)
            {
                if (showSummary)
                {
                    MessageBox.Show(this,
                        "No hay un campo abierto. Abri o crea un campo antes de cargar un "
                        + "shapefile para poder dibujarlo sobre el mapa.",
                        "Shapefile", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return false;
            }

            shapefileLayer = new ShapefileLayer(result, Path.GetFileName(shpPath));
            shapefileLayer.SourceFullPath = shpPath;
            shapefileLayer.ShowFill = autoShowFill;
            shapefileLayer.ShowOutline = autoShowOutline;
            shapefileLayer.IsVisible = autoVisible;

            if (shapefileToggleItem != null)
            {
                shapefileToggleItem.Enabled = !shapefileLayer.IsEmpty;
                shapefileToggleItem.Checked = autoVisible;
            }
            if (shapefileStyleItem != null)
            {
                // El estilo por DBF solo aplica al fill de poligonos.
                shapefileStyleItem.Enabled = shapefileLayer.PolygonCount > 0
                    && shapefileLayer.FieldNames.Count > 0;
            }

            // Auto-aplicar estilo si viene de persistencia.
            if (!string.IsNullOrEmpty(autoStyleField))
                shapefileLayer.ApplyColorByField(autoStyleField);

            // Dialogo de estilo solo en carga manual.
            if (promptStyle
                && shapefileLayer.PolygonCount > 0
                && shapefileLayer.FieldNames.Count > 0)
            {
                OpenShapefileStyleDialog();
            }

            RefreshShapefileLegend();
            SaveShapefileState();
            return true;
        }

        private void OpenShapefileStyleDialog()
        {
            if (shapefileLayer == null || shapefileLayer.PolygonCount == 0) return;

            using (var dlg = new FormShapefileStyle(shapefileLayer))
            {
                dlg.ShowDialog(this);
            }
            RefreshShapefileLegend();
            SaveShapefileState();
        }

        private string GetCurrentFieldFullPath()
        {
            if (string.IsNullOrEmpty(currentFieldDirectory)) return null;
            if (string.IsNullOrEmpty(RegistrySettings.fieldsDirectory)) return null;
            return Path.Combine(RegistrySettings.fieldsDirectory, currentFieldDirectory);
        }

        private void SaveShapefileState()
        {
            try
            {
                string fieldPath = GetCurrentFieldFullPath();
                if (string.IsNullOrEmpty(fieldPath)) return;
                if (!Directory.Exists(fieldPath)) return;

                if (shapefileLayer == null || string.IsNullOrEmpty(shapefileLayer.SourceFullPath))
                {
                    // Nada que persistir — si habia un archivo previo, borrarlo.
                    ShapefilePersistence.Delete(fieldPath);
                    return;
                }

                var cfg = new ShapefileFieldConfig
                {
                    ShpPath = shapefileLayer.SourceFullPath,
                    StyleField = shapefileLayer.StyleField,
                    Visible = shapefileLayer.IsVisible,
                    ShowFill = shapefileLayer.ShowFill,
                    ShowOutline = shapefileLayer.ShowOutline
                };
                ShapefilePersistence.Save(fieldPath, cfg);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Shapefile] SaveShapefileState: "
                    + ex.Message);
            }
        }

        public void TryAutoLoadShapefileForCurrentField()
        {
            try
            {
                string fieldPath = GetCurrentFieldFullPath();
                if (string.IsNullOrEmpty(fieldPath)) return;

                var cfg = ShapefilePersistence.Load(fieldPath);
                if (cfg == null || string.IsNullOrWhiteSpace(cfg.ShpPath)) return;
                if (!File.Exists(cfg.ShpPath))
                {
                    System.Diagnostics.Debug.WriteLine("[Shapefile] Auto-load: shape no existe "
                        + cfg.ShpPath);
                    return;
                }

                LoadShapefileFromPath(cfg.ShpPath,
                    showSummary: false, promptStyle: false,
                    autoStyleField: cfg.StyleField,
                    autoVisible: cfg.Visible,
                    autoShowFill: cfg.ShowFill,
                    autoShowOutline: cfg.ShowOutline);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[Shapefile] TryAutoLoad: " + ex.Message);
            }
        }

        public void ClearShapefileLayer()
        {
            shapefileLayer = null;
            if (shapefileToggleItem != null)
            {
                shapefileToggleItem.Enabled = false;
                shapefileToggleItem.Checked = false;
            }
            if (shapefileStyleItem != null)
                shapefileStyleItem.Enabled = false;
            RefreshShapefileLegend();
        }

        // Intenta procesar un click en modo inspeccion. Retorna true si lo
        // manejo (en cuyo caso el handler del mapa no debe hacer nada mas).
        private void OpenShapefileExportDialog()
        {
            if (shapefileLayer == null || shapefileLayer.IsEmpty)
            {
                MessageBox.Show(this,
                    "No hay shapefile cargado para exportar.",
                    "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (var sfd = new SaveFileDialog())
            {
                sfd.Title = "Exportar shapefile";
                sfd.Filter = "Shapefile (*.shp)|*.shp|KML (*.kml)|*.kml";
                sfd.DefaultExt = "shp";
                sfd.AddExtension = true;

                string fieldPath = GetCurrentFieldFullPath();
                if (!string.IsNullOrEmpty(fieldPath) && Directory.Exists(fieldPath))
                    sfd.InitialDirectory = fieldPath;

                string stem = string.IsNullOrEmpty(shapefileLayer.Source)
                    ? "export"
                    : Path.GetFileNameWithoutExtension(shapefileLayer.Source);
                sfd.FileName = stem + "_export";

                if (sfd.ShowDialog(this) != DialogResult.OK) return;

                Cursor oldCursor = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
                try
                {
                    IShapefileExportSource src = shapefileLayer;
                    bool isKml = sfd.FilterIndex == 2
                        || string.Equals(Path.GetExtension(sfd.FileName),
                            ".kml", StringComparison.OrdinalIgnoreCase);

                    if (isKml)
                        ShapefileExporter.ExportKml(src, sfd.FileName, shapefileLayer.Source);
                    else
                        ShapefileExporter.ExportShapefile(src, sfd.FileName);
                }
                catch (Exception ex)
                {
                    Cursor.Current = oldCursor;
                    System.Diagnostics.Debug.WriteLine("[Shapefile] Export: " + ex);
                    MessageBox.Show(this,
                        "Error exportando:\n\n" + ex.Message,
                        "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Cursor.Current = oldCursor;

                MessageBox.Show(this,
                    "Export completo:\n" + sfd.FileName,
                    "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public bool TryHandleShapefileInspectClick(int screenX, int screenY)
        {
            if (!shapefileInspectMode) return false;
            if (shapefileLayer == null || shapefileLayer.PolygonCount == 0) return true;
            if (!_glMatricesValid) return true;

            double east, north;
            if (!UnprojectMouseToGround(screenX, screenY, out east, out north)) return true;

            int idx = shapefileLayer.FindPolygonAt(east, north);
            if (idx < 0)
            {
                TimedMessageBox(1500, "Shapefile", "Click fuera de poligonos.");
                return true;
            }

            var attrs = shapefileLayer.GetPolygonAttributes(idx);
            double? curVal = null;
            if (!string.IsNullOrEmpty(shapefileLayer.StyleField))
            {
                double v;
                if (shapefileLayer.TryGetPolygonNumeric(idx, shapefileLayer.StyleField, out v))
                    curVal = v;
            }

            using (var dlg = new FormShapefileAttributes(
                idx, shapefileLayer.FieldNames, attrs,
                shapefileLayer.StyleField, curVal))
            {
                dlg.ShowDialog(this);
            }
            return true;
        }

        // Unproyecta (screenX, screenY) al plano z=0 del mundo usando las
        // matrices capturadas en el ultimo frame. (screenX, screenY) son
        // coords de oglMain (top-left origin).
        private bool UnprojectMouseToGround(int screenX, int screenY, out double east, out double north)
        {
            east = 0; north = 0;
            if (!_glMatricesValid) return false;

            int vpW = _glViewport[2];
            int vpH = _glViewport[3];
            if (vpW <= 0 || vpH <= 0) return false;

            // GL viewport tiene origen abajo-izq; oglMain abajo-arriba invertido.
            double ndcX = (2.0 * (screenX - _glViewport[0])) / vpW - 1.0;
            double ndcY = 1.0 - (2.0 * (screenY - _glViewport[1])) / vpH;

            // mvp = modelview * projection (convention columna-major GL; el
            // producto se hace en el mismo orden en el que GL aplica M y P:
            // p_clip = P * M * p_world, entonces invirtiendo:
            // p_world = (P * M)^(-1) * p_clip = invMvp * p_clip).
            double[] mvp = MulMat4(_glProjection, _glModelView);
            double[] inv;
            if (!InvertMat4(mvp, out inv)) return false;

            double nx, ny, nz;
            if (!TransformMat4Point(inv, ndcX, ndcY, -1.0, out nx, out ny, out nz)) return false;
            double fx, fy, fz;
            if (!TransformMat4Point(inv, ndcX, ndcY, 1.0, out fx, out fy, out fz)) return false;

            double dz = fz - nz;
            if (Math.Abs(dz) < 1e-9) return false;
            double t = -nz / dz;
            east = nx + t * (fx - nx);
            north = ny + t * (fy - ny);
            return true;
        }

        // Matrices column-major como GL las devuelve. Indice row r col c: a[c*4 + r].
        private static double[] MulMat4(double[] a, double[] b)
        {
            var r = new double[16];
            for (int c = 0; c < 4; c++)
                for (int row = 0; row < 4; row++)
                {
                    double s = 0;
                    for (int k = 0; k < 4; k++)
                        s += a[k * 4 + row] * b[c * 4 + k];
                    r[c * 4 + row] = s;
                }
            return r;
        }

        private static bool TransformMat4Point(double[] m, double x, double y, double z,
            out double ox, out double oy, out double oz)
        {
            double rx = m[0] * x + m[4] * y + m[8] * z + m[12];
            double ry = m[1] * x + m[5] * y + m[9] * z + m[13];
            double rz = m[2] * x + m[6] * y + m[10] * z + m[14];
            double rw = m[3] * x + m[7] * y + m[11] * z + m[15];
            if (Math.Abs(rw) < 1e-12) { ox = oy = oz = 0; return false; }
            ox = rx / rw; oy = ry / rw; oz = rz / rw;
            return true;
        }

        private static bool InvertMat4(double[] m, out double[] inv)
        {
            inv = new double[16];
            inv[0] = m[5] * m[10] * m[15] - m[5] * m[11] * m[14] - m[9] * m[6] * m[15] + m[9] * m[7] * m[14] + m[13] * m[6] * m[11] - m[13] * m[7] * m[10];
            inv[4] = -m[4] * m[10] * m[15] + m[4] * m[11] * m[14] + m[8] * m[6] * m[15] - m[8] * m[7] * m[14] - m[12] * m[6] * m[11] + m[12] * m[7] * m[10];
            inv[8] = m[4] * m[9] * m[15] - m[4] * m[11] * m[13] - m[8] * m[5] * m[15] + m[8] * m[7] * m[13] + m[12] * m[5] * m[11] - m[12] * m[7] * m[9];
            inv[12] = -m[4] * m[9] * m[14] + m[4] * m[10] * m[13] + m[8] * m[5] * m[14] - m[8] * m[6] * m[13] - m[12] * m[5] * m[10] + m[12] * m[6] * m[9];
            inv[1] = -m[1] * m[10] * m[15] + m[1] * m[11] * m[14] + m[9] * m[2] * m[15] - m[9] * m[3] * m[14] - m[13] * m[2] * m[11] + m[13] * m[3] * m[10];
            inv[5] = m[0] * m[10] * m[15] - m[0] * m[11] * m[14] - m[8] * m[2] * m[15] + m[8] * m[3] * m[14] + m[12] * m[2] * m[11] - m[12] * m[3] * m[10];
            inv[9] = -m[0] * m[9] * m[15] + m[0] * m[11] * m[13] + m[8] * m[1] * m[15] - m[8] * m[3] * m[13] - m[12] * m[1] * m[11] + m[12] * m[3] * m[9];
            inv[13] = m[0] * m[9] * m[14] - m[0] * m[10] * m[13] - m[8] * m[1] * m[14] + m[8] * m[2] * m[13] + m[12] * m[1] * m[10] - m[12] * m[2] * m[9];
            inv[2] = m[1] * m[6] * m[15] - m[1] * m[7] * m[14] - m[5] * m[2] * m[15] + m[5] * m[3] * m[14] + m[13] * m[2] * m[7] - m[13] * m[3] * m[6];
            inv[6] = -m[0] * m[6] * m[15] + m[0] * m[7] * m[14] + m[4] * m[2] * m[15] - m[4] * m[3] * m[14] - m[12] * m[2] * m[7] + m[12] * m[3] * m[6];
            inv[10] = m[0] * m[5] * m[15] - m[0] * m[7] * m[13] - m[4] * m[1] * m[15] + m[4] * m[3] * m[13] + m[12] * m[1] * m[7] - m[12] * m[3] * m[5];
            inv[14] = -m[0] * m[5] * m[14] + m[0] * m[6] * m[13] + m[4] * m[1] * m[14] - m[4] * m[2] * m[13] - m[12] * m[1] * m[6] + m[12] * m[2] * m[5];
            inv[3] = -m[1] * m[6] * m[11] + m[1] * m[7] * m[10] + m[5] * m[2] * m[11] - m[5] * m[3] * m[10] - m[9] * m[2] * m[7] + m[9] * m[3] * m[6];
            inv[7] = m[0] * m[6] * m[11] - m[0] * m[7] * m[10] - m[4] * m[2] * m[11] + m[4] * m[3] * m[10] + m[8] * m[2] * m[7] - m[8] * m[3] * m[6];
            inv[11] = -m[0] * m[5] * m[11] + m[0] * m[7] * m[9] + m[4] * m[1] * m[11] - m[4] * m[3] * m[9] - m[8] * m[1] * m[7] + m[8] * m[3] * m[5];
            inv[15] = m[0] * m[5] * m[10] - m[0] * m[6] * m[9] - m[4] * m[1] * m[10] + m[4] * m[2] * m[9] + m[8] * m[1] * m[6] - m[8] * m[2] * m[5];

            double det = m[0] * inv[0] + m[1] * inv[4] + m[2] * inv[8] + m[3] * inv[12];
            if (Math.Abs(det) < 1e-15) return false;
            double invDet = 1.0 / det;
            for (int i = 0; i < 16; i++) inv[i] *= invDet;
            return true;
        }

        private void InitShapefileLegend()
        {
            if (shapefileLegend != null) return;

            shapefileLegend = new ShapefileLegendControl();
            shapefileLegend.Visible = false;
            shapefileLegend.Anchor = AnchorStyles.Left | AnchorStyles.Bottom;
            // Sobre oglMain (oglMain.Left=75); margen izquierdo + encima del footer.
            shapefileLegend.Location = new Point(85, this.ClientSize.Height - shapefileLegend.Height - 90);
            this.Controls.Add(shapefileLegend);
            shapefileLegend.BringToFront();
        }

        private void RefreshShapefileLegend()
        {
            if (shapefileLegend == null) return;
            if (shapefileLayer != null && !string.IsNullOrEmpty(shapefileLayer.StyleField))
                shapefileLegend.SetLegend(shapefileLayer.StyleField, shapefileLayer.StyleMin, shapefileLayer.StyleMax);
            else
                shapefileLegend.Clear();
            UpdateShapefileLegendVisibility();
        }

        private void UpdateShapefileLegendVisibility()
        {
            if (shapefileLegend == null) return;
            bool show = shapefileLegend.HasData
                && shapefileLayer != null
                && shapefileLayer.IsVisible;
            shapefileLegend.Visible = show;
            if (show) shapefileLegend.BringToFront();
        }

        public void UpdateShapefileCurrentDose()
        {
            if (shapefileLegend == null || shapefileLayer == null) return;
            shapefileLegend.SetCurrent(
                shapefileLayer.CurrentDose,
                shapefileLayer.HasCurrentDose);

            // QUANTIX_MOD_START
            if (quantiXSender != null)
            {
                shapefileLegend.SetUdpStatus(
                    quantiXSender.IsRunning,
                    quantiXSender.PacketsSent,
                    quantiXSender.LastSendUtc);
            }
            else
            {
                shapefileLegend.SetUdpStatus(false, 0, null);
            }
            // QUANTIX_MOD_END
        }

        // QUANTIX_MOD_START
        private void StartQuantiXSender()
        {
            try
            {
                if (quantiXSender != null) return;

                if (quantiXConfig == null)
                    quantiXConfig = QuantiXConfig.Load();

                if (!quantiXConfig.Enabled)
                {
                    System.Diagnostics.Debug.WriteLine("[QuantiX] deshabilitado en quantiX.json");
                    return;
                }

                quantiXSender = new QuantiXSender(quantiXConfig, BuildQuantiXSample);
                quantiXSender.Start();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] StartSender: " + ex.Message);
            }
        }

        private void StopQuantiXSender()
        {
            try
            {
                if (quantiXSender == null) return;
                quantiXSender.Dispose();
                quantiXSender = null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] StopSender: " + ex.Message);
            }
        }

        private void OpenQuantiXConfigDialog()
        {
            try
            {
                if (quantiXConfig == null)
                    quantiXConfig = QuantiXConfig.Load();

                using (var dlg = new FormQuantiXConfig(quantiXConfig))
                {
                    if (dlg.ShowDialog(this) != DialogResult.OK) return;
                }

                // Reiniciar el sender con la nueva config. Si ahora esta
                // deshabilitado o cambio host/puerto/rate, Stop+Start aplica.
                StopQuantiXSender();
                if (isJobStarted && quantiXConfig.Enabled)
                    StartQuantiXSender();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] OpenConfigDialog: "
                    + ex.Message);
            }
        }

        private QuantiXSample BuildQuantiXSample()
        {
            var sample = new QuantiXSample();

            if (shapefileLayer != null)
            {
                sample.Inside = shapefileLayer.CurrentInside;
                sample.Dose = shapefileLayer.CurrentDose;
                sample.FieldName = shapefileLayer.StyleField;
            }

            // Posicion actual del pivot en WGS84 via la reproyeccion inversa.
            try
            {
                var plane = AppModel != null ? AppModel.LocalPlane : null;
                if (plane != null)
                {
                    var geo = new AgOpenGPS.Core.Models.GeoCoord(
                        pivotAxlePos.northing, pivotAxlePos.easting);
                    var ll = plane.ConvertGeoCoordToWgs84(geo);
                    sample.Latitude = ll.Latitude;
                    sample.Longitude = ll.Longitude;
                }
                sample.HeadingRad = pivotAxlePos.heading;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("[QuantiX] BuildSample pos: " + ex.Message);
            }

            return sample;
        }
        // QUANTIX_MOD_END

        private void ShowShapefileSummary(string fileName, ShapefileReadResult r)
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("Archivo: " + fileName);
            sb.AppendLine("Poligonos: " + r.Polygons.Count);
            sb.AppendLine("Lineas: " + r.Lines.Count);
            sb.AppendLine("Puntos: " + r.Points.Count);

            if (r.Polygons.Count + r.Lines.Count + r.Points.Count > 0)
            {
                sb.AppendLine(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Extent lat: {0:F6} .. {1:F6}", r.MinLat, r.MaxLat));
                sb.AppendLine(string.Format(
                    System.Globalization.CultureInfo.InvariantCulture,
                    "Extent lon: {0:F6} .. {1:F6}", r.MinLon, r.MaxLon));
            }

            sb.AppendLine();
            sb.AppendLine("Campos DBF (" + r.DbfFieldNames.Count + "):");
            if (r.DbfFieldNames.Count == 0)
            {
                sb.AppendLine("  (ninguno)");
            }
            else
            {
                int shown = Math.Min(r.DbfFieldNames.Count, 20);
                for (int i = 0; i < shown; i++)
                    sb.AppendLine("  - " + r.DbfFieldNames[i]);
                if (r.DbfFieldNames.Count > shown)
                    sb.AppendLine("  ... y " + (r.DbfFieldNames.Count - shown) + " mas");
            }

            if (r.Warnings.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("Advertencias:");
                foreach (var w in r.Warnings)
                    sb.AppendLine("  ! " + w);
            }

            MessageBox.Show(this, sb.ToString(), "Shapefile cargado",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // SHAPEFILE_MOD_END

        // COREX_FIELD_MOD_START
        /// <summary>
        /// Escribe current_field.json en la carpeta raíz de AgOpenGPS (Documents\AgOpenGPS\)
        /// para que CoreX y los módulos de AgroParallel detecten el campo activo sin AppData.
        ///
        /// Detecta automáticamente la acción:
        ///   "nuevo"    → directorio recién creado, sin datos previos
        ///   "abierto"  → campo diferente al último notificado, con datos existentes
        ///   "continuar"→ mismo campo que estaba activo la última vez
        ///   "cerrado"  → no hay campo activo (JobClose)
        /// </summary>
        private void NotificarCampoExterno_JobNew()
        {
            try
            {
                string nombre = currentFieldDirectory ?? "";
                if (string.IsNullOrEmpty(nombre)) return;

                string accion;
                if (nombre == _apLastFieldNotified)
                {
                    // Mismo campo que la última vez → el usuario eligió "Continuar"
                    accion = "continuar";
                }
                else
                {
                    // Campo distinto → determinar si ya tenía datos o es completamente nuevo
                    string fieldDir = Path.Combine(RegistrySettings.fieldsDirectory, nombre);
                    bool tieneDatos = false;
                    if (Directory.Exists(fieldDir))
                    {
                        // Archivos con contenido real indican campo preexistente
                        string[] archivosConDatos = { "Boundary.txt", "TrackLines.txt", "ABLines.txt" };
                        foreach (string arch in archivosConDatos)
                        {
                            string ruta = Path.Combine(fieldDir, arch);
                            if (File.Exists(ruta) && new FileInfo(ruta).Length > 10)
                            {
                                tieneDatos = true;
                                break;
                            }
                        }
                    }
                    accion = tieneDatos ? "abierto" : "nuevo";
                }

                _apLastFieldNotified = nombre;
                EscribirCurrentFieldJson(nombre, accion);
            }
            catch
            {
                // Nunca interrumpir AgOpenGPS por un error de notificación
            }
        }

        private void NotificarCampoExterno_JobClose()
        {
            try
            {
                _apLastFieldNotified = "";
                EscribirCurrentFieldJson("", "cerrado");
            }
            catch { }
        }

        /// <summary>
        /// Serializa el estado del campo a JSON sin dependencias externas.
        /// Ruta: Documents\AgOpenGPS\current_field.json
        /// </summary>
        private void EscribirCurrentFieldJson(string fieldName, string accion)
        {
            try
            {
                string nombreSeguro = (fieldName ?? "")
                    .Replace("\\", "\\\\")
                    .Replace("\"", "\\\"");

                long ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

                string json =
                    "{\r\n" +
                    "  \"fieldName\": \"" + nombreSeguro + "\",\r\n" +
                    "  \"accion\": \""    + accion       + "\",\r\n" +
                    "  \"ts\": "          + ts           + "\r\n" +
                    "}";

                // Intentar múltiples rutas en orden de prioridad
                var candidatos = new System.Collections.Generic.List<string>();

                // 1. RegistrySettings.baseDirectory (ruta oficial)
                string regBase = RegistrySettings.baseDirectory;
                if (!string.IsNullOrEmpty(regBase))
                    candidatos.Add(regBase);

                // 2. fieldsDirectory un nivel arriba (si baseDirectory falla)
                string regFields = RegistrySettings.fieldsDirectory;
                if (!string.IsNullOrEmpty(regFields))
                {
                    string parent = System.IO.Path.GetDirectoryName(regFields.TrimEnd('\\', '/'));
                    if (!string.IsNullOrEmpty(parent))
                        candidatos.Add(parent);
                }

                // 3. Carpetas estándar de Documents como fallback
                string docs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                candidatos.Add(Path.Combine(docs, "AgOpenGPS"));
                candidatos.Add(Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    "OneDrive", "Documents", "AgOpenGPS"));

                // 4. Último recurso: directorio del ejecutable
                candidatos.Add(Application.StartupPath);

                System.Diagnostics.Debug.WriteLine("[CoreX] baseDirectory = \"" + regBase + "\"");
                System.Diagnostics.Debug.WriteLine("[CoreX] fieldsDirectory = \"" + regFields + "\"");

                string filePath = null;
                foreach (string dir in candidatos)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(dir)) continue;
                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                        string ruta = Path.Combine(dir, "current_field.json");
                        File.WriteAllText(ruta, json, System.Text.Encoding.UTF8);
                        filePath = ruta;
                        System.Diagnostics.Debug.WriteLine(
                            "[CoreX] current_field.json escrito en: " + ruta);
                        break; // Éxito — no seguir intentando
                    }
                    catch (Exception exInner)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "[CoreX] Falló " + dir + ": " + exInner.Message);
                    }
                }

                if (filePath == null)
                    System.Diagnostics.Debug.WriteLine("[CoreX] ERROR: No se pudo escribir en ninguna ruta");
                else
                    System.Diagnostics.Debug.WriteLine(
                        "[CoreX] OK → " + accion + ": \"" + nombreSeguro + "\"");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(
                    "[CoreX] EXCEPCIÓN en EscribirCurrentFieldJson: " + ex.Message);
            }
        }

        /// <summary>
        /// El usuario confirmó el borrado del área trabajada.
        /// Escribe current_field.json con accion = "area_borrada" manteniendo el campo activo.
        /// CoreX / VistaX detectan esto para resetear el lote actual sin cerrarlo.
        /// </summary>
        public void NotificarBorradoArea()
        {
            try
            {
                string nombre = currentFieldDirectory ?? "";
                System.Diagnostics.Debug.WriteLine("[CoreX] NotificarBorradoArea → campo: \"" + nombre + "\"");
                EscribirCurrentFieldJson(nombre, "area_borrada");
            }
            catch { }
        }
        // COREX_FIELD_MOD_END
    }//class FormGPS
}//namespace AgOpenGPS



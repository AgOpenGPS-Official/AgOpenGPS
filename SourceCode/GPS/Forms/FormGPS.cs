//Please, if you use this, share the improvements

using AgOpenGPS;
using AgOpenGPS.Culture;
using AgOpenGPS.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Sockets;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace AgOpenGPS
{
    //the main form object
    public partial class FormGPS : Form
    {
        //To bring forward AgIO if running
        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(IntPtr handle);

        [System.Runtime.InteropServices.DllImport("User32.dll")]
        private static extern bool ShowWindow(IntPtr hWind, int nCmdShow);

        #region // Class Props and instances

        //maximum sections available
        public const int MAXSECTIONS = 64;

        //How many boundaries allowed
        public const int MAXBOUNDARIES = 6;

        //How many headlands allowed
        public const int MAXHEADS = 6;

        //The base directory where AgOpenGPS will be stored and fields and vehicles branch from
        public string baseDirectory;

        //current directory of vehicle
        public string vehiclesDirectory, vehicleFileName = "";

        //current directory of tools
        public string toolsDirectory, toolFileName = "";

        //current directory of Environments
        public string envDirectory, envFileName = "";

        //current fields and field directory
        public string fieldsDirectory, currentFieldDirectory, displayFieldName;

        private bool leftMouseDownOnOpenGL; //mousedown event in opengl window
        public int flagNumberPicked = 0;

        //bool for whether or not a job is active
        public bool isJobStarted = false, isBtnAutoSteerOn, isLidarBtnOn = true;

        //if we are saving a file
        public bool isSavingFile = false, isLogNMEA = false;

        //texture holders
        public uint[] texture;

        //the currentversion of software
        public string currentVersionStr, inoVersionStr;

        public int inoVersionInt;

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

        public int pbarSteer, pbarMachine, pbarUDP;

        public double nudNumber = 0;

        public double m2InchOrCm, inchOrCm2m, m2FtOrM, ftOrMtoM, cm2CmOrIn, inOrCm2Cm;
        public string unitsFtM, unitsInCm, unitsInCmNS;

        public char[] hotkeys;

        //used by filePicker Form to return picked file and directory
        public string filePickerFileAndDirectory;

        //the position of the GPS Data window within the FormGPS window
        public int GPSDataWindowLeft = 80, GPSDataWindowTopOffset = 220;

        //the autoManual drive button. Assume in Auto
        public bool isInAutoDrive = true;

        //isGPSData form up
        public bool isGPSSentencesOn = false, isKeepOffsetsOn = false;

        /// <summary>
        /// create the scene camera
        /// </summary>
        public CCamera camera = new CCamera();

        /// <summary>
        /// create world grid
        /// </summary>
        public CWorldGrid worldGrid;

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
        /// Resource manager for gloabal strings
        /// </summary>
        public ResourceManager _rm;

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

        /// <summary>
        /// The font class
        /// </summary>
        public CFont font;

        /// <summary>
        /// The new steer algorithms
        /// </summary>
        public CGuidance gyd;

        /// <summary>
        /// The new brightness code
        /// </summary>
        public CWindowsSettingsBrightnessController displayBrightness;

        #endregion // Class Props and instances

        //Enumeration to interpret ACLineStatus in a right manner
        public enum ACLineStatus : byte
        {
            Disconnected = 0,
            Charging = 1,
            Unknown = 255
        }

        //A structure to access returned data properly and a method to reach pursued goal
        [StructLayout(LayoutKind.Sequential)]
        public class PowerState
        {
            //The only parameter we are interested in, so it's the only parameter interpreted
            //Make sure to keep all the other parameters with relevant types in this class, otherwise you can face bugs
            private ACLineStatus ACLineStatus;
            private byte BatteryFlag;
            private byte Reserved1;
            private int BatteryLifeTime;
            private int BatteryFullLifeTime;

            //Win32 api function import
            [DllImport("Kernel32", EntryPoint = "GetSystemPowerStatus")]
            private static extern bool GetSystemPowerStatusByRef(PowerState ps);

            //The method we use to get the current status of AC Power Source connection
            public static ACLineStatus GetPowerLineStatus()
            {
                PowerState ps = new PowerState();

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && GetSystemPowerStatusByRef(ps))
                    return ps.ACLineStatus;

                return ACLineStatus.Unknown;
            }
        }

        //The method assigned to the PowerModeChanged event call
        private void SystemEvents_PowerModeChanged(object sender, Microsoft.Win32.PowerModeChangedEventArgs e)
        {
            //We are interested only in StatusChange cases
            if (e.Mode.HasFlag(Microsoft.Win32.PowerModes.StatusChange))
            {
                string bob = PowerState.GetPowerLineStatus().ToString();

                SystemEventWriter("Power Line Status Change to: " + bob);

                if (bob == "Charging")
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


                if (Properties.Settings.Default.setDisplay_isShutdownWhenNoPower && bob == "Disconnected")
                {
                    Close();
                }
            }
        }

        public FormGPS()
        {
            //winform initialization
            InitializeComponent();

            CheckSettingsNotNull();

            //time keeper
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            //create the world grid
            worldGrid = new CWorldGrid(this);

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

            //resource for gloabal language strings
            _rm = new ResourceManager("AgOpenGPS.gStr", Assembly.GetExecutingAssembly());

            //access to font class
            font = new CFont(this);

            //the new steer algorithms
            gyd = new CGuidance(this);

            //sounds class
            sounds = new CSound();

            //brightness object class
            displayBrightness = new CWindowsSettingsBrightnessController(Properties.Settings.Default.setDisplay_isBrightnessOn);
        }

        private void FormGPS_Load(object sender, EventArgs e)
        {
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
            SetLanguage(Settings.Default.setF_culture, false);

            currentVersionStr = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            string[] fullVers = currentVersionStr.Split('.');
            int inoV = int.Parse(fullVers[0], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[1], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[2], CultureInfo.InvariantCulture);
            inoVersionInt = inoV;
            inoVersionStr = inoV.ToString();

            if (Settings.Default.setF_workingDirectory == "Default")
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\AgOpenGPS\\";
            else baseDirectory = Settings.Default.setF_workingDirectory + "\\AgOpenGPS\\";

            //get the fields directory, if not exist, create
            fieldsDirectory = baseDirectory + "Fields\\";
            string dir = Path.GetDirectoryName(fieldsDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //get the fields directory, if not exist, create
            vehiclesDirectory = baseDirectory + "Vehicles\\";
            dir = Path.GetDirectoryName(vehiclesDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //get the abLines directory, if not exist, create
            ablinesDirectory = baseDirectory + "ABLines\\";
            dir = Path.GetDirectoryName(fieldsDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            //system event log file
            FileInfo txtfile = new FileInfo("zSystemEventsLog_log.txt");
            if (txtfile.Exists)
            {
                if (txtfile.Length > (500000))       // ## NOTE: 0.5MB max file size
                {
                    StringBuilder sbF = new StringBuilder();
                    long lines = txtfile.Length - 450000;

                    //create some extra space
                    lines /= 30;

                    using (StreamReader reader = new StreamReader("zSystemEventsLog_log.txt"))
                    {
                        try
                        {
                            //Date time line
                            for (long i = 0; i < lines; i++)
                            {
                                reader.ReadLine();
                            }

                            while (!reader.EndOfStream)
                            {
                                sbF.AppendLine(reader.ReadLine());
                            }
                        }
                        catch { }
                    }

                    using (StreamWriter writer = new StreamWriter("zSystemEventsLog_log.txt"))
                    {
                        writer.WriteLine(sbF);
                    }
                }
            }

            //make sure current field directory exists, null if not
            currentFieldDirectory = Settings.Default.setF_CurrentDir;

            string curDir;
            if (currentFieldDirectory != "")
            {
                curDir = fieldsDirectory + currentFieldDirectory + "//";
                dir = Path.GetDirectoryName(curDir);
                if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir))
                {
                    currentFieldDirectory = "";
                    Settings.Default.setF_CurrentDir = "";
                    Settings.Default.Save();
                }
            }

            sbSystemEvents.Append("AOG Version: ");
            sbSystemEvents.Append(Application.ProductVersion.ToString(CultureInfo.InvariantCulture));
            sbSystemEvents.Append("\r");

            sbSystemEvents.Append("Current Field Directory: ");
            sbSystemEvents.Append(fieldsDirectory + currentFieldDirectory);
            sbSystemEvents.Append("\r");

            FileSaveSystemEvents();
            sbSystemEvents.Clear();


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

            if (Properties.Settings.Default.setDisplay_isAutoStartAgIO)
            {
                //Start AgIO process
                Process[] processName = Process.GetProcessesByName("AgIO");
                if (processName.Length == 0)
                {
                    //Start application here
                    DirectoryInfo di = new DirectoryInfo(Application.StartupPath);
                    string strPath = di.ToString();
                    strPath += "\\AgIO.exe";
                    try
                    {
                        ProcessStartInfo processInfo = new ProcessStartInfo
                        {
                            FileName = strPath,
                            WorkingDirectory = Path.GetDirectoryName(strPath)
                        };
                        Process proc = Process.Start(processInfo);
                    }
                    catch
                    {
                        TimedMessageBox(2000, "No File Found", "Can't Find AgIO");
                        SystemEventWriter("Can't Find AgIO");

                    }
                }
            }

            //nmea limiter
            udpWatch.Start();

            ControlExtension.Draggable(panelDrag, true);

            enterSimCoordsToolStripMenuItem.Text = gStr.gsEnterSimCoords;
            aboutToolStripMenuItem.Text = gStr.gsAbout;
            menustripLanguage.Text = gStr.gsLanguage;

            simulatorOnToolStripMenuItem.Text = gStr.gsSimulatorOn;
            resetALLToolStripMenuItem.Text = gStr.gsResetAll;

            kioskModeToolStrip.Text= gStr.gsKioskMode;

            helpMenuItem.Text = gStr.gsHelp;

            toolStripColors.Text = gStr.gsColors;
            // toolStripSectionColors.Text = "Section " + gStr.gsColors;
            toolStripSectionColors.Text = gStr.gsSectionColors;
            toolStripConfig.Text = gStr.gsConfiguration;
            toolStripSteerSettings.Text = gStr.gsAutoSteer;
            toolStripAllSettings.Text = gStr.gsViewAllSettings;
            toolStripWorkingDirectories.Text = gStr.gsDirectories;

            toolStripGPSData.Text = gStr.gsGpsData;

            resetEverythingToolStripMenuItem.Text = gStr.gsResetAllForSure;
            steerChartStripMenu.Text = gStr.gsCharts;
            
            
            //Tools Menu
            SmoothABtoolStripMenu.Text = gStr.gsSmoothABCurve;
            boundariesToolStripMenuItem.Text = gStr.gsBoundary;
            headlandToolStripMenuItem.Text = gStr.gsHeadland;
            headlandBuildToolStripMenuItem.Text = gStr.gsHeadlandBuilder;
            deleteContourPathsToolStripMenuItem.Text = gStr.gsDeleteContourPaths;
            deleteAppliedToolStripMenuItem.Text = gStr.gsDeleteAppliedArea;
            tramLinesMenuField.Text = gStr.gsTramLines;
            recordedPathStripMenu.Text = gStr.gsRecordedPathMenu;
            flagByLatLonToolStripMenuItem.Text = gStr.gsFlagByLatLon;
            boundaryToolToolStripMenu.Text = gStr.gsBoundary + " Tool";

            webcamToolStrip.Text = gStr.gsWebCam;
            offsetFixToolStrip.Text = gStr.gsOffsetFix;
            wizardsMenu.Text = gStr.gsWizards;
            steerWizardMenuItem.Text = gStr.gsSteerWizard;
            steerChartToolStripMenuItem.Text = gStr.gsSteerChart;
            headingChartToolStripMenuItem.Text = gStr.gsHeadingChart;
            xTEChartToolStripMenuItem.Text = gStr.gsXTEChart;
            rollCheckToolStripMenuItem.Text = gStr.gsRollChack;
            
            eventViewerToolStripMenuItem.Text = gStr.gsEventViewer;

            guidelinesToolStripMenuItem.Text = gStr.gsExtraGuides;




            btnChangeMappingColor.Text = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
            //btnChangeMappingColor.Text = btnChangeMappingColor.Text.Substring(2);

            hotkeys = new char[19];

            hotkeys = Properties.Settings.Default.setKey_hotkeys.ToCharArray();

            if (!isTermsAccepted)
            {
                if (!Properties.Settings.Default.setDisplay_isTermsAccepted)
                {
                    using (var form = new Form_First(this))
                    {
                        if (form.ShowDialog(this) != DialogResult.OK)
                        {
                            Close();
                        }
                    }
                }
            }
        }

        private void FormGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = Application.OpenForms["FormFieldData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            f = Application.OpenForms["FormPan"];

            if (f != null)
            {
                isPanFormVisible = false;
                f.Focus();
                f.Close();
            }

            f = Application.OpenForms["FormTimedMessage"];

            if (f != null)
            {
                f.Focus();
                f.Close();
            }

            if (this.OwnedForms.Any())
            {
                TimedMessageBox(2000, gStr.gsWindowsStillOpen, gStr.gsCloseAllWindowsFirst);
                e.Cancel = true;
                return;
            }

            bool closing = true;
            int choice = SaveOrNot(closing);

            //simple cancel return to AOG
            if (choice == 1)
            {
                e.Cancel = true;
                return;
            }

            if (isJobStarted)
            {
                if (autoBtnState == btnStates.Auto)
                    btnSectionMasterAuto.PerformClick();

                if (manualBtnState == btnStates.On)
                    btnSectionMasterManual.PerformClick();

                FileSaveEverythingBeforeClosingField();
            }

            SaveFormGPSWindowSettings();
            FileUpdateAllFieldsKML();
            //save current vehicle
            SettingsIO.ExportAll(vehiclesDirectory + vehicleFileName + ".XML");

            if (displayBrightness.isWmiMonitor)
                displayBrightness.SetBrightness(Settings.Default.setDisplay_brightnessSystem);

            if (choice == 2)
            {
                Process[] processName = Process.GetProcessesByName("AgIO");
                if (processName.Length != 0)
                {
                    processName[0].CloseMainWindow();
                }

                Process.Start("shutdown", "/s /t 0");
            }

            if (loopBackSocket != null)
            {
                try
                {
                    loopBackSocket.Shutdown(SocketShutdown.Both);
                    loopBackSocket.Close();
                }
                catch { }
                finally { }
            }

            if (Properties.Settings.Default.setDisplay_isAutoOffAgIO)
            {
                Process[] processName = Process.GetProcessesByName("AgIO");
                if (processName.Length != 0)
                {
                    processName[0].CloseMainWindow();
                }
            }
        }

        public int SaveOrNot(bool closing)
        {
            CloseTopMosts();

            using (FormSaveOrNot form = new FormSaveOrNot(closing))
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
                f.Top = this.Top + 90;
                f.Left = this.Left + 120;
            }
        }

        // Return True if a certain percent of a rectangle is shown across the total screen area of all monitors, otherwise return False.
        public bool IsOnScreen(System.Drawing.Point RecLocation, System.Drawing.Size RecSize, double MinPercentOnScreen = 0.8)
        {
            double PixelsVisible = 0;
            System.Drawing.Rectangle Rec = new System.Drawing.Rectangle(RecLocation, RecSize);

            foreach (Screen Scrn in Screen.AllScreens)
            {
                System.Drawing.Rectangle r = System.Drawing.Rectangle.Intersect(Rec, Scrn.WorkingArea);
                // intersect rectangle with screen
                if (r.Width != 0 & r.Height != 0)
                {
                    PixelsVisible += (r.Width * r.Height);
                    // tally visible pixels
                }
            }
            return PixelsVisible >= (Rec.Width * Rec.Height) * MinPercentOnScreen;
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

        public void CheckSettingsNotNull()
        {
            if (Settings.Default.setFeatures == null)
            {
                Settings.Default.setFeatures = new CFeatureSettings();
            }
        }

        public enum textures : uint
        {
            Floor, Font,
            Turn, TurnCancel, TurnManual,
            Compass, Speedo, SpeedoNeedle,
            Lift, SteerPointer,
            SteerDot, Tractor, QuestionMark,
            FrontWheels, FourWDFront, FourWDRear,
            Harvester,
            Lateral, bingGrid,
            NoGPS, ZoomIn48, ZoomOut48,
            Pan, MenuHideShow,
            ToolWheels, Tire, TramDot,
            YouTurnU, YouTurnH, CrossTrackBkgrnd
        }

        public void LoadGLTextures()
        {
            GL.Enable(EnableCap.Texture2D);

            Bitmap[] oglTextures = new Bitmap[]
            {
                Resources.z_Floor,Resources.z_Font,
                Resources.z_Turn,Resources.z_TurnCancel,Resources.z_TurnManual,
                Resources.z_Compass,Resources.z_Speedo,Resources.z_SpeedoNeedle,
                Resources.z_Lift,Resources.z_SteerPointer,
                Resources.z_SteerDot,GetTractorBrand(Settings.Default.setBrand_TBrand),Resources.z_QuestionMark,
                Resources.z_FrontWheels,Get4WDBrandFront(Settings.Default.setBrand_WDBrand),
                Get4WDBrandRear(Settings.Default.setBrand_WDBrand),
                GetHarvesterBrand(Settings.Default.setBrand_HBrand),
                Resources.z_LateralManual, Resources.z_bingMap,
                Resources.z_NoGPS, Resources.ZoomIn48, Resources.ZoomOut48,
                Resources.Pan, Resources.MenuHideShow,
                Resources.z_Tool, Resources.z_Tire, Resources.z_TramOnOff,
                Resources.YouTurnU, Resources.YouTurnH, Resources.z_crossTrackBkgnd
            };

            texture = new uint[oglTextures.Length];

            for (int h = 0; h < oglTextures.Length; h++)
            {
                using (Bitmap bitmap = oglTextures[h])
                {
                    GL.GenTextures(1, out texture[h]);
                    GL.BindTexture(TextureTarget.Texture2D, texture[h]);
                    BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                    GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                    bitmap.UnlockBits(bitmapData);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                }
            }
        }

        public bool KeypadToNUD(NudlessNumericUpDown sender, Form owner)
        {
            var colour = sender.BackColor;
            sender.BackColor = Color.Red;
            sender.Value = Math.Round(sender.Value, sender.DecimalPlaces);

            using (FormNumeric form = new FormNumeric((double)sender.Minimum, (double)sender.Maximum, (double)sender.Value))
            {
                DialogResult result = form.ShowDialog(owner);
                if (result == DialogResult.OK)
                {
                    sender.Value = (decimal)form.ReturnValue;
                    sender.BackColor = colour;
                    return true;
                }
                else if (result == DialogResult.Cancel)
                {
                    sender.BackColor = colour;
                }
                return false;
            }
        }

        public void KeyboardToText(TextBox sender, Form owner)
        {
            var colour = sender.BackColor;
            sender.BackColor = Color.Red;
            using (FormKeyboard form = new FormKeyboard(sender.Text))
            {
                if (form.ShowDialog(owner) == DialogResult.OK)
                {
                    sender.Text = form.ReturnString;
                }
            }
            sender.BackColor = colour;
        }

        //request a new job
        public void JobNew()
        {
            //SendSteerSettingsOutAutoSteerPort();
            isJobStarted = true;
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
                pn.fixOffset.easting = 0;
                pn.fixOffset.northing = 0;
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
            isJobStarted = false;

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
            btnAutoSteer.Image = Properties.Resources.AutoSteerOff;

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

            displayFieldName = gStr.gsNone;

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

            using (Bitmap bitmap = Properties.Resources.z_bingMap)
            {
                GL.GenTextures(1, out texture[(int)FormGPS.textures.bingGrid]);
                GL.BindTexture(TextureTarget.Texture2D, texture[(int)FormGPS.textures.bingGrid]);
                BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                bitmap.UnlockBits(bitmapData);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
            }
        }

        public void FieldMenuButtonEnableDisable(bool isOn)
        {
            SmoothABtoolStripMenu.Enabled = isOn;
            deleteContourPathsToolStripMenuItem.Enabled = isOn;
            boundaryToolToolStripMenu.Enabled = isOn;
            offsetFixToolStrip.Enabled = isOn;

            boundariesToolStripMenuItem.Enabled = isOn;
            headlandToolStripMenuItem.Enabled = isOn;
            headlandBuildToolStripMenuItem.Enabled = isOn;
            flagByLatLonToolStripMenuItem.Enabled = isOn;
            tramLinesMenuField.Enabled = isOn;
            recordedPathStripMenu.Enabled = isOn;
        }

        //take the distance from object and convert to camera data
        public void SetZoom()
        {
            //match grid to cam distance and redo perspective
            camera.gridZoom = camera.camSetDistance / -15;

            gridToolSpacing = (int)(camera.gridZoom / tool.width + 0.5);
            if (gridToolSpacing < 1) gridToolSpacing = 1;
            camera.gridZoom = gridToolSpacing * tool.width;

            oglMain.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView((float)fovy, oglMain.AspectRatio, 1f, (float)(camDistanceFactor * camera.camSetDistance));
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        //All the files that need to be saved when closing field or app
        //an error log called by all try catches
        public void WriteErrorLog(string strErrorText)
        {
            try
            {
                //set up file and folder if it doesn't exist
                const string strFileName = "Error Log.txt";
                //string strPath = Application.StartupPath;

                //Write out the error appending to existing
                File.AppendAllText(baseDirectory + "\\" + strFileName, strErrorText + " - " +
                    DateTime.Now.ToString() + "\r\n\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in WriteErrorLog: " + ex.Message, "Error Logging", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //message box pops up with info then goes away
        public void TimedMessageBox(int timeout, string s1, string s2)
        {
            FormTimedMessage form = new FormTimedMessage(timeout, s1, s2);
            form.Show(this);
        }

        public void YesMessageBox(string s1)
        {
            var form = new FormYes(s1);
            form.ShowDialog(this);
        }

        // Generates a random number within a range.
        public double RandomNumber(double min, double max)
        {
            return min + _random.NextDouble() * (max - min);
        }

        private readonly Random _random = new Random();

    }//class FormGPS
}//namespace AgOpenGPS



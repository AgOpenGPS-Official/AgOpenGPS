using System;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.Testing
{
    /// <summary>
    /// Concrete implementation of test orchestrator that wires up FormGPS
    /// with all the testing controllers
    /// </summary>
    public class TestOrchestrator
    {
        private FormGPS formGPS;
        private bool isHeadless;
        private bool isInitialized;
        private double currentSimulationTime;

        public IFieldController FieldController { get; private set; }
        public ISimulatorController SimulatorController { get; private set; }
        public IAutosteerController AutosteerController { get; private set; }
        public IUTurnController UTurnController { get; private set; }
        public IPathLogger PathLogger { get; private set; }

        public double CurrentSimulationTime => currentSimulationTime;
        public bool IsInitialized => isInitialized;

        /// <summary>
        /// Initialize the test orchestrator and underlying FormGPS in headless mode
        /// </summary>
        /// <param name="headless">If true, skip UI initialization and rendering</param>
        public void Initialize(bool headless = true)
        {
            if (isInitialized)
            {
                throw new InvalidOperationException("TestOrchestrator already initialized");
            }

            isHeadless = headless;
            currentSimulationTime = 0;

            // Load registry settings (required before creating FormGPS)
            RegistrySettings.Load();

            // Auto-accept terms for testing to avoid dialog popup
            // Set in-memory only (don't save to disk) to avoid changing user's settings
            Properties.Settings.Default.setDisplay_isTermsAccepted = true;

            // Create FormGPS in headless mode
            formGPS = new FormGPS(headless);

            // Wait a moment for initialization to complete
            System.Threading.Thread.Sleep(100);

            // CRITICAL: In headless mode, set headingFromSource because LoadSettings() is not called
            // (FormGPS_Load event only fires when form is shown, which doesn't happen in headless).
            // Without this, UpdateFixPosition's switch statement skips all heading calculation logic.
            if (headless)
            {
                formGPS.headingFromSource = Properties.Settings.Default.setGPS_headingFromWhichSource;
            }

            // Initialize all controllers with the FormGPS instance
            FieldController = new Controllers.FieldController(formGPS);
            SimulatorController = new Controllers.SimulatorController(formGPS);
            AutosteerController = new Controllers.AutosteerController(formGPS);
            UTurnController = new Controllers.UTurnController(formGPS);
            PathLogger = new Controllers.PathLogger(formGPS);

            isInitialized = true;
        }

        /// <summary>
        /// Advance simulation by a time step
        /// </summary>
        /// <param name="deltaTimeSeconds">Time step in seconds (typically 0.1 for 10Hz)</param>
        public void StepSimulation(double deltaTimeSeconds)
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("TestOrchestrator not initialized");
            }

            currentSimulationTime += deltaTimeSeconds;

            // Step the simulator which will trigger FormGPS.UpdateFixPosition()
            // The simulator works by setting stepDistance and then calling DoSimTick
            // which internally calls UpdateFixPosition()

            // CRITICAL: Increment counters that are normally incremented by tmrWatchdog_tick
            // The U-turn logic requires makeUTurnCounter >= 4 before it can build turns
            // tmrWatchdog_tick fires every 125ms, so we simulate that by incrementing
            // the counter on each step (which is typically 50-100ms)
            formGPS.makeUTurnCounter++;

            // Get steer angle: use commanded angle from autosteer if active, otherwise use actual
            // In headless mode with autosteer ON, use guidanceLineSteerAngle (commanded by autosteer)
            // In headless mode with autosteer OFF, use actualSteerAngleDegrees (manual control)
            double steerAngle;
            if (formGPS.isBtnAutoSteerOn)
            {
                // Autosteer is commanding - use guidanceLineSteerAngle (in hundredths of degrees)
                steerAngle = formGPS.guidanceLineSteerAngle / 100.0;
            }
            else
            {
                // Manual control - use the actual steer angle that was set
                steerAngle = formGPS.mc.actualSteerAngleDegrees;
            }

            // CSim.DoSimTick assumes 10Hz (0.1s ticks) in its hardcoded formulas
            // To maintain correct simulation without modifying CSim.cs, we call it
            // multiple times at 0.1s intervals to match the requested deltaTimeSeconds
            const double CSIM_TICK_RATE = 0.1; // CSim assumes 10Hz
            int numTicks = (int)Math.Round(deltaTimeSeconds / CSIM_TICK_RATE);
            if (numTicks < 1) numTicks = 1; // At least one tick

            for (int i = 0; i < numTicks; i++)
            {
                formGPS.sim.DoSimTick(steerAngle);

                // CSim.DoSimTick incorrectly calculates vtgSpeed with formula: 4 * stepDistance * 10
                // We need to fix it after each tick. stepDistance is in meters per 0.1s tick,
                // so the correct speed is: stepDistance * 10 (to get m/s)
                double correctSpeedMs = Math.Abs(formGPS.sim.stepDistance * 10.0);
                double correctSpeedKph = correctSpeedMs * 3.6;

                formGPS.pn.vtgSpeed = Math.Abs(Math.Round(correctSpeedMs, 2));
                formGPS.pn.speed = Math.Abs(Math.Round(correctSpeedKph, 2));

                // Also need to recalculate avgSpeed since AverageTheSpeed() was already called with wrong value
                formGPS.pn.AverageTheSpeed();
            }

            // Update section control in headless mode (normally called by Paint handler)
            // This allows section control logic to run without requiring OpenGL Paint events
            if (isHeadless)
            {
                formGPS.UpdateSectionControl();
            }

            // If path logging is active, log the current state
            if (PathLogger != null && ((Controllers.PathLogger)PathLogger).IsLogging)
            {
                PathLogger.LogCurrentState(currentSimulationTime);
            }

            // Process UI events if in visual mode
            if (!isHeadless)
            {
                // Force form to refresh/redraw
                formGPS.Refresh();
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <summary>
        /// Show the FormGPS window (only works in non-headless mode)
        /// </summary>
        public void ShowForm()
        {
            if (isHeadless)
            {
                throw new InvalidOperationException("Cannot show form in headless mode. Initialize with headless: false");
            }

            if (formGPS != null && !formGPS.Visible)
            {
                formGPS.Show();
                System.Windows.Forms.Application.DoEvents();
            }
        }

        /// <summary>
        /// Get whether the form is in headless mode
        /// </summary>
        public bool IsHeadless => isHeadless;

        /// <summary>
        /// Shutdown the orchestrator and clean up resources
        /// </summary>
        public void Shutdown()
        {
            if (!isInitialized)
            {
                return;
            }

            // Stop any logging
            if (PathLogger != null && ((Controllers.PathLogger)PathLogger).IsLogging)
            {
                PathLogger.StopLogging();
            }

            // Cleanup controllers
            FieldController = null;
            SimulatorController = null;
            AutosteerController = null;
            UTurnController = null;
            PathLogger = null;

            // Close FormGPS if needed
            // Dispose directly to bypass all the shutdown dialogs and delays
            if (formGPS != null)
            {
                try
                {
                    if (!formGPS.IsDisposed)
                    {
                        formGPS.Dispose();
                    }
                }
                catch { }
            }

            formGPS = null;
            isInitialized = false;
        }

        /// <summary>
        /// Get direct access to FormGPS (for advanced testing scenarios)
        /// </summary>
        public FormGPS GetFormGPS()
        {
            return formGPS;
        }
    }
}

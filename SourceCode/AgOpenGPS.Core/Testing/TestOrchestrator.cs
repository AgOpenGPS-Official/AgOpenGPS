using System;

namespace AgOpenGPS.Core.Testing
{
    /// <summary>
    /// Central coordinator for integration testing that wraps FormGPS and provides
    /// testable interfaces for field management, simulation, autosteer, and U-turns.
    /// </summary>
    public class TestOrchestrator
    {
        private object formGPS; // Will be FormGPS but avoiding direct dependency
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
        /// Initialize the test orchestrator and underlying FormGPS
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

            // Note: Actual FormGPS initialization will be implemented by concrete class
            // that has access to FormGPS. This is the interface definition.

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

            // Note: Actual simulation step will be implemented by concrete class
            // This will call FormGPS update methods
        }

        /// <summary>
        /// Shutdown the orchestrator and clean up resources
        /// </summary>
        public void Shutdown()
        {
            if (!isInitialized)
            {
                return;
            }

            // Cleanup controllers
            FieldController = null;
            SimulatorController = null;
            AutosteerController = null;
            UTurnController = null;
            PathLogger = null;

            formGPS = null;
            isInitialized = false;
        }

        /// <summary>
        /// Get direct access to FormGPS (for advanced testing scenarios)
        /// </summary>
        public object GetFormGPS()
        {
            return formGPS;
        }
    }
}

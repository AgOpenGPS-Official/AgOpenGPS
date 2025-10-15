using NUnit.Framework;
using FluentAssertions;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    /// <summary>
    /// Basic tests to verify simulator controller functionality
    /// </summary>
    [TestFixture]
    public class SimulatorBasicTests
    {
        private AgOpenGPS.Testing.TestOrchestrator orchestrator;

        [SetUp]
        public void Setup()
        {
            orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: true);
        }

        [TearDown]
        public void Teardown()
        {
            orchestrator?.Shutdown();
        }

        [Test]
        public void Test_Simulator_SetPosition()
        {
            // Arrange
            var simController = orchestrator.SimulatorController;
            simController.Enable();

            double targetLat = 39.0;
            double targetLon = -94.0;

            // Act
            simController.SetPosition(targetLat, targetLon);
            var state = simController.GetState();

            // Assert
            state.Position.Latitude.Should().BeApproximately(targetLat, 0.0001);
            state.Position.Longitude.Should().BeApproximately(targetLon, 0.0001);
        }

        [Test]
        public void Test_Simulator_SetHeading()
        {
            // Arrange
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);

            double targetHeading = 45.0;

            // Act
            simController.SetHeading(targetHeading);
            var state = simController.GetState();

            // Assert
            state.HeadingDegrees.Should().BeApproximately(targetHeading, 0.1);
        }

        [Test]
        public void Test_Simulator_SetSpeed()
        {
            // Arrange
            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPosition(39.0, -94.0);

            double targetSpeed = 8.0; // 8 km/h

            // Act
            simController.SetSpeed(targetSpeed);
            var state = simController.GetState();

            // Assert
            state.SpeedKph.Should().BeApproximately(targetSpeed, 0.5);
        }
    }
}

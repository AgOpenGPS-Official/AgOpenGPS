using System;
using NUnit.Framework;
using AgOpenGPS.Core.Testing;

namespace AgOpenGPS.IntegrationTests.Tests
{
    [TestFixture]
    public class SteeringTest
    {
        [Test]
        public void Test_ManualSteering()
        {
            var orchestrator = new AgOpenGPS.Testing.TestOrchestrator();
            orchestrator.Initialize(headless: true);

            var fieldController = orchestrator.FieldController;
            fieldController.CreateNewField("SteeringTest", 39.0, -94.0);

            var simController = orchestrator.SimulatorController;
            simController.Enable();
            simController.SetPositionLocal(0, 0);
            simController.SetHeading(0.0);
            simController.SetSpeed(8.0);
            simController.SetSteerAngle(20.0); // Turn right 20 degrees

            Console.WriteLine("Testing manual steering with 20 degree angle...");

            for (int i = 0; i < 100; i++)
            {
                orchestrator.StepSimulation(0.05);

                if (i % 20 == 0)
                {
                    var state = simController.GetState();
                    Console.WriteLine("Step " + i + ": E=" + state.Easting.ToString("F2") + ", N=" + state.Northing.ToString("F2") +
                        ", Heading=" + state.HeadingDegrees.ToString("F2") + ", Steer=" + state.SteerAngleDegrees.ToString("F2"));
                }
            }

            var finalState = simController.GetState();
            Console.WriteLine("\nFinal: E=" + finalState.Easting.ToString("F2") + ", N=" + finalState.Northing.ToString("F2") +
                ", Heading=" + finalState.HeadingDegrees.ToString("F2"));
            Console.WriteLine("Heading changed: " + (finalState.HeadingDegrees > 5).ToString());

            orchestrator.Shutdown();
        }
    }
}

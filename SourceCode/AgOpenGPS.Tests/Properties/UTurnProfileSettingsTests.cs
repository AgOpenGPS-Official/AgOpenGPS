using System;
using System.IO;
using AgOpenGPS.Properties;
using NUnit.Framework;

namespace AgOpenGPS.Tests.Properties
{
    // The U-turn settings moved from the shared environment file into the tool profile.
    // Tool profiles saved before the move must get the old global values.
    public class UTurnProfileSettingsTests
    {
        private string baseDir;
        private string savedToolsDir, savedBaseDir, savedToolName;
        private Settings savedEnv;

        [SetUp]
        public void SetUp()
        {
            savedToolsDir = RegistrySettings.toolsDirectory;
            savedBaseDir = RegistrySettings.baseDirectory;
            savedToolName = RegistrySettings.toolProfileName;
            savedEnv = new Settings();
            CopyUTurn(Settings.Default, savedEnv);

            baseDir = Path.Combine(Path.GetTempPath(), "AgOpenGPS_UTurnProfiles_" + Guid.NewGuid().ToString("N"));
            RegistrySettings.baseDirectory = baseDir;
            RegistrySettings.toolsDirectory = Path.Combine(baseDir, "ToolProfiles");
            Directory.CreateDirectory(RegistrySettings.toolsDirectory);

            // The old global values from environment.xml
            Settings env = Settings.Default;
            env.set_youTurnDistanceFromBoundary = 7.5;
            env.set_youSkipWidth = 3;
            env.set_youTurnRadius = 11.2;
            env.set_youTurnExtensionLength = 35;
            env.set_uTurnStyle = 2;
            env.setAS_uTurnSmoothing = 18;
            env.setAS_uTurnCompensation = 1.4;
        }

        [TearDown]
        public void TearDown()
        {
            RegistrySettings.toolsDirectory = savedToolsDir;
            RegistrySettings.baseDirectory = savedBaseDir;
            RegistrySettings.toolProfileName = savedToolName;
            CopyUTurn(savedEnv, Settings.Default);
            Directory.Delete(baseDir, true);
        }

        private static void CopyUTurn(Settings from, Settings to)
        {
            to.set_youTurnDistanceFromBoundary = from.set_youTurnDistanceFromBoundary;
            to.set_youSkipWidth = from.set_youSkipWidth;
            to.set_youTurnRadius = from.set_youTurnRadius;
            to.set_youTurnExtensionLength = from.set_youTurnExtensionLength;
            to.set_uTurnStyle = from.set_uTurnStyle;
            to.setAS_uTurnSmoothing = from.setAS_uTurnSmoothing;
            to.setAS_uTurnCompensation = from.setAS_uTurnCompensation;
        }

        private static void WriteToolProfile(string name, params (string Name, string Value)[] settings)
        {
            var xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?><configuration><userSettings><AgOpenGPS.Properties.ToolSettings>";
            foreach (var (key, value) in settings)
                xml += "<setting name=\"" + key + "\" serializeAs=\"String\"><value>" + value + "</value></setting>";
            xml += "</AgOpenGPS.Properties.ToolSettings></userSettings></configuration>";
            File.WriteAllText(Path.Combine(RegistrySettings.toolsDirectory, name + ".xml"), xml);
        }

        [Test]
        public void OldToolProfileGetsEnvironmentValuesNotPreviousProfiles()
        {
            WriteToolProfile("Plough", ("setVehicle_toolWidth", "3"),
                ("set_youTurnDistanceFromBoundary", "4.25"), ("set_youSkipWidth", "2"), ("set_youTurnRadius", "6.5"),
                ("set_youTurnExtensionLength", "15"), ("set_uTurnStyle", "1"),
                ("setAS_uTurnSmoothing", "10"), ("setAS_uTurnCompensation", "0.8"));
            WriteToolProfile("Old", ("setVehicle_toolWidth", "6"));

            var tool = new ToolSettings();
            tool.Load("Plough");
            Assert.That(tool.set_youTurnDistanceFromBoundary, Is.EqualTo(4.25));
            Assert.That(tool.set_youSkipWidth, Is.EqualTo(2));
            Assert.That(tool.set_youTurnRadius, Is.EqualTo(6.5));
            Assert.That(tool.set_youTurnExtensionLength, Is.EqualTo(15));
            Assert.That(tool.set_uTurnStyle, Is.EqualTo(1));
            Assert.That(tool.setAS_uTurnSmoothing, Is.EqualTo(10));
            Assert.That(tool.setAS_uTurnCompensation, Is.EqualTo(0.8));

            tool.Load("Old");
            Assert.That(tool.setVehicle_toolWidth, Is.EqualTo(6));
            Assert.That(tool.set_youTurnDistanceFromBoundary, Is.EqualTo(7.5));
            Assert.That(tool.set_youSkipWidth, Is.EqualTo(3));
            Assert.That(tool.set_youTurnRadius, Is.EqualTo(11.2));
            Assert.That(tool.set_youTurnExtensionLength, Is.EqualTo(35));
            Assert.That(tool.set_uTurnStyle, Is.EqualTo(2));
            Assert.That(tool.setAS_uTurnSmoothing, Is.EqualTo(18));
            Assert.That(tool.setAS_uTurnCompensation, Is.EqualTo(1.4));
        }

        [Test]
        public void EachToolProfileKeepsItsOwnValuesAfterSave()
        {
            var tool = new ToolSettings();
            tool.set_youTurnDistanceFromBoundary = 1.5;
            tool.set_youTurnRadius = 6;
            tool.Save("A");
            tool.set_youTurnDistanceFromBoundary = 9;
            tool.set_youTurnRadius = 12;
            tool.Save("B");

            tool.Load("A");
            Assert.That(tool.set_youTurnDistanceFromBoundary, Is.EqualTo(1.5));
            Assert.That(tool.set_youTurnRadius, Is.EqualTo(6));
            tool.Load("B");
            Assert.That(tool.set_youTurnDistanceFromBoundary, Is.EqualTo(9));
            Assert.That(tool.set_youTurnRadius, Is.EqualTo(12));
        }
    }
}

using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AgLibrary.Settings;
using NUnit.Framework;

namespace AgLibrary.Tests.Settings
{
    public class XmlSettingsHandlerTests
    {
        [Test]
        public void LoadXMLFile_ShouldLoadSuccessfully()
        {
            // Arrange
            var filePath = Path.Combine(TestContext.CurrentContext.TestDirectory, "Settings", "TestSettings.xml");
            var testSettings = new TestSettings();

            // Act
            var loadResult = XmlSettingsHandler.LoadXMLFile(filePath, testSettings);

            // Assert
            Assert.That(loadResult, Is.EqualTo(LoadResult.Ok));
            Assert.That(testSettings.StringSetting, Is.EqualTo("Some text"));
            Assert.That(testSettings.EnumSetting, Is.EqualTo(TestEnum.Two));
            Assert.That(testSettings.BoolSetting, Is.True);
            Assert.That(testSettings.IntSetting, Is.EqualTo(42));
            Assert.That(testSettings.DoubleSetting, Is.EqualTo(1.23));
            Assert.That(testSettings.ColorSetting, Is.EqualTo(Color.FromArgb(0, 128, 255)));
            Assert.That(testSettings.PointSetting, Is.EqualTo(new Point(33, 66)));
            Assert.That(testSettings.SizeSetting, Is.EqualTo(new Size(600, 480)));
            Assert.That(testSettings.ListSetting, Is.EquivalentTo(new List<int> { 1, 2, 3, 4, 5 }));
            Assert.That(testSettings.ClassSetting.Id, Is.EqualTo(10));
            Assert.That(testSettings.ClassSetting.Name, Is.EqualTo("My setting"));
        }

        [Test]
        public void LoadXMLFile_ShouldReturnMissingFile()
        {
            // Arrange
            var filePath = @"C:\Path\To\Nonexisting\Settings.xml";

            // Act
            var loadResult = XmlSettingsHandler.LoadXMLFile(filePath, null);

            // Assert
            Assert.That(loadResult, Is.EqualTo(LoadResult.MissingFile));
        }

        [Test]
        public void SaveXMLFile_ShouldSaveWithoutException()
        {
            // Arrange
            var filePath = Path.GetTempFileName();
            var testSettings = new TestSettings
            {
                StringSetting = "Some text",
                EnumSetting = TestEnum.Two,
                BoolSetting = true,
                IntSetting = 42,
                DoubleSetting = 1.23,
                ColorSetting = Color.FromArgb(0, 128, 255),
                PointSetting = new Point(33, 66),
                SizeSetting = new Size(600, 480),
                ListSetting = new List<int> { 1, 2, 3, 4, 5 },
                ClassSetting = new TestSubSettings
                {
                    Id = 10,
                    Name = "My setting"
                }
            };

            // Act
            XmlSettingsHandler.SaveXMLFile(filePath, testSettings);

            // Assert
            var actual = File.ReadAllText(filePath);
            var expected = File.ReadAllText(Path.Combine(TestContext.CurrentContext.TestDirectory, "Settings", "TestSettings.xml"));
            Assert.That(actual, Is.EqualTo(expected));
        }
    }

    public enum TestEnum { One, Two, Three }

    public class TestSubSettings
    {
        public int Id;
        public string Name;
    }

    public class TestSettings
    {
        public string StringSetting;
        public TestEnum EnumSetting;
        public bool BoolSetting;
        public int IntSetting;
        public double DoubleSetting;
        public Color ColorSetting;
        public Point PointSetting;
        public Size SizeSetting;
        public List<int> ListSetting;
        public TestSubSettings ClassSetting;
    }
}
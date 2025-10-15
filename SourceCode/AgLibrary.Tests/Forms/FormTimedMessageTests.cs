using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgLibrary.Forms;
using NUnit.Framework;

namespace AgLibrary.Tests.Forms
{
    /// <summary>
    /// Tests for FormTimedMessage that was consolidated from GPS/AgIO/ModSim into AgLibrary.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class FormTimedMessageTests
    {
        [SetUp]
        public void SetUp()
        {
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Assert.Inconclusive("Tests must run on STA thread for WinForms");
            }
        }

        /// <summary>
        /// Test that FormTimedMessage sets title and message text correctly.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormTimedMessage_SetsTextCorrectly()
        {
            using (var form = new FormTimedMessage(TimeSpan.FromSeconds(5), "Test Title", "Test message"))
            {
                var lblTitle = FindControlRecursive<Label>(form, "lblTitle");
                var lblMessage = FindControlRecursive<Label>(form, "lblMessage");

                Assert.That(lblTitle.Text, Is.EqualTo("Test Title"));
                Assert.That(lblMessage.Text, Is.EqualTo("Test message"));
            }
        }

        /// <summary>
        /// Test that FormTimedMessage calculates width dynamically.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormTimedMessage_CalculatesDynamicWidth()
        {
            using (var shortForm = new FormTimedMessage(TimeSpan.FromSeconds(1), "Title", "Short"))
            using (var longForm = new FormTimedMessage(TimeSpan.FromSeconds(1), "Title",
                "This is a much longer message that should make the form wider"))
            {
                Assert.That(longForm.Width, Is.GreaterThan(shortForm.Width),
                    "Longer message should result in wider form");
            }
        }

        private T FindControlRecursive<T>(Control parent, string name) where T : Control
        {
            foreach (Control control in parent.Controls)
            {
                if (control is T && control.Name == name)
                {
                    return control as T;
                }

                var found = FindControlRecursive<T>(control, name);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
    }
}

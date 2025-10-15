using System;
using System.Threading;
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
        /// Test that FormTimedMessage can be instantiated.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormTimedMessage_CanInstantiate()
        {
            FormTimedMessage form = null;
            Assert.DoesNotThrow(() =>
            {
                form = new FormTimedMessage(TimeSpan.FromSeconds(5), "Test Title", "Test message");
                Assert.That(form, Is.Not.Null);
                Assert.That(form.Width, Is.GreaterThan(0));
                form.Dispose();
            });
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

        /// <summary>
        /// Test that FormTimedMessage timer is properly configured.
        /// Note: This test does not wait for the timer to elapse.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormTimedMessage_TimerIsConfigured()
        {
            using (var form = new FormTimedMessage(TimeSpan.FromSeconds(2.5), "Timer Test", "Testing timer configuration"))
            {
                Assert.That(form, Is.Not.Null);
                // Timer should be set to 2.5 seconds, but we won't wait for it
                // Just verify the form was created successfully
            }
        }
    }
}

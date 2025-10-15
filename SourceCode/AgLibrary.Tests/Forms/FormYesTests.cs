using System;
using System.Threading;
using System.Windows.Forms;
using AgLibrary.Forms;
using NUnit.Framework;

namespace AgLibrary.Tests.Forms
{
    /// <summary>
    /// Tests for FormYes that was consolidated from GPS/AgIO/ModSim into AgLibrary.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class FormYesTests
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
        /// Test that FormYes can be instantiated with default parameters (no cancel button).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_CanInstantiate_WithoutCancelButton()
        {
            FormYes form = null;
            Assert.DoesNotThrow(() =>
            {
                form = new FormYes("This is a test message");
                Assert.That(form, Is.Not.Null);
                Assert.That(form.Width, Is.GreaterThan(0));
                form.Dispose();
            });
        }

        /// <summary>
        /// Test that FormYes can be instantiated with cancel button visible.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_CanInstantiate_WithCancelButton()
        {
            FormYes form = null;
            Assert.DoesNotThrow(() =>
            {
                form = new FormYes("This is a test message with cancel", showCancel: true);
                Assert.That(form, Is.Not.Null);
                Assert.That(form.Width, Is.GreaterThan(0));
                form.Dispose();
            });
        }

        /// <summary>
        /// Test that FormYes calculates width dynamically based on message length.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_CalculatesDynamicWidth()
        {
            using (var shortForm = new FormYes("Short"))
            using (var longForm = new FormYes("This is a much longer message that should make the form wider"))
            {
                Assert.That(longForm.Width, Is.GreaterThan(shortForm.Width),
                    "Longer message should result in wider form");
            }
        }
    }
}

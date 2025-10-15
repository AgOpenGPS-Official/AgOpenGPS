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
        /// Test that FormYes sets message text correctly.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_SetsMessageText()
        {
            using (var form = new FormYes("This is a test message"))
            {
                var lblMessage = FindControlRecursive<Label>(form, "lblMessage");
                Assert.That(lblMessage.Text, Is.EqualTo("This is a test message"));
            }
        }

        /// <summary>
        /// Test that FormYes shows/hides cancel button based on parameter.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_CancelButtonVisibility()
        {
            using (var formWithoutCancel = new FormYes("Test", showCancel: false))
            using (var formWithCancel = new FormYes("Test", showCancel: true))
            {
                // Need to show the form for controls to be fully initialized
                formWithoutCancel.Show();
                formWithCancel.Show();
                Application.DoEvents();

                var btnCancelWithout = FindControlRecursive<Button>(formWithoutCancel, "btnCancel");
                var btnCancelWith = FindControlRecursive<Button>(formWithCancel, "btnCancel");

                Assert.That(btnCancelWithout, Is.Not.Null, "Cancel button should exist");
                Assert.That(btnCancelWith, Is.Not.Null, "Cancel button should exist");
                Assert.That(btnCancelWithout.Visible, Is.False, "Cancel button should be hidden when showCancel=false");
                Assert.That(btnCancelWith.Visible, Is.True, "Cancel button should be visible when showCancel=true");

                formWithoutCancel.Close();
                formWithCancel.Close();
            }
        }

        /// <summary>
        /// Test that FormYes shows/hides title based on parameter.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_TitleVisibility()
        {
            using (var formWithoutTitle = new FormYes("Test message"))
            using (var formWithTitle = new FormYes("Test message", false, "Test Title"))
            {
                // Need to show the form for controls to be fully initialized
                formWithoutTitle.Show();
                formWithTitle.Show();
                Application.DoEvents();

                var lblTitleWithout = FindControlRecursive<Label>(formWithoutTitle, "lblTitle");
                var lblTitleWith = FindControlRecursive<Label>(formWithTitle, "lblTitle");

                Assert.That(lblTitleWithout, Is.Not.Null, "Title label should exist");
                Assert.That(lblTitleWith, Is.Not.Null, "Title label should exist");
                Assert.That(lblTitleWithout.Visible, Is.False, "Title should be hidden when not provided");
                Assert.That(lblTitleWith.Visible, Is.True, "Title should be visible when provided");
                Assert.That(lblTitleWith.Text, Is.EqualTo("Test Title"), "Title text should match");

                formWithoutTitle.Close();
                formWithTitle.Close();
            }
        }

        /// <summary>
        /// Test that FormYes sets AcceptButton and CancelButton correctly.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("ConsolidatedForms")]
        public void FormYes_AcceptAndCancelButtons()
        {
            using (var formWithoutCancel = new FormYes("Test", showCancel: false))
            using (var formWithCancel = new FormYes("Test", showCancel: true))
            {
                var btnOkWithout = FindControlRecursive<Button>(formWithoutCancel, "btnSerialOK");
                var btnOkWith = FindControlRecursive<Button>(formWithCancel, "btnSerialOK");
                var btnCancel = FindControlRecursive<Button>(formWithCancel, "btnCancel");

                Assert.That(formWithoutCancel.AcceptButton, Is.EqualTo(btnOkWithout), "AcceptButton should be OK button");
                Assert.That(formWithCancel.AcceptButton, Is.EqualTo(btnOkWith), "AcceptButton should be OK button");
                Assert.That(formWithCancel.CancelButton, Is.EqualTo(btnCancel), "CancelButton should be set when showCancel=true");
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

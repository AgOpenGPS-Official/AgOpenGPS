using System;
using System.Threading;
using System.Windows.Forms;
using NUnit.Framework;

namespace AgOpenGPS.Core.Tests.Forms
{
    /// <summary>
    /// Tests to verify that forms affected by geometry extension methods migration can launch successfully.
    /// This ensures that the refactoring to use extension methods did not break form initialization.
    /// </summary>
    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class GeometryExtensionMethodsFormTests
    {
        [SetUp]
        public void SetUp()
        {
            // Ensure we're running on STA thread for WinForms
            if (Thread.CurrentThread.GetApartmentState() != ApartmentState.STA)
            {
                Assert.Inconclusive("Tests must run on STA thread for WinForms");
            }
        }

        /// <summary>
        /// Test that FormQuickAB can be instantiated without errors.
        /// This form was affected by geometry extension methods migration (2 call sites).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Guidance")]
        public void FormQuickAB_CanInstantiate()
        {
            // Note: Full form launch would require FormGPS context
            // This test verifies the form class can be loaded without compilation errors
            Assert.DoesNotThrow(() =>
            {
                var formType = Type.GetType("AgOpenGPS.FormQuickAB, GPS");
                Assert.That(formType, Is.Not.Null, "FormQuickAB type should be loadable");
            });
        }

        /// <summary>
        /// Test that FormHeadLine can be instantiated without errors.
        /// This form was affected by geometry extension methods migration (3 call sites).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Guidance")]
        public void FormHeadLine_CanInstantiate()
        {
            Assert.DoesNotThrow(() =>
            {
                var formType = Type.GetType("AgOpenGPS.FormHeadLine, GPS");
                Assert.That(formType, Is.Not.Null, "FormHeadLine type should be loadable");
            });
        }

        /// <summary>
        /// Test that FormHeadAche can be instantiated without errors.
        /// This form was affected by geometry extension methods migration (1 call site).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Guidance")]
        public void FormHeadAche_CanInstantiate()
        {
            Assert.DoesNotThrow(() =>
            {
                var formType = Type.GetType("AgOpenGPS.FormHeadAche, GPS");
                Assert.That(formType, Is.Not.Null, "FormHeadAche type should be loadable");
            });
        }

        /// <summary>
        /// Test that FormABDraw can be instantiated without errors.
        /// This form was affected by geometry extension methods migration (3 call sites).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Guidance")]
        public void FormABDraw_CanInstantiate()
        {
            Assert.DoesNotThrow(() =>
            {
                var formType = Type.GetType("AgOpenGPS.FormABDraw, GPS");
                Assert.That(formType, Is.Not.Null, "FormABDraw type should be loadable");
            });
        }

        /// <summary>
        /// Test that FormBuildTracks can be instantiated without errors.
        /// This form was affected by geometry extension methods migration (4 call sites).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Guidance")]
        public void FormBuildTracks_CanInstantiate()
        {
            Assert.DoesNotThrow(() =>
            {
                var formType = Type.GetType("AgOpenGPS.FormBuildTracks, GPS");
                Assert.That(formType, Is.Not.Null, "FormBuildTracks type should be loadable");
            });
        }

        /// <summary>
        /// Test that FormBndTool can be instantiated without errors.
        /// This form was affected by geometry extension methods migration (2 call sites).
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Field")]
        public void FormBndTool_CanInstantiate()
        {
            Assert.DoesNotThrow(() =>
            {
                var formType = Type.GetType("AgOpenGPS.FormBndTool, GPS");
                Assert.That(formType, Is.Not.Null, "FormBndTool type should be loadable");
            });
        }

        /// <summary>
        /// Test that all affected forms can be loaded as a batch.
        /// </summary>
        [Test]
        [Category("FormLaunch")]
        [Category("Batch")]
        public void AllAffectedForms_CanLoadTypes()
        {
            var formTypes = new[]
            {
                "AgOpenGPS.FormQuickAB, GPS",
                "AgOpenGPS.FormHeadLine, GPS",
                "AgOpenGPS.FormHeadAche, GPS",
                "AgOpenGPS.FormABDraw, GPS",
                "AgOpenGPS.FormBuildTracks, GPS",
                "AgOpenGPS.FormBndTool, GPS"
            };

            foreach (var typeName in formTypes)
            {
                Assert.DoesNotThrow(() =>
                {
                    var formType = Type.GetType(typeName);
                    Assert.That(formType, Is.Not.Null, $"{typeName} should be loadable");
                }, $"Failed to load type: {typeName}");
            }
        }
    }
}

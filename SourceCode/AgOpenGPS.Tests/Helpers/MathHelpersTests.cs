using AgOpenGPS.Helpers;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace AgOpenGPS.Tests.Helpers
{
    [TestFixture]
    public class MathHelpersTests
    {
        #region CalculateMedian Tests

        [Test]
        public void CalculateMedian_EmptyList_ReturnsZero()
        {
            // Arrange
            var data = new List<double>();

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateMedian_NullList_ReturnsZero()
        {
            // Arrange
            List<double> data = null;

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateMedian_SingleValue_ReturnsThatValue()
        {
            // Arrange
            var data = new List<double> { 5.0 };

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void CalculateMedian_OddNumberOfValues_ReturnsMiddleValue()
        {
            // Arrange
            var data = new List<double> { 1.0, 3.0, 7.0, 9.0, 5.0 };

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(5.0));
        }

        [Test]
        public void CalculateMedian_EvenNumberOfValues_ReturnsAverage()
        {
            // Arrange
            var data = new List<double> { 2.6, 4.2, 4.9, 2.1, 2.2, 2.4 };

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(2.5));
        }

        [Test]
        public void CalculateMedian_NegativeValues_ReturnsCorrectMedian()
        {
            // Arrange
            var data = new List<double> { -5.0, -1.0, 1.0, 0.0, 5.0 };

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateMedian_DuplicateValues_ReturnsCorrectMedian()
        {
            // Arrange
            var data = new List<double> { 3.0, 3.0, 3.0, 3.0, 3.0 };

            // Act
            double result = MathHelpers.CalculateMedian(data);

            // Assert
            Assert.That(result, Is.EqualTo(3.0));
        }

        #endregion

        #region CalculateStandardDeviation Tests

        [Test]
        public void CalculateStandardDeviation_EmptyList_ReturnsZero()
        {
            // Arrange
            var data = new List<double>();
            double mean = 0.0;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateStandardDeviation_NullList_ReturnsZero()
        {
            // Arrange
            List<double> data = null;
            double mean = 0.0;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateStandardDeviation_SingleValue_ReturnsZero()
        {
            // Arrange
            var data = new List<double> { 5.0 };
            double mean = 5.0;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateStandardDeviation_IdenticalValues_ReturnsZero()
        {
            // Arrange
            var data = new List<double> { 5.0, 5.0, 5.0, 5.0 };
            double mean = 5.0;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            Assert.That(result, Is.EqualTo(0.0));
        }

        [Test]
        public void CalculateStandardDeviation_SimpleDataSet_ReturnsCorrectValue()
        {
            // Arrange
            var data = new List<double> { 2.0, 4.0, 4.0, 4.0, 5.0, 5.0, 7.0, 9.0 };
            double mean = 5.0;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            // Expected std dev = sqrt(sum((x-mean)^2)/(n-1))
            // = sqrt((9+1+1+1+0+0+4+16)/7) = sqrt(32/7) ≈ 2.138
            Assert.That(result, Is.EqualTo(2.138).Within(0.001));
        }

        [Test]
        public void CalculateStandardDeviation_NegativeValues_ReturnsCorrectValue()
        {
            // Arrange
            var data = new List<double> { -2.0, -1.0, 0.0, 1.0, 2.0 };
            double mean = 0.0;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            // Expected std dev = sqrt((4+1+0+1+4)/4) = sqrt(10/4) ≈ 1.581
            Assert.That(result, Is.EqualTo(1.581).Within(0.001));
        }

        [Test]
        public void CalculateStandardDeviation_LargeSpread_ReturnsCorrectValue()
        {
            // Arrange
            var data = new List<double> { 1.0, 100.0 };
            double mean = 50.5;

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            // Expected std dev = sqrt((2450.25+2450.25)/1) = sqrt(4900.5) ≈ 70.0
            Assert.That(result, Is.EqualTo(70.0).Within(0.1));
        }

        [Test]
        public void CalculateStandardDeviation_RealWorldWASData_ReturnsCorrectValue()
        {
            // Arrange - Simulated WAS calibration data (steering angles in degrees)
            var data = new List<double> { -2.1, -1.8, -2.0, -1.9, -2.2, -2.0, -1.7, -2.1, -1.9, -2.0 };
            double mean = -1.97; // Approximately

            // Act
            double result = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            // Small standard deviation indicates consistent steering
            Assert.That(result, Is.LessThan(0.2)); // Should be ~0.15
            Assert.That(result, Is.GreaterThan(0.0));
        }

        #endregion

        #region Integration Tests

        [Test]
        public void MedianAndStdDev_WorkTogetherCorrectly()
        {
            // Arrange - WAS calibration scenario
            var steerAngles = new List<double> { -2.5, -2.0, -1.5, -2.0, -2.5, -2.0, -1.5, -2.0 };

            // Act
            double median = MathHelpers.CalculateMedian(steerAngles);
            double mean = steerAngles.Sum() / steerAngles.Count;
            double stdDev = MathHelpers.CalculateStandardDeviation(steerAngles, mean);

            // Assert
            Assert.That(median, Is.EqualTo(-2.0));
            Assert.That(stdDev, Is.GreaterThan(0.0));
            Assert.That(stdDev, Is.LessThan(1.0)); // Reasonable steering consistency
        }

        [Test]
        public void MedianAndStdDev_HandleOutliers()
        {
            // Arrange - Data with outlier (sensor glitch)
            var data = new List<double> { 2.0, 2.1, 2.0, 2.1, 2.0, 15.0 }; // 15.0 is outlier

            // Act
            double median = MathHelpers.CalculateMedian(data);
            double mean = data.Sum() / data.Count;
            double stdDev = MathHelpers.CalculateStandardDeviation(data, mean);

            // Assert
            // Median should be less affected by outlier than mean
            Assert.That(median, Is.EqualTo(2.05).Within(0.01));
            Assert.That(mean, Is.GreaterThan(median)); // Mean pulled up by outlier
            Assert.That(stdDev, Is.GreaterThan(3.0)); // High std dev due to outlier
        }

        #endregion
    }
}

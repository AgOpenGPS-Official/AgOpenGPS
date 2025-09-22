using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AgOpenGPS
{
    /// <summary>
    /// Smart WAS Calibration system that collects and analyzes guidance line steer angle data
    /// to determine the optimal WAS zero point based on statistical distribution analysis
    /// </summary>
    public class CSmartWASCalibration
    {
        #region Properties and Fields

        private readonly FormGPS mf;
        private readonly List<double> steerAngleHistory;
        private readonly object lockObject = new object();

        // Data collection settings
        private const int MAX_SAMPLES = 2000;  // Maximum number of samples to keep
        private const int MIN_SAMPLES_FOR_ANALYSIS = 200;  // Minimum samples for reliable analysis
        private const double MIN_SPEED_THRESHOLD = 2.0;  // km/h - minimum speed for data collection
        private const double MAX_ANGLE_THRESHOLD = 25.0;  // degrees - maximum angle to consider valid

        // Analysis parameters
        private const double NORMAL_DISTRIBUTION_THRESHOLD = 0.8;  // How much of data should be within 1 std dev

        // Public properties for UI updates
        public bool IsCollectingData { get; private set; }
        public int SampleCount { get; private set; }
        public double RecommendedWASZero { get; private set; }
        public double ConfidenceLevel { get; private set; }
        public bool HasValidRecommendation { get; private set; }
        public DateTime LastCollectionTime { get; private set; }

        // Statistics
        public double Mean { get; private set; }
        public double StandardDeviation { get; private set; }
        public double Median { get; private set; }

        #endregion

        #region Constructor

        public CSmartWASCalibration(FormGPS formGPS)
        {
            mf = formGPS;
            steerAngleHistory = new List<double>();
            ResetData();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Start collecting steer angle data
        /// </summary>
        public void StartDataCollection()
        {
            lock (lockObject)
            {
                IsCollectingData = true;
                LastCollectionTime = DateTime.Now;
            }
        }

        /// <summary>
        /// Stop collecting steer angle data
        /// </summary>
        public void StopDataCollection()
        {
            lock (lockObject)
            {
                IsCollectingData = false;
            }
        }

        /// <summary>
        /// Clear all collected data and reset analysis
        /// </summary>
        public void ResetData()
        {
            lock (lockObject)
            {
                steerAngleHistory.Clear();
                SampleCount = 0;
                RecommendedWASZero = 0;
                ConfidenceLevel = 0;
                HasValidRecommendation = false;
                Mean = 0;
                StandardDeviation = 0;
                Median = 0;
            }
        }

        /// <summary>
        /// Apply an offset to all collected data to account for WAS zero changes
        /// This prevents the offset from being applied multiple times
        /// </summary>
        /// <param name="appliedOffsetDegrees">The offset that was applied to the WAS zero in degrees</param>
        public void ApplyOffsetToCollectedData(double appliedOffsetDegrees)
        {
            lock (lockObject)
            {
                if (steerAngleHistory.Count == 0) return;

                // Apply the offset to all collected samples
                for (int i = 0; i < steerAngleHistory.Count; i++)
                {
                    steerAngleHistory[i] += appliedOffsetDegrees;
                }

                // Recalculate statistics with the adjusted data
                if (SampleCount >= MIN_SAMPLES_FOR_ANALYSIS)
                {
                    PerformStatisticalAnalysis();
                }

                AgLibrary.Logging.Log.EventWriter($"Smart WAS: Applied {appliedOffsetDegrees:F2}° offset to {steerAngleHistory.Count} collected samples");
            }
        }

        /// <summary>
        /// Add a new steer angle measurement to the collection
        /// Called from the main GPS update loop
        /// </summary>
        /// <param name="guidanceSteerAngle">Current guidance line steer angle in degrees</param>
        /// <param name="currentSpeed">Current vehicle speed in km/h</param>
        public void AddSteerAngleSample(double guidanceSteerAngle, double currentSpeed)
        {
            if (!IsCollectingData) return;

            // Check if we should collect this sample
            if (!ShouldCollectSample(guidanceSteerAngle, currentSpeed)) return;

            lock (lockObject)
            {
                // Add the sample
                steerAngleHistory.Add(guidanceSteerAngle);
                LastCollectionTime = DateTime.Now;

                // Limit the number of samples to prevent memory issues
                if (steerAngleHistory.Count > MAX_SAMPLES)
                {
                    steerAngleHistory.RemoveAt(0);  // Remove oldest sample
                }

                SampleCount = steerAngleHistory.Count;

                // Perform analysis if we have enough samples
                if (SampleCount >= MIN_SAMPLES_FOR_ANALYSIS)
                {
                    PerformStatisticalAnalysis();
                }
            }
        }

        /// <summary>
        /// Get the recommended WAS offset adjustment based on collected data
        /// </summary>
        /// <param name="currentCPD">Current counts per degree setting</param>
        /// <returns>Recommended offset adjustment in counts</returns>
        public int GetRecommendedWASOffsetAdjustment(int currentCPD)
        {
            if (!HasValidRecommendation) return 0;

            // Convert degrees to counts
            return (int)Math.Round(RecommendedWASZero * currentCPD);
        }

        /// <summary>
        /// Get a detailed analysis report for debugging/logging
        /// </summary>
        /// <returns>String containing analysis details</returns>
        public string GetAnalysisReport()
        {
            if (SampleCount == 0) return "No data collected yet.";

            var report = $"Smart WAS Calibration Analysis Report:\n" +
                        $"Samples Collected: {SampleCount}\n" +
                        $"Mean Angle: {Mean:F3}°\n" +
                        $"Median Angle: {Median:F3}°\n" +
                        $"Std Deviation: {StandardDeviation:F3}°\n" +
                        $"Recommended WAS Zero: {RecommendedWASZero:F3}°\n" +
                        $"Confidence Level: {ConfidenceLevel:F1}%\n" +
                        $"Valid Recommendation: {HasValidRecommendation}";

            return report;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determine if a sample should be collected based on quality criteria
        /// </summary>
        private bool ShouldCollectSample(double steerAngle, double speed)
        {
            // Only collect data when moving at reasonable speed
            if (speed < MIN_SPEED_THRESHOLD) return false;

            // Ignore extreme angles that might be outliers
            if (Math.Abs(steerAngle) > MAX_ANGLE_THRESHOLD) return false;

            // Only collect when autosteer is active and we have valid guidance
            if (!mf.isBtnAutoSteerOn) return false;
            if (Math.Abs(mf.guidanceLineDistanceOff) > 15000) return false;  // > 15m off line

            return true;
        }

        /// <summary>
        /// Perform statistical analysis on collected data to determine optimal WAS zero
        /// </summary>
        private void PerformStatisticalAnalysis()
        {
            if (steerAngleHistory.Count < MIN_SAMPLES_FOR_ANALYSIS) return;

            try
            {
                // Calculate basic statistics
                var sortedData = steerAngleHistory.OrderBy(x => x).ToList();
                Mean = steerAngleHistory.Average();
                Median = CalculateMedian(sortedData);
                StandardDeviation = CalculateStandardDeviation(steerAngleHistory, Mean);

                // Determine the recommended zero point
                // For a well-calibrated system, the distribution should be centered around zero
                // We use the median as it's more robust to outliers than the mean
                RecommendedWASZero = -Median;  // Negative because we want to adjust to center at zero

                // Calculate confidence level based on data distribution
                CalculateConfidenceLevel(sortedData);

                // Determine if we have a valid recommendation
                HasValidRecommendation = ConfidenceLevel > 50.0 && SampleCount >= MIN_SAMPLES_FOR_ANALYSIS;
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                AgLibrary.Logging.Log.EventWriter($"Error in Smart WAS analysis: {ex.Message}");
                HasValidRecommendation = false;
            }
        }

        /// <summary>
        /// Calculate median value from sorted data
        /// </summary>
        private double CalculateMedian(List<double> sortedData)
        {
            int count = sortedData.Count;
            if (count == 0) return 0;

            if (count % 2 == 0)
            {
                return (sortedData[count / 2 - 1] + sortedData[count / 2]) / 2.0;
            }
            else
            {
                return sortedData[count / 2];
            }
        }

        /// <summary>
        /// Calculate standard deviation
        /// </summary>
        private double CalculateStandardDeviation(List<double> data, double mean)
        {
            if (data.Count < 2) return 0;

            double sumSquaredDifferences = data.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(sumSquaredDifferences / (data.Count - 1));
        }

        /// <summary>
        /// Calculate confidence level based on how well the data fits a normal distribution
        /// centered around the guidance line
        /// </summary>
        private void CalculateConfidenceLevel(List<double> sortedData)
        {
            if (sortedData.Count < MIN_SAMPLES_FOR_ANALYSIS)
            {
                ConfidenceLevel = 0;
                return;
            }

            // Check how much of the data falls within reasonable bounds
            double oneStdDevRange = StandardDeviation;
            double twoStdDevRange = 2 * StandardDeviation;

            // Count samples within 1 and 2 standard deviations of the median
            int withinOneStdDev = 0;
            int withinTwoStdDev = 0;

            foreach (double angle in sortedData)
            {
                double deviationFromMedian = Math.Abs(angle - Median);
                if (deviationFromMedian <= oneStdDevRange) withinOneStdDev++;
                if (deviationFromMedian <= twoStdDevRange) withinTwoStdDev++;
            }

            double oneStdDevPercentage = (double)withinOneStdDev / sortedData.Count;
            double twoStdDevPercentage = (double)withinTwoStdDev / sortedData.Count;

            // For a normal distribution, ~68% should be within 1 std dev, ~95% within 2 std dev
            // Calculate confidence based on how close we are to these expected values
            double expectedOneStdDev = 0.68;
            double expectedTwoStdDev = 0.95;

            double oneStdDevScore = Math.Max(0, 1 - Math.Abs(oneStdDevPercentage - expectedOneStdDev) / expectedOneStdDev);
            double twoStdDevScore = Math.Max(0, 1 - Math.Abs(twoStdDevPercentage - expectedTwoStdDev) / expectedTwoStdDev);

            // Also consider the magnitude of the recommended adjustment
            // Smaller adjustments get higher confidence (more likely to be correct)
            double magnitudeScore = Math.Max(0, 1 - Math.Abs(RecommendedWASZero) / 10.0);  // Penalize large adjustments

            // Sample size factor - more samples = higher confidence
            double sampleSizeFactor = Math.Min(1.0, (double)sortedData.Count / (MIN_SAMPLES_FOR_ANALYSIS * 3));

            // Combine factors for overall confidence
            ConfidenceLevel = ((oneStdDevScore * 0.3 + twoStdDevScore * 0.3 + magnitudeScore * 0.2 + sampleSizeFactor * 0.2) * 100);
            ConfidenceLevel = Math.Max(0, Math.Min(100, ConfidenceLevel));
        }

        #endregion
    }
}
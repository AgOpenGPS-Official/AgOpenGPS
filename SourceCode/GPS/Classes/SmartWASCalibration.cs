using AgLibrary.Logging;
using AgOpenGPS.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AgOpenGPS
{
    /// <summary>
    /// Smart WAS Calibration system that collects and analyzes guidance line steer angle data
    /// to determine the optimal WAS zero point based on statistical distribution analysis
    /// </summary>
    public class SmartWASCalibration
    {
        #region Properties and Fields

        private readonly FormGPS mf;
        private readonly List<double> steerAngleHistory;
        private readonly object lockObject = new object();

        // Data collection settings
        private const int MAX_SAMPLES = 2000;  // Maximum number of samples to keep
        private const int MIN_SAMPLES_FOR_ANALYSIS = 200;  // Minimum samples for reliable analysis
        private const double MIN_SPEED_THRESHOLD = 2.0;  // km/h - minimum speed for data collection
        private const double MAX_ANGLE_THRESHOLD = 15.0;  // degrees - maximum angle to consider valid
        public const double MAX_DISTANCE_OFF_LINE = 500.0;  // millimeters - maximum distance from guidance line (50cm)

        // Analysis parameters
        private const double NORMAL_DISTRIBUTION_THRESHOLD = 0.8;  // How much of data should be within 1 std dev

        // Public properties for UI updates
        public bool IsCollectingData { get; private set; }
        public int SampleCount => steerAngleHistory.Count;
        public double RecommendedWASZero { get; private set; }
        public double ConfidenceLevel { get; private set; }
        public bool HasValidRecommendation { get; private set; }

        // Statistics
        public double Mean { get; private set; }
        public double StandardDeviation { get; private set; }
        public double Median { get; private set; }

        #endregion

        #region Constructor

        public SmartWASCalibration(FormGPS formGPS)
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
                PerformStatisticalAnalysis();

                Log.EventWriter($"Smart WAS: Applied {appliedOffsetDegrees:F2}° offset to {steerAngleHistory.Count} collected samples");
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
            // Check if we should collect this sample
            if (!ShouldCollectSample(guidanceSteerAngle, currentSpeed)) return;

            lock (lockObject)
            {
                // Add the sample
                steerAngleHistory.Add(guidanceSteerAngle);

                // Limit the number of samples to prevent memory issues
                if (steerAngleHistory.Count > MAX_SAMPLES)
                {
                    steerAngleHistory.RemoveAt(0);  // Remove oldest sample
                }

                // Perform analysis if we have enough samples
                PerformStatisticalAnalysis();
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
        /// Get statistics and analysis results for UI display
        /// </summary>
        public CalibrationStats GetCalibrationStats()
        {
            return new CalibrationStats
            {
                SampleCount = SampleCount,
                Mean = Mean,
                Median = Median,
                StandardDeviation = StandardDeviation,
                RecommendedWASZero = RecommendedWASZero,
                ConfidenceLevel = ConfidenceLevel,
                HasValidRecommendation = HasValidRecommendation
            };
        }

        /// <summary>
        /// Log the current analysis report
        /// </summary>
        public void LogAnalysisReport()
        {
            if (SampleCount == 0)
            {
                Log.EventWriter("Smart WAS: No data collected yet.");
                return;
            }

            Log.EventWriter($"Smart WAS Calibration Analysis Report:");
            Log.EventWriter($"  Samples Collected: {SampleCount}");
            Log.EventWriter($"  Mean Angle: {Mean:F3}°");
            Log.EventWriter($"  Median Angle: {Median:F3}°");
            Log.EventWriter($"  Std Deviation: {StandardDeviation:F3}°");
            Log.EventWriter($"  Recommended WAS Zero: {RecommendedWASZero:F3}°");
            Log.EventWriter($"  Confidence Level: {ConfidenceLevel:F1}%");
            Log.EventWriter($"  Valid Recommendation: {HasValidRecommendation}");
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Determine if a sample should be collected based on quality criteria
        /// </summary>
        private bool ShouldCollectSample(double steerAngle, double speed)
        {
            // Check if collection is active
            if (!IsCollectingData) return false;

            // Only collect data when moving at reasonable speed
            if (speed < MIN_SPEED_THRESHOLD) return false;

            // Ignore extreme angles that might be outliers
            if (Math.Abs(steerAngle) > MAX_ANGLE_THRESHOLD) return false;

            // Only collect when autosteer is active and we have valid guidance
            if (!mf.isBtnAutoSteerOn) return false;
            if (Math.Abs(mf.guidanceLineDistanceOff) > MAX_DISTANCE_OFF_LINE) return false;

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
                Median = MathHelpers.CalculateMedian(steerAngleHistory);
                StandardDeviation = MathHelpers.CalculateStandardDeviation(steerAngleHistory, Mean);

                // Determine the recommended zero point
                // For a well-calibrated system, the distribution should be centered around zero
                // We use the median as it's more robust to outliers than the mean
                RecommendedWASZero = -Median;  // Negative because we want to adjust to center at zero

                // Calculate confidence level based on data distribution
                ConfidenceLevel = CalculateConfidenceLevel(sortedData, StandardDeviation);

                // Determine if we have a valid recommendation
                HasValidRecommendation = ConfidenceLevel > 50.0 && SampleCount >= MIN_SAMPLES_FOR_ANALYSIS;
            }
            catch (Exception ex)
            {
                // Log error but don't crash
                Log.EventWriter($"Error in Smart WAS analysis: {ex.Message}");
                HasValidRecommendation = false;
            }
        }

        /// <summary>
        /// Calculate confidence level based on how well the data fits a normal distribution
        /// centered around the guidance line
        /// </summary>
        private double CalculateConfidenceLevel(List<double> sortedData, double standardDeviation)
        {
            if (sortedData.Count < MIN_SAMPLES_FOR_ANALYSIS)
            {
                return 0;
            }

            // Check how much of the data falls within reasonable bounds
            double oneStdDevRange = standardDeviation;
            double twoStdDevRange = 2 * standardDeviation;

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
            double confidenceLevel = ((oneStdDevScore * 0.3 + twoStdDevScore * 0.3 + magnitudeScore * 0.2 + sampleSizeFactor * 0.2) * 100);
            return Math.Max(0, Math.Min(100, confidenceLevel));
        }

        #endregion
    }

    /// <summary>
    /// Statistics data structure for UI display
    /// </summary>
    public struct CalibrationStats
    {
        public int SampleCount { get; set; }
        public double Mean { get; set; }
        public double Median { get; set; }
        public double StandardDeviation { get; set; }
        public double RecommendedWASZero { get; set; }
        public double ConfidenceLevel { get; set; }
        public bool HasValidRecommendation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace AgOpenGPS.Helpers
{
    /// <summary>
    /// Mathematical helper functions for statistical calculations
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Calculate median value from a list of data
        /// </summary>
        /// <param name="data">Data to calculate median from (will be sorted)</param>
        /// <returns>Median value</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null</exception>
        public static double CalculateMedian(List<double> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Count == 0) return 0;

            var sortedData = data.OrderBy(x => x).ToList();
            int count = sortedData.Count;

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
        /// Calculate mean and standard deviation using sample standard deviation formula
        /// </summary>
        /// <param name="data">Data to calculate statistics from</param>
        /// <returns>Tuple of (mean, standardDeviation)</returns>
        /// <exception cref="ArgumentNullException">Thrown when data is null</exception>
        public static (double mean, double standardDeviation) CalculateMeanAndStandardDeviation(List<double> data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Count < 2) return (0, 0);

            double mean = data.Average();
            double sumSquaredDifferences = data.Sum(x => Math.Pow(x - mean, 2));
            double standardDeviation = Math.Sqrt(sumSquaredDifferences / (data.Count - 1));

            return (mean, standardDeviation);
        }
    }
}

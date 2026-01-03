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
        public static double CalculateMedian(List<double> data)
        {
            if (data == null || data.Count == 0) return 0;

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
        /// Calculate standard deviation using sample standard deviation formula
        /// </summary>
        /// <param name="data">Data to calculate standard deviation from</param>
        /// <param name="mean">Pre-calculated mean value</param>
        /// <returns>Standard deviation</returns>
        public static double CalculateStandardDeviation(List<double> data, double mean)
        {
            if (data == null || data.Count < 2) return 0;

            double sumSquaredDifferences = data.Sum(x => Math.Pow(x - mean, 2));
            return Math.Sqrt(sumSquaredDifferences / (data.Count - 1));
        }
    }
}

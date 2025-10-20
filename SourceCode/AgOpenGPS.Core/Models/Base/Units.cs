using System;

namespace AgOpenGPS.Core.Models
{
    public static class Units
    {
        public static double DegreesToRadians(double degrees)
        {
            const double degreesToRadians = Math.PI / 180.0;
            return degrees * degreesToRadians;
        }

        public static double RadiansToDegrees(double radians)
        {
            const double radiansToDegrees = 180.0 / Math.PI;
            return radians * radiansToDegrees;
        }

    }

    public class Distance
    {
        private const double metersToKilometers = 0.001;
        private const double milesToKilometers = 1.609344;
        private const double kilometersToMiles = 1 / milesToKilometers;
        private const double metersToFeet = 3.28;
        private const double metersToInches = 39.3701;
        private const double metersToCm = 100;
        private const double metersToMiles = metersToKilometers * kilometersToMiles;

        private double _distanceInMeters;
        public Distance(double distanceInMeters)
        {
            _distanceInMeters = distanceInMeters;
        }

        public double InMeters => _distanceInMeters;
        public double InKilometers => _distanceInMeters * metersToKilometers;
        public double InMiles => _distanceInMeters * metersToMiles;
        public double InFeet => _distanceInMeters * metersToFeet;


        // Return the distance (value and unit) in a string expressed in cm or inches, (depending on argument isMetric)
        // Use 0 decimals for cm and 1 decimal for inches.

        public static string VerySmallDistanceString(bool isMetric, double distanceInMeters)
        {
            if (isMetric)
            {
                return (distanceInMeters * metersToCm).ToString("N0") + " cm";
            }
            else
            {
                return (distanceInMeters * metersToInches).ToString("N1") + " in";
            }
        }

        // Return the distance (value and unit) in a string expressed in cm or inches, (depending on argument isMetric)
        // Use 0 decimals for both cm and inches.
        public static string SmallDistanceString(bool isMetric, double distanceInMeters)
        {
            if (isMetric)
            {
                return (distanceInMeters * metersToCm).ToString("N0") + " cm";
            }
            else
            {
                return (distanceInMeters * metersToInches).ToString("N0") + " in";
            }
        }

        // Return the distance (value and unit) in a string expressed in meters or feet and inches, (depending on argument isMetric)
        public static string MediumDistanceString(bool isMetric, double distanceInMeters)
        {
            if (isMetric)
            {
                return distanceInMeters.ToString("N2") + " m";
            }
            else
            {
                int totalInches = Convert.ToInt32(distanceInMeters * metersToInches);
                int feet = totalInches / 12;
                int inches = totalInches - 12 * feet;
                return feet.ToString() + "' " + inches.ToString() + '"';
            }
        }

        public static string MediumBigDistanceString(bool isMetric, double distanceInMeters,
            int decimalsFeet = 2, int decimalsMeters = 1)
        {
            string format = isMetric
                ? "F" + decimalsMeters.ToString()
                : "F" + decimalsFeet.ToString();

            double displayValue = isMetric
                ? distanceInMeters
                : distanceInMeters * metersToFeet;

            string unit = isMetric ? " m" : " ft";
            return displayValue.ToString(format, System.Globalization.CultureInfo.CurrentUICulture) + unit;
        }
    }

    public class Area
    {
        private const double _squareMetersToHectares = 0.0001;
        private const double _squareMetersToAcres = 0.000247105;

        private double _areaInSquareMeters;
        public Area(double areaInSquareMeters)
        {
            _areaInSquareMeters = areaInSquareMeters;
        }

        public double InSquareMeters => _areaInSquareMeters;
        public double InHectares => _squareMetersToHectares * _areaInSquareMeters;
        public double InAcres => _squareMetersToAcres * _areaInSquareMeters;
    }

    public class Speed
    {
        private const double kmhToMph = 0.621371;
        private const double mphToKmh = 1 / kmhToMph;

        private double _speedInKmh;
        public Speed(double speedInKmh)
        {
            _speedInKmh = speedInKmh;
        }

        public double InKmh => _speedInKmh;
        public double InMph => _speedInKmh * kmhToMph;

        public static double KmhToMph(double kmh) => kmh * kmhToMph;
        public static double MphToKmh(double mph) => mph * mphToKmh;
    }

}

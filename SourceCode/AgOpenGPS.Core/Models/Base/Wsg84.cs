namespace AgOpenGPS.Core.Models
{
    // Represents a coordinate in the World Geodetic System 1984
    public struct Wgs84
    {
        Wgs84(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }

        public double Latitude { get; }
        public double Longitude { get; }
    }
}

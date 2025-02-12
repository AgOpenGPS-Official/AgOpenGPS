namespace AgOpenGPS.Core.Models
{
    
    public struct GeoArea
    {
        public GeoArea(double area)
        {
            Area = area;
        }

        public double Area { get; } // Area in Square meters

        public static GeoArea operator +(GeoArea area1, GeoArea area2)
        {
            return new GeoArea(area1.Area + area2.Area);
        }

    }
}

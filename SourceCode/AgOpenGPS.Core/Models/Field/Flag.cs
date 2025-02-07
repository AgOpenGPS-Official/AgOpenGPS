namespace AgOpenGPS.Core.Models
{
    public enum FlagColor { Red = 0, Green = 1, Yellow = 2}

    public class Flag
    {
        public Flag(Wgs84 wgs84, GeoCoord geoCoord, GeoDir heading, FlagColor flagColor, int uniqueNumber, string notes)
        {
            Wgs84 = wgs84;
            GeoCoordDir = new GeoCoordDir(geoCoord, heading);
            FlagColor = flagColor;
            UniqueNumber = uniqueNumber;
            Notes = notes;
        }

        public  Wgs84 Wgs84 { get; }
        public GeoCoordDir GeoCoordDir { get; }

        public double Latitude => Wgs84.Latitude;
        public double Longitude => Wgs84.Longitude;
        public GeoCoord GeoCoord => GeoCoordDir.Coord;
        public GeoDir Heading => GeoCoordDir.Direction;
        public double Northing => GeoCoord.Northing;
        public double Easting => GeoCoord.Easting;

        public FlagColor FlagColor { get; }
        public int UniqueNumber { get; set; }

        public string Notes { get; set;}

    }
}
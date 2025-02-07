namespace AgOpenGPS.Core.Models
{
    public class BoundaryPolygon : GeoPolygonWithHeading
    {
        public BoundaryPolygon()
        {
        }

        public bool IsDriveThru { get; set; }
    }
}

using System.Collections.Generic;

namespace AgOpenGPS.Core.Models
{
    public class RecordedPoint
    {

        public RecordedPoint(GeoCoordDir geoCoordDir, double speed, bool autoButtonState)
        {
            GeoCoordDir = geoCoordDir;
            Speed = speed;
            AutoButtonState = autoButtonState;
        }

        public GeoCoordDir GeoCoordDir { get; set; }
        public double Speed { get; set; }
        public bool AutoButtonState { get; set; }

        public GeoCoord AsGeoCoord => GeoCoordDir.Coord;
    }

}

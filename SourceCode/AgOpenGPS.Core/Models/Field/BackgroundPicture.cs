using System.Drawing;

namespace AgOpenGPS.Core.Models
{
    public class BackgroundPicture
    {
        public BackgroundPicture(bool isGeoMap)
        {
            IsGeoMap = isGeoMap;
        }

        public bool IsGeoMap { get; }
        public GeoBoundingBox GeoBoundingBox { get; set; }
        public Bitmap Picture { get; set; }

    }
}

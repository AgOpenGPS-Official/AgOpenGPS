using AgOpenGPS.Core.Base;
using System.Drawing;

namespace AgOpenGPS.Core.Models
{
    public class BingMap : DisposableObject
    {
        public BingMap(
            GeoBoundingBox geoBoundingBox,
            Bitmap bitmap)
        {
            GeoBoundingBox = geoBoundingBox;
            Bitmap = bitmap;
        }

        public GeoBoundingBox GeoBoundingBox { get; }
        public Bitmap Bitmap { get; }

        protected override void OnDispose()
        {
            Bitmap.Dispose();
        }
    }
}

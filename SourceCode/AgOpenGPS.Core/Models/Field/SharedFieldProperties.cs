namespace AgOpenGPS.Core.Models
{
    public class SharedFieldProperties
    {
        public SharedFieldProperties()
        {
            DriftCompensation = new GeoDelta(0.0, 0.0);
        }

        public GeoDelta DriftCompensation { get; set; }
    }
}
